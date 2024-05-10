// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalsListTriggerToPauseBeacon
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Population;
using Mafi.Core.Population.Refugees;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  /// <summary>
  /// Special trigger that kicks in when player has too many workers and should pause a beacon.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class GoalsListTriggerToPauseBeacon : GoalListTrigger
  {
    private readonly SettlementsManager m_settlementsManager;
    private readonly WorkersManager m_workersManager;
    private readonly RefugeesManager m_refugeesManager;
    private readonly ICalendar m_calendar;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalsListTriggerToPauseBeacon(
      GoalsListTriggerToPauseBeacon.Data triggerData,
      GoalsList goalListToActivate,
      SettlementsManager settlementsManager,
      WorkersManager workersManager,
      RefugeesManager refugeesManager,
      ICalendar calendar,
      GoalsManager goalsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(goalListToActivate, goalsManager, triggerData.Version);
      this.m_settlementsManager = settlementsManager;
      this.m_workersManager = workersManager;
      this.m_refugeesManager = refugeesManager;
      this.m_calendar = calendar;
      this.m_calendar.NewDayEnd.Add<GoalsListTriggerToPauseBeacon>(this, new Action(this.onNewDay));
    }

    private void onNewDay()
    {
      if (this.m_settlementsManager.GetTotalPopulation() >= 300)
      {
        this.OnDestroy();
      }
      else
      {
        if (this.m_workersManager.AmountOfFreeWorkers <= 70 || !this.m_refugeesManager.Beacon.HasValue || !this.m_refugeesManager.StepsDoneSoFar.IsPositive)
          return;
        this.ActivateGoal();
      }
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewDayEnd.Remove<GoalsListTriggerToPauseBeacon>(this, new Action(this.onNewDay));
    }

    public static void Serialize(GoalsListTriggerToPauseBeacon value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalsListTriggerToPauseBeacon>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalsListTriggerToPauseBeacon.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      RefugeesManager.Serialize(this.m_refugeesManager, writer);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      WorkersManager.Serialize(this.m_workersManager, writer);
    }

    public static GoalsListTriggerToPauseBeacon Deserialize(BlobReader reader)
    {
      GoalsListTriggerToPauseBeacon triggerToPauseBeacon;
      if (reader.TryStartClassDeserialization<GoalsListTriggerToPauseBeacon>(out triggerToPauseBeacon))
        reader.EnqueueDataDeserialization((object) triggerToPauseBeacon, GoalsListTriggerToPauseBeacon.s_deserializeDataDelayedAction);
      return triggerToPauseBeacon;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalsListTriggerToPauseBeacon>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<GoalsListTriggerToPauseBeacon>(this, "m_refugeesManager", (object) RefugeesManager.Deserialize(reader));
      reader.SetField<GoalsListTriggerToPauseBeacon>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      reader.SetField<GoalsListTriggerToPauseBeacon>(this, "m_workersManager", (object) WorkersManager.Deserialize(reader));
    }

    static GoalsListTriggerToPauseBeacon()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalsListTriggerToPauseBeacon.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GoalListTrigger) obj).SerializeData(writer));
      GoalsListTriggerToPauseBeacon.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GoalListTrigger) obj).DeserializeData(reader));
    }

    public class Data : IGoalListTriggerData
    {
      public Type Implementation => typeof (GoalsListTriggerToPauseBeacon);

      public int Version { get; }

      public Data()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Version = 0;
      }
    }
  }
}
