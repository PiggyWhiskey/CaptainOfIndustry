// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.PricePanelCompact
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup
{
  public class PricePanelCompact : IUiElement, IDynamicSizeElement
  {
    private readonly UiBuilder m_builder;
    private readonly StackContainer m_priceStack;
    private readonly ViewsCacheHomogeneous<IconContainer> m_plusCache;
    private readonly ViewsCacheHomogeneous<ProductQuantityWithIcon> m_iconCache;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_priceStack.GameObject;

    public RectTransform RectTransform => this.m_priceStack.RectTransform;

    public float PreferredHeight => 60f;

    public bool IsEmpty => this.m_priceStack.IsEmpty;

    public PricePanelCompact(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_priceStack = builder.NewStackContainer("Price").SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetStackingDirection(StackContainer.Direction.LeftToRight).OnMouseEnter(new Action(this.onPanelEnter)).OnMouseLeave(new Action(this.onPanelLeave)).SetHeight<StackContainer>(this.PreferredHeight);
      this.m_plusCache = new ViewsCacheHomogeneous<IconContainer>((Func<IconContainer>) (() => this.m_builder.NewIconContainer("Plus").SetIcon(new IconStyle(this.m_builder.Style.Icons.Plus, new ColorRgba?(ColorRgba.White)))));
      this.m_iconCache = new ViewsCacheHomogeneous<ProductQuantityWithIcon>((Func<ProductQuantityWithIcon>) (() => new ProductQuantityWithIcon((IUiElement) this.m_priceStack, this.m_builder)));
    }

    public void SetPrice(IIndexable<ProductQuantity> products)
    {
      this.m_priceStack.StartBatchOperation();
      this.m_priceStack.Show<StackContainer>();
      this.m_iconCache.ReturnAll();
      this.m_plusCache.ReturnAll();
      this.m_priceStack.ClearAll();
      bool flag = false;
      foreach (ProductQuantity product in products)
      {
        if (flag)
        {
          IconContainer view = this.m_plusCache.GetView();
          view.Show<IconContainer>();
          view.AppendTo<IconContainer>(this.m_priceStack, new Vector2?(14.Vector2()), ContainerPosition.MiddleOrCenter, Offset.LeftRight(0.0f));
        }
        ProductQuantityWithIcon view1 = this.m_iconCache.GetView();
        view1.SetProduct(product);
        view1.HideProductName();
        view1.AppendTo<ProductQuantityWithIcon>(this.m_priceStack, new float?(70f));
        flag = true;
      }
      this.m_priceStack.FinishBatchOperation();
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    public float GetDynamicWidth() => this.m_priceStack.GetDynamicWidth();

    private void onPanelEnter()
    {
      foreach (ProductQuantityWithIcon allExistingOne in this.m_iconCache.AllExistingOnes())
        allExistingOne.ShowProductName();
    }

    private void onPanelLeave()
    {
      foreach (ProductQuantityWithIcon allExistingOne in this.m_iconCache.AllExistingOnes())
        allExistingOne.HideProductName();
    }
  }
}
