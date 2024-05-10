// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.ProductQuantitiesView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  /// <summary>Displays a grid of products and quantities.</summary>
  internal class ProductQuantitiesView : IUiElement, IDynamicSizeElement
  {
    private readonly ViewsCacheHomogeneous<ProductQuantityWithIcon> m_viewsCache;
    private readonly Option<Action<ProductProto>> m_removeOnClick;
    private readonly GridContainer m_itemsContainer;
    private readonly Panel m_container;
    private readonly Txt m_noItemsText;
    private readonly bool m_useDynamicWidth;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public ProductQuantitiesView(
      IUiElement parent,
      UiBuilder builder,
      Action<ProductProto> removeOnClick = null,
      int cellSize = 75,
      bool useDynamicWidth = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProductQuantitiesView productQuantitiesView = this;
      this.m_removeOnClick = (Option<Action<ProductProto>>) removeOnClick;
      this.m_useDynamicWidth = useDynamicWidth;
      this.m_container = builder.NewPanel("Products", parent).SetBackground(builder.Style.Panel.ItemOverlay);
      this.m_noItemsText = builder.NewTxt("NoItemsInfo", (IUiElement) this.m_container).SetTextStyle(builder.Style.Global.Text).SetText((LocStrFormatted) Tr.Empty).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_container).Hide<Txt>();
      this.m_itemsContainer = builder.NewGridContainer("Grid").SetCellSize(cellSize.Vector2()).SetCellSpacing(5f).PutToLeftTopOf<GridContainer>((IUiElement) this.m_container, 0.Vector2(), Offset.Top(5f));
      if (useDynamicWidth)
        this.m_itemsContainer.SetDynamicWidthMode(1);
      else
        this.m_itemsContainer.SetDynamicHeightMode(5);
      this.m_viewsCache = new ViewsCacheHomogeneous<ProductQuantityWithIcon>((Func<ProductQuantityWithIcon>) (() =>
      {
        ProductQuantityWithIcon quantityWithIcon = new ProductQuantityWithIcon((IUiElement) productQuantitiesView.m_itemsContainer, builder);
        if (productQuantitiesView.m_removeOnClick.HasValue)
          quantityWithIcon.EnableRemoveHoverEffect(new Action<Option<ProductProto>>(productQuantitiesView.onProductClick));
        else
          quantityWithIcon.EnableShowNameOnHover();
        return quantityWithIcon;
      }));
    }

    public void SetProducts(ImmutableArray<ProductQuantity> products)
    {
      this.SetProducts(products.AsIndexable);
    }

    public void SetProducts(IIndexable<ProductQuantity> products)
    {
      this.m_itemsContainer.StartBatchOperation();
      this.m_itemsContainer.ClearAll();
      this.m_viewsCache.ReturnAll();
      foreach (ProductQuantity product in products)
      {
        ProductQuantityWithIcon view = this.m_viewsCache.GetView();
        view.SetProduct(product);
        view.HideProductName();
        this.m_itemsContainer.Append((IUiElement) view);
      }
      this.m_itemsContainer.FinishBatchOperation();
      this.m_noItemsText.SetVisibility<Txt>(products.IsEmpty<ProductQuantity>());
      if (this.m_useDynamicWidth)
      {
        this.m_container.SetSize<Panel>(new Vector2(this.m_itemsContainer.GetRequiredWidth(), (this.m_itemsContainer.GetHeight() + 10f).Max(40f)));
      }
      else
      {
        Vector2 size = this.m_itemsContainer.GetSize();
        this.m_container.SetSize<Panel>(new Vector2(size.x, (size.y + 10f).Max(40f)));
      }
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    public void SetTransparentBackground() => this.m_container.SetBackground(ColorRgba.Empty);

    private void onProductClick(Option<ProductProto> product)
    {
      if (!product.HasValue)
        return;
      Action<ProductProto> valueOrNull = this.m_removeOnClick.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull(product.Value);
    }
  }
}
