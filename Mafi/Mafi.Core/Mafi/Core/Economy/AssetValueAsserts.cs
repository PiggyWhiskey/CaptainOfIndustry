// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Economy.AssetValueAsserts
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Core.Economy
{
  public static class AssetValueAsserts
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void HasAllValuesPositive(this Assertion<AssetValue> actual, string message = "")
    {
      foreach (ProductQuantity product in actual.Value.Products)
      {
        if (!product.Quantity.IsPositive)
        {
          Mafi.Assert.FailAssertion("AssetValue has some non-positive values: " + actual.Value.Products.ToLyst().Where<ProductQuantity>((Func<ProductQuantity, bool>) (x => x.Quantity.IsNotPositive)).Select<ProductQuantity, string>((Func<ProductQuantity, string>) (x => x.ToString())).JoinStrings(", "), message);
          break;
        }
      }
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void HasAllValuesPositive<T0>(
      this Assertion<AssetValue> actual,
      string message,
      T0 arg0)
    {
      foreach (ProductQuantity product in actual.Value.Products)
      {
        if (!product.Quantity.IsPositive)
        {
          Mafi.Assert.FailAssertion("AssetValue has some non-positive values: " + actual.Value.Products.ToLyst().Where<ProductQuantity>((Func<ProductQuantity, bool>) (x => x.Quantity.IsNotPositive)).Select<ProductQuantity, string>((Func<ProductQuantity, string>) (x => x.ToString())).JoinStrings(", "), message.FormatInvariant((object) arg0));
          break;
        }
      }
    }
  }
}
