// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.ProceduralIslandMapGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Vornoi;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Map
{
  /// <summary>
  /// Generates map cells in a circular shape with starting cell at center left. Cells are getting larger as they are
  /// further from starting cell.
  /// </summary>
  public class ProceduralIslandMapGenerator : IIslandMapGenerator
  {
    public const string NAME = "ProceduralIslandMapGenerator";
    private static readonly RelTile1i MIN_EDGE_LENGTH;
    private readonly IRandom m_random;
    private readonly ProceduralIslandMapGeneratorConfig m_config;
    private readonly PoissonDiskPointSampler m_pointsSampler;
    private readonly ProtosDb m_protosDb;
    private readonly ImmutableArray<IResourceGeneratorFactory> m_generatorFactories;
    private ImmutableArray<MapCell> m_resultCells;
    private ImmutableArray<Tile2i> m_resultCtrlPts;
    private ImmutableArray<Tile2i> m_resultPerimPts;
    private ImmutableArray<ITerrainResourceGenerator> m_resources;
    private ImmutableArray<IVirtualTerrainResource> m_virtualResources;
    private ImmutableArray<ITerrainPostProcessor> m_postProcessors;
    private StartingLocation m_startLocation;

    public string Name => nameof (ProceduralIslandMapGenerator);

    public ProceduralIslandMapGenerator(
      RandomProvider randomProvider,
      ProceduralIslandMapGeneratorConfig config,
      AllImplementationsOf<IResourceGeneratorFactory> generatorFactories,
      ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_resultCells = (ImmutableArray<MapCell>) ImmutableArray.Empty;
      this.m_resultCtrlPts = (ImmutableArray<Tile2i>) ImmutableArray.Empty;
      this.m_resultPerimPts = (ImmutableArray<Tile2i>) ImmutableArray.Empty;
      this.m_resources = (ImmutableArray<ITerrainResourceGenerator>) ImmutableArray.Empty;
      this.m_virtualResources = (ImmutableArray<IVirtualTerrainResource>) ImmutableArray.Empty;
      this.m_postProcessors = (ImmutableArray<ITerrainPostProcessor>) ImmutableArray.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_random = randomProvider.GetSimRandomFor((object) this, config.MapRandomSeed);
      this.m_config = config.CheckNotNull<ProceduralIslandMapGeneratorConfig>();
      this.m_pointsSampler = new PoissonDiskPointSampler(this.m_random);
      this.m_generatorFactories = generatorFactories.Implementations.OrderBy<int>((Func<IResourceGeneratorFactory, int>) (x => x.Priority)).ToImmutableArray<IResourceGeneratorFactory>();
      this.m_protosDb = protosDb;
    }

    public IslandMap GetMapAndClear()
    {
      IslandMap mapAndClear = new IslandMap(this.Name, 1, this.m_startLocation, this.m_protosDb, this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.m_config.BedrockMaterialId), this.m_resultCells, this.m_resultCtrlPts, this.m_resultPerimPts, this.m_resources, this.m_virtualResources, this.m_postProcessors, ImmutableArray<TerrainPropMapData>.Empty, IslandMapConfig.Default, new IslandMapDifficultyConfig());
      this.m_resultCells = ImmutableArray<MapCell>.Empty;
      this.m_resultCtrlPts = ImmutableArray<Tile2i>.Empty;
      this.m_resultPerimPts = ImmutableArray<Tile2i>.Empty;
      this.m_resources = ImmutableArray<ITerrainResourceGenerator>.Empty;
      this.m_virtualResources = ImmutableArray<IVirtualTerrainResource>.Empty;
      this.m_postProcessors = ImmutableArray<ITerrainPostProcessor>.Empty;
      return mapAndClear;
    }

    public IEnumerator<string> GenerateIslandMapTimeSliced()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new ProceduralIslandMapGenerator.\u003CGenerateIslandMapTimeSliced\u003Ed__18(0)
      {
        \u003C\u003E4__this = this
      };
    }

    private Lyst<Vector2i> generatePoints(int islandRadiusTiles)
    {
      int minDiameter = this.m_config.MinCellDiameter.Value;
      Vector2i startCell = new Vector2i(-islandRadiusTiles + this.m_config.MinCellDiameter.Value / 2, 0);
      Lyst<Vector2i> points = this.m_pointsSampler.GeneratePoints(1000, startCell, this.m_config.MinCellDiameter.Value, this.m_config.MinCellDiameter.Value * this.m_config.CellMinMaxDiameterMult, this.m_config.CellExpansionTrials, new Func<Vector2i, Vector2i>(getMinMaxDistance));
      int num1 = this.m_config.OceanCellDiameter.Value;
      int radius = islandRadiusTiles + num1 / 2;
      int num2 = radius + num1 / 3;
      int denominator = (MafiMath.CircleCircumferenceFromRadius((float) radius) / (float) num1).RoundToInt();
      for (int index = 0; index < denominator; ++index)
      {
        Vector2f directionVector = (AngleDegrees1f.Deg180 + (360 * index).Over(denominator).Degrees()).DirectionVector;
        Vector2f vector2f = directionVector * radius;
        Vector2i roundedVector2i1 = vector2f.RoundedVector2i;
        vector2f = directionVector * num2;
        Vector2i roundedVector2i2 = vector2f.RoundedVector2i;
        points.Add(roundedVector2i1, roundedVector2i2);
      }
      return points;

      Vector2i getMinMaxDistance(Vector2i cellPosition)
      {
        if (((Fix32) islandRadiusTiles - cellPosition.Length).IsNotPositive)
          return Vector2i.Zero;
        Fix32 x = (Fix32) minDiameter + (cellPosition - startCell).Length.Pow(this.m_config.CellSizeGrowthFromStartExp);
        return new Vector2f(x, x * this.m_config.CellMinMaxDiameterMult).RoundedVector2i;
      }
    }

    private void generateHeights(
      MapCell startCell,
      ImmutableArray<MapCell> cells,
      ImmutableArray<Tile2i> cellCenters)
    {
      Lyst<MapCell> elements = new Lyst<MapCell>()
      {
        startCell
      };
      Tile2i cellCenter = cellCenters[elements.First.CenterPointIndex];
      Set<MapCell> queuedCells = new Set<MapCell>();
      queuedCells.AddRange((IEnumerable<MapCell>) elements);
      int num1 = 0;
      Percent percent;
      while (elements.Count > 0)
      {
        int index = this.m_random.NextInt(0, elements.Count);
        MapCell mapCell1 = elements[index];
        Assert.That<bool>(mapCell1.IsOnMapBoundary).IsFalse();
        Assert.That<bool>(mapCell1.IsOcean).IsFalse();
        elements.Remove(mapCell1);
        ++num1;
        foreach (MapCellId neighborCellsValidIndex in mapCell1.NeighborCellsValidIndices)
        {
          MapCell cell = cells[neighborCellsValidIndex.Value];
          if (cell.IsNotOnMapBoundary && queuedCells.Add(cell))
            elements.Add(cell);
        }
        if (mapCell1 == startCell)
        {
          ((IMapCellGeneratorFriend) mapCell1).SetHeight(this.m_config.MinCellHeight);
        }
        else
        {
          MinMaxPair<HeightTilesI> minMaxPair = mapCell1.NeighborCellsValidIndices.Select<MapCell>((Func<MapCellId, MapCell>) (i => cells[i.Value])).Where<MapCell>((Func<MapCell, bool>) (c => !c.IsOnMapBoundary && queuedCells.Contains(c))).Select<MapCell, HeightTilesI>((Func<MapCell, HeightTilesI>) (c => c.GroundHeight)).MinMax<HeightTilesI>();
          int num2 = minMaxPair.Min.Max(this.m_config.MinCellHeight).Value;
          int num3 = minMaxPair.Max.Max(this.m_config.MinCellHeight).Value;
          Fix32 fix32_1 = (Fix32) this.m_config.CellHeightDiffMean.Value + cellCenters[mapCell1.CenterPointIndex].DistanceTo(cellCenter) * this.m_config.CellHeightMeanDistanceToStartMult + 0.3.ToFix32() * num2 + 0.7.ToFix32() * num3;
          percent = this.m_random.NextGaussianTrunc(this.m_config.CellHeightDiffMaxStdDev);
          Fix32 fix32_2 = (Fix32) percent.Apply(this.m_config.CellHeightDiffStdDev.Value);
          HeightTilesI height = new HeightTilesI((fix32_1 + fix32_2).ToIntRounded().Max(this.m_config.MinCellHeight.Value));
          MapCell mapCell2 = mapCell1.NeighborCellsValidIndices.Select<MapCell>((Func<MapCellId, MapCell>) (i => cells[i.Value])).FirstOrDefault<MapCell>((Func<MapCell, bool>) (c => c == startCell));
          if (mapCell2 != null)
            height = height.Average(mapCell2.GroundHeight);
          ((IMapCellGeneratorFriend) mapCell1).SetHeight(height);
        }
      }
      foreach (MapCell mapCell in cells.Where((Func<MapCell, bool>) (x => x.IsOnMapBoundary)))
      {
        ++num1;
        if (mapCell.IsOcean)
        {
          ((IMapCellGeneratorFriend) mapCell).SetHeight(MapCell.DEFAULT_OCEAN_FLOOR_HEIGHT);
        }
        else
        {
          MinMaxPair<HeightTilesI> minMaxPair = mapCell.NeighborCellsValidIndices.Select<MapCell>((Func<MapCellId, MapCell>) (i => cells[i.Value])).Where<MapCell>((Func<MapCell, bool>) (c => c.IsNotOnMapBoundary || c.IsOcean)).Select<MapCell, HeightTilesI>((Func<MapCell, HeightTilesI>) (c => c.GroundHeight)).MinMax<HeightTilesI>();
          Fix32 fix32_3 = (Fix32) ((minMaxPair.Min.Value + minMaxPair.Max.Value) / 2);
          percent = this.m_random.NextGaussianTrunc(this.m_config.CellHeightDiffMaxStdDev);
          Fix32 fix32_4 = (Fix32) percent.Apply(this.m_config.CellHeightDiffStdDev.Value);
          HeightTilesI height = new HeightTilesI((fix32_3 + fix32_4).ToIntRounded().Max(this.m_config.MinCellHeight.Value));
          ((IMapCellGeneratorFriend) mapCell).SetHeight(height);
        }
      }
      Assert.That<int>(num1).IsEqualTo(cells.Length, "Some cells were not processed as they are not accessible from starting cell.");
    }

    private void generateResources(ImmutableArray<MapCell> allCells, MapCell startingCell)
    {
      Lyst<ITerrainResourceGenerator> source1 = new Lyst<ITerrainResourceGenerator>();
      Lyst<IVirtualTerrainResource> source2 = new Lyst<IVirtualTerrainResource>();
      this.m_resources = source1.OrderBy<ITerrainResourceGenerator, int>((Func<ITerrainResourceGenerator, int>) (x => x.Priority)).ToImmutableArray<ITerrainResourceGenerator>();
      this.m_virtualResources = source2.OrderBy<IVirtualTerrainResource, int>((Func<IVirtualTerrainResource, int>) (x => x.Priority)).ToImmutableArray<IVirtualTerrainResource>();
    }

    static ProceduralIslandMapGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ProceduralIslandMapGenerator.MIN_EDGE_LENGTH = new RelTile1i(8);
    }
  }
}
