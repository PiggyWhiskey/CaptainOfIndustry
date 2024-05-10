// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.IncinerationPlantData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class IncinerationPlantData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto incinerator = registrator.MachineProtoBuilder.Start("Incineration plant", Ids.Machines.IncinerationPlant).Description("Burns waste with much better efficiency than a basic burner. The process is energy positive and generates steam.", "short description of a machine").SetCost(Costs.Machines.IncinerationPlant).SetElectricityConsumption(500.Kw()).SetCategories(Ids.ToolbarCategories.Waste).SetLayout("   [2][2][2][5][5][5][8][8][8][8][8][8][8][6]   ", "C@>[2][2][2][5][5][5][8][8][8][8][8][8][8][6]>@Y", "   [2][2][2][5][5][5][8][8][8][8][8][8][8][6]   ", "   [2][7][7][7][7][7][8][8][8][8][8][8][8][6]   ", "A#>[2][7][7][7][7][7][8][8][8][8][8][8][8][6]   ", "B~>[2][7][7][7][7][7][8][8][8][8][8][8][8][6]   ", "   [2][7][7][7][7][7][8][8][8][8][8][8][8][6]   ", "D@>[2][2][2][3][3][3][8][8][8][8][8][8][8][6]>@X").SetPrefabPath("Assets/Base/Machines/Waste/IncinerationPlant.prefab").SetAnimationParams((AnimationParams) AnimationParams.PlayOnce(Duration.FromKeyframes(320))).EnableSemiInstancedRendering().BuildAndAdd();
      Duration duration = 20.Seconds();
      addRecipe(Ids.Recipes.IncinerationOfWaste, Ids.Products.FuelGas, false);
      addRecipe(Ids.Recipes.IncinerationOfWastePressed, Ids.Products.FuelGas, true);
      addRecipe(Ids.Recipes.IncinerationOfWasteHydrogen, Ids.Products.Hydrogen, false);
      addRecipe(Ids.Recipes.IncinerationOfWastePressedHydrogen, Ids.Products.Hydrogen, true);

      void addRecipe(RecipeProto.ID recipeId, ProductProto.ID fuelId, bool isPressedWaste)
      {
        registrator.RecipeProtoBuilder.Start("Waste incineration", recipeId, incinerator).AddInput<RecipeProtoBuilder.State>(isPressedWaste ? 16 : 48, isPressedWaste ? Ids.Products.WastePressed : Ids.Products.Waste).AddInput<RecipeProtoBuilder.State>(2, fuelId, "C").AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Water, "D").SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(24, Ids.Products.Exhaust, "Y").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.SteamHi, "X").BuildAndAdd();
      }
    }

    public IncinerationPlantData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
