// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsPopulationTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Population;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsPopulationTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly SettlementsManager m_settlementsManager;
    private readonly WorkersManager m_workersManager;
    private ChartWithRangeSelectors m_chart;

    public StatsPopulationTab(
      ICalendar calendar,
      SettlementsManager settlementsManager,
      WorkersManager workersManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("PopulationStats");
      this.m_calendar = calendar;
      this.m_settlementsManager = settlementsManager;
      this.m_workersManager = workersManager;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>((LocStrFormatted) Tr.TotalPopulation, "Assets/Unity/UserInterface/General/Population.svg", (ItemStats) this.m_settlementsManager.TotalPopulationStats, new ColorRgba?(ChartColors.GREEN)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>((LocStrFormatted) Tr.HousingCap, "Assets/Unity/UserInterface/Toolbar/Settlement.svg", (ItemStats) this.m_settlementsManager.TotalHousingStats, new ColorRgba?(ChartColors.BLUE)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>((LocStrFormatted) Tr.WorkersDemand, "Assets/Unity/UserInterface/General/Workers.svg", (ItemStats) this.m_workersManager.TotalWorkersNeededStats, new ColorRgba?(ChartColors.ORANGE)));
      this.SetSize<StatsPopulationTab>(this.m_chart.GetSize());
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
