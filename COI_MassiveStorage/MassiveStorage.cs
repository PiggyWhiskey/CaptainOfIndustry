using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;
using System;

namespace MassiveStorage
{
    public sealed class MassiveStorageMod : DataOnlyMod
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static Version ModVersion = new Version(1, 0, 0);
        public override string Name => "Massive Storage Mod";

        public override int Version => 1;

        // Mod constructor that lists mod dependencies as parameters.
        // This guarantee that all listed mods will be loaded before this mod.
        // It is a good idea to depend on both `Mafi.Core.CoreMod` and `Mafi.Base.BaseMod`.
#pragma warning disable IDE0060 // Remove unused parameter
        public MassiveStorageMod(CoreMod coreMod, BaseMod baseMod) { }

        public override void RegisterPrototypes(ProtoRegistrator registrator)
        {
            //Build Prototypes
#pragma warning disable IDE0001 // Simplify Names
            registrator.RegisterData<MassiveStorage.Buildings.StorageFluidProtoBuilder>();
            registrator.RegisterData<MassiveStorage.Buildings.StorageLooseProtoBuilder>();
            registrator.RegisterData<MassiveStorage.Buildings.StorageUnitProtoBuilder>();
            registrator.RegisterData<MassiveStorage.Buildings.OreSortingPlantUpgradableProtoBuilder>();

            //Build Research Tree
            registrator.RegisterData<MassiveStorage.Research.Research>();
#pragma warning restore IDE0001 // Simplify Names

        }
    }
}