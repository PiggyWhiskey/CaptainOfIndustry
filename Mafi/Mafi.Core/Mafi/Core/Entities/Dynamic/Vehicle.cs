// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Dynamic.Vehicle
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.PathFinding;
using Mafi.Core.Population;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Mafi.Core.Entities.Dynamic
{
  /// <summary>A vehicle that handles jobs.</summary>
  public abstract class Vehicle : 
    PathFindingEntity,
    IVehicleFriend,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IMaintainedEntity,
    IAssignedVehicleEntityFriend,
    IEntityWithCustomTitle,
    IEntityWithGeneralPriorityFriend
  {
    private static readonly Duration MAX_NO_MOVEMENT_DURATION;
    private static readonly int NOTIFY_FUEL_AFTER_TICKS;
    private readonly Option<Mafi.Core.Entities.Dynamic.FuelTank> m_fuelTank;
    public static readonly Percent SPEED_WHEN_BROKEN;
    public static readonly Percent SPEED_ON_LOW_FUEL;
    [DoNotSave(0, null)]
    private bool m_slowDownOnLowFuel;
    /// <summary>
    /// Whether this vehicle has to go for fuel on its own. Used only for vehicles that fail to get a refueling truck
    /// assigned to them.
    /// </summary>
    [DoNotSave(140, null)]
    public bool FailedToRequestFuelTruck;
    /// <summary>
    /// Whether this vehicle has to go for fuel on its own. Used only for vehicles that fail to get a refueling truck
    /// assigned to them.
    /// </summary>
    [NewInSaveVersion(140, null, null, null, null)]
    public RefuelRequestIssue LastRefuelRequestIssue;
    /// <summary>
    /// The depot at which this vehicle is scheduled to be replaced at.
    /// </summary>
    private Option<VehicleDepotBase> m_replaceQueuedDepot;
    private EntityNotificator m_noFuelNotif;
    [NewInSaveVersion(165, null, null, null, null)]
    private int m_ticksWithNoFuel;
    protected readonly GetUnstuckJob.Factory m_unstuckJobFactory;
    private int m_stateHash;
    private int m_stateHashSameCounter;
    [NewInSaveVersion(140, null, null, null, null)]
    private TileSurfaceSlimId? m_currentSurfaceSlimId;
    /// <summary>
    /// If HasValue, vehicle will try to request refueling from fuel stations.
    /// </summary>
    private Option<IFuelStationsManager> m_fuelStationsManager;

    public override bool CanBePaused => true;

    public Option<string> CustomTitle { get; set; }

    /// <summary>Entity to which is this vehicle assigned.</summary>
    public Option<IEntityAssignedWithVehicles> AssignedTo { get; private set; }

    /// <summary>
    /// Whether this vehicle is idle and can accept a new job. This is when it is idle in the depot or returning to
    /// the depot. This value is false when <see cref="P:Mafi.Core.Entities.Dynamic.Vehicle.AssignedTo" /> is has value.
    /// </summary>
    public virtual bool NeedsJob => this.AssignedTo.IsNone && !this.Jobs.ContainsTrueJob;

    public bool NeedsRefueling
    {
      get
      {
        Mafi.Core.Entities.Dynamic.FuelTank valueOrNull = this.m_fuelTank.ValueOrNull;
        return valueOrNull != null && valueOrNull.IsUnderReserve;
      }
    }

    public bool IsFuelTankEmpty
    {
      get
      {
        Mafi.Core.Entities.Dynamic.FuelTank valueOrNull = this.m_fuelTank.ValueOrNull;
        return valueOrNull != null && valueOrNull.IsEmpty;
      }
    }

    public bool CannotWorkDueToLowFuel => this.IsFuelTankEmpty && !this.CanRunWithNoFuel;

    public bool CanRunWithNoFuel => this.m_slowDownOnLowFuel;

    public Option<IFuelTankReadonly> FuelTank => this.m_fuelTank.As<IFuelTankReadonly>();

    /// <summary>Jobs for the vehicle.</summary>
    /// g
    protected VehicleJobsSequence Jobs { get; private set; }

    public int JobsCount => this.Jobs.Count;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public override bool IsEngineOn => this.HasJobs;

    public bool IsOnWayToDepotForScrap { get; private set; }

    public bool IsOnWayToDepotForReplacement { get; private set; }

    /// <summary>
    /// The replacement for this vehicle, only valid if replacement was scheduled.
    /// </summary>
    public Option<DrivingEntityProto> ReplacementProto { get; private set; }

    /// <summary>
    /// Whether this vehicle is queued for replacement. It can continue working
    /// until that replacement is spawned.
    /// </summary>
    public bool ReplaceQueued => this.m_replaceQueuedDepot.HasValue;

    public virtual bool CanBeAssigned
    {
      get
      {
        return this.AssignedTo.IsNone && !this.IsOnWayToDepotForScrap && !this.IsOnWayToDepotForReplacement;
      }
    }

    public bool HasJobs => this.Jobs.IsNotEmpty;

    public bool HasTrueJob => this.Jobs.ContainsTrueJob;

    public Option<IVehicleJobReadOnly> CurrentJob
    {
      get
      {
        return !this.Jobs.IsEmpty ? Option<IVehicleJobReadOnly>.Some((IVehicleJobReadOnly) this.Jobs.CurrentJob.Value) : Option<IVehicleJobReadOnly>.None;
      }
    }

    public virtual bool IsIdle => !this.HasTrueJob;

    public LocStrFormatted CurrentJobInfo
    {
      get => !this.Jobs.IsEmpty ? this.Jobs.CurrentJob.Value.JobInfo : LocStrFormatted.Empty;
    }

    protected bool HasFuelNotificationOn => this.m_noFuelNotif.IsActive;

    /// <summary>
    /// Vehicle is stuck when it is located on occupied (non-pathable) tiles. Vehicle will try to unstuck itself
    /// with <see cref="T:Mafi.Core.Vehicles.Jobs.GetUnstuckJob" />.
    /// </summary>
    public bool IsStuck
    {
      get
      {
        return this.PfState == PathFindingEntityState.DrivingToValidLocation || this.PfState == PathFindingEntityState.FindingValidLocation;
      }
    }

    bool IMaintainedEntity.IsIdleForMaintenance
    {
      get
      {
        return this.Jobs.CurrentJob.IsNone || this.Jobs.CurrentJob.Value.CurrentFuelConsumption != VehicleFuelConsumption.Full;
      }
    }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public int GeneralPriority { get; private set; }

    public virtual bool IsCargoAffectedByGeneralPriority => false;

    public virtual bool IsGeneralPriorityVisible => this.IsPriorityVisibleByDefault();

    void IEntityWithGeneralPriorityFriend.SetGeneralPriorityInternal(int priority)
    {
      this.GeneralPriority = priority;
      this.NotifyOnGeneralPriorityChange();
    }

    protected Vehicle(
      EntityId id,
      DrivingEntityProto prototype,
      EntityContext context,
      TerrainManager terrain,
      IVehiclePathFindingManager pathFindingManager,
      IVehiclesManager vehiclesManager,
      IVehicleSurfaceProvider surfaceProvider,
      GetUnstuckJob.Factory unstuckJobFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_replaceQueuedDepot = Option<VehicleDepotBase>.None;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, prototype, context, Tile2f.Zero, terrain, pathFindingManager, vehiclesManager, surfaceProvider);
      this.m_unstuckJobFactory = unstuckJobFactory;
      this.updateProperties();
      this.GeneralPriority = prototype.Costs.DefaultPriority;
      this.Jobs = new VehicleJobsSequence(this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.m_noFuelNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.VehicleNoFuel);
      if (prototype.FuelTankProto.HasValue)
      {
        this.m_fuelTank = (Option<Mafi.Core.Entities.Dynamic.FuelTank>) new Mafi.Core.Entities.Dynamic.FuelTank(prototype, context.PropertiesDb, context.AirPollutionManager, context.ProductsManager);
        this.m_fuelTank.Value.FillWithFirstProduct();
      }
      else
        this.m_fuelTank = Option<Mafi.Core.Entities.Dynamic.FuelTank>.None;
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      this.updateProperties();
      if (saveVersion < 140)
        this.CancelAllJobsAndResetState();
      if (saveVersion < 140 && this.FailedToRequestFuelTruck)
        this.LastRefuelRequestIssue = RefuelRequestIssue.Failed;
      if (saveVersion != 162 || !this.m_fuelStationsManager.HasValue)
        return;
      Mafi.Core.Entities.Dynamic.FuelTank valueOrNull = this.m_fuelTank.ValueOrNull;
      if ((valueOrNull != null ? (valueOrNull.IsUnderReserve ? 1 : 0) : 0) == 0)
        return;
      this.LastRefuelRequestIssue = RefuelRequestIssue.Failed;
    }

    private void updateProperties()
    {
      this.m_slowDownOnLowFuel = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.VehicleSlowDownOnLowFuel);
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    /// <summary>
    /// If set, vehicle will try to request refueling from fuel stations.
    /// </summary>
    protected void SetToAutoRequestRefuelingTrucks(IFuelStationsManager fuelStationManager)
    {
      this.m_fuelStationsManager = fuelStationManager.CreateOption<IFuelStationsManager>();
    }

    public void ConsumeFuelPerUpdate(VehicleFuelConsumption? consumption = null)
    {
      if (this.m_fuelTank.IsNone)
        return;
      Mafi.Core.Entities.Dynamic.FuelTank fuelTank = this.m_fuelTank.Value;
      bool isUnderReserve = fuelTank.IsUnderReserve;
      fuelTank.ConsumeFuelPerUpdate((VehicleFuelConsumption) ((int) consumption ?? (int) this.Jobs.CurrentFuelConsumption));
      if (this.m_fuelStationsManager.HasValue & (isUnderReserve != fuelTank.IsUnderReserve && fuelTank.IsUnderReserve))
        this.LastRefuelRequestIssue = this.m_fuelStationsManager.Value.TryRequestTruckForRefueling(this) ? RefuelRequestIssue.None : RefuelRequestIssue.Failed;
      if (!fuelTank.IsEmpty)
        return;
      this.updateSpeedFactor();
    }

    public void AddFuelExactly(ProductQuantity fuel)
    {
      Mafi.Assert.That<Quantity>(this.AddFuelAsMuchAs(fuel)).IsZero();
    }

    /// <returns>Excessive fuel that we could not add.</returns>
    public Quantity AddFuelAsMuchAs(ProductQuantity fuel)
    {
      if (this.m_fuelTank.IsNone)
        return fuel.Quantity;
      Quantity quantity = this.m_fuelTank.Value.AddFuelAsMuchAs(fuel);
      if (!this.NeedsRefueling)
      {
        this.LastRefuelRequestIssue = RefuelRequestIssue.None;
        this.m_fuelStationsManager.ValueOrNull?.VehicleRefueledByTruck(this);
      }
      this.updateSpeedFactor();
      return quantity;
    }

    private void updateSpeedFactor()
    {
      Percent speedFactor = 100.Percent();
      if (this.CanRunWithNoFuel)
      {
        Mafi.Core.Entities.Dynamic.FuelTank valueOrNull = this.m_fuelTank.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.IsEmpty ? 1 : 0) : 0) != 0)
          speedFactor = speedFactor.ScaleBy(Vehicle.SPEED_ON_LOW_FUEL);
      }
      if (this.Maintenance.ShouldSlowDown())
        speedFactor = speedFactor.ScaleBy(Vehicle.SPEED_WHEN_BROKEN);
      this.SetSpeedFactor(speedFactor);
    }

    public virtual LocStrFormatted GetSlowDownMessageForUi()
    {
      if (this.SpeedFactor >= Percent.Hundred)
        return LocStrFormatted.Empty;
      string str = TrCore.SpeedReduced__Vehicle.Format(this.SpeedFactor.ToStringRounded()).Value;
      if (this.CanRunWithNoFuel)
      {
        Mafi.Core.Entities.Dynamic.FuelTank fuelTank = this.m_fuelTank.Value;
        if ((fuelTank != null ? (fuelTank.IsEmpty ? 1 : 0) : 0) != 0)
          str += string.Format("\n- {0}", (object) TrCore.EntityStatus__NeedsFuel);
      }
      if (this.Maintenance.ShouldSlowDown())
        str += string.Format("\n- {0}", (object) TrCore.EntityStatus__Broken);
      return new LocStrFormatted(str);
    }

    protected override void SimUpdateInternal()
    {
      Mafi.Assert.That<bool>(this.IsSpawned || this.HasJobs).IsTrue("Updating non-spawned entity.");
      if (this.m_ticksWithNoFuel > 0 && this.Jobs.ContainsJobOfType<RefuelSelfJob>())
        this.m_ticksWithNoFuel = 0;
      else if (this.IsEnabled && this.IsFuelTankEmpty && (this.m_noFuelNotif.IsActive || !this.HasJobs))
        ++this.m_ticksWithNoFuel;
      else
        this.m_ticksWithNoFuel = 0;
      this.m_noFuelNotif.NotifyIff(this.m_ticksWithNoFuel > Vehicle.NOTIFY_FUEL_AFTER_TICKS, (IEntity) this);
      if (!this.IsEnabled || Entity.IsMissingWorkers((IEntityWithWorkers) this))
      {
        if (this.HasJobs)
          this.CancelAllJobsAndResetState();
        base.SimUpdateInternal();
      }
      else
      {
        TileSurfaceData tileSurfaceData;
        if (this.Terrain.TryGetTileSurface(this.Terrain.GetTileIndex(this.GroundPositionTile2i), out tileSurfaceData))
        {
          TileSurfaceSlimId surfaceSlimId = tileSurfaceData.SurfaceSlimId;
          TileSurfaceSlimId? currentSurfaceSlimId = this.m_currentSurfaceSlimId;
          if ((currentSurfaceSlimId.HasValue ? (surfaceSlimId != currentSurfaceSlimId.GetValueOrDefault() ? 1 : 0) : 1) != 0)
          {
            TerrainTileSurfaceProto tileSurfaceProto = tileSurfaceData.SurfaceSlimId.AsProtoOrPhantom(this.Terrain);
            Mafi.Assert.That<TerrainTileSurfaceProto>(tileSurfaceProto).IsNotNullOrPhantom<TerrainTileSurfaceProto>();
            this.Maintenance.SetDynamicExtraMultiplier(tileSurfaceProto.MaintenanceScale);
            this.m_currentSurfaceSlimId = new TileSurfaceSlimId?(tileSurfaceData.SurfaceSlimId);
          }
        }
        else if (this.m_currentSurfaceSlimId.HasValue)
        {
          this.Maintenance.SetDynamicExtraMultiplier(Percent.Hundred);
          this.m_currentSurfaceSlimId = new TileSurfaceSlimId?();
        }
        if (!this.HasJobs)
        {
          if (this.IsOnWayToDepotForReplacement && this.ReplacementProto.HasValue)
            this.TryRequestToGoToDepotForReplacement(this.ReplacementProto.Value);
          else if (this.IsOnWayToDepotForScrap)
            this.TryRequestScrap();
        }
        int stateHash = this.GetStateHash();
        if (this.m_stateHash == stateHash && !this.SkipNoMovementMonitoring())
        {
          ++this.m_stateHashSameCounter;
          if (this.m_stateHashSameCounter == Vehicle.MAX_NO_MOVEMENT_DURATION.Ticks - 10)
          {
            if (DebugGameRendererConfig.SaveVehicleIdleTooLong)
              DebugGameRenderer.StartMapDrawing(this.GroundPositionTile2i.AddXy(-20), new RelTile2i(40, 40)).DrawTilesTicks().DrawTerrain().DrawTrees().DrawPathabilityOverlayFor((PathFindingEntity) this).DrawAllDesignations().DrawUnreachableDesignationsFor(this).DrawAllStaticEntities().DrawAllDynamicEntities().DrawDynamicEntity((DynamicGroundEntity) this, ColorRgba.White).SaveMapAsTga(string.Format("Vehicle {0} is idle too long", (object) this));
            Mafi.Log.Info(string.Format("Vehicle {0} seems to be idle too long: Early warning.", (object) this));
          }
          if (this.m_stateHashSameCounter > Vehicle.MAX_NO_MOVEMENT_DURATION.Ticks)
          {
            Mafi.Log.Error(string.Format("Vehicle {0} seems to be idle for more than ", (object) this) + Vehicle.MAX_NO_MOVEMENT_DURATION.Seconds.ToStringRounded(0) + " seconds. Canceling all its jobs.\n" + string.Format("Position: {0}\n", (object) this.Position2f) + string.Format("Rotation: {0}\n", (object) this.Direction) + string.Format("Job: {0}\n", (object) this.CurrentJobInfo) + "  " + this.Jobs.AllJobs.Select<IVehicleJobReadOnly, string>((Func<IVehicleJobReadOnly, string>) (x => string.Format("{0} ({1})", (object) x, (object) x.GetType().Name))).JoinStrings("\n  ") + "\n" + string.Format("Driving state: {0}\n", (object) this.DrivingState) + string.Format("PF state: {0}\n", (object) this.PfState) + string.Format("Assignee: {0}\n", (object) this.AssignedTo) + string.Format("NeedsRefueling: {0}\n", (object) this.NeedsRefueling) + string.Format("NeedsToGoForFuel: {0}\n", (object) this.LastRefuelRequestIssue) + string.Format("HasWorker: {0}\n", (object) ((IEntityWithWorkers) this).HasWorkersCached) + string.Format("ScrapPending: {0}\n", (object) this.IsOnWayToDepotForScrap) + string.Format("UpgradePending: {0}", (object) this.IsOnWayToDepotForReplacement));
            this.m_stateHashSameCounter = 0;
            this.CancelAllJobsAndResetState();
          }
        }
        else
        {
          this.m_stateHash = stateHash;
          this.m_stateHashSameCounter = 0;
        }
        if (!this.HasJobs && !this.IsPathable(this.GroundPositionTile2i))
          this.m_unstuckJobFactory.EnqueueJob(this);
        base.SimUpdateInternal();
      }
    }

    protected override void OnDestroy()
    {
      Mafi.Assert.That<bool>(this.IsSpawned).IsFalse("Destroying spawned entity.");
      this.Jobs.CancelAllAndClear();
      if (this.AssignedTo.HasValue)
      {
        Mafi.Log.Warning(string.Format("Destroying vehicle '{0}' assigned to '{1}'.", (object) this, (object) this.AssignedTo.Value));
        this.UnassignFrom(this.AssignedTo.Value);
      }
      base.OnDestroy();
    }

    public virtual void EnqueueJob(VehicleJob job, bool enqueueFirst = false)
    {
      if (enqueueFirst)
        this.Jobs.AddFirst(job);
      else
        this.Jobs.EnqueueJob(job);
    }

    protected bool DoJob()
    {
      if (this.IsOnWayToDepotForScrap && this.Jobs.IsEmpty)
      {
        Mafi.Log.Warning("Scrap pending but no jobs.");
        this.IsOnWayToDepotForScrap = false;
        return false;
      }
      if (!this.IsOnWayToDepotForReplacement || !this.Jobs.IsEmpty)
        return this.Jobs.DoJob();
      Mafi.Log.Warning("Upgrade pending but no jobs.");
      this.IsOnWayToDepotForReplacement = false;
      return false;
    }

    /// <summary>
    /// Cancels all ongoing jobs and assigns this entity to given assignee.
    /// DANGER: This method is not bi-directional, it will not assign this vehicle on the owner's side
    /// </summary>
    void IAssignedVehicleEntityFriend.AssignTo(
      IEntityAssignedWithVehicles owner,
      bool doNotCancelJobs)
    {
      this.OnAssignTo(owner, doNotCancelJobs);
    }

    protected virtual void OnAssignTo(IEntityAssignedWithVehicles owner, bool doNotCancelJobs)
    {
      if (this.AssignedTo.HasValue)
      {
        Mafi.Assert.Fail("Assigning when already assigned.");
        this.UnassignFrom(this.AssignedTo.Value);
      }
      Mafi.Assert.That<Option<IEntityAssignedWithVehicles>>(this.AssignedTo).IsNone<IEntityAssignedWithVehicles>();
      if (!doNotCancelJobs)
        this.Jobs.CancelAll();
      this.AssignedTo = Option.Some<IEntityAssignedWithVehicles>(owner);
    }

    /// <summary>
    /// Unassigns this vehicle from given assignee and optionally cancels all ongoing jobs.
    /// </summary>
    public virtual void UnassignFrom(IEntityAssignedWithVehicles owner, bool cancelJobs = true)
    {
      if (this.AssignedTo.IsNone)
      {
        Mafi.Assert.Fail("Un-assigning when already unassigned.");
      }
      else
      {
        Mafi.Assert.That<Option<IEntityAssignedWithVehicles>>(this.AssignedTo).IsEqualTo<IEntityAssignedWithVehicles>(owner);
        if (cancelJobs)
          this.Jobs.CancelAll();
        this.AssignedTo.Value.UnassignVehicle(this, cancelJobs);
        this.AssignedTo = (Option<IEntityAssignedWithVehicles>) Option.None;
      }
    }

    public void CancelAllJobsAndResetState()
    {
      Option<IVehicleJob> currentJob = this.Jobs.CurrentJob;
      this.Jobs.CancelAll();
      if (!this.IsNavigating || this.HasJobs)
        return;
      Mafi.Log.Warning(string.Format("Vehicle '{0}' has no jobs but is navigating. Some job did not cancel navigation? First job was '{1}'.", (object) this, (object) currentJob));
      this.StopNavigating();
    }

    void IVehicleFriend.AlsoCancelAllOtherJobs(VehicleJob caller)
    {
      this.Jobs.CancelAllExcept(caller);
    }

    void IVehicleFriend.CancelAllJobsExcept(VehicleJob job) => this.Jobs.CancelAllExcept(job);

    public override void Spawn(Tile2f position, AngleDegrees1f direction)
    {
      base.Spawn(position, direction);
      this.VehiclesManager.VehicleSpawned(this);
    }

    /// <summary>
    /// Cancels all jobs and requests scrapping of this vehicle.
    /// </summary>
    public bool TryRequestScrap()
    {
      if (!this.TryRequestScrapInternal())
        return false;
      if (this.AssignedTo.HasValue)
        this.UnassignFrom(this.AssignedTo.Value);
      this.Jobs.CancelAll();
      this.IsOnWayToDepotForScrap = this.VehiclesManager.TryEnqueueScrapJob(this);
      if (this.IsOnWayToDepotForScrap && this.ReplaceQueued)
        this.CancelReplaceQueue(true);
      return this.IsOnWayToDepotForScrap;
    }

    /// <summary>
    /// Tries to scrap vehicle and returns if it was possible. If this function returned false, state of the vehicle
    /// should not change. Make sure to always test the result of `base.TryRequestScrap()` before performing any
    /// operations.
    /// </summary>
    protected virtual bool TryRequestScrapInternal()
    {
      if (!this.IsDestroyed)
        return true;
      Mafi.Log.Error(string.Format("Trying to scrap destroyed vehicle `{0}`", (object) this));
      return false;
    }

    /// <summary>Requests replacement of this vehicle to be queued.</summary>
    public bool TryToRequestConstructionOfReplacement(DrivingEntityProto newProto)
    {
      if (this.IsDestroyed)
      {
        Mafi.Log.Error(string.Format("Trying to replace destroyed vehicle `{0}`", (object) this));
        return false;
      }
      VehicleDepotBase closestDepot;
      if (!this.VehiclesManager.TryGetClosestDepotForReplacement(this, newProto, out closestDepot))
      {
        Mafi.Log.Warning(string.Format("No valid depot found for '{0}'", (object) this));
        return false;
      }
      if (!closestDepot.TryAddVehicleToReplaceQueue(this, newProto))
      {
        Mafi.Log.Warning(string.Format("Depot failed to add vehicle to replace queue '{0}' to '{1}'", (object) this, (object) newProto));
        return false;
      }
      this.m_replaceQueuedDepot = (Option<VehicleDepotBase>) closestDepot;
      this.ReplacementProto = (Option<DrivingEntityProto>) newProto;
      return true;
    }

    /// <summary>
    /// Cancels all jobs and requests immediate replacement of this vehicle.
    /// </summary>
    public virtual bool TryRequestToGoToDepotForReplacement(DrivingEntityProto targetProto)
    {
      if (this.IsDestroyed)
      {
        Mafi.Log.Error(string.Format("Trying to replace destroyed vehicle `{0}`", (object) this));
        return false;
      }
      this.Jobs.CancelAll();
      this.IsOnWayToDepotForReplacement = this.VehiclesManager.TryEnqueueReplaceJob(this, targetProto);
      if (this.IsOnWayToDepotForReplacement && this.ReplaceQueued)
      {
        Option<DrivingEntityProto> replacementProto = this.ReplacementProto;
        this.CancelReplaceQueue(false);
        this.ReplacementProto = replacementProto;
      }
      return this.IsOnWayToDepotForReplacement;
    }

    public virtual bool TryRequestRecovery()
    {
      Mafi.Assert.That<bool>(this.IsDestroyed).IsFalse();
      if (this.IsOnWayToDepotForScrap || this.IsOnWayToDepotForReplacement)
        return false;
      this.Jobs.CancelAllAndClear();
      if (!this.VehiclesManager.TryEnqueueRecoveryJob(this))
        return false;
      if (this.ReplaceQueued)
        this.CancelReplaceQueue(true);
      return true;
    }

    /// <summary>Cancels all jobs and cancels scrap request.</summary>
    public void CancelScrap()
    {
      Mafi.Assert.That<bool>(this.IsDestroyed).IsFalse();
      this.Jobs.CancelAll();
      this.IsOnWayToDepotForScrap = false;
    }

    /// <summary>Cancels all jobs and cancels replace request.</summary>
    public bool CancelReplace(bool updateDepot)
    {
      if (this.IsOnWayToDepotForReplacement)
      {
        this.CancelReplaceEnRoute();
        return true;
      }
      if (!this.ReplaceQueued)
        return false;
      this.CancelReplaceQueue(updateDepot);
      return true;
    }

    /// <summary>Cancels all jobs and cancels replace request.</summary>
    public void CancelReplaceEnRoute()
    {
      Mafi.Assert.That<bool>(this.IsDestroyed).IsFalse();
      this.Jobs.CancelAll();
      this.IsOnWayToDepotForReplacement = false;
    }

    /// <summary>Removes this vehicle from a queue to be replaced.</summary>
    public void CancelReplaceQueue(bool updateDepot)
    {
      Mafi.Assert.That<bool>(this.IsDestroyed).IsFalse();
      Mafi.Assert.That<bool>(this.ReplaceQueued).IsTrue();
      if (updateDepot)
      {
        if (this.m_replaceQueuedDepot.IsNone)
          Mafi.Log.Error("Asking to update depot but depot is none.");
        else
          this.m_replaceQueuedDepot.Value.RemoveVehicleFromReplaceQueue(this);
      }
      this.m_replaceQueuedDepot = Option<VehicleDepotBase>.None;
      this.ReplacementProto = Option<DrivingEntityProto>.None;
    }

    public override void Despawn()
    {
      Option<IVehicleJob> currentJob;
      if (this.Jobs.Count <= 1)
      {
        if (this.Jobs.Count == 1)
        {
          currentJob = this.Jobs.CurrentJob;
          if (!(currentJob.ValueOrNull is ScrapOrReplaceVehicleInDepotJob))
          {
            currentJob = this.Jobs.CurrentJob;
            if (currentJob.ValueOrNull is RecoverVehicleJob)
              goto label_5;
          }
          else
            goto label_5;
        }
        else
          goto label_5;
      }
      // ISSUE: variable of a boxed type
      __Boxed<DynamicEntityProto.ID> id = (ValueType) this.Prototype.Id;
      currentJob = this.Jobs.CurrentJob;
      IVehicleJob valueOrNull = currentJob.ValueOrNull;
      Mafi.Log.Error(string.Format("Vehicle '{0}' despawning with job: {1}", (object) id, (object) valueOrNull));
      this.Jobs.CancelAllAndClear();
label_5:
      base.Despawn();
    }

    [Conditional("MAFI_ASSERTIONS")]
    public void AssertHasNoJobOfType<T>() where T : IVehicleJob
    {
      for (int index = 0; index < this.Jobs.Count; ++index)
      {
        if (this.Jobs[index].GetType() == typeof (T))
          Mafi.Assert.Fail(string.Format("Job sequence of {0} already contains job of type {1}: {2}", (object) this, (object) typeof (T), (object) this.Jobs[index]));
      }
    }

    protected virtual int GetStateHash()
    {
      return Hash.Combine<Tile2f, AngleDegrees1f>(this.Position2f, this.Direction);
    }

    protected virtual bool SkipNoMovementMonitoring()
    {
      return this.Jobs.CurrentJob.IsNone || this.Jobs.CurrentJob.Value.SkipNoMovementMonitoring || this.IsStrugglingToNavigate || this.IsStuck || this.PfState == PathFindingEntityState.PathFinding;
    }

    protected override void OnEnabledChanged()
    {
      if (!this.IsEnabled)
      {
        this.Jobs.CancelAll();
        if (this.IsDriving)
          this.StopDriving();
      }
      base.OnEnabledChanged();
    }

    protected override void OnIsBrokenChanged()
    {
      this.updateSpeedFactor();
      base.OnIsBrokenChanged();
    }

    public override string ToString()
    {
      string str = this.HasJobs ? this.CurrentJobInfo.Value : (this.IsSpawned ? "No job" : "Not spawned");
      return base.ToString() + ": " + str;
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<IEntityAssignedWithVehicles>.Serialize(this.AssignedTo, writer);
      Option<string>.Serialize(this.CustomTitle, writer);
      writer.WriteInt(this.GeneralPriority);
      writer.WriteBool(this.IsOnWayToDepotForReplacement);
      writer.WriteBool(this.IsOnWayToDepotForScrap);
      VehicleJobsSequence.Serialize(this.Jobs, writer);
      writer.WriteInt((int) this.LastRefuelRequestIssue);
      writer.WriteNullableStruct<TileSurfaceSlimId>(this.m_currentSurfaceSlimId);
      Option<IFuelStationsManager>.Serialize(this.m_fuelStationsManager, writer);
      Option<Mafi.Core.Entities.Dynamic.FuelTank>.Serialize(this.m_fuelTank, writer);
      EntityNotificator.Serialize(this.m_noFuelNotif, writer);
      Option<VehicleDepotBase>.Serialize(this.m_replaceQueuedDepot, writer);
      writer.WriteInt(this.m_stateHash);
      writer.WriteInt(this.m_stateHashSameCounter);
      writer.WriteInt(this.m_ticksWithNoFuel);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      Option<DrivingEntityProto>.Serialize(this.ReplacementProto, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AssignedTo = Option<IEntityAssignedWithVehicles>.Deserialize(reader);
      this.CustomTitle = Option<string>.Deserialize(reader);
      if (reader.LoadedSaveVersion < 140)
        this.FailedToRequestFuelTruck = reader.ReadBool();
      this.GeneralPriority = reader.ReadInt();
      this.IsOnWayToDepotForReplacement = reader.ReadBool();
      this.IsOnWayToDepotForScrap = reader.ReadBool();
      this.Jobs = VehicleJobsSequence.Deserialize(reader);
      this.LastRefuelRequestIssue = reader.LoadedSaveVersion >= 140 ? (RefuelRequestIssue) reader.ReadInt() : RefuelRequestIssue.None;
      this.m_currentSurfaceSlimId = reader.LoadedSaveVersion >= 140 ? reader.ReadNullableStruct<TileSurfaceSlimId>() : new TileSurfaceSlimId?();
      this.m_fuelStationsManager = Option<IFuelStationsManager>.Deserialize(reader);
      reader.SetField<Vehicle>(this, "m_fuelTank", (object) Option<Mafi.Core.Entities.Dynamic.FuelTank>.Deserialize(reader));
      this.m_noFuelNotif = EntityNotificator.Deserialize(reader);
      this.m_replaceQueuedDepot = Option<VehicleDepotBase>.Deserialize(reader);
      this.m_stateHash = reader.ReadInt();
      this.m_stateHashSameCounter = reader.ReadInt();
      this.m_ticksWithNoFuel = reader.LoadedSaveVersion >= 165 ? reader.ReadInt() : 0;
      reader.RegisterResolvedMember<Vehicle>(this, "m_unstuckJobFactory", typeof (GetUnstuckJob.Factory), true);
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.ReplacementProto = Option<DrivingEntityProto>.Deserialize(reader);
      reader.RegisterInitAfterLoad<Vehicle>(this, "initSelf", InitPriority.Normal);
    }

    static Vehicle()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Vehicle.MAX_NO_MOVEMENT_DURATION = 1.Minutes();
      Vehicle.NOTIFY_FUEL_AFTER_TICKS = 20.Seconds().Ticks;
      Vehicle.SPEED_WHEN_BROKEN = 50.Percent();
      Vehicle.SPEED_ON_LOW_FUEL = 60.Percent();
    }
  }
}
