// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsPollutionChartTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Environment;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsPollutionChartTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly AirPollutionManager m_airPollutionManager;
    private readonly WaterPollutionManager m_waterPollutionManager;
    private readonly RadiationManager m_radiationManager;
    private readonly LandfillOnTerrainManager m_landfillOnTerrainManager;
    private readonly ProtosDb m_protosDb;
    private ChartWithRangeSelectors m_chart;

    public StatsPollutionChartTab(
      ICalendar calendar,
      AirPollutionManager airPollutionManager,
      WaterPollutionManager waterPollutionManager,
      RadiationManager radiationManager,
      LandfillOnTerrainManager landfillOnTerrainManager,
      ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("PollutionStats");
      this.m_calendar = calendar;
      this.m_airPollutionManager = airPollutionManager;
      this.m_waterPollutionManager = waterPollutionManager;
      this.m_radiationManager = radiationManager;
      this.m_landfillOnTerrainManager = landfillOnTerrainManager;
      this.m_protosDb = protosDb;
    }

    protected override void BuildUi()
    {
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight);
      HealthPointsCategoryProto orThrow1 = this.m_protosDb.GetOrThrow<HealthPointsCategoryProto>(IdsCore.HealthPointsCategories.AirPollutionVehicles);
      HealthPointsCategoryProto orThrow2 = this.m_protosDb.GetOrThrow<HealthPointsCategoryProto>(IdsCore.HealthPointsCategories.AirPollutionShips);
      ProductProto first = this.m_airPollutionManager.ProvidedProducts.First;
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(first, (ItemStats) this.m_airPollutionManager.Stats, new ColorRgba?(ChartColors.GRAY_LIGHT)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>((LocStrFormatted) orThrow1.Title, first.IconPath, (ItemStats) this.m_airPollutionManager.StatsVehicles, new ColorRgba?(ChartColors.ORANGE)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>((LocStrFormatted) orThrow2.Title, first.IconPath, (ItemStats) this.m_airPollutionManager.StatsShips, new ColorRgba?(ChartColors.RED)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(this.m_waterPollutionManager.ProvidedProducts.First, (ItemStats) this.m_waterPollutionManager.Stats, new ColorRgba?(ChartColors.TURQUOISE)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(this.m_landfillOnTerrainManager.LandfillProduct, (ItemStats) this.m_landfillOnTerrainManager.Stats, new ColorRgba?(ChartColors.BROWN)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>("Radiation", "Assets/Unity/UserInterface/General/Radiation.svg", (ItemStats) this.m_radiationManager.Stats, new ColorRgba?(ChartColors.YELLOW)));
      this.SetSize<StatsPollutionChartTab>(this.m_chart.GetSize());
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
