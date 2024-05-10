// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.NavigateToJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("m_navStarted", 140, typeof (bool), 0, false)]
  public sealed class NavigateToJob : VehicleJob, IJobWithPreNavigation
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private LocStrFormatted m_jobInfo;
    private readonly Vehicle m_vehicle;
    private readonly IVehicleGoalFull m_goal;
    private readonly bool m_isTrueJob;
    private readonly bool m_navigateClosebyIsSufficient;
    private readonly NavigateToJob.Factory m_factory;
    private readonly RobustNavHelper m_navHelper;

    public static void Serialize(NavigateToJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NavigateToJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NavigateToJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<IVehicleGoalFull>(this.m_goal);
      writer.WriteBool(this.m_isTrueJob);
      LocStrFormatted.Serialize(this.m_jobInfo, writer);
      RobustNavHelper.Serialize(this.m_navHelper, writer);
      writer.WriteBool(this.m_navigateClosebyIsSufficient);
      writer.WriteGeneric<Vehicle>(this.m_vehicle);
    }

    public static NavigateToJob Deserialize(BlobReader reader)
    {
      NavigateToJob navigateToJob;
      if (reader.TryStartClassDeserialization<NavigateToJob>(out navigateToJob))
        reader.EnqueueDataDeserialization((object) navigateToJob, NavigateToJob.s_deserializeDataDelayedAction);
      return navigateToJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.RegisterResolvedMember<NavigateToJob>(this, "m_factory", typeof (NavigateToJob.Factory), true);
      reader.SetField<NavigateToJob>(this, "m_goal", (object) reader.ReadGenericAs<IVehicleGoalFull>());
      reader.SetField<NavigateToJob>(this, "m_isTrueJob", (object) reader.ReadBool());
      this.m_jobInfo = LocStrFormatted.Deserialize(reader);
      reader.SetField<NavigateToJob>(this, "m_navHelper", (object) RobustNavHelper.Deserialize(reader));
      reader.SetField<NavigateToJob>(this, "m_navigateClosebyIsSufficient", (object) reader.ReadBool());
      if (reader.LoadedSaveVersion < 140)
        reader.ReadBool();
      reader.SetField<NavigateToJob>(this, "m_vehicle", (object) reader.ReadGenericAs<Vehicle>());
    }

    public override LocStrFormatted JobInfo => this.m_jobInfo;

    public override bool IsTrueJob => this.m_isTrueJob;

    public override VehicleFuelConsumption CurrentFuelConsumption
    {
      get
      {
        return this.m_vehicle.PfState != PathFindingEntityState.DrivingToDestination ? VehicleFuelConsumption.Idle : VehicleFuelConsumption.Full;
      }
    }

    public LocStrFormatted GoalName => this.m_goal.GoalName;

    private NavigateToJob(
      VehicleJobId id,
      NavigateToJob.Factory factory,
      Vehicle vehicle,
      IVehicleGoalFull goal,
      bool navigateClosebyIsSufficient,
      bool asTrueJob,
      bool enqueueFirst)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory;
      this.m_navHelper = new RobustNavHelper(factory.PathFindingManager);
      Assert.That<bool>(goal.IsInitialized).IsTrue("No goal was set.");
      this.m_vehicle = vehicle.CheckNotNull<Vehicle>();
      this.m_goal = goal.CheckNotNull<IVehicleGoalFull>();
      this.m_navigateClosebyIsSufficient = navigateClosebyIsSufficient;
      this.m_isTrueJob = asTrueJob;
      vehicle.EnqueueJob((VehicleJob) this, enqueueFirst);
      this.m_jobInfo = new LocStrFormatted("Preparing for navigation");
    }

    protected override bool DoJobInternal()
    {
      if (!this.m_navHelper.IsNavigating)
      {
        this.StartNavigation();
        this.m_jobInfo = (LocStrFormatted) TrCore.VehicleJob__Navigating;
        return true;
      }
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          return true;
        case RobustNavResult.GoalReachedSuccessfully:
          return false;
        case RobustNavResult.FailGoalUnreachable:
          this.RequestCancel();
          return false;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }
    }

    public RobustNavHelper StartNavigation()
    {
      RobustNavHelper navHelper = this.m_navHelper;
      Vehicle vehicle = this.m_vehicle;
      IVehicleGoalFull goal = this.m_goal;
      bool closebyIsSufficient = this.m_navigateClosebyIsSufficient;
      RelTile1i? extraTolerancePerRetry = new RelTile1i?();
      int num = closebyIsSufficient ? 1 : 0;
      RelTile1f? maxNavigateClosebyDistance = new RelTile1f?();
      ThicknessTilesF? maxNavigateCloseByHeightDifference = new ThicknessTilesF?();
      navHelper.StartNavigationTo(vehicle, goal, extraTolerancePerRetry: extraTolerancePerRetry, navigateClosebyIsSufficient: num != 0, maxNavigateClosebyDistance: maxNavigateClosebyDistance, maxNavigateCloseByHeightDifference: maxNavigateCloseByHeightDifference);
      return this.m_navHelper;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.m_navHelper.CancelAndClear();
      ((IVehicleFriend) this.m_vehicle).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy() => this.m_navHelper.Clear();

    static NavigateToJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NavigateToJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      NavigateToJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly IVehiclePathFindingManager PathFindingManager;

      public Factory(
        VehicleJobId.Factory vehicleJobIdFactory,
        IVehiclePathFindingManager pathFindingManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.PathFindingManager = pathFindingManager;
      }

      /// <summary>
      /// Creates and enqueues a job for navigation towards given goal. If the navigation fails all other jobs will be
      /// canceled.
      /// </summary>
      public NavigateToJob EnqueueJob(
        Vehicle vehicle,
        IVehicleGoalFull goal,
        bool navigateClosebyIsSufficient = false,
        bool asTrueJob = true,
        bool enqueueFirst = false)
      {
        return new NavigateToJob(this.m_vehicleJobIdFactory.GetNextId(), this, vehicle, goal, navigateClosebyIsSufficient, asTrueJob, enqueueFirst);
      }
    }
  }
}
