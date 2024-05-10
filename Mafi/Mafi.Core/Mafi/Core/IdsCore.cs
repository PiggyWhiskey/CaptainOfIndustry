// Decompiled with JetBrains decompiler
// Type: Mafi.Core.IdsCore
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.World.Entities;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core
{
  public static class IdsCore
  {
    public static class Buildings
    {
      public static readonly StaticEntityProto.ID MineTower;

      static Buildings()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.Buildings.MineTower = new StaticEntityProto.ID(nameof (MineTower));
      }
    }

    public static class TerrainDesignators
    {
      public const string DESIGNATOR_SUFFIX = "Designator";
      public const string MINING_DESIGNATOR = "MiningDesignator";
      public static Proto.ID MiningDesignator;
      [OnlyForSaveCompatibility(null)]
      public const string FORESTRY_DESIGNATOR = "ForestryDesignator";
      public static Proto.ID ForestryDesignator;
      public const string DUMPING_DESIGNATOR = "DumpingDesignator";
      public static Proto.ID DumpingDesignator;
      public const string LEVEL_DESIGNATOR = "LevelDesignator";
      public static Proto.ID LevelDesignator;
      [OnlyForSaveCompatibility(null)]
      public const string PLACE_SURFACE_DESIGNATOR = "ConcreteDesignator";
      public static Proto.ID PlaceSurfaceDesignator;
      public const string CLEAR_SURFACE_DESIGNATOR = "ClearSurfaceDesignator";
      public static Proto.ID ClearSurfaceDesignator;
      public const string PLACE_DECAL_DESIGNATOR = "PlaceDecalDesignator";
      public static Proto.ID PlaceDecalDesignator;
      public const string CLEAR_DECAL_DESIGNATOR = "ClearDecalDesignator";
      public static Proto.ID ClearDecalDesignator;

      static TerrainDesignators()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.TerrainDesignators.MiningDesignator = new Proto.ID(nameof (MiningDesignator));
        IdsCore.TerrainDesignators.ForestryDesignator = new Proto.ID(nameof (ForestryDesignator));
        IdsCore.TerrainDesignators.DumpingDesignator = new Proto.ID(nameof (DumpingDesignator));
        IdsCore.TerrainDesignators.LevelDesignator = new Proto.ID(nameof (LevelDesignator));
        IdsCore.TerrainDesignators.PlaceSurfaceDesignator = new Proto.ID("ConcreteDesignator");
        IdsCore.TerrainDesignators.ClearSurfaceDesignator = new Proto.ID(nameof (ClearSurfaceDesignator));
        IdsCore.TerrainDesignators.PlaceDecalDesignator = new Proto.ID(nameof (PlaceDecalDesignator));
        IdsCore.TerrainDesignators.ClearDecalDesignator = new Proto.ID(nameof (ClearDecalDesignator));
      }
    }

    public static class Technology
    {
      public static readonly Proto.ID CustomRoutes;
      [OnlyForSaveCompatibility(null)]
      public static readonly Proto.ID MechPowerAutoBalance;
      public static readonly Proto.ID CustomSurfaces;
      public static readonly Proto.ID Recycling;
      public static readonly Proto.ID CropRotation;
      public static readonly Proto.ID Blueprints;
      public static readonly Proto.ID CopyTool;
      public static readonly Proto.ID CutTool;
      public static readonly Proto.ID CloneTool;
      public static readonly Proto.ID UnityTool;
      public static readonly Proto.ID PauseTool;
      public static readonly Proto.ID UpgradeTool;
      public static readonly Proto.ID PlanningTool;
      public static readonly Proto.ID TerrainLeveling;

      private static Proto.ID createTechId(string id) => new Proto.ID(nameof (Technology) + id);

      static Technology()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.Technology.CustomRoutes = IdsCore.Technology.createTechId(nameof (CustomRoutes));
        IdsCore.Technology.MechPowerAutoBalance = IdsCore.Technology.createTechId(nameof (MechPowerAutoBalance));
        IdsCore.Technology.CustomSurfaces = IdsCore.Technology.createTechId(nameof (CustomSurfaces));
        IdsCore.Technology.Recycling = IdsCore.Technology.createTechId(nameof (Recycling));
        IdsCore.Technology.CropRotation = IdsCore.Technology.createTechId(nameof (CropRotation));
        IdsCore.Technology.Blueprints = IdsCore.Technology.createTechId(nameof (Blueprints));
        IdsCore.Technology.CopyTool = IdsCore.Technology.createTechId(nameof (CopyTool));
        IdsCore.Technology.CutTool = IdsCore.Technology.createTechId(nameof (CutTool));
        IdsCore.Technology.CloneTool = IdsCore.Technology.createTechId(nameof (CloneTool));
        IdsCore.Technology.UnityTool = IdsCore.Technology.createTechId(nameof (UnityTool));
        IdsCore.Technology.PauseTool = IdsCore.Technology.createTechId(nameof (PauseTool));
        IdsCore.Technology.UpgradeTool = IdsCore.Technology.createTechId(nameof (UpgradeTool));
        IdsCore.Technology.PlanningTool = IdsCore.Technology.createTechId(nameof (PlanningTool));
        IdsCore.Technology.TerrainLeveling = IdsCore.Technology.createTechId(nameof (TerrainLeveling));
      }
    }

    public static class Products
    {
      public const string PREFIX = "Product_";
      public const string VIRTUAL_PREFIX = "Product_Virtual_";
      public const string VIRTUAL_RESOURCE_PREFIX = "Product_VirtualResource_";
      public static readonly Proto.ID VirtualCrudeOil;
      public const string GROUND_WATER = "Product_Virtual_Groundwater";
      public static readonly Proto.ID Groundwater;
      public const string POLLUTED_WATER = "Product_Virtual_PollutedWater";
      public static readonly ProductProto.ID PollutedWater;
      public const string POLLUTED_AIR = "Product_Virtual_PollutedAir";
      public static readonly ProductProto.ID PollutedAir;
      public const string MECHANICAL_POWER = "Product_Virtual_MechPower";
      public static readonly ProductProto.ID MechanicalPower;
      public const string ELECTRICITY = "Product_Virtual_Electricity";
      public static readonly ProductProto.ID Electricity;
      public const string COMPUTING = "Product_Virtual_Computing";
      public static readonly ProductProto.ID Computing;
      public const string UPOINTS = "Product_Virtual_Upoints";
      public static readonly ProductProto.ID Upoints;
      public const string DIESEL = "Product_Diesel";
      public static readonly ProductProto.ID Diesel;
      public const string CONCRETE_SLAB = "ConcreteSlab";
      public static readonly ProductProto.ID ConcreteSlab;
      public const string WOOD = "Product_Wood";
      public static readonly ProductProto.ID Wood;
      public const string CLEAN_WATER = "Product_Water";
      public static readonly ProductProto.ID CleanWater;
      public const string WASTE = "Product_Waste";
      public static readonly ProductProto.ID Waste;
      public const string BIOMASS = "Product_Biomass";
      public static readonly ProductProto.ID Biomass;
      public const string RECYCLABLES = "Product_Recyclables";
      public static readonly ProductProto.ID Recyclables;

      static Products()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.Products.VirtualCrudeOil = new Proto.ID("Product_VirtualResource_CrudeOil");
        IdsCore.Products.Groundwater = new Proto.ID("Product_Virtual_Groundwater");
        IdsCore.Products.PollutedWater = new ProductProto.ID("Product_Virtual_PollutedWater");
        IdsCore.Products.PollutedAir = new ProductProto.ID("Product_Virtual_PollutedAir");
        IdsCore.Products.MechanicalPower = new ProductProto.ID("Product_Virtual_MechPower");
        IdsCore.Products.Electricity = new ProductProto.ID("Product_Virtual_Electricity");
        IdsCore.Products.Computing = new ProductProto.ID("Product_Virtual_Computing");
        IdsCore.Products.Upoints = new ProductProto.ID("Product_Virtual_Upoints");
        IdsCore.Products.Diesel = new ProductProto.ID("Product_Diesel");
        IdsCore.Products.ConcreteSlab = new ProductProto.ID("Product_ConcreteSlab");
        IdsCore.Products.Wood = new ProductProto.ID("Product_Wood");
        IdsCore.Products.CleanWater = new ProductProto.ID("Product_Water");
        IdsCore.Products.Waste = new ProductProto.ID("Product_Waste");
        IdsCore.Products.Biomass = new ProductProto.ID("Product_Biomass");
        IdsCore.Products.Recyclables = new ProductProto.ID("Product_Recyclables");
      }
    }

    public static class TerrainMaterials
    {
      public static readonly Proto.ID HardenedRock;
      public static readonly Proto.ID Grass;
      public static readonly Proto.ID FarmGround;
      public static readonly Proto.ID Landfill;
      public const string BEDROCK_SOLID = "Bedrock_Terrain";
      public static readonly Proto.ID Bedrock;

      public static Proto.ID CreateId(string id) => new Proto.ID(id + "_Terrain");

      static TerrainMaterials()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.TerrainMaterials.HardenedRock = IdsCore.TerrainMaterials.CreateId(nameof (HardenedRock));
        IdsCore.TerrainMaterials.Grass = IdsCore.TerrainMaterials.CreateId(nameof (Grass));
        IdsCore.TerrainMaterials.FarmGround = IdsCore.TerrainMaterials.CreateId(nameof (FarmGround));
        IdsCore.TerrainMaterials.Landfill = IdsCore.TerrainMaterials.CreateId(nameof (Landfill));
        IdsCore.TerrainMaterials.Bedrock = new Proto.ID("Bedrock_Terrain");
      }
    }

    public static class TerrainTileSurfaces
    {
      public static readonly Proto.ID DefaultConcrete;

      public static Proto.ID CreateId(string id) => new Proto.ID(id + "_TerrainSurface");

      static TerrainTileSurfaces()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.TerrainTileSurfaces.DefaultConcrete = IdsCore.TerrainTileSurfaces.CreateId(nameof (DefaultConcrete));
      }
    }

    public static class Notifications
    {
      public static readonly EntityNotificationProto.ID UpgradeInProgress;
      public static readonly EntityNotificationProto.ID ConstructionPrioritized;
      public static readonly GeneralNotificationProto.ID Homeless;
      public static readonly GeneralNotificationProto.ID LowFoodSupply;
      public static readonly GeneralNotificationProto.ID PopsStarving;
      public static readonly GeneralNotificationProto<int>.ID PopsStarvedToDeath;
      public static readonly GeneralNotificationProto<int>.ID HomelessLeft;
      public static readonly EntityNotificationProto<Proto>.ID CropWillDrySoon;
      public static readonly EntityNotificationProto<Proto>.ID CropLacksMaintenance;
      public static readonly EntityNotificationProto<Proto>.ID CropDiedNoWater;
      public static readonly EntityNotificationProto<Proto>.ID CropDiedNoFertility;
      public static readonly EntityNotificationProto<Proto>.ID CropDiedNoMaintenance;
      public static readonly EntityNotificationProto<Proto>.ID CropCouldNotBeStored;
      public static readonly EntityNotificationProto.ID LowFarmFertility;
      public static readonly EntityNotificationProto.ID NoCropToGrow;
      public static readonly EntityNotificationProto.ID NotEnoughWorkers;
      public static readonly EntityNotificationProto.ID EntityIsBoosted;
      public static readonly EntityNotificationProto.ID VehicleIsBroken;
      public static readonly EntityNotificationProto.ID MachineIsBroken;
      public static readonly EntityNotificationProto<Proto>.ID TruckCannotDeliver;
      public static readonly EntityNotificationProto.ID TruckCannotDeliverMixedCargo;
      public static readonly EntityNotificationProto.ID SortingPlantNoProductSet;
      public static readonly EntityNotificationProto.ID SortingPlantBlockedOutput;
      public static readonly EntityNotificationProto.ID VehicleGoalUnreachable;
      public static readonly EntityNotificationProto.ID VehicleGoalUnreachableCannotGoUnder;
      public static readonly EntityNotificationProto.ID VehicleGoalStruggling;
      public static readonly EntityNotificationProto.ID VehicleGoalStrugglingCannotGoUnder;
      public static readonly EntityNotificationProto.ID VehicleNoFuel;
      public static readonly EntityNotificationProto.ID EntityCannotBeReached;
      public static readonly EntityNotificationProto.ID TruckHasNoValidExcavator;
      public static readonly EntityNotificationProto.ID ExcavatorHasNoValidTruck;
      public static readonly EntityNotificationProto.ID NoTruckAssignedToTreeHarvester;
      public static readonly EntityNotificationProto.ID NoTreeSaplingsForPlanter;
      public static readonly EntityNotificationProto.ID NoTreesToHarvest;
      public static readonly GeneralNotificationProto.ID NotEnoughElectricity;
      public static readonly EntityNotificationProto.ID NotEnoughElectricityForEntity;
      public static readonly EntityNotificationProto.ID NotEnoughComputingForEntity;
      public static readonly EntityNotificationProto.ID NoResourceToExtract;
      public static readonly EntityNotificationProto<Proto>.ID ResourceIsLow;
      public static readonly GeneralNotificationProto.ID LowGroundwater;
      public static readonly GeneralNotificationProto.ID NotEnoughFuelToRefuel;
      public static readonly EntityNotificationProto.ID FuelStationOutOfFuel;
      public static readonly EntityNotificationProto.ID FuelStationNotConnected;
      public static readonly EntityNotificationProto.ID NoMineDesignInTowerArea;
      public static readonly EntityNotificationProto.ID NoAvailableMineDesignInTowerArea;
      public static readonly EntityNotificationProto.ID NoForestryDesignInTowerArea;
      public static readonly EntityNotificationProto.ID NoAvailableForestryDesignInTowerArea;
      public static readonly EntityNotificationProto.ID CannotDeliverFromMineTower;
      public static readonly GeneralNotificationProto.ID AreasWithoutTowers;
      public static readonly GeneralNotificationProto.ID AreasWithoutForestryTowers;
      public static readonly EntityNotificationProto.ID VehicleNoReachableDesignations;
      public static readonly EntityNotificationProto.ID NoRecipeSelected;
      public static readonly EntityNotificationProto<Proto>.ID NeedsTransportConnected;
      public static readonly EntityNotificationProto.ID TransportTooLong;
      public static readonly GeneralNotificationProto<EntityProto>.ID ShipCargoLoaded;
      public static readonly GeneralNotificationProto<EntityProto>.ID ShipCargoDelivered;
      public static readonly GeneralNotificationProto.ID ShipRepaired;
      public static readonly GeneralNotificationProto.ID ShipModified;
      public static readonly EntityNotificationProto.ID OceanAccessBlocked;
      public static readonly GeneralNotificationProto<WorldMapMineProto>.ID WorldEntityRepaired;
      public static readonly EntityNotificationProto.ID CargoShipMissingFuel;
      public static readonly EntityNotificationProto.ID CargoShipContractLacksUpoints;
      public static readonly EntityNotificationProto.ID CargoDepotHasNoShip;
      public static readonly EntityNotificationProto.ID CargoDepotHasNoModule;
      public static readonly GeneralNotificationProto.ID NotEnoughUpoints;
      public static readonly EntityNotificationProto.ID NotEnoughUpointsForEntity;
      public static readonly EntityNotificationProto.ID LabCannotResearchHigherTech;
      public static readonly EntityNotificationProto.ID LabMissingInputProducts;
      public static readonly EntityNotificationProto.ID SettlementHasNoFoodModule;
      public static readonly EntityNotificationProto.ID SettlementIsStarving;
      public static readonly EntityNotificationProto.ID SettlementFullOfLandfill;
      public static readonly EntityNotificationProto.ID NoProductAssignedToEntity;
      public static readonly GeneralNotificationProto<Proto>.ID NotEnoughMaintenance;
      public static readonly EntityNotificationProto.ID CargoDepotModuleNoProductAssigned;
      public static readonly EntityNotificationProto.ID CargoDepotModuleContractNotMatching;
      public static readonly GeneralNotificationProto<string>.ID NewErrorOccurred;
      public static readonly EntityNotificationProto.ID NuclearReactorInMeltdown;
      public static readonly EntityNotificationProto.ID NuclearReactorLacksMaintenance;
      public static readonly EntityNotificationProto<Proto>.ID StorageSupplyTooLow;
      public static readonly EntityNotificationProto<Proto>.ID StorageSupplyTooHigh;
      public static readonly EntityNotificationProto.ID EntityMayCollapseUnevenTerrain;
      public static readonly EntityNotificationProto.ID AnimalFarmMissingFood;
      public static readonly EntityNotificationProto.ID AnimalFarmMissingWater;
      public static readonly EntityNotificationProto.ID InvalidImportRoute;
      public static readonly EntityNotificationProto.ID InvalidExportRoute;
      public static readonly GeneralNotificationProto<Proto>.ID LoanPaymentDelayed;
      public static readonly GeneralNotificationProto<Proto>.ID LoanPaymentFailed;

      static Notifications()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.Notifications.UpgradeInProgress = new EntityNotificationProto.ID(nameof (UpgradeInProgress));
        IdsCore.Notifications.ConstructionPrioritized = new EntityNotificationProto.ID(nameof (ConstructionPrioritized));
        IdsCore.Notifications.Homeless = new GeneralNotificationProto.ID(nameof (Homeless));
        IdsCore.Notifications.LowFoodSupply = new GeneralNotificationProto.ID(nameof (LowFoodSupply));
        IdsCore.Notifications.PopsStarving = new GeneralNotificationProto.ID(nameof (PopsStarving));
        IdsCore.Notifications.PopsStarvedToDeath = new GeneralNotificationProto<int>.ID(nameof (PopsStarvedToDeath));
        IdsCore.Notifications.HomelessLeft = new GeneralNotificationProto<int>.ID(nameof (HomelessLeft));
        IdsCore.Notifications.CropWillDrySoon = new EntityNotificationProto<Proto>.ID(nameof (CropWillDrySoon));
        IdsCore.Notifications.CropLacksMaintenance = new EntityNotificationProto<Proto>.ID(nameof (CropLacksMaintenance));
        IdsCore.Notifications.CropDiedNoWater = new EntityNotificationProto<Proto>.ID(nameof (CropDiedNoWater));
        IdsCore.Notifications.CropDiedNoFertility = new EntityNotificationProto<Proto>.ID(nameof (CropDiedNoFertility));
        IdsCore.Notifications.CropDiedNoMaintenance = new EntityNotificationProto<Proto>.ID(nameof (CropDiedNoMaintenance));
        IdsCore.Notifications.CropCouldNotBeStored = new EntityNotificationProto<Proto>.ID(nameof (CropCouldNotBeStored));
        IdsCore.Notifications.LowFarmFertility = new EntityNotificationProto.ID(nameof (LowFarmFertility));
        IdsCore.Notifications.NoCropToGrow = new EntityNotificationProto.ID(nameof (NoCropToGrow));
        IdsCore.Notifications.NotEnoughWorkers = new EntityNotificationProto.ID(nameof (NotEnoughWorkers));
        IdsCore.Notifications.EntityIsBoosted = new EntityNotificationProto.ID(nameof (EntityIsBoosted));
        IdsCore.Notifications.VehicleIsBroken = new EntityNotificationProto.ID(nameof (VehicleIsBroken));
        IdsCore.Notifications.MachineIsBroken = new EntityNotificationProto.ID(nameof (MachineIsBroken));
        IdsCore.Notifications.TruckCannotDeliver = new EntityNotificationProto<Proto>.ID(nameof (TruckCannotDeliver));
        IdsCore.Notifications.TruckCannotDeliverMixedCargo = new EntityNotificationProto.ID(nameof (TruckCannotDeliverMixedCargo));
        IdsCore.Notifications.SortingPlantNoProductSet = new EntityNotificationProto.ID(nameof (SortingPlantNoProductSet));
        IdsCore.Notifications.SortingPlantBlockedOutput = new EntityNotificationProto.ID(nameof (SortingPlantBlockedOutput));
        IdsCore.Notifications.VehicleGoalUnreachable = new EntityNotificationProto.ID(nameof (VehicleGoalUnreachable));
        IdsCore.Notifications.VehicleGoalUnreachableCannotGoUnder = new EntityNotificationProto.ID(nameof (VehicleGoalUnreachableCannotGoUnder));
        IdsCore.Notifications.VehicleGoalStruggling = new EntityNotificationProto.ID(nameof (VehicleGoalStruggling));
        IdsCore.Notifications.VehicleGoalStrugglingCannotGoUnder = new EntityNotificationProto.ID(nameof (VehicleGoalStrugglingCannotGoUnder));
        IdsCore.Notifications.VehicleNoFuel = new EntityNotificationProto.ID(nameof (VehicleNoFuel));
        IdsCore.Notifications.EntityCannotBeReached = new EntityNotificationProto.ID(nameof (EntityCannotBeReached));
        IdsCore.Notifications.TruckHasNoValidExcavator = new EntityNotificationProto.ID(nameof (TruckHasNoValidExcavator));
        IdsCore.Notifications.ExcavatorHasNoValidTruck = new EntityNotificationProto.ID(nameof (ExcavatorHasNoValidTruck));
        IdsCore.Notifications.NoTruckAssignedToTreeHarvester = new EntityNotificationProto.ID(nameof (NoTruckAssignedToTreeHarvester));
        IdsCore.Notifications.NoTreeSaplingsForPlanter = new EntityNotificationProto.ID(nameof (NoTreeSaplingsForPlanter));
        IdsCore.Notifications.NoTreesToHarvest = new EntityNotificationProto.ID(nameof (NoTreesToHarvest));
        IdsCore.Notifications.NotEnoughElectricity = new GeneralNotificationProto.ID("NotEnoughPower");
        IdsCore.Notifications.NotEnoughElectricityForEntity = new EntityNotificationProto.ID("NotEnoughPowerForEntity");
        IdsCore.Notifications.NotEnoughComputingForEntity = new EntityNotificationProto.ID(nameof (NotEnoughComputingForEntity));
        IdsCore.Notifications.NoResourceToExtract = new EntityNotificationProto.ID(nameof (NoResourceToExtract));
        IdsCore.Notifications.ResourceIsLow = new EntityNotificationProto<Proto>.ID(nameof (ResourceIsLow));
        IdsCore.Notifications.LowGroundwater = new GeneralNotificationProto.ID(nameof (LowGroundwater));
        IdsCore.Notifications.NotEnoughFuelToRefuel = new GeneralNotificationProto.ID(nameof (NotEnoughFuelToRefuel));
        IdsCore.Notifications.FuelStationOutOfFuel = new EntityNotificationProto.ID(nameof (FuelStationOutOfFuel));
        IdsCore.Notifications.FuelStationNotConnected = new EntityNotificationProto.ID(nameof (FuelStationNotConnected));
        IdsCore.Notifications.NoMineDesignInTowerArea = new EntityNotificationProto.ID(nameof (NoMineDesignInTowerArea));
        IdsCore.Notifications.NoAvailableMineDesignInTowerArea = new EntityNotificationProto.ID(nameof (NoAvailableMineDesignInTowerArea));
        IdsCore.Notifications.NoForestryDesignInTowerArea = new EntityNotificationProto.ID(nameof (NoForestryDesignInTowerArea));
        IdsCore.Notifications.NoAvailableForestryDesignInTowerArea = new EntityNotificationProto.ID(nameof (NoAvailableForestryDesignInTowerArea));
        IdsCore.Notifications.CannotDeliverFromMineTower = new EntityNotificationProto.ID(nameof (CannotDeliverFromMineTower));
        IdsCore.Notifications.AreasWithoutTowers = new GeneralNotificationProto.ID(nameof (AreasWithoutTowers));
        IdsCore.Notifications.AreasWithoutForestryTowers = new GeneralNotificationProto.ID(nameof (AreasWithoutForestryTowers));
        IdsCore.Notifications.VehicleNoReachableDesignations = new EntityNotificationProto.ID(nameof (VehicleNoReachableDesignations));
        IdsCore.Notifications.NoRecipeSelected = new EntityNotificationProto.ID(nameof (NoRecipeSelected));
        IdsCore.Notifications.NeedsTransportConnected = new EntityNotificationProto<Proto>.ID(nameof (NeedsTransportConnected));
        IdsCore.Notifications.TransportTooLong = new EntityNotificationProto.ID(nameof (TransportTooLong));
        IdsCore.Notifications.ShipCargoLoaded = new GeneralNotificationProto<EntityProto>.ID(nameof (ShipCargoLoaded));
        IdsCore.Notifications.ShipCargoDelivered = new GeneralNotificationProto<EntityProto>.ID(nameof (ShipCargoDelivered));
        IdsCore.Notifications.ShipRepaired = new GeneralNotificationProto.ID(nameof (ShipRepaired));
        IdsCore.Notifications.ShipModified = new GeneralNotificationProto.ID(nameof (ShipModified));
        IdsCore.Notifications.OceanAccessBlocked = new EntityNotificationProto.ID(nameof (OceanAccessBlocked));
        IdsCore.Notifications.WorldEntityRepaired = new GeneralNotificationProto<WorldMapMineProto>.ID(nameof (WorldEntityRepaired));
        IdsCore.Notifications.CargoShipMissingFuel = new EntityNotificationProto.ID(nameof (CargoShipMissingFuel));
        IdsCore.Notifications.CargoShipContractLacksUpoints = new EntityNotificationProto.ID(nameof (CargoShipContractLacksUpoints));
        IdsCore.Notifications.CargoDepotHasNoShip = new EntityNotificationProto.ID(nameof (CargoDepotHasNoShip));
        IdsCore.Notifications.CargoDepotHasNoModule = new EntityNotificationProto.ID(nameof (CargoDepotHasNoModule));
        IdsCore.Notifications.NotEnoughUpoints = new GeneralNotificationProto.ID(nameof (NotEnoughUpoints));
        IdsCore.Notifications.NotEnoughUpointsForEntity = new EntityNotificationProto.ID(nameof (NotEnoughUpointsForEntity));
        IdsCore.Notifications.LabCannotResearchHigherTech = new EntityNotificationProto.ID(nameof (LabCannotResearchHigherTech));
        IdsCore.Notifications.LabMissingInputProducts = new EntityNotificationProto.ID(nameof (LabMissingInputProducts));
        IdsCore.Notifications.SettlementHasNoFoodModule = new EntityNotificationProto.ID(nameof (SettlementHasNoFoodModule));
        IdsCore.Notifications.SettlementIsStarving = new EntityNotificationProto.ID(nameof (SettlementIsStarving));
        IdsCore.Notifications.SettlementFullOfLandfill = new EntityNotificationProto.ID(nameof (SettlementFullOfLandfill));
        IdsCore.Notifications.NoProductAssignedToEntity = new EntityNotificationProto.ID(nameof (NoProductAssignedToEntity));
        IdsCore.Notifications.NotEnoughMaintenance = new GeneralNotificationProto<Proto>.ID(nameof (NotEnoughMaintenance));
        IdsCore.Notifications.CargoDepotModuleNoProductAssigned = new EntityNotificationProto.ID(nameof (CargoDepotModuleNoProductAssigned));
        IdsCore.Notifications.CargoDepotModuleContractNotMatching = new EntityNotificationProto.ID(nameof (CargoDepotModuleContractNotMatching));
        IdsCore.Notifications.NewErrorOccurred = new GeneralNotificationProto<string>.ID(nameof (NewErrorOccurred));
        IdsCore.Notifications.NuclearReactorInMeltdown = new EntityNotificationProto.ID(nameof (NuclearReactorInMeltdown));
        IdsCore.Notifications.NuclearReactorLacksMaintenance = new EntityNotificationProto.ID(nameof (NuclearReactorLacksMaintenance));
        IdsCore.Notifications.StorageSupplyTooLow = new EntityNotificationProto<Proto>.ID(nameof (StorageSupplyTooLow));
        IdsCore.Notifications.StorageSupplyTooHigh = new EntityNotificationProto<Proto>.ID(nameof (StorageSupplyTooHigh));
        IdsCore.Notifications.EntityMayCollapseUnevenTerrain = new EntityNotificationProto.ID(nameof (EntityMayCollapseUnevenTerrain));
        IdsCore.Notifications.AnimalFarmMissingFood = new EntityNotificationProto.ID(nameof (AnimalFarmMissingFood));
        IdsCore.Notifications.AnimalFarmMissingWater = new EntityNotificationProto.ID(nameof (AnimalFarmMissingWater));
        IdsCore.Notifications.InvalidImportRoute = new EntityNotificationProto.ID(nameof (InvalidImportRoute));
        IdsCore.Notifications.InvalidExportRoute = new EntityNotificationProto.ID(nameof (InvalidExportRoute));
        IdsCore.Notifications.LoanPaymentDelayed = new GeneralNotificationProto<Proto>.ID(nameof (LoanPaymentDelayed));
        IdsCore.Notifications.LoanPaymentFailed = new GeneralNotificationProto<Proto>.ID(nameof (LoanPaymentFailed));
      }
    }

    public static class PropertyIds
    {
      public static readonly PropertyId<Percent> VehiclesFuelConsumptionMultiplier;
      public static readonly PropertyId<Percent> ShipsFuelConsumptionMultiplier;
      public static readonly PropertyId<Percent> TrucksCapacityMultiplier;
      public static readonly PropertyId<Percent> MaintenanceConsumptionMultiplier;
      public static readonly PropertyId<Percent> TrucksMaintenanceMultiplier;
      public static readonly PropertyId<bool> FuelConsumptionDisabled;
      public static readonly PropertyId<Percent> UnityProductionMultiplier;
      public static readonly PropertyId<Percent> SettlementConsumptionMultiplier;
      public static readonly PropertyId<Percent> MiningMultiplier;
      public static readonly PropertyId<Percent> ResearchStepsMultiplier;
      public static readonly PropertyId<Percent> DeconstructionRefundMultiplier;
      public static readonly PropertyId<Percent> ConstructionCostsMultiplier;
      public static readonly PropertyId<Percent> QuickActionsUnityCostMultiplier;
      public static readonly PropertyId<Percent> FoodConsumptionMultiplier;
      /// <summary>
      /// Instead of removing pops on starvation they will just be withheld from the workforce.
      /// </summary>
      public static readonly PropertyId<bool> CanWithholdWorkersOnStarvation;
      public static readonly PropertyId<Percent> BaseHealthMultiplier;
      public static readonly PropertyId<Percent> BaseHealthDiffEdicts;
      public static readonly PropertyId<Percent> TradeVolumeMultiplier;
      public static readonly PropertyId<bool> ForceRunAllMachinesEnabled;
      public static readonly PropertyId<Percent> FarmWaterConsumptionMultiplier;
      public static readonly PropertyId<Percent> FarmYieldMultiplier;
      public static readonly PropertyId<Percent> TreesGrowthSpeed;
      public static readonly PropertyId<Percent> RecyclingRatioDiff;
      public static readonly PropertyId<Percent> SolarPowerMultiplier;
      [OnlyForSaveCompatibility(null)]
      public static readonly PropertyId<Percent> DiseaseEffectsMultiplier;
      public static readonly PropertyId<bool> LogisticsCanWorkOnLowPower;
      public static readonly PropertyId<bool> LogisticsIgnorePower;
      public static readonly PropertyId<bool> SlowDownIfBroken;
      public static readonly PropertyId<Percent> MachineSpeedOnLowPower;
      public static readonly PropertyId<Percent> MachineSpeedOnLowComputing;
      public static readonly PropertyId<Percent> RainYieldMultiplier;
      public static readonly PropertyId<Percent> GroundWaterPumpSpeedWhenDepleted;
      public static readonly PropertyId<Percent> GroundWaterReplenishWhenLow;
      public static readonly PropertyId<bool> ShipsCanUseUnityIfOutOfFuel;
      public static readonly PropertyId<bool> VehicleSlowDownOnLowFuel;
      public static readonly PropertyId<bool> WorldMinesCanRunWithoutUnity;
      public static readonly PropertyId<bool> UnlimitedWorldMines;
      public static readonly PropertyId<Percent> WorldMinesReserveMultiplier;
      public static readonly PropertyId<Percent> ContractsProfitMultiplier;
      public static readonly PropertyId<Percent> DiseaseMortalityMultiplier;
      public static readonly PropertyId<Percent> WaterPollutionMultiplier;
      public static readonly PropertyId<Percent> AirPollutionMultiplier;
      public static readonly PropertyId<Percent> LandfillPollutionMultiplier;
      public static readonly PropertyId<bool> OreSortingEnabled;

      static PropertyIds()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.PropertyIds.VehiclesFuelConsumptionMultiplier = new PropertyId<Percent>(nameof (VehiclesFuelConsumptionMultiplier));
        IdsCore.PropertyIds.ShipsFuelConsumptionMultiplier = new PropertyId<Percent>(nameof (ShipsFuelConsumptionMultiplier));
        IdsCore.PropertyIds.TrucksCapacityMultiplier = new PropertyId<Percent>(nameof (TrucksCapacityMultiplier));
        IdsCore.PropertyIds.MaintenanceConsumptionMultiplier = new PropertyId<Percent>(nameof (MaintenanceConsumptionMultiplier));
        IdsCore.PropertyIds.TrucksMaintenanceMultiplier = new PropertyId<Percent>(nameof (TrucksMaintenanceMultiplier));
        IdsCore.PropertyIds.FuelConsumptionDisabled = new PropertyId<bool>(nameof (FuelConsumptionDisabled));
        IdsCore.PropertyIds.UnityProductionMultiplier = new PropertyId<Percent>(nameof (UnityProductionMultiplier));
        IdsCore.PropertyIds.SettlementConsumptionMultiplier = new PropertyId<Percent>(nameof (SettlementConsumptionMultiplier));
        IdsCore.PropertyIds.MiningMultiplier = new PropertyId<Percent>(nameof (MiningMultiplier));
        IdsCore.PropertyIds.ResearchStepsMultiplier = new PropertyId<Percent>(nameof (ResearchStepsMultiplier));
        IdsCore.PropertyIds.DeconstructionRefundMultiplier = new PropertyId<Percent>(nameof (DeconstructionRefundMultiplier));
        IdsCore.PropertyIds.ConstructionCostsMultiplier = new PropertyId<Percent>(nameof (ConstructionCostsMultiplier));
        IdsCore.PropertyIds.QuickActionsUnityCostMultiplier = new PropertyId<Percent>(nameof (QuickActionsUnityCostMultiplier));
        IdsCore.PropertyIds.FoodConsumptionMultiplier = new PropertyId<Percent>(nameof (FoodConsumptionMultiplier));
        IdsCore.PropertyIds.CanWithholdWorkersOnStarvation = new PropertyId<bool>(nameof (CanWithholdWorkersOnStarvation));
        IdsCore.PropertyIds.BaseHealthMultiplier = new PropertyId<Percent>(nameof (BaseHealthMultiplier));
        IdsCore.PropertyIds.BaseHealthDiffEdicts = new PropertyId<Percent>(nameof (BaseHealthDiffEdicts));
        IdsCore.PropertyIds.TradeVolumeMultiplier = new PropertyId<Percent>(nameof (TradeVolumeMultiplier));
        IdsCore.PropertyIds.ForceRunAllMachinesEnabled = new PropertyId<bool>(nameof (ForceRunAllMachinesEnabled));
        IdsCore.PropertyIds.FarmWaterConsumptionMultiplier = new PropertyId<Percent>(nameof (FarmWaterConsumptionMultiplier));
        IdsCore.PropertyIds.FarmYieldMultiplier = new PropertyId<Percent>(nameof (FarmYieldMultiplier));
        IdsCore.PropertyIds.TreesGrowthSpeed = new PropertyId<Percent>(nameof (TreesGrowthSpeed));
        IdsCore.PropertyIds.RecyclingRatioDiff = new PropertyId<Percent>(nameof (RecyclingRatioDiff));
        IdsCore.PropertyIds.SolarPowerMultiplier = new PropertyId<Percent>(nameof (SolarPowerMultiplier));
        IdsCore.PropertyIds.DiseaseEffectsMultiplier = new PropertyId<Percent>(nameof (DiseaseEffectsMultiplier));
        IdsCore.PropertyIds.LogisticsCanWorkOnLowPower = new PropertyId<bool>(nameof (LogisticsCanWorkOnLowPower));
        IdsCore.PropertyIds.LogisticsIgnorePower = new PropertyId<bool>(nameof (LogisticsIgnorePower));
        IdsCore.PropertyIds.SlowDownIfBroken = new PropertyId<bool>(nameof (SlowDownIfBroken));
        IdsCore.PropertyIds.MachineSpeedOnLowPower = new PropertyId<Percent>(nameof (MachineSpeedOnLowPower));
        IdsCore.PropertyIds.MachineSpeedOnLowComputing = new PropertyId<Percent>(nameof (MachineSpeedOnLowComputing));
        IdsCore.PropertyIds.RainYieldMultiplier = new PropertyId<Percent>(nameof (RainYieldMultiplier));
        IdsCore.PropertyIds.GroundWaterPumpSpeedWhenDepleted = new PropertyId<Percent>(nameof (GroundWaterPumpSpeedWhenDepleted));
        IdsCore.PropertyIds.GroundWaterReplenishWhenLow = new PropertyId<Percent>(nameof (GroundWaterReplenishWhenLow));
        IdsCore.PropertyIds.ShipsCanUseUnityIfOutOfFuel = new PropertyId<bool>(nameof (ShipsCanUseUnityIfOutOfFuel));
        IdsCore.PropertyIds.VehicleSlowDownOnLowFuel = new PropertyId<bool>(nameof (VehicleSlowDownOnLowFuel));
        IdsCore.PropertyIds.WorldMinesCanRunWithoutUnity = new PropertyId<bool>(nameof (WorldMinesCanRunWithoutUnity));
        IdsCore.PropertyIds.UnlimitedWorldMines = new PropertyId<bool>(nameof (UnlimitedWorldMines));
        IdsCore.PropertyIds.WorldMinesReserveMultiplier = new PropertyId<Percent>(nameof (WorldMinesReserveMultiplier));
        IdsCore.PropertyIds.ContractsProfitMultiplier = new PropertyId<Percent>(nameof (ContractsProfitMultiplier));
        IdsCore.PropertyIds.DiseaseMortalityMultiplier = new PropertyId<Percent>(nameof (DiseaseMortalityMultiplier));
        IdsCore.PropertyIds.WaterPollutionMultiplier = new PropertyId<Percent>(nameof (WaterPollutionMultiplier));
        IdsCore.PropertyIds.AirPollutionMultiplier = new PropertyId<Percent>(nameof (AirPollutionMultiplier));
        IdsCore.PropertyIds.LandfillPollutionMultiplier = new PropertyId<Percent>(nameof (LandfillPollutionMultiplier));
        IdsCore.PropertyIds.OreSortingEnabled = new PropertyId<bool>(nameof (OreSortingEnabled));
      }
    }

    public static class UpointsStatsCategories
    {
      public static readonly Proto.ID IslandBuilding;
      public static readonly Proto.ID OneTimeAction;
      public static readonly Proto.ID Ignore;

      static UpointsStatsCategories()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.UpointsStatsCategories.IslandBuilding = new Proto.ID("UpointsCat_IslandBuildings");
        IdsCore.UpointsStatsCategories.OneTimeAction = new Proto.ID("UpointsCat_OneTimeActions");
        IdsCore.UpointsStatsCategories.Ignore = new Proto.ID("UpointsCat_Ignore");
      }
    }

    public static class UpointsCategories
    {
      public static readonly Proto.ID Edict;
      public static readonly Proto.ID Boost;
      public static readonly Proto.ID Health;
      public static readonly Proto.ID Starvation;
      public static readonly Proto.ID Homeless;
      public static readonly Proto.ID SettlementQuality;
      public static readonly Proto.ID Rockets;
      public static readonly Proto.ID Contract;
      public static readonly Proto.ID FreeUnity;
      public static readonly Proto.ID PopsAdoption;
      public static readonly Proto.ID QuickTrade;
      public static readonly Proto.ID QuickBuild;
      public static readonly Proto.ID QuickRemove;
      public static readonly Proto.ID QuickRepair;
      public static readonly Proto.ID ContractEstablish;
      public static readonly Proto.ID VehicleRecovery;
      public static readonly Proto.ID OtherDecorations;
      public static readonly Proto.ID ShipFuel;

      static UpointsCategories()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.UpointsCategories.Edict = new Proto.ID("UpointsCat_Edict");
        IdsCore.UpointsCategories.Boost = new Proto.ID("UpointsCat_Boost");
        IdsCore.UpointsCategories.Health = new Proto.ID("UpointsCat_Health");
        IdsCore.UpointsCategories.Starvation = new Proto.ID("UpointsCat_Starvation");
        IdsCore.UpointsCategories.Homeless = new Proto.ID("UpointsCat_Homeless");
        IdsCore.UpointsCategories.SettlementQuality = new Proto.ID("UpointsCat_Decorations");
        IdsCore.UpointsCategories.Rockets = new Proto.ID("UpointsCat_Rockets");
        IdsCore.UpointsCategories.Contract = new Proto.ID("UpointsCat_Contract");
        IdsCore.UpointsCategories.FreeUnity = new Proto.ID("UpointsCat_FreeUnity");
        IdsCore.UpointsCategories.PopsAdoption = new Proto.ID("UpointsCat_PopsAdoption");
        IdsCore.UpointsCategories.QuickTrade = new Proto.ID("UpointsCat_QuickTrade");
        IdsCore.UpointsCategories.QuickBuild = new Proto.ID("UpointsCat_QuickBuild");
        IdsCore.UpointsCategories.QuickRemove = new Proto.ID("UpointsCat_QuickRemove");
        IdsCore.UpointsCategories.QuickRepair = new Proto.ID("UpointsCat_QuickRepair");
        IdsCore.UpointsCategories.ContractEstablish = new Proto.ID("UpointsCat_ContractEstablish");
        IdsCore.UpointsCategories.VehicleRecovery = new Proto.ID("UpointsCat_VehicleRecovery");
        IdsCore.UpointsCategories.OtherDecorations = new Proto.ID("UpointsCat_OtherDecorations");
        IdsCore.UpointsCategories.ShipFuel = new Proto.ID("UpointsCat_ShipFuel");
      }
    }

    public static class HealthPointsCategories
    {
      public static readonly Proto.ID Base;
      public static readonly Proto.ID Edicts;
      public static readonly Proto.ID LandfillPollution;
      public static readonly Proto.ID WaterPollution;
      public static readonly Proto.ID AirPollution;
      public static readonly Proto.ID AirPollutionVehicles;
      public static readonly Proto.ID AirPollutionShips;
      public static readonly Proto.ID Food;
      public static readonly Proto.ID Healthcare;
      public static readonly Proto.ID WasteInSettlement;
      public static readonly Proto.ID Disease;

      static HealthPointsCategories()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.HealthPointsCategories.Base = new Proto.ID("HealthPointsCat_Base");
        IdsCore.HealthPointsCategories.Edicts = new Proto.ID("HealthPointsCat_Edicts");
        IdsCore.HealthPointsCategories.LandfillPollution = new Proto.ID("HealthPointsCat_LandfillPollution");
        IdsCore.HealthPointsCategories.WaterPollution = new Proto.ID("HealthPointsCat_WaterPollution");
        IdsCore.HealthPointsCategories.AirPollution = new Proto.ID("HealthPointsCat_AirPollution");
        IdsCore.HealthPointsCategories.AirPollutionVehicles = new Proto.ID("HealthPointsCat_AirPollutionVehicles");
        IdsCore.HealthPointsCategories.AirPollutionShips = new Proto.ID("HealthPointsCat_AirPollutionShips");
        IdsCore.HealthPointsCategories.Food = new Proto.ID("HealthPointsCat_Food");
        IdsCore.HealthPointsCategories.Healthcare = new Proto.ID("HealthPointsCat_Healthcare");
        IdsCore.HealthPointsCategories.WasteInSettlement = new Proto.ID("HealthPointsCat_WasteInSettlement");
        IdsCore.HealthPointsCategories.Disease = new Proto.ID("HealthPointsCat_Disease");
      }
    }

    public static class BirthRateCategories
    {
      public static readonly Proto.ID Base;
      public static readonly Proto.ID Starvation;
      public static readonly Proto.ID Radiation;
      public static readonly Proto.ID Disease;
      public static readonly Proto.ID Edicts;
      public static readonly Proto.ID Health;

      static BirthRateCategories()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.BirthRateCategories.Base = new Proto.ID("BirthRateCategoryCat_Base");
        IdsCore.BirthRateCategories.Starvation = new Proto.ID("BirthRateCategoryCat_Starvation");
        IdsCore.BirthRateCategories.Radiation = new Proto.ID("BirthRateCategoryCat_Radiation");
        IdsCore.BirthRateCategories.Disease = new Proto.ID("BirthRateCategoryCat_Disease");
        IdsCore.BirthRateCategories.Edicts = new Proto.ID("BirthRateCategoryCat_Edicts");
        IdsCore.BirthRateCategories.Health = new Proto.ID("BirthRateCategoryCat_Health");
      }
    }

    public static class PopNeeds
    {
      public static readonly Proto.ID Food;
      public static readonly Proto.ID PowerNeed;
      public static readonly Proto.ID WaterNeed;
      public static readonly Proto.ID HouseholdGoodsNeed;
      public static readonly Proto.ID HouseholdAppliancesNeed;
      public static readonly Proto.ID ConsumerElectronicsNeed;
      public static readonly Proto.ID HealthCareNeed;

      static PopNeeds()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.PopNeeds.Food = new Proto.ID("FoodNeed");
        IdsCore.PopNeeds.PowerNeed = new Proto.ID(nameof (PowerNeed));
        IdsCore.PopNeeds.WaterNeed = new Proto.ID(nameof (WaterNeed));
        IdsCore.PopNeeds.HouseholdGoodsNeed = new Proto.ID(nameof (HouseholdGoodsNeed));
        IdsCore.PopNeeds.HouseholdAppliancesNeed = new Proto.ID(nameof (HouseholdAppliancesNeed));
        IdsCore.PopNeeds.ConsumerElectronicsNeed = new Proto.ID(nameof (ConsumerElectronicsNeed));
        IdsCore.PopNeeds.HealthCareNeed = new Proto.ID(nameof (HealthCareNeed));
      }
    }

    public static class World
    {
      public static readonly EntityProto.ID Fleet;

      static World()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.World.Fleet = new EntityProto.ID(nameof (Fleet));
      }
    }

    public static class Transports
    {
      public const string SHAFT_PORT_SHAPE = "IoPortShape_Shaft";
      public static readonly IoPortShapeProto.ID ShaftPortShape;
      public static readonly StaticEntityProto.ID Pillar;

      public static StaticEntityProto.ID GetZipperIdFor(IoPortShapeProto.ID shapeId)
      {
        return new StaticEntityProto.ID("Zipper_" + shapeId.Value);
      }

      public static StaticEntityProto.ID GetMiniZipperIdFor(IoPortShapeProto.ID shapeId)
      {
        return new StaticEntityProto.ID("MiniZip_" + shapeId.Value);
      }

      [OnlyForSaveCompatibility(null)]
      public static StaticEntityProto.ID GetSorterIdFor(IoPortShapeProto.ID shapeId)
      {
        if (shapeId.Value == "IoPortShape_FlatConveyor")
          return new StaticEntityProto.ID("FlatConveyorSorter");
        return shapeId.Value == "IoPortShape_LooseMaterialConveyor" ? new StaticEntityProto.ID("LooseConveyorSorter") : new StaticEntityProto.ID("Sorter_" + shapeId.Value);
      }

      public static StaticEntityProto.ID GetLiftIdFor(IoPortShapeProto.ID shapeId, int height)
      {
        return new StaticEntityProto.ID("Lift" + shapeId.Value + "_" + (height < 0 ? (object) string.Format("_{0}", (object) height.Abs()) : (object) height)?.ToString());
      }

      static Transports()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.Transports.ShaftPortShape = new IoPortShapeProto.ID("IoPortShape_Shaft");
        IdsCore.Transports.Pillar = new StaticEntityProto.ID("TransportsPillar");
      }
    }

    public static class Messages
    {
      public static readonly Proto.ID TutorialOnFarming;
      public static readonly Proto.ID TutorialOnFarmFertility;
      public static readonly Proto.ID TutorialOnTreesPlanting;
      public static readonly Proto.ID TutorialOnTransports;
      public static readonly Proto.ID TutorialTreeHarvesting;
      public static readonly Proto.ID TutorialOnMineTower;
      public static readonly Proto.ID TutorialOnRetainingWalls;
      public static readonly Proto.ID TutorialOnDumping;
      public static readonly Proto.ID TutorialOnVehiclesAccessibility;
      public static readonly Proto.ID TutorialOnCargoShip;
      public static readonly Proto.ID TutorialOnAdvancedLogistics;
      public static readonly Proto.ID TutorialOnMaintenance;
      public static readonly Proto.ID TutorialOnPopsAndUnity;
      public static readonly Proto.ID TutorialOnCoalPower;
      public static readonly Proto.ID TutorialOnWorldEntities;
      public static readonly Proto.ID TutorialOnContracts;
      public static readonly Proto.ID PlanningModeTutorial;
      public static readonly Proto.ID TutorialOnCopyTool;
      public static readonly Proto.ID TutorialOnCutTool;
      public static readonly Proto.ID TutorialOnPauseTool;
      public static readonly Proto.ID TutorialOnCloneTool;
      public static readonly Proto.ID TutorialOnUnityTool;
      public static readonly Proto.ID TutorialOnHealth;

      static Messages()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.Messages.TutorialOnFarming = new Proto.ID(nameof (TutorialOnFarming));
        IdsCore.Messages.TutorialOnFarmFertility = new Proto.ID(nameof (TutorialOnFarmFertility));
        IdsCore.Messages.TutorialOnTreesPlanting = new Proto.ID(nameof (TutorialOnTreesPlanting));
        IdsCore.Messages.TutorialOnTransports = new Proto.ID(nameof (TutorialOnTransports));
        IdsCore.Messages.TutorialTreeHarvesting = new Proto.ID(nameof (TutorialTreeHarvesting));
        IdsCore.Messages.TutorialOnMineTower = new Proto.ID(nameof (TutorialOnMineTower));
        IdsCore.Messages.TutorialOnRetainingWalls = new Proto.ID(nameof (TutorialOnRetainingWalls));
        IdsCore.Messages.TutorialOnDumping = new Proto.ID(nameof (TutorialOnDumping));
        IdsCore.Messages.TutorialOnVehiclesAccessibility = new Proto.ID(nameof (TutorialOnVehiclesAccessibility));
        IdsCore.Messages.TutorialOnCargoShip = new Proto.ID(nameof (TutorialOnCargoShip));
        IdsCore.Messages.TutorialOnAdvancedLogistics = new Proto.ID(nameof (TutorialOnAdvancedLogistics));
        IdsCore.Messages.TutorialOnMaintenance = new Proto.ID(nameof (TutorialOnMaintenance));
        IdsCore.Messages.TutorialOnPopsAndUnity = new Proto.ID(nameof (TutorialOnPopsAndUnity));
        IdsCore.Messages.TutorialOnCoalPower = new Proto.ID(nameof (TutorialOnCoalPower));
        IdsCore.Messages.TutorialOnWorldEntities = new Proto.ID(nameof (TutorialOnWorldEntities));
        IdsCore.Messages.TutorialOnContracts = new Proto.ID(nameof (TutorialOnContracts));
        IdsCore.Messages.PlanningModeTutorial = new Proto.ID(nameof (PlanningModeTutorial));
        IdsCore.Messages.TutorialOnCopyTool = new Proto.ID(nameof (TutorialOnCopyTool));
        IdsCore.Messages.TutorialOnCutTool = new Proto.ID(nameof (TutorialOnCutTool));
        IdsCore.Messages.TutorialOnPauseTool = new Proto.ID(nameof (TutorialOnPauseTool));
        IdsCore.Messages.TutorialOnCloneTool = new Proto.ID(nameof (TutorialOnCloneTool));
        IdsCore.Messages.TutorialOnUnityTool = new Proto.ID(nameof (TutorialOnUnityTool));
        IdsCore.Messages.TutorialOnHealth = new Proto.ID(nameof (TutorialOnHealth));
      }
    }

    public static class Weather
    {
      public static readonly Proto.ID Sunny;

      public static Proto.ID Create(string name) => new Proto.ID(name + nameof (Weather));

      static Weather()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.Weather.Sunny = IdsCore.Weather.Create(nameof (Sunny));
      }
    }

    public static class ToolbarCategories
    {
      public static readonly Proto.ID Surfaces;

      static ToolbarCategories()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        IdsCore.ToolbarCategories.Surfaces = new Proto.ID("surfaceCategory");
      }
    }
  }
}
