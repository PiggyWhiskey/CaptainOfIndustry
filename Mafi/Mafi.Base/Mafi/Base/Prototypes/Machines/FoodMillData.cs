// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.FoodMillData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class FoodMillData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Mill", Ids.Machines.FoodMill).Description("Grinds organic products into a fine powder or oil.", "short description of a machine").SetCost(Costs.Machines.FoodMill).SetElectricityConsumption(120.Kw()).SetCategories(Ids.ToolbarCategories.MachinesFood).SetLayout("   [3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][3][3][3][3]>#X", "A~>[3][3][3][3][3][3][3][3]>@Y", "C@>[3][3][3][3][3][3][3][3]>~Z", "   [3][3][3][3][3][3][3][3]   ").SetPrefabPath("Assets/Base/Machines/Food/Mill.prefab").SetMachineSound("Assets/Base/Machines/Food/Mill/MillSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(60.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Wheat milling", Ids.Recipes.WheatMilling, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Wheat).SetDurationSeconds(30).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Flour).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.AnimalFeed).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Soybean milling", Ids.Recipes.SoybeanMilling, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Soybean).SetDurationSeconds(30).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.CookingOil).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.AnimalFeed).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Canola milling", Ids.Recipes.CanolaMilling, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Canola).SetDurationSeconds(30).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.CookingOil).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.AnimalFeed).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Corn milling (wet)", Ids.Recipes.CornMilling, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Corn).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.CornMash).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.AnimalFeed).BuildAndAdd();
    }

    public FoodMillData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
