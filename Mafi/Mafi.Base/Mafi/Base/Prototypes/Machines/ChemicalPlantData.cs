// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.ChemicalPlantData
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

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class ChemicalPlantData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      LocStr desc = Loc.Str(Ids.Machines.ChemicalPlant.ToString() + "__desc", "Performs variety of chemical recipes including processing of fluids and their packaging.", "description of a machine");
      MachineProto machineProto = registrator.MachineProtoBuilder.Start("Chemical plant II", Ids.Machines.ChemicalPlant2).Description(desc).SetCost(Costs.Machines.ChemicalPlant2).SetElectricityConsumption(400.Kw()).SetCategories(Ids.ToolbarCategories.MachinesOil).SetLayout("~E>[7][8][7][6][5][5][5]   ", "~F>[7][7][7][6][5][5][5]   ", "#D>[6][6][6][6][5][5][5]>X@", "@A>[5][5][5][5][5][5][5]>Y#", "@B>[5][5][5][5][5][5][5]>Z@", "@C>[5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5]   ").SetPrefabPath("Assets/Base/Machines/Oil/ReformerT2.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).AddParticleParams(ParticlesParams.Loop("Steam")).EnableSemiInstancedRendering().BuildAndAdd();
      MachineProto machine = registrator.MachineProtoBuilder.Start("Chemical plant", Ids.Machines.ChemicalPlant).Description(desc).SetCost(Costs.Machines.ChemicalPlant).SetElectricityConsumption(250.Kw()).SetCategories(Ids.ToolbarCategories.MachinesOil).SetNextTier(machineProto).SetLayout("~E>[7][8][7][6][5][5][5]   ", "~F>[7][7][7][6][5][5][5]   ", "#D>[6][6][6][6][5][5][5]>X@", "@A>[5][5][5][5][5][5][5]>Y#", "@B>[5][5][5][5][5][5][5]>Z@", "@C>[5][5][5][5][5][5][5]   ", "   [5][5][5][5][5][5][5]   ").SetPrefabPath("Assets/Base/Machines/Oil/ReformerT1.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).AddParticleParams(ParticlesParams.Loop("Steam")).EnableSemiInstancedRendering().BuildAndAdd();
      registerFertilizerSynthesis(Ids.Recipes.FertilizerProduction, machine, 20.Seconds());
      registerFertilizerSynthesis(Ids.Recipes.FertilizerProductionT2, machineProto, 10.Seconds());
      registerAmmoniaSynthesis(Ids.Recipes.AmmoniaSynthesis, machine, 40.Seconds());
      registerAmmoniaSynthesis(Ids.Recipes.AmmoniaSynthesisT2, machineProto, 20.Seconds());
      registerPaperProduction(Ids.Recipes.PaperProduction, machine, 40.Seconds());
      registerPaperProduction(Ids.Recipes.PaperProductionT2, machineProto, 20.Seconds());
      registerEthanolFuel(Ids.Recipes.EthanolCookingOilReforming, machine, 2 * OilDistillationData.DURATION);
      registerEthanolFuel(Ids.Recipes.EthanolCookingOilReformingT2, machineProto, OilDistillationData.DURATION);
      registrator.RecipeProtoBuilder.Start("FuelGas synthesis", Ids.Recipes.FuelGasSynthesis, machineProto).AddInput<RecipeProtoBuilder.State>(14, Ids.Products.Hydrogen, "A").AddInput<RecipeProtoBuilder.State>(12, Ids.Products.CarbonDioxide, "B").SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.FuelGas, "X").AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.Water, "Z").BuildAndAdd();
      registerGraphiteProduction("Graphite production", Ids.Recipes.GraphiteProductionT1, machine, 40.Seconds());
      registerGraphiteProduction("Graphite production", Ids.Recipes.GraphiteProductionT2, machineProto, 20.Seconds());
      registrator.RecipeProtoBuilder.Start("Graphite production", Ids.Recipes.GraphiteProductionCo2, machineProto).AddInput<RecipeProtoBuilder.State>(24, Ids.Products.CarbonDioxide).SetDurationSeconds(10).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).BuildAndAdd();
      registerDisinfectant(Ids.Recipes.DisinfectantProduction, machine, 40.Seconds());
      registerDisinfectant(Ids.Recipes.DisinfectantProductionT2, machineProto, 20.Seconds());
      registrator.RecipeProtoBuilder.Start("Anesthetics production", Ids.Recipes.AnestheticsProduction, machineProto).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Ammonia).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.HydrogenFluoride).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Steel).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Anesthetics).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Morphine production", Ids.Recipes.MorphineProduction, machineProto).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Poppy).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Acid).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Glass).SetDurationSeconds(20).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Morphine).BuildAndAdd();
      registerMoxProduction("Mox rods", Ids.Recipes.MoxRodsAssemblyT1, machineProto, 90.Seconds());
      registerBlanketFuelFromDepleted("Blanket fuel production", Ids.Recipes.BlanketFuelFromDepleted, machineProto, 20.Seconds());
      registerBlanketFuelFromYellowcake("Blanket fuel production", Ids.Recipes.BlanketFuelFromYellowcake, machineProto, 20.Seconds());
      registerCoreFromEnriched("Core fuel production", Ids.Recipes.CoreFuelFromEnriched, machineProto, 20.Seconds());

      void registerFertilizerSynthesis(
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start("Fertilizer synthesis", recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Ammonia, "A").AddInput<RecipeProtoBuilder.State>(6, Ids.Products.Oxygen, "B").SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(10, Ids.Products.FertilizerChemical).BuildAndAdd();
      }

      void registerAmmoniaSynthesis(
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start("Ammonia synthesis", recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Hydrogen, "A").AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Nitrogen, "B").SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Ammonia).BuildAndAdd();
      }

      void registerPaperProduction(
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start("Paper production", recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Woodchips).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Limestone).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.SteamHi).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Paper).BuildAndAdd();
      }

      void registerEthanolFuel(RecipeProto.ID recipeId, MachineProto machine, Duration duration)
      {
        registrator.RecipeProtoBuilder.Start("Fuel transesterification", recipeId, machine).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Ethanol).AddInput<RecipeProtoBuilder.State>(9, Ids.Products.CookingOil).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(18, Ids.Products.Diesel, "X").BuildAndAdd();
      }

      void registerGraphiteProduction(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.Coal).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Chlorine).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Graphite).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.SourWater).BuildAndAdd();
      }

      void registerDisinfectant(RecipeProto.ID recipeId, MachineProto machine, Duration duration)
      {
        registrator.RecipeProtoBuilder.Start("Disinfectant production", recipeId, machine).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Ethanol).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Plastic).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Disinfectant).BuildAndAdd();
      }

      void registerMoxProduction(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Plutonium).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.UraniumDepleted).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.MoxRod).BuildAndAdd();
      }

      void registerBlanketFuelFromDepleted(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.UraniumDepleted).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Salt).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.BlanketFuel, "X").BuildAndAdd();
      }

      void registerBlanketFuelFromYellowcake(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Yellowcake).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Salt).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.BlanketFuel, "X").BuildAndAdd();
      }

      void registerCoreFromEnriched(
        string name,
        RecipeProto.ID recipeId,
        MachineProto machine,
        Duration duration)
      {
        registrator.RecipeProtoBuilder.Start(name, recipeId, machine).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.UraniumEnriched20).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Salt).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.CoreFuel, "Z").BuildAndAdd();
      }
    }

    public ChemicalPlantData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
