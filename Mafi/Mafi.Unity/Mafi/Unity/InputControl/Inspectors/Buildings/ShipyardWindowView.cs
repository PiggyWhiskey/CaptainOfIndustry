// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ShipyardWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Shipyard;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Unity.InputControl.Fleet.ShipDesign;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class ShipyardWindowView : StaticEntityInspectorBase<Mafi.Core.Buildings.Shipyard.Shipyard>
  {
    private readonly WorldMapManager m_worldMapManager;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly ShipDesignerView m_shipDesignerView;
    private readonly ShipyardInspector m_controller;
    private ProductQuantitiesView m_cargoView;
    private ViewsCacheHomogeneous<ShipyardWindowView.WorldMapEntityConstructionPanel> m_entitiesRepairPanels;
    private ShipyardWindowView.ProductDiscardConfirmDialog m_discardConfirmDialog;
    private ShipyardWindowView.ProductCannotDiscardDialog m_discardCannotDialog;
    private Panel m_overlay;

    protected override Mafi.Core.Buildings.Shipyard.Shipyard Entity
    {
      get => this.m_controller.SelectedEntity;
    }

    public ShipyardWindowView(
      ShipyardInspector controller,
      WorldMapManager worldMapManager,
      TravelingFleetManager fleetManager,
      ShipDesignerView shipDesignerView)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_worldMapManager = worldMapManager;
      this.m_fleetManager = fleetManager;
      this.m_shipDesignerView = shipDesignerView;
      this.m_controller = controller.CheckNotNull<ShipyardInspector>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.m_entitiesRepairPanels = new ViewsCacheHomogeneous<ShipyardWindowView.WorldMapEntityConstructionPanel>((Func<ShipyardWindowView.WorldMapEntityConstructionPanel>) (() => new ShipyardWindowView.WorldMapEntityConstructionPanel(this.Builder, this.m_controller.InputScheduler, (Func<Mafi.Core.Buildings.Shipyard.Shipyard>) (() => this.Entity))));
      this.AddUpdater(this.m_entitiesRepairPanels.Updater);
      UpdaterBuilder updater = UpdaterBuilder.Start();
      UiStyle style = this.Builder.Style;
      this.m_shipDesignerView.BuildUi(this.Builder);
      this.AttachSidePanel((IWindow) this.m_shipDesignerView);
      updater.Observe<bool>((Func<bool>) (() => this.Entity.CanPerformModifications)).Do((Action<bool>) (canModify => this.m_shipDesignerView.SetVisibility<ShipDesignerView>(canModify)));
      this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoTitle, new LocStrFormatted?((LocStrFormatted) Tr.ShipyardCargo__Tooltip));
      this.AddSwitch(itemContainer, TrCore.ShipyardKeepEmpty.TranslatedString, (Action<bool>) (x => this.m_controller.ScheduleInputCmd<ShipyardToggleUnloadPriorityCmd>(new ShipyardToggleUnloadPriorityCmd(this.Entity.Id))), updater, (Func<bool>) (() => this.Entity.HasHighCargoUnloadPrio), Tr.ShipyardKeepEmpty__Tooltip.TranslatedString);
      this.m_cargoView = new ProductQuantitiesView((IUiElement) itemContainer, this.Builder, new Action<ProductProto>(this.onDiscardProductClick));
      this.m_cargoView.AppendTo<ProductQuantitiesView>(itemContainer, new float?(0.0f));
      Panel shipyardFullWarningHolder = this.Builder.NewPanel("ShipyardFull").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(itemContainer, new float?(20f));
      this.Builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) shipyardFullWarningHolder).SetText((LocStrFormatted) Tr.ShipyardFullMessage__Tooltip);
      this.Builder.NewTxt("ShipyardFull").SetTextStyle(this.Builder.Style.Global.Text.Extend(new ColorRgba?(this.Builder.Style.Global.OrangeText))).SetText((LocStrFormatted) Tr.ShipyardFullMessage).SetAlignment(TextAnchor.UpperCenter).PutTo<Txt>((IUiElement) shipyardFullWarningHolder);
      Lyst<ProductQuantity> cargoCache = new Lyst<ProductQuantity>();
      updater.Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
      {
        this.Entity.PeekAllCargo(cargoCache);
        return (IIndexable<ProductQuantity>) cargoCache;
      }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Observe<Quantity>((Func<Quantity>) (() => this.Entity.TotalStoredQuantity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Prototype.CargoCapacity)).Do((Action<Lyst<ProductQuantity>, Quantity, Quantity>) ((cargo, totalCargo, totalCapacity) =>
      {
        this.m_cargoView.SetProducts((IIndexable<ProductQuantity>) cargo);
        itemContainer.SetItemVisibility((IUiElement) shipyardFullWarningHolder, totalCargo > totalCapacity);
      }));
      this.m_overlay = this.AddOverlay(new Action(this.hideDialogsOverlay));
      this.m_discardConfirmDialog = new ShipyardWindowView.ProductDiscardConfirmDialog(this.Builder, (IUiElement) this.m_overlay, new Action<ProductProto>(this.discardProduct));
      this.m_discardCannotDialog = new ShipyardWindowView.ProductCannotDiscardDialog(this.Builder, (IUiElement) this.m_overlay);
      Txt fuelBufferTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.FuelForShip__Title, new LocStrFormatted?((LocStrFormatted) Tr.FuelForShip__Tooltip));
      BufferViewTwoSliders fuelBuffer = this.Builder.NewBufferWithTwoSliders((IUiElement) itemContainer, (Action<float>) (x => this.m_controller.ScheduleInputCmd<ShipayardSetFuelSliderStepCmd>(new ShipayardSetFuelSliderStepCmd(this.Entity.Id, new int?((int) x), new int?()))), (Action<float>) (x => this.m_controller.ScheduleInputCmd<ShipayardSetFuelSliderStepCmd>(new ShipayardSetFuelSliderStepCmd(this.Entity.Id, new int?(), new int?((int) x)))), TrCore.StoredProduct__KeepFull.TranslatedString, TrCore.StoredProduct__KeepEmpty.TranslatedString, 10).AppendTo<BufferViewTwoSliders>(itemContainer, new float?(this.Style.BufferView.HeightWithSlider));
      Btn makePrimaryBtn = this.Builder.NewBtnPrimary("Btn").SetText((LocStrFormatted) Tr.ShipyardMakePrimary).OnClick((Action) (() => this.m_controller.Context.InputScheduler.ScheduleInputCmd<ShipyardMakePrimaryCmd>(new ShipyardMakePrimaryCmd(this.Entity.Id))));
      makePrimaryBtn.AppendTo<Btn>(itemContainer, new Vector2?(makePrimaryBtn.GetOptimalSize()), ContainerPosition.MiddleOrCenter, Offset.Top(10f));
      Tooltip makePrimaryTooltip = makePrimaryBtn.AddToolTipAndReturn();
      updater.Observe<ProductProto>((Func<ProductProto>) (() => this.Entity.FuelBuffer.Product)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FuelBuffer.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.FuelBuffer.Quantity)).Do(new Action<ProductProto, Quantity, Quantity>(((BufferView) fuelBuffer).UpdateState));
      updater.Observe<int>((Func<int>) (() => this.Entity.FuelBuffer.ImportUntilPercent.DivAsFix32(LogisticsBuffer.SingleStep).ToIntRounded())).Observe<int>((Func<int>) (() => this.Entity.FuelBuffer.ExportFromPercent.DivAsFix32(LogisticsBuffer.SingleStep).ToIntRounded())).Do(new Action<int, int>(fuelBuffer.UpdateSliders));
      updater.Observe<bool>((Func<bool>) (() => this.Entity.DockedFleet.HasValue)).Observe<bool>((Func<bool>) (() => this.Entity.CanBePrimary())).Observe<bool>((Func<bool>) (() => this.Entity.Prototype.CanRepair)).Do((Action<bool, bool, bool>) ((hasShip, canBePrimary, canRepair) =>
      {
        itemContainer.StartBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) fuelBufferTitle, hasShip & canRepair);
        itemContainer.SetItemVisibility((IUiElement) fuelBuffer, hasShip & canRepair);
        itemContainer.SetItemVisibility((IUiElement) makePrimaryBtn, canBePrimary);
        itemContainer.FinishBatchOperation();
      }));
      updater.Observe<bool>((Func<bool>) (() => this.m_fleetManager.HasFleet && this.m_fleetManager.TravelingFleet.PendingDockAssignment == this.Entity)).Do((Action<bool>) (isPendingAssignment =>
      {
        makePrimaryBtn.SetEnabled(!isPendingAssignment);
        makePrimaryTooltip.SetText((LocStrFormatted) (isPendingAssignment ? Tr.ShipyardMakePrimary__TooltipInProgress : Tr.ShipyardMakePrimary__Tooltip));
      }));
      this.UpgradeView.ValueOrNull?.ShowTooltipIfNotAvailable((LocStrFormatted) Tr.ResearchToRepair__Tooltip);
      Txt repairsTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ShipLoading__Title, new LocStrFormatted?((LocStrFormatted) Tr.ShipLoading__Desc));
      ScrollableStackContainer repairsContainer = new ScrollableStackContainer(this.Builder, 300f).AppendTo<ScrollableStackContainer>(this.ItemsContainer);
      updater.Observe<IWorldMapRepairableEntity>((Func<IEnumerable<IWorldMapRepairableEntity>>) (() => this.m_worldMapManager.EntitiesUnderConstruction.Where<IWorldMapRepairableEntity>((Func<IWorldMapRepairableEntity, bool>) (x => x.NeedsProductsForConstruction))), (ICollectionComparator<IWorldMapRepairableEntity, IEnumerable<IWorldMapRepairableEntity>>) CompareFixedOrder<IWorldMapRepairableEntity>.Instance).Observe<bool>((Func<bool>) (() => this.Entity.DockedFleet.HasValue)).Do((Action<Lyst<IWorldMapRepairableEntity>, bool>) ((entitiesBeingRepaired, hasShip) =>
      {
        this.ItemsContainer.SetItemVisibility((IUiElement) repairsTitle, hasShip && entitiesBeingRepaired.IsNotEmpty);
        this.ItemsContainer.SetItemVisibility((IUiElement) repairsContainer, hasShip);
        repairsContainer.ItemsContainer.StartBatchOperation();
        repairsContainer.ItemsContainer.ClearAll();
        this.m_entitiesRepairPanels.ReturnAll();
        foreach (IWorldMapRepairableEntity entity in entitiesBeingRepaired)
        {
          ShipyardWindowView.WorldMapEntityConstructionPanel view = this.m_entitiesRepairPanels.GetView();
          view.SetEntity(entity);
          repairsContainer.ItemsContainer.Append((IUiElement) view, new float?(view.GetHeight()));
        }
        repairsContainer.ItemsContainer.FinishBatchOperation();
      }));
      this.AddUpdater(updater.Build());
      CustomPriorityPanel customPriorityPanel1 = CustomPriorityPanel.NewForShipFuelImport((IUiElement) fuelBuffer, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel1.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) fuelBuffer, customPriorityPanel1.GetSize(), Offset.Right((float) (-(double) customPriorityPanel1.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel1.Updater);
      CustomPriorityPanel customPriorityPanel2 = CustomPriorityPanel.NewForShipFuelExport((IUiElement) fuelBuffer, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel2.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) fuelBuffer, customPriorityPanel2.GetSize(), Offset.Right((float) (-(double) customPriorityPanel2.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel2.Updater);
      CustomPriorityPanel customPriorityPanel3 = CustomPriorityPanel.NewForShipyardStoredCargo((IUiElement) this.m_cargoView, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel3.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) this.m_cargoView, customPriorityPanel3.GetSize(), Offset.Right((float) (-(double) customPriorityPanel3.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel3.Updater);
      CustomPriorityPanel customPriorityPanel4 = CustomPriorityPanel.NewForShipyardWorldCargoImport((IUiElement) repairsTitle, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel4.PutToRightTopOf<CustomPriorityPanel>((IUiElement) repairsTitle, customPriorityPanel4.GetSize(), Offset.Right((float) (-(double) customPriorityPanel4.GetWidth() + 1.0)) + Offset.Top(repairsTitle.GetHeight()));
      this.AddUpdater(customPriorityPanel4.Updater);
      this.OnShowStart += new Action(this.hideDialogsOverlay);
    }

    protected override void AddCustomItemsEnd(StackContainer itemContainer)
    {
      base.AddCustomItemsEnd(itemContainer);
      this.UpgradeView.ValueOrNull?.SetActionTitle((LocStrFormatted) Tr.Repair);
    }

    private void onDiscardProductClick(ProductProto product)
    {
      if (Mafi.Core.Buildings.Shipyard.Shipyard.CanDiscardProduct(product))
        this.m_discardConfirmDialog.SetProductAndShow(product);
      else
        this.m_discardCannotDialog.ShowInOverlay();
    }

    private void discardProduct(ProductProto product)
    {
      this.m_controller.Context.InputScheduler.ScheduleInputCmd<ShipyardDiscardProductCmd>(new ShipyardDiscardProductCmd(this.Entity.Id, product.Id));
    }

    private void hideDialogsOverlay()
    {
      this.m_discardConfirmDialog.HideFromCustomOverlay((IUiElement) this.m_overlay);
      this.m_discardCannotDialog.HideFromCustomOverlay((IUiElement) this.m_overlay);
    }

    private class WorldMapEntityConstructionPanel : 
      IUiElementWithUpdater,
      IUiElement,
      IDynamicSizeElement
    {
      private readonly IInputScheduler m_inputScheduler;
      private readonly Func<Mafi.Core.Buildings.Shipyard.Shipyard> m_shipyardProvider;
      private readonly Panel m_container;
      private readonly Txt m_entityName;
      private readonly StackContainer m_stackContainer;

      public IUiUpdater Updater { get; }

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      private IWorldMapRepairableEntity Entity { get; set; }

      public event Action<IUiElement> SizeChanged;

      public WorldMapEntityConstructionPanel(
        UiBuilder builder,
        IInputScheduler inputScheduler,
        Func<Mafi.Core.Buildings.Shipyard.Shipyard> shipyardProvider)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_inputScheduler = inputScheduler;
        this.m_shipyardProvider = shipyardProvider;
        this.m_container = builder.NewPanel("ConstructEntityPanel").SetBackground(builder.Style.Panel.ItemOverlay);
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        this.m_stackContainer = builder.NewStackContainer("Stack").SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetStackingDirection(StackContainer.Direction.TopToBottom).SetInnerPadding(Offset.Bottom(10f)).PutToTopOf<StackContainer>((IUiElement) this.m_container, 0.0f);
        this.m_entityName = builder.NewTxt("title").SetTextStyle(builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_stackContainer, new float?(20f));
        Txt infoTxt = builder.NewTxt("info").SetTextStyle(builder.Style.Global.Text).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_stackContainer, new float?(30f));
        ProductQuantitiesView requiredProductsView = new ProductQuantitiesView((IUiElement) this.m_stackContainer, builder);
        requiredProductsView.AppendTo<ProductQuantitiesView>(this.m_stackContainer, new float?(0.0f));
        Panel buttonsContainer = builder.NewPanel("Btns");
        Btn objectToPlace = builder.NewBtnPrimary("LoadCargoBtn").SetText((LocStrFormatted) Tr.ShipLoading__Action).OnClick(new Action(this.loadCargoToggleClicked));
        objectToPlace.PutToLeftOf<Btn>((IUiElement) buttonsContainer, objectToPlace.GetOptimalWidth());
        Btn element = builder.NewBtnDanger("Cancel").SetIcon("Assets/Unity/UserInterface/General/Cancel.svg").OnClick((Action) (() => this.m_inputScheduler.ScheduleInputCmd<WorldMapEntityCancelRepairCmd>(new WorldMapEntityCancelRepairCmd(this.Entity.Id))));
        element.SetWidth<Btn>(28f);
        element.AddToolTip(Tr.ShipLoading__CancelProject).PutToRightOf<Btn>((IUiElement) buttonsContainer, element.GetWidth());
        buttonsContainer.AppendTo<Panel>(this.m_stackContainer, new Vector2?(new Vector2((float) ((double) objectToPlace.GetOptimalWidth() + (double) element.GetWidth() + 10.0), 28f)), ContainerPosition.MiddleOrCenter);
        Btn cancelLoadingBtn = builder.NewBtnDanger("CancelLoadBtn").SetText((LocStrFormatted) Tr.Cancel).OnClick(new Action(this.loadCargoToggleClicked));
        cancelLoadingBtn.AppendTo<Btn>(this.m_stackContainer, new Vector2?(cancelLoadingBtn.GetOptimalSize()), ContainerPosition.MiddleOrCenter);
        Lyst<ProductQuantity> tempProducts = new Lyst<ProductQuantity>();
        updaterBuilder.Observe<ProductQuantity>((Func<IIndexable<ProductQuantity>>) (() =>
        {
          tempProducts.Clear();
          this.m_shipyardProvider().ProductsNeededForWorldEntityConstruction(tempProducts);
          return (IIndexable<ProductQuantity>) tempProducts;
        }), (ICollectionComparator<ProductQuantity, IIndexable<ProductQuantity>>) CompareFixedOrder<ProductQuantity>.Instance).Observe<bool>((Func<bool>) (() => this.m_shipyardProvider().WorldEntityToConstruct.ValueOrNull == this.Entity)).Do((Action<Lyst<ProductQuantity>, bool>) ((productsNeeded, isUnderConstruction) =>
        {
          this.m_stackContainer.SetItemVisibility((IUiElement) buttonsContainer, !isUnderConstruction);
          this.m_stackContainer.SetItemVisibility((IUiElement) cancelLoadingBtn, isUnderConstruction);
          this.m_stackContainer.SetItemVisibility((IUiElement) requiredProductsView, isUnderConstruction && productsNeeded.IsNotEmpty);
          if (productsNeeded.IsNotEmpty)
            requiredProductsView.SetProducts((IIndexable<ProductQuantity>) productsNeeded);
          if (!isUnderConstruction)
            infoTxt.SetText((LocStrFormatted) Tr.ShipLoading__NotStarted);
          else if (productsNeeded.IsEmpty)
            infoTxt.SetText((LocStrFormatted) Tr.ShipLoading__Done);
          else
            infoTxt.SetText((LocStrFormatted) Tr.ShipLoading__InProgress);
          this.m_container.SetSize<Panel>(new Vector2(this.m_container.GetWidth(), this.m_stackContainer.GetDynamicHeight()));
          Action<IUiElement> sizeChanged = this.SizeChanged;
          if (sizeChanged == null)
            return;
          sizeChanged((IUiElement) this);
        }));
        this.Updater = updaterBuilder.Build();
      }

      public float GetHeight() => this.m_stackContainer.GetDynamicHeight();

      private void loadCargoToggleClicked()
      {
        this.m_inputScheduler.ScheduleInputCmd<ShipyardWorldEntityConstructionToggle>(new ShipyardWorldEntityConstructionToggle(this.m_shipyardProvider().Id, this.Entity.Id));
      }

      public void SetEntity(IWorldMapRepairableEntity entity)
      {
        this.Entity = entity;
        this.m_entityName.SetText((LocStrFormatted) entity.Prototype.Strings.Name);
      }
    }

    internal class ProductDiscardConfirmDialog : DialogView
    {
      private readonly IUiElement m_parentOverlay;
      private Option<ProductProto> m_product;

      public ProductDiscardConfirmDialog(
        UiBuilder builder,
        IUiElement parentOverlay,
        Action<ProductProto> onConfirmClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder);
        ShipyardWindowView.ProductDiscardConfirmDialog discardConfirmDialog = this;
        this.m_parentOverlay = parentOverlay;
        this.AppendBtnDanger((LocStrFormatted) Tr.DiscardAllProducts__Action).OnClick((Action) (() =>
        {
          discardConfirmDialog.HideFromCustomOverlay(discardConfirmDialog.m_parentOverlay);
          if (!discardConfirmDialog.m_product.HasValue)
            return;
          onConfirmClick(discardConfirmDialog.m_product.Value);
        }));
        this.AppendBtnGeneral((LocStrFormatted) Tr.Cancel).OnClick((Action) (() => discardConfirmDialog.HideFromCustomOverlay(discardConfirmDialog.m_parentOverlay)));
        this.HighlightAsDanger();
        this.Width = 350f;
      }

      public void SetProductAndShow(ProductProto product)
      {
        this.m_product = (Option<ProductProto>) product;
        this.SetMessage(Tr.DiscardAllProducts__Confirmation.Format(product.Strings.Name));
        this.ShowInCustomOverlay(this.m_parentOverlay);
      }
    }

    internal class ProductCannotDiscardDialog : DialogView
    {
      private readonly IUiElement m_parentOverlay;

      public ProductCannotDiscardDialog(UiBuilder builder, IUiElement parentOverlay)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder);
        this.m_parentOverlay = parentOverlay;
        this.AppendBtnGeneral((LocStrFormatted) Tr.Dismiss).OnClick((Action) (() => this.HideFromCustomOverlay(this.m_parentOverlay)));
        this.HighlightAsGeneral();
        this.Width = 350f;
        this.SetMessage((LocStrFormatted) Tr.DiscardAllProducts__NotSupported);
      }

      public void ShowInOverlay() => this.ShowInCustomOverlay(this.m_parentOverlay);
    }
  }
}
