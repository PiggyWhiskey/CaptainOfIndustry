// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.TravelingFleetManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Buildings.Shipyard;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Fleet;
using Mafi.Core.Input;
using Mafi.Core.MessageNotifications;
using Mafi.Core.Notifications;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Stats;
using Mafi.Core.Vehicles;
using Mafi.Core.World.Entities;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.World
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TravelingFleetManager : 
    ICommandProcessor<GoToLocationCmd>,
    IAction<GoToLocationCmd>,
    ICommandProcessor<FleetLoadCrewCmd>,
    IAction<FleetLoadCrewCmd>,
    ICommandProcessor<FleetUnloadCrewCmd>,
    IAction<FleetUnloadCrewCmd>,
    ICommandProcessor<FleetToggleAutoReturnCmd>,
    IAction<FleetToggleAutoReturnCmd>,
    ICommandProcessor<ExploreFinishCheatCmd>,
    IAction<ExploreFinishCheatCmd>,
    ICommandProcessor<FleetRepairCheatCmd>,
    IAction<FleetRepairCheatCmd>,
    ICommandProcessor<FleetModificationsPrepareCmd>,
    IAction<FleetModificationsPrepareCmd>,
    ICommandProcessor<FleetModificationsCancelCmd>,
    IAction<FleetModificationsCancelCmd>,
    ICommandProcessor<FleetUnloadFuelCmd>,
    IAction<FleetUnloadFuelCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    [DoNotSaveCreateNewOnLoad("new Lyst<WorldMapLocId>(canOmitClearing: true)", 0)]
    private readonly Lyst<WorldMapLocId> m_pathToGoalTmp;
    [DoNotSaveCreateNewOnLoad("new Lyst<WorldMapLocId>(canOmitClearing: true)", 0)]
    private readonly Lyst<WorldMapLocId> m_pathToHomeTmp;
    private Option<TravelingFleet> m_travelingFleet;
    private readonly WorldMapManager m_mapManager;
    private readonly ProtosDb m_protosDb;
    private readonly EntityContext m_context;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly IWorldMapPathFinder m_pathFinder;
    private readonly IVehicleBuffersRegistry m_vehicleBuffersRegistry;
    private readonly EntitiesManager m_entitiesManager;
    private readonly EntityId.Factory m_idFactory;
    private readonly SettlementsManager m_settlementsManager;
    private readonly IProductsManager m_productsManager;
    private readonly IBattleSimulator m_battleSimulator;
    private readonly INotificationsManager m_notificationsManager;
    private readonly IFuelStatsCollector m_fuelStatsCollector;
    private readonly IMessageNotificationsManager m_messageNotificationsManager;

    public static void Serialize(TravelingFleetManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TravelingFleetManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TravelingFleetManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteInt(this.FarthestLocationVisited);
      writer.WriteGeneric<IBattleSimulator>(this.m_battleSimulator);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<IFuelStatsCollector>(this.m_fuelStatsCollector);
      EntityId.Factory.Serialize(this.m_idFactory, writer);
      WorldMapManager.Serialize(this.m_mapManager, writer);
      writer.WriteGeneric<IMessageNotificationsManager>(this.m_messageNotificationsManager);
      writer.WriteGeneric<INotificationsManager>(this.m_notificationsManager);
      writer.WriteGeneric<IProductsManager>(this.m_productsManager);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      Option<TravelingFleet>.Serialize(this.m_travelingFleet, writer);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
      writer.WriteGeneric<IVehicleBuffersRegistry>(this.m_vehicleBuffersRegistry);
    }

    public static TravelingFleetManager Deserialize(BlobReader reader)
    {
      TravelingFleetManager travelingFleetManager;
      if (reader.TryStartClassDeserialization<TravelingFleetManager>(out travelingFleetManager))
        reader.EnqueueDataDeserialization((object) travelingFleetManager, TravelingFleetManager.s_deserializeDataDelayedAction);
      return travelingFleetManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.FarthestLocationVisited = reader.ReadInt();
      reader.SetField<TravelingFleetManager>(this, "m_battleSimulator", (object) reader.ReadGenericAs<IBattleSimulator>());
      reader.RegisterResolvedMember<TravelingFleetManager>(this, "m_context", typeof (EntityContext), true);
      reader.SetField<TravelingFleetManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<TravelingFleetManager>(this, "m_fuelStatsCollector", (object) reader.ReadGenericAs<IFuelStatsCollector>());
      reader.SetField<TravelingFleetManager>(this, "m_idFactory", (object) EntityId.Factory.Deserialize(reader));
      reader.SetField<TravelingFleetManager>(this, "m_mapManager", (object) WorldMapManager.Deserialize(reader));
      reader.SetField<TravelingFleetManager>(this, "m_messageNotificationsManager", (object) reader.ReadGenericAs<IMessageNotificationsManager>());
      reader.SetField<TravelingFleetManager>(this, "m_notificationsManager", (object) reader.ReadGenericAs<INotificationsManager>());
      reader.RegisterResolvedMember<TravelingFleetManager>(this, "m_pathFinder", typeof (IWorldMapPathFinder), true);
      reader.SetField<TravelingFleetManager>(this, "m_pathToGoalTmp", (object) new Lyst<WorldMapLocId>(true));
      reader.SetField<TravelingFleetManager>(this, "m_pathToHomeTmp", (object) new Lyst<WorldMapLocId>(true));
      reader.SetField<TravelingFleetManager>(this, "m_productsManager", (object) reader.ReadGenericAs<IProductsManager>());
      reader.RegisterResolvedMember<TravelingFleetManager>(this, "m_protosDb", typeof (ProtosDb), true);
      reader.SetField<TravelingFleetManager>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      this.m_travelingFleet = Option<TravelingFleet>.Deserialize(reader);
      reader.SetField<TravelingFleetManager>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
      reader.SetField<TravelingFleetManager>(this, "m_vehicleBuffersRegistry", (object) reader.ReadGenericAs<IVehicleBuffersRegistry>());
    }

    public event Action<TravelingFleet> OnFleetCreated;

    public bool HasFleet => this.m_travelingFleet != (TravelingFleet) null;

    public TravelingFleet TravelingFleet
    {
      get
      {
        return !(this.m_travelingFleet == (TravelingFleet) null) ? this.m_travelingFleet.Value : throw new Exception("Getting traveling fleet before it was created.");
      }
    }

    public int FarthestLocationVisited { get; private set; }

    public TravelingFleetManager(
      WorldMapManager mapManager,
      ProtosDb protosDb,
      EntityContext context,
      UnlockedProtosDb unlockedProtosDb,
      IWorldMapPathFinder pathFinder,
      IVehicleBuffersRegistry vehicleBuffersRegistry,
      EntitiesManager entitiesManager,
      EntityId.Factory idFactory,
      SettlementsManager settlementsManager,
      ISimLoopEvents simLoopEvents,
      IProductsManager productsManager,
      IBattleSimulator battleSimulator,
      INotificationsManager notificationsManager,
      IFuelStatsCollector fuelStatsCollector,
      IMessageNotificationsManager messageNotificationsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_pathToGoalTmp = new Lyst<WorldMapLocId>(true);
      this.m_pathToHomeTmp = new Lyst<WorldMapLocId>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_mapManager = mapManager;
      this.m_protosDb = protosDb;
      this.m_context = context;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_pathFinder = pathFinder;
      this.m_vehicleBuffersRegistry = vehicleBuffersRegistry;
      this.m_entitiesManager = entitiesManager;
      this.m_idFactory = idFactory;
      this.m_settlementsManager = settlementsManager;
      this.m_productsManager = productsManager;
      this.m_battleSimulator = battleSimulator;
      this.m_notificationsManager = notificationsManager;
      this.m_fuelStatsCollector = fuelStatsCollector;
      this.m_messageNotificationsManager = messageNotificationsManager;
      simLoopEvents.Update.Add<TravelingFleetManager>(this, new Action(this.simUpdate));
    }

    public void CreateAndAddShip(Mafi.Core.Buildings.Shipyard.Shipyard dock)
    {
      Assert.That<Option<TravelingFleet>>(this.m_travelingFleet).IsNone<TravelingFleet>("Cannon create new fleet. It already exists!");
      TravelingFleetProto orThrow = this.m_protosDb.GetOrThrow<TravelingFleetProto>((Proto.ID) IdsCore.World.Fleet);
      this.m_travelingFleet = (Option<TravelingFleet>) new TravelingFleet(this.m_idFactory.GetNextId(), orThrow, this.m_context, (IWorldMapManager) this.m_mapManager, this.m_protosDb, this.m_unlockedProtosDb, this.m_vehicleBuffersRegistry, dock, this.m_productsManager, this.m_battleSimulator, this.m_pathFinder, this.m_settlementsManager, this.m_notificationsManager, this.m_fuelStatsCollector, this.m_messageNotificationsManager, (IEntitiesManager) this.m_entitiesManager, this);
      this.m_entitiesManager.AddEntityNoChecks((IEntity) this.TravelingFleet);
      this.m_travelingFleet.Value.OnLocationFullyExplored.Add<TravelingFleetManager>(this, new Action<WorldMapLocation, Option<WorldMapLoot>>(this.onLocationExplored));
      dock.SetShip(this.TravelingFleet);
      Action<TravelingFleet> onFleetCreated = this.OnFleetCreated;
      if (onFleetCreated != null)
        onFleetCreated(this.TravelingFleet);
      this.OnFleetCreated = (Action<TravelingFleet>) null;
    }

    private void onLocationExplored(WorldMapLocation location, Option<WorldMapLoot> loot)
    {
      this.FarthestLocationVisited = this.FarthestLocationVisited.Max(this.ComputeTravelDistanceBetween(this.m_mapManager.Map.HomeLocation.Id, location.Id));
    }

    private void simUpdate()
    {
      if (this.m_travelingFleet == (TravelingFleet) null || this.TravelingFleet.LocationState != FleetLocationState.Docked || this.TravelingFleet.RefugeesCount <= 0)
        return;
      this.TravelingFleet.UnloadAllRefugees(this.m_settlementsManager);
    }

    public TravelingFleetManager.LocationVisitCheckResult CanRequestLocationVisit(
      WorldMapLocation location)
    {
      WorldMapLocId id1 = location.Id;
      WorldMapLocId? currentLocationId1 = this.TravelingFleet.CurrentLocationId;
      if ((currentLocationId1.HasValue ? (id1 == currentLocationId1.GetValueOrDefault() ? 1 : 0) : 0) != 0 && location.Entity.HasValue && location.Entity.Value is WorldMapMine worldMapMine && worldMapMine.Buffer.IsNotEmpty())
        return TravelingFleetManager.LocationVisitCheckResult.Ok;
      WorldMapLocId id2 = location.Id;
      WorldMapLocId? currentLocationId2 = this.TravelingFleet.CurrentLocationId;
      if ((currentLocationId2.HasValue ? (id2 == currentLocationId2.GetValueOrDefault() ? 1 : 0) : 0) != 0 || this.TravelingFleet.Path.Contains<WorldMapLocId>(location.Id))
        return TravelingFleetManager.LocationVisitCheckResult.AlreadyHeadingThereOrPresent;
      if (location == this.m_mapManager.Map.HomeLocation)
        return TravelingFleetManager.LocationVisitCheckResult.Ok;
      if (this.TravelingFleet.CurrentHp < this.TravelingFleet.MinOperableHp)
        return TravelingFleetManager.LocationVisitCheckResult.Damaged;
      if (this.TravelingFleet.Dock.ModificationState == ShipModificationState.Applying)
        return TravelingFleetManager.LocationVisitCheckResult.ShipIsBeingModified;
      if (this.TravelingFleet.Dock.IsRepairing)
        return TravelingFleetManager.LocationVisitCheckResult.ShipIsBeingRepaired;
      Quantity fuelCost;
      if (!this.ComputeRoundtripPathAndCosts(location.Id, out int _, out RelGameDate _, out fuelCost))
        return TravelingFleetManager.LocationVisitCheckResult.NotAccessible;
      if (fuelCost > this.TravelingFleet.FuelBuffer.Capacity)
        return TravelingFleetManager.LocationVisitCheckResult.TooFar;
      if (fuelCost > this.TravelingFleet.FuelQuantity)
        return TravelingFleetManager.LocationVisitCheckResult.NotEnoughFuel;
      if (!this.TravelingFleet.HasAllRequiredCrew)
        return TravelingFleetManager.LocationVisitCheckResult.NotEnoughCrew;
      if (this.TravelingFleet.LocationState == FleetLocationState.ArrivingFromWorld)
        return TravelingFleetManager.LocationVisitCheckResult.Docking;
      Assert.That<bool>(this.TravelingFleet.CanOperate).IsTrue();
      return TravelingFleetManager.LocationVisitCheckResult.Ok;
    }

    /// <summary>Computes distance (in km) between two nodes.</summary>
    public int ComputeTravelDistanceBetween(
      WorldMapLocId origin,
      WorldMapLocId goal,
      bool allowOnlyExplored = true)
    {
      TravelingFleet valueOrNull = this.m_travelingFleet.ValueOrNull;
      return valueOrNull == null ? 0 : valueOrNull.ComputeTravelDistanceBetween(origin, goal, allowOnlyExplored);
    }

    public bool ComputeToGoalPathAndCosts(
      WorldMapLocId goal,
      out int distance,
      out RelGameDate duration,
      out Quantity fuelCost)
    {
      distance = 0;
      duration = RelGameDate.Zero;
      fuelCost = Quantity.Zero;
      this.m_pathToGoalTmp.Clear();
      if (!this.TravelingFleet.FindPathTo(goal, this.m_pathFinder, this.m_pathToGoalTmp))
        return false;
      WorldMap map = this.m_mapManager.Map;
      Vector2f other = map[this.m_pathToGoalTmp.First].Value.Position.Vector2f;
      Fix32 totalDist = this.TravelingFleet.WorldPosition.DistanceTo(other);
      for (int index = 1; index < this.m_pathToGoalTmp.Count; ++index)
      {
        Vector2f vector2f = map[this.m_pathToGoalTmp[index]].Value.Position.Vector2f;
        totalDist += other.DistanceTo(vector2f);
        other = vector2f;
      }
      distance = totalDist.ToIntCeiled();
      duration = this.TravelingFleet.GetTravelTimeFromDistance(totalDist);
      fuelCost = this.TravelingFleet.GetFuelCostFromDistance(totalDist, this.m_mapManager.Map[goal].Value.State == WorldMapLocationState.Explored);
      return true;
    }

    /// <summary>
    /// Computes path to goal and back home, together with estimated food and fuel costs.
    /// </summary>
    public bool ComputeRoundtripPathAndCosts(
      WorldMapLocId goal,
      out int distance,
      out RelGameDate duration,
      out Quantity fuelCost,
      Lyst<WorldMapLocId> toGoalPath = null,
      Lyst<WorldMapLocId> toHomePath = null)
    {
      if (toGoalPath == null)
        toGoalPath = this.m_pathToGoalTmp;
      if (toHomePath == null)
        toHomePath = this.m_pathToHomeTmp;
      Assert.That<Lyst<WorldMapLocId>>(toGoalPath).IsNotEqualTo<Lyst<WorldMapLocId>>(toHomePath);
      distance = 0;
      duration = RelGameDate.Zero;
      fuelCost = Quantity.Zero;
      toGoalPath.Clear();
      if (!this.TravelingFleet.FindPathTo(goal, this.m_pathFinder, toGoalPath))
        return false;
      toHomePath.Clear();
      if (!this.m_pathFinder.FindPath(this.m_pathToGoalTmp.Last, this.m_mapManager.Map.HomeLocation.Id, true, toHomePath))
        return false;
      WorldMap map = this.m_mapManager.Map;
      Vector2f other = map[toGoalPath.First].Value.Position.Vector2f;
      Fix32 totalDist = this.TravelingFleet.WorldPosition.DistanceTo(other);
      for (int index = 1; index < toGoalPath.Count; ++index)
      {
        Vector2f vector2f = map[toGoalPath[index]].Value.Position.Vector2f;
        totalDist += other.DistanceTo(vector2f);
        other = vector2f;
      }
      for (int index = 1; index < toHomePath.Count; ++index)
      {
        Vector2f vector2f = map[toHomePath[index]].Value.Position.Vector2f;
        totalDist += other.DistanceTo(vector2f);
        other = vector2f;
      }
      distance = totalDist.ToIntCeiled();
      duration = this.TravelingFleet.GetTravelTimeFromDistance(totalDist);
      fuelCost = this.TravelingFleet.GetFuelCostFromDistance(totalDist, this.m_mapManager.Map[goal].Value.State == WorldMapLocationState.Explored);
      return true;
    }

    void IAction<GoToLocationCmd>.Invoke(GoToLocationCmd cmd)
    {
      Option<WorldMapLocation> option = this.m_mapManager.Map[cmd.LocationId];
      if (option.IsNone)
      {
        cmd.SetResultError(string.Format("Invalid goal location: {0}", (object) cmd.LocationId));
      }
      else
      {
        Quantity fuelCost;
        if (!this.ComputeRoundtripPathAndCosts(option.Value.Id, out int _, out RelGameDate _, out fuelCost))
          cmd.SetResultError(string.Format("Cannot find path to loc {0} and back home.", (object) option.Value.Id));
        else if (this.m_mapManager.Map.HomeLocation.Id != cmd.LocationId && fuelCost > this.TravelingFleet.FuelQuantity)
          cmd.SetResultError(string.Format("Not enough fuel to go to loc {0} anc back home.", (object) option.Value.Id));
        else if (this.TravelingFleet.LocationState == FleetLocationState.Docked && !this.TravelingFleet.TryLeaveToWorld())
        {
          cmd.SetResultError("Fleet cannot undock.");
        }
        else
        {
          this.TravelingFleet.SetPath((IEnumerable<WorldMapLocId>) this.m_pathToGoalTmp, cmd.Reason);
          cmd.SetResultSuccess();
        }
      }
    }

    void IAction<ExploreFinishCheatCmd>.Invoke(ExploreFinishCheatCmd cmd)
    {
      foreach (WorldMapLocId id in this.TravelingFleet.Path.AsEnumerable().ToArray<WorldMapLocId>())
        this.TravelingFleet.SetCurrentLocationAndClearPath(this.m_mapManager.Map[id].Value);
      cmd.SetResultSuccess();
    }

    void IAction<FleetLoadCrewCmd>.Invoke(FleetLoadCrewCmd cmd)
    {
      if (this.TravelingFleet.LocationState != FleetLocationState.Docked)
      {
        cmd.SetResultError("Fleet not docked.");
      }
      else
      {
        this.TravelingFleet.TryToLoadCrew();
        cmd.SetResultSuccess();
      }
    }

    void IAction<FleetUnloadCrewCmd>.Invoke(FleetUnloadCrewCmd cmd)
    {
      if (this.TravelingFleet.LocationState != FleetLocationState.Docked)
      {
        cmd.SetResultError("Fleet not docked.");
      }
      else
      {
        this.TravelingFleet.UnloadAllCrew();
        cmd.SetResultSuccess();
      }
    }

    void IAction<FleetUnloadFuelCmd>.Invoke(FleetUnloadFuelCmd cmd)
    {
      if (this.TravelingFleet.LocationState != FleetLocationState.Docked)
      {
        cmd.SetResultError("Fleet not docked.");
      }
      else
      {
        this.TravelingFleet.UnloadFuel();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(FleetRepairCheatCmd cmd)
    {
      this.TravelingFleet.FleetEntity.Repair();
      cmd.SetResultSuccess();
    }

    public void Invoke(FleetModificationsPrepareCmd prepareCmd)
    {
      Mafi.Core.Buildings.Shipyard.Shipyard dock = this.TravelingFleet.Dock;
      if (dock.IsRepairing)
        prepareCmd.SetResultError("Fleet is being repaired.");
      else if (dock.DockedFleet.Value.NeedsRepair)
        prepareCmd.SetResultError("Fleet needs repairs first.");
      else if (dock.ModificationState != ShipModificationState.None)
      {
        prepareCmd.SetResultError("Fleet is already being modified.");
      }
      else
      {
        dock.PerformShipModifications(prepareCmd.ModificationRequest);
        prepareCmd.SetResultSuccess();
      }
    }

    public void Invoke(FleetModificationsCancelCmd cmd)
    {
      if (!this.TravelingFleet.Dock.CanCancelModifications)
      {
        cmd.SetResultError("There are no modifications in progress!");
      }
      else
      {
        this.TravelingFleet.Dock.CancelModifications();
        cmd.SetResultSuccess();
      }
    }

    public void Invoke(FleetToggleAutoReturnCmd cmd)
    {
      this.TravelingFleet.IsAutoReturnEnabled = !this.TravelingFleet.IsAutoReturnEnabled;
      cmd.SetResultSuccess();
    }

    static TravelingFleetManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TravelingFleetManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TravelingFleetManager) obj).SerializeData(writer));
      TravelingFleetManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TravelingFleetManager) obj).DeserializeData(reader));
    }

    public enum LocationVisitCheckResult
    {
      Ok,
      AlreadyHeadingThereOrPresent,
      Damaged,
      ShipIsBeingModified,
      ShipIsBeingRepaired,
      NotAccessible,
      NotEnoughFuel,
      NotEnoughCrew,
      Docking,
      TooFar,
    }
  }
}
