// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.FoodProcessorData
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
  internal class FoodProcessorData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Food processor", Ids.Machines.FoodProcessor).Description("Can produce different types of food from given ingredients.", "short description of a machine").SetCost(Costs.Machines.FoodProcessor).SetElectricityConsumption(100.Kw()).SetCategories(Ids.ToolbarCategories.MachinesFood).SetLayout("   [4][4][4][4][4][4][4]   ", "A~>[4][4][4][4][4][4][4]>~X", "B~>[4][4][4][4][4][4][4]>~Y", "C~>[4][4][4][4][4][4][4]>#Z", "D#>[4][4][4][4][4][4][4]   ", "   [4][4][4][4][4][4][4]   ", "E@>[4][4][4][4][4][4][4]   ").SetPrefabPath("Assets/Base/Machines/Food/FoodProcessor.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(120.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Tofu making", Ids.Recipes.TofuProduction, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Soybean).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Sulfur).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Limestone).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Tofu).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.AnimalFeed).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Meat processing", Ids.Recipes.MeatProcessing, machine).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.ChickenCarcass).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Water).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Salt).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(5, Ids.Products.Meat).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.MeatTrimmings).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Meat processing", Ids.Recipes.MeatProcessingTrimmings, machine).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.ChickenCarcass).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(9, Ids.Products.MeatTrimmings).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Sausage processing", Ids.Recipes.SausageProduction, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MeatTrimmings).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Flour).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Salt).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Sausage).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Snack production", Ids.Recipes.SnackProductionPotato, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Potato).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Salt).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.CookingOil).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Plastic).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Snack).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.Biomass).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Snack production", Ids.Recipes.SnackProductionCorn, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Corn).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Salt).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.CookingOil).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Plastic).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Snack).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.Biomass).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Sugar refining (cane)", Ids.Recipes.SugarRefiningCane, machine).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.SugarCane).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Sugar, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Biomass, "Y").BuildAndAdd();
    }

    public FoodProcessorData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
