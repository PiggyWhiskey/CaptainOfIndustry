// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.SmokeStackData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class SmokeStackData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      LocStr desc = Loc.Str(Ids.Machines.SmokeStack.ToString() + "__desc", "Allows release of gasses into atmosphere. Some gasses such as exhaust cause pollution.", "description of a smoke stack");
      registrator.MachineProtoBuilder.Start("Smoke stack (large)", Ids.Machines.SmokeStackLarge).Description(desc).SetCost(Costs.Machines.SmokeStackLarge).SetCategories(Ids.ToolbarCategories.Waste).SetLayout(new EntityLayoutParams(customTokens: (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[1]
      {
        new CustomLayoutToken("[0!", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = 2 * h;
          int? terrainSurfaceHeight = new int?(0);
          Proto.ID? nullable = new Proto.ID?(p.HardenedFloorSurfaceId);
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = new int?();
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = nullable;
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "   v@Cv@D   ", "B@>[7![7!<@E", "A@>[7![7!<@F", "   ^@H^@G   ").SetPrefabPath("Assets/Base/Machines/Waste/SmokeStackLarge.prefab").SetAsWasteDisposal().DisableLogisticsByDefault().UseAllRecipesAtStartOrAfterUnlock().AddParticleParams(ParticlesParams.Loop("SmokeDark", recipesSelector: new Func<RecipeProto, bool>(isDarkSmokeLarge))).AddParticleParams(ParticlesParams.Loop("SmokeWhite", recipesSelector: new Func<RecipeProto, bool>(isWhiteSmokeLarge))).AddParticleParams(ParticlesParams.Loop("SteamPress", recipesSelector: new Func<RecipeProto, bool>(isSteamPressLarge))).AddParticleParams(ParticlesParams.Loop("Steam", recipesSelector: (Func<RecipeProto, bool>) (r => !isDarkSmokeLarge(r) && !isWhiteSmokeLarge(r) && !isSteamPressLarge(r)))).EnableSemiInstancedRendering().BuildAndAdd();
      registrator.MachineProtoBuilder.Start("Smoke stack", Ids.Machines.SmokeStack).Description(desc).SetCost(Costs.Machines.SmokeStack).SetCategories(Ids.ToolbarCategories.Waste).SetLayout("A@>[8]").SetPrefabPath("Assets/Base/Machines/Waste/SmokeStack.prefab").SetAsWasteDisposal().DisableLogisticsByDefault().UseAllRecipesAtStartOrAfterUnlock().AddParticleParams(ParticlesParams.Loop("SmokeDark", recipesSelector: new Func<RecipeProto, bool>(isDarkSmoke))).AddParticleParams(ParticlesParams.Loop("SmokeWhite", recipesSelector: new Func<RecipeProto, bool>(isWhiteSmoke))).AddParticleParams(ParticlesParams.Loop("SteamPress", recipesSelector: new Func<RecipeProto, bool>(isSteamPress))).AddParticleParams(ParticlesParams.Loop("Steam", recipesSelector: (Func<RecipeProto, bool>) (r => !isDarkSmoke(r) && !isWhiteSmoke(r) && !isSteamPress(r)))).EnableSemiInstancedRendering().BuildAndAdd();
      Duration duration = 20.Seconds();
      Quantity quantity = 20.Quantity();
      this.addDisposalRecipe(registrator, Ids.Recipes.SmokeStackDepletedSteam, Ids.Recipes.SmokeStackLargeDepletedSteam, Ids.Products.SteamDepleted, quantity, duration);
      this.addDisposalRecipe(registrator, Ids.Recipes.SmokeStackLpSteam, Ids.Recipes.SmokeStackLargeLpSteam, Ids.Products.SteamLo, quantity, duration);
      this.addDisposalRecipe(registrator, Ids.Recipes.SmokeStackHpSteam, Ids.Recipes.SmokeStackLargeHpSteam, Ids.Products.SteamHi, quantity, duration);
      this.addDisposalRecipe(registrator, Ids.Recipes.SmokeStackSpSteam, Ids.Recipes.SmokeStackLargeSpSteam, Ids.Products.SteamSp, quantity, duration);
      this.addDisposalRecipe(registrator, Ids.Recipes.SmokeStackOxygen, Ids.Recipes.SmokeStackLargeOxygen, Ids.Products.Oxygen, quantity, duration);
      this.addDisposalRecipe(registrator, Ids.Recipes.SmokeStackNitrogen, Ids.Recipes.SmokeStackLargeNitrogen, Ids.Products.Nitrogen, quantity, duration);
      this.addDisposalRecipe(registrator, Ids.Recipes.SmokeStackExhaust, Ids.Recipes.SmokeStackLargeExhaust, Ids.Products.Exhaust, quantity, duration, new Quantity?(quantity / 2));
      this.addDisposalRecipe(registrator, Ids.Recipes.SmokeStackCarbonDioxide, Ids.Recipes.SmokeStackLargeCarbonDioxide, Ids.Products.CarbonDioxide, quantity, duration, new Quantity?(quantity / 4));

      static bool isDarkSmokeLarge(RecipeProto r) => r.Id == Ids.Recipes.SmokeStackLargeExhaust;

      static bool isWhiteSmokeLarge(RecipeProto r)
      {
        return r.Id == Ids.Recipes.SmokeStackLargeCarbonDioxide;
      }

      static bool isSteamPressLarge(RecipeProto r)
      {
        return r.Id == Ids.Recipes.SmokeStackLargeHpSteam || r.Id == Ids.Recipes.SmokeStackLargeSpSteam;
      }

      static bool isDarkSmoke(RecipeProto r) => r.Id == Ids.Recipes.SmokeStackExhaust;

      static bool isWhiteSmoke(RecipeProto r) => r.Id == Ids.Recipes.SmokeStackCarbonDioxide;

      static bool isSteamPress(RecipeProto r)
      {
        return r.Id == Ids.Recipes.SmokeStackHpSteam || r.Id == Ids.Recipes.SmokeStackSpSteam;
      }
    }

    private void addDisposalRecipe(
      ProtoRegistrator registrator,
      RecipeProto.ID recipeId,
      RecipeProto.ID recipeLargeId,
      ProductProto.ID productId,
      Quantity quantity,
      Duration duration,
      Quantity? producePollutedAir = null)
    {
      RecipeProtoBuilder.State builder1 = registrator.RecipeProtoBuilder.Start("Product disposal", recipeId, Ids.Machines.SmokeStack).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).SetDuration(duration).AddInput<RecipeProtoBuilder.State>("*", productId, quantity).EnablePartialExecution(25.Percent());
      if (producePollutedAir.HasValue)
        builder1 = builder1.AddOutput<RecipeProtoBuilder.State>("VIRTUAL", Ids.Products.PollutedAir, producePollutedAir.Value);
      builder1.BuildAndAdd();
      int num = 10;
      RecipeProtoBuilder.State builder2 = registrator.RecipeProtoBuilder.Start("Product disposal", recipeLargeId, Ids.Machines.SmokeStackLarge).SetDuration(duration).AddInput<RecipeProtoBuilder.State>("*", productId, num * quantity).EnablePartialExecution(25.Percent());
      if (producePollutedAir.HasValue)
        builder2 = builder2.AddOutput<RecipeProtoBuilder.State>("VIRTUAL", Ids.Products.PollutedAir, num * producePollutedAir.Value);
      builder2.BuildAndAdd();
    }

    public SmokeStackData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
