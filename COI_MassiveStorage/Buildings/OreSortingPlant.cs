using Mafi;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Mods;
using MassiveStorage.BuildingDefaults;
using MassiveStorage.CustomBuilding.OreSortingPlantWithUpgrade;

namespace MassiveStorage.Buildings
{
    public class OreSortingPlantUpgradableProtoBuilder : IModData
    {
        private readonly DefaultsOreSortingBase defaults = new DefaultsOreSortingBase();
        public OreSortingPlantUpgradableBuilder protBuilder;
        public void RegisterData(ProtoRegistrator registrator)
        {
            protBuilder = new OreSortingPlantUpgradableBuilder(registrator);
            //T4
            _ = protBuilder
                .Start(string.Concat(defaults.IDDescription, "T4"), CustomIDs.Buildings.OreSortingT4)
                .Description(string.Concat(defaults.Description, "T4"))
                .SetCost(defaults.BuildCost.T4)
                .SetBufferCapacity(defaults.StorageCapacity.T4)
                .SetWorkQuantityPerDuration(defaults.ProcessQuantity.T4)
                .SetElectricityConsumption(defaults.ElectricityConsumed.T4)
                .SetWorkDurationSeconds(defaults.WorkDuration.T4)
                .SetWorkConversionLoss(defaults.ConversionLoss.T4)
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetProtoAnimationParams(ImmutableArray.Create((AnimationParams)AnimationParams.Loop()))
                .BuildAndAdd();

            //T3
            _ = protBuilder
                .Start(string.Concat(defaults.IDDescription, "T3"), CustomIDs.Buildings.OreSortingT3)
                .Description(string.Concat(defaults.Description, "T3"))
                .SetCost(defaults.BuildCost.T3)
                .SetBufferCapacity(defaults.StorageCapacity.T3)
                .SetWorkQuantityPerDuration(defaults.ProcessQuantity.T3)
                .SetElectricityConsumption(defaults.ElectricityConsumed.T3)
                .SetWorkDurationSeconds(defaults.WorkDuration.T3)
                .SetWorkConversionLoss(defaults.ConversionLoss.T3)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<OreSortingPlantUpgradableProto>(CustomIDs.Buildings.OreSortingT4))
                .SetCategories(defaults.Category)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetPrefabPath(defaults.PrefabPath)
                .SetLayout(defaults.Layout)
                .SetProtoAnimationParams(ImmutableArray.Create((AnimationParams)AnimationParams.Loop()))
                .BuildAndAdd();

            //T2
            _ = protBuilder
                .Start(string.Concat(defaults.IDDescription, "T2"), CustomIDs.Buildings.OreSortingT2)
                .Description(string.Concat(defaults.Description, "T2"))
                .SetCost(defaults.BuildCost.T2)
                .SetBufferCapacity(defaults.StorageCapacity.T2)
                .SetWorkQuantityPerDuration(defaults.ProcessQuantity.T2)
                .SetElectricityConsumption(defaults.ElectricityConsumed.T2)
                .SetWorkDurationSeconds(defaults.WorkDuration.T2)
                .SetWorkConversionLoss(defaults.ConversionLoss.T2)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<OreSortingPlantUpgradableProto>(CustomIDs.Buildings.OreSortingT3))
                .SetCategories(defaults.Category)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetPrefabPath(defaults.PrefabPath)
                .SetLayout(defaults.Layout)
                .SetProtoAnimationParams(ImmutableArray.Create((AnimationParams)AnimationParams.Loop()))
                .BuildAndAdd();

            //T1
            _ = protBuilder
                .Start(string.Concat(defaults.IDDescription, "T1"), CustomIDs.Buildings.OreSortingT1)
                .Description(string.Concat(defaults.Description, "T1"))
                .SetCost(defaults.BuildCost.T1)
                .SetBufferCapacity(defaults.StorageCapacity.T1)
                .SetWorkQuantityPerDuration(defaults.ProcessQuantity.T1)
                .SetElectricityConsumption(defaults.ElectricityConsumed.T1)
                .SetWorkDurationSeconds(defaults.WorkDuration.T1)
                .SetWorkConversionLoss(defaults.ConversionLoss.T1)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<OreSortingPlantUpgradableProto>(CustomIDs.Buildings.OreSortingT2))
                .SetCategories(defaults.Category)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetPrefabPath(defaults.PrefabPath)
                .SetLayout(defaults.Layout)
                .SetProtoAnimationParams(ImmutableArray.Create((AnimationParams)AnimationParams.Loop()))
                .BuildAndAdd();

            /*
            

            //Gfx
            ImmutableArray<AnimationParams> animationParams = ImmutableArray.Create((AnimationParams)AnimationParams.Loop());
            OreSortingPlantProto.Gfx ProtoGraphics = new OreSortingPlantProto.Gfx(
                                                            prefabPath: defaults.PrefabPath,
                                                            smoothPileObjectPath: "Pile_Soft",
                                                            loosePileTextureParams: new LoosePileTextureParams(0.9f),
                                                            prefabOrigin: new RelTile3f(),
                                                            customIconPath: defaults.CustomIconPath,
                                                            color: new ColorRgba(),
                                                            visualizedLayers: new LayoutEntityProto.VisualizedLayers?(),
                                                            categories: new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(defaults.Category))
                                                            );

            //T4
            registrator.PrototypesDb.Add(
                new OreSortingPlantUpgradableProto(
                    id: CustomIDs.Buildings.OreSortingT4,
                    strings: CreateStr(CustomIDs.Buildings.OreSortingT4, string.Concat(defaults.IDDescription, "T4"), string.Concat(defaults.Description, "T4")),
                    layout: registrator.LayoutParser.ParseLayoutOrThrow(defaults.Layout),
                    costs: defaults.BuildCost.T4.MapToEntityCosts(registrator),
                    inputBufferCapacity: defaults.StorageCapacity.T4.Quantity(),
                    outputBuffersCapacity: defaults.StorageCapacity.T4.Quantity(),
                    duration: defaults.WorkDuration.T4.Seconds(),
                    quantityPerDuration: defaults.ProcessQuantity.T4.Quantity(),
                    conversionLoss: defaults.ConversionLoss.T4.Percent(),
                    electricityConsumed: defaults.ElectricityConsumed.T4.Kw(),
                    upgradeData: new Option<OreSortingPlantUpgradableProto>(),
                    animationParams: animationParams,
                    graphics: ProtoGraphics
                    )
            ).SetAvailability(false);

            //T3
            Log.Info(string.Format("[MassiveStorage]: Adding Proto {0}", "T3"));
            registrator.PrototypesDb.Add(
                new OreSortingPlantUpgradableProto(
                    id: CustomIDs.Buildings.OreSortingT3,
                    strings: CreateStr(CustomIDs.Buildings.OreSortingT3, string.Concat(defaults.IDDescription, "T3"), string.Concat(defaults.Description, "T3")),
                    layout: registrator.LayoutParser.ParseLayoutOrThrow(defaults.Layout),
                    costs: defaults.BuildCost.T3.MapToEntityCosts(registrator),
                    inputBufferCapacity: defaults.StorageCapacity.T3.Quantity(),
                    outputBuffersCapacity: defaults.StorageCapacity.T3.Quantity(),
                    duration: defaults.WorkDuration.T3.Seconds(),
                    quantityPerDuration: defaults.ProcessQuantity.T3.Quantity(),
                    conversionLoss: defaults.ConversionLoss.T3.Percent(),
                    electricityConsumed: defaults.ElectricityConsumed.T3.Kw(),
                    upgradeData: registrator.PrototypesDb.GetOrThrow<OreSortingPlantUpgradableProto>(CustomIDs.Buildings.OreSortingT4),
                    animationParams: animationParams,
                    graphics: ProtoGraphics
                    )
            ).SetAvailability(false);

            //T2
            Log.Info(string.Format("[MassiveStorage]: Adding Proto {0}", "T2"));
            registrator.PrototypesDb.Add(
                new OreSortingPlantUpgradableProto(
                    id: CustomIDs.Buildings.OreSortingT2,
                    strings: CreateStr(CustomIDs.Buildings.OreSortingT2, string.Concat(defaults.IDDescription, "T2"), string.Concat(defaults.Description, "T2")),
                    layout: registrator.LayoutParser.ParseLayoutOrThrow(defaults.Layout),
                    costs: defaults.BuildCost.T2.MapToEntityCosts(registrator),
                    inputBufferCapacity: defaults.StorageCapacity.T2.Quantity(),
                    outputBuffersCapacity: defaults.StorageCapacity.T2.Quantity(),
                    duration: defaults.WorkDuration.T2.Seconds(),
                    quantityPerDuration: defaults.ProcessQuantity.T2.Quantity(),
                    conversionLoss: defaults.ConversionLoss.T2.Percent(),
                    electricityConsumed: defaults.ElectricityConsumed.T2.Kw(),
                    upgradeData: registrator.PrototypesDb.GetOrThrow<OreSortingPlantUpgradableProto>(CustomIDs.Buildings.OreSortingT3),
                    animationParams: animationParams,
                    graphics: ProtoGraphics
                    )
            ).SetAvailability(false);

            //T1
            Log.Info(string.Format("[MassiveStorage]: Adding Proto {0}", "T1"));
            registrator.PrototypesDb.Add(
                new OreSortingPlantUpgradableProto(
                    id: CustomIDs.Buildings.OreSortingT1,
                    strings: CreateStr(CustomIDs.Buildings.OreSortingT1, string.Concat(defaults.IDDescription, "T1"), string.Concat(defaults.Description, "T1")),
                    layout: registrator.LayoutParser.ParseLayoutOrThrow(defaults.Layout),
                    costs: defaults.BuildCost.T1.MapToEntityCosts(registrator),
                    inputBufferCapacity: defaults.StorageCapacity.T1.Quantity(),
                    outputBuffersCapacity: defaults.StorageCapacity.T1.Quantity(),
                    duration: defaults.WorkDuration.T1.Seconds(),
                    quantityPerDuration: defaults.ProcessQuantity.T1.Quantity(),
                    conversionLoss: defaults.ConversionLoss.T1.Percent(),
                    electricityConsumed: defaults.ElectricityConsumed.T1.Kw(),
                    upgradeData: registrator.PrototypesDb.GetOrThrow<OreSortingPlantUpgradableProto>(CustomIDs.Buildings.OreSortingT2),
                    animationParams: animationParams,
                    graphics: ProtoGraphics
                    )
            ).SetAvailability(false);
            */
        }
    }
}
