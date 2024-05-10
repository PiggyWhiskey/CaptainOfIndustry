// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.SettlementWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  internal class SettlementWindow : WindowView
  {
    private TabsContainer m_tabsContainer;
    private readonly SettlementGeneralTab m_generalTab;
    private readonly SettlementFoodTab m_foodTab;

    internal SettlementWindow(
      NewInstanceOf<SettlementGeneralTab> settlementGeneralTab,
      NewInstanceOf<SettlementFoodTab> settlementFoodTab)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (SettlementWindow));
      this.ShowAfterSync = true;
      this.m_generalTab = settlementGeneralTab.Instance;
      this.m_generalTab.WidthAvailable = 680;
      this.m_foodTab = settlementFoodTab.Instance;
      this.m_foodTab.WidthAvailable = 680;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.SettlementTitle);
      Vector2 size = new Vector2(680f, 540f);
      this.SetContentSize(size);
      this.m_tabsContainer = this.Builder.NewTabsContainer(size.x.RoundToInt(), size.y.RoundToInt(), (IUiElement) this.GetContentPanel()).PutTo<TabsContainer>((IUiElement) this.GetContentPanel()).AddTab((LocStrFormatted) Tr.General, new IconStyle?(new IconStyle("Assets/Unity/UserInterface/Toolbar/Settlement.svg", new ColorRgba?(this.Builder.Style.Global.DefaultPanelTextColor))), (Tab) this.m_generalTab).AddTab((LocStrFormatted) Tr.Food, new IconStyle?(new IconStyle("Assets/Unity/UserInterface/General/Food.svg", new ColorRgba?(this.Builder.Style.Global.DefaultPanelTextColor))), (Tab) this.m_foodTab);
    }

    public void OpenFoodTab() => this.m_tabsContainer.SwitchToTab((Tab) this.m_foodTab);

    public void SetSettlementProvider(Func<Settlement> settlementProvider)
    {
      this.m_generalTab.SetSettlementProvider(settlementProvider);
      this.m_foodTab.SetSettlementProvider(settlementProvider);
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
