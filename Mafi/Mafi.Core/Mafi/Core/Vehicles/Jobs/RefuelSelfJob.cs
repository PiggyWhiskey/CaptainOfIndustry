// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.RefuelSelfJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Stats;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job that drives vehicle to a given <see cref="T:Mafi.Core.Buildings.FuelStations.FuelStation" /> and refuels its tank there.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class RefuelSelfJob : 
    VehicleJob,
    IQueueTipJob,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey,
    ICargoPickUpJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>
    /// Duration of refueling in a <see cref="T:Mafi.Core.Buildings.FuelStations.FuelStation" />.
    /// </summary>
    private static readonly Duration REFUELLING_DURATION;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly RegisteredOutputBuffer m_outputBuffer;
    private Quantity m_reservedFuel;
    private readonly TickTimer m_timer;
    private readonly RefuelSelfJob.Factory m_factory;

    public static void Serialize(RefuelSelfJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RefuelSelfJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RefuelSelfJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ProductQuantity.Serialize(this.CargoToPickup, writer);
      RegisteredOutputBuffer.Serialize(this.m_outputBuffer, writer);
      Quantity.Serialize(this.m_reservedFuel, writer);
      TickTimer.Serialize(this.m_timer, writer);
      writer.WriteGeneric<Vehicle>(this.Vehicle);
    }

    public static RefuelSelfJob Deserialize(BlobReader reader)
    {
      RefuelSelfJob refuelSelfJob;
      if (reader.TryStartClassDeserialization<RefuelSelfJob>(out refuelSelfJob))
        reader.EnqueueDataDeserialization((object) refuelSelfJob, RefuelSelfJob.s_deserializeDataDelayedAction);
      return refuelSelfJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CargoToPickup = ProductQuantity.Deserialize(reader);
      reader.RegisterResolvedMember<RefuelSelfJob>(this, "m_factory", typeof (RefuelSelfJob.Factory), true);
      reader.SetField<RefuelSelfJob>(this, "m_outputBuffer", (object) RegisteredOutputBuffer.Deserialize(reader));
      this.m_reservedFuel = Quantity.Deserialize(reader);
      reader.SetField<RefuelSelfJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      this.Vehicle = reader.ReadGenericAs<Vehicle>();
    }

    public override LocStrFormatted JobInfo => RefuelSelfJob.s_jobInfo;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Idle;

    bool IQueueTipJob.WaitBehindQueueTipVehicle => false;

    public ProductQuantity CargoToPickup { get; private set; }

    public Vehicle Vehicle { get; private set; }

    internal RefuelSelfJob(
      VehicleJobId id,
      RefuelSelfJob.Factory factory,
      Vehicle vehicle,
      RegisteredOutputBuffer outputBuffer)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory.CheckNotNull<RefuelSelfJob.Factory>();
      this.CargoToPickup = outputBuffer.Product.WithQuantity(vehicle.FuelTank.Value.GetFreeCapacity());
      this.Vehicle = vehicle.CheckNotNull<Vehicle>();
      Assert.That<Option<IFuelTankReadonly>>(vehicle.FuelTank).HasValue<IFuelTankReadonly>();
      Assert.That<ProductProto>(this.Vehicle.FuelTank.Value.Proto.Product).IsEqualTo<ProductProto>(outputBuffer.Product);
      this.m_outputBuffer = outputBuffer.CheckNotNull<RegisteredOutputBuffer>();
      this.m_outputBuffer.Reserve((ICargoPickUpJob) this);
      VehicleQueue<Vehicle, IStaticEntity> queue;
      if (outputBuffer.Entity is IStaticEntityWithQueue entity && entity.TryGetVehicleQueueFor(vehicle, out queue))
      {
        VehicleQueueJob<Vehicle> staticOwnedQueue = this.m_factory.QueueingJobFactory.CreateJobForStaticOwnedQueue<IStaticEntity, Vehicle>(vehicle, queue);
        staticOwnedQueue.DoJobAtQueueTipAndLeave((IQueueTipJob) this);
        this.Vehicle.EnqueueJob((VehicleJob) staticOwnedQueue);
      }
      else
        this.m_factory.NavigateToJobFactory.EnqueueJob(vehicle, (IVehicleGoalFull) this.m_factory.StaticEntityGoalFactory.Create(outputBuffer.Entity));
      this.Vehicle.EnqueueJob((VehicleJob) this);
      this.m_timer.Start(RefuelSelfJob.REFUELLING_DURATION);
    }

    protected override bool DoJobInternal()
    {
      if (this.m_timer.Decrement())
        return true;
      Quantity cargo = this.m_outputBuffer.GetCargo((ICargoPickUpJob) this, this.CargoToPickup.Quantity, this.CargoToPickup.Quantity);
      if (cargo.IsPositive)
      {
        this.Vehicle.AddFuelExactly(new ProductQuantity(this.m_outputBuffer.Product, cargo));
        this.m_factory.FuelStatsCollector.ReportFuelUseAndDestroy(this.m_outputBuffer.Product, cargo, FuelUsedBy.Vehicle);
      }
      Quantity freeCapacity = this.Vehicle.FuelTank.Value.GetFreeCapacity();
      if (freeCapacity.IsPositive && this.m_outputBuffer.AvailableQuantity.IsPositive)
      {
        Quantity quantity = this.m_outputBuffer.Buffer.RemoveAsMuchAs(freeCapacity);
        this.Vehicle.AddFuelExactly(new ProductQuantity(this.m_outputBuffer.Product, quantity));
        this.m_factory.FuelStatsCollector.ReportFuelUseAndDestroy(this.m_outputBuffer.Product, quantity, FuelUsedBy.Vehicle);
      }
      this.InvokeJobDone();
      return false;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_outputBuffer.CancelReservation((ICargoPickUpJob) this);
      this.InvokeJobDone();
      ((IVehicleFriend) this.Vehicle).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      this.m_reservedFuel = Quantity.Zero;
      this.m_timer.Reset();
    }

    static RefuelSelfJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RefuelSelfJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      RefuelSelfJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      RefuelSelfJob.REFUELLING_DURATION = 5.Seconds();
      RefuelSelfJob.s_jobInfo = new LocStrFormatted("Refueling.");
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      public readonly VehicleQueueJobFactory QueueingJobFactory;
      public readonly NavigateToJob.Factory NavigateToJobFactory;
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly IFuelStatsCollector FuelStatsCollector;
      public readonly StaticEntityVehicleGoal.Factory StaticEntityGoalFactory;

      public Factory(
        VehicleQueueJobFactory queueingJobFactory,
        NavigateToJob.Factory navigateToJobFactory,
        VehicleJobId.Factory vehicleJobIdFactory,
        IFuelStatsCollector fuelStatsCollector,
        StaticEntityVehicleGoal.Factory staticEntityGoalFactory)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.FuelStatsCollector = fuelStatsCollector;
        this.QueueingJobFactory = queueingJobFactory;
        this.NavigateToJobFactory = navigateToJobFactory;
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.StaticEntityGoalFactory = staticEntityGoalFactory;
      }

      public void EnqueueJob(Vehicle vehicle, RegisteredOutputBuffer outputBuffer)
      {
        RefuelSelfJob refuelSelfJob = new RefuelSelfJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicle, outputBuffer);
      }
    }
  }
}
