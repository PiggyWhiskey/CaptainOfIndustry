// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.CostTooltip
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public class CostTooltip : TooltipBase
  {
    private readonly PricePanel m_price;
    private IUiElement m_parent;

    public CostTooltip(
      UiBuilder builder,
      IAssetTransactionManager assetsManager,
      PricePanelUiStyle.PricePanelStyle panelStyle = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder);
      this.m_price = new PricePanel(builder, panelStyle ?? builder.Style.PricePanel.VehiclesBuyPricePanelStyle, (Option<IAvailableProductsProvider>) (IAvailableProductsProvider) new ProductsAvailableInStorage(assetsManager));
      this.m_price.PutToLeftOf<PricePanel>((IUiElement) this.Container, 0.0f, Offset.All(10f));
    }

    public CostTooltip SetCost(AssetValue cost)
    {
      this.m_price.SetPrice(cost);
      return this;
    }

    public IUiUpdater CreateUpdater() => this.m_price.CreateUpdater();

    private void OnParentMouseEnter()
    {
      if (this.m_price.CurrentPrice.IsEmpty)
        return;
      float height = this.m_price.PreferredHeight + 20f;
      this.PositionSelf(this.m_parent, this.m_price.GetDynamicWidth(), height);
    }

    public void AttachTo<T>(IUiElementWithHover<T> element)
    {
      this.m_parent = (IUiElement) element;
      element.SetOnMouseEnterLeaveActions(new Action(this.OnParentMouseEnter), new Action(((TooltipBase) this).onParentMouseLeave));
    }
  }
}
