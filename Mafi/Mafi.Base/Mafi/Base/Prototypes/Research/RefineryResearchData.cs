// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Research.RefineryResearchData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;

#nullable disable
namespace Mafi.Base.Prototypes.Research
{
  internal class RefineryResearchData : IResearchNodesData, IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.ResearchNodeProtoBuilder.Start("Basic diesel", Ids.Research.BasicDiesel).Description("Enables to pump island's limited reserve of oil and convert it to diesel. Not very efficient.", "description of a research node in the research tree").AddMachineToUnlock(Ids.Machines.BasicDieselDistiller).AddRecipeToUnlock(Ids.Recipes.DieselDistillationBasic).AddMachineToUnlock(Ids.Machines.OilPump, true).AddMachineToUnlock(Ids.Machines.WasteDump).AddRecipeToUnlock(Ids.Recipes.WasteWaterDumping).AddLayoutEntityToUnlock(Ids.Buildings.StorageFluid).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Advanced diesel", Ids.Research.CrudeOilDistillation).Description("Advanced process for diesel production and much more.", "description of a research node in the research tree").AddRequiredProto(Ids.Technology.OilDrilling).AddMachineToUnlock(Ids.Machines.BoilerCoal).AddRecipeToUnlock(Ids.Recipes.SteamGenerationCoal).AddMachineToUnlock(Ids.Machines.DistillationTowerT1, true).AddMachineToUnlock(Ids.Machines.DistillationTowerT2, true).AddMachineToUnlock(Ids.Machines.Flare).AddRecipeToUnlock(Ids.Recipes.FlareHeavyOil).AddRecipeToUnlock(Ids.Recipes.FlareLightOil).AddRecipeToUnlock(Ids.Recipes.SourWaterDumping).AddProductIcon(Ids.Products.Diesel).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Sulfur processing", Ids.Research.SulfurProcessing).AddMachineToUnlock(Ids.Machines.SourWaterStripper).AddMachineToUnlock(Ids.Machines.IndustrialMixer).AddRecipeToUnlock(Ids.Recipes.SourWaterStripping).AddRecipeToUnlock(Ids.Recipes.AcidMixMixing).AddRecipeToUnlock(Ids.Recipes.SulfurNeutralization).AddRecipeToUnlock(Ids.Recipes.WasteAcidDumping).AddRecipeToUnlock(Ids.Recipes.FlareAmmonia).AddRecipeToUnlock(Ids.Recipes.SulfurBurning).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Gas combustion", Ids.Research.GasCombustion).AddMachineToUnlock(Ids.Machines.BoilerGas).AddRecipeToUnlock(Ids.Recipes.SteamGenerationHeavyOil).AddRecipeToUnlock(Ids.Recipes.SteamGenerationMediumOil).AddRecipeToUnlock(Ids.Recipes.SteamGenerationLightOil).AddRecipeToUnlock(Ids.Recipes.SteamGenerationFuelGas).AddRecipeToUnlock(Ids.Recipes.SteamGenerationHydrogen).AddRecipeToUnlock(Ids.Recipes.SmokeStackDepletedSteam).AddRecipeToUnlock(Ids.Recipes.SmokeStackLargeDepletedSteam, true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Biofuel", Ids.Research.Biofuel).AddMachineToUnlock(Ids.Machines.AnaerobicDigester).AddMachineToUnlock(Ids.Machines.HydroCrackerT1).AddRecipeToUnlock(Ids.Recipes.PotatoDigestion).AddRecipeToUnlock(Ids.Recipes.VegetablesDigestion).AddRecipeToUnlock(Ids.Recipes.FuelGasReforming).AddMachineToUnlock(Ids.Machines.AirSeparator, true).AddRecipeToUnlock(Ids.Recipes.SmokeStackNitrogen, true).AddRecipeToUnlock(Ids.Recipes.SmokeStackLargeNitrogen, true).AddRecipeToUnlock(Ids.Recipes.SmokeStackOxygen, true).AddRecipeToUnlock(Ids.Recipes.SmokeStackLargeOxygen, true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Biodiesel", Ids.Research.BioDiesel).AddMachineToUnlock(Ids.Machines.ChemicalPlant).AddRecipeToUnlock(Ids.Recipes.EthanolCookingOilReforming).AddRecipeToUnlock(Ids.Recipes.EthanolCookingOilReformingT2, true).AddRecipeToUnlock(Ids.Recipes.SoybeanMilling).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Naphtha processing", Ids.Research.NaphthaProcessing).AddMachineToUnlock(Ids.Machines.DistillationTowerT3, true).AddRecipeToUnlock(Ids.Recipes.RubberProductionNaphtha).AddRecipeToUnlock(Ids.Recipes.RubberProductionNaphthaAlt).AddRecipeToUnlock(Ids.Recipes.SteamGenerationNaphtha).AddRecipeToUnlock(Ids.Recipes.DieselReforming).AddRecipeToUnlock(Ids.Recipes.FlareNaphtha).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Reforming & cracking", Ids.Research.NaphthaReforming).AddMachineToUnlock(Ids.Machines.HydroCrackerT1).AddRecipeToUnlock(Ids.Recipes.NaphthaReforming).AddRecipeToUnlock(Ids.Recipes.NaphthaReformingToGas).AddRecipeToUnlock(Ids.Recipes.HeavyOilCracking).AddRecipeToUnlock(Ids.Recipes.HeavyOilCrackingToNaphtha).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Heavy oil cracking", Ids.Research.HeavyOilCracking).BuildAndAdd().SetAvailability(false);
      registrator.ResearchNodeProtoBuilder.Start("Hydrogen production", Ids.Research.HydrogenReforming).AddMachineToUnlock(Ids.Machines.HydrogenReformer).AddRecipeToUnlock(Ids.Recipes.HydrogenReforming).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Incineration plant", Ids.Research.IncinerationPlant).UseDescriptionFrom((Proto.ID) Ids.Machines.IncinerationPlant).AddMachineToUnlock(Ids.Machines.IncinerationPlant, true).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Synthetic rubber", Ids.Research.RubberProduction).AddMachineToUnlock(Ids.Machines.VacuumDistillationTower).AddRecipeToUnlock(Ids.Recipes.RubberProductionDieselWithCoal).AddRecipeToUnlock(Ids.Recipes.RubberProductionDiesel).AddProductIcon(Ids.Products.Rubber).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Plastic", Ids.Research.PlasticProduction).AddMachineToUnlock(Ids.Machines.PolymerizationPlant, true).AddProductIcon(Ids.Products.Plastic).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Household appliances", Ids.Research.HouseholdAppliances).AddRecipeToUnlock(Ids.Recipes.HouseholdAppliancesAssemblyT1).AddRecipeToUnlock(Ids.Recipes.HouseholdAppliancesAssemblyT2, true).AddRecipeToUnlock(Ids.Recipes.HouseholdAppliancesAssemblyT3, true).AddLayoutEntityToUnlock(Ids.Buildings.SettlementHouseholdAppliancesModule).AddProductIcon(Ids.Products.HouseholdAppliances).AddEdictToUnlock(Ids.Edicts.HouseholdAppliancesConsumptionIncrease).AddEdictToUnlock(Ids.Edicts.HouseholdAppliancesConsumptionIncreaseT2).AddEdictToUnlock(Ids.Edicts.HouseholdAppliancesConsumptionIncreaseT3).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Consumer electronics", Ids.Research.ConsumerElectronics).AddRecipeToUnlock(Ids.Recipes.ConsumerElectronicsAssemblyT1).AddLayoutEntityToUnlock(Ids.Buildings.SettlementConsumerElectronicsModule).AddProductIcon(Ids.Products.ConsumerElectronics).AddEdictToUnlock(Ids.Edicts.ConsumerElectronicsConsumptionIncrease).AddEdictToUnlock(Ids.Edicts.ConsumerElectronicsConsumptionIncreaseT2).AddEdictToUnlock(Ids.Edicts.ConsumerElectronicsConsumptionIncreaseT3).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Super heated steam", Ids.Research.SuperPressSteam).Description("Super pressurized steam heated to 800 °C. Can be leveraged to produce hydrogen using sulfur-iodine cycle which is more efficient than electrolysis.", "description of a research node in the research tree").AddMachineToUnlock(Ids.Machines.TurbineSuperPress).AddProductIcon(Ids.Products.SteamSp).AddRecipeToUnlock(Ids.Recipes.SteamGenerationSpElectric).AddRecipeToUnlock(Ids.Recipes.HydrogenProductionFromSteamSp).AddRecipeToUnlock(Ids.Recipes.SmokeStackSpSteam).AddRecipeToUnlock(Ids.Recipes.SmokeStackLargeSpSteam).AddRecipeToUnlock(Ids.Recipes.SteamSpCondensationT2).BuildAndAdd();
    }

    public RefineryResearchData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
