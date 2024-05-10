// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.MapCellTerrainChunkGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Map;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Numerics;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>
  /// Generates terrain based on the <see cref="T:Mafi.Core.Map.IslandMap" />.
  /// </summary>
  public class MapCellTerrainChunkGenerator : ITerrainChunkGenerator
  {
    private static readonly HeightTilesF MIN_OCEAN_FLOOR_HEIGHT;
    private IslandMap m_map;

    public MapCellTerrainChunkGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void InitializeTerrainGeneration(IslandMap map) => this.m_map = map;

    public ChunkTerrainData GenerateChunk(Chunk2i chunkCoord)
    {
      ChunkTerrainData data = new ChunkTerrainData(chunkCoord);
      Tile2i centerTile = chunkCoord.CenterTile2i;
      ITerrainResourceChunkGenerator[] array = this.m_map.AllTerrainGenerators.Where((Func<ITerrainResourceGenerator, bool>) (x => x.Position.Tile2i.DistanceSqrTo(centerTile) < (x.MaxRadius + TerrainChunk.Size).Squared)).Select<ITerrainResourceGenerator, ITerrainResourceChunkGenerator>((Func<ITerrainResourceGenerator, ITerrainResourceChunkGenerator>) (x => x.CreateChunkGenerator(chunkCoord))).ToArray<ITerrainResourceChunkGenerator>();
      ImmutableArray<MapCell> cellsOnChunk = this.m_map.GetCellsOnChunk(chunkCoord);
      Option<MapCell> currentCell = Option<MapCell>.None;
      TerrainGenerationBuffer buffer = new TerrainGenerationBuffer();
      Fix32 fix32_1;
      Fix32 fix32_2;
      Fix32 to1;
      Fix32 to2;
      if (this.m_map.CellCoastLines.IsNotEmpty && (currentCell.IsNone || currentCell.Value.IsOcean))
      {
        Vector2i tile = chunkCoord.Tile2i.Vector2i;
        fix32_1 = this.m_map.CellCoastLines.Min<Fix64>((Func<Line2i, Fix64>) (x => x.DistanceSqrToLineSegment(tile))).SqrtToFix32();
        Vector2i tileX = chunkCoord.PlusXNeighbor.Tile2i.Vector2i;
        fix32_2 = this.m_map.CellCoastLines.Min<Fix64>((Func<Line2i, Fix64>) (x => x.DistanceSqrToLineSegment(tileX))).SqrtToFix32();
        Chunk2i chunk2i = chunkCoord.PlusYNeighbor;
        Vector2i tileY = chunk2i.Tile2i.Vector2i;
        to1 = this.m_map.CellCoastLines.Min<Fix64>((Func<Line2i, Fix64>) (x => x.DistanceSqrToLineSegment(tileY))).SqrtToFix32();
        chunk2i = chunkCoord.PlusXNeighbor;
        chunk2i = chunk2i.PlusYNeighbor;
        Vector2i tileXy = chunk2i.Tile2i.Vector2i;
        to2 = this.m_map.CellCoastLines.Min<Fix64>((Func<Line2i, Fix64>) (x => x.DistanceSqrToLineSegment(tileXy))).SqrtToFix32();
      }
      else
      {
        fix32_1 = Fix32.Zero;
        fix32_2 = Fix32.Zero;
        to1 = Fix32.Zero;
        to2 = Fix32.Zero;
      }
      Fix32 fix32_3 = 64.ToFix32();
      HeightTilesF heightTilesF1 = this.m_map.Config.OceanFloorBaseHeight.HeightTilesF;
      Fix32 fix32_4 = (Fix32) this.m_map.Config.OceanFloorFlatDistance.Value;
      ThicknessTilesF distanceFromCoast = this.m_map.Config.OceanFloorHeightPerDistanceFromCoast;
      for (int y = 0; y < 64; ++y)
      {
        int num = y << 6;
        Fix32 fix32_5 = fix32_1.Lerp(to1, y / fix32_3);
        Fix32 to3 = fix32_2.Lerp(to2, y / fix32_3);
        for (int x = 0; x < 64; ++x)
        {
          Tile2i coord = chunkCoord + new TileInChunk2i(x, y);
          if (currentCell.IsNone || !currentCell.Value.Contains(coord))
            currentCell = (Option<MapCell>) cellsOnChunk.FirstOrDefault((Func<MapCell, bool>) (c => c.Contains(coord)));
          bool isOcean = currentCell.IsNone || currentCell.Value.IsOcean;
          HeightTilesF defaultSurfaceHeight;
          if (isOcean)
          {
            HeightTilesF heightTilesF2 = heightTilesF1;
            Fix32 fix32_6 = fix32_5.Lerp(to3, x / fix32_3) - fix32_4;
            if (fix32_6.IsPositive)
              heightTilesF2 += fix32_6 * distanceFromCoast;
            defaultSurfaceHeight = heightTilesF2.Max(MapCellTerrainChunkGenerator.MIN_OCEAN_FLOOR_HEIGHT);
          }
          else
            defaultSurfaceHeight = currentCell.Value.GroundHeight.HeightTilesF;
          buffer.Initialize(defaultSurfaceHeight);
          this.generateTile(currentCell, coord, array, buffer, isOcean, ref data.Data[num + x]);
        }
      }
      foreach (ITerrainResourceChunkGenerator resourceChunkGenerator in array)
        resourceChunkGenerator.ChunkGenerationDone(data);
      return data;
    }

    private void generateTile(
      Option<MapCell> currentCell,
      Tile2i coord,
      ITerrainResourceChunkGenerator[] resGens,
      TerrainGenerationBuffer buffer,
      bool isOcean,
      ref TileTerrainData tileData)
    {
      if (currentCell.HasValue)
        currentCell.Value.SurfaceGenerator.GenerateSurfaceAt(currentCell.Value, coord, buffer, false);
      foreach (ITerrainResourceChunkGenerator resGen in resGens)
        resGen.GenerateResource(coord, buffer);
      tileData.Products.Clear();
      tileData.SurfaceHeight = buffer.GetThicknessesStackAndClear(this.m_map.Bedrock, ref tileData.Products, out tileData.TreeData, out tileData.TerrainPropData);
      if (!isOcean || !(tileData.SurfaceHeight < OceanTerrainManager.OCEAN_THRESHOLD))
        return;
      tileData.TreeData = new TreeData();
      tileData.TerrainPropData = new TerrainPropData();
    }

    static MapCellTerrainChunkGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MapCellTerrainChunkGenerator.MIN_OCEAN_FLOOR_HEIGHT = new HeightTilesF(-30);
    }
  }
}
