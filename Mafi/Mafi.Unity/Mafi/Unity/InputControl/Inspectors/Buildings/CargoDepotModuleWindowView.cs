// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.CargoDepotModuleWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Core.World.Contracts;
using Mafi.Localization;
using Mafi.Unity.InputControl.World;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  internal class CargoDepotModuleWindowView : StaticEntityInspectorBase<CargoDepotModule>
  {
    private readonly ContractsManager m_contractsManager;
    private readonly SourceProductsAnalyzer m_sourceProductsAnalyzer;
    private readonly WorldMapManager m_worldMapManager;
    private readonly CargoDepotModuleInspector m_controller;
    private readonly ProtoPicker<ProductProto> m_protoPicker;
    private readonly ContractPicker m_contractPicker;
    private Panel m_windowOverlay;
    private Option<ContractProto> m_lastSeenContract;

    protected override CargoDepotModule Entity => this.m_controller.SelectedEntity;

    public CargoDepotModuleWindowView(
      CargoDepotModuleInspector controller,
      ContractsManager contractsManager,
      SourceProductsAnalyzer sourceProductsAnalyzer,
      TradeWindowController tradeWindowController,
      WorldMapManager worldMapManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_contractsManager = contractsManager;
      this.m_sourceProductsAnalyzer = sourceProductsAnalyzer;
      this.m_worldMapManager = worldMapManager;
      this.m_controller = controller.CheckNotNull<CargoDepotModuleInspector>();
      this.m_protoPicker = new ProtoPicker<ProductProto>(new Action<ProductProto>(this.setProductToStore));
      this.m_contractPicker = new ContractPicker(controller.Context, contractsManager, sourceProductsAnalyzer, tradeWindowController, (Func<CargoDepotModule>) (() => this.Entity), new Action<ContractProto>(this.onContractClicked));
    }

    private void onContractClicked(ContractProto contract)
    {
      this.m_controller.ScheduleInputCmd<CargoDepotAssignContractCmd>(new CargoDepotAssignContractCmd(this.Entity.Depot.Value, contract));
      this.m_contractPicker.Hide();
      this.m_windowOverlay.Hide<Panel>();
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      base.AddCustomItems(itemContainer);
      this.m_contractPicker.SetOnCloseButtonClickAction((Action) (() =>
      {
        this.m_contractPicker.Hide();
        this.m_windowOverlay.Hide<Panel>();
      }));
      this.m_contractPicker.BuildUi(this.Builder);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      this.AddClearButton(new Action(((EntityInspector<CargoDepotModule, CargoDepotModuleWindowView>) this.m_controller).Clear));
      StatusPanel statusInfo = this.AddStatusInfoPanel();
      updaterBuilder.Observe<CargoDepotModule.State>((Func<CargoDepotModule.State>) (() => this.Entity.CurrentState)).Do((Action<CargoDepotModule.State>) (state =>
      {
        switch (state)
        {
          case CargoDepotModule.State.Paused:
            statusInfo.SetStatus(Tr.EntityStatus__Paused, StatusPanel.State.Warning);
            break;
          case CargoDepotModule.State.Broken:
            statusInfo.SetStatus(TrCore.EntityStatus__Broken, StatusPanel.State.Critical);
            break;
          case CargoDepotModule.State.Working:
            statusInfo.SetStatus(Tr.EntityStatus__Working);
            break;
          case CargoDepotModule.State.MissingWorkers:
            statusInfo.SetStatus(Tr.EntityStatus__NoWorkers, StatusPanel.State.Critical);
            break;
          case CargoDepotModule.State.NotEnoughPower:
            statusInfo.SetStatus(TrCore.EntityStatus__LowPower, StatusPanel.State.Critical);
            break;
          case CargoDepotModule.State.Idle:
            statusInfo.SetStatus(Tr.EntityStatus__Idle, StatusPanel.State.Warning);
            break;
        }
      }));
      this.AddStorageLogisticsPanel(updaterBuilder, (Func<IEntityWithSimpleLogisticsControl>) (() => (IEntityWithSimpleLogisticsControl) this.Entity), this.m_controller.Context.InputScheduler);
      Txt currentContractTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.ContractAssigned__Title, new LocStrFormatted?((LocStrFormatted) Tr.ContractAssigned__Tooltip));
      ContractView contractView = new ContractView((IUiElement) itemContainer, this.Builder, this.m_controller.Context, this.m_contractsManager, this.m_sourceProductsAnalyzer);
      contractView.SetupForDepotInspector(new Action(onContractUnAssignClick), (Func<CargoDepot>) (() => this.Entity.Depot.Value));
      this.AddUpdater(contractView.Updater);
      itemContainer.Append((IUiElement) contractView, new float?(contractView.GetHeight()));
      updaterBuilder.Observe<Option<ContractProto>>((Func<Option<ContractProto>>) (() =>
      {
        CargoDepot valueOrNull = this.Entity.Depot.ValueOrNull;
        return valueOrNull == null ? (Option<ContractProto>) Option.None : valueOrNull.ContractAssigned;
      })).Do((Action<Option<ContractProto>>) (contractMaybe =>
      {
        if (contractMaybe.HasValue)
          contractView.SetContract(contractMaybe.Value);
        itemContainer.SetItemVisibility((IUiElement) currentContractTitle, contractMaybe.HasValue);
        itemContainer.SetItemVisibility((IUiElement) contractView, contractMaybe.HasValue);
      }));
      Txt bufferTitle = this.AddSectionTitle(itemContainer, LocStrFormatted.Empty);
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.IsForImport())).Do((Action<bool>) (isForImport => bufferTitle.SetText((LocStrFormatted) (isForImport ? Tr.CargoDepotProduct__ImportTitle : Tr.CargoDepotProduct__ExportTitle))));
      BufferView buffer = this.Builder.NewBufferView((IUiElement) itemContainer, (Action) (() => this.m_controller.InputScheduler.ScheduleInputCmd<CargoDepotModuleClearProductCmd>(new CargoDepotModuleClearProductCmd(this.Entity))), new Action(this.plusBtnClicked)).AppendTo<BufferView>(itemContainer, new float?(this.Style.BufferView.Height));
      this.SetupProductPickerWithBuffer(this.m_protoPicker, buffer);
      Panel assignContractPanel = this.AddOverlayPanel(this.ItemsContainer, offset: Offset.Zero);
      Btn objectToPlace1 = this.Builder.NewBtnGeneral("contracts").SetText((LocStrFormatted) Tr.CargoDepotWizard__AssignContract).OnClick(new Action(this.showContractPicker));
      objectToPlace1.PutToRightBottomOf<Btn>((IUiElement) assignContractPanel, objectToPlace1.GetOptimalSize(), Offset.Right(66f) + Offset.Bottom(16f));
      LocStrFormatted trashError = LocStrFormatted.Empty;
      Tooltip trashBtnToolTip = this.Builder.AddTooltipFor<Btn>((IUiElementWithHover<Btn>) buffer.TrashBtn.Value);
      updaterBuilder.Observe<bool>((Func<bool>) (() => this.Entity.CanRemoveAssignedProduct(out trashError))).Do((Action<bool>) (canRemove =>
      {
        trashBtnToolTip.SetText(trashError);
        buffer.TrashBtn.Value.SetEnabled(canRemove);
      }));
      bool importRequested = false;
      Txt wizardTitle = this.AddSectionTitle(itemContainer, (LocStrFormatted) Tr.CargoDepotWizard__Title, new LocStrFormatted?((LocStrFormatted) Tr.CargoDepotWizard__Tooltip));
      Panel modeWizardContainer = this.AddOverlayPanel(itemContainer, 50);
      Btn btn = this.Builder.NewBtnPrimary("import").SetText((LocStrFormatted) Tr.CargoDepotWizard__ImportProducts).OnClick((Action) (() => importRequested = true));
      btn.PutToLeftMiddleOf<Btn>((IUiElement) modeWizardContainer, btn.GetOptimalSize(), Offset.Left(10f));
      Btn objectToPlace2 = this.Builder.NewBtnPrimary("contracts").SetText((LocStrFormatted) Tr.CargoDepotWizard__AssignContract).OnClick(new Action(this.showContractPicker));
      objectToPlace2.PutToLeftMiddleOf<Btn>((IUiElement) modeWizardContainer, objectToPlace2.GetOptimalSize(), Offset.Left(20f + btn.GetWidth()));
      updaterBuilder.Observe<Option<ContractProto>>((Func<Option<ContractProto>>) (() =>
      {
        CargoDepot valueOrNull = this.Entity.Depot.ValueOrNull;
        return valueOrNull == null ? (Option<ContractProto>) Option.None : valueOrNull.ContractAssigned;
      })).Observe<bool>((Func<bool>) (() => this.m_contractsManager.IsAnyContractUnlocked())).Observe<bool>((Func<bool>) (() =>
      {
        foreach (Option<CargoDepotModule> module in this.Entity.Depot.Value.Modules)
        {
          if (module.HasValue && module.Value.StoredProduct.HasValue)
            return true;
        }
        return false;
      })).Observe<bool>((Func<bool>) (() => importRequested)).Do((Action<Option<ContractProto>, bool, bool, bool>) ((contract, anyContractUnlocked, hasDepotAnyProductAssigned, importReq) =>
      {
        this.m_lastSeenContract = contract;
        bool hasValue = contract.HasValue;
        bool isVisible1 = anyContractUnlocked && !hasValue && !hasDepotAnyProductAssigned && !importReq;
        bool isVisible2 = anyContractUnlocked && !isVisible1 && contract.IsNone;
        itemContainer.StartBatchOperation();
        itemContainer.SetItemVisibility((IUiElement) buffer, !isVisible1);
        itemContainer.SetItemVisibility((IUiElement) bufferTitle, !isVisible1);
        itemContainer.SetItemVisibility((IUiElement) assignContractPanel, isVisible2);
        itemContainer.SetItemVisibility((IUiElement) wizardTitle, isVisible1);
        itemContainer.SetItemVisibility((IUiElement) modeWizardContainer, isVisible1);
        itemContainer.FinishBatchOperation();
      }));
      Panel parent = this.AddOverlayPanel(itemContainer, offset: Offset.Top(10f));
      TextWithIcon throughput = new TextWithIcon(this.Builder).SetTextStyle(this.Builder.Style.Global.TextControlsBold).SetIcon("Assets/Unity/UserInterface/General/Clock.svg").PutToLeftOf<TextWithIcon>((IUiElement) parent, 0.0f, Offset.Left(15f));
      updaterBuilder.Observe<CargoDepotModuleProto>((Func<CargoDepotModuleProto>) (() => this.Entity.Prototype)).Do((Action<CargoDepotModuleProto>) (proto =>
      {
        Duration duration = 60.Seconds();
        Fix32 fix32 = duration.Ticks.ToFix32() / proto.DurationPerExchange.Ticks * proto.QuantityPerExchange.Value;
        throughput.SetPrefixText(Tr.ThroughputWithParam.Format(string.Format("{0} / {1}", (object) fix32.ToStringRounded(1), (object) duration.Seconds.IntegerPart)).Value.ToUpper(LocalizationManager.CurrentCultureInfo));
      }));
      CustomPriorityPanel customPriorityPanel1 = CustomPriorityPanel.NewForStorageImport((IUiElement) buffer, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel1.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) buffer, customPriorityPanel1.GetSize(), Offset.Right((float) (-(double) customPriorityPanel1.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel1.Updater);
      CustomPriorityPanel customPriorityPanel2 = CustomPriorityPanel.NewForStorageExport((IUiElement) buffer, this.m_controller.InputScheduler, this.Builder, (Func<IEntityWithCustomPriority>) (() => (IEntityWithCustomPriority) this.Entity));
      customPriorityPanel2.PutToRightMiddleOf<CustomPriorityPanel>((IUiElement) buffer, customPriorityPanel2.GetSize(), Offset.Right((float) (-(double) customPriorityPanel2.GetWidth() + 1.0)));
      this.AddUpdater(customPriorityPanel2.Updater);
      updaterBuilder.Observe<Option<ProductProto>>((Func<Option<ProductProto>>) (() => this.Entity.StoredProduct)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.Capacity)).Observe<Quantity>((Func<Quantity>) (() => this.Entity.CurrentQuantity)).Do(new Action<Option<ProductProto>, Quantity, Quantity>(buffer.UpdateState));
      this.AddUpdater(updaterBuilder.Build());
      this.m_windowOverlay = this.Builder.NewPanel("Overlay").SetBackground(new ColorRgba(4408131, 110)).PutTo<Panel>((IUiElement) this.GetContentPanel()).Hide<Panel>();
      this.OnHide += (Action) (() =>
      {
        this.m_protoPicker.Hide();
        this.m_windowOverlay.Hide<Panel>();
        this.m_contractPicker.Hide();
        importRequested = false;
      });
      this.SetWidth(450f);

      void onContractUnAssignClick()
      {
        this.m_controller.InputScheduler.ScheduleInputCmd<CargoDepotAssignContractCmd>(new CargoDepotAssignContractCmd(this.Entity.Depot.Value, (ContractProto) null));
      }
    }

    private void plusBtnClicked()
    {
      if (this.m_protoPicker.IsVisible)
      {
        this.m_protoPicker.Hide();
      }
      else
      {
        if (this.m_lastSeenContract.HasValue)
          this.m_protoPicker.SetVisibleProtos(this.m_lastSeenContract.Value.AllProducts.Where((Func<ProductProto, bool>) (x => x.Type == this.m_controller.SelectedEntity.Prototype.ProductType)));
        else
          this.m_protoPicker.SetVisibleProtos(this.m_worldMapManager.AllMinableProducts.Where<ProductProto>((Func<ProductProto, bool>) (x => x.Type == this.m_controller.SelectedEntity.Prototype.ProductType)));
        this.m_protoPicker.Show();
      }
    }

    private void setProductToStore(IProtoWithIconAndName product)
    {
      if (!(product is ProductProto product1))
      {
        Log.Error(string.Format("Trying to store UI product which isn't a ProductProto '{0}'", (object) product));
      }
      else
      {
        this.m_controller.ScheduleInputCmd<CargoDepotModuleSetProductCmd>(new CargoDepotModuleSetProductCmd(this.m_controller.SelectedEntity, product1));
        this.m_protoPicker.Hide();
      }
    }

    private void showContractPicker()
    {
      this.m_contractPicker.PutToCenterMiddleOf<ContractPicker>((IUiElement) this.m_windowOverlay, this.m_contractPicker.GetSize());
      this.m_contractPicker.Show();
      this.m_windowOverlay.Show<Panel>();
      this.SetContentSize(450f, this.ItemsContainer.GetDynamicHeight().Max(this.m_contractPicker.GetSize().y + 20f));
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_contractPicker.RenderUpdate(gameTime);
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      this.m_contractPicker.SyncUpdate(gameTime);
    }
  }
}
