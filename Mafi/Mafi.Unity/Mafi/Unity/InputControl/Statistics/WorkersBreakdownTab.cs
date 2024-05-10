// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.WorkersBreakdownTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Population;
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
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class WorkersBreakdownTab : Tab
  {
    private readonly WorkersManager m_workersManager;
    private GridContainer m_consumersContainer;
    private ViewsCacheHomogeneous<ResourceStatView> m_viewsCache;
    private int m_lastSyncUpdate;
    private readonly Lyst<Pair<ResourceStatView, WorkersManager.WorkersStatsPerProto>> m_workersDataCache;

    public WorkersBreakdownTab(WorkersManager workersManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_workersDataCache = new Lyst<Pair<ResourceStatView, WorkersManager.WorkersStatsPerProto>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (WorkersBreakdownTab));
      this.m_workersManager = workersManager;
    }

    protected override void BuildUi()
    {
      this.m_viewsCache = new ViewsCacheHomogeneous<ResourceStatView>((Func<ResourceStatView>) (() => new ResourceStatView(this.Builder, true, 100)));
      this.SetBackground(ElectricityBreakdownTab.PANEL_BG);
      ScrollableContainer parent = this.Builder.NewScrollableContainer("ScrollableConsumers", (IUiElement) this).AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this, Offset.Top(5f));
      this.m_consumersContainer = this.Builder.NewGridContainer("ConsumersContainer").SetCellSize(new Vector2((float) ((double) this.AvailableWidth / 3.0 - 4.0), 60f)).SetDynamicHeightMode(3).SetCellSpacing(2f).PutToTopOf<GridContainer>((IUiElement) parent, 0.0f);
      parent.AddItemTop((IUiElement) this.m_consumersContainer);
      this.SetHeight<WorkersBreakdownTab>((float) this.ViewportHeight);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      if (this.shouldSkipSync())
        return;
      this.m_viewsCache.ReturnAll();
      this.m_consumersContainer.ClearAll();
      this.m_workersDataCache.Clear();
      this.m_consumersContainer.StartBatchOperation();
      IOrderedEnumerable<WorkersManager.WorkersStatsPerProto> orderedEnumerable = this.m_workersManager.GetWorkersStatsPerProto().AsEnumerable<WorkersManager.WorkersStatsPerProto>().OrderByDescending<WorkersManager.WorkersStatsPerProto, int>((Func<WorkersManager.WorkersStatsPerProto, int>) (x => x.WorkersNeeded));
      int num = 0;
      foreach (WorkersManager.WorkersStatsPerProto second in (IEnumerable<WorkersManager.WorkersStatsPerProto>) orderedEnumerable)
      {
        if (second.EntitiesTotal != 0)
        {
          num = num.Max(second.WorkersNeeded);
          ResourceStatView view = this.m_viewsCache.GetView();
          this.m_workersDataCache.Add(Pair.Create<ResourceStatView, WorkersManager.WorkersStatsPerProto>(view, second));
          this.m_consumersContainer.Append((IUiElement) view);
        }
      }
      this.m_consumersContainer.FinishBatchOperation();
      foreach (Pair<ResourceStatView, WorkersManager.WorkersStatsPerProto> pair in this.m_workersDataCache)
        pair.First.SetData(pair.Second, num);
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
