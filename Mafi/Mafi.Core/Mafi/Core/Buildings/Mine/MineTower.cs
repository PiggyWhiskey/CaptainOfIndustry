// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Mine.MineTower
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.FuelStations;
using Mafi.Core.Buildings.OreSorting;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Core.Vehicles.Trucks.JobProviders;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Mine
{
  [GenerateSerializer(false, null, 0)]
  public class MineTower : 
    LayoutEntity,
    IAreaManagingTower,
    IEntity,
    IIsSafeAsHashKey,
    IAssignableToFuelStation,
    IEntityAssignedAsOutput,
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IEntityAssignedAsInput,
    IEntityAssignedWithVehicles,
    IEntityWithCloneableConfig
  {
    public readonly MineTowerProto Prototype;
    private readonly Set<IEntityAssignedAsInput> m_assignedInputEntities;
    private readonly Set<IEntityAssignedAsOutput> m_assignedOutputEntities;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<IEntityAssignedAsInput> m_assignedInputStorages;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<OreSortingPlant> m_assignedInputOreSorters;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Set<IEntityAssignedAsOutput> m_assignedOutputStorages;
    [DoNotSaveCreateNewOnLoad(null, 140)]
    private readonly Set<FuelStation> m_assignedFuelStations;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<MineTower> m_assignedInputTowers;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<MineTower> m_assignedOutputTowers;
    private readonly Set<TerrainDesignation> m_managedDesignations;
    private readonly Set<ProductProto> m_dumpableProducts;
    private readonly Set<ProductProto> m_productsToNotifyIfCannotGetRidOf;
    private readonly AssignedVehicles<Excavator, ExcavatorProto> m_excavators;
    private readonly AssignedVehicles<Truck, TruckProto> m_trucks;
    private readonly MineTowerTruckJobProvider m_trucksJobProvider;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly Lyst<Vehicle> m_allVehicles;
    private readonly ITerrainDesignationsManager m_designationManager;
    private readonly ICalendar m_calendar;
    private Option<MineTowersManager> m_manager;
    private EntityNotificator m_noMineDesignationsNotif;
    private EntityNotificator m_noAvailableMineDesignationsNotif;
    private EntityNotificator m_cannotDumpNotif;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    /// <summary>Whether any entity accepts our stuff.</summary>
    public bool HasInputStorageOrTowerAssigned
    {
      get
      {
        return this.AssignedInputStorages.IsNotEmpty<IEntityAssignedAsInput>() || this.AssignedInputTowers.IsNotEmpty<MineTower>();
      }
    }

    /// <summary>Whether any entity delivers stuff to this tower.</summary>
    public bool HasOutputStorageOrTowerAssigned
    {
      get
      {
        return this.AssignedOutputStorages.IsNotEmpty<IEntityAssignedAsOutput>() || this.AssignedOutputTowers.IsNotEmpty<MineTower>();
      }
    }

    public IReadOnlySet<IEntityAssignedAsInput> AssignedInputs
    {
      get => (IReadOnlySet<IEntityAssignedAsInput>) this.m_assignedInputEntities;
    }

    public IReadOnlySet<IEntityAssignedAsOutput> AssignedOutputs
    {
      get => (IReadOnlySet<IEntityAssignedAsOutput>) this.m_assignedOutputEntities;
    }

    /// <summary>
    /// Storages that accept stuff from this tower as their input.
    /// </summary>
    public IReadOnlySet<IEntityAssignedAsInput> AssignedInputStorages
    {
      get => (IReadOnlySet<IEntityAssignedAsInput>) this.m_assignedInputStorages;
    }

    /// <summary>
    /// Ore sorting plants that accept stuff from this tower as their input.
    /// </summary>
    public IReadOnlySet<OreSortingPlant> AssignedInputOreSorters
    {
      get => (IReadOnlySet<OreSortingPlant>) this.m_assignedInputOreSorters;
    }

    /// <summary>
    /// Storages that provide stuff to this tower as their output.
    /// </summary>
    public IReadOnlySet<IEntityAssignedAsOutput> AssignedOutputStorages
    {
      get => (IReadOnlySet<IEntityAssignedAsOutput>) this.m_assignedOutputStorages;
    }

    public bool AllowNonAssignedOutput { get; set; }

    public IReadOnlySet<FuelStation> AssignedFuelStations
    {
      get => (IReadOnlySet<FuelStation>) this.m_assignedFuelStations;
    }

    public IIndexable<MineTower> AssignedInputTowers
    {
      get => (IIndexable<MineTower>) this.m_assignedInputTowers;
    }

    public IIndexable<MineTower> AssignedOutputTowers
    {
      get => (IIndexable<MineTower>) this.m_assignedOutputTowers;
    }

    /// <summary>
    /// Designations managed by this tower. We don't exclude fulfilled designations.
    /// </summary>
    public IReadOnlySet<TerrainDesignation> ManagedDesignations
    {
      get => (IReadOnlySet<TerrainDesignation>) this.m_managedDesignations;
    }

    /// <summary>
    /// Products that are allowed to be dumped in the given area's designators.
    /// </summary>
    public IReadOnlySet<ProductProto> DumpableProducts
    {
      get => (IReadOnlySet<ProductProto>) this.m_dumpableProducts;
    }

    /// <summary>
    /// Products on which we notify if we have trucks that can't get rid of them.
    /// </summary>
    public IReadOnlySet<ProductProto> ProductsToNotifyIfCannotGetRidOf
    {
      get => (IReadOnlySet<ProductProto>) this.m_productsToNotifyIfCannotGetRidOf;
    }

    /// <summary>Area that is controlled by the mine tower.</summary>
    public RectangleTerrainArea2i Area { get; private set; }

    /// <summary>Total number of assigned tree harvesters.</summary>
    public int AssignedExcavatorsTotal => this.m_excavators.Count;

    public IIndexable<Excavator> AllAssignedExcavators => this.m_excavators.All;

    /// <summary>Total number of assigned trucks.</summary>
    public int AssignedTrucksTotal => this.m_trucks.Count;

    public IIndexable<Vehicle> AllVehicles => (IIndexable<Vehicle>) this.m_allVehicles;

    public MineTower(
      EntityId id,
      MineTowerProto mineTowerProto,
      TileTransform transform,
      EntityContext context,
      ProtosDb protosDb,
      ITerrainDumpingManager dumpingManager,
      IMineTowerTruckJobProviderFactory truckJobProviderFactory,
      IVehiclesManager vehiclesManager,
      ITerrainDesignationsManager designationManager,
      ICalendar calendar,
      MineTowersManager manager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_assignedInputEntities = new Set<IEntityAssignedAsInput>();
      this.m_assignedOutputEntities = new Set<IEntityAssignedAsOutput>();
      this.m_assignedInputStorages = new Set<IEntityAssignedAsInput>();
      this.m_assignedInputOreSorters = new Set<OreSortingPlant>();
      this.m_assignedOutputStorages = new Set<IEntityAssignedAsOutput>();
      this.m_assignedFuelStations = new Set<FuelStation>();
      this.m_assignedInputTowers = new Lyst<MineTower>();
      this.m_assignedOutputTowers = new Lyst<MineTower>();
      this.m_managedDesignations = new Set<TerrainDesignation>();
      this.m_dumpableProducts = new Set<ProductProto>();
      this.m_allVehicles = new Lyst<Vehicle>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) mineTowerProto, transform, context);
      this.Prototype = mineTowerProto;
      this.m_designationManager = designationManager;
      this.m_calendar = calendar;
      this.m_manager = (Option<MineTowersManager>) manager;
      this.m_noMineDesignationsNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoMineDesignInTowerArea);
      this.m_noAvailableMineDesignationsNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.NoAvailableMineDesignInTowerArea);
      this.m_cannotDumpNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CannotDeliverFromMineTower);
      this.m_designationManager.DesignationAdded.Add<MineTower>(this, new Action<TerrainDesignation>(this.designationAdded));
      this.m_designationManager.DesignationRemoved.Add<MineTower>(this, new Action<TerrainDesignation>(this.designationRemoved));
      this.m_designationManager.DesignationFulfilledChanged.Add<MineTower>(this, new Action<TerrainDesignation>(this.designationFulfilledChanged));
      this.m_calendar.NewDay.Add<MineTower>(this, new Action(this.onNewDay));
      this.m_dumpableProducts.AddRange((IEnumerable<ProductProto>) dumpingManager.ProductsAllowedToDump);
      this.m_productsToNotifyIfCannotGetRidOf = protosDb.Filter<ProductProto>((Func<ProductProto, bool>) (x => x.TryGetParam<NotifyIfCannotDumpFromTowerParam>(out NotifyIfCannotDumpFromTowerParam _))).ToSet<ProductProto>();
      this.m_excavators = new AssignedVehicles<Excavator, ExcavatorProto>((IEntityAssignedWithVehicles) this);
      this.m_trucks = new AssignedVehicles<Truck, TruckProto>((IEntityAssignedWithVehicles) this);
      this.m_trucksJobProvider = truckJobProviderFactory.CreateJobProvider(this, this.m_excavators);
      this.updateNotifications();
      this.calcInitialArea();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(int saveVersion)
    {
      foreach (IEntityAssignedAsInput assignedInputEntity in this.m_assignedInputEntities)
      {
        switch (assignedInputEntity)
        {
          case Storage storage:
            this.m_assignedInputStorages.Add((IEntityAssignedAsInput) storage);
            continue;
          case MineTower mineTower:
            this.m_assignedInputTowers.Add(mineTower);
            continue;
          case OreSortingPlant oreSortingPlant:
            this.m_assignedInputOreSorters.Add(oreSortingPlant);
            continue;
          default:
            continue;
        }
      }
      foreach (IEntityAssignedAsOutput assignedOutputEntity in this.m_assignedOutputEntities)
      {
        switch (assignedOutputEntity)
        {
          case Storage storage:
            this.m_assignedOutputStorages.Add((IEntityAssignedAsOutput) storage);
            continue;
          case MineTower mineTower:
            this.m_assignedOutputTowers.Add(mineTower);
            continue;
          case FuelStation fuelStation:
            this.m_assignedFuelStations.Add(fuelStation);
            continue;
          default:
            continue;
        }
      }
      this.updateAssignedVehicles();
    }

    private void onNewDay()
    {
      bool shouldNotify = false;
      foreach (Truck truck in this.m_trucks.All)
      {
        if (shouldNotifyCannotDumpFor(truck))
        {
          shouldNotify = true;
          break;
        }
      }
      this.m_cannotDumpNotif.NotifyIff(shouldNotify, (IEntity) this);

      bool shouldNotifyCannotDumpFor(Truck truck)
      {
        if (!truck.IsCannotDeliverNotificationActive || !truck.Cargo.IsNotEmpty)
          return false;
        foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
        {
          if (!this.m_productsToNotifyIfCannotGetRidOf.Contains(keyValuePair.Key))
            return false;
        }
        return true;
      }
    }

    /// <summary>
    /// Whether this mine tower is currently able to accept the given product to dump. NOTE: this does not take into
    /// account if there are some dumping designators available.
    /// </summary>
    public bool CanAcceptDumpOf(ProductProto product)
    {
      return this.IsEnabled && this.m_dumpableProducts.Contains(product);
    }

    /// <summary>
    /// Makes the given product to be allowed to be dumped here.
    /// </summary>
    public void AddProductToDump(LooseProductProto product)
    {
      Assert.That<LooseProductProto>(product).IsNotNullOrPhantom<LooseProductProto>();
      if (!product.CanBeOnTerrain)
      {
        Log.Warning(string.Format("The given product '{0}' cannot be on terrain!", (object) product.Id));
      }
      else
      {
        if (this.m_dumpableProducts.Add((ProductProto) product))
          return;
        Log.Warning(string.Format("Trying to make dumpable an already dumpable product '{0}'", (object) product.Id));
      }
    }

    /// <summary>
    /// Removes the given product from the allowed dumpable products.
    /// </summary>
    public void RemoveProductToDump(LooseProductProto product)
    {
      Assert.That<LooseProductProto>(product).IsNotNullOrPhantom<LooseProductProto>();
      if (this.m_dumpableProducts.Remove((ProductProto) product))
        return;
      Log.Warning(string.Format("Trying to remove dumpable product '{0}' that was not dumpable!", (object) product.Id));
    }

    public void AddProductToNotifyIfCannotGetRidOff(LooseProductProto product)
    {
      Assert.That<LooseProductProto>(product).IsNotNullOrPhantom<LooseProductProto>();
      this.m_productsToNotifyIfCannotGetRidOf.AddAndAssertNew((ProductProto) product);
    }

    public void RemoveProductToNotifyIfCannotGetRidOff(LooseProductProto product)
    {
      Assert.That<LooseProductProto>(product).IsNotNullOrPhantom<LooseProductProto>();
      this.m_productsToNotifyIfCannotGetRidOf.RemoveAndAssert((ProductProto) product);
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (this.IsNotEnabled)
      {
        foreach (Vehicle vehicle in this.m_excavators.All)
          vehicle.CancelAllJobsAndResetState();
      }
      this.updateNotifications();
    }

    protected override void OnDestroy()
    {
      foreach (Vehicle immutable in this.m_allVehicles.ToImmutableArray())
        this.UnassignVehicle(immutable, true);
      this.m_designationManager.DesignationAdded.Remove<MineTower>(this, new Action<TerrainDesignation>(this.designationAdded));
      this.m_designationManager.DesignationRemoved.Remove<MineTower>(this, new Action<TerrainDesignation>(this.designationRemoved));
      this.m_designationManager.DesignationFulfilledChanged.Remove<MineTower>(this, new Action<TerrainDesignation>(this.designationFulfilledChanged));
      this.m_calendar.NewDay.Remove<MineTower>(this, new Action(this.onNewDay));
      this.m_assignedInputEntities.ForEachAndClear((Action<IEntityAssignedAsInput>) (x => x.UnassignStaticOutputEntity((IEntityAssignedAsOutput) this)));
      this.m_assignedOutputEntities.ForEachAndClear((Action<IEntityAssignedAsOutput>) (x => x.UnassignStaticInputEntity((IEntityAssignedAsInput) this)));
      this.m_assignedFuelStations.Clear();
      this.m_assignedOutputStorages.Clear();
      this.m_assignedInputStorages.Clear();
      this.m_assignedInputTowers.Clear();
      this.m_assignedOutputTowers.Clear();
      this.cleanManagedDesignations();
      this.m_manager = Option<MineTowersManager>.None;
      base.OnDestroy();
    }

    public bool CanBeAssignedWithInput(IEntityAssignedAsInput entity)
    {
      if (entity == this || this.m_assignedInputEntities.Contains(entity))
        return false;
      switch (entity)
      {
        case Storage storage:
          ProductType? productType = storage.Prototype.ProductType;
          if (!productType.HasValue)
            return true;
          productType = storage.Prototype.ProductType;
          return productType.Value == LooseProductProto.ProductType;
        case MineTower _:
          return true;
        default:
          return entity is OreSortingPlant;
      }
    }

    public bool CanBeAssignedWithOutput(IEntityAssignedAsOutput entity)
    {
      if (entity == this || this.m_assignedOutputEntities.Contains(entity))
        return false;
      switch (entity)
      {
        case FuelStation _:
          return true;
        case Storage storage:
          ProductType? productType = storage.Prototype.ProductType;
          if (!productType.HasValue)
            return true;
          productType = storage.Prototype.ProductType;
          return productType.Value == LooseProductProto.ProductType;
        default:
          return entity is MineTower;
      }
    }

    void IEntityAssignedAsOutput.AssignStaticInputEntity(IEntityAssignedAsInput entity)
    {
      if (!this.CanBeAssignedWithInput(entity))
        return;
      this.m_assignedInputEntities.Add(entity);
      switch (entity)
      {
        case MineTower mineTower:
          this.m_assignedInputTowers.Add(mineTower);
          break;
        case Storage storage:
          this.m_assignedInputStorages.Add((IEntityAssignedAsInput) storage);
          break;
        case OreSortingPlant oreSortingPlant:
          this.m_assignedInputOreSorters.Add(oreSortingPlant);
          break;
      }
    }

    void IEntityAssignedAsInput.AssignStaticOutputEntity(IEntityAssignedAsOutput entity)
    {
      if (!this.CanBeAssignedWithOutput(entity))
        return;
      this.m_assignedOutputEntities.Add(entity);
      switch (entity)
      {
        case MineTower mineTower:
          this.m_assignedOutputTowers.Add(mineTower);
          break;
        case FuelStation fuelStation:
          this.m_assignedFuelStations.Add(fuelStation);
          break;
        case Storage storage:
          this.m_assignedOutputStorages.Add((IEntityAssignedAsOutput) storage);
          break;
      }
    }

    void IEntityAssignedAsOutput.UnassignStaticInputEntity(IEntityAssignedAsInput entity)
    {
      this.m_assignedInputEntities.Remove(entity);
      switch (entity)
      {
        case MineTower mineTower:
          this.m_assignedInputTowers.Remove(mineTower);
          break;
        case Storage storage:
          this.m_assignedInputStorages.Remove((IEntityAssignedAsInput) storage);
          break;
        case OreSortingPlant oreSortingPlant:
          this.m_assignedInputOreSorters.Remove(oreSortingPlant);
          break;
      }
    }

    void IEntityAssignedAsInput.UnassignStaticOutputEntity(IEntityAssignedAsOutput entity)
    {
      this.m_assignedOutputEntities.Remove(entity);
      switch (entity)
      {
        case MineTower mineTower:
          this.m_assignedOutputTowers.Remove(mineTower);
          break;
        case FuelStation fuelStation:
          this.m_assignedFuelStations.Remove(fuelStation);
          break;
        case Storage storage:
          this.m_assignedOutputStorages.Remove((IEntityAssignedAsOutput) storage);
          break;
      }
    }

    public void SetNewArea(RectangleTerrainArea2i area)
    {
      this.cleanManagedDesignations();
      RectangleTerrainArea2i area1 = this.Area;
      Tile2i origin = TerrainDesignation.GetOrigin(area.Origin);
      Tile2i tile2i = area.PlusXyCoordExcl;
      tile2i = tile2i.AddX(area.Size.X == 0 ? 0 : -1);
      Tile2i p2 = TerrainDesignation.GetOrigin(tile2i.AddY(area.Size.Y == 0 ? 0 : -1)) + TerrainDesignation.Size - 1;
      this.Area = RectangleTerrainArea2i.FromTwoPositions(origin, p2);
      foreach (TerrainDesignation designation in (IEnumerable<TerrainDesignation>) this.m_designationManager.Designations)
        this.designationAdded(designation);
      if (!this.m_manager.HasValue)
        return;
      this.m_manager.Value.InvokeOnAreaChanged(this, area1);
    }

    private void calcInitialArea()
    {
      RelTile2i origin = this.Prototype.Area.Origin;
      RelTile2i relTile2i1 = new RelTile2i(0, this.Prototype.Area.InitialSize.X / 2);
      RelTile2i relTile2i2 = new RelTile2i(this.Prototype.Area.InitialSize.Y, 0);
      Tile2i absolute1 = this.relAreaToAbsolute(origin + relTile2i1);
      Tile2i absolute2 = this.relAreaToAbsolute(origin + relTile2i2 + relTile2i1);
      Tile2i absolute3 = this.relAreaToAbsolute(origin + relTile2i2 - relTile2i1);
      Tile2i absolute4 = this.relAreaToAbsolute(origin - relTile2i1);
      Tile2i tile2i = absolute1.Min(absolute2);
      tile2i = tile2i.Min(absolute3);
      this.SetNewArea(new RectangleTerrainArea2i(tile2i.Min(absolute4), this.Prototype.Area.InitialSize));
    }

    private Tile2i relAreaToAbsolute(RelTile2i tile)
    {
      return this.Prototype.Layout.Transform(tile, this.Transform);
    }

    private void designationAdded(TerrainDesignation designation)
    {
      if (!designation.Prototype.IsTerraforming || !this.isWithinArea((IDesignation) designation))
        return;
      designation.AddManagingTower((IAreaManagingTower) this);
      this.m_managedDesignations.AddAndAssertNew(designation);
      this.updateNotifications();
    }

    private void designationRemoved(TerrainDesignation designation)
    {
      if (!designation.Prototype.IsTerraforming || !this.m_managedDesignations.Remove(designation))
        return;
      Assert.That<bool>(this.isWithinArea((IDesignation) designation)).IsTrue();
      designation.RemoveManagingTower((IAreaManagingTower) this);
      this.updateNotifications();
    }

    private void designationFulfilledChanged(TerrainDesignation designation)
    {
      if (!designation.Prototype.IsTerraforming || !this.m_managedDesignations.Contains(designation) || !designation.Prototype.ShouldUpdateTowerNotificationOnFulfilledChanged)
        return;
      Assert.That<bool>(this.isWithinArea((IDesignation) designation)).IsTrue();
      this.updateNotifications();
    }

    private void cleanManagedDesignations()
    {
      foreach (TerrainDesignation designation in this.m_managedDesignations.ToArray())
        this.designationRemoved(designation);
      Assert.That<Set<TerrainDesignation>>(this.m_managedDesignations).IsEmpty<TerrainDesignation>();
    }

    private bool isWithinArea(IDesignation designation)
    {
      return this.Area.ContainsTile(designation.OriginTileCoord);
    }

    private void updateNotifications()
    {
      if (this.m_excavators.Count == 0 || this.IsPaused)
      {
        this.m_noMineDesignationsNotif.Deactivate((IEntity) this);
        this.m_noAvailableMineDesignationsNotif.Deactivate((IEntity) this);
      }
      else
      {
        int num1 = 0;
        int num2 = 0;
        foreach (TerrainDesignation managedDesignation in this.m_managedDesignations)
        {
          if (managedDesignation.Prototype.Id == IdsCore.TerrainDesignators.MiningDesignator)
          {
            ++num1;
            if (managedDesignation.IsNotFulfilled && managedDesignation.IsReadyToBeFulfilled)
            {
              ++num2;
              break;
            }
          }
          else if (managedDesignation.Prototype.Id == IdsCore.TerrainDesignators.LevelDesignator && managedDesignation.IsMiningNotFulfilled)
          {
            ++num1;
            if (managedDesignation.IsMiningReadyToBeFulfilled)
            {
              ++num2;
              break;
            }
          }
        }
        if (num1 == 0)
        {
          this.m_noMineDesignationsNotif.Activate((IEntity) this);
          this.m_noAvailableMineDesignationsNotif.Deactivate((IEntity) this);
        }
        else if (num2 == 0)
        {
          this.m_noMineDesignationsNotif.Deactivate((IEntity) this);
          this.m_noAvailableMineDesignationsNotif.Activate((IEntity) this);
        }
        else
        {
          this.m_noMineDesignationsNotif.Deactivate((IEntity) this);
          this.m_noAvailableMineDesignationsNotif.Deactivate((IEntity) this);
        }
      }
    }

    public bool CanVehicleBeAssigned(DynamicEntityProto vehicleProto)
    {
      switch (vehicleProto)
      {
        case ExcavatorProto _:
          return true;
        case TruckProto truckProto:
          return !truckProto.ProductType.HasValue || truckProto.ProductType.Value == LooseProductProto.ProductType;
        default:
          return false;
      }
    }

    private void updateAssignedVehicles()
    {
      this.m_allVehicles.Clear();
      this.m_allVehicles.AddRange(this.m_trucks.AllUntyped);
      this.m_allVehicles.AddRange(this.m_excavators.AllUntyped);
      this.updateNotifications();
    }

    void IEntityWithCloneableConfig.AddToConfig(EntityConfigData data)
    {
      data.SetDumpableProducts(new ImmutableArray<ProductProto>?(this.m_dumpableProducts.ToImmutableArray<ProductProto>()));
      data.SetNotifyOnProducts(new ImmutableArray<ProductProto>?(this.m_productsToNotifyIfCannotGetRidOf.ToImmutableArray<ProductProto>()));
      data.AssignedInputs = new ImmutableArray<EntityId>?(this.AssignedInputs.Select<IEntityAssignedAsInput, EntityId>((Func<IEntityAssignedAsInput, EntityId>) (x => x.Id)).ToImmutableArray<EntityId>());
      data.AssignedOutputs = new ImmutableArray<EntityId>?(this.AssignedOutputs.Select<IEntityAssignedAsOutput, EntityId>((Func<IEntityAssignedAsOutput, EntityId>) (x => x.Id)).ToImmutableArray<EntityId>());
    }

    void IEntityWithCloneableConfig.ApplyConfig(EntityConfigData data)
    {
      ImmutableArray<ProductProto>? dumpableProducts = data.GetDumpableProducts();
      if (dumpableProducts.HasValue)
      {
        this.m_dumpableProducts.Clear();
        this.m_dumpableProducts.AddRange(dumpableProducts.Value);
      }
      ImmutableArray<ProductProto>? notifyOnProducts = data.GetNotifyOnProducts();
      if (!notifyOnProducts.HasValue)
        return;
      this.m_productsToNotifyIfCannotGetRidOf.Clear();
      this.m_productsToNotifyIfCannotGetRidOf.AddRange(notifyOnProducts.Value);
    }

    public void AssignVehicle(Vehicle vehicle, bool doNotCancelJobs = false)
    {
      switch (vehicle)
      {
        case Truck truck:
          if (!this.m_trucks.AssignVehicle(truck, doNotCancelJobs))
            break;
          this.onTruckAssigned(truck);
          break;
        case Excavator excavator:
          if (!this.m_excavators.AssignVehicle(excavator, doNotCancelJobs))
            break;
          this.onExcavatorAssigned(excavator);
          break;
      }
    }

    public void UnassignVehicle(Vehicle vehicle, bool cancelJobs = true)
    {
      switch (vehicle)
      {
        case Truck truck:
          if (!this.m_trucks.UnassignVehicle(truck, cancelJobs))
            break;
          this.onTruckUnassigned(truck);
          break;
        case Excavator excavator:
          if (!this.m_excavators.UnassignVehicle(excavator, cancelJobs))
            break;
          this.onExcavatorUnassigned(excavator);
          break;
      }
    }

    private void onExcavatorAssigned(Excavator excavator)
    {
      this.m_trucksJobProvider.OnExcavatorAssigned(excavator);
      this.updateIncompatibleVehiclesNotifs();
      this.updateAssignedVehicles();
    }

    private void onExcavatorUnassigned(Excavator excavator)
    {
      this.m_trucksJobProvider.OnExcavatorUnassigned(excavator);
      this.updateIncompatibleVehiclesNotifs();
      excavator.NotifyHasNoValidTruckIff(false);
      this.updateAssignedVehicles();
    }

    private void onTruckAssigned(Truck truck)
    {
      truck.SetJobProvider((IJobProvider<Truck>) this.m_trucksJobProvider);
      truck.DefaultProduct = (Option<ProductProto>) (ProductProto) this.Prototype.DefaultProductOfAssignedTrucks.ValueOrNull;
      this.updateIncompatibleVehiclesNotifs();
      this.updateAssignedVehicles();
    }

    private void onTruckUnassigned(Truck truck)
    {
      truck.ResetJobProvider();
      truck.DefaultProduct = (Option<ProductProto>) Option.None;
      this.updateIncompatibleVehiclesNotifs();
      truck.NotifyHasNoValidExcavatorIff(false);
      this.updateAssignedVehicles();
    }

    private void updateIncompatibleVehiclesNotifs()
    {
      foreach (Excavator excavator in this.m_excavators.All)
        excavator.NotifyHasNoValidTruckIff(!hasValidTruck(excavator));
      foreach (Truck truck in this.m_trucks.All)
        truck.NotifyHasNoValidExcavatorIff(!hasValidExcavator(truck));

      bool hasValidTruck(Excavator excavator)
      {
        foreach (Truck truck in this.m_trucks.All)
        {
          if (excavator.Prototype.IsTruckSupported(truck.Prototype))
            return true;
        }
        return false;
      }

      bool hasValidExcavator(Truck truck)
      {
        foreach (Excavator excavator in this.m_excavators.All)
        {
          if (excavator.Prototype.IsTruckSupported(truck.Prototype))
            return true;
        }
        return false;
      }
    }

    public static void Serialize(MineTower value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MineTower>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MineTower.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.AllowNonAssignedOutput);
      RectangleTerrainArea2i.Serialize(this.Area, writer);
      Set<IEntityAssignedAsInput>.Serialize(this.m_assignedInputEntities, writer);
      Set<IEntityAssignedAsOutput>.Serialize(this.m_assignedOutputEntities, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      EntityNotificator.Serialize(this.m_cannotDumpNotif, writer);
      writer.WriteGeneric<ITerrainDesignationsManager>(this.m_designationManager);
      Set<ProductProto>.Serialize(this.m_dumpableProducts, writer);
      AssignedVehicles<Excavator, ExcavatorProto>.Serialize(this.m_excavators, writer);
      Set<TerrainDesignation>.Serialize(this.m_managedDesignations, writer);
      Option<MineTowersManager>.Serialize(this.m_manager, writer);
      EntityNotificator.Serialize(this.m_noAvailableMineDesignationsNotif, writer);
      EntityNotificator.Serialize(this.m_noMineDesignationsNotif, writer);
      Set<ProductProto>.Serialize(this.m_productsToNotifyIfCannotGetRidOf, writer);
      AssignedVehicles<Truck, TruckProto>.Serialize(this.m_trucks, writer);
      MineTowerTruckJobProvider.Serialize(this.m_trucksJobProvider, writer);
      writer.WriteGeneric<MineTowerProto>(this.Prototype);
    }

    public static MineTower Deserialize(BlobReader reader)
    {
      MineTower mineTower;
      if (reader.TryStartClassDeserialization<MineTower>(out mineTower))
        reader.EnqueueDataDeserialization((object) mineTower, MineTower.s_deserializeDataDelayedAction);
      return mineTower;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AllowNonAssignedOutput = reader.ReadBool();
      this.Area = RectangleTerrainArea2i.Deserialize(reader);
      reader.SetField<MineTower>(this, "m_allVehicles", (object) new Lyst<Vehicle>());
      reader.SetField<MineTower>(this, "m_assignedFuelStations", (object) new Set<FuelStation>());
      if (reader.LoadedSaveVersion < 140)
        Set<FuelStation>.Deserialize(reader);
      reader.SetField<MineTower>(this, "m_assignedInputEntities", (object) Set<IEntityAssignedAsInput>.Deserialize(reader));
      reader.SetField<MineTower>(this, "m_assignedInputOreSorters", (object) new Set<OreSortingPlant>());
      reader.SetField<MineTower>(this, "m_assignedInputStorages", (object) new Set<IEntityAssignedAsInput>());
      reader.SetField<MineTower>(this, "m_assignedInputTowers", (object) new Lyst<MineTower>());
      reader.SetField<MineTower>(this, "m_assignedOutputEntities", (object) Set<IEntityAssignedAsOutput>.Deserialize(reader));
      reader.SetField<MineTower>(this, "m_assignedOutputStorages", (object) new Set<IEntityAssignedAsOutput>());
      reader.SetField<MineTower>(this, "m_assignedOutputTowers", (object) new Lyst<MineTower>());
      reader.SetField<MineTower>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      this.m_cannotDumpNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<MineTower>(this, "m_designationManager", (object) reader.ReadGenericAs<ITerrainDesignationsManager>());
      reader.SetField<MineTower>(this, "m_dumpableProducts", (object) Set<ProductProto>.Deserialize(reader));
      reader.SetField<MineTower>(this, "m_excavators", (object) AssignedVehicles<Excavator, ExcavatorProto>.Deserialize(reader));
      reader.SetField<MineTower>(this, "m_managedDesignations", (object) Set<TerrainDesignation>.Deserialize(reader));
      this.m_manager = Option<MineTowersManager>.Deserialize(reader);
      this.m_noAvailableMineDesignationsNotif = EntityNotificator.Deserialize(reader);
      this.m_noMineDesignationsNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<MineTower>(this, "m_productsToNotifyIfCannotGetRidOf", (object) Set<ProductProto>.Deserialize(reader));
      reader.SetField<MineTower>(this, "m_trucks", (object) AssignedVehicles<Truck, TruckProto>.Deserialize(reader));
      reader.SetField<MineTower>(this, "m_trucksJobProvider", (object) MineTowerTruckJobProvider.Deserialize(reader));
      reader.SetField<MineTower>(this, "Prototype", (object) reader.ReadGenericAs<MineTowerProto>());
      reader.RegisterInitAfterLoad<MineTower>(this, "initSelf", InitPriority.Normal);
    }

    static MineTower()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MineTower.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      MineTower.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
