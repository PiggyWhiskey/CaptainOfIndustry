// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.StaticEntitiesTerrainInteractionManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Notifications;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  /// <summary>
  /// Handles interaction between terrain and static entities such validation and collapse detection.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class StaticEntitiesTerrainInteractionManager : 
    IEntityAdditionValidator<IEntityWithOccupiedTilesAddRequest>,
    IEntityAdditionValidator
  {
    public static readonly Duration UPDATE_FREQ_TICKS;
    private static readonly ThicknessTilesF TOLERANCE;
    private readonly TerrainManager m_terrainManager;
    private readonly OccupiedTerrainVertexManger m_occupiedGroundVertexManger;
    private readonly INotificationsManager m_notifManager;
    private readonly EntityCollapseHelper m_collapseHelper;
    [NewInSaveVersion(140, null, null, typeof (TerrainOccupancyManager), null)]
    private readonly TerrainOccupancyManager m_terrainOccupancyManager;
    private readonly Dict<IStaticEntity, StaticEntitiesTerrainInteractionManager.EntityDataMutable> m_affectedEntities;
    private readonly Set<IStaticEntity> m_checkForTerrainIssues;
    private int m_updateCounter;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<OccupiedTerrainVertexManger.TileVertexData> m_entitiesVertexDataTmp;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<IStaticEntity> m_entitiesTmp;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public StaticEntitiesTerrainInteractionManager(
      ISimLoopEvents simLoopEvents,
      EntitiesManager entitiesManager,
      TerrainManager terrainManager,
      ConstructionManager constructionManager,
      OccupiedTerrainVertexManger occupiedGroundVertexManger,
      INotificationsManager notifManager,
      EntityCollapseHelper collapseHelper,
      TerrainOccupancyManager terrainOccupancyManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_affectedEntities = new Dict<IStaticEntity, StaticEntitiesTerrainInteractionManager.EntityDataMutable>();
      this.m_checkForTerrainIssues = new Set<IStaticEntity>();
      this.m_updateCounter = StaticEntitiesTerrainInteractionManager.UPDATE_FREQ_TICKS.Ticks;
      this.m_entitiesVertexDataTmp = new Lyst<OccupiedTerrainVertexManger.TileVertexData>();
      this.m_entitiesTmp = new Lyst<IStaticEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_occupiedGroundVertexManger = occupiedGroundVertexManger;
      this.m_notifManager = notifManager;
      this.m_collapseHelper = collapseHelper;
      this.m_terrainOccupancyManager = terrainOccupancyManager;
      simLoopEvents.Update.Add<StaticEntitiesTerrainInteractionManager>(this, new Action(this.simUpdate));
      terrainManager.HeightChanged.Add<StaticEntitiesTerrainInteractionManager>(this, new Action<Tile2iAndIndex>(this.heightChanged));
      constructionManager.EntityConstructed.Add<StaticEntitiesTerrainInteractionManager>(this, new Action<IStaticEntity>(this.staticEntityConstructed));
      entitiesManager.StaticEntityRemoved.Add<StaticEntitiesTerrainInteractionManager>(this, new Action<IStaticEntity>(this.staticEntityRemoved));
      entitiesManager.OnUpgradeToBePerformed.Add<StaticEntitiesTerrainInteractionManager>(this, new Action<IUpgradableEntity>(this.entityToBeUpgraded));
      entitiesManager.OnUpgradeJustPerformed.Add<StaticEntitiesTerrainInteractionManager>(this, new Action<IUpgradableEntity, IEntityProto>(this.entityUpgraded));
    }

    private void simUpdate()
    {
      if (this.m_checkForTerrainIssues.IsEmpty)
        return;
      --this.m_updateCounter;
      if (this.m_updateCounter > 0)
        return;
      this.m_updateCounter = StaticEntitiesTerrainInteractionManager.UPDATE_FREQ_TICKS.Ticks;
      this.m_entitiesTmp.Clear();
      this.m_entitiesTmp.AddRange((IEnumerable<IStaticEntity>) this.m_checkForTerrainIssues);
      this.m_checkForTerrainIssues.Clear();
      for (int index = 0; index < this.m_entitiesTmp.Count; ++index)
      {
        IStaticEntity key = this.m_entitiesTmp[index];
        StaticEntitiesTerrainInteractionManager.EntityDataMutable entityDataMutable;
        if (!this.m_affectedEntities.TryGetValue(key, out entityDataMutable))
          Log.Error("Entity record of terrain issues not found.");
        Assert.That<Set<int>>(entityDataMutable.VerticesViolatingGroundConstraints).IsNotEmpty<int>();
        key.TryCollapseOnUnevenTerrain((IReadOnlySet<int>) entityDataMutable.VerticesViolatingGroundConstraints, this.m_collapseHelper);
      }
    }

    private void heightChanged(Tile2iAndIndex tileAndIndex)
    {
      this.m_entitiesVertexDataTmp.Clear();
      this.m_occupiedGroundVertexManger.GetEntitiesOnTerrainVertex(tileAndIndex.TileCoord, this.m_entitiesVertexDataTmp);
      if (this.m_entitiesVertexDataTmp.IsEmpty)
        return;
      foreach (OccupiedTerrainVertexManger.TileVertexData tileVertexData in this.m_entitiesVertexDataTmp)
        this.processTileHeightChange(tileVertexData.Entity, tileVertexData.VertexIndex, tileAndIndex.Index);
    }

    private void entityUpgraded(IEntity entity, IEntityProto previousProto)
    {
      if (!(entity is IStaticEntity entity1))
        return;
      this.staticEntityConstructed(entity1);
    }

    private void staticEntityConstructed(IStaticEntity entity)
    {
      for (int index = 0; index < entity.OccupiedVertices.Length; ++index)
      {
        OccupiedVertexRelative occupiedVertex = entity.OccupiedVertices[index];
        if (occupiedVertex.MinTerrainHeightOrMinValueRaw > short.MinValue || occupiedVertex.MaxTerrainHeightOrMaxValueRaw < short.MaxValue)
          this.processTileHeightChange(entity, index, this.m_terrainManager.GetTileIndex(entity.CenterTile.Xy + occupiedVertex.RelCoord));
      }
    }

    private void entityToBeUpgraded(IEntity entity)
    {
      if (!(entity is IStaticEntity entity1))
        return;
      this.staticEntityRemoved(entity1);
    }

    private void staticEntityRemoved(IStaticEntity entity)
    {
      StaticEntitiesTerrainInteractionManager.EntityDataMutable entityDataMutable;
      if (!this.m_affectedEntities.TryRemove(entity, out entityDataMutable))
        return;
      entityDataMutable.ViolationNotification.Deactivate((IEntity) entity);
      this.m_checkForTerrainIssues.Remove(entity);
    }

    private void processTileHeightChange(
      IStaticEntity entity,
      int vertexIndex,
      Tile2iIndex tileIndex)
    {
      HeightTilesI height1 = entity.CenterTile.Height;
      HeightTilesF height2 = this.m_terrainManager.GetHeight(tileIndex);
      OccupiedVertexRelative occupiedVertex = entity.OccupiedVertices[vertexIndex];
      int num = 0;
      if (occupiedVertex.Constraint.HasAnyConstraints(LayoutTileConstraint.UsingPillar))
      {
        OccupiedTileRelative occupiedTile = entity.OccupiedTiles[(int) occupiedVertex.LowestTileIndex];
        if (!occupiedTile.Constraint.HasAnyConstraints(LayoutTileConstraint.UsingPillar))
        {
          num = height1.Value - height2.HeightI.Value;
        }
        else
        {
          TransportPillar entity1;
          if (this.m_terrainOccupancyManager.TryGetOccupyingEntityInRange<TransportPillar>(entity.CenterTile + occupiedTile.RelCoord.ExtendZ(-1), ThicknessTilesI.One, out entity1))
          {
            if (entity1 == entity)
              Log.Error("Pillar supported by a pillar???");
            else
              this.staticEntityConstructed((IStaticEntity) entity1);
            if (!this.m_affectedEntities.TryGetValue((IStaticEntity) entity1, out StaticEntitiesTerrainInteractionManager.EntityDataMutable _))
              num = entity1.Height.Value;
          }
        }
      }
      if (height2 + StaticEntitiesTerrainInteractionManager.TOLERANCE < height1 + new ThicknessTilesI((int) occupiedVertex.MinTerrainHeightOrMinValueRaw - num) || height2 - StaticEntitiesTerrainInteractionManager.TOLERANCE > height1 + new ThicknessTilesI((int) occupiedVertex.MaxTerrainHeightOrMaxValueRaw))
      {
        bool exists;
        ref StaticEntitiesTerrainInteractionManager.EntityDataMutable local = ref this.m_affectedEntities.GetRefValue(entity, out exists);
        if (!exists)
        {
          EntityNotificator notificatorFor = this.m_notifManager.CreateNotificatorFor(IdsCore.Notifications.EntityMayCollapseUnevenTerrain);
          notificatorFor.Activate((IEntity) entity);
          local = new StaticEntitiesTerrainInteractionManager.EntityDataMutable(notificatorFor);
        }
        if (!local.VerticesViolatingGroundConstraints.Add(vertexIndex))
          return;
        if (exists && local.ViolationNotification.IsActive)
          this.m_notifManager.UnsuppressNotification(local.ViolationNotification.NotificationId);
        bool canCollapse;
        entity.NotifyUnevenTerrain((IReadOnlySet<int>) local.VerticesViolatingGroundConstraints, vertexIndex, true, out canCollapse);
        if (!canCollapse)
          return;
        this.m_checkForTerrainIssues.Add(entity);
      }
      else
      {
        StaticEntitiesTerrainInteractionManager.EntityDataMutable entityDataMutable;
        if (!this.m_affectedEntities.TryGetValue(entity, out entityDataMutable) || !entityDataMutable.VerticesViolatingGroundConstraints.Remove(vertexIndex))
          return;
        if (entityDataMutable.VerticesViolatingGroundConstraints.IsEmpty)
        {
          entityDataMutable.ViolationNotification.Deactivate((IEntity) entity);
          this.m_checkForTerrainIssues.Remove(entity);
          this.m_affectedEntities.RemoveAndAssert(entity);
        }
        bool canCollapse;
        entity.NotifyUnevenTerrain((IReadOnlySet<int>) entityDataMutable.VerticesViolatingGroundConstraints, vertexIndex, false, out canCollapse);
        if (!canCollapse)
          return;
        this.m_checkForTerrainIssues.Add(entity);
      }
    }

    public EntityValidationResult CanAdd(IEntityWithOccupiedTilesAddRequest addRequest)
    {
      Tile3i origin = addRequest.Origin;
      EntityValidationResult validationResult = EntityValidationResult.Success;
      foreach (OccupiedVertexRelative occupiedVertex in addRequest.OccupiedVertices)
      {
        if ((occupiedVertex.MinTerrainHeightOrMinValueRaw != short.MinValue || occupiedVertex.MaxTerrainHeightOrMaxValueRaw != short.MaxValue) && !occupiedVertex.Constraint.HasAnyConstraints(LayoutTileConstraint.UsingPillar))
        {
          HeightTilesF height = this.m_terrainManager[origin.Xy + occupiedVertex.RelCoord].Height;
          if (height + StaticEntitiesTerrainInteractionManager.TOLERANCE < origin.Height + new ThicknessTilesI((int) occupiedVertex.MinTerrainHeightOrMinValueRaw))
          {
            validationResult = EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__TerrainTooLow);
            if (!addRequest.RecordTileErrorsAndMetadata)
              return validationResult;
            addRequest.SetTileError((int) occupiedVertex.LowestTileIndex);
          }
          if (height - StaticEntitiesTerrainInteractionManager.TOLERANCE > origin.Height + new ThicknessTilesI((int) occupiedVertex.MaxTerrainHeightOrMaxValueRaw))
          {
            validationResult = EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__TerrainTooHigh);
            if (!addRequest.RecordTileErrorsAndMetadata)
              return validationResult;
            addRequest.SetTileError((int) occupiedVertex.LowestTileIndex);
          }
        }
      }
      return validationResult;
    }

    public static void Serialize(StaticEntitiesTerrainInteractionManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StaticEntitiesTerrainInteractionManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StaticEntitiesTerrainInteractionManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Dict<IStaticEntity, StaticEntitiesTerrainInteractionManager.EntityDataMutable>.Serialize(this.m_affectedEntities, writer);
      Set<IStaticEntity>.Serialize(this.m_checkForTerrainIssues, writer);
      EntityCollapseHelper.Serialize(this.m_collapseHelper, writer);
      writer.WriteGeneric<INotificationsManager>(this.m_notifManager);
      OccupiedTerrainVertexManger.Serialize(this.m_occupiedGroundVertexManger, writer);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      TerrainOccupancyManager.Serialize(this.m_terrainOccupancyManager, writer);
      writer.WriteInt(this.m_updateCounter);
    }

    public static StaticEntitiesTerrainInteractionManager Deserialize(BlobReader reader)
    {
      StaticEntitiesTerrainInteractionManager interactionManager;
      if (reader.TryStartClassDeserialization<StaticEntitiesTerrainInteractionManager>(out interactionManager))
        reader.EnqueueDataDeserialization((object) interactionManager, StaticEntitiesTerrainInteractionManager.s_deserializeDataDelayedAction);
      return interactionManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_affectedEntities", (object) Dict<IStaticEntity, StaticEntitiesTerrainInteractionManager.EntityDataMutable>.Deserialize(reader));
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_checkForTerrainIssues", (object) Set<IStaticEntity>.Deserialize(reader));
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_collapseHelper", (object) EntityCollapseHelper.Deserialize(reader));
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_entitiesTmp", (object) new Lyst<IStaticEntity>());
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_entitiesVertexDataTmp", (object) new Lyst<OccupiedTerrainVertexManger.TileVertexData>());
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_notifManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_occupiedGroundVertexManger", (object) OccupiedTerrainVertexManger.Deserialize(reader));
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      reader.SetField<StaticEntitiesTerrainInteractionManager>(this, "m_terrainOccupancyManager", reader.LoadedSaveVersion >= 140 ? (object) TerrainOccupancyManager.Deserialize(reader) : (object) (TerrainOccupancyManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<StaticEntitiesTerrainInteractionManager>(this, "m_terrainOccupancyManager", typeof (TerrainOccupancyManager), true);
      this.m_updateCounter = reader.ReadInt();
    }

    static StaticEntitiesTerrainInteractionManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StaticEntitiesTerrainInteractionManager.UPDATE_FREQ_TICKS = 12.Seconds();
      StaticEntitiesTerrainInteractionManager.TOLERANCE = 0.5.TilesThick();
      StaticEntitiesTerrainInteractionManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((StaticEntitiesTerrainInteractionManager) obj).SerializeData(writer));
      StaticEntitiesTerrainInteractionManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((StaticEntitiesTerrainInteractionManager) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private readonly struct EntityDataMutable
    {
      public readonly Set<int> VerticesViolatingGroundConstraints;
      public readonly EntityNotificator ViolationNotification;

      [LoadCtor]
      private EntityDataMutable(
        Set<int> verticesViolatingGroundConstraints,
        EntityNotificator violationNotification)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.VerticesViolatingGroundConstraints = verticesViolatingGroundConstraints;
        this.ViolationNotification = violationNotification;
      }

      public EntityDataMutable(EntityNotificator violationNotification)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.VerticesViolatingGroundConstraints = new Set<int>();
        this.ViolationNotification = violationNotification;
      }

      public static void Serialize(
        StaticEntitiesTerrainInteractionManager.EntityDataMutable value,
        BlobWriter writer)
      {
        Set<int>.Serialize(value.VerticesViolatingGroundConstraints, writer);
        EntityNotificator.Serialize(value.ViolationNotification, writer);
      }

      public static StaticEntitiesTerrainInteractionManager.EntityDataMutable Deserialize(
        BlobReader reader)
      {
        return new StaticEntitiesTerrainInteractionManager.EntityDataMutable(Set<int>.Deserialize(reader), EntityNotificator.Deserialize(reader));
      }
    }
  }
}
