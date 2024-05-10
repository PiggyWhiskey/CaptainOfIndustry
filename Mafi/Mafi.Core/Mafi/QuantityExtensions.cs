// Decompiled with JetBrains decompiler
// Type: Mafi.QuantityExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Products;

#nullable disable
namespace Mafi
{
  public static class QuantityExtensions
  {
    public static Mafi.Quantity Quantity(this int value) => new Mafi.Quantity(value);

    public static ProductQuantity Of(this int value, ProductProto product)
    {
      return new ProductQuantity(product, new Mafi.Quantity(value));
    }

    public static void AddQuantity<TKey>(
      this Dict<TKey, Mafi.Quantity> dict,
      TKey key,
      Mafi.Quantity quantity)
    {
      Mafi.Quantity quantity1;
      if (dict.TryGetValue(key, out quantity1))
        dict[key] = quantity1 + quantity;
      else
        dict[key] = quantity;
    }
  }
}
