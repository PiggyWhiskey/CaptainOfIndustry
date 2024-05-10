// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipUpgradeProgressPanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Shipyard;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Localization;
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
  public class ShipUpgradeProgressPanel : WindowView
  {
    private readonly TravelingFleetManager m_fleetManager;
    private readonly IInputScheduler m_inputScheduler;

    public ShipUpgradeProgressPanel(
      IInputScheduler inputScheduler,
      TravelingFleetManager fleetManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("UpgradeInProgress", WindowView.FooterStyle.None);
      this.m_fleetManager = fleetManager;
      this.m_inputScheduler = inputScheduler;
    }

    protected override void BuildWindowContent()
    {
      this.SetTitle("");
      this.SetTitleStyle(this.Builder.Style.Global.OrangeText);
      StackContainer stackContainer = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetInnerPadding(Offset.All(5f)).PutTo<StackContainer>((IUiElement) this.GetContentPanel());
      ConstructionProgressView objectToPlace1 = new ConstructionProgressView((IUiElement) stackContainer, this.Builder, (Func<Option<IConstructionProgress>>) (() => this.m_fleetManager.TravelingFleet.Dock.ModificationProgress));
      objectToPlace1.AppendTo<ConstructionProgressView>(stackContainer, new float?(95f)).SetBackground(this.Builder.Style.Panel.ItemOverlay);
      this.AddUpdater(objectToPlace1.Updater);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      Panel parent = this.Builder.NewPanel("ModifInfoHolder").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(stackContainer, new float?(40f));
      Txt modifInfoText = this.Builder.NewTxt("ModifInfo").SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.Style.Global.Text).PutTo<Txt>((IUiElement) parent, Offset.LeftRight(10f));
      Panel panel = this.Builder.NewPanel("CancelHolder");
      Btn objectToPlace2 = this.Builder.NewBtn("Cancel").SetText((LocStrFormatted) Tr.Cancel).SetButtonStyle(this.Style.Global.DangerBtn);
      objectToPlace2.PutToLeftMiddleOf<Btn>((IUiElement) panel, objectToPlace2.GetOptimalSize()).OnClick(new Action(this.cancelModifications));
      Btn pauseModifsBtn = this.Builder.NewBtn("Pause").SetText("Enable input").SetButtonStyle(this.Style.Global.GeneralBtn);
      pauseModifsBtn.PutToRightMiddleOf<Btn>((IUiElement) panel, pauseModifsBtn.GetOptimalSize()).OnClick(new Action(this.toggleWorksPause));
      panel.AppendTo<Panel>(stackContainer, new Vector2?(new Vector2(objectToPlace2.GetOptimalWidth() + 10f + pauseModifsBtn.GetOptimalWidth(), 30f)), ContainerPosition.MiddleOrCenter, Offset.Top(10f));
      updaterBuilder.Observe<ShipModificationState>((Func<ShipModificationState>) (() => this.m_fleetManager.TravelingFleet.Dock.ModificationState)).Do((Action<ShipModificationState>) (state =>
      {
        switch (state)
        {
          case ShipModificationState.Preparing:
            this.SetTitle((LocStrFormatted) Tr.ShipUpgrade_Preparing);
            modifInfoText.SetText((LocStrFormatted) Tr.ShipUpgrade_Preparing__Tooltip);
            break;
          case ShipModificationState.Prepared:
            this.SetTitle((LocStrFormatted) Tr.ShipUpgrade_ReadyWaiting);
            modifInfoText.SetText((LocStrFormatted) Tr.ShipUpgrade_ReadyWaiting__Tooltip);
            break;
          case ShipModificationState.Applying:
            this.SetTitle((LocStrFormatted) Tr.ShipUpgrade_Performing);
            modifInfoText.SetText((LocStrFormatted) Tr.ShipUpgrade_Performing__Tooltip);
            break;
        }
      }));
      updaterBuilder.Observe<ShipModificationState>((Func<ShipModificationState>) (() => this.m_fleetManager.TravelingFleet.Dock.ModificationState)).Observe<bool>((Func<bool>) (() => this.m_fleetManager.TravelingFleet.Dock.CargoInputPaused)).Do((Action<ShipModificationState, bool>) ((state, isPaused) =>
      {
        pauseModifsBtn.SetVisibility<Btn>(state == ShipModificationState.Preparing);
        pauseModifsBtn.SetText((LocStrFormatted) (isPaused ? Tr.Input__Enable : Tr.Input__Pause));
        pauseModifsBtn.SetButtonStyle(isPaused ? this.Style.Global.GeneralBtnActive : this.Style.Global.GeneralBtn);
      }));
      this.AddUpdater(updaterBuilder.Build());
    }

    private void cancelModifications()
    {
      this.m_inputScheduler.ScheduleInputCmd<FleetModificationsCancelCmd>(new FleetModificationsCancelCmd());
    }

    private void toggleWorksPause()
    {
      this.m_inputScheduler.ScheduleInputCmd<ShipyardToggleWorksPauseCmd>(new ShipyardToggleWorksPauseCmd(this.m_fleetManager.TravelingFleet.Dock.Id));
    }
  }
}
