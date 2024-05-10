// Decompiled with JetBrains decompiler
// Type: Mafi.PathFinding.AStarPriorityQueue`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Mafi.PathFinding
{
  [DebuggerDisplay("Count={Count}")]
  public class AStarPriorityQueue<TNode> where TNode : AStarNode<TNode>
  {
    private readonly Heap<TNode> m_heap;

    public int Count => this.m_heap.Count;

    public bool IsEmpty => this.m_heap.IsEmpty;

    public bool IsNotEmpty => this.m_heap.IsNotEmpty;

    /// <summary>
    /// Number of nodes in this list that were explored from start node ( <see cref="P:Mafi.PathFinding.AStarNode`1.IsVisitedFromStart" /> set to true).
    /// </summary>
    public int VisitedFromStartSetSize { get; private set; }

    /// <summary>
    /// Number of nodes in this list that were explored from goal nodes ( <see cref="P:Mafi.PathFinding.AStarNode`1.IsVisitedFromStart" /> set to false).
    /// </summary>
    public int VisitedFromGoalSetSize { get; private set; }

    public void Insert(TNode node, Fix32 estimatedTotalCost)
    {
      Mafi.Assert.That<bool>(node.IsInitialized).IsTrue("Adding non-initialized node to queue.");
      Mafi.Assert.That<bool>(node.IsProcessed).IsFalse("Adding already processed node to queue.");
      Mafi.Assert.That<bool>(node.IsVisited).IsFalse("Adding already visited node to queue.");
      this.m_heap.Push(estimatedTotalCost, node);
      node.SetIsVisited();
      if (node.IsVisitedFromStart)
        ++this.VisitedFromStartSetSize;
      else
        ++this.VisitedFromGoalSetSize;
    }

    public KeyValuePair<Fix32, TNode> PeekMin() => this.m_heap.PeekMin();

    public KeyValuePair<Fix32, TNode> PopMin()
    {
      KeyValuePair<Fix32, TNode> keyValuePair = this.m_heap.PopMin();
      if (keyValuePair.Value.IsVisitedFromStart)
        --this.VisitedFromStartSetSize;
      else
        --this.VisitedFromGoalSetSize;
      return keyValuePair;
    }

    /// <summary>
    /// Clears this heap and calls <see cref="M:Mafi.PathFinding.AStarNode`1.Clear" /> on all saved nodes.
    /// </summary>
    public void Clear()
    {
      this.VisitedFromStartSetSize = 0;
      this.VisitedFromGoalSetSize = 0;
      foreach (KeyValuePair<Fix32, TNode> keyValuePair in this.m_heap)
        keyValuePair.Value.Clear();
      this.m_heap.Clear();
    }

    public void ClearSkipNode(TNode node)
    {
      this.VisitedFromStartSetSize = 0;
      this.VisitedFromGoalSetSize = 0;
      foreach (KeyValuePair<Fix32, TNode> keyValuePair in this.m_heap)
      {
        if ((object) keyValuePair.Value != (object) node)
          keyValuePair.Value.Clear();
      }
      this.m_heap.Clear();
    }

    /// <summary>Returns currently stored elements as an array slice.</summary>
    public ReadOnlyArraySlice<KeyValuePair<Fix32, TNode>> GetElements()
    {
      return this.m_heap.GetElements();
    }

    public AStarPriorityQueue()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_heap = new Heap<TNode>(1024, true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
