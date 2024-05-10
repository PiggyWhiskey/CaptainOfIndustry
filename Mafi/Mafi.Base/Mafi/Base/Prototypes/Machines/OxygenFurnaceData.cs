// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.OxygenFurnaceData
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
  internal class OxygenFurnaceData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration totalDuration = Duration.FromKeyframes(810);
      Duration pauseAt = Duration.FromKeyframes(480);
      string[] strArray = new string[7]
      {
        "                     ^@S   ",
        "   [6][6][6][6][6][6][1]   ",
        "   [6][6][6][6][6][6][2]   ",
        "A'>[6][6][6][6][6][6][2]>'X",
        "   [6][6][6][6][6][6][2]   ",
        "   [6][6][6][6][6][6][2]   ",
        "                     B@^   "
      };
      LocStr desc = Loc.Str(Ids.Machines.OxygenFurnace.ToString() + "__desc", "Creates steel by blowing pure oxygen under high-pressure into molten iron, lowering its carbon content.", "description of a machine");
      MachineProto machineProto = registrator.MachineProtoBuilder.Start("Oxygen furnace II", Ids.Machines.OxygenFurnaceT2).Description(desc).SetLayout(strArray).SetElectricityConsumption(200.Kw()).SetPrefabPath("Assets/Base/Machines/MetalWorks/OxygenFurnaceT2.prefab").SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt)).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetCost(Costs.Machines.OxygenFurnace2).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steel smelting", Ids.Recipes.SteelSmeltingT2, machineProto).SetDurationSeconds(40).AddInput<RecipeProtoBuilder.State>(32, Ids.Products.MoltenIron, "A").AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Oxygen, "B").AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenSteel, "X").AddOutput<RecipeProtoBuilder.State>(24, Ids.Products.Exhaust, "S").BuildAndAdd();
      registrator.MachineProtoBuilder.Start("Oxygen furnace", Ids.Machines.OxygenFurnace).Description(desc).SetLayout(strArray).SetNextTier(machineProto).SetElectricityConsumption(120.Kw()).SetPrefabPath("Assets/Base/Machines/MetalWorks/OxygenFurnace.prefab").SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt)).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetCost(Costs.Machines.OxygenFurnace).EnableSemiInstancedRendering().SetProtoToCopyAnimationsFrom(Ids.Machines.OxygenFurnaceT2).AddMaterialSwapForAnimationsLoad("OxygenFurnace", "OxygenFurnaceT2").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steel smelting", Ids.Recipes.SteelSmelting, Ids.Machines.OxygenFurnace).SetDurationSeconds(40).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenIron, "A").AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Oxygen, "B").AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenSteel, "X").AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Exhaust, "S").BuildAndAdd();
    }

    public OxygenFurnaceData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
