// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.GeneratedTerrainData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public sealed class GeneratedTerrainData
  {
    public readonly IReadOnlyDictionary<Chunk2i, ChunkTerrainData> Chunks;
    private TileTerrainData m_dummyData;

    public GeneratedTerrainData(
      IReadOnlyDictionary<Chunk2i, ChunkTerrainData> chunks)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Chunks = chunks;
    }

    /// <summary>
    /// Tries to return tile data at given coordinate. Returned struct is a copy and it should be used only fo
    /// reading. Use <see cref="M:Mafi.Core.Terrain.Generation.GeneratedTerrainData.TryGetTileDataRef(Mafi.Tile2i,System.Boolean@)" /> if you with to mutate data.
    /// </summary>
    public bool TryGetTileDataOrDummy(Tile2i position, out TileTerrainData tileData)
    {
      ChunkTerrainData chunkTerrainData;
      if (this.Chunks.TryGetValue(position.ChunkCoord2i, out chunkTerrainData))
      {
        tileData = chunkTerrainData.Data[position.TileInChunkCoord.ChunkDataIndex];
        return true;
      }
      tileData = new TileTerrainData();
      return false;
    }

    public bool TryGetSurfaceHeight(Tile2i position, out HeightTilesF height)
    {
      ChunkTerrainData chunkTerrainData;
      if (this.Chunks.TryGetValue(position.ChunkCoord2i, out chunkTerrainData))
      {
        height = chunkTerrainData.Data[position.TileInChunkCoord.ChunkDataIndex].SurfaceHeight;
        return true;
      }
      height = new HeightTilesF();
      return false;
    }

    public ref TileTerrainData TryGetTileDataRef(Tile2i position, out bool exists)
    {
      ChunkTerrainData chunkTerrainData;
      if (this.Chunks.TryGetValue(position.ChunkCoord2i, out chunkTerrainData))
      {
        exists = true;
        return ref chunkTerrainData.Data[position.TileInChunkCoord.ChunkDataIndex];
      }
      exists = false;
      return ref this.m_dummyData;
    }

    public TileTerrainData GetTileDataOrDummy(Tile2i position)
    {
      ChunkTerrainData chunkTerrainData;
      return this.Chunks.TryGetValue(position.ChunkCoord2i, out chunkTerrainData) ? chunkTerrainData.Data[position.TileInChunkCoord.ChunkDataIndex] : this.m_dummyData;
    }
  }
}
