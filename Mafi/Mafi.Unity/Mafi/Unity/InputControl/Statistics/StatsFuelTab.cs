// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsFuelTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsFuelTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly FuelStatsCollector m_fuelStatsCollector;
    private readonly ProductsManager m_productsManager;
    private Dict<ProductProto, ChartWithRangeSelectors> m_charts;
    private Option<ChartWithRangeSelectors> m_activeChart;
    private Panel m_container;
    private Panel m_chartContainer;
    private StackContainer m_btnsContainer;

    public StatsFuelTab(
      ICalendar calendar,
      FuelStatsCollector fuelStatsCollector,
      ProductsManager productsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_charts = new Dict<ProductProto, ChartWithRangeSelectors>();
      // ISSUE: explicit constructor call
      base.\u002Ector("FuelStats");
      this.m_calendar = calendar;
      this.m_fuelStatsCollector = fuelStatsCollector;
      this.m_productsManager = productsManager;
    }

    protected override void BuildUi()
    {
      this.m_container = this.Builder.NewPanel("Container").PutTo<Panel>((IUiElement) this);
      this.m_chartContainer = this.Builder.NewPanel("ChartContainer").PutToCenterTopOf<Panel>((IUiElement) this.m_container, Vector2.zero);
      this.m_btnsContainer = this.Builder.NewStackContainer("Btns").SetItemSpacing(10f).SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.LeftToRight);
      this.m_btnsContainer.PutToLeftTopOf<StackContainer>((IUiElement) this.m_container, new Vector2(0.0f, 40f), Offset.Top(10f) + Offset.Left(180f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<FuelStatsCollector.StatsPerProduct>((Func<ReadOnlyArraySlice<FuelStatsCollector.StatsPerProduct>>) (() => this.m_fuelStatsCollector.Stats), (ICollectionComparator<FuelStatsCollector.StatsPerProduct, ReadOnlyArraySlice<FuelStatsCollector.StatsPerProduct>>) CompareFixedOrder<FuelStatsCollector.StatsPerProduct>.Instance).Do((Action<Lyst<FuelStatsCollector.StatsPerProduct>>) (stats =>
      {
        foreach (FuelStatsCollector.StatsPerProduct stat in stats)
        {
          if (!this.m_charts.ContainsKey(stat.Product))
            this.createChartFor(stat);
        }
      }));
      this.AddUpdater(updaterBuilder.Build());
    }

    private void createChartFor(FuelStatsCollector.StatsPerProduct stats)
    {
      ChartWithRangeSelectors chart = new ChartWithRangeSelectors((IUiElement) this.m_chartContainer, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      chart.SetTitle(stats.Product.Strings.Name.TranslatedString);
      Btn element = this.Builder.NewBtn("Btn", (IUiElement) this.m_container).EnableDynamicSize().SetText((LocStrFormatted) stats.Product.Strings.Name).SetButtonStyle(this.Builder.Style.Global.GeneralBtn).OnClick((Action) (() => this.selectChart(chart)));
      this.m_btnsContainer.Append((IUiElement) element, new float?(element.GetOptimalWidth()));
      ProductStats statsFor = this.m_productsManager.GetStatsFor(stats.Product);
      chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.Production), stats.Product.IconPath, (ItemStats) statsFor.CreatedTotalStats, new ColorRgba?(ChartColors.GREEN)));
      chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.Total), stats.Product.IconPath, (ItemStats) statsFor.UsedTotalStats, new ColorRgba?(ChartColors.RED_DARK)));
      chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsCat__Vehicles), "Assets/Unity/UserInterface/Toolbar/Vehicles.svg", (ItemStats) stats.TotalConsumedInVehicles, new ColorRgba?(ChartColors.RED)));
      chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsCat__CargoShips), "Assets/Unity/UserInterface/Toolbar/CargoShip.svg", (ItemStats) stats.TotalConsumedInCargoShips, new ColorRgba?(ChartColors.ORANGE)));
      chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsCat__MainShip), "Assets/Unity/UserInterface/WorldMap/Battleship-256.png", (ItemStats) stats.TotalConsumedInBattleship, new ColorRgba?(ChartColors.VIOLET)));
      chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsCat__PowerProduction), "Assets/Unity/UserInterface/Toolbar/Power.svg", (ItemStats) stats.TotalConsumedInPowerGenerators, new ColorRgba?(ChartColors.YELLOW)));
      this.m_charts.Add(stats.Product, chart);
      this.SetSize<StatsFuelTab>(chart.GetSize());
      this.m_chartContainer.SetSize<Panel>(chart.GetSize());
      chart.PutTo<ChartWithRangeSelectors>((IUiElement) this.m_chartContainer);
      if (this.m_activeChart.IsNone)
        this.m_activeChart = (Option<ChartWithRangeSelectors>) chart;
      else
        chart.Hide<ChartWithRangeSelectors>();
    }

    private void selectChart(ChartWithRangeSelectors chart)
    {
      if (this.m_activeChart == chart)
        return;
      ChartWithRangeSelectors valueOrNull = this.m_activeChart.ValueOrNull;
      if (valueOrNull != null)
        valueOrNull.Hide<ChartWithRangeSelectors>();
      chart.Show<ChartWithRangeSelectors>();
      this.m_activeChart = (Option<ChartWithRangeSelectors>) chart;
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_activeChart.ValueOrNull?.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_activeChart.ValueOrNull?.SyncUpdate(gameTime);
    }
  }
}
