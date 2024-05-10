// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Modules.CargoDepotModule
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.World;
using Mafi.Core.World.Contracts;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Modules
{
  [GenerateSerializer(false, null, 0)]
  public class CargoDepotModule : 
    StorageBase,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IInputBufferPriorityProvider,
    IOutputBufferPriorityProvider,
    IEntityWithCustomPriority,
    IElectricityConsumingEntity,
    IMaintainedEntity,
    IEntityWithSimUpdate,
    IEntityWithCloneableConfig,
    IEntityWithPorts
  {
    private static readonly Duration DURATION_TO_LOWER_PIPE;
    private CargoDepotModuleProto m_proto;
    private int m_slot;
    private readonly IElectricityConsumer m_electricityConsumer;
    [DoNotSave(0, null)]
    private bool m_ignorePower;
    private readonly Event m_onProductStoredChanged;
    private readonly CargoDepotManager m_binder;
    private readonly WorldMapManager m_worldMapManager;
    private readonly ContractsManager m_contractsManager;
    private ProductQuantity m_pendingQuantityToShip;
    private ProductQuantity m_pendingQuantityFromShip;
    private ProductQuantity m_pendingQuantityToStorage;
    private readonly TickTimer m_craneAnimationTimer;
    public bool IsPipeDown;
    private readonly TickTimer m_pipeMovementTimer;
    private EntityNotificator m_noProductAssignedNotif;
    private EntityNotificator m_contractNotMatchingNotif;
    private bool m_lacksPower;
    private readonly IProductsManager m_productsManager;
    [DoNotSave(0, null)]
    private bool m_contractNotMatching;
    [DoNotSave(0, null)]
    private Option<ContractProto> m_lastContractChecked;
    [DoNotSave(0, null)]
    private bool m_canWorkOnLowPower;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override LogisticsControl LogisticsInputControl
    {
      get
      {
        return !this.StoredProduct.HasValue || this.IsForImport() ? LogisticsControl.NotAvailable : LogisticsControl.Enabled;
      }
    }

    public override LogisticsControl LogisticsOutputControl
    {
      get
      {
        return !this.StoredProduct.HasValue || !this.IsForImport() ? LogisticsControl.NotAvailable : LogisticsControl.Enabled;
      }
    }

    [DoNotSave(0, null)]
    public CargoDepotModuleProto Prototype
    {
      get => this.m_proto;
      private set
      {
        this.m_proto = value;
        this.Prototype = (StorageBaseProto) value;
      }
    }

    public override bool CanBePaused => true;

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public bool IsIdleForMaintenance => false;

    Electricity IElectricityConsumingEntity.PowerRequired
    {
      get => !this.m_ignorePower ? this.Prototype.ConsumedPowerForCranePerTick : Electricity.Zero;
    }

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    protected override bool IoDisabled => base.IoDisabled || !this.IsOperational;

    protected override bool IsOperational
    {
      get => base.IsOperational && ((IEntityWithWorkers) this).HasWorkersCached;
    }

    public CargoDepotModule.State CurrentState { get; private set; }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IUpgrader Upgrader { get; private set; }

    public Option<CargoDepot> Depot { get; private set; }

    public Quantity UsableCapacity
    {
      get => !this.Buffer.HasValue ? Quantity.Zero : this.Buffer.Value.UsableCapacity;
    }

    public IEvent OnProductStoredChanged => (IEvent) this.m_onProductStoredChanged;

    public int ImportPriority { get; private set; }

    public int ExportPriority { get; private set; }

    protected override bool ReportFullStorageCapacityInStats => false;

    public bool IsAnimatingImport
    {
      get
      {
        return this.m_pendingQuantityFromShip.IsNotEmpty || this.m_pendingQuantityToStorage.IsNotEmpty;
      }
    }

    public Percent CargoAnimationProgress => this.m_craneAnimationTimer.PercentFinished;

    public bool IsCargoTransferAnimating
    {
      get
      {
        return this.m_craneAnimationTimer.IsNotFinished && this.CurrentState == CargoDepotModule.State.Working;
      }
    }

    public bool IsPipeMoving
    {
      get => this.Prototype.HasPipeCraneAnimation && this.m_pipeMovementTimer.IsNotFinished;
    }

    public Percent PipeMovementProgress => this.m_pipeMovementTimer.PercentFinished;

    public CargoDepotModule(
      EntityId id,
      CargoDepotModuleProto cargoDepotProto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      ILayoutEntityUpgraderFactory upgraderFactory,
      ContractsManager contractsManager,
      CargoDepotManager binder,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      WorldMapManager worldMapManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_slot = -1;
      this.m_onProductStoredChanged = new Event();
      this.m_pendingQuantityToShip = ProductQuantity.None;
      this.m_pendingQuantityFromShip = ProductQuantity.None;
      this.m_pendingQuantityToStorage = ProductQuantity.None;
      this.m_craneAnimationTimer = new TickTimer();
      this.m_pipeMovementTimer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StorageBaseProto) cargoDepotProto, transform, context, simLoopEvents, vehicleBuffersRegistry);
      this.m_proto = cargoDepotProto;
      this.m_productsManager = this.Context.ProductsManager;
      this.m_contractsManager = contractsManager;
      this.m_binder = binder;
      this.m_worldMapManager = worldMapManager;
      this.updateProperties();
      this.Upgrader = upgraderFactory.CreateInstance<CargoDepotModuleProto, CargoDepotModule>(this, cargoDepotProto);
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_noProductAssignedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CargoDepotModuleNoProductAssigned);
      this.m_contractNotMatchingNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CargoDepotModuleContractNotMatching);
      this.ImportPriority = 14;
      this.ExportPriority = 14;
      this.attachToCargoDepot(worldMapManager);
      this.SetLogisticsInputDisabled(true);
      this.SetLogisticsOutputDisabled(true);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf() => this.updateProperties();

    private void updateProperties()
    {
      this.m_canWorkOnLowPower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.LogisticsCanWorkOnLowPower);
      if (this.m_canWorkOnLowPower)
        ((IEntityWithGeneralPriorityFriend) this).SetGeneralPriorityInternal(0);
      this.m_ignorePower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.LogisticsIgnorePower);
      this.m_electricityConsumer?.OnPowerRequiredChanged();
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    protected override LogisticsBuffer CreateNewBuffer(Quantity capacity, ProductProto product)
    {
      bool flag = (Proto) this.Depot.Value.ContractAssigned.ValueOrNull?.ProductToBuy == (Proto) product;
      return (LogisticsBuffer) new CargoDepotModule.CargoDepotBuffer(this.Prototype.Capacity, product, this.Context.ProductsManager, 15, (IEntity) this, !flag);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      bool isOperational = this.IsOperational;
      this.CurrentState = this.updateState();
      this.updateContractNotification();
      this.m_contractNotMatchingNotif.NotifyIff(this.IsEnabled && this.m_contractNotMatching, (IEntity) this);
      this.m_noProductAssignedNotif.NotifyIff(!this.m_contractNotMatchingNotif.IsActive && this.IsEnabled && this.StoredProduct.IsNone, (IEntity) this);
      if (isOperational != this.IsOperational)
      {
        this.UpdateLogisticsInputReg();
        this.UpdateLogisticsOutputReg();
      }
      if (!this.IsEnabled)
        return;
      this.SendAllOutputsIfCan();
    }

    private void updateContractNotification()
    {
      if (this.m_lastContractChecked == this.Depot.Value.ContractAssigned)
        return;
      this.m_lastContractChecked = this.Depot.Value.ContractAssigned;
      this.m_contractNotMatching = false;
      if (!this.Depot.Value.ContractAssigned.HasValue)
        return;
      ContractProto contractProto = this.Depot.Value.ContractAssigned.Value;
      if (this.StoredProduct.HasValue)
        this.m_contractNotMatching = (Proto) contractProto.ProductToPayWith != (Proto) this.StoredProduct.Value && (Proto) contractProto.ProductToBuy != (Proto) this.StoredProduct.Value;
      else
        this.m_contractNotMatching = contractProto.ProductToPayWith.Type != this.Prototype.ProductType && contractProto.ProductToBuy.Type != this.Prototype.ProductType;
    }

    public override bool IsProductSupported(ProductProto product)
    {
      if (product.Type != this.Prototype.ProductType)
        return false;
      CargoDepot cargoDepot = this.Depot.Value;
      return cargoDepot.ContractAssigned.HasValue ? cargoDepot.ContractAssigned.Value.AllProducts.Contains(product) : this.m_worldMapManager.AllMinableProducts.Contains(product);
    }

    public bool IsForImport()
    {
      return !this.StoredProduct.HasValue || !(this.Depot.Value.ContractAssigned.ValueOrNull?.ProductToPayWith == this.StoredProduct);
    }

    private CargoDepotModule.State updateState()
    {
      if (this.IsNotEnabled)
        return !this.Maintenance.Status.IsBroken ? CargoDepotModule.State.Paused : CargoDepotModule.State.Broken;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return CargoDepotModule.State.MissingWorkers;
      if (this.m_lacksPower)
        return CargoDepotModule.State.NotEnoughPower;
      return !this.IsMovingCargo() ? CargoDepotModule.State.Idle : CargoDepotModule.State.Working;
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled || this.Buffer.IsNone)
        return pq.Quantity;
      LogisticsBuffer logisticsBuffer = this.Buffer.Value;
      return logisticsBuffer.CleaningMode || (Proto) logisticsBuffer.Product != (Proto) pq.Product ? pq.Quantity : this.Buffer.Value.StoreAsMuchAs(pq);
    }

    protected override void OnDestroy()
    {
      if (this.Depot.HasValue)
      {
        this.Depot.Value.RemoveModule(this);
        this.Depot = (Option<CargoDepot>) Option.None;
      }
      this.m_pendingQuantityToStorage = ProductQuantity.None;
      this.m_pendingQuantityFromShip = ProductQuantity.None;
      if (this.m_pendingQuantityToShip.Quantity.IsPositive)
      {
        this.m_productsManager.ProductDestroyed(this.m_pendingQuantityToShip, DestroyReason.Export);
        this.m_pendingQuantityToShip = ProductQuantity.None;
      }
      base.OnDestroy();
    }

    public bool AssignProduct(ProductProto product)
    {
      Option<CargoShipModule> shipModule = this.GetShipModule();
      if (shipModule.HasValue)
      {
        CargoShipModule cargoShipModule = shipModule.Value;
        if (cargoShipModule.Quantity.IsPositive && cargoShipModule.StoredProduct != product)
          return false;
      }
      bool flag = this.TryAssignProduct(product);
      if (flag)
      {
        this.m_lastContractChecked = Option<ContractProto>.None;
        this.m_onProductStoredChanged.Invoke();
      }
      return flag;
    }

    public void OnContractChangedOrCleared()
    {
      bool flag = this.IsForImport();
      if (flag && !this.IsLogisticsInputDisabled)
        this.SetLogisticsInputDisabled(true);
      if (!flag && !this.IsLogisticsOutputDisabled)
        this.SetLogisticsOutputDisabled(true);
      if (!this.Buffer.HasValue)
        return;
      ((CargoDepotModule.CargoDepotBuffer) this.Buffer.Value).SetIsInput(!flag);
    }

    /// <summary>For debugging purposes only.</summary>
    internal void Cheat_FullProduct()
    {
      if (!this.StoredProduct.HasValue)
        return;
      this.Context.ProductsManager.ProductCreated(this.StoredProduct.Value, Quantity.MaxValue - this.Buffer.Value.StoreAsMuchAs(Quantity.MaxValue), CreateReason.Cheated);
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
      {
        Log.Error("Upgrade not available!");
      }
      else
      {
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
        this.Depot.ValueOrNull?.NotifyModuleUpgraded(this.m_slot, this);
      }
    }

    public Option<CargoShipModule> GetShipModule()
    {
      return this.Depot.IsNone || this.Depot.Value.CargoShip.IsNone || this.m_slot < 0 || this.m_slot >= this.Depot.Value.CargoShip.Value.Modules.Count ? Option<CargoShipModule>.None : this.Depot.Value.CargoShip.Value.Modules[this.m_slot];
    }

    public void UpdateProductExchange()
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse();
      this.m_lacksPower = false;
      if (this.IsNotEnabled || !this.IsOperational)
        return;
      if ((this.m_pipeMovementTimer.IsNotFinished || this.m_craneAnimationTimer.IsNotFinished) && !this.m_electricityConsumer.TryConsume(this.m_canWorkOnLowPower) && !this.m_canWorkOnLowPower)
      {
        this.m_lacksPower = true;
      }
      else
      {
        if (this.m_pipeMovementTimer.IsNotFinished)
        {
          this.m_pipeMovementTimer.DecrementOnly();
          if (!this.m_pipeMovementTimer.IsFinished)
            return;
          this.IsPipeDown = !this.IsPipeDown;
        }
        if (this.m_craneAnimationTimer.Decrement())
        {
          if (this.m_pendingQuantityToShip.IsNotEmpty && this.m_craneAnimationTimer.PercentFinished >= this.Prototype.PercentOfAnimationToDropCargoToShip)
          {
            Option<CargoShipModule> shipModule = this.GetShipModule();
            if (shipModule.IsNone)
            {
              Log.Error("Ship module disappeared during unloading!");
              this.m_pendingQuantityToShip = ProductQuantity.None;
            }
            else
            {
              Quantity quantity1 = shipModule.Value.StoreAsMuchAs(this.m_pendingQuantityToShip);
              Quantity quantity2 = this.m_pendingQuantityToShip.Quantity - quantity1;
              Assert.That<Quantity>(quantity1).IsZero();
              this.m_productsManager.ProductDestroyed(this.m_pendingQuantityToShip.Product, quantity2, DestroyReason.Export);
              this.m_pendingQuantityToShip = ProductQuantity.None;
            }
          }
          else
          {
            if (!this.m_pendingQuantityFromShip.IsNotEmpty || !(this.m_craneAnimationTimer.PercentFinished >= this.Prototype.PercentOfAnimationToDropCargoToShip.InverseTo100()))
              return;
            Option<CargoShipModule> shipModule = this.GetShipModule();
            if (shipModule.IsNone)
            {
              Log.Error("Ship module disappeared during unloading!");
              this.m_pendingQuantityFromShip = ProductQuantity.None;
            }
            else
            {
              ((ICargoShipModuleFriend) shipModule.Value).RemoveExactly(this.m_pendingQuantityFromShip.Quantity);
              this.m_pendingQuantityToStorage = this.m_pendingQuantityFromShip;
              this.m_pendingQuantityFromShip = ProductQuantity.None;
            }
          }
        }
        else
        {
          if (this.m_pendingQuantityToStorage.IsNotEmpty)
          {
            this.Buffer.Value.StoreExactly(this.m_pendingQuantityToStorage.Quantity);
            this.m_productsManager.ProductCreated(this.m_pendingQuantityToStorage, CreateReason.Imported);
            this.m_pendingQuantityToStorage = ProductQuantity.None;
          }
          Assert.That<ProductQuantity>(this.m_pendingQuantityFromShip).IsEmpty();
          Assert.That<ProductQuantity>(this.m_pendingQuantityToShip).IsEmpty();
          Assert.That<ProductQuantity>(this.m_pendingQuantityToStorage).IsEmpty();
          if (this.IsForImport())
          {
            ProductQuantity availableForImport = this.getAvailableForImport();
            if (availableForImport.IsEmpty)
            {
              if (!this.Prototype.HasPipeCraneAnimation || !this.IsPipeDown)
                return;
              this.m_pipeMovementTimer.Start(CargoDepotModule.DURATION_TO_LOWER_PIPE);
            }
            else
            {
              if (!availableForImport.IsNotEmpty)
                return;
              if (this.Prototype.HasPipeCraneAnimation && !this.IsPipeDown)
              {
                this.m_pipeMovementTimer.Start(CargoDepotModule.DURATION_TO_LOWER_PIPE);
              }
              else
              {
                this.m_pendingQuantityFromShip = availableForImport;
                this.m_craneAnimationTimer.Start(this.Prototype.DurationPerExchange);
              }
            }
          }
          else
          {
            ProductQuantity availableForExport = this.getAvailableForExport();
            if (availableForExport.IsEmpty)
            {
              if (!this.Prototype.HasPipeCraneAnimation || !this.IsPipeDown)
                return;
              this.m_pipeMovementTimer.Start(CargoDepotModule.DURATION_TO_LOWER_PIPE);
            }
            else
            {
              if (!availableForExport.IsNotEmpty)
                return;
              if (this.Prototype.HasPipeCraneAnimation && !this.IsPipeDown)
              {
                this.m_pipeMovementTimer.Start(CargoDepotModule.DURATION_TO_LOWER_PIPE);
              }
              else
              {
                this.m_pendingQuantityToShip = availableForExport;
                this.Buffer.Value.RemoveExactly(availableForExport.Quantity);
                this.m_craneAnimationTimer.Start(this.Prototype.DurationPerExchange);
              }
            }
          }
        }
      }
    }

    private ProductQuantity getAvailableForImport()
    {
      if (this.StoredProduct.IsNone)
        return ProductQuantity.None;
      Option<CargoShipModule> shipModule = this.GetShipModule();
      if (shipModule.IsNone)
        return ProductQuantity.None;
      CargoShipModule cargoShipModule = shipModule.Value;
      return !cargoShipModule.CargoShip.IsDocked ? ProductQuantity.None : this.StoredProduct.Value.WithQuantity(cargoShipModule.Quantity.Min(this.UsableCapacity).Min(this.Prototype.QuantityPerExchange));
    }

    private ProductQuantity getAvailableForExport()
    {
      if (!this.Buffer.IsNone)
      {
        Quantity quantity1 = this.Buffer.Value.Quantity;
        if (!quantity1.IsNotPositive)
        {
          Option<CargoShipModule> shipModule = this.GetShipModule();
          if (shipModule.IsNone)
            return ProductQuantity.None;
          CargoShipModule cargoShipModule = shipModule.Value;
          if (!cargoShipModule.CargoShip.IsDocked || cargoShipModule.StoredProduct != this.Buffer.Value.Product)
            return ProductQuantity.None;
          ProductProto product = this.StoredProduct.Value;
          quantity1 = cargoShipModule.UsableCapacity;
          quantity1 = quantity1.Min(this.Buffer.Value.Quantity);
          Quantity quantity2 = quantity1.Min(this.Prototype.QuantityPerExchange);
          return product.WithQuantity(quantity2);
        }
      }
      return ProductQuantity.None;
    }

    public bool CanAcceptCargo()
    {
      if (!this.IsEnabled)
        return false;
      return this.ConstructionState == ConstructionState.Constructed || this.ConstructionState == ConstructionState.PreparingUpgrade;
    }

    public bool CanRemoveAssignedProduct(out LocStrFormatted error)
    {
      if (this.CurrentQuantity.IsPositive)
      {
        error = (LocStrFormatted) TrCore.RemovalError__HasProductsStored;
        return false;
      }
      Option<CargoShipModule> shipModule = this.GetShipModule();
      if (shipModule.HasValue && shipModule.Value.Quantity.IsPositive)
      {
        error = (LocStrFormatted) TrCore.RemovalError__ShipHasCargo;
        return false;
      }
      if (this.IsMovingCargo())
      {
        error = (LocStrFormatted) TrCore.RemovalError__DepotMovingCargo;
        return false;
      }
      error = LocStrFormatted.Empty;
      return true;
    }

    public override void ClearAssignedProduct()
    {
      if (!this.CanRemoveAssignedProduct(out LocStrFormatted _))
        return;
      base.ClearAssignedProduct();
      this.m_lastContractChecked = Option<ContractProto>.None;
    }

    public override EntityValidationResult CanStartDeconstruction()
    {
      return this.Depot.IsNone || this.Depot.Value.IsDestroyed || !this.IsMovingCargo() ? EntityValidationResult.Success : EntityValidationResult.CreateError((LocStrFormatted) TrCore.RemovalError__DepotMovingCargo);
    }

    public bool IsMovingCargo()
    {
      return this.m_craneAnimationTimer.IsNotFinished || this.m_pendingQuantityToStorage.IsNotEmpty || this.m_pipeMovementTimer.IsNotFinished;
    }

    private void attachToCargoDepot(WorldMapManager worldMapManager)
    {
      Assert.That<Option<CargoDepot>>(this.Depot).IsNone<CargoDepot>();
      KeyValuePair<CargoDepot, int>? ownerForModule = this.m_binder.FindOwnerForModule(this.Prototype, this.Transform);
      if (!ownerForModule.HasValue)
      {
        Log.Error("Failed to bind cargo module to a parent depot.");
      }
      else
      {
        KeyValuePair<CargoDepot, int> keyValuePair = ownerForModule.Value;
        this.Depot = (Option<CargoDepot>) keyValuePair.Key;
        keyValuePair = ownerForModule.Value;
        this.m_slot = keyValuePair.Value;
        CargoDepot cargoDepot = this.Depot.Value;
        cargoDepot.AddModule(this.m_slot, this);
        if (cargoDepot.ContractAssigned.HasValue)
        {
          ContractProto contractProto = cargoDepot.ContractAssigned.Value;
          if (!(contractProto.ProductToPayWith.Type != contractProto.ProductToBuy.Type))
            return;
          if (this.Prototype.ProductType == contractProto.ProductToBuy.Type)
          {
            this.AssignProduct(contractProto.ProductToBuy);
          }
          else
          {
            if (!(this.Prototype.ProductType == contractProto.ProductToPayWith.Type))
              return;
            this.AssignProduct(contractProto.ProductToPayWith);
          }
        }
        else
        {
          if (this.m_contractsManager.IsAnyContractUnlocked())
            return;
          ProductProto[] array = worldMapManager.AllMinableProducts.Where<ProductProto>((Func<ProductProto, bool>) (x => x.Type == this.Prototype.ProductType)).ToArray<ProductProto>();
          if (array.Length != 1)
            return;
          this.AssignProduct(array.First<ProductProto>());
        }
      }
    }

    public int RecalculateDepotSlot()
    {
      Assert.That<Option<CargoDepot>>(this.Depot).HasValue<CargoDepot>();
      KeyValuePair<CargoDepot, int>? ownerForModule = this.m_binder.FindOwnerForModule(this.Prototype, this.Transform);
      if (!ownerForModule.HasValue)
      {
        Log.Error("Failed to bind cargo module during re-attach!");
        return -1;
      }
      if (!(ownerForModule.Value.Key != this.Depot))
        return ownerForModule.Value.Value;
      Log.Error("Found a different depot during re-attach!");
      return -1;
    }

    protected override Option<IInputBufferPriorityProvider> GetVehicleInputBufferPriority(
      LogisticsBuffer buffer)
    {
      return !this.IsForImport() ? (Option<IInputBufferPriorityProvider>) (IInputBufferPriorityProvider) this : (Option<IInputBufferPriorityProvider>) Option.None;
    }

    protected override Option<IOutputBufferPriorityProvider> GetVehicleOutputBufferPriority(
      LogisticsBuffer buffer)
    {
      return this.IsForImport() ? (Option<IOutputBufferPriorityProvider>) (IOutputBufferPriorityProvider) this : (Option<IOutputBufferPriorityProvider>) Option.None;
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      return BufferStrategy.NoQuantityPreference(this.ImportPriority);
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      return BufferStrategy.NoQuantityPreference(this.ExportPriority);
    }

    public int GetCustomPriority(string id)
    {
      switch (id)
      {
        case "ImportPrio":
          return this.ImportPriority;
        case "ExportPrio":
          return this.ExportPriority;
        default:
          Assert.Fail("Unknown custom priority: " + id);
          return 0;
      }
    }

    public bool IsCustomPriorityVisible(string id)
    {
      if (this.Buffer.IsNone)
        return false;
      switch (id)
      {
        case "ImportPrio":
          return this.LogisticsInputControl == LogisticsControl.Enabled && !this.IsLogisticsInputDisabled;
        case "ExportPrio":
          return this.LogisticsOutputControl == LogisticsControl.Enabled && !this.IsLogisticsOutputDisabled;
        default:
          return false;
      }
    }

    public void SetCustomPriority(string id, int priority)
    {
      if (!GeneralPriorities.AssertAssignableRange(priority))
        return;
      switch (id)
      {
        case "ImportPrio":
          this.ImportPriority = priority;
          break;
        case "ExportPrio":
          this.ExportPriority = priority;
          break;
        default:
          Assert.Fail("Unknown custom priority: " + id);
          break;
      }
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      EntityConfigData data1 = data;
      CargoDepot valueOrNull = this.Depot.ValueOrNull;
      Option<ContractProto> option = valueOrNull != null ? valueOrNull.ContractAssigned : (Option<ContractProto>) Option.None;
      data1.SetContractAssigned(option);
      data.SetProductAssignedToDepotModule((Option<ProductProto>) this.Buffer.ValueOrNull?.Product);
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      if (this.GetType() != data.EntityType)
        return;
      Option<ContractProto>? contractAssigned1 = this.Depot.ValueOrNull?.ContractAssigned;
      Option<ContractProto> contractAssigned2 = data.GetContractAssigned();
      if ((contractAssigned1.HasValue ? (contractAssigned1.GetValueOrDefault() != contractAssigned2 ? 1 : 0) : 1) != 0)
        return;
      Option<ProductProto> assignedToDepotModule = data.GetProductAssignedToDepotModule();
      if (assignedToDepotModule.HasValue)
        this.AssignProduct(assignedToDepotModule.Value);
      else
        this.ClearAssignedProduct();
    }

    public static void Serialize(CargoDepotModule value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepotModule>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepotModule.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt((int) this.CurrentState);
      Option<CargoDepot>.Serialize(this.Depot, writer);
      writer.WriteInt(this.ExportPriority);
      writer.WriteInt(this.ImportPriority);
      writer.WriteBool(this.IsPipeDown);
      CargoDepotManager.Serialize(this.m_binder, writer);
      EntityNotificator.Serialize(this.m_contractNotMatchingNotif, writer);
      ContractsManager.Serialize(this.m_contractsManager, writer);
      TickTimer.Serialize(this.m_craneAnimationTimer, writer);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      writer.WriteBool(this.m_lacksPower);
      EntityNotificator.Serialize(this.m_noProductAssignedNotif, writer);
      Event.Serialize(this.m_onProductStoredChanged, writer);
      ProductQuantity.Serialize(this.m_pendingQuantityFromShip, writer);
      ProductQuantity.Serialize(this.m_pendingQuantityToShip, writer);
      ProductQuantity.Serialize(this.m_pendingQuantityToStorage, writer);
      TickTimer.Serialize(this.m_pipeMovementTimer, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<CargoDepotModuleProto>(this.m_proto);
      writer.WriteInt(this.m_slot);
      WorldMapManager.Serialize(this.m_worldMapManager, writer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static CargoDepotModule Deserialize(BlobReader reader)
    {
      CargoDepotModule cargoDepotModule;
      if (reader.TryStartClassDeserialization<CargoDepotModule>(out cargoDepotModule))
        reader.EnqueueDataDeserialization((object) cargoDepotModule, CargoDepotModule.s_deserializeDataDelayedAction);
      return cargoDepotModule;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CurrentState = (CargoDepotModule.State) reader.ReadInt();
      this.Depot = Option<CargoDepot>.Deserialize(reader);
      this.ExportPriority = reader.ReadInt();
      this.ImportPriority = reader.ReadInt();
      this.IsPipeDown = reader.ReadBool();
      reader.SetField<CargoDepotModule>(this, "m_binder", (object) CargoDepotManager.Deserialize(reader));
      this.m_contractNotMatchingNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<CargoDepotModule>(this, "m_contractsManager", (object) ContractsManager.Deserialize(reader));
      reader.SetField<CargoDepotModule>(this, "m_craneAnimationTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<CargoDepotModule>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.m_lacksPower = reader.ReadBool();
      this.m_noProductAssignedNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<CargoDepotModule>(this, "m_onProductStoredChanged", (object) Event.Deserialize(reader));
      this.m_pendingQuantityFromShip = ProductQuantity.Deserialize(reader);
      this.m_pendingQuantityToShip = ProductQuantity.Deserialize(reader);
      this.m_pendingQuantityToStorage = ProductQuantity.Deserialize(reader);
      reader.SetField<CargoDepotModule>(this, "m_pipeMovementTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<CargoDepotModule>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<CargoDepotModuleProto>();
      this.m_slot = reader.ReadInt();
      reader.SetField<CargoDepotModule>(this, "m_worldMapManager", (object) WorldMapManager.Deserialize(reader));
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      reader.RegisterInitAfterLoad<CargoDepotModule>(this, "initSelf", InitPriority.Normal);
    }

    static CargoDepotModule()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepotModule.DURATION_TO_LOWER_PIPE = 5.Seconds();
      CargoDepotModule.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      CargoDepotModule.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Paused,
      Broken,
      Working,
      MissingWorkers,
      NotEnoughPower,
      Idle,
    }

    [GenerateSerializer(false, null, 0)]
    public class CargoDepotBuffer : LogisticsBuffer
    {
      private readonly ProductStats m_productStats;
      private readonly int m_priority;
      private readonly IEntity m_entity;
      private bool m_isInput;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public CargoDepotBuffer(
        Quantity capacity,
        ProductProto product,
        IProductsManager productsManager,
        int priority,
        IEntity entity,
        bool isInput)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product);
        this.m_priority = priority;
        this.m_entity = entity;
        this.m_isInput = isInput;
        this.m_productStats = productsManager.GetStatsFor(product);
        this.register();
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        if (this.m_isInput)
          this.m_productStats.StoredUnavailableQuantityChange(diff);
        else
          this.m_productStats.StoredAvailableQuantityChange(diff);
      }

      private void register()
      {
        if (this.m_isInput)
          this.m_productStats.ProductsManager.AssetManager.AddGlobalInput((IProductBuffer) this, this.m_priority, this.m_entity.CreateOption<IEntity>());
        else
          this.m_productStats.ProductsManager.AssetManager.AddGlobalOutput((IProductBuffer) this, this.m_priority, this.m_entity.CreateOption<IEntity>());
        this.OnQuantityChanged(this.Quantity);
      }

      private void unregister()
      {
        if (this.m_isInput)
          this.m_productStats.ProductsManager.AssetManager.RemoveGlobalInput((IProductBuffer) this);
        else
          this.m_productStats.ProductsManager.AssetManager.RemoveGlobalOutput((IProductBuffer) this);
        this.OnQuantityChanged(-this.Quantity);
      }

      public void SetIsInput(bool isInput)
      {
        if (this.m_isInput == isInput)
          return;
        this.unregister();
        this.m_isInput = isInput;
        this.register();
      }

      public override void Destroy()
      {
        Assert.That<Quantity>(this.Quantity).IsZero("Buffer was not cleared before destroy!");
        this.unregister();
        base.Destroy();
      }

      public static void Serialize(CargoDepotModule.CargoDepotBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<CargoDepotModule.CargoDepotBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, CargoDepotModule.CargoDepotBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        writer.WriteGeneric<IEntity>(this.m_entity);
        writer.WriteBool(this.m_isInput);
        writer.WriteInt(this.m_priority);
        ProductStats.Serialize(this.m_productStats, writer);
      }

      public static CargoDepotModule.CargoDepotBuffer Deserialize(BlobReader reader)
      {
        CargoDepotModule.CargoDepotBuffer cargoDepotBuffer;
        if (reader.TryStartClassDeserialization<CargoDepotModule.CargoDepotBuffer>(out cargoDepotBuffer))
          reader.EnqueueDataDeserialization((object) cargoDepotBuffer, CargoDepotModule.CargoDepotBuffer.s_deserializeDataDelayedAction);
        return cargoDepotBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<CargoDepotModule.CargoDepotBuffer>(this, "m_entity", (object) reader.ReadGenericAs<IEntity>());
        this.m_isInput = reader.ReadBool();
        reader.SetField<CargoDepotModule.CargoDepotBuffer>(this, "m_priority", (object) reader.ReadInt());
        reader.SetField<CargoDepotModule.CargoDepotBuffer>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
      }

      static CargoDepotBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        CargoDepotModule.CargoDepotBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        CargoDepotModule.CargoDepotBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
