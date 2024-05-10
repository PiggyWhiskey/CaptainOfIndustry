// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.MiningJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job in which an excavator mines material from the terrain using a terrain designation.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("m_preferredProductToMine", 103, typeof (Option<LooseProductProto>), 0, false)]
  public sealed class MiningJob : VehicleJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    /// <summary>
    /// How many ticks to wait after designation was mined successfully. We have this delay to wait for potential
    /// terrain falling.
    /// </summary>
    private static readonly Duration DONE_WAITING_DURATION;
    private static readonly ThicknessTilesF MAX_TOP_LAYER_FOR_MINING_BELOW;
    /// <summary>
    /// Duration of truck queue requests in this job. Should be long enough to cover excavator short movements
    /// during mining.
    /// </summary>
    private static readonly Duration TRUCK_QUEUE_ENABLED_DURATION;
    private static readonly Duration TRUCK_QUEUE_ENABLED_MOVING_DURATION;
    private readonly TickTimer m_timer;
    private readonly Excavator m_excavator;
    [DoNotSave(110, null)]
    private TerrainDesignation m_primaryDesignationOld;
    [NewInSaveVersion(110, "m_primaryDesignationZzz", "m_primaryDesignationOld", null, null)]
    private Option<TerrainDesignation> m_primaryDesignation;
    private Option<TerrainDesignation> m_designationToMine;
    private readonly Lyst<TerrainDesignation> m_extraDesignations;
    private readonly Lyst<TerrainDesignation> m_fulfilledDesignations;
    private TerrainTile? m_tileToMine;
    private Option<LooseProductProto> m_lastMinedProduct;
    private int m_iterationsToMine;
    private Option<TerrainDesignationVehicleGoal> m_initialNavGoal;
    private MiningJob.State m_state;
    private MiningJob.State m_previousState;
    private readonly MiningJob.Factory m_factory;
    private readonly RobustNavHelper m_navHelper;
    private Tile2f m_driveStartPosition;
    private Option<LooseProductProto> m_lastSeenPriorityProduct;
    [DoNotSave(0, null)]
    private Func<IDesignation, TerrainTile, RelTile2i, Fix32> m_targetTileCostFn;
    [DoNotSave(0, null)]
    private Predicate<IDesignation> m_filterDestroyedAndUnreachablePredicate;
    [DoNotSave(0, null)]
    private IReadOnlySet<IDesignation> m_unreachableDesignationsCache;
    [NewInSaveVersion(140, null, null, typeof (TreesManager), null)]
    private readonly TreesManager m_treesManager;
    private readonly TerrainPropsManager m_terrainPropsManager;

    public static void Serialize(MiningJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MiningJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MiningJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<TerrainDesignation>.Serialize(this.m_designationToMine, writer);
      Tile2f.Serialize(this.m_driveStartPosition, writer);
      Excavator.Serialize(this.m_excavator, writer);
      Lyst<TerrainDesignation>.Serialize(this.m_extraDesignations, writer);
      Lyst<TerrainDesignation>.Serialize(this.m_fulfilledDesignations, writer);
      Option<TerrainDesignationVehicleGoal>.Serialize(this.m_initialNavGoal, writer);
      writer.WriteInt(this.m_iterationsToMine);
      Option<LooseProductProto>.Serialize(this.m_lastMinedProduct, writer);
      Option<LooseProductProto>.Serialize(this.m_lastSeenPriorityProduct, writer);
      RobustNavHelper.Serialize(this.m_navHelper, writer);
      writer.WriteInt((int) this.m_previousState);
      Option<TerrainDesignation>.Serialize(this.m_primaryDesignation, writer);
      writer.WriteInt((int) this.m_state);
      TerrainPropsManager.Serialize(this.m_terrainPropsManager, writer);
      writer.WriteNullableStruct<TerrainTile>(this.m_tileToMine);
      TickTimer.Serialize(this.m_timer, writer);
      TreesManager.Serialize(this.m_treesManager, writer);
    }

    public static MiningJob Deserialize(BlobReader reader)
    {
      MiningJob miningJob;
      if (reader.TryStartClassDeserialization<MiningJob>(out miningJob))
        reader.EnqueueDataDeserialization((object) miningJob, MiningJob.s_deserializeDataDelayedAction);
      return miningJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_designationToMine = Option<TerrainDesignation>.Deserialize(reader);
      this.m_driveStartPosition = Tile2f.Deserialize(reader);
      reader.SetField<MiningJob>(this, "m_excavator", (object) Excavator.Deserialize(reader));
      reader.SetField<MiningJob>(this, "m_extraDesignations", (object) Lyst<TerrainDesignation>.Deserialize(reader));
      reader.RegisterResolvedMember<MiningJob>(this, "m_factory", typeof (MiningJob.Factory), true);
      reader.SetField<MiningJob>(this, "m_fulfilledDesignations", (object) Lyst<TerrainDesignation>.Deserialize(reader));
      this.m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.Deserialize(reader);
      this.m_iterationsToMine = reader.ReadInt();
      this.m_lastMinedProduct = Option<LooseProductProto>.Deserialize(reader);
      this.m_lastSeenPriorityProduct = Option<LooseProductProto>.Deserialize(reader);
      reader.SetField<MiningJob>(this, "m_navHelper", (object) RobustNavHelper.Deserialize(reader));
      if (reader.LoadedSaveVersion < 103)
        Option<LooseProductProto>.Deserialize(reader);
      this.m_previousState = (MiningJob.State) reader.ReadInt();
      if (reader.LoadedSaveVersion < 110)
        this.m_primaryDesignationOld = TerrainDesignation.Deserialize(reader);
      this.m_primaryDesignation = reader.LoadedSaveVersion >= 110 ? Option<TerrainDesignation>.Deserialize(reader) : (Option<TerrainDesignation>) this.m_primaryDesignationOld;
      this.m_state = (MiningJob.State) reader.ReadInt();
      reader.SetField<MiningJob>(this, "m_terrainPropsManager", (object) TerrainPropsManager.Deserialize(reader));
      this.m_tileToMine = reader.ReadNullableStruct<TerrainTile>();
      reader.SetField<MiningJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<MiningJob>(this, "m_treesManager", reader.LoadedSaveVersion >= 140 ? (object) TreesManager.Deserialize(reader) : (object) (TreesManager) null);
      if (reader.LoadedSaveVersion < 140)
        reader.RegisterResolvedMember<MiningJob>(this, "m_treesManager", typeof (TreesManager), true);
      reader.RegisterInitAfterLoad<MiningJob>(this, "initAfterLoad", InitPriority.Normal);
    }

    public override LocStrFormatted JobInfo
    {
      get
      {
        switch (this.m_state)
        {
          case MiningJob.State.InitialPathFindingAndDesignationSelect:
            Excavator excavator = this.m_excavator;
            return (LocStrFormatted) ((excavator != null ? (excavator.IsDriving ? 1 : 0) : 0) != 0 ? TrCore.VehicleJob__DrivingToGoal : TrCore.VehicleJob__SearchingForDesignation);
          case MiningJob.State.DrivingToNewMineLocation:
            return (LocStrFormatted) TrCore.VehicleJob__DrivingToGoal;
          case MiningJob.State.DecideMiningOnReachableTile:
          case MiningJob.State.PreparingToMine:
          case MiningJob.State.Mining:
          case MiningJob.State.WaitingForShovel:
          case MiningJob.State.WaitingForFulfilled:
          case MiningJob.State.CheckAllFulfilled:
            return (LocStrFormatted) TrCore.VehicleJob__Loading;
          default:
            return (LocStrFormatted) TrCore.VehicleJob__InvalidState;
        }
      }
    }

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption
    {
      get
      {
        return this.m_excavator.State != ExcavatorState.WaitingForTruck && (this.m_state != MiningJob.State.InitialPathFindingAndDesignationSelect || this.m_excavator.IsDriving) ? VehicleFuelConsumption.Full : VehicleFuelConsumption.Idle;
      }
    }

    public TerrainTile? TileToMine => this.m_tileToMine;

    /// <summary>Whether the state changed last update.</summary>
    private bool StateChanged => this.m_state != this.m_previousState;

    private MiningJob(
      VehicleJobId id,
      TerrainPropsManager terrainPropsManager,
      TreesManager treesManager,
      MiningJob.Factory factory,
      Excavator excavator,
      TerrainDesignation primaryDesignation,
      IEnumerable<TerrainDesignation> extraDesignations)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      this.m_fulfilledDesignations = new Lyst<TerrainDesignation>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory.CheckNotNull<MiningJob.Factory>();
      this.m_terrainPropsManager = terrainPropsManager.CheckNotNull<TerrainPropsManager>();
      this.m_navHelper = new RobustNavHelper(factory.PathFindingManager);
      this.initSelf();
      Assert.That<Option<IEntityAssignedWithVehicles>>(excavator.AssignedTo).HasValue<IEntityAssignedWithVehicles>();
      Assert.That<bool>(primaryDesignation.IsMiningFulfilled).IsFalse();
      this.m_excavator = excavator.CheckNotNull<Excavator>();
      this.m_treesManager = treesManager;
      this.m_primaryDesignation = Option<TerrainDesignation>.Create(primaryDesignation);
      this.m_extraDesignations = extraDesignations.ToLyst<TerrainDesignation>();
      this.m_lastSeenPriorityProduct = this.m_excavator.PrioritizedProduct;
      this.m_excavator.EnqueueJob((VehicleJob) this, false);
      this.m_previousState = MiningJob.State.Done;
      this.m_state = MiningJob.State.InitialPathFindingAndDesignationSelect;
    }

    private void initSelf()
    {
      this.m_targetTileCostFn = new Func<IDesignation, TerrainTile, RelTile2i, Fix32>(this.targetTileCostFunction);
      this.m_filterDestroyedAndUnreachablePredicate = (Predicate<IDesignation>) (d => d.IsDestroyed || this.m_unreachableDesignationsCache.Contains(d));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad(int saveVersion) => this.initSelf();

    protected override bool DoJobInternal()
    {
      Assert.That<Option<IEntityAssignedWithVehicles>>(this.m_excavator.AssignedTo).HasValue<IEntityAssignedWithVehicles>();
      if (this.m_state == MiningJob.State.DecideMiningOnReachableTile && this.m_excavator.LastRefuelRequestIssue != RefuelRequestIssue.None && this.m_excavator.NeedsRefueling && !this.m_excavator.CanRunWithNoFuel)
      {
        if (this.m_factory.FuelStationsManager.TryRequestTruckForRefueling((Vehicle) this.m_excavator))
          this.m_excavator.LastRefuelRequestIssue = RefuelRequestIssue.None;
        else if (this.m_factory.FuelStationsManager.TryRefuelSelf((Vehicle) this.m_excavator))
        {
          this.m_excavator.TruckQueue.Disable();
          this.cleanup();
          return false;
        }
      }
      MiningJob.State state = this.handleState();
      this.m_previousState = this.m_state;
      this.m_state = state;
      if (this.m_state != MiningJob.State.Done)
        return true;
      this.cleanup();
      return false;
    }

    private void clearDesignationToMine()
    {
      if (!this.m_designationToMine.HasValue)
        return;
      this.m_designationToMine.Value.RemoveAssignment((IVehicleJob) this);
      this.m_designationToMine = Option<TerrainDesignation>.None;
    }

    private void cleanup()
    {
      if (this.m_excavator != null)
      {
        this.m_excavator.ResetCabinTarget();
        this.m_excavator.SetShovelState(ExcavatorShovelState.Tucked);
        this.m_excavator.ClearCargoImmediately();
      }
      this.m_navHelper.CancelAndClear();
      this.clearDesignationToMine();
      this.m_extraDesignations.Clear();
      this.m_fulfilledDesignations.Clear();
    }

    private MiningJob.State handleState()
    {
      switch (this.m_state)
      {
        case MiningJob.State.InitialPathFindingAndDesignationSelect:
          return this.handleInitialPathFindingAndDesignationSelect();
        case MiningJob.State.DrivingToNewMineLocation:
          return this.handleDrivingToNewMineLocation();
        case MiningJob.State.DecideMiningOnReachableTile:
          return this.handleDecideMiningOnReachableTile();
        case MiningJob.State.PreparingToMine:
          return this.handlePreparingToMine();
        case MiningJob.State.Mining:
          return this.handleMining();
        case MiningJob.State.WaitingForShovel:
          return this.handleWaitingForShovel();
        case MiningJob.State.WaitingForFulfilled:
          return this.handleWaitingForFulfilled();
        case MiningJob.State.CheckAllFulfilled:
          return this.handleCheckAllFulfilled();
        default:
          Assert.Fail(string.Format("Unknown/invalid mining job state '{0}'.", (object) this.m_state));
          return MiningJob.State.Done;
      }
    }

    private Option<TerrainDesignation> trySelectPrimaryDesignation(
      Option<TerrainDesignation> candidate)
    {
      IReadOnlySet<IDesignation> readOnlySet = this.filterDestroyedAndUnreachable(this.m_extraDesignations);
      this.m_extraDesignations.RemoveWhere((Predicate<TerrainDesignation>) (x => x.IsMiningFulfilled), this.m_fulfilledDesignations);
      TerrainDesignation valueOrNull = candidate.ValueOrNull;
      if (valueOrNull != null)
      {
        if (!valueOrNull.IsDestroyed && valueOrNull.IsMiningNotFulfilled && valueOrNull.IsMiningReadyToBeFulfilled && valueOrNull.CanBeAssigned(false) && !readOnlySet.Contains((IDesignation) valueOrNull))
          return Option<TerrainDesignation>.Create(valueOrNull);
        if (!valueOrNull.IsDestroyed && !readOnlySet.Contains((IDesignation) valueOrNull))
        {
          if (valueOrNull.IsMiningFulfilled)
            this.m_fulfilledDesignations.Add(valueOrNull);
          else
            this.m_extraDesignations.Add(valueOrNull);
        }
      }
      for (int index = 0; index < this.m_extraDesignations.Count; ++index)
      {
        TerrainDesignation extraDesignation = this.m_extraDesignations[index];
        if (extraDesignation.IsMiningReadyToBeFulfilled && extraDesignation.CanBeAssigned(false))
        {
          this.m_extraDesignations.RemoveAt(index);
          return Option<TerrainDesignation>.Create(extraDesignation);
        }
      }
      return Option<TerrainDesignation>.None;
    }

    private MiningJob.State handleInitialPathFindingAndDesignationSelect()
    {
      if (this.StateChanged || this.m_primaryDesignation.IsNone)
      {
        Assert.That<Option<TerrainDesignation>>(this.m_designationToMine).IsNone<TerrainDesignation>();
        this.m_primaryDesignation = this.trySelectPrimaryDesignation(this.m_primaryDesignation);
        if (this.m_primaryDesignation.IsNone)
          return MiningJob.State.CheckAllFulfilled;
        RelTile1i tolerance = this.m_terrainPropsManager.ContainsPropInDesignation((IDesignation) this.m_primaryDesignation.Value) || this.m_treesManager.ContainsStumpInDesignation((IDesignation) this.m_primaryDesignation.Value) ? (this.m_excavator.Prototype.MaxMiningDistance + this.m_excavator.Prototype.MinMiningDistance) / 2 : this.m_excavator.Prototype.MinMiningDistance;
        RelTile1i relTile1i = this.m_excavator.Prototype.MaxMiningDistance - tolerance;
        TerrainDesignationVehicleGoal goal = this.m_factory.DesignationGoalFactory.Create((IDesignation) this.m_primaryDesignation.Value, tolerance, (IEnumerable<IDesignation>) this.m_extraDesignations.Where<TerrainDesignation>((Func<TerrainDesignation, bool>) (x => x.IsMiningReadyToBeFulfilled)));
        this.m_navHelper.StartNavigationTo((Vehicle) this.m_excavator, (IVehicleGoalFull) goal, extraTolerancePerRetry: new RelTile1i?(relTile1i));
        this.m_initialNavGoal = (Option<TerrainDesignationVehicleGoal>) goal;
        return MiningJob.State.InitialPathFindingAndDesignationSelect;
      }
      TerrainDesignation d = this.m_primaryDesignation.Value;
      if (this.m_designationToMine.IsNone && this.m_initialNavGoal.Value.ActualGoalDesignation.ValueOrNull is TerrainDesignation valueOrNull)
      {
        if (tryAssignTo(valueOrNull))
          this.m_designationToMine = (Option<TerrainDesignation>) valueOrNull;
        else if (valueOrNull != d && tryAssignTo(d))
        {
          this.m_designationToMine = (Option<TerrainDesignation>) d;
        }
        else
        {
          this.m_extraDesignations.RemoveWhere((Predicate<TerrainDesignation>) (x => x.IsDestroyed));
          foreach (TerrainDesignation extraDesignation in this.m_extraDesignations)
          {
            if (tryAssignTo(extraDesignation))
            {
              this.m_designationToMine = Option<TerrainDesignation>.Create(extraDesignation);
              break;
            }
          }
          if (this.m_designationToMine.IsNone)
            return MiningJob.State.Done;
        }
        Assert.That<Option<TerrainDesignation>>(this.m_designationToMine).HasValue<TerrainDesignation>();
        Assert.That<bool>(this.m_designationToMine.Value.IsMiningNotFulfilled).IsTrue();
        Assert.That<bool>(this.m_designationToMine.Value.IsMiningReadyToBeFulfilled).IsTrue();
      }
      Assert.That<Option<TerrainDesignationVehicleGoal>>(this.m_initialNavGoal).HasValue<TerrainDesignationVehicleGoal>();
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          return MiningJob.State.InitialPathFindingAndDesignationSelect;
        case RobustNavResult.GoalReachedSuccessfully:
          if (this.m_designationToMine.IsNone)
          {
            Log.Error("Mining designation was not set.");
            this.m_designationToMine = Option<TerrainDesignation>.Create(d);
            if (!this.m_designationToMine.Value.TryAssignTo((IVehicleJob) this))
              return MiningJob.State.Done;
          }
          this.m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.None;
          return MiningJob.State.DecideMiningOnReachableTile;
        case RobustNavResult.FailGoalUnreachable:
          return MiningJob.State.CheckAllFulfilled;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }

      bool tryAssignTo(TerrainDesignation d)
      {
        return !d.IsDestroyed && d.IsMiningNotFulfilled && d.IsMiningReadyToBeFulfilled && d.TryAssignTo((IVehicleJob) this);
      }
    }

    private bool isControlledByAssignedTower(TerrainDesignation designation)
    {
      if (!this.m_excavator.AssignedTo.IsNone)
        return designation.IsManagedByTower(this.m_excavator.AssignedTo.Value);
      Log.Warning("Unassigned excavator trying to mine.");
      return false;
    }

    private MiningJob.State handleDrivingToNewMineLocation()
    {
      if (this.m_designationToMine.IsNone)
      {
        Log.Error("No designation to mine.");
        return MiningJob.State.Done;
      }
      TerrainDesignation designation = this.m_designationToMine.Value;
      if (this.StateChanged)
      {
        if (!this.isControlledByAssignedTower(designation))
          return MiningJob.State.Done;
        if (designation.IsMiningFulfilled)
          return MiningJob.State.WaitingForFulfilled;
        this.m_navHelper.StartNavigationTo((Vehicle) this.m_excavator, (IVehicleGoalFull) this.m_factory.DesignationGoalFactory.Create((IDesignation) designation, this.m_excavator.Prototype.MinMiningDistance), allowSimplePathsOnly: true);
        this.m_excavator.SetShovelState(ExcavatorShovelState.Tucked);
        this.m_driveStartPosition = this.m_excavator.Position2f;
        return MiningJob.State.DrivingToNewMineLocation;
      }
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          return MiningJob.State.DrivingToNewMineLocation;
        case RobustNavResult.GoalReachedSuccessfully:
          Assert.That<Tile2f>(this.m_driveStartPosition).IsNotEqualTo<Tile2f>(this.m_excavator.Position2f, "Nav failed to move excavator closer to any reachable mine tiles. Adjust tolerances/retries?");
          return MiningJob.State.DecideMiningOnReachableTile;
        case RobustNavResult.FailGoalUnreachable:
          if (!designation.IsDestroyed && designation.IsMiningNotFulfilled)
            this.m_factory.UnreachableDesignationsManager.MarkUnreachableFor((IDesignation) designation, (IPathFindingVehicle) this.m_excavator);
          if (this.m_primaryDesignation == designation)
            this.m_primaryDesignation = Option<TerrainDesignation>.None;
          else
            this.m_extraDesignations.Remove(designation);
          if (this.m_extraDesignations.IsEmpty)
            return MiningJob.State.CheckAllFulfilled;
          this.m_primaryDesignation = this.trySelectPrimaryDesignation(this.m_primaryDesignation);
          if (!this.m_primaryDesignation.HasValue)
            return MiningJob.State.CheckAllFulfilled;
          this.clearDesignationToMine();
          return MiningJob.State.InitialPathFindingAndDesignationSelect;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }
    }

    private bool hasReachableTilesToMine()
    {
      Fix64 minCost;
      return !this.m_designationToMine.IsNone && this.m_designationToMine.Value.FindBestNonFulfilledTileCoord<Fix64>((Func<TerrainDesignation, TerrainTile, RelTile2i, Fix64>) ((d, t, c) => (Fix64) t.TileCoord.DistanceSqrTo(this.m_excavator.GroundPositionTile2i)), (Option<Predicate<RelTile2i>>) (Predicate<RelTile2i>) (r => !this.m_designationToMine.Value.IsMiningFulfilledAt(r)), out minCost).HasValue && minCost <= this.m_excavator.Prototype.MaxMiningDistance.Squared;
    }

    private MiningJob.State handleDecideMiningOnReachableTile()
    {
      Assert.That<bool>(this.m_excavator.IsDriving).IsFalse();
      Assert.That<TerrainTile?>(this.m_tileToMine).IsNull<TerrainTile>();
      if (this.m_designationToMine.IsNone)
      {
        Log.Error("No designation to mine.");
        return MiningJob.State.CheckAllFulfilled;
      }
      if (!this.isControlledByAssignedTower(this.m_designationToMine.Value))
        return MiningJob.State.Done;
      if (this.m_excavator.DrivingState == DrivingState.Paused)
        return MiningJob.State.DecideMiningOnReachableTile;
      this.m_excavator.KeepTruckQueueEnabled(MiningJob.TRUCK_QUEUE_ENABLED_MOVING_DURATION);
      if (this.m_excavator.IsNotEmpty)
      {
        this.m_excavator.UnloadToTruck();
        return MiningJob.State.DecideMiningOnReachableTile;
      }
      if (this.m_excavator.CannotWorkDueToLowFuel)
      {
        this.m_excavator.TruckQueue.Disable();
        return MiningJob.State.Done;
      }
      if (this.m_excavator.PrioritizedProduct.HasValue && this.m_lastSeenPriorityProduct != this.m_excavator.PrioritizedProduct)
        return MiningJob.State.Done;
      if (this.m_designationToMine.Value.IsMiningFulfilled)
        return MiningJob.State.WaitingForFulfilled;
      Assert.That<bool>(this.m_excavator.IsEmpty).IsTrue();
      Option<Truck> firstVehicle = this.m_excavator.TruckQueue.FirstVehicle;
      if (firstVehicle.HasValue && firstVehicle.Value.IsNotEmpty && !(firstVehicle.Value.Cargo.FirstOrPhantom.Product is LooseProductProto))
      {
        Assert.Fail("Truck came to excavator with non-loose cargo!");
        this.m_excavator.TruckQueue.ReleaseFirstVehicle();
        return MiningJob.State.DecideMiningOnReachableTile;
      }
      TerrainTile tile;
      if (this.findBestTileToMine(Option<LooseProductProto>.None, out tile))
      {
        this.m_tileToMine = new TerrainTile?(tile);
        return MiningJob.State.PreparingToMine;
      }
      Assert.That<TerrainTile?>(this.m_tileToMine).IsNull<TerrainTile>();
      return MiningJob.State.DrivingToNewMineLocation;
    }

    /// <summary>
    /// Tries to find the best tile for mining according to cost function. This functions fills up <see cref="F:Mafi.Core.Vehicles.Jobs.MiningJob.m_tileToMine" />
    /// that can be then used by <see cref="M:Mafi.Core.Vehicles.Jobs.MiningJob.handleMining" />.
    /// </summary>
    /// <returns>True if a tile was found.</returns>
    private bool findBestTileToMine(Option<LooseProductProto> product, out TerrainTile tile)
    {
      Fix32 minCost;
      Tile2i? fulfilledTileCoord = this.m_designationToMine.Value.FindBestNonFulfilledTileCoord<Fix32>((Func<TerrainDesignation, TerrainTile, RelTile2i, Fix32>) this.m_targetTileCostFn, (Option<Predicate<RelTile2i>>) (Predicate<RelTile2i>) (r => !this.m_designationToMine.Value.IsMiningFulfilledAt(r)), out minCost);
      if (fulfilledTileCoord.HasValue && minCost < Fix32.MaxValue)
      {
        Assert.That<bool>(this.m_designationToMine.Value.IsMiningFulfilledAt(fulfilledTileCoord.Value)).IsFalse("Already fulfilled?");
        tile = this.m_factory.TerrainManager[fulfilledTileCoord.Value];
        return true;
      }
      tile = new TerrainTile();
      return false;
    }

    private IReadOnlySet<IDesignation> filterDestroyedAndUnreachable(
      Lyst<TerrainDesignation> designations)
    {
      this.m_unreachableDesignationsCache = this.m_factory.UnreachableDesignationsManager.GetUnreachableDesignationsFor((IPathFindingVehicle) this.m_excavator);
      designations.RemoveWhere((Predicate<TerrainDesignation>) this.m_filterDestroyedAndUnreachablePredicate);
      return this.m_unreachableDesignationsCache;
    }

    /// <summary>
    /// Cost function that is responsible for choosing the best tile on the designation.
    /// </summary>
    private Fix32 targetTileCostFunction(
      IDesignation designation,
      TerrainTile tile,
      RelTile2i coord)
    {
      if (tile.TileCoord.DistanceSqrTo(this.m_excavator.GroundPositionTile2i) > this.m_excavator.Prototype.MaxMiningDistance.Squared && this.m_previousState != MiningJob.State.DrivingToNewMineLocation)
        return Fix32.MaxValue;
      RelTile2i relTile2i = (Tile2i) tile.TileCoordSlim - this.m_excavator.GroundPositionTile2i;
      Fix32 zero = Fix32.Zero;
      if (relTile2i.IsNotZero)
      {
        Vector2f vector2f = relTile2i.Vector2f;
        ref Vector2f local = ref vector2f;
        AngleDegrees1f direction = this.m_excavator.Direction;
        Vector2f directionVector = direction.DirectionVector;
        direction = local.AngleTo(directionVector);
        AngleDegrees1f abs = direction.Abs;
        zero += abs.Radians;
      }
      if (this.m_terrainPropsManager.TerrainTileToProp.ContainsKey(tile.TileCoordSlim))
        return zero - (Fix32) 10;
      if (this.m_treesManager.Stumps.ContainsKey(new TreeId(tile.TileCoordSlim)))
        return zero - (Fix32) 10;
      ThicknessTilesF thicknessTilesF = tile.FirstLayer.Thickness.Min(tile.Height - designation.GetTargetHeightAt(coord));
      Assert.That<ThicknessTilesF>(thicknessTilesF).IsPositive();
      return zero - thicknessTilesF.Value.Min((Fix32) 2).Squared().ToFix32();
    }

    /// <summary>Prepares shovel and cabin for mining.</summary>
    private MiningJob.State handlePreparingToMine()
    {
      Assert.That<TerrainTile?>(this.m_tileToMine).HasValue<TerrainTile>();
      Assert.That<bool>(this.m_excavator.IsDriving).IsFalse();
      this.m_excavator.KeepTruckQueueEnabled(MiningJob.TRUCK_QUEUE_ENABLED_DURATION);
      if (this.StateChanged)
      {
        this.m_excavator.SetCabinTarget(this.m_tileToMine.Value.CenterTile2f, nameof (handlePreparingToMine));
        this.m_excavator.SetShovelState(ExcavatorShovelState.PrepareToMine);
      }
      return this.m_excavator.IsMoving || !this.m_excavator.CabinTargetDelta.IsNear(AngleDegrees1f.Zero, 10.Degrees()) || !this.m_excavator.IsShovelAtTarget ? MiningJob.State.PreparingToMine : MiningJob.State.Mining;
    }

    /// <summary>Performs the mining operation.</summary>
    private MiningJob.State handleMining()
    {
      Assert.That<TerrainTile?>(this.m_tileToMine).HasValue<TerrainTile>();
      Assert.That<bool>(this.m_excavator.IsDriving).IsFalse();
      Assert.That<AngleDegrees1f>(this.m_excavator.CabinTargetDelta).IsNear(AngleDegrees1f.Zero, 10.Degrees());
      this.m_excavator.KeepTruckQueueEnabled(MiningJob.TRUCK_QUEUE_ENABLED_DURATION);
      if (this.StateChanged)
      {
        Assert.That<ExcavatorShovelState>(this.m_excavator.ShovelState).IsEqualTo<ExcavatorShovelState>(ExcavatorShovelState.PrepareToMine);
        this.m_excavator.SetShovelState(ExcavatorShovelState.Mine);
        this.m_iterationsToMine = this.m_excavator.Prototype.MineTimings.MineTileIterations;
        this.m_timer.Start(this.m_excavator.Prototype.MineTimings.MineIterationDuration);
      }
      if (this.m_timer.Decrement())
        return MiningJob.State.Mining;
      TerrainPropId key;
      bool flag1 = this.m_terrainPropsManager.TerrainTileToProp.TryGetValue(this.m_tileToMine.Value.TileCoordSlim, out key);
      bool flag2 = false;
      if (!flag1)
      {
        flag2 = this.m_treesManager.Stumps.TryGetValue(new TreeId(this.m_tileToMine.Value.TileCoordSlim), out TreeStumpData _);
        if (!flag2)
          this.m_excavator.MineMixedAt(this.m_tileToMine.Value.CoordAndIndex, new Excavator.MineTileMixedFn(this.mineTileMixed));
      }
      --this.m_iterationsToMine;
      if (this.m_iterationsToMine > 0)
      {
        this.m_timer.Start(this.m_excavator.Prototype.MineTimings.MineIterationDuration);
        return MiningJob.State.Mining;
      }
      if (flag1)
      {
        TerrainPropData propData;
        if (!this.m_terrainPropsManager.TerrainProps.TryGetValue(key, out propData))
          Log.Warning(string.Format("Prop '{0}' can't find prop data.", (object) key));
        else if (!this.m_terrainPropsManager.TryRemovePropAtTile(this.m_tileToMine.Value.TileCoord, false))
          Log.Warning(string.Format("Failed to remove prop '{0}'.", (object) key));
        else
          this.m_excavator.MineProp(propData);
      }
      else if (flag2)
        this.m_treesManager.RemoveStumpAtTile(this.m_tileToMine.Value.TileCoord);
      this.m_tileToMine = new TerrainTile?();
      return MiningJob.State.WaitingForShovel;
    }

    private void mineTileMixed(
      Tile2iAndIndex tileAndIndex,
      PartialMinedProductTracker partialMinedProductTracker,
      ThicknessTilesF maxThickness)
    {
      Assert.That<ThicknessTilesF>(maxThickness).IsPositive("Max mined is not positive. Why are we even mining?");
      TerrainManager terrainManager = this.m_factory.TerrainManager;
      HeightTilesF targetHeight = this.getTargetHeight(tileAndIndex.TileCoord);
      HeightTilesF height = terrainManager.GetHeight(tileAndIndex.Index);
      if (height <= targetHeight)
        return;
      TileSurfaceData tileSurfaceData;
      if (terrainManager.TryGetTileSurface(tileAndIndex.Index, out tileSurfaceData) && !tileSurfaceData.IsAutoPlaced && tileSurfaceData.Height > targetHeight)
        terrainManager.ClearCustomSurface(tileAndIndex);
      TerrainMaterialThickness firstLayer = terrainManager.GetFirstLayer(tileAndIndex.Index);
      PartialQuantity quantity = partialMinedProductTracker.MaxAllowedQuantityOf((ProductProto) firstLayer.Material.MinedProduct);
      TerrainMaterialThicknessSlim materialThicknessSlim = new TerrainMaterialThicknessSlim();
      if (quantity.IsPositive)
      {
        ThicknessTilesF thicknessTilesF = height - targetHeight;
        thicknessTilesF = thicknessTilesF.Min(maxThickness);
        ThicknessTilesF maxThickness1 = thicknessTilesF.Min(firstLayer.Material.QuantityToThickness(quantity));
        if (maxThickness1.IsPositive)
        {
          materialThicknessSlim = terrainManager.MineMaterial(tileAndIndex, maxThickness1);
          terrainManager.DisruptExactly(tileAndIndex, ThicknessTilesF.One);
        }
      }
      ThicknessTilesF thicknessTilesF1 = materialThicknessSlim.Thickness;
      if (thicknessTilesF1.IsZero && terrainManager.GetFirstLayer(tileAndIndex.Index).Thickness <= MiningJob.MAX_TOP_LAYER_FOR_MINING_BELOW && terrainManager.GetLayersCountNoBedrock(tileAndIndex.Index) >= 1)
      {
        TerrainMaterialThickness secondLayer = terrainManager.GetSecondLayer(tileAndIndex.Index);
        quantity = partialMinedProductTracker.MaxAllowedQuantityOf((ProductProto) secondLayer.Material.MinedProduct);
        if (quantity.IsPositive)
        {
          thicknessTilesF1 = terrainManager.GetHeight(tileAndIndex.Index) - targetHeight;
          thicknessTilesF1 = thicknessTilesF1.Min(maxThickness);
          ThicknessTilesF maxThickness2 = thicknessTilesF1.Min(secondLayer.Material.QuantityToThickness(quantity));
          if (maxThickness2.IsPositive)
          {
            materialThicknessSlim = terrainManager.MineMaterialFromSecondLayer(tileAndIndex, maxThickness2);
            terrainManager.DisruptExactly(tileAndIndex, ThicknessTilesF.One);
          }
        }
      }
      thicknessTilesF1 = materialThicknessSlim.Thickness;
      if (!thicknessTilesF1.IsNotZero)
        return;
      PartialProductQuantity partialProductQuantity = materialThicknessSlim.ToPartialProductQuantity(terrainManager);
      partialMinedProductTracker.AddMinedProduct(partialProductQuantity);
    }

    private HeightTilesF getTargetHeight(Tile2i position)
    {
      if (this.m_designationToMine.Value.ContainsPosition(position))
        return this.m_designationToMine.Value.GetTargetHeightAt(position);
      Option<TerrainDesignation> miningDesignationAt = this.m_factory.MiningManager.GetMiningDesignationAt(position);
      return miningDesignationAt.HasValue ? miningDesignationAt.Value.GetTargetHeightAt(position) : HeightTilesF.MaxValue;
    }

    private MiningJob.State handleWaitingForShovel()
    {
      return !this.m_excavator.IsShovelAtTarget ? MiningJob.State.WaitingForShovel : MiningJob.State.DecideMiningOnReachableTile;
    }

    /// <summary>
    /// When designation is fulfilled, we wait little bit to make sure it stays this way.
    /// </summary>
    private MiningJob.State handleWaitingForFulfilled()
    {
      if (this.StateChanged)
        this.m_timer.Start(MiningJob.DONE_WAITING_DURATION);
      if (!this.m_designationToMine.Value.IsMiningFulfilled)
        return MiningJob.State.DecideMiningOnReachableTile;
      if (!this.m_timer.Decrement())
        return MiningJob.State.Done;
      this.m_excavator.KeepTruckQueueEnabled(MiningJob.TRUCK_QUEUE_ENABLED_DURATION);
      return MiningJob.State.WaitingForFulfilled;
    }

    /// <summary>
    /// Waits a little (for terrain physics) and moves all non-fulfilled designations from
    /// `m_fulfilledDesignations` to `m_extraDesignations` and promotes the first one to be the
    /// `m_primaryDesignation`.
    /// 
    /// Returns <see cref="F:Mafi.Core.Vehicles.Jobs.MiningJob.State.Done" /> if all designations are fulfilled.
    /// </summary>
    private MiningJob.State handleCheckAllFulfilled()
    {
      if (this.StateChanged)
      {
        if (this.m_designationToMine.HasValue)
          this.clearDesignationToMine();
        this.m_timer.Start(MiningJob.DONE_WAITING_DURATION);
        return MiningJob.State.CheckAllFulfilled;
      }
      if (this.m_timer.Decrement())
        return MiningJob.State.CheckAllFulfilled;
      this.m_fulfilledDesignations.RemoveWhere((Predicate<TerrainDesignation>) (x => x.IsMiningFulfilled));
      this.m_extraDesignations.AddRange(this.m_fulfilledDesignations);
      this.m_fulfilledDesignations.Clear();
      this.m_primaryDesignation = this.trySelectPrimaryDesignation(this.m_primaryDesignation);
      return this.m_primaryDesignation.HasValue ? MiningJob.State.InitialPathFindingAndDesignationSelect : MiningJob.State.Done;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.cleanup();
      ((IVehicleFriend) this.m_excavator).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      Assert.That<Excavator>(this.m_excavator).IsNotNull<Excavator>("ReturnToPool on non-initialized instance.");
      this.cleanup();
      this.m_primaryDesignation = Option<TerrainDesignation>.None;
      this.m_extraDesignations.Clear();
      this.m_tileToMine = new TerrainTile?();
      this.m_unreachableDesignationsCache = (IReadOnlySet<IDesignation>) null;
      this.m_lastMinedProduct = Option<LooseProductProto>.None;
      this.m_timer.Reset();
    }

    static MiningJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MiningJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      MiningJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      MiningJob.DONE_WAITING_DURATION = 1.Seconds();
      MiningJob.MAX_TOP_LAYER_FOR_MINING_BELOW = 0.5.TilesThick();
      MiningJob.TRUCK_QUEUE_ENABLED_DURATION = 20.Seconds();
      MiningJob.TRUCK_QUEUE_ENABLED_MOVING_DURATION = 30.Seconds();
    }

    private enum State
    {
      InitialPathFindingAndDesignationSelect,
      DrivingToNewMineLocation,
      DecideMiningOnReachableTile,
      PreparingToMine,
      Mining,
      WaitingForShovel,
      WaitingForFulfilled,
      Done,
      CheckAllFulfilled,
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly ITerrainMiningManager MiningManager;
      public readonly IVehiclePathFindingManager PathFindingManager;
      public readonly IFuelStationsManager FuelStationsManager;
      public readonly TerrainManager TerrainManager;
      public readonly TreesManager TreesManager;
      public readonly TerrainPropsManager TerrainPropsManager;
      public readonly TerrainDesignationVehicleGoal.Factory DesignationGoalFactory;
      public readonly UnreachableTerrainDesignationsManager UnreachableDesignationsManager;
      private readonly Lyst<TerrainDesignation> m_extraDesignationsTmp;

      public Factory(
        VehicleJobId.Factory vehicleJobIdFactory,
        ITerrainMiningManager miningManager,
        IVehiclePathFindingManager pathFindingManager,
        IFuelStationsManager fuelStationsManager,
        TerrainManager terrainManager,
        TreesManager treesManager,
        TerrainPropsManager terrainPropsManager,
        TerrainDesignationVehicleGoal.Factory designationGoalFactory,
        UnreachableTerrainDesignationsManager unreachableDesignationsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_extraDesignationsTmp = new Lyst<TerrainDesignation>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.MiningManager = miningManager;
        this.PathFindingManager = pathFindingManager;
        this.FuelStationsManager = fuelStationsManager;
        this.TerrainManager = terrainManager;
        this.TreesManager = treesManager;
        this.TerrainPropsManager = terrainPropsManager;
        this.DesignationGoalFactory = designationGoalFactory;
        this.UnreachableDesignationsManager = unreachableDesignationsManager;
      }

      public void EnqueueJob(
        Excavator excavator,
        TerrainDesignation designation,
        IEnumerable<TerrainDesignation> extraDesignations)
      {
        this.m_extraDesignationsTmp.Clear();
        MiningJob miningJob = new MiningJob(this.m_vehicleJobIdFactory.GetNextId(), this.TerrainPropsManager, this.TreesManager, this, excavator, designation, extraDesignations ?? (IEnumerable<TerrainDesignation>) this.m_extraDesignationsTmp);
      }

      public bool TryCreateAndEnqueueJob(
        Excavator excavator,
        bool tryIgnoreReservations = false,
        Predicate<TerrainDesignation> predicate = null)
      {
        if (excavator.MineTower.IsNone)
          return false;
        MineTower tower = excavator.MineTower.Value;
        if (!tower.IsEnabled)
          return false;
        this.m_extraDesignationsTmp.Clear();
        TerrainDesignation bestDesignation;
        if (!this.MiningManager.TryFindClosestReadyToMine(tower, excavator.Position2f.Tile2i, (Vehicle) excavator, out bestDesignation, excavator.PrioritizedProduct, tryIgnoreReservations, predicate, this.m_extraDesignationsTmp))
          return false;
        MiningJob miningJob = new MiningJob(this.m_vehicleJobIdFactory.GetNextId(), this.TerrainPropsManager, this.TreesManager, this, excavator, bestDesignation, (IEnumerable<TerrainDesignation>) this.m_extraDesignationsTmp);
        this.m_extraDesignationsTmp.Clear();
        return true;
      }
    }
  }
}
