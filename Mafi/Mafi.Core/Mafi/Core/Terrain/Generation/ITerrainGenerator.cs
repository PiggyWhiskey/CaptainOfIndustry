// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core.Products;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ITerrainGenerator
  {
    int TerrainWidth { get; }

    int TerrainHeight { get; }

    TerrainMaterialProto Bedrock { get; }

    /// <summary>
    /// If set, automatic ocean will not be created on the map. This is mostly useful in tests.
    /// </summary>
    bool DoNotCreateOcean { get; }

    IEnumerator<string> GenerateTerrain(
      RelTile2i terrainSize,
      Dict<Chunk2i, ChunkTerrainData> resultChunks);

    IEnumerator<string> PostProcessTerrain(TerrainManager terrain, bool gameIsBeingLoaded);

    Lyst<ChunkTerrainData> RegenerateChunkNoPostProcess(IEnumerable<Chunk2i> coords);
  }
}
