// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.ElectricityPerProtoChartTab
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components.Tabs;
using System;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class ElectricityPerProtoChartTab : Tab
  {
    private readonly ICalendar m_calendar;
    private readonly ElectricityManager m_electricityManager;
    private TabsContainer m_tabsContainer;
    private Option<ChartWithRangeSelectors> m_activeChart;
    private readonly Dict<IEntityProto, ChartWithRangeSelectors> m_protoToChart;
    private Option<IEntityProto> m_pendingChart;
    private bool m_isConsumer;

    internal ElectricityPerProtoChartTab(ICalendar calendar, ElectricityManager electricityManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_protoToChart = new Dict<IEntityProto, ChartWithRangeSelectors>();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (ElectricityPerProtoChartTab));
      this.m_calendar = calendar;
      this.m_electricityManager = electricityManager;
    }

    protected override void BuildUi()
    {
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      if (this.m_activeChart == (ChartWithRangeSelectors) null || !this.m_activeChart.HasValue)
        return;
      this.m_activeChart.Value.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      if (this.m_pendingChart.HasValue)
      {
        this.showConsumerChartFor(this.m_pendingChart.Value, this.m_isConsumer);
        this.m_pendingChart = (Option<IEntityProto>) Option.None;
      }
      this.m_activeChart.ValueOrNull?.SyncUpdate(gameTime);
    }

    public void SetTabsView(TabsContainer container) => this.m_tabsContainer = container;

    public void OpenChartFor(IEntityProto proto, bool isConsumer)
    {
      this.m_pendingChart = proto.CreateOption<IEntityProto>();
      this.m_isConsumer = isConsumer;
      this.m_tabsContainer.SwitchToTab((Tab) this);
    }

    private void showConsumerChartFor(IEntityProto proto, bool isConsumer)
    {
      ElectricityAvgStats data = !isConsumer ? this.m_electricityManager.GetProductionStatsPerProto().FirstOrDefault<ElectricityManager.ProductionPerProto>((Func<ElectricityManager.ProductionPerProto, bool>) (x => x.ProducerProto == proto)).ProductionStats : this.m_electricityManager.GetConsumptionStatsPerProto().FirstOrDefault<ElectricityManager.ConsumptionPerProto>((Func<ElectricityManager.ConsumptionPerProto, bool>) (x => x.ConsumerProto == proto)).ConsumptionStats;
      if (data == null)
        return;
      ChartWithRangeSelectors withRangeSelectors;
      if (!this.m_protoToChart.TryGetValue(proto, out withRangeSelectors))
      {
        withRangeSelectors = new ChartWithRangeSelectors((IUiElement) this, this.Builder, this.m_calendar, Tr.Stats_NoDataYet.TranslatedString, this.AvailableWidth, this.ViewportHeight);
        withRangeSelectors.SetTitle(proto.Strings.Name.TranslatedString);
        string iconPath = this.m_electricityManager.ElectricityProto.Graphics.IconPath;
        withRangeSelectors.Chart.AddSeries(new ChartSeriesData<ItemStats>(string.Format("- {0}", (object) (isConsumer ? Tr.Consumption : Tr.Production)), iconPath, (ItemStats) data, new ColorRgba?(isConsumer ? ChartColors.RED : ChartColors.GREEN)));
        this.SetSize<ElectricityPerProtoChartTab>(withRangeSelectors.GetSize());
        withRangeSelectors.PutTo<ChartWithRangeSelectors>((IUiElement) this);
      }
      if (this.m_activeChart == withRangeSelectors)
        return;
      ChartWithRangeSelectors valueOrNull1 = this.m_activeChart.ValueOrNull;
      if (valueOrNull1 != null)
        valueOrNull1.Hide<ChartWithRangeSelectors>();
      this.m_activeChart = (Option<ChartWithRangeSelectors>) withRangeSelectors;
      ChartWithRangeSelectors valueOrNull2 = this.m_activeChart.ValueOrNull;
      if (valueOrNull2 == null)
        return;
      valueOrNull2.Show<ChartWithRangeSelectors>();
    }
  }
}
