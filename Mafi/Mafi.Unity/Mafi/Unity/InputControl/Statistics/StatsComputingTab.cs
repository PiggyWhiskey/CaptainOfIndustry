// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsComputingTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsComputingTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly ComputingManager m_computingManager;
    private ChartWithRangeSelectors m_chart;

    public StatsComputingTab(ICalendar calendar, ComputingManager computingManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (StatsComputingTab));
      this.m_calendar = calendar;
      this.m_computingManager = computingManager;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      this.m_chart.SetTitle("");
      string iconPath = this.m_computingManager.ComputingProductProto.Graphics.IconPath;
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(Tr.MaxCapacity.TranslatedString, iconPath, (ItemStats) this.m_computingManager.GenerationCapacityStats, new ColorRgba?(ChartColors.BLUE)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.Production), iconPath, (ItemStats) this.m_computingManager.ProductionStats, new ColorRgba?(ChartColors.GREEN)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.Demand), iconPath, (ItemStats) this.m_computingManager.DemandStats, new ColorRgba?(ChartColors.RED)));
      this.SetSize<StatsComputingTab>(this.m_chart.GetSize());
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
