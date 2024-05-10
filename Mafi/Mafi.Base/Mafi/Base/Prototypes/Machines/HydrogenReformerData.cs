// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.HydrogenReformerData
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
  internal class HydrogenReformerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Hydrogen reformer", Ids.Machines.HydrogenReformer).SetCost(Costs.Machines.HydrogenReformer).SetElectricityConsumption(250.Kw()).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("      B@v                        ", "   [5][5][5][5][5][5][5][5][5]>@Y", "   [5][5][5][5][5][5][5][5][5]   ", "   [2][2][2][2][2][2][2][2][4]>@Z", "   [1][1][2][2][2][2][2][2][1]   ", "A@>[1][1][2][2][2][2][2][1][1]>@X").SetPrefabPath("Assets/Base/Machines/Oil/HydrogenSynthesizer.prefab").SetMachineSound("Assets/Base/Machines/Oil/HydrogenSynthesizer/HydrogenSynthesizerSound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Hydrogen reforming", Ids.Recipes.HydrogenReforming, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.FuelGas, "A").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SteamHi, "B").SetDuration(OilDistillationData.DURATION).AddOutput<RecipeProtoBuilder.State>(14, Ids.Products.Hydrogen, "X").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.CarbonDioxide, "Y").BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Hydrogen from SP steam", Ids.Recipes.HydrogenProductionFromSteamSp, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Water).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.SteamSp).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Hydrogen, "X").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Oxygen, "Z").AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.SteamDepleted, "Y").BuildAndAdd();
    }

    public HydrogenReformerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
