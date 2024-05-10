// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.ElectricityBreakdownTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Localization;
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
  internal class ElectricityBreakdownTab : Tab
  {
    public static readonly ColorRgba PANEL_BG;
    public static readonly ColorRgba TITLE_BAR_BG;
    public static readonly ColorRgba DIVIDER_BG;
    private readonly ElectricityManager m_electricityManager;
    private readonly ElectricityPerProtoChartTab m_perProtoChartTab;
    private StackContainer m_consumersContainer;
    private ViewsCacheHomogeneous<ResourceStatView> m_consumerViewsCache;
    private ViewsCacheHomogeneous<ResourceStatView> m_producerViewsCache;
    private StackContainer m_producersContainer;
    private int m_lastSyncUpdate;
    private readonly Lyst<Pair<ResourceStatView, ElectricityManager.ConsumptionPerProto>> m_consumersDataCache;
    private readonly Lyst<Pair<ResourceStatView, ElectricityManager.ProductionPerProto>> m_producersDataCache;

    public ElectricityBreakdownTab(
      ElectricityManager electricityManager,
      ElectricityPerProtoChartTab perProtoChartTab)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_consumersDataCache = new Lyst<Pair<ResourceStatView, ElectricityManager.ConsumptionPerProto>>();
      this.m_producersDataCache = new Lyst<Pair<ResourceStatView, ElectricityManager.ProductionPerProto>>();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (ElectricityBreakdownTab));
      this.m_electricityManager = electricityManager;
      this.m_perProtoChartTab = perProtoChartTab;
    }

    protected override void BuildUi()
    {
      this.m_consumerViewsCache = new ViewsCacheHomogeneous<ResourceStatView>((Func<ResourceStatView>) (() => new ResourceStatView(this.Builder, true, openStatsAction: new Action<IEntityProto, bool>(this.OnChartRequested))));
      this.m_producerViewsCache = new ViewsCacheHomogeneous<ResourceStatView>((Func<ResourceStatView>) (() => new ResourceStatView(this.Builder, false, openStatsAction: new Action<IEntityProto, bool>(this.OnChartRequested))));
      this.SetBackground(ElectricityBreakdownTab.PANEL_BG);
      int num1 = 5;
      float num2 = (float) this.AvailableWidth / 2f - (float) num1;
      this.Builder.NewPanel("Divider").SetBackground(ElectricityBreakdownTab.DIVIDER_BG).PutToLeftOf<Panel>((IUiElement) this, (float) (2 * num1), Offset.Left(num2));
      Panel leftTopOf = this.Builder.NewPanel("ConsumersTitle").SetBackground(ElectricityBreakdownTab.TITLE_BAR_BG).PutToLeftTopOf<Panel>((IUiElement) this, new Vector2(num2, 30f));
      this.Builder.NewTitleBigCentered((IUiElement) leftTopOf).SetText((LocStrFormatted) Tr.Consumption).PutTo<Txt>((IUiElement) leftTopOf);
      ScrollableContainer leftOf = this.Builder.NewScrollableContainer("ScrollableConsumers", (IUiElement) this).AddVerticalScrollbar().PutToLeftOf<ScrollableContainer>((IUiElement) this, num2, Offset.Top(leftTopOf.GetHeight()));
      this.m_consumersContainer = this.Builder.NewStackContainer("ConsumersContainer", (IUiElement) leftOf).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(2f).PutToTopOf<StackContainer>((IUiElement) leftOf, 0.0f);
      leftOf.AddItemTop((IUiElement) this.m_consumersContainer);
      Panel rightTopOf = this.Builder.NewPanel("ProducersTitle").SetBackground(ElectricityBreakdownTab.TITLE_BAR_BG).PutToRightTopOf<Panel>((IUiElement) this, new Vector2(num2, 30f));
      this.Builder.NewTitleBigCentered((IUiElement) rightTopOf).SetText((LocStrFormatted) Tr.Production).PutTo<Txt>((IUiElement) rightTopOf);
      ScrollableContainer rightOf = this.Builder.NewScrollableContainer("ScrollableProducers", (IUiElement) this).AddVerticalScrollbar().PutToRightOf<ScrollableContainer>((IUiElement) this, num2, Offset.Top(rightTopOf.GetHeight()));
      this.m_producersContainer = this.Builder.NewStackContainer("ProducersContainer", (IUiElement) rightOf).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(2f).PutToTopOf<StackContainer>((IUiElement) leftOf, 0.0f);
      rightOf.AddItemTop((IUiElement) this.m_producersContainer);
      this.SetHeight<ElectricityBreakdownTab>((float) this.ViewportHeight);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      if (this.shouldSkipSync())
        return;
      this.m_consumerViewsCache.ReturnAll();
      this.m_consumersContainer.ClearAll();
      this.m_consumersDataCache.Clear();
      this.m_consumersContainer.StartBatchOperation();
      IOrderedEnumerable<ElectricityManager.ConsumptionPerProto> orderedEnumerable1 = this.m_electricityManager.GetConsumptionStatsPerProto().AsEnumerable<ElectricityManager.ConsumptionPerProto>().OrderByDescending<ElectricityManager.ConsumptionPerProto, Electricity>((Func<ElectricityManager.ConsumptionPerProto, Electricity>) (x => x.LastTick.Demand));
      Electricity maxDemand = Electricity.Zero;
      foreach (ElectricityManager.ConsumptionPerProto second in (IEnumerable<ElectricityManager.ConsumptionPerProto>) orderedEnumerable1)
      {
        if (second.EntitiesTotal != 0)
        {
          maxDemand = maxDemand.Max(second.LastTick.Demand);
          ResourceStatView view = this.m_consumerViewsCache.GetView();
          this.m_consumersDataCache.Add(Pair.Create<ResourceStatView, ElectricityManager.ConsumptionPerProto>(view, second));
          this.m_consumersContainer.Append((IUiElement) view, new float?(60f));
        }
      }
      this.m_consumersContainer.FinishBatchOperation();
      foreach (Pair<ResourceStatView, ElectricityManager.ConsumptionPerProto> pair in this.m_consumersDataCache)
        pair.First.SetData(pair.Second, maxDemand);
      this.m_producerViewsCache.ReturnAll();
      this.m_producersContainer.ClearAll();
      this.m_producersDataCache.Clear();
      this.m_producersContainer.StartBatchOperation();
      IOrderedEnumerable<ElectricityManager.ProductionPerProto> orderedEnumerable2 = this.m_electricityManager.GetProductionStatsPerProto().AsEnumerable<ElectricityManager.ProductionPerProto>().OrderByDescending<ElectricityManager.ProductionPerProto, Electricity>((Func<ElectricityManager.ProductionPerProto, Electricity>) (x => x.LastTick.ProduceAndWasted));
      Electricity maxProductionPlusWasted = Electricity.Zero;
      foreach (ElectricityManager.ProductionPerProto second in (IEnumerable<ElectricityManager.ProductionPerProto>) orderedEnumerable2)
      {
        if (second.EntitiesTotal != 0)
        {
          maxProductionPlusWasted = maxProductionPlusWasted.Max(second.LastTick.ProduceAndWasted);
          ResourceStatView view = this.m_producerViewsCache.GetView();
          this.m_producersDataCache.Add(Pair.Create<ResourceStatView, ElectricityManager.ProductionPerProto>(view, second));
          this.m_producersContainer.Append((IUiElement) view, new float?(60f));
        }
      }
      this.m_producersContainer.FinishBatchOperation();
      foreach (Pair<ResourceStatView, ElectricityManager.ProductionPerProto> pair in this.m_producersDataCache)
        pair.First.SetData(pair.Second, maxProductionPlusWasted);
    }

    private void OnChartRequested(IEntityProto proto, bool isConsumer)
    {
      this.m_perProtoChartTab.OpenChartFor(proto, isConsumer);
    }

    private bool shouldSkipSync()
    {
      if ((Environment.TickCount - this.m_lastSyncUpdate).Abs() < StatisticsView.UpdatePeriod.Millis)
        return true;
      this.m_lastSyncUpdate = Environment.TickCount;
      return false;
    }

    static ElectricityBreakdownTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ElectricityBreakdownTab.PANEL_BG = (ColorRgba) 3618615;
      ElectricityBreakdownTab.TITLE_BAR_BG = (ColorRgba) 5723991;
      ElectricityBreakdownTab.DIVIDER_BG = (ColorRgba) 2565927;
    }
  }
}
