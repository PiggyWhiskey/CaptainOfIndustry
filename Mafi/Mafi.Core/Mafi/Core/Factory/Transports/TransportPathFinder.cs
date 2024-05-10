// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.TransportPathFinder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.PathFinding;
using Mafi.Utils;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>
  /// Transport path-finder that is highly optimized for its purpose. It implements a variant of A* that is able to
  /// handle "jumps" (transport ramps) but may not be able to find a valid path in some cases (see remarks).
  /// 
  /// Both start and goals are not checked for collisions. Pillars checking is "best-effort" and found paths may not
  /// have valid solution for supporting via pillars.
  /// 
  /// Path-finder works in limited space domain 64 x 64 x 8 tiles. This means that the requested goal may get clamped
  /// to this domain during initialization. See <see cref="P:Mafi.Core.Factory.Transports.TransportPathFinder.CurrentGoal" />.
  /// </summary>
  /// <remarks>
  /// Reference: https://www.redblobgames.com/pathfinding/a-star/implementation.html
  /// 
  /// The core is a standard A* graph search algorithm but there are following changes to make work for transports.
  /// 
  /// ===== RAMPS =====
  /// Unlike standard A*, we need to be able to perform "jumps" between non-neighboring nodes which is not
  /// normally allowed in A*. These jumps correspond to transport ramps. Without jumps, we'd be limited only to
  /// straight up/down in the grid.
  /// 
  /// The ramps present a new issue with self collisions: A ramp occupies a set of extra nodes that should not be
  /// used by the path. Every node remembers an extra set of ramp-occupied-tiles that needs to be check for
  /// collisions when expanding. We use pooled arrays to avoid large number of small allocations here.
  /// 
  /// These jumps also cause a systematic issue with the algorithm. We can no longer guarantee that every path will be
  /// found, even if it exists, precisely because of the ramps that may block them. For example, imagine a path from
  /// A to C that goes through B. Then, a first explored path from A to B blocks C due to ramp. Unfortunately,
  /// the optimal path A -&gt; X -&gt; B -&gt; C will be never explored because node B was already "expanded" and closed by the
  /// previous path. Concrete example: Path straight down from (0, 0, 1) to (0, 0, 0) where any ramp directly from
  /// start blocks the goal, but will be explored earlier than any other paths.
  /// 
  /// ===== GOAL CHANGING =====
  /// It is typical that for one start there may be many goals tested as the player moves their mouse while building
  /// a transport.
  /// 
  /// Goal-changing is implemented by re-computing priorities of all pending nodes. All already found nodes have
  /// optimal path lengths regardless of goal position (except when ramps screw us over).
  /// 
  /// It is important that goal position must not affect any logic during the path-finding. For example, since goal
  /// may be on a banned tile, we need to expand into banned tiles, bot not from them.
  /// 
  /// Support for goal changing also restricts us from only searching from the start. This means that we don't have
  /// an easy way of detecting blocked path before exploring ALL reachable nodes.
  /// 
  /// ===== PILLARS CHECKING =====
  /// Path-finder does its best effort to check pillars and discard paths that cannot be supported, but the checking
  /// is not perfect. Since we cannot predict when supports will be available, path-finder does search until
  /// `2 * pillar_radius` tiles are not supported. For example, it will fail in these cases:
  ///  1) Transport is shorter than `pillar_radius` and no pillars were found (a special case of #2).
  ///  2) Goal is reached but more than `pillar_radius` (but less than `2 * pillar_radius`) of the last tiles are
  ///     not supported.
  /// 
  /// Due to challenges with jumps and goal changing, it is not possible to discard paths to goal that do not
  /// satisfy pillars constraints since such paths might be explored further when goal is changed.
  /// 
  /// ===== WHY WE HAVE MULTIPLE NODES PER TILE =====
  /// It is tempting to disallow certain path shapes by not expanding to certain neighbors. For example, we wanted
  /// to ban turns at start/end of ramps. This was easy, when expanding node and making a ramp, just check that parent
  /// was in the same direction as the extension. Similarly, after a ramp, only straight segment can be allowed.
  /// 
  /// The big problem is that such rules break an important assumption that we are searching on a graph. Every node of
  /// a graph must have a list of neighbors regardless of its parent.
  /// 
  /// For example, imagine we search a path A -&gt; B -&gt; C and node C has no neighbors coming from B. But there might be
  /// a longer path A -&gt; X -&gt; C where C does have a neighbor D, but this path will be never explored since A -&gt; X -&gt; C
  /// is longer than A -&gt; B -&gt; C and when expanding X to C, this expansion will be discarded since it is not the
  /// shortest path.
  /// 
  /// The takeaway is: Since we don't explicitly construct a graph, neighbor expansion have rules that does make it
  /// behave like it was a graph. For example, neighbors must not depend on current node's parent.
  /// 
  /// To fix this we added "overlapped" nodes, where a single point in space contains more than one node.
  /// Depending on where the parent node is we can move to a different node, and these nodes can then have unique
  /// connectivity. While this increases our memory consumption by a reasonable amount, performance was measured
  /// to be only 80% of the implementation without these overlapped nodes
  /// 
  /// ===== PERF =====
  /// Many optimizations were done to make the path-finding as fast as possible and allocation free.
  /// 
  /// 1) All base node's data are pre-allocated structs around start node. Size of this pre-allocated area is
  ///    64 x 64 x 8 * 3 = 96k. This restricts the path-finder to search only in +-32 in X-Y, and +-4 in Z around
  ///    start, and includes two extra arrays for nodes used by ramps (one for X, one for Y). Node's index can be
  ///    easily translated to a coordinate using a few bit operations, but this also restricts domain dimensions
  ///    to powers of two.
  /// 2) Node data itself is tightly-packed 8-byte struct. This makes path-finder's pre-allocated data only 774 kB.
  ///    The smaller the data the less cache misses during path-finding will occur.
  /// 3) Collision checks for nodes are postponed. Instead of checking collisions during expansion and adding
  ///    only valid nodes to the priority queue (as traditional A* does), we postpone expensive checks so that only
  ///    nodes that are being taken out of the priority queue are being checked. This saves large amount of
  ///    checks (proportional to queue size). Nodes that are never expanded are not even checked for collisions.
  ///    The downsides are a slightly larger queue (can contain "blocked" nodes) and extra code complexity of
  ///    collision checking.
  /// 4) Terrain height is cached so that we query terrain only once per unique 2D tile. Additionally, 3D tiles that
  ///    are in collision are marked as banned for faster rejection next time.
  /// 5) Max pillars height is cached so that we pillars only once per unique 2D tile.
  /// 6) Occupancy collisions are also cached in nodes so that once a node has a collision, is marked as banned
  ///    and collision on such node is never checked again.
  /// 7) Instead of tracking of used nodes so that they can be reset, clearing is done using `Array.Clear` that
  ///    effectively clears ALL nodes at once. This turned out to be faster. The only downside is that we cannot
  ///    store any persistent data in nodes such as node ID.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class TransportPathFinder : ITransportPathFinder
  {
    /// <summary>
    /// Size of pre-allocated node array in both X and Y directions. Must be power of two.
    /// </summary>
    public const int XY_SIZE = 64;
    /// <summary>
    /// Size of pre-allocated node array in both Z direction. Must be power of two.
    /// </summary>
    public const int Z_SIZE = 8;
    private static readonly RelTile3i MAX_REL_COORD;
    private static readonly RelTile3i START_REL_COORD;
    private static readonly int START_NODE_INDEX;
    /// <summary>
    /// Current absolute position of node at origin (corner) coord [0, 0, 0].
    /// </summary>
    private Tile3i m_originCoord;
    /// <summary>Pre-allocated array of PF nodes.</summary>
    private readonly TransportPathFinder.NodeMutable[] m_nodes;
    private readonly Lyst<ushort[]> m_extraOccupiedNodesData;
    private readonly ArrayPoolCustom<ushort> m_ushortArrayPool;
    /// <summary>
    /// Provides fast lookup and cache for terrain data. Stored value is 1 + min allowed relative z-coord.
    /// Use via <see cref="M:Mafi.Core.Factory.Transports.TransportPathFinder.isCollidingWithTerrain(System.Int32)" />.
    /// </summary>
    private readonly byte[] m_terrainHeightLookup;
    private readonly byte[] m_pillarHeightLookup;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly TerrainManager m_terrainManager;
    private readonly IoPortsManager m_portsManager;
    private readonly ITransportsPredicates m_transportsPredicates;
    private readonly IPillarsChecker m_pillarsChecker;
    private bool m_undirected;
    private TransportProto m_proto;
    private bool m_startMustBeFlat;
    private bool m_goalMustBeFlat;
    private bool m_invertTieBreaking;
    private bool m_allowOnlyStraight;
    private bool m_checkPillars;
    private bool m_goalMustNotHavePerpendicularRamp;
    private Direction90 m_goalAllowedRampDirection;
    private bool m_startMustNotHavePerpendicularRamp;
    private Direction90 m_startAllowedRampDirection;
    private readonly Lyst<Tile3i> m_tmpTiles;
    private readonly Lyst<OccupiedTileRange> m_occupiedTilesTmp;
    private readonly Lyst<ushort> m_occupiedTilesIndicesTmp;
    private readonly Heap<int> m_toProcessHeap;
    private bool m_pathFound;
    private RelTile3i m_goalRelCoord;
    private RelTile3i m_toStartToGoal;
    private int m_goalNodeIndex;
    private int m_goalNodeIndexFound;
    private int m_optimalRelZ;
    private int m_maxDistanceWithoutPillars;

    /// <summary>
    /// Converts node index to relative coordinate. Does not check input.
    /// </summary>
    private static RelTile3i coordFromIndex(int i)
    {
      return new RelTile3i(i & 63, (i & 4032) >> 6, (i & 28672) >> 12);
    }

    private static RelTile2i coord2FromIndex(int i) => new RelTile2i(i & 63, (i & 4032) >> 6);

    /// <summary>
    /// Converts relative coordinate to node index. Does not check input.
    /// </summary>
    private static ushort indexFromCoord(RelTile3i coord)
    {
      return (ushort) (coord.X | coord.Y << 6 | coord.Z << 12);
    }

    private static bool isValidRelCoord(RelTile2i coord)
    {
      return (uint) coord.X < 64U && (uint) coord.Y < 64U;
    }

    private static bool isValidRelCoord(RelTile3i coord)
    {
      return (uint) coord.X < 64U && (uint) coord.Y < 64U && (uint) coord.Z < 8U;
    }

    public Tile3i CurrentStart => this.m_originCoord + TransportPathFinder.START_REL_COORD;

    /// <summary>
    /// Note that the current goal might be different from the requested goal due to clamping.
    /// </summary>
    public Tile3i CurrentGoal => this.m_originCoord + this.m_goalRelCoord;

    /// <summary>Goal as requested.</summary>
    public Tile3i OriginalGoal { get; private set; }

    public Option<TransportProto> CurrentTransportProto => (Option<TransportProto>) this.m_proto;

    public TransportPathFinderOptions Options { get; private set; }

    public int CurrentPfId { get; private set; }

    /// <summary>Number of performed steps of the current task.</summary>
    public int TotalStepsCount { get; private set; }

    public int QueueSize => this.m_toProcessHeap.Count;

    public TransportPathFinder(
      TerrainOccupancyManager occupancyManager,
      TerrainManager terrainManager,
      IoPortsManager portsManager,
      ITransportsPredicates transportsPredicates,
      IPillarsChecker pillarsChecker)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_originCoord = Tile3i.MinValue;
      this.m_nodes = new TransportPathFinder.NodeMutable[131072];
      this.m_extraOccupiedNodesData = new Lyst<ushort[]>(256, true);
      this.m_ushortArrayPool = new ArrayPoolCustom<ushort>(50);
      this.m_terrainHeightLookup = new byte[4096];
      this.m_pillarHeightLookup = new byte[4096];
      this.m_tmpTiles = new Lyst<Tile3i>(true);
      this.m_occupiedTilesTmp = new Lyst<OccupiedTileRange>(16, true);
      this.m_occupiedTilesIndicesTmp = new Lyst<ushort>(true);
      this.m_toProcessHeap = new Heap<int>(256, true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_occupancyManager = occupancyManager;
      this.m_terrainManager = terrainManager;
      this.m_portsManager = portsManager;
      this.m_transportsPredicates = transportsPredicates;
      this.m_pillarsChecker = pillarsChecker;
      Assert.That<RelTile3i>(TransportPathFinder.coordFromIndex(0)).IsEqualTo<RelTile3i>(RelTile3i.Zero);
      Assert.That<ushort>(TransportPathFinder.indexFromCoord(RelTile3i.Zero)).IsEqualTo<ushort>((ushort) 0);
      Assert.That<RelTile3i>(TransportPathFinder.coordFromIndex(TransportPathFinder.START_NODE_INDEX)).IsEqualTo<RelTile3i>(TransportPathFinder.START_REL_COORD);
      Assert.That<ushort>(TransportPathFinder.indexFromCoord(TransportPathFinder.START_REL_COORD)).IsEqualTo<ushort>((ushort) TransportPathFinder.START_NODE_INDEX);
      Assert.That<RelTile3i>(TransportPathFinder.coordFromIndex((int) short.MaxValue)).IsEqualTo<RelTile3i>(TransportPathFinder.MAX_REL_COORD);
      Assert.That<ushort>(TransportPathFinder.indexFromCoord(TransportPathFinder.MAX_REL_COORD)).IsEqualTo<ushort>((ushort) short.MaxValue);
    }

    public void ResetState()
    {
      Array.Clear((Array) this.m_nodes, 0, this.m_nodes.Length);
      Array.Clear((Array) this.m_terrainHeightLookup, 0, this.m_terrainHeightLookup.Length);
      Array.Clear((Array) this.m_pillarHeightLookup, 0, this.m_pillarHeightLookup.Length);
      this.m_toProcessHeap.Clear();
      foreach (ushort[] arr in this.m_extraOccupiedNodesData)
        this.m_ushortArrayPool.ReturnToPool(arr);
      this.m_extraOccupiedNodesData.Clear();
      this.TotalStepsCount = 0;
      this.m_pathFound = false;
      this.m_undirected = false;
    }

    public void InitPathFinding(
      TransportProto proto,
      Tile3i start,
      Tile3i goal,
      TransportPathFinderOptions options,
      IEnumerable<Tile3i> bannedTiles = null)
    {
      this.ResetState();
      ++this.CurrentPfId;
      this.m_proto = proto;
      this.Options = options;
      this.m_startMustBeFlat = proto.ZStepLength.IsNotZero && options.HasFlags(TransportPathFinderFlags.StartMustBeFlat);
      this.m_goalMustBeFlat = proto.ZStepLength.IsNotZero && options.HasFlags(TransportPathFinderFlags.GoalMustBeFlat);
      this.m_invertTieBreaking = options.HasFlags(TransportPathFinderFlags.InvertTieBreaking);
      this.m_allowOnlyStraight = options.HasFlags(TransportPathFinderFlags.AllowOnlyStraight);
      this.m_checkPillars = proto.NeedsPillars;
      this.m_goalMustNotHavePerpendicularRamp = false;
      this.m_goalAllowedRampDirection = new Direction90();
      this.m_startMustNotHavePerpendicularRamp = false;
      this.m_startAllowedRampDirection = new Direction90();
      this.m_maxDistanceWithoutPillars = proto.NeedsPillars ? proto.MaxPillarSupportRadius.Value * 2 : 0;
      start = this.m_terrainManager.ClampToTerrainBounds(start);
      goal = this.m_terrainManager.ClampToTerrainBounds(goal);
      this.m_originCoord = start - TransportPathFinder.START_REL_COORD;
      if (this.m_proto.ZStepLength == RelTile1i.MaxValue)
        goal = new Tile3i(goal.X, goal.Y, start.Z);
      this.initGoalData(goal);
      ref TransportPathFinder.NodeMutable local = ref this.m_nodes[TransportPathFinder.START_NODE_INDEX];
      int unsupportedLength = !this.m_checkPillars || this.canBeSupportedByPillar(TransportPathFinder.START_NODE_INDEX) ? 0 : proto.MaxPillarSupportRadius.Value;
      local.InitializeAsStart((byte) unsupportedLength);
      this.insertInHeapToBeProcessed(TransportPathFinder.START_NODE_INDEX, ref local);
      ushort[] numArray = this.m_ushortArrayPool.Get(1);
      numArray[0] = (ushort) TransportPathFinder.START_NODE_INDEX;
      this.m_extraOccupiedNodesData.Add(numArray);
      local.ExtraOccupiedTilesIndex = 0;
      for (int index = 0; index < (this.m_goalMustBeFlat ? 2 : 4); ++index)
        this.m_nodes[this.m_goalNodeIndex + index * 32768].InitializeAsGoal(this.m_goalNodeIndex + index * 32768);
      if (options.HasFlags(TransportPathFinderFlags.BanTilesInFrontOfPorts))
      {
        foreach (IoPort port in this.m_portsManager.Ports)
        {
          RelTile3i coord = port.ExpectedConnectedPortCoord - this.m_originCoord;
          if (TransportPathFinder.isValidRelCoord(coord))
          {
            bool flag;
            switch (port.OwnerEntity)
            {
              case Transport _:
              case Zipper _:
              case MiniZipper _:
                flag = true;
                break;
              default:
                flag = false;
                break;
            }
            if (!flag)
            {
              int index = (int) TransportPathFinder.indexFromCoord(coord);
              Direction903d direction;
              if (index == this.m_goalNodeIndex && proto.ZStepLength.IsNotZero)
              {
                this.m_goalMustNotHavePerpendicularRamp = true;
                direction = port.Direction;
                this.m_goalAllowedRampDirection = direction.ToHorizontalOrError();
              }
              if (index == TransportPathFinder.START_NODE_INDEX && proto.ZStepLength.IsNotZero)
              {
                this.m_startMustNotHavePerpendicularRamp = true;
                direction = port.Direction;
                this.m_startAllowedRampDirection = direction.ToHorizontalOrError();
              }
              this.m_nodes[index].SetFlag(TransportPathFinder.NodeFlag.IsBanned);
            }
          }
        }
      }
      if (bannedTiles != null)
      {
        foreach (Tile3i bannedTile in bannedTiles)
        {
          RelTile3i coord = bannedTile - this.m_originCoord;
          if (TransportPathFinder.isValidRelCoord(coord))
            this.m_nodes[(int) TransportPathFinder.indexFromCoord(coord)].SetFlag(TransportPathFinder.NodeFlag.IsBanned);
        }
      }
      if (!(start == goal))
        return;
      this.m_pathFound = true;
      this.m_goalNodeIndex = TransportPathFinder.START_NODE_INDEX;
      this.m_goalNodeIndexFound = TransportPathFinder.START_NODE_INDEX;
    }

    /// <summary>Changes goal of the path-finding.</summary>
    public void ChangeGoal(Tile3i goal)
    {
      this.m_undirected = false;
      ++this.CurrentPfId;
      if (this.m_proto.ZStepLength == RelTile1i.MaxValue)
        goal = new Tile3i(goal.X, goal.Y, this.OriginalGoal.Z);
      this.initGoalData(goal);
      uint num = uint.MaxValue;
      for (int index = 0; index < (this.m_goalMustBeFlat ? 2 : 4); ++index)
      {
        ref TransportPathFinder.NodeMutable local = ref this.m_nodes[this.m_goalNodeIndex + index * 32768];
        if (local.IsProcessed && local.CurrentCost < num)
        {
          num = local.CurrentCost;
          this.m_pathFound = true;
          this.m_goalNodeIndexFound = this.m_goalNodeIndex + index * 32768;
        }
      }
      if (num != uint.MaxValue)
        return;
      this.m_goalMustNotHavePerpendicularRamp = false;
      this.m_goalAllowedRampDirection = new Direction90();
      if (this.Options.HasFlags(TransportPathFinderFlags.BanTilesInFrontOfPorts) && this.m_proto.ZStepLength.IsNotZero)
      {
        foreach (IoPort port in this.m_portsManager.Ports)
        {
          RelTile3i coord = port.ExpectedConnectedPortCoord - this.m_originCoord;
          if (TransportPathFinder.isValidRelCoord(coord))
          {
            bool flag;
            switch (port.OwnerEntity)
            {
              case Transport _:
              case Zipper _:
                flag = true;
                break;
              default:
                flag = false;
                break;
            }
            if (!flag && (int) TransportPathFinder.indexFromCoord(coord) == this.m_goalNodeIndex)
            {
              this.m_goalMustNotHavePerpendicularRamp = true;
              this.m_goalAllowedRampDirection = port.Direction.ToHorizontalOrError();
              break;
            }
          }
        }
      }
      for (int index = 0; index < (this.m_goalMustBeFlat ? 2 : 4); ++index)
      {
        ref TransportPathFinder.NodeMutable local = ref this.m_nodes[this.m_goalNodeIndex + index * 32768];
        if (!local.IsVisited)
          local.InitializeAsGoal(this.m_goalNodeIndex + index * 32768);
      }
      this.m_pathFound = false;
      this.m_toProcessHeap.UpdateAllPriorities((Func<int, Fix32>) (nodeIndex => (Fix32) (int) this.m_nodes[nodeIndex].CurrentCost + this.estPathCostToGoal(nodeIndex)));
    }

    private void initGoalData(Tile3i goal)
    {
      this.OriginalGoal = goal;
      this.m_goalRelCoord = goal - this.m_originCoord;
      this.m_goalRelCoord = new RelTile3i(this.m_goalRelCoord.X.Clamp(0, 63), this.m_goalRelCoord.Y.Clamp(0, 63), this.m_goalRelCoord.Z.Clamp(0, 7));
      if (this.m_allowOnlyStraight)
      {
        RelTile2i absValue = (this.m_goalRelCoord.Xy - TransportPathFinder.START_REL_COORD.Xy).AbsValue;
        this.m_goalRelCoord = absValue.X <= absValue.Y ? this.m_goalRelCoord.SetX(TransportPathFinder.START_REL_COORD.X) : this.m_goalRelCoord.SetY(TransportPathFinder.START_REL_COORD.Y);
      }
      this.m_goalNodeIndex = (int) TransportPathFinder.indexFromCoord(this.m_goalRelCoord);
      this.m_toStartToGoal = this.m_goalRelCoord - TransportPathFinder.START_REL_COORD;
      TransportPathFinderOptions options = this.Options;
      int num;
      if (!options.PreferredHeight.HasValue)
      {
        num = this.m_goalRelCoord.Z;
      }
      else
      {
        options = this.Options;
        num = (options.PreferredHeight.Value.Value - this.m_originCoord.Height.Value).Clamp(0, 7);
      }
      this.m_optimalRelZ = num;
    }

    public void SetUndirected()
    {
      if (this.m_undirected)
        return;
      this.m_toProcessHeap.UpdateAllPriorities((Func<int, Fix32>) (nodeIndex => (Fix32) (int) this.m_nodes[nodeIndex].CurrentCost));
      this.m_undirected = true;
    }

    /// <summary>
    /// Runs the path-finding for given number of iterations. Returns status of the operation.
    /// </summary>
    public PathFinderResult ContinuePathFinding(
      ref int iterations,
      out ImmutableArray<Tile3i> outPivots)
    {
      PathFinderResult pathFinderResult = this.continuePathFinding(ref iterations);
      if (pathFinderResult == PathFinderResult.PathFound)
      {
        this.m_tmpTiles.Clear();
        this.ReconstructPath(this.m_tmpTiles);
        outPivots = this.m_tmpTiles.ToImmutableArrayAndClear();
      }
      return pathFinderResult;
    }

    private PathFinderResult continuePathFinding(ref int iterations)
    {
      Assert.That<int>(iterations).IsPositive();
      if (this.m_pathFound && !this.m_undirected)
        return PathFinderResult.PathFound;
      while (this.m_toProcessHeap.IsNotEmpty)
      {
        if (iterations <= 0)
          return PathFinderResult.StillSearching;
        --iterations;
        ++this.TotalStepsCount;
        int index = this.m_toProcessHeap.PopMin().Value;
        ref TransportPathFinder.NodeMutable local1 = ref this.m_nodes[index];
        ref TransportPathFinder.NodeMutable local2 = ref this.m_nodes[index & (int) short.MaxValue];
        if (!local1.HasAnyFlags(TransportPathFinder.NodeFlag.IsProcessed))
        {
          if (!local1.AreCollisionComputed)
          {
            int occupiedArrIndex;
            if (!this.computeCollisionsReturnConnCollided(index, local1.ParentOnPathIndex, out occupiedArrIndex))
              local1.ExtraOccupiedTilesIndex = occupiedArrIndex;
            else
              continue;
          }
          local1.SetFlag(TransportPathFinder.NodeFlag.IsProcessed);
          if (!local2.IsBanned || index == TransportPathFinder.START_NODE_INDEX)
          {
            this.expandNeighborsTowards(index, local1, Direction903d.PlusX);
            this.expandNeighborsTowards(index, local1, Direction903d.PlusY);
            this.expandNeighborsTowards(index, local1, Direction903d.MinusX);
            this.expandNeighborsTowards(index, local1, Direction903d.MinusY);
            if (this.m_proto.ZStepLength.IsZero)
            {
              bool flag = false;
              if (index == TransportPathFinder.START_NODE_INDEX)
              {
                Tile3i position = this.m_originCoord + TransportPathFinder.coordFromIndex(index);
                Transport entity;
                if (this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(position, out entity, this.m_transportsPredicates.IgnorePillarsPredicate) && (Proto) this.m_proto.PortsShape == (Proto) entity.Prototype.PortsShape)
                  flag = !(position == entity.StartPosition) ? !(position == entity.EndPosition) || entity.EndOutputPort.IsConnected : entity.StartInputPort.IsConnected;
              }
              if (!flag)
              {
                this.expandNeighborsTowards(index, local1, Direction903d.PlusZ);
                this.expandNeighborsTowards(index, local1, Direction903d.MinusZ);
              }
            }
          }
          if (!this.m_undirected && this.isGoalIndex(index) && this.isIndexAllowedAsGoal(index))
          {
            this.m_goalNodeIndexFound = index;
            this.m_pathFound = true;
            return PathFinderResult.PathFound;
          }
        }
      }
      return PathFinderResult.PathDoesNotExist;
    }

    /// <summary>
    /// Check whether a transport from <paramref name="parentIndex" /> to <paramref name="nodeIndex" /> has
    /// collisions ON THE WAY to the given node, ignoring collisions of the node itself. However, if the node has
    /// any collisions, it is marked as banned.
    /// </summary>
    /// <remarks>
    /// Node collisions are ignored to enable goals on blocked tiles and to have shortest paths computed to them.
    /// This is necessary to have goal changing working properly. However, any connections with segments in between
    /// them are discarded immediately.
    /// </remarks>
    private bool computeCollisionsReturnConnCollided(
      int nodeIndex,
      int parentIndex,
      out int occupiedArrIndex)
    {
      Tile3i tile3i = this.m_originCoord + TransportPathFinder.coordFromIndex(nodeIndex);
      occupiedArrIndex = this.m_nodes[parentIndex].ExtraOccupiedTilesIndex;
      if (this.m_terrainManager.IsBlockingBuildings(this.m_terrainManager.GetTileIndex(tile3i.Xy)))
      {
        this.m_nodes[nodeIndex & (int) short.MaxValue].SetFlag(TransportPathFinder.NodeFlag.IsBanned);
        return true;
      }
      if (!this.m_nodes[nodeIndex & (int) short.MaxValue].IsBanned && (this.m_occupancyManager.IsOccupiedAt(tile3i, this.m_transportsPredicates.IgnorePillarsPredicate) || this.isCollidingWithTerrain(nodeIndex)))
        this.m_nodes[nodeIndex & (int) short.MaxValue].SetFlag(TransportPathFinder.NodeFlag.IsBanned);
      ushort[] longArray = this.m_extraOccupiedNodesData[occupiedArrIndex];
      foreach (int num in longArray)
      {
        if ((num & (int) short.MaxValue) == (nodeIndex & (int) short.MaxValue))
          return true;
      }
      Tile3i start = this.m_originCoord + TransportPathFinder.coordFromIndex(parentIndex);
      if (tile3i.Z != start.Z && tile3i.Xy != start.Xy)
      {
        this.m_occupiedTilesTmp.Clear();
        TransportHelper.ComputeOccupiedTilesForSegment(start, tile3i, this.m_occupiedTilesTmp);
        if (!this.m_occupancyManager.CanAdd(this.m_occupiedTilesTmp.BackingArrayAsSlice, out EntityId _, this.m_transportsPredicates.IgnorePillarsPredicate))
          return true;
        this.m_occupiedTilesIndicesTmp.Clear();
        foreach (OccupiedTileRange occupiedTileRange in this.m_occupiedTilesTmp)
        {
          RelTile3i coord = occupiedTileRange.Position.ExtendHeight(occupiedTileRange.From) - this.m_originCoord;
          if (this.m_terrainManager.IsBlockingBuildings(this.m_terrainManager.GetTileIndex(occupiedTileRange.Position)))
          {
            this.m_nodes[(int) TransportPathFinder.indexFromCoord(coord) & (int) short.MaxValue].SetFlag(TransportPathFinder.NodeFlag.IsBanned);
            return true;
          }
          int nodeIndex1 = (int) TransportPathFinder.indexFromCoord(coord);
          int num = 0;
          while (num < occupiedTileRange.VerticalSize.Value)
          {
            ref TransportPathFinder.NodeMutable local = ref this.m_nodes[nodeIndex1 & (int) short.MaxValue];
            if (local.IsBanned)
              return true;
            if (this.isCollidingWithTerrain(nodeIndex1))
            {
              local.SetFlag(TransportPathFinder.NodeFlag.IsBanned);
              return true;
            }
            this.m_occupiedTilesIndicesTmp.Add((ushort) (nodeIndex1 & (int) short.MaxValue));
            ++num;
            nodeIndex1 += 4096;
          }
        }
        if (TransportPathFinder.areOccupiedIndicesColliding(this.m_occupiedTilesIndicesTmp.BackingArrayAsSlice, longArray))
          return true;
        occupiedArrIndex = this.extendExtraOccupiedTiles(this.m_occupiedTilesIndicesTmp.BackingArrayAsSlice, longArray);
      }
      else
        Assert.That<int>((tile3i - start).LengthSqrInt).IsEqualTo(1);
      return false;
    }

    /// <summary>
    /// Checks whether there are any identical values in the two given arrays.
    /// </summary>
    private static bool areOccupiedIndicesColliding(
      ReadOnlyArraySlice<ushort> shortArray,
      ushort[] longArray)
    {
      int index1 = 0;
      for (int length = longArray.Length; index1 < length; ++index1)
      {
        int num1 = (int) longArray[index1];
        for (int index2 = 0; index2 < shortArray.Length; ++index2)
        {
          int num2 = (int) shortArray[index2];
          if (num1 == num2)
            return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Adds new entry to <see cref="F:Mafi.Core.Factory.Transports.TransportPathFinder.m_extraOccupiedNodesData" /> by combining the two given arrays and returns
    /// index of the newly added record.
    /// </summary>
    private int extendExtraOccupiedTiles(ReadOnlyArraySlice<ushort> shortArray, ushort[] longArray)
    {
      ushort[] numArray = this.m_ushortArrayPool.Get(shortArray.Length + longArray.Length);
      Array.Copy((Array) longArray, 0, (Array) numArray, 0, longArray.Length);
      shortArray.CopyTo(numArray, longArray.Length);
      this.m_extraOccupiedNodesData.Add(numArray);
      return this.m_extraOccupiedNodesData.Count - 1;
    }

    /// <summary>
    /// Computes cost for a single step.
    /// Note that cost per step should not be larger than 255 on average (spikes are acceptable) since entire path
    /// cost must fit into <c>int</c>.
    /// </summary>
    private bool tryGetStepCost(int nodeIndex, int parentIndex, out int outCost)
    {
      Assert.That<int>(nodeIndex).IsNotEqualTo(parentIndex);
      int num1 = 0;
      RelTile3i relTile3i1 = TransportPathFinder.coordFromIndex(nodeIndex);
      Tile2i tile2i = relTile3i1.Xy + this.m_originCoord.Xy;
      if (!this.m_terrainManager.IsValidCoord(tile2i))
      {
        outCost = 0;
        return false;
      }
      if (this.m_terrainManager.IsBlockingBuildings(this.m_terrainManager.GetTileIndex(tile2i)))
      {
        outCost = 0;
        return false;
      }
      if (this.m_allowOnlyStraight)
      {
        RelTile2i relTile2i = (relTile3i1 - TransportPathFinder.START_REL_COORD).Xy;
        relTile2i = relTile2i.AbsValue;
        relTile2i = relTile2i.Signs;
        if (relTile2i.Sum >= 2)
        {
          outCost = 0;
          return false;
        }
      }
      RelTile3i relTile3i2 = TransportPathFinder.coordFromIndex(parentIndex);
      RelTile3i rhs = relTile3i1 - relTile3i2;
      TransportPathFinder.NodeMutable node = this.m_nodes[parentIndex];
      RelTile2i xy;
      if (node.ParentOnPathIndex != parentIndex)
      {
        RelTile3i relTile3i3 = relTile3i2 - TransportPathFinder.coordFromIndex(node.ParentOnPathIndex);
        xy = relTile3i3.Xy;
        if (xy.IsNotZero && relTile3i3.Z != 0)
        {
          xy = relTile3i3.Xy;
          if (!xy.IsParallelTo(rhs.Xy))
          {
            outCost = 0;
            return false;
          }
        }
        xy = rhs.Xy;
        if (xy.IsNotZero && rhs.Z != 0)
        {
          xy = rhs.Xy;
          if (!xy.IsParallelTo(relTile3i3.Xy))
          {
            outCost = 0;
            return false;
          }
        }
        xy = relTile3i3.Xy;
        if (!xy.IsZero)
        {
          xy = rhs.Xy;
          if (!xy.IsZero)
          {
            xy = relTile3i3.Xy;
            if (xy.IsParallelOrAntiParallelTo(rhs.Xy))
            {
              xy = relTile3i3.Xy;
              if (xy.Dot(rhs.Xy) <= 0L)
              {
                outCost = 0;
                return false;
              }
              goto label_26;
            }
            else
            {
              if (this.m_allowOnlyStraight)
              {
                outCost = 0;
                return false;
              }
              num1 += 50;
              goto label_26;
            }
          }
        }
        int num2 = relTile3i3.Z * rhs.Z;
        if (num2 <= 0)
        {
          if (num2 < 0)
          {
            outCost = 0;
            return false;
          }
          num1 += 50;
        }
label_26:
        long z = (long) relTile3i3.Cross(rhs).Z;
        if (this.m_invertTieBreaking)
        {
          if (z > 0L)
            num1 += 5;
        }
        else if (z < 0L)
          num1 += 5;
      }
      RelTile3i absValue = rhs.AbsValue;
      int num3 = num1;
      xy = absValue.Xy;
      int num4 = xy.Sum * 30;
      int num5 = num3 + num4;
      int num6 = this.m_proto.ZStepLength.Value != 0 ? num5 + absValue.Z * 100 : num5 + absValue.Z * 30;
      int num7 = (relTile3i1.Z - this.m_optimalRelZ).Abs();
      int num8 = this.m_proto.ZStepLength.Value != 0 ? num6 + num7 * 10 : num6 + num7 * 10;
      outCost = num8;
      return true;
    }

    private bool isGoalIndex(int index)
    {
      return !this.m_undirected && (index & (int) short.MaxValue) == this.m_goalNodeIndex;
    }

    private bool isIndexAllowedAsGoal(int nodeIndex)
    {
      return (nodeIndex == this.m_goalNodeIndex || !this.m_goalMustBeFlat || nodeIndex < 65536) && (!this.m_goalMustNotHavePerpendicularRamp || (this.m_goalAllowedRampDirection.DirectionVector.X == 0 || nodeIndex < 98304) && (this.m_goalAllowedRampDirection.DirectionVector.Y == 0 || nodeIndex < 65536 || nodeIndex >= 98304));
    }

    /// <summary>
    /// Returns estimated cost from given node to the goal node. This MUST NOT overestimate.
    /// </summary>
    private Fix32 estPathCostToGoal(int nodeIndex)
    {
      if (this.m_undirected)
        return (Fix32) 0;
      RelTile3i relTile3i1 = TransportPathFinder.coordFromIndex(nodeIndex);
      RelTile3i relTile3i2 = this.m_goalRelCoord - relTile3i1;
      RelTile3i absValue = relTile3i2.AbsValue;
      if (this.m_proto.ZStepLength == RelTile1i.MaxValue && absValue.Z != 0)
      {
        Log.Warning("Trying to estimate path cost to different Z when proto won't even allow this.");
        return (Fix32) 1000;
      }
      int num1 = absValue.Xy.Sum.Max(this.m_proto.ZStepLength.Value * absValue.Z) * 30;
      int num2 = (this.m_proto.ZStepLength.Value != 0 ? num1 + absValue.Z * 100 : num1 + absValue.Z * 30) + (relTile3i2.X == 0 | relTile3i2.Y == 0 ? 0 : 50);
      int num3 = (relTile3i1.Z - this.m_optimalRelZ).Abs();
      int goal = this.m_proto.ZStepLength.Value != 0 ? num2 + num3 * 10 : num2 + num3 * 10;
      long z = (long) (relTile3i1 - TransportPathFinder.START_REL_COORD).Cross(this.m_toStartToGoal).Z;
      if (this.m_invertTieBreaking)
      {
        if (z < 0L)
          --goal;
      }
      else if (z > 0L)
        --goal;
      return (Fix32) goal;
    }

    /// <summary>
    /// Efficient way to check whether a node is colliding with terrain.
    /// </summary>
    private bool isCollidingWithTerrain(int nodeIndex)
    {
      nodeIndex &= (int) short.MaxValue;
      byte heightData = this.m_terrainHeightLookup[nodeIndex & 4095];
      if (heightData == (byte) 0)
        heightData = this.computeHeightData(nodeIndex);
      return nodeIndex >> 12 < (int) heightData - 1;
    }

    /// <summary>Computes and encodes min height</summary>
    private byte computeHeightData(int nodeIndex)
    {
      nodeIndex &= (int) short.MaxValue;
      ThicknessTilesI thicknessTilesI = TransportHelper.GetLowestNonCollidingHeight(this.m_terrainManager[TransportPathFinder.coord2FromIndex(nodeIndex) + this.m_originCoord.Xy]) - this.m_originCoord.Height;
      if (thicknessTilesI.IsNegative)
        thicknessTilesI = ThicknessTilesI.Zero;
      byte heightData = (byte) (thicknessTilesI.Value + 1);
      this.m_terrainHeightLookup[nodeIndex & 4095] = heightData;
      return heightData;
    }

    /// <summary>
    /// Tests whether given position is in collision with terrain (using
    /// <see cref="M:Mafi.Core.Factory.Transports.TransportHelper.GetLowestNonCollidingHeight(Mafi.Core.Terrain.TerrainTile)" />). This is exposed only for tests and works only
    /// in the PF domain.
    /// </summary>
    internal bool TestOnly_IsCollidingWithTerrain(Tile3i position)
    {
      return this.isCollidingWithTerrain((int) TransportPathFinder.indexFromCoord(position - this.m_originCoord));
    }

    private bool canBeSupportedByPillar(int nodeIndex)
    {
      nodeIndex &= (int) short.MaxValue;
      byte pillarData = this.m_pillarHeightLookup[nodeIndex & 4095];
      if (pillarData == (byte) 0)
        pillarData = this.computePillarData(nodeIndex);
      return nodeIndex >> 12 < (int) pillarData - 2;
    }

    private byte computePillarData(int nodeIndex)
    {
      nodeIndex &= (int) short.MaxValue;
      HeightTilesI? maxPillarHeightAt = this.m_pillarsChecker.GetMaxPillarHeightAt(TransportPathFinder.coord2FromIndex(nodeIndex) + this.m_originCoord.Xy);
      if (!maxPillarHeightAt.HasValue)
        return 1;
      ThicknessTilesI thicknessTilesI = maxPillarHeightAt.Value - this.m_originCoord.Height;
      if (thicknessTilesI.IsNegative)
        return 1;
      byte pillarData = (byte) (thicknessTilesI.Value + 2);
      this.m_pillarHeightLookup[nodeIndex & 4095] = pillarData;
      return pillarData;
    }

    /// <summary>
    /// Tests whether given position is in the domain understood by the pathfinder.
    /// This is exposed only for tests.
    /// </summary>
    public bool TestOnly_IsValidRelCoord(Tile3i position)
    {
      return TransportPathFinder.isValidRelCoord(position - this.m_originCoord);
    }

    /// <summary>
    /// Tests whether given position can be supported by pillar (using
    /// <see cref="M:Mafi.Core.Factory.Transports.IPillarsChecker.GetMaxPillarHeightAt(Mafi.Tile2i)" />). This is exposed only for tests and works only
    /// in the PF domain.
    /// </summary>
    internal bool TestOnly_CanBeSupportedByPillar(Tile3i position)
    {
      RelTile3i coord = position - this.m_originCoord;
      return TransportPathFinder.isValidRelCoord(coord) && this.canBeSupportedByPillar((int) TransportPathFinder.indexFromCoord(coord));
    }

    /// <summary>
    /// Extends path-finding tree from <paramref name="parentNode" /> to <paramref name="nodeIndex" /> if they are
    /// connectable according to the const function. Nodes that are not connectable will not be added to the <see cref="F:Mafi.Core.Factory.Transports.TransportPathFinder.m_toProcessHeap" />.
    /// </summary>
    private void extendPath(
      int nodeIndex,
      int parentNodeIndex,
      TransportPathFinder.NodeMutable parentNode,
      bool ramp)
    {
      Assert.That<int>(nodeIndex).IsNotEqualTo(parentNodeIndex);
      Assert.That<bool>(parentNode.AreCollisionComputed).IsTrue();
      int num = nodeIndex & (int) short.MaxValue;
      int nodeIndex1 = (TransportPathFinder.coordFromIndex(nodeIndex) - TransportPathFinder.coordFromIndex(parentNodeIndex)).X == 0 ? num + (ramp ? 98304 : 32768) : num + (ramp ? 65536 : 0);
      ref TransportPathFinder.NodeMutable local = ref this.m_nodes[nodeIndex1];
      int outCost;
      if (local.HasAnyFlags(TransportPathFinder.NodeFlag.IsProcessed) || !this.tryGetStepCost(nodeIndex1, parentNodeIndex, out outCost))
        return;
      Assert.That<int>(outCost).IsPositive("Non-positive step cost.");
      int unsupportedLength = this.computeUnsupportedLength(nodeIndex1, parentNodeIndex);
      if (unsupportedLength > this.m_maxDistanceWithoutPillars)
        return;
      if (!local.IsVisited)
      {
        local.SetParent(parentNodeIndex, parentNode, outCost, (byte) unsupportedLength, -1);
        this.insertInHeapToBeProcessed(nodeIndex1, ref local);
      }
      else
      {
        if (!local.AreCollisionComputed)
        {
          int occupiedArrIndex;
          if (this.computeCollisionsReturnConnCollided(nodeIndex1, local.ParentOnPathIndex, out occupiedArrIndex))
          {
            local.SetParent(parentNodeIndex, parentNode, outCost, (byte) unsupportedLength, -1);
            this.insertInHeapToBeProcessed(nodeIndex1, ref local);
            return;
          }
          local.ExtraOccupiedTilesIndex = occupiedArrIndex;
        }
        int occupiedArrIndex1;
        if ((long) parentNode.CurrentCost + (long) outCost >= (long) local.CurrentCost || this.computeCollisionsReturnConnCollided(nodeIndex1, parentNodeIndex, out occupiedArrIndex1))
          return;
        local.SetParent(parentNodeIndex, parentNode, outCost, (byte) unsupportedLength, occupiedArrIndex1);
        this.insertInHeapToBeProcessed(nodeIndex1, ref local);
        Assert.That<bool>(local.IsVisited).IsTrue();
      }
    }

    private int computeUnsupportedLength(int nodeIndex, int parentIndex)
    {
      return !this.m_checkPillars || this.canBeSupportedByPillar(nodeIndex) ? 0 : (int) this.m_nodes[parentIndex].UnsupportedLength + (TransportPathFinder.coord2FromIndex(nodeIndex) - TransportPathFinder.coord2FromIndex(parentIndex)).Sum.Abs();
    }

    /// <summary>
    /// Inserts given node to the <see cref="F:Mafi.Core.Factory.Transports.TransportPathFinder.m_toProcessHeap" /> with key based on heuristic and marks it visited.
    /// </summary>
    private void insertInHeapToBeProcessed(
      int nodeIndex,
      ref TransportPathFinder.NodeMutable nodeRef)
    {
      this.m_toProcessHeap.Push((Fix32) (int) nodeRef.CurrentCost + this.estPathCostToGoal(nodeIndex), nodeIndex);
      nodeRef.SetFlag(TransportPathFinder.NodeFlag.IsVisited);
    }

    /// <summary>
    /// Reconstructs found path. This should be called only if <see cref="M:Mafi.Core.Factory.Transports.TransportPathFinder.ContinuePathFinding(System.Int32@,Mafi.Collections.ImmutableCollections.ImmutableArray{Mafi.Tile3i}@)" /> returned <see cref="F:Mafi.PathFinding.PathFinderResult.PathFound" />.
    /// </summary>
    public void ReconstructPath(Lyst<Tile3i> foundPath, bool doNotOptimizePivots = false)
    {
      Assert.That<Lyst<Tile3i>>(foundPath).IsEmpty<Tile3i>("Output path list is not empty.");
      if (!this.m_pathFound)
        return;
      int i = this.m_goalNodeIndexFound;
      TransportPathFinder.NodeMutable node = this.m_nodes[i];
      while (i != TransportPathFinder.START_NODE_INDEX)
      {
        if (!node.IsVisited)
        {
          Log.Error(string.Format("Failed to reconstruct path. Unvisited node {0}", (object) i));
          break;
        }
        if (foundPath.Count > 200)
        {
          Log.Error(string.Format("Failed to reconstruct path. High count {0}", (object) foundPath.Count));
          break;
        }
        foundPath.Add(TransportPathFinder.coordFromIndex(i) + this.m_originCoord);
        i = node.ParentOnPathIndex;
        node = this.m_nodes[i];
      }
      foundPath.Add(TransportPathFinder.START_REL_COORD + this.m_originCoord);
      foundPath.Reverse();
      if (doNotOptimizePivots)
        return;
      TransportTrajectory.OptimizePivots(foundPath);
    }

    /// <summary>
    /// Fills given list with nodes that were explored during last or current path-finding run.
    /// </summary>
    public void GetExploredTiles(Lyst<TransportPfExploredTile> result)
    {
      Assert.That<Lyst<TransportPfExploredTile>>(result).IsEmpty<TransportPfExploredTile>();
    }

    private void expandNeighborsTowards(
      int nodeIndex,
      TransportPathFinder.NodeMutable node,
      Direction903d direction)
    {
      bool flag = nodeIndex == TransportPathFinder.START_NODE_INDEX;
      RelTile3i relTile3i = TransportPathFinder.coordFromIndex(node.ParentOnPathIndex) - TransportPathFinder.coordFromIndex(nodeIndex);
      RelTile3i dirVec = direction.ToTileDirection();
      if (flag)
      {
        if (this.Options.BannedStartDirections.IsNotEmpty && this.Options.BannedStartDirections.Contains(direction) || this.Options.ForcedStartDirection.HasValue && this.Options.ForcedStartDirection.Value != direction)
          return;
      }
      else if (relTile3i.IsZero || relTile3i.IsParallelTo(dirVec))
        return;
      RelTile3i relCoord = TransportPathFinder.coordFromIndex(nodeIndex);
      tryExtendTo(relCoord + dirVec, false);
      if (!this.m_proto.CanGoUpDown || flag && this.m_startMustBeFlat || !this.m_proto.ZStepLength.IsNotZero || flag && (this.Options.Flags & TransportPathFinderFlags.BanStartRampsInX) != TransportPathFinderFlags.None && dirVec.X != 0 || flag && (this.Options.Flags & TransportPathFinderFlags.BanStartRampsInY) != TransportPathFinderFlags.None && dirVec.Y != 0 || flag && this.m_startMustNotHavePerpendicularRamp && (this.m_startAllowedRampDirection.DirectionVector.X != 0 && dirVec.X == 0 || this.m_startAllowedRampDirection.DirectionVector.Y != 0 && dirVec.Y == 0))
        return;
      tryExtendTo((relCoord + dirVec * this.m_proto.ZStepLength.Value).Xy.ExtendZ(relCoord.Z + 1), true);
      tryExtendTo((relCoord + dirVec * this.m_proto.ZStepLength.Value).Xy.ExtendZ(relCoord.Z - 1), true);

      void tryExtendTo(RelTile3i newRelPosition, bool ramp)
      {
        if (!TransportPathFinder.isValidRelCoord(newRelPosition))
          return;
        int num = (int) TransportPathFinder.indexFromCoord(newRelPosition);
        if (this.m_goalMustBeFlat && this.isGoalIndex(num) && relCoord.Z != newRelPosition.Z)
          return;
        if (dirVec.Z != 0)
        {
          Tile3i position = this.m_originCoord + TransportPathFinder.coordFromIndex(num);
          Transport entity;
          if (this.m_occupancyManager.TryGetOccupyingEntityAt<Transport>(position, out entity, this.m_transportsPredicates.IgnorePillarsPredicate) && (Proto) this.m_proto.PortsShape == (Proto) entity.Prototype.PortsShape)
          {
            if (position == entity.StartPosition)
            {
              if (entity.StartInputPort.IsConnected)
                return;
            }
            else if (!(position == entity.EndPosition) || entity.EndOutputPort.IsConnected)
              return;
          }
        }
        this.extendPath(num, nodeIndex, node, ramp);
      }
    }

    static TransportPathFinder()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TransportPathFinder.MAX_REL_COORD = new RelTile3i(63, 63, 7);
      TransportPathFinder.START_REL_COORD = new RelTile3i(32, 32, 4);
      TransportPathFinder.START_NODE_INDEX = (int) TransportPathFinder.indexFromCoord(TransportPathFinder.START_REL_COORD);
    }

    [ExpectedStructSize(12)]
    private struct NodeMutable
    {
      /// <summary>
      /// Packing for several properties to keep us in 8B. This contains the parent path index (18 bits)
      /// packed into the least significant bits, the then flags (6 bits, only 3 required) and finally
      /// the unsupported length (8 bits).
      /// </summary>
      private uint m_data;
      /// <summary>
      /// Index to an auxiliary array that stores extra occupied tiles that needs to be checked for collisions.
      /// This is typically coming from up/down segments.
      /// </summary>
      public int ExtraOccupiedTilesIndex;

      /// <summary>
      /// Current minimal cost of the path to this node from start. Is set to int.MaxValue if the current node
      /// was not verified for collisions.
      /// </summary>
      public uint CurrentCost { get; private set; }

      /// <summary>
      /// Predecessor on the path from this node. It is used when reconstructing the path. Parent of the
      /// very first node on the path is set to itself by definition. When there is no parent (node was not
      /// processed yet, the value is <c>int.MaxValue</c>.
      /// </summary>
      public int ParentOnPathIndex
      {
        readonly get => (int) this.m_data & 262143;
        private set => this.m_data = (uint) ((int) this.m_data & -262144 | value & 262143);
      }

      /// <summary>
      /// Length of transport that is currently unsupported by pillars. Is zero at positions where pillar exists.
      /// </summary>
      public byte UnsupportedLength
      {
        readonly get => (byte) (this.m_data >> 24);
        private set => this.m_data = (uint) ((int) this.m_data & 16777215 | (int) value << 24);
      }

      private TransportPathFinder.NodeFlag m_flags
      {
        readonly get => (TransportPathFinder.NodeFlag) ((this.m_data & 16515072U) >> 18);
        set => this.m_data = (uint) ((int) this.m_data & -16515073 | (int) value << 18);
      }

      /// <summary>
      /// Whether this node was visited (present in heap) but its neighbors were not explored yet.
      /// </summary>
      public readonly bool IsVisited
      {
        get => (this.m_flags & TransportPathFinder.NodeFlag.IsVisited) != 0;
      }

      /// <summary>
      /// Whether this node is processed all its neighbors have been explored.
      /// </summary>
      public readonly bool IsProcessed
      {
        get => (this.m_flags & TransportPathFinder.NodeFlag.IsProcessed) != 0;
      }

      public readonly bool IsBanned => (this.m_flags & TransportPathFinder.NodeFlag.IsBanned) != 0;

      public readonly bool AreCollisionComputed => this.ExtraOccupiedTilesIndex >= 0;

      public void InitializeAsStart(byte unsupportedLength)
      {
        this.ParentOnPathIndex = TransportPathFinder.START_NODE_INDEX;
        this.UnsupportedLength = unsupportedLength;
      }

      public void InitializeAsGoal(int indexSelf) => this.ParentOnPathIndex = indexSelf;

      public void SetParent(
        int parentNodeIndex,
        TransportPathFinder.NodeMutable parentNode,
        int stepCost,
        byte unsupportedLength,
        int occupiedTilesSegmentFromParentIndex)
      {
        this.ParentOnPathIndex = parentNodeIndex;
        this.CurrentCost = (uint) ((ulong) parentNode.CurrentCost + (ulong) stepCost);
        this.UnsupportedLength = unsupportedLength;
        this.ExtraOccupiedTilesIndex = occupiedTilesSegmentFromParentIndex;
      }

      public void SetFlag(TransportPathFinder.NodeFlag flag) => this.m_flags |= flag;

      public void ClearFlag(TransportPathFinder.NodeFlag flag) => this.m_flags &= ~flag;

      public readonly bool HasAnyFlags(TransportPathFinder.NodeFlag flags)
      {
        return (this.m_flags & flags) != 0;
      }
    }

    [Flags]
    private enum NodeFlag : byte
    {
      None = 0,
      IsVisited = 1,
      IsProcessed = 2,
      IsBanned = 4,
    }
  }
}
