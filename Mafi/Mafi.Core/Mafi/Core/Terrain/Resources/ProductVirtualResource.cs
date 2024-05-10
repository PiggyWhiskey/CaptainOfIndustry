// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Resources.ProductVirtualResource
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Terrain.Resources
{
  /// <summary>
  /// Contains information about one virtual resource of products in one tile for resource visualization.
  /// </summary>
  public struct ProductVirtualResource
  {
    /// <summary>The actual product.</summary>
    public readonly VirtualResourceProductProto Product;
    /// <summary>
    /// Virtual thickness of the product in a single tile, virtual resources do not have real thickness
    /// in the terrain.
    /// </summary>
    public readonly ThicknessTilesF VirtualThickness;

    public ProductVirtualResource(
      VirtualResourceProductProto product,
      ThicknessTilesF virtualThickness)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Product = product;
      this.VirtualThickness = virtualThickness;
    }
  }
}
