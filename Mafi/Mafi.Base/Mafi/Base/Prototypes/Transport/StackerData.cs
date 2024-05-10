// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Transport.StackerData
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Prototypes.Transport
{
  internal class StackerData : IModData
  {
    public void RegisterData(ProtoRegistrator registrator)
    {
      ProtosDb prototypesDb = registrator.PrototypesDb;
      CustomLayoutToken[] customTokens = new CustomLayoutToken[9]
      {
        new CustomLayoutToken("(0A", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(h - 2, h))),
        new CustomLayoutToken("(0B", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(h - 3, h))),
        new CustomLayoutToken("(0C", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(h - 4, h))),
        new CustomLayoutToken("(0D", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(h - 5, h))),
        new CustomLayoutToken("(0E", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(h - 6, h))),
        new CustomLayoutToken("(0G", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(heightToExcl: h))),
        new CustomLayoutToken("10A", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(h + 10 - 2, h + 10))),
        new CustomLayoutToken("10B", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) => new LayoutTokenSpec(h + 10 - 3, h + 10))),
        new CustomLayoutToken("(0X", (Func<EntityLayoutParams, int, LayoutTokenSpec>) ((p, h) =>
        {
          int heightToExcl = h;
          int? nullable1 = new int?(-4);
          int? nullable2 = new int?(4);
          int? terrainSurfaceHeight = new int?();
          int? minTerrainHeight = nullable1;
          int? maxTerrainHeight = nullable2;
          Fix32? vehicleHeight = new Fix32?();
          Proto.ID? terrainMaterialId = new Proto.ID?();
          Proto.ID? surfaceId = new Proto.ID?();
          return new LayoutTokenSpec(heightToExcl: heightToExcl, terrainSurfaceHeight: terrainSurfaceHeight, minTerrainHeight: minTerrainHeight, maxTerrainHeight: maxTerrainHeight, vehicleHeight: vehicleHeight, terrainMaterialId: terrainMaterialId, surfaceId: surfaceId);
        }))
      };
      int? nullable = new int?(2);
      Proto.ID? hardenedFloorSurfaceId = new Proto.ID?();
      int? customCollapseVerticesThreshold = nullable;
      ThicknessIRange? customPlacementRange = new ThicknessIRange?();
      Option<IEnumerable<KeyValuePair<char, int>>> customPortHeights = new Option<IEnumerable<KeyValuePair<char, int>>>();
      EntityLayoutParams layoutParams = new EntityLayoutParams((Predicate<LayoutTile>) (x => x.TileSurfaceProto.IsNone), (IEnumerable<CustomLayoutToken>) customTokens, hardenedFloorSurfaceId: hardenedFloorSurfaceId, customCollapseVerticesThreshold: customCollapseVerticesThreshold, customPlacementRange: customPlacementRange, customPortHeights: customPortHeights);
      Lyst<ParticlesParams> lyst1 = new Lyst<ParticlesParams>()
      {
        ParticlesParams.Loop("WasteParticles", colorSelector: (Func<RecipeProto, ColorRgba>) (r =>
        {
          LooseProductProto valueOrNull = r.AllInputs.First.Product.DumpableProduct.ValueOrNull;
          return valueOrNull == null ? ColorRgba.Empty : valueOrNull.TerrainMaterial.Value.Graphics.ParticleColor.Rgba;
        }))
      };
      Lyst<ToolbarCategoryProto> lyst2 = new Lyst<ToolbarCategoryProto>()
      {
        prototypesDb.GetOrThrow<ToolbarCategoryProto>(Ids.ToolbarCategories.Transports)
      };
      LayoutEntityProto.VisualizedLayers empty = LayoutEntityProto.VisualizedLayers.Empty;
      ProtosDb protosDb = prototypesDb;
      StaticEntityProto.ID stacker = Ids.Transports.Stacker;
      Proto.Str str = Proto.CreateStr((Proto.ID) Ids.Transports.Stacker, "Stacker", "Dumps material from connected conveyor belts directly on the terrain.");
      EntityLayout layoutOrThrow = registrator.LayoutParser.ParseLayoutOrThrow(layoutParams, "                        (1G(1G                                             ", "   [2][2][2]      (3A(2G(2X(3X(4B(5A                                       ", "A~>[2][2][2](3A(3A(4B(4G(5X(5X(6D(6B(7A(7A(8A(8A(9B(9A10B10A11B11A12B12A12A", "   [2][2][2]      (3A(2G(2X(3X(4B(5A                                       ", "                        (1G(1G                                             ");
      EntityCosts entityCosts = Costs.Buildings.Stacker.MapToEntityCosts(registrator);
      Electricity consumedPowerPerTick = 40.Kw();
      ThicknessTilesI minDumpOffset = 1.TilesThick();
      ThicknessTilesI thicknessTilesI = 2.TilesThick();
      RelTile3i dumpHeadRelPos = new RelTile3i(22, 0, 11);
      Duration dumpDelay = 2.Seconds();
      Duration dumpPeriod = 3.Ticks();
      StackerProto.Gfx graphics = new StackerProto.Gfx("Assets/Base/Transports/Stacker/Stacker.prefab", new RelTile3f(-10.5.ToFix32(), (Fix32) 0, (Fix32) 0), Option<string>.None, lyst1.ToImmutableArray(), ImmutableArray<EmissionParams>.Empty, Option<string>.None, ColorRgba.Empty, false, false, empty, lyst2.ToImmutableArray());
      ThicknessTilesI defaultDumpOffset = thicknessTilesI;
      StackerProto proto = new StackerProto(stacker, str, layoutOrThrow, entityCosts, consumedPowerPerTick, minDumpOffset, dumpHeadRelPos, dumpDelay, dumpPeriod, graphics, defaultDumpOffset);
      protosDb.Add<StackerProto>(proto);
    }

    public StackerData()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
