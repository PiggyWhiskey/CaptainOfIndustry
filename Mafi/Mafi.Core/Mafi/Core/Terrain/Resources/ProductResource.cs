// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Resources.ProductResource
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
  /// Contains information about one terrain products in one tile for resource visualization.
  /// </summary>
  public struct ProductResource
  {
    /// <summary>The actual product.</summary>
    public readonly LooseProductProto Product;
    /// <summary>
    /// Sum of total thickness of the product in a single tile.
    /// </summary>
    public readonly ThicknessTilesF Height;
    /// <summary>Minimal depth of the product in a single tile.</summary>
    public readonly ThicknessTilesF Depth;

    public ProductResource(
      LooseProductProto product,
      ThicknessTilesF height,
      ThicknessTilesF depth)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Product = product.CheckNotNull<LooseProductProto>();
      this.Height = height.CheckPositive();
      this.Depth = depth.CheckNotNegative();
    }

    /// <summary>Adds the given thickness to a newly created object.</summary>
    public ProductResource AddHeight(ThicknessTilesF inc)
    {
      return new ProductResource(this.Product, this.Height + inc, this.Depth);
    }
  }
}
