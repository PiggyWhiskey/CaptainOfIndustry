// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Costs
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Base
{
  public static class Costs
  {
    public static EntityCostsTpl.Builder Build => new EntityCostsTpl.Builder();

    public static class Buildings
    {
      public static EntityCostsTpl HousingT1 => (EntityCostsTpl) Costs.Build.CP(80);

      public static EntityCostsTpl HousingT2 => (EntityCostsTpl) Costs.Build.CP2(140);

      public static EntityCostsTpl HousingT3 => (EntityCostsTpl) Costs.Build.CP3(200).Glass(240);

      public static EntityCostsTpl SettlementFoodModule
      {
        get => (EntityCostsTpl) Costs.Build.CP(25).Workers(3).Priority(7);
      }

      public static EntityCostsTpl SettlementFoodModuleT2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(6).Priority(7);
      }

      public static EntityCostsTpl SettlementLandfillModule
      {
        get => (EntityCostsTpl) Costs.Build.CP(25).Workers(4).Priority(7);
      }

      public static EntityCostsTpl SettlementRecyclablesModule
      {
        get => (EntityCostsTpl) Costs.Build.CP3(60).Workers(12).Priority(7);
      }

      public static EntityCostsTpl SettlementBiomassModule
      {
        get => (EntityCostsTpl) Costs.Build.CP3(60).Workers(6).Priority(7);
      }

      public static EntityCostsTpl SettlementWaterModule
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(6).MaintenanceT1((Fix32) 6).Priority(7);
      }

      public static EntityCostsTpl SettlementPowerModule
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(8).MaintenanceT1((Fix32) 6).Priority(7);
      }

      public static EntityCostsTpl SettlementHouseholdGoodsModule
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(60).Glass(40).Workers(16).MaintenanceT1((Fix32) 6).Priority(7);
        }
      }

      public static EntityCostsTpl SettlementHouseholdAppliancesModule
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP4(40).Glass(40).Workers(20).MaintenanceT2(8).Priority(7);
        }
      }

      public static EntityCostsTpl SettlementConsumerElectronicsModule
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP4(100).Glass(80).Workers(24).MaintenanceT2(10).Priority(7);
        }
      }

      public static EntityCostsTpl Hospital
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(160).Workers(36).MaintenanceT1((Fix32) 12).Priority(7);
        }
      }

      public static EntityCostsTpl SettlementSquare
      {
        get => (EntityCostsTpl) Costs.Build.CP(100).Concrete(120);
      }

      public static EntityCostsTpl SettlementPillar
      {
        get => (EntityCostsTpl) Costs.Build.CP(200).Concrete(220);
      }

      public static EntityCostsTpl SettlementFountain
      {
        get => (EntityCostsTpl) Costs.Build.CP(200).Concrete(220);
      }

      public static EntityCostsTpl Farm
      {
        get => (EntityCostsTpl) Costs.Build.CP(30).Workers(10).Priority(8);
      }

      public static EntityCostsTpl FarmT2
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP2(60).Workers(12).MaintenanceT1((Fix32) 2).Priority(8);
        }
      }

      public static EntityCostsTpl FarmT3
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(100).Glass(320).Workers(18).MaintenanceT1((Fix32) 6).Priority(8);
        }
      }

      public static EntityCostsTpl FarmT4
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(200).Glass(640).Workers(24).MaintenanceT1((Fix32) 8).Priority(8);
        }
      }

      public static EntityCostsTpl ChickenFarm => (EntityCostsTpl) Costs.Build.CP3(50).Workers(12);

      public static EntityCostsTpl StorageUnit => (EntityCostsTpl) Costs.Build.CP(30).Priority(4);

      public static EntityCostsTpl StorageUnitT2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(30).Priority(4);
      }

      public static EntityCostsTpl StorageUnitT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(90).Priority(4);
      }

      public static EntityCostsTpl StorageUnitT4
      {
        get => (EntityCostsTpl) Costs.Build.CP3(180).Priority(4);
      }

      public static EntityCostsTpl StorageLoose => (EntityCostsTpl) Costs.Build.CP(30).Priority(4);

      public static EntityCostsTpl StorageLooseT2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(30).Priority(4);
      }

      public static EntityCostsTpl StorageLooseT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(90).Priority(4);
      }

      public static EntityCostsTpl StorageLooseT4
      {
        get => (EntityCostsTpl) Costs.Build.CP3(180).Priority(4);
      }

      public static EntityCostsTpl StorageFluid => (EntityCostsTpl) Costs.Build.CP(30).Priority(4);

      public static EntityCostsTpl StorageFluidT2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(30).Priority(4);
      }

      public static EntityCostsTpl StorageFluidT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(90).Priority(4);
      }

      public static EntityCostsTpl StorageFluidT4
      {
        get => (EntityCostsTpl) Costs.Build.CP3(180).Priority(4);
      }

      public static EntityCostsTpl ThermalStorage
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(200).Product(600, Ids.Products.Salt).Workers(4).MaintenanceT1((Fix32) 4).Priority(8);
        }
      }

      public static EntityCostsTpl WasteSortingPlant
      {
        get => (EntityCostsTpl) Costs.Build.CP3(400).Workers(45).MaintenanceT1((Fix32) 20);
      }

      public static EntityCostsTpl OreSortingPlantT1
      {
        get => (EntityCostsTpl) Costs.Build.CP(80).Workers(6).MaintenanceT1Early(10);
      }

      public static EntityCostsTpl NuclearReactor
      {
        get => (EntityCostsTpl) Costs.Build.CP4(400).Workers(80).MaintenanceT2(24).Priority(8);
      }

      public static EntityCostsTpl NuclearReactorT2
      {
        get => (EntityCostsTpl) Costs.Build.CP4(700).Workers(110).MaintenanceT2(36).Priority(8);
      }

      public static EntityCostsTpl NuclearReactorT3
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP4(1500).Electronics3(200).Workers(200).MaintenanceT3(36).Priority(8);
        }
      }

      public static EntityCostsTpl NuclearWasteStorage
      {
        get => (EntityCostsTpl) Costs.Build.CP3(150).Concrete(500).Workers(10);
      }

      public static EntityCostsTpl FuelStationT1 => (EntityCostsTpl) Costs.Build.CP2(20);

      public static EntityCostsTpl FuelStationT2 => (EntityCostsTpl) Costs.Build.CP3(40);

      public static EntityCostsTpl FuelStationT3 => (EntityCostsTpl) Costs.Build.CP3(70);

      public static EntityCostsTpl MineTower => (EntityCostsTpl) Costs.Build.CP(20);

      public static EntityCostsTpl ForestryTower => (EntityCostsTpl) Costs.Build.CP(20);

      public static EntityCostsTpl VehiclesDepot
      {
        get => (EntityCostsTpl) Costs.Build.CP(40).Workers(6).Priority(7);
      }

      public static EntityCostsTpl VehiclesDepot2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(120).Workers(10).Priority(7);
      }

      public static EntityCostsTpl VehiclesDepot3
      {
        get => (EntityCostsTpl) Costs.Build.CP4(180).Workers(16).Priority(7);
      }

      public static EntityCostsTpl ResearchLab => (EntityCostsTpl) Costs.Build.Workers(4).CP(30);

      public static EntityCostsTpl ResearchLab2
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.Workers(8).CP2(60).Product(10, Ids.Products.LabEquipment).MaintenanceT1((Fix32) 8);
        }
      }

      public static EntityCostsTpl ResearchLab3
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.Workers(16).CP3(120).Product(20, Ids.Products.LabEquipment2).MaintenanceT1((Fix32) 12);
        }
      }

      public static EntityCostsTpl ResearchLab4
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.Workers(30).CP4(120).Product(20, Ids.Products.LabEquipment3).MaintenanceT2(8);
        }
      }

      public static EntityCostsTpl ResearchLab5
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.Workers(48).CP4(240).Product(20, Ids.Products.LabEquipment4).MaintenanceT2(16);
        }
      }

      public static EntityCostsTpl Beacon
      {
        get => (EntityCostsTpl) Costs.Build.CP(30).Copper(10).Workers(1);
      }

      public static EntityCostsTpl RainwaterHarvester
      {
        get => (EntityCostsTpl) Costs.Build.CP(20).Product(30, Ids.Products.Wood);
      }

      public static EntityCostsTpl TradeDock
      {
        get => (EntityCostsTpl) Costs.Build.Product(100, Ids.Products.Wood);
      }

      public static EntityCostsTpl Shipyard => (EntityCostsTpl) Costs.Build.CP(500).Concrete(800);

      public static EntityCostsTpl Shipyard2 => (EntityCostsTpl) Costs.Build.CP(600).Concrete(800);

      public static EntityCostsTpl VehicleRamp => (EntityCostsTpl) Costs.Build.CP(40);

      public static EntityCostsTpl VehicleRamp2 => (EntityCostsTpl) Costs.Build.CP(50).Concrete(80);

      public static EntityCostsTpl VehicleRamp3
      {
        get => (EntityCostsTpl) Costs.Build.CP(60).Concrete(160);
      }

      public static EntityCostsTpl Stacker
      {
        get => (EntityCostsTpl) Costs.Build.CP2(10).Product(10, Ids.Products.Rubber);
      }

      public static EntityCostsTpl MaintenanceDepotT0
      {
        get => (EntityCostsTpl) Costs.Build.CP(40).Workers(6);
      }

      public static EntityCostsTpl MaintenanceDepotT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(50).Workers(20);
      }

      public static EntityCostsTpl MaintenanceDepotT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).CP4(10).Workers(28);
      }

      public static EntityCostsTpl MaintenanceDepotT3
      {
        get => (EntityCostsTpl) Costs.Build.CP4(40).Electronics3(20).Workers(28);
      }

      public static EntityCostsTpl MainframeComputer
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP4(100).Electronics2(200).Product(60, Ids.Products.Gold).Workers(12).MaintenanceT2(15);
        }
      }

      public static EntityCostsTpl Datacenter
      {
        get => (EntityCostsTpl) Costs.Build.CP4(120).Electronics3(40).Workers(6).MaintenanceT3(4);
      }

      public static EntityCostsTpl CargoDepotT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Concrete(100);
      }

      public static EntityCostsTpl CargoDepotT2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(120).Concrete(240);
      }

      public static EntityCostsTpl CargoDepotT3
      {
        get => (EntityCostsTpl) Costs.Build.CP2(200).Concrete(400);
      }

      public static EntityCostsTpl CargoDepotT4
      {
        get => (EntityCostsTpl) Costs.Build.CP2(300).Concrete(600);
      }

      public static EntityCostsTpl CargoModuleUnitT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(3).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl CargoModuleUnitT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(60).Workers(4).MaintenanceT1((Fix32) 3);
      }

      public static EntityCostsTpl CargoModuleUnitT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(120).Workers(5).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl CargoModuleLooseT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(3).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl CargoModuleLooseT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(60).Workers(5).MaintenanceT1((Fix32) 3);
      }

      public static EntityCostsTpl CargoModuleLooseT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(120).Workers(5).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl CargoModuleFluidT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(3).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl CargoModuleFluidT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(60).Workers(4).MaintenanceT1((Fix32) 3);
      }

      public static EntityCostsTpl CargoModuleFluidT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(120).Workers(5).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl CaptainOfficeT1
      {
        get => (EntityCostsTpl) Costs.Build.CP(60).Workers(8).Priority(8);
      }

      public static EntityCostsTpl CaptainOfficeT2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(200).CP3(100).Workers(24).Priority(8);
      }

      public static EntityCostsTpl RocketAssemblyDepot
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP4(1200).Product(160, Ids.Products.VehicleParts3).Workers(160).Priority(7);
        }
      }

      public static EntityCostsTpl RocketLaunchPad
      {
        get => (EntityCostsTpl) Costs.Build.CP4(300).Concrete(1000).Steel(800).Workers(30);
      }

      public static EntityCostsTpl RetainingWall1
      {
        get => (EntityCostsTpl) Costs.Build.Iron(2).Concrete(6);
      }

      public static EntityCostsTpl RetainingWall2
      {
        get => (EntityCostsTpl) Costs.Build.Iron(3).Concrete(12);
      }

      public static EntityCostsTpl RetainingWall4
      {
        get => (EntityCostsTpl) Costs.Build.Iron(6).Concrete(24);
      }

      public static EntityCostsTpl Barrier1 => (EntityCostsTpl) Costs.Build.Concrete(1);

      public static EntityCostsTpl StatueOfMaintenance
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(200).Concrete(400).Steel(300).Copper(1000).MaintenanceT1((Fix32) 10);
        }
      }

      public static EntityCostsTpl StatueOfMaintenanceGolden
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(250).Steel(300).Copper(1000).Product(800, Ids.Products.Gold).MaintenanceT1((Fix32) 10);
        }
      }

      public static EntityCostsTpl TombOfCaptains1
      {
        get => (EntityCostsTpl) Costs.Build.CP3(400).Concrete(3000);
      }

      public static EntityCostsTpl TombOfCaptains2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(800).Concrete(5000);
      }

      public static EntityCostsTpl TombOfCaptains3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(1200).Concrete(6000);
      }

      public static EntityCostsTpl TombOfCaptains4
      {
        get => (EntityCostsTpl) Costs.Build.CP3(1600).Concrete(8000).Glass(500);
      }

      public static EntityCostsTpl TombOfCaptains5
      {
        get => (EntityCostsTpl) Costs.Build.CP3(2200).Concrete(10000).Glass(1000);
      }

      public static EntityCostsTpl TombOfCaptainsFinal
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(2800).Concrete(11000).Glass(2000).Product(2000, Ids.Products.Gold);
        }
      }
    }

    public static class CargoShip
    {
      public static EntityCostsTpl CargoShipT1
      {
        get => (EntityCostsTpl) Costs.Build.Workers(12).Priority(0);
      }

      public static EntityCostsTpl CargoShipT2
      {
        get => (EntityCostsTpl) Costs.Build.Workers(22).Priority(0);
      }

      public static EntityCostsTpl CargoShipT3
      {
        get => (EntityCostsTpl) Costs.Build.Workers(30).Priority(0);
      }

      public static EntityCostsTpl CargoShipT4
      {
        get => (EntityCostsTpl) Costs.Build.Workers(36).Priority(0);
      }
    }

    public static class Machines
    {
      public static EntityCostsTpl AssemblyManual => (EntityCostsTpl) Costs.Build.CP(25).Workers(4);

      public static EntityCostsTpl AssemblyElectrified
      {
        get => (EntityCostsTpl) Costs.Build.CP2(30).Workers(6).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl AssemblyElectrifiedT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(8).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl AssemblyRoboticT1
      {
        get => (EntityCostsTpl) Costs.Build.CP4(40).Workers(2).MaintenanceT2(4);
      }

      public static EntityCostsTpl AssemblyRoboticT2
      {
        get => (EntityCostsTpl) Costs.Build.CP4(80).Workers(2).MaintenanceT2(7);
      }

      public static EntityCostsTpl BioDieselDistiller
      {
        get => (EntityCostsTpl) Costs.Build.CP(40).Workers(6);
      }

      public static EntityCostsTpl CharcoalMaker => (EntityCostsTpl) Costs.Build.CP(20).Workers(2);

      public static EntityCostsTpl SmeltingFurnaceT1
      {
        get => (EntityCostsTpl) Costs.Build.CP(60).Workers(8);
      }

      public static EntityCostsTpl SmeltingFurnaceT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(180).Workers(18).MaintenanceT1((Fix32) 5);
      }

      public static EntityCostsTpl ArcFurnace
      {
        get => (EntityCostsTpl) Costs.Build.CP3(160).Workers(12).MaintenanceT1((Fix32) 5);
      }

      public static EntityCostsTpl ArcFurnace2
      {
        get => (EntityCostsTpl) Costs.Build.CP4(140).Workers(18).MaintenanceT2(5);
      }

      public static EntityCostsTpl Boiler
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(6).Priority(8);
      }

      public static EntityCostsTpl BoilerGas
      {
        get => (EntityCostsTpl) Costs.Build.CP2(50).Workers(6).Priority(8);
      }

      public static EntityCostsTpl BoilerElectric
      {
        get => (EntityCostsTpl) Costs.Build.CP4(30).Workers(2).MaintenanceT2(2).Priority(8);
      }

      public static EntityCostsTpl DieselGenerator
      {
        get => (EntityCostsTpl) Costs.Build.CP(20).Electronics(20).Workers(2).MaintenanceT1Early(6);
      }

      public static EntityCostsTpl DieselGeneratorT2
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP3(40).Electronics(60).Workers(2).MaintenanceT1((Fix32) 10);
        }
      }

      public static EntityCostsTpl Flare => (EntityCostsTpl) Costs.Build.CP(10).Workers(1);

      public static EntityCostsTpl SolidBurner => (EntityCostsTpl) Costs.Build.CP2(15).Workers(1);

      public static EntityCostsTpl Compactor => (EntityCostsTpl) Costs.Build.CP3(25).Workers(2);

      public static EntityCostsTpl Shredder => (EntityCostsTpl) Costs.Build.CP3(25).Workers(2);

      public static EntityCostsTpl IncinerationPlant
      {
        get => (EntityCostsTpl) Costs.Build.CP3(180).Workers(18);
      }

      public static EntityCostsTpl Caster => (EntityCostsTpl) Costs.Build.CP(30).Workers(2);

      public static EntityCostsTpl Caster2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Steel(20).Workers(3);
      }

      public static EntityCostsTpl CasterCooled => (EntityCostsTpl) Costs.Build.CP2(30).Workers(4);

      public static EntityCostsTpl CasterCooledT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(6);
      }

      public static EntityCostsTpl PowerGeneratorT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(20).Electronics(20).MaintenanceT1((Fix32) 1);
      }

      public static EntityCostsTpl PowerGeneratorT2
      {
        get => (EntityCostsTpl) Costs.Build.CP4(40).Electronics(180).MaintenanceT1((Fix32) 6);
      }

      public static EntityCostsTpl Flywheel
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Product(80, Ids.Products.Iron);
      }

      public static EntityCostsTpl SmokeStack => (EntityCostsTpl) Costs.Build.CP(10);

      public static EntityCostsTpl SmokeStackLarge
      {
        get => (EntityCostsTpl) Costs.Build.CP(10).Concrete(40);
      }

      public static EntityCostsTpl TurbineSuperPress
      {
        get => (EntityCostsTpl) Costs.Build.CP4(40).Workers(1).MaintenanceT2(3).Priority(8);
      }

      public static EntityCostsTpl TurbineHighPress
      {
        get => (EntityCostsTpl) Costs.Build.CP2(50).Workers(2).MaintenanceT1((Fix32) 2).Priority(8);
      }

      public static EntityCostsTpl TurbineHighPressT2
      {
        get => (EntityCostsTpl) Costs.Build.CP4(40).Workers(2).MaintenanceT2(2).Priority(8);
      }

      public static EntityCostsTpl TurbineLowPress
      {
        get => (EntityCostsTpl) Costs.Build.CP3(60).Workers(2).MaintenanceT1((Fix32) 2).Priority(8);
      }

      public static EntityCostsTpl TurbineLowPressT2
      {
        get => (EntityCostsTpl) Costs.Build.CP4(60).Workers(2).MaintenanceT2(2).Priority(8);
      }

      public static EntityCostsTpl BricksMaker
      {
        get => (EntityCostsTpl) Costs.Build.CP(50).Workers(6).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl RotaryKiln
      {
        get => (EntityCostsTpl) Costs.Build.CP2(30).Workers(6).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl RotaryKilnGas
      {
        get => (EntityCostsTpl) Costs.Build.CP2(60).Workers(10).MaintenanceT1((Fix32) 3);
      }

      public static EntityCostsTpl Mixer
      {
        get => (EntityCostsTpl) Costs.Build.CP2(30).Workers(4).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl MixerT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(7).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl MixerT3
      {
        get => (EntityCostsTpl) Costs.Build.CP4(40).Workers(10).MaintenanceT1((Fix32) 6);
      }

      public static EntityCostsTpl FiltrationStation
      {
        get => (EntityCostsTpl) Costs.Build.CP2(80).Workers(8).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl CoolingTower
      {
        get => (EntityCostsTpl) Costs.Build.CP3(30).Concrete(40).Workers(2).Priority(8);
      }

      public static EntityCostsTpl CoolingTowerT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(80).Concrete(200).Workers(4).Priority(8);
      }

      public static EntityCostsTpl FoodProcessor
      {
        get => (EntityCostsTpl) Costs.Build.CP3(30).Workers(8).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl BakingUnit
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(8).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl FoodMill
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(5).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl IndustrialMixer
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(4).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl IndustrialMixerT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(4).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl ReprocessingPlant
      {
        get => (EntityCostsTpl) Costs.Build.CP4(300).Workers(30).MaintenanceT2(10);
      }

      public static EntityCostsTpl DistillationTowerT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(50).Workers(6).MaintenanceT1((Fix32) 3);
      }

      public static EntityCostsTpl DistillationTowerT2
      {
        get => (EntityCostsTpl) Costs.Build.CP2(70).Workers(8).MaintenanceT1((Fix32) 3);
      }

      public static EntityCostsTpl DistillationTowerT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(70).Workers(8).MaintenanceT1((Fix32) 3);
      }

      public static EntityCostsTpl HydroCrackerT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(100).Workers(12).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl PolymerizationPlant
      {
        get => (EntityCostsTpl) Costs.Build.CP3(100).Workers(12).MaintenanceT1((Fix32) 6);
      }

      public static EntityCostsTpl HydrogenReformer
      {
        get => (EntityCostsTpl) Costs.Build.CP3(50).Workers(12).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl SourWaterStripper
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(10).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl AnaerobicDigester
      {
        get => (EntityCostsTpl) Costs.Build.CP2(50).Workers(4).MaintenanceT1((Fix32) 1);
      }

      public static EntityCostsTpl FermentationTank
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(4).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl OxygenFurnace
      {
        get => (EntityCostsTpl) Costs.Build.CP2(60).Workers(6).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl OxygenFurnace2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(70).Workers(10).MaintenanceT1((Fix32) 6);
      }

      public static EntityCostsTpl CopperElectrolysis
      {
        get => (EntityCostsTpl) Costs.Build.CP(30).Workers(5).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl GlassMakerT1
      {
        get => (EntityCostsTpl) Costs.Build.CP3(50).Workers(6).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl GlassMakerT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(120).Workers(14).MaintenanceT1((Fix32) 8);
      }

      public static EntityCostsTpl Crusher
      {
        get => (EntityCostsTpl) Costs.Build.CP2(20).Workers(2).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl CrusherLarge
      {
        get => (EntityCostsTpl) Costs.Build.CP3(120).Workers(6).MaintenanceT1((Fix32) 8);
      }

      public static EntityCostsTpl SettlingTank
      {
        get => (EntityCostsTpl) Costs.Build.CP3(80).Workers(6).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl GoldFurnace
      {
        get => (EntityCostsTpl) Costs.Build.CP3(70).Workers(6).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl SiliconReactor
      {
        get => (EntityCostsTpl) Costs.Build.CP3(20).Workers(2).MaintenanceT1((Fix32) 1);
      }

      public static EntityCostsTpl SiliconCrystallizer
      {
        get => (EntityCostsTpl) Costs.Build.CP4(40).Workers(8).MaintenanceT2(5);
      }

      public static EntityCostsTpl MicrochipMachine
      {
        get => (EntityCostsTpl) Costs.Build.CP4(60).Workers(4).MaintenanceT2(6);
      }

      public static EntityCostsTpl MicrochipMachineT2
      {
        get => (EntityCostsTpl) Costs.Build.CP4(120).Workers(6).MaintenanceT2(10);
      }

      public static EntityCostsTpl ChemicalPlant
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(8).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl ChemicalPlant2
      {
        get => (EntityCostsTpl) Costs.Build.CP4(60).Workers(14).MaintenanceT2(4);
      }

      public static EntityCostsTpl AirSeparator
      {
        get => (EntityCostsTpl) Costs.Build.CP2(100).Workers(6).MaintenanceT1((Fix32) 6);
      }

      public static EntityCostsTpl Electrolyzer
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(3).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl ElectrolyzerT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(80).Workers(4).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl OilPump
      {
        get => (EntityCostsTpl) Costs.Build.CP(30).Workers(1).MaintenanceT1Early(2);
      }

      public static EntityCostsTpl UraniumEnrichmentPlant
      {
        get => (EntityCostsTpl) Costs.Build.CP4(80).Workers(14).MaintenanceT2(4);
      }

      public static EntityCostsTpl OceanWaterPump
      {
        get => (EntityCostsTpl) Costs.Build.CP2(50).Workers(1).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl OceanWaterPumpLarge
      {
        get => (EntityCostsTpl) Costs.Build.CP3(50).Workers(1).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl EvaporationPond
      {
        get => (EntityCostsTpl) Costs.Build.CP3(30).Workers(4).MaintenanceT1((Fix32) 1);
      }

      public static EntityCostsTpl EvaporationPondHeated
      {
        get => (EntityCostsTpl) Costs.Build.CP3(50).Workers(6).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl ThermalDesalinator
      {
        get => (EntityCostsTpl) Costs.Build.CP3(30).Steel(30).Workers(4).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl WasteWaterPump => (EntityCostsTpl) Costs.Build.CP(20).Workers(1);

      public static EntityCostsTpl LandWaterPump
      {
        get => (EntityCostsTpl) Costs.Build.CP2(40).Workers(2).MaintenanceT1((Fix32) 4).Priority(8);
      }

      public static EntityCostsTpl GasInjectionPump
      {
        get => (EntityCostsTpl) Costs.Build.CP3(50).Workers(4).MaintenanceT1((Fix32) 4);
      }

      public static EntityCostsTpl WaterTreatmentPlant
      {
        get => (EntityCostsTpl) Costs.Build.CP3(140).Workers(26).MaintenanceT1((Fix32) 10);
      }

      public static EntityCostsTpl VacuumDistillationTower
      {
        get => (EntityCostsTpl) Costs.Build.CP(60).Workers(6).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl WaterChiller
      {
        get => (EntityCostsTpl) Costs.Build.CP3(40).Workers(3).MaintenanceT1((Fix32) 2);
      }

      public static EntityCostsTpl SolarPanel
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP4(10).Product(120, Ids.Products.SolarCell).MaintenanceT2(0.8.ToFix32());
        }
      }

      public static EntityCostsTpl SolarPanelMono
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.CP4(10).Product(120, Ids.Products.SolarCellMono).MaintenanceT2(0.8.ToFix32());
        }
      }
    }

    public static class Rockets
    {
      public static EntityCostsTpl TestingRocketT0
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.Product(300, Ids.Products.Steel).Product(30, Ids.Products.Gold).Product(80, Ids.Products.Electronics3);
        }
      }
    }

    public static class Transports
    {
      public static readonly RelTile1i LENGTH_PER_COST;

      public static EntityCostsTpl FlatConveyorT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(2).Product(2, Ids.Products.Rubber).Priority(4);
      }

      public static EntityCostsTpl FlatConveyorT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(2).Product(4, Ids.Products.Rubber).Priority(4);
      }

      public static EntityCostsTpl FlatConveyorT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(4).Product(6, Ids.Products.Rubber).Priority(4);
      }

      public static EntityCostsTpl LooseConveyorT1
      {
        get => (EntityCostsTpl) Costs.Build.CP2(2).Product(2, Ids.Products.Rubber).Priority(4);
      }

      public static EntityCostsTpl LooseConveyorT2
      {
        get => (EntityCostsTpl) Costs.Build.CP3(2).Product(4, Ids.Products.Rubber).Priority(4);
      }

      public static EntityCostsTpl LooseConveyorT3
      {
        get => (EntityCostsTpl) Costs.Build.CP3(4).Product(6, Ids.Products.Rubber).Priority(4);
      }

      public static EntityCostsTpl Pipe => (EntityCostsTpl) Costs.Build.CP(2);

      public static EntityCostsTpl PipeT2 => (EntityCostsTpl) Costs.Build.CP2(2);

      public static EntityCostsTpl PipeT3 => (EntityCostsTpl) Costs.Build.CP3(2);

      public static EntityCostsTpl MoltenMetalChannel => (EntityCostsTpl) Costs.Build.CP(2);

      public static EntityCostsTpl Shaft => (EntityCostsTpl) Costs.Build.CP(2);

      public static EntityCostsTpl FlatZipper => (EntityCostsTpl) Costs.Build.CP2(8).Priority(4);

      public static EntityCostsTpl LooseZipper => (EntityCostsTpl) Costs.Build.CP2(8).Priority(4);

      public static EntityCostsTpl FluidZipper => (EntityCostsTpl) Costs.Build.CP2(8).Priority(4);

      public static EntityCostsTpl MoltenZipper => (EntityCostsTpl) Costs.Build.CP2(8).Priority(4);

      public static EntityCostsTpl FlatSorter => (EntityCostsTpl) Costs.Build.CP2(12).Priority(4);

      public static EntityCostsTpl LooseSorter => (EntityCostsTpl) Costs.Build.CP2(12).Priority(4);

      public static EntityCostsTpl LiftBase => (EntityCostsTpl) Costs.Build.CP2(6).Priority(4);

      public static EntityCostsTpl LiftPerHeightDelta => (EntityCostsTpl) Costs.Build.CP2(2);

      static Transports()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Costs.Transports.LENGTH_PER_COST = new RelTile1i(7);
      }
    }

    public static class Vehicles
    {
      public static Percent HYDROGEN_MAINTENANCE_RATIO;

      public static EntityCostsTpl TruckT1
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.Product(20, Ids.Products.VehicleParts).Product(10, Ids.Products.Rubber).Workers(1).Priority(0).MaintenanceT1Early(2);
        }
      }

      public static EntityCostsTpl TruckT2 => (EntityCostsTpl) Costs.Vehicles.TruckT2Builder;

      public static EntityCostsTpl TruckT2H
      {
        get
        {
          return (EntityCostsTpl) Costs.Vehicles.TruckT2Builder.Product(20, Ids.Products.Electronics).MaintenanceT1(4.ScaledByToFix32(Costs.Vehicles.HYDROGEN_MAINTENANCE_RATIO));
        }
      }

      private static EntityCostsTpl.Builder TruckT2Builder
      {
        get
        {
          return Costs.Build.Product(40, Ids.Products.VehicleParts2).Product(30, Ids.Products.Rubber).Workers(1).Priority(0).MaintenanceT1((Fix32) 4);
        }
      }

      public static EntityCostsTpl TruckT3 => (EntityCostsTpl) Costs.Vehicles.TruckT3Builder;

      public static EntityCostsTpl TruckT3H
      {
        get
        {
          return (EntityCostsTpl) Costs.Vehicles.TruckT3Builder.ReplaceProduct(140, Ids.Products.VehicleParts3).MaintenanceT2(6.ScaledByToFix32(Costs.Vehicles.HYDROGEN_MAINTENANCE_RATIO));
        }
      }

      private static EntityCostsTpl.Builder TruckT3Builder
      {
        get
        {
          return Costs.Build.Product(120, Ids.Products.VehicleParts3).Product(120, Ids.Products.Rubber).Workers(1).Priority(0).MaintenanceT2(6);
        }
      }

      public static EntityCostsTpl ExcavatorT1
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.Product(40, Ids.Products.VehicleParts).Product(20, Ids.Products.Iron).Workers(1).Priority(0).MaintenanceT1Early(6);
        }
      }

      public static EntityCostsTpl ExcavatorT2
      {
        get => (EntityCostsTpl) Costs.Vehicles.ExcavatorT2Builder;
      }

      public static EntityCostsTpl ExcavatorT2H
      {
        get
        {
          return (EntityCostsTpl) Costs.Vehicles.ExcavatorT2Builder.Product(40, Ids.Products.Electronics).MaintenanceT1(12.ScaledByToFix32(Costs.Vehicles.HYDROGEN_MAINTENANCE_RATIO));
        }
      }

      private static EntityCostsTpl.Builder ExcavatorT2Builder
      {
        get
        {
          return Costs.Build.Product(80, Ids.Products.VehicleParts2).Product(40, Ids.Products.Steel).Workers(1).Priority(0).MaintenanceT1((Fix32) 12);
        }
      }

      public static EntityCostsTpl ExcavatorT3
      {
        get => (EntityCostsTpl) Costs.Vehicles.ExcavatorT3Builder;
      }

      public static EntityCostsTpl ExcavatorT3H
      {
        get
        {
          return (EntityCostsTpl) Costs.Vehicles.ExcavatorT3Builder.ReplaceProduct(280, Ids.Products.VehicleParts3).MaintenanceT2(18.ScaledByToFix32(Costs.Vehicles.HYDROGEN_MAINTENANCE_RATIO));
        }
      }

      private static EntityCostsTpl.Builder ExcavatorT3Builder
      {
        get
        {
          return Costs.Build.Product(240, Ids.Products.VehicleParts3).Product(120, Ids.Products.Steel).Workers(1).Priority(0).MaintenanceT2(18);
        }
      }

      public static EntityCostsTpl TreeHarvesterT1
      {
        get
        {
          return (EntityCostsTpl) Costs.Build.Product(30, Ids.Products.VehicleParts).Product(10, Ids.Products.Iron).Workers(1).Priority(0).MaintenanceT1Early(4);
        }
      }

      public static EntityCostsTpl TreeHarvesterT2
      {
        get => (EntityCostsTpl) Costs.Vehicles.TreeHarvesterT2Builder;
      }

      public static EntityCostsTpl TreeHarvesterT2H
      {
        get
        {
          return (EntityCostsTpl) Costs.Vehicles.TreeHarvesterT2Builder.Product(40, Ids.Products.Electronics).MaintenanceT1(6.ScaledByToFix32(Costs.Vehicles.HYDROGEN_MAINTENANCE_RATIO));
        }
      }

      private static EntityCostsTpl.Builder TreeHarvesterT2Builder
      {
        get
        {
          return Costs.Build.Product(60, Ids.Products.VehicleParts2).Product(20, Ids.Products.Steel).Workers(1).Priority(0).MaintenanceT1((Fix32) 6);
        }
      }

      public static EntityCostsTpl TreePlanterT1
      {
        get => (EntityCostsTpl) Costs.Vehicles.TreePlanterT1Builder;
      }

      public static EntityCostsTpl TreePlanterT1H
      {
        get
        {
          return (EntityCostsTpl) Costs.Vehicles.TreePlanterT1Builder.Product(40, Ids.Products.Electronics).MaintenanceT1(4.ScaledByToFix32(Costs.Vehicles.HYDROGEN_MAINTENANCE_RATIO));
        }
      }

      private static EntityCostsTpl.Builder TreePlanterT1Builder
      {
        get
        {
          return Costs.Build.Product(30, Ids.Products.VehicleParts).Product(10, Ids.Products.Iron).Workers(1).Priority(0).MaintenanceT1Early(4);
        }
      }

      static Vehicles()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        Costs.Vehicles.HYDROGEN_MAINTENANCE_RATIO = 90.Percent();
      }
    }
  }
}
