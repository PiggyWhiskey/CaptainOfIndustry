// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Ships.CargoShipView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Entities;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Ships
{
  internal class CargoShipView : ItemDetailWindowView
  {
    private readonly CargoShipInspector m_controller;
    private readonly WorldMapCargoManager m_cargoManager;
    private readonly Lyst<Option<CargoShipModule>> m_syncModules;
    private StatusPanel m_statusInfo;
    private CargoShipModuleView.Cache m_buffersCache;
    private ViewsCacheHomogeneous<BufferView> m_worldBuffersCache;
    private StackContainer m_buffersContainer;
    private Txt m_departureLabel;

    private CargoShip Entity => this.m_controller.SelectedEntity;

    public CargoShipView(CargoShipInspector controller, WorldMapCargoManager cargoManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_syncModules = new Lyst<Option<CargoShipModule>>();
      // ISSUE: explicit constructor call
      base.\u002Ector("CargoShipInspector");
      this.m_cargoManager = cargoManager;
      this.m_controller = controller.CheckNotNull<CargoShipInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.MakeScrollableWithHeightLimit();
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddTitleRenameButton((Func<IEntityWithCustomTitle>) (() => (IEntityWithCustomTitle) this.Entity), this.m_controller.InputScheduler);
      this.AddEnableToggleButton((Action<bool>) (isNotPaused =>
      {
        Assert.That<bool>(isNotPaused).IsNotEqualTo<bool>(this.Entity.IsNotPaused(), "Paused state mismatch.");
        this.m_controller.InputScheduler.ScheduleInputCmd<ToggleEnabledCmd>(new ToggleEnabledCmd((IEntity) this.Entity));
      }), updaterBuilder, (Func<bool>) (() => !this.Entity.IsPaused));
      Tooltip tooltip = this.AddHelpButton();
      updaterBuilder.Observe<CargoShipProto>((Func<CargoShipProto>) (() => this.Entity.Prototype)).Do((Action<CargoShipProto>) (proto => tooltip.SetText((LocStrFormatted) this.Entity.Prototype.Strings.DescShort)));
      this.AddWorkersPanel(updaterBuilder, (Func<IEntityWithWorkers>) (() => (IEntityWithWorkers) this.Entity));
      this.m_statusInfo = this.AddStatusInfoPanel();
      Txt extraStatus = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.EntityStatus);
      this.m_departureLabel = this.AddLabel(itemContainer, "");
      this.SetupAlertsIndicator(updaterBuilder, this.m_controller.Context, (Func<IEntity>) (() => (IEntity) this.Entity));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FuelTank_Title);
      BufferView parent1 = this.Builder.NewBufferView((IUiElement) itemContainer, isCompact: true).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.CompactHeight));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.FuelBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FuelBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FuelBuffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(parent1.UpdateState));
      TextWithIcon fuelWithQuantity = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Panel.TextMedium).EnableRichText().PutToLeftBottomOf<TextWithIcon>((IUiElement) parent1, new Vector2(0.0f, 25f), Offset.Left(100f));
      fuelWithQuantity.SetSuffixText(string.Format("/ {0}", (object) Tr.FuelPerJourneySuffix));
      updaterBuilder.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.FuelProto)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FuelPerJourneyNeeded())).Observe<bool>((Func<bool>) (() => this.Entity.IsFuelReductionEnabled)).Do((Action<ProductProto, Quantity, bool>) ((fuelProto, fuelNeeded, isReducerOn) =>
      {
        fuelWithQuantity.SetIcon(fuelProto.Graphics.IconPath);
        fuelWithQuantity.SetPrefixText(isReducerOn ? string.Format("{0}: <color=#FFC000FF><b>{1}</b></color>", (object) Tr.Requires, (object) fuelNeeded) : string.Format("{0}: <b>{1}</b>", (object) Tr.Requires, (object) fuelNeeded));
      }));
      Panel unityFuelPanel = this.AddOverlayPanel(itemContainer, 40);
      CostButton useUnityBtn = new CostButton(this.Builder, Tr.RunOnLowFuel__Action.TranslatedString, "Assets/Unity/UserInterface/General/UnitySmall.svg");
      useUnityBtn.SetSuffix(string.Format("/ {0}", (object) Tr.PerJourneySuffix));
      useUnityBtn.SetButtonStyle(this.Builder.Style.Global.UpointsBtn).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<CargoShipPayWithUnityIfOutOfFuelCmd>(new CargoShipPayWithUnityIfOutOfFuelCmd(this.Entity, !this.Entity.CanPayWithUnityIfOutOfFuel)))).AddToolTip(Tr.RunOnLowFuel__Tooltip).PutToCenterMiddleOf<Btn>((IUiElement) unityFuelPanel, useUnityBtn.GetSize());
      updaterBuilder.Observe<Upoints?>((Func<Upoints?>) (() => this.Entity.GetUpointsCostIfNoFuel())).Do((Action<Upoints?>) (upointsCostIfNoFuel =>
      {
        itemContainer.SetItemVisibility((IUiElement) unityFuelPanel, upointsCostIfNoFuel.HasValue);
        if (!upointsCostIfNoFuel.HasValue)
          return;
        useUnityBtn.SetCost(upointsCostIfNoFuel.Value.ToString());
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.CanPayWithUnityIfOutOfFuel)).Do((Action<bool>) (useUnity => useUnityBtn.SetButtonStyle(useUnity ? this.Builder.Style.Global.UpointsBtnActive : this.Builder.Style.Global.UpointsBtn)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoShip_JourneyOptions);
      Panel parent2 = this.Builder.NewPanel("Journey").SetBackground(this.Style.Panel.ItemOverlayDark).AppendTo<Panel>(itemContainer, new float?((float) (2.0 * (double) this.Builder.Style.Panel.LineHeight + 10.0)));
      string stringRounded1 = (100.Percent() / CargoShip.SAVER_TRAVEL_DURATION_MULT).ToStringRounded();
      string stringRounded2 = CargoShip.SAVER_FUEL_MULT.InverseTo100().ToStringRounded();
      SwitchBtn fuelSaverSwitch = this.Builder.NewSwitchBtn().SetFontSize(14).SetText((LocStrFormatted) Tr.CargoShip_FuelSaver__Toggle).AddTooltip(Tr.CargoShip_FuelSaver__Tooltip.Format(stringRounded1, stringRounded2)).SetOnToggleAction((Action<bool>) (isOn => this.m_controller.InputScheduler.ScheduleInputCmd<CargoShipSetFuelSaverCmd>(new CargoShipSetFuelSaverCmd(this.Entity, isOn))));
      fuelSaverSwitch.PutToLeftTopOf<SwitchBtn>((IUiElement) parent2, new Vector2(fuelSaverSwitch.GetWidth(), this.Builder.Style.Panel.LineHeight), Offset.Left(this.Builder.Style.Panel.Indent) + Offset.Top(5f));
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsFuelReductionEnabled)).Do((Action<bool>) (isEnabled => fuelSaverSwitch.SetIsOn(isEnabled)));
      TextWithIcon journeyDuration = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.TextMedium).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").EnableRichText().PutToLeftBottomOf<TextWithIcon>((IUiElement) parent2, new Vector2(0.0f, this.Builder.Style.Panel.LineHeight), Offset.Left(this.Builder.Style.Panel.Indent) + Offset.Bottom(5f));
      updaterBuilder.Observe<Duration>((Func<Duration>) (() => this.Entity.JourneyDuration)).Observe<bool>((Func<bool>) (() => this.Entity.IsFuelReductionEnabled)).Do((Action<Duration, bool>) ((duration, isReducerOn) =>
      {
        journeyDuration.SetPrefixText(string.Format("{0}: ", (object) Tr.CargoShip_TripDuration) + (isReducerOn ? string.Format("<color=#FFC000FF><b>{0}</b></color>", (object) duration.Seconds.ToIntRounded()) : string.Format("<b>{0}</b>", (object) duration.Seconds.ToIntRounded())));
        Fix64 fix64 = duration.Months.ToFix64();
        journeyDuration.SetSuffixText(" (" + TrCore.NumberOfMonths.Format(fix64.ToStringRounded(1), fix64).Value + ")");
      }));
      this.Builder.AddTooltipFor<TextWithIcon>((IUiElementWithHover<TextWithIcon>) journeyDuration).SetText((LocStrFormatted) Tr.CargoShip_TripDuration__Tooltip);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoTitle);
      this.m_buffersContainer = this.Builder.NewStackContainer("Buffers").SetStackingDirection(StackContainer.Direction.TopToBottom).AppendTo<StackContainer>(itemContainer, new float?(0.0f));
      this.m_buffersCache = new CargoShipModuleView.Cache((IUiElement) this.m_buffersContainer, this.Builder);
      updaterBuilder.Observe<CargoShip.ShipState>((Func<CargoShip.ShipState>) (() => this.Entity.State)).Observe<CargoShip.DockedStatus>((Func<CargoShip.DockedStatus>) (() => this.Entity.LastDockedStatus)).Do((Action<CargoShip.ShipState, CargoShip.DockedStatus>) ((state, dockedStatus) =>
      {
        this.updateStatusInfo(state, dockedStatus);
        bool isVisible = false;
        if (state == CargoShip.ShipState.Docked)
        {
          isVisible = true;
          switch (dockedStatus)
          {
            case CargoShip.DockedStatus.NoModulesBuilt:
              this.m_departureLabel.SetText((LocStrFormatted) Tr.CargoShip__NoModulesBuilt);
              break;
            case CargoShip.DockedStatus.ShipIsBeingUnloaded:
              this.m_departureLabel.SetText((LocStrFormatted) Tr.CargoShip__ShipIsBeingUnloaded);
              break;
            case CargoShip.DockedStatus.NothingToPickUp:
              this.m_departureLabel.SetText((LocStrFormatted) Tr.CargoShip__NothingToPickUp);
              break;
            case CargoShip.DockedStatus.NotEnoughToPickUp:
              this.m_departureLabel.SetText((LocStrFormatted) Tr.CargoShip__NotEnoughToPickUp);
              break;
            default:
              isVisible = false;
              break;
          }
        }
        itemContainer.SetItemVisibility((IUiElement) extraStatus, isVisible);
        itemContainer.SetItemVisibility((IUiElement) this.m_departureLabel, isVisible);
      }));
      updaterBuilder.Observe<string>((Func<string>) (() => this.Entity.GetTitle())).Observe<bool>((Func<bool>) (() => this.Entity.IsPaused)).Do((Action<string, bool>) ((title, isPaused) =>
      {
        this.SetTitle(title, isPaused);
        this.m_headerText.SetWidth<Txt>(this.m_headerText.GetPreferedWidth().Min(this.m_headerHolder.GetWidth() - 22f));
      }));
      Txt quantitiesTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.WorldCargo__Title);
      StackContainer worldBuffersContainer = this.Builder.NewStackContainer("Buffers").SetStackingDirection(StackContainer.Direction.TopToBottom).AppendTo<StackContainer>(itemContainer);
      this.m_worldBuffersCache = new ViewsCacheHomogeneous<BufferView>((Func<BufferView>) (() => this.Builder.NewBufferView((IUiElement) worldBuffersContainer, isCompact: true).SetAsSuperCompact()));
      Lyst<WorldMapCargoManager.WorldCargoData> cache = new Lyst<WorldMapCargoManager.WorldCargoData>();
      updaterBuilder.Observe<WorldMapCargoManager.WorldCargoData>((Func<IIndexable<WorldMapCargoManager.WorldCargoData>>) (() =>
      {
        this.m_cargoManager.GetAvailableWorldCargo(this.Entity, cache);
        return (IIndexable<WorldMapCargoManager.WorldCargoData>) cache;
      }), (ICollectionComparator<WorldMapCargoManager.WorldCargoData, IIndexable<WorldMapCargoManager.WorldCargoData>>) CompareFixedOrder<WorldMapCargoManager.WorldCargoData>.Instance).Do((Action<Lyst<WorldMapCargoManager.WorldCargoData>>) (cargo =>
      {
        quantitiesTitle.SetVisibility<Txt>(cargo.IsNotEmpty);
        this.m_worldBuffersCache.ReturnAll();
        worldBuffersContainer.StartBatchOperation();
        worldBuffersContainer.ClearAll();
        foreach (WorldMapCargoManager.WorldCargoData worldCargoData in cargo)
        {
          BufferView view = this.m_worldBuffersCache.GetView();
          view.UpdateState(worldCargoData.Product, worldCargoData.Capacity, worldCargoData.Quantity);
          view.Show<BufferView>();
          view.AppendTo<BufferView>(worldBuffersContainer, new float?(this.Builder.Style.BufferView.SuperCompactHeight));
        }
        worldBuffersContainer.FinishBatchOperation();
      }));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.Orders);
      StackContainer container = this.AddButtonsSection(itemContainer, ItemDetailWindowView.VEHICLE_BTN_SIZE + 10, Offset.Zero);
      Btn forceDepartBtn = this.Builder.NewBtnGeneral("ForceDepartBtn").SetText((LocStrFormatted) TrCore.CargoShipDepartNow__Action).OnClick((Action) (() => this.m_controller.Context.InputScheduler.ScheduleInputCmd<CargoShipDepartNowCmd>(new CargoShipDepartNowCmd(this.Entity))));
      Tooltip departTooltip = forceDepartBtn.AddToolTipAndReturn();
      forceDepartBtn.AppendTo<Btn>(container, new float?(forceDepartBtn.GetOptimalWidth()));
      LocStrFormatted reason;
      updaterBuilder.Observe<KeyValuePair<bool, LocStrFormatted>>((Func<KeyValuePair<bool, LocStrFormatted>>) (() => Make.Kvp<bool, LocStrFormatted>(this.Entity.IsDepartNowAvailable(out reason), reason))).Do((Action<KeyValuePair<bool, LocStrFormatted>>) (forceDepartResult =>
      {
        forceDepartBtn.SetEnabled(forceDepartResult.Key);
        departTooltip.SetText(forceDepartResult.Value.IsNotEmpty ? forceDepartResult.Value : (LocStrFormatted) TrCore.CargoShipDepartNow__Tooltip);
      }));
      CargoShipFuelReplaceView objectToPlace = new CargoShipFuelReplaceView(this.Builder, this.m_controller.Context, (IUiElement) this.ItemsContainer, (WindowView) this, this.AddOverlay((Action) (() => { })), (Func<CargoShip>) (() => this.Entity));
      this.AddUpdater(objectToPlace.Updater);
      objectToPlace.AppendTo<CargoShipFuelReplaceView>(container);
      this.AddUpdater(updaterBuilder.Build());
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_syncModules.Clear();
      this.m_syncModules.AddRange(this.Entity.Modules);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.updateModules();
      this.InputUpdateForRenaming();
    }

    private void updateModules()
    {
      this.m_buffersContainer.ClearAll();
      this.m_buffersCache.ReturnAll();
      foreach (Option<CargoShipModule> syncModule in this.m_syncModules)
      {
        CargoShipModuleView view = this.m_buffersCache.GetView();
        view.Show<CargoShipModuleView>();
        view.AppendTo<CargoShipModuleView>(this.m_buffersContainer, new float?(view.RequiredHeight));
        view.Update(syncModule);
      }
    }

    private void updateStatusInfo(CargoShip.ShipState state, CargoShip.DockedStatus dockedStatus)
    {
      switch (state)
      {
        case CargoShip.ShipState.ArrivingFromWorld:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__Ship_Arriving);
          break;
        case CargoShip.ShipState.Docked:
          switch (dockedStatus)
          {
            case CargoShip.DockedStatus.NotEnoughFuel:
              this.m_statusInfo.SetStatus(TrCore.EntityStatus__NeedsFuel, StatusPanel.State.Critical);
              return;
            case CargoShip.DockedStatus.Paused:
              this.m_statusInfo.SetStatusPaused();
              return;
            case CargoShip.DockedStatus.NotEnoughWorkers:
              this.m_statusInfo.SetStatusNoWorkers();
              return;
            default:
              this.m_statusInfo.SetStatus(Tr.EntityStatus__Ship_Docked);
              return;
          }
        case CargoShip.ShipState.DepartingToWorld:
          this.m_statusInfo.SetStatus(Tr.EntityStatus__Ship_Departing);
          break;
      }
    }
  }
}
