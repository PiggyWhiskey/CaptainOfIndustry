// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Shipyard.Shipyard
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Fleet;
using Mafi.Core.Notifications;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.World;
using Mafi.Core.World.Entities;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.Shipyard
{
  [GenerateSerializer(false, null, 0)]
  public class Shipyard : 
    LayoutEntity,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IOverflowProductsStorage,
    IEntityWithCustomPriority,
    IEntityWithPorts,
    IStaticEntityWithReservedOcean,
    ILayoutEntity,
    IEntityWithSimUpdate
  {
    private ShipyardProto m_proto;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly ProductsManager m_productsManager;
    private readonly IInstaBuildManager m_instaBuildManager;
    private readonly INotificationsManager m_notificationsManager;
    private readonly LogisticsBuffer m_fuelBuffer;
    private readonly TickTimer m_fuelAutoTransferTimer;
    private Option<Mafi.Core.Entities.Static.ConstructionProgress> m_repairProgress;
    public const string FUEL_IMPORT_PRIO_ID = "FuelImportPrio";
    public const string FUEL_EXPORT_PRIO_ID = "FuelExportPrio";
    public const string CARGO_EXPORT_PRIO_ID = "CargoExportPrio";
    public const string SHIP_REPAIR_IMPORT_PRIO_ID = "ShipRepairImportPrio";
    public const string WORLD_CARGO_IMPORT_PRIO_ID = "WorldCargoImportPrio";
    private int m_fuelImportPriority;
    private int m_fuelExportPriority;
    private int m_cargoExportPriority;
    private int m_shipRepairImportPriority;
    private int m_worldCargoImportPriority;
    private Option<Mafi.Core.Entities.Static.ConstructionProgress> m_modifPreparationProgress;
    private Option<Mafi.Core.Entities.Static.ConstructionProgress> m_modifApplicationProgress;
    private FleetEntityModificationRequest? m_modificationRequest;
    public Option<IWorldMapRepairableEntity> WorldEntityToConstruct;
    private readonly Lyst<ProductBuffer> m_worldEntityConstructBuffers;
    private readonly Set<ProductProto> m_worldEntityConstructProducts;
    private readonly Dict<ProductProto, Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer> m_cargo;
    private readonly Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider m_fuelBufferPrioProvider;
    private readonly Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider m_storedCargoPrioProvider;
    private readonly Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider m_shipRepairBufferPrioProvider;
    private readonly Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider m_worldCargoImportPrioProvider;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public ShipyardProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public override bool CanBePaused => false;

    public ILogisticsBufferReadOnly FuelBuffer => (ILogisticsBufferReadOnly) this.m_fuelBuffer;

    public bool CanRepair
    {
      get
      {
        return this.Prototype.CanRepair && this.DockedFleet.HasValue && this.DockedFleet.Value.IsDocked && this.DockedFleet.Value.NeedsRepair;
      }
    }

    public bool IsAutoRepairEnabled { get; set; }

    public bool IsRepairing => this.m_repairProgress.HasValue;

    public Option<IConstructionProgress> RepairProgress
    {
      get => this.m_repairProgress.As<IConstructionProgress>();
    }

    public bool CargoInputPaused { get; private set; }

    public bool CanPerformModifications => this.Prototype.CanRepair && this.DockedFleet.HasValue;

    public bool CanCancelModifications => this.ModificationState != 0;

    public bool CanApplyModification
    {
      get
      {
        return this.ModificationState == ShipModificationState.Prepared && this.DockedFleet.HasValue && this.DockedFleet.Value.IsDocked;
      }
    }

    public Option<IConstructionProgress> ModificationProgress
    {
      get
      {
        return !this.m_modifPreparationProgress.HasValue ? this.m_modifApplicationProgress.As<IConstructionProgress>() : this.m_modifPreparationProgress.As<IConstructionProgress>();
      }
    }

    public ShipModificationState ModificationState
    {
      get
      {
        if (this.m_modifApplicationProgress.HasValue)
          return ShipModificationState.Applying;
        if (!this.m_modifPreparationProgress.HasValue)
          return ShipModificationState.None;
        return this.m_modifPreparationProgress.Value.IsDone ? ShipModificationState.Prepared : ShipModificationState.Preparing;
      }
    }

    public Option<TravelingFleet> DockedFleet { get; private set; }

    public IUpgrader Upgrader { get; private set; }

    public bool HasHighCargoUnloadPrio { get; set; }

    public bool IsFull => this.TotalStoredQuantity >= this.Prototype.CargoCapacity;

    public Quantity TotalStoredQuantity { get; private set; }

    public ReservedOceanAreaState ReservedOceanAreaState { get; private set; }

    public Shipyard(
      EntityId id,
      ShipyardProto proto,
      TileTransform transform,
      EntityContext context,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      ProductsManager productsManager,
      IInstaBuildManager instaBuildManager,
      ILayoutEntityUpgraderFactory upgraderFactory,
      INotificationsManager notificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_fuelAutoTransferTimer = new TickTimer();
      this.m_fuelImportPriority = 8;
      this.m_fuelExportPriority = 8;
      this.m_cargoExportPriority = 8;
      this.m_shipRepairImportPriority = 8;
      this.m_worldCargoImportPriority = 8;
      this.m_worldEntityConstructBuffers = new Lyst<ProductBuffer>();
      this.m_worldEntityConstructProducts = new Set<ProductProto>();
      this.m_cargo = new Dict<ProductProto, Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_productsManager = productsManager;
      this.m_instaBuildManager = instaBuildManager;
      this.m_notificationsManager = notificationsManager;
      this.Upgrader = upgraderFactory.CreateInstance<ShipyardProto, Mafi.Core.Buildings.Shipyard.Shipyard>(this, this.Prototype);
      this.ReservedOceanAreaState = new ReservedOceanAreaState((IProtoWithReservedOcean) proto, (IStaticEntityWithReservedOcean) this, new EntityNotificationProto.ID?(IdsCore.Notifications.OceanAccessBlocked), notificationsManager);
      this.m_fuelBufferPrioProvider = new Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider(this);
      this.m_storedCargoPrioProvider = new Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider(this);
      this.m_shipRepairBufferPrioProvider = new Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider(this);
      this.m_worldCargoImportPrioProvider = new Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider(this);
      foreach (ProductProto product in context.ProtosDb.All<ProductProto>())
      {
        if (!product.CanBeDiscarded && !(product.Type == LooseProductProto.ProductType))
          this.getOrCreateCargoBufferFor(product);
      }
      this.m_fuelBuffer = new LogisticsBuffer(10.Quantity(), context.ProtosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Diesel));
      this.m_fuelBuffer.SetImportStep(0);
    }

    public void SetShip(TravelingFleet fleet)
    {
      Assert.That<Option<TravelingFleet>>(this.DockedFleet).IsNone<TravelingFleet>();
      this.DockedFleet = (Option<TravelingFleet>) fleet;
      fleet.FleetEntity.OnFuelCapacityChange.Add<Mafi.Core.Buildings.Shipyard.Shipyard>(this, new Action<Quantity>(this.onShipFuelCapacityChange));
      this.m_fuelBuffer.IncreaseCapacityTo(fleet.FleetEntity.FuelTankCapacity);
      this.Context.AssetTransactionManager.AddOverflowProductsStorage((IOverflowProductsStorage) this);
    }

    public bool CanBePrimary()
    {
      return this.DockedFleet.IsNone && !this.IsDestroyed && this.IsConstructed && this.ReservedOceanAreaState.HasAnyValidAreaSet;
    }

    public void RemoveShip()
    {
      Assert.That<Option<TravelingFleet>>(this.DockedFleet).HasValue<TravelingFleet>();
      if (this.DockedFleet.HasValue)
        this.DockedFleet.Value.FleetEntity.OnFuelCapacityChange.Remove<Mafi.Core.Buildings.Shipyard.Shipyard>(this, new Action<Quantity>(this.onShipFuelCapacityChange));
      if (this.m_repairProgress.HasValue)
      {
        this.unloadAndRemoveTemporaryBuffers(this.m_repairProgress.Value.ConstructionBuffers);
        this.m_repairProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) Option.None;
      }
      if (this.m_modificationRequest.HasValue)
        this.cancelModification();
      if (this.m_fuelBuffer.IsNotEmpty)
        this.getOrCreateCargoBufferFor(this.m_fuelBuffer.Product).StoreExactly(this.m_fuelBuffer.RemoveAll());
      this.clearEntityRepair();
      this.UpdateFuelImportExportStep(0, 10);
      this.DockedFleet = Option<TravelingFleet>.None;
    }

    private void onShipFuelCapacityChange(Quantity capacity)
    {
      this.m_fuelBuffer.IncreaseCapacityTo(capacity);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.DockedFleet.IsNone)
        return;
      TravelingFleet travelingFleet = this.DockedFleet.Value;
      if (travelingFleet.IsDocked && this.m_fuelBuffer.IsNotEmpty())
        this.m_fuelBuffer.RemoveExactly(this.m_fuelBuffer.Quantity - this.DockedFleet.Value.StoreFuelAsMuchAs(this.m_fuelBuffer.Quantity));
      this.tryMoveFuelFromCargoToFuelStorage();
      if (this.IsNotEnabled)
        return;
      this.simulateModifications();
      if (this.IsAutoRepairEnabled && !this.IsRepairing && this.CanRepair)
        this.SetRepairEnabled(true);
      if (!travelingFleet.IsDocked)
        this.refreshWorldMapRepairCargo();
      else if (this.CanApplyModification)
      {
        this.applyPendingModifications();
      }
      else
      {
        this.simulateConstruction();
        this.tryToUnloadShip();
        this.loadCargoToShipIfNeeded();
      }
    }

    /// <summary>
    /// When we have fuel in general cargo, we try to move it to fuel tank in case player requested it to be refueled.
    /// </summary>
    private void tryMoveFuelFromCargoToFuelStorage()
    {
      if (this.m_fuelBuffer.ImportUntilPercent.IsZero || !this.Prototype.CanRepair || this.m_fuelAutoTransferTimer.Decrement())
        return;
      this.m_fuelAutoTransferTimer.Start(Duration.OneSecond);
      Quantity maxQuantity = this.m_fuelBuffer.ImportUntilPercent.Apply(this.m_fuelBuffer.Capacity.Value).Quantity() - this.m_fuelBuffer.Quantity;
      Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer cargoProductBuffer;
      if (maxQuantity.IsNotPositive || !this.m_cargo.TryGetValue(this.m_fuelBuffer.Product, out cargoProductBuffer))
        return;
      this.m_fuelBuffer.StoreExactly(cargoProductBuffer.RemoveAsMuchAs(maxQuantity));
    }

    public void PeekAllCargo(Lyst<ProductQuantity> cacheToFill)
    {
      cacheToFill.Clear();
      foreach (Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer cargoProductBuffer in this.m_cargo.Values)
      {
        if (cargoProductBuffer.Quantity.IsPositive)
          cacheToFill.Add(cargoProductBuffer.ProductQuantity);
      }
      foreach (ProductBuffer entityConstructBuffer in this.m_worldEntityConstructBuffers)
      {
        if (entityConstructBuffer.Quantity.IsPositive)
          cacheToFill.Add(entityConstructBuffer.ProductQuantity);
      }
    }

    public void TryToDiscardCargo(ProductProto product)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer cargoProductBuffer;
      if (!Mafi.Core.Buildings.Shipyard.Shipyard.CanDiscardProduct(product) || !this.m_cargo.TryGetValue(product, out cargoProductBuffer) || cargoProductBuffer.IsEmpty)
        return;
      this.m_productsManager.ProductDestroyed(cargoProductBuffer.ProductQuantity, DestroyReason.Cleared);
      cargoProductBuffer.Clear();
    }

    public static bool CanDiscardProduct(ProductProto product)
    {
      return !product.IsWaste && product.Radioactivity <= 0;
    }

    private void tryToUnloadShip()
    {
      TravelingFleet travelingFleet = this.DockedFleet.Value;
      if (travelingFleet.Cargo.IsEmpty<KeyValuePair<ProductProto, IProductBuffer>>())
        return;
      Quantity maxQuantity = this.Prototype.CargoCapacity - this.TotalStoredQuantity;
      if (!maxQuantity.IsPositive)
        return;
      ProductQuantity productQuantity = travelingFleet.TryUnloadCargo(maxQuantity, (IReadOnlySet<ProductProto>) this.m_worldEntityConstructProducts);
      if (!productQuantity.IsNotEmpty)
        return;
      this.getOrCreateCargoBufferFor(productQuantity.Product).StoreExactly(productQuantity.Quantity);
    }

    public void SetRepairEnabled(bool enabled)
    {
      if (this.IsRepairing == enabled)
        return;
      if (enabled)
      {
        if (!this.CanRepair)
          return;
        AssetValue repairCost = this.DockedFleet.Value.FleetEntity.GetRepairCost();
        Lyst<ProductBuffer> lyst = new Lyst<ProductBuffer>();
        foreach (ProductQuantity product in repairCost.Products)
        {
          ProductBuffer buffer = new ProductBuffer(product.Quantity, product.Product);
          lyst.Add(buffer);
          this.m_vehicleBuffersRegistry.RegisterInputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer, (IInputBufferPriorityProvider) this.m_shipRepairBufferPrioProvider);
        }
        this.m_repairProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) new Mafi.Core.Entities.Static.ConstructionProgress((IEntity) this, lyst.ToImmutableArray(), repairCost, FleetEntityHullProto.RepairDurationPerProduct, Duration.Zero);
      }
      else
      {
        this.unloadAndRemoveTemporaryBuffers(this.m_repairProgress.Value.ConstructionBuffers);
        this.m_repairProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) Option.None;
      }
    }

    public void Cheat_FuelExactly(Quantity fuelToAdd) => this.m_fuelBuffer.StoreExactly(fuelToAdd);

    public void UpdateFuelImportExportStep(int importStep, int exportStep)
    {
      if (importStep >= 0)
        this.m_fuelBuffer.SetImportStep(importStep);
      if (importStep > 0)
        this.m_vehicleBuffersRegistry.TryRegisterInputBuffer((IStaticEntity) this, (IProductBuffer) this.m_fuelBuffer, (IInputBufferPriorityProvider) this.m_fuelBufferPrioProvider);
      else if (importStep == 0)
        this.m_vehicleBuffersRegistry.TryUnregisterInputBuffer((IProductBuffer) this.m_fuelBuffer);
      if (exportStep >= 0)
        this.m_fuelBuffer.SetExportStep(exportStep);
      if (exportStep >= 10)
      {
        this.m_vehicleBuffersRegistry.TryUnregisterOutputBuffer((IProductBuffer) this.m_fuelBuffer);
      }
      else
      {
        if (exportStep < 0)
          return;
        this.m_vehicleBuffersRegistry.TryRegisterOutputBuffer((IStaticEntity) this, (IProductBuffer) this.m_fuelBuffer, (IOutputBufferPriorityProvider) this.m_fuelBufferPrioProvider);
      }
    }

    private void simulateConstruction()
    {
      TravelingFleet travelingFleet = this.DockedFleet.Value;
      Assert.That<bool>(travelingFleet.LocationState == FleetLocationState.Docked).IsTrue();
      if (!this.IsRepairing)
        return;
      Mafi.Core.Entities.Static.ConstructionProgress constructionProgress = this.m_repairProgress.Value;
      this.pushToTemporaryBuffers(constructionProgress.ConstructionBuffers);
      constructionProgress.TryMakeStep();
      if (!constructionProgress.IsDone)
        return;
      foreach (ProductBuffer constructionBuffer in constructionProgress.ConstructionBuffers)
      {
        this.m_productsManager.ProductDestroyed(constructionBuffer.Product, constructionBuffer.Quantity, DestroyReason.General);
        this.m_vehicleBuffersRegistry.UnregisterInputBufferAndAssert((IProductBuffer) constructionBuffer);
      }
      this.m_repairProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) Option.None;
      travelingFleet.FleetEntity.Repair();
      this.m_notificationsManager.NotifyOnce(IdsCore.Notifications.ShipRepaired);
    }

    public void ToggleWorksPause()
    {
      if (!this.m_modifPreparationProgress.HasValue)
        return;
      this.CargoInputPaused = !this.CargoInputPaused;
    }

    public override EntityValidationResult CanStartDeconstruction()
    {
      return this.DockedFleet.HasValue ? EntityValidationResult.CreateError((LocStrFormatted) TrCore.RemovalError__HasShipAssigned) : EntityValidationResult.Success;
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      return this.IsNotEnabled || this.DockedFleet.IsNone || !this.Prototype.CanRepair ? pq.Quantity : this.m_fuelBuffer.StoreAsMuchAs(pq);
    }

    protected override void OnDestroy()
    {
      if (this.DockedFleet.HasValue)
        this.RemoveShip();
      this.Context.AssetTransactionManager.TryRemoveOverflowStorage((IOverflowProductsStorage) this);
      foreach (Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer buffer in this.m_cargo.Values)
      {
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) buffer);
        this.m_vehicleBuffersRegistry.UnregisterOutputBufferAndAssert((IProductBuffer) buffer);
        this.m_vehicleBuffersRegistry.UnregisterInputBufferAndAssert((IProductBuffer) buffer);
      }
      base.OnDestroy();
    }

    private void simulateModifications()
    {
      if (this.m_modifPreparationProgress.HasValue)
      {
        Mafi.Core.Entities.Static.ConstructionProgress constructionProgress = this.m_modifPreparationProgress.Value;
        this.pushToTemporaryBuffers(constructionProgress.ConstructionBuffers);
        constructionProgress.TryMakeStep();
      }
      else
      {
        if (!this.m_modifApplicationProgress.HasValue)
          return;
        Assert.That<bool>(this.DockedFleet.Value.IsDocked).IsTrue();
        Mafi.Core.Entities.Static.ConstructionProgress constructionProgress = this.m_modifApplicationProgress.Value;
        constructionProgress.TryMakeStep();
        if (!constructionProgress.IsDone)
          return;
        this.finishModification();
      }
    }

    private void cancelModification()
    {
      Assert.That<FleetEntityModificationRequest?>(this.m_modificationRequest).HasValue<FleetEntityModificationRequest>();
      if (this.m_modifPreparationProgress.HasValue)
        this.unloadAndRemoveTemporaryBuffers(this.m_modifPreparationProgress.Value.ConstructionBuffers);
      else if (this.m_modifApplicationProgress.HasValue)
        this.unloadAndRemoveTemporaryBuffers(this.m_modifApplicationProgress.Value.ConstructionBuffers);
      this.m_modifApplicationProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) Option.None;
      this.m_modifPreparationProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) Option.None;
      this.m_modificationRequest = new FleetEntityModificationRequest?();
      this.CargoInputPaused = false;
    }

    private void finishModification()
    {
      Assert.That<FleetEntityModificationRequest?>(this.m_modificationRequest).HasValue<FleetEntityModificationRequest>();
      Assert.That<Option<Mafi.Core.Entities.Static.ConstructionProgress>>(this.m_modifApplicationProgress).HasValue<Mafi.Core.Entities.Static.ConstructionProgress>();
      Assert.That<Option<Mafi.Core.Entities.Static.ConstructionProgress>>(this.m_modifPreparationProgress).IsNone<Mafi.Core.Entities.Static.ConstructionProgress>();
      FleetEntityModificationRequest modRequest = this.m_modificationRequest.Value;
      this.m_productsManager.ClearBuffersAndReportProducts(this.m_modifApplicationProgress.Value.ConstructionBuffers, DestroyReason.Construction);
      this.DockedFleet.Value.PerformModfications(modRequest);
      this.unloadAndRemoveTemporaryBuffers(this.m_modifApplicationProgress.Value.ConstructionBuffers);
      this.m_modifApplicationProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) Option.None;
      this.m_modificationRequest = new FleetEntityModificationRequest?();
      this.CargoInputPaused = false;
      this.m_notificationsManager.NotifyOnce(IdsCore.Notifications.ShipModified);
    }

    public void PerformShipModifications(FleetEntityModificationRequest modRequest)
    {
      if (this.DockedFleet.Value.NeedsRepair || this.IsRepairing)
        Log.Error("Can't upgrade ship that is being repaired or needs repairs!");
      else if (this.m_instaBuildManager.IsInstaBuildEnabled)
      {
        this.DockedFleet.Value.PerformModfications(modRequest);
      }
      else
      {
        AssetValue valueToPay;
        modRequest.GetPriceForModifications(this.DockedFleet.Value.FleetEntity, this.Context.ProtosDb, out valueToPay);
        if (valueToPay.GetQuantitySum().IsNotPositive)
        {
          Log.Error("Modification has no positive cost!");
        }
        else
        {
          Lyst<ProductBuffer> lyst = new Lyst<ProductBuffer>();
          foreach (ProductQuantity product in valueToPay.Products)
          {
            ProductBuffer buffer = new ProductBuffer(product.Quantity, product.Product);
            lyst.Add(buffer);
            this.m_vehicleBuffersRegistry.RegisterInputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer, (IInputBufferPriorityProvider) new Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider(this));
          }
          Duration oneTick = Duration.OneTick;
          this.m_modificationRequest = new FleetEntityModificationRequest?(modRequest);
          this.m_modifPreparationProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) new Mafi.Core.Entities.Static.ConstructionProgress((IEntity) this, lyst.ToImmutableArray(), valueToPay, oneTick, Duration.Zero);
        }
      }
    }

    private void applyPendingModifications()
    {
      if (!this.CanApplyModification)
        Log.Error("Cannot apply modification. Is the ship docked?");
      else if (!this.m_modificationRequest.HasValue)
      {
        Log.Error("No modification request.");
      }
      else
      {
        AssetValue valueToPay;
        this.m_modificationRequest.Value.GetPriceForModifications(this.DockedFleet.Value.FleetEntity, this.Context.ProtosDb, out valueToPay);
        Duration oneTick = Duration.OneTick;
        this.m_modifApplicationProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) new Mafi.Core.Entities.Static.ConstructionProgress((IEntity) this, this.m_modifPreparationProgress.Value.Buffers.CastArray<ProductBuffer>(), valueToPay, oneTick, Duration.Zero);
        this.m_modifPreparationProgress = (Option<Mafi.Core.Entities.Static.ConstructionProgress>) Option.None;
      }
    }

    public void CancelModifications()
    {
      if (!this.CanCancelModifications)
        Log.Error("Can't cancel ship modifications as none are being performed.");
      else
        this.cancelModification();
    }

    public void ProductsNeededForWorldEntityConstruction(Lyst<ProductQuantity> result)
    {
      Assert.That<Lyst<ProductQuantity>>(result).IsEmpty<ProductQuantity>();
      if (this.m_worldEntityConstructBuffers.IsEmpty)
        return;
      foreach (ProductBuffer entityConstructBuffer in this.m_worldEntityConstructBuffers)
      {
        if (!entityConstructBuffer.IsFull())
          result.Add(entityConstructBuffer.Product.WithQuantity(entityConstructBuffer.UsableCapacity));
      }
    }

    public void ToggleCargoLoadFor(IWorldMapRepairableEntity worldMapEntity)
    {
      if (this.WorldEntityToConstruct.HasValue)
      {
        if (this.WorldEntityToConstruct == worldMapEntity)
        {
          this.clearEntityRepair();
          return;
        }
        this.clearEntityRepair();
      }
      if (this.DockedFleet.IsNone)
      {
        Log.Error("No fleet docked, cannot start world entity repair!");
      }
      else
      {
        if (worldMapEntity.ConstructionProgress.IsNone)
          return;
        IConstructionProgress constructionProgress = worldMapEntity.ConstructionProgress.Value;
        this.WorldEntityToConstruct = Option.Some<IWorldMapRepairableEntity>(worldMapEntity);
        worldMapEntity.OnAllConstructionProductsAvailable.Add<Mafi.Core.Buildings.Shipyard.Shipyard>(this, new Action<IWorldMapRepairableEntity>(this.onEntityNoLongerNeedsProducts));
        worldMapEntity.OnConstructionDone.Add<Mafi.Core.Buildings.Shipyard.Shipyard>(this, new Action<IWorldMapRepairableEntity>(this.onEntityNoLongerNeedsProducts));
        foreach (IProductBufferReadOnly buffer1 in constructionProgress.Buffers)
        {
          Quantity quantityOf = constructionProgress.TotalCost.GetQuantityOf(buffer1.Product);
          Quantity quantity1 = Quantity.Zero;
          IProductBuffer productBuffer;
          if (this.DockedFleet.Value.Cargo.TryGetValue(buffer1.Product, out productBuffer))
            quantity1 = productBuffer.Quantity;
          ProductBuffer buffer2 = new ProductBuffer((quantityOf - buffer1.Quantity - quantity1).Max(Quantity.Zero), buffer1.Product);
          this.m_vehicleBuffersRegistry.RegisterInputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer2, (IInputBufferPriorityProvider) this.m_worldCargoImportPrioProvider);
          this.m_worldEntityConstructBuffers.Add(buffer2);
          this.m_worldEntityConstructProducts.Add(buffer2.Product);
          IProductBuffer cargoBufferFor = this.getOrCreateCargoBufferFor(buffer1.Product);
          if (cargoBufferFor.IsNotEmpty())
          {
            Quantity quantity2 = cargoBufferFor.Quantity - buffer2.StoreAsMuchAs(cargoBufferFor.Quantity);
            cargoBufferFor.RemoveExactly(quantity2);
          }
        }
      }
    }

    private void onEntityNoLongerNeedsProducts(IWorldMapRepairableEntity repairedEntity)
    {
      if (this.WorldEntityToConstruct.HasValue && this.WorldEntityToConstruct.Value != repairedEntity)
        Log.Error("Wrong entity repair callback registered!");
      else
        this.clearEntityRepair();
    }

    private void clearEntityRepair()
    {
      if (this.WorldEntityToConstruct.IsNone)
        return;
      this.WorldEntityToConstruct.Value.OnAllConstructionProductsAvailable.Remove<Mafi.Core.Buildings.Shipyard.Shipyard>(this, new Action<IWorldMapRepairableEntity>(this.onEntityNoLongerNeedsProducts));
      this.WorldEntityToConstruct.Value.OnConstructionDone.Remove<Mafi.Core.Buildings.Shipyard.Shipyard>(this, new Action<IWorldMapRepairableEntity>(this.onEntityNoLongerNeedsProducts));
      this.WorldEntityToConstruct = (Option<IWorldMapRepairableEntity>) Option.None;
      foreach (ProductBuffer entityConstructBuffer in this.m_worldEntityConstructBuffers)
      {
        if (entityConstructBuffer.IsNotEmpty())
        {
          this.getOrCreateCargoBufferFor(entityConstructBuffer.Product).StoreExactly(entityConstructBuffer.Quantity);
          entityConstructBuffer.Clear();
        }
        this.m_vehicleBuffersRegistry.UnregisterInputBufferAndAssert((IProductBuffer) entityConstructBuffer);
      }
      this.m_worldEntityConstructBuffers.Clear();
      this.m_worldEntityConstructProducts.Clear();
    }

    private void loadCargoToShipIfNeeded()
    {
      if (this.DockedFleet.IsNone || !this.DockedFleet.Value.IsDocked || this.WorldEntityToConstruct.IsNone)
        return;
      bool flag = false;
      TravelingFleet travelingFleet = this.DockedFleet.Value;
      foreach (ProductBuffer entityConstructBuffer in this.m_worldEntityConstructBuffers)
      {
        if (entityConstructBuffer.IsNotEmpty())
        {
          Quantity quantity = entityConstructBuffer.Quantity;
          travelingFleet.LoadCargo(entityConstructBuffer.Product.WithQuantity(quantity));
          entityConstructBuffer.RemoveAndReduceCapacity(quantity);
          flag = quantity.IsPositive;
        }
      }
      if (!flag || !this.m_worldEntityConstructBuffers.All<ProductBuffer>((Func<ProductBuffer, bool>) (x => x.Capacity.IsZero)))
        return;
      this.m_notificationsManager.NotifyOnce<EntityProto>(IdsCore.Notifications.ShipCargoLoaded, this.WorldEntityToConstruct.Value.Prototype);
    }

    /// <summary>
    /// The reason we have this method is to catch cases where the cargo for a world map entity repair
    /// disappears from the ship. This can happen when the player for instance repairs a different
    /// entity.
    /// </summary>
    private void refreshWorldMapRepairCargo()
    {
      if (this.DockedFleet.IsNone || this.WorldEntityToConstruct.IsNone)
        return;
      TravelingFleet travelingFleet = this.DockedFleet.Value;
      foreach (ProductBuffer entityConstructBuffer in this.m_worldEntityConstructBuffers)
      {
        Quantity missingQuantityFor = this.WorldEntityToConstruct.Value.ConstructionProgress.Value.GetMissingQuantityFor(entityConstructBuffer.Product);
        IProductBuffer productBuffer;
        Quantity newCapacity = !travelingFleet.Cargo.TryGetValue(entityConstructBuffer.Product, out productBuffer) ? missingQuantityFor.Max(Quantity.Zero) : (missingQuantityFor - productBuffer.Quantity).Max(Quantity.Zero);
        if (entityConstructBuffer.Capacity < newCapacity)
          entityConstructBuffer.IncreaseCapacityTo(newCapacity);
      }
    }

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
        Log.Error("Upgrade not available!");
      else
        this.Prototype = this.Prototype.Upgrade.NextTier.Value;
    }

    private void transferBuffersTo(Mafi.Core.Buildings.Shipyard.Shipyard newShipyard)
    {
      newShipyard.m_repairProgress = this.m_repairProgress;
      this.m_repairProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.None;
      foreach (Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer buffer in this.m_cargo.Values)
      {
        IProductBuffer cargoBufferFor = newShipyard.getOrCreateCargoBufferFor(buffer.Product);
        Quantity quantity = buffer.Quantity;
        cargoBufferFor.StoreExactly(quantity);
        buffer.RemoveExactly(quantity);
        buffer.Destroy();
      }
      newShipyard.HasHighCargoUnloadPrio = this.HasHighCargoUnloadPrio;
    }

    private IProductBuffer getOrCreateCargoBufferFor(ProductProto product)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer cargoBufferFor;
      if (this.m_cargo.TryGetValue(product, out cargoBufferFor))
        return (IProductBuffer) cargoBufferFor;
      Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer buffer = new Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer(Quantity.MaxValue, product, this);
      this.m_cargo.Add(buffer.Product, buffer);
      this.m_vehicleBuffersRegistry.RegisterOutputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer, (IOutputBufferPriorityProvider) this.m_storedCargoPrioProvider);
      this.m_vehicleBuffersRegistry.RegisterInputBufferAndAssert((IStaticEntity) this, (IProductBuffer) buffer, (IInputBufferPriorityProvider) StaticPriorityProvider.Ignore, true, true);
      return (IProductBuffer) buffer;
    }

    public void StoreProduct(ProductQuantity productQuantity)
    {
      Assert.That<bool>(productQuantity.Product.IsStorable).IsTrue("Trying to store non-storable product in shipyard.");
      this.getOrCreateCargoBufferFor(productQuantity.Product).StoreExactly(productQuantity.Quantity);
    }

    private void pushToTemporaryBuffers(ImmutableArray<ProductBuffer> buffers)
    {
      foreach (ProductBuffer buffer in buffers)
      {
        IProductBuffer cargoBufferFor = this.getOrCreateCargoBufferFor(buffer.Product);
        if (!cargoBufferFor.Quantity.IsZero)
        {
          Quantity quantity = buffer.StoreAsMuchAs(cargoBufferFor.Quantity);
          cargoBufferFor.RemoveExactly(cargoBufferFor.Quantity - quantity);
        }
      }
    }

    private void unloadAndRemoveTemporaryBuffers(ImmutableArray<ProductBuffer> buffers)
    {
      foreach (ProductBuffer buffer in buffers)
      {
        this.m_vehicleBuffersRegistry.UnregisterInputBufferAndAssert((IProductBuffer) buffer);
        if (buffer.IsNotEmpty())
          this.getOrCreateCargoBufferFor(buffer.Product).StoreExactly(buffer.Quantity);
      }
    }

    public int GetCustomPriority(string id)
    {
      switch (id)
      {
        case "FuelImportPrio":
          return this.m_fuelImportPriority;
        case "FuelExportPrio":
          return this.m_fuelExportPriority;
        case "CargoExportPrio":
          return this.m_cargoExportPriority;
        case "ShipRepairImportPrio":
          return this.m_shipRepairImportPriority;
        case "WorldCargoImportPrio":
          return this.m_worldCargoImportPriority;
        default:
          Assert.Fail("Unknown custom priority: " + id);
          return 0;
      }
    }

    public bool IsCustomPriorityVisible(string id)
    {
      switch (id)
      {
        case "FuelImportPrio":
          return this.m_fuelBuffer.ImportUntilPercent.IsPositive;
        case "FuelExportPrio":
          return this.m_fuelBuffer.ExportFromPercent < Percent.Hundred;
        case "CargoExportPrio":
          return this.HasHighCargoUnloadPrio;
        case "ShipRepairImportPrio":
          return this.IsRepairing;
        case "WorldCargoImportPrio":
          return this.m_worldEntityConstructBuffers.IsNotEmpty;
        default:
          return false;
      }
    }

    public void SetCustomPriority(string id, int priority)
    {
      if (!GeneralPriorities.AssertAssignableRange(priority))
        return;
      switch (id)
      {
        case "FuelImportPrio":
          this.m_fuelImportPriority = priority;
          break;
        case "FuelExportPrio":
          this.m_fuelExportPriority = priority;
          break;
        case "CargoExportPrio":
          this.m_cargoExportPriority = priority;
          break;
        case "ShipRepairImportPrio":
          this.m_shipRepairImportPriority = priority;
          break;
        case "WorldCargoImportPrio":
          this.m_worldCargoImportPriority = priority;
          break;
        default:
          Assert.Fail("Unknown custom priority: " + id);
          break;
      }
    }

    public static void Serialize(Mafi.Core.Buildings.Shipyard.Shipyard value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Mafi.Core.Buildings.Shipyard.Shipyard>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Mafi.Core.Buildings.Shipyard.Shipyard.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.CargoInputPaused);
      Option<TravelingFleet>.Serialize(this.DockedFleet, writer);
      writer.WriteBool(this.HasHighCargoUnloadPrio);
      writer.WriteBool(this.IsAutoRepairEnabled);
      Dict<ProductProto, Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer>.Serialize(this.m_cargo, writer);
      writer.WriteInt(this.m_cargoExportPriority);
      TickTimer.Serialize(this.m_fuelAutoTransferTimer, writer);
      LogisticsBuffer.Serialize(this.m_fuelBuffer, writer);
      Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider.Serialize(this.m_fuelBufferPrioProvider, writer);
      writer.WriteInt(this.m_fuelExportPriority);
      writer.WriteInt(this.m_fuelImportPriority);
      writer.WriteGeneric<IInstaBuildManager>(this.m_instaBuildManager);
      Option<Mafi.Core.Entities.Static.ConstructionProgress>.Serialize(this.m_modifApplicationProgress, writer);
      writer.WriteNullableStruct<FleetEntityModificationRequest>(this.m_modificationRequest);
      Option<Mafi.Core.Entities.Static.ConstructionProgress>.Serialize(this.m_modifPreparationProgress, writer);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      ProductsManager.Serialize(this.m_productsManager, writer);
      writer.WriteGeneric<ShipyardProto>(this.m_proto);
      Option<Mafi.Core.Entities.Static.ConstructionProgress>.Serialize(this.m_repairProgress, writer);
      Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider.Serialize(this.m_shipRepairBufferPrioProvider, writer);
      writer.WriteInt(this.m_shipRepairImportPriority);
      Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider.Serialize(this.m_storedCargoPrioProvider, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider.Serialize(this.m_worldCargoImportPrioProvider, writer);
      writer.WriteInt(this.m_worldCargoImportPriority);
      Lyst<ProductBuffer>.Serialize(this.m_worldEntityConstructBuffers, writer);
      Set<ProductProto>.Serialize(this.m_worldEntityConstructProducts, writer);
      ReservedOceanAreaState.Serialize(this.ReservedOceanAreaState, writer);
      Quantity.Serialize(this.TotalStoredQuantity, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
      Option<IWorldMapRepairableEntity>.Serialize(this.WorldEntityToConstruct, writer);
    }

    public static Mafi.Core.Buildings.Shipyard.Shipyard Deserialize(BlobReader reader)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard shipyard;
      if (reader.TryStartClassDeserialization<Mafi.Core.Buildings.Shipyard.Shipyard>(out shipyard))
        reader.EnqueueDataDeserialization((object) shipyard, Mafi.Core.Buildings.Shipyard.Shipyard.s_deserializeDataDelayedAction);
      return shipyard;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.CargoInputPaused = reader.ReadBool();
      this.DockedFleet = Option<TravelingFleet>.Deserialize(reader);
      this.HasHighCargoUnloadPrio = reader.ReadBool();
      this.IsAutoRepairEnabled = reader.ReadBool();
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_cargo", (object) Dict<ProductProto, Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer>.Deserialize(reader));
      this.m_cargoExportPriority = reader.ReadInt();
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_fuelAutoTransferTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_fuelBuffer", (object) LogisticsBuffer.Deserialize(reader));
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_fuelBufferPrioProvider", (object) Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider.Deserialize(reader));
      this.m_fuelExportPriority = reader.ReadInt();
      this.m_fuelImportPriority = reader.ReadInt();
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_instaBuildManager", (object) reader.ReadGenericAs<IInstaBuildManager>());
      this.m_modifApplicationProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.Deserialize(reader);
      this.m_modificationRequest = reader.ReadNullableStruct<FleetEntityModificationRequest>();
      this.m_modifPreparationProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.Deserialize(reader);
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_productsManager", (object) ProductsManager.Deserialize(reader));
      this.m_proto = reader.ReadGenericAs<ShipyardProto>();
      this.m_repairProgress = Option<Mafi.Core.Entities.Static.ConstructionProgress>.Deserialize(reader);
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_shipRepairBufferPrioProvider", (object) Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider.Deserialize(reader));
      this.m_shipRepairImportPriority = reader.ReadInt();
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_storedCargoPrioProvider", (object) Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider.Deserialize(reader));
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_worldCargoImportPrioProvider", (object) Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider.Deserialize(reader));
      this.m_worldCargoImportPriority = reader.ReadInt();
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_worldEntityConstructBuffers", (object) Lyst<ProductBuffer>.Deserialize(reader));
      reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard>(this, "m_worldEntityConstructProducts", (object) Set<ProductProto>.Deserialize(reader));
      this.ReservedOceanAreaState = ReservedOceanAreaState.Deserialize(reader);
      this.TotalStoredQuantity = Quantity.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      this.WorldEntityToConstruct = Option<IWorldMapRepairableEntity>.Deserialize(reader);
    }

    static Shipyard()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Mafi.Core.Buildings.Shipyard.Shipyard.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Mafi.Core.Buildings.Shipyard.Shipyard.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class ShipRepairBufferPriorityProvider : IInputBufferPriorityProvider
    {
      private readonly Mafi.Core.Buildings.Shipyard.Shipyard m_shipyard;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public ShipRepairBufferPriorityProvider(Mafi.Core.Buildings.Shipyard.Shipyard shipyard)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_shipyard = shipyard;
      }

      public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
      {
        return new BufferStrategy(this.m_shipyard.m_shipRepairImportPriority, new Quantity?(buffer.UsableCapacity - pendingQuantity));
      }

      public static void Serialize(
        Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.Serialize(this.m_shipyard, writer);
      }

      public static Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider Deserialize(
        BlobReader reader)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider priorityProvider;
        if (reader.TryStartClassDeserialization<Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider>(out priorityProvider))
          reader.EnqueueDataDeserialization((object) priorityProvider, Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider.s_deserializeDataDelayedAction);
        return priorityProvider;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider>(this, "m_shipyard", (object) Mafi.Core.Buildings.Shipyard.Shipyard.Deserialize(reader));
      }

      static ShipRepairBufferPriorityProvider()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider) obj).SerializeData(writer));
        Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Mafi.Core.Buildings.Shipyard.Shipyard.ShipRepairBufferPriorityProvider) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public class WorldCargoImportPriorityProvider : IInputBufferPriorityProvider
    {
      private readonly Mafi.Core.Buildings.Shipyard.Shipyard m_shipyard;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public WorldCargoImportPriorityProvider(Mafi.Core.Buildings.Shipyard.Shipyard shipyard)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_shipyard = shipyard;
      }

      public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
      {
        return new BufferStrategy(this.m_shipyard.m_worldCargoImportPriority, new Quantity?(buffer.UsableCapacity - pendingQuantity));
      }

      public static void Serialize(
        Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.Serialize(this.m_shipyard, writer);
      }

      public static Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider Deserialize(
        BlobReader reader)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider priorityProvider;
        if (reader.TryStartClassDeserialization<Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider>(out priorityProvider))
          reader.EnqueueDataDeserialization((object) priorityProvider, Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider.s_deserializeDataDelayedAction);
        return priorityProvider;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider>(this, "m_shipyard", (object) Mafi.Core.Buildings.Shipyard.Shipyard.Deserialize(reader));
      }

      static WorldCargoImportPriorityProvider()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider) obj).SerializeData(writer));
        Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Mafi.Core.Buildings.Shipyard.Shipyard.WorldCargoImportPriorityProvider) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    private class FuelBufferPriorityProvider : 
      IInputBufferPriorityProvider,
      IOutputBufferPriorityProvider
    {
      private readonly Mafi.Core.Buildings.Shipyard.Shipyard m_shipyard;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public FuelBufferPriorityProvider(Mafi.Core.Buildings.Shipyard.Shipyard shipyard)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_shipyard = shipyard;
      }

      public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
      {
        return this.m_shipyard.m_fuelBuffer.GetInputPriority(this.m_shipyard.m_fuelImportPriority, pendingQuantity);
      }

      public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
      {
        return this.m_shipyard.m_fuelBuffer.GetOutputPriority(this.m_shipyard.m_fuelExportPriority, request);
      }

      public static void Serialize(Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.Serialize(this.m_shipyard, writer);
      }

      public static Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider Deserialize(
        BlobReader reader)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider priorityProvider;
        if (reader.TryStartClassDeserialization<Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider>(out priorityProvider))
          reader.EnqueueDataDeserialization((object) priorityProvider, Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider.s_deserializeDataDelayedAction);
        return priorityProvider;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider>(this, "m_shipyard", (object) Mafi.Core.Buildings.Shipyard.Shipyard.Deserialize(reader));
      }

      static FuelBufferPriorityProvider()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider) obj).SerializeData(writer));
        Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Mafi.Core.Buildings.Shipyard.Shipyard.FuelBufferPriorityProvider) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    private class StoredCargoPriorityProvider : IOutputBufferPriorityProvider
    {
      private readonly Mafi.Core.Buildings.Shipyard.Shipyard m_shipyard;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public StoredCargoPriorityProvider(Mafi.Core.Buildings.Shipyard.Shipyard shipyard)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_shipyard = shipyard;
      }

      public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
      {
        return !this.m_shipyard.HasHighCargoUnloadPrio ? BufferStrategy.NoQuantityPreference(14) : new BufferStrategy(this.m_shipyard.m_cargoExportPriority, new Quantity?(request.Buffer.Quantity - request.PendingQuantity));
      }

      public static void Serialize(Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.Serialize(this.m_shipyard, writer);
      }

      public static Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider Deserialize(
        BlobReader reader)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider priorityProvider;
        if (reader.TryStartClassDeserialization<Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider>(out priorityProvider))
          reader.EnqueueDataDeserialization((object) priorityProvider, Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider.s_deserializeDataDelayedAction);
        return priorityProvider;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider>(this, "m_shipyard", (object) Mafi.Core.Buildings.Shipyard.Shipyard.Deserialize(reader));
      }

      static StoredCargoPriorityProvider()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider) obj).SerializeData(writer));
        Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Mafi.Core.Buildings.Shipyard.Shipyard.StoredCargoPriorityProvider) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    private class ModificationBufferPriorityProvider : IInputBufferPriorityProvider
    {
      private readonly Mafi.Core.Buildings.Shipyard.Shipyard m_shipyard;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public ModificationBufferPriorityProvider(Mafi.Core.Buildings.Shipyard.Shipyard shipyard)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_shipyard = shipyard;
      }

      public BufferStrategy GetInputPriority(IProductBuffer buffer, Quantity pendingQuantity)
      {
        if (this.m_shipyard.CargoInputPaused)
          return BufferStrategy.Ignore;
        Mafi.Core.Entities.Static.ConstructionProgress constructionProgress = this.m_shipyard.m_modifPreparationProgress.Value;
        return new BufferStrategy(new Quantity((constructionProgress.AllowedSteps - constructionProgress.CurrentSteps) / constructionProgress.DurationPerProduct.Ticks) + pendingQuantity < 6 * TruckCaps.SmallTruckCapacity ? this.m_shipyard.m_shipRepairImportPriority : 14, new Quantity?(buffer.UsableCapacity - pendingQuantity));
      }

      public static void Serialize(
        Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.Serialize(this.m_shipyard, writer);
      }

      public static Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider Deserialize(
        BlobReader reader)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider priorityProvider;
        if (reader.TryStartClassDeserialization<Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider>(out priorityProvider))
          reader.EnqueueDataDeserialization((object) priorityProvider, Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider.s_deserializeDataDelayedAction);
        return priorityProvider;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider>(this, "m_shipyard", (object) Mafi.Core.Buildings.Shipyard.Shipyard.Deserialize(reader));
      }

      static ModificationBufferPriorityProvider()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider) obj).SerializeData(writer));
        Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Mafi.Core.Buildings.Shipyard.Shipyard.ModificationBufferPriorityProvider) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    private sealed class CargoProductBuffer : GlobalOutputBuffer
    {
      private readonly Mafi.Core.Buildings.Shipyard.Shipyard m_shipyard;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public CargoProductBuffer(Quantity capacity, ProductProto product, Mafi.Core.Buildings.Shipyard.Shipyard shipyard)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector(capacity, product, shipyard.Context.ProductsManager, 0, (IEntity) shipyard);
        this.m_shipyard = shipyard;
      }

      protected override void OnQuantityChanged(Quantity diff)
      {
        base.OnQuantityChanged(diff);
        this.m_shipyard.TotalStoredQuantity += diff;
        Assert.That<Quantity>(this.m_shipyard.TotalStoredQuantity).IsNotNegative();
      }

      public static void Serialize(Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer.s_serializeDataDelayedAction);
      }

      protected override void SerializeData(BlobWriter writer)
      {
        base.SerializeData(writer);
        Mafi.Core.Buildings.Shipyard.Shipyard.Serialize(this.m_shipyard, writer);
      }

      public static Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer Deserialize(
        BlobReader reader)
      {
        Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer cargoProductBuffer;
        if (reader.TryStartClassDeserialization<Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer>(out cargoProductBuffer))
          reader.EnqueueDataDeserialization((object) cargoProductBuffer, Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer.s_deserializeDataDelayedAction);
        return cargoProductBuffer;
      }

      protected override void DeserializeData(BlobReader reader)
      {
        base.DeserializeData(reader);
        reader.SetField<Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer>(this, "m_shipyard", (object) Mafi.Core.Buildings.Shipyard.Shipyard.Deserialize(reader));
      }

      static CargoProductBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ProductBuffer) obj).SerializeData(writer));
        Mafi.Core.Buildings.Shipyard.Shipyard.CargoProductBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ProductBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
