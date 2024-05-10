// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Console;
using Mafi.Core.Economy;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class GoalsManager : 
    ICommandProcessor<MarkGoalAsFinishedCmd>,
    IAction<MarkGoalAsFinishedCmd>
  {
    [NewInSaveVersion(160, null, null, typeof (ISimLoopEvents), null)]
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly IAssetTransactionManager m_assetTransactionMgr;
    private readonly DependencyResolver m_resolver;
    private readonly MessagesManager m_messagesManager;
    private readonly ProtosDb m_protosDb;
    private readonly Lyst<GoalsList> m_completedGoals;
    private readonly Lyst<GoalsList> m_activeGoals;
    private Lyst<GoalsList> m_goalsWaitingForActivation;
    private readonly Event<GoalsList> m_onGoalFinished;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IIndexable<GoalsList> CompletedGoals => (IIndexable<GoalsList>) this.m_completedGoals;

    public IIndexable<GoalsList> ActiveGoals => (IIndexable<GoalsList>) this.m_activeGoals;

    /// <summary>
    /// Invoked when a goal was finished or skipped, but reward was not yet taken.
    /// </summary>
    public IEvent<GoalsList> OnGoalFinished => (IEvent<GoalsList>) this.m_onGoalFinished;

    public GoalsManager(
      IGameLoopEvents gameEvents,
      ISimLoopEvents simLoopEvents,
      IAssetTransactionManager assetTransactionMgr,
      DependencyResolver resolver,
      MessagesManager messagesManager,
      ProtosDb protosDb,
      TutorialsConfig tutorialsConfig)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_completedGoals = new Lyst<GoalsList>();
      this.m_activeGoals = new Lyst<GoalsList>();
      this.m_goalsWaitingForActivation = new Lyst<GoalsList>();
      this.m_onGoalFinished = new Event<GoalsList>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_simLoopEvents = simLoopEvents;
      this.m_assetTransactionMgr = assetTransactionMgr;
      this.m_resolver = resolver;
      this.m_messagesManager = messagesManager;
      this.m_protosDb = protosDb;
      simLoopEvents.Update.Add<GoalsManager>(this, new Action(this.simUpdate));
      if (!tutorialsConfig.AreTutorialsEnabled)
        return;
      gameEvents.RegisterNewGameCreated((object) this, new Action(this.newGameCreated));
    }

    [ConsoleCommand(false, false, null, null)]
    private void clearAllGoalsPermanently()
    {
      foreach (GoalsList goalsList in this.m_activeGoals.ToArray())
      {
        if (!goalsList.IsCompleted)
          goalsList.ForceCompleteAllGoals();
      }
      foreach (GoalsList goalsList in this.m_goalsWaitingForActivation)
        goalsList.DestroyTrigger();
      this.m_activeGoals.Clear();
      this.m_goalsWaitingForActivation.Clear();
      this.m_completedGoals.Clear();
      Log.GameProgress("Goals cleared permanently");
    }

    private void newGameCreated()
    {
      foreach (GoalListProto prototype in this.m_protosDb.All<GoalListProto>())
      {
        if (!prototype.IsObsolete)
          this.m_goalsWaitingForActivation.Add(new GoalsList(prototype, this.m_resolver));
      }
    }

    private void simUpdate()
    {
      foreach (GoalsList activeGoal in this.m_activeGoals)
      {
        activeGoal.Update();
        if (activeGoal.IsCompleted)
        {
          this.m_activeGoals.Remove(activeGoal);
          this.onGoalsListCompleted(activeGoal);
          break;
        }
      }
    }

    private void onGoalsListCompleted(GoalsList goalsList)
    {
      if (goalsList.Prototype.Rewards.IsNotEmpty)
        this.m_completedGoals.Add(goalsList);
      this.m_onGoalFinished.Invoke(goalsList);
      Log.GameProgress("Goal completed", new KeyValuePair<string, long>[1]
      {
        Make.Kvp<string, long>("goal_duration", (long) (this.m_simLoopEvents.CurrentStep - goalsList.ActivatedAtSimStep).Ticks)
      }, new KeyValuePair<string, string>[1]
      {
        Make.Kvp<string, string>("goal_id", goalsList.Prototype.Id.Value)
      });
    }

    public void ActivateGoal(GoalsList goalList)
    {
      if (goalList.IsCompleted)
      {
        Log.Error(string.Format("Adding already completed goal '{0}'!", (object) goalList.Prototype.Id));
      }
      else
      {
        foreach (Goal goal in goalList.Goals)
        {
          MessageProto proto;
          if (goal.Prototype.Tutorial.HasValue && goal.Prototype.TutorialUnlock != GoalProto.TutorialUnlockMode.DoNotUnlock && this.m_protosDb.TryGetProto<MessageProto>(goal.Prototype.Tutorial.Value, out proto))
            this.m_messagesManager.AddMessage(proto, goal.Prototype.TutorialUnlock == GoalProto.TutorialUnlockMode.UnlockSilently);
        }
        goalList.Update();
        this.m_goalsWaitingForActivation.Remove(goalList);
        this.m_activeGoals.AddIfNotPresent(goalList);
        goalList.NotifyActivated(this.m_simLoopEvents.CurrentStep);
      }
    }

    public void Invoke(MarkGoalAsFinishedCmd cmd)
    {
      if (cmd.GoalWasSkipped)
      {
        GoalsList goalsList = this.m_activeGoals.FirstOrDefault<GoalsList>((Predicate<GoalsList>) (x => x.Prototype.Id == cmd.ProtoId));
        if (goalsList == null)
        {
          cmd.SetResultError(string.Format("Goal with id {0} not found", (object) cmd.ProtoId));
        }
        else
        {
          this.m_activeGoals.Remove(goalsList);
          this.m_goalsWaitingForActivation.Remove(goalsList);
          this.m_onGoalFinished.Invoke(goalsList);
          cmd.SetResultSuccess();
          Log.GameProgress("Goal skipped", new KeyValuePair<string, long>[1]
          {
            Make.Kvp<string, long>("goal_duration", (long) (this.m_simLoopEvents.CurrentStep - goalsList.ActivatedAtSimStep).Ticks)
          }, new KeyValuePair<string, string>[1]
          {
            Make.Kvp<string, string>("goal_id", goalsList.Prototype.Id.Value)
          });
        }
      }
      else
      {
        GoalsList goalsList = this.m_completedGoals.FirstOrDefault<GoalsList>((Predicate<GoalsList>) (x => x.Prototype.Id == cmd.ProtoId));
        if (goalsList == null)
        {
          cmd.SetResultError(string.Format("Goal with id {0} not found", (object) cmd.ProtoId));
        }
        else
        {
          foreach (ProductQuantity reward in goalsList.Prototype.Rewards)
            this.m_assetTransactionMgr.StoreProduct(reward, new CreateReason?(CreateReason.Loot));
          this.m_completedGoals.Remove(goalsList);
          this.m_goalsWaitingForActivation.Remove(goalsList);
          cmd.SetResultSuccess();
        }
      }
    }

    public static void Serialize(GoalsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Lyst<GoalsList>.Serialize(this.m_activeGoals, writer);
      writer.WriteGeneric<IAssetTransactionManager>(this.m_assetTransactionMgr);
      Lyst<GoalsList>.Serialize(this.m_completedGoals, writer);
      Lyst<GoalsList>.Serialize(this.m_goalsWaitingForActivation, writer);
      MessagesManager.Serialize(this.m_messagesManager, writer);
      Event<GoalsList>.Serialize(this.m_onGoalFinished, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
    }

    public static GoalsManager Deserialize(BlobReader reader)
    {
      GoalsManager goalsManager;
      if (reader.TryStartClassDeserialization<GoalsManager>(out goalsManager))
        reader.EnqueueDataDeserialization((object) goalsManager, GoalsManager.s_deserializeDataDelayedAction);
      return goalsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GoalsManager>(this, "m_activeGoals", (object) Lyst<GoalsList>.Deserialize(reader));
      reader.SetField<GoalsManager>(this, "m_assetTransactionMgr", (object) reader.ReadGenericAs<IAssetTransactionManager>());
      reader.SetField<GoalsManager>(this, "m_completedGoals", (object) Lyst<GoalsList>.Deserialize(reader));
      this.m_goalsWaitingForActivation = Lyst<GoalsList>.Deserialize(reader);
      reader.SetField<GoalsManager>(this, "m_messagesManager", (object) MessagesManager.Deserialize(reader));
      reader.SetField<GoalsManager>(this, "m_onGoalFinished", (object) Event<GoalsList>.Deserialize(reader));
      reader.RegisterResolvedMember<GoalsManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<GoalsManager>(this, "m_resolver", (object) DependencyResolver.Deserialize(reader));
      reader.SetField<GoalsManager>(this, "m_simLoopEvents", reader.LoadedSaveVersion >= 160 ? (object) reader.ReadGenericAs<ISimLoopEvents>() : (object) (ISimLoopEvents) null);
      if (reader.LoadedSaveVersion >= 160)
        return;
      reader.RegisterResolvedMember<GoalsManager>(this, "m_simLoopEvents", typeof (ISimLoopEvents), true);
    }

    static GoalsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GoalsManager) obj).SerializeData(writer));
      GoalsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GoalsManager) obj).DeserializeData(reader));
    }
  }
}
