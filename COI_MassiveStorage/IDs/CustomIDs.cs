using Mafi.Base;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Research;

namespace MassiveStorage
{
    public partial class CustomIDs
    {
        public static class Buildings
        {
            //Storage Fluid
            public static readonly MachineProto.ID StorageFluidT1 = Ids.Machines.CreateId("MassiveStorageFluidT1");
            public static readonly MachineProto.ID StorageFluidT2 = Ids.Machines.CreateId("MassiveStorageFluidT2");
            public static readonly MachineProto.ID StorageFluidT3 = Ids.Machines.CreateId("MassiveStorageFluidT3");
            public static readonly MachineProto.ID StorageFluidT4 = Ids.Machines.CreateId("MassiveStorageFluidT4");

            //Storage Loose
            public static readonly MachineProto.ID StorageLooseT1 = Ids.Machines.CreateId("MassiveStorageLooseT1");
            public static readonly MachineProto.ID StorageLooseT2 = Ids.Machines.CreateId("MassiveStorageLooseT2");
            public static readonly MachineProto.ID StorageLooseT3 = Ids.Machines.CreateId("MassiveStorageLooseT3");
            public static readonly MachineProto.ID StorageLooseT4 = Ids.Machines.CreateId("MassiveStorageLooseT4");

            //Storage Unit
            public static readonly MachineProto.ID StorageUnitT1 = Ids.Machines.CreateId("MassiveStorageUnitT1");
            public static readonly MachineProto.ID StorageUnitT2 = Ids.Machines.CreateId("MassiveStorageUnitT2");
            public static readonly MachineProto.ID StorageUnitT3 = Ids.Machines.CreateId("MassiveStorageUnitT3");
            public static readonly MachineProto.ID StorageUnitT4 = Ids.Machines.CreateId("MassiveStorageUnitT4");

            //Ore Sorting Plant
            //public static readonly MachineProto.ID OreSortingT1 = Ids.Machines.CreateId("MassiveOreSortingPlantT1");
            //public static readonly MachineProto.ID OreSortingT2 = Ids.Machines.CreateId("MassiveOreSortingPlantT2");
            //public static readonly MachineProto.ID OreSortingT3 = Ids.Machines.CreateId("MassiveOreSortingPlantT3");
            //public static readonly MachineProto.ID OreSortingT4 = Ids.Machines.CreateId("MassiveOreSortingPlantT4");

            public static readonly LayoutEntityProto.ID OreSortingT1 = new LayoutEntityProto.ID("MassiveOreSortingPlantT1");
            public static readonly LayoutEntityProto.ID OreSortingT2 = new LayoutEntityProto.ID("MassiveOreSortingPlantT2");
            public static readonly LayoutEntityProto.ID OreSortingT3 = new LayoutEntityProto.ID("MassiveOreSortingPlantT3");
            public static readonly LayoutEntityProto.ID OreSortingT4 = new LayoutEntityProto.ID("MassiveOreSortingPlantT4");

            //public static readonly MachineProto.ID OreSortingT1 = new MachineProto.ID("MassiveOreSortingPlantT1");
            //public static readonly MachineProto.ID OreSortingT2 = new MachineProto.ID("MassiveOreSortingPlantT2");
            //public static readonly MachineProto.ID OreSortingT3 = new MachineProto.ID("MassiveOreSortingPlantT3");
            //public static readonly MachineProto.ID OreSortingT4 = new MachineProto.ID("MassiveOreSortingPlantT4");

        }

        public static class Research
        {
            public static readonly ResearchNodeProto.ID MassiveStorageT1 = Ids.Research.CreateId("MassiveStorageT1");
            public static readonly ResearchNodeProto.ID MassiveStorageT2 = Ids.Research.CreateId("MassiveStorageT2");
            public static readonly ResearchNodeProto.ID MassiveStorageT3 = Ids.Research.CreateId("MassiveStorageT3");
            public static readonly ResearchNodeProto.ID MassiveStorageT4 = Ids.Research.CreateId("MassiveStorageT4");
        }
    }
}
