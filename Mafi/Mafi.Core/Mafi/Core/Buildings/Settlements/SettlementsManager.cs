// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Settlements.SettlementsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Settlements
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class SettlementsManager
  {
    private static readonly Upoints HOMELESS_UNITY_PENALTY_PER_5_PERCENT;
    private static readonly Upoints MAX_HOMELESS_PENALTY;
    public Upoints LastHomelessPenalty;
    private readonly LazyResolve<UpointsManager> m_upointsManager;
    private readonly INotificationsManager m_notificationsManager;
    public readonly IntAvgStats TotalHousingStats;
    public readonly IntAvgStats TotalPopulationStats;
    public readonly Fix32SumStats NewPopsFromAdoptions;
    private readonly Event<int> m_onWorkersRemoved;
    private readonly Event<int> m_onWorkersAdded;
    private Notificator m_homelessNotificator;
    private Notificator m_lowFoodNotif;
    private Notificator m_popsStarvingNotif;
    private readonly Lyst<Settlement> m_settlements;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int LastPopulationDiff { get; private set; }

    public int AmountStarvedToDeathLastMonth { get; private set; }

    public bool ArePeopleStarving { get; private set; }

    public int TotalHousingCapacity { get; private set; }

    public int FreeHousingCapacity => this.TotalHousingCapacity - this.GetTotalPopulation();

    public int NumberOfHomeless { get; private set; }

    public int StarvingHomelessCountDays { get; set; }

    [NewInSaveVersion(140, null, null, null, null)]
    public int NumberOfStarvingPopsWithheld { get; private set; }

    public IEvent<int> OnWorkersRemoved => (IEvent<int>) this.m_onWorkersRemoved;

    public IEvent<int> OnWorkersAdded => (IEvent<int>) this.m_onWorkersAdded;

    public int SettlementsCount => this.m_settlements.Count;

    public int MonthsOfFood { get; private set; }

    public IReadOnlyCollection<Settlement> Settlements
    {
      get => (IReadOnlyCollection<Settlement>) this.m_settlements;
    }

    internal bool IsHomelessNotificationOn => this.m_homelessNotificator.IsActive;

    internal bool IsStarvingNotificationOn => this.m_popsStarvingNotif.IsActive;

    public SettlementsManager(
      LazyResolve<UpointsManager> upointsManager,
      ICalendar calendar,
      StatsManager statsManager,
      ISimLoopEvents simLoopEvents,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.LastHomelessPenalty = Upoints.Zero;
      this.m_onWorkersRemoved = new Event<int>();
      this.m_onWorkersAdded = new Event<int>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMonthsOfFood\u003Ek__BackingField = 99;
      this.m_settlements = new Lyst<Settlement>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_upointsManager = upointsManager;
      this.m_notificationsManager = notificationsManager;
      this.m_homelessNotificator = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.Homeless);
      this.m_lowFoodNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.LowFoodSupply);
      this.m_popsStarvingNotif = notificationsManager.CreateNotificatorFor(IdsCore.Notifications.PopsStarving);
      this.TotalHousingStats = new IntAvgStats((Option<StatsManager>) statsManager);
      this.TotalPopulationStats = new IntAvgStats((Option<StatsManager>) statsManager);
      this.NewPopsFromAdoptions = new Fix32SumStats((Option<StatsManager>) statsManager, true);
      calendar.NewMonth.Add<SettlementsManager>(this, new Action(this.onNewMonth));
      calendar.NewDay.Add<SettlementsManager>(this, new Action(this.onNewDay));
      simLoopEvents.Update.Add<SettlementsManager>(this, new Action(this.simUpdate));
    }

    public int GetTotalPopulation()
    {
      return this.NumberOfHomeless + this.GetTotalPopulationWithoutHomeless();
    }

    public int GetTotalPopulationWithoutHomeless()
    {
      int populationWithoutHomeless = 0;
      foreach (Settlement settlement in this.m_settlements)
        populationWithoutHomeless += settlement.Population;
      return populationWithoutHomeless;
    }

    public void AddSettlement(Settlement settlement) => this.m_settlements.AddAssertNew(settlement);

    public void RemoveSettlement(Settlement settlement)
    {
      this.m_settlements.RemoveAndAssert(settlement);
    }

    private void simUpdate()
    {
      this.m_homelessNotificator.NotifyIff(this.NumberOfHomeless > 0);
      this.m_popsStarvingNotif.NotifyIff(this.AmountStarvedToDeathLastMonth <= 0 && this.ArePeopleStarving);
      this.m_lowFoodNotif.NotifyIff(this.AmountStarvedToDeathLastMonth <= 0 && !this.ArePeopleStarving && this.MonthsOfFood <= 12);
    }

    private void onNewDay()
    {
      int populationWithoutHomeless = this.GetTotalPopulationWithoutHomeless();
      this.TotalPopulationStats.Set((long) (populationWithoutHomeless + this.NumberOfHomeless));
      this.TotalHousingStats.Set((long) this.TotalHousingCapacity);
      int self = 99;
      this.StarvingHomelessCountDays += this.NumberOfHomeless;
      foreach (Settlement settlement in this.m_settlements)
      {
        int homelessNotFed;
        settlement.OnNewDay(populationWithoutHomeless, this.StarvingHomelessCountDays, out homelessNotFed);
        this.StarvingHomelessCountDays = homelessNotFed;
        self = self.Min(settlement.MonthsOfFood);
      }
      this.MonthsOfFood = self;
      if (this.NumberOfHomeless > 0 && this.TotalHousingCapacity > 0)
      {
        Percent.FromRatio(this.NumberOfHomeless, this.TotalHousingCapacity);
        this.LastHomelessPenalty = (this.NumberOfHomeless * 100 / this.TotalHousingCapacity * SettlementsManager.HOMELESS_UNITY_PENALTY_PER_5_PERCENT / 5).Min(SettlementsManager.MAX_HOMELESS_PENALTY);
      }
      else
        this.LastHomelessPenalty = Upoints.Zero;
    }

    private void onNewMonth()
    {
      this.LastPopulationDiff = 0;
      this.AmountStarvedToDeathLastMonth = 0;
      this.ArePeopleStarving = false;
      int populationWithoutHomeless = this.GetTotalPopulationWithoutHomeless();
      foreach (Settlement settlement in this.m_settlements)
      {
        settlement.OnNewMonth(populationWithoutHomeless);
        this.AmountStarvedToDeathLastMonth += settlement.AmountStarvedToDeathLastMonth;
        this.ArePeopleStarving |= settlement.ArePeopleStarving;
      }
      if (this.LastHomelessPenalty.IsPositive)
        this.m_upointsManager.Value.ConsumeAsMuchAs(IdsCore.UpointsCategories.Homeless, this.LastHomelessPenalty, new Option<IEntity>(), new LocStr?());
      if (this.StarvingHomelessCountDays > 0)
      {
        int num = this.StarvingHomelessCountDays.CeilDiv(30).Min(this.NumberOfHomeless).CeilDiv(2);
        this.NumberOfHomeless -= num;
        this.LastPopulationDiff -= num;
        this.m_notificationsManager.NotifyOnce<int>(IdsCore.Notifications.HomelessLeft, num);
        this.StarvingHomelessCountDays = 0;
      }
      if (this.AmountStarvedToDeathLastMonth <= 0)
        return;
      this.m_notificationsManager.NotifyOnce<int>(IdsCore.Notifications.PopsStarvedToDeath, this.AmountStarvedToDeathLastMonth);
      this.tryToAccomodateHomeless();
    }

    /// <summary>Returns amount of pops it failed to add.</summary>
    public void AddPops(int amount, PopsAdditionReason reason)
    {
      if (amount == 0)
        return;
      if (amount < 0)
      {
        Log.Error("Adding negative population.");
      }
      else
      {
        if (reason == PopsAdditionReason.RefugeesOrAdopted)
          this.NewPopsFromAdoptions.Add((Fix32) amount);
        this.accomodate(amount);
      }
    }

    /// <summary>Returns removed population.</summary>
    public int RemovePopsAsMuchAs(int amount)
    {
      if (amount == 0)
        return 0;
      if (amount < 0)
      {
        Log.Error("Removing negative population.");
        return 0;
      }
      int num = this.remove(amount);
      return amount - num;
    }

    /// <summary>
    /// Happens when housing is lost from a Settlement and the settlement gets full.
    /// </summary>
    public void ReturnPopsBack(int popsToReturn)
    {
      this.m_onWorkersRemoved.Invoke(popsToReturn);
      this.NumberOfHomeless += popsToReturn;
      this.tryToAccomodateHomeless();
    }

    public void RecalculateValues()
    {
      int num = 0;
      Upoints zero = Upoints.Zero;
      foreach (Settlement settlement in this.m_settlements)
      {
        num += settlement.TotalHousingCapacity;
        zero += settlement.UpointsCapacity;
      }
      this.TotalHousingCapacity = num;
      this.m_upointsManager.Value.ChangeUnityCap(zero + UpointsManager.UNITY_BASE_CAP - this.m_upointsManager.Value.TotalUnityCap);
      this.tryToAccomodateHomeless();
    }

    public void AddInitialPopulation(int pops) => this.accomodate(pops, true);

    private void accomodate(int amountOfPeople, bool isInitialChange = false)
    {
      if (!isInitialChange)
        this.LastPopulationDiff += amountOfPeople;
      this.NumberOfHomeless += amountOfPeople;
      this.tryToAccomodateHomeless();
    }

    private void tryToAccomodateHomeless()
    {
      if (this.NumberOfHomeless <= 0)
        return;
      int numberOfHomeless = this.NumberOfHomeless;
      foreach (Settlement settlement in this.m_settlements)
      {
        numberOfHomeless -= settlement.TryAccomodate(numberOfHomeless);
        if (numberOfHomeless == 0)
          break;
      }
      int num = this.NumberOfHomeless - numberOfHomeless;
      this.NumberOfHomeless -= num;
      if (num <= 0)
        return;
      this.m_onWorkersAdded.Invoke(num);
    }

    public void UpdateWithheldWorkers()
    {
      int num1 = 0;
      foreach (Settlement settlement in this.m_settlements)
        num1 += settlement.NumberOfWorkersWithheld;
      if (this.NumberOfStarvingPopsWithheld != num1)
      {
        int num2 = num1 - this.NumberOfStarvingPopsWithheld;
        if (num2 > 0)
          this.m_onWorkersRemoved.Invoke(num2);
        else
          this.m_onWorkersAdded.Invoke(-num2);
      }
      this.NumberOfStarvingPopsWithheld = num1;
    }

    /// <summary>
    /// Returns the number of people it did not manage to remove.
    /// </summary>
    public int RemovePopFromSettlement(int toRemove, Settlement settlement)
    {
      return this.remove(toRemove, settlement);
    }

    /// <summary>
    /// Returns the number of people it did not manage to remove.
    /// </summary>
    private int remove(int amountOfPeople, Settlement settlementToUse = null)
    {
      this.LastPopulationDiff -= amountOfPeople;
      int num1;
      if (settlementToUse == null)
      {
        int num2 = amountOfPeople.Min(this.NumberOfHomeless);
        this.NumberOfHomeless -= num2;
        num1 = amountOfPeople - num2;
      }
      else
        num1 = amountOfPeople;
      int population = num1;
      foreach (Settlement settlement in this.m_settlements)
      {
        if (settlementToUse == null || settlement == settlementToUse)
        {
          population -= settlement.TryRemove(population);
          if (population == 0)
            break;
        }
      }
      int num3 = num1 - population;
      if (num3 > 0)
        this.m_onWorkersRemoved.Invoke(num3);
      return population;
    }

    public static void Serialize(SettlementsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SettlementsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SettlementsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.AmountStarvedToDeathLastMonth);
      writer.WriteBool(this.ArePeopleStarving);
      Upoints.Serialize(this.LastHomelessPenalty, writer);
      writer.WriteInt(this.LastPopulationDiff);
      Notificator.Serialize(this.m_homelessNotificator, writer);
      Notificator.Serialize(this.m_lowFoodNotif, writer);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      Event<int>.Serialize(this.m_onWorkersAdded, writer);
      Event<int>.Serialize(this.m_onWorkersRemoved, writer);
      Notificator.Serialize(this.m_popsStarvingNotif, writer);
      Lyst<Settlement>.Serialize(this.m_settlements, writer);
      LazyResolve<UpointsManager>.Serialize(this.m_upointsManager, writer);
      writer.WriteInt(this.MonthsOfFood);
      Fix32SumStats.Serialize(this.NewPopsFromAdoptions, writer);
      writer.WriteInt(this.NumberOfHomeless);
      writer.WriteInt(this.NumberOfStarvingPopsWithheld);
      writer.WriteInt(this.StarvingHomelessCountDays);
      writer.WriteInt(this.TotalHousingCapacity);
      IntAvgStats.Serialize(this.TotalHousingStats, writer);
      IntAvgStats.Serialize(this.TotalPopulationStats, writer);
    }

    public static SettlementsManager Deserialize(BlobReader reader)
    {
      SettlementsManager settlementsManager;
      if (reader.TryStartClassDeserialization<SettlementsManager>(out settlementsManager))
        reader.EnqueueDataDeserialization((object) settlementsManager, SettlementsManager.s_deserializeDataDelayedAction);
      return settlementsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AmountStarvedToDeathLastMonth = reader.ReadInt();
      this.ArePeopleStarving = reader.ReadBool();
      this.LastHomelessPenalty = Upoints.Deserialize(reader);
      this.LastPopulationDiff = reader.ReadInt();
      this.m_homelessNotificator = Notificator.Deserialize(reader);
      this.m_lowFoodNotif = Notificator.Deserialize(reader);
      reader.SetField<SettlementsManager>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<SettlementsManager>(this, "m_onWorkersAdded", (object) Event<int>.Deserialize(reader));
      reader.SetField<SettlementsManager>(this, "m_onWorkersRemoved", (object) Event<int>.Deserialize(reader));
      this.m_popsStarvingNotif = Notificator.Deserialize(reader);
      reader.SetField<SettlementsManager>(this, "m_settlements", (object) Lyst<Settlement>.Deserialize(reader));
      reader.SetField<SettlementsManager>(this, "m_upointsManager", (object) LazyResolve<UpointsManager>.Deserialize(reader));
      this.MonthsOfFood = reader.ReadInt();
      reader.SetField<SettlementsManager>(this, "NewPopsFromAdoptions", (object) Fix32SumStats.Deserialize(reader));
      this.NumberOfHomeless = reader.ReadInt();
      this.NumberOfStarvingPopsWithheld = reader.LoadedSaveVersion >= 140 ? reader.ReadInt() : 0;
      this.StarvingHomelessCountDays = reader.ReadInt();
      this.TotalHousingCapacity = reader.ReadInt();
      reader.SetField<SettlementsManager>(this, "TotalHousingStats", (object) IntAvgStats.Deserialize(reader));
      reader.SetField<SettlementsManager>(this, "TotalPopulationStats", (object) IntAvgStats.Deserialize(reader));
    }

    static SettlementsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SettlementsManager.HOMELESS_UNITY_PENALTY_PER_5_PERCENT = 0.25f.Upoints();
      SettlementsManager.MAX_HOMELESS_PENALTY = 2.5f.Upoints();
      SettlementsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SettlementsManager) obj).SerializeData(writer));
      SettlementsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SettlementsManager) obj).DeserializeData(reader));
    }
  }
}
