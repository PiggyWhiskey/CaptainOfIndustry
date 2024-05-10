// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Excavators.Excavator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Mafi.Core.Vehicles.Excavators
{
  /// <summary>
  /// A vehicle that is able to mine terrain and fulfill <see cref="T:Mafi.Core.Vehicles.Jobs.MiningJob" />.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("m_clearUnreachableAfterTimer", 140, typeof (bool), 122, false)]
  [MemberRemovedInSaveVersion("m_activitySinceUnreachableCacheClear", 140, typeof (bool), 122, false)]
  public sealed class Excavator : 
    Vehicle,
    ITruckQueueObserver,
    IEntityWithCloneableConfig,
    IEntityWithMaxServiceRadius,
    IEntity,
    IIsSafeAsHashKey
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ExcavatorProto Prototype;
    private readonly Event<TruckQueue> m_queueEnabledChanged;
    private VehicleCargo m_cargo;
    private readonly PartialMinedProductTracker m_minedProductsTracker;
    private ExcavatorState m_state;
    private ExcavatorState m_previousState;
    private EntityNotificator m_noDesignationNotif;
    private EntityNotificator m_hasNoValidTruckNotif;
    private readonly TickTimer m_shovelTransitionTimer;
    private readonly RotatingCabinDriver m_cabinDriver;
    private Option<Truck> m_loadedTruck;
    private bool m_forceUnloadToTruck;
    private readonly WaitHelper m_waitHelper;
    private readonly TickTimer m_timer;
    private readonly IJobProvider<Excavator> m_jobProvider;
    private readonly IProductsManager m_productsManager;
    private Duration m_truckQueueEnabledDuration;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly UnreachableTerrainDesignationsManager m_unreachableDesignationsManager;

    public static void Serialize(Excavator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Excavator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Excavator.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RotatingCabinDriver.Serialize(this.m_cabinDriver, writer);
      VehicleCargo.Serialize(this.m_cargo, writer);
      writer.WriteBool(this.m_forceUnloadToTruck);
      EntityNotificator.Serialize(this.m_hasNoValidTruckNotif, writer);
      writer.WriteGeneric<IJobProvider<Excavator>>(this.m_jobProvider);
      Option<Truck>.Serialize(this.m_loadedTruck, writer);
      PartialMinedProductTracker.Serialize(this.m_minedProductsTracker, writer);
      EntityNotificator.Serialize(this.m_noDesignationNotif, writer);
      writer.WriteInt((int) this.m_previousState);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      Event<TruckQueue>.Serialize(this.m_queueEnabledChanged, writer);
      TickTimer.Serialize(this.m_shovelTransitionTimer, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteInt((int) this.m_state);
      TickTimer.Serialize(this.m_timer, writer);
      Duration.Serialize(this.m_truckQueueEnabledDuration, writer);
      UnreachableTerrainDesignationsManager.Serialize(this.m_unreachableDesignationsManager, writer);
      WaitHelper.Serialize(this.m_waitHelper, writer);
      Option<Mafi.Core.Buildings.Mine.MineTower>.Serialize(this.MineTower, writer);
      writer.WriteInt((int) this.NextShovelState);
      Option<LooseProductProto>.Serialize(this.PrioritizedProduct, writer);
      writer.WriteGeneric<ExcavatorProto>(this.Prototype);
      writer.WriteInt((int) this.ShovelState);
      writer.WriteInt((int) this.State);
      SimStep.Serialize(this.StateChangedOnSimStep, writer);
      TruckQueue.Serialize(this.TruckQueue, writer);
    }

    public static Excavator Deserialize(BlobReader reader)
    {
      Excavator excavator;
      if (reader.TryStartClassDeserialization<Excavator>(out excavator))
        reader.EnqueueDataDeserialization((object) excavator, Excavator.s_deserializeDataDelayedAction);
      return excavator;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      if (reader.LoadedSaveVersion >= 122 && reader.LoadedSaveVersion < 140)
        reader.ReadBool();
      reader.SetField<Excavator>(this, "m_cabinDriver", (object) RotatingCabinDriver.Deserialize(reader));
      this.m_cargo = VehicleCargo.Deserialize(reader);
      if (reader.LoadedSaveVersion >= 122 && reader.LoadedSaveVersion < 140)
        reader.ReadBool();
      this.m_forceUnloadToTruck = reader.ReadBool();
      this.m_hasNoValidTruckNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<Excavator>(this, "m_jobProvider", (object) reader.ReadGenericAs<IJobProvider<Excavator>>());
      this.m_loadedTruck = Option<Truck>.Deserialize(reader);
      reader.SetField<Excavator>(this, "m_minedProductsTracker", (object) PartialMinedProductTracker.Deserialize(reader));
      this.m_noDesignationNotif = EntityNotificator.Deserialize(reader);
      this.m_previousState = (ExcavatorState) reader.ReadInt();
      reader.SetField<Excavator>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<Excavator>(this, "m_queueEnabledChanged", (object) Event<TruckQueue>.Deserialize(reader));
      reader.SetField<Excavator>(this, "m_shovelTransitionTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<Excavator>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_state = (ExcavatorState) reader.ReadInt();
      reader.SetField<Excavator>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      this.m_truckQueueEnabledDuration = Duration.Deserialize(reader);
      reader.SetField<Excavator>(this, "m_unreachableDesignationsManager", (object) UnreachableTerrainDesignationsManager.Deserialize(reader));
      reader.SetField<Excavator>(this, "m_waitHelper", (object) WaitHelper.Deserialize(reader));
      this.MineTower = Option<Mafi.Core.Buildings.Mine.MineTower>.Deserialize(reader);
      this.NextShovelState = (ExcavatorShovelState) reader.ReadInt();
      this.PrioritizedProduct = Option<LooseProductProto>.Deserialize(reader);
      reader.SetField<Excavator>(this, "Prototype", (object) reader.ReadGenericAs<ExcavatorProto>());
      this.ShovelState = (ExcavatorShovelState) reader.ReadInt();
      this.State = (ExcavatorState) reader.ReadInt();
      this.StateChangedOnSimStep = SimStep.Deserialize(reader);
      this.TruckQueue = TruckQueue.Deserialize(reader);
    }

    public override bool NeedsJob => this.IsIdle && base.NeedsJob;

    public bool IsEmpty => this.Cargo.IsEmpty;

    public bool IsNotEmpty => this.Cargo.IsNotEmpty;

    public bool IsFull => this.Cargo.TotalQuantity >= this.Prototype.Capacity;

    public bool IsNotFull => !this.IsFull;

    public Quantity RemainingCapacity
    {
      get => (this.Prototype.Capacity - this.Cargo.TotalQuantity).Max(Quantity.Zero);
    }

    public IEvent<TruckQueue> QueueEnabledChanged
    {
      get => (IEvent<TruckQueue>) this.m_queueEnabledChanged;
    }

    public Option<LooseProductProto> PrioritizedProduct { get; private set; }

    public IVehicleCargo Cargo => (IVehicleCargo) this.m_cargo;

    public RelTile1i MaxServiceRadius => 3 * this.Prototype.NavTolerance;

    /// <summary>
    /// Mine tower to which is this excavator assigned or none.
    /// </summary>
    public Option<Mafi.Core.Buildings.Mine.MineTower> MineTower { get; private set; }

    public new bool IsIdle
    {
      get => this.m_state == ExcavatorState.Idle && this.m_previousState == ExcavatorState.Idle;
    }

    public ExcavatorState State
    {
      get => this.m_state;
      private set
      {
        this.m_previousState = this.m_state;
        if (value == this.m_state)
          return;
        this.m_state = value;
        this.StateChangedOnSimStep = this.m_simLoopEvents.CurrentStep;
      }
    }

    public SimStep StateChangedOnSimStep { get; private set; }

    private bool StateChanged => this.m_state != this.m_previousState;

    /// <summary>
    /// Whether the shovel has reached requested target and is at rest.
    /// </summary>
    public bool IsShovelAtTarget => this.ShovelState == this.NextShovelState;

    public ExcavatorShovelState ShovelState { get; private set; }

    public ExcavatorShovelState NextShovelState { get; private set; }

    public AngleDegrees1f CabinDirection => this.m_cabinDriver.CabinDirection;

    public AngleDegrees1f CabinDirectionRelative => this.m_cabinDriver.CabinDirectionRelative;

    public bool IsCabinAtTarget => this.m_cabinDriver.IsCabinAtTarget;

    public AngleDegrees1f CabinTargetDelta => this.m_cabinDriver.GetCabinDelta();

    public Tile2f? CabinTarget => this.m_cabinDriver.CabinTarget;

    /// <summary>Trucks that are waiting to be loaded.</summary>
    public TruckQueue TruckQueue { get; private set; }

    protected override bool m_shouldSuppressStrugglingToNavigate
    {
      get => this.m_noDesignationNotif.IsActive;
    }

    public Excavator(
      EntityId id,
      ExcavatorProto prototype,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      TerrainManager terrainManager,
      IVehiclePathFindingManager pathFinder,
      IJobProvider<Excavator> jobProvider,
      IProductsManager productsManager,
      IVehiclesManager vehiclesManager,
      IFuelStationsManager fuelStationsManager,
      IVehicleSurfaceProvider surfaceProvider,
      GetUnstuckJob.Factory unstuckJobFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      UnreachableTerrainDesignationsManager unreachableDesignationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_queueEnabledChanged = new Event<TruckQueue>();
      this.m_cargo = new VehicleCargo();
      this.m_shovelTransitionTimer = new TickTimer();
      this.m_waitHelper = new WaitHelper(int.MaxValue);
      this.m_timer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (DrivingEntityProto) prototype, context, terrainManager, pathFinder, vehiclesManager, surfaceProvider, unstuckJobFactory, maintenanceProvidersFactory);
      this.Prototype = prototype.CheckNotNull<ExcavatorProto>();
      this.m_jobProvider = jobProvider.CheckNotNull<IJobProvider<Excavator>>();
      this.m_productsManager = productsManager.CheckNotNull<IProductsManager>();
      this.m_simLoopEvents = simLoopEvents;
      this.m_unreachableDesignationsManager = unreachableDesignationsManager;
      this.m_noDesignationNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.VehicleNoReachableDesignations);
      this.m_hasNoValidTruckNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.ExcavatorHasNoValidTruck);
      this.State = ExcavatorState.Idle;
      this.m_cabinDriver = new RotatingCabinDriver(prototype.RotatingCabinDriverProto, (DynamicGroundEntity) this);
      this.m_minedProductsTracker = new PartialMinedProductTracker();
      this.TruckQueue = new TruckQueue((Vehicle) this);
      this.SetToAutoRequestRefuelingTrucks(fuelStationsManager);
    }

    protected override void SimUpdateInternal()
    {
      if (!this.IsSpawned && !this.HasJobs)
        return;
      base.SimUpdateInternal();
      if (this.m_waitHelper.WaitOne())
      {
        this.m_previousState = this.m_state;
        this.m_cabinDriver.Update();
        this.simulateShovel();
      }
      else
      {
        if (this.IsMoving && this.HasJobs && this.Jobs.CurrentJob.ValueOrNull is MiningJob)
          this.indicateIsActivelyMining();
        ExcavatorState excavatorState = this.handleState();
        this.State = excavatorState;
        if (this.m_previousState == ExcavatorState.DoJob)
        {
          if (excavatorState == ExcavatorState.DoJob)
          {
            this.m_truckQueueEnabledDuration -= Duration.OneTick;
            if (this.m_truckQueueEnabledDuration.IsNotPositive)
              this.TruckQueue.Disable();
          }
        }
        else
          this.ConsumeFuelPerUpdate();
        this.m_cabinDriver.Update();
        this.simulateShovel();
      }
    }

    public override void EnqueueJob(VehicleJob job, bool enqueueFirst = false)
    {
      base.EnqueueJob(job, enqueueFirst);
    }

    private ExcavatorState handleState()
    {
      if (!this.IsEnabled)
        return ExcavatorState.Idle;
      switch (this.State)
      {
        case ExcavatorState.Idle:
          return this.handleIdleState();
        case ExcavatorState.DoJob:
          return this.handleDoJobState();
        case ExcavatorState.WaitingForTruck:
          return this.handleWaitingForTruck();
        case ExcavatorState.LoadTruck:
          return this.handleLoadTruck();
        case ExcavatorState.WaitingForShovel:
          return this.handleWaitingForShovel();
        case ExcavatorState.GettingUnstuck:
          return this.handleGettingUnstuck();
        default:
          Assert.Fail(string.Format("Invalid state '{0}'.", (object) this.State));
          return ExcavatorState.Idle;
      }
    }

    private ExcavatorState handleIdleState()
    {
      if (this.m_forceUnloadToTruck)
        return !this.m_loadedTruck.HasValue ? ExcavatorState.WaitingForTruck : ExcavatorState.LoadTruck;
      if (this.HasJobs)
        return ExcavatorState.DoJob;
      this.m_cabinDriver.ResetCabinTarget();
      this.SetShovelState(ExcavatorShovelState.Tucked);
      if (!this.StateChanged)
        this.TruckQueue.Disable();
      Assert.That<bool>(this.IsEnabled).IsTrue();
      return this.tryGetJob();
    }

    private ExcavatorState tryGetJob()
    {
      Assert.That<bool>(this.IsDriving).IsFalse();
      Assert.That<bool>(this.HasJobs).IsFalse();
      if (!this.IsSpawned)
        return ExcavatorState.Idle;
      if (this.m_jobProvider.TryGetJobFor(this))
      {
        Assert.That<bool>(this.HasJobs).IsTrue();
        this.m_waitHelper.Reset();
        return ExcavatorState.DoJob;
      }
      this.m_unreachableDesignationsManager.ReportVehicleIdle((Vehicle) this);
      this.m_waitHelper.StartWait();
      return ExcavatorState.Idle;
    }

    private ExcavatorState handleDoJobState()
    {
      bool flag = this.DoJob();
      return this.m_forceUnloadToTruck ? (!this.m_loadedTruck.HasValue ? ExcavatorState.WaitingForTruck : ExcavatorState.LoadTruck) : (flag ? ExcavatorState.DoJob : this.tryGetJob());
    }

    private ExcavatorState handleWaitingForTruck()
    {
      if (this.Cargo.IsEmpty)
        return ExcavatorState.Idle;
      if (this.m_loadedTruck.HasValue)
      {
        Log.Warning("Waiting for truck but already has truck.");
        return ExcavatorState.LoadTruck;
      }
      Option<Truck> firstTruckFor = this.TruckQueue.TryGetFirstTruckFor(this.Cargo);
      if (firstTruckFor.IsNone)
      {
        if (this.AssignedTo.IsNone)
          return ExcavatorState.Idle;
        if (this.IsPathable(this.GroundPositionTile2i))
          return ExcavatorState.WaitingForTruck;
        this.m_unstuckJobFactory.EnqueueJob((Vehicle) this);
        return ExcavatorState.GettingUnstuck;
      }
      this.m_cabinDriver.SetCabinTarget(firstTruckFor.Value.Position2f);
      this.SetShovelState(ExcavatorShovelState.PrepareToDump);
      if (!this.IsCabinAtTarget || !this.IsShovelAtTarget || !this.TruckQueue.FirstVehicleReadyAtQueueTip())
        return ExcavatorState.WaitingForTruck;
      this.m_loadedTruck = firstTruckFor;
      return ExcavatorState.LoadTruck;
    }

    private ExcavatorState handleLoadTruck()
    {
      if (this.m_cargo.IsEmpty)
        return ExcavatorState.Idle;
      Assert.That<Option<Truck>>(this.m_loadedTruck).HasValue<Truck>();
      if (this.StateChanged)
      {
        this.SetShovelState(ExcavatorShovelState.Dump);
        this.m_timer.Start(this.Prototype.MineTimings.DumpDelay);
      }
      if (!this.TruckQueue.IsFirstVehicle(this.m_loadedTruck.Value))
      {
        this.m_loadedTruck = Option<Truck>.None;
        return ExcavatorState.WaitingForTruck;
      }
      if (this.m_timer.Decrement())
        return ExcavatorState.LoadTruck;
      Assert.That<Fix64>(this.Position2f.DistanceSqrTo(this.m_loadedTruck.Value.Position2f)).IsLess((Fix64) this.MaxServiceRadius.Squared, "Excavator is loading truck that is too far.");
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_cargo)
      {
        if (this.m_loadedTruck.Value.Cargo.CanAdd(keyValuePair.Key))
        {
          this.m_cargo.PlanRemove(this.m_loadedTruck.Value.LoadAsMuchAs(new ProductQuantity(keyValuePair.Key, keyValuePair.Value)));
          if (this.m_loadedTruck.Value.IsFull)
            break;
        }
      }
      this.m_cargo.ExecutePlan();
      if (this.m_loadedTruck.Value.IsFull)
        this.TruckQueue.ReleaseFirstVehicle();
      if (this.m_cargo.IsEmpty)
        this.m_forceUnloadToTruck = false;
      this.m_loadedTruck = Option<Truck>.None;
      return ExcavatorState.WaitingForShovel;
    }

    private ExcavatorState handleWaitingForShovel()
    {
      return !this.IsShovelAtTarget ? ExcavatorState.WaitingForShovel : ExcavatorState.Idle;
    }

    private ExcavatorState handleGettingUnstuck()
    {
      return this.Jobs.CurrentJob.ValueOrNull is GetUnstuckJob && this.DoJob() ? ExcavatorState.GettingUnstuck : ExcavatorState.WaitingForTruck;
    }

    internal void SetAbsoluteCabinDirection(AngleDegrees1f direction, [CallerMemberName] string callerMemberName = "")
    {
      this.m_cabinDriver.SetAbsoluteCabinDirection(direction);
    }

    public void SetCabinTarget(Tile2f cabinTarget, [CallerMemberName] string callerMemberName = "")
    {
      this.m_cabinDriver.SetCabinTarget(cabinTarget);
    }

    private void indicateIsActivelyMining()
    {
      if (!this.m_noDesignationNotif.IsActive)
        return;
      this.m_noDesignationNotif.Deactivate((IEntity) this);
    }

    public void ResetCabinTarget() => this.m_cabinDriver.ResetCabinTarget();

    public void SetShovelState(ExcavatorShovelState newShovelState)
    {
      if (newShovelState == this.NextShovelState || newShovelState == this.ShovelState)
        return;
      if (newShovelState == ExcavatorShovelState.PrepareToMine)
        this.indicateIsActivelyMining();
      this.m_shovelTransitionTimer.Start(this.SpeedFactor.ApplyInverseFloored(this.Prototype.MineTimings.GetDuration(newShovelState).Ticks).Ticks());
      this.NextShovelState = newShovelState;
      Assert.That<bool>(this.IsShovelAtTarget).IsFalse();
    }

    private void simulateShovel()
    {
      if (this.IsShovelAtTarget || this.m_shovelTransitionTimer.Decrement())
        return;
      this.ShovelState = this.NextShovelState;
    }

    /// <summary>
    /// Asks the Excavator to unload its Cargo to Truck as fast as it can. If the Excavator does not have an <see cref="P:Mafi.Core.Entities.Dynamic.Vehicle.AssignedTo" /> or loses it after the call and before unload, there will be no effect. (The
    /// Excavator then won't be able to get a Truck.)
    /// </summary>
    public void UnloadToTruck()
    {
      Assert.That<bool>(this.Cargo.IsNotEmpty).IsTrue();
      this.m_forceUnloadToTruck = true;
    }

    /// <summary>
    /// For now to be used when there is nothing we can do with the cargo. In future, we could maybe dump the cargo?
    /// </summary>
    public void ClearCargoImmediately()
    {
      if (this.m_cargo.IsNotEmpty)
        this.m_cargo.ClearCargoImmediately(this.Context.AssetTransactionManager);
      this.m_forceUnloadToTruck = false;
    }

    /// <summary>
    /// Keeps truck queue enabled for specified amount of time. This solution was chosen to avoid state issues.
    /// TODO: Hack, improve this.
    /// </summary>
    public void KeepTruckQueueEnabled(Duration duration)
    {
      this.TruckQueue.Enable();
      this.m_truckQueueEnabledDuration = duration;
    }

    protected override void OnDestroy()
    {
      Assert.That<bool>(this.TruckQueue.IsEnabled).IsFalse();
      this.ClearCargoImmediately();
      base.OnDestroy();
    }

    /// <summary>Mines all terrain at given tile (+shovel radius).</summary>
    internal void MineMixedAt(Tile2iAndIndex tileAndIndex, Excavator.MineTileMixedFn mineTileFn)
    {
      if (this.IsFull)
        return;
      this.m_minedProductsTracker.Reset(this.Prototype.Capacity - this.m_cargo.TotalQuantity, this.m_cargo);
      ImmutableArray<ThicknessTilesF> thicknessByDistance = this.Prototype.MinedThicknessByDistance;
      if (this.Terrain.IsOffLimitsOrInvalid(tileAndIndex.TileCoord))
      {
        Log.Warning("Mining from off-limits tile.");
      }
      else
      {
        mineTileFn(tileAndIndex, this.m_minedProductsTracker, thicknessByDistance[0]);
        int num = 1;
        for (int length = thicknessByDistance.Length; num < length; ++num)
        {
          ThicknessTilesF maxThickness = thicknessByDistance[num];
          for (int index = 0; index <= num; ++index)
          {
            mineTileFn(this.Terrain.OffsetUnclamped(tileAndIndex, index, num), this.m_minedProductsTracker, maxThickness);
            mineTileFn(this.Terrain.OffsetUnclamped(tileAndIndex, num, -index), this.m_minedProductsTracker, maxThickness);
            mineTileFn(this.Terrain.OffsetUnclamped(tileAndIndex, -index, -num), this.m_minedProductsTracker, maxThickness);
            mineTileFn(this.Terrain.OffsetUnclamped(tileAndIndex, -num, index), this.m_minedProductsTracker, maxThickness);
            if (this.m_minedProductsTracker.IsFull)
              break;
          }
          if (this.m_minedProductsTracker.IsFull)
            break;
        }
        foreach (LooseProductQuantity looseProductQuantity in this.m_minedProductsTracker.FinalProductsReadonly())
        {
          if (looseProductQuantity.Quantity.IsPositive)
          {
            this.m_productsManager.ProductCreated((ProductProto) looseProductQuantity.Product, looseProductQuantity.Quantity, CreateReason.MinedFromTerrain);
            this.m_cargo.Add((ProductProto) looseProductQuantity.Product, looseProductQuantity.Quantity);
          }
        }
        Assert.That<Quantity>(this.m_cargo.TotalQuantity).IsLessOrEqual(this.Prototype.Capacity);
      }
    }

    internal void MineProp(TerrainPropData propData)
    {
      Assert.That<bool>(this.IsFull).IsFalse();
      if (!propData.Proto.ProductWhenHarvested.IsNotEmpty)
        return;
      ProductQuantity productQuantity = propData.Proto.ProductWhenHarvested.ScaledBy(propData.Scale);
      this.m_productsManager.ProductCreated(productQuantity.Product, productQuantity.Quantity, CreateReason.MinedFromTerrain);
      this.m_cargo.Add(productQuantity.Product, productQuantity.Quantity);
    }

    protected override void OnAssignTo(IEntityAssignedWithVehicles owner, bool doNotCancelJobs)
    {
      this.MineTower = (Option<Mafi.Core.Buildings.Mine.MineTower>) (owner as Mafi.Core.Buildings.Mine.MineTower);
      Assert.That<Option<Mafi.Core.Buildings.Mine.MineTower>>(this.MineTower).HasValue<Mafi.Core.Buildings.Mine.MineTower>();
      base.OnAssignTo(owner, doNotCancelJobs);
    }

    public override void UnassignFrom(IEntityAssignedWithVehicles owner, bool cancelJobs = true)
    {
      base.UnassignFrom(owner, cancelJobs);
      this.MineTower = (Option<Mafi.Core.Buildings.Mine.MineTower>) Option.None;
      this.PrioritizedProduct = Option<LooseProductProto>.None;
    }

    public void SetPrioritizeProduct(Option<LooseProductProto> product)
    {
      this.PrioritizedProduct = product;
    }

    public override void Spawn(Tile2f position, AngleDegrees1f direction)
    {
      base.Spawn(position, direction);
      this.ShovelState = this.NextShovelState = ExcavatorShovelState.Tucked;
      this.m_cabinDriver.Reset();
    }

    public override void Despawn()
    {
      this.TruckQueue.Disable();
      this.ClearCargoImmediately();
      this.State = ExcavatorState.Idle;
      this.m_previousState = ExcavatorState.Idle;
      this.m_noDesignationNotif.Deactivate((IEntity) this);
      base.Despawn();
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (this.IsEnabled)
        return;
      this.TruckQueue.Disable();
    }

    protected override void OnUpdateSpeedFactor()
    {
      base.OnUpdateSpeedFactor();
      this.m_cabinDriver.SetSpeedFactor(this.SpeedFactor);
    }

    protected override int GetStateHash()
    {
      return Hash.Combine<int, AngleDegrees1f>(base.GetStateHash(), this.m_cabinDriver.CabinDirectionRelative);
    }

    protected override bool SkipNoMovementMonitoring()
    {
      if (base.SkipNoMovementMonitoring())
        return true;
      return this.m_state == ExcavatorState.WaitingForTruck && this.AssignedTo.HasValue && this.TruckQueue.WaitingTrucksCount == 0;
    }

    protected override bool TryRequestScrapInternal()
    {
      if (!base.TryRequestScrapInternal())
        return false;
      this.TruckQueue.Disable();
      this.ClearCargoImmediately();
      return true;
    }

    public override bool TryRequestToGoToDepotForReplacement(DrivingEntityProto targetProto)
    {
      if (!base.TryRequestToGoToDepotForReplacement(targetProto))
        return false;
      this.TruckQueue.Disable();
      this.ClearCargoImmediately();
      return true;
    }

    public void NotifyHasNoValidTruckIff(bool shouldNotify)
    {
      this.m_hasNoValidTruckNotif.NotifyIff(shouldNotify, (IEntity) this);
    }

    public void onQueueEnabledChanged(TruckQueue queue) => this.m_queueEnabledChanged.Invoke(queue);

    public void AddToConfig(EntityConfigData data)
    {
      data.SetPrioritizedProductToMine(this.PrioritizedProduct);
    }

    public void ApplyConfig(EntityConfigData data)
    {
      this.SetPrioritizeProduct(data.GetPrioritizedProductToMine());
    }

    static Excavator()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Excavator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Excavator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    internal delegate void MineTileMixedFn(
      Tile2iAndIndex tileAndIndex,
      PartialMinedProductTracker partialMinedProductTracker,
      ThicknessTilesF maxThickness);
  }
}
