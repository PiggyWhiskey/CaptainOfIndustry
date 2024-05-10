// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.CopperElectrolysisData
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
  internal class CopperElectrolysisData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration totalDuration = Duration.FromKeyframes(540);
      MachineProto machine = registrator.MachineProtoBuilder.Start("Copper electrolysis", Ids.Machines.CopperElectrolysis).Description("Purifies copper by electrolytic refining to over 99.95% purity.", "short description of a machine").SetCost(Costs.Machines.CopperElectrolysis).SetElectricityConsumption(400.Kw()).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("   [5][5][5][5][5][5][5]>#X", "   [5][5][5][5][5][5][5]   ", "B@>[5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5]   ", "A#>[5][5][5][5][5][5][5]   ").SetPrefabPath("Assets/Base/Machines/MetalWorks/Electrolysis.prefab").SetAnimationParams((AnimationParams) AnimationParams.PlayOnce(totalDuration)).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Copper purification (acid)", Ids.Recipes.CopperElectrolysis, machine).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.ImpureCopper).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Acid).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Copper).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Copper purification (water)", Ids.Recipes.CopperElectrolysisWithWater, machine).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.ImpureCopper).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water).SetDurationSeconds(40).AddOutput<RecipeProtoBuilder.State>(13, Ids.Products.Copper).BuildAndAdd();
    }

    public CopperElectrolysisData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
