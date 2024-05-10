// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.IWorldRegionMap
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface IWorldRegionMap
  {
    RelTile2i Size { get; }

    TerrainMaterialProto BedrockMaterial { get; }

    MapEdgeType MapEdgeType { get; }

    MapOffLimitsSize OffLimitsSize { get; }

    IIndexable<ITerrainFeatureGenerator> TerrainFeatureGenerators { get; }

    IIndexable<ITerrainPostProcessorV2> TerrainPostProcessors { get; }

    IIndexable<IVirtualTerrainResourceGenerator> VirtualResourcesGenerators { get; }

    IIndexable<IStartingLocationV2> StartingLocations { get; }

    IEnumerable<ITerrainFeatureBase> EnumerateAllFeatures();
  }
}
