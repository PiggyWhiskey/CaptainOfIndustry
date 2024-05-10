// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Research.ConstructionResearchData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Entities.Static;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Research
{
  internal class ConstructionResearchData : IResearchNodesData, IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.ResearchNodeProtoBuilder.Start("Construction", Ids.Research.CpPacking).Description("Production of basic construction parts.", "description of a research node in the research tree").AddMachineToUnlock(Ids.Machines.AssemblyManual).AddRecipeToUnlock(Ids.Recipes.CpBricksAssemblyT1).AddRecipeToUnlock(Ids.Recipes.MechPartsAssemblyT1).AddRecipeToUnlock(Ids.Recipes.ElectronicsAssemblyT1).AddProductIcon(Ids.Products.ConstructionParts).AddProductIcon(Ids.Products.MechanicalParts).AddProductIcon(Ids.Products.Electronics).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Custom surfaces", Ids.Research.CustomSurfaces).UseDescriptionFrom(IdsCore.Technology.CustomSurfaces).AddTechnologyToUnlock(IdsCore.Technology.CustomSurfaces).AddSurfaceToUnlock(IdsCore.TerrainTileSurfaces.DefaultConcrete).AddSurfaceToUnlock(Ids.TerrainTileSurfaces.Bricks).AddSurfaceToUnlock(Ids.TerrainTileSurfaces.Cobblestone).AddSurfaceToUnlock(Ids.TerrainTileSurfaces.Sand1).AddSurfaceToUnlock(Ids.TerrainTileSurfaces.Sand2).AddSurfaceToUnlock(Ids.TerrainTileSurfaces.Metal2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Ship dock repair", Ids.Research.RepairDock).Description("Enables to repair the ship dock so we can start working on ship repairs.", "description of a research node in the research tree").AddLayoutEntityToUnlock(Ids.Buildings.Shipyard2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Maintenance Depot II", Ids.Research.MaintenanceDepot).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Buildings.MaintenanceDepotT1).AddRecipeToUnlock(Ids.Recipes.MaintenanceT1).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Recycling", Ids.Research.Recycling).UseDescriptionFrom((Proto.ID) Ids.Buildings.WasteSortingPlant).AddTechnologyToUnlock(IdsCore.Technology.Recycling).AddLayoutEntityToUnlock(Ids.Buildings.WasteSortingPlant).AddLayoutEntityToUnlock(Ids.Buildings.SettlementBiomassModule).AddRecipeToUnlock(Ids.Recipes.MaintenanceT1Recycling).AddRecipeToUnlock(Ids.Recipes.BiomassBurning).AddRecipeToUnlock(Ids.Recipes.SteamGenerationBiomass).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Compactor", Ids.Research.Compactor).UseDescriptionFrom((Proto.ID) Ids.Machines.Compactor).AddMachineToUnlock(Ids.Machines.Compactor).AddMachineToUnlock(Ids.Machines.Shredder).AddRecipeToUnlock(Ids.Recipes.PressingOfIronScrap).AddRecipeToUnlock(Ids.Recipes.PressingOfCopperScrap).AddRecipeToUnlock(Ids.Recipes.PressingOfRecyclables).AddRecipeToUnlock(Ids.Recipes.PressingOfWaste).AddRecipeToUnlock(Ids.Recipes.ShreddingIronScrap).AddRecipeToUnlock(Ids.Recipes.ShreddingCopperScrap).AddRecipeToUnlock(Ids.Recipes.ShreddingWaste).AddRecipeToUnlock(Ids.Recipes.ShreddingSaplings).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Settlement recycling", Ids.Research.RecyclingForSettlement).UseDescriptionFrom((Proto.ID) Ids.Buildings.SettlementRecyclablesModule).AddLayoutEntityToUnlock(Ids.Buildings.SettlementRecyclablesModule).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Bricks production", Ids.Research.BricksProduction).AddProductIcon(Ids.Products.Bricks).AddMachineToUnlock(Ids.Machines.BricksMaker, true).AddLayoutEntityToUnlock(Ids.Buildings.RainwaterHarvester).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Rotary kiln (gas)", Ids.Research.RotaryKilnGas).AddMachineToUnlock(Ids.Machines.RotaryKilnGas, true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Retaining walls", Ids.Research.RetainingWalls).UseDescriptionFrom((Proto.ID) Ids.Buildings.RetainingWallStraight1).AddLayoutEntityToUnlock(Ids.Buildings.RetainingWallStraight1).AddLayoutEntityToUnlock(Ids.Buildings.RetainingWallStraight4).AddLayoutEntityToUnlock(Ids.Buildings.RetainingWallCorner).AddLayoutEntityToUnlock(Ids.Buildings.RetainingWallTee).AddLayoutEntityToUnlock(Ids.Buildings.RetainingWallCross).BuildAndAdd();
      LocStr desc = Loc.Str(Ids.Research.Cp2Packing.ToString() + "__desc", "Production of advanced construction parts.", "description of a research that provides advanced construction parts");
      registrator.ResearchNodeProtoBuilder.Start("Construction II", Ids.Research.Cp2Packing).Description(desc).AddMachineToUnlock(Ids.Machines.AssemblyElectrified).AddRecipeToUnlock(Ids.Recipes.Cp2AssemblyT1).AddRecipeToUnlock(Ids.Recipes.CpBricksAssemblyT2).AddRecipeToUnlock(Ids.Recipes.Cp2AssemblyT2).AddRecipeToUnlock(Ids.Recipes.MechPartsAssemblyT2).AddRecipeToUnlock(Ids.Recipes.ElectronicsAssemblyT2).AddRecipeToUnlock(Ids.Recipes.VehicleParts1AssemblyT2).AddProductIcon(Ids.Products.ConstructionParts2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Settlement water", Ids.Research.SettlementWater).UseDescriptionFrom((Proto.ID) Ids.Buildings.SettlementWaterModule).AddLayoutEntityToUnlock(Ids.Buildings.SettlementWaterModule).AddProtoUnlockNoIcon(IdsCore.PopNeeds.WaterNeed).AddProductIcon(Ids.Products.Water).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Food market II", Ids.Research.FoodMarket2).UseDescriptionFrom((Proto.ID) Ids.Buildings.SettlementFoodModuleT2).AddLayoutEntityToUnlock(Ids.Buildings.SettlementFoodModuleT2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Settlement power", Ids.Research.SettlementPower).AddLayoutEntityToUnlock(Ids.Buildings.SettlementPowerModule).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Housing II", Ids.Research.Housing2).AddLayoutEntityToUnlock(Ids.Buildings.HousingT2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Construction III", Ids.Research.Cp3Packing).Description(desc).AddMachineToUnlock(Ids.Machines.AssemblyElectrifiedT2).AddMachineToUnlock(Ids.Machines.ConcreteMixerT2, true).AddProductIcon(Ids.Products.ConstructionParts3).AddRecipeToUnlock(Ids.Recipes.Cp3AssemblyT1).AddRecipeToUnlock(Ids.Recipes.CpBricksAssemblyT3).AddRecipeToUnlock(Ids.Recipes.CpAssemblyT3).AddRecipeToUnlock(Ids.Recipes.Cp2AssemblyT3).AddRecipeToUnlock(Ids.Recipes.Cp3AssemblyT2).AddRecipeToUnlock(Ids.Recipes.MechPartsAssemblyT3Iron).AddRecipeToUnlock(Ids.Recipes.MechPartsAssemblyT3).AddRecipeToUnlock(Ids.Recipes.ElectronicsAssemblyT3).AddRecipeToUnlock(Ids.Recipes.VehicleParts1AssemblyT3).AddRecipeToUnlock(Ids.Recipes.LabEquipment1AssemblyT2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Construction IV", Ids.Research.Cp4Packing).Description(desc).AddRequirementForLifetimeProduction(Ids.Products.Electronics2, 20).AddRecipeToUnlock(Ids.Recipes.Cp4AssemblyElectrifiedT2).AddProductIcon(Ids.Products.ConstructionParts4).AddProductToUnlock(Ids.Products.MaintenanceT2, true).AddMachineToUnlock(Ids.Buildings.MaintenanceDepotT2, true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Basic computing", Ids.Research.BasicComputing).UseDescriptionFrom((Proto.ID) Ids.DataCenters.Mainframe).AddProductIcon(Ids.Products.Computing).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.DataCenters.Mainframe).AddMachineToUnlock(Ids.Machines.AssemblyRoboticT1).AddRecipeToUnlock(Ids.Recipes.CpBricksAssemblyT4).AddRecipeToUnlock(Ids.Recipes.CpAssemblyT4).AddRecipeToUnlock(Ids.Recipes.Cp2AssemblyT4).AddRecipeToUnlock(Ids.Recipes.Cp3AssemblyT3).AddRecipeToUnlock(Ids.Recipes.Cp4AssemblyRoboticT1).AddRecipeToUnlock(Ids.Recipes.MechPartsAssemblyT4).AddRecipeToUnlock(Ids.Recipes.VehicleParts1AssemblyT4).AddRecipeToUnlock(Ids.Recipes.LabEquipment1AssemblyT3).AddRecipeToUnlock(Ids.Recipes.LabEquipment2AssemblyT2).AddRecipeToUnlock(Ids.Recipes.LabEquipment3AssemblyT2).AddRecipeToUnlock(Ids.Recipes.HouseholdGoodsAssemblyT3).AddRecipeToUnlock(Ids.Recipes.PCBAssemblyT2).AddRecipeToUnlock(Ids.Recipes.ElectronicsAssemblyT4).AddRecipeToUnlock(Ids.Recipes.Electronics2AssemblyT2).AddMachineToUnlock(Ids.Machines.ConcreteMixerT3, true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Statue of maintenance", Ids.Research.StatueOfMaintenance).UseDescriptionFrom((Proto.ID) Ids.Buildings.StatueOfMaintenance).AddLayoutEntityToUnlock(Ids.Buildings.StatueOfMaintenance).AddLayoutEntityToUnlock(Ids.Buildings.StatueOfMaintenanceGolden).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Housing III", Ids.Research.Housing3).AddLayoutEntityToUnlock(Ids.Buildings.HousingT3).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Settlement decorations", Ids.Research.SettlementDecorations).AddLayoutEntityToUnlock(Ids.Buildings.SettlementFountain).AddLayoutEntityToUnlock(Ids.Buildings.SettlementPillar).AddLayoutEntityToUnlock(Ids.Buildings.SettlementSquare1).AddLayoutEntityToUnlock(Ids.Buildings.SettlementSquare2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Tomb of Captains", Ids.Research.TombOfCaptains).UseDescriptionFrom((Proto.ID) Ids.Buildings.TombOfCaptainsStage1).AddLayoutEntityToUnlock(Ids.Buildings.TombOfCaptainsStage1).AddLayoutEntityToUnlock(Ids.Buildings.TombOfCaptainsStage2).AddLayoutEntityToUnlock(Ids.Buildings.TombOfCaptainsStage3).AddLayoutEntityToUnlock(Ids.Buildings.TombOfCaptainsStage4).AddLayoutEntityToUnlock(Ids.Buildings.TombOfCaptainsStage5).AddLayoutEntityToUnlock(Ids.Buildings.TombOfCaptainsStageFinal).AddCropToUnlock(Ids.Crops.Flowers).AddProductToUnlock(Ids.Products.Flowers).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Assembly (robotic) II", Ids.Research.Assembler3).AddMachineToUnlock(Ids.Machines.AssemblyRoboticT2).AddRecipeToUnlock(Ids.Recipes.Cp4AssemblyRoboticT2).AddRecipeToUnlock(Ids.Recipes.ElectronicsAssemblyT5).AddRecipeToUnlock(Ids.Recipes.Electronics2AssemblyT3).AddRecipeToUnlock(Ids.Recipes.Electronics3AssemblyRoboticT2).AddRecipeToUnlock(Ids.Recipes.PCBAssemblyT3).AddRecipeToUnlock(Ids.Recipes.ServerAssemblyT2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Microchip production", Ids.Research.MicrochipProduction).UseDescriptionFrom((Proto.ID) Ids.Machines.MicrochipMachine).AddRequiredProto(Ids.Technology.Microchip).AddMachineToUnlock(Ids.Machines.SiliconCrystallizer, true).AddMachineToUnlock(Ids.Machines.MicrochipMachine, true).AddRecipeToUnlock(Ids.Recipes.Electronics3AssemblyRoboticT1).AddProductIcon(Ids.Products.Microchips).AddProductIcon(Ids.Products.Electronics3).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Microchip production II", Ids.Research.MicrochipProduction2).AddMachineToUnlock(Ids.Machines.MicrochipMachineT2, true).BuildAndAdd();
    }

    public ConstructionResearchData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
