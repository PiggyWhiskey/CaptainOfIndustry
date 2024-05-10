// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsHealthTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Population;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsHealthTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly PopsHealthManager m_popsHealthManager;
    private ChartWithRangeSelectors m_chart;

    public StatsHealthTab(ICalendar calendar, PopsHealthManager popsHealthManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (StatsHealthTab));
      this.m_calendar = calendar;
      this.m_popsHealthManager = popsHealthManager;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      string iconPath = "Assets/Unity/UserInterface/General/Health.svg";
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.Total), iconPath, (ItemStats) this.m_popsHealthManager.HealthStats.GeneratedTotalStats, new ColorRgba?(ChartColors.INCOMES_COLORS[0])));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.Total), iconPath, (ItemStats) this.m_popsHealthManager.HealthStats.ConsumedTotalStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[0])));
      int num1 = 1;
      foreach (KeyValuePair<HealthPointsCategoryProto, IntAvgStats> generatedStat in (IEnumerable<KeyValuePair<HealthPointsCategoryProto, IntAvgStats>>) this.m_popsHealthManager.HealthStats.GeneratedStats)
      {
        this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>("+ " + generatedStat.Key.Title.TranslatedString, iconPath, (ItemStats) generatedStat.Value, new ColorRgba?(ChartColors.INCOMES_COLORS[num1 % ChartColors.INCOMES_COLORS.Length])));
        ++num1;
      }
      int num2 = 1;
      foreach (KeyValuePair<HealthPointsCategoryProto, IntAvgStats> consumedStat in (IEnumerable<KeyValuePair<HealthPointsCategoryProto, IntAvgStats>>) this.m_popsHealthManager.HealthStats.ConsumedStats)
      {
        this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>("- " + consumedStat.Key.Title.TranslatedString, iconPath, (ItemStats) consumedStat.Value, new ColorRgba?(ChartColors.EXPENSE_COLORS[num2 % ChartColors.EXPENSE_COLORS.Length])));
        ++num2;
      }
      this.SetSize<StatsHealthTab>(this.m_chart.GetSize());
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
  }
}
