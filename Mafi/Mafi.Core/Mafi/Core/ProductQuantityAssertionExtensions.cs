// Decompiled with JetBrains decompiler
// Type: Mafi.Core.ProductQuantityAssertionExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using System.Diagnostics;

#nullable disable
namespace Mafi.Core
{
  public static class ProductQuantityAssertionExtensions
  {
    [Conditional("MAFI_ASSERTIONS")]
    public static void IsEmpty(this Assertion<ProductQuantity> value, string message = "")
    {
      if (value.Value.Quantity.IsZero)
        return;
      Mafi.Assert.FailAssertion(string.Format("Product quantity '{0}' is not empty.", (object) value.Value.Product), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotEmpty(this Assertion<ProductQuantity> value, string message = "")
    {
      if (value.Value.Product.IsPhantom)
        Mafi.Assert.FailAssertion("Product quantity is Phantom.", message);
      if (!value.Value.Quantity.IsNotPositive)
        return;
      Mafi.Assert.FailAssertion(string.Format("Product quantity '{0}' is empty.", (object) value.Value.Product), message);
    }

    [Conditional("MAFI_ASSERTIONS")]
    public static void IsNotPhantom(this Assertion<ProductQuantity> value, string message = "")
    {
      if (!value.Value.Product.IsPhantom)
        return;
      Mafi.Assert.FailAssertion("Product quantity is Phantom.", message);
    }
  }
}
