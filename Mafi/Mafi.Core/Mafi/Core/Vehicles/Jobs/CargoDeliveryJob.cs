// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.CargoDeliveryJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job in which a vehicle delivers some cargo to an input port.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class CargoDeliveryJob : 
    VehicleJob,
    ICargoDeliveryJob,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey,
    IQueueTipJob,
    IJobWithPreNavigation
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Truck m_truck;
    private readonly RegisteredInputBuffer m_inputBuffer;
    private readonly TickTimer m_timer;
    private Quantity? m_amountOfCargoDelivered;
    /// <summary>
    /// Cargo from Truck is first gradually moved to this buffer and after it is done, it is all moved to the
    /// storage.
    /// </summary>
    private Quantity m_cargoTakenFromTruck;
    /// <summary>
    /// Secondary delivery target that are reasonably close so we can just teleport
    ///  rest of our cargo to them. These deliveries are still subject to reservations.
    /// </summary>
    private readonly Option<Lyst<SecondaryInputBufferSpec>> m_secondaryBuffers;
    private readonly CargoDeliveryJob.Factory m_factory;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly IJobWithPreNavigation m_navigationJob;

    public static void Serialize(CargoDeliveryJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoDeliveryJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoDeliveryJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductQuantity.Serialize(this.CargoToDeliver, writer);
      writer.WriteNullableStruct<Quantity>(this.m_amountOfCargoDelivered);
      Quantity.Serialize(this.m_cargoTakenFromTruck, writer);
      RegisteredInputBuffer.Serialize(this.m_inputBuffer, writer);
      writer.WriteGeneric<IJobWithPreNavigation>(this.m_navigationJob);
      Option<Lyst<SecondaryInputBufferSpec>>.Serialize(this.m_secondaryBuffers, writer);
      TickTimer.Serialize(this.m_timer, writer);
      Truck.Serialize(this.m_truck, writer);
    }

    public static CargoDeliveryJob Deserialize(BlobReader reader)
    {
      CargoDeliveryJob cargoDeliveryJob;
      if (reader.TryStartClassDeserialization<CargoDeliveryJob>(out cargoDeliveryJob))
        reader.EnqueueDataDeserialization((object) cargoDeliveryJob, CargoDeliveryJob.s_deserializeDataDelayedAction);
      return cargoDeliveryJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CargoToDeliver = ProductQuantity.Deserialize(reader);
      this.m_amountOfCargoDelivered = reader.ReadNullableStruct<Quantity>();
      this.m_cargoTakenFromTruck = Quantity.Deserialize(reader);
      reader.RegisterResolvedMember<CargoDeliveryJob>(this, "m_factory", typeof (CargoDeliveryJob.Factory), true);
      reader.SetField<CargoDeliveryJob>(this, "m_inputBuffer", (object) RegisteredInputBuffer.Deserialize(reader));
      reader.SetField<CargoDeliveryJob>(this, "m_navigationJob", reader.LoadedSaveVersion >= 140 ? (object) reader.ReadGenericAs<IJobWithPreNavigation>() : (object) (IJobWithPreNavigation) null);
      reader.SetField<CargoDeliveryJob>(this, "m_secondaryBuffers", (object) Option<Lyst<SecondaryInputBufferSpec>>.Deserialize(reader));
      reader.SetField<CargoDeliveryJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<CargoDeliveryJob>(this, "m_truck", (object) Truck.Deserialize(reader));
    }

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public LocStrFormatted GoalName => this.m_inputBuffer.Entity.DefaultTitle;

    public ProductQuantity CargoToDeliver { get; private set; }

    bool IQueueTipJob.WaitBehindQueueTipVehicle => false;

    internal RegisteredInputBuffer InputBuffer => this.m_inputBuffer;

    private CargoDeliveryJob(
      VehicleJobId id,
      CargoDeliveryJob.Factory factory,
      Truck truck,
      ProductQuantity toDeliver,
      RegisteredInputBuffer inputBuffer,
      Option<Lyst<SecondaryInputBufferSpec>> secondaryBuffers)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      Assert.That<bool>(toDeliver.IsEmpty).IsFalse();
      this.CargoToDeliver = toDeliver;
      this.m_truck = truck.CheckNotNull<Truck>();
      this.m_inputBuffer = inputBuffer.CheckNotNull<RegisteredInputBuffer>();
      this.m_secondaryBuffers = secondaryBuffers;
      Assert.That<bool>(this.m_inputBuffer.Reserve((ICargoDeliveryJob) this)).IsTrue();
      if (this.m_secondaryBuffers.HasValue)
      {
        foreach (SecondaryInputBufferSpec secondaryInputBufferSpec in this.m_secondaryBuffers.Value)
          secondaryInputBufferSpec.Buffer.Reserve((ICargoDeliveryJob) this, secondaryInputBufferSpec.Quantity);
      }
      VehicleQueue<Vehicle, IStaticEntity> queue;
      if (inputBuffer.Entity is IStaticEntityWithQueue entity && entity.TryGetVehicleQueueFor((Vehicle) truck, out queue))
      {
        Assert.That<bool>(queue.IsEnabled).IsTrue();
        VehicleQueueJob<Vehicle> staticOwnedQueue = this.m_factory.QueueingJobFactory.CreateJobForStaticOwnedQueue<IStaticEntity, Vehicle>((Vehicle) truck, queue, navigateClosebyIsSufficient: inputBuffer.AllowDeliveryAtDistanceWhenBlocked);
        this.m_navigationJob = (IJobWithPreNavigation) staticOwnedQueue;
        staticOwnedQueue.DoJobAtQueueTipAndLeave((IQueueTipJob) this);
        truck.EnqueueJob((VehicleJob) staticOwnedQueue, false);
      }
      else
        this.m_navigationJob = (IJobWithPreNavigation) this.m_factory.NavigateToJobFactory.EnqueueJob((Vehicle) truck, (IVehicleGoalFull) this.m_factory.StaticEntityGoalFactory.Create(inputBuffer.Entity), inputBuffer.AllowDeliveryAtDistanceWhenBlocked);
      this.m_truck.EnqueueJob((VehicleJob) this, false);
    }

    public override LocStrFormatted JobInfo
    {
      get
      {
        if (this.m_inputBuffer == null)
          return LocStrFormatted.Empty;
        return new LocStrFormatted("Delivering {0} to {1}.".FormatInvariant((object) this.CargoToDeliver.FormatNumberAndUnitOnly(), (object) this.m_inputBuffer.Entity.Prototype.Strings.Name));
      }
    }

    public RobustNavHelper StartNavigation() => this.m_navigationJob.StartNavigation();

    /// <summary>Returns how much was removed.</summary>
    /// <param name="shouldJobBeCanceled">Whether this job should be canceled after the removal was done.</param>
    public Quantity TryRemoveQuantityForQuickDeliver(
      Quantity maxToRemove,
      RegisteredInputBuffer buffer,
      out bool shouldJobBeCanceled)
    {
      if (this.m_truck.Cargo.GetQuantityOf(buffer.Product).IsNotPositive)
      {
        shouldJobBeCanceled = true;
        return Quantity.Zero;
      }
      if (this.InputBuffer == buffer)
      {
        if (this.m_amountOfCargoDelivered.HasValue)
        {
          shouldJobBeCanceled = false;
          return Quantity.Zero;
        }
        Quantity quantity = maxToRemove.Min(this.CargoToDeliver.Quantity);
        ProductQuantity cargo = this.m_truck.TakeCargo(buffer.Product.WithQuantity(quantity));
        buffer.CancelReservation((ICargoDeliveryJob) this);
        this.CargoToDeliver = this.CargoToDeliver.WithNewQuantity(Quantity.Zero);
        shouldJobBeCanceled = this.CargoToDeliver.IsEmpty && (this.m_secondaryBuffers.IsNone || this.m_secondaryBuffers.Value.IsEmpty);
        return cargo.Quantity;
      }
      SecondaryInputBufferSpec? nullable = new SecondaryInputBufferSpec?();
      if (this.m_secondaryBuffers.HasValue)
      {
        foreach (SecondaryInputBufferSpec secondaryInputBufferSpec in this.m_secondaryBuffers.Value)
        {
          if (secondaryInputBufferSpec.Buffer == buffer)
          {
            nullable = new SecondaryInputBufferSpec?(secondaryInputBufferSpec);
            break;
          }
        }
      }
      if (!nullable.HasValue)
      {
        shouldJobBeCanceled = false;
        return Quantity.Zero;
      }
      this.m_secondaryBuffers.Value.Remove(nullable.Value);
      buffer.CancelReservation((ICargoDeliveryJob) this, nullable.Value.Quantity);
      Quantity quantity1 = maxToRemove.Min(nullable.Value.Quantity);
      ProductQuantity cargo1 = this.m_truck.TakeCargo(buffer.Product.WithQuantity(quantity1));
      shouldJobBeCanceled = this.CargoToDeliver.IsEmpty && (this.m_secondaryBuffers.IsNone || this.m_secondaryBuffers.Value.IsEmpty);
      return cargo1.Quantity;
    }

    protected override bool DoJobInternal()
    {
      Assert.That<bool>(this.m_truck.IsDriving).IsFalse();
      if (this.CargoToDeliver.IsEmpty)
      {
        Assert.That<Option<Lyst<SecondaryInputBufferSpec>>>(this.m_secondaryBuffers).HasValue<Lyst<SecondaryInputBufferSpec>>();
        Assert.That<bool>(!this.m_amountOfCargoDelivered.HasValue).IsTrue();
        Assert.That<bool>(this.CargoToDeliver.Product.IsNotPhantom).IsTrue();
        this.m_factory.VehicleJobStatsManager.RecordJobStatsFor(this.m_truck, this.CargoToDeliver.Product.WithQuantity(this.deliverToSecondaryBuffers()));
        this.InvokeJobDone();
        return false;
      }
      if (!this.m_amountOfCargoDelivered.HasValue)
      {
        Assert.That<Quantity>(this.m_truck.Cargo.TotalQuantity).IsPositive("Truck does not have any cargo");
        Assert.That<bool>(this.m_truck.Cargo.HasProduct(this.CargoToDeliver.Product)).IsTrue("Truck doesn't have correct product.");
        this.m_amountOfCargoDelivered = new Quantity?(this.m_inputBuffer.ReceiveCargo((ICargoDeliveryJob) this, this.m_truck.Cargo.GetQuantityOf(this.CargoToDeliver.Product), this.CargoToDeliver.Quantity));
        Quantity? ofCargoDelivered = this.m_amountOfCargoDelivered;
        Quantity zero = Quantity.Zero;
        if ((ofCargoDelivered.HasValue ? (ofCargoDelivered.GetValueOrDefault() == zero ? 1 : 0) : 0) != 0)
        {
          Quantity secondaryBuffers = this.deliverToSecondaryBuffers();
          if (secondaryBuffers.IsPositive)
            this.m_factory.VehicleJobStatsManager.RecordJobStatsFor(this.m_truck, this.CargoToDeliver.Product.WithQuantity(secondaryBuffers));
          this.InvokeJobDone();
          return false;
        }
        Assert.That<Quantity>(this.m_amountOfCargoDelivered.Value).IsLessOrEqual(this.m_truck.Cargo.GetQuantityOf(this.CargoToDeliver.Product), "Delivery job promised more cargo than truck has.");
        this.m_timer.Start(this.m_truck.Prototype.CargoPickupDuration);
        return true;
      }
      if (this.m_timer.Decrement())
      {
        Assert.That<Quantity>(this.m_truck.Cargo.GetQuantityOf(this.CargoToDeliver.Product)).IsGreaterOrEqual(this.m_amountOfCargoDelivered.Value - this.m_cargoTakenFromTruck);
        int ticks = this.m_truck.Prototype.CargoPickupDuration.Ticks;
        int num = this.m_timer.Ticks.Ticks - 1;
        Quantity cargoTakenFromTruck = this.m_cargoTakenFromTruck;
        this.m_cargoTakenFromTruck = Quantity.Zero.Lerp(this.m_amountOfCargoDelivered.Value, ticks - num, ticks);
        if (this.m_cargoTakenFromTruck > cargoTakenFromTruck)
        {
          Quantity quantity = this.m_cargoTakenFromTruck - cargoTakenFromTruck;
          Assert.That<Quantity>(this.m_truck.TakeCargo(new ProductQuantity(this.CargoToDeliver.Product, quantity)).Quantity).IsEqualTo(quantity);
        }
        return true;
      }
      Assert.That<Quantity>(this.m_cargoTakenFromTruck).IsLessOrEqual(this.m_amountOfCargoDelivered.Value, "Not all goods were delivered up.");
      if (this.m_cargoTakenFromTruck < this.m_amountOfCargoDelivered.Value)
      {
        Quantity quantity = this.m_amountOfCargoDelivered.Value - this.m_cargoTakenFromTruck;
        Assert.That<Quantity>(this.m_truck.TakeCargo(new ProductQuantity(this.CargoToDeliver.Product, quantity)).Quantity).IsEqualTo(quantity);
      }
      this.m_factory.VehicleJobStatsManager.RecordJobStatsFor(this.m_truck, this.CargoToDeliver.Product.WithQuantity(this.m_amountOfCargoDelivered.Value + this.deliverToSecondaryBuffers()));
      this.InvokeJobDone();
      return false;
    }

    private Quantity deliverToSecondaryBuffers()
    {
      if (this.m_secondaryBuffers.IsNone)
        return Quantity.Zero;
      Quantity zero = Quantity.Zero;
      foreach (SecondaryInputBufferSpec secondaryInputBufferSpec in this.m_secondaryBuffers.Value)
      {
        if (secondaryInputBufferSpec.Buffer.IsEnabled)
        {
          Quantity maxQuantity = secondaryInputBufferSpec.Quantity.Min(this.m_truck.Cargo.GetQuantityOf(secondaryInputBufferSpec.Buffer.Product));
          if (maxQuantity.IsNotPositive)
          {
            secondaryInputBufferSpec.Buffer.CancelReservation((ICargoDeliveryJob) this, secondaryInputBufferSpec.Quantity);
          }
          else
          {
            Quantity cargo = secondaryInputBufferSpec.Buffer.ReceiveCargo((ICargoDeliveryJob) this, maxQuantity, secondaryInputBufferSpec.Quantity);
            Assert.That<Quantity>(this.m_truck.TakeCargo(new ProductQuantity(secondaryInputBufferSpec.Buffer.Product, cargo)).Quantity).IsEqualTo(cargo);
            zero += cargo;
          }
        }
        else
          secondaryInputBufferSpec.Buffer.CancelReservation((ICargoDeliveryJob) this, secondaryInputBufferSpec.Quantity);
      }
      return zero;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      if (!this.CargoToDeliver.IsEmpty)
      {
        if (!this.m_amountOfCargoDelivered.HasValue)
        {
          this.m_inputBuffer.CancelReservation((ICargoDeliveryJob) this);
        }
        else
        {
          this.m_factory.VehicleJobStatsManager.RecordJobStatsFor(this.m_truck, this.CargoToDeliver.Product.WithQuantity(this.m_amountOfCargoDelivered.Value));
          Quantity quantity = this.m_amountOfCargoDelivered.Value - this.m_cargoTakenFromTruck;
          if (quantity.IsPositive)
            Assert.That<Quantity>(this.m_truck.TakeCargo(new ProductQuantity(this.CargoToDeliver.Product, quantity)).Quantity).IsEqualTo(quantity);
        }
      }
      if (this.m_secondaryBuffers.HasValue)
      {
        foreach (SecondaryInputBufferSpec secondaryInputBufferSpec in this.m_secondaryBuffers.Value)
          secondaryInputBufferSpec.Buffer.CancelReservation((ICargoDeliveryJob) this, secondaryInputBufferSpec.Quantity);
      }
      this.InvokeJobDone();
      ((IVehicleFriend) this.m_truck).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      this.CargoToDeliver = ProductQuantity.None;
      this.m_cargoTakenFromTruck = Quantity.Zero;
      this.m_amountOfCargoDelivered = new Quantity?();
      this.m_timer.Reset();
    }

    static CargoDeliveryJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoDeliveryJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      CargoDeliveryJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      public readonly VehicleQueueJobFactory QueueingJobFactory;
      public readonly NavigateToJob.Factory NavigateToJobFactory;
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly StaticEntityVehicleGoal.Factory StaticEntityGoalFactory;
      public readonly VehicleJobStatsManager VehicleJobStatsManager;

      public Factory(
        VehicleQueueJobFactory queueingJobFactory,
        NavigateToJob.Factory navigateToJobFactory,
        VehicleJobId.Factory vehicleJobIdFactory,
        StaticEntityVehicleGoal.Factory staticEntityGoalFactory,
        VehicleJobStatsManager vehicleJobStatsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.QueueingJobFactory = queueingJobFactory;
        this.NavigateToJobFactory = navigateToJobFactory;
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.StaticEntityGoalFactory = staticEntityGoalFactory;
        this.VehicleJobStatsManager = vehicleJobStatsManager;
      }

      public CargoDeliveryJob EnqueueJob(
        Truck truck,
        ProductQuantity toDeliver,
        RegisteredInputBuffer inputBuffer,
        Lyst<SecondaryInputBufferSpec> secondaryBuffers = null)
      {
        return new CargoDeliveryJob(this.m_vehicleJobIdFactory.GetNextId(), this, truck, toDeliver, inputBuffer, (Option<Lyst<SecondaryInputBufferSpec>>) secondaryBuffers);
      }
    }
  }
}
