// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalsListTriggerOnGoalListDone
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  /// <summary>
  /// Trigger a goal list when some other goal list was just done.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class GoalsListTriggerOnGoalListDone : GoalListTrigger
  {
    private readonly Lyst<Mafi.Core.Prototypes.Proto.ID> m_goalsIdsLeftToSatisfy;
    private readonly GoalsListTriggerOnGoalListDone.TriggerRule m_rule;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalsListTriggerOnGoalListDone(
      GoalsListTriggerOnGoalListDone.Data triggerData,
      GoalsList goalListToActivate,
      GoalsManager goalsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(goalListToActivate, goalsManager, triggerData.Version);
      this.m_goalsIdsLeftToSatisfy = triggerData.GoalListProtosIds.ToLyst();
      this.m_rule = triggerData.Rule;
      goalsManager.OnGoalFinished.Add<GoalsListTriggerOnGoalListDone>(this, new Action<GoalsList>(this.onGoalFinished));
    }

    private void onGoalFinished(GoalsList finishedGoal)
    {
      if (!this.m_goalsIdsLeftToSatisfy.Remove(finishedGoal.Prototype.Id) || !this.m_goalsIdsLeftToSatisfy.IsEmpty && this.m_rule != GoalsListTriggerOnGoalListDone.TriggerRule.AnySatisfied)
        return;
      this.ActivateGoal();
    }

    protected override void OnDestroy()
    {
      this.GoalsManager.OnGoalFinished.Remove<GoalsListTriggerOnGoalListDone>(this, new Action<GoalsList>(this.onGoalFinished));
    }

    public static void Serialize(GoalsListTriggerOnGoalListDone value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalsListTriggerOnGoalListDone>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalsListTriggerOnGoalListDone.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Lyst<Mafi.Core.Prototypes.Proto.ID>.Serialize(this.m_goalsIdsLeftToSatisfy, writer);
      writer.WriteInt((int) this.m_rule);
    }

    public static GoalsListTriggerOnGoalListDone Deserialize(BlobReader reader)
    {
      GoalsListTriggerOnGoalListDone triggerOnGoalListDone;
      if (reader.TryStartClassDeserialization<GoalsListTriggerOnGoalListDone>(out triggerOnGoalListDone))
        reader.EnqueueDataDeserialization((object) triggerOnGoalListDone, GoalsListTriggerOnGoalListDone.s_deserializeDataDelayedAction);
      return triggerOnGoalListDone;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalsListTriggerOnGoalListDone>(this, "m_goalsIdsLeftToSatisfy", (object) Lyst<Mafi.Core.Prototypes.Proto.ID>.Deserialize(reader));
      reader.SetField<GoalsListTriggerOnGoalListDone>(this, "m_rule", (object) (GoalsListTriggerOnGoalListDone.TriggerRule) reader.ReadInt());
    }

    static GoalsListTriggerOnGoalListDone()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalsListTriggerOnGoalListDone.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GoalListTrigger) obj).SerializeData(writer));
      GoalsListTriggerOnGoalListDone.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GoalListTrigger) obj).DeserializeData(reader));
    }

    public enum TriggerRule
    {
      AllSatisfied,
      AnySatisfied,
    }

    public class Data : IGoalListTriggerData
    {
      public readonly ImmutableArray<Mafi.Core.Prototypes.Proto.ID> GoalListProtosIds;
      public readonly GoalsListTriggerOnGoalListDone.TriggerRule Rule;

      public Type Implementation => typeof (GoalsListTriggerOnGoalListDone);

      public int Version { get; }

      public Data(Mafi.Core.Prototypes.Proto.ID goalListIdProtoDone, int version = 0)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.GoalListProtosIds = ImmutableArray.Create<Mafi.Core.Prototypes.Proto.ID>(goalListIdProtoDone);
        this.Rule = GoalsListTriggerOnGoalListDone.TriggerRule.AnySatisfied;
        this.Version = version;
      }

      public Data(
        ImmutableArray<Mafi.Core.Prototypes.Proto.ID> goalListProtoIds,
        GoalsListTriggerOnGoalListDone.TriggerRule rule)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.GoalListProtosIds = goalListProtoIds;
        this.Rule = rule;
      }
    }
  }
}
