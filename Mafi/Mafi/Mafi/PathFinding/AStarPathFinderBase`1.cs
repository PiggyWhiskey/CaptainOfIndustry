// Decompiled with JetBrains decompiler
// Type: Mafi.PathFinding.AStarPathFinderBase`1
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.PathFinding
{
  /// <summary>
  /// Generic A*-based path finder. Implemented path-finding is time-sliced and bi-directional with non-reachability
  /// detection. Found paths might not be strictly optimal due to the double-direction search.
  /// 
  /// Derived classes must implement `Node` class derived from `AStarNode` and following methods:
  /// * <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.GetNeighbors(`0,Mafi.Collections.Lyst{`0})" />
  /// * <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.GetStepCost(`0,`0)" />
  /// * <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.EstPathCost(`0,`0)" />
  /// 
  /// Note that it is up to the derived classes to manage Nodes in efficient way and do their lookups.
  /// 
  /// Following methods should be used for the actual path finding:
  /// * <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.InitPathFinding(`0,`0,System.Boolean)" />
  /// * <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.ContinuePathFinding(System.Int32@)" />
  /// 
  /// If the <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.ContinuePathFinding(System.Int32@)" /> returns <see cref="F:Mafi.PathFinding.PathFinderResult.PathFound" />, the <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.ReconstructPath``1(Mafi.Collections.Lyst{``0},System.Func{`0,``0})" /> can be used to get the found path.
  /// 
  /// The <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.GetExploredTiles``1(Mafi.Collections.Lyst{``0},System.Func{`0,``0})" /> method can be used for visualization/debugging purposes.
  /// </summary>
  public abstract class AStarPathFinderBase<TNode> where TNode : AStarNode<TNode>
  {
    public int CurrentPfId;
    /// <summary>Visited nodes that awaiting processing.</summary>
    private readonly AStarPriorityQueue<TNode> m_toProcessList;
    /// <summary>
    /// Nodes that were already processed (all neighbors were expanded).
    /// </summary>
    private readonly Lyst<TNode> m_processedList;
    /// <summary>
    /// Temporary list of nodes used for neighbors collection and initialization.
    /// </summary>
    private readonly Lyst<TNode> m_nodesTmp;
    private readonly Lyst<TNode> m_nodesTmp2;
    private readonly Lyst<KeyValuePair<TNode, Fix32>> m_resultsTmp;
    private TNode m_cheapestNode;
    private TNode m_cheapestNbr;
    private int m_maxPathSteps;
    private bool m_pathFound;

    /// <summary>Current start node.</summary>
    protected TNode DistanceEstimationStartNode { get; private set; }

    /// <summary>Used for cost estimation in A*.</summary>
    protected TNode DistanceEstimationGoalNode { get; private set; }

    protected bool DisableBiDirectionalSearch { get; private set; }

    /// <summary>Number of performed steps of the current task.</summary>
    protected int TotalStepsCount { get; private set; }

    protected Fix32 CurrentLowestPathCost { get; private set; }

    /// <summary>
    /// Whether some path was already found. This may be true even when
    /// <see cref="F:Mafi.PathFinding.PathFinderResult.StillSearching" /> was returned when
    /// <see cref="P:Mafi.PathFinding.AStarPathFinderBase`1.KeepSearchingToFindOptimalPath" /> was set to true. In this case, found path may not be optimal
    /// yet.
    /// </summary>
    public bool SomePathAlreadyFound => (object) this.m_cheapestNbr != null;

    protected AStarPathFinderBase()
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_toProcessList = new AStarPriorityQueue<TNode>();
      this.m_processedList = new Lyst<TNode>(true);
      this.m_nodesTmp = new Lyst<TNode>(true);
      this.m_nodesTmp2 = new Lyst<TNode>(true);
      this.m_resultsTmp = new Lyst<KeyValuePair<TNode, Fix32>>(true);
    }

    protected abstract bool KeepSearchingToFindOptimalPath { get; }

    /// <summary>
    /// Retrieves all neighbors of the <paramref name="node" /> and saves them to <paramref name="outNeighbors" />.
    /// Derived class should only return viable neighbors.
    /// </summary>
    protected abstract void GetNeighbors(TNode node, Lyst<TNode> outNeighbors);

    /// <summary>
    /// Returns path cost between two neighboring nodes. The cost must be positive. The <see cref="F:System.Single.PositiveInfinity" /> can be returned to denote that the two nodes cannot be connected.
    /// </summary>
    protected abstract Fix32 GetStepCost(TNode node, TNode neighbor);

    /// <summary>
    /// Helper function that returns result of <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.GetStepCost(`0,`0)" /> and additionally asserts returned cost.
    /// </summary>
    private Fix32 getStepCost(TNode node, TNode neighbor)
    {
      Assert.That<TNode>(node).IsNotEqualTo<TNode>(neighbor);
      Fix32 stepCost = this.GetStepCost(node, neighbor);
      Assert.That<Fix32>(stepCost).IsPositive("Non-positive step cost!");
      return stepCost;
    }

    /// <summary>
    /// Returns total path cost estimate between two nodes.
    /// 
    /// IMPORTANT: In order to find an optimal path, the heuristic must be admissible i.e. it should never
    /// overestimate the actual path cost (it should not return higher distance than the resulting A* path finds).
    /// Non admissible heuristic will work too and in some cases might produce
    /// faster search, however, finding an optimal path is not guaranteed anymore.
    /// </summary>
    protected abstract Fix32 EstPathCost(TNode from, TNode to);

    /// <summary>Returns maximal path length in the number of nodes.</summary>
    protected abstract int GetMaxPathSteps(TNode start, TNode goal);

    /// <summary>
    /// Helper function that returns result of <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.EstPathCost(`0,`0)" /> and additionally asserts returned cost.
    /// </summary>
    private Fix32 estPathCost(TNode from, TNode to)
    {
      Fix32 fix32 = this.EstPathCost(from, to);
      Assert.That<Fix32>(fix32).IsNotNegative("Negative cost estimation!");
      Assert.That<Fix32>(fix32).IsNotEqualTo(Fix32.MaxValue);
      return fix32;
    }

    /// <summary>
    /// Should be called before <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.InitPathFinding(`0,`0,System.Boolean)" />.
    /// </summary>
    protected void ResetState()
    {
      this.m_toProcessList.Clear();
      this.m_processedList.ForEachAndClear((Action<TNode>) (node => node.Clear()));
      this.DistanceEstimationStartNode = default (TNode);
      this.DistanceEstimationGoalNode = default (TNode);
      this.CurrentLowestPathCost = Fix32.MaxValue;
      this.m_cheapestNode = default (TNode);
      this.m_cheapestNbr = default (TNode);
      this.TotalStepsCount = 0;
      this.m_pathFound = false;
    }

    /// <summary>Initializes path-finding with single goal node.</summary>
    protected void InitPathFinding(
      TNode startNode,
      TNode goalNode,
      bool disableBiDirectionalSearch = false)
    {
      this.m_nodesTmp.Clear();
      this.m_nodesTmp.Add(startNode);
      this.m_nodesTmp2.Clear();
      this.m_nodesTmp2.Add(goalNode);
      this.InitPathFinding(this.m_nodesTmp, startNode, this.m_nodesTmp2, goalNode, disableBiDirectionalSearch);
    }

    protected void InitPathFinding(
      TNode startNode,
      Lyst<TNode> goalNodesTmp,
      TNode distanceEstimationGoalNode,
      bool disableBiDirectionalSearch = false)
    {
      this.m_nodesTmp.Clear();
      this.m_nodesTmp.Add(startNode);
      this.InitPathFinding(this.m_nodesTmp, startNode, goalNodesTmp, distanceEstimationGoalNode, disableBiDirectionalSearch);
    }

    /// <summary>
    /// Initializes path-finding with the given start and goal nodes.
    /// </summary>
    protected void InitPathFinding(
      Lyst<TNode> startNodesTmp,
      TNode distanceEstimationStartNode,
      Lyst<TNode> goalNodesTmp,
      TNode distanceEstimationGoalNode,
      bool disableBiDirectionalSearch = false)
    {
      Assert.That<Lyst<TNode>>(goalNodesTmp).IsNotEmpty<TNode>();
      if (this.m_toProcessList.Count > 0 || (object) this.DistanceEstimationStartNode != null || this.TotalStepsCount != 0)
      {
        Log.Error("Forgot to reset state of path finder.");
        this.ResetState();
      }
      ++this.CurrentPfId;
      Assert.That<bool>(distanceEstimationStartNode.IsInitialized).IsFalse("Start node is already initialized. State was not cleared properly.");
      this.DistanceEstimationStartNode = distanceEstimationStartNode;
      this.DistanceEstimationGoalNode = distanceEstimationGoalNode;
      this.DisableBiDirectionalSearch = disableBiDirectionalSearch;
      foreach (TNode node in goalNodesTmp)
      {
        if (!node.IsVisited)
        {
          node.Initialize(false);
          if (disableBiDirectionalSearch)
          {
            node.SetIsVisited();
            node.SetIsProcessed();
            this.m_processedList.Add(node);
          }
          else
            this.m_toProcessList.Insert(node, this.estPathCost(node, distanceEstimationStartNode));
        }
      }
      foreach (TNode node in startNodesTmp)
      {
        if (node.IsInitialized)
        {
          if (!node.IsVisitedFromStart)
          {
            this.m_cheapestNode = this.m_cheapestNbr = node;
            this.m_maxPathSteps = 1;
            this.CurrentLowestPathCost = (Fix32) 0;
            this.m_processedList.Add(node);
            this.m_toProcessList.ClearSkipNode(node);
            return;
          }
        }
        else
        {
          node.Initialize(true);
          this.m_toProcessList.Insert(node, this.estPathCost(node, distanceEstimationGoalNode));
        }
      }
      this.m_maxPathSteps = this.GetMaxPathSteps(distanceEstimationStartNode, distanceEstimationGoalNode);
      if (!disableBiDirectionalSearch)
        this.m_maxPathSteps = this.m_maxPathSteps / 2 + 1;
      Assert.That<int>(this.m_maxPathSteps).IsPositive("Invalid max path steps.");
    }

    /// <summary>
    /// Adds new goal nodes to the ongoing or failed path finding run. This can be used to extend search without
    /// starting completely new rerun.
    /// </summary>
    protected void AddNewGoalNodes(Lyst<TNode> goalNodesTmp)
    {
      Assert.That<TNode>(this.DistanceEstimationStartNode).IsNotNull<TNode>("PF was not initialized.");
      Assert.That<TNode>(this.DistanceEstimationGoalNode).IsNotNull<TNode>("PF was not initialized.");
      Assert.That<bool>(this.m_pathFound).IsFalse("Path already found.");
      Assert.That<Fix32>(this.CurrentLowestPathCost).IsEqualTo(Fix32.MaxValue, "Path already found.");
      foreach (TNode node in goalNodesTmp)
      {
        if (node.IsVisited)
        {
          if (node.IsVisitedFromStart && node.CurrentCost < this.CurrentLowestPathCost)
          {
            this.CurrentLowestPathCost = node.CurrentCost;
            if (node.IsProcessed)
            {
              this.m_toProcessList.Clear();
            }
            else
            {
              this.m_toProcessList.ClearSkipNode(node);
              node.SetIsProcessed();
              this.m_processedList.Add(node);
            }
            this.m_cheapestNode = this.m_cheapestNbr = node;
          }
        }
        else if (!(this.CurrentLowestPathCost < Fix32.MaxValue))
        {
          node.Initialize(false);
          this.m_toProcessList.Insert(node, this.estPathCost(node, this.DistanceEstimationStartNode));
        }
      }
    }

    /// <summary>
    /// Runs the path-finding for given number of iterations. Returns status of the operation. Should be called again
    /// only when <see cref="F:Mafi.PathFinding.PathFinderResult.StillSearching" /> is returned.
    /// </summary>
    protected PathFinderResult ContinuePathFinding(ref int iterations)
    {
      Assert.That<int>(iterations).IsPositive();
      Assert.That<int>(this.m_maxPathSteps).IsPositive();
      Assert.That<bool>(this.m_pathFound).IsFalse("Invalid path finder state. Some path was already found.");
      while (this.m_toProcessList.IsNotEmpty && this.m_toProcessList.VisitedFromStartSetSize != 0 && (this.DisableBiDirectionalSearch || this.m_toProcessList.VisitedFromGoalSetSize != 0))
      {
        if (iterations <= 0)
          return PathFinderResult.StillSearching;
        --iterations;
        ++this.TotalStepsCount;
        KeyValuePair<Fix32, TNode> keyValuePair1 = this.m_toProcessList.PopMin();
        TNode node1 = keyValuePair1.Value;
        node1.SetIsProcessed();
        this.m_processedList.Add(node1);
        if ((object) this.m_cheapestNbr == null || !(keyValuePair1.Key >= this.CurrentLowestPathCost))
        {
          if (node1.PathLength <= this.m_maxPathSteps)
          {
            this.m_nodesTmp.Clear();
            this.GetNeighbors(node1, this.m_nodesTmp);
            this.m_resultsTmp.Clear();
            foreach (TNode node2 in this.m_nodesTmp)
            {
              Fix32 stepCost;
              if (this.extendPath(node1, node2, out stepCost))
                this.m_resultsTmp.Add(Make.Kvp<TNode, Fix32>(node2, stepCost));
            }
            foreach (KeyValuePair<TNode, Fix32> keyValuePair2 in this.m_resultsTmp)
            {
              TNode key = keyValuePair2.Key;
              Fix32 fix32_1 = keyValuePair2.Value;
              if (this.isValidPathConnection(node1, key))
              {
                Assert.That<Fix32>(fix32_1).IsNotEqualTo(Fix32.MaxValue);
                Fix32 fix32_2 = node1.CurrentCost + fix32_1 + key.CurrentCost;
                if (!(fix32_2 >= this.CurrentLowestPathCost))
                {
                  Assert.That<Fix32>(fix32_2).IsNotEqualTo(Fix32.MaxValue);
                  this.CurrentLowestPathCost = fix32_2;
                  this.m_cheapestNode = node1;
                  this.m_cheapestNbr = key;
                }
              }
            }
            if ((object) this.m_cheapestNbr != null && !this.KeepSearchingToFindOptimalPath)
              break;
          }
        }
        else
          break;
      }
      if ((object) this.m_cheapestNode == null)
        return PathFinderResult.PathDoesNotExist;
      this.m_pathFound = true;
      return PathFinderResult.PathFound;
    }

    /// <summary>
    /// Extends path-finding tree from <paramref name="currentNode" /> to <paramref name="neighborNode" /> if they are
    /// connectable according to the const function. Nodes that are not connectable will not be added to the <see cref="F:Mafi.PathFinding.AStarPathFinderBase`1.m_toProcessList" />.
    /// </summary>
    /// <returns>True if path was found. False otherwise.</returns>
    private bool extendPath(TNode currentNode, TNode neighborNode, out Fix32 stepCost)
    {
      Assert.That<TNode>(currentNode).IsNotEqualTo<TNode>(neighborNode);
      if (neighborNode.IsProcessed)
      {
        if (currentNode.IsVisitedFromStart == neighborNode.IsVisitedFromStart)
        {
          stepCost = Fix32.MaxValue;
          return false;
        }
        stepCost = this.getStepCost(currentNode, neighborNode);
        return stepCost != Fix32.MaxValue;
      }
      stepCost = this.getStepCost(currentNode, neighborNode);
      if (stepCost == Fix32.MaxValue)
        return false;
      Fix32 newTotalCost = currentNode.CurrentCost + stepCost;
      if (neighborNode.IsVisited)
      {
        if (currentNode.IsVisitedFromStart != neighborNode.IsVisitedFromStart)
          return true;
        if (newTotalCost < neighborNode.CurrentCost)
          neighborNode.SetParent(currentNode, newTotalCost);
      }
      else
      {
        TNode to = currentNode.IsVisitedFromStart ? this.DistanceEstimationGoalNode : this.DistanceEstimationStartNode;
        Fix32 estimatedTotalCost = newTotalCost + this.estPathCost(neighborNode, to);
        neighborNode.SetParent(currentNode, newTotalCost);
        this.m_toProcessList.Insert(neighborNode, estimatedTotalCost);
      }
      Assert.That<bool>(neighborNode.IsVisited).IsTrue();
      return false;
    }

    private bool isValidPathConnection(TNode node, TNode neighbor)
    {
      this.m_nodesTmp.Clear();
      this.GetNeighbors(neighbor, this.m_nodesTmp);
      foreach (TNode node1 in this.m_nodesTmp)
      {
        if ((object) node1 == (object) node)
          return true;
      }
      return false;
    }

    /// <summary>
    /// Reconstructs found path. This should be called only if <see cref="M:Mafi.PathFinding.AStarPathFinderBase`1.ContinuePathFinding(System.Int32@)" /> returned <see cref="F:Mafi.PathFinding.PathFinderResult.PathFound" />.
    /// </summary>
    protected void ReconstructPath<TResult>(Lyst<TResult> foundPath, Func<TNode, TResult> selector)
    {
      Assert.That<Lyst<TResult>>(foundPath).IsEmpty<TResult>("Output path list is not empty.");
      Assert.That<bool>((object) this.m_cheapestNode == null).IsEqualTo<bool>((object) this.m_cheapestNbr == null);
      if ((object) this.m_cheapestNode == null || (object) this.m_cheapestNbr == null)
        return;
      TNode node1 = this.m_cheapestNode;
      TNode cheapestNbr = this.m_cheapestNbr;
      if ((object) node1 == (object) cheapestNbr)
      {
        if (node1.HasParent)
        {
          Assert.That<bool>(node1.IsVisitedFromStart).IsTrue();
          foundPath.Count = node1.PathLength + 1;
          for (int index = foundPath.Count - 1; index >= 0; --index)
          {
            Assert.That<bool>(index == 0).IsEqualTo<bool>((object) node1 == (object) node1.ParentOnPath);
            foundPath[index] = selector(node1);
            node1 = node1.ParentOnPath;
          }
        }
        else
          foundPath.Add(selector(node1));
      }
      else
      {
        Assert.That<bool>(node1.IsVisitedFromStart).IsNotEqualTo<bool>(cheapestNbr.IsVisitedFromStart);
        int num = node1.PathLength + cheapestNbr.PathLength + 2;
        foundPath.Count = num;
        TNode node2;
        TNode node3;
        if (node1.IsVisitedFromStart)
        {
          node2 = node1;
          node3 = cheapestNbr;
        }
        else
        {
          node2 = cheapestNbr;
          node3 = node1;
        }
        int pathLength = node2.PathLength;
        for (int index = pathLength; index >= 0; --index)
        {
          Assert.That<bool>(index == 0).IsEqualTo<bool>((object) node2 == (object) node2.ParentOnPath, "Start node's parent should point to itself.");
          foundPath[index] = selector(node2);
          node2 = node2.ParentOnPath;
        }
        for (int index = pathLength + 1; index < num; ++index)
        {
          Assert.That<bool>(index + 1 == num).IsEqualTo<bool>((object) node3 == (object) node3.ParentOnPath, "Goal node's parent should point to itself.");
          foundPath[index] = selector(node3);
          node3 = node3.ParentOnPath;
        }
      }
    }

    /// <summary>
    /// Fills given list with nodes that were explored during last or current path-finding run.
    /// </summary>
    protected void GetExploredTiles<TResult>(Lyst<TResult> result, Func<TNode, TResult> selector)
    {
      Assert.That<Lyst<TResult>>(result).IsEmpty<TResult>();
      foreach (TNode processed in this.m_processedList)
        result.Add(selector(processed));
      foreach (KeyValuePair<Fix32, TNode> element in this.m_toProcessList.GetElements())
        result.Add(selector(element.Value));
    }
  }
}
