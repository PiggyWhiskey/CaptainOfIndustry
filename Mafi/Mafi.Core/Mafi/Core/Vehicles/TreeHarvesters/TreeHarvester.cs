// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.TreeHarvesters.TreeHarvester
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ReadonlyCollections;
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
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Core.Vehicles.Trucks.JobProviders;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.TreeHarvesters
{
  [MemberRemovedInSaveVersion("m_lastUnreachablesCacheFlushStep", 140, typeof (SimStep), 125, false)]
  [MemberRemovedInSaveVersion("m_unreachablesCacheResetAttempts", 140, typeof (int), 125, false)]
  [GenerateSerializer(false, null, 0)]
  public class TreeHarvester : 
    Vehicle,
    IEntityAssignedWithVehicles,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithMaxServiceRadius
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly RelTile1i MAX_SERVICE_DISTANCE;
    public static readonly int NUM_SECTIONS_AT_MAX_TREE_SIZE;
    public static readonly int MIN_SECTIONS_PER_CUT;
    public readonly TreeHarvesterProto Prototype;
    /// <summary>
    /// Set of all assigned trucks that work for this harvester.
    /// </summary>
    private readonly AssignedVehicles<Truck, TruckProto> m_trucks;
    private readonly IJobProvider<TreeHarvester> m_jobProvider;
    private readonly ITreeHarvestingManager m_treeHarvestingManager;
    private readonly IProductsManager m_productsManager;
    private UnreachableTerrainDesignationsManager m_unreachablesManager;
    /// <summary>Tree that is currently being chopped.</summary>
    public TreeData? TreeToBeCut;
    /// <summary>Current truck that is being loaded.</summary>
    public Option<Truck> TruckToBeLoaded;
    private readonly TickTimer m_transitionTimer;
    private readonly RotatingCabinDriver m_cabinDriver;
    private readonly ISimLoopEvents m_simLoopEvents;
    private EntityNotificator m_hasNoTruckAssignedNotif;
    private EntityNotificator m_hasNoTreesToHarvest;
    private bool m_hadTruckWhenSearchingForTrees;
    private TreeHarvesterState m_state;
    private WaitHelper m_waitHelper;
    private readonly TreeHarvesterTruckJobProvider m_trucksJobProvider;

    public static void Serialize(TreeHarvester value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreeHarvester>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreeHarvester.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Percent.Serialize(this.ArmStateChangeSpeedFactor, writer);
      ProductQuantity.Serialize(this.Cargo, writer);
      writer.WriteBool(this.DidNotFindTreeToHarvest);
      Option<Mafi.Core.Buildings.Forestry.ForestryTower>.Serialize(this.ForestryTower, writer);
      Option<TreeProto>.Serialize(this.LastCutTreeProto, writer);
      RotatingCabinDriver.Serialize(this.m_cabinDriver, writer);
      writer.WriteBool(this.m_hadTruckWhenSearchingForTrees);
      EntityNotificator.Serialize(this.m_hasNoTreesToHarvest, writer);
      EntityNotificator.Serialize(this.m_hasNoTruckAssignedNotif, writer);
      writer.WriteGeneric<IJobProvider<TreeHarvester>>(this.m_jobProvider);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteInt((int) this.m_state);
      TickTimer.Serialize(this.m_transitionTimer, writer);
      writer.WriteGeneric<ITreeHarvestingManager>(this.m_treeHarvestingManager);
      AssignedVehicles<Truck, TruckProto>.Serialize(this.m_trucks, writer);
      TreeHarvesterTruckJobProvider.Serialize(this.m_trucksJobProvider, writer);
      UnreachableTerrainDesignationsManager.Serialize(this.m_unreachablesManager, writer);
      WaitHelper.Serialize(this.m_waitHelper, writer);
      writer.WriteInt(this.NumCutsMade);
      writer.WriteInt(this.NumSectionsToMake);
      writer.WriteGeneric<TreeHarvesterProto>(this.Prototype);
      writer.WriteInt((int) this.State);
      SimStep.Serialize(this.StateChangedOnSimStep, writer);
      writer.WriteNullableStruct<TreeData>(this.TreeToBeCut);
      TruckQueue.Serialize(this.TruckQueue, writer);
      Option<Truck>.Serialize(this.TruckToBeLoaded, writer);
    }

    public static TreeHarvester Deserialize(BlobReader reader)
    {
      TreeHarvester treeHarvester;
      if (reader.TryStartClassDeserialization<TreeHarvester>(out treeHarvester))
        reader.EnqueueDataDeserialization((object) treeHarvester, TreeHarvester.s_deserializeDataDelayedAction);
      return treeHarvester;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ArmStateChangeSpeedFactor = Percent.Deserialize(reader);
      this.Cargo = ProductQuantity.Deserialize(reader);
      this.DidNotFindTreeToHarvest = reader.ReadBool();
      this.ForestryTower = Option<Mafi.Core.Buildings.Forestry.ForestryTower>.Deserialize(reader);
      this.LastCutTreeProto = reader.LoadedSaveVersion >= 98 ? Option<TreeProto>.Deserialize(reader) : new Option<TreeProto>();
      reader.SetField<TreeHarvester>(this, "m_cabinDriver", (object) RotatingCabinDriver.Deserialize(reader));
      this.m_hadTruckWhenSearchingForTrees = reader.ReadBool();
      this.m_hasNoTreesToHarvest = EntityNotificator.Deserialize(reader);
      this.m_hasNoTruckAssignedNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<TreeHarvester>(this, "m_jobProvider", (object) reader.ReadGenericAs<IJobProvider<TreeHarvester>>());
      if (reader.LoadedSaveVersion >= 125 && reader.LoadedSaveVersion < 140)
        SimStep.Deserialize(reader);
      reader.SetField<TreeHarvester>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<TreeHarvester>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_state = (TreeHarvesterState) reader.ReadInt();
      reader.SetField<TreeHarvester>(this, "m_transitionTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<TreeHarvester>(this, "m_treeHarvestingManager", (object) reader.ReadGenericAs<ITreeHarvestingManager>());
      reader.SetField<TreeHarvester>(this, "m_trucks", (object) AssignedVehicles<Truck, TruckProto>.Deserialize(reader));
      reader.SetField<TreeHarvester>(this, "m_trucksJobProvider", (object) TreeHarvesterTruckJobProvider.Deserialize(reader));
      if (reader.LoadedSaveVersion >= 125 && reader.LoadedSaveVersion < 140)
        reader.ReadInt();
      this.m_unreachablesManager = UnreachableTerrainDesignationsManager.Deserialize(reader);
      this.m_waitHelper = WaitHelper.Deserialize(reader);
      this.NumCutsMade = reader.ReadInt();
      this.NumSectionsToMake = reader.ReadInt();
      reader.SetField<TreeHarvester>(this, "Prototype", (object) reader.ReadGenericAs<TreeHarvesterProto>());
      this.State = (TreeHarvesterState) reader.ReadInt();
      this.StateChangedOnSimStep = SimStep.Deserialize(reader);
      this.TreeToBeCut = reader.ReadNullableStruct<TreeData>();
      this.TruckQueue = TruckQueue.Deserialize(reader);
      this.TruckToBeLoaded = Option<Truck>.Deserialize(reader);
    }

    public IIndexable<Vehicle> AllVehicles => this.m_trucks.AllUntyped;

    public TreeHarvesterState State
    {
      get => this.m_state;
      private set
      {
        if (value == this.m_state)
          return;
        this.m_state = value;
        this.StateChangedOnSimStep = this.m_simLoopEvents.CurrentStep;
      }
    }

    public SimStep StateChangedOnSimStep { get; private set; }

    public ProductQuantity Cargo { get; private set; }

    [NewInSaveVersion(98, null, null, null, null)]
    public Option<TreeProto> LastCutTreeProto { get; private set; }

    public int NumSectionsToMake { get; private set; }

    public int NumCutsMade { get; private set; }

    /// <summary>
    /// Tower to which is this tree harvester assigned or none.
    /// </summary>
    public Option<Mafi.Core.Buildings.Forestry.ForestryTower> ForestryTower { get; private set; }

    public Duration CurrentStateDuration => this.m_transitionTimer.StartedAtTicks;

    public Duration CurrentStateRemaining => this.m_transitionTimer.Ticks;

    /// <summary>Speed factor used for arm state change.</summary>
    public Percent ArmStateChangeSpeedFactor { get; private set; }

    public AngleDegrees1f CabinDirection => this.m_cabinDriver.CabinDirection;

    public AngleDegrees1f CabinDirectionRelative => this.m_cabinDriver.CabinDirectionRelative;

    public bool IsCabinAtTarget => this.m_cabinDriver.IsCabinAtTarget;

    public Tile2f? CabinTarget => this.m_cabinDriver.CabinTarget;

    /// <summary>Trucks that are waiting to be loaded.</summary>
    public TruckQueue TruckQueue { get; private set; }

    public bool DidNotFindTreeToHarvest { get; set; }

    public RelTile1i MaxServiceRadius => TreeHarvester.MAX_SERVICE_DISTANCE;

    public TreeHarvester(
      EntityId id,
      TreeHarvesterProto prototype,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      TerrainManager terrain,
      IVehiclePathFindingManager pathFinder,
      IVehiclesManager vehiclesManager,
      IFuelStationsManager fuelStationsManager,
      IJobProvider<TreeHarvester> jobProvider,
      TreeHarvesterTruckJobProviderFactory truckJobProviderFactory,
      IVehicleSurfaceProvider surfaceProvider,
      GetUnstuckJob.Factory unstuckJobFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      ITreeHarvestingManager treeHarvestingManager,
      IProductsManager productsManager,
      UnreachableTerrainDesignationsManager unreachablesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CCargo\u003Ek__BackingField = ProductQuantity.None;
      this.m_transitionTimer = new TickTimer();
      this.m_waitHelper = new WaitHelper(int.MaxValue);
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (DrivingEntityProto) prototype, context, terrain, pathFinder, vehiclesManager, surfaceProvider, unstuckJobFactory, maintenanceProvidersFactory);
      this.Prototype = prototype;
      this.m_simLoopEvents = simLoopEvents;
      this.m_jobProvider = jobProvider;
      this.m_treeHarvestingManager = treeHarvestingManager;
      this.m_productsManager = productsManager;
      this.m_unreachablesManager = unreachablesManager;
      this.m_state = TreeHarvesterState.Idle;
      this.StateChangedOnSimStep = simLoopEvents.CurrentStep;
      this.m_cabinDriver = new RotatingCabinDriver(prototype.RotatingCabinDriverProto, (DynamicGroundEntity) this);
      this.m_trucksJobProvider = truckJobProviderFactory.CreateJobProvider(this);
      this.m_trucks = new AssignedVehicles<Truck, TruckProto>((IEntityAssignedWithVehicles) this);
      this.m_hasNoTruckAssignedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoTruckAssignedToTreeHarvester);
      this.m_hasNoTreesToHarvest = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoTreesToHarvest);
      this.SetToAutoRequestRefuelingTrucks(fuelStationsManager);
      this.TruckQueue = new TruckQueue((Vehicle) this);
      this.TruckQueue.Enable();
    }

    protected override void SimUpdateInternal()
    {
      if (!this.IsSpawned && !this.HasJobs)
        return;
      this.m_hasNoTreesToHarvest.NotifyIff(this.IsEnabled && !this.HasFuelNotificationOn && !this.IsStrugglingToNavigate && this.DidNotFindTreeToHarvest && this.m_hadTruckWhenSearchingForTrees && this.AssignedTo.IsNone && this.VehiclesTotal() > 0, (IEntity) this);
      this.m_hasNoTruckAssignedNotif.NotifyIff(this.IsEnabled && this.Cargo.IsNotEmpty && this.VehiclesTotal() == 0, (IEntity) this);
      base.SimUpdateInternal();
      this.m_cabinDriver.Update();
      this.m_transitionTimer.DecrementOnly();
      if (this.TreeToBeCut.HasValue)
      {
        if (this.m_transitionTimer.IsFinished)
          this.State = this.getNextTreeCuttingState();
        this.ConsumeFuelPerUpdate();
      }
      else if (this.TruckToBeLoaded.HasValue)
      {
        if (this.m_transitionTimer.IsFinished)
          this.State = this.getNextUnloadingState();
        this.ConsumeFuelPerUpdate();
      }
      else
      {
        if (this.State == TreeHarvesterState.RaisingTreeUp || this.State == TreeHarvesterState.ReturningFromUnloadWithCargo)
        {
          if (this.m_transitionTimer.IsFinished)
          {
            this.m_transitionTimer.Reset();
            this.State = TreeHarvesterState.TreeIsUp;
          }
          else
          {
            this.ConsumeFuelPerUpdate();
            return;
          }
        }
        if (this.State == TreeHarvesterState.ReturningFromUnloadToIdle)
        {
          if (this.m_transitionTimer.IsFinished)
          {
            this.startTransitionTimer(this.Prototype.HarvestTimings.ToFoldedDuration);
            this.State = TreeHarvesterState.FoldingArm;
          }
          this.ConsumeFuelPerUpdate();
        }
        else
        {
          if (this.State == TreeHarvesterState.FoldingArm)
          {
            if (this.m_transitionTimer.IsFinished)
            {
              this.m_transitionTimer.Reset();
              this.State = TreeHarvesterState.Idle;
            }
            else
            {
              this.ConsumeFuelPerUpdate();
              return;
            }
          }
          if (this.Jobs.IsEmpty && this.m_waitHelper.WaitOne())
          {
            this.ConsumeFuelPerUpdate();
          }
          else
          {
            if (this.DoJob())
              return;
            if (this.IsEnabled && this.IsSpawned && this.m_jobProvider.TryGetJobFor(this))
            {
              this.m_waitHelper.Reset();
            }
            else
            {
              this.DidNotFindTreeToHarvest = true;
              this.m_hadTruckWhenSearchingForTrees = this.m_trucks.Count > 0;
              this.m_unreachablesManager.ReportVehicleIdle((Vehicle) this);
              this.m_waitHelper.StartWait();
              this.m_cabinDriver.ResetCabinTarget();
            }
          }
        }
      }
    }

    public override void EnqueueJob(VehicleJob job, bool enqueueFirst = false)
    {
      this.m_cabinDriver.ResetCabinTarget();
      base.EnqueueJob(job, enqueueFirst);
    }

    public bool TryStartNearbyTreeHarvest(TreeId tree)
    {
      Assert.That<bool>(this.IsDriving).IsFalse();
      Assert.That<TreeData?>(this.TreeToBeCut).IsNone<TreeData>();
      Assert.That<Option<Truck>>(this.TruckToBeLoaded).IsNone<Truck>();
      Assert.That<ProductQuantity>(this.Cargo).IsEmpty();
      Assert.That<TreeHarvesterState>(this.State).IsTrue<TreeHarvesterState>((Predicate<TreeHarvesterState>) (x => x == TreeHarvesterState.Idle || x == TreeHarvesterState.ReturningFromUnloadToIdle));
      Assert.That<long>(tree.Position.AsFull.DistanceSqrTo(this.GroundPositionTile2i)).IsLessOrEqual(TreeHarvester.MAX_SERVICE_DISTANCE.Squared, "Harvested tree too far!");
      TreeData treeData;
      if (!this.m_treeHarvestingManager.TryGetTree(tree, out treeData))
        return false;
      this.TreeToBeCut = new TreeData?(treeData);
      this.TruckToBeLoaded = Option<Truck>.None;
      this.State = TreeHarvesterState.Idle;
      return true;
    }

    /// <summary>
    /// This should be called only when harvester is finished with tree cutting and has cargo
    /// </summary>
    /// <remarks>
    /// This method intentionally does not check all the pre-conditions but only asserts them to avoid live-loops
    /// where buggy jobs could would try to start unloading but due to some failed pre-condition unload would never
    /// actually start.
    /// </remarks>
    public void StartCargoUnloadTo(Truck truck)
    {
      Assert.That<bool>(this.IsDriving).IsFalse();
      Assert.That<TreeData?>(this.TreeToBeCut).IsNone<TreeData>();
      Assert.That<Option<Truck>>(this.TruckToBeLoaded).IsNone<Truck>();
      Assert.That<ProductQuantity>(this.Cargo).IsNotEmpty();
      Assert.That<bool>(this.State == TreeHarvesterState.RaisingTreeUp).Or(this.State == TreeHarvesterState.TreeIsUp).IsTrue();
      Assert.That<Fix64>(truck.Position2f.DistanceSqrTo(this.Position2f)).IsLessOrEqual((Fix64) TreeHarvester.MAX_SERVICE_DISTANCE.Squared, "Truck too far!");
      this.TruckToBeLoaded = (Option<Truck>) truck;
      this.TreeToBeCut = new TreeData?();
    }

    private TreeHarvesterState getNextTreeCuttingState()
    {
      Assert.That<TreeData?>(this.TreeToBeCut).HasValue<TreeData>();
      switch (this.State)
      {
        case TreeHarvesterState.Idle:
        case TreeHarvesterState.ReturningFromUnloadToIdle:
          this.m_cabinDriver.SetCabinTarget(this.TreeToBeCut.Value.Position);
          this.startTransitionTimer(this.Prototype.HarvestTimings.ToPrepareForHarvestDuration);
          return TreeHarvesterState.PositioningArm;
        case TreeHarvesterState.PositioningArm:
          if (!this.m_cabinDriver.IsCabinAtTarget)
            return TreeHarvesterState.PositioningArm;
          this.startTransitionTimer(this.Prototype.HarvestTimings.CuttingDuration);
          this.LastCutTreeProto = (Option<TreeProto>) this.TreeToBeCut.Value.Proto;
          ProductQuantity productQuantity = this.m_treeHarvestingManager.HarvestTree(this.TreeToBeCut.Value.Id);
          Assert.That<ProductQuantity>(this.Cargo).IsEmpty();
          this.Cargo = productQuantity;
          this.NumSectionsToMake = (TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE / TreeHarvester.MIN_SECTIONS_PER_CUT * productQuantity.Quantity.Value.ToFix32() / this.TreeToBeCut.Value.Proto.ProductWhenHarvested.Quantity.Value.ToFix32()).ToIntCeiled();
          this.NumCutsMade = 0;
          return TreeHarvesterState.CuttingTree;
        case TreeHarvesterState.CuttingTree:
          this.startTransitionTimer(this.Prototype.HarvestTimings.ToTreeLayingDownDuration);
          return TreeHarvesterState.LayingTreeDown;
        case TreeHarvesterState.LayingTreeDown:
          this.startTransitionTimer(this.Prototype.HarvestTimings.TrimmingDuration);
          return TreeHarvesterState.BranchTrimming;
        case TreeHarvesterState.BranchTrimming:
          this.startTransitionTimer(this.Prototype.HarvestTimings.ToTreeAboveTruckDuration);
          this.TreeToBeCut = new TreeData?();
          return TreeHarvesterState.RaisingTreeUp;
        default:
          Log.Error(string.Format("Unknown/invalid tree cutting state '{0}'.", (object) this.State));
          this.TreeToBeCut = new TreeData?();
          return !this.Cargo.IsEmpty ? TreeHarvesterState.TreeIsUp : TreeHarvesterState.Idle;
      }
    }

    private TreeHarvesterState getNextUnloadingState()
    {
      Assert.That<Option<Truck>>(this.TruckToBeLoaded).HasValue<Truck>();
      if (this.TruckToBeLoaded.Value.AssignedTo.ValueOrNull != this)
      {
        this.TruckToBeLoaded = Option<Truck>.None;
        this.startTransitionTimer(this.Prototype.HarvestTimings.ToArmUpDuration);
        return TreeHarvesterState.ReturningFromUnloadWithCargo;
      }
      int num = this.NumSectionsToMake - this.NumCutsMade;
      if (num <= 0)
      {
        Log.Error(string.Format("Tree harvester sections left is '{0}'. Should be <= 0", (object) num));
        return TreeHarvesterState.Idle;
      }
      Quantity newQuantity = this.TruckToBeLoaded.Value.RemainingCapacity.Min((this.Cargo.Quantity.Value.ToFix32() / num).ToIntRounded().Quantity());
      switch (this.State)
      {
        case TreeHarvesterState.RaisingTreeUp:
        case TreeHarvesterState.TreeIsUp:
        case TreeHarvesterState.ReturningFromUnloadWithCargo:
          this.m_cabinDriver.SetCabinTarget(this.TruckToBeLoaded.Value.Position2f);
          return TreeHarvesterState.PositioningForUnload;
        case TreeHarvesterState.PositioningForUnload:
          if (!this.m_cabinDriver.IsCabinAtTarget)
            return TreeHarvesterState.PositioningForUnload;
          this.startTransitionTimer(this.Prototype.HarvestTimings.ToTreeOnTruckDuration);
          return TreeHarvesterState.CuttingSection;
        case TreeHarvesterState.UnloadingTree:
          ProductQuantity productQuantity = this.Cargo.WithNewQuantity(this.Cargo.Quantity - newQuantity);
          this.TruckToBeLoaded.Value.LoadCargoReturnExcess(this.Cargo.WithNewQuantity(newQuantity));
          if (num > 1)
            ++this.NumCutsMade;
          this.Cargo = productQuantity;
          if (this.Cargo.Quantity.IsZero || this.TruckToBeLoaded.Value.RemainingCapacity.IsZero)
            this.TruckToBeLoaded = Option<Truck>.None;
          if (this.Cargo.IsEmpty)
          {
            this.startTransitionTimer(this.Prototype.HarvestTimings.ToArmUpDuration);
            return TreeHarvesterState.ReturningFromUnloadToIdle;
          }
          if (this.TruckToBeLoaded.IsNone)
          {
            this.startTransitionTimer(this.Prototype.HarvestTimings.ToArmUpDuration);
            return TreeHarvesterState.ReturningFromUnloadWithCargo;
          }
          this.startTransitionTimer(this.Prototype.HarvestTimings.MoveToNextSectionDuration);
          return TreeHarvesterState.CuttingSection;
        case TreeHarvesterState.CuttingSection:
          this.startTransitionTimer(this.Prototype.HarvestTimings.CutNextSectionDuration);
          return TreeHarvesterState.UnloadingTree;
        default:
          Log.Error(string.Format("Unknown/invalid cargo unloading state '{0}'.", (object) this.State));
          this.TruckToBeLoaded = Option<Truck>.None;
          return !this.Cargo.IsEmpty ? TreeHarvesterState.TreeIsUp : TreeHarvesterState.Idle;
      }
    }

    private void startTransitionTimer(Duration duration)
    {
      this.ArmStateChangeSpeedFactor = this.SpeedFactor;
      this.m_transitionTimer.Start(this.SpeedFactor.ApplyInverseCeiled(duration.Ticks).Ticks());
    }

    public void SetCabinTarget(Tile2f target) => this.m_cabinDriver.SetCabinTarget(target);

    public void ResetCabinTarget() => this.m_cabinDriver.ResetCabinTarget();

    public void MarkTreeChunkAsUnreachable(Chunk2i chunk)
    {
      this.m_unreachablesManager.MarkUnreachableTreeChunkFor(chunk, (IPathFindingVehicle) this);
    }

    protected override void OnAssignTo(IEntityAssignedWithVehicles owner, bool doNotCancelJobs)
    {
      this.ForestryTower = (Option<Mafi.Core.Buildings.Forestry.ForestryTower>) (owner as Mafi.Core.Buildings.Forestry.ForestryTower);
      Assert.That<Option<Mafi.Core.Buildings.Forestry.ForestryTower>>(this.ForestryTower).HasValue<Mafi.Core.Buildings.Forestry.ForestryTower>();
      base.OnAssignTo(owner, doNotCancelJobs);
    }

    public override void UnassignFrom(IEntityAssignedWithVehicles owner, bool cancelJobs = true)
    {
      base.UnassignFrom(owner, cancelJobs);
      this.ForestryTower = (Option<Mafi.Core.Buildings.Forestry.ForestryTower>) Option.None;
    }

    protected override void OnDestroy()
    {
      this.unassignAllVehicles();
      if (this.TruckQueue.IsEnabled)
        this.TruckQueue.Clear();
      base.OnDestroy();
    }

    protected override bool TryRequestScrapInternal()
    {
      if (!base.TryRequestScrapInternal())
        return false;
      this.TruckQueue.Disable();
      this.unassignAllVehicles();
      if (this.Cargo.IsNotEmpty)
        this.clearCargoImmediately();
      this.State = TreeHarvesterState.Idle;
      this.TreeToBeCut = new TreeData?();
      this.TruckToBeLoaded = Option<Truck>.None;
      return true;
    }

    public override bool TryRequestToGoToDepotForReplacement(DrivingEntityProto targetProto)
    {
      if (!base.TryRequestToGoToDepotForReplacement(targetProto))
        return false;
      this.TruckQueue.Disable();
      if (this.Cargo.IsNotEmpty)
        this.clearCargoImmediately();
      this.State = TreeHarvesterState.Idle;
      this.TreeToBeCut = new TreeData?();
      this.TruckToBeLoaded = Option<Truck>.None;
      return true;
    }

    public override void Despawn()
    {
      this.clearCargoImmediately();
      this.m_cabinDriver.Reset();
      if (this.TruckQueue.IsEnabled)
        this.TruckQueue.Clear();
      base.Despawn();
    }

    private void unassignAllVehicles()
    {
      foreach (Vehicle immutable in this.m_trucks.All.ToImmutableArray<Truck>())
        immutable.UnassignFrom((IEntityAssignedWithVehicles) this);
    }

    protected override void OnUpdateSpeedFactor()
    {
      base.OnUpdateSpeedFactor();
      this.m_cabinDriver.SetSpeedFactor(this.SpeedFactor);
    }

    private void clearCargoImmediately()
    {
      if (this.Cargo.IsEmpty)
        return;
      this.m_productsManager.ProductDestroyed(this.Cargo, DestroyReason.Cleared);
      this.Cargo = ProductQuantity.None;
      this.State = TreeHarvesterState.Idle;
      this.m_transitionTimer.Start(Duration.OneTick);
      this.m_transitionTimer.DecrementOnly();
    }

    public bool CanVehicleBeAssigned(DynamicEntityProto vehicle)
    {
      return vehicle is TruckProto truck && this.Prototype.IsTruckSupported(truck);
    }

    public void AssignVehicle(Vehicle vehicle, bool doNotCancelJobs = false)
    {
      if (!(vehicle is Truck vehicle1))
      {
        Log.Error(string.Format("Trying to assign invalid vehicle '{0}'.", (object) vehicle.GetType()));
      }
      else
      {
        if (!this.m_trucks.AssignVehicle(vehicle1, doNotCancelJobs))
          return;
        vehicle1.SetJobProvider((IJobProvider<Truck>) this.m_trucksJobProvider);
      }
    }

    public void UnassignVehicle(Vehicle vehicle, bool cancelJobs = true)
    {
      if (!(vehicle is Truck vehicle1))
      {
        Log.Error(string.Format("Trying to unassign invalid vehicle '{0}'.", (object) vehicle.GetType()));
      }
      else
      {
        if (!this.m_trucks.UnassignVehicle(vehicle1, cancelJobs))
          return;
        vehicle1.ResetJobProvider();
      }
    }

    static TreeHarvester()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreeHarvester.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      TreeHarvester.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      TreeHarvester.MAX_SERVICE_DISTANCE = 10.Tiles();
      TreeHarvester.NUM_SECTIONS_AT_MAX_TREE_SIZE = 12;
      TreeHarvester.MIN_SECTIONS_PER_CUT = 2;
    }
  }
}
