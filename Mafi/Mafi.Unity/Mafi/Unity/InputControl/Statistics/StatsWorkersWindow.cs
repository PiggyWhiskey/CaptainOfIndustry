// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatsWorkersWindow
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
  internal class StatsWorkersWindow : WindowView
  {
    private TabsContainer m_tabsContainer;
    private readonly StatsPopulationTab m_chartTab;
    private readonly WorkersBreakdownTab m_breakdownTab;

    internal StatsWorkersWindow(
      NewInstanceOf<StatsPopulationTab> chartTab,
      WorkersBreakdownTab breakdownTab)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("WorkersStats");
      this.m_chartTab = chartTab.Instance;
      this.m_breakdownTab = breakdownTab;
      this.ShowAfterSync = true;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.WorkersDemand);
      Vector2 size = this.ResolveWindowSize();
      this.SetContentSize(size);
      this.PositionSelfToCenter();
      this.m_tabsContainer = this.Builder.NewTabsContainer(size.x.RoundToInt(), size.y.RoundToInt(), (IUiElement) this.GetContentPanel()).PutTo<TabsContainer>((IUiElement) this.GetContentPanel()).AddTab((LocStrFormatted) Tr.StatsTab__Breakdown, new IconStyle?(this.Style.Statistics.BreakdownIcon), (Tab) this.m_breakdownTab).AddTab((LocStrFormatted) Tr.Population, new IconStyle?(this.Style.Statistics.ChartIcon), (Tab) this.m_chartTab);
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
