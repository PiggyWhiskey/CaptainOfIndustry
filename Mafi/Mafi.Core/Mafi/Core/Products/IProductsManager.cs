// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.IProductsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Products
{
  public interface IProductsManager
  {
    IAssetTransactionManager AssetManager { get; }

    ImmutableArray<Mafi.Core.Products.ProductStats> ProductStats { get; }

    ProductsSlimIdManager SlimIdManager { get; }

    /// <summary>
    /// Informs the manager that new quantity of the given product was added to the world.
    /// </summary>
    void ProductCreated(ProductProto product, Quantity quantity, CreateReason reason);

    /// <summary>
    /// Informs the manager of products transformation. Typically done by a machine. All quantities will get
    /// reported and also source products get transferred.
    /// </summary>
    void ReportProductsTransformation(
      IIndexable<ProductQuantity> inputs,
      IIndexable<ProductQuantity> outputs,
      DestroyReason destroyReason,
      CreateReason createReason,
      bool disableSourceProductsConversionLoss = false);

    /// <summary>
    /// This will remove the product from global quantity and remove its proportional source products
    /// and returns them.
    /// </summary>
    void DestroyProductReturnRemovedSourceProducts(
      ProductProto product,
      Quantity quantity,
      DestroyReason reason,
      Lyst<KeyValuePair<ProductProto, PartialQuantityLarge>> result);

    /// <summary>
    /// Informs the manager that new quantity of the given product was added to the world.
    /// This one also adds its source products.
    /// </summary>
    void ProductCreated(
      ProductProto product,
      Quantity quantity,
      IIndexable<ProductQuantity> sources,
      CreateReason reason);

    /// <summary>
    /// Informs the manager that quantity of the given product was removed from the world. Also handles money
    /// changes.
    /// </summary>
    void ProductDestroyed(ProductProto product, Quantity quantity, DestroyReason reason);

    void ProductDestroyed(ProductSlimId slimId, Quantity quantity, DestroyReason reason);

    /// <summary>
    /// Whether the product can be cleared from storage, transports etc. If not such product cannot be destroyed
    /// (e.g. Nuclear waste).
    /// </summary>
    bool CanBeCleared(ProductProto product);

    /// <summary>
    /// Checks <see cref="M:Mafi.Core.Products.IProductsManager.CanBeCleared(Mafi.Core.Products.ProductProto)" /> and if true it removes the quantity from its statistics and handles
    /// payments.
    /// </summary>
    void ClearProduct(ProductProto product, Quantity quantity);

    /// <summary>
    /// Clears product immediately without checks as to whether the product can be cleared.
    /// </summary>
    /// <remarks>
    /// Usable for deleting products that are not visible to the user - like Machine input buffers.
    /// </remarks>
    void ClearProductNoChecks(ProductProto product, Quantity quantity);

    /// <summary>
    /// Informs the manager that storage capacity for the given product has changed by the given quantity. Can be
    /// negative, in such case the capacity was decreased.
    /// </summary>
    void ReportStorageCapacityChange(ProductProto product, Quantity quantity);

    /// <summary>
    /// Returns statistics for the given product. These are kept up to date no need to requery them every time.
    /// Thread safe.
    /// </summary>
    Mafi.Core.Products.ProductStats GetStatsFor(ProductProto product);

    void IncreaseRecyclingRatio(Percent percent);
  }
}
