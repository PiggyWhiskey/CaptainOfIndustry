// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.RegisteredInputBuffer
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
  public class RegisteredInputBuffer : IRegisteredBuffer, IEntityObserverForEnabled, IEntityObserver
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>Initial number of jobs we don't penalize.</summary>
    internal const int JOBS_NO_WITHOUT_PENALTY = 4;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredInputBuffer.RefreshPriorities" />.
    /// </summary>
    [DoNotSave(0, null)]
    public bool IsAvailableCached;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredInputBuffer.RefreshPriorities" />.
    /// </summary>
    [DoNotSave(0, null)]
    public Quantity? OptimalQuantityCached;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredInputBuffer.RefreshPriorities" />.
    /// </summary>
    [DoNotSave(0, null)]
    public Quantity OptimalQuantityOrMaxCached;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredInputBuffer.RefreshPriorities" />.
    /// </summary>
    [DoNotSave(0, null)]
    public int RawPriorityCached;
    /// <summary>
    /// WARNING: Do not use this unless you called <see cref="M:Mafi.Core.Vehicles.RegisteredInputBuffer.RefreshPriorities" />.
    /// This priority includes penalty for already assigned vehicles. If you need the
    /// original priority use <see cref="F:Mafi.Core.Vehicles.RegisteredInputBuffer.RawPriorityCached" />
    /// </summary>
    [DoNotSave(0, null)]
    public int CombinedPriorityCached;
    /// <summary>
    /// Indicates that this buffer is supposed to be used only as a last resort when
    /// we get into bad state otherwise (deconstructions, stuck with cargo).
    /// 
    /// Currently provided by shipyard.
    /// </summary>
    public readonly bool IsFallbackOnly;
    [DoNotSave(0, null)]
    public Quantity RemainingCapacityCached;
    [DoNotSave(0, null)]
    private int m_numberOfVehiclesAssignedCache;
    [DoNotSave(0, null)]
    private Option<LogisticsBuffer> m_logisticsBuffer;
    public readonly IProductBuffer Buffer;
    private readonly IInputBufferPriorityProvider m_priorityProvider;
    private readonly bool m_alwaysEnabled;
    private Quantity m_pendingQuantity;
    private readonly VehicleJobs m_jobs;

    public static void Serialize(RegisteredInputBuffer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RegisteredInputBuffer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RegisteredInputBuffer.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.AllowDeliveryAtDistanceWhenBlocked);
      writer.WriteGeneric<IProductBuffer>(this.Buffer);
      writer.WriteGeneric<IStaticEntity>(this.Entity);
      writer.WriteBool(this.IgnoreAssignedEntities);
      writer.WriteBool(this.IsEnabled);
      writer.WriteBool(this.IsFallbackOnly);
      writer.WriteBool(this.m_alwaysEnabled);
      VehicleJobs.Serialize(this.m_jobs, writer);
      Quantity.Serialize(this.m_pendingQuantity, writer);
      writer.WriteGeneric<IInputBufferPriorityProvider>(this.m_priorityProvider);
    }

    public static RegisteredInputBuffer Deserialize(BlobReader reader)
    {
      RegisteredInputBuffer registeredInputBuffer;
      if (reader.TryStartClassDeserialization<RegisteredInputBuffer>(out registeredInputBuffer))
        reader.EnqueueDataDeserialization((object) registeredInputBuffer, RegisteredInputBuffer.s_deserializeDataDelayedAction);
      return registeredInputBuffer;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AllowDeliveryAtDistanceWhenBlocked = reader.ReadBool();
      reader.SetField<RegisteredInputBuffer>(this, "Buffer", (object) reader.ReadGenericAs<IProductBuffer>());
      this.Entity = reader.ReadGenericAs<IStaticEntity>();
      this.IgnoreAssignedEntities = reader.ReadBool();
      this.IsEnabled = reader.ReadBool();
      reader.SetField<RegisteredInputBuffer>(this, "IsFallbackOnly", (object) reader.ReadBool());
      reader.SetField<RegisteredInputBuffer>(this, "m_alwaysEnabled", (object) reader.ReadBool());
      reader.SetField<RegisteredInputBuffer>(this, "m_jobs", (object) VehicleJobs.Deserialize(reader));
      this.m_pendingQuantity = Quantity.Deserialize(reader);
      reader.SetField<RegisteredInputBuffer>(this, "m_priorityProvider", (object) reader.ReadGenericAs<IInputBufferPriorityProvider>());
      reader.RegisterInitAfterLoad<RegisteredInputBuffer>(this, "initSelf", InitPriority.Normal);
    }

    /// <summary>Do not use this in perf sensitive code.</summary>
    public BufferStrategy StrategySlow
    {
      get => this.m_priorityProvider.GetInputPriority(this.Buffer, this.m_pendingQuantity);
    }

    public Quantity RemainingCapacity
    {
      get
      {
        LogisticsBuffer valueOrNull = this.m_logisticsBuffer.ValueOrNull;
        return (valueOrNull != null ? valueOrNull.UsableCapacityForImport() : this.Buffer.UsableCapacity) - this.m_pendingQuantity;
      }
    }

    public ProductProto Product => this.Buffer.Product;

    public bool IsEnabled { get; private set; }

    public bool IgnoreAssignedEntities { get; private set; }

    public IStaticEntity Entity { get; private set; }

    [DoNotSave(0, null)]
    public Tile2f Position2f { get; private set; }

    public bool IsConstructionBuffer => this.m_alwaysEnabled && !this.Entity.IsConstructed;

    [DoNotSave(0, null)]
    public Option<IEntityAssignedAsInput> EntityAsAssignee { get; private set; }

    public bool HasAssignedOutputEntities
    {
      get
      {
        IEntityAssignedAsInput valueOrNull = this.EntityAsAssignee.ValueOrNull;
        return valueOrNull != null && valueOrNull.AssignedOutputs.IsNotEmpty<IEntityAssignedAsOutput>();
      }
    }

    [DoNotSave(0, null)]
    public Option<IEntityEnforcingAssignedVehicles> VehiclesEnforcer { get; private set; }

    public bool AllowDeliveryAtDistanceWhenBlocked { get; private set; }

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

    public IReadOnlySet<IVehicleJob> AllReservedJobs => this.m_jobs.AllJobs;

    public RegisteredInputBuffer(
      IStaticEntity entity,
      IProductBuffer buffer,
      IInputBufferPriorityProvider priorityProvider,
      bool alwaysEnabled,
      bool isFallbackOnly,
      bool allowDeliveryAtDistanceWhenBlocked)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_numberOfVehiclesAssignedCache = -1;
      this.m_jobs = new VehicleJobs();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Entity = entity;
      this.m_priorityProvider = priorityProvider.CheckNotNull<IInputBufferPriorityProvider>();
      this.Buffer = buffer;
      this.m_alwaysEnabled = alwaysEnabled;
      this.IsFallbackOnly = isFallbackOnly;
      this.AllowDeliveryAtDistanceWhenBlocked = allowDeliveryAtDistanceWhenBlocked;
      this.IsEnabled = alwaysEnabled || entity.IsEnabled;
      this.IgnoreAssignedEntities = alwaysEnabled;
      this.initSelf();
      if (isFallbackOnly)
      {
        Assert.That<int>(this.StrategySlow.Priority).IsEqualTo(16);
        Assert.That<bool>(alwaysEnabled).IsTrue();
      }
      entity.AddObserver((IEntityObserver) this);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.Position2f = this.Entity.Position2f;
      this.EntityAsAssignee = Option.Create<IEntityAssignedAsInput>(this.Entity as IEntityAssignedAsInput);
      this.VehiclesEnforcer = Option.Create<IEntityEnforcingAssignedVehicles>(this.Entity as IEntityEnforcingAssignedVehicles);
      this.m_logisticsBuffer = Option<LogisticsBuffer>.Create(this.Buffer as LogisticsBuffer);
    }

    public void RefreshPriorities()
    {
      this.m_numberOfVehiclesAssignedCache = -1;
      this.RemainingCapacityCached = this.RemainingCapacity;
      if (this.IsEnabled && this.RemainingCapacityCached.IsPositive)
      {
        BufferStrategy strategySlow = this.StrategySlow;
        this.RawPriorityCached = strategySlow.Priority;
        this.CombinedPriorityCached = this.RawPriorityCached + (this.m_jobs.Count - 4).Max(0) / 2;
        this.OptimalQuantityCached = strategySlow.OptimalQuantity;
        this.IsAvailableCached = this.RawPriorityCached != 16 && !this.IsFallbackOnly;
        this.OptimalQuantityOrMaxCached = this.OptimalQuantityCached ?? Quantity.MaxValue;
      }
      else
      {
        this.IsAvailableCached = false;
        this.RawPriorityCached = 16;
        this.CombinedPriorityCached = 16;
        this.OptimalQuantityCached = new Quantity?();
        this.RemainingCapacityCached = Quantity.Zero;
        this.OptimalQuantityOrMaxCached = Quantity.MaxValue;
      }
    }

    public bool CanBeServedBy(Vehicle vehicle, bool isBalancingJob)
    {
      return this.VehiclesEnforcer.IsNone || this.IgnoreAssignedEntities || this.VehiclesEnforcer.Value.CanBeServedBy(vehicle, isBalancingJob);
    }

    public bool Reserve(ICargoDeliveryJob job) => this.Reserve(job, job.CargoToDeliver.Quantity);

    public bool Reserve(ICargoDeliveryJob job, Quantity quantity)
    {
      if (this.Buffer.UsableCapacity.IsNotPositive || !this.m_jobs.TryAddJob((IVehicleJob) job))
        return false;
      this.m_pendingQuantity += quantity;
      return true;
    }

    /// <summary>Returns amount of received cargo.</summary>
    public Quantity ReceiveCargo(ICargoDeliveryJob job, Quantity maxQuantity, Quantity reserved)
    {
      if (!this.m_jobs.TryRemoveJob((IVehicleJob) job))
        return Quantity.Zero;
      this.m_pendingQuantity -= reserved;
      if (this.m_pendingQuantity.IsNegative)
      {
        Log.Error("m_pendingQuantity got negative!");
        this.m_pendingQuantity = Quantity.Zero;
      }
      Quantity quantity = maxQuantity.Min(this.Buffer.UsableCapacity - this.m_pendingQuantity).Max(Quantity.Zero);
      return quantity - this.Buffer.StoreAsMuchAs(quantity);
    }

    public void CancelReservation(ICargoDeliveryJob job)
    {
      this.CancelReservation(job, job.CargoToDeliver.Quantity);
    }

    public void CancelReservation(ICargoDeliveryJob job, Quantity quantity)
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

    static RegisteredInputBuffer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RegisteredInputBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RegisteredInputBuffer) obj).SerializeData(writer));
      RegisteredInputBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RegisteredInputBuffer) obj).DeserializeData(reader));
    }
  }
}
