// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.MixedCargoDeliveryJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job in which a vehicle delivers mixed cargo to an ore sorting plant.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class MixedCargoDeliveryJob : 
    VehicleJob,
    IQueueTipJob,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly MixedCargoDeliveryJob.Factory m_factory;
    private readonly OreSortingPlant m_targetPlant;

    public static void Serialize(MixedCargoDeliveryJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MixedCargoDeliveryJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MixedCargoDeliveryJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      OreSortingPlant.Serialize(this.m_targetPlant, writer);
      Truck.Serialize(this.Truck, writer);
    }

    public static MixedCargoDeliveryJob Deserialize(BlobReader reader)
    {
      MixedCargoDeliveryJob cargoDeliveryJob;
      if (reader.TryStartClassDeserialization<MixedCargoDeliveryJob>(out cargoDeliveryJob))
        reader.EnqueueDataDeserialization((object) cargoDeliveryJob, MixedCargoDeliveryJob.s_deserializeDataDelayedAction);
      return cargoDeliveryJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.RegisterResolvedMember<MixedCargoDeliveryJob>(this, "m_factory", typeof (MixedCargoDeliveryJob.Factory), true);
      reader.SetField<MixedCargoDeliveryJob>(this, "m_targetPlant", (object) OreSortingPlant.Deserialize(reader));
      this.Truck = Truck.Deserialize(reader);
    }

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public override LocStrFormatted JobInfo
    {
      get
      {
        return new LocStrFormatted("Delivering to {0}.".FormatInvariant((object) this.m_targetPlant.Prototype.Strings.Name));
      }
    }

    bool IQueueTipJob.WaitBehindQueueTipVehicle => false;

    public Truck Truck { get; private set; }

    private MixedCargoDeliveryJob(
      VehicleJobId id,
      MixedCargoDeliveryJob.Factory factory,
      Truck truck,
      OreSortingPlant targetPlant)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.Truck = truck.CheckNotNull<Truck>();
      this.m_targetPlant = targetPlant.CheckNotNull<OreSortingPlant>();
      Assert.That<bool>(this.m_targetPlant.Reserve(this)).IsTrue();
      VehicleQueue<Vehicle, IStaticEntity> queue;
      if (this.m_targetPlant.TryGetVehicleQueueFor((Vehicle) truck, out queue))
      {
        Assert.That<bool>(queue.IsEnabled).IsTrue();
        VehicleQueueJob<Vehicle> staticOwnedQueue = this.m_factory.QueueingJobFactory.CreateJobForStaticOwnedQueue<IStaticEntity, Vehicle>((Vehicle) truck, queue);
        staticOwnedQueue.DoJobAtQueueTipAndLeave((IQueueTipJob) this);
        truck.EnqueueJob((VehicleJob) staticOwnedQueue, false);
      }
      else
        this.m_factory.NavigateToJobFactory.EnqueueJob((Vehicle) truck, (IVehicleGoalFull) this.m_factory.StaticEntityGoalFactory.Create((IStaticEntity) this.m_targetPlant));
      this.Truck.EnqueueJob((VehicleJob) this, false);
    }

    protected override bool DoJobInternal()
    {
      Assert.That<bool>(this.Truck.IsDriving).IsFalse();
      Assert.That<Quantity>(this.Truck.Cargo.TotalQuantity).IsPositive("Truck does not have any cargo");
      this.m_targetPlant.TakeProductsAndRemoveReservation(this, this.m_factory.VehicleJobStatsManager);
      this.InvokeJobDone();
      return false;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_targetPlant.CancelReservation(this);
      this.InvokeJobDone();
      ((IVehicleFriend) this.Truck).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
    }

    static MixedCargoDeliveryJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MixedCargoDeliveryJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      MixedCargoDeliveryJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
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

      public MixedCargoDeliveryJob EnqueueJob(Truck truck, OreSortingPlant targetPlant)
      {
        return new MixedCargoDeliveryJob(this.m_vehicleJobIdFactory.GetNextId(), this, truck, targetPlant);
      }
    }
  }
}
