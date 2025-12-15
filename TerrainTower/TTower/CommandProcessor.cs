using Mafi;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Serialization;

using static TerrainTower.TTower.TerrainTowerCommands;

namespace TerrainTower.TTower
{
    //ICommandProcessor<TerrainTowerConfigToggleCmd>,
    //IAction<TerrainTowerConfigToggleCmd>,
    internal class TerrainTowerCommandProcessor :
        ICommandProcessor<TerrainTowerAreaChangeCmd>,
        IAction<TerrainTowerAreaChangeCmd>,
        ICommandProcessor<AddProductToDumpCmd>,
        IAction<AddProductToDumpCmd>,
        ICommandProcessor<RemoveProductToDumpCmd>,
        IAction<RemoveProductToDumpCmd>,
        ICommandProcessor<AddProductToSortCmd>,
        IAction<AddProductToSortCmd>,
        ICommandProcessor<RemoveProductToSortCmd>,
        IAction<RemoveProductToSortCmd>,
        ICommandProcessor<SetProductPortCmd>,
        IAction<SetProductPortCmd>,
        ICommandProcessor<SetBlockedProductAlertCmd>,
        IAction<SetBlockedProductAlertCmd>
    {
        #region Fields

        private readonly ITerrainDumpingManager m_dumpingManager;
        private readonly IEntitiesManager m_entitiesManager;
        private readonly ProtosDb m_protosDb;

        #endregion Fields

        #region Constructors

        public TerrainTowerCommandProcessor(
            ProtosDb protosDb,
            IEntitiesManager entitiesManager,
            ITerrainDumpingManager dumpingManager) : base()
        {
            m_protosDb = protosDb;
            m_entitiesManager = entitiesManager;
            m_dumpingManager = dumpingManager;
        }

        #endregion Constructors

        #region Methods

        public void Invoke(AddProductToDumpCmd cmd)
        {
            if (!m_protosDb.TryGetProto((Proto.ID)cmd.ProductId, out LooseProductProto proto))
            {
                cmd.SetResultError(string.Format("Failed to get product {0}.", cmd.ProductId));
            }
            else if (!cmd.TerrainTowerId.HasValue)
            {
                m_dumpingManager.AddProductToDump(proto);
                cmd.SetResultSuccess();
            }
            else if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId.Value, out TerrainTowerEntity entity))
            {
                cmd.SetResultError(string.Format("Failed to get mine tower with ID {0}.", cmd.TerrainTowerId));
            }
            else
            {
                entity.AddProductToDump(proto);
                cmd.SetResultSuccess();
            }
        }

        public void Invoke(RemoveProductToDumpCmd cmd)
        {
            if (!m_protosDb.TryGetProto(cmd.ProductId, out LooseProductProto proto))
            {
                cmd.SetResultError(string.Format("Failed to get product {0}.", cmd.ProductId));
            }
            else if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity entity))
            {
                cmd.SetResultError(string.Format("Terrain Tower with id {0} not found.", cmd.TerrainTowerId));
            }
            else
            {
                entity.RemoveProductToDump(proto);
                cmd.SetResultSuccess();
            }
        }

        public void Invoke(AddProductToSortCmd cmd)
        {
            if (!m_protosDb.TryGetProto(cmd.ProductId, out LooseProductProto proto))
            {
                cmd.SetResultError(string.Format("Failed to get product {0}.", cmd.ProductId));
            }
            else if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity entity))
            {
                cmd.SetResultError(string.Format("Terrain Tower with id {0} not found.", cmd.TerrainTowerId));
            }
            else
            {
                entity.AddProductToSort(proto);
                cmd.SetResultSuccess();
            }
        }

        public void Invoke(RemoveProductToSortCmd cmd)
        {
            if (!m_protosDb.TryGetProto(cmd.ProductId, out LooseProductProto proto))
            {
                cmd.SetResultError(string.Format("Failed to get product {0}.", cmd.ProductId));
            }
            else if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity entity))
            {
                cmd.SetResultError(string.Format("Terrain Tower with id {0} not found.", cmd.TerrainTowerId));
            }
            else
            {
                entity.RemoveProductToSort(proto);
                cmd.SetResultSuccess();
            }
        }

        public void Invoke(SetProductPortCmd cmd)
        {
            if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity terrainTower))
            {
                cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", cmd.TerrainTowerId));
            }
            else if (!m_protosDb.TryGetProto((Proto.ID)cmd.ProductId, out ProductProto proto))
            {
                cmd.SetResultError(string.Format("Product with id {0} not found.", cmd.ProductId));
            }
            else
            {
                if (cmd.ClearPortInstead)
                {
                    terrainTower.RemoveProductPortIndex(proto, cmd.PortIndex);
                }
                else
                {
                    terrainTower.AddProductPortIndex(proto, cmd.PortIndex);
                }
                cmd.SetResultSuccess();
            }
        }

        public void Invoke(SetBlockedProductAlertCmd cmd)
        {
            if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity entity))
            {
                cmd.SetResultError(string.Format("Sorting plant with id {0} not found.", cmd.TerrainTowerId));
            }
            else if (!m_protosDb.TryGetProto((Proto.ID)cmd.ProductId, out ProductProto proto))
            {
                cmd.SetResultError(string.Format("Product with id {0} not found.", cmd.ProductId));
            }
            else
            {
                entity.SetProductBlockedAlert(proto, cmd.IsAlertEnabled);
                cmd.SetResultSuccess();
            }
        }

        public void Invoke(TerrainTowerAreaChangeCmd cmd)
        {
            if (!m_entitiesManager.TryGetEntity(cmd.TerrainTowerId, out TerrainTowerEntity entity))
            {
                cmd.SetResultError(string.Format("Terrain Tower with id {0} not found.", cmd.TerrainTowerId));
            }
            else
            {
                entity.SetNewArea(cmd.Area);
                cmd.SetResultSuccess();
            }
        }

        #endregion Methods
    }

    /// <summary>
    /// Tower Commands are used to store or initiate ASync changes and interactions between User and Entity/Prototype
    /// </summary>
    public class TerrainTowerCommands
    {
        /// <summary>
        /// Class used to add a product to the dump of a Terrain Tower
        /// </summary>
        public class AddProductToDumpCmd : InputCommand
        {
            public readonly ProductProto.ID ProductId;

            public readonly EntityId? TerrainTowerId;

            private AddProductToDumpCmd(EntityId terrainTowerId, ProductProto.ID productId) : base()
            {
                TerrainTowerId = terrainTowerId;
                ProductId = productId;
            }

            public AddProductToDumpCmd(TerrainTowerEntity terrainTower, ProductProto productProto)
                : this(terrainTower.Id, new ProductProto.ID(productProto.Id.Value))
            {
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, nameof(TerrainTowerId), EntityId.Deserialize(reader));
                reader.SetField(this, nameof(ProductId), ProductProto.ID.Deserialize(reader));
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                EntityId.Serialize(TerrainTowerId.Value, writer);
                ProductProto.ID.Serialize(ProductId, writer);
            }
        }

        /// <summary>
        /// Class used to add allowed products to be mined by the Terrain Tower
        /// </summary>
        ///
        public class AddProductToSortCmd : InputCommand
        {
            public readonly ProductProto.ID ProductId;
            public readonly EntityId TerrainTowerId;

            public AddProductToSortCmd(TerrainTowerEntity terrainTower, ProductProto productProto) : this(terrainTower.Id, productProto.Id)
            {
            }

            public AddProductToSortCmd(EntityId terrainTowerId, ProductProto.ID productId) : base()
            {
                TerrainTowerId = terrainTowerId;
                ProductId = productId;
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, "TerrainTowerId", EntityId.Deserialize(reader));
                reader.SetField(this, "ProductId", ProductProto.ID.Deserialize(reader));
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                EntityId.Serialize(TerrainTowerId, writer);
                ProductProto.ID.Serialize(ProductId, writer);
            }
        }

        /// <summary>
        /// Class used to remove a product from the dump of a Terrain Tower
        /// </summary>
        public class RemoveProductToDumpCmd : InputCommand
        {
            public readonly ProductProto.ID ProductId;
            public readonly EntityId TerrainTowerId;

            private RemoveProductToDumpCmd(EntityId terrainTowerId, ProductProto.ID productId) : base()
            {
                TerrainTowerId = terrainTowerId;
                ProductId = productId;
            }

            public RemoveProductToDumpCmd(TerrainTowerEntity terrainTower, ProductProto productProto) : this(terrainTower.Id, productProto.Id)
            {
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, nameof(TerrainTowerId), EntityId.Deserialize(reader));
                reader.SetField(this, nameof(ProductId), ProductProto.ID.Deserialize(reader));
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                EntityId.Serialize(TerrainTowerId, writer);
                ProductProto.ID.Serialize(ProductId, writer);
            }
        }

        /// <summary>
        /// Class used to remove a product from the Mine/Sort of a Terrain Tower
        /// </summary>
        public class RemoveProductToSortCmd : InputCommand
        {
            public readonly ProductProto.ID ProductId;

            public readonly EntityId TerrainTowerId;

            private RemoveProductToSortCmd(EntityId terrainTowerId, ProductProto.ID productId) : base()
            {
                TerrainTowerId = terrainTowerId;
                ProductId = productId;
            }

            public RemoveProductToSortCmd(TerrainTowerEntity terrainTower, ProductProto productProto) : this(terrainTower.Id, productProto.Id)
            {
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, nameof(TerrainTowerId), EntityId.Deserialize(reader));
                reader.SetField(this, nameof(ProductId), ProductProto.ID.Deserialize(reader));
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                EntityId.Serialize(TerrainTowerId, writer);
                ProductProto.ID.Serialize(ProductId, writer);
            }
        }

        /// <summary>
        /// Class used to set Blocked Product alert for Terrain Tower from User Input via UI
        /// </summary>
        public class SetBlockedProductAlertCmd : InputCommand
        {
            public readonly bool IsAlertEnabled;
            public readonly ProductProto.ID ProductId;
            public readonly EntityId TerrainTowerId;

            public SetBlockedProductAlertCmd(EntityId terrainTowerId, ProductProto.ID productId, bool isAlertEnabled) : base()
            {
                TerrainTowerId = terrainTowerId;
                ProductId = productId;
                IsAlertEnabled = isAlertEnabled;
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, nameof(TerrainTowerId), EntityId.Deserialize(reader));
                reader.SetField(this, nameof(ProductId), ProductProto.ID.Deserialize(reader));
                reader.SetField(this, nameof(IsAlertEnabled), reader.ReadBool());
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
        /// Class used to set Product Port for Terrain Tower from User Input via UI
        /// </summary>
        public class SetProductPortCmd : InputCommand
        {
            public readonly bool ClearPortInstead;

            public readonly int PortIndex;

            public readonly ProductProto.ID ProductId;

            public readonly EntityId TerrainTowerId;

            private SetProductPortCmd(EntityId sortingPlantId, ProductProto.ID productId, int portIndex, bool clearPortInstead) : base()
            {
                TerrainTowerId = sortingPlantId;
                ProductId = productId;
                PortIndex = portIndex;
                ClearPortInstead = clearPortInstead;
            }

            public SetProductPortCmd(TerrainTowerEntity sortingPlant, ProductProto productProto, int portIndex, bool clearPortInstead) : this(sortingPlant.Id, productProto.Id, portIndex, clearPortInstead)
            {
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                reader.SetField(this, nameof(TerrainTowerId), EntityId.Deserialize(reader));
                reader.SetField(this, nameof(ProductId), ProductProto.ID.Deserialize(reader));
                reader.SetField(this, nameof(PortIndex), reader.ReadInt());
                reader.SetField(this, nameof(ClearPortInstead), reader.ReadBool());
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                EntityId.Serialize(TerrainTowerId, writer);
                ProductProto.ID.Serialize(ProductId, writer);
                writer.WriteInt(PortIndex);
                writer.WriteBool(ClearPortInstead);
            }
        }

        /// <summary>
        /// Class used to action Area Changes from User Input via UI
        /// </summary>
        public class TerrainTowerAreaChangeCmd : InputCommand
        {
            public readonly EntityId TerrainTowerId;

            public PolygonTerrainArea2i Area { get; private set; }

            public TerrainTowerAreaChangeCmd(EntityId terrainTowerId, PolygonTerrainArea2i area) : base()
            {
                TerrainTowerId = terrainTowerId;
                Area = area;
            }

            protected override void DeserializeData(BlobReader reader)
            {
                base.DeserializeData(reader);
                Area = PolygonTerrainArea2i.Deserialize(reader);
                reader.SetField(this, nameof(TerrainTowerId), EntityId.Deserialize(reader));
            }

            protected override void SerializeData(BlobWriter writer)
            {
                base.SerializeData(writer);
                PolygonTerrainArea2i.Serialize(Area, writer);
                EntityId.Serialize(TerrainTowerId, writer);
            }

#if false
            private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
            private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;

            static TerrainTowerAreaChangeCmd()
            {
                s_serializeDataDelayedAction = (obj, writer) => ((TerrainTowerAreaChangeCmd)obj).SerializeData(writer);
                s_deserializeDataDelayedAction = (obj, reader) => ((TerrainTowerAreaChangeCmd)obj).DeserializeData(reader);
            }
            public new static TerrainTowerAreaChangeCmd Deserialize(BlobReader reader)
            {
                if (reader.TryStartClassDeserialization(out TerrainTowerAreaChangeCmd obj, null)) reader.EnqueueDataDeserialization(obj, s_deserializeDataDelayedAction);
                return obj;
            }

            public static void Serialize(TerrainTowerAreaChangeCmd value, BlobWriter writer)
            {
                if (writer.TryStartClassSerialization(value)) writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
            }

#endif
        }
    }
}