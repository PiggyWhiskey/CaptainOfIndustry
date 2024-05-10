// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.PopsHealthManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.GameLoop;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.World;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Population
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class PopsHealthManager
  {
    private static readonly int DISEASE_FREE_EARLY_GAME_YEARS;
    private static readonly int MIN_MONTHS_BETWEEN_DISEASES;
    private static readonly int MAX_MONTHS_BETWEEN_DISEASES;
    /// <summary>Once health drops under this value, pops start to die</summary>
    public static readonly Percent MIN_HEALTH;
    /// <summary>
    /// Points given or deducted based on health points above / below MIN_HEALTH
    /// </summary>
    public static readonly Upoints UPOINTS_PER_HEALTHPOINT;
    /// <summary>Given if health is above MIN_HEALTH</summary>
    public static readonly Upoints UPOINTS_FOR_ABOVE_MIN;
    private int m_monthsSinceLastDisease;
    public readonly Fix32SumStats BornTotal;
    public readonly Fix32SumStats LostTotal;
    public static readonly Percent BASE_HEALTH_DEFAULT;
    /// <summary>Note: can be negative</summary>
    public Upoints UpointsForHealthLastMonth;
    private readonly IProperty<Percent> m_baseHealthMultiplier;
    private readonly IProperty<Percent> m_baseHealthDiffEdicts;
    private readonly IProperty<Percent> m_diseaseEffectsMultiplier;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IProperty<Percent> m_diseaseMortalityMultiplier;
    private Percent m_addedPopsGrowth;
    private Fix32 m_popsDiffBuffer;
    private ImmutableArray<DiseaseProto> m_diseasesWithoutCustomTrigger;
    private ImmutableArray<IDiseaseTrigger> m_diseasesWithCustomTrigger;
    private readonly SettlementsManager m_settlementsManager;
    private readonly ICalendar m_calendar;
    private readonly ProtosDb m_protosDb;
    private readonly LazyResolve<TravelingFleetManager> m_fleetManager;
    private readonly IRandom m_random;
    private Percent m_diseaseHealthPenaltySum;
    private Percent m_diseaseMortalityRateSum;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Option<DiseaseProto> CurrentDisease { get; private set; }

    public int CurrentDiseaseMonthsLeft { get; private set; }

    public Percent CurrentDiseaseMortality
    {
      get
      {
        return !this.CurrentDisease.HasValue ? Percent.Zero : this.CurrentDisease.Value.MonthlyMortalityRate.ScaleBy(this.m_diseaseMortalityMultiplier.Value);
      }
    }

    public bool IsDiseaseMortalityIgnored => this.m_diseaseMortalityMultiplier.Value.IsNotPositive;

    public HealthStatistics HealthStats { get; private set; }

    public BirthStatistics BirthStats { get; private set; }

    public bool IsPopulationGrowthPaused { get; set; }

    private Percent BaseHealth
    {
      get
      {
        return PopsHealthManager.BASE_HEALTH_DEFAULT.ScaleBy(this.m_baseHealthMultiplier.Value).Clamp0To100();
      }
    }

    public PopsHealthManager(
      SettlementsManager settlementsManager,
      LazyResolve<TravelingFleetManager> fleetManager,
      ICalendar calendar,
      IGameLoopEvents gameLoopEvents,
      ProtosDb protosDb,
      DependencyResolver resolver,
      IPropertiesDb propertiesDb,
      RandomProvider randomProvider,
      StatsManager statsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_addedPopsGrowth = Percent.Zero;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      PopsHealthManager popsHealthManager = this;
      this.m_settlementsManager = settlementsManager;
      this.m_calendar = calendar;
      this.m_protosDb = protosDb;
      this.m_fleetManager = fleetManager;
      this.m_random = randomProvider.GetSimRandomFor((object) this);
      this.m_baseHealthMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.BaseHealthMultiplier);
      this.m_baseHealthDiffEdicts = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.BaseHealthDiffEdicts);
      this.m_diseaseEffectsMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.DiseaseEffectsMultiplier);
      this.m_diseaseMortalityMultiplier = propertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.DiseaseMortalityMultiplier);
      calendar.NewMonthStart.Add<PopsHealthManager>(this, new Action(this.onNewMonthStart));
      calendar.NewMonthEnd.Add<PopsHealthManager>(this, new Action(this.onNewMonthEnd));
      calendar.NewDay.Add<PopsHealthManager>(this, new Action(this.onNewDay));
      this.BornTotal = new Fix32SumStats(Option<StatsManager>.None, true);
      statsManager.RegisterForYearlyUpdatesOnly((ItemStats) this.BornTotal);
      this.LostTotal = new Fix32SumStats(Option<StatsManager>.None, true);
      statsManager.RegisterForYearlyUpdatesOnly((ItemStats) this.LostTotal);
      this.HealthStats = new HealthStatistics(protosDb, statsManager);
      this.BirthStats = new BirthStatistics();
      Lyst<DiseaseProto> diseasesWithoutCustomTrigger = new Lyst<DiseaseProto>();
      Lyst<IDiseaseTrigger> diseasesWithCustomTrigger = new Lyst<IDiseaseTrigger>();
      this.HealthStats.HealthLastMonth = this.BaseHealth;
      gameLoopEvents.RegisterNewGameCreated((object) this, (Action) (() =>
      {
        foreach (DiseaseProto diseaseProto in protosDb.All<DiseaseProto>())
        {
          if (diseaseProto.CustomTrigger.HasValue)
          {
            if (!(resolver.Instantiate(diseaseProto.CustomTrigger.Value, (object) diseaseProto) is IDiseaseTrigger diseaseTrigger2))
              Log.Error(string.Format("Failed create instance of {0} for {1}", (object) diseaseProto.CustomTrigger, (object) diseaseProto));
            else
              diseasesWithCustomTrigger.Add(diseaseTrigger2);
          }
          else
            diseasesWithoutCustomTrigger.Add(diseaseProto);
        }
        popsHealthManager.m_diseasesWithoutCustomTrigger = diseasesWithoutCustomTrigger.ToImmutableArray();
        popsHealthManager.m_diseasesWithCustomTrigger = diseasesWithCustomTrigger.ToImmutableArray();
      }));
    }

    [InitAfterLoad(InitPriority.Normal)]
    [OnlyForSaveCompatibility(null)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<PopsHealthManager>(this, "m_diseaseMortalityMultiplier", (object) resolver.Resolve<IPropertiesDb>().GetProperty<Percent>(IdsCore.PropertyIds.DiseaseMortalityMultiplier));
    }

    private void onNewDay() => this.updateDiseaseOnNewDay();

    public void AddHealthDecrease(Proto.ID categoryId, Percent reduction, Percent? max = null)
    {
      HealthPointsCategoryProto proto;
      if (!this.m_protosDb.TryGetProto<HealthPointsCategoryProto>(categoryId, out proto))
        Log.Error(string.Format("Category '{0}' not found!", (object) categoryId));
      else
        this.HealthStats.AddReduction(proto, reduction, max.GetValueOrDefault(reduction));
    }

    public void AddHealthIncrease(Proto.ID categoryId, Percent increase, Percent? max = null)
    {
      HealthPointsCategoryProto proto;
      if (!this.m_protosDb.TryGetProto<HealthPointsCategoryProto>(categoryId, out proto))
        Log.Error(string.Format("Category '{0}' not found!", (object) categoryId));
      else
        this.HealthStats.AddIncrease(proto, increase, max.GetValueOrDefault(increase));
    }

    public void AddBirthDecrease(
      Proto.ID categoryId,
      Percent reduction,
      Percent? max = null,
      bool wasAlreadyApplied = false)
    {
      BirthRateCategoryProto proto;
      if (!this.m_protosDb.TryGetProto<BirthRateCategoryProto>(categoryId, out proto))
        Log.Error(string.Format("Category '{0}' not found!", (object) categoryId));
      else
        this.BirthStats.AddReduction(proto, reduction, max.GetValueOrDefault(reduction), wasAlreadyApplied);
    }

    public void AddBirthIncrease(Proto.ID categoryId, Percent increase, Percent? max = null)
    {
      BirthRateCategoryProto proto;
      if (!this.m_protosDb.TryGetProto<BirthRateCategoryProto>(categoryId, out proto))
        Log.Error(string.Format("Category '{0}' not found!", (object) categoryId));
      else
        this.BirthStats.AddIncrease(proto, increase, max.GetValueOrDefault(increase));
    }

    private void onNewMonthStart()
    {
      this.AddHealthIncrease(IdsCore.HealthPointsCategories.Base, this.BaseHealth);
      if (this.m_baseHealthDiffEdicts.Value.IsPositive)
        this.AddHealthIncrease(IdsCore.HealthPointsCategories.Edicts, this.m_baseHealthDiffEdicts.Value);
      else if (this.m_baseHealthDiffEdicts.Value.IsNegative)
        this.AddHealthDecrease(IdsCore.HealthPointsCategories.Edicts, this.m_baseHealthDiffEdicts.Value);
      if (!this.m_addedPopsGrowth.IsPositive)
        return;
      this.AddBirthIncrease(IdsCore.BirthRateCategories.Base, this.m_addedPopsGrowth);
    }

    private void onNewMonthEnd()
    {
      this.updateDiseaseOnNewMonth();
      this.applyBirthRate();
      this.BirthStats.OnNewMonthEnd();
      this.HealthStats.OnNewMonthEnd();
      ((IItemStatsEvents) this.BornTotal).OnNewMonth();
      ((IItemStatsEvents) this.LostTotal).OnNewMonth();
    }

    private void applyBirthRate()
    {
      Percent healthThisMonth = this.HealthStats.HealthThisMonth;
      if (healthThisMonth < PopsHealthManager.MIN_HEALTH)
      {
        Fix32 fix32 = 0.1.ToFix32();
        Percent percent = PopsHealthManager.MIN_HEALTH - healthThisMonth;
        Percent reduction = (percent.ToIntPercentRounded() * fix32).ToPercent() / 100;
        if (reduction.IsPositive)
          this.AddBirthDecrease(IdsCore.BirthRateCategories.Health, reduction, new Percent?(reduction));
        percent = PopsHealthManager.MIN_HEALTH - healthThisMonth;
        this.UpointsForHealthLastMonth = percent.ToIntPercentRounded() * -PopsHealthManager.UPOINTS_PER_HEALTHPOINT;
      }
      else
      {
        Fix32 fix32 = 0.005.ToFix32();
        Percent percent = healthThisMonth - PopsHealthManager.MIN_HEALTH;
        Percent increase = (percent.ToIntPercentRounded() * fix32).ToPercent() / 100;
        if (increase.IsPositive && !this.IsPopulationGrowthPaused)
          this.AddBirthIncrease(IdsCore.BirthRateCategories.Health, increase, new Percent?(increase));
        Upoints upointsForAboveMin = PopsHealthManager.UPOINTS_FOR_ABOVE_MIN;
        percent = healthThisMonth - PopsHealthManager.MIN_HEALTH;
        Upoints upoints = percent.ToIntPercentRounded() * PopsHealthManager.UPOINTS_PER_HEALTHPOINT;
        this.UpointsForHealthLastMonth = upointsForAboveMin + upoints;
      }
      Percent percent1 = this.BirthStats.BirthRateThisMonth - this.BirthStats.BirthRateThisMonthAlreadyApplied;
      int num1 = 0;
      int num2 = 0;
      if (percent1.IsNotZero)
      {
        int totalPopulation = this.m_settlementsManager.GetTotalPopulation();
        this.m_popsDiffBuffer += percent1.Apply(totalPopulation.ToFix32());
        if (this.m_popsDiffBuffer.IsPositive && this.m_settlementsManager.FreeHousingCapacity <= 0)
          this.m_popsDiffBuffer = (Fix32) 0;
        else if (this.m_popsDiffBuffer.IntegerPart > 0)
        {
          int amount = this.m_popsDiffBuffer.IntegerPart.Min(this.m_settlementsManager.FreeHousingCapacity);
          this.m_settlementsManager.AddPops(amount, PopsAdditionReason.Other);
          this.m_popsDiffBuffer -= (Fix32) amount;
          num1 = amount;
        }
        else if (this.m_popsDiffBuffer.IntegerPart < 0)
        {
          int num3 = this.m_settlementsManager.RemovePopsAsMuchAs(this.m_popsDiffBuffer.IntegerPart.Abs());
          this.m_popsDiffBuffer += (Fix32) num3;
          num2 = num3;
        }
      }
      this.BornTotal.Add((Fix32) num1);
      this.LostTotal.Add((Fix32) num2);
    }

    private void startDisease(DiseaseProto disease)
    {
      Assert.That<Option<DiseaseProto>>(this.CurrentDisease).IsNone<DiseaseProto>();
      this.CurrentDisease = (Option<DiseaseProto>) disease;
      this.CurrentDiseaseMonthsLeft = disease.DurationInMonths;
    }

    private void updateDiseaseOnNewDay()
    {
      if (this.CurrentDisease.IsNone || this.CurrentDiseaseMonthsLeft <= 1)
        return;
      DiseaseProto diseaseProto = this.CurrentDisease.Value;
      Percent healthPenalty = diseaseProto.HealthPenalty;
      Percent zero = Percent.Zero;
      foreach (Settlement settlement in (IEnumerable<Settlement>) this.m_settlementsManager.Settlements)
        zero += settlement.DiseaseMortalityDeductionLastDay;
      this.m_diseaseMortalityRateSum += (diseaseProto.MonthlyMortalityRate.ScaleBy(this.m_diseaseEffectsMultiplier.Value).ScaleBy(this.m_diseaseMortalityMultiplier.Value) - zero).Max(Percent.Zero);
      this.m_diseaseHealthPenaltySum += healthPenalty.ScaleBy(this.m_diseaseEffectsMultiplier.Value);
    }

    private void updateDiseaseOnNewMonth()
    {
      if (this.CurrentDisease.IsNone)
      {
        this.startDiseaseIfNeeded();
      }
      else
      {
        Percent reduction1 = this.m_diseaseMortalityRateSum / 30;
        if (reduction1.IsPositive)
          this.AddBirthDecrease(IdsCore.BirthRateCategories.Disease, reduction1);
        this.m_diseaseMortalityRateSum = Percent.Zero;
        Percent reduction2 = this.m_diseaseHealthPenaltySum / 30;
        if (reduction2.IsPositive)
          this.AddHealthDecrease(IdsCore.HealthPointsCategories.Disease, reduction2);
        this.m_diseaseHealthPenaltySum = Percent.Zero;
        --this.CurrentDiseaseMonthsLeft;
        if (this.CurrentDiseaseMonthsLeft != 0)
          return;
        this.CurrentDisease = Option<DiseaseProto>.None;
        this.m_monthsSinceLastDisease = 0;
      }
    }

    private void startDiseaseIfNeeded()
    {
      if (this.CurrentDisease.HasValue)
        return;
      ++this.m_monthsSinceLastDisease;
      int totalPopulation = this.m_settlementsManager.GetTotalPopulation();
      if (totalPopulation <= 0)
        return;
      foreach (IDiseaseTrigger diseaseTrigger in this.m_diseasesWithCustomTrigger)
      {
        Option<DiseaseProto> trigger = diseaseTrigger.TryToTrigger(this.m_monthsSinceLastDisease, totalPopulation);
        if (trigger.HasValue)
        {
          this.startDisease(trigger.Value);
          return;
        }
      }
      if (this.m_calendar.CurrentDate.Year <= PopsHealthManager.DISEASE_FREE_EARLY_GAME_YEARS || this.m_calendar.CurrentDate.Year <= 2 * PopsHealthManager.DISEASE_FREE_EARLY_GAME_YEARS && totalPopulation < 300 || this.m_monthsSinceLastDisease < PopsHealthManager.MIN_MONTHS_BETWEEN_DISEASES)
        return;
      Percent diseaseSpawnChance = this.getDiseaseSpawnChance(this.m_monthsSinceLastDisease);
      if (this.m_monthsSinceLastDisease < PopsHealthManager.MAX_MONTHS_BETWEEN_DISEASES && !this.m_random.TestProbability(diseaseSpawnChance))
        return;
      Option<DiseaseProto> disease = this.generateDisease();
      if (!disease.HasValue)
        return;
      this.startDisease(disease.Value);
    }

    private Option<DiseaseProto> generateDisease()
    {
      Lyst<DiseaseProto> lyst = this.m_diseasesWithoutCustomTrigger.Where((Func<DiseaseProto, bool>) (x => x.MinDistanceTraveled <= this.m_fleetManager.Value.FarthestLocationVisited)).OrderByDescending<DiseaseProto, Percent>((Func<DiseaseProto, Percent>) (x => x.HealthPenalty)).Take<DiseaseProto>(3).ToLyst<DiseaseProto>();
      return lyst.IsEmpty ? (Option<DiseaseProto>) Option.None : (Option<DiseaseProto>) lyst[this.m_random];
    }

    private Percent getDiseaseSpawnChance(int monthsSinceLastDisease)
    {
      int num = monthsSinceLastDisease - PopsHealthManager.MIN_MONTHS_BETWEEN_DISEASES;
      if (num <= 8)
        return 10.Percent();
      if (num <= 16)
        return 20.Percent();
      return num <= 24 ? 30.Percent() : 40.Percent();
    }

    public static void Serialize(PopsHealthManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PopsHealthManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PopsHealthManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      BirthStatistics.Serialize(this.BirthStats, writer);
      Fix32SumStats.Serialize(this.BornTotal, writer);
      Option<DiseaseProto>.Serialize(this.CurrentDisease, writer);
      writer.WriteInt(this.CurrentDiseaseMonthsLeft);
      HealthStatistics.Serialize(this.HealthStats, writer);
      writer.WriteBool(this.IsPopulationGrowthPaused);
      Fix32SumStats.Serialize(this.LostTotal, writer);
      Percent.Serialize(this.m_addedPopsGrowth, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_baseHealthDiffEdicts);
      writer.WriteGeneric<IProperty<Percent>>(this.m_baseHealthMultiplier);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<IProperty<Percent>>(this.m_diseaseEffectsMultiplier);
      Percent.Serialize(this.m_diseaseHealthPenaltySum, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_diseaseMortalityMultiplier);
      Percent.Serialize(this.m_diseaseMortalityRateSum, writer);
      ImmutableArray<IDiseaseTrigger>.Serialize(this.m_diseasesWithCustomTrigger, writer);
      ImmutableArray<DiseaseProto>.Serialize(this.m_diseasesWithoutCustomTrigger, writer);
      LazyResolve<TravelingFleetManager>.Serialize(this.m_fleetManager, writer);
      writer.WriteInt(this.m_monthsSinceLastDisease);
      Fix32.Serialize(this.m_popsDiffBuffer, writer);
      writer.WriteGeneric<IRandom>(this.m_random);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      Upoints.Serialize(this.UpointsForHealthLastMonth, writer);
    }

    public static PopsHealthManager Deserialize(BlobReader reader)
    {
      PopsHealthManager popsHealthManager;
      if (reader.TryStartClassDeserialization<PopsHealthManager>(out popsHealthManager))
        reader.EnqueueDataDeserialization((object) popsHealthManager, PopsHealthManager.s_deserializeDataDelayedAction);
      return popsHealthManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.BirthStats = BirthStatistics.Deserialize(reader);
      reader.SetField<PopsHealthManager>(this, "BornTotal", (object) Fix32SumStats.Deserialize(reader));
      this.CurrentDisease = Option<DiseaseProto>.Deserialize(reader);
      this.CurrentDiseaseMonthsLeft = reader.ReadInt();
      this.HealthStats = HealthStatistics.Deserialize(reader);
      this.IsPopulationGrowthPaused = reader.ReadBool();
      reader.SetField<PopsHealthManager>(this, "LostTotal", (object) Fix32SumStats.Deserialize(reader));
      this.m_addedPopsGrowth = Percent.Deserialize(reader);
      reader.SetField<PopsHealthManager>(this, "m_baseHealthDiffEdicts", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<PopsHealthManager>(this, "m_baseHealthMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<PopsHealthManager>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<PopsHealthManager>(this, "m_diseaseEffectsMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      this.m_diseaseHealthPenaltySum = Percent.Deserialize(reader);
      reader.SetField<PopsHealthManager>(this, "m_diseaseMortalityMultiplier", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IProperty<Percent>>() : (object) (IProperty<Percent>) null);
      this.m_diseaseMortalityRateSum = Percent.Deserialize(reader);
      this.m_diseasesWithCustomTrigger = ImmutableArray<IDiseaseTrigger>.Deserialize(reader);
      this.m_diseasesWithoutCustomTrigger = ImmutableArray<DiseaseProto>.Deserialize(reader);
      reader.SetField<PopsHealthManager>(this, "m_fleetManager", (object) LazyResolve<TravelingFleetManager>.Deserialize(reader));
      this.m_monthsSinceLastDisease = reader.ReadInt();
      this.m_popsDiffBuffer = Fix32.Deserialize(reader);
      reader.RegisterResolvedMember<PopsHealthManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<PopsHealthManager>(this, "m_random", (object) reader.ReadGenericAs<IRandom>());
      reader.SetField<PopsHealthManager>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      this.UpointsForHealthLastMonth = Upoints.Deserialize(reader);
      reader.RegisterInitAfterLoad<PopsHealthManager>(this, "initSelf", InitPriority.Normal);
    }

    static PopsHealthManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PopsHealthManager.DISEASE_FREE_EARLY_GAME_YEARS = 20;
      PopsHealthManager.MIN_MONTHS_BETWEEN_DISEASES = 36;
      PopsHealthManager.MAX_MONTHS_BETWEEN_DISEASES = 72;
      PopsHealthManager.MIN_HEALTH = 0.Percent();
      PopsHealthManager.UPOINTS_PER_HEALTHPOINT = 0.02.Upoints();
      PopsHealthManager.UPOINTS_FOR_ABOVE_MIN = 0.2.Upoints();
      PopsHealthManager.BASE_HEALTH_DEFAULT = 10.Percent();
      PopsHealthManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PopsHealthManager) obj).SerializeData(writer));
      PopsHealthManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PopsHealthManager) obj).DeserializeData(reader));
    }
  }
}
