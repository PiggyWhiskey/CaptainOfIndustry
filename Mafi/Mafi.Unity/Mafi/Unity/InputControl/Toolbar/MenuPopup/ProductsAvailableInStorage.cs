// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.ProductsAvailableInStorage
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Products;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup
{
  public class ProductsAvailableInStorage : IAvailableProductsProvider
  {
    private readonly IAssetTransactionManager m_assetTransactionManager;

    public ProductsAvailableInStorage(IAssetTransactionManager assetTransactionManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetTransactionManager = assetTransactionManager;
    }

    public Quantity GetAvailableProductQuantity(ProductProto product)
    {
      return this.m_assetTransactionManager.GetAvailableQuantityForRemoval(product);
    }
  }
}
