// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipUpgradeConfirmationPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.ShipDesign
{
  public class ShipUpgradeConfirmationPanel : WindowView
  {
    private readonly TravelingFleetManager m_fleetManager;
    private readonly ProductsAvailableInStorage m_availableProductsProvider;
    private readonly Action m_onCommitClick;
    private readonly Action m_onResetClick;
    private PricePanel m_valueToPayPanel;
    private StackContainer m_costsContainer;

    public ShipUpgradeConfirmationPanel(
      TravelingFleetManager fleetManager,
      ProductsAvailableInStorage availableProductsProvider,
      Action onCommitClick,
      Action onResetClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("UpgradeDialog", WindowView.FooterStyle.None);
      this.m_fleetManager = fleetManager;
      this.m_availableProductsProvider = availableProductsProvider;
      this.m_onCommitClick = onCommitClick;
      this.m_onResetClick = onResetClick;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.ShipDesignerConfirmation__Title);
      this.SetTitleStyle(this.Builder.Style.Global.OrangeText);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.m_costsContainer = this.Builder.NewStackContainer("CostsContainer").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetInnerPadding(Offset.TopBottom(5f) + Offset.LeftRight(10f)).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      this.m_valueToPayPanel = new PricePanel(this.Builder, this.Builder.Style.PricePanel.VehiclesBuyPricePanelStyle, (Option<IAvailableProductsProvider>) (IAvailableProductsProvider) this.m_availableProductsProvider);
      this.AddUpdater(this.m_valueToPayPanel.CreateUpdater());
      this.m_valueToPayPanel.AppendTo<PricePanel>(this.m_costsContainer, new float?(this.m_valueToPayPanel.PreferredHeight));
      this.Builder.NewTxt("BtnsTitle").SetTextStyle(this.Builder.Style.Global.Title).SetText((LocStrFormatted) Tr.ShipDesignerConfirmation__Text).SetAlignment(TextAnchor.MiddleLeft).AppendTo<Txt>(this.m_costsContainer, new float?(20f));
      Txt errorText = this.Builder.NewTxt("Error").SetTextStyle(this.Builder.Style.Global.Title.Extend(new ColorRgba?(this.Builder.Style.Global.OrangeText))).SetText("").SetAlignment(TextAnchor.MiddleLeft).AppendTo<Txt>(this.m_costsContainer, new float?(20f));
      Panel parent = this.Builder.NewPanel("Buttons").AppendTo<Panel>(this.m_costsContainer, new float?(28f), Offset.Top(5f));
      Btn commitBtn = this.Builder.NewBtnPrimary("Proceed").SetText((LocStrFormatted) Tr.Continue).PlayErrorSoundWhenDisabled().OnClick(this.m_onCommitClick);
      commitBtn.PutToLeftOf<Btn>((IUiElement) parent, commitBtn.GetOptimalWidth());
      Btn objectToPlace = this.Builder.NewBtnDanger("Cancel").SetText((LocStrFormatted) Tr.Cancel).OnClick(this.m_onResetClick);
      objectToPlace.PutToLeftOf<Btn>((IUiElement) parent, objectToPlace.GetOptimalWidth(), Offset.Left(commitBtn.GetOptimalWidth() + 10f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.m_fleetManager.HasFleet && this.m_fleetManager.TravelingFleet.Dock.IsRepairing)).Observe<bool>((Func<bool>) (() => this.m_fleetManager.HasFleet && this.m_fleetManager.TravelingFleet.NeedsRepair)).Do((Action<bool, bool>) ((isRepairing, needsRepair) =>
      {
        bool isVisible = isRepairing | needsRepair;
        commitBtn.SetEnabled(!isVisible);
        if (isRepairing)
          errorText.SetText((LocStrFormatted) Tr.ShipDesigner_ShipBeingRepaired);
        else if (needsRepair)
          errorText.SetText((LocStrFormatted) Tr.ShipDesigner_ShipNeedsRepairs);
        this.m_costsContainer.SetItemVisibility((IUiElement) errorText, isVisible);
      }));
      this.AddUpdater(updaterBuilder.Build());
      this.SetContentSize(new Vector2(360f, this.m_costsContainer.GetDynamicHeight()));
    }

    public void UpdatePrices(AssetValue valueToPay)
    {
      this.m_valueToPayPanel.SetPrice(valueToPay);
      this.m_costsContainer.SetItemVisibility((IUiElement) this.m_valueToPayPanel, valueToPay.IsNotEmpty);
    }
  }
}
