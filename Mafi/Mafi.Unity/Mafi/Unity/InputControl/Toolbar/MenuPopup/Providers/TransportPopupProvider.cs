// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers.TransportPopupProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Products;
using Mafi.Localization;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TransportPopupProvider : PopupProviderBase<TransportProto>
  {
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly Dict<ProductType, Lyst<ProductProto>> m_productsPerType;

    public TransportPopupProvider(UnlockedProtosDbForUi unlockedProtosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_productsPerType = new Dict<ProductType, Lyst<ProductProto>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_unlockedProtosDb = unlockedProtosDb;
      unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.onUnlockedSetChangedForUi);
      this.refreshUnlockedProducts();
    }

    private void onUnlockedSetChangedForUi()
    {
      foreach (Lyst<ProductProto> lyst in this.m_productsPerType.Values)
        lyst.Clear();
      foreach (ProductProto productProto in this.m_unlockedProtosDb.AllUnlocked<ProductProto>())
        this.m_productsPerType.GetOrAdd<ProductType, Lyst<ProductProto>>(productProto.Type, (Func<ProductType, Lyst<ProductProto>>) (_ => new Lyst<ProductProto>())).Add(productProto);
    }

    private void refreshUnlockedProducts()
    {
      foreach (Lyst<ProductProto> lyst in this.m_productsPerType.Values)
        lyst.Clear();
      foreach (ProductProto productProto in this.m_unlockedProtosDb.AllUnlocked<ProductProto>())
        this.m_productsPerType.GetOrAdd<ProductType, Lyst<ProductProto>>(productProto.Type, (Func<ProductType, Lyst<ProductProto>>) (_ => new Lyst<ProductProto>())).Add(productProto);
    }

    protected override void PopulateView(
      MenuPopupView view,
      TransportProto proto,
      bool isForResearch)
    {
      view.SetTitle((LocStrFormatted) proto.Strings.Name);
      view.SetPricePerTile(proto.Costs.Price);
      view.SetDescription((LocStrFormatted) proto.Strings.DescShort);
      view.SetThroughput(proto.ThroughputPerTick);
      Lyst<ProductProto> products;
      if (!this.m_productsPerType.TryGetValue(proto.PortsShape.AllowedProductType, out products))
        return;
      view.SetProducts(products, (LocStrFormatted) Tr.SupportedProducts);
    }
  }
}
