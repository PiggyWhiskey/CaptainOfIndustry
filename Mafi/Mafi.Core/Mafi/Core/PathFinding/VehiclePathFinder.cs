// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.VehiclePathFinder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Roads;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.PathFinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Core.PathFinding
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class VehiclePathFinder : IVehiclePathFinder
  {
    /// <summary>
    /// Traveling on roads in 50% cheaper. Unfortunately this breaks A* metric admissibility (the estimation function may  over-estimate), but if we'd make non-road travel more expensive, terrain only PF will spend way too much
    /// time exploring nonsense (metric would work worse).
    /// </summary>
    public static readonly Fix32 ROAD_DISTANCE_MULT;
    public readonly ClearancePathabilityProvider ClearancePathabilityProvider;
    private readonly IRoadsManager m_roadsManager;
    private readonly IRandom m_random;
    private Option<IVehiclePathFindingTask> m_currentTask;
    private int m_currentRetryNumber;
    private int m_currentCapabilityIndex;
    private readonly Lyst<PfNode> m_resultPathTmp;
    private readonly Lyst<Option<RoadGraphPath>> m_connectingRoadsTmp;
    private readonly Dict<PfNode, Lyst<Tile2i>> m_startNodesToRawTiles;
    private readonly Dict<PfNode, Lyst<Tile2i>> m_goalNodesToRawTiles;
    /// <summary>Visited nodes that awaiting processing.</summary>
    private readonly VehiclePathFinder.AStarHeap m_toProcessList;
    private readonly Heap<VehiclePathFinder.RoadShortcut> m_roadShortcuts;
    private int m_shortcutsVisitedFromStart;
    private int m_shortcutsVisitedFromGoal;
    /// <summary>
    /// Nodes that were already processed (all neighbors were expanded).
    /// </summary>
    private readonly Lyst<PfNode> m_processedList;
    private readonly Lyst<Tile2i> m_goalsTmp;
    private readonly Lyst<ClearancePathabilityProvider.CapabilityChunkData> m_chunksToDestroy;
    private readonly Lyst<Tile2iSlim> m_roadConnTilesTmp;
    private Fix32 m_currentLowestPathCost;
    private Option<PfNode> m_foundPathNode;
    private Option<PfNode> m_foundPathNbr;
    private Option<RoadGraphPath> m_foundPathConnectingRoad;
    /// <summary>
    /// When this is set, path finding is also tracking the closest node from start set and terminates when new
    /// closer nodes are not found for <see cref="F:Mafi.Core.PathFinding.VehiclePathFinder.CLOSEST_NODE_IMPROVEMENTS_ATTEMPTS" /> attempts.
    /// This is used for cases when we need a path to the closes point even if the goal is blocked.
    /// </summary>
    private Option<PfNode> m_trackedClosestStartSetNodeToGoal;
    private Tile3i m_trackedClosestStartSetNodeToGoalTile;
    private bool m_closestNodeSearchFinalized;
    private int m_closestNodeImprovementAttemptsLeft;
    private int m_closestNodeImprovementResetsLeft;
    private readonly Stopwatch m_totalPfTime;
    private int m_vehiclePathFindingRecursionDepth;
    private readonly IRandom m_randomForGoalSelection;
    private Option<PfNode> m_currentRoadEntranceNode;

    public int CurrentPfId { get; private set; }

    public int TotalStepsCount { get; private set; }

    public Tile2i DistanceEstimationStartCoord { get; private set; }

    public Tile2i DistanceEstimationGoalCoord { get; private set; }

    public IPathabilityProvider PathabilityProvider
    {
      get => (IPathabilityProvider) this.ClearancePathabilityProvider;
    }

    public VehiclePathFinder(
      ClearancePathabilityProvider clearancePathabilityProvider,
      RandomProvider randomProvider,
      IRoadsManager roadsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_resultPathTmp = new Lyst<PfNode>();
      this.m_connectingRoadsTmp = new Lyst<Option<RoadGraphPath>>();
      this.m_startNodesToRawTiles = new Dict<PfNode, Lyst<Tile2i>>();
      this.m_goalNodesToRawTiles = new Dict<PfNode, Lyst<Tile2i>>();
      this.m_toProcessList = new VehiclePathFinder.AStarHeap();
      this.m_roadShortcuts = new Heap<VehiclePathFinder.RoadShortcut>();
      this.m_processedList = new Lyst<PfNode>();
      this.m_goalsTmp = new Lyst<Tile2i>(64, true);
      this.m_chunksToDestroy = new Lyst<ClearancePathabilityProvider.CapabilityChunkData>();
      this.m_roadConnTilesTmp = new Lyst<Tile2iSlim>(true);
      this.m_totalPfTime = new Stopwatch();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ClearancePathabilityProvider = clearancePathabilityProvider;
      this.m_roadsManager = roadsManager;
      this.m_random = randomProvider.GetSimRandomFor((object) this);
      this.m_randomForGoalSelection = (IRandom) new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 1UL, 2UL);
      this.ResetState();
    }

    public VehiclePathFinderInitResult InitVehiclePathFinding(
      IVehiclePathFindingTask task,
      ref int stepsLeft)
    {
      Mafi.Assert.That<Option<IVehiclePathFindingTask>>(this.m_currentTask).IsNone<IVehiclePathFindingTask>("State not cleared. Forgot to call ResetState?");
      Mafi.Assert.That<int>(task.MaxRetries).IsNotNegative();
      this.m_totalPfTime.Restart();
      this.m_currentRetryNumber = 0;
      VehiclePathFinderInitResult finderInitResult = VehiclePathFinderInitResult.Unknown;
      for (int currentRetryNumber = this.m_currentRetryNumber; currentRetryNumber <= task.MaxRetries; ++currentRetryNumber)
      {
        finderInitResult = this.initializeTask(task, currentRetryNumber, ref stepsLeft);
        ++this.m_currentRetryNumber;
        switch (finderInitResult)
        {
          case VehiclePathFinderInitResult.GoalAlreadyReached:
          case VehiclePathFinderInitResult.PathFound:
          case VehiclePathFinderInitResult.ReadyForPf:
            goto label_5;
          case VehiclePathFinderInitResult.NoStarts:
          case VehiclePathFinderInitResult.AllStartsInvalid:
          case VehiclePathFinderInitResult.NoGoals:
          case VehiclePathFinderInitResult.AllGoalsInvalid:
            continue;
          default:
            Mafi.Log.Error(string.Format("Invalid state: {0}", (object) finderInitResult));
            goto label_5;
        }
      }
label_5:
      this.m_totalPfTime.Stop();
      return finderInitResult;
    }

    /// <summary>
    /// Resets state and frees all nodes from internal lists. This should be called when PF is done and path is
    /// reconstructed.
    /// </summary>
    public void ResetState()
    {
      foreach (KeyValuePair<Fix32, PfNode> toProcess in this.m_toProcessList)
        this.clearNode(toProcess.Value);
      this.m_toProcessList.Clear();
      foreach (PfNode processed in this.m_processedList)
        this.clearNode(processed);
      this.m_processedList.Clear();
      foreach (ClearancePathabilityProvider.CapabilityChunkData capabilityChunkData in this.m_chunksToDestroy)
        capabilityChunkData.Parent.DestroyPfDataAt(capabilityChunkData.CapabilityIndex);
      this.m_chunksToDestroy.Clear();
      this.m_currentLowestPathCost = Fix32.MaxValue;
      this.TotalStepsCount = 0;
      this.m_foundPathNode = Option<PfNode>.None;
      this.m_foundPathNbr = Option<PfNode>.None;
      this.m_foundPathConnectingRoad = Option<RoadGraphPath>.None;
      this.m_currentRoadEntranceNode = Option<PfNode>.None;
      this.m_currentTask = Option<IVehiclePathFindingTask>.None;
      this.m_goalsTmp.Clear();
      this.m_startNodesToRawTiles.Clear();
      this.m_goalNodesToRawTiles.Clear();
      this.m_roadShortcuts.Clear();
      this.m_shortcutsVisitedFromStart = 0;
      this.m_shortcutsVisitedFromGoal = 0;
    }

    private void clearNode(PfNode node)
    {
      if (node.IsDestroyed)
        return;
      node.Clear();
      if (!node.ParentChunk.IsDirty)
        return;
      this.m_chunksToDestroy.Add(node.ParentChunk);
    }

    /// <summary>
    /// Returns cost estimate from <paramref name="current" /> to <paramref name="to" />.
    /// This is A* heuristic function that should not overestimate.
    /// The better the heuristic matches the final cost function, the better A* will perform.
    /// </summary>
    /// <remarks>
    /// * The heuristic function h(n) is admissible if h(n) is never larger than h*(n) or if h(n)
    ///   is always less or equal to the true value.
    /// * If A* employs an admissible heuristic and h(goal)=0, then we can argue that A* is admissible.
    /// * If the heuristic function is constantly tuned to be low with respect to the true cost,
    ///   i.e. h(n) ≤ h*(n), then you are going to get an optimal solution of 100%
    /// </remarks>
    private static Fix32 estPathCost(Tile2i from, PfNode current, Tile2i to)
    {
      Tile2i centerCoord = current.Area.CenterCoord;
      RelTile2i relTile2i = to - centerCoord;
      RelTile2i other = to - from;
      return (Fix32) relTile2i.AbsValue.Sum + relTile2i.PseudoCross(other).Abs().ToFix64().DivToFix32(100);
    }

    private VehiclePathFinderInitResult initializeTask(
      IVehiclePathFindingTask task,
      int retryNumber,
      ref int stepsLeft)
    {
      Mafi.Assert.That<Option<PfNode>>(this.m_foundPathNode).IsNone<PfNode>("Invalid path finder state. Some path was already found.");
      Mafi.Assert.That<Fix32>(this.m_currentLowestPathCost).IsEqualTo(Fix32.MaxValue, "Invalid path finder state. Some path was already found.");
      if (this.m_toProcessList.Count > 0 || this.TotalStepsCount != 0)
      {
        Mafi.Log.Error("Forgot to reset state of path finder.");
        this.ResetState();
      }
      ++this.CurrentPfId;
      this.m_currentTask = Option.Some<IVehiclePathFindingTask>(task);
      this.m_currentCapabilityIndex = this.ClearancePathabilityProvider.GetPathabilityClassIndex(task.PathFindingParams.PathabilityQueryMask);
      this.m_trackedClosestStartSetNodeToGoal = Option<PfNode>.None;
      this.m_closestNodeSearchFinalized = false;
      this.m_closestNodeImprovementAttemptsLeft = 0;
      this.m_vehiclePathFindingRecursionDepth = 0;
      stepsLeft -= 100;
      if (task.InitializeStartAndGoals(retryNumber))
        return VehiclePathFinderInitResult.GoalAlreadyReached;
      if (task.StartTiles.IsEmpty<Tile2i>())
        return VehiclePathFinderInitResult.NoStarts;
      if (task.GoalTiles.IsEmpty<Tile2i>())
        return VehiclePathFinderInitResult.NoGoals;
      int recomputedChunksCount = this.ClearancePathabilityProvider.RecomputedChunksCount;
      this.getNodes(task.StartTiles, task.PathFindingParams, this.m_startNodesToRawTiles);
      if (this.m_startNodesToRawTiles.IsEmpty)
      {
        Tile2i? alternativeValidTile = this.findAlternativeValidTile(task.StartTiles.First<Tile2i>(), task.PathFindingParams);
        stepsLeft -= 50;
        if (!alternativeValidTile.HasValue)
          return VehiclePathFinderInitResult.AllStartsInvalid;
        Option<PfNode> pfNodeAt = this.ClearancePathabilityProvider.GetOrCreatePfNodeAt(alternativeValidTile.Value, this.m_currentCapabilityIndex);
        if (pfNodeAt.IsNone)
        {
          Mafi.Log.Error("Failed to get start node at pathable tile.");
          return VehiclePathFinderInitResult.AllStartsInvalid;
        }
        Mafi.Assert.That<bool>(pfNodeAt.Value.IsDirty).IsFalse();
        Lyst<Tile2i> lyst;
        if (!this.m_startNodesToRawTiles.TryGetValue(pfNodeAt.Value, out lyst))
        {
          lyst = new Lyst<Tile2i>();
          this.m_startNodesToRawTiles.Add(pfNodeAt.Value, lyst);
        }
        lyst.Add(alternativeValidTile.Value);
      }
      this.getNodes(task.GoalTiles, task.PathFindingParams, this.m_goalNodesToRawTiles);
      if (this.m_goalNodesToRawTiles.IsEmpty && !task.NavigateClosebyIsSufficient)
        return VehiclePathFinderInitResult.AllGoalsInvalid;
      IRandom random = this.m_random;
      Tile2i tile2i = task.DistanceEstimationStartTile;
      int hashCode1 = tile2i.GetHashCode();
      tile2i = task.DistanceEstimationGoalTile;
      int hashCode2 = tile2i.GetHashCode();
      int currentPfId = this.CurrentPfId;
      random.SeedFast(hashCode1, hashCode2, currentPfId);
      this.DistanceEstimationStartCoord = task.PathFindingParams.ConvertToCornerTileSpace(task.DistanceEstimationStartTile);
      this.DistanceEstimationGoalCoord = task.PathFindingParams.ConvertToCornerTileSpace(task.DistanceEstimationGoalTile);
      foreach (PfNode key in this.m_goalNodesToRawTiles.Keys)
      {
        if (!key.IsVisited)
        {
          key.Initialize(false);
          this.m_toProcessList.Insert(key, VehiclePathFinder.estPathCost(this.DistanceEstimationGoalCoord, key, this.DistanceEstimationStartCoord));
        }
      }
      foreach (PfNode key in this.m_startNodesToRawTiles.Keys)
      {
        Option<PfNode> option = key.ParentNodeOnPath;
        if (option.HasValue)
        {
          if (!key.IsVisitedFromStart)
          {
            this.m_currentLowestPathCost = (Fix32) 0;
            this.m_foundPathNbr = option = (Option<PfNode>) key;
            this.m_foundPathNode = option;
            return VehiclePathFinderInitResult.PathFound;
          }
        }
        else
        {
          key.Initialize(true);
          this.m_toProcessList.Insert(key, VehiclePathFinder.estPathCost(this.DistanceEstimationGoalCoord, key, this.DistanceEstimationGoalCoord));
        }
      }
      int num = this.ClearancePathabilityProvider.RecomputedChunksCount - recomputedChunksCount;
      Mafi.Assert.That<int>(num).IsNotNegative();
      stepsLeft -= num * 25;
      return VehiclePathFinderInitResult.ReadyForPf;
    }

    private void getNodes(
      IIndexable<Tile2i> tiles,
      VehiclePathFindingParams pfParams,
      Dict<PfNode, Lyst<Tile2i>> outNodesToTiles)
    {
      foreach (Tile2i tile in tiles)
      {
        Tile2i cornerTileSpace = pfParams.ConvertToCornerTileSpace(tile);
        Option<PfNode> pfNodeAt = this.ClearancePathabilityProvider.GetOrCreatePfNodeAt(cornerTileSpace, this.m_currentCapabilityIndex);
        if (!pfNodeAt.IsNone)
        {
          Lyst<Tile2i> lyst;
          if (!outNodesToTiles.TryGetValue(pfNodeAt.Value, out lyst))
          {
            lyst = new Lyst<Tile2i>();
            outNodesToTiles.Add(pfNodeAt.Value, lyst);
          }
          lyst.Add(cornerTileSpace);
        }
      }
    }

    /// <summary>
    /// Returns PF node at given coord from the given capability index (or from the last used one).
    /// </summary>
    internal Option<PfNode> GetPfNodeAt(Tile2i coord, int? capabilityIndex = null)
    {
      return this.ClearancePathabilityProvider.GetPfNodeAt(coord, capabilityIndex ?? this.m_currentCapabilityIndex);
    }

    public Tile2i? FindClosestValidPosition(
      Tile2i coord,
      VehiclePathFindingParams pfParams,
      Predicate<PfNode> predicate = null)
    {
      Option<PfNode> closestValidPfNode = this.ClearancePathabilityProvider.FindClosestValidPfNode(coord, this.ClearancePathabilityProvider.GetPathabilityClassIndex(pfParams.PathabilityQueryMask), predicate);
      return !closestValidPfNode.HasValue ? new Tile2i?() : new Tile2i?(closestValidPfNode.Value.Area.CenterCoord);
    }

    private Tile2i? findAlternativeValidTile(Tile2i centerTile, VehiclePathFindingParams pfParams)
    {
      Tile2i cornerTileSpace = pfParams.ConvertToCornerTileSpace(centerTile);
      int num = pfParams.RequiredClearance.Value + 2;
      for (int index = 1; index <= num; ++index)
      {
        for (int y = -index; y <= index; ++y)
        {
          Tile2i t1 = cornerTileSpace + new RelTile2i(-index, y);
          if (isPathable(t1))
            return new Tile2i?(t1);
          Tile2i t2 = cornerTileSpace + new RelTile2i(index, y);
          if (isPathable(t2))
            return new Tile2i?(t2);
        }
        for (int x = -index + 1; x < index; ++x)
        {
          Tile2i t3 = cornerTileSpace + new RelTile2i(x, -index);
          if (isPathable(t3))
            return new Tile2i?(t3);
          Tile2i t4 = cornerTileSpace + new RelTile2i(x, index);
          if (isPathable(t4))
            return new Tile2i?(t4);
        }
      }
      return new Tile2i?();

      bool isPathable(Tile2i t)
      {
        return this.PathabilityProvider.IsPathableRaw(t, pfParams.PathabilityQueryMask);
      }
    }

    public PathFinderResult ContinueVehiclePathFinding(
      ref int iterations,
      bool newTaskStartedThisSimStep,
      IVehicleSurfaceProvider surfaceProvider,
      bool isFinalAttempt)
    {
      Mafi.Assert.That<bool>(this.m_closestNodeSearchFinalized).IsFalse();
      this.m_totalPfTime.Restart();
      ++this.m_vehiclePathFindingRecursionDepth;
      if (this.m_vehiclePathFindingRecursionDepth > 3)
      {
        Mafi.Log.Error("Potential infinite recursion in path-finding detected. Returning path not found.");
        return PathFinderResult.PathDoesNotExist;
      }
      PathFinderResult pathFinderResult = this.continuePathFinding(ref iterations, surfaceProvider, newTaskStartedThisSimStep);
      switch (pathFinderResult)
      {
        case PathFinderResult.StillSearching:
          if (this.m_currentTask.IsNone)
          {
            Mafi.Log.Error(string.Format("Current task in '{0}' is none. Returning path not found.", (object) pathFinderResult));
            return PathFinderResult.PathDoesNotExist;
          }
          if (isFinalAttempt && this.m_currentTask.Value.NavigateClosebyIsSufficient)
          {
            if (this.m_trackedClosestStartSetNodeToGoal.IsNone)
            {
              this.m_trackedClosestStartSetNodeToGoal = (Option<PfNode>) this.getClosestNodeToGoalFromNodesVisitedFromStart(surfaceProvider);
              this.m_trackedClosestStartSetNodeToGoalTile = this.extendTileWithHeight(this.m_trackedClosestStartSetNodeToGoal.Value.Area.CenterCoord, surfaceProvider);
            }
            pathFinderResult = this.finalizeSearchToClosest(surfaceProvider);
            goto case PathFinderResult.PathFound;
          }
          else
            goto case PathFinderResult.PathFound;
        case PathFinderResult.PathFound:
          if (pathFinderResult != PathFinderResult.PathFound)
            ;
          this.m_totalPfTime.Stop();
          --this.m_vehiclePathFindingRecursionDepth;
          return pathFinderResult;
        case PathFinderResult.PathDoesNotExist:
          if (this.m_currentTask.IsNone)
          {
            Mafi.Log.Error(string.Format("Current task in '{0}' is none.", (object) pathFinderResult));
            goto case PathFinderResult.PathFound;
          }
          else if (this.m_currentTask.Value.NavigateClosebyIsSufficient)
          {
            if (!this.m_closestNodeSearchFinalized)
            {
              this.m_trackedClosestStartSetNodeToGoal = (Option<PfNode>) this.getClosestNodeToGoalFromNodesVisitedFromStart(surfaceProvider);
              this.m_trackedClosestStartSetNodeToGoalTile = this.extendTileWithHeight(this.m_trackedClosestStartSetNodeToGoal.Value.Area.CenterCoord, surfaceProvider);
              this.m_closestNodeImprovementAttemptsLeft = 4000;
              this.m_closestNodeImprovementResetsLeft = 3;
              pathFinderResult = iterations <= 0 ? (!isFinalAttempt ? PathFinderResult.StillSearching : this.finalizeSearchToClosest(surfaceProvider)) : this.ContinueVehiclePathFinding(ref iterations, newTaskStartedThisSimStep, surfaceProvider, isFinalAttempt);
              goto case PathFinderResult.PathFound;
            }
            else
              goto case PathFinderResult.PathFound;
          }
          else
          {
            bool? nullable = this.TryExtendGoals();
            if (!nullable.HasValue)
            {
              pathFinderResult = PathFinderResult.PathDoesNotExist;
              goto case PathFinderResult.PathFound;
            }
            else if (nullable.Value)
            {
              pathFinderResult = PathFinderResult.PathFound;
              goto case PathFinderResult.PathFound;
            }
            else
            {
              iterations -= 50;
              pathFinderResult = iterations <= 0 ? PathFinderResult.StillSearching : this.ContinueVehiclePathFinding(ref iterations, newTaskStartedThisSimStep, surfaceProvider, isFinalAttempt);
              goto case PathFinderResult.PathFound;
            }
          }
        default:
          Mafi.Log.Error(string.Format("Invalid PF state: {0}", (object) pathFinderResult));
          goto case PathFinderResult.PathFound;
      }
    }

    /// <summary>
    /// Returns the closest processed PF node (to goal) visited from any start nodes.
    /// </summary>
    private PfNode getClosestNodeToGoalFromNodesVisitedFromStart(
      IVehicleSurfaceProvider surfaceProvider)
    {
      PfNode visitedFromStart = (PfNode) null;
      long num1 = long.MaxValue;
      Tile3i other = this.extendTileWithHeight(this.DistanceEstimationGoalCoord, surfaceProvider);
      foreach (PfNode processed in this.m_processedList)
      {
        if (processed.IsVisitedFromStart)
        {
          long num2 = this.extendTileWithHeight(processed.Area.CenterCoord, surfaceProvider).DistanceSqrTo(other);
          if (num2 < num1)
          {
            num1 = num2;
            visitedFromStart = processed;
          }
        }
      }
      if (visitedFromStart == null)
      {
        foreach (PfNode key in this.m_startNodesToRawTiles.Keys)
        {
          long num3 = this.extendTileWithHeight(key.Area.CenterCoord, surfaceProvider).DistanceSqrTo(other);
          if (num3 < num1)
          {
            num1 = num3;
            visitedFromStart = key;
          }
        }
      }
      Mafi.Assert.That<PfNode>(visitedFromStart).IsNotNull<PfNode>();
      return visitedFromStart;
    }

    /// <summary>
    /// Runs the path-finding for given number of iterations. Returns status of the operation. Should be called again
    /// only when <see cref="F:Mafi.PathFinding.PathFinderResult.StillSearching" /> is returned.
    /// </summary>
    private PathFinderResult continuePathFinding(
      ref int stepsLeft,
      IVehicleSurfaceProvider surfaceProvider,
      bool newTaskStartedThisSimStep)
    {
      Mafi.Assert.That<int>(stepsLeft).IsPositive();
      Mafi.Assert.That<Option<PfNode>>(this.m_foundPathNode).IsNone<PfNode>("Invalid path finder state. Some path was already found.");
      Mafi.Assert.That<Fix32>(this.m_currentLowestPathCost).IsEqualTo(Fix32.MaxValue, "Invalid path finder state. Some path was already found.");
      int recomputedChunksCount = this.ClearancePathabilityProvider.RecomputedChunksCount;
      Tile3i other = this.extendTileWithHeight(this.DistanceEstimationGoalCoord, surfaceProvider);
      for (int index = stepsLeft; index >= 0; --index)
      {
        int num1 = this.m_toProcessList.VisitedFromStartSetSize + this.m_shortcutsVisitedFromStart;
        int num2 = this.m_toProcessList.VisitedFromGoalSetSize + this.m_shortcutsVisitedFromGoal;
        if (this.m_trackedClosestStartSetNodeToGoal.HasValue)
        {
          if (num1 == 0)
            return this.finalizeSearchToClosest(surfaceProvider);
        }
        else if (num1 == 0 || num2 == 0)
          return PathFinderResult.PathDoesNotExist;
        if (stepsLeft <= 0)
          return PathFinderResult.StillSearching;
        --stepsLeft;
        ++this.TotalStepsCount;
        PfNode currNode = this.m_toProcessList.PopMin().Value;
        Mafi.Assert.That<bool>(currNode.IsDestroyed).IsFalse("PF is expanding destroyed node, node got destroyed in between ticks?");
        IIndexable<PfNode.Edge> allValidNeighbors = currNode.GetAllValidNeighbors();
        currNode.SetIsProcessed();
        this.m_processedList.Add(currNode);
        foreach (PfNode.Edge edge in allValidNeighbors)
        {
          Mafi.Assert.That<bool>(edge.Node.IsDestroyed).IsFalse("PF expanded into destroyed node.");
          Mafi.Assert.That<bool>(!newTaskStartedThisSimStep || !edge.Node.IsDirty).IsTrue("Dirty node return on the first PF round. This should not happen as all chunks should be available to recompute.");
          this.extendPathAndHandleResult(currNode, edge.Node, edge.Distance, Option<RoadGraphPath>.None);
        }
        int num3 = this.ClearancePathabilityProvider.RecomputedChunksCount - recomputedChunksCount;
        Mafi.Assert.That<int>(num3).IsNotNegative();
        stepsLeft -= num3 * 25;
        recomputedChunksCount = this.ClearancePathabilityProvider.RecomputedChunksCount;
        if (this.m_foundPathNode.HasValue)
          return PathFinderResult.PathFound;
        if (this.m_trackedClosestStartSetNodeToGoal.HasValue)
        {
          Tile3i tile3i = this.extendTileWithHeight(currNode.Area.CenterCoord, surfaceProvider);
          Tile3i setNodeToGoalTile = this.m_trackedClosestStartSetNodeToGoalTile;
          ThicknessTilesI abs = (tile3i.Height - setNodeToGoalTile.Height).Abs;
          long num4 = setNodeToGoalTile.DistanceSqrTo(other);
          if (abs < this.m_currentTask.Value.MaxNavigateClosebyHeightDifference && tile3i.DistanceSqrTo(other) < num4)
          {
            this.m_trackedClosestStartSetNodeToGoal = (Option<PfNode>) currNode;
            this.m_trackedClosestStartSetNodeToGoalTile = this.extendTileWithHeight(this.m_trackedClosestStartSetNodeToGoal.Value.Area.CenterCoord, surfaceProvider);
            this.m_closestNodeImprovementAttemptsLeft = 4000;
          }
          else
          {
            --this.m_closestNodeImprovementAttemptsLeft;
            if (this.m_closestNodeImprovementAttemptsLeft <= 0)
            {
              if (!(num4 > this.m_currentTask.Value.MaxNavigateClosebyDistance.Squared) && !(abs > this.m_currentTask.Value.MaxNavigateClosebyHeightDifference) || this.m_closestNodeImprovementResetsLeft < 0)
                return this.finalizeSearchToClosest(surfaceProvider);
              this.m_closestNodeSearchFinalized = false;
              this.m_closestNodeImprovementAttemptsLeft = 4000;
              --this.m_closestNodeImprovementResetsLeft;
            }
          }
        }
      }
      Mafi.Log.Error("This should not happen: path-finding loop failed to exit.");
      return PathFinderResult.PathDoesNotExist;
    }

    private PathFinderResult finalizeSearchToClosest(IVehicleSurfaceProvider surfaceProvider)
    {
      Mafi.Assert.That<bool>(this.m_closestNodeSearchFinalized).IsFalse();
      Mafi.Assert.That<Option<PfNode>>(this.m_trackedClosestStartSetNodeToGoal).HasValue<PfNode>();
      Mafi.Assert.That<bool>(this.m_trackedClosestStartSetNodeToGoal.Value.IsVisitedFromStart).IsTrue();
      this.m_closestNodeSearchFinalized = true;
      PfNode key = this.m_trackedClosestStartSetNodeToGoal.Value;
      Tile3i other = this.extendTileWithHeight(this.DistanceEstimationGoalCoord, surfaceProvider);
      Tile3i setNodeToGoalTile = this.m_trackedClosestStartSetNodeToGoalTile;
      if ((other.Height - setNodeToGoalTile.Height).Abs > this.m_currentTask.Value.MaxNavigateClosebyHeightDifference || setNodeToGoalTile.DistanceSqrTo(other) > this.m_currentTask.Value.MaxNavigateClosebyDistance.Squared)
      {
        if (DebugGameRendererConfig.SaveVehiclePathFindingFailuresClosebyApproach)
        {
          Tile2i from = this.DistanceEstimationStartCoord.Min(this.DistanceEstimationGoalCoord);
          Tile2i to = this.DistanceEstimationStartCoord.Max(this.DistanceEstimationGoalCoord);
          foreach (PfNode processed in this.m_processedList)
          {
            from = from.Min(processed.Area.Origin);
            to = to.Max(processed.Area.Origin + processed.Area.Size);
          }
          DebugGameMapDrawing debugGameMapDrawing1 = this.DrawProcessedPfNodes(DebugGameRenderer.DrawGameImage(from, to, 30));
          Tile2f centerTile2f1 = this.DistanceEstimationStartCoord.CenterTile2f;
          Tile2i tile2i = this.DistanceEstimationGoalCoord;
          Tile2f centerTile2f2 = tile2i.CenterTile2f;
          ColorRgba darkGreen = ColorRgba.DarkGreen;
          RelTile1f one = RelTile1f.One;
          DebugGameMapDrawing debugGameMapDrawing2 = debugGameMapDrawing1.DrawArrow(centerTile2f1, centerTile2f2, darkGreen, one);
          tile2i = this.DistanceEstimationGoalCoord;
          Tile2f centerTile2f3 = tile2i.CenterTile2f;
          RelTile1i roundedRelTile1i = (this.m_currentTask.Value.MaxNavigateClosebyDistance / 2).RoundedRelTile1i;
          ColorRgba color = ColorRgba.DarkGreen.SetA((byte) 127);
          DebugGameMapDrawing debugGameMapDrawing3 = debugGameMapDrawing2.DrawCircle(centerTile2f3, roundedRelTile1i, color);
          tile2i = this.DistanceEstimationStartCoord;
          Tile2f centerTile2f4 = tile2i.CenterTile2f;
          ColorRgba green = ColorRgba.Green;
          DebugGameMapDrawing debugGameMapDrawing4 = debugGameMapDrawing3.DrawCross(centerTile2f4, green).DrawCross(this.m_trackedClosestStartSetNodeToGoal.Value.Area.CenterCoordF, ColorRgba.Blue);
          tile2i = this.DistanceEstimationGoalCoord;
          Tile2f centerTile2f5 = tile2i.CenterTile2f;
          ColorRgba orange = ColorRgba.Orange;
          DebugGameMapDrawing debugGameMapDrawing5 = debugGameMapDrawing4.DrawCross(centerTile2f5, orange);
          Tile2f centerCoordF = this.m_trackedClosestStartSetNodeToGoal.Value.Area.CenterCoordF;
          tile2i = this.DistanceEstimationGoalCoord;
          Tile2f centerTile2f6 = tile2i.CenterTile2f;
          ColorRgba red = ColorRgba.Red;
          debugGameMapDrawing5.DrawLine(centerCoordF, centerTile2f6, red).SaveMapAsTga("PF_fail_not_close_enough");
        }
        return PathFinderResult.PathDoesNotExist;
      }
      this.m_goalNodesToRawTiles.Add(key, key.Area.EnumerateTiles().ToLyst<Tile2i>());
      this.m_foundPathNode = (Option<PfNode>) key;
      this.m_foundPathNbr = (Option<PfNode>) key;
      this.m_foundPathConnectingRoad = Option<RoadGraphPath>.None;
      return PathFinderResult.PathFound;
    }

    private void extendPathAndHandleResult(
      PfNode currNode,
      PfNode neighborNode,
      Fix32 edgeDistance,
      Option<RoadGraphPath> roadConnection)
    {
      if (!this.extendPath(currNode, neighborNode, edgeDistance, roadConnection))
        return;
      Fix32 fix32 = currNode.CurrentCost + edgeDistance + neighborNode.CurrentCost;
      if (!(fix32 < this.m_currentLowestPathCost))
        return;
      this.m_currentLowestPathCost = fix32;
      this.m_foundPathNode = (Option<PfNode>) currNode;
      this.m_foundPathNbr = (Option<PfNode>) neighborNode;
      this.m_foundPathConnectingRoad = roadConnection;
    }

    /// <summary>
    /// Extends path from <paramref name="currNode" /> to <paramref name="neighborNode" />. Returns whether a path was found.
    /// </summary>
    private bool extendPath(
      PfNode currNode,
      PfNode neighborNode,
      Fix32 edgeDistance,
      Option<RoadGraphPath> roadConnection)
    {
      Mafi.Assert.That<PfNode>(currNode).IsNotEqualTo<PfNode>(neighborNode);
      if (neighborNode.IsProcessed)
      {
        Mafi.Assert.That<bool>(neighborNode.IsVisited).IsTrue();
        return currNode.IsVisitedFromStart != neighborNode.IsVisitedFromStart;
      }
      Fix32 newTotalCost = currNode.CurrentCost + edgeDistance;
      if (neighborNode.IsVisited)
      {
        if (currNode.IsVisitedFromStart != neighborNode.IsVisitedFromStart)
          return true;
        if (newTotalCost < neighborNode.CurrentCost)
          neighborNode.TrySetParent(currNode, newTotalCost, roadConnection).AssertTrue("Failed to set parent, given neighbor does not exist.");
        return false;
      }
      neighborNode.TrySetParent(currNode, newTotalCost, roadConnection).AssertTrue("Failed to set parent, given neighbor does not exist.");
      Fix32 fix32 = newTotalCost;
      Fix32 estimatedTotalCost = !currNode.IsVisitedFromStart ? fix32 + VehiclePathFinder.estPathCost(this.DistanceEstimationGoalCoord, neighborNode, this.DistanceEstimationStartCoord) : fix32 + VehiclePathFinder.estPathCost(this.DistanceEstimationStartCoord, neighborNode, this.DistanceEstimationGoalCoord);
      this.m_toProcessList.Insert(neighborNode, estimatedTotalCost);
      Mafi.Assert.That<bool>(neighborNode.IsVisited).IsTrue();
      return false;
    }

    private Tile3i extendTileWithHeight(Tile2i position, IVehicleSurfaceProvider surfaceProvider)
    {
      return position.ExtendHeight(surfaceProvider.GetVehicleSurfaceAt(position, out bool _).TilesHeightRounded);
    }

    /// <summary>
    /// Adds new goal nodes to the ongoing or failed path finding run. This can be used to extend search without
    /// starting completely new rerun.
    /// </summary>
    public bool? TryExtendGoals()
    {
      if (this.m_currentTask.IsNone)
      {
        Mafi.Log.Error("Failed to extend goals, no active task!");
        return new bool?();
      }
      for (int currentRetryNumber = this.m_currentRetryNumber; currentRetryNumber <= this.m_currentTask.Value.MaxRetries; ++currentRetryNumber)
      {
        bool? nullable = this.extendGoalsOfCurrentTask(this.m_currentRetryNumber);
        ++this.m_currentRetryNumber;
        if (nullable.HasValue)
          return new bool?(nullable.Value);
      }
      return new bool?();
    }

    private bool? extendGoalsOfCurrentTask(int retryNumber)
    {
      Mafi.Assert.That<Option<IVehiclePathFindingTask>>(this.m_currentTask).HasValue<IVehiclePathFindingTask>();
      Mafi.Assert.That<Option<PfNode>>(this.m_foundPathNode).IsNone<PfNode>("Path already found.");
      Mafi.Assert.That<Fix32>(this.m_currentLowestPathCost).IsEqualTo(Fix32.MaxValue, "Path already found.");
      if (this.m_currentTask.Value.InitializeStartAndGoals(retryNumber))
        return new bool?(true);
      if (this.m_currentTask.Value.GoalTiles.IsEmpty<Tile2i>())
        return new bool?();
      this.getNodes(this.m_currentTask.Value.GoalTiles, this.m_currentTask.Value.PathFindingParams, this.m_goalNodesToRawTiles);
      bool flag = false;
      foreach (PfNode key in this.m_goalNodesToRawTiles.Keys)
      {
        if (key.IsVisited)
        {
          if (key.IsVisitedFromStart)
          {
            flag = true;
            if (key.CurrentCost < this.m_currentLowestPathCost)
            {
              this.m_currentLowestPathCost = key.CurrentCost;
              this.m_foundPathNode = this.m_foundPathNbr = (Option<PfNode>) key;
              this.m_foundPathConnectingRoad = Option<RoadGraphPath>.None;
            }
          }
        }
        else if (!(this.m_currentLowestPathCost < Fix32.MaxValue))
        {
          key.Initialize(false);
          this.m_toProcessList.Insert(key, VehiclePathFinder.estPathCost(this.DistanceEstimationGoalCoord, key, this.DistanceEstimationStartCoord));
          flag = true;
        }
      }
      return !flag ? new bool?() : new bool?(this.m_currentLowestPathCost < Fix32.MaxValue);
    }

    public bool TryReconstructFoundPath(
      out IVehiclePathSegment firstSegment,
      out Tile2i goalTileRaw)
    {
      firstSegment = (IVehiclePathSegment) null;
      if (this.m_currentTask.IsNone)
      {
        Mafi.Log.Error("Failed to reconstruct path, no active task!");
        goalTileRaw = new Tile2i();
        return false;
      }
      IVehiclePathFindingTask vehiclePathFindingTask = this.m_currentTask.Value;
      if (this.m_foundPathNode.IsNone)
      {
        Mafi.Assert.That<IIndexable<Tile2i>>(vehiclePathFindingTask.GoalTiles).IsNotEmpty<Tile2i>();
        VehiclePathFindingParams pathFindingParams = this.m_currentTask.Value.PathFindingParams;
        long num1 = long.MaxValue;
        Tile2i tile2i = vehiclePathFindingTask.PathFindingParams.ConvertToCornerTileSpace(vehiclePathFindingTask.GoalTiles.First<Tile2i>());
        foreach (Tile2i goalTile in vehiclePathFindingTask.GoalTiles)
        {
          Tile2i cornerTileSpace = vehiclePathFindingTask.PathFindingParams.ConvertToCornerTileSpace(goalTile);
          if (this.PathabilityProvider.IsPathableRaw(cornerTileSpace, pathFindingParams.PathabilityQueryMask))
          {
            long num2 = goalTile.DistanceSqrTo(vehiclePathFindingTask.DistanceEstimationStartTile);
            if (num2 < num1)
            {
              num1 = num2;
              tile2i = cornerTileSpace;
            }
          }
        }
        goalTileRaw = tile2i;
        return false;
      }
      this.m_resultPathTmp.Clear();
      this.m_connectingRoadsTmp.Clear();
      if (!this.TryReconstructFoundPath(this.m_resultPathTmp, this.m_connectingRoadsTmp))
      {
        Mafi.Assert.Fail("Failed to reconstruct nodes.");
        goalTileRaw = vehiclePathFindingTask.PathFindingParams.ConvertToCornerTileSpace(vehiclePathFindingTask.GoalTiles.FirstOrDefault<Tile2i>());
        return false;
      }
      if (this.m_resultPathTmp.IsEmpty || this.m_resultPathTmp.Count != this.m_connectingRoadsTmp.Count + 1)
      {
        Mafi.Assert.Fail("Invalid path reconstruction.");
        goalTileRaw = vehiclePathFindingTask.PathFindingParams.ConvertToCornerTileSpace(vehiclePathFindingTask.GoalTiles.FirstOrDefault<Tile2i>());
        return false;
      }
      Lyst<Tile2i> list1;
      if (!this.m_goalNodesToRawTiles.TryGetValue(this.m_resultPathTmp.Last, out list1))
      {
        Mafi.Assert.Fail("No goal tiles found in a goal node. Wrong goal node?");
        goalTileRaw = vehiclePathFindingTask.PathFindingParams.ConvertToCornerTileSpace(vehiclePathFindingTask.GoalTiles.FirstOrDefault<Tile2i>());
        return false;
      }
      VehicleTerrainPathSegment terrainPathSegment = new VehicleTerrainPathSegment();
      Mafi.Assert.That<Lyst<Tile2i>>(list1).IsNotEmpty<Tile2i>();
      int count = list1.Count;
      Lyst<Tile2i> goalsTmp = this.m_goalsTmp;
      goalsTmp.Clear();
      goalsTmp.AddRange(list1);
      foreach (PfNode.Edge currentNeighbor in this.m_resultPathTmp.Last.CurrentNeighbors)
      {
        Lyst<Tile2i> list2;
        if (this.m_goalNodesToRawTiles.TryGetValue(currentNeighbor.Node, out list2))
          goalsTmp.AddRange(list2);
      }
      this.m_randomForGoalSelection.SeedFast(goalsTmp.Count, this.CurrentPfId);
      int index1 = this.m_randomForGoalSelection.NextInt(goalsTmp.Count);
      if (index1 >= count)
      {
        int expected = count;
        foreach (PfNode.Edge currentNeighbor in this.m_resultPathTmp.Last.CurrentNeighbors)
        {
          Lyst<Tile2i> lyst;
          if (this.m_goalNodesToRawTiles.TryGetValue(currentNeighbor.Node, out lyst))
          {
            expected += lyst.Count;
            if (index1 < expected)
            {
              this.m_resultPathTmp.Add(currentNeighbor.Node);
              this.m_connectingRoadsTmp.Add(Option<RoadGraphPath>.None);
              break;
            }
          }
        }
        Mafi.Assert.That<int>(index1).IsLess(expected);
      }
      goalTileRaw = goalsTmp[index1];
      terrainPathSegment.PathRawReversed.Add(goalTileRaw);
      goalsTmp.Clear();
      if (this.m_resultPathTmp.Count == 1)
      {
        firstSegment = (IVehiclePathSegment) terrainPathSegment;
        return true;
      }
      Vector2i vector2i = goalTileRaw.Vector2i;
      PfNode.Edge edge = this.m_resultPathTmp.Last.GetEdgeTo(this.m_resultPathTmp.PreLast);
      for (int index2 = this.m_resultPathTmp.Count - 1; index2 >= 1; --index2)
      {
        Option<RoadGraphPath> option = this.m_connectingRoadsTmp[index2 - 1];
        RoadGraphPath roadConn = option.ValueOrNull;
        if (roadConn != null)
        {
          VehiclePathFindingParams pathFindingParams = this.m_currentTask.Value.PathFindingParams;
          terrainPathSegment.PathRawReversed.Add(pathFindingParams.ConvertToCornerTileSpace(roadConn.GoalTile));
          VehicleRoadPathSegment vehicleRoadPathSegment = new VehicleRoadPathSegment();
          vehicleRoadPathSegment.Path = roadConn.Path;
          vehicleRoadPathSegment.NextSegment = (Option<IVehiclePathSegment>) (IVehiclePathSegment) terrainPathSegment;
          terrainPathSegment = new VehicleTerrainPathSegment();
          terrainPathSegment.NextSegment = (Option<IVehiclePathSegment>) (IVehiclePathSegment) vehicleRoadPathSegment;
          if (roadConn.Path.First.Entity is RoadEntranceEntity entity)
          {
            int index3 = entity.Prototype.TerrainConnections.IndexOf((Predicate<LaneTerrainConnectionSpec>) (x => x.LaneIndex == roadConn.Path.First.LaneIndex));
            if (index3 >= 0)
            {
              RoadTerrainConnection terrainConnection = entity.GetRoadTerrainConnection(index3);
              terrainPathSegment.PathRawReversed.Add(pathFindingParams.ConvertToCornerTileSpace(terrainConnection.TerrainTile));
            }
          }
          if (index2 >= 2)
          {
            option = this.m_connectingRoadsTmp[index2 - 2];
            if (option.IsNone)
              edge = this.m_resultPathTmp[index2 - 1].GetEdgeTo(this.m_resultPathTmp[index2 - 2]);
          }
        }
        else
        {
          if (index2 > 1)
          {
            option = this.m_connectingRoadsTmp[index2 - 2];
            if (!option.HasValue)
            {
              PfNode.Edge edgeTo = this.m_resultPathTmp[index2 - 1].GetEdgeTo(this.m_resultPathTmp[index2 - 2]);
              Vector2i lineSegment = edgeTo.ConnectionLine.ClosestPointToLineSegment(vector2i);
              Vector2i other = edge.ConnectionLine.P0;
              Fix32 fix32_1 = vector2i.DistanceTo(other) + other.DistanceTo(lineSegment);
              foreach (Vector2i iterateRasterizedPoint in edge.ConnectionLine.IterateRasterizedPoints(true))
              {
                Fix32 fix32_2 = vector2i.DistanceTo(iterateRasterizedPoint) + iterateRasterizedPoint.DistanceTo(lineSegment);
                if (fix32_2 < fix32_1)
                {
                  other = iterateRasterizedPoint;
                  fix32_1 = fix32_2;
                }
              }
              Vector2i coord = other;
              terrainPathSegment.PathRawReversed.Add(new Tile2i(coord));
              vector2i = coord + edge.NeighborDirection.DirectionVector;
              terrainPathSegment.PathRawReversed.Add(new Tile2i(vector2i));
              edge = edgeTo;
              continue;
            }
          }
          vector2i = edge.ConnectionLine.ClosestPointToLineSegment(vector2i);
          terrainPathSegment.PathRawReversed.Add(new Tile2i(vector2i));
        }
      }
      firstSegment = (IVehiclePathSegment) terrainPathSegment;
      return true;
    }

    public bool TryReconstructFoundPath(
      Lyst<PfNode> foundPath,
      Lyst<Option<RoadGraphPath>> connectingRoads)
    {
      foundPath.Clear();
      connectingRoads.Clear();
      if (this.m_foundPathNode.IsNone)
        return true;
      PfNode parentSafe = this.m_foundPathNode.Value;
      PfNode pfNode1 = this.m_foundPathNbr.Value;
      if (parentSafe == pfNode1)
      {
        Mafi.Assert.That<Option<RoadGraphPath>>(this.m_foundPathConnectingRoad).IsNone<RoadGraphPath>();
        Mafi.Assert.That<bool>(parentSafe.HasParent).IsEqualTo<bool>(parentSafe.PathLength > 0);
        Mafi.Assert.That<bool>(parentSafe.PathLength == 0 || parentSafe.IsVisitedFromStart).IsTrue();
        foundPath.Count = parentSafe.PathLength + 1;
        connectingRoads.Count = parentSafe.PathLength;
        for (int index = foundPath.Count - 1; index > 0; --index)
        {
          if (!parentSafe.HasParent)
          {
            Mafi.Log.Error("Failed to reconstruct path, node has no parent.");
            return false;
          }
          foundPath[index] = parentSafe;
          connectingRoads[index - 1] = parentSafe.RoadConnectionToParent;
          parentSafe = parentSafe.GetParentSafe();
        }
        foundPath[0] = parentSafe;
        return true;
      }
      Mafi.Assert.That<bool>(parentSafe.IsVisitedFromStart).IsNotEqualTo<bool>(pfNode1.IsVisitedFromStart);
      int num = parentSafe.PathLength + pfNode1.PathLength + 2;
      foundPath.Count = num;
      connectingRoads.Count = num - 1;
      PfNode pfNode2;
      PfNode pfNode3;
      if (parentSafe.IsVisitedFromStart)
      {
        pfNode2 = parentSafe;
        pfNode3 = pfNode1;
      }
      else
      {
        pfNode2 = pfNode1;
        pfNode3 = parentSafe;
      }
      int pathLength = pfNode2.PathLength;
      connectingRoads[pathLength] = this.m_foundPathConnectingRoad;
      for (int index = pathLength; index > 0; --index)
      {
        if (!pfNode2.HasParent)
        {
          Mafi.Log.Error("Failed to reconstruct path, node from start end has no parent.");
          return false;
        }
        foundPath[index] = pfNode2;
        connectingRoads[index - 1] = pfNode2.RoadConnectionToParent;
        pfNode2 = pfNode2.ParentNodeOnPath.Value;
      }
      foundPath[0] = pfNode2;
      for (int index = pathLength + 1; index < num - 1; ++index)
      {
        if (!pfNode3.HasParent)
        {
          Mafi.Log.Error("Failed to reconstruct path, node from goal end has no parent.");
          return false;
        }
        foundPath[index] = pfNode3;
        connectingRoads[index] = pfNode3.RoadConnectionToParent;
        pfNode3 = pfNode3.GetParentSafe();
      }
      foundPath[num - 1] = pfNode3;
      return true;
    }

    /// <summary>
    /// Fills given list with nodes that were explored during last or current path-finding run.
    /// </summary>
    public void GetExploredTiles(Lyst<ExploredPfNode> result)
    {
      Mafi.Assert.That<Lyst<ExploredPfNode>>(result).IsEmpty<ExploredPfNode>();
      foreach (PfNode processed in this.m_processedList)
        result.Add(new ExploredPfNode(processed.Area.CenterCoord, processed.GetParentSafe().Area.CenterCoord, processed.CurrentCost, processed.IsProcessed, processed.IsVisitedFromStart));
      foreach (KeyValuePair<Fix32, PfNode> toProcess in this.m_toProcessList)
      {
        PfNode pfNode = toProcess.Value;
        result.Add(new ExploredPfNode(pfNode.Area.CenterCoord, pfNode.GetParentSafe().Area.CenterCoord, pfNode.CurrentCost, pfNode.IsProcessed, pfNode.IsVisitedFromStart));
      }
    }

    public DebugGameMapDrawing DrawProcessedPfNodes(DebugGameMapDrawing drawing)
    {
      foreach (PfNode processed in this.m_processedList)
      {
        ColorRgba color = processed.IsVisitedFromStart ? ColorRgba.DarkGreen.SetA((byte) 128) : ColorRgba.Blue.SetA((byte) 128);
        drawing.DrawArea(processed.Area, color);
        if (processed.ParentNodeOnPath.HasValue)
          drawing.DrawArrow(processed.Area.CenterCoordF, processed.ParentNodeOnPath.Value.Area.CenterCoordF, color, RelTile1f.Half);
      }
      return drawing;
    }

    public DebugGameMapDrawing DrawPendingPfNodes(DebugGameMapDrawing drawing)
    {
      foreach (KeyValuePair<Fix32, PfNode> toProcess in this.m_toProcessList)
      {
        PfNode pfNode = toProcess.Value;
        ColorRgba color = pfNode.IsVisitedFromStart ? ColorRgba.DarkGreen.SetA((byte) 64) : ColorRgba.Blue.SetA((byte) 64);
        drawing.DrawArea(pfNode.Area, color);
        if (pfNode.ParentNodeOnPath.HasValue)
          drawing.DrawArrow(pfNode.Area.CenterCoordF, pfNode.ParentNodeOnPath.Value.Area.CenterCoordF, color, RelTile1f.Half);
        drawing.DrawString(pfNode.Area.CenterCoordF, toProcess.Key.ToString(), ColorRgba.Black.SetA((byte) 192), centered: true);
      }
      return drawing;
    }

    static VehiclePathFinder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehiclePathFinder.ROAD_DISTANCE_MULT = Fix32.Half;
    }

    private sealed class AStarHeap
    {
      private readonly Heap<PfNode> m_heap;

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

      public void Insert(PfNode node, Fix32 estimatedTotalCost)
      {
        Mafi.Assert.That<Option<PfNode>>(node.ParentNodeOnPath).HasValue<PfNode>("Adding non-initialized node to queue.");
        Mafi.Assert.That<bool>(node.IsProcessed).IsFalse("Adding already processed node to queue.");
        Mafi.Assert.That<bool>(node.IsVisited).IsFalse("Adding already visited node to queue.");
        this.m_heap.Push(estimatedTotalCost, node);
        node.SetIsVisited();
        if (node.IsVisitedFromStart)
          ++this.VisitedFromStartSetSize;
        else
          ++this.VisitedFromGoalSetSize;
      }

      public Fix32 PeekMinKey() => this.m_heap.PeekMinKey();

      public KeyValuePair<Fix32, PfNode> PopMin()
      {
        KeyValuePair<Fix32, PfNode> keyValuePair = this.m_heap.PopMin();
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
        this.m_heap.Clear();
      }

      [Pure]
      public Heap<PfNode>.Enumerator GetEnumerator() => this.m_heap.GetEnumerator();

      [Pure]
      public IEnumerable<KeyValuePair<Fix32, PfNode>> AsEnumerable()
      {
        return this.m_heap.AsEnumerable<KeyValuePair<Fix32, PfNode>>();
      }

      public AStarHeap()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_heap = new Heap<PfNode>(128);
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private readonly struct RoadShortcut
    {
      public readonly PfNode From;
      public readonly PfNode To;
      public readonly RoadGraphPath Path;
      public readonly Fix32 Cost;

      public RoadShortcut(PfNode from, PfNode to, RoadGraphPath path, Fix32 cost)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.From = from;
        this.To = to;
        this.Path = path;
        this.Cost = cost;
      }
    }
  }
}
