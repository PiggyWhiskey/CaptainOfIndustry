// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.DrivingEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.PathFinding;
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
  /// Driving entity is able to drive to given target ignoring all obstacles.
  /// </summary>
  public abstract class DrivingEntity : DynamicGroundEntity
  {
    public static readonly RelTile1f DEFAULT_DRIVING_TOLERANCE;
    public static readonly AngleDegrees1f MAX_GOAL_ANGLE_BEFORE_STARTING;
    private static readonly Fix64 DRIVING_STOP_TRESHOLD_SQR;
    public readonly DrivingEntityProto Prototype;
    /// <summary>
    /// The goal is considered behind if angle to goal is greater than this value.
    /// </summary>
    private static readonly AngleDegrees1f s_goalBehindAngle;
    /// <summary>
    /// The goal is considered in front if the angle is less than this value.
    /// </summary>
    private static readonly AngleDegrees1f s_goalInFrontAngle;
    /// <summary>
    /// Current target only valid when driving. When not driving, this is the target of the last drive command.
    /// This variable is intentionally not nullable to avoid null checks.
    /// </summary>
    protected Tile2f DrivingTarget;
    /// <summary>
    /// Tolerance of reaching target. This tolerance is used when entity is about to stop or is already standing (new
    /// target). This should be large enough to allow entity to reach its target even if it is a little off but small
    /// enough to not introduce any noticeable imprecisions.
    /// </summary>
    private RelTile1f m_targetTolerance;
    /// <summary>State of this vehicle. This controls its behavior.</summary>
    private DrivingState m_state;
    private DrivingState m_previousState;
    private readonly SmoothDriver m_speedDriver;
    private readonly SmoothDriver m_steeringDriver;
    private bool m_driveBackwards;
    protected readonly IPathabilityProvider PathabilityProvider;
    protected readonly ulong PathabilityMask;
    protected readonly ulong SingleTilePathabilityMask;
    [NewInSaveVersion(140, null, "ImmutableArray<RoadPathSegment>.Empty", null, null)]
    private ImmutableArray<RoadPathSegment> m_roadSegments;
    [NewInSaveVersion(140, null, null, null, null)]
    private int m_currentRoadSegmentIndex;
    [NewInSaveVersion(140, null, null, null, null)]
    private int m_currentRoadTrajIndex;
    [NewInSaveVersion(140, null, null, null, null)]
    private bool m_approachingRoad;
    [DoNotSave(0, null)]
    private Fix32 m_remainingRoadDistance;
    [DoNotSave(0, null)]
    private RoadPathSegment m_currentRoadSegment;
    [DoNotSave(0, null)]
    private RoadLaneTrajectory m_currentRoadTraj;

    /// <summary>Driving data.</summary>
    public DrivingData DrivingData => this.Prototype.DrivingData;

    /// <summary>
    /// Current target if any. Is valid only when <see cref="P:Mafi.Core.Entities.Dynamic.DrivingEntity.IsDriving" /> is true.
    /// </summary>
    public Tile2f? Target => !this.IsDriving ? new Tile2f?() : new Tile2f?(this.DrivingTarget);

    public Tile2f CurrentOrLastDrivingTarget => this.DrivingTarget;

    /// <summary>
    /// Whether this entity is actively driving somewhere. This is the main flag to query to know whether the entity
    /// is driving. Note that the vehicle may be still moving a little bit when <see cref="P:Mafi.Core.Entities.Dynamic.DrivingEntity.IsDriving" /> is false.
    /// </summary>
    public bool IsDriving { get; private set; }

    public bool IsMoving
    {
      get => this.m_speedDriver.Speed.IsNotZero || this.m_steeringDriver.Speed.IsNotZero;
    }

    public override RelTile1f Speed => new RelTile1f(this.m_speedDriver.Speed);

    /// <summary>
    /// Speed as a percent of max a vehicle can achieve. -ve =&gt; reversing
    /// </summary>
    public Percent SpeedPercentOfPeak => this.m_speedDriver.SpeedPercentBase;

    /// <summary>
    /// Acceleration as a percent of max a vehicle can achieve. Positive when speeding-up, negative when slowing down.
    /// </summary>
    public Percent AccelerationPercentOfPeak => this.m_speedDriver.AccelerationPercentBase;

    /// <summary>
    /// Current angle of steering relative to the vehicle's <see cref="P:Mafi.Core.Entities.Dynamic.DynamicGroundEntity.Direction" />. For vehicles
    /// that can turn in place this is turning velocity per tick.
    /// </summary>
    public AngleDegrees1f SteeringAngle => this.m_steeringDriver.Speed.Degrees();

    public Percent SteeringAccelerationPercent => this.m_steeringDriver.AccelerationPercent;

    /// <summary>
    /// Braking distance of this entity. That's total distance traveled when braking at 100% every step. Returned
    /// value is always non-negative.
    /// </summary>
    public RelTile1f DistanceToFullStop => new RelTile1f(this.m_speedDriver.DistanceToFullStop);

    public abstract bool IsEngineOn { get; }

    /// <summary>
    /// Whether current target is intermediate. Driver will not attempt to brake when reaching an intermediate target
    /// but rather drive through it at the full speed.
    /// </summary>
    public bool TargetIsTerminal { get; private set; }

    public DrivingState DrivingState => this.m_state;

    /// <summary>
    /// Speed factor for animations. 100% means default speed, 50% means speed is halved (timings are doubled).
    /// </summary>
    public Percent SpeedFactor { get; private set; }

    protected Tile2i LastValidPosition { get; private set; }

    protected Tile2i PreLastValidPosition { get; private set; }

    protected int CurrentRoadSegmentIndex => this.m_currentRoadSegmentIndex;

    public RoadPathSegment CurrentRoadSegmentOrDefault => this.m_currentRoadSegment;

    public bool IsDrivingOnRoad => this.m_currentRoadSegment.Entity != null;

    protected DrivingEntity(
      EntityId id,
      DrivingEntityProto prototype,
      EntityContext context,
      Tile2f initialPosition,
      TerrainManager terrain,
      IVehicleSurfaceProvider surfaceProvider,
      IPathabilityProvider pathabilityProvider)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CSpeedFactor\u003Ek__BackingField = Percent.Hundred;
      this.m_roadSegments = ImmutableArray<RoadPathSegment>.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (DynamicGroundEntityProto) prototype, context, initialPosition, terrain, surfaceProvider);
      this.Prototype = prototype;
      this.PathabilityProvider = pathabilityProvider;
      this.PathabilityMask = pathabilityProvider.GetPathabilityMask(prototype.PathFindingParams);
      this.SingleTilePathabilityMask = pathabilityProvider.GetPathabilityMaskSingleTile(prototype.PathFindingParams);
      this.m_speedDriver = new SmoothDriver(this.DrivingData.MaxForwardsSpeed.Value, this.DrivingData.MaxBackwardsSpeed.Value, this.DrivingData.Acceleration.Value, this.DrivingData.Braking.Value, this.DrivingData.BrakingConservativness);
      this.m_steeringDriver = new SmoothDriver(this.DrivingData.MaxSteeringAngle.Degrees, this.DrivingData.MaxSteeringAngle.Degrees, this.DrivingData.MaxSteeringSpeed.Degrees, this.DrivingData.MaxSteeringSpeed.Degrees, (Fix32) 3);
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad()
    {
      if (this.m_currentRoadSegmentIndex >= this.m_roadSegments.Length)
        return;
      this.m_currentRoadSegment = this.m_roadSegments[this.m_currentRoadSegmentIndex];
      this.m_currentRoadTraj = this.m_currentRoadSegment.Entity.GetTransformedRoadLane(this.m_currentRoadSegment.LaneIndex);
      ImmutableArray<RelTile1f> lengthsPrefixSums = this.m_currentRoadTraj.SegmentLengthsPrefixSums;
      if (this.m_currentRoadTrajIndex < lengthsPrefixSums.Length)
        this.m_remainingRoadDistance = (lengthsPrefixSums.Last - lengthsPrefixSums[this.m_currentRoadTrajIndex]).Value;
      for (int index = this.m_currentRoadSegmentIndex + 1; index < this.m_roadSegments.Length; ++index)
      {
        RoadPathSegment roadSegment = this.m_roadSegments[index];
        this.m_remainingRoadDistance += roadSegment.Entity.RoadProto.LanesData[roadSegment.LaneIndex].LaneLength.Value;
      }
    }

    public bool IsPathable(Tile2i pos)
    {
      return this.PathabilityProvider.IsPathable(pos, this.PathabilityMask);
    }

    /// <summary>Sets speed of this entity. This is for tests.</summary>
    internal void TestOnly_SetSpeed(RelTile1f speed)
    {
      this.m_speedDriver.SetSpeed(speed.Value);
      if (speed.IsPositive)
      {
        this.m_state = DrivingState.DrivingForwards;
      }
      else
      {
        if (!speed.IsNegative)
          return;
        this.m_state = DrivingState.DrivingBackwards;
      }
    }

    internal void TestOnly_SetSteering(AngleDegrees1f angle)
    {
      this.m_steeringDriver.SetSpeed(angle.Degrees);
    }

    internal void TestOnly_PerformMoveStep() => this.performMoveStep();

    /// <summary>
    /// Whether this vehicle can keep max speed to given target.
    /// </summary>
    protected bool ShouldGetNextTarget()
    {
      return this.m_currentRoadSegment.Entity == null && (this.m_speedDriver.DistanceToFullStop + this.Prototype.EntitySize.X.HalfFast).Squared() >= this.Position2f.DistanceSqrTo(this.DrivingTarget);
    }

    /// <summary>
    /// Drives this entity to given <paramref name="target" />. If <paramref name="isTerminal" /> is set to
    /// <c>false</c> this target is not terminal and the driver will not try to stop at the given goal but rather
    /// drive through it at full speed (and then just brake at full speed if no other target is given).
    /// </summary>
    public bool SetDrivingTarget(
      Tile2f target,
      bool isTerminal,
      RelTile1f tolerance = default (RelTile1f),
      bool driveBackwards = false,
      bool longDistanceIsOk = false)
    {
      Assert.That<RelTile1f>(tolerance).IsNotNegative();
      Assert.That<bool>(!driveBackwards || this.DrivingData.CanTurnInPlace).IsTrue("Driving backwards is only functional for vehicles that can turn in place.");
      if (!longDistanceIsOk && target.DistanceSqrTo(this.Position2f) > 1024)
      {
        if (DebugGameRendererConfig.SaveVehicleDriveTooFarWithoutPf)
          DebugGameRenderer.DrawGameImage(this.Position2f.Tile2i, target.Tile2i, 20).DrawPathabilityOverlayFor((PathFindingEntity) this).DrawLine(this.Position2f, target, ColorRgba.Red).SaveMapAsTga("TargetTooFar");
        Log.Warning(string.Format("Driving target of {0} is {1} tiles away. ", (object) this, (object) target.DistanceTo(this.Position2f)) + "Did you meant to use path finding instead?");
      }
      this.DrivingTarget = target;
      this.TargetIsTerminal = isTerminal;
      this.m_targetTolerance = tolerance.IsPositive ? tolerance : DrivingEntity.DEFAULT_DRIVING_TOLERANCE;
      this.m_driveBackwards = driveBackwards;
      this.m_approachingRoad = false;
      Fix64 lengthSqr = (target - this.Position2f).LengthSqr;
      if (!this.IsDriving && !this.IsMoving && lengthSqr < this.m_targetTolerance.Squared)
      {
        this.m_state = DrivingState.Stopped;
        return false;
      }
      this.IsDriving = true;
      return true;
    }

    public bool SetDrivingTarget(
      ImmutableArray<RoadPathSegment> roadSegments,
      bool longDistanceIsOk = false)
    {
      if (this.m_currentRoadSegment.Entity != null)
      {
        for (int index = 0; index < roadSegments.Length; ++index)
        {
          if (roadSegments[index] == this.m_currentRoadSegment)
            this.m_currentRoadSegmentIndex = index;
        }
      }
      this.m_roadSegments = roadSegments;
      this.m_currentRoadSegmentIndex = 0;
      this.m_currentRoadTrajIndex = 0;
      if (roadSegments.IsNotValidOrEmpty)
      {
        Log.Warning("Setting empty road driving target.");
        this.m_roadSegments = ImmutableArray<RoadPathSegment>.Empty;
        return false;
      }
      this.m_currentRoadSegment = this.m_roadSegments.First;
      this.m_currentRoadTraj = this.m_currentRoadSegment.Entity.GetTransformedRoadLane(this.m_currentRoadSegment.LaneIndex);
      this.m_remainingRoadDistance = roadSegments.Sum((Func<RoadPathSegment, Fix32>) (x => x.Entity.RoadProto.LanesData[x.LaneIndex].LaneLength.Value));
      Tile3f tile3f = this.m_currentRoadTraj.LaneCenterSamples.First + this.m_currentRoadSegment.Entity.CenterTile.CornerTile3f;
      if (!tile3f.Height.IsNear(this.Terrain.GetHeight(tile3f.Xy), HeightTilesF.Half))
        Log.Warning("Start of the road is not on the ground.");
      Tile2f xy = tile3f.Xy;
      bool flag = longDistanceIsOk;
      RelTile1f tolerance = new RelTile1f();
      int num = flag ? 1 : 0;
      if (!this.SetDrivingTarget(xy, false, tolerance, longDistanceIsOk: num != 0))
        return false;
      this.m_approachingRoad = true;
      return true;
    }

    public void SetTurningTarget(AngleDegrees1f targetDirection)
    {
      this.SetTurningTarget(this.Position2f + new RelTile2f(targetDirection.DirectionVector));
    }

    public void SetTurningTarget(Tile2f turnTowardsTarget)
    {
      Assert.That<bool>(this.IsDriving).IsFalse();
      Assert.That<bool>(this.IsMoving).IsFalse();
      this.DrivingTarget = turnTowardsTarget;
      this.TargetIsTerminal = true;
      this.m_state = DrivingState.TurningInPlace;
      this.m_driveBackwards = false;
      this.IsDriving = true;
    }

    /// <summary>Stops driving and resets the target.</summary>
    public void StopDriving()
    {
      this.IsDriving = false;
      this.m_approachingRoad = false;
      if (this.m_state == DrivingState.Paused)
        return;
      this.m_state = this.IsMoving ? DrivingState.Stopping : DrivingState.Stopped;
    }

    protected override void OnNewTile2iVisited(Tile2i oldPosition)
    {
      base.OnNewTile2iVisited(oldPosition);
      if (!this.IsPathable(oldPosition))
        return;
      this.PreLastValidPosition = this.LastValidPosition;
      this.LastValidPosition = oldPosition;
    }

    public override void Spawn(Tile2f position, AngleDegrees1f direction)
    {
      base.Spawn(position, direction);
      this.LastValidPosition = this.PreLastValidPosition = this.GroundPositionTile2i;
    }

    internal override void TeleportTo(Tile2f position, AngleDegrees1f? angle = null)
    {
      base.TeleportTo(position, angle);
      this.LastValidPosition = this.PreLastValidPosition = this.GroundPositionTile2i;
    }

    /// <summary>
    /// Performs driving logic. This should be called by any inherited class. This should be called always regardless
    /// of <see cref="P:Mafi.Core.Entities.Dynamic.DrivingEntity.IsDriving" /> status because vehicle may be not driving but it still needs to decelerate and
    /// stop.
    /// </summary>
    protected override void SimUpdateInternal()
    {
      this.m_speedDriver.StartUpdate();
      this.m_steeringDriver.StartUpdate();
      DrivingState state = this.m_state;
      this.m_state = this.handleState();
      if (this.m_state == DrivingState.Stopped)
        return;
      if (this.m_state != DrivingState.DrivingForwardsOnRoad)
      {
        this.performMoveStep();
        if (this.m_state != DrivingState.Stopping && this.m_state != DrivingState.Paused && !this.m_approachingRoad && this.reachedGoal(new RelTile1f((this.DrivingTarget - this.Position2f).Length)))
          this.StopDriving();
      }
      this.m_previousState = state;
      base.SimUpdateInternal();
    }

    private DrivingState handleState()
    {
      switch (this.m_state)
      {
        case DrivingState.Stopped:
          return this.handleStopped();
        case DrivingState.Stopping:
          return this.handleStopping();
        case DrivingState.StopAndContinueForwards:
          return this.handleStopAndContinueForwards();
        case DrivingState.DrivingForwards:
          return this.handleDrivingForwards();
        case DrivingState.DrivingBackwards:
          return this.handleDrivingBackwards();
        case DrivingState.TurningInPlace:
          return this.handleTurningInPlace();
        case DrivingState.Paused:
          return this.handlePause();
        case DrivingState.StopAndContinueBackwards:
          return this.handleStopAndContinueBackwards();
        case DrivingState.DrivingForwardsOnRoad:
          return this.handleDrivingForwardsOnRoad();
        default:
          Assert.Fail(string.Format("Invalid state '{0}'.", (object) this.m_state));
          return DrivingState.Stopping;
      }
    }

    private DrivingState handlePause()
    {
      if (this.m_speedDriver.Speed.IsNotZero)
      {
        this.m_speedDriver.BrakeBy(Percent.Hundred);
        this.steerTowardsGoal(this.Speed < RelTile1f.Zero);
      }
      else if (this.m_steeringDriver.Speed.IsNotZero)
        this.m_steeringDriver.BrakeBy(Percent.Hundred);
      this.m_state = DrivingState.Stopping;
      return this.m_state;
    }

    /// <summary>
    /// Vehicle is at complete stop and is waiting for target.
    /// </summary>
    private DrivingState handleStopped()
    {
      Assert.That<Fix32>(this.m_speedDriver.Speed).IsZero();
      Assert.That<Fix32>(this.m_steeringDriver.Speed).IsZero();
      if (!this.IsDriving)
        return DrivingState.Stopped;
      return !this.m_driveBackwards ? DrivingState.DrivingForwards : DrivingState.DrivingBackwards;
    }

    /// <summary>
    /// Vehicle is moving but is not driving. This effectively brakes the vehicle to complete stop or resumes
    /// driving.
    /// </summary>
    private DrivingState handleStopping()
    {
      if (this.IsDriving)
        return !this.m_driveBackwards ? DrivingState.DrivingForwards : DrivingState.DrivingBackwards;
      if (this.m_speedDriver.Speed.IsNotZero)
      {
        this.m_speedDriver.BrakeBy(Percent.Hundred);
        this.steerTowardsGoal(this.Speed < RelTile1f.Zero);
        return DrivingState.Stopping;
      }
      if (!this.m_steeringDriver.Speed.IsNotZero)
        return DrivingState.Stopped;
      this.m_steeringDriver.BrakeBy(Percent.Hundred);
      return DrivingState.Stopping;
    }

    /// <summary>
    /// This is recovery state when vehicle hits steering singularity. This is solved by fully stopping. Driving
    /// forwards will keep steering when standing towards the goal.
    /// </summary>
    private DrivingState handleStopAndContinueForwards()
    {
      Assert.That<bool>(this.DrivingData.CanTurnInPlace).IsTrue();
      this.m_speedDriver.BrakeBy(Percent.Hundred);
      this.steerTowardsGoal(this.Speed < RelTile1f.Zero);
      return this.Speed.IsZero ? DrivingState.DrivingForwards : DrivingState.StopAndContinueForwards;
    }

    private DrivingState handleStopAndContinueBackwards()
    {
      Assert.That<bool>(this.DrivingData.CanTurnInPlace).IsTrue();
      this.m_speedDriver.BrakeBy(Percent.Hundred);
      this.steerTowardsGoal(this.Speed > RelTile1f.Zero);
      return this.Speed.IsZero ? DrivingState.DrivingBackwards : DrivingState.StopAndContinueBackwards;
    }

    /// <summary>
    /// Drives the vehicle forwards. If vehicle is currently moving backwards, it stops first and then drive
    /// forwards.
    /// </summary>
    private DrivingState handleDrivingForwards()
    {
      if (this.Speed < RelTile1f.Zero)
      {
        this.m_speedDriver.BrakeBy(Percent.Hundred);
        this.steerTowardsGoal(true);
        return DrivingState.DrivingForwards;
      }
      bool flag1 = this.TargetIsTerminal;
      RelTile1f relTile1f1 = this.DrivingData.MaxForwardsSpeed;
      if (this.m_currentRoadSegment.Entity != null)
      {
        if (!this.m_approachingRoad)
          return DrivingState.DrivingForwardsOnRoad;
        if (this.DrivingTarget.DistanceSqrTo(this.Position2f) < this.DrivingData.SteeringAxleOffset.Squared)
        {
          this.m_approachingRoad = false;
          return DrivingState.DrivingForwardsOnRoad;
        }
        flag1 = true;
        relTile1f1 *= Fix32.Half;
      }
      RelTile2f relTile2f = this.DrivingTarget - this.Position2f;
      if (relTile2f.LengthSqr < DrivingEntity.DRIVING_STOP_TRESHOLD_SQR)
      {
        this.m_speedDriver.BrakeBy(Percent.Hundred);
        this.m_steeringDriver.SetSpeed((Fix32) 0);
        return DrivingState.Stopping;
      }
      AngleDegrees1f angleDegrees1f = this.Direction.DirectionVector.AngleTo(relTile2f.Vector2f);
      if (this.DrivingData.CanTurnInPlace)
      {
        if (this.m_driveBackwards)
          return DrivingState.StopAndContinueBackwards;
        if (this.isTargetUnreachable())
        {
          this.m_speedDriver.BrakeBy(Percent.Hundred);
          this.steerTowardsGoal(false);
          return DrivingState.StopAndContinueForwards;
        }
      }
      else if (angleDegrees1f.Abs > DrivingEntity.s_goalBehindAngle && this.canDriveBackwards() || this.isTargetUnreachable())
      {
        this.m_speedDriver.BrakeBy(Percent.Hundred);
        return DrivingState.DrivingBackwards;
      }
      bool flag2 = this.steerTowardsGoal(false);
      if (this.Speed.IsZero && (this.DrivingData.CanTurnInPlace ? (angleDegrees1f.Abs > DrivingEntity.MAX_GOAL_ANGLE_BEFORE_STARTING ? 1 : 0) : (flag2 ? 1 : 0)) != 0)
        return DrivingState.DrivingForwards;
      if (this.Speed > RelTile1f.Zero && !this.canDriveForwards())
        relTile1f1 = this.DrivingData.MaxForwardsSpeed / 4;
      Percent t = Percent.FromRatio(this.SteeringAngle.Abs.Degrees, this.DrivingData.MaxSteeringAngle.Degrees);
      RelTile1f other = this.DrivingData.MaxForwardsSpeed.ScaledBy(this.DrivingData.SteeringSpeedMult);
      RelTile1f relTile1f2 = relTile1f1.Min(this.DrivingData.MaxForwardsSpeed.Lerp(other, t));
      Fix32 maxBrakeDistance = relTile2f.Length;
      if (!flag1)
        maxBrakeDistance = maxBrakeDistance.Times2Fast;
      this.m_speedDriver.KeepSpeed(relTile1f2.Value, maxBrakeDistance);
      return DrivingState.DrivingForwards;
    }

    private DrivingState handleDrivingForwardsOnRoad()
    {
      if (this.m_currentRoadSegment.Entity == null || this.m_currentRoadTraj.LaneCenterSamples.IsNotValidOrEmpty)
      {
        Log.Warning("Invalid road driving state.");
        this.StopDriving();
        return this.m_state;
      }
      this.m_speedDriver.KeepSpeed(this.DrivingData.MaxForwardsSpeed.Value, this.m_remainingRoadDistance + Fix32.Two);
      Tile2f position2f = this.Position2f;
      if (!this.Prototype.DrivingData.CanTurnInPlace)
        position2f += new RelTile2f(this.Direction.DirectionVector) * this.Prototype.DrivingData.SteeringAxleOffset.Value;
      Tile2f roadOrigin = this.m_currentRoadSegment.Entity.CenterTile.Xy.CornerTile2f;
      Tile2f other = this.m_currentRoadTraj.LaneCenterSamples[this.m_currentRoadTrajIndex].Xy + roadOrigin;
      Fix32 currDist = position2f.DistanceTo(other);
      Fix32 maxDist = this.m_speedDriver.Speed;
      RelTile2f direction;
      Tile2f position1;
      if (currDist >= maxDist)
      {
        direction = (other - position2f).Normalized;
        position1 = position2f + direction * maxDist;
      }
      else
        position1 = findNewTrackingPos(out direction);
      if (this.Prototype.DrivingData.CanTurnInPlace)
      {
        AngleDegrees1f angle = direction.Angle;
        this.m_steeringDriver.SetSpeed((angle - this.Direction).Degrees);
        this.Direction = angle;
        this.SetGroundPosition(position1);
      }
      else if (!position1.IsNear(position2f))
      {
        Vector2f directionVector = this.Direction.DirectionVector;
        RelTile2f relTile2f1 = position1 - position2f;
        Tile2f tile2f1 = position2f + relTile2f1.HalfFast;
        Line2f line2f;
        ref Line2f local = ref line2f;
        Vector2f vector2f1 = tile2f1.Vector2f;
        Tile2f tile2f2 = tile2f1;
        RelTile2f relTile2f2 = relTile2f1.Normalized;
        RelTile2f orthogonalVector = relTile2f2.LeftOrthogonalVector;
        Vector2f vector2f2 = (tile2f2 + orthogonalVector).Vector2f;
        local = new Line2f(vector2f1, vector2f2);
        Tile2f tile2f3 = this.Position2f - new RelTile2f(directionVector) * this.Prototype.DrivingData.NonSteeringAxleOffset.Value;
        Line2f otherLine = new Line2f(tile2f3.Vector2f, tile2f3.Vector2f + directionVector.LeftOrthogonalVector);
        AngleDegrees1f angleDegrees1f = line2f.Direction.AngleBetween(otherLine.Direction);
        if (angleDegrees1f < AngleDegrees1f.OneDegree || angleDegrees1f > AngleDegrees1f.Deg179)
        {
          this.SetGroundPosition(this.Position2f + relTile2f1);
          this.Direction = direction.Angle;
          this.m_steeringDriver.SetSpeed(Fix32.Zero);
        }
        else
        {
          Vector2f? nullable = line2f.Intersect(otherLine);
          if (nullable.HasValue)
          {
            Tile2f pivot = new Tile2f(nullable.Value);
            relTile2f2 = position2f - pivot;
            Tile2f position2 = this.Position2f.Rotate(relTile2f2.AngleTo(position1 - pivot), pivot);
            this.SetGroundPosition(position2);
            relTile2f2 = position1 - position2;
            this.Direction = relTile2f2.Angle;
            this.m_steeringDriver.SetSpeed((direction.Angle - this.Direction).Degrees);
          }
          else
          {
            Log.Warning("Failed to find center of rotation.");
            this.SetGroundPosition(this.Position2f + relTile2f1);
            this.Direction = direction.Angle;
            this.m_steeringDriver.SetSpeed(Fix32.Zero);
          }
        }
      }
      this.DrivingTarget = position1;
      return this.m_currentRoadSegment.Entity == null ? DrivingState.Stopping : DrivingState.DrivingForwardsOnRoad;

      Tile2f findNewTrackingPos(out RelTile2f direction)
      {
        while (true)
        {
          ImmutableArray<RelTile1f> lengthsPrefixSums = this.m_currentRoadTraj.SegmentLengthsPrefixSums;
          for (++this.m_currentRoadTrajIndex; this.m_currentRoadTrajIndex < lengthsPrefixSums.Length; ++this.m_currentRoadTrajIndex)
          {
            Fix32 scale = lengthsPrefixSums[this.m_currentRoadTrajIndex].Value - lengthsPrefixSums[this.m_currentRoadTrajIndex - 1].Value;
            currDist += scale;
            this.m_remainingRoadDistance -= scale;
            if (currDist >= maxDist)
            {
              Fix32 fix32 = currDist - maxDist;
              ImmutableArray<RelTile3f> laneCenterSamples = this.m_currentRoadTraj.LaneCenterSamples;
              ImmutableArray<RelTile3f> directionSamples = this.m_currentRoadTraj.LaneDirectionSamples;
              direction = directionSamples[this.m_currentRoadTrajIndex - 1].Xy.Lerp(directionSamples[this.m_currentRoadTrajIndex].Xy, scale - fix32, scale);
              return laneCenterSamples[this.m_currentRoadTrajIndex - 1].Xy.Lerp(laneCenterSamples[this.m_currentRoadTrajIndex].Xy, scale - fix32, scale) + roadOrigin;
            }
          }
          this.m_currentRoadTrajIndex = 0;
          ++this.m_currentRoadSegmentIndex;
          if (this.m_currentRoadSegmentIndex < this.m_roadSegments.Length)
          {
            this.m_currentRoadSegment = this.m_roadSegments[this.m_currentRoadSegmentIndex];
            this.m_currentRoadTraj = this.m_currentRoadSegment.Entity.GetTransformedRoadLane(this.m_currentRoadSegment.LaneIndex);
            roadOrigin = this.m_currentRoadSegment.Entity.CenterTile.Xy.CornerTile2f;
          }
          else
            break;
        }
        ImmutableArray<RelTile3f> laneCenterSamples1 = this.m_currentRoadTraj.LaneCenterSamples;
        ImmutableArray<RelTile3f> directionSamples1 = this.m_currentRoadTraj.LaneDirectionSamples;
        direction = directionSamples1.Last.Xy;
        Tile2f newTrackingPos = laneCenterSamples1.Last.Xy + roadOrigin;
        this.m_currentRoadSegment = new RoadPathSegment();
        this.m_currentRoadTraj = new RoadLaneTrajectory();
        return newTrackingPos;
      }
    }

    /// <summary>
    /// Drives the vehicle backwards. If vehicle is currently moving forwards, it stops first and then drive
    /// backwards.
    /// </summary>
    private DrivingState handleDrivingBackwards()
    {
      if (this.Speed > RelTile1f.Zero)
      {
        this.m_speedDriver.BrakeBy(Percent.Hundred);
        this.steerTowardsGoal(false);
        return DrivingState.DrivingBackwards;
      }
      RelTile1f relTile1f1 = this.DrivingData.MaxBackwardsSpeed;
      if (this.m_currentRoadSegment.Entity != null)
      {
        if (this.m_approachingRoad)
        {
          if (this.DrivingTarget.DistanceSqrTo(this.Position2f) < DrivingEntity.DRIVING_STOP_TRESHOLD_SQR)
          {
            this.m_approachingRoad = false;
            return DrivingState.DrivingForwardsOnRoad;
          }
          relTile1f1 *= Fix32.Half;
        }
        else
        {
          Log.Warning("Vehicle is not approaching road but is driving backwards.");
          return DrivingState.DrivingForwardsOnRoad;
        }
      }
      RelTile2f relTile2f = this.DrivingTarget - this.Position2f;
      if (relTile2f.LengthSqr < DrivingEntity.DRIVING_STOP_TRESHOLD_SQR)
      {
        this.m_speedDriver.BrakeBy(Percent.Hundred);
        this.m_steeringDriver.SetSpeed((Fix32) 0);
        return DrivingState.Stopping;
      }
      AngleDegrees1f angleDegrees1f1 = this.Direction.DirectionVector.AngleTo(relTile2f.Vector2f);
      bool flag;
      if (this.DrivingData.CanTurnInPlace)
      {
        if (this.isTargetUnreachable())
        {
          this.m_speedDriver.KeepSpeed((Fix32) 0);
          this.steerTowardsGoal(this.m_driveBackwards);
          return DrivingState.DrivingBackwards;
        }
        if (!this.m_driveBackwards)
          return DrivingState.StopAndContinueForwards;
        flag = this.steerTowardsGoal(this.m_driveBackwards);
      }
      else
      {
        if (angleDegrees1f1.Abs < DrivingEntity.s_goalInFrontAngle && !this.isTargetUnreachable() && this.canDriveForwards())
          return DrivingState.DrivingForwards;
        flag = this.steerTowardsGoal(true);
      }
      AngleDegrees1f angleDegrees1f2;
      if (this.Speed.IsZero)
      {
        int num;
        if (!this.DrivingData.CanTurnInPlace)
        {
          num = flag ? 1 : 0;
        }
        else
        {
          angleDegrees1f2 = angleDegrees1f1 + AngleDegrees1f.Deg180;
          num = !angleDegrees1f2.IsNear(AngleDegrees1f.Zero, DrivingEntity.MAX_GOAL_ANGLE_BEFORE_STARTING) ? 1 : 0;
        }
        if (num != 0)
          return DrivingState.DrivingBackwards;
      }
      if (this.Speed < RelTile1f.Zero && !this.canDriveBackwards())
      {
        if (!this.m_driveBackwards)
          return DrivingState.DrivingForwards;
        relTile1f1 = (this.DrivingData.MaxBackwardsSpeed / 4).Max(new RelTile1f(0.05.ToFix32()));
      }
      angleDegrees1f2 = this.SteeringAngle;
      Percent t = Percent.FromRatio(angleDegrees1f2.Abs.Degrees, this.DrivingData.MaxSteeringAngle.Degrees);
      RelTile1f other = this.DrivingData.MaxBackwardsSpeed.ScaledBy(this.DrivingData.SteeringSpeedMult);
      RelTile1f relTile1f2 = relTile1f1.Min(this.DrivingData.MaxBackwardsSpeed.Lerp(other, t));
      Fix32 length = relTile2f.Length;
      if (!this.TargetIsTerminal)
        length *= 2;
      this.m_speedDriver.KeepSpeed(-relTile1f2.Value, -length);
      return DrivingState.DrivingBackwards;
    }

    private DrivingState handleTurningInPlace()
    {
      Assert.That<bool>(this.DrivingData.CanTurnInPlace).IsTrue();
      Assert.That<Fix32>(this.m_speedDriver.Speed).IsZero();
      Assert.That<bool>(this.m_driveBackwards).IsFalse();
      RelTile2f relTile2f = this.DrivingTarget - this.Position2f;
      if (relTile2f.LengthSqr < DrivingEntity.DRIVING_STOP_TRESHOLD_SQR || relTile2f.Angle.IsNear(this.Direction, this.DrivingData.MaxSteeringSpeed))
      {
        this.IsDriving = false;
        this.m_steeringDriver.SetSpeed((Fix32) 0);
        return DrivingState.Stopping;
      }
      this.steerTowards(this.DrivingTarget, false);
      return DrivingState.TurningInPlace;
    }

    private bool steerTowardsGoal(bool reverse) => this.steerTowards(this.DrivingTarget, reverse);

    /// <summary>
    /// Steers the vehicle by setting <see cref="P:Mafi.Core.Entities.Dynamic.DrivingEntity.SteeringAngle" /> towards the goal. If <paramref name="reverse" /> is
    /// true, it steers towards goal while vehicle is moving backwards. Returns true when max steering was performed.
    /// </summary>
    private bool steerTowards(Tile2f target, bool reverse)
    {
      RelTile2f target1 = target - this.Position2f;
      if (this.m_driveBackwards)
      {
        target1 = -target1;
        reverse = !reverse;
      }
      if (target1.IsZero)
      {
        this.m_steeringDriver.KeepSpeed((Fix32) 0);
        return false;
      }
      target1 = target1.Rotate(-this.Direction);
      AngleDegrees1f angleDegrees1f = target1.Angle;
      if (this.DrivingData.CanTurnInPlace)
      {
        if (reverse)
          angleDegrees1f = -angleDegrees1f;
        Assert.That<AngleDegrees1f>(angleDegrees1f).IsWithinIncl(-AngleDegrees1f.Deg180, AngleDegrees1f.Deg180);
        this.m_steeringDriver.KeepSpeed(angleDegrees1f.Degrees, angleDegrees1f.Degrees);
      }
      else
      {
        if (angleDegrees1f.Abs < this.DrivingData.MaxSteeringAngle)
          angleDegrees1f = this.getRequiredSteeringAngleFor(target1);
        if (reverse)
          angleDegrees1f = -angleDegrees1f;
        this.m_steeringDriver.KeepSpeed(angleDegrees1f.Degrees);
      }
      return this.m_steeringDriver.AccelerationPercent.Abs() > 30.Percent();
    }

    /// <summary>
    /// Whether area in front of the vehicle is pathable. This takes into account current speed.
    /// </summary>
    private bool canDriveForwards()
    {
      return this.PathabilityProvider.IsTilePathable((this.Position2f + new RelTile2f(this.Direction.DirectionVector * this.DistanceToFullStop.Value)).Tile2i, this.SingleTilePathabilityMask);
    }

    /// <summary>
    /// Whether area behind the vehicle is pathable. This takes into account current speed.
    /// </summary>
    private bool canDriveBackwards()
    {
      return this.PathabilityProvider.IsTilePathable((this.Position2f - new RelTile2f(this.Direction.DirectionVector * this.DistanceToFullStop.Value)).Tile2i, this.SingleTilePathabilityMask);
    }

    private bool reachedGoal(RelTile1f distanceToTarget)
    {
      if (distanceToTarget > this.m_targetTolerance)
        return false;
      if (!this.TargetIsTerminal)
        return true;
      if (!(this.Speed.Abs < this.DrivingData.Braking))
        return false;
      this.m_speedDriver.SetSpeed((Fix32) 0);
      this.m_steeringDriver.SetSpeed((Fix32) 0);
      return true;
    }

    /// <summary>Performs logic responsible for moving the entity.</summary>
    private void performMoveStep()
    {
      AngleDegrees1f angleDegrees1f;
      if (!this.DrivingData.CanTurnInPlace)
      {
        angleDegrees1f = this.SteeringAngle;
        if (!(angleDegrees1f.Abs < 2.Degrees()))
        {
          if (this.Speed.IsZero)
            return;
          RelTile2f relativeTurnCenter = this.getRelativeTurnCenter(this.SteeringAngle);
          Fix32 length = relativeTurnCenter.Length;
          angleDegrees1f = this.SteeringAngle;
          AngleDegrees1f angle = angleDegrees1f.Sign * MafiMath.ArcAngle(this.Speed.Value, length);
          Assert.That<AngleDegrees1f>(angle.Abs).IsLess(30.Degrees(), "Vehicle is turning more than 30 degrees per tick. Consider lowering MaxSteeringAngle, decreasing MaxSpeed or SteeringSpeedMult, or improve driving logic so that we brake before turning.");
          RelTile2f relTile2f = RelTile2f.Zero.Rotate(angle, relativeTurnCenter);
          if (relTile2f.IsZero)
          {
            ref RelTile2f local = ref relTile2f;
            angleDegrees1f = this.Direction;
            Vector2f vector = angleDegrees1f.DirectionVector * this.Speed.Value;
            local = new RelTile2f(vector);
          }
          this.SetGroundPosition(this.Position2f + relTile2f);
          angleDegrees1f = this.Direction + angle;
          this.Direction = angleDegrees1f.Normalized;
          return;
        }
      }
      angleDegrees1f = this.Direction + this.SteeringAngle;
      this.Direction = angleDegrees1f.Normalized;
      Tile2f position2f = this.Position2f;
      angleDegrees1f = this.Direction;
      RelTile2f relTile2f1 = new RelTile2f(angleDegrees1f.DirectionVector * this.Speed.Value);
      this.SetGroundPosition(position2f + relTile2f1);
    }

    /// <summary>
    /// Tests whether current target is unreachable due to turning radius singularity. This assumes maximum steering.
    /// </summary>
    /// <remarks>
    /// This tests whether the target is inside of circles of maximal steering radius. This is assuming that we are
    /// going forwards. Reachability while backing up is complicated.
    /// </remarks>
    private bool isTargetUnreachable()
    {
      AngleDegrees1f angleDegrees1f = 9 * this.DrivingData.MaxSteeringAngle / 10;
      if (this.DrivingData.CanTurnInPlace)
      {
        if (this.Speed == RelTile1f.Zero)
          return false;
        Fix32 fix32 = (this.Speed.Value / (2 * angleDegrees1f.Sin())).ToFix32();
        Fix64 fix64 = fix32.Squared();
        return (this.DrivingTarget - (this.Position2f + new RelTile2f((this.Direction + angleDegrees1f).DirectionVector.LeftOrthogonalVector * fix32))).LengthSqr <= fix64 || (this.DrivingTarget - (this.Position2f + new RelTile2f((this.Direction - angleDegrees1f).DirectionVector.RightOrthogonalVector * fix32))).LengthSqr <= fix64;
      }
      Fix32 y = this.DrivingData.AxlesDistance.Value / angleDegrees1f.Tan().ToFix32();
      RelTile2f relTile2f1 = new RelTile2f(-this.DrivingData.NonSteeringAxleOffset.Value, y);
      Fix64 lengthSqr = relTile2f1.LengthSqr;
      if ((this.DrivingTarget - (this.Position2f + relTile2f1.Rotate(this.Direction))).LengthSqr <= lengthSqr)
        return true;
      Tile2f position2f = this.Position2f;
      RelTile2f relTile2f2 = relTile2f1.SetY(-y);
      RelTile2f relTile2f3 = relTile2f2.Rotate(this.Direction);
      relTile2f2 = this.DrivingTarget - (position2f + relTile2f3);
      return relTile2f2.LengthSqr <= lengthSqr;
    }

    /// <summary>
    /// Returns center of rotation based on given steering angle. The center is relative to current position
    /// and is next to the rear axle. Given angle should be non-zero.
    /// </summary>
    private RelTile2f getRelativeTurnCenter(AngleDegrees1f steeringAngle)
    {
      Assert.That<AngleDegrees1f>(steeringAngle).IsNotZero();
      return new RelTile2f(-this.DrivingData.NonSteeringAxleOffset.Value, (Fix32) (this.DrivingData.AxlesDistance.Value / steeringAngle.Tan()).ToIntRounded()).Rotate(this.Direction);
    }

    protected void SetSpeedFactor(Percent speedFactor)
    {
      if (speedFactor == this.SpeedFactor)
        return;
      if (this.SpeedFactor.IsNotPositive)
        this.SpeedFactor = Percent.FromPercentVal(1);
      this.SpeedFactor = speedFactor;
      this.m_speedDriver.SetSpeedFactor(this.SpeedFactor);
      this.OnUpdateSpeedFactor();
    }

    protected virtual void OnUpdateSpeedFactor()
    {
    }

    private AngleDegrees1f getRequiredSteeringAngleFor(RelTile2f target)
    {
      if (target.Y.Abs() < 0.05.Tiles().Value)
        return target.Angle.ScaledBy(120.Percent());
      RelTile2f relTile2f1 = target.AddX(this.DrivingData.NonSteeringAxleOffset.Value);
      if (relTile2f1.X.Abs() < 0.05.Tiles().Value)
        return !(target.Y > 0) ? 80.Degrees() : -80.Degrees();
      RelTile2f relTile2f2 = relTile2f1 / (Fix32) 2;
      RelTile2f orthogonalVector = relTile2f1.RightOrthogonalVector;
      Vector2f? nullable = new Line2f(new Vector2f((Fix32) 0, (Fix32) 0), new Vector2f((Fix32) 0, (Fix32) 1)).Intersect(new Line2f(relTile2f2.Vector2f, (relTile2f2 + orthogonalVector).Vector2f));
      if (!nullable.HasValue)
      {
        Log.Error("Axle line does not intersect line to the center of rotation. Are we going straight?");
        return !(target.Y > 0) ? 80.Degrees() : -80.Degrees();
      }
      Vector2f vector2f = nullable.Value;
      Assert.That<Fix64>(vector2f.DistanceSqrTo(relTile2f1.Vector2f)).IsNear(vector2f.LengthSqr, Fix64.One, "Computed center of rotation has not the same distance to relative goal and to rear axle.");
      Assert.That<Fix32>(vector2f.X).IsNear(Fix32.Zero, 5.Over(100), "Center of rotation must lie on the rear axle line.");
      Fix32 fix32_1 = (this.DrivingData.AxlesDistance.Value / vector2f.Y).Atan();
      if (fix32_1.IsZero)
        return AngleDegrees1f.Zero;
      Assert.That<int>(fix32_1.Sign()).IsEqualTo(vector2f.Y.Sign(), "Incorrect sign of steering angle.");
      Fix32 fix32_2 = this.DrivingData.NonSteeringAxleOffset.Value / relTile2f1.X;
      return AngleDegrees1f.FromRadians(fix32_1 * ((Fix32) 1 + fix32_2.Min((Fix32) 1)));
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Tile2f.Serialize(this.DrivingTarget, writer);
      writer.WriteBool(this.IsDriving);
      Tile2i.Serialize(this.LastValidPosition, writer);
      writer.WriteBool(this.m_approachingRoad);
      writer.WriteInt(this.m_currentRoadSegmentIndex);
      writer.WriteInt(this.m_currentRoadTrajIndex);
      writer.WriteBool(this.m_driveBackwards);
      writer.WriteInt((int) this.m_previousState);
      ImmutableArray<RoadPathSegment>.Serialize(this.m_roadSegments, writer);
      SmoothDriver.Serialize(this.m_speedDriver, writer);
      writer.WriteInt((int) this.m_state);
      SmoothDriver.Serialize(this.m_steeringDriver, writer);
      RelTile1f.Serialize(this.m_targetTolerance, writer);
      writer.WriteULong(this.PathabilityMask);
      writer.WriteGeneric<IPathabilityProvider>(this.PathabilityProvider);
      Tile2i.Serialize(this.PreLastValidPosition, writer);
      writer.WriteGeneric<DrivingEntityProto>(this.Prototype);
      writer.WriteULong(this.SingleTilePathabilityMask);
      Percent.Serialize(this.SpeedFactor, writer);
      writer.WriteBool(this.TargetIsTerminal);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.DrivingTarget = Tile2f.Deserialize(reader);
      this.IsDriving = reader.ReadBool();
      this.LastValidPosition = Tile2i.Deserialize(reader);
      this.m_approachingRoad = reader.LoadedSaveVersion >= 140 && reader.ReadBool();
      this.m_currentRoadSegmentIndex = reader.LoadedSaveVersion >= 140 ? reader.ReadInt() : 0;
      this.m_currentRoadTrajIndex = reader.LoadedSaveVersion >= 140 ? reader.ReadInt() : 0;
      this.m_driveBackwards = reader.ReadBool();
      this.m_previousState = (DrivingState) reader.ReadInt();
      this.m_roadSegments = reader.LoadedSaveVersion >= 140 ? ImmutableArray<RoadPathSegment>.Deserialize(reader) : ImmutableArray<RoadPathSegment>.Empty;
      reader.SetField<DrivingEntity>(this, "m_speedDriver", (object) SmoothDriver.Deserialize(reader));
      this.m_state = (DrivingState) reader.ReadInt();
      reader.SetField<DrivingEntity>(this, "m_steeringDriver", (object) SmoothDriver.Deserialize(reader));
      this.m_targetTolerance = RelTile1f.Deserialize(reader);
      reader.SetField<DrivingEntity>(this, "PathabilityMask", (object) reader.ReadULong());
      reader.SetField<DrivingEntity>(this, "PathabilityProvider", (object) reader.ReadGenericAs<IPathabilityProvider>());
      this.PreLastValidPosition = Tile2i.Deserialize(reader);
      reader.SetField<DrivingEntity>(this, "Prototype", (object) reader.ReadGenericAs<DrivingEntityProto>());
      reader.SetField<DrivingEntity>(this, "SingleTilePathabilityMask", (object) reader.ReadULong());
      this.SpeedFactor = Percent.Deserialize(reader);
      this.TargetIsTerminal = reader.ReadBool();
      reader.RegisterInitAfterLoad<DrivingEntity>(this, "initAfterLoad", InitPriority.Normal);
    }

    static DrivingEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DrivingEntity.DEFAULT_DRIVING_TOLERANCE = 0.25.Tiles();
      DrivingEntity.MAX_GOAL_ANGLE_BEFORE_STARTING = 10.Degrees();
      DrivingEntity.DRIVING_STOP_TRESHOLD_SQR = 0.01.ToFix64();
      DrivingEntity.s_goalBehindAngle = 100.Degrees();
      DrivingEntity.s_goalInFrontAngle = 70.Degrees();
    }
  }
}
