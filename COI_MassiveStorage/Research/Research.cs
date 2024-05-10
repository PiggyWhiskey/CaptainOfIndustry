using Mafi;
using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Research;

namespace MassiveStorage.Research
{
    internal class Research : IModData
    {
        public void RegisterData(ProtoRegistrator registrator)
        {
            _ = registrator.ResearchNodeProtoBuilder
                .Start("Massive Storage Containers", CustomIDs.Research.MassiveStorageT1)
                .Description("Massive Storage Containers T1")
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageFluidT1)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageLooseT1)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageUnitT1)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.OreSortingT1)
                .SetGridPosition(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.IronSmeltingScrap).GridPosition + new Vector2i(0, -4))
                .SetCosts(new ResearchCostsTpl(1))
                .BuildAndAdd();

            _ = registrator.ResearchNodeProtoBuilder
                .Start("Massive Storage Containers", CustomIDs.Research.MassiveStorageT2)
                .Description("Massive Storage Containers T1")
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageFluidT2)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageLooseT2)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageUnitT2)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.OreSortingT2)
                .SetGridPosition(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(CustomIDs.Research.MassiveStorageT1).GridPosition + new Vector2i(4, 0))
                .SetCosts(new ResearchCostsTpl(2))
                .AddParents(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(CustomIDs.Research.MassiveStorageT1))
                .BuildAndAdd();

            _ = registrator.ResearchNodeProtoBuilder
                .Start("Massive Storage Containers", CustomIDs.Research.MassiveStorageT3)
                .Description("Massive Storage Containers T1")
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageFluidT3)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageLooseT3)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageUnitT3)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.OreSortingT3)
                .SetGridPosition(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(CustomIDs.Research.MassiveStorageT2).GridPosition + new Vector2i(4, 0))
                .SetCosts(new ResearchCostsTpl(4))
                .AddParents(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(CustomIDs.Research.MassiveStorageT2))
                .BuildAndAdd();

            _ = registrator.ResearchNodeProtoBuilder
                .Start("Massive Storage Containers", CustomIDs.Research.MassiveStorageT4)
                .Description("Massive Storage Containers T1")
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageFluidT4)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageLooseT4)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.StorageUnitT4)
                .AddLayoutEntityToUnlock(CustomIDs.Buildings.OreSortingT4)
                .SetGridPosition(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(CustomIDs.Research.MassiveStorageT3).GridPosition + new Vector2i(4, 0))
                .SetCosts(new ResearchCostsTpl(8))
                .AddParents(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(CustomIDs.Research.MassiveStorageT3))
                .BuildAndAdd();
        }
    }
}
