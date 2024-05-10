// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.TradeWindow
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Components.Tabs;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class TradeWindow : WindowView
  {
    private TabsContainer m_tabsContainer;
    private readonly LoansTab m_loansTab;
    private readonly QuickTradesTab m_quickTradeTab;
    private readonly ContractsTab m_contractsTab;

    internal TradeWindow(
      LoansTab loansTab,
      QuickTradesTab quickTradeTab,
      ContractsTab contractsTab)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("Trades");
      this.m_loansTab = loansTab;
      this.m_quickTradeTab = quickTradeTab;
      this.m_contractsTab = contractsTab;
      this.ShowAfterSync = true;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.TradeTitle);
      Vector2 size = this.ResolveWindowSize();
      size = new Vector2(size.x.Min((float) (2.0 * (double) ContractView.OptimalSize.x + 40.0)), size.y);
      this.SetContentSize(size);
      this.PositionSelfToCenter();
      IconStyle iconStyle1 = new IconStyle("Assets/Unity/UserInterface/Toolbar/Trade.svg", new ColorRgba?(this.Builder.Style.Global.DefaultPanelTextColor));
      IconStyle iconStyle2 = new IconStyle("Assets/Unity/UserInterface/General/Loans.svg", new ColorRgba?(this.Builder.Style.Global.DefaultPanelTextColor));
      this.m_loansTab.SetTradeWindow(this);
      this.m_tabsContainer = this.Builder.NewTabsContainer(size.x.RoundToInt(), size.y.RoundToInt(), (IUiElement) this.GetContentPanel()).AddTab((LocStrFormatted) Tr.TradeOffers, new IconStyle?(iconStyle1), (Tab) this.m_quickTradeTab).AddTab((LocStrFormatted) Tr.Loans_Title, new IconStyle?(iconStyle2), (Tab) this.m_loansTab).AddTab((LocStrFormatted) Tr.Contracts__Title, new IconStyle?(iconStyle1), (Tab) this.m_contractsTab).PutTo<TabsContainer>((IUiElement) this.GetContentPanel());
    }

    public void OpenContractsTab() => this.m_tabsContainer.SwitchToTab((Tab) this.m_contractsTab);

    public void OpenLoansTab() => this.m_tabsContainer.SwitchToTab((Tab) this.m_loansTab);

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

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      return this.m_loansTab.IsVisible && this.m_loansTab.InputUpdate(inputScheduler);
    }
  }
}
