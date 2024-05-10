// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Trucks.Truck
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.PathFinding;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks.JobProviders;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Vehicles.Trucks
{
  /// <summary>Represents an entity that is able to transport cargo.</summary>
  [GenerateSerializer(false, null, 0)]
  public sealed class Truck : Vehicle, IVehicleForCargoJob
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly TruckProto Prototype;
    private readonly IProperty<Percent> m_truckCapacityMultiplier;
    private readonly IProductsManager m_productsManager;
    private EntityNotificatorWithProtoParam m_cannotDeliverNotif;
    [NewInSaveVersion(140, null, null, null, null)]
    private EntityNotificator m_cannotDeliverMixedNotif;
    private EntityNotificator m_hasNoValidExcavatorNotif;
    private readonly WaitHelper m_waitHelper;
    private VehicleCargo m_cargo;
    private readonly DefaultTruckJobProvider m_defaultJobProvider;
    private Option<IJobProvider<Truck>> m_jobProvider;
    /// <summary>
    /// This allows to specify for what type of product is the truck being used for. Examples:
    /// - while the truck is assigned to a FuelStation, it is used to transport fuel of that station
    /// - while the truck is assigned to a MineTower, is is used to transport loose products.
    /// Only purpose of this is to determine what truck attachment to use when there is no cargo.
    /// </summary>
    public Option<ProductProto> DefaultProduct;
    /// <summary>Whether dumping was not completely finished yet.</summary>
    public bool DumpingOfAllCargoPending;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly UnreachableTerrainDesignationsManager m_unreachableDesignationsManager;

    public static void Serialize(Truck value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Truck>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Truck.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Quantity.Serialize(this.Capacity, writer);
      Option<ProductProto>.Serialize(this.DefaultProduct, writer);
      writer.WriteBool(this.DumpingOfAllCargoPending);
      writer.WriteBool(this.IsDumping);
      Option<ILayoutEntity>.Serialize(this.LayoutEntity, writer);
      EntityNotificator.Serialize(this.m_cannotDeliverMixedNotif, writer);
      EntityNotificatorWithProtoParam.Serialize(this.m_cannotDeliverNotif, writer);
      VehicleCargo.Serialize(this.m_cargo, writer);
      DefaultTruckJobProvider.Serialize(this.m_defaultJobProvider, writer);
      EntityNotificator.Serialize(this.m_hasNoValidExcavatorNotif, writer);
      Option<IJobProvider<Truck>>.Serialize(this.m_jobProvider, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      writer.WriteGeneric<IProperty<Percent>>(this.m_truckCapacityMultiplier);
      UnreachableTerrainDesignationsManager.Serialize(this.m_unreachableDesignationsManager, writer);
      WaitHelper.Serialize(this.m_waitHelper, writer);
      writer.WriteGeneric<TruckProto>(this.Prototype);
    }

    public static Truck Deserialize(BlobReader reader)
    {
      Truck truck;
      if (reader.TryStartClassDeserialization<Truck>(out truck))
        reader.EnqueueDataDeserialization((object) truck, Truck.s_deserializeDataDelayedAction);
      return truck;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Capacity = Quantity.Deserialize(reader);
      this.DefaultProduct = Option<ProductProto>.Deserialize(reader);
      this.DumpingOfAllCargoPending = reader.ReadBool();
      this.IsDumping = reader.ReadBool();
      this.LayoutEntity = Option<ILayoutEntity>.Deserialize(reader);
      this.m_cannotDeliverMixedNotif = reader.LoadedSaveVersion >= 140 ? EntityNotificator.Deserialize(reader) : new EntityNotificator();
      this.m_cannotDeliverNotif = EntityNotificatorWithProtoParam.Deserialize(reader);
      this.m_cargo = VehicleCargo.Deserialize(reader);
      reader.SetField<Truck>(this, "m_defaultJobProvider", (object) DefaultTruckJobProvider.Deserialize(reader));
      this.m_hasNoValidExcavatorNotif = EntityNotificator.Deserialize(reader);
      this.m_jobProvider = Option<IJobProvider<Truck>>.Deserialize(reader);
      reader.SetField<Truck>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<Truck>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      reader.SetField<Truck>(this, "m_truckCapacityMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<Truck>(this, "m_unreachableDesignationsManager", (object) UnreachableTerrainDesignationsManager.Deserialize(reader));
      reader.SetField<Truck>(this, "m_waitHelper", (object) WaitHelper.Deserialize(reader));
      reader.SetField<Truck>(this, "Prototype", (object) reader.ReadGenericAs<TruckProto>());
      reader.RegisterInitAfterLoad<Truck>(this, "initSelf", InitPriority.Low);
    }

    public Duration CargoPickupDuration => this.Prototype.CargoPickupDuration;

    /// <summary>
    /// Type of product that this truck supports.
    /// If set, this truck has just one attachment. If null, this truck has multiple switchable attachments.
    /// </summary>
    public Mafi.Core.Products.ProductType? ProductType => this.Prototype.ProductType;

    /// <summary>
    /// Product with quantity that is currently stored in the truck.
    /// </summary>
    public IVehicleCargo Cargo => (IVehicleCargo) this.m_cargo;

    public Quantity TotalCargoQuantity => this.Cargo.TotalQuantity;

    public bool IsEmpty => this.Cargo.IsEmpty;

    public bool IsNotEmpty => this.Cargo.IsNotEmpty;

    public bool IsFull => this.TotalCargoQuantity >= this.Capacity;

    public bool IsNotFull => !this.IsFull;

    [OnlyForSaveCompatibility(null)]
    public Quantity RemainingCapacity
    {
      get => (this.Capacity - this.TotalCargoQuantity).Max(Quantity.Zero);
    }

    public override bool IsIdle => !this.HasTrueJob && this.Cargo.IsEmpty;

    public Quantity Capacity { get; private set; }

    /// <summary>
    /// Whether the truck is currently dumping material on the terrain. Used in graphics to animate the dump bed.
    /// </summary>
    public bool IsDumping { get; set; }

    /// <summary>Layout entity that this truck is assigned to.</summary>
    public Option<ILayoutEntity> LayoutEntity { get; private set; }

    public bool IsCannotDeliverNotificationActive
    {
      get => this.m_cannotDeliverNotif.IsActive || this.m_cannotDeliverMixedNotif.IsActive;
    }

    public Truck(
      EntityId id,
      TruckProto prototype,
      EntityContext context,
      TerrainManager terrainManager,
      IVehiclePathFindingManager pathFinder,
      DefaultTruckJobProvider defaultJobProvider,
      IProductsManager productsManager,
      IVehiclesManager vehiclesManager,
      INotificationsManager notifManager,
      IVehicleSurfaceProvider surfaceProvider,
      GetUnstuckJob.Factory unstuckJobFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      ISimLoopEvents simLoopEvents,
      UnreachableTerrainDesignationsManager unreachableDesignationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_waitHelper = new WaitHelper(int.MaxValue);
      this.m_cargo = new VehicleCargo();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (DrivingEntityProto) prototype, context, terrainManager, pathFinder, vehiclesManager, surfaceProvider, unstuckJobFactory, maintenanceProvidersFactory);
      this.m_simLoopEvents = simLoopEvents;
      this.m_unreachableDesignationsManager = unreachableDesignationsManager;
      this.Prototype = prototype.CheckNotNull<TruckProto>();
      this.m_defaultJobProvider = defaultJobProvider.CheckNotNull<DefaultTruckJobProvider>();
      this.m_productsManager = productsManager.CheckNotNull<IProductsManager>();
      this.m_cannotDeliverNotif = notifManager.CreateNotificatorFor(IdsCore.Notifications.TruckCannotDeliver);
      this.m_cannotDeliverMixedNotif = notifManager.CreateNotificatorFor(IdsCore.Notifications.TruckCannotDeliverMixedCargo);
      this.m_hasNoValidExcavatorNotif = notifManager.CreateNotificatorFor(IdsCore.Notifications.TruckHasNoValidExcavator);
      this.Maintenance.SetExtraMultiplierProperty(context.PropertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.TrucksMaintenanceMultiplier));
      this.m_truckCapacityMultiplier = context.PropertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.TrucksCapacityMultiplier);
      this.m_truckCapacityMultiplier.OnChange.Add<Truck>(this, new Action<Percent>(this.onCapacityMultiplierChange));
      this.onCapacityMultiplierChange(this.m_truckCapacityMultiplier.Value);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf(DependencyResolver resolver, int saveVersion)
    {
      if (this.IsDestroyed && this.m_jobProvider.HasValue)
      {
        Log.Error("Clearing leaked job provider");
        this.m_jobProvider = Option<IJobProvider<Truck>>.None;
      }
      if (saveVersion >= 140)
        return;
      this.m_cannotDeliverMixedNotif = resolver.Resolve<INotificationsManager>().CreateNotificatorFor(IdsCore.Notifications.TruckCannotDeliverMixedCargo);
    }

    private void onCapacityMultiplierChange(Percent newMultiplier)
    {
      this.Capacity = this.Prototype.CapacityBase.ScaledBy(newMultiplier);
    }

    /// <summary>
    /// Takes requested quantity of trucks cargo. Returns amount removed.
    /// </summary>
    public ProductQuantity TakeCargo(ProductQuantity maxTakenQuantity)
    {
      Quantity quantity = this.m_cargo.TryRemoveAsMuchAs(maxTakenQuantity);
      return new ProductQuantity(maxTakenQuantity.Product, quantity);
    }

    /// <summary>
    /// Loads the given cargo. There is no limit enforced as cargo capacity can fluctuate
    /// and enforcement of capacity is hard to deal with at that point.
    /// Returns how much it did not fit (always zero).
    /// </summary>
    public ProductQuantity LoadCargoReturnExcess(ProductQuantity pq)
    {
      Assert.That<bool>(this.IsOnWayToDepotForScrap).IsFalse();
      Assert.That<bool>(this.IsOnWayToDepotForReplacement).IsFalse();
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

    /// <summary>
    /// Loads the as much of given cargo given remaining capacity.
    /// Returns how much was loaded.
    /// </summary>
    /// <param name="pq"></param>
    /// <returns></returns>
    public ProductQuantity LoadAsMuchAs(ProductQuantity pq)
    {
      ProductQuantity pq1 = new ProductQuantity(pq.Product, this.RemainingCapacity.Min(pq.Quantity));
      return this.LoadCargoReturnExcess(pq1).IsNotEmpty ? pq : pq1;
    }

    /// <summary>Sets job provider.</summary>
    public void SetJobProvider(IJobProvider<Truck> jobProvider)
    {
      Assert.That<bool>(this.m_jobProvider.IsNone || object.Equals((object) jobProvider, (object) this.m_jobProvider.Value)).IsTrue();
      this.m_jobProvider = Option.Some<IJobProvider<Truck>>(jobProvider);
    }

    /// <summary>Resets job provider to the default one.</summary>
    public void ResetJobProvider() => this.m_jobProvider = Option<IJobProvider<Truck>>.None;

    protected override void SimUpdateInternal()
    {
      if (!this.IsSpawned && !this.HasJobs)
        return;
      base.SimUpdateInternal();
      if (this.HasJobs)
      {
        this.doJob();
      }
      else
      {
        if (!this.IsEnabled)
          return;
        this.tryGetJob();
      }
    }

    private void tryGetJob()
    {
      Assert.That<bool>(this.IsDriving).IsFalse();
      Assert.That<bool>(this.HasJobs).IsFalse();
      if (!this.IsSpawned || this.m_waitHelper.WaitOne())
        return;
      if (this.m_jobProvider.ValueOr((IJobProvider<Truck>) this.m_defaultJobProvider).TryGetJobFor(this))
      {
        Assert.That<bool>(this.HasJobs).IsTrue();
        this.m_waitHelper.Reset();
      }
      else
      {
        this.m_unreachableDesignationsManager.ReportVehicleIdle((Vehicle) this);
        this.m_waitHelper.StartWait();
      }
    }

    private void doJob()
    {
      if (this.DoJob())
        return;
      this.tryGetJob();
    }

    public Quantity DumpLooseAt(
      Tile2iAndIndex tileAndIndex,
      ProductProto productToDump,
      Truck.DumpTileFn dumpFunc)
    {
      if (this.m_cargo.IsEmpty)
        return Quantity.Zero;
      Assert.That<bool>(this.Cargo.HasProduct(productToDump)).IsTrue();
      return this.dumpLooseAtInternal(tileAndIndex, new ProductQuantity(productToDump, this.Cargo.GetQuantityOf(productToDump)), dumpFunc);
    }

    private Quantity dumpLooseAtInternal(
      Tile2iAndIndex tileAndIndex,
      ProductQuantity pq,
      Truck.DumpTileFn dumpFunc)
    {
      if (pq.Product.DumpableProduct.IsNone)
      {
        Log.Warning(string.Format("Trying to dump non-dumpable product {0}!", (object) pq.Product));
        return Quantity.Zero;
      }
      if (this.Terrain.IsOffLimitsOrInvalid(tileAndIndex.TileCoord))
      {
        Log.Warning("Dumping to off-limits tile.");
        return Quantity.Zero;
      }
      LooseProductQuantity looseProductQuantity = new LooseProductQuantity(pq.Product.DumpableProduct.Value, pq.Quantity);
      TerrainMaterialThickness terrainThickness = looseProductQuantity.ToTerrainThickness();
      ImmutableArray<ThicknessTilesF> thicknessByDistance = this.Prototype.DumpedThicknessByDistance;
      bool flag = dumpFunc(tileAndIndex, ref terrainThickness, thicknessByDistance[0]);
      int num = 1;
      for (int length = thicknessByDistance.Length; num < length & flag; ++num)
      {
        ThicknessTilesF maxDumped = thicknessByDistance[num];
        for (int index = -num; index < num & flag; ++index)
          flag = dumpFunc(this.Terrain.OffsetUnclamped(tileAndIndex, index, num), ref terrainThickness, maxDumped) && dumpFunc(this.Terrain.OffsetUnclamped(tileAndIndex, num, -index), ref terrainThickness, maxDumped) && dumpFunc(this.Terrain.OffsetUnclamped(tileAndIndex, -index, -num), ref terrainThickness, maxDumped) && dumpFunc(this.Terrain.OffsetUnclamped(tileAndIndex, -num, index), ref terrainThickness, maxDumped);
      }
      LooseProductQuantity productQuantityRounded = terrainThickness.ToLooseProductQuantityRounded(looseProductQuantity.Product);
      Quantity removedQuantity = looseProductQuantity.Quantity - productQuantityRounded.Quantity;
      if (removedQuantity.IsNegative)
      {
        Log.Warning("Dumped negative amount.");
        return Quantity.Zero;
      }
      this.m_productsManager.ProductDestroyed(removedQuantity.Of((ProductProto) looseProductQuantity.Product), DestroyReason.DumpedOnTerrain);
      this.m_cargo.RemoveExactly((ProductProto) looseProductQuantity.Product, removedQuantity);
      Assert.That<Quantity>(this.m_cargo.TotalQuantity).IsNotNegative("Dumping produced negative truck quantity.");
      return removedQuantity;
    }

    public void RemoveProductForSurface(ProductQuantity pq)
    {
      Assert.That<ProductProto>(pq.Product).IsNotNullOrPhantom<ProductProto>();
      Quantity quantity = this.m_cargo.TryRemoveAsMuchAs(pq);
      this.m_productsManager.ProductDestroyed(quantity.Of(pq.Product), DestroyReason.DumpedOnTerrain);
      if (quantity != pq.Quantity)
        Log.Error(string.Format("Failed to remove {0} of {1} from truck during surface placement", (object) pq.Quantity, (object) pq.Product));
      Assert.That<Quantity>(this.m_cargo.TotalQuantity).IsNotNegative("Concreting produced negative truck quantity.");
    }

    public void ClearCargoForSortingPlant() => this.m_cargo.Clear();

    public void AddProductFromSurface(ProductQuantity pq)
    {
      Assert.That<ProductProto>(pq.Product).IsNotNullOrPhantom<ProductProto>();
      Quantity quantity = this.Capacity.Min(pq.Quantity);
      if (quantity.IsPositive)
      {
        this.m_cargo.Add(pq.Product, quantity);
        this.m_productsManager.ProductCreated(quantity.Of(pq.Product), CreateReason.MinedFromTerrain);
      }
      if (!(quantity != pq.Quantity))
        return;
      Log.Error(string.Format("Failed to add {0} of {1} to truck during surface removal", (object) pq.Quantity, (object) pq.Product));
    }

    protected override void OnAssignTo(IEntityAssignedWithVehicles owner, bool doNotCancelJobs)
    {
      this.LayoutEntity = (owner as ILayoutEntity).CreateOption<ILayoutEntity>();
      this.m_defaultJobProvider.ClearCachedBuffer(this);
      base.OnAssignTo(owner, doNotCancelJobs);
    }

    public override void UnassignFrom(IEntityAssignedWithVehicles owner, bool cancelJobs = true)
    {
      base.UnassignFrom(owner, cancelJobs);
      this.LayoutEntity = (Option<ILayoutEntity>) Option.None;
      this.m_defaultJobProvider.ClearCachedBuffer(this);
    }

    public void ClearCargoImmediately()
    {
      if (this.m_cargo.IsNotEmpty)
        this.m_cargo.ClearCargoImmediately(this.Context.AssetTransactionManager);
      this.DeactivateCannotDeliver();
    }

    public void ClearCargoImmediately(ProductProto product)
    {
      if (this.m_cargo.IsNotEmpty)
        this.m_cargo.ClearCargoImmediately(this.Context.AssetTransactionManager, product);
      this.DeactivateCannotDeliver();
    }

    public void Cheat_NewProductAndCancelJobs(ProductProto proto, Quantity maxNewQuantity)
    {
      if (maxNewQuantity.IsNotPositive)
        return;
      if (this.Cargo.IsNotEmpty)
        this.ClearCargoImmediately();
      this.CancelAllJobsAndResetState();
      Quantity quantity = maxNewQuantity.Min(this.Capacity);
      this.m_cargo.Add(proto, quantity);
    }

    protected override bool TryRequestScrapInternal()
    {
      if (!base.TryRequestScrapInternal())
        return false;
      this.DeactivateCannotDeliver();
      if (this.Cargo.IsNotEmpty)
        this.ClearCargoImmediately();
      return true;
    }

    public override bool TryRequestToGoToDepotForReplacement(DrivingEntityProto targetProto)
    {
      if (!base.TryRequestToGoToDepotForReplacement(targetProto))
        return false;
      this.DeactivateCannotDeliver();
      if (this.Cargo.IsNotEmpty)
        this.ClearCargoImmediately();
      return true;
    }

    protected override void OnDestroy()
    {
      Assert.That<bool>(this.IsEmpty).IsTrue();
      this.ClearCargoImmediately();
      this.m_jobProvider = Option<IJobProvider<Truck>>.None;
      this.m_truckCapacityMultiplier.OnChange.Remove<Truck>(this, new Action<Percent>(this.onCapacityMultiplierChange));
      base.OnDestroy();
    }

    public PooledArray<ProductQuantity> TryGetCargoToDiscard()
    {
      if (this.Cargo.IsEmpty || !this.IsCannotDeliverNotificationActive)
        return PooledArray<ProductQuantity>.Empty;
      PooledArray<ProductQuantity> pooled = PooledArray<ProductQuantity>.GetPooled(this.m_cargo.Count);
      int i = 0;
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in this.m_cargo)
      {
        pooled[i] = new ProductQuantity(keyValuePair.Key, keyValuePair.Value);
        ++i;
      }
      this.m_cargo.Clear();
      Assert.That<int>(i).IsEqualTo(pooled.Length);
      this.DeactivateCannotDeliver();
      return pooled;
    }

    public void NotifyHasNoValidExcavatorIff(bool shouldNotify)
    {
      this.m_hasNoValidExcavatorNotif.NotifyIff(shouldNotify, (IEntity) this);
    }

    public override string ToString()
    {
      return base.ToString() + ", cargo: " + (this.m_cargo.IsEmpty ? "--" : this.m_cargo.ToString());
    }

    public void NotifyIffCannotDeliver(bool shouldNotify, ProductProto proto)
    {
      if (shouldNotify && this.m_cannotDeliverMixedNotif.IsActive)
        return;
      this.m_cannotDeliverNotif.NotifyIff((Proto) proto, shouldNotify, (IEntity) this);
    }

    public void NotifyIffCannotDeliverMixed(bool shouldNotify)
    {
      this.m_cannotDeliverMixedNotif.NotifyIff(shouldNotify, (IEntity) this);
      this.m_cannotDeliverNotif.Deactivate((IEntity) this);
    }

    public void DeactivateCannotDeliver()
    {
      this.m_cannotDeliverNotif.Deactivate((IEntity) this);
      this.m_cannotDeliverMixedNotif.Deactivate((IEntity) this);
    }

    public void AssignBalancingJob(BalancingJobSpec spec)
    {
      this.m_defaultJobProvider.AssignBalancingJob(spec);
    }

    /// <summary>
    /// Called anytime after truck registers for a balancing job.
    /// </summary>
    public bool IsAvailableToBalanceCargo()
    {
      return !this.IsNotEnabled && !this.IsDestroyed && !this.IsOnWayToDepotForScrap && !this.Cargo.IsNotEmpty && !this.HasTrueJob && (!this.m_jobProvider.HasValue || this.m_jobProvider.Value == this.m_defaultJobProvider) && !this.HasTrueJob;
    }

    static Truck()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Truck.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Truck.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public delegate bool DumpTileFn(
      Tile2iAndIndex tileAndIndex,
      ref TerrainMaterialThickness product,
      ThicknessTilesF maxDumped);
  }
}
