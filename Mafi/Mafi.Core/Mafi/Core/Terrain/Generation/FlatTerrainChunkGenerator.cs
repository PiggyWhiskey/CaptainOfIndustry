// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.FlatTerrainChunkGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Map;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>Chunk generator that generates flat terrain.</summary>
  public class FlatTerrainChunkGenerator : ITerrainChunkGenerator
  {
    private HeightTilesF m_defaultHeight;
    private Option<IslandMap> m_map;

    public FlatTerrainChunkGenerator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void InitializeValues(HeightTilesF defaultHeight = default (HeightTilesF))
    {
      this.m_defaultHeight = defaultHeight;
    }

    public void InitializeTerrainGeneration(IslandMap map) => this.m_map = (Option<IslandMap>) map;

    public ChunkTerrainData GenerateChunk(Chunk2i chunkCoord)
    {
      HeightTilesF heightTilesF;
      if (this.m_map.HasValue)
      {
        Tile2i chunkCenter = chunkCoord.CenterTile2i;
        heightTilesF = this.m_map.Value.Cells.MinElement<long>((Func<MapCell, long>) (x => x.CenterTile.DistanceSqrTo(chunkCenter))).GroundHeight.HeightTilesF;
      }
      else
        heightTilesF = this.m_defaultHeight;
      ChunkTerrainData chunk = new ChunkTerrainData(chunkCoord);
      for (int index1 = 0; index1 < 64; ++index1)
      {
        int num = index1 << 6;
        for (int index2 = 0; index2 < 64; ++index2)
          chunk.Data[num + index2] = new TileTerrainData()
          {
            SurfaceHeight = heightTilesF
          };
      }
      return chunk;
    }
  }
}
