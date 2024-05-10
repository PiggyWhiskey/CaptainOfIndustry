// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Research.IntelligenceResearchData
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
  internal class IntelligenceResearchData : IResearchNodesData, IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.ResearchNodeProtoBuilder.Start("Captain's office", Ids.Research.CaptainsOffice).Description("Your own office! This is where all the important decision are made.", "description of a research node in the research tree").AddLayoutEntityToUnlock(Ids.Buildings.CaptainOfficeT1).AddEdictToUnlock(Ids.Edicts.MaintenanceReduction).AddEdictToUnlock(Ids.Edicts.FuelReduction).AddEdictToUnlock(Ids.Edicts.PopsBoostT1).AddEdictToUnlock(Ids.Edicts.FoodConsumptionIncrease).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Tools", Ids.Research.Tools).Description("Provides advanced tools for easier planning and management.", "description of a research node in the research tree").AddTechnologyToUnlock(IdsCore.Technology.CopyTool).AddTechnologyToUnlock(IdsCore.Technology.CutTool).AddTechnologyToUnlock(IdsCore.Technology.CloneTool).AddTechnologyToUnlock(IdsCore.Technology.UnityTool).AddTechnologyToUnlock(IdsCore.Technology.PauseTool).AddTechnologyToUnlock(IdsCore.Technology.UpgradeTool).AddTechnologyToUnlock(IdsCore.Technology.PlanningTool).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Blueprints", Ids.Research.Blueprints).UseDescriptionFrom(IdsCore.Technology.Blueprints).AddTechnologyToUnlock(IdsCore.Technology.Blueprints).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Terrain leveling", Ids.Research.TerrainLeveling).UseDescriptionFrom(IdsCore.Technology.TerrainLeveling).AddTechnologyToUnlock(IdsCore.Technology.TerrainLeveling).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Edicts", Ids.Research.Edicts1).Description("Some really useful edicts that you can apply in your office.", "description of a research node in the research tree").AddEdictToUnlock(Ids.Edicts.GrowthPause).AddEdictToUnlock(Ids.Edicts.PopsEviction).AddEdictToUnlock(Ids.Edicts.FoodConsumptionReduction).AddEdictToUnlock(Ids.Edicts.FarmYieldIncrease).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Edicts II", Ids.Research.Edicts2).Description("Another set of useful edicts.", "description of a research node in the research tree").AddEdictToUnlock(Ids.Edicts.HealthBonus).AddEdictToUnlock(Ids.Edicts.WaterConsumptionReduction).AddEdictToUnlock(Ids.Edicts.PopsQuarantine).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Captain's office II", Ids.Research.CaptainsOffice2).Description("A bit more spacious office. It provides access to advanced edicts and gives a passive increase in quick trade volume.", "description of a research node in the research tree").AddLayoutEntityToUnlock(Ids.Buildings.CaptainOfficeT2).AddEdictToUnlock(Ids.Edicts.PopsBoostT2).AddEdictToUnlock(Ids.Edicts.WaterConsumptionReductionT2).AddEdictToUnlock(Ids.Edicts.FarmYieldIncreaseT2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Edicts III", Ids.Research.Edicts3).AddEdictToUnlock(Ids.Edicts.PopsBoostT3).AddEdictToUnlock(Ids.Edicts.FoodConsumptionReductionT2).AddEdictToUnlock(Ids.Edicts.HealthBonusT2).AddEdictToUnlock(Ids.Edicts.FuelReductionT2).AddEdictToUnlock(Ids.Edicts.ShipFuelReduction).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Edicts IV", Ids.Research.Edicts4).AddEdictToUnlock(Ids.Edicts.FoodConsumptionIncreaseT2).AddEdictToUnlock(Ids.Edicts.WaterConsumptionReductionT3).AddEdictToUnlock(Ids.Edicts.FarmYieldIncreaseT3).AddEdictToUnlock(Ids.Edicts.MaintenanceReductionT2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Research lab II", Ids.Research.ResearchLab2).UseDescriptionFrom((Proto.ID) Ids.Buildings.ResearchLab2).AddLayoutEntityToUnlock(Ids.Buildings.ResearchLab2).AddRecipeToUnlock(Ids.Recipes.LabEquipment1AssemblyT1).AddProductIcon(Ids.Products.LabEquipment).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Research lab III", Ids.Research.ResearchLab3).UseDescriptionFrom((Proto.ID) Ids.Buildings.ResearchLab3).AddLayoutEntityToUnlock(Ids.Buildings.ResearchLab3).AddRecipeToUnlock(Ids.Recipes.LabEquipment2AssemblyT1).AddProductIcon(Ids.Products.LabEquipment2).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Research lab IV", Ids.Research.ResearchLab4).UseDescriptionFrom((Proto.ID) Ids.Buildings.ResearchLab4).AddLayoutEntityToUnlock(Ids.Buildings.ResearchLab4).AddRecipeToUnlock(Ids.Recipes.LabEquipment3AssemblyT1).AddProductIcon(Ids.Products.LabEquipment3).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Research lab V", Ids.Research.ResearchLab5).UseDescriptionFrom((Proto.ID) Ids.Buildings.ResearchLab5).AddLayoutEntityToUnlock(Ids.Buildings.ResearchLab5).AddRecipeToUnlock(Ids.Recipes.LabEquipment4AssemblyT2).AddProductIcon(Ids.Products.LabEquipment4).BuildAndAdd();
      registrator.ResearchNodeProtoBuilder.Start("Datacenter", Ids.Research.Datacenter).UseDescriptionFrom((Proto.ID) Ids.DataCenters.DataCenter).AddLayoutEntityToUnlock((StaticEntityProto.ID) Ids.DataCenters.DataCenter).AddProductIcon(Ids.Products.Server).AddMachineToUnlock(Ids.Buildings.MaintenanceDepotT3, true).AddProductIcon(Ids.Products.MaintenanceT3).AddMachineToUnlock(Ids.Machines.WaterChiller, true).AddRecipeToUnlock(Ids.Recipes.ServerAssemblyT1).AddRequirementForLifetimeProduction(Ids.Products.Electronics3, 20).BuildAndAdd();
    }

    public IntelligenceResearchData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
