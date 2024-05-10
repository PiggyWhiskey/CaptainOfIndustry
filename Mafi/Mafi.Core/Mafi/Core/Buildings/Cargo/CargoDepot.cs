// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.CargoDepot
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Cargo.Ships;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Notifications;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Core.World.Contracts;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Cargo
{
  [GenerateSerializer(false, null, 0)]
  public class CargoDepot : 
    LayoutEntity,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IEntityWithSimpleLogisticsControl,
    IEntityWithCustomPriority,
    IOutputBufferPriorityProvider,
    IInputBufferPriorityProvider,
    IStaticEntityWithReservedOcean,
    ILayoutEntity,
    IEntityWithSimUpdate,
    IEntityWithCloneableConfig,
    IEntityWithPorts
  {
    private CargoDepotProto m_proto;
    private Option<CargoDepotModule>[] m_modules;
    private int m_occupiedModulesCount;
    private LogisticsBuffer m_fuelBuffer;
    private readonly Event<CargoDepotModule, int> m_onModuleAdded;
    private readonly Event<CargoDepotModule, int> m_onModuleUpgraded;
    private readonly Event<int> m_onModuleRemoved;
    private readonly EntityCollapseHelper m_collapseHelper;
    private int m_fuelImportPriority;
    private int m_fuelExportPriority;
    private readonly ICargoShipFactory m_cargoShipFactory;
    private readonly EntitiesManager m_entitiesManager;
    private readonly CargoDepotManager m_cargoDepotManager;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private bool m_upgradeInProgress;
    private EntityNotificator m_noCargoShipNotif;
    private EntityNotificator m_noModuleNotif;
    private readonly IProperty<Percent> m_fuelConsumptionMultiplier;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public CargoDepotProto Prototype
    {
      get => this.m_proto;
      private set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public Option<ContractProto> ContractAssigned { get; private set; }

    public override bool CanBePaused => false;

    public bool CanAcceptShip => this.IsEnabled && this.ReservedOceanAreaState.HasAnyValidAreaSet;

    public IUpgrader Upgrader { get; private set; }

    public LogisticsControl LogisticsInputControl => LogisticsControl.Enabled;

    public LogisticsControl LogisticsOutputControl => LogisticsControl.Enabled;

    public bool IsLogisticsInputDisabled { get; private set; }

    public bool IsLogisticsOutputDisabled { get; private set; }

    /// <summary>
    /// Slots for modules. Index of a slot/module corresponds to position of the slot. Leftmost slot has index 0,
    /// rightmost slot has index m_modules.Length - 1 (looking from the side of the modules).
    /// </summary>
    [DoNotSaveCreateNewOnLoad("new ReadOnlyArray<Option<CargoDepotModule>>(m_modules)", 0)]
    public ReadOnlyArray<Option<CargoDepotModule>> Modules { get; private set; }

    public int SlotCount => this.m_modules.Length;

    /// <summary>The cargo ship that assigned to this depot.</summary>
    public Option<Mafi.Core.Buildings.Cargo.Ships.CargoShip> CargoShip { get; private set; }

    public ILogisticsBufferReadOnly FuelBuffer => (ILogisticsBufferReadOnly) this.m_fuelBuffer;

    public IEvent<CargoDepotModule, int> OnModuleAdded
    {
      get => (IEvent<CargoDepotModule, int>) this.m_onModuleAdded;
    }

    public IEvent<CargoDepotModule, int> OnModuleUpgraded
    {
      get => (IEvent<CargoDepotModule, int>) this.m_onModuleUpgraded;
    }

    public IEvent<int> OnModuleRemoved => (IEvent<int>) this.m_onModuleRemoved;

    public ReservedOceanAreaState ReservedOceanAreaState { get; private set; }

    public CargoDepot(
      EntityId id,
      CargoDepotProto cargoDepotProto,
      TileTransform transform,
      EntityContext context,
      CargoDepotManager cargoDepotManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      ICargoShipFactory cargoShipFactory,
      ILayoutEntityUpgraderFactory upgraderFactory,
      EntitiesManager entitiesManager,
      IPropertiesDb propsDb,
      EntityCollapseHelper collapseHelper)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onModuleAdded = new Event<CargoDepotModule, int>();
      this.m_onModuleUpgraded = new Event<CargoDepotModule, int>();
      this.m_onModuleRemoved = new Event<int>();
      this.m_fuelImportPriority = 8;
      this.m_fuelExportPriority = 8;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) cargoDepotProto, transform, context);
      Assert.That<bool>(this.Transform.IsReflected).IsFalse("Cargo depot should not be reflected!");
      this.m_proto = cargoDepotProto;
      this.m_cargoDepotManager = cargoDepotManager;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_cargoShipFactory = cargoShipFactory;
      this.m_entitiesManager = entitiesManager;
      this.m_collapseHelper = collapseHelper;
      this.m_fuelConsumptionMultiplier = propsDb.GetProperty<Percent>(IdsCore.PropertyIds.ShipsFuelConsumptionMultiplier);
      this.m_fuelConsumptionMultiplier.OnChange.Add<CargoDepot>(this, new Action<Percent>(this.onFuelMultiplierChange));
      this.m_noCargoShipNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CargoDepotHasNoShip);
      this.m_noModuleNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CargoDepotHasNoModule);
      this.Upgrader = upgraderFactory.CreateInstance<CargoDepotProto, CargoDepot>(this, this.Prototype);
      this.ReservedOceanAreaState = new ReservedOceanAreaState((IProtoWithReservedOcean) cargoDepotProto, (IStaticEntityWithReservedOcean) this, new EntityNotificationProto.ID?(IdsCore.Notifications.OceanAccessBlocked), context.NotificationsManager);
      this.m_fuelBuffer = this.createFuelBuffer(20.Quantity());
      this.m_fuelBuffer.SetImportStep(0);
      this.m_modules = new Option<CargoDepotModule>[this.Prototype.ModuleSlots.Length];
      this.Modules = new ReadOnlyArray<Option<CargoDepotModule>>(this.m_modules);
      this.SetLogisticsOutputDisabled(true);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf() => this.updateFuelCapacity();

    public void AssignContract(ContractProto contract)
    {
      if (this.ContractAssigned.HasValue)
      {
        Assert.Fail("Depot already has contract assigned!");
      }
      else
      {
        if (!this.CanAssignContract(contract, out LocStrFormatted _))
          return;
        this.ContractAssigned = (Option<ContractProto>) contract;
        if (contract.ProductToBuy.Type != contract.ProductToPayWith.Type)
        {
          foreach (Option<CargoDepotModule> module in this.m_modules)
          {
            if (!module.IsNone)
            {
              CargoDepotModule cargoDepotModule = module.Value;
              if (cargoDepotModule.StoredProduct.IsNone)
              {
                if (cargoDepotModule.Prototype.ProductType == contract.ProductToBuy.Type)
                  cargoDepotModule.AssignProduct(contract.ProductToBuy);
                else if (cargoDepotModule.Prototype.ProductType == contract.ProductToPayWith.Type)
                  cargoDepotModule.AssignProduct(contract.ProductToPayWith);
              }
            }
          }
        }
        foreach (Option<CargoDepotModule> module in this.m_modules)
          module.ValueOrNull?.OnContractChangedOrCleared();
      }
    }

    public void ClearContract()
    {
      if (this.ContractAssigned.IsNone)
        return;
      this.ContractAssigned = Option<ContractProto>.None;
      foreach (Option<CargoDepotModule> module in this.m_modules)
        module.ValueOrNull?.OnContractChangedOrCleared();
    }

    public bool CanAssignContract(ContractProto contract, out LocStrFormatted errorReason)
    {
      errorReason = LocStrFormatted.Empty;
      if (this.ContractAssigned.HasValue)
        return false;
      foreach (Option<CargoDepotModule> module in this.m_modules)
      {
        if (!module.IsNone)
        {
          CargoDepotModuleProto prototype = module.Value.Prototype;
          if (contract.ProductToBuy.Type != prototype.ProductType && contract.ProductToPayWith.Type != prototype.ProductType)
          {
            errorReason = TrCore.ContractAssignCheck__ModuleNotSupported.Format(prototype.Strings.Name.TranslatedString);
            return false;
          }
          if (module.Value.StoredProduct.HasValue)
          {
            ProductProto productProto = module.Value.StoredProduct.Value;
            if ((Proto) contract.ProductToBuy != (Proto) productProto && (Proto) contract.ProductToPayWith != (Proto) productProto)
            {
              errorReason = TrCore.ContractAssignCheck__IncompatibleProduct.Format(productProto.Strings.Name.TranslatedString);
              return false;
            }
          }
        }
      }
      return true;
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.m_noCargoShipNotif.NotifyIff(this.IsEnabled && this.CargoShip.IsNone, (IEntity) this);
      this.m_noModuleNotif.NotifyIff(this.IsEnabled && this.m_occupiedModulesCount == 0 && this.CargoShip.HasValue, (IEntity) this);
      this.replaceFuelBufferIfNeeded();
      if (this.m_upgradeInProgress && this.IsConstructed)
      {
        this.m_upgradeInProgress = false;
        if (this.CargoShip.HasValue)
          this.CargoShip.Value.RequestUpgrade();
      }
      if (this.IsNotEnabled)
        return;
      if (this.CargoShip.IsNone)
      {
        AvailableCargoShipData? dataForCargoDepot = this.m_cargoDepotManager.GetShipSpawnDataForCargoDepot();
        if (!dataForCargoDepot.HasValue)
          return;
        this.CargoShip = (Option<Mafi.Core.Buildings.Cargo.Ships.CargoShip>) this.m_cargoShipFactory.AddCargoShip(this, this.Prototype.CargoShipProto, dataForCargoDepot.Value.FuelProto);
        this.updateFuelCapacity();
        this.CargoShip.Value.RefillFuel(dataForCargoDepot.Value.FuelQuantity);
      }
      else
      {
        if (this.CargoShip.Value.IsDocked && this.m_fuelBuffer.IsNotEmpty())
          this.m_fuelBuffer.RemoveExactly(this.m_fuelBuffer.Quantity - this.CargoShip.Value.StoreFuelAsMuchAs(this.m_fuelBuffer.Quantity));
        if (this.CargoShip.Value.State != Mafi.Core.Buildings.Cargo.Ships.CargoShip.ShipState.Docked)
          return;
        foreach (Option<CargoDepotModule> module in this.m_modules)
        {
          if (module.HasValue)
            module.Value.UpdateProductExchange();
        }
      }
    }

    public Option<CargoDepotModule> GetModule(int slot) => this.m_modules[slot];

    public void NotifyModuleUpgraded(int slot, CargoDepotModule module)
    {
      this.m_onModuleUpgraded.Invoke(module, slot);
    }

    public void AddModule(int slot, CargoDepotModule module)
    {
      Assert.That<int>(slot).IsValidIndexFor<Option<CargoDepotModule>>(this.m_modules);
      if (this.m_modules[slot].HasValue)
      {
        Log.Error(string.Format("Module {0} in slot {1} already exists ", (object) this.m_modules[slot].Value.Id, (object) slot) + string.Format("cannot add new with id {0}!", (object) module.Id));
      }
      else
      {
        ++this.m_occupiedModulesCount;
        this.m_modules[slot] = (Option<CargoDepotModule>) module;
        this.m_onModuleAdded.Invoke(module, slot);
      }
    }

    public void RemoveModule(CargoDepotModule module)
    {
      int index = Array.IndexOf<Option<CargoDepotModule>>(this.m_modules, Option.Some<CargoDepotModule>(module));
      if (index != -1)
      {
        --this.m_occupiedModulesCount;
        this.m_modules[index] = (Option<CargoDepotModule>) Option.None;
        this.m_onModuleRemoved.Invoke(index);
      }
      else
        Assert.Fail("Module to be removed not found.");
    }

    public void Cheat_FuelExactly(Quantity fuelToAdd)
    {
      this.m_fuelBuffer.StoreExactly(fuelToAdd);
      this.Context.ProductsManager.ProductCreated(this.m_fuelBuffer.Product, fuelToAdd, CreateReason.Cheated);
    }

    public void UpdateFuelImportExportStep(int importStep, int exportStep)
    {
      if (importStep >= 0)
      {
        this.m_fuelBuffer.SetImportStep(importStep);
        if (this.IsLogisticsInputDisabled)
          this.SetLogisticsInputDisabled(false);
      }
      if (exportStep < 0)
        return;
      this.m_fuelBuffer.SetExportStep(exportStep);
      if (!this.IsLogisticsOutputDisabled)
        return;
      this.SetLogisticsOutputDisabled(false);
    }

    public bool IsUnloadingCargo()
    {
      foreach (Option<CargoDepotModule> module in this.m_modules)
      {
        if (module.HasValue && module.Value.IsMovingCargo())
          return true;
      }
      return false;
    }

    public override EntityValidationResult CanStartDeconstruction()
    {
      return ((IEnumerable<Option<CargoDepotModule>>) this.m_modules).Any<Option<CargoDepotModule>>((Func<Option<CargoDepotModule>, bool>) (m => m.HasValue)) ? EntityValidationResult.CreateError((LocStrFormatted) TrCore.RemovalError__RemoveModulesFirst) : EntityValidationResult.Success;
    }

    public Quantity ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      return this.IsNotEnabled ? pq.Quantity : this.m_fuelBuffer.StoreAsMuchAs(pq);
    }

    protected override void OnDestroy()
    {
      foreach (Option<CargoDepotModule> option in ((IEnumerable<Option<CargoDepotModule>>) this.m_modules).ToArray<Option<CargoDepotModule>>())
      {
        if (option.HasValue)
          Assert.That<bool>(this.m_collapseHelper.TryDestroyEntityAndAddRubble((IStaticEntity) option.Value)).IsTrue();
      }
      this.CargoShip.ValueOrNull?.RequestDestroy();
      this.CargoShip = (Option<Mafi.Core.Buildings.Cargo.Ships.CargoShip>) Option.None;
      this.m_fuelConsumptionMultiplier.OnChange.Remove<CargoDepot>(this, new Action<Percent>(this.onFuelMultiplierChange));
      base.OnDestroy();
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      CargoDepotProto cargoDepotProto = this.Prototype.Upgrade.NextTier.Value;
      RelTile3i layoutSize = this.Prototype.Layout.LayoutSize;
      RelTile2i xy1 = layoutSize.Xy;
      layoutSize = cargoDepotProto.Layout.LayoutSize;
      RelTile2i xy2 = layoutSize.Xy;
      RelTile2i relTile2i = xy1 - xy2;
      relTile2i = relTile2i.Rotate(this.Transform.Rotation);
      TileTransform transform = new TileTransform(this.Transform.Position + (relTile2i / 2).ExtendZ(0), this.Transform.Rotation, this.Transform.IsReflected);
      this.Prototype = cargoDepotProto;
      this.SetTransform(transform);
      Option<CargoDepotModule>[] modules = this.m_modules;
      this.m_modules = new Option<CargoDepotModule>[cargoDepotProto.ModuleSlots.Length];
      foreach (Option<CargoDepotModule> option in modules)
      {
        if (option.HasValue)
        {
          int index = option.Value.RecalculateDepotSlot();
          if (index >= 0)
            this.m_modules[index] = option;
        }
      }
      this.Modules = new ReadOnlyArray<Option<CargoDepotModule>>(this.m_modules);
      this.m_upgradeInProgress = true;
    }

    private void onFuelMultiplierChange(Percent newValue) => this.updateFuelCapacity();

    private void updateFuelCapacity()
    {
      if (!this.CargoShip.HasValue)
        return;
      this.m_fuelBuffer.IncreaseCapacityTo(this.CargoShip.Value.GetFuelReserveNeeded());
    }

    /// <summary>
    /// Called by the old ship to notify this depot that the new ship is ready.
    /// </summary>
    public void ReplaceShipAndDestroyCurrent(Mafi.Core.Buildings.Cargo.Ships.CargoShip newShip)
    {
      Mafi.Core.Buildings.Cargo.Ships.CargoShip valueOrNull = this.CargoShip.ValueOrNull;
      if (this.CargoShip != (Mafi.Core.Buildings.Cargo.Ships.CargoShip) null)
        this.m_entitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) valueOrNull, EntityRemoveReason.Remove);
      this.CargoShip = (Option<Mafi.Core.Buildings.Cargo.Ships.CargoShip>) newShip;
      this.updateFuelCapacity();
    }

    public TileTransform GetModuleTransform(int slotIndex, CargoDepotModuleProto moduleProto)
    {
      Assert.That<bool>(this.Transform.IsReflected).IsFalse("Cargo depot should not be reflected!");
      return new TileTransform(this.Prototype.ModuleSlots[slotIndex].GetModulePosition(this, moduleProto), this.Transform.Rotation, this.Transform.IsReflected);
    }

    private void replaceFuelBufferIfNeeded()
    {
      if (this.CargoShip.IsNone || (Proto) this.CargoShip.Value.FuelProto == (Proto) this.m_fuelBuffer.Product)
        return;
      this.m_vehicleBuffersRegistry.UnregisterInputBufferAndAssert((IProductBuffer) this.m_fuelBuffer);
      this.Context.AssetTransactionManager.ClearBuffer((IProductBuffer) this.m_fuelBuffer);
      this.m_fuelBuffer.Destroy();
      LogisticsBuffer fuelBuffer = this.m_fuelBuffer;
      this.m_fuelBuffer = this.createFuelBuffer(this.CargoShip.Value.GetFuelReserveNeeded());
      this.m_fuelBuffer.SetImportStep(fuelBuffer.ImportUntilPercent);
    }

    private LogisticsBuffer createFuelBuffer(Quantity capacity)
    {
      ProductProto product = this.CargoShip.ValueOrNull?.FuelProto ?? this.Context.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Diesel);
      LogisticsBuffer buffer = new LogisticsBuffer(capacity, product, true);
      this.m_vehicleBuffersRegistry.RegisterInputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer, (IInputBufferPriorityProvider) this);
      return buffer;
    }

    public void SetLogisticsInputDisabled(bool isDisabled)
    {
      if (this.IsLogisticsInputDisabled == isDisabled)
        return;
      this.IsLogisticsInputDisabled = isDisabled;
      if (this.IsLogisticsInputDisabled)
      {
        this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer((IProductBuffer) this.m_fuelBuffer);
        this.m_fuelBuffer.SetImportStep(Percent.Zero);
      }
      else
        this.m_vehicleBuffersRegistry.TryRegisterInputBuffer((IStaticEntity) this, (IProductBuffer) this.m_fuelBuffer, (IInputBufferPriorityProvider) this);
    }

    public void SetLogisticsOutputDisabled(bool isDisabled)
    {
      if (this.IsLogisticsOutputDisabled == isDisabled)
        return;
      this.IsLogisticsOutputDisabled = isDisabled;
      if (this.IsLogisticsOutputDisabled)
      {
        this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) this.m_fuelBuffer);
        this.m_fuelBuffer.SetExportStep(Percent.Hundred);
      }
      else
        this.m_vehicleBuffersRegistry.TryRegisterOutputBuffer((IStaticEntity) this, (IProductBuffer) this.m_fuelBuffer, (IOutputBufferPriorityProvider) this);
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      return this.m_fuelBuffer.ExportFromPercent.IsNearHundred ? BufferStrategy.Ignore : this.m_fuelBuffer.GetOutputPriority(this.m_fuelExportPriority, request);
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      return this.m_fuelBuffer.GetInputPriority(this.m_fuelImportPriority, pendingQuantity);
    }

    public int GetCustomPriority(string id)
    {
      switch (id)
      {
        case "FuelImportPrio":
          return this.m_fuelImportPriority;
        case "FuelExportPrio":
          return this.m_fuelExportPriority;
        default:
          Assert.Fail("Unknown custom priority: " + id);
          return 0;
      }
    }

    public bool IsCustomPriorityVisible(string id)
    {
      switch (id)
      {
        case "FuelImportPrio":
          return this.m_fuelBuffer.ImportUntilPercent.IsPositive;
        case "FuelExportPrio":
          return this.m_fuelBuffer.ExportFromPercent < Percent.Hundred;
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
        case "FuelImportPrio":
          this.m_fuelImportPriority = priority;
          break;
        case "FuelExportPrio":
          this.m_fuelExportPriority = priority;
          break;
        default:
          Assert.Fail("Unknown custom priority: " + id);
          break;
      }
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.FuelImportUntilPercent = new Percent?(this.m_fuelBuffer.ImportUntilPercent);
      data.FuelExportFromPercent = new Percent?(this.m_fuelBuffer.ExportFromPercent);
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      Percent? nullable;
      if (data.FuelImportUntilPercent.HasValue)
      {
        LogisticsBuffer fuelBuffer = this.m_fuelBuffer;
        nullable = data.FuelImportUntilPercent;
        Percent percent = nullable.Value;
        fuelBuffer.SetImportStep(percent);
      }
      nullable = data.FuelExportFromPercent;
      if (!nullable.HasValue)
        return;
      LogisticsBuffer fuelBuffer1 = this.m_fuelBuffer;
      nullable = data.FuelExportFromPercent;
      Percent percent1 = nullable.Value;
      fuelBuffer1.SetExportStep(percent1);
    }

    public static void Serialize(CargoDepot value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDepot>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDepot.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<Mafi.Core.Buildings.Cargo.Ships.CargoShip>.Serialize(this.CargoShip, writer);
      Option<ContractProto>.Serialize(this.ContractAssigned, writer);
      writer.WriteBool(this.IsLogisticsInputDisabled);
      writer.WriteBool(this.IsLogisticsOutputDisabled);
      CargoDepotManager.Serialize(this.m_cargoDepotManager, writer);
      writer.WriteGeneric<ICargoShipFactory>(this.m_cargoShipFactory);
      EntityCollapseHelper.Serialize(this.m_collapseHelper, writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      LogisticsBuffer.Serialize(this.m_fuelBuffer, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_fuelConsumptionMultiplier);
      writer.WriteInt(this.m_fuelExportPriority);
      writer.WriteInt(this.m_fuelImportPriority);
      writer.WriteArray<Option<CargoDepotModule>>(this.m_modules);
      EntityNotificator.Serialize(this.m_noCargoShipNotif, writer);
      EntityNotificator.Serialize(this.m_noModuleNotif, writer);
      writer.WriteInt(this.m_occupiedModulesCount);
      Event<CargoDepotModule, int>.Serialize(this.m_onModuleAdded, writer);
      Event<int>.Serialize(this.m_onModuleRemoved, writer);
      Event<CargoDepotModule, int>.Serialize(this.m_onModuleUpgraded, writer);
      writer.WriteGeneric<CargoDepotProto>(this.m_proto);
      writer.WriteBool(this.m_upgradeInProgress);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      ReservedOceanAreaState.Serialize(this.ReservedOceanAreaState, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static CargoDepot Deserialize(BlobReader reader)
    {
      CargoDepot cargoDepot;
      if (reader.TryStartClassDeserialization<CargoDepot>(out cargoDepot))
        reader.EnqueueDataDeserialization((object) cargoDepot, CargoDepot.s_deserializeDataDelayedAction);
      return cargoDepot;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CargoShip = Option<Mafi.Core.Buildings.Cargo.Ships.CargoShip>.Deserialize(reader);
      this.ContractAssigned = Option<ContractProto>.Deserialize(reader);
      this.IsLogisticsInputDisabled = reader.ReadBool();
      this.IsLogisticsOutputDisabled = reader.ReadBool();
      reader.SetField<CargoDepot>(this, "m_cargoDepotManager", (object) CargoDepotManager.Deserialize(reader));
      reader.SetField<CargoDepot>(this, "m_cargoShipFactory", (object) reader.ReadGenericAs<ICargoShipFactory>());
      reader.SetField<CargoDepot>(this, "m_collapseHelper", (object) EntityCollapseHelper.Deserialize(reader));
      reader.SetField<CargoDepot>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      this.m_fuelBuffer = LogisticsBuffer.Deserialize(reader);
      reader.SetField<CargoDepot>(this, "m_fuelConsumptionMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      this.m_fuelExportPriority = reader.ReadInt();
      this.m_fuelImportPriority = reader.ReadInt();
      this.m_modules = reader.ReadArray<Option<CargoDepotModule>>();
      this.m_noCargoShipNotif = EntityNotificator.Deserialize(reader);
      this.m_noModuleNotif = EntityNotificator.Deserialize(reader);
      this.m_occupiedModulesCount = reader.ReadInt();
      reader.SetField<CargoDepot>(this, "m_onModuleAdded", (object) Event<CargoDepotModule, int>.Deserialize(reader));
      reader.SetField<CargoDepot>(this, "m_onModuleRemoved", (object) Event<int>.Deserialize(reader));
      reader.SetField<CargoDepot>(this, "m_onModuleUpgraded", (object) Event<CargoDepotModule, int>.Deserialize(reader));
      this.m_proto = reader.ReadGenericAs<CargoDepotProto>();
      this.m_upgradeInProgress = reader.ReadBool();
      reader.SetField<CargoDepot>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      this.Modules = new ReadOnlyArray<Option<CargoDepotModule>>(this.m_modules);
      this.ReservedOceanAreaState = ReservedOceanAreaState.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      reader.RegisterInitAfterLoad<CargoDepot>(this, "initSelf", InitPriority.Low);
    }

    static CargoDepot()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDepot.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      CargoDepot.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
