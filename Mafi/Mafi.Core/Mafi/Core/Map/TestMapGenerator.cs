// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.TestMapGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.Generators;
using Mafi.Core.Terrain.Trees;
using Mafi.Random.Noise;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Map
{
  /// <summary>
  /// Highly customizable and configurable map generator that is mostly for tests.
  /// </summary>
  /// <remarks>
  /// Generated map has following layout:
  /// <code>
  ///      +---------+-----------+---------------------------+-----------------------------+----
  ///      |  ~      |           #                           |                             |
  ///      |         |           #                           |                             |
  ///      |         |           #                           |                             |
  ///      |       ~ |           #                           |                             |
  ///      | ~       |           c                           |                             |
  ///      |  Extra  |   Extra   l                           |                             |
  ///      |  cell   |   cell    i          Cell #2'         |           Cell #n'          |
  ///      | 128x256 |  128x256  f          256x256          |           256x256           |
  ///      |         |   [low]   f          [high]           |            [high]           |
  ///      |    ~    |           #                           |                             |
  ///      |         |           #                           |                             |
  ///      |         |           #                           |                             |
  ///      |  ~      |           #                           |                             |
  ///      |       ~ |           #                           |                             |
  ///      | ~       |           #                           |                             |
  ///      +---------+-----------+###cliff########cliff######+#####cliff########cliff######+----
  ///      |      ~  |           |                           |                             |
  ///      |   ~     |           |                           |                             |
  ///      |         |           |                           |                             |
  ///      | ~    ~  |           |                           |                             |
  ///      |         |           |                           |                             |
  ///      |         |           |                           |                             |
  ///      | Cell #0 |  Cell #1  |          Cell #2          |           Cell #n           |
  ///      | 128x256 |  128x256  |          256x256          |           256x256           |
  ///      |         |           |                           |                             |
  ///      |    ~    |    . t .  | . r .   . r .   . r .     |                             |
  ///      |         |    . r .  | . e .   . e .   . e .     |                             |
  ///      |  ~    ~ |    . e .  | . s .   . s .   . s .  ...|                             |
  ///      |     ~   |    . e .  | . # .   . # .   . # .  etc|                             |
  ///      |  ~      |    . s .  | . 1 .   . 2 .   . 3 .     |                             |
  ///      |         |     trees |   trees   trees   trees   |    trees   trees   trees    |
  /// X -- 0---------+-----------+---------------------------+-----------------------------+----
  ///      |
  ///      Y
  /// </code>
  /// </remarks>
  public class TestMapGenerator : IIslandMapGenerator
  {
    public const string NAME = "TestMapGenerator";
    public static readonly Quantity OIL_DEPOSIT_CAP;
    public static readonly Tile3i OIL_DEPOSIT_POS;
    public static readonly RelTile1i OIL_DEPOSIT_SIZE;
    public static readonly Quantity GROUNDWATER_DEPOSIT_CAP;
    public static readonly Tile3i GROUNDWATER_DEPOSIT_POS;
    public static readonly RelTile1i GROUNDWATER_DEPOSIT_SIZE;
    private static readonly RelTile1i RES_TRANSITION;
    public readonly TestMapGeneratorConfig Config;
    private readonly ProtosDb m_protosDb;
    private Option<IslandMap> m_generatedMap;

    public string Name => nameof (TestMapGenerator);

    public TestMapGenerator(TestMapGeneratorConfig config, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Config = config;
      this.m_protosDb = protosDb;
    }

    public static RectangleTerrainArea2i GetDumpArea()
    {
      return new RectangleTerrainArea2i(new Tile2i(128, 256), new RelTile2i(60, 120));
    }

    public static RectangleTerrainArea2i GetTreesArea()
    {
      return new RectangleTerrainArea2i(TerrainDesignation.GetOrigin(new Tile2i(156, 0)), new RelTile2i(90, 120));
    }

    public static RectangleTerrainArea2i GetTreesAreaSecondary()
    {
      return new RectangleTerrainArea2i(TerrainDesignation.GetOrigin(new Tile2i(236, 10)), new RelTile2i(768, 30));
    }

    public static RectangleTerrainArea2i? GetResourceArea(
      Proto.ID protoId,
      TestMapGeneratorConfig config)
    {
      int index = config.MineableResources.IndexOf((Predicate<TestMapResource>) (x => x.ResourceId == protoId));
      return index < 0 ? new RectangleTerrainArea2i?() : new RectangleTerrainArea2i?(TestMapGenerator.GetResourceArea(index, config));
    }

    public static RectangleTerrainArea2i GetResourceArea(int index, TestMapGeneratorConfig config)
    {
      Tile2i origin = TerrainDesignation.GetOrigin(new Tile2i(256 + index * 50, 40));
      RelTile2i size = new RelTile2i(30.RoundToMultipleOf(4), 60.RoundToMultipleOf(4));
      RectangleTerrainArea2i resourceArea = new RectangleTerrainArea2i(origin, size);
      if (index >= 0)
      {
        int num = index;
        ImmutableArray<TestMapResource> mineableResources = config.MineableResources;
        int length = mineableResources.Length;
        if (num < length)
        {
          mineableResources = config.MineableResources;
          TestMapResource testMapResource = mineableResources[index];
          resourceArea = new RectangleTerrainArea2i(origin, new RelTile2i(size.X, size.Y.ScaledByRounded(testMapResource.Size)));
        }
      }
      return resourceArea;
    }

    public static RectangleTerrainArea2i? GetCliffResourceArea(
      Proto.ID protoId,
      TestMapGeneratorConfig config)
    {
      int index = config.MineableResourcesOnCliff.IndexOf((Predicate<TestMapResource>) (x => x.ResourceId == protoId));
      return index < 0 ? new RectangleTerrainArea2i?() : new RectangleTerrainArea2i?(TestMapGenerator.GetCliffResourceArea(index, config));
    }

    public static RectangleTerrainArea2i GetCliffResourceArea(
      int index,
      TestMapGeneratorConfig config)
    {
      Tile2i origin = TerrainDesignation.GetOrigin(new Tile2i(296 + index * 50, 296));
      RelTile2i size = new RelTile2i(30.RoundToMultipleOf(4), 60.RoundToMultipleOf(4));
      RectangleTerrainArea2i cliffResourceArea = new RectangleTerrainArea2i(origin, size);
      if (index >= 0)
      {
        int num = index;
        ImmutableArray<TestMapResource> resourcesOnCliff = config.MineableResourcesOnCliff;
        int length = resourcesOnCliff.Length;
        if (num < length)
        {
          resourcesOnCliff = config.MineableResourcesOnCliff;
          TestMapResource testMapResource = resourcesOnCliff[index];
          cliffResourceArea = new RectangleTerrainArea2i(origin, new RelTile2i(size.X, size.Y.ScaledByRounded(testMapResource.Size)));
        }
      }
      return cliffResourceArea;
    }

    public IEnumerator<string> GenerateIslandMapTimeSliced()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new TestMapGenerator.\u003CGenerateIslandMapTimeSliced\u003Ed__30(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private void generateResources(
      out Lyst<ITerrainResourceGenerator> terrainResources,
      out Lyst<IVirtualTerrainResource> virtualResources)
    {
      Lyst<ITerrainResourceGenerator> terrResources = new Lyst<ITerrainResourceGenerator>();
      terrainResources = terrResources;
      if (this.Config.PrimaryForestProtoId.HasValue)
        addTrees(TestMapGenerator.GetTreesArea(), this.m_protosDb.GetOrThrow<ForestProto>(this.Config.PrimaryForestProtoId.Value));
      if (this.Config.SecondaryForestProtoId.HasValue)
        addTrees(TestMapGenerator.GetTreesAreaSecondary(), this.m_protosDb.GetOrThrow<ForestProto>(this.Config.SecondaryForestProtoId.Value));
      for (int index = 0; index < this.Config.MineableResources.Length; ++index)
      {
        TerrainMaterialProto orThrow = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.Config.MineableResources[index].ResourceId);
        addMineableProduct(TestMapGenerator.GetResourceArea(index, this.Config), orThrow);
      }
      if (this.Config.ExtraTopCellsSurfaceId.HasValue)
      {
        for (int index = 0; index < this.Config.MineableResourcesOnCliff.Length; ++index)
        {
          TerrainMaterialProto orThrow = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.Config.MineableResourcesOnCliff[index].ResourceId);
          addMineableProduct(TestMapGenerator.GetCliffResourceArea(index, this.Config), orThrow);
        }
      }
      virtualResources = new Lyst<IVirtualTerrainResource>();
      SimpleVirtualResource simpleVirtualResource1 = new SimpleVirtualResource(this.m_protosDb.GetOrThrow<VirtualResourceProductProto>(IdsCore.Products.VirtualCrudeOil), TestMapGenerator.OIL_DEPOSIT_CAP, TestMapGenerator.OIL_DEPOSIT_POS, TestMapGenerator.OIL_DEPOSIT_SIZE);
      virtualResources.Add((IVirtualTerrainResource) simpleVirtualResource1);
      SimpleVirtualResource simpleVirtualResource2 = new SimpleVirtualResource(this.m_protosDb.GetOrThrow<VirtualResourceProductProto>(IdsCore.Products.Groundwater), TestMapGenerator.GROUNDWATER_DEPOSIT_CAP, TestMapGenerator.GROUNDWATER_DEPOSIT_POS, TestMapGenerator.GROUNDWATER_DEPOSIT_SIZE);
      virtualResources.Add((IVirtualTerrainResource) simpleVirtualResource2);

      static void lineFromArea(
        RectangleTerrainArea2i area,
        out Tile2i start,
        out Tile2i end,
        out RelTile1i radius)
      {
        if (area.Size.X >= area.Size.Y)
        {
          radius = new RelTile1i(area.Size.Y / 2);
          start = area.Origin + new RelTile2i(radius, radius);
          end = area.Origin + new RelTile2i(area.Size.X - radius.Value, radius);
        }
        else
        {
          radius = new RelTile1i(area.Size.X / 2);
          start = area.Origin + new RelTile2i(radius, radius);
          end = area.Origin + new RelTile2i(radius, area.Size.Y - radius.Value);
        }
      }

      void addTrees(RectangleTerrainArea2i area, ForestProto forestProto)
      {
        Tile2i start;
        Tile2i end;
        RelTile1i radius;
        lineFromArea(area, out start, out end, out radius);
        terrResources.Add((ITerrainResourceGenerator) new TreesResourceGenerator(start, end, -terrResources.Count, forestProto, radius - TestMapGenerator.RES_TRANSITION - RelTile1i.One, TestMapGenerator.RES_TRANSITION, 0.5.ToFix32(), new SimplexNoise2dParams(), (Fix32) 2, (Fix32) 3, new SimplexNoise2dSeed(Fix32.Tau + (Fix32) terrResources.Count, Fix32.Sqrt2), true, Option<TerrainMaterialProto>.None));
      }

      void addMineableProduct(RectangleTerrainArea2i area, TerrainMaterialProto materialProto)
      {
        Tile2i start;
        Tile2i end;
        RelTile1i radius;
        lineFromArea(area, out start, out end, out radius);
        Lyst<ITerrainResourceGenerator> terrResources = terrResources;
        Tile2i from = start;
        Tile2i to = end;
        int count = terrResources.Count;
        TerrainMaterialProto resourceProto = materialProto;
        RelTile1i resourceRadius = radius - TestMapGenerator.RES_TRANSITION;
        RelTile1i resTransition = TestMapGenerator.RES_TRANSITION;
        Percent zero = Percent.Zero;
        SimplexNoise2dParams baseNoiseParams = new SimplexNoise2dParams((Fix32) 3, (Fix32) 0, (Fix32) 1);
        ThicknessTilesI thicknessTilesI1 = 5.TilesThick();
        ThicknessTilesI thicknessTilesI2 = 2.TilesThick();
        SimplexNoise2dSeed noiseSeed = new SimplexNoise2dSeed(Fix32.Tau + (Fix32) terrResources.Count, Fix32.Sqrt2);
        ThicknessTilesI belowSurfaceMaxDepth = thicknessTilesI1;
        int deltaPriority = count;
        Percent extraFalloffTransitionRadius = zero;
        Percent belowSurfaceExtraHeight = new Percent();
        ThicknessTilesI shapeInversionDepth = thicknessTilesI2;
        Percent groundLevelBias = new Percent();
        NoiseTurbulenceParams turbulenceParams = new NoiseTurbulenceParams();
        SteppedNoiseParams steppedNoiseParams = new SteppedNoiseParams();
        Fix32 sigmoidCenterDistance = new Fix32();
        Fix32 sigmoidSmoothness = new Fix32();
        Option<TerrainMaterialProto> surfaceCoverResourceProto = new Option<TerrainMaterialProto>();
        SimplexNoise2dParams surfaceCoverThickness = new SimplexNoise2dParams();
        SimplexNoise2dParams coordWarpNoiseParams = new SimplexNoise2dParams();
        ThicknessTilesF heightBiasAtFromPoint = new ThicknessTilesF();
        ThicknessTilesF heightBiasAtToPoint = new ThicknessTilesF();
        LineBlobTerrainResourceGenerator resourceGenerator = new LineBlobTerrainResourceGenerator(from, to, resourceProto, resourceRadius, resTransition, baseNoiseParams, noiseSeed, belowSurfaceMaxDepth, deltaPriority, extraFalloffTransitionRadius, belowSurfaceExtraHeight: belowSurfaceExtraHeight, shapeInversionDepth: shapeInversionDepth, groundLevelBias: groundLevelBias, turbulenceParams: turbulenceParams, steppedNoiseParams: steppedNoiseParams, replacePreviousResource: true, sigmoidCenterDistance: sigmoidCenterDistance, sigmoidSmoothness: sigmoidSmoothness, surfaceCoverResourceProto: surfaceCoverResourceProto, surfaceCoverThickness: surfaceCoverThickness, coordWarpNoiseParams: coordWarpNoiseParams, heightBiasAtFromPoint: heightBiasAtFromPoint, heightBiasAtToPoint: heightBiasAtToPoint);
        terrResources.Add((ITerrainResourceGenerator) resourceGenerator);
      }
    }

    public IslandMap GetMapAndClear()
    {
      Assert.That<Option<IslandMap>>(this.m_generatedMap).HasValue<IslandMap>("No map was generated.");
      IslandMap mapAndClear = this.m_generatedMap.Value;
      this.m_generatedMap = (Option<IslandMap>) Option.None;
      return mapAndClear;
    }

    public static IMapCellEdgeTerrainFactory CreateCellEdgeFactory(
      TerrainMaterialProto cliffMaterialProto)
    {
      return (IMapCellEdgeTerrainFactory) new CellEdgeTerrainGeneratorFactory(10, cliffMaterialProto, 10.Tiles(), 6.Tiles(), 1.2.Percent(), 1.Tiles(), 5.TilesThick(), new SimplexNoise2dSeed(Fix32.Tau, Fix32.Sqrt2), new NoiseTurbulenceParams(8, Percent.FromFloat(1.92f), Percent.FromFloat(0.5f)), true);
    }

    public static IMapCellEdgeTerrainFactory CreateOceanCellEdgeFactory(
      TerrainMaterialProto cliffMaterialProto)
    {
      return (IMapCellEdgeTerrainFactory) new CellEdgeTerrainGeneratorFactory(10, cliffMaterialProto, 30.Tiles(), 0.Tiles(), 0.25.Percent(), 1.Tiles(), 5.TilesThick(), new SimplexNoise2dSeed(Fix32.Sqrt2, Fix32.Tau), new NoiseTurbulenceParams(5, Percent.FromFloat(1.92f), Percent.FromFloat(0.5f)), true);
    }

    static TestMapGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TestMapGenerator.OIL_DEPOSIT_CAP = new Quantity(22000);
      TestMapGenerator.OIL_DEPOSIT_POS = new Tile3i(256, 196, 0);
      TestMapGenerator.OIL_DEPOSIT_SIZE = new RelTile1i(12);
      TestMapGenerator.GROUNDWATER_DEPOSIT_CAP = new Quantity(20000);
      TestMapGenerator.GROUNDWATER_DEPOSIT_POS = new Tile3i(316, 196, 0);
      TestMapGenerator.GROUNDWATER_DEPOSIT_SIZE = new RelTile1i(90);
      TestMapGenerator.RES_TRANSITION = new RelTile1i(12);
    }
  }
}
