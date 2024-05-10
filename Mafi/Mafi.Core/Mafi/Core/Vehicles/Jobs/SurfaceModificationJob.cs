// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.SurfaceModificationJob
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.PathFinding;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job in which a vehicle modify surface of the terrain based on assigned surface designation.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class SurfaceModificationJob : 
    VehicleJob,
    IJobWithPreNavigation,
    IVehicleJobWithOwner,
    IVehicleJob,
    IVehicleJobReadOnly,
    IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly RelTile1f MAX_NAV_DISTANCE;
    private readonly TickTimer m_timer;
    private readonly Truck m_truck;
    private readonly Lyst<SurfaceDesignation> m_designations;
    private readonly Lyst<SurfaceDesignation> m_designationsTmp;
    private Option<SurfaceDesignation> m_designationToProcess;
    private TerrainTile? m_tileToProcess;
    private readonly bool m_isClearingJob;
    private SurfaceModificationJob.State m_state;
    private SurfaceModificationJob.State m_previousState;
    private Option<TerrainDesignationVehicleGoal> m_initialNavGoal;
    private ProductQuantity m_materialsExchanged;
    private readonly ProductProto m_product;
    private readonly SurfaceModificationJob.Factory m_factory;
    private readonly RobustNavHelper m_navHelper;
    [DoNotSave(0, null)]
    private Func<IDesignation, TerrainTile, RelTile2i, Fix64> m_targetTileCostFn;
    private long m_distanceOffset;

    public static void Serialize(SurfaceModificationJob value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SurfaceModificationJob>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SurfaceModificationJob.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Lyst<SurfaceDesignation>.Serialize(this.m_designations, writer);
      Lyst<SurfaceDesignation>.Serialize(this.m_designationsTmp, writer);
      Option<SurfaceDesignation>.Serialize(this.m_designationToProcess, writer);
      writer.WriteLong(this.m_distanceOffset);
      Option<TerrainDesignationVehicleGoal>.Serialize(this.m_initialNavGoal, writer);
      writer.WriteBool(this.m_isClearingJob);
      ProductQuantity.Serialize(this.m_materialsExchanged, writer);
      RobustNavHelper.Serialize(this.m_navHelper, writer);
      writer.WriteInt((int) this.m_previousState);
      writer.WriteGeneric<ProductProto>(this.m_product);
      writer.WriteInt((int) this.m_state);
      writer.WriteNullableStruct<TerrainTile>(this.m_tileToProcess);
      TickTimer.Serialize(this.m_timer, writer);
      Truck.Serialize(this.m_truck, writer);
    }

    public static SurfaceModificationJob Deserialize(BlobReader reader)
    {
      SurfaceModificationJob surfaceModificationJob;
      if (reader.TryStartClassDeserialization<SurfaceModificationJob>(out surfaceModificationJob))
        reader.EnqueueDataDeserialization((object) surfaceModificationJob, SurfaceModificationJob.s_deserializeDataDelayedAction);
      return surfaceModificationJob;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SurfaceModificationJob>(this, "m_designations", (object) Lyst<SurfaceDesignation>.Deserialize(reader));
      reader.SetField<SurfaceModificationJob>(this, "m_designationsTmp", (object) Lyst<SurfaceDesignation>.Deserialize(reader));
      this.m_designationToProcess = Option<SurfaceDesignation>.Deserialize(reader);
      this.m_distanceOffset = reader.ReadLong();
      reader.RegisterResolvedMember<SurfaceModificationJob>(this, "m_factory", typeof (SurfaceModificationJob.Factory), true);
      this.m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.Deserialize(reader);
      reader.SetField<SurfaceModificationJob>(this, "m_isClearingJob", (object) reader.ReadBool());
      this.m_materialsExchanged = ProductQuantity.Deserialize(reader);
      reader.SetField<SurfaceModificationJob>(this, "m_navHelper", (object) RobustNavHelper.Deserialize(reader));
      this.m_previousState = (SurfaceModificationJob.State) reader.ReadInt();
      reader.SetField<SurfaceModificationJob>(this, "m_product", (object) reader.ReadGenericAs<ProductProto>());
      this.m_state = (SurfaceModificationJob.State) reader.ReadInt();
      this.m_tileToProcess = reader.ReadNullableStruct<TerrainTile>();
      reader.SetField<SurfaceModificationJob>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      reader.SetField<SurfaceModificationJob>(this, "m_truck", (object) Truck.Deserialize(reader));
      reader.RegisterInitAfterLoad<SurfaceModificationJob>(this, "initSelf", InitPriority.Normal);
    }

    public override LocStrFormatted JobInfo
    {
      get
      {
        switch (this.m_state)
        {
          case SurfaceModificationJob.State.DesignationSelectionAndNavigation:
            Truck truck = this.m_truck;
            return (LocStrFormatted) ((truck != null ? (truck.IsDriving ? 1 : 0) : 0) != 0 ? TrCore.VehicleJob__DrivingToGoal : TrCore.VehicleJob__SearchingForDesignation);
          case SurfaceModificationJob.State.ProcessingSurface:
            return (LocStrFormatted) TrCore.VehicleJob__ProcessingSurface;
          default:
            return (LocStrFormatted) TrCore.VehicleJob__InvalidState;
        }
      }
    }

    public Vehicle Vehicle => (Vehicle) this.m_truck;

    public override bool IsTrueJob => true;

    public override VehicleFuelConsumption CurrentFuelConsumption
    {
      get
      {
        return this.m_state != SurfaceModificationJob.State.DesignationSelectionAndNavigation || this.m_truck.IsDriving ? VehicleFuelConsumption.Full : VehicleFuelConsumption.Idle;
      }
    }

    public LocStrFormatted GoalName => "Surface modification".ToDoTranslate();

    /// <summary>Whether the state changed last update.</summary>
    private bool StateChanged => this.m_state != this.m_previousState;

    private SurfaceModificationJob(
      bool isClearingJob,
      ProductProto product,
      VehicleJobId id,
      SurfaceModificationJob.Factory factory,
      Truck truck,
      IEnumerable<SurfaceDesignation> designations)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      this.m_designations = new Lyst<SurfaceDesignation>();
      this.m_designationsTmp = new Lyst<SurfaceDesignation>();
      this.m_materialsExchanged = ProductQuantity.None;
      // ISSUE: explicit constructor call
      base.\u002Ector(id);
      this.m_isClearingJob = isClearingJob;
      this.m_product = product;
      this.m_factory = factory.CheckNotNull<SurfaceModificationJob.Factory>();
      this.m_truck = truck.CheckNotNull<Truck>();
      this.m_navHelper = new RobustNavHelper(factory.PathFindingManager);
      this.initSelf();
      Assert.That<Option<SurfaceDesignation>>(this.m_designationToProcess).IsNone<SurfaceDesignation>();
      foreach (SurfaceDesignation designation in designations)
      {
        if (this.m_factory.SurfaceDesignationsManager.Reserve((IVehicleJobWithOwner) this, designation))
          this.m_designations.Add(designation);
      }
      this.m_truck.EnqueueJob((VehicleJob) this, false);
      this.m_previousState = SurfaceModificationJob.State.Done;
      this.m_state = SurfaceModificationJob.State.DesignationSelectionAndNavigation;
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.m_targetTileCostFn = new Func<IDesignation, TerrainTile, RelTile2i, Fix64>(this.targetTileCostFunction);
    }

    protected override bool DoJobInternal()
    {
      SurfaceModificationJob.State state = this.handleState();
      this.m_previousState = this.m_state;
      this.m_state = state;
      if (this.m_state != SurfaceModificationJob.State.Done)
        return true;
      if (this.m_isClearingJob && this.m_truck.Cargo.IsEmpty)
        ((IVehicleFriend) this.m_truck).CancelAllJobsExcept((VehicleJob) this);
      this.cleanup();
      return false;
    }

    private void clearDesignationToProcess()
    {
      if (this.m_designationToProcess.IsNone)
        return;
      SurfaceDesignation designation = this.m_designationToProcess.Value;
      this.m_designationToProcess = (Option<SurfaceDesignation>) Option.None;
      if (designation.IsDestroyed)
        return;
      this.m_factory.SurfaceDesignationsManager.RemoveReservation((IVehicleJobWithOwner) this, designation);
    }

    private void cleanup()
    {
      this.clearAllDesignations();
      this.m_navHelper.CancelAndClear();
      if (!this.m_materialsExchanged.IsNotEmpty)
        return;
      this.m_factory.VehicleJobStatsManager.RecordJobStatsFor(this.m_truck, this.m_materialsExchanged);
      this.m_materialsExchanged = ProductQuantity.None;
    }

    private void clearAllDesignations()
    {
      this.clearDesignationToProcess();
      foreach (SurfaceDesignation designation in this.m_designations)
      {
        if (!designation.IsDestroyed)
          this.m_factory.SurfaceDesignationsManager.RemoveReservation((IVehicleJobWithOwner) this, designation);
      }
      this.m_designations.Clear();
    }

    private SurfaceModificationJob.State handleState()
    {
      switch (this.m_state)
      {
        case SurfaceModificationJob.State.DesignationSelectionAndNavigation:
          return this.handleDesignationSelectionAndNavigation();
        case SurfaceModificationJob.State.ProcessingSurface:
          return this.handleProcessingSurface();
        case SurfaceModificationJob.State.Done:
          Log.Warning(string.Format("Already done! Previous state: {0}", (object) this.m_previousState));
          return SurfaceModificationJob.State.Done;
        default:
          Assert.Fail(string.Format("Unknown/invalid mining job state '{0}'.", (object) this.m_state));
          return SurfaceModificationJob.State.Done;
      }
    }

    private void pruneDesignations()
    {
      IReadOnlySet<IDesignation> unreachableDesignationsFor = this.m_factory.UnreachableDesignationsManager.GetUnreachableDesignationsFor((IPathFindingVehicle) this.m_truck);
      this.m_designationsTmp.Clear();
      foreach (SurfaceDesignation designation in this.m_designations)
      {
        if (!designation.IsDestroyed)
        {
          if (designation.IsFulfilled || unreachableDesignationsFor.Contains((IDesignation) designation))
            this.m_factory.SurfaceDesignationsManager.RemoveReservation((IVehicleJobWithOwner) this, designation);
          else
            this.m_designationsTmp.Add(designation);
        }
      }
      this.m_designations.Clear();
      this.m_designations.AddRange(this.m_designationsTmp);
      this.m_designationsTmp.Clear();
    }

    public RobustNavHelper StartNavigation()
    {
      this.pruneDesignations();
      if (this.m_designations.IsEmpty)
        return this.m_navHelper;
      TerrainDesignationVehicleGoal designationVehicleGoal = this.m_factory.DesignationGoalFactory.Create((IDesignation) this.m_designations.First, this.m_truck.Prototype.MinSurfaceProcessingDistance, (IEnumerable<IDesignation>) this.m_designations.Skip<SurfaceDesignation>(1));
      RobustNavHelper navHelper = this.m_navHelper;
      Truck truck = this.m_truck;
      TerrainDesignationVehicleGoal goal = designationVehicleGoal;
      RelTile1f? nullable = new RelTile1f?(SurfaceModificationJob.MAX_NAV_DISTANCE);
      RelTile1i? extraTolerancePerRetry = new RelTile1i?();
      RelTile1f? maxNavigateClosebyDistance = nullable;
      ThicknessTilesF? maxNavigateCloseByHeightDifference = new ThicknessTilesF?();
      navHelper.StartNavigationTo((Vehicle) truck, (IVehicleGoalFull) goal, extraTolerancePerRetry: extraTolerancePerRetry, navigateClosebyIsSufficient: true, maxNavigateClosebyDistance: maxNavigateClosebyDistance, maxNavigateCloseByHeightDifference: maxNavigateCloseByHeightDifference);
      this.m_initialNavGoal = (Option<TerrainDesignationVehicleGoal>) designationVehicleGoal;
      return this.m_navHelper;
    }

    private SurfaceModificationJob.State handleDesignationSelectionAndNavigation()
    {
      if (this.StateChanged)
      {
        Assert.That<Option<SurfaceDesignation>>(this.m_designationToProcess).IsNone<SurfaceDesignation>();
        if (!this.m_isClearingJob && this.m_truck.Cargo.GetQuantityOf(this.m_product).IsNotPositive || !this.m_isClearingJob && this.m_truck.IsEmpty)
          return SurfaceModificationJob.State.Done;
        this.pruneDesignations();
        if (this.m_designations.IsEmpty)
          return SurfaceModificationJob.State.Done;
        long num1 = long.MaxValue;
        foreach (SurfaceDesignation designation in this.m_designations)
        {
          long num2 = designation.CenterTileCoord.DistanceSqrTo(this.m_truck.GroundPositionTile2i);
          if (num2 < num1 && num2 <= this.m_distanceOffset + this.m_truck.Prototype.MaxSurfaceProcessingDistance.Squared)
          {
            this.m_designationToProcess = (Option<SurfaceDesignation>) designation;
            num1 = num2;
          }
        }
        if (this.m_designationToProcess.HasValue)
        {
          this.m_designations.Remove(this.m_designationToProcess.Value);
          return SurfaceModificationJob.State.ProcessingSurface;
        }
        if (!this.m_navHelper.IsNavigating)
          this.StartNavigation();
        return SurfaceModificationJob.State.DesignationSelectionAndNavigation;
      }
      if (this.m_designationToProcess.IsNone && this.m_initialNavGoal.Value.ActualGoalDesignation.HasValue)
      {
        this.pruneDesignations();
        if (!(this.m_initialNavGoal.Value.ActualGoalDesignation.Value is SurfaceDesignation surfaceDesignation))
          return SurfaceModificationJob.State.Done;
        this.m_designationToProcess = !this.m_designations.Contains(surfaceDesignation) ? (Option<SurfaceDesignation>) this.m_designations.FirstOrDefault<SurfaceDesignation>() : (Option<SurfaceDesignation>) surfaceDesignation;
        if (this.m_designationToProcess.IsNone)
          return SurfaceModificationJob.State.Done;
        this.m_designations.Remove(this.m_designationToProcess.Value);
        Assert.That<Option<SurfaceDesignation>>(this.m_designationToProcess).HasValue<SurfaceDesignation>();
        Assert.That<bool>(this.m_designationToProcess.Value.IsNotFulfilled).IsTrue();
        this.m_truck.DeactivateCannotDeliver();
      }
      Assert.That<Option<TerrainDesignationVehicleGoal>>(this.m_initialNavGoal).HasValue<TerrainDesignationVehicleGoal>();
      RobustNavResult robustNavResult = this.m_navHelper.StepNavigation();
      switch (robustNavResult)
      {
        case RobustNavResult.Navigating:
          return SurfaceModificationJob.State.DesignationSelectionAndNavigation;
        case RobustNavResult.GoalReachedSuccessfully:
          if (this.m_designationToProcess.IsNone)
          {
            Log.Error("Goal reached but designation was not set.");
            return SurfaceModificationJob.State.DesignationSelectionAndNavigation;
          }
          this.m_distanceOffset = this.m_designationToProcess.Value.CenterTileCoord.DistanceSqrTo(this.m_truck.GroundPositionTile2i);
          this.m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.None;
          return SurfaceModificationJob.State.ProcessingSurface;
        case RobustNavResult.FailGoalUnreachable:
          return SurfaceModificationJob.State.Done;
        default:
          Log.Error(string.Format("Invalid state: {0}", (object) robustNavResult));
          goto case RobustNavResult.FailGoalUnreachable;
      }
    }

    private bool isCurrentDesignationNotValid(out SurfaceModificationJob.State newState)
    {
      if (this.m_designationToProcess.IsNone)
      {
        newState = this.m_designations.IsEmpty ? SurfaceModificationJob.State.Done : SurfaceModificationJob.State.DesignationSelectionAndNavigation;
        return true;
      }
      SurfaceDesignation surfaceDesignation = this.m_designationToProcess.Value;
      if (surfaceDesignation.IsDestroyed || surfaceDesignation.IsFulfilled)
      {
        this.clearDesignationToProcess();
        newState = SurfaceModificationJob.State.DesignationSelectionAndNavigation;
        return true;
      }
      newState = SurfaceModificationJob.State.Done;
      return false;
    }

    private bool isTileEligible(TerrainTile tile)
    {
      SurfaceDesignation surfaceDesignation = this.m_designationToProcess.Value;
      if (surfaceDesignation.IsFulfilledAt(tile.TileCoord))
        return false;
      if (this.m_factory.TerrainManager.IsOffLimitsOrInvalid(tile.TileCoord))
      {
        Log.Error("Concreting to off-limits tile.");
        return false;
      }
      if (this.m_isClearingJob)
      {
        TileSurfaceData tileSurfaceData;
        if (!this.m_factory.TerrainManager.TryGetTileSurface(this.m_factory.TerrainManager[tile.TileCoord].DataIndex, out tileSurfaceData))
        {
          if (surfaceDesignation.IsPlacing)
            return false;
          Log.Error("Failed to find surface under clearing designation!");
          return false;
        }
        if (tileSurfaceData.IsAutoPlaced)
          return false;
        ProductQuantity costPerTile = tileSurfaceData.ResolveToProto(this.m_factory.TerrainManager).CostPerTile;
        if ((Proto) costPerTile.Product != (Proto) this.m_product || costPerTile.Quantity > this.m_truck.RemainingCapacity)
          return false;
      }
      else
      {
        ProductQuantity costPerTile = surfaceDesignation.GetSurfaceAt(tile.TileCoord).ResolveToProto(this.m_factory.TerrainManager).CostPerTile;
        TileSurfaceData tileSurfaceData;
        if (this.m_factory.TerrainManager.TryGetTileSurface(tile.DataIndex, out tileSurfaceData) && !tileSurfaceData.IsAutoPlaced || (Proto) costPerTile.Product != (Proto) this.m_product || costPerTile.Quantity > this.m_truck.Cargo.GetQuantityOf(this.m_product))
          return false;
      }
      return true;
    }

    private Fix64 targetTileCostFunction(
      IDesignation designation,
      TerrainTile tile,
      RelTile2i coord)
    {
      return tile.CenterTile2f.DistanceSqrTo(this.m_truck.Position2f);
    }

    private Duration getProcessingDuration()
    {
      return this.m_truck.SpeedFactor.ApplyInverseFloored(this.m_truck.Prototype.DumpIterationDuration.Ticks).Ticks();
    }

    private SurfaceModificationJob.State handleProcessingSurface()
    {
      Assert.That<bool>(this.m_truck.IsDriving).IsFalse();
      SurfaceModificationJob.State newState;
      if (this.isCurrentDesignationNotValid(out newState))
      {
        this.m_tileToProcess = new TerrainTile?();
        return newState;
      }
      SurfaceDesignation designation = this.m_designationToProcess.Value;
      if (!this.m_tileToProcess.HasValue)
      {
        Tile2i? fulfilledTileCoord = designation.FindBestNonFulfilledTileCoord<Fix64>(this.m_targetTileCostFn, (Option<Predicate<TerrainTile>>) new Predicate<TerrainTile>(this.isTileEligible), out Fix64 _);
        if (!fulfilledTileCoord.HasValue)
        {
          this.clearDesignationToProcess();
          return SurfaceModificationJob.State.DesignationSelectionAndNavigation;
        }
        Assert.That<bool>(designation.IsFulfilledAt(fulfilledTileCoord.Value)).IsFalse("Already fulfilled?");
        this.m_tileToProcess = new TerrainTile?(this.m_factory.TerrainManager[fulfilledTileCoord.Value]);
        this.m_timer.Start(this.getProcessingDuration());
        return SurfaceModificationJob.State.ProcessingSurface;
      }
      if (this.m_timer.Decrement())
        return SurfaceModificationJob.State.ProcessingSurface;
      TerrainTile terrainTile = this.m_tileToProcess.Value;
      if (this.m_isClearingJob)
      {
        TileSurfaceData tileSurfaceData;
        if (!this.m_factory.TerrainManager.TryGetTileSurface(terrainTile.DataIndex, out tileSurfaceData))
        {
          this.clearDesignationToProcess();
          return SurfaceModificationJob.State.DesignationSelectionAndNavigation;
        }
        TerrainTileSurfaceProto proto1 = tileSurfaceData.ResolveToProto(this.m_factory.TerrainManager);
        bool placedAutoSurface;
        this.m_factory.SurfaceDesignationsManager.ClearCustomSurface(terrainTile.CoordAndIndex, out placedAutoSurface);
        ProductQuantity pq = proto1.CostPerTile;
        if (designation.IsPlacing && !placedAutoSurface)
        {
          TerrainTileSurfaceProto proto2 = designation.GetSurfaceAt(terrainTile.TileCoord).ResolveToProto(this.m_factory.TerrainManager);
          if (pq == proto2.CostPerTile && this.m_factory.SurfaceDesignationsManager.TryAddSurface(terrainTile.CoordAndIndex, designation))
            pq = ProductQuantity.None;
        }
        if (pq.IsNotEmpty)
        {
          this.m_truck.AddProductFromSurface(pq);
          this.m_materialsExchanged = pq.Product.WithQuantity(this.m_materialsExchanged.Quantity + pq.Quantity);
        }
      }
      else
      {
        ProductQuantity costPerTile = this.m_factory.TerrainManager.ResolveSlimSurface(designation.GetSurfaceAt(terrainTile.TileCoord)).CostPerTile;
        if (this.m_factory.SurfaceDesignationsManager.TryAddSurface(terrainTile.CoordAndIndex, designation))
        {
          this.m_truck.RemoveProductForSurface(costPerTile);
          this.m_materialsExchanged = costPerTile.Product.WithQuantity(this.m_materialsExchanged.Quantity + costPerTile.Quantity);
        }
      }
      this.m_tileToProcess = new TerrainTile?();
      return this.isCurrentDesignationNotValid(out newState) ? newState : SurfaceModificationJob.State.ProcessingSurface;
    }

    public bool CancelCurrentSurface(SurfaceDesignation designation)
    {
      if (this.m_designationToProcess != designation)
        return false;
      this.clearDesignationToProcess();
      return true;
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
      this.m_designations.Clear();
      this.m_tileToProcess = new TerrainTile?();
      this.m_timer.Reset();
      this.m_initialNavGoal = Option<TerrainDesignationVehicleGoal>.None;
    }

    static SurfaceModificationJob()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SurfaceModificationJob.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleJob) obj).SerializeData(writer));
      SurfaceModificationJob.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleJob) obj).DeserializeData(reader));
      SurfaceModificationJob.MAX_NAV_DISTANCE = 32.0.Tiles();
    }

    private enum State
    {
      DesignationSelectionAndNavigation,
      ProcessingSurface,
      Done,
    }

    [GlobalDependency(RegistrationMode.AsSelf, false, false)]
    public class Factory
    {
      private readonly VehicleJobId.Factory m_vehicleJobIdFactory;
      public readonly IVehiclePathFindingManager PathFindingManager;
      public readonly TerrainManager TerrainManager;
      public readonly TerrainDesignationVehicleGoal.Factory DesignationGoalFactory;
      public readonly SurfaceDesignationsManager SurfaceDesignationsManager;
      public readonly UnreachableTerrainDesignationsManager UnreachableDesignationsManager;
      public readonly VehicleJobStatsManager VehicleJobStatsManager;
      private readonly Lyst<SurfaceDesignation> m_designationsTmp;

      public Factory(
        VehicleJobId.Factory vehicleJobIdFactory,
        IVehiclePathFindingManager pathFindingManager,
        TerrainManager terrainManager,
        TerrainDesignationVehicleGoal.Factory designationGoalFactory,
        SurfaceDesignationsManager surfaceDesignationsManager,
        UnreachableTerrainDesignationsManager unreachableDesignationsManager,
        VehicleJobStatsManager vehicleJobStatsManager)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.m_designationsTmp = new Lyst<SurfaceDesignation>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_vehicleJobIdFactory = vehicleJobIdFactory;
        this.PathFindingManager = pathFindingManager;
        this.DesignationGoalFactory = designationGoalFactory;
        this.SurfaceDesignationsManager = surfaceDesignationsManager;
        this.UnreachableDesignationsManager = unreachableDesignationsManager;
        this.TerrainManager = terrainManager;
        this.VehicleJobStatsManager = vehicleJobStatsManager;
      }

      public SurfaceModificationJob EnqueueJob(
        bool isClearingJob,
        ProductQuantity productQuantity,
        Truck truck,
        IEnumerable<SurfaceDesignation> designations)
      {
        return new SurfaceModificationJob(isClearingJob, productQuantity.Product, this.m_vehicleJobIdFactory.GetNextId(), this, truck, designations);
      }

      public bool TryCreateAndEnqueuePlacementJob(ProductProto product, Truck truck)
      {
        DesignationsPerProductCache data;
        if (!this.SurfaceDesignationsManager.TryGetDataFor(product, out data) || data.LeftToPlace.IsNotPositive)
          return false;
        Quantity quantityOf = truck.Cargo.GetQuantityOf(product);
        if (quantityOf.IsNotPositive)
          return false;
        this.m_designationsTmp.Clear();
        if (!this.SurfaceDesignationsManager.TryFindClosestReadyToPlace(product, truck.Position2f.Tile2i, truck, quantityOf, this.m_designationsTmp, out Quantity _))
          return false;
        SurfaceModificationJob surfaceModificationJob = new SurfaceModificationJob(false, product, this.m_vehicleJobIdFactory.GetNextId(), this, truck, (IEnumerable<SurfaceDesignation>) this.m_designationsTmp);
        this.m_designationsTmp.Clear();
        return true;
      }
    }
  }
}
