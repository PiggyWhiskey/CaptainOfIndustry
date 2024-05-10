// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.ProductRewardsView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Products;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  /// <summary>
  /// Displays a list of products that the player has received as reward / loot.
  /// </summary>
  internal class ProductRewardsView : IUiElement, IDynamicSizeElement
  {
    private readonly ViewsCacheHomogeneous<ProductRewardsView.ProductRewardView> m_viewsCache;
    private readonly StackContainer m_itemsContainer;

    public GameObject GameObject => this.m_itemsContainer.GameObject;

    public RectTransform RectTransform => this.m_itemsContainer.RectTransform;

    public event Action<IUiElement> SizeChanged;

    public ProductRewardsView(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      ProductRewardsView productRewardsView = this;
      this.m_viewsCache = new ViewsCacheHomogeneous<ProductRewardsView.ProductRewardView>((Func<ProductRewardsView.ProductRewardView>) (() => new ProductRewardsView.ProductRewardView(builder)));
      this.m_itemsContainer = builder.NewStackContainer("Items container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.Top(builder.Style.Panel.Padding));
      this.m_itemsContainer.SizeChanged += (Action<IUiElement>) (x =>
      {
        Action<IUiElement> sizeChanged = productRewardsView.SizeChanged;
        if (sizeChanged == null)
          return;
        sizeChanged((IUiElement) productRewardsView);
      });
    }

    public void SetReward(AssetValue assets) => this.SetReward(assets.Products.ToImmutableArray());

    public void SetReward(ImmutableArray<ProductQuantity> products)
    {
      this.m_itemsContainer.StartBatchOperation();
      this.m_itemsContainer.ClearAll();
      this.m_viewsCache.ReturnAll();
      if (products.IsEmpty)
      {
        this.m_itemsContainer.FinishBatchOperation();
      }
      else
      {
        foreach (ProductQuantity product in products)
        {
          ProductRewardsView.ProductRewardView view = this.m_viewsCache.GetView();
          view.SetProduct(product);
          this.m_itemsContainer.Append((IUiElement) view, new float?(80f));
        }
        this.m_itemsContainer.FinishBatchOperation();
      }
    }

    private class ProductRewardView : IUiElement
    {
      private ProductQuantity m_product;
      private readonly ProtoWithIcon<ProductProto> m_protoWithIcon;
      private readonly Panel m_container;
      private readonly Txt m_quantity;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public ProductRewardView(UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_container = builder.NewPanel("PendingProduct");
        this.m_container.SetBackground(builder.Style.Panel.ItemOverlay);
        this.m_protoWithIcon = new ProtoWithIcon<ProductProto>((IUiElement) this.m_container, builder).PutToLeftOf<ProtoWithIcon<ProductProto>>((IUiElement) this.m_container, 80f);
        Txt txt = builder.NewTxt("Quantity");
        TextStyle text = builder.Style.Global.Text;
        ref TextStyle local = ref text;
        int? nullable = new int?(14);
        ColorRgba? color = new ColorRgba?();
        FontStyle? fontStyle = new FontStyle?();
        int? fontSize = nullable;
        bool? isCapitalized = new bool?();
        TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
        this.m_quantity = txt.SetTextStyle(textStyle).PutToLeftMiddleOf<Txt>((IUiElement) this.m_container, new Vector2(80f, 30f), Offset.Left(80f));
      }

      public void SetProduct(ProductQuantity pq)
      {
        this.m_product = pq;
        this.m_protoWithIcon.SetProto((Option<ProductProto>) pq.Product);
        this.m_quantity.SetText(this.m_product.Product.WithQuantity(pq.Quantity).FormatNumberAndUnitOnly());
      }
    }
  }
}
