// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.DumpingJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job in which a vehicle dumps material on terrain using an assigned terrain designation.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class DumpingJob : VehicleJob, IJobWithPreNavigation
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly TickTimer m_timer;
    private readonly Truck m_truck;
    private TerrainDesignation m_primaryDesignation;
    private readonly Lyst<TerrainDesignation> m_extraDesignations;
    private readonly Lyst<TerrainDesignation> m_fulfilledDesignations;
    private Option<TerrainDesignation> m_designationToDump;
    private TerrainTile? m_tileToDump;
    [NewInSaveVersion(140, null, null, null, null)]
    private readonly Option<LooseProductProto> m_productToDump;
    [RenamedInVersion(140, "m_productToDump")]
    [Obsolete]
    [DoNotSave(140, null)]
    private readonly LooseProductProto m_productToDumpOld;
    private int m_dumpIterationsWithoutQuantityChange;
    private Quantity m_dumpedTotal;
    [NewInSaveVersion(140, null, null, null, null)]
    private Option<Lyst<KeyValuePair<ProductProto, Quantity>>> m_dumpedTotals;
    private DumpingJob.State m_state;
    private DumpingJob.State m_previousState;
    private int m_navAttempts;
    private Option<TerrainDesignationVehicleGoal> m_initialNavGoal;
    private readonly DumpingJob.Factory m_factory;
    private readonly RobustNavHelper m_navHelper;
    [DoNotSave(0, null)]
    private Func<TerrainDesignation, TerrainTile, RelTile2i, Fix64> m_targetTileCostFn;
    [DoNotSave(0, null)]
    private Predicate<TerrainDesignation> m_filterDestroyedAndUnreachablePredicate;
    [DoNotSave(0, null)]
    private IReadOnlySet<IDesignation> m_unreachableDesignationsCache;

    public static void Serialize(DumpingJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<DumpingJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, DumpingJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<TerrainDesignation>.Serialize(this.m_designationToDump, writer);
      Quantity.Serialize(this.m_dumpedTotal, writer);
      Option<Lyst<KeyValuePair<ProductProto, Quantity>>>.Serialize(this.m_dumpedTotals, writer);
      writer.WriteInt(this.m_dumpIterationsWithoutQuantityChange);
      Lyst<TerrainDesignation>.Serialize(this.m_extraDesignations, writer);
      Lyst<TerrainDesignation>.Serialize(this.m_fulfilledDesignations, writer);
      Option<TerrainDesignationVehicleGoal>.Serialize(this.m_initialNavGoal, writer);
      writer.WriteInt(this.m_navAttempts);
      RobustNavHelper.Serialize(this.m_navHelper, writer);
      writer.WriteInt((int) this.m_previousState);
      TerrainDesignation.Serialize(this.m_primaryDesignation, writer);
      Option<LooseProductProto>.Serialize(this.m_productToDump, writer);
      writer.WriteInt((int) this.m_state);
      writer.WriteNullableStruct<TerrainTile>(this.m_tileToDump);
      TickTimer.Serialize(this.m_timer, writer);
      Truck.Serialize(this.m_truck, writer);
    }

    public static DumpingJob Deserialize(BlobReader reader)
    {
      DumpingJob dumpingJob;
      if (reader.TryStartClassDeserialization<DumpingJob>(out dumpingJob))
        reader.EnqueueDataDeserialization((object) dumpingJob, DumpingJob.s_deserializeDataDelayedAction);
      return dumpingJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.m_designationToDump = Option<TerrainDesignation>.Deserialize(reader);
      this.m_dumpedTotal = Quantity.Deserialize(reader);
      this.m_dumpedTotals = reader.LoadedSaveVersion >= 140 ? Option<Lyst<KeyValuePair<ProductProto, Quantity>>>.Deserialize(reader) : new Option<Lyst<KeyValuePair<ProductProto, Quantity>>>();
      this.m_dumpIterationsWithoutQuantityChange = reader.ReadInt();
      reader.SetField<DumpingJob>(this, "m_extraDesignations", (object) Lyst<TerrainDesignation>.Deserialize(reader));
      reader.RegisterResolvedMember<DumpingJob>(this, "m_factory", typeof (DumpingJob.Factory), true);
      reader.SetField<DumpingJob>(this, "m_fulfilledDesignations", (object) Lyst<TerrainDesignation>.Deserialize(reader));
      this.m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.Deserialize(reader);
      this.m_navAttempts = reader.ReadInt();
      reader.SetField<DumpingJob>(this, "m_navHelper", (object) RobustNavHelper.Deserialize(reader));
      this.m_previousState = (DumpingJob.State) reader.ReadInt();
      this.m_primaryDesignation = TerrainDesignation.Deserialize(reader);
      reader.SetField<DumpingJob>(this, "m_productToDump", (object) (reader.LoadedSaveVersion >= 140 ? Option<LooseProductProto>.Deserialize(reader) : new Option<LooseProductProto>()));
      if (reader.LoadedSaveVersion < 140)
        reader.SetField<DumpingJob>(this, "m_productToDumpOld", (object) reader.ReadGenericAs<LooseProductProto>());
      this.m_state = (DumpingJob.State) reader.ReadInt();
      this.m_tileToDump = reader.ReadNullableStruct<TerrainTile>();
      reader.SetField<DumpingJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<DumpingJob>(this, "m_truck", (object) Truck.Deserialize(reader));
      reader.RegisterInitAfterLoad<DumpingJob>(this, "initSelf", InitPriority.Normal);
      reader.RegisterInitAfterLoad<DumpingJob>(this, "initForUpdate2", InitPriority.Normal);
    }

    public override LocStrFormatted JobInfo
    {
      get
      {
        switch (this.m_state)
        {
          case DumpingJob.State.InitialPathFindingAndDesignationSelect:
            Truck truck = this.m_truck;
            return (LocStrFormatted) ((truck != null ? (truck.IsDriving ? 1 : 0) : 0) != 0 ? TrCore.VehicleJob__DrivingToGoal : TrCore.VehicleJob__SearchingForDesignation);
          case DumpingJob.State.DrivingToDumpLocation:
            return (LocStrFormatted) TrCore.VehicleJob__DrivingToGoal;
          case DumpingJob.State.DecideDumpingOnReachableTile:
          case DumpingJob.State.Dumping:
          case DumpingJob.State.WaitingForFulfilled:
          case DumpingJob.State.CheckAllFulfilled:
            return (LocStrFormatted) TrCore.VehicleJob__Unloading;
          case DumpingJob.State.FindMoreDesignations:
            return (LocStrFormatted) TrCore.VehicleJob__SearchingForDesignation;
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
        return this.m_state != DumpingJob.State.WaitingForFulfilled && (this.m_state != DumpingJob.State.InitialPathFindingAndDesignationSelect || this.m_truck.IsDriving) ? VehicleFuelConsumption.Full : VehicleFuelConsumption.Idle;
      }
    }

    public LocStrFormatted GoalName
    {
      get => (LocStrFormatted) this.m_primaryDesignation.Prototype.Strings.Name;
    }

    public TerrainDesignation PrimaryDesignation => this.m_primaryDesignation;

    public IIndexable<TerrainDesignation> ExtraDesignations
    {
      get => (IIndexable<TerrainDesignation>) this.m_extraDesignations;
    }

    /// <summary>Whether the state changed last update.</summary>
    private bool StateChanged => this.m_state != this.m_previousState;

    private DumpingJob(
      VehicleJobId id,
      DumpingJob.Factory factory,
      Truck truck,
      Option<ProductProto> product,
      TerrainDesignation primaryDesignation,
      IEnumerable<TerrainDesignation> extraDesignations)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      this.m_fulfilledDesignations = new Lyst<TerrainDesignation>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_factory = factory.CheckNotNull<DumpingJob.Factory>();
      this.m_navHelper = new RobustNavHelper(factory.PathFindingManager);
      this.initSelf();
      Assert.That<Option<TerrainDesignation>>(this.m_designationToDump).IsNone<TerrainDesignation>();
      Assert.That<bool>(primaryDesignation.IsDumpingFulfilled).IsFalse();
      this.m_truck = truck.CheckNotNull<Truck>();
      this.m_primaryDesignation = primaryDesignation;
      this.m_extraDesignations = extraDesignations.ToLyst<TerrainDesignation>();
      this.m_productToDump = (Option<LooseProductProto>) (product.ValueOrNull as LooseProductProto);
      if (product.IsNone)
        this.m_dumpedTotals = (Option<Lyst<KeyValuePair<ProductProto, Quantity>>>) new Lyst<KeyValuePair<ProductProto, Quantity>>();
      this.m_truck.EnqueueJob((VehicleJob) this, false);
      this.m_truck.DumpingOfAllCargoPending = true;
      this.m_previousState = DumpingJob.State.Done;
      this.m_state = DumpingJob.State.InitialPathFindingAndDesignationSelect;
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.m_targetTileCostFn = new Func<TerrainDesignation, TerrainTile, RelTile2i, Fix64>(this.targetTileCostFunction);
      this.m_filterDestroyedAndUnreachablePredicate = (Predicate<TerrainDesignation>) (d => d.IsDestroyed || this.m_unreachableDesignationsCache.Contains((IDesignation) d));
    }

    [InitAfterLoad(InitPriority.Normal)]
    [OnlyForSaveCompatibility(null)]
    private void initForUpdate2(int saveVersion)
    {
      if (saveVersion >= 140)
        return;
      ReflectionUtils.SetField<DumpingJob>(this, "m_productToDump", (object) this.m_productToDumpOld.SomeOption<LooseProductProto>());
    }

    public RobustNavHelper StartNavigation()
    {
      TerrainDesignationVehicleGoal goal = this.m_factory.DesignationGoalFactory.Create((IDesignation) this.m_primaryDesignation, this.m_truck.Prototype.MinDumpingDistance, (IEnumerable<IDesignation>) this.m_extraDesignations.Where<TerrainDesignation>((Func<TerrainDesignation, bool>) (x => x.IsDumpingReadyToBeFulfilled)));
      this.m_navHelper.StartNavigationTo((Vehicle) this.m_truck, (IVehicleGoalFull) goal);
      this.m_initialNavGoal = (Option<TerrainDesignationVehicleGoal>) goal;
      return this.m_navHelper;
    }

    protected override bool DoJobInternal()
    {
      DumpingJob.State state = this.handleState();
      this.m_previousState = this.m_state;
      this.m_state = state;
      if (this.m_state != DumpingJob.State.Done)
        return true;
      this.cleanup();
      return false;
    }

    private void clearDesignationToDump()
    {
      if (!this.m_designationToDump.HasValue)
        return;
      this.m_designationToDump.Value.RemoveAssignment((IVehicleJob) this);
      this.m_designationToDump = Option<TerrainDesignation>.None;
    }

    private void cleanup()
    {
      if (this.m_dumpedTotals.HasValue)
      {
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_dumpedTotals.Value)
          this.m_factory.VehicleJobStatsManager.RecordJobStatsFor(this.m_truck, keyValuePair.Key.WithQuantity(keyValuePair.Value));
        this.m_dumpedTotals = (Option<Lyst<KeyValuePair<ProductProto, Quantity>>>) Option.None;
      }
      else if (this.m_productToDump.HasValue && this.m_dumpedTotal.IsPositive)
      {
        this.m_factory.VehicleJobStatsManager.RecordJobStatsFor(this.m_truck, this.m_productToDump.Value.WithQuantity(this.m_dumpedTotal));
        this.m_dumpedTotal = Quantity.Zero;
      }
      this.m_truck.IsDumping = false;
      this.clearDesignationToDump();
      this.m_navHelper.CancelAndClear();
      this.m_extraDesignations.Clear();
      this.m_fulfilledDesignations.Clear();
    }

    private DumpingJob.State handleState()
    {
      switch (this.m_state)
      {
        case DumpingJob.State.InitialPathFindingAndDesignationSelect:
          return this.handleInitialPathFindingAndDesignationSelect();
        case DumpingJob.State.DrivingToDumpLocation:
          return this.handleDrivingToDumpLocation();
        case DumpingJob.State.DecideDumpingOnReachableTile:
          return this.handleDecideDumpingOnReachableTile();
        case DumpingJob.State.Dumping:
          return this.handleDumping();
        case DumpingJob.State.WaitingForFulfilled:
          return this.handleWaitingForFulfilled();
        case DumpingJob.State.Done:
          Log.Warning(string.Format("Already done! Previous state: {0}", (object) this.m_previousState));
          return DumpingJob.State.Done;
        case DumpingJob.State.CheckAllFulfilled:
          return this.handleCheckAllFulfilled();
        case DumpingJob.State.FindMoreDesignations:
          return this.handleFindMoreDesignations();
        default:
          Assert.Fail(string.Format("Unknown/invalid mining job state '{0}'.", (object) this.m_state));
          return DumpingJob.State.Done;
      }
    }

    private bool trySelectPrimaryDesignation()
    {
      IReadOnlySet<IDesignation> readOnlySet = this.filterDestroyedAndUnreachable(this.m_extraDesignations);
      this.m_extraDesignations.RemoveWhere((Predicate<TerrainDesignation>) (x => x.IsDumpingFulfilled), this.m_fulfilledDesignations);
      if (!this.m_primaryDesignation.IsDestroyed && this.m_primaryDesignation.IsDumpingNotFulfilled && this.m_primaryDesignation.IsDumpingReadyToBeFulfilled && this.m_primaryDesignation.CanBeAssigned(false) && !readOnlySet.Contains((IDesignation) this.m_primaryDesignation))
        return true;
      if (!this.m_primaryDesignation.IsDestroyed && !readOnlySet.Contains((IDesignation) this.m_primaryDesignation))
      {
        if (this.m_primaryDesignation.IsDumpingFulfilled)
          this.m_fulfilledDesignations.Add(this.m_primaryDesignation);
        else
          this.m_extraDesignations.Add(this.m_primaryDesignation);
      }
      for (int index = 0; index < this.m_extraDesignations.Count; ++index)
      {
        TerrainDesignation extraDesignation = this.m_extraDesignations[index];
        if (extraDesignation.IsDumpingReadyToBeFulfilled && extraDesignation.CanBeAssigned(false))
        {
          this.m_primaryDesignation = extraDesignation;
          this.m_extraDesignations.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    private DumpingJob.State handleInitialPathFindingAndDesignationSelect()
    {
      if (this.StateChanged)
      {
        Assert.That<Option<TerrainDesignation>>(this.m_designationToDump).IsNone<TerrainDesignation>();
        if (this.m_truck.IsEmpty)
        {
          Log.Warning("Dumping job has nothing to do.");
          return DumpingJob.State.Done;
        }
        if (!this.trySelectPrimaryDesignation())
          return DumpingJob.State.CheckAllFulfilled;
        if (!this.m_navHelper.IsNavigating)
        {
          this.StartNavigation();
          return DumpingJob.State.InitialPathFindingAndDesignationSelect;
        }
      }
      if (this.m_designationToDump.IsNone && this.m_initialNavGoal.Value.ActualGoalDesignation.ValueOrNull is TerrainDesignation valueOrNull)
      {
        if (tryAssignTo(valueOrNull))
          this.m_designationToDump = (Option<TerrainDesignation>) valueOrNull;
        else if (valueOrNull != this.m_primaryDesignation && tryAssignTo(this.m_primaryDesignation))
        {
          this.m_designationToDump = (Option<TerrainDesignation>) this.m_primaryDesignation;
        }
        else
        {
          this.m_extraDesignations.RemoveWhere((Predicate<TerrainDesignation>) (x => x.IsDestroyed));
          foreach (TerrainDesignation extraDesignation in this.m_extraDesignations)
          {
            if (tryAssignTo(extraDesignation))
            {
              this.m_designationToDump = (Option<TerrainDesignation>) extraDesignation;
              break;
            }
          }
          if (this.m_designationToDump.IsNone)
            return DumpingJob.State.Done;
        }
        Assert.That<Option<TerrainDesignation>>(this.m_designationToDump).HasValue<TerrainDesignation>();
        Assert.That<bool>(this.m_designationToDump.Value.IsDumpingNotFulfilled).IsTrue();
        Assert.That<bool>(this.m_designationToDump.Value.IsDumpingReadyToBeFulfilled).IsTrue();
        this.m_truck.DeactivateCannotDeliver();
      }
      Assert.That<Option<TerrainDesignationVehicleGoal>>(this.m_initialNavGoal).HasValue<TerrainDesignationVehicleGoal>();
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          return DumpingJob.State.InitialPathFindingAndDesignationSelect;
        case RobustNavResult.GoalReachedSuccessfully:
          if (this.m_designationToDump.IsNone)
          {
            Log.Error("Goal reached but designation was not set.");
            this.m_designationToDump = (Option<TerrainDesignation>) this.m_primaryDesignation;
            if (!this.m_designationToDump.Value.TryAssignTo((IVehicleJob) this))
              return DumpingJob.State.Done;
          }
          this.m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.None;
          this.m_navAttempts = 0;
          return DumpingJob.State.DecideDumpingOnReachableTile;
        case RobustNavResult.FailGoalUnreachable:
          return DumpingJob.State.CheckAllFulfilled;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }

      bool tryAssignTo(TerrainDesignation d)
      {
        return !d.IsDestroyed && d.IsDumpingNotFulfilled && d.IsDumpingReadyToBeFulfilled && d.TryAssignTo((IVehicleJob) this);
      }
    }

    private DumpingJob.State handleDrivingToDumpLocation()
    {
      if (this.m_designationToDump.IsNone)
      {
        Log.Error("No designation to dump.");
        return DumpingJob.State.CheckAllFulfilled;
      }
      if (this.StateChanged)
      {
        this.m_truck.DeactivateCannotDeliver();
        if (this.m_truck.IsEmpty)
          return DumpingJob.State.Done;
        if (this.m_designationToDump.Value.IsDumpingFulfilled)
          return DumpingJob.State.WaitingForFulfilled;
        this.m_navHelper.StartNavigationTo((Vehicle) this.m_truck, (IVehicleGoalFull) this.m_factory.DesignationGoalFactory.Create((IDesignation) this.m_designationToDump.Value, this.m_truck.Prototype.MinDumpingDistance), allowSimplePathsOnly: true);
        ++this.m_navAttempts;
        return DumpingJob.State.DrivingToDumpLocation;
      }
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          return DumpingJob.State.DrivingToDumpLocation;
        case RobustNavResult.GoalReachedSuccessfully:
          return DumpingJob.State.DecideDumpingOnReachableTile;
        case RobustNavResult.FailGoalUnreachable:
          if (this.m_extraDesignations.IsEmpty)
            return DumpingJob.State.CheckAllFulfilled;
          if (this.m_primaryDesignation == this.m_designationToDump)
            this.m_primaryDesignation = this.m_extraDesignations.PopLast();
          else
            this.m_extraDesignations.Remove(this.m_designationToDump.Value);
          if (!this.trySelectPrimaryDesignation())
            return DumpingJob.State.CheckAllFulfilled;
          this.clearDesignationToDump();
          return DumpingJob.State.InitialPathFindingAndDesignationSelect;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }
    }

    private DumpingJob.State handleDecideDumpingOnReachableTile()
    {
      Assert.That<bool>(this.m_truck.IsDriving).IsFalse();
      Assert.That<TerrainTile?>(this.m_tileToDump).IsNull<TerrainTile>();
      if (this.m_truck.IsEmpty)
      {
        this.m_truck.IsDumping = false;
        return DumpingJob.State.Done;
      }
      if (this.m_designationToDump.IsNone)
      {
        Log.Error("No designation to dump on.");
        return DumpingJob.State.CheckAllFulfilled;
      }
      TerrainDesignation designation = this.m_designationToDump.Value;
      Fix64 minDistSqr;
      Tile2i? closestTileCoordToDump = this.findClosestTileCoordToDump(designation, out minDistSqr);
      if (!closestTileCoordToDump.HasValue)
      {
        Assert.That<bool>(this.hasReachableTilesToDump(designation)).IsFalse<Truck>("Vehicle '{0}' has dumpable tiles in range but dumping has ended.", this.m_truck);
        this.m_truck.IsDumping = false;
        return DumpingJob.State.DrivingToDumpLocation;
      }
      if (minDistSqr > this.m_truck.Prototype.MaxDumpingDistance.Squared)
      {
        if (this.m_navAttempts <= 2)
        {
          this.m_truck.IsDumping = false;
          return DumpingJob.State.DrivingToDumpLocation;
        }
        Log.Warning(string.Format("Vehicle '{0}' just finished navigation to a new dumping goal but the closest tile ", (object) this.m_truck) + string.Format("is still not reachable ({0} > {1}). ", (object) minDistSqr.Sqrt(), (object) this.m_truck.Prototype.MaxDumpingDistance) + "Increase `MaxDumpingDistance` or decrease `DumpingNavTolerance`?");
      }
      Assert.That<bool>(designation.IsDumpingFulfilledAt(closestTileCoordToDump.Value)).IsFalse("Already fulfilled?");
      this.m_tileToDump = new TerrainTile?(this.m_factory.TerrainManager[closestTileCoordToDump.Value]);
      this.m_navAttempts = 0;
      return DumpingJob.State.Dumping;
    }

    /// <summary>
    /// Finds the closest tile available for dumping. Returns null when no tile is available.
    /// </summary>
    private Tile2i? findClosestTileCoordToDump(TerrainDesignation designation, out Fix64 minDistSqr)
    {
      Predicate<RelTile2i> predicate = (Predicate<RelTile2i>) (r => !designation.IsDumpingFulfilledAt(r));
      return designation.FindBestNonFulfilledTileCoord<Fix64>(this.m_targetTileCostFn, (Option<Predicate<RelTile2i>>) predicate, out minDistSqr);
    }

    private IReadOnlySet<IDesignation> filterDestroyedAndUnreachable(
      Lyst<TerrainDesignation> designations)
    {
      this.m_unreachableDesignationsCache = this.m_factory.UnreachableDesignationsManager.GetUnreachableDesignationsFor((IPathFindingVehicle) this.m_truck);
      designations.RemoveWhere(this.m_filterDestroyedAndUnreachablePredicate);
      return this.m_unreachableDesignationsCache;
    }

    private Fix64 targetTileCostFunction(
      IDesignation designation,
      TerrainTile tile,
      RelTile2i coord)
    {
      return tile.CenterTile2f.DistanceSqrTo(this.m_truck.Position2f);
    }

    private bool hasReachableTilesToDump(TerrainDesignation designation)
    {
      Fix64 minDistSqr;
      return this.findClosestTileCoordToDump(designation, out minDistSqr).HasValue && minDistSqr < this.m_truck.Prototype.MaxDumpingDistance.Squared;
    }

    private Duration getDumpingDuration()
    {
      return this.m_truck.SpeedFactor.ApplyInverseFloored(this.m_truck.Prototype.DumpIterationDuration.Ticks).Ticks();
    }

    private DumpingJob.State handleDumping()
    {
      if (!this.m_tileToDump.HasValue)
      {
        Log.Error("Invalid state, no tile to dump.");
        return DumpingJob.State.DecideDumpingOnReachableTile;
      }
      if (this.StateChanged)
      {
        this.m_timer.Start(this.getDumpingDuration());
        this.m_dumpIterationsWithoutQuantityChange = 0;
      }
      this.m_truck.IsDumping = true;
      if (this.m_timer.Decrement())
        return DumpingJob.State.Dumping;
      if (this.m_designationToDump.IsNone)
      {
        Log.Error("No designation to dump on (during dumping).");
        return DumpingJob.State.CheckAllFulfilled;
      }
      TerrainDesignation terrainDesignation = this.m_designationToDump.Value;
      TerrainTile terrainTile = this.m_tileToDump.Value;
      HeightTilesF height = terrainTile.Height;
      Quantity quantity1 = Quantity.Zero;
      if (this.m_productToDump.HasValue)
      {
        quantity1 = this.m_truck.DumpLooseAt(terrainTile.CoordAndIndex, (ProductProto) this.m_productToDump.Value, new Truck.DumpTileFn(this.dumpTile));
        this.m_dumpedTotal += quantity1;
        if (this.m_truck.Cargo.GetQuantityOf((ProductProto) this.m_productToDump.Value).IsZero)
        {
          this.m_tileToDump = new TerrainTile?();
          this.m_truck.IsDumping = false;
          return DumpingJob.State.Done;
        }
      }
      else
      {
        bool flag = false;
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_truck.Cargo)
        {
          if (keyValuePair.Key is LooseProductProto key && !key.DumpableProduct.IsNone)
          {
            flag = true;
            quantity1 = this.m_truck.DumpLooseAt(terrainTile.CoordAndIndex, (ProductProto) key, new Truck.DumpTileFn(this.dumpTile));
            if (quantity1.IsPositive)
            {
              Quantity quantity2;
              int index;
              if (this.m_dumpedTotals.Value.TryGetValue<ProductProto, Quantity>(keyValuePair.Key, out quantity2, out index))
              {
                this.m_dumpedTotals.Value[index] = Make.Kvp<ProductProto, Quantity>(keyValuePair.Key, quantity2 + quantity1);
                break;
              }
              this.m_dumpedTotals.Value.Add<ProductProto, Quantity>(keyValuePair.Key, quantity1);
              break;
            }
            break;
          }
        }
        if (this.m_truck.Cargo.IsEmpty || !flag)
        {
          this.m_tileToDump = new TerrainTile?();
          this.m_truck.IsDumping = false;
          return DumpingJob.State.Done;
        }
      }
      if (quantity1.IsPositive)
      {
        this.m_dumpIterationsWithoutQuantityChange = 0;
        this.m_timer.Start(this.getDumpingDuration());
        return DumpingJob.State.Dumping;
      }
      ++this.m_dumpIterationsWithoutQuantityChange;
      if (this.m_dumpIterationsWithoutQuantityChange < this.m_truck.Prototype.MinDumpIterationsWithoutQuantityChanged)
      {
        this.m_timer.Start(this.getDumpingDuration());
        return DumpingJob.State.Dumping;
      }
      if (terrainDesignation.IsDumpingFulfilledAt((Tile2i) terrainTile.TileCoordSlim))
      {
        this.m_tileToDump = new TerrainTile?();
        return DumpingJob.State.DecideDumpingOnReachableTile;
      }
      if (terrainTile.Height == height)
      {
        Log.Error("Designation is not fulfilled but dumping operation failed to change tile height.");
        Lyst<KeyValuePair<Tile2i, bool>> errorLocations = new Lyst<KeyValuePair<Tile2i, bool>>();
        terrainDesignation.ValidateAndFixFulfilledBitmap(errorLocations);
        if (errorLocations.IsNotEmpty)
          Log.Error("Designation validation failed during dumping: " + ((IEnumerable<string>) errorLocations.Select<string>((Func<KeyValuePair<Tile2i, bool>, string>) (x => string.Format("{0} incorrect value: {1}fulfilled", (object) x.Key, x.Value ? (object) "" : (object) "not ")))).JoinStrings("\n"));
        this.m_tileToDump = new TerrainTile?();
        return DumpingJob.State.DecideDumpingOnReachableTile;
      }
      this.m_timer.Start(this.getDumpingDuration());
      return DumpingJob.State.Dumping;
    }

    private bool dumpTile(
      Tile2iAndIndex tileAndIndex,
      ref TerrainMaterialThickness product,
      ThicknessTilesF maxDumped)
    {
      Assert.That<bool>(product.IsNotEmpty).IsTrue();
      TerrainManager terrainManager = this.m_factory.TerrainManager;
      HeightTilesF targetHeight = this.getTargetHeight(tileAndIndex.TileCoord);
      HeightTilesF height = terrainManager.GetHeight(tileAndIndex.Index);
      if (height >= targetHeight)
        return true;
      ThicknessTilesF thicknessTilesF = targetHeight - height;
      thicknessTilesF = thicknessTilesF.Min(maxDumped);
      ThicknessTilesF thickness = thicknessTilesF.Min(product.Thickness);
      terrainManager.DumpMaterial(tileAndIndex, new TerrainMaterialThicknessSlim(product.Material.SlimId, thickness));
      product = new TerrainMaterialThickness(product.Material, product.Thickness - thickness);
      return product.IsNotEmpty;
    }

    private HeightTilesF getTargetHeight(Tile2i position)
    {
      if (this.m_designationToDump.Value.ContainsPosition(position))
        return this.m_designationToDump.Value.GetTargetHeightAt(position);
      Option<TerrainDesignation> dumpingDesignationAt = this.m_factory.DumpingManager.GetDumpingDesignationAt(position);
      if (!dumpingDesignationAt.HasValue || this.m_designationToDump.Value.ManagedByTowers.Count != dumpingDesignationAt.Value.ManagedByTowers.Count)
        return HeightTilesF.MinValue;
      foreach (IAreaManagingTower managedByTower in this.m_designationToDump.Value.ManagedByTowers)
      {
        if (!dumpingDesignationAt.Value.ManagedByTowers.Contains(managedByTower))
          return HeightTilesF.MinValue;
      }
      return dumpingDesignationAt.Value.GetTargetHeightAt(position);
    }

    /// <summary>
    /// When designation is fulfilled, we wait little bit to make sure it stays this way.
    /// </summary>
    private DumpingJob.State handleWaitingForFulfilled()
    {
      if (this.StateChanged)
        this.m_timer.Start(new Duration(7));
      if (!this.m_designationToDump.Value.IsDumpingFulfilled)
        return DumpingJob.State.DecideDumpingOnReachableTile;
      return this.m_timer.Decrement() ? DumpingJob.State.WaitingForFulfilled : DumpingJob.State.CheckAllFulfilled;
    }

    /// <summary>
    /// Waits a little (for terrain physics) and moves all non-fulfilled designations from
    /// `m_fulfilledDesignations` to `m_extraDesignations` and promotes the first one to be the
    /// `m_primaryDesignation`.
    /// 
    /// Returns <see cref="F:Mafi.Core.Vehicles.Jobs.DumpingJob.State.Done" /> if all designations are fulfilled.
    /// </summary>
    private DumpingJob.State handleCheckAllFulfilled()
    {
      if (this.StateChanged)
      {
        if (this.m_designationToDump.HasValue)
          this.clearDesignationToDump();
        if (this.m_truck.IsEmpty)
          return DumpingJob.State.Done;
        this.m_timer.Start(new Duration(7));
        return DumpingJob.State.CheckAllFulfilled;
      }
      if (this.m_timer.Decrement())
        return DumpingJob.State.CheckAllFulfilled;
      this.filterDestroyedAndUnreachable(this.m_fulfilledDesignations);
      this.m_fulfilledDesignations.RemoveWhere((Predicate<TerrainDesignation>) (x => x.IsDumpingFulfilled));
      this.m_extraDesignations.AddRange(this.m_fulfilledDesignations);
      this.m_fulfilledDesignations.Clear();
      return this.trySelectPrimaryDesignation() ? DumpingJob.State.InitialPathFindingAndDesignationSelect : DumpingJob.State.FindMoreDesignations;
    }

    private DumpingJob.State handleFindMoreDesignations()
    {
      if (this.m_truck.IsEmpty)
      {
        Log.Warning("Truck is already empty.");
        return DumpingJob.State.Done;
      }
      RegisteredOutputBuffer buffer;
      if (!this.m_factory.VehicleLastOutputBufferManager.TryGetLastOutputBufferFor((Vehicle) this.m_truck, out buffer))
        return DumpingJob.State.Done;
      this.m_fulfilledDesignations.Clear();
      this.m_extraDesignations.Clear();
      this.clearDesignationToDump();
      return this.m_factory.DumpingManager.TryFindClosestReadyToDump(this.m_truck.GroundPositionTile2i, buffer, this.m_productToDump, this.m_truck, out this.m_primaryDesignation, this.m_extraDesignations) ? DumpingJob.State.InitialPathFindingAndDesignationSelect : DumpingJob.State.Done;
    }

    protected override Duration RequestCancelReturnDeadline()
    {
      this.cleanup();
      ((IVehicleFriend) this.m_truck).AlsoCancelAllOtherJobs((VehicleJob) this);
      return Duration.Zero;
    }

    protected override void OnDestroy()
    {
      this.cleanup();
      this.m_extraDesignations.Clear();
      this.m_tileToDump = new TerrainTile?();
      this.m_timer.Reset();
      this.m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.None;
    }

    static DumpingJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      DumpingJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      DumpingJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
    }

    private enum State
    {
      InitialPathFindingAndDesignationSelect,
      DrivingToDumpLocation,
      DecideDumpingOnReachableTile,
      Dumping,
      WaitingForFulfilled,
      Done,
      CheckAllFulfilled,
      FindMoreDesignations,
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly ITerrainDumpingManager DumpingManager;
      public readonly IVehiclePathFindingManager PathFindingManager;
      public readonly TerrainManager TerrainManager;
      public readonly TerrainDesignationVehicleGoal.Factory DesignationGoalFactory;
      public readonly TerrainDumpingManager TerrainDumpingManager;
      public readonly UnreachableTerrainDesignationsManager UnreachableDesignationsManager;
      public readonly VehicleJobStatsManager VehicleJobStatsManager;
      public readonly VehicleLastOutputBufferManager VehicleLastOutputBufferManager;
      private readonly Lyst<TerrainDesignation> m_extraDesignationsTmp;

      public Factory(
        VehicleJobId.Factory vehicleJobIdFactory,
        ITerrainDumpingManager dumpingManager,
        IVehiclePathFindingManager pathFindingManager,
        TerrainManager terrainManager,
        TerrainDesignationVehicleGoal.Factory designationGoalFactory,
        TerrainDumpingManager terrainDumpingManager,
        UnreachableTerrainDesignationsManager unreachableDesignationsManager,
        VehicleJobStatsManager vehicleJobStatsManager,
        VehicleLastOutputBufferManager vehicleLastOutputBufferManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_extraDesignationsTmp = new Lyst<TerrainDesignation>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.DumpingManager = dumpingManager;
        this.PathFindingManager = pathFindingManager;
        this.DesignationGoalFactory = designationGoalFactory;
        this.TerrainDumpingManager = terrainDumpingManager;
        this.UnreachableDesignationsManager = unreachableDesignationsManager;
        this.TerrainManager = terrainManager;
        this.VehicleJobStatsManager = vehicleJobStatsManager;
        this.VehicleLastOutputBufferManager = vehicleLastOutputBufferManager;
      }

      public DumpingJob EnqueueJob(
        Truck truck,
        LooseProductProto looseProductProto,
        TerrainDesignation designation,
        IEnumerable<TerrainDesignation> extraDesignations = null)
      {
        this.m_extraDesignationsTmp.Clear();
        return new DumpingJob(this.m_vehicleJobIdFactory.GetNextId(), this, truck, (Option<ProductProto>) (ProductProto) looseProductProto, designation, extraDesignations ?? (IEnumerable<TerrainDesignation>) this.m_extraDesignationsTmp);
      }

      public bool TryCreateAndEnqueueJob(
        Truck truck,
        Option<ProductProto> productProto,
        bool tryIgnoreReservations = false,
        Predicate<TerrainDesignation> predicate = null)
      {
        if (productProto.HasValue && productProto.Value.DumpableProduct.IsNone)
          return false;
        this.m_extraDesignationsTmp.Clear();
        TerrainDesignation bestDesignation;
        if (!this.TerrainDumpingManager.TryFindClosestReadyToDump(truck.Position2f.Tile2i, (Option<LooseProductProto>) productProto.ValueOrNull?.DumpableProduct.Value, truck, out bestDesignation, tryIgnoreReservations, predicate, this.m_extraDesignationsTmp))
          return false;
        DumpingJob dumpingJob = new DumpingJob(this.m_vehicleJobIdFactory.GetNextId(), this, truck, productProto, bestDesignation, (IEnumerable<TerrainDesignation>) this.m_extraDesignationsTmp);
        this.m_extraDesignationsTmp.Clear();
        return true;
      }

      public bool TryCreateAndEnqueueJob(
        Truck truck,
        ProductProto productProto,
        IReadOnlySet<IEntityAssignedAsInput> assignedEntities,
        bool tryIgnoreReservations = false)
      {
        if (productProto.DumpableProduct.IsNone)
          return false;
        this.m_extraDesignationsTmp.Clear();
        TerrainDesignation bestDesignation;
        if (!this.TerrainDumpingManager.TryFindClosestReadyToDump(truck.Position2f.Tile2i, (Option<LooseProductProto>) productProto.DumpableProduct.Value, truck, out bestDesignation, tryIgnoreReservations, (Predicate<TerrainDesignation>) (designation =>
        {
          foreach (IAreaManagingTower managedByTower in designation.ManagedByTowers)
          {
            if (managedByTower is MineTower mineTower2 && assignedEntities.Contains((IEntityAssignedAsInput) mineTower2))
              return true;
          }
          return false;
        }), this.m_extraDesignationsTmp))
          return false;
        DumpingJob dumpingJob = new DumpingJob(this.m_vehicleJobIdFactory.GetNextId(), this, truck, (Option<ProductProto>) productProto, bestDesignation, (IEnumerable<TerrainDesignation>) this.m_extraDesignationsTmp);
        this.m_extraDesignationsTmp.Clear();
        return true;
      }
    }
  }
}
