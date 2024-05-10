// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainGeneratorV2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ITerrainGeneratorV2
  {
    TerrainGenerationContext CreateContext(
      RelTile2i terrainSize,
      Tile2i zero,
      TerrainManager.TerrainData data,
      TerrainMaterialProto bedrockMaterial,
      int initialMapCreationSaveVersion);

    IEnumerator<Percent> GenerateTerrainTimeSliced(
      TerrainGenerationContext context,
      IIndexable<ITerrainFeatureGenerator> terrainFeatureGenerators,
      IIndexable<ITerrainPostProcessorV2> terrainPostProcessors,
      Func<Tile2i, bool> isOcean);

    IEnumerator<Percent> ApplyGeneratedData(
      TerrainGenerationContext context,
      Chunk64Area areaToApply,
      TerrainManager terrainManager,
      bool isInMapEditor);

    IEnumerator<Percent> GenerateTerrainFeaturesTimeSliced(
      IIndexable<ITerrainFeatureGenerator> generators,
      Chunk64Area areaChunks,
      TerrainGenerationContext terrainData,
      int reportProgressFrequencyMs);

    IEnumerator<Percent> PostProcessTerrainTimeSliced(
      IIndexable<ITerrainPostProcessorV2> postProcessors,
      Chunk64Area areaChunks,
      TerrainGenerationContext terrainData,
      int reportProgressFrequencyMs);

    bool IsSerial();

    void SetSerial();

    void SetPerfDataCollection(bool isEnabled);

    void Cancel();

    void Clear();
  }
}
