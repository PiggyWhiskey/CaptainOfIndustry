// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.TravelingFleet
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Shipyard;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Fleet;
using Mafi.Core.MessageNotifications;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Stats;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.World.Entities;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.World
{
  /// <summary>
  /// A wrapper around <see cref="T:Mafi.Core.Fleet.BattleFleet" /> that adds capability to move on the world map.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class TravelingFleet : 
    Entity,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IEntityWithSimUpdate,
    IEntityWithWorkers,
    IEntityWithGeneralPriority,
    IEntityWithCustomTitle,
    IEntityWithGeneralPriorityFriend
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly Duration EXPLORATION_DURATION;
    public const int EXPLORATION_COST_IN_KM = 550;
    public readonly BattleFleet BattleFleet;
    public readonly FleetEntity FleetEntity;
    public readonly TravelingFleetProto Prototype;
    [DoNotSaveCreateNewOnLoad("new Lyst<WorldMapLocId>(canOmitClearing: true)", 0)]
    private readonly Lyst<WorldMapLocId> m_pathToGoalTmp;
    private readonly IWorldMapManager m_mapManager;
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly IProductsManager m_productsManager;
    private readonly IBattleSimulator m_battleSimulator;
    private readonly IWorldMapPathFinder m_pathFinder;
    private readonly SettlementsManager m_settlementsManager;
    private readonly INotificationsManager m_notificationsManager;
    private readonly IFuelStatsCollector m_fuelStatsCollector;
    private readonly IMessageNotificationsManager m_messageNotificationsManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly TravelingFleetManager m_fleetManager;
    private LocationVisitReason m_lastVisitReason;
    private readonly Event<WorldMapLocation, Option<WorldMapLoot>> m_onLocationFullyExplored;
    private readonly Queueue<WorldMapLocId> m_path;
    private Fix32 m_totalPathDistance;
    private Fix32 m_distanceOnPathTraveled;
    private readonly Lyst<WorldMapLocId> m_fleetStartLocTmp;
    private readonly Dict<ProductProto, IProductBuffer> m_cargo;
    private bool m_didFindNextValidLocation;
    private Fix32 m_fuelDistanceTraveled;
    private readonly ProductBuffer m_fuelBuffer;
    private Option<IBattleState> m_battleState;
    private readonly TickTimer m_explorationTimer;
    private int m_explorationFuelTicksCounter;
    private int m_explorationTicksPerFuelQuantity;
    private int m_battlesEncountered;

    public static void Serialize(TravelingFleet value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TravelingFleet>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TravelingFleet.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      BattleFleet.Serialize(this.BattleFleet, writer);
      writer.WriteNullableStruct<WorldMapLocId>(this.CurrentLocationId);
      Option<string>.Serialize(this.CustomTitle, writer);
      AngleDegrees1f.Serialize(this.Direction, writer);
      Mafi.Core.Buildings.Shipyard.Shipyard.Serialize(this.Dock, writer);
      FleetEntity.Serialize(this.FleetEntity, writer);
      writer.WriteInt(this.GeneralPriority);
      writer.WriteBool(this.IsAutoReturnEnabled);
      writer.WriteBool(this.IsIdle);
      writer.WriteInt((int) this.LocationState);
      writer.WriteInt(this.m_battlesEncountered);
      writer.WriteGeneric<IBattleSimulator>(this.m_battleSimulator);
      Option<IBattleState>.Serialize(this.m_battleState, writer);
      Dict<ProductProto, IProductBuffer>.Serialize(this.m_cargo, writer);
      writer.WriteBool(this.m_didFindNextValidLocation);
      Fix32.Serialize(this.m_distanceOnPathTraveled, writer);
      writer.WriteGeneric<IEntitiesManager>(this.m_entitiesManager);
      writer.WriteInt(this.m_explorationFuelTicksCounter);
      writer.WriteInt(this.m_explorationTicksPerFuelQuantity);
      TickTimer.Serialize(this.m_explorationTimer, writer);
      TravelingFleetManager.Serialize(this.m_fleetManager, writer);
      Lyst<WorldMapLocId>.Serialize(this.m_fleetStartLocTmp, writer);
      ProductBuffer.Serialize(this.m_fuelBuffer, writer);
      Fix32.Serialize(this.m_fuelDistanceTraveled, writer);
      writer.WriteGeneric<IFuelStatsCollector>(this.m_fuelStatsCollector);
      writer.WriteInt((int) this.m_lastVisitReason);
      writer.WriteGeneric<IWorldMapManager>(this.m_mapManager);
      writer.WriteGeneric<IMessageNotificationsManager>(this.m_messageNotificationsManager);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      Event<WorldMapLocation, Option<WorldMapLoot>>.Serialize(this.m_onLocationFullyExplored, writer);
      Queueue<WorldMapLocId>.Serialize(this.m_path, writer);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      Fix32.Serialize(this.m_totalPathDistance, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
      WorldMapLocId.Serialize(this.NextLocationId, writer);
      Option<Mafi.Core.Buildings.Shipyard.Shipyard>.Serialize(this.PendingDockAssignment, writer);
      WorldMapLocId.Serialize(this.PreviousLocationId, writer);
      writer.WriteGeneric<TravelingFleetProto>(this.Prototype);
      writer.WriteInt(this.RefugeesCount);
      Duration.Serialize(this.RemainingTransitionDuration, writer);
      Vector2f.Serialize(this.WorldPosition, writer);
    }

    public static TravelingFleet Deserialize(BlobReader reader)
    {
      TravelingFleet travelingFleet;
      if (reader.TryStartClassDeserialization<TravelingFleet>(out travelingFleet))
        reader.EnqueueDataDeserialization((object) travelingFleet, TravelingFleet.s_deserializeDataDelayedAction);
      return travelingFleet;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<TravelingFleet>(this, "BattleFleet", (object) BattleFleet.Deserialize(reader));
      this.CurrentLocationId = reader.ReadNullableStruct<WorldMapLocId>();
      this.CustomTitle = Option<string>.Deserialize(reader);
      this.Direction = AngleDegrees1f.Deserialize(reader);
      this.Dock = Mafi.Core.Buildings.Shipyard.Shipyard.Deserialize(reader);
      reader.SetField<TravelingFleet>(this, "FleetEntity", (object) FleetEntity.Deserialize(reader));
      this.GeneralPriority = reader.ReadInt();
      this.IsAutoReturnEnabled = reader.ReadBool();
      this.IsIdle = reader.ReadBool();
      this.LocationState = (FleetLocationState) reader.ReadInt();
      this.m_battlesEncountered = reader.ReadInt();
      reader.SetField<TravelingFleet>(this, "m_battleSimulator", (object) reader.ReadGenericAs<IBattleSimulator>());
      this.m_battleState = Option<IBattleState>.Deserialize(reader);
      reader.SetField<TravelingFleet>(this, "m_cargo", (object) Dict<ProductProto, IProductBuffer>.Deserialize(reader));
      this.m_didFindNextValidLocation = reader.ReadBool();
      this.m_distanceOnPathTraveled = Fix32.Deserialize(reader);
      reader.SetField<TravelingFleet>(this, "m_entitiesManager", (object) reader.ReadGenericAs<IEntitiesManager>());
      this.m_explorationFuelTicksCounter = reader.ReadInt();
      this.m_explorationTicksPerFuelQuantity = reader.ReadInt();
      reader.SetField<TravelingFleet>(this, "m_explorationTimer", (object) TickTimer.Deserialize(reader));
      reader.SetField<TravelingFleet>(this, "m_fleetManager", (object) TravelingFleetManager.Deserialize(reader));
      reader.SetField<TravelingFleet>(this, "m_fleetStartLocTmp", (object) Lyst<WorldMapLocId>.Deserialize(reader));
      reader.SetField<TravelingFleet>(this, "m_fuelBuffer", (object) ProductBuffer.Deserialize(reader));
      this.m_fuelDistanceTraveled = Fix32.Deserialize(reader);
      reader.SetField<TravelingFleet>(this, "m_fuelStatsCollector", (object) reader.ReadGenericAs<IFuelStatsCollector>());
      this.m_lastVisitReason = (LocationVisitReason) reader.ReadInt();
      reader.SetField<TravelingFleet>(this, "m_mapManager", (object) reader.ReadGenericAs<IWorldMapManager>());
      reader.SetField<TravelingFleet>(this, "m_messageNotificationsManager", (object) reader.ReadGenericAs<IMessageNotificationsManager>());
      reader.SetField<TravelingFleet>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.SetField<TravelingFleet>(this, "m_onLocationFullyExplored", (object) Event<WorldMapLocation, Option<WorldMapLoot>>.Deserialize(reader));
      reader.SetField<TravelingFleet>(this, "m_path", (object) Queueue<WorldMapLocId>.Deserialize(reader));
      reader.RegisterResolvedMember<TravelingFleet>(this, "m_pathFinder", typeof (IWorldMapPathFinder), true);
      reader.SetField<TravelingFleet>(this, "m_pathToGoalTmp", (object) new Lyst<WorldMapLocId>(true));
      reader.SetField<TravelingFleet>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.RegisterResolvedMember<TravelingFleet>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<TravelingFleet>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      this.m_totalPathDistance = Fix32.Deserialize(reader);
      reader.SetField<TravelingFleet>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.SetField<TravelingFleet>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
      this.NextLocationId = WorldMapLocId.Deserialize(reader);
      this.PendingDockAssignment = Option<Mafi.Core.Buildings.Shipyard.Shipyard>.Deserialize(reader);
      this.PreviousLocationId = WorldMapLocId.Deserialize(reader);
      reader.SetField<TravelingFleet>(this, "Prototype", (object) reader.ReadGenericAs<TravelingFleetProto>());
      this.RefugeesCount = reader.ReadInt();
      this.RemainingTransitionDuration = Duration.Deserialize(reader);
      this.WorldPosition = Vector2f.Deserialize(reader);
    }

    public override bool CanBePaused => false;

    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    public int WorkersNeeded => this.FleetEntity.MinCrewNeeded;

    /// <summary>
    /// Called when location was explored so that loot was given or any hidden entity revealed.
    /// This happens after explore unless the player loses battle in which case this can get delayed.
    /// </summary>
    public IEvent<WorldMapLocation, Option<WorldMapLoot>> OnLocationFullyExplored
    {
      get => (IEvent<WorldMapLocation, Option<WorldMapLoot>>) this.m_onLocationFullyExplored;
    }

    public event Action<IBattleState, bool> OnBattleStarted;

    public event Action<IBattleState> OnBattleFinished;

    public event Action<TravelingFleet> OnModificationsDone;

    public WorldMapLocId PreviousLocationId { get; private set; }

    /// <summary>
    /// Current location. If null, the ship is moving between <see cref="P:Mafi.Core.World.TravelingFleet.PreviousLocationId" /> and <see cref="P:Mafi.Core.World.TravelingFleet.NextLocationId" />.
    /// </summary>
    public WorldMapLocId? CurrentLocationId { get; private set; }

    public WorldMapLocId NextLocationId { get; private set; }

    public Vector2f WorldPosition { get; private set; }

    public Tile2f DockedPosition
    {
      get
      {
        return this.Dock.Position2f + new RelTile2f(this.Dock.Transform.TransformMatrix.Transform(this.Prototype.ShipyardOffset.Vector2f));
      }
    }

    public Tile3f Position3f => this.DockedPosition.ExtendZ(Fix32.Zero);

    /// <summary>
    /// Current path of the fleet. If not empty, the fleet is moving towards its destination. First element is next
    /// immediate goal, last element is the final destination.
    /// </summary>
    public IIndexable<WorldMapLocId> Path => (IIndexable<WorldMapLocId>) this.m_path;

    public Percent TravelProgress
    {
      get
      {
        return this.IsIdle || !this.m_totalPathDistance.IsPositive ? Percent.Zero : Percent.FromRatio(this.m_distanceOnPathTraveled, this.m_totalPathDistance);
      }
    }

    public bool IsMoving => this.m_path.IsNotEmpty;

    public IReadOnlyDictionary<ProductProto, IProductBuffer> Cargo
    {
      get => (IReadOnlyDictionary<ProductProto, IProductBuffer>) this.m_cargo;
    }

    public int RefugeesCount { get; private set; }

    public bool IsAtHomeCell
    {
      get
      {
        WorldMapLocId? currentLocationId = this.CurrentLocationId;
        WorldMapLocId id = this.m_mapManager.Map.HomeLocation.Id;
        return currentLocationId.HasValue && currentLocationId.GetValueOrDefault() == id;
      }
    }

    public bool IsAutoReturnEnabled { get; set; }

    public bool IsDocked => this.LocationState == FleetLocationState.Docked;

    public Mafi.Core.Buildings.Shipyard.Shipyard Dock { get; private set; }

    public Option<Mafi.Core.Buildings.Shipyard.Shipyard> PendingDockAssignment { get; private set; }

    public AngleDegrees1f Direction { get; private set; }

    public int CrewRequired => this.WorkersNeeded;

    public bool HasAllRequiredCrew => ((IEntityWithWorkers) this).HasWorkersCached;

    public int CurrentCrew => !this.HasAllRequiredCrew ? 0 : this.WorkersNeeded;

    public int MaxHp => this.FleetEntity.Hull.MaxHp.GetValue();

    public int CurrentHp => this.FleetEntity.Hull.CurrentHp;

    public int MissingHp => this.MaxHp - this.CurrentHp;

    public bool NeedsRepair => this.MissingHp > 0;

    public Percent MissingHpPercent => Percent.Hundred - this.HpPercent;

    public Percent HpPercent => Percent.FromRatio(this.CurrentHp, this.MaxHp);

    public int MinOperableHp => this.Prototype.MinOperableHp.Apply(this.MaxHp);

    public bool HasEnoughHpToOperate => this.CurrentHp >= this.MinOperableHp;

    public bool CanOperate
    {
      get
      {
        return this.HasAllRequiredCrew && this.HasEnoughHpToOperate && this.Dock.ModificationState != ShipModificationState.Applying && !this.Dock.IsRepairing;
      }
    }

    public Percent ExplorationProgress => this.m_explorationTimer.PercentFinished;

    public IProductBufferReadOnly FuelBuffer => (IProductBufferReadOnly) this.m_fuelBuffer;

    public Quantity FuelQuantity => this.m_fuelBuffer.Quantity;

    public bool IsIdle { get; private set; }

    public FleetLocationState LocationState { get; private set; }

    public Duration RemainingTransitionDuration { get; private set; }

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

    public TravelingFleet(
      EntityId id,
      TravelingFleetProto proto,
      EntityContext context,
      IWorldMapManager mapManager,
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      Mafi.Core.Buildings.Shipyard.Shipyard dock,
      IProductsManager productsManager,
      IBattleSimulator battleSimulator,
      IWorldMapPathFinder pathFinder,
      SettlementsManager settlementsManager,
      INotificationsManager notificationsManager,
      IFuelStatsCollector fuelStatsCollector,
      IMessageNotificationsManager messageNotificationsManager,
      IEntitiesManager entitiesManager,
      TravelingFleetManager fleetManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_pathToGoalTmp = new Lyst<WorldMapLocId>(true);
      this.m_onLocationFullyExplored = new Event<WorldMapLocation, Option<WorldMapLoot>>();
      this.m_path = new Queueue<WorldMapLocId>();
      this.m_fleetStartLocTmp = new Lyst<WorldMapLocId>(true);
      this.m_cargo = new Dict<ProductProto, IProductBuffer>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CIsAutoReturnEnabled\u003Ek__BackingField = true;
      this.m_explorationTimer = new TickTimer();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (EntityProto) proto, context);
      this.Prototype = proto;
      this.Dock = dock;
      this.m_mapManager = mapManager;
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_productsManager = productsManager;
      this.m_battleSimulator = battleSimulator;
      this.m_pathFinder = pathFinder;
      this.m_settlementsManager = settlementsManager;
      this.m_notificationsManager = notificationsManager;
      this.m_fuelStatsCollector = fuelStatsCollector;
      this.m_messageNotificationsManager = messageNotificationsManager;
      this.m_entitiesManager = entitiesManager;
      this.m_fleetManager = fleetManager;
      this.GeneralPriority = proto.Costs.DefaultPriority;
      this.FleetEntity = new FleetEntity(proto.InitialHullProto, proto.InitialEngine, proto.InitialBridge);
      this.BattleFleet = new BattleFleet("Our ship", true);
      this.BattleFleet.AddEntity(this.FleetEntity);
      this.FleetEntity.OnFuelCapacityChange.Add<TravelingFleet>(this, new Action<Quantity>(this.onFuelCapacityChange));
      this.m_fuelBuffer = new ProductBuffer(this.FleetEntity.FuelTankCapacity, protosDb.GetOrThrow<ProductProto>((Proto.ID) IdsCore.Products.Diesel));
      this.SetCurrentLocationAndClearPath(this.m_mapManager.Map.HomeLocation);
      this.SetHp(this.Prototype.StartingHp.Apply(this.MaxHp));
      Assert.That<bool>(this.IsAtHomeCell).IsTrue();
      this.Direction = this.Dock.Transform.Rotation.Angle + AngleDegrees1f.Deg90;
      this.LocationState = FleetLocationState.Docked;
    }

    private void onFuelCapacityChange(Quantity newCapacity)
    {
      this.m_fuelBuffer.ForceNewCapacityTo(newCapacity);
    }

    public RelGameDate GetTravelTimeFromDistance(Fix32 totalDist)
    {
      Fix32 fix32 = this.FleetEntity.DistancePerStep * 20;
      return RelGameDate.FromDays((totalDist / fix32).ToIntCeiled());
    }

    public Quantity GetFuelCostFromDistance(Fix32 totalDist, bool isTargetExplored)
    {
      Quantity costFromDistance = this.FleetEntity.GetFuelCostFromDistance(totalDist);
      if (!isTargetExplored)
        costFromDistance += this.FleetEntity.GetFuelCostFromDistance((Fix32) 550);
      return costFromDistance;
    }

    void IEntityWithSimUpdate.SimUpdate()
    {
      this.IsIdle = this.LocationState == FleetLocationState.AtWorld && this.m_path.IsEmpty && !this.IsAtHomeCell;
      switch (this.LocationState)
      {
        case FleetLocationState.AtWorld:
          this.simulateWorldMapMovement();
          break;
        case FleetLocationState.ArrivingFromWorld:
        case FleetLocationState.DepartingToWorld:
          this.simulateIslandMovement();
          break;
        case FleetLocationState.Docked:
          this.handleDockedState();
          break;
        case FleetLocationState.ExploreInProgress:
          this.continueExploration();
          break;
        case FleetLocationState.BattleInProgress:
          this.waitForBattleResult();
          break;
        default:
          Log.Error(string.Format("Invalid fleet state: {0}", (object) this.LocationState));
          this.LocationState = FleetLocationState.AtWorld;
          break;
      }
    }

    private void handleDockedState()
    {
      if (this.PendingDockAssignment.HasValue)
      {
        this.LocationState = FleetLocationState.DepartingToWorld;
        this.RemainingTransitionDuration = this.Prototype.DockTransitionDuration;
      }
      if (!this.Dock.IsDestroyed)
        return;
      foreach (Mafi.Core.Buildings.Shipyard.Shipyard shipyard in this.m_entitiesManager.GetAllEntitiesOfType<Mafi.Core.Buildings.Shipyard.Shipyard>())
      {
        if (shipyard.CanBePrimary())
        {
          this.PendingDockAssignment = (Option<Mafi.Core.Buildings.Shipyard.Shipyard>) shipyard;
          break;
        }
      }
    }

    private void simulateIslandMovement()
    {
      if (this.RemainingTransitionDuration.IsPositive)
        this.RemainingTransitionDuration -= Duration.OneTick;
      else if (this.LocationState == FleetLocationState.ArrivingFromWorld)
        this.LocationState = FleetLocationState.Docked;
      else if (this.LocationState == FleetLocationState.DepartingToWorld)
        this.LocationState = FleetLocationState.AtWorld;
      else
        Log.Error("Invalid state.");
    }

    /// <summary>Leaves to the world if it is operational.</summary>
    public bool TryLeaveToWorld()
    {
      Assert.That<bool>(this.IsAtHomeCell).IsTrue();
      if (this.LocationState != FleetLocationState.Docked)
      {
        Log.Error(string.Format("Fleet cannot undock at state: {0}", (object) this.LocationState));
        return false;
      }
      if (!this.CanOperate || this.PendingDockAssignment.HasValue)
        return false;
      this.LocationState = FleetLocationState.DepartingToWorld;
      this.RemainingTransitionDuration = this.Prototype.DockTransitionDuration;
      return true;
    }

    internal void ForceLeaveToWorld()
    {
      if (this.LocationState != FleetLocationState.Docked)
        return;
      this.LocationState = FleetLocationState.DepartingToWorld;
      this.RemainingTransitionDuration = this.Prototype.DockTransitionDuration;
    }

    /// <summary>Returns free cargo capacity for refugees and goods.</summary>
    public int GetFreeCapacity()
    {
      return 0.Max(this.Prototype.CargoAndRefugeesCapacity - this.RefugeesCount - this.m_cargo.Sum<KeyValuePair<ProductProto, IProductBuffer>>((Func<KeyValuePair<ProductProto, IProductBuffer>, int>) (kvp => kvp.Value.Quantity.Value)));
    }

    public int GetFuelRemainingDistance()
    {
      return (this.m_fuelBuffer.Quantity.Value * this.FleetEntity.DistancePerFuel).ToIntFloored();
    }

    /// <summary>Teleports the fleet to given cell.</summary>
    public void SetCurrentLocationAndClearPath(WorldMapLocation location)
    {
      if (this.CurrentLocationId.HasValue)
        this.PreviousLocationId = this.CurrentLocationId.Value;
      this.CurrentLocationId = new WorldMapLocId?(this.NextLocationId = location.Id);
      this.WorldPosition = location.Position.Vector2f;
      this.m_path.Clear();
      this.visitLocation(location, true);
    }

    private void waitForBattleResult()
    {
      Assert.That<FleetLocationState>(this.LocationState).IsEqualTo<FleetLocationState>(FleetLocationState.BattleInProgress);
      if (this.m_battleState.IsNone)
      {
        Log.Error("No battle state set!");
        this.LocationState = FleetLocationState.AtWorld;
      }
      else
      {
        if (this.m_battleState.Value.Result.IsNone)
          return;
        this.LocationState = FleetLocationState.AtWorld;
        BattleResult battleResult = this.m_battleState.Value.Result.Value;
        Action<IBattleState> onBattleFinished = this.OnBattleFinished;
        if (onBattleFinished != null)
          onBattleFinished(this.m_battleState.Value);
        this.m_battleState = Option<IBattleState>.None;
        if (!battleResult.Winner.IsHuman)
        {
          this.m_pathToGoalTmp.Clear();
          if (this.FindPathTo(this.m_mapManager.Map.HomeLocation.Id, this.m_pathFinder, this.m_pathToGoalTmp) && this.m_pathToGoalTmp.Count > 1)
            this.SetPath(((IEnumerable<WorldMapLocId>) new WorldMapLocId[1]
            {
              this.m_pathToGoalTmp[1]
            }).AsEnumerable<WorldMapLocId>(), LocationVisitReason.General);
          else
            this.SetPath(((IEnumerable<WorldMapLocId>) new WorldMapLocId[1]
            {
              this.PreviousLocationId
            }).AsEnumerable<WorldMapLocId>(), LocationVisitReason.General);
          battleResult.Winner.RepairAll();
        }
        else
        {
          WorldMapLocation location = this.m_mapManager.Map[this.CurrentLocationId.Value].Value;
          location.MarkEnemyAsDefeated();
          this.loadLootIfCan(location);
        }
      }
    }

    private void visitLocation(WorldMapLocation location, bool skipExploration = false)
    {
      if (location.State != WorldMapLocationState.Explored)
      {
        if (skipExploration)
        {
          this.LocationState = FleetLocationState.ExploreInProgress;
          this.finishCurrentLocationExplore(location);
        }
        else
        {
          this.LocationState = FleetLocationState.ExploreInProgress;
          this.m_explorationFuelTicksCounter = 0;
          int intCeiled = (550 / this.FleetEntity.DistancePerFuel).ToIntCeiled();
          this.m_explorationTicksPerFuelQuantity = TravelingFleet.EXPLORATION_DURATION.Ticks / intCeiled;
          this.m_explorationTimer.Start(TravelingFleet.EXPLORATION_DURATION);
        }
      }
      else
      {
        if (this.startBattleIfNeeded(location))
          return;
        this.markLocationExplored(location);
        if (!this.m_path.IsEmpty)
          return;
        Option<IWorldMapEntity> entity = location.Entity;
        if (entity.ValueOrNull is WorldMapRepairableEntity valueOrNull1 && this.m_lastVisitReason == LocationVisitReason.DeliverCargo && valueOrNull1.IsUnderConstruction)
        {
          bool flag = false;
          foreach (IProductBuffer buffer in this.m_cargo.Values.ToArray<IProductBuffer>())
          {
            ProductQuantity toStore = buffer.Product.WithQuantity(buffer.Quantity);
            Quantity quantity = toStore.Quantity - valueOrNull1.StoreAsMuchAs(toStore);
            buffer.RemoveExactly(quantity);
            this.removeBufferIfEmpty(buffer);
            flag |= quantity.IsPositive;
          }
          if (flag)
            this.m_notificationsManager.NotifyOnce<EntityProto>(IdsCore.Notifications.ShipCargoDelivered, location.Entity.Value.Prototype);
        }
        entity = location.Entity;
        if (!(entity.ValueOrNull is WorldMapMine valueOrNull2) || this.m_lastVisitReason != LocationVisitReason.LoadCargo || !valueOrNull2.IsRepaired || !valueOrNull2.Buffer.IsNotEmpty())
          return;
        ProductQuantity productQuantity = valueOrNull2.Buffer.Product.WithQuantity(valueOrNull2.Buffer.Quantity);
        this.m_productsManager.ProductCreated(productQuantity, CreateReason.Imported);
        this.LoadCargo(productQuantity);
        valueOrNull2.RemoveAsMuchAs(productQuantity.Quantity);
      }
    }

    private bool startBattleIfNeeded(WorldMapLocation location)
    {
      if (location.Enemy.IsNone)
        return false;
      location.IsEnemyKnown = true;
      this.m_battleState = this.m_battleSimulator.StartBattle(location.Enemy.Value, this.BattleFleet);
      if (this.m_battleState.IsNone)
      {
        Log.Error("Failed to start a battle! Something is not right.");
        return false;
      }
      ++this.m_battlesEncountered;
      this.LocationState = FleetLocationState.BattleInProgress;
      bool flag = this.m_battlesEncountered <= 1;
      if (!flag)
        this.m_messageNotificationsManager.AddMessage((IMessageNotification) new ShipInBattleNotification(location, this.m_battleState.Value));
      Action<IBattleState, bool> onBattleStarted = this.OnBattleStarted;
      if (onBattleStarted != null)
        onBattleStarted(this.m_battleState.Value, flag);
      return true;
    }

    private void loadLootIfCan(WorldMapLocation location)
    {
      if (location.Loot.IsNone)
      {
        this.LocationState = FleetLocationState.AtWorld;
        this.m_messageNotificationsManager.AddMessage((IMessageNotification) new LocationExploredMessage(location, Option<WorldMapLoot>.None, ImmutableArray<TechnologyProto>.Empty));
        this.m_onLocationFullyExplored.Invoke(location, Option<WorldMapLoot>.None);
      }
      else
      {
        WorldMapLoot worldMapLoot = location.Loot.Value;
        this.RefugeesCount += worldMapLoot.People;
        if (worldMapLoot.Products.IsNotEmpty)
        {
          this.m_productsManager.ProductCreated(worldMapLoot.Products, CreateReason.Loot);
          this.LoadCargo(worldMapLoot.Products);
        }
        Lyst<TechnologyProto> lyst = new Lyst<TechnologyProto>();
        foreach (TechnologyProto technologyProto in worldMapLoot.ProtosToUnlock)
        {
          if (!this.m_unlockedProtosDb.IsUnlocked((Proto) technologyProto))
          {
            this.m_unlockedProtosDb.Unlock((Proto) technologyProto);
            lyst.Add(technologyProto);
          }
        }
        location.ClearLoot();
        this.LocationState = FleetLocationState.AtWorld;
        this.m_messageNotificationsManager.AddMessage((IMessageNotification) new LocationExploredMessage(location, Option.Create<WorldMapLoot>(worldMapLoot), lyst.ToImmutableArray()));
        this.m_onLocationFullyExplored.Invoke(location, Option.Create<WorldMapLoot>(worldMapLoot));
      }
    }

    private void continueExploration()
    {
      Assert.That<FleetLocationState>(this.LocationState).IsEqualTo<FleetLocationState>(FleetLocationState.ExploreInProgress);
      ++this.m_explorationFuelTicksCounter;
      if (this.m_explorationFuelTicksCounter >= this.m_explorationTicksPerFuelQuantity)
      {
        this.m_explorationFuelTicksCounter = 0;
        if (this.m_fuelBuffer.RemoveAsMuchAs(Quantity.One).IsPositive)
          this.m_fuelStatsCollector.ReportFuelUseAndDestroy(this.m_fuelBuffer.Product, Quantity.One, FuelUsedBy.BattleShip);
      }
      if (this.m_explorationTimer.Decrement())
        return;
      this.finishCurrentLocationExplore(this.m_mapManager.Map[this.CurrentLocationId.Value].Value);
    }

    private void finishCurrentLocationExplore(WorldMapLocation location)
    {
      Assert.That<FleetLocationState>(this.LocationState).IsEqualTo<FleetLocationState>(FleetLocationState.ExploreInProgress);
      this.markLocationExplored(location);
      if (this.startBattleIfNeeded(location))
        return;
      this.loadLootIfCan(location);
    }

    public void markLocationExplored(WorldMapLocation location)
    {
      int neighborDistanceToReveal = this.FleetEntity.Hull.RadarRange.GetValue().Max(1);
      this.m_mapManager.Map.Visit(location, neighborDistanceToReveal, this.FleetEntity.Hull.RadarRange.GetValue());
    }

    public void UnloadAllRefugees(SettlementsManager manager)
    {
      manager.AddPops(this.RefugeesCount, PopsAdditionReason.RefugeesOrAdopted);
      this.RefugeesCount = 0;
    }

    /// <summary>
    /// Adds as much cargo as possible to the ship. If ProductsManager is set, taken products are reported as new.
    /// </summary>
    public void LoadCargo(AssetValue value)
    {
      AssetValue assetValue = this.fillAsMuchAs(value, this.m_fuelBuffer);
      if (assetValue.IsEmpty)
        return;
      foreach (ProductQuantity product in assetValue.Products)
        this.LoadCargo(product);
    }

    private AssetValue fillAsMuchAs(AssetValue value, ProductBuffer buffer)
    {
      if (buffer.IsFull())
        return value;
      foreach (ProductQuantity product in value.Products)
      {
        if (!((Proto) product.Product != (Proto) buffer.Product))
        {
          Quantity quantity = buffer.UsableCapacity.Min(product.Quantity);
          buffer.StoreExactly(quantity);
          return value - new AssetValue(product.Product, quantity);
        }
      }
      return value;
    }

    /// <summary>Returns quantity that was not able to fit.</summary>
    public Quantity StoreFuelAsMuchAs(Quantity fuel)
    {
      Assert.That<bool>(this.IsDocked).IsTrue();
      return this.m_fuelBuffer.StoreAsMuchAs(fuel);
    }

    private IProductBuffer getOrCreateCargoBufferFor(ProductProto product)
    {
      IProductBuffer cargoBufferFor1;
      if (this.m_cargo.TryGetValue(product, out cargoBufferFor1))
        return cargoBufferFor1;
      IProductBuffer cargoBufferFor2 = (IProductBuffer) new ProductBuffer(Quantity.MaxValue, product);
      this.m_cargo.Add(cargoBufferFor2.Product, cargoBufferFor2);
      return cargoBufferFor2;
    }

    public void LoadCargo(ProductQuantity productQuantity)
    {
      if (productQuantity.IsEmpty)
        return;
      this.getOrCreateCargoBufferFor(productQuantity.Product).StoreExactly(productQuantity.Quantity);
    }

    /// <summary>Used by shipyard to transfer cargo from the ship.</summary>
    public ProductQuantity TryUnloadCargo(
      Quantity maxQuantity,
      IReadOnlySet<ProductProto> productsToSkip)
    {
      foreach (IProductBuffer buffer in this.m_cargo.Values)
      {
        if (!productsToSkip.Contains(buffer.Product))
        {
          Quantity quantity = buffer.RemoveAsMuchAs(maxQuantity);
          this.removeBufferIfEmpty(buffer);
          if (quantity.IsPositive)
            return new ProductQuantity(buffer.Product, quantity);
        }
      }
      return ProductQuantity.None;
    }

    private void removeBufferIfEmpty(IProductBuffer buffer)
    {
      if (!buffer.IsEmpty())
        return;
      this.m_cargo.Remove(buffer.Product);
    }

    private void simulateWorldMapMovement()
    {
      Assert.That<FleetLocationState>(this.LocationState).IsEqualTo<FleetLocationState>(FleetLocationState.AtWorld);
      if (this.m_path.IsEmpty)
      {
        if (this.IsAtHomeCell)
        {
          this.spawnOnIslandAndGoToDock();
        }
        else
        {
          if (!this.IsAutoReturnEnabled || this.m_battleState.HasValue || this.m_lastVisitReason == LocationVisitReason.LoadCargo || this.m_didFindNextValidLocation)
            return;
          if (this.CurrentHp < this.MinOperableHp)
          {
            this.sendShipHome();
          }
          else
          {
            this.m_didFindNextValidLocation = this.hasValidLocationToVisit();
            if (this.m_didFindNextValidLocation)
              return;
            this.sendShipHome();
          }
        }
      }
      else
      {
        this.m_didFindNextValidLocation = false;
        Fix32 fix32 = this.MoveOnMapBy(this.FleetEntity.DistancePerStep);
        if (fix32 <= 0)
          return;
        this.m_distanceOnPathTraveled += fix32;
        this.m_fuelDistanceTraveled += fix32;
        if (!(this.m_fuelDistanceTraveled > this.FleetEntity.DistancePerFuel))
          return;
        if (this.m_fuelBuffer.RemoveAsMuchAs(Quantity.One).IsPositive)
          this.m_fuelStatsCollector.ReportFuelUseAndDestroy(this.m_fuelBuffer.Product, Quantity.One, FuelUsedBy.BattleShip);
        this.m_fuelDistanceTraveled -= this.FleetEntity.DistancePerFuel;
      }
    }

    private void sendShipHome()
    {
      this.m_pathToGoalTmp.Clear();
      this.FindPathTo(this.m_mapManager.Map.HomeLocation.Id, this.m_pathFinder, this.m_pathToGoalTmp);
      this.SetPath((IEnumerable<WorldMapLocId>) this.m_pathToGoalTmp, LocationVisitReason.General);
    }

    private bool hasValidLocationToVisit()
    {
      foreach (WorldMapLocation location in (IEnumerable<WorldMapLocation>) this.m_mapManager.Map.Locations)
      {
        WorldMapLocId id = location.Id;
        WorldMapLocId? currentLocationId = this.CurrentLocationId;
        Quantity fuelCost;
        if ((currentLocationId.HasValue ? (id == currentLocationId.GetValueOrDefault() ? 1 : 0) : 0) == 0 && location.State == WorldMapLocationState.NotExplored && !(this.m_fleetManager.TravelingFleet.GetFuelCostFromDistance(this.WorldPosition.DistanceTo(location.Position.Vector2f) + location.Position.DistanceTo(this.m_mapManager.Map.HomeLocation.Position), false) > this.m_fleetManager.TravelingFleet.FuelQuantity) && this.m_fleetManager.ComputeRoundtripPathAndCosts(location.Id, out int _, out RelGameDate _, out fuelCost) && fuelCost < this.FuelQuantity)
        {
          this.m_didFindNextValidLocation = true;
          return true;
        }
      }
      return false;
    }

    public void MoveToNewShipyard(Mafi.Core.Buildings.Shipyard.Shipyard newDock)
    {
      if (this.Dock == newDock)
        Log.Error("Cannot move to the same dock we already have!");
      else
        this.PendingDockAssignment = (Option<Mafi.Core.Buildings.Shipyard.Shipyard>) newDock;
    }

    private void spawnOnIslandAndGoToDock()
    {
      Assert.That<FleetLocationState>(this.LocationState).IsEqualTo<FleetLocationState>(FleetLocationState.AtWorld);
      if (this.PendingDockAssignment.HasValue)
      {
        if (this.PendingDockAssignment.Value.CanBePrimary())
        {
          if (this.Dock.DockedFleet.HasValue)
            this.Dock.RemoveShip();
          this.Dock = this.PendingDockAssignment.Value;
          this.Dock.SetShip(this);
          this.Direction = this.Dock.Transform.Rotation.Angle + AngleDegrees1f.Deg90;
        }
        this.PendingDockAssignment = Option<Mafi.Core.Buildings.Shipyard.Shipyard>.None;
      }
      this.LocationState = FleetLocationState.ArrivingFromWorld;
      this.RemainingTransitionDuration = this.Prototype.DockTransitionDuration;
    }

    public Fix32 MoveOnMapBy(Fix32 distance)
    {
      Assert.That<FleetLocationState>(this.LocationState).IsEqualTo<FleetLocationState>(FleetLocationState.AtWorld);
      Assert.That<Queueue<WorldMapLocId>>(this.m_path).IsNotEmpty<WorldMapLocId>();
      Option<WorldMapLocation> option = this.m_mapManager.Map[this.m_path.First];
      if (option.IsNone)
      {
        Log.Error("Invalid location found on path. Ignoring.");
        this.m_path.Dequeue();
        return (Fix32) 0;
      }
      Vector2f vector2f1 = option.Value.Position.Vector2f;
      Vector2f vector2f2 = vector2f1 - this.WorldPosition;
      Fix32 length = vector2f2.Length;
      if (length <= distance)
      {
        WorldMapLocId id = this.m_path.Dequeue();
        this.CurrentLocationId = new WorldMapLocId?(this.NextLocationId = id);
        this.WorldPosition = vector2f1;
        this.visitLocation(this.m_mapManager.Map[id].Value);
        return length;
      }
      if (this.CurrentLocationId.HasValue)
      {
        this.PreviousLocationId = this.CurrentLocationId.Value;
        this.NextLocationId = this.m_path.First;
        this.CurrentLocationId = new WorldMapLocId?();
      }
      this.WorldPosition += distance * vector2f2 / length;
      return distance;
    }

    /// <summary>
    /// Finds path from current fleet position to given goal location.
    /// 
    /// If the fleet is in between locations, both ends of the edge are considered as valid start locations to avoid
    /// unnecessary and awkward fleet movement.
    /// </summary>
    public bool FindPathTo(
      WorldMapLocId goalLocationId,
      IWorldMapPathFinder pathFinder,
      Lyst<WorldMapLocId> outPath)
    {
      this.m_fleetStartLocTmp.Clear();
      if (this.CurrentLocationId.HasValue)
      {
        this.m_fleetStartLocTmp.Add(this.CurrentLocationId.Value);
      }
      else
      {
        Assert.That<WorldMapLocId>(this.PreviousLocationId).IsNotEqualTo<WorldMapLocId>(this.NextLocationId);
        this.m_fleetStartLocTmp.Add(this.NextLocationId);
        this.m_fleetStartLocTmp.Add(this.PreviousLocationId);
      }
      return pathFinder.FindPath(this.m_fleetStartLocTmp, goalLocationId, true, outPath);
    }

    /// <summary>
    /// Replaces current path. It is up to the caller to ensure that the new path is valid.
    /// </summary>
    public void SetPath(IEnumerable<WorldMapLocId> path, LocationVisitReason reason)
    {
      if (this.LocationState == FleetLocationState.Docked)
      {
        Log.Error("Cannot set path. Fleet is docked.");
      }
      else
      {
        this.m_lastVisitReason = reason;
        this.m_path.Clear();
        this.m_path.EnqueueRange(path);
        WorldMapLocId? currentLocationId = this.CurrentLocationId;
        if (currentLocationId.HasValue && this.m_path.Count > 0)
        {
          currentLocationId = this.CurrentLocationId;
          this.m_totalPathDistance = (Fix32) this.ComputeTravelDistanceBetween(currentLocationId.Value, this.m_path.Last<WorldMapLocId>());
        }
        else
          this.m_totalPathDistance = (Fix32) 0;
        this.m_distanceOnPathTraveled = Fix32.Zero;
      }
    }

    public void TryToLoadCrew()
    {
      this.Context.WorkersManager.CanWork((IEntityWithWorkers) this, true);
    }

    public void UnloadAllCrew()
    {
      this.Context.WorkersManager.ReturnWorkersVoluntarily((IEntityWithWorkers) this);
    }

    public void UnloadFuel()
    {
      if (this.FuelBuffer.IsEmpty() || this.LocationState != FleetLocationState.Docked)
        return;
      this.Dock.StoreProduct(this.FuelBuffer.Product.WithQuantity(this.FuelBuffer.Quantity));
      this.m_fuelBuffer.Clear();
    }

    public void SetHp(int hp) => this.FleetEntity.Hull.SetHp(hp);

    public void PerformModfications(FleetEntityModificationRequest modRequest)
    {
      int workersNeeded = this.WorkersNeeded;
      this.FleetEntity.PerformModfications(modRequest, this.m_protosDb);
      if (this.WorkersNeeded != workersNeeded)
        this.Context.WorkersManager.ReturnWorkersVoluntarily((IEntityWithWorkers) this);
      Action<TravelingFleet> modificationsDone = this.OnModificationsDone;
      if (modificationsDone == null)
        return;
      modificationsDone(this);
    }

    public string GetMainPrefab()
    {
      FleetEntitySlot fleetEntitySlot = this.FleetEntity.Slots.FirstOrDefault<FleetEntitySlot>((Predicate<FleetEntitySlot>) (x => x.Proto.TypeOfSlot == FleetEntitySlotProto.SlotType.Radars));
      return fleetEntitySlot == null || fleetEntitySlot.ExistingPart.IsNone || !(fleetEntitySlot.ExistingPart.Value is FleetBridgePartProto fleetBridgePartProto) ? "EMPTY" : fleetBridgePartProto.Graphics.PrefabPath;
    }

    /// <summary>Computes distance (in km) between two nodes.</summary>
    public int ComputeTravelDistanceBetween(
      WorldMapLocId origin,
      WorldMapLocId goal,
      bool allowOnlyExplored = true)
    {
      this.m_pathToGoalTmp.Clear();
      if (!this.m_pathFinder.FindPath(origin, goal, allowOnlyExplored, this.m_pathToGoalTmp))
        return 0;
      WorldMap map = this.m_mapManager.Map;
      Vector2f vector2f1 = map[this.m_pathToGoalTmp.First].Value.Position.Vector2f;
      Fix32 zero = Fix32.Zero;
      for (int index = 1; index < this.m_pathToGoalTmp.Count; ++index)
      {
        Vector2f vector2f2 = map[this.m_pathToGoalTmp[index]].Value.Position.Vector2f;
        zero += vector2f1.DistanceTo(vector2f2);
        vector2f1 = vector2f2;
      }
      return zero.ToIntCeiled();
    }

    public Option<string> CustomTitle { get; set; }

    static TravelingFleet()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TravelingFleet.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      TravelingFleet.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
      TravelingFleet.EXPLORATION_DURATION = 75.Seconds();
    }
  }
}
