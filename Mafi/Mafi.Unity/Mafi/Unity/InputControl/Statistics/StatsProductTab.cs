// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsProductTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Table;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsProductTab : Tab
  {
    private readonly ICalendar m_calendar;
    private static int s_columnsCount;
    private readonly QuantityColumn[] m_columns;
    private ProductStats m_product;
    private TabsContainer m_tabsContainer;
    private Panel m_container;
    private int m_lastSyncUpdate;
    private Mafi.Unity.UiFramework.Components.Table.Table m_table;
    private Option<ChartWithRangeSelectors> m_activeChart;
    private bool m_showQuantityChart;
    private readonly Dict<ProductProto, ChartWithRangeSelectors> m_productToProductionChart;
    private readonly Dict<ProductProto, ChartWithRangeSelectors> m_productToQuantityChart;
    private Panel m_chartContainer;
    private ProductStats m_currentProductStats;

    internal StatsProductTab(ICalendar calendar)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_columns = new QuantityColumn[StatsProductTab.s_columnsCount];
      this.m_productToProductionChart = new Dict<ProductProto, ChartWithRangeSelectors>();
      this.m_productToQuantityChart = new Dict<ProductProto, ChartWithRangeSelectors>();
      // ISSUE: explicit constructor call
      base.\u002Ector("ProductTab");
      this.m_calendar = calendar;
    }

    protected override void BuildUi()
    {
      this.m_container = this.Builder.NewPanel("Container").PutTo<Panel>((IUiElement) this);
      this.m_table = this.buildTable(this.Builder);
      this.m_table.PutToLeftBottomOf<Mafi.Unity.UiFramework.Components.Table.Table>((IUiElement) this.m_container, new Vector2((float) this.m_table.GetMinWidth(), this.m_table.GetHeight()), Offset.Left(25f));
      this.m_chartContainer = this.Builder.NewPanel("ChartContainer").PutToCenterTopOf<Panel>((IUiElement) this.m_container, Vector2.zero, Offset.Bottom(this.m_table.GetHeight()));
      Btn production = this.Builder.NewBtn("Production", (IUiElement) this.m_container).EnableDynamicSize().SetText((LocStrFormatted) Tr.Production).SetButtonStyle(this.Builder.Style.Global.GeneralBtn);
      production.PutToLeftTopOf<Btn>((IUiElement) this.m_container, new Vector2(production.GetOptimalWidth(), 40f), Offset.TopBottom(10f) + Offset.Left(180f));
      Btn quantity = this.Builder.NewBtn("Quantity", (IUiElement) this.m_container).EnableDynamicSize().SetText((LocStrFormatted) Tr.StatsProduct_Quantity).SetButtonStyle(this.Builder.Style.Global.GeneralBtn);
      quantity.PutToLeftTopOf<Btn>((IUiElement) this.m_container, new Vector2(quantity.GetOptimalWidth(), 40f), Offset.TopBottom(10f) + Offset.Left((float) ((double) production.GetOptimalWidth() + 180.0 + 4.0)));
      production.OnClick((Action) (() =>
      {
        this.m_showQuantityChart = false;
        quantity.SetEnabled(true);
        production.SetEnabled(false);
        this.showChartFor(this.m_currentProductStats);
      }));
      production.SetEnabled(this.m_showQuantityChart);
      quantity.OnClick((Action) (() =>
      {
        this.m_showQuantityChart = true;
        quantity.SetEnabled(false);
        production.SetEnabled(true);
        this.showChartFor(this.m_currentProductStats);
      }));
      quantity.SetEnabled(!this.m_showQuantityChart);
      this.SetWidth<StatsProductTab>((float) this.AvailableWidth);
    }

    public void SetTabsView(TabsContainer container) => this.m_tabsContainer = container;

    public void OpenProductDetails(ProductStats product)
    {
      this.m_product = product.CheckNotNull<ProductStats>();
      this.showChartFor(product);
      this.SetHeight<StatsProductTab>(this.m_table.GetHeight() + this.m_activeChart.Value.GetHeight());
      this.m_tabsContainer.SwitchToTab((Tab) this);
    }

    private Mafi.Unity.UiFramework.Components.Table.Table buildTable(UiBuilder builder)
    {
      UiStyle style = builder.Style;
      Mafi.Unity.UiFramework.Components.Table.Table table = builder.NewTable();
      TxtColumn txtColumn = table.AddTextColumn("", 160);
      this.m_columns[0] = table.AddQuantityColumn((LocStrFormatted) Tr.StatsRange__ThisYear, style.Statistics.QuantityColumnWidth);
      this.m_columns[1] = table.AddQuantityColumn((LocStrFormatted) Tr.StatsRange__LastYear, style.Statistics.QuantityColumnWidth);
      this.m_columns[2] = table.AddQuantityColumn((LocStrFormatted) Tr.StatsRange__Lifetime, style.Statistics.QuantityColumnWidth);
      table.AddRows(2);
      txtColumn.UpdateCell(0, Tr.Production.TranslatedString);
      txtColumn.UpdateCell(1, Tr.Consumption.TranslatedString);
      return table;
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      if (this.m_product == null || !this.m_activeChart.HasValue)
        return;
      this.m_activeChart.Value.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      if (this.m_product == null)
        return;
      if (this.m_activeChart.HasValue)
        this.m_activeChart.Value.SyncUpdate(gameTime);
      if (this.shouldSkipSync())
        return;
      this.updateTable();
    }

    private void showChartFor(ProductStats productStats)
    {
      this.m_currentProductStats = productStats;
      ChartWithRangeSelectors withRangeSelectors;
      if (!this.m_productToProductionChart.TryGetValue(productStats.Product, out withRangeSelectors))
      {
        withRangeSelectors = new ChartWithRangeSelectors((IUiElement) this.m_chartContainer, this.Builder, this.m_calendar, Tr.Stats_NoDataYet.TranslatedString, this.AvailableWidth, this.ViewportHeight - this.m_table.GetHeight().RoundToInt());
        withRangeSelectors.SetTitle(productStats.Product.Strings.Name.TranslatedString);
        string iconPath = productStats.Product.Graphics.IconPath;
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.StatsEntry__TotalProduction), iconPath, (ItemStats) productStats.CreatedTotalStats, new ColorRgba?(ChartColors.INCOMES_COLORS[0])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.StatsEntry__Mining), iconPath, (ItemStats) productStats.CreatedByMiningStats, new ColorRgba?(ChartColors.INCOMES_COLORS[1])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.StatsEntry__Recycling), iconPath, (ItemStats) productStats.CreatedByRecyclingStats, new ColorRgba?(ChartColors.INCOMES_COLORS[2])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.Research), iconPath, (ItemStats) productStats.CreatedByResearchStats, new ColorRgba?(ChartColors.INCOMES_COLORS[3])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.SettlementTitle), iconPath, (ItemStats) productStats.CreatedBySettlementStats, new ColorRgba?(ChartColors.INCOMES_COLORS[4])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.StatsEntry__Import), iconPath, (ItemStats) productStats.CreatedByImportStats, new ColorRgba?(ChartColors.INCOMES_COLORS[5])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.StatsEntry__Deconstruction), iconPath, (ItemStats) productStats.CreatedByDeconstructionStats, new ColorRgba?(ChartColors.INCOMES_COLORS[6])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsEntry__TotalConsumption), iconPath, (ItemStats) productStats.UsedTotalStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[0])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsEntry__Dumping), iconPath, (ItemStats) productStats.UsedInDumpingStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[1])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsEntry__Construction), iconPath, (ItemStats) productStats.UsedInConstructionStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[2])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.Maintenance), iconPath, (ItemStats) productStats.UsedInMaintenanceStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[3])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.SettlementTitle), iconPath, (ItemStats) productStats.UsedInSettlementStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[4])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.Research), iconPath, (ItemStats) productStats.UsedInResearchStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[5])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsEntry__Farming), iconPath, (ItemStats) productStats.UsedInFarmsStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[6])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsEntry__Export), iconPath, (ItemStats) productStats.UsedInExportStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[7])));
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.Fuel), iconPath, (ItemStats) productStats.UsedAsFuelStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[8])));
        this.m_chartContainer.SetSize<Panel>(withRangeSelectors.GetSize());
        withRangeSelectors.PutTo<ChartWithRangeSelectors>((IUiElement) this.m_chartContainer).Hide<ChartWithRangeSelectors>();
        this.m_productToProductionChart.Add(productStats.Product, withRangeSelectors);
      }
      ChartWithRangeSelectors objectToPlace;
      if (!this.m_productToQuantityChart.TryGetValue(productStats.Product, out objectToPlace))
      {
        objectToPlace = new ChartWithRangeSelectors((IUiElement) this.m_chartContainer, this.Builder, this.m_calendar, Tr.Stats_NoDataYet.TranslatedString, this.AvailableWidth, this.ViewportHeight - this.m_table.GetHeight().RoundToInt());
        objectToPlace.SetTitle(productStats.Product.Strings.Name.TranslatedString);
        string iconPath = productStats.Product.Graphics.IconPath;
        objectToPlace.Chart.AddSeries(new ChartSeriesData<ItemStats>((LocStrFormatted) Tr.StatsEntry__TotalQuantity, iconPath, (ItemStats) productStats.GlobalQuantityStats, new ColorRgba?(ColorRgba.LightGray)));
        objectToPlace.PutTo<ChartWithRangeSelectors>((IUiElement) this.m_chartContainer).Hide<ChartWithRangeSelectors>();
        this.m_productToQuantityChart.Add(productStats.Product, objectToPlace);
      }
      if (this.m_showQuantityChart)
      {
        if (this.m_activeChart == objectToPlace)
          return;
        ChartWithRangeSelectors valueOrNull = this.m_activeChart.ValueOrNull;
        if (valueOrNull != null)
          valueOrNull.Hide<ChartWithRangeSelectors>();
        this.m_activeChart = (Option<ChartWithRangeSelectors>) objectToPlace;
      }
      else
      {
        if (this.m_activeChart == withRangeSelectors)
          return;
        ChartWithRangeSelectors valueOrNull = this.m_activeChart.ValueOrNull;
        if (valueOrNull != null)
          valueOrNull.Hide<ChartWithRangeSelectors>();
        this.m_activeChart = (Option<ChartWithRangeSelectors>) withRangeSelectors;
      }
      ChartWithRangeSelectors valueOrNull1 = this.m_activeChart.ValueOrNull;
      if (valueOrNull1 == null)
        return;
      valueOrNull1.Show<ChartWithRangeSelectors>();
    }

    private void updateTable()
    {
      QuantityLarge thisYear1;
      QuantityLarge lastYear1;
      QuantityLarge lifetime1;
      this.m_product.GetProducedTotals(out thisYear1, out lastYear1, out lifetime1);
      this.m_columns[0].UpdateCell(0, new ProductQuantityLarge(this.m_product.Product, thisYear1));
      this.m_columns[1].UpdateCell(0, new ProductQuantityLarge(this.m_product.Product, lastYear1));
      this.m_columns[2].UpdateCell(0, new ProductQuantityLarge(this.m_product.Product, lifetime1));
      QuantityLarge thisYear2;
      QuantityLarge lastYear2;
      QuantityLarge lifetime2;
      this.m_product.GetConsumedTotals(out thisYear2, out lastYear2, out lifetime2);
      this.m_columns[0].UpdateCell(1, new ProductQuantityLarge(this.m_product.Product, thisYear2));
      this.m_columns[1].UpdateCell(1, new ProductQuantityLarge(this.m_product.Product, lastYear2));
      this.m_columns[2].UpdateCell(1, new ProductQuantityLarge(this.m_product.Product, lifetime2));
    }

    private bool shouldSkipSync()
    {
      if ((Environment.TickCount - this.m_lastSyncUpdate).Abs() < StatisticsView.UpdatePeriod.Millis)
        return true;
      this.m_lastSyncUpdate = Environment.TickCount;
      return false;
    }

    static StatsProductTab()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      StatsProductTab.s_columnsCount = 3;
    }
  }
}
