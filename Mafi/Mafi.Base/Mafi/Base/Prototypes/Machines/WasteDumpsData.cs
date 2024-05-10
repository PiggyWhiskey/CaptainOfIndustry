// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.WasteDumpsData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Machines
{
  internal class WasteDumpsData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      MachineProto.ID wasteDump = Ids.Machines.WasteDump;
      Proto.Str strFormatDesc1 = Proto.CreateStrFormatDesc1((Proto.ID) Ids.Machines.WasteDump, "Liquid dump", "Dumps liquid into the ocean. Some liquid will cause water pollution which can affect health and happiness of your people. Works at the maximum height of {0} from the ocean level.", new LocStrFormatted(11.ToString()), "{0} is an integer specifying max height such as '5'");
      EntityLayout layoutOrThrow = registrator.LayoutParser.ParseLayoutOrThrow(new EntityLayoutParams((Predicate<LayoutTile>) (x => x.Constraint == LayoutTileConstraint.None || x.Constraint.HasAnyConstraints(LayoutTileConstraint.Ocean)), (IEnumerable<CustomLayoutToken>) new CustomLayoutToken[2]
      {
        new CustomLayoutToken("~~~", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(-12, -10, LayoutTileConstraint.Ocean))),
        new CustomLayoutToken("-0}", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightFrom = -h;
          int? nullable = new int?(0);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = new int?();
          int? maxTerrainHeight = nullable;
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightFrom, 2, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      }), "~~~                                                      [2][2]   ", "~~~-1}-1}-1}-1}-1}-2}-2}-2}-2}-2}-2}-2}-2}-5}-5}{2}{2}{2}[2][2]A@<", "~~~                                                      [2][2]   ");
      EntityCosts entityCosts = Costs.Machines.WasteWaterPump.MapToEntityCosts(registrator);
      Electricity zero = Electricity.Zero;
      ImmutableArray<AnimationParams> empty = ImmutableArray<AnimationParams>.Empty;
      ImmutableArray<ImmutableArray<RectangleTerrainArea2iRelative>> reservedOceanAreasSets = ImmutableArray.Create<ImmutableArray<RectangleTerrainArea2iRelative>>(ImmutableArray.Create<RectangleTerrainArea2iRelative>(new RectangleTerrainArea2iRelative(new RelTile2i(-22, -2), new RelTile2i(2, 3))));
      HeightTilesI minGroundHeight = new HeightTilesI(1);
      HeightTilesI maxGroundHeight = new HeightTilesI(30);
      RelTile3f relTile3f = new RelTile3f(-0.25.ToFix32(), -0.125.ToFix32(), (Fix32) 0);
      ImmutableArray<ToolbarCategoryProto> categoriesProtos = registrator.GetCategoriesProtos(Ids.ToolbarCategories.Waste);
      RelTile3f prefabOffset = relTile3f;
      ImmutableArray<ParticlesParams> immutableArray = ImmutableArray.Create<ParticlesParams>(ParticlesParams.Loop("WasteParticles", colorSelector: (Func<RecipeProto, ColorRgba>) (r => r.AllInputs.First.Product.Graphics.Color)));
      Option<string> option = (Option<string>) "Assets/Base/Machines/Water/WasteDump/WasteDump_Sound.prefab";
      Option<string> customIconPath = new Option<string>();
      ImmutableArray<ParticlesParams> particlesParams = immutableArray;
      ImmutableArray<EmissionParams> emissionsParams = new ImmutableArray<EmissionParams>();
      Option<string> machineSoundPrefabPath = option;
      ImmutableArray<string> instancedRenderingExcludedObjects = new ImmutableArray<string>();
      ColorRgba color = new ColorRgba();
      LayoutEntityProto.VisualizedLayers? visualizedLayers = new LayoutEntityProto.VisualizedLayers?();
      MachineProto.Gfx graphics = new MachineProto.Gfx("Assets/Base/Machines/Water/WasteDump.prefab", categoriesProtos, prefabOffset, customIconPath, particlesParams, emissionsParams, machineSoundPrefabPath, useSemiInstancedRendering: true, instancedRenderingExcludedObjects: instancedRenderingExcludedObjects, color: color, visualizedLayers: visualizedLayers);
      int? buffersMultiplier = new int?();
      Option<MachineProto> nextTier = new Option<MachineProto>();
      Computing computingConsumed = new Computing();
      int? emissionWhenRunning = new int?();
      Upoints? boostCost = new Upoints?();
      OceanLiquidDumpProto proto = new OceanLiquidDumpProto(wasteDump, strFormatDesc1, layoutOrThrow, entityCosts, zero, empty, reservedOceanAreasSets, minGroundHeight, maxGroundHeight, graphics, buffersMultiplier, nextTier: nextTier, computingConsumed: computingConsumed, emissionWhenRunning: emissionWhenRunning, isWasteDisposal: true, boostCost: boostCost);
      OceanLiquidDumpProto machine = prototypesDb.Add<OceanLiquidDumpProto>(proto);
      registrator.RecipeProtoBuilder.Start("Water dumping", Ids.Recipes.WaterDumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Water).SetDurationSeconds(2).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Brine dumping", Ids.Recipes.BrineDumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Brine).SetDurationSeconds(2).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Waste water dumping", Ids.Recipes.WasteWaterDumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.WasteWater).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.PollutedWater, "VIRTUAL").SetDurationSeconds(2).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Sour water dumping", Ids.Recipes.SourWaterDumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.SourWater).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.PollutedWater, "VIRTUAL").SetDurationSeconds(2).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Acid dumping", Ids.Recipes.WasteAcidDumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Acid).AddOutput<RecipeProtoBuilder.State>(4, Ids.Products.PollutedWater, "VIRTUAL").SetDurationSeconds(2).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Toxic slurry dumping", Ids.Recipes.ToxicSlurryDumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.ToxicSlurry).AddOutput<RecipeProtoBuilder.State>(5, Ids.Products.PollutedWater, "VIRTUAL").SetDurationSeconds(2).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Seawater dumping", Ids.Recipes.SeaWaterDumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.Seawater).SetDurationSeconds(2).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Fertilizer dumping", Ids.Recipes.FertilizerOrganicDumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(4, Ids.Products.FertilizerOrganic).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.PollutedWater, "VIRTUAL").SetDurationSeconds(4).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Fertilizer dumping", Ids.Recipes.FertilizerChem1Dumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.FertilizerChemical).AddOutput<RecipeProtoBuilder.State>(1, Ids.Products.PollutedWater, "VIRTUAL").SetDurationSeconds(4).EnablePartialExecution(1.Percent()).BuildAndAdd();
      registrator.RecipeProtoBuilder.Start("Fertilizer dumping", Ids.Recipes.FertilizerChem2Dumping, (MachineProto) machine).SetProductsDestroyReason(DestroyReason.DumpedOnTerrain).AddInput<RecipeProtoBuilder.State>(2, Ids.Products.FertilizerChemical2).AddOutput<RecipeProtoBuilder.State>(2, Ids.Products.PollutedWater, "VIRTUAL").SetDurationSeconds(4).EnablePartialExecution(1.Percent()).BuildAndAdd();
    }

    public WasteDumpsData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
