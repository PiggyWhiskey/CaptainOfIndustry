// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IProductBufferReadOnlyExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public static class IProductBufferReadOnlyExtensions
  {
    public static bool IsFull(this IProductBufferReadOnly buffer)
    {
      return buffer.UsableCapacity.IsNotPositive;
    }

    public static bool IsNotFull(this IProductBufferReadOnly buffer)
    {
      return buffer.UsableCapacity.IsPositive;
    }

    public static bool IsEmpty(this IProductBufferReadOnly buffer) => buffer.Quantity.IsNotPositive;

    public static bool IsNotEmpty(this IProductBufferReadOnly buffer) => buffer.Quantity.IsPositive;

    public static Mafi.Core.ProductQuantity ProductQuantity(this IProductBufferReadOnly buffer)
    {
      return buffer.Product.WithQuantity(buffer.Quantity);
    }
  }
}
