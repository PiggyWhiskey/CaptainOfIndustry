// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Resources.TerrainTileResources
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;

#nullable disable
namespace Mafi.Core.Terrain.Resources
{
  /// <summary>
  /// Contains all the given (filtered) products in the given tile.
  /// </summary>
  public struct TerrainTileResources
  {
    /// <summary>Tile whose products this class contains.</summary>
    public readonly TerrainTile Tile;
    /// <summary>
    /// All the given products in <see cref="F:Mafi.Core.Terrain.Resources.TerrainTileResources.Tile" />. Ordered from the shallowest to the deepest.
    /// </summary>
    public readonly ImmutableArray<ProductResource> Products;
    /// <summary>
    /// All products with virtual resources on <see cref="F:Mafi.Core.Terrain.Resources.TerrainTileResources.Tile" />.
    /// </summary>
    public readonly ImmutableArray<ProductVirtualResource> VirtualResources;

    public TerrainTileResources(
      TerrainTile tile,
      ImmutableArray<ProductResource> products,
      ImmutableArray<ProductVirtualResource> virtualResources)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Tile = tile;
      this.Products = products.CheckNotDefaultStruct<ImmutableArray<ProductResource>>();
      this.VirtualResources = virtualResources.CheckNotDefaultStruct<ImmutableArray<ProductVirtualResource>>();
    }
  }
}
