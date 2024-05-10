// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.BufferWithMultipleProductsView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  public class BufferWithMultipleProductsView : IUiElement
  {
    private static readonly ColorRgba[] COLORS;
    private readonly Panel m_container;
    private readonly ViewsCacheHomogeneous<Panel> m_barsCache;
    private readonly ViewsCacheHomogeneous<BufferWithMultipleProductsView.SingleProductLegend> m_legendsCache;
    private readonly Txt m_quantityText;
    private readonly StackContainer m_legendsContainer;
    private readonly Panel m_barContainer;
    private readonly Btn m_trashBtn;
    private readonly Txt m_emptyText;
    private readonly Lyst<BufferWithMultipleProductsView.ProductData> m_dataCache;
    private readonly Lyst<QuantityBar.Marker> m_markers;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public Btn TrashBtn => this.m_trashBtn;

    public BufferWithMultipleProductsView(IUiElement parent, UiBuilder builder, Action trashAction = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_dataCache = new Lyst<BufferWithMultipleProductsView.ProductData>();
      this.m_markers = new Lyst<QuantityBar.Marker>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      BufferWithMultipleProductsView multipleProductsView = this;
      UiStyle style = builder.Style;
      this.m_container = builder.NewPanel("BufferRow", parent).SetBackground(style.Panel.ItemOverlay);
      this.m_barContainer = builder.NewPanel("Bar").SetBackground(style.QuantityBar.BackgroundColor).SetBorderStyle(style.Global.DefaultDarkBorder).PutToBottomOf<Panel>((IUiElement) this.m_container, 25f, Offset.LeftRight(15f) + Offset.Bottom(10f));
      this.m_quantityText = builder.NewTxt("Quantity text").SetText("0").SetAlignment(TextAnchor.MiddleRight).SetTextStyle(style.QuantityBar.Text).PutTo<Txt>((IUiElement) this.m_barContainer, Offset.Right(5f));
      this.m_emptyText = builder.NewTxt("Empty").SetText((LocStrFormatted) Tr.Empty).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.Global.Title).PutToLeftTopOf<Txt>((IUiElement) this.m_barContainer, new Vector2(200f, 20f), Offset.Left(30f) + Offset.Top(-40f));
      builder.NewIconContainer("EmptyIcon").SetIcon("Assets/Unity/UserInterface/General/Empty128.png").PutToLeftOf<IconContainer>((IUiElement) this.m_emptyText, 16f, Offset.Left(-20f));
      this.m_legendsContainer = builder.NewStackContainer("LegendsContainer").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(10f).PutToTopOf<StackContainer>((IUiElement) this.m_container, 50f, Offset.LeftRight(15f) + Offset.Top(10f));
      this.m_trashBtn = builder.NewBtnDanger("Trash icon", (IUiElement) this.m_legendsContainer).SetIcon("Assets/Unity/UserInterface/General/Trash128.png").OnClick((Action) (() =>
      {
        Action action = trashAction;
        if (action == null)
          return;
        action();
      })).Hide<Btn>();
      this.m_barsCache = new ViewsCacheHomogeneous<Panel>((Func<Panel>) (() => new Panel(builder, "Bar", multipleProductsView.m_barContainer.GameObject)));
      this.m_legendsCache = new ViewsCacheHomogeneous<BufferWithMultipleProductsView.SingleProductLegend>((Func<BufferWithMultipleProductsView.SingleProductLegend>) (() => new BufferWithMultipleProductsView.SingleProductLegend(builder, (IUiElement) multipleProductsView.m_legendsContainer)));
      this.SetHeight<BufferWithMultipleProductsView>(100f);
    }

    public void SetProducts(Lyst<ProductQuantity> products, Quantity capacity, bool showTrashBtn)
    {
      this.m_trashBtn.Hide<Btn>();
      this.m_barsCache.ReturnAll();
      this.m_legendsContainer.StartBatchOperation();
      this.m_legendsContainer.ClearAll();
      this.m_legendsCache.ReturnAll();
      float width = this.m_barContainer.GetWidth();
      Quantity quantity1 = products.Sum<ProductQuantity>((Func<ProductQuantity, int>) (x => x.Quantity.Value)).Quantity();
      Quantity quantity2 = quantity1.Max(capacity);
      this.m_dataCache.Clear();
      float num1 = 0.0f;
      foreach (ProductQuantity productQuantity in (IEnumerable<ProductQuantity>) products.OrderByDescending<ProductQuantity, Quantity>((Func<ProductQuantity, Quantity>) (x => x.Quantity)))
      {
        float num2 = Percent.FromRatio(productQuantity.Quantity.Value, quantity2.Value).Apply(width);
        float spaceAssigned = (double) num2 < 20.0 ? 20f : num2;
        num1 += spaceAssigned;
        this.m_dataCache.Add(new BufferWithMultipleProductsView.ProductData(productQuantity, spaceAssigned));
      }
      if (quantity1 < capacity && capacity.IsPositive)
      {
        float num3 = Percent.FromRatio(capacity.Value - quantity1.Value, capacity.Value).Apply(width);
        float num4 = (double) num3 < 20.0 ? 20f : num3;
        num1 += num4;
      }
      float num5 = width / num1;
      int num6 = 0;
      float leftOffset = 0.0f;
      foreach (BufferWithMultipleProductsView.ProductData productData in this.m_dataCache)
      {
        ColorRgba colorRgba = BufferWithMultipleProductsView.COLORS[num6 % BufferWithMultipleProductsView.COLORS.Length];
        ++num6;
        Panel view1 = this.m_barsCache.GetView();
        float size = productData.SpaceAssigned * num5;
        view1.PutToLeftOf<Panel>((IUiElement) this.m_barContainer, size, Offset.Left(leftOffset));
        view1.SetBackground(colorRgba);
        leftOffset += size;
        BufferWithMultipleProductsView.SingleProductLegend view2 = this.m_legendsCache.GetView();
        view2.SetProduct(productData.ProductQuantity, colorRgba);
        this.m_legendsContainer.Append((IUiElement) view2, new float?(70f));
      }
      if (showTrashBtn && quantity1.IsPositive)
      {
        this.m_trashBtn.Show<Btn>();
        this.m_legendsContainer.Append((IUiElement) this.m_trashBtn, new Vector2?(30.Vector2()), new ContainerPosition?(ContainerPosition.LeftOrTop), Offset.Top(5f));
      }
      this.m_legendsContainer.FinishBatchOperation();
      this.m_emptyText.SetVisibility<Txt>(products.IsEmpty);
      Percent percent = capacity.IsZero ? Percent.Zero : Percent.FromRatio(quantity1.Value, capacity.Value);
      this.m_quantityText.SetText(string.Format("{0} / {1} ({2})", (object) quantity1.Value, (object) capacity.Value, (object) percent.ToStringRounded()));
      this.m_quantityText.SendToFront<Txt>();
      this.m_barContainer.Border.ValueOrNull?.transform.SetAsLastSibling();
      foreach (QuantityBar.Marker marker in this.m_markers)
        marker.SendToFront<QuantityBar.Marker>();
    }

    public QuantityBar.Marker AddMarker(UiBuilder builder, Percent position, ColorRgba color)
    {
      QuantityBar.Marker marker = new QuantityBar.Marker(builder.NewPanel("marker"), (IUiElement) this.m_barContainer);
      marker.SetPosition(position);
      marker.SetColor(color);
      this.m_markers.Add(marker);
      return marker;
    }

    static BufferWithMultipleProductsView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BufferWithMultipleProductsView.COLORS = new ColorRgba[9]
      {
        (ColorRgba) 9667897,
        (ColorRgba) 5146443,
        (ColorRgba) 3564694,
        (ColorRgba) 9657385,
        (ColorRgba) 10329501,
        (ColorRgba) 9648445,
        (ColorRgba) 11956626,
        (ColorRgba) 8010168,
        (ColorRgba) 3768211
      };
    }

    private class SingleProductLegend : IUiElement
    {
      private readonly Panel m_container;
      private readonly Panel m_clrRect;
      private readonly IconContainer m_productIcon;
      private readonly TitleTooltip m_productName;
      private readonly Txt m_quantity;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public SingleProductLegend(UiBuilder builder, IUiElement parent)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_container = builder.NewPanel("ProductLegend", parent).SetBackground(ColorRgba.Empty).SetOnMouseEnterLeaveActions(new Action(this.onMouseEnter), new Action(this.onMouseLeave));
        this.m_clrRect = builder.NewPanel("ColorRect").SetBackground(ColorRgba.White).PutToLeftMiddleOf<Panel>((IUiElement) this.m_container, 12.Vector2());
        Panel leftOf = builder.NewPanel("ProductQuantityIcon", parent).PutToLeftOf<Panel>((IUiElement) this.m_container, 30f, Offset.Left(20f));
        this.m_productIcon = builder.NewIconContainer("Icon", (IUiElement) leftOf).PutTo<IconContainer>((IUiElement) leftOf, Offset.Bottom(20f));
        this.m_quantity = builder.NewTxt("QuantityText", (IUiElement) leftOf).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(builder.Style.Global.BoldText).PutToBottomOf<Txt>((IUiElement) leftOf, 20f);
        this.m_productName = new TitleTooltip(builder);
      }

      public void SetProduct(ProductQuantity pq, ColorRgba legendColor)
      {
        this.m_productName.SetText((LocStrFormatted) pq.Product.Strings.Name);
        this.m_productIcon.SetIcon(pq.Product.IconPath);
        this.m_quantity.SetText(pq.FormatNumberAndUnitOnly());
        this.m_clrRect.SetBackground(legendColor);
      }

      private void onMouseEnter() => this.m_productName.Show((IUiElement) this.m_container);

      private void onMouseLeave() => this.m_productName.Hide();
    }

    private readonly struct ProductData
    {
      public readonly ProductQuantity ProductQuantity;
      public readonly float SpaceAssigned;

      public ProductData(ProductQuantity productQuantity, float spaceAssigned)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.ProductQuantity = productQuantity;
        this.SpaceAssigned = spaceAssigned;
      }
    }
  }
}
