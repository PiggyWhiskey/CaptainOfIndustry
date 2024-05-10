using Mafi;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Factory;
using Mafi.Core.Mods;
using MassiveStorage.BuildingDefaults;

namespace MassiveStorage.Buildings
{
    internal sealed class StorageFluidProtoBuilder : IModData
    {
        //Get Generic properties/defaults
        private readonly DefaultsStorageFluid defaults = new DefaultsStorageFluid();

        public void RegisterData(ProtoRegistrator registrator)
        {
            //Tier 4
            _ = registrator.StorageProtoBuilder
                .Start(string.Concat(defaults.IDDescription, "T4"), CustomIDs.Buildings.StorageFluidT4)
                .Description(string.Concat(defaults.Description, "T4"))
                .SetCost(defaults.BuildCost.T4)
                .SetCapacity(defaults.StorageCapacity.T4)
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetFluidIndicatorGfxParams("part1/icon", "part1/liquid", new FluidIndicatorGfxParams(1f, 2.6f, 2f))
                .SetNoTransferLimit()
                .BuildAsFluidAndAdd()
                .AddParam(new DrawArrowWileBuildingProtoParam(4f));

            //Tier 3
            _ = registrator.StorageProtoBuilder
                .Start(string.Concat(defaults.IDDescription, "T3"), CustomIDs.Buildings.StorageFluidT3)
                .Description(string.Concat(defaults.Description, "T3"))
                .SetCost(defaults.BuildCost.T3)
                .SetCapacity(defaults.StorageCapacity.T3)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<StorageProto>(CustomIDs.Buildings.StorageFluidT4))
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetFluidIndicatorGfxParams("part1/icon", "part1/liquid", new FluidIndicatorGfxParams(1f, 2.6f, 2f))
                .SetNoTransferLimit()
                .BuildAsFluidAndAdd()
                .AddParam(new DrawArrowWileBuildingProtoParam(4f));

            //Tier 2
            _ = registrator.StorageProtoBuilder
                .Start(string.Concat(defaults.IDDescription, "T2"), CustomIDs.Buildings.StorageFluidT2)
                .Description(string.Concat(defaults.Description, "T2"))
                .SetCost(defaults.BuildCost.T2)
                .SetCapacity(defaults.StorageCapacity.T2)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<StorageProto>(CustomIDs.Buildings.StorageFluidT3))
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetFluidIndicatorGfxParams("part1/icon", "part1/liquid", new FluidIndicatorGfxParams(1f, 2.6f, 2f))
                .SetNoTransferLimit()
                .BuildAsFluidAndAdd()
                .AddParam(new DrawArrowWileBuildingProtoParam(4f));

            //Tier 1
            _ = registrator.StorageProtoBuilder
                .Start(string.Concat(defaults.IDDescription, "T1"), CustomIDs.Buildings.StorageFluidT1)
                .Description(string.Concat(defaults.Description, "T1"))
                .SetCost(defaults.BuildCost.T1)
                .SetCapacity(defaults.StorageCapacity.T1)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<StorageProto>(CustomIDs.Buildings.StorageFluidT2))
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetFluidIndicatorGfxParams("part1/icon", "part1/liquid", new FluidIndicatorGfxParams(1f, 2.6f, 2f))
                .SetNoTransferLimit()
                .BuildAsFluidAndAdd()
                .AddParam(new DrawArrowWileBuildingProtoParam(4f));
        }
    }
}
