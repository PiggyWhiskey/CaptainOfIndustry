// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.TreePlanters.TreePlanter
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.TreePlanters
{
  [GenerateSerializer(false, null, 0)]
  public class TreePlanter : Vehicle, IVehicleForCargoJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public static readonly RelTile1i MAX_SERVICE_DISTANCE;
    public readonly TreePlanterProto Prototype;
    private VehicleCargo m_cargo;
    private readonly IJobProvider<TreePlanter> m_jobProvider;
    private readonly ITreePlantingManager m_treePlantingManager;
    private readonly IProductsManager m_productsManager;
    [NewInSaveVersion(102, null, null, typeof (UnreachableTerrainDesignationsManager), null)]
    private readonly UnreachableTerrainDesignationsManager m_unreachablesManager;
    [NewInSaveVersion(102, null, "new WaitHelper(int.MaxValue)", null, null)]
    private WaitHelper m_waitHelper;
    /// <summary>Planting data. Location and proto.</summary>
    public Pair<Tile2f, TreeProto>? PlantingData;
    private readonly TickTimer m_transitionTimer;
    private readonly RotatingCabinDriver m_cabinDriver;
    private readonly ISimLoopEvents m_simLoopEvents;
    private EntityNotificator m_hasNoSaplingsNotif;
    private TreePlanterState m_state;

    public static void Serialize(TreePlanter value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TreePlanter>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TreePlanter.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Percent.Serialize(this.ArmStateChangeSpeedFactor, writer);
      Option<Mafi.Core.Buildings.Forestry.ForestryTower>.Serialize(this.ForestryTower, writer);
      RotatingCabinDriver.Serialize(this.m_cabinDriver, writer);
      VehicleCargo.Serialize(this.m_cargo, writer);
      EntityNotificator.Serialize(this.m_hasNoSaplingsNotif, writer);
      writer.WriteGeneric<IJobProvider<TreePlanter>>(this.m_jobProvider);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteInt((int) this.m_state);
      TickTimer.Serialize(this.m_transitionTimer, writer);
      writer.WriteGeneric<ITreePlantingManager>(this.m_treePlantingManager);
      UnreachableTerrainDesignationsManager.Serialize(this.m_unreachablesManager, writer);
      WaitHelper.Serialize(this.m_waitHelper, writer);
      writer.WriteNullableStruct<Pair<Tile2f, TreeProto>>(this.PlantingData);
      writer.WriteGeneric<TreePlanterProto>(this.Prototype);
      writer.WriteInt((int) this.State);
      SimStep.Serialize(this.StateChangedOnSimStep, writer);
    }

    public static TreePlanter Deserialize(BlobReader reader)
    {
      TreePlanter treePlanter;
      if (reader.TryStartClassDeserialization<TreePlanter>(out treePlanter))
        reader.EnqueueDataDeserialization((object) treePlanter, TreePlanter.s_deserializeDataDelayedAction);
      return treePlanter;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ArmStateChangeSpeedFactor = Percent.Deserialize(reader);
      this.ForestryTower = Option<Mafi.Core.Buildings.Forestry.ForestryTower>.Deserialize(reader);
      reader.SetField<TreePlanter>(this, "m_cabinDriver", (object) RotatingCabinDriver.Deserialize(reader));
      this.m_cargo = VehicleCargo.Deserialize(reader);
      this.m_hasNoSaplingsNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<TreePlanter>(this, "m_jobProvider", (object) reader.ReadGenericAs<IJobProvider<TreePlanter>>());
      reader.SetField<TreePlanter>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<TreePlanter>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_state = (TreePlanterState) reader.ReadInt();
      reader.SetField<TreePlanter>(this, "m_transitionTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<TreePlanter>(this, "m_treePlantingManager", (object) reader.ReadGenericAs<ITreePlantingManager>());
      reader.SetField<TreePlanter>(this, "m_unreachablesManager", reader.LoadedSaveVersion >= 102 ? (object) UnreachableTerrainDesignationsManager.Deserialize(reader) : (object) (UnreachableTerrainDesignationsManager) null);
      if (reader.LoadedSaveVersion < 102)
        reader.RegisterResolvedMember<TreePlanter>(this, "m_unreachablesManager", typeof (UnreachableTerrainDesignationsManager), true);
      this.m_waitHelper = reader.LoadedSaveVersion >= 102 ? WaitHelper.Deserialize(reader) : new WaitHelper(int.MaxValue);
      this.PlantingData = reader.ReadNullableStruct<Pair<Tile2f, TreeProto>>();
      reader.SetField<TreePlanter>(this, "Prototype", (object) reader.ReadGenericAs<TreePlanterProto>());
      this.State = (TreePlanterState) reader.ReadInt();
      this.StateChangedOnSimStep = SimStep.Deserialize(reader);
    }

    public Duration CargoPickupDuration => this.Prototype.CargoPickupDuration;

    /// <summary>Type of product that this tree planter supports.</summary>
    public ProductProto ProductProto => this.Prototype.ProductProto;

    /// <summary>
    /// Product with quantity that is currently stored in the planter.
    /// </summary>
    public IVehicleCargo Cargo => (IVehicleCargo) this.m_cargo;

    public bool IsEmpty => this.Cargo.IsEmpty;

    public bool IsNotEmpty => this.Cargo.IsNotEmpty;

    public bool IsFull => this.Cargo.TotalQuantity >= this.Capacity;

    public bool IsNotFull => !this.IsFull;

    public Quantity RemainingCapacity
    {
      get => (this.Capacity - this.Cargo.TotalQuantity).Max(Quantity.Zero);
    }

    public Quantity Capacity => this.Prototype.Capacity;

    public TreePlanterState State
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

    /// <summary>Tower to which is this tree planter assigned or none.</summary>
    public Option<Mafi.Core.Buildings.Forestry.ForestryTower> ForestryTower { get; private set; }

    public Duration CurrentStateDuration => this.m_transitionTimer.StartedAtTicks;

    public Duration CurrentStateRemaining => this.m_transitionTimer.Ticks;

    /// <summary>Speed factor used for arm state change.</summary>
    public Percent ArmStateChangeSpeedFactor { get; private set; }

    public AngleDegrees1f CabinDirectionRelative => this.m_cabinDriver.CabinDirectionRelative;

    public TreePlanter(
      EntityId id,
      TreePlanterProto prototype,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      TerrainManager terrain,
      IVehiclePathFindingManager pathFinder,
      IVehiclesManager vehiclesManager,
      IFuelStationsManager fuelStationsManager,
      IJobProvider<TreePlanter> jobProvider,
      IVehicleSurfaceProvider surfaceProvider,
      GetUnstuckJob.Factory unstuckJobFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      ITreePlantingManager treePlantingManager,
      IProductsManager productsManager,
      UnreachableTerrainDesignationsManager unreachablesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_cargo = new VehicleCargo();
      this.m_waitHelper = new WaitHelper(int.MaxValue);
      this.m_transitionTimer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (DrivingEntityProto) prototype, context, terrain, pathFinder, vehiclesManager, surfaceProvider, unstuckJobFactory, maintenanceProvidersFactory);
      this.Prototype = prototype;
      this.m_simLoopEvents = simLoopEvents;
      this.m_jobProvider = jobProvider;
      this.m_treePlantingManager = treePlantingManager;
      this.m_productsManager = productsManager;
      this.m_unreachablesManager = unreachablesManager;
      this.m_state = TreePlanterState.Idle;
      this.StateChangedOnSimStep = simLoopEvents.CurrentStep;
      this.m_cabinDriver = new RotatingCabinDriver(prototype.RotatingCabinDriverProto, (DynamicGroundEntity) this);
      this.m_hasNoSaplingsNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoTreeSaplingsForPlanter);
      this.SetToAutoRequestRefuelingTrucks(fuelStationsManager);
    }

    /// <summary>
    /// Loads the given cargo. There is no limit enforced as cargo capacity can fluctuate
    /// and enforcement of capacity is hard to deal with at that point.
    /// Returns how much it did not fit.
    /// </summary>
    public ProductQuantity LoadCargoReturnExcess(ProductQuantity pq)
    {
      Assert.That<Quantity>(pq.Quantity).IsNotNegative();
      Assert.That<bool>(this.IsOnWayToDepotForScrap).IsFalse();
      Assert.That<bool>(this.IsOnWayToDepotForReplacement).IsFalse();
      if ((Proto) pq.Product != (Proto) this.ProductProto)
      {
        Log.Error(string.Format("Loading incorrect cargo {0} on a planter which only supports {1}.", (object) pq.Product, (object) this.ProductProto));
        return pq;
      }
      if (pq.Quantity.IsNotPositive)
      {
        Log.Warning(string.Format("Loading empty cargo {0} on a truck.", (object) pq));
        return pq;
      }
      if (!this.Cargo.CanAdd(pq.Product))
      {
        Log.Error(string.Format("Cannot add {0} to a truck with {1}.", (object) pq, (object) this.Cargo));
        return pq;
      }
      this.m_cargo.Add(pq);
      return ProductQuantity.NoneOf(pq.Product);
    }

    protected override void SimUpdateInternal()
    {
      this.m_hasNoSaplingsNotif.NotifyIff(this.IsSpawned && this.IsEnabled && !this.HasJobs && this.m_cargo.IsEmpty, (IEntity) this);
      if (!this.IsSpawned && !this.HasJobs)
        return;
      base.SimUpdateInternal();
      this.m_cabinDriver.Update();
      this.m_transitionTimer.DecrementOnly();
      if (this.PlantingData.HasValue)
      {
        if (this.m_transitionTimer.IsFinished)
          this.State = this.getNextTreePlantingState();
        this.ConsumeFuelPerUpdate();
        if (this.State != TreePlanterState.Idle)
          return;
      }
      if (this.State == TreePlanterState.ReturningToIdle)
      {
        if (this.m_transitionTimer.IsFinished)
        {
          this.m_transitionTimer.Reset();
          this.State = TreePlanterState.Idle;
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
          this.m_unreachablesManager.ReportVehicleIdle((Vehicle) this);
          this.m_waitHelper.StartWait();
          this.m_cabinDriver.ResetCabinTarget();
        }
      }
    }

    public override void EnqueueJob(VehicleJob job, bool enqueueFirst = false)
    {
      this.m_cabinDriver.ResetCabinTarget();
      base.EnqueueJob(job, enqueueFirst);
    }

    public bool TryStartNearbyTreePlant(Tile2i targetPosition, TreeProto proto)
    {
      Assert.That<bool>(this.IsDriving).IsFalse();
      Assert.That<bool>(this.IsNotEmpty).IsTrue();
      Assert.That<TreePlanterState>(this.State).IsTrue<TreePlanterState>((Predicate<TreePlanterState>) (x => x == TreePlanterState.Idle));
      Assert.That<long>(targetPosition.DistanceSqrTo(this.GroundPositionTile2i)).IsLessOrEqual(TreePlanter.MAX_SERVICE_DISTANCE.Squared, "Planting tree too far!");
      this.PlantingData = new Pair<Tile2f, TreeProto>?(Pair.Create<Tile2f, TreeProto>(targetPosition.CenterTile2f, proto));
      this.State = TreePlanterState.Idle;
      return true;
    }

    private TreePlanterState getNextTreePlantingState()
    {
      Assert.That<Pair<Tile2f, TreeProto>?>(this.PlantingData).HasValue<Pair<Tile2f, TreeProto>>();
      Tile2f first = this.PlantingData.Value.First;
      TreeProto second = this.PlantingData.Value.Second;
      switch (this.State)
      {
        case TreePlanterState.Idle:
          this.m_cabinDriver.SetCabinTarget(first);
          this.startTransitionTimer(this.Prototype.PlantingTimings.PlantingDuration);
          return TreePlanterState.PlantingTree;
        case TreePlanterState.PlantingTree:
          AngleSlim plantingRotation = this.m_treePlantingManager.GetRandomPlantingRotation();
          if (!this.m_treePlantingManager.TryPlantTree(second, first, plantingRotation))
            Log.Error(string.Format("Failed to plant tree '{0}'.", (object) first));
          this.PlantingData = new Pair<Tile2f, TreeProto>?();
          Assert.That<bool>(this.IsNotEmpty).IsTrue();
          ProductProto product = this.m_cargo.FirstOrPhantom.Product;
          if (product.IsNotPhantom && this.IsNotEmpty)
            this.Context.ProductsManager.ProductDestroyed(product, Quantity.One, DestroyReason.General);
          else
            Log.Error("Got a phantom product or empty cargo instead of a sapling!");
          this.m_cargo.RemoveExactly(product, Quantity.One);
          this.startTransitionTimer(this.Prototype.PlantingTimings.ReturningToIdleDuration);
          return TreePlanterState.ReturningToIdle;
        case TreePlanterState.ReturningToIdle:
          return TreePlanterState.Idle;
        default:
          Log.Error(string.Format("Unknown/invalid tree planting state '{0}'.", (object) this.State));
          this.PlantingData = new Pair<Tile2f, TreeProto>?();
          return TreePlanterState.Idle;
      }
    }

    private void startTransitionTimer(Duration duration)
    {
      this.ArmStateChangeSpeedFactor = this.SpeedFactor;
      this.m_transitionTimer.Start(this.SpeedFactor.ApplyInverseCeiled(duration.Ticks).Ticks());
    }

    public void SetCabinTarget(Tile2f target) => this.m_cabinDriver.SetCabinTarget(target);

    public void ResetCabinTarget() => this.m_cabinDriver.ResetCabinTarget();

    /// <summary>Planting tile this planter has a job to service.</summary>
    public Tile2i? GetReservedPlantingTile()
    {
      if (!this.IsEnabled)
        return new Tile2i?();
      if (this.Cargo.IsEmpty)
        return new Tile2i?();
      return !(this.Jobs.CurrentJob.ValueOrNull is TreePlantingJob valueOrNull) ? new Tile2i?() : new Tile2i?(valueOrNull.PlantingPosition);
    }

    /// <summary>Planting tile this planter has a job to service.</summary>
    [CanBeNull]
    public TreeProto GetJobTreeProto()
    {
      if (!this.IsEnabled)
        return (TreeProto) null;
      if (this.Cargo.IsEmpty)
        return (TreeProto) null;
      return !(this.Jobs.CurrentJob.ValueOrNull is TreePlantingJob valueOrNull) ? (TreeProto) null : valueOrNull.PlantingProto;
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
      base.OnDestroy();
      Assert.That<bool>(this.Cargo.IsEmpty).IsTrue();
    }

    protected override bool TryRequestScrapInternal()
    {
      if (!base.TryRequestScrapInternal())
        return false;
      if (this.Cargo.IsNotEmpty)
        this.clearCargoImmediately();
      return true;
    }

    public override bool TryRequestToGoToDepotForReplacement(DrivingEntityProto targetProto)
    {
      if (!base.TryRequestToGoToDepotForReplacement(targetProto))
        return false;
      if (this.Cargo.IsNotEmpty)
        this.clearCargoImmediately();
      return true;
    }

    public override void Despawn()
    {
      this.clearCargoImmediately();
      this.m_cabinDriver.Reset();
      base.Despawn();
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
      this.m_productsManager.ProductDestroyed(this.Cargo.FirstOrPhantom, DestroyReason.Cleared);
      this.m_cargo.ClearCargoImmediately(this.Context.AssetTransactionManager, this.Cargo.FirstOrPhantom.Product);
      this.State = TreePlanterState.Idle;
      this.m_transitionTimer.Start(Duration.OneTick);
      this.m_transitionTimer.DecrementOnly();
    }

    public bool CanVehicleBeAssigned(DynamicEntityProto vehicle) => false;

    static TreePlanter()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TreePlanter.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      TreePlanter.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      TreePlanter.MAX_SERVICE_DISTANCE = 10.Tiles();
    }
  }
}
