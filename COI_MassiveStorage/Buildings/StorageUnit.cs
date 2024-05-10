using Mafi.Core.Buildings.Storages;
using Mafi.Core.Mods;
using MassiveStorage.BuildingDefaults;

namespace MassiveStorage.Buildings
{
    internal sealed class StorageUnitProtoBuilder : IModData
    {
        //Get Generic properties/defaults
        private readonly DefaultsStorageUnit defaults = new DefaultsStorageUnit();
        public void RegisterData(ProtoRegistrator registrator)
        {

            //Tier 4
            _ = registrator.StorageProtoBuilder
                .Start(string.Concat(defaults.IDDescription, "T4"), CustomIDs.Buildings.StorageUnitT4)
                .Description(string.Concat(defaults.Description, "T4"))
                .SetCost(defaults.BuildCost.T4)
                .SetCapacity(defaults.StorageCapacity.T4)
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetNoTransferLimit()
                .BuildUnitAndAdd(new UnitStorageRackData[1]
                {
                    new UnitStorageRackData(3, 6, -4f)
                }, "Assets/Base/Buildings/Storages/UnitT1_rack.prefab");

            //Tier 3
            _ = registrator.StorageProtoBuilder
                .Start(string.Concat(defaults.IDDescription, "T3"), CustomIDs.Buildings.StorageUnitT3)
                .Description(string.Concat(defaults.Description, "T3"))
                .SetCost(defaults.BuildCost.T3)
                .SetCapacity(defaults.StorageCapacity.T3)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<StorageProto>(CustomIDs.Buildings.StorageUnitT4))
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetNoTransferLimit()
                .BuildUnitAndAdd(new UnitStorageRackData[1]
                {
                    new UnitStorageRackData(3, 6, -4f)
                }, "Assets/Base/Buildings/Storages/UnitT1_rack.prefab");

            //Tier 2
            _ = registrator.StorageProtoBuilder
                .Start(string.Concat(defaults.IDDescription, "T2"), CustomIDs.Buildings.StorageUnitT2)
                .Description(string.Concat(defaults.Description, "T2"))
                .SetCost(defaults.BuildCost.T2)
                .SetCapacity(defaults.StorageCapacity.T2)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<StorageProto>(CustomIDs.Buildings.StorageUnitT3))
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetNoTransferLimit()
                .BuildUnitAndAdd(new UnitStorageRackData[1]
                {
                    new UnitStorageRackData(3, 6, -4f)
                }, "Assets/Base/Buildings/Storages/UnitT1_rack.prefab");

            //Tier 1
            _ = registrator.StorageProtoBuilder
                .Start(string.Concat(defaults.IDDescription, "T1"), CustomIDs.Buildings.StorageUnitT1)
                .Description(string.Concat(defaults.Description, "T1"))
                .SetCost(defaults.BuildCost.T1)
                .SetCapacity(defaults.StorageCapacity.T1)
                .SetNextTier(registrator.PrototypesDb.GetOrThrow<StorageProto>(CustomIDs.Buildings.StorageUnitT2))
                .SetLayout(defaults.Layout)
                .SetCategories(defaults.Category)
                .SetPrefabPath(defaults.PrefabPath)
                .SetCustomIconPath(defaults.CustomIconPath)
                .SetNoTransferLimit()
                .BuildUnitAndAdd(new UnitStorageRackData[1]
                {
                    new UnitStorageRackData(3, 6, -4f)
                }, "Assets/Base/Buildings/Storages/UnitT1_rack.prefab");
        }
    }
}
