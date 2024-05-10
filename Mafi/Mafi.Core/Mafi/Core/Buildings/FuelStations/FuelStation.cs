// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.FuelStations.FuelStation
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout.Upgrade;
using Mafi.Core.Notifications;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Jobs;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Core.Vehicles.Trucks.JobProviders;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.FuelStations
{
  [GenerateSerializer(false, null, 0)]
  public class FuelStation : 
    StorageBase,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IEntityAssignedWithVehicles,
    IOutputBufferPriorityProvider,
    IEntityAssignedAsOutput,
    ILayoutEntity,
    IEntityWithSimUpdate
  {
    private FuelStationProto m_proto;
    private IFuelStatsCollector m_fuelStatsCollector;
    private readonly AssignedVehicles<Truck, TruckProto> m_assignedTrucks;
    private readonly FuelStationTruckJobProvider m_trucksJobProvider;
    private Set<IEntityAssignedAsInput> m_assignedInputEntities;
    private EntityNotificator m_notConnectedNotif;
    private EntityNotificator m_outOfFuelNotif;
    private bool m_failedToRefuelOwnVehicle;
    private TickTimer m_timer;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [DoNotSave(0, null)]
    public FuelStationProto Prototype
    {
      get => this.m_proto;
      protected set
      {
        this.m_proto = value;
        this.Prototype = (StorageBaseProto) value;
      }
    }

    public override bool CanBePaused => true;

    public ProductProto FuelProto => this.Prototype.FuelProto;

    public Quantity StoredFuel => this.Buffer.Value.Quantity;

    public IUpgrader Upgrader { get; private set; }

    public bool AllowTrucksToRefuel { get; private set; }

    public IIndexable<Vehicle> AllVehicles => this.m_assignedTrucks.AllUntyped;

    public bool CanRefuelOthers => this.IsEnabled && this.m_assignedTrucks.IsNotEmpty;

    public IReadOnlySet<IEntityAssignedAsInput> AssignedInputs
    {
      get => (IReadOnlySet<IEntityAssignedAsInput>) this.m_assignedInputEntities;
    }

    protected override bool ReportFullStorageCapacityInStats => false;

    public FuelStation(
      EntityId id,
      FuelStationProto proto,
      TileTransform transform,
      EntityContext context,
      ISimLoopEvents simLoopEvents,
      IFuelStationTruckJobProviderFactory jobProviderFactory,
      ILayoutEntityUpgraderFactory upgraderFactory,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      IFuelStatsCollector fuelStatsCollector)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CAllowTrucksToRefuel\u003Ek__BackingField = true;
      this.m_assignedInputEntities = new Set<IEntityAssignedAsInput>();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StorageBaseProto) proto, transform, context, simLoopEvents, vehicleBuffersRegistry);
      this.m_fuelStatsCollector = fuelStatsCollector;
      this.Prototype = proto;
      this.m_assignedTrucks = new AssignedVehicles<Truck, TruckProto>((IEntityAssignedWithVehicles) this);
      this.Upgrader = upgraderFactory.CreateInstance<FuelStationProto, FuelStation>(this, proto);
      this.m_notConnectedNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.FuelStationNotConnected);
      this.m_outOfFuelNotif = context.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.FuelStationOutOfFuel);
      Assert.That<bool>(this.TryAssignProduct(proto.FuelProto)).IsTrue();
      this.m_trucksJobProvider = jobProviderFactory.CreateJobProvider(this);
      this.m_timer = new TickTimer();
      this.m_timer.Start(60.Seconds());
      this.SetLogisticsInputDisabled(true);
    }

    protected override LogisticsBuffer CreateNewBuffer(Quantity capacity, ProductProto product)
    {
      return (LogisticsBuffer) new GlobalLogisticsInputBuffer(this.Prototype.Capacity, product, this.Context.ProductsManager, 10, (IEntity) this);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      bool flag1 = this.IsEnabled && this.m_failedToRefuelOwnVehicle && this.m_assignedTrucks.IsNotEmpty && this.Buffer.Value.IsEmpty;
      if (flag1)
        this.m_timer.DecrementOnly();
      else
        this.m_timer.Start(60.Seconds());
      bool flag2 = this.Ports.Any((Func<IoPort, bool>) (x => x.IsConnected));
      this.m_notConnectedNotif.NotifyIff(this.IsEnabled && !flag2 && this.IsEmpty && this.m_assignedTrucks.IsNotEmpty, (IEntity) this);
      this.m_outOfFuelNotif.NotifyIff(flag1 && this.m_timer.IsFinished, (IEntity) this);
    }

    public override bool IsProductSupported(ProductProto product)
    {
      return (Proto) product == (Proto) this.Prototype.FuelProto;
    }

    protected override Option<IInputBufferPriorityProvider> GetVehicleInputBufferPriority(
      LogisticsBuffer buffer)
    {
      return (Option<IInputBufferPriorityProvider>) (IInputBufferPriorityProvider) StaticPriorityProvider.LowestNoQuantityPreference;
    }

    protected override Option<IOutputBufferPriorityProvider> GetVehicleOutputBufferPriority(
      LogisticsBuffer buffer)
    {
      return (Option<IOutputBufferPriorityProvider>) (IOutputBufferPriorityProvider) this;
    }

    public bool TryRefuelTruck(Truck truck)
    {
      this.m_failedToRefuelOwnVehicle = this.Buffer.Value.IsEmpty;
      if (this.m_failedToRefuelOwnVehicle)
        return false;
      return this.tryRefuelTank(truck) || this.tryRefuelCargo(truck);
    }

    private bool tryRefuelCargo(Truck truck)
    {
      if (truck.IsFull || truck.Cargo.IsNotEmpty && !truck.Cargo.CanAdd(this.Buffer.Value.Product))
        return false;
      Quantity quantity = truck.RemainingCapacity.Min(this.Buffer.Value.Quantity).Min(this.Prototype.MaxTransferQuantityPerVehicle);
      if (!quantity.IsPositive)
        return false;
      truck.LoadCargoReturnExcess(this.Buffer.Value.Product.WithQuantity(quantity));
      this.Buffer.Value.RemoveExactly(quantity);
      return true;
    }

    private bool tryRefuelTank(Truck truck)
    {
      Quantity quantity = this.Buffer.Value.ProductQuantity.Quantity - truck.AddFuelAsMuchAs(this.Buffer.Value.ProductQuantity);
      if (!quantity.IsPositive)
        return false;
      this.Buffer.Value.RemoveExactly(quantity);
      this.m_fuelStatsCollector.ReportFuelUseAndDestroy(this.Buffer.Value.Product, quantity, FuelUsedBy.Vehicle);
      return true;
    }

    protected override void OnDestroy()
    {
      foreach (Vehicle immutable in this.m_assignedTrucks.All.ToImmutableArray<Truck>())
        this.UnassignVehicle(immutable, true);
      this.m_assignedInputEntities.ForEachAndClear((Action<IEntityAssignedAsInput>) (x => x.UnassignStaticOutputEntity((IEntityAssignedAsOutput) this)));
      base.OnDestroy();
    }

    void IUpgradableEntity.UpgradeSelf() => this.Prototype = this.Prototype.Upgrade.NextTier.Value;

    bool IUpgradableEntity.IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    internal void Cheat_FillWithFuel()
    {
      this.Context.ProductsManager.ProductCreated(this.FuelProto, this.Buffer.Value.Capacity - this.Buffer.Value.StoreAsMuchAs(this.Buffer.Value.Capacity), CreateReason.Cheated);
    }

    public void ToggleAllowTrucksToRefuel() => this.AllowTrucksToRefuel = !this.AllowTrucksToRefuel;

    public BufferStrategy GetOutputPriority(OutputPriorityRequest request)
    {
      return new BufferStrategy(16, this.AllowTrucksToRefuel ? 14 : 16, new Quantity?());
    }

    public bool CanBeAssignedWithInput(IEntityAssignedAsInput entity)
    {
      return !this.m_assignedInputEntities.Contains(entity) && entity is IAssignableToFuelStation;
    }

    public void AssignStaticInputEntity(IEntityAssignedAsInput entity)
    {
      if (!this.CanBeAssignedWithInput(entity))
        return;
      this.m_assignedInputEntities.Add(entity);
    }

    public void UnassignStaticInputEntity(IEntityAssignedAsInput entity)
    {
      this.m_assignedInputEntities.Remove(entity);
    }

    public bool CanVehicleBeAssigned(DynamicEntityProto vehicleProto)
    {
      if (!(vehicleProto is TruckProto truckProto))
        return false;
      return !truckProto.ProductType.HasValue || truckProto.ProductType.Value == this.FuelProto.Type;
    }

    public void AssignVehicle(Vehicle vehicle, bool doNotCancelJobs = false)
    {
      if (!(vehicle is Truck vehicle1))
      {
        Log.Error(string.Format("Trying to assign invalid vehicle '{0}'.", (object) vehicle.GetType()));
      }
      else
      {
        if (!this.m_assignedTrucks.AssignVehicle(vehicle1, doNotCancelJobs))
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
        if (!this.m_assignedTrucks.UnassignVehicle(vehicle1, cancelJobs))
          return;
        vehicle1.ResetJobProvider();
      }
    }

    public static void Serialize(FuelStation value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FuelStation>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FuelStation.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.AllowTrucksToRefuel);
      Set<IEntityAssignedAsInput>.Serialize(this.m_assignedInputEntities, writer);
      AssignedVehicles<Truck, TruckProto>.Serialize(this.m_assignedTrucks, writer);
      writer.WriteBool(this.m_failedToRefuelOwnVehicle);
      writer.WriteGeneric<IFuelStatsCollector>(this.m_fuelStatsCollector);
      EntityNotificator.Serialize(this.m_notConnectedNotif, writer);
      EntityNotificator.Serialize(this.m_outOfFuelNotif, writer);
      writer.WriteGeneric<FuelStationProto>(this.m_proto);
      TickTimer.Serialize(this.m_timer, writer);
      FuelStationTruckJobProvider.Serialize(this.m_trucksJobProvider, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static FuelStation Deserialize(BlobReader reader)
    {
      FuelStation fuelStation;
      if (reader.TryStartClassDeserialization<FuelStation>(out fuelStation))
        reader.EnqueueDataDeserialization((object) fuelStation, FuelStation.s_deserializeDataDelayedAction);
      return fuelStation;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AllowTrucksToRefuel = reader.ReadBool();
      this.m_assignedInputEntities = Set<IEntityAssignedAsInput>.Deserialize(reader);
      reader.SetField<FuelStation>(this, "m_assignedTrucks", (object) AssignedVehicles<Truck, TruckProto>.Deserialize(reader));
      this.m_failedToRefuelOwnVehicle = reader.ReadBool();
      this.m_fuelStatsCollector = reader.ReadGenericAs<IFuelStatsCollector>();
      this.m_notConnectedNotif = EntityNotificator.Deserialize(reader);
      this.m_outOfFuelNotif = EntityNotificator.Deserialize(reader);
      this.m_proto = reader.ReadGenericAs<FuelStationProto>();
      this.m_timer = TickTimer.Deserialize(reader);
      reader.SetField<FuelStation>(this, "m_trucksJobProvider", (object) FuelStationTruckJobProvider.Deserialize(reader));
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
    }

    static FuelStation()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FuelStation.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      FuelStation.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
