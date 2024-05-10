// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Economy.AssetTransactionManagerExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Economy
{
  public static class AssetTransactionManagerExtensions
  {
    /// <summary>
    /// Adds assets to compatible storages with free capacity. This almost never fails (unless Shipyard is lost).
    /// </summary>
    public static void StoreValue(
      this IAssetTransactionManager atm,
      AssetValue income,
      CreateReason? createReason)
    {
      foreach (ProductQuantity product in income.Products)
        atm.StoreProduct(product, createReason);
    }

    /// <summary>
    /// WARNING: Always prefer ClearAndDestroyBuffer if you need to destroy the buffer as well
    /// because it properly handles GlobalInputBuffers.
    /// </summary>
    /// <param name="atm"></param>
    /// <param name="buffer"></param>
    public static void ClearBuffer(this IAssetTransactionManager atm, IProductBuffer buffer)
    {
      if (buffer.Product.Type == VirtualProductProto.ProductType)
      {
        Log.Error("Cannot clear virtual buffer");
      }
      else
      {
        if (buffer.IsEmpty())
          return;
        Quantity quantity = buffer.RemoveAsMuchAs(Quantity.MaxValue);
        atm.StoreClearedProduct(buffer.Product.WithQuantity(quantity));
        Assert.That<bool>(buffer.IsEmpty()).IsTrue();
      }
    }

    public static void ClearAndDestroyBuffer(
      this IAssetTransactionManager atm,
      IProductBuffer buffer)
    {
      if (buffer.Product.Type == VirtualProductProto.ProductType)
        Log.Error("Cannot clear virtual buffer");
      else if (buffer.IsEmpty())
      {
        if (!(buffer is ProductBuffer productBuffer))
          return;
        productBuffer.Destroy();
      }
      else
      {
        Quantity quantity = buffer.RemoveAsMuchAs(Quantity.MaxValue);
        if (buffer is ProductBuffer productBuffer)
          productBuffer.Destroy();
        atm.StoreClearedProduct(buffer.Product.WithQuantity(quantity));
        Assert.That<bool>(buffer.IsEmpty()).IsTrue();
      }
    }

    public static void AddClearedProduct(this IAssetTransactionManager atm, AssetValue value)
    {
      foreach (ProductQuantity product in value.Products)
        atm.StoreClearedProduct(product);
    }
  }
}
