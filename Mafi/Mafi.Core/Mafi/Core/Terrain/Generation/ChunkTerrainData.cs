// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ChunkTerrainData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public class ChunkTerrainData
  {
    public readonly Chunk2i ChunkCoord;
    public readonly TileTerrainData[] Data;

    public ChunkTerrainData(Chunk2i chunkCoord)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ChunkCoord = chunkCoord;
      this.Data = new TileTerrainData[4096];
    }

    public ChunkTerrainData(Chunk2i chunkCoord, TileTerrainData[] data)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ChunkCoord = chunkCoord;
      this.Data = data;
    }
  }
}
