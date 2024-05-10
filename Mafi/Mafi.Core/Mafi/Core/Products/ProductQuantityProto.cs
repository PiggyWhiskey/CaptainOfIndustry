// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.ProductQuantityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Products
{
  public struct ProductQuantityProto
  {
    public readonly ProductProto Product;
    public readonly Quantity Quantity;

    public ProductQuantityProto(ProductProto product, Quantity quantity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Product = product.CheckNotNull<ProductProto>();
      this.Quantity = quantity.CheckNotNegative();
    }
  }
}
