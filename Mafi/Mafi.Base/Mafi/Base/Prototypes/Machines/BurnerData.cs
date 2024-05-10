// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.BurnerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using System;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class BurnerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      MachineProto burner = registrator.MachineProtoBuilder.Start("Burner (solid)", Ids.Machines.SolidBurner).Description("Disposes solid waste by burning it", "short description of a machine").SetCost(Costs.Machines.SolidBurner).SetCategories(Ids.ToolbarCategories.Waste).SetAsWasteDisposal().SetLayout("A~>[3][9][9]", "B#>[3][9][9]").SetPrefabPath("Assets/Base/Machines/Waste/BurnerSolid.prefab").SetEmissionWhenWorking(3).AddParticleParams(ParticlesParams.Loop("SmokeDark", recipesSelector: new Func<RecipeProto, bool>(isDarkSmoke))).AddParticleParams(ParticlesParams.Loop("SmokeBlue", recipesSelector: new Func<RecipeProto, bool>(isBlueSmoke))).AddParticleParams(ParticlesParams.Loop("SmokeGray", recipesSelector: (Func<RecipeProto, bool>) (r => !isDarkSmoke(r) && !isBlueSmoke(r)))).EnableSemiInstancedRendering().BuildAndAdd();
      addBurningRecipe(Ids.Recipes.LandfillBurning, Ids.Products.Waste, 6, 20.Seconds(), 3);
      addBurningRecipe(Ids.Recipes.BiomassBurning, Ids.Products.Biomass, 6, 10.Seconds(), 2);
      addBurningRecipe(Ids.Recipes.AnimalFeedBurning, Ids.Products.AnimalFeed, 6, 10.Seconds(), 2);
      addBurningRecipe(Ids.Recipes.MeatTrimmingsBurning, Ids.Products.MeatTrimmings, 4, 20.Seconds(), 3);
      addBurningRecipe(Ids.Recipes.ChickenCarcassBurning, Ids.Products.ChickenCarcass, 4, 20.Seconds(), 3);
      addBurningRecipe(Ids.Recipes.SulfurBurning, Ids.Products.Sulfur, 2, 10.Seconds(), 8);
      addBurningRecipe(Ids.Recipes.SludgeBurning, Ids.Products.Sludge, 6, 20.Seconds(), 6);

      static bool isDarkSmoke(RecipeProto r)
      {
        return r.Id == Ids.Recipes.LandfillBurning || r.Id == Ids.Recipes.SludgeBurning;
      }

      static bool isBlueSmoke(RecipeProto r) => r.Id == Ids.Recipes.SulfurBurning;

      void addBurningRecipe(
        RecipeProto.ID id,
        ProductProto.ID productToBurn,
        int quantity,
        Duration duration,
        int pollution)
      {
        registrator.RecipeProtoBuilder.Start("Burning", id, burner).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(quantity, productToBurn).SetDuration(duration).AddOutput<RecipeProtoBuilder.State>("VIRTUAL", Ids.Products.PollutedAir, pollution.Quantity()).BuildAndAdd();
      }
    }

    public BurnerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
