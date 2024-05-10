// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainFeatureGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ITerrainFeatureGenerator : ITerrainFeatureBase
  {
    /// <summary>
    /// Returns sorting priority, lower number is higher priority. Use <see cref="T:Mafi.Core.Terrain.Generation.TerrainFeaturePriorityBase" />
    /// for base range which is usually biased by the height.
    /// </summary>
    int SortingPriority { get; }

    /// <summary>
    /// Returns resource info that will be displayed to players.
    /// Only generators that are resource nodes should return this.
    /// </summary>
    TerrainFeatureResourceInfo? GetResourceInfo();

    /// <summary>
    /// Generates all tiles in the given area.
    /// Note: This method may be invoked from multiple threads simultaneously and must be thread safe.
    /// </summary>
    void GenerateChunkThreadSafe(TerrainGeneratorChunkData data);

    /// <summary>Set by terrain generator.</summary>
    TimeSpan LastGenerationTime { get; set; }
  }
}
