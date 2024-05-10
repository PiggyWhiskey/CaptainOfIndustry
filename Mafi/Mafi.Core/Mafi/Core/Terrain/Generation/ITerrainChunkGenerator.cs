// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainChunkGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Map;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>
  /// Defines interface for generation of terrain chunks. This interface is chunk-based not tile-bases so that the
  /// generator can optimize generation of the whole chunk.
  /// </summary>
  public interface ITerrainChunkGenerator
  {
    /// <summary>Initializes terrain generation.</summary>
    void InitializeTerrainGeneration(IslandMap map);

    /// <summary>
    /// Generates data for given chunk coordinate. This method MUST be thread safe!
    /// </summary>
    ChunkTerrainData GenerateChunk(Chunk2i chunkCoord);
  }
}
