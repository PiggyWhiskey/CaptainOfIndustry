// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Research.PowerPlantResearchData
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

#nullable disable
namespace Mafi.Base.Prototypes.Research
{
  internal class PowerPlantResearchData : IResearchNodesData, IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.ResearchNodeProtoBuilder.Start("Electricity", Ids.Research.Electricity).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.DieselGenerator).AddProductToUnlock(Ids.Products.Electricity, true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Maintenance", Ids.Research.PowerAndMaintenance).UseDescriptionFrom((Proto.ID) Ids.Buildings.MaintenanceDepotT0).AddMachineToUnlock(Ids.Buildings.MaintenanceDepotT0, true).AddProductToUnlock(Ids.Products.MaintenanceT1, true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Beacon", Ids.Research.Beacon).AddLayoutEntityToUnlock(Ids.Buildings.Beacon).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Power generation II", Ids.Research.PowerGeneration2).Description("Unlocks power generation from coal.", "description of a research node in the research tree").AddMachineToUnlock(Ids.Machines.BoilerCoal).AddRecipeToUnlock(Ids.Recipes.SteamGenerationCoal).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.TurbineHighPress).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.PowerGeneratorT1).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.Flywheel).AddRecipeToUnlock(Ids.Recipes.SmokeStackHpSteam).AddRecipeToUnlock(Ids.Recipes.SmokeStackLargeHpSteam, true).AddRecipeToUnlock(Ids.Recipes.SmokeStackLpSteam).AddRecipeToUnlock(Ids.Recipes.SmokeStackLargeLpSteam, true).AddProductIcon(IdsCore.Products.Electricity).AddProductToUnlock(Ids.Products.MechanicalPower, hideInUi: true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Large generator", Ids.Research.DieselGeneratorLarge).UseDescriptionFrom((Proto.ID) Ids.Machines.DieselGeneratorT2).AddMachineToUnlock(Ids.Machines.DieselGeneratorT2).AddProductIcon(IdsCore.Products.Electricity).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Electric boiler", Ids.Research.BoilerElectric).AddMachineToUnlock(Ids.Machines.BoilerElectric).AddRecipeToUnlock(Ids.Recipes.SteamGenerationElectric).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Mechanical power storage", Ids.Research.MechPowerStorage).Description("Provides mechanical power storage and turbines auto-balancing capability.", "description of a research node in the research tree").BuildAndAdd().MarkAsObsolete();
      registrator.ResearchNodeProtoBuilder.Start("Power generation III", Ids.Research.PowerGeneration3).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.TurbineLowPress).AddRecipeToUnlock(Ids.Recipes.SmokeStackDepletedSteam).AddRecipeToUnlock(Ids.Recipes.SmokeStackLargeDepletedSteam, true).AddProductIcon(IdsCore.Products.Electricity).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Power generation IV", Ids.Research.PowerGeneration4).UseDescriptionFrom((Proto.ID) Ids.Machines.PowerGeneratorT2).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.TurbineHighPressT2).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.TurbineLowPressT2).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.PowerGeneratorT2).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.CoolingTowerT2).AddRecipeToUnlock(Ids.Recipes.SteamHpCondensationT2).AddRecipeToUnlock(Ids.Recipes.SteamLpCondensationT2).AddRecipeToUnlock(Ids.Recipes.SteamDepletedCondensationT2).AddProductIcon(IdsCore.Products.Electricity).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Water recovery", Ids.Research.WaterRecovery).AddMachineToUnlock(Ids.Machines.CoolingTowerT1, true).AddProductIcon(Ids.Products.Water).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Thermal storage", Ids.Research.ThermalStorage).UseDescriptionFrom((Proto.ID) Ids.Buildings.ThermalStorage).AddLayoutEntityToUnlock(Ids.Buildings.ThermalStorage).AddProductToUnlock(Ids.Products.Heat, hideInUi: true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Uranium enrichment", Ids.Research.UraniumEnrichment).UseDescriptionFrom((Proto.ID) Ids.Machines.UraniumEnrichmentPlant).AddRequiredProto(Ids.Technology.NuclearEnergy).AddMachineToUnlock(Ids.Machines.UraniumEnrichmentPlant).AddMachineToUnlock(Ids.Machines.SettlingTank).AddRecipeToUnlock(Ids.Recipes.UraniumEnrichment).AddRecipeToUnlock(Ids.Recipes.UraniumCrushing).AddRecipeToUnlock(Ids.Recipes.UraniumCrushingT2).AddRecipeToUnlock(Ids.Recipes.UraniumLeaching).AddRecipeToUnlock(Ids.Recipes.FluorideLeaching).AddRecipeToUnlock(Ids.Recipes.UraniumRodsAssemblyT1).AddRecipeToUnlock(Ids.Recipes.ToxicSlurryDumping).AddProductIcon(Ids.Products.HydrogenFluoride).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Nuclear reactor", Ids.Research.NuclearReactor).UseDescriptionFrom((Proto.ID) Ids.Buildings.NuclearReactor).AddLayoutEntityToUnlock(Ids.Buildings.NuclearReactor).AddLayoutEntityToUnlock(Ids.Buildings.NuclearWasteStorage).AddProductToUnlock(Ids.Products.UraniumRod).AddProductToUnlock(Ids.Products.SpentFuel).AddProductIcon(Ids.Products.Electricity).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Nuclear reactor II", Ids.Research.NuclearReactor2).UseDescriptionFrom((Proto.ID) Ids.Buildings.NuclearReactorT2).AddLayoutEntityToUnlock(Ids.Buildings.NuclearReactorT2).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.NuclearFuelReprocessingPlant).AddRecipeToUnlock(Ids.Recipes.SpentFuelReprocessing).AddRecipeToUnlock(Ids.Recipes.MoxRodsAssemblyT1).AddRecipeToUnlock(Ids.Recipes.ReprocessedUraniumEnrichment).AddRecipeToUnlock(Ids.Recipes.ShreddingRetiredWaste).AddProductToUnlock(Ids.Products.MoxRod, true).AddProductToUnlock(Ids.Products.SpentMox).AddProductToUnlock(Ids.Products.RetiredWaste).AddRecipeToUnlock(Ids.Recipes.UraniumEnrichment20).AddRequirementForLifetimeProduction(Ids.Products.SpentFuel, 200).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Fast breeder reactor", Ids.Research.NuclearReactor3).UseDescriptionFrom((Proto.ID) Ids.Buildings.FastBreederReactor).AddLayoutEntityToUnlock(Ids.Buildings.FastBreederReactor).AddRecipeToUnlock(Ids.Recipes.CoreFuelFromEnriched).AddRecipeToUnlock(Ids.Recipes.BlanketFuelFromDepleted).AddRecipeToUnlock(Ids.Recipes.BlanketFuelFromYellowcake).AddRecipeToUnlock(Ids.Recipes.BlanketFuelReprocessing).AddRecipeToUnlock(Ids.Recipes.SpentFuelToBlanket).AddRecipeToUnlock(Ids.Recipes.SpentMoxToBlanket).AddRecipeToUnlock(Ids.Recipes.CoreFuelReprocessing).AddRequirementForLifetimeProduction(Ids.Products.SpentMox, 60).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Solar panels", Ids.Research.SolarPanels).UseDescriptionFrom((Proto.ID) Ids.Machines.SolarPanel).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.SolarPanel).AddRecipeToUnlock(Ids.Recipes.SolarCellAssemblyT1).AddRecipeToUnlock(Ids.Recipes.SolarCellAssemblyT2, true).AddRecipeToUnlock(Ids.Recipes.SolarCellAssemblyT3, true).AddEdictToUnlock(Ids.Edicts.SolarPowerIncrease).AddEdictToUnlock(Ids.Edicts.SolarPowerIncreaseT2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Solar panels II", Ids.Research.SolarPanels2).UseDescriptionFrom((Proto.ID) Ids.Machines.SolarPanelMono).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.Machines.SolarPanelMono).AddRecipeToUnlock(Ids.Recipes.SolarCellMonoAssemblyT1).AddRecipeToUnlock(Ids.Recipes.ShreddingPolyCells).AddEdictToUnlock(Ids.Edicts.SolarPowerIncreaseT3).BuildAndAdd();
    }

    public PowerPlantResearchData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
