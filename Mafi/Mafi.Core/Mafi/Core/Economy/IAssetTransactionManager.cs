// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Economy.IAssetTransactionManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;

#nullable disable
namespace Mafi.Core.Economy
{
  /// <summary>
  /// Facilitates spending and earning of both money and products.
  /// </summary>
  public interface IAssetTransactionManager
  {
    /// <summary>
    /// Returns how much is available for global removal. Thread safe.
    /// IMPORTANT: This is might be not 100% reliable if some entity misreports its data.
    /// But it is fast and great for UI.
    /// If you need to be 100% certain, use <see cref="M:Mafi.Core.Economy.IAssetTransactionManager.CanRemoveProduct(Mafi.Core.ProductQuantity)" /> or
    /// <see cref="M:Mafi.Core.Economy.IAssetTransactionManager.TryRemoveProduct(Mafi.Core.ProductQuantity,System.Nullable{Mafi.Core.Products.DestroyReason})" />
    /// </summary>
    Quantity GetAvailableQuantityForRemoval(ProductProto productProto);

    /// <summary>
    /// Returns whether the player can spend given assets.
    /// IMPORTANT: This is slow, don't use frequently in UI.
    /// </summary>
    bool CanRemoveProduct(ProductQuantity price);

    /// <summary>
    /// Removes given assets from storages.
    /// Also removes them from product manager case reason is not null.
    /// </summary>
    bool TryRemoveProduct(ProductQuantity product, DestroyReason? reason);

    /// <summary>Returns what was removed.</summary>
    Quantity RemoveAsMuchAs(ProductQuantity pq, DestroyReason? reason);

    /// <summary>
    /// Stores the given products in compatible storages with free capacity. If the player
    /// has not enough storage capacity the products end up in overflow storage, which is typically
    /// a shipyard.
    /// 
    /// If reason is provided we also report the products to product manager.
    /// </summary>
    void StoreProduct(ProductQuantity productQuantity, CreateReason? reason);

    /// <summary>
    /// Add output buffer that serves as global provider of products where needed.
    /// For priority use <see cref="T:Mafi.Core.Economy.GlobalOutputPriority" />
    /// </summary>
    void AddGlobalOutput(IProductBuffer buffer, int priority, Option<IEntity> entity);

    void RemoveGlobalOutput(IProductBuffer buffer);

    /// <summary>
    /// Add input buffer that serves as global receiver of products.
    /// For priority use <see cref="T:Mafi.Core.Economy.GlobalInputPriority" />
    /// </summary>
    void AddGlobalInput(IProductBuffer buffer, int priority, Option<IEntity> entity);

    void RemoveGlobalInput(IProductBuffer buffer);

    void StoreClearedProduct(ProductQuantity productQuantity);

    /// <summary>
    /// Can be called multiple times to replace shipyard (e.g. due upgrade).
    /// </summary>
    void AddOverflowProductsStorage(IOverflowProductsStorage shipyard);

    void TryRemoveOverflowStorage(IOverflowProductsStorage shipyard);
  }
}
