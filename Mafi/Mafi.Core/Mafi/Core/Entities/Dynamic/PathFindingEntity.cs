// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.PathFindingEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Notifications;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Roads;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Numerics;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  /// <summary>
  /// A dynamic entity that is able to navigate using path finding.
  /// TODO: Rename to PathFindingVehicle
  /// </summary>
  public abstract class PathFindingEntity : 
    DrivingEntity,
    IPathFindingVehicle,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey
  {
    private static readonly RelTile1i LONG_DRIVE_MAX_DIST;
    private static readonly Fix64 LONG_DRIVE_MAX_DIST_SQR;
    public static readonly long MAX_DIST_DURING_PF_SQR;
    private static readonly int STRUGGLING_STREAK_NOTIF_THRESHOLD_FAILURES;
    private static readonly int STRUGGLING_STREAK_NOTIF_THRESHOLD_TICKS;
    private static readonly int STRUGGLING_STREAK_NOTIF_THRESHOLD_FAILURES_LONG;
    private static readonly int STRUGGLING_STREAK_NOTIF_THRESHOLD_TICKS_LONG;
    private static readonly int STRUGGLING_STREAK_NOTIF_HIDE_AFTER_TICKS;
    public static readonly Duration STRUGGLING_STREAK_PRIO_PENALTY;
    public static readonly Duration STRUGGLING_STREAK_MAX_PENALTY;
    [NewInSaveVersion(105, null, null, null, null)]
    private int m_navigationFailedStepsSinceFirst;
    [NewInSaveVersion(154, null, null, null, null)]
    private int m_navigationFailedStepsSinceLast;
    private PathFindingEntityState m_prevPfState;
    private readonly IVehiclePathFindingManager m_pathFindingManager;
    protected readonly IVehiclesManager VehiclesManager;
    private Option<IVehicleGoalFull> m_navGoal;
    private Tile2i m_pathCheckTile;
    private Tile2i m_targetRaw;
    [DoNotSave(166, null)]
    private bool m_noValidLocationNearby;
    [NewInSaveVersion(166, null, null, null, null)]
    private int m_noValidLocationNearbyCooldown;
    private bool m_retryPfAfterFindingValidLocation;
    [NewInSaveVersion(140, null, null, null, null)]
    private bool m_skipDrivingOneTime;
    [DoNotSave(140, null)]
    private EntityNotificator m_goalUnreachableNotif;
    [NewInSaveVersion(105, null, null, null, null)]
    private EntityNotificator m_strugglingToNavigateNotif;
    [NewInSaveVersion(140, null, null, null, null)]
    private Option<VehicleTerrainPathSegment> m_currentTerrainPathSegment;
    [NewInSaveVersion(140, null, null, null, null)]
    private Option<VehicleRoadPathSegment> m_currentRoadPathSegment;

    /// <summary>
    /// Whether this entity is actively trying to reach its goal. Detailed info can be obtained from <see cref="P:Mafi.Core.Entities.Dynamic.PathFindingEntity.PfState" />.
    /// </summary>
    public bool IsNavigating => this.PfState != 0;

    /// <summary>
    /// Whether path finding was successful and the entity navigated to given goal. This may be false due to
    /// inability to find a path to the destination because entity will perform limited number of retries before it
    /// gives up.
    /// </summary>
    public bool NavigatedSuccessfully { get; private set; }

    /// <summary>
    /// When path finding fails multiple times and the vehicle has given-up on the navigation.
    /// </summary>
    public bool NavigationFailed { get; private set; }

    [NewInSaveVersion(105, null, null, null, null)]
    public int NavigationFailedStreak { get; private set; }

    /// <summary>Extra status information about the entity state.</summary>
    public PathFindingEntityState PfState { get; private set; }

    /// <summary>Whether to track explored tiles with the path-finder.</summary>
    public bool TrackExploredTiles { get; set; }

    /// <summary>
    /// Every entity have its own pathfinder task which is re-used for all path finding tasks of this entity. Saved
    /// using SaveLoadPfTask.
    /// </summary>
    public VehiclePathFindingTask PfTask { get; private set; }

    public IPathFindingResult PathFindingResult => (IPathFindingResult) this.PfTask.Result;

    public Option<IVehicleGoal> NavigationGoal
    {
      get => this.m_navGoal.ValueOrNull.CreateOption<IVehicleGoal>();
    }

    public VehiclePathFindingParams PathFindingParams => this.Prototype.PathFindingParams;

    public Option<IVehicleGoal> UnreachableGoal { get; private set; }

    public bool IsStrugglingToNavigate => this.m_strugglingToNavigateNotif.IsActive;

    protected virtual bool m_shouldSuppressStrugglingToNavigate => false;

    public Option<IVehiclePathSegment> CurrentPathSegment
    {
      get
      {
        return ((IVehiclePathSegment) this.m_currentTerrainPathSegment.ValueOrNull ?? (IVehiclePathSegment) this.m_currentRoadPathSegment.ValueOrNull).CreateOption<IVehiclePathSegment>();
      }
    }

    protected PathFindingEntity(
      EntityId id,
      DrivingEntityProto prototype,
      EntityContext context,
      Tile2f initialPosition,
      TerrainManager terrain,
      IVehiclePathFindingManager pathFindingManager,
      IVehiclesManager vehiclesManager,
      IVehicleSurfaceProvider surfaceProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, prototype, context, initialPosition, terrain, surfaceProvider, pathFindingManager.PathabilityProvider);
      this.m_pathFindingManager = pathFindingManager.CheckNotNull<IVehiclePathFindingManager>();
      this.VehiclesManager = vehiclesManager.CheckNotNull<IVehiclesManager>();
      this.PfTask = new VehiclePathFindingTask((IPathFindingVehicle) this);
      if (prototype.PathFindingParams.HeightClearancePathability == HeightClearancePathability.CanPassUnder)
        this.m_strugglingToNavigateNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.VehicleGoalStruggling);
      else
        this.m_strugglingToNavigateNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.VehicleGoalStrugglingCannotGoUnder);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      if (saveVersion < 105)
        this.m_strugglingToNavigateNotif = this.Prototype.PathFindingParams.HeightClearancePathability != HeightClearancePathability.CanPassUnder ? this.Context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.VehicleGoalStrugglingCannotGoUnder) : this.Context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.VehicleGoalStruggling);
      if (saveVersion >= 140)
        return;
      this.m_goalUnreachableNotif.Deactivate((IEntity) this);
    }

    public VehiclePathFindingTask ExtractPfTask()
    {
      VehiclePathFindingTask pfTask = this.PfTask;
      this.PfTask = new VehiclePathFindingTask((IPathFindingVehicle) this);
      this.m_navGoal = Option<IVehicleGoalFull>.None;
      this.StopNavigating();
      return pfTask;
    }

    public void InjectPfTask(VehiclePathFindingTask task, IVehicleGoalFull goal)
    {
      this.StopNavigating();
      this.PfTask = task;
      this.m_navGoal = goal.SomeOption<IVehicleGoalFull>();
      this.PfState = PathFindingEntityState.PathFinding;
      this.NavigatedSuccessfully = false;
    }

    /// <summary>
    /// Finds a path to the given goal and navigates there. Stops any ongoing navigation. Use <see cref="P:Mafi.Core.Entities.Dynamic.PathFindingEntity.PfState" />
    /// to monitor ongoing navigation.
    /// </summary>
    /// <remarks>
    /// This will also perform re-planning when the current path is occupied or when the goal moved.
    /// </remarks>
    /// <param name="goal">Goal specification.</param>
    /// <param name="maxRetries">
    /// Max number of retries. Each retry will extend goals according to the implementation
    /// of <see cref="!:VehicleGoal" />.
    /// </param>
    /// <param name="allowSimplePathOnly">
    /// Whether to only allow simple mostly direct paths with no long detours. This restricts max path length.
    /// </param>
    /// <param name="disableUnstuck">
    /// Whether to disable automatic unstack trial when starting navigation.
    /// </param>
    public void NavigateTo(
      IVehicleGoalFull goal,
      int maxRetries,
      RelTile1i? extraTolerancePerRetry = null,
      bool allowSimplePathOnly = false,
      bool disableUnstuck = false,
      bool navigateClosebyIsSufficient = false,
      RelTile1f? maxNavigateClosebyDistance = null,
      ThicknessTilesF? maxNavigateCloseByHeightDifference = null,
      bool keepPath = false)
    {
      this.StopNavigating(keepPath);
      this.m_navGoal = goal.SomeOption<IVehicleGoalFull>();
      this.PfState = this.startPathFinding(keepPath, maxRetries, extraTolerancePerRetry ?? RobustNavHelper.DEFAULT_EXTRA_TOLERANCE_PER_RETRY, allowSimplePathOnly, !disableUnstuck, navigateClosebyIsSufficient, maxNavigateClosebyDistance, maxNavigateCloseByHeightDifference);
    }

    /// <summary>
    /// Fills given array with goal tiles. Some goals might be repeated due to tolerance radius. Returns <c>true</c>
    /// when the goal is already reached and no path finding is needed.
    /// </summary>
    public bool GetGoalTiles(
      IVehicleGoalFull vehicleGoal,
      int retryNumber,
      RelTile1i extraTolerancePerRetry,
      Tile2f? customStartTile,
      out Tile2f startTile,
      Lyst<Tile2i> extraStartTiles,
      Lyst<Tile2i> goalTiles,
      out Tile2i distanceEstimationGoalTile)
    {
      Assert.That<Lyst<Tile2i>>(goalTiles).IsEmpty<Tile2i>();
      if (!customStartTile.HasValue && retryNumber > 0)
      {
        extraStartTiles.Add(this.LastValidPosition);
        extraStartTiles.Add(this.PreLastValidPosition);
      }
      bool retryPf;
      if (!vehicleGoal.IsGoalValid(this, out retryPf) && !retryPf)
      {
        startTile = customStartTile ?? this.Position2f;
        distanceEstimationGoalTile = this.GroundPositionTile2i;
        return false;
      }
      if (!customStartTile.HasValue && this.IsDriving)
      {
        RelTile2f relTile2f = this.DrivingTarget - this.Position2f;
        Fix64 lengthSqr = relTile2f.LengthSqr;
        Fix64 squared = (this.DrivingData.MaxForwardsSpeed * 5).Squared;
        startTile = !(lengthSqr < squared) ? this.Position2f + relTile2f.Normalized * (this.DrivingData.MaxForwardsSpeed.Value * 5) : this.DrivingTarget;
      }
      else
        startTile = customStartTile ?? this.Position2f;
      return vehicleGoal.GetGoalTiles(startTile.Tile2i, this.Prototype.PathFindingParams, goalTiles, out distanceEstimationGoalTile, retryNumber, extraTolerancePerRetry);
    }

    private PathFindingEntityState startPathFinding(
      bool keepPath,
      int maxRetries,
      RelTile1i extraTolerancePerRetry,
      bool allowSimplePathOnly,
      bool tryUnstuck,
      bool navigateClosebyIsSufficient,
      RelTile1f? maxNavigateClosebyDistance = null,
      ThicknessTilesF? maxNavigateCloseByHeightDifference = null)
    {
      Assert.That<IPathFindingVehicle>(this.PfTask.Vehicle).IsEqualTo<IPathFindingVehicle>((IPathFindingVehicle) this);
      this.PfTask.Clear(keepPath);
      if (this.m_navGoal.IsNone || !this.m_navGoal.Value.IsInitialized)
      {
        Log.Error(string.Format("Starting path-finding of '{0}' with no goal. Ignoring.", (object) this.Prototype.Id));
        this.StopNavigating();
        return PathFindingEntityState.Idle;
      }
      this.PfTask.Initialize(this.m_navGoal.Value, maxRetries, extraTolerancePerRetry, allowSimplePathOnly, navigateClosebyIsSufficient, maxNavigateClosebyDistance, maxNavigateCloseByHeightDifference);
      if (tryUnstuck && !this.IsPathable(this.GroundPositionTile2i))
      {
        this.StopNavigating();
        return this.driveToValidLocation(true);
      }
      this.NavigatedSuccessfully = false;
      this.NavigationFailed = false;
      this.m_noValidLocationNearbyCooldown = 0;
      this.m_pathFindingManager.EnqueueTask((IManagedVehiclePathFindingTask) this.PfTask, this.CalculatePfPriority());
      return PathFindingEntityState.PathFinding;
    }

    public int CalculatePfPriority()
    {
      return (this.NavigationFailedStreak * PathFindingEntity.STRUGGLING_STREAK_PRIO_PENALTY.Ticks).Min(PathFindingEntity.STRUGGLING_STREAK_MAX_PENALTY.Ticks);
    }

    /// <summary>
    /// Stops the entity and terminates any tasks or path-finding.
    /// </summary>
    public void StopNavigating(bool keepPath = false)
    {
      if (!keepPath)
        this.StopDriving();
      this.PfTask.Clear(keepPath);
      this.PfState = PathFindingEntityState.Idle;
      this.m_pathCheckTile = Tile2i.MaxValue;
      this.m_retryPfAfterFindingValidLocation = false;
      this.m_noValidLocationNearbyCooldown = 0;
    }

    protected override void SimUpdateInternal()
    {
      this.m_prevPfState = this.PfState;
      if (this.NavigationFailedStreak > 0)
      {
        ++this.m_navigationFailedStepsSinceFirst;
        ++this.m_navigationFailedStepsSinceLast;
      }
      else
        this.m_navigationFailedStepsSinceFirst = 0;
      this.m_strugglingToNavigateNotif.NotifyIff((this.NavigationFailedStreak >= PathFindingEntity.STRUGGLING_STREAK_NOTIF_THRESHOLD_FAILURES && this.m_navigationFailedStepsSinceFirst >= PathFindingEntity.STRUGGLING_STREAK_NOTIF_THRESHOLD_TICKS) | (this.NavigationFailedStreak >= PathFindingEntity.STRUGGLING_STREAK_NOTIF_THRESHOLD_FAILURES_LONG && this.m_navigationFailedStepsSinceFirst >= PathFindingEntity.STRUGGLING_STREAK_NOTIF_THRESHOLD_TICKS_LONG) && this.m_navigationFailedStepsSinceLast <= PathFindingEntity.STRUGGLING_STREAK_NOTIF_HIDE_AFTER_TICKS && !this.m_shouldSuppressStrugglingToNavigate, (IEntity) this);
      this.PfState = this.handleState();
      base.SimUpdateInternal();
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.PfTask.Clear(false);
      this.m_navGoal = Option<IVehicleGoalFull>.None;
    }

    private PathFindingEntityState handleState()
    {
      switch (this.PfState)
      {
        case PathFindingEntityState.Idle:
          return PathFindingEntityState.Idle;
        case PathFindingEntityState.PathFinding:
        case PathFindingEntityState.ReadyToDriveToDestination:
          return this.handlePathFinding();
        case PathFindingEntityState.DrivingToDestination:
          PathFindingEntityState destination = this.handleDrivingToDestination();
          Assert.That<bool>(this.IsDriving || !this.PfTask.Result.HasNextPathSegment).IsTrue<PathFindingEntity, PathFindingEntityState, PathFindingEntityState>("Vehicle '{0}' has path but is not driving. Flawed driving logic? State transition: {1} => {2}", this, this.m_prevPfState, this.PfState);
          return destination;
        case PathFindingEntityState.DrivingToValidLocation:
          return this.handleDrivingToValidLocation();
        case PathFindingEntityState.FindingValidLocation:
          return this.handleFindingValidLocation();
        default:
          Log.Error(string.Format("Invalid state '{0}'.", (object) this.PfState));
          return PathFindingEntityState.Idle;
      }
    }

    private PathFindingEntityState handlePathFinding()
    {
      if (this.PfTask.IsFinished)
        return this.processPfResult();
      this.maintainDrivingTargetFromPath();
      return PathFindingEntityState.PathFinding;
    }

    public void SkipDrivingToDestination() => this.m_skipDrivingOneTime = true;

    /// <summary>
    /// Called when an external PF job failed to navigate on this vehicle's behalf.
    /// </summary>
    public void IncNavigationFailedStreak()
    {
      ++this.NavigationFailedStreak;
      this.m_navigationFailedStepsSinceLast = 0;
      this.NavigationFailed = true;
    }

    private PathFindingEntityState processPfResult()
    {
      Assert.That<bool>(this.PfTask.IsFinished).IsTrue();
      Assert.That<IPathFindingVehicle>(this.PfTask.Vehicle).IsEqualTo<IPathFindingVehicle>((IPathFindingVehicle) this);
      if (this.PfTask.Result.ResultStatus == VehiclePfResultStatus.StartInvalid)
      {
        Assert.That<bool>(this.IsPathable(this.GroundPositionTile2i)).IsFalse();
        this.StopDriving();
        return this.driveToValidLocation(true);
      }
      if (this.PfTask.Result.ResultStatus != VehiclePfResultStatus.PathFound)
      {
        if (!this.isValidArea(this.GroundPositionTile2i))
        {
          this.m_retryPfAfterFindingValidLocation = true;
          return PathFindingEntityState.FindingValidLocation;
        }
        this.StopNavigating();
        this.IncNavigationFailedStreak();
        return PathFindingEntityState.Idle;
      }
      if (this.PfState != PathFindingEntityState.ReadyToDriveToDestination)
        this.m_navGoal.Value.NotifyGoalFound(this.Prototype.PathFindingParams.ConvertToCenterTileSpace2i(this.PfTask.Result.GoalRawTile));
      if (this.m_skipDrivingOneTime)
      {
        this.m_skipDrivingOneTime = false;
        return PathFindingEntityState.ReadyToDriveToDestination;
      }
      this.NavigationFailedStreak = 0;
      this.m_navigationFailedStepsSinceLast = 0;
      this.m_currentTerrainPathSegment = Option<VehicleTerrainPathSegment>.None;
      this.m_currentRoadPathSegment = Option<VehicleRoadPathSegment>.None;
      if (this.PfTask.Result.HasNextPathSegment)
      {
        Assert.That<long>(this.PfTask.DistanceEstimationStartTile.DistanceSqrTo(this.GroundPositionTile2i)).IsLess(PathFindingEntity.MAX_DIST_DURING_PF_SQR, "Vehicle moved too much from the pathfinding start.");
        this.maintainDrivingTargetFromPath();
      }
      else
        this.SetDrivingTarget(this.Position2f, true);
      return PathFindingEntityState.DrivingToDestination;
    }

    private PathFindingEntityState handleDrivingToDestination()
    {
      Assert.That<Option<IVehicleGoalFull>>(this.m_navGoal).HasValue<IVehicleGoalFull>();
      Assert.That<bool>(this.PfTask.IsFinished).IsTrue();
      Assert.That<bool>(this.NavigatedSuccessfully).IsFalse();
      if (!this.maintainDrivingTargetFromPath() && !this.IsDriving && (!(this.m_navGoal.Value is DynamicEntityVehicleGoal entityVehicleGoal) || !entityVehicleGoal.GoalVehicle.Value.IsNavigating))
      {
        this.NavigatedSuccessfully = true;
        this.NavigationFailedStreak = 0;
        this.m_navigationFailedStepsSinceLast = 0;
        return PathFindingEntityState.Idle;
      }
      if (this.m_pathCheckTile == this.GroundPositionTile2i)
        return PathFindingEntityState.DrivingToDestination;
      this.m_pathCheckTile = this.GroundPositionTile2i;
      bool retryPf;
      if (this.m_navGoal.Value.IsGoalValid(this, out retryPf))
      {
        if (this.checkPathValid())
          return PathFindingEntityState.DrivingToDestination;
        retryPf = true;
      }
      if (retryPf)
        return this.startPathFinding(true, this.PfTask.MaxRetries, this.PfTask.ExtraTolerancePerRetry, this.PfTask.AllowSimplePathOnly, false, this.PfTask.NavigateClosebyIsSufficient);
      this.StopNavigating();
      return PathFindingEntityState.Idle;
    }

    private bool checkPathValid()
    {
      Assert.That<bool>(this.IsNavigating).IsTrue();
      if (this.m_currentTerrainPathSegment.HasValue)
      {
        LystStruct<Tile2i> pathReversed = this.m_currentTerrainPathSegment.Value.PathRawReversed;
        if (pathReversed.IsNotEmpty)
        {
          foreach (Vector2i coord in new LineRasterizer(this.m_targetRaw.Vector2i, pathReversed.Last.Vector2i, false))
          {
            if (!this.PathabilityProvider.IsPathableRaw(new Tile2i(coord), this.PathabilityMask))
            {
              if (DebugGameRendererConfig.SaveVehicleGoalReplanDueToBlock)
                drawReplanningDebugImage(new Tile2i(coord));
              return false;
            }
          }
          int index1 = pathReversed.Count - 2;
          if (index1 >= 0)
          {
            if (!this.PathabilityProvider.IsPathableRaw(pathReversed[index1], this.PathabilityMask))
            {
              if (DebugGameRendererConfig.SaveVehicleGoalReplanDueToBlock)
                drawReplanningDebugImage(pathReversed[index1]);
              return false;
            }
            int index2 = index1 - 1;
            if (index2 >= 0 && !this.PathabilityProvider.IsPathableRaw(pathReversed[index2], this.PathabilityMask))
            {
              if (DebugGameRendererConfig.SaveVehicleGoalReplanDueToBlock)
                drawReplanningDebugImage(pathReversed[index2]);
              return false;
            }
          }
        }
        return !(this.m_currentTerrainPathSegment.Value.NextSegment.ValueOrNull is VehicleRoadPathSegment valueOrNull) || !valueOrNull.Path.IsNotEmpty || valueOrNull.Path.First.Entity.IsConstructed;

        void drawReplanningDebugImage(Tile2i offendingTile)
        {
          this.DEBUG_DrawPfResultMap().DrawLine(this.PathFindingParams.ConvertToCenterTileSpace(this.m_targetRaw), this.PathFindingParams.ConvertToCenterTileSpace(pathReversed.Last), ColorRgba.Red.SetA((byte) 64)).DrawCross(this.PathFindingParams.ConvertToCenterTileSpace(offendingTile), ColorRgba.Red).SaveMapAsTga(string.Format("{0}_replanning_terrain-issue", (object) this));
        }
      }
      if (!this.m_currentRoadPathSegment.HasValue)
        return true;
      ImmutableArray<RoadPathSegment> path = this.m_currentRoadPathSegment.Value.Path;
      int num = 0;
      for (int roadSegmentIndex = this.CurrentRoadSegmentIndex; num < 5 && roadSegmentIndex < path.Length; ++roadSegmentIndex)
      {
        if (!path[roadSegmentIndex].Entity.IsConstructed)
        {
          if (DebugGameRendererConfig.SaveVehicleGoalReplanDueToBlock)
            this.DEBUG_DrawPfResultMap().DrawRoadEntity(path[roadSegmentIndex].Entity, new ColorRgba?(ColorRgba.Red.SetA((byte) 64))).SaveMapAsTga(string.Format("{0}_replanning_road-issue", (object) this));
          return false;
        }
        ++num;
      }
      return true;
    }

    private PathFindingEntityState handleFindingValidLocation()
    {
      if (this.m_noValidLocationNearbyCooldown <= 0)
      {
        Tile2i? closestValidLocation = this.findClosestValidLocation();
        if (closestValidLocation.HasValue)
        {
          this.SetDrivingTarget(closestValidLocation.Value.CenterTile2f, true);
          if (DebugGameRendererConfig.SaveVehicleGettingUnstuck)
            drawAndSaveDebugMap("GettingUnstuck_FirstTrial");
          return PathFindingEntityState.DrivingToValidLocation;
        }
        this.m_noValidLocationNearbyCooldown += 10;
        return PathFindingEntityState.FindingValidLocation;
      }
      --this.m_noValidLocationNearbyCooldown;
      if (this.PathabilityProvider.IsPathableIgnoringTerrain(this.GroundPositionTile2i, this.PathabilityMask))
        return PathFindingEntityState.FindingValidLocation;
      Tile2i? closestValidPfNode = this.m_pathFindingManager.FindClosestValidPfNode(this.GroundPositionTile2i, this.PathFindingParams, (Predicate<PfNode>) (pfNode => pfNode.CurrentNeighbors.Count >= 2));
      if (closestValidPfNode.HasValue)
      {
        this.SetDrivingTarget(closestValidPfNode.Value.CenterTile2f, true, longDistanceIsOk: true);
        if (DebugGameRendererConfig.SaveVehicleGettingUnstuck)
          drawAndSaveDebugMap("GettingUnstuck_SecondTrial");
        return PathFindingEntityState.DrivingToValidLocation;
      }
      this.StopNavigating();
      if (this is Vehicle vehicle)
      {
        this.VehiclesManager.TeleportVehicleToAnyValidPosition(vehicle);
      }
      else
      {
        Log.Error(string.Format("No valid locations for stuck vehicle {0} at {1}.", (object) this, (object) this.GroundPositionTile2i));
        if (DebugGameRendererConfig.SaveVehicleGettingUnstuck)
          drawAndSaveDebugMap("Stuck");
      }
      return PathFindingEntityState.Idle;

      void drawAndSaveDebugMap(string name)
      {
        if (DebugGameRenderer.IsDisabled)
          return;
        Tile2i groundPositionTile2i = this.GroundPositionTile2i;
        Tile2i tile2i1 = this.CurrentOrLastDrivingTarget.Tile2i;
        Tile2i from = groundPositionTile2i.Min(tile2i1).AddXy(-10);
        Tile2i tile2i2 = groundPositionTile2i.Max(tile2i1).AddXy(10);
        DebugGameRenderer.DrawGameImage(from, tile2i2 - from).DrawPathabilityOverlayFor(this).DrawCross(this.Position2f, new ColorRgba((int) byte.MaxValue, 0, 0, (int) byte.MaxValue)).DrawCrossAlt(this.Target.Value, new ColorRgba(0, 128, (int) byte.MaxValue, (int) byte.MaxValue)).SaveMapAsTga(name);
      }
    }

    private PathFindingEntityState handleDrivingToValidLocation()
    {
      if (this.IsDriving)
        return PathFindingEntityState.DrivingToValidLocation;
      return this.m_retryPfAfterFindingValidLocation ? this.startPathFinding(false, this.PfTask.MaxRetries, this.PfTask.ExtraTolerancePerRetry, this.PfTask.AllowSimplePathOnly, false, this.PfTask.NavigateClosebyIsSufficient) : PathFindingEntityState.Idle;
    }

    private bool maintainDrivingTargetFromPath()
    {
      if ((this.m_currentTerrainPathSegment.HasValue || this.m_currentRoadPathSegment.HasValue) && this.IsDriving && !this.ShouldGetNextTarget())
        return true;
      if (this.m_currentTerrainPathSegment.HasValue && this.m_currentTerrainPathSegment.Value.PathRawReversed.IsNotEmpty)
      {
        this.continueDrivingOnTerrain(this.m_currentTerrainPathSegment.Value);
        return true;
      }
      this.m_currentTerrainPathSegment = Option<VehicleTerrainPathSegment>.None;
      this.m_currentRoadPathSegment = Option<VehicleRoadPathSegment>.None;
      if (!this.PfTask.Result.HasNextPathSegment)
        return false;
      IVehiclePathSegment vehiclePathSegment = this.PfTask.Result.PopNextPathSegment();
      switch (vehiclePathSegment)
      {
        case VehicleTerrainPathSegment segment:
          this.m_currentTerrainPathSegment = (Option<VehicleTerrainPathSegment>) segment;
          if (segment.PathRawReversed.IsNotEmpty)
          {
            this.continueDrivingOnTerrain(segment);
            return true;
          }
          break;
        case VehicleRoadPathSegment vehicleRoadPathSegment:
          this.m_currentRoadPathSegment = (Option<VehicleRoadPathSegment>) vehicleRoadPathSegment;
          this.SetDrivingTarget(vehicleRoadPathSegment.Path);
          return true;
        default:
          Log.Error("Unknown segment type '" + vehiclePathSegment.GetType().Name + "'");
          break;
      }
      return this.PfTask.Result.HasNextPathSegment;
    }

    private void continueDrivingOnTerrain(VehicleTerrainPathSegment segment)
    {
      this.m_targetRaw = segment.PathRawReversed.PopLast();
      Tile2f centerTileSpace = this.Prototype.PathFindingParams.ConvertToCenterTileSpace(this.m_targetRaw);
      if (DebugGameRendererConfig.SaveSuspiciouslyLongVehicleDriveTargets && centerTileSpace.DistanceSqrTo(this.Position2f) >= PathFindingEntity.LONG_DRIVE_MAX_DIST_SQR)
        this.DEBUG_DrawPfResultMap().DrawLine(this.Position2f, centerTileSpace, ColorRgba.Red).SaveMapAsTga("LongDrive");
      Assert.That<Fix64>(centerTileSpace.DistanceSqrTo(this.Position2f)).IsLess(PathFindingEntity.LONG_DRIVE_MAX_DIST_SQR);
      this.SetDrivingTarget(centerTileSpace, segment.PathRawReversed.IsEmpty);
    }

    /// <summary>
    /// Checks if the goal is valid and restarts path finding if needed. Returns true if goal is valid and
    /// additionally, <paramref name="pathFindingRetried" /> is set path finding was retried. Otherwise, if goal is
    /// not valid, false is returned. If false is returned, the <paramref name="pathFindingRetried" /> is always
    /// false.
    /// </summary>
    public bool CheckGoalValidityAndRetryNavigationIfNeeded(out bool pathFindingRetried)
    {
      Assert.That<Option<IVehicleGoalFull>>(this.m_navGoal).HasValue<IVehicleGoalFull>();
      bool retryPf;
      if (this.m_navGoal.Value.IsGoalValid(this, out retryPf))
      {
        pathFindingRetried = false;
        return true;
      }
      if (retryPf)
      {
        this.PfState = this.startPathFinding(false, this.PfTask.MaxRetries, this.PfTask.ExtraTolerancePerRetry, this.PfTask.AllowSimplePathOnly, true, this.PfTask.NavigateClosebyIsSufficient);
        pathFindingRetried = true;
        return true;
      }
      pathFindingRetried = false;
      return false;
    }

    public void DriveToValidLocation()
    {
      Assert.That<PathFindingEntityState>(this.PfState).IsEqualTo<PathFindingEntityState>(PathFindingEntityState.Idle);
      this.PfState = this.driveToValidLocation(false);
    }

    private PathFindingEntityState driveToValidLocation(bool retryPf)
    {
      Assert.That<bool>(this.IsDriving).IsFalse();
      this.m_retryPfAfterFindingValidLocation = retryPf;
      return PathFindingEntityState.FindingValidLocation;
    }

    private Tile2i? findClosestValidLocation()
    {
      if (this.isValidArea(this.LastValidPosition))
        return new Tile2i?(this.LastValidPosition);
      if (this.isValidArea(this.PreLastValidPosition))
        return new Tile2i?(this.PreLastValidPosition);
      Tile2i origin = this.GroundPositionTile2i;
      Tile2i? tile = new Tile2i?();
      TerrainManager terrainManager = this.Context.TerrainManager;
      HeightTilesF baseHeight = terrainManager.GetHeight(this.GroundPositionTile2i);
      int r = 1;
      Fix32 bestScore = Fix32.MinValue;
      for (; r <= 13; r += 2)
      {
        MafiMath.IterateCirclePoints(r, new Action<int, int>(testCoord));
        if (bestScore >= -r)
        {
          Assert.That<Tile2i?>(tile).IsNotNull<Tile2i>();
          break;
        }
      }
      return tile;

      void testCoord(int dx, int dy)
      {
        Tile2i tile2i = origin + new RelTile2i(dx, dy);
        Fix32 fix32_1 = (terrainManager.GetHeight(tile2i) - baseHeight).Value;
        if (fix32_1 > r)
          return;
        Fix32 fix32_2 = -((Fix32) r + fix32_1.Abs() * (fix32_1 > 0 ? Fix32.One : Fix32.Half));
        if (!(fix32_2 >= bestScore) || !this.isValidArea(tile2i))
          return;
        tile = new Tile2i?(tile2i);
        bestScore = fix32_2;
      }
    }

    private bool isValidArea(Tile2i position)
    {
      if (!this.IsPathable(position))
        return false;
      if (this.IsPathable(position.IncrementX) && this.IsPathable(position.DecrementX))
        return true;
      return this.IsPathable(position.IncrementY) && this.IsPathable(position.DecrementY);
    }

    public void SetUnreachableGoal(IVehicleGoal unreachableGoal)
    {
      this.UnreachableGoal = unreachableGoal.SomeOption<IVehicleGoal>();
      if (!DebugGameRendererConfig.SaveVehicleGoalUnreachable || !DebugGameRenderer.IsEnabled)
        return;
      Tile2f xy = unreachableGoal.GetGoalPosition().Xy;
      this.DEBUG_DrawPfResultMap().DrawDynamicEntity((DynamicGroundEntity) this, ColorRgba.White).DrawCross(xy, ColorRgba.White).DrawLine(this.Position2f, xy, ColorRgba.Red).SaveMapAsTga("VehicleGoalUnreachable");
    }

    public void ClearUnreachableGoal() => this.UnreachableGoal = Option<IVehicleGoal>.None;

    internal DebugGameMapDrawing DEBUG_DrawPfResultMap(bool forceEnable = false, int pixelsPerTile = 9)
    {
      return this.PfTask.DEBUG_DrawResultMap(forceEnable, pixelsPerTile);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<VehicleRoadPathSegment>.Serialize(this.m_currentRoadPathSegment, writer);
      Option<VehicleTerrainPathSegment>.Serialize(this.m_currentTerrainPathSegment, writer);
      Option<IVehicleGoalFull>.Serialize(this.m_navGoal, writer);
      writer.WriteInt(this.m_navigationFailedStepsSinceFirst);
      writer.WriteInt(this.m_navigationFailedStepsSinceLast);
      writer.WriteInt(this.m_noValidLocationNearbyCooldown);
      Tile2i.Serialize(this.m_pathCheckTile, writer);
      writer.WriteGeneric<IVehiclePathFindingManager>(this.m_pathFindingManager);
      writer.WriteInt((int) this.m_prevPfState);
      writer.WriteBool(this.m_retryPfAfterFindingValidLocation);
      writer.WriteBool(this.m_skipDrivingOneTime);
      EntityNotificator.Serialize(this.m_strugglingToNavigateNotif, writer);
      Tile2i.Serialize(this.m_targetRaw, writer);
      writer.WriteBool(this.NavigatedSuccessfully);
      writer.WriteBool(this.NavigationFailed);
      writer.WriteInt(this.NavigationFailedStreak);
      writer.WriteInt((int) this.PfState);
      VehiclePathFindingTask.Serialize(this.PfTask, writer);
      writer.WriteBool(this.TrackExploredTiles);
      Option<IVehicleGoal>.Serialize(this.UnreachableGoal, writer);
      writer.WriteGeneric<IVehiclesManager>(this.VehiclesManager);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_currentRoadPathSegment = reader.LoadedSaveVersion >= 140 ? Option<VehicleRoadPathSegment>.Deserialize(reader) : new Option<VehicleRoadPathSegment>();
      this.m_currentTerrainPathSegment = reader.LoadedSaveVersion >= 140 ? Option<VehicleTerrainPathSegment>.Deserialize(reader) : new Option<VehicleTerrainPathSegment>();
      if (reader.LoadedSaveVersion < 140)
        this.m_goalUnreachableNotif = EntityNotificator.Deserialize(reader);
      this.m_navGoal = Option<IVehicleGoalFull>.Deserialize(reader);
      this.m_navigationFailedStepsSinceFirst = reader.LoadedSaveVersion >= 105 ? reader.ReadInt() : 0;
      this.m_navigationFailedStepsSinceLast = reader.LoadedSaveVersion >= 154 ? reader.ReadInt() : 0;
      if (reader.LoadedSaveVersion < 166)
        this.m_noValidLocationNearby = reader.ReadBool();
      this.m_noValidLocationNearbyCooldown = reader.LoadedSaveVersion >= 166 ? reader.ReadInt() : 0;
      this.m_pathCheckTile = Tile2i.Deserialize(reader);
      reader.SetField<PathFindingEntity>(this, "m_pathFindingManager", (object) reader.ReadGenericAs<IVehiclePathFindingManager>());
      this.m_prevPfState = (PathFindingEntityState) reader.ReadInt();
      this.m_retryPfAfterFindingValidLocation = reader.ReadBool();
      this.m_skipDrivingOneTime = reader.LoadedSaveVersion >= 140 && reader.ReadBool();
      this.m_strugglingToNavigateNotif = reader.LoadedSaveVersion >= 105 ? EntityNotificator.Deserialize(reader) : new EntityNotificator();
      this.m_targetRaw = Tile2i.Deserialize(reader);
      this.NavigatedSuccessfully = reader.ReadBool();
      this.NavigationFailed = reader.ReadBool();
      this.NavigationFailedStreak = reader.LoadedSaveVersion >= 105 ? reader.ReadInt() : 0;
      this.PfState = (PathFindingEntityState) reader.ReadInt();
      this.PfTask = VehiclePathFindingTask.Deserialize(reader);
      this.TrackExploredTiles = reader.ReadBool();
      this.UnreachableGoal = Option<IVehicleGoal>.Deserialize(reader);
      reader.SetField<PathFindingEntity>(this, "VehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
      reader.RegisterInitAfterLoad<PathFindingEntity>(this, "initSelf", InitPriority.Normal);
    }

    static PathFindingEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      PathFindingEntity.LONG_DRIVE_MAX_DIST = 3 * 8.Tiles();
      PathFindingEntity.LONG_DRIVE_MAX_DIST_SQR = (Fix64) PathFindingEntity.LONG_DRIVE_MAX_DIST.Squared;
      PathFindingEntity.MAX_DIST_DURING_PF_SQR = (long) 16.Squared();
      PathFindingEntity.STRUGGLING_STREAK_NOTIF_THRESHOLD_FAILURES = 5;
      PathFindingEntity.STRUGGLING_STREAK_NOTIF_THRESHOLD_TICKS = 30.Seconds().Ticks;
      PathFindingEntity.STRUGGLING_STREAK_NOTIF_THRESHOLD_FAILURES_LONG = 2;
      PathFindingEntity.STRUGGLING_STREAK_NOTIF_THRESHOLD_TICKS_LONG = 80.Seconds().Ticks;
      PathFindingEntity.STRUGGLING_STREAK_NOTIF_HIDE_AFTER_TICKS = 90.Seconds().Ticks;
      PathFindingEntity.STRUGGLING_STREAK_PRIO_PENALTY = 37.Ticks();
      PathFindingEntity.STRUGGLING_STREAK_MAX_PENALTY = 151.Ticks();
    }
  }
}
