// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.EvaporationPondData
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
  internal class EvaporationPondData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProtoBuilder.MachineProtoBuilderStateBase builderStateBase1 = registrator.MachineProtoBuilder.Start("Evaporation pond (heated)", Ids.Machines.EvaporationPondHeated).Description("Produces salt by evaporating residual water from brine. The process is accelerated by utilizing a set of electric heaters.", "short description of a machine").SetElectricityConsumption(250.Kw()).DisableBoost().SetCost(Costs.Machines.EvaporationPondHeated).SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout("   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "A@>[2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]>~X", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]>@Y", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ").SetPrefabPath("Assets/Base/Machines/Water/EvaporationPondHeated.prefab");
      Duration totalDuration1 = Duration.FromKeyframes(920);
      Duration pauseAt1 = Duration.FromKeyframes(220);
      Percent? nullable1 = new Percent?(200.Percent());
      Duration pauseDuration1 = new Duration();
      Percent? baseSpeed1 = nullable1;
      AnimationWithPauseParams animParams1 = AnimationParams.PlayOnceAndPauseAt(totalDuration1, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt1, pauseDuration1, baseSpeed1, "EvaporationPondWater");
      MachineProtoBuilder.MachineProtoBuilderStateBase builderStateBase2 = builderStateBase1.SetAnimationParams((AnimationParams) animParams1);
      Duration totalDuration2 = Duration.FromKeyframes(1420);
      Duration pauseAt2 = Duration.FromKeyframes(200);
      nullable1 = new Percent?(360.Percent());
      Duration pauseDuration2 = new Duration();
      Percent? baseSpeed2 = nullable1;
      AnimationWithPauseParams animParams2 = AnimationParams.PlayOnceAndPauseAt(totalDuration2, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt2, pauseDuration2, baseSpeed2, "EvaporationPondSalt");
      MachineProto machineProto = builderStateBase2.SetAnimationParams((AnimationParams) animParams2).SetEmissionWhenWorking(3).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Brine making", Ids.Recipes.BrineMakingHeated, machineProto).AddInput<RecipeProtoBuilder.State>(30, Ids.Products.Seawater).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Brine).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Salt making", Ids.Recipes.SaltMakingHeated, machineProto).AddInput<RecipeProtoBuilder.State>(30, Ids.Products.Seawater).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Salt).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Salt making", Ids.Recipes.SaltMakingFromBrineHeated, machineProto).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.Brine).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Salt).BuildAndAdd();
      MachineProtoBuilder.MachineProtoBuilderStateBase builderStateBase3 = registrator.MachineProtoBuilder.Start("Evaporation pond", Ids.Machines.EvaporationPond).Description("Produces salt by evaporating residual water from brine.", "short description of a machine").SetCost(Costs.Machines.EvaporationPond).DisableBoost().SetCategories(Ids.ToolbarCategories.MachinesWater).SetLayout("   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "A@>[2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]>~X", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]>@Y", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ").SetPrefabPath("Assets/Base/Machines/Water/EvaporationPond.prefab").SetNextTier(machineProto);
      Duration totalDuration3 = Duration.FromKeyframes(920);
      Duration pauseAt3 = Duration.FromKeyframes(220);
      Duration pauseDuration3 = new Duration();
      Percent? nullable2 = new Percent?();
      Percent? baseSpeed3 = nullable2;
      AnimationWithPauseParams animParams3 = AnimationParams.PlayOnceAndPauseAt(totalDuration3, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt3, pauseDuration3, baseSpeed3, "EvaporationPondWater");
      MachineProtoBuilder.MachineProtoBuilderStateBase builderStateBase4 = builderStateBase3.SetAnimationParams((AnimationParams) animParams3);
      Duration totalDuration4 = Duration.FromKeyframes(1420);
      Duration pauseAt4 = Duration.FromKeyframes(200);
      nullable2 = new Percent?(180.Percent());
      Duration pauseDuration4 = new Duration();
      Percent? baseSpeed4 = nullable2;
      AnimationWithPauseParams animParams4 = AnimationParams.PlayOnceAndPauseAt(totalDuration4, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt4, pauseDuration4, baseSpeed4, "EvaporationPondSalt");
      MachineProto machine = builderStateBase4.SetAnimationParams((AnimationParams) animParams4).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Brine making", Ids.Recipes.BrineMaking, machine).AddInput<RecipeProtoBuilder.State>(30, Ids.Products.Seawater).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Brine).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Salt making", Ids.Recipes.SaltMaking, machine).AddInput<RecipeProtoBuilder.State>(30, Ids.Products.Seawater).SetDurationSeconds(80).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Salt).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Salt making", Ids.Recipes.SaltMakingFromBrine, machine).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.Brine).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Salt).BuildAndAdd();
    }

    public EvaporationPondData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
