// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.ConcreteMixerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class ConcreteMixerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      string[] strArray = new string[6]
      {
        "         C@v            ",
        "   [1][2][2][2]         ",
        "A~>[2][3][3][3][2][2]   ",
        "D#>[2][3][3][3][2][2]>#X",
        "B~>[2][3][3][3][2][2]   ",
        "   [1][2][2][2]         "
      };
      int quantity1 = 1;
      int quantity2 = 2;
      int quantity3 = 6;
      int quantity4 = 4;
      int quantity5 = 8;
      LocStr desc = Loc.Str(Ids.Machines.ConcreteMixerT2.ToString() + "__desc", "High-powered mixer that creates concrete. Also provides alternative recipes for concrete.", "description of a machine");
      MachineProto machine1 = registrator.MachineProtoBuilder.Start("Concrete Mixer III", Ids.Machines.ConcreteMixerT3).Description(desc).SetLayout(strArray).SetElectricityConsumption(400.Kw()).SetPrefabPath("Assets/Base/Machines/Infrastructure/ConcreteMixerT3.prefab").SetMachineSound("Assets/Base/Machines/Infrastructure/ConcreteMixer/ConcreteMixer_Sound.prefab").SetCategories(Ids.ToolbarCategories.Machines).SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(120.Percent()))).SetCost(Costs.Machines.MixerT3).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingSlagT3, machine1).AddInput<RecipeProtoBuilder.State>(2 * quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(2 * quantity2, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(2 * quantity3, Ids.Products.SlagCrushed).AddInput<RecipeProtoBuilder.State>(2 * quantity4, Ids.Products.Water).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(2 * quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingGravelT3, machine1).AddInput<RecipeProtoBuilder.State>(2 * quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(2 * quantity2, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(2 * quantity3, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(2 * quantity4, Ids.Products.Water).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(2 * quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingSlagMT3, machine1).AddInput<RecipeProtoBuilder.State>(2 * quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(2 * quantity2, Ids.Products.ManufacturedSand).AddInput<RecipeProtoBuilder.State>(2 * quantity3, Ids.Products.SlagCrushed).AddInput<RecipeProtoBuilder.State>(2 * quantity4, Ids.Products.Water).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(2 * quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingGravelMT3, machine1).AddInput<RecipeProtoBuilder.State>(2 * quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(2 * quantity2, Ids.Products.ManufacturedSand).AddInput<RecipeProtoBuilder.State>(2 * quantity3, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(2 * quantity4, Ids.Products.Water).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(2 * quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      MachineProto machine2 = registrator.MachineProtoBuilder.Start("Concrete Mixer II", Ids.Machines.ConcreteMixerT2).Description(desc).SetCost(Costs.Machines.MixerT2).SetElectricityConsumption(200.Kw()).SetNextTier(Ids.Machines.ConcreteMixerT3).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray).SetPrefabPath("Assets/Base/Machines/Infrastructure/ConcreteMixerT2.prefab").SetMachineSound("Assets/Base/Machines/Infrastructure/ConcreteMixer/ConcreteMixer_Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingSlagT2, machine2).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.SlagCrushed).AddInput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Water).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingGravelT2, machine2).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Water).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingSlagMT2, machine2).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.ManufacturedSand).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.SlagCrushed).AddInput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Water).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingGravelMT2, machine2).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.ManufacturedSand).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Water).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      MachineProto machine3 = registrator.MachineProtoBuilder.Start("Concrete Mixer", Ids.Machines.ConcreteMixer).Description("High-power mixer that creates concrete.", "short description of a machine").SetCost(Costs.Machines.Mixer).SetElectricityConsumption(100.Kw()).SetNextTier(Ids.Machines.ConcreteMixerT2).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray).SetPrefabPath("Assets/Base/Machines/Infrastructure/ConcreteMixerT1.prefab").SetMachineSound("Assets/Base/Machines/Infrastructure/ConcreteMixer/ConcreteMixer_Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingSlag, machine3).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.SlagCrushed).AddInput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Water).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingGravel, machine3).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Water).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingGravelM, machine3).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.ManufacturedSand).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Water).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Concrete mixing", Ids.Recipes.ConcreteMixingSlagM, machine3).AddInput<RecipeProtoBuilder.State>(quantity1, Ids.Products.Cement).AddInput<RecipeProtoBuilder.State>(quantity2, Ids.Products.ManufacturedSand).AddInput<RecipeProtoBuilder.State>(quantity3, Ids.Products.SlagCrushed).AddInput<RecipeProtoBuilder.State>(quantity4, Ids.Products.Water).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(quantity5, Ids.Products.ConcreteSlab).BuildAndAdd();
    }

    public ConcreteMixerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
