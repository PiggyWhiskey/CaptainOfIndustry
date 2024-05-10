// Decompiled with JetBrains decompiler
// Type: Mafi.Core.PathFinding.ClearancePathabilityProvider
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Console;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Core.Roads;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Mafi.Core.PathFinding
{
  /// <summary>
  /// Data structure that provides efficient querying of navigability based on terrain slope, height clearance, and tile
  /// pathability.
  /// 
  /// Additionally this same data structure keeps track pf path-finding nodes. This is makes everything easier, even
  /// though the class is more complex.
  /// </summary>
  /// <remarks>
  /// This class only stores data that has been queried (lazy evaluation). This may cause queries into new or stale
  /// chunks significantly slower.
  /// 
  /// This class can do a pathability query for area of NxN tiles in just N logical AND operations and one if. It is
  /// implemented using overlapping data chunks that store pathability data encoded in ulong.
  /// 
  /// Currently, chunks are 8x8 tiles with extra 4 tiles of overlap which makes them 12x12. Each tile stores 5 bits of
  /// data which results in 60 bits per row that is conveniently stored in an ulong.
  /// 
  /// The overlap allows queries for size up to (overlap + 1), in our case 5x5.
  /// 
  /// Note that this class will work with dynamic terrain (adding chunks during the game) only when there are no PF
  /// nodes on the newly created chunks. This can be achieved by having non-pathable areas around map edges.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class ClearancePathabilityProvider : IPathabilityProvider
  {
    public static readonly ThicknessTilesF FLAT_STEEPNESS_DELTA;
    public static readonly ThicknessTilesF MAX_STEEPNESS_DELTA;
    public static readonly ThicknessTilesI BASIC_HEIGHT_CLEARANCE;
    public static readonly ThicknessTilesI MAX_HEIGHT_CLEARANCE;
    internal const int CHUNK_SIZE_BITS = 3;
    internal const int CHUNK_SIZE = 8;
    internal const int CHUNK_SIZE_MASK = 7;
    internal const int CHUNK_SIZE_INCL_OVERLAP = 12;
    public const uint STEEPNESS_SLIGHT_SLOPE = 1;
    public const uint STEEPNESS_STEEP_SLOPE = 3;
    public const uint ALLOW_SLIGHT_SLOPE = 2;
    public const uint ALLOW_NO_SLOPE = 3;
    public const uint HEIGHT_CLEARANCE_LOW = 4;
    public const uint HEIGHT_CLEARANCE_BLOCKED = 12;
    public const uint ALLOW_LOW_CLEARANCE = 8;
    public const uint ALLOW_MAX_CLEARANCE = 12;
    public const uint TILE_FREE = 0;
    public const uint TILE_BLOCKED = 16;
    public const int MAX_QUERY_CLEARANCE = 5;
    [RenamedInVersion(109, "m_terrainManager")]
    public readonly TerrainManager TerrainManager;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly IVehicleSurfaceProvider m_vehicleSurfaceProvider;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<uint, ClearancePathabilityProvider.DataChunk> m_data;
    [DoNotSave(0, null)]
    private Option<Chunk8Index[]> m_chunkIndicesToLoad;
    private readonly ulong[] m_pathabilityCapabilities;
    [DoNotSave(0, null)]
    private int m_chunksCountX;
    [DoNotSave(0, null)]
    private byte[] m_chunkRoadConnectionsCount;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<int, Lyst<GraphTerrainConnection>> m_chunkRoadConnections;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    private Chunk8Index[] m_initializedChunks
    {
      get
      {
        return this.m_data.Values.Select<ClearancePathabilityProvider.DataChunk, Chunk8Index>((Func<ClearancePathabilityProvider.DataChunk, Chunk8Index>) (x => this.TerrainManager.GetChunk8Index(x.OriginTile.TileCoord))).ToArray<Chunk8Index>();
      }
      set => this.m_chunkIndicesToLoad = (Option<Chunk8Index[]>) value;
    }

    internal IReadOnlyDictionary<uint, ClearancePathabilityProvider.DataChunk> Chunks
    {
      get => (IReadOnlyDictionary<uint, ClearancePathabilityProvider.DataChunk>) this.m_data;
    }

    public event Action<ClearancePathabilityProvider.DataChunk> OnChunkCreated;

    public event Action<ClearancePathabilityProvider.DataChunk> OnChunkUpdated;

    public event Action AllChunksCleared;

    public int DataChunksCount => this.m_data.Count;

    /// <summary>
    /// This gets incremented every time a <see cref="T:Mafi.Core.PathFinding.ClearancePathabilityProvider.DataChunk" /> or <see cref="T:Mafi.Core.PathFinding.ClearancePathabilityProvider.CapabilityChunkData" /> gets
    /// recomputed. Since re-computation is relatively expensive, this can be used by caller to ensure PF is not
    /// taking too long when many chunks are being recomputed.
    /// </summary>
    public int RecomputedChunksCount { get; private set; }

    public ClearancePathabilityProvider(
      TerrainManager terrainManager,
      TerrainOccupancyManager occupancyManager,
      IVehicleSurfaceProvider vehicleSurfaceProvider,
      ProtosDb protosDb,
      IGameLoopEvents gameLoopEvents,
      IRoadsManager roadsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(terrainManager, occupancyManager, vehicleSurfaceProvider, protosDb.All<DrivingEntityProto>().Select<DrivingEntityProto, VehiclePathFindingParams>((Func<DrivingEntityProto, VehiclePathFindingParams>) (x => x.PathFindingParams)).ToImmutableArray<VehiclePathFindingParams>(), gameLoopEvents, roadsManager);
    }

    internal ClearancePathabilityProvider(
      TerrainManager terrainManager,
      TerrainOccupancyManager occupancyManager,
      IVehicleSurfaceProvider vehicleSurfaceProvider,
      ImmutableArray<VehiclePathFindingParams> supportedPfParams,
      IGameLoopEvents gameLoopEvents,
      IRoadsManager roadsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_data = new Dict<uint, ClearancePathabilityProvider.DataChunk>();
      this.m_chunkRoadConnections = new Dict<int, Lyst<GraphTerrainConnection>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TerrainManager = terrainManager;
      this.m_occupancyManager = occupancyManager;
      this.m_vehicleSurfaceProvider = vehicleSurfaceProvider;
      this.m_pathabilityCapabilities = supportedPfParams.Select<ulong>((Func<VehiclePathFindingParams, ulong>) (x => x.PathabilityQueryMask)).Distinct<ulong>().ToArray<ulong>();
      terrainManager.HeightChanged.Add<ClearancePathabilityProvider>(this, new Action<Tile2iAndIndex>(this.onTileHeightChanged));
      terrainManager.TileFlagsChanged.Add<ClearancePathabilityProvider>(this, new Action<Tile2iAndIndex, uint>(this.onTileFlagFlagChanged));
      occupancyManager.TileOccupancyChanged.Add<ClearancePathabilityProvider>(this, new Action<Tile2iAndIndex>(this.onClearanceChangedTile));
      vehicleSurfaceProvider.OnVehicleSurfaceChanged.Add<ClearancePathabilityProvider>(this, new Action<Tile2i>(this.onHeightChanged));
      gameLoopEvents.Terminate.AddNonSaveable<ClearancePathabilityProvider>(this, new Action(this.onTerminate));
      this.initCommon(roadsManager);
    }

    [InitAfterLoad(InitPriority.High)]
    private void initSelf(DependencyResolver resolver)
    {
      IEnumerable<ulong> ulongs = resolver.Resolve<ProtosDb>().All<DrivingEntityProto>().Select<DrivingEntityProto, ulong>((Func<DrivingEntityProto, ulong>) (x => x.PathFindingParams.PathabilityQueryMask)).Distinct<ulong>();
      Lyst<ulong> lyst = new Lyst<ulong>();
      lyst.AddRange(this.m_pathabilityCapabilities);
      foreach (ulong num in ulongs)
        lyst.AddIfNotPresent(num);
      if (lyst.Count > this.m_pathabilityCapabilities.Length)
        ReflectionUtils.SetField<ClearancePathabilityProvider>(this, "m_pathabilityCapabilities", (object) lyst.ToArray());
      if (this.m_chunkIndicesToLoad.HasValue)
      {
        int chunk8PerWidth = this.TerrainManager.Chunk8PerWidth;
        foreach (Chunk8Index chunk8Index in this.m_chunkIndicesToLoad.Value)
        {
          Tile2i tile2i = new Tile2i(chunk8Index.Value % chunk8PerWidth, chunk8Index.Value / chunk8PerWidth) * 8;
          ClearancePathabilityProvider.DataChunk chunk = new ClearancePathabilityProvider.DataChunk(this, this.TerrainManager[tile2i]);
          this.m_data.AddAndAssertNew(ClearancePathabilityProvider.getChunkKey(tile2i), chunk);
          chunk.RecomputeCoreData();
          this.connectNeighbors(chunk);
        }
        this.m_chunkIndicesToLoad = Option<Chunk8Index[]>.None;
      }
      else
        Log.Warning("No chunks to initialize");
      this.initCommon(resolver.Resolve<IRoadsManager>());
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelfLow(DependencyResolver resolver)
    {
      foreach (GraphTerrainConnection conn in resolver.Resolve<IRoadsManager>().TerrainGraphConnections.Values)
        this.roadConnectionAdded(conn);
    }

    private void initCommon(IRoadsManager roadsManager)
    {
      roadsManager.RoadConnectionAdded += new Action<GraphTerrainConnection>(this.roadConnectionAdded);
      roadsManager.RoadConnectionRemoved += new Action<GraphTerrainConnection>(this.roadConnectionRemoved);
      this.m_chunksCountX = this.TerrainManager.TerrainWidth >> 3;
      this.m_chunkRoadConnectionsCount = new byte[this.m_chunksCountX * (this.TerrainManager.TerrainWidth >> 3)];
    }

    private void notifyChunkUpdated(ClearancePathabilityProvider.DataChunk chunk)
    {
      Action<ClearancePathabilityProvider.DataChunk> onChunkUpdated = this.OnChunkUpdated;
      if (onChunkUpdated != null)
        onChunkUpdated(chunk);
      ++this.RecomputedChunksCount;
    }

    /// <summary>
    /// Returns an existing chunk at given coordinate. This chunk might not have all neighbors and might be dirty.
    /// </summary>
    public Option<ClearancePathabilityProvider.DataChunk> GetChunkAt(Tile2i coord)
    {
      return this.m_data.Get<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(coord));
    }

    /// <summary>
    /// Returns existing chunk or creates a new chunk at given coordinate. This chunk might not have all neighbors
    /// and might be dirty.
    /// </summary>
    public bool TryGetOrCreateChunkAt(
      Tile2i coord,
      out ClearancePathabilityProvider.DataChunk chunk,
      bool ensureOverlap = false)
    {
      if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(coord), out chunk))
        return true;
      Tile2i originTile = ClearancePathabilityProvider.getOriginTile(coord);
      if (!this.TerrainManager.IsValidCoord(originTile))
        return false;
      chunk = this.createAndConnectChunkAt(originTile, ensureOverlap);
      return true;
    }

    public Option<PfNode> GetPfNodeAt(Tile2i coord, int capabilityIndex)
    {
      Assert.That<int>(capabilityIndex).IsValidIndexFor<ulong>(this.m_pathabilityCapabilities);
      Option<ClearancePathabilityProvider.DataChunk> chunkAt = this.GetChunkAt(coord);
      if (chunkAt.IsNone)
        return Option<PfNode>.None;
      Option<ClearancePathabilityProvider.CapabilityChunkData> pfData = chunkAt.Value.GetPfData(capabilityIndex);
      return pfData.IsNone ? Option<PfNode>.None : pfData.Value.GetNodeAt(coord);
    }

    /// <summary>
    /// Returns or creates a valid PF node at given coord. If a node is returned, it is guaranteed to be not dirty
    /// if no nodes are visited and have all neighbors that are also not dirty if no nodes are visited there.
    /// </summary>
    public Option<PfNode> GetOrCreatePfNodeAt(Tile2i coord, int capabilityIndex)
    {
      Assert.That<int>(capabilityIndex).IsValidIndexFor<ulong>(this.m_pathabilityCapabilities);
      ClearancePathabilityProvider.DataChunk chunk;
      if (!this.TryGetOrCreateChunkAt(coord, out chunk))
        return Option<PfNode>.None;
      ClearancePathabilityProvider.CapabilityChunkData pfData = chunk.GetOrCreatePfData(capabilityIndex);
      pfData.EnsureNotDirtyIfNoNodesVisited();
      return pfData.GetNodeAndEnsurePfNeighbors(coord);
    }

    /// <summary>
    /// Returns mask that can be used to efficiently query pathability using <see cref="M:Mafi.Core.PathFinding.ClearancePathabilityProvider.IsPathableRaw(Mafi.Tile2i,System.UInt64)" />.
    /// </summary>
    /// <param name="clearanceSize">Width and height of required clearance area.</param>
    /// <param name="heightClearance">Required height clearance for entire area.</param>
    public static ulong GetPathabilityQueryMask(
      RelTile1i clearanceSize,
      SteepnessPathability steepness,
      HeightClearancePathability heightClearance)
    {
      return ClearancePathabilityProvider.GetPathabilityMaskRaw(clearanceSize, steepness, heightClearance) | (ulong) (uint) clearanceSize.Value << 56;
    }

    /// <summary>Returns raw mask without clearance encoded.</summary>
    public static ulong GetPathabilityMaskRaw(
      RelTile1i clearanceSize,
      SteepnessPathability steepness,
      HeightClearancePathability heightClearance)
    {
      Assert.That<RelTile1i>(clearanceSize).IsWithinIncl(1, 5);
      ulong pathabilityMaskRaw = 16UL | (ulong) heightClearance | (ulong) steepness;
      for (int index = 1; index < clearanceSize.Value; ++index)
        pathabilityMaskRaw |= pathabilityMaskRaw << 5;
      return pathabilityMaskRaw;
    }

    /// <summary>Returns an index for given pathability capability.</summary>
    public int GetPathabilityClassIndex(ulong pathabilityMask)
    {
      int pathabilityClassIndex = Array.IndexOf<ulong>(this.m_pathabilityCapabilities, pathabilityMask);
      if (pathabilityClassIndex >= 0)
        return pathabilityClassIndex;
      Log.Error("Unknown pathability class: 0b" + pathabilityMask.AsBin() + ". Only classes on registered driving entities can be used.");
      return 0;
    }

    public bool IsTilePathable(Tile2i tileCoord, ulong pathabilityMask)
    {
      Assert.That<RelTile1i>(ClearancePathabilityProvider.ExtractClearanceFromMask(ref pathabilityMask)).IsEqualTo(RelTile1i.One);
      ClearancePathabilityProvider.DataChunk chunk;
      return this.TryGetOrCreateChunkAt(tileCoord, out chunk, true) && chunk.IsTilePathable(tileCoord, pathabilityMask);
    }

    public bool IsPathable(Tile2i centerCoord, ulong pathabilityMask)
    {
      Assert.That<ulong>(pathabilityMask).IsNotEqualTo<ulong>(0UL, "Zero is not valid pathability mask.");
      RelTile1i clearanceFromMask = ClearancePathabilityProvider.ExtractClearanceFromMask(ref pathabilityMask);
      Tile2i cornerTileSpace = VehiclePathFindingParams.ConvertToCornerTileSpace(centerCoord, clearanceFromMask);
      ClearancePathabilityProvider.DataChunk chunk;
      return this.TryGetOrCreateChunkAt(cornerTileSpace, out chunk, true) && chunk.IsPathable(cornerTileSpace, clearanceFromMask, pathabilityMask);
    }

    public bool IsPathableIgnoringTerrain(Tile2i centerCoord, ulong pathabilityMask)
    {
      Assert.That<ulong>(pathabilityMask).IsNotEqualTo<ulong>(0UL, "Zero is not valid pathability mask.");
      RelTile1i clearanceFromMask = ClearancePathabilityProvider.ExtractClearanceFromMask(ref pathabilityMask);
      for (int index = 0; index < clearanceFromMask.Value; ++index)
      {
        int num = index * 5;
        pathabilityMask &= (ulong) ~(3L << num);
      }
      Tile2i cornerTileSpace = VehiclePathFindingParams.ConvertToCornerTileSpace(centerCoord, clearanceFromMask);
      ClearancePathabilityProvider.DataChunk chunk;
      return this.TryGetOrCreateChunkAt(cornerTileSpace, out chunk, true) && chunk.IsPathable(cornerTileSpace, clearanceFromMask, pathabilityMask);
    }

    /// <summary>
    /// Tests whether an area specified by <paramref name="pathabilityMask" /> is pathable. Mask can be obtained via <see cref="M:Mafi.Core.PathFinding.ClearancePathabilityProvider.GetPathabilityQueryMask(Mafi.RelTile1i,Mafi.Core.PathFinding.SteepnessPathability,Mafi.Core.PathFinding.HeightClearancePathability)" />.
    /// </summary>
    public bool IsPathableRaw(Tile2i cornerCoord, ulong pathabilityMask)
    {
      Assert.That<ulong>(pathabilityMask).IsNotEqualTo<ulong>(0UL, "Zero is not valid pathability mask.");
      RelTile1i clearanceFromMask = ClearancePathabilityProvider.ExtractClearanceFromMask(ref pathabilityMask);
      ClearancePathabilityProvider.DataChunk chunk;
      return this.TryGetOrCreateChunkAt(cornerCoord, out chunk, true) && chunk.IsPathable(cornerCoord, clearanceFromMask, pathabilityMask);
    }

    public ulong GetPathabilityMask(VehiclePathFindingParams pfParams)
    {
      return ClearancePathabilityProvider.GetPathabilityQueryMask(pfParams.RequiredClearance, pfParams.SteepnessPathability, pfParams.HeightClearancePathability);
    }

    public ulong GetPathabilityMaskSingleTile(VehiclePathFindingParams pfParams)
    {
      return ClearancePathabilityProvider.GetPathabilityQueryMask(RelTile1i.One, pfParams.SteepnessPathability, pfParams.HeightClearancePathability);
    }

    public Option<PfNode> FindClosestValidPfNode(
      Tile2i coord,
      int capabilityIndex,
      Predicate<PfNode> predicate = null)
    {
      Tile2iSlim centerOrigin = (coord + new RelTile2i(8, 8) / 2).AsSlim;
      Lyst<PfNode> closestNodes = (Lyst<PfNode>) null;
      long closestDistSqr = long.MaxValue;
      for (int radius = 1; radius <= 4; ++radius)
      {
        MafiMath.IterateCirclePoints(radius, new Action<int, int>(testCoord));
        if (closestNodes != null)
          return (Option<PfNode>) closestNodes.MinElement<PfNode, long>((Func<PfNode, long>) (x => x.Area.CenterCoord.DistanceSqrTo(coord)));
      }
      foreach (ClearancePathabilityProvider.DataChunk dataChunk in this.m_data.Values)
      {
        Option<ClearancePathabilityProvider.CapabilityChunkData> pfData = dataChunk.GetPfData(capabilityIndex);
        if (!pfData.IsNone)
        {
          Lyst<PfNode> nodes = pfData.Value.Nodes;
          foreach (PfNode pfNode in nodes)
          {
            if (predicate == null || predicate(pfNode))
            {
              long num = (long) dataChunk.OriginTile.TileCoordSlim.DistanceSqrTo(centerOrigin);
              if (num < closestDistSqr)
              {
                closestDistSqr = num;
                closestNodes = nodes;
                break;
              }
              break;
            }
          }
        }
      }
      return closestNodes == null ? Option<PfNode>.None : (Option<PfNode>) closestNodes.MinElement<PfNode, long>((Func<PfNode, long>) (x => x.Area.CenterCoord.DistanceSqrTo(coord)));

      void testCoord(int dx, int dy)
      {
        ClearancePathabilityProvider.DataChunk chunk;
        if (!this.TryGetOrCreateChunkAt(coord + new RelTile2i(dx * 8, dy * 8), out chunk))
          return;
        Option<ClearancePathabilityProvider.CapabilityChunkData> pfData = chunk.GetPfData(capabilityIndex);
        if (pfData.IsNone)
          return;
        ClearancePathabilityProvider.CapabilityChunkData capabilityChunkData = pfData.Value;
        capabilityChunkData.EnsureNotDirtyIfNoNodesVisited();
        Lyst<PfNode> nodes = capabilityChunkData.Nodes;
        foreach (PfNode pfNode in nodes)
        {
          if (predicate == null || predicate(pfNode))
          {
            long num = (long) chunk.OriginTile.TileCoordSlim.DistanceSqrTo(centerOrigin);
            if (num >= closestDistSqr)
              break;
            closestDistSqr = num;
            closestNodes = nodes;
            break;
          }
        }
      }
    }

    internal void RecomputeDataAt(Tile2i coord)
    {
      ClearancePathabilityProvider.DataChunk dataChunk;
      if (!this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(coord), out dataChunk) || !dataChunk.AllNeighborsEnsured)
        return;
      TerrainTile tile = this.TerrainManager[coord];
      dataChunk.RecomputeDataAt(tile);
    }

    /// <summary>
    /// Extracts clearance information from given pathability mask.
    /// </summary>
    internal static RelTile1i ExtractClearanceFromMask(ref ulong mask)
    {
      int num = (int) (mask >> 56);
      mask &= 72057594037927935UL;
      return new RelTile1i(num);
    }

    public void NotifyTileHeightChanged(Tile2i tile) => this.onHeightChanged(tile);

    private void onTileHeightChanged(Tile2iAndIndex tile) => this.onHeightChanged(tile.TileCoord);

    private void onHeightChanged(Tile2i tile)
    {
      ClearancePathabilityProvider.DataChunk dataChunk1;
      if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(tile), out dataChunk1))
        dataChunk1.MarkSteepnessDirty(tile);
      ClearancePathabilityProvider.DataChunk dataChunk2;
      if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(tile.DecrementX), out dataChunk2))
        dataChunk2.MarkSteepnessDirty(tile.DecrementX);
      ClearancePathabilityProvider.DataChunk dataChunk3;
      if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(tile.DecrementY), out dataChunk3))
        dataChunk3.MarkSteepnessDirty(tile.DecrementY);
      this.onClearanceChanged(tile);
    }

    private void onClearanceChangedTile(Tile2iAndIndex tile)
    {
      this.onClearanceChanged(tile.TileCoord);
    }

    private void onClearanceChanged(Tile2i tileCoord)
    {
      ClearancePathabilityProvider.DataChunk dataChunk1;
      if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(tileCoord), out dataChunk1))
        dataChunk1.MarkHeightClearanceDirty(tileCoord);
      ClearancePathabilityProvider.DataChunk dataChunk2;
      if (!this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(tileCoord - RelTile2i.One), out dataChunk2))
        return;
      dataChunk2.MarkHeightClearanceDirty(tileCoord - RelTile2i.One);
    }

    private void onTileFlagFlagChanged(Tile2iAndIndex tile, uint changedFlags)
    {
      ClearancePathabilityProvider.DataChunk dataChunk;
      if (((int) changedFlags & (int) this.TerrainManager.BlocksVehiclesFlagsMask) == 0 || !this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(tile.TileCoord), out dataChunk))
        return;
      dataChunk.MarkPathabilityDirty(tile.TileCoord);
    }

    private void onTerminate()
    {
      Lyst<KeyValuePair<Tile2iSlim, string>> lyst = this.ValidateData();
      if (!lyst.IsNotEmpty)
        return;
      Log.Error(string.Format("{0} PF graph errors found during termination:\n", (object) lyst) + ((IEnumerable<string>) lyst.Select<string>((Func<KeyValuePair<Tile2iSlim, string>, string>) (kvp => kvp.Value))).JoinStrings("\n"));
    }

    /// <summary>
    /// Returns origin tile of a <see cref="T:Mafi.Core.PathFinding.ClearancePathabilityProvider.DataChunk" />.
    /// </summary>
    private static Tile2i getOriginTile(Tile2i tileCoord)
    {
      return new Tile2i(tileCoord.X & -8, tileCoord.Y & -8);
    }

    /// <summary>
    /// Returns a key that uniquely identifies <see cref="T:Mafi.Core.PathFinding.ClearancePathabilityProvider.DataChunk" /> that owns given <paramref name="tileCoord" />.
    /// </summary>
    private static uint getChunkKey(Tile2i tileCoord)
    {
      return (uint) ((tileCoord.X & -8) >>> 3 | (tileCoord.Y & -8) >>> 3 << 16);
    }

    /// <summary>
    /// Creates data chunk and ensures that it has chunks on its overlapping area.
    /// </summary>
    /// <remarks>
    /// Chunk that does not have neighbor at +X, +Y, or +XY also does not have updated data in its overlap in the
    /// respective direction. Only once a new chunk on the overlap is created, its data is copied to our overlap
    /// in <see cref="M:Mafi.Core.PathFinding.ClearancePathabilityProvider.connectNeighbors(Mafi.Core.PathFinding.ClearancePathabilityProvider.DataChunk)" />.
    /// </remarks>
    private ClearancePathabilityProvider.DataChunk createAndConnectChunkAt(
      Tile2i originCoord,
      bool ensureOverlap)
    {
      ClearancePathabilityProvider.DataChunk chunk = new ClearancePathabilityProvider.DataChunk(this, this.TerrainManager[originCoord]);
      this.m_data.AddAndAssertNew(ClearancePathabilityProvider.getChunkKey(originCoord), chunk);
      chunk.RecomputeCoreData();
      this.connectNeighbors(chunk);
      if (ensureOverlap)
        chunk.EnsureAllNeighbors();
      Action<ClearancePathabilityProvider.DataChunk> onChunkCreated = this.OnChunkCreated;
      if (onChunkCreated != null)
        onChunkCreated(chunk);
      return chunk;
    }

    /// <summary>
    /// Connects given chunk to available -x, -y, and -xy neighbors and copies data from the given chunk to the
    /// connected neighbors overlap areas.
    /// </summary>
    private void connectNeighbors(ClearancePathabilityProvider.DataChunk chunk)
    {
      if (chunk.MinusXNeighbor.IsNone)
      {
        ClearancePathabilityProvider.DataChunk dataChunk;
        if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddX(-8)), out dataChunk))
        {
          chunk.MinusXNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
          dataChunk.PlusXNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) chunk;
          for (int index = 0; index < 8; ++index)
            dataChunk.Data[index] = (ulong) ((long) dataChunk.Data[index] & -1152920405095219201L | ((long) chunk.Data[index] & 1048575L) << 40);
        }
      }
      else
        Assert.That<Dict<uint, ClearancePathabilityProvider.DataChunk>>(this.m_data).ContainsKeyValue<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddX(-8)), chunk.MinusXNeighbor.Value);
      if (chunk.MinusYNeighbor.IsNone)
      {
        ClearancePathabilityProvider.DataChunk dataChunk;
        if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddY(-8)), out dataChunk))
        {
          chunk.MinusYNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
          dataChunk.PlusYNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) chunk;
          for (int index = 0; index < 4; ++index)
            dataChunk.Data[index + 8] = (ulong) ((long) dataChunk.Data[index + 8] & -1099511627776L | (long) chunk.Data[index] & 1099511627775L);
        }
      }
      else
        Assert.That<Dict<uint, ClearancePathabilityProvider.DataChunk>>(this.m_data).ContainsKeyValue<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddY(-8)), chunk.MinusYNeighbor.Value);
      if (chunk.MinusXyNeighbor.IsNone)
      {
        ClearancePathabilityProvider.DataChunk dataChunk;
        if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddXy(-8)), out dataChunk))
        {
          chunk.MinusXyNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
          dataChunk.PlusXyNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) chunk;
          for (int index = 0; index < 4; ++index)
            dataChunk.Data[index + 8] = (ulong) ((long) dataChunk.Data[index + 8] & -1152920405095219201L | ((long) chunk.Data[index] & 1048575L) << 40);
        }
      }
      else
        Assert.That<Dict<uint, ClearancePathabilityProvider.DataChunk>>(this.m_data).ContainsKeyValue<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddXy(-8)), chunk.MinusXyNeighbor.Value);
      ClearancePathabilityProvider.DataChunk dataChunk1;
      if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddX(8)), out dataChunk1))
      {
        chunk.PlusXNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk1;
        dataChunk1.MinusXNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) chunk;
        if (dataChunk1.IsDirty)
          dataChunk1.RecomputeDirty();
        for (int index = 0; index < 8; ++index)
          chunk.Data[index] = (ulong) ((long) chunk.Data[index] & -1152920405095219201L | ((long) dataChunk1.Data[index] & 1048575L) << 40);
      }
      ClearancePathabilityProvider.DataChunk dataChunk2;
      if (this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddY(8)), out dataChunk2))
      {
        chunk.PlusYNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk2;
        dataChunk2.MinusYNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) chunk;
        if (dataChunk2.IsDirty)
          dataChunk2.RecomputeDirty();
        for (int index = 0; index < 4; ++index)
          chunk.Data[index + 8] = (ulong) ((long) chunk.Data[index + 8] & -1099511627776L | (long) dataChunk2.Data[index] & 1099511627775L);
      }
      ClearancePathabilityProvider.DataChunk dataChunk3;
      if (!this.m_data.TryGetValue(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddXy(8)), out dataChunk3))
        return;
      chunk.PlusXyNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk3;
      dataChunk3.MinusXyNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) chunk;
      if (dataChunk3.IsDirty)
        dataChunk3.RecomputeDirty();
      for (int index = 0; index < 4; ++index)
        chunk.Data[index + 8] = (ulong) ((long) chunk.Data[index + 8] & -1152920405095219201L | ((long) dataChunk3.Data[index] & 1048575L) << 40);
    }

    private ulong getEncodedHeightClearance(Tile2i tile)
    {
      bool isAccessible1;
      HeightTilesF? nullable = this.m_vehicleSurfaceProvider.GetEntityVehicleSurfaceAt(tile, out isAccessible1);
      if (nullable.HasValue)
      {
        HeightTilesF rhs = nullable.Value;
        bool isAccessible2;
        HeightTilesF? vehicleSurfaceAt = this.m_vehicleSurfaceProvider.GetEntityVehicleSurfaceAt(tile.AddXy(1), out isAccessible2);
        ref HeightTilesF? local = ref vehicleSurfaceAt;
        nullable = local.HasValue ? new HeightTilesF?(local.GetValueOrDefault().Max(rhs)) : new HeightTilesF?();
        isAccessible1 &= isAccessible2;
      }
      HeightTilesI height1;
      ThicknessTilesI thicknessTilesI;
      if (nullable.HasValue)
      {
        if (!isAccessible1)
          return 12;
        HeightTilesF heightTilesF = nullable.Value + ThicknessTilesF.Epsilon;
        height1 = heightTilesF.TilesHeightCeiled;
        heightTilesF = nullable.Value;
        thicknessTilesI = heightTilesF.TilesHeightCeiled + ClearancePathabilityProvider.BASIC_HEIGHT_CLEARANCE - height1;
      }
      else
      {
        HeightTilesF height2 = this.TerrainManager.GetHeight(tile);
        height1 = height2.TilesHeightFloored;
        thicknessTilesI = (height2 + 0.9.TilesThick()).TilesHeightFloored + ClearancePathabilityProvider.BASIC_HEIGHT_CLEARANCE - height1;
      }
      ThicknessTilesI clearanceAt = this.m_occupancyManager.GetClearanceAt(tile.ExtendHeight(height1));
      return clearanceAt >= ClearancePathabilityProvider.MAX_HEIGHT_CLEARANCE ? 0UL : (clearanceAt >= thicknessTilesI ? 4UL : 12UL);
    }

    private ulong getEncodedSteepness(TerrainTile tile)
    {
      bool isAccessible;
      HeightTilesF? vehicleSurfaceAt = this.m_vehicleSurfaceProvider.GetEntityVehicleSurfaceAt((Tile2i) tile.TileCoordSlim, out isAccessible);
      HeightTilesF heightTilesF1 = vehicleSurfaceAt ?? tile.Height;
      vehicleSurfaceAt = this.m_vehicleSurfaceProvider.GetEntityVehicleSurfaceAt((Tile2i) tile.TileCoordSlim.IncrementX, out isAccessible);
      TerrainTile terrainTile;
      HeightTilesF heightTilesF2;
      if (!vehicleSurfaceAt.HasValue)
      {
        terrainTile = tile.PlusXNeighbor;
        heightTilesF2 = terrainTile.Height;
      }
      else
        heightTilesF2 = vehicleSurfaceAt.GetValueOrDefault();
      HeightTilesF heightTilesF3 = heightTilesF2;
      vehicleSurfaceAt = this.m_vehicleSurfaceProvider.GetEntityVehicleSurfaceAt((Tile2i) tile.TileCoordSlim.IncrementY, out isAccessible);
      HeightTilesF heightTilesF4;
      if (!vehicleSurfaceAt.HasValue)
      {
        terrainTile = tile.PlusYNeighbor;
        heightTilesF4 = terrainTile.Height;
      }
      else
        heightTilesF4 = vehicleSurfaceAt.GetValueOrDefault();
      HeightTilesF heightTilesF5 = heightTilesF4;
      ThicknessTilesF thicknessTilesF = (heightTilesF1 - heightTilesF3).Abs.Max((heightTilesF1 - heightTilesF5).Abs);
      return thicknessTilesF < ClearancePathabilityProvider.FLAT_STEEPNESS_DELTA ? 0UL : (thicknessTilesF <= ClearancePathabilityProvider.MAX_STEEPNESS_DELTA ? 1UL : 3UL);
    }

    [ConsoleCommand(false, false, null, null)]
    private GameCommandResult validatePfData()
    {
      Lyst<KeyValuePair<Tile2iSlim, string>> lyst = this.ValidateData();
      return !lyst.IsNotEmpty ? GameCommandResult.Success((object) "No errors found.") : GameCommandResult.Error(string.Format("{0} errors found:\n{1}", (object) lyst, (object) ((IEnumerable<string>) lyst.Select<string>((Func<KeyValuePair<Tile2iSlim, string>, string>) (kvp => kvp.Value))).JoinStrings("\n")));
    }

    /// <summary>
    /// Validates that all chunks have data matching the terrain and all overlapped areas have correctly
    /// propagated data.
    /// </summary>
    public Lyst<KeyValuePair<Tile2iSlim, string>> ValidateData()
    {
      Lyst<KeyValuePair<Tile2iSlim, string>> errors = new Lyst<KeyValuePair<Tile2iSlim, string>>();
      Lyst<Tile2iSlim> lyst1 = this.m_data.Values.Select<ClearancePathabilityProvider.DataChunk, Tile2iSlim>((Func<ClearancePathabilityProvider.DataChunk, Tile2iSlim>) (x => x.OriginTile.TileCoordSlim)).ToLyst<Tile2iSlim>();
      Set<Tile2iSlim> set = new Set<Tile2iSlim>(lyst1);
      if (lyst1.Count != set.Count)
      {
        lyst1.RemoveWhere(new Predicate<Tile2iSlim>(set.Contains));
        foreach (Tile2iSlim key in lyst1)
          errors.Add(Make.Kvp<Tile2iSlim, string>(key, "Repeated coord."));
      }
      foreach (KeyValuePair<uint, ClearancePathabilityProvider.DataChunk> keyValuePair in this.m_data)
      {
        uint key = keyValuePair.Key;
        ClearancePathabilityProvider.DataChunk chunk = keyValuePair.Value;
        chunk.ValidatePfData(errors);
        chunk.ValidateCoreData(errors);
        this.validateOverlapData(chunk, errors);
      }
      if (errors.IsNotEmpty && DebugGameRenderer.IsEnabled && DebugGameRendererConfig.SaveClearancePathabilityProviderValidationErrors)
      {
        Lyst<Tile2iSlim> lyst2 = ((IEnumerable<Tile2iSlim>) errors.Select<Tile2iSlim>((Func<KeyValuePair<Tile2iSlim, string>, Tile2iSlim>) (x => x.Key))).ToLyst<Tile2iSlim>();
        Tile2iSlim from = errors.First.Key;
        Tile2iSlim tile2iSlim = from;
        foreach (Tile2iSlim rhs in lyst2)
        {
          from = from.Min(rhs);
          tile2iSlim = tile2iSlim.Max(rhs);
        }
        from = from.AddXy(-10);
        tile2iSlim = tile2iSlim.AddXy(10);
        DebugGameRenderer.DrawGameImage((Tile2i) from, tile2iSlim.AsFull - (Tile2i) from + RelTile2i.One).DrawPathabilityOverlay().HighlightTiles((IEnumerable<Tile2iSlim>) lyst2, new ColorRgba(192, 0, 0, 192)).SaveMapAsTga("ClearancePathabilityErrors_Broken");
        this.RecomputeAllData();
        DebugGameRenderer.DrawGameImage((Tile2i) from, tile2iSlim.AsFull - (Tile2i) from + RelTile2i.One).DrawPathabilityOverlay().HighlightTiles((IEnumerable<Tile2iSlim>) lyst2, new ColorRgba(192, 0, 0, 192)).SaveMapAsTga("ClearancePathabilityErrors_Fixed");
      }
      return errors;
    }

    private void validateOverlapData(
      ClearancePathabilityProvider.DataChunk chunk,
      Lyst<KeyValuePair<Tile2iSlim, string>> errors)
    {
      Assert.That<bool>(chunk.IsDirty).IsFalse();
      Option<ClearancePathabilityProvider.DataChunk> option1 = this.m_data.Get<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddX(8)));
      Option<ClearancePathabilityProvider.DataChunk> option2 = this.m_data.Get<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddXy(8)));
      Option<ClearancePathabilityProvider.DataChunk> option3 = this.m_data.Get<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddY(8)));
      if (chunk.OriginTile.TileCoord.X > 0)
      {
        Option<ClearancePathabilityProvider.DataChunk> option4 = this.m_data.Get<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddX(-8)));
        if (chunk.MinusXNeighbor != option4)
          errors.Add(Make.Kvp<Tile2iSlim, string>(chunk.OriginTile.TileCoordSlim, string.Format("The -X neighbor of chunk at {0} ", (object) chunk.OriginTile) + (chunk.MinusXNeighbor.ValueOrNull?.OriginTile.TileCoordSlim.ToString() ?? "(null)") + " is not equal to the actual chunk at expected location " + (option4.ValueOrNull?.OriginTile.TileCoordSlim.ToString() ?? "(null)")));
      }
      if (chunk.OriginTile.TileCoord.Y > 0)
      {
        Option<ClearancePathabilityProvider.DataChunk> option5 = this.m_data.Get<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddY(-8)));
        if (chunk.MinusYNeighbor != option5)
          errors.Add(Make.Kvp<Tile2iSlim, string>(chunk.OriginTile.TileCoordSlim, string.Format("The -Y neighbor of chunk at {0} ", (object) chunk.OriginTile) + (chunk.MinusYNeighbor.ValueOrNull?.OriginTile.TileCoordSlim.ToString() ?? "(null)") + " is not equal to the actual chunk at expected location " + (option5.ValueOrNull?.OriginTile.TileCoordSlim.ToString() ?? "(null)")));
      }
      if (chunk.OriginTile.TileCoord > Tile2i.Zero)
      {
        Option<ClearancePathabilityProvider.DataChunk> option6 = this.m_data.Get<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(chunk.OriginTile.TileCoord.AddXy(-8)));
        if (chunk.MinusXyNeighbor != option6)
          errors.Add(Make.Kvp<Tile2iSlim, string>(chunk.OriginTile.TileCoordSlim, string.Format("The -XY neighbor of chunk at {0} ", (object) chunk.OriginTile) + (chunk.MinusXyNeighbor.ValueOrNull?.OriginTile.TileCoordSlim.ToString() ?? "(null)") + " is not equal to the actual chunk at expected location " + (option6.ValueOrNull?.OriginTile.TileCoordSlim.ToString() ?? "(null)")));
      }
      if (option1.HasValue)
      {
        if (option1.Value.IsDirty)
          option1.Value.RecomputeDirty();
        for (int y = 0; y < 8; ++y)
        {
          for (int x = 0; x < 4; ++x)
          {
            ulong rawDataAt1 = chunk.GetRawDataAt(x + 8, y);
            ulong rawDataAt2 = option1.Value.GetRawDataAt(x, y);
            if ((long) rawDataAt1 != (long) rawDataAt2)
            {
              string debugStr1 = ClearancePathabilityProvider.DataChunk.RawDataToDebugStr(rawDataAt1);
              string debugStr2 = ClearancePathabilityProvider.DataChunk.RawDataToDebugStr(rawDataAt2);
              errors.Add(Make.Kvp<Tile2iSlim, string>(chunk.OriginTile.TileCoordSlim + new RelTile2i(x + 8, y), string.Format("Overlap data of chunk {0} ", (object) chunk.OriginTile) + string.Format("at [{0},{1}] is not matching its +X neighbor chunk ", (object) (x + 8), (object) y) + string.Format("(ground truth) {0} at [{1},{2}]: ", (object) option1.Value.OriginTile, (object) x, (object) y) + "0b" + rawDataAt1.AsBin(5) + " " + debugStr1 + " != 0b" + rawDataAt2.AsBin(5) + " " + debugStr2));
            }
          }
        }
      }
      if (option3.HasValue)
      {
        if (option3.Value.IsDirty)
          option3.Value.RecomputeDirty();
        for (int y = 0; y < 4; ++y)
        {
          for (int x = 0; x < 8; ++x)
          {
            ulong rawDataAt3 = chunk.GetRawDataAt(x, y + 8);
            ulong rawDataAt4 = option3.Value.GetRawDataAt(x, y);
            if ((long) rawDataAt3 != (long) rawDataAt4)
            {
              string debugStr3 = ClearancePathabilityProvider.DataChunk.RawDataToDebugStr(rawDataAt3);
              string debugStr4 = ClearancePathabilityProvider.DataChunk.RawDataToDebugStr(rawDataAt4);
              errors.Add(Make.Kvp<Tile2iSlim, string>(chunk.OriginTile.TileCoordSlim + new RelTile2i(x, y + 8), string.Format("Overlap data of chunk {0} ", (object) chunk.OriginTile) + string.Format("at [{0},{1}] is not matching its +Y neighbor chunk ", (object) x, (object) (y + 8)) + string.Format("(ground truth) {0} at [{1},{2}]: ", (object) option3.Value.OriginTile, (object) x, (object) y) + "0x" + rawDataAt3.AsBin(5) + " " + debugStr3 + " != 0x" + rawDataAt4.AsBin(5) + " " + debugStr4));
            }
          }
        }
      }
      if (!option2.HasValue)
        return;
      if (option2.Value.IsDirty)
        option2.Value.RecomputeDirty();
      for (int y = 0; y < 4; ++y)
      {
        for (int x = 0; x < 4; ++x)
        {
          ulong rawDataAt5 = chunk.GetRawDataAt(x + 8, y + 8);
          ulong rawDataAt6 = option2.Value.GetRawDataAt(x, y);
          if ((long) rawDataAt5 != (long) rawDataAt6)
          {
            string debugStr5 = ClearancePathabilityProvider.DataChunk.RawDataToDebugStr(rawDataAt5);
            string debugStr6 = ClearancePathabilityProvider.DataChunk.RawDataToDebugStr(rawDataAt6);
            errors.Add(Make.Kvp<Tile2iSlim, string>(chunk.OriginTile.TileCoordSlim + new RelTile2i(x + 8, y + 8), string.Format("Overlap data of chunk {0} ", (object) chunk.OriginTile) + string.Format("at [{0},{1}] is not matching its +Xy neighbor chunk ", (object) (x + 8), (object) (y + 8)) + string.Format("(ground truth) {0} at [{1},{2}]: ", (object) option2.Value.OriginTile, (object) x, (object) y) + "0x" + rawDataAt5.AsBin(5) + " " + debugStr5 + " != 0x" + rawDataAt6.AsBin(5) + " " + debugStr6));
          }
        }
      }
    }

    /// <summary>
    /// Returns human-friendly pathability info. Handy for debugging and can be invoked via immediate console.
    /// </summary>
    public string GetPathabilityDebugInfoAt(Tile2i tileCoord)
    {
      Option<ClearancePathabilityProvider.DataChunk> option = this.m_data.Get<uint, ClearancePathabilityProvider.DataChunk>(ClearancePathabilityProvider.getChunkKey(tileCoord));
      if (option.IsNone)
        return string.Format("No data at {0}", (object) tileCoord);
      RelTile2i relTile2i = tileCoord - (Tile2i) option.Value.OriginTile.TileCoordSlim;
      Assert.That<int>(relTile2i.X).IsWithinExcl(0, 8);
      Assert.That<int>(relTile2i.Y).IsWithinExcl(0, 8);
      return ClearancePathabilityProvider.explainBits(option.Value.GetRawDataAt(relTile2i.X, relTile2i.Y));
    }

    private static string explainBits(ulong data)
    {
      Assert.That<bool>(((long) data & -32L) == 0L).IsTrue();
      string str1 = ((long) data & 16L) == 16L ? "blocked" : "free";
      string str2 = ((long) data & 3L) == 3L ? "steep" : (((long) data & 1L) == 1L ? "slight" : "none");
      string str3 = ((long) data & 12L) == 12L ? "blocked" : (((long) data & 4L) == 4L ? "low" : "any");
      return "Raw: 0b" + data.AsBin(5) + ", tile: " + str1 + ", slope: " + str2 + ", clearance: " + str3;
    }

    internal void RecomputeAllData()
    {
      foreach (ClearancePathabilityProvider.DataChunk dataChunk in this.m_data.Values)
        dataChunk.RecomputeAllData();
    }

    internal void RecomputeAllDataAt(Chunk2i terrainChunk)
    {
      for (int y = 0; y < 64; y += 8)
      {
        for (int x = 0; x < 64; x += 8)
        {
          ClearancePathabilityProvider.DataChunk chunk;
          if (this.TryGetOrCreateChunkAt(terrainChunk.Tile2i + new RelTile2i(x, y), out chunk))
            chunk.RecomputeAllData();
        }
      }
    }

    internal void ClearAllData()
    {
      this.m_data.Clear();
      Action allChunksCleared = this.AllChunksCleared;
      if (allChunksCleared == null)
        return;
      allChunksCleared();
    }

    private int getContiguousChunkId(Tile2iSlim tile)
    {
      return ((int) tile.X >> 3) + ((int) tile.Y >> 3) * this.m_chunksCountX;
    }

    private int getContiguousChunkId(Tile2i tile)
    {
      return (tile.X >> 3) + (tile.Y >> 3) * this.m_chunksCountX;
    }

    private void roadConnectionAdded(GraphTerrainConnection conn)
    {
      int contiguousChunkId1 = this.getContiguousChunkId(conn.TerrainTile);
      addConn(contiguousChunkId1);
      int key = 0;
      Tile2iSlim terrainTile;
      if (conn.TerrainTile.X > (ushort) 8)
      {
        terrainTile = conn.TerrainTile;
        key = this.getContiguousChunkId(terrainTile.AddX(-5));
        if (key != contiguousChunkId1)
          addConn(key);
      }
      if (conn.TerrainTile.Y <= (ushort) 8)
        return;
      terrainTile = conn.TerrainTile;
      int contiguousChunkId2 = this.getContiguousChunkId(terrainTile.AddY(-5));
      if (contiguousChunkId2 != contiguousChunkId1)
        addConn(contiguousChunkId2);
      if (conn.TerrainTile.X <= (ushort) 8)
        return;
      terrainTile = conn.TerrainTile;
      int contiguousChunkId3 = this.getContiguousChunkId(terrainTile.AddXy(-5));
      if (contiguousChunkId3 == key || contiguousChunkId3 == contiguousChunkId2)
        return;
      addConn(contiguousChunkId3);

      void addConn(int key)
      {
        Lyst<GraphTerrainConnection> lyst;
        if (!this.m_chunkRoadConnections.TryGetValue(key, out lyst))
        {
          lyst = new Lyst<GraphTerrainConnection>();
          this.m_chunkRoadConnections.Add(key, lyst);
        }
        lyst.Add(conn);
        ++this.m_chunkRoadConnectionsCount[key];
      }
    }

    private void roadConnectionRemoved(GraphTerrainConnection conn)
    {
      int contiguousChunkId1 = this.getContiguousChunkId(conn.TerrainTile);
      removeConn(contiguousChunkId1);
      int key = 0;
      Tile2iSlim terrainTile;
      if (conn.TerrainTile.X > (ushort) 8)
      {
        terrainTile = conn.TerrainTile;
        key = this.getContiguousChunkId(terrainTile.AddX(-5));
        if (key != contiguousChunkId1)
          removeConn(key);
      }
      if (conn.TerrainTile.Y <= (ushort) 8)
        return;
      terrainTile = conn.TerrainTile;
      int contiguousChunkId2 = this.getContiguousChunkId(terrainTile.AddY(-5));
      if (contiguousChunkId2 != contiguousChunkId1)
        removeConn(contiguousChunkId2);
      if (conn.TerrainTile.X <= (ushort) 8)
        return;
      terrainTile = conn.TerrainTile;
      int contiguousChunkId3 = this.getContiguousChunkId(terrainTile.AddXy(-5));
      if (contiguousChunkId3 == key || contiguousChunkId3 == contiguousChunkId2)
        return;
      removeConn(contiguousChunkId3);

      void removeConn(int key)
      {
        Lyst<GraphTerrainConnection> lyst;
        if (!this.m_chunkRoadConnections.TryGetValue(key, out lyst))
        {
          Log.Error("Trying to remove non-existent road entrance entry.");
        }
        else
        {
          lyst.Remove(conn);
          --this.m_chunkRoadConnectionsCount[key];
        }
      }
    }

    public bool AreRoadNetworkConnectionsNearby(PfNode node)
    {
      return this.m_chunkRoadConnectionsCount[this.getContiguousChunkId(node.Area.Origin)] > (byte) 0;
    }

    public void GetRoadNetworkConnectionsInArea(
      PfNode node,
      VehiclePathFindingParams pfParams,
      bool fromTerrainToRoad,
      Lyst<Tile2iSlim> connTiles)
    {
      int contiguousChunkId = this.getContiguousChunkId(node.Area.Origin);
      Lyst<GraphTerrainConnection> lyst;
      if (this.m_chunkRoadConnectionsCount[contiguousChunkId] == (byte) 0 || !this.m_chunkRoadConnections.TryGetValue(contiguousChunkId, out lyst))
        return;
      foreach (GraphTerrainConnection terrainConnection in lyst)
      {
        if (terrainConnection.IsFromTerrainToRoad == fromTerrainToRoad)
        {
          Tile2i cornerTileSpace = pfParams.ConvertToCornerTileSpace((Tile2i) terrainConnection.TerrainTile);
          if (node.Area.ContainsTile(cornerTileSpace))
            connTiles.Add(terrainConnection.TerrainTile);
        }
      }
    }

    public static void Serialize(ClearancePathabilityProvider value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ClearancePathabilityProvider>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ClearancePathabilityProvider.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteArray<Chunk8Index>(this.m_initializedChunks);
      TerrainOccupancyManager.Serialize(this.m_occupancyManager, writer);
      writer.WriteArray<ulong>(this.m_pathabilityCapabilities);
      TerrainManager.Serialize(this.TerrainManager, writer);
      writer.WriteGeneric<IVehicleSurfaceProvider>(this.m_vehicleSurfaceProvider);
      writer.WriteInt(this.RecomputedChunksCount);
    }

    public static ClearancePathabilityProvider Deserialize(BlobReader reader)
    {
      ClearancePathabilityProvider pathabilityProvider;
      if (reader.TryStartClassDeserialization<ClearancePathabilityProvider>(out pathabilityProvider))
        reader.EnqueueDataDeserialization((object) pathabilityProvider, ClearancePathabilityProvider.s_deserializeDataDelayedAction);
      return pathabilityProvider;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<ClearancePathabilityProvider>(this, "m_chunkRoadConnections", (object) new Dict<int, Lyst<GraphTerrainConnection>>());
      reader.SetField<ClearancePathabilityProvider>(this, "m_data", (object) new Dict<uint, ClearancePathabilityProvider.DataChunk>());
      this.m_initializedChunks = reader.ReadArray<Chunk8Index>();
      reader.SetField<ClearancePathabilityProvider>(this, "m_occupancyManager", (object) TerrainOccupancyManager.Deserialize(reader));
      reader.SetField<ClearancePathabilityProvider>(this, "m_pathabilityCapabilities", (object) reader.ReadArray<ulong>());
      reader.SetField<ClearancePathabilityProvider>(this, "TerrainManager", (object) TerrainManager.Deserialize(reader));
      reader.SetField<ClearancePathabilityProvider>(this, "m_vehicleSurfaceProvider", (object) reader.ReadGenericAs<IVehicleSurfaceProvider>());
      this.RecomputedChunksCount = reader.ReadInt();
      reader.RegisterInitAfterLoad<ClearancePathabilityProvider>(this, "initSelf", InitPriority.High);
      reader.RegisterInitAfterLoad<ClearancePathabilityProvider>(this, "initSelfLow", InitPriority.Low);
    }

    static ClearancePathabilityProvider()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ClearancePathabilityProvider.FLAT_STEEPNESS_DELTA = 2 * EntityLayoutParser.VEHICLE_SURFACE_EXTRA_THICKNESS;
      ClearancePathabilityProvider.MAX_STEEPNESS_DELTA = 0.5.TilesThick();
      ClearancePathabilityProvider.BASIC_HEIGHT_CLEARANCE = new ThicknessTilesI(2);
      ClearancePathabilityProvider.MAX_HEIGHT_CLEARANCE = new ThicknessTilesI(5);
      ClearancePathabilityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ClearancePathabilityProvider) obj).SerializeData(writer));
      ClearancePathabilityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ClearancePathabilityProvider) obj).DeserializeData(reader));
    }

    /// <summary>
    /// Holds pathability data and reference to neighbors for fast updating.
    /// </summary>
    public class DataChunk
    {
      public readonly TerrainTile OriginTile;
      public readonly ClearancePathabilityProvider Parent;
      public readonly ulong[] Data;
      private readonly Option<ClearancePathabilityProvider.CapabilityChunkData>[] m_pfData;
      private ulong m_pathabilityDirty;
      private ulong m_steepnessDirty;
      private ulong m_heightClearanceDirty;
      public Option<ClearancePathabilityProvider.DataChunk> PlusXNeighbor;
      public Option<ClearancePathabilityProvider.DataChunk> PlusXyNeighbor;
      public Option<ClearancePathabilityProvider.DataChunk> PlusYNeighbor;
      public Option<ClearancePathabilityProvider.DataChunk> MinusXNeighbor;
      public Option<ClearancePathabilityProvider.DataChunk> MinusXyNeighbor;
      public Option<ClearancePathabilityProvider.DataChunk> MinusYNeighbor;

      public bool IsDirty
      {
        get
        {
          return (this.m_pathabilityDirty | this.m_steepnessDirty | this.m_heightClearanceDirty) > 0UL;
        }
      }

      public bool IsDirtyPathability => this.m_pathabilityDirty > 0UL;

      public bool IsDirtySteepness => this.m_steepnessDirty > 0UL;

      public bool IsDirtyHeightClearance => this.m_heightClearanceDirty > 0UL;

      /// <summary>
      /// Whether this chunk has all neighboring chunks. Currently only +-X, ++XY, and +-Y chunks are tracked.
      /// </summary>
      public bool AllNeighborsEnsured { get; private set; }

      public DataChunk(ClearancePathabilityProvider parent, TerrainTile originTile)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Data = new ulong[12];
        // ISSUE: explicit constructor call
        base.\u002Ector();
        Assert.That<Tile2iSlim>(originTile.TileCoordSlim & -8).IsEqualTo<Tile2iSlim>(originTile.TileCoordSlim, "Origin tile is not at origin");
        this.OriginTile = originTile;
        this.Parent = parent;
        this.m_pfData = new Option<ClearancePathabilityProvider.CapabilityChunkData>[parent.m_pathabilityCapabilities.Length];
      }

      public void ExtendPfData(int newLength)
      {
        Assert.That<int>(newLength).IsGreater(this.m_pfData.Length);
        Option<ClearancePathabilityProvider.CapabilityChunkData>[] destinationArray = new Option<ClearancePathabilityProvider.CapabilityChunkData>[newLength];
        Array.Copy((Array) this.m_pfData, 0, (Array) destinationArray, 0, this.m_pfData.Length);
        ReflectionUtils.SetField<ClearancePathabilityProvider.DataChunk>(this, "m_pfData", (object) destinationArray);
      }

      public override string ToString()
      {
        return string.Format("Data chunk {0} (dirty: {1})", (object) this.OriginTile.TileCoordSlim, (object) this.IsDirty);
      }

      internal void RecomputeCoreData()
      {
        this.markPfDataDirty();
        TerrainTile terrainTile = this.OriginTile;
        int index = 0;
        while (index < 8)
        {
          TerrainTile tile = terrainTile;
          ulong num1 = 0;
          int num2 = 0;
          while (num2 < 8)
          {
            ulong tileData = this.computeTileData(tile);
            num1 |= tileData << num2 * 5;
            ++num2;
            tile = tile.PlusXNeighbor;
          }
          this.Data[index] = num1;
          ++index;
          terrainTile = terrainTile.PlusYNeighbor;
        }
        this.m_pathabilityDirty = 0UL;
        this.m_steepnessDirty = 0UL;
        this.m_heightClearanceDirty = 0UL;
        this.Parent.notifyChunkUpdated(this);
      }

      internal void RecomputeAllData()
      {
        this.markPfDataDirty();
        TerrainTile terrainTile = this.OriginTile;
        int index = 0;
        while (index < 12)
        {
          TerrainTile tile = terrainTile;
          ulong num1 = 0;
          int num2 = 0;
          while (num2 < 12)
          {
            ulong tileData = this.computeTileData(tile);
            num1 |= tileData << num2 * 5;
            ++num2;
            tile = tile.PlusXNeighbor;
          }
          this.Data[index] = num1;
          ++index;
          terrainTile = terrainTile.PlusYNeighbor;
        }
        this.m_pathabilityDirty = 0UL;
        this.m_steepnessDirty = 0UL;
        this.m_heightClearanceDirty = 0UL;
        this.Parent.notifyChunkUpdated(this);
      }

      internal void RecomputeDataAt(TerrainTile tile)
      {
        this.updatePathability(tile);
        this.updateHeightClearance((Tile2i) tile.TileCoordSlim);
        this.updateSteepness(tile);
      }

      private ulong computeTileData(TerrainTile tile)
      {
        ulong tileData = this.Parent.getEncodedHeightClearance((Tile2i) tile.TileCoordSlim) | this.Parent.getEncodedSteepness(tile);
        if (tile.TerrainManager.IsBlockingVehicles(tile.DataIndex))
          tileData |= 16UL;
        return tileData;
      }

      public bool IsTilePathable(Tile2i coord, ulong pathabilityMask)
      {
        return this.IsTilePathable(this.getLocalCoord(coord), pathabilityMask);
      }

      public bool IsTilePathable(RelTile2i localCoord, ulong pathabilityMask)
      {
        this.EnsureNotDirty();
        return ((long) this.Data[localCoord.Y] & (long) pathabilityMask << localCoord.X * 5) == 0L;
      }

      public bool IsPathable(Tile2i coord, RelTile1i clearance, ulong pathabilityMask)
      {
        return this.IsPathable(this.getLocalCoord(coord), clearance, pathabilityMask);
      }

      public bool IsPathable(RelTile2i localCoord, RelTile1i clearance, ulong pathabilityMask)
      {
        this.EnsureAllNeighbors();
        this.EnsureNotDirty();
        pathabilityMask <<= localCoord.X * 5;
        if (localCoord.X + clearance.Value > 8)
          this.PlusXNeighbor.Value.EnsureNotDirty();
        if (((long) this.Data[localCoord.Y] & (long) pathabilityMask) != 0L)
          return false;
        if (clearance == RelTile1i.One)
          return true;
        if (localCoord.Y + clearance.Value > 8)
        {
          this.PlusYNeighbor.Value.EnsureNotDirty();
          if (localCoord.X + clearance.Value > 8)
            this.PlusXyNeighbor.Value.EnsureNotDirty();
        }
        for (int index = 1; index < clearance.Value; ++index)
        {
          if (((long) this.Data[localCoord.Y + index] & (long) pathabilityMask) != 0L)
            return false;
        }
        return true;
      }

      public void MarkPathabilityDirty(Tile2i tileCoord)
      {
        RelTile2i localCoord = this.getLocalCoord(tileCoord);
        if (this.m_pathabilityDirty == 0UL)
          this.markPfDataDirty();
        this.m_pathabilityDirty |= (ulong) (1L << (localCoord.Y << 3 | localCoord.X));
        this.markPfDataDirtyOnOverlapNeighbors(localCoord);
      }

      public void MarkPathabilityDirtyAll_NoOverlapCheck()
      {
        this.markPfDataDirty();
        this.m_pathabilityDirty = ulong.MaxValue;
      }

      public void MarkSteepnessDirty(Tile2i tileCoord)
      {
        RelTile2i localCoord = this.getLocalCoord(tileCoord);
        if (this.m_steepnessDirty == 0UL)
          this.markPfDataDirty();
        this.m_steepnessDirty |= (ulong) (1L << (localCoord.Y << 3 | localCoord.X));
        this.markPfDataDirtyOnOverlapNeighbors(localCoord);
      }

      public void MarkHeightClearanceDirty(Tile2i tileCoord)
      {
        RelTile2i localCoord = this.getLocalCoord(tileCoord);
        if (this.m_heightClearanceDirty == 0UL)
          this.markPfDataDirty();
        this.m_heightClearanceDirty |= (ulong) (1L << (localCoord.Y << 3 | localCoord.X));
        this.markPfDataDirtyOnOverlapNeighbors(localCoord);
      }

      private void markPfDataDirtyOnOverlapNeighbors(RelTile2i localCoord)
      {
        if (localCoord.X < 4)
        {
          this.MinusXNeighbor.ValueOrNull?.markPfDataDirty();
          if (localCoord.Y < 4)
            this.MinusXyNeighbor.ValueOrNull?.markPfDataDirty();
        }
        if (localCoord.Y >= 4)
          return;
        this.MinusYNeighbor.ValueOrNull?.markPfDataDirty();
      }

      /// <summary>
      /// Marks all PF data chunks on this chunk as dirty. This may remove them.
      /// </summary>
      private void markPfDataDirty()
      {
        for (int capabilityIndex = 0; capabilityIndex < this.m_pfData.Length; ++capabilityIndex)
        {
          Option<ClearancePathabilityProvider.CapabilityChunkData> option = this.m_pfData[capabilityIndex];
          if (!option.IsNone && !option.Value.IsDirty)
          {
            if (option.Value.HasVisitedNodes())
              option.Value.MarkDirty();
            else
              this.DestroyPfDataAt(capabilityIndex);
          }
        }
        Action<ClearancePathabilityProvider.DataChunk> onChunkUpdated = this.Parent.OnChunkUpdated;
        if (onChunkUpdated == null)
          return;
        onChunkUpdated(this);
      }

      public void DestroyPfDataAt(int capabilityIndex)
      {
        Assert.That<int>(capabilityIndex).IsValidIndexFor<Option<ClearancePathabilityProvider.CapabilityChunkData>>(this.m_pfData);
        Option<ClearancePathabilityProvider.CapabilityChunkData> option = this.m_pfData[capabilityIndex];
        if (!(option != (ClearancePathabilityProvider.CapabilityChunkData) null))
          return;
        option.Value.DisconnectAndDestroyAllNodes();
        this.m_pfData[capabilityIndex] = Option<ClearancePathabilityProvider.CapabilityChunkData>.None;
      }

      private RelTile2i getLocalCoord(Tile2i tileCoord)
      {
        return tileCoord - this.OriginTile.TileCoord & 7;
      }

      public void EnsureNotDirty()
      {
        if (!this.IsDirty)
          return;
        this.RecomputeDirty();
      }

      public void RecomputeDirty()
      {
        if (this.m_pathabilityDirty != 0UL)
          this.recomputeDirtyPathabilityClearance();
        if (this.m_steepnessDirty != 0UL)
          this.recomputeDirtySteepnessClearance();
        if (this.m_heightClearanceDirty != 0UL)
          this.recomputeDirtyHeightClearance();
        this.Parent.notifyChunkUpdated(this);
        Assert.That<bool>(this.IsDirty).IsFalse("Data still dirty after re-computation.");
      }

      private void recomputeDirtyPathabilityClearance()
      {
        Assert.That<ulong>(this.m_pathabilityDirty).IsNotEqualTo<ulong>(0UL);
        for (int y = 0; y < 8; ++y)
        {
          uint num = (uint) (this.m_pathabilityDirty >> (y << 3) & (ulong) byte.MaxValue);
          if (num != 0U)
          {
            for (int x = 0; x < 8; ++x)
            {
              if (((int) num & 1 << x) != 0)
                this.updatePathability(this.OriginTile[new RelTile2i(x, y)]);
            }
          }
        }
        this.m_pathabilityDirty = 0UL;
      }

      private void recomputeDirtySteepnessClearance()
      {
        Assert.That<ulong>(this.m_steepnessDirty).IsNotEqualTo<ulong>(0UL);
        for (int y = 0; y < 8; ++y)
        {
          uint num = (uint) (this.m_steepnessDirty >> (y << 3) & (ulong) byte.MaxValue);
          if (num != 0U)
          {
            for (int x = 0; x < 8; ++x)
            {
              if (((int) num & 1 << x) != 0)
                this.updateSteepness(this.OriginTile[new RelTile2i(x, y)]);
            }
          }
        }
        this.m_steepnessDirty = 0UL;
      }

      private void recomputeDirtyHeightClearance()
      {
        Assert.That<ulong>(this.m_heightClearanceDirty).IsNotEqualTo<ulong>(0UL);
        for (int y = 0; y < 8; ++y)
        {
          uint num = (uint) (this.m_heightClearanceDirty >> (y << 3) & (ulong) byte.MaxValue);
          if (num != 0U)
          {
            for (int x = 0; x < 8; ++x)
            {
              if (((int) num & 1 << x) != 0)
                this.updateHeightClearance(new RelTile2i(x, y));
            }
          }
        }
        this.m_heightClearanceDirty = 0UL;
      }

      private void updateSteepness(TerrainTile tile)
      {
        RelTile2i localCoord = this.getLocalCoord((Tile2i) tile.TileCoordSlim);
        ulong encodedSteepness = this.Parent.getEncodedSteepness(tile);
        this.updateSteepnessInternal(localCoord, encodedSteepness);
        if (localCoord.X < 4)
        {
          if (this.MinusXNeighbor.HasValue)
            this.MinusXNeighbor.Value.updateSteepnessInternal(localCoord.AddX(8), encodedSteepness);
          if (localCoord.Y < 4 && this.MinusXyNeighbor.HasValue)
            this.MinusXyNeighbor.Value.updateSteepnessInternal(localCoord.AddXy(8), encodedSteepness);
        }
        if (localCoord.Y >= 4 || !this.MinusYNeighbor.HasValue)
          return;
        this.MinusYNeighbor.Value.updateSteepnessInternal(localCoord.AddY(8), encodedSteepness);
      }

      private void updateSteepnessInternal(RelTile2i localCoord, ulong newSteepnessEncoded)
      {
        Assert.That<int>(localCoord.X).IsWithinExcl(0, 12);
        Assert.That<int>(localCoord.Y).IsWithinExcl(0, 12);
        int num = localCoord.X * 5;
        this.Data[localCoord.Y] = (ulong) ((long) this.Data[localCoord.Y] & ~(3L << num) | (long) newSteepnessEncoded << num);
      }

      private void updateHeightClearance(Tile2i tileCoord)
      {
        this.updateHeightClearance(this.getLocalCoord(tileCoord));
      }

      private void updateHeightClearance(RelTile2i localCoord)
      {
        ulong encodedHeightClearance = this.Parent.getEncodedHeightClearance((Tile2i) (this.OriginTile.TileCoordSlim + localCoord));
        this.updateHeightClearanceInternal(localCoord, encodedHeightClearance);
        if (localCoord.X < 4)
        {
          if (this.MinusXNeighbor.HasValue)
            this.MinusXNeighbor.Value.updateHeightClearanceInternal(localCoord.AddX(8), encodedHeightClearance);
          if (localCoord.Y < 4 && this.MinusXyNeighbor.HasValue)
            this.MinusXyNeighbor.Value.updateHeightClearanceInternal(localCoord.AddXy(8), encodedHeightClearance);
        }
        if (localCoord.Y >= 4 || !this.MinusYNeighbor.HasValue)
          return;
        this.MinusYNeighbor.Value.updateHeightClearanceInternal(localCoord.AddY(8), encodedHeightClearance);
      }

      private void updateHeightClearanceInternal(RelTile2i localCoord, ulong newClearanceEncoded)
      {
        Assert.That<int>(localCoord.X).IsWithinExcl(0, 12);
        Assert.That<int>(localCoord.Y).IsWithinExcl(0, 12);
        int num = localCoord.X * 5;
        this.Data[localCoord.Y] = (ulong) ((long) this.Data[localCoord.Y] & ~(12L << num) | (long) newClearanceEncoded << num);
      }

      private void updatePathability(TerrainTile tile)
      {
        RelTile2i localCoord = this.getLocalCoord((Tile2i) tile.TileCoordSlim);
        ulong newPathabilityEncoded = tile.TerrainManager.IsBlockingVehicles(tile.DataIndex) ? 16UL : 0UL;
        this.updatePathabilityInternal(localCoord, newPathabilityEncoded);
        if (localCoord.X < 4)
        {
          if (this.MinusXNeighbor.HasValue)
            this.MinusXNeighbor.Value.updatePathabilityInternal(localCoord.AddX(8), newPathabilityEncoded);
          if (localCoord.Y < 4 && this.MinusXyNeighbor.HasValue)
            this.MinusXyNeighbor.Value.updatePathabilityInternal(localCoord.AddXy(8), newPathabilityEncoded);
        }
        if (localCoord.Y >= 4 || !this.MinusYNeighbor.HasValue)
          return;
        this.MinusYNeighbor.Value.updatePathabilityInternal(localCoord.AddY(8), newPathabilityEncoded);
      }

      private void updatePathabilityInternal(RelTile2i localCoord, ulong newPathabilityEncoded)
      {
        Assert.That<int>(localCoord.X).IsWithinExcl(0, 12);
        Assert.That<int>(localCoord.Y).IsWithinExcl(0, 12);
        int num = localCoord.X * 5;
        this.Data[localCoord.Y] = (ulong) ((long) this.Data[localCoord.Y] & ~(16L << num) | (long) newPathabilityEncoded << num);
      }

      public ulong GetRawDataAt(int x, int y)
      {
        Assert.That<int>(x).IsWithinExcl(0, 12);
        Assert.That<int>(y).IsWithinExcl(0, 12);
        return this.Data[y] >> x * 5 & 31UL;
      }

      /// <summary>Returns decoded pathability info at relative coord.</summary>
      public bool GetDecodedDataAt(
        int x,
        int y,
        out bool tileBlocked,
        out bool clearanceBlocked,
        out bool heightClearanceLimited,
        out bool slopeBlocked,
        out bool slopeLimited)
      {
        Assert.That<int>(x).IsWithinExcl(0, 8);
        Assert.That<int>(y).IsWithinExcl(0, 8);
        return ClearancePathabilityProvider.DataChunk.DecodeData(this.GetRawDataAt(x, y), out tileBlocked, out clearanceBlocked, out heightClearanceLimited, out slopeBlocked, out slopeLimited);
      }

      public static bool DecodeData(
        ulong rawData,
        out bool tileBlocked,
        out bool clearanceBlocked,
        out bool heightClearanceLimited,
        out bool slopeBlocked,
        out bool slopeLimited)
      {
        Assert.That<ulong>(rawData & 18446744073709551584UL).IsEqualTo<ulong>(0UL, "Invalid raw data.");
        if (rawData == 0UL)
        {
          tileBlocked = false;
          clearanceBlocked = false;
          heightClearanceLimited = false;
          slopeBlocked = false;
          slopeLimited = false;
          return true;
        }
        tileBlocked = ((long) rawData & 16L) == 16L;
        clearanceBlocked = ((long) rawData & 12L) == 12L;
        heightClearanceLimited = ((long) rawData & 4L) == 4L;
        slopeBlocked = ((long) rawData & 3L) == 3L;
        slopeLimited = ((long) rawData & 1L) == 1L;
        return false;
      }

      public string RawDataToDebugStr(int x, int y)
      {
        return ClearancePathabilityProvider.DataChunk.RawDataToDebugStr(this.GetRawDataAt(x, y));
      }

      public static string RawDataToDebugStr(ulong rawData)
      {
        bool tileBlocked;
        bool clearanceBlocked;
        bool heightClearanceLimited;
        bool slopeBlocked;
        bool slopeLimited;
        return !ClearancePathabilityProvider.DataChunk.DecodeData(rawData, out tileBlocked, out clearanceBlocked, out heightClearanceLimited, out slopeBlocked, out slopeLimited) ? (tileBlocked ? "T" : ".") + (clearanceBlocked ? "C" : (heightClearanceLimited ? "v" : ".")) + (slopeBlocked ? "S" : (slopeLimited ? "d" : ".")) : "...";
      }

      public string ToDebugString()
      {
        StringBuilder stringBuilder = new StringBuilder(314);
        for (int y = 7; y >= 0; --y)
        {
          stringBuilder.Append(y);
          stringBuilder.Append('|');
          for (int x = 0; x < 8; ++x)
          {
            stringBuilder.Append(ClearancePathabilityProvider.DataChunk.RawDataToDebugStr(this.GetRawDataAt(x, y)));
            stringBuilder.Append('|');
          }
          stringBuilder.Append('\n');
        }
        stringBuilder.Append("  ");
        for (int index = 0; index < 8; ++index)
        {
          stringBuilder.Append(' ');
          stringBuilder.Append(index);
          stringBuilder.Append("  ");
        }
        Assert.That<int>(stringBuilder.Length).IsEqualTo(314);
        return stringBuilder.ToString();
      }

      public void ValidateCoreData(Lyst<KeyValuePair<Tile2iSlim, string>> errors)
      {
        if (this.IsDirty)
          this.RecomputeDirty();
        TerrainTile terrainTile = this.OriginTile;
        int index = 0;
        while (index < 8)
        {
          TerrainTile tile = terrainTile;
          int num = 0;
          while (num < 8)
          {
            ulong tileData = this.computeTileData(tile);
            ulong data = this.Data[index] >> num * 5 & 31UL;
            if ((long) tileData != (long) data)
              errors.Add(Make.Kvp<Tile2iSlim, string>(tile.TileCoordSlim, string.Format("Data mismatch at core tile {0}: ", (object) tile.TileCoordSlim) + "Expected: " + ClearancePathabilityProvider.explainBits(tileData) + ", Actual: " + ClearancePathabilityProvider.explainBits(data)));
            ++num;
            tile = tile.PlusXNeighbor;
          }
          ++index;
          terrainTile = terrainTile.PlusYNeighbor;
        }
      }

      public void ValidatePfData(Lyst<KeyValuePair<Tile2iSlim, string>> errors)
      {
        if (this.IsDirty)
          return;
        for (int capabilityIndex = 0; capabilityIndex < this.m_pfData.Length; ++capabilityIndex)
        {
          Option<ClearancePathabilityProvider.CapabilityChunkData> option = this.m_pfData[capabilityIndex];
          if (!option.IsNone && !option.Value.IsDirty && !option.Value.HasVisitedNodes())
          {
            ClearancePathabilityProvider.CapabilityChunkData capabilityChunkData = option.Value;
            Lyst<PfNode> lyst1 = capabilityChunkData.Nodes.ToLyst<PfNode>();
            capabilityChunkData.RecomputePfData();
            PathabilityBitmap pathabilityBitmap;
            if (lyst1.Count != capabilityChunkData.Nodes.Count)
            {
              Lyst<KeyValuePair<Tile2iSlim, string>> lyst2 = errors;
              Tile2iSlim tileCoordSlim = capabilityChunkData.Parent.OriginTile.TileCoordSlim;
              string[] strArray = new string[10]
              {
                string.Format("PF nodes mismatch at capability index {0} of chunk at {1}. ", (object) capabilityIndex, (object) capabilityChunkData.Parent.OriginTile),
                string.Format("Old count: {0}, new count: {1}\n", (object) lyst1.Count, (object) capabilityChunkData.Nodes.Count),
                "Old nodes:\n",
                capabilityChunkData.GetNodesDebugString(lyst1),
                "\nNew nodes:\n",
                capabilityChunkData.GetNodesDebugString(),
                "\nOccupancy (incl clearance):\n",
                null,
                null,
                null
              };
              pathabilityBitmap = capabilityChunkData.Parent.ComputePathabilityBitmap(capabilityIndex);
              strArray[7] = pathabilityBitmap.ToDebugString();
              strArray[8] = "\nExplained (no clearance):\n";
              strArray[9] = capabilityChunkData.Parent.ToDebugString();
              string str = string.Concat(strArray);
              KeyValuePair<Tile2iSlim, string> keyValuePair = Make.Kvp<Tile2iSlim, string>(tileCoordSlim, str);
              lyst2.Add(keyValuePair);
            }
            else
            {
              for (int index = 0; index < lyst1.Count; ++index)
              {
                if (lyst1[index].Area != capabilityChunkData.Nodes[index].Area)
                {
                  Lyst<KeyValuePair<Tile2iSlim, string>> lyst3 = errors;
                  Tile2iSlim tileCoordSlim = capabilityChunkData.Parent.OriginTile.TileCoordSlim;
                  string[] strArray = new string[10]
                  {
                    string.Format("PF Area mismatch at capability index {0} of chunk at {1}. ", (object) capabilityIndex, (object) capabilityChunkData.Parent.OriginTile),
                    string.Format("Old node at {0}: {1}, new node at {2}: {3}\n", (object) index, (object) lyst1[index].Area, (object) index, (object) capabilityChunkData.Nodes[index]),
                    "Old nodes:\n",
                    capabilityChunkData.GetNodesDebugString(lyst1),
                    "\nNew nodes:\n",
                    capabilityChunkData.GetNodesDebugString(),
                    "\nOccupancy (incl clearance):\n",
                    null,
                    null,
                    null
                  };
                  pathabilityBitmap = capabilityChunkData.Parent.ComputePathabilityBitmap(capabilityIndex);
                  strArray[7] = pathabilityBitmap.ToDebugString();
                  strArray[8] = "\nExplained (no clearance):\n";
                  strArray[9] = capabilityChunkData.Parent.ToDebugString();
                  string str = string.Concat(strArray);
                  KeyValuePair<Tile2iSlim, string> keyValuePair = Make.Kvp<Tile2iSlim, string>(tileCoordSlim, str);
                  lyst3.Add(keyValuePair);
                  break;
                }
              }
            }
          }
        }
      }

      /// <summary>
      /// Returns current PF data for the given capability index.
      /// </summary>
      public Option<ClearancePathabilityProvider.CapabilityChunkData> GetPfData(int capabilityIndex)
      {
        Assert.That<int>(capabilityIndex).IsValidIndexFor<Option<ClearancePathabilityProvider.CapabilityChunkData>>(this.m_pfData);
        return this.m_pfData[capabilityIndex];
      }

      /// <summary>
      /// Gets or creates PF data for the given capability index. This also ensures that neighbors are created.
      /// </summary>
      public ClearancePathabilityProvider.CapabilityChunkData GetOrCreatePfData(int capabilityIndex)
      {
        Assert.That<int>(capabilityIndex).IsValidIndexFor<Option<ClearancePathabilityProvider.CapabilityChunkData>>(this.m_pfData);
        this.EnsureAllNeighbors();
        Option<ClearancePathabilityProvider.CapabilityChunkData> option = this.m_pfData[capabilityIndex];
        if (option.HasValue)
          return option.Value;
        ClearancePathabilityProvider.CapabilityChunkData pfData = new ClearancePathabilityProvider.CapabilityChunkData(this, capabilityIndex);
        this.m_pfData[capabilityIndex] = (Option<ClearancePathabilityProvider.CapabilityChunkData>) pfData;
        return pfData;
      }

      /// <summary>
      /// Ensures that this chunk has data chunks covering its overlaps.
      /// </summary>
      internal void EnsureAllNeighbors()
      {
        if (this.AllNeighborsEnsured)
          return;
        Tile2i tileCoordSlim = (Tile2i) this.OriginTile.TileCoordSlim;
        ClearancePathabilityProvider.DataChunk chunk;
        this.Parent.TryGetOrCreateChunkAt(tileCoordSlim.AddX(8), out chunk);
        this.Parent.TryGetOrCreateChunkAt(tileCoordSlim.AddXy(8), out chunk);
        this.Parent.TryGetOrCreateChunkAt(tileCoordSlim.AddY(8), out chunk);
        this.Parent.TryGetOrCreateChunkAt(tileCoordSlim.AddX(-8), out chunk);
        this.Parent.TryGetOrCreateChunkAt(tileCoordSlim.AddXy(-8), out chunk);
        this.Parent.TryGetOrCreateChunkAt(tileCoordSlim.AddY(-8), out chunk);
        this.AllNeighborsEnsured = true;
      }

      public PathabilityBitmap ComputePathabilityBitmap(int capabilityIndex)
      {
        this.EnsureAllNeighbors();
        this.EnsureNotDirty();
        this.PlusXNeighbor.ValueOrNull?.EnsureNotDirty();
        this.PlusYNeighbor.ValueOrNull?.EnsureNotDirty();
        this.PlusXyNeighbor.ValueOrNull?.EnsureNotDirty();
        return this.computePathabilityBitmap(capabilityIndex);
      }

      public PathabilityBitmap ComputePathabilityBitmapNoRecomputeDirty(int capabilityIndex)
      {
        return this.computePathabilityBitmap(capabilityIndex);
      }

      private PathabilityBitmap computePathabilityBitmap(int capabilityIndex)
      {
        PathabilityBitmap pathabilityBitmap = new PathabilityBitmap(ulong.MaxValue);
        ulong pathabilityCapability = this.Parent.m_pathabilityCapabilities[capabilityIndex];
        RelTile1i clearanceFromMask = ClearancePathabilityProvider.ExtractClearanceFromMask(ref pathabilityCapability);
        for (int y = 0; y < 8; ++y)
        {
          ulong num = pathabilityCapability;
          int x = 0;
          while (x < 8)
          {
            for (int index = 0; index < clearanceFromMask.Value; ++index)
            {
              if (((long) this.Data[y + index] & (long) num) != 0L)
              {
                pathabilityBitmap = pathabilityBitmap.SetNotPathableAt(x, y);
                break;
              }
            }
            ++x;
            num <<= 5;
          }
        }
        return pathabilityBitmap;
      }

      public void ConnectToNeighborsAfterLoad(ClearancePathabilityProvider parent, TerrainTile tile)
      {
        ReflectionUtils.SetField<ClearancePathabilityProvider.DataChunk>(this, "Parent", (object) parent);
        ReflectionUtils.SetField<ClearancePathabilityProvider.DataChunk>(this, "OriginTile", (object) tile);
        int num = 0;
        Dict<uint, ClearancePathabilityProvider.DataChunk> data = parent.m_data;
        if (this.MinusXNeighbor.IsNone)
        {
          ClearancePathabilityProvider.DataChunk dataChunk;
          if (data.TryGetValue(ClearancePathabilityProvider.getChunkKey(this.OriginTile.TileCoord.AddX(-8)), out dataChunk))
          {
            this.MinusXNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
            dataChunk.PlusXNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) this;
            ++num;
          }
        }
        else
          ++num;
        if (this.MinusYNeighbor.IsNone)
        {
          ClearancePathabilityProvider.DataChunk dataChunk;
          if (data.TryGetValue(ClearancePathabilityProvider.getChunkKey(this.OriginTile.TileCoord.AddY(-8)), out dataChunk))
          {
            this.MinusYNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
            dataChunk.PlusYNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) this;
            ++num;
          }
        }
        else
          ++num;
        if (this.MinusXyNeighbor.IsNone)
        {
          ClearancePathabilityProvider.DataChunk dataChunk;
          if (data.TryGetValue(ClearancePathabilityProvider.getChunkKey(this.OriginTile.TileCoord.AddXy(-8)), out dataChunk))
          {
            this.MinusXyNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
            dataChunk.PlusXyNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) this;
            ++num;
          }
        }
        else
          ++num;
        if (this.PlusXNeighbor.IsNone)
        {
          ClearancePathabilityProvider.DataChunk dataChunk;
          if (data.TryGetValue(ClearancePathabilityProvider.getChunkKey(this.OriginTile.TileCoord.AddX(8)), out dataChunk))
          {
            this.PlusXNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
            dataChunk.MinusXNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) this;
            ++num;
          }
        }
        else
          ++num;
        if (this.PlusYNeighbor.IsNone)
        {
          ClearancePathabilityProvider.DataChunk dataChunk;
          if (data.TryGetValue(ClearancePathabilityProvider.getChunkKey(this.OriginTile.TileCoord.AddY(8)), out dataChunk))
          {
            this.PlusYNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
            dataChunk.MinusYNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) this;
            ++num;
          }
        }
        else
          ++num;
        if (this.PlusXyNeighbor.IsNone)
        {
          ClearancePathabilityProvider.DataChunk dataChunk;
          if (data.TryGetValue(ClearancePathabilityProvider.getChunkKey(this.OriginTile.TileCoord.AddXy(8)), out dataChunk))
          {
            this.PlusXyNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) dataChunk;
            dataChunk.MinusXyNeighbor = (Option<ClearancePathabilityProvider.DataChunk>) this;
            ++num;
          }
        }
        else
          ++num;
        Assert.That<int>(num).IsWithinIncl(0, 6);
        this.AllNeighborsEnsured = num == 6;
      }
    }

    /// <summary>Data for a pathability class.</summary>
    public class CapabilityChunkData
    {
      internal readonly ClearancePathabilityProvider.DataChunk Parent;
      public readonly int CapabilityIndex;
      public readonly Lyst<PfNode> Nodes;

      public bool IsDirty { get; private set; }

      public CapabilityChunkData(ClearancePathabilityProvider.DataChunk parent, int capabilityIndex)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Nodes = new Lyst<PfNode>();
        // ISSUE: reference to a compiler-generated field
        this.\u003CIsDirty\u003Ek__BackingField = true;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Parent = parent;
        this.CapabilityIndex = capabilityIndex;
      }

      public Option<PfNode> GetNodeAt(Tile2i coord)
      {
        foreach (PfNode node in this.Nodes)
        {
          if (node.Area.ContainsTile(coord))
            return (Option<PfNode>) node;
        }
        return Option<PfNode>.None;
      }

      public Option<PfNode> GetNodeAndEnsurePfNeighbors(Tile2i coord)
      {
        foreach (PfNode node in this.Nodes)
        {
          if (node.Area.ContainsTile(coord))
          {
            node.EnsureAllPfNeighbors();
            return (Option<PfNode>) node;
          }
        }
        return Option<PfNode>.None;
      }

      public bool HasVisitedNodes()
      {
        foreach (PfNode node in this.Nodes)
        {
          if (node.IsVisited)
            return true;
        }
        return false;
      }

      public void DisconnectAndDestroyAllNodes()
      {
        foreach (PfNode node in this.Nodes)
          node.DisconnectAndDestroy();
        this.Nodes.Clear();
      }

      public void MarkDirty() => this.IsDirty = true;

      internal void EnsureNotDirty()
      {
        if (!this.IsDirty)
          return;
        this.recomputePfData(this.Parent.ComputePathabilityBitmap(this.CapabilityIndex));
      }

      /// <summary>
      /// Recomputes PF data if it is dirty and no nodes are visited.
      /// 
      /// We do not recompute chunks that have nodes visited to not affect ongoing pathfinding.
      /// It is responsibility of the pathfinder to release visited nodes ASAP to not block recomputations (e.g.
      /// right after PF is node, not waiting until next PF request).
      /// </summary>
      internal void EnsureNotDirtyIfNoNodesVisited()
      {
        if (!this.IsDirty)
        {
          Assert.That<bool>(this.Parent.IsDirty).IsFalse();
        }
        else
        {
          if (this.HasVisitedNodes())
            return;
          this.recomputePfData(this.Parent.ComputePathabilityBitmap(this.CapabilityIndex));
        }
      }

      internal void RecomputePfData()
      {
        this.recomputePfData(this.Parent.ComputePathabilityBitmap(this.CapabilityIndex));
      }

      private void recomputePfData(PathabilityBitmap pathabilityBitmap)
      {
        this.DisconnectAndDestroyAllNodes();
        for (int index1 = 0; index1 < 8; ++index1)
        {
          for (int index2 = 0; index2 < 8; ++index2)
          {
            if (pathabilityBitmap.IsPathableAt(index2, index1))
              this.Nodes.Add(this.createNodeAt(index2, index1, ref pathabilityBitmap));
          }
        }
        this.IsDirty = false;
        this.Parent.Parent.notifyChunkUpdated(this.Parent);
        if (this.Nodes.IsEmpty)
          return;
        for (int index3 = 0; index3 < this.Nodes.Count; ++index3)
        {
          for (int index4 = index3 + 1; index4 < this.Nodes.Count; ++index4)
            ClearancePathabilityProvider.CapabilityChunkData.connectNodesIfTouching(this.Nodes[index3], this.Nodes[index4]);
        }
        foreach (PfNode node in this.Nodes)
        {
          Tile2i tile2i1 = node.Area.Origin & 7;
          if (tile2i1.X == 0)
            this.tryConnectToNeighbor(node, this.Parent.MinusXNeighbor);
          if (tile2i1.Y == 0)
            this.tryConnectToNeighbor(node, this.Parent.MinusYNeighbor);
          Tile2i tile2i2 = node.Area.PlusXyCoordExcl & 7;
          if (tile2i2.X == 0)
            this.tryConnectToNeighbor(node, this.Parent.PlusXNeighbor);
          if (tile2i2.Y == 0)
            this.tryConnectToNeighbor(node, this.Parent.PlusYNeighbor);
        }
      }

      /// <summary>
      /// Returns true when neighbor was connected, false when connection was attempted but failed, and null when
      /// no connection was attempted.
      /// </summary>
      private void tryConnectToNeighbor(
        PfNode node,
        Option<ClearancePathabilityProvider.DataChunk> nbrChunk)
      {
        if (nbrChunk.IsNone)
          return;
        Option<ClearancePathabilityProvider.CapabilityChunkData> pfData = nbrChunk.Value.GetPfData(this.CapabilityIndex);
        if (pfData.IsNone || pfData.Value.Nodes.IsEmpty)
          return;
        foreach (PfNode node1 in pfData.Value.Nodes)
          ClearancePathabilityProvider.CapabilityChunkData.connectNodesIfTouching(node, node1);
      }

      private static void connectNodesIfTouching(PfNode node1, PfNode node2)
      {
        Line2i connectionLine;
        Direction90 nbrDirection;
        if (!ClearancePathabilityProvider.CapabilityChunkData.areNodesTouching(node1, node2, out connectionLine, out nbrDirection))
          return;
        node1.ConnectTo(node2, connectionLine, nbrDirection);
      }

      private PfNode createNodeAt(
        int originX,
        int originY,
        ref PathabilityBitmap pathabilityBitmap)
      {
        int x1 = 1;
        int y1 = 1;
        while (true)
        {
          int x2;
          int y2;
          bool flag1;
          do
          {
            x2 = originX + x1;
            bool flag2;
            if (x2 < 8)
            {
              flag2 = true;
              for (int index = 0; index < y1; ++index)
                flag2 &= pathabilityBitmap.IsPathableAt(x2, originY + index);
            }
            else
              flag2 = false;
            y2 = originY + y1;
            if (y2 < 8)
            {
              flag1 = true;
              for (int index = 0; index < x1; ++index)
                flag1 &= pathabilityBitmap.IsPathableAt(originX + index, y2);
            }
            else
              flag1 = false;
            if (flag2)
              ++x1;
            else
              goto label_14;
          }
          while (!flag1 || !pathabilityBitmap.IsPathableAt(x2, y2));
          ++y1;
          continue;
label_14:
          if (flag1)
            ++y1;
          else
            break;
        }
        for (int index1 = 0; index1 < y1; ++index1)
        {
          for (int index2 = 0; index2 < x1; ++index2)
            pathabilityBitmap = pathabilityBitmap.SetNotPathableAt(originX + index2, originY + index1);
        }
        return new PfNode(this, new RectangleTerrainArea2i((Tile2i) (this.Parent.OriginTile.TileCoordSlim + new RelTile2i(originX, originY)), new RelTile2i(x1, y1)));
      }

      private static bool areNodesTouching(
        PfNode node1,
        PfNode node2,
        out Line2i connectionLine,
        out Direction90 nbrDirection)
      {
        int x1 = node1.Area.Origin.X;
        int y1 = node1.Area.Origin.Y;
        int num1 = x1 + node1.Area.Size.X;
        int num2 = y1 + node1.Area.Size.Y;
        int x2 = node2.Area.Origin.X;
        int y2 = node2.Area.Origin.Y;
        int x3 = x2 + node2.Area.Size.X;
        int y3 = y2 + node2.Area.Size.Y;
        Line2i? nullable = new Line2i?();
        nbrDirection = new Direction90();
        if (num1 == x2)
        {
          int from;
          int to;
          if (getOverlap(y1, num2 - 1, y2, y3 - 1, out from, out to))
          {
            nullable = new Line2i?(new Line2i(new Vector2i(x2 - 1, from), new Vector2i(x2 - 1, to)));
            nbrDirection = Direction90.PlusX;
          }
        }
        else if (x3 == x1)
        {
          int from;
          int to;
          if (getOverlap(y1, num2 - 1, y2, y3 - 1, out from, out to))
          {
            nullable = new Line2i?(new Line2i(new Vector2i(x3, from), new Vector2i(x3, to)));
            nbrDirection = Direction90.MinusX;
          }
        }
        else if (num2 == y2)
        {
          int from;
          int to;
          if (getOverlap(x1, num1 - 1, x2, x3 - 1, out from, out to))
          {
            nullable = new Line2i?(new Line2i(new Vector2i(from, y2 - 1), new Vector2i(to, y2 - 1)));
            nbrDirection = Direction90.PlusY;
          }
        }
        else if (y3 == y1)
        {
          int from;
          int to;
          if (getOverlap(x1, num1 - 1, x2, x3 - 1, out from, out to))
          {
            nullable = new Line2i?(new Line2i(new Vector2i(from, y3), new Vector2i(to, y3)));
            nbrDirection = Direction90.MinusY;
          }
        }
        else
        {
          connectionLine = new Line2i();
          return false;
        }
        if (!nullable.HasValue)
        {
          connectionLine = new Line2i();
          return false;
        }
        connectionLine = nullable.Value;
        return true;

        static bool getOverlap(int from1, int to1, int from2, int to2, out int from, out int to)
        {
          Assert.That<int>(from1).IsLessOrEqual(to1);
          Assert.That<int>(from2).IsLessOrEqual(to2);
          if (from1 > to2 || to1 < from2)
          {
            from = 0;
            to = 0;
            return false;
          }
          from = from1.Max(from2);
          to = to1.Min(to2);
          Assert.That<int>(from).IsLessOrEqual(to);
          return true;
        }
      }

      public string GetNodesDebugString(Lyst<PfNode> altNodes = null)
      {
        StringBuilder stringBuilder = new StringBuilder(242);
        for (int index1 = 7; index1 >= 0; --index1)
        {
          stringBuilder.Append(index1);
          stringBuilder.Append('|');
          for (int index2 = 0; index2 < 8; ++index2)
            stringBuilder.Append("  |");
          stringBuilder.Append('\n');
        }
        stringBuilder.Append("  ");
        for (int index = 0; index < 8; ++index)
        {
          stringBuilder.Append(index);
          stringBuilder.Append("  ");
        }
        Lyst<PfNode> lyst = altNodes ?? this.Nodes;
        for (int index3 = 0; index3 < lyst.Count; ++index3)
        {
          foreach (Tile2i enumerateTile in lyst[index3].Area.EnumerateTiles())
          {
            RelTile2i relTile2i = enumerateTile - (Tile2i) this.Parent.OriginTile.TileCoordSlim;
            int index4 = 27 * (8 - relTile2i.Y - 1) + 3 * relTile2i.X + 2;
            stringBuilder[index4] = (index3 / 10).ToString()[0];
            stringBuilder[index4 + 1] = (index3 % 10).ToString()[0];
          }
        }
        Assert.That<int>(stringBuilder.Length).IsEqualTo(242);
        return stringBuilder.ToString();
      }
    }
  }
}
