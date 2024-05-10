// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.VehicleTooltip
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  public class VehicleTooltip : TooltipBase
  {
    private readonly PricePanel m_price;
    private readonly Txt m_textView;
    private readonly TextWithIcon m_fuelWithQuantity;
    private IUiElement m_parent;
    private float m_finalWidth;
    private float m_finalHeight;

    public VehicleTooltip(
      UiBuilder builder,
      IAssetTransactionManager assetsManager,
      PricePanelUiStyle.PricePanelStyle panelStyle = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(builder);
      this.m_price = new PricePanel(builder, panelStyle ?? builder.Style.PricePanel.VehiclesBuyPricePanelStyle, (Option<IAvailableProductsProvider>) (IAvailableProductsProvider) new ProductsAvailableInStorage(assetsManager));
      this.m_price.PutToLeftTopOf<PricePanel>((IUiElement) this.Container, new Vector2(0.0f, this.m_price.PreferredHeight), Offset.Top(10f));
      Txt txt = builder.NewTxt("text");
      TextStyle text = builder.Style.Global.Text;
      ref TextStyle local = ref text;
      int? nullable = new int?(15);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_textView = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleLeft).PutToTopOf<Txt>((IUiElement) this.Container, 0.0f);
      this.m_fuelWithQuantity = new TextWithIcon(this.Builder).SetSuffixIcon("Assets/Unity/UserInterface/General/Clock.svg").SetSuffixText("/ 60").PutToLeftTopOf<TextWithIcon>((IUiElement) this.Container, 0.Vector2());
    }

    public VehicleTooltip SetData(
      AssetValue cost,
      LocStrFormatted text,
      PartialProductQuantity fuelConsumed)
    {
      this.m_price.SetPrice(cost);
      this.m_textView.SetText(text);
      string stringRounded = fuelConsumed.Quantity.ToStringRounded(1);
      this.m_fuelWithQuantity.SetPrefixText(string.Format("{0}: {1}", (object) Tr.Consumption, (object) stringRounded));
      this.m_fuelWithQuantity.SetIcon(fuelConsumed.Product.Graphics.IconPath);
      float dynamicWidth = this.m_price.GetDynamicWidth();
      this.m_price.SetWidth<PricePanel>(this.m_price.GetDynamicWidth());
      float width1 = this.m_fuelWithQuantity.GetWidth();
      float width2 = 300f.Max(dynamicWidth).Max(width1);
      float preferedHeight = this.m_textView.GetPreferedHeight(width2);
      if ((double) preferedHeight < 50.0)
        width2 = this.m_textView.GetPreferedWidth();
      this.m_textView.PutToTopOf<Txt>((IUiElement) this.Container, preferedHeight, Offset.LeftRight(10f) + Offset.Top(this.m_price.GetHeight() + 10f));
      this.m_fuelWithQuantity.PutToLeftTopOf<TextWithIcon>((IUiElement) this.Container, new Vector2(this.m_fuelWithQuantity.GetWidth(), 25f), Offset.Top((float) ((double) this.m_price.GetHeight() + (double) preferedHeight + 10.0 + 5.0)) + Offset.Left(10f));
      this.m_finalWidth = dynamicWidth.Max(width2) + 20f;
      this.m_finalHeight = (float) ((double) this.m_price.GetHeight() + (double) this.m_fuelWithQuantity.GetHeight() + (double) preferedHeight + 20.0 + 5.0);
      this.m_fuelWithQuantity.SetVisibility<TextWithIcon>(fuelConsumed.IsNotEmpty);
      return this;
    }

    public IUiUpdater CreateUpdater() => this.m_price.CreateUpdater();

    private void OnParentMouseEnter()
    {
      if (this.m_price.CurrentPrice.IsEmpty)
        return;
      this.PositionSelf(this.m_parent, this.m_finalWidth, this.m_finalHeight);
    }

    public void AttachTo<T>(IUiElementWithHover<T> element)
    {
      this.m_parent = (IUiElement) element;
      element.SetOnMouseEnterLeaveActions(new Action(this.OnParentMouseEnter), new Action(((TooltipBase) this).onParentMouseLeave));
    }
  }
}
