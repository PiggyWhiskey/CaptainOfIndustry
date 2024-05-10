// Decompiled with JetBrains decompiler
// Type: Mafi.PathFinding.AStarNode`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.PathFinding
{
  public abstract class AStarNode<TNode> where TNode : AStarNode<TNode>
  {
    private bool m_isVisitedFromStart;

    /// <summary>Current minimal cost of the path to this node.</summary>
    public Fix32 CurrentCost { get; private set; }

    /// <summary>
    /// Predecessor on the path from this node tot he start. It is used when reconstructing the path. Parent of the
    /// very first node on the path is set to itself by definition. This value is never null on a valid path.
    /// </summary>
    public TNode ParentOnPath { get; private set; }

    /// <summary>
    /// Number of steps to get to the origin of the path (start or goal).
    /// </summary>
    public int PathLength { get; private set; }

    /// <summary>
    /// Whether this node was explored from the start node or from the goal node.
    /// </summary>
    public bool IsVisitedFromStart
    {
      get
      {
        Assert.That<TNode>(this.ParentOnPath).IsNotNull<TNode>("Querying `IsVisitedFromStart` on non-initialized node.");
        return this.m_isVisitedFromStart;
      }
    }

    /// <summary>
    /// Whether this node was visited but its neighbors were not explored yet.
    /// </summary>
    public bool IsVisited { get; private set; }

    /// <summary>
    /// Whether this node is processed all its neighbors have been explored.
    /// </summary>
    public bool IsProcessed { get; private set; }

    /// <summary>
    /// Whether this node has parent that is not equal to itself. By our convention, start/goal nodes have parents
    /// set to themselves.
    /// </summary>
    public bool HasParent
    {
      get
      {
        Assert.That<TNode>(this.ParentOnPath).IsNotNull<TNode>("Querying `HasParent` on non-initialized node.");
        return (object) this.ParentOnPath != this;
      }
    }

    public bool IsInitialized => (object) this.ParentOnPath != null;

    public virtual void Initialize(bool isStartNode)
    {
      Assert.That<TNode>(this.ParentOnPath).IsNull<TNode>("Node is already initialized.");
      this.ParentOnPath = (TNode) this;
      this.CurrentCost = (Fix32) 0;
      this.PathLength = 0;
      this.m_isVisitedFromStart = isStartNode;
    }

    public virtual void SetParent(TNode parent, Fix32 newTotalCost)
    {
      Assert.That<Fix32>(newTotalCost).IsGreaterOrEqual(parent.CurrentCost);
      this.ParentOnPath = parent;
      this.CurrentCost = newTotalCost;
      this.m_isVisitedFromStart = this.ParentOnPath.IsVisitedFromStart;
      this.PathLength = this.ParentOnPath.PathLength + 1;
    }

    public void Clear()
    {
      this.ParentOnPath = default (TNode);
      this.IsVisited = false;
      this.IsProcessed = false;
    }

    public void SetIsVisited()
    {
      Assert.That<bool>(this.IsVisited).IsFalse("Node was already visited.");
      this.IsVisited = true;
    }

    public void SetIsProcessed()
    {
      Assert.That<bool>(this.IsProcessed).IsFalse("Node was already processed.");
      this.IsProcessed = true;
    }

    protected AStarNode()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
