// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.ProductBufferExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Products;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public static class ProductBufferExtensions
  {
    /// <summary>How full the buffer is in percent.</summary>
    public static Percent PercentFull(this IProductBuffer buffer)
    {
      return Percent.FromRatio(buffer.Quantity.Min(buffer.Capacity).Value, buffer.Capacity.Value);
    }

    /// <summary>
    /// Returns true if given quantity can be stored to this buffer.
    /// </summary>
    public static bool CanStore(this IProductBuffer buffer, Quantity quantity)
    {
      return buffer.UsableCapacity >= quantity;
    }

    /// <summary>
    /// Returns true if requested quantity can be removed from this buffer.
    /// </summary>
    public static bool CanRemove(this IProductBuffer buffer, Quantity quantity)
    {
      return buffer.Quantity >= quantity;
    }

    /// <summary>
    /// Removes the given amount from the buffer and fires assert if failed. Use this only when you are sure that you
    /// can remove the quantity.
    /// </summary>
    public static void RemoveExactly(this IProductBuffer buffer, Quantity quantity)
    {
      Assert.That<Quantity>(buffer.RemoveAsMuchAs(quantity)).IsEqualTo(quantity);
    }

    /// <summary>
    /// Removes as much quantity as possible with regards to given max quantity constraint. Returns quantity
    /// that could not be removed.
    /// </summary>
    public static Quantity RemoveAsMuchAsReturnLeft(
      this IProductBuffer buffer,
      Quantity maxQuantity)
    {
      return maxQuantity - buffer.RemoveAsMuchAs(maxQuantity);
    }

    /// <summary>
    /// Stores the given amount to the buffer and fires assert if failed. Use this only when you are sure that you
    /// can store the quantity.
    /// </summary>
    public static void StoreExactly(this IProductBuffer buffer, Quantity quantity)
    {
      Assert.That<Quantity>(buffer.StoreAsMuchAs(quantity)).IsZero();
    }

    /// <summary>
    /// Tries to remove given amount of quantity. Returns true if requested quantity was removed, otherwise false.
    /// </summary>
    public static bool TryRemove(this IProductBuffer buffer, Quantity quantity)
    {
      if (!buffer.CanRemove(quantity))
        return false;
      buffer.RemoveExactly(quantity);
      return true;
    }

    /// <summary>
    /// Stores as much quantity from given <see cref="T:Mafi.Core.ProductQuantity" /> as possible. Returns quantity that was not
    /// able to fit to this buffer wrapped in <see cref="T:Mafi.Core.ProductQuantity" /> with corresponding product.
    /// </summary>
    public static Quantity StoreAsMuchAs(
      this IProductBuffer buffer,
      ProductQuantity productQuantity)
    {
      Assert.That<ProductProto>(buffer.Product).IsEqualTo<ProductProto>(productQuantity.Product);
      return buffer.StoreAsMuchAs(productQuantity.Quantity);
    }

    /// <summary>
    /// Stores as much quantity from given <see cref="T:Mafi.Core.ProductQuantity" /> as possible. Returns quantity that was stored.
    /// </summary>
    public static Quantity StoreAsMuchAsReturnStored(this IProductBuffer buffer, Quantity quantity)
    {
      return quantity - buffer.StoreAsMuchAs(quantity);
    }

    public static void StoreAsMuchAsFrom(
      this IProductBuffer buffer,
      IProductBuffer sourceBuffer,
      Quantity maxQuantity)
    {
      if ((Proto) buffer.Product != (Proto) sourceBuffer.Product)
      {
        Log.Error(string.Format("Transfer between incompatible products {0} and {1}", (object) buffer.Product, (object) sourceBuffer.Product));
      }
      else
      {
        maxQuantity = maxQuantity.Min(buffer.UsableCapacity);
        buffer.StoreExactly(sourceBuffer.RemoveAsMuchAs(maxQuantity));
      }
    }
  }
}
