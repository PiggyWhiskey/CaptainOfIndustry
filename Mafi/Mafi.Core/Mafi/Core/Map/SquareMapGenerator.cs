// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.SquareMapGenerator
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
using Mafi.Core.Terrain.Generation.Generators;
using Mafi.Core.Terrain.Trees;
using Mafi.Random.Noise;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Map
{
  /// <summary>
  /// Very simple map generator that generates a grid of cells based on config. This is mainly for tests.
  /// </summary>
  public class SquareMapGenerator : IIslandMapGenerator
  {
    public const string NAME = "SquareMapGenerator";
    private readonly SquareMapGeneratorConfig m_config;
    private readonly ProtosDb m_protosDb;
    private readonly MapCellSurfaceGeneratorProto m_surfaceProto;
    private ImmutableArray<MapCell> m_cells;
    private ImmutableArray<Tile2i> m_centers;
    private ImmutableArray<Tile2i> m_perimeter;
    private StartingLocation m_startingLocation;

    public string Name => nameof (SquareMapGenerator);

    public SquareMapGenerator(SquareMapGeneratorConfig config, ProtosDb protosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_config = config;
      this.m_protosDb = protosDb;
      this.m_surfaceProto = protosDb.GetOrThrow<MapCellSurfaceGeneratorProto>((Proto.ID) config.SurfaceProtoId);
    }

    public IEnumerator<string> GenerateIslandMapTimeSliced()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new SquareMapGenerator.\u003CGenerateIslandMapTimeSliced\u003Ed__11(0)
      {
        \u003C\u003E4__this = this
      };
    }

    public IslandMap GetMapAndClear()
    {
      Lyst<ITerrainResourceGenerator> lyst = new Lyst<ITerrainResourceGenerator>();
      if (this.m_config.ForestProtoId.HasValue)
        lyst.Add((ITerrainResourceGenerator) new TreesResourceGenerator(this.m_config.ForestArea.Origin, this.m_config.ForestArea.PlusXyTileIncl, 10, this.m_protosDb.GetOrThrow<ForestProto>(this.m_config.ForestProtoId.Value), 5.Tiles(), 1.Tiles(), 0.5.ToFix32(), new SimplexNoise2dParams(), (Fix32) 2, (Fix32) 3, new SimplexNoise2dSeed(Fix32.Tau, Fix32.Sqrt2), true, Option<TerrainMaterialProto>.None));
      TerrainMaterialProto orThrow = this.m_protosDb.GetOrThrow<TerrainMaterialProto>(this.m_config.BedrockMaterialId);
      string name = this.Name;
      StartingLocation startingLocation = this.m_startingLocation;
      ProtosDb protosDb = this.m_protosDb;
      TerrainMaterialProto bedrock = orThrow;
      ImmutableArray<MapCell> cells = this.m_cells;
      ImmutableArray<Tile2i> centers = this.m_centers;
      ImmutableArray<Tile2i> perimeter = this.m_perimeter;
      ImmutableArray<ITerrainResourceGenerator> immutableArrayAndClear = lyst.ToImmutableArrayAndClear();
      ImmutableArray<IVirtualTerrainResource> empty = (ImmutableArray<IVirtualTerrainResource>) ImmutableArray.Empty;
      Lyst<ITerrainPostProcessor> terrainPostProcessors1 = this.m_config.TerrainPostProcessors;
      ImmutableArray<ITerrainPostProcessor> terrainPostProcessors2 = terrainPostProcessors1 != null ? terrainPostProcessors1.ToImmutableArray() : ImmutableArray<ITerrainPostProcessor>.Empty;
      ImmutableArray<TerrainPropMapData> immutableArray = this.m_config.TerrainProps.ToImmutableArray();
      IslandMapConfig config = this.m_config.OceanSize > 0 ? IslandMapConfig.Default : IslandMapConfig.DefaultNoOcean;
      IslandMapDifficultyConfig difficultyConfig = new IslandMapDifficultyConfig();
      IslandMap mapAndClear = new IslandMap(name, 1, startingLocation, protosDb, bedrock, cells, centers, perimeter, immutableArrayAndClear, empty, terrainPostProcessors2, immutableArray, config, difficultyConfig);
      this.m_cells = ImmutableArray<MapCell>.Empty;
      this.m_centers = ImmutableArray<Tile2i>.Empty;
      this.m_perimeter = ImmutableArray<Tile2i>.Empty;
      this.m_startingLocation = (StartingLocation) null;
      return mapAndClear;
    }
  }
}
