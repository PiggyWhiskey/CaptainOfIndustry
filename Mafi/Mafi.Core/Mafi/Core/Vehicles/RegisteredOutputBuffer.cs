// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.RegisteredOutputBuffer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [GenerateSerializer(false, null, 0)]
  public class RegisteredOutputBuffer : IRegisteredBuffer, IEntityObserverForEnabled, IEntityObserver
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredOutputBuffer.RefreshPriorities(System.Boolean)" />.
    /// </summary>
    [DoNotSave(0, null)]
    public bool IsAvailableCached;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredOutputBuffer.RefreshPriorities(System.Boolean)" />.
    /// </summary>
    [DoNotSave(0, null)]
    public Quantity? OptimalQuantityCached;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredOutputBuffer.RefreshPriorities(System.Boolean)" />.
    /// </summary>
    [DoNotSave(0, null)]
    public Quantity OptimalQuantityOrMaxCached;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredOutputBuffer.RefreshPriorities(System.Boolean)" />.
    /// </summary>
    [DoNotSave(0, null)]
    public int RawPriorityCached;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredOutputBuffer.RefreshPriorities(System.Boolean)" />.
    /// This priority includes penalty for already assigned vehicles. If you need the
    /// original priority use <see cref="F:Mafi.Core.Vehicles.RegisteredOutputBuffer.RawPriorityCached" />
    /// </summary>
    [DoNotSave(0, null)]
    public int CombinedPriorityCached;
    [DoNotSave(0, null)]
    public Quantity AvailableQuantityCached;
    /// <summary>
    /// Indicates that this buffer has such a important role that we need to try to
    /// pair it with fallback buffer if we can't satisfy it otherwise.
    /// 
    /// Used by deconstruction to dump stuff into shipyard.
    /// </summary>
    public readonly bool UseFallbackIfNeeded;
    [DoNotSave(0, null)]
    private int m_numberOfVehiclesAssignedCache;
    [DoNotSave(0, null)]
    private Option<LogisticsBuffer> m_logisticsBuffer;
    public readonly IProductBuffer Buffer;
    private readonly IOutputBufferPriorityProvider m_priorityProvider;
    private readonly bool m_alwaysEnabled;
    private Quantity m_pendingQuantity;
    private VehicleJobs m_jobs;

    public static void Serialize(RegisteredOutputBuffer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RegisteredOutputBuffer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RegisteredOutputBuffer.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.AllowPickupAtDistanceWhenBlocked);
      writer.WriteGeneric<IProductBuffer>(this.Buffer);
      writer.WriteGeneric<IStaticEntity>(this.Entity);
      writer.WriteBool(this.IsEnabled);
      writer.WriteBool(this.m_alwaysEnabled);
      VehicleJobs.Serialize(this.m_jobs, writer);
      Quantity.Serialize(this.m_pendingQuantity, writer);
      writer.WriteGeneric<IOutputBufferPriorityProvider>(this.m_priorityProvider);
      writer.WriteBool(this.UseFallbackIfNeeded);
    }

    public static RegisteredOutputBuffer Deserialize(BlobReader reader)
    {
      RegisteredOutputBuffer registeredOutputBuffer;
      if (reader.TryStartClassDeserialization<RegisteredOutputBuffer>(out registeredOutputBuffer))
        reader.EnqueueDataDeserialization((object) registeredOutputBuffer, RegisteredOutputBuffer.s_deserializeDataDelayedAction);
      return registeredOutputBuffer;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AllowPickupAtDistanceWhenBlocked = reader.ReadBool();
      reader.SetField<RegisteredOutputBuffer>(this, "Buffer", (object) reader.ReadGenericAs<IProductBuffer>());
      this.Entity = reader.ReadGenericAs<IStaticEntity>();
      this.IsEnabled = reader.ReadBool();
      reader.SetField<RegisteredOutputBuffer>(this, "m_alwaysEnabled", (object) reader.ReadBool());
      this.m_jobs = VehicleJobs.Deserialize(reader);
      this.m_pendingQuantity = Quantity.Deserialize(reader);
      reader.SetField<RegisteredOutputBuffer>(this, "m_priorityProvider", (object) reader.ReadGenericAs<IOutputBufferPriorityProvider>());
      reader.SetField<RegisteredOutputBuffer>(this, "UseFallbackIfNeeded", (object) reader.ReadBool());
      reader.RegisterInitAfterLoad<RegisteredOutputBuffer>(this, "initSelf", InitPriority.Normal);
    }

    /// <summary>Do not use this in perf sensitive code.</summary>
    public BufferStrategy StrategySlow
    {
      get
      {
        return this.m_priorityProvider.GetOutputPriority(new OutputPriorityRequest(this.Buffer, this.m_pendingQuantity, false));
      }
    }

    private BufferStrategy StrategySlowForRefuelling
    {
      get
      {
        return this.m_priorityProvider.GetOutputPriority(new OutputPriorityRequest(this.Buffer, this.m_pendingQuantity, true));
      }
    }

    public Quantity AvailableQuantity
    {
      get
      {
        LogisticsBuffer valueOrNull = this.m_logisticsBuffer.ValueOrNull;
        return (valueOrNull != null ? valueOrNull.QuantityForExport() : this.Buffer.Quantity) - this.m_pendingQuantity;
      }
    }

    public Quantity AvailableQuantityForRefuel => this.Buffer.Quantity - this.m_pendingQuantity;

    public ProductProto Product => this.Buffer.Product;

    public bool IsEnabled { get; private set; }

    public bool IgnoreAssignedEntities => this.m_alwaysEnabled;

    public IStaticEntity Entity { get; private set; }

    [DoNotSave(0, null)]
    public Tile2f Position2f { get; private set; }

    public bool IsConstructionBuffer => this.m_alwaysEnabled && !this.Entity.IsConstructed;

    [DoNotSave(0, null)]
    public Option<IEntityAssignedAsOutput> EntityAsAssignee { get; private set; }

    public bool HasAssignedInputEntities
    {
      get
      {
        IEntityAssignedAsOutput valueOrNull = this.EntityAsAssignee.ValueOrNull;
        return valueOrNull != null && valueOrNull.AssignedInputs.IsNotEmpty<IEntityAssignedAsInput>();
      }
    }

    [DoNotSave(0, null)]
    public Option<IEntityEnforcingAssignedVehicles> VehiclesEnforcer { get; private set; }

    public bool AllowPickupAtDistanceWhenBlocked { get; private set; }

    public int NumberOfVehiclesAssigned
    {
      get
      {
        if (this.m_numberOfVehiclesAssignedCache < 0)
        {
          IEntityEnforcingAssignedVehicles valueOrNull = this.VehiclesEnforcer.ValueOrNull;
          this.m_numberOfVehiclesAssignedCache = valueOrNull != null ? valueOrNull.VehiclesTotal() : 0;
        }
        return this.m_numberOfVehiclesAssignedCache;
      }
    }

    public int JobsCount => this.m_jobs.Count;

    public RegisteredOutputBuffer(
      IStaticEntity entity,
      IProductBuffer buffer,
      IOutputBufferPriorityProvider priorityProvider,
      bool alwaysEnabled,
      bool useFallbackIfNeeded,
      bool allowPickupAtDistanceWhenBlocked)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_numberOfVehiclesAssignedCache = -1;
      this.m_jobs = new VehicleJobs();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Entity = entity;
      this.m_priorityProvider = priorityProvider.CheckNotNull<IOutputBufferPriorityProvider>();
      this.Buffer = buffer;
      this.m_alwaysEnabled = alwaysEnabled;
      this.UseFallbackIfNeeded = useFallbackIfNeeded;
      this.AllowPickupAtDistanceWhenBlocked = allowPickupAtDistanceWhenBlocked;
      this.IsEnabled = alwaysEnabled || entity.IsEnabled;
      this.initSelf();
      entity.AddObserver((IEntityObserver) this);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.Position2f = this.Entity.Position2f;
      this.EntityAsAssignee = Option.Create<IEntityAssignedAsOutput>(this.Entity as IEntityAssignedAsOutput);
      this.VehiclesEnforcer = Option.Create<IEntityEnforcingAssignedVehicles>(this.Entity as IEntityEnforcingAssignedVehicles);
      this.m_logisticsBuffer = Option<LogisticsBuffer>.Create(this.Buffer as LogisticsBuffer);
    }

    public void RefreshPriorities(bool isRefuelRequest = false)
    {
      this.m_numberOfVehiclesAssignedCache = -1;
      this.AvailableQuantityCached = isRefuelRequest ? this.AvailableQuantityForRefuel : this.AvailableQuantity;
      if (this.IsEnabled && this.AvailableQuantityCached.IsPositive)
      {
        BufferStrategy bufferStrategy = isRefuelRequest ? this.StrategySlowForRefuelling : this.StrategySlow;
        this.RawPriorityCached = isRefuelRequest ? bufferStrategy.PriorityForRefueling : bufferStrategy.Priority;
        this.CombinedPriorityCached = this.RawPriorityCached + (this.m_jobs.Count - 4).Max(0) / 2;
        this.OptimalQuantityCached = bufferStrategy.OptimalQuantity;
        this.OptimalQuantityOrMaxCached = this.OptimalQuantityCached ?? Quantity.MaxValue;
        this.IsAvailableCached = this.RawPriorityCached != 16;
      }
      else
      {
        this.IsAvailableCached = false;
        this.RawPriorityCached = 16;
        this.CombinedPriorityCached = 16;
        this.OptimalQuantityCached = new Quantity?();
        this.AvailableQuantityCached = Quantity.Zero;
        this.OptimalQuantityOrMaxCached = Quantity.MaxValue;
      }
    }

    public bool CanBeServedBy(Vehicle vehicle, bool isBalancingJob)
    {
      return this.VehiclesEnforcer.IsNone || this.IgnoreAssignedEntities || this.VehiclesEnforcer.Value.CanBeServedBy(vehicle, isBalancingJob);
    }

    public bool Reserve(ICargoPickUpJob job) => this.Reserve(job, job.CargoToPickup.Quantity);

    public bool Reserve(ICargoPickUpJob job, Quantity quantity)
    {
      if (this.Buffer.Quantity.IsNotPositive || !this.m_jobs.TryAddJob((IVehicleJob) job))
        return false;
      this.m_pendingQuantity += quantity;
      return true;
    }

    /// <summary>Returns how much was removed</summary>
    public Quantity GetCargo(ICargoPickUpJob job, Quantity maxToPickUp, Quantity reserved)
    {
      if (!this.m_jobs.TryRemoveJob((IVehicleJob) job))
        return Quantity.Zero;
      this.m_pendingQuantity -= reserved;
      if (this.m_pendingQuantity.IsNegative)
      {
        Log.Error("m_pendingQuantity got negative!");
        this.m_pendingQuantity = Quantity.Zero;
      }
      return this.Buffer.RemoveAsMuchAs(maxToPickUp.Min(this.Buffer.Quantity - this.m_pendingQuantity).Max(Quantity.Zero));
    }

    public void CancelReservation(ICargoPickUpJob job)
    {
      this.CancelReservation(job, job.CargoToPickup.Quantity);
    }

    public void CancelReservation(ICargoPickUpJob job, Quantity quantity)
    {
      if (!this.m_jobs.TryRemoveJob((IVehicleJob) job))
        return;
      this.m_pendingQuantity -= quantity;
      if (!this.m_pendingQuantity.IsNegative)
        return;
      Log.Error("m_pendingQuantity got negative!");
      this.m_pendingQuantity = Quantity.Zero;
    }

    public void Destroy()
    {
      this.m_jobs.Destroy();
      this.Entity.RemoveObserver((IEntityObserver) this);
    }

    public void ClearAndCancelAllJobs() => this.m_jobs.ClearAndCancelAllJobs();

    public bool WrapsBuffer(IProductBuffer buffer) => this.Buffer == buffer;

    void IEntityObserverForEnabled.OnEnabledChange(IEntity entity, bool isEnabled)
    {
      this.IsEnabled = this.m_alwaysEnabled | isEnabled;
    }

    void IEntityObserver.OnEntityDestroy(IEntity entity)
    {
      entity.RemoveObserver((IEntityObserver) this);
    }

    static RegisteredOutputBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RegisteredOutputBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RegisteredOutputBuffer) obj).SerializeData(writer));
      RegisteredOutputBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RegisteredOutputBuffer) obj).DeserializeData(reader));
    }
  }
}
