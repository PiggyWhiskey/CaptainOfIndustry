// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.CrusherData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class CrusherData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto machine1 = registrator.MachineProtoBuilder.Start("Crusher", Ids.Machines.Crusher).Description("Crushes ores into more fine grained materials so they can be used in advanced processing.", "short description of a machine").SetCost(Costs.Machines.Crusher).SetElectricityConsumption(300.Kw()).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout(new EntityLayoutParams(customPortHeights: (Option<IEnumerable<KeyValuePair<char, int>>>) (IEnumerable<KeyValuePair<char, int>>) new Lyst<KeyValuePair<char, int>>()
      {
        Make.Kvp<char, int>('A', 3)
      }), "   [3][4][3][3][3][3]   ", "A~>[3][4][3][3][3][3]>~X", "   [3][4][3][3][3][3]   ", "   [2][3][2][2]         ").SetPrefabPath("Assets/Base/Machines/MetalWorks/CrusherT1.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).SetMachineSound("Assets/Base/Machines/MetalWorks/CrusherT1/Crusher_Sound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      MachineProto machine2 = registrator.MachineProtoBuilder.Start("Crusher (large)", Ids.Machines.CrusherLarge).Description("Large crusher for better efficiency and increased throughput.", "short description of a machine").SetCost(Costs.Machines.CrusherLarge).SetElectricityConsumption(1200.Kw()).SetCategories(Ids.ToolbarCategories.MachinesMetallurgy).SetLayout("   [3][3][3][3][3][3][3]                           ", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][4][4][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][4][4][3][3][3][3][3][3][3][3][3]   ", "A~>[3][3][3][3][4][4][4][4][4][4][4][4][4][3][3]>~X", "   [3][3][3][3][4][4][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][4][4][3][3][3][3][3][3][3][3][3]   ", "   [3][3][3][3][3][3][3][3][3][3][3][3][3][3][3]   ").SetPrefabPath("Assets/Base/Machines/MetalWorks/CrusherT2.prefab").SetAnimationParams((AnimationParams) AnimationParams.Loop()).SetMachineSound("Assets/Base/Machines/MetalWorks/CrusherT1/Crusher_Sound.prefab").EnableSemiInstancedRendering().BuildAndAdd();
      Duration crushingDuration = 20.Seconds();
      int mult = 6;
      registerIronOreCrushing(machine1, Ids.Recipes.IronOreCrushing);
      registerIronOreCrushing(machine2, Ids.Recipes.IronOreCrushingT2, mult);
      registerCopperOreCrushing(machine1, Ids.Recipes.CopperOreCrushing);
      registerCopperOreCrushing(machine2, Ids.Recipes.CopperOreCrushingT2, mult);
      registerSlagCrushing(machine1, Ids.Recipes.SlagCrushing);
      registerSlagCrushing(machine2, Ids.Recipes.SlagCrushingT2, mult);
      registerRockCrushing(machine1, Ids.Recipes.RockCrushing);
      registerRockCrushing(machine2, Ids.Recipes.RockCrushingT2, mult);
      registerGravelCrushing(machine1, Ids.Recipes.GravelCrushing);
      registerGravelCrushing(machine2, Ids.Recipes.GravelCrushingT2, mult);
      registerQuartzCrushing(machine1, Ids.Recipes.QuartzCrushing);
      registerQuartzCrushing(machine2, Ids.Recipes.QuartzCrushingT2, mult);
      registerQuartzMilling(machine1, Ids.Recipes.QuartzMilling);
      registerQuartzMilling(machine2, Ids.Recipes.QuartzMillingT2, mult);
      registerUraniumCrushing(machine1, Ids.Recipes.UraniumCrushing);
      registerUraniumCrushing(machine2, Ids.Recipes.UraniumCrushingT2, mult);
      registerGoldOreCrushing(machine1, Ids.Recipes.GoldOreCrushing);
      registerGoldOreCrushing(machine2, Ids.Recipes.GoldOreCrushingT2, mult);
      registerGoldOreMilling(machine1, Ids.Recipes.GoldOreMilling);
      registerGoldOreMilling(machine2, Ids.Recipes.GoldOreMillingT2, mult);

      void registerIronOreCrushing(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Iron ore crushing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(12 * mult, Ids.Products.IronOre).SetDuration(crushingDuration).AddOutput<RecipeProtoBuilder.State>(12 * mult, Ids.Products.IronOreCrushed).BuildAndAdd();
      }

      void registerCopperOreCrushing(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Copper ore crushing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(12 * mult, Ids.Products.CopperOre).SetDuration(crushingDuration).AddOutput<RecipeProtoBuilder.State>(12 * mult, Ids.Products.CopperOreCrushed).BuildAndAdd();
      }

      void registerSlagCrushing(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Slag crushing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.Slag).SetDuration(crushingDuration).AddOutput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.SlagCrushed).BuildAndAdd();
      }

      void registerRockCrushing(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Rock crushing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.Rock).SetDuration(crushingDuration).AddOutput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.Gravel).BuildAndAdd();
      }

      void registerGravelCrushing(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Gravel milling", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.Gravel).SetDuration(3 * crushingDuration).AddOutput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.ManufacturedSand).BuildAndAdd();
      }

      void registerQuartzCrushing(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Quartz crushing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.Quartz).SetDuration(crushingDuration).AddOutput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.QuartzCrushed).BuildAndAdd();
      }

      void registerQuartzMilling(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Quartz milling", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.QuartzCrushed).SetDuration(3 * crushingDuration).AddOutput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.Sand).BuildAndAdd();
      }

      void registerUraniumCrushing(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Uranium crushing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(12 * mult, Ids.Products.UraniumOre).SetDuration(40.Seconds()).AddOutput<RecipeProtoBuilder.State>(12 * mult, Ids.Products.UraniumOreCrushed).BuildAndAdd();
      }

      void registerGoldOreCrushing(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Gold ore crushing", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.GoldOre).SetDuration(20.Seconds()).AddOutput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.GoldOreCrushed).BuildAndAdd();
      }

      void registerGoldOreMilling(MachineProto machine, RecipeProto.ID recipeId, int mult = 1)
      {
        registrator.RecipeProtoBuilder.Start("Gold ore milling", recipeId, machine).AddInput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.GoldOreCrushed).SetDuration(40.Seconds()).AddOutput<RecipeProtoBuilder.State>(8 * mult, Ids.Products.GoldOrePowder).BuildAndAdd();
      }
    }

    public CrusherData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
