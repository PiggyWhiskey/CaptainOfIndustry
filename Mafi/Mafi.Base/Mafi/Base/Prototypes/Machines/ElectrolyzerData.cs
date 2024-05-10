// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.ElectrolyzerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class ElectrolyzerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      string[] strArray = new string[5]
      {
        "   [3][3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3][3]>@X",
        "A@>[3][3][3][3][3][3][3][3]   ",
        "   [3][3][3][3][3][3][3][3]>@Y",
        "   [3][3][3][3][3][3][3][3]   "
      };
      LocStr desc = Loc.Str(Ids.Machines.Electrolyzer.ToString() + "__desc", "Decomposes a product into simpler substances by passing an electric current through it.", "description of a machine");
      MachineProto machineProto = registrator.MachineProtoBuilder.Start("Electrolyzer II", Ids.Machines.ElectrolyzerT2).Description(desc).SetCost(Costs.Machines.ElectrolyzerT2).SetElectricityConsumption(4 * 900.Kw()).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout(strArray).SetPrefabPath("Assets/Base/Machines/Water/ElectrolyzerT2.prefab").DisableBoost().EnableInstancedRendering().BuildAndAdd();
      MachineProto machine = registrator.MachineProtoBuilder.Start("Electrolyzer", Ids.Machines.Electrolyzer).Description(desc).SetCost(Costs.Machines.Electrolyzer).SetElectricityConsumption(900.Kw()).SetCategories(Ids.ToolbarCategories.MachinesWater).SetNextTier(machineProto).SetLayout(strArray).SetPrefabPath("Assets/Base/Machines/Water/ElectrolyzerT1.prefab").DisableBoost().EnableInstancedRendering().BuildAndAdd();
      addBrineRecipe(Ids.Recipes.BrineElectrolysis, machine);
      addBrineRecipe(Ids.Recipes.BrineElectrolysisT2, machineProto, 4);
      addWaterRecipe(Ids.Recipes.WaterElectrolysis, machine, 40.Seconds());
      addWaterRecipe(Ids.Recipes.WaterElectrolysisT2, machineProto, 10.Seconds());
      addAmmoniaRecipe(Ids.Recipes.AmmoniaElectrolysis, machine);
      addAmmoniaRecipe(Ids.Recipes.AmmoniaElectrolysisT2, machineProto, 4);

      void addBrineRecipe(RecipeProto.ID recipeId, MachineProto machine, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Brine electrolysis", recipeId, machine).AddInput<RecipeProtoBuilder.State>(6 * mult, Ids.Products.Brine).SetDurationSeconds(10).AddOutput<RecipeProtoBuilder.State>(4 * mult, Ids.Products.Chlorine).BuildAndAdd();
      }

      void addWaterRecipe(RecipeProto.ID recipeId, MachineProto machine, Duration duration)
      {
        registrator.RecipeProtoBuilder.Start("Water electrolysis", recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Water).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Oxygen, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Hydrogen, "Y").BuildAndAdd();
      }

      void addAmmoniaRecipe(RecipeProto.ID recipeId, MachineProto machine, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Ammonia electrolysis", recipeId, machine).AddInput<RecipeProtoBuilder.State>(4 * mult, Ids.Products.Ammonia).SetDurationSeconds(10).AddOutput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.Nitrogen, "X").AddOutput<RecipeProtoBuilder.State>(4 * mult, Ids.Products.Hydrogen, "Y").BuildAndAdd();
      }
    }

    public ElectrolyzerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
