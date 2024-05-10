// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsComputingWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatsComputingWindow : WindowView
  {
    private TabsContainer m_tabsContainer;
    private readonly ComputingPerProtoChartTab m_perProtoChartTab;
    private readonly StatsComputingTab m_chartTab;
    private readonly ComputingBreakdownTab m_breakdownTab;

    internal StatsComputingWindow(
      ComputingPerProtoChartTab perProtoChartTab,
      StatsComputingTab chartTab,
      ComputingBreakdownTab breakdownTab)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("ComputingStats");
      this.m_perProtoChartTab = perProtoChartTab;
      this.m_chartTab = chartTab;
      this.m_breakdownTab = breakdownTab;
      this.ShowAfterSync = true;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.ComputingStats);
      Vector2 size = this.ResolveWindowSize();
      this.SetContentSize(size);
      this.PositionSelfToCenter();
      this.m_tabsContainer = this.Builder.NewTabsContainer(size.x.RoundToInt(), size.y.RoundToInt(), (IUiElement) this.GetContentPanel()).PutTo<TabsContainer>((IUiElement) this.GetContentPanel()).AddTab((LocStrFormatted) Tr.StatsTab__Breakdown, new IconStyle?(this.Style.Statistics.BreakdownIcon), (Tab) this.m_breakdownTab).AddTab((LocStrFormatted) Tr.StatsTab__Chart, new IconStyle?(this.Style.Statistics.ChartIcon), (Tab) this.m_chartTab).AddTab("Proto", new IconStyle?(this.Style.Statistics.ChartIcon), (Tab) this.m_perProtoChartTab, true);
      this.m_perProtoChartTab.SetTabsView(this.m_tabsContainer);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_tabsContainer.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_tabsContainer.SyncUpdate(gameTime);
    }
  }
}
