// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.Storage
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Notifications;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  [GenerateSerializer(false, null, 0)]
  public class Storage : 
    StorageBase,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IEntityEnforcingAssignedVehicles,
    IEntityAssignedWithVehicles,
    IEntityAssignedAsOutput,
    ILayoutEntity,
    IEntityAssignedAsInput,
    IInputBufferPriorityProvider,
    IOutputBufferPriorityProvider,
    IEntityWithCustomPriority,
    IEntityWithSimUpdate,
    IEntityWithCloneableConfig,
    IEntityWithAlertAbove,
    IEntityWithStorageAlert,
    IEntityWithAlertBelow,
    IElectricityConsumingEntity,
    IEntityWithGeneralPriority
  {
    private static readonly Duration CONSUME_POWER_FOR;
    private StorageProto m_proto;
    private readonly Set<IEntityAssignedAsInput> m_assignedInputEntities;
    private readonly Set<IEntityAssignedAsOutput> m_assignedOutputEntities;
    private bool? m_wasLogisticsInputDisabledBefore;
    private bool? m_wasLogisticsOutputDisabledBefore;
    public const string IMPORT_PRIO_ID = "ImportPrio";
    public const string EXPORT_PRIO_ID = "ExportPrio";
    private bool m_isEnforcingCustomVehicles;
    private Option<AssignedVehicles<Truck, TruckProto>> m_assignedTrucks;
    private Option<IElectricityConsumer> m_electricityConsumer;
    [DoNotSave(0, null)]
    private bool m_ignorePower;
    private static readonly Percent NOTIFICATION_HYSTERESIS;
    private EntityNotificatorWithProtoParam m_supplyToLowNotif;
    private EntityNotificatorWithProtoParam m_supplyToHighNotif;
    [DoNotSave(0, null)]
    private Option<Storage> m_storageBehindPorts;
    private EntityNotificator m_invalidImportRouteNotif;
    private EntityNotificator m_invalidExportRouteNotif;
    private Quantity m_receivedFromPortsLastUpdate;
    private bool m_failedToConsumePower;
    private Duration m_consumePowerFor;
    [DoNotSave(0, null)]
    private bool m_canWorkOnLowPower;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public StorageProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (StorageBaseProto) value;
      }
    }

    public override bool CanBePaused => true;

    public IUpgrader Upgrader { get; private set; }

    public Percent ImportUntilPercent
    {
      get => !this.Buffer.HasValue ? Percent.Zero : this.Buffer.Value.ImportUntilPercent;
    }

    public Percent ExportFromPercent
    {
      get => !this.Buffer.HasValue ? Percent.Hundred : this.Buffer.Value.ExportFromPercent;
    }

    public bool CleaningInProgress => this.Buffer.HasValue && this.Buffer.Value.CleaningMode;

    public Quantity UsableCapacity
    {
      get => !this.Buffer.HasValue ? Quantity.Zero : this.Buffer.Value.UsableCapacity;
    }

    public IReadOnlySet<IEntityAssignedAsInput> AssignedInputs
    {
      get => (IReadOnlySet<IEntityAssignedAsInput>) this.m_assignedInputEntities;
    }

    public IReadOnlySet<IEntityAssignedAsOutput> AssignedOutputs
    {
      get => (IReadOnlySet<IEntityAssignedAsOutput>) this.m_assignedOutputEntities;
    }

    public bool AllowNonAssignedOutput { get; set; }

    public override LogisticsControl LogisticsInputControl => LogisticsControl.Enabled;

    public override LogisticsControl LogisticsOutputControl => LogisticsControl.Enabled;

    public override bool IsGeneralPriorityVisible => !this.m_canWorkOnLowPower;

    public int ImportPriority { get; private set; }

    public int ExportPriority { get; private set; }

    public bool AreOnlyAssignedVehiclesAllowed
    {
      get
      {
        if (!this.m_isEnforcingCustomVehicles)
          return false;
        AssignedVehicles<Truck, TruckProto> valueOrNull = this.m_assignedTrucks.ValueOrNull;
        return valueOrNull != null && valueOrNull.All.IsNotEmpty<Truck>();
      }
    }

    public IIndexable<Vehicle> AllVehicles
    {
      get => this.m_assignedTrucks.ValueOrNull?.AllUntyped ?? Indexable<Vehicle>.Empty;
    }

    internal bool IsGodModeEnabled { get; private set; }

    protected override bool IoDisabled => this.m_failedToConsumePower && !this.m_canWorkOnLowPower;

    public bool AreAlertsAvailable => this.StoredProduct.HasValue;

    public bool AlertWhenAboveEnabled { get; set; }

    public Percent AlertWhenAbove { get; private set; }

    public bool AlertWhenBelowEnabled { get; set; }

    public Percent AlertWhenBelow { get; private set; }

    public Electricity PowerRequired
    {
      get
      {
        return !this.m_ignorePower ? this.Prototype.PowerConsumedForProductsExchange : Electricity.Zero;
      }
    }

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.As<IElectricityConsumerReadonly>();
    }

    public Storage(
      EntityId id,
      StorageProto storageProto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IVehiclesManager vehiclesManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_assignedInputEntities = new Set<IEntityAssignedAsInput>();
      this.m_assignedOutputEntities = new Set<IEntityAssignedAsOutput>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CAlertWhenAbove\u003Ek__BackingField = Percent.Hundred;
      // ISSUE: reference to a compiler-generated field
      this.\u003CAlertWhenBelow\u003Ek__BackingField = Percent.Zero;
      this.m_storageBehindPorts = Option<Storage>.None;
      this.m_consumePowerFor = Duration.Zero;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StorageBaseProto) storageProto, transform, context, simLoopEvents, vehicleBuffersRegistry, true);
      this.Prototype = storageProto;
      this.m_assignedTrucks = (Option<AssignedVehicles<Truck, TruckProto>>) new AssignedVehicles<Truck, TruckProto>((IEntityAssignedWithVehicles) this);
      this.ImportPriority = 8;
      this.ExportPriority = 8;
      this.Upgrader = upgraderFactory.CreateInstance<StorageProto, Storage>(this, storageProto);
      this.updateProperties();
      this.m_supplyToLowNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.StorageSupplyTooLow);
      this.m_supplyToHighNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.StorageSupplyTooHigh);
      this.m_invalidImportRouteNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.InvalidImportRoute);
      this.m_invalidExportRouteNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.InvalidExportRoute);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion, DependencyResolver resolver)
    {
      this.updateProperties();
      this.rescanOutputPorts(true);
    }

    private void updateProperties()
    {
      this.m_canWorkOnLowPower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.LogisticsCanWorkOnLowPower);
      if (this.m_canWorkOnLowPower)
        ((IEntityWithGeneralPriorityFriend) this).SetGeneralPriorityInternal(0);
      this.m_ignorePower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.LogisticsIgnorePower);
      if (this.PowerRequired.IsPositive && this.m_electricityConsumer.IsNone)
        this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this).CreateOption<IElectricityConsumer>();
      this.m_electricityConsumer.ValueOrNull?.OnPowerRequiredChanged();
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    protected override LogisticsBuffer CreateNewBuffer(Quantity capacity, ProductProto product)
    {
      return (LogisticsBuffer) new Storage.StorageBuffer(this.Prototype.Capacity, product, this.Context.ProductsManager, (IEntity) this);
    }

    public override bool IsProductSupported(ProductProto product)
    {
      return this.Prototype.IsProductSupported(product);
    }

    public override void SetConstructionState(ConstructionState state)
    {
      base.SetConstructionState(state);
      if (state != ConstructionState.Constructed)
        return;
      Storage storage = (Storage) null;
      foreach (IoPort port in this.Ports)
      {
        if (port.IsConnectedAsInput && port.ConnectedPort.ValueOrNull?.OwnerEntity is Storage ownerEntity && storage != ownerEntity)
        {
          storage = ownerEntity;
          storage.rescanOutputPorts(false);
        }
      }
    }

    /// <summary>
    /// Performance optimization.
    /// It turns out that it is at least 2 times slower to exchange products via ports instead of
    /// between storage directly. One reason can be that we are invoking global capacity changes
    /// events which is not great.
    /// </summary>
    private void rescanOutputPorts(bool doNotUpdateLogistics)
    {
      bool flag1 = true;
      Storage storage = (Storage) null;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      int index = 0;
      while (true)
      {
        int num5 = index;
        ImmutableArray<IoPort> ports = this.Ports;
        int length = ports.Length;
        if (num5 < length)
        {
          ports = this.Ports;
          Storage ownerEntity = ports[index].ConnectedPort.ValueOrNull?.OwnerEntity as Storage;
          ports = this.Ports;
          if (ports[index].Type == IoPortType.Output)
            ++num3;
          else
            ++num4;
          ports = this.Ports;
          if (ports[index].IsConnectedAsInput && ownerEntity != null)
          {
            ++num1;
          }
          else
          {
            ports = this.Ports;
            if (ports[index].IsConnectedAsOutput)
            {
              if (ownerEntity != null)
              {
                ++num2;
                if (storage != null && storage != ownerEntity)
                  flag1 = false;
                storage = ownerEntity;
              }
              else
                flag1 = false;
            }
          }
          ++index;
        }
        else
          break;
      }
      this.m_storageBehindPorts = !(storage != null & flag1) ? Option<Storage>.None : (Option<Storage>) storage;
      bool flag2 = this.m_storageBehindPorts.IsNone || this.m_storageBehindPorts.Value.IsConstructed;
      if (doNotUpdateLogistics)
        return;
      if (num1 >= num4)
      {
        if (!this.m_wasLogisticsInputDisabledBefore.HasValue)
          this.m_wasLogisticsInputDisabledBefore = new bool?(this.IsLogisticsInputDisabled);
        this.SetLogisticsInputDisabled(true);
      }
      else
      {
        if (this.m_wasLogisticsInputDisabledBefore.HasValue)
          this.SetLogisticsInputDisabled(this.m_wasLogisticsInputDisabledBefore.Value);
        this.m_wasLogisticsInputDisabledBefore = new bool?();
      }
      if (num2 >= num3 & flag2)
      {
        if (!this.m_wasLogisticsOutputDisabledBefore.HasValue)
          this.m_wasLogisticsOutputDisabledBefore = new bool?(this.IsLogisticsOutputDisabled);
        this.SetLogisticsOutputDisabled(true);
      }
      else
      {
        if (this.m_wasLogisticsOutputDisabledBefore.HasValue)
          this.SetLogisticsOutputDisabled(this.m_wasLogisticsOutputDisabledBefore.Value);
        this.m_wasLogisticsOutputDisabledBefore = new bool?();
      }
    }

    public bool IsChainedTo(Storage storage)
    {
      Option<Storage> storageBehindPorts = this.m_storageBehindPorts;
      for (int index = 0; index < 50 && !storageBehindPorts.IsNone; ++index)
      {
        if (storageBehindPorts == storage)
          return true;
        storageBehindPorts = storageBehindPorts.Value.m_storageBehindPorts;
      }
      return false;
    }

    public void SetAlertAbove(Percent above)
    {
      if (above > Percent.Hundred)
        return;
      this.AlertWhenAbove = above;
    }

    public void SetAlertBelow(Percent below)
    {
      if (below < Percent.Zero)
        return;
      this.AlertWhenBelow = below;
    }

    public void SetAlertAboveEnabled(bool isEnabled) => this.AlertWhenAboveEnabled = isEnabled;

    public void SetAlertBelowEnabled(bool isEnabled) => this.AlertWhenBelowEnabled = isEnabled;

    public virtual Upoints GetQuickRemoveCost(out bool canAfford)
    {
      canAfford = false;
      if (this.Buffer.IsNone)
        return Upoints.Zero;
      LogisticsBuffer logisticsBuffer = this.Buffer.Value;
      if (logisticsBuffer.IsEmpty || !logisticsBuffer.CleaningMode)
        return Upoints.Zero;
      Upoints unity = QuickDeliverCostHelper.QuantityToUnityCost(logisticsBuffer.Quantity.Value, this.Context.UpointsManager.QuickActionCostMultiplier) ?? Upoints.Zero;
      canAfford = this.Context.UpointsManager.CanConsume(unity);
      return unity;
    }

    public void QuickRemoveProduct()
    {
      bool canAfford;
      Upoints quickRemoveCost = this.GetQuickRemoveCost(out canAfford);
      if (quickRemoveCost.IsNotPositive || !canAfford)
        return;
      LogisticsBuffer logisticsBuffer = this.Buffer.Value;
      this.Context.UpointsManager.ConsumeExactly(IdsCore.UpointsCategories.QuickRemove, quickRemoveCost);
      this.Context.AssetTransactionManager.StoreClearedProduct(this.Buffer.Value.Product.WithQuantity(logisticsBuffer.RemoveAll()));
    }

    protected override Option<IInputBufferPriorityProvider> GetVehicleInputBufferPriority(
      LogisticsBuffer buffer)
    {
      return (Option<IInputBufferPriorityProvider>) (IInputBufferPriorityProvider) this;
    }

    protected override Option<IOutputBufferPriorityProvider> GetVehicleOutputBufferPriority(
      LogisticsBuffer buffer)
    {
      return (Option<IOutputBufferPriorityProvider>) (IOutputBufferPriorityProvider) this;
    }

    public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
    {
      LogisticsBuffer valueOrNull = this.Buffer.ValueOrNull;
      return valueOrNull == null ? BufferStrategy.Ignore : valueOrNull.GetInputPriority(this.ImportPriority, pendingQuantity);
    }

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      LogisticsBuffer valueOrNull = this.Buffer.ValueOrNull;
      return valueOrNull == null ? BufferStrategy.Ignore : valueOrNull.GetOutputPriority(this.ExportPriority, request);
    }

    void IEntityWithSimUpdate.SimUpdate() => this.SimUpdateInternal();

    protected virtual void SimUpdateInternal()
    {
      Quantity quantity1 = this.SendOutputs();
      if (this.m_receivedFromPortsLastUpdate.IsPositive || quantity1.IsPositive)
        this.m_consumePowerFor = Storage.CONSUME_POWER_FOR;
      this.m_receivedFromPortsLastUpdate = Quantity.Zero;
      if (this.m_electricityConsumer.HasValue && this.m_consumePowerFor.IsPositive)
      {
        this.m_failedToConsumePower = !this.m_electricityConsumer.TryConsume(this.m_canWorkOnLowPower);
        if (!this.m_failedToConsumePower)
          this.m_consumePowerFor -= 1.Ticks();
      }
      Option<LogisticsBuffer> buffer1 = this.Buffer;
      if (buffer1.HasValue && this.IsEnabled)
      {
        LogisticsBuffer buffer2 = this.Buffer.Value;
        Percent percent = this.AlertWhenBelow;
        Percent alertWhenAbove = this.AlertWhenAbove;
        if (this.m_supplyToLowNotif.IsActive)
          percent = (percent + Storage.NOTIFICATION_HYSTERESIS).Min(Percent.Hundred);
        if (this.m_supplyToHighNotif.IsActive)
          percent = (percent - Storage.NOTIFICATION_HYSTERESIS).Max(Percent.Zero);
        if (percent > alertWhenAbove)
        {
          percent = this.AlertWhenBelow;
          alertWhenAbove = this.AlertWhenAbove;
        }
        this.m_supplyToLowNotif.NotifyIff((Proto) buffer2.Product, this.AlertWhenBelowEnabled && buffer2.PercentFull() <= percent, (IEntity) this);
        this.m_supplyToHighNotif.NotifyIff((Proto) buffer2.Product, this.AlertWhenAboveEnabled && buffer2.PercentFull() >= alertWhenAbove, (IEntity) this);
      }
      else
      {
        this.m_supplyToLowNotif.Deactivate((IEntity) this);
        this.m_supplyToHighNotif.Deactivate((IEntity) this);
      }
      this.updateRoutesNotifications();
      if (!this.IsGodModeEnabled)
        return;
      buffer1 = this.Buffer;
      if (buffer1.IsNone)
        return;
      LogisticsBuffer buffer3 = this.Buffer.Value;
      if (buffer3.ImportUntilPercent.IsPositive)
      {
        Quantity quantity2 = buffer3.StoreAsMuchAsReturnStored(Quantity.MaxValue);
        this.Context.ProductsManager.ProductCreated(buffer3.Product, quantity2, CreateReason.Cheated);
      }
      else
      {
        if (!(buffer3.ExportFromPercent < Percent.Hundred))
          return;
        Quantity quantity3 = buffer3.RemoveAsMuchAs(Quantity.MaxValue);
        this.Context.ProductsManager.ClearProduct(buffer3.Product, quantity3);
      }
    }

    protected override void OnReceivedFromPort(Quantity quantityReceived)
    {
      base.OnReceivedFromPort(quantityReceived);
      this.m_receivedFromPortsLastUpdate += quantityReceived;
    }

    protected virtual Quantity SendOutputs()
    {
      if (!this.IsEnabled)
        return Quantity.Zero;
      return this.m_storageBehindPorts.HasValue ? this.SendToStorageDirectly(this.m_storageBehindPorts.Value) : this.SendAllOutputsIfCan();
    }

    /// <summary>
    /// Tries to assign product to this storage and returns true on success.
    /// </summary>
    public bool AssignProduct(ProductProto product) => this.TryAssignProduct(product);

    public void SetImportStep(int step)
    {
      if (!this.Buffer.HasValue || !this.CanMoveSlider())
        return;
      if (this.IsLogisticsInputDisabled)
        this.SetLogisticsInputDisabled(false);
      this.Buffer.Value.SetImportStep(step);
      this.updateRoutesNotifications();
    }

    public void SetImportPercent(Percent percent)
    {
      if (!this.Buffer.HasValue || !this.CanMoveSlider())
        return;
      if (this.IsLogisticsInputDisabled)
        this.SetLogisticsInputDisabled(false);
      this.Buffer.Value.SetImportStep(percent.Apply(10));
      this.updateRoutesNotifications();
    }

    public void SetExportStep(int step)
    {
      if (!this.Buffer.HasValue || !this.CanMoveSlider())
        return;
      if (this.IsLogisticsOutputDisabled)
        this.SetLogisticsOutputDisabled(false);
      this.Buffer.Value.SetExportStep(step);
      this.updateRoutesNotifications();
    }

    public void SetExportPercent(Percent percent)
    {
      if (!this.Buffer.HasValue || !this.CanMoveSlider())
        return;
      if (this.IsLogisticsOutputDisabled)
        this.SetLogisticsOutputDisabled(false);
      this.Buffer.Value.SetExportStep(percent.Apply(10));
      this.updateRoutesNotifications();
    }

    private void updateRoutesNotifications()
    {
      ref EntityNotificator local1 = ref this.m_invalidImportRouteNotif;
      Option<LogisticsBuffer> buffer;
      int num1;
      if (this.m_assignedOutputEntities.Count > 0)
      {
        buffer = this.Buffer;
        num1 = !buffer.HasValue || this.Buffer.Value.ExportFromPercent.IsNearHundred ? (this.IsLogisticsInputDisabled ? 1 : 0) : 1;
      }
      else
        num1 = 0;
      local1.NotifyIff(num1 != 0, (IEntity) this);
      ref EntityNotificator local2 = ref this.m_invalidExportRouteNotif;
      int num2;
      if (this.m_assignedInputEntities.Count > 0)
      {
        buffer = this.Buffer;
        num2 = !buffer.HasValue || !this.Buffer.Value.ImportUntilPercent.IsPositive ? (this.IsLogisticsOutputDisabled ? 1 : 0) : 1;
      }
      else
        num2 = 0;
      local2.NotifyIff(num2 != 0, (IEntity) this);
    }

    public Quantity AddAsMuchAs(Quantity quantity)
    {
      return !this.IsEnabled || this.Buffer.IsNone ? Quantity.Zero : quantity - this.Buffer.Value.StoreAsMuchAs(quantity);
    }

    public Quantity RemoveAsMuchAs(Quantity quantity)
    {
      return !this.IsEnabled || this.Buffer.IsNone ? Quantity.Zero : this.Buffer.Value.RemoveAsMuchAs(quantity);
    }

    internal void SetInitialProduct(ProductProto productProto, Percent percentFull)
    {
      this.SetInitialProduct(productProto, this.Prototype.Capacity.ScaledBy(percentFull.Clamp0To100()));
    }

    internal void SetInitialProduct(ProductProto productProto, Quantity quantity)
    {
      Assert.That<bool>(this.IsProductSupported(productProto)).IsTrue<ProductProto>("Product '{0}' not storable.", productProto);
      if (this.Buffer.IsNone && !this.TryAssignProduct(productProto))
      {
        Log.Error(string.Format("Failed to assign initial product {0}.", (object) productProto));
      }
      else
      {
        Assert.That<Quantity>(this.Buffer.Value.StoreAsMuchAs(quantity)).IsZero();
        this.Context.ProductsManager.ProductCreated(productProto, quantity, CreateReason.Cheated);
      }
    }

    public void SetEnforceAssignedVehicles(bool isEnforceOn)
    {
      this.m_isEnforcingCustomVehicles = isEnforceOn;
    }

    protected override void OnDestroy()
    {
      if (this.m_assignedTrucks.HasValue)
      {
        foreach (Vehicle immutable in this.m_assignedTrucks.Value.All.ToImmutableArray<Truck>())
          this.UnassignVehicle(immutable, true);
      }
      this.m_assignedInputEntities.ForEachAndClear((Action<IEntityAssignedAsInput>) (x => x.UnassignStaticOutputEntity((IEntityAssignedAsOutput) this)));
      this.m_assignedOutputEntities.ForEachAndClear((Action<IEntityAssignedAsOutput>) (x => x.UnassignStaticInputEntity((IEntityAssignedAsInput) this)));
      base.OnDestroy();
    }

    protected override void OnPortConnectionChanged(IoPort ourPort)
    {
      base.OnPortConnectionChanged(ourPort);
      this.rescanOutputPorts(false);
    }

    internal void Cheat_NewProduct(ProductProto productProto, Quantity? quantity = null)
    {
      Assert.That<bool>(this.IsProductSupported(productProto)).IsTrue<ProductProto>("Product '{0}' not storable.", productProto);
      if (this.Buffer.HasValue && (Proto) this.Buffer.Value.Product != (Proto) productProto)
      {
        this.Cheat_ForceClear();
        this.ClearAssignedProduct();
      }
      if (this.Buffer.IsNone && !this.TryAssignProduct(productProto))
      {
        Log.Warning("Failed to cheat product to storage.");
      }
      else
      {
        Assert.That<ProductProto>(this.Buffer.Value.Product).IsEqualTo<ProductProto>(productProto);
        Quantity quantity1 = quantity ?? this.Capacity;
        Quantity quantity2 = this.Buffer.Value.StoreAsMuchAs(quantity1);
        Assert.That<bool>(!quantity.HasValue || quantity2.IsZero).IsTrue<Quantity>("Not all quantity stored, remainder: {0}", quantity2);
        this.Context.ProductsManager.ProductCreated(this.Buffer.Value.Product, quantity1 - quantity2, CreateReason.Cheated);
      }
    }

    internal void Cheat_SetGodMode(bool isEnabled) => this.IsGodModeEnabled = isEnabled;

    internal void Cheat_ForceClear()
    {
      if (this.Buffer.IsNone)
        return;
      this.Buffer.Value.SetCleaningMode(false);
      this.Context.ProductsManager.ProductDestroyed(this.Buffer.Value.Product, this.Buffer.Value.RemoveAll(), DestroyReason.Cleared);
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public bool CanBeAssignedWithInput(IEntityAssignedAsInput entity)
    {
      if (entity == this || this.m_assignedInputEntities.Contains(entity))
        return false;
      switch (entity)
      {
        case MineTower _:
          ProductType? productType1 = this.Prototype.ProductType;
          if (!productType1.HasValue)
            return true;
          productType1 = this.Prototype.ProductType;
          return productType1.Value == LooseProductProto.ProductType;
        case ForestryTower _:
          ProductType? productType2 = this.Prototype.ProductType;
          if (!productType2.HasValue)
            return true;
          productType2 = this.Prototype.ProductType;
          return productType2.Value == CountableProductProto.ProductType;
        case Storage storage:
          Option<LogisticsBuffer> buffer = this.Buffer;
          if (buffer.HasValue)
          {
            buffer = storage.Buffer;
            if (buffer.HasValue)
              return (Proto) this.Buffer.Value.Product == (Proto) storage.Buffer.Value.Product;
          }
          return storage.Prototype.StorableProducts.Overlaps<ProductProto>((IEnumerable<ProductProto>) this.Prototype.StorableProducts);
        default:
          return false;
      }
    }

    public bool CanBeAssignedWithOutput(IEntityAssignedAsOutput entity)
    {
      if (entity == this || this.m_assignedOutputEntities.Contains(entity))
        return false;
      switch (entity)
      {
        case MineTower _:
          return true;
        case ForestryTower _:
          ProductType? productType = this.Prototype.ProductType;
          if (!productType.HasValue)
            return true;
          productType = this.Prototype.ProductType;
          return productType.Value == CountableProductProto.ProductType;
        case Storage storage:
          Option<LogisticsBuffer> buffer = this.Buffer;
          if (buffer.HasValue)
          {
            buffer = storage.Buffer;
            if (buffer.HasValue)
              return (Proto) this.Buffer.Value.Product == (Proto) storage.Buffer.Value.Product;
          }
          return storage.Prototype.StorableProducts.Overlaps<ProductProto>((IEnumerable<ProductProto>) this.Prototype.StorableProducts);
        default:
          return false;
      }
    }

    void IEntityAssignedAsOutput.AssignStaticInputEntity(IEntityAssignedAsInput entity)
    {
      if (!this.CanBeAssignedWithInput(entity))
        return;
      this.m_assignedInputEntities.Add(entity);
      this.updateRoutesNotifications();
    }

    void IEntityAssignedAsInput.AssignStaticOutputEntity(IEntityAssignedAsOutput entity)
    {
      if (!this.CanBeAssignedWithOutput(entity))
        return;
      this.m_assignedOutputEntities.Add(entity);
      this.updateRoutesNotifications();
    }

    void IEntityAssignedAsOutput.UnassignStaticInputEntity(IEntityAssignedAsInput entity)
    {
      this.m_assignedInputEntities.Remove(entity);
      this.updateRoutesNotifications();
    }

    void IEntityAssignedAsInput.UnassignStaticOutputEntity(IEntityAssignedAsOutput entity)
    {
      this.m_assignedOutputEntities.Remove(entity);
      this.updateRoutesNotifications();
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
          return this.Buffer.Value.ImportUntilPercent.IsPositive;
        case "ExportPrio":
          return this.Buffer.Value.ExportFromPercent < Percent.Hundred;
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

    public bool CanVehicleBeAssigned(DynamicEntityProto vehicleProto)
    {
      if (!(vehicleProto is TruckProto truckProto))
        return false;
      if (truckProto.ProductType.HasValue)
      {
        ProductType? productType1 = this.Prototype.ProductType;
        if (productType1.HasValue)
        {
          productType1 = truckProto.ProductType;
          ProductType? productType2 = this.Prototype.ProductType;
          if (productType1.HasValue != productType2.HasValue)
            return false;
          return !productType1.HasValue || productType1.GetValueOrDefault() == productType2.GetValueOrDefault();
        }
      }
      return true;
    }

    public void AssignVehicle(Vehicle vehicle, bool doNotCancelJobs = false)
    {
      if (!(vehicle is Truck vehicle1))
      {
        Log.Error(string.Format("Trying to unassign invalid vehicle '{0}'.", (object) vehicle.GetType()));
      }
      else
      {
        if (this.m_assignedTrucks.IsNone)
          this.m_assignedTrucks = (Option<AssignedVehicles<Truck, TruckProto>>) new AssignedVehicles<Truck, TruckProto>((IEntityAssignedWithVehicles) this);
        this.m_assignedTrucks.Value.AssignVehicle(vehicle1, doNotCancelJobs);
      }
    }

    public void UnassignVehicle(Vehicle vehicle, bool cancelJobs = true)
    {
      if (!(vehicle is Truck vehicle1))
      {
        Log.Error(string.Format("Trying to unassign invalid vehicle '{0}'.", (object) vehicle.GetType()));
      }
      else
      {
        if (this.m_assignedTrucks.IsNone)
          return;
        this.m_assignedTrucks.Value.UnassignVehicle(vehicle1, cancelJobs);
      }
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.SetStorageStoredProduct(this.StoredProduct);
      if (this.Buffer.HasValue)
      {
        data.SetStorageImportUntilPercent(this.ImportUntilPercent);
        data.SetStorageExportFromPercent(this.ExportFromPercent);
      }
      data.SetStorageImportPriority(this.ImportPriority);
      data.SetStorageExportPriority(this.ExportPriority);
      if (this.AlertWhenAboveEnabled)
        data.SetAlertWhenAboveEnabled(this.AlertWhenAboveEnabled);
      if (this.AlertWhenAbove != Percent.Hundred)
        data.SetAlertWhenAbove(this.AlertWhenAbove);
      if (this.AlertWhenBelowEnabled)
        data.SetAlertWhenBelowEnabled(this.AlertWhenBelowEnabled);
      if (this.AlertWhenBelow != Percent.Zero)
        data.SetAlertWhenBelow(this.AlertWhenBelow);
      if (!this.m_isEnforcingCustomVehicles)
        return;
      data.SetIsEnforcingCustomVehicles(this.m_isEnforcingCustomVehicles);
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      Option<ProductProto> storageStoredProduct = data.GetStorageStoredProduct();
      if (storageStoredProduct.HasValue)
      {
        if (storageStoredProduct != this.StoredProduct)
          this.TryAssignProduct(storageStoredProduct.Value);
      }
      else if (this.Buffer.HasValue && this.Buffer.Value.IsEmpty)
        this.ClearAssignedProduct();
      Percent? nullable1;
      if (this.Buffer.HasValue)
      {
        Percent? importUntilPercent1 = data.GetStorageImportUntilPercent();
        if (importUntilPercent1.HasValue)
        {
          nullable1 = importUntilPercent1;
          Percent importUntilPercent2 = this.ImportUntilPercent;
          if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() != importUntilPercent2 ? 1 : 0) : 1) != 0)
            this.SetImportPercent(importUntilPercent1.Value);
        }
        Percent? exportFromPercent1 = data.GetStorageExportFromPercent();
        if (exportFromPercent1.HasValue)
        {
          nullable1 = exportFromPercent1;
          Percent exportFromPercent2 = this.ExportFromPercent;
          if ((nullable1.HasValue ? (nullable1.GetValueOrDefault() != exportFromPercent2 ? 1 : 0) : 1) != 0)
            this.SetExportPercent(exportFromPercent1.Value);
        }
      }
      int? nullable2 = data.GetStorageImportPriority();
      this.ImportPriority = nullable2 ?? this.ImportPriority;
      nullable2 = data.GetStorageExportPriority();
      this.ExportPriority = nullable2 ?? this.ExportPriority;
      bool? nullable3 = data.GetAlertWhenAboveEnabled();
      this.AlertWhenAboveEnabled = nullable3.GetValueOrDefault();
      nullable1 = data.GetAlertWhenAbove();
      this.AlertWhenAbove = nullable1 ?? Percent.Hundred;
      nullable3 = data.GetAlertWhenBelowEnabled();
      this.AlertWhenBelowEnabled = nullable3.GetValueOrDefault();
      nullable1 = data.GetAlertWhenBelow();
      this.AlertWhenBelow = nullable1 ?? Percent.Zero;
      nullable3 = data.GetIsEnforcingCustomVehicles();
      this.m_isEnforcingCustomVehicles = nullable3.GetValueOrDefault();
      this.rescanOutputPorts(false);
    }

    public static void Serialize(Storage value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Storage>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Storage.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Percent.Serialize(this.AlertWhenAbove, writer);
      writer.WriteBool(this.AlertWhenAboveEnabled);
      Percent.Serialize(this.AlertWhenBelow, writer);
      writer.WriteBool(this.AlertWhenBelowEnabled);
      writer.WriteBool(this.AllowNonAssignedOutput);
      writer.WriteInt(this.ExportPriority);
      writer.WriteInt(this.ImportPriority);
      writer.WriteBool(this.IsGodModeEnabled);
      Set<IEntityAssignedAsInput>.Serialize(this.m_assignedInputEntities, writer);
      Set<IEntityAssignedAsOutput>.Serialize(this.m_assignedOutputEntities, writer);
      Option<AssignedVehicles<Truck, TruckProto>>.Serialize(this.m_assignedTrucks, writer);
      Duration.Serialize(this.m_consumePowerFor, writer);
      Option<IElectricityConsumer>.Serialize(this.m_electricityConsumer, writer);
      writer.WriteBool(this.m_failedToConsumePower);
      EntityNotificator.Serialize(this.m_invalidExportRouteNotif, writer);
      EntityNotificator.Serialize(this.m_invalidImportRouteNotif, writer);
      writer.WriteBool(this.m_isEnforcingCustomVehicles);
      writer.WriteGeneric<StorageProto>(this.m_proto);
      Quantity.Serialize(this.m_receivedFromPortsLastUpdate, writer);
      EntityNotificatorWithProtoParam.Serialize(this.m_supplyToHighNotif, writer);
      EntityNotificatorWithProtoParam.Serialize(this.m_supplyToLowNotif, writer);
      writer.WriteNullableStruct<bool>(this.m_wasLogisticsInputDisabledBefore);
      writer.WriteNullableStruct<bool>(this.m_wasLogisticsOutputDisabledBefore);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static Storage Deserialize(BlobReader reader)
    {
      Storage storage;
      if (reader.TryStartClassDeserialization<Storage>(out storage))
        reader.EnqueueDataDeserialization((object) storage, Storage.s_deserializeDataDelayedAction);
      return storage;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AlertWhenAbove = Percent.Deserialize(reader);
      this.AlertWhenAboveEnabled = reader.ReadBool();
      this.AlertWhenBelow = Percent.Deserialize(reader);
      this.AlertWhenBelowEnabled = reader.ReadBool();
      this.AllowNonAssignedOutput = reader.ReadBool();
      this.ExportPriority = reader.ReadInt();
      this.ImportPriority = reader.ReadInt();
      this.IsGodModeEnabled = reader.ReadBool();
      reader.SetField<Storage>(this, "m_assignedInputEntities", (object) Set<IEntityAssignedAsInput>.Deserialize(reader));
      reader.SetField<Storage>(this, "m_assignedOutputEntities", (object) Set<IEntityAssignedAsOutput>.Deserialize(reader));
      this.m_assignedTrucks = Option<AssignedVehicles<Truck, TruckProto>>.Deserialize(reader);
      this.m_consumePowerFor = Duration.Deserialize(reader);
      this.m_electricityConsumer = Option<IElectricityConsumer>.Deserialize(reader);
      this.m_failedToConsumePower = reader.ReadBool();
      this.m_invalidExportRouteNotif = EntityNotificator.Deserialize(reader);
      this.m_invalidImportRouteNotif = EntityNotificator.Deserialize(reader);
      this.m_isEnforcingCustomVehicles = reader.ReadBool();
      this.m_proto = reader.ReadGenericAs<StorageProto>();
      this.m_receivedFromPortsLastUpdate = Quantity.Deserialize(reader);
      this.m_supplyToHighNotif = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.m_supplyToLowNotif = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.m_wasLogisticsInputDisabledBefore = reader.ReadNullableStruct<bool>();
      this.m_wasLogisticsOutputDisabledBefore = reader.ReadNullableStruct<bool>();
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      reader.RegisterInitAfterLoad<Storage>(this, "initSelf", InitPriority.Normal);
    }

    static Storage()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Storage.CONSUME_POWER_FOR = 1.Seconds();
      Storage.NOTIFICATION_HYSTERESIS = Percent.FromPercentVal(5);
      Storage.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Storage.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private class StorageBuffer : LogisticsBuffer
    {
      private readonly ProductStats m_productStats;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public StorageBuffer(
        Quantity bufferCapacity,
        ProductProto product,
        IProductsManager productsManager,
        IEntity entity,
        bool usePartialTrucksForHighPriorities = false)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(bufferCapacity, product, usePartialTrucksForHighPriorities);
        this.m_productStats = productsManager.GetStatsFor(product);
        this.m_productStats.ProductsManager.AssetManager.AddGlobalOutput((IProductBuffer) this, 10, entity.SomeOption<IEntity>());
        this.m_productStats.ProductsManager.AssetManager.AddGlobalInput((IProductBuffer) this, 10, entity.SomeOption<IEntity>());
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        this.m_productStats.StoredAvailableQuantityChange(diff);
      }

      public override void Destroy()
      {
        this.m_productStats.ProductsManager.AssetManager.RemoveGlobalOutput((IProductBuffer) this);
        this.m_productStats.ProductsManager.AssetManager.RemoveGlobalInput((IProductBuffer) this);
        base.Destroy();
      }

      public static void Serialize(Storage.StorageBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Storage.StorageBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Storage.StorageBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        ProductStats.Serialize(this.m_productStats, writer);
      }

      public static Storage.StorageBuffer Deserialize(BlobReader reader)
      {
        Storage.StorageBuffer storageBuffer;
        if (reader.TryStartClassDeserialization<Storage.StorageBuffer>(out storageBuffer))
          reader.EnqueueDataDeserialization((object) storageBuffer, Storage.StorageBuffer.s_deserializeDataDelayedAction);
        return storageBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<Storage.StorageBuffer>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
      }

      static StorageBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Storage.StorageBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        Storage.StorageBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
