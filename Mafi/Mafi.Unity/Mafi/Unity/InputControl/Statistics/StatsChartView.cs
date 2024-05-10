// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsChartView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Charts;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  internal class StatsChartView : IUiElement
  {
    private readonly ICalendar m_calendar;
    private readonly Material m_pointsMaterial;
    private readonly Material m_lineMaterial;
    private readonly GraphChart m_chart;
    private bool m_invalidated;
    private bool m_drawRequested;
    private Fix64 m_lastInvalidationTimeMs;
    private readonly Action m_onDailySeriesAdded;
    /// <summary>How many (last) years are shown in the chart.</summary>
    private readonly Lyst<KeyValuePair<ItemStats, ChartTailAdapter>> m_adapters;
    private readonly StackContainer m_chartContainer;
    private int m_seriesCount;
    private readonly Lyst<ChartSeriesData<ItemStats>> m_hiddenSeries;
    private StatsDataRange m_dataRange;
    private int m_lastSeenMonth;

    public GameObject GameObject => this.m_chartContainer.GameObject;

    public RectTransform RectTransform => this.m_chartContainer.RectTransform;

    public StatsChartView(
      IUiElement parent,
      UiBuilder builder,
      ICalendar calendar,
      string messageIfNoData,
      int width,
      int maxHeight,
      Action onDailySeriesAdded)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_adapters = new Lyst<KeyValuePair<ItemStats, ChartTailAdapter>>();
      this.m_hiddenSeries = new Lyst<ChartSeriesData<ItemStats>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_calendar = calendar;
      this.m_onDailySeriesAdded = onDailySeriesAdded;
      ChartStyle style = new ChartStyle((float) width - 200f, ((float) ((double) (width - 200) / 600.0 * 400.0)).Min((float) maxHeight), 3f, 7f, 12f, 30f, 50f, new ColorRgba(0.2f, 0.2f, 0.2f, 0.7f));
      AxisStyle axisStyle = new AxisStyle(15);
      this.m_pointsMaterial = builder.AssetsDb.GetClonedMaterial("Assets/Unity/UserInterface/Charts/Point.mat");
      this.m_lineMaterial = builder.AssetsDb.GetClonedMaterial("Assets/Unity/UserInterface/Charts/Line.mat");
      this.m_chartContainer = builder.NewStackContainer("Chart container", parent).SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.Dynamic);
      this.m_chart = new GraphChart((IUiElement) this.m_chartContainer, builder, style, axisStyle, axisStyle);
      this.m_chart.VerticalAxisParams = new AxisParams<long>(6);
      this.m_chart.HorizontalAxisParams = new AxisParams<int>(11);
      this.m_chartContainer.SetHeight<StackContainer>(this.m_chart.GetHeight());
      this.m_chartContainer.Append((IUiElement) this.m_chart, new float?());
      new ChartLegend(builder, this.m_chart, messageIfNoData).AppendTo<ChartLegend>(this.m_chartContainer, new float?(200f), Offset.Top(35f));
    }

    public void AddSeries(ChartSeriesData<ItemStats> series)
    {
      ChartTailAdapter adapter = new ChartTailAdapter(this.m_calendar);
      this.m_adapters.Add<ItemStats, ChartTailAdapter>(series.Data, adapter);
      if (!series.Data.IsMonthlyEvent)
      {
        Action dailySeriesAdded = this.m_onDailySeriesAdded;
        if (dailySeriesAdded != null)
          dailySeriesAdded();
      }
      if (series.Data.HasAnyNonZeroData)
        this.addSeriesToChart(series, adapter);
      else
        this.m_hiddenSeries.Add(series);
    }

    public void AddSeriesMultiple(IEnumerable<ChartSeriesData<ItemStats>> data)
    {
      foreach (ChartSeriesData<ItemStats> series in data)
        this.AddSeries(series);
    }

    private void addSeriesToChart(ChartSeriesData<ItemStats> seriesData, ChartTailAdapter adapter)
    {
      ColorRgba colorRgba = seriesData.Color ?? ChartColors.DEFAULT_COLORS[this.m_seriesCount % ChartColors.DEFAULT_COLORS.Length];
      this.m_chart.AddSeries(new DataSeries(seriesData.Label, adapter.ChartData, adapter.HorizontalLabels, (IDataValuesFormatter) seriesData.Data), new DataSeriesStyle(this.m_pointsMaterial, this.m_pointsMaterial, this.m_lineMaterial, colorRgba, colorRgba, seriesData.IconPath));
      ++this.m_seriesCount;
      this.m_invalidated = true;
    }

    public void SetRange(StatsDataRange range)
    {
      this.m_dataRange = range;
      this.m_invalidated = true;
    }

    public void RenderUpdate(GameTime gameTime)
    {
      if (!this.m_drawRequested)
        return;
      this.m_drawRequested = false;
      this.m_chart.Generate();
    }

    public void SyncUpdate(GameTime gameTime)
    {
      if (this.m_calendar.CurrentDate.Month != this.m_lastSeenMonth)
      {
        this.m_lastSeenMonth = this.m_calendar.CurrentDate.Month;
        this.m_invalidated = true;
      }
      if (!this.m_invalidated && !(gameTime.TimeSinceStartMs - this.m_lastInvalidationTimeMs > 3000))
        return;
      this.m_lastInvalidationTimeMs = gameTime.TimeSinceStartMs;
      for (int index = 0; index < this.m_hiddenSeries.Count; ++index)
      {
        ChartSeriesData<ItemStats> seriesData = this.m_hiddenSeries[index];
        if (seriesData.Data.HasAnyNonZeroData)
        {
          ChartTailAdapter adapter;
          if (!this.m_adapters.TryGetValue<ItemStats, ChartTailAdapter>(seriesData.Data, out adapter))
          {
            Log.Error("Failed to find adapter for series");
          }
          else
          {
            this.addSeriesToChart(seriesData, adapter);
            this.m_hiddenSeries.RemoveAtReplaceWithLast(index);
            --index;
          }
        }
      }
      if (this.m_dataRange == StatsDataRange.QuarterCenturies && !this.m_adapters.Any<KeyValuePair<ItemStats, ChartTailAdapter>>((Predicate<KeyValuePair<ItemStats, ChartTailAdapter>>) (x => x.Key.AreAnnualDataFull)))
        this.m_dataRange = StatsDataRange.Last100Years;
      foreach (KeyValuePair<ItemStats, ChartTailAdapter> adapter in this.m_adapters)
        adapter.Value.RefreshData(adapter.Key, this.m_dataRange);
      this.m_invalidated = false;
      this.m_drawRequested = true;
    }
  }
}
