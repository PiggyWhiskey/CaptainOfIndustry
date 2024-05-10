// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.TerrainOccupancyManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Serialization;
using Mafi.Utils;
using System;

#nullable disable
namespace Mafi.Core.Terrain
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TerrainOccupancyManager : 
    IEntityAdditionValidator<IEntityWithOccupiedTilesAddRequest>,
    IEntityAdditionValidator
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Event<Tile2iAndIndex> m_tileOccupancyChanged;
    public readonly TerrainManager TerrainManager;
    private readonly IEntitiesManager m_entitiesManager;
    [DoNotSave(0, null)]
    private TerrainOccupancyManager.OccupancyData[] m_data;
    [DoNotSave(0, null)]
    private LystStruct<TerrainOccupancyManager.OccupancyData> m_overflowData;
    [DoNotSave(0, null)]
    private LystStruct<int> m_freeOverflowIndices;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<Tile3i> m_selfCollisionSetTmp;

    public static void Serialize(TerrainOccupancyManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainOccupancyManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainOccupancyManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      Event<Tile2iAndIndex>.Serialize(this.m_tileOccupancyChanged, writer);
      TerrainManager.Serialize(this.TerrainManager, writer);
    }

    public static TerrainOccupancyManager Deserialize(BlobReader reader)
    {
      TerrainOccupancyManager occupancyManager;
      if (reader.TryStartClassDeserialization<TerrainOccupancyManager>(out occupancyManager))
        reader.EnqueueDataDeserialization((object) occupancyManager, TerrainOccupancyManager.s_deserializeDataDelayedAction);
      return occupancyManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TerrainOccupancyManager>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      reader.SetField<TerrainOccupancyManager>(this, "m_selfCollisionSetTmp", (object) new Set<Tile3i>());
      reader.SetField<TerrainOccupancyManager>(this, "m_tileOccupancyChanged", (object) Event<Tile2iAndIndex>.Deserialize(reader));
      reader.SetField<TerrainOccupancyManager>(this, "TerrainManager", (object) TerrainManager.Deserialize(reader));
      reader.RegisterInitAfterLoad<TerrainOccupancyManager>(this, "allocateData", InitPriority.Highest);
      reader.RegisterInitAfterLoad<TerrainOccupancyManager>(this, "reconstructOccupancy", InitPriority.High);
    }

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    /// <summary>
    /// Invoked when tile occupancy changes. Invoked on the simulation thread.
    /// </summary>
    public IEvent<Tile2iAndIndex> TileOccupancyChanged
    {
      get => (IEvent<Tile2iAndIndex>) this.m_tileOccupancyChanged;
    }

    public TerrainOccupancyManager(TerrainManager terrainManager, IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_tileOccupancyChanged = new Event<Tile2iAndIndex>();
      this.m_selfCollisionSetTmp = new Set<Tile3i>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.TerrainManager = terrainManager;
      this.m_entitiesManager = entitiesManager;
      this.allocateData();
      entitiesManager.StaticEntityAdded.Add<TerrainOccupancyManager>(this, new Action<IStaticEntity>(this.entityAdded));
      entitiesManager.StaticEntityRemoved.Add<TerrainOccupancyManager>(this, new Action<IStaticEntity>(this.entityRemoved));
      entitiesManager.OnUpgradeToBePerformed.Add<TerrainOccupancyManager>(this, new Action<IUpgradableEntity>(this.beforeUpgrade));
      entitiesManager.OnUpgradeJustPerformed.Add<TerrainOccupancyManager>(this, new Action<IUpgradableEntity, IEntityProto>(this.afterUpgrade));
    }

    [InitAfterLoad(InitPriority.Highest)]
    private void allocateData()
    {
      Assert.That<int>(this.TerrainManager.TerrainTilesCount).IsPositive();
      this.m_data = new TerrainOccupancyManager.OccupancyData[this.TerrainManager.TerrainTilesCount];
    }

    [InitAfterLoad(InitPriority.High)]
    private void reconstructOccupancy()
    {
      foreach (IEntity entity in this.m_entitiesManager.Entities)
      {
        if (entity is IStaticEntity staticEntity)
        {
          if (staticEntity.IsDestroyed)
          {
            Log.Warning(string.Format("Loaded destroyed entity: {0}", (object) staticEntity));
          }
          else
          {
            ImmutableArray<OccupiedTileRelative> occupiedTiles = staticEntity.OccupiedTiles;
            if (occupiedTiles.IsEmpty)
            {
              Log.Warning(string.Format("Static entity '{0}' has no occupied tiles.", (object) staticEntity));
              break;
            }
            EntityId id = staticEntity.Id;
            Tile3i centerTile = staticEntity.CenterTile;
            foreach (OccupiedTileRelative occupiedTileRelative in occupiedTiles)
              this.addOccupiedRecord_noEvents(this.TerrainManager.GetTileIndex(centerTile.Xy + occupiedTileRelative.RelCoord), id, centerTile.Height + occupiedTileRelative.FromHeightRel, occupiedTileRelative.VerticalSize);
          }
        }
      }
    }

    private void beforeUpgrade(IUpgradableEntity entity)
    {
      IStaticEntity entity1 = (IStaticEntity) entity;
      if (entity1 == null)
        return;
      this.entityRemoved(entity1);
    }

    private void afterUpgrade(IUpgradableEntity entity, IEntityProto proto)
    {
      IStaticEntity entity1 = (IStaticEntity) entity;
      if (entity1 == null)
        return;
      this.entityAdded(entity1);
    }

    public bool IsOccupied(Tile2i tile)
    {
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      uint index = (uint) this.TerrainManager.GetTileIndex(tile).Value;
      if (index < (uint) data.Length)
        return data[(int) index].EntityId.IsValid;
      Log.Error("IsOccupied: Tile index out of bounds");
      return false;
    }

    public bool IsOccupied(Tile2iIndex index)
    {
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) index.Value < (uint) data.Length)
        return data[index.Value].EntityId.IsValid;
      Log.Error("IsOccupied: Tile index out of bounds");
      return false;
    }

    /// <summary>
    /// Returns the lowest occupying entity on this tile. If there are no occupying entities the returned ID will be
    /// <see cref="F:Mafi.Core.EntityId.Invalid" />.
    /// </summary>
    public EntityId GetLowestOccupyingEntity(Tile2iIndex index)
    {
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) index.Value < (uint) data.Length)
        return data[index.Value].EntityId;
      Log.Error("GetLowestOccupyingEntity: Tile index out of bounds");
      return new EntityId();
    }

    public bool IsOccupiedAt(
      Tile2iIndex index,
      HeightTilesI height,
      Predicate<EntityId> ignoredIds = null)
    {
      return this.TryGetAnyOccupyingEntityInRange(index, height, ThicknessTilesI.One, out EntityId _, ignoredIds);
    }

    public bool IsOccupiedAt(Tile3i position, Predicate<EntityId> ignoredIds = null)
    {
      return this.TryGetAnyOccupyingEntityInRange(this.TerrainManager.GetTileIndex(position.Xy), position.Height, ThicknessTilesI.One, out EntityId _, ignoredIds);
    }

    public bool IsOccupiedInRange(
      Tile3i basePosition,
      ThicknessTilesI verticalSize,
      Predicate<EntityId> ignoredIds = null)
    {
      return this.TryGetAnyOccupyingEntityInRange(this.TerrainManager.GetTileIndex(basePosition.Xy), basePosition.Height, verticalSize, out EntityId _, ignoredIds);
    }

    public bool TryGetAnyOccupyingEntityAt(
      Tile3i position,
      out EntityId entityId,
      Predicate<EntityId> ignoredIds = null)
    {
      return this.TryGetAnyOccupyingEntityInRange(this.TerrainManager.GetTileIndex(position.Xy), position.Height, ThicknessTilesI.One, out entityId, ignoredIds);
    }

    public bool TryGetAnyOccupyingEntityExcept(
      Tile2iIndex index,
      EntityId ignoredId,
      out EntityId entityId)
    {
      entityId = EntityId.Invalid;
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) index.Value >= (uint) data.Length)
      {
        Log.Error("TryGetAnyOccupyingEntityExcept: Tile index out of bounds");
        return false;
      }
      TerrainOccupancyManager.OccupancyData occupancyData1 = data[index.Value];
      if (!occupancyData1.EntityId.IsValid)
        return false;
      if (occupancyData1.EntityId != ignoredId)
      {
        entityId = occupancyData1.EntityId;
        return true;
      }
      TerrainOccupancyManager.OccupancyData occupancyData2;
      for (int overflowIndex = occupancyData1.OverflowIndex; overflowIndex >= 0; overflowIndex = occupancyData2.OverflowIndex)
      {
        occupancyData2 = this.m_overflowData[overflowIndex];
        if (occupancyData2.EntityId != ignoredId)
        {
          entityId = occupancyData2.EntityId;
          return true;
        }
      }
      return false;
    }

    public bool TryGetOccupyingEntity(
      Tile2iIndex index,
      Predicate<IStaticEntity> predicate,
      out IStaticEntity entity)
    {
      entity = (IStaticEntity) null;
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) index.Value >= (uint) data.Length)
      {
        Log.Error("TryGetOccupyingEntity: Tile index out of bounds");
        return false;
      }
      TerrainOccupancyManager.OccupancyData occupancyData1 = data[index.Value];
      if (!occupancyData1.EntityId.IsValid)
        return false;
      if (this.m_entitiesManager.TryGetEntity<IStaticEntity>(occupancyData1.EntityId, out entity) && predicate(entity))
        return true;
      int overflowIndex = occupancyData1.OverflowIndex;
      while (overflowIndex >= 0)
      {
        TerrainOccupancyManager.OccupancyData occupancyData2 = this.m_overflowData[overflowIndex];
        overflowIndex = occupancyData2.OverflowIndex;
        if (this.m_entitiesManager.TryGetEntity<IStaticEntity>(occupancyData2.EntityId, out entity) && predicate(entity))
          return true;
      }
      return false;
    }

    public bool TryGetOccupyingEntityAt<TEntity>(
      Tile3i position,
      out TEntity entity,
      Predicate<EntityId> ignoredIds = null)
      where TEntity : class, IStaticEntity
    {
      return this.TryGetOccupyingEntityInRange<TEntity>(this.TerrainManager.GetTileIndex(position.Xy), position.Height, ThicknessTilesI.One, this.m_entitiesManager, out entity, ignoredIds);
    }

    public bool TryGetOccupyingEntityInRange<TEntity>(
      Tile3i basePosition,
      ThicknessTilesI verticalSize,
      out TEntity entity,
      Predicate<EntityId> ignoredIds = null)
      where TEntity : class, IStaticEntity
    {
      return this.TryGetOccupyingEntityInRange<TEntity>(this.TerrainManager.GetTileIndex(basePosition.Xy), basePosition.Height, verticalSize, this.m_entitiesManager, out entity, ignoredIds);
    }

    public bool TryGetAnyOccupyingEntityInRange(
      Tile3i basePosition,
      ThicknessTilesI verticalSize,
      out EntityId entityId,
      Predicate<EntityId> ignoredIds = null)
    {
      return this.TryGetAnyOccupyingEntityInRange(this.TerrainManager.GetTileIndex(basePosition.Xy), basePosition.Height, verticalSize, out entityId, ignoredIds);
    }

    /// <summary>
    /// Returns any occupying entity in give range on this tile, or null when there are none.
    /// The <paramref name="ignoredIds" /> parameter can specify ignored IDs.
    /// </summary>
    /// <remarks>
    /// Implementation needs to be in sync with <see cref="M:Mafi.Core.Terrain.TerrainOccupancyManager.TryGetOccupyingEntityInRange``1(Mafi.Tile3i,Mafi.ThicknessTilesI,``0@,System.Predicate{Mafi.Core.EntityId})" />.
    /// </remarks>
    public bool TryGetAnyOccupyingEntityInRange(
      Tile2iIndex index,
      HeightTilesI from,
      ThicknessTilesI verticalSize,
      out EntityId entityId,
      Predicate<EntityId> ignoredIds = null)
    {
      entityId = EntityId.Invalid;
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) index.Value >= (uint) data.Length)
      {
        Log.Error(string.Format("TryGetOccupyingEntity: Tile index out of bounds, index {0} {1}, arr len {2}, off limits {3}", (object) index, (object) this.TerrainManager.IndexToTile_Slow(index), (object) data.Length, this.TerrainManager.OffLimitsDisabled ? (object) "no" : (object) "yes"));
        return false;
      }
      TerrainOccupancyManager.OccupancyData occupancyData1 = data[index.Value];
      if (!occupancyData1.EntityId.IsValid || from + verticalSize <= occupancyData1.From)
        return false;
      if (from < occupancyData1.From + occupancyData1.VerticalSize && (ignoredIds == null || !ignoredIds(occupancyData1.EntityId)))
      {
        entityId = occupancyData1.EntityId;
        return true;
      }
      int overflowIndex = occupancyData1.OverflowIndex;
      while (overflowIndex >= 0)
      {
        TerrainOccupancyManager.OccupancyData occupancyData2 = this.m_overflowData[overflowIndex];
        overflowIndex = occupancyData2.OverflowIndex;
        if (!(from + verticalSize <= occupancyData2.From) && from < occupancyData2.From + occupancyData2.VerticalSize && (ignoredIds == null || !ignoredIds(occupancyData2.EntityId)))
        {
          entityId = occupancyData2.EntityId;
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Returns the first entity of type <typeparamref name="TEntity" /> occupying entity in give range on this tile,
    /// or null when there are none.
    /// The <paramref name="ignoredIds" /> parameter can specify ignored IDs.
    /// </summary>
    /// <remarks>
    /// Implementation needs to be in sync with <see cref="M:Mafi.Core.Terrain.TerrainOccupancyManager.TryGetAnyOccupyingEntityInRange(Mafi.Tile3i,Mafi.ThicknessTilesI,Mafi.Core.EntityId@,System.Predicate{Mafi.Core.EntityId})" />.
    /// </remarks>
    public bool TryGetOccupyingEntityInRange<TEntity>(
      Tile2iIndex index,
      HeightTilesI from,
      ThicknessTilesI verticalSize,
      IEntitiesManager entitiesManager,
      out TEntity entity,
      Predicate<EntityId> ignoredIds = null)
      where TEntity : class, IStaticEntity
    {
      entity = default (TEntity);
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) index.Value >= (uint) data.Length)
      {
        Log.Error("TryGetOccupyingEntityInRange: Tile index out of bounds");
        return false;
      }
      TerrainOccupancyManager.OccupancyData occupancyData1 = data[index.Value];
      if (!occupancyData1.EntityId.IsValid || from + verticalSize <= occupancyData1.From)
        return false;
      if (from < occupancyData1.From + occupancyData1.VerticalSize && (ignoredIds == null || !ignoredIds(occupancyData1.EntityId)) && entitiesManager.TryGetEntity<TEntity>(occupancyData1.EntityId, out entity))
        return true;
      int overflowIndex = occupancyData1.OverflowIndex;
      while (overflowIndex >= 0)
      {
        TerrainOccupancyManager.OccupancyData occupancyData2 = this.m_overflowData[overflowIndex];
        overflowIndex = occupancyData2.OverflowIndex;
        if (!(from + verticalSize <= occupancyData2.From) && from < occupancyData2.From + occupancyData2.VerticalSize && (ignoredIds == null || !ignoredIds(occupancyData2.EntityId)) && entitiesManager.TryGetEntity<TEntity>(occupancyData2.EntityId, out entity))
          return true;
      }
      return false;
    }

    public void GetAllOccupiedEntitiesAt(Tile2iIndex index, Lyst<IStaticEntity> occupiedEntities)
    {
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) index.Value >= (uint) data.Length)
      {
        Log.Error("GetAllOccupiedEntitiesAt: Tile index out of bounds");
      }
      else
      {
        TerrainOccupancyManager.OccupancyData occupancyData1 = data[index.Value];
        if (!occupancyData1.EntityId.IsValid)
          return;
        IStaticEntity entity;
        if (this.m_entitiesManager.TryGetEntity<IStaticEntity>(occupancyData1.EntityId, out entity))
          occupiedEntities.Add(entity);
        else
          Log.Warning("Occupied entity not found.");
        TerrainOccupancyManager.OccupancyData occupancyData2;
        for (int overflowIndex = occupancyData1.OverflowIndex; overflowIndex >= 0; overflowIndex = occupancyData2.OverflowIndex)
        {
          occupancyData2 = this.m_overflowData[overflowIndex];
          if (this.m_entitiesManager.TryGetEntity<IStaticEntity>(occupancyData2.EntityId, out entity))
            occupiedEntities.Add(entity);
          else
            Log.Warning("Occupied entity not found.");
        }
      }
    }

    public ThicknessTilesI GetClearanceAt(Tile3i position)
    {
      return this.GetHeightClearance(this.TerrainManager.GetTileIndex(position.Xy), position.Height);
    }

    public ThicknessTilesI GetHeightClearance(Tile2iIndex index, HeightTilesI fromHeight)
    {
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) index.Value >= (uint) data.Length)
      {
        Log.Error("GetHeightClearance: Tile index out of bounds");
        return ThicknessTilesI.MaxValue;
      }
      TerrainOccupancyManager.OccupancyData occupancyData1 = data[index.Value];
      if (!occupancyData1.EntityId.IsValid)
        return ThicknessTilesI.MaxValue;
      if (fromHeight <= occupancyData1.From)
        return occupancyData1.From - fromHeight;
      if (fromHeight < occupancyData1.From + occupancyData1.VerticalSize)
        return ThicknessTilesI.Zero;
      int overflowIndex = occupancyData1.OverflowIndex;
      int self = int.MaxValue;
      TerrainOccupancyManager.OccupancyData occupancyData2;
      for (; overflowIndex >= 0; overflowIndex = occupancyData2.OverflowIndex)
      {
        occupancyData2 = this.m_overflowData[overflowIndex];
        if (fromHeight < occupancyData2.From)
          self = self.Min(occupancyData2.From.Value - fromHeight.Value);
        else if (fromHeight < occupancyData2.From + occupancyData2.VerticalSize)
          return ThicknessTilesI.Zero;
      }
      return new ThicknessTilesI(self);
    }

    public bool CanAdd(
      ReadOnlyArraySlice<OccupiedTileRange> occupiedTiles,
      out EntityId blockingEntityId,
      Predicate<EntityId> ignoredIds = null)
    {
      if (occupiedTiles.IsEmpty)
      {
        blockingEntityId = EntityId.Invalid;
        return true;
      }
      foreach (OccupiedTileRange occupiedTile in occupiedTiles)
      {
        if (this.TryGetAnyOccupyingEntityInRange(this.TerrainManager.GetTileIndex(occupiedTile.Position), occupiedTile.From, occupiedTile.VerticalSize, out blockingEntityId, ignoredIds))
          return false;
      }
      blockingEntityId = EntityId.Invalid;
      return true;
    }

    public bool CanAdd(
      Tile3i origin,
      ReadOnlyArraySlice<OccupiedTileRelative> occupiedTiles,
      out EntityId blockingEntityId,
      Predicate<EntityId> ignoredIds = null,
      Action<int> reportFailures = null)
    {
      blockingEntityId = EntityId.Invalid;
      if (occupiedTiles.IsEmpty)
        return true;
      bool flag = true;
      for (int index = 0; index < occupiedTiles.Length; ++index)
      {
        OccupiedTileRelative occupiedTile = occupiedTiles[index];
        EntityId entityId;
        if (this.TryGetAnyOccupyingEntityInRange(this.TerrainManager.GetTileIndex(origin.Xy + occupiedTile.RelCoord), origin.Height + new ThicknessTilesI((int) occupiedTile.RelativeFrom), occupiedTile.VerticalSize, out entityId, ignoredIds))
        {
          blockingEntityId = entityId;
          if (reportFailures == null)
            return false;
          reportFailures(index);
          flag = false;
        }
      }
      return flag;
    }

    public bool AreTilesSelfColliding(
      ReadOnlyArraySlice<OccupiedTileRange> occupiedTiles)
    {
      this.m_selfCollisionSetTmp.Clear();
      return TerrainOccupancyManager.AreTilesSelfColliding(occupiedTiles, this.m_selfCollisionSetTmp);
    }

    public static bool AreTilesSelfColliding(
      ReadOnlyArraySlice<OccupiedTileRange> occupiedTiles,
      Set<Tile3i> occupiedTilesSet)
    {
      if (occupiedTiles.IsEmpty)
        return false;
      foreach (OccupiedTileRange occupiedTile in occupiedTiles)
      {
        for (int index = 0; index < occupiedTile.VerticalSize.Value; ++index)
        {
          Tile3i tile3i = occupiedTile.Position.ExtendHeight(occupiedTile.From + index.TilesThick());
          if (!occupiedTilesSet.Add(tile3i))
            return true;
        }
      }
      return false;
    }

    public static bool OccupiedTilesContains(ReadOnlyArray<OccupiedTileRelative> tiles, RelTile3i t)
    {
      foreach (OccupiedTileRelative tile in tiles)
      {
        if (tile.RelCoord == t.Xy && t.Thickness >= tile.FromHeightRel && t.Thickness < tile.ToHeightRelExcl)
          return true;
      }
      return false;
    }

    public static bool OccupiedTilesContains(ReadOnlyArray<OccupiedTileRange> tiles, Tile3i t)
    {
      foreach (OccupiedTileRange tile in tiles)
      {
        if ((Tile2i) tile.Position == t.Xy && tile.From >= t.Height && tile.From + tile.VerticalSize < t.Height)
          return true;
      }
      return false;
    }

    private void entityAdded(IStaticEntity entity)
    {
      ImmutableArray<OccupiedTileRelative> occupiedTiles = entity.OccupiedTiles;
      if (occupiedTiles.IsEmpty)
      {
        Log.Warning(string.Format("Static entity '{0}' has no occupied tiles.", (object) entity));
      }
      else
      {
        EntityId id = entity.Id;
        Tile3i centerTile = entity.CenterTile;
        foreach (OccupiedTileRelative occupiedTileRelative in occupiedTiles)
        {
          Tile2iAndIndex tile2iAndIndex = this.TerrainManager.ExtendTileIndex(centerTile.Xy + occupiedTileRelative.RelCoord);
          this.addOccupiedRecord_noEvents(tile2iAndIndex.Index, id, centerTile.Height + occupiedTileRelative.FromHeightRel, occupiedTileRelative.VerticalSize);
          this.m_tileOccupancyChanged.Invoke(tile2iAndIndex);
        }
      }
    }

    private void entityRemoved(IStaticEntity entity)
    {
      ImmutableArray<OccupiedTileRelative> occupiedTiles = entity.OccupiedTiles;
      if (occupiedTiles.IsEmpty)
      {
        Log.Warning(string.Format("Static entity '{0}' has no occupied tiles.", (object) entity));
      }
      else
      {
        EntityId id = entity.Id;
        Tile3i centerTile = entity.CenterTile;
        foreach (OccupiedTileRelative occupiedTileRelative in occupiedTiles)
        {
          Tile2iAndIndex tile2iAndIndex = this.TerrainManager.ExtendTileIndex(centerTile.Xy + occupiedTileRelative.RelCoord);
          this.tryRemoveOccupiedRecord_noEvents(tile2iAndIndex.Index, id).AssertTrue();
          this.m_tileOccupancyChanged.Invoke(tile2iAndIndex);
        }
      }
    }

    EntityValidationResult IEntityAdditionValidator<IEntityWithOccupiedTilesAddRequest>.CanAdd(
      IEntityWithOccupiedTilesAddRequest request)
    {
      EntityId blockingEntityId;
      if (this.CanAdd(request.Origin, request.OccupiedTiles.AsSlice, out blockingEntityId, request.IgnoreForCollisions.ValueOrNull, request.RecordTileErrorsAndMetadata ? new Action<int>(request.SetTileError) : (Action<int>) null))
        return EntityValidationResult.Success;
      IEntity entity;
      if (this.m_entitiesManager.TryGetEntity<IEntity>(blockingEntityId, out entity))
      {
        Assert.That<string>(entity.Prototype.Strings.Name.TranslatedString).IsNotNullOrEmpty<string>("Collision with entity '{0}' that has no name", entity.Prototype.Id.Value);
        return EntityValidationResult.CreateError(TrCore.AdditionError__CollisionWith.Format(entity.Prototype.Strings.Name.TranslatedString).Value, "");
      }
      Log.Error("Blocked by unknown entity!");
      return EntityValidationResult.CreateError(TrCore.AdditionError__CollisionWith.Format("??").Value, "Blocked by unknown entity!");
    }

    internal void AddOccupiedRecord_TestOnly(
      Tile2iAndIndex tileAndIndex,
      EntityId id,
      HeightTilesI from,
      ThicknessTilesI height,
      bool doNotInvokeEvent = false)
    {
      this.addOccupiedRecord_noEvents(tileAndIndex.Index, id, from, height);
      if (doNotInvokeEvent)
        return;
      this.m_tileOccupancyChanged.Invoke(tileAndIndex);
    }

    private void addOccupiedRecord_noEvents(
      Tile2iIndex tileIndex,
      EntityId id,
      HeightTilesI from,
      ThicknessTilesI height)
    {
      Assert.That<bool>(id.IsValid).IsTrue("Adding occupancy for invalid entity ID");
      if (height.IsNotPositive)
      {
        Log.Warning(string.Format("Adding occupied entity '{0}' that has no height ({1}).", (object) id, (object) height));
      }
      else
      {
        TerrainOccupancyManager.OccupancyData[] data1 = this.m_data;
        if ((uint) tileIndex.Value >= (uint) data1.Length)
        {
          Log.Error("addOccupiedRecord_noEvents: Tile index out of bounds");
        }
        else
        {
          TerrainOccupancyManager.OccupancyData data2 = data1[tileIndex.Value];
          if (data2.EntityId.IsNotValid)
          {
            data1[tileIndex.Value] = new TerrainOccupancyManager.OccupancyData(id, from, height, -1);
          }
          else
          {
            Assert.That<bool>(data2.EntityId != id || from >= data2.ToExcl || from + height <= data2.From).IsTrue("Entity overlaps itself.");
            if (from < data2.From)
            {
              int occupancyOverflow = this.insertToOccupancyOverflow(data2);
              data1[tileIndex.Value] = new TerrainOccupancyManager.OccupancyData(id, from, height, occupancyOverflow);
            }
            else
            {
              int occupancyOverflow = this.insertToOccupancyOverflow(new TerrainOccupancyManager.OccupancyData(id, from, height, data2.OverflowIndex));
              data1[tileIndex.Value] = data2.WithNewOverflowIndex(occupancyOverflow);
            }
          }
        }
      }
    }

    private int insertToOccupancyOverflow(TerrainOccupancyManager.OccupancyData data)
    {
      int index;
      if (this.m_freeOverflowIndices.IsNotEmpty)
      {
        index = this.m_freeOverflowIndices.PopLast();
        this.m_overflowData[index] = data;
      }
      else
      {
        index = this.m_overflowData.Count;
        this.m_overflowData.Add(data);
      }
      return index;
    }

    internal bool TryRemoveOccupiedRecord_TestOnly(
      Tile2iAndIndex tileAndIndex,
      EntityId idToRemove,
      bool doNotInvokeEvent = false)
    {
      if (!this.tryRemoveOccupiedRecord_noEvents(tileAndIndex.Index, idToRemove))
        return false;
      if (!doNotInvokeEvent)
        this.m_tileOccupancyChanged.Invoke(tileAndIndex);
      return true;
    }

    private bool tryRemoveOccupiedRecord_noEvents(Tile2iIndex tileIndex, EntityId idToRemove)
    {
      Assert.That<bool>(idToRemove.IsValid).IsTrue("Removing occupancy for invalid entity ID");
      TerrainOccupancyManager.OccupancyData[] data = this.m_data;
      if ((uint) tileIndex.Value >= (uint) data.Length)
      {
        Log.Error("tryRemoveOccupiedRecord_noEvents: Tile index out of bounds");
        return false;
      }
      TerrainOccupancyManager.OccupancyData occupancyData1 = data[tileIndex.Value];
      if (occupancyData1.OverflowIndex < 0)
      {
        if (occupancyData1.EntityId != idToRemove)
          return false;
        data[tileIndex.Value] = new TerrainOccupancyManager.OccupancyData();
        return true;
      }
      if (occupancyData1.EntityId == idToRemove)
      {
        data[tileIndex.Value] = this.removeLowestFromOverflow(occupancyData1.OverflowIndex);
        return true;
      }
      int overflowIndex = occupancyData1.OverflowIndex;
      int index = -1;
      TerrainOccupancyManager.OccupancyData occupancyData2;
      for (; overflowIndex >= 0; overflowIndex = occupancyData2.OverflowIndex)
      {
        occupancyData2 = this.m_overflowData[overflowIndex];
        if (occupancyData2.EntityId != idToRemove)
        {
          index = overflowIndex;
        }
        else
        {
          if (index < 0)
            data[tileIndex.Value] = occupancyData1.WithNewOverflowIndex(occupancyData2.OverflowIndex);
          else
            this.m_overflowData[index] = this.m_overflowData[index].WithNewOverflowIndex(occupancyData2.OverflowIndex);
          this.m_freeOverflowIndices.Add(overflowIndex);
          return true;
        }
      }
      return false;
    }

    private TerrainOccupancyManager.OccupancyData removeLowestFromOverflow(int initialOverflowIndex)
    {
      int index1 = initialOverflowIndex;
      int num = -1;
      int index2 = -1;
      int index3 = -1;
      HeightTilesI heightTilesI = HeightTilesI.MaxValue;
      do
      {
        TerrainOccupancyManager.OccupancyData occupancyData = this.m_overflowData[index1];
        if (occupancyData.From < heightTilesI)
        {
          heightTilesI = occupancyData.From;
          index3 = index1;
          index2 = num;
        }
        num = index1;
        index1 = occupancyData.OverflowIndex;
      }
      while (index1 >= 0);
      TerrainOccupancyManager.OccupancyData occupancyData1 = this.m_overflowData[index3];
      this.m_freeOverflowIndices.Add(index3);
      if (index2 < 0)
        return occupancyData1;
      this.m_overflowData[index2] = this.m_overflowData[index2].WithNewOverflowIndex(occupancyData1.OverflowIndex);
      return occupancyData1.WithNewOverflowIndex(initialOverflowIndex);
    }

    static TerrainOccupancyManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TerrainOccupancyManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainOccupancyManager) obj).SerializeData(writer));
      TerrainOccupancyManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainOccupancyManager) obj).DeserializeData(reader));
    }

    [ExpectedStructSize(12)]
    private readonly struct OccupancyData
    {
      public readonly EntityId EntityId;
      public readonly short FromRaw;
      public readonly ushort VerticalSizeRaw;
      /// <summary>This value must be negative if there is no overflow.</summary>
      public readonly int OverflowIndex;

      public HeightTilesI From => new HeightTilesI((int) this.FromRaw);

      public ThicknessTilesI VerticalSize => new ThicknessTilesI((int) this.VerticalSizeRaw);

      public HeightTilesI ToExcl => new HeightTilesI(this.From.Value + (int) this.VerticalSizeRaw);

      public OccupancyData(
        EntityId entityId,
        short fromRaw,
        ushort verticalSizeRaw,
        int overflowIndex)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.EntityId = entityId;
        this.FromRaw = fromRaw;
        this.VerticalSizeRaw = verticalSizeRaw;
        this.OverflowIndex = overflowIndex;
      }

      public OccupancyData(
        EntityId entityId,
        HeightTilesI from,
        ThicknessTilesI verticalSize,
        int overflowIndex)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this = new TerrainOccupancyManager.OccupancyData(entityId, (short) from.Value, (ushort) verticalSize.Value, overflowIndex);
      }

      public TerrainOccupancyManager.OccupancyData WithNewOverflowIndex(int overflowIndex)
      {
        return new TerrainOccupancyManager.OccupancyData(this.EntityId, this.FromRaw, this.VerticalSizeRaw, overflowIndex);
      }

      public override string ToString()
      {
        return string.Format("#{0} {1}+{2} (o {3})", (object) this.EntityId, (object) this.FromRaw, (object) this.VerticalSizeRaw, (object) this.OverflowIndex);
      }
    }
  }
}
