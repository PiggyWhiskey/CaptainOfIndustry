// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsPopsDiffsTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Population;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Table;
using Mafi.Unity.UiFramework.Components.Tabs;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsPopsDiffsTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly PopsHealthManager m_popsHealthManager;
    private readonly SettlementsManager m_settlementsManager;
    private Fix32Column[] m_columns;
    private Mafi.Unity.UiFramework.Components.Table.Table m_table;
    private ChartWithRangeSelectors m_chart;

    public StatsPopsDiffsTab(
      ICalendar calendar,
      PopsHealthManager popsHealthManager,
      SettlementsManager settlementsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("PopsDiffStats");
      this.m_calendar = calendar;
      this.m_popsHealthManager = popsHealthManager;
      this.m_settlementsManager = settlementsManager;
    }

    protected override void BuildUi()
    {
      this.m_columns = new Fix32Column[7];
      this.m_table = this.Builder.NewTable();
      int width = 120;
      TxtColumn txtColumn = this.m_table.AddTextColumn("", 100);
      this.m_columns[0] = this.m_table.AddFix32Column(string.Format("+ {0}", (object) Tr.StatsPops__Born), width);
      this.m_columns[1] = this.m_table.AddFix32Column(string.Format("+ {0}", (object) Tr.StatsPops__Refugees), width);
      this.m_columns[2] = this.m_table.AddFix32Column(string.Format("- {0}", (object) Tr.StatsPops__Lost), width);
      this.m_table.AddRows(1);
      txtColumn.UpdateCell(0, Tr.StatsRange__Lifetime.TranslatedString);
      this.m_chart = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, "", this.AvailableWidth, this.ViewportHeight - this.m_table.GetMinDimensions().y.RoundToInt());
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.StatsPops__Born), "Assets/Unity/UserInterface/General/Population.svg", (ItemStats) this.m_popsHealthManager.BornTotal, new ColorRgba?(ChartColors.GREEN_DARK)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("+ {0}", (object) Tr.StatsPops__Refugees), "Assets/Unity/UserInterface/WorldMap/Village.svg", (ItemStats) this.m_settlementsManager.NewPopsFromAdoptions, new ColorRgba?(ChartColors.GRAY_LIGHT)));
      this.m_chart.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) Tr.StatsPops__Lost), "Assets/Unity/UserInterface/General/Population.svg", (ItemStats) this.m_popsHealthManager.LostTotal, new ColorRgba?(ChartColors.RED_DARK)));
      this.SetHeight<StatsPopsDiffsTab>(this.m_chart.GetHeight() + this.m_table.GetHeight());
      this.SetWidth<StatsPopsDiffsTab>(this.m_chart.GetWidth());
      this.m_chart.PutToTopOf<ChartWithRangeSelectors>((IUiElement) this, this.m_chart.GetHeight());
      this.m_table.PutToLeftBottomOf<Mafi.Unity.UiFramework.Components.Table.Table>((IUiElement) this, this.m_table.GetMinDimensions(), Offset.Left(25f));
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
      this.m_columns[0].UpdateCell(0, this.m_popsHealthManager.BornTotal.Lifetime);
      this.m_columns[1].UpdateCell(0, this.m_settlementsManager.NewPopsFromAdoptions.Lifetime);
      this.m_columns[2].UpdateCell(0, this.m_popsHealthManager.LostTotal.Lifetime);
    }
  }
}
