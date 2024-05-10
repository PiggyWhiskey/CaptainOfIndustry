// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.SpaceProgram.RocketLaunchPad
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Population;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Core.SpaceProgram;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.RocketTransporters;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.SpaceProgram
{
  [GenerateSerializer(false, null, 0)]
  public class RocketLaunchPad : 
    LayoutEntity,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntity,
    IIsSafeAsHashKey,
    IRocketOwner,
    IRenderedEntity,
    IStaticEntityWithQueue,
    IEntityWithSimUpdate,
    IEntityWithPorts,
    IStaticEntity,
    IEntityWithPosition,
    IAreaSelectableEntity
  {
    public readonly RocketLaunchPadProto Prototype;
    private Duration m_remainingStateDuration;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly VehicleQueue<Vehicle, IStaticEntity> m_vehicleQueue;
    private Option<RocketTransporter> m_deliveryTransporter;
    private ProductBuffer m_waterBuffer;
    private Duration m_sprinklingDurationRemaining;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => true;

    public RocketLaunchPadState State { get; private set; }

    public Duration RemainingStateDuration => this.m_remainingStateDuration;

    int IEntityWithWorkers.WorkersNeeded => this.Prototype.Costs.Workers;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public Option<RocketEntityBase> AttachedRocketBase
    {
      get => (Option<RocketEntityBase>) (RocketEntityBase) this.AttachedRocket.ValueOrNull;
    }

    public Option<RocketEntity> AttachedRocket { get; private set; }

    public Tile2f RocketAnchor { get; private set; }

    public Tile2f RocketTransporterNavGoal { get; private set; }

    public Tile2f RocketTransporterAlignGoal { get; private set; }

    public Tile2f RocketTransporterExitGoal { get; private set; }

    public int IncomingRocketsQueueLength => this.m_vehicleQueue.TrucksCount;

    public bool AutoLaunch { get; private set; }

    public Duration? LaunchCountdown
    {
      get
      {
        return this.State != RocketLaunchPadState.LaunchCountdown ? new Duration?() : new Duration?(this.m_remainingStateDuration);
      }
    }

    public IProductBufferReadOnly WaterBuffer => (IProductBufferReadOnly) this.m_waterBuffer;

    public bool IsSprinklingWater { get; private set; }

    public bool IsCrawlerBridgeErected { get; private set; }

    public RocketLaunchPad(
      EntityId id,
      RocketLaunchPadProto proto,
      TileTransform transform,
      EntityContext context,
      IEntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (LayoutEntityProto) proto, transform, context);
      this.Prototype = proto;
      this.m_entitiesManager = entitiesManager;
      this.updateCachedTiles();
      this.m_vehicleQueue = new VehicleQueue<Vehicle, IStaticEntity>((IStaticEntity) this);
      this.m_vehicleQueue.Enable();
      this.m_waterBuffer = new ProductBuffer(proto.WaterPerLaunch.Quantity, proto.WaterPerLaunch.Product);
    }

    [InitAfterLoad(InitPriority.ImmediatelyAfterSelfDeserialized)]
    private void initSelf()
    {
      if (this.m_waterBuffer != null)
        return;
      this.m_waterBuffer = new ProductBuffer(this.Prototype.WaterPerLaunch.Quantity, this.Prototype.WaterPerLaunch.Product);
    }

    public override bool GetCustomPfTargetTiles(int retryNumber, Lyst<Tile2i> outTiles)
    {
      outTiles.Add(this.RocketTransporterNavGoal.Tile2i);
      return false;
    }

    protected override void SetTransform(TileTransform transform)
    {
      base.SetTransform(transform);
      this.updateCachedTiles();
    }

    private void updateCachedTiles()
    {
      this.RocketAnchor = this.Prototype.Layout.GetModelOrigin(this.Transform).Xy;
      RelTile2f direction = new RelTile2f(this.Prototype.RocketTransporterNavGoalDeltaX.RelTile1f, RelTile1f.Zero);
      this.RocketTransporterNavGoal = this.RocketAnchor + this.Prototype.Layout.TransformDirection(direction.AddX((Fix32) -1).AddY((Fix32) this.Prototype.RocketTransporterArriveDeltaY.Value), this.Transform);
      this.RocketTransporterAlignGoal = this.RocketAnchor + this.Prototype.Layout.TransformDirection(direction, this.Transform);
      this.RocketTransporterExitGoal = this.RocketAnchor + this.Prototype.Layout.TransformDirection(direction.AddX((Fix32) -1).AddY((Fix32) this.Prototype.RocketTransporterExitDeltaY.Value), this.Transform);
    }

    public void SetAutoLaunch(bool autoLaunch) => this.AutoLaunch = autoLaunch;

    public void SetCrawlerBridgeErected(bool isErected) => this.IsCrawlerBridgeErected = isErected;

    void IEntityWithSimUpdate.SimUpdate()
    {
      if (this.IsNotEnabled)
        return;
      Entity.HasWorkers((IEntityWithWorkers) this);
      switch (this.State)
      {
        case RocketLaunchPadState.WaitingForRocket:
          Assert.That<Option<RocketEntity>>(this.AttachedRocket).IsNone<RocketEntity>();
          break;
        case RocketLaunchPadState.AttachingRocket:
          Assert.That<Option<RocketEntity>>(this.AttachedRocket).HasValue<RocketEntity>();
          Assert.That<Duration>(this.m_remainingStateDuration).IsPositive();
          this.m_remainingStateDuration -= Duration.OneTick;
          if (!this.m_remainingStateDuration.IsNotPositive)
            break;
          this.State = RocketLaunchPadState.RocketAttached;
          break;
        case RocketLaunchPadState.RocketAttached:
          Assert.That<Option<RocketEntity>>(this.AttachedRocket).HasValue<RocketEntity>();
          if (!this.AutoLaunch || !this.CanStartLaunchCountdown())
            break;
          this.TryStartLaunchCountdown();
          break;
        case RocketLaunchPadState.LaunchCountdown:
          Assert.That<Option<RocketEntity>>(this.AttachedRocket).HasValue<RocketEntity>();
          Assert.That<Duration>(this.m_remainingStateDuration).IsPositive();
          this.m_remainingStateDuration -= Duration.OneTick;
          if (this.m_remainingStateDuration == Duration.OneSecond)
            this.m_sprinklingDurationRemaining = this.Prototype.WaterSprinklingDuration;
          this.handleWaterSprinkling();
          if (!this.m_remainingStateDuration.IsNotPositive)
            break;
          this.State = this.launchRocket();
          break;
        case RocketLaunchPadState.RocketLaunching:
          Assert.That<Option<RocketEntity>>(this.AttachedRocket).IsNone<RocketEntity>();
          Assert.That<Duration>(this.m_remainingStateDuration).IsPositive();
          this.m_remainingStateDuration -= Duration.OneTick;
          this.handleWaterSprinkling();
          if (!this.m_remainingStateDuration.IsNotPositive)
            break;
          this.State = RocketLaunchPadState.WaitingForRocket;
          break;
        default:
          Log.Error(string.Format("Invalid state '{0}'", (object) this.State));
          this.State = RocketLaunchPadState.WaitingForRocket;
          break;
      }
    }

    private void handleWaterSprinkling()
    {
      if (this.m_sprinklingDurationRemaining.IsNotPositive)
        return;
      this.m_sprinklingDurationRemaining -= Duration.OneTick;
      this.m_waterBuffer.RemoveExactly(this.Prototype.WaterPerTick);
      this.IsSprinklingWater = this.m_sprinklingDurationRemaining.IsPositive;
    }

    public bool CanStartLaunchCountdown()
    {
      return this.State == RocketLaunchPadState.RocketAttached && this.IsEnabled && this.AttachedRocket.HasValue && this.AttachedRocket.Value.CanLaunch() && this.WaterBuffer.IsFull();
    }

    public bool TryStartLaunchCountdown()
    {
      if (!this.CanStartLaunchCountdown())
        return false;
      this.State = RocketLaunchPadState.LaunchCountdown;
      this.m_remainingStateDuration = this.Prototype.RocketCountdownDuration;
      return true;
    }

    private bool canAcceptNewRocket()
    {
      return this.State == RocketLaunchPadState.WaitingForRocket && this.AttachedRocket.IsNone && this.IsEnabled;
    }

    public bool TryAcceptNewRocketFrom(RocketTransporter transporter)
    {
      if (!this.canAcceptNewRocket() || this.m_deliveryTransporter.HasValue)
        return false;
      this.m_deliveryTransporter = (Option<RocketTransporter>) transporter;
      return true;
    }

    public bool CanAttachRocket(RocketEntityBase rocketBase)
    {
      if (rocketBase is RocketEntity)
        return this.canAcceptNewRocket();
      Log.Error("Attaching invalid rocket type '" + rocketBase.GetType().Name + "'.");
      return false;
    }

    void IRocketOwner.AttachRocket(RocketEntityBase rocketBase)
    {
      Assert.That<Option<IRocketOwner>>(rocketBase.Owner).IsNone<IRocketOwner>("Rocket already owned by someone. Forgot to call `SetOwner`?");
      if (!this.CanAttachRocket(rocketBase))
        Log.Error("Cannot attach rocket '" + rocketBase.GetType().Name + "'.");
      else if (rocketBase is RocketEntity rocketEntity)
      {
        this.AttachedRocket = (Option<RocketEntity>) rocketEntity;
        rocketEntity.SetOwner((Option<IRocketOwner>) (IRocketOwner) this);
        this.m_remainingStateDuration = this.Prototype.RocketAttachDuration;
        this.State = RocketLaunchPadState.AttachingRocket;
        this.m_deliveryTransporter = Option<RocketTransporter>.None;
      }
      else
        Log.Error("Attaching invalid rocket type '" + rocketBase.GetType().Name + "'.");
    }

    Option<RocketEntityBase> IRocketOwner.DetachRocket()
    {
      if (this.AttachedRocket.IsNone)
        return Option<RocketEntityBase>.None;
      RocketEntity rocketEntity = this.AttachedRocket.Value;
      rocketEntity.SetOwner(Option<IRocketOwner>.None);
      this.AttachedRocket = Option<RocketEntity>.None;
      return (Option<RocketEntityBase>) (RocketEntityBase) rocketEntity;
    }

    private RocketLaunchPadState launchRocket()
    {
      RocketEntity rocketEntity = this.AttachedRocket.Value;
      rocketEntity.TryDetachAndLaunch(this.Prototype.Layout.GetModelOrigin(this.Transform).AddZ(rocketEntity.Prototype.GroundOffset.Value)).AssertTrue("Failed to launch the rocket.");
      this.AttachedRocket = Option<RocketEntity>.None;
      this.m_remainingStateDuration = this.Prototype.RocketLaunchDuration;
      return RocketLaunchPadState.RocketLaunching;
    }

    public bool TryGetVehicleQueueFor(
      Vehicle vehicle,
      out VehicleQueue<Vehicle, IStaticEntity> queue)
    {
      if (this.IsNotEnabled || !(vehicle is RocketTransporter))
      {
        queue = (VehicleQueue<Vehicle, IStaticEntity>) null;
        return false;
      }
      queue = this.m_vehicleQueue;
      return true;
    }

    public override EntityValidationResult CanStartDeconstruction()
    {
      if (this.AttachedRocket.HasValue)
        return EntityValidationResult.CreateError("Has a rocket");
      return this.m_deliveryTransporter.HasValue ? EntityValidationResult.CreateError("Rocket transporter needs to return first") : EntityValidationResult.Success;
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
      Assert.That<Option<RocketEntity>>(this.AttachedRocket).IsNone<RocketEntity>();
      base.OnDestroy();
      this.m_vehicleQueue.CancelJobsAndDisable();
      if (!this.AttachedRocketBase.HasValue)
        return;
      ((IEntityFriend) this.AttachedRocketBase.Value).Destroy();
    }

    Quantity IEntityWithPorts.ReceiveAsMuchAsFromPort(ProductQuantity pq, IoPortToken sourcePort)
    {
      if (this.IsNotEnabled)
        return pq.Quantity;
      return this.Prototype.WaterPortNames.Contains(sourcePort.Name) ? (!((Proto) pq.Product == (Proto) this.m_waterBuffer.Product) ? pq.Quantity : this.m_waterBuffer.StoreAsMuchAs(pq.Quantity)) : (this.AttachedRocket.IsNone || this.State != RocketLaunchPadState.RocketAttached ? pq.Quantity : this.AttachedRocket.Value.LoadAsMuchAs(pq));
    }

    public static void Serialize(RocketLaunchPad value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RocketLaunchPad>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RocketLaunchPad.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Option<RocketEntity>.Serialize(this.AttachedRocket, writer);
      writer.WriteBool(this.AutoLaunch);
      writer.WriteBool(this.IsCrawlerBridgeErected);
      writer.WriteBool(this.IsSprinklingWater);
      Option<RocketTransporter>.Serialize(this.m_deliveryTransporter, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      Duration.Serialize(this.m_remainingStateDuration, writer);
      Duration.Serialize(this.m_sprinklingDurationRemaining, writer);
      VehicleQueue<Vehicle, IStaticEntity>.Serialize(this.m_vehicleQueue, writer);
      ProductBuffer.Serialize(this.m_waterBuffer, writer);
      writer.WriteGeneric<RocketLaunchPadProto>(this.Prototype);
      Tile2f.Serialize(this.RocketAnchor, writer);
      Tile2f.Serialize(this.RocketTransporterAlignGoal, writer);
      Tile2f.Serialize(this.RocketTransporterExitGoal, writer);
      Tile2f.Serialize(this.RocketTransporterNavGoal, writer);
      writer.WriteInt((int) this.State);
    }

    public static RocketLaunchPad Deserialize(BlobReader reader)
    {
      RocketLaunchPad rocketLaunchPad;
      if (reader.TryStartClassDeserialization<RocketLaunchPad>(out rocketLaunchPad))
        reader.EnqueueDataDeserialization((object) rocketLaunchPad, RocketLaunchPad.s_deserializeDataDelayedAction);
      return rocketLaunchPad;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.AttachedRocket = Option<RocketEntity>.Deserialize(reader);
      this.AutoLaunch = reader.ReadBool();
      this.IsCrawlerBridgeErected = reader.ReadBool();
      this.IsSprinklingWater = reader.ReadBool();
      this.m_deliveryTransporter = Option<RocketTransporter>.Deserialize(reader);
      reader.SetField<RocketLaunchPad>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      this.m_remainingStateDuration = Duration.Deserialize(reader);
      this.m_sprinklingDurationRemaining = Duration.Deserialize(reader);
      reader.SetField<RocketLaunchPad>(this, "m_vehicleQueue", (object) VehicleQueue<Vehicle, IStaticEntity>.Deserialize(reader));
      this.m_waterBuffer = ProductBuffer.Deserialize(reader);
      reader.SetField<RocketLaunchPad>(this, "Prototype", (object) reader.ReadGenericAs<RocketLaunchPadProto>());
      this.RocketAnchor = Tile2f.Deserialize(reader);
      this.RocketTransporterAlignGoal = Tile2f.Deserialize(reader);
      this.RocketTransporterExitGoal = Tile2f.Deserialize(reader);
      this.RocketTransporterNavGoal = Tile2f.Deserialize(reader);
      this.State = (RocketLaunchPadState) reader.ReadInt();
      this.initSelf();
    }

    static RocketLaunchPad()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RocketLaunchPad.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      RocketLaunchPad.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
