// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.GoldFurnaceData
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
  internal class GoldFurnaceData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration totalDuration = Duration.FromKeyframes(800);
      Duration pauseAt = Duration.FromKeyframes(120);
      MachineProto machine = registrator.MachineProtoBuilder.Start("Gold furnace", Ids.Machines.GoldFurnace).SetCost(Costs.Machines.GoldFurnace).SetElectricityConsumption(800.Kw()).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("            ^@Z         ", "      [2][2][2][2]      ", "A~>[2][2][2][2][2][2]>#X", "B~>[2][2][2][2][2]      ", "      [2][2][2][2]      ").SetPrefabPath("Assets/Base/Machines/Gold/GoldFurnace.prefab").SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration, AnimationWithPauseParams.Mode.ScaleAnimationSpeedToFit, pauseAt, 3.Seconds())).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Gold smelting", Ids.Recipes.GoldSmelting, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.GoldOreConcentrate).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Sand).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.Gold).AddQuarterExhaust<RecipeProtoBuilder.State>().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Gold smelting", Ids.Recipes.GoldScrapSmelting, machine).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.GoldScrap).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.Gold).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.Exhaust).BuildAndAdd();
    }

    public GoldFurnaceData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
