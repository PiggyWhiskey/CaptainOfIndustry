// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.RobustNavHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles
{
  [MemberRemovedInSaveVersion("m_cancelWhenGoalUnreachable", 140, typeof (bool), 0, false)]
  [GenerateSerializer(false, null, 0)]
  public class RobustNavHelper
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly RelTile1i DEFAULT_EXTRA_TOLERANCE_PER_RETRY;
    private readonly IVehiclePathFindingManager m_pathFindingManager;
    private Option<Vehicle> m_vehicle;
    private Option<IVehicleGoalFull> m_goal;
    private RobustNavHelper.State m_state;
    private int m_maxRetries;
    private RelTile1i m_extraTolerancePerRetry;
    private bool m_navStarted;
    private Duration m_delayRemaining;
    private bool m_allowSimplePathsOnly;
    private bool m_navigateClosebyIsSufficient;
    [NewInSaveVersion(140, null, null, null, null)]
    private RelTile1f? m_maxNavigateClosebyDistance;
    [NewInSaveVersion(168, null, null, null, null)]
    private ThicknessTilesF? m_maxNavigateCloseByHeightDifference;
    private bool m_keepPath;

    public static void Serialize(RobustNavHelper value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RobustNavHelper>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RobustNavHelper.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsNavigating);
      writer.WriteBool(this.m_allowSimplePathsOnly);
      Duration.Serialize(this.m_delayRemaining, writer);
      RelTile1i.Serialize(this.m_extraTolerancePerRetry, writer);
      Option<IVehicleGoalFull>.Serialize(this.m_goal, writer);
      writer.WriteBool(this.m_keepPath);
      writer.WriteNullableStruct<RelTile1f>(this.m_maxNavigateClosebyDistance);
      writer.WriteNullableStruct<ThicknessTilesF>(this.m_maxNavigateCloseByHeightDifference);
      writer.WriteInt(this.m_maxRetries);
      writer.WriteBool(this.m_navigateClosebyIsSufficient);
      writer.WriteBool(this.m_navStarted);
      writer.WriteGeneric<IVehiclePathFindingManager>(this.m_pathFindingManager);
      writer.WriteInt((int) this.m_state);
      Option<Vehicle>.Serialize(this.m_vehicle, writer);
      Option<VehiclePathFindingTask>.Serialize(this.TaskToInject, writer);
    }

    public static RobustNavHelper Deserialize(BlobReader reader)
    {
      RobustNavHelper robustNavHelper;
      if (reader.TryStartClassDeserialization<RobustNavHelper>(out robustNavHelper))
        reader.EnqueueDataDeserialization((object) robustNavHelper, RobustNavHelper.s_deserializeDataDelayedAction);
      return robustNavHelper;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsNavigating = reader.ReadBool();
      this.m_allowSimplePathsOnly = reader.ReadBool();
      if (reader.LoadedSaveVersion < 140)
        reader.ReadBool();
      this.m_delayRemaining = Duration.Deserialize(reader);
      this.m_extraTolerancePerRetry = RelTile1i.Deserialize(reader);
      this.m_goal = Option<IVehicleGoalFull>.Deserialize(reader);
      this.m_keepPath = reader.ReadBool();
      this.m_maxNavigateClosebyDistance = reader.LoadedSaveVersion >= 140 ? reader.ReadNullableStruct<RelTile1f>() : new RelTile1f?();
      this.m_maxNavigateCloseByHeightDifference = reader.LoadedSaveVersion >= 168 ? reader.ReadNullableStruct<ThicknessTilesF>() : new ThicknessTilesF?();
      this.m_maxRetries = reader.ReadInt();
      this.m_navigateClosebyIsSufficient = reader.ReadBool();
      this.m_navStarted = reader.ReadBool();
      reader.SetField<RobustNavHelper>(this, "m_pathFindingManager", (object) reader.ReadGenericAs<IVehiclePathFindingManager>());
      this.m_state = (RobustNavHelper.State) reader.ReadInt();
      this.m_vehicle = Option<Vehicle>.Deserialize(reader);
      this.TaskToInject = reader.LoadedSaveVersion >= 140 ? Option<VehiclePathFindingTask>.Deserialize(reader) : new Option<VehiclePathFindingTask>();
    }

    [NewInSaveVersion(140, null, null, null, null)]
    public Option<VehiclePathFindingTask> TaskToInject { get; private set; }

    public bool IsNavigating { get; private set; }

    public RobustNavHelper(IVehiclePathFindingManager pathFindingManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_pathFindingManager = pathFindingManager;
    }

    public void StartNavigationTo(
      Vehicle vehicle,
      IVehicleGoalFull goal,
      int maxRetries = 1,
      RelTile1i? extraTolerancePerRetry = null,
      bool allowSimplePathsOnly = false,
      bool navigateClosebyIsSufficient = false,
      RelTile1f? maxNavigateClosebyDistance = null,
      ThicknessTilesF? maxNavigateCloseByHeightDifference = null,
      bool keepPath = false)
    {
      Assert.That<bool>(goal.IsInitialized).IsTrue();
      this.Clear();
      this.IsNavigating = true;
      this.m_vehicle = (Option<Vehicle>) vehicle.CheckNotNull<Vehicle>();
      this.m_goal = goal.CheckNotNull<IVehicleGoalFull>().CreateOption<IVehicleGoalFull>();
      this.m_state = RobustNavHelper.State.Navigation;
      this.m_maxRetries = maxRetries;
      this.m_extraTolerancePerRetry = extraTolerancePerRetry ?? RobustNavHelper.DEFAULT_EXTRA_TOLERANCE_PER_RETRY;
      this.m_allowSimplePathsOnly = allowSimplePathsOnly;
      this.m_navigateClosebyIsSufficient = navigateClosebyIsSufficient;
      this.m_maxNavigateClosebyDistance = maxNavigateClosebyDistance;
      this.m_maxNavigateCloseByHeightDifference = maxNavigateCloseByHeightDifference;
      this.m_keepPath = keepPath;
      this.m_navStarted = false;
    }

    /// <summary>
    /// Instead of having the params passed to the vehicle this allows to extract standalone
    /// PF task. Used when performing chained navigation.
    /// </summary>
    public VehiclePathFindingTask CreateAndEnqueuePfTask(Tile2f startTile)
    {
      VehiclePathFindingTask task = new VehiclePathFindingTask((IPathFindingVehicle) this.m_vehicle.Value);
      task.Initialize(this.m_goal.Value, this.m_maxRetries, this.m_extraTolerancePerRetry, this.m_allowSimplePathsOnly, this.m_navigateClosebyIsSufficient, this.m_maxNavigateClosebyDistance, this.m_maxNavigateCloseByHeightDifference, new Tile2f?(startTile));
      this.m_pathFindingManager.EnqueueTask((IManagedVehiclePathFindingTask) task, this.m_vehicle.Value.CalculatePfPriority());
      return task;
    }

    /// <summary>
    /// Instead of starting a navigation, the given task will be injected into the vehicle.
    /// This is used in case there was already a path found and the vehicle should reuse it.
    /// </summary>
    public void InjectTask(VehiclePathFindingTask task)
    {
      Assert.That<RobustNavHelper.State>(this.m_state).IsEqualTo<RobustNavHelper.State>(RobustNavHelper.State.Navigation);
      this.TaskToInject = (Option<VehiclePathFindingTask>) task;
    }

    public void Clear()
    {
      this.IsNavigating = false;
      if (this.TaskToInject.HasValue)
      {
        this.TaskToInject.Value.Clear(false);
        this.TaskToInject = (Option<VehiclePathFindingTask>) Option.None;
      }
      this.m_vehicle = Option<Vehicle>.None;
      this.m_goal = Option<IVehicleGoalFull>.None;
    }

    public void CancelAndClear()
    {
      if (this.m_vehicle.IsNone)
        return;
      this.m_vehicle.Value.StopNavigating();
      this.Clear();
    }

    private bool tryStartNavigation()
    {
      Assert.That<bool>(this.m_navStarted).IsFalse();
      Assert.That<bool>(this.m_goal.Value.IsInitialized).IsTrue();
      if (this.m_goal.IsNone)
      {
        Log.Warning("Failed to start nav, goal is null.");
        return false;
      }
      if (this.m_vehicle.IsNone)
      {
        Log.Warning("Failed to start nav, vehicle is null.");
        return false;
      }
      if (!this.m_goal.Value.IsInitialized)
      {
        Log.Warning("Failed to start nav, goal is not initialized.");
        return false;
      }
      bool retryPf;
      if (!this.m_goal.Value.IsGoalValid((PathFindingEntity) this.m_vehicle.Value, out retryPf) && !retryPf)
        return false;
      this.m_navStarted = true;
      this.m_vehicle.Value.NavigateTo(this.m_goal.Value, this.m_maxRetries, new RelTile1i?(this.m_extraTolerancePerRetry), this.m_allowSimplePathsOnly, navigateClosebyIsSufficient: this.m_navigateClosebyIsSufficient, maxNavigateClosebyDistance: this.m_maxNavigateClosebyDistance, maxNavigateCloseByHeightDifference: this.m_maxNavigateCloseByHeightDifference, keepPath: this.m_keepPath);
      return true;
    }

    /// <summary>Returns true when done.</summary>
    public RobustNavResult StepNavigation()
    {
      Assert.That<bool>(this.IsNavigating).IsTrue();
      Assert.That<Option<Vehicle>>(this.m_vehicle).HasValue<Vehicle>();
      RobustNavResult? result = new RobustNavResult?();
      RobustNavHelper.State state;
      switch (this.m_state)
      {
        case RobustNavHelper.State.Navigation:
          state = this.handleNavigationState(out result);
          break;
        case RobustNavHelper.State.WaitingForRetry:
          state = this.handleWaitingForRetry();
          break;
        case RobustNavHelper.State.Done:
          result = new RobustNavResult?(RobustNavResult.FailGoalUnreachable);
          state = RobustNavHelper.State.Done;
          Log.Error(string.Format("Invalid state: {0}", (object) this.m_state));
          break;
        default:
          Log.Error(string.Format("Unknown or invalid state: {0}", (object) this.m_state));
          goto case RobustNavHelper.State.Navigation;
      }
      this.m_state = state;
      if (!result.HasValue)
        return RobustNavResult.Navigating;
      Assert.That<RobustNavHelper.State>(this.m_state).IsEqualTo<RobustNavHelper.State>(RobustNavHelper.State.Done);
      Assert.That<bool>(this.m_vehicle.Value.IsNavigating).IsFalse();
      this.Clear();
      Assert.That<bool>(this.IsNavigating).IsFalse();
      return result.Value;
    }

    private RobustNavHelper.State handleNavigationState(out RobustNavResult? result)
    {
      Vehicle vehicle = this.m_vehicle.Value;
      if (this.TaskToInject.HasValue)
      {
        if (this.TaskToInject.Value.DistanceEstimationStartTile.DistanceSqrTo(vehicle.GroundPositionTile2i) >= PathFindingEntity.MAX_DIST_DURING_PF_SQR)
        {
          this.TaskToInject.Value.Clear(false);
        }
        else
        {
          vehicle.InjectPfTask(this.TaskToInject.Value, this.m_goal.Value);
          this.m_navStarted = true;
        }
        this.TaskToInject = (Option<VehiclePathFindingTask>) Option.None;
      }
      if (!this.m_navStarted)
      {
        if (!this.tryStartNavigation())
        {
          result = new RobustNavResult?(RobustNavResult.FailGoalUnreachable);
          return RobustNavHelper.State.Done;
        }
        result = new RobustNavResult?();
        return RobustNavHelper.State.Navigation;
      }
      if (vehicle.IsNavigating)
      {
        result = new RobustNavResult?();
        return RobustNavHelper.State.Navigation;
      }
      if (vehicle.NavigatedSuccessfully)
      {
        Assert.That<bool>(vehicle.IsDriving).IsFalse();
        result = new RobustNavResult?(RobustNavResult.GoalReachedSuccessfully);
        return RobustNavHelper.State.Done;
      }
      result = new RobustNavResult?(RobustNavResult.FailGoalUnreachable);
      return RobustNavHelper.State.Done;
    }

    private RobustNavHelper.State handleWaitingForRetry()
    {
      this.m_delayRemaining -= Duration.OneTick;
      if (this.m_delayRemaining.IsPositive)
        return RobustNavHelper.State.WaitingForRetry;
      this.m_navStarted = false;
      return RobustNavHelper.State.Navigation;
    }

    static RobustNavHelper()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RobustNavHelper.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((RobustNavHelper) obj).SerializeData(writer));
      RobustNavHelper.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((RobustNavHelper) obj).DeserializeData(reader));
      RobustNavHelper.DEFAULT_EXTRA_TOLERANCE_PER_RETRY = new RelTile1i(2);
    }

    private enum State
    {
      Navigation,
      [OnlyForSaveCompatibility(null)] WaitingForRetry,
      Done,
    }
  }
}
