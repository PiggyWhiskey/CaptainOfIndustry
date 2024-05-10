// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.ResearchLab;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.UnlockingTree;
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Research
{
  /// <summary>
  /// Manages all research. Mainly assigns research nodes to research labs and manages spending of resources.
  /// </summary>
  /// <remarks>Only one node can be research at the same time.</remarks>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class ResearchManager
  {
    /// <summary>
    /// Research nodes indexed by <see cref="T:Mafi.Core.Research.ResearchNodeProto.ID" />
    /// </summary>
    [DoNotSave(0, null)]
    private Dict<ResearchNodeProto.ID, ResearchNode> m_nodes;
    private readonly Lyst<ResearchNode> m_researchedNodes;
    private readonly Queueue<ResearchNode> m_researchQueue;
    private readonly ProtosDb m_protosDb;
    private readonly INodeUnlocker m_unlocker;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly IInstaBuildManager m_instaBuildManager;
    private readonly IMessageNotificationsManager m_messageNotificationsManager;
    private readonly Set<Mafi.Core.Buildings.ResearchLab.ResearchLab> m_researchLabs;
    [DoNotSave(0, null)]
    private ImmutableArray<IResearchUnlockingConditionManager> m_conditionsManagers;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ImmutableArray<ResearchNode> AllNodes { get; private set; }

    /// <summary>
    /// Currently researched node. Empty if nothing is research at the moment.
    /// </summary>
    public Option<ResearchNode> CurrentResearch { get; private set; }

    /// <summary>
    /// Steps of research that is consider optimal with the current set of research labs that player has.
    /// </summary>
    public int OptimalSteps { get; private set; }

    public bool HasActiveLab { get; private set; }

    public bool WasLabEverBuilt { get; private set; }

    public IIndexable<ResearchNode> ResearchedNodes
    {
      get => (IIndexable<ResearchNode>) this.m_researchedNodes;
    }

    public IIndexable<ResearchNode> ResearchQueue
    {
      get => (IIndexable<ResearchNode>) this.m_researchQueue;
    }

    public ResearchManager(
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      ISimLoopEvents simLoopEvents,
      INodeUnlocker unlocker,
      IInstaBuildManager instaBuildManager,
      IConstructionManager constructionManager,
      IEntitiesManager entitiesManager,
      IMessageNotificationsManager messageNotificationsManager,
      AllImplementationsOf<IResearchUnlockingConditionManager> unlocksManagers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_nodes = new Dict<ResearchNodeProto.ID, ResearchNode>();
      this.m_researchedNodes = new Lyst<ResearchNode>();
      this.m_researchQueue = new Queueue<ResearchNode>();
      this.m_researchLabs = new Set<Mafi.Core.Buildings.ResearchLab.ResearchLab>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_unlocker = unlocker;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_instaBuildManager = instaBuildManager;
      this.m_messageNotificationsManager = messageNotificationsManager;
      foreach (ResearchNodeProto nodeProto in protosDb.All<ResearchNodeProto>())
      {
        if (nodeProto.IsAvailable)
          this.m_nodes.Add(nodeProto.Id, new ResearchNode(nodeProto));
      }
      this.AllNodes = this.m_nodes.Values.ToImmutableArray<ResearchNode>();
      foreach (IResearchNodeFriend allNode in this.AllNodes)
        allNode.BuildGraph(this.AllNodes);
      ImmutableArray<ResearchNode> immutableArray = this.AllNodes;
      foreach (ResearchNode researchNode in immutableArray)
      {
        immutableArray = researchNode.Parents;
        if (immutableArray.IsEmpty)
          ((IResearchNodeFriend) researchNode).PopulateRequiredLab((Option<ResearchLabProto>) Option.None);
      }
      immutableArray = this.AllNodes;
      foreach (ResearchNode node in immutableArray)
      {
        if (node.Proto.IsUnlockedFromStart)
          this.markNodeResearched(node, true);
      }
      this.m_conditionsManagers = unlocksManagers.Implementations;
      foreach (IResearchUnlockingConditionManager conditionsManager in this.m_conditionsManagers)
        conditionsManager.Initialize(this);
      this.refreshQueueValues();
      simLoopEvents.Update.Add<ResearchManager>(this, new Action(this.simUpdate));
      constructionManager.EntityConstructed.Add<ResearchManager>(this, new Action<IStaticEntity>(this.onEntityConstructed));
      constructionManager.EntityStartedDeconstruction.Add<ResearchManager>(this, new Action<IStaticEntity>(this.onEntityStartedDeconstruction));
      entitiesManager.EntityEnabledChanged.Add<ResearchManager>(this, new Action<IEntity, bool>(this.onEnabledChanged));
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf(DependencyResolver resolver)
    {
      // ISSUE: object of a compiler-generated type is created
      this.m_nodes = this.AllNodes.Select(node => new \u003C\u003Ef__AnonymousType0<ResearchNodeProto.ID, ResearchNode>(node.Proto.Id, node)).ToDict(x => x.Id, x => x.node);
      Set<ResearchNodeProto> set = this.m_protosDb.All<ResearchNodeProto>().ToSet<ResearchNodeProto>();
      foreach (ResearchNodeProto nodeProto in set)
      {
        if (!this.m_nodes.ContainsKey(nodeProto.Id))
          this.m_nodes.Add(nodeProto.Id, new ResearchNode(nodeProto));
      }
      foreach (ResearchNode researchNode in this.m_nodes.Values)
      {
        if (!set.Contains(researchNode.Proto))
          this.m_nodes.Remove(researchNode.Proto.Id);
      }
      this.AllNodes = this.m_nodes.Values.ToImmutableArray<ResearchNode>();
      ImmutableArray<ResearchNode> immutableArray = this.AllNodes;
      foreach (IResearchNodeFriend researchNodeFriend in immutableArray)
        researchNodeFriend.BuildGraph(this.AllNodes);
      immutableArray = this.AllNodes;
      foreach (IResearchNodeFriend researchNodeFriend in immutableArray)
        researchNodeFriend.ClearRequiredLab();
      immutableArray = this.AllNodes;
      foreach (ResearchNode researchNode in immutableArray)
      {
        immutableArray = researchNode.Parents;
        if (immutableArray.IsEmpty)
          ((IResearchNodeFriend) researchNode).PopulateRequiredLab((Option<ResearchLabProto>) Option.None);
      }
      immutableArray = this.AllNodes;
      foreach (ResearchNode node in immutableArray)
      {
        if (node.Proto.IsUnlockedFromStart && node.State != ResearchNodeState.Researched)
        {
          this.markNodeResearched(node, true);
          ((IResearchNodeFriend) node).ForceStepsToDone();
        }
        else
        {
          bool flag = false;
          if (node.IsLockedByParents && this.isNotLockedByParents(node))
            ((IResearchNodeFriend) node).SetIsLockedByParents(false);
          if (node.State == ResearchNodeState.Researched)
          {
            IEnumerable<IProto> unlockedProtos = ProtoUnlock.GetUnlockedProtos(node.Units.AsEnumerable());
            if (this.m_unlockedProtosDb.IsLockedButAvailable((IProto) node.Proto))
              this.m_unlockedProtosDb.Unlock((Proto) node.Proto);
            foreach (IProto proto in unlockedProtos)
            {
              if (this.m_unlockedProtosDb.IsLocked(proto))
              {
                flag = true;
                break;
              }
            }
          }
          if (flag)
            this.m_unlocker.UnlockUnitsOf((IUnlockingNode) node);
        }
      }
      Option<ResearchNode> currentResearch = this.CurrentResearch;
      if (currentResearch.ValueOrNull?.Proto?.IsNotAvailable.GetValueOrDefault())
        this.StopResearch();
      currentResearch = this.CurrentResearch;
      ResearchNode valueOrNull = currentResearch.ValueOrNull;
      if ((valueOrNull != null ? (valueOrNull.State == ResearchNodeState.Researched ? 1 : 0) : 0) != 0)
      {
        this.CurrentResearch = (Option<ResearchNode>) Option.None;
        this.m_researchQueue.Clear();
      }
      this.m_conditionsManagers = resolver.ResolveAll<IResearchUnlockingConditionManager>().Implementations;
      foreach (IResearchUnlockingConditionManager conditionsManager in this.m_conditionsManagers)
        conditionsManager.Initialize(this);
      this.refreshQueueValues();
    }

    private void simUpdate()
    {
      this.HasActiveLab = this.m_researchLabs.Any<Mafi.Core.Buildings.ResearchLab.ResearchLab>((Func<Mafi.Core.Buildings.ResearchLab.ResearchLab, bool>) (x => x.IsEnabled && !x.IsBlockedOnAdvancedResearch));
    }

    private void onEntityConstructed(IStaticEntity entity)
    {
      if (!(entity is Mafi.Core.Buildings.ResearchLab.ResearchLab researchLab))
        return;
      this.WasLabEverBuilt = true;
      this.m_researchLabs.AddAndAssertNew(researchLab);
      this.updateDifficulty();
    }

    private void onEntityStartedDeconstruction(IStaticEntity entity)
    {
      if (!(entity is Mafi.Core.Buildings.ResearchLab.ResearchLab researchLab))
        return;
      this.m_researchLabs.RemoveAndAssert(researchLab);
      this.updateDifficulty();
    }

    private void onEnabledChanged(IEntity entity, bool isEnabled)
    {
      if (!(entity is Mafi.Core.Buildings.ResearchLab.ResearchLab))
        return;
      this.updateDifficulty();
    }

    private void updateDifficulty()
    {
      if (this.m_researchLabs.IsEmpty)
        this.OptimalSteps = 0;
      Duration duration = 360 * Duration.OneDay;
      Fix32 fix32 = (Fix32) 0;
      foreach (Mafi.Core.Buildings.ResearchLab.ResearchLab researchLab in this.m_researchLabs)
      {
        if (researchLab.IsEnabled)
          fix32 += researchLab.Prototype.StepsPerRecipe * duration.Ticks / researchLab.Prototype.DurationForRecipe.Ticks;
      }
      this.OptimalSteps = fix32.ToIntRounded();
    }

    /// <summary>
    /// Returns the corresponding <see cref="T:Mafi.Core.Research.ResearchNode" /> wrapper for the given <see cref="T:Mafi.Core.Research.ResearchNodeProto" />.
    /// </summary>
    public ResearchNode GetResearchNode(ResearchNodeProto nodeProto)
    {
      Option<ResearchNode> node = this.getNode(nodeProto.Id);
      Assert.That<Option<ResearchNode>>(node).HasValue<ResearchNode>();
      return node.Value;
    }

    public bool TryGetResearchNode(ResearchNodeProto.ID nodeProtoId, out ResearchNode node)
    {
      ResearchNode researchNode;
      if (this.m_nodes.TryGetValue(nodeProtoId, out researchNode))
      {
        node = researchNode;
        return true;
      }
      node = (ResearchNode) null;
      return false;
    }

    /// <summary>Starts research of the given node.</summary>
    public bool TryStartResearch(ResearchNodeProto nodeProto, out string errorMessage)
    {
      if (this.CurrentResearch.HasValue && (Proto) this.CurrentResearch.Value.Proto == (Proto) nodeProto)
      {
        errorMessage = string.Format("Requested research '{0}' is already in progress.", (object) nodeProto.Id);
        return false;
      }
      if (this.CurrentResearch.HasValue)
      {
        errorMessage = string.Format("Other research '{0}' is in progress.", (object) nodeProto.Id);
        return false;
      }
      Option<ResearchNode> node = this.getNode(nodeProto.Id);
      if (!node.HasValue)
      {
        errorMessage = "Unknown research node.";
        return false;
      }
      if (node.Value.IsLocked || node.Value.State != ResearchNodeState.NotResearched)
      {
        errorMessage = string.Format("Research '{0}' is not available yet!", (object) nodeProto.Id);
        return false;
      }
      this.CurrentResearch = node;
      ((IResearchNodeFriend) node.Value).SetState(ResearchNodeState.InProgress);
      errorMessage = "";
      if (this.m_instaBuildManager.IsInstaBuildEnabled)
        this.Cheat_FinishCurrent();
      this.refreshQueueValues();
      return true;
    }

    private bool isNodeEventuallyLockedByCondition(ResearchNode node)
    {
      if (node.IsLockedByCondition)
        return true;
      foreach (ResearchNode parent in node.Parents)
      {
        if (this.isNodeEventuallyLockedByCondition(parent))
          return true;
      }
      return false;
    }

    private bool canEnqueueResearch(ResearchNode node)
    {
      if (node.State != ResearchNodeState.NotResearched || node.IsLockedByCondition || this.CurrentResearch == node || this.m_researchQueue.Contains(node))
        return false;
      foreach (ResearchNode parent in node.Parents)
      {
        if (this.isNodeEventuallyLockedByCondition(parent))
          return false;
      }
      return true;
    }

    private bool canEnqueueResearchDirect(ResearchNode node)
    {
      if (node.State != ResearchNodeState.NotResearched || node.IsLockedByCondition || this.CurrentResearch.IsNone || this.CurrentResearch == node || this.m_researchQueue.Contains(node))
        return false;
      if (!node.IsLockedByParents)
        return true;
      foreach (ResearchNode parent in node.Parents)
      {
        if (!(parent == this.CurrentResearch) && parent.State != ResearchNodeState.Researched)
          return false;
      }
      return true;
    }

    private bool canDequeueResearch(ResearchNode node)
    {
      return !this.m_researchQueue.IsEmpty && (this.m_researchQueue.Last<ResearchNode>() == node || this.m_researchQueue.Contains(node));
    }

    public bool TryEnqueueResearch(ResearchNodeProto nodeProto)
    {
      Option<ResearchNode> node1 = this.getNode(nodeProto.Id);
      if (!node1.HasValue)
        return false;
      ResearchNode node2 = node1.Value;
      if (!this.canEnqueueResearch(node2))
        return false;
      foreach (ResearchNode parent in node2.Parents)
      {
        if (parent.State == ResearchNodeState.NotResearched && !this.m_researchQueue.Contains(parent))
        {
          if (!parent.IsLockedByParents && this.CurrentResearch.IsNone)
            this.TryStartResearch(parent.Proto, out string _);
          else if (!this.m_researchQueue.Contains(node2))
            this.TryEnqueueResearch(parent.Proto);
        }
      }
      this.m_researchQueue.Enqueue(node2);
      this.refreshQueueValues();
      return true;
    }

    public bool TryDequeueResearch(ResearchNodeProto nodeProto, bool calledRecursively = false)
    {
      Option<ResearchNode> node1 = this.getNode(nodeProto.Id);
      if (!node1.HasValue)
        return false;
      ResearchNode node2 = node1.Value;
      if (!this.canDequeueResearch(node2))
        return false;
      foreach (ResearchNode child in node2.Children)
        this.TryDequeueResearch(child.Proto, true);
      bool flag = this.m_researchQueue.TryRemove(node2);
      if (!calledRecursively)
        this.refreshQueueValues();
      return flag;
    }

    /// <summary>
    /// Stops current research or does nothing if no research is happening.
    /// </summary>
    public void StopResearch()
    {
      if (this.CurrentResearch.IsNone)
        return;
      Assert.That<ResearchNodeState>(this.CurrentResearch.Value.State).IsEqualTo<ResearchNodeState>(ResearchNodeState.InProgress);
      ((IResearchNodeFriend) this.CurrentResearch.Value).SetState(ResearchNodeState.NotResearched);
      this.CurrentResearch = (Option<ResearchNode>) Option.None;
      this.m_researchQueue.Clear();
      this.refreshQueueValues();
    }

    /// <summary>
    /// Finishes current research as if it would be normally researched.
    /// </summary>
    public void Cheat_FinishCurrent()
    {
      if (this.CurrentResearch.IsNone)
        return;
      ((IResearchNodeFriend) this.CurrentResearch.Value).IncStepsDone(this.CurrentResearch.Value.RemainingSteps);
      this.finishCurrentResearchNode();
    }

    /// <summary>
    /// Research lab use this method to report that the research step was done.
    /// </summary>
    public void ReportResearchStepsDone(Fix32 stepsDone)
    {
      if (this.CurrentResearch.IsNone)
        return;
      if (this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        this.Cheat_FinishCurrent();
      }
      else
      {
        ResearchNode researchNode = this.CurrentResearch.Value;
        Fix32 steps = stepsDone.Min(researchNode.RemainingSteps);
        ((IResearchNodeFriend) researchNode).IncStepsDone(steps);
        if (researchNode.RemainingSteps > 0)
          return;
        this.finishCurrentResearchNode();
        Log.GameProgress("Research done", additionalStrings: new KeyValuePair<string, string>[1]
        {
          Make.Kvp<string, string>("research_id", researchNode.Proto.Id.Value)
        });
      }
    }

    private void finishCurrentResearchNode()
    {
      if (this.CurrentResearch.IsNone)
        return;
      ResearchNode node = this.CurrentResearch.Value;
      this.markNodeResearched(node);
      if (node == this.CurrentResearch)
        this.CurrentResearch = (Option<ResearchNode>) Option.None;
      while (this.m_researchQueue.IsNotEmpty)
      {
        ResearchNode researchNode = this.m_researchQueue.Dequeue();
        if (!researchNode.Proto.IsNotAvailable)
        {
          string errorMessage;
          if (this.TryStartResearch(researchNode.Proto, out errorMessage))
            return;
          Log.Error("Failed to start queued research - " + errorMessage);
        }
      }
      this.refreshQueueValues();
    }

    private void refreshQueueValues()
    {
      foreach (ResearchNode node in this.m_nodes.Values)
      {
        node.CanBeDequeued = this.canDequeueResearch(node);
        node.CanBeEnqueued = !node.CanBeDequeued && this.canEnqueueResearch(node);
        node.CanBeEnqueuedDirect = !node.CanBeDequeued && this.canEnqueueResearchDirect(node);
        node.IndexInQueue = -1;
      }
      int num = 0;
      foreach (ResearchNode research in this.m_researchQueue)
      {
        research.IndexInQueue = num;
        ++num;
      }
    }

    private void markNodeResearched(ResearchNode node, bool noNotification = false)
    {
      Assert.That<bool>(node.IsLockedByParents).IsFalse();
      Assert.That<bool>(node.IsLockedByCondition).IsFalse();
      Assert.That<ResearchNodeState>(node.State).IsNotEqualTo<ResearchNodeState>(ResearchNodeState.Researched);
      ((IResearchNodeFriend) node).SetState(ResearchNodeState.Researched);
      this.m_researchedNodes.AddAssertNew(node);
      Set<Proto> unlockedProtos = new Set<Proto>();
      foreach (Proto unlockedProto in ProtoUnlock.GetUnlockedProtos(node.Units.Where((Func<IUnlockNodeUnit, bool>) (x => !x.HideInUI)).AsEnumerable<IUnlockNodeUnit>()))
      {
        if (!this.m_unlockedProtosDb.IsUnlocked(unlockedProto))
          unlockedProtos.Add(unlockedProto);
      }
      this.m_unlockedProtosDb.Unlock((Proto) node.Proto);
      this.m_unlocker.UnlockUnitsOf((IUnlockingNode) node);
      foreach (ResearchNode child in node.Children)
      {
        if (this.isNotLockedByParents(child) && child.State != ResearchNodeState.Researched)
          ((IResearchNodeFriend) child).SetIsLockedByParents(false);
      }
      if (noNotification)
        return;
      this.m_messageNotificationsManager.AddMessage((IMessageNotification) new ResearchFinishedMessage(node, unlockedProtos));
    }

    private bool isNotLockedByParents(ResearchNode node)
    {
      return !node.Proto.AnyParentCanUnlock ? node.Parents.All((Func<ResearchNode, bool>) (x => x.State == ResearchNodeState.Researched)) : node.Parents.Any((Func<ResearchNode, bool>) (x => x.State == ResearchNodeState.Researched));
    }

    public void OnSomeNodeUnlockedMaybe() => this.refreshQueueValues();

    private Option<ResearchNode> getNode(ResearchNodeProto.ID nodeProtoId)
    {
      ResearchNode node;
      if (this.m_nodes.TryGetValue(nodeProtoId, out node))
        return (Option<ResearchNode>) node;
      Log.Error(string.Format("Given research node {0} is not registered in the manager", (object) nodeProtoId));
      return (Option<ResearchNode>) Option.None;
    }

    public static void LockProtosFromResearchTree(Set<Proto> unlockedProtos, ProtosDb protosDb)
    {
      IEnumerable<ResearchNodeProto> source1 = protosDb.All<ResearchNodeProto>().Where<ResearchNodeProto>((Func<ResearchNodeProto, bool>) (node => !node.IsUnlockedFromStart));
      unlockedProtos.RemoveRange(ProtoUnlock.GetUnlockedProtos(source1.SelectMany<ResearchNodeProto, IUnlockNodeUnit>((Func<ResearchNodeProto, IEnumerable<IUnlockNodeUnit>>) (node => node.Units.AsEnumerable()))).OfType<Proto>());
      IEnumerable<ResearchNodeProto> source2 = protosDb.All<ResearchNodeProto>().Where<ResearchNodeProto>((Func<ResearchNodeProto, bool>) (node => node.IsUnlockedFromStart));
      unlockedProtos.AddRange(ProtoUnlock.GetUnlockedProtos(source2.SelectMany<ResearchNodeProto, IUnlockNodeUnit>((Func<ResearchNodeProto, IEnumerable<IUnlockNodeUnit>>) (node => node.Units.AsEnumerable()))).OfType<Proto>());
    }

    internal void Cheat_UnlockAllResearch()
    {
      foreach (ResearchNode allNode in this.AllNodes)
      {
        if (allNode.State != ResearchNodeState.Researched)
        {
          this.clearAllConditionsFrom(allNode);
          if (allNode.IsLockedByParents)
            ((IResearchNodeFriend) allNode).SetIsLockedByParents(false);
          this.markNodeResearched(allNode, true);
        }
      }
      this.refreshQueueValues();
    }

    private void clearAllConditionsFrom(ResearchNode node)
    {
      if (!node.IsLockedByCondition)
        return;
      foreach (IResearchUnlockingConditionManager conditionsManager in this.m_conditionsManagers)
        conditionsManager.RemoveConditionsIfCan(node);
      Assert.That<bool>(node.IsLockedByCondition).IsFalse();
    }

    public static void Serialize(ResearchManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResearchManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResearchManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<ResearchNode>.Serialize(this.AllNodes, writer);
      Option<ResearchNode>.Serialize(this.CurrentResearch, writer);
      writer.WriteBool(this.HasActiveLab);
      writer.WriteGeneric<IInstaBuildManager>(this.m_instaBuildManager);
      writer.WriteGeneric<IMessageNotificationsManager>(this.m_messageNotificationsManager);
      Lyst<ResearchNode>.Serialize(this.m_researchedNodes, writer);
      Set<Mafi.Core.Buildings.ResearchLab.ResearchLab>.Serialize(this.m_researchLabs, writer);
      Queueue<ResearchNode>.Serialize(this.m_researchQueue, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      writer.WriteInt(this.OptimalSteps);
      writer.WriteBool(this.WasLabEverBuilt);
    }

    public static ResearchManager Deserialize(BlobReader reader)
    {
      ResearchManager researchManager;
      if (reader.TryStartClassDeserialization<ResearchManager>(out researchManager))
        reader.EnqueueDataDeserialization((object) researchManager, ResearchManager.s_deserializeDataDelayedAction);
      return researchManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AllNodes = ImmutableArray<ResearchNode>.Deserialize(reader);
      this.CurrentResearch = Option<ResearchNode>.Deserialize(reader);
      this.HasActiveLab = reader.ReadBool();
      reader.SetField<ResearchManager>(this, "m_instaBuildManager", (object) reader.ReadGenericAs<IInstaBuildManager>());
      reader.SetField<ResearchManager>(this, "m_messageNotificationsManager", (object) reader.ReadGenericAs<IMessageNotificationsManager>());
      reader.RegisterResolvedMember<ResearchManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<ResearchManager>(this, "m_researchedNodes", (object) Lyst<ResearchNode>.Deserialize(reader));
      reader.SetField<ResearchManager>(this, "m_researchLabs", (object) Set<Mafi.Core.Buildings.ResearchLab.ResearchLab>.Deserialize(reader));
      reader.SetField<ResearchManager>(this, "m_researchQueue", (object) Queueue<ResearchNode>.Deserialize(reader));
      reader.SetField<ResearchManager>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.RegisterResolvedMember<ResearchManager>(this, "m_unlocker", typeof (INodeUnlocker), true);
      this.OptimalSteps = reader.ReadInt();
      this.WasLabEverBuilt = reader.ReadBool();
      reader.RegisterInitAfterLoad<ResearchManager>(this, "initSelf", InitPriority.Low);
    }

    static ResearchManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResearchManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ResearchManager) obj).SerializeData(writer));
      ResearchManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ResearchManager) obj).DeserializeData(reader));
    }
  }
}
