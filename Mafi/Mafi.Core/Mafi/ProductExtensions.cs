// Decompiled with JetBrains decompiler
// Type: Mafi.ProductExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core;
using Mafi.Core.Products;

#nullable disable
namespace Mafi
{
  public static class ProductExtensions
  {
    /// <summary>
    /// See also <see cref="M:Mafi.QuantityExtensions.Of(System.Int32,Mafi.Core.Products.ProductProto)" /> which is even shorter.
    /// </summary>
    public static ProductQuantity WithQuantity(this ProductProto product, Quantity quantity)
    {
      return new ProductQuantity(product, quantity);
    }

    public static ProductQuantity WithQuantity(this ProductProto product, int quantity)
    {
      return new ProductQuantity(product, new Quantity(quantity));
    }

    public static ProductQuantityLarge WithQuantity(
      this ProductProto product,
      QuantityLarge quantity)
    {
      return new ProductQuantityLarge(product, quantity);
    }
  }
}
