// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalsList
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  public class GoalsList
  {
    public readonly GoalListProto Prototype;
    private readonly Lyst<Goal> m_goals;
    private GoalListTrigger m_trigger;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public LocStrFormatted Title => (LocStrFormatted) this.Prototype.Strings.Name;

    public IIndexable<Goal> Goals => (IIndexable<Goal>) this.m_goals;

    public bool IsCompleted { get; private set; }

    [NewInSaveVersion(160, null, null, null, null)]
    public SimStep ActivatedAtSimStep { get; private set; }

    public GoalsList(GoalListProto prototype, DependencyResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_goals = new Lyst<Goal>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Prototype = prototype;
      foreach (GoalProto goal1 in prototype.Goals)
      {
        if (!goal1.IsNotAvailable)
        {
          try
          {
            if (resolver.Instantiate(goal1.Implementation, (object) goal1) is Goal goal2)
            {
              this.m_goals.Add(goal2);
              if (goal2.Prototype.LockedByIndex >= 0)
                goal2.IsLocked = true;
            }
            else
              Log.Error("Failed to instantiate goal " + goal1.GetType().Name);
          }
          catch (Exception ex)
          {
            Log.Exception(ex, "Failed to instantiate goal " + goal1.GetType().Name);
          }
        }
      }
      try
      {
        this.m_trigger = (GoalListTrigger) resolver.Instantiate(prototype.TriggerData.Implementation, (object) prototype.TriggerData, (object) this);
      }
      catch (Exception ex)
      {
        Log.Exception(ex, "Failed to instantiate trigger " + prototype.TriggerData.Implementation.Name);
        this.IsCompleted = true;
      }
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.m_goals.RemoveWhere((Predicate<Goal>) (x => x.Prototype.IsObsolete));
      this.m_goals.RemoveWhere((Predicate<Goal>) (x => !this.Prototype.Goals.Contains(x.Prototype)));
      for (int index = 0; index < this.m_goals.Count; ++index)
      {
        if (index <= this.m_goals[index].Prototype.LockedByIndex)
          this.m_goals[index].IsLocked = false;
      }
    }

    public void NotifyActivated(SimStep simStep) => this.ActivatedAtSimStep = simStep;

    public void Update()
    {
      bool flag = true;
      foreach (Goal goal in this.m_goals)
      {
        goal.Update();
        flag &= goal.IsCompleted;
      }
      foreach (Goal goal in this.m_goals)
      {
        if (goal.IsLocked)
        {
          int lockedByIndex = goal.Prototype.LockedByIndex;
          if (lockedByIndex < 0 || lockedByIndex >= this.m_goals.Count || this.m_goals[lockedByIndex].IsCompleted)
            goal.IsLocked = false;
        }
      }
      this.IsCompleted = flag;
    }

    public void ForceCompleteAllGoals()
    {
      foreach (Goal goal in this.m_goals)
        goal.ForceComplete();
      this.IsCompleted = true;
    }

    public void DestroyTrigger() => this.m_trigger.Destroy();

    public static void Serialize(GoalsList value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalsList>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalsList.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      SimStep.Serialize(this.ActivatedAtSimStep, writer);
      writer.WriteBool(this.IsCompleted);
      Lyst<Goal>.Serialize(this.m_goals, writer);
      writer.WriteGeneric<GoalListTrigger>(this.m_trigger);
      writer.WriteGeneric<GoalListProto>(this.Prototype);
    }

    public static GoalsList Deserialize(BlobReader reader)
    {
      GoalsList goalsList;
      if (reader.TryStartClassDeserialization<GoalsList>(out goalsList))
        reader.EnqueueDataDeserialization((object) goalsList, GoalsList.s_deserializeDataDelayedAction);
      return goalsList;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.ActivatedAtSimStep = reader.LoadedSaveVersion >= 160 ? SimStep.Deserialize(reader) : new SimStep();
      this.IsCompleted = reader.ReadBool();
      reader.SetField<GoalsList>(this, "m_goals", (object) Lyst<Goal>.Deserialize(reader));
      this.m_trigger = reader.ReadGenericAs<GoalListTrigger>();
      reader.SetField<GoalsList>(this, "Prototype", (object) reader.ReadGenericAs<GoalListProto>());
      reader.RegisterInitAfterLoad<GoalsList>(this, "initSelf", InitPriority.Normal);
    }

    static GoalsList()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalsList.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GoalsList) obj).SerializeData(writer));
      GoalsList.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GoalsList) obj).DeserializeData(reader));
    }
  }
}
