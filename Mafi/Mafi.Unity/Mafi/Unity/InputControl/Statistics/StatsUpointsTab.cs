// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsUpointsTab
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
  internal class StatsUpointsTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly UpointsManager m_upointsManager;
    private ChartWithRangeSelectors m_chart;

    public StatsUpointsTab(ICalendar calendar, UpointsManager upointsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("UpointsStats");
      this.m_calendar = calendar;
      this.m_upointsManager = upointsManager;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.Production), "Assets/Unity/UserInterface/General/UnitySmall.svg", (ItemStats) this.m_upointsManager.Stats.GeneratedTotalStats, new ColorRgba?(ChartColors.INCOMES_COLORS[0])));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.Consumption), "Assets/Unity/UserInterface/General/UnitySmall.svg", (ItemStats) this.m_upointsManager.Stats.ConsumedTotalStats, new ColorRgba?(ChartColors.EXPENSE_COLORS[0])));
      int num1 = 1;
      foreach (KeyValuePair<UpointsStatsCategoryProto, Fix32SumStats> generatedStat in (IEnumerable<KeyValuePair<UpointsStatsCategoryProto, Fix32SumStats>>) this.m_upointsManager.Stats.GeneratedStats)
      {
        if (!generatedStat.Key.HideInUI)
        {
          this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>("+ " + generatedStat.Key.Title.TranslatedString, "Assets/Unity/UserInterface/General/UnitySmall.svg", (ItemStats) generatedStat.Value, new ColorRgba?(ChartColors.INCOMES_COLORS[num1 % ChartColors.INCOMES_COLORS.Length])));
          ++num1;
        }
      }
      int num2 = 1;
      foreach (KeyValuePair<UpointsStatsCategoryProto, Fix32SumStats> consumedStat in (IEnumerable<KeyValuePair<UpointsStatsCategoryProto, Fix32SumStats>>) this.m_upointsManager.Stats.ConsumedStats)
      {
        if (!consumedStat.Key.HideInUI)
        {
          this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>("- " + consumedStat.Key.Title.TranslatedString, "Assets/Unity/UserInterface/General/UnitySmall.svg", (ItemStats) consumedStat.Value, new ColorRgba?(ChartColors.EXPENSE_COLORS[num2 % ChartColors.EXPENSE_COLORS.Length])));
          ++num2;
        }
      }
      this.SetSize<StatsUpointsTab>(this.m_chart.GetSize());
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
