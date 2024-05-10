// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.TerrainDesignation
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
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
  /// <summary>Terrain height designation.</summary>
  [GenerateSerializer(false, null, 0)]
  public class TerrainDesignation : IDesignation, IIsSafeAsHashKey, IEquatable<TerrainDesignation>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>Designations size in bits.</summary>
    public const int SIZE_BITS = 2;
    public const int SIZE_MASK = 3;
    /// <summary>Designation size in tiles is 4.</summary>
    /// <remarks>
    /// By making default terrain designation larger than 1x1 we decrease micro-management with terrain designation,
    /// make the visualization nicer, and mining less scary. Designation size also sets the steepness of ramps.
    /// 
    /// The size 4x4 was chosen so that they are aligned with terrain chunks to make rendering more efficient
    /// and to allow more flexibility during mine designation. Other options were 2x2 which is too small and 8x8
    /// which is too large.
    /// </remarks>
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
    public const int AREA_TILES_NO_OVERLAP = 16;
    public const int AREA_TILE_VERTICES = 25;
    /// <summary>Edges indexed by `NeighborCoord`.</summary>
    private static readonly ReadOnlyArray<uint> MASK_FOR_EDGE;
    /// <summary>
    /// How far from the terrain can a surface be when placed.
    /// </summary>
    public static readonly HeightTilesF SURFACE_HEIGHT_TOLERANCE;
    /// <summary>Prototype of this designation.</summary>
    public readonly TerrainDesignationProto Prototype;
    /// <summary>
    /// Bit is set when a tile is fulfilled. All unused bits are zero.
    /// </summary>
    private uint m_tilesFulfilledBitmap;
    /// <summary>
    /// Bit is set when a tile is fulfilled for the mining criterion. All unused bits are zero.
    /// </summary>
    private uint m_tilesFulfilledMiningBitmap;
    /// <summary>
    /// Bit is set when a tile is fulfilled for the dumping criterion. All unused bits are zero.
    /// </summary>
    private uint m_tilesFulfilledDumpingBitmap;
    /// <summary>All mine towers that manage this area.</summary>
    [DoNotSaveCreateNewOnLoad("new AssignedTowers(this)", 0)]
    public readonly TerrainDesignation.AssignedTowers ManagedByTowers;
    private Option<IAreaManagingTower> m_tower;
    private LystStruct<IAreaManagingTower> m_towers;

    public static void Serialize(TerrainDesignation value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainDesignation>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainDesignation.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      DesignationData.Serialize(this.Data, writer);
      writer.WriteBool(this.IsDestroyed);
      writer.WriteUInt(this.m_tilesFulfilledBitmap);
      writer.WriteUInt(this.m_tilesFulfilledDumpingBitmap);
      writer.WriteUInt(this.m_tilesFulfilledMiningBitmap);
      Option<IAreaManagingTower>.Serialize(this.m_tower, writer);
      LystStruct<IAreaManagingTower>.Serialize(this.m_towers, writer);
      writer.WriteGeneric<TerrainDesignationProto>(this.Prototype);
    }

    public static TerrainDesignation Deserialize(BlobReader reader)
    {
      TerrainDesignation terrainDesignation;
      if (reader.TryStartClassDeserialization<TerrainDesignation>(out terrainDesignation))
        reader.EnqueueDataDeserialization((object) terrainDesignation, TerrainDesignation.s_deserializeDataDelayedAction);
      return terrainDesignation;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.Data = DesignationData.Deserialize(reader);
      this.IsDestroyed = reader.ReadBool();
      this.m_tilesFulfilledBitmap = reader.ReadUInt();
      this.m_tilesFulfilledDumpingBitmap = reader.ReadUInt();
      this.m_tilesFulfilledMiningBitmap = reader.ReadUInt();
      this.m_tower = Option<IAreaManagingTower>.Deserialize(reader);
      this.m_towers = LystStruct<IAreaManagingTower>.Deserialize(reader);
      reader.SetField<TerrainDesignation>(this, "ManagedByTowers", (object) new TerrainDesignation.AssignedTowers(this));
      reader.SetField<TerrainDesignation>(this, "Prototype", (object) reader.ReadGenericAs<TerrainDesignationProto>());
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

    public Proto.ID ProtoId => this.Prototype.Id;

    [DoNotSave(0, null)]
    public Option<ITerrainDesignationsManagerInternal> Manager { get; private set; }

    /// <summary>
    /// Origin tile coord of this designation. Origin is always at whole multiples of <c>SIZE_TILES</c>.
    /// </summary>
    public Tile2i OriginTileCoord => this.Data.OriginTile;

    public Tile2i PlusXTileCoord => this.Data.PlusXTileCoord;

    public Tile2i PlusYTileCoord => this.Data.PlusYTileCoord;

    public Tile2i PlusXyTileCoord => this.Data.PlusXyTileCoord;

    public Vector2i WithinChunkRelCoord => this.Data.WithinChunkRelCoord;

    public int WithinChunkRelIndex => this.Data.WithinChunkRelIndex;

    public Chunk2i ChunkCoord => this.Data.ChunkCoord;

    /// <summary>Tile at the center of this designation.</summary>
    public Tile2i CenterTileCoord => this.OriginTileCoord.AddXy(2);

    public Tile3i Origin3i => this.OriginTileCoord.ExtendHeight(this.Data.OriginTargetHeight);

    public Tile3i PlusX3i => this.PlusXTileCoord.ExtendHeight(this.Data.PlusXTargetHeight);

    public Tile3i PlusY3i => this.PlusYTileCoord.ExtendHeight(this.Data.PlusYTargetHeight);

    public Tile3i PlusXy3i => this.PlusXyTileCoord.ExtendHeight(this.Data.PlusXyTargetHeight);

    public RectangleTerrainArea2i Area
    {
      get => new RectangleTerrainArea2i(this.Data.OriginTile, TerrainDesignation.Size);
    }

    /// <summary>
    /// Whether this designation was destroyed and is no longer valid.
    /// </summary>
    public bool IsDestroyed { get; private set; }

    /// <summary>Designated height at origin.</summary>
    public DesignationData Data { get; private set; }

    /// <summary>
    /// Whether this designation is flat. That's when all four heights are identical. This value is cached to allow
    /// fast queries.
    /// </summary>
    [DoNotSave(0, null)]
    public bool IsFlat { get; private set; }

    /// <summary>
    /// The direction of the ramp. This returns +X if the designation is flat.
    /// </summary>
    [DoNotSave(0, null)]
    public Direction90 RampDirection { get; private set; }

    /// <summary>
    /// Minimal target height of this designation. This value is cached to allow fast queries.
    /// </summary>
    [DoNotSave(0, null)]
    public HeightTilesI MinTargetHeight { get; private set; }

    /// <summary>
    /// Maximal target height of this designation. This value is cached to allow fast queries.
    /// </summary>
    [DoNotSave(0, null)]
    public HeightTilesI MaxTargetHeight { get; private set; }

    /// <summary>
    /// Number of jobs that are taking care of fulfilling this designation.
    /// </summary>
    [DoNotSave(0, null)]
    public int NumberOfJobsAssigned { get; protected set; }

    /// <summary>
    /// Bit is set when a tile is fulfilled. All unused bits are zero.
    /// </summary>
    public uint TilesFulfilledBitmap => this.m_tilesFulfilledBitmap;

    /// <summary>Whether this designation is fulfilled.</summary>
    public bool IsFulfilled => this.m_tilesFulfilledBitmap == 33554431U;

    public bool IsNotFulfilled => this.m_tilesFulfilledBitmap != 33554431U;

    /// <summary>
    /// Whether this designation's mining criterion is fulfilled.
    /// </summary>
    public bool IsMiningFulfilled => this.m_tilesFulfilledMiningBitmap == 33554431U;

    public bool IsMiningNotFulfilled => this.m_tilesFulfilledMiningBitmap != 33554431U;

    /// <summary>
    /// Whether this designation's dumping criterion is fulfilled.
    /// </summary>
    public bool IsDumpingFulfilled => this.m_tilesFulfilledDumpingBitmap == 33554431U;

    public bool IsDumpingNotFulfilled => this.m_tilesFulfilledDumpingBitmap != 33554431U;

    /// <summary>Whether this designation is a forestry designation.</summary>
    public bool IsForestry => this.Prototype.Id.Value == "ForestryDesignator";

    /// <summary>Whether this designation is ready to be fulfilled.</summary>
    public bool IsReadyToBeFulfilled
    {
      get
      {
        if (!this.Prototype.IsTerraforming)
          return true;
        if (!(this.Prototype.Id.Value == "LevelDesignator"))
          return (this.m_tilesFulfilledBitmap & 33080895U) > 0U;
        if (this.IsMiningReadyToBeFulfilled && this.IsMiningNotFulfilled)
          return true;
        return this.IsDumpingReadyToBeFulfilled && this.IsDumpingNotFulfilled;
      }
    }

    /// <summary>
    /// Whether this designation is ready to be fulfilled for mining.
    /// </summary>
    public bool IsMiningReadyToBeFulfilled => (this.m_tilesFulfilledMiningBitmap & 33080895U) > 0U;

    /// <summary>
    /// Whether this designation is ready to be fulfilled for dumping.
    /// </summary>
    public bool IsDumpingReadyToBeFulfilled
    {
      get => (this.m_tilesFulfilledDumpingBitmap & 33080895U) > 0U;
    }

    /// <summary>
    /// Number of vehicles that recently failed to reach this designation.
    /// Should be only set by unreachable manager.
    /// </summary>
    [DoNotSave(0, null)]
    public ushort UnreachableVehiclesCount { get; set; }

    public Option<TerrainDesignation> PlusXNeighbor
    {
      get
      {
        return !this.Manager.HasValue ? Option<TerrainDesignation>.None : this.Manager.Value.GetDesignationAt(this.OriginTileCoord.AddX(4));
      }
    }

    public Option<TerrainDesignation> PlusYNeighbor
    {
      get
      {
        return !this.Manager.HasValue ? Option<TerrainDesignation>.None : this.Manager.Value.GetDesignationAt(this.OriginTileCoord.AddY(4));
      }
    }

    public Option<TerrainDesignation> MinusXNeighbor
    {
      get
      {
        return !this.Manager.HasValue ? Option<TerrainDesignation>.None : this.Manager.Value.GetDesignationAt(this.OriginTileCoord.AddX(-4));
      }
    }

    public Option<TerrainDesignation> MinusYNeighbor
    {
      get
      {
        return !this.Manager.HasValue ? Option<TerrainDesignation>.None : this.Manager.Value.GetDesignationAt(this.OriginTileCoord.AddY(-4));
      }
    }

    public TerrainDesignation(TerrainDesignationProto prototype, DesignationData data)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Prototype = prototype.CheckNotNull<TerrainDesignationProto>();
      this.Data = data;
      this.ManagedByTowers = new TerrainDesignation.AssignedTowers(this);
      this.initMinMaxTargets();
    }

    private void initMinMaxTargets()
    {
      this.IsFlat = this.Data.OriginTargetHeight == this.Data.PlusXTargetHeight && this.Data.OriginTargetHeight == this.Data.PlusYTargetHeight && this.Data.OriginTargetHeight == this.Data.PlusXyTargetHeight;
      if (this.IsFlat)
      {
        this.MinTargetHeight = this.Data.OriginTargetHeight;
        this.MaxTargetHeight = this.Data.OriginTargetHeight;
      }
      else
      {
        if (this.Data.PlusXTargetHeight > this.Data.OriginTargetHeight)
          this.RampDirection = Direction90.PlusX;
        else if (this.Data.PlusXTargetHeight < this.Data.OriginTargetHeight)
          this.RampDirection = Direction90.MinusX;
        else if (this.Data.PlusYTargetHeight > this.Data.OriginTargetHeight)
          this.RampDirection = Direction90.PlusY;
        else if (this.Data.PlusYTargetHeight < this.Data.OriginTargetHeight)
          this.RampDirection = Direction90.MinusY;
        HeightTilesI heightTilesI1 = this.Data.OriginTargetHeight.Min(this.Data.PlusXTargetHeight);
        heightTilesI1 = heightTilesI1.Min(this.Data.PlusYTargetHeight);
        this.MinTargetHeight = heightTilesI1.Min(this.Data.PlusXyTargetHeight);
        HeightTilesI heightTilesI2 = this.Data.OriginTargetHeight.Max(this.Data.PlusXTargetHeight);
        heightTilesI2 = heightTilesI2.Max(this.Data.PlusYTargetHeight);
        this.MaxTargetHeight = heightTilesI2.Max(this.Data.PlusXyTargetHeight);
      }
    }

    internal void InitAfterLoad(ITerrainDesignationsManagerInternal manager)
    {
      this.Manager = manager.SomeOption<ITerrainDesignationsManagerInternal>();
      this.NumberOfJobsAssigned = 0;
      this.initMinMaxTargets();
    }

    internal void InitNumberOfJobsAssigned(int numberOfJobsAssigned)
    {
      this.NumberOfJobsAssigned = numberOfJobsAssigned;
    }

    /// <summary>
    /// Whether this designation is assigned to some dynamic entity.
    /// </summary>
    public bool CanBeAssigned(bool tryIgnoreReservations)
    {
      if (this.NumberOfJobsAssigned < this.Prototype.MaxAssignedEntities)
        return true;
      return tryIgnoreReservations && this.Prototype.CanOverflowReservations;
    }

    internal void SetManager(ITerrainDesignationsManagerInternal manager)
    {
      this.Manager = Option.Some<ITerrainDesignationsManagerInternal>(manager);
      this.ForEachTile(new Action<Tile2iAndIndex, HeightTilesF>(this.updateBitmaps));
    }

    internal void SetDestroyed()
    {
      Assert.That<bool>(this.ManagedByTowers.IsEmpty()).IsTrue("Managed towers were not cleared properly.");
      if (this.NumberOfJobsAssigned > 0 && this.Manager.HasValue)
      {
        IIndexable<IVehicleJob> assignedJobsFor = this.Manager.Value.GetAssignedJobsFor(this);
        foreach (IVehicleJob immutable in assignedJobsFor.ToImmutableArray<IVehicleJob>())
          immutable.RequestCancel();
        Assert.That<IIndexable<IVehicleJob>>(assignedJobsFor).IsEmpty<IVehicleJob>();
      }
      this.IsDestroyed = true;
      this.Manager = Option<ITerrainDesignationsManagerInternal>.None;
    }

    public void AddManagingTower(IAreaManagingTower tower)
    {
      if (tower.IsDestroyed)
      {
        Log.Error("Adding a destroyed tower!");
      }
      else
      {
        this.addTowerInternal(tower);
        this.Manager.ValueOrNull?.OnManagedTowersChanged(this);
      }
    }

    public void RemoveManagingTower(IAreaManagingTower tower)
    {
      this.removeTowerInternal(tower);
      this.Manager.ValueOrNull?.OnManagedTowersChanged(this);
    }

    public bool IsNeighborWith(TerrainDesignation td)
    {
      RelTile2i relTile2i = this.Data.OriginTile - td.Data.OriginTile;
      return relTile2i.X == 4 ^ relTile2i.Y == 4;
    }

    /// <summary>
    /// Called by <see cref="T:Mafi.Core.Terrain.Designation.ITerrainDesignationsManager" /> when a tile under this designation is changed.
    /// </summary>
    public virtual void TileChanged(Tile2iAndIndex tileAndIndex)
    {
      this.updateBitmaps(tileAndIndex, this.GetTargetHeightAt(tileAndIndex.TileCoord));
    }

    public bool TryAssignTo(IVehicleJob job)
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse("Assigning to invalid designation.");
      if (this.NumberOfJobsAssigned >= this.Prototype.MaxAssignedEntities && !this.Prototype.CanOverflowReservations)
        return false;
      ITerrainDesignationsManagerInternal valueOrNull = this.Manager.ValueOrNull;
      int numberOfJobsAssigned;
      if ((valueOrNull != null ? (valueOrNull.TryAddAssignedJob(this, job, out numberOfJobsAssigned) ? 1 : 0) : 0) == 0)
        return false;
      this.NumberOfJobsAssigned = numberOfJobsAssigned;
      return true;
    }

    public bool IsAssignedTo(IVehicleJob job)
    {
      ITerrainDesignationsManagerInternal valueOrNull = this.Manager.ValueOrNull;
      return valueOrNull != null && valueOrNull.GetAssignedJobsFor(this).Contains<IVehicleJob>(job);
    }

    public void RemoveAssignment(IVehicleJob job)
    {
      ITerrainDesignationsManagerInternal valueOrNull = this.Manager.ValueOrNull;
      this.NumberOfJobsAssigned = valueOrNull != null ? valueOrNull.RemoveAssignedJob(this, job) : 0;
    }

    /// <summary>Whether this designation contains given position.</summary>
    public bool ContainsPosition(Tile2i position)
    {
      return position >= this.OriginTileCoord && position <= this.OriginTileCoord + new RelTile2i(4, 4);
    }

    public bool IsFulfilledAt(Tile2i coord)
    {
      return (this.m_tilesFulfilledBitmap & this.getMask(coord)) > 0U;
    }

    public bool IsFulfilledAt(RelTile2i coord)
    {
      return (this.m_tilesFulfilledBitmap & TerrainDesignation.getMask(coord)) > 0U;
    }

    public bool IsMiningFulfilledAt(Tile2i coord)
    {
      return (this.m_tilesFulfilledMiningBitmap & this.getMask(coord)) > 0U;
    }

    public bool IsMiningFulfilledAt(RelTile2i coord)
    {
      return (this.m_tilesFulfilledMiningBitmap & TerrainDesignation.getMask(coord)) > 0U;
    }

    public bool IsDumpingFulfilledAt(Tile2i coord)
    {
      return (this.m_tilesFulfilledDumpingBitmap & this.getMask(coord)) > 0U;
    }

    public bool IsDumpingFulfilledAt(RelTile2i coord)
    {
      return (this.m_tilesFulfilledDumpingBitmap & TerrainDesignation.getMask(coord)) > 0U;
    }

    public bool IsReadyToBeFulfilledAt(Tile2i coord)
    {
      return !this.Prototype.IsTerraforming || (this.m_tilesFulfilledBitmap & 33080895U) > 0U;
    }

    public bool IsReadyToBeFulfilledAt(RelTile2i coord)
    {
      return !this.Prototype.IsTerraforming || (this.m_tilesFulfilledBitmap & 33080895U) > 0U;
    }

    /// <summary>
    /// Whether this designation has flat edge towards given direction.
    /// </summary>
    public bool IsFlatTowards(NeighborCoord nbrCoord)
    {
      switch (nbrCoord.Index)
      {
        case 0:
          return this.Data.PlusXTargetHeight == this.Data.PlusXyTargetHeight;
        case 1:
          return this.Data.OriginTargetHeight == this.Data.PlusYTargetHeight;
        case 2:
          return this.Data.PlusYTargetHeight == this.Data.PlusXyTargetHeight;
        case 3:
          return this.Data.OriginTargetHeight == this.Data.PlusXTargetHeight;
        default:
          Assert.Fail("Invalid neighbor coord.");
          return false;
      }
    }

    /// <summary>
    /// Whether this designation is ramp-up in given direction.
    /// </summary>
    public bool IsRampUpTowards(RelTile2i direction)
    {
      if (this.IsFlat)
        return false;
      if (direction.X > 0)
        return this.Data.OriginTargetHeight == this.Data.PlusYTargetHeight && this.Data.PlusXTargetHeight == this.Data.PlusXyTargetHeight && this.Data.OriginTargetHeight + ThicknessTilesI.One == this.Data.PlusXTargetHeight;
      if (direction.X < 0)
        return this.Data.OriginTargetHeight == this.Data.PlusYTargetHeight && this.Data.PlusXTargetHeight == this.Data.PlusXyTargetHeight && this.Data.OriginTargetHeight - ThicknessTilesI.One == this.Data.PlusXTargetHeight;
      if (direction.Y > 0)
        return this.Data.OriginTargetHeight == this.Data.PlusXTargetHeight && this.Data.PlusYTargetHeight == this.Data.PlusXyTargetHeight && this.Data.OriginTargetHeight + ThicknessTilesI.One == this.Data.PlusYTargetHeight;
      if (direction.Y < 0)
        return this.Data.OriginTargetHeight == this.Data.PlusXTargetHeight && this.Data.PlusYTargetHeight == this.Data.PlusXyTargetHeight && this.Data.OriginTargetHeight - ThicknessTilesI.One == this.Data.PlusYTargetHeight;
      Assert.Fail("Invalid direction given.");
      return false;
    }

    /// <summary>
    /// Returns unit vector that represents up-direction, or zero vector for flat ramps. Both components may be set
    /// to non-zero for designations with slopes in both X and Y directions. Magnitude of components is height
    /// difference, usually 1.
    /// </summary>
    public RelTile2i GetRampUpDirection()
    {
      return new RelTile2i((this.Data.PlusXTargetHeight - this.Data.OriginTargetHeight).Value, (this.Data.PlusYTargetHeight - this.Data.OriginTargetHeight).Value);
    }

    /// <summary>
    /// Whether this designation is fulfilled at edge towards given direction.
    /// </summary>
    public bool IsFulfilledTowards(NeighborCoord nbrCoord)
    {
      uint num = TerrainDesignation.MASK_FOR_EDGE[nbrCoord.Index];
      return ((int) this.m_tilesFulfilledBitmap & (int) num) == (int) num;
    }

    public bool IsNotAtAllFulfilledTowards(NeighborCoord nbrCoord)
    {
      return ((int) this.m_tilesFulfilledBitmap & (int) TerrainDesignation.MASK_FOR_EDGE[nbrCoord.Index]) == 0;
    }

    public bool IsPartiallyFulfilledTowards(NeighborCoord nbrCoord)
    {
      uint num1 = TerrainDesignation.MASK_FOR_EDGE[nbrCoord.Index];
      uint num2 = this.m_tilesFulfilledBitmap & num1;
      return num2 != 0U && (int) num2 != (int) num1;
    }

    public bool DisplayWarningTowards(NeighborCoord nbrCoord)
    {
      if (this.Manager.IsNone)
        return false;
      if (!this.Prototype.IsTerraforming)
      {
        Option<TerrainDesignation> designationAt = this.Manager.Value.GetDesignationAt(this.OriginTileCoord + new RelTile2i(nbrCoord.Dx * 4, nbrCoord.Dy * 4));
        return designationAt.IsNone || designationAt.Value.Prototype.IsTerraforming;
      }
      return this.IsNotAtAllFulfilledTowards(nbrCoord) && !this.IsSnappedTowards(nbrCoord);
    }

    public bool HasNoForestryDesignationTowards(NeighborCoord nbrCoord0, NeighborCoord nbrCoord1)
    {
      if (this.Manager.IsNone)
        return false;
      Option<TerrainDesignation> designationAt = this.Manager.Value.GetDesignationAt(this.OriginTileCoord + new RelTile2i((nbrCoord0.Dx + nbrCoord1.Dx) * 4, (nbrCoord0.Dy + nbrCoord1.Dy) * 4));
      return designationAt.IsNone || !designationAt.Value.IsForestry;
    }

    public LocStrFormatted? GetWarningStr()
    {
      if (this.Manager.IsNone)
        return new LocStrFormatted?();
      if (this.ManagedByTowers.IsEmpty() && this.Prototype.DisplayWarningWhenNotOwned)
        return new LocStrFormatted?((LocStrFormatted) this.Prototype.WarningWhenNotOwned);
      if (this.IsForestry && !this.IsFulfilled)
        return new LocStrFormatted?((LocStrFormatted) TrCore.DesignationWarning__CannotStartForestry);
      if (!this.IsNotAtAllFulfilledTowards(NeighborCoord.PlusX) || !this.IsNotAtAllFulfilledTowards(NeighborCoord.PlusY) || !this.IsNotAtAllFulfilledTowards(NeighborCoord.MinusX) || !this.IsNotAtAllFulfilledTowards(NeighborCoord.MinusY))
        return new LocStrFormatted?();
      switch (this.Prototype.Id.Value)
      {
        case "MiningDesignator":
          return new LocStrFormatted?((LocStrFormatted) TrCore.DesignationWarning__CannotStartMining);
        case "DumpingDesignator":
          return new LocStrFormatted?((LocStrFormatted) TrCore.DesignationWarning__CannotStartDumping);
        case "LevelDesignator":
          return new LocStrFormatted?((LocStrFormatted) TrCore.DesignationWarning__CannotStartLeveling);
        default:
          Assert.Fail("Missing specific designation warning message.");
          return new LocStrFormatted?();
      }
    }

    /// <summary>
    /// Returns target height for a given position. This is computed using bi-linear interpolation. Requested
    /// position should lie in this designation.
    /// </summary>
    public HeightTilesF GetTargetHeightAt(Tile2i position)
    {
      Assert.That<bool>(this.ContainsPosition(position)).IsTrue<Tile2i, TerrainDesignation>("Tile at {0} is outside of area '{1}'.", position, this);
      return this.GetTargetHeightAt(position - this.OriginTileCoord);
    }

    /// <summary>
    /// Returns target height for a given position. This is computed using bi-linear interpolation. Requested
    /// position should lie in this designation.
    /// </summary>
    public HeightTilesF GetTargetHeightAt(Tile2f position)
    {
      Assert.That<bool>(this.ContainsPosition(position.Tile2i)).IsTrue<Tile2f, TerrainDesignation>("Tile at {0} is outside of area '{1}'.", position, this);
      return this.GetTargetHeightAt(position - new Tile2f((Fix32) this.OriginTileCoord.X, (Fix32) this.OriginTileCoord.Y));
    }

    public HeightTilesF GetTargetHeightAt(RelTile2i position)
    {
      return this.Data.OriginTargetHeight.HeightTilesF.Lerp(this.Data.PlusYTargetHeight.HeightTilesF, (Fix32) position.Y, (Fix32) 4).Lerp(this.Data.PlusXTargetHeight.HeightTilesF.Lerp(this.Data.PlusXyTargetHeight.HeightTilesF, (Fix32) position.Y, (Fix32) 4), (Fix32) position.X, (Fix32) 4);
    }

    public HeightTilesF GetTargetHeightAt(RelTile2f position)
    {
      return this.Data.OriginTargetHeight.HeightTilesF.Lerp(this.Data.PlusYTargetHeight.HeightTilesF, position.Y, (Fix32) 4).Lerp(this.Data.PlusXTargetHeight.HeightTilesF.Lerp(this.Data.PlusXyTargetHeight.HeightTilesF, position.Y, (Fix32) 4), position.X, (Fix32) 4);
    }

    /// <summary>
    /// Extends given coordinate for target height. This assumes that given coordinate belongs to this designation.
    /// </summary>
    public Tile3f GetTargetCoordAt(RelTile2i position)
    {
      return (this.Data.OriginTile + position).CornerTile2f.ExtendHeight(this.GetTargetHeightAt(position));
    }

    /// <summary>
    /// Calls given function on every tile of this designation.
    /// TODO: Kill TerrainTile
    /// </summary>
    public void ForEachTile(Action<TerrainTile> action)
    {
      if (this.Manager.IsNone)
      {
        Log.Error("ForEachTile on a designation can be only performed on managed managed designations.");
      }
      else
      {
        TerrainTile plusYneighbor = this.Manager.Value.TerrainManager[this.OriginTileCoord];
        int num1 = 0;
        while (num1 <= 4)
        {
          TerrainTile terrainTile = plusYneighbor;
          int num2 = 0;
          while (num2 <= 4)
          {
            action(terrainTile);
            ++num2;
            terrainTile = terrainTile.PlusXNeighbor;
          }
          ++num1;
          plusYneighbor = plusYneighbor.PlusYNeighbor;
        }
      }
    }

    /// <summary>
    /// Calls given function on every tile of this designation.
    /// TODO: Kill TerrainTile
    /// </summary>
    public void ForEachTile(Action<TerrainTile, HeightTilesF> action)
    {
      if (this.Manager.IsNone)
      {
        Log.Error("ForEachTile on a designation can be only called on managed managed designations.");
      }
      else
      {
        TerrainTile plusYneighbor = this.Manager.Value.TerrainManager[this.OriginTileCoord];
        if (this.IsFlat)
        {
          HeightTilesF heightTilesF = this.Data.OriginTargetHeight.HeightTilesF;
          int num1 = 0;
          while (num1 <= 4)
          {
            TerrainTile terrainTile = plusYneighbor;
            int num2 = 0;
            while (num2 <= 4)
            {
              action(terrainTile, heightTilesF);
              ++num2;
              terrainTile = terrainTile.PlusXNeighbor;
            }
            ++num1;
            plusYneighbor = plusYneighbor.PlusYNeighbor;
          }
        }
        else
        {
          int num3 = 0;
          while (num3 <= 4)
          {
            TerrainTile terrainTile = plusYneighbor;
            int num4 = 0;
            while (num4 <= 4)
            {
              action(terrainTile, this.GetTargetHeightAt((Tile2i) terrainTile.TileCoordSlim));
              ++num4;
              terrainTile = terrainTile.PlusXNeighbor;
            }
            ++num3;
            plusYneighbor = plusYneighbor.PlusYNeighbor;
          }
        }
      }
    }

    public void ForEachTile(Action<Tile2iAndIndex, HeightTilesF> action)
    {
      if (this.Manager.IsNone)
      {
        Log.Error("ForEachTile on a designation can be only called on managed managed designations.");
      }
      else
      {
        TerrainManager terrainManager = this.Manager.Value.TerrainManager;
        if (this.IsFlat)
        {
          HeightTilesF heightTilesF = this.Data.OriginTargetHeight.HeightTilesF;
          for (int index1 = 0; index1 <= 4; ++index1)
          {
            int y = this.OriginTileCoord.Y + index1;
            for (int index2 = 0; index2 <= 4; ++index2)
            {
              int x = this.OriginTileCoord.X + index2;
              action(terrainManager.ExtendTileIndex(x, y), heightTilesF);
            }
          }
        }
        else
        {
          for (int y1 = 0; y1 <= 4; ++y1)
          {
            int y2 = this.OriginTileCoord.Y + y1;
            for (int x1 = 0; x1 <= 4; ++x1)
            {
              int x2 = this.OriginTileCoord.X + x1;
              action(terrainManager.ExtendTileIndex(x2, y2), this.GetTargetHeightAt(new RelTile2i(x1, y1)));
            }
          }
        }
      }
    }

    /// <summary>
    /// Calls given function on every tile of this designation. If the function returns false, the loop over tiles is
    /// terminated and current tile is returned. Otherwise, when the function returns true for all tiles, None is
    /// returned.
    /// </summary>
    public TerrainTile? ForEachTileOrBreak(Func<TerrainTile, HeightTilesF, bool> action)
    {
      if (this.Manager.IsNone)
      {
        Log.Error("ForEach tile on a designation can be only called on managed managed designations.");
        return new TerrainTile?();
      }
      TerrainTile plusYneighbor = this.Manager.Value.TerrainManager[this.OriginTileCoord];
      if (this.IsFlat)
      {
        HeightTilesF heightTilesF = this.Data.OriginTargetHeight.HeightTilesF;
        int num1 = 0;
        while (num1 <= 4)
        {
          TerrainTile terrainTile = plusYneighbor;
          int num2 = 0;
          while (num2 <= 4)
          {
            if (!action(terrainTile, heightTilesF))
              return new TerrainTile?(terrainTile);
            ++num2;
            terrainTile = terrainTile.PlusXNeighbor;
          }
          ++num1;
          plusYneighbor = plusYneighbor.PlusYNeighbor;
        }
      }
      else
      {
        int num3 = 0;
        while (num3 <= 4)
        {
          TerrainTile terrainTile = plusYneighbor;
          int num4 = 0;
          while (num4 <= 4)
          {
            if (!action(terrainTile, this.GetTargetHeightAt((Tile2i) terrainTile.TileCoordSlim)))
              return new TerrainTile?(terrainTile);
            ++num4;
            terrainTile = terrainTile.PlusXNeighbor;
          }
          ++num3;
          plusYneighbor = plusYneighbor.PlusYNeighbor;
        }
      }
      return new TerrainTile?();
    }

    public TerrainTile? ForEachTileOrBreak(
      Func<TerrainDesignation, TerrainTile, RelTile2i, bool> action)
    {
      if (this.Manager.IsNone)
      {
        Log.Error("ForEach tile on a designation can be only called on managed managed designations.");
        return new TerrainTile?();
      }
      TerrainTile plusYneighbor = this.Manager.Value.TerrainManager[this.OriginTileCoord];
      int y = 0;
      while (y <= 4)
      {
        TerrainTile terrainTile = plusYneighbor;
        int x = 0;
        while (x <= 4)
        {
          if (!action(this, terrainTile, new RelTile2i(x, y)))
            return new TerrainTile?(terrainTile);
          ++x;
          terrainTile = terrainTile.PlusXNeighbor;
        }
        ++y;
        plusYneighbor = plusYneighbor.PlusYNeighbor;
      }
      return new TerrainTile?();
    }

    public void ForEachNonFulfilledTileCoord(Action<Tile2i> action)
    {
      for (int y = 0; y <= 4; ++y)
      {
        for (int x = 0; x <= 4; ++x)
        {
          RelTile2i coord = new RelTile2i(x, y);
          if (!this.IsFulfilledAt(coord))
            action(this.Data.OriginTile + coord);
        }
      }
    }

    public void ForEachNonFulfilledTile(Action<TerrainTile> action)
    {
      if (this.Manager.IsNone)
      {
        Log.Error("ForEachTile on a designation can be only performed on managed managed designations.");
      }
      else
      {
        TerrainTile plusYneighbor = this.Manager.Value.TerrainManager[this.OriginTileCoord];
        int y = 0;
        while (y <= 4)
        {
          TerrainTile terrainTile = plusYneighbor;
          int x = 0;
          while (x <= 4)
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
    }

    public Percent RatioOfNonFulfilledTilesWithoutProduct(LooseProductProto productProto)
    {
      TerrainTile plusYneighbor = this.Manager.Value.TerrainManager[this.OriginTileCoord];
      int num1 = 0;
      int numerator = 0;
      int num2 = 1;
      int y = 0;
      while (y <= 4)
      {
        TerrainTile terrainTile = plusYneighbor;
        int x = 0;
        while (x <= 4)
        {
          if (!this.IsFulfilledAt(new RelTile2i(x, y)))
          {
            if ((Proto) terrainTile.FirstLayer.Material.MinedProduct == (Proto) productProto)
              ++num1;
            else
              ++numerator;
          }
          x += num2;
          terrainTile = terrainTile.PlusXNeighbor;
        }
        y += num2;
        plusYneighbor = plusYneighbor.PlusYNeighbor;
      }
      int denominator = num1 + numerator;
      return denominator == 0 ? Percent.Zero : Percent.FromRatio(numerator, denominator);
    }

    /// <summary>
    /// Finds a tile with the minimal cost based on given cost function. The cost function can return
    /// <see cref="F:Mafi.Fix32.MaxValue" /> for tiles that should not be considered. If all tiles return that value, null
    /// is returned.
    /// </summary>
    public TerrainTile? FindBestTile(
      Func<TerrainDesignation, TerrainTile, RelTile2i, Fix32> costFn,
      out Fix32 minCost)
    {
      if (this.Manager.IsNone)
      {
        Log.Error("FindBestTile on a designation can be only called on managed managed designations.");
        minCost = new Fix32();
        return new TerrainTile?();
      }
      RelTile2i relTile2i1 = new RelTile2i();
      minCost = Fix32.MaxValue;
      TerrainTile plusYneighbor = this.Manager.Value.TerrainManager[this.OriginTileCoord];
      int y = 0;
      while (y <= 4)
      {
        TerrainTile terrainTile = plusYneighbor;
        int x = 0;
        while (x <= 4)
        {
          RelTile2i relTile2i2 = new RelTile2i(x, y);
          Fix32 fix32 = costFn(this, terrainTile, relTile2i2);
          if (fix32 < minCost)
          {
            relTile2i1 = relTile2i2;
            minCost = fix32;
          }
          ++x;
          terrainTile = terrainTile.PlusXNeighbor;
        }
        ++y;
        plusYneighbor = plusYneighbor.PlusYNeighbor;
      }
      return !(minCost == Fix32.MaxValue) ? new TerrainTile?(this.Manager.Value.TerrainManager[this.OriginTileCoord + relTile2i1]) : new TerrainTile?();
    }

    public Tile2i? FindBestNonFulfilledTileCoord<T>(
      Func<TerrainDesignation, TerrainTile, RelTile2i, T> costFn,
      Option<Predicate<RelTile2i>> predicate,
      out T minCost)
      where T : IComparable<T>
    {
      minCost = default (T);
      if (this.Manager.IsNone)
      {
        Log.Error("FindBestTile on a designation can be only called on managed designations.");
        return new Tile2i?();
      }
      RelTile2i relTile2i = new RelTile2i();
      bool flag = false;
      TerrainTile plusYneighbor = this.Manager.Value.TerrainManager[this.OriginTileCoord];
      Comparer<T> comparer = Comparer<T>.Default;
      int y = 0;
      while (y <= 4)
      {
        TerrainTile terrainTile = plusYneighbor;
        int x1 = 0;
        while (x1 <= 4)
        {
          RelTile2i coord = new RelTile2i(x1, y);
          if (!this.IsFulfilledAt(coord) && (!predicate.HasValue || predicate.Value(coord)))
          {
            T x2 = costFn(this, terrainTile, coord);
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

    /// <summary>
    /// Returns bit mask for given absolute position. This position must be on this designation.
    /// </summary>
    private uint getMask(Tile2i position)
    {
      return TerrainDesignation.getMask(position - this.Data.OriginTile);
    }

    /// <summary>
    /// Returns bit mask for given relative coordinate. This coordinate must be on this designation.
    /// </summary>
    private static uint getMask(RelTile2i coord) => (uint) (1 << coord.X + coord.Y * 5);

    /// <summary>
    /// Updates all bitmap states for the given tile. This should be called whenever a tile is changed.
    /// </summary>
    private void updateBitmaps(Tile2iAndIndex tileAndIndex, HeightTilesF designationHeight)
    {
      RelTile2i tileCoord = tileAndIndex.TileCoord - this.OriginTileCoord;
      bool flag = tileCoord.X == 4 || tileCoord.Y == 4;
      bool isFulfilled = this.Prototype.IsFulfilledFn((ITerrainDesignationsManager) this.Manager.Value, tileAndIndex, designationHeight, flag);
      this.setFulfilledAt(tileCoord, isFulfilled);
      if (this.Prototype.IsFulfilledMiningFn.HasValue)
      {
        if (this.Prototype.IsFulfilledDumpingFn.IsNone)
          this.m_tilesFulfilledMiningBitmap = this.m_tilesFulfilledBitmap;
        else
          this.setMiningFulfilledAt(tileCoord, this.Prototype.IsFulfilledMiningFn.Value((ITerrainDesignationsManager) this.Manager.Value, tileAndIndex, designationHeight, flag));
      }
      if (!this.Prototype.IsFulfilledDumpingFn.HasValue)
        return;
      if (this.Prototype.IsFulfilledMiningFn.IsNone)
        this.m_tilesFulfilledDumpingBitmap = this.m_tilesFulfilledBitmap;
      else
        this.setDumpingFulfilledAt(tileCoord, this.Prototype.IsFulfilledDumpingFn.Value((ITerrainDesignationsManager) this.Manager.Value, tileAndIndex, designationHeight, flag));
    }

    private void setFulfilledAt(RelTile2i tileCoord, bool isFulfilled)
    {
      uint mask = TerrainDesignation.getMask(tileCoord);
      if (isFulfilled)
        this.m_tilesFulfilledBitmap |= mask;
      else
        this.m_tilesFulfilledBitmap &= ~mask;
    }

    private void setMiningFulfilledAt(RelTile2i tileCoord, bool isFulfilled)
    {
      uint mask = TerrainDesignation.getMask(tileCoord);
      if (isFulfilled)
        this.m_tilesFulfilledMiningBitmap |= mask;
      else
        this.m_tilesFulfilledMiningBitmap &= ~mask;
    }

    private void setDumpingFulfilledAt(RelTile2i tileCoord, bool isFulfilled)
    {
      uint mask = TerrainDesignation.getMask(tileCoord);
      if (isFulfilled)
        this.m_tilesFulfilledDumpingBitmap |= mask;
      else
        this.m_tilesFulfilledDumpingBitmap &= ~mask;
    }

    internal bool ValidateAndFixFulfilledBitmap(Lyst<KeyValuePair<Tile2i, bool>> errorLocations)
    {
      bool flag1 = true;
      TerrainManager terrainManager = this.Manager.Value.TerrainManager;
      for (int y = 0; y <= 4; ++y)
      {
        for (int x = 0; x <= 4; ++x)
        {
          RelTile2i relTile2i = new RelTile2i(x, y);
          bool flag2 = x == 4 || y == 4;
          Tile2i tile2i = this.OriginTileCoord + relTile2i;
          Tile2iAndIndex tile2iAndIndex = terrainManager.ExtendTileIndex(tile2i);
          HeightTilesF targetHeightAt = this.GetTargetHeightAt(relTile2i);
          bool isFulfilled1 = this.Prototype.IsFulfilledFn((ITerrainDesignationsManager) this.Manager.Value, tile2iAndIndex, targetHeightAt, flag2);
          bool flag3 = this.IsFulfilledAt(relTile2i);
          if (isFulfilled1 != flag3)
          {
            errorLocations.Add(Make.Kvp<Tile2i, bool>(tile2i, flag3));
            this.setFulfilledAt(relTile2i, isFulfilled1);
            flag1 = false;
          }
          if (this.Prototype.IsFulfilledMiningFn.HasValue)
          {
            bool isFulfilled2 = this.Prototype.IsFulfilledMiningFn.Value((ITerrainDesignationsManager) this.Manager.Value, tile2iAndIndex, targetHeightAt, flag2);
            bool flag4 = this.IsMiningFulfilledAt(relTile2i);
            if (isFulfilled2 != flag4)
            {
              errorLocations.Add(Make.Kvp<Tile2i, bool>(tile2i, flag4));
              this.setMiningFulfilledAt(relTile2i, isFulfilled2);
              flag1 = false;
            }
          }
          if (this.Prototype.IsFulfilledDumpingFn.HasValue)
          {
            bool isFulfilled3 = this.Prototype.IsFulfilledDumpingFn.Value((ITerrainDesignationsManager) this.Manager.Value, tile2iAndIndex, targetHeightAt, flag2);
            bool flag5 = this.IsDumpingFulfilledAt(relTile2i);
            if (isFulfilled3 != flag5)
            {
              errorLocations.Add(Make.Kvp<Tile2i, bool>(tile2i, flag5));
              this.setDumpingFulfilledAt(relTile2i, isFulfilled3);
              flag1 = false;
            }
          }
        }
      }
      return flag1;
    }

    public bool Equals(TerrainDesignation other)
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
      if (!(obj is TerrainDesignation terrainDesignation) || !this.OriginTileCoord.Equals(terrainDesignation.OriginTileCoord) || !this.Prototype.Equals((Proto) terrainDesignation.Prototype))
        return false;
      Log.Error("Area designations that are not equal references have equal position and proto.");
      return true;
    }

    public override int GetHashCode()
    {
      return Hash.Combine<TerrainDesignationProto, Tile2i>(this.Prototype, this.OriginTileCoord);
    }

    public override string ToString()
    {
      return string.Format("{0} at {1}", (object) this.Prototype?.Id, (object) this.Origin3i) + (this.IsDestroyed ? " (destroyed)" : "");
    }

    public void Cheat_SetTerrainToMatch()
    {
      this.ForEachTile((Action<TerrainTile, HeightTilesF>) ((t, h) => t.TerrainManager.SetHeight(t.CoordAndIndex, h)));
    }

    public int CountFulfilledPerimeterTiles()
    {
      return (this.m_tilesFulfilledBitmap & 33080895U).CountSetBits();
    }

    public int CountFulfilledTiles() => this.m_tilesFulfilledBitmap.CountSetBits();

    public bool IsCloseToTerrain(ThicknessTilesI maxDeltaApprox)
    {
      Assert.That<Option<ITerrainDesignationsManagerInternal>>(this.Manager).HasValue<ITerrainDesignationsManagerInternal>();
      TerrainManager terrainManager = this.Manager.Value.TerrainManager;
      ThicknessTilesF thicknessTilesF = this.Data.OriginTargetHeight.HeightTilesF - terrainManager[this.OriginTileCoord].Height;
      thicknessTilesF = thicknessTilesF.Abs;
      thicknessTilesF = thicknessTilesF.Min((this.Data.PlusXTargetHeight.HeightTilesF - terrainManager[this.PlusXTileCoord].Height).Abs);
      thicknessTilesF = thicknessTilesF.Min((this.Data.PlusXyTargetHeight.HeightTilesF - terrainManager[this.PlusXyTileCoord].Height).Abs);
      return thicknessTilesF.Min((this.Data.PlusYTargetHeight.HeightTilesF - terrainManager[this.PlusYTileCoord].Height).Abs).FlooredThicknessTilesI < maxDeltaApprox;
    }

    public bool IsSnappedTowards(NeighborCoord direction)
    {
      Assert.That<Option<ITerrainDesignationsManagerInternal>>(this.Manager).HasValue<ITerrainDesignationsManagerInternal>();
      switch (direction.Index)
      {
        case 0:
          Option<TerrainDesignation> plusXneighbor = this.PlusXNeighbor;
          return plusXneighbor.HasValue && plusXneighbor.Value.Data.OriginTargetHeight == this.Data.PlusXTargetHeight && plusXneighbor.Value.Data.PlusYTargetHeight == this.Data.PlusXyTargetHeight;
        case 1:
          Option<TerrainDesignation> minusXneighbor = this.MinusXNeighbor;
          return minusXneighbor.HasValue && minusXneighbor.Value.Data.PlusXTargetHeight == this.Data.OriginTargetHeight && minusXneighbor.Value.Data.PlusXyTargetHeight == this.Data.PlusYTargetHeight;
        case 2:
          Option<TerrainDesignation> plusYneighbor = this.PlusYNeighbor;
          return plusYneighbor.HasValue && plusYneighbor.Value.Data.OriginTargetHeight == this.Data.PlusYTargetHeight && plusYneighbor.Value.Data.PlusXTargetHeight == this.Data.PlusXyTargetHeight;
        case 3:
          Option<TerrainDesignation> minusYneighbor = this.MinusYNeighbor;
          return minusYneighbor.HasValue && minusYneighbor.Value.Data.PlusYTargetHeight == this.Data.OriginTargetHeight && minusYneighbor.Value.Data.PlusXyTargetHeight == this.Data.PlusXTargetHeight;
        default:
          Assert.Fail("Invalid neighbor coord.");
          return false;
      }
    }

    private void addTowerInternal(IAreaManagingTower tower)
    {
      if (this.m_tower.IsNone)
      {
        this.m_tower = Option<IAreaManagingTower>.Create(tower);
      }
      else
      {
        Assert.That<Option<IAreaManagingTower>>(this.m_tower).IsNotEqualTo<IAreaManagingTower>(tower);
        this.m_towers.AddIfNotPresent(tower).AssertTrue();
      }
    }

    private void removeTowerInternal(IAreaManagingTower tower)
    {
      if (this.m_tower == tower)
      {
        this.m_tower = Option<IAreaManagingTower>.None;
        if (this.m_towers.Count <= 0)
          return;
        this.m_tower = Option<IAreaManagingTower>.Create(this.m_towers.PopLast());
        this.m_towers.FreeUpBackingArrayIfEmpty();
      }
      else
      {
        this.m_towers.RemoveAndAssert(tower);
        this.m_towers.FreeUpBackingArrayIfEmpty();
      }
    }

    public bool IsManagedByTower(IEntityAssignedWithVehicles tower)
    {
      return this.ManagedByTowers.Contains((IEntity) tower);
    }

    static TerrainDesignation()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainDesignation.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainDesignation) obj).SerializeData(writer));
      TerrainDesignation.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainDesignation) obj).DeserializeData(reader));
      TerrainDesignation.MASK_FOR_EDGE = new ReadOnlyArray<uint>(new uint[4]
      {
        17318416U,
        1082401U,
        32505856U,
        31U
      });
      TerrainDesignation.SURFACE_HEIGHT_TOLERANCE = new HeightTilesF((1.0 / 16.0).ToFix32());
    }

    public readonly struct AssignedTowers
    {
      private readonly TerrainDesignation m_designation;

      public AssignedTowers(TerrainDesignation designation)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_designation = designation;
      }

      public int Count
      {
        get => !this.m_designation.m_tower.IsNone ? 1 + this.m_designation.m_towers.Count : 0;
      }

      public bool IsEmpty() => this.m_designation.m_tower.IsNone;

      public bool Contains(IAreaManagingTower tower)
      {
        if (this.m_designation.m_tower.IsNone)
          return false;
        return this.m_designation.m_tower == tower || this.m_designation.m_towers.Contains(tower);
      }

      public bool Contains(IEntity tower)
      {
        if (this.m_designation.m_tower.IsNone)
          return false;
        if (this.m_designation.m_tower.ValueOrNull == tower)
          return true;
        foreach (IAreaManagingTower tower1 in this.m_designation.m_towers)
        {
          if (tower1 == tower)
            return true;
        }
        return false;
      }

      public TerrainDesignation.AssignedTowers.Enumerator GetEnumerator()
      {
        return new TerrainDesignation.AssignedTowers.Enumerator(this.m_designation);
      }

      public struct Enumerator
      {
        private readonly TerrainDesignation m_designation;
        private int m_index;

        internal Enumerator(TerrainDesignation designation)
        {
          MBiHIp97M4MqqbtZOh.rMWAw2OR8();
          this.m_designation = designation;
          this.m_index = 0;
          this.Current = (IAreaManagingTower) null;
        }

        public IAreaManagingTower Current { get; private set; }

        public bool MoveNext()
        {
          if (this.m_index == 0)
          {
            if (this.m_designation.m_tower.IsNone)
              return false;
            this.Current = this.m_designation.m_tower.Value;
            ++this.m_index;
            return true;
          }
          if (this.m_index - 1 >= this.m_designation.m_towers.Count)
            return false;
          this.Current = this.m_designation.m_towers[this.m_index - 1];
          ++this.m_index;
          return true;
        }
      }
    }
  }
}
