// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.FurnacesData
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
  internal class FurnacesData : IModData
  {
    public const int METAL_SCRAPS_PER_BATCH = 8;

    public void RegisterData(ProtoRegistrator registrator)
    {
      Duration totalDuration1 = Duration.FromKeyframes(360);
      Duration totalDuration2 = Duration.FromKeyframes(450);
      Duration pauseAt = Duration.FromKeyframes(280);
      MachineProto arcFurnace2 = registrator.MachineProtoBuilder.Start("Arc furnace II", Ids.Machines.ArcFurnace2).Description("This furnace has a cooling system to safely reach higher operating temperatures. This provides increased throughput and an opportunity to reuse some of the excess heat. Power requirements are increased as well.", "short description of a machine").SetCost(Costs.Machines.ArcFurnace2).SetElectricityConsumption(5500.Kw()).SetComputingConsumption(Computing.FromTFlops(4)).DisableBoost().SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("C#>[6][6][6][6][6][6][6][6][6]>~Y", "   [2][7][7][6][6][6][6][4][3]>@Z", "A~>[3][7][7][6][7][7][6][4][3]>'V", "B~>[3][7][7][6][7][7][6][4][3]>'W", "   [2][7][7][6][6][6][6][6][3]   ", "D@>[6][6][6][6][6][6][6][6][6]>@E").SetPrefabPath("Assets/Base/Machines/MetalWorks/ArcFurnace/ArcFurnaceT2.prefab").AddEmissionParams(EmissionParams.Timed(ImmutableArray.Create<string>("electrode"), "Rods", pauseAt - 2.Seconds(), 5.Seconds(), 5f, 0.12f, 0.05f)).AddEmissionParams(EmissionParams.AllTime(ImmutableArray.Create<string>("Object116", "liquid-B", "liquidA"), "ArcFurnace", 2.5f, color: new ColorRgba?((ColorRgba) 16545655))).SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration2, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt)).EnableSemiInstancedRendering().BuildAndAdd();
      MachineProto arcFurnace = registrator.MachineProtoBuilder.Start("Arc furnace", Ids.Machines.ArcFurnace).Description("Melts metals via a powerful electric arc. The arc is deployed using graphite anodes that are partially spent during the process due to the high heat. Beware that the furnace consumes a significant amount of power. It would be polite to notify your local power plant before turning this on.", "short description of a machine").SetCost(Costs.Machines.ArcFurnace).SetNextTier(arcFurnace2).SetElectricityConsumption(3000.Kw()).DisableBoost().SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("C#>[6][6][6][6][6][6][6][6][6]>~Y", "   [2][7][7][6][6][6][6][4][3]   ", "A~>[3][7][7][6][7][7][6][4][3]>'V", "B~>[3][7][7][6][7][7][6][4][3]>'W", "   [2][7][7][6][6][6][6][6][3]   ", "   [6][6][6][6][6][6][6][6][6]>@E").SetPrefabPath("Assets/Base/Machines/MetalWorks/ArcFurnace/ArcFurnaceT1.prefab").AddEmissionParams(EmissionParams.Timed(ImmutableArray.Create<string>("electrode"), "Rods", pauseAt - 2.Seconds(), 5.Seconds(), 4f, 0.12f, 0.05f)).AddEmissionParams(EmissionParams.AllTime(ImmutableArray.Create<string>("Object116", "liquid-B", "liquidA"), "ArcFurnace", 2.5f, color: new ColorRgba?((ColorRgba) 16545655))).SetAnimationParams((AnimationParams) AnimationParams.PlayOnceAndPauseAt(totalDuration2, AnimationWithPauseParams.Mode.ExtendPauseToFit, pauseAt)).EnableSemiInstancedRendering().BuildAndAdd();
      string description = "Smelts various materials such as iron or copper. The output is a molten material that needs to be sent via molten channel transport, for instance, into a caster for further processing. Molten materials cannot be transported via trucks.";
      MachineProto furnace2 = registrator.MachineProtoBuilder.Start("Blast furnace II", Ids.Machines.SmeltingFurnaceT2).Description(description, "short description of a machine").SetCost(Costs.Machines.SmeltingFurnaceT2).SetNextTier(arcFurnace2).SetLayout("   [2][2][2][4][4][4][4][3][2]>~Y", "A~>[2][3][3][4][9][9][9][9][3]   ", "B~>[2][3][5][7][9][9][9][9][3]>'V", "C~>[2][3][5][7][9][9][9][9][3]>'W", "   [2][2][2][4][4][9][9][9][3]   ", "   [2][2][2][3][3][5][4][4][3]>@E").SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetPrefabPath("Assets/Base/Machines/MetalWorks/BlastFurnaceT2.prefab").SetMachineSound("Assets/Base/Machines/MetalWorks/BlastFurnaceT1/BlastFurnace_Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatTimes(totalDuration1, 2, true)).BuildAndAdd();
      MachineProto furnace1 = registrator.MachineProtoBuilder.Start("Blast furnace", Ids.Machines.SmeltingFurnaceT1).Description(description, "short description of a machine").SetCost(Costs.Machines.SmeltingFurnaceT1).SetNextTier(furnace2).SetLayout("   [2][2][2][3][3][3][3][3][2]>~Y", "   [2][2][3][5][5][7][7][4][3]   ", "A~>[2][2][3][5][5][7][7][4][3]>'V", "B~>[2][2][3][5][5][7][7][4][3]>'W", "   [2][2][2][3][3][7][7][4][3]   ", "   [2][2][2][2][2][2][2][2][3]>@E").SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetPrefabPath("Assets/Base/Machines/MetalWorks/BlastFurnaceT1.prefab").SetMachineSound("Assets/Base/Machines/MetalWorks/BlastFurnaceT1/BlastFurnace_Sound.prefab").SetAnimationParams((AnimationParams) AnimationParams.RepeatTimes(totalDuration1, 2, true)).BuildAndAdd();
      registerIron();
      registerCopper();
      registerGlass();
      registerSilicon();

      void registerIron()
      {
        Duration durationTicks1 = 20.Seconds();
        Duration durationTicks2 = 20.Seconds();
        registrator.RecipeProtoBuilder.Start("Iron scrap smelting", Ids.Recipes.IronSmeltingT1Scrap, furnace1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.IronScrap).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Coal).SetDuration(durationTicks1).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenIron).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Iron smelting", Ids.Recipes.IronSmeltingT1Coal, furnace1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.IronOre).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Coal).SetDuration(durationTicks1).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenIron).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Slag).AddHalfExhaust<RecipeProtoBuilder.State>().BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Iron scrap smelting", Ids.Recipes.IronSmeltingT2Scrap, furnace2).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.IronScrap).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Coal).SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenIron).AddOutput<RecipeProtoBuilder.State>(10, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Iron smelting (lime)", Ids.Recipes.IronSmeltingT2, furnace2).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.IronOreCrushed).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Limestone).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Coal).SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenIron).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Iron scrap smelting (arc)", Ids.Recipes.IronSmeltingArcScrap, arcFurnace2).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.IronScrap).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water, "D").SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenIron).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SteamLo, "Z").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Exhaust, "E").BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Iron smelting (arc)", Ids.Recipes.IronSmeltingArc, arcFurnace2).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.IronOreCrushed).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Limestone).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water, "D").SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenIron).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SteamLo, "Z").AddQuarterExhaust<RecipeProtoBuilder.State>("E").BuildAndAdd();
      }

      void registerCopper()
      {
        Duration durationTicks1 = 20.Seconds();
        Duration durationTicks2 = 20.Seconds();
        registrator.RecipeProtoBuilder.Start("Copper scrap smelting", Ids.Recipes.CopperSmeltingT1Scrap, furnace1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.CopperScrap).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Coal).SetDuration(durationTicks1).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenCopper).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Copper smelting", Ids.Recipes.CopperSmeltingT1, furnace1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.CopperOre).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Coal).SetDuration(durationTicks1).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenCopper).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Slag).AddHalfExhaust<RecipeProtoBuilder.State>().BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Copper scrap smelting", Ids.Recipes.CopperSmeltingT2Scrap, furnace2).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.CopperScrap).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Coal).SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenCopper).AddOutput<RecipeProtoBuilder.State>(10, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Copper smelting (sand)", Ids.Recipes.CopperSmeltingT2, furnace2).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.CopperOreCrushed).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Coal).SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenCopper).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Copper scrap smelting (arc)", Ids.Recipes.CopperSmeltingArcScrap, arcFurnace2).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.CopperScrap).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water, "D").SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenCopper).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SteamLo, "Z").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Exhaust, "E").BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Copper smelting (arc)", Ids.Recipes.CopperSmeltingArc, arcFurnace2).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.CopperOreCrushed).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water, "D").SetDuration(durationTicks2).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenCopper).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SteamLo, "Z").AddQuarterExhaust<RecipeProtoBuilder.State>("E").BuildAndAdd();
      }

      void registerGlass()
      {
        Duration durationTicks = 20.Seconds();
        registrator.RecipeProtoBuilder.Start("Glass smelting", Ids.Recipes.GlassSmelting, furnace1).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.GlassMix).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Coal).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenGlass).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Glass broken smelting", Ids.Recipes.GlassSmeltingWithBroken, furnace1).AddInput<RecipeProtoBuilder.State>(12, Ids.Products.BrokenGlass).AddInput<RecipeProtoBuilder.State>(3, Ids.Products.Coal).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenGlass).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Glass smelting", Ids.Recipes.GlassSmeltingT2, furnace2).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.GlassMix).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Coal).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenGlass).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Glass broken smelting", Ids.Recipes.GlassSmeltingT2WithBroken, furnace2).AddInput<RecipeProtoBuilder.State>(24, Ids.Products.BrokenGlass).AddInput<RecipeProtoBuilder.State>(5, Ids.Products.Coal).SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenGlass).AddOutput<RecipeProtoBuilder.State>(10, Ids.Products.Exhaust).BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Glass smelting", Ids.Recipes.GlassSmeltingArc, arcFurnace2).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.GlassMix).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water, "D").SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenGlass).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SteamLo, "Z").AddQuarterExhaust<RecipeProtoBuilder.State>("E").BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Glass smelting", Ids.Recipes.GlassSmeltingArcWithBroken, arcFurnace2).AddInput<RecipeProtoBuilder.State>(24, Ids.Products.BrokenGlass).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water, "D").SetDuration(durationTicks).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenGlass).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SteamLo, "Z").AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.Exhaust, "E").BuildAndAdd();
      }

      void registerSilicon()
      {
        registrator.RecipeProtoBuilder.Start("Silicon smelting (Arc)", Ids.Recipes.SiliconSmeltingArc, arcFurnace).AddInput<RecipeProtoBuilder.State>(8, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Coal).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(8, Ids.Products.MoltenSilicon).AddOutput<RecipeProtoBuilder.State>(3, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Exhaust, "E").BuildAndAdd();
        registrator.RecipeProtoBuilder.Start("Silicon smelting (Arc II)", Ids.Recipes.SiliconSmeltingArc2, arcFurnace2).AddInput<RecipeProtoBuilder.State>(16, Ids.Products.Sand).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Coal).AddInput<RecipeProtoBuilder.State>(1, Ids.Products.Graphite).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water, "D").SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(16, Ids.Products.MoltenSilicon).AddOutput<RecipeProtoBuilder.State>(6, Ids.Products.Slag).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.SteamLo, "Z").AddOutput<RecipeProtoBuilder.State>(12, Ids.Products.Exhaust, "E").BuildAndAdd();
      }
    }

    public FurnacesData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
