// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Physics;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Logging;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

#nullable disable
namespace Mafi.Core.Terrain
{
  /// <summary>
  /// Terrain manager controls the implementation details of infinite chunk-based terrain and hides the most of
  /// terrain-tiling mechanics while offering interface that uses global coordinates to interact with the terrain.
  /// NOTE: All write-actions have to be performed on the simulation thread.
  /// </summary>
  /// <remarks>
  /// Terrain is composed of <see cref="T:Mafi.Core.Terrain.TerrainChunk" /> s in 2D grid. Every chunk contains a smaller grid of <see cref="T:Mafi.Core.Terrain.TerrainTile" /> s.
  /// 
  /// All non-connected chunk and tile references point to phantom chunk and tile. There are no null references
  /// pointing to chunk or tile in the terrain.
  /// 
  /// Terrain manager also provides utility methods that work with multiple grids such as interpolated positions,
  /// normals, etc.
  /// 
  /// PERF: For proper way of checking array bounds see do the following (more details in <c>JitTests</c>):
  /// <code>
  /// var arr = m_data.Array;                         // 1) Cache queried array. Even member array must be cached.
  /// if ((unchecked((uint)i) &gt;= (uint)arr.Length) {  // 2) Combined array-length check, do not check null if not needed.
  /// 	Log.Error("Bad index bro");                 // 3) Do not format error message if not absolutely necessary.
  /// 	return default;
  /// }
  /// return arr[i];                      // 4) Index into array with checked index. Wrapped index in a struct is OK.
  /// </code>
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public sealed class TerrainManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public const int TERRAIN_GEN_PROGRESS_STEPS = 50;
    /// <summary>
    /// Terrain dimension must be multiple of this value (256). Terrain dimension is guaranteed to be divisible by
    /// this number without remainder for easy chunking without the need for checking edge cases.
    /// </summary>
    public const int TERRAIN_DIMENSION_MULTIPLE = 256;
    public const int TERRAIN_DIMENSION_MULTIPLE_BITS = 8;
    /// <summary>
    /// Minimum terrain size is naturally 1 dimension multiple.
    /// </summary>
    public const int TERRAIN_DIMENSION_MIN = 256;
    /// <summary>Masks terrain size bits that must be zero.</summary>
    public const int TERRAIN_DIMENSION_MULTIPLE_MASK = 255;
    /// <summary>
    /// Size of the off-limits region around the map. This region makes some checks more efficient since we are able
    /// to detect when the edge of map is near.
    /// Off-limit tiles are flagged with <see cref="F:Mafi.Core.Terrain.TerrainTile.FLAG_IS_OFF_LIMITS" />.
    /// </summary>
    public const int MIN_OFF_LIMITS_SIZE = 32;
    public const int DEFAULT_OFF_LIMITS_SIZE = 64;
    public const int MAX_OFF_LIMITS_SIZE = 256;
    /// <summary>
    /// Maximum value for a terrain dimension is 32768 (2^16).
    /// </summary>
    /// <remarks>
    /// Only 15 bits is used so that a multiplication of two max coords still fits to a signed int32 without overflow
    /// and adding +1 to a max coord won't overflow ushort. Maxing-out 65k would require being extremely careful about
    /// arithmetic operations to not cause overflows which is just not worth the effort.
    /// </remarks>
    public const int TERRAIN_DIMENSION_MAX = 32768;
    public const int TERRAIN_DIMENSION_MAX_BITS = 15;
    /// <summary>
    /// Max area is dictated by Unity restriction of max 2 GB per object on heap. With 2^26 array elements we can have
    /// arrays of structs up to 32 bytes.
    /// </summary>
    /// <remarks>The <c>UnsafeUtility.Malloc</c> can be used to allocate larger arrays if needed.</remarks>
    /// v
    public const int TERRAIN_AREA_MAX = 67108864;
    public const int TERRAIN_AREA_MAX_BITS = 26;
    public static readonly ThicknessTilesF MAX_DISRUPTION_DEPTH;
    /// <summary>
    /// Whenever bedrock layer is returned it will have this thickness. Note that bedrock has unlimited thickness.
    /// This value is used instead of <see cref="P:Mafi.ThicknessTilesF.MaxValue" /> to avoid overflow issues.
    /// This value was also chosen to fit to <see cref="T:Mafi.Core.Terrain.TerrainMaterialThicknessSlim" />.
    /// </summary>
    public static readonly ThicknessTilesF BedrockLayerThicknessDefault;
    [NewInSaveVersion(140, null, "new MapOffLimitsSize(32.Tiles(), 32.Tiles(), 32.Tiles(), 32.Tiles())", null, null)]
    public readonly MapOffLimitsSize OffLimitsSize;
    private readonly Event<GeneratedTerrainData, bool> m_terrainDataGenerated;
    private readonly Event m_terrainGeneratedButNotLoaded;
    private readonly Event m_terrainGenerated;
    private readonly Event<Tile2iAndIndex> m_heightChanged;
    private readonly Event<Tile2iAndIndex> m_tileMaterialsChanged;
    private readonly Event<Tile2iAndIndex, uint> m_tileFlagsChanged;
    private readonly Event<Tile2iAndIndex> m_oceanFlagChanged;
    private readonly Event<Tile2iAndIndex> m_tileCustomSurfaceChanged;
    private readonly ITerrainPhysicsSimulator m_physicsSimulator;
    private readonly ITerrainDisruptionSimulator m_terrainDisruptionSimulator;
    public uint BlocksBuildingsOrVehiclesMask;
    /// <summary>Number of assigned tile flags.</summary>
    private int m_tileFlagsCount;
    /// <summary>All created flags reporters.</summary>
    private LystStruct<TileFlagReporter> m_flagsReporters;
    [DoNotSave(0, null)]
    private ImmutableArray<Percent> m_disruptedAmountMults;
    public readonly bool OffLimitsDisabled;
    public readonly int ExtendTerrainByCloningCount;
    [NewInSaveVersion(118, null, null, null, null)]
    public readonly bool MapCacheDisabled;
    [DoNotSave(0, null)]
    public bool WasLoadedFromCache;
    [OnlyForSaveCompatibility(null)]
    [DoNotSave(140, null)]
    private readonly ITerrainGenerator m_terrainGenerator;
    [NewInSaveVersion(140, null, null, null, null)]
    [OnlyForSaveCompatibility(null)]
    private readonly Option<ITerrainGenerator> m_terrainGeneratorLegacy;
    [OnlyForSaveCompatibility(null)]
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly Option<IWorldRegionMap> m_worldRegionMap;
    [DoNotSave(0, null)]
    private Option<ITerrainGeneratorV2> m_terrainGeneratorV2;
    /// <summary>
    /// Save version when the map was initially created. This allows to make backwards-compatible changes to
    /// the map generation code.
    /// </summary>
    [NewInSaveVersion(143, null, null, null, null)]
    public readonly int InitialMapCreationSaveVersion;
    [RenamedInVersion(126, "m_terrainMaterialSlimIdManager")]
    public readonly TerrainMaterialsSlimIdManager TerrainMaterialSlimIdManager;
    [NewInSaveVersion(140, null, null, typeof (TileSurfaceDecalsSlimIdManager), null)]
    public readonly TileSurfaceDecalsSlimIdManager TileSurfaceDecalsSlimIdManager;
    [RenamedInVersion(126, "m_tileSurfacesSlimIdManager")]
    public readonly TileSurfacesSlimIdManager TileSurfacesSlimIdManager;
    [DoNotSave(0, typeof (IFileSystemHelper))]
    private readonly IFileSystemHelper m_fileSystemHelper;
    [NewInSaveVersion(118, null, null, typeof (IGameIdProvider), null)]
    private readonly IGameIdProvider m_gameIdProvider;
    [DoNotSave(0, typeof (ProtosDb))]
    private readonly ProtosDb m_protosDb;
    private TerrainManager.TerrainData m_data;
    [ThreadStatic]
    private static Lyst<string> s_errorsTmp;

    public static void Serialize(TerrainManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      writer.WriteUInt(this.BlocksBuildingsFlagsMask);
      writer.WriteUInt(this.BlocksBuildingsOrVehiclesMask);
      writer.WriteUInt(this.BlocksVehiclesFlagsMask);
      writer.WriteInt(this.ExtendTerrainByCloningCount);
      writer.WriteInt(this.InitialMapCreationSaveVersion);
      TerrainManager.TerrainData.Serialize(this.m_data, writer);
      LystStruct<TileFlagReporter>.Serialize(this.m_flagsReporters, writer);
      writer.WriteGeneric<IGameIdProvider>(this.m_gameIdProvider);
      Event<Tile2iAndIndex>.Serialize(this.m_heightChanged, writer);
      Event<Tile2iAndIndex>.Serialize(this.m_oceanFlagChanged, writer);
      writer.WriteGeneric<ITerrainPhysicsSimulator>(this.m_physicsSimulator);
      Event<GeneratedTerrainData, bool>.Serialize(this.m_terrainDataGenerated, writer);
      writer.WriteGeneric<ITerrainDisruptionSimulator>(this.m_terrainDisruptionSimulator);
      Event.Serialize(this.m_terrainGenerated, writer);
      Event.Serialize(this.m_terrainGeneratedButNotLoaded, writer);
      Option<ITerrainGenerator>.Serialize(this.m_terrainGeneratorLegacy, writer);
      TerrainMaterialsSlimIdManager.Serialize(this.TerrainMaterialSlimIdManager, writer);
      Event<Tile2iAndIndex>.Serialize(this.m_tileCustomSurfaceChanged, writer);
      Event<Tile2iAndIndex, uint>.Serialize(this.m_tileFlagsChanged, writer);
      writer.WriteInt(this.m_tileFlagsCount);
      Event<Tile2iAndIndex>.Serialize(this.m_tileMaterialsChanged, writer);
      TileSurfacesSlimIdManager.Serialize(this.TileSurfacesSlimIdManager, writer);
      Option<IWorldRegionMap>.Serialize(this.m_worldRegionMap, writer);
      writer.WriteBool(this.MapCacheDisabled);
      writer.WriteBool(this.OffLimitsDisabled);
      MapOffLimitsSize.Serialize(this.OffLimitsSize, writer);
      writer.WriteInt(this.TerrainHeight);
      writer.WriteInt(this.TerrainWidth);
      TileSurfaceDecalsSlimIdManager.Serialize(this.TileSurfaceDecalsSlimIdManager, writer);
    }

    public static TerrainManager Deserialize(BlobReader reader)
    {
      TerrainManager terrainManager;
      if (reader.TryStartClassDeserialization<TerrainManager>(out terrainManager))
        reader.EnqueueDataDeserialization((object) terrainManager, TerrainManager.s_deserializeDataDelayedAction);
      return terrainManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      this.BlocksBuildingsFlagsMask = reader.ReadUInt();
      this.BlocksBuildingsOrVehiclesMask = reader.ReadUInt();
      this.BlocksVehiclesFlagsMask = reader.ReadUInt();
      reader.SetField<TerrainManager>(this, "ExtendTerrainByCloningCount", (object) reader.ReadInt());
      reader.SetField<TerrainManager>(this, "InitialMapCreationSaveVersion", (object) (reader.LoadedSaveVersion >= 143 ? reader.ReadInt() : 0));
      this.m_data = TerrainManager.TerrainData.Deserialize(reader);
      reader.RegisterResolvedMember<TerrainManager>(this, "m_fileSystemHelper", typeof (IFileSystemHelper), true);
      this.m_flagsReporters = LystStruct<TileFlagReporter>.Deserialize(reader);
      reader.SetField<TerrainManager>(this, "m_gameIdProvider", reader.LoadedSaveVersion >= 118 ? (object) reader.ReadGenericAs<IGameIdProvider>() : (object) (IGameIdProvider) null);
      if (reader.LoadedSaveVersion < 118)
        reader.RegisterResolvedMember<TerrainManager>(this, "m_gameIdProvider", typeof (IGameIdProvider), true);
      reader.SetField<TerrainManager>(this, "m_heightChanged", (object) Event<Tile2iAndIndex>.Deserialize(reader));
      reader.SetField<TerrainManager>(this, "m_oceanFlagChanged", (object) Event<Tile2iAndIndex>.Deserialize(reader));
      reader.SetField<TerrainManager>(this, "m_physicsSimulator", (object) reader.ReadGenericAs<ITerrainPhysicsSimulator>());
      reader.RegisterResolvedMember<TerrainManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<TerrainManager>(this, "m_terrainDataGenerated", (object) Event<GeneratedTerrainData, bool>.Deserialize(reader));
      reader.SetField<TerrainManager>(this, "m_terrainDisruptionSimulator", (object) reader.ReadGenericAs<ITerrainDisruptionSimulator>());
      reader.SetField<TerrainManager>(this, "m_terrainGenerated", (object) Event.Deserialize(reader));
      reader.SetField<TerrainManager>(this, "m_terrainGeneratedButNotLoaded", (object) Event.Deserialize(reader));
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<TerrainManager>(this, "m_terrainGenerator", (object) reader.ReadGenericAs<ITerrainGenerator>());
      reader.SetField<TerrainManager>(this, "m_terrainGeneratorLegacy", (object) (reader.LoadedSaveVersion >= 140 ? Option<ITerrainGenerator>.Deserialize(reader) : new Option<ITerrainGenerator>()));
      reader.SetField<TerrainManager>(this, "TerrainMaterialSlimIdManager", (object) TerrainMaterialsSlimIdManager.Deserialize(reader));
      reader.SetField<TerrainManager>(this, "m_tileCustomSurfaceChanged", (object) Event<Tile2iAndIndex>.Deserialize(reader));
      reader.SetField<TerrainManager>(this, "m_tileFlagsChanged", (object) Event<Tile2iAndIndex, uint>.Deserialize(reader));
      this.m_tileFlagsCount = reader.ReadInt();
      reader.SetField<TerrainManager>(this, "m_tileMaterialsChanged", (object) Event<Tile2iAndIndex>.Deserialize(reader));
      reader.SetField<TerrainManager>(this, "TileSurfacesSlimIdManager", (object) TileSurfacesSlimIdManager.Deserialize(reader));
      reader.SetField<TerrainManager>(this, "m_worldRegionMap", (object) (reader.LoadedSaveVersion >= 140 ? Option<IWorldRegionMap>.Deserialize(reader) : new Option<IWorldRegionMap>()));
      reader.SetField<TerrainManager>(this, "MapCacheDisabled", (object) (bool) (reader.LoadedSaveVersion >= 118 ? (reader.ReadBool() ? 1 : 0) : 0));
      reader.SetField<TerrainManager>(this, "OffLimitsDisabled", (object) reader.ReadBool());
      reader.SetField<TerrainManager>(this, "OffLimitsSize", (object) (reader.LoadedSaveVersion >= 140 ? MapOffLimitsSize.Deserialize(reader) : new MapOffLimitsSize(32.Tiles(), 32.Tiles(), 32.Tiles(), 32.Tiles())));
      this.TerrainHeight = reader.ReadInt();
      this.TerrainWidth = reader.ReadInt();
      reader.SetField<TerrainManager>(this, "TileSurfaceDecalsSlimIdManager", reader.LoadedSaveVersion >= 140 ? (object) TileSurfaceDecalsSlimIdManager.Deserialize(reader) : (object) (TileSurfaceDecalsSlimIdManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<TerrainManager>(this, "TileSurfaceDecalsSlimIdManager", typeof (TileSurfaceDecalsSlimIdManager), true);
      this.initializeConstants();
      reader.RegisterInitAfterLoad<TerrainManager>(this, "initAfterLoad", InitPriority.High);
      reader.RegisterInitAfterLoad<TerrainManager>(this, "validateTerrain", InitPriority.Low);
    }

    [OnlyForSaveCompatibility(null)]
    public int TerrainWidth { get; private set; }

    public int TerrainHeight { get; private set; }

    [DoNotSave(0, null)]
    public int TerrainTilesCount { get; private set; }

    [DoNotSave(0, null)]
    public Tile2i MaxTileCoord { get; private set; }

    [DoNotSave(0, null)]
    public RectangleTerrainArea2i TerrainArea { get; private set; }

    [DoNotSave(0, null)]
    public Chunk64Area TerrainAreaChunks { get; private set; }

    [DoNotSave(0, null)]
    public Tile2i MinOnLimits { get; private set; }

    [DoNotSave(0, null)]
    public Tile2i MaxOnLimitsExcl { get; private set; }

    [DoNotSave(0, null)]
    public Tile2i MaxOnLimitsIncl { get; private set; }

    public RelTile2i TerrainSize => new RelTile2i(this.TerrainWidth, this.TerrainHeight);

    [DoNotSave(0, null)]
    public int Chunk8PerWidth { get; private set; }

    [DoNotSave(0, null)]
    public int Chunk8PerHeight { get; private set; }

    [DoNotSave(0, null)]
    public int Chunk8TotalCount { get; private set; }

    [DoNotSave(0, null)]
    public int Chunk64PerWidth { get; private set; }

    [DoNotSave(0, null)]
    public int Chunk64PerHeight { get; private set; }

    [DoNotSave(0, null)]
    public int Chunk64TotalCount { get; private set; }

    /// <summary>
    /// Deltas of four neighbors in cache-friendly order: -X, +X, -Y, +Y. Use this to iterate through neighbors
    /// efficiently.
    /// WARNING: You must be sure that this delta is applied to a tile that is NOT on the map boundary.
    /// </summary>
    [DoNotSave(0, null)]
    public ImmutableArray<Tile2iAndIndexRel> FourSideNeighborsDeltas { get; private set; }

    /// <summary>
    /// Delta indices of four neighbors in cache-friendly order: -X, +X, -Y, +Y. Use this to iterate through neighbors
    /// efficiently.
    /// WARNING: You must be sure that this delta is applied to a tile that is NOT on the map boundary.
    /// </summary>
    [DoNotSave(0, null)]
    public ImmutableArray<int> FourSideNeighborsDeltasIndices { get; private set; }

    /// <summary>
    /// Delta indices of four neighbors in cache-friendly order: (-X, -Y), (+X, -Y), (-X, +Y), (+X, +Y).
    /// Use this to iterate through neighbors efficiently.
    /// WARNING: You must be sure that this delta is applied to a tile that is NOT on the map boundary.
    /// </summary>
    [DoNotSave(0, null)]
    public ImmutableArray<Tile2iAndIndexRel> FourCornerNeighborsDeltas { get; private set; }

    /// <summary>
    /// Delta indices of eight neighbors in cache-friendly order: (-X, -Y), (0, -Y), (+X, -Y), (-X, 0), (+X, 0),
    /// (-X, +Y), (0, +Y), (+X, +Y). Use this to iterate through neighbors efficiently.
    /// WARNING: You must be sure that this delta is applied to a tile that is NOT on the map boundary.
    /// </summary>
    [DoNotSave(0, null)]
    public ImmutableArray<Tile2iAndIndexRel> EightNeighborsDeltas { get; private set; }

    /// <summary>
    /// Delta indices of four tile corners in cache-friendly order: self, +X, +Y, +XY. Use this to iterate through
    /// tile vertices efficiently.
    /// WARNING: You must be sure that this delta is applied to a tile that is NOT on the map boundary.
    /// </summary>
    [DoNotSave(0, null)]
    public ImmutableArray<Tile2iAndIndexRel> FourTileCornersDeltas { get; private set; }

    /// <summary>
    /// Bedrock is a default material that is not explicitly stored in tile layers data.
    /// Tile with no material layers will have this material.
    /// </summary>
    public TerrainMaterialProto Bedrock
    {
      get
      {
        return this.m_terrainGeneratorLegacy.ValueOrNull?.Bedrock ?? this.m_worldRegionMap.Value.BedrockMaterial;
      }
    }

    /// <summary>
    /// Called when terrain gets regenerated during new game or during game load (bool is <c>true</c> when loaded).
    /// When new game is created, this is called in <c>NewGameCreated</c> stage. When the game is loaded,
    /// this is called in the <c>InitAfterLoad</c> phase with priority <c>InitPriority.High</c>.
    /// </summary>
    public IEvent<GeneratedTerrainData, bool> TerrainDataGenerated
    {
      get => (IEvent<GeneratedTerrainData, bool>) this.m_terrainDataGenerated;
    }

    /// <summary>
    /// Invoked at the end of terrain generation, before loaded data are applied to the terrain.
    /// This can be used to apply loaded data such as removing trees and props.
    /// </summary>
    public IEvent TerrainGeneratedButNotLoaded => (IEvent) this.m_terrainGeneratedButNotLoaded;

    public IEvent TerrainGenerated => (IEvent) this.m_terrainGenerated;

    /// <summary>
    /// Invoked when height of a tile is changed. This also means that relative layer thicknesses are likely changed.
    /// Called on simulation thread.
    /// </summary>
    public IEvent<Tile2iAndIndex> HeightChanged => (IEvent<Tile2iAndIndex>) this.m_heightChanged;

    /// <summary>
    /// Invoked when only tile material layers are changed (and not height). This is to save on height processing
    /// of height when height actually did not change (such as dirt changing to grass). Called on simulation thread.
    /// </summary>
    public IEvent<Tile2iAndIndex> TileMaterialsOnlyChanged
    {
      get => (IEvent<Tile2iAndIndex>) this.m_tileMaterialsChanged;
    }

    /// <summary>
    /// Invoked when <see cref="!:TerrainTile.Flags" /> is changed.
    /// </summary>
    public IEvent<Tile2iAndIndex, uint> TileFlagsChanged
    {
      get => (IEvent<Tile2iAndIndex, uint>) this.m_tileFlagsChanged;
    }

    /// <summary>
    /// Invoked when ocean tile flag is changed. This event is special since multiple classes listen to only
    /// ocean changes and it is more efficient to only make them process this event instead of the
    /// <see cref="P:Mafi.Core.Terrain.TerrainManager.TileFlagsChanged" />.
    /// </summary>
    public IEvent<Tile2iAndIndex> OceanFlagChanged
    {
      get => (IEvent<Tile2iAndIndex>) this.m_oceanFlagChanged;
    }

    public IEvent<Tile2iAndIndex> TileCustomSurfaceChanged
    {
      get => (IEvent<Tile2iAndIndex>) this.m_tileCustomSurfaceChanged;
    }

    public bool IsProcessingPhysics => this.m_physicsSimulator.IsProcessingTiles;

    public int GetPhysicsProcessingQueueSize() => this.m_physicsSimulator.GetQueueSize();

    public bool IsProcessingDisruption => this.m_terrainDisruptionSimulator.IsProcessingTiles;

    public int GetDisruptionQueueSize() => this.m_terrainDisruptionSimulator.GetQueueSize();

    /// <summary>Masks flags that block buildings, conveyors, etc.</summary>
    public uint BlocksBuildingsFlagsMask { get; private set; }

    /// <summary>Masks flags that blocks vehicles (path-finding).</summary>
    public uint BlocksVehiclesFlagsMask { get; private set; }

    /// <summary>
    /// All terrain materials that can be indexed by their <see cref="T:Mafi.Core.Products.TerrainMaterialSlimId" />.
    /// Shorthand for <see cref="!:TerrainMaterialsSlimIdManager.ManagedProtos" />.
    /// </summary>
    [DoNotSave(0, null)]
    public ImmutableArray<TerrainMaterialProto> TerrainMaterials { get; private set; }

    [DoNotSave(0, null)]
    public ImmutableArray<LooseProductProto> MinedProducts { get; private set; }

    /// <summary>Converts material ID to disrupted material ID.</summary>
    [DoNotSave(0, null)]
    public ImmutableArray<TerrainMaterialSlimId> DisruptedMaterialIds { get; private set; }

    /// <summary>Converts material ID to recovered material ID.</summary>
    [DoNotSave(0, null)]
    public ImmutableArray<TerrainMaterialSlimId> RecoveredMaterialIds { get; private set; }

    [DoNotSave(0, null)]
    public ImmutableArray<TerrainTileSurfaceProto> TerrainSurfaces { get; private set; }

    [DoNotSave(0, null)]
    public bool IsGeneratingTerrain { get; private set; }

    [DoNotSave(0, null)]
    public bool IsGeneratingTerrainLegacy { get; private set; }

    public ReadOnlyArray<HeightTilesF> HeightsData
    {
      get => new ReadOnlyArray<HeightTilesF>(this.m_data.Heights);
    }

    public ReadOnlyArray<TileMaterialLayers> TileLayersData
    {
      get => new ReadOnlyArray<TileMaterialLayers>(this.m_data.MaterialLayers);
    }

    public ReadOnlyArray<TileSurfaceData> TileSurfacesData
    {
      get => new ReadOnlyArray<TileSurfaceData>(this.m_data.Surfaces);
    }

    public ReadOnlyArraySlice<TileMaterialLayerOverflow> MaterialLayersOverflowData
    {
      get => this.m_data.MaterialLayersOverflow.BackingArrayAsSlice;
    }

    public ReadOnlyArray<ushort> TileFlags => new ReadOnlyArray<ushort>(this.m_data.Flags);

    /// <summary>
    /// Returns reference mutable terrain data. You must retain the ref, passing this by value will result
    /// in errors as data won't be properly stored. Be extremely careful with this one.
    /// </summary>
    internal ref TerrainManager.TerrainData GetMutableTerrainDataRef() => ref this.m_data;

    public TerrainManager(
      TerrainManagerConfig config,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      IWorldRegionMap worldRegionMap,
      ITerrainGenerator legacyTerrainGenerator,
      ITerrainGeneratorV2 terrainGeneratorV2,
      ITerrainPhysicsSimulator terrainPhysicsSimulator,
      ITerrainDisruptionSimulator terrainSurfaceSimulator,
      TerrainMaterialsSlimIdManager terrainMaterialSlimIdManager,
      TileSurfacesSlimIdManager tileSurfacesSlimIdManager,
      TileSurfaceDecalsSlimIdManager tileSurfaceDecalsSlimIdManager,
      IFileSystemHelper fileSystemHelper,
      IGameIdProvider gameIdProvider,
      ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_terrainDataGenerated = new Event<GeneratedTerrainData, bool>();
      this.m_terrainGeneratedButNotLoaded = new Event();
      this.m_terrainGenerated = new Event();
      this.m_heightChanged = new Event<Tile2iAndIndex>();
      this.m_tileMaterialsChanged = new Event<Tile2iAndIndex>();
      this.m_tileFlagsChanged = new Event<Tile2iAndIndex, uint>();
      this.m_oceanFlagChanged = new Event<Tile2iAndIndex>();
      this.m_tileCustomSurfaceChanged = new Event<Tile2iAndIndex>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBlocksVehiclesFlagsMask\u003Ek__BackingField = 7U;
      this.BlocksBuildingsOrVehiclesMask = 7U;
      this.m_tileFlagsCount = 3;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_physicsSimulator = terrainPhysicsSimulator.CheckNotNull<ITerrainPhysicsSimulator>();
      this.m_terrainDisruptionSimulator = terrainSurfaceSimulator.CheckNotNull<ITerrainDisruptionSimulator>();
      this.TerrainMaterialSlimIdManager = terrainMaterialSlimIdManager;
      this.TileSurfacesSlimIdManager = tileSurfacesSlimIdManager;
      this.TileSurfaceDecalsSlimIdManager = tileSurfaceDecalsSlimIdManager;
      this.m_fileSystemHelper = fileSystemHelper;
      this.m_gameIdProvider = gameIdProvider;
      this.m_protosDb = protosDb;
      Mafi.Assert.That<bool>(config.DisableMarkingOfOffLimitsAreas).IsFalse("Off-limits cannot be disabled in production.");
      Mafi.Assert.That<bool>(config.AllowDenormalizedTerrainSize).IsFalse("Denormalized terrain size cannot be enabled in production.");
      this.OffLimitsDisabled = false;
      this.ExtendTerrainByCloningCount = 0;
      bool allowDenorm = false;
      this.MapCacheDisabled = config.MapCacheDisabled;
      this.InitialMapCreationSaveVersion = 168;
      if (worldRegionMap.Size == RelTile2i.Zero)
      {
        this.m_terrainGeneratorLegacy = legacyTerrainGenerator.SomeOption<ITerrainGenerator>();
        this.TerrainWidth = this.checkTerrainDimension(legacyTerrainGenerator.TerrainWidth, "width", allowDenorm);
        this.TerrainHeight = this.checkTerrainDimension(legacyTerrainGenerator.TerrainHeight, "height", allowDenorm);
        RelTile1i relTile1i = 32.Tiles();
        this.OffLimitsSize = new MapOffLimitsSize(relTile1i, relTile1i, relTile1i, relTile1i);
      }
      else
      {
        IWorldRegionMap worldRegionMap1 = worldRegionMap;
        this.m_worldRegionMap = worldRegionMap1.SomeOption<IWorldRegionMap>();
        this.m_terrainGeneratorV2 = terrainGeneratorV2.SomeOption<ITerrainGeneratorV2>();
        this.TerrainWidth = this.checkTerrainDimension(worldRegionMap1.Size.X, "width", allowDenorm);
        this.TerrainHeight = this.checkTerrainDimension(worldRegionMap1.Size.Y, "height", allowDenorm);
        this.OffLimitsSize = this.OffLimitsDisabled ? worldRegionMap1.OffLimitsSize : this.checkOffLimitsSize(worldRegionMap1.OffLimitsSize);
      }
      if (this.TerrainWidth * this.TerrainHeight > 67108864)
        throw new FatalGameException(string.Format("Terrain is too vast! It's {0}x{1} tiles with area of ", (object) this.TerrainWidth, (object) this.TerrainHeight) + string.Format("{0} but maximum area is {1} tiles.", (object) (this.TerrainWidth * this.TerrainHeight), (object) 67108864));
      Mafi.Log.Info(string.Format("Allocating terrain {0}", (object) this.TerrainSize));
      this.m_data = new TerrainManager.TerrainData(this.TerrainWidth, this.TerrainHeight);
      if (config.EnableHeightSnapshotting)
        this.m_data.EnableAndInitializeHeightSnapshot();
      this.initializeConstants();
      this.initializeTerrain();
      ++this.m_tileFlagsCount;
      this.m_physicsSimulator.Initialize(this);
      this.m_terrainDisruptionSimulator.Initialize(this);
      gameLoopEvents.RegisterNewGameCreated((object) this, this.m_terrainGeneratorLegacy.HasValue ? this.generateTerrainLegacy(this.m_terrainGeneratorLegacy.Value, false) : this.generateTerrainV2(this.m_terrainGeneratorV2.Value));
      simLoopEvents.Update.Add<TerrainManager>(this, new Action(this.update));
    }

    [InitAfterLoad(InitPriority.ImmediatelyAfterSelfDeserialized)]
    private void initializeConstants()
    {
      if (this.TerrainWidth % 64 != 0 || (uint) this.TerrainWidth > 32768U)
        throw new CorruptedSaveException(string.Format("Failed to read valid terrain width: {0}", (object) this.TerrainWidth));
      if (this.TerrainHeight % 64 != 0 || (uint) this.TerrainHeight > 32768U)
        throw new CorruptedSaveException(string.Format("Failed to read valid terrain width: {0}", (object) this.TerrainWidth));
      if (this.m_terrainGenerator != null)
      {
        ReflectionUtils.SetField<TerrainManager>(this, "m_terrainGeneratorLegacy", (object) this.m_terrainGenerator.CreateOption<ITerrainGenerator>());
        ReflectionUtils.SetField<TerrainManager>(this, "m_terrainGenerator", (object) null);
      }
      if (this.m_data.Width == 0)
      {
        object data = (object) this.m_data;
        ReflectionUtils.SetField(data, typeof (TerrainManager.TerrainData), "Width", (object) this.TerrainWidth);
        ReflectionUtils.SetField(data, typeof (TerrainManager.TerrainData), "Height", (object) this.TerrainHeight);
        this.m_data = (TerrainManager.TerrainData) data;
      }
      bool flag = this.OffLimitsDisabled;
      MapOffLimitsSize offLimitsSize = this.OffLimitsSize;
      if (flag)
      {
        Mafi.Log.Warning("Disabled in saved game");
        flag = false;
        ReflectionUtils.SetField<TerrainManager>(this, "OffLimitsDisabled", (object) false);
      }
      MapOffLimitsSize mapOffLimitsSize = this.checkOffLimitsSize(this.OffLimitsSize);
      ReflectionUtils.SetField<TerrainManager>(this, "OffLimitsSize", (object) mapOffLimitsSize);
      this.TerrainTilesCount = this.TerrainWidth * this.TerrainHeight;
      this.TerrainArea = new RectangleTerrainArea2i(Tile2i.Zero, new RelTile2i(this.TerrainWidth, this.TerrainHeight));
      this.TerrainAreaChunks = new Chunk64Area(Chunk2i.Zero, this.TerrainSize.Vector2i / 64);
      this.MaxTileCoord = new Tile2i(this.TerrainWidth - 1, this.TerrainHeight - 1);
      this.MinOnLimits = flag ? new Tile2i(0, 0) : new Tile2i(mapOffLimitsSize.MinusX.Value, mapOffLimitsSize.MinusY.Value);
      this.MaxOnLimitsExcl = flag ? new Tile2i(this.TerrainWidth, this.TerrainHeight) : new Tile2i(this.TerrainWidth - mapOffLimitsSize.PlusX.Value, this.TerrainHeight - mapOffLimitsSize.PlusY.Value);
      this.MaxOnLimitsIncl = this.MaxOnLimitsExcl - RelTile2i.One;
      this.Chunk8PerWidth = this.TerrainWidth / 8;
      this.Chunk8PerHeight = this.TerrainHeight / 8;
      this.Chunk8TotalCount = this.Chunk8PerWidth * this.Chunk8PerHeight;
      this.Chunk64PerWidth = this.TerrainWidth / 64;
      this.Chunk64PerHeight = this.TerrainHeight / 64;
      this.Chunk64TotalCount = this.Chunk64PerWidth * this.Chunk64PerHeight;
      this.FourSideNeighborsDeltas = ImmutableArray.Create<Tile2iAndIndexRel>(new Tile2iAndIndexRel((short) -1, (short) 0, -1), new Tile2iAndIndexRel((short) 1, (short) 0, 1), new Tile2iAndIndexRel((short) 0, (short) -1, -this.TerrainWidth), new Tile2iAndIndexRel((short) 0, (short) 1, this.TerrainWidth));
      this.FourSideNeighborsDeltasIndices = this.FourSideNeighborsDeltas.Map<int>((Func<Tile2iAndIndexRel, int>) (x => x.IndexDelta));
      this.FourTileCornersDeltas = ImmutableArray.Create<Tile2iAndIndexRel>(new Tile2iAndIndexRel((short) 0, (short) 0, 0), new Tile2iAndIndexRel((short) 1, (short) 0, 1), new Tile2iAndIndexRel((short) 0, (short) 1, this.TerrainWidth), new Tile2iAndIndexRel((short) 1, (short) 1, this.TerrainWidth + 1));
      this.FourCornerNeighborsDeltas = ImmutableArray.Create<Tile2iAndIndexRel>(new Tile2iAndIndexRel((short) -1, (short) -1, -this.TerrainWidth - 1), new Tile2iAndIndexRel((short) 1, (short) -1, -this.TerrainWidth + 1), new Tile2iAndIndexRel((short) -1, (short) 1, this.TerrainWidth - 1), new Tile2iAndIndexRel((short) 1, (short) 1, this.TerrainWidth + 1));
      this.EightNeighborsDeltas = ImmutableArray.Create<Tile2iAndIndexRel>(new Tile2iAndIndexRel((short) -1, (short) -1, -this.TerrainWidth - 1), new Tile2iAndIndexRel((short) 0, (short) -1, -this.TerrainWidth), new Tile2iAndIndexRel((short) 1, (short) -1, -this.TerrainWidth + 1), new Tile2iAndIndexRel((short) -1, (short) 0, -1), new Tile2iAndIndexRel((short) 1, (short) 0, 1), new Tile2iAndIndexRel((short) -1, (short) 1, this.TerrainWidth - 1), new Tile2iAndIndexRel((short) 0, (short) 1, this.TerrainWidth), new Tile2iAndIndexRel((short) 1, (short) 1, this.TerrainWidth + 1));
    }

    [InitAfterLoad(InitPriority.High)]
    private IEnumerator<string> initAfterLoad(DependencyResolver resolver)
    {
      if (this.m_terrainGeneratorLegacy.IsNone)
        this.m_terrainGeneratorV2 = resolver.Resolve<ITerrainGeneratorV2>().SomeOption<ITerrainGeneratorV2>();
      this.initializeTerrain();
      return !this.m_terrainGeneratorLegacy.HasValue ? this.generateTerrainV2(this.m_terrainGeneratorV2.Value) : this.generateTerrainLegacy(this.m_terrainGeneratorLegacy.Value, true);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void validateTerrain(DependencyResolver resolver)
    {
      TileSurfaceData[] surfaces = this.m_data.Surfaces;
      ThicknessTilesF half = ThicknessTilesF.Half;
      TerrainOccupancyManager occupancyManager = resolver.Resolve<TerrainOccupancyManager>();
      for (int index1 = 0; index1 < surfaces.Length; ++index1)
      {
        TileSurfaceData tileSurfaceData = surfaces[index1];
        if (tileSurfaceData.RawValue != 0)
        {
          if (tileSurfaceData.IsNotValid)
          {
            Mafi.Log.Warning(string.Format("Tile surface was invalid but it was not entirely zero, clearing: {0:X8}", (object) tileSurfaceData.RawValue));
            surfaces[index1] = new TileSurfaceData();
          }
          else
          {
            Tile2iIndex index2 = new Tile2iIndex(index1);
            if (this.IsOnMapBoundary(index2))
            {
              Mafi.Log.Warning("Terrain surface is on terrain boundary, removing.");
              this.ClearCustomSurface(this.ExtendTileCoord_Slow(index2));
            }
            else
            {
              HeightTilesF height = surfaces[index1].Height;
              HeightTilesF heightTilesF = this.m_data.Heights[index1].Max(this.m_data.Heights[index1 + 1]).Max(this.m_data.Heights[index1 + this.TerrainWidth]).Max(this.m_data.Heights[index1 + this.TerrainWidth + 1]);
              if (height > heightTilesF + half && !occupancyManager.IsOccupiedAt(index2, height.HeightI))
              {
                Tile2iAndIndex tileAndIndex = this.ExtendTileCoord_Slow(index2);
                Mafi.Log.Warning(string.Format("Terrain surface at {0} is levitating at height ", (object) tileAndIndex.TileCoord) + string.Format("{0} above the terrain at {1}, removing.", (object) surfaces[index1].Height, (object) heightTilesF));
                this.ClearCustomSurface(tileAndIndex);
              }
              else if (tileSurfaceData.IsAutoPlaced && !occupancyManager.TryGetOccupyingEntity(index2, (Predicate<IStaticEntity>) (p => true), out IStaticEntity _))
              {
                Mafi.Log.Warning(string.Format("Terrain surface at {0} is auto placed ", (object) this.ExtendTileCoord_Slow(index2).TileCoord) + "but has no corresponding entity, removing auto placed property.");
                surfaces[index1] = new TileSurfaceData(tileSurfaceData.RawValue & -1025, tileSurfaceData.RawValueDecal);
              }
            }
          }
        }
      }
    }

    private int checkTerrainDimension(int value, string name, bool allowDenorm)
    {
      if (value > 32768)
      {
        Mafi.Log.Warning(string.Format("Terrain {0} of {1} is too large, ", (object) name, (object) value) + string.Format("reducing it to the maximum size of {0}.", (object) 32768));
        return 32768;
      }
      if (!allowDenorm && (value & (int) byte.MaxValue) != 0)
        value = (value & -256) + 256;
      if (this.ExtendTerrainByCloningCount > 1)
        value *= this.ExtendTerrainByCloningCount;
      return value;
    }

    private MapOffLimitsSize checkOffLimitsSize(MapOffLimitsSize limits)
    {
      return new MapOffLimitsSize(this.checkOffLimitsDimension(limits.MinusX, this.TerrainWidth, "-X"), this.checkOffLimitsDimension(limits.MinusY, this.TerrainHeight, "-Y"), this.checkOffLimitsDimension(limits.PlusX, this.TerrainWidth, "+X"), this.checkOffLimitsDimension(limits.PlusY, this.TerrainHeight, "+Y"));
    }

    private RelTile1i checkOffLimitsDimension(RelTile1i value, int mapSize, string name)
    {
      if (value.Value < 32)
      {
        Mafi.Log.Warning(string.Format("Off-limits value {0} of {1} is too small, using {2}.", (object) name, (object) value, (object) 32));
        return new RelTile1i(32);
      }
      int max = (mapSize / 4).CeilToPowerOfTwoOrZero().Min(256);
      Mafi.Assert.That<int>(max).IsGreaterOrEqual(32);
      if (value.Value > max)
      {
        Mafi.Log.Warning(string.Format("Off-limits value {0} of {1} is too large, using {2}.", (object) name, (object) value, (object) max));
        return new RelTile1i(max);
      }
      if (value.Value.IsPowerOfTwo())
        return value;
      int num = value.Value.CeilToPowerOfTwoOrZero().Clamp(32, max);
      Mafi.Log.Warning(string.Format("Off-limits value {0} of {1} is not a power of two, using {2}.", (object) name, (object) value, (object) num));
      return new RelTile1i(num);
    }

    private void update()
    {
      this.m_terrainDisruptionSimulator.Update();
      this.m_physicsSimulator.Update();
    }

    private void initializeTerrain()
    {
      this.TerrainMaterials = this.TerrainMaterialSlimIdManager.ManagedProtos;
      this.TerrainSurfaces = this.TileSurfacesSlimIdManager.ManagedProtos;
      ImmutableArray<TerrainMaterialProto> terrainMaterials = this.TerrainMaterials;
      this.DisruptedMaterialIds = terrainMaterials.Map<TerrainMaterialSlimId>((Func<TerrainMaterialProto, TerrainMaterialSlimId>) (x =>
      {
        TerrainMaterialProto valueOrNull = x.DisruptedMaterialProto.ValueOrNull;
        // ISSUE: explicit non-virtual call
        TerrainMaterialSlimId terrainMaterialSlimId = valueOrNull != null ? __nonvirtual (valueOrNull.SlimId) : TerrainMaterialSlimId.PhantomId;
        if (terrainMaterialSlimId.IsNotPhantom && terrainMaterialSlimId == x.SlimId)
        {
          Mafi.Log.Warning(string.Format("Terrain material '{0}' is disrupted to itself, removing disruption material.", (object) x));
          terrainMaterialSlimId = TerrainMaterialSlimId.PhantomId;
        }
        return terrainMaterialSlimId;
      }));
      terrainMaterials = this.TerrainMaterials;
      this.RecoveredMaterialIds = terrainMaterials.Map<TerrainMaterialSlimId>((Func<TerrainMaterialProto, TerrainMaterialSlimId>) (x =>
      {
        TerrainMaterialProto valueOrNull = x.RecoveredMaterialProto.ValueOrNull;
        // ISSUE: explicit non-virtual call
        TerrainMaterialSlimId terrainMaterialSlimId = valueOrNull != null ? __nonvirtual (valueOrNull.SlimId) : TerrainMaterialSlimId.PhantomId;
        if (terrainMaterialSlimId.IsNotPhantom && terrainMaterialSlimId == x.SlimId)
        {
          Mafi.Log.Warning(string.Format("Terrain material '{0}' is recovered to itself, removing recovery material.", (object) x));
          terrainMaterialSlimId = TerrainMaterialSlimId.PhantomId;
        }
        return terrainMaterialSlimId;
      }));
      terrainMaterials = this.TerrainMaterials;
      this.MinedProducts = terrainMaterials.Map<LooseProductProto>((Func<TerrainMaterialProto, LooseProductProto>) (x => x.MinedProduct));
      terrainMaterials = this.TerrainMaterials;
      this.m_disruptedAmountMults = terrainMaterials.Map<Percent>((Func<TerrainMaterialProto, Percent>) (x => x.DisruptionSpeedMult));
      this.m_data.SavedFlagsMask = 4U;
      foreach (TileFlagReporter flagsReporter in this.m_flagsReporters)
      {
        if (flagsReporter.IsSaved)
          this.m_data.SavedFlagsMask |= flagsReporter.FlagMask;
      }
    }

    [OnlyForSaveCompatibility(null)]
    private IEnumerator<string> generateTerrainOrLoadCache(
      ITerrainGenerator generator,
      RelTile2i size,
      Dict<Chunk2i, ChunkTerrainData> resultChunks,
      bool saveCache)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new TerrainManager.\u003CgenerateTerrainOrLoadCache\u003Ed__216(0)
      {
        \u003C\u003E4__this = this,
        generator = generator,
        size = size,
        resultChunks = resultChunks,
        saveCache = saveCache
      };
    }

    private bool tryReadTerrainCacheData(
      string cacheFilePath,
      Dict<Chunk2i, ChunkTerrainData> resultChunks)
    {
      resultChunks.Clear();
      using (FileStream fileStream = new FileStream(cacheFilePath, FileMode.Open))
      {
        using (GZipStream inputStream = new GZipStream((Stream) fileStream, CompressionMode.Decompress))
        {
          BlobReader reader = new BlobReader((Stream) inputStream, 168);
          reader.SetSpecialSerializers(ImmutableArray.Create<ISpecialSerializerFactory>((ISpecialSerializerFactory) new ProtosSerializerFactory(this.m_protosDb)));
          if (reader.ReadLong() != this.m_gameIdProvider.GameId)
          {
            Mafi.Log.Warning("Game ID of cached terrain data does not match, ignoring cache.");
            return false;
          }
          if (reader.ReadInt() != 1)
            return false;
          int num = reader.ReadIntNotNegative();
          for (int index1 = 0; index1 < num; ++index1)
          {
            Chunk2i chunk2i = Chunk2i.Deserialize(reader);
            int length = reader.ReadInt();
            TileTerrainData[] data = new TileTerrainData[length];
            for (int index2 = 0; index2 < length; ++index2)
              data[index2] = TileTerrainData.DeserializeFromCache(reader);
            resultChunks.Add(chunk2i, new ChunkTerrainData(chunk2i, data));
          }
          reader.FinalizeLoading(Option<DependencyResolver>.None);
          return true;
        }
      }
    }

    private bool tryWriteTerrainCacheData(
      string cacheFilePath,
      Dict<Chunk2i, ChunkTerrainData> resultChunks)
    {
      using (FileStream fileStream = new FileStream(cacheFilePath, FileMode.CreateNew))
      {
        using (GZipStream outputStream = new GZipStream((Stream) fileStream, CompressionMode.Compress))
        {
          using (BlobWriter writer = new BlobWriter((Stream) outputStream, new ImmutableArray<ISpecialSerializerFactory>?(ImmutableArray.Create<ISpecialSerializerFactory>((ISpecialSerializerFactory) new ProtosSerializerFactory(this.m_protosDb)))))
          {
            writer.WriteLong(this.m_gameIdProvider.GameId);
            writer.WriteInt(1);
            writer.WriteIntNotNegative(resultChunks.Count);
            foreach (KeyValuePair<Chunk2i, ChunkTerrainData> resultChunk in resultChunks)
            {
              Chunk2i.Serialize(resultChunk.Key, writer);
              TileTerrainData[] data1 = resultChunk.Value.Data;
              writer.WriteInt(data1.Length);
              foreach (TileTerrainData data2 in data1)
                TileTerrainData.SerializeToCache(data2, writer);
            }
            writer.FinalizeSerialization();
            return true;
          }
        }
      }
    }

    [OnlyForSaveCompatibility(null)]
    private IEnumerator<string> generateTerrainLegacy(
      ITerrainGenerator generator,
      bool gameIsBeingLoaded)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new TerrainManager.\u003CgenerateTerrainLegacy\u003Ed__219(0)
      {
        \u003C\u003E4__this = this,
        generator = generator,
        gameIsBeingLoaded = gameIsBeingLoaded
      };
    }

    private IEnumerator<Percent> applyLoadedData(
      Lyst<Pair<Tile2iIndex, TerrainManager.LoadedTileData>> data,
      Lyst<TileMaterialLayerOverflow> overflow,
      int tilesPerStage = 10000)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<Percent>) new TerrainManager.\u003CapplyLoadedData\u003Ed__220(0)
      {
        \u003C\u003E4__this = this,
        data = data,
        overflow = overflow,
        tilesPerStage = tilesPerStage
      };
    }

    private IEnumerator<string> generateTerrainV2(ITerrainGeneratorV2 generator)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new TerrainManager.\u003CgenerateTerrainV2\u003Ed__221(0)
      {
        \u003C\u003E4__this = this,
        generator = generator
      };
    }

    public bool IsOceanSeed(Tile2i tile)
    {
      if (tile.X <= 0)
        return !this.m_worldRegionMap.Value.MapEdgeType.GroundTowardsMinusX;
      if (tile.X + 1 >= this.TerrainWidth)
        return !this.m_worldRegionMap.Value.MapEdgeType.GroundTowardsPlusX;
      if (tile.Y <= 0)
        return !this.m_worldRegionMap.Value.MapEdgeType.GroundTowardsMinusY;
      return tile.Y + 1 >= this.TerrainHeight && !this.m_worldRegionMap.Value.MapEdgeType.GroundTowardsPlusY;
    }

    [OnlyForSaveCompatibility(null)]
    private void createOcean()
    {
      Queueue<Tile2iIndex> queueue = new Queueue<Tile2iIndex>();
      foreach (Tile2iIndex enumerateBoundaryIndex in this.TerrainArea.EnumerateBoundaryIndices(this))
      {
        if (!(this.m_data.Heights[enumerateBoundaryIndex.Value] >= OceanTerrainManager.OCEAN_THRESHOLD))
        {
          this.m_data.Flags[enumerateBoundaryIndex.Value] |= (ushort) 4;
          queueue.Enqueue(enumerateBoundaryIndex);
        }
      }
      while (queueue.IsNotEmpty)
      {
        Tile2iIndex index1 = queueue.Dequeue();
        if (this.IsOnMapBoundary(index1))
        {
          foreach (int neighborsDeltasIndex in this.FourSideNeighborsDeltasIndices)
          {
            Tile2iIndex index2 = index1 + neighborsDeltasIndex;
            if (this.IsValidIndex(index2) && !this.IsOcean(index2) && this.m_data.Heights[index2.Value] < OceanTerrainManager.OCEAN_THRESHOLD)
            {
              this.m_data.Flags[index2.Value] |= (ushort) 4;
              queueue.Enqueue(index2);
            }
          }
        }
        else
        {
          foreach (int neighborsDeltasIndex in this.FourSideNeighborsDeltasIndices)
          {
            Tile2iIndex index3 = index1 + neighborsDeltasIndex;
            if (!this.IsOcean(index3) && this.m_data.Heights[index3.Value] < OceanTerrainManager.OCEAN_THRESHOLD)
            {
              this.m_data.Flags[index3.Value] |= (ushort) 4;
              queueue.Enqueue(index3);
            }
          }
        }
      }
    }

    private void setChunkData(ChunkTerrainData chunkData, Tile2i origin)
    {
      TileTerrainData[] data = chunkData.Data;
      if (!this.IsValidCoord(origin) || !this.IsValidCoord(origin + 63))
      {
        Mafi.Log.Warning(string.Format("Skipping generated chunk {0} + {1} ", (object) origin, (object) TerrainChunk.Size2i) + string.Format("that is not on the terrain of size {0}.", (object) this.TerrainSize));
      }
      else
      {
        ulong[] backingArray = this.m_data.ChangedTiles.BackingArray;
        int num1 = this.GetTileIndex(origin).Value;
        Mafi.Assert.That<int>(num1 & 63).IsZero();
        int index1 = num1 >> 6;
        int num2 = this.TerrainWidth >> 6;
        int num3 = 0;
        while (num3 < 64)
        {
          int num4 = num3 * this.TerrainWidth + num1;
          if (backingArray[index1] != ulong.MaxValue)
          {
            int num5 = num3 * 64;
            for (int index2 = 0; index2 < 64; ++index2)
            {
              TileTerrainData tileTerrainData = data[num5 + index2];
              int index3 = num4 + index2;
              if (!this.m_data.ChangedTiles.IsSet(index3))
              {
                this.m_data.Heights[index3] = tileTerrainData.SurfaceHeight;
                this.setTileMaterialLayers(ref this.m_data.MaterialLayers[index3], tileTerrainData.Products);
                if (tileTerrainData.Products.Count > 100)
                  Mafi.Log.Warning("Suspiciously too many layers on tile " + string.Format("{0}: {1}.", (object) this.IndexToTile_Slow(new Tile2iIndex(index3)), (object) tileTerrainData.Products.Count));
              }
            }
          }
          ++num3;
          index1 += num2;
        }
      }
    }

    [OnlyForSaveCompatibility(null)]
    private void setTileMaterialLayers(
      ref TileMaterialLayers data,
      LystStruct<TerrainMaterialThicknessSlim> layers)
    {
      data.Count = layers.Count;
      if (data.Count <= 0)
        return;
      if (data.Count <= 4)
      {
        if (data.Count >= 1)
          data.First = layers[idx(0)];
        if (data.Count >= 2)
          data.Second = layers[idx(1)];
        if (data.Count >= 3)
          data.Third = layers[idx(2)];
        if (data.Count < 4)
          return;
        data.Fourth = layers[idx(3)];
      }
      else
      {
        data.First = layers[idx(0)];
        data.Second = layers[idx(1)];
        data.Third = layers[idx(2)];
        data.Fourth = layers[idx(3)];
        data.OverflowIndex = this.m_data.AddLayerToOverflow(layers[idx(4)], 0);
        int index = data.OverflowIndex;
        for (int x = 5; x < data.Count; ++x)
        {
          int overflow = this.m_data.AddLayerToOverflow(layers[idx(x)], 0);
          this.m_data.MaterialLayersOverflow.GetBackingArray()[index].OverflowIndex = overflow;
          index = overflow;
        }
      }

      int idx(int x) => layers.Count - x - 1;
    }

    internal void ContinueTerrainPhysicsStabilization(int steps)
    {
      for (int index = 0; index < steps && this.m_physicsSimulator.IsProcessingTiles; ++index)
        this.m_physicsSimulator.Update();
    }

    public void StartTerrainPhysicsSimulationAt(Tile2iAndIndex tileAndIndex)
    {
      this.m_physicsSimulator.StartPhysicsSimulationAt(tileAndIndex);
    }

    public void StopTerrainPhysicsSimulationAt(Tile2iAndIndex tileAndIndex)
    {
      this.m_physicsSimulator.StopPhysicsSimulationAt(tileAndIndex);
    }

    internal void ClearTerrainPhysicsSimulation() => this.m_physicsSimulator.Clear();

    /// <summary>Gets a tile on given coordinate.</summary>
    public TerrainTile this[Tile2i coord] => new TerrainTile(coord, this);

    public Tile2iIndex GetTileIndex(int x, int y) => new Tile2iIndex(y * this.TerrainWidth + x);

    public Tile2iIndex GetTileIndex(Tile2i tile)
    {
      return new Tile2iIndex(tile.Y * this.TerrainWidth + tile.X);
    }

    public Tile2iIndex GetTileIndex(Tile2iSlim tile)
    {
      return new Tile2iIndex((int) tile.Y * this.TerrainWidth + (int) tile.X);
    }

    public Tile2iAndIndex ExtendTileIndex(int x, int y)
    {
      return new Tile2iAndIndex((ushort) x, (ushort) y, y * this.TerrainWidth + x);
    }

    public Tile2iAndIndex ExtendTileIndex(Tile2iSlim tile)
    {
      return new Tile2iAndIndex(tile.X, tile.Y, (int) tile.Y * this.TerrainWidth + (int) tile.X);
    }

    public Tile2iAndIndex ExtendTileIndex(Tile2i tile)
    {
      return new Tile2iAndIndex((ushort) tile.X, (ushort) tile.Y, tile.Y * this.TerrainWidth + tile.X);
    }

    public Chunk8Index GetChunk8Index(Tile2i tile)
    {
      return new Chunk8Index((tile.X >> 3) + (tile.Y >> 3) * this.Chunk8PerWidth);
    }

    public Chunk8Index GetChunk8Index(Tile2iSlim tile)
    {
      return new Chunk8Index(((int) tile.X >> 3) + ((int) tile.Y >> 3) * this.Chunk8PerWidth);
    }

    public Chunk64Index GetChunk64Index(Tile2i tile)
    {
      return new Chunk64Index((tile.X >> 6) + (tile.Y >> 6) * this.Chunk64PerWidth);
    }

    public Chunk64Index GetChunk64Index(Tile2iSlim tile)
    {
      return new Chunk64Index(((int) tile.X >> 6) + ((int) tile.Y >> 6) * this.Chunk64PerWidth);
    }

    public Chunk64Index GetChunk64Index(Chunk2i chunk)
    {
      return new Chunk64Index(chunk.X + chunk.Y * this.Chunk64PerWidth);
    }

    /// <summary>Checks whether the given index is valid.</summary>
    /// <remarks>
    /// For internal operations that access data arrays directly, prefer checking the index value
    /// against the array length, so that automated array bounds check can be eliminated.
    /// </remarks>
    public bool IsValidIndex(Tile2iIndex index)
    {
      return (uint) index.Value < (uint) this.TerrainTilesCount;
    }

    public bool IsValidCoord(Tile2i coord)
    {
      return (uint) coord.X < (uint) this.TerrainWidth && (uint) coord.Y < (uint) this.TerrainHeight;
    }

    public bool IsValidCoord(Tile2f coord)
    {
      return (uint) coord.X.ToIntFloored() < (uint) this.TerrainWidth && (uint) coord.Y.ToIntFloored() < (uint) this.TerrainHeight;
    }

    public bool IsValidCoord(Tile2iSlim coord)
    {
      return (uint) coord.X < (uint) this.TerrainWidth && (uint) coord.Y < (uint) this.TerrainHeight;
    }

    public bool IsOffLimitsOrInvalid(Tile2i coord)
    {
      return coord.X < this.MinOnLimits.X || coord.Y < this.MinOnLimits.Y || coord.X >= this.MaxOnLimitsExcl.X || coord.Y >= this.MaxOnLimitsExcl.Y;
    }

    public Tile2i IndexToTile_Slow(Tile2iIndex index)
    {
      return new Tile2i(index.Value % this.TerrainWidth, index.Value / this.TerrainWidth);
    }

    public Tile2iAndIndex ExtendTileCoord_Slow(Tile2iIndex index)
    {
      return new Tile2iAndIndex((ushort) (index.Value % this.TerrainWidth), (ushort) (index.Value / this.TerrainWidth), index.Value);
    }

    public Tile2iAndIndex OffsetUnclamped(Tile2iAndIndex tileAndIndex, int dx, int dy)
    {
      return this.ExtendTileIndex((int) tileAndIndex.X + dx, (int) tileAndIndex.Y + dy);
    }

    public Tile2iAndIndex OffsetAndClamp(Tile2iAndIndex tileAndIndex, int dx, int dy)
    {
      return this.ExtendTileIndex(((int) tileAndIndex.X + dx).Clamp(0, this.TerrainWidth - 1), ((int) tileAndIndex.Y + dy).Clamp(0, this.TerrainHeight - 1));
    }

    public Tile2i ClampToTerrainBounds(Tile2i coord)
    {
      return coord.Max(Tile2i.Zero).Min(this.MaxTileCoord);
    }

    public Tile2f ClampToTerrainBounds(Tile2f coord)
    {
      return coord.Max(Tile2f.Zero).Min(new Tile2f((Fix32) this.TerrainWidth - Fix32.Epsilon, (Fix32) this.TerrainHeight - Fix32.Epsilon));
    }

    public Tile3i ClampToTerrainBounds(Tile3i coord)
    {
      Tile2i tile2i = coord.Xy;
      tile2i = tile2i.Max(Tile2i.Zero);
      tile2i = tile2i.Min(this.MaxTileCoord);
      return tile2i.ExtendZ(coord.Z);
    }

    public Tile2i ClampToTerrainLimits(Tile2i coord)
    {
      return coord.Max(this.MinOnLimits).Min(this.MaxOnLimitsIncl);
    }

    public bool IsFlat(Tile2iIndex i)
    {
      HeightTilesF height = this.GetHeight(i);
      return !this.IsOnMapBoundary(i) && this.GetHeight(i.PlusXNeighborUnchecked) == height && this.GetHeight(i.PlusYNeighborUnchecked(this.TerrainWidth)) == height && this.GetHeight(i.PlusXPlusYNeighborUnchecked(this.TerrainWidth)) == height;
    }

    /// <summary>
    /// Resolves given slim ID to a full material proto with that ID.
    /// </summary>
    [Pure]
    public TerrainMaterialProto ResolveSlimMaterial(TerrainMaterialSlimId slimId)
    {
      return this.TerrainMaterials[(int) slimId.Value];
    }

    public IEnumerable<Tile2iAndIndex> EnumerateAllTiles()
    {
      return this.TerrainArea.EnumerateTilesAndIndices(this);
    }

    public IEnumerable<Tile2iAndIndex> EnumerateAllTilesSkipBoundaryTiles()
    {
      RectangleTerrainArea2i rectangleTerrainArea2i = this.TerrainArea;
      rectangleTerrainArea2i = rectangleTerrainArea2i.ExtendBy(-1);
      return rectangleTerrainArea2i.EnumerateTilesAndIndices(this);
    }

    public IEnumerable<Tile2iAndIndex> EnumerateAllTilesSkipOffLimits()
    {
      return new RectangleTerrainArea2i(this.MinOnLimits, this.MaxOnLimitsExcl - this.MinOnLimits).EnumerateTilesAndIndices(this);
    }

    /// <summary>
    /// Consider using for-loop form 0 to <see cref="P:Mafi.Core.Terrain.TerrainManager.TerrainTilesCount" /> (exclusive) instead.
    /// <code>
    /// for (int i = 0, max = terrain.TerrainTilesCount; i &lt; max; ++i) {
    /// 	Tile2iIndex tileIndex = new(i);
    /// 	...
    /// }
    /// </code>
    /// </summary>
    [Obsolete("Use for-loop instead")]
    public IEnumerable<Tile2iIndex> EnumerateAllTileIndices()
    {
      return this.TerrainArea.EnumerateTileIndices(this);
    }

    public IEnumerable<Tile2iIndex> EnumerateAllTileIndicesSkipBoundaryTiles()
    {
      RectangleTerrainArea2i rectangleTerrainArea2i = this.TerrainArea;
      rectangleTerrainArea2i = rectangleTerrainArea2i.ExtendBy(-1);
      return rectangleTerrainArea2i.EnumerateTileIndices(this);
    }

    public IEnumerable<Tile2iIndex> EnumerateAllTileIndicesSkipOffLimits()
    {
      return new RectangleTerrainArea2i(this.MinOnLimits, this.MaxOnLimitsExcl - this.MinOnLimits).EnumerateTileIndices(this);
    }

    /// <summary>
    /// Creates new tile flat reporter that can be used to report tile-based data as well as building or vehicle
    /// occupancy. Only <see cref="F:Mafi.Core.Terrain.TerrainTile.MAX_FLAGS_COUNT" /> reporters can exist in the game (incl reserved flags).
    /// Returned reporter should be preserved and saved by the calling class for its usage.
    /// </summary>
    public TileFlagReporter CreateNewTileFlagReporter(
      string name,
      bool blocksBuildings,
      bool blocksVehicles,
      bool isSaved)
    {
      ++this.m_tileFlagsCount;
      uint flagMask;
      if (this.m_tileFlagsCount > 16)
      {
        Mafi.Log.Error("Failed to create tile flag reporter '" + name + "', too many reporters already created: " + this.m_flagsReporters.AsEnumerable().Select<TileFlagReporter, string>((Func<TileFlagReporter, string>) (x => x.Name)).JoinStrings(", "));
        flagMask = 0U;
      }
      else
        flagMask = (uint) (1 << this.m_tileFlagsCount);
      TileFlagReporter tileFlagReporter = new TileFlagReporter(name, this, flagMask, blocksBuildings, blocksVehicles, isSaved);
      if (tileFlagReporter.BlocksBuildings)
      {
        this.BlocksBuildingsFlagsMask |= flagMask;
        this.BlocksBuildingsOrVehiclesMask |= flagMask;
      }
      if (tileFlagReporter.BlocksVehicles)
      {
        this.BlocksVehiclesFlagsMask |= flagMask;
        this.BlocksBuildingsOrVehiclesMask |= flagMask;
      }
      if (tileFlagReporter.IsSaved)
        this.m_data.SavedFlagsMask |= flagMask;
      this.m_flagsReporters.Add(tileFlagReporter);
      return tileFlagReporter;
    }

    public bool IsBlockingBuildings(Tile2iIndex index)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((uint) flags[index.Value] & this.BlocksBuildingsFlagsMask) > 0U;
      Mafi.Log.Error("IsBlockingBuildings: Tile index out of bounds");
      return true;
    }

    public bool IsBlockingVehicles(Tile2iIndex index)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((uint) flags[index.Value] & this.BlocksVehiclesFlagsMask) > 0U;
      Mafi.Log.Error("IsBlockingVehicles: Tile index out of bounds");
      return true;
    }

    public bool IsBlockingBuildingsOrVehicles(Tile2iIndex index)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((uint) flags[index.Value] & this.BlocksBuildingsOrVehiclesMask) > 0U;
      Mafi.Log.Error("IsBlockingBuildingsOrVehicles: Tile index out of bounds");
      return true;
    }

    public uint GetTileFlags(Tile2iIndex index)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return (uint) flags[index.Value];
      Mafi.Log.Error("GetTileFlags: Tile index out of bounds");
      return 0;
    }

    public bool IsOcean(Tile2iIndex index)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((uint) flags[index.Value] & 4U) > 0U;
      Mafi.Log.Error("IsOcean: Tile index out of bounds");
      return true;
    }

    /// <summary>For use when flags may not have been set up yet.</summary>
    public bool IsOnMapBoundary_Slow(Tile2iIndex index)
    {
      return this.IsOnMapBoundary_Slow(this.IndexToTile_Slow(index));
    }

    public bool IsOnMapBoundary_Slow(Tile2i tile)
    {
      return tile.X == 0 || tile.X >= this.TerrainWidth || tile.Y == 0 || tile.Y >= this.TerrainHeight;
    }

    public bool IsOnMapBoundary(Tile2iIndex index)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((uint) flags[index.Value] & 1U) > 0U;
      Mafi.Log.Error("IsOnMapBoundary: Tile index out of bounds");
      return true;
    }

    public bool IsOceanOrOnMapBoundary(Tile2iIndex index)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((uint) flags[index.Value] & 5U) > 0U;
      Mafi.Log.Error("IsOnMapBoundary: Tile index out of bounds");
      return true;
    }

    public bool IsOffLimits(Tile2iIndex index)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((uint) flags[index.Value] & 2U) > 0U;
      Mafi.Log.Error("IsOffLimits: Tile index out of bounds");
      return true;
    }

    public bool IsChanged(Tile2iIndex index) => this.m_data.ChangedTiles.IsSet(index.Value);

    public bool HasAnyTileFlagSet(Tile2iIndex index, uint mask)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((uint) flags[index.Value] & mask) > 0U;
      Mafi.Log.Error("HasAnyTileFlagSet: Tile index out of bounds");
      return false;
    }

    public bool HasAllTileFlagsSet(Tile2iIndex index, uint mask)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((int) flags[index.Value] & (int) mask) == (int) mask;
      Mafi.Log.Error("HasAllTileFlagsSet: Tile index out of bounds");
      return false;
    }

    public bool HasNoTileFlagsSet(Tile2iIndex index, uint mask)
    {
      ushort[] flags = this.m_data.Flags;
      if ((uint) index.Value < (uint) flags.Length)
        return ((int) flags[index.Value] & (int) mask) == 0;
      Mafi.Log.Error("HasNoTileFlagsSet: Tile index out of bounds");
      return true;
    }

    public void SetTileFlags(Tile2iAndIndex tileAndIndex, uint flags)
    {
      ushort[] flags1 = this.m_data.Flags;
      if ((uint) tileAndIndex.IndexRaw >= (uint) flags1.Length)
      {
        Mafi.Log.Error("SetTileFlags: Tile index out of bounds");
      }
      else
      {
        uint num1 = (uint) flags1[tileAndIndex.IndexRaw];
        uint num2 = num1 | flags;
        flags1[tileAndIndex.IndexRaw] = (ushort) num2;
        if ((int) num1 == (int) num2)
          return;
        uint num3 = num1 ^ num2;
        if (((int) num3 & (int) this.m_data.SavedFlagsMask) != 0)
          this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_tileFlagsChanged.Invoke(tileAndIndex, num3);
        if (((int) num3 & 4) == 0)
          return;
        this.m_oceanFlagChanged.Invoke(tileAndIndex);
      }
    }

    public void SetTileFlagsNoEvents(Tile2iIndex index, uint flags)
    {
      ushort[] flags1 = this.m_data.Flags;
      if ((uint) index.Value >= (uint) flags1.Length)
      {
        Mafi.Log.Error("SetTileFlagsNoEvents: Tile index out of bounds");
      }
      else
      {
        uint num1 = (uint) flags1[index.Value];
        uint num2 = num1 | flags;
        flags1[index.Value] = (ushort) num2;
        if ((((int) num1 ^ (int) num2) & (int) this.m_data.SavedFlagsMask) == 0)
          return;
        this.m_data.ChangedTiles.SetBit(index.Value);
      }
    }

    public bool SetTileFlagsNoEventsReportChanged(Tile2iIndex index, uint flags)
    {
      ushort[] flags1 = this.m_data.Flags;
      if ((uint) index.Value >= (uint) flags1.Length)
      {
        Mafi.Log.Error("SetTileFlagsNoEventsReportChanged: Tile index out of bounds");
        return false;
      }
      uint num1 = (uint) flags1[index.Value];
      uint num2 = num1 | flags;
      flags1[index.Value] = (ushort) num2;
      if ((((int) num1 ^ (int) num2) & (int) this.m_data.SavedFlagsMask) != 0)
        this.m_data.ChangedTiles.SetBit(index.Value);
      return (int) num1 != (int) num2;
    }

    public void ClearTileFlags(Tile2iAndIndex tileAndIndex, uint flags)
    {
      ushort[] flags1 = this.m_data.Flags;
      if ((uint) tileAndIndex.IndexRaw >= (uint) flags1.Length)
      {
        Mafi.Log.Error("ClearTileFlags: Tile index out of bounds");
      }
      else
      {
        uint num1 = (uint) flags1[tileAndIndex.IndexRaw];
        uint num2 = num1 & ~flags;
        flags1[tileAndIndex.IndexRaw] = (ushort) num2;
        if ((int) num1 == (int) num2)
          return;
        uint num3 = num1 ^ num2;
        if (((int) num3 & (int) this.m_data.SavedFlagsMask) != 0)
          this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_tileFlagsChanged.Invoke(tileAndIndex, num3);
        if (((int) num3 & 4) == 0)
          return;
        this.m_oceanFlagChanged.Invoke(tileAndIndex);
      }
    }

    public void ClearTileFlagsNoEvents(Tile2iIndex index, uint flags)
    {
      ushort[] flags1 = this.m_data.Flags;
      if ((uint) index.Value >= (uint) flags1.Length)
      {
        Mafi.Log.Error("ClearTileFlagsNoEvents: Tile index out of bounds");
      }
      else
      {
        uint num1 = (uint) flags1[index.Value];
        uint num2 = num1 & ~flags;
        flags1[index.Value] = (ushort) num2;
        if ((((int) num1 ^ (int) num2) & (int) this.m_data.SavedFlagsMask) == 0)
          return;
        this.m_data.ChangedTiles.SetBit(index.Value);
      }
    }

    public void ClearTileFlagsNoEventsNoChange(Tile2iIndex index, uint flags)
    {
      ushort[] flags1 = this.m_data.Flags;
      if ((uint) index.Value >= (uint) flags1.Length)
        Mafi.Log.Error("ClearTileFlagsNoEventsNoChange: Tile index out of bounds");
      else
        flags1[index.Value] &= (ushort) flags;
    }

    public bool ClearTileFlagsNoEventsReportChanged(Tile2iIndex index, uint flags)
    {
      ushort[] flags1 = this.m_data.Flags;
      if ((uint) index.Value >= (uint) flags1.Length)
      {
        Mafi.Log.Error("ClearTileFlagsNoEventsReportChanged: Tile index out of bounds");
        return false;
      }
      uint num1 = (uint) flags1[index.Value];
      uint num2 = num1 & ~flags;
      flags1[index.Value] = (ushort) num2;
      if ((((int) num1 ^ (int) num2) & (int) this.m_data.SavedFlagsMask) != 0)
        this.m_data.ChangedTiles.SetBit(index.Value);
      return (int) num1 != (int) num2;
    }

    internal string Debug_ExplainFlags(Tile2iIndex index)
    {
      StringBuilder sb = new StringBuilder(128);
      uint flags = (uint) this.m_data.Flags[index.Value];
      sb.AppendLine("0b" + Convert.ToString((long) flags, 2).PadLeft(16, '0') + " current flags (all)");
      sb.AppendLine("0b" + Convert.ToString((long) this.BlocksBuildingsFlagsMask, 2).PadLeft(16, '0') + " blocks buildings mask");
      sb.AppendLine("0b" + Convert.ToString((long) this.BlocksVehiclesFlagsMask, 2).PadLeft(16, '0') + " blocks vehicles mask");
      printFlag("Ocean", 4U);
      printFlag("On boundary", 1U);
      printFlag("Off limits", 2U);
      foreach (TileFlagReporter flagsReporter in this.m_flagsReporters)
        printFlag(flagsReporter.Name, flagsReporter.FlagMask);
      return sb.ToString();

      void printFlag(string name, uint mask)
      {
        sb.AppendLine(string.Format("0b{0} {1,-12}: {2}", (object) Convert.ToString((long) mask, 2).PadLeft(16, '0'), (object) name, (object) ((flags & mask) > 0U)));
      }
    }

    [Pure]
    public HeightTilesF GetHeight(Tile2iIndex index)
    {
      HeightTilesF[] heights = this.m_data.Heights;
      if ((uint) index.Value < (uint) heights.Length)
        return heights[index.Value];
      Mafi.Log.Error("GetHeight: Tile index out of bounds");
      return new HeightTilesF();
    }

    [Pure]
    public HeightTilesF GetHeight(Tile2i tile) => this.GetHeight(this.GetTileIndex(tile));

    [Pure]
    public HeightTilesF GetHeightOrOceanSurface(Tile2i tile)
    {
      Tile2iIndex tileIndex = this.GetTileIndex(tile);
      return !this.IsOcean(tileIndex) ? this.GetHeight(tileIndex) : HeightTilesF.One;
    }

    [Pure]
    public HeightTilesF GetHeightOrOceanSurface(Tile2iIndex index)
    {
      return !this.IsOcean(index) ? this.GetHeight(index) : HeightTilesF.One;
    }

    /// <summary>
    /// Returns interpolated absolute terrain height of given global position. Bi-linear interpolation is used to
    /// obtain height in between tile vertices.
    /// </summary>
    [OnlyForSaveCompatibility(null)]
    [Pure]
    public HeightTilesF GetHeight(Tile2f globalCoord)
    {
      int index = this.GetTileIndex(globalCoord.Tile2i).Value;
      HeightTilesF[] heights = this.m_data.Heights;
      if ((uint) index >= (uint) heights.Length)
      {
        Mafi.Log.Error("GetHeight: Tile index out of bounds");
        return new HeightTilesF();
      }
      if (((int) this.m_data.Flags[index] & 1) != 0)
        return heights[index];
      RelTile2f relTile2f = globalCoord.FractionalPartNonNegative();
      Percent percent = relTile2f.X.ToPercent();
      return heights[index].Lerp(heights[index + 1], percent).Lerp(heights[index + this.TerrainWidth].Lerp(heights[index + this.TerrainWidth + 1], percent), relTile2f.Y.ToPercent());
    }

    /// <summary>
    /// Returns terrain height for coordinates that are on the terrain, or default for off-terrain coords.
    /// </summary>
    public HeightTilesF GetHeightOrDefault(Tile2f coord)
    {
      return !this.IsValidCoord(coord) ? new HeightTilesF() : this.GetHeight(coord);
    }

    [Pure]
    public Tile3f ExtendHeight(Tile2i position)
    {
      return position.CornerTile2f.ExtendHeight(this.GetHeight(position));
    }

    [Pure]
    public Tile3f ExtendHeight(Tile2f position) => position.ExtendHeight(this.GetHeight(position));

    [Pure]
    public Tile3f ExtendHeightOrDefault(Tile2f position)
    {
      return position.ExtendHeight(this.GetHeightOrDefault(position));
    }

    /// <summary>
    /// Sets tile surface height either by increasing thickness of top material or by removing layers.
    /// This preserves absolute layer heights.
    /// </summary>
    public void SetHeight(Tile2iAndIndex tileAndIndex, HeightTilesF height)
    {
      HeightTilesF[] heights = this.m_data.Heights;
      if ((uint) tileAndIndex.IndexRaw >= (uint) heights.Length)
      {
        Mafi.Log.Error("SetHeight: Tile index out of bounds");
      }
      else
      {
        HeightTilesF heightTilesF = heights[tileAndIndex.IndexRaw];
        if (height == heightTilesF)
          return;
        heights[tileAndIndex.IndexRaw] = height;
        this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        ref TileMaterialLayers local = ref this.m_data.MaterialLayers[tileAndIndex.IndexRaw];
        if (local.Count <= 0)
        {
          this.m_heightChanged.Invoke(tileAndIndex);
        }
        else
        {
          ThicknessTilesF maxThickness = height - heightTilesF;
          if (maxThickness.IsPositive)
          {
            local.First += maxThickness;
            this.m_heightChanged.Invoke(tileAndIndex);
          }
          else
          {
            maxThickness = -maxThickness;
            while (maxThickness.IsPositive)
              maxThickness -= this.mineTopLayerRaw(ref local, maxThickness).Thickness;
            this.m_heightChanged.Invoke(tileAndIndex);
          }
        }
      }
    }

    /// <summary>
    /// Sets tile surface height but preserves relative thicknesses of all layers. This effectively moves all materials
    /// within the tiles. Perf note: This is a very efficient operation.
    /// </summary>
    public void SetHeightPreserveRelativeLayers(Tile2iAndIndex tileAndIndex, HeightTilesF height)
    {
      HeightTilesF[] heights = this.m_data.Heights;
      if ((uint) tileAndIndex.IndexRaw >= (uint) heights.Length)
      {
        Mafi.Log.Error("SetHeightPreserveRelativeLayers: Tile index out of bounds");
      }
      else
      {
        if (height == heights[tileAndIndex.IndexRaw])
          return;
        heights[tileAndIndex.IndexRaw] = height;
        this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_heightChanged.Invoke(tileAndIndex);
      }
    }

    /// <summary>
    /// Sets tile surface height but preserves relative thicknesses of all layers. This effectively moves all materials
    /// within the tiles. Disables physics on the modified tile. Perf note: This is a very efficient operation.
    /// </summary>
    public void SetHeightPreserveRelativeLayersNoPhysics(
      Tile2iAndIndex tileAndIndex,
      HeightTilesF height)
    {
      this.SetHeightPreserveRelativeLayers(tileAndIndex, height);
      this.StopTerrainPhysicsSimulationAt(tileAndIndex);
    }

    public int GetLayersCountNoBedrock(Tile2iIndex index)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) index.Value < (uint) materialLayers.Length)
        return materialLayers[index.Value].Count;
      Mafi.Log.Error("GetLayersCountNoBedrock: Tile index out of bounds");
      return 0;
    }

    public TileMaterialLayers GetLayersRawData(Tile2iIndex index)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) index.Value < (uint) materialLayers.Length)
        return materialLayers[index.Value];
      Mafi.Log.Error("GetLayersRawData: Tile index out of bounds");
      return new TileMaterialLayers();
    }

    /// <summary>
    /// Efficient and allocation-free enumeration of all terrain layers, including overflow and bedrock.
    /// </summary>
    public TerrainLayerEnumerator EnumerateLayers(Tile2iIndex index)
    {
      return new TerrainLayerEnumerator(this, index);
    }

    /// <summary>
    /// Returns material layer at the given index (including overflow and bedrock).
    /// This is a O(n) operation so prefer more efficient <see cref="M:Mafi.Core.Terrain.TerrainManager.EnumerateLayers(Mafi.Tile2iIndex)" /> is possible.
    /// </summary>
    public TerrainMaterialThicknessSlim GetLayerAt(Tile2iIndex tileIndex, int layerIndex)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileIndex.Value >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("TODO: Tile index out of bounds");
        return new TerrainMaterialThicknessSlim();
      }
      TileMaterialLayers tileMaterialLayers = materialLayers[tileIndex.Value];
      if (layerIndex >= tileMaterialLayers.Count)
        return new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, TerrainManager.BedrockLayerThicknessDefault);
      if (layerIndex < 4)
      {
        TerrainMaterialThicknessSlim layerAt;
        switch (layerIndex)
        {
          case 0:
            layerAt = tileMaterialLayers.First;
            break;
          case 1:
            layerAt = tileMaterialLayers.Second;
            break;
          case 2:
            layerAt = tileMaterialLayers.Third;
            break;
          default:
            layerAt = tileMaterialLayers.Fourth;
            break;
        }
        return layerAt;
      }
      TileMaterialLayerOverflow materialLayerOverflow = this.m_data.MaterialLayersOverflow[tileMaterialLayers.OverflowIndex];
      for (int index = 4; index < layerIndex; ++index)
        materialLayerOverflow = this.m_data.MaterialLayersOverflow[materialLayerOverflow.OverflowIndex];
      return materialLayerOverflow.Material;
    }

    public TileMaterialLayerOverflow GetLayerOverflowRawData(int overflowIndex)
    {
      if ((uint) overflowIndex < (uint) this.m_data.MaterialLayersOverflow.Count)
        return this.m_data.MaterialLayersOverflow[overflowIndex];
      Mafi.Log.Error("GetLayerOverflowRawData: Tile index out of bounds");
      return new TileMaterialLayerOverflow();
    }

    /// <summary>
    /// Returns top layer (as slim) but ignores bedrock (as it is not a real layer). This is very efficient.
    /// </summary>
    public TerrainMaterialThicknessSlim GetFirstLayerSlimOrNoneNoBedrock(Tile2iIndex index)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) index.Value < (uint) materialLayers.Length)
        return materialLayers[index.Value].First;
      Mafi.Log.Error("GetFirstLayerSlimOrNoneNoBedrock: Tile index out of bounds");
      return new TerrainMaterialThicknessSlim();
    }

    /// <summary>
    /// Returns top layer or bedrock (as slim). This is very efficient.
    /// </summary>
    public TerrainMaterialThicknessSlim GetFirstLayerSlim(Tile2iIndex index)
    {
      TerrainMaterialThicknessSlim slimOrNoneNoBedrock = this.GetFirstLayerSlimOrNoneNoBedrock(index);
      return !slimOrNoneNoBedrock.HasValue ? new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, TerrainManager.BedrockLayerThicknessDefault) : slimOrNoneNoBedrock;
    }

    /// <summary>Returns top layer or bedrock (if tile has no layers).</summary>
    public TerrainMaterialThickness GetFirstLayer(Tile2iIndex index)
    {
      TerrainMaterialThicknessSlim slimOrNoneNoBedrock = this.GetFirstLayerSlimOrNoneNoBedrock(index);
      return !slimOrNoneNoBedrock.HasValue ? new TerrainMaterialThickness(this.Bedrock, TerrainManager.BedrockLayerThicknessDefault) : slimOrNoneNoBedrock.ToFull(this);
    }

    /// <summary>
    /// Returns second layer (as slim) but ignores bedrock (as it is not a real layer). This is very efficient.
    /// </summary>
    public TerrainMaterialThicknessSlim GetSecondLayerSlimOrNoneNoBedrock(Tile2iIndex index)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) index.Value < (uint) materialLayers.Length)
        return materialLayers[index.Value].Second;
      Mafi.Log.Error("GetSecondLayerSlimOrNoneNoBedrock: Tile index out of bounds");
      return new TerrainMaterialThicknessSlim();
    }

    /// <summary>
    /// Returns second layer or bedrock (as slim). This is very efficient.
    /// </summary>
    public TerrainMaterialThicknessSlim GetSecondLayerSlim(Tile2iIndex index)
    {
      TerrainMaterialThicknessSlim slimOrNoneNoBedrock = this.GetSecondLayerSlimOrNoneNoBedrock(index);
      return !slimOrNoneNoBedrock.HasValue ? new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, TerrainManager.BedrockLayerThicknessDefault) : slimOrNoneNoBedrock;
    }

    /// <summary>
    /// Returns second layer or bedrock (if tile has less than 2 layers).
    /// </summary>
    public TerrainMaterialThickness GetSecondLayer(Tile2iIndex index)
    {
      TerrainMaterialThicknessSlim slimOrNoneNoBedrock = this.GetSecondLayerSlimOrNoneNoBedrock(index);
      return !slimOrNoneNoBedrock.HasValue ? new TerrainMaterialThickness(this.Bedrock, TerrainManager.BedrockLayerThicknessDefault) : slimOrNoneNoBedrock.ToFull(this);
    }

    /// <summary>
    /// Returns third layer (as slim) but ignores bedrock (as it is not a real layer). This is very efficient.
    /// </summary>
    public TerrainMaterialThicknessSlim GetThirdLayerSlimOrNoneNoBedrock(Tile2iIndex index)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) index.Value < (uint) materialLayers.Length)
        return materialLayers[index.Value].Third;
      Mafi.Log.Error("GetThirdLayerSlimOrNoneNoBedrock: Tile index out of bounds");
      return new TerrainMaterialThicknessSlim();
    }

    /// <summary>
    /// Returns third layer or bedrock (as slim). This is very efficient.
    /// </summary>
    public TerrainMaterialThicknessSlim GetThirdLayerSlim(Tile2iIndex index)
    {
      TerrainMaterialThicknessSlim slimOrNoneNoBedrock = this.GetThirdLayerSlimOrNoneNoBedrock(index);
      return !slimOrNoneNoBedrock.HasValue ? new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, TerrainManager.BedrockLayerThicknessDefault) : slimOrNoneNoBedrock;
    }

    /// <summary>
    /// Returns third layer or bedrock (if tile has less than 3 layers).
    /// </summary>
    public TerrainMaterialThickness GetThirdLayer(Tile2iIndex index)
    {
      TerrainMaterialThicknessSlim slimOrNoneNoBedrock = this.GetThirdLayerSlimOrNoneNoBedrock(index);
      return !slimOrNoneNoBedrock.HasValue ? new TerrainMaterialThickness(this.Bedrock, TerrainManager.BedrockLayerThicknessDefault) : slimOrNoneNoBedrock.ToFull(this);
    }

    /// <summary>
    /// Returns fourth layer (as slim) but ignores bedrock (as it is not a real layer). This is very efficient.
    /// </summary>
    public TerrainMaterialThicknessSlim GetFourthLayerSlimOrNoneNoBedrock(Tile2iIndex index)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) index.Value < (uint) materialLayers.Length)
        return materialLayers[index.Value].Fourth;
      Mafi.Log.Error("GetFourthLayerSlimOrNoneNoBedrock: Tile index out of bounds");
      return new TerrainMaterialThicknessSlim();
    }

    /// <summary>
    /// Returns fourth layer or bedrock (as slim). This is very efficient.
    /// </summary>
    public TerrainMaterialThicknessSlim GetFourthLayerSlim(Tile2iIndex index)
    {
      TerrainMaterialThicknessSlim slimOrNoneNoBedrock = this.GetFourthLayerSlimOrNoneNoBedrock(index);
      return !slimOrNoneNoBedrock.HasValue ? new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, TerrainManager.BedrockLayerThicknessDefault) : slimOrNoneNoBedrock;
    }

    /// <summary>
    /// Returns fourth layer or bedrock (if tile has less than 3 layers).
    /// </summary>
    public TerrainMaterialThickness GetFourthLayer(Tile2iIndex index)
    {
      TerrainMaterialThicknessSlim slimOrNoneNoBedrock = this.GetFourthLayerSlimOrNoneNoBedrock(index);
      return !slimOrNoneNoBedrock.HasValue ? new TerrainMaterialThickness(this.Bedrock, TerrainManager.BedrockLayerThicknessDefault) : slimOrNoneNoBedrock.ToFull(this);
    }

    internal string Debug_ExplainTileContents(Tile2i coord)
    {
      return this.Debug_ExplainTileContents(this.GetTileIndex(coord));
    }

    internal string Debug_ExplainTileContents(Tile2iIndex index)
    {
      StringBuilder stringBuilder = new StringBuilder(500);
      HeightTilesF height = this.GetHeight(index);
      stringBuilder.AppendLine(string.Format("Coord: {0}", (object) this.IndexToTile_Slow(index)));
      stringBuilder.AppendLine(string.Format("Height: {0}", (object) height));
      stringBuilder.AppendLine(string.Format("Layers: {0}", (object) this.GetLayersCountNoBedrock(index)));
      int num = 0;
      foreach (TerrainMaterialThicknessSlim enumerateLayer in this.EnumerateLayers(index))
        stringBuilder.AppendLine(string.Format("  #{0}: {1} of {2} (slim {3})", (object) num++, (object) enumerateLayer.Thickness, (object) enumerateLayer.SlimId.ToFull(this), (object) enumerateLayer.SlimId));
      ThicknessTilesF thicknessTilesF1 = ThicknessTilesF.One;
      ThicknessTilesF thicknessTilesF2 = -ThicknessTilesF.One;
      foreach (Tile2iAndIndexRel eightNeighborsDelta in this.EightNeighborsDeltas)
      {
        ThicknessTilesF thicknessTilesF3 = this.GetHeight(index + eightNeighborsDelta.IndexDelta) - height;
        if (thicknessTilesF3 < thicknessTilesF1)
          thicknessTilesF1 = thicknessTilesF3;
        if (thicknessTilesF3 > thicknessTilesF2)
          thicknessTilesF2 = thicknessTilesF3;
      }
      TerrainMaterialProto material = this.GetFirstLayer(index).Material;
      stringBuilder.AppendLine(string.Format("Diff to lower: {0}", (object) -thicknessTilesF1));
      stringBuilder.AppendLine(string.Format("Diff to higher: {0}", (object) thicknessTilesF2));
      stringBuilder.AppendLine(string.Format("Collapse range: {0} - {1}", (object) material.MinCollapseHeightDiff, (object) material.MaxCollapseHeightDiff));
      return stringBuilder.ToString();
    }

    public ThicknessTilesF GetThicknessOfMaterialInFirst4Layers(
      Tile2iIndex index,
      TerrainMaterialSlimId slimId)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) index.Value >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("GetThicknessOfMaterialInFirst4Layers: Tile index out of bounds");
        return new ThicknessTilesF();
      }
      TileMaterialLayers tileMaterialLayers = materialLayers[index.Value];
      return (tileMaterialLayers.First.SlimId == slimId ? tileMaterialLayers.First.Thickness : ThicknessTilesF.Zero) + (tileMaterialLayers.Second.SlimId == slimId ? tileMaterialLayers.Second.Thickness : ThicknessTilesF.Zero) + (tileMaterialLayers.Third.SlimId == slimId ? tileMaterialLayers.Third.Thickness : ThicknessTilesF.Zero) + (tileMaterialLayers.Fourth.SlimId == slimId ? tileMaterialLayers.Fourth.Thickness : ThicknessTilesF.Zero);
    }

    /// <summary>
    /// Disrupts terrain by replacing materials for their disrupted variants (see
    /// <see cref="P:Mafi.Core.Products.TerrainMaterialProto.DisruptedMaterialProto" />). Disruption amount is measured in thickness.
    /// This does consider <see cref="F:Mafi.Core.Products.TerrainMaterialProto.DisruptionSpeedMult" />.
    /// </summary>
    /// <remarks>
    /// For performance reason, maximum depth for investigation of disruption is <see cref="F:Mafi.Core.Terrain.TerrainManager.MAX_DISRUPTION_DEPTH" />
    /// and only the first four terrain layers are investigated.
    /// </remarks>
    public void Disrupt(Tile2iAndIndex tileAndIndex, ThicknessTilesF disruptionAmount)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("Disrupt: Tile index out of bounds");
      }
      else
      {
        ref TileMaterialLayers local = ref materialLayers[tileAndIndex.IndexRaw];
        disruptionAmount = disruptionAmount.ScaledBy(this.m_disruptedAmountMults[local.First.SlimIdRaw]);
        if (disruptionAmount.IsNotPositive || !(this.tryConvertMaterialOnTile(ref local, disruptionAmount, TerrainManager.MAX_DISRUPTION_DEPTH, this.DisruptedMaterialIds) < disruptionAmount))
          return;
        this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      }
    }

    /// <summary>
    /// Similar to <see cref="M:Mafi.Core.Terrain.TerrainManager.Disrupt(Mafi.Tile2iAndIndex,Mafi.ThicknessTilesF)" /> but does not consider <see cref="F:Mafi.Core.Products.TerrainMaterialProto.DisruptionSpeedMult" />.
    /// </summary>
    public ThicknessTilesF DisruptExactly(
      Tile2iAndIndex tileAndIndex,
      ThicknessTilesF disruptionAmount)
    {
      if (disruptionAmount.IsNotPositive)
        return ThicknessTilesF.Zero;
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("DisruptExactly: Tile index out of bounds");
        return disruptionAmount;
      }
      ThicknessTilesF thicknessTilesF = this.tryConvertMaterialOnTile(ref materialLayers[tileAndIndex.IndexRaw], disruptionAmount, TerrainManager.MAX_DISRUPTION_DEPTH, this.DisruptedMaterialIds);
      if (thicknessTilesF < disruptionAmount)
      {
        this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      }
      return thicknessTilesF;
    }

    public void DisruptTopLayer(Tile2iAndIndex tileAndIndex)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("DisruptTopLayer: Tile index out of bounds");
      }
      else
      {
        ref TileMaterialLayers local = ref materialLayers[tileAndIndex.IndexRaw];
        if (local.Count == 0)
          return;
        TerrainMaterialThicknessSlim first = local.First;
        TerrainMaterialSlimId disruptedMaterialId = this.DisruptedMaterialIds[first.SlimIdRaw];
        if (disruptedMaterialId.IsPhantom)
          return;
        this.convertMaterialInFirstLayer(ref local, disruptedMaterialId, first.Thickness, first.Thickness);
        this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      }
    }

    public ThicknessTilesF RecoverExactly(
      Tile2iAndIndex tileAndIndex,
      ThicknessTilesF recoveredAmount)
    {
      if (recoveredAmount.IsNotPositive)
        return ThicknessTilesF.Zero;
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("RecoverExactly: Tile index out of bounds");
        return recoveredAmount;
      }
      ThicknessTilesF thicknessTilesF = this.tryConvertMaterialOnTile(ref materialLayers[tileAndIndex.IndexRaw], recoveredAmount, TerrainManager.MAX_DISRUPTION_DEPTH, this.RecoveredMaterialIds);
      if (thicknessTilesF < recoveredAmount)
      {
        this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      }
      return thicknessTilesF;
    }

    public void StabilizeTerrainPhysics(int maxIterations = 10000)
    {
      for (int index = 0; index < maxIterations && this.m_physicsSimulator.IsProcessingTiles; ++index)
        this.m_physicsSimulator.Update();
    }

    public void StabilizeTerrainDisruption(int maxIterations = 10000)
    {
      for (int index = 0; index < maxIterations && this.m_terrainDisruptionSimulator.IsProcessingTiles; ++index)
        this.m_terrainDisruptionSimulator.Update();
    }

    /// <summary>
    /// Converts material in the first layer and returns remaining non-replaced thickness.
    /// </summary>
    public ThicknessTilesF ConvertMaterialInFirstLayer(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialSlimId convertedMaterial,
      ThicknessTilesF maxThickness,
      ThicknessTilesF maxThicknessToMerge)
    {
      if (maxThickness.IsNotPositive)
        return maxThickness;
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("ConvertMaterialInFirstLayer: Tile index out of bounds");
        return maxThickness;
      }
      ThicknessTilesF thicknessTilesF = this.convertMaterialInFirstLayer(ref materialLayers[tileAndIndex.IndexRaw], convertedMaterial, maxThickness, maxThicknessToMerge);
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      return thicknessTilesF;
    }

    public ThicknessTilesF ConvertMaterialInSecondLayer(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialSlimId convertedMaterial,
      ThicknessTilesF maxThickness,
      ThicknessTilesF maxThicknessToMerge)
    {
      if (maxThickness.IsNotPositive)
        return maxThickness;
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("ConvertMaterialInSecondLayer: Tile index out of bounds");
        return maxThickness;
      }
      ThicknessTilesF thicknessTilesF = this.convertMaterialInSecondLayer(ref materialLayers[tileAndIndex.IndexRaw], convertedMaterial, maxThickness, maxThicknessToMerge);
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      return thicknessTilesF;
    }

    public ThicknessTilesF ConvertMaterialInThirdLayer(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialSlimId convertedMaterial,
      ThicknessTilesF maxThickness,
      ThicknessTilesF maxThicknessToMerge)
    {
      if (maxThickness.IsNotPositive)
        return maxThickness;
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("ConvertMaterialInThirdLayer: Tile index out of bounds");
        return maxThickness;
      }
      ThicknessTilesF thicknessTilesF = this.convertMaterialInThirdLayer(ref materialLayers[tileAndIndex.IndexRaw], convertedMaterial, maxThickness, maxThicknessToMerge);
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      return thicknessTilesF;
    }

    public ThicknessTilesF ConvertMaterialInFourthLayer(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialSlimId convertedMaterial,
      ThicknessTilesF maxThickness)
    {
      if (maxThickness.IsNotPositive)
        return maxThickness;
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("ConvertMaterialInFourthLayer: Tile index out of bounds");
        return maxThickness;
      }
      ThicknessTilesF thicknessTilesF = this.convertMaterialInFourthLayer(ref materialLayers[tileAndIndex.IndexRaw], convertedMaterial, maxThickness);
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      return thicknessTilesF;
    }

    /// <summary>
    /// Removes the first layer from the tile (if there is any) but does not change tile height.
    /// </summary>
    public TerrainMaterialThicknessSlim RemoveFirstLayerNoHeightChange(Tile2iAndIndex tileAndIndex)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("ConvertMaterialInFourthLayer: Tile index out of bounds");
        return new TerrainMaterialThicknessSlim();
      }
      ref TileMaterialLayers local = ref materialLayers[tileAndIndex.IndexRaw];
      if (local.Count <= 0)
        return new TerrainMaterialThicknessSlim();
      TerrainMaterialThicknessSlim first = local.First;
      this.m_data.RemoveFirstLayer_noChecks(ref local);
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      return first;
    }

    /// <summary>
    /// Mines raw top product and reduces the surface height byt he mined amount.
    /// </summary>
    public TerrainMaterialThicknessSlim MineMaterial(
      Tile2iAndIndex tileAndIndex,
      ThicknessTilesF maxThickness)
    {
      HeightTilesF[] heights = this.m_data.Heights;
      if ((uint) tileAndIndex.IndexRaw >= (uint) heights.Length)
      {
        Mafi.Log.Error("MineMaterial: Tile index out of bounds");
        return new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, ThicknessTilesF.Zero);
      }
      TerrainMaterialThicknessSlim materialThicknessSlim = this.MineMaterial_RawNoEvents(tileAndIndex.Index, maxThickness);
      heights[tileAndIndex.IndexRaw] -= materialThicknessSlim.Thickness;
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_heightChanged.Invoke(tileAndIndex);
      return materialThicknessSlim;
    }

    /// <summary>
    /// Mines raw product from the second layer and reduces the surface height byt he mined amount.
    /// </summary>
    public TerrainMaterialThicknessSlim MineMaterialFromSecondLayer(
      Tile2iAndIndex tileAndIndex,
      ThicknessTilesF maxThickness)
    {
      HeightTilesF[] heights = this.m_data.Heights;
      if ((uint) tileAndIndex.IndexRaw >= (uint) heights.Length)
      {
        Mafi.Log.Error("MineMaterialFromSecondLayer: Tile index out of bounds");
        return new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, ThicknessTilesF.Zero);
      }
      TerrainMaterialThicknessSlim materialThicknessSlim = this.MineMaterialFromSecondLayer_RawNoEvents(tileAndIndex.Index, maxThickness);
      heights[tileAndIndex.IndexRaw] -= materialThicknessSlim.Thickness;
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_heightChanged.Invoke(tileAndIndex);
      return materialThicknessSlim;
    }

    public void DumpMaterial(Tile2iAndIndex tileAndIndex, TerrainMaterialThicknessSlim newLayer)
    {
      ThicknessTilesF thickness = newLayer.Thickness;
      if (thickness.IsNotPositive)
        return;
      TerrainMaterialSlimId slimId = newLayer.SlimId;
      if (slimId.IsPhantom)
      {
        Mafi.Log.Warning("Trying to dump phantom material.");
      }
      else
      {
        TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
        if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
        {
          Mafi.Log.Error("DumpMaterial: Tile index out of bounds");
        }
        else
        {
          ref TileMaterialLayers local = ref materialLayers[tileAndIndex.IndexRaw];
          if (local.Count <= 0)
          {
            if (!(slimId == this.Bedrock.SlimId))
              this.m_data.PushNewFirstLayer(ref local, newLayer);
          }
          else if (local.First.SlimId == slimId)
          {
            local.First += thickness;
          }
          else
          {
            this.m_data.PushNewFirstLayer(ref local, newLayer);
            if ((local.Count & 15) == 0)
              this.compressLayers(ref local);
          }
          this.m_data.Heights[tileAndIndex.IndexRaw] += thickness;
          this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
          this.m_heightChanged.Invoke(tileAndIndex);
        }
      }
    }

    /// <summary>
    /// Dumps the given material on the terrain up to the given height and returns unused thickness.
    /// </summary>
    public ThicknessTilesF DumpMaterialUpToHeight(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialThicknessSlim newLayer,
      HeightTilesF maxHeight)
    {
      HeightTilesF[] heights = this.m_data.Heights;
      if ((uint) tileAndIndex.IndexRaw >= (uint) heights.Length)
      {
        Mafi.Log.Error("DumpMaterialUpToHeight: Tile index out of bounds");
        return newLayer.Thickness;
      }
      ThicknessTilesF thicknessTilesF = maxHeight - heights[tileAndIndex.IndexRaw];
      if (thicknessTilesF.IsNotPositive)
        return newLayer.Thickness;
      ThicknessTilesF thickness = !(newLayer.Thickness > thicknessTilesF) ? newLayer.Thickness : thicknessTilesF;
      this.DumpMaterial(tileAndIndex, new TerrainMaterialThicknessSlim(newLayer.SlimId, thickness));
      return newLayer.Thickness - thickness;
    }

    /// <summary>
    /// Same as <see cref="M:Mafi.Core.Terrain.TerrainManager.DumpMaterial(Mafi.Tile2iAndIndex,Mafi.Core.Terrain.TerrainMaterialThicknessSlim)" /> but does not change the surface height.
    /// </summary>
    public void DumpMaterial_NoHeightChange(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialThicknessSlim newLayer)
    {
      ThicknessTilesF thickness = newLayer.Thickness;
      if (thickness.IsNotPositive)
        return;
      if (newLayer.SlimId.IsPhantom)
      {
        Mafi.Log.Warning("Trying to push phantom material.");
      }
      else
      {
        TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
        if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
        {
          Mafi.Log.Error("DumpMaterial_NoHeightChange: Tile index out of bounds");
        }
        else
        {
          ref TileMaterialLayers local = ref materialLayers[tileAndIndex.IndexRaw];
          if (local.Count == 0)
          {
            if (newLayer.SlimId == this.Bedrock.SlimId)
              return;
            local.First = newLayer;
            local.Count = 1;
          }
          else if (local.First.SlimId == newLayer.SlimId)
          {
            local.First += thickness;
          }
          else
          {
            this.m_data.PushNewFirstLayer(ref local, newLayer);
            if ((local.Count & 15) == 0)
              this.compressLayers(ref local);
          }
          this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
          this.m_tileMaterialsChanged.Invoke(tileAndIndex);
        }
      }
    }

    /// <summary>
    /// Pushes the given material to the second layer but does not change terrain height.
    /// </summary>
    public void DumpMaterialToSecondLayer_NoHeightChange(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialThicknessSlim newLayer)
    {
      ThicknessTilesF thickness = newLayer.Thickness;
      if (thickness.IsNotPositive)
        return;
      if (newLayer.SlimId.IsPhantom)
      {
        Mafi.Log.Warning("Trying to push phantom material to the second layer.");
      }
      else
      {
        TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
        if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
        {
          Mafi.Log.Error("DumpMaterial_NoHeightChange: Tile index out of bounds");
        }
        else
        {
          ref TileMaterialLayers local = ref materialLayers[tileAndIndex.IndexRaw];
          if (local.Count == 0)
          {
            if (newLayer.SlimId == this.Bedrock.SlimId)
              return;
            local.First = newLayer;
            local.Count = 1;
          }
          else if (local.First.SlimId == newLayer.SlimId)
            local.First += thickness;
          else if (local.Count == 1)
          {
            if (newLayer.SlimId == this.Bedrock.SlimId)
              return;
            local.Second = newLayer;
            local.Count = 2;
          }
          else if (local.Second.SlimId == newLayer.SlimId)
          {
            local.Second += thickness;
          }
          else
          {
            this.m_data.PushNewSecondLayer(ref local, newLayer);
            if ((local.Count & 15) == 0)
              this.compressLayers(ref local);
          }
          this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
          this.m_tileMaterialsChanged.Invoke(tileAndIndex);
        }
      }
    }

    /// <summary>Tries to add material to any of the 2nd-4th layers.</summary>
    public bool TryAddMaterialToUndergroundTopFourLayer_NoHeightChange(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialThicknessSlim toAdd)
    {
      if (toAdd.Thickness.IsNotPositive)
        return false;
      if (toAdd.SlimId.IsPhantom)
      {
        Mafi.Log.Warning("Trying to add phantom material.");
        return false;
      }
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("TryAddMaterialToTopFourLayer_NoHeightChange: Tile index out of bounds");
        return false;
      }
      ref TileMaterialLayers local = ref materialLayers[tileAndIndex.IndexRaw];
      if (local.Count < 2)
        return false;
      int slimIdRaw = toAdd.SlimIdRaw;
      if (local.Second.SlimIdRaw == slimIdRaw)
        local.Second += toAdd.Thickness;
      else if (local.Third.SlimIdRaw == slimIdRaw)
      {
        local.Third += toAdd.Thickness;
      }
      else
      {
        if (local.Fourth.SlimIdRaw != slimIdRaw)
          return false;
        local.Fourth += toAdd.Thickness;
      }
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      return true;
    }

    /// <summary>
    /// Finds the first layer of the given material and removes thickness up to the given amount. Returns the amount
    /// of thickness removed. Does NOT change surface height.
    /// </summary>
    public ThicknessTilesF FindAndRemoveFirstLayer(
      Tile2iAndIndex tileAndIndex,
      TerrainMaterialSlimId materialSlimId,
      ThicknessTilesF maxRemovedThickness)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileAndIndex.IndexRaw >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("FindAndRemoveFirstLayer: Tile index out of bounds");
        return ThicknessTilesF.Zero;
      }
      ThicknessTilesF removeFirstLayerOf = this.findAndRemoveFirstLayerOf(ref materialLayers[tileAndIndex.IndexRaw], materialSlimId, maxRemovedThickness);
      if (removeFirstLayerOf.IsNotPositive)
        return ThicknessTilesF.Zero;
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_tileMaterialsChanged.Invoke(tileAndIndex);
      return removeFirstLayerOf;
    }

    internal void CompressLayers(Tile2iIndex tileIndex)
    {
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileIndex.Value >= (uint) materialLayers.Length)
        Mafi.Log.Error("CompressLayers: Tile index out of bounds");
      else
        this.compressLayers(ref materialLayers[tileIndex.Value]);
    }

    /// <summary>
    /// Raises <see cref="P:Mafi.Core.Terrain.TerrainManager.HeightChanged" /> event and sets the changed flag. Use this after "raw" operations.
    /// </summary>
    public void NotifyTileHeightLayersChanged(Tile2iAndIndex tileAndIndex)
    {
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_heightChanged.Invoke(tileAndIndex);
    }

    /// <summary>
    /// Raises <see cref="P:Mafi.Core.Terrain.TerrainManager.TileMaterialsOnlyChanged" /> event and sets the changed flag. Use this after "raw" operations.
    /// </summary>
    public void NotifyTileMaterialsOnlyChanged(Tile2iAndIndex tileAndIndex)
    {
      this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
      this.m_tileMaterialsChanged.Invoke(tileAndIndex);
    }

    /// <summary>
    /// Mines the first layer but does not change tile height, invokes no events, and does not set the changed flag.
    /// Don't forget to call <see cref="M:Mafi.Core.Terrain.TerrainManager.NotifyTileMaterialsOnlyChanged(Mafi.Tile2iAndIndex)" /> or call other non-raw operation that invokes
    /// needed events.
    /// </summary>
    public TerrainMaterialThicknessSlim MineMaterial_RawNoEvents(
      Tile2iIndex tileIndex,
      ThicknessTilesF maxThickness)
    {
      if (maxThickness.IsNotPositive)
        return new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, ThicknessTilesF.Zero);
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileIndex.Value < (uint) materialLayers.Length)
        return this.mineTopLayerRaw(ref materialLayers[tileIndex.Value], maxThickness);
      Mafi.Log.Error("MineMaterial_RawNoEvents: Tile index out of bounds");
      return new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, ThicknessTilesF.Zero);
    }

    /// <summary>
    /// Mines the second layer but does not change tile height, invokes no events, and does not set the changed flag.
    /// Don't forget to call <see cref="M:Mafi.Core.Terrain.TerrainManager.NotifyTileMaterialsOnlyChanged(Mafi.Tile2iAndIndex)" /> or call other non-raw operation that invokes
    /// needed events.
    /// </summary>
    public TerrainMaterialThicknessSlim MineMaterialFromSecondLayer_RawNoEvents(
      Tile2iIndex tileIndex,
      ThicknessTilesF maxThickness)
    {
      if (maxThickness.IsNotPositive)
        return new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, ThicknessTilesF.Zero);
      TileMaterialLayers[] materialLayers = this.m_data.MaterialLayers;
      if ((uint) tileIndex.Value >= (uint) materialLayers.Length)
      {
        Mafi.Log.Error("MineMaterialFromSecondLayer_RawNoEvents: Tile index out of bounds");
        return new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, ThicknessTilesF.Zero);
      }
      ref TileMaterialLayers local = ref materialLayers[tileIndex.Value];
      if (local.Count < 2)
        return new TerrainMaterialThicknessSlim(this.Bedrock, maxThickness);
      TerrainMaterialThicknessSlim materialThicknessSlim;
      if (local.Second.Thickness <= maxThickness)
      {
        materialThicknessSlim = new TerrainMaterialThicknessSlim(local.Second.SlimId, local.Second.Thickness);
        this.m_data.RemoveSecondLayer_noChecks(ref local);
      }
      else
      {
        materialThicknessSlim = new TerrainMaterialThicknessSlim(local.Second.SlimId, maxThickness);
        local.Second -= maxThickness;
      }
      return materialThicknessSlim;
    }

    [Pure]
    public ReadOnlyArray<TileSurfaceData> GetRawSurfacesData()
    {
      return this.m_data.Surfaces.AsReadOnlyArray<TileSurfaceData>();
    }

    [Pure]
    public TerrainTileSurfaceProto ResolveSlimSurface(TileSurfaceSlimId slimId)
    {
      return this.TerrainSurfaces[(int) slimId.Value];
    }

    [Pure]
    public bool HasTileSurface(Tile2iIndex tileIndex)
    {
      TileSurfaceData[] surfaces = this.m_data.Surfaces;
      if ((uint) tileIndex.Value < (uint) surfaces.Length)
        return surfaces[tileIndex.Value].SurfaceSlimId.IsNotPhantom;
      Mafi.Log.Error("HasTileSurface: Tile index out of bounds");
      return false;
    }

    [Pure]
    public TileSurfaceData GetTileSurface(Tile2iIndex tileIndex)
    {
      TileSurfaceData[] surfaces = this.m_data.Surfaces;
      if ((uint) tileIndex.Value < (uint) surfaces.Length)
        return surfaces[tileIndex.Value];
      Mafi.Log.Error("GetTileSurface: Tile index out of bounds");
      return new TileSurfaceData();
    }

    [Pure]
    public bool TryGetTileSurface(Tile2iIndex tileIndex, out TileSurfaceData tileSurfaceData)
    {
      TileSurfaceData[] surfaces = this.m_data.Surfaces;
      if ((uint) tileIndex.Value >= (uint) surfaces.Length)
      {
        Mafi.Log.Error("GetTileSurface: Tile index out of bounds");
        tileSurfaceData = new TileSurfaceData();
        return false;
      }
      TileSurfaceData tileSurfaceData1 = surfaces[tileIndex.Value];
      if (tileSurfaceData1.IsNotValid)
      {
        tileSurfaceData = new TileSurfaceData();
        return false;
      }
      tileSurfaceData = tileSurfaceData1;
      return true;
    }

    public void SetCustomSurface(Tile2iAndIndex tileAndIndex, TileSurfaceData surfData)
    {
      TileSurfaceData[] surfaces = this.m_data.Surfaces;
      if ((uint) tileAndIndex.IndexRaw >= (uint) surfaces.Length)
      {
        Mafi.Log.Error("SetCustomSurface: Tile index out of bounds");
      }
      else
      {
        surfaces[tileAndIndex.IndexRaw] = surfData;
        this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_tileCustomSurfaceChanged.Invoke(tileAndIndex);
      }
    }

    public void ClearCustomSurface(Tile2iAndIndex tileAndIndex)
    {
      TileSurfaceData[] surfaces = this.m_data.Surfaces;
      if ((uint) tileAndIndex.IndexRaw >= (uint) surfaces.Length)
      {
        Mafi.Log.Error("ClearCustomSurface: Tile index out of bounds");
      }
      else
      {
        surfaces[tileAndIndex.IndexRaw] = new TileSurfaceData();
        this.m_data.ChangedTiles.SetBit(tileAndIndex.IndexRaw);
        this.m_tileCustomSurfaceChanged.Invoke(tileAndIndex);
      }
    }

    [MustUseReturnValue]
    public string ValidateAndFixAllTilesDataAndReturnError()
    {
      Lyst<string> errors = TerrainManager.s_errorsTmp ?? (TerrainManager.s_errorsTmp = new Lyst<string>());
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < this.m_data.MaterialLayers.Length; ++index)
      {
        errors.Clear();
        this.m_data.ValidateAndFixData(new Tile2iIndex(index), this.Bedrock.SlimId, errors);
        if (errors.IsNotEmpty)
        {
          foreach (string str in errors)
            stringBuilder.AppendLine(string.Format("{0}: {1}", (object) this.ExtendTileCoord_Slow(new Tile2iIndex(index)), (object) str));
        }
      }
      return stringBuilder.ToString();
    }

    [Conditional("VALIDATE_DATA_AFTER_EACH_CHANGE")]
    private void validateTileDataOrThrow_explicitOnly(Tile2iIndex index)
    {
      string errorsStr;
      if (!this.ValidateAndFixTileData(index, out errorsStr))
        throw new Exception(errorsStr);
    }

    public bool ValidateAndFixTileData(Tile2iIndex index, out string errorsStr)
    {
      Lyst<string> lyst = TerrainManager.s_errorsTmp ?? (TerrainManager.s_errorsTmp = new Lyst<string>());
      lyst.Clear();
      this.m_data.ValidateAndFixData(index, this.Bedrock.SlimId, lyst);
      if (lyst.IsNotEmpty)
      {
        errorsStr = lyst.JoinStrings("\n");
        lyst.Clear();
        return false;
      }
      errorsStr = "";
      return true;
    }

    public void ValidateAndFixTileData(Tile2iIndex index, Lyst<string> errors)
    {
      this.m_data.ValidateAndFixData(index, this.Bedrock.SlimId, errors);
    }

    public void ResetIsChangedFlag_TestOnly() => this.m_data.ChangedTiles.ClearAllBits();

    /// <summary>
    /// Mines top layer, always returns positive amount of material. Does not change tile height nor raises any events.
    /// </summary>
    private TerrainMaterialThicknessSlim mineTopLayerRaw(
      ref TileMaterialLayers layersRef,
      ThicknessTilesF maxThickness)
    {
      if (layersRef.Count <= 0)
        return new TerrainMaterialThicknessSlim(this.Bedrock.SlimId, maxThickness);
      if (layersRef.First.Thickness > maxThickness)
      {
        layersRef.First -= maxThickness;
        return new TerrainMaterialThicknessSlim(layersRef.First.SlimId, maxThickness);
      }
      TerrainMaterialThicknessSlim first = layersRef.First;
      this.m_data.RemoveFirstLayer_noChecks(ref layersRef);
      return first;
    }

    /// <summary>
    /// Converts the all convertible material based on the given <paramref name="conversionTable" />
    /// up to the <paramref name="thicknessToConvert" />.
    /// </summary>
    private ThicknessTilesF tryConvertMaterialOnTile(
      ref TileMaterialLayers layersRef,
      ThicknessTilesF thicknessToConvert,
      ThicknessTilesF thicknessToExplore,
      ImmutableArray<TerrainMaterialSlimId> conversionTable)
    {
      if (layersRef.Count <= 0)
      {
        TerrainMaterialSlimId slimId = conversionTable[(int) this.Bedrock.SlimId.Value];
        if (slimId.IsNotPhantom)
        {
          layersRef.First = new TerrainMaterialThicknessSlim(slimId, thicknessToConvert);
          layersRef.Count = 1;
        }
        return ThicknessTilesF.Zero;
      }
      TerrainMaterialThicknessSlim first = layersRef.First;
      TerrainMaterialSlimId convertedMaterial1 = conversionTable[first.SlimIdRaw];
      if (convertedMaterial1.IsNotPhantom)
      {
        thicknessToConvert = this.convertMaterialInFirstLayer(ref layersRef, convertedMaterial1, thicknessToConvert, thicknessToExplore);
        if (thicknessToConvert.IsNotPositive)
          return ThicknessTilesF.Zero;
      }
      thicknessToExplore -= first.Thickness;
      if (thicknessToExplore.IsNotPositive)
        return thicknessToConvert;
      if (layersRef.Count == 1)
      {
        TerrainMaterialSlimId slimId = conversionTable[(int) this.Bedrock.SlimId.Value];
        if (slimId.IsNotPhantom)
        {
          if (layersRef.First.SlimId == slimId)
          {
            layersRef.First += thicknessToConvert;
          }
          else
          {
            layersRef.Second = new TerrainMaterialThicknessSlim(slimId, thicknessToConvert);
            layersRef.Count = 2;
          }
        }
        return ThicknessTilesF.Zero;
      }
      TerrainMaterialThicknessSlim second = layersRef.Second;
      TerrainMaterialSlimId convertedMaterial2 = conversionTable[second.SlimIdRaw];
      if (convertedMaterial2.IsNotPhantom)
      {
        thicknessToConvert = this.convertMaterialInSecondLayer(ref layersRef, convertedMaterial2, thicknessToConvert, thicknessToExplore);
        if (thicknessToConvert.IsNotPositive)
          return ThicknessTilesF.Zero;
      }
      thicknessToExplore -= second.Thickness;
      if (thicknessToExplore.IsNotPositive)
        return thicknessToConvert;
      if (layersRef.Count == 2)
      {
        TerrainMaterialSlimId slimId = conversionTable[(int) this.Bedrock.SlimId.Value];
        if (slimId.IsNotPhantom)
        {
          if (layersRef.Second.SlimId == slimId)
          {
            layersRef.Second += thicknessToConvert;
          }
          else
          {
            layersRef.Third = new TerrainMaterialThicknessSlim(slimId, thicknessToConvert);
            layersRef.Count = 3;
          }
        }
        return ThicknessTilesF.Zero;
      }
      TerrainMaterialThicknessSlim third = layersRef.Third;
      TerrainMaterialSlimId convertedMaterial3 = conversionTable[third.SlimIdRaw];
      if (convertedMaterial3.IsNotPhantom)
      {
        thicknessToConvert = this.convertMaterialInThirdLayer(ref layersRef, convertedMaterial3, thicknessToConvert, thicknessToExplore);
        if (thicknessToConvert.IsNotPositive)
          return ThicknessTilesF.Zero;
      }
      thicknessToExplore -= third.Thickness;
      if (thicknessToExplore.IsNotPositive)
        return thicknessToConvert;
      if (layersRef.Count == 3)
      {
        TerrainMaterialSlimId slimId = conversionTable[(int) this.Bedrock.SlimId.Value];
        if (slimId.IsNotPhantom)
        {
          if (layersRef.Third.SlimId == slimId)
          {
            layersRef.Third += thicknessToConvert;
          }
          else
          {
            layersRef.Fourth = new TerrainMaterialThicknessSlim(slimId, thicknessToConvert);
            layersRef.Count = 4;
          }
        }
        return ThicknessTilesF.Zero;
      }
      TerrainMaterialThicknessSlim fourth = layersRef.Fourth;
      TerrainMaterialSlimId convertedMaterial4 = conversionTable[fourth.SlimIdRaw];
      if (convertedMaterial4.IsNotPhantom)
        thicknessToConvert = this.convertMaterialInFourthLayer(ref layersRef, convertedMaterial4, thicknessToConvert);
      return thicknessToConvert;
    }

    private ThicknessTilesF convertAllLayersOnTile(
      ref TileMaterialLayers layersRef,
      ImmutableArray<TerrainMaterialSlimId> conversionTable)
    {
      if (layersRef.Count <= 0)
        return ThicknessTilesF.Zero;
      ThicknessTilesF convertedThickness = ThicknessTilesF.Zero;
      if (layersRef.Count <= 4)
      {
        convertLayer(ref layersRef.First);
        if (layersRef.Count >= 2)
        {
          convertLayer(ref layersRef.Second);
          if (layersRef.Count >= 3)
          {
            convertLayer(ref layersRef.Third);
            if (layersRef.Count == 4)
              convertLayer(ref layersRef.Fourth);
          }
        }
        return convertedThickness;
      }
      convertLayer(ref layersRef.First);
      convertLayer(ref layersRef.Second);
      convertLayer(ref layersRef.Third);
      convertLayer(ref layersRef.Fourth);
      int overflowIndex = layersRef.OverflowIndex;
      TileMaterialLayerOverflow[] backingArray = this.m_data.MaterialLayersOverflow.GetBackingArray();
      for (int index = 4; index < layersRef.Count; ++index)
      {
        ref TileMaterialLayerOverflow local = ref backingArray[overflowIndex];
        convertLayer(ref local.Material);
        overflowIndex = local.OverflowIndex;
      }
      return convertedThickness;

      void convertLayer(ref TerrainMaterialThicknessSlim layerRef)
      {
        TerrainMaterialSlimId newId = conversionTable[layerRef.SlimIdRaw];
        if (!newId.IsNotPhantom)
          return;
        layerRef = layerRef.WithNewId(newId);
        convertedThickness += layerRef.Thickness;
      }
    }

    /// <summary>
    /// Converts material in the first layer and returns amount that was not converted.
    /// </summary>
    private ThicknessTilesF convertMaterialInFirstLayer(
      ref TileMaterialLayers layersRef,
      TerrainMaterialSlimId convertedMaterial,
      ThicknessTilesF maxThickness,
      ThicknessTilesF maxThicknessToMerge)
    {
      ThicknessTilesF thickness = layersRef.First.Thickness;
      if (thickness > maxThickness)
      {
        layersRef.First -= maxThickness;
        if (layersRef.Second.SlimId == convertedMaterial && thickness <= maxThicknessToMerge)
          layersRef.Second += maxThickness;
        else
          this.m_data.PushNewFirstLayer(ref layersRef, new TerrainMaterialThicknessSlim(convertedMaterial, maxThickness));
        return ThicknessTilesF.Zero;
      }
      if (layersRef.Second.SlimId == convertedMaterial)
      {
        layersRef.Second += thickness;
        this.m_data.RemoveFirstLayer_noChecks(ref layersRef);
      }
      else
        layersRef.First = new TerrainMaterialThicknessSlim(convertedMaterial, thickness);
      return maxThickness - thickness;
    }

    private ThicknessTilesF convertMaterialInSecondLayer(
      ref TileMaterialLayers layersRef,
      TerrainMaterialSlimId convertedMaterial,
      ThicknessTilesF maxThickness,
      ThicknessTilesF maxThicknessToExplore)
    {
      ThicknessTilesF thickness = layersRef.Second.Thickness;
      if (thickness > maxThickness)
      {
        layersRef.Second -= maxThickness;
        if (layersRef.First.SlimId == convertedMaterial)
          layersRef.First += maxThickness;
        else if (layersRef.Third.SlimId == convertedMaterial && thickness <= maxThicknessToExplore)
          layersRef.Third += maxThickness;
        else
          this.m_data.PushNewSecondLayer(ref layersRef, new TerrainMaterialThicknessSlim(convertedMaterial, maxThickness));
        return ThicknessTilesF.Zero;
      }
      if (layersRef.First.SlimId == convertedMaterial)
      {
        layersRef.First += thickness;
        this.m_data.RemoveSecondLayer_noChecks(ref layersRef);
        if (layersRef.Second.SlimId == layersRef.First.SlimId)
        {
          layersRef.First += layersRef.Second.Thickness;
          this.m_data.RemoveSecondLayer_noChecks(ref layersRef);
        }
      }
      else if (layersRef.Third.SlimId == convertedMaterial)
      {
        layersRef.Third += thickness;
        this.m_data.RemoveSecondLayer_noChecks(ref layersRef);
      }
      else
        layersRef.Second = new TerrainMaterialThicknessSlim(convertedMaterial, thickness);
      return maxThickness - thickness;
    }

    private ThicknessTilesF convertMaterialInThirdLayer(
      ref TileMaterialLayers layersRef,
      TerrainMaterialSlimId convertedMaterial,
      ThicknessTilesF maxThickness,
      ThicknessTilesF maxThicknessToExplore)
    {
      ThicknessTilesF thickness = layersRef.Third.Thickness;
      if (thickness > maxThickness)
      {
        layersRef.Third -= maxThickness;
        if (layersRef.Second.SlimId == convertedMaterial)
          layersRef.Second += maxThickness;
        else if (layersRef.Fourth.SlimId == convertedMaterial && thickness <= maxThicknessToExplore)
          layersRef.Fourth += maxThickness;
        else
          this.m_data.PushNewThirdLayer(ref layersRef, new TerrainMaterialThicknessSlim(convertedMaterial, maxThickness));
        return ThicknessTilesF.Zero;
      }
      if (layersRef.Second.SlimId == convertedMaterial)
      {
        layersRef.Second += thickness;
        this.m_data.RemoveThirdLayer_noChecks(ref layersRef);
        if (layersRef.Third.SlimId == layersRef.Second.SlimId)
        {
          layersRef.Second += layersRef.Third.Thickness;
          this.m_data.RemoveThirdLayer_noChecks(ref layersRef);
        }
      }
      else if (layersRef.Fourth.SlimId == convertedMaterial)
      {
        layersRef.Fourth += thickness;
        this.m_data.RemoveThirdLayer_noChecks(ref layersRef);
      }
      else
        layersRef.Third = new TerrainMaterialThicknessSlim(convertedMaterial, thickness);
      return maxThickness - thickness;
    }

    private ThicknessTilesF convertMaterialInFourthLayer(
      ref TileMaterialLayers layersRef,
      TerrainMaterialSlimId convertedMaterial,
      ThicknessTilesF maxThickness)
    {
      ThicknessTilesF thickness = layersRef.Fourth.Thickness;
      if (thickness > maxThickness)
      {
        layersRef.Fourth -= maxThickness;
        if (layersRef.Third.SlimId == convertedMaterial)
          layersRef.Third += maxThickness;
        else
          this.m_data.PushNewFourthLayer(ref layersRef, new TerrainMaterialThicknessSlim(convertedMaterial, maxThickness));
        return ThicknessTilesF.Zero;
      }
      if (layersRef.Third.SlimId == convertedMaterial)
      {
        layersRef.Third += thickness;
        this.m_data.RemoveFourthLayer_noChecks(ref layersRef);
      }
      else if (layersRef.Count >= 5 && this.m_data.MaterialLayersOverflow[layersRef.OverflowIndex].Material.SlimId == convertedMaterial)
      {
        this.m_data.MaterialLayersOverflow.GetBackingArray()[layersRef.OverflowIndex].Material += thickness;
        this.m_data.RemoveFourthLayer_noChecks(ref layersRef);
      }
      else
        layersRef.Fourth = new TerrainMaterialThicknessSlim(convertedMaterial, thickness);
      return maxThickness - thickness;
    }

    /// <summary>Returns removed amount.</summary>
    private ThicknessTilesF findAndRemoveFirstLayerOf(
      ref TileMaterialLayers layersRef,
      TerrainMaterialSlimId materialSlimId,
      ThicknessTilesF maxRemovedThickness)
    {
      if (layersRef.First.SlimId == materialSlimId)
      {
        if (layersRef.First.Thickness > maxRemovedThickness)
        {
          layersRef.First -= maxRemovedThickness;
          return maxRemovedThickness;
        }
        ThicknessTilesF thickness = layersRef.First.Thickness;
        this.m_data.RemoveFirstLayer_noChecks(ref layersRef);
        return thickness;
      }
      if (layersRef.Second.SlimId == materialSlimId)
      {
        if (layersRef.Second.Thickness > maxRemovedThickness)
        {
          layersRef.Second -= maxRemovedThickness;
          return maxRemovedThickness;
        }
        ThicknessTilesF thickness = layersRef.Second.Thickness;
        this.m_data.RemoveSecondLayer_noChecks(ref layersRef);
        if (layersRef.Count >= 2 && layersRef.First.SlimId == layersRef.Second.SlimId)
        {
          layersRef.First += layersRef.Second.Thickness;
          this.m_data.RemoveSecondLayer_noChecks(ref layersRef);
        }
        return thickness;
      }
      if (layersRef.Third.SlimId == materialSlimId)
      {
        if (layersRef.Third.Thickness > maxRemovedThickness)
        {
          layersRef.Third -= maxRemovedThickness;
          return maxRemovedThickness;
        }
        ThicknessTilesF thickness = layersRef.Third.Thickness;
        this.m_data.RemoveThirdLayer_noChecks(ref layersRef);
        if (layersRef.Count >= 3 && layersRef.Second.SlimId == layersRef.Third.SlimId)
        {
          layersRef.Second += layersRef.Third.Thickness;
          this.m_data.RemoveThirdLayer_noChecks(ref layersRef);
        }
        return thickness;
      }
      if (layersRef.Fourth.SlimId == materialSlimId)
      {
        if (layersRef.Fourth.Thickness > maxRemovedThickness)
        {
          layersRef.Fourth -= maxRemovedThickness;
          return maxRemovedThickness;
        }
        ThicknessTilesF thickness = layersRef.Fourth.Thickness;
        this.m_data.RemoveFourthLayer_noChecks(ref layersRef);
        if (layersRef.Count >= 4 && layersRef.Third.SlimId == layersRef.Fourth.SlimId)
        {
          layersRef.Third += layersRef.Fourth.Thickness;
          this.m_data.RemoveFourthLayer_noChecks(ref layersRef);
        }
        return thickness;
      }
      if (layersRef.Count <= 4)
        return ThicknessTilesF.Zero;
      int overflowIndex = layersRef.OverflowIndex;
      int index1 = -1;
      TileMaterialLayerOverflow[] backingArray = this.m_data.MaterialLayersOverflow.GetBackingArray();
      for (int index2 = 4; index2 < layersRef.Count; ++index2)
      {
        ref TileMaterialLayerOverflow local = ref backingArray[overflowIndex];
        if (local.Material.SlimId == materialSlimId)
        {
          if (local.Material.Thickness > maxRemovedThickness)
          {
            local.Material -= maxRemovedThickness;
            return maxRemovedThickness;
          }
          ThicknessTilesF thickness = local.Material.Thickness;
          if (index1 < 0)
            this.m_data.RemoveFirstLayerFromOverflow_noChecks(ref layersRef);
          else
            this.m_data.RemoveMiddleLayerFromOverflow_noChecks(ref backingArray[index1]);
          --layersRef.Count;
          return thickness;
        }
        index1 = overflowIndex;
        overflowIndex = local.OverflowIndex;
      }
      return ThicknessTilesF.Zero;
    }

    private void compressLayers(ref TileMaterialLayers layersRef)
    {
      if (layersRef.Count <= 5)
        return;
      int num1 = 4 + (layersRef.Count >> 3);
      ThicknessTilesF thicknessTilesF1 = layersRef.First.Thickness + layersRef.Second.Thickness + layersRef.Third.Thickness + layersRef.Fourth.Thickness;
      int index1 = layersRef.OverflowIndex;
      int count = layersRef.Count;
      TileMaterialLayerOverflow[] backingArray = this.m_data.MaterialLayersOverflow.GetBackingArray();
      int num2 = 4;
      while (num2 < count)
      {
        ref TileMaterialLayerOverflow local1 = ref backingArray[index1];
        int overflowIndex = local1.OverflowIndex;
        if (local1.Material.IsNone)
        {
          Mafi.Log.Warning("Encountered layer with phantom material.");
        }
        else
        {
          TerrainMaterialSlimId recoveredMaterialId = this.RecoveredMaterialIds[(int) local1.Material.SlimId.Value];
          if (recoveredMaterialId.IsNotPhantom)
            local1.Material = local1.Material.WithNewId(recoveredMaterialId);
          thicknessTilesF1 += local1.Material.Thickness;
          ThicknessTilesF thicknessTilesF2 = TerrainTile.MIN_LAYER_THICKNESS + thicknessTilesF1.Value * TerrainTile.MIN_LAYER_THICKNESS_PER_DEPTH;
          if (!(local1.Material.Thickness >= thicknessTilesF2))
          {
            TerrainMaterialSlimId slimId = local1.Material.SlimId;
            int num3 = num2 + 1;
            int index2 = overflowIndex;
            int index3 = index1;
            for (int index4 = 0; index4 < num1 && num3 < count; ++num3)
            {
              ref TileMaterialLayerOverflow local2 = ref backingArray[index2];
              if (local2.Material.SlimId == slimId)
              {
                local1.Material += local2.Material.Thickness;
                index2 = local2.OverflowIndex;
                this.m_data.RemoveMiddleLayerFromOverflow_noChecks(ref backingArray[index3]);
                --count;
                --num3;
                if (local1.Material.Thickness >= thicknessTilesF2)
                  break;
              }
              else
              {
                index3 = index2;
                index2 = local2.OverflowIndex;
              }
              ++index4;
            }
            overflowIndex = local1.OverflowIndex;
          }
        }
        ++num2;
        index1 = overflowIndex;
      }
      layersRef.Count = count;
    }

    static TerrainManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainManager) obj).SerializeData(writer));
      TerrainManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainManager) obj).DeserializeData(reader));
      TerrainManager.MAX_DISRUPTION_DEPTH = 1.0.TilesThick();
      TerrainManager.BedrockLayerThicknessDefault = 1000.0.TilesThick();
    }

    /// <summary>
    /// Stores terrain data. Each tile property is stored as a separate row-major array (as opposed to a single array
    /// of a struct with all the properties) to have cache-friendly access for cases when not all tile properties are
    /// accessed at the same time. This also allow storing some properties like occupancy or
    /// </summary>
    [ManuallyWrittenSerialization]
    public struct TerrainData
    {
      private static readonly ThicknessTilesF EMERGENCY_THICKNESS;
      public readonly int Width;
      public readonly int Height;
      public readonly HeightTilesF[] Heights;
      /// <summary>Stores layers of materials for each tile.</summary>
      public readonly TileMaterialLayers[] MaterialLayers;
      /// <summary>Custom tile surfaces.</summary>
      /// <remarks>
      /// Surfaces are kept here instead of in separate manager to reuse and unify changed tiles tracking.
      /// This is very beneficial for fast serialization since tiles with a custom tile surface are most likely
      /// already marked as changed.
      /// </remarks>
      public readonly TileSurfaceData[] Surfaces;
      /// <summary>
      /// Tile flags. Max 16 per tile. If we need more, expand to <c>uint</c>.
      /// </summary>
      public readonly ushort[] Flags;
      /// <summary>
      /// Stores layers that did not fit to the predefined layers in <see cref="T:Mafi.Core.Terrain.TileMaterialLayers" /> struct.
      /// </summary>
      public LystStruct<TileMaterialLayerOverflow> MaterialLayersOverflow;
      /// <summary>
      /// Stores unused/removed indices from <see cref="F:Mafi.Core.Terrain.TerrainManager.TerrainData.MaterialLayersOverflow" /> for efficient
      /// reuse of removed elements.
      /// </summary>
      public LystStruct<int> MaterialLayersOverflowFreeIndices;
      [OnlyForSaveCompatibility(null)]
      public readonly BitMap ChangedTiles;
      /// <summary>Masks flags that are saved.</summary>
      [DoNotSave(0, null)]
      public uint SavedFlagsMask;
      [DoNotSave(0, null)]
      internal Option<Lyst<Pair<Tile2iIndex, TerrainManager.LoadedTileData>>> LoadedData;
      [DoNotSave(0, null)]
      internal Option<Lyst<TileMaterialLayerOverflow>> LoadedOverflowData;

      public readonly RelTile2i Size => new RelTile2i(this.Width, this.Height);

      public Option<HeightTilesF[]> HeightSnapshot { get; private set; }

      [OnlyForSaveCompatibility(null)]
      public TerrainData(int size)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Width = 0;
        this.Height = 0;
        this.MaterialLayersOverflow = new LystStruct<TileMaterialLayerOverflow>();
        this.MaterialLayersOverflowFreeIndices = new LystStruct<int>();
        // ISSUE: reference to a compiler-generated field
        this.\u003CHeightSnapshot\u003Ek__BackingField = new Option<HeightTilesF[]>();
        this.SavedFlagsMask = 0U;
        this.LoadedData = new Option<Lyst<Pair<Tile2iIndex, TerrainManager.LoadedTileData>>>();
        this.LoadedOverflowData = new Option<Lyst<TileMaterialLayerOverflow>>();
        this.Heights = new HeightTilesF[size];
        this.MaterialLayers = new TileMaterialLayers[size];
        this.Surfaces = new TileSurfaceData[size];
        this.Flags = new ushort[size];
        this.ChangedTiles = new BitMap(size);
      }

      public TerrainData(int width, int height)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this = new TerrainManager.TerrainData(width * height);
        this.Width = width;
        this.Height = height;
      }

      /// <summary>
      /// Returns index of the given tile, assuming that data starts at [0, 0].
      /// Note that this does not perform any validity checks.
      /// </summary>
      public readonly int GetTileIndex(Tile2i tile) => tile.X + tile.Y * this.Width;

      public readonly int GetTileIndex(Tile2i tile, Tile2i dataOrigin)
      {
        return this.GetTileIndex(tile - dataOrigin);
      }

      public readonly int GetTileIndex(RelTile2i tile) => tile.X + tile.Y * this.Width;

      public readonly HeightTilesF GetHeightInterpolated(Tile2f tile)
      {
        int tileIndex = this.GetTileIndex(tile.Tile2i);
        ushort[] flags = this.Flags;
        if ((uint) tileIndex >= (uint) flags.Length)
        {
          Mafi.Log.Error("GetHeight: Tile index out of bounds");
          return new HeightTilesF();
        }
        if (((int) flags[tileIndex] & 1) != 0)
          return this.Heights[tileIndex];
        RelTile2f relTile2f = tile.FractionalPartNonNegative();
        return this.Heights[tileIndex].Lerp(this.Heights[tileIndex + 1], relTile2f.X).Lerp(this.Heights[tileIndex + this.Width].Lerp(this.Heights[tileIndex + this.Width + 1], relTile2f.X), relTile2f.Y);
      }

      public void EnableAndInitializeHeightSnapshot()
      {
        Mafi.Assert.That<Option<HeightTilesF[]>>(this.HeightSnapshot).IsNone<HeightTilesF[]>("Snapshot already allocated.");
        Mafi.Log.Info("Allocating terrain height snapshot.");
        this.HeightSnapshot = (Option<HeightTilesF[]>) new HeightTilesF[this.Heights.Length];
      }

      public readonly unsafe void ValidateAndFixData(
        Tile2iIndex index,
        TerrainMaterialSlimId validMaterial,
        Lyst<string> errors)
      {
        ref TileMaterialLayers local1 = ref this.MaterialLayers[index.Value];
        if (local1.Count < 0)
        {
          errors.Add(string.Format("Invalid value on tile {0}: Count = {1}, should be non-negative.", (object) index, (object) local1.Count));
          *(TileMaterialLayers*) ref local1 = new TileMaterialLayers();
        }
        if (local1.Count >= 1)
        {
          if (local1.First.SlimId.IsPhantom)
          {
            errors.Add(string.Format("Invalid product on tile {0} in layer 1/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, ", (object) local1.First) + string.Format("replacing with ID #{0}.", (object) validMaterial));
            local1.First = local1.First.WithNewId(validMaterial);
          }
          if (local1.First.Thickness.IsNotPositive)
          {
            errors.Add(string.Format("Invalid thickness on tile {0} in layer 1/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, replacing with {1}.", (object) local1.First, (object) TerrainManager.TerrainData.EMERGENCY_THICKNESS));
            local1.First = local1.First.WithThickness(TerrainManager.TerrainData.EMERGENCY_THICKNESS);
          }
        }
        else if (local1.First != new TerrainMaterialThicknessSlim())
          errors.Add(string.Format("Invalid value on tile {0} in layer 1/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, should be default.", (object) local1.First));
        if (local1.Count >= 2)
        {
          if (local1.Second.SlimId.IsPhantom)
          {
            errors.Add(string.Format("Invalid product on tile {0} in layer 2/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, replacing with ID #{1}.", (object) local1.Second, (object) validMaterial));
            local1.Second = local1.Second.WithNewId(validMaterial);
          }
          if (local1.Second.Thickness.IsNotPositive)
          {
            errors.Add(string.Format("Invalid thickness on tile {0} in layer 2/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, replacing with {1}.", (object) local1.Second, (object) TerrainManager.TerrainData.EMERGENCY_THICKNESS));
            local1.Second = local1.Second.WithThickness(TerrainManager.TerrainData.EMERGENCY_THICKNESS);
          }
        }
        else if (local1.Second != new TerrainMaterialThicknessSlim())
          errors.Add(string.Format("Invalid value on tile {0} in layer 2/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, should be default.", (object) local1.Second));
        if (local1.Count >= 3)
        {
          if (local1.Third.SlimId.IsPhantom)
          {
            errors.Add(string.Format("Invalid product on tile {0} in layer 3/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, replacing with ID #{1}.", (object) local1.Third, (object) validMaterial));
            local1.Third = local1.Third.WithNewId(validMaterial);
          }
          if (local1.Third.Thickness.IsNotPositive)
          {
            errors.Add(string.Format("Invalid thickness on tile {0} in layer 3/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, replacing with {1}.", (object) local1.Third, (object) TerrainManager.TerrainData.EMERGENCY_THICKNESS));
            local1.Third = local1.Third.WithThickness(TerrainManager.TerrainData.EMERGENCY_THICKNESS);
          }
        }
        else if (local1.Third != new TerrainMaterialThicknessSlim())
          errors.Add(string.Format("Invalid value on tile {0} in layer 3/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, should be default.", (object) local1.Third));
        if (local1.Count >= 4)
        {
          if (local1.Fourth.SlimId.IsPhantom)
          {
            errors.Add(string.Format("Invalid product on tile {0} in layer 4/{1}: {2}, ", (object) index, (object) local1.Count, (object) local1.Fourth) + string.Format("replacing with ID #{0}.", (object) validMaterial));
            local1.Fourth = local1.Fourth.WithNewId(validMaterial);
          }
          if (local1.Fourth.Thickness.IsNotPositive)
          {
            errors.Add(string.Format("Invalid thickness on tile {0} in layer 4/{1}: {2}, ", (object) index, (object) local1.Count, (object) local1.Fourth) + string.Format("replacing with {0}.", (object) TerrainManager.TerrainData.EMERGENCY_THICKNESS));
            local1.Fourth = local1.Fourth.WithThickness(TerrainManager.TerrainData.EMERGENCY_THICKNESS);
          }
        }
        else if (local1.Fourth != new TerrainMaterialThicknessSlim())
          errors.Add(string.Format("Invalid value on tile {0} in layer 4/{1}: ", (object) index, (object) local1.Count) + string.Format("{0}, should be default.", (object) local1.Fourth));
        if (local1.Count <= 4)
          return;
        int overflowIndex = local1.OverflowIndex;
        int num1 = -1;
        TileMaterialLayerOverflow[] backingArray = this.MaterialLayersOverflow.GetBackingArray();
        int num2 = 4;
        while (num2 < local1.Count)
        {
          if ((uint) overflowIndex >= (uint) backingArray.Length)
          {
            errors.Add(string.Format("Invalid overflow index {0} in layer {1}/{2} on tile {3}, ", (object) overflowIndex, (object) num2, (object) local1.Count, (object) index) + string.Format("removing and setting count from {0} to {1}.", (object) local1.Count, (object) num2));
            local1.Count = num2;
            if (num1 >= 0)
              break;
            local1.OverflowIndex = 0;
            break;
          }
          ref TileMaterialLayerOverflow local2 = ref backingArray[overflowIndex];
          if (local2.Material.SlimId.IsPhantom)
          {
            errors.Add(string.Format("Invalid product on tile {0} in layer {1}/{2}: ", (object) index, (object) num2, (object) local1.Count) + string.Format("{0}, replacing with ID #{1}.", (object) local2.Material, (object) validMaterial));
            local2.Material = local2.Material.WithNewId(validMaterial);
          }
          if (local2.Material.Thickness.IsNotPositive)
          {
            errors.Add(string.Format("Invalid thickness on tile {0} in layer {1}/{2}: ", (object) index, (object) num2, (object) local1.Count) + string.Format("{0}, replacing with {1}.", (object) local2.Material, (object) TerrainManager.TerrainData.EMERGENCY_THICKNESS));
            local2.Material = local2.Material.WithThickness(TerrainManager.TerrainData.EMERGENCY_THICKNESS);
          }
          overflowIndex = local2.OverflowIndex;
          ++num2;
          num1 = overflowIndex;
        }
      }

      public void AppendOrPushFirstLayer(Tile2iIndex tileIndex, TerrainMaterialThicknessSlim layer)
      {
        ref TileMaterialLayers local = ref this.MaterialLayers[tileIndex.Value];
        if (local.First.SlimId == layer.SlimId)
          local.First += layer.Thickness;
        else
          this.PushNewFirstLayer(ref local, layer);
      }

      public void AppendOrPushFirstLayer(
        Tile2iIndex tileIndex,
        TerrainMaterialSlimId slimId,
        ThicknessTilesF thickness)
      {
        ref TileMaterialLayers local = ref this.MaterialLayers[tileIndex.Value];
        if (local.First.SlimId == slimId)
          local.First += thickness;
        else
          this.PushNewFirstLayer(ref local, new TerrainMaterialThicknessSlim(slimId, thickness));
      }

      public void ChangeFirstLayerTo(Tile2iIndex tileIndex, TerrainMaterialSlimId slimId)
      {
        ref TileMaterialLayers local = ref this.MaterialLayers[tileIndex.Value];
        if (local.Second.SlimId == slimId)
        {
          local.Second += local.First.Thickness;
          this.RemoveFirstLayer_noChecks(ref local);
        }
        else
          local.First = local.First.WithNewId(slimId);
      }

      /// <summary>
      /// Pushes the given layer to the top of the tile.
      /// The caller is responsible to ensure that duplicate layers won't occur.
      /// </summary>
      public void PushNewFirstLayer(
        ref TileMaterialLayers layersRef,
        TerrainMaterialThicknessSlim newLayer)
      {
        if (layersRef.Count >= 4)
          layersRef.OverflowIndex = this.AddLayerToOverflow(layersRef.Fourth, layersRef.OverflowIndex);
        layersRef.Fourth = layersRef.Third;
        layersRef.Third = layersRef.Second;
        layersRef.Second = layersRef.First;
        layersRef.First = newLayer;
        ++layersRef.Count;
      }

      /// <summary>
      /// Pushes the given layer to the second spot of the tile.
      /// The caller is responsible to ensure that the tile already has at least one layer and that duplicate layers
      /// won't occur.
      /// </summary>
      public void PushNewSecondLayer(
        ref TileMaterialLayers layersRef,
        TerrainMaterialThicknessSlim newLayer)
      {
        if (layersRef.Count >= 4)
          layersRef.OverflowIndex = this.AddLayerToOverflow(layersRef.Fourth, layersRef.OverflowIndex);
        layersRef.Fourth = layersRef.Third;
        layersRef.Third = layersRef.Second;
        layersRef.Second = newLayer;
        ++layersRef.Count;
      }

      public void PushNewThirdLayer(
        ref TileMaterialLayers layersRef,
        TerrainMaterialThicknessSlim newLayer)
      {
        if (layersRef.Count >= 4)
          layersRef.OverflowIndex = this.AddLayerToOverflow(layersRef.Fourth, layersRef.OverflowIndex);
        layersRef.Fourth = layersRef.Third;
        layersRef.Third = newLayer;
        ++layersRef.Count;
      }

      public void PushNewFourthLayer(
        ref TileMaterialLayers layersRef,
        TerrainMaterialThicknessSlim newLayer)
      {
        if (layersRef.Count >= 4)
          layersRef.OverflowIndex = this.AddLayerToOverflow(layersRef.Fourth, layersRef.OverflowIndex);
        layersRef.Fourth = newLayer;
        ++layersRef.Count;
      }

      /// <summary>
      /// Adds the given terrain layer to the overflow data structure and returns its index.
      /// This does not change layers count.
      /// WARNING: This many re-allocate the internal array of <see cref="F:Mafi.Core.Terrain.TerrainManager.TerrainData.MaterialLayersOverflowFreeIndices" />.
      /// </summary>
      public int AddLayerToOverflow(
        TerrainMaterialThicknessSlim materialThickness,
        int overflowIndex)
      {
        int index;
        if (this.MaterialLayersOverflowFreeIndices.IsNotEmpty)
        {
          index = this.MaterialLayersOverflowFreeIndices.PopLast();
          this.MaterialLayersOverflow[index] = new TileMaterialLayerOverflow(materialThickness, overflowIndex);
        }
        else
        {
          index = this.MaterialLayersOverflow.Count;
          this.MaterialLayersOverflow.Add(new TileMaterialLayerOverflow(materialThickness, overflowIndex));
        }
        return index;
      }

      public void ClearAllLayersAt(Tile2iIndex index)
      {
        this.ClearAllLayersOf(ref this.MaterialLayers[index.Value]);
      }

      public void ClearAllLayersOf(ref TileMaterialLayers layersRef)
      {
        if (layersRef.Count > 4)
        {
          int overflowIndex = layersRef.OverflowIndex;
          for (int index = 4; index < layersRef.Count; ++index)
          {
            this.MaterialLayersOverflowFreeIndices.Add(overflowIndex);
            overflowIndex = this.MaterialLayersOverflow[overflowIndex].OverflowIndex;
          }
        }
        layersRef = new TileMaterialLayers();
      }

      public void ClearAllAndReset()
      {
        Array.Clear((Array) this.Heights, 0, this.Heights.Length);
        Array.Clear((Array) this.MaterialLayers, 0, this.MaterialLayers.Length);
        Array.Clear((Array) this.Surfaces, 0, this.Surfaces.Length);
        Array.Clear((Array) this.Flags, 0, this.Flags.Length);
        this.ChangedTiles.ClearAllBits();
        this.MaterialLayersOverflow.Clear();
        this.MaterialLayersOverflowFreeIndices.Clear();
        this.LoadedData = Option<Lyst<Pair<Tile2iIndex, TerrainManager.LoadedTileData>>>.None;
        this.LoadedOverflowData = Option<Lyst<TileMaterialLayerOverflow>>.None;
      }

      /// <summary>
      /// Removes the first layer from the given structure. Does NOT perform any checks, the layer must exist.
      /// Does not change height, does not invoke any events.
      /// </summary>
      public void RemoveFirstLayer_noChecks(ref TileMaterialLayers layersRef)
      {
        layersRef.First = layersRef.Second;
        layersRef.Second = layersRef.Third;
        layersRef.Third = layersRef.Fourth;
        layersRef.Fourth = layersRef.Count <= 4 ? new TerrainMaterialThicknessSlim() : this.RemoveFirstLayerFromOverflow_noChecks(ref layersRef);
        --layersRef.Count;
      }

      /// <summary>
      /// Removes second layer the given structure. Does NOT perform any checks, the layer must exist.
      /// Does not change height, does not invoke any events.
      /// </summary>
      public void RemoveSecondLayer_noChecks(ref TileMaterialLayers layersRef)
      {
        layersRef.Second = layersRef.Third;
        layersRef.Third = layersRef.Fourth;
        layersRef.Fourth = layersRef.Count <= 4 ? new TerrainMaterialThicknessSlim() : this.RemoveFirstLayerFromOverflow_noChecks(ref layersRef);
        --layersRef.Count;
      }

      /// <summary>
      /// Removes third layer the given structure. Does NOT perform any checks, the layer must exist.
      /// Does not change height, does not invoke any events.
      /// </summary>
      public void RemoveThirdLayer_noChecks(ref TileMaterialLayers layersRef)
      {
        layersRef.Third = layersRef.Fourth;
        layersRef.Fourth = layersRef.Count <= 4 ? new TerrainMaterialThicknessSlim() : this.RemoveFirstLayerFromOverflow_noChecks(ref layersRef);
        --layersRef.Count;
      }

      /// <summary>
      /// Removes fourth layer the given structure. Does NOT perform any checks, the layer must exist.
      /// Does not change height, does not invoke any events.
      /// </summary>
      public void RemoveFourthLayer_noChecks(ref TileMaterialLayers layersRef)
      {
        layersRef.Fourth = layersRef.Count <= 4 ? new TerrainMaterialThicknessSlim() : this.RemoveFirstLayerFromOverflow_noChecks(ref layersRef);
        --layersRef.Count;
      }

      /// <summary>
      /// Removes and returns the first layer from overflow. Does NOT perform any checks and does NOT decrement layers
      /// count. It is callers responsibility to handle layers count and ensure that the overflow has at least one layer.
      /// </summary>
      public TerrainMaterialThicknessSlim RemoveFirstLayerFromOverflow_noChecks(
        ref TileMaterialLayers layersRef)
      {
        TileMaterialLayerOverflow materialLayerOverflow = this.MaterialLayersOverflow[layersRef.OverflowIndex];
        this.MaterialLayersOverflowFreeIndices.Add(layersRef.OverflowIndex);
        layersRef.OverflowIndex = materialLayerOverflow.OverflowIndex;
        return materialLayerOverflow.Material;
      }

      /// <summary>
      /// Removes a middle layer from overflow. Does NOT perform any checks and does NOT decrement layers count.
      /// Note that the argument is parent layer, not the layer itself.
      /// </summary>
      public void RemoveMiddleLayerFromOverflow_noChecks(
        ref TileMaterialLayerOverflow parentLayerRef)
      {
        this.MaterialLayersOverflowFreeIndices.Add(parentLayerRef.OverflowIndex);
        int overflowIndex = this.MaterialLayersOverflow[parentLayerRef.OverflowIndex].OverflowIndex;
        parentLayerRef.OverflowIndex = overflowIndex;
      }

      /// <summary>Initializes tile with layers. Tile must be empty.</summary>
      public void InitializeLayers(
        ref TileMaterialLayers layersRef,
        ReadOnlyArraySlice<TerrainMaterialThicknessSlim> layers)
      {
        if (layersRef.Count != 0)
        {
          Mafi.Log.Error("Appending layers to non-empty tiles is not supported.");
        }
        else
        {
          layersRef.Count = layers.Length;
          if (layers.Length <= 4)
          {
            if (layers.Length == 0)
              return;
            layersRef.First = layers[0];
            if (layers.Length == 1)
              return;
            layersRef.Second = layers[1];
            if (layers.Length == 2)
              return;
            layersRef.Third = layers[2];
            if (layers.Length == 3)
              return;
            layersRef.Fourth = layers[3];
          }
          else
          {
            layersRef.First = layers[0];
            layersRef.Second = layers[1];
            layersRef.Third = layers[2];
            layersRef.Fourth = layers[3];
            layersRef.OverflowIndex = this.AddLayerToOverflow(layers[4], 0);
            int index1 = layersRef.OverflowIndex;
            for (int index2 = 5; index2 < layersRef.Count; ++index2)
            {
              int overflow = this.AddLayerToOverflow(layers[index2], 0);
              this.MaterialLayersOverflow.GetBackingArray()[index1].OverflowIndex = overflow;
              index1 = overflow;
            }
          }
        }
      }

      public readonly void GetLayersAt(
        TileMaterialLayers layers,
        Lyst<TerrainMaterialThicknessSlim> outLayers)
      {
        if (layers.Count <= 4)
        {
          if (layers.Count == 0)
            return;
          outLayers.Add(layers.First);
          if (layers.Count == 1)
            return;
          outLayers.Add(layers.Second);
          if (layers.Count == 2)
            return;
          outLayers.Add(layers.Third);
          if (layers.Count == 3)
            return;
          outLayers.Add(layers.Fourth);
        }
        else
        {
          outLayers.Add(layers.First);
          outLayers.Add(layers.Second);
          outLayers.Add(layers.Third);
          outLayers.Add(layers.Fourth);
          int overflowIndex = layers.OverflowIndex;
          for (int index = 5; index < layers.Count; ++index)
          {
            TileMaterialLayerOverflow materialLayerOverflow = this.MaterialLayersOverflow[overflowIndex];
            outLayers.Add(materialLayerOverflow.Material);
            overflowIndex = materialLayerOverflow.OverflowIndex;
          }
        }
      }

      /// <summary>
      /// Custom terrain data serialization that is highly optimized to save bytes and be fast.
      /// </summary>
      /// <remarks>
      /// Only changed tiles are saved. To avoid need of storing (x, y) coordinates of changed tiles and to make
      /// saving more performant, the changed bit structure is used for natural tile grouping by 64 (ulong).
      /// Each group writes the bit mask of changed tiles and then writes data of tiles that have bit set.
      /// 
      /// To further save data and avoid writing many zeros in areas where there are no changes (nearly the entire
      /// map at the start of the game), a single byte representing sequence length is computed and written.
      /// This byte denotes whether the next group will be all zeros or all non-zeros (the changed tiles ulongs).
      /// This information is saved in the HSB. The remaining 7 bits denote length of the group. So value
      /// 0b1000_0011 means that the next group has 4 (0b11 + 1) values and each value contains changed tiles
      /// (0b1000_0000). On the other hand value 0b0000_0101 represents a group of 6 zeros and the zeros won't be
      /// even written. Note that the group length is saved as (len - 1) since we never need the value zero. This
      /// was the max group length is 128 (not just 127 :).
      /// </remarks>
      public static void Serialize(TerrainManager.TerrainData value, BlobWriter writer)
      {
        writer.WriteIntNotNegative(value.Width);
        writer.WriteIntNotNegative(value.Height);
        ulong[] changedData = value.ChangedTiles.BackingArray;
        int num1 = value.ChangedTiles.CountSetBits();
        int changedTilesWritten = 0;
        int num2;
        for (int index1 = 0; index1 < changedData.Length; index1 += num2)
        {
          if (changedData[index1] == 0UL)
          {
            num2 = 128;
            for (int index2 = 1; index2 <= 128; ++index2)
            {
              int index3 = index1 + index2;
              if (index3 >= changedData.Length || changedData[index3] != 0UL)
              {
                num2 = index2;
                break;
              }
            }
            writer.WriteByte((byte) (num2 - 1));
          }
          else
          {
            num2 = 128;
            for (int index4 = 1; index4 <= 128; ++index4)
            {
              int index5 = index1 + index4;
              if (index5 >= changedData.Length || changedData[index5] == 0UL)
              {
                num2 = index4;
                break;
              }
            }
            writer.WriteByte((byte) (num2 - 1 | 128));
            for (int index6 = 0; index6 < num2; ++index6)
              writeChangedTiles(index1 + index6);
          }
        }
        if (num1 == changedTilesWritten)
          return;
        Mafi.Log.Error(string.Format("Number of changed tiles {0} is not equal to the number of ", (object) num1) + string.Format("written tiles {0}. Terrain changed during save?", (object) changedTilesWritten));

        void writeTile(int absI)
        {
          HeightTilesF.Serialize(value.Heights[absI], writer);
          TileSurfaceData.Serialize(value.Surfaces[absI], writer);
          writer.WriteUInt((uint) value.Flags[absI] & value.SavedFlagsMask);
          TileMaterialLayers materialLayer = value.MaterialLayers[absI];
          writer.WriteIntNotNegative(materialLayer.Count);
          if (materialLayer.Count <= 4)
          {
            if (materialLayer.Count <= 0)
              return;
            TerrainMaterialThicknessSlim.Serialize(materialLayer.First, writer);
            if (materialLayer.Count == 1)
              return;
            TerrainMaterialThicknessSlim.Serialize(materialLayer.Second, writer);
            if (materialLayer.Count == 2)
              return;
            TerrainMaterialThicknessSlim.Serialize(materialLayer.Third, writer);
            if (materialLayer.Count == 3)
              return;
            TerrainMaterialThicknessSlim.Serialize(materialLayer.Fourth, writer);
          }
          else
          {
            TerrainMaterialThicknessSlim.Serialize(materialLayer.First, writer);
            TerrainMaterialThicknessSlim.Serialize(materialLayer.Second, writer);
            TerrainMaterialThicknessSlim.Serialize(materialLayer.Third, writer);
            TerrainMaterialThicknessSlim.Serialize(materialLayer.Fourth, writer);
            TileMaterialLayerOverflow materialLayerOverflow = value.MaterialLayersOverflow[materialLayer.OverflowIndex];
            TerrainMaterialThicknessSlim.Serialize(materialLayerOverflow.Material, writer);
            for (int index = 5; index < materialLayer.Count; ++index)
            {
              materialLayerOverflow = value.MaterialLayersOverflow[materialLayerOverflow.OverflowIndex];
              TerrainMaterialThicknessSlim.Serialize(materialLayerOverflow.Material, writer);
            }
          }
        }

        void writeChangedTiles(int index)
        {
          ulong num = changedData[index];
          writer.WriteULong(num);
          int absI = index << 6;
          if (num == ulong.MaxValue)
          {
            for (int index1 = 0; index1 < 64; ++index1)
              writeTile(absI + index1);
            changedTilesWritten += 64;
          }
          else
          {
            do
            {
              if (((long) num & 1L) != 0L)
              {
                writeTile(absI);
                ++changedTilesWritten;
              }
              num >>= 1;
              ++absI;
            }
            while (num != 0UL);
          }
        }
      }

      public static TerrainManager.TerrainData Deserialize(BlobReader reader)
      {
        return TerrainManager.TerrainData.DeserializeCustom(reader, out int _);
      }

      public static TerrainManager.TerrainData DeserializeCustom(
        BlobReader reader,
        out int loadedTiles)
      {
        Lyst<Pair<Tile2iIndex, TerrainManager.LoadedTileData>> loadedData = new Lyst<Pair<Tile2iIndex, TerrainManager.LoadedTileData>>(4096);
        Lyst<TileMaterialLayerOverflow> loadedOverflow = new Lyst<TileMaterialLayerOverflow>(4096);
        TerrainManager.TerrainData terrainData = (reader.LoadedSaveVersion < 140 ? new TerrainManager.TerrainData(reader.ReadIntNotNegative()) : new TerrainManager.TerrainData(reader.ReadIntNotNegative(), reader.ReadIntNotNegative())) with
        {
          LoadedData = (Option<Lyst<Pair<Tile2iIndex, TerrainManager.LoadedTileData>>>) loadedData,
          LoadedOverflowData = (Option<Lyst<TileMaterialLayerOverflow>>) loadedOverflow
        };
        int loadedTilesTmp = 0;
        int length = terrainData.ChangedTiles.BackingArray.Length;
        int num1;
        for (int index1 = 0; index1 < length; index1 += num1)
        {
          int num2 = (int) reader.ReadByte();
          if ((num2 & 128) == 0)
          {
            num1 = num2 + 1;
          }
          else
          {
            num1 = (num2 & -129) + 1;
            for (int index2 = 0; index2 < num1; ++index2)
              readChangedTiles(index1 + index2);
          }
        }
        loadedTiles = loadedTilesTmp;
        return terrainData;

        TerrainManager.LoadedTileData readTile(int absI)
        {
          HeightTilesF height = HeightTilesF.Deserialize(reader);
          TileSurfaceData surface = TileSurfaceData.Deserialize(reader);
          ushort flags = (ushort) reader.ReadUInt();
          TileMaterialLayers layers = new TileMaterialLayers();
          layers.Count = reader.ReadIntNotNegative();
          if (layers.Count <= 4)
          {
            if (layers.Count > 0)
            {
              layers.First = TerrainMaterialThicknessSlim.Deserialize(reader);
              if (layers.Count > 1)
              {
                layers.Second = TerrainMaterialThicknessSlim.Deserialize(reader);
                if (layers.Count > 2)
                {
                  layers.Third = TerrainMaterialThicknessSlim.Deserialize(reader);
                  if (layers.Count > 3)
                    layers.Fourth = TerrainMaterialThicknessSlim.Deserialize(reader);
                }
              }
            }
          }
          else
          {
            layers.First = TerrainMaterialThicknessSlim.Deserialize(reader);
            layers.Second = TerrainMaterialThicknessSlim.Deserialize(reader);
            layers.Third = TerrainMaterialThicknessSlim.Deserialize(reader);
            layers.Fourth = TerrainMaterialThicknessSlim.Deserialize(reader);
            layers.OverflowIndex = loadedOverflow.Count;
            loadedOverflow.Add(new TileMaterialLayerOverflow(TerrainMaterialThicknessSlim.Deserialize(reader), loadedOverflow.Count + 1));
            for (int index = 5; index < layers.Count; ++index)
              loadedOverflow.Add(new TileMaterialLayerOverflow(TerrainMaterialThicknessSlim.Deserialize(reader), loadedOverflow.Count + 1));
          }
          return new TerrainManager.LoadedTileData(height, surface, flags, layers);
        }

        void readChangedTiles(int index)
        {
          ulong num1 = reader.ReadULong();
          int num2 = index << 6;
          if (num1 == ulong.MaxValue)
          {
            for (int index1 = 0; index1 < 64; ++index1)
            {
              Tile2iIndex first = new Tile2iIndex(num2 + index1);
              loadedData.Add(Pair.Create<Tile2iIndex, TerrainManager.LoadedTileData>(first, readTile(first.Value)));
            }
            loadedTilesTmp += 64;
          }
          else
          {
            do
            {
              if (((long) num1 & 1L) != 0L)
              {
                Tile2iIndex first = new Tile2iIndex(num2);
                loadedData.Add(Pair.Create<Tile2iIndex, TerrainManager.LoadedTileData>(first, readTile(first.Value)));
                ++loadedTilesTmp;
              }
              num1 >>= 1;
              ++num2;
            }
            while (num1 != 0UL);
          }
        }
      }

      [Conditional("VALIDATE_DATA_AFTER_EACH_CHANGE")]
      public readonly void VerifyOverflowChainOrThrow_ExplicitOnly(TileMaterialLayers layers)
      {
        if (layers.Count <= 4)
          return;
        int overflowIndex = layers.OverflowIndex;
        for (int index = 4; index < layers.Count; ++index)
        {
          if ((uint) overflowIndex >= (uint) this.MaterialLayersOverflow.Capacity)
            throw new Exception(string.Format("Invalid overflow index {0} at layer {1}/{2}. ", (object) overflowIndex, (object) index, (object) layers.Count) + string.Format("The overflow array has length of {0} ", (object) this.MaterialLayersOverflow.Capacity) + string.Format("and there is {0} valid elements.", (object) this.MaterialLayersOverflow.Count));
          if ((uint) overflowIndex >= (uint) this.MaterialLayersOverflow.Count)
            throw new Exception(string.Format("Incorrect overflow index {0} at layer {1}/{2}. ", (object) overflowIndex, (object) index, (object) layers.Count) + string.Format("The overflow array has {0} valid elements.", (object) this.MaterialLayersOverflow.Count));
          overflowIndex = this.MaterialLayersOverflow[overflowIndex].OverflowIndex;
        }
      }

      static TerrainData()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        TerrainManager.TerrainData.EMERGENCY_THICKNESS = 0.1.TilesThick();
      }
    }

    internal readonly struct LoadedTileData
    {
      public readonly HeightTilesF Height;
      public readonly TileSurfaceData Surface;
      public readonly ushort Flags;
      public readonly TileMaterialLayers Layers;

      public LoadedTileData(
        HeightTilesF height,
        TileSurfaceData surface,
        ushort flags,
        TileMaterialLayers layers)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.Height = height;
        this.Surface = surface;
        this.Flags = flags;
        this.Layers = layers;
      }
    }
  }
}
