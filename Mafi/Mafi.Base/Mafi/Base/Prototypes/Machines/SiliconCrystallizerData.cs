// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.SiliconCrystallizerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class SiliconCrystallizerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration duration1 = 60.Seconds();
      Duration totalDuration1 = Duration.FromKeyframes(440);
      Duration duration2 = duration1 - totalDuration1 - 5.Seconds();
      Duration totalDuration2 = Duration.FromKeyframes(120);
      int times = (duration1 - totalDuration1).Ticks / totalDuration2.Ticks;
      MachineProto machine = registrator.MachineProtoBuilder.Start("Silicon crystallizer", Ids.Machines.SiliconCrystallizer).Description("Produces monocrystalline silicon. Dips a rod with seed crystal into molten silicon and slowly pulls it upwards and rotates it simultaneously. The final ingot is then sliced into wafers.", "short description of a machine").SetCost(Costs.Machines.SiliconCrystallizer).SetElectricityConsumption(500.Kw()).SetComputingConsumption(Computing.FromTFlops(4)).DisableBoost().SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("   [2][3][3][3][2][2]   ", "A#>[2][3][3][3][2][2]   ", "   [2][6][6][6][2][2]   ", "   [2][6][6][6][3][3]   ", "B@>[2][6][6][6][3][3]>#X").SetPrefabPath("Assets/Base/Machines/Electronics/SiliconCrystallizer.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatTimes(totalDuration2, times, stateName: "Motor")).SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration1, AnimationWithPauseParams.Mode.ExtendPauseToFit, 0.Seconds(), stateName: "Arm")).AddEmissionParams(EmissionParams.Timed(ImmutableArray<string>.Empty, "SiliconCrystallizer", 0.Seconds(), duration2, 5f, 0.08f, 0.03f)).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Silicon crystallization", Ids.Recipes.SiliconCrystallization, machine).AddInput<RecipeProtoBuilder.State>(24, Ids.Products.PolySilicon).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Water).SetDurationSeconds(60).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.SiliconWafer).BuildAndAdd();
    }

    public SiliconCrystallizerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
