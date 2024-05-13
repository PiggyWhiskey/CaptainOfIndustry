using Mafi;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Serialization;

using System;

using static TerrainTower.TTower.TerrainTowerCommands;

namespace TerrainTower.TTower
{
    /// <summary>
    /// Tower Commands are used to store or initiate ASync changes and interactions between User and Entity/Prototype
    /// </summary>
    public class TerrainTowerCommands
    {
        /// <summary>
        /// Class used to set Product Port for Terrain Tower from User Input via UI
        /// </summary>
        public class SetProductPortCmd : InputCommand
        {
            public readonly int PortIndex;
            public readonly ProductProto.ID ProductId;
            public readonly EntityId TerrainTowerId;
            private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
            private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;

            static SetProductPortCmd()
            {
                s_serializeDataDelayedAction = (obj, writer) => ((SetProductPortCmd)obj).SerializeData(writer);
                s_deserializeDataDelayedAction = (obj, reader) => ((SetProductPortCmd)obj).DeserializeData(reader);
            }

            public SetProductPortCmd(TerrainTowerEntity sortingPlant, ProductProto productProto, int portIndex) : this(sortingPlant.Id, productProto.Id, portIndex)
            { }

            private SetProductPortCmd(EntityId sortingPlantId, ProductProto.ID productId, int portIndex) : base()
            {
                TerrainTowerId = sortingPlantId;
                ProductId = productId;
                PortIndex = portIndex;
            }

            public new static SetProductPortCmd Deserialize(BlobReader reader)
            {
                if (reader.TryStartClassDeserialization(out SetProductPortCmd setProductPortCmd))
                    reader.EnqueueDataDeserialization(setProductPortCmd, s_deserializeDataDelayedAction);
                return setProductPortCmd;
            }

            public static void Serialize(SetProductPortCmd value, BlobWriter writer)
            {
                if (!writer.TryStartClassSerialization(value))
                    return;
                writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, "TerrainTowerId", EntityId.Deserialize(reader));
                reader.SetField(this, "ProductId", ProductProto.ID.Deserialize(reader));
                reader.SetField(this, "PortIndex", reader.ReadInt());
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                EntityId.Serialize(TerrainTowerId, writer);
                ProductProto.ID.Serialize(ProductId, writer);
                writer.WriteInt(PortIndex);
            }
        }

        /// <summary>
        /// Class used to set Blocked Product alert for Terrain Tower from User Input via UI
        /// </summary>
        public class SortingPlantSetBlockedProductAlertCmd : InputCommand
        {
            public readonly bool IsAlertEnabled;
            public readonly ProductProto.ID ProductId;
            public readonly EntityId TerrainTowerId;
            private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
            private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;

            static SortingPlantSetBlockedProductAlertCmd()
            {
                s_serializeDataDelayedAction = (obj, writer) => ((SortingPlantSetBlockedProductAlertCmd)obj).SerializeData(writer);
                s_deserializeDataDelayedAction = (obj, reader) => ((SortingPlantSetBlockedProductAlertCmd)obj).DeserializeData(reader);
            }

            public SortingPlantSetBlockedProductAlertCmd(
                            EntityId terrainTowerId,
                ProductProto.ID productId,
                bool isAlertEnabled
                ) : base()
            {
                TerrainTowerId = terrainTowerId;
                ProductId = productId;
                IsAlertEnabled = isAlertEnabled;
            }

            public new static SortingPlantSetBlockedProductAlertCmd Deserialize(BlobReader reader)
            {
                if (reader.TryStartClassDeserialization(out SortingPlantSetBlockedProductAlertCmd blockedProductAlertCmd))
                    reader.EnqueueDataDeserialization(blockedProductAlertCmd, s_deserializeDataDelayedAction);
                return blockedProductAlertCmd;
            }

            public static void Serialize(SortingPlantSetBlockedProductAlertCmd value, BlobWriter writer)
            {
                if (!writer.TryStartClassSerialization(value))
                    return;
                writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, "TerrainTowerId", EntityId.Deserialize(reader));
                reader.SetField(this, "ProductId", ProductProto.ID.Deserialize(reader));
                reader.SetField(this, "IsAlertEnabled", reader.ReadBool());
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                EntityId.Serialize(TerrainTowerId, writer);
                ProductProto.ID.Serialize(ProductId, writer);
                writer.WriteBool(IsAlertEnabled);
            }
        }

        /// <summary>
        /// Class used to toggle Terrain Tower Config from User Input via UI
        /// </summary>
        public class TerrainTowerConfigToggleCmd : InputCommand
        {
            public readonly TerrainTowerEntity.TerrainTowerConfigState ConfigState;
            public readonly EntityId TerrainTowerId;
            private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
            private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;

            static TerrainTowerConfigToggleCmd()
            {
                s_serializeDataDelayedAction = (obj, writer) => ((TerrainTowerConfigToggleCmd)obj).SerializeData(writer);
                s_deserializeDataDelayedAction = (obj, reader) => ((TerrainTowerConfigToggleCmd)obj).DeserializeData(reader);
            }

            public TerrainTowerConfigToggleCmd(TerrainTowerEntity terrainTower, TerrainTowerEntity.TerrainTowerConfigState configState) : this(terrainTower.Id, configState)
            {
            }

            public TerrainTowerConfigToggleCmd(EntityId terrainTowerId, TerrainTowerEntity.TerrainTowerConfigState configState) : base()
            {
                TerrainTowerId = terrainTowerId;
                ConfigState = configState;
            }

            public new static TerrainTowerConfigToggleCmd Deserialize(BlobReader reader)
            {
                if (reader.TryStartClassDeserialization(out TerrainTowerConfigToggleCmd entityTerrainToggleCommand))
                    reader.EnqueueDataDeserialization(entityTerrainToggleCommand, s_deserializeDataDelayedAction);
                return entityTerrainToggleCommand;
            }

            public static void Serialize(TerrainTowerConfigToggleCmd value, BlobWriter writer)
            {
                if (!writer.TryStartClassSerialization(value))
                    return;
                writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, nameof(TerrainTowerId), EntityId.Deserialize(reader));
                reader.SetField(this, nameof(ConfigState), (TerrainTowerEntity.TerrainTowerConfigState)reader.ReadInt());
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                EntityId.Serialize(TerrainTowerId, writer);
                writer.WriteInt((int)ConfigState);
            }
        }
    }

    /// <summary>
    /// Manager is used to co-ordinate between all Terrain Towers with a common set of management in the form of Commands
    /// </summary>
    [GlobalDependency(RegistrationMode.AsEverything, false, false)]
    internal class TerrainTowersManager :
        ICommandProcessor<TerrainTowerConfigToggleCmd>,
        IAction<TerrainTowerConfigToggleCmd>,
        ICommandProcessor<SetProductPortCmd>,
        IAction<SetProductPortCmd>,
        ICommandProcessor<SortingPlantSetBlockedProductAlertCmd>,
        IAction<SortingPlantSetBlockedProductAlertCmd>
    {
        private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
        private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
        private readonly IEntitiesManager m_entitiesManager;
        private readonly ProtosDb m_protosDb;
        private readonly Lyst<TerrainTowerEntity> m_terrainTowers;

        static TerrainTowersManager()
        {
            s_serializeDataDelayedAction = (obj, writer) => ((TerrainTowersManager)obj).SerializeData(writer);
            s_deserializeDataDelayedAction = (obj, reader) => ((TerrainTowersManager)obj).DeserializeData(reader);
        }

        public TerrainTowersManager(
            ProtosDb protosDb,
            IEntitiesManager entitiesManager,
            IConstructionManager constructionManager) : base()
        {
            m_protosDb = protosDb;
            m_entitiesManager = entitiesManager;
            m_terrainTowers = new Lyst<TerrainTowerEntity>();
            m_onAreaChange = new Event<TerrainTowerEntity, RectangleTerrainArea2i>();
            m_onTTAdded = new Event<TerrainTowerEntity, EntityAddReason>();
            m_onTTRemoved = new Event<TerrainTowerEntity, EntityRemoveReason>();
            entitiesManager.EntityAddedFull.Add(this, entityAdded);
            entitiesManager.EntityRemovedFull.Add(this, entityRemoved);
            constructionManager.EntityConstructed.Add(this, new Action<IStaticEntity>(onEntityConstructed));
            constructionManager.EntityStartedDeconstruction.Add(this, new Action<IStaticEntity>(onEntityDesconstructionStarted));
        }

        public IIndexable<TerrainTowerEntity> AllTowers => m_terrainTowers;

        #region EVENTS - Harmony Patch - TowerAreasRenderer

        private readonly Event<TerrainTowerEntity, RectangleTerrainArea2i> m_onAreaChange;
        private readonly Event<TerrainTowerEntity, EntityAddReason> m_onTTAdded;
        private readonly Event<TerrainTowerEntity, EntityRemoveReason> m_onTTRemoved;
        public IEvent<TerrainTowerEntity, RectangleTerrainArea2i> OnAreaChange => m_onAreaChange;
        public IEvent<TerrainTowerEntity, EntityAddReason> OnTTAdded => m_onTTAdded;
        public IEvent<TerrainTowerEntity, EntityRemoveReason> OnTTRemoved => m_onTTRemoved;

        #endregion EVENTS - Harmony Patch - TowerAreasRenderer

        public static TerrainTowersManager Deserialize(BlobReader reader)
        {
            if (reader.TryStartClassDeserialization(out TerrainTowersManager towerManager))
                reader.EnqueueDataDeserialization(towerManager, s_deserializeDataDelayedAction);
            return towerManager;
        }

        public static void Serialize(TerrainTowersManager value, BlobWriter writer)
        {
            if (!writer.TryStartClassSerialization(value))
                return;
            writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
        }

        public void Invoke(TerrainTowerConfigToggleCmd cmd)
        {
            if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity terrainTower))
            {
                cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", cmd.TerrainTowerId));
            }
            else
            {
                if (terrainTower.TerrainConfigState != cmd.ConfigState)
                {
                    //Only update state if it's changed
                    terrainTower.SetConfigState(cmd.ConfigState);
                }
                cmd.SetResultSuccess();
            }
        }

        public void Invoke(SetProductPortCmd cmd)
        {
            if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity terrainTower))
            {
                cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", cmd.TerrainTowerId));
            }
            else
            {
                if (!m_protosDb.TryGetProto((Proto.ID)cmd.ProductId, out ProductProto proto))
                {
                    cmd.SetResultError(string.Format("Product with id {0} not found.", cmd.ProductId));
                }
                else
                {
                    terrainTower.SetProductPortIndex(proto, cmd.PortIndex);
                    cmd.SetResultSuccess();
                }
            }
        }

        public void Invoke(SortingPlantSetBlockedProductAlertCmd cmd)
        {
            if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity entity))
            {
                cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", cmd.TerrainTowerId));
            }
            else
            {
                if (!m_protosDb.TryGetProto((Proto.ID)cmd.ProductId, out ProductProto proto))
                {
                    cmd.SetResultError(string.Format("Product with id {0} not found.", cmd.ProductId));
                }
                else
                {
                    entity.SetProductBlockedAlert(proto, cmd.IsAlertEnabled);
                    cmd.SetResultSuccess();
                }
            }
        }

        internal void InvokeOnAreaChanged(TerrainTowerEntity tower, RectangleTerrainArea2i area)
        {
            m_onAreaChange.Invoke(tower, area);
        }

        protected virtual void DeserializeData(BlobReader reader)
        {
            reader.SetField(this, nameof(m_entitiesManager), reader.ReadGenericAs<IEntitiesManager>());
            reader.RegisterResolvedMember(this, nameof(m_protosDb), typeof(ProtosDb), true);
            reader.SetField(this, nameof(m_terrainTowers), Lyst<TerrainTowerEntity>.Deserialize(reader));
            reader.SetField(this, nameof(m_onAreaChange), Event<TerrainTowerEntity, RectangleTerrainArea2i>.Deserialize(reader));
            reader.SetField(this, nameof(m_onTTAdded), Event<TerrainTowerEntity, EntityAddReason>.Deserialize(reader));
            reader.SetField(this, nameof(m_onTTRemoved), Event<TerrainTowerEntity, EntityRemoveReason>.Deserialize(reader));
        }

        protected virtual void SerializeData(BlobWriter writer)
        {
            writer.WriteGeneric(m_entitiesManager);
            Lyst<TerrainTowerEntity>.Serialize(m_terrainTowers, writer);
            Event<TerrainTowerEntity, RectangleTerrainArea2i>.Serialize(m_onAreaChange, writer);
            Event<TerrainTowerEntity, EntityAddReason>.Serialize(m_onTTAdded, writer);
            Event<TerrainTowerEntity, EntityRemoveReason>.Serialize(m_onTTRemoved, writer);
        }

        private void entityAdded(IEntity entity, EntityAddReason addReason)
        {
            if (entity is TerrainTowerEntity tower)
            {
                m_terrainTowers.Add(tower);
                m_onTTAdded.Invoke(tower, addReason);
            }
        }

        private void entityRemoved(IEntity entity, EntityRemoveReason removeReason)
        {
            if (entity is TerrainTowerEntity tower)
            {
                Assert.AssertTrue(m_terrainTowers.TryRemoveReplaceLast(tower));
                m_onTTRemoved.Invoke(tower, removeReason);
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
                m_terrainTowers.TryRemoveReplaceLast(tower);
            }
        }
    }
}