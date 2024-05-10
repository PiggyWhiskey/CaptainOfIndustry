// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.IWorldRegionMapAdditionalData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>
  /// Additional data that are usually not needed when loading map previews.
  /// </summary>
  public interface IWorldRegionMapAdditionalData
  {
    /// <summary>Number of tiles that are not ocean (optional).</summary>
    int NonOceanTilesCount { get; }

    /// <summary>Number of tiles that are flat (optional).</summary>
    int FlatNonOceanTilesCount { get; }

    ImmutableArray<StartingLocationPreview> StartingLocations { get; }

    ImmutableArray<MapTerrainResourceStats> EasyToReachTerrainResourcesStats { get; }

    ImmutableArray<MapTerrainResourceStats> TotalTerrainResourcesStats { get; }

    /// <summary>Precomputed stats for mined products and their display names, for use when protos aren't available</summary>
    ImmutableArray<MapProductStats> EasyToReachProductStats { get; }

    /// <summary>Precomputed stats for mined products and their display names, for use when protos aren't available</summary>
    ImmutableArray<MapProductStats> TotalProductStats { get; }

    ImmutableArray<MapOtherResourceStats> EasyToReachOtherResourcesStats { get; }

    ImmutableArray<MapOtherResourceStats> TotalOtherResourcesStats { get; }

    ImmutableArray<MapResourceLocation> ResourceLocations { get; }

    /// <summary>
    /// Each entry contains number of tiles at or above certain elevation. Note that some heights might be missing
    /// if there are no tiles at that particular elevation.
    /// </summary>
    ImmutableArray<Pair<HeightTilesI, int>> TilesAtOrAboveElevationDataSorted { get; }

    /// <summary>
    /// Larger map preview that can be used for starting location selection. Encoded as JPG. TODO: Define optimal size.
    /// </summary>
    ImmutableArray<EncodedImageAndMatrix> PreviewImagesData { get; }
  }
}
