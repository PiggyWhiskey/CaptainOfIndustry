// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.ShipDesign.ShipDesignerView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Fleet;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.Utils;
using Mafi.Core.World;
using Mafi.Localization;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.ShipDesign
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ShipDesignerView : WindowView
  {
    public static int WIDTH;
    private readonly IInputScheduler m_inputScheduler;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly ProtosDb m_protosDb;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDbForUi;
    private readonly ProductsAvailableInStorage m_availableProductsProvider;
    private GridContainer m_slotsContainer;
    private ShipPartPickerDialog m_shipPartPickerDialog;
    private ShipUpgradeConfirmationPanel m_shipUpgradeConfirmationPanel;
    private ShipUpgradeProgressPanel m_shipUpgradeProgressPanel;
    private ShipStatsPanel m_shipStatsPanel;
    private TravelingFleet m_fleet;
    private ViewsCacheHomogeneous<ShipSlotView> m_groupViewsCache;
    private readonly Lyst<ShipSlotWrapper> m_groups;
    private FleetEntity m_entity;
    private FleetEntityModificationRequest? m_latestModifRequest;
    private bool m_isModificationInProgress;
    private float m_bottomStripHeight;

    public ShipDesignerView(
      IInputScheduler inputScheduler,
      IGameLoopEvents gameLoop,
      ProtosDb protosDb,
      TravelingFleetManager fleetManager,
      UnlockedProtosDbForUi unlockedProtosDbForUi,
      IAssetTransactionManager assetsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_groups = new Lyst<ShipSlotWrapper>();
      // ISSUE: explicit constructor call
      base.\u002Ector("ShipDesigner");
      this.m_inputScheduler = inputScheduler;
      this.m_gameLoop = gameLoop;
      this.m_protosDb = protosDb;
      this.m_fleetManager = fleetManager;
      this.m_unlockedProtosDbForUi = unlockedProtosDbForUi;
      this.m_availableProductsProvider = new ProductsAvailableInStorage(assetsManager);
      this.OnHide += new Action(this.closeUpgraderPicker);
      this.ShowAfterSync = true;
    }

    private void onFleetCreated(TravelingFleet fleet)
    {
      if (this.m_fleet == null)
        this.RefreshData(fleet.FleetEntity);
      this.m_fleet = fleet;
      new DelayedEvent<TravelingFleet>(this.m_gameLoop, (Action<Action<TravelingFleet>>) (action => fleet.OnModificationsDone += action)).OnSync += new Action<TravelingFleet>(this.onModificationDone);
      this.m_unlockedProtosDbForUi.OnUnlockedSetChangedForUi += (Action) (() => this.RefreshData(this.m_fleet.FleetEntity));
    }

    private void onModificationDone(TravelingFleet fleet) => this.RefreshData(fleet.FleetEntity);

    protected override void BuildWindowContent()
    {
      this.SetTitle((LocStrFormatted) Tr.ShipDesigner);
      this.SetContentSize((float) ShipDesignerView.WIDTH, 0.0f);
      this.PositionSelfToCenter();
      this.m_groupViewsCache = new ViewsCacheHomogeneous<ShipSlotView>((Func<ShipSlotView>) (() => new ShipSlotView(this.Builder, this.m_unlockedProtosDbForUi)));
      this.m_shipPartPickerDialog = new ShipPartPickerDialog(this.m_unlockedProtosDbForUi);
      this.m_shipPartPickerDialog.SetOnCloseButtonClickAction(new Action(this.closeUpgraderPicker));
      this.m_shipPartPickerDialog.BuildUi(this.Builder);
      this.m_shipUpgradeConfirmationPanel = new ShipUpgradeConfirmationPanel(this.m_fleetManager, this.m_availableProductsProvider, new Action(this.onCommitChangesClick), new Action(this.onResetChangesClick));
      this.m_shipUpgradeConfirmationPanel.BuildUi(this.Builder);
      this.m_shipUpgradeProgressPanel = new ShipUpgradeProgressPanel(this.m_inputScheduler, this.m_fleetManager);
      this.m_shipUpgradeProgressPanel.BuildUi(this.Builder);
      this.m_shipStatsPanel = new ShipStatsPanel();
      this.m_shipStatsPanel.BuildUi(this.Builder);
      this.m_shipStatsPanel.Show();
      this.m_bottomStripHeight = this.m_shipUpgradeConfirmationPanel.GetHeight().Max(this.m_shipStatsPanel.GetHeight()).Max(this.m_shipUpgradeProgressPanel.GetHeight());
      Panel bottomOf = this.Builder.NewPanel("BottomStrip").SetBackground(this.Builder.Style.Panel.ItemOverlayDark).PutToBottomOf<Panel>((IUiElement) this.GetContentPanel(), this.m_bottomStripHeight);
      this.m_shipUpgradeConfirmationPanel.PutToRightOf<ShipUpgradeConfirmationPanel>((IUiElement) bottomOf, this.m_shipUpgradeConfirmationPanel.GetWidth());
      this.m_shipUpgradeProgressPanel.PutToRightOf<ShipUpgradeProgressPanel>((IUiElement) bottomOf, this.m_shipUpgradeConfirmationPanel.GetWidth());
      this.m_shipStatsPanel.PutToLeftOf<ShipStatsPanel>((IUiElement) bottomOf, this.m_shipStatsPanel.GetWidth());
      this.m_slotsContainer = this.Builder.NewGridContainer("Slots").SetCellSize(112.Vector2()).SetCellSpacing(5f).SetDynamicHeightMode(4).PutToCenterTopOf<GridContainer>((IUiElement) this.GetContentPanel(), Vector2.zero);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<Option<IConstructionProgress>>((Func<Option<IConstructionProgress>>) (() => !this.m_fleetManager.HasFleet ? Option<IConstructionProgress>.None : this.m_fleetManager.TravelingFleet.Dock.ModificationProgress)).Do((Action<Option<IConstructionProgress>>) (pendingModificationMaybe =>
      {
        this.m_isModificationInProgress = pendingModificationMaybe.HasValue;
        this.m_shipUpgradeProgressPanel.SetVisibility<ShipUpgradeProgressPanel>(this.m_isModificationInProgress);
        this.updatePendingChangesDialog();
      }));
      this.AddUpdater(updaterBuilder.Build());
      if (this.m_fleetManager.HasFleet)
        this.onFleetCreated(this.m_fleetManager.TravelingFleet);
      else
        this.m_fleetManager.OnFleetCreated += new Action<TravelingFleet>(this.onFleetCreated);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_shipUpgradeConfirmationPanel.RenderUpdate(gameTime);
      this.m_shipUpgradeProgressPanel.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_shipUpgradeConfirmationPanel.SyncUpdate(gameTime);
      this.m_shipUpgradeProgressPanel.SyncUpdate(gameTime);
    }

    public void RefreshData(FleetEntity entity)
    {
      this.m_entity = entity;
      this.m_groups.Clear();
      foreach (FleetEntitySlot slot in this.m_entity.Slots)
        this.m_groups.Add(new ShipSlotWrapper(slot));
      this.rebuildGroupsViews();
      this.m_latestModifRequest = new FleetEntityModificationRequest?();
      this.m_shipUpgradeConfirmationPanel.Hide();
      this.updatePendingChangesDialog();
      this.closeUpgraderPicker();
    }

    private void rebuildGroupsViews()
    {
      this.m_slotsContainer.ClearAll();
      this.m_groupViewsCache.ReturnAll();
      foreach (ShipSlotWrapper group in this.m_groups)
      {
        ShipSlotView view = this.m_groupViewsCache.GetView();
        view.AppendTo<ShipSlotView>(this.m_slotsContainer);
        view.SetGroup(group, new Action<ShipSlotWrapper>(this.onPlusClick), new Action<ShipSlotWrapper>(this.onSlotClick));
      }
      this.m_slotsContainer.PutToCenterTopOf<GridContainer>((IUiElement) this.GetContentPanel(), new Vector2(this.m_slotsContainer.GetRequiredWidth(), this.m_slotsContainer.GetHeight()), Offset.Top(10f));
      this.SetContentSize((float) ShipDesignerView.WIDTH, this.m_slotsContainer.GetHeight() + 10f + this.m_bottomStripHeight);
    }

    private void onSlotClick(ShipSlotWrapper group) => this.showUpgradePicker(group);

    private void onPlusClick(ShipSlotWrapper group) => this.showUpgradePicker(group);

    private void closeUpgraderPicker() => this.m_shipPartPickerDialog.Hide();

    private void onPartSetRequest(ShipSlotWrapper group, Option<FleetEntityPartProto> partProto)
    {
      int num = partProto.HasValue ? 1 : 0;
      group.SelectedPart = partProto;
      this.closeUpgraderPicker();
      this.rebuildGroupsViews();
      this.updatePendingChangesDialog();
    }

    private void showUpgradePicker(ShipSlotWrapper group)
    {
      this.m_shipPartPickerDialog.SetUpgrades(group, this.m_isModificationInProgress, new Action<ShipSlotWrapper, Option<FleetEntityPartProto>>(this.onPartSetRequest));
      this.m_shipPartPickerDialog.PutToCenterMiddleOf<ShipPartPickerDialog>((IUiElement) this, this.m_shipPartPickerDialog.GetSize());
      this.m_shipPartPickerDialog.Show();
    }

    private void onCommitChangesClick()
    {
      if (!this.m_latestModifRequest.HasValue)
        Log.Error("Expected modification request to be present!");
      else
        this.m_inputScheduler.ScheduleInputCmd<FleetModificationsPrepareCmd>(new FleetModificationsPrepareCmd(this.m_latestModifRequest.Value));
    }

    private void onResetChangesClick()
    {
      foreach (ShipSlotWrapper group in this.m_groups)
        group.SelectedPart = group.Group.ExistingPart;
      this.rebuildGroupsViews();
      this.updatePendingChangesDialog();
    }

    private void updatePendingChangesDialog()
    {
      if (this.m_isModificationInProgress)
      {
        this.m_shipUpgradeConfirmationPanel.Hide();
      }
      else
      {
        this.m_latestModifRequest = new FleetEntityModificationRequest?(this.generateModificationRequest());
        AssetValue valueToPay;
        FleetEntityStats oldStats;
        FleetEntityStats newStats;
        this.m_shipUpgradeConfirmationPanel.SetVisibility<ShipUpgradeConfirmationPanel>(this.m_latestModifRequest.Value.GetPriceForModifications(this.m_entity, this.m_protosDb, out valueToPay, out oldStats, out newStats));
        this.m_shipUpgradeConfirmationPanel.UpdatePrices(valueToPay);
        this.m_shipStatsPanel.SetStats(oldStats, newStats);
      }
    }

    private FleetEntityModificationRequest generateModificationRequest()
    {
      Lyst<SlotModification> lyst = new Lyst<SlotModification>();
      foreach (ShipSlotWrapper group in this.m_groups)
      {
        Option<FleetEntityPartProto> selectedPart = group.SelectedPart;
        FleetEntityPartProto.ID? part = selectedPart.HasValue ? new FleetEntityPartProto.ID?(selectedPart.Value.Id) : new FleetEntityPartProto.ID?();
        lyst.Add(new SlotModification(group.Group.Proto.Id, part));
      }
      return new FleetEntityModificationRequest(lyst.ToImmutableArray(), new FleetEntityHullProto.ID?());
    }

    static ShipDesignerView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ShipDesignerView.WIDTH = 540;
    }
  }
}
