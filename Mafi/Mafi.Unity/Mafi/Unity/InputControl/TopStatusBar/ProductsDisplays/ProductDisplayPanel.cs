// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.ProductsDisplays.ProductDisplayPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Maintenance;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar.ProductsDisplays
{
  internal class ProductDisplayPanel : View
  {
    private readonly IProductsManager m_productsManager;
    private readonly IInputScheduler m_inputScheduler;
    private readonly PinnedProductsManager m_pinnedProductsManager;
    private readonly MaintenanceManager m_maintenanceManager;
    private readonly Action m_onClick;
    private readonly Action m_onMaintenanceClick;
    private readonly Action<ProductProto> m_onProductRightClick;
    private StackContainer m_leftColumn;
    private StackContainer m_rightColumn;
    private bool m_refreshProductsOnSync;
    private readonly IUiUpdater m_updater;
    private readonly Dict<ProductProto, ProductDisplay> m_viewsCache;
    private StackContainer m_maintenanceDisplays;
    private Panel m_container;
    private readonly Lyst<ProductDisplay> m_allVisibleDisplays;
    private readonly Lyst<ProductProto> m_pinnedProducts;
    private Option<ProductDisplay> m_displayBeingDragged;
    private Option<ProductDisplay> m_lastHoveredDisplay;
    private int m_moveToIndex;
    private float m_lastCanvasHeight;

    public ProductDisplayPanel(
      ICalendar calendar,
      IGameLoopEvents gameLoopEvents,
      IProductsManager productsManager,
      IInputScheduler inputScheduler,
      PinnedProductsManager pinnedProductsManager,
      MaintenanceManager maintenanceManager,
      Action onClick,
      Action onMaintenanceClick,
      Action<ProductProto> onProductRightClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_viewsCache = new Dict<ProductProto, ProductDisplay>();
      this.m_allVisibleDisplays = new Lyst<ProductDisplay>();
      this.m_pinnedProducts = new Lyst<ProductProto>();
      this.m_moveToIndex = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (ProductDisplayPanel));
      this.m_productsManager = productsManager;
      this.m_inputScheduler = inputScheduler;
      this.m_pinnedProductsManager = pinnedProductsManager;
      this.m_maintenanceManager = maintenanceManager;
      this.m_onClick = onClick;
      this.m_onMaintenanceClick = onMaintenanceClick;
      this.m_onProductRightClick = onProductRightClick;
      this.m_updater = UpdaterBuilder.Start().Build();
      this.AddUpdater(this.m_updater);
      gameLoopEvents.RenderUpdate.AddNonSaveable<ProductDisplayPanel>(this, new Action<GameTime>(((View) this).RenderUpdate));
      gameLoopEvents.SyncUpdate.AddNonSaveable<ProductDisplayPanel>(this, new Action<GameTime>(((View) this).SyncUpdate));
      pinnedProductsManager.OnPinnedProductsChanged += new Action(this.refreshProductsOnSync);
      this.m_refreshProductsOnSync = true;
      calendar.NewMonthEnd.AddNonSaveable<ProductDisplayPanel>(this, new Action(this.onNewMonth));
    }

    private void onNewMonth()
    {
      foreach (ProductDisplay allVisibleDisplay in this.m_allVisibleDisplays)
        allVisibleDisplay.OnNewMonth();
    }

    public override void SyncUpdate(GameTime time)
    {
      base.SyncUpdate(time);
      if (!this.m_refreshProductsOnSync)
        return;
      this.m_refreshProductsOnSync = false;
      this.m_pinnedProducts.Clear();
      this.m_pinnedProducts.AddRange(this.m_pinnedProductsManager.AllPinnedProducts);
      this.refreshProducts(false);
    }

    public override void RenderUpdate(GameTime time)
    {
      base.RenderUpdate(time);
      foreach (ProductDisplay allVisibleDisplay in this.m_allVisibleDisplays)
        allVisibleDisplay.RenderUpdate();
      if (this.Builder.MainCanvas.GetHeight().IsNear(this.m_lastCanvasHeight, 25f))
        return;
      this.m_lastCanvasHeight = this.Builder.MainCanvas.GetHeight();
      this.m_refreshProductsOnSync = true;
    }

    public void refreshProductsOnSync() => this.m_refreshProductsOnSync = true;

    protected override void BuildUi()
    {
      this.m_container = this.Builder.NewPanel("Container").SetBackground(this.Style.Global.PanelsBg).OnClick(this.m_onClick).PutTo<Panel>((IUiElement) this);
      this.m_leftColumn = this.Builder.NewStackContainer("left").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(4f).SetInnerPadding(Offset.Bottom(4f)).PutToLeftTopOf<StackContainer>((IUiElement) this.m_container, new Vector2(90f, 0.0f), Offset.Left(10f));
      this.m_rightColumn = this.Builder.NewStackContainer("right").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(4f).SetInnerPadding(Offset.Bottom(4f)).PutToRightTopOf<StackContainer>((IUiElement) this.m_container, new Vector2(90f, 0.0f), Offset.Right(10f));
      Panel maintenanceContainer = this.Builder.NewPanel("Container").SetBackground(this.Style.Global.PanelsBg).OnClick(this.m_onMaintenanceClick).PutToRightTopOf<Panel>((IUiElement) this.m_container, new Vector2(110f, 0.0f));
      this.m_maintenanceDisplays = this.Builder.NewStackContainer("maintenance").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(4f).SetInnerPadding(Offset.Bottom(4f)).PutToTopOf<StackContainer>((IUiElement) maintenanceContainer, 0.0f, Offset.Left(10f));
      this.m_maintenanceDisplays.SizeChanged += (Action<IUiElement>) (_ =>
      {
        float dynamicHeight = this.m_maintenanceDisplays.GetDynamicHeight();
        maintenanceContainer.PutToRightTopOf<Panel>((IUiElement) this.m_container, new Vector2(110f, dynamicHeight), Offset.Top((float) (-(double) dynamicHeight - 20.0)));
      });
      foreach (IMaintenanceBufferReadonly buffer in (IEnumerable<IMaintenanceBufferReadonly>) this.m_maintenanceManager.MaintenanceBuffers.OrderBy<IMaintenanceBufferReadonly, ProductProto>((Func<IMaintenanceBufferReadonly, ProductProto>) (x => x.Product)))
      {
        MaintenanceDisplay objectToPlace = new MaintenanceDisplay(buffer, this.Builder, this.m_maintenanceDisplays);
        this.AddUpdater(objectToPlace.Updater);
        objectToPlace.AppendTo<MaintenanceDisplay>(this.m_maintenanceDisplays);
      }
      this.Show();
    }

    public void AddBottomPanel(IUiElement element, Vector2 size)
    {
      element.PutToRightBottomOf<IUiElement>((IUiElement) this.m_container, size, Offset.Bottom((float) (-(double) size.y - 20.0)));
    }

    private void refreshProducts(bool isPreviewForDrag)
    {
      int num1 = 0;
      bool visibility = false;
      float num2 = (float) (this.m_pinnedProducts.Count * 36);
      this.m_lastCanvasHeight = this.Builder.MainCanvas.GetHeight();
      int num3 = (double) num2 >= (double) this.m_lastCanvasHeight - 600.0 ? this.m_pinnedProducts.Count / 2 + this.m_pinnedProducts.Count % 2 : this.m_pinnedProducts.Count;
      this.m_leftColumn.StartBatchOperation();
      this.m_rightColumn.StartBatchOperation();
      if (!isPreviewForDrag)
      {
        this.m_leftColumn.ClearAll(true);
        this.m_rightColumn.ClearAll(true);
        this.m_updater.ClearAllChildUpdaters();
        this.m_allVisibleDisplays.Clear();
      }
      else
      {
        this.m_leftColumn.ClearAll();
        this.m_rightColumn.ClearAll();
      }
      Vector2 position = Vector2.zero;
      foreach (ProductProto pinnedProduct in this.m_pinnedProducts)
      {
        ProductProto product = pinnedProduct;
        ProductDisplay element;
        if (!isPreviewForDrag)
        {
          element = this.m_viewsCache.GetOrAdd<ProductProto, ProductDisplay>(product, (Func<ProductProto, ProductDisplay>) (p => new ProductDisplay((IUiElement) this.m_leftColumn, this, p, this.Builder, this.m_productsManager, this.m_onProductRightClick, new Action(this.onClick))));
          this.m_allVisibleDisplays.Add(element);
          this.m_updater.AddChildUpdater(element.Updater);
        }
        else
        {
          element = this.m_allVisibleDisplays.FirstOrDefault<ProductDisplay>((Predicate<ProductDisplay>) (x => (Proto) x.Product == (Proto) product));
          if (element == this.m_displayBeingDragged)
            position = element.RectTransform.anchoredPosition;
        }
        if (num1 >= num3)
        {
          visibility = true;
          this.m_rightColumn.Append((IUiElement) element, new float?(32f));
        }
        else
          this.m_leftColumn.Append((IUiElement) element, new float?(32f));
        if (!isPreviewForDrag)
        {
          element.Show<ProductDisplay>();
          element.RefreshDataNow();
        }
        ++num1;
      }
      this.m_leftColumn.FinishBatchOperation();
      this.m_rightColumn.FinishBatchOperation();
      if (!isPreviewForDrag)
      {
        int num4 = visibility ? 2 : 1;
        this.m_rightColumn.SetVisibility<StackContainer>(visibility);
        this.SetHeight<ProductDisplayPanel>(this.m_leftColumn.GetDynamicHeight());
        this.SetWidth<ProductDisplayPanel>((float) ((double) num4 * 90.0 + 20.0) + (float) (10 * (num4 - 1)));
        this.PutToRightMiddleOf<ProductDisplayPanel>((IUiElement) this.Builder.MainCanvas, this.GetSize());
        this.SendToBack<ProductDisplayPanel>();
      }
      else
      {
        ProductDisplay valueOrNull1 = this.m_displayBeingDragged.ValueOrNull;
        if (valueOrNull1 != null)
          valueOrNull1.SetParent<ProductDisplay>((IUiElement) this);
        ProductDisplay valueOrNull2 = this.m_displayBeingDragged.ValueOrNull;
        if (valueOrNull2 == null)
          return;
        valueOrNull2.SetAnchoredPosition<ProductDisplay>(position);
      }
    }

    protected override Option<IUiElement> GetParent(UiBuilder builder)
    {
      return (Option<IUiElement>) (IUiElement) this.Builder.MainCanvas;
    }

    private void onClick()
    {
      if (!this.m_displayBeingDragged.IsNone)
        return;
      Action onClick = this.m_onClick;
      if (onClick == null)
        return;
      onClick();
    }

    public void OnDragStart(ProductDisplay displayDragged)
    {
      this.m_displayBeingDragged = (Option<ProductDisplay>) displayDragged;
      this.m_displayBeingDragged.Value.SetParent<ProductDisplay>((IUiElement) this);
    }

    public void OnDragDone()
    {
      if (this.m_displayBeingDragged.IsNone)
        return;
      if (this.m_moveToIndex >= 0)
        this.m_inputScheduler.ScheduleInputCmd<PinnedProductReorderCmd>(new PinnedProductReorderCmd(this.m_displayBeingDragged.Value.Product.Id, this.m_moveToIndex));
      else
        this.refreshProductsOnSync();
      this.m_moveToIndex = -1;
      this.m_lastHoveredDisplay = Option<ProductDisplay>.None;
      this.m_displayBeingDragged = Option<ProductDisplay>.None;
    }

    public void OnDragMove(Vector2 screenPoint)
    {
      if (this.m_displayBeingDragged.IsNone)
        return;
      ProductDisplay productDisplay = this.m_allVisibleDisplays.FirstOrDefault<ProductDisplay>((Predicate<ProductDisplay>) (x => x != this.m_displayBeingDragged && RectTransformUtility.RectangleContainsScreenPoint(x.RectTransform, screenPoint)));
      if (productDisplay == null || productDisplay == this.m_lastHoveredDisplay)
        return;
      this.m_lastHoveredDisplay = (Option<ProductDisplay>) productDisplay;
      if (!(productDisplay != this.m_displayBeingDragged))
        return;
      this.m_moveToIndex = this.m_pinnedProducts.IndexOf(productDisplay.Product);
      PinnedProductsManager.MoveProductInList(this.m_pinnedProducts, this.m_displayBeingDragged.Value.Product, this.m_pinnedProducts.IndexOf(productDisplay.Product));
      this.refreshProducts(true);
      this.m_lastHoveredDisplay = Option<ProductDisplay>.None;
    }
  }
}
