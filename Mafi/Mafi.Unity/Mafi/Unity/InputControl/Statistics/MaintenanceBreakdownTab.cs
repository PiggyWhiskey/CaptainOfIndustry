// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.MaintenanceBreakdownTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Maintenance;
using Mafi.Core.Products;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  internal class MaintenanceBreakdownTab : Tab
  {
    private readonly MaintenanceManager m_maintenanceManager;
    private readonly ProductProto m_product;
    private GridContainer m_consumersContainer;
    private ViewsCacheHomogeneous<ResourceStatView> m_viewsCache;
    private int m_lastSyncUpdate;
    private readonly Lyst<Pair<ResourceStatView, MaintenanceManager.ConsumptionPerProto>> m_dataCache;

    public MaintenanceBreakdownTab(MaintenanceManager maintenanceManager, ProductProto product)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_dataCache = new Lyst<Pair<ResourceStatView, MaintenanceManager.ConsumptionPerProto>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (MaintenanceBreakdownTab));
      this.m_maintenanceManager = maintenanceManager;
      this.m_product = product;
    }

    protected override void BuildUi()
    {
      this.m_viewsCache = new ViewsCacheHomogeneous<ResourceStatView>((Func<ResourceStatView>) (() => new ResourceStatView(this.Builder, true, 100)));
      this.SetBackground(ElectricityBreakdownTab.PANEL_BG);
      ScrollableContainer parent = this.Builder.NewScrollableContainer("ScrollableConsumers", (IUiElement) this).AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this, Offset.Top(5f));
      this.m_consumersContainer = this.Builder.NewGridContainer("ConsumersContainer").SetCellSize(new Vector2((float) ((double) this.AvailableWidth / 3.0 - 4.0), 60f)).SetDynamicHeightMode(3).SetCellSpacing(2f).PutToTopOf<GridContainer>((IUiElement) parent, 0.0f);
      parent.AddItemTop((IUiElement) this.m_consumersContainer);
      this.SetHeight<MaintenanceBreakdownTab>((float) this.ViewportHeight);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      if (this.shouldSkipSync())
        return;
      this.m_viewsCache.ReturnAll();
      this.m_consumersContainer.ClearAll();
      this.m_dataCache.Clear();
      this.m_consumersContainer.StartBatchOperation();
      IOrderedEnumerable<MaintenanceManager.ConsumptionPerProto> orderedEnumerable = this.m_maintenanceManager.GetConsumptionStatsPerProto(this.m_product).AsEnumerable<MaintenanceManager.ConsumptionPerProto>().OrderByDescending<MaintenanceManager.ConsumptionPerProto, PartialQuantity>((Func<MaintenanceManager.ConsumptionPerProto, PartialQuantity>) (x => x.LastTick.Demand));
      PartialQuantity maxDemand = PartialQuantity.Zero;
      foreach (MaintenanceManager.ConsumptionPerProto second in (IEnumerable<MaintenanceManager.ConsumptionPerProto>) orderedEnumerable)
      {
        if (second.EntitiesTotal != 0)
        {
          maxDemand = maxDemand.Max(second.LastTick.Demand);
          ResourceStatView view = this.m_viewsCache.GetView();
          this.m_dataCache.Add(Pair.Create<ResourceStatView, MaintenanceManager.ConsumptionPerProto>(view, second));
          this.m_consumersContainer.Append((IUiElement) view);
        }
      }
      this.m_consumersContainer.FinishBatchOperation();
      foreach (Pair<ResourceStatView, MaintenanceManager.ConsumptionPerProto> pair in this.m_dataCache)
        pair.First.SetData(pair.Second, maxDemand);
    }

    private bool shouldSkipSync()
    {
      if ((Environment.TickCount - this.m_lastSyncUpdate).Abs() < StatisticsView.UpdatePeriod.Millis)
        return true;
      this.m_lastSyncUpdate = Environment.TickCount;
      return false;
    }
  }
}
