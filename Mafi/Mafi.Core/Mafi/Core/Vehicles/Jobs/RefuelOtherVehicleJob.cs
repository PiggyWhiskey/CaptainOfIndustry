// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.RefuelOtherVehicleJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Stats;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Job for refueling other vehicle using fuel we have as cargo. This job expects to be executed once we (the vehicle
  /// executing it) have cargo usable as fuel for the vehicle to be refueled and we are reasonably close to the vehicle
  /// being refueled. The job pauses navigation of the vehicle being refueled, navigates to it, refuels it and resumes
  /// its navigation.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class RefuelOtherVehicleJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Duration REFUELING_DURATION;
    private static readonly RelTile1i EXTRA_TOLERANCE_PER_RETRY;
    private static readonly string s_jobInfo;
    private readonly TickTimer m_timer;
    private readonly Truck m_truckWithFuel;
    private RefuelOtherVehicleJob.State m_state;
    private readonly FuelStationsManager m_fuelStationsManager;
    private readonly IFuelStatsCollector m_fuelStatsCollector;
    private readonly DynamicEntityVehicleGoal.Factory m_dynamicEntityGoalFactory;
    private readonly VehicleJobStatsManager m_vehicleJobStatsManager;
    private readonly RobustNavHelper m_navHelper;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<Vehicle> m_vehiclesCache;

    public static void Serialize(RefuelOtherVehicleJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RefuelOtherVehicleJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RefuelOtherVehicleJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      DynamicEntityVehicleGoal.Factory.Serialize(this.m_dynamicEntityGoalFactory, writer);
      FuelStationsManager.Serialize(this.m_fuelStationsManager, writer);
      writer.WriteGeneric<IFuelStatsCollector>(this.m_fuelStatsCollector);
      RobustNavHelper.Serialize(this.m_navHelper, writer);
      writer.WriteInt((int) this.m_state);
      TickTimer.Serialize(this.m_timer, writer);
      Truck.Serialize(this.m_truckWithFuel, writer);
      VehicleJobStatsManager.Serialize(this.m_vehicleJobStatsManager, writer);
      writer.WriteGeneric<Vehicle>(this.VehicleToRefuel);
    }

    public static RefuelOtherVehicleJob Deserialize(BlobReader reader)
    {
      RefuelOtherVehicleJob refuelOtherVehicleJob;
      if (reader.TryStartClassDeserialization<RefuelOtherVehicleJob>(out refuelOtherVehicleJob))
        reader.EnqueueDataDeserialization((object) refuelOtherVehicleJob, RefuelOtherVehicleJob.s_deserializeDataDelayedAction);
      return refuelOtherVehicleJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RefuelOtherVehicleJob>(this, "m_dynamicEntityGoalFactory", (object) DynamicEntityVehicleGoal.Factory.Deserialize(reader));
      reader.SetField<RefuelOtherVehicleJob>(this, "m_fuelStationsManager", (object) FuelStationsManager.Deserialize(reader));
      reader.SetField<RefuelOtherVehicleJob>(this, "m_fuelStatsCollector", (object) reader.ReadGenericAs<IFuelStatsCollector>());
      reader.SetField<RefuelOtherVehicleJob>(this, "m_navHelper", (object) RobustNavHelper.Deserialize(reader));
      this.m_state = (RefuelOtherVehicleJob.State) reader.ReadInt();
      reader.SetField<RefuelOtherVehicleJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<RefuelOtherVehicleJob>(this, "m_truckWithFuel", (object) Truck.Deserialize(reader));
      reader.SetField<RefuelOtherVehicleJob>(this, "m_vehicleJobStatsManager", (object) VehicleJobStatsManager.Deserialize(reader));
      reader.SetField<RefuelOtherVehicleJob>(this, "m_vehiclesCache", (object) new Lyst<Vehicle>());
      this.VehicleToRefuel = reader.ReadGenericAs<Vehicle>();
    }

    public override LocStrFormatted JobInfo
    {
      get
      {
        return new LocStrFormatted(RefuelOtherVehicleJob.s_jobInfo.FormatInvariant((object) this.VehicleToRefuel.Prototype.Id.Value));
      }
    }

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.Full;

    public Vehicle VehicleToRefuel { get; private set; }

    public RefuelOtherVehicleJob(
      VehicleJobId id,
      FuelStationsManager fuelStationsManager,
      IVehiclePathFindingManager pathFindingManager,
      IFuelStatsCollector fuelStatsCollector,
      DynamicEntityVehicleGoal.Factory dynamicEntityGoalFactory,
      VehicleJobStatsManager vehicleJobStatsManager,
      Truck truckWithFuel,
      Vehicle vehicleToRefuel)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      this.m_vehiclesCache = new Lyst<Vehicle>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_fuelStationsManager = fuelStationsManager;
      this.m_fuelStatsCollector = fuelStatsCollector;
      this.m_dynamicEntityGoalFactory = dynamicEntityGoalFactory;
      this.m_vehicleJobStatsManager = vehicleJobStatsManager;
      this.m_navHelper = new RobustNavHelper(pathFindingManager);
      this.m_truckWithFuel = truckWithFuel.CheckNotNull<Truck>();
      this.VehicleToRefuel = vehicleToRefuel.CheckNotNull<Vehicle>();
      this.m_state = RefuelOtherVehicleJob.State.Initialization;
    }

    protected override bool DoJobInternal()
    {
      switch (this.m_state)
      {
        case RefuelOtherVehicleJob.State.Initialization:
          this.m_state = this.handleInitialization();
          break;
        case RefuelOtherVehicleJob.State.Navigation:
          this.m_state = this.handleNavigation();
          break;
        case RefuelOtherVehicleJob.State.Waiting:
          this.m_state = this.handleWaiting();
          break;
        case RefuelOtherVehicleJob.State.Refueling:
          this.m_state = this.handleRefueling();
          break;
        default:
          Log.Error(string.Format("Invalid RefuelOtherVehicleJob state: {0}", (object) this.m_state));
          this.m_state = RefuelOtherVehicleJob.State.Done;
          break;
      }
      return this.m_state != RefuelOtherVehicleJob.State.Done;
    }

    private RefuelOtherVehicleJob.State handleInitialization()
    {
      this.m_navHelper.StartNavigationTo((Vehicle) this.m_truckWithFuel, (IVehicleGoalFull) this.m_dynamicEntityGoalFactory.Create(this.VehicleToRefuel), 2, new RelTile1i?(RefuelOtherVehicleJob.EXTRA_TOLERANCE_PER_RETRY));
      return RefuelOtherVehicleJob.State.Navigation;
    }

    private RefuelOtherVehicleJob.State handleNavigation()
    {
      if (this.m_truckWithFuel.DistanceSqrTo(this.VehicleToRefuel.Position2f) < (RefuelOtherVehicleJob.EXTRA_TOLERANCE_PER_RETRY + this.m_truckWithFuel.Prototype.NavTolerance).Squared)
      {
        this.m_truckWithFuel.StopNavigating();
        this.m_timer.Start(RefuelOtherVehicleJob.REFUELING_DURATION);
        return RefuelOtherVehicleJob.State.Waiting;
      }
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          return RefuelOtherVehicleJob.State.Navigation;
        case RobustNavResult.GoalReachedSuccessfully:
          this.m_timer.Start(RefuelOtherVehicleJob.REFUELING_DURATION);
          return RefuelOtherVehicleJob.State.Waiting;
        case RobustNavResult.FailGoalUnreachable:
          this.RequestCancel();
          return RefuelOtherVehicleJob.State.Done;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          return RefuelOtherVehicleJob.State.Done;
      }
    }

    private RefuelOtherVehicleJob.State handleWaiting()
    {
      return this.m_timer.Decrement() ? RefuelOtherVehicleJob.State.Waiting : RefuelOtherVehicleJob.State.Refueling;
    }

    private RefuelOtherVehicleJob.State handleRefueling()
    {
      ProductQuantity firstOrPhantom = this.m_truckWithFuel.Cargo.FirstOrPhantom;
      this.refuelVehicle(this.VehicleToRefuel);
      this.m_fuelStationsManager.RefuelOtherVehicleJobCompleted(this);
      if (this.m_truckWithFuel.Cargo.IsEmpty)
      {
        this.m_vehicleJobStatsManager.RecordRefuelingJobStats(firstOrPhantom);
        return RefuelOtherVehicleJob.State.Done;
      }
      this.m_fuelStationsManager.GetNearbyVehiclesToRefuel(this.m_truckWithFuel, this.VehicleToRefuel, this.m_vehiclesCache);
      foreach (Vehicle vehicleToRefuel in this.m_vehiclesCache)
      {
        if (!this.m_truckWithFuel.IsEmpty)
          this.refuelVehicle(vehicleToRefuel);
        else
          break;
      }
      this.m_vehiclesCache.Clear();
      this.m_vehicleJobStatsManager.RecordRefuelingJobStats(firstOrPhantom - this.m_truckWithFuel.Cargo.TotalQuantity);
      return RefuelOtherVehicleJob.State.Done;
    }

    private void refuelVehicle(Vehicle vehicleToRefuel)
    {
      ProductProto product1 = this.m_truckWithFuel.Cargo.FirstOrPhantom.Product;
      ProductProto productProto1 = product1;
      Option<IFuelTankReadonly> fuelTank = vehicleToRefuel.FuelTank;
      ProductProto product2 = fuelTank.ValueOrNull?.Proto.Product;
      if ((Proto) productProto1 != (Proto) product2)
      {
        ProductProto productProto2 = product1;
        fuelTank = vehicleToRefuel.FuelTank;
        ProductProto product3 = fuelTank.ValueOrNull?.Proto.Product;
        Log.Error("Trying to refuel vehicle with incompatible fuel " + string.Format("'{0}' to '{1}'", (object) productProto2, (object) product3));
      }
      else
      {
        Quantity quantity = this.m_truckWithFuel.Cargo.TotalQuantity - vehicleToRefuel.AddFuelAsMuchAs(this.m_truckWithFuel.Cargo.FirstOrPhantom);
        this.m_fuelStatsCollector.ReportFuelUseAndDestroy(product1, quantity, FuelUsedBy.Vehicle);
        this.m_truckWithFuel.TakeCargo(new ProductQuantity(product1, quantity));
      }
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_navHelper.CancelAndClear();
      this.m_fuelStationsManager.RefuelOtherVehicleJobCancelled(this);
      ((IVehicleFriend) this.m_truckWithFuel).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy() => this.m_navHelper.Clear();

    static RefuelOtherVehicleJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RefuelOtherVehicleJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      RefuelOtherVehicleJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      RefuelOtherVehicleJob.REFUELING_DURATION = 1.Seconds();
      RefuelOtherVehicleJob.EXTRA_TOLERANCE_PER_RETRY = new RelTile1i(5);
      RefuelOtherVehicleJob.s_jobInfo = "Refueling vehicle: '{0}'.";
    }

    private enum State
    {
      Initialization,
      Navigation,
      Waiting,
      Refueling,
      Done,
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly LazyResolve<FuelStationsManager> m_fuelStationsManager;
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      private readonly IVehiclePathFindingManager m_pathFindingManager;
      private readonly IFuelStatsCollector m_fuelStatsCollector;
      private readonly VehicleJobStatsManager m_jobStatsManager;
      private readonly DynamicEntityVehicleGoal.Factory m_dynamicEntityGoalFactory;

      public Factory(
        LazyResolve<FuelStationsManager> fuelStationsManager,
        VehicleJobId.Factory vehicleJobIdFactory,
        IVehiclePathFindingManager pathFindingManager,
        IFuelStatsCollector fuelStatsCollector,
        VehicleJobStatsManager jobStatsManager,
        DynamicEntityVehicleGoal.Factory dynamicEntityGoalFactory)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_fuelStationsManager = fuelStationsManager;
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.m_pathFindingManager = pathFindingManager;
        this.m_fuelStatsCollector = fuelStatsCollector;
        this.m_jobStatsManager = jobStatsManager;
        this.m_dynamicEntityGoalFactory = dynamicEntityGoalFactory;
      }

      public RefuelOtherVehicleJob EnqueueJob(Truck truckWithFuel, Vehicle vehicleToRefuel)
      {
        RefuelOtherVehicleJob job = new RefuelOtherVehicleJob(this.m_vehicleJobIdFactory.GetNextId(), this.m_fuelStationsManager.Value, this.m_pathFindingManager, this.m_fuelStatsCollector, this.m_dynamicEntityGoalFactory, this.m_jobStatsManager, truckWithFuel, vehicleToRefuel);
        truckWithFuel.EnqueueJob((VehicleJob) job, false);
        return job;
      }
    }
  }
}
