// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.GlassMakerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class GlassMakerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      registrator.MachineProtoBuilder.Start("Glass maker II", Ids.Machines.GlassMakerT2).Description("Casts molten glass into glass sheets and with much greater efficiency", "short description of a machine").SetLayout("   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "A'>[2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]>#X", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2][2]   ").SetPrefabPath("Assets/Base/Machines/Glass/GlassMakerT2.prefab").SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetElectricityConsumption(800.Kw()).SetAnimationParams((AnimationParams) AnimationParams.PlayOnce(Duration.FromKeyframes(330), customSpeed: new Percent?(120.Percent()), stateName: "InAnim")).SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(Duration.FromKeyframes(260), AnimationWithPauseParams.Mode.ExtendPauseToFit, 0.Seconds(), stateName: "OutAnim")).SetMachineSound("Assets/Base/Machines/Glass/GlassMaker/GlassMakerSound.prefab").SetCost(Costs.Machines.GlassMakerT2).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Glass casting", Ids.Recipes.GlassCastingT2, Ids.Machines.GlassMakerT2).SetDuration(20.Seconds()).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenGlass, "A").AddOutput<RecipeProtoBuilder.State>(24, Ids.Products.Glass, "X").BuildAndAdd();
      registrator.MachineProtoBuilder.Start("Glass maker", Ids.Machines.GlassMakerT1).Description("Casts molten glass into glass sheets", "short description of a machine").SetLayout("                           ^~Y            ", "   [2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2]   ", "A'>[2][2][2][2][2][2][2][2][2][2][2][2]>#X", "   [2][2][2][2][2][2][2][2][2][2][2][2]   ", "   [2][2][2][2][2][2][2][2][2][2][2][2]   ").SetPrefabPath("Assets/Base/Machines/Glass/GlassMakerT1.prefab").SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetElectricityConsumption(350.Kw()).SetAnimationParams((AnimationParams) AnimationParams.PlayOnce(Duration.FromKeyframes(330), customSpeed: new Percent?(120.Percent()), stateName: "InAnim")).SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(Duration.FromKeyframes(260), AnimationWithPauseParams.Mode.ExtendPauseToFit, 0.Seconds(), stateName: "OutAnim")).SetMachineSound("Assets/Base/Machines/Glass/GlassMaker/GlassMakerSound.prefab").SetCost(Costs.Machines.GlassMakerT1).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Glass casting", Ids.Recipes.GlassCastingT1, Ids.Machines.GlassMakerT1).SetDuration(20.Seconds()).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenGlass, "A").AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Glass, "X").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Slag, "Y").BuildAndAdd();
    }

    public GlassMakerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
