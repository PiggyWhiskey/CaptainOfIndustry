// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchNode
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.ResearchLab;
using Mafi.Core.Prototypes;
using Mafi.Core.UnlockingTree;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Research
{
  [GenerateSerializer(false, null, 0)]
  public class ResearchNode : IUnlockingNode<ResearchNode>, IUnlockingNode, IResearchNodeFriend
  {
    [DoNotSave(0, null)]
    private bool m_isBeingVisited;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Fix32 RemainingSteps
    {
      get => ((Fix32) this.Proto.TotalStepsRequired - this.StepsDone).Max((Fix32) 0);
    }

    public ResearchNodeProto Proto { get; private set; }

    /// <summary>
    /// How many steps we have already done to research this node.
    /// </summary>
    public Fix32 StepsDone { get; private set; }

    /// <summary>Current state of the research node.</summary>
    public ResearchNodeState State { get; private set; }

    public bool IsLockedByCondition => this.LockedByConditions.IsNotEmpty;

    [DoNotSaveCreateNewOnLoad(null, 0)]
    public Lyst<IResearchNodeUnlockingCondition> LockedByConditions { get; set; }

    /// <summary>Direct child nodes of the current node.</summary>
    public ImmutableArray<ResearchNode> Children { get; private set; }

    /// <summary>Direct parent nodes of the current node.</summary>
    public ImmutableArray<ResearchNode> Parents { get; private set; }

    public bool AnyParentCanUnlock => this.Proto.AnyParentCanUnlock;

    public ImmutableArray<IUnlockNodeUnit> Units => this.Proto.Units;

    public bool IsLockedByParents { get; private set; }

    public bool IsLocked => this.IsLockedByParents || this.IsLockedByCondition;

    public Option<ResearchLabProto> LabRequired { get; private set; }

    /// <summary>Progress done on the research in percents.</summary>
    public Percent ProgressInPerc { get; private set; }

    public Vector2i GridPosition => this.Proto.GridPosition;

    public bool IsNotAvailable()
    {
      if (this.Proto.IsNotAvailable)
        return true;
      foreach (IUnlockNodeUnit unit in this.Proto.Units)
      {
        if (!unit.HideInUI)
          return false;
      }
      return true;
    }

    /// <summary>
    /// Whether this node has it's tree of dependencies valid and hence can be enqueued.
    /// </summary>
    [DoNotSave(0, null)]
    public bool CanBeEnqueued { get; set; }

    /// <summary>
    /// Whether this node has it's direct dependencies satisfied and hence can be enqueued.
    /// </summary>
    [DoNotSave(0, null)]
    public bool CanBeEnqueuedDirect { get; set; }

    [DoNotSave(0, null)]
    public bool CanBeDequeued { get; set; }

    [DoNotSave(0, null)]
    public int IndexInQueue { get; set; }

    public ResearchNode(ResearchNodeProto nodeProto)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CLockedByConditions\u003Ek__BackingField = new Lyst<IResearchNodeUnlockingCondition>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CChildren\u003Ek__BackingField = ImmutableArray<ResearchNode>.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CParents\u003Ek__BackingField = ImmutableArray<ResearchNode>.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Proto = nodeProto.CheckNotNull<ResearchNodeProto>();
      this.State = ResearchNodeState.NotResearched;
      this.IsLockedByParents = nodeProto.Parents.Length != 0;
    }

    void IResearchNodeFriend.SetState(ResearchNodeState state)
    {
      this.State = state;
      if (this.State == ResearchNodeState.NotResearched || !this.IsLockedByParents)
        return;
      Log.Error(string.Format("IsLockedByParents = true while State is {0}", (object) this.State));
      this.IsLockedByParents = false;
    }

    void IResearchNodeFriend.SetIsLockedByParents(bool isLockedByParents)
    {
      this.IsLockedByParents = isLockedByParents;
    }

    void IResearchNodeFriend.IncStepsDone(Fix32 steps)
    {
      Assert.That<Fix32>(this.StepsDone + steps).IsLessOrEqual((Fix32) this.Proto.TotalStepsRequired);
      this.StepsDone += steps;
      this.ProgressInPerc = Percent.FromRatio(this.StepsDone, (Fix32) this.Proto.TotalStepsRequired).Min(Percent.Hundred);
    }

    void IResearchNodeFriend.ForceStepsToDone()
    {
      this.StepsDone += (Fix32) this.Proto.TotalStepsRequired;
      this.ProgressInPerc = Percent.Hundred;
    }

    void IResearchNodeFriend.BuildGraph(ImmutableArray<ResearchNode> nodes)
    {
      Lyst<ResearchNode> lyst1 = new Lyst<ResearchNode>();
      Lyst<ResearchNode> lyst2 = new Lyst<ResearchNode>();
      foreach (ResearchNode node in nodes)
      {
        ImmutableArray<ResearchNodeProto> parents = node.Proto.Parents;
        if (parents.Contains(this.Proto))
          lyst1.Add(node);
        parents = this.Proto.Parents;
        if (parents.Contains(node.Proto))
          lyst2.Add(node);
      }
      this.Children = lyst1.ToImmutableArray();
      this.Parents = lyst2.ToImmutableArray();
    }

    void IResearchNodeFriend.ClearRequiredLab() => this.LabRequired = Option<ResearchLabProto>.None;

    void IResearchNodeFriend.PopulateRequiredLab(Option<ResearchLabProto> labRequired)
    {
      if (this.m_isBeingVisited)
      {
        Log.Error("There is loop in the tree! Node '" + this.Proto.Id.Value + "' was already visited!");
      }
      else
      {
        this.m_isBeingVisited = true;
        if (this.LabRequired.HasValue && (labRequired.IsNone || labRequired.Value.TierIndex <= this.LabRequired.Value.TierIndex))
        {
          this.m_isBeingVisited = false;
        }
        else
        {
          if (labRequired.HasValue && (Mafi.Core.Prototypes.Proto) labRequired.Value == (Mafi.Core.Prototypes.Proto) null)
          {
            Log.Error(string.Format("labRequired set with null value on `{0}`.", (object) this.Proto));
            labRequired = Option<ResearchLabProto>.None;
          }
          this.LabRequired = labRequired;
          foreach (IUnlockNodeUnit unit in this.Proto.Units)
          {
            if (unit is ProtoUnlock protoUnlock)
            {
              ResearchLabProto researchLabProto = protoUnlock.UnlockedProtos.FirstOrDefault((Func<IProto, bool>) (x => x is ResearchLabProto)) as ResearchLabProto;
              if ((Mafi.Core.Prototypes.Proto) researchLabProto != (Mafi.Core.Prototypes.Proto) null)
              {
                labRequired = (Option<ResearchLabProto>) researchLabProto;
                break;
              }
            }
          }
          foreach (IResearchNodeFriend child in this.Children)
            child.PopulateRequiredLab(labRequired);
          this.m_isBeingVisited = false;
        }
      }
    }

    string IUnlockingNode<ResearchNode>.GetNodeId()
    {
      return "Ids.Research." + this.Proto.Id.Value.Substring("Research".Length);
    }

    public ResearchNode.InfoForUi GetInfo() => new ResearchNode.InfoForUi(this);

    public static void Serialize(ResearchNode value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResearchNode>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResearchNode.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<ResearchNode>.Serialize(this.Children, writer);
      writer.WriteBool(this.IsLockedByParents);
      Option<ResearchLabProto>.Serialize(this.LabRequired, writer);
      ImmutableArray<ResearchNode>.Serialize(this.Parents, writer);
      Percent.Serialize(this.ProgressInPerc, writer);
      writer.WriteGeneric<ResearchNodeProto>(this.Proto);
      writer.WriteInt((int) this.State);
      Fix32.Serialize(this.StepsDone, writer);
    }

    public static ResearchNode Deserialize(BlobReader reader)
    {
      ResearchNode researchNode;
      if (reader.TryStartClassDeserialization<ResearchNode>(out researchNode))
        reader.EnqueueDataDeserialization((object) researchNode, ResearchNode.s_deserializeDataDelayedAction);
      return researchNode;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Children = ImmutableArray<ResearchNode>.Deserialize(reader);
      this.IsLockedByParents = reader.ReadBool();
      this.LabRequired = Option<ResearchLabProto>.Deserialize(reader);
      this.LockedByConditions = new Lyst<IResearchNodeUnlockingCondition>();
      this.Parents = ImmutableArray<ResearchNode>.Deserialize(reader);
      this.ProgressInPerc = Percent.Deserialize(reader);
      this.Proto = reader.ReadGenericAs<ResearchNodeProto>();
      this.State = (ResearchNodeState) reader.ReadInt();
      this.StepsDone = Fix32.Deserialize(reader);
    }

    static ResearchNode()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResearchNode.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ResearchNode) obj).SerializeData(writer));
      ResearchNode.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ResearchNode) obj).DeserializeData(reader));
    }

    public readonly struct InfoForUi : IEquatable<ResearchNode.InfoForUi>
    {
      public readonly ResearchNodeState State;
      public readonly int IndexInQueue;
      public readonly bool CanBeEnqueued;
      public readonly bool CanBeEnqueuedDirect;
      public readonly bool CanBeDequeued;
      public readonly bool IsLockedByCondition;
      public readonly bool IsLockedByParents;

      public bool IsInQueue => this.IndexInQueue >= 0;

      public bool IsLocked => this.IsLockedByCondition || this.IsLockedByParents;

      public InfoForUi(ResearchNode node)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.State = node.State;
        this.IndexInQueue = node.IndexInQueue;
        this.CanBeEnqueued = node.CanBeEnqueued;
        this.CanBeEnqueuedDirect = node.CanBeEnqueuedDirect;
        this.CanBeDequeued = node.CanBeDequeued;
        this.IsLockedByCondition = node.IsLockedByCondition;
        this.IsLockedByParents = node.IsLockedByParents;
      }

      public bool Equals(ResearchNode.InfoForUi other)
      {
        return this.State == other.State && this.IndexInQueue == other.IndexInQueue && this.CanBeEnqueued == other.CanBeEnqueued && this.CanBeEnqueuedDirect == other.CanBeEnqueuedDirect && this.CanBeDequeued == other.CanBeDequeued && this.IsLockedByCondition == other.IsLockedByCondition && this.IsLockedByParents == other.IsLockedByParents;
      }

      public override bool Equals(object obj)
      {
        return obj is ResearchNode.InfoForUi other && this.Equals(other);
      }

      public override int GetHashCode()
      {
        return ((((((int) this.State * 397 ^ this.IndexInQueue) * 397 ^ this.CanBeEnqueued.GetHashCode()) * 397 ^ this.CanBeEnqueuedDirect.GetHashCode()) * 397 ^ this.CanBeDequeued.GetHashCode()) * 397 ^ this.IsLockedByCondition.GetHashCode()) * 397 ^ this.IsLockedByParents.GetHashCode();
      }
    }
  }
}
