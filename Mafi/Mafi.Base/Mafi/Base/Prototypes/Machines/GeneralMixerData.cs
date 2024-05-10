// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.GeneralMixerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class GeneralMixerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      string[] strArray = new string[7]
      {
        "      D@v                  ",
        "   [2][3][3][3][3][3]      ",
        "   [2][3][3][3][3][3][3]   ",
        "A~>[2][3][3][4][4][3][3]>~X",
        "B~>[2][3][3][4][4][3][3]   ",
        "C~>[2][3][3][3][3][3][3]>@Y",
        "   [2][3][3][3][3][3]      "
      };
      LocStr desc = Loc.Str(Ids.Machines.IndustrialMixer.ToString() + "__desc", "High-power mixer for general materials mixing.", "description of a machine");
      MachineProto machineProto = registrator.MachineProtoBuilder.Start("Mixer II", Ids.Machines.IndustrialMixerT2).Description(desc).SetCost(Costs.Machines.IndustrialMixerT2).SetElectricityConsumption(200.Kw()).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray).SetPrefabPath("Assets/Base/Machines/General/IndustrialMixerT2.prefab").SetMachineSound("Assets/Base/Machines/Infrastructure/ConcreteMixer/ConcreteMixer_Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      MachineProto machine = registrator.MachineProtoBuilder.Start("Mixer", Ids.Machines.IndustrialMixer).Description(desc).SetCost(Costs.Machines.IndustrialMixer).SetElectricityConsumption(100.Kw()).SetNextTier(machineProto).SetCategories(Ids.ToolbarCategories.Machines).SetLayout(strArray).SetPrefabPath("Assets/Base/Machines/General/IndustrialMixerT1.prefab").SetMachineSound("Assets/Base/Machines/Infrastructure/ConcreteMixer/ConcreteMixer_Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop(new Percent?(80.Percent()))).EnableSemiInstancedRendering().BuildAndAdd();
      addGlassMixing(Ids.Recipes.GlassMixMixing, 20.Seconds(), machine);
      addGlassMixing(Ids.Recipes.GlassMixMixingT2, 10.Seconds(), machineProto);
      addGlassMixingWithAcid(Ids.Recipes.GlassMixMixingWithAcid, 20.Seconds(), machine);
      addGlassMixingWithAcid(Ids.Recipes.GlassMixMixingWithAcidT2, 10.Seconds(), machineProto);
      addAcidMixing(Ids.Recipes.AcidMixMixing, 20.Seconds(), machine);
      addAcidMixing(Ids.Recipes.AcidMixMixingT2, 10.Seconds(), machineProto);
      addSulfurNeutralization(Ids.Recipes.SulfurNeutralization, 20.Seconds(), machine);
      addSulfurNeutralization(Ids.Recipes.SulfurNeutralizationT2, 10.Seconds(), machineProto);
      addBrineProduction(Ids.Recipes.BrineProduction, 20.Seconds(), machine);
      addBrineProduction(Ids.Recipes.BrineProductionT2, 10.Seconds(), machineProto);
      addFilterMedia(Ids.Recipes.FilterMediaMixing, 20.Seconds(), machine);
      addFilterMedia(Ids.Recipes.FilterMediaMixingT2, 10.Seconds(), machineProto);
      addFilterMediaSandM(Ids.Recipes.FilterMediaMixingM, 20.Seconds(), machine);
      addFilterMediaSandM(Ids.Recipes.FilterMediaMixingMT2, 10.Seconds(), machineProto);
      addFertilizerMixing(Ids.Recipes.OrganicFertilizerProduction, 20.Seconds(), machine);
      addFertilizerMixing(Ids.Recipes.OrganicFertilizerProductionT2, 10.Seconds(), machineProto);
      addFertilizer2(Ids.Recipes.Fertilizer2Production, 20.Seconds(), machine);
      addFertilizer2(Ids.Recipes.Fertilizer2ProductionT2, 10.Seconds(), machineProto);
      addDirtMixing(Ids.Recipes.DirtMixing, 20.Seconds(), machine);
      addDirtMixing(Ids.Recipes.DirtMixingT2, 10.Seconds(), machineProto);
      addAnimalFeed(Ids.Recipes.AnimalFeedFromPotato, machine, Ids.Products.Potato, 10, 8, 10);
      addAnimalFeed(Ids.Recipes.AnimalFeedFromPotatoT2, machineProto, Ids.Products.Potato, 20, 16, 10);
      addAnimalFeed(Ids.Recipes.AnimalFeedFromWheat, machine, Ids.Products.Wheat, 10, 16, 10);
      addAnimalFeed(Ids.Recipes.AnimalFeedFromWheatT2, machineProto, Ids.Products.Wheat, 20, 32, 10);
      addAnimalFeed(Ids.Recipes.AnimalFeedFromCorn, machine, Ids.Products.Corn, 10, 12, 10);
      addAnimalFeed(Ids.Recipes.AnimalFeedFromCornT2, machineProto, Ids.Products.Corn, 20, 24, 10);
      addAnimalFeed(Ids.Recipes.AnimalFeedFromSoybean, machine, Ids.Products.Soybean, 10, 18, 10);
      addAnimalFeed(Ids.Recipes.AnimalFeedFromSoybeanT2, machineProto, Ids.Products.Soybean, 20, 36, 10);
      addCompost(Ids.Recipes.AnimalFeedCompost, machine, Ids.Products.AnimalFeed, 12, 6, 60);
      addCompost(Ids.Recipes.AnimalFeedCompostT2, machineProto, Ids.Products.AnimalFeed, 12, 6, 30);
      addCompost(Ids.Recipes.BiomassCompost, machine, Ids.Products.Biomass, 12, 8, 60);
      addCompost(Ids.Recipes.BiomassCompostT2, machineProto, Ids.Products.Biomass, 12, 8, 30);
      addCompost(Ids.Recipes.MeatTrimmingsCompost, machine, Ids.Products.MeatTrimmings, 12, 4, 60);
      addCompost(Ids.Recipes.MeatTrimmingsCompostT2, machineProto, Ids.Products.MeatTrimmings, 12, 4, 30);

      void addGlassMixing(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Glass mix mixing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Limestone).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Salt).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.GlassMix).BuildAndAdd();
      }

      void addGlassMixingWithAcid(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Glass mix mixing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Limestone).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Salt).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Acid).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.GlassMix).BuildAndAdd();
      }

      void addAcidMixing(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Acid mixing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Sulfur).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.Water).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Acid).BuildAndAdd();
      }

      void addSulfurNeutralization(
        RecipeProto.ID recipeId,
        Duration duration,
        MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Sulfur neutralization", recipeId, machine).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.Sulfur).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Limestone).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(5, Ids.Products.Slag).BuildAndAdd();
      }

      void addBrineProduction(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Brine production", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Salt).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Water).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Brine).BuildAndAdd();
      }

      void addFilterMedia(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Filter media mixing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Coal).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.FilterMedia).BuildAndAdd();
      }

      void addFilterMediaSandM(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Filter media mixing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.ManufacturedSand).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Coal).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.FilterMedia).BuildAndAdd();
      }

      void addFertilizerMixing(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Fertilizer mixing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.Compost).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Water).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.FertilizerOrganic).BuildAndAdd();
      }

      void addFertilizer2(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Fertilizer II synthesis", recipeId, machine).AddInput<RecipeProtoBuilder.State>(10, Ids.Products.FertilizerChemical).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Limestone).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Sulfur).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(14, Ids.Products.FertilizerChemical2).BuildAndAdd();
      }

      void addDirtMixing(RecipeProto.ID recipeId, Duration duration, MachineProto machine)
      {
        registrator.RecipeProtoBuilder.Start("Dirt mixing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Gravel).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Compost).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.Dirt).BuildAndAdd();
      }

      void addAnimalFeed(
        RecipeProto.ID id,
        MachineProto machine,
        ProductProto.ID productIn,
        int quantityIn,
        int feedOut,
        int seconds)
      {
        registrator.RecipeProtoBuilder.Start("Animal feed production", id, machine).AddInput<RecipeProtoBuilder.State>(quantityIn, productIn).SetDurationSeconds(seconds).AddOutput<RecipeProtoBuilder.State>(feedOut, Ids.Products.AnimalFeed).BuildAndAdd();
      }

      void addCompost(
        RecipeProto.ID id,
        MachineProto machine,
        ProductProto.ID productIn,
        int quantityIn,
        int compostOut,
        int seconds)
      {
        registrator.RecipeProtoBuilder.Start("Composing", id, machine).AddInput<RecipeProtoBuilder.State>(quantityIn, productIn).SetDurationSeconds(seconds).AddOutput<RecipeProtoBuilder.State>(compostOut, Ids.Products.Compost).BuildAndAdd();
      }
    }

    public GeneralMixerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
