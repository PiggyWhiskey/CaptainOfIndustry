// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsTestChartTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  internal class StatsTestChartTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly RandomProvider m_randomProvider;
    private readonly StatsManager m_statsManager;
    private readonly ProtosDb m_protosDb;
    private ChartWithRangeSelectors m_chart;

    public StatsTestChartTab(
      ICalendar calendar,
      RandomProvider randomProvider,
      StatsManager statsManager,
      ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("TestTab");
      this.m_calendar = calendar;
      this.m_randomProvider = randomProvider;
      this.m_statsManager = statsManager;
      this.m_protosDb = protosDb;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "Nothing was added yet", this.AvailableWidth, this.ViewportHeight);
      this.m_chart.SetTitle("Test statistics");
      IRandom simRandomFor = this.m_randomProvider.GetSimRandomFor((object) this);
      StatsTestChartTab.RandomStatsGenerator randomStatsGenerator1 = new StatsTestChartTab.RandomStatsGenerator(simRandomFor, this.m_calendar, this.m_statsManager);
      StatsTestChartTab.RandomStatsGenerator randomStatsGenerator2 = new StatsTestChartTab.RandomStatsGenerator(simRandomFor, this.m_calendar, this.m_statsManager);
      StatsTestChartTab.RandomStatsGenerator randomStatsGenerator3 = new StatsTestChartTab.RandomStatsGenerator(simRandomFor, this.m_calendar, this.m_statsManager, 2);
      IRandom nonSimRandomFor = this.m_randomProvider.GetNonSimRandomFor((object) this);
      this.m_chart.Chart.AddSeriesMultiple((IEnumerable<ChartSeriesData<ItemStats>>) new ChartSeriesData<ItemStats>[3]
      {
        new ChartSeriesData<ItemStats>(this.m_protosDb.All<ProductProto>().SampleRandomOrDefault<ProductProto>(nonSimRandomFor), (ItemStats) randomStatsGenerator1.Stats),
        new ChartSeriesData<ItemStats>(this.m_protosDb.All<ProductProto>().SampleRandomOrDefault<ProductProto>(nonSimRandomFor), (ItemStats) randomStatsGenerator2.Stats),
        new ChartSeriesData<ItemStats>(this.m_protosDb.All<ProductProto>().SampleRandomOrDefault<ProductProto>(nonSimRandomFor), (ItemStats) randomStatsGenerator3.Stats)
      });
      this.SetSize<StatsTestChartTab>(this.m_chart.GetSize());
      this.m_chart.PutTo<ChartWithRangeSelectors>((IUiElement) this);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_chart.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_chart.SyncUpdate(gameTime);
    }

    private class RandomStatsGenerator
    {
      public readonly QuantitySumStats Stats;
      private readonly IRandom m_random;
      private int m_daysDelayed;

      public RandomStatsGenerator(
        IRandom random,
        ICalendar calendar,
        StatsManager statsManager,
        int monthsDelayed = 0)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_daysDelayed = monthsDelayed * 30;
        this.m_random = random;
        calendar.NewDay.AddNonSaveable<StatsTestChartTab.RandomStatsGenerator>(this, new Action(this.onNewDay));
        this.Stats = new QuantitySumStats((Option<StatsManager>) statsManager);
      }

      private void onNewDay()
      {
        --this.m_daysDelayed;
        for (int index = 0; index < 12; ++index)
        {
          if (this.m_daysDelayed < 0)
            this.Stats.Add(this.m_random.NextInt(100).Quantity().AsLarge);
        }
      }
    }
  }
}
