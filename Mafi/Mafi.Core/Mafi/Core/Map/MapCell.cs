// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.MapCell
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Terrain.Generation;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Map
{
  /// <summary>
  /// A polygon that specifies an area on the map that is specified by center tile and perimeter tiles (vertices). Each
  /// cell should be convex. Cells form a graph through <see cref="P:Mafi.Core.Map.MapCell.Neighbors" />.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class MapCell : 
    IMapCellFriend,
    IMapCellGeneratorFriend,
    IEquatable<MapCell>,
    IIsSafeAsHashKey
  {
    public static readonly HeightTilesI MIN_GROUND_HEIGHT;
    public static readonly HeightTilesI DEFAULT_OCEAN_FLOOR_HEIGHT;
    /// <summary>
    /// Cell ID is also an index to a consecutive array of all cells.
    /// </summary>
    public readonly MapCellId Id;
    /// <summary>
    /// Index into <see cref="F:Mafi.Core.Map.IslandMap.ControlPoints" /> of tile in the center of this cell.
    /// </summary>
    public readonly int CenterPointIndex;
    /// <summary>
    /// Indices of the perimeter points. These indices are pointing to <see cref="F:Mafi.Core.Map.IslandMap.CellEdgePoints" />.
    /// Indices at [i, i+ 1] are shared with neighbor cell at index i.
    /// </summary>
    public readonly ImmutableArray<int> PerimeterIndices;
    /// <summary>
    /// Neighboring cell IDs in counterclockwise order around the center tile.
    /// </summary>
    public readonly ImmutableArray<MapCellId?> NeighborCellsIndices;
    /// <summary>
    /// Only valid neighboring cell indices. Note that the order does not correlate with <see cref="F:Mafi.Core.Map.MapCell.PerimeterIndices" /> anymore.
    /// </summary>
    public readonly ImmutableArray<MapCellId> NeighborCellsValidIndices;
    /// <summary>
    /// Whether this cell is on map boundary (some neighbors are `None`).
    /// </summary>
    public readonly bool IsOnMapBoundary;
    public readonly bool DisableHeightBiasFromConfig;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public IslandMap IslandMap { get; private set; }

    /// <summary>Tile in the center of this cell.</summary>
    public Tile2i CenterTile
    {
      get
      {
        Assert.That<IslandMap>(this.IslandMap).IsNotNull<IslandMap>("CenterTile can be queried only when cell is added to IslandMap. Use CenterPointIndex instead.");
        return this.IslandMap.ControlPoints[this.CenterPointIndex];
      }
    }

    public Tile3i CenterTileWithHeight => this.CenterTile.ExtendHeight(this.GroundHeight);

    public ImmutableArray<Option<MapCell>> Neighbors { get; private set; }

    public ImmutableArray<MapCell> ValidNeighbors { get; private set; }

    public bool IsNotOnMapBoundary => !this.IsOnMapBoundary;

    /// <summary>Whether this cell is ocean.</summary>
    public bool IsOcean { get; private set; }

    public bool IsNotOcean => !this.IsOcean;

    /// <summary>
    /// Whether at least one neighbor is ocean cell. Note: This value is not valid until the cell generation is
    /// finalized.
    /// </summary>
    public bool IsNextToOcean { get; private set; }

    /// <summary>
    /// Coordinates of all terrain chunks that have at least one tile in this cell.
    /// </summary>
    public ImmutableArray<Chunk2i> Chunks { get; private set; }

    /// <summary>
    /// A radius of a circle with center in the cell center that fits the whole cell.
    /// </summary>
    public RelTile1f OuterRadius { get; private set; }

    /// <summary>
    /// A radius of a circle with center in the cell center that fits inside of the cell without sticking out.
    /// </summary>
    public RelTile1f InnerRadius { get; private set; }

    public MapCellState State { get; private set; }

    public Option<IMapCellEdgeTerrainFactory> EdgeTerrainFactory { get; private set; }

    public bool IsUnlocked => this.State == MapCellState.Unlocked;

    public bool IsAvailableToUnlock => this.State == MapCellState.AvailableToUnlock;

    /// <summary>Base ground height of this cell.</summary>
    public HeightTilesI GroundHeight { get; private set; }

    public ICellSurfaceGenerator SurfaceGenerator { get; private set; }

    public bool IsUnlockedByDefault { get; private set; }

    internal bool IsInitialized => this.IslandMap != null;

    public MapCell(
      MapCellId id,
      int centerPointIndex,
      ImmutableArray<int> perimeterIndices,
      ImmutableArray<MapCellId?> neighborCellsIndices,
      bool isOnMapBoundary,
      bool disableHeightBiasFromConfig = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CIsUnlockedByDefault\u003Ek__BackingField = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(id.Value).IsNotNegative("Negative map cell ID");
      Assert.That<ImmutableArray<int>>(perimeterIndices).HasLength<int>(neighborCellsIndices.Length);
      this.Id = id;
      this.CenterPointIndex = centerPointIndex;
      this.PerimeterIndices = perimeterIndices.CheckNotEmpty<int>();
      this.NeighborCellsIndices = neighborCellsIndices.CheckNotEmpty<MapCellId?>();
      this.NeighborCellsValidIndices = neighborCellsIndices.Where((Func<MapCellId?, bool>) (x => x.HasValue)).Select<MapCellId?, MapCellId>((Func<MapCellId?, MapCellId>) (x => x.Value)).ToImmutableArray<MapCellId>();
      this.IsOnMapBoundary = isOnMapBoundary;
      this.DisableHeightBiasFromConfig = disableHeightBiasFromConfig;
    }

    [CSharpGenCtor]
    public MapCell(
      MapCellId id,
      int centerPointIndex,
      ImmutableArray<int> perimeterIndices,
      ImmutableArray<MapCellId?> neighborCellsIndices,
      bool isOnMapBoundary,
      bool disableHeightBiasFromConfig,
      bool isOcean,
      bool isUnlockedByDefault,
      HeightTilesI groundHeight,
      ICellSurfaceGenerator surfaceGenerator,
      MapCellState state,
      Option<IMapCellEdgeTerrainFactory> edgeTerrainFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(id, centerPointIndex, perimeterIndices, neighborCellsIndices, isOnMapBoundary, disableHeightBiasFromConfig);
      this.IsOcean = isOcean;
      this.IsUnlockedByDefault = isUnlockedByDefault;
      this.GroundHeight = groundHeight;
      this.SurfaceGenerator = surfaceGenerator;
      this.State = state;
      this.EdgeTerrainFactory = edgeTerrainFactory;
    }

    internal void SetIslandMap(IslandMap islandMap)
    {
      if (this.SurfaceGenerator == null)
        throw new InvalidOperationException(string.Format("Surface generator was not set on cell {0}.", (object) this));
      if (this.IslandMap != null)
      {
        Log.Error(string.Format("World map cell {0} is already initialized.", (object) this.Id));
      }
      else
      {
        this.IslandMap = islandMap.CheckNotNull<IslandMap>();
        if (!this.DisableHeightBiasFromConfig && islandMap.DifficultyConfig.CellHeightsBias.IsNotZero && this.GroundHeight > MapCell.MIN_GROUND_HEIGHT)
        {
          this.GroundHeight += new ThicknessTilesI(islandMap.DifficultyConfig.CellHeightsBias.ApplyToFix32(this.GroundHeight.Value).ToIntRounded());
          if (this.GroundHeight < MapCell.MIN_GROUND_HEIGHT)
            this.GroundHeight = MapCell.MIN_GROUND_HEIGHT;
        }
        this.Neighbors = this.NeighborCellsIndices.Map<Option<MapCell>>((Func<MapCellId?, Option<MapCell>>) (i => !i.HasValue ? Option<MapCell>.None : (Option<MapCell>) islandMap[i.Value]));
        this.ValidNeighbors = this.NeighborCellsValidIndices.Map<MapCell>((Func<MapCellId, MapCell>) (i => islandMap[i]));
        Tile2i center = this.CenterTile;
        Tile2f center2f = center.CornerTile2f;
        this.InnerRadius = new RelTile1f(this.EnumerateEdges().Min<MapCellEdge, Fix64>((Func<MapCellEdge, Fix64>) (edge => edge.CenterPoint.DistanceSqrTo(center2f))).SqrtToFix32());
        this.OuterRadius = new RelTile1f(this.PerimeterIndices.Max<long>((Func<int, long>) (i => islandMap.CellEdgePoints[i].DistanceSqrTo(center))).SqrtToFix32() + Fix32.EpsilonNear);
        this.Chunks = this.computeChunksFromPerimeter().CheckNotEmpty<Chunk2i>();
        this.IsNextToOcean = this.ValidNeighbors.Any((Func<MapCell, bool>) (x => x.IsOcean));
        Assert.That<bool>(this.Contains(this.CenterTile)).IsTrue<MapCellId>("Invalid map cell {0}: Center is not contained in the cell or perimeter is not counter-clock wise.", this.Id);
      }
    }

    internal void HACK_ClearIslandMap() => this.IslandMap = (IslandMap) null;

    /// <summary>
    /// Helper function that computes all the chunks of this cell.
    /// </summary>
    /// <remarks>This is intentionally a static function because it is called from constructor.</remarks>
    private ImmutableArray<Chunk2i> computeChunksFromPerimeter()
    {
      Set<Chunk2i> source = new Set<Chunk2i>();
      foreach (Tile2i enumeratePerimeterTile in this.EnumeratePerimeterTiles())
        source.Add(enumeratePerimeterTile.ChunkCoord2i);
      foreach (IGrouping<int, Chunk2i> enumerable in source.GroupBy<Chunk2i, int>((Func<Chunk2i, int>) (c => c.X)))
      {
        Lyst<Chunk2i> lyst = enumerable.ToLyst<Chunk2i>();
        if (lyst.Count != 1)
        {
          int num1 = lyst.Min<Chunk2i>((Func<Chunk2i, int>) (c => c.Y));
          int num2 = lyst.Max<Chunk2i>((Func<Chunk2i, int>) (c => c.Y));
          for (int y = num1 + 1; y < num2; ++y)
            source.Add(new Chunk2i(enumerable.Key, y));
        }
      }
      return source.OrderBy<Chunk2i, int>((Func<Chunk2i, int>) (c => c.X)).ThenBy<Chunk2i, int>((Func<Chunk2i, int>) (c => c.Y)).ToImmutableArray<Chunk2i>();
    }

    /// <summary>
    /// Enumerates all tiles around the perimeter of this cell in counter-clock wise order. Returned tiles are
    /// computed on the fly with enumerator semantics. It is possible that some tiles repeat if the edges are
    /// "pointy".Memory complexity is O(1).
    /// </summary>
    public IEnumerable<Tile2i> EnumeratePerimeterTiles()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tile2i>) new MapCell.\u003CEnumeratePerimeterTiles\u003Ed__80(-2)
      {
        \u003C\u003E4__this = this
      };
    }

    void IMapCellGeneratorFriend.SetIsOcean(bool isOcean)
    {
      if (this.IsInitialized)
        throw new InvalidOperationException(string.Format("Failed to SetIsOcean. Cell {0} is already generated and initialized.", (object) this.Id));
      this.IsOcean = isOcean;
    }

    void IMapCellFriend.SetState(MapCellState newState)
    {
      Assert.That<bool>(this.State < newState).IsTrue<MapCellState, MapCellState>("Invalid map cell state transition from {0} to {1}.", this.State, newState);
      this.State = newState;
    }

    public void ResetState() => this.State = MapCellState.NotAvailable;

    void IMapCellGeneratorFriend.SetState(MapCellState newState)
    {
      if (this.IsInitialized)
        throw new InvalidOperationException(string.Format("Failed to SetState. Cell {0} is already generated and initialized.", (object) this.Id));
      this.State = newState;
    }

    void IMapCellGeneratorFriend.SetHeight(HeightTilesI height)
    {
      if (this.IsInitialized)
        throw new InvalidOperationException(string.Format("Failed to SetHeight. Cell {0} is already generated and initialized.", (object) this.Id));
      this.GroundHeight = height;
    }

    void IMapCellGeneratorFriend.SetSurfaceGenerator(ICellSurfaceGenerator surfaceGenerator)
    {
      if (this.IsInitialized)
        throw new InvalidOperationException(string.Format("Failed to SetSurfaceGenerator. Cell {0} is already generated and initialized.", (object) this.Id));
      this.SurfaceGenerator = surfaceGenerator.CheckNotNull<ICellSurfaceGenerator>();
    }

    void IMapCellGeneratorFriend.SetEdgeTerrainFactory(
      Option<IMapCellEdgeTerrainFactory> edgeTerrainFactory)
    {
      this.EdgeTerrainFactory = edgeTerrainFactory;
    }

    void IMapCellGeneratorFriend.SetIsUnlockedByDefault(bool isUnlockedByDefault)
    {
      if (this.IsInitialized)
        throw new InvalidOperationException(string.Format("Failed to SetIsUnlockedByDefault. Cell {0} is already generated and initialized.", (object) this.Id));
      this.IsUnlockedByDefault = isUnlockedByDefault;
    }

    public MapCellId? GetNeighborAt(int edgeIndex)
    {
      if ((long) (uint) edgeIndex < (long) this.NeighborCellsIndices.Length)
        return this.NeighborCellsIndices[edgeIndex];
      Log.Error(string.Format("Invalid neighbor index {0} of cell {1} with {2} neighbors.", (object) edgeIndex, (object) this.Id, (object) this.NeighborCellsIndices.Length));
      return new MapCellId?();
    }

    public int GetNeighborIndex(MapCell cell)
    {
      return this.NeighborCellsIndices.IndexOf(new MapCellId?(cell.Id));
    }

    /// <summary>
    /// Returns edge to given neighbor index. This cell is to the left of returned line.
    /// </summary>
    public Line2i GetEdgeToNeighbor(int index)
    {
      Assert.That<int>(this.PerimeterIndices.Length).IsEqualTo(this.NeighborCellsIndices.Length);
      ImmutableArray<Tile2i> cellEdgePoints = this.IslandMap.CellEdgePoints;
      Tile2i tile2i = cellEdgePoints[this.PerimeterIndices[index]];
      Vector2i vector2i1 = tile2i.Vector2i;
      tile2i = cellEdgePoints[this.PerimeterIndices[index + 1 == this.PerimeterIndices.Length ? 0 : index + 1]];
      Vector2i vector2i2 = tile2i.Vector2i;
      return new Line2i(vector2i1, vector2i2);
    }

    /// <summary>
    /// Returns edge line to given neighbor cell. This cell is to the left of returned line, the <paramref name="neighborCell" /> is to the right.
    /// </summary>
    public Line2i GetEdgeToNeighbor(MapCell neighborCell)
    {
      return this.GetEdgeToNeighbor(this.GetNeighborIndex(neighborCell));
    }

    /// <summary>
    /// Returns edge length between this cell and given neighbor. Given cell must be in <see cref="P:Mafi.Core.Map.MapCell.Neighbors" />.
    /// Returned value is always positive.
    /// </summary>
    public RelTile1f GetNeighborEdgeLength(MapCell neighborCell)
    {
      RelTile1f neighborEdgeLength = new RelTile1f(this.GetEdgeToNeighbor(this.GetNeighborIndex(neighborCell)).SegmentLength);
      Assert.That<RelTile1f>(neighborEdgeLength).IsPositive();
      return neighborEdgeLength;
    }

    /// <summary>
    /// Whether given point is inside of this cell. Note that this computation is mathematically exact, thus, each
    /// tile can be contained only in one cell, except, when it is exactly on the boundary between two cells. This
    /// does not match how <see cref="M:Mafi.Core.Map.MapCell.EnumeratePerimeterTiles" /> works.
    /// </summary>
    /// <remarks>This test assumes that perimeter is in counter-clock order.</remarks>
    public bool Contains(Tile2i tile)
    {
      if (tile.DistanceSqrTo(this.CenterTile) > this.OuterRadius.Squared)
        return false;
      ImmutableArray<Tile2i> cellEdgePoints = this.IslandMap.CellEdgePoints;
      Tile2i tile2i1 = cellEdgePoints[this.PerimeterIndices.Last];
      foreach (int perimeterIndex in this.PerimeterIndices)
      {
        Tile2i tile2i2 = cellEdgePoints[perimeterIndex];
        if ((tile2i2 - tile2i1).PseudoCross(tile - tile2i1) < 0L)
          return false;
        tile2i1 = tile2i2;
      }
      return true;
    }

    /// <summary>
    /// Computes signed distance to the boundary. This distance is positive for points outside of this cell and
    /// negative for points inside of this cell.
    /// </summary>
    public Fix32 SignedBoundaryDistanceTo(Tile2i tile)
    {
      Fix32 fix32 = this.BoundaryDistanceSqrTo(tile).SqrtToFix32();
      return !this.Contains(tile) ? fix32 : -fix32;
    }

    /// <summary>
    /// Computes signed squared distance to the boundary. This distance is positive for points outside of this cell
    /// and negative for points inside of this cell. This is more efficient than <see cref="M:Mafi.Core.Map.MapCell.SignedBoundaryDistanceTo(Mafi.Tile2i)" /> as it avoids square root computation.
    /// </summary>
    public long SignedBoundaryDistanceSqrTo(Tile2i tile)
    {
      long num = this.BoundaryDistanceSqrTo(tile);
      return !this.Contains(tile) ? num : -num;
    }

    /// <summary>
    /// Computes squared distance to the boundary. This distance is always positive.
    /// </summary>
    public long BoundaryDistanceSqrTo(Tile2i tile)
    {
      ImmutableArray<Tile2i> cellEdgePoints = this.IslandMap.CellEdgePoints;
      long num = long.MaxValue;
      Vector2i p0 = cellEdgePoints[this.PerimeterIndices.Last].Vector2i;
      Vector2i vector2i1 = tile.Vector2i;
      foreach (int perimeterIndex in this.PerimeterIndices)
      {
        Vector2i vector2i2 = cellEdgePoints[perimeterIndex].Vector2i;
        long lineSegmentApprox = new Line2i(p0, vector2i2).DistanceSqrToLineSegmentApprox(vector2i1);
        if (lineSegmentApprox < num)
          num = lineSegmentApprox;
        p0 = vector2i2;
      }
      return num;
    }

    public Tile2f ClosestBoundaryPoint(Tile2i tile)
    {
      ImmutableArray<Tile2i> cellEdgePoints = this.IslandMap.CellEdgePoints;
      Fix64 fix64_1 = Fix64.MaxValue;
      Vector2i p0 = cellEdgePoints[this.PerimeterIndices.Last].Vector2i;
      Vector2i vector2i1 = tile.Vector2i;
      Vector2i vector2i2 = vector2i1;
      foreach (int perimeterIndex in this.PerimeterIndices)
      {
        Vector2i vector2i3 = cellEdgePoints[perimeterIndex].Vector2i;
        Vector2i lineSegment = new Line2i(p0, vector2i3).ClosestPointToLineSegment(vector2i1);
        Fix64 fix64_2 = (Fix64) vector2i1.DistanceSqrTo(lineSegment);
        if (fix64_2 < fix64_1)
        {
          fix64_1 = fix64_2;
          vector2i2 = lineSegment;
        }
        p0 = vector2i3;
      }
      return new Tile2f(vector2i2.Vector2f);
    }

    /// <summary>
    /// Returns approximate distance to given chunk. Returned value may be negative if the chunk is in the cell.
    /// </summary>
    public Fix32 SignedBoundaryDistanceTo(Chunk2i chunkCoord)
    {
      return this.SignedBoundaryDistanceTo(chunkCoord + TileInChunk2i.FromMinusX(0, 0)).Min(this.SignedBoundaryDistanceTo(chunkCoord.Tile2i)).Min(this.SignedBoundaryDistanceTo(chunkCoord + TileInChunk2i.FromMinusY(0, 0)).Min(this.SignedBoundaryDistanceTo(chunkCoord + TileInChunk2i.FromMinusXy(0, 0)))).Min(this.SignedBoundaryDistanceTo(chunkCoord.CenterTile2i));
    }

    /// <summary>
    /// Enumerates all tiles in this cell. Tiles computed on the fly so caller can enumerate them in time-sliced
    /// manner.
    /// </summary>
    /// <remarks>
    /// While this returns tiles in time-sliced manner the resulting algorithm has to cache all the tiles along the
    /// way so memory complexity is O(number of returned tiles).
    /// </remarks>
    public IEnumerable<Tile2i> EnumerateTiles()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<Tile2i>) new MapCell.\u003CEnumerateTiles\u003Ed__100(-2)
      {
        \u003C\u003E4__this = this
      };
    }

    public IEnumerable<MapCellEdge> EnumerateEdges()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<MapCellEdge>) new MapCell.\u003CEnumerateEdges\u003Ed__101(-2)
      {
        \u003C\u003E4__this = this
      };
    }

    public bool Equals(MapCell other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (!this.Id.Equals(other.Id))
        return false;
      Log.Error(string.Format("Map cells that are not equal references have equal IDs: {0}", (object) this.Id));
      return true;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      if (!(obj is MapCell mapCell) || !this.Id.Equals(mapCell.Id))
        return false;
      Log.Error(string.Format("Map cells that are not equal references have equal IDs: {0}", (object) this.Id));
      return true;
    }

    public override int GetHashCode() => this.Id.GetHashCode();

    public static void Serialize(MapCell value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MapCell>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MapCell.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.CenterPointIndex);
      ImmutableArray<Chunk2i>.Serialize(this.Chunks, writer);
      writer.WriteBool(this.DisableHeightBiasFromConfig);
      Option<IMapCellEdgeTerrainFactory>.Serialize(this.EdgeTerrainFactory, writer);
      HeightTilesI.Serialize(this.GroundHeight, writer);
      MapCellId.Serialize(this.Id, writer);
      RelTile1f.Serialize(this.InnerRadius, writer);
      IslandMap.Serialize(this.IslandMap, writer);
      writer.WriteBool(this.IsNextToOcean);
      writer.WriteBool(this.IsOcean);
      writer.WriteBool(this.IsOnMapBoundary);
      writer.WriteBool(this.IsUnlockedByDefault);
      ImmutableArray<MapCellId?>.Serialize(this.NeighborCellsIndices, writer);
      ImmutableArray<MapCellId>.Serialize(this.NeighborCellsValidIndices, writer);
      ImmutableArray<Option<MapCell>>.Serialize(this.Neighbors, writer);
      RelTile1f.Serialize(this.OuterRadius, writer);
      ImmutableArray<int>.Serialize(this.PerimeterIndices, writer);
      writer.WriteInt((int) this.State);
      writer.WriteGeneric<ICellSurfaceGenerator>(this.SurfaceGenerator);
      ImmutableArray<MapCell>.Serialize(this.ValidNeighbors, writer);
    }

    public static MapCell Deserialize(BlobReader reader)
    {
      MapCell mapCell;
      if (reader.TryStartClassDeserialization<MapCell>(out mapCell))
        reader.EnqueueDataDeserialization((object) mapCell, MapCell.s_deserializeDataDelayedAction);
      return mapCell;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<MapCell>(this, "CenterPointIndex", (object) reader.ReadInt());
      this.Chunks = ImmutableArray<Chunk2i>.Deserialize(reader);
      reader.SetField<MapCell>(this, "DisableHeightBiasFromConfig", (object) reader.ReadBool());
      this.EdgeTerrainFactory = Option<IMapCellEdgeTerrainFactory>.Deserialize(reader);
      this.GroundHeight = HeightTilesI.Deserialize(reader);
      reader.SetField<MapCell>(this, "Id", (object) MapCellId.Deserialize(reader));
      this.InnerRadius = RelTile1f.Deserialize(reader);
      this.IslandMap = IslandMap.Deserialize(reader);
      this.IsNextToOcean = reader.ReadBool();
      this.IsOcean = reader.ReadBool();
      reader.SetField<MapCell>(this, "IsOnMapBoundary", (object) reader.ReadBool());
      this.IsUnlockedByDefault = reader.ReadBool();
      reader.SetField<MapCell>(this, "NeighborCellsIndices", (object) ImmutableArray<MapCellId?>.Deserialize(reader));
      reader.SetField<MapCell>(this, "NeighborCellsValidIndices", (object) ImmutableArray<MapCellId>.Deserialize(reader));
      this.Neighbors = ImmutableArray<Option<MapCell>>.Deserialize(reader);
      this.OuterRadius = RelTile1f.Deserialize(reader);
      reader.SetField<MapCell>(this, "PerimeterIndices", (object) ImmutableArray<int>.Deserialize(reader));
      this.State = (MapCellState) reader.ReadInt();
      this.SurfaceGenerator = reader.ReadGenericAs<ICellSurfaceGenerator>();
      this.ValidNeighbors = ImmutableArray<MapCell>.Deserialize(reader);
    }

    static MapCell()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MapCell.MIN_GROUND_HEIGHT = new HeightTilesI(2);
      MapCell.DEFAULT_OCEAN_FLOOR_HEIGHT = new HeightTilesI(-5);
      MapCell.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MapCell) obj).SerializeData(writer));
      MapCell.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MapCell) obj).DeserializeData(reader));
    }
  }
}
