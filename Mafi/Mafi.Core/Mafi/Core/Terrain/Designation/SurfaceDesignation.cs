// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.SurfaceDesignation
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  /// <summary>Surface designation.</summary>
  [GenerateSerializer(false, null, 0)]
  public class SurfaceDesignation : IDesignation, IIsSafeAsHashKey, IEquatable<SurfaceDesignation>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>Designations size in bits.</summary>
    public const int SIZE_BITS = 2;
    public const int SIZE_MASK = 3;
    public const int SIZE_TILES = 4;
    public const int BITS_PER_CHUNK_EDGE = 4;
    public const int DESIGNATIONS_PER_CHUNK_EDGE = 16;
    public const int DESIGNATIONS_PER_CHUNK_EDGE_MASK = 15;
    public const int MASK_LOCAL_COORD = 15;
    /// <summary>Number of designations per terrain chunk.</summary>
    public const int DESIGNATIONS_PER_CHUNK = 256;
    /// <summary>
    /// Total number of affected tiles by a designation considering not counting overlapping tiles.
    /// </summary>
    public const int AREA_TILES = 16;
    /// <summary>
    /// How far from the terrain can a surface be when placed.
    /// </summary>
    public static readonly HeightTilesF SURFACE_HEIGHT_TOLERANCE;
    /// <summary>Prototype of this designation.</summary>
    public readonly SurfaceDesignationProto Prototype;
    public readonly ISurfaceDesignationsManagerInternal Manager;
    public readonly TileSurfaceSlimId SurfaceProtoSlimId;
    /// <summary>
    /// Bit is set when a tile is fulfilled. All unused bits are zero.
    /// </summary>
    private uint m_tilesFulfilledBitmap;

    public static void Serialize(SurfaceDesignation value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SurfaceDesignation>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SurfaceDesignation.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsDestroyed);
      writer.WriteUInt(this.m_tilesFulfilledBitmap);
      writer.WriteGeneric<ISurfaceDesignationsManagerInternal>(this.Manager);
      Tile2iSlim.Serialize(this.OriginTile, writer);
      writer.WriteGeneric<SurfaceDesignationProto>(this.Prototype);
      TileSurfaceSlimId.Serialize(this.SurfaceProtoSlimId, writer);
      writer.WriteULong(this.SurfaceTypeMap);
      writer.WriteUInt(this.UnassignedTilesBitmap);
    }

    public static SurfaceDesignation Deserialize(BlobReader reader)
    {
      SurfaceDesignation surfaceDesignation;
      if (reader.TryStartClassDeserialization<SurfaceDesignation>(out surfaceDesignation))
        reader.EnqueueDataDeserialization((object) surfaceDesignation, SurfaceDesignation.s_deserializeDataDelayedAction);
      return surfaceDesignation;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsDestroyed = reader.ReadBool();
      this.m_tilesFulfilledBitmap = reader.ReadUInt();
      reader.SetField<SurfaceDesignation>(this, "Manager", (object) reader.ReadGenericAs<ISurfaceDesignationsManagerInternal>());
      this.OriginTile = Tile2iSlim.Deserialize(reader);
      reader.SetField<SurfaceDesignation>(this, "Prototype", (object) reader.ReadGenericAs<SurfaceDesignationProto>());
      reader.SetField<SurfaceDesignation>(this, "SurfaceProtoSlimId", (object) TileSurfaceSlimId.Deserialize(reader));
      this.SurfaceTypeMap = reader.ReadULong();
      this.UnassignedTilesBitmap = reader.ReadUInt();
    }

    public LocStrFormatted Name => (LocStrFormatted) this.Prototype.Strings.Name;

    public int SizeTiles => 4;

    public static RelTile2i Size => new RelTile2i(4, 4);

    /// <summary>
    /// Returns origin tile of designation from given position.
    /// </summary>
    [Pure]
    public static Tile2i GetOrigin(Tile2i position) => new Tile2i(position.X & -4, position.Y & -4);

    [Pure]
    public static RelTile2i GetRelCoordWithin(Tile2i position)
    {
      return new RelTile2i(position.X & 3, position.Y & 3);
    }

    /// <summary>
    /// Returns center tile of a designation given an origin tile.
    /// </summary>
    [Pure]
    public static Tile2i GetCenterTileForOrigin(Tile2i origin) => origin.AddXy(2);

    public bool IsPlacing => this.SurfaceProtoSlimId.IsNotPhantom;

    public Proto.ID ProtoId => this.Prototype.Id;

    /// <summary>
    /// Origin tile coord of this designation. Origin is always at whole multiples of <c>SIZE_TILES</c>.
    /// </summary>
    public Tile2i OriginTileCoord => (Tile2i) this.OriginTile;

    public int WithinChunkRelIndex
    {
      get => SurfaceDesignationData.GetWithinChunkRelIndex((Tile2i) this.OriginTile);
    }

    public Chunk2i ChunkCoord => this.OriginTile.ChunkCoord2i;

    /// <summary>Vertex at the center of this designation.</summary>
    public Tile2i CenterTileCoord => this.OriginTileCoord.AddXy(2);

    public RectangleTerrainArea2i Area
    {
      get => new RectangleTerrainArea2i((Tile2i) this.OriginTile, SurfaceDesignation.Size);
    }

    /// <summary>Enumeration helper to enumerate all fulfilled tiles</summary>
    public SurfaceDesignation.EnumeratorHolder AllFulfilled
    {
      get => new SurfaceDesignation.EnumeratorHolder(this, true);
    }

    /// <summary>
    /// Enumeration helper to enumerate all non-fulfilled tiles
    /// </summary>
    public SurfaceDesignation.EnumeratorHolder AllNonFulfilled
    {
      get => new SurfaceDesignation.EnumeratorHolder(this, false);
    }

    /// <summary>
    /// Whether this designation was destroyed and is no longer valid.
    /// </summary>
    public bool IsDestroyed { get; private set; }

    public Tile2iSlim OriginTile { get; private set; }

    /// <summary>
    /// Number of jobs that are taking care of fulfilling this designation.
    /// </summary>
    [DoNotSave(0, null)]
    public int NumberOfJobsAssigned { get; internal set; }

    /// <summary>
    /// Number of vehicles that recently failed to reach this designation.
    /// Should be only set by unreachable manager.
    /// </summary>
    [DoNotSave(0, null)]
    public ushort UnreachableVehiclesCount { get; set; }

    /// <summary>
    /// Bit is set when a tile is fulfilled. All unused bits are zero.
    /// </summary>
    public uint TilesFulfilledBitmap => this.m_tilesFulfilledBitmap;

    /// <summary>Whether this designation is fulfilled.</summary>
    public bool IsFulfilled
    {
      get => this.IsNotAssigned || this.m_tilesFulfilledBitmap == (uint) ushort.MaxValue;
    }

    public bool IsNotFulfilled
    {
      get => !this.IsNotAssigned && this.m_tilesFulfilledBitmap != (uint) ushort.MaxValue;
    }

    /// <summary>
    /// Whether the designation has any assignments. This does not mean jobs!
    /// </summary>
    public bool IsNotAssigned
    {
      get => ((int) this.UnassignedTilesBitmap & (int) ushort.MaxValue) == (int) ushort.MaxValue;
    }

    /// <summary>
    /// 16-bits used, each bit representing whether a tile at (y * 4 + x) is assigned (0) or unassigned (1).
    /// </summary>
    public uint UnassignedTilesBitmap { get; private set; }

    /// <summary>
    /// 4 bits per tile giving the target surface. 0 =&gt; clear.
    /// </summary>
    public ulong SurfaceTypeMap { get; private set; }

    /// <summary>Whether this designation is ready to be fulfilled.</summary>
    public bool IsReadyToBeFulfilled => this.IsNotFulfilled;

    public SurfaceDesignation(
      ISurfaceDesignationsManagerInternal manager,
      SurfaceDesignationProto prototype,
      SurfaceDesignationData data)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Manager = manager;
      this.Prototype = prototype.CheckNotNull<SurfaceDesignationProto>();
      this.OriginTile = data.OriginTile;
      this.SurfaceProtoSlimId = data.SurfaceProtoSlimId;
      this.UnassignedTilesBitmap = (uint) data.UnassignedTilesBitmap;
      if (this.SurfaceProtoSlimId.IsNotPhantom)
      {
        Assert.That<int>((int) this.SurfaceProtoSlimId.Value).IsLessOrEqual(15);
        for (int index1 = 0; index1 < 4; ++index1)
        {
          for (int index2 = 0; index2 < 4; ++index2)
            this.SurfaceTypeMap |= (ulong) this.SurfaceProtoSlimId.Value << (index1 * 4 + index2) * 4;
        }
      }
      this.ForEachTile(new Action<Tile2iAndIndex>(this.updateBitmaps));
    }

    internal void InitNumberOfJobsAssigned(int numberOfJobsAssigned)
    {
      this.NumberOfJobsAssigned = numberOfJobsAssigned;
    }

    internal void SetDestroyed()
    {
      if (this.NumberOfJobsAssigned > 0)
      {
        IIndexable<IVehicleJobWithOwner> assignedJobsFor = this.Manager.GetAssignedJobsFor(this);
        if (assignedJobsFor.FirstOrDefault<IVehicleJobWithOwner>() is SurfaceModificationJob surfaceModificationJob)
        {
          surfaceModificationJob.CancelCurrentSurface(this);
          assignedJobsFor = this.Manager.GetAssignedJobsFor(this);
        }
        foreach (IVehicleJob immutable in assignedJobsFor.ToImmutableArray<IVehicleJobWithOwner>())
          immutable.RequestCancel();
        Assert.That<IIndexable<IVehicleJobWithOwner>>(assignedJobsFor).IsEmpty<IVehicleJobWithOwner>();
      }
      this.IsDestroyed = true;
    }

    /// <summary>
    /// Called by <see cref="T:Mafi.Core.Terrain.Designation.ISurfaceDesignationsManager" /> when a tile under this designation is changed.
    /// </summary>
    public virtual void TileChanged(Tile2iAndIndex tileAndIndex)
    {
      this.updateBitmaps(tileAndIndex);
    }

    public bool IsAssignedTo(IVehicleJobWithOwner job)
    {
      return this.Manager.GetAssignedJobsFor(this).Contains<IVehicleJobWithOwner>(job);
    }

    /// <summary>Whether this designation contains given position.</summary>
    public bool ContainsPosition(Tile2i position)
    {
      return position >= this.OriginTileCoord && position < this.OriginTileCoord + new RelTile2i(4, 4);
    }

    public bool IsAssignedAt(Tile2i coord)
    {
      return this.UnassignedTilesBitmap == 0U || ((int) this.UnassignedTilesBitmap & (int) this.getMask(coord)) == 0;
    }

    public bool IsAssignedAt(RelTile2i coord)
    {
      return this.UnassignedTilesBitmap == 0U || ((int) this.UnassignedTilesBitmap & (int) SurfaceDesignation.getMask(coord)) == 0;
    }

    public bool IsFulfilledAt(Tile2i coord)
    {
      return ((int) this.m_tilesFulfilledBitmap & (int) this.getMask(coord)) != 0 || !this.IsAssignedAt(coord);
    }

    public bool IsFulfilledAt(RelTile2i coord)
    {
      return ((int) this.m_tilesFulfilledBitmap & (int) SurfaceDesignation.getMask(coord)) != 0 || !this.IsAssignedAt(coord);
    }

    public LocStrFormatted? GetWarningStr() => new LocStrFormatted?();

    /// <summary>
    /// Returns designation height for a given position. Requested position should lie in this designation.
    /// </summary>
    public HeightTilesF GetTargetHeightAt(Tile2i position)
    {
      Assert.That<bool>(this.ContainsPosition(position)).IsTrue<Tile2i, SurfaceDesignation>("Tile at {0} is outside of area '{1}'.", position, this);
      return this.Manager.TerrainManager.GetHeight(position);
    }

    public HeightTilesF GetTargetHeightAt(RelTile2i position)
    {
      return this.Manager.TerrainManager.GetHeight(this.OriginTileCoord + position);
    }

    /// <summary>
    /// Extends given coordinate for target height. This assumes that given coordinate belongs to this designation.
    /// </summary>
    public Tile3f GetTargetCoordAt(RelTile2i position)
    {
      return (this.OriginTile + position).CornerTile2f.ExtendHeight(this.GetTargetHeightAt(position));
    }

    public void ForEachTile(Action<Tile2iAndIndex> action)
    {
      TerrainManager terrainManager = this.Manager.TerrainManager;
      for (int index1 = 0; index1 < 4; ++index1)
      {
        int y = this.OriginTileCoord.Y + index1;
        for (int index2 = 0; index2 < 4; ++index2)
        {
          int x = this.OriginTileCoord.X + index2;
          action(terrainManager.ExtendTileIndex(x, y));
        }
      }
    }

    public void ForEachNonFulfilledTile(Action<TerrainTile> action)
    {
      TerrainTile plusYneighbor = this.Manager.TerrainManager[this.OriginTileCoord];
      int y = 0;
      while (y < 4)
      {
        TerrainTile terrainTile = plusYneighbor;
        int x = 0;
        while (x < 4)
        {
          if (!this.IsFulfilledAt(new RelTile2i(x, y)))
            action(terrainTile);
          ++x;
          terrainTile = terrainTile.PlusXNeighbor;
        }
        ++y;
        plusYneighbor = plusYneighbor.PlusYNeighbor;
      }
    }

    public Tile2i? FindBestNonFulfilledTileCoord<T>(
      Func<IDesignation, TerrainTile, RelTile2i, T> costFn,
      Option<Predicate<TerrainTile>> predicate,
      out T minCost)
      where T : IComparable<T>
    {
      minCost = default (T);
      RelTile2i relTile2i = new RelTile2i();
      bool flag = false;
      TerrainTile plusYneighbor = this.Manager.TerrainManager[this.OriginTileCoord];
      Comparer<T> comparer = Comparer<T>.Default;
      int y = 0;
      while (y < 4)
      {
        TerrainTile terrainTile = plusYneighbor;
        int x1 = 0;
        while (x1 < 4)
        {
          RelTile2i coord = new RelTile2i(x1, y);
          if (!this.IsFulfilledAt(coord) && (!predicate.HasValue || predicate.Value(terrainTile)))
          {
            T x2 = costFn((IDesignation) this, terrainTile, coord);
            if (flag)
            {
              if (comparer.Compare(x2, minCost) < 0)
              {
                relTile2i = coord;
                minCost = x2;
              }
            }
            else
            {
              flag = true;
              relTile2i = coord;
              minCost = x2;
            }
          }
          ++x1;
          terrainTile = terrainTile.PlusXNeighbor;
        }
        ++y;
        plusYneighbor = plusYneighbor.PlusYNeighbor;
      }
      return !flag ? new Tile2i?() : new Tile2i?(this.OriginTileCoord + relTile2i);
    }

    public Quantity GetQuantityToPlace(ProductProto product, Quantity limit)
    {
      TerrainManager terrainManager = this.Manager.TerrainManager;
      Quantity zero = Quantity.Zero;
      TerrainTileSurfaceProto tileSurfaceProto = (TerrainTileSurfaceProto) null;
      foreach (TerrainTile terrainTile in this.AllNonFulfilled)
      {
        TileSurfaceSlimId surfaceAt = this.GetSurfaceAt(terrainTile.TileCoord);
        if ((Proto) tileSurfaceProto == (Proto) null || tileSurfaceProto.SlimId != surfaceAt)
          tileSurfaceProto = terrainManager.ResolveSlimSurface(surfaceAt);
        ProductQuantity costPerTile = tileSurfaceProto.CostPerTile;
        if ((Proto) costPerTile.Product == (Proto) product && zero + costPerTile.Quantity <= limit)
          zero += costPerTile.Quantity;
      }
      return zero;
    }

    public Quantity GetQuantityToClear(ProductProto product, Quantity limit)
    {
      TerrainManager terrainManager = this.Manager.TerrainManager;
      Quantity zero = Quantity.Zero;
      TerrainTileSurfaceProto tileSurfaceProto = (TerrainTileSurfaceProto) null;
      foreach (TerrainTile terrainTile in this.AllNonFulfilled)
      {
        TileSurfaceData tileSurfaceData;
        if (terrainManager.TryGetTileSurface(terrainTile.DataIndex, out tileSurfaceData) && !tileSurfaceData.IsAutoPlaced)
        {
          if ((Proto) tileSurfaceProto == (Proto) null || tileSurfaceProto.SlimId != tileSurfaceData.SurfaceSlimId)
            tileSurfaceProto = tileSurfaceData.ResolveToProto(terrainManager);
          ProductQuantity costPerTile = tileSurfaceProto.CostPerTile;
          if ((Proto) costPerTile.Product == (Proto) product && zero + costPerTile.Quantity <= limit)
            zero += costPerTile.Quantity;
        }
      }
      return zero;
    }

    /// <summary>
    /// Returns bit mask for given absolute position. This position must be on this designation.
    /// </summary>
    private uint getMask(Tile2i position)
    {
      return SurfaceDesignation.getMask(position - (Tile2i) this.OriginTile);
    }

    /// <summary>
    /// Returns bit mask for given relative coordinate. This coordinate must be on this designation.
    /// </summary>
    private static uint getMask(RelTile2i coord) => (uint) (1 << coord.X + coord.Y * 4);

    private void unassignTile(RelTile2i relTile)
    {
      this.UnassignedTilesBitmap |= SurfaceDesignation.getMask(relTile);
      this.m_tilesFulfilledBitmap &= ~SurfaceDesignation.getMask(relTile);
    }

    public void UnassignArea(RectangleTerrainArea2i area)
    {
      for (int y = 0; y < 4; ++y)
      {
        for (int x = 0; x < 4; ++x)
        {
          RelTile2i relTile = new RelTile2i(x, y);
          if (area.ContainsTile(this.OriginTileCoord + relTile))
            this.unassignTile(relTile);
        }
      }
    }

    public void AssignTiles(uint assignedTilesBitmap, TileSurfaceSlimId surfaceProtoSlimId)
    {
      this.UnassignedTilesBitmap &= ~assignedTilesBitmap;
      Assert.That<int>((int) surfaceProtoSlimId.Value).IsLessOrEqual(15);
      for (int index1 = 0; index1 < 4; ++index1)
      {
        for (int index2 = 0; index2 < 4; ++index2)
        {
          int num = index1 * 4 + index2;
          if (((long) assignedTilesBitmap & (long) (1 << num)) != 0L)
          {
            this.SurfaceTypeMap &= ~(15UL << num * 4);
            this.SurfaceTypeMap |= (ulong) surfaceProtoSlimId.Value << num * 4;
          }
        }
      }
      this.ForEachTile(new Action<Tile2iAndIndex>(this.updateBitmaps));
    }

    public void UnassignTiles(uint tilesToUnassign)
    {
      this.UnassignedTilesBitmap |= tilesToUnassign;
      this.ForEachTile(new Action<Tile2iAndIndex>(this.updateBitmaps));
    }

    public TileSurfaceSlimId GetSurfaceAt(Tile2i tile)
    {
      RelTile2i relTile2i = tile - this.OriginTileCoord;
      return new TileSurfaceSlimId((byte) (this.SurfaceTypeMap >> (relTile2i.Y * 4 + relTile2i.X) * 4 & 15UL));
    }

    /// <summary>
    /// Updates all bitmap states for the given tile. This should be called whenever a tile is changed.
    /// </summary>
    private void updateBitmaps(Tile2iAndIndex tileAndIndex)
    {
      RelTile2i relTile2i = tileAndIndex.TileCoord - this.OriginTileCoord;
      if (!this.IsAssignedAt(relTile2i))
        return;
      if (!this.Manager.CanPlaceSurfaceTile(tileAndIndex.TileCoord))
      {
        this.unassignTile(relTile2i);
      }
      else
      {
        bool isFulfilled = this.isFulfilledAt(tileAndIndex);
        this.setFulfilledAt(relTile2i, isFulfilled);
        if (!isFulfilled)
          return;
        this.unassignTile(relTile2i);
      }
    }

    private bool isFulfilledAt(Tile2iAndIndex tileAndIndex)
    {
      TerrainManager terrainManager = this.Manager.TerrainManager;
      if (this.IsPlacing)
      {
        OccupiedTileRelative occupiedTile;
        IStaticEntityProto entityProto;
        if (this.Manager.TryGetEntityOccupyingAt(tileAndIndex, out occupiedTile, out IStaticEntity _, out entityProto) && occupiedTile.TileSurface.IsPhantom && entityProto is ILayoutEntityProto layoutEntityProto && layoutEntityProto.Layout.LayoutParams.EnforceEmptySurface)
          return true;
        TileSurfaceData tileSurfaceData;
        return terrainManager.TryGetTileSurface(tileAndIndex.Index, out tileSurfaceData) && tileSurfaceData.Height.IsNear(terrainManager.GetHeight(tileAndIndex.TileCoord.CenterTile2f), TerrainDesignation.SURFACE_HEIGHT_TOLERANCE) && this.GetSurfaceAt(tileAndIndex.TileCoord) == tileSurfaceData.SurfaceSlimId;
      }
      TileSurfaceData tileSurfaceData1;
      return !terrainManager.TryGetTileSurface(tileAndIndex.Index, out tileSurfaceData1) || tileSurfaceData1.IsAutoPlaced || !tileSurfaceData1.Height.IsNear(terrainManager.GetHeight(tileAndIndex.TileCoord.CenterTile2f), TerrainDesignation.SURFACE_HEIGHT_TOLERANCE);
    }

    private void setFulfilledAt(RelTile2i tileCoord, bool isFulfilled)
    {
      uint mask = SurfaceDesignation.getMask(tileCoord);
      if (isFulfilled)
        this.m_tilesFulfilledBitmap |= mask;
      else
        this.m_tilesFulfilledBitmap &= ~mask;
    }

    internal bool ValidateAndFixFulfilledBitmap(Lyst<KeyValuePair<Tile2i, bool>> errorLocations)
    {
      bool flag1 = true;
      TerrainManager terrainManager = this.Manager.TerrainManager;
      for (int y = 0; y < 4; ++y)
      {
        for (int x = 0; x < 4; ++x)
        {
          RelTile2i relTile2i = new RelTile2i(x, y);
          Tile2i tile2i = this.OriginTileCoord + relTile2i;
          if (this.IsAssignedAt(tile2i))
          {
            bool isFulfilled = this.isFulfilledAt(terrainManager.ExtendTileIndex(tile2i));
            bool flag2 = this.IsFulfilledAt(relTile2i);
            if (isFulfilled != flag2)
            {
              errorLocations.Add(Make.Kvp<Tile2i, bool>(tile2i, flag2));
              this.setFulfilledAt(relTile2i, isFulfilled);
              flag1 = false;
            }
          }
        }
      }
      return flag1;
    }

    public bool Equals(SurfaceDesignation other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      if (!this.OriginTileCoord.Equals(other.OriginTileCoord) || !this.Prototype.Equals((Proto) other.Prototype))
        return false;
      Log.Error("Area designations that are not equal references have equal position and proto.");
      return true;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      if (!(obj is SurfaceDesignation surfaceDesignation) || !this.OriginTileCoord.Equals(surfaceDesignation.OriginTileCoord) || !this.Prototype.Equals((Proto) surfaceDesignation.Prototype))
        return false;
      Log.Error("Area designations that are not equal references have equal position and proto.");
      return true;
    }

    public override int GetHashCode()
    {
      return Hash.Combine<SurfaceDesignationProto, Tile2i>(this.Prototype, this.OriginTileCoord);
    }

    public override string ToString()
    {
      return string.Format("{0} at {1}", (object) this.Prototype.Id, (object) this.OriginTileCoord) + (this.IsDestroyed ? " (destroyed)" : "");
    }

    public int CountFulfilledTiles() => this.m_tilesFulfilledBitmap.CountSetBits();

    public int CountUnfulfilledTiles()
    {
      return this.IsNotAssigned ? 0 : 16 - this.m_tilesFulfilledBitmap.CountSetBits();
    }

    static SurfaceDesignation()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SurfaceDesignation.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SurfaceDesignation) obj).SerializeData(writer));
      SurfaceDesignation.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SurfaceDesignation) obj).DeserializeData(reader));
      SurfaceDesignation.SURFACE_HEIGHT_TOLERANCE = new HeightTilesF((1.0 / 16.0).ToFix32());
    }

    public readonly struct EnumeratorHolder
    {
      private readonly SurfaceDesignation m_designation;
      private readonly bool m_fulfilled;

      public EnumeratorHolder(SurfaceDesignation designation, bool fulfilled)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_designation = designation;
        this.m_fulfilled = fulfilled;
      }

      public SurfaceDesignation.EnumeratorHolder.Enumerator GetEnumerator()
      {
        return new SurfaceDesignation.EnumeratorHolder.Enumerator(this.m_designation, this.m_fulfilled);
      }

      /// <summary>Enumerates unfulfilled tiles.</summary>
      public struct Enumerator
      {
        private int m_x;
        private int m_y;
        private TerrainTile m_yTile;
        private TerrainTile m_xTile;
        private readonly SurfaceDesignation m_designation;
        private readonly bool m_fulfilled;

        public TerrainTile Current { get; private set; }

        internal Enumerator(SurfaceDesignation designation, bool fulfilled)
        {
          MBiHIp97M4MqqbtZOh.rMWAw2OR8();
          this.m_x = -1;
          this.m_y = 0;
          this.m_designation = designation;
          this.m_fulfilled = fulfilled;
          this.m_xTile = this.m_yTile = designation.Manager.TerrainManager[designation.OriginTileCoord];
          this.Current = new TerrainTile();
        }

        public bool MoveNext()
        {
          do
          {
            if (this.m_x < 3)
            {
              ++this.m_x;
              if (this.m_x > 0)
                this.m_xTile = this.m_xTile.PlusXNeighbor;
            }
            else
            {
              if (this.m_y >= 3)
                return false;
              this.m_x = 0;
              ++this.m_y;
              this.m_yTile = this.m_yTile.PlusYNeighbor;
              this.m_xTile = this.m_yTile;
            }
          }
          while (this.m_designation.IsFulfilledAt(new RelTile2i(this.m_x, this.m_y)) != this.m_fulfilled);
          this.Current = this.m_xTile;
          return true;
        }
      }
    }
  }
}
