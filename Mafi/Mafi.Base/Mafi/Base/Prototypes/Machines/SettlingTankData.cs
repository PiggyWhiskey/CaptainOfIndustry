// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.SettlingTankData
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
  internal class SettlingTankData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Settling tank", Ids.Machines.SettlingTank).SetElectricityConsumption(120.Kw()).SetCategories(Ids.ToolbarCategories.Machines).SetCost(Costs.Machines.SettlingTank).SetLayout("         [3][3][3][3]            ", "   A@>[2][3][3][3][3][2][2][2]>~X", "C~>[2][3][3][3][3][3][2][2][2]   ", "   B@>[2][3][3][3][3][2][2][2]>@Y", "         [3][3][3][3]            ").SetPrefabPath("Assets/Base/Machines/Gold/SettlingTank.prefab").SetMachineSound("Assets/Base/Machines/Gold/SettlingTank/SettlingTankSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Gold settling", Ids.Recipes.GoldSettling, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.GoldOrePowder).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Acid).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.GoldOreConcentrate, "X").AddOutput<RecipeProtoBuilder.State>(9, Ids.Products.ToxicSlurry, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Uranium leaching", Ids.Recipes.UraniumLeaching, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.UraniumOreCrushed).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Acid).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Yellowcake, "X").AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.ToxicSlurry, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Fluoride leaching", Ids.Recipes.FluorideLeaching, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Rock).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Acid).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.HydrogenFluoride, "Y").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Slag, "X").BuildAndAdd();
    }

    public SettlingTankData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
