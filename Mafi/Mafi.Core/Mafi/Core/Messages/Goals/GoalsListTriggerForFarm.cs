// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalsListTriggerForFarm
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  /// <summary>Trigger a goal list on  when food supply drops low.</summary>
  [GenerateSerializer(false, null, 0)]
  public class GoalsListTriggerForFarm : GoalListTrigger
  {
    private readonly SettlementsManager m_settlementsManager;
    private readonly EntitiesManager m_entitiesManager;
    private readonly ICalendar m_calendar;
    private readonly Mafi.Core.Prototypes.Proto.ID m_triggerOnlyAfterId;
    private readonly int m_numberOfFarmsToRemoveThis;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalsListTriggerForFarm(
      GoalsListTriggerForFarm.Data triggerData,
      GoalsList goalListToActivate,
      SettlementsManager settlementsManager,
      EntitiesManager entitiesManager,
      ICalendar calendar,
      GoalsManager goalsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(goalListToActivate, goalsManager, triggerData.Version);
      this.m_settlementsManager = settlementsManager;
      this.m_entitiesManager = entitiesManager;
      this.m_calendar = calendar;
      this.m_triggerOnlyAfterId = triggerData.TriggerOnlyAfter;
      this.m_numberOfFarmsToRemoveThis = triggerData.NumberOfFarmsToRemoveThis;
      this.m_calendar.NewDayEnd.Add<GoalsListTriggerForFarm>(this, new Action(this.onNewDay));
    }

    private void onNewDay()
    {
      if (this.m_entitiesManager.GetCountOf<Farm>((Predicate<Farm>) (x => x.IsConstructed)) >= this.m_numberOfFarmsToRemoveThis)
      {
        this.OnDestroy();
      }
      else
      {
        if (this.m_settlementsManager.MonthsOfFood > 20 || !this.GoalsManager.CompletedGoals.Any<GoalsList>((Predicate<GoalsList>) (x => x.Prototype.Id == this.m_triggerOnlyAfterId)))
          return;
        this.ActivateGoal();
      }
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewDayEnd.Remove<GoalsListTriggerForFarm>(this, new Action(this.onNewDay));
    }

    public static void Serialize(GoalsListTriggerForFarm value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalsListTriggerForFarm>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalsListTriggerForFarm.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteInt(this.m_numberOfFarmsToRemoveThis);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      Mafi.Core.Prototypes.Proto.ID.Serialize(this.m_triggerOnlyAfterId, writer);
    }

    public static GoalsListTriggerForFarm Deserialize(BlobReader reader)
    {
      GoalsListTriggerForFarm listTriggerForFarm;
      if (reader.TryStartClassDeserialization<GoalsListTriggerForFarm>(out listTriggerForFarm))
        reader.EnqueueDataDeserialization((object) listTriggerForFarm, GoalsListTriggerForFarm.s_deserializeDataDelayedAction);
      return listTriggerForFarm;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalsListTriggerForFarm>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<GoalsListTriggerForFarm>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<GoalsListTriggerForFarm>(this, "m_numberOfFarmsToRemoveThis", (object) reader.ReadInt());
      reader.SetField<GoalsListTriggerForFarm>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<GoalsListTriggerForFarm>(this, "m_triggerOnlyAfterId", (object) Mafi.Core.Prototypes.Proto.ID.Deserialize(reader));
    }

    static GoalsListTriggerForFarm()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalsListTriggerForFarm.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GoalListTrigger) obj).SerializeData(writer));
      GoalsListTriggerForFarm.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GoalListTrigger) obj).DeserializeData(reader));
    }

    public class Data : IGoalListTriggerData
    {
      public readonly Mafi.Core.Prototypes.Proto.ID TriggerOnlyAfter;
      public readonly int NumberOfFarmsToRemoveThis;

      public Type Implementation => typeof (GoalsListTriggerForFarm);

      public int Version { get; }

      public Data(Mafi.Core.Prototypes.Proto.ID triggerOnlyAfter, int numberOfFarmsToRemoveThis, int version = 0)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.TriggerOnlyAfter = triggerOnlyAfter;
        this.NumberOfFarmsToRemoveThis = numberOfFarmsToRemoveThis;
        this.Version = version;
      }
    }
  }
}
