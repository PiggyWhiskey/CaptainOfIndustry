// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Cargo.Ships.CargoShip
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.Buildings.Cargo.Ships.Modules;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Stats;
using Mafi.Core.Terrain;
using Mafi.Core.Utils;
using Mafi.Core.World;
using Mafi.Core.World.Contracts;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.Cargo.Ships
{
  [GenerateSerializer(false, null, 0)]
  [MemberRemovedInSaveVersion("FuelProto", 140, typeof (ProductProto), 0, false)]
  public class CargoShip : 
    Entity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithPosition,
    IRenderedEntity,
    IEntityWithSimUpdate,
    IAreaSelectableEntity,
    IEntityWithCustomTitle,
    IEntityWithGeneralPriorityFriend
  {
    private static readonly Percent POLLUTION_MULT;
    public static readonly Percent SAVER_FUEL_MULT;
    public static readonly Percent SAVER_TRAVEL_DURATION_MULT;
    private static readonly Duration WORLD_DELAY_ONE_WAY;
    private static readonly Duration TIME_BETWEEN_DEPARTURE_CHECKS;
    private static readonly Percent MINIMAL_CAPACITY_UTILIZATION_FOR_DEPARTURE;
    private static readonly Upoints UPOINTS_PER_BASE_IF_NO_FUEL;
    private static readonly Upoints UPOINTS_PER_MODULE_IF_NO_FUEL;
    public readonly CargoShipProto Prototype;
    private readonly TickTimer m_timer;
    private readonly Lyst<CargoShipModule> m_nonEmptyModules;
    private readonly Lyst<Option<CargoShipModule>> m_modules;
    private bool m_isFuelReducedForCurrentJourney;
    [DoNotSave(0, null)]
    private bool m_canUseOnUnityIfOutOfFuel;
    [NewInSaveVersion(140, null, null, null, null)]
    private bool m_departedWithLowFuel;
    private ProductBuffer m_fuelBuffer;
    [NewInSaveVersion(140, null, null, null, null)]
    public Option<ProductProto> PendingFuelToChangeTo;
    [NewInSaveVersion(140, null, "AssetValue.Empty", null, null)]
    private AssetValue m_pendingFuelChangeCost;
    [DoNotSave(0, null)]
    private CargoShipProto.FuelData m_fuelData;
    private CargoShip.ForceLeaveMode m_forceLeaveMode;
    private bool m_upgradeConstructionPending;
    private bool m_departedForContract;
    private bool m_wasLackingUpointsAfterLastCheck;
    private readonly IProperty<Percent> m_fuelConsumptionMultiplier;
    private readonly IFuelStatsCollector m_fuelStatsCollector;
    private readonly WorldMapCargoManager m_worldMapCargoManager;
    private readonly ContractsManager m_contractsManager;
    private readonly ICargoShipFactory m_cargoShipFactory;
    private readonly CargoDepotManager m_cargoDepotManager;
    [DoNotSave(0, null)]
    public Action<CargoShipModule, int> OnModuleAdded;
    [DoNotSave(0, null)]
    public Action<int> OnModuleRemoved;
    private EntityNotificator m_noUpointsNotif;
    private EntityNotificator m_lowFuelNotif;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public Option<string> CustomTitle { get; set; }

    public Duration RemainingTransitionDuration { get; private set; }

    public CargoShip.ShipState State { get; private set; }

    public CargoShip.DockedStatus LastDockedStatus { get; private set; }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IIndexable<CargoShipModule> NonEmptyModules
    {
      get => (IIndexable<CargoShipModule>) this.m_nonEmptyModules;
    }

    public IIndexable<Option<CargoShipModule>> Modules
    {
      get => (IIndexable<Option<CargoShipModule>>) this.m_modules;
    }

    public Duration JourneyDuration
    {
      get
      {
        return (2 * CargoShip.WORLD_DELAY_ONE_WAY + 2 * this.Prototype.DockTransitionDuration).ScaledBy(this.IsFuelReductionEnabled ? CargoShip.SAVER_TRAVEL_DURATION_MULT : Percent.Hundred);
      }
    }

    public Duration DockTransitionDurationForCurrentJourney
    {
      get => this.Prototype.DockTransitionDuration.ScaledBy(this.DurationMultForCurrentJourney);
    }

    private Percent DurationMultForCurrentJourney
    {
      get
      {
        return !this.m_isFuelReducedForCurrentJourney ? Percent.Hundred : CargoShip.SAVER_TRAVEL_DURATION_MULT;
      }
    }

    public bool IsFuelReductionEnabled { get; private set; }

    [NewInSaveVersion(140, null, null, null, null)]
    public bool CanPayWithUnityIfOutOfFuel { get; private set; }

    public CargoDepot CargoDepot { get; private set; }

    public ProductProto FuelProto => this.m_fuelBuffer.Product;

    public IProductBufferReadOnly FuelBuffer => (IProductBufferReadOnly) this.m_fuelBuffer;

    public Tile2f Position2f { get; private set; }

    public Tile3f Position3f => this.Position2f.ExtendZ(Fix32.Zero);

    public AngleDegrees1f Direction { get; private set; }

    public bool IsDocked => this.State == CargoShip.ShipState.Docked;

    public bool DepartureRequestedByPlayer { get; private set; }

    [DoNotSave(0, null)]
    ulong IRenderedEntity.RendererData { get; set; }

    public int GeneralPriority { get; private set; }

    public virtual bool IsCargoAffectedByGeneralPriority => false;

    public virtual bool IsGeneralPriorityVisible => this.IsPriorityVisibleByDefault();

    void IEntityWithGeneralPriorityFriend.SetGeneralPriorityInternal(int priority)
    {
      this.GeneralPriority = priority;
      this.NotifyOnGeneralPriorityChange();
    }

    public CargoShip(
      EntityId id,
      CargoShipProto prototype,
      ProductProto fuelProto,
      EntityContext context,
      CargoDepot cargoDepot,
      CargoDepotManager cargoDepotManager,
      WorldMapCargoManager worldMapCargoManager,
      ContractsManager contractsManager,
      ICargoShipFactory cargoShipFactory,
      IFuelStatsCollector fuelStatsCollector)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_timer = new TickTimer();
      this.m_nonEmptyModules = new Lyst<CargoShipModule>();
      this.m_modules = new Lyst<Option<CargoShipModule>>();
      this.m_pendingFuelChangeCost = AssetValue.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (EntityProto) prototype, context);
      this.Prototype = prototype;
      this.m_cargoDepotManager = cargoDepotManager;
      this.m_worldMapCargoManager = worldMapCargoManager;
      this.m_contractsManager = contractsManager;
      this.m_cargoShipFactory = cargoShipFactory;
      this.m_fuelStatsCollector = fuelStatsCollector;
      this.GeneralPriority = prototype.Costs.DefaultPriority;
      this.CargoDepot = cargoDepot;
      this.updateProperties();
      this.m_noUpointsNotif = this.Context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CargoShipContractLacksUpoints);
      this.m_lowFuelNotif = this.Context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CargoShipMissingFuel);
      this.m_fuelConsumptionMultiplier = this.Context.PropertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.ShipsFuelConsumptionMultiplier);
      this.m_fuelConsumptionMultiplier.OnChange.Add<CargoShip>(this, new Action<Percent>(this.onFuelMultiplierChange));
      int num = this.Prototype.MaximumModulesCount.Min(this.CargoDepot.SlotCount);
      for (int slot = 0; slot < num; ++slot)
      {
        Option<CargoDepotModule> module = this.CargoDepot.GetModule(slot);
        this.m_modules.Add(module.HasValue ? this.createModule(module.Value) : (Option<CargoShipModule>) Option.None);
      }
      this.updateNonEmptyModules();
      this.loadFuelDataFor(fuelProto);
      this.m_fuelBuffer = new ProductBuffer(this.GetFuelReserveNeeded(), fuelProto);
      this.State = CargoShip.ShipState.ArrivingFromWorld;
      this.RemainingTransitionDuration = this.DockTransitionDurationForCurrentJourney;
      this.computeShipPose();
      this.CargoDepot.OnModuleAdded.Add<CargoShip>(this, new Action<CargoDepotModule, int>(this.onDepotModuleAdded));
      this.CargoDepot.OnModuleRemoved.Add<CargoShip>(this, new Action<int>(this.onDepotModuleRemoved));
    }

    [InitAfterLoad(InitPriority.High)]
    private void initSelfHigh()
    {
      this.loadFuelDataFor(this.m_fuelBuffer.Product);
      this.m_fuelBuffer.IncreaseCapacityTo(this.GetFuelReserveNeeded());
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf() => this.updateProperties();

    private void loadFuelDataFor(ProductProto fuel)
    {
      this.m_fuelData = this.Prototype.AvailableFuels.FirstOrDefault((Func<CargoShipProto.FuelData, bool>) (x => (Proto) x.FuelProto == (Proto) fuel));
      if (this.m_fuelData != null)
        return;
      Log.Error("Fuel data not found!");
      this.m_fuelData = this.Prototype.AvailableFuels.First;
    }

    private void updateProperties()
    {
      this.m_canUseOnUnityIfOutOfFuel = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.ShipsCanUseUnityIfOutOfFuel);
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    public void RequestDeparture() => this.DepartureRequestedByPlayer = true;

    public void SetFuelReductionEnabled(bool isEnabled) => this.IsFuelReductionEnabled = isEnabled;

    public void SetPayWithUnityIfOutOfFuel(bool payWithUnity)
    {
      if (payWithUnity && !this.m_canUseOnUnityIfOutOfFuel)
        return;
      this.CanPayWithUnityIfOutOfFuel = payWithUnity;
    }

    private void onFuelMultiplierChange(Percent newValue)
    {
      this.m_fuelBuffer.IncreaseCapacityTo(this.GetFuelReserveNeeded());
    }

    private void computeShipPose()
    {
      Tile2f tile2f = this.CargoDepot.Prototype.Layout.TransformF_Point(this.CargoDepot.Prototype.ModuleSlots.First.Origin.RelTile2f.AddY(this.CargoDepot.Prototype.ModuleSlots.Sum((Func<CargoDepotProto.ModuleSlotPosition, int>) (x => x.SlotSize.Y)) / 2.ToFix32()), this.CargoDepot.Transform);
      RelTile2f relTile2f = this.Prototype.DockOffset.Rotate(Rotation90.Deg90);
      relTile2f = new RelTile2f(this.CargoDepot.Transform.TransformMatrix.Transform(relTile2f.Vector2f));
      this.Position2f = tile2f + relTile2f;
      this.Direction = this.CargoDepot.Transform.Rotation.Angle + AngleDegrees1f.Deg90;
    }

    private Option<CargoShipModule> createModule(CargoDepotModule cargoDepotModule)
    {
      CargoShipModuleProto prototype = this.Prototype.AvailableModules.FirstOrDefault((Func<CargoShipModuleProto, bool>) (x => x.ProductType == cargoDepotModule.Prototype.ProductType));
      if (!((Proto) prototype == (Proto) null))
        return (Option<CargoShipModule>) new CargoShipModule(prototype, this, cargoDepotModule);
      Log.Error("Corresponding module not found!");
      return (Option<CargoShipModule>) Option.None;
    }

    private void onDepotModuleRemoved(int slot)
    {
      if (this.m_upgradeConstructionPending)
        return;
      if (slot < 0 || slot >= this.m_modules.Count)
      {
        Log.Error(string.Format("Received slot at invalid index {0}, while there is {1} ship modules", (object) slot, (object) this.m_modules.Count));
      }
      else
      {
        Option<CargoShipModule> module = this.m_modules[slot];
        if (module.IsNone)
        {
          Log.Error(string.Format("No existing ship module for depot in slot {0}", (object) slot));
        }
        else
        {
          ((ICargoShipModuleFriend) module.Value).Destroy();
          this.m_modules[slot] = Option<CargoShipModule>.None;
          this.updateNonEmptyModules();
          Action<int> onModuleRemoved = this.OnModuleRemoved;
          if (onModuleRemoved == null)
            return;
          onModuleRemoved(slot);
        }
      }
    }

    private void onDepotModuleAdded(CargoDepotModule module, int slot)
    {
      if (this.m_upgradeConstructionPending)
        return;
      if (slot < 0 || slot >= this.m_modules.Count)
        Log.Error(string.Format("CargoShip slot index '{0}' is out of range, has '{1}' modules", (object) slot, (object) this.m_modules.Count));
      else if (this.m_modules[slot].HasValue)
      {
        Log.Error(string.Format("Cannot register cargo depot module {0}, ship already has module {1} assigned", (object) module.Prototype.Id, (object) this.m_modules[slot].Value.DepotModule.Id));
      }
      else
      {
        Option<CargoShipModule> module1 = this.createModule(module);
        if (module1.IsNone)
          return;
        this.m_modules[slot] = module1;
        this.updateNonEmptyModules();
        Action<CargoShipModule, int> onModuleAdded = this.OnModuleAdded;
        if (onModuleAdded == null)
          return;
        onModuleAdded(module1.Value, slot);
      }
    }

    private void updateNonEmptyModules()
    {
      this.m_nonEmptyModules.Clear();
      foreach (Option<CargoShipModule> module in this.m_modules)
      {
        if (module.HasValue)
          this.m_nonEmptyModules.Add(module.Value);
      }
    }

    public Quantity FuelPerJourneyNeeded() => this.FuelPerJourneyNeeded(this.m_fuelData);

    public Quantity FuelPerJourneyNeeded(CargoShipProto.FuelData fuelData)
    {
      Quantity quantity = (fuelData.FuelPerJourneyBase + this.NonEmptyModules.Count * fuelData.FuelPerJourneyPerModule).ScaledBy(this.m_fuelConsumptionMultiplier.Value);
      return this.IsFuelReductionEnabled ? quantity.ScaledBy(CargoShip.SAVER_FUEL_MULT) : quantity;
    }

    /// <summary>Returns quantity that was not able to fit.</summary>
    public Quantity StoreFuelAsMuchAs(Quantity fuel)
    {
      Assert.That<bool>(this.IsDocked).IsTrue();
      return this.m_fuelBuffer.StoreAsMuchAs(fuel);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.m_lowFuelNotif.NotifyIff(this.IsEnabled && this.LastDockedStatus == CargoShip.DockedStatus.NotEnoughFuel, (IEntity) this);
      this.m_noUpointsNotif.NotifyIff(this.IsEnabled && this.m_wasLackingUpointsAfterLastCheck && this.CargoDepot.ContractAssigned.HasValue, (IEntity) this);
      if (this.IsEnabled)
        Entity.HasWorkers((IEntityWithWorkers) this);
      switch (this.State)
      {
        case CargoShip.ShipState.ArrivingFromWorld:
        case CargoShip.ShipState.DepartingToWorld:
          this.simulateIslandMovement();
          break;
        case CargoShip.ShipState.Docked:
          this.handleDocked();
          break;
        case CargoShip.ShipState.AtWorldGoingForCargo:
          this.handleAtWorldGoingForCargo();
          break;
        case CargoShip.ShipState.AtWorldReturningHome:
          this.handleAtWorldReturningHome();
          break;
        default:
          Assert.Fail(string.Format("Invalid state '{0}'.", (object) this.State));
          break;
      }
    }

    private void simulateIslandMovement()
    {
      if (this.RemainingTransitionDuration.IsPositive)
        this.RemainingTransitionDuration -= Duration.OneTick;
      else if (this.State == CargoShip.ShipState.ArrivingFromWorld)
        this.State = CargoShip.ShipState.Docked;
      else if (this.State == CargoShip.ShipState.DepartingToWorld)
      {
        this.State = CargoShip.ShipState.AtWorldGoingForCargo;
        this.m_timer.Start(CargoShip.WORLD_DELAY_ONE_WAY.ScaledBy(this.DurationMultForCurrentJourney));
        this.consumeHalfFuel();
      }
      else
        Log.Error("Invalid state.");
    }

    private void consumeHalfFuel()
    {
      this.m_fuelStatsCollector.ReportFuelUseAndDestroy(this.m_fuelBuffer.Product, this.m_fuelBuffer.RemoveAsMuchAs(this.FuelPerJourneyNeeded() / 2), FuelUsedBy.CargoShip);
      if (!this.m_fuelData.PollutionPercent.IsPositive)
        return;
      this.Context.AirPollutionManager.EmitShipPollution((this.FuelPerJourneyNeeded().AsPartial.ScaledBy(this.m_fuelData.PollutionPercent) / 2).ScaledBy(CargoShip.POLLUTION_MULT));
    }

    public bool IsDepartNowAvailable(out LocStrFormatted reason)
    {
      if (this.DepartureRequestedByPlayer)
      {
        reason = (LocStrFormatted) TrCore.CargoShipCannotDepartNow__WasRequested;
        return false;
      }
      reason = (LocStrFormatted) TrCore.CargoShipCannotDepartNow__General;
      if (this.m_forceLeaveMode != CargoShip.ForceLeaveMode.None || !this.IsDocked || this.NonEmptyModules.IsEmpty<CargoShipModule>())
        return false;
      if (this.m_fuelBuffer.Quantity < this.FuelPerJourneyNeeded() && !this.CanPayWithUnityIfOutOfFuel)
      {
        reason = (LocStrFormatted) TrCore.ShipCantVisit__NoFuel;
        return false;
      }
      if (!((IEntityWithWorkers) this).HasWorkersCached)
      {
        reason = (LocStrFormatted) TrCore.ShipCantVisit__NoCrew;
        return false;
      }
      if (this.CargoDepot.ContractAssigned.HasValue)
        return this.m_contractsManager.CanShipDepartForContract(this, this.CargoDepot.ContractAssigned.Value, false, true) == ContractsManager.ShipDepartureCheckResult.Ok;
      if (this.m_worldMapCargoManager.CalculateCapacityUtilization(this).IsNotPositive)
        return false;
      reason = LocStrFormatted.Empty;
      return true;
    }

    /// <summary>Handles product exchange with the CargoDepot.</summary>
    private void handleDocked()
    {
      if ((this.IsNotEnabled || this.CargoDepot.IsNotEnabled) && this.m_forceLeaveMode == CargoShip.ForceLeaveMode.None)
      {
        this.LastDockedStatus = CargoShip.DockedStatus.Paused;
        this.m_timer.Reset();
      }
      else if (this.CargoDepot.IsUnloadingCargo())
      {
        this.LastDockedStatus = CargoShip.DockedStatus.ShipIsBeingUnloaded;
        this.m_timer.Reset();
      }
      else
      {
        if (this.m_forceLeaveMode == CargoShip.ForceLeaveMode.None && !this.CargoDepot.CanAcceptShip)
          this.m_forceLeaveMode = CargoShip.ForceLeaveMode.LeftForBlocked;
        if (this.m_forceLeaveMode != CargoShip.ForceLeaveMode.None)
        {
          this.startDeparture();
          this.m_timer.Reset();
        }
        else if (this.NonEmptyModules.IsEmpty<CargoShipModule>())
        {
          this.LastDockedStatus = CargoShip.DockedStatus.NoModulesBuilt;
          this.m_timer.Reset();
        }
        else if (this.m_fuelBuffer.Quantity < this.FuelPerJourneyNeeded() && !this.CanPayWithUnityIfOutOfFuel)
        {
          this.LastDockedStatus = CargoShip.DockedStatus.NotEnoughFuel;
          this.m_timer.Reset();
        }
        else if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        {
          this.LastDockedStatus = CargoShip.DockedStatus.NotEnoughWorkers;
          this.m_timer.Reset();
        }
        else
        {
          if (this.m_timer.Decrement())
            return;
          if (this.CargoDepot.ContractAssigned.HasValue)
          {
            ContractsManager.ShipDepartureCheckResult departureCheckResult = this.m_contractsManager.CanShipDepartForContract(this, this.CargoDepot.ContractAssigned.Value, true, this.DepartureRequestedByPlayer);
            this.m_wasLackingUpointsAfterLastCheck = departureCheckResult == ContractsManager.ShipDepartureCheckResult.NotEnoughUpoints;
            if (departureCheckResult != ContractsManager.ShipDepartureCheckResult.Ok)
            {
              this.LastDockedStatus = CargoShip.DockedStatus.NotEnoughToPickUp;
              this.m_timer.Start(CargoShip.TIME_BETWEEN_DEPARTURE_CHECKS);
              return;
            }
            this.m_departedForContract = true;
          }
          else
          {
            Percent capacityUtilization = this.m_worldMapCargoManager.CalculateCapacityUtilization(this);
            Percent percent = this.DepartureRequestedByPlayer ? 1.Percent() : CargoShip.MINIMAL_CAPACITY_UTILIZATION_FOR_DEPARTURE;
            if (capacityUtilization < percent)
            {
              this.LastDockedStatus = !capacityUtilization.IsZero ? CargoShip.DockedStatus.NotEnoughToPickUp : CargoShip.DockedStatus.NothingToPickUp;
              this.m_timer.Start(CargoShip.TIME_BETWEEN_DEPARTURE_CHECKS);
              return;
            }
            this.m_worldMapCargoManager.ReserveCargoForShip(this);
            this.m_departedForContract = false;
          }
          this.LastDockedStatus = CargoShip.DockedStatus.Ok;
          this.DepartureRequestedByPlayer = false;
          this.startDeparture();
        }
      }
    }

    private void startDeparture()
    {
      this.State = CargoShip.ShipState.DepartingToWorld;
      this.m_departedWithLowFuel = this.CanPayWithUnityIfOutOfFuel && this.m_fuelBuffer.Quantity < this.FuelPerJourneyNeeded();
      this.m_isFuelReducedForCurrentJourney = this.IsFuelReductionEnabled || this.m_departedWithLowFuel;
      this.RemainingTransitionDuration = this.DockTransitionDurationForCurrentJourney;
    }

    private void handleAtWorldGoingForCargo()
    {
      if (this.m_forceLeaveMode != CargoShip.ForceLeaveMode.None)
      {
        if (this.m_forceLeaveMode == CargoShip.ForceLeaveMode.LeftForUpgrade)
        {
          this.replaceSelfWithNewShip();
          return;
        }
        if (this.m_forceLeaveMode == CargoShip.ForceLeaveMode.LeftForFuelTypeChange)
        {
          if (this.PendingFuelToChangeTo.HasValue)
            this.setNewFuel(this.PendingFuelToChangeTo.Value);
          this.PendingFuelToChangeTo = Option<ProductProto>.None;
          this.m_pendingFuelChangeCost = AssetValue.Empty;
          this.m_forceLeaveMode = CargoShip.ForceLeaveMode.None;
          this.State = CargoShip.ShipState.AtWorldReturningHome;
          return;
        }
        if (this.m_forceLeaveMode == CargoShip.ForceLeaveMode.LeftForDestroy)
        {
          this.destroySelf();
          return;
        }
        if (this.m_forceLeaveMode == CargoShip.ForceLeaveMode.LeftForBlocked)
        {
          this.m_forceLeaveMode = CargoShip.ForceLeaveMode.None;
          this.State = CargoShip.ShipState.AtWorldReturningHome;
          return;
        }
        Log.Warning(string.Format("Unhandled leave mode: {0}", (object) this.m_forceLeaveMode));
      }
      if (this.m_departedForContract && this.CargoDepot.ContractAssigned.IsNone || !this.m_departedForContract && this.CargoDepot.ContractAssigned.HasValue)
      {
        if (!this.m_departedForContract)
          this.m_worldMapCargoManager.RemoveShipFromReservation(this);
        this.State = CargoShip.ShipState.AtWorldReturningHome;
      }
      else
      {
        if (this.m_timer.Decrement())
          return;
        if (this.CargoDepot.ContractAssigned.HasValue)
          this.m_contractsManager.ExchangeContractProducts(this, this.CargoDepot.ContractAssigned.Value);
        else
          this.m_worldMapCargoManager.LoadCargo(this);
        if (this.m_departedWithLowFuel)
        {
          Upoints? upointsCostIfNoFuel = this.GetUpointsCostIfNoFuel();
          this.Context.UpointsManager.TryConsume(IdsCore.UpointsCategories.ShipFuel, upointsCostIfNoFuel ?? Upoints.Zero);
        }
        this.m_departedWithLowFuel = false;
        this.m_timer.Start(CargoShip.WORLD_DELAY_ONE_WAY.ScaledBy(this.DurationMultForCurrentJourney));
        this.State = CargoShip.ShipState.AtWorldReturningHome;
      }
    }

    private void handleAtWorldReturningHome()
    {
      if (this.m_timer.Decrement() || !this.CargoDepot.CanAcceptShip)
        return;
      this.State = CargoShip.ShipState.ArrivingFromWorld;
      this.RemainingTransitionDuration = this.DockTransitionDurationForCurrentJourney;
      this.consumeHalfFuel();
    }

    protected override void OnDestroy()
    {
      this.CargoDepot.OnModuleAdded.Remove<CargoShip>(this, new Action<CargoDepotModule, int>(this.onDepotModuleAdded));
      this.CargoDepot.OnModuleRemoved.Remove<CargoShip>(this, new Action<int>(this.onDepotModuleRemoved));
      foreach (Option<CargoShipModule> module in this.m_modules)
      {
        if (module.HasValue)
          ((ICargoShipModuleFriend) module.Value).Destroy();
      }
      if (!this.m_upgradeConstructionPending)
        this.m_cargoDepotManager.ReleaseShipFromDepot(this);
      this.Context.ProductsManager.ProductDestroyed(this.m_fuelBuffer.Product, this.m_fuelBuffer.Quantity, DestroyReason.Cleared);
      this.m_fuelConsumptionMultiplier.OnChange.Remove<CargoShip>(this, new Action<Percent>(this.onFuelMultiplierChange));
      base.OnDestroy();
    }

    /// <summary>
    /// Called when the ship is added to the island after depot is built.
    /// </summary>
    public void RefillFuel(Quantity? toAdd)
    {
      this.Context.ProductsManager.ProductCreated(this.m_fuelBuffer.Product, this.m_fuelBuffer.StoreAsMuchAsReturnStored(toAdd ?? this.m_fuelBuffer.Capacity), CreateReason.InitialResource);
    }

    private void replaceSelfWithNewShip()
    {
      CargoShip newShip = this.m_cargoShipFactory.AddCargoShip(this.CargoDepot, this.CargoDepot.Prototype.CargoShipProto, (Option<ProductProto>) this.FuelProto);
      this.m_worldMapCargoManager.RemoveShipFromReservation(this);
      newShip.m_fuelBuffer.StoreExactly(this.m_fuelBuffer.Quantity);
      this.m_fuelBuffer.Clear();
      for (int index = 0; index < this.Modules.Count; ++index)
      {
        Option<CargoShipModule> module = this.Modules[index];
        CargoShipModule valueOrNull1 = module.ValueOrNull;
        if (valueOrNull1 != null && !valueOrNull1.StoredProduct.IsNone && index < newShip.Modules.Count)
        {
          module = newShip.Modules[index];
          CargoShipModule valueOrNull2 = module.ValueOrNull;
          if (valueOrNull2 != null && !(valueOrNull1.StoredProduct != valueOrNull2.StoredProduct))
          {
            Quantity quantity = valueOrNull2.StoreAsMuchAs(valueOrNull2.StoredProduct.Value.WithQuantity(valueOrNull1.Quantity));
            ((ICargoShipModuleFriend) valueOrNull1).RemoveExactly(valueOrNull1.Quantity);
            Assert.That<Quantity>(quantity).IsZero();
          }
        }
      }
      this.CargoDepot.ReplaceShipAndDestroyCurrent(newShip);
    }

    private void destroySelf()
    {
      this.Context.EntitiesManager.RemoveAndDestroyEntityNoChecks((IEntity) this, EntityRemoveReason.Remove);
    }

    /// <summary>Called by the cargo depot to request upgrade.</summary>
    public void RequestUpgrade()
    {
      Assert.That<bool>(this.IsDestroyed).IsFalse();
      if (this.State == CargoShip.ShipState.AtWorldGoingForCargo || this.State == CargoShip.ShipState.AtWorldReturningHome)
      {
        this.m_upgradeConstructionPending = true;
        this.replaceSelfWithNewShip();
      }
      else
      {
        this.m_upgradeConstructionPending = true;
        this.m_forceLeaveMode = CargoShip.ForceLeaveMode.LeftForUpgrade;
      }
    }

    public void RequestDestroy()
    {
      if (this.State == CargoShip.ShipState.AtWorldGoingForCargo || this.State == CargoShip.ShipState.AtWorldReturningHome)
        this.destroySelf();
      else
        this.m_forceLeaveMode = CargoShip.ForceLeaveMode.LeftForDestroy;
    }

    public bool CanChangeFuelTo(
      ProductProto fuel,
      out AssetValue cost,
      out LocStrFormatted errorMessage)
    {
      cost = AssetValue.Empty;
      errorMessage = LocStrFormatted.Empty;
      if ((Proto) this.FuelProto == (Proto) fuel)
      {
        if (!this.PendingFuelToChangeTo.IsNone)
          return true;
        errorMessage = (LocStrFormatted) TrCore.ShipFuelSwitch__InUse;
        return false;
      }
      if (this.m_forceLeaveMode != CargoShip.ForceLeaveMode.None && this.m_forceLeaveMode != CargoShip.ForceLeaveMode.LeftForFuelTypeChange)
      {
        errorMessage = (LocStrFormatted) TrCore.ShipFuelSwitch__ShipBusy;
        return false;
      }
      CargoShipProto.FuelData fuelData1 = this.Prototype.AvailableFuels.FirstOrDefault((Func<CargoShipProto.FuelData, bool>) (x => (Proto) x.FuelProto == (Proto) fuel));
      if (fuelData1 == null)
      {
        Log.Error(string.Format("Ship does not support {0} as fuel!", (object) fuel));
        return false;
      }
      CargoShipProto.FuelData fuelData2 = this.Prototype.AvailableFuels.FirstOrDefault((Func<CargoShipProto.FuelData, bool>) (x => (Proto) x.FuelProto == (Proto) this.FuelProto));
      if (fuelData2 == null || fuelData2.CompatibleFuels.Contains(fuel))
        return true;
      cost = fuelData1.Cost;
      foreach (ProductQuantity product in fuelData1.Cost.Products)
      {
        if (!this.Context.AssetTransactionManager.CanRemoveProduct(product))
        {
          errorMessage = (LocStrFormatted) TrCore.ShipFuelSwitch__MissingMaterials;
          return false;
        }
      }
      return true;
    }

    public void SetNewFuelType(ProductProto fuel)
    {
      AssetValue cost;
      if (!this.CanChangeFuelTo(fuel, out cost, out LocStrFormatted _))
        return;
      if ((Proto) this.FuelProto == (Proto) fuel)
      {
        if (this.State != CargoShip.ShipState.DepartingToWorld)
          this.m_forceLeaveMode = CargoShip.ForceLeaveMode.None;
        foreach (ProductQuantity product in this.m_pendingFuelChangeCost.Products)
          this.Context.AssetTransactionManager.StoreProduct(product, new CreateReason?(CreateReason.Deconstruction));
        this.m_pendingFuelChangeCost = AssetValue.Empty;
        this.PendingFuelToChangeTo = (Option<ProductProto>) Option.None;
      }
      else if (cost.IsEmpty)
      {
        this.setNewFuel(fuel);
        this.m_forceLeaveMode = CargoShip.ForceLeaveMode.None;
        this.PendingFuelToChangeTo = (Option<ProductProto>) Option.None;
      }
      else
      {
        Lyst<ProductQuantity> lyst = new Lyst<ProductQuantity>(cost.Products.Length);
        foreach (ProductQuantity product in cost.Products)
        {
          Quantity newQuantity = this.Context.AssetTransactionManager.RemoveAsMuchAs(product, new DestroyReason?(DestroyReason.Construction));
          lyst.Add(product.WithNewQuantity(newQuantity));
        }
        this.m_pendingFuelChangeCost = new AssetValue(lyst.ToImmutableArray());
        this.PendingFuelToChangeTo = (Option<ProductProto>) fuel;
        this.m_forceLeaveMode = CargoShip.ForceLeaveMode.LeftForFuelTypeChange;
      }
    }

    private void setNewFuel(ProductProto fuel)
    {
      if (!this.Prototype.AvailableFuels.Any((Func<CargoShipProto.FuelData, bool>) (x => (Proto) x.FuelProto == (Proto) fuel)))
      {
        Log.Error(string.Format("Fuel {0} not available!", (object) fuel));
      }
      else
      {
        ProductProto product = this.m_fuelBuffer.Product;
        this.Context.AssetTransactionManager.ClearBuffer((IProductBuffer) this.m_fuelBuffer);
        this.m_fuelBuffer.Destroy();
        this.loadFuelDataFor(fuel);
        this.m_fuelBuffer = new ProductBuffer(this.GetFuelReserveNeeded(), fuel);
        if (this.Prototype.GetGraphicsFor(fuel) == this.Prototype.GetGraphicsFor(product))
          return;
        this.Context.EntitiesManager.InvokeOnEntityVisualChanged((IEntity) this);
      }
    }

    public Quantity GetFuelReserveNeeded()
    {
      Quantity quantity = this.m_fuelData.FuelPerJourneyBase + this.Prototype.MaximumModulesCount.Min(this.CargoDepot.SlotCount) * this.m_fuelData.FuelPerJourneyPerModule;
      quantity = quantity.ScaledBy(this.m_fuelConsumptionMultiplier.Value.Max(Percent.Hundred));
      return quantity.ScaledBy(120.Percent()).Value.CeilToMultipleOf(10).Quantity();
    }

    public Upoints? GetUpointsCostIfNoFuel()
    {
      return !this.m_canUseOnUnityIfOutOfFuel ? new Upoints?() : new Upoints?(CargoShip.UPOINTS_PER_BASE_IF_NO_FUEL + CargoShip.UPOINTS_PER_MODULE_IF_NO_FUEL * this.Modules.Count);
    }

    bool IAreaSelectableEntity.IsSelected(RectangleTerrainArea2i area)
    {
      return area.Contains(this.Position2f);
    }

    public static void Serialize(CargoShip value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CargoShip>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CargoShip.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.CanPayWithUnityIfOutOfFuel);
      CargoDepot.Serialize(this.CargoDepot, writer);
      Option<string>.Serialize(this.CustomTitle, writer);
      writer.WriteBool(this.DepartureRequestedByPlayer);
      AngleDegrees1f.Serialize(this.Direction, writer);
      writer.WriteInt(this.GeneralPriority);
      writer.WriteBool(this.IsFuelReductionEnabled);
      writer.WriteInt((int) this.LastDockedStatus);
      CargoDepotManager.Serialize(this.m_cargoDepotManager, writer);
      writer.WriteGeneric<ICargoShipFactory>(this.m_cargoShipFactory);
      ContractsManager.Serialize(this.m_contractsManager, writer);
      writer.WriteBool(this.m_departedForContract);
      writer.WriteBool(this.m_departedWithLowFuel);
      writer.WriteInt((int) this.m_forceLeaveMode);
      ProductBuffer.Serialize(this.m_fuelBuffer, writer);
      writer.WriteGeneric<IProperty<Percent>>(this.m_fuelConsumptionMultiplier);
      writer.WriteGeneric<IFuelStatsCollector>(this.m_fuelStatsCollector);
      writer.WriteBool(this.m_isFuelReducedForCurrentJourney);
      EntityNotificator.Serialize(this.m_lowFuelNotif, writer);
      Lyst<Option<CargoShipModule>>.Serialize(this.m_modules, writer);
      Lyst<CargoShipModule>.Serialize(this.m_nonEmptyModules, writer);
      EntityNotificator.Serialize(this.m_noUpointsNotif, writer);
      AssetValue.Serialize(this.m_pendingFuelChangeCost, writer);
      TickTimer.Serialize(this.m_timer, writer);
      writer.WriteBool(this.m_upgradeConstructionPending);
      writer.WriteBool(this.m_wasLackingUpointsAfterLastCheck);
      WorldMapCargoManager.Serialize(this.m_worldMapCargoManager, writer);
      Option<ProductProto>.Serialize(this.PendingFuelToChangeTo, writer);
      Tile2f.Serialize(this.Position2f, writer);
      writer.WriteGeneric<CargoShipProto>(this.Prototype);
      Duration.Serialize(this.RemainingTransitionDuration, writer);
      writer.WriteInt((int) this.State);
    }

    public static CargoShip Deserialize(BlobReader reader)
    {
      CargoShip cargoShip;
      if (reader.TryStartClassDeserialization<CargoShip>(out cargoShip))
        reader.EnqueueDataDeserialization((object) cargoShip, CargoShip.s_deserializeDataDelayedAction);
      return cargoShip;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CanPayWithUnityIfOutOfFuel = reader.LoadedSaveVersion >= 140 && reader.ReadBool();
      this.CargoDepot = CargoDepot.Deserialize(reader);
      this.CustomTitle = Option<string>.Deserialize(reader);
      this.DepartureRequestedByPlayer = reader.ReadBool();
      this.Direction = AngleDegrees1f.Deserialize(reader);
      if (reader.LoadedSaveVersion < 140)
        reader.ReadGenericAs<ProductProto>();
      this.GeneralPriority = reader.ReadInt();
      this.IsFuelReductionEnabled = reader.ReadBool();
      this.LastDockedStatus = (CargoShip.DockedStatus) reader.ReadInt();
      reader.SetField<CargoShip>(this, "m_cargoDepotManager", (object) CargoDepotManager.Deserialize(reader));
      reader.SetField<CargoShip>(this, "m_cargoShipFactory", (object) reader.ReadGenericAs<ICargoShipFactory>());
      reader.SetField<CargoShip>(this, "m_contractsManager", (object) ContractsManager.Deserialize(reader));
      this.m_departedForContract = reader.ReadBool();
      this.m_departedWithLowFuel = reader.LoadedSaveVersion >= 140 && reader.ReadBool();
      this.m_forceLeaveMode = (CargoShip.ForceLeaveMode) reader.ReadInt();
      this.m_fuelBuffer = ProductBuffer.Deserialize(reader);
      reader.SetField<CargoShip>(this, "m_fuelConsumptionMultiplier", (object) reader.ReadGenericAs<IProperty<Percent>>());
      reader.SetField<CargoShip>(this, "m_fuelStatsCollector", (object) reader.ReadGenericAs<IFuelStatsCollector>());
      this.m_isFuelReducedForCurrentJourney = reader.ReadBool();
      this.m_lowFuelNotif = EntityNotificator.Deserialize(reader);
      reader.SetField<CargoShip>(this, "m_modules", (object) Lyst<Option<CargoShipModule>>.Deserialize(reader));
      reader.SetField<CargoShip>(this, "m_nonEmptyModules", (object) Lyst<CargoShipModule>.Deserialize(reader));
      this.m_noUpointsNotif = EntityNotificator.Deserialize(reader);
      this.m_pendingFuelChangeCost = reader.LoadedSaveVersion >= 140 ? AssetValue.Deserialize(reader) : AssetValue.Empty;
      reader.SetField<CargoShip>(this, "m_timer", (object) TickTimer.Deserialize(reader));
      this.m_upgradeConstructionPending = reader.ReadBool();
      this.m_wasLackingUpointsAfterLastCheck = reader.ReadBool();
      reader.SetField<CargoShip>(this, "m_worldMapCargoManager", (object) WorldMapCargoManager.Deserialize(reader));
      this.PendingFuelToChangeTo = reader.LoadedSaveVersion >= 140 ? Option<ProductProto>.Deserialize(reader) : new Option<ProductProto>();
      this.Position2f = Tile2f.Deserialize(reader);
      reader.SetField<CargoShip>(this, "Prototype", (object) reader.ReadGenericAs<CargoShipProto>());
      this.RemainingTransitionDuration = Duration.Deserialize(reader);
      this.State = (CargoShip.ShipState) reader.ReadInt();
      reader.RegisterInitAfterLoad<CargoShip>(this, "initSelfHigh", InitPriority.High);
      reader.RegisterInitAfterLoad<CargoShip>(this, "initSelf", InitPriority.Low);
    }

    static CargoShip()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CargoShip.POLLUTION_MULT = 60.Percent();
      CargoShip.SAVER_FUEL_MULT = 70.Percent();
      CargoShip.SAVER_TRAVEL_DURATION_MULT = 200.Percent();
      CargoShip.WORLD_DELAY_ONE_WAY = 1.Minutes();
      CargoShip.TIME_BETWEEN_DEPARTURE_CHECKS = 10.Seconds();
      CargoShip.MINIMAL_CAPACITY_UTILIZATION_FOR_DEPARTURE = 100.Percent();
      CargoShip.UPOINTS_PER_BASE_IF_NO_FUEL = 1.Upoints();
      CargoShip.UPOINTS_PER_MODULE_IF_NO_FUEL = 0.5.Upoints();
      CargoShip.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      CargoShip.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum ForceLeaveMode
    {
      None,
      LeftForUpgrade,
      LeftForDestroy,
      LeftForBlocked,
      LeftForFuelTypeChange,
    }

    public enum ShipState
    {
      ArrivingFromWorld,
      Docked,
      DepartingToWorld,
      AtWorldGoingForCargo,
      AtWorldReturningHome,
    }

    public enum DockedStatus
    {
      Ok,
      NoModulesBuilt,
      NotEnoughFuel,
      Paused,
      ShipIsBeingUnloaded,
      NothingToPickUp,
      NotEnoughToPickUp,
      NotEnoughWorkers,
    }
  }
}
