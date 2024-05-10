// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainPostProcessorV2
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ITerrainPostProcessorV2 : ITerrainFeatureBase
  {
    /// <summary>
    /// Parallelization strategy, if unsure, choose
    /// <see cref="F:Mafi.Core.Terrain.Generation.TerrainPostProcessorParallelizationStrategy.AnalyzeInterleaveAndApply" />.
    /// </summary>
    TerrainPostProcessorParallelizationStrategy ParallelizationStrategy { get; }

    /// <summary>
    /// Returns sorting priority, lower number is higher priority. Use <see cref="T:Mafi.Core.Terrain.Generation.TerrainPostProcessorPriorityBase" />
    /// for base range which is usually biased by the height.
    /// </summary>
    int SortingPriority { get; }

    /// <summary>
    /// Number of times to run through non-custom parallelization strategies. For example, if we're doing analyze
    /// all then apply, and NumPasses == 2, it'll call analyze, apply, analyze, apply.
    /// </summary>
    int PassCount { get; }

    /// <summary>Set by terrain generator.</summary>
    TimeSpan LastGenerationTime { get; set; }

    /// <summary>
    /// Analyzes the given chunk from the given terrain data and stores any potential changes internally. The given
    /// terrain data MUST NOT be changed. This operation should do as much work as possible as it will run parallel
    /// with other chunks.
    /// </summary>
    void AnalyzeChunkThreadSafe(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly,
      int pass);

    /// <summary>
    /// Applies data from previously completed analysis for a chunk at <paramref name="chunk" />. Data application
    /// is always invoked synchronously since <see cref="T:Mafi.Core.Terrain.TerrainManager.TerrainData" /> is not thread safe.
    /// </summary>
    void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass);
  }
}
