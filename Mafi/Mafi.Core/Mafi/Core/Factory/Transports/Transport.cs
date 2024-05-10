// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Transports.Transport
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Maintenance;
using Mafi.Core.Notifications;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Products;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles;
using Mafi.Localization;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Factory.Transports
{
  /// <summary>Transport.</summary>
  /// <remarks>
  /// Notes from transport optimization (Change-Id: I9988f39e22133491718cbe14b8135a69ac022232)
  /// 
  /// Transport trajectory representation was changed from segmented curve to discrete waypoints spaced out exactly
  /// by transport speed per tick. While this increases memory requirements for transports, it allows very significant
  /// optimizations for all processes. For example, transported products position on transport is just an index to
  /// waypoint array.
  /// 
  /// Changed transported product representation from large and complex class to 8-byte struct. This makes operations
  /// on transported products memory-efficient since they are all located very close together.
  /// 
  /// Significantly optimized transported product simulation, that is in many cases O(1), independent of the number
  /// of products transported:
  ///  - This works by having transported product index split into absolute index which is per transport,
  ///    and relative index per product.
  ///  - If all or none products are moved, only the global index needs to be incremented.
  ///  - Additionally, transported products remember if they are already at minimal spacing with previous product
  ///    and can avoid spacing check when moving.
  ///  - If all transported products are marked as "at minimal spacing", even transports that cannot move the first
  ///    product can skip checking all remaining products and just increment global index (and not iterate all
  ///    products to check if they can move).
  /// </remarks>
  [GenerateSerializer(false, null, 0)]
  public class Transport : 
    StaticEntity,
    ITransportFriend,
    IUpgradableEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity,
    IMaintainedEntity,
    IEntityWithGeneralPriority,
    IElectricityConsumingEntity,
    IEntityWithCloneableConfig,
    IEntityWithSimUpdate,
    IEntityWithGeneralPriorityFriend,
    IEntityWithPorts
  {
    private static readonly Percent PIPE_COLOR_CHANGE_PER_PRODUCT;
    /// <summary>
    /// Transport length is limited so that the number of waypoints never gets close to `short.MaxValue`.
    /// </summary>
    public const int MAX_TRANSPORT_WAYPOINTS = 16383;
    [ThreadStatic]
    private static Lyst<Tile2i> s_tilesTmp;
    private AssetValue m_value;
    private AssetValue m_constructionCost;
    [DoNotSave(0, null)]
    private ImmutableArray<OccupiedTileRelative> m_occupiedTilesCache;
    [DoNotSave(0, null)]
    private ImmutableArray<OccupiedVertexRelative> m_occupiedVerticesCache;
    [DoNotSave(0, null)]
    private LayoutTileConstraint m_occVertsCombinedConstraintCache;
    [DoNotSave(0, null)]
    private StaticEntityPfTargetTiles m_pfTargetTilesCache;
    private TransportTrajectory m_trajectory;
    private ImmutableArray<IoPort> m_ports;
    private IoPort m_startInputPort;
    private IoPort m_endOutputPort;
    [DoNotSave(0, null)]
    private IoPortData m_endOutputPortFast;
    private bool m_powerLow;
    [DoNotSave(0, null)]
    private bool m_canWorkOnLowPower;
    /// <summary>
    /// Products transported by the transport system in the order from oldest to newest (in order of delivery). Each
    /// entry represents one transported unit of a product. The transport has FIFO semantics as this queue, the front
    /// are the "oldest" products that will be delivered the first.
    /// </summary>
    private readonly Queueue<TransportedProductMutable> m_products;
    private Option<Lyst<Transport.TransportClearingBuffer>> m_clearingBuffers;
    private bool m_allProductsAreImmediatelyBehindFirstProduct;
    [NewInSaveVersion(140, null, "ColorRgba.Gray", null, null)]
    private ColorRgba m_transportColor;
    private Option<IElectricityConsumer> m_electricityConsumer;
    [DoNotSave(0, null)]
    private bool m_ignorePower;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly IProductsManager m_productsManager;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly TerrainManager m_terrainManager;
    [DoNotSave(101, null)]
    private EntityNotificator? m_tooLongTransportNotificator;
    [NewInSaveVersion(101, null, null, null, null)]
    private EntityNotificator m_tooLongTransportNotif;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    [NewInSaveVersion(140, null, null, null, null)]
    public ProductSlimId LastInsertedProduct { get; private set; }

    public TransportProto Prototype => this.m_trajectory.TransportProto;

    public override bool CanBePaused => true;

    public override AssetValue Value => this.m_value;

    public override AssetValue ConstructionCost => this.m_constructionCost;

    public int GeneralPriority { get; private set; }

    public virtual bool IsCargoAffectedByGeneralPriority => false;

    public bool IsGeneralPriorityVisible => !this.m_canWorkOnLowPower;

    void IEntityWithGeneralPriorityFriend.SetGeneralPriorityInternal(int priority)
    {
      this.GeneralPriority = priority;
      this.NotifyOnGeneralPriorityChange();
    }

    public override ImmutableArray<OccupiedTileRelative> OccupiedTiles
    {
      get
      {
        if (this.m_occupiedTilesCache.IsNotValid)
          this.m_occupiedTilesCache = TransportHelper.ComputeOccupiedTilesRelative(this.CenterTile, this.Trajectory, this.m_terrainManager);
        return this.m_occupiedTilesCache;
      }
    }

    public override ImmutableArray<OccupiedVertexRelative> OccupiedVertices
    {
      get
      {
        if (this.m_occupiedVerticesCache.IsNotValid)
          this.computeOccVerts();
        return this.m_occupiedVerticesCache;
      }
    }

    public override LayoutTileConstraint OccupiedVerticesCombinedConstraint
    {
      get
      {
        if (this.m_occupiedVerticesCache.IsNotValid)
          this.computeOccVerts();
        return this.m_occVertsCombinedConstraintCache;
      }
    }

    private void computeOccVerts()
    {
      this.m_occupiedVerticesCache = TransportHelper.ComputeOccupiedVertices(this.OccupiedTiles, this.Prototype.CanBeBuried);
      this.m_occVertsCombinedConstraintCache = this.m_occupiedVerticesCache.CombineConstraint();
    }

    public override ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>> VehicleSurfaceHeights
    {
      get => ImmutableArray<KeyValuePair<Tile2i, HeightTilesF>>.Empty;
    }

    public override StaticEntityPfTargetTiles PfTargetTiles
    {
      get
      {
        if (this.m_pfTargetTilesCache == null)
        {
          if (Transport.s_tilesTmp == null)
            Transport.s_tilesTmp = new Lyst<Tile2i>(64, true);
          Lyst<Tile2i> tilesTmp = Transport.s_tilesTmp;
          tilesTmp.Clear();
          Tile2i tile2i1 = this.Trajectory.Pivots.First.Xy;
          tilesTmp.Add(tile2i1);
          for (int index = 1; index < this.Trajectory.Pivots.Length; ++index)
          {
            Tile2i xy = this.Trajectory.Pivots[index].Xy;
            tilesTmp.Add(xy);
            RelTile2i relTile2i1 = xy - tile2i1;
            int sum = relTile2i1.Sum;
            if (sum > 15)
            {
              RelTile2i relTile2i2 = relTile2i1.Signs * 10;
              int num1 = sum / 10;
              Tile2i tile2i2 = tile2i1 + relTile2i2 / 2;
              int num2 = 0;
              while (num2 < num1)
              {
                tilesTmp.Add(tile2i2);
                ++num2;
                tile2i2 += relTile2i2;
              }
            }
            tile2i1 = xy;
          }
          this.m_pfTargetTilesCache = StaticEntityPfTargetTiles.FromGroundTiles(tilesTmp.ToImmutableArray());
        }
        return this.m_pfTargetTilesCache;
      }
    }

    /// <summary>Trajectory of this transport.</summary>
    public TransportTrajectory Trajectory => this.m_trajectory;

    /// <summary>
    /// Index of last pivot. Shorthand for <c>Trajectory.Pivots.Length - 1</c>.
    /// </summary>
    public int LastPivotIndex => this.Trajectory.Pivots.Length - 1;

    public Tile3i StartPosition => this.m_startInputPort.Position;

    public Tile3i EndPosition => this.m_endOutputPort.Position;

    /// <summary>
    /// Normalized direction of the transport start. This direction always points away from the trajectory.
    /// </summary>
    [DoNotSave(0, null)]
    public Direction903d StartDirection { get; private set; }

    /// <summary>
    /// Normalized direction of the transport end. This direction always points away from the trajectory.
    /// </summary>
    [DoNotSave(0, null)]
    public Direction903d EndDirection { get; private set; }

    public ImmutableArray<IoPort> Ports => this.m_ports;

    /// <summary>Input port that is at the start of the trajectory.</summary>
    public IoPort StartInputPort => this.m_startInputPort;

    /// <summary>Output port that is at the end of the trajectory.</summary>
    public IoPort EndOutputPort => this.m_endOutputPort;

    /// <summary>
    /// All transported products in the order from oldest to newest (in order of delivery).
    /// </summary>
    public IIndexable<TransportedProductMutable> TransportedProducts
    {
      get => (IIndexable<TransportedProductMutable>) this.m_products;
    }

    /// <summary>
    /// Use only for rendering that needs to update
    /// <see cref="F:Mafi.Core.Factory.Transports.TransportedProductMutable.LastSeenIndexAbsoluteForUi" />.
    /// </summary>
    Queueue<TransportedProductMutable> ITransportFriend.TransportedProductsMutable
    {
      get => this.m_products;
    }

    /// <summary>
    /// The product that is the closest to the output, the "first" or "oldest" in the queue.
    /// </summary>
    public TransportedProductMutable? FirstProduct
    {
      get => this.m_products.FirstOrNull<TransportedProductMutable>();
    }

    /// <summary>
    /// The product that is the closest to the input, the "last" or "youngest" in the queue.
    /// </summary>
    public TransportedProductMutable? LastProduct
    {
      get => this.m_products.LastOrNull<TransportedProductMutable>();
    }

    /// <summary>
    /// Whether this transport can receive products through ports.
    /// </summary>
    public bool CanReceiveProducts
    {
      get
      {
        return this.IsFullyConnected && (this.IsConstructed || this.ConstructionState == ConstructionState.PreparingUpgrade) && this.IsEnabled && !this.IsProductsRemovalInProgress;
      }
    }

    /// <summary>Total number of moved steps.</summary>
    public int MovedStepsTotal { get; private set; }

    /// <summary>
    /// Whether products on this transport were moved in the last sim step.  TODO: Only for tests? Kill?
    /// </summary>
    public bool IsMoving { get; private set; }

    /// <summary>
    /// True when both ports are connected. Note that the transport can move even if this is false.
    /// </summary>
    public bool IsFullyConnected { get; private set; }

    public TransportsManager TransportManager { get; private set; }

    public IUpgrader Upgrader { get; private set; }

    public IEntityMaintenanceProvider Maintenance { get; private set; }

    bool IMaintainedEntity.IsIdleForMaintenance => !this.IsMoving;

    public MaintenanceCosts MaintenanceCosts
    {
      get => this.Prototype.GetMaintenanceCosts(this.Trajectory.TrajectoryLength);
    }

    protected override bool IsEnabledNow => base.IsEnabledNow && this.Maintenance.CanWork();

    public override bool DoNotAdjustTerrainDuringConstruction => this.Prototype.CanBeBuried;

    /// <summary>
    /// This gets incremented by adding or removing of products from transport.
    /// It does not get incremented when transport moves but products don't change..
    /// </summary>
    public int ProductsStateVersion { get; private set; }

    public int ProductsIndexBase { get; private set; }

    public ColorRgba TransportColor => this.m_transportColor;

    [NewInSaveVersion(140, null, "new ColorRgba(0x333333)", null, null)]
    public ColorRgba TransportAccentColor { get; private set; }

    public Electricity PowerRequired { get; private set; }

    public Option<IElectricityConsumerReadonly> ElectricityConsumer
    {
      get => this.m_electricityConsumer.As<IElectricityConsumerReadonly>();
    }

    public bool IsTooLongTransportNotificationOn => this.m_tooLongTransportNotif.IsActive;

    public Transport(
      EntityId id,
      TransportTrajectory trajectory,
      EntityContext context,
      AssetValue value,
      AssetValue constructionCost,
      ISimLoopEvents simLoopEvents,
      TransportsManager transportManager,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      ITransportUpgraderFactory upgraderFactory,
      IEntityMaintenanceProvidersFactory maintenanceProvidersFactory,
      TerrainManager terrainManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_products = new Queueue<TransportedProductMutable>(true);
      this.m_transportColor = ColorRgba.Gray;
      // ISSUE: reference to a compiler-generated field
      this.\u003CTransportAccentColor\u003Ek__BackingField = (ColorRgba) 3355443;
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (StaticEntityProto) trajectory.TransportProto, context, Transport.getCenterTile(trajectory.Pivots));
      Assert.That<long>(trajectory.StartDirection.LengthSqr).IsEqualTo(1L, "Transport's start direction is not normalized!");
      Assert.That<long>(trajectory.EndDirection.LengthSqr).IsEqualTo(1L, "Transport's end direction is not normalized!");
      this.m_value = value;
      this.m_constructionCost = constructionCost;
      this.m_simLoopEvents = simLoopEvents;
      this.m_productsManager = this.Context.ProductsManager;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_terrainManager = terrainManager;
      this.TransportManager = transportManager;
      this.setTrajectory(trajectory);
      this.updateProperties();
      this.Upgrader = upgraderFactory.CreateInstance(this);
      this.Maintenance = maintenanceProvidersFactory.CreateFor((IMaintainedEntity) this);
      this.buildPorts();
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initAfterLoad(int saveVersion)
    {
      if (saveVersion < 101 && this.m_tooLongTransportNotificator.HasValue)
      {
        this.m_tooLongTransportNotificator.Value.Deactivate((IEntity) this);
        this.Context.NotificationsManager.RemoveAllNotificationFor((IEntity) this, this.m_tooLongTransportNotificator.Value.Prototype);
        if (this.Trajectory.Waypoints.Length > 16383)
        {
          EntityNotificator notificatorFor = this.TransportManager.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.TransportTooLong);
          notificatorFor.Activate((IEntity) this);
          this.m_tooLongTransportNotif = notificatorFor;
        }
      }
      this.updateProperties();
      this.m_endOutputPortFast = new IoPortData(this.m_endOutputPort);
      int length = this.m_trajectory.Waypoints.Length;
      bool flag = false;
      int index;
      for (index = 0; index < this.m_products.Count; ++index)
      {
        ref TransportedProductMutable local = ref this.m_products.GetRefAt(index);
        int num1 = (int) local.TrajectoryIndexRelative + this.ProductsIndexBase;
        if (num1 >= length)
        {
          if (saveVersion > 114)
            Log.Warning(string.Format("Invalid TP index: {0} (waypoints count {1})", (object) num1, (object) length));
          int num2 = length - 1 - this.ProductsIndexBase;
          local.TrajectoryIndexRelative = (short) num2;
          local.IsImmediatelyBehindNextProduct = false;
          flag = true;
        }
        else
          break;
      }
      if (flag)
      {
        for (; index < this.m_products.Count; ++index)
          this.m_products.GetRefAt(index).IsImmediatelyBehindNextProduct = false;
      }
      this.StartDirection = this.m_trajectory.StartDirection.ToDirection903d();
      this.EndDirection = this.m_trajectory.EndDirection.ToDirection903d();
    }

    private void updateProperties()
    {
      this.m_canWorkOnLowPower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.LogisticsCanWorkOnLowPower);
      if (this.m_canWorkOnLowPower)
        this.GeneralPriority = this.m_canWorkOnLowPower ? 0 : this.Trajectory.TransportProto.Costs.DefaultPriority;
      this.m_ignorePower = this.Context.PropertiesDb.GetValueAndRegisterForUpdates<bool>((IEntity) this, IdsCore.PropertyIds.LogisticsIgnorePower);
      this.updatePowerConsumption();
    }

    private void updatePowerConsumption()
    {
      this.PowerRequired = this.m_ignorePower ? Electricity.Zero : this.m_trajectory.TransportProto.GetElectricityConsumption(this.Trajectory.TrajectoryLength);
      if (this.PowerRequired.IsPositive && this.m_electricityConsumer.IsNone)
        this.m_electricityConsumer = this.Context.ElectricityConsumerFactory.CreateConsumer((IElectricityConsumingEntity) this).CreateOption<IElectricityConsumer>();
      this.m_electricityConsumer.ValueOrNull?.OnPowerRequiredChanged();
    }

    protected override void OnPropertiesChanged()
    {
      this.updateProperties();
      base.OnPropertiesChanged();
    }

    /// <summary>
    /// Returns tile in the center of transport. This affects icon position for example.
    /// </summary>
    private static Tile3i getCenterTile(ImmutableArray<Tile3i> pivots)
    {
      if (pivots.IsNotValidOrEmpty)
      {
        Log.Error("Invalid pivots");
        return new Tile3i();
      }
      if (pivots.Length == 1)
        return pivots.First;
      if (pivots.Length == 2)
        return pivots.First.Average(pivots.Last);
      int num1 = 0;
      Tile2i tile2i1 = pivots.First.Xy;
      Tile3i tile3i;
      RelTile2i absValue;
      for (int index = 1; index < pivots.Length; ++index)
      {
        tile3i = pivots[index];
        Tile2i xy = tile3i.Xy;
        RelTile2i relTile2i = xy - tile2i1;
        int num2 = num1;
        absValue = relTile2i.AbsValue;
        int sum = absValue.Sum;
        num1 = num2 + sum;
        tile2i1 = xy;
      }
      int num3 = num1 / 2;
      tile3i = pivots.First;
      Tile2i tile2i2 = tile3i.Xy;
      for (int index = 1; index < pivots.Length; ++index)
      {
        tile3i = pivots[index];
        Tile2i xy = tile3i.Xy;
        absValue = (xy - tile2i2).AbsValue;
        int sum = absValue.Sum;
        num3 -= sum;
        if (num3 <= 0)
        {
          if (sum <= 0)
            return pivots[index];
          tile3i = pivots[index];
          return tile3i.Lerp(pivots[index - 1], (long) -num3, (long) sum);
        }
        tile2i2 = xy;
      }
      Assert.Fail("Mid segment not found. This should not happen.");
      return pivots[pivots.Length / 2];
    }

    private void setTrajectory(TransportTrajectory newTrajectory)
    {
      if (newTrajectory.Waypoints.Length > 16383)
      {
        EntityNotificator notificatorFor = this.TransportManager.NotificationsManager.CreateNotificatorFor(IdsCore.Notifications.TransportTooLong);
        notificatorFor.Activate((IEntity) this);
        this.m_tooLongTransportNotif = notificatorFor;
      }
      else if (this.m_tooLongTransportNotif.IsValid)
      {
        this.m_tooLongTransportNotif.Deactivate((IEntity) this);
        this.m_tooLongTransportNotif = new EntityNotificator();
      }
      this.m_trajectory = newTrajectory;
      this.Prototype = (StaticEntityProto) newTrajectory.TransportProto;
      this.StartDirection = newTrajectory.StartDirection.ToDirection903d();
      this.EndDirection = newTrajectory.EndDirection.ToDirection903d();
      this.updatePowerConsumption();
    }

    private void resetTrajectory(TransportTrajectory newTrajectory)
    {
      Assert.That<TransportTrajectory>(this.m_trajectory).IsNotNull<TransportTrajectory>();
      this.setTrajectory(newTrajectory);
      this.reinitializePorts();
      this.TransportManager.ReportTrajectoryChanged(this);
    }

    private void reinitializePorts()
    {
      IIoPortsManager ioPortsManager = this.Context.IoPortsManager;
      ioPortsManager.DisconnectAndRemove(this.m_startInputPort);
      ioPortsManager.DisconnectAndRemove(this.m_endOutputPort);
      this.buildPorts();
      ioPortsManager.AddPortAndTryConnect(this.m_startInputPort);
      ioPortsManager.AddPortAndTryConnect(this.m_endOutputPort);
    }

    public void OnPortConnectionChanged(IoPort ourPort, IoPort otherPort)
    {
      if ((int) ourPort.Name == (int) this.m_endOutputPort.Name)
        this.m_endOutputPortFast = new IoPortData(this.m_endOutputPort);
      else
        Assert.That<bool>((int) ourPort.Name == (int) this.m_startInputPort.Name).IsTrue();
      this.IsFullyConnected = this.m_startInputPort.IsConnected & this.m_endOutputPort.IsConnected;
    }

    private void buildPorts()
    {
      this.m_startInputPort = new IoPort(this.Context.PortIdFactory.GetNextId(), (IEntityWithPorts) this, new PortSpec('I', IoPortType.Input, this.Prototype.PortsShape, false), this.m_trajectory.Pivots.First, this.StartDirection, 0, true);
      this.m_endOutputPort = new IoPort(this.Context.PortIdFactory.GetNextId(), (IEntityWithPorts) this, new PortSpec('O', IoPortType.Output, this.Prototype.PortsShape, false), this.m_trajectory.Pivots.Last, this.EndDirection, 1, true);
      this.m_ports = ImmutableArray.Create<IoPort>(this.m_startInputPort, this.m_endOutputPort);
      this.m_endOutputPortFast = new IoPortData(this.m_endOutputPort);
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.IsMoving = false;
      if (this.IsNotEnabled || this.IsProductsRemovalInProgress)
        return;
      this.m_powerLow = this.m_electricityConsumer.HasValue && !this.m_electricityConsumer.Value.CanConsume(this.m_canWorkOnLowPower);
      if (this.m_powerLow && !this.m_canWorkOnLowPower || !this.tryMoveProducts())
        return;
      ++this.MovedStepsTotal;
      this.IsMoving = true;
      if (this.m_powerLow)
        return;
      IElectricityConsumer valueOrNull = this.m_electricityConsumer.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull.ConsumeAndAssert();
    }

    protected override void OnDestroy()
    {
      ImmutableArray<ProductProto> managedProtos = this.m_productsManager.SlimIdManager.ManagedProtos;
      foreach (TransportedProductMutable product in this.m_products)
        this.Context.AssetTransactionManager.StoreClearedProduct(managedProtos[(int) product.SlimId.Value].WithQuantity(product.Quantity));
      this.m_products.Clear();
      ++this.ProductsStateVersion;
      this.Context.IoPortsManager.Disconnect(this.m_startInputPort);
      this.Context.IoPortsManager.Disconnect(this.m_endOutputPort);
      this.IsFullyConnected = false;
      base.OnDestroy();
    }

    public int? GetFirstProductIndexFromStartUntilTrajPos(RelTile1f maxTrajPosition)
    {
      int intFloored = (maxTrajPosition.Value / this.Prototype.SpeedPerTick.Value).ToIntFloored();
      int productsIndexBase = this.ProductsIndexBase;
      int? startUntilTrajPos = new int?();
      for (int index = this.m_products.Count - 1; index >= 0; --index)
      {
        TransportedProductMutable product = this.m_products[index];
        if (productsIndexBase + (int) product.TrajectoryIndexRelative <= intFloored)
          startUntilTrajPos = new int?(index);
        else
          break;
      }
      return startUntilTrajPos;
    }

    public int? GetFirstProductIndexFromEndUntilTrajPos(RelTile1f minTrajPosition)
    {
      int intCeiled = (minTrajPosition.Value / this.Prototype.SpeedPerTick.Value).ToIntCeiled();
      int productsIndexBase = this.ProductsIndexBase;
      int? fromEndUntilTrajPos = new int?();
      int index = 0;
      for (int count = this.m_products.Count; index < count; ++index)
      {
        TransportedProductMutable product = this.m_products[index];
        if (productsIndexBase + (int) product.TrajectoryIndexRelative >= intCeiled)
          fromEndUntilTrajPos = new int?(index);
        else
          break;
      }
      return fromEndUntilTrajPos;
    }

    public AssetValue ClearAndReturnTransportedProducts()
    {
      AssetValueBuilder pooledInstance = AssetValueBuilder.GetPooledInstance();
      ImmutableArray<ProductProto> managedProtos = this.m_productsManager.SlimIdManager.ManagedProtos;
      while (this.m_products.IsNotEmpty)
      {
        TransportedProductMutable transportedProductMutable = this.m_products.Dequeue();
        pooledInstance.Add(managedProtos[(int) transportedProductMutable.SlimId.Value].WithQuantity(transportedProductMutable.Quantity));
      }
      ++this.ProductsStateVersion;
      this.m_allProductsAreImmediatelyBehindFirstProduct = false;
      this.LastInsertedProduct = ProductSlimId.PhantomId;
      return pooledInstance.GetAssetValueAndReturnToPool();
    }

    /// <summary>
    /// Transfers products from given transport to this transport assuming that the two transports share the same
    /// trajectory prefix (one is subset of the other, from start).
    /// </summary>
    /// <param name="source">Source transport</param>
    /// <param name="srcProductsDistFromStartBias">Bias applied to the distance of all transferred products on the
    /// source transport. This can be used to transfer products from the "middle" of the transport. However, no
    /// products are allowed to be in the "skipped" transport part.</param>
    public void TransferProductsSharedStart(
      Transport source,
      RelTile1f srcProductsDistFromStartBias = default (RelTile1f))
    {
      if (srcProductsDistFromStartBias == new RelTile1f())
        Assert.That<Tile3i>(source.Trajectory.Pivots.First).IsEqualTo<Tile3i>(this.Trajectory.Pivots.First);
      Queueue<TransportedProductMutable> products = source.m_products;
      if (products.IsEmpty)
        return;
      if (this.ProductsIndexBase != 0)
        this.normalizeProductIndices();
      int productsIndexBase = source.ProductsIndexBase;
      RelTile1f speedPerTick1 = source.Prototype.SpeedPerTick;
      RelTile1f speedPerTick2 = this.Prototype.SpeedPerTick;
      RelTile1f relTile1f1 = this.m_trajectory.TrajectoryLength - srcProductsDistFromStartBias;
      int num1 = this.m_trajectory.Waypoints.Length - 1;
      int count1 = this.m_products.Count;
      int count2 = products.Count;
      if (srcProductsDistFromStartBias.IsNegative)
        Assert.That<RelTile1f>((productsIndexBase + (int) products.Last.TrajectoryIndexRelative) * speedPerTick1 + srcProductsDistFromStartBias).IsNotNegative("The first product to transfer is not on destination transport, incorrect bias?.");
      int index1;
      for (index1 = count2 - 1; index1 >= 0; --index1)
      {
        TransportedProductMutable transportedProductMutable = products[index1];
        if ((productsIndexBase + (int) transportedProductMutable.TrajectoryIndexRelative) * speedPerTick1 > relTile1f1)
          break;
      }
      for (int index2 = index1 + 1; index2 < count2; ++index2)
      {
        TransportedProductMutable transportedProductMutable1 = products[index2];
        RelTile1f relTile1f2 = (productsIndexBase + (int) transportedProductMutable1.TrajectoryIndexRelative) * speedPerTick1 + srcProductsDistFromStartBias;
        Assert.That<RelTile1f>(relTile1f2).IsNotNegative();
        int num2 = (relTile1f2.Value / speedPerTick2.Value).ToIntRounded().Min(num1);
        if (num2 < 0)
        {
          Log.Error("Dst index is negative, attempting to transfer products that are 'in front' of destination transport due to bias?");
          num2 = 0;
        }
        TransportedProductMutable transportedProductMutable2 = new TransportedProductMutable(transportedProductMutable1.SlimId, (short) num2, transportedProductMutable1.Quantity, (ushort) num2, transportedProductMutable1.SeqNumber);
        this.m_products.Enqueue(transportedProductMutable2);
        if (this.Prototype.Graphics.UsePerProductColoring)
          this.updateTransportColor(transportedProductMutable2.SlimId);
        this.LastInsertedProduct = transportedProductMutable2.SlimId;
      }
      for (int index3 = index1 + 1; index3 < count2; ++index3)
        products.PopLast();
      Assert.That<int>(count2 + count1).IsEqualTo(products.Count + this.m_products.Count);
      ++this.ProductsStateVersion;
      ++source.ProductsStateVersion;
      this.m_allProductsAreImmediatelyBehindFirstProduct = false;
      source.m_allProductsAreImmediatelyBehindFirstProduct = false;
    }

    /// <summary>
    /// Transfers products from given transport to this transport assuming that the two transports share the same
    /// trajectory suffix (one is subset of the other, from end).
    /// 
    /// Note: This is more efficient if there are not too many extra products to transfer. If
    /// <see cref="M:Mafi.Core.Factory.Transports.Transport.TransferProductsSharedStart(Mafi.Core.Factory.Transports.Transport,Mafi.RelTile1f)" /> is also being called, call it first.
    /// </summary>
    public void TransferProductsSharedEnd(Transport source)
    {
      Assert.That<Tile3i>(source.Trajectory.Pivots.Last).IsEqualTo<Tile3i>(this.Trajectory.Pivots.Last);
      Queueue<TransportedProductMutable> products = source.m_products;
      if (products.IsEmpty)
        return;
      if (this.ProductsIndexBase != 0)
        this.normalizeProductIndices();
      int productsIndexBase = source.ProductsIndexBase;
      RelTile1f speedPerTick1 = source.Prototype.SpeedPerTick;
      RelTile1f trajectoryLength1 = source.Trajectory.TrajectoryLength;
      RelTile1f speedPerTick2 = this.Prototype.SpeedPerTick;
      RelTile1f trajectoryLength2 = this.m_trajectory.TrajectoryLength;
      int num1 = this.m_trajectory.Waypoints.Length - 1;
      int count1 = this.m_products.Count;
      int count2 = products.Count;
      int index1;
      for (index1 = 0; index1 < count2; ++index1)
      {
        TransportedProductMutable transportedProductMutable = products[index1];
        int num2 = productsIndexBase + (int) transportedProductMutable.TrajectoryIndexRelative;
        if (trajectoryLength1 - num2 * speedPerTick1 > trajectoryLength2)
          break;
      }
      for (int index2 = index1 - 1; index2 >= 0; --index2)
      {
        TransportedProductMutable transportedProductMutable1 = products[index2];
        int num3 = productsIndexBase + (int) transportedProductMutable1.TrajectoryIndexRelative;
        RelTile1f relTile1f = trajectoryLength1 - num3 * speedPerTick1;
        int num4 = ((trajectoryLength2 - relTile1f).Value / speedPerTick2.Value).ToIntRounded().Min(num1);
        TransportedProductMutable transportedProductMutable2 = new TransportedProductMutable(transportedProductMutable1.SlimId, (short) num4, transportedProductMutable1.Quantity, (ushort) num4, transportedProductMutable1.SeqNumber);
        this.m_products.EnqueueFirst(transportedProductMutable2);
        if (this.Prototype.Graphics.UsePerProductColoring)
          this.updateTransportColor(transportedProductMutable2.SlimId);
        this.LastInsertedProduct = transportedProductMutable2.SlimId;
      }
      for (int index3 = index1 - 1; index3 >= 0; --index3)
        products.Dequeue();
      Assert.That<int>(count2 + count1).IsEqualTo(products.Count + this.m_products.Count);
      ++this.ProductsStateVersion;
      ++source.ProductsStateVersion;
      this.m_allProductsAreImmediatelyBehindFirstProduct = false;
      source.m_allProductsAreImmediatelyBehindFirstProduct = false;
    }

    private void realignProductsToNewTrajectory(TransportTrajectory oldTrajectory)
    {
      RelTile1f speedPerTick1 = oldTrajectory.TransportProto.SpeedPerTick;
      RelTile1f speedPerTick2 = this.Prototype.SpeedPerTick;
      int productsIndexBase = this.ProductsIndexBase;
      this.ProductsIndexBase = 0;
      int num1 = this.m_trajectory.Waypoints.Length - 1;
      for (int index = 0; index < this.m_products.Count; ++index)
      {
        ref TransportedProductMutable local = ref this.m_products.GetRefAt(index);
        int num2 = (((productsIndexBase + (int) local.TrajectoryIndexRelative) * speedPerTick1).Value / speedPerTick2.Value).ToIntRounded().Min(num1);
        local.TrajectoryIndexRelative = (short) num2;
        local.LastSeenIndexAbsoluteForUi = (ushort) num2;
        local.IsImmediatelyBehindNextProduct = false;
      }
      ++this.ProductsStateVersion;
      this.m_allProductsAreImmediatelyBehindFirstProduct = false;
    }

    public bool HasValidProductsOrder(out string error)
    {
      for (int index = 1; index < this.m_products.Count; ++index)
      {
        if ((int) this.m_products[index].TrajectoryIndexRelative > (int) this.m_products[index - 1].TrajectoryIndexRelative)
        {
          error = string.Format("Invalid products order at indices {0} ", (object) (index - 1)) + string.Format("(dist {0}) ", (object) (this.ProductsIndexBase + (int) this.m_products[index - 1].TrajectoryIndexRelative)) + string.Format("and {0} (dist {1}) which should be ", (object) index, (object) (this.ProductsIndexBase + (int) this.m_products[index].TrajectoryIndexRelative)) + "behind it (indexed from end, trajectory distance from start).";
          return false;
        }
      }
      error = "";
      return true;
    }

    public bool HasValidProductsSpacing(out string error)
    {
      for (int index = 1; index < this.m_products.Count; ++index)
      {
        int num = (int) this.m_products[index - 1].TrajectoryIndexRelative - (int) this.m_products[index].TrajectoryIndexRelative;
        if (num < this.Prototype.ProductSpacingWaypoints)
        {
          error = string.Format("Invalid products spacing {0} at indices {1} ", (object) num, (object) (index - 1)) + string.Format("(dis {0}) ", (object) (this.ProductsIndexBase + (int) this.m_products[index - 1].TrajectoryIndexRelative)) + string.Format("and {0} (dist {1}), ", (object) index, (object) (this.ProductsIndexBase + (int) this.m_products[index].TrajectoryIndexRelative)) + string.Format("min is {0}.", (object) this.Prototype.ProductSpacingWaypoints);
          return false;
        }
      }
      error = "";
      return true;
    }

    /// <summary>
    /// Tries to move products and returns whether any products were moved.
    /// </summary>
    private bool tryMoveProducts()
    {
      Queueue<TransportedProductMutable> products = this.m_products;
      if (products.IsEmpty || this.m_tooLongTransportNotif.IsActive)
        return false;
      ref TransportedProductMutable local1 = ref products.GetRefFirst();
      int num1 = this.ProductsIndexBase + (int) local1.TrajectoryIndexRelative + 1;
      ImmutableArray<TransportWaypoint> waypoints = this.m_trajectory.Waypoints;
      int length1 = waypoints.Length;
      if (num1 < length1)
      {
        this.moveAllProducts();
        return true;
      }
      if (this.trySendToOutput(ref local1))
      {
        products.Dequeue();
        ++this.ProductsStateVersion;
        if (products.IsNotEmpty)
        {
          int num2 = this.ProductsIndexBase + (int) products.First.TrajectoryIndexRelative + 1;
          waypoints = this.m_trajectory.Waypoints;
          int length2 = waypoints.Length;
          if (num2 < length2)
            this.moveAllProducts();
        }
        return true;
      }
      if (this.m_allProductsAreImmediatelyBehindFirstProduct)
        return false;
      bool flag = true;
      for (int index = 1; index < products.Count; ++index)
      {
        if (!products[index].IsImmediatelyBehindNextProduct)
        {
          int num3 = (int) products[index - 1].TrajectoryIndexRelative - (int) products[index].TrajectoryIndexRelative - this.m_trajectory.TransportProto.ProductSpacingWaypoints;
          if (num3 > 0)
          {
            if (num3 == 1)
            {
              ref TransportedProductMutable local2 = ref products.GetRefAt(index);
              ++local2.TrajectoryIndexRelative;
              local2.IsImmediatelyBehindNextProduct = true;
              ++index;
            }
            for (; index < products.Count; ++index)
              ++products.GetRefAt(index).TrajectoryIndexRelative;
            return true;
          }
          if (num3 == 0)
            products.GetRefAt(index).IsImmediatelyBehindNextProduct = true;
          flag = false;
        }
      }
      this.m_allProductsAreImmediatelyBehindFirstProduct = flag;
      return false;
    }

    private bool trySendToOutput(ref TransportedProductMutable firstProduct)
    {
      if (!this.m_endOutputPortFast.IsConnected)
        return false;
      ProductProto product = this.m_productsManager.SlimIdManager.ResolveOrPhantom(firstProduct.SlimId);
      if (product.IsPhantom)
      {
        Log.Error("Attempting to send phantom, discarding.");
        return true;
      }
      Quantity quantity = this.m_endOutputPortFast.SendAsMuchAs(product.WithQuantity(firstProduct.Quantity));
      if (!quantity.IsPositive)
        return true;
      firstProduct.Quantity = quantity;
      return false;
    }

    private void moveAllProducts()
    {
      ++this.ProductsIndexBase;
      if (this.ProductsIndexBase < (int) short.MaxValue)
        return;
      this.normalizeProductIndices();
    }

    private void normalizeProductIndices()
    {
      for (int index = 0; index < this.m_products.Count; ++index)
        this.m_products.GetRefAt(index).TrajectoryIndexRelative += (short) this.ProductsIndexBase;
      this.ProductsIndexBase = 0;
    }

    public Transport.Status GetStatus()
    {
      if (!this.IsFullyConnected)
        return Transport.Status.NotConnected;
      if (!this.IsEnabled)
        return Transport.Status.Paused;
      if (this.m_powerLow)
        return Transport.Status.PowerLow;
      return !this.IsMoving ? Transport.Status.Idle : Transport.Status.Moving;
    }

    public bool IsUpgradeAvailable(out LocStrFormatted errorMessage)
    {
      errorMessage = LocStrFormatted.Empty;
      return true;
    }

    void IUpgradableEntity.UpgradeSelf()
    {
      if (this.Prototype.Upgrade.NextTier.IsNone)
      {
        Log.Warning(string.Format("Cannot upgrade: {0}", (object) this));
      }
      else
      {
        TransportProto proto = this.Prototype.Upgrade.NextTier.Value;
        Assert.That<IoPortShapeProto>(this.Prototype.PortsShape).IsEqualTo<IoPortShapeProto>(proto.PortsShape);
        TransportTrajectory trajectory = this.m_trajectory;
        this.resetTrajectory(this.Trajectory.CreateCopyWithNewProto(proto));
        this.m_value = this.m_constructionCost = this.Trajectory.Price;
        ImmutableArray<TransportWaypoint> waypoints = trajectory.Waypoints;
        int length1 = waypoints.Length;
        waypoints = this.m_trajectory.Waypoints;
        int length2 = waypoints.Length;
        if (length1 == length2)
          return;
        this.realignProductsToNewTrajectory(trajectory);
      }
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (pq.IsEmpty || !this.CanReceiveProducts)
        return pq.Quantity;
      if (this.Prototype.PortsShape.AllowedProductType != pq.Product.Type)
      {
        Assert.Fail<ProductType, ProductType>("Transport for '{0}' is receiving incorrect product type '{1}'.", this.Prototype.PortsShape.AllowedProductType, pq.Product.Type);
        return pq.Quantity;
      }
      Queueue<TransportedProductMutable> products = this.m_products;
      if (!this.Prototype.AllowMixedProducts && products.IsNotEmpty && products.First.SlimId != pq.Product.SlimId)
        return pq.Quantity;
      Quantity maxRemovedQuantity = pq.Product.MaxQuantityPerTransportedProduct.Min(this.Prototype.MaxQuantityPerTransportedProduct);
      int num = 1;
      if (products.IsNotEmpty)
      {
        ref TransportedProductMutable local = ref products.GetRefLast();
        num = (int) local.TrajectoryIndexRelative + this.ProductsIndexBase - this.m_trajectory.TransportProto.ProductSpacingWaypoints;
        if (num < 0)
        {
          if (!(local.SlimId == pq.Product.SlimId) || !(local.Quantity < maxRemovedQuantity))
            return pq.Quantity;
          Quantity quantity = (maxRemovedQuantity - local.Quantity).Min(pq.Quantity);
          local.Quantity += quantity;
          if (this.Prototype.Graphics.UsePerProductColoring)
            this.updateTransportColor(local.SlimId);
          return pq.Quantity - quantity;
        }
      }
      ProductQuantity remainder;
      ProductQuantity productQuantity = pq.Remove(maxRemovedQuantity, out remainder);
      TransportedProductMutable transportedProductMutable = new TransportedProductMutable(productQuantity.Product.SlimId, (short) -this.ProductsIndexBase, productQuantity.Quantity, (ushort) 0, this.TransportManager.GetNextSeqNumber());
      if (num == 0)
        transportedProductMutable.IsImmediatelyBehindNextProduct = true;
      else
        this.m_allProductsAreImmediatelyBehindFirstProduct = false;
      products.Enqueue(transportedProductMutable);
      if (this.Prototype.Graphics.UsePerProductColoring)
        this.updateTransportColor(transportedProductMutable.SlimId);
      ++this.ProductsStateVersion;
      this.LastInsertedProduct = transportedProductMutable.SlimId;
      return remainder.Quantity;
    }

    /// <summary>
    /// Returns whether the transport is flat at and around given position. If the position is a pivot, it makes
    /// sure that both neighboring pivots are on the same Z. If then position is a segment, it makes sure that the both
    /// surrounding pivots are on the same Z.
    /// </summary>
    public bool IsFlatAround(Tile3i position)
    {
      int index;
      bool isAtPivot;
      if (!this.Trajectory.TryGetLowPivotIndexFor(position, out index, out isAtPivot))
        return false;
      int z = this.Trajectory.Pivots[index].Z;
      if (!isAtPivot)
        return this.Trajectory.Pivots[index + 1].Z == z;
      return (index <= 0 || this.Trajectory.Pivots[index - 1].Z == z) && (index >= this.Trajectory.Pivots.LastIndex || this.Trajectory.Pivots[index + 1].Z == z);
    }

    public bool IsProductsRemovalInProgress
    {
      get => this.m_clearingBuffers.HasValue && this.m_clearingBuffers.Value.IsNotEmpty;
    }

    public void CancelProductsRemoval()
    {
      if (!this.m_clearingBuffers.HasValue)
        return;
      this.m_clearingBuffers.Value.ForEachAndClear((Action<Transport.TransportClearingBuffer>) (buffer => buffer.Destroy()));
    }

    public void RequestProductsRemoval()
    {
      if (this.IsDestroyed || this.ConstructionState != ConstructionState.Constructed)
      {
        Log.Warning(string.Format("Cannot start products removal, invalid construction state: {0}", (object) this.ConstructionState));
      }
      else
      {
        Dict<ProductProto, Transport.TransportClearingBuffer> dict = new Dict<ProductProto, Transport.TransportClearingBuffer>();
        Set<ProductProto> set = new Set<ProductProto>();
        if (this.m_clearingBuffers.HasValue)
          this.m_clearingBuffers.Value.ForEachAndClear((Action<Transport.TransportClearingBuffer>) (buffer => buffer.Destroy()));
        else
          this.m_clearingBuffers = (Option<Lyst<Transport.TransportClearingBuffer>>) new Lyst<Transport.TransportClearingBuffer>();
        ImmutableArray<ProductProto> managedProtos = this.m_productsManager.SlimIdManager.ManagedProtos;
        foreach (TransportedProductMutable transportedProduct in this.TransportedProducts)
        {
          ProductProto productProto = managedProtos[(int) transportedProduct.SlimId.Value];
          if (productProto.CanBeDiscarded && !productProto.IsWaste)
            set.Add(productProto);
          else if (productProto.CanBeLoadedOnTruck && !dict.ContainsKey(productProto))
          {
            Transport.TransportClearingBuffer transportClearingBuffer = new Transport.TransportClearingBuffer(this, productProto);
            dict.Add(productProto, transportClearingBuffer);
          }
        }
        this.m_clearingBuffers.Value.AddRange((IEnumerable<Transport.TransportClearingBuffer>) dict.Values);
        foreach (ProductProto product in set)
        {
          Quantity removed;
          this.removeProductOfType(product, Quantity.MaxValue, out removed);
          this.m_productsManager.ClearProduct(product, removed);
        }
      }
    }

    /// <summary>Returns if all instances of product were removed.</summary>
    private bool removeProductOfType(
      ProductProto product,
      Quantity maxQuantity,
      out Quantity removed)
    {
      removed = Quantity.Zero;
      ProductSlimId slimId = product.SlimId;
      for (int index = 0; index < this.m_products.Count; ++index)
      {
        ref TransportedProductMutable local = ref this.m_products.GetRefAt(index);
        if (!(local.SlimId != slimId))
        {
          removed += local.Quantity;
          if (removed <= maxQuantity)
          {
            this.m_products.RemoveAt(index);
            ++this.ProductsStateVersion;
            --index;
            this.m_allProductsAreImmediatelyBehindFirstProduct = false;
          }
          else
          {
            local.Quantity = removed - maxQuantity;
            removed = maxQuantity;
            return false;
          }
        }
      }
      return true;
    }

    public TransportWaypoint GetProductPose(TransportedProductMutable tp)
    {
      int index = this.ProductsIndexBase + (int) tp.TrajectoryIndexRelative;
      ImmutableArray<TransportWaypoint> waypoints = this.m_trajectory.Waypoints;
      if ((uint) index < (uint) waypoints.Length)
        return waypoints[index];
      Log.Error(string.Format("Invalid index '{0}' for transported product, waypoints count: {1}.", (object) index, (object) waypoints.Length));
      return index >= 0 ? waypoints.Last : waypoints.First;
    }

    public bool TryReverse(out string error)
    {
      TransportTrajectory reversedTrajectory;
      if (!this.Trajectory.TryReverse(out reversedTrajectory, out error))
        return false;
      int length1 = this.Trajectory.Waypoints.Length;
      this.resetTrajectory(reversedTrajectory);
      int length2 = this.Trajectory.Waypoints.Length;
      Assert.That<int>(length2).IsNear(length1, 2);
      int productsIndexBase = this.ProductsIndexBase;
      this.ProductsIndexBase = 0;
      for (int index = 0; index < this.m_products.Count; ++index)
      {
        ref TransportedProductMutable local = ref this.m_products.GetRefAt(index);
        int num = productsIndexBase + (int) local.TrajectoryIndexRelative;
        local.TrajectoryIndexRelative = (short) (length2 - num - 1).Max(0);
        local.LastSeenIndexAbsoluteForUi = (ushort) (length2 - (int) local.LastSeenIndexAbsoluteForUi - 1).Max(0);
        local.IsImmediatelyBehindNextProduct = false;
      }
      this.m_products.Reverse();
      ++this.ProductsStateVersion;
      this.m_allProductsAreImmediatelyBehindFirstProduct = false;
      return true;
    }

    public bool TryChangeDirection(Direction903d newDirection, bool atStart, out string error)
    {
      return !atStart ? this.TryChangeDirectionAtEnd(newDirection, out error) : this.TryChangeDirectionAtStart(newDirection, out error);
    }

    public bool TryChangeDirectionAtStart(Direction903d newDirection, out string error)
    {
      TransportTrajectory newTrajectory;
      if (!this.Trajectory.TryChangeStartDirection(newDirection.ToTileDirection(), out newTrajectory, out error))
        return false;
      int length1 = this.Trajectory.Waypoints.Length;
      this.resetTrajectory(newTrajectory);
      int length2 = this.Trajectory.Waypoints.Length;
      int num1 = length2 - length1;
      if (num1 == 0 || this.TransportedProducts.Count == 0)
        return true;
      int productsIndexBase = this.ProductsIndexBase;
      this.ProductsIndexBase = 0;
      if (num1 > 0)
      {
        for (int index = 0; index < this.m_products.Count; ++index)
        {
          ref TransportedProductMutable local = ref this.m_products.GetRefAt(index);
          int num2 = productsIndexBase + (int) local.TrajectoryIndexRelative + num1;
          local.TrajectoryIndexRelative = (short) num2;
          int num3 = (int) local.LastSeenIndexAbsoluteForUi + num1;
          Assert.That<int>(num3).IsLess(length2);
          local.LastSeenIndexAbsoluteForUi = (ushort) num3;
        }
      }
      else
      {
        for (int index = 0; index < this.m_products.Count; ++index)
        {
          ref TransportedProductMutable local = ref this.m_products.GetRefAt(index);
          int num4 = productsIndexBase + (int) local.TrajectoryIndexRelative + num1;
          if (num4 < 0)
          {
            this.m_productsManager.ProductDestroyed(local.SlimId, local.Quantity, DestroyReason.Cleared);
            this.m_products.RemoveAt(index);
            --index;
          }
          else
          {
            local.TrajectoryIndexRelative = (short) num4;
            int self = (int) local.LastSeenIndexAbsoluteForUi + num1;
            local.LastSeenIndexAbsoluteForUi = (ushort) self.Max(0);
          }
        }
      }
      ++this.ProductsStateVersion;
      return true;
    }

    public bool TryChangeDirectionAtEnd(Direction903d newDirection, out string error)
    {
      TransportTrajectory newTrajectory;
      if (!this.Trajectory.TryChangeEndDirection(newDirection.ToTileDirection(), out newTrajectory, out error))
        return false;
      int length1 = this.Trajectory.Waypoints.Length;
      this.resetTrajectory(newTrajectory);
      int length2 = this.Trajectory.Waypoints.Length;
      int num1 = length2 - length1;
      if (num1 == 0 || this.TransportedProducts.Count == 0)
        return true;
      if (num1 <= 0)
      {
        int num2 = length2 - 1;
        while (this.m_products.IsNotEmpty)
        {
          TransportedProductMutable transportedProductMutable = this.m_products.Peek();
          if (this.ProductsIndexBase + (int) transportedProductMutable.TrajectoryIndexRelative - num1 >= num2)
          {
            this.m_productsManager.ProductDestroyed(transportedProductMutable.SlimId, transportedProductMutable.Quantity, DestroyReason.Cleared);
            this.m_products.Dequeue();
          }
          else
            break;
        }
      }
      ++this.ProductsStateVersion;
      return true;
    }

    void ITransportFriend.InsertNewProduct(
      ProductSlimId product,
      Quantity quantity,
      int waypointIndex)
    {
      int trajectoryIndexRelative = waypointIndex - this.ProductsIndexBase;
      TransportedProductMutable transportedProductMutable = new TransportedProductMutable(product, (short) trajectoryIndexRelative, quantity, (ushort) waypointIndex, this.TransportManager.GetNextSeqNumber());
      ++this.ProductsStateVersion;
      this.m_allProductsAreImmediatelyBehindFirstProduct = false;
      if (this.m_products.IsEmpty || (int) this.m_products.Last.TrajectoryIndexRelative >= trajectoryIndexRelative)
      {
        this.m_products.Enqueue(transportedProductMutable);
        if (!this.Prototype.Graphics.UsePerProductColoring)
          return;
        this.updateTransportColor(transportedProductMutable.SlimId);
      }
      else
      {
        for (int index = 0; index < this.m_products.Count; ++index)
        {
          if ((int) this.m_products[index].TrajectoryIndexRelative <= trajectoryIndexRelative)
          {
            this.m_products.GetRefAt(index).IsImmediatelyBehindNextProduct = false;
            this.m_products.EnqueueAt(transportedProductMutable, index);
            if (!this.Prototype.Graphics.UsePerProductColoring)
              break;
            this.updateTransportColor(transportedProductMutable.SlimId);
            break;
          }
        }
      }
    }

    public override void NotifyUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      int newIndex,
      bool wasAdded,
      out bool canCollapse)
    {
      canCollapse = groundVerticesViolatingConstraints.Count >= 8 || groundVerticesViolatingConstraints.Count > this.OccupiedVertices.Length / 2;
    }

    public override bool TryCollapseOnUnevenTerrain(
      IReadOnlySet<int> groundVerticesViolatingConstraints,
      EntityCollapseHelper collapseHelper)
    {
      Lyst<KeyValuePair<Tile3i, int>> list = new Lyst<KeyValuePair<Tile3i, int>>();
      ImmutableArray<OccupiedTileRelative> occupiedTiles = this.OccupiedTiles;
      ImmutableArray<OccupiedVertexRelative> occupiedVertices = this.OccupiedVertices;
      ImmutableArray<TransportSupportableTile> supportableTiles = this.Trajectory.SupportableTiles;
      foreach (int violatingConstraint in (IEnumerable<int>) groundVerticesViolatingConstraints)
      {
        OccupiedVertexRelative occupiedVertexRelative = occupiedVertices[violatingConstraint];
        Tile3i coord = this.CenterTile + occupiedTiles[(int) occupiedVertexRelative.LowestTileIndex].RelCoord.ExtendHeight(occupiedVertexRelative.FromHeightRel);
        if (!list.ContainsKey<Tile3i, int>(coord))
        {
          int num = supportableTiles.MinIndex<long>((Func<TransportSupportableTile, long>) (x => x.Position.DistanceSqrTo(coord)));
          list.Add<Tile3i, int>(coord, num);
        }
      }
      if (list.IsEmpty)
      {
        Log.Error("Failed to collapse transport.");
        return false;
      }
      list.Sort((Comparison<KeyValuePair<Tile3i, int>>) ((x, y) => x.Value.CompareTo(y.Value)));
      int num1 = list[0].Value;
      int num2 = num1;
      int num3 = num1;
      int num4 = num1;
      for (int index = 1; index < list.Count; ++index)
      {
        int num5 = list[index].Value;
        if (num5 - num2 <= 2)
        {
          num2 = num5;
          if (index + 1 < list.Count)
            continue;
        }
        if (num2 - num1 > num4 - num3)
        {
          num4 = num2;
          num3 = num1;
        }
        num1 = num2 = num5;
      }
      int index1 = (num3 - 2).Max(0);
      int index2 = (num4 + 2).Min(supportableTiles.Length - 1);
      LocStrFormatted error;
      if (this.TransportManager.TryCollapseSubTransport(this, supportableTiles[index1].Position, supportableTiles[index2].Position, out Option<Transport> _, out Option<Transport> _, out error))
        return true;
      Log.Error(string.Format("Failed to collapse sub-transport: {0}", (object) error));
      return false;
    }

    public Tile3i GetClosestTransportPosition(Tile3i position)
    {
      RelTile2i other = position.Xy - this.CenterTile.Xy;
      int num1 = position.Z - this.CenterTile.Z;
      long num2 = long.MaxValue;
      int index1 = -1;
      int index2 = 0;
      ImmutableArray<OccupiedTileRelative> occupiedTiles;
      while (true)
      {
        int num3 = index2;
        occupiedTiles = this.OccupiedTiles;
        int length = occupiedTiles.Length;
        if (num3 < length)
        {
          occupiedTiles = this.OccupiedTiles;
          OccupiedTileRelative occupiedTileRelative = occupiedTiles[index2];
          if (this.Trajectory.OccupiedTilesMetadata[index2].HasValue)
          {
            long num4 = occupiedTileRelative.RelCoord.DistanceSqrTo(other);
            if (num4 < num2)
            {
              if (num1 < (int) occupiedTileRelative.RelativeFrom)
                num4 += (long) ((int) occupiedTileRelative.RelativeFrom - num1).Squared();
              else if (num1 >= occupiedTileRelative.ToHeightRelExcl.Value)
                num4 += (long) (num1 - occupiedTileRelative.ToHeightRelExcl.Value + 1).Squared();
              if (num4 < num2)
              {
                num2 = num4;
                index1 = index2;
                if (num4 == 0L)
                  break;
              }
            }
          }
          ++index2;
        }
        else
          goto label_12;
      }
      return position;
label_12:
      occupiedTiles = this.OccupiedTiles;
      OccupiedTileRelative occupiedTileRelative1 = occupiedTiles[index1];
      int z = num1 < (int) occupiedTileRelative1.RelativeFrom ? (int) occupiedTileRelative1.RelativeFrom : (num1 >= occupiedTileRelative1.ToHeightRelExcl.Value ? occupiedTileRelative1.ToHeightRelExcl.Value - 1 : num1);
      occupiedTiles = this.OccupiedTiles;
      return occupiedTiles[index1].RelCoord.ExtendZ(z) + this.CenterTile;
    }

    public override bool IsSelected(RectangleTerrainArea2i area) => this.Trajectory.IsInArea(area);

    public override ImmutableArray<ConstrCubeSpec> GetConstructionCubesSpec(out int totalCubesVolume)
    {
      ref int local = ref totalCubesVolume;
      ImmutableArray<OccupiedTileRelative> occupiedTiles = this.OccupiedTiles;
      int num = occupiedTiles.Sum((Func<OccupiedTileRelative, int>) (x => (int) x.VerticalSizeRaw));
      local = num;
      occupiedTiles = this.OccupiedTiles;
      return occupiedTiles.Map<ConstrCubeSpec, Tile3i>(this.CenterTile, (Func<OccupiedTileRelative, Tile3i, ConstrCubeSpec>) ((x, c) => new ConstrCubeSpec((c.Xy + x.RelCoord).AsSlim, (c.Height + x.FromHeightRel).AsSlim, (byte) 1, (byte) 1, (byte) x.VerticalSizeRaw, (byte) x.VerticalSizeRaw.Min((ushort) byte.MaxValue))));
    }

    private void updateTransportColor(ProductSlimId newProduct)
    {
      ProductProto.Gfx graphics = newProduct.ToFullOrPhantom(this.m_productsManager.SlimIdManager).Graphics;
      ColorRgba transportColor = graphics.TransportColor;
      this.m_transportColor = new ColorRgba(this.m_transportColor.R.LerpStepAtLeastOne(transportColor.R, Transport.PIPE_COLOR_CHANGE_PER_PRODUCT), this.m_transportColor.G.LerpStepAtLeastOne(transportColor.G, Transport.PIPE_COLOR_CHANGE_PER_PRODUCT), this.m_transportColor.B.LerpStepAtLeastOne(transportColor.B, Transport.PIPE_COLOR_CHANGE_PER_PRODUCT), byte.MaxValue);
      ColorRgba transportAccentColor1 = graphics.TransportAccentColor;
      ColorRgba transportAccentColor2 = this.TransportAccentColor;
      int r = (int) transportAccentColor2.R.LerpStepAtLeastOne(transportAccentColor1.R, Transport.PIPE_COLOR_CHANGE_PER_PRODUCT);
      transportAccentColor2 = this.TransportAccentColor;
      int g = (int) transportAccentColor2.G.LerpStepAtLeastOne(transportAccentColor1.G, Transport.PIPE_COLOR_CHANGE_PER_PRODUCT);
      transportAccentColor2 = this.TransportAccentColor;
      int b = (int) transportAccentColor2.B.LerpStepAtLeastOne(transportAccentColor1.B, Transport.PIPE_COLOR_CHANGE_PER_PRODUCT);
      this.TransportAccentColor = new ColorRgba((byte) r, (byte) g, (byte) b, byte.MaxValue);
    }

    public void AddToConfig(EntityConfigData data)
    {
      data.SetTransportColor(this.m_transportColor, this.TransportAccentColor);
    }

    public void ApplyConfig(EntityConfigData data)
    {
      ColorRgba? color;
      ColorRgba? accentColor;
      data.GetTransportColor(out color, out accentColor);
      if (color.HasValue)
        this.m_transportColor = color.Value;
      if (!accentColor.HasValue)
        return;
      this.TransportAccentColor = accentColor.Value;
    }

    public static void Serialize(Transport value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<Transport>(value))
        return;
      writer.EnqueueDataSerialization((object) value, Transport.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.GeneralPriority);
      writer.WriteBool(this.IsFullyConnected);
      writer.WriteBool(this.IsMoving);
      ProductSlimId.Serialize(this.LastInsertedProduct, writer);
      writer.WriteBool(this.m_allProductsAreImmediatelyBehindFirstProduct);
      Option<Lyst<Transport.TransportClearingBuffer>>.Serialize(this.m_clearingBuffers, writer);
      AssetValue.Serialize(this.m_constructionCost, writer);
      Option<IElectricityConsumer>.Serialize(this.m_electricityConsumer, writer);
      IoPort.Serialize(this.m_endOutputPort, writer);
      ImmutableArray<IoPort>.Serialize(this.m_ports, writer);
      writer.WriteBool(this.m_powerLow);
      Queueue<TransportedProductMutable>.Serialize(this.m_products, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      writer.WriteGeneric<ISimLoopEvents>(this.m_simLoopEvents);
      IoPort.Serialize(this.m_startInputPort, writer);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      EntityNotificator.Serialize(this.m_tooLongTransportNotif, writer);
      TransportTrajectory.Serialize(this.m_trajectory, writer);
      ColorRgba.Serialize(this.m_transportColor, writer);
      AssetValue.Serialize(this.m_value, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      writer.WriteGeneric<IEntityMaintenanceProvider>(this.Maintenance);
      writer.WriteInt(this.MovedStepsTotal);
      Electricity.Serialize(this.PowerRequired, writer);
      writer.WriteInt(this.ProductsIndexBase);
      writer.WriteInt(this.ProductsStateVersion);
      ColorRgba.Serialize(this.TransportAccentColor, writer);
      TransportsManager.Serialize(this.TransportManager, writer);
      writer.WriteGeneric<IUpgrader>(this.Upgrader);
    }

    public static Transport Deserialize(BlobReader reader)
    {
      Transport transport;
      if (reader.TryStartClassDeserialization<Transport>(out transport))
        reader.EnqueueDataDeserialization((object) transport, Transport.s_deserializeDataDelayedAction);
      return transport;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.GeneralPriority = reader.ReadInt();
      this.IsFullyConnected = reader.ReadBool();
      this.IsMoving = reader.ReadBool();
      this.LastInsertedProduct = reader.LoadedSaveVersion >= 140 ? ProductSlimId.Deserialize(reader) : new ProductSlimId();
      this.m_allProductsAreImmediatelyBehindFirstProduct = reader.ReadBool();
      this.m_clearingBuffers = Option<Lyst<Transport.TransportClearingBuffer>>.Deserialize(reader);
      this.m_constructionCost = AssetValue.Deserialize(reader);
      this.m_electricityConsumer = Option<IElectricityConsumer>.Deserialize(reader);
      this.m_endOutputPort = IoPort.Deserialize(reader);
      this.m_ports = ImmutableArray<IoPort>.Deserialize(reader);
      this.m_powerLow = reader.ReadBool();
      reader.SetField<Transport>(this, "m_products", (object) Queueue<TransportedProductMutable>.Deserialize(reader));
      reader.SetField<Transport>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.SetField<Transport>(this, "m_simLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.m_startInputPort = IoPort.Deserialize(reader);
      reader.SetField<Transport>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      this.m_tooLongTransportNotif = reader.LoadedSaveVersion >= 101 ? EntityNotificator.Deserialize(reader) : new EntityNotificator();
      if (reader.LoadedSaveVersion < 101)
        this.m_tooLongTransportNotificator = reader.ReadNullableStruct<EntityNotificator>();
      this.m_trajectory = TransportTrajectory.Deserialize(reader);
      this.m_transportColor = reader.LoadedSaveVersion >= 140 ? ColorRgba.Deserialize(reader) : ColorRgba.Gray;
      this.m_value = AssetValue.Deserialize(reader);
      reader.SetField<Transport>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      this.Maintenance = reader.ReadGenericAs<IEntityMaintenanceProvider>();
      this.MovedStepsTotal = reader.ReadInt();
      this.PowerRequired = Electricity.Deserialize(reader);
      this.ProductsIndexBase = reader.ReadInt();
      this.ProductsStateVersion = reader.ReadInt();
      this.TransportAccentColor = reader.LoadedSaveVersion >= 140 ? ColorRgba.Deserialize(reader) : new ColorRgba(3355443);
      this.TransportManager = TransportsManager.Deserialize(reader);
      this.Upgrader = reader.ReadGenericAs<IUpgrader>();
      reader.RegisterInitAfterLoad<Transport>(this, "initAfterLoad", InitPriority.Normal);
    }

    static Transport()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      Transport.PIPE_COLOR_CHANGE_PER_PRODUCT = Percent.FromPercentVal(4);
      Transport.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      Transport.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }

    /// <summary>The current state of the transport.</summary>
    public enum Status
    {
      /// <summary>
      /// The transport is connected properly but is not moving (e.g. full consumer or the consumer does not accept
      /// the product we have).
      /// </summary>
      Idle,
      /// <summary>The transport is not fully connected.</summary>
      NotConnected,
      /// <summary>The transport is currently moving products forward.</summary>
      Moving,
      /// <summary>The transport is disabled.</summary>
      Paused,
      /// <summary>The transport does not have enough electricity.</summary>
      PowerLow,
    }

    [GenerateSerializer(false, null, 0)]
    private class TransportClearingBuffer : IProductBuffer, IProductBufferReadOnly
    {
      private readonly Transport m_transport;
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public ProductProto Product { get; private set; }

      public Quantity UsableCapacity => Quantity.Zero;

      public Quantity Capacity => this.Quantity;

      public Quantity Quantity
      {
        get
        {
          Quantity zero = Quantity.Zero;
          ProductSlimId slimId = this.Product.SlimId;
          foreach (TransportedProductMutable transportedProduct in this.m_transport.TransportedProducts)
          {
            if (transportedProduct.SlimId == slimId)
              zero += transportedProduct.Quantity;
          }
          return zero;
        }
      }

      public TransportClearingBuffer(Transport transport, ProductProto product)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_transport = transport;
        this.Product = product;
        StaticPriorityProvider priorityProvider = new StaticPriorityProvider(BufferStrategy.FullFillAtAnyCost(7));
        transport.m_vehicleBuffersRegistry.RegisterOutputBufferAndAssert((IStaticEntity) transport, (IProductBuffer) this, (IOutputBufferPriorityProvider) priorityProvider, true, true, true);
      }

      public Quantity StoreAsMuchAs(Quantity quantity)
      {
        Assert.Fail("Unsupported operation");
        return quantity;
      }

      public void Destroy()
      {
        this.m_transport.m_vehicleBuffersRegistry.UnregisterOutputBufferAndAssert((IProductBuffer) this);
      }

      public Quantity RemoveAsMuchAs(Quantity maxQuantity)
      {
        Quantity removed;
        bool flag = this.m_transport.removeProductOfType(this.Product, maxQuantity, out removed);
        Assert.That<Quantity>(removed).IsLessOrEqual(maxQuantity);
        if (flag)
        {
          this.m_transport.m_vehicleBuffersRegistry.UnregisterOutputBufferAndAssert((IProductBuffer) this);
          if (this.m_transport.m_clearingBuffers.HasValue)
            this.m_transport.m_clearingBuffers.Value.Remove(this);
        }
        return removed;
      }

      public static void Serialize(Transport.TransportClearingBuffer value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<Transport.TransportClearingBuffer>(value))
          return;
        writer.EnqueueDataSerialization((object) value, Transport.TransportClearingBuffer.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Transport.Serialize(this.m_transport, writer);
        writer.WriteGeneric<ProductProto>(this.Product);
      }

      public static Transport.TransportClearingBuffer Deserialize(BlobReader reader)
      {
        Transport.TransportClearingBuffer transportClearingBuffer;
        if (reader.TryStartClassDeserialization<Transport.TransportClearingBuffer>(out transportClearingBuffer))
          reader.EnqueueDataDeserialization((object) transportClearingBuffer, Transport.TransportClearingBuffer.s_deserializeDataDelayedAction);
        return transportClearingBuffer;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<Transport.TransportClearingBuffer>(this, "m_transport", (object) Transport.Deserialize(reader));
        this.Product = reader.ReadGenericAs<ProductProto>();
      }

      static TransportClearingBuffer()
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        Transport.TransportClearingBuffer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Transport.TransportClearingBuffer) obj).SerializeData(writer));
        Transport.TransportClearingBuffer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Transport.TransportClearingBuffer) obj).DeserializeData(reader));
      }
    }
  }
}
