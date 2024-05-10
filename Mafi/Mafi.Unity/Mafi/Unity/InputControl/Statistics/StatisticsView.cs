// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Statistics.StatisticsView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Statistics
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class StatisticsView : WindowView
  {
    /// <summary>
    /// How often should be an active statistics panel updated with new values. Having reasonable values improves
    /// performance for large tables of data and also reduces visual aggresivnes of the updates.
    /// </summary>
    public static readonly Duration UpdatePeriod;
    private TabsContainer m_tabsContainer;
    private readonly StatsProductTab m_productTab;
    private readonly StatsFoodTab m_foodTab;
    private readonly StatsHealthTab m_healthTab;
    private readonly StatsPollutionChartTab m_pollutionChartTab;
    private readonly StatsPopulationTab m_populationTab;
    private readonly StatsPopsDiffsTab m_popsDiffTab;
    private readonly StatsUpointsTab m_upointsTab;
    private readonly StatsFuelTab m_fuelChartTab;
    private readonly StatsProductsTab m_productsTab;

    internal StatisticsView(
      StatsProductsTab productsTab,
      StatsFoodTab foodTab,
      StatsHealthTab healthTab,
      StatsPollutionChartTab pollutionChartTab,
      StatsPopulationTab populationTab,
      StatsPopsDiffsTab popsDiffTab,
      StatsUpointsTab upointsTab,
      StatsFuelTab fuelChartTab,
      StatsProductTab productTab)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("Statistics");
      this.m_productsTab = productsTab;
      this.m_foodTab = foodTab;
      this.m_healthTab = healthTab;
      this.m_pollutionChartTab = pollutionChartTab;
      this.m_populationTab = populationTab;
      this.m_popsDiffTab = popsDiffTab;
      this.m_upointsTab = upointsTab;
      this.m_fuelChartTab = fuelChartTab;
      this.m_productTab = productTab;
      this.ShowAfterSync = true;
      this.OnShowStart += (Action) (() =>
      {
        if (!(this.m_tabsContainer.ActiveTab.ValueOrNull is StatsProductsTab valueOrNull2))
          return;
        valueOrNull2.ClearSearch();
      });
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.Statistics);
      Vector2 size = this.ResolveWindowSize();
      this.SetContentSize(size);
      this.PositionSelfToCenter();
      this.m_tabsContainer = this.Builder.NewTabsContainer(size.x.RoundToInt(), size.y.RoundToInt(), (IUiElement) this.GetContentPanel()).PutTo<TabsContainer>((IUiElement) this.GetContentPanel()).AddTab((LocStrFormatted) Tr.Products, new IconStyle?(this.Style.Statistics.ProductsIcon), (Tab) this.m_productsTab).AddTab((LocStrFormatted) Tr.Food, new IconStyle?(this.Style.Statistics.FoodIcon), (Tab) this.m_foodTab).AddTab((LocStrFormatted) Tr.Health, new IconStyle?(this.Style.Statistics.HealthIcon), (Tab) this.m_healthTab).AddTab((LocStrFormatted) Tr.Pollution, new IconStyle?(this.Style.Statistics.PollutionIcon), (Tab) this.m_pollutionChartTab).AddTab((LocStrFormatted) Tr.Population, new IconStyle?(this.Style.Statistics.PopulationIcon), (Tab) this.m_populationTab).AddTab((LocStrFormatted) TrCore.Unity, new IconStyle?(this.Style.Statistics.UnityIcon), (Tab) this.m_upointsTab).AddTab((LocStrFormatted) Tr.PopGrowth, new IconStyle?(this.Style.Statistics.PopDiffIcon), (Tab) this.m_popsDiffTab).AddTab((LocStrFormatted) Tr.Fuel, new IconStyle?(this.Style.Statistics.FuelIcon), (Tab) this.m_fuelChartTab).AddTab("Product", new IconStyle?(this.Style.Statistics.ProductsIcon), (Tab) this.m_productTab, true);
      this.m_productTab.SetTabsView(this.m_tabsContainer);
    }

    public void OpenProductsTab() => this.m_tabsContainer.SwitchToTab((Tab) this.m_productsTab);

    public void OpenProductsTab(ProductStats product)
    {
      this.m_productTab.OpenProductDetails(product);
    }

    public void OpenAndShowFoodStats() => this.m_tabsContainer.SwitchToTab((Tab) this.m_foodTab);

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

    static StatisticsView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      StatisticsView.UpdatePeriod = 2.Seconds();
    }
  }
}
