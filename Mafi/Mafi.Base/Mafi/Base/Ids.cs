// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Ids
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Storages.NuclearWaste;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Datacenters;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Fleet;
using Mafi.Core.Localization.Quantity;
using Mafi.Core.Map;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Base
{
  public static class Ids
  {
    public static class Buildings
    {
      public static readonly StaticEntityProto.ID FarmT1;
      public static readonly StaticEntityProto.ID FarmT2;
      public static readonly StaticEntityProto.ID FarmT3;
      public static readonly StaticEntityProto.ID FarmT4;
      public static readonly StaticEntityProto.ID ChickenFarm;
      public static readonly StaticEntityProto.ID CaptainOfficeT1;
      public static readonly StaticEntityProto.ID CaptainOfficeT2;
      public static readonly StaticEntityProto.ID ResearchLab1;
      public static readonly StaticEntityProto.ID ResearchLab2;
      public static readonly StaticEntityProto.ID ResearchLab3;
      public static readonly StaticEntityProto.ID ResearchLab4;
      public static readonly StaticEntityProto.ID ResearchLab5;
      public static readonly StaticEntityProto.ID Beacon;
      public static readonly StaticEntityProto.ID VehiclesDepot;
      public static readonly StaticEntityProto.ID VehiclesDepotT2;
      public static readonly StaticEntityProto.ID VehiclesDepotT3;
      public static readonly MachineProto.ID MaintenanceDepotT0;
      public static readonly MachineProto.ID MaintenanceDepotT1;
      public static readonly MachineProto.ID MaintenanceDepotT2;
      public static readonly MachineProto.ID MaintenanceDepotT3;
      public static readonly CargoDepotProto.ID CargoDepotT1;
      public static readonly CargoDepotProto.ID CargoDepotT2;
      public static readonly CargoDepotProto.ID CargoDepotT3;
      public static readonly CargoDepotProto.ID CargoDepotT4;
      public static readonly StaticEntityProto.ID MineTower;
      public static readonly StaticEntityProto.ID ForestryTower;
      public static readonly StaticEntityProto.ID RainwaterHarvester;
      public static readonly StaticEntityProto.ID FuelStationT1;
      public static readonly StaticEntityProto.ID FuelStationT2;
      public static readonly StaticEntityProto.ID FuelStationT3;
      public static readonly StaticEntityProto.ID FuelStationHydrogenT1;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleFluidT1;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleFluidT2;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleFluidT3;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleLooseT1;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleLooseT2;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleLooseT3;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleUnitT1;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleUnitT2;
      public static readonly CargoDepotModuleProto.ID CargoDepotModuleUnitT3;
      public static readonly StaticEntityProto.ID StorageFluid;
      public static readonly StaticEntityProto.ID StorageFluidT2;
      public static readonly StaticEntityProto.ID StorageFluidT3;
      public static readonly StaticEntityProto.ID StorageFluidT4;
      public static readonly StaticEntityProto.ID StorageLoose;
      public static readonly StaticEntityProto.ID StorageLooseT2;
      public static readonly StaticEntityProto.ID StorageLooseT3;
      public static readonly StaticEntityProto.ID StorageLooseT4;
      public static readonly StaticEntityProto.ID StorageUnit;
      public static readonly StaticEntityProto.ID StorageUnitT2;
      public static readonly StaticEntityProto.ID StorageUnitT3;
      public static readonly StaticEntityProto.ID StorageUnitT4;
      public static readonly StaticEntityProto.ID NuclearWasteStorage;
      public static readonly StaticEntityProto.ID ThermalStorage;
      public static readonly StaticEntityProto.ID TradeDock;
      public static readonly StaticEntityProto.ID Housing;
      public static readonly StaticEntityProto.ID HousingT2;
      public static readonly StaticEntityProto.ID HousingT3;
      public static readonly StaticEntityProto.ID SettlementLandfillModule;
      public static readonly StaticEntityProto.ID SettlementRecyclablesModule;
      public static readonly StaticEntityProto.ID SettlementBiomassModule;
      public static readonly StaticEntityProto.ID SettlementWaterModule;
      public static readonly StaticEntityProto.ID SettlementFoodModule;
      public static readonly StaticEntityProto.ID SettlementFoodModuleT2;
      public static readonly StaticEntityProto.ID SettlementPowerModule;
      public static readonly StaticEntityProto.ID SettlementHouseholdGoodsModule;
      public static readonly StaticEntityProto.ID SettlementHouseholdAppliancesModule;
      public static readonly StaticEntityProto.ID SettlementConsumerElectronicsModule;
      public static readonly StaticEntityProto.ID SettlementPillar;
      public static readonly StaticEntityProto.ID SettlementFountain;
      public static readonly StaticEntityProto.ID SettlementSquare1;
      public static readonly StaticEntityProto.ID SettlementSquare2;
      public static readonly StaticEntityProto.ID Clinic;
      public static readonly StaticEntityProto.ID WasteSortingPlant;
      public static readonly StaticEntityProto.ID OreSortingPlantT1;
      public static readonly StaticEntityProto.ID Shipyard;
      public static readonly StaticEntityProto.ID Shipyard2;
      public static readonly StaticEntityProto.ID VehicleRamp;
      public static readonly StaticEntityProto.ID VehicleRamp2;
      public static readonly StaticEntityProto.ID VehicleRamp3;
      public static readonly StaticEntityProto.ID Ruins;
      public static readonly StaticEntityProto.ID UniversalProductsSource;
      public static readonly StaticEntityProto.ID UniversalProductsSink;
      public static readonly StaticEntityProto.ID NuclearReactor;
      public static readonly StaticEntityProto.ID NuclearReactorT2;
      public static readonly StaticEntityProto.ID FastBreederReactor;
      public static readonly StaticEntityProto.ID RocketAssemblyDepot;
      public static readonly StaticEntityProto.ID RocketLaunchPad;
      public static readonly StaticEntityProto.ID RetainingWallStraight1;
      public static readonly StaticEntityProto.ID RetainingWallStraight4;
      public static readonly StaticEntityProto.ID RetainingWallCorner;
      public static readonly StaticEntityProto.ID RetainingWallCross;
      public static readonly StaticEntityProto.ID RetainingWallTee;
      public static readonly StaticEntityProto.ID BarrierStraight1;
      public static readonly StaticEntityProto.ID BarrierCorner;
      public static readonly StaticEntityProto.ID BarrierCross;
      public static readonly StaticEntityProto.ID BarrierTee;
      public static readonly StaticEntityProto.ID BarrierEnding;
      public static readonly StaticEntityProto.ID StatueOfMaintenance;
      public static readonly StaticEntityProto.ID StatueOfMaintenanceGolden;
      public static readonly StaticEntityProto.ID TombOfCaptainsStage1;
      public static readonly StaticEntityProto.ID TombOfCaptainsStage2;
      public static readonly StaticEntityProto.ID TombOfCaptainsStage3;
      public static readonly StaticEntityProto.ID TombOfCaptainsStage4;
      public static readonly StaticEntityProto.ID TombOfCaptainsStage5;
      public static readonly StaticEntityProto.ID TombOfCaptainsStageFinal;

      static Buildings()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Buildings.FarmT1 = new StaticEntityProto.ID(nameof (FarmT1));
        Ids.Buildings.FarmT2 = new StaticEntityProto.ID(nameof (FarmT2));
        Ids.Buildings.FarmT3 = new StaticEntityProto.ID(nameof (FarmT3));
        Ids.Buildings.FarmT4 = new StaticEntityProto.ID(nameof (FarmT4));
        Ids.Buildings.ChickenFarm = new StaticEntityProto.ID(nameof (ChickenFarm));
        Ids.Buildings.CaptainOfficeT1 = new StaticEntityProto.ID(nameof (CaptainOfficeT1));
        Ids.Buildings.CaptainOfficeT2 = new StaticEntityProto.ID(nameof (CaptainOfficeT2));
        Ids.Buildings.ResearchLab1 = new StaticEntityProto.ID(nameof (ResearchLab1));
        Ids.Buildings.ResearchLab2 = new StaticEntityProto.ID(nameof (ResearchLab2));
        Ids.Buildings.ResearchLab3 = new StaticEntityProto.ID(nameof (ResearchLab3));
        Ids.Buildings.ResearchLab4 = new StaticEntityProto.ID(nameof (ResearchLab4));
        Ids.Buildings.ResearchLab5 = new StaticEntityProto.ID(nameof (ResearchLab5));
        Ids.Buildings.Beacon = new StaticEntityProto.ID(nameof (Beacon));
        Ids.Buildings.VehiclesDepot = new StaticEntityProto.ID(nameof (VehiclesDepot));
        Ids.Buildings.VehiclesDepotT2 = new StaticEntityProto.ID(nameof (VehiclesDepotT2));
        Ids.Buildings.VehiclesDepotT3 = new StaticEntityProto.ID(nameof (VehiclesDepotT3));
        Ids.Buildings.MaintenanceDepotT0 = new MachineProto.ID(nameof (MaintenanceDepotT0));
        Ids.Buildings.MaintenanceDepotT1 = new MachineProto.ID(nameof (MaintenanceDepotT1));
        Ids.Buildings.MaintenanceDepotT2 = new MachineProto.ID(nameof (MaintenanceDepotT2));
        Ids.Buildings.MaintenanceDepotT3 = new MachineProto.ID(nameof (MaintenanceDepotT3));
        Ids.Buildings.CargoDepotT1 = new CargoDepotProto.ID(nameof (CargoDepotT1));
        Ids.Buildings.CargoDepotT2 = new CargoDepotProto.ID(nameof (CargoDepotT2));
        Ids.Buildings.CargoDepotT3 = new CargoDepotProto.ID(nameof (CargoDepotT3));
        Ids.Buildings.CargoDepotT4 = new CargoDepotProto.ID(nameof (CargoDepotT4));
        Ids.Buildings.MineTower = IdsCore.Buildings.MineTower;
        Ids.Buildings.ForestryTower = new StaticEntityProto.ID(nameof (ForestryTower));
        Ids.Buildings.RainwaterHarvester = new StaticEntityProto.ID(nameof (RainwaterHarvester));
        Ids.Buildings.FuelStationT1 = new StaticEntityProto.ID(nameof (FuelStationT1));
        Ids.Buildings.FuelStationT2 = new StaticEntityProto.ID(nameof (FuelStationT2));
        Ids.Buildings.FuelStationT3 = new StaticEntityProto.ID(nameof (FuelStationT3));
        Ids.Buildings.FuelStationHydrogenT1 = new StaticEntityProto.ID(nameof (FuelStationHydrogenT1));
        Ids.Buildings.CargoDepotModuleFluidT1 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleFluidT1));
        Ids.Buildings.CargoDepotModuleFluidT2 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleFluidT2));
        Ids.Buildings.CargoDepotModuleFluidT3 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleFluidT3));
        Ids.Buildings.CargoDepotModuleLooseT1 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleLooseT1));
        Ids.Buildings.CargoDepotModuleLooseT2 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleLooseT2));
        Ids.Buildings.CargoDepotModuleLooseT3 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleLooseT3));
        Ids.Buildings.CargoDepotModuleUnitT1 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleUnitT1));
        Ids.Buildings.CargoDepotModuleUnitT2 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleUnitT2));
        Ids.Buildings.CargoDepotModuleUnitT3 = new CargoDepotModuleProto.ID(nameof (CargoDepotModuleUnitT3));
        Ids.Buildings.StorageFluid = new StaticEntityProto.ID(nameof (StorageFluid));
        Ids.Buildings.StorageFluidT2 = new StaticEntityProto.ID(nameof (StorageFluidT2));
        Ids.Buildings.StorageFluidT3 = new StaticEntityProto.ID(nameof (StorageFluidT3));
        Ids.Buildings.StorageFluidT4 = new StaticEntityProto.ID(nameof (StorageFluidT4));
        Ids.Buildings.StorageLoose = new StaticEntityProto.ID(nameof (StorageLoose));
        Ids.Buildings.StorageLooseT2 = new StaticEntityProto.ID(nameof (StorageLooseT2));
        Ids.Buildings.StorageLooseT3 = new StaticEntityProto.ID(nameof (StorageLooseT3));
        Ids.Buildings.StorageLooseT4 = new StaticEntityProto.ID(nameof (StorageLooseT4));
        Ids.Buildings.StorageUnit = new StaticEntityProto.ID(nameof (StorageUnit));
        Ids.Buildings.StorageUnitT2 = new StaticEntityProto.ID(nameof (StorageUnitT2));
        Ids.Buildings.StorageUnitT3 = new StaticEntityProto.ID(nameof (StorageUnitT3));
        Ids.Buildings.StorageUnitT4 = new StaticEntityProto.ID(nameof (StorageUnitT4));
        Ids.Buildings.NuclearWasteStorage = new StaticEntityProto.ID(nameof (NuclearWasteStorage));
        Ids.Buildings.ThermalStorage = new StaticEntityProto.ID(nameof (ThermalStorage));
        Ids.Buildings.TradeDock = new StaticEntityProto.ID(nameof (TradeDock));
        Ids.Buildings.Housing = new StaticEntityProto.ID(nameof (Housing));
        Ids.Buildings.HousingT2 = new StaticEntityProto.ID(nameof (HousingT2));
        Ids.Buildings.HousingT3 = new StaticEntityProto.ID(nameof (HousingT3));
        Ids.Buildings.SettlementLandfillModule = new StaticEntityProto.ID(nameof (SettlementLandfillModule));
        Ids.Buildings.SettlementRecyclablesModule = new StaticEntityProto.ID(nameof (SettlementRecyclablesModule));
        Ids.Buildings.SettlementBiomassModule = new StaticEntityProto.ID(nameof (SettlementBiomassModule));
        Ids.Buildings.SettlementWaterModule = new StaticEntityProto.ID(nameof (SettlementWaterModule));
        Ids.Buildings.SettlementFoodModule = new StaticEntityProto.ID(nameof (SettlementFoodModule));
        Ids.Buildings.SettlementFoodModuleT2 = new StaticEntityProto.ID(nameof (SettlementFoodModuleT2));
        Ids.Buildings.SettlementPowerModule = new StaticEntityProto.ID(nameof (SettlementPowerModule));
        Ids.Buildings.SettlementHouseholdGoodsModule = new StaticEntityProto.ID(nameof (SettlementHouseholdGoodsModule));
        Ids.Buildings.SettlementHouseholdAppliancesModule = new StaticEntityProto.ID(nameof (SettlementHouseholdAppliancesModule));
        Ids.Buildings.SettlementConsumerElectronicsModule = new StaticEntityProto.ID(nameof (SettlementConsumerElectronicsModule));
        Ids.Buildings.SettlementPillar = new StaticEntityProto.ID(nameof (SettlementPillar));
        Ids.Buildings.SettlementFountain = new StaticEntityProto.ID(nameof (SettlementFountain));
        Ids.Buildings.SettlementSquare1 = new StaticEntityProto.ID(nameof (SettlementSquare1));
        Ids.Buildings.SettlementSquare2 = new StaticEntityProto.ID(nameof (SettlementSquare2));
        Ids.Buildings.Clinic = new StaticEntityProto.ID("Hospital");
        Ids.Buildings.WasteSortingPlant = new StaticEntityProto.ID(nameof (WasteSortingPlant));
        Ids.Buildings.OreSortingPlantT1 = new StaticEntityProto.ID(nameof (OreSortingPlantT1));
        Ids.Buildings.Shipyard = new StaticEntityProto.ID(nameof (Shipyard));
        Ids.Buildings.Shipyard2 = new StaticEntityProto.ID(nameof (Shipyard2));
        Ids.Buildings.VehicleRamp = new StaticEntityProto.ID(nameof (VehicleRamp));
        Ids.Buildings.VehicleRamp2 = new StaticEntityProto.ID(nameof (VehicleRamp2));
        Ids.Buildings.VehicleRamp3 = new StaticEntityProto.ID(nameof (VehicleRamp3));
        Ids.Buildings.Ruins = new StaticEntityProto.ID(nameof (Ruins));
        Ids.Buildings.UniversalProductsSource = new StaticEntityProto.ID(nameof (UniversalProductsSource));
        Ids.Buildings.UniversalProductsSink = new StaticEntityProto.ID(nameof (UniversalProductsSink));
        Ids.Buildings.NuclearReactor = new StaticEntityProto.ID(nameof (NuclearReactor));
        Ids.Buildings.NuclearReactorT2 = new StaticEntityProto.ID(nameof (NuclearReactorT2));
        Ids.Buildings.FastBreederReactor = new StaticEntityProto.ID(nameof (FastBreederReactor));
        Ids.Buildings.RocketAssemblyDepot = new StaticEntityProto.ID(nameof (RocketAssemblyDepot));
        Ids.Buildings.RocketLaunchPad = new StaticEntityProto.ID(nameof (RocketLaunchPad));
        Ids.Buildings.RetainingWallStraight1 = new StaticEntityProto.ID(nameof (RetainingWallStraight1));
        Ids.Buildings.RetainingWallStraight4 = new StaticEntityProto.ID(nameof (RetainingWallStraight4));
        Ids.Buildings.RetainingWallCorner = new StaticEntityProto.ID(nameof (RetainingWallCorner));
        Ids.Buildings.RetainingWallCross = new StaticEntityProto.ID(nameof (RetainingWallCross));
        Ids.Buildings.RetainingWallTee = new StaticEntityProto.ID(nameof (RetainingWallTee));
        Ids.Buildings.BarrierStraight1 = new StaticEntityProto.ID(nameof (BarrierStraight1));
        Ids.Buildings.BarrierCorner = new StaticEntityProto.ID(nameof (BarrierCorner));
        Ids.Buildings.BarrierCross = new StaticEntityProto.ID(nameof (BarrierCross));
        Ids.Buildings.BarrierTee = new StaticEntityProto.ID(nameof (BarrierTee));
        Ids.Buildings.BarrierEnding = new StaticEntityProto.ID("BarrierEnd");
        Ids.Buildings.StatueOfMaintenance = new StaticEntityProto.ID(nameof (StatueOfMaintenance));
        Ids.Buildings.StatueOfMaintenanceGolden = new StaticEntityProto.ID(nameof (StatueOfMaintenanceGolden));
        Ids.Buildings.TombOfCaptainsStage1 = new StaticEntityProto.ID(nameof (TombOfCaptainsStage1));
        Ids.Buildings.TombOfCaptainsStage2 = new StaticEntityProto.ID(nameof (TombOfCaptainsStage2));
        Ids.Buildings.TombOfCaptainsStage3 = new StaticEntityProto.ID(nameof (TombOfCaptainsStage3));
        Ids.Buildings.TombOfCaptainsStage4 = new StaticEntityProto.ID(nameof (TombOfCaptainsStage4));
        Ids.Buildings.TombOfCaptainsStage5 = new StaticEntityProto.ID(nameof (TombOfCaptainsStage5));
        Ids.Buildings.TombOfCaptainsStageFinal = new StaticEntityProto.ID(nameof (TombOfCaptainsStageFinal));
      }
    }

    public static class CellSurfaces
    {
      public static readonly MapCellSurfaceGeneratorProto.ID NoSurface;
      public static readonly MapCellSurfaceGeneratorProto.ID Grass;
      public static readonly MapCellSurfaceGeneratorProto.ID GrassAndSand;
      public static readonly MapCellSurfaceGeneratorProto.ID Rock;
      public static readonly MapCellSurfaceGeneratorProto.ID Sand;

      static CellSurfaces()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.CellSurfaces.NoSurface = new MapCellSurfaceGeneratorProto.ID("NoCellSurface");
        Ids.CellSurfaces.Grass = new MapCellSurfaceGeneratorProto.ID("GrassCellSurface");
        Ids.CellSurfaces.GrassAndSand = new MapCellSurfaceGeneratorProto.ID("GrassAndSandCellSurface");
        Ids.CellSurfaces.Rock = new MapCellSurfaceGeneratorProto.ID("RockCellSurface");
        Ids.CellSurfaces.Sand = new MapCellSurfaceGeneratorProto.ID("SandCellSurface");
      }
    }

    [OnlyForSaveCompatibility(null)]
    public static class IoPortShapes
    {
      public static readonly IoPortShapeProto.ID FlatConveyor;
      public static readonly IoPortShapeProto.ID LooseMaterialConveyor;
      public static readonly IoPortShapeProto.ID Pipe;
      public static readonly IoPortShapeProto.ID MoltenMetalChannel;
      public static readonly IoPortShapeProto.ID Shaft;

      static IoPortShapes()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.IoPortShapes.FlatConveyor = new IoPortShapeProto.ID("IoPortShape_FlatConveyor");
        Ids.IoPortShapes.LooseMaterialConveyor = new IoPortShapeProto.ID("IoPortShape_LooseMaterialConveyor");
        Ids.IoPortShapes.Pipe = new IoPortShapeProto.ID("IoPortShape_Pipe");
        Ids.IoPortShapes.MoltenMetalChannel = new IoPortShapeProto.ID("IoPortShape_MoltenMetalChannel");
        Ids.IoPortShapes.Shaft = IdsCore.Transports.ShaftPortShape;
      }
    }

    public static class Transports
    {
      public const string FLAT_CONVEYOR_T1 = "FlatConveyorT1";
      public static readonly StaticEntityProto.ID FlatConveyor;
      public const string FLAT_CONVEYOR_T2 = "FlatConveyorT2";
      public static readonly StaticEntityProto.ID FlatConveyorT2;
      public const string FLAT_CONVEYOR_T3 = "FlatConveyorT3";
      public static readonly StaticEntityProto.ID FlatConveyorT3;
      public static readonly StaticEntityProto.ID FlatConveyorSorter;
      public static readonly StaticEntityProto.ID LooseMaterialConveyor;
      public static readonly StaticEntityProto.ID LooseMaterialConveyorT2;
      public static readonly StaticEntityProto.ID LooseMaterialConveyorT3;
      public static readonly StaticEntityProto.ID LooseConveyorSorter;
      public const string PIPE_T1 = "PipeT1";
      public static readonly StaticEntityProto.ID PipeT1;
      public const string PIPE_T2 = "PipeT2";
      public static readonly StaticEntityProto.ID PipeT2;
      public const string PIPE_T3 = "PipeT3";
      public static readonly StaticEntityProto.ID PipeT3;
      public static readonly StaticEntityProto.ID MoltenMetalChannel;
      public static readonly StaticEntityProto.ID Shaft;
      public static readonly StaticEntityProto.ID Stacker;

      static Transports()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Transports.FlatConveyor = new StaticEntityProto.ID("FlatConveyorT1");
        Ids.Transports.FlatConveyorT2 = new StaticEntityProto.ID(nameof (FlatConveyorT2));
        Ids.Transports.FlatConveyorT3 = new StaticEntityProto.ID(nameof (FlatConveyorT3));
        Ids.Transports.FlatConveyorSorter = new StaticEntityProto.ID(nameof (FlatConveyorSorter));
        Ids.Transports.LooseMaterialConveyor = new StaticEntityProto.ID(nameof (LooseMaterialConveyor));
        Ids.Transports.LooseMaterialConveyorT2 = new StaticEntityProto.ID(nameof (LooseMaterialConveyorT2));
        Ids.Transports.LooseMaterialConveyorT3 = new StaticEntityProto.ID(nameof (LooseMaterialConveyorT3));
        Ids.Transports.LooseConveyorSorter = new StaticEntityProto.ID(nameof (LooseConveyorSorter));
        Ids.Transports.PipeT1 = new StaticEntityProto.ID(nameof (PipeT1));
        Ids.Transports.PipeT2 = new StaticEntityProto.ID(nameof (PipeT2));
        Ids.Transports.PipeT3 = new StaticEntityProto.ID(nameof (PipeT3));
        Ids.Transports.MoltenMetalChannel = new StaticEntityProto.ID(nameof (MoltenMetalChannel));
        Ids.Transports.Shaft = new StaticEntityProto.ID(nameof (Shaft));
        Ids.Transports.Stacker = new StaticEntityProto.ID(nameof (Stacker));
      }
    }

    public static class Crops
    {
      public static readonly Proto.ID NoCrop;
      public static readonly Proto.ID Wheat;
      public static readonly Proto.ID Corn;
      public static readonly Proto.ID Potato;
      public static readonly Proto.ID GreenManure;
      public static readonly Proto.ID Soybeans;
      public static readonly Proto.ID Vegetables;
      public static readonly Proto.ID Fruits;
      public static readonly Proto.ID Canola;
      public static readonly Proto.ID SugarCane;
      public static readonly Proto.ID Poppy;
      public static readonly Proto.ID TreeSapling;
      public static readonly Proto.ID Flowers;

      static Crops()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Crops.NoCrop = new Proto.ID("Crop_NoCrop");
        Ids.Crops.Wheat = new Proto.ID("Crop_Wheat");
        Ids.Crops.Corn = new Proto.ID("Crop_Corn");
        Ids.Crops.Potato = new Proto.ID("Crop_Potato");
        Ids.Crops.GreenManure = new Proto.ID("Crop_GreenManure");
        Ids.Crops.Soybeans = new Proto.ID("Crop_Soybeans");
        Ids.Crops.Vegetables = new Proto.ID("Crop_Vegetables");
        Ids.Crops.Fruits = new Proto.ID("Crop_Fruits");
        Ids.Crops.Canola = new Proto.ID("Crop_Canola");
        Ids.Crops.SugarCane = new Proto.ID("Crop_SugarCane");
        Ids.Crops.Poppy = new Proto.ID("Crop_Poppy");
        Ids.Crops.TreeSapling = new Proto.ID("Crop_TreeSapling");
        Ids.Crops.Flowers = new Proto.ID("Crop_Flowers");
      }
    }

    public static class FoodCategories
    {
      public static readonly Proto.ID Carbs;
      public static readonly Proto.ID Protein;
      public static readonly Proto.ID Vitamins;
      public static readonly Proto.ID Treats;

      static FoodCategories()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.FoodCategories.Carbs = new Proto.ID("FoodCategory_Carbs");
        Ids.FoodCategories.Protein = new Proto.ID("FoodCategory_Protein");
        Ids.FoodCategories.Vitamins = new Proto.ID("FoodCategory_Vitamins");
        Ids.FoodCategories.Treats = new Proto.ID("FoodCategory_Treats");
      }
    }

    public static class FoodTypes
    {
      public static readonly Proto.ID Potato;
      public static readonly Proto.ID Bread;
      public static readonly Proto.ID Corn;
      public static readonly Proto.ID Meat;
      public static readonly Proto.ID Eggs;
      public static readonly Proto.ID Tofu;
      public static readonly Proto.ID Sausage;
      public static readonly Proto.ID Fruits;
      public static readonly Proto.ID Vegetables;
      public static readonly Proto.ID Cake;
      public static readonly Proto.ID Snack;

      static FoodTypes()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.FoodTypes.Potato = new Proto.ID("Food_Potato");
        Ids.FoodTypes.Bread = new Proto.ID("Food_Bread");
        Ids.FoodTypes.Corn = new Proto.ID("Food_Corn");
        Ids.FoodTypes.Meat = new Proto.ID("Food_Meat");
        Ids.FoodTypes.Eggs = new Proto.ID("Food_Eggs");
        Ids.FoodTypes.Tofu = new Proto.ID("Food_Tofu");
        Ids.FoodTypes.Sausage = new Proto.ID("Food_Sausage");
        Ids.FoodTypes.Fruits = new Proto.ID("Food_Fruits");
        Ids.FoodTypes.Vegetables = new Proto.ID("Food_Vegetables");
        Ids.FoodTypes.Cake = new Proto.ID("Food_Cake");
        Ids.FoodTypes.Snack = new Proto.ID("Food_Snack");
      }
    }

    public static class MedicalSupplies
    {
      public static readonly Proto.ID SuppliesT1;
      public static readonly Proto.ID SuppliesT2;

      static MedicalSupplies()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.MedicalSupplies.SuppliesT1 = new Proto.ID(nameof (SuppliesT1));
        Ids.MedicalSupplies.SuppliesT2 = new Proto.ID(nameof (SuppliesT2));
      }
    }

    public static class DataCenters
    {
      public static readonly DataCenterProto.ID Mainframe;
      public static readonly DataCenterProto.ID DataCenter;
      public static readonly Proto.ID BasicServerRack;

      static DataCenters()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.DataCenters.Mainframe = new DataCenterProto.ID(nameof (Mainframe));
        Ids.DataCenters.DataCenter = new DataCenterProto.ID(nameof (DataCenter));
        Ids.DataCenters.BasicServerRack = (Proto.ID) new DataCenterProto.ID(nameof (BasicServerRack));
      }
    }

    public static class Edicts
    {
      public static readonly Proto.ID GrowthPause;
      public static readonly Proto.ID PopsBoostT1;
      public static readonly Proto.ID PopsBoostT2;
      public static readonly Proto.ID PopsBoostT3;
      public static readonly Proto.ID PopsEviction;
      public static readonly Proto.ID PopsQuarantine;
      public static readonly Proto.ID FuelReduction;
      public static readonly Proto.ID FuelReductionT2;
      public static readonly Proto.ID ShipFuelReduction;
      public static readonly Proto.ID TruckCapacityIncrease;
      public static readonly Proto.ID TruckCapacityIncreaseT2;
      public static readonly Proto.ID FoodConsumptionIncrease;
      public static readonly Proto.ID FoodConsumptionIncreaseT2;
      public static readonly Proto.ID FoodConsumptionReduction;
      public static readonly Proto.ID FoodConsumptionReductionT2;
      public static readonly Proto.ID HouseholdGoodsConsumptionIncrease;
      public static readonly Proto.ID HouseholdGoodsConsumptionIncreaseT2;
      public static readonly Proto.ID HouseholdGoodsConsumptionIncreaseT3;
      public static readonly Proto.ID HouseholdAppliancesConsumptionIncrease;
      public static readonly Proto.ID HouseholdAppliancesConsumptionIncreaseT2;
      public static readonly Proto.ID HouseholdAppliancesConsumptionIncreaseT3;
      public static readonly Proto.ID ConsumerElectronicsConsumptionIncrease;
      public static readonly Proto.ID ConsumerElectronicsConsumptionIncreaseT2;
      public static readonly Proto.ID ConsumerElectronicsConsumptionIncreaseT3;
      public static readonly Proto.ID MaintenanceReduction;
      public static readonly Proto.ID MaintenanceReductionT2;
      public static readonly Proto.ID RecyclingIncrease;
      public static readonly Proto.ID RecyclingIncreaseT2;
      public static readonly Proto.ID RecyclingIncreaseT3;
      public static readonly Proto.ID RecyclingIncreaseT4;
      public static readonly Proto.ID FarmYieldIncrease;
      public static readonly Proto.ID FarmYieldIncreaseT2;
      public static readonly Proto.ID FarmYieldIncreaseT3;
      public static readonly Proto.ID WaterConsumptionReduction;
      public static readonly Proto.ID WaterConsumptionReductionT2;
      public static readonly Proto.ID WaterConsumptionReductionT3;
      public static readonly Proto.ID HealthBonus;
      public static readonly Proto.ID HealthBonusT2;
      public static readonly Proto.ID SolarPowerIncrease;
      public static readonly Proto.ID SolarPowerIncreaseT2;
      public static readonly Proto.ID SolarPowerIncreaseT3;

      private static Proto.ID CreateEdictID(string id) => new Proto.ID("Edict_" + id);

      static Edicts()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Edicts.GrowthPause = Ids.Edicts.CreateEdictID(nameof (GrowthPause));
        Ids.Edicts.PopsBoostT1 = Ids.Edicts.CreateEdictID(nameof (PopsBoostT1));
        Ids.Edicts.PopsBoostT2 = Ids.Edicts.CreateEdictID(nameof (PopsBoostT2));
        Ids.Edicts.PopsBoostT3 = Ids.Edicts.CreateEdictID(nameof (PopsBoostT3));
        Ids.Edicts.PopsEviction = Ids.Edicts.CreateEdictID(nameof (PopsEviction));
        Ids.Edicts.PopsQuarantine = Ids.Edicts.CreateEdictID(nameof (PopsQuarantine));
        Ids.Edicts.FuelReduction = Ids.Edicts.CreateEdictID(nameof (FuelReduction));
        Ids.Edicts.FuelReductionT2 = Ids.Edicts.CreateEdictID(nameof (FuelReductionT2));
        Ids.Edicts.ShipFuelReduction = Ids.Edicts.CreateEdictID(nameof (ShipFuelReduction));
        Ids.Edicts.TruckCapacityIncrease = Ids.Edicts.CreateEdictID(nameof (TruckCapacityIncrease));
        Ids.Edicts.TruckCapacityIncreaseT2 = Ids.Edicts.CreateEdictID(nameof (TruckCapacityIncreaseT2));
        Ids.Edicts.FoodConsumptionIncrease = Ids.Edicts.CreateEdictID(nameof (FoodConsumptionIncrease));
        Ids.Edicts.FoodConsumptionIncreaseT2 = Ids.Edicts.CreateEdictID(nameof (FoodConsumptionIncreaseT2));
        Ids.Edicts.FoodConsumptionReduction = Ids.Edicts.CreateEdictID(nameof (FoodConsumptionReduction));
        Ids.Edicts.FoodConsumptionReductionT2 = Ids.Edicts.CreateEdictID(nameof (FoodConsumptionReductionT2));
        Ids.Edicts.HouseholdGoodsConsumptionIncrease = Ids.Edicts.CreateEdictID(nameof (HouseholdGoodsConsumptionIncrease));
        Ids.Edicts.HouseholdGoodsConsumptionIncreaseT2 = Ids.Edicts.CreateEdictID(nameof (HouseholdGoodsConsumptionIncreaseT2));
        Ids.Edicts.HouseholdGoodsConsumptionIncreaseT3 = Ids.Edicts.CreateEdictID(nameof (HouseholdGoodsConsumptionIncreaseT3));
        Ids.Edicts.HouseholdAppliancesConsumptionIncrease = Ids.Edicts.CreateEdictID(nameof (HouseholdAppliancesConsumptionIncrease));
        Ids.Edicts.HouseholdAppliancesConsumptionIncreaseT2 = Ids.Edicts.CreateEdictID(nameof (HouseholdAppliancesConsumptionIncreaseT2));
        Ids.Edicts.HouseholdAppliancesConsumptionIncreaseT3 = Ids.Edicts.CreateEdictID(nameof (HouseholdAppliancesConsumptionIncreaseT3));
        Ids.Edicts.ConsumerElectronicsConsumptionIncrease = Ids.Edicts.CreateEdictID(nameof (ConsumerElectronicsConsumptionIncrease));
        Ids.Edicts.ConsumerElectronicsConsumptionIncreaseT2 = Ids.Edicts.CreateEdictID(nameof (ConsumerElectronicsConsumptionIncreaseT2));
        Ids.Edicts.ConsumerElectronicsConsumptionIncreaseT3 = Ids.Edicts.CreateEdictID(nameof (ConsumerElectronicsConsumptionIncreaseT3));
        Ids.Edicts.MaintenanceReduction = Ids.Edicts.CreateEdictID(nameof (MaintenanceReduction));
        Ids.Edicts.MaintenanceReductionT2 = Ids.Edicts.CreateEdictID(nameof (MaintenanceReductionT2));
        Ids.Edicts.RecyclingIncrease = Ids.Edicts.CreateEdictID(nameof (RecyclingIncrease));
        Ids.Edicts.RecyclingIncreaseT2 = Ids.Edicts.CreateEdictID(nameof (RecyclingIncreaseT2));
        Ids.Edicts.RecyclingIncreaseT3 = Ids.Edicts.CreateEdictID(nameof (RecyclingIncreaseT3));
        Ids.Edicts.RecyclingIncreaseT4 = Ids.Edicts.CreateEdictID(nameof (RecyclingIncreaseT4));
        Ids.Edicts.FarmYieldIncrease = Ids.Edicts.CreateEdictID(nameof (FarmYieldIncrease));
        Ids.Edicts.FarmYieldIncreaseT2 = Ids.Edicts.CreateEdictID(nameof (FarmYieldIncreaseT2));
        Ids.Edicts.FarmYieldIncreaseT3 = Ids.Edicts.CreateEdictID(nameof (FarmYieldIncreaseT3));
        Ids.Edicts.WaterConsumptionReduction = Ids.Edicts.CreateEdictID(nameof (WaterConsumptionReduction));
        Ids.Edicts.WaterConsumptionReductionT2 = Ids.Edicts.CreateEdictID(nameof (WaterConsumptionReductionT2));
        Ids.Edicts.WaterConsumptionReductionT3 = Ids.Edicts.CreateEdictID(nameof (WaterConsumptionReductionT3));
        Ids.Edicts.HealthBonus = Ids.Edicts.CreateEdictID(nameof (HealthBonus));
        Ids.Edicts.HealthBonusT2 = Ids.Edicts.CreateEdictID(nameof (HealthBonusT2));
        Ids.Edicts.SolarPowerIncrease = Ids.Edicts.CreateEdictID(nameof (SolarPowerIncrease));
        Ids.Edicts.SolarPowerIncreaseT2 = Ids.Edicts.CreateEdictID(nameof (SolarPowerIncreaseT2));
        Ids.Edicts.SolarPowerIncreaseT3 = Ids.Edicts.CreateEdictID(nameof (SolarPowerIncreaseT3));
      }
    }

    public static class EdictCategories
    {
      public static readonly Proto.ID Population;
      public static readonly Proto.ID Industry;

      static EdictCategories()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.EdictCategories.Population = new Proto.ID("EdictCategory_Population");
        Ids.EdictCategories.Industry = new Proto.ID("EdictCategory_Industry");
      }
    }

    public static class Fleet
    {
      public static class Hulls
      {
        public static readonly FleetEntityHullProto.ID Scout;
        public static readonly FleetEntityHullProto.ID Patrol;
        public static readonly FleetEntityHullProto.ID Cruiser;
        public static readonly FleetEntityHullProto.ID Battleship;

        static Hulls()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Fleet.Hulls.Scout = new FleetEntityHullProto.ID("ScoutHull");
          Ids.Fleet.Hulls.Patrol = new FleetEntityHullProto.ID("PatrolHull");
          Ids.Fleet.Hulls.Cruiser = new FleetEntityHullProto.ID("CruiserHull");
          Ids.Fleet.Hulls.Battleship = new FleetEntityHullProto.ID("BattleshipHull");
        }
      }

      public static class Engines
      {
        public static readonly FleetWeaponProto.ID EngineT1;
        public static readonly FleetWeaponProto.ID EngineT2;
        public static readonly FleetWeaponProto.ID EngineT3;

        static Engines()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Fleet.Engines.EngineT1 = new FleetWeaponProto.ID(nameof (EngineT1));
          Ids.Fleet.Engines.EngineT2 = new FleetWeaponProto.ID(nameof (EngineT2));
          Ids.Fleet.Engines.EngineT3 = new FleetWeaponProto.ID(nameof (EngineT3));
        }
      }

      public static class Weapons
      {
        public static readonly FleetWeaponProto.ID Gun0;
        public static readonly FleetWeaponProto.ID Gun1;
        public static readonly FleetWeaponProto.ID Gun2;
        public static readonly FleetWeaponProto.ID Gun3;
        public static readonly FleetWeaponProto.ID Gun1Rear;
        public static readonly FleetWeaponProto.ID Gun2Rear;
        public static readonly FleetWeaponProto.ID Gun3Rear;

        static Weapons()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Fleet.Weapons.Gun0 = new FleetWeaponProto.ID(nameof (Gun0));
          Ids.Fleet.Weapons.Gun1 = new FleetWeaponProto.ID(nameof (Gun1));
          Ids.Fleet.Weapons.Gun2 = new FleetWeaponProto.ID(nameof (Gun2));
          Ids.Fleet.Weapons.Gun3 = new FleetWeaponProto.ID(nameof (Gun3));
          Ids.Fleet.Weapons.Gun1Rear = new FleetWeaponProto.ID(nameof (Gun1Rear));
          Ids.Fleet.Weapons.Gun2Rear = new FleetWeaponProto.ID(nameof (Gun2Rear));
          Ids.Fleet.Weapons.Gun3Rear = new FleetWeaponProto.ID(nameof (Gun3Rear));
        }
      }

      public static class Armor
      {
        public static readonly FleetWeaponProto.ID ArmorT1;
        public static readonly FleetWeaponProto.ID ArmorT2;

        static Armor()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Fleet.Armor.ArmorT1 = new FleetWeaponProto.ID(nameof (ArmorT1));
          Ids.Fleet.Armor.ArmorT2 = new FleetWeaponProto.ID(nameof (ArmorT2));
        }
      }

      public static class Bridges
      {
        public static readonly FleetWeaponProto.ID BridgeT1;
        public static readonly FleetWeaponProto.ID BridgeT2;
        public static readonly FleetWeaponProto.ID BridgeT3;

        static Bridges()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Fleet.Bridges.BridgeT1 = new FleetWeaponProto.ID(nameof (BridgeT1));
          Ids.Fleet.Bridges.BridgeT2 = new FleetWeaponProto.ID(nameof (BridgeT2));
          Ids.Fleet.Bridges.BridgeT3 = new FleetWeaponProto.ID(nameof (BridgeT3));
        }
      }

      public static class FuelTanks
      {
        public static readonly FleetWeaponProto.ID FuelTankT1;

        static FuelTanks()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Fleet.FuelTanks.FuelTankT1 = new FleetWeaponProto.ID(nameof (FuelTankT1));
        }
      }
    }

    public static class Islands
    {
      public static readonly Proto.ID HomeIsland;

      static Islands()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Islands.HomeIsland = new Proto.ID("Islands_HomeIsland");
      }
    }

    public static class Machines
    {
      public static readonly MachineProto.ID BasicDieselDistiller;
      public static readonly MachineProto.ID OilPump;
      public static readonly MachineProto.ID Crusher;
      public static readonly MachineProto.ID CrusherLarge;
      public static readonly MachineProto.ID SmeltingFurnaceT2;
      public static readonly MachineProto.ID SmeltingFurnaceT1;
      public static readonly MachineProto.ID Caster;
      public static readonly MachineProto.ID CasterT2;
      public static readonly MachineProto.ID CasterCooled;
      public static readonly MachineProto.ID CasterCooledT2;
      public static readonly MachineProto.ID OxygenFurnace;
      public static readonly MachineProto.ID OxygenFurnaceT2;
      public static readonly MachineProto.ID ExhaustScrubber;
      public static readonly MachineProto.ID Electrolyzer;
      public static readonly MachineProto.ID ElectrolyzerT2;
      public static readonly MachineProto.ID AirSeparator;
      public static readonly MachineProto.ID CopperElectrolysis;
      public static readonly MachineProto.ID BricksMaker;
      public static readonly MachineProto.ID RotaryKiln;
      public static readonly MachineProto.ID RotaryKilnGas;
      public static readonly MachineProto.ID ConcreteMixer;
      public static readonly MachineProto.ID ConcreteMixerT2;
      public static readonly MachineProto.ID ConcreteMixerT3;
      public static readonly MachineProto.ID IndustrialMixer;
      public static readonly MachineProto.ID IndustrialMixerT2;
      public static readonly MachineProto.ID NuclearFuelReprocessingPlant;
      public static readonly MachineProto.ID FoodMill;
      public static readonly MachineProto.ID BakingUnit;
      public static readonly MachineProto.ID FoodProcessor;
      public static readonly MachineProto.ID SolidBurner;
      public static readonly MachineProto.ID Compactor;
      public static readonly MachineProto.ID Shredder;
      public static readonly MachineProto.ID IncinerationPlant;
      public static readonly MachineProto.ID WaterChiller;
      public static readonly MachineProto.ID ThermalDesalinator;
      [OnlyForSaveCompatibility("Rename to just Assembly")]
      public static readonly MachineProto.ID AssemblyManual;
      public static readonly MachineProto.ID AssemblyElectrified;
      public static readonly MachineProto.ID AssemblyElectrifiedT2;
      public static readonly MachineProto.ID AssemblyRoboticT1;
      public static readonly MachineProto.ID AssemblyRoboticT2;
      public static readonly MachineProto.ID MicrochipMachine;
      public static readonly MachineProto.ID MicrochipMachineT2;
      public static readonly MachineProto.ID WasteDump;
      public static readonly MachineProto.ID LandWaterPump;
      public static readonly MachineProto.ID GasInjectionPump;
      public static readonly MachineProto.ID OceanWaterPumpT1;
      public static readonly MachineProto.ID OceanWaterPumpLarge;
      public static readonly MachineProto.ID ChemicalPlant;
      public static readonly MachineProto.ID ChemicalPlant2;
      public static readonly MachineProto.ID WaterTreatmentPlant;
      public static readonly MachineProto.ID EvaporationPond;
      public static readonly MachineProto.ID EvaporationPondHeated;
      public static readonly MachineProto.ID AnaerobicDigester;
      public static readonly MachineProto.ID BoilerCoal;
      public static readonly MachineProto.ID BoilerGas;
      public static readonly MachineProto.ID BoilerElectric;
      public static readonly MachineProto.ID TurbineSuperPress;
      public static readonly MachineProto.ID TurbineHighPress;
      public static readonly MachineProto.ID TurbineHighPressT2;
      public static readonly MachineProto.ID TurbineLowPress;
      public static readonly MachineProto.ID TurbineLowPressT2;
      public static readonly MachineProto.ID Flywheel;
      public static readonly MachineProto.ID PowerGeneratorT1;
      public static readonly MachineProto.ID PowerGeneratorT2;
      public static readonly MachineProto.ID DieselGenerator;
      public static readonly MachineProto.ID DieselGeneratorT2;
      public static readonly MachineProto.ID SmokeStack;
      public static readonly MachineProto.ID SmokeStackLarge;
      public static readonly MachineProto.ID CoolingTowerT1;
      public static readonly MachineProto.ID CoolingTowerT2;
      public static readonly MachineProto.ID GlassMakerT1;
      public static readonly MachineProto.ID GlassMakerT2;
      public static readonly MachineProto.ID FermentationTank;
      public static readonly MachineProto.ID UraniumEnrichmentPlant;
      public static readonly MachineProto.ID ArcFurnace;
      public static readonly MachineProto.ID ArcFurnace2;
      public static readonly MachineProto.ID SiliconReactor;
      public static readonly MachineProto.ID SiliconCrystallizer;
      public static readonly MachineProto.ID CharcoalMaker;
      public static readonly MachineProto.ID SettlingTank;
      public static readonly MachineProto.ID GoldFurnace;
      public static readonly MachineProto.ID SolarPanel;
      public static readonly MachineProto.ID SolarPanelMono;
      public static readonly MachineProto.ID DistillationTowerT1;
      public static readonly MachineProto.ID DistillationTowerT2;
      public static readonly MachineProto.ID DistillationTowerT3;
      public static readonly MachineProto.ID VacuumDistillationTower;
      public static readonly MachineProto.ID HydroCrackerT1;
      public static readonly MachineProto.ID Flare;
      public static readonly MachineProto.ID HydrogenReformer;
      public static readonly MachineProto.ID SourWaterStripper;
      public static readonly MachineProto.ID PolymerizationPlant;

      public static MachineProto.ID CreateId(string id) => new MachineProto.ID("Machine_" + id);

      static Machines()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Machines.BasicDieselDistiller = new MachineProto.ID(nameof (BasicDieselDistiller));
        Ids.Machines.OilPump = new MachineProto.ID(nameof (OilPump));
        Ids.Machines.Crusher = new MachineProto.ID(nameof (Crusher));
        Ids.Machines.CrusherLarge = new MachineProto.ID(nameof (CrusherLarge));
        Ids.Machines.SmeltingFurnaceT2 = new MachineProto.ID(nameof (SmeltingFurnaceT2));
        Ids.Machines.SmeltingFurnaceT1 = new MachineProto.ID(nameof (SmeltingFurnaceT1));
        Ids.Machines.Caster = new MachineProto.ID(nameof (Caster));
        Ids.Machines.CasterT2 = new MachineProto.ID(nameof (CasterT2));
        Ids.Machines.CasterCooled = new MachineProto.ID(nameof (CasterCooled));
        Ids.Machines.CasterCooledT2 = new MachineProto.ID(nameof (CasterCooledT2));
        Ids.Machines.OxygenFurnace = new MachineProto.ID(nameof (OxygenFurnace));
        Ids.Machines.OxygenFurnaceT2 = new MachineProto.ID(nameof (OxygenFurnaceT2));
        Ids.Machines.ExhaustScrubber = new MachineProto.ID(nameof (ExhaustScrubber));
        Ids.Machines.Electrolyzer = new MachineProto.ID(nameof (Electrolyzer));
        Ids.Machines.ElectrolyzerT2 = new MachineProto.ID(nameof (ElectrolyzerT2));
        Ids.Machines.AirSeparator = new MachineProto.ID(nameof (AirSeparator));
        Ids.Machines.CopperElectrolysis = new MachineProto.ID(nameof (CopperElectrolysis));
        Ids.Machines.BricksMaker = new MachineProto.ID(nameof (BricksMaker));
        Ids.Machines.RotaryKiln = new MachineProto.ID(nameof (RotaryKiln));
        Ids.Machines.RotaryKilnGas = new MachineProto.ID(nameof (RotaryKilnGas));
        Ids.Machines.ConcreteMixer = new MachineProto.ID(nameof (ConcreteMixer));
        Ids.Machines.ConcreteMixerT2 = new MachineProto.ID(nameof (ConcreteMixerT2));
        Ids.Machines.ConcreteMixerT3 = new MachineProto.ID(nameof (ConcreteMixerT3));
        Ids.Machines.IndustrialMixer = new MachineProto.ID(nameof (IndustrialMixer));
        Ids.Machines.IndustrialMixerT2 = new MachineProto.ID(nameof (IndustrialMixerT2));
        Ids.Machines.NuclearFuelReprocessingPlant = new MachineProto.ID("NuclearReprocessingPlant");
        Ids.Machines.FoodMill = new MachineProto.ID(nameof (FoodMill));
        Ids.Machines.BakingUnit = new MachineProto.ID(nameof (BakingUnit));
        Ids.Machines.FoodProcessor = new MachineProto.ID(nameof (FoodProcessor));
        Ids.Machines.SolidBurner = new MachineProto.ID("Burner");
        Ids.Machines.Compactor = new MachineProto.ID(nameof (Compactor));
        Ids.Machines.Shredder = new MachineProto.ID(nameof (Shredder));
        Ids.Machines.IncinerationPlant = new MachineProto.ID(nameof (IncinerationPlant));
        Ids.Machines.WaterChiller = new MachineProto.ID(nameof (WaterChiller));
        Ids.Machines.ThermalDesalinator = new MachineProto.ID(nameof (ThermalDesalinator));
        Ids.Machines.AssemblyManual = new MachineProto.ID(nameof (AssemblyManual));
        Ids.Machines.AssemblyElectrified = new MachineProto.ID(nameof (AssemblyElectrified));
        Ids.Machines.AssemblyElectrifiedT2 = new MachineProto.ID(nameof (AssemblyElectrifiedT2));
        Ids.Machines.AssemblyRoboticT1 = new MachineProto.ID(nameof (AssemblyRoboticT1));
        Ids.Machines.AssemblyRoboticT2 = new MachineProto.ID(nameof (AssemblyRoboticT2));
        Ids.Machines.MicrochipMachine = new MachineProto.ID(nameof (MicrochipMachine));
        Ids.Machines.MicrochipMachineT2 = new MachineProto.ID(nameof (MicrochipMachineT2));
        Ids.Machines.WasteDump = new MachineProto.ID(nameof (WasteDump));
        Ids.Machines.LandWaterPump = new MachineProto.ID(nameof (LandWaterPump));
        Ids.Machines.GasInjectionPump = new MachineProto.ID(nameof (GasInjectionPump));
        Ids.Machines.OceanWaterPumpT1 = new MachineProto.ID(nameof (OceanWaterPumpT1));
        Ids.Machines.OceanWaterPumpLarge = new MachineProto.ID(nameof (OceanWaterPumpLarge));
        Ids.Machines.ChemicalPlant = new MachineProto.ID(nameof (ChemicalPlant));
        Ids.Machines.ChemicalPlant2 = new MachineProto.ID(nameof (ChemicalPlant2));
        Ids.Machines.WaterTreatmentPlant = new MachineProto.ID(nameof (WaterTreatmentPlant));
        Ids.Machines.EvaporationPond = new MachineProto.ID(nameof (EvaporationPond));
        Ids.Machines.EvaporationPondHeated = new MachineProto.ID(nameof (EvaporationPondHeated));
        Ids.Machines.AnaerobicDigester = new MachineProto.ID(nameof (AnaerobicDigester));
        Ids.Machines.BoilerCoal = new MachineProto.ID(nameof (BoilerCoal));
        Ids.Machines.BoilerGas = new MachineProto.ID(nameof (BoilerGas));
        Ids.Machines.BoilerElectric = new MachineProto.ID(nameof (BoilerElectric));
        Ids.Machines.TurbineSuperPress = new MachineProto.ID(nameof (TurbineSuperPress));
        Ids.Machines.TurbineHighPress = new MachineProto.ID(nameof (TurbineHighPress));
        Ids.Machines.TurbineHighPressT2 = new MachineProto.ID(nameof (TurbineHighPressT2));
        Ids.Machines.TurbineLowPress = new MachineProto.ID(nameof (TurbineLowPress));
        Ids.Machines.TurbineLowPressT2 = new MachineProto.ID(nameof (TurbineLowPressT2));
        Ids.Machines.Flywheel = new MachineProto.ID(nameof (Flywheel));
        Ids.Machines.PowerGeneratorT1 = new MachineProto.ID(nameof (PowerGeneratorT1));
        Ids.Machines.PowerGeneratorT2 = new MachineProto.ID(nameof (PowerGeneratorT2));
        Ids.Machines.DieselGenerator = new MachineProto.ID(nameof (DieselGenerator));
        Ids.Machines.DieselGeneratorT2 = new MachineProto.ID(nameof (DieselGeneratorT2));
        Ids.Machines.SmokeStack = new MachineProto.ID(nameof (SmokeStack));
        Ids.Machines.SmokeStackLarge = new MachineProto.ID(nameof (SmokeStackLarge));
        Ids.Machines.CoolingTowerT1 = new MachineProto.ID(nameof (CoolingTowerT1));
        Ids.Machines.CoolingTowerT2 = new MachineProto.ID(nameof (CoolingTowerT2));
        Ids.Machines.GlassMakerT1 = new MachineProto.ID(nameof (GlassMakerT1));
        Ids.Machines.GlassMakerT2 = new MachineProto.ID(nameof (GlassMakerT2));
        Ids.Machines.FermentationTank = new MachineProto.ID(nameof (FermentationTank));
        Ids.Machines.UraniumEnrichmentPlant = new MachineProto.ID(nameof (UraniumEnrichmentPlant));
        Ids.Machines.ArcFurnace = new MachineProto.ID(nameof (ArcFurnace));
        Ids.Machines.ArcFurnace2 = new MachineProto.ID(nameof (ArcFurnace2));
        Ids.Machines.SiliconReactor = new MachineProto.ID(nameof (SiliconReactor));
        Ids.Machines.SiliconCrystallizer = new MachineProto.ID(nameof (SiliconCrystallizer));
        Ids.Machines.CharcoalMaker = new MachineProto.ID(nameof (CharcoalMaker));
        Ids.Machines.SettlingTank = new MachineProto.ID(nameof (SettlingTank));
        Ids.Machines.GoldFurnace = new MachineProto.ID(nameof (GoldFurnace));
        Ids.Machines.SolarPanel = new MachineProto.ID(nameof (SolarPanel));
        Ids.Machines.SolarPanelMono = new MachineProto.ID(nameof (SolarPanelMono));
        Ids.Machines.DistillationTowerT1 = new MachineProto.ID(nameof (DistillationTowerT1));
        Ids.Machines.DistillationTowerT2 = new MachineProto.ID(nameof (DistillationTowerT2));
        Ids.Machines.DistillationTowerT3 = new MachineProto.ID(nameof (DistillationTowerT3));
        Ids.Machines.VacuumDistillationTower = new MachineProto.ID(nameof (VacuumDistillationTower));
        Ids.Machines.HydroCrackerT1 = new MachineProto.ID(nameof (HydroCrackerT1));
        Ids.Machines.Flare = new MachineProto.ID(nameof (Flare));
        Ids.Machines.HydrogenReformer = new MachineProto.ID(nameof (HydrogenReformer));
        Ids.Machines.SourWaterStripper = new MachineProto.ID(nameof (SourWaterStripper));
        Ids.Machines.PolymerizationPlant = new MachineProto.ID(nameof (PolymerizationPlant));
      }
    }

    public static class Messages
    {
      public static readonly Proto.ID MessageWelcome;
      public static readonly Proto.ID MessageGameVictory;
      public static readonly Proto.ID TutorialOnWasteDumping;
      public static readonly Proto.ID TutorialOnCrisis;
      public static readonly Proto.ID TutorialOnFurnace;
      public static readonly Proto.ID TutorialOnIronOreSmelting;
      public static readonly Proto.ID TutorialOnDiesel;
      public static readonly Proto.ID TutorialOnStoragesAndTransports;
      public static readonly Proto.ID TutorialOnTrucks;
      public static readonly Proto.ID TutorialOnShipRepair;
      public static readonly Proto.ID WarningNoWorkersNoBeacon;
      public static readonly Proto.ID WarningLowMaintenanceNoDepot;
      public static readonly Proto.ID WarningLowDiesel;

      static Messages()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Messages.MessageWelcome = new Proto.ID(nameof (MessageWelcome));
        Ids.Messages.MessageGameVictory = new Proto.ID(nameof (MessageGameVictory));
        Ids.Messages.TutorialOnWasteDumping = new Proto.ID(nameof (TutorialOnWasteDumping));
        Ids.Messages.TutorialOnCrisis = new Proto.ID(nameof (TutorialOnCrisis));
        Ids.Messages.TutorialOnFurnace = new Proto.ID(nameof (TutorialOnFurnace));
        Ids.Messages.TutorialOnIronOreSmelting = new Proto.ID(nameof (TutorialOnIronOreSmelting));
        Ids.Messages.TutorialOnDiesel = new Proto.ID(nameof (TutorialOnDiesel));
        Ids.Messages.TutorialOnStoragesAndTransports = new Proto.ID(nameof (TutorialOnStoragesAndTransports));
        Ids.Messages.TutorialOnTrucks = new Proto.ID(nameof (TutorialOnTrucks));
        Ids.Messages.TutorialOnShipRepair = new Proto.ID(nameof (TutorialOnShipRepair));
        Ids.Messages.WarningNoWorkersNoBeacon = new Proto.ID(nameof (WarningNoWorkersNoBeacon));
        Ids.Messages.WarningLowMaintenanceNoDepot = new Proto.ID(nameof (WarningLowMaintenanceNoDepot));
        Ids.Messages.WarningLowDiesel = new Proto.ID(nameof (WarningLowDiesel));
      }
    }

    public static class MessageGroups
    {
      public static readonly Proto.ID Tools;
      public static readonly Proto.ID GettingStarted;
      public static readonly Proto.ID General;
      public static readonly Proto.ID FoodProduction;
      public static readonly Proto.ID Settlement;
      public static readonly Proto.ID Terraforming;
      public static readonly Proto.ID Logistics;
      public static readonly Proto.ID World;
      public static readonly Proto.ID Warnings;

      static MessageGroups()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.MessageGroups.Tools = new Proto.ID("MessageGroupTools");
        Ids.MessageGroups.GettingStarted = new Proto.ID("MessageGroupGettingStarted");
        Ids.MessageGroups.General = new Proto.ID("MessageGroupGeneral");
        Ids.MessageGroups.FoodProduction = new Proto.ID("MessageGroupFoodProduction");
        Ids.MessageGroups.Settlement = new Proto.ID("MessageGroupSettlement");
        Ids.MessageGroups.Terraforming = new Proto.ID("MessageGroupTerraforming");
        Ids.MessageGroups.Logistics = new Proto.ID("MessageGroupLogistics");
        Ids.MessageGroups.World = new Proto.ID("MessageGroupWorld");
        Ids.MessageGroups.Warnings = new Proto.ID("MessageGroupWarnings");
      }
    }

    public static class Notifications
    {
    }

    public static class Products
    {
      [VirtualProduct(typeof (ElectricityQuantityFormatter), "Assets/Base/Products/Icons/Electricity.svg", null, true, true, 0.0f, null)]
      public static readonly ProductProto.ID Electricity;
      [VirtualProduct(typeof (ElectricityQuantityFormatter), "Assets/Base/Products/Icons/MechanicalPower.svg", null, true, true, 0.0f, null)]
      public static readonly ProductProto.ID MechanicalPower;
      [VirtualProduct(typeof (ComputingQuantityFormatter), "Assets/Unity/UserInterface/General/Computing128.png", null, true, true, 0.0f, null)]
      public static readonly ProductProto.ID Computing;
      [VirtualProduct(typeof (NoUnitsQuantityFormatter), "Assets/Unity/UserInterface/General/Unity128.png", "Unity", false, true, 0.0f, null)]
      public static readonly ProductProto.ID Upoints;
      [VirtualProduct(typeof (ProductCountQuantityFormatter), "Assets/Base/Products/Icons/Heat.svg", "Heat", false, true, 0.0f, null)]
      public static readonly ProductProto.ID Heat;
      [VirtualProduct(typeof (ProductCountQuantityFormatter), "Assets/Base/Products/Icons/Maintenance1.svg", "Maintenance I", false, true, 0.035f, null)]
      public static readonly ProductProto.ID MaintenanceT1;
      [VirtualProduct(typeof (ProductCountQuantityFormatter), "Assets/Base/Products/Icons/Maintenance2.svg", "Maintenance II", false, true, 0.09f, null)]
      public static readonly ProductProto.ID MaintenanceT2;
      [VirtualProduct(typeof (ProductCountQuantityFormatter), "Assets/Base/Products/Icons/Maintenance3.svg", "Maintenance III", false, true, 0.18f, null)]
      public static readonly ProductProto.ID MaintenanceT3;
      [CountableProduct("Assets/Base/Products/Countable/RawWood.prefab", null, "Assets/Base/Products/Icons/Wood.svg", false, false, true, 0, null, "Product_Wood", 1.0, false, false, CountableProductStackingMode.TriangleHorizontal, false, false)]
      public static readonly ProductProto.ID Wood;
      [LooseProduct("Assets/Base/Products/Loose/Woodchips.mat", null, false, "Assets/Base/Products/Icons/Woodchips.svg", false, false, false, -1, false, false, true, null, null, 1.0)]
      public static readonly ProductProto.ID Woodchips;
      [FluidProduct(9666918, "Assets/Base/Products/Icons/Fertilizer.svg", 9666918, 2769152, "Fertilizer I", false, true, false, false, null)]
      public static readonly ProductProto.ID FertilizerChemical;
      [ProtoParamFor("FertilizerChemical")]
      public static readonly FertilizerProductParam FertilizerChemicalParam;
      [FluidProduct(11250015, "Assets/Base/Products/Icons/Fertilizer2.svg", 9669967, 2769152, "Fertilizer II", false, true, false, false, null)]
      public static readonly ProductProto.ID FertilizerChemical2;
      [ProtoParamFor("FertilizerChemical2")]
      public static readonly FertilizerProductParam FertilizerChemical2Param;
      [FluidProduct(9280808, "Assets/Base/Products/Icons/FertilizerOrganic.svg", 8096325, 2769152, "Fertilizer (organic)", false, true, false, false, null)]
      public static readonly ProductProto.ID FertilizerOrganic;
      [ProtoParamFor("FertilizerOrganic")]
      public static readonly FertilizerProductParam FertilizerOrganicParam;
      [CountableProduct("Assets/Base/Products/Countable/TreeSapling.prefab", null, "Assets/Base/Products/Icons/TreeSapling.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID TreeSapling;
      [CountableProduct("Assets/Base/Products/Countable/Paper.prefab", null, "Assets/Base/Products/Icons/Paper.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Paper;
      [LooseProduct("Assets/Base/Products/Loose/Dirt.mat", null, false, "Assets/Base/Products/Icons/Dirt.svg", false, true, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Dirt;
      [ProtoParamFor("Dirt")]
      public static readonly LooseProductParam DirtParams;
      [ProtoParamFor("Dirt")]
      public static readonly NotifyIfCannotDumpFromTowerParam DirtNotifyDumpParam;
      [LooseProduct("Assets/Base/Products/Loose/Compost.mat", null, false, "Assets/Base/Products/Icons/Digestate.svg", false, false, false, -1, false, false, true, null, null, 1.0)]
      public static readonly ProductProto.ID Compost;
      [ProtoParamFor("Compost")]
      public static readonly LooseProductParam CompostParams;
      [LooseProduct("Assets/Base/Products/Loose/Sludge.mat", null, false, "Assets/Base/Products/Icons/Sludge.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Sludge;
      [LooseProduct("Assets/Base/Products/Loose/Limestone.mat", null, false, "Assets/Base/Products/Icons/Limestone.svg", false, false, false, 14272160, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Limestone;
      [ProtoParamFor("Limestone")]
      public static readonly LooseProductParam LimestoneParams;
      [LooseProduct("Assets/Base/Products/Loose/Rock.mat", null, true, "Assets/Base/Products/Icons/Rock.svg", false, true, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Rock;
      [ProtoParamFor("Rock")]
      public static readonly LooseProductParam RockParams;
      [ProtoParamFor("Rock")]
      public static readonly NotifyIfCannotDumpFromTowerParam RockNotifyDumpParam;
      [LooseProduct("Assets/Base/Products/Loose/Gravel.mat", null, false, "Assets/Base/Products/Icons/Gravel.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Gravel;
      [ProtoParamFor("Gravel")]
      public static readonly LooseProductParam GravelParams;
      [LooseProduct("Assets/Base/Products/Loose/ManufacturedSand.mat", null, false, "Assets/Base/Products/Icons/ManufacturedSand.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID ManufacturedSand;
      [LooseProduct("Assets/Base/Products/Loose/FilterMedia.mat", null, false, "Assets/Base/Products/Icons/FilterMedia.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID FilterMedia;
      [LooseProduct("Assets/Base/Products/Loose/Coal.mat", null, false, "Assets/Base/Products/Icons/Coal.svg", false, false, false, 4465169, true, false, true, null, null, 1.0)]
      public static readonly ProductProto.ID Coal;
      [ProtoParamFor("Coal")]
      public static readonly LooseProductParam CoalParams;
      [CountableProduct("Assets/Base/Products/Countable/Graphite.prefab", null, "Assets/Base/Products/Icons/Graphite.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.TriangleHorizontal, false, false)]
      public static readonly ProductProto.ID Graphite;
      [LooseProduct("Assets/Base/Products/Loose/Slag.mat", null, true, "Assets/Base/Products/Icons/Slag.svg", false, true, false, -1, false, false, false, "by-product of smelting ores in furnace", null, 1.0)]
      public static readonly ProductProto.ID Slag;
      [ProtoParamFor("Slag")]
      public static readonly LooseProductParam SlagParams;
      [LooseProduct("Assets/Base/Products/Loose/SlagCrushed.mat", null, false, "Assets/Base/Products/Icons/SlagCrushed.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID SlagCrushed;
      [ProtoParamFor("SlagCrushed")]
      public static readonly LooseProductParam SlagCrushedParams;
      [LooseProduct("Assets/Base/Products/Loose/IronOre.mat", null, true, "Assets/Base/Products/Icons/IronOre.svg", false, false, false, 13391155, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID IronOre;
      [ProtoParamFor("IronOre")]
      public static readonly LooseProductParam IronOreParams;
      [LooseProduct("Assets/Base/Products/Loose/IronOreCrushed.mat", null, false, "Assets/Base/Products/Icons/IronCrushed.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID IronOreCrushed;
      [ProtoParamFor("IronOreCrushed")]
      public static readonly LooseProductParam IronOreCrushedParams;
      [LooseProduct("Assets/Base/Products/Loose/IronScrap.mat", null, true, "Assets/Base/Products/Icons/IronScrap.svg", false, false, false, -1, false, true, false, null, null, 1.0)]
      public static readonly ProductProto.ID IronScrap;
      [CountableProduct("Assets/Base/Products/Countable/IronScrapPressed.prefab", null, "Assets/Base/Products/Icons/IronScrapPressed.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.StackedAlternating, true, false)]
      public static readonly ProductProto.ID IronScrapPressed;
      [MoltenProduct("Assets/Base/Products/Molten/Steel.mat", "Assets/Base/Products/Molten/MoltenSteel.prefab", "Assets/Base/Products/Icons/IronMolten.svg", null)]
      public static readonly ProductProto.ID MoltenIron;
      [MoltenProduct("Assets/Base/Products/Molten/Steel.mat", "Assets/Base/Products/Molten/MoltenSteel.prefab", "Assets/Base/Products/Icons/SteelMolten.svg", null)]
      public static readonly ProductProto.ID MoltenSteel;
      [CountableProduct("Assets/Base/Products/Countable/IronSlab.prefab", null, "Assets/Base/Products/Icons/Iron.svg", false, false, true, 0, null, "Product_IronScrap", 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Iron;
      [CountableProduct("Assets/Base/Products/Countable/SteelSlab.prefab", null, "Assets/Base/Products/Icons/Steel.svg", false, false, true, 0, null, "Product_IronScrap", 2.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Steel;
      [LooseProduct("Assets/Base/Products/Loose/CopperOre.mat", null, true, "Assets/Base/Products/Icons/CopperOre.svg", false, false, false, 6732731, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID CopperOre;
      [ProtoParamFor("CopperOre")]
      public static readonly LooseProductParam CopperOreParams;
      [LooseProduct("Assets/Base/Products/Loose/CopperOreCrushed.mat", null, false, "Assets/Base/Products/Icons/CopperCrushed.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID CopperOreCrushed;
      [ProtoParamFor("CopperOreCrushed")]
      public static readonly LooseProductParam CopperOreCrushedParams;
      [LooseProduct("Assets/Base/Products/Loose/CopperScrap.mat", null, true, "Assets/Base/Products/Icons/CopperScrap.svg", false, false, false, -1, false, true, false, null, null, 1.0)]
      public static readonly ProductProto.ID CopperScrap;
      [CountableProduct("Assets/Base/Products/Countable/CopperScrapPressed.prefab", null, "Assets/Base/Products/Icons/CopperScrapPressed.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.StackedAlternating, true, false)]
      public static readonly ProductProto.ID CopperScrapPressed;
      [MoltenProduct("Assets/Base/Products/Molten/Copper.mat", "Assets/Base/Products/Molten/MoltenCopper.prefab", "Assets/Base/Products/Icons/CopperMolten.svg", null)]
      public static readonly ProductProto.ID MoltenCopper;
      [CountableProduct("Assets/Base/Products/Countable/CopperSlabDirty.prefab", null, "Assets/Base/Products/Icons/CopperImpure.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID ImpureCopper;
      [CountableProduct("Assets/Base/Products/Countable/CopperSlab.prefab", null, "Assets/Base/Products/Icons/Copper.svg", false, false, true, 0, null, "Product_CopperScrap", 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Copper;
      [LooseProduct("Assets/Base/Products/Loose/GoldOre.mat", null, true, "Assets/Base/Products/Icons/GoldOre.svg", false, false, false, 15641105, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID GoldOre;
      [ProtoParamFor("GoldOre")]
      public static readonly LooseProductParam GoldOreParams;
      [LooseProduct("Assets/Base/Products/Loose/GoldOreCrushed.mat", null, false, "Assets/Base/Products/Icons/GoldOreCrushed.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID GoldOreCrushed;
      [ProtoParamFor("GoldOreCrushed")]
      public static readonly LooseProductParam GoldOreCrushedParams;
      [LooseProduct("Assets/Base/Products/Loose/GoldOrePowder.mat", null, false, "Assets/Base/Products/Icons/GoldOrePowder.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID GoldOrePowder;
      [LooseProduct("Assets/Base/Products/Loose/GoldOreConcentrate.mat", null, false, "Assets/Base/Products/Icons/GoldOreConcentrate.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID GoldOreConcentrate;
      [CountableProduct("Assets/Base/Products/Countable/GoldIngot.prefab", null, "Assets/Base/Products/Icons/Gold.svg", false, false, false, 0, null, "Product_GoldScrap", 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Gold;
      [LooseProduct("Assets/Base/Products/Loose/GoldScrap.mat", null, false, "Assets/Base/Products/Icons/GoldScrap.svg", false, false, false, -1, false, true, false, null, null, 1.0)]
      public static readonly ProductProto.ID GoldScrap;
      [CountableProduct("Assets/Base/Products/Countable/GoldenScrap.prefab", null, "Assets/Base/Products/Icons/GoldScrapPressed.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.StackedAlternating, true, false)]
      public static readonly ProductProto.ID GoldScrapPressed;
      [LooseProduct("Assets/Base/Products/Loose/Sand.mat", null, false, "Assets/Base/Products/Icons/Sand.svg", false, false, false, 13478004, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Sand;
      [ProtoParamFor("Sand")]
      public static readonly LooseProductParam SandParams;
      [MoltenProduct("Assets/Base/Products/Molten/Silicon.mat", "Assets/Base/Products/Molten/MoltenSillicon.prefab", "Assets/Base/Products/Icons/GlassMolten.svg", null)]
      public static readonly ProductProto.ID MoltenGlass;
      [LooseProduct("Assets/Base/Products/Loose/BrokenGlass.mat", null, true, "Assets/Base/Products/Icons/BrokenGlass.svg", false, false, false, -1, false, true, false, null, null, 1.0)]
      public static readonly ProductProto.ID BrokenGlass;
      [CountableProduct("Assets/Base/Products/Countable/GlassSheets.prefab", null, "Assets/Base/Products/Icons/Glass.svg", false, false, false, 0, null, "Product_BrokenGlass", 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Glass;
      [LooseProduct("Assets/Base/Products/Loose/GlassMix.mat", null, false, "Assets/Base/Products/Icons/GlassMix.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID GlassMix;
      [LooseProduct("Assets/Base/Products/Loose/UraniumOre.mat", null, true, "Assets/Base/Products/Icons/UraniumOre.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID UraniumOre;
      [LooseProduct("Assets/Base/Products/Loose/UraniumOreCrushed.mat", null, false, "Assets/Base/Products/Icons/UraniumOreCrushed.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID UraniumOreCrushed;
      [LooseProduct("Assets/Base/Products/Loose/Yellowcake.mat", null, false, "Assets/Base/Products/Icons/YellowCake.svg", false, false, false, -1, false, false, false, "uranium concentrate powder (U3O8), yellow color, https://en.wikipedia.org/wiki/Yellowcake", null, 1.0)]
      public static readonly ProductProto.ID Yellowcake;
      [CountableProduct("Assets/Base/Products/Countable/UraniumEnriched4.prefab", "Enriched uranium (4%)", "Assets/Base/Products/Icons/UraniumEnriched4Perc.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID UraniumEnriched;
      [CountableProduct("Assets/Base/Products/Countable/UraniumEnriched20.prefab", "Enriched uranium (20%)", "Assets/Base/Products/Icons/UraniumEnriched20Perc.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID UraniumEnriched20;
      [CountableProduct("Assets/Base/Products/Countable/UraniumReprocessed.prefab", "Reprocessed uranium (1%)", "Assets/Base/Products/Icons/ReprocessedUranium.svg", false, false, false, 0, "Recycled uranium created from partially spent fuel", null, 1.0, false, true, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID UraniumReprocessed;
      [CountableProduct("Assets/Base/Products/Countable/Plutonium.prefab", null, "Assets/Base/Products/Icons/Plutonium.svg", false, false, false, 2, null, null, 1.0, false, true, CountableProductStackingMode.Triangle, false, false)]
      public static readonly ProductProto.ID Plutonium;
      [LooseProduct("Assets/Base/Products/Loose/UraniumDepleted.mat", "Depleted uranium", false, "Assets/Base/Products/Icons/DepletedUranium.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID UraniumDepleted;
      [ProtoParamFor("UraniumDepleted")]
      public static readonly LooseProductParam UraniumDepletedParams;
      [CountableProduct("Assets/Base/Products/Countable/UraniumRod.prefab", null, "Assets/Base/Products/Icons/UraniumRod.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID UraniumRod;
      [CountableProduct("Assets/Base/Products/Countable/MoxRod.prefab", "MOX rod", "Assets/Base/Products/Icons/MoxRod.svg", false, false, false, 0, "special type of fuel for nuclear reactor called MOX, https://en.wikipedia.org/wiki/MOX_fuel", null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID MoxRod;
      [CountableProduct("Assets/Base/Products/Countable/SpentFuel.prefab", null, "Assets/Base/Products/Icons/SpentFuel.svg", false, false, false, 2, "spent nuclear fuel, radioactive, keep short", "Product_IronScrap", 0.5, false, false, CountableProductStackingMode.Triangle, false, false)]
      public static readonly ProductProto.ID SpentFuel;
      [CountableProduct("Assets/Base/Products/Countable/SpentMox.prefab", "Spent MOX", "Assets/Base/Products/Icons/SpentMox.svg", false, false, false, 2, "spent nuclear fuel called MOX, radioactive, https://en.wikipedia.org/wiki/MOX_fuel", "Product_IronScrap", 1.0, false, false, CountableProductStackingMode.Triangle, false, false)]
      public static readonly ProductProto.ID SpentMox;
      [CountableProduct("Assets/Base/Products/Countable/FissionProduct.prefab", null, "Assets/Base/Products/Icons/FissionProduct.svg", false, false, false, 4, "waste from nuclear fission", null, 1.0, false, false, CountableProductStackingMode.Triangle, false, false)]
      public static readonly ProductProto.ID FissionProduct;
      [CountableProduct("Assets/Base/Products/Countable/RetiredWaste.prefab", null, "Assets/Base/Products/Icons/RetiredWaste.svg", false, false, false, 0, "nuclear waste that is no longer radioactive (was retired and can be recycled)", null, 1.0, false, false, CountableProductStackingMode.Triangle, false, false)]
      public static readonly ProductProto.ID RetiredWaste;
      [ProtoParamFor("FissionProduct")]
      public static readonly RadioactiveWasteParam FissionProductParam;
      [FluidProduct(11877694, "Assets/Base/Products/Icons/CoreFuel.svg", 10976637, 5581868, null, true, false, false, false, "liquid fuel for a fast breeder reactor that goes into its core, see https://en.wikipedia.org/wiki/Breeder_reactor#Breeder_reactor_concepts")]
      public static readonly ProductProto.ID CoreFuel;
      [OnlyForSaveCompatibility(null)]
      [FluidProduct(10642176, "Assets/Base/Products/Icons/CoreFuelDirty.svg", 9994854, 5581868, "Core fuel (spent)", true, false, false, false, "result of Product_CoreFuel after it was used in nuclear reactor, but this still gets filtered and reprocessed back to some Product_CoreFuel, so it is not entirely spent yet")]
      public static readonly ProductProto.ID CoreFuelDirty;
      [FluidProduct(3768777, "Assets/Base/Products/Icons/BlanketFuel.svg", 8819879, 5581868, null, true, false, false, false, "liquid content to be enriched in a fast breeder reactor, placed to wrap the reactor core as blanket, see https://en.wikipedia.org/wiki/Breeder_reactor#Breeder_reactor_concepts")]
      public static readonly ProductProto.ID BlanketFuel;
      [FluidProduct(10520291, "Assets/Base/Products/Icons/BlanketFuelEnriched.svg", 9604007, 5581868, "Blanket fuel (enriched)", true, false, false, false, "see more in Product_BlanketFuel")]
      public static readonly ProductProto.ID BlanketFuelEnriched;
      [LooseProduct("Assets/Base/Products/Loose/Quartz.mat", null, true, "Assets/Base/Products/Icons/Quartz.svg", false, false, false, 15658734, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Quartz;
      [ProtoParamFor("Quartz")]
      public static readonly LooseProductParam QuartzParams;
      [LooseProduct("Assets/Base/Products/Loose/QuartzCrushed.mat", null, false, "Assets/Base/Products/Icons/QuartzCrushed.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID QuartzCrushed;
      [ProtoParamFor("QuartzCrushed")]
      public static readonly LooseProductParam QuartzCrushedParams;
      [MoltenProduct("Assets/Base/Products/Molten/Silicon.mat", "Assets/Base/Products/Molten/MoltenSillicon.prefab", "Assets/Base/Products/Icons/SiliconMolten.svg", null)]
      public static readonly ProductProto.ID MoltenSilicon;
      [CountableProduct("Assets/Base/Products/Countable/Silicon.prefab", "Silicon (poly)", "Assets/Base/Products/Icons/Silicon.svg", false, false, false, 0, "high purity polycrystalline form of silicon, keep short", null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID PolySilicon;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-1a.prefab", null, "Assets/Base/Products/Icons/MonoWafer.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID SiliconWafer;
      [FluidProduct(12309742, "Assets/Base/Products/Icons/Water.svg", 7967920, 3294843, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Water;
      [FluidProduct(8969727, "Assets/Base/Products/Icons/WaterChilled.svg", 7051671, 3294843, null, true, true, false, false, null)]
      public static readonly ProductProto.ID ChilledWater;
      [FluidProduct(11520255, "Assets/Base/Products/Icons/SeaWater.svg", 8423855, 3294843, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Seawater;
      [FluidProduct(15658734, "Assets/Base/Products/Icons/Brine.svg", 11119017, 3294843, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Brine;
      [FluidProduct(7097892, "Assets/Base/Products/Icons/WasteWater.svg", 5850409, 12230776, null, false, true, true, false, null)]
      public static readonly ProductProto.ID WasteWater;
      [FluidProduct(13092661, "Assets/Base/Products/Icons/ToxicSlurry.svg", 9673826, 3355443, null, false, true, true, false, null)]
      public static readonly ProductProto.ID ToxicSlurry;
      [VirtualProduct(typeof (ProductCountQuantityFormatter), "Assets/Base/Products/Icons/WaterPollution.svg", "Water pollution", false, true, 0.0f, null)]
      public static readonly ProductProto.ID PollutedWater;
      [VirtualProduct(typeof (ProductCountQuantityFormatter), "Assets/Base/Products/Icons/AirPollution.svg", "Air pollution", false, true, 0.0f, null)]
      public static readonly ProductProto.ID PollutedAir;
      [FluidProduct(15528751, "Assets/Base/Products/Icons/Chlorine.svg", 9540414, 3355443, null, false, false, false, false, null)]
      public static readonly ProductProto.ID Chlorine;
      [FluidProduct(16777215, "Assets/Base/Products/Icons/SteamSp.svg", 11184810, 9325387, "Steam (super)", true, true, false, false, "super pressure steam (more pressure than high press steam), keep short!")]
      public static readonly ProductProto.ID SteamSp;
      [FluidProduct(15658734, "Assets/Base/Products/Icons/SteamHp.svg", 11184810, 10576187, "Steam (high)", true, true, false, false, "high pressure steam, keep short!")]
      public static readonly ProductProto.ID SteamHi;
      [FluidProduct(14540253, "Assets/Base/Products/Icons/SteamLp.svg", 11184810, 4615565, "Steam (low)", true, true, false, false, "low pressure steam, keep short!")]
      public static readonly ProductProto.ID SteamLo;
      [FluidProduct(13421772, "Assets/Base/Products/Icons/SteamDepleated.svg", 11184810, 5592405, "Steam (depleted)", true, true, false, false, null)]
      public static readonly ProductProto.ID SteamDepleted;
      [LooseProduct("Assets/Base/Products/Loose/Salt.mat", null, false, "Assets/Base/Products/Icons/Salt.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Salt;
      [FluidProduct(4473924, "Assets/Base/Products/Icons/Exhaust.svg", 6710886, 11184810, null, true, true, false, false, null)]
      public static readonly ProductProto.ID Exhaust;
      [LooseProduct("Assets/Base/Products/Loose/Recyclables.mat", null, true, "Assets/Base/Products/Icons/MetalScrap128.png", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Recyclables;
      [CountableProduct("Assets/Base/Products/Countable/RecyclablesPressed.prefab", null, "Assets/Base/Products/Icons/RecyclablesPressed.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.StackedAlternating, true, false)]
      public static readonly ProductProto.ID RecyclablesPressed;
      [FluidProduct(2236962, "Assets/Base/Products/Icons/CrudeOil.svg", 4144959, 13017404, null, false, false, false, false, null)]
      public static readonly ProductProto.ID CrudeOil;
      [FluidProduct(15058789, "Assets/Base/Products/Icons/Diesel.svg", 11374395, 8533030, null, false, false, false, true, null)]
      public static readonly ProductProto.ID Diesel;
      [FluidProduct(13419160, "Assets/Base/Products/Icons/Naphtha.svg", 10459230, 5981723, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Naphtha;
      [FluidProduct(14128694, "Assets/Base/Products/Icons/FuelGas.svg", 8871467, 14068050, null, false, false, false, false, null)]
      public static readonly ProductProto.ID FuelGas;
      [LooseProduct("Assets/Base/Products/Loose/Sulfur.mat", null, false, "Assets/Base/Products/Icons/Sulfur.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Sulfur;
      [FluidProduct(9942865, "Assets/Base/Products/Icons/SourWater.svg", 8227677, 3294843, null, false, true, true, false, null)]
      public static readonly ProductProto.ID SourWater;
      [FluidProduct(15658734, "Assets/Base/Products/Icons/Ammonia.svg", 9283002, 10706987, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Ammonia;
      [FluidProduct(13459298, "Assets/Base/Products/Icons/Acid.svg", 10055530, 3355443, null, false, true, true, false, null)]
      public static readonly ProductProto.ID Acid;
      [FluidProduct(6048037, "Assets/Base/Products/Icons/OilHeavy.svg", 5982503, 13017404, null, false, true, false, false, null)]
      public static readonly ProductProto.ID HeavyOil;
      [FluidProduct(7488882, "Assets/Base/Products/Icons/OilMedium.svg", 6511203, 13017404, null, false, true, false, false, null)]
      public static readonly ProductProto.ID MediumOil;
      [FluidProduct(9278282, "Assets/Base/Products/Icons/OilLight.svg", 7106371, 13017404, null, false, true, false, false, null)]
      public static readonly ProductProto.ID LightOil;
      [CountableProduct("Assets/Base/Products/Countable/Plastic.prefab", null, "Assets/Base/Products/Icons/Plastic.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Plastic;
      [CountableProduct("Assets/Base/Products/Countable/Rubber.prefab", null, "Assets/Base/Products/Icons/Rubber.svg", false, false, true, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Rubber;
      [FluidProduct(15658734, "Assets/Base/Products/Icons/Hydrogen.svg", 11188190, 3355443, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Hydrogen;
      [FluidProduct(15658734, "Assets/Base/Products/Icons/Nitrogen.svg", 5730195, 11184810, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Nitrogen;
      [FluidProduct(13421772, "Assets/Base/Products/Icons/CarbonDioxide.svg", 5592405, 11184810, null, false, true, false, false, null)]
      public static readonly ProductProto.ID CarbonDioxide;
      [FluidProduct(15658734, "Assets/Base/Products/Icons/Ethanol.svg", 10329501, 3563063, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Ethanol;
      [LooseProduct("Assets/Base/Products/Loose/Potato.mat", null, false, "Assets/Base/Products/Icons/Potato128.png", false, false, false, -1, true, false, false, null, "Product_Biomass", 1.0)]
      public static readonly ProductProto.ID Potato;
      [LooseProduct("Assets/Base/Products/Loose/Corn.mat", null, true, "Assets/Base/Products/Icons/Corn.svg", false, false, false, -1, false, false, false, null, "Product_Biomass", 1.0)]
      public static readonly ProductProto.ID Corn;
      [LooseProduct("Assets/Base/Products/Loose/Wheat.mat", null, false, "Assets/Base/Products/Icons/Wheat.svg", false, false, false, -1, false, false, false, null, "Product_Biomass", 1.0)]
      public static readonly ProductProto.ID Wheat;
      [LooseProduct("Assets/Base/Products/Loose/SoyBeans.mat", null, false, "Assets/Base/Products/Icons/Soybean.svg", false, false, false, -1, false, false, false, null, "Product_Biomass", 1.0)]
      public static readonly ProductProto.ID Soybean;
      [LooseProduct("Assets/Base/Products/Loose/Sugarcane.mat", null, true, "Assets/Base/Products/Icons/SugarCane.svg", false, false, false, -1, false, false, false, null, "Product_Biomass", 1.0)]
      public static readonly ProductProto.ID SugarCane;
      [LooseProduct("Assets/Base/Products/Loose/Canola.mat", null, false, "Assets/Base/Products/Icons/Canola.svg", false, false, false, -1, false, false, false, null, "Product_Biomass", 1.0)]
      public static readonly ProductProto.ID Canola;
      [CountableProduct("Assets/Base/Products/Countable/Vegetables.prefab", null, "Assets/Base/Products/Icons/Vegetables.svg", false, false, false, 0, null, "Product_Biomass", 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Vegetables;
      [CountableProduct("Assets/Base/Products/Countable/Fruits.prefab", null, "Assets/Base/Products/Icons/Fruits.svg", false, false, false, 0, null, "Product_Biomass", 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Fruit;
      [LooseProduct("Assets/Base/Products/Loose/Poppy.mat", null, false, "Assets/Base/Products/Icons/Poppy.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Poppy;
      [CountableProduct("Assets/Base/Products/Countable/Bread.prefab", null, "Assets/Base/Products/Icons/Bread.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Bread;
      [CountableProduct("Assets/Base/Products/Countable/Flour.prefab", null, "Assets/Base/Products/Icons/Flour.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Flour;
      [LooseProduct("Assets/Base/Products/Loose/AnimalFeed.mat", null, false, "Assets/Base/Products/Icons/AnimalFood.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID AnimalFeed;
      [LooseProduct("Assets/Base/Products/Loose/Biomass.mat", null, false, "Assets/Base/Products/Icons/Biomass.svg", false, false, false, -1, false, false, true, null, null, 1.0)]
      public static readonly ProductProto.ID Biomass;
      [CountableProduct("Assets/Base/Products/Countable/Eggs.prefab", null, "Assets/Base/Products/Icons/Eggs.svg", false, false, false, 0, null, "Product_Biomass", 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Eggs;
      [CountableProduct("Assets/Base/Products/Countable/Tofu.prefab", null, "Assets/Base/Products/Icons/Tofu.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Tofu;
      [FluidProduct(14273855, "Assets/Base/Products/Icons/CookingOil.svg", 10785858, 3355443, null, false, true, false, false, null)]
      public static readonly ProductProto.ID CookingOil;
      [CountableProduct("Assets/Base/Products/Countable/Meat.prefab", null, "Assets/Base/Products/Icons/Meat.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Meat;
      [CountableProduct("Assets/Base/Products/Countable/Sausages.prefab", null, "Assets/Base/Products/Icons/Sausage.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Sausage;
      [CountableProduct("Assets/Base/Products/Countable/Snacks.prefab", null, "Assets/Base/Products/Icons/Snack.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Snack;
      [CountableProduct("Assets/Base/Products/Countable/Cake.prefab", null, "Assets/Base/Products/Icons/Cake.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Cake;
      [CountableProduct("Assets/Base/Products/Countable/FoodPack.prefab", null, "Assets/Base/Products/Icons/FoodPack.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID FoodPack;
      [VirtualProduct(typeof (ProductCountQuantityFormatter), "Assets/Base/Products/Icons/Chicken.svg", null, false, false, 0.0f, null)]
      public static readonly ProductProto.ID Chicken;
      [VirtualProduct(typeof (ProductCountQuantityFormatter), "Assets/Unity/UserInterface/WorldMap/CargoShipStoryIcon256.png", null, false, true, 0.0f, null)]
      public static readonly ProductProto.ID CargoShip;
      [CountableProduct("Assets/Base/Products/Countable/Chicken.prefab", null, "Assets/Base/Products/Icons/ChickenCarcass.svg", false, false, false, 0, "a chicken body with removed limbs and feathers, ready to be butchered", "Product_Biomass", 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID ChickenCarcass;
      [LooseProduct("Assets/Base/Products/Loose/MeatTrimmings.mat", null, false, "Assets/Base/Products/Icons/MeatTrimmings.svg", false, false, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID MeatTrimmings;
      [LooseProduct("Assets/Base/Products/Loose/Sugar.mat", null, false, "Assets/Base/Products/Icons/Sugar.svg", false, false, false, -1, false, false, true, null, null, 1.0)]
      public static readonly ProductProto.ID Sugar;
      [FluidProduct(13739555, "Assets/Base/Products/Icons/CornMash.svg", 9994560, 3355443, null, false, true, false, false, null)]
      public static readonly ProductProto.ID CornMash;
      [CountableProduct("Assets/Base/Products/Countable/HouseholdGoods.prefab", null, "Assets/Base/Products/Icons/HouseholdGoods.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID HouseholdGoods;
      [CountableProduct("Assets/Base/Products/Countable/HouseholdAppliances.prefab", null, "Assets/Base/Products/Icons/HouseholdAppliances.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID HouseholdAppliances;
      [CountableProduct("Assets/Base/Products/Countable/ConsumerElectronics.prefab", null, "Assets/Base/Products/Icons/ConsumerElectronics.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID ConsumerElectronics;
      [CountableProduct("Assets/Base/Products/Countable/Antibiotics.prefab", null, "Assets/Base/Products/Icons/Penicillin.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Antibiotics;
      [CountableProduct("Assets/Base/Products/Countable/Disinfectant.prefab", null, "Assets/Base/Products/Icons/Disinfectant.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Disinfectant;
      [CountableProduct("Assets/Base/Products/Countable/Anesthetics.prefab", null, "Assets/Base/Products/Icons/Anesthetics.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Triangle, false, false)]
      public static readonly ProductProto.ID Anesthetics;
      [CountableProduct("Assets/Base/Products/Countable/Morphine.prefab", null, "Assets/Base/Products/Icons/Morphine.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Morphine;
      [CountableProduct("Assets/Base/Products/Countable/MedicalEquipment.prefab", null, "Assets/Base/Products/Icons/MedicalEquipment.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID MedicalEquipment;
      [CountableProduct("Assets/Base/Products/Countable/MedicalSupplies.prefab", "Medical Supplies", "Assets/Base/Products/Icons/MedicalSupplies.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID MedicalSupplies;
      [CountableProduct("Assets/Base/Products/Countable/MedicalSupplies2.prefab", "Medical Supplies II", "Assets/Base/Products/Icons/MedicalSupplies2.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID MedicalSupplies2;
      [CountableProduct("Assets/Base/Products/Countable/MedicalSupplies3.prefab", "Medical Supplies III", "Assets/Base/Products/Icons/MedicalSupplies3.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID MedicalSupplies3;
      [CountableProduct("Assets/Base/Products/Countable/Cement.prefab", null, "Assets/Base/Products/Icons/Cement.svg", false, false, true, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Cement;
      [CountableProduct("Assets/Base/Products/Countable/ConcreteSlab.prefab", null, "Assets/Base/Products/Icons/Concrete.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID ConcreteSlab;
      [CountableProduct("Assets/Base/Products/Countable/Bricks.prefab", null, "Assets/Base/Products/Icons/Bricks.svg", false, false, true, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Bricks;
      [CountableProduct("Assets/Base/Products/Countable/ConstructionParts1.prefab", "Construction Parts", "Assets/Base/Products/Icons/ConstructionParts1.svg", false, false, true, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID ConstructionParts;
      [ProtoParamFor("ConstructionParts")]
      public static readonly AllowProductDiscountInUpgrade ConstructionPartsInUpgrade;
      [CountableProduct("Assets/Base/Products/Countable/ConstructionParts2.prefab", "Construction Parts II", "Assets/Base/Products/Icons/ConstructionParts2.svg", false, false, true, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID ConstructionParts2;
      [ProtoParamFor("ConstructionParts2")]
      public static readonly AllowProductDiscountInUpgrade ConstructionParts2InUpgrade;
      [CountableProduct("Assets/Base/Products/Countable/ConstructionParts3.prefab", "Construction Parts III", "Assets/Base/Products/Icons/ConstructionParts3.svg", false, false, true, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID ConstructionParts3;
      [ProtoParamFor("ConstructionParts3")]
      public static readonly AllowProductDiscountInUpgrade ConstructionParts3InUpgrade;
      [CountableProduct("Assets/Base/Products/Countable/ConstructionParts4.prefab", "Construction Parts IV", "Assets/Base/Products/Icons/ConstructionParts4.svg", false, false, true, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID ConstructionParts4;
      [ProtoParamFor("ConstructionParts4")]
      public static readonly AllowProductDiscountInUpgrade ConstructionParts4InUpgrade;
      [CountableProduct("Assets/Base/Products/Countable/MechanicalParts.prefab", "Mechanical Parts", "Assets/Base/Products/Icons/MechanicalParts.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID MechanicalParts;
      [CountableProduct("Assets/Base/Products/Countable/VehicleParts1.prefab", "Vehicle Parts", "Assets/Base/Products/Icons/VehicleParts1.svg", false, false, true, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID VehicleParts;
      [CountableProduct("Assets/Base/Products/Countable/VehicleParts2.prefab", "Vehicle Parts II", "Assets/Base/Products/Icons/VehicleParts2.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID VehicleParts2;
      [CountableProduct("Assets/Base/Products/Countable/VehicleParts3.prefab", "Vehicle Parts III", "Assets/Base/Products/Icons/VehicleParts3.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID VehicleParts3;
      [CountableProduct("Assets/Base/Products/Countable/LabEquipment.prefab", "Lab Equipment", "Assets/Base/Products/Icons/LabEquipment1.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID LabEquipment;
      [CountableProduct("Assets/Base/Products/Countable/LabEquipment2.prefab", "Lab Equipment II", "Assets/Base/Products/Icons/LabEquipment2.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID LabEquipment2;
      [CountableProduct("Assets/Base/Products/Countable/LabEquipment3.prefab", "Lab Equipment III", "Assets/Base/Products/Icons/LabEquipment3.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID LabEquipment3;
      [CountableProduct("Assets/Base/Products/Countable/LabEquipment4.prefab", "Lab Equipment IV", "Assets/Base/Products/Icons/LabEquipment4.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID LabEquipment4;
      [CountableProduct("Assets/Base/Products/Countable/Electronics1.prefab", null, "Assets/Base/Products/Icons/Electronics1.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Electronics;
      [CountableProduct("Assets/Base/Products/Countable/PCB.prefab", null, "Assets/Base/Products/Icons/PCB.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID PCB;
      [CountableProduct("Assets/Base/Products/Countable/Electronics2.prefab", "Electronics II", "Assets/Base/Products/Icons/Electronics2.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Electronics2;
      [CountableProduct("Assets/Base/Products/Countable/El3.prefab", "Electronics III", "Assets/Base/Products/Icons/Electronics3.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Electronics3;
      [CountableProduct("Assets/Base/Products/Countable/Server.prefab", null, "Assets/Base/Products/Icons/Server.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID Server;
      [CountableProduct("Assets/Base/Products/Countable/Microchip.prefab", null, "Assets/Base/Products/Icons/Microchip.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Microchips;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-1a.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer1A.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage1A;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-1b.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer1B.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage1B;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-1c.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer1C.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage1C;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-2a.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer2A.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage2A;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-2b.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer2B.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage2B;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-2c.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer2C.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage2C;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-3a.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer3A.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage3A;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-3b.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer3B.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage3B;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-3c.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer3C.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage3C;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-3a.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer4A.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage4A;
      [CountableProduct("Assets/Base/Products/Countable/Microchip/MicrochipWafer-3b.prefab", null, "Assets/Base/Products/Icons/MicrochipWafer4B.svg", false, true, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Stacked, false, false)]
      public static readonly ProductProto.ID MicrochipsStage4B;
      [CountableProduct("Assets/Base/Products/Countable/SolarCellPoly.prefab", null, "Assets/Base/Products/Icons/SolarCell.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID SolarCell;
      [CountableProduct("Assets/Base/Products/Countable/SolarCellMono.prefab", null, "Assets/Base/Products/Icons/SolarCellMono.svg", false, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.Auto, true, false)]
      public static readonly ProductProto.ID SolarCellMono;
      [FluidProduct(15658734, "Assets/Base/Products/Icons/Oxygen.svg", 8081483, 11184810, null, false, true, false, false, null)]
      public static readonly ProductProto.ID Oxygen;
      [FluidProduct(14016670, "Assets/Base/Products/Icons/HydrogenFluoride.svg", 9868369, 3355443, null, false, true, false, false, null)]
      public static readonly ProductProto.ID HydrogenFluoride;
      [LooseProduct("Assets/Base/Products/Loose/Landfill.mat", null, false, "Assets/Base/Products/Icons/Waste.svg", true, true, false, -1, false, false, false, null, null, 1.0)]
      public static readonly ProductProto.ID Waste;
      [ProtoParamFor("Waste")]
      public static readonly LooseProductParam WasteParams;
      [CountableProduct("Assets/Base/Products/Countable/WastePressed.prefab", null, "Assets/Base/Products/Icons/WastePressed.svg", true, false, false, 0, null, null, 1.0, false, false, CountableProductStackingMode.StackedAlternating, true, false)]
      public static readonly ProductProto.ID WastePressed;
      [CountableProduct("Assets/Base/Products/Countable/Flowers.prefab", null, "Assets/Base/Products/Icons/Flowers.svg", false, false, false, 0, null, null, 1.0, true, false, CountableProductStackingMode.Auto, false, false)]
      public static readonly ProductProto.ID Flowers;

      public static ProductProto.ID CreateId(string id) => new ProductProto.ID("Product_" + id);

      public static ProductProto.ID CreateVirtualId(string id)
      {
        return new ProductProto.ID("Product_Virtual_" + id);
      }

      static Products()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Products.Electricity = IdsCore.Products.Electricity;
        Ids.Products.MechanicalPower = IdsCore.Products.MechanicalPower;
        Ids.Products.Computing = IdsCore.Products.Computing;
        Ids.Products.Upoints = IdsCore.Products.Upoints;
        Ids.Products.Heat = Ids.Products.CreateVirtualId(nameof (Heat));
        Ids.Products.MaintenanceT1 = Ids.Products.CreateVirtualId(nameof (MaintenanceT1));
        Ids.Products.MaintenanceT2 = Ids.Products.CreateVirtualId(nameof (MaintenanceT2));
        Ids.Products.MaintenanceT3 = Ids.Products.CreateVirtualId(nameof (MaintenanceT3));
        Ids.Products.Wood = IdsCore.Products.Wood;
        Ids.Products.Woodchips = Ids.Products.CreateId(nameof (Woodchips));
        Ids.Products.FertilizerChemical = Ids.Products.CreateId("Fertilizer");
        Ids.Products.FertilizerChemicalParam = new FertilizerProductParam(2.Percent(), 120.Percent());
        Ids.Products.FertilizerChemical2 = Ids.Products.CreateId("Fertilizer2");
        Ids.Products.FertilizerChemical2Param = new FertilizerProductParam(2.Percent(), Farm.MAX_FERTILITY_SLIDER_VALUE);
        Ids.Products.FertilizerOrganic = Ids.Products.CreateId(nameof (FertilizerOrganic));
        Ids.Products.FertilizerOrganicParam = new FertilizerProductParam(1.Percent(), 100.Percent());
        Ids.Products.TreeSapling = Ids.Products.CreateId(nameof (TreeSapling));
        Ids.Products.Paper = Ids.Products.CreateId(nameof (Paper));
        Ids.Products.Dirt = Ids.Products.CreateId(nameof (Dirt));
        Ids.Products.DirtParams = new LooseProductParam(Ids.TerrainMaterials.Dirt);
        Ids.Products.DirtNotifyDumpParam = new NotifyIfCannotDumpFromTowerParam();
        Ids.Products.Compost = Ids.Products.CreateId(nameof (Compost));
        Ids.Products.CompostParams = new LooseProductParam(Ids.TerrainMaterials.Compost);
        Ids.Products.Sludge = Ids.Products.CreateId(nameof (Sludge));
        Ids.Products.Limestone = Ids.Products.CreateId(nameof (Limestone));
        Ids.Products.LimestoneParams = new LooseProductParam(Ids.TerrainMaterials.LimestoneDisrupted);
        Ids.Products.Rock = Ids.Products.CreateId(nameof (Rock));
        Ids.Products.RockParams = new LooseProductParam(Ids.TerrainMaterials.RockDisrupted);
        Ids.Products.RockNotifyDumpParam = new NotifyIfCannotDumpFromTowerParam();
        Ids.Products.Gravel = Ids.Products.CreateId(nameof (Gravel));
        Ids.Products.GravelParams = new LooseProductParam(Ids.TerrainMaterials.Gravel);
        Ids.Products.ManufacturedSand = Ids.Products.CreateId(nameof (ManufacturedSand));
        Ids.Products.FilterMedia = Ids.Products.CreateId(nameof (FilterMedia));
        Ids.Products.Coal = Ids.Products.CreateId(nameof (Coal));
        Ids.Products.CoalParams = new LooseProductParam(Ids.TerrainMaterials.CoalDisrupted);
        Ids.Products.Graphite = Ids.Products.CreateId(nameof (Graphite));
        Ids.Products.Slag = Ids.Products.CreateId(nameof (Slag));
        Ids.Products.SlagParams = new LooseProductParam(Ids.TerrainMaterials.Slag);
        Ids.Products.SlagCrushed = Ids.Products.CreateId(nameof (SlagCrushed));
        Ids.Products.SlagCrushedParams = new LooseProductParam(Ids.TerrainMaterials.SlagCrushed);
        Ids.Products.IronOre = Ids.Products.CreateId(nameof (IronOre));
        Ids.Products.IronOreParams = new LooseProductParam(Ids.TerrainMaterials.IronOreDisrupted);
        Ids.Products.IronOreCrushed = Ids.Products.CreateId(nameof (IronOreCrushed));
        Ids.Products.IronOreCrushedParams = new LooseProductParam(Ids.TerrainMaterials.IronOreCrushed);
        Ids.Products.IronScrap = new ProductProto.ID("Product_IronScrap");
        Ids.Products.IronScrapPressed = Ids.Products.CreateId(nameof (IronScrapPressed));
        Ids.Products.MoltenIron = Ids.Products.CreateId(nameof (MoltenIron));
        Ids.Products.MoltenSteel = Ids.Products.CreateId(nameof (MoltenSteel));
        Ids.Products.Iron = Ids.Products.CreateId(nameof (Iron));
        Ids.Products.Steel = Ids.Products.CreateId(nameof (Steel));
        Ids.Products.CopperOre = Ids.Products.CreateId(nameof (CopperOre));
        Ids.Products.CopperOreParams = new LooseProductParam(Ids.TerrainMaterials.CopperOreDisrupted);
        Ids.Products.CopperOreCrushed = Ids.Products.CreateId(nameof (CopperOreCrushed));
        Ids.Products.CopperOreCrushedParams = new LooseProductParam(Ids.TerrainMaterials.CopperOreCrushed);
        Ids.Products.CopperScrap = new ProductProto.ID("Product_CopperScrap");
        Ids.Products.CopperScrapPressed = Ids.Products.CreateId(nameof (CopperScrapPressed));
        Ids.Products.MoltenCopper = Ids.Products.CreateId(nameof (MoltenCopper));
        Ids.Products.ImpureCopper = Ids.Products.CreateId(nameof (ImpureCopper));
        Ids.Products.Copper = Ids.Products.CreateId(nameof (Copper));
        Ids.Products.GoldOre = Ids.Products.CreateId(nameof (GoldOre));
        Ids.Products.GoldOreParams = new LooseProductParam(Ids.TerrainMaterials.GoldOreDisrupted);
        Ids.Products.GoldOreCrushed = Ids.Products.CreateId(nameof (GoldOreCrushed));
        Ids.Products.GoldOreCrushedParams = new LooseProductParam(Ids.TerrainMaterials.GoldOreCrushed);
        Ids.Products.GoldOrePowder = Ids.Products.CreateId(nameof (GoldOrePowder));
        Ids.Products.GoldOreConcentrate = Ids.Products.CreateId(nameof (GoldOreConcentrate));
        Ids.Products.Gold = Ids.Products.CreateId(nameof (Gold));
        Ids.Products.GoldScrap = new ProductProto.ID("Product_GoldScrap");
        Ids.Products.GoldScrapPressed = Ids.Products.CreateId(nameof (GoldScrapPressed));
        Ids.Products.Sand = Ids.Products.CreateId(nameof (Sand));
        Ids.Products.SandParams = new LooseProductParam(Ids.TerrainMaterials.Sand);
        Ids.Products.MoltenGlass = Ids.Products.CreateId(nameof (MoltenGlass));
        Ids.Products.BrokenGlass = new ProductProto.ID("Product_BrokenGlass");
        Ids.Products.Glass = Ids.Products.CreateId(nameof (Glass));
        Ids.Products.GlassMix = Ids.Products.CreateId(nameof (GlassMix));
        Ids.Products.UraniumOre = Ids.Products.CreateId(nameof (UraniumOre));
        Ids.Products.UraniumOreCrushed = Ids.Products.CreateId(nameof (UraniumOreCrushed));
        Ids.Products.Yellowcake = Ids.Products.CreateId(nameof (Yellowcake));
        Ids.Products.UraniumEnriched = Ids.Products.CreateId(nameof (UraniumEnriched));
        Ids.Products.UraniumEnriched20 = Ids.Products.CreateId(nameof (UraniumEnriched20));
        Ids.Products.UraniumReprocessed = Ids.Products.CreateId(nameof (UraniumReprocessed));
        Ids.Products.Plutonium = Ids.Products.CreateId(nameof (Plutonium));
        Ids.Products.UraniumDepleted = Ids.Products.CreateId(nameof (UraniumDepleted));
        Ids.Products.UraniumDepletedParams = new LooseProductParam(Ids.TerrainMaterials.UraniumDepleted);
        Ids.Products.UraniumRod = Ids.Products.CreateId(nameof (UraniumRod));
        Ids.Products.MoxRod = Ids.Products.CreateId(nameof (MoxRod));
        Ids.Products.SpentFuel = Ids.Products.CreateId(nameof (SpentFuel));
        Ids.Products.SpentMox = Ids.Products.CreateId(nameof (SpentMox));
        Ids.Products.FissionProduct = Ids.Products.CreateId(nameof (FissionProduct));
        Ids.Products.RetiredWaste = new ProductProto.ID("Product_RetiredWaste");
        Ids.Products.FissionProductParam = new RadioactiveWasteParam(100, new ProductProto.ID("Product_RetiredWaste"));
        Ids.Products.CoreFuel = Ids.Products.CreateId(nameof (CoreFuel));
        Ids.Products.CoreFuelDirty = Ids.Products.CreateId(nameof (CoreFuelDirty));
        Ids.Products.BlanketFuel = Ids.Products.CreateId(nameof (BlanketFuel));
        Ids.Products.BlanketFuelEnriched = Ids.Products.CreateId(nameof (BlanketFuelEnriched));
        Ids.Products.Quartz = Ids.Products.CreateId(nameof (Quartz));
        Ids.Products.QuartzParams = new LooseProductParam(Ids.TerrainMaterials.QuartzDisrupted);
        Ids.Products.QuartzCrushed = Ids.Products.CreateId(nameof (QuartzCrushed));
        Ids.Products.QuartzCrushedParams = new LooseProductParam(Ids.TerrainMaterials.QuartzCrushed);
        Ids.Products.MoltenSilicon = Ids.Products.CreateId(nameof (MoltenSilicon));
        Ids.Products.PolySilicon = Ids.Products.CreateId(nameof (PolySilicon));
        Ids.Products.SiliconWafer = Ids.Products.CreateId(nameof (SiliconWafer));
        Ids.Products.Water = IdsCore.Products.CleanWater;
        Ids.Products.ChilledWater = Ids.Products.CreateId(nameof (ChilledWater));
        Ids.Products.Seawater = Ids.Products.CreateId(nameof (Seawater));
        Ids.Products.Brine = Ids.Products.CreateId(nameof (Brine));
        Ids.Products.WasteWater = Ids.Products.CreateId(nameof (WasteWater));
        Ids.Products.ToxicSlurry = Ids.Products.CreateId(nameof (ToxicSlurry));
        Ids.Products.PollutedWater = IdsCore.Products.PollutedWater;
        Ids.Products.PollutedAir = IdsCore.Products.PollutedAir;
        Ids.Products.Chlorine = Ids.Products.CreateId(nameof (Chlorine));
        Ids.Products.SteamSp = Ids.Products.CreateId(nameof (SteamSp));
        Ids.Products.SteamHi = Ids.Products.CreateId(nameof (SteamHi));
        Ids.Products.SteamLo = Ids.Products.CreateId("SteamLP");
        Ids.Products.SteamDepleted = Ids.Products.CreateId(nameof (SteamDepleted));
        Ids.Products.Salt = Ids.Products.CreateId(nameof (Salt));
        Ids.Products.Exhaust = Ids.Products.CreateId(nameof (Exhaust));
        Ids.Products.Recyclables = IdsCore.Products.Recyclables;
        Ids.Products.RecyclablesPressed = Ids.Products.CreateId(nameof (RecyclablesPressed));
        Ids.Products.CrudeOil = Ids.Products.CreateId(nameof (CrudeOil));
        Ids.Products.Diesel = Ids.Products.CreateId(nameof (Diesel));
        Ids.Products.Naphtha = Ids.Products.CreateId(nameof (Naphtha));
        Ids.Products.FuelGas = Ids.Products.CreateId(nameof (FuelGas));
        Ids.Products.Sulfur = Ids.Products.CreateId(nameof (Sulfur));
        Ids.Products.SourWater = Ids.Products.CreateId(nameof (SourWater));
        Ids.Products.Ammonia = Ids.Products.CreateId(nameof (Ammonia));
        Ids.Products.Acid = Ids.Products.CreateId(nameof (Acid));
        Ids.Products.HeavyOil = Ids.Products.CreateId(nameof (HeavyOil));
        Ids.Products.MediumOil = Ids.Products.CreateId(nameof (MediumOil));
        Ids.Products.LightOil = Ids.Products.CreateId(nameof (LightOil));
        Ids.Products.Plastic = Ids.Products.CreateId(nameof (Plastic));
        Ids.Products.Rubber = Ids.Products.CreateId(nameof (Rubber));
        Ids.Products.Hydrogen = Ids.Products.CreateId(nameof (Hydrogen));
        Ids.Products.Nitrogen = Ids.Products.CreateId(nameof (Nitrogen));
        Ids.Products.CarbonDioxide = Ids.Products.CreateId(nameof (CarbonDioxide));
        Ids.Products.Ethanol = Ids.Products.CreateId(nameof (Ethanol));
        Ids.Products.Potato = Ids.Products.CreateId(nameof (Potato));
        Ids.Products.Corn = Ids.Products.CreateId(nameof (Corn));
        Ids.Products.Wheat = Ids.Products.CreateId(nameof (Wheat));
        Ids.Products.Soybean = Ids.Products.CreateId(nameof (Soybean));
        Ids.Products.SugarCane = Ids.Products.CreateId(nameof (SugarCane));
        Ids.Products.Canola = Ids.Products.CreateId(nameof (Canola));
        Ids.Products.Vegetables = Ids.Products.CreateId(nameof (Vegetables));
        Ids.Products.Fruit = Ids.Products.CreateId(nameof (Fruit));
        Ids.Products.Poppy = Ids.Products.CreateId(nameof (Poppy));
        Ids.Products.Bread = Ids.Products.CreateId(nameof (Bread));
        Ids.Products.Flour = Ids.Products.CreateId(nameof (Flour));
        Ids.Products.AnimalFeed = Ids.Products.CreateId(nameof (AnimalFeed));
        Ids.Products.Biomass = IdsCore.Products.Biomass;
        Ids.Products.Eggs = Ids.Products.CreateId(nameof (Eggs));
        Ids.Products.Tofu = Ids.Products.CreateId(nameof (Tofu));
        Ids.Products.CookingOil = Ids.Products.CreateId(nameof (CookingOil));
        Ids.Products.Meat = Ids.Products.CreateId(nameof (Meat));
        Ids.Products.Sausage = Ids.Products.CreateId(nameof (Sausage));
        Ids.Products.Snack = Ids.Products.CreateId(nameof (Snack));
        Ids.Products.Cake = Ids.Products.CreateId(nameof (Cake));
        Ids.Products.FoodPack = Ids.Products.CreateId(nameof (FoodPack));
        Ids.Products.Chicken = Ids.Products.CreateId(nameof (Chicken));
        Ids.Products.CargoShip = Ids.Products.CreateId(nameof (CargoShip));
        Ids.Products.ChickenCarcass = Ids.Products.CreateId(nameof (ChickenCarcass));
        Ids.Products.MeatTrimmings = Ids.Products.CreateId(nameof (MeatTrimmings));
        Ids.Products.Sugar = Ids.Products.CreateId(nameof (Sugar));
        Ids.Products.CornMash = Ids.Products.CreateId(nameof (CornMash));
        Ids.Products.HouseholdGoods = Ids.Products.CreateId(nameof (HouseholdGoods));
        Ids.Products.HouseholdAppliances = Ids.Products.CreateId(nameof (HouseholdAppliances));
        Ids.Products.ConsumerElectronics = Ids.Products.CreateId(nameof (ConsumerElectronics));
        Ids.Products.Antibiotics = Ids.Products.CreateId(nameof (Antibiotics));
        Ids.Products.Disinfectant = Ids.Products.CreateId(nameof (Disinfectant));
        Ids.Products.Anesthetics = Ids.Products.CreateId(nameof (Anesthetics));
        Ids.Products.Morphine = Ids.Products.CreateId(nameof (Morphine));
        Ids.Products.MedicalEquipment = Ids.Products.CreateId(nameof (MedicalEquipment));
        Ids.Products.MedicalSupplies = Ids.Products.CreateId(nameof (MedicalSupplies));
        Ids.Products.MedicalSupplies2 = Ids.Products.CreateId(nameof (MedicalSupplies2));
        Ids.Products.MedicalSupplies3 = Ids.Products.CreateId(nameof (MedicalSupplies3));
        Ids.Products.Cement = Ids.Products.CreateId(nameof (Cement));
        Ids.Products.ConcreteSlab = IdsCore.Products.ConcreteSlab;
        Ids.Products.Bricks = Ids.Products.CreateId(nameof (Bricks));
        Ids.Products.ConstructionParts = Ids.Products.CreateId(nameof (ConstructionParts));
        Ids.Products.ConstructionPartsInUpgrade = new AllowProductDiscountInUpgrade();
        Ids.Products.ConstructionParts2 = Ids.Products.CreateId(nameof (ConstructionParts2));
        Ids.Products.ConstructionParts2InUpgrade = new AllowProductDiscountInUpgrade();
        Ids.Products.ConstructionParts3 = Ids.Products.CreateId(nameof (ConstructionParts3));
        Ids.Products.ConstructionParts3InUpgrade = new AllowProductDiscountInUpgrade();
        Ids.Products.ConstructionParts4 = Ids.Products.CreateId(nameof (ConstructionParts4));
        Ids.Products.ConstructionParts4InUpgrade = new AllowProductDiscountInUpgrade();
        Ids.Products.MechanicalParts = Ids.Products.CreateId(nameof (MechanicalParts));
        Ids.Products.VehicleParts = Ids.Products.CreateId(nameof (VehicleParts));
        Ids.Products.VehicleParts2 = Ids.Products.CreateId(nameof (VehicleParts2));
        Ids.Products.VehicleParts3 = Ids.Products.CreateId(nameof (VehicleParts3));
        Ids.Products.LabEquipment = Ids.Products.CreateId(nameof (LabEquipment));
        Ids.Products.LabEquipment2 = Ids.Products.CreateId(nameof (LabEquipment2));
        Ids.Products.LabEquipment3 = Ids.Products.CreateId(nameof (LabEquipment3));
        Ids.Products.LabEquipment4 = Ids.Products.CreateId(nameof (LabEquipment4));
        Ids.Products.Electronics = Ids.Products.CreateId(nameof (Electronics));
        Ids.Products.PCB = Ids.Products.CreateId(nameof (PCB));
        Ids.Products.Electronics2 = Ids.Products.CreateId(nameof (Electronics2));
        Ids.Products.Electronics3 = Ids.Products.CreateId(nameof (Electronics3));
        Ids.Products.Server = Ids.Products.CreateId(nameof (Server));
        Ids.Products.Microchips = Ids.Products.CreateId(nameof (Microchips));
        Ids.Products.MicrochipsStage1A = Ids.Products.CreateId(nameof (MicrochipsStage1A));
        Ids.Products.MicrochipsStage1B = Ids.Products.CreateId(nameof (MicrochipsStage1B));
        Ids.Products.MicrochipsStage1C = Ids.Products.CreateId(nameof (MicrochipsStage1C));
        Ids.Products.MicrochipsStage2A = Ids.Products.CreateId(nameof (MicrochipsStage2A));
        Ids.Products.MicrochipsStage2B = Ids.Products.CreateId(nameof (MicrochipsStage2B));
        Ids.Products.MicrochipsStage2C = Ids.Products.CreateId(nameof (MicrochipsStage2C));
        Ids.Products.MicrochipsStage3A = Ids.Products.CreateId(nameof (MicrochipsStage3A));
        Ids.Products.MicrochipsStage3B = Ids.Products.CreateId(nameof (MicrochipsStage3B));
        Ids.Products.MicrochipsStage3C = Ids.Products.CreateId(nameof (MicrochipsStage3C));
        Ids.Products.MicrochipsStage4A = Ids.Products.CreateId(nameof (MicrochipsStage4A));
        Ids.Products.MicrochipsStage4B = Ids.Products.CreateId(nameof (MicrochipsStage4B));
        Ids.Products.SolarCell = Ids.Products.CreateId(nameof (SolarCell));
        Ids.Products.SolarCellMono = Ids.Products.CreateId(nameof (SolarCellMono));
        Ids.Products.Oxygen = Ids.Products.CreateId(nameof (Oxygen));
        Ids.Products.HydrogenFluoride = Ids.Products.CreateId(nameof (HydrogenFluoride));
        Ids.Products.Waste = IdsCore.Products.Waste;
        Ids.Products.WasteParams = new LooseProductParam(Ids.TerrainMaterials.Landfill);
        Ids.Products.WastePressed = Ids.Products.CreateId(nameof (WastePressed));
        Ids.Products.Flowers = Ids.Products.CreateId(nameof (Flowers));
      }
    }

    public static class Properties
    {
      public static readonly PropertyId<Percent> HouseholdGoodsConsumptionMultiplier;
      public static readonly PropertyId<Percent> HouseholdAppliancesConsumptionMultiplier;
      public static readonly PropertyId<Percent> ConsumerElectronicsConsumptionMultiplier;
      public static readonly PropertyId<Percent> SettlementWaterConsumptionMultiplier;
      public static readonly PropertyId<Percent> HouseholdGoodsUnityMultiplier;
      public static readonly PropertyId<Percent> HouseholdAppliancesUnityMultiplier;
      public static readonly PropertyId<Percent> ConsumerElectronicsUnityMultiplier;

      static Properties()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Properties.HouseholdGoodsConsumptionMultiplier = new PropertyId<Percent>(nameof (HouseholdGoodsConsumptionMultiplier));
        Ids.Properties.HouseholdAppliancesConsumptionMultiplier = new PropertyId<Percent>(nameof (HouseholdAppliancesConsumptionMultiplier));
        Ids.Properties.ConsumerElectronicsConsumptionMultiplier = new PropertyId<Percent>(nameof (ConsumerElectronicsConsumptionMultiplier));
        Ids.Properties.SettlementWaterConsumptionMultiplier = new PropertyId<Percent>(nameof (SettlementWaterConsumptionMultiplier));
        Ids.Properties.HouseholdGoodsUnityMultiplier = new PropertyId<Percent>(nameof (HouseholdGoodsUnityMultiplier));
        Ids.Properties.HouseholdAppliancesUnityMultiplier = new PropertyId<Percent>(nameof (HouseholdAppliancesUnityMultiplier));
        Ids.Properties.ConsumerElectronicsUnityMultiplier = new PropertyId<Percent>(nameof (ConsumerElectronicsUnityMultiplier));
      }
    }

    public static class Recipes
    {
      public const string ID_PREFIX = "Recipe_";
      public static readonly RecipeProto.ID WaterDumping;
      public static readonly RecipeProto.ID SeaWaterDumping;
      public static readonly RecipeProto.ID BrineDumping;
      public static readonly RecipeProto.ID WasteWaterDumping;
      public static readonly RecipeProto.ID SourWaterDumping;
      public static readonly RecipeProto.ID WasteAcidDumping;
      public static readonly RecipeProto.ID ToxicSlurryDumping;
      public static readonly RecipeProto.ID FertilizerOrganicDumping;
      public static readonly RecipeProto.ID FertilizerChem1Dumping;
      public static readonly RecipeProto.ID FertilizerChem2Dumping;
      public static readonly RecipeProto.ID OceanWaterPumping;
      public static readonly RecipeProto.ID OceanWaterPumpingT2;
      public static readonly RecipeProto.ID LandWaterPumping;
      public static readonly RecipeProto.ID CarbonDioxideInjection;
      public static readonly RecipeProto.ID WaterTreatment;
      public static readonly RecipeProto.ID WaterTreatmentT2;
      public static readonly RecipeProto.ID ToxicSlurryTreatment;
      public static readonly RecipeProto.ID SaltMakingFromBrine;
      public static readonly RecipeProto.ID SaltMaking;
      public static readonly RecipeProto.ID SaltMakingFromBrineHeated;
      public static readonly RecipeProto.ID SaltMakingHeated;
      public static readonly RecipeProto.ID BrineMaking;
      public static readonly RecipeProto.ID BrineMakingHeated;
      public static readonly RecipeProto.ID WaterElectrolysis;
      public static readonly RecipeProto.ID WaterElectrolysisT2;
      public static readonly RecipeProto.ID AmmoniaElectrolysis;
      public static readonly RecipeProto.ID AmmoniaElectrolysisT2;
      public static readonly RecipeProto.ID BrineElectrolysis;
      public static readonly RecipeProto.ID BrineElectrolysisT2;
      public static readonly RecipeProto.ID DesalinationFromSP;
      public static readonly RecipeProto.ID DesalinationFromHP;
      public static readonly RecipeProto.ID DesalinationFromLP;
      public static readonly RecipeProto.ID DesalinationFromDepleted;
      public static readonly RecipeProto.ID AirSeparation;
      public static readonly RecipeProto.ID CharcoalBurning;
      public static readonly RecipeProto.ID CpAssemblyT1;
      public static readonly RecipeProto.ID CpAssemblyT2;
      public static readonly RecipeProto.ID CpAssemblyT3;
      public static readonly RecipeProto.ID CpAssemblyT4;
      public static readonly RecipeProto.ID CpBricksAssemblyT1;
      public static readonly RecipeProto.ID CpBricksAssemblyT2;
      public static readonly RecipeProto.ID CpBricksAssemblyT3;
      public static readonly RecipeProto.ID CpBricksAssemblyT4;
      public static readonly RecipeProto.ID Cp2AssemblyT1;
      public static readonly RecipeProto.ID Cp2AssemblyT2;
      public static readonly RecipeProto.ID Cp2AssemblyT3;
      public static readonly RecipeProto.ID Cp2AssemblyT4;
      public static readonly RecipeProto.ID Cp3AssemblyT1;
      public static readonly RecipeProto.ID Cp3AssemblyT2;
      public static readonly RecipeProto.ID Cp3AssemblyT3;
      public static readonly RecipeProto.ID Cp4AssemblyElectrifiedT2;
      public static readonly RecipeProto.ID Cp4AssemblyRoboticT1;
      public static readonly RecipeProto.ID Cp4AssemblyRoboticT2;
      public static readonly RecipeProto.ID MechPartsAssemblyT1;
      public static readonly RecipeProto.ID MechPartsAssemblyT2;
      public static readonly RecipeProto.ID MechPartsAssemblyT3Iron;
      public static readonly RecipeProto.ID MechPartsAssemblyT3;
      public static readonly RecipeProto.ID MechPartsAssemblyT4;
      public static readonly RecipeProto.ID VehicleParts1AssemblyT1;
      public static readonly RecipeProto.ID VehicleParts1AssemblyT2;
      public static readonly RecipeProto.ID VehicleParts1AssemblyT3;
      public static readonly RecipeProto.ID VehicleParts1AssemblyT4;
      public static readonly RecipeProto.ID VehicleParts2AssemblyT1;
      public static readonly RecipeProto.ID VehicleParts2AssemblyT2;
      public static readonly RecipeProto.ID VehicleParts3AssemblyT1;
      public static readonly RecipeProto.ID VehicleParts3AssemblyT2;
      public static readonly RecipeProto.ID LabEquipment1AssemblyT1;
      public static readonly RecipeProto.ID LabEquipment1AssemblyT2;
      public static readonly RecipeProto.ID LabEquipment1AssemblyT3;
      public static readonly RecipeProto.ID LabEquipment2AssemblyT1;
      public static readonly RecipeProto.ID LabEquipment2AssemblyT2;
      public static readonly RecipeProto.ID LabEquipment3AssemblyT1;
      public static readonly RecipeProto.ID LabEquipment3AssemblyT2;
      public static readonly RecipeProto.ID LabEquipment4AssemblyT2;
      public static readonly RecipeProto.ID MaintenanceT0;
      public static readonly RecipeProto.ID MaintenanceT1;
      public static readonly RecipeProto.ID MaintenanceT1Recycling;
      public static readonly RecipeProto.ID MaintenanceT2;
      public static readonly RecipeProto.ID MaintenanceT2Recycling;
      public static readonly RecipeProto.ID MaintenanceT3;
      public static readonly RecipeProto.ID MaintenanceT3Recycling;
      public static readonly RecipeProto.ID BricksMaking;
      public static readonly RecipeProto.ID ConcreteMixingSlag;
      public static readonly RecipeProto.ID ConcreteMixingSlagM;
      public static readonly RecipeProto.ID ConcreteMixingGravel;
      public static readonly RecipeProto.ID ConcreteMixingGravelM;
      public static readonly RecipeProto.ID ConcreteMixingSlagT2;
      public static readonly RecipeProto.ID ConcreteMixingSlagMT2;
      public static readonly RecipeProto.ID ConcreteMixingGravelT2;
      public static readonly RecipeProto.ID ConcreteMixingGravelMT2;
      public static readonly RecipeProto.ID ConcreteMixingSlagT3;
      public static readonly RecipeProto.ID ConcreteMixingSlagMT3;
      public static readonly RecipeProto.ID ConcreteMixingGravelT3;
      public static readonly RecipeProto.ID ConcreteMixingGravelMT3;
      public static readonly RecipeProto.ID SlagCrushing;
      public static readonly RecipeProto.ID SlagCrushingT2;
      public static readonly RecipeProto.ID CementProduction;
      public static readonly RecipeProto.ID CementProductionGas;
      public static readonly RecipeProto.ID CementProductionHydrogen;
      public static readonly RecipeProto.ID SteamGenerationCoal;
      public static readonly RecipeProto.ID SteamGenerationWood;
      public static readonly RecipeProto.ID SteamGenerationBiomass;
      public static readonly RecipeProto.ID SteamGenerationAnimalFeed;
      public static readonly RecipeProto.ID SteamGenerationFuelGas;
      public static readonly RecipeProto.ID SteamGenerationHeavyOil;
      public static readonly RecipeProto.ID SteamGenerationMediumOil;
      public static readonly RecipeProto.ID SteamGenerationLightOil;
      public static readonly RecipeProto.ID SteamGenerationNaphtha;
      public static readonly RecipeProto.ID SteamGenerationHydrogen;
      public static readonly RecipeProto.ID SteamGenerationEthanol;
      public static readonly RecipeProto.ID SteamGenerationElectric;
      public static readonly RecipeProto.ID SteamGenerationSpElectric;
      public static readonly RecipeProto.ID SiliconSmeltingArc;
      public static readonly RecipeProto.ID SiliconSmeltingArc2;
      public static readonly RecipeProto.ID SiliconTreatment;
      public static readonly RecipeProto.ID SiliconCrystallization;
      public static readonly RecipeProto.ID QuartzCrushing;
      public static readonly RecipeProto.ID QuartzCrushingT2;
      public static readonly RecipeProto.ID QuartzMilling;
      public static readonly RecipeProto.ID QuartzMillingT2;
      public static readonly RecipeProto.ID RockCrushing;
      public static readonly RecipeProto.ID RockCrushingT2;
      public static readonly RecipeProto.ID GravelCrushing;
      public static readonly RecipeProto.ID GravelCrushingT2;
      public static readonly RecipeProto.ID IronOreCrushing;
      public static readonly RecipeProto.ID IronOreCrushingT2;
      public static readonly RecipeProto.ID IronSmeltingT1Scrap;
      public static readonly RecipeProto.ID IronSmeltingT1Coal;
      public static readonly RecipeProto.ID IronSmeltingT2Scrap;
      public static readonly RecipeProto.ID IronSmeltingT2;
      public static readonly RecipeProto.ID IronSmeltingArcScrap;
      public static readonly RecipeProto.ID IronSmeltingArc;
      public static readonly RecipeProto.ID SteelSmelting;
      public static readonly RecipeProto.ID SteelSmeltingT2;
      public static readonly RecipeProto.ID IronCasting;
      public static readonly RecipeProto.ID IronCastingT2;
      public static readonly RecipeProto.ID SteelCastingCooled;
      public static readonly RecipeProto.ID SteelCastingCooledT2;
      public static readonly RecipeProto.ID CopperSmeltingT1Scrap;
      public static readonly RecipeProto.ID CopperSmeltingT1;
      public static readonly RecipeProto.ID CopperSmeltingT2Scrap;
      public static readonly RecipeProto.ID CopperSmeltingT2;
      public static readonly RecipeProto.ID CopperSmeltingArcScrap;
      public static readonly RecipeProto.ID CopperSmeltingArc;
      public static readonly RecipeProto.ID CopperOreCrushing;
      public static readonly RecipeProto.ID CopperOreCrushingT2;
      public static readonly RecipeProto.ID CopperCasting;
      public static readonly RecipeProto.ID CopperCastingT2;
      public static readonly RecipeProto.ID CopperElectrolysisWithWater;
      public static readonly RecipeProto.ID CopperElectrolysis;
      public static readonly RecipeProto.ID GlassMixMixing;
      public static readonly RecipeProto.ID GlassMixMixingT2;
      public static readonly RecipeProto.ID GlassMixMixingWithAcid;
      public static readonly RecipeProto.ID GlassMixMixingWithAcidT2;
      public static readonly RecipeProto.ID GlassSmelting;
      public static readonly RecipeProto.ID GlassSmeltingT2;
      public static readonly RecipeProto.ID GlassSmeltingWithBroken;
      public static readonly RecipeProto.ID GlassSmeltingT2WithBroken;
      public static readonly RecipeProto.ID GlassSmeltingArc;
      public static readonly RecipeProto.ID GlassSmeltingArcWithBroken;
      public static readonly RecipeProto.ID GlassCastingT1;
      public static readonly RecipeProto.ID GlassCastingT2;
      public static readonly RecipeProto.ID AcidMixMixing;
      public static readonly RecipeProto.ID AcidMixMixingT2;
      public static readonly RecipeProto.ID BrineProduction;
      public static readonly RecipeProto.ID BrineProductionT2;
      public static readonly RecipeProto.ID SulfurNeutralization;
      public static readonly RecipeProto.ID SulfurNeutralizationT2;
      public static readonly RecipeProto.ID FilterMediaMixing;
      public static readonly RecipeProto.ID FilterMediaMixingM;
      public static readonly RecipeProto.ID FilterMediaMixingT2;
      public static readonly RecipeProto.ID FilterMediaMixingMT2;
      public static readonly RecipeProto.ID DirtMixing;
      public static readonly RecipeProto.ID DirtMixingT2;
      public static readonly RecipeProto.ID OrganicFertilizerProduction;
      public static readonly RecipeProto.ID OrganicFertilizerProductionT2;
      public static readonly RecipeProto.ID GoldOreCrushing;
      public static readonly RecipeProto.ID GoldOreCrushingT2;
      public static readonly RecipeProto.ID GoldOreMilling;
      public static readonly RecipeProto.ID GoldOreMillingT2;
      public static readonly RecipeProto.ID GoldSettling;
      public static readonly RecipeProto.ID GoldSmelting;
      public static readonly RecipeProto.ID GoldScrapSmelting;
      public static readonly RecipeProto.ID FluorideLeaching;
      public static readonly RecipeProto.ID UraniumLeaching;
      public static readonly RecipeProto.ID UraniumCrushing;
      public static readonly RecipeProto.ID UraniumCrushingT2;
      public static readonly RecipeProto.ID SmokeStackDepletedSteam;
      public static readonly RecipeProto.ID SmokeStackLargeDepletedSteam;
      public static readonly RecipeProto.ID SmokeStackLpSteam;
      public static readonly RecipeProto.ID SmokeStackLargeLpSteam;
      public static readonly RecipeProto.ID SmokeStackHpSteam;
      public static readonly RecipeProto.ID SmokeStackLargeHpSteam;
      public static readonly RecipeProto.ID SmokeStackSpSteam;
      public static readonly RecipeProto.ID SmokeStackLargeSpSteam;
      public static readonly RecipeProto.ID SmokeStackOxygen;
      public static readonly RecipeProto.ID SmokeStackLargeOxygen;
      public static readonly RecipeProto.ID SmokeStackNitrogen;
      public static readonly RecipeProto.ID SmokeStackLargeNitrogen;
      public static readonly RecipeProto.ID SmokeStackExhaust;
      public static readonly RecipeProto.ID SmokeStackLargeExhaust;
      public static readonly RecipeProto.ID SmokeStackCarbonDioxide;
      public static readonly RecipeProto.ID SmokeStackLargeCarbonDioxide;
      public static readonly RecipeProto.ID ExhaustFiltering;
      public static readonly RecipeProto.ID SteamHpCondensation;
      public static readonly RecipeProto.ID SteamLpCondensation;
      public static readonly RecipeProto.ID SteamDepletedCondensation;
      public static readonly RecipeProto.ID SteamSpCondensationT2;
      public static readonly RecipeProto.ID SteamHpCondensationT2;
      public static readonly RecipeProto.ID SteamLpCondensationT2;
      public static readonly RecipeProto.ID SteamDepletedCondensationT2;
      public static readonly RecipeProto.ID PCBAssemblyT1;
      public static readonly RecipeProto.ID PCBAssemblyT2;
      public static readonly RecipeProto.ID PCBAssemblyT3;
      public static readonly RecipeProto.ID ElectronicsAssemblyT1;
      public static readonly RecipeProto.ID ElectronicsAssemblyT2;
      public static readonly RecipeProto.ID ElectronicsAssemblyT3;
      public static readonly RecipeProto.ID ElectronicsAssemblyT4;
      public static readonly RecipeProto.ID ElectronicsAssemblyT5;
      public static readonly RecipeProto.ID Electronics2AssemblyT1;
      public static readonly RecipeProto.ID Electronics2AssemblyT2;
      public static readonly RecipeProto.ID Electronics2AssemblyT3;
      public static readonly RecipeProto.ID Electronics3AssemblyRoboticT1;
      public static readonly RecipeProto.ID Electronics3AssemblyRoboticT2;
      public static readonly RecipeProto.ID ServerAssemblyT1;
      public static readonly RecipeProto.ID ServerAssemblyT2;
      public static readonly RecipeProto.ID UraniumRodsAssemblyT1;
      public static readonly RecipeProto.ID UraniumEnrichedAssemblyT1;
      public static readonly RecipeProto.ID MoxRodsAssemblyT1;
      public static readonly RecipeProto.ID SolarCellAssemblyT1;
      public static readonly RecipeProto.ID SolarCellAssemblyT2;
      public static readonly RecipeProto.ID SolarCellAssemblyT3;
      public static readonly RecipeProto.ID SolarCellMonoAssemblyT1;
      public static readonly RecipeProto.ID MedicalEquipmentAssemblyT1;
      public static readonly RecipeProto.ID MedicalEquipmentAssemblyT2;
      public static readonly RecipeProto.ID MedicalSuppliesAssemblyT1;
      public static readonly RecipeProto.ID MedicalSuppliesAssemblyT2;
      public static readonly RecipeProto.ID MedicalSupplies2AssemblyT1;
      public static readonly RecipeProto.ID MedicalSupplies2AssemblyT2;
      public static readonly RecipeProto.ID MedicalSupplies3AssemblyT1;
      public static readonly RecipeProto.ID MedicalSupplies3AssemblyT2;
      public static readonly RecipeProto.ID FoodPackAssemblyMeat;
      public static readonly RecipeProto.ID FoodPackAssemblyEggs;
      public static readonly RecipeProto.ID BlanketFuelFromDepleted;
      public static readonly RecipeProto.ID BlanketFuelFromYellowcake;
      public static readonly RecipeProto.ID CoreFuelFromEnriched;
      public static readonly RecipeProto.ID SpentFuelReprocessing;
      public static readonly RecipeProto.ID SpentFuelToBlanket;
      public static readonly RecipeProto.ID SpentMoxToBlanket;
      public static readonly RecipeProto.ID CoreFuelReprocessing;
      public static readonly RecipeProto.ID DieselDistillationBasic;
      public static readonly RecipeProto.ID OilGroundPumping;
      public static readonly RecipeProto.ID WaterDesalinationBasic;
      public static readonly RecipeProto.ID CrudeOilRefiningT1;
      public static readonly RecipeProto.ID CrudeOilRefiningT2;
      public static readonly RecipeProto.ID HeavyDistillateRefining;
      public static readonly RecipeProto.ID HeavyOilCracking;
      public static readonly RecipeProto.ID HeavyOilCrackingToNaphtha;
      public static readonly RecipeProto.ID NaphthaReforming;
      public static readonly RecipeProto.ID DieselReforming;
      public static readonly RecipeProto.ID NaphthaReformingToGas;
      public static readonly RecipeProto.ID FuelGasReforming;
      public static readonly RecipeProto.ID HydrogenReforming;
      public static readonly RecipeProto.ID HydrogenProductionFromSteamSp;
      public static readonly RecipeProto.ID PlasticMaking;
      public static readonly RecipeProto.ID PlasticMakingEthanol;
      public static readonly RecipeProto.ID EthanolCookingOilReforming;
      public static readonly RecipeProto.ID EthanolCookingOilReformingT2;
      public static readonly RecipeProto.ID RubberProductionNaphtha;
      public static readonly RecipeProto.ID RubberProductionNaphthaAlt;
      public static readonly RecipeProto.ID RubberProductionEthanol;
      public static readonly RecipeProto.ID RubberProductionDiesel;
      public static readonly RecipeProto.ID RubberProductionDieselWithCoal;
      public static readonly RecipeProto.ID GraphiteProductionT1;
      public static readonly RecipeProto.ID GraphiteProductionT2;
      public static readonly RecipeProto.ID GraphiteProductionCo2;
      public static readonly RecipeProto.ID SourWaterStripping;
      public static readonly RecipeProto.ID FlareDiesel;
      public static readonly RecipeProto.ID FlareHydrogen;
      public static readonly RecipeProto.ID FlareHeavyOil;
      public static readonly RecipeProto.ID FlareLightOil;
      public static readonly RecipeProto.ID FlareNaphtha;
      public static readonly RecipeProto.ID FlareEthanol;
      public static readonly RecipeProto.ID FlareFuelGas;
      public static readonly RecipeProto.ID FlareAmmonia;
      public static readonly RecipeProto.ID LandfillBurning;
      public static readonly RecipeProto.ID BiomassBurning;
      public static readonly RecipeProto.ID AnimalFeedBurning;
      public static readonly RecipeProto.ID MeatTrimmingsBurning;
      public static readonly RecipeProto.ID ChickenCarcassBurning;
      public static readonly RecipeProto.ID SulfurBurning;
      public static readonly RecipeProto.ID SludgeBurning;
      public static readonly RecipeProto.ID WheatMilling;
      public static readonly RecipeProto.ID SoybeanMilling;
      public static readonly RecipeProto.ID CanolaMilling;
      public static readonly RecipeProto.ID CornMilling;
      public static readonly RecipeProto.ID BreadProduction;
      public static readonly RecipeProto.ID TofuProduction;
      public static readonly RecipeProto.ID MeatProcessing;
      public static readonly RecipeProto.ID MeatProcessingTrimmings;
      public static readonly RecipeProto.ID SausageProduction;
      public static readonly RecipeProto.ID CakeProduction;
      public static readonly RecipeProto.ID SnackProductionPotato;
      public static readonly RecipeProto.ID SnackProductionCorn;
      public static readonly RecipeProto.ID SugarRefiningCane;
      public static readonly RecipeProto.ID DisinfectantProduction;
      public static readonly RecipeProto.ID DisinfectantProductionT2;
      public static readonly RecipeProto.ID AnestheticsProduction;
      public static readonly RecipeProto.ID MorphineProduction;
      public static readonly RecipeProto.ID AnimalFeedFromSoybean;
      public static readonly RecipeProto.ID AnimalFeedFromSoybeanT2;
      public static readonly RecipeProto.ID AnimalFeedFromCorn;
      public static readonly RecipeProto.ID AnimalFeedFromCornT2;
      public static readonly RecipeProto.ID AnimalFeedFromPotato;
      public static readonly RecipeProto.ID AnimalFeedFromPotatoT2;
      public static readonly RecipeProto.ID AnimalFeedFromWheat;
      public static readonly RecipeProto.ID AnimalFeedFromWheatT2;
      public static readonly RecipeProto.ID FertilizerProduction;
      public static readonly RecipeProto.ID FertilizerProductionT2;
      public static readonly RecipeProto.ID Fertilizer2Production;
      public static readonly RecipeProto.ID Fertilizer2ProductionT2;
      public static readonly RecipeProto.ID FuelGasSynthesis;
      public static readonly RecipeProto.ID AmmoniaSynthesis;
      public static readonly RecipeProto.ID AmmoniaSynthesisT2;
      public static readonly RecipeProto.ID PaperProduction;
      public static readonly RecipeProto.ID PaperProductionT2;
      public static readonly RecipeProto.ID SludgeDigestion;
      public static readonly RecipeProto.ID PotatoDigestion;
      public static readonly RecipeProto.ID VegetablesDigestion;
      public static readonly RecipeProto.ID FruitDigestion;
      public static readonly RecipeProto.ID PoppyDigestion;
      public static readonly RecipeProto.ID WheatDigestion;
      public static readonly RecipeProto.ID MeatTrimmingsDigestion;
      public static readonly RecipeProto.ID AnimalFeedCompost;
      public static readonly RecipeProto.ID AnimalFeedCompostT2;
      public static readonly RecipeProto.ID BiomassCompost;
      public static readonly RecipeProto.ID BiomassCompostT2;
      public static readonly RecipeProto.ID MeatTrimmingsCompost;
      public static readonly RecipeProto.ID MeatTrimmingsCompostT2;
      public static readonly RecipeProto.ID UraniumEnrichment;
      public static readonly RecipeProto.ID UraniumEnrichment20;
      public static readonly RecipeProto.ID BlanketFuelReprocessing;
      public static readonly RecipeProto.ID ReprocessedUraniumEnrichment;
      public static readonly RecipeProto.ID SugarToEthanolFermentation;
      public static readonly RecipeProto.ID CornToEthanolFermentation;
      public static readonly RecipeProto.ID AntibioticsFermentation;
      public static readonly RecipeProto.ID HouseholdGoodsAssemblyT1;
      public static readonly RecipeProto.ID HouseholdGoodsAssemblyT2;
      public static readonly RecipeProto.ID HouseholdGoodsAssemblyT3;
      public static readonly RecipeProto.ID HouseholdAppliancesAssemblyT1;
      public static readonly RecipeProto.ID HouseholdAppliancesAssemblyT2;
      public static readonly RecipeProto.ID HouseholdAppliancesAssemblyT3;
      public static readonly RecipeProto.ID ConsumerElectronicsAssemblyT1;
      public static readonly RecipeProto.ID WaterChilling;
      public static readonly RecipeProto.ID PressingOfRecyclables;
      public static readonly RecipeProto.ID PressingOfIronScrap;
      public static readonly RecipeProto.ID PressingOfCopperScrap;
      public static readonly RecipeProto.ID PressingOfGoldScrap;
      public static readonly RecipeProto.ID PressingOfWaste;
      public static readonly RecipeProto.ID ShreddingWood;
      public static readonly RecipeProto.ID ShreddingSaplings;
      public static readonly RecipeProto.ID ShreddingIronScrap;
      public static readonly RecipeProto.ID ShreddingCopperScrap;
      public static readonly RecipeProto.ID ShreddingGoldScrap;
      public static readonly RecipeProto.ID ShreddingWaste;
      public static readonly RecipeProto.ID ShreddingRetiredWaste;
      public static readonly RecipeProto.ID ShreddingPolyCells;
      public static readonly RecipeProto.ID IncinerationOfWaste;
      public static readonly RecipeProto.ID IncinerationOfWasteHydrogen;
      public static readonly RecipeProto.ID IncinerationOfWastePressed;
      public static readonly RecipeProto.ID IncinerationOfWastePressedHydrogen;

      public static RecipeProto.ID CreateId(string id) => new RecipeProto.ID("Recipe_" + id);

      public static RecipeProto.ID GetMicrochipManufacturingId(int stage, Proto.ID machineId)
      {
        return new RecipeProto.ID(string.Format("{0}_MicrochipProdStage{1}{2}", (object) machineId, (object) (stage % 3 + 1), (object) (char) (65 + stage / 3)));
      }

      static Recipes()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Recipes.WaterDumping = new RecipeProto.ID("OceanWaterDumping");
        Ids.Recipes.SeaWaterDumping = new RecipeProto.ID(nameof (SeaWaterDumping));
        Ids.Recipes.BrineDumping = new RecipeProto.ID(nameof (BrineDumping));
        Ids.Recipes.WasteWaterDumping = new RecipeProto.ID("OceanWasteWaterPumping");
        Ids.Recipes.SourWaterDumping = new RecipeProto.ID(nameof (SourWaterDumping));
        Ids.Recipes.WasteAcidDumping = new RecipeProto.ID(nameof (WasteAcidDumping));
        Ids.Recipes.ToxicSlurryDumping = new RecipeProto.ID(nameof (ToxicSlurryDumping));
        Ids.Recipes.FertilizerOrganicDumping = new RecipeProto.ID(nameof (FertilizerOrganicDumping));
        Ids.Recipes.FertilizerChem1Dumping = new RecipeProto.ID(nameof (FertilizerChem1Dumping));
        Ids.Recipes.FertilizerChem2Dumping = new RecipeProto.ID(nameof (FertilizerChem2Dumping));
        Ids.Recipes.OceanWaterPumping = new RecipeProto.ID(nameof (OceanWaterPumping));
        Ids.Recipes.OceanWaterPumpingT2 = new RecipeProto.ID(nameof (OceanWaterPumpingT2));
        Ids.Recipes.LandWaterPumping = new RecipeProto.ID(nameof (LandWaterPumping));
        Ids.Recipes.CarbonDioxideInjection = new RecipeProto.ID(nameof (CarbonDioxideInjection));
        Ids.Recipes.WaterTreatment = new RecipeProto.ID(nameof (WaterTreatment));
        Ids.Recipes.WaterTreatmentT2 = new RecipeProto.ID(nameof (WaterTreatmentT2));
        Ids.Recipes.ToxicSlurryTreatment = new RecipeProto.ID(nameof (ToxicSlurryTreatment));
        Ids.Recipes.SaltMakingFromBrine = new RecipeProto.ID(nameof (SaltMakingFromBrine));
        Ids.Recipes.SaltMaking = new RecipeProto.ID(nameof (SaltMaking));
        Ids.Recipes.SaltMakingFromBrineHeated = new RecipeProto.ID(nameof (SaltMakingFromBrineHeated));
        Ids.Recipes.SaltMakingHeated = new RecipeProto.ID(nameof (SaltMakingHeated));
        Ids.Recipes.BrineMaking = new RecipeProto.ID(nameof (BrineMaking));
        Ids.Recipes.BrineMakingHeated = new RecipeProto.ID(nameof (BrineMakingHeated));
        Ids.Recipes.WaterElectrolysis = new RecipeProto.ID(nameof (WaterElectrolysis));
        Ids.Recipes.WaterElectrolysisT2 = new RecipeProto.ID(nameof (WaterElectrolysisT2));
        Ids.Recipes.AmmoniaElectrolysis = new RecipeProto.ID(nameof (AmmoniaElectrolysis));
        Ids.Recipes.AmmoniaElectrolysisT2 = new RecipeProto.ID(nameof (AmmoniaElectrolysisT2));
        Ids.Recipes.BrineElectrolysis = new RecipeProto.ID(nameof (BrineElectrolysis));
        Ids.Recipes.BrineElectrolysisT2 = new RecipeProto.ID(nameof (BrineElectrolysisT2));
        Ids.Recipes.DesalinationFromSP = new RecipeProto.ID(nameof (DesalinationFromSP));
        Ids.Recipes.DesalinationFromHP = new RecipeProto.ID(nameof (DesalinationFromHP));
        Ids.Recipes.DesalinationFromLP = new RecipeProto.ID(nameof (DesalinationFromLP));
        Ids.Recipes.DesalinationFromDepleted = new RecipeProto.ID(nameof (DesalinationFromDepleted));
        Ids.Recipes.AirSeparation = new RecipeProto.ID(nameof (AirSeparation));
        Ids.Recipes.CharcoalBurning = new RecipeProto.ID(nameof (CharcoalBurning));
        Ids.Recipes.CpAssemblyT1 = new RecipeProto.ID(nameof (CpAssemblyT1));
        Ids.Recipes.CpAssemblyT2 = new RecipeProto.ID(nameof (CpAssemblyT2));
        Ids.Recipes.CpAssemblyT3 = new RecipeProto.ID(nameof (CpAssemblyT3));
        Ids.Recipes.CpAssemblyT4 = new RecipeProto.ID(nameof (CpAssemblyT4));
        Ids.Recipes.CpBricksAssemblyT1 = new RecipeProto.ID(nameof (CpBricksAssemblyT1));
        Ids.Recipes.CpBricksAssemblyT2 = new RecipeProto.ID(nameof (CpBricksAssemblyT2));
        Ids.Recipes.CpBricksAssemblyT3 = new RecipeProto.ID(nameof (CpBricksAssemblyT3));
        Ids.Recipes.CpBricksAssemblyT4 = new RecipeProto.ID(nameof (CpBricksAssemblyT4));
        Ids.Recipes.Cp2AssemblyT1 = new RecipeProto.ID(nameof (Cp2AssemblyT1));
        Ids.Recipes.Cp2AssemblyT2 = new RecipeProto.ID(nameof (Cp2AssemblyT2));
        Ids.Recipes.Cp2AssemblyT3 = new RecipeProto.ID(nameof (Cp2AssemblyT3));
        Ids.Recipes.Cp2AssemblyT4 = new RecipeProto.ID(nameof (Cp2AssemblyT4));
        Ids.Recipes.Cp3AssemblyT1 = new RecipeProto.ID(nameof (Cp3AssemblyT1));
        Ids.Recipes.Cp3AssemblyT2 = new RecipeProto.ID(nameof (Cp3AssemblyT2));
        Ids.Recipes.Cp3AssemblyT3 = new RecipeProto.ID(nameof (Cp3AssemblyT3));
        Ids.Recipes.Cp4AssemblyElectrifiedT2 = new RecipeProto.ID(nameof (Cp4AssemblyElectrifiedT2));
        Ids.Recipes.Cp4AssemblyRoboticT1 = new RecipeProto.ID(nameof (Cp4AssemblyRoboticT1));
        Ids.Recipes.Cp4AssemblyRoboticT2 = new RecipeProto.ID(nameof (Cp4AssemblyRoboticT2));
        Ids.Recipes.MechPartsAssemblyT1 = new RecipeProto.ID(nameof (MechPartsAssemblyT1));
        Ids.Recipes.MechPartsAssemblyT2 = new RecipeProto.ID(nameof (MechPartsAssemblyT2));
        Ids.Recipes.MechPartsAssemblyT3Iron = new RecipeProto.ID(nameof (MechPartsAssemblyT3Iron));
        Ids.Recipes.MechPartsAssemblyT3 = new RecipeProto.ID(nameof (MechPartsAssemblyT3));
        Ids.Recipes.MechPartsAssemblyT4 = new RecipeProto.ID(nameof (MechPartsAssemblyT4));
        Ids.Recipes.VehicleParts1AssemblyT1 = new RecipeProto.ID(nameof (VehicleParts1AssemblyT1));
        Ids.Recipes.VehicleParts1AssemblyT2 = new RecipeProto.ID(nameof (VehicleParts1AssemblyT2));
        Ids.Recipes.VehicleParts1AssemblyT3 = new RecipeProto.ID(nameof (VehicleParts1AssemblyT3));
        Ids.Recipes.VehicleParts1AssemblyT4 = new RecipeProto.ID(nameof (VehicleParts1AssemblyT4));
        Ids.Recipes.VehicleParts2AssemblyT1 = new RecipeProto.ID(nameof (VehicleParts2AssemblyT1));
        Ids.Recipes.VehicleParts2AssemblyT2 = new RecipeProto.ID(nameof (VehicleParts2AssemblyT2));
        Ids.Recipes.VehicleParts3AssemblyT1 = new RecipeProto.ID(nameof (VehicleParts3AssemblyT1));
        Ids.Recipes.VehicleParts3AssemblyT2 = new RecipeProto.ID(nameof (VehicleParts3AssemblyT2));
        Ids.Recipes.LabEquipment1AssemblyT1 = new RecipeProto.ID(nameof (LabEquipment1AssemblyT1));
        Ids.Recipes.LabEquipment1AssemblyT2 = new RecipeProto.ID(nameof (LabEquipment1AssemblyT2));
        Ids.Recipes.LabEquipment1AssemblyT3 = new RecipeProto.ID(nameof (LabEquipment1AssemblyT3));
        Ids.Recipes.LabEquipment2AssemblyT1 = new RecipeProto.ID(nameof (LabEquipment2AssemblyT2));
        Ids.Recipes.LabEquipment2AssemblyT2 = new RecipeProto.ID("LabEquipment2AssemblyT3");
        Ids.Recipes.LabEquipment3AssemblyT1 = new RecipeProto.ID(nameof (LabEquipment3AssemblyT1));
        Ids.Recipes.LabEquipment3AssemblyT2 = new RecipeProto.ID(nameof (LabEquipment3AssemblyT2));
        Ids.Recipes.LabEquipment4AssemblyT2 = new RecipeProto.ID(nameof (LabEquipment4AssemblyT2));
        Ids.Recipes.MaintenanceT0 = new RecipeProto.ID("MaintenanceT0Recipe");
        Ids.Recipes.MaintenanceT1 = new RecipeProto.ID("MaintenanceT1Recipe");
        Ids.Recipes.MaintenanceT1Recycling = new RecipeProto.ID(nameof (MaintenanceT1Recycling));
        Ids.Recipes.MaintenanceT2 = new RecipeProto.ID("MaintenanceT2Recipe");
        Ids.Recipes.MaintenanceT2Recycling = new RecipeProto.ID(nameof (MaintenanceT2Recycling));
        Ids.Recipes.MaintenanceT3 = new RecipeProto.ID("MaintenanceT3Recipe");
        Ids.Recipes.MaintenanceT3Recycling = new RecipeProto.ID(nameof (MaintenanceT3Recycling));
        Ids.Recipes.BricksMaking = new RecipeProto.ID(nameof (BricksMaking));
        Ids.Recipes.ConcreteMixingSlag = new RecipeProto.ID(nameof (ConcreteMixingSlag));
        Ids.Recipes.ConcreteMixingSlagM = new RecipeProto.ID(nameof (ConcreteMixingSlagM));
        Ids.Recipes.ConcreteMixingGravel = new RecipeProto.ID(nameof (ConcreteMixingGravel));
        Ids.Recipes.ConcreteMixingGravelM = new RecipeProto.ID(nameof (ConcreteMixingGravelM));
        Ids.Recipes.ConcreteMixingSlagT2 = new RecipeProto.ID(nameof (ConcreteMixingSlagT2));
        Ids.Recipes.ConcreteMixingSlagMT2 = new RecipeProto.ID(nameof (ConcreteMixingSlagMT2));
        Ids.Recipes.ConcreteMixingGravelT2 = new RecipeProto.ID(nameof (ConcreteMixingGravelT2));
        Ids.Recipes.ConcreteMixingGravelMT2 = new RecipeProto.ID(nameof (ConcreteMixingGravelMT2));
        Ids.Recipes.ConcreteMixingSlagT3 = new RecipeProto.ID(nameof (ConcreteMixingSlagT3));
        Ids.Recipes.ConcreteMixingSlagMT3 = new RecipeProto.ID(nameof (ConcreteMixingSlagMT3));
        Ids.Recipes.ConcreteMixingGravelT3 = new RecipeProto.ID(nameof (ConcreteMixingGravelT3));
        Ids.Recipes.ConcreteMixingGravelMT3 = new RecipeProto.ID(nameof (ConcreteMixingGravelMT3));
        Ids.Recipes.SlagCrushing = new RecipeProto.ID(nameof (SlagCrushing));
        Ids.Recipes.SlagCrushingT2 = new RecipeProto.ID(nameof (SlagCrushingT2));
        Ids.Recipes.CementProduction = new RecipeProto.ID(nameof (CementProduction));
        Ids.Recipes.CementProductionGas = new RecipeProto.ID(nameof (CementProductionGas));
        Ids.Recipes.CementProductionHydrogen = new RecipeProto.ID(nameof (CementProductionHydrogen));
        Ids.Recipes.SteamGenerationCoal = new RecipeProto.ID(nameof (SteamGenerationCoal));
        Ids.Recipes.SteamGenerationWood = new RecipeProto.ID(nameof (SteamGenerationWood));
        Ids.Recipes.SteamGenerationBiomass = new RecipeProto.ID(nameof (SteamGenerationBiomass));
        Ids.Recipes.SteamGenerationAnimalFeed = new RecipeProto.ID(nameof (SteamGenerationAnimalFeed));
        Ids.Recipes.SteamGenerationFuelGas = new RecipeProto.ID(nameof (SteamGenerationFuelGas));
        Ids.Recipes.SteamGenerationHeavyOil = new RecipeProto.ID(nameof (SteamGenerationHeavyOil));
        Ids.Recipes.SteamGenerationMediumOil = new RecipeProto.ID(nameof (SteamGenerationMediumOil));
        Ids.Recipes.SteamGenerationLightOil = new RecipeProto.ID(nameof (SteamGenerationLightOil));
        Ids.Recipes.SteamGenerationNaphtha = new RecipeProto.ID(nameof (SteamGenerationNaphtha));
        Ids.Recipes.SteamGenerationHydrogen = new RecipeProto.ID(nameof (SteamGenerationHydrogen));
        Ids.Recipes.SteamGenerationEthanol = new RecipeProto.ID(nameof (SteamGenerationEthanol));
        Ids.Recipes.SteamGenerationElectric = new RecipeProto.ID(nameof (SteamGenerationElectric));
        Ids.Recipes.SteamGenerationSpElectric = new RecipeProto.ID(nameof (SteamGenerationSpElectric));
        Ids.Recipes.SiliconSmeltingArc = new RecipeProto.ID(nameof (SiliconSmeltingArc));
        Ids.Recipes.SiliconSmeltingArc2 = new RecipeProto.ID(nameof (SiliconSmeltingArc2));
        Ids.Recipes.SiliconTreatment = new RecipeProto.ID(nameof (SiliconTreatment));
        Ids.Recipes.SiliconCrystallization = new RecipeProto.ID(nameof (SiliconCrystallization));
        Ids.Recipes.QuartzCrushing = new RecipeProto.ID(nameof (QuartzCrushing));
        Ids.Recipes.QuartzCrushingT2 = new RecipeProto.ID(nameof (QuartzCrushingT2));
        Ids.Recipes.QuartzMilling = new RecipeProto.ID(nameof (QuartzMilling));
        Ids.Recipes.QuartzMillingT2 = new RecipeProto.ID(nameof (QuartzMillingT2));
        Ids.Recipes.RockCrushing = new RecipeProto.ID(nameof (RockCrushing));
        Ids.Recipes.RockCrushingT2 = new RecipeProto.ID(nameof (RockCrushingT2));
        Ids.Recipes.GravelCrushing = new RecipeProto.ID(nameof (GravelCrushing));
        Ids.Recipes.GravelCrushingT2 = new RecipeProto.ID(nameof (GravelCrushingT2));
        Ids.Recipes.IronOreCrushing = new RecipeProto.ID(nameof (IronOreCrushing));
        Ids.Recipes.IronOreCrushingT2 = new RecipeProto.ID(nameof (IronOreCrushingT2));
        Ids.Recipes.IronSmeltingT1Scrap = new RecipeProto.ID(nameof (IronSmeltingT1Scrap));
        Ids.Recipes.IronSmeltingT1Coal = new RecipeProto.ID(nameof (IronSmeltingT1Coal));
        Ids.Recipes.IronSmeltingT2Scrap = new RecipeProto.ID(nameof (IronSmeltingT2Scrap));
        Ids.Recipes.IronSmeltingT2 = new RecipeProto.ID(nameof (IronSmeltingT2));
        Ids.Recipes.IronSmeltingArcScrap = new RecipeProto.ID(nameof (IronSmeltingArcScrap));
        Ids.Recipes.IronSmeltingArc = new RecipeProto.ID(nameof (IronSmeltingArc));
        Ids.Recipes.SteelSmelting = new RecipeProto.ID(nameof (SteelSmelting));
        Ids.Recipes.SteelSmeltingT2 = new RecipeProto.ID(nameof (SteelSmeltingT2));
        Ids.Recipes.IronCasting = new RecipeProto.ID(nameof (IronCasting));
        Ids.Recipes.IronCastingT2 = new RecipeProto.ID("IronCastingCooled");
        Ids.Recipes.SteelCastingCooled = new RecipeProto.ID(nameof (SteelCastingCooled));
        Ids.Recipes.SteelCastingCooledT2 = new RecipeProto.ID(nameof (SteelCastingCooledT2));
        Ids.Recipes.CopperSmeltingT1Scrap = new RecipeProto.ID(nameof (CopperSmeltingT1Scrap));
        Ids.Recipes.CopperSmeltingT1 = new RecipeProto.ID(nameof (CopperSmeltingT1));
        Ids.Recipes.CopperSmeltingT2Scrap = new RecipeProto.ID(nameof (CopperSmeltingT2Scrap));
        Ids.Recipes.CopperSmeltingT2 = new RecipeProto.ID(nameof (CopperSmeltingT2));
        Ids.Recipes.CopperSmeltingArcScrap = new RecipeProto.ID(nameof (CopperSmeltingArcScrap));
        Ids.Recipes.CopperSmeltingArc = new RecipeProto.ID(nameof (CopperSmeltingArc));
        Ids.Recipes.CopperOreCrushing = new RecipeProto.ID(nameof (CopperOreCrushing));
        Ids.Recipes.CopperOreCrushingT2 = new RecipeProto.ID(nameof (CopperOreCrushingT2));
        Ids.Recipes.CopperCasting = new RecipeProto.ID(nameof (CopperCasting));
        Ids.Recipes.CopperCastingT2 = new RecipeProto.ID("CopperCastingCooled");
        Ids.Recipes.CopperElectrolysisWithWater = new RecipeProto.ID(nameof (CopperElectrolysisWithWater));
        Ids.Recipes.CopperElectrolysis = new RecipeProto.ID("CopperElectrolysisProcess");
        Ids.Recipes.GlassMixMixing = new RecipeProto.ID(nameof (GlassMixMixing));
        Ids.Recipes.GlassMixMixingT2 = new RecipeProto.ID(nameof (GlassMixMixingT2));
        Ids.Recipes.GlassMixMixingWithAcid = new RecipeProto.ID(nameof (GlassMixMixingWithAcid));
        Ids.Recipes.GlassMixMixingWithAcidT2 = new RecipeProto.ID(nameof (GlassMixMixingWithAcidT2));
        Ids.Recipes.GlassSmelting = new RecipeProto.ID(nameof (GlassSmelting));
        Ids.Recipes.GlassSmeltingT2 = new RecipeProto.ID(nameof (GlassSmeltingT2));
        Ids.Recipes.GlassSmeltingWithBroken = new RecipeProto.ID(nameof (GlassSmeltingWithBroken));
        Ids.Recipes.GlassSmeltingT2WithBroken = new RecipeProto.ID(nameof (GlassSmeltingT2WithBroken));
        Ids.Recipes.GlassSmeltingArc = new RecipeProto.ID(nameof (GlassSmeltingArc));
        Ids.Recipes.GlassSmeltingArcWithBroken = new RecipeProto.ID(nameof (GlassSmeltingArcWithBroken));
        Ids.Recipes.GlassCastingT1 = new RecipeProto.ID(nameof (GlassCastingT1));
        Ids.Recipes.GlassCastingT2 = new RecipeProto.ID(nameof (GlassCastingT2));
        Ids.Recipes.AcidMixMixing = new RecipeProto.ID(nameof (AcidMixMixing));
        Ids.Recipes.AcidMixMixingT2 = new RecipeProto.ID(nameof (AcidMixMixingT2));
        Ids.Recipes.BrineProduction = new RecipeProto.ID(nameof (BrineProduction));
        Ids.Recipes.BrineProductionT2 = new RecipeProto.ID(nameof (BrineProductionT2));
        Ids.Recipes.SulfurNeutralization = new RecipeProto.ID(nameof (SulfurNeutralization));
        Ids.Recipes.SulfurNeutralizationT2 = new RecipeProto.ID(nameof (SulfurNeutralizationT2));
        Ids.Recipes.FilterMediaMixing = new RecipeProto.ID(nameof (FilterMediaMixing));
        Ids.Recipes.FilterMediaMixingM = new RecipeProto.ID(nameof (FilterMediaMixingM));
        Ids.Recipes.FilterMediaMixingT2 = new RecipeProto.ID(nameof (FilterMediaMixingT2));
        Ids.Recipes.FilterMediaMixingMT2 = new RecipeProto.ID(nameof (FilterMediaMixingMT2));
        Ids.Recipes.DirtMixing = new RecipeProto.ID(nameof (DirtMixing));
        Ids.Recipes.DirtMixingT2 = new RecipeProto.ID(nameof (DirtMixingT2));
        Ids.Recipes.OrganicFertilizerProduction = new RecipeProto.ID(nameof (OrganicFertilizerProduction));
        Ids.Recipes.OrganicFertilizerProductionT2 = new RecipeProto.ID(nameof (OrganicFertilizerProductionT2));
        Ids.Recipes.GoldOreCrushing = new RecipeProto.ID(nameof (GoldOreCrushing));
        Ids.Recipes.GoldOreCrushingT2 = new RecipeProto.ID(nameof (GoldOreCrushingT2));
        Ids.Recipes.GoldOreMilling = new RecipeProto.ID("GoldMilling");
        Ids.Recipes.GoldOreMillingT2 = new RecipeProto.ID(nameof (GoldOreMillingT2));
        Ids.Recipes.GoldSettling = new RecipeProto.ID(nameof (GoldSettling));
        Ids.Recipes.GoldSmelting = new RecipeProto.ID(nameof (GoldSmelting));
        Ids.Recipes.GoldScrapSmelting = new RecipeProto.ID(nameof (GoldScrapSmelting));
        Ids.Recipes.FluorideLeaching = new RecipeProto.ID(nameof (FluorideLeaching));
        Ids.Recipes.UraniumLeaching = new RecipeProto.ID(nameof (UraniumLeaching));
        Ids.Recipes.UraniumCrushing = new RecipeProto.ID(nameof (UraniumCrushing));
        Ids.Recipes.UraniumCrushingT2 = new RecipeProto.ID(nameof (UraniumCrushingT2));
        Ids.Recipes.SmokeStackDepletedSteam = new RecipeProto.ID(nameof (SmokeStackDepletedSteam));
        Ids.Recipes.SmokeStackLargeDepletedSteam = new RecipeProto.ID(nameof (SmokeStackLargeDepletedSteam));
        Ids.Recipes.SmokeStackLpSteam = new RecipeProto.ID(nameof (SmokeStackLpSteam));
        Ids.Recipes.SmokeStackLargeLpSteam = new RecipeProto.ID(nameof (SmokeStackLargeLpSteam));
        Ids.Recipes.SmokeStackHpSteam = new RecipeProto.ID(nameof (SmokeStackHpSteam));
        Ids.Recipes.SmokeStackLargeHpSteam = new RecipeProto.ID(nameof (SmokeStackLargeHpSteam));
        Ids.Recipes.SmokeStackSpSteam = new RecipeProto.ID(nameof (SmokeStackSpSteam));
        Ids.Recipes.SmokeStackLargeSpSteam = new RecipeProto.ID(nameof (SmokeStackLargeSpSteam));
        Ids.Recipes.SmokeStackOxygen = new RecipeProto.ID(nameof (SmokeStackOxygen));
        Ids.Recipes.SmokeStackLargeOxygen = new RecipeProto.ID(nameof (SmokeStackLargeOxygen));
        Ids.Recipes.SmokeStackNitrogen = new RecipeProto.ID(nameof (SmokeStackNitrogen));
        Ids.Recipes.SmokeStackLargeNitrogen = new RecipeProto.ID(nameof (SmokeStackLargeNitrogen));
        Ids.Recipes.SmokeStackExhaust = new RecipeProto.ID(nameof (SmokeStackExhaust));
        Ids.Recipes.SmokeStackLargeExhaust = new RecipeProto.ID(nameof (SmokeStackLargeExhaust));
        Ids.Recipes.SmokeStackCarbonDioxide = new RecipeProto.ID(nameof (SmokeStackCarbonDioxide));
        Ids.Recipes.SmokeStackLargeCarbonDioxide = new RecipeProto.ID(nameof (SmokeStackLargeCarbonDioxide));
        Ids.Recipes.ExhaustFiltering = new RecipeProto.ID(nameof (ExhaustFiltering));
        Ids.Recipes.SteamHpCondensation = new RecipeProto.ID(nameof (SteamHpCondensation));
        Ids.Recipes.SteamLpCondensation = new RecipeProto.ID(nameof (SteamLpCondensation));
        Ids.Recipes.SteamDepletedCondensation = new RecipeProto.ID(nameof (SteamDepletedCondensation));
        Ids.Recipes.SteamSpCondensationT2 = new RecipeProto.ID(nameof (SteamSpCondensationT2));
        Ids.Recipes.SteamHpCondensationT2 = new RecipeProto.ID(nameof (SteamHpCondensationT2));
        Ids.Recipes.SteamLpCondensationT2 = new RecipeProto.ID(nameof (SteamLpCondensationT2));
        Ids.Recipes.SteamDepletedCondensationT2 = new RecipeProto.ID(nameof (SteamDepletedCondensationT2));
        Ids.Recipes.PCBAssemblyT1 = new RecipeProto.ID(nameof (PCBAssemblyT1));
        Ids.Recipes.PCBAssemblyT2 = new RecipeProto.ID(nameof (PCBAssemblyT2));
        Ids.Recipes.PCBAssemblyT3 = new RecipeProto.ID(nameof (PCBAssemblyT3));
        Ids.Recipes.ElectronicsAssemblyT1 = new RecipeProto.ID(nameof (ElectronicsAssemblyT1));
        Ids.Recipes.ElectronicsAssemblyT2 = new RecipeProto.ID(nameof (ElectronicsAssemblyT2));
        Ids.Recipes.ElectronicsAssemblyT3 = new RecipeProto.ID(nameof (ElectronicsAssemblyT3));
        Ids.Recipes.ElectronicsAssemblyT4 = new RecipeProto.ID(nameof (ElectronicsAssemblyT4));
        Ids.Recipes.ElectronicsAssemblyT5 = new RecipeProto.ID(nameof (ElectronicsAssemblyT5));
        Ids.Recipes.Electronics2AssemblyT1 = new RecipeProto.ID(nameof (Electronics2AssemblyT1));
        Ids.Recipes.Electronics2AssemblyT2 = new RecipeProto.ID(nameof (Electronics2AssemblyT2));
        Ids.Recipes.Electronics2AssemblyT3 = new RecipeProto.ID(nameof (Electronics2AssemblyT3));
        Ids.Recipes.Electronics3AssemblyRoboticT1 = new RecipeProto.ID(nameof (Electronics3AssemblyRoboticT1));
        Ids.Recipes.Electronics3AssemblyRoboticT2 = new RecipeProto.ID(nameof (Electronics3AssemblyRoboticT2));
        Ids.Recipes.ServerAssemblyT1 = new RecipeProto.ID(nameof (ServerAssemblyT1));
        Ids.Recipes.ServerAssemblyT2 = new RecipeProto.ID(nameof (ServerAssemblyT2));
        Ids.Recipes.UraniumRodsAssemblyT1 = new RecipeProto.ID(nameof (UraniumRodsAssemblyT1));
        Ids.Recipes.UraniumEnrichedAssemblyT1 = new RecipeProto.ID(nameof (UraniumEnrichedAssemblyT1));
        Ids.Recipes.MoxRodsAssemblyT1 = new RecipeProto.ID(nameof (MoxRodsAssemblyT1));
        Ids.Recipes.SolarCellAssemblyT1 = new RecipeProto.ID(nameof (SolarCellAssemblyT1));
        Ids.Recipes.SolarCellAssemblyT2 = new RecipeProto.ID(nameof (SolarCellAssemblyT2));
        Ids.Recipes.SolarCellAssemblyT3 = new RecipeProto.ID(nameof (SolarCellAssemblyT3));
        Ids.Recipes.SolarCellMonoAssemblyT1 = new RecipeProto.ID(nameof (SolarCellMonoAssemblyT1));
        Ids.Recipes.MedicalEquipmentAssemblyT1 = new RecipeProto.ID(nameof (MedicalEquipmentAssemblyT1));
        Ids.Recipes.MedicalEquipmentAssemblyT2 = new RecipeProto.ID(nameof (MedicalEquipmentAssemblyT2));
        Ids.Recipes.MedicalSuppliesAssemblyT1 = new RecipeProto.ID(nameof (MedicalSuppliesAssemblyT1));
        Ids.Recipes.MedicalSuppliesAssemblyT2 = new RecipeProto.ID(nameof (MedicalSuppliesAssemblyT2));
        Ids.Recipes.MedicalSupplies2AssemblyT1 = new RecipeProto.ID(nameof (MedicalSupplies2AssemblyT1));
        Ids.Recipes.MedicalSupplies2AssemblyT2 = new RecipeProto.ID(nameof (MedicalSupplies2AssemblyT2));
        Ids.Recipes.MedicalSupplies3AssemblyT1 = new RecipeProto.ID(nameof (MedicalSupplies3AssemblyT1));
        Ids.Recipes.MedicalSupplies3AssemblyT2 = new RecipeProto.ID(nameof (MedicalSupplies3AssemblyT2));
        Ids.Recipes.FoodPackAssemblyMeat = new RecipeProto.ID(nameof (FoodPackAssemblyMeat));
        Ids.Recipes.FoodPackAssemblyEggs = new RecipeProto.ID(nameof (FoodPackAssemblyEggs));
        Ids.Recipes.BlanketFuelFromDepleted = new RecipeProto.ID(nameof (BlanketFuelFromDepleted));
        Ids.Recipes.BlanketFuelFromYellowcake = new RecipeProto.ID(nameof (BlanketFuelFromYellowcake));
        Ids.Recipes.CoreFuelFromEnriched = new RecipeProto.ID(nameof (CoreFuelFromEnriched));
        Ids.Recipes.SpentFuelReprocessing = new RecipeProto.ID(nameof (SpentFuelReprocessing));
        Ids.Recipes.SpentFuelToBlanket = new RecipeProto.ID(nameof (SpentFuelToBlanket));
        Ids.Recipes.SpentMoxToBlanket = new RecipeProto.ID(nameof (SpentMoxToBlanket));
        Ids.Recipes.CoreFuelReprocessing = new RecipeProto.ID(nameof (CoreFuelReprocessing));
        Ids.Recipes.DieselDistillationBasic = new RecipeProto.ID(nameof (DieselDistillationBasic));
        Ids.Recipes.OilGroundPumping = new RecipeProto.ID(nameof (OilGroundPumping));
        Ids.Recipes.WaterDesalinationBasic = new RecipeProto.ID(nameof (WaterDesalinationBasic));
        Ids.Recipes.CrudeOilRefiningT1 = new RecipeProto.ID(nameof (CrudeOilRefiningT1));
        Ids.Recipes.CrudeOilRefiningT2 = new RecipeProto.ID(nameof (CrudeOilRefiningT2));
        Ids.Recipes.HeavyDistillateRefining = new RecipeProto.ID(nameof (HeavyDistillateRefining));
        Ids.Recipes.HeavyOilCracking = new RecipeProto.ID(nameof (HeavyOilCracking));
        Ids.Recipes.HeavyOilCrackingToNaphtha = new RecipeProto.ID(nameof (HeavyOilCrackingToNaphtha));
        Ids.Recipes.NaphthaReforming = new RecipeProto.ID(nameof (NaphthaReforming));
        Ids.Recipes.DieselReforming = new RecipeProto.ID(nameof (DieselReforming));
        Ids.Recipes.NaphthaReformingToGas = new RecipeProto.ID(nameof (NaphthaReformingToGas));
        Ids.Recipes.FuelGasReforming = new RecipeProto.ID(nameof (FuelGasReforming));
        Ids.Recipes.HydrogenReforming = new RecipeProto.ID(nameof (HydrogenReforming));
        Ids.Recipes.HydrogenProductionFromSteamSp = new RecipeProto.ID(nameof (HydrogenProductionFromSteamSp));
        Ids.Recipes.PlasticMaking = new RecipeProto.ID(nameof (PlasticMaking));
        Ids.Recipes.PlasticMakingEthanol = new RecipeProto.ID(nameof (PlasticMakingEthanol));
        Ids.Recipes.EthanolCookingOilReforming = new RecipeProto.ID(nameof (EthanolCookingOilReforming));
        Ids.Recipes.EthanolCookingOilReformingT2 = new RecipeProto.ID(nameof (EthanolCookingOilReformingT2));
        Ids.Recipes.RubberProductionNaphtha = new RecipeProto.ID(nameof (RubberProductionNaphtha));
        Ids.Recipes.RubberProductionNaphthaAlt = new RecipeProto.ID(nameof (RubberProductionNaphthaAlt));
        Ids.Recipes.RubberProductionEthanol = new RecipeProto.ID(nameof (RubberProductionEthanol));
        Ids.Recipes.RubberProductionDiesel = new RecipeProto.ID(nameof (RubberProductionDiesel));
        Ids.Recipes.RubberProductionDieselWithCoal = new RecipeProto.ID(nameof (RubberProductionDieselWithCoal));
        Ids.Recipes.GraphiteProductionT1 = new RecipeProto.ID(nameof (GraphiteProductionT1));
        Ids.Recipes.GraphiteProductionT2 = new RecipeProto.ID(nameof (GraphiteProductionT2));
        Ids.Recipes.GraphiteProductionCo2 = new RecipeProto.ID(nameof (GraphiteProductionCo2));
        Ids.Recipes.SourWaterStripping = new RecipeProto.ID(nameof (SourWaterStripping));
        Ids.Recipes.FlareDiesel = new RecipeProto.ID(nameof (FlareDiesel));
        Ids.Recipes.FlareHydrogen = new RecipeProto.ID(nameof (FlareHydrogen));
        Ids.Recipes.FlareHeavyOil = new RecipeProto.ID(nameof (FlareHeavyOil));
        Ids.Recipes.FlareLightOil = new RecipeProto.ID(nameof (FlareLightOil));
        Ids.Recipes.FlareNaphtha = new RecipeProto.ID(nameof (FlareNaphtha));
        Ids.Recipes.FlareEthanol = new RecipeProto.ID(nameof (FlareEthanol));
        Ids.Recipes.FlareFuelGas = new RecipeProto.ID(nameof (FlareFuelGas));
        Ids.Recipes.FlareAmmonia = new RecipeProto.ID(nameof (FlareAmmonia));
        Ids.Recipes.LandfillBurning = new RecipeProto.ID(nameof (LandfillBurning));
        Ids.Recipes.BiomassBurning = new RecipeProto.ID(nameof (BiomassBurning));
        Ids.Recipes.AnimalFeedBurning = new RecipeProto.ID(nameof (AnimalFeedBurning));
        Ids.Recipes.MeatTrimmingsBurning = new RecipeProto.ID(nameof (MeatTrimmingsBurning));
        Ids.Recipes.ChickenCarcassBurning = new RecipeProto.ID(nameof (ChickenCarcassBurning));
        Ids.Recipes.SulfurBurning = new RecipeProto.ID(nameof (SulfurBurning));
        Ids.Recipes.SludgeBurning = new RecipeProto.ID(nameof (SludgeBurning));
        Ids.Recipes.WheatMilling = new RecipeProto.ID(nameof (WheatMilling));
        Ids.Recipes.SoybeanMilling = new RecipeProto.ID(nameof (SoybeanMilling));
        Ids.Recipes.CanolaMilling = new RecipeProto.ID(nameof (CanolaMilling));
        Ids.Recipes.CornMilling = new RecipeProto.ID(nameof (CornMilling));
        Ids.Recipes.BreadProduction = new RecipeProto.ID(nameof (BreadProduction));
        Ids.Recipes.TofuProduction = new RecipeProto.ID(nameof (TofuProduction));
        Ids.Recipes.MeatProcessing = new RecipeProto.ID(nameof (MeatProcessing));
        Ids.Recipes.MeatProcessingTrimmings = new RecipeProto.ID(nameof (MeatProcessingTrimmings));
        Ids.Recipes.SausageProduction = new RecipeProto.ID(nameof (SausageProduction));
        Ids.Recipes.CakeProduction = new RecipeProto.ID(nameof (CakeProduction));
        Ids.Recipes.SnackProductionPotato = new RecipeProto.ID(nameof (SnackProductionPotato));
        Ids.Recipes.SnackProductionCorn = new RecipeProto.ID(nameof (SnackProductionCorn));
        Ids.Recipes.SugarRefiningCane = new RecipeProto.ID(nameof (SugarRefiningCane));
        Ids.Recipes.DisinfectantProduction = new RecipeProto.ID(nameof (DisinfectantProduction));
        Ids.Recipes.DisinfectantProductionT2 = new RecipeProto.ID(nameof (DisinfectantProductionT2));
        Ids.Recipes.AnestheticsProduction = new RecipeProto.ID(nameof (AnestheticsProduction));
        Ids.Recipes.MorphineProduction = new RecipeProto.ID(nameof (MorphineProduction));
        Ids.Recipes.AnimalFeedFromSoybean = new RecipeProto.ID(nameof (AnimalFeedFromSoybean));
        Ids.Recipes.AnimalFeedFromSoybeanT2 = new RecipeProto.ID(nameof (AnimalFeedFromSoybeanT2));
        Ids.Recipes.AnimalFeedFromCorn = new RecipeProto.ID(nameof (AnimalFeedFromCorn));
        Ids.Recipes.AnimalFeedFromCornT2 = new RecipeProto.ID(nameof (AnimalFeedFromCornT2));
        Ids.Recipes.AnimalFeedFromPotato = new RecipeProto.ID(nameof (AnimalFeedFromPotato));
        Ids.Recipes.AnimalFeedFromPotatoT2 = new RecipeProto.ID(nameof (AnimalFeedFromPotatoT2));
        Ids.Recipes.AnimalFeedFromWheat = new RecipeProto.ID(nameof (AnimalFeedFromWheat));
        Ids.Recipes.AnimalFeedFromWheatT2 = new RecipeProto.ID(nameof (AnimalFeedFromWheatT2));
        Ids.Recipes.FertilizerProduction = new RecipeProto.ID(nameof (FertilizerProduction));
        Ids.Recipes.FertilizerProductionT2 = new RecipeProto.ID(nameof (FertilizerProductionT2));
        Ids.Recipes.Fertilizer2Production = new RecipeProto.ID(nameof (Fertilizer2Production));
        Ids.Recipes.Fertilizer2ProductionT2 = new RecipeProto.ID(nameof (Fertilizer2ProductionT2));
        Ids.Recipes.FuelGasSynthesis = new RecipeProto.ID(nameof (FuelGasSynthesis));
        Ids.Recipes.AmmoniaSynthesis = new RecipeProto.ID(nameof (AmmoniaSynthesis));
        Ids.Recipes.AmmoniaSynthesisT2 = new RecipeProto.ID(nameof (AmmoniaSynthesisT2));
        Ids.Recipes.PaperProduction = new RecipeProto.ID(nameof (PaperProduction));
        Ids.Recipes.PaperProductionT2 = new RecipeProto.ID(nameof (PaperProductionT2));
        Ids.Recipes.SludgeDigestion = new RecipeProto.ID(nameof (SludgeDigestion));
        Ids.Recipes.PotatoDigestion = new RecipeProto.ID(nameof (PotatoDigestion));
        Ids.Recipes.VegetablesDigestion = new RecipeProto.ID(nameof (VegetablesDigestion));
        Ids.Recipes.FruitDigestion = new RecipeProto.ID(nameof (FruitDigestion));
        Ids.Recipes.PoppyDigestion = new RecipeProto.ID(nameof (PoppyDigestion));
        Ids.Recipes.WheatDigestion = new RecipeProto.ID(nameof (WheatDigestion));
        Ids.Recipes.MeatTrimmingsDigestion = new RecipeProto.ID(nameof (MeatTrimmingsDigestion));
        Ids.Recipes.AnimalFeedCompost = new RecipeProto.ID(nameof (AnimalFeedCompost));
        Ids.Recipes.AnimalFeedCompostT2 = new RecipeProto.ID(nameof (AnimalFeedCompostT2));
        Ids.Recipes.BiomassCompost = new RecipeProto.ID(nameof (BiomassCompost));
        Ids.Recipes.BiomassCompostT2 = new RecipeProto.ID(nameof (BiomassCompostT2));
        Ids.Recipes.MeatTrimmingsCompost = new RecipeProto.ID(nameof (MeatTrimmingsCompost));
        Ids.Recipes.MeatTrimmingsCompostT2 = new RecipeProto.ID(nameof (MeatTrimmingsCompostT2));
        Ids.Recipes.UraniumEnrichment = new RecipeProto.ID(nameof (UraniumEnrichment));
        Ids.Recipes.UraniumEnrichment20 = new RecipeProto.ID(nameof (UraniumEnrichment20));
        Ids.Recipes.BlanketFuelReprocessing = new RecipeProto.ID(nameof (BlanketFuelReprocessing));
        Ids.Recipes.ReprocessedUraniumEnrichment = new RecipeProto.ID(nameof (ReprocessedUraniumEnrichment));
        Ids.Recipes.SugarToEthanolFermentation = new RecipeProto.ID(nameof (SugarToEthanolFermentation));
        Ids.Recipes.CornToEthanolFermentation = new RecipeProto.ID(nameof (CornToEthanolFermentation));
        Ids.Recipes.AntibioticsFermentation = new RecipeProto.ID(nameof (AntibioticsFermentation));
        Ids.Recipes.HouseholdGoodsAssemblyT1 = new RecipeProto.ID(nameof (HouseholdGoodsAssemblyT1));
        Ids.Recipes.HouseholdGoodsAssemblyT2 = new RecipeProto.ID(nameof (HouseholdGoodsAssemblyT2));
        Ids.Recipes.HouseholdGoodsAssemblyT3 = new RecipeProto.ID(nameof (HouseholdGoodsAssemblyT3));
        Ids.Recipes.HouseholdAppliancesAssemblyT1 = new RecipeProto.ID(nameof (HouseholdAppliancesAssemblyT1));
        Ids.Recipes.HouseholdAppliancesAssemblyT2 = new RecipeProto.ID(nameof (HouseholdAppliancesAssemblyT2));
        Ids.Recipes.HouseholdAppliancesAssemblyT3 = new RecipeProto.ID(nameof (HouseholdAppliancesAssemblyT3));
        Ids.Recipes.ConsumerElectronicsAssemblyT1 = new RecipeProto.ID(nameof (ConsumerElectronicsAssemblyT1));
        Ids.Recipes.WaterChilling = new RecipeProto.ID(nameof (WaterChilling));
        Ids.Recipes.PressingOfRecyclables = new RecipeProto.ID(nameof (PressingOfRecyclables));
        Ids.Recipes.PressingOfIronScrap = new RecipeProto.ID(nameof (PressingOfIronScrap));
        Ids.Recipes.PressingOfCopperScrap = new RecipeProto.ID(nameof (PressingOfCopperScrap));
        Ids.Recipes.PressingOfGoldScrap = new RecipeProto.ID(nameof (PressingOfGoldScrap));
        Ids.Recipes.PressingOfWaste = new RecipeProto.ID(nameof (PressingOfWaste));
        Ids.Recipes.ShreddingWood = new RecipeProto.ID(nameof (ShreddingWood));
        Ids.Recipes.ShreddingSaplings = new RecipeProto.ID(nameof (ShreddingSaplings));
        Ids.Recipes.ShreddingIronScrap = new RecipeProto.ID(nameof (ShreddingIronScrap));
        Ids.Recipes.ShreddingCopperScrap = new RecipeProto.ID(nameof (ShreddingCopperScrap));
        Ids.Recipes.ShreddingGoldScrap = new RecipeProto.ID(nameof (ShreddingGoldScrap));
        Ids.Recipes.ShreddingWaste = new RecipeProto.ID(nameof (ShreddingWaste));
        Ids.Recipes.ShreddingRetiredWaste = new RecipeProto.ID(nameof (ShreddingRetiredWaste));
        Ids.Recipes.ShreddingPolyCells = new RecipeProto.ID(nameof (ShreddingPolyCells));
        Ids.Recipes.IncinerationOfWaste = new RecipeProto.ID(nameof (IncinerationOfWaste));
        Ids.Recipes.IncinerationOfWasteHydrogen = new RecipeProto.ID(nameof (IncinerationOfWasteHydrogen));
        Ids.Recipes.IncinerationOfWastePressed = new RecipeProto.ID(nameof (IncinerationOfWastePressed));
        Ids.Recipes.IncinerationOfWastePressedHydrogen = new RecipeProto.ID(nameof (IncinerationOfWastePressedHydrogen));
      }
    }

    public static class Research
    {
      public const string ID_PREFIX = "Research";
      [ResearchCosts(0)]
      public static readonly ResearchNodeProto.ID IronSmeltingScrap;
      [ResearchCosts(0)]
      public static readonly ResearchNodeProto.ID PipeTransports;
      [ResearchCosts(0)]
      public static readonly ResearchNodeProto.ID SettlementWaste;
      [ResearchCosts(0)]
      public static readonly ResearchNodeProto.ID Electricity;
      [ResearchCosts(0)]
      public static readonly ResearchNodeProto.ID BasicFarming;
      [ResearchCosts(1)]
      public static readonly ResearchNodeProto.ID CpPacking;
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID VehicleAndMining;
      [ResearchCosts(1)]
      public static readonly ResearchNodeProto.ID TradeDock;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID SettlementWater;
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID BasicDiesel;
      [OnlyForSaveCompatibility("Rename to Maintenance")]
      [ResearchCosts(1)]
      public static readonly ResearchNodeProto.ID PowerAndMaintenance;
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID Beacon;
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID BricksProduction;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID RetainingWalls;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID Cp2Packing;
      [ResearchCosts(3)]
      public static readonly ResearchNodeProto.ID CaptainsOffice;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID CaptainsOffice2;
      [ResearchCosts(1)]
      public static readonly ResearchNodeProto.ID Tools;
      [ResearchCosts(3)]
      public static readonly ResearchNodeProto.ID Blueprints;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID TerrainLeveling;
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID CopperRefinement;
      [OnlyForSaveCompatibility(null)]
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID StoragesT1;
      [ResearchCosts(3)]
      public static readonly ResearchNodeProto.ID ConcreteAdvanced;
      [ResearchCosts(3)]
      public static readonly ResearchNodeProto.ID CustomSurfaces;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID ResearchLab2;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID MaintenanceDepot;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID ConveyorBelts;
      [ResearchCosts(10)]
      public static readonly ResearchNodeProto.ID ResearchLab3;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID ShipWeapons;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID ShipArmor;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID ShipRadar;
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID RepairDock;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID FarmingT2;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID TreePlanting;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID CropRotation;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID Vegetables;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID FuelStation;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID VehicleCapIncrease;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID VehicleCapIncrease2;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID RecyclingEdict;
      [ResearchCosts(19)]
      public static readonly ResearchNodeProto.ID RecyclingEdict2;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID RecyclingIncrease;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID DeconstructionRatioIncrease;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID DeconstructionRatioIncrease2;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID PowerGeneration2;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID DieselGeneratorLarge;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID SettlementPower;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID TransportsBalancing;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID TransportsLifts;
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID VehicleRamps;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID Housing2;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID Edicts1;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID Edicts2;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID Edicts3;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID Edicts4;
      [ResearchCosts(5)]
      [OnlyForSaveCompatibility(null)]
      public static readonly ResearchNodeProto.ID MechPowerStorage;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID Stacker;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID FoodMarket2;
      [ResearchCosts(4)]
      public static readonly ResearchNodeProto.ID UndergroundWater;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID CrudeOilDistillation;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID Biofuel;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID CargoDepot;
      [ResearchCosts(8)]
      public static readonly ResearchNodeProto.ID SulfurProcessing;
      [ResearchCosts(7)]
      public static readonly ResearchNodeProto.ID GasCombustion;
      [ResearchCosts(8)]
      public static readonly ResearchNodeProto.ID CopperRefinement2;
      [ResearchCosts(8)]
      public static readonly ResearchNodeProto.ID SteelSmelting;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID Cp3Packing;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID AdvancedLogisticsControl;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID PipeTransportsT2;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID ConveyorBeltsT2;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID SaltProduction;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID ThermalDesalinationBasic;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID ThermalDesalination;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID IncinerationPlant;
      [ResearchCosts(7)]
      public static readonly ResearchNodeProto.ID CornCrop;
      [ResearchCosts(8)]
      public static readonly ResearchNodeProto.ID ChemicalPlant;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID Fertilizers;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID ChemicalPlant2;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID Fermentation;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID BioDiesel;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID Hospital;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID MedicalSupplies2;
      [ResearchCosts(38)]
      public static readonly ResearchNodeProto.ID MedicalSupplies3;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID Electrolysis;
      [ResearchCosts(15)]
      public static readonly ResearchNodeProto.ID Electrolysis2;
      [ResearchCosts(16)]
      public static readonly ResearchNodeProto.ID VacuumDesalination;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID RotaryKilnGas;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID WaterTreatment;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID WaterTreatment2;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID ExhaustFiltration;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID GlassSmelting;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID HouseholdGoods;
      [ResearchCosts(7)]
      public static readonly ResearchNodeProto.ID Storage2;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID Storage3;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID Storage4;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID VehicleCapIncrease3;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID VehicleCapIncrease4;
      [ResearchCosts(10)]
      public static readonly ResearchNodeProto.ID TrucksCapacityEdict;
      [ResearchCosts(2)]
      public static readonly ResearchNodeProto.ID RubberProduction;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID Recycling;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID Compactor;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID RecyclingForSettlement;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID NaphthaProcessing;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID VehicleAssembly2;
      [ResearchCosts(15)]
      public static readonly ResearchNodeProto.ID HydrogenCell;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID VehicleAssembly3;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID VehicleAssembly3H;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID FuelStation2;
      [ResearchCosts(16)]
      public static readonly ResearchNodeProto.ID FuelStation3;
      [OnlyForSaveCompatibility(null)]
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID TreeHarvester;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID ResearchLab4;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID Engine2;
      [ResearchCosts(16)]
      public static readonly ResearchNodeProto.ID ShipWeapons2;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID ShipFuelTankUpgrade;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID ShipArmor2;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID ShipRadar2;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID PowerGeneration3;
      [ResearchCosts(38)]
      public static readonly ResearchNodeProto.ID CarbonDioxideRecycling;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID Fruit;
      [ResearchCosts(7)]
      public static readonly ResearchNodeProto.ID OrganicFertilizer;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID PlasticProduction;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID HydrogenReforming;
      [ResearchCosts(8)]
      public static readonly ResearchNodeProto.ID WaterRecovery;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID ThermalStorage;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID AdvancedSmelting;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID CrusherLarge;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID IndustrialMixerT2;
      [ResearchCosts(19)]
      public static readonly ResearchNodeProto.ID GlassSmeltingT2;
      [ResearchCosts(9)]
      public static readonly ResearchNodeProto.ID CargoDepot2;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID CargoDepot3;
      [ResearchCosts(36)]
      public static readonly ResearchNodeProto.ID CargoDepot4;
      [ResearchCosts(16)]
      public static readonly ResearchNodeProto.ID PolySiliconProduction;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID VehicleCapIncrease5;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID NuclearReactor;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID UraniumEnrichment;
      [ResearchCosts(34)]
      public static readonly ResearchNodeProto.ID NuclearReactor2;
      [ResearchCosts(56)]
      public static readonly ResearchNodeProto.ID NuclearReactor3;
      [ResearchCosts(26)]
      public static readonly ResearchNodeProto.ID BasicComputing;
      [ResearchCosts(30)]
      public static readonly ResearchNodeProto.ID Datacenter;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID StatueOfMaintenance;
      [ResearchCosts(16)]
      public static readonly ResearchNodeProto.ID Cp4Packing;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID PipeTransportsT3;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID ConveyorBeltsT3;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID ConveyorRouting;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID Housing3;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID SettlementDecorations;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID FarmingT3;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID NaphthaReforming;
      [OnlyForSaveCompatibility(null)]
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID HeavyOilCracking;
      [ResearchCosts(21)]
      public static readonly ResearchNodeProto.ID FarmingT4;
      [ResearchCosts(5)]
      public static readonly ResearchNodeProto.ID Burner;
      [ResearchCosts(6)]
      public static readonly ResearchNodeProto.ID WheatCrop;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID SugarCane;
      [ResearchCosts(13)]
      public static readonly ResearchNodeProto.ID Canola;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID ChickenFarm;
      [ResearchCosts(14)]
      public static readonly ResearchNodeProto.ID SnacksProduction;
      [ResearchCosts(21)]
      public static readonly ResearchNodeProto.ID SausageProduction;
      [ResearchCosts(12)]
      public static readonly ResearchNodeProto.ID FoodPacking;
      [ResearchCosts(8)]
      public static readonly ResearchNodeProto.ID SoybeanCrop;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID TombOfCaptains;
      [ResearchCosts(32)]
      public static readonly ResearchNodeProto.ID ResearchLab5;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID PowerGeneration4;
      [ResearchCosts(22)]
      public static readonly ResearchNodeProto.ID BoilerElectric;
      [ResearchCosts(38)]
      public static readonly ResearchNodeProto.ID ArcFurnace2;
      [ResearchCosts(40)]
      public static readonly ResearchNodeProto.ID MicrochipProduction2;
      [ResearchCosts(38)]
      public static readonly ResearchNodeProto.ID Engine3;
      [ResearchCosts(36)]
      public static readonly ResearchNodeProto.ID VehicleCapIncrease6;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID HouseholdAppliances;
      [ResearchCosts(42)]
      public static readonly ResearchNodeProto.ID ConsumerElectronics;
      [ResearchCosts(20)]
      public static readonly ResearchNodeProto.ID GoldSmelting;
      [ResearchCosts(40)]
      public static readonly ResearchNodeProto.ID ShipWeapons3;
      [ResearchCosts(30)]
      public static readonly ResearchNodeProto.ID MicrochipProduction;
      [ResearchCosts(40)]
      public static readonly ResearchNodeProto.ID Assembler3;
      [ResearchCosts(52)]
      public static readonly ResearchNodeProto.ID RocketAssemblyAndLaunch;
      [ResearchCosts(18)]
      public static readonly ResearchNodeProto.ID SolarPanels;
      [ResearchCosts(42)]
      public static readonly ResearchNodeProto.ID SolarPanels2;
      [ResearchCosts(42)]
      public static readonly ResearchNodeProto.ID SuperPressSteam;

      public static ResearchNodeProto.ID CreateId(string id)
      {
        return new ResearchNodeProto.ID(nameof (Research) + id);
      }

      static Research()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Research.IronSmeltingScrap = Ids.Research.CreateId(nameof (IronSmeltingScrap));
        Ids.Research.PipeTransports = Ids.Research.CreateId(nameof (PipeTransports));
        Ids.Research.SettlementWaste = Ids.Research.CreateId(nameof (SettlementWaste));
        Ids.Research.Electricity = Ids.Research.CreateId(nameof (Electricity));
        Ids.Research.BasicFarming = Ids.Research.CreateId(nameof (BasicFarming));
        Ids.Research.CpPacking = Ids.Research.CreateId(nameof (CpPacking));
        Ids.Research.VehicleAndMining = Ids.Research.CreateId(nameof (VehicleAndMining));
        Ids.Research.TradeDock = Ids.Research.CreateId(nameof (TradeDock));
        Ids.Research.SettlementWater = Ids.Research.CreateId(nameof (SettlementWater));
        Ids.Research.BasicDiesel = Ids.Research.CreateId(nameof (BasicDiesel));
        Ids.Research.PowerAndMaintenance = Ids.Research.CreateId(nameof (PowerAndMaintenance));
        Ids.Research.Beacon = Ids.Research.CreateId(nameof (Beacon));
        Ids.Research.BricksProduction = Ids.Research.CreateId(nameof (BricksProduction));
        Ids.Research.RetainingWalls = Ids.Research.CreateId(nameof (RetainingWalls));
        Ids.Research.Cp2Packing = Ids.Research.CreateId(nameof (Cp2Packing));
        Ids.Research.CaptainsOffice = Ids.Research.CreateId(nameof (CaptainsOffice));
        Ids.Research.CaptainsOffice2 = Ids.Research.CreateId(nameof (CaptainsOffice2));
        Ids.Research.Tools = Ids.Research.CreateId(nameof (Tools));
        Ids.Research.Blueprints = Ids.Research.CreateId(nameof (Blueprints));
        Ids.Research.TerrainLeveling = Ids.Research.CreateId(nameof (TerrainLeveling));
        Ids.Research.CopperRefinement = Ids.Research.CreateId(nameof (CopperRefinement));
        Ids.Research.StoragesT1 = Ids.Research.CreateId(nameof (StoragesT1));
        Ids.Research.ConcreteAdvanced = Ids.Research.CreateId(nameof (ConcreteAdvanced));
        Ids.Research.CustomSurfaces = Ids.Research.CreateId(nameof (CustomSurfaces));
        Ids.Research.ResearchLab2 = Ids.Research.CreateId(nameof (ResearchLab2));
        Ids.Research.MaintenanceDepot = Ids.Research.CreateId(nameof (MaintenanceDepot));
        Ids.Research.ConveyorBelts = Ids.Research.CreateId(nameof (ConveyorBelts));
        Ids.Research.ResearchLab3 = Ids.Research.CreateId(nameof (ResearchLab3));
        Ids.Research.ShipWeapons = Ids.Research.CreateId(nameof (ShipWeapons));
        Ids.Research.ShipArmor = Ids.Research.CreateId(nameof (ShipArmor));
        Ids.Research.ShipRadar = Ids.Research.CreateId(nameof (ShipRadar));
        Ids.Research.RepairDock = Ids.Research.CreateId(nameof (RepairDock));
        Ids.Research.FarmingT2 = Ids.Research.CreateId(nameof (FarmingT2));
        Ids.Research.TreePlanting = Ids.Research.CreateId(nameof (TreePlanting));
        Ids.Research.CropRotation = Ids.Research.CreateId(nameof (CropRotation));
        Ids.Research.Vegetables = Ids.Research.CreateId(nameof (Vegetables));
        Ids.Research.FuelStation = Ids.Research.CreateId(nameof (FuelStation));
        Ids.Research.VehicleCapIncrease = Ids.Research.CreateId(nameof (VehicleCapIncrease));
        Ids.Research.VehicleCapIncrease2 = Ids.Research.CreateId(nameof (VehicleCapIncrease2));
        Ids.Research.RecyclingEdict = Ids.Research.CreateId(nameof (RecyclingEdict));
        Ids.Research.RecyclingEdict2 = Ids.Research.CreateId(nameof (RecyclingEdict2));
        Ids.Research.RecyclingIncrease = Ids.Research.CreateId(nameof (RecyclingIncrease));
        Ids.Research.DeconstructionRatioIncrease = Ids.Research.CreateId(nameof (DeconstructionRatioIncrease));
        Ids.Research.DeconstructionRatioIncrease2 = Ids.Research.CreateId(nameof (DeconstructionRatioIncrease2));
        Ids.Research.PowerGeneration2 = Ids.Research.CreateId(nameof (PowerGeneration2));
        Ids.Research.DieselGeneratorLarge = Ids.Research.CreateId(nameof (DieselGeneratorLarge));
        Ids.Research.SettlementPower = Ids.Research.CreateId(nameof (SettlementPower));
        Ids.Research.TransportsBalancing = Ids.Research.CreateId(nameof (TransportsBalancing));
        Ids.Research.TransportsLifts = Ids.Research.CreateId(nameof (TransportsLifts));
        Ids.Research.VehicleRamps = Ids.Research.CreateId(nameof (VehicleRamps));
        Ids.Research.Housing2 = Ids.Research.CreateId(nameof (Housing2));
        Ids.Research.Edicts1 = Ids.Research.CreateId(nameof (Edicts1));
        Ids.Research.Edicts2 = Ids.Research.CreateId(nameof (Edicts2));
        Ids.Research.Edicts3 = Ids.Research.CreateId(nameof (Edicts3));
        Ids.Research.Edicts4 = Ids.Research.CreateId(nameof (Edicts4));
        Ids.Research.MechPowerStorage = Ids.Research.CreateId(nameof (MechPowerStorage));
        Ids.Research.Stacker = Ids.Research.CreateId(nameof (Stacker));
        Ids.Research.FoodMarket2 = Ids.Research.CreateId(nameof (FoodMarket2));
        Ids.Research.UndergroundWater = Ids.Research.CreateId(nameof (UndergroundWater));
        Ids.Research.CrudeOilDistillation = Ids.Research.CreateId(nameof (CrudeOilDistillation));
        Ids.Research.Biofuel = Ids.Research.CreateId(nameof (Biofuel));
        Ids.Research.CargoDepot = Ids.Research.CreateId(nameof (CargoDepot));
        Ids.Research.SulfurProcessing = Ids.Research.CreateId(nameof (SulfurProcessing));
        Ids.Research.GasCombustion = Ids.Research.CreateId(nameof (GasCombustion));
        Ids.Research.CopperRefinement2 = Ids.Research.CreateId(nameof (CopperRefinement2));
        Ids.Research.SteelSmelting = Ids.Research.CreateId(nameof (SteelSmelting));
        Ids.Research.Cp3Packing = Ids.Research.CreateId(nameof (Cp3Packing));
        Ids.Research.AdvancedLogisticsControl = Ids.Research.CreateId(nameof (AdvancedLogisticsControl));
        Ids.Research.PipeTransportsT2 = Ids.Research.CreateId(nameof (PipeTransportsT2));
        Ids.Research.ConveyorBeltsT2 = Ids.Research.CreateId(nameof (ConveyorBeltsT2));
        Ids.Research.SaltProduction = Ids.Research.CreateId(nameof (SaltProduction));
        Ids.Research.ThermalDesalinationBasic = Ids.Research.CreateId(nameof (ThermalDesalinationBasic));
        Ids.Research.ThermalDesalination = Ids.Research.CreateId(nameof (ThermalDesalination));
        Ids.Research.IncinerationPlant = Ids.Research.CreateId(nameof (IncinerationPlant));
        Ids.Research.CornCrop = Ids.Research.CreateId(nameof (CornCrop));
        Ids.Research.ChemicalPlant = Ids.Research.CreateId(nameof (ChemicalPlant));
        Ids.Research.Fertilizers = Ids.Research.CreateId(nameof (Fertilizers));
        Ids.Research.ChemicalPlant2 = Ids.Research.CreateId(nameof (ChemicalPlant2));
        Ids.Research.Fermentation = Ids.Research.CreateId(nameof (Fermentation));
        Ids.Research.BioDiesel = Ids.Research.CreateId(nameof (BioDiesel));
        Ids.Research.Hospital = Ids.Research.CreateId(nameof (Hospital));
        Ids.Research.MedicalSupplies2 = Ids.Research.CreateId(nameof (MedicalSupplies2));
        Ids.Research.MedicalSupplies3 = Ids.Research.CreateId(nameof (MedicalSupplies3));
        Ids.Research.Electrolysis = Ids.Research.CreateId(nameof (Electrolysis));
        Ids.Research.Electrolysis2 = Ids.Research.CreateId(nameof (Electrolysis2));
        Ids.Research.VacuumDesalination = Ids.Research.CreateId(nameof (VacuumDesalination));
        Ids.Research.RotaryKilnGas = Ids.Research.CreateId(nameof (RotaryKilnGas));
        Ids.Research.WaterTreatment = Ids.Research.CreateId(nameof (WaterTreatment));
        Ids.Research.WaterTreatment2 = Ids.Research.CreateId(nameof (WaterTreatment2));
        Ids.Research.ExhaustFiltration = Ids.Research.CreateId(nameof (ExhaustFiltration));
        Ids.Research.GlassSmelting = Ids.Research.CreateId(nameof (GlassSmelting));
        Ids.Research.HouseholdGoods = Ids.Research.CreateId(nameof (HouseholdGoods));
        Ids.Research.Storage2 = Ids.Research.CreateId(nameof (Storage2));
        Ids.Research.Storage3 = Ids.Research.CreateId(nameof (Storage3));
        Ids.Research.Storage4 = Ids.Research.CreateId(nameof (Storage4));
        Ids.Research.VehicleCapIncrease3 = Ids.Research.CreateId(nameof (VehicleCapIncrease3));
        Ids.Research.VehicleCapIncrease4 = Ids.Research.CreateId(nameof (VehicleCapIncrease4));
        Ids.Research.TrucksCapacityEdict = Ids.Research.CreateId(nameof (TrucksCapacityEdict));
        Ids.Research.RubberProduction = Ids.Research.CreateId(nameof (RubberProduction));
        Ids.Research.Recycling = Ids.Research.CreateId(nameof (Recycling));
        Ids.Research.Compactor = Ids.Research.CreateId(nameof (Compactor));
        Ids.Research.RecyclingForSettlement = Ids.Research.CreateId(nameof (RecyclingForSettlement));
        Ids.Research.NaphthaProcessing = Ids.Research.CreateId(nameof (NaphthaProcessing));
        Ids.Research.VehicleAssembly2 = Ids.Research.CreateId(nameof (VehicleAssembly2));
        Ids.Research.HydrogenCell = Ids.Research.CreateId(nameof (HydrogenCell));
        Ids.Research.VehicleAssembly3 = Ids.Research.CreateId(nameof (VehicleAssembly3));
        Ids.Research.VehicleAssembly3H = Ids.Research.CreateId(nameof (VehicleAssembly3H));
        Ids.Research.FuelStation2 = Ids.Research.CreateId(nameof (FuelStation2));
        Ids.Research.FuelStation3 = Ids.Research.CreateId(nameof (FuelStation3));
        Ids.Research.TreeHarvester = Ids.Research.CreateId(nameof (TreeHarvester));
        Ids.Research.ResearchLab4 = Ids.Research.CreateId(nameof (ResearchLab4));
        Ids.Research.Engine2 = Ids.Research.CreateId(nameof (Engine2));
        Ids.Research.ShipWeapons2 = Ids.Research.CreateId(nameof (ShipWeapons2));
        Ids.Research.ShipFuelTankUpgrade = Ids.Research.CreateId(nameof (ShipFuelTankUpgrade));
        Ids.Research.ShipArmor2 = Ids.Research.CreateId(nameof (ShipArmor2));
        Ids.Research.ShipRadar2 = Ids.Research.CreateId(nameof (ShipRadar2));
        Ids.Research.PowerGeneration3 = Ids.Research.CreateId(nameof (PowerGeneration3));
        Ids.Research.CarbonDioxideRecycling = Ids.Research.CreateId(nameof (CarbonDioxideRecycling));
        Ids.Research.Fruit = Ids.Research.CreateId(nameof (Fruit));
        Ids.Research.OrganicFertilizer = Ids.Research.CreateId(nameof (OrganicFertilizer));
        Ids.Research.PlasticProduction = Ids.Research.CreateId(nameof (PlasticProduction));
        Ids.Research.HydrogenReforming = Ids.Research.CreateId(nameof (HydrogenReforming));
        Ids.Research.WaterRecovery = Ids.Research.CreateId(nameof (WaterRecovery));
        Ids.Research.ThermalStorage = Ids.Research.CreateId(nameof (ThermalStorage));
        Ids.Research.AdvancedSmelting = Ids.Research.CreateId(nameof (AdvancedSmelting));
        Ids.Research.CrusherLarge = Ids.Research.CreateId(nameof (CrusherLarge));
        Ids.Research.IndustrialMixerT2 = Ids.Research.CreateId(nameof (IndustrialMixerT2));
        Ids.Research.GlassSmeltingT2 = Ids.Research.CreateId(nameof (GlassSmeltingT2));
        Ids.Research.CargoDepot2 = Ids.Research.CreateId(nameof (CargoDepot2));
        Ids.Research.CargoDepot3 = Ids.Research.CreateId(nameof (CargoDepot3));
        Ids.Research.CargoDepot4 = Ids.Research.CreateId(nameof (CargoDepot4));
        Ids.Research.PolySiliconProduction = Ids.Research.CreateId(nameof (PolySiliconProduction));
        Ids.Research.VehicleCapIncrease5 = Ids.Research.CreateId(nameof (VehicleCapIncrease5));
        Ids.Research.NuclearReactor = Ids.Research.CreateId(nameof (NuclearReactor));
        Ids.Research.UraniumEnrichment = Ids.Research.CreateId(nameof (UraniumEnrichment));
        Ids.Research.NuclearReactor2 = Ids.Research.CreateId(nameof (NuclearReactor2));
        Ids.Research.NuclearReactor3 = Ids.Research.CreateId(nameof (NuclearReactor3));
        Ids.Research.BasicComputing = Ids.Research.CreateId(nameof (BasicComputing));
        Ids.Research.Datacenter = Ids.Research.CreateId(nameof (Datacenter));
        Ids.Research.StatueOfMaintenance = Ids.Research.CreateId(nameof (StatueOfMaintenance));
        Ids.Research.Cp4Packing = Ids.Research.CreateId(nameof (Cp4Packing));
        Ids.Research.PipeTransportsT3 = Ids.Research.CreateId(nameof (PipeTransportsT3));
        Ids.Research.ConveyorBeltsT3 = Ids.Research.CreateId(nameof (ConveyorBeltsT3));
        Ids.Research.ConveyorRouting = Ids.Research.CreateId(nameof (ConveyorRouting));
        Ids.Research.Housing3 = Ids.Research.CreateId(nameof (Housing3));
        Ids.Research.SettlementDecorations = Ids.Research.CreateId(nameof (SettlementDecorations));
        Ids.Research.FarmingT3 = Ids.Research.CreateId(nameof (FarmingT3));
        Ids.Research.NaphthaReforming = Ids.Research.CreateId(nameof (NaphthaReforming));
        Ids.Research.HeavyOilCracking = Ids.Research.CreateId(nameof (HeavyOilCracking));
        Ids.Research.FarmingT4 = Ids.Research.CreateId(nameof (FarmingT4));
        Ids.Research.Burner = Ids.Research.CreateId(nameof (Burner));
        Ids.Research.WheatCrop = Ids.Research.CreateId(nameof (WheatCrop));
        Ids.Research.SugarCane = Ids.Research.CreateId(nameof (SugarCane));
        Ids.Research.Canola = Ids.Research.CreateId(nameof (Canola));
        Ids.Research.ChickenFarm = Ids.Research.CreateId(nameof (ChickenFarm));
        Ids.Research.SnacksProduction = Ids.Research.CreateId(nameof (SnacksProduction));
        Ids.Research.SausageProduction = Ids.Research.CreateId(nameof (SausageProduction));
        Ids.Research.FoodPacking = Ids.Research.CreateId(nameof (FoodPacking));
        Ids.Research.SoybeanCrop = Ids.Research.CreateId(nameof (SoybeanCrop));
        Ids.Research.TombOfCaptains = Ids.Research.CreateId(nameof (TombOfCaptains));
        Ids.Research.ResearchLab5 = Ids.Research.CreateId(nameof (ResearchLab5));
        Ids.Research.PowerGeneration4 = Ids.Research.CreateId(nameof (PowerGeneration4));
        Ids.Research.BoilerElectric = Ids.Research.CreateId(nameof (BoilerElectric));
        Ids.Research.ArcFurnace2 = Ids.Research.CreateId(nameof (ArcFurnace2));
        Ids.Research.MicrochipProduction2 = Ids.Research.CreateId(nameof (MicrochipProduction2));
        Ids.Research.Engine3 = Ids.Research.CreateId(nameof (Engine3));
        Ids.Research.VehicleCapIncrease6 = Ids.Research.CreateId(nameof (VehicleCapIncrease6));
        Ids.Research.HouseholdAppliances = Ids.Research.CreateId(nameof (HouseholdAppliances));
        Ids.Research.ConsumerElectronics = Ids.Research.CreateId(nameof (ConsumerElectronics));
        Ids.Research.GoldSmelting = Ids.Research.CreateId(nameof (GoldSmelting));
        Ids.Research.ShipWeapons3 = Ids.Research.CreateId(nameof (ShipWeapons3));
        Ids.Research.MicrochipProduction = Ids.Research.CreateId(nameof (MicrochipProduction));
        Ids.Research.Assembler3 = Ids.Research.CreateId(nameof (Assembler3));
        Ids.Research.RocketAssemblyAndLaunch = Ids.Research.CreateId(nameof (RocketAssemblyAndLaunch));
        Ids.Research.SolarPanels = Ids.Research.CreateId(nameof (SolarPanels));
        Ids.Research.SolarPanels2 = Ids.Research.CreateId(nameof (SolarPanels2));
        Ids.Research.SuperPressSteam = Ids.Research.CreateId(nameof (SuperPressSteam));
      }
    }

    public static class Rockets
    {
      public static readonly EntityProto.ID TestingRocketT0;

      public static DynamicEntityProto.ID GetRocketTransporterId(EntityProto.ID rocketId)
      {
        return new DynamicEntityProto.ID(rocketId.Value + "Transporter");
      }

      static Rockets()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Rockets.TestingRocketT0 = new EntityProto.ID(nameof (TestingRocketT0));
      }
    }

    public static class Ships
    {
      public static readonly EntityProto.ID CargoShipT1;
      public static readonly EntityProto.ID CargoShipT2;
      public static readonly EntityProto.ID CargoShipT3;
      public static readonly EntityProto.ID CargoShipT4;
      public static readonly EntityProto.ID CargoShipFluidModule;
      public static readonly EntityProto.ID CargoShipLooseModule;
      public static readonly EntityProto.ID CargoShipUnitModule;

      static Ships()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Ships.CargoShipT1 = new EntityProto.ID(nameof (CargoShipT1));
        Ids.Ships.CargoShipT2 = new EntityProto.ID(nameof (CargoShipT2));
        Ids.Ships.CargoShipT3 = new EntityProto.ID(nameof (CargoShipT3));
        Ids.Ships.CargoShipT4 = new EntityProto.ID(nameof (CargoShipT4));
        Ids.Ships.CargoShipFluidModule = new EntityProto.ID(nameof (CargoShipFluidModule));
        Ids.Ships.CargoShipLooseModule = new EntityProto.ID(nameof (CargoShipLooseModule));
        Ids.Ships.CargoShipUnitModule = new EntityProto.ID(nameof (CargoShipUnitModule));
      }
    }

    public static class Technology
    {
      public static readonly Proto.ID OilDrilling;
      public static readonly Proto.ID CargoShip;
      public static readonly Proto.ID ShipRadar;
      public static readonly Proto.ID ShipRadarT2;
      public static readonly Proto.ID Electronics2;
      public static readonly Proto.ID HydrogenCargoShip;
      public static readonly Proto.ID Microchip;
      public static readonly Proto.ID NuclearEnergy;
      public static readonly Proto.ID WheatSeeds;
      public static readonly Proto.ID CanolaSeeds;
      public static readonly Proto.ID SugarCaneSeeds;
      public static readonly Proto.ID SoybeansSeeds;
      public static readonly Proto.ID FruitSeeds;
      public static readonly Proto.ID CornSeeds;
      public static readonly Proto.ID PoppySeeds;

      private static Proto.ID createId(string id) => new Proto.ID(nameof (Technology) + id);

      static Technology()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Technology.OilDrilling = Ids.Technology.createId(nameof (OilDrilling));
        Ids.Technology.CargoShip = Ids.Technology.createId(nameof (CargoShip));
        Ids.Technology.ShipRadar = Ids.Technology.createId(nameof (ShipRadar));
        Ids.Technology.ShipRadarT2 = Ids.Technology.createId(nameof (ShipRadarT2));
        Ids.Technology.Electronics2 = Ids.Technology.createId(nameof (Electronics2));
        Ids.Technology.HydrogenCargoShip = Ids.Technology.createId(nameof (HydrogenCargoShip));
        Ids.Technology.Microchip = Ids.Technology.createId(nameof (Microchip));
        Ids.Technology.NuclearEnergy = Ids.Technology.createId("NuclearPower");
        Ids.Technology.WheatSeeds = Ids.Technology.createId(nameof (WheatSeeds));
        Ids.Technology.CanolaSeeds = Ids.Technology.createId(nameof (CanolaSeeds));
        Ids.Technology.SugarCaneSeeds = Ids.Technology.createId(nameof (SugarCaneSeeds));
        Ids.Technology.SoybeansSeeds = Ids.Technology.createId(nameof (SoybeansSeeds));
        Ids.Technology.FruitSeeds = Ids.Technology.createId(nameof (FruitSeeds));
        Ids.Technology.CornSeeds = Ids.Technology.createId(nameof (CornSeeds));
        Ids.Technology.PoppySeeds = Ids.Technology.createId(nameof (PoppySeeds));
      }
    }

    public static class TerrainDetails
    {
      public static readonly Proto.ID Grass;
      public static readonly Proto.ID GrassLush;
      public static readonly Proto.ID FlowersWhite;
      public static readonly Proto.ID FlowersRed;
      public static readonly Proto.ID FlowersYellowLush;
      public static readonly Proto.ID FlowersPurpleLush;
      public static readonly Proto.ID ForestGrass;
      public static readonly Proto.ID Rocks;
      public static readonly Proto.ID DebrisFlat;

      private static Proto.ID create(string id) => new Proto.ID(id + "_TerrainDetail");

      static TerrainDetails()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.TerrainDetails.Grass = Ids.TerrainDetails.create(nameof (Grass));
        Ids.TerrainDetails.GrassLush = Ids.TerrainDetails.create(nameof (GrassLush));
        Ids.TerrainDetails.FlowersWhite = Ids.TerrainDetails.create(nameof (FlowersWhite));
        Ids.TerrainDetails.FlowersRed = Ids.TerrainDetails.create(nameof (FlowersRed));
        Ids.TerrainDetails.FlowersYellowLush = Ids.TerrainDetails.create(nameof (FlowersYellowLush));
        Ids.TerrainDetails.FlowersPurpleLush = Ids.TerrainDetails.create(nameof (FlowersPurpleLush));
        Ids.TerrainDetails.ForestGrass = Ids.TerrainDetails.create(nameof (ForestGrass));
        Ids.TerrainDetails.Rocks = Ids.TerrainDetails.create(nameof (Rocks));
        Ids.TerrainDetails.DebrisFlat = Ids.TerrainDetails.create(nameof (DebrisFlat));
      }
    }

    public static class TerrainMaterials
    {
      public static readonly Proto.ID Grass;
      public static readonly Proto.ID Dirt;
      public static readonly Proto.ID GrassNoDetails;
      public static readonly Proto.ID DirtNoDetails;
      public static readonly Proto.ID DirtBare;
      public static readonly Proto.ID GrassLush;
      public static readonly Proto.ID DirtLush;
      public static readonly Proto.ID FlowersWhite;
      public static readonly Proto.ID DirtFlowersWhite;
      public static readonly Proto.ID FlowersYellowLush;
      public static readonly Proto.ID DirtFlowersYellowLush;
      public static readonly Proto.ID FlowersRed;
      public static readonly Proto.ID DirtFlowersRed;
      public static readonly Proto.ID FlowersPurpleLush;
      public static readonly Proto.ID DirtFlowersPurpleLush;
      public static readonly Proto.ID FarmGround;
      public static readonly Proto.ID ForestFloor;
      public static readonly Proto.ID ForestDirt;
      public static readonly Proto.ID Compost;
      public static readonly Proto.ID Landfill;
      public static readonly Proto.ID LandfillOld;
      public static readonly Proto.ID UraniumDepleted;
      public static readonly Proto.ID Rock;
      public static readonly Proto.ID RockDisrupted;
      public static readonly Proto.ID RockNoGrassCover;
      public static readonly Proto.ID HardenedRock;
      public static readonly Proto.ID Gravel;
      public static readonly Proto.ID Bedrock;
      public static readonly Proto.ID Slag;
      public static readonly Proto.ID SlagCrushed;
      public static readonly Proto.ID Sand;
      public static readonly Proto.ID SandDisrupted;
      public static readonly Proto.ID Limestone;
      public static readonly Proto.ID LimestoneDisrupted;
      public static readonly Proto.ID Quartz;
      public static readonly Proto.ID QuartzDisrupted;
      public static readonly Proto.ID QuartzCrushed;
      public static readonly Proto.ID Coal;
      public static readonly Proto.ID CoalDisrupted;
      public static readonly Proto.ID IronOre;
      public static readonly Proto.ID IronOreDisrupted;
      public static readonly Proto.ID IronOreCrushed;
      public static readonly Proto.ID CopperOre;
      public static readonly Proto.ID CopperOreDisrupted;
      public static readonly Proto.ID CopperOreCrushed;
      public static readonly Proto.ID GoldOre;
      public static readonly Proto.ID GoldOreDisrupted;
      public static readonly Proto.ID GoldOreCrushed;

      public static Proto.ID CreateId(string id) => IdsCore.TerrainMaterials.CreateId(id);

      static TerrainMaterials()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.TerrainMaterials.Grass = IdsCore.TerrainMaterials.Grass;
        Ids.TerrainMaterials.Dirt = Ids.TerrainMaterials.CreateId(nameof (Dirt));
        Ids.TerrainMaterials.GrassNoDetails = Ids.TerrainMaterials.CreateId(nameof (GrassNoDetails));
        Ids.TerrainMaterials.DirtNoDetails = Ids.TerrainMaterials.CreateId(nameof (DirtNoDetails));
        Ids.TerrainMaterials.DirtBare = Ids.TerrainMaterials.CreateId(nameof (DirtBare));
        Ids.TerrainMaterials.GrassLush = Ids.TerrainMaterials.CreateId(nameof (GrassLush));
        Ids.TerrainMaterials.DirtLush = Ids.TerrainMaterials.CreateId(nameof (DirtLush));
        Ids.TerrainMaterials.FlowersWhite = Ids.TerrainMaterials.CreateId(nameof (FlowersWhite));
        Ids.TerrainMaterials.DirtFlowersWhite = Ids.TerrainMaterials.CreateId(nameof (DirtFlowersWhite));
        Ids.TerrainMaterials.FlowersYellowLush = Ids.TerrainMaterials.CreateId(nameof (FlowersYellowLush));
        Ids.TerrainMaterials.DirtFlowersYellowLush = Ids.TerrainMaterials.CreateId(nameof (DirtFlowersYellowLush));
        Ids.TerrainMaterials.FlowersRed = Ids.TerrainMaterials.CreateId(nameof (FlowersRed));
        Ids.TerrainMaterials.DirtFlowersRed = Ids.TerrainMaterials.CreateId(nameof (DirtFlowersRed));
        Ids.TerrainMaterials.FlowersPurpleLush = Ids.TerrainMaterials.CreateId(nameof (FlowersPurpleLush));
        Ids.TerrainMaterials.DirtFlowersPurpleLush = Ids.TerrainMaterials.CreateId(nameof (DirtFlowersPurpleLush));
        Ids.TerrainMaterials.FarmGround = IdsCore.TerrainMaterials.FarmGround;
        Ids.TerrainMaterials.ForestFloor = Ids.TerrainMaterials.CreateId(nameof (ForestFloor));
        Ids.TerrainMaterials.ForestDirt = Ids.TerrainMaterials.CreateId(nameof (ForestDirt));
        Ids.TerrainMaterials.Compost = Ids.TerrainMaterials.CreateId(nameof (Compost));
        Ids.TerrainMaterials.Landfill = IdsCore.TerrainMaterials.Landfill;
        Ids.TerrainMaterials.LandfillOld = Ids.TerrainMaterials.CreateId(nameof (LandfillOld));
        Ids.TerrainMaterials.UraniumDepleted = Ids.TerrainMaterials.CreateId(nameof (UraniumDepleted));
        Ids.TerrainMaterials.Rock = Ids.TerrainMaterials.CreateId(nameof (Rock));
        Ids.TerrainMaterials.RockDisrupted = Ids.TerrainMaterials.CreateId(nameof (RockDisrupted));
        Ids.TerrainMaterials.RockNoGrassCover = Ids.TerrainMaterials.CreateId(nameof (RockNoGrassCover));
        Ids.TerrainMaterials.HardenedRock = IdsCore.TerrainMaterials.HardenedRock;
        Ids.TerrainMaterials.Gravel = Ids.TerrainMaterials.CreateId(nameof (Gravel));
        Ids.TerrainMaterials.Bedrock = IdsCore.TerrainMaterials.Bedrock;
        Ids.TerrainMaterials.Slag = Ids.TerrainMaterials.CreateId(nameof (Slag));
        Ids.TerrainMaterials.SlagCrushed = Ids.TerrainMaterials.CreateId(nameof (SlagCrushed));
        Ids.TerrainMaterials.Sand = Ids.TerrainMaterials.CreateId(nameof (Sand));
        Ids.TerrainMaterials.SandDisrupted = Ids.TerrainMaterials.CreateId(nameof (SandDisrupted));
        Ids.TerrainMaterials.Limestone = Ids.TerrainMaterials.CreateId(nameof (Limestone));
        Ids.TerrainMaterials.LimestoneDisrupted = Ids.TerrainMaterials.CreateId(nameof (LimestoneDisrupted));
        Ids.TerrainMaterials.Quartz = Ids.TerrainMaterials.CreateId(nameof (Quartz));
        Ids.TerrainMaterials.QuartzDisrupted = Ids.TerrainMaterials.CreateId(nameof (QuartzDisrupted));
        Ids.TerrainMaterials.QuartzCrushed = Ids.TerrainMaterials.CreateId(nameof (QuartzCrushed));
        Ids.TerrainMaterials.Coal = Ids.TerrainMaterials.CreateId(nameof (Coal));
        Ids.TerrainMaterials.CoalDisrupted = Ids.TerrainMaterials.CreateId(nameof (CoalDisrupted));
        Ids.TerrainMaterials.IronOre = Ids.TerrainMaterials.CreateId(nameof (IronOre));
        Ids.TerrainMaterials.IronOreDisrupted = Ids.TerrainMaterials.CreateId(nameof (IronOreDisrupted));
        Ids.TerrainMaterials.IronOreCrushed = Ids.TerrainMaterials.CreateId(nameof (IronOreCrushed));
        Ids.TerrainMaterials.CopperOre = Ids.TerrainMaterials.CreateId(nameof (CopperOre));
        Ids.TerrainMaterials.CopperOreDisrupted = Ids.TerrainMaterials.CreateId(nameof (CopperOreDisrupted));
        Ids.TerrainMaterials.CopperOreCrushed = Ids.TerrainMaterials.CreateId(nameof (CopperOreCrushed));
        Ids.TerrainMaterials.GoldOre = Ids.TerrainMaterials.CreateId(nameof (GoldOre));
        Ids.TerrainMaterials.GoldOreDisrupted = Ids.TerrainMaterials.CreateId(nameof (GoldOreDisrupted));
        Ids.TerrainMaterials.GoldOreCrushed = Ids.TerrainMaterials.CreateId(nameof (GoldOreCrushed));
      }
    }

    public static class TerrainProps
    {
      public static readonly Proto.ID Stone01;
      public static readonly Proto.ID Stone02;
      public static readonly Proto.ID Stone03;
      public static readonly Proto.ID StoneSharp01;
      public static readonly Proto.ID BushSmall;
      public static readonly Proto.ID BushMedium;
      public static readonly Proto.ID HedgeCube01;

      static TerrainProps()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.TerrainProps.Stone01 = new Proto.ID("TerProp_Stone01");
        Ids.TerrainProps.Stone02 = new Proto.ID("TerProp_Stone02");
        Ids.TerrainProps.Stone03 = new Proto.ID("TerProp_Stone03");
        Ids.TerrainProps.StoneSharp01 = new Proto.ID("TerProp_StoneSharp01");
        Ids.TerrainProps.BushSmall = new Proto.ID("TerProp_Bush01");
        Ids.TerrainProps.BushMedium = new Proto.ID("TerProp_Bush02");
        Ids.TerrainProps.HedgeCube01 = new Proto.ID("TerProp_HedgeCube01");
      }
    }

    public static class TerrainTileSurfaces
    {
      public static readonly Proto.ID DefaultConcrete;
      public static readonly Proto.ID ConcreteReinforced;
      public static readonly Proto.ID Bricks;
      public static readonly Proto.ID Cobblestone;
      public static readonly Proto.ID Sand1;
      public static readonly Proto.ID Sand2;
      public static readonly Proto.ID Metal1;
      public static readonly Proto.ID Metal2;
      public static readonly Proto.ID Metal3;
      public static readonly Proto.ID Metal4;
      public static readonly Proto.ID Gold;
      public static readonly Proto.ID SettlementPaths;

      public static Proto.ID CreateId(string id) => IdsCore.TerrainTileSurfaces.CreateId(id);

      static TerrainTileSurfaces()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.TerrainTileSurfaces.DefaultConcrete = IdsCore.TerrainTileSurfaces.DefaultConcrete;
        Ids.TerrainTileSurfaces.ConcreteReinforced = Ids.TerrainTileSurfaces.CreateId(nameof (ConcreteReinforced));
        Ids.TerrainTileSurfaces.Bricks = Ids.TerrainTileSurfaces.CreateId(nameof (Bricks));
        Ids.TerrainTileSurfaces.Cobblestone = Ids.TerrainTileSurfaces.CreateId(nameof (Cobblestone));
        Ids.TerrainTileSurfaces.Sand1 = Ids.TerrainTileSurfaces.CreateId(nameof (Sand1));
        Ids.TerrainTileSurfaces.Sand2 = Ids.TerrainTileSurfaces.CreateId(nameof (Sand2));
        Ids.TerrainTileSurfaces.Metal1 = Ids.TerrainTileSurfaces.CreateId(nameof (Metal1));
        Ids.TerrainTileSurfaces.Metal2 = Ids.TerrainTileSurfaces.CreateId(nameof (Metal2));
        Ids.TerrainTileSurfaces.Metal3 = Ids.TerrainTileSurfaces.CreateId(nameof (Metal3));
        Ids.TerrainTileSurfaces.Metal4 = Ids.TerrainTileSurfaces.CreateId(nameof (Metal4));
        Ids.TerrainTileSurfaces.Gold = Ids.TerrainTileSurfaces.CreateId(nameof (Gold));
        Ids.TerrainTileSurfaces.SettlementPaths = Ids.TerrainTileSurfaces.CreateId(nameof (SettlementPaths));
      }
    }

    public static class ToolbarCategories
    {
      public static readonly Proto.ID Transports;
      public static readonly Proto.ID Machines;
      public static readonly Proto.ID MachinesWater;
      public static readonly Proto.ID MachinesFood;
      public static readonly Proto.ID MachinesMetallurgy;
      public static readonly Proto.ID MachinesOil;
      public static readonly Proto.ID MachinesElectricity;
      public static readonly Proto.ID Waste;
      public static readonly Proto.ID Storages;
      public static readonly Proto.ID Buildings;
      public static readonly Proto.ID BuildingsForVehicles;
      public static readonly Proto.ID Housing;
      public static readonly Proto.ID Docks;
      public static readonly Proto.ID Landmarks;

      static ToolbarCategories()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.ToolbarCategories.Transports = new Proto.ID("transportsCategory");
        Ids.ToolbarCategories.Machines = new Proto.ID("machinesCategory");
        Ids.ToolbarCategories.MachinesWater = new Proto.ID("machinesWaterCategory");
        Ids.ToolbarCategories.MachinesFood = new Proto.ID("machinesFoodCategory");
        Ids.ToolbarCategories.MachinesMetallurgy = new Proto.ID("machinesMetallurgyCategory");
        Ids.ToolbarCategories.MachinesOil = new Proto.ID("machinesOilCategory");
        Ids.ToolbarCategories.MachinesElectricity = new Proto.ID("machinesElectricityCategory");
        Ids.ToolbarCategories.Waste = new Proto.ID("wasteCategory");
        Ids.ToolbarCategories.Storages = new Proto.ID("storagesCategory");
        Ids.ToolbarCategories.Buildings = new Proto.ID("buildingsCategory");
        Ids.ToolbarCategories.BuildingsForVehicles = new Proto.ID("buildingsForVehiclesCategory");
        Ids.ToolbarCategories.Housing = new Proto.ID("housingCategory");
        Ids.ToolbarCategories.Docks = new Proto.ID("docksCategory");
        Ids.ToolbarCategories.Landmarks = new Proto.ID("landmarksCategory");
      }
    }

    public static class Forests
    {
      public static readonly Proto.ID ConiferForest;

      static Forests()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Forests.ConiferForest = new Proto.ID(nameof (ConiferForest));
      }
    }

    public static class PlantingGroups
    {
      public static readonly Proto.ID ConiferGroup;
      public static readonly Proto.ID DeciduousGroup;
      public static readonly Proto.ID PalmsGroup;
      public static readonly Proto.ID NonPlantableGroup;

      static PlantingGroups()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.PlantingGroups.ConiferGroup = new Proto.ID(nameof (ConiferGroup));
        Ids.PlantingGroups.DeciduousGroup = new Proto.ID(nameof (DeciduousGroup));
        Ids.PlantingGroups.PalmsGroup = new Proto.ID(nameof (PalmsGroup));
        Ids.PlantingGroups.NonPlantableGroup = new Proto.ID(nameof (NonPlantableGroup));
      }
    }

    public static class Trees
    {
      public static readonly Proto.ID SpruceTree;
      public static readonly Proto.ID FirTree;
      public static readonly Proto.ID BirchTree;
      public static readonly Proto.ID BirchTreeDry;
      public static readonly Proto.ID MapleTree;
      public static readonly Proto.ID MapleTreeDry;
      public static readonly Proto.ID OakTree;
      public static readonly Proto.ID OakTreeDry;
      public static readonly Proto.ID PalmTree;

      static Trees()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Trees.SpruceTree = new Proto.ID(nameof (SpruceTree));
        Ids.Trees.FirTree = new Proto.ID(nameof (FirTree));
        Ids.Trees.BirchTree = new Proto.ID(nameof (BirchTree));
        Ids.Trees.BirchTreeDry = new Proto.ID(nameof (BirchTreeDry));
        Ids.Trees.MapleTree = new Proto.ID(nameof (MapleTree));
        Ids.Trees.MapleTreeDry = new Proto.ID(nameof (MapleTreeDry));
        Ids.Trees.OakTree = new Proto.ID(nameof (OakTree));
        Ids.Trees.OakTreeDry = new Proto.ID(nameof (OakTreeDry));
        Ids.Trees.PalmTree = new Proto.ID(nameof (PalmTree));
      }
    }

    public static class Vehicles
    {
      public static readonly DynamicEntityProto.ID ExcavatorT1;
      public static readonly DynamicEntityProto.ID ExcavatorT2;
      public static readonly DynamicEntityProto.ID ExcavatorT2H;
      public static readonly DynamicEntityProto.ID ExcavatorT3;
      public static readonly DynamicEntityProto.ID ExcavatorT3H;
      public static readonly DynamicEntityProto.ID TreeHarvesterT1;
      public static readonly DynamicEntityProto.ID TreeHarvesterT2;
      public static readonly DynamicEntityProto.ID TreeHarvesterT2H;
      public static readonly DynamicEntityProto.ID TreePlanterT1;
      public static readonly DynamicEntityProto.ID TreePlanterT1H;

      static Vehicles()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Vehicles.ExcavatorT1 = new DynamicEntityProto.ID(nameof (ExcavatorT1));
        Ids.Vehicles.ExcavatorT2 = new DynamicEntityProto.ID(nameof (ExcavatorT2));
        Ids.Vehicles.ExcavatorT2H = new DynamicEntityProto.ID(nameof (ExcavatorT2H));
        Ids.Vehicles.ExcavatorT3 = new DynamicEntityProto.ID(nameof (ExcavatorT3));
        Ids.Vehicles.ExcavatorT3H = new DynamicEntityProto.ID(nameof (ExcavatorT3H));
        Ids.Vehicles.TreeHarvesterT1 = new DynamicEntityProto.ID(nameof (TreeHarvesterT1));
        Ids.Vehicles.TreeHarvesterT2 = new DynamicEntityProto.ID(nameof (TreeHarvesterT2));
        Ids.Vehicles.TreeHarvesterT2H = new DynamicEntityProto.ID(nameof (TreeHarvesterT2H));
        Ids.Vehicles.TreePlanterT1 = new DynamicEntityProto.ID(nameof (TreePlanterT1));
        Ids.Vehicles.TreePlanterT1H = new DynamicEntityProto.ID(nameof (TreePlanterT1H));
      }

      public static class TruckT1
      {
        public static readonly DynamicEntityProto.ID Id;

        static TruckT1()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Vehicles.TruckT1.Id = new DynamicEntityProto.ID(nameof (TruckT1));
        }
      }

      public static class TruckT2
      {
        public static readonly DynamicEntityProto.ID Id;

        static TruckT2()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Vehicles.TruckT2.Id = new DynamicEntityProto.ID(nameof (TruckT2));
        }
      }

      public static class TruckT2H
      {
        public static readonly DynamicEntityProto.ID Id;

        static TruckT2H()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Vehicles.TruckT2H.Id = new DynamicEntityProto.ID(nameof (TruckT2H));
        }
      }

      public static class TruckT3Fluid
      {
        public static readonly DynamicEntityProto.ID Id;

        static TruckT3Fluid()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Vehicles.TruckT3Fluid.Id = new DynamicEntityProto.ID(nameof (TruckT3Fluid));
        }
      }

      public static class TruckT3Loose
      {
        public static readonly DynamicEntityProto.ID Id;

        static TruckT3Loose()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Vehicles.TruckT3Loose.Id = new DynamicEntityProto.ID(nameof (TruckT3Loose));
        }
      }

      public static class TruckT3LooseH
      {
        public static readonly DynamicEntityProto.ID Id;

        static TruckT3LooseH()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Vehicles.TruckT3LooseH.Id = new DynamicEntityProto.ID(nameof (TruckT3LooseH));
        }
      }

      public static class TruckT3FluidH
      {
        public static readonly DynamicEntityProto.ID Id;

        static TruckT3FluidH()
        {
          MBiHIp97M4MqqbtZOh.chMFXj727();
          Ids.Vehicles.TruckT3FluidH.Id = new DynamicEntityProto.ID(nameof (TruckT3FluidH));
        }
      }
    }

    public static class Weather
    {
      public static readonly Proto.ID Sunny;
      public static readonly Proto.ID Cloudy;
      public static readonly Proto.ID Rainy;
      public static readonly Proto.ID HeavyRain;

      public static Proto.ID Create(string name) => IdsCore.Weather.Create(name);

      static Weather()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.Weather.Sunny = IdsCore.Weather.Sunny;
        Ids.Weather.Cloudy = Ids.Weather.Create(nameof (Cloudy));
        Ids.Weather.Rainy = Ids.Weather.Create(nameof (Rainy));
        Ids.Weather.HeavyRain = Ids.Weather.Create(nameof (HeavyRain));
      }
    }

    public static class World
    {
      public static readonly EntityProto.ID OilRigCost1;
      public static readonly EntityProto.ID OilRigCost2;
      public static readonly EntityProto.ID OilRigCost3;
      public static readonly EntityProto.ID WaterWell;
      public static readonly EntityProto.ID QuartzMine;
      public static readonly EntityProto.ID SulfurMine;
      public static readonly EntityProto.ID CoalMine;
      public static readonly EntityProto.ID UraniumMine;
      public static readonly EntityProto.ID RockMine;
      public static readonly EntityProto.ID LimestoneMine;
      public static readonly EntityProto.ID Settlement1;
      public static readonly EntityProto.ID Settlement2;
      public static readonly EntityProto.ID Settlement3;
      public static readonly EntityProto.ID Settlement4;
      public static readonly EntityProto.ID Settlement5;
      public static readonly EntityProto.ID SettlementForFuel;
      public static readonly EntityProto.ID SettlementForUranium;
      public static readonly EntityProto.ID SettlementForShips;
      public static readonly EntityProto.ID CargoShipWreckCost1;
      public static readonly EntityProto.ID CargoShipWreckCost2;

      static World()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Ids.World.OilRigCost1 = new EntityProto.ID(nameof (OilRigCost1));
        Ids.World.OilRigCost2 = new EntityProto.ID(nameof (OilRigCost2));
        Ids.World.OilRigCost3 = new EntityProto.ID(nameof (OilRigCost3));
        Ids.World.WaterWell = new EntityProto.ID(nameof (WaterWell));
        Ids.World.QuartzMine = new EntityProto.ID(nameof (QuartzMine));
        Ids.World.SulfurMine = new EntityProto.ID(nameof (SulfurMine));
        Ids.World.CoalMine = new EntityProto.ID(nameof (CoalMine));
        Ids.World.UraniumMine = new EntityProto.ID(nameof (UraniumMine));
        Ids.World.RockMine = new EntityProto.ID(nameof (RockMine));
        Ids.World.LimestoneMine = new EntityProto.ID(nameof (LimestoneMine));
        Ids.World.Settlement1 = new EntityProto.ID(nameof (Settlement1));
        Ids.World.Settlement2 = new EntityProto.ID(nameof (Settlement2));
        Ids.World.Settlement3 = new EntityProto.ID(nameof (Settlement3));
        Ids.World.Settlement4 = new EntityProto.ID(nameof (Settlement4));
        Ids.World.Settlement5 = new EntityProto.ID(nameof (Settlement5));
        Ids.World.SettlementForFuel = new EntityProto.ID("Settlement6");
        Ids.World.SettlementForUranium = new EntityProto.ID("Settlement7");
        Ids.World.SettlementForShips = new EntityProto.ID(nameof (SettlementForShips));
        Ids.World.CargoShipWreckCost1 = new EntityProto.ID(nameof (CargoShipWreckCost1));
        Ids.World.CargoShipWreckCost2 = new EntityProto.ID(nameof (CargoShipWreckCost2));
      }
    }
  }
}
