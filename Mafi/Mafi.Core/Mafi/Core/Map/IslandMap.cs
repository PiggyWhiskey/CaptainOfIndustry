// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.IslandMap
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.Generators;
using Mafi.Logging;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public sealed class IslandMap : IStartLocationProvider, IMapInfoProvider
  {
    public const int MAP_VERSION_LATEST = 1;
    public readonly string MapName;
    public readonly int MapVersion;
    public readonly StartingLocation StartingLocation;
    /// <summary>Map cells represented as convex irregular polygons.</summary>
    public readonly ImmutableArray<MapCell> Cells;
    /// <summary>
    /// All control points that were used to generate the map. Note that not all points will end up as actual cells.
    /// For example, points on boundary may not form an enclosed cell.
    /// </summary>
    public readonly ImmutableArray<Tile2i> ControlPoints;
    /// <summary>
    /// Subset of <see cref="F:Mafi.Core.Map.IslandMap.ControlPoints" /> that did end up as cells.
    /// </summary>
    public readonly ImmutableArray<Tile2i> CellControlPoints;
    /// <summary>
    /// Subset of <see cref="F:Mafi.Core.Map.IslandMap.ControlPoints" /> that did not end up as cells.
    /// </summary>
    public readonly ImmutableArray<Tile2i> NonCellControlPoints;
    /// <summary>Locations of perimeter points (vornoi points).</summary>
    public readonly ImmutableArray<Tile2i> CellEdgePoints;
    /// <summary>
    /// Default material that will fill the terrain if no other material is present.
    /// </summary>
    public readonly TerrainMaterialProto Bedrock;
    /// <summary>
    /// All resource generators on the map in the correct order.
    /// </summary>
    public readonly ImmutableArray<ITerrainResourceGenerator> ResourcesGenerators;
    /// <summary>The prop generator for the map.</summary>
    public readonly TerrainPropGenerationParams PropGenerationParams;
    /// <summary>Explicitly placed terrain props.</summary>
    public readonly ImmutableArray<TerrainPropMapData> TerrainProps;
    /// <summary>Edge terrain generator obtained from cells.</summary>
    public readonly IReadOnlyDictionary<MapCell, ImmutableArray<ITerrainResourceGenerator>> CellEdgeTerrainGenerators;
    /// <summary>
    /// All terrain generators, incl <see cref="F:Mafi.Core.Map.IslandMap.ResourcesGenerators" /> and <see cref="F:Mafi.Core.Map.IslandMap.CellEdgeTerrainGenerators" />.
    /// </summary>
    public readonly ImmutableArray<ITerrainResourceGenerator> AllTerrainGenerators;
    /// <summary>
    /// All virtual resources on the map. These resources are not present as products in the tiles but are mineable
    /// with special API. These can be for example oil or water.
    /// </summary>
    public readonly ImmutableArray<IVirtualTerrainResource> VirtualResources;
    public readonly ImmutableArray<ITerrainPostProcessor> TerrainPostProcessors;
    public readonly ProtosDb ProtosDb;
    public readonly IslandMapConfig Config;
    public readonly IslandMapDifficultyConfig DifficultyConfig;
    public readonly int TerrainWidth;
    public readonly int TerrainHeight;
    /// <summary>
    /// Coordinates of all chunks that are part of least one map cell.
    /// </summary>
    public readonly ImmutableArray<Chunk2i> Chunks;
    /// <summary>Cached map from chunk to cells.</summary>
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Dict<Chunk2i, ImmutableArray<MapCell>> m_chunkToCells;
    [DoNotSave(0, null)]
    private ImmutableArray<Line2i> m_cellCoastLinesCache;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    string IMapInfoProvider.Name => this.MapName + " (legacy)";

    int IMapInfoProvider.MapVersion => this.MapVersion;

    StartingLocation IStartLocationProvider.StartingLocation => this.StartingLocation;

    /// <summary>
    /// Lines that are on the boundary between ocean and non-ocean cells.
    /// </summary>
    public ImmutableArray<Line2i> CellCoastLines
    {
      get
      {
        if (this.m_cellCoastLinesCache.IsNotValid)
          this.m_cellCoastLinesCache = this.Cells.Filter((Predicate<MapCell>) (c => c.IsOcean)).SelectMany<Line2i>((Func<MapCell, IEnumerable<Line2i>>) (c => c.Neighbors.WhereValues<MapCell>((Func<MapCell, bool>) (n => n.IsNotOcean)).Select<MapCell, Line2i>((Func<MapCell, Line2i>) (n => n.GetEdgeToNeighbor(c))))).ToImmutableArray<Line2i>();
        return this.m_cellCoastLinesCache;
      }
    }

    public MapCell this[MapCellId id] => this.Cells[id.Value];

    public IslandMap(
      string mapName,
      int mapVersion,
      StartingLocation startingLocation,
      ProtosDb protosDb,
      TerrainMaterialProto bedrock,
      ImmutableArray<MapCell> cells,
      ImmutableArray<Tile2i> controlPoints,
      ImmutableArray<Tile2i> cellEdgePoints,
      ImmutableArray<ITerrainResourceGenerator> resourcesGenerators,
      ImmutableArray<IVirtualTerrainResource> virtualResources,
      ImmutableArray<ITerrainPostProcessor> terrainPostProcessors,
      ImmutableArray<TerrainPropMapData> terrainProps,
      IslandMapConfig config,
      IslandMapDifficultyConfig difficultyConfig)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.PropGenerationParams = new TerrainPropGenerationParams();
      this.m_chunkToCells = new Dict<Chunk2i, ImmutableArray<MapCell>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(controlPoints.Length).IsGreaterOrEqual(cells.Length);
      this.MapName = mapName.CheckNotNull<string>();
      this.StartingLocation = startingLocation.CheckNotNull<StartingLocation>();
      this.Bedrock = bedrock.CheckNotNull<TerrainMaterialProto>();
      this.Cells = cells.CheckNotEmpty<MapCell>();
      this.ControlPoints = controlPoints.CheckNotEmpty<Tile2i>();
      this.CellControlPoints = cells.Map<Tile2i>((Func<MapCell, Tile2i>) (c => controlPoints[c.CenterPointIndex]));
      this.NonCellControlPoints = controlPoints.AsEnumerable().Except<Tile2i>(this.CellControlPoints.AsEnumerable()).ToImmutableArray<Tile2i>();
      Assert.That<ImmutableArray<Tile2i>>(this.ControlPoints).HasLength<Tile2i>(this.CellControlPoints.Length + this.NonCellControlPoints.Length);
      this.CellEdgePoints = cellEdgePoints.CheckNotEmpty<Tile2i>();
      this.ResourcesGenerators = resourcesGenerators.CheckNotDefaultStruct<ImmutableArray<ITerrainResourceGenerator>>();
      this.VirtualResources = virtualResources.CheckNotDefaultStruct<ImmutableArray<IVirtualTerrainResource>>();
      this.TerrainProps = terrainProps;
      this.ProtosDb = protosDb;
      this.Config = config;
      this.DifficultyConfig = difficultyConfig;
      this.MapVersion = mapVersion;
      this.TerrainPostProcessors = terrainPostProcessors;
      MinMaxPair<int> minMaxPair1 = this.CellEdgePoints.AsEnumerable().MinMax<int, Tile2i>((Func<Tile2i, int>) (x => x.X));
      MinMaxPair<int> minMaxPair2 = this.CellEdgePoints.AsEnumerable().MinMax<int, Tile2i>((Func<Tile2i, int>) (x => x.Y));
      if (minMaxPair1.Min < 0 || minMaxPair2.Min < 0)
        Log.Error("Map '" + mapName + "' has some cells in negative coordinates, " + string.Format("min coord: ({0}, {1})", (object) minMaxPair1.Min, (object) minMaxPair2.Min));
      int num = (minMaxPair1.Min + minMaxPair2.Min).Max(0) / 2;
      this.TerrainWidth = minMaxPair1.Max + num + 1;
      this.TerrainHeight = minMaxPair2.Max + num + 1;
      for (int index = 0; index < this.Cells.Length; ++index)
      {
        MapCell cell = this.Cells[index];
        Assert.That<int>(cell.Id.Value).IsEqualTo(index, "Cell at index n should have ID equal to n.");
        cell.SetIslandMap(this);
      }
      this.Chunks = cells.SelectMany<Chunk2i>((Func<MapCell, IEnumerable<Chunk2i>>) (c => c.Chunks.AsEnumerable())).Distinct<Chunk2i>().OrderBy<Chunk2i, Chunk2i>((Func<Chunk2i, Chunk2i>) (x => x)).ToImmutableArray<Chunk2i>();
      Lyst<MapCell>[] lystArray = new Lyst<MapCell>[this.CellEdgePoints.Length];
      foreach (MapCell cell in this.Cells)
      {
        foreach (int perimeterIndex in cell.PerimeterIndices)
        {
          Lyst<MapCell> lyst = lystArray[perimeterIndex];
          if (lyst == null)
            lystArray[perimeterIndex] = lyst = new Lyst<MapCell>();
          lyst.Add(cell);
        }
      }
      Lyst<ITerrainResourceGenerator> lyst1 = new Lyst<ITerrainResourceGenerator>();
      Dict<MapCell, ImmutableArray<ITerrainResourceGenerator>> dict = new Dict<MapCell, ImmutableArray<ITerrainResourceGenerator>>();
      foreach (MapCell cell in cells)
      {
        if (!cell.EdgeTerrainFactory.IsNone)
        {
          lyst1.Clear();
          cell.EdgeTerrainFactory.Value.GenerateEdgeTerrainGeneratorsFor(cell, lyst1);
          dict.Add(cell, lyst1.OrderBy<ITerrainResourceGenerator, int>((Func<ITerrainResourceGenerator, int>) (x => x.Priority)).ToImmutableArray<ITerrainResourceGenerator>());
        }
      }
      this.CellEdgeTerrainGenerators = (IReadOnlyDictionary<MapCell, ImmutableArray<ITerrainResourceGenerator>>) dict;
      this.AllTerrainGenerators = this.ResourcesGenerators.AsEnumerable().Concat<ITerrainResourceGenerator>(dict.Values.SelectMany<ImmutableArray<ITerrainResourceGenerator>, ITerrainResourceGenerator>((Func<ImmutableArray<ITerrainResourceGenerator>, IEnumerable<ITerrainResourceGenerator>>) (x => x.AsEnumerable()))).OrderBy<ITerrainResourceGenerator, int>((Func<ITerrainResourceGenerator, int>) (x => x.Priority)).ToImmutableArray<ITerrainResourceGenerator>();
      foreach (ITerrainResource terrainGenerator in this.AllTerrainGenerators)
        terrainGenerator.Initialize(this);
      foreach (ITerrainResource virtualResource in this.VirtualResources)
        virtualResource.Initialize(this);
      this.initializeCaches(168, (DependencyResolver) null);
    }

    [InitAfterLoad(InitPriority.Highest)]
    private void initializeCaches(int saveVersion, DependencyResolver resolver)
    {
      Assert.That<Dict<Chunk2i, ImmutableArray<MapCell>>>(this.m_chunkToCells).IsEmpty<Chunk2i, ImmutableArray<MapCell>>();
      foreach (IGrouping<Chunk2i, KeyValuePair<Chunk2i, MapCell>> source in this.Cells.SelectMany<KeyValuePair<Chunk2i, MapCell>>((Func<MapCell, IEnumerable<KeyValuePair<Chunk2i, MapCell>>>) (cell => cell.Chunks.Select<KeyValuePair<Chunk2i, MapCell>>((Func<Chunk2i, KeyValuePair<Chunk2i, MapCell>>) (x => Make.Kvp<Chunk2i, MapCell>(x, cell))))).GroupBy<KeyValuePair<Chunk2i, MapCell>, Chunk2i>((Func<KeyValuePair<Chunk2i, MapCell>, Chunk2i>) (x => x.Key)))
        this.m_chunkToCells.Add(source.Key, source.Select<KeyValuePair<Chunk2i, MapCell>, MapCell>((Func<KeyValuePair<Chunk2i, MapCell>, MapCell>) (y => y.Value)).ToImmutableArray<MapCell>());
      if (saveVersion < 140)
        resolver.TryRegisterAdditionalInterface<IStartLocationProvider>((object) this).AssertTrue();
      if (saveVersion >= 161)
        return;
      resolver.TryRegisterAdditionalInterface<IMapInfoProvider>((object) this);
    }

    public bool IsValidCellId(MapCellId id) => (uint) id.Value < (uint) this.Cells.Length;

    public ImmutableArray<MapCell> GetCellsOnChunk(Chunk2i chunk)
    {
      ImmutableArray<MapCell> immutableArray;
      return !this.m_chunkToCells.TryGetValue(chunk, out immutableArray) ? ImmutableArray<MapCell>.Empty : immutableArray;
    }

    public MapCell GetClosestCell(Tile2i coord)
    {
      return this.Cells.MinElement<long>((Func<MapCell, long>) (x => x.CenterTile.DistanceSqrTo(coord)));
    }

    public static void Serialize(IslandMap value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<IslandMap>(value))
        return;
      writer.EnqueueDataSerialization((object) value, IslandMap.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      ImmutableArray<ITerrainResourceGenerator>.Serialize(this.AllTerrainGenerators, writer);
      writer.WriteGeneric<TerrainMaterialProto>(this.Bedrock);
      ImmutableArray<Tile2i>.Serialize(this.CellControlPoints, writer);
      ImmutableArray<Tile2i>.Serialize(this.CellEdgePoints, writer);
      writer.WriteGeneric<IReadOnlyDictionary<MapCell, ImmutableArray<ITerrainResourceGenerator>>>(this.CellEdgeTerrainGenerators);
      ImmutableArray<MapCell>.Serialize(this.Cells, writer);
      ImmutableArray<Chunk2i>.Serialize(this.Chunks, writer);
      IslandMapConfig.Serialize(this.Config, writer);
      ImmutableArray<Tile2i>.Serialize(this.ControlPoints, writer);
      IslandMapDifficultyConfig.Serialize(this.DifficultyConfig, writer);
      writer.WriteString(this.MapName);
      writer.WriteInt(this.MapVersion);
      ImmutableArray<Tile2i>.Serialize(this.NonCellControlPoints, writer);
      TerrainPropGenerationParams.Serialize(this.PropGenerationParams, writer);
      ImmutableArray<ITerrainResourceGenerator>.Serialize(this.ResourcesGenerators, writer);
      StartingLocation.Serialize(this.StartingLocation, writer);
      writer.WriteInt(this.TerrainHeight);
      ImmutableArray<ITerrainPostProcessor>.Serialize(this.TerrainPostProcessors, writer);
      ImmutableArray<TerrainPropMapData>.Serialize(this.TerrainProps, writer);
      writer.WriteInt(this.TerrainWidth);
      ImmutableArray<IVirtualTerrainResource>.Serialize(this.VirtualResources, writer);
    }

    public static IslandMap Deserialize(BlobReader reader)
    {
      IslandMap islandMap;
      if (reader.TryStartClassDeserialization<IslandMap>(out islandMap))
        reader.EnqueueDataDeserialization((object) islandMap, IslandMap.s_deserializeDataDelayedAction);
      return islandMap;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<IslandMap>(this, "AllTerrainGenerators", (object) ImmutableArray<ITerrainResourceGenerator>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "Bedrock", (object) reader.ReadGenericAs<TerrainMaterialProto>());
      reader.SetField<IslandMap>(this, "CellControlPoints", (object) ImmutableArray<Tile2i>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "CellEdgePoints", (object) ImmutableArray<Tile2i>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "CellEdgeTerrainGenerators", (object) reader.ReadGenericAs<IReadOnlyDictionary<MapCell, ImmutableArray<ITerrainResourceGenerator>>>());
      reader.SetField<IslandMap>(this, "Cells", (object) ImmutableArray<MapCell>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "Chunks", (object) ImmutableArray<Chunk2i>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "Config", (object) IslandMapConfig.Deserialize(reader));
      reader.SetField<IslandMap>(this, "ControlPoints", (object) ImmutableArray<Tile2i>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "DifficultyConfig", (object) IslandMapDifficultyConfig.Deserialize(reader));
      reader.SetField<IslandMap>(this, "m_chunkToCells", (object) new Dict<Chunk2i, ImmutableArray<MapCell>>());
      reader.SetField<IslandMap>(this, "MapName", (object) reader.ReadString());
      reader.SetField<IslandMap>(this, "MapVersion", (object) reader.ReadInt());
      reader.SetField<IslandMap>(this, "NonCellControlPoints", (object) ImmutableArray<Tile2i>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "PropGenerationParams", (object) TerrainPropGenerationParams.Deserialize(reader));
      reader.RegisterResolvedMember<IslandMap>(this, "ProtosDb", typeof (ProtosDb), true);
      reader.SetField<IslandMap>(this, "ResourcesGenerators", (object) ImmutableArray<ITerrainResourceGenerator>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "StartingLocation", (object) StartingLocation.Deserialize(reader));
      reader.SetField<IslandMap>(this, "TerrainHeight", (object) reader.ReadInt());
      reader.SetField<IslandMap>(this, "TerrainPostProcessors", (object) ImmutableArray<ITerrainPostProcessor>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "TerrainProps", (object) ImmutableArray<TerrainPropMapData>.Deserialize(reader));
      reader.SetField<IslandMap>(this, "TerrainWidth", (object) reader.ReadInt());
      reader.SetField<IslandMap>(this, "VirtualResources", (object) ImmutableArray<IVirtualTerrainResource>.Deserialize(reader));
      reader.RegisterInitAfterLoad<IslandMap>(this, "initializeCaches", InitPriority.Highest);
    }

    static IslandMap()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      IslandMap.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((IslandMap) obj).SerializeData(writer));
      IslandMap.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((IslandMap) obj).DeserializeData(reader));
    }
  }
}
