using Mafi;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Terrain;
using Mafi.Serialization;

using System;

namespace TerrainTower.TTower
{
    /// <summary>
    /// Manager is used to co-ordinate between all Terrain Towers with a common set of management
    /// Covers Tower Added/Removed, Area Change, and Tower List
    /// </summary>
    [GlobalDependency(RegistrationMode.AsEverything, false, false)]
    public class TerrainTowerManager
    {
        private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
        private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
        private readonly EntitiesManager m_entitiesManager;
        private readonly Event<TerrainTowerEntity, PolygonTerrainArea2i> m_onAreaChange;
        private readonly Event<TerrainTowerEntity, EntityAddReason> m_onTowerAdded;
        private readonly Event<TerrainTowerEntity, EntityRemoveReason> m_onTowerRemoved;
        private readonly Lyst<TerrainTowerEntity> m_terrainTowers;

        private void entityAdded(IEntity entity, EntityAddReason addReason)
        {
            if (entity is TerrainTowerEntity tower)
            {
                m_terrainTowers.Add(tower);
                m_onTowerAdded.Invoke(tower, addReason);
            }
        }

        private void entityRemoved(IEntity entity, EntityRemoveReason removeReason)
        {
            if (entity is TerrainTowerEntity tower)
            {
                Assert.AssertTrue(m_terrainTowers.TryRemoveReplaceLast(tower));
                m_onTowerRemoved.Invoke(tower, removeReason);
            }
        }

        private void onEntityConstructed(IStaticEntity entity)
        {
            if (entity is TerrainTowerEntity tower)
            {
                m_terrainTowers.Add(tower);
            }
        }

        private void onEntityDesconstructionStarted(IStaticEntity entity)
        {
            if (entity is TerrainTowerEntity tower)
            {
                Assert.AssertTrue(m_terrainTowers.TryRemoveReplaceLast(tower));
            }
        }

        internal void InvokeOnAreaChanged(TerrainTowerEntity tower, PolygonTerrainArea2i oldArea) => m_onAreaChange.Invoke(tower, oldArea);

        public TerrainTowerManager(
            EntitiesManager entitiesManager,
            IConstructionManager constructionManager) : base()
        {
            m_onTowerAdded = new Event<TerrainTowerEntity, EntityAddReason>();
            m_onTowerRemoved = new Event<TerrainTowerEntity, EntityRemoveReason>();
            m_onAreaChange = new Event<TerrainTowerEntity, PolygonTerrainArea2i>();
            m_terrainTowers = new Lyst<TerrainTowerEntity>();
            m_entitiesManager = entitiesManager;
            entitiesManager.EntityAddedFull.Add(this, entityAdded);
            entitiesManager.EntityRemovedFull.Add(this, entityRemoved);
            constructionManager.EntityConstructed.Add(this, new Action<IStaticEntity>(onEntityConstructed));
            constructionManager.EntityStartedDeconstruction.Add(this, new Action<IStaticEntity>(onEntityDesconstructionStarted));
        }

        public IIndexable<TerrainTowerEntity> AllTowers => m_terrainTowers;
        public IEvent<TerrainTowerEntity, PolygonTerrainArea2i> OnAreaChange => m_onAreaChange;
        public IEvent<TerrainTowerEntity, EntityAddReason> OnTowerAdded => m_onTowerAdded;
        public IEvent<TerrainTowerEntity, EntityRemoveReason> OnTowerRemoved => m_onTowerRemoved;

        #region SERIALIZATION

        protected virtual void DeserializeData(BlobReader reader)
        {
            reader.SetField(this, "m_entitiesManager", EntitiesManager.Deserialize(reader));
            reader.SetField(this, "m_terrainTowers", Lyst<TerrainTowerEntity>.Deserialize(reader));
            reader.SetField(this, "m_onAreaChange", Event<TerrainTowerEntity, PolygonTerrainArea2i>.Deserialize(reader));
            reader.SetField(this, "m_onTowerAdded", Event<TerrainTowerEntity, EntityAddReason>.Deserialize(reader));
            reader.SetField(this, "m_onTowerRemoved", Event<TerrainTowerEntity, EntityRemoveReason>.Deserialize(reader));
        }

        protected virtual void SerializeData(BlobWriter writer)
        {
            EntitiesManager.Serialize(m_entitiesManager, writer);
            Lyst<TerrainTowerEntity>.Serialize(m_terrainTowers, writer);
            Event<TerrainTowerEntity, PolygonTerrainArea2i>.Serialize(m_onAreaChange, writer);
            Event<TerrainTowerEntity, EntityAddReason>.Serialize(m_onTowerAdded, writer);
            Event<TerrainTowerEntity, EntityRemoveReason>.Serialize(m_onTowerRemoved, writer);
        }

        static TerrainTowerManager()
        {
            s_serializeDataDelayedAction = (obj, writer) => ((TerrainTowerManager)obj).SerializeData(writer);
            s_deserializeDataDelayedAction = (obj, reader) => ((TerrainTowerManager)obj).DeserializeData(reader);
        }

        public static TerrainTowerManager Deserialize(BlobReader reader)
        {
            if (reader.TryStartClassDeserialization(out TerrainTowerManager terrainTowerManager)) reader.EnqueueDataDeserialization(terrainTowerManager, s_deserializeDataDelayedAction);
            return terrainTowerManager;
        }

        public static void Serialize(TerrainTowerManager value, BlobWriter writer)
        {
            if (writer.TryStartClassSerialization(value)) writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
        }

        #endregion SERIALIZATION
    }
}