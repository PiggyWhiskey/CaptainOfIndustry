// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.ProductQuantityWithIcon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  internal class ProductQuantityWithIcon : IUiElement
  {
    private readonly UiBuilder m_builder;
    private readonly Panel m_container;
    private readonly TitleTooltip m_productName;
    private readonly IconContainer m_icon;
    private readonly Txt m_quantity;
    private Option<TextWithIcon> m_quantityPerDuration;
    private Option<ProductProto> m_product;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public ProductQuantityWithIcon(IUiElement parent, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      UiStyle style = builder.Style;
      this.m_container = builder.NewPanel("ProductQuantityIcon", parent);
      this.m_icon = builder.NewIconContainer("Icon", (IUiElement) this.m_container).PutTo<IconContainer>((IUiElement) this.m_container, Offset.Bottom(style.ProductWithIcon.QuantityLineHeight));
      this.m_productName = new TitleTooltip(builder);
      this.m_quantity = builder.NewTxt("QuantityText", (IUiElement) this.m_container).SetAlignment(TextAnchor.MiddleCenter).EnableRichText().SetTextStyle(style.ProductWithIcon.QuantityText).PutToBottomOf<Txt>((IUiElement) this.m_container, style.ProductWithIcon.QuantityLineHeight);
    }

    public void SetProduct(ProductQuantity productQuantity)
    {
      this.m_product = (Option<ProductProto>) productQuantity.Product;
      this.m_icon.SetIcon(productQuantity.Product.Graphics.IconPath);
      this.setProductName((LocStrFormatted) productQuantity.Product.Strings.Name);
      this.m_quantity.SetText(productQuantity.FormatNumberAndUnitOnly());
      this.m_quantity.Show<Txt>();
      TextWithIcon valueOrNull = this.m_quantityPerDuration.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Hide<TextWithIcon>();
    }

    public void SetIcon(string iconPath) => this.m_icon.SetIcon(iconPath);

    public void SetProduct(ProductProto product, Fix32 quantity)
    {
      this.m_product = (Option<ProductProto>) product;
      this.m_icon.SetIcon(product.Graphics.IconPath);
      this.setProductName((LocStrFormatted) product.Strings.Name);
      this.m_quantity.SetText(quantity.ToString());
      this.m_quantity.Show<Txt>();
      TextWithIcon valueOrNull = this.m_quantityPerDuration.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Hide<TextWithIcon>();
    }

    public void SetRawData(string iconPath, LocStrFormatted text, string quantity)
    {
      this.m_icon.SetIcon(iconPath);
      this.setProductName(text);
      this.m_quantity.SetText(quantity);
      this.m_quantity.Show<Txt>();
      TextWithIcon valueOrNull = this.m_quantityPerDuration.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Hide<TextWithIcon>();
    }

    private void setProductName(LocStrFormatted text) => this.m_productName.SetText(text);

    public void SetQuantityText(string text)
    {
      this.m_quantity.SetText(text);
      this.m_quantity.Show<Txt>();
      TextWithIcon valueOrNull = this.m_quantityPerDuration.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.Hide<TextWithIcon>();
    }

    public TextWithIcon SetQuantityPerDuration(string text, int seconds)
    {
      if (this.m_quantityPerDuration.IsNone)
        this.m_quantityPerDuration = (Option<TextWithIcon>) new TextWithIcon(this.m_builder, 16).SetTextStyle(this.m_builder.Style.ProductWithIcon.QuantityText).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToCenterBottomOf<TextWithIcon>((IUiElement) this.m_container, new Vector2(0.0f, this.m_builder.Style.ProductWithIcon.QuantityLineHeight));
      this.m_quantityPerDuration.Value.SetPrefixText(string.Format("{0} / {1}", (object) text, (object) seconds));
      this.m_quantityPerDuration.Value.Show<TextWithIcon>();
      this.m_quantity.Hide<Txt>();
      return this.m_quantityPerDuration.Value;
    }

    public void ShowProductName() => this.m_productName.Show((IUiElement) this.m_container);

    public void HideProductName() => this.m_productName.Hide();

    public void EnableShowNameOnHover()
    {
      this.m_productName.AttachTo<Panel>((IUiElementWithHover<Panel>) this.m_container);
    }

    public ProductQuantityWithIcon EnableRemoveHoverEffect(Action<Option<ProductProto>> onClick)
    {
      ColorRgba hoveredTextClr = this.m_builder.Style.Global.DangerClr;
      Offset iconOffset = Offset.Bottom(this.m_builder.Style.ProductWithIcon.QuantityLineHeight) + Offset.All(2f);
      IconContainer iconOnHover = this.m_builder.NewIconContainer("OnHoverIcon", (IUiElement) this.m_container).SetIcon("Assets/Unity/UserInterface/General/Trash128.png", this.m_builder.Style.Global.DangerClr).PutToRightBottomOf<IconContainer>((IUiElement) this.m_container, 18.Vector2(), Offset.BottomRight(5f, 2f)).Hide<IconContainer>();
      this.m_container.OnMouseEnter(new Action(onMouseEnter));
      this.m_container.OnMouseLeave(new Action(onMouseLeave));
      this.m_container.OnClick((Action) (() => onClick(this.m_product)));
      return this;

      void onMouseEnter()
      {
        this.m_quantity.SetColor(hoveredTextClr);
        this.m_icon.PutTo<IconContainer>((IUiElement) this.m_container, iconOffset - Offset.All(2f));
        iconOnHover.Show<IconContainer>();
        this.m_productName.Show((IUiElement) this.m_container);
      }

      void onMouseLeave()
      {
        this.m_quantity.SetColor(this.m_builder.Style.EntitiesMenu.ItemTitleStyle.Color);
        this.m_icon.PutTo<IconContainer>((IUiElement) this.m_container, iconOffset);
        iconOnHover.Hide<IconContainer>();
        this.m_productName.Hide();
      }
    }
  }
}
