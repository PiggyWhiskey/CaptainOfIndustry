// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.UraniumEnrichmentPlant
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
  internal class UraniumEnrichmentPlant : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine = registrator.MachineProtoBuilder.Start("Enrichment plant", Ids.Machines.UraniumEnrichmentPlant).Description("Utilizes a large set of centrifuges to concentrate uranium-235 isotope out of natural uranium (a process called isotope separation). The resulting product is fissile uranium, ready to undergo a chain reaction in a nuclear reactor.", "short description of a machine").SetCost(Costs.Machines.UraniumEnrichmentPlant).SetElectricityConsumption(1200.Kw()).DisableBoost().SetCategories(Ids.ToolbarCategories.MachinesElectricity).SetLayout("   [4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ", "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ", "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4]>#Z", "A~>[4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ", "C#>[4][4][4][4][4][4][4][4][4][4][4][4][4][4]>~Y", "B@>[4][4][4][4][4][4][4][4][4][4][4][4][4][4]>@X", "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4]>@J", "   [4][4][4][4][4][4][4][4][4][4][4][4][4][4]   ").SetEmissionWhenWorking(1).SetPrefabPath("Assets/Base/Machines/PowerPlant/UraniumEnrichmentPlant.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(150.Percent()))).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Uranium enrichment", Ids.Recipes.UraniumEnrichment, machine).AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Yellowcake, "A").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.HydrogenFluoride, "B").SetDurationSeconds(80).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.UraniumEnriched).AddOutput<RecipeProtoBuilder.State>(5, Ids.Products.UraniumDepleted).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Uranium enrichment", Ids.Recipes.ReprocessedUraniumEnrichment, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.UraniumReprocessed, "C").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.HydrogenFluoride, "B").SetDurationSeconds(80).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.UraniumEnriched).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.UraniumDepleted).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Uranium enrichment", Ids.Recipes.UraniumEnrichment20, machine).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.UraniumEnriched, "C").AddInput<RecipeProtoBuilder.State>(1, Ids.Products.HydrogenFluoride, "B").SetDurationSeconds(160).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.UraniumEnriched20).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.UraniumDepleted).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Blanket fuel reprocessing", Ids.Recipes.BlanketFuelReprocessing, machine).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.BlanketFuelEnriched).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.BlanketFuel, "X").AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.CoreFuel, "J").BuildAndAdd();
    }

    public UraniumEnrichmentPlant()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
