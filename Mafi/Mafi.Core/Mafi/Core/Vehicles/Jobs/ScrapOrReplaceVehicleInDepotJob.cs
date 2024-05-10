// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.ScrapOrReplaceVehicleInDepotJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>Drives vehicle to the depot and despawns it there.</summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class ScrapOrReplaceVehicleInDepotJob : 
    VehicleJob,
    IDepotJob,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey,
    IQueueTipJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Duration WAIT_DURATION_PER_NAV_ATTEMPT;
    private static readonly LocStrFormatted s_jobInfo;
    private readonly Vehicle m_vehicle;
    private readonly VehicleDepotBase m_depot;
    private ScrapOrReplaceVehicleInDepotJob.State m_state;
    private int m_navAttemptsPerformed;
    private Duration m_waitDuration;
    private readonly ScrapOrReplaceVehicleInDepotJob.Factory m_factory;
    private readonly QueueJobResultRef m_navResult;

    public static void Serialize(ScrapOrReplaceVehicleInDepotJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ScrapOrReplaceVehicleInDepotJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ScrapOrReplaceVehicleInDepotJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<VehicleDepotBase>(this.m_depot);
      writer.WriteInt(this.m_navAttemptsPerformed);
      QueueJobResultRef.Serialize(this.m_navResult, writer);
      writer.WriteInt((int) this.m_state);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
      Duration.Serialize(this.m_waitDuration, writer);
    }

    public static ScrapOrReplaceVehicleInDepotJob Deserialize(BlobReader reader)
    {
      ScrapOrReplaceVehicleInDepotJob vehicleInDepotJob;
      if (reader.TryStartClassDeserialization<ScrapOrReplaceVehicleInDepotJob>(out vehicleInDepotJob))
        reader.EnqueueDataDeserialization((object) vehicleInDepotJob, ScrapOrReplaceVehicleInDepotJob.s_deserializeDataDelayedAction);
      return vehicleInDepotJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ScrapOrReplaceVehicleInDepotJob>(this, "m_depot", (object) reader.ReadGenericAs<VehicleDepotBase>());
      reader.RegisterResolvedMember<ScrapOrReplaceVehicleInDepotJob>(this, "m_factory", typeof (ScrapOrReplaceVehicleInDepotJob.Factory), true);
      this.m_navAttemptsPerformed = reader.ReadInt();
      reader.SetField<ScrapOrReplaceVehicleInDepotJob>(this, "m_navResult", (object) QueueJobResultRef.Deserialize(reader));
      this.m_state = (ScrapOrReplaceVehicleInDepotJob.State) reader.ReadInt();
      reader.SetField<ScrapOrReplaceVehicleInDepotJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
      this.m_waitDuration = Duration.Deserialize(reader);
    }

    public override LocStrFormatted JobInfo => ScrapOrReplaceVehicleInDepotJob.s_jobInfo;

    /// <summary>
    /// Go to depot job is not true job until the vehicle starts driving to the depot.
    /// </summary>
    /// <remarks>
    /// We do not want to assign it a new job when is driving to the depot because that would result in vehicle
    /// driving through the depot to its goal and ignoring doors.
    /// </remarks>
    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption
    {
      get
      {
        bool flag;
        switch (this.m_state)
        {
          case ScrapOrReplaceVehicleInDepotJob.State.NavDone:
          case ScrapOrReplaceVehicleInDepotJob.State.WaitingForDoors:
          case ScrapOrReplaceVehicleInDepotJob.State.DepotUnreachableWaiting:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        return !flag ? VehicleFuelConsumption.Full : VehicleFuelConsumption.Idle;
      }
    }

    public override bool SkipNoMovementMonitoring
    {
      get
      {
        return this.m_state == ScrapOrReplaceVehicleInDepotJob.State.WaitingForDoors && !this.m_depot.CanWork;
      }
    }

    bool IQueueTipJob.WaitBehindQueueTipVehicle => false;

    private ScrapOrReplaceVehicleInDepotJob(
      VehicleJobId id,
      ScrapOrReplaceVehicleInDepotJob.Factory factory,
      Vehicle vehicle,
      Option<DrivingEntityProto> replaceWith,
      VehicleDepotBase depot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_navResult = new QueueJobResultRef();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      if (replaceWith.IsNone)
      {
        Assert.That<Option<IEntityAssignedWithVehicles>>(vehicle.AssignedTo).IsNone<IEntityAssignedWithVehicles>("Sending assigned vehicle to the depot.");
        if (vehicle is IEntityAssignedWithVehicles assignedWithVehicles)
          Assert.That<int>(assignedWithVehicles.AllVehicles.Count).IsZero("Sending vehicle with assignees to the depot.");
      }
      this.m_vehicle = vehicle.CheckNotNull<Vehicle>();
      this.m_depot = depot.CheckNotNull<VehicleDepotBase>();
      this.m_state = ScrapOrReplaceVehicleInDepotJob.State.ScheduleNavJob;
      this.m_navAttemptsPerformed = 0;
      this.m_vehicle.AssertHasNoJobOfType<ScrapOrReplaceVehicleInDepotJob>();
      this.m_vehicle.EnqueueJob((VehicleJob) this);
      this.m_depot.AddJob((IDepotJob) this);
    }

    protected override bool DoJobInternal()
    {
      Assert.That<bool>(this.m_vehicle.IsSpawned).IsTrue();
      Assert.That<bool>(this.m_depot.IsDestroyed).IsFalse();
      switch (this.m_state)
      {
        case ScrapOrReplaceVehicleInDepotJob.State.ScheduleNavJob:
          this.m_navResult.ClearResult();
          VehicleQueueJob<Vehicle> staticOwnedQueue = this.m_factory.VehicleQueueJobFactory.CreateJobForStaticOwnedQueue<VehicleDepotBase, Vehicle>(this.m_vehicle, this.m_depot.VehicleQueue, useCustomTarget: true, doNotCancelAllJobsOnNavFailure: true, resultRef: (Option<QueueJobResultRef>) this.m_navResult);
          staticOwnedQueue.DoJobAtQueueTipAndLeave((IQueueTipJob) this);
          this.m_vehicle.EnqueueJob((VehicleJob) staticOwnedQueue, true);
          ++this.m_navAttemptsPerformed;
          this.m_state = ScrapOrReplaceVehicleInDepotJob.State.NavDone;
          break;
        case ScrapOrReplaceVehicleInDepotJob.State.NavDone:
          Assert.That<bool?>(this.m_navResult.ReachedGoal).HasValue<bool>();
          bool? reachedGoal = this.m_navResult.ReachedGoal;
          bool flag = true;
          if (reachedGoal.GetValueOrDefault() == flag & reachedGoal.HasValue)
          {
            this.m_state = ScrapOrReplaceVehicleInDepotJob.State.WaitingForDoors;
            break;
          }
          if (this.m_navAttemptsPerformed < 3)
          {
            this.m_waitDuration = this.m_navAttemptsPerformed * ScrapOrReplaceVehicleInDepotJob.WAIT_DURATION_PER_NAV_ATTEMPT;
            this.m_state = ScrapOrReplaceVehicleInDepotJob.State.DepotUnreachableWaiting;
            break;
          }
          goto case ScrapOrReplaceVehicleInDepotJob.State.Despawn;
        case ScrapOrReplaceVehicleInDepotJob.State.WaitingForDoors:
          if (this.m_depot.WaitForOpenDoors())
          {
            this.m_state = ScrapOrReplaceVehicleInDepotJob.State.DrivingToDepot;
            this.m_vehicle.SetDrivingTarget(this.m_depot.DespawnPosition, true);
            break;
          }
          break;
        case ScrapOrReplaceVehicleInDepotJob.State.DrivingToDepot:
          this.m_depot.WaitForOpenDoors().AssertTrue();
          if (!this.m_vehicle.IsDriving)
          {
            this.m_state = ScrapOrReplaceVehicleInDepotJob.State.WaitingForClosedDoors;
            break;
          }
          break;
        case ScrapOrReplaceVehicleInDepotJob.State.DepotUnreachableWaiting:
          this.m_waitDuration -= Duration.OneTick;
          if (this.m_waitDuration.IsNotPositive)
          {
            this.m_state = ScrapOrReplaceVehicleInDepotJob.State.ScheduleNavJob;
            break;
          }
          break;
        case ScrapOrReplaceVehicleInDepotJob.State.Despawn:
          if (this.m_vehicle.AssignedTo.HasValue)
          {
            Assert.That<bool>(this.m_vehicle.IsOnWayToDepotForReplacement).IsTrue();
            this.m_depot.SetNextSpawnAssignment(this.m_vehicle.AssignedTo.Value);
            this.m_vehicle.UnassignFrom(this.m_vehicle.AssignedTo.Value, false);
          }
          if (this.m_vehicle is IEntityAssignedWithVehicles vehicle && vehicle.AllVehicles.Count > 0)
          {
            Assert.That<bool>(this.m_vehicle.IsOnWayToDepotForReplacement).IsTrue();
            this.m_depot.SetNextSpawnAssignees(vehicle.AllVehicles);
            foreach (Vehicle immutable in vehicle.AllVehicles.ToImmutableArray<Vehicle>())
              immutable.UnassignFrom(vehicle, false);
          }
          this.m_factory.VehiclesManager.Value.DestroyVehicle(this.m_vehicle);
          this.m_depot.JobDone((IDepotJob) this);
          this.InvokeJobDone();
          this.m_state = ScrapOrReplaceVehicleInDepotJob.State.Done;
          AssetValue assetValue = this.m_vehicle.Prototype.Costs.Price.Apply(this.m_factory.ConstructionManager.DeconstructionRatio);
          if (this.m_vehicle.FuelTank.HasValue)
          {
            FuelTank.TankInfo info = this.m_vehicle.FuelTank.Value.Info;
            if (info.Quantity.IsPositive)
              assetValue += new AssetValue(info.Product, info.Quantity);
          }
          this.m_depot.StoreMaterialsFromVehicleScrap(assetValue);
          return false;
        case ScrapOrReplaceVehicleInDepotJob.State.WaitingForClosedDoors:
          if (this.m_depot.WaitForClosedDoors())
          {
            this.m_state = ScrapOrReplaceVehicleInDepotJob.State.Despawn;
            break;
          }
          break;
        default:
          Log.Error(string.Format("Invalid state {0}", (object) this.m_state));
          goto case ScrapOrReplaceVehicleInDepotJob.State.Despawn;
      }
      return true;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      if (this.m_state == ScrapOrReplaceVehicleInDepotJob.State.DrivingToDepot)
        return 1.Minutes();
      this.m_depot.JobCanceled((IDepotJob) this);
      this.InvokeJobDone();
      this.m_state = ScrapOrReplaceVehicleInDepotJob.State.Done;
      if (this.m_depot.IsDestroyed || this.m_depot.DestroyCallbackStarted)
      {
        if (this.m_vehicle.IsOnWayToDepotForScrap)
          this.m_vehicle.CancelScrap();
        if (this.m_vehicle.IsOnWayToDepotForReplacement)
          this.m_vehicle.CancelReplaceEnRoute();
      }
      ((IVehicleFriend) this.m_vehicle).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      if (this.m_state != ScrapOrReplaceVehicleInDepotJob.State.Done)
      {
        this.m_depot?.JobCanceled((IDepotJob) this);
        this.InvokeJobDone();
        this.m_state = ScrapOrReplaceVehicleInDepotJob.State.Done;
        this.m_vehicle.CancelScrap();
        this.m_vehicle.CancelReplaceEnRoute();
      }
      this.m_navResult.ClearResult();
    }

    static ScrapOrReplaceVehicleInDepotJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ScrapOrReplaceVehicleInDepotJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      ScrapOrReplaceVehicleInDepotJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      ScrapOrReplaceVehicleInDepotJob.WAIT_DURATION_PER_NAV_ATTEMPT = 10.Seconds();
      ScrapOrReplaceVehicleInDepotJob.s_jobInfo = new LocStrFormatted("Going to the vehicle depot to get scrapped.");
    }

    private enum State
    {
      ScheduleNavJob,
      NavDone,
      WaitingForDoors,
      DrivingToDepot,
      DepotUnreachableWaiting,
      Despawn,
      Done,
      WaitingForClosedDoors,
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      public readonly VehicleQueueJobFactory VehicleQueueJobFactory;
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly LazyResolve<VehiclesManager> VehiclesManager;
      public readonly ConstructionManager ConstructionManager;

      public Factory(
        VehicleQueueJobFactory vehicleQueueJobFactory,
        VehicleJobId.Factory vehicleJobIdFactory,
        LazyResolve<VehiclesManager> vehiclesManager,
        ConstructionManager constructionManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.VehicleQueueJobFactory = vehicleQueueJobFactory;
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.VehiclesManager = vehiclesManager;
        this.ConstructionManager = constructionManager;
      }

      public void EnqueueJob(
        Vehicle vehicle,
        Option<DrivingEntityProto> replaceWith,
        VehicleDepotBase depot)
      {
        ScrapOrReplaceVehicleInDepotJob vehicleInDepotJob = new ScrapOrReplaceVehicleInDepotJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicle, replaceWith, depot);
      }
    }
  }
}
