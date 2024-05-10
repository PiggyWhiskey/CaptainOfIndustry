// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.BakingUnitData
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
  internal class BakingUnitData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration duration1 = 30.Seconds();
      Duration totalDuration1 = Duration.FromKeyframes(450);
      Duration totalDuration2 = Duration.FromKeyframes(60);
      Duration duration2 = duration1 + 2.Seconds() - totalDuration1;
      MachineProto machine = registrator.MachineProtoBuilder.Start("Baking unit", Ids.Machines.BakingUnit).Description("Bakes organic products such as bread.", "short description of a machine").SetCost(Costs.Machines.BakingUnit).SetElectricityConsumption(200.Kw()).SetCategories(Ids.ToolbarCategories.MachinesFood).SetLayout("A@>[3][3][3][3][2][2][2]   ", "B~>[3][3][3][3][3][2][2]   ", "C#>[3][3][3][3][3][2][2]>#X", "D#>[3][3][3][3][3][2][2]   ", "E#>[3][3][3][3][3][2][2]   ").SetPrefabPath("Assets/Base/Machines/Food/Bakery.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(totalDuration1, stateName: "Bakery")).SetAnimationParams((AnimationParams) AnimationParams.RepeatAutoTimes(totalDuration2, new Percent?(180.Percent()), new Duration?(totalDuration1), "Cutter")).AddEmissionParams(EmissionParams.Timed(ImmutableArray<string>.Empty, "Bakery", totalDuration1 - 2.Seconds(), duration2, 4f, 0.14f, 0.06f)).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Bread making", Ids.Recipes.BreadProduction, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Flour).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water).SetDurationSeconds(30).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Bread).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Cake production", Ids.Recipes.CakeProduction, machine).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Flour).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Sugar).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.CookingOil).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Eggs).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Fruit).SetDurationSeconds(30).AddOutput<RecipeProtoBuilder.State>(7, Ids.Products.Cake).BuildAndAdd();
    }

    public BakingUnitData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
