// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.ChainedNavigationJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GenerateSerializer(false, null, 0)]
  public sealed class ChainedNavigationJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Vehicle m_vehicle;
    private readonly IJobWithPreNavigation m_firstJob;
    private readonly IJobWithPreNavigation m_secondJob;
    [DoNotSave(0, null)]
    private LocStrFormatted m_detailedJobInfo;
    private Option<RobustNavHelper> m_fistNavHelper;
    private Option<RobustNavHelper> m_secondNavHelper;
    private Option<VehiclePathFindingTask> m_secondPfTask;

    public static void Serialize(ChainedNavigationJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ChainedNavigationJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ChainedNavigationJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<IJobWithPreNavigation>(this.m_firstJob);
      Option<RobustNavHelper>.Serialize(this.m_fistNavHelper, writer);
      writer.WriteGeneric<IJobWithPreNavigation>(this.m_secondJob);
      Option<RobustNavHelper>.Serialize(this.m_secondNavHelper, writer);
      Option<VehiclePathFindingTask>.Serialize(this.m_secondPfTask, writer);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
    }

    public static ChainedNavigationJob Deserialize(BlobReader reader)
    {
      ChainedNavigationJob chainedNavigationJob;
      if (reader.TryStartClassDeserialization<ChainedNavigationJob>(out chainedNavigationJob))
        reader.EnqueueDataDeserialization((object) chainedNavigationJob, ChainedNavigationJob.s_deserializeDataDelayedAction);
      return chainedNavigationJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ChainedNavigationJob>(this, "m_firstJob", (object) reader.ReadGenericAs<IJobWithPreNavigation>());
      this.m_fistNavHelper = Option<RobustNavHelper>.Deserialize(reader);
      reader.SetField<ChainedNavigationJob>(this, "m_secondJob", (object) reader.ReadGenericAs<IJobWithPreNavigation>());
      this.m_secondNavHelper = Option<RobustNavHelper>.Deserialize(reader);
      this.m_secondPfTask = Option<VehiclePathFindingTask>.Deserialize(reader);
      reader.SetField<ChainedNavigationJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
      reader.RegisterInitAfterLoad<ChainedNavigationJob>(this, "initAfterLoad", InitPriority.Normal);
    }

    public override LocStrFormatted JobInfo => this.m_detailedJobInfo;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption => VehicleFuelConsumption.None;

    private ChainedNavigationJob(
      VehicleJobId id,
      ChainedNavigationJob.Factory factory,
      Vehicle vehicle,
      IJobWithPreNavigation firstJob,
      IJobWithPreNavigation secondJob)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_vehicle = vehicle;
      this.m_firstJob = firstJob;
      this.m_secondJob = secondJob;
      this.m_vehicle.EnqueueJob((VehicleJob) this, true);
      this.m_vehicle.SkipDrivingToDestination();
      this.m_fistNavHelper = (Option<RobustNavHelper>) this.m_firstJob.StartNavigation();
      this.initAfterLoad();
    }

    [InitAfterLoad(InitPriority.Normal)]
    public void initAfterLoad()
    {
      if (this.m_firstJob != null && this.m_secondJob != null)
        this.m_detailedJobInfo = TrCore.VehicleJob__NavigatingToVia.Format(this.m_secondJob.GoalName, this.m_firstJob.GoalName);
      else
        this.m_detailedJobInfo = TrCore.VehicleJob__Navigating.AsFormatted;
    }

    protected override bool DoJobInternal()
    {
      if (this.m_fistNavHelper.HasValue && this.m_secondNavHelper.HasValue && this.m_secondPfTask.IsNone)
      {
        Log.Error("Job should already be done!");
        return false;
      }
      if (this.m_fistNavHelper.IsNone)
        return this.startFirstPfJob();
      return this.m_secondPfTask.HasValue ? this.stepSecondPfTask() : this.stepFirstNavHelper();
    }

    private bool startFirstPfJob()
    {
      this.m_fistNavHelper = (Option<RobustNavHelper>) this.m_firstJob.StartNavigation();
      if (this.m_fistNavHelper.Value.IsNavigating)
        return true;
      this.abort();
      return false;
    }

    private bool stepSecondPfTask()
    {
      VehiclePathFindingTask task = this.m_secondPfTask.Value;
      if (!task.IsFinished)
        return true;
      if (task.Result.ResultStatus == VehiclePfResultStatus.PathFound)
      {
        this.m_secondNavHelper.Value.InjectTask(task);
        this.m_secondPfTask = Option<VehiclePathFindingTask>.None;
      }
      else
      {
        this.m_vehicle.IncNavigationFailedStreak();
        this.abort();
      }
      return false;
    }

    private bool stepFirstNavHelper()
    {
      RobustNavResult robustNavResult = this.m_fistNavHelper.Value.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          if (this.m_vehicle.PfState == PathFindingEntityState.ReadyToDriveToDestination)
          {
            if (!this.m_vehicle.PfTask.IsFinished || this.m_vehicle.PfTask.Result.ResultStatus != VehiclePfResultStatus.PathFound)
            {
              Log.Error("Invalid state");
              return false;
            }
            VehiclePathFindingTask pfTask = this.m_vehicle.ExtractPfTask();
            this.m_fistNavHelper.Value.InjectTask(pfTask);
            Tile2f centerTileSpace = pfTask.PathFindingParams.ConvertToCenterTileSpace(pfTask.Result.GoalRawTile);
            this.m_secondNavHelper = (Option<RobustNavHelper>) this.m_secondJob.StartNavigation();
            if (!this.m_secondNavHelper.Value.IsNavigating)
            {
              this.abort();
              return false;
            }
            this.m_secondPfTask = (Option<VehiclePathFindingTask>) this.m_secondNavHelper.Value.CreateAndEnqueuePfTask(centerTileSpace);
          }
          return true;
        case RobustNavResult.GoalReachedSuccessfully:
          Log.Error("We should not arrive there!");
          return false;
        case RobustNavResult.FailGoalUnreachable:
          ((IVehicleFriend) this.m_vehicle).CancelAllJobsExcept((VehicleJob) this);
          return false;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }
    }

    private void abort()
    {
      ((IVehicleFriend) this.m_vehicle).CancelAllJobsExcept((VehicleJob) this);
    }

    protected override void OnDestroy()
    {
      if (!this.m_secondPfTask.HasValue)
        return;
      this.m_secondPfTask.Value.Clear(false);
      this.m_secondPfTask = (Option<VehiclePathFindingTask>) Option.None;
    }

    static ChainedNavigationJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ChainedNavigationJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      ChainedNavigationJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;

      public Factory(VehicleJobId.Factory vehicleJobIdFactory)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
      }

      public void EnqueueAsFirstJob(
        Vehicle vehicle,
        IJobWithPreNavigation firstJob,
        IJobWithPreNavigation secondJob)
      {
        ChainedNavigationJob chainedNavigationJob = new ChainedNavigationJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicle, firstJob, secondJob);
      }
    }
  }
}
