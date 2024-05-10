// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Storages.NuclearWaste.NuclearWasteStorage
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Environment;
using Mafi.Core.Population;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Storages.NuclearWaste
{
  [GenerateSerializer(false, null, 0)]
  public class NuclearWasteStorage : 
    Storage,
    IEntityWithEmission,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithWorkers,
    IEntityWithGeneralPriority
  {
    private NuclearWasteStorageProto m_proto;
    private readonly ISimLoopEvents m_simLoopEvents;
    private new readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private bool m_isOperational;
    private readonly IRadiationManager m_radiationManager;
    private Option<WasteAgeTracker> m_wasteAgeTracker;
    private Option<ProductBuffer> m_outputBuffer;
    [DoNotSave(0, null)]
    private bool m_productDeprecationInProgress;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public NuclearWasteStorageProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (StorageProto) value;
      }
    }

    public float? EmissionIntensity
    {
      get
      {
        return !this.Prototype.EmissionIntensity.HasValue ? new float?() : new float?(!this.IsEnabled || !this.IsOperational ? 0.0f : (float) this.Prototype.EmissionIntensity.Value);
      }
    }

    protected override bool IsOperational => base.IsOperational && this.m_isOperational;

    protected override bool IoDisabled => base.IoDisabled || !this.m_isOperational;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public Option<IProductBufferReadOnly> OutputBuffer
    {
      get => this.m_outputBuffer.As<IProductBufferReadOnly>();
    }

    public bool DoNotSendRetiredWasteToOutputPort { get; private set; }

    public NuclearWasteStorage(
      EntityId id,
      NuclearWasteStorageProto storageProto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      UnlockedProtosDb unlockedProtosDb,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IVehiclesManager vehiclesManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IRadiationManager radiationManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StorageProto) storageProto, transform, context, simLoopEvents, upgraderFactory, vehiclesManager, vehicleBuffersRegistry);
      this.m_proto = storageProto;
      this.m_simLoopEvents = simLoopEvents;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_radiationManager = radiationManager;
      ProductProto[] array = this.Prototype.StorableProducts.Where<ProductProto>(new Func<ProductProto, bool>(unlockedProtosDb.IsUnlocked)).ToArray<ProductProto>();
      if (array.Length != 1)
        return;
      this.TryAssignProduct(array.First<ProductProto>());
    }

    [InitAfterLoad(InitPriority.Normal)]
    [OnlyForSaveCompatibility(null)]
    private void initSelf(int saveVersion)
    {
      RadioactiveWasteParam paramValue;
      if (!this.StoredProduct.HasValue || !this.OutputBuffer.IsNone || !this.StoredProduct.Value.TryGetParam<RadioactiveWasteParam>(out paramValue) || paramValue.YearsUntilSafeToDispose <= 0)
        return;
      this.createFuelQueueIfNeeded(this.StoredProduct.Value);
      if (saveVersion < 116)
        return;
      Log.Error(string.Format("Output buffer for {0} was not set!", (object) this.StoredProduct.Value));
    }

    public override Upoints GetQuickRemoveCost(out bool canAfford)
    {
      canAfford = false;
      return Upoints.Zero;
    }

    private void createFuelQueueIfNeeded(ProductProto product)
    {
      this.clearFuelQueueIfCan();
      if (this.m_outputBuffer.HasValue && (Proto) this.m_outputBuffer.Value.Product != (Proto) product)
        this.clearRetiredBufferIfNotEmpty();
      RadioactiveWasteParam paramValue;
      if (!product.TryGetParam<RadioactiveWasteParam>(out paramValue) || paramValue.YearsUntilSafeToDispose <= 0)
        return;
      if (this.m_outputBuffer.IsNone)
      {
        ProductProto proto;
        if (!this.Context.ProtosDb.TryGetProto<ProductProto>((Proto.ID) paramValue.TransferIntoProduct, out proto))
        {
          Log.Error(string.Format("Failed to resolve product {0}", (object) paramValue.TransferIntoProduct));
          return;
        }
        this.m_outputBuffer = (Option<ProductBuffer>) new ProductBuffer(this.Prototype.RetiredWasteCapacity, proto);
        this.updateOutputBufferReg();
      }
      this.m_wasteAgeTracker = (Option<WasteAgeTracker>) new WasteAgeTracker(this.m_simLoopEvents, paramValue.YearsUntilSafeToDispose);
    }

    public void ToggleDoNotSendRetiredWasteToOutputPort()
    {
      if (this.OutputBuffer.IsNone)
        Log.Error("No reason to toggle DoNotSendRetiredWasteToOutputPort for now output buffer");
      else
        this.DoNotSendRetiredWasteToOutputPort = !this.DoNotSendRetiredWasteToOutputPort;
    }

    /// <summary>
    /// Returns -1 if there is no fuel to retire or retiring is not possible.
    /// </summary>
    public int YearsUntilFirstWasteGetsRetired()
    {
      return this.m_wasteAgeTracker.IsNone ? -1 : this.m_wasteAgeTracker.Value.YearsUntilFirstWasteGetsRetired();
    }

    private void rotateFuelIfNeeded()
    {
      if (this.IsEmpty || this.m_wasteAgeTracker.IsNone || this.m_outputBuffer.IsNone)
        return;
      this.m_wasteAgeTracker.Value.SimStepAndRotateFuel();
      Quantity quantity = this.m_wasteAgeTracker.Value.WasteToRetire.Min(this.m_outputBuffer.Value.UsableCapacity).Min(this.Buffer.Value.Quantity);
      if (quantity.IsNotPositive)
        return;
      this.m_wasteAgeTracker.Value.WasteToRetire -= quantity;
      this.m_productDeprecationInProgress = true;
      this.Buffer.Value.RemoveExactly(quantity);
      this.m_productDeprecationInProgress = false;
      ProductQuantity output = new ProductQuantity(this.m_outputBuffer.Value.Product, quantity);
      this.Context.ProductsManager.ReportProductsTransformation(new ProductQuantity(this.Buffer.Value.Product, quantity), output, DestroyReason.General, CreateReason.Produced);
      this.m_outputBuffer.Value.StoreExactly(output.Quantity);
    }

    private void onStoredFuelChanged(ProductProto product, Quantity diff)
    {
      this.m_radiationManager.ReportSafelyStoredQuantityChange(product, diff);
      if (this.m_productDeprecationInProgress || this.m_wasteAgeTracker.IsNone)
        return;
      this.m_wasteAgeTracker.Value.ReportQuantityChanged(diff);
    }

    private void clearFuelQueueIfCan()
    {
      if (!this.m_wasteAgeTracker.HasValue)
        return;
      this.m_wasteAgeTracker = (Option<WasteAgeTracker>) Option.None;
    }

    private void clearRetiredBufferIfNotEmpty()
    {
      if (this.m_outputBuffer.IsNone)
        return;
      this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) this.m_outputBuffer.Value);
      this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) this.m_outputBuffer.Value);
      this.m_outputBuffer = Option<ProductBuffer>.None;
      this.DoNotSendRetiredWasteToOutputPort = false;
    }

    protected override void UpdateLogisticsOutputReg()
    {
      base.UpdateLogisticsOutputReg();
      this.updateOutputBufferReg();
    }

    private void updateOutputBufferReg()
    {
      if (this.m_outputBuffer.IsNone)
        return;
      ProductBuffer buffer = this.m_outputBuffer.Value;
      if (this.IsLogisticsOutputDisabled || !this.IsOperational)
        this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) buffer);
      else
        this.m_vehicleBuffersRegistry.TryRegisterOutputBuffer((IStaticEntity) this, (IProductBuffer) buffer, (IOutputBufferPriorityProvider) StaticPriorityProvider.LowestNoQuantityPreference);
    }

    protected override void SimUpdateInternal()
    {
      this.rotateFuelIfNeeded();
      if (this.IsNotEnabled)
        return;
      bool isOperational = this.m_isOperational;
      this.m_isOperational = Entity.HasWorkers((IEntityWithWorkers) this);
      if (isOperational != this.m_isOperational)
      {
        this.UpdateLogisticsInputReg();
        this.UpdateLogisticsOutputReg();
      }
      base.SimUpdateInternal();
    }

    protected override Quantity SendOutputs()
    {
      if (this.m_outputBuffer.IsNone || this.DoNotSendRetiredWasteToOutputPort)
        return base.SendOutputs();
      if (this.IoDisabled)
        return Quantity.Zero;
      Quantity zero = Quantity.Zero;
      ProductBuffer productBuffer = this.m_outputBuffer.Value;
      ImmutableArray<IoPortData> connectedOutputPorts = this.ConnectedOutputPorts;
      for (int index = 0; index < connectedOutputPorts.Length && productBuffer.Quantity.IsPositive; ++index)
      {
        Quantity maxQuantity = productBuffer.Quantity - connectedOutputPorts[index].SendAsMuchAs(new ProductQuantity(productBuffer.Product, productBuffer.Quantity));
        productBuffer.RemoveAsMuchAs(maxQuantity);
        zero += maxQuantity;
      }
      return zero;
    }

    protected override LogisticsBuffer CreateNewBuffer(Quantity capacity, ProductProto product)
    {
      NuclearWasteStorage.FuelBuffer newBuffer = new NuclearWasteStorage.FuelBuffer(capacity, product, this.Context.ProductsManager, this);
      this.createFuelQueueIfNeeded(product);
      return (LogisticsBuffer) newBuffer;
    }

    protected override void OnBufferDestroyed(ProductBuffer buffer)
    {
      this.clearFuelQueueIfCan();
      this.clearRetiredBufferIfNotEmpty();
    }

    public static void Serialize(NuclearWasteStorage value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NuclearWasteStorage>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NuclearWasteStorage.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.DoNotSendRetiredWasteToOutputPort);
      writer.WriteBool(this.m_isOperational);
      Option<ProductBuffer>.Serialize(this.m_outputBuffer, writer);
      writer.WriteGeneric<NuclearWasteStorageProto>(this.m_proto);
      writer.WriteGeneric<IRadiationManager>(this.m_radiationManager);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      Option<WasteAgeTracker>.Serialize(this.m_wasteAgeTracker, writer);
    }

    public static NuclearWasteStorage Deserialize(BlobReader reader)
    {
      NuclearWasteStorage nuclearWasteStorage;
      if (reader.TryStartClassDeserialization<NuclearWasteStorage>(out nuclearWasteStorage))
        reader.EnqueueDataDeserialization((object) nuclearWasteStorage, NuclearWasteStorage.s_deserializeDataDelayedAction);
      return nuclearWasteStorage;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.DoNotSendRetiredWasteToOutputPort = reader.ReadBool();
      this.m_isOperational = reader.ReadBool();
      this.m_outputBuffer = Option<ProductBuffer>.Deserialize(reader);
      this.m_proto = reader.ReadGenericAs<NuclearWasteStorageProto>();
      reader.SetField<NuclearWasteStorage>(this, "m_radiationManager", (object) reader.ReadGenericAs<IRadiationManager>());
      reader.SetField<NuclearWasteStorage>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<NuclearWasteStorage>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      this.m_wasteAgeTracker = Option<WasteAgeTracker>.Deserialize(reader);
      reader.RegisterInitAfterLoad<NuclearWasteStorage>(this, "initSelf", InitPriority.Normal);
    }

    static NuclearWasteStorage()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NuclearWasteStorage.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      NuclearWasteStorage.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class FuelBuffer : LogisticsBuffer
    {
      private readonly NuclearWasteStorage m_wasteStorage;
      private readonly ProductStats m_productStats;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public FuelBuffer(
        Quantity capacity,
        ProductProto product,
        IProductsManager productsManager,
        NuclearWasteStorage wasteStorage)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product);
        this.m_wasteStorage = wasteStorage;
        this.m_productStats = productsManager.GetStatsFor(product);
        productsManager.AssetManager.AddGlobalOutput((IProductBuffer) this, 10, (Option<IEntity>) (IEntity) wasteStorage);
      }

      [InitAfterLoad(InitPriority.Normal)]
      private void initSelf(int saveVersion)
      {
        if (saveVersion >= 104)
          return;
        this.m_productStats.ProductsManager.AssetManager.AddGlobalOutput((IProductBuffer) this, 10, (Option<IEntity>) (IEntity) this.m_wasteStorage);
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        this.m_productStats.StoredAvailableQuantityChange(diff);
        this.m_wasteStorage.onStoredFuelChanged(this.Product, diff);
      }

      public override void Destroy()
      {
        Assert.That<Quantity>(this.Quantity).IsZero("Buffer was not cleared before destroy!");
        this.m_productStats.ProductsManager.AssetManager.RemoveGlobalOutput((IProductBuffer) this);
        base.Destroy();
      }

      public static void Serialize(NuclearWasteStorage.FuelBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<NuclearWasteStorage.FuelBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, NuclearWasteStorage.FuelBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        ProductStats.Serialize(this.m_productStats, writer);
        NuclearWasteStorage.Serialize(this.m_wasteStorage, writer);
      }

      public static NuclearWasteStorage.FuelBuffer Deserialize(BlobReader reader)
      {
        NuclearWasteStorage.FuelBuffer fuelBuffer;
        if (reader.TryStartClassDeserialization<NuclearWasteStorage.FuelBuffer>(out fuelBuffer))
          reader.EnqueueDataDeserialization((object) fuelBuffer, NuclearWasteStorage.FuelBuffer.s_deserializeDataDelayedAction);
        return fuelBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<NuclearWasteStorage.FuelBuffer>(this, "m_productStats", (object) ProductStats.Deserialize(reader));
        reader.SetField<NuclearWasteStorage.FuelBuffer>(this, "m_wasteStorage", (object) NuclearWasteStorage.Deserialize(reader));
        reader.RegisterInitAfterLoad<NuclearWasteStorage.FuelBuffer>(this, "initSelf", InitPriority.Normal);
      }

      static FuelBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        NuclearWasteStorage.FuelBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        NuclearWasteStorage.FuelBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
