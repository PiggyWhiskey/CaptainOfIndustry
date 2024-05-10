// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.MetalCastersData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class MetalCastersData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration totalDuration1 = Duration.FromKeyframes(810);
      Duration delay = Duration.FromKeyframes(192);
      Duration duration = Duration.FromKeyframes(138);
      Duration pauseAt1 = Duration.FromKeyframes(530);
      Duration durationTicks1 = 40.Seconds();
      Duration durationTicks2 = 20.Seconds();
      string[] strArray1 = new string[7]
      {
        "   [5][5][5][2][1]   ",
        "   [5][5][5][2][1]   ",
        "   [5][5][5][2][1]   ",
        "A'>[5][5][5][2][1]   ",
        "   [5][5][5][2][1]   ",
        "   [5][5][5][2][1]   ",
        "B@>[1][2][1][1][1]>#X"
      };
      LocStr desc1 = Loc.Str(Ids.Machines.CasterCooled.ToString() + "__desc", "Casts molten materials into slabs. This one also utilizes water for cooling.", "description of a machine");
      MachineProtoBuilder.MachineProtoBuilderStateBase builderStateBase1 = registrator.MachineProtoBuilder.Start("Cooled caster II", Ids.Machines.CasterCooledT2).Description(desc1).SetCost(Costs.Machines.CasterCooledT2).SetLayout(strArray1).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetPrefabPath("Assets/Base/Machines/MetalWorks/CasterWithCoolingT2.prefab").UseAllRecipesAtStartOrAfterUnlock();
      Duration totalDuration2 = totalDuration1;
      Duration pauseAt2 = pauseAt1;
      Percent? nullable1 = new Percent?(140.Percent());
      Duration pauseDuration1 = new Duration();
      Percent? baseSpeed1 = nullable1;
      AnimationWithPauseParams animParams1 = AnimationParams.PlayOnceAndPauseAt(totalDuration2, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt2, pauseDuration1, baseSpeed1);
      MachineProto machineProto1 = builderStateBase1.SetAnimationParams((AnimationParams) animParams1).AddParticleParams(ParticlesParams.Timed("PouringOut", ((double) delay.Ticks / 1.4).RoundToInt().Ticks(), ((double) duration.Ticks / 1.4).RoundToInt().Ticks())).AddParticleParams(ParticlesParams.Timed("WaterCooling", ((double) pauseAt1.Ticks / 1.4).RoundToInt().Ticks() - 3.Seconds(), 3.Seconds(), recipesSelector: (Func<RecipeProto, bool>) (recipe => recipe.Id.Value.EndsWith("Cooled")))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steel casting (cooled)", Ids.Recipes.SteelCastingCooledT2, machineProto1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenSteel, "A").AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water, "B").SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Steel, "X").BuildAndAdd();
      MachineProtoBuilder.MachineProtoBuilderStateBase builderStateBase2 = registrator.MachineProtoBuilder.Start("Cooled caster", Ids.Machines.CasterCooled).Description(desc1).SetCost(Costs.Machines.CasterCooled).SetLayout(strArray1).SetNextTier(machineProto1).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetPrefabPath("Assets/Base/Machines/MetalWorks/CasterWithCooling.prefab").UseAllRecipesAtStartOrAfterUnlock();
      Duration totalDuration3 = totalDuration1;
      Duration pauseAt3 = pauseAt1;
      Duration pauseDuration2 = new Duration();
      Percent? nullable2 = new Percent?();
      Percent? baseSpeed2 = nullable2;
      AnimationWithPauseParams animParams2 = AnimationParams.PlayOnceAndPauseAt(totalDuration3, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt3, pauseDuration2, baseSpeed2);
      MachineProto machine1 = builderStateBase2.SetAnimationParams((AnimationParams) animParams2).AddParticleParams(ParticlesParams.Timed("PouringOut", ((double) delay.Ticks / 1.4).RoundToInt().Ticks(), ((double) duration.Ticks / 1.4).RoundToInt().Ticks())).AddParticleParams(ParticlesParams.Timed("WaterCooling", ((double) pauseAt1.Ticks / 1.4).RoundToInt().Ticks() - 3.Seconds(), 3.Seconds(), recipesSelector: (Func<RecipeProto, bool>) (recipe => recipe.Id.Value.EndsWith("Cooled")))).EnableSemiInstancedRendering().SetProtoToCopyAnimationsFrom(Ids.Machines.CasterCooledT2).AddMaterialSwapForAnimationsLoad("Caster", "CasterT2").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Steel casting (cooled)", Ids.Recipes.SteelCastingCooled, machine1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenSteel, "A").AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water, "B").SetDuration(durationTicks1).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Steel, "X").BuildAndAdd();
      string[] strArray2 = new string[7]
      {
        "   [5][5][5][2][1]   ",
        "   [5][5][5][2][1]   ",
        "   [5][5][5][2][1]   ",
        "A'>[5][5][5][2][1]   ",
        "   [5][5][5][2][1]   ",
        "   [5][5][5][2][1]   ",
        "   [1][2][1][1][1]>#X"
      };
      LocStr desc2 = Loc.Str(Ids.Machines.Caster.ToString() + "__desc", "Casts molten materials into slabs.", "description of a machine");
      MachineProtoBuilder.MachineProtoBuilderStateBase builderStateBase3 = registrator.MachineProtoBuilder.Start("Metal caster II", Ids.Machines.CasterT2).Description(desc2).SetCost(Costs.Machines.Caster2).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout(strArray2).SetPrefabPath("Assets/Base/Machines/MetalWorks/CasterT2.prefab").UseAllRecipesAtStartOrAfterUnlock();
      Duration totalDuration4 = totalDuration1;
      Duration pauseAt4 = pauseAt1;
      nullable2 = new Percent?(140.Percent());
      Duration pauseDuration3 = new Duration();
      Percent? baseSpeed3 = nullable2;
      AnimationWithPauseParams animParams3 = AnimationParams.PlayOnceAndPauseAt(totalDuration4, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt4, pauseDuration3, baseSpeed3);
      MachineProto machineProto2 = builderStateBase3.SetAnimationParams((AnimationParams) animParams3).AddParticleParams(ParticlesParams.Timed("PouringOut", delay, duration)).EnableSemiInstancedRendering().SetProtoToCopyAnimationsFrom(Ids.Machines.CasterCooledT2).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Iron casting", Ids.Recipes.IronCastingT2, machineProto2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenIron).SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Iron).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Copper casting", Ids.Recipes.CopperCastingT2, machineProto2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenCopper).SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.ImpureCopper).BuildAndAdd();
      MachineProto machine2 = registrator.MachineProtoBuilder.Start("Metal caster", Ids.Machines.Caster).Description(desc2).SetCost(Costs.Machines.Caster).SetNextTier(machineProto2).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout(strArray2).SetPrefabPath("Assets/Base/Machines/MetalWorks/Caster.prefab").UseAllRecipesAtStartOrAfterUnlock().SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration1, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt1)).AddParticleParams(ParticlesParams.Timed("PouringOut", delay, duration)).SetProtoToCopyAnimationsFrom(Ids.Machines.CasterCooledT2).AddMaterialSwapForAnimationsLoad("Caster", "CasterT2").EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Iron casting", Ids.Recipes.IronCasting, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenIron).SetDuration(durationTicks1).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Iron).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Copper casting", Ids.Recipes.CopperCasting, machine2).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenCopper).SetDuration(durationTicks1).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.ImpureCopper).BuildAndAdd();
    }

    public MetalCastersData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
