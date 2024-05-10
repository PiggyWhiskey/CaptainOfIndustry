// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.VehicleQueueJob`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>Job for queueing in a VehicleQueue.</summary>
  [MemberRemovedInSaveVersion("m_cancelWhenGoalUnreachable", 140, typeof (bool), 0, false)]
  [GenerateSerializer(false, null, 0)]
  public class VehicleQueueJob<TVehicle> : VehicleJob, IVehicleJobObserver, IJobWithPreNavigation where TVehicle : Mafi.Core.Entities.Dynamic.Vehicle
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>
    /// Job to be started after the vehicle gets to the front of the queue. If the job is specified the vehicle is
    /// released from the queue after the job is done. If the job is not specified the vehicle waits in the queue
    /// until it is released by someone (presumably owner of the queue).
    /// </summary>
    private Option<IQueueTipJob> m_queueTipJob;
    private bool m_leaveAfterQueueTipJobDone;
    private IVehicleQueueFriend<TVehicle> m_queue;
    private Option<Mafi.Core.Entities.Dynamic.Vehicle> m_vehicleOwner;
    private Option<IStaticEntity> m_staticOwner;
    private bool m_useCustomTargetForStaticOwner;
    private VehicleQueueJob<TVehicle>.State m_state;
    private VehicleQueueJob<TVehicle>.QueueRegistrationState m_queueRegistrationState;
    private Option<Mafi.Core.Entities.Dynamic.Vehicle> m_currentVehicleNavigationTarget;
    private Option<IStaticEntity> m_currentStaticNavigationTarget;
    private bool m_isTrueJob;
    private readonly VehicleQueueJobFactory m_factory;
    private readonly RobustNavHelper m_navHelper;
    private bool m_navigateClosebyIsSufficient;
    private bool m_doNotCancelAllJobsOnNavFailure;
    private Option<QueueJobResultRef> m_resultRef;

    public static void Serialize(VehicleQueueJob<TVehicle> value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleQueueJob<TVehicle>>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleQueueJob<TVehicle>.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<IStaticEntity>.Serialize(this.m_currentStaticNavigationTarget, writer);
      Option<Mafi.Core.Entities.Dynamic.Vehicle>.Serialize(this.m_currentVehicleNavigationTarget, writer);
      writer.WriteBool(this.m_doNotCancelAllJobsOnNavFailure);
      writer.WriteBool(this.m_isTrueJob);
      writer.WriteBool(this.m_leaveAfterQueueTipJobDone);
      RobustNavHelper.Serialize(this.m_navHelper, writer);
      writer.WriteBool(this.m_navigateClosebyIsSufficient);
      writer.WriteGeneric<IVehicleQueueFriend<TVehicle>>(this.m_queue);
      writer.WriteInt((int) this.m_queueRegistrationState);
      Option<IQueueTipJob>.Serialize(this.m_queueTipJob, writer);
      Option<QueueJobResultRef>.Serialize(this.m_resultRef, writer);
      writer.WriteInt((int) this.m_state);
      Option<IStaticEntity>.Serialize(this.m_staticOwner, writer);
      writer.WriteBool(this.m_useCustomTargetForStaticOwner);
      Option<Mafi.Core.Entities.Dynamic.Vehicle>.Serialize(this.m_vehicleOwner, writer);
      writer.WriteGeneric<TVehicle>(this.Vehicle);
    }

    public static VehicleQueueJob<TVehicle> Deserialize(BlobReader reader)
    {
      VehicleQueueJob<TVehicle> vehicleQueueJob;
      if (reader.TryStartClassDeserialization<VehicleQueueJob<TVehicle>>(out vehicleQueueJob))
        reader.EnqueueDataDeserialization((object) vehicleQueueJob, VehicleQueueJob<TVehicle>.s_deserializeDataDelayedAction);
      return vehicleQueueJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      if (reader.LoadedSaveVersion < 140)
        reader.ReadBool();
      this.m_currentStaticNavigationTarget = Option<IStaticEntity>.Deserialize(reader);
      this.m_currentVehicleNavigationTarget = Option<Mafi.Core.Entities.Dynamic.Vehicle>.Deserialize(reader);
      this.m_doNotCancelAllJobsOnNavFailure = reader.ReadBool();
      reader.RegisterResolvedMember<VehicleQueueJob<TVehicle>>(this, "m_factory", typeof (VehicleQueueJobFactory), true);
      this.m_isTrueJob = reader.ReadBool();
      this.m_leaveAfterQueueTipJobDone = reader.ReadBool();
      reader.SetField<VehicleQueueJob<TVehicle>>(this, "m_navHelper", (object) RobustNavHelper.Deserialize(reader));
      this.m_navigateClosebyIsSufficient = reader.ReadBool();
      this.m_queue = reader.ReadGenericAs<IVehicleQueueFriend<TVehicle>>();
      this.m_queueRegistrationState = (VehicleQueueJob<TVehicle>.QueueRegistrationState) reader.ReadInt();
      this.m_queueTipJob = Option<IQueueTipJob>.Deserialize(reader);
      this.m_resultRef = Option<QueueJobResultRef>.Deserialize(reader);
      this.m_state = (VehicleQueueJob<TVehicle>.State) reader.ReadInt();
      this.m_staticOwner = Option<IStaticEntity>.Deserialize(reader);
      this.m_useCustomTargetForStaticOwner = reader.ReadBool();
      this.m_vehicleOwner = Option<Mafi.Core.Entities.Dynamic.Vehicle>.Deserialize(reader);
      this.Vehicle = reader.ReadGenericAs<TVehicle>();
    }

    public override LocStrFormatted JobInfo => new LocStrFormatted("Queued and waiting");

    public override bool IsTrueJob => this.m_isTrueJob;

    public override VehicleFuelConsumption CurrentFuelConsumption
    {
      get => !this.Vehicle.IsMoving ? VehicleFuelConsumption.Idle : VehicleFuelConsumption.Full;
    }

    public LocStrFormatted GoalName => this.m_queue.Owner.DefaultTitle;

    public TVehicle Vehicle { get; private set; }

    public override bool SkipNoMovementMonitoring
    {
      get => this.m_state == VehicleQueueJob<TVehicle>.State.Waiting;
    }

    public VehicleQueueJob(VehicleJobId id, VehicleQueueJobFactory factory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.m_navHelper = new RobustNavHelper(factory.PathFindingManager);
    }

    public void InitializeWithStaticOwner<TOwner>(
      TVehicle vehicle,
      VehicleQueue<TVehicle, TOwner> queue,
      bool isTrueJob,
      bool navigateClosebyIsSufficient,
      bool useCustomTarget,
      bool doNotCancelAllJobsOnNavFailure,
      Option<QueueJobResultRef> resultRef)
      where TOwner : class, IStaticEntity
    {
      this.commonInit(vehicle, (IVehicleQueueFriend<TVehicle>) queue, isTrueJob);
      this.m_staticOwner = (Option<IStaticEntity>) (IStaticEntity) queue.Owner;
      this.m_useCustomTargetForStaticOwner = useCustomTarget;
      this.m_vehicleOwner = (Option<Mafi.Core.Entities.Dynamic.Vehicle>) Option.None;
      this.m_navigateClosebyIsSufficient = navigateClosebyIsSufficient;
      this.m_doNotCancelAllJobsOnNavFailure = doNotCancelAllJobsOnNavFailure;
      this.m_resultRef = resultRef;
    }

    public void InitializeWithVehicleOwner<TOwner>(
      TVehicle vehicle,
      VehicleQueue<TVehicle, TOwner> queue,
      bool isTrueJob,
      bool navigateClosebyIsSufficient)
      where TOwner : Mafi.Core.Entities.Dynamic.Vehicle
    {
      this.commonInit(vehicle, (IVehicleQueueFriend<TVehicle>) queue, isTrueJob);
      this.m_staticOwner = (Option<IStaticEntity>) Option.None;
      this.m_vehicleOwner = (Option<Mafi.Core.Entities.Dynamic.Vehicle>) (Mafi.Core.Entities.Dynamic.Vehicle) queue.Owner;
      this.m_navigateClosebyIsSufficient = navigateClosebyIsSufficient;
    }

    private void commonInit(TVehicle vehicle, IVehicleQueueFriend<TVehicle> queue, bool isTrueJob)
    {
      Assert.That<bool>(queue.IsEnabled).IsTrue();
      this.Vehicle = vehicle.CheckNotNull<TVehicle>();
      this.m_queue = queue.CheckNotNull<IVehicleQueueFriend<TVehicle>>();
      this.m_isTrueJob = isTrueJob;
      this.m_state = VehicleQueueJob<TVehicle>.State.Initialization;
      this.m_queueRegistrationState = VehicleQueueJob<TVehicle>.QueueRegistrationState.None;
    }

    public void DoJobAtQueueTipAndLeave(IQueueTipJob job)
    {
      Assert.That<bool>(job.IsDestroyed).IsFalse<IQueueTipJob>("Adding destroyed job {0}", job);
      Assert.That<Option<IQueueTipJob>>(this.m_queueTipJob).IsNotEqualTo<IQueueTipJob>(job, "Replacing job with itself, is that ok?");
      this.clearQueueTipJob();
      this.m_queueTipJob = Option.Some<IQueueTipJob>(job);
      this.m_queueTipJob.Value.AddObserver((IVehicleJobObserver) this);
      this.m_leaveAfterQueueTipJobDone = true;
    }

    private void clearQueueTipJob()
    {
      if (this.m_queueTipJob.IsNone)
        return;
      Assert.That<bool>(this.m_queueTipJob.Value.IsDestroyed).IsFalse<IQueueTipJob>("Cleared queue tip job is destroyed {0}", this.m_queueTipJob.Value);
      this.m_queueTipJob.Value.RemoveObserver((IVehicleJobObserver) this);
      this.m_queueTipJob = (Option<IQueueTipJob>) Option.None;
    }

    public void OnJobDone(IVehicleJob job)
    {
      if (this.m_queueTipJob.ValueOrNull == job)
        this.clearQueueTipJob();
      else
        Log.Error("Wrong IQueueTipJob informed us that it is done.");
    }

    protected override bool DoJobInternal()
    {
      switch (this.m_state)
      {
        case VehicleQueueJob<TVehicle>.State.Initialization:
          this.m_state = this.handleInitialization();
          return true;
        case VehicleQueueJob<TVehicle>.State.Navigating:
          this.m_state = this.handleNavigation();
          if (this.m_state != VehicleQueueJob<TVehicle>.State.GoalInvalid)
            return true;
          goto case VehicleQueueJob<TVehicle>.State.GoalInvalid;
        case VehicleQueueJob<TVehicle>.State.Waiting:
          this.m_state = this.handleWaiting();
          return true;
        case VehicleQueueJob<TVehicle>.State.Released:
          Assert.That<bool>(this.Vehicle.IsDriving).IsFalse();
          this.clearQueueTipJob();
          if (this.m_resultRef.HasValue)
            this.m_resultRef.Value.ReportResult(true);
          return false;
        case VehicleQueueJob<TVehicle>.State.GoalInvalid:
          if (this.m_resultRef.HasValue)
            this.m_resultRef.Value.ReportResult(false);
          if (this.m_doNotCancelAllJobsOnNavFailure)
            this.clearRegistrations();
          else
            this.RequestCancel();
          return false;
        default:
          Assert.Fail("Invalid state.");
          this.RequestCancel();
          return false;
      }
    }

    private VehicleQueueJob<TVehicle>.State handleInitialization()
    {
      Assert.That<bool>(this.Vehicle.IsDriving).IsFalse();
      if (!this.m_queue.IsEnabled)
        return VehicleQueueJob<TVehicle>.State.Released;
      Assert.That<VehicleQueueJob<TVehicle>.QueueRegistrationState>(this.m_queueRegistrationState).IsEqualTo<VehicleQueueJob<TVehicle>.QueueRegistrationState>(VehicleQueueJob<TVehicle>.QueueRegistrationState.None);
      this.m_queue.VehicleArriving(this);
      this.m_queueRegistrationState = VehicleQueueJob<TVehicle>.QueueRegistrationState.Arriving;
      return VehicleQueueJob<TVehicle>.State.Navigating;
    }

    private VehicleQueueJob<TVehicle>.State handleNavigation()
    {
      if (this.navigateToEntityFormLine())
      {
        RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
        switch (robustNavResult)
        {
          case RobustNavResult.Navigating:
            return VehicleQueueJob<TVehicle>.State.Navigating;
          case RobustNavResult.GoalReachedSuccessfully:
            break;
          case RobustNavResult.FailGoalUnreachable:
            return VehicleQueueJob<TVehicle>.State.GoalInvalid;
          default:
            Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
            goto case RobustNavResult.FailGoalUnreachable;
        }
      }
      if (this.Vehicle.IsNavigating)
        return VehicleQueueJob<TVehicle>.State.Navigating;
      Assert.That<VehicleQueueJob<TVehicle>.QueueRegistrationState>(this.m_queueRegistrationState).IsNotEqualTo<VehicleQueueJob<TVehicle>.QueueRegistrationState>(VehicleQueueJob<TVehicle>.QueueRegistrationState.None);
      if (this.m_queueRegistrationState == VehicleQueueJob<TVehicle>.QueueRegistrationState.Arriving)
      {
        this.m_queue.RemoveArrivingVehicle(this);
        this.m_queue.VehicleArrivedAndWaiting(this);
        this.m_queueRegistrationState = VehicleQueueJob<TVehicle>.QueueRegistrationState.Waiting;
      }
      return VehicleQueueJob<TVehicle>.State.Waiting;
    }

    private VehicleQueueJob<TVehicle>.State handleWaiting()
    {
      Assert.That<bool>(this.Vehicle.IsNavigating).IsFalse();
      if (this.navigateToEntityFormLine())
        return VehicleQueueJob<TVehicle>.State.Navigating;
      bool pathFindingRetried;
      if (!this.Vehicle.CheckGoalValidityAndRetryNavigationIfNeeded(out pathFindingRetried))
      {
        Assert.That<bool>(pathFindingRetried).IsFalse();
        return VehicleQueueJob<TVehicle>.State.GoalInvalid;
      }
      if (pathFindingRetried)
        return VehicleQueueJob<TVehicle>.State.Navigating;
      if (!this.m_leaveAfterQueueTipJobDone || !this.m_queue.IsFirstVehicle(this.Vehicle))
        return VehicleQueueJob<TVehicle>.State.Waiting;
      Assert.That<bool>(this.Vehicle.IsNavigating).IsFalse();
      if (this.m_queueTipJob.HasValue)
      {
        this.m_queue.ReplaceFirstJobWith(this.m_queueTipJob.Value);
        return VehicleQueueJob<TVehicle>.State.Released;
      }
      this.RequestCancel();
      return this.m_state;
    }

    /// <summary>Makes the vehicle navigate to the goal entity.</summary>
    /// <returns>Whether the navigation is in progress.</returns>
    private bool navigateToEntityFormLine() => this.StartNavigation().IsNavigating;

    public RobustNavHelper StartNavigation()
    {
      Option<Mafi.Core.Entities.Dynamic.Vehicle> waitTargetFor = this.m_queue.GetWaitTargetFor(this);
      IEntity entity = (IEntity) waitTargetFor.ValueOrNull ?? (IEntity) this.m_vehicleOwner.ValueOrNull ?? (IEntity) this.m_staticOwner.ValueOrNull;
      if (((IEntity) this.m_currentVehicleNavigationTarget.ValueOrNull ?? (IEntity) this.m_currentStaticNavigationTarget.ValueOrNull) == entity)
      {
        Option<IVehicleGoal> option = this.m_navHelper.TaskToInject.HasValue ? this.m_navHelper.TaskToInject.Value.Goal : this.Vehicle.NavigationGoal;
        if (option.HasValue && option.Value.IsGoalValid((PathFindingEntity) this.Vehicle, out bool _))
          return this.m_navHelper;
      }
      this.m_currentStaticNavigationTarget = (Option<IStaticEntity>) Option.None;
      this.m_currentVehicleNavigationTarget = (Option<Mafi.Core.Entities.Dynamic.Vehicle>) Option.None;
      IVehicleGoalFull vehicleGoalFull;
      if (waitTargetFor.HasValue || this.m_vehicleOwner.HasValue)
      {
        this.m_currentVehicleNavigationTarget = Option.Some<Mafi.Core.Entities.Dynamic.Vehicle>(waitTargetFor.ValueOrNull ?? this.m_vehicleOwner.ValueOrNull);
        vehicleGoalFull = (IVehicleGoalFull) this.m_factory.DynamicEntityGoalFactory.Create(this.m_currentVehicleNavigationTarget.Value);
      }
      else
      {
        this.m_currentStaticNavigationTarget = this.m_staticOwner;
        vehicleGoalFull = (IVehicleGoalFull) this.m_factory.StaticEntityGoalFactory.Create(this.m_staticOwner.Value, this.m_useCustomTargetForStaticOwner);
      }
      RobustNavHelper navHelper = this.m_navHelper;
      // ISSUE: variable of a boxed type
      __Boxed<TVehicle> vehicle = (object) this.Vehicle;
      IVehicleGoalFull goal = vehicleGoalFull;
      bool closebyIsSufficient = this.m_navigateClosebyIsSufficient;
      RelTile1i? extraTolerancePerRetry = new RelTile1i?();
      int num = closebyIsSufficient ? 1 : 0;
      RelTile1f? maxNavigateClosebyDistance = new RelTile1f?();
      ThicknessTilesF? maxNavigateCloseByHeightDifference = new ThicknessTilesF?();
      navHelper.StartNavigationTo((Mafi.Core.Entities.Dynamic.Vehicle) vehicle, goal, extraTolerancePerRetry: extraTolerancePerRetry, navigateClosebyIsSufficient: num != 0, maxNavigateClosebyDistance: maxNavigateClosebyDistance, maxNavigateCloseByHeightDifference: maxNavigateCloseByHeightDifference, keepPath: true);
      Assert.That<bool>(this.m_navHelper.IsNavigating).IsTrue();
      return this.m_navHelper;
    }

    /// <summary>
    /// Whether the vehicle is waiting at front of the queue and is ready for loading. If not, it automatically goes
    /// closer to the front of the queue.
    /// </summary>
    public bool IsReadyAtQueueTip()
    {
      if (this.m_state != VehicleQueueJob<TVehicle>.State.Waiting || !this.m_queue.IsFirstVehicle(this.Vehicle))
        return false;
      if (this.Vehicle.NavigationGoal.IsNone)
      {
        Log.Error(string.Format("No navigation goal, vehicle: {0}.", (object) this.Vehicle));
        return false;
      }
      IVehicleGoal vehicleGoal = this.Vehicle.NavigationGoal.Value;
      bool pathFindingRetried;
      bool flag;
      if (this.m_currentVehicleNavigationTarget.HasValue)
      {
        Assert.That<bool>(vehicleGoal is DynamicEntityVehicleGoal entityVehicleGoal && entityVehicleGoal.GoalVehicle == this.m_currentVehicleNavigationTarget).IsTrue();
        flag = this.Vehicle.CheckGoalValidityAndRetryNavigationIfNeeded(out pathFindingRetried);
      }
      else if (this.m_currentStaticNavigationTarget.HasValue)
      {
        Assert.That<bool>(vehicleGoal is StaticEntityVehicleGoal entityVehicleGoal && entityVehicleGoal.GoalStaticEntity == this.m_currentStaticNavigationTarget).IsTrue();
        flag = this.Vehicle.CheckGoalValidityAndRetryNavigationIfNeeded(out pathFindingRetried);
      }
      else
      {
        Log.Error(string.Format("Invalid vehicle target when is ready at queue tip, vehicle: {0}.", (object) this.Vehicle));
        return false;
      }
      if (flag)
        return !pathFindingRetried;
      Log.Error(string.Format("Retry failed and goal is no longer valid, vehicle: {0}, goal: {1}.", (object) this.Vehicle, (object) vehicleGoal));
      return false;
    }

    /// <summary>
    /// Called by the queue to inform us that we were released (removed) from the queue.
    /// </summary>
    public void Released()
    {
      if (this.IsDestroyed)
      {
        Log.Error("Released on destroyed instance.");
      }
      else
      {
        Assert.That<VehicleQueueJob<TVehicle>.State>(this.m_state).IsNotEqualTo<VehicleQueueJob<TVehicle>.State>(VehicleQueueJob<TVehicle>.State.Released);
        this.Vehicle.StopNavigating(false);
        this.m_queueRegistrationState = VehicleQueueJob<TVehicle>.QueueRegistrationState.None;
        this.m_state = VehicleQueueJob<TVehicle>.State.Released;
      }
    }

    private void clearRegistrations()
    {
      if (this.m_queueRegistrationState == VehicleQueueJob<TVehicle>.QueueRegistrationState.Arriving)
        this.m_queue.RemoveArrivingVehicle(this);
      else if (this.m_queueRegistrationState == VehicleQueueJob<TVehicle>.QueueRegistrationState.Waiting)
        this.m_queue.RemoveWaitingJob(this);
      this.m_navHelper.CancelAndClear();
      if (this.Vehicle.IsDriving)
        this.Vehicle.StopDriving();
      Assert.That<IVehicleQueueFriend<TVehicle>>(this.m_queue).NotContains<TVehicle>(this);
      this.m_queueRegistrationState = VehicleQueueJob<TVehicle>.QueueRegistrationState.None;
      this.m_state = VehicleQueueJob<TVehicle>.State.Released;
      this.clearQueueTipJob();
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.clearRegistrations();
      this.Vehicle.AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      if (this.m_queue != null)
        Assert.That<IVehicleQueueFriend<TVehicle>>(this.m_queue).NotContains<TVehicle>(this);
      this.clearQueueTipJob();
      this.m_navHelper.Clear();
      this.m_leaveAfterQueueTipJobDone = false;
      this.m_resultRef = Option<QueueJobResultRef>.None;
    }

    static VehicleQueueJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleQueueJob<TVehicle>.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      VehicleQueueJob<TVehicle>.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
    }

    private enum State
    {
      Initialization,
      Navigating,
      Waiting,
      Released,
      GoalInvalid,
    }

    private enum QueueRegistrationState
    {
      None,
      Arriving,
      Waiting,
    }
  }
}
