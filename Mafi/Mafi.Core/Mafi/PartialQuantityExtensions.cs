// Decompiled with JetBrains decompiler
// Type: Mafi.PartialQuantityExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core;
using Mafi.Core.Products;

#nullable disable
namespace Mafi
{
  public static class PartialQuantityExtensions
  {
    public static PartialQuantity Quantity(this Fix32 value) => new PartialQuantity(value);

    public static PartialQuantity Quantity(this float value)
    {
      return new PartialQuantity(value.ToFix32());
    }

    public static PartialQuantity Quantity(this double value)
    {
      return new PartialQuantity(value.ToFix32());
    }

    public static PartialProductQuantity Of(this double value, ProductProto product)
    {
      return new PartialProductQuantity(product, value.Quantity());
    }

    public static PartialProductQuantity Of(this PartialQuantity value, ProductProto product)
    {
      return new PartialProductQuantity(product, value);
    }
  }
}
