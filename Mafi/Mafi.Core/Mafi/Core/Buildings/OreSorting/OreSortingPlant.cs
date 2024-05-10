// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.OreSorting.OreSortingPlant
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Buildings.OreSorting
{
  [GenerateSerializer(false, null, 0)]
  public class OreSortingPlant : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithLogisticsControl,
    IMaintainedEntity,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IAreaSelectableEntity,
    IElectricityConsumingEntity,
    IAnimatedEntity,
    IEntityWithSimUpdate,
    IEntityAssignedAsInput,
    ILayoutEntity,
    IStaticEntityWithQueue,
    IEntityWithCloneableConfig,
    IEntityWithParticles
  {
    public const int OUTPUT_PORTS_COUNT = 4;
    public const int MAX_PRODUCTS = 8;
    private OreSortingPlantProto m_proto;
    private readonly IElectricityConsumer m_electricityConsumer;
    private readonly Set<IEntityAssignedAsOutput> m_assignedOutputEntities;
    [DoNotSave(0, null)]
    private Lyst<IProductBuffer> m_outputBuffers;
    private readonly Dict<ProductProto, OreSortingPlant.OreSortingPlantProductData> m_productsData;
    private readonly VehicleJobs m_jobs;
    [DoNotSave(0, null)]
    public ImmutableArray<ProductProto> AllSupportedProducts;
    [DoNotSave(0, null)]
    private Quantity m_reservedTotal;
    [DoNotSave(0, null)]
    private ImmutableArray<char> m_outputsNames;
    private readonly ICalendar m_calendar;
    private readonly IProductsManager m_productsManager;
    private readonly AutoBufferLogisticsHelper m_autoLogisticsHelper;
    private readonly TickTimer m_sortingTimer;
    private bool m_tempBuffersFull;
    private readonly VehicleQueue<Vehicle, IStaticEntity> m_vehicleQueue;
    private EntityNotificator m_noProductsSetNotif;
    [NewInSaveVersion(156, null, null, null, null)]
    private EntityNotificator m_outputBlockedNotif;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public OreSortingPlantProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (LayoutEntityProto) value;
      }
    }

    public ImmutableArray<Mafi.Core.Entities.Animations.AnimationParams> AnimationParams
    {
      get => this.Prototype.AnimationParams;
    }

    public AnimationStatesProvider AnimationStatesProvider { get; private set; }

    public bool AreParticlesEnabled => this.CurrentState == OreSortingPlant.State.Working;

    Electricity IElectricityConsumingEntity.PowerRequired => this.Prototype.ElectricityConsumed;

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.SomeOption<IElectricityConsumerReadonly>();
    }

    MaintenanceCosts IMaintainedEntity.MaintenanceCosts => this.Prototype.Costs.Maintenance;

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public bool IsIdleForMaintenance => this.CurrentState != OreSortingPlant.State.Working;

    public override bool CanBePaused => true;

    public OreSortingPlant.State CurrentState { get; private set; }

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public IReadOnlySet<IEntityAssignedAsOutput> AssignedOutputs
    {
      get => (IReadOnlySet<IEntityAssignedAsOutput>) this.m_assignedOutputEntities;
    }

    public bool AllowNonAssignedOutput { get; set; }

    [NewInSaveVersion(153, null, null, null, null)]
    public bool DoNotAcceptSingleProduct { get; set; }

    public IIndexable<IProductBuffer> OutputBuffers
    {
      get => (IIndexable<IProductBuffer>) this.m_outputBuffers;
    }

    public IEnumerable<ProductProto> AllowedProducts
    {
      get => (IEnumerable<ProductProto>) this.m_productsData.Keys;
    }

    public IReadOnlyDictionary<ProductProto, OreSortingPlant.OreSortingPlantProductData> ProductsData
    {
      get
      {
        return (IReadOnlyDictionary<ProductProto, OreSortingPlant.OreSortingPlantProductData>) this.m_productsData;
      }
    }

    public IReadOnlySet<IVehicleJob> AllReservedJobs => this.m_jobs.AllJobs;

    [DoNotSave(0, null)]
    public Quantity Capacity { get; private set; }

    public Quantity CapacityLeft => (this.Capacity - this.MixedTotal).Max(Quantity.Zero);

    [DoNotSave(0, null)]
    public Quantity SortedPerDuration { get; private set; }

    [DoNotSave(0, null)]
    public Quantity MixedTotal { get; private set; }

    public Percent PercentFull
    {
      get
      {
        if (this.Capacity.IsNotPositive || this.MixedTotal.IsNotPositive)
          return Percent.Zero;
        return !(this.MixedTotal > this.Capacity) ? Percent.FromRatio(this.MixedTotal.Value, this.Capacity.Value) : Percent.Hundred;
      }
    }

    public bool IsEmpty => this.MixedTotal.IsNotPositive;

    public bool IsNotEmpty => this.MixedTotal.IsPositive;

    public OreSortingPlant(
      EntityId id,
      OreSortingPlantProto proto,
      TileTransform transform,
      EntityContext context,
      ICalendar calendar,
      IProductsManager productsManager,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IAnimationStateFactory animationStateFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_assignedOutputEntities = new Set<IEntityAssignedAsOutput>();
      this.m_outputBuffers = new Lyst<IProductBuffer>();
      this.m_productsData = new Dict<ProductProto, OreSortingPlant.OreSortingPlantProductData>();
      this.m_jobs = new VehicleJobs();
      this.m_sortingTimer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_calendar = calendar;
      this.m_productsManager = productsManager;
      this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this);
      this.AnimationStatesProvider = animationStateFactory.CreateProviderFor((IAnimatedEntity) this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      EntityGeneralPriorityProvider priorityProvider = new EntityGeneralPriorityProvider((IEntityWithGeneralPriority) this);
      this.m_autoLogisticsHelper = new AutoBufferLogisticsHelper((IStaticEntity) this, (IInputBufferPriorityProvider) priorityProvider, (IOutputBufferPriorityProvider) priorityProvider, vehicleBuffersRegistry);
      this.m_vehicleQueue = new VehicleQueue<Vehicle, IStaticEntity>((IStaticEntity) this);
      this.m_vehicleQueue.Enable();
      this.m_noProductsSetNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.SortingPlantNoProductSet);
      this.m_outputBlockedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.SortingPlantBlockedOutput);
      this.initData();
      calendar.NewMonth.Add<OreSortingPlant>(this, new Action(this.onNewMonth));
      this.AnimationStatesProvider.Start(this.Prototype.Duration);
    }

    [InitAfterLoad(InitPriority.Low)]
    private void initSelf(int saveVersion)
    {
      this.m_outputBuffers = this.m_productsData.Values.Select<OreSortingPlant.OreSortingPlantProductData, IProductBuffer>((Func<OreSortingPlant.OreSortingPlantProductData, IProductBuffer>) (x => (IProductBuffer) x.Buffer)).ToLyst<IProductBuffer>();
      this.initData();
      this.updateMixedQuantity();
      this.updateReservedTotal();
      if (saveVersion >= 156)
        return;
      this.m_outputBlockedNotif = this.Context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.CannotDeliverFromMineTower);
    }

    private void initData()
    {
      IProperty<Percent> property = this.Context.PropertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.TrucksCapacityMultiplier);
      property.OnChange.AddNonSaveable<OreSortingPlant>(this, new Action<Percent>(this.updateCapacity));
      this.updateCapacity(property.Value);
      this.AllSupportedProducts = this.Context.ProtosDb.All<TerrainMaterialProto>().Select<TerrainMaterialProto, ProductProto>((Func<TerrainMaterialProto, ProductProto>) (x => (ProductProto) x.MinedProduct)).Distinct<ProductProto>().ToImmutableArray<ProductProto>();
      this.m_outputsNames = this.Ports.Where((Func<IoPort, bool>) (x => x.Type == IoPortType.Output)).Select<IoPort, char>((Func<IoPort, char>) (x => x.Name)).OrderBy<char, char>((Func<char, char>) (x => x)).ToImmutableArray<char>();
    }

    private void updateCapacity(Percent newMultiplier)
    {
      this.SortedPerDuration = this.Prototype.QuantityPerDuration.ScaledBy(newMultiplier);
      this.Capacity = this.Prototype.InputBufferCapacity.ScaledBy(newMultiplier);
      foreach (KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData> keyValuePair in this.m_productsData)
        keyValuePair.Value.Buffer.ForceNewCapacityTo(this.Prototype.OutputBuffersCapacity.ScaledBy(newMultiplier));
    }

    private void onNewMonth()
    {
      foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
        plantProductData.OnNewMonth();
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.CurrentState = this.updateState();
      this.m_noProductsSetNotif.NotifyIff(this.IsEnabled && this.m_productsData.Count <= 1, (IEntity) this);
      bool shouldNotify = false;
      if (this.IsEnabled && this.m_tempBuffersFull)
      {
        foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
        {
          if (plantProductData.NotifyIfFullOutput && plantProductData.Buffer.IsFull)
          {
            shouldNotify = true;
            break;
          }
        }
      }
      this.m_outputBlockedNotif.NotifyIff(shouldNotify, (IEntity) this);
      this.AnimationStatesProvider.Step(this.CurrentState == OreSortingPlant.State.Working ? Percent.Hundred : Percent.Zero, this.m_sortingTimer.PercentFinished);
      if (!this.IsEnabled)
        return;
      this.sentOutputs();
    }

    private OreSortingPlant.State updateState()
    {
      if (this.IsNotEnabled)
        return !this.Maintenance.Status.IsBroken ? OreSortingPlant.State.Paused : OreSortingPlant.State.Broken;
      if (Entity.IsMissingWorkers((IEntityWithWorkers) this))
        return OreSortingPlant.State.MissingWorkers;
      if (!this.m_electricityConsumer.CanConsume())
        return OreSortingPlant.State.NotEnoughPower;
      OreSortingPlant.State state = this.simStepSorting();
      if (state == OreSortingPlant.State.Working)
        this.m_electricityConsumer.TryConsume();
      return state;
    }

    private OreSortingPlant.State simStepSorting()
    {
      if (this.m_tempBuffersFull)
        this.pushResultsToBuffers();
      if (this.m_sortingTimer.IsFinished)
      {
        if (!this.sortProducts())
          return OreSortingPlant.State.MissingInput;
        this.m_sortingTimer.Start(this.Prototype.Duration);
        return OreSortingPlant.State.Working;
      }
      if (this.m_sortingTimer.Decrement())
        return OreSortingPlant.State.Working;
      this.pushResultsToBuffers();
      return OreSortingPlant.State.Working;
    }

    private void addProductDataFor(ProductProto product, char? port = null)
    {
      char outputPort = (char) ((int) port ?? (int) this.m_outputsNames.FirstOrDefault((Func<char, bool>) (x => this.m_productsData.Values.All<OreSortingPlant.OreSortingPlantProductData>((Func<OreSortingPlant.OreSortingPlantProductData, bool>) (p => (int) p.OutputPort != (int) x)))));
      if (outputPort == char.MinValue)
        outputPort = this.m_outputsNames.Last;
      GlobalOutputBuffer buffer = new GlobalOutputBuffer(this.Prototype.OutputBuffersCapacity.ScaledBy(this.Context.PropertiesDb.GetProperty<Percent>(IdsCore.PropertyIds.TrucksCapacityMultiplier).Value), product, this.m_productsManager, 15, (IEntity) this);
      OreSortingPlant.OreSortingPlantProductData plantProductData = new OreSortingPlant.OreSortingPlantProductData(buffer, outputPort);
      this.m_productsData.Add(buffer.Product, plantProductData);
      this.m_outputBuffers.Add((IProductBuffer) plantProductData.Buffer);
      this.m_autoLogisticsHelper.AddOutputBuffer((IProductBuffer) plantProductData.Buffer);
    }

    private void pushResultsToBuffers()
    {
      this.m_tempBuffersFull = false;
      foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
      {
        Quantity beingSorted = plantProductData.BeingSorted;
        if (beingSorted.IsNotPositive)
        {
          Assert.That<Quantity>(plantProductData.BeingSorted).IsNotNegative();
        }
        else
        {
          Quantity quantity = plantProductData.Buffer.StoreAsMuchAsReturnStored(plantProductData.BeingSorted);
          if (quantity.IsPositive)
          {
            plantProductData.BeingSorted -= quantity;
            plantProductData.SortedThisMonth += quantity;
          }
          beingSorted = plantProductData.BeingSorted;
          if (beingSorted.IsPositive)
            this.m_tempBuffersFull = true;
        }
      }
    }

    private bool sortProducts()
    {
      if (this.MixedTotal.IsNotPositive)
        return false;
      Quantity sortedPerDuration = this.SortedPerDuration;
      Quantity zero1 = Quantity.Zero;
      Quantity zero2 = Quantity.Zero;
      foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
      {
        if (!plantProductData.BeingSorted.IsPositive)
          zero2 += plantProductData.UnsortedQuantity;
      }
      foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
      {
        if (!(zero1 >= sortedPerDuration))
        {
          Quantity quantity1 = plantProductData.BeingSorted;
          if (!quantity1.IsPositive)
          {
            quantity1 = plantProductData.UnsortedQuantity;
            if (quantity1.IsNotPositive)
            {
              Assert.That<Quantity>(plantProductData.UnsortedQuantity).IsNotNegative();
            }
            else
            {
              quantity1 = (plantProductData.UnsortedQuantity.Value / zero2.Value.ToFix32() * sortedPerDuration.Value).ToIntCeiled().Quantity();
              quantity1 = quantity1.Min(plantProductData.UnsortedQuantity);
              Quantity quantity2 = quantity1.Min(sortedPerDuration - zero1);
              plantProductData.UnsortedQuantity -= quantity2;
              plantProductData.BeingSorted += quantity2;
              this.MixedTotal -= quantity2;
              zero1 += quantity2;
              if (plantProductData.CanBeWasted)
              {
                plantProductData.ToWaste += quantity2.AsPartial.ScaledBy(this.Prototype.ConversionLoss);
                quantity1 = plantProductData.ToWaste.IntegerPart;
                Quantity quantity3 = quantity1.Min(plantProductData.BeingSorted);
                if (quantity3.IsPositive)
                {
                  this.Context.ProductsManager.ProductDestroyed(plantProductData.Buffer.Product, quantity3, DestroyReason.Wasted);
                  plantProductData.BeingSorted -= quantity3;
                  plantProductData.ToWaste -= quantity3.AsPartial;
                }
              }
            }
          }
        }
        else
          break;
      }
      return zero1.IsPositive;
    }

    private void sentOutputs()
    {
      foreach (IoPortData connectedOutputPort in this.ConnectedOutputPorts)
      {
        foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
        {
          if ((int) plantProductData.OutputPort == (int) connectedOutputPort.Name)
          {
            GlobalOutputBuffer buffer = plantProductData.Buffer;
            Quantity quantity1 = buffer.Quantity;
            Quantity quantity2 = quantity1 - connectedOutputPort.SendAsMuchAs(buffer.Product.WithQuantity(quantity1));
            if (quantity2.IsPositive)
            {
              buffer.RemoveExactly(quantity2);
              this.m_autoLogisticsHelper.OnProductSentToPort((IProductBuffer) buffer);
              break;
            }
          }
        }
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      Log.Error("Cannot receive products!");
      return pq.Quantity;
    }

    public void AddProductToSort(ProductProto product, char? port = null)
    {
      if (this.m_productsData.ContainsKey(product) || this.m_productsData.Count >= 8 || !this.AllSupportedProducts.Contains(product))
        return;
      this.addProductDataFor(product, port);
    }

    public void RemoveProductToSort(ProductProto product)
    {
      OreSortingPlant.OreSortingPlantProductData plantProductData;
      if (!this.m_productsData.TryRemove(product, out plantProductData))
        return;
      this.m_autoLogisticsHelper.RemoveOutputBuffer((IProductBuffer) plantProductData.Buffer);
      this.m_outputBuffers.RemoveAndAssert((IProductBuffer) plantProductData.Buffer);
      this.Context.ProductsManager.ProductDestroyed(plantProductData.Buffer.Product, plantProductData.BeingSorted + plantProductData.UnsortedQuantity, DestroyReason.Cleared);
      this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) plantProductData.Buffer);
      this.MixedTotal -= plantProductData.UnsortedQuantity;
    }

    public void GetMixedInputProducts(Lyst<ProductQuantity> result)
    {
      foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
      {
        if (plantProductData.UnsortedQuantity.IsPositive)
          result.Add(plantProductData.Buffer.Product.WithQuantity(plantProductData.UnsortedQuantity));
      }
    }

    public bool CanAcceptMoreTrucksForUi(ProductProto product)
    {
      OreSortingPlant.OreSortingPlantProductData plantProductData;
      return this.m_productsData.TryGetValue(product, out plantProductData) && plantProductData.CanAcceptMoreTrucksForUi;
    }

    public bool CanAllProductsBeAcceptedForUi()
    {
      return this.m_productsData.All<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>>((Func<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>, bool>) (x => x.Value.CanAcceptMoreTrucksForUi));
    }

    public ImmutableArray<ProductProto> GetPortProducts(IoPort port)
    {
      return this.m_productsData.Values.Where<OreSortingPlant.OreSortingPlantProductData>((Func<OreSortingPlant.OreSortingPlantProductData, bool>) (x => (int) x.OutputPort == (int) port.Name)).Select<OreSortingPlant.OreSortingPlantProductData, ProductProto>((Func<OreSortingPlant.OreSortingPlantProductData, ProductProto>) (x => x.Buffer.Product)).ToImmutableArray<ProductProto>();
    }

    public int GetPortIndexFor(ProductProto product)
    {
      OreSortingPlant.OreSortingPlantProductData plantProductData;
      return this.m_productsData.TryGetValue(product, out plantProductData) ? this.m_outputsNames.IndexOf(plantProductData.OutputPort).Clamp(0, this.m_outputsNames.Length - 1) : 0;
    }

    public void SetProductPortIndex(ProductProto product, int index)
    {
      if (index < 0 || index >= this.m_outputsNames.Length)
        return;
      char outputsName = this.m_outputsNames[index];
      OreSortingPlant.OreSortingPlantProductData plantProductData;
      if (!this.m_productsData.TryGetValue(product, out plantProductData) || (int) plantProductData.OutputPort == (int) outputsName)
        return;
      plantProductData.OutputPort = outputsName;
      this.m_autoLogisticsHelper.ReRegisterOutputBufferIfAuto((IProductBuffer) plantProductData.Buffer);
    }

    public void SetProductBlockedAlert(ProductProto product, bool isAlertEnabled)
    {
      OreSortingPlant.OreSortingPlantProductData plantProductData;
      if (!this.m_productsData.TryGetValue(product, out plantProductData))
        return;
      plantProductData.NotifyIfFullOutput = isAlertEnabled;
    }

    public bool IsProductBlockedAlertEnabled(ProductProto product)
    {
      OreSortingPlant.OreSortingPlantProductData plantProductData;
      return this.m_productsData.TryGetValue(product, out plantProductData) && plantProductData.NotifyIfFullOutput;
    }

    public bool TakeProductsAndRemoveReservation(
      MixedCargoDeliveryJob deliveryJob,
      VehicleJobStatsManager statsManager)
    {
      this.m_jobs.TryRemoveJob((IVehicleJob) deliveryJob);
      this.updateReservedTotal();
      Truck truck = deliveryJob.Truck;
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        if (!this.m_productsData.ContainsKey(keyValuePair.Key))
          return false;
      }
      Quantity zero = Quantity.Zero;
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        this.m_productsData[keyValuePair.Key].UnsortedQuantity += keyValuePair.Value;
        statsManager.RecordJobStatsFor(truck, keyValuePair.Key.WithQuantity(keyValuePair.Value));
        zero += keyValuePair.Value;
      }
      this.MixedTotal += zero;
      truck.ClearCargoForSortingPlant();
      return true;
    }

    public bool CanAcceptTruck(Truck truck, out bool hadMatchingProducts)
    {
      hadMatchingProducts = false;
      if (this.IsNotEnabled || this.DoNotAcceptSingleProduct && truck.Cargo.Count == 1)
        return false;
      Quantity zero = Quantity.Zero;
      bool flag = false;
      foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
      {
        OreSortingPlant.OreSortingPlantProductData plantProductData;
        if (!this.m_productsData.TryGetValue(keyValuePair.Key, out plantProductData))
          return false;
        if (!plantProductData.CanAcceptMoreTrucks)
          flag = true;
        zero += keyValuePair.Value;
      }
      hadMatchingProducts = true;
      return !flag && zero <= this.CapacityLeft - this.m_reservedTotal;
    }

    private void updateMixedQuantity()
    {
      Quantity zero = Quantity.Zero;
      foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
        zero += plantProductData.UnsortedQuantity;
      this.MixedTotal = zero;
    }

    public bool Reserve(MixedCargoDeliveryJob job)
    {
      if (!this.m_jobs.TryAddJob((IVehicleJob) job))
        return false;
      this.updateReservedTotal();
      return true;
    }

    public void CancelReservation(MixedCargoDeliveryJob job)
    {
      this.m_jobs.TryRemoveJob((IVehicleJob) job);
      this.updateReservedTotal();
    }

    private void updateReservedTotal()
    {
      Quantity zero = Quantity.Zero;
      foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
        plantProductData.Reserved = Quantity.Zero;
      foreach (IVehicleJob allJob in (IEnumerable<IVehicleJob>) this.m_jobs.AllJobs)
      {
        Truck truck = allJob is MixedCargoDeliveryJob cargoDeliveryJob ? cargoDeliveryJob.Truck : (Truck) null;
        if (truck != null)
        {
          foreach (KeyValuePair<ProductProto, Quantity> keyValuePair in truck.Cargo)
          {
            OreSortingPlant.OreSortingPlantProductData plantProductData;
            if (this.m_productsData.TryGetValue(keyValuePair.Key, out plantProductData))
            {
              plantProductData.Reserved += keyValuePair.Value;
              zero += keyValuePair.Value;
            }
          }
        }
      }
      this.m_reservedTotal = zero;
    }

    protected override void OnEnabledChanged()
    {
      base.OnEnabledChanged();
      if (this.IsEnabled)
        this.m_vehicleQueue.Enable();
      else
        this.m_vehicleQueue.CancelJobsAndDisable();
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewMonth.Remove<OreSortingPlant>(this, new Action(this.onNewMonth));
      this.m_jobs.Destroy();
      this.m_vehicleQueue.CancelJobsAndDisable();
      foreach (OreSortingPlant.OreSortingPlantProductData plantProductData in this.m_productsData.Values)
      {
        this.Context.ProductsManager.ProductDestroyed(plantProductData.Buffer.Product, plantProductData.BeingSorted + plantProductData.UnsortedQuantity, DestroyReason.Cleared);
        this.Context.AssetTransactionManager.ClearAndDestroyBuffer((IProductBuffer) plantProductData.Buffer);
      }
      this.m_assignedOutputEntities.ForEachAndClear((Action<IEntityAssignedAsOutput>) (x => x.UnassignStaticInputEntity((IEntityAssignedAsInput) this)));
      base.OnDestroy();
    }

    public void AddToConfig(EntityConfigData data)
    {
      data.AllowedProducts = new ImmutableArray<KeyValuePair<string, ProductProto>>?(this.m_productsData.Values.Select<OreSortingPlant.OreSortingPlantProductData, KeyValuePair<string, ProductProto>>((Func<OreSortingPlant.OreSortingPlantProductData, KeyValuePair<string, ProductProto>>) (x => Make.Kvp<string, ProductProto>(x.OutputPort.ToString(), x.Buffer.Product))).ToImmutableArray<KeyValuePair<string, ProductProto>>());
      data.SetDoNotAcceptSingleProduct(this.DoNotAcceptSingleProduct);
      data.SetProductsToNotify(this.m_productsData.Where<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>>((Func<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>, bool>) (x => x.Value.NotifyIfFullOutput)).Select<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>, ProductProto>((Func<KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData>, ProductProto>) (x => x.Key)).ToImmutableArray<ProductProto>());
    }

    public void ApplyConfig(EntityConfigData data)
    {
      ImmutableArray<KeyValuePair<string, ProductProto>>? allowedProducts = data.AllowedProducts;
      if (allowedProducts.HasValue)
      {
        foreach (ProductProto immutable in this.m_productsData.Keys.ToImmutableArray<ProductProto>())
        {
          ProductProto product = immutable;
          if (!allowedProducts.Value.Any((Func<KeyValuePair<string, ProductProto>, bool>) (x => (Proto) x.Value == (Proto) product)))
            this.RemoveProductToSort(product);
        }
        foreach (KeyValuePair<string, ProductProto> keyValuePair in allowedProducts.Value)
        {
          OreSortingPlant.OreSortingPlantProductData plantProductData;
          if (this.m_productsData.TryGetValue(keyValuePair.Value, out plantProductData))
            plantProductData.OutputPort = keyValuePair.Key.FirstOrDefault<char>();
          else
            this.AddProductToSort(keyValuePair.Value, new char?(keyValuePair.Key.FirstOrDefault<char>()));
        }
      }
      ImmutableArray<ProductProto>? productsToNotify = data.GetProductsToNotify();
      if (productsToNotify.HasValue)
      {
        foreach (KeyValuePair<ProductProto, OreSortingPlant.OreSortingPlantProductData> keyValuePair in this.m_productsData)
          keyValuePair.Value.NotifyIfFullOutput = productsToNotify.Value.Contains(keyValuePair.Key);
      }
      this.DoNotAcceptSingleProduct = ((int) data.GetDoNotAcceptSingleProduct() ?? (this.DoNotAcceptSingleProduct ? 1 : 0)) != 0;
    }

    public bool CanDisableLogisticsInput => false;

    public bool CanDisableLogisticsOutput => true;

    public EntityLogisticsMode LogisticsInputMode => this.m_autoLogisticsHelper.LogisticsInputMode;

    public EntityLogisticsMode LogisticsOutputMode
    {
      get => this.m_autoLogisticsHelper.LogisticsOutputMode;
    }

    public void SetLogisticsInputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsInputMode(mode);
    }

    public void SetLogisticsOutputMode(EntityLogisticsMode mode)
    {
      this.m_autoLogisticsHelper.SetLogisticsOutputMode(mode);
    }

    public Quantity GetSortedLastMonth(ProductProto product)
    {
      OreSortingPlant.OreSortingPlantProductData plantProductData;
      return this.m_productsData.TryGetValue(product, out plantProductData) ? plantProductData.SortedLastMonth : Quantity.Zero;
    }

    public bool CanBeAssignedWithOutput(IEntityAssignedAsOutput entity)
    {
      return !this.m_assignedOutputEntities.Contains(entity) && entity is MineTower;
    }

    void IEntityAssignedAsInput.AssignStaticOutputEntity(IEntityAssignedAsOutput entity)
    {
      if (!this.CanBeAssignedWithOutput(entity))
        return;
      this.m_assignedOutputEntities.Add(entity);
    }

    void IEntityAssignedAsInput.UnassignStaticOutputEntity(IEntityAssignedAsOutput entity)
    {
      this.m_assignedOutputEntities.Remove(entity);
    }

    public bool TryGetVehicleQueueFor(
      Vehicle vehicle,
      out VehicleQueue<Vehicle, IStaticEntity> queue)
    {
      if (this.IsNotEnabled)
      {
        queue = (VehicleQueue<Vehicle, IStaticEntity>) null;
        return false;
      }
      queue = this.m_vehicleQueue;
      return true;
    }

    public static void Serialize(OreSortingPlant value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<OreSortingPlant>(value))
        return;
      writer.EnqueueDataSerialization((object) value, OreSortingPlant.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.AllowNonAssignedOutput);
      AnimationStatesProvider.Serialize(this.AnimationStatesProvider, writer);
      writer.WriteInt((int) this.CurrentState);
      writer.WriteBool(this.DoNotAcceptSingleProduct);
      Set<IEntityAssignedAsOutput>.Serialize(this.m_assignedOutputEntities, writer);
      AutoBufferLogisticsHelper.Serialize(this.m_autoLogisticsHelper, writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteGeneric<IElectricityConsumer>(this.m_electricityConsumer);
      VehicleJobs.Serialize(this.m_jobs, writer);
      EntityNotificator.Serialize(this.m_noProductsSetNotif, writer);
      EntityNotificator.Serialize(this.m_outputBlockedNotif, writer);
      Dict<ProductProto, OreSortingPlant.OreSortingPlantProductData>.Serialize(this.m_productsData, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<OreSortingPlantProto>(this.m_proto);
      TickTimer.Serialize(this.m_sortingTimer, writer);
      writer.WriteBool(this.m_tempBuffersFull);
      VehicleQueue<Vehicle, IStaticEntity>.Serialize(this.m_vehicleQueue, writer);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
    }

    public static OreSortingPlant Deserialize(BlobReader reader)
    {
      OreSortingPlant oreSortingPlant;
      if (reader.TryStartClassDeserialization<OreSortingPlant>(out oreSortingPlant))
        reader.EnqueueDataDeserialization((object) oreSortingPlant, OreSortingPlant.s_deserializeDataDelayedAction);
      return oreSortingPlant;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AllowNonAssignedOutput = reader.ReadBool();
      this.AnimationStatesProvider = AnimationStatesProvider.Deserialize(reader);
      this.CurrentState = (OreSortingPlant.State) reader.ReadInt();
      this.DoNotAcceptSingleProduct = reader.LoadedSaveVersion >= 153 && reader.ReadBool();
      reader.SetField<OreSortingPlant>(this, "m_assignedOutputEntities", (object) Set<IEntityAssignedAsOutput>.Deserialize(reader));
      reader.SetField<OreSortingPlant>(this, "m_autoLogisticsHelper", (object) AutoBufferLogisticsHelper.Deserialize(reader));
      reader.SetField<OreSortingPlant>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<OreSortingPlant>(this, "m_electricityConsumer", (object) reader.ReadGenericAs<IElectricityConsumer>());
      reader.SetField<OreSortingPlant>(this, "m_jobs", (object) VehicleJobs.Deserialize(reader));
      this.m_noProductsSetNotif = EntityNotificator.Deserialize(reader);
      this.m_outputBlockedNotif = reader.LoadedSaveVersion >= 156 ? EntityNotificator.Deserialize(reader) : new EntityNotificator();
      reader.SetField<OreSortingPlant>(this, "m_productsData", (object) Dict<ProductProto, OreSortingPlant.OreSortingPlantProductData>.Deserialize(reader));
      reader.SetField<OreSortingPlant>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      this.m_proto = reader.ReadGenericAs<OreSortingPlantProto>();
      reader.SetField<OreSortingPlant>(this, "m_sortingTimer", (object) TickTimer.Deserialize(reader));
      this.m_tempBuffersFull = reader.ReadBool();
      reader.SetField<OreSortingPlant>(this, "m_vehicleQueue", (object) VehicleQueue<Vehicle, IStaticEntity>.Deserialize(reader));
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      reader.RegisterInitAfterLoad<OreSortingPlant>(this, "initSelf", InitPriority.Low);
    }

    static OreSortingPlant()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      OreSortingPlant.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      OreSortingPlant.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    public enum State
    {
      Paused,
      Broken,
      Working,
      MissingInput,
      MissingWorkers,
      NotEnoughPower,
      [OnlyForSaveCompatibility(null)] FullOutput,
    }

    [GenerateSerializer(false, null, 0)]
    public class OreSortingPlantProductData
    {
      public readonly GlobalOutputBuffer Buffer;
      public readonly bool CanBeWasted;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public Quantity UnsortedQuantity { get; set; }

      public Quantity BeingSorted { get; set; }

      public PartialQuantity ToWaste { get; set; }

      public Quantity SortedLastMonth { get; set; }

      public Quantity SortedThisMonth { get; set; }

      [NewInSaveVersion(156, null, null, null, null)]
      public bool NotifyIfFullOutput { get; set; }

      public char OutputPort { get; set; }

      public bool CanAcceptMoreTrucksForUi
      {
        get => this.Buffer.Quantity + this.UnsortedQuantity <= this.Buffer.Capacity;
      }

      public bool CanAcceptMoreTrucks
      {
        get => this.Buffer.Quantity + this.UnsortedQuantity + this.Reserved <= this.Buffer.Capacity;
      }

      [DoNotSave(0, null)]
      public Quantity Reserved { get; set; }

      public OreSortingPlantProductData(GlobalOutputBuffer buffer, char outputPort)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Buffer = buffer;
        ProductProto product = buffer.Product;
        this.OutputPort = outputPort;
        this.CanBeWasted = !product.IsWaste && !product.Id.Value.Contains("Rock") && !product.Id.Value.Contains("Gravel") && !product.Id.Value.Contains("Slag") && !product.Id.Value.Contains("Dirt");
        this.NotifyIfFullOutput = product.Id.Value.Contains("Rock") || product.Id.Value.Contains("Dirt");
      }

      public void OnNewMonth()
      {
        this.SortedLastMonth = this.SortedThisMonth;
        this.SortedThisMonth = Quantity.Zero;
      }

      public static void Serialize(
        OreSortingPlant.OreSortingPlantProductData value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<OreSortingPlant.OreSortingPlantProductData>(value))
          return;
        writer.EnqueueDataSerialization((object) value, OreSortingPlant.OreSortingPlantProductData.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Quantity.Serialize(this.BeingSorted, writer);
        GlobalOutputBuffer.Serialize(this.Buffer, writer);
        writer.WriteBool(this.CanBeWasted);
        writer.WriteBool(this.NotifyIfFullOutput);
        writer.WriteChar(this.OutputPort);
        Quantity.Serialize(this.SortedLastMonth, writer);
        Quantity.Serialize(this.SortedThisMonth, writer);
        PartialQuantity.Serialize(this.ToWaste, writer);
        Quantity.Serialize(this.UnsortedQuantity, writer);
      }

      public static OreSortingPlant.OreSortingPlantProductData Deserialize(BlobReader reader)
      {
        OreSortingPlant.OreSortingPlantProductData plantProductData;
        if (reader.TryStartClassDeserialization<OreSortingPlant.OreSortingPlantProductData>(out plantProductData))
          reader.EnqueueDataDeserialization((object) plantProductData, OreSortingPlant.OreSortingPlantProductData.s_deserializeDataDelayedAction);
        return plantProductData;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.BeingSorted = Quantity.Deserialize(reader);
        reader.SetField<OreSortingPlant.OreSortingPlantProductData>(this, "Buffer", (object) GlobalOutputBuffer.Deserialize(reader));
        reader.SetField<OreSortingPlant.OreSortingPlantProductData>(this, "CanBeWasted", (object) reader.ReadBool());
        this.NotifyIfFullOutput = reader.LoadedSaveVersion >= 156 && reader.ReadBool();
        this.OutputPort = reader.ReadChar();
        this.SortedLastMonth = Quantity.Deserialize(reader);
        this.SortedThisMonth = Quantity.Deserialize(reader);
        this.ToWaste = PartialQuantity.Deserialize(reader);
        this.UnsortedQuantity = Quantity.Deserialize(reader);
      }

      static OreSortingPlantProductData()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        OreSortingPlant.OreSortingPlantProductData.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((OreSortingPlant.OreSortingPlantProductData) obj).SerializeData(writer));
        OreSortingPlant.OreSortingPlantProductData.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((OreSortingPlant.OreSortingPlantProductData) obj).DeserializeData(reader));
      }
    }
  }
}
