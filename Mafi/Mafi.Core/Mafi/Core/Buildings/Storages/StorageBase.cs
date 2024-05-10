// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.StorageBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Buildings.Storages
{
  public abstract class StorageBase : 
    LayoutEntity,
    IStaticEntityWithQueue,
    IEntityWithSimpleLogisticsControl,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity
  {
    private StorageBaseProto m_proto;
    private readonly VehicleQueue<Vehicle, IStaticEntity> m_vehicleQueue;
    /// <summary>
    /// Determines the ordering of output ports when sending outputs. This defines index of the first port, others
    /// follow in ascending order.
    /// </summary>
    private int m_lastUsedPortIndex;
    private readonly ISimLoopEvents m_simLoopEvents;
    protected readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    /// <summary>
    /// Whether before we deconstruct this entity we should request logistics to clean its cargo.
    /// NOTE: This will only work if StorageBuffer strategy is used otherwise needs to mimic the strategies.
    /// </summary>
    private readonly bool m_performCleaningBeforeDeconstruction;
    private Quantity m_transferLimitUsed;
    private SimStep m_nextTransferResetSimStep;

    [DoNotSave(0, null)]
    public StorageBaseProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
        this.setBufferCapacityTo(value.Capacity);
      }
    }

    public Option<ProductProto> StoredProduct
    {
      get
      {
        return !this.Buffer.HasValue ? Option<ProductProto>.None : (Option<ProductProto>) this.Buffer.Value.Product;
      }
    }

    public Quantity Capacity => !this.Buffer.HasValue ? Quantity.Zero : this.Buffer.Value.Capacity;

    public Quantity CurrentQuantity
    {
      get => !this.Buffer.HasValue ? Quantity.Zero : this.Buffer.Value.Quantity;
    }

    public Percent PercentFull
    {
      get
      {
        if (this.Buffer.IsNone || this.CurrentQuantity.IsNotPositive)
          return Percent.Zero;
        return !(this.CurrentQuantity > this.Capacity) ? Percent.FromRatio(this.CurrentQuantity.Value, this.Capacity.Value) : Percent.Hundred;
      }
    }

    protected virtual bool IsOperational => true;

    /// <summary>
    /// Reports the entire capacity of this buffer as storage capacity as long as
    /// this entity is constructed.
    /// If this is set to false, implementors need to report capacity on their own.
    /// </summary>
    protected virtual bool ReportFullStorageCapacityInStats => true;

    public bool IsEmpty => this.Buffer.IsNone || this.Buffer.Value.IsEmpty;

    public bool IsNotEmpty => !this.IsEmpty;

    public bool IsFull => this.Buffer.HasValue && this.Buffer.Value.IsFull;

    public bool IsNotFull => !this.IsFull;

    public virtual LogisticsControl LogisticsInputControl => LogisticsControl.NotAvailable;

    public virtual LogisticsControl LogisticsOutputControl => LogisticsControl.NotAvailable;

    public bool IsLogisticsInputDisabled { get; private set; }

    public bool IsLogisticsOutputDisabled { get; private set; }

    protected virtual bool IoDisabled => false;

    /// <summary>None when the storage has no product assigned.</summary>
    protected Option<LogisticsBuffer> Buffer { get; private set; }

    protected StorageBase(
      EntityId id,
      StorageBaseProto storageProto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      bool performCleaningBeforeDeconstruction = false)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_lastUsedPortIndex = -1;
      this.m_transferLimitUsed = Quantity.Zero;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) storageProto, transform, context);
      this.m_proto = storageProto.CheckNotNull<StorageBaseProto>();
      this.m_simLoopEvents = simLoopEvents;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_performCleaningBeforeDeconstruction = performCleaningBeforeDeconstruction;
      this.m_vehicleQueue = new VehicleQueue<Vehicle, IStaticEntity>((IStaticEntity) this);
      this.m_vehicleQueue.Enable();
    }

    public abstract bool IsProductSupported(ProductProto product);

    public void SetLogisticsInputDisabled(bool isDisabled)
    {
      if (this.IsLogisticsInputDisabled == isDisabled)
        return;
      this.IsLogisticsInputDisabled = isDisabled;
      this.UpdateLogisticsInputReg();
    }

    public void SetLogisticsOutputDisabled(bool isDisabled)
    {
      if (this.IsLogisticsOutputDisabled == isDisabled || isDisabled && (this.Buffer.HasValue && this.Buffer.Value.CleaningMode || !this.CanMoveSlider()))
        return;
      this.IsLogisticsOutputDisabled = isDisabled;
      this.UpdateLogisticsOutputReg();
    }

    protected void UpdateLogisticsInputReg()
    {
      if (this.Buffer.IsNone)
        return;
      LogisticsBuffer buffer = this.Buffer.Value;
      if (this.IsLogisticsInputDisabled || !this.IsOperational)
      {
        this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer((IProductBuffer) buffer);
        buffer.SetImportStep(Percent.Zero);
      }
      else
      {
        Option<IInputBufferPriorityProvider> inputBufferPriority = this.GetVehicleInputBufferPriority(buffer);
        if (!inputBufferPriority.HasValue)
          return;
        this.m_vehicleBuffersRegistry.TryRegisterInputBuffer((IStaticEntity) this, (IProductBuffer) buffer, inputBufferPriority.Value);
      }
    }

    protected virtual void UpdateLogisticsOutputReg()
    {
      if (this.Buffer.IsNone)
        return;
      LogisticsBuffer buffer = this.Buffer.Value;
      if (this.IsLogisticsOutputDisabled || !this.IsOperational)
      {
        this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) buffer);
        buffer.SetExportStep(Percent.Hundred);
      }
      else
      {
        Option<IOutputBufferPriorityProvider> outputBufferPriority = this.GetVehicleOutputBufferPriority(buffer);
        if (!outputBufferPriority.HasValue)
          return;
        this.m_vehicleBuffersRegistry.TryRegisterOutputBuffer((IStaticEntity) this, (IProductBuffer) buffer, outputBufferPriority.Value);
      }
    }

    /// <summary>
    /// Tries to assign product to this storage and returns true on success.
    /// </summary>
    protected bool TryAssignProduct(ProductProto product)
    {
      if (!this.IsProductSupported(product))
        return false;
      Percent? nullable1 = new Percent?();
      Percent? nullable2 = new Percent?();
      if (this.Buffer.HasValue)
      {
        LogisticsBuffer buffer = this.Buffer.Value;
        if (buffer.IsNotEmpty())
          return (Proto) buffer.Product == (Proto) product;
        nullable1 = new Percent?(buffer.ImportUntilPercent);
        nullable2 = new Percent?(buffer.ExportFromPercent);
        this.ClearAssignedProduct();
      }
      this.Buffer = (Option<LogisticsBuffer>) this.CreateNewBuffer(this.Prototype.Capacity, product);
      if (nullable1.HasValue)
        this.Buffer.Value.SetImportStep(nullable1.Value);
      if (nullable2.HasValue)
        this.Buffer.Value.SetExportStep(nullable2.Value);
      if (this.IsConstructedIgnoreUpgrade && this.ReportFullStorageCapacityInStats)
        this.Context.ProductsManager.ReportStorageCapacityChange(this.Buffer.Value.Product, this.Buffer.Value.Capacity);
      this.UpdateLogisticsInputReg();
      this.UpdateLogisticsOutputReg();
      return true;
    }

    private void setBufferCapacityTo(Quantity newCapacity)
    {
      if (this.Buffer.IsNone)
        return;
      LogisticsBuffer logisticsBuffer = this.Buffer.Value;
      Quantity quantity = (newCapacity - logisticsBuffer.Capacity).Max(Quantity.Zero);
      logisticsBuffer.ForceNewCapacityTo(newCapacity);
      if (!this.IsConstructedIgnoreUpgrade)
        return;
      this.Context.ProductsManager.ReportStorageCapacityChange(logisticsBuffer.Product, quantity);
    }

    protected abstract LogisticsBuffer CreateNewBuffer(Quantity capacity, ProductProto product);

    protected abstract Option<IInputBufferPriorityProvider> GetVehicleInputBufferPriority(
      LogisticsBuffer buffer);

    protected abstract Option<IOutputBufferPriorityProvider> GetVehicleOutputBufferPriority(
      LogisticsBuffer buffer);

    public void ToggleClearProduct()
    {
      if (this.Buffer.IsNone)
        return;
      if (this.Buffer.Value.CleaningMode)
      {
        if (!this.CanMoveSlider())
          return;
        this.Buffer.Value.SetCleaningMode(false);
      }
      else
        this.ClearAssignedProduct();
    }

    public bool CanMoveSlider()
    {
      return this.ConstructionState != ConstructionState.PendingDeconstruction && this.ConstructionState != ConstructionState.InDeconstruction;
    }

    /// <summary>
    /// Clears currently assigned products if the storage is empty. Otherwise, enables clearing mode. Does nothing if
    /// storage product is not assigned.
    /// </summary>
    public virtual void ClearAssignedProduct()
    {
      if (this.Buffer.IsNone)
        return;
      LogisticsBuffer buffer = this.Buffer.Value;
      if (buffer.Quantity != Quantity.Zero)
      {
        buffer.SetCleaningMode(true);
        this.SetLogisticsOutputDisabled(false);
      }
      else
      {
        this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer((IProductBuffer) buffer);
        this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) buffer);
        this.destroyBuffer((ProductBuffer) buffer);
        this.Buffer = (Option<LogisticsBuffer>) Option.None;
      }
    }

    private void destroyBuffer(ProductBuffer buffer)
    {
      if (this.IsConstructedIgnoreUpgrade && this.ReportFullStorageCapacityInStats)
        this.Context.ProductsManager.ReportStorageCapacityChange(buffer.Product, -buffer.Capacity);
      this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) buffer);
      this.OnBufferDestroyed(buffer);
    }

    protected virtual void OnBufferDestroyed(ProductBuffer buffer)
    {
    }

    public override void SetConstructionState(ConstructionState state)
    {
      bool constructedIgnoreUpgrade = this.IsConstructedIgnoreUpgrade;
      base.SetConstructionState(state);
      if (constructedIgnoreUpgrade == this.IsConstructedIgnoreUpgrade || !this.Buffer.HasValue || !this.ReportFullStorageCapacityInStats)
        return;
      LogisticsBuffer logisticsBuffer = this.Buffer.Value;
      this.Context.ProductsManager.ReportStorageCapacityChange(logisticsBuffer.Product, this.IsConstructedIgnoreUpgrade ? logisticsBuffer.Capacity : -logisticsBuffer.Capacity);
    }

    public override void StartDeconstructionIfCan()
    {
      if (!this.CanMoveFromPendingDeconstruction())
      {
        if (this.ConstructionState == ConstructionState.PendingDeconstruction)
          return;
        this.SetLogisticsOutputDisabled(false);
        this.Buffer.Value.SetCleaningMode(true);
        this.Context.ConstructionManager.MarkForPendingDeconstruction((IStaticEntity) this);
      }
      else
        base.StartDeconstructionIfCan();
    }

    public override bool CanMoveFromPendingDeconstruction()
    {
      return !this.m_performCleaningBeforeDeconstruction || !this.Buffer.HasValue || !this.Buffer.Value.Quantity.IsPositive;
    }

    protected override void OnDeconstructionAborted()
    {
      if (!this.m_performCleaningBeforeDeconstruction || !this.Buffer.HasValue)
        return;
      this.Buffer.Value.SetCleaningMode(false);
    }

    /// <summary>
    /// Sends contents of output buffers to a storage directly. Used when storages are daisy chained.
    /// </summary>
    protected Quantity SendToStorageDirectly(Storage storage)
    {
      if (this.IoDisabled || !this.Buffer.HasValue || !this.Buffer.Value.Quantity.IsPositive)
        return Quantity.Zero;
      LogisticsBuffer logisticsBuffer = this.Buffer.Value;
      Quantity maxQuantity = logisticsBuffer.Quantity - storage.receiveAsMuchAsBetweenStorages(logisticsBuffer.ProductQuantity);
      logisticsBuffer.RemoveAsMuchAs_DoNotReport(maxQuantity);
      if (logisticsBuffer.CleaningMode && logisticsBuffer.Quantity == Quantity.Zero)
        this.ClearAssignedProduct();
      return maxQuantity;
    }

    /// <summary>Sends contents of output buffers to output ports.</summary>
    protected Quantity SendAllOutputsIfCan()
    {
      if (this.IoDisabled || !this.Buffer.HasValue || !this.Buffer.Value.Quantity.IsPositive)
        return Quantity.Zero;
      LogisticsBuffer logisticsBuffer = this.Buffer.Value;
      ImmutableArray<IoPortData> connectedOutputPorts = this.ConnectedOutputPorts;
      if (connectedOutputPorts.Length == 0)
        return Quantity.Zero;
      Quantity zero = Quantity.Zero;
      for (int index = 0; index < connectedOutputPorts.Length && logisticsBuffer.Quantity.IsPositive; ++index)
      {
        this.m_lastUsedPortIndex = (this.m_lastUsedPortIndex + 1) % this.ConnectedOutputPorts.Length;
        IoPortData ioPortData = connectedOutputPorts[this.m_lastUsedPortIndex];
        Quantity maxQuantity = logisticsBuffer.Quantity - ioPortData.SendAsMuchAs(new ProductQuantity(logisticsBuffer.Product, logisticsBuffer.Quantity));
        zero += maxQuantity;
        logisticsBuffer.RemoveAsMuchAs(maxQuantity);
      }
      if (logisticsBuffer.CleaningMode && logisticsBuffer.Quantity == Quantity.Zero)
        this.ClearAssignedProduct();
      return zero;
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      return !this.canReceive(pq) ? pq.Quantity : this.receiveAsMuchAsLimitTransfer(pq.Quantity);
    }

    /// <summary>
    /// Performance optimization to transfer between storages directly and avoid ports.
    /// </summary>
    private Quantity receiveAsMuchAsBetweenStorages(ProductQuantity pq)
    {
      return !this.canReceive(pq) ? pq.Quantity : this.receiveAsMuchAsLimitTransfer(pq.Quantity, true);
    }

    private Quantity receiveAsMuchAsLimitTransfer(Quantity quantity, bool doNotReport = false)
    {
      if (this.m_nextTransferResetSimStep <= this.m_simLoopEvents.CurrentStep)
        this.m_transferLimitUsed = Quantity.Zero;
      Quantity quantity1 = quantity.Min(this.Prototype.TransferLimit - this.m_transferLimitUsed);
      if (quantity1.IsNotPositive)
        return quantity;
      Quantity quantityReceived = quantity1 - (doNotReport ? this.Buffer.Value.StoreAsMuchAs_DoNotReport(quantity1) : this.Buffer.Value.StoreAsMuchAs(quantity1));
      this.m_transferLimitUsed += quantityReceived;
      if (this.m_nextTransferResetSimStep <= this.m_simLoopEvents.CurrentStep && this.m_transferLimitUsed.IsPositive)
        this.m_nextTransferResetSimStep = this.m_simLoopEvents.CurrentStep + new SimStep(this.Prototype.TransferLimitDuration.Ticks);
      this.OnReceivedFromPort(quantityReceived);
      return quantity - quantityReceived;
    }

    protected virtual void OnReceivedFromPort(Quantity quantityReceived)
    {
    }

    private bool canReceive(ProductQuantity pq)
    {
      if (this.IoDisabled || this.IsNotEnabled)
        return false;
      if (this.Buffer.IsNone)
      {
        if (!this.IsProductSupported(pq.Product))
          return false;
        this.TryAssignProduct(pq.Product);
        return true;
      }
      return !this.Buffer.Value.CleaningMode && (Proto) this.Buffer.Value.Product == (Proto) pq.Product;
    }

    public bool TryGetVehicleQueueFor(
      Vehicle vehicle,
      out VehicleQueue<Vehicle, IStaticEntity> queue)
    {
      if (this.IsNotEnabled)
      {
        queue = (VehicleQueue<Vehicle, IStaticEntity>) null;
        return false;
      }
      queue = this.m_vehicleQueue;
      return true;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (this.IsEnabled)
        this.m_vehicleQueue.Enable();
      else
        this.m_vehicleQueue.CancelJobsAndDisable();
    }

    protected override void OnDestroy()
    {
      if (this.Buffer.HasValue)
      {
        this.destroyBuffer((ProductBuffer) this.Buffer.Value);
        this.Buffer = (Option<LogisticsBuffer>) Option.None;
      }
      this.m_vehicleQueue.CancelJobsAndDisable();
      base.OnDestroy();
    }

    public override string ToString()
    {
      if (!this.Buffer.HasValue)
        return base.ToString() + " (empty)";
      LogisticsBuffer logisticsBuffer = this.Buffer.Value;
      return string.Format("{0} (has {1} of {2})", (object) base.ToString(), (object) logisticsBuffer.Quantity, (object) logisticsBuffer.Product);
    }

    internal void Cheat_ForceNewCapacityTo(Quantity newCapacity)
    {
      this.Buffer.Value.ForceNewCapacityTo(newCapacity);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<LogisticsBuffer>.Serialize(this.Buffer, writer);
      writer.WriteBool(this.IsLogisticsInputDisabled);
      writer.WriteBool(this.IsLogisticsOutputDisabled);
      writer.WriteInt(this.m_lastUsedPortIndex);
      SimStep.Serialize(this.m_nextTransferResetSimStep, writer);
      writer.WriteBool(this.m_performCleaningBeforeDeconstruction);
      writer.WriteGeneric<StorageBaseProto>(this.m_proto);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      Quantity.Serialize(this.m_transferLimitUsed, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      VehicleQueue<Vehicle, IStaticEntity>.Serialize(this.m_vehicleQueue, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Buffer = Option<LogisticsBuffer>.Deserialize(reader);
      this.IsLogisticsInputDisabled = reader.ReadBool();
      this.IsLogisticsOutputDisabled = reader.ReadBool();
      this.m_lastUsedPortIndex = reader.ReadInt();
      this.m_nextTransferResetSimStep = SimStep.Deserialize(reader);
      reader.SetField<StorageBase>(this, "m_performCleaningBeforeDeconstruction", (object) reader.ReadBool());
      this.m_proto = reader.ReadGenericAs<StorageBaseProto>();
      reader.SetField<StorageBase>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_transferLimitUsed = Quantity.Deserialize(reader);
      reader.SetField<StorageBase>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.SetField<StorageBase>(this, "m_vehicleQueue", (object) VehicleQueue<Vehicle, IStaticEntity>.Deserialize(reader));
    }
  }
}
