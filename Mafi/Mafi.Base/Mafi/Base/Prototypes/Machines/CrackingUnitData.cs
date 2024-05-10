// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.CrackingUnitData
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
  internal class CrackingUnitData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Cracking unit", Ids.Machines.HydroCrackerT1).Description("Transforms different fuel types between each other to help with consumption imbalance.", "short description of a machine").SetCost(Costs.Machines.HydroCrackerT1).SetElectricityConsumption(160.Kw()).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("   [2][5][5][5][5][6][6][4][2]   ", "   [2][8][8][8][8][6][7][7][7]   ", "A@>[2][8][8][8][8][8][7][7][7]>@X", "   [2][2][8][8][8][6][7][7][7]   ", "         [1][6][3][3][3][3]      ", "         [1][2][2][2][2][2]      ", "         B@^         v@Y         ").SetPrefabPath("Assets/Base/Machines/Oil/HydroCracker.prefab").SetMachineSound("Assets/Base/Machines/Oil/HydroCracker/HydroCrackerSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("HeavyOil cracking", Ids.Recipes.HeavyOilCracking, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.HeavyOil).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Hydrogen).SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Diesel, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.FuelGas, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("HeavyOil cracking", Ids.Recipes.HeavyOilCrackingToNaphtha, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.HeavyOil).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Hydrogen).SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Naphtha, "X").AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.FuelGas, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Naphtha reforming", Ids.Recipes.NaphthaReforming, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Naphtha).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Hydrogen).SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Diesel, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.FuelGas, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Diesel reforming", Ids.Recipes.DieselReforming, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Diesel).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SteamHi).SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Naphtha, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SourWater, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Naphtha reforming", Ids.Recipes.NaphthaReformingToGas, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Naphtha).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SteamHi).SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(10, Ids.Products.FuelGas, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SourWater, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("FuelGas reforming", Ids.Recipes.FuelGasReforming, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.FuelGas).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Oxygen).SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Diesel, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Water, "Y").BuildAndAdd();
    }

    public CrackingUnitData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
