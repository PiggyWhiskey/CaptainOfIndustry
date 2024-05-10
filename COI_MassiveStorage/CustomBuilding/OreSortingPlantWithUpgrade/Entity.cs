using Mafi;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System.Globalization;
using System;

namespace MassiveStorage.CustomBuilding.OreSortingPlantWithUpgrade
{
    [GenerateSerializer(false, null, 0)]
    public class OreSortingPlantUpgradable : OreSortingPlant, IUpgradableEntity
    {

        private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
        private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

        public IUpgrader Upgrader { get; private set; }
        private OreSortingPlantUpgradableProto m_proto;

        [DoNotSave(0, null)]
        public new OreSortingPlantUpgradableProto Prototype
        {
            get => m_proto;
            protected set
            {
                m_proto = value;
                base.Prototype = value;
            }
        }

        public OreSortingPlantUpgradable(
            EntityId id,
            OreSortingPlantUpgradableProto proto,
            TileTransform transform,
            EntityContext context,
            ICalendar calendar,
            IProductsManager productsManager,
            IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
            IVehicleBuffersRegistry vehicleBuffersRegistry,
            IAnimationStateFactory animationStateFactory,
            ILayoutEntityUpgraderFactory upgraderFactory
            ) : base(id, proto, transform, context, calendar, productsManager, maintenanceProvidersFactory, vehicleBuffersRegistry, animationStateFactory)
        {
            Upgrader = upgraderFactory.CreateInstance(this, proto);
            Prototype = proto;
        }

        public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
        {
            errorMessage = LocStrFormatted.Empty;
            return true;
        }

        public void UpgradeSelf()
        {
            if (Prototype.Upgrade.NextTier.IsNone)
            {
                Log.Error("Upgrade not available!");
            }
            else
            {
                Prototype = Prototype.Upgrade.NextTier.Value;
            }
        }

        public static void Serialize(OreSortingPlantUpgradable value, BlobWriter writer)
        {
            if (writer.TryStartClassSerialization(value))
            {
                writer.EnqueueDataSerialization(value, s_serializeDataDelayedAction);
            }
        }

        protected override void SerializeData(BlobWriter writer)
        {
            base.SerializeData(writer);
            writer.WriteGeneric(Upgrader);
            writer.WriteGeneric(m_proto);
        }

        public static new OreSortingPlantUpgradable Deserialize(BlobReader reader)
        {
            if (reader.TryStartClassDeserialization(out OreSortingPlantUpgradable obj, (Func<BlobReader, Type, OreSortingPlantUpgradable>)null))
            {
                reader.EnqueueDataDeserialization(obj, s_deserializeDataDelayedAction);
            }
            return obj;
        }

        protected override void DeserializeData(BlobReader reader)
        {
            base.DeserializeData(reader);
            reader.SetField(this, "Upgrader", reader.ReadGenericAs<IUpgrader>());
            reader.SetField(this, "m_proto", reader.ReadGenericAs<OreSortingPlantUpgradableProto>());
        }
        static OreSortingPlantUpgradable()
        {
            s_serializeDataDelayedAction = delegate (object obj, BlobWriter writer)
            {
                ((OreSortingPlantUpgradable)obj).SerializeData(writer);
            };
            s_deserializeDataDelayedAction = delegate (object obj, BlobReader reader)
            {
                ((OreSortingPlantUpgradable)obj).DeserializeData(reader);
            };
        }
    }
}
