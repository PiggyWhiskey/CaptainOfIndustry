// Decompiled with JetBrains decompiler
// Type: Mafi.PartialQuantityLargeExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi
{
  public static class PartialQuantityLargeExtensions
  {
    public static Mafi.PartialQuantityLarge AsPartialQuantityLarge(this Quantity value)
    {
      return new Mafi.PartialQuantityLarge(value);
    }

    public static Mafi.PartialQuantityLarge AsPartialQuantityLarge(this PartialQuantity value)
    {
      return new Mafi.PartialQuantityLarge(value.Value.ToFix64());
    }

    public static Mafi.PartialQuantityLarge PartialQuantityLarge(this int value)
    {
      return new Mafi.PartialQuantityLarge(value.Quantity());
    }

    public static Mafi.PartialQuantityLarge AsPartial(this QuantityLarge value)
    {
      return new Mafi.PartialQuantityLarge(value);
    }
  }
}
