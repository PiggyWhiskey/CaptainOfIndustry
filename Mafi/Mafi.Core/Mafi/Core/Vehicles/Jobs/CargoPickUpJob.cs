// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.CargoPickUpJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job in which a vehicle picks up some cargo from output port.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class CargoPickUpJob : 
    VehicleJob,
    ICargoPickUpJob,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey,
    IQueueTipJob,
    IJobWithPreNavigation
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly IVehicleForCargoJob m_vehicleForCargoJob;
    private readonly RegisteredOutputBuffer m_outputBuffer;
    private readonly TickTimer m_timer;
    /// <summary>
    /// Tracks how we progressively add cargo to the truck to animate the addition.
    /// </summary>
    private Quantity m_loadedOnTruck;
    private Quantity? m_realQuantityToPickup;
    /// <summary>
    /// Secondary delivery target that are reasonably close so we can just teleport
    ///  rest of our cargo to them. These deliveries are still subject to reservations.
    /// </summary>
    private readonly Option<Lyst<SecondaryOutputBufferSpec>> m_secondaryBuffers;
    private readonly CargoPickUpJob.Factory m_factory;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IJobWithPreNavigation m_navigationJob;

    public static void Serialize(CargoPickUpJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoPickUpJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoPickUpJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductQuantity.Serialize(this.CargoToPickup, writer);
      Quantity.Serialize(this.m_loadedOnTruck, writer);
      writer.WriteGeneric<IJobWithPreNavigation>(this.m_navigationJob);
      RegisteredOutputBuffer.Serialize(this.m_outputBuffer, writer);
      writer.WriteNullableStruct<Quantity>(this.m_realQuantityToPickup);
      Option<Lyst<SecondaryOutputBufferSpec>>.Serialize(this.m_secondaryBuffers, writer);
      TickTimer.Serialize(this.m_timer, writer);
      writer.WriteGeneric<IVehicleForCargoJob>(this.m_vehicleForCargoJob);
    }

    public static CargoPickUpJob Deserialize(BlobReader reader)
    {
      CargoPickUpJob cargoPickUpJob;
      if (reader.TryStartClassDeserialization<CargoPickUpJob>(out cargoPickUpJob))
        reader.EnqueueDataDeserialization((object) cargoPickUpJob, CargoPickUpJob.s_deserializeDataDelayedAction);
      return cargoPickUpJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CargoToPickup = ProductQuantity.Deserialize(reader);
      reader.RegisterResolvedMember<CargoPickUpJob>(this, "m_factory", typeof (CargoPickUpJob.Factory), true);
      this.m_loadedOnTruck = Quantity.Deserialize(reader);
      reader.SetField<CargoPickUpJob>(this, "m_navigationJob", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IJobWithPreNavigation>() : (object) (IJobWithPreNavigation) null);
      reader.SetField<CargoPickUpJob>(this, "m_outputBuffer", (object) RegisteredOutputBuffer.Deserialize(reader));
      this.m_realQuantityToPickup = reader.ReadNullableStruct<Quantity>();
      reader.SetField<CargoPickUpJob>(this, "m_secondaryBuffers", (object) Option<Lyst<SecondaryOutputBufferSpec>>.Deserialize(reader));
      reader.SetField<CargoPickUpJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<CargoPickUpJob>(this, "m_vehicleForCargoJob", (object) reader.ReadGenericAs<IVehicleForCargoJob>());
    }

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public LocStrFormatted GoalName => this.m_outputBuffer.Entity.DefaultTitle;

    public ProductQuantity CargoToPickup { get; private set; }

    bool IQueueTipJob.WaitBehindQueueTipVehicle => false;

    internal RegisteredOutputBuffer OutputBuffer => this.m_outputBuffer;

    internal CargoPickUpJob(
      VehicleJobId id,
      CargoPickUpJob.Factory factory,
      IVehicleForCargoJob vehicleForCargoJob,
      ProductQuantity toPickup,
      RegisteredOutputBuffer outputBuffer,
      Option<Lyst<SecondaryOutputBufferSpec>> secondaryBuffers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory.CheckNotNull<CargoPickUpJob.Factory>();
      Assert.That<bool>(toPickup.IsEmpty).IsFalse();
      this.CargoToPickup = toPickup;
      this.m_vehicleForCargoJob = vehicleForCargoJob;
      this.m_outputBuffer = outputBuffer.CheckNotNull<RegisteredOutputBuffer>();
      this.m_secondaryBuffers = secondaryBuffers;
      Vehicle vehicle = (vehicleForCargoJob as Vehicle).CheckNotNull<Vehicle>();
      Assert.That<bool>(this.m_outputBuffer.Reserve((ICargoPickUpJob) this)).IsTrue();
      if (this.m_secondaryBuffers.HasValue)
      {
        foreach (SecondaryOutputBufferSpec outputBufferSpec in this.m_secondaryBuffers.Value)
          outputBufferSpec.Buffer.Reserve((ICargoPickUpJob) this, outputBufferSpec.Quantity);
      }
      VehicleQueue<Vehicle, IStaticEntity> queue;
      if (outputBuffer.Entity is IStaticEntityWithQueue entity && entity.TryGetVehicleQueueFor(vehicle, out queue))
      {
        VehicleQueueJob<Vehicle> staticOwnedQueue = this.m_factory.QueueingJobFactory.CreateJobForStaticOwnedQueue<IStaticEntity, Vehicle>(vehicle, queue, navigateClosebyIsSufficient: outputBuffer.AllowPickupAtDistanceWhenBlocked);
        this.m_navigationJob = (IJobWithPreNavigation) staticOwnedQueue;
        staticOwnedQueue.DoJobAtQueueTipAndLeave((IQueueTipJob) this);
        vehicle.EnqueueJob((VehicleJob) staticOwnedQueue);
      }
      else
        this.m_navigationJob = (IJobWithPreNavigation) this.m_factory.NavigateToJobFactory.EnqueueJob(vehicle, (IVehicleGoalFull) this.m_factory.StaticEntityGoalFactory.Create(outputBuffer.Entity), outputBuffer.AllowPickupAtDistanceWhenBlocked);
      this.m_vehicleForCargoJob.EnqueueJob((VehicleJob) this);
    }

    public override LocStrFormatted JobInfo
    {
      get
      {
        return new LocStrFormatted("Picking up {0} at {1}.".FormatInvariant((object) this.CargoToPickup.FormatNumberAndUnitOnly(), (object) this.m_outputBuffer.Entity.Prototype.Strings.Name));
      }
    }

    protected override bool DoJobInternal()
    {
      Assert.That<bool>(this.m_vehicleForCargoJob.IsDriving).IsFalse();
      if (!this.m_realQuantityToPickup.HasValue)
      {
        Assert.That<bool>(this.m_vehicleForCargoJob.Cargo.CanAdd(this.CargoToPickup.Product)).IsTrue();
        this.m_realQuantityToPickup = new Quantity?(this.m_outputBuffer.GetCargo((ICargoPickUpJob) this, this.CargoToPickup.Quantity, this.CargoToPickup.Quantity));
        Quantity? quantityToPickup = this.m_realQuantityToPickup;
        Quantity zero = Quantity.Zero;
        if ((quantityToPickup.HasValue ? (quantityToPickup.GetValueOrDefault() == zero ? 1 : 0) : 0) != 0)
        {
          this.RequestCancel();
          return false;
        }
        this.m_timer.Start(this.m_vehicleForCargoJob.CargoPickupDuration);
        return true;
      }
      if (this.m_timer.Decrement())
      {
        int ticks = this.m_vehicleForCargoJob.CargoPickupDuration.Ticks;
        int num = this.m_timer.Ticks.Ticks - 1;
        Quantity loadedOnTruck = this.m_loadedOnTruck;
        this.m_loadedOnTruck = Quantity.Zero.Lerp(this.m_realQuantityToPickup.Value, ticks - num, ticks);
        if (this.m_loadedOnTruck > loadedOnTruck)
          Assert.That<ProductQuantity>(this.m_vehicleForCargoJob.LoadCargoReturnExcess(this.CargoToPickup.WithNewQuantity(this.m_loadedOnTruck - loadedOnTruck))).IsEmpty("Failed to load some cargo on the truck.");
        return true;
      }
      Assert.That<Quantity>(this.m_loadedOnTruck).IsLessOrEqual(this.m_realQuantityToPickup.Value, "Too many goods were picked up.");
      if (this.m_loadedOnTruck < this.m_realQuantityToPickup.Value)
        Assert.That<ProductQuantity>(this.m_vehicleForCargoJob.LoadCargoReturnExcess(this.CargoToPickup.WithNewQuantity(this.m_realQuantityToPickup.Value - this.m_loadedOnTruck))).IsEmpty("Failed to load some cargo on the truck.");
      if (this.m_secondaryBuffers.HasValue)
      {
        foreach (SecondaryOutputBufferSpec outputBufferSpec in this.m_secondaryBuffers.Value)
        {
          if (outputBufferSpec.Buffer.IsEnabled)
          {
            Quantity maxToPickUp = outputBufferSpec.Quantity.Min(this.m_vehicleForCargoJob.RemainingCapacity);
            Assert.That<ProductQuantity>(this.m_vehicleForCargoJob.LoadCargoReturnExcess(this.CargoToPickup.Product.WithQuantity(outputBufferSpec.Buffer.GetCargo((ICargoPickUpJob) this, maxToPickUp, outputBufferSpec.Quantity)))).IsEmpty();
          }
          else
            outputBufferSpec.Buffer.CancelReservation((ICargoPickUpJob) this, outputBufferSpec.Quantity);
        }
      }
      this.InvokeJobDone();
      return false;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      if (!this.m_realQuantityToPickup.HasValue)
      {
        this.m_outputBuffer.CancelReservation((ICargoPickUpJob) this);
      }
      else
      {
        Quantity newQuantity = this.m_realQuantityToPickup.Value - this.m_loadedOnTruck;
        if (newQuantity.IsPositive)
          this.m_factory.AssetTransactionManager.StoreProduct(this.CargoToPickup.WithNewQuantity(newQuantity), new CreateReason?());
      }
      if (this.m_secondaryBuffers.HasValue)
      {
        foreach (SecondaryOutputBufferSpec outputBufferSpec in this.m_secondaryBuffers.Value)
          outputBufferSpec.Buffer.CancelReservation((ICargoPickUpJob) this, outputBufferSpec.Quantity);
      }
      this.InvokeJobDone();
      ((IVehicleFriend) this.m_vehicleForCargoJob).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      this.CargoToPickup = ProductQuantity.None;
      this.m_loadedOnTruck = Quantity.Zero;
      this.m_realQuantityToPickup = new Quantity?();
      this.m_timer.Reset();
    }

    public RobustNavHelper StartNavigation() => this.m_navigationJob.StartNavigation();

    static CargoPickUpJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoPickUpJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      CargoPickUpJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      public readonly NavigateToJob.Factory NavigateToJobFactory;
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly VehicleQueueJobFactory QueueingJobFactory;
      internal readonly StaticEntityVehicleGoal.Factory StaticEntityGoalFactory;
      public readonly IAssetTransactionManager AssetTransactionManager;

      public Factory(
        VehicleQueueJobFactory queueingJobFactory,
        NavigateToJob.Factory navigateToJobFactory,
        VehicleJobId.Factory vehicleJobIdFactory,
        StaticEntityVehicleGoal.Factory staticEntityGoalFactory,
        IAssetTransactionManager assetTransactionManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.QueueingJobFactory = queueingJobFactory;
        this.NavigateToJobFactory = navigateToJobFactory;
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.StaticEntityGoalFactory = staticEntityGoalFactory;
        this.AssetTransactionManager = assetTransactionManager;
      }

      public CargoPickUpJob EnqueueJob(
        IVehicleForCargoJob vehicleForCargoJob,
        ProductQuantity toPickup,
        RegisteredOutputBuffer outputBuffer,
        Option<Lyst<SecondaryOutputBufferSpec>> secondaryBuffers)
      {
        return new CargoPickUpJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicleForCargoJob, toPickup, outputBuffer, secondaryBuffers);
      }
    }
  }
}
