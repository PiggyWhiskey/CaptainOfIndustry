// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.PricePanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup
{
  public class PricePanel : IUiElement, IDynamicSizeElement
  {
    private readonly UiBuilder m_builder;
    private readonly PricePanelUiStyle.PricePanelStyle m_style;
    private readonly Option<IAvailableProductsProvider> m_availableProductsProvider;
    private readonly StackContainer m_priceStack;
    private readonly ViewsCacheHomogeneous<IconContainer> m_plusCache;
    private readonly Lyst<PricePanel.SingleProductPriceContainer> m_productPrices;
    private readonly Lyst<ProductQuantity> m_availableProductsCache;
    private AssetValue m_price;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_priceStack.GameObject;

    public RectTransform RectTransform => this.m_priceStack.RectTransform;

    public float PreferredHeight => this.m_builder.Style.ProductWithIcon.Size.y;

    public float PreferredCompactHeight => 60f;

    public AssetValue CurrentPrice => this.m_price;

    public PricePanel(
      UiBuilder builder,
      PricePanelUiStyle.PricePanelStyle style,
      Option<IAvailableProductsProvider> availableProductsProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_productPrices = new Lyst<PricePanel.SingleProductPriceContainer>();
      this.m_availableProductsCache = new Lyst<ProductQuantity>();
      this.m_price = AssetValue.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_style = style;
      this.m_availableProductsProvider = availableProductsProvider;
      this.m_priceStack = builder.NewStackContainer("Price").SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetStackingDirection(StackContainer.Direction.LeftToRight).SetInnerPadding(style.InnerPadding).OnMouseEnter(new Action(this.onPanelEnter)).OnMouseLeave(new Action(this.onPanelLeave)).SetHeight<StackContainer>(this.PreferredHeight);
      this.m_plusCache = new ViewsCacheHomogeneous<IconContainer>((Func<IconContainer>) (() => this.m_builder.NewIconContainer("Plus").SetIcon(new IconStyle(this.m_builder.Style.Icons.Plus, new ColorRgba?(ColorRgba.White)))));
    }

    public void SetPrice(AssetValue price, TextStyle? labelStyle = null)
    {
      if (this.m_price == price)
        return;
      this.m_price = price;
      this.m_priceStack.StartBatchOperation();
      this.m_priceStack.Show<StackContainer>();
      this.m_priceStack.ClearAll(true);
      this.m_plusCache.ReturnAll();
      if (price.IsEmpty)
      {
        this.m_priceStack.FinishBatchOperation();
      }
      else
      {
        bool flag = false;
        int index = 0;
        foreach (ProductQuantity product in price.Products)
        {
          if (flag)
          {
            IconContainer view = this.m_plusCache.GetView();
            view.Show<IconContainer>();
            view.AppendTo<IconContainer>(this.m_priceStack, new Vector2?(new Vector2(this.m_style.PlusSignSize, this.m_style.PlusSignSize)), ContainerPosition.MiddleOrCenter, Offset.LeftRight(this.m_style.PlusSignMargin));
          }
          PricePanel.SingleProductPriceContainer productPriceContainer;
          if (index < this.m_productPrices.Count)
          {
            productPriceContainer = this.m_productPrices[index];
          }
          else
          {
            productPriceContainer = new PricePanel.SingleProductPriceContainer((IUiElement) this.m_priceStack, this.m_builder, this.m_availableProductsProvider.HasValue);
            this.m_productPrices.Add(productPriceContainer);
          }
          productPriceContainer.SetLabelTextStyle(labelStyle ?? this.m_style.TextStyle);
          productPriceContainer.SetProductQuantity(product);
          if (this.m_availableProductsProvider.HasValue)
          {
            ProductQuantity availableProductQuantity = product.WithNewQuantity(this.m_availableProductsProvider.Value.GetAvailableProductQuantity(product.Product));
            productPriceContainer.SetAvailableProductQuantity(availableProductQuantity);
          }
          productPriceContainer.AppendTo(this.m_priceStack);
          flag = true;
          ++index;
        }
        this.m_priceStack.FinishBatchOperation();
        Action<IUiElement> sizeChanged = this.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) this);
      }
    }

    public IUiUpdater CreateUpdater()
    {
      return UpdaterBuilder.Start().Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
      {
        this.m_availableProductsCache.Clear();
        if (this.m_availableProductsProvider.IsNone)
          return (IIndexable<ProductQuantity>) this.m_availableProductsCache;
        foreach (ProductQuantity product in this.m_price.Products)
          this.m_availableProductsCache.Add(product.WithNewQuantity(this.m_availableProductsProvider.Value.GetAvailableProductQuantity(product.Product)));
        return (IIndexable<ProductQuantity>) this.m_availableProductsCache;
      }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Do((Action<Lyst<ProductQuantity>>) (availableProducts =>
      {
        for (int index = 0; index < availableProducts.Count && index < this.m_price.Products.Length; ++index)
        {
          if (availableProducts[index].Product.IsExcludedFromStats)
            this.m_productPrices[index].SetEmptyProductName();
          else
            this.m_productPrices[index].SetAvailableProductQuantity(availableProducts[index]);
        }
        Action<IUiElement> sizeChanged = this.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) this);
      })).Build();
    }

    public float GetDynamicWidth() => this.m_priceStack.GetDynamicWidth();

    private void onPanelEnter()
    {
      foreach (PricePanel.SingleProductPriceContainer productPrice in this.m_productPrices)
        productPrice.ShowProductName();
    }

    private void onPanelLeave()
    {
      foreach (PricePanel.SingleProductPriceContainer productPrice in this.m_productPrices)
        productPrice.HideProductName();
    }

    private class SingleProductPriceContainer
    {
      private readonly bool m_isCompactModeOn;
      private readonly UiStyle m_style;
      private readonly Txt m_quantityLabel;
      private readonly ProtoWithIcon<ProductProto> m_protoWithIcon;
      private TitleTooltip m_nameTooltip;

      public ProductQuantity? AvailableProductQuantity { get; private set; }

      public SingleProductPriceContainer(
        IUiElement parent,
        UiBuilder builder,
        bool isCompactModeOn)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_isCompactModeOn = isCompactModeOn;
        this.m_style = builder.Style;
        this.m_quantityLabel = builder.NewTxt("Quantity", parent).SetTextStyle(this.m_style.Global.Title.Extend(new ColorRgba?(this.m_style.Global.BlueForDark))).AllowHorizontalOverflow().SetAlignment(TextAnchor.MiddleLeft);
        this.m_nameTooltip = new TitleTooltip(builder);
        this.m_nameTooltip.SetMaxWidthOverflow(40);
        this.m_protoWithIcon = new ProtoWithIcon<ProductProto>(parent, builder);
      }

      public void SetProductQuantity(ProductQuantity productQuantity)
      {
        this.m_quantityLabel.SetText(productQuantity.FormatNumberAndUnitOnly());
        this.m_protoWithIcon.SetProto((Option<ProductProto>) productQuantity.Product);
        this.m_protoWithIcon.SetCustomIconText("");
        this.m_nameTooltip.SetText((LocStrFormatted) productQuantity.Product.Strings.Name);
      }

      public void SetLabelTextStyle(TextStyle textStyle)
      {
        this.m_quantityLabel.SetTextStyle(textStyle);
      }

      public void SetAvailableProductQuantity(ProductQuantity availableProductQuantity)
      {
        if (this.m_isCompactModeOn)
          this.m_protoWithIcon.ReplaceNameWithQuantity(availableProductQuantity.Quantity);
        else
          this.m_protoWithIcon.AppendQuantityAfterName(availableProductQuantity.Quantity);
      }

      public void SetEmptyProductName() => this.m_protoWithIcon.SetCustomIconText("");

      public void AppendTo(StackContainer container)
      {
        this.m_quantityLabel.Show<Txt>();
        float preferedWidth = this.m_quantityLabel.GetPreferedWidth();
        this.m_quantityLabel.AppendTo<Txt>(container, new float?(preferedWidth));
        this.m_protoWithIcon.Show<ProtoWithIcon<ProductProto>>();
        float num = this.m_isCompactModeOn ? 40f : this.m_style.ProductWithIcon.Size.y;
        this.m_protoWithIcon.AppendTo<ProtoWithIcon<ProductProto>>(container, new float?(num));
      }

      public void ShowProductName()
      {
        if (!this.m_protoWithIcon.IsVisible())
          return;
        this.m_nameTooltip.Show((IUiElement) this.m_protoWithIcon);
      }

      public void HideProductName() => this.m_nameTooltip.Hide();
    }
  }
}
