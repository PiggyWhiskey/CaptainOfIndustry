using Mafi;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using System;

namespace MassiveStorage.CustomBuilding.OreSortingPlantWithUpgrade
{
    public sealed class OreSortingPlantUpgradableProto : OreSortingPlantProto, IProtoWithUpgrade<OreSortingPlantUpgradableProto>
    {
        public override Type EntityType => typeof(OreSortingPlantUpgradable);
        public UpgradeData<OreSortingPlantUpgradableProto> Upgrade { get; }
        public IUpgradeData UpgradeNonGeneric => Upgrade;
        public OreSortingPlantUpgradableProto(
            ID id,
            Str strings,
            EntityLayout layout,
            EntityCosts costs,
            Quantity inputBufferCapacity,
            Quantity outputBuffersCapacity,
            Duration duration,
            Quantity quantityPerDuration,
            Percent conversionLoss,
            Electricity electricityConsumed,
            Option<OreSortingPlantUpgradableProto> upgradeData,
            ImmutableArray<AnimationParams> animationParams, Gfx graphics
            ) : base(id, strings, layout, costs,
                inputBufferCapacity, outputBuffersCapacity,
                duration, quantityPerDuration, conversionLoss,
                electricityConsumed, animationParams, graphics)
        {
            Upgrade = new UpgradeData<OreSortingPlantUpgradableProto>(this, upgradeData);
        }
    }

}
