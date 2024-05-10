// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ICellVirtualResourceFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Map;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface ICellVirtualResourceFactory : IResourceGeneratorFactory
  {
    /// <summary>
    /// Returns total number of resources that should be spawned on the terrain cells using <see cref="M:Mafi.Core.Terrain.Generation.ICellVirtualResourceFactory.Spawn(Mafi.Core.Map.MapCell,Mafi.Fix32)" />
    /// according to probabilities given by <see cref="M:Mafi.Core.Terrain.Generation.ICellVirtualResourceFactory.GetSpawnProbability(Mafi.Core.Map.MapCell,System.Boolean)" />. This is queried only once before any
    /// generators are spawned.
    /// </summary>
    /// <remarks>
    /// It may happen that not all factories can be spawned. Spawning is probabilistic with limited number of trials.
    /// </remarks>
    int GetResourcesCount(ImmutableArray<MapCell> cells);

    /// <summary>
    /// Returns probability of spawning this resource on given cell.
    /// </summary>
    Percent GetSpawnProbability(MapCell cell, bool isInitialResource);

    /// <summary>
    /// Creates resource generator for given cell. The multiplier should control amount of the resource. Value 2.0
    /// should spawn generator that yields roughly 2x of the resource.
    /// IMPORTANT: The multiplier already captures compensation for the cell size, distance from the original cell,
    /// and player settings.
    /// </summary>
    IVirtualTerrainResource Spawn(MapCell cell, Fix32 resourceMult);
  }
}
