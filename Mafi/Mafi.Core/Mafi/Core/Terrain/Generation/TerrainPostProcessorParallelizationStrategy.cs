// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.TerrainPostProcessorParallelizationStrategy
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public enum TerrainPostProcessorParallelizationStrategy
  {
    /// <summary>
    /// All chunks are analyzed in parallel and then all are applied on the main thread.
    /// Use this strategy when the amount of data that needs to be saved per chunk is small and data from the entire
    /// terrain can be stored before they are applied. This maximizes parallelization of the analysis steps by having
    /// only one sync at the end. This option also allows to have unlimited context for the analysis as no terrain data
    /// is written before all chunks are analyzed.
    /// </summary>
    AnalyzeAllThenApply,
    /// <summary>
    /// Analysis happens in parallel in stages. In the first stage, one or more rows of chunks are analyzed in parallel
    /// and in the second stage the data from all but the last row is applied. This then repeats for the rest of rows.
    /// This ensures that the analysis has at least one chunk worth of context available (terrain around analyzed chunk
    /// is guaranteed to not have changes applied). The advantage is that the changes are being applied gradually and
    /// their memory can be freed or reused. Disadvantage is that threads that perform the analysis have to be synced
    /// in between stages. Note that when the generated terrain area is small enough, <see cref="F:Mafi.Core.Terrain.Generation.TerrainPostProcessorParallelizationStrategy.AnalyzeAllThenApply" />
    /// could be used to improve performance.
    /// </summary>
    AnalyzeInterleaveAndApply,
    /// <summary>
    /// Parallelization is handled by the post-processor. Use <see cref="T:Mafi.Core.Terrain.Generation.CustomTerrainPostProcessorV2" /> for this option.
    /// </summary>
    CustomSchedule,
  }
}
