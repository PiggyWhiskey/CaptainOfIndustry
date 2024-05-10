// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Fleet.TravelingFleetWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Shipyard;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
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
namespace Mafi.Unity.InputControl.Inspectors.Fleet
{
  internal class TravelingFleetWindowView : ItemDetailWindowView
  {
    private readonly TravelingFleetManager m_fleetManager;
    private readonly TravelingFleetInspector m_controller;
    private ProductQuantitiesView m_cargoView;

    protected TravelingFleet Ship => this.m_controller.SelectedEntity;

    public TravelingFleetWindowView(
      TravelingFleetInspector controller,
      TravelingFleetManager fleetManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("TravelingFleetInspector");
      this.m_fleetManager = fleetManager;
      this.m_controller = controller.CheckNotNull<TravelingFleetInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      UpdaterBuilder updater = UpdaterBuilder.Start();
      this.AddTitleRenameButton((Func<IEntityWithCustomTitle>) (() => (IEntityWithCustomTitle) this.Ship), this.m_controller.InputScheduler);
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ShipCrew, new LocStrFormatted?((LocStrFormatted) Tr.ShipCrew__Tooltip));
      Panel parent = this.AddOverlayPanel(this.ItemsContainer, 40);
      TextWithIcon textWithIcon = new TextWithIcon(this.Builder, 25);
      TextStyle title1 = this.Builder.Style.Global.Title;
      ref TextStyle local = ref title1;
      int? nullable = new int?(16);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      TextWithIcon crewTxt = textWithIcon.SetTextStyle(textStyle).SetIcon("Assets/Unity/UserInterface/General/Sailor.svg").SetSuffixText(" 100 / 100");
      crewTxt.PutToLeftOf<TextWithIcon>((IUiElement) parent, crewTxt.GetWidth(), Offset.Left(this.Style.Panel.Indent));
      this.AddSwitch(itemContainer, Tr.ShipAutoReturn__Toggle.TranslatedString, (Action<bool>) (x => this.m_controller.ScheduleInputCmd<FleetToggleAutoReturnCmd>(new FleetToggleAutoReturnCmd())), updater, (Func<bool>) (() => this.Ship.IsAutoReturnEnabled), Tr.ShipAutoReturn__Tooltip.TranslatedString);
      Panel actionsContainer = this.Builder.NewPanel("crew actions const").PutToLeftOf<Panel>((IUiElement) parent, 200f, Offset.Left((float) ((double) crewTxt.GetSize().x + (double) this.Style.Panel.Indent + 10.0)));
      Btn loadCrewBtn = this.Builder.NewBtnPrimary("Load crew").PlayErrorSoundWhenDisabled().SetText((LocStrFormatted) Tr.ShipCrew__Load);
      Vector2 size1 = loadCrewBtn.GetOptimalSize() + new Vector2(25f, 0.0f);
      loadCrewBtn.SetSize<Btn>(size1).PutToLeftMiddleOf<Btn>((IUiElement) actionsContainer, size1).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<FleetLoadCrewCmd>(new FleetLoadCrewCmd())));
      Btn unloadCrewBtn = this.Builder.NewBtnPrimary("Unload crew").SetText((LocStrFormatted) Tr.ShipCrew__Unload);
      Vector2 size2 = unloadCrewBtn.GetOptimalSize() + new Vector2(25f, 0.0f);
      unloadCrewBtn.SetSize<Btn>(size2).PutToLeftMiddleOf<Btn>((IUiElement) actionsContainer, size2).OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<FleetUnloadCrewCmd>(new FleetUnloadCrewCmd())));
      updater.Observe<bool>((Func<bool>) (() => this.Ship.IsDocked)).Observe<int>((Func<int>) (() => this.Ship.CurrentCrew)).Observe<int>((Func<int>) (() => this.Ship.CrewRequired)).Do((Action<bool, int, int>) ((isDocked, crew, crewRequired) =>
      {
        bool visibility = crew == crewRequired;
        actionsContainer.SetVisibility<Panel>(isDocked);
        crewTxt.SetSuffixText(string.Format(" {0} / {1}", (object) crew, (object) crewRequired));
        loadCrewBtn.SetVisibility<Btn>(!visibility);
        unloadCrewBtn.SetVisibility<Btn>(visibility);
      }));
      Txt cargoTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoTitle);
      this.m_cargoView = new ProductQuantitiesView((IUiElement) itemContainer, this.Builder);
      this.m_cargoView.AppendTo<ProductQuantitiesView>(itemContainer, new float?(0.0f));
      Panel shipyardFullWarningHolder = this.Builder.NewPanel("ShipyardFull").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(20f));
      this.Builder.NewTxt("ShipyardFull").SetColor(this.Builder.Style.Global.DangerClr).SetText((LocStrFormatted) Tr.ShipCannotUnload).SetAlignment(TextAnchor.UpperCenter).PutTo<Txt>((IUiElement) shipyardFullWarningHolder);
      Lyst<ProductQuantity> cargoCache = new Lyst<ProductQuantity>();
      updater.Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
      {
        cargoCache.Clear();
        foreach (IProductBuffer productBuffer in this.Ship.Cargo.Values)
          cargoCache.Add(productBuffer.Product.WithQuantity(productBuffer.Quantity));
        return (IIndexable<ProductQuantity>) cargoCache;
      }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Do((Action<Lyst<ProductQuantity>>) (cargo =>
      {
        this.m_cargoView.SetProducts((IIndexable<ProductQuantity>) cargo);
        itemContainer.SetItemVisibility((IUiElement) cargoTitle, cargo.IsNotEmpty);
        itemContainer.SetItemVisibility((IUiElement) this.m_cargoView, cargo.IsNotEmpty);
      }));
      updater.Observe<string>((Func<string>) (() => this.Ship.GetTitle())).Observe<bool>((Func<bool>) (() => this.Ship.CurrentHp > this.Ship.MinOperableHp)).Do((Action<string, bool>) ((title, hasEnoughHp) =>
      {
        this.SetTitle(hasEnoughHp ? title : Tr.DamagedSuffix.Format(title).Value);
        this.m_headerText.SetWidth<Txt>(this.m_headerText.GetPreferedWidth().Min(this.m_headerHolder.GetWidth() - 22f));
        loadCrewBtn.SetEnabled(hasEnoughHp);
      }));
      updater.Observe<bool>((Func<bool>) (() => this.Ship.IsDocked && this.Ship.Cargo.IsNotEmpty<KeyValuePair<ProductProto, IProductBuffer>>() && this.Ship.Dock.IsFull)).Do((Action<bool>) (isOutOfSpaceForCargo => itemContainer.SetItemVisibility((IUiElement) shipyardFullWarningHolder, isOutOfSpaceForCargo)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FuelTank_Title);
      Btn unloadFuelBtn = this.Builder.NewBtnGeneral("unloadFuel").SetText((LocStrFormatted) Tr.ShipFuelUnload).AddToolTip(Tr.ShipFuelUnload__Tooltip).UseSmallTextIfNeeded().OnClick((Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<FleetUnloadFuelCmd>(new FleetUnloadFuelCmd()))).SetWidth<Btn>(80f);
      updater.Observe<bool>((Func<bool>) (() => this.Ship.IsDocked && this.Ship.FuelBuffer.IsNotEmpty())).Do((Action<bool>) (canUnload => unloadFuelBtn.SetEnabled(canUnload)));
      BufferView bufferView = this.Builder.NewBufferView((IUiElement) itemContainer, rightButton: unloadFuelBtn).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.Height));
      updater.Observe<ProductProto>((Func<ProductProto>) (() => this.Ship.FuelBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Ship.FuelBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Ship.FuelBuffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(bufferView.UpdateState));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ShipHealth__Title, new LocStrFormatted?((LocStrFormatted) Tr.ShipHealth__Tooltip));
      SwitchBtn autoRepairSwitch = this.AddSwitch(itemContainer, Tr.ShipAutoRepair__Toggle.TranslatedString, (Action<bool>) (x => this.m_controller.ScheduleInputCmd<ShipyardToggleAutoRepairCmd>(new ShipyardToggleAutoRepairCmd(this.Ship.Dock.Id))), updater, (Func<bool>) (() => this.Ship.Dock.IsAutoRepairEnabled), Tr.ShipAutoRepair__Tooltip.TranslatedString);
      QuantityBar hpBar = new QuantityBar(this.Builder).PutTo<QuantityBar>((IUiElement) this.Builder.NewPanel("Bar container").SetBackground(this.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(this.Style.BufferView.Height)), Offset.All(20f));
      QuantityBar.Marker hpMidMarker = hpBar.AddMarker(Percent.Zero, (ColorRgba) 65280);
      updater.Observe<int>((Func<int>) (() => this.m_fleetManager.TravelingFleet.CurrentHp)).Observe<int>((Func<int>) (() => this.m_fleetManager.TravelingFleet.MinOperableHp)).Observe<int>((Func<int>) (() => this.m_fleetManager.TravelingFleet.MaxHp)).Do((Action<int, int, int>) ((currentHp, minOperableHp, maxHp) =>
      {
        Percent percentFull = Percent.FromRatio(currentHp, maxHp);
        hpMidMarker.SetPosition(Percent.FromRatio(minOperableHp, maxHp));
        hpBar.UpdateValues(percentFull, string.Format("{0}/{1} ({2})", (object) currentHp, (object) maxHp, (object) percentFull.ToStringRounded()));
        hpBar.SetColor(currentHp < minOperableHp ? (ColorRgba) 12942120 : this.Style.QuantityBar.PositiveBarColor);
      }));
      ConstructionProgressView repairView = new ConstructionProgressView((IUiElement) itemContainer, this.Builder, (Func<Option<IConstructionProgress>>) (() => this.Ship.Dock.RepairProgress));
      repairView.AppendTo<ConstructionProgressView>(itemContainer, new float?(95f)).SetBackground(this.Builder.Style.Panel.ItemOverlay);
      this.AddUpdater(repairView.Updater);
      Panel btnHolder = this.Builder.NewPanel("RepairBtnHolder").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(40f));
      Btn repairBtn = this.Builder.NewBtnPrimary("Repair").PlayErrorSoundWhenDisabled().SetText((LocStrFormatted) Tr.Repair);
      Tooltip repairBtnTooltip = this.Builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) repairBtn).SetErrorTextStyle();
      repairBtn.PutToCenterTopOf<Btn>((IUiElement) btnHolder, repairBtn.GetOptimalSize() + new Vector2(40f, 0.0f)).OnClick((Action) (() => this.m_controller.ToggleRepair(this.Ship.Dock.Id, !this.Ship.Dock.IsRepairing)));
      Btn cancelBtn = this.Builder.NewBtn("Stop").SetButtonStyle(this.Builder.Style.Global.GeneralBtn).SetText((LocStrFormatted) Tr.Cancel);
      cancelBtn.PutToCenterTopOf<Btn>((IUiElement) btnHolder, cancelBtn.GetOptimalSize() + new Vector2(40f, 0.0f)).OnClick((Action) (() => this.m_controller.ToggleRepair(this.Ship.Dock.Id, !this.Ship.Dock.IsRepairing)));
      updater.Observe<ShipyardProto>((Func<ShipyardProto>) (() => this.Ship.Dock.Prototype)).Observe<bool>((Func<bool>) (() => this.Ship.Dock.CanRepair)).Observe<bool>((Func<bool>) (() => this.Ship.Dock.IsRepairing)).Observe<ShipModificationState>((Func<ShipModificationState>) (() => this.Ship.Dock.ModificationState)).Do((Action<ShipyardProto, bool, bool, ShipModificationState>) ((proto, canRepair, isRepairing, modifState) =>
      {
        cancelBtn.SetVisibility<Btn>(isRepairing);
        repairBtn.SetVisibility<Btn>(!isRepairing);
        repairBtnTooltip.SetText((LocStrFormatted) (proto.CanRepair ? LocStr.Empty : Tr.ShipyardNeedsRepairs));
        itemContainer.SetItemVisibility((IUiElement) autoRepairSwitch, proto.CanRepair);
        repairBtn.SetEnabled(canRepair && modifState == ShipModificationState.None);
        itemContainer.SetItemVisibility((IUiElement) btnHolder, modifState == ShipModificationState.None);
        itemContainer.SetItemVisibility((IUiElement) repairView, isRepairing);
      }));
      CustomPriorityPanel customPriorityPanel = CustomPriorityPanel.NewForShipRepairImport((IUiElement) repairView, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Ship.Dock));
      customPriorityPanel.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) repairView, customPriorityPanel.GetSize(), Offset.Right((float) (-(double) customPriorityPanel.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel.Updater);
      this.AddUpdater(updater.Build());
      this.SetWidth(500f);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.InputUpdateForRenaming();
    }
  }
}
