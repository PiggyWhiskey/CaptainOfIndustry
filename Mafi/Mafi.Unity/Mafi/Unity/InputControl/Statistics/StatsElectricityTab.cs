// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsElectricityTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory.ElectricPower;
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
  internal class StatsElectricityTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly ElectricityManager m_electricityManager;
    private ChartWithRangeSelectors m_chart;

    public StatsElectricityTab(ICalendar calendar, ElectricityManager electricityManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (StatsElectricityTab));
      this.m_calendar = calendar;
      this.m_electricityManager = electricityManager;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      this.m_chart.SetTitle("");
      string iconPath = this.m_electricityManager.ElectricityProto.Graphics.IconPath;
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>((LocStrFormatted) Tr.MaxCapacity, iconPath, (ItemStats) this.m_electricityManager.GenerationCapacityStats, new ColorRgba?(ChartColors.BLUE)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.Production), iconPath, (ItemStats) this.m_electricityManager.ProductionStats, new ColorRgba?(ChartColors.GREEN)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.Consumption), iconPath, (ItemStats) this.m_electricityManager.ConsumptionStats, new ColorRgba?(ChartColors.RED)));
      this.SetSize<StatsElectricityTab>(this.m_chart.GetSize());
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
