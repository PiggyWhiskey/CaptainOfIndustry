// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.VehicleDepots.VehicleDepotBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.ComputingPower;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.VehicleDepots
{
  [GenerateSerializer(false, null, 0)]
  public abstract class VehicleDepotBase : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithSimUpdate,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IElectricityConsumingEntity,
    IComputingConsumingEntity,
    IEntityWithLogisticsControl,
    IUpgradableEntity,
    IEntityWithSound
  {
    private VehicleDepotBaseProto m_proto;
    private readonly IElectricityConsumer m_electricityConsumer;
    private readonly IComputingConsumer m_computingConsumer;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly TerrainManager m_terrainManager;
    private readonly SpawnJob.Factory m_spawnJobFactory;
    [NewInSaveVersion(113, null, null, typeof (IVehicleBuffersRegistry), null)]
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly IProductsManager m_productsManager;
    protected readonly IInstaBuildManager InstaBuildManager;
    private readonly UpointsManager m_upointsManager;
    protected readonly EntitiesCreator EntitiesCreator;
    [NewInSaveVersion(111, null, null, null, null)]
    private readonly EntitiesCloneConfigHelper m_cloneConfigHelper;
    private readonly VehicleJobs m_vehicleJobs;
    private SimStep m_lastSpawnSimStep;
    private int m_doorOpenTicks;
    private int m_doorOpenTicksTarget;
    private readonly Queueue<DrivingEntityProto> m_buildQueue;
    private readonly Queueue<DrivingEntityProto> m_replaceQueue;
    private readonly Queueue<Vehicle> m_replaceQueueVehicleToReplace;
    private readonly Dict<ProductProto, ProductBuffer> m_buffers;
    private Option<Mafi.Core.Entities.Static.ConstructionProgress> m_constructionState;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private Option<Vehicle> m_replacementInProgress;
    private Option<IEntityAssignedWithVehicles> m_replacementAssignments;
    private readonly Lyst<DrivingEntityProto> m_replacementAssigneeProtos;

    [DoNotSave(0, null)]
    public VehicleDepotBaseProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    [DoNotSave(0, null)]
    public Tile2f SpawnPosition { get; private set; }

    [DoNotSave(0, null)]
    public Tile2f DespawnPosition { get; private set; }

    [DoNotSave(0, null)]
    public AngleDegrees1f SpawnDirection { get; private set; }

    [DoNotSave(0, null)]
    public Tile2f SpawnDrivePosition { get; private set; }

    [DoNotSave(0, null)]
    public Tile2f DespawnDrivePosition { get; private set; }

    public IUpgrader Upgrader { get; private set; }

    public Electricity PowerRequired => this.Prototype.ConsumedPowerPerTick;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    protected bool ConsumedPower { get; set; }

    public Computing ComputingRequired => this.Prototype.ConsumedComputingPerTick;

    public Option<IComputingConsumerReadonly> ComputingConsumer
    {
      get => this.m_computingConsumer.CreateOption<IComputingConsumerReadonly>();
    }

    protected bool ConsumedComputing { get; set; }

    public Mafi.Core.Entities.SoundParams? SoundParams
    {
      get
      {
        return !this.Prototype.Graphics.SoundPrefabPath.HasValue ? new Mafi.Core.Entities.SoundParams?() : new Mafi.Core.Entities.SoundParams?(new Mafi.Core.Entities.SoundParams(this.Prototype.Graphics.SoundPrefabPath.Value, SoundSignificance.Normal));
      }
    }

    public bool IsSoundOn => this.IsEnabled && this.CurrentState == VehicleDepotBase.State.Working;

    public bool CanWork
    {
      get
      {
        bool canWork;
        switch (this.CurrentState)
        {
          case VehicleDepotBase.State.Idle:
          case VehicleDepotBase.State.Working:
            canWork = true;
            break;
          default:
            canWork = false;
            break;
        }
        return canWork;
      }
    }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public VehicleDepotBase.State CurrentState { get; private set; }

    protected virtual bool ProvideProductsForVehicleScrap => true;

    public int VehicleJobsCount => this.m_vehicleJobs.Count;

    public Percent DoorOpenPerc
    {
      get => Percent.FromRatio(this.m_doorOpenTicks, this.Prototype.DoorOpenDuration.Ticks);
    }

    public Mafi.Core.Vehicles.VehicleQueue<Vehicle, VehicleDepotBase> VehicleQueue { get; private set; }

    public IIndexable<DrivingEntityProto> BuildQueue
    {
      get => (IIndexable<DrivingEntityProto>) this.m_buildQueue;
    }

    public IIndexable<DrivingEntityProto> ReplaceQueue
    {
      get => (IIndexable<DrivingEntityProto>) this.m_replaceQueue;
    }

    public Option<DrivingEntityProto> CurrentlyBuildVehicle
    {
      get
      {
        return !this.m_replaceQueue.IsNotEmpty ? this.m_buildQueue.FirstOrNone<DrivingEntityProto>() : this.m_replaceQueue.FirstOrNone<DrivingEntityProto>();
      }
    }

    public IEnumerable<IProductBufferReadOnly> Buffers
    {
      get => (IEnumerable<IProductBufferReadOnly>) this.m_buffers.Values;
    }

    public Option<IConstructionProgress> VehicleConstructionProgress
    {
      get => this.m_constructionState.As<IConstructionProgress>();
    }

    [NewInSaveVersion(97, null, null, null, null)]
    public bool DestroyCallbackStarted { get; private set; }

    public VehicleDepotBase(
      EntityId id,
      VehicleDepotBaseProto proto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      IVehiclesManager vehiclesManager,
      TerrainManager terrainManager,
      SpawnJob.Factory spawnJobFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IProductsManager productsManager,
      IInstaBuildManager instaBuildManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      UpointsManager upointsManager,
      EntitiesCreator entitiesCreator,
      EntitiesCloneConfigHelper cloneConfigHelper)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_vehicleJobs = new VehicleJobs();
      this.m_lastSpawnSimStep = SimStep.One;
      this.m_buildQueue = new Queueue<DrivingEntityProto>();
      this.m_replaceQueue = new Queueue<DrivingEntityProto>();
      this.m_replaceQueueVehicleToReplace = new Queueue<Vehicle>();
      this.m_buffers = new Dict<ProductProto, ProductBuffer>();
      this.m_replacementInProgress = Option<Vehicle>.None;
      this.m_replacementAssigneeProtos = new Lyst<DrivingEntityProto>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.m_simLoopEvents = simLoopEvents;
      this.m_vehiclesManager = vehiclesManager;
      this.m_terrainManager = terrainManager;
      this.m_spawnJobFactory = spawnJobFactory;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_productsManager = productsManager;
      this.InstaBuildManager = instaBuildManager;
      this.m_upointsManager = upointsManager;
      this.EntitiesCreator = entitiesCreator;
      this.m_cloneConfigHelper = cloneConfigHelper;
      this.Prototype = proto.CheckNotNull<VehicleDepotBaseProto>();
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.m_computingConsumer = this.Context.ComputingConsumerFactory.CreateConsumer((IComputingConsumingEntity) this);
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) new KeepFullEntityPriorityProvider((IEntityWithGeneralPriority) this), (IOutputBufferPriorityProvider) new KeepEmptyPriorityProvider(14), vehicleBuffersRegistry);
      this.Upgrader = upgraderFactory.CreateInstance<VehicleDepotBaseProto, VehicleDepotBase>(this, this.Prototype);
      this.VehicleQueue = new Mafi.Core.Vehicles.VehicleQueue<Vehicle, VehicleDepotBase>(this);
      this.VehicleQueue.Enable();
      this.initialize();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initialize() => this.updateCachedTiles();

    public virtual void SimUpdate()
    {
      this.simulateDoors();
      if (this.IsNotEnabled)
        this.CurrentState = VehicleDepotBase.State.Paused;
      else if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
      {
        this.CurrentState = VehicleDepotBase.State.NotEnoughWorkers;
      }
      else
      {
        if (this.ConsumedPower)
        {
          if (!this.m_electricityConsumer.TryConsume())
          {
            this.CurrentState = VehicleDepotBase.State.NotEnoughPower;
            return;
          }
          this.ConsumedPower = false;
        }
        if (this.ConsumedComputing)
        {
          if (!this.m_computingConsumer.TryConsume())
          {
            this.CurrentState = VehicleDepotBase.State.NotEnoughComputing;
            return;
          }
          this.ConsumedComputing = false;
        }
        this.CurrentState = this.stepVehicleConstruction();
      }
    }

    public bool WaitForOpenDoors()
    {
      this.m_doorOpenTicksTarget = this.Prototype.DoorOpenDuration.Ticks;
      return this.m_doorOpenTicks >= this.m_doorOpenTicksTarget;
    }

    public bool WaitForClosedDoors()
    {
      this.m_doorOpenTicksTarget = 0;
      return this.m_doorOpenTicks <= this.m_doorOpenTicksTarget;
    }

    public override bool GetCustomPfTargetTiles(int retryNumber, Lyst<Tile2i> outTiles)
    {
      outTiles.Add(this.DespawnDrivePosition.Tile2i);
      return false;
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      ProductBuffer bufferReceived;
      if (this.IsNotEnabled || !this.m_buffers.TryGetValue(pq.Product, out bufferReceived) || this.m_autoLogisticsHelper.HasOutputBufferFor(pq.Product))
        return pq.Quantity;
      Quantity asMuchAsFromPort = bufferReceived.StoreAsMuchAs(pq);
      this.m_autoLogisticsHelper.OnProductReceivedFromPort((IProductBuffer) bufferReceived);
      return asMuchAsFromPort;
    }

    public void StoreMaterialsFromVehicleScrap(AssetValue value)
    {
      if (!this.ProvideProductsForVehicleScrap)
        return;
      foreach (ProductQuantity product in value.Products)
      {
        if (!product.IsEmpty)
        {
          ProductBuffer buffer = this.getOrCreateBuffer(product.Product);
          buffer.IncreaseCapacityTo(buffer.Quantity + product.Quantity);
          buffer.StoreExactly(product.Quantity);
          this.m_productsManager.ProductCreated(product, CreateReason.Deconstruction);
          if (this.m_constructionState.IsNone || !this.m_constructionState.Value.Buffers.Contains((IProductBufferReadOnly) buffer))
            this.m_autoLogisticsHelper.TryAddOutputBuffer((IProductBuffer) buffer);
        }
      }
    }

    protected override void SetTransform(TileTransform transform)
    {
      base.SetTransform(transform);
      this.updateCachedTiles();
    }

    private void updateCachedTiles()
    {
      Tile2f xy = this.Prototype.Layout.GetModelOrigin(this.Transform).Xy;
      this.SpawnDrivePosition = xy + this.Prototype.Layout.TransformDirection(this.Prototype.SpawnDriveTargetPosition, this.Transform);
      this.DespawnDrivePosition = xy + this.Prototype.Layout.TransformDirection(this.Prototype.DespawnDriveTargetPosition, this.Transform);
      this.SpawnPosition = xy + this.Prototype.Layout.TransformDirection(this.Prototype.SpawnPosition, this.Transform);
      this.DespawnPosition = xy + this.Prototype.Layout.TransformDirection(this.Prototype.DespawnPosition, this.Transform);
      this.SpawnDirection = this.Transform.Transform(this.Prototype.SpawnDirection);
    }

    public virtual bool CanAccept(DynamicGroundEntity entity)
    {
      return this.IsEnabled && this.Prototype.BuildableEntities.Contains(entity.Prototype);
    }

    public virtual bool CanAcceptForUpgrade(
      DrivingEntityProto currentProto,
      DrivingEntityProto newProto)
    {
      return this.IsEnabled && this.Prototype.BuildableEntities.Contains((DynamicGroundEntityProto) currentProto) && this.Prototype.BuildableEntities.Contains((DynamicGroundEntityProto) newProto);
    }

    /// <summary>
    /// Handles scrapping of the vehicle that should be replaced before the built vehicle can spawn.
    /// Returns true if this processing isn't ready for build yet.
    /// </summary>
    private bool processReplacement()
    {
      if (this.m_replacementInProgress.HasValue)
      {
        if (this.m_replacementInProgress.Value.IsOnWayToDepotForReplacement)
          return !this.m_replacementInProgress.Value.IsDestroyed;
        this.m_replacementAssignments = Option<IEntityAssignedWithVehicles>.None;
        this.m_replacementAssigneeProtos.Clear();
        return false;
      }
      if (this.m_replaceQueue.IsEmpty || this.m_replaceQueueVehicleToReplace.IsEmpty)
      {
        this.m_replacementAssignments = Option<IEntityAssignedWithVehicles>.None;
        this.m_replacementAssigneeProtos.Clear();
        return false;
      }
      this.m_replacementInProgress = (Option<Vehicle>) this.m_replaceQueueVehicleToReplace.Peek();
      Vehicle vehicle = this.m_replacementInProgress.Value;
      if (!vehicle.ReplaceQueued)
        return false;
      if (vehicle.TryRequestToGoToDepotForReplacement(this.m_replaceQueue.First))
        return true;
      this.m_replacementAssignments = Option<IEntityAssignedWithVehicles>.None;
      this.m_replacementAssigneeProtos.Clear();
      return false;
    }

    public void SetNextSpawnAssignment(IEntityAssignedWithVehicles assignedTo)
    {
      this.m_replacementAssignments = Option<IEntityAssignedWithVehicles>.Create(assignedTo);
    }

    public void SetNextSpawnAssignees(IIndexable<Vehicle> allVehicles)
    {
      this.m_replacementAssigneeProtos.Clear();
      this.m_replacementAssigneeProtos.AddRange(allVehicles.ToImmutableArray<Vehicle>().Map<DrivingEntityProto>((Func<Vehicle, DrivingEntityProto>) (v => v.Prototype)));
    }

    public virtual bool CanFinalizeVehicleBuildAndAddToWorld() => true;

    private VehicleDepotBase.State stepVehicleConstruction()
    {
      if (this.CurrentlyBuildVehicle.IsNone)
        return VehicleDepotBase.State.Idle;
      if (this.m_constructionState.IsNone)
      {
        Assert.Fail("Expected construction state to be present during build!");
        return VehicleDepotBase.State.Idle;
      }
      Mafi.Core.Entities.Static.ConstructionProgress constructionProgress = this.m_constructionState.Value;
      constructionProgress.IsPaused = false;
      this.ConsumedPower = true;
      this.ConsumedComputing = true;
      if (this.InstaBuildManager.IsInstaBuildEnabled)
      {
        if (this.processReplacement())
        {
          this.ConsumedPower = false;
          this.ConsumedComputing = false;
          return VehicleDepotBase.State.Idle;
        }
        if (!this.m_vehiclesManager.CanBuildVehicle((DynamicEntityProto) this.CurrentlyBuildVehicle.Value))
        {
          this.ConsumedPower = false;
          this.ConsumedComputing = false;
          return VehicleDepotBase.State.Idle;
        }
        if (!this.CanFinalizeVehicleBuildAndAddToWorld())
          return VehicleDepotBase.State.Working;
        this.tryBuildAndSpawnVehicle();
        return VehicleDepotBase.State.Working;
      }
      if (!constructionProgress.IsDone)
      {
        constructionProgress.TryMakeStep();
        return VehicleDepotBase.State.Working;
      }
      if (this.processReplacement())
      {
        this.ConsumedPower = false;
        this.ConsumedComputing = false;
        return VehicleDepotBase.State.Working;
      }
      if (!this.m_vehiclesManager.CanBuildVehicle((DynamicEntityProto) this.CurrentlyBuildVehicle.Value))
      {
        this.ConsumedPower = false;
        this.ConsumedComputing = false;
        return VehicleDepotBase.State.Idle;
      }
      if (!this.CanFinalizeVehicleBuildAndAddToWorld())
        return VehicleDepotBase.State.Working;
      foreach (ProductQuantity product in constructionProgress.TotalCost.Products)
      {
        Quantity quantity = this.m_buffers[product.Product].RemoveAsMuchAs(product.Quantity);
        if (quantity < product.Quantity)
          Log.Error(string.Format("There was not enough {0} to remove from depot (removed: {1})", (object) product, (object) quantity));
        this.m_productsManager.ProductDestroyed(product.Product.WithQuantity(quantity), DestroyReason.Construction);
      }
      this.tryBuildAndSpawnVehicle();
      return VehicleDepotBase.State.Working;
    }

    public void RemoveVehicleFromReplaceQueue(Vehicle vehicle)
    {
      Assert.That<int>(this.m_replaceQueue.Count).IsNotZero();
      Assert.That<int>(this.m_replaceQueueVehicleToReplace.Count).IsEqualTo(this.m_replaceQueue.Count);
      Assert.That<bool>(vehicle.ReplaceQueued).IsTrue();
      for (int index = 0; index < this.m_replaceQueueVehicleToReplace.Count; ++index)
      {
        if (this.m_replaceQueueVehicleToReplace.GetRefAt(index).Equals((Entity) vehicle))
        {
          this.RemoveVehicleFromBuildOrReplaceQueue(index);
          return;
        }
      }
      Assert.Fail(string.Format("'{0}' not found in queue.", (object) vehicle));
    }

    public void RemoveVehicleFromBuildOrReplaceQueue(int index)
    {
      if (this.m_replaceQueue.Count + this.m_buildQueue.Count <= index)
      {
        Assert.Fail(string.Format("Invalid vehicle remove index {0}. Count: {1}.", (object) index, (object) (this.m_replaceQueue.Count + this.m_buildQueue.Count)));
      }
      else
      {
        if (index >= this.m_replaceQueue.Count)
        {
          this.m_buildQueue.RemoveAt(index - this.m_replaceQueue.Count);
        }
        else
        {
          Assert.That<int>(this.m_replaceQueueVehicleToReplace.Count).IsEqualTo(this.m_replaceQueue.Count);
          Vehicle vehicle = this.m_replaceQueueVehicleToReplace[index];
          Assert.That<bool>(vehicle.ReplaceQueued || vehicle.IsOnWayToDepotForReplacement).IsTrue();
          vehicle.CancelReplace(false);
          if (index == 0)
          {
            this.m_replacementInProgress = Option<Vehicle>.None;
            this.m_replacementAssignments = Option<IEntityAssignedWithVehicles>.None;
            this.m_replacementAssigneeProtos.Clear();
          }
          this.m_replaceQueue.RemoveAt(index);
          this.m_replaceQueueVehicleToReplace.RemoveAt(index);
        }
        if (index != 0)
          return;
        this.onTipOfQueueChange();
      }
    }

    public bool AddVehicleToBuildQueue(DrivingEntityProto vehicleProto)
    {
      if (!this.Prototype.BuildableEntities.Contains((DynamicGroundEntityProto) vehicleProto))
      {
        Log.Error(string.Format("Trying to build unsupported vehicle '{0}' in depot {1}.", (object) vehicleProto, (object) this));
        return false;
      }
      this.m_buildQueue.Enqueue(vehicleProto);
      if (this.m_buildQueue.Count == 1 && this.m_replaceQueue.Count == 0)
        this.onTipOfQueueChange();
      return true;
    }

    public bool TryAddVehicleToReplaceQueue(Vehicle vehicle, DrivingEntityProto vehicleProto)
    {
      if (!this.Prototype.BuildableEntities.Contains((DynamicGroundEntityProto) vehicleProto))
      {
        Log.Error(string.Format("Trying to replace unsupported vehicle '{0}' in depot {1}.", (object) vehicleProto, (object) this));
        return false;
      }
      this.m_replaceQueue.Enqueue(vehicleProto);
      this.m_replaceQueueVehicleToReplace.Enqueue(vehicle);
      Assert.That<int>(this.m_replaceQueueVehicleToReplace.Count).IsEqualTo(this.m_replaceQueue.Count);
      if (this.m_replaceQueue.Count == 1)
        this.onTipOfQueueChange();
      return true;
    }

    public bool TryQuickBuildCurrentVehicle()
    {
      return !this.CurrentlyBuildVehicle.IsNone && this.m_constructionState.Value.TryPerformQuickBuild(this.Context.AssetTransactionManager, (IUpointsManager) this.m_upointsManager, this.m_vehicleBuffersRegistry);
    }

    internal void Cheat_FinishBuildOfCurrentVehicle()
    {
      if (this.CurrentlyBuildVehicle.IsNone)
        return;
      Mafi.Core.Entities.Static.ConstructionProgress constructionProgress = this.m_constructionState.Value;
      foreach (ProductBuffer constructionBuffer in constructionProgress.ConstructionBuffers)
      {
        Quantity quantity = constructionBuffer.UsableCapacity - constructionBuffer.StoreAsMuchAs(constructionBuffer.Product.WithQuantity(constructionBuffer.UsableCapacity));
        this.m_productsManager.ProductCreated(constructionBuffer.Product, quantity, CreateReason.Cheated);
      }
      constructionProgress.MakeFinished();
    }

    private ProductBuffer getOrCreateBuffer(ProductProto product)
    {
      ProductBuffer buffer;
      if (!this.m_buffers.TryGetValue(product, out buffer))
      {
        buffer = new ProductBuffer(Quantity.One, product);
        this.m_buffers.Add(product, buffer);
      }
      return buffer;
    }

    private void onTipOfQueueChange()
    {
      foreach (ProductBuffer buffer in this.m_buffers.Values)
      {
        this.m_autoLogisticsHelper.TryRemoveInputBuffer((IProductBuffer) buffer);
        if (buffer.IsNotEmpty())
        {
          buffer.ForceNewCapacityTo(buffer.Quantity);
          this.m_autoLogisticsHelper.TryAddOutputBuffer((IProductBuffer) buffer);
        }
        else
          buffer.ForceNewCapacityTo(Quantity.Zero);
      }
      if (this.m_constructionState.HasValue)
        this.m_constructionState = Option<Mafi.Core.Entities.Static.ConstructionProgress>.None;
      if (this.CurrentlyBuildVehicle.IsNone)
        return;
      Lyst<ProductBuffer> lyst = new Lyst<ProductBuffer>();
      DrivingEntityProto drivingEntityProto = this.CurrentlyBuildVehicle.Value;
      foreach (ProductQuantity product in drivingEntityProto.CostToBuild.Products)
      {
        ProductBuffer buffer = this.getOrCreateBuffer(product.Product);
        buffer.SetCapacityAsLessAs(product.Quantity);
        this.m_autoLogisticsHelper.TryAddInputBuffer((IProductBuffer) buffer);
        this.m_autoLogisticsHelper.TryRemoveOutputBuffer((IProductBuffer) buffer);
        lyst.Add(buffer);
      }
      this.m_constructionState = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) new Mafi.Core.Entities.Static.ConstructionProgress((IEntity) this, lyst.ToImmutableArray(), drivingEntityProto.CostToBuild, drivingEntityProto.BuildDurationPerProduct, drivingEntityProto.BuildExtraDuration);
      this.m_constructionState.Value.IsPaused = !this.IsEnabled;
    }

    private bool tryBuildAndSpawnVehicle()
    {
      if (this.CurrentlyBuildVehicle.IsNone)
        Log.Error("Trying to spawn vehicle with none in queue.");
      Vehicle vehicle;
      if (!this.TryBuildVehicle(out vehicle))
        return false;
      this.SpawnVehicle(vehicle);
      return true;
    }

    private void restoreAssignments(Vehicle vehicle, bool isCancel)
    {
      if (this.m_replacementAssignments.HasValue)
      {
        if (!isCancel)
        {
          Assert.That<Option<Vehicle>>(this.m_replacementInProgress).HasValue<Vehicle>();
          Assert.That<bool>(this.m_replacementInProgress.Value.IsDestroyed).IsTrue();
        }
        IEntityAssignedWithVehicles assignedWithVehicles = this.m_replacementAssignments.Value;
        if (!assignedWithVehicles.IsDestroyed)
        {
          if (assignedWithVehicles.CanVehicleBeAssigned((DynamicEntityProto) vehicle.Prototype))
            assignedWithVehicles.AssignVehicle(vehicle);
          else if (isCancel)
            Log.Warning(string.Format("'{0}' cannot be assigned to '{1}'", (object) vehicle, (object) assignedWithVehicles));
        }
      }
      if (this.m_replacementAssigneeProtos.Count > 0)
      {
        if (vehicle is IEntityAssignedWithVehicles entity)
        {
          foreach (DrivingEntityProto replacementAssigneeProto in this.m_replacementAssigneeProtos)
          {
            if (entity.CanVehicleBeAssigned((DynamicEntityProto) replacementAssigneeProto))
              entity.AssignVehicle(this.m_vehiclesManager, (DynamicEntityProto) replacementAssigneeProto);
            else if (isCancel)
              Log.Warning(string.Format("Assigned proto '{0}' isn't assignable to. '{1}'", (object) replacementAssigneeProto, (object) vehicle));
          }
        }
        else if (isCancel)
          Log.Warning(string.Format("Vehicle with replacement assignees but isn't assignable to. '{0}'", (object) vehicle));
      }
      if (!this.m_replacementInProgress.HasValue)
        return;
      this.m_cloneConfigHelper.ApplyConfigTo(this.m_cloneConfigHelper.CreateConfigFrom((IEntity) this.m_replacementInProgress.Value), (IEntity) vehicle);
    }

    protected virtual bool TryBuildVehicle(out Vehicle vehicle)
    {
      Queueue<DrivingEntityProto> queueue = this.m_replaceQueue.Count > 0 ? this.m_replaceQueue : this.m_buildQueue;
      Assert.That<Queueue<DrivingEntityProto>>(queueue).IsNotEmpty<DrivingEntityProto>();
      Assert.That<bool>(this.CanFinalizeVehicleBuildAndAddToWorld()).IsTrue();
      DynamicGroundEntityProto proto = (DynamicGroundEntityProto) queueue.Dequeue();
      if (queueue == this.m_replaceQueue)
        this.m_replaceQueueVehicleToReplace.Dequeue();
      Assert.That<int>(this.m_replaceQueueVehicleToReplace.Count).IsEqualTo(this.m_replaceQueue.Count);
      this.onTipOfQueueChange();
      if (!this.EntitiesCreator.TryCreateVehicle(proto, out vehicle))
      {
        Log.Warning(string.Format("Failed to instantiate vehicle '{0}'.", (object) proto.Id));
        return false;
      }
      vehicle.TeleportTo(this.Position2f, new AngleDegrees1f?());
      this.restoreAssignments(vehicle, false);
      this.m_replacementInProgress = Option<Vehicle>.None;
      this.m_replacementAssignments = Option<IEntityAssignedWithVehicles>.None;
      this.m_replacementAssigneeProtos.Clear();
      EntityValidationResult validationResult = ((EntitiesManager) this.Context.EntitiesManager).TryAddEntity((IEntity) vehicle);
      if (validationResult.IsError)
      {
        Log.Warning(string.Format("Failed to add vehicle '{0}' to the world. Error: {1}", (object) proto.Id, (object) validationResult.ErrorMessage));
        ((IEntityFriend) vehicle).Destroy();
        vehicle = (Vehicle) null;
        return false;
      }
      TerrainManager terrainManager1 = this.m_terrainManager;
      TerrainManager terrainManager2 = this.m_terrainManager;
      Tile2f tile2f = this.SpawnDrivePosition;
      Tile2i tile2i1 = tile2f.Tile2i;
      Tile2iIndex tileIndex1 = terrainManager2.GetTileIndex(tile2i1);
      Assertion<bool> actual1 = Assert.That<bool>(terrainManager1.IsBlockingVehicles(tileIndex1));
      TerrainManager terrainManager3 = this.m_terrainManager;
      TerrainManager terrainManager4 = this.m_terrainManager;
      tile2f = this.SpawnDrivePosition;
      Tile2i tile2i2 = tile2f.Tile2i;
      Tile2iIndex tileIndex2 = terrainManager4.GetTileIndex(tile2i2);
      string str1 = terrainManager3.Debug_ExplainFlags(tileIndex2);
      actual1.IsFalse<string>("Vehicle depot spawn tile is not pathable:\n{0}", str1);
      TerrainManager terrainManager5 = this.m_terrainManager;
      TerrainManager terrainManager6 = this.m_terrainManager;
      tile2f = this.DespawnDrivePosition;
      Tile2i tile2i3 = tile2f.Tile2i;
      Tile2iIndex tileIndex3 = terrainManager6.GetTileIndex(tile2i3);
      Assertion<bool> actual2 = Assert.That<bool>(terrainManager5.IsBlockingVehicles(tileIndex3));
      TerrainManager terrainManager7 = this.m_terrainManager;
      TerrainManager terrainManager8 = this.m_terrainManager;
      tile2f = this.DespawnDrivePosition;
      Tile2i tile2i4 = tile2f.Tile2i;
      Tile2iIndex tileIndex4 = terrainManager8.GetTileIndex(tile2i4);
      string str2 = terrainManager7.Debug_ExplainFlags(tileIndex4);
      actual2.IsFalse<string>("Vehicle depot despawn tile is not pathable:\n{0}", str2);
      return true;
    }

    protected virtual void SpawnVehicle(Vehicle vehicle)
    {
      if (this.InstaBuildManager.IsInstaBuildEnabled)
        vehicle.Spawn(this.SpawnDrivePosition, this.SpawnDirection);
      else
        this.m_spawnJobFactory.EnqueueFirstJob(vehicle, this);
    }

    private void simulateDoors()
    {
      if (!this.CanWork)
        this.m_doorOpenTicksTarget = 0;
      if (this.m_doorOpenTicks != this.m_doorOpenTicksTarget)
      {
        this.m_doorOpenTicks += (this.m_doorOpenTicksTarget - this.m_doorOpenTicks).Sign();
        this.ConsumedPower = true;
      }
      this.m_doorOpenTicksTarget = 0;
    }

    /// <summary>
    /// Returns true if a vehicle can be spawned based on min time between spawns.
    /// This also sets the spawn time when true is returned.
    /// </summary>
    public bool TrySpawnVehicle()
    {
      if (this.m_simLoopEvents.CurrentStep - this.m_lastSpawnSimStep < this.Prototype.SpawnInterval)
        return false;
      this.m_lastSpawnSimStep = this.m_simLoopEvents.CurrentStep;
      return true;
    }

    protected override void OnDestroy()
    {
      this.DestroyCallbackStarted = true;
      foreach (ProductBuffer productBuffer in this.m_buffers.Values)
      {
        this.Context.AssetTransactionManager.StoreClearedProduct(new ProductQuantity(productBuffer.Product, productBuffer.Quantity));
        productBuffer.RemoveAll();
      }
      foreach (Vehicle vehicle in this.m_replaceQueueVehicleToReplace)
      {
        if (vehicle.ReplaceQueued)
        {
          vehicle.CancelReplaceQueue(false);
        }
        else
        {
          vehicle.CancelReplaceEnRoute();
          if (this.m_replacementInProgress.HasValue && vehicle == this.m_replacementInProgress)
            this.restoreAssignments(vehicle, true);
          else
            Log.Warning("En route vehicle is not as we expect.");
        }
      }
      this.m_vehicleJobs.Destroy();
      base.OnDestroy();
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (!this.m_constructionState.HasValue)
        return;
      this.m_constructionState.Value.IsPaused = !this.IsEnabled;
    }

    public void AddJob(IDepotJob job) => this.m_vehicleJobs.TryAddJob((IVehicleJob) job);

    public void JobDone(IDepotJob job) => this.m_vehicleJobs.TryRemoveJob((IVehicleJob) job);

    public void JobCanceled(IDepotJob job) => this.m_vehicleJobs.TryRemoveJob((IVehicleJob) job);

    public bool CanDisableLogisticsInput => true;

    public bool CanDisableLogisticsOutput => false;

    public EntityLogisticsMode LogisticsInputMode => this.m_autoLogisticsHelper.LogisticsInputMode;

    public EntityLogisticsMode LogisticsOutputMode => EntityLogisticsMode.Off;

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsInputMode(mode);
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    public void UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.ConsumedComputing);
      writer.WriteBool(this.ConsumedPower);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteBool(this.DestroyCallbackStarted);
      writer.WriteGeneric<IInstaBuildManager>(this.InstaBuildManager);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      Dict<ProductProto, ProductBuffer>.Serialize(this.m_buffers, writer);
      Queueue<DrivingEntityProto>.Serialize(this.m_buildQueue, writer);
      writer.WriteGeneric<IComputingConsumer>(this.m_computingConsumer);
      Option<Mafi.Core.Entities.Static.ConstructionProgress>.Serialize(this.m_constructionState, writer);
      writer.WriteInt(this.m_doorOpenTicks);
      writer.WriteInt(this.m_doorOpenTicksTarget);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      SimStep.Serialize(this.m_lastSpawnSimStep, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<VehicleDepotBaseProto>(this.m_proto);
      Lyst<DrivingEntityProto>.Serialize(this.m_replacementAssigneeProtos, writer);
      Option<IEntityAssignedWithVehicles>.Serialize(this.m_replacementAssignments, writer);
      Option<Vehicle>.Serialize(this.m_replacementInProgress, writer);
      Queueue<DrivingEntityProto>.Serialize(this.m_replaceQueue, writer);
      Queueue<Vehicle>.Serialize(this.m_replaceQueueVehicleToReplace, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      UpointsManager.Serialize(this.m_upointsManager, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      VehicleJobs.Serialize(this.m_vehicleJobs, writer);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
      Mafi.Core.Vehicles.VehicleQueue<Vehicle, VehicleDepotBase>.Serialize(this.VehicleQueue, writer);
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ConsumedComputing = reader.ReadBool();
      this.ConsumedPower = reader.ReadBool();
      this.CurrentState = (VehicleDepotBase.State) reader.ReadInt();
      this.DestroyCallbackStarted = reader.LoadedSaveVersion >= 97 && reader.ReadBool();
      reader.RegisterResolvedMember<VehicleDepotBase>(this, "EntitiesCreator", typeof (EntitiesCreator), true);
      reader.SetField<VehicleDepotBase>(this, "InstaBuildManager", (object) reader.ReadGenericAs<IInstaBuildManager>());
      reader.SetField<VehicleDepotBase>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      reader.SetField<VehicleDepotBase>(this, "m_buffers", (object) Dict<ProductProto, ProductBuffer>.Deserialize(reader));
      reader.SetField<VehicleDepotBase>(this, "m_buildQueue", (object) Queueue<DrivingEntityProto>.Deserialize(reader));
      reader.RegisterResolvedMember<VehicleDepotBase>(this, "m_cloneConfigHelper", typeof (EntitiesCloneConfigHelper), true);
      reader.SetField<VehicleDepotBase>(this, "m_computingConsumer", (object) reader.ReadGenericAs<IComputingConsumer>());
      this.m_constructionState = Option<Mafi.Core.Entities.Static.ConstructionProgress>.Deserialize(reader);
      this.m_doorOpenTicks = reader.ReadInt();
      this.m_doorOpenTicksTarget = reader.ReadInt();
      reader.SetField<VehicleDepotBase>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      this.m_lastSpawnSimStep = SimStep.Deserialize(reader);
      reader.SetField<VehicleDepotBase>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<VehicleDepotBaseProto>();
      reader.SetField<VehicleDepotBase>(this, "m_replacementAssigneeProtos", (object) Lyst<DrivingEntityProto>.Deserialize(reader));
      this.m_replacementAssignments = Option<IEntityAssignedWithVehicles>.Deserialize(reader);
      this.m_replacementInProgress = Option<Vehicle>.Deserialize(reader);
      reader.SetField<VehicleDepotBase>(this, "m_replaceQueue", (object) Queueue<DrivingEntityProto>.Deserialize(reader));
      reader.SetField<VehicleDepotBase>(this, "m_replaceQueueVehicleToReplace", (object) Queueue<Vehicle>.Deserialize(reader));
      reader.SetField<VehicleDepotBase>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.RegisterResolvedMember<VehicleDepotBase>(this, "m_spawnJobFactory", typeof (SpawnJob.Factory), true);
      reader.SetField<VehicleDepotBase>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      reader.SetField<VehicleDepotBase>(this, "m_upointsManager", (object) UpointsManager.Deserialize(reader));
      reader.SetField<VehicleDepotBase>(this, "m_vehicleBuffersRegistry", reader.LoadedSaveVersion >= 113 ? (object) reader.ReadGenericAs<IVehicleBuffersRegistry>() : (object) (IVehicleBuffersRegistry) null);
      if (reader.LoadedSaveVersion < 113)
        reader.RegisterResolvedMember<VehicleDepotBase>(this, "m_vehicleBuffersRegistry", typeof (IVehicleBuffersRegistry), true);
      reader.SetField<VehicleDepotBase>(this, "m_vehicleJobs", (object) VehicleJobs.Deserialize(reader));
      reader.SetField<VehicleDepotBase>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      this.VehicleQueue = Mafi.Core.Vehicles.VehicleQueue<Vehicle, VehicleDepotBase>.Deserialize(reader);
      reader.RegisterInitAfterLoad<VehicleDepotBase>(this, "initialize", InitPriority.Normal);
    }

    public enum State
    {
      Idle,
      Paused,
      NotEnoughWorkers,
      NotEnoughPower,
      NotEnoughComputing,
      Working,
    }
  }
}
