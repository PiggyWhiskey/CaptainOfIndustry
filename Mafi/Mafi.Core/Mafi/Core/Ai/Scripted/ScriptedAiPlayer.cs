// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ai.Scripted.ScriptedAiPlayer
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Ai.Scripted.Actions;
using Mafi.Core.Buildings.Beacons;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.ElectricPower;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Maintenance;
using Mafi.Core.Population;
using Mafi.Core.Population.Refugees;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Utils;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Commands;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Ai.Scripted
{
  [GenerateSerializer(false, null, 0)]
  public class ScriptedAiPlayer
  {
    public const string DOCK_ENTITY_NAME = "dock_entity";
    public const string SETTLEMENT_NAME = "house_0";
    public readonly ISimLoopEvents SimLoopEvents;
    public readonly ProtosDb ProtosDb;
    private readonly ScriptedAiPlayerConfig m_config;
    private readonly DependencyResolver m_resolver;
    private readonly TerrainManager m_terrainManager;
    private readonly EntitiesManager m_entitiesManager;
    private readonly WorkersManager m_workersManager;
    private readonly RefugeesManager m_refugeesManager;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly IInputScheduler m_inputScheduler;
    private readonly SettlementsManager m_settlementsManager;
    private readonly MaintenanceManager m_maintenanceManager;
    private readonly IElectricityManager m_electricityManager;
    private readonly UpointsManager m_upointsManager;
    private int m_nextActionIndex;
    private Option<IScriptedAiPlayerAction> m_currentAction;
    private Option<IScriptedAiPlayerActionCore> m_currentActionCore;
    private bool m_isInitialized;
    private Tile2i m_previousBuildingPosition;
    private RelTile2i m_previousBuildingSize;
    private Tile2i m_previousBuildingPositionAlt;
    private RelTile2i m_previousBuildingSizeAlt;
    private readonly Dict<string, EntityId> m_namedEntities;
    private bool m_brokenEntitiesEarlyWarningReported;
    private bool m_lowPowerEarlyWarningReported;
    private int m_skipLowPowerEarlyWarning;
    private bool m_noRecipesEarlyWarningReported;
    private int m_recoveredVehicles;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public int Stage { get; private set; }

    public bool IsDoneWithAllActions { get; private set; }

    public int CurrentActionIndex => this.m_nextActionIndex - 1;

    public int ActionsCount => this.m_config.Actions.Length;

    public string CurrentActionDescription
    {
      get => !this.m_currentAction.HasValue ? "n/a" : this.m_currentAction.Value.Description;
    }

    public bool IsInstaBuildEnabled { get; private set; }

    public ScriptedAiPlayer(
      ScriptedAiPlayerConfig config,
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      DependencyResolver resolver,
      TerrainManager terrainManager,
      ProtosDb protosDb,
      EntitiesManager entitiesManager,
      WorkersManager workersManager,
      RefugeesManager refugeesManager,
      IVehiclesManager vehiclesManager,
      IInputScheduler inputScheduler,
      SettlementsManager settlementsManager,
      MaintenanceManager maintenanceManager,
      IElectricityManager electricityManager,
      UpointsManager upointsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_namedEntities = new Dict<string, EntityId>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_config = config;
      this.SimLoopEvents = simLoopEvents;
      this.m_resolver = resolver;
      this.m_terrainManager = terrainManager;
      this.ProtosDb = protosDb;
      this.m_entitiesManager = entitiesManager;
      this.m_workersManager = workersManager;
      this.m_refugeesManager = refugeesManager;
      this.m_vehiclesManager = vehiclesManager;
      this.m_inputScheduler = inputScheduler;
      this.m_settlementsManager = settlementsManager;
      this.m_maintenanceManager = maintenanceManager;
      this.m_electricityManager = electricityManager;
      this.m_upointsManager = upointsManager;
      this.m_isInitialized = false;
      this.m_previousBuildingPosition = config.FirstBuildingPosition;
      this.m_previousBuildingSize = RelTile2i.Zero;
      this.m_previousBuildingPositionAlt = config.FirstBuildingPositionAlt;
      this.m_previousBuildingSizeAlt = RelTile2i.Zero;
      gameLoopEvents.RegisterNewGameInitialized((object) this, (Action) (() => this.SimLoopEvents.Sync.Add<ScriptedAiPlayer>(this, new Action(this.sync))));
    }

    private void sync()
    {
      Assert.That<bool>(this.IsDoneWithAllActions).IsFalse();
      if (!this.m_isInitialized)
      {
        if (this.m_config.StartAtStage != 0)
          this.setInstaBuild(true);
        this.RegisterNamedEntity(this.m_entitiesManager.GetFirstEntityOfType<Mafi.Core.Buildings.Shipyard.Shipyard>().ValueOrThrow("Shipyard not found").Id, "dock_entity");
        this.RegisterNamedEntity(this.m_entitiesManager.GetFirstEntityOfType<SettlementHousingModule>().ValueOrThrow("Settlement not found").Id, "house_0");
        this.m_isInitialized = true;
      }
      else if (this.SimLoopEvents.CurrentStep.Value % 100 == 0 && !this.performChecks())
      {
        this.Stop();
      }
      else
      {
        if (this.IsInstaBuildEnabled && this.m_workersManager.AmountOfFreeWorkersOrMissing < 0 && this.m_refugeesManager.NextReward.HasValue)
          this.m_refugeesManager.Cheat_FinishCurrentDiscovery();
        if (this.m_currentAction.IsNone)
        {
          Assert.That<Option<IScriptedAiPlayerActionCore>>(this.m_currentActionCore).IsNone<IScriptedAiPlayerActionCore>();
          int nextActionIndex = this.m_nextActionIndex;
          ImmutableArray<IScriptedAiPlayerAction> actions = this.m_config.Actions;
          int length = actions.Length;
          if (nextActionIndex >= length)
          {
            Log.Info("Scripted player: Done with processing all actions. " + string.Format("Current action {0}/{1}, stage: {2}.", (object) this.CurrentActionIndex, (object) this.ActionsCount, (object) this.Stage));
            this.Stop();
            return;
          }
          actions = this.m_config.Actions;
          IScriptedAiPlayerAction scriptedAiPlayerAction = actions[this.m_nextActionIndex];
          Log.Info(string.Format("Scripted player: Starting action #{0}: {1}", (object) this.m_nextActionIndex, (object) scriptedAiPlayerAction.Description));
          ++this.m_nextActionIndex;
          object[] andInit = ArrayPool<object>.GetAndInit((object) scriptedAiPlayerAction);
          object obj = this.m_resolver.Instantiate(scriptedAiPlayerAction.ActionCoreType, andInit);
          andInit.ReturnToPool<object>();
          if (!(obj is IScriptedAiPlayerActionCore playerActionCore))
          {
            Log.Error("Scripted player: Action's core is not `IScriptedPlayerActionCore`, skipping. " + string.Format("Current action {0}/{1}, stage: {2}.", (object) this.CurrentActionIndex, (object) this.ActionsCount, (object) this.Stage));
            return;
          }
          this.m_currentAction = Option.Some<IScriptedAiPlayerAction>(scriptedAiPlayerAction);
          this.m_currentActionCore = Option.Some<IScriptedAiPlayerActionCore>(playerActionCore);
        }
        if (!this.m_currentActionCore.HasValue || !this.m_currentActionCore.Value.Perform(this))
          return;
        this.m_currentActionCore = Option<IScriptedAiPlayerActionCore>.None;
        Log.Info(string.Format("Scripted player: Done with action #{0}/{1}", (object) (this.m_nextActionIndex - 1), (object) this.ActionsCount));
        this.m_currentAction = Option<IScriptedAiPlayerAction>.None;
      }
    }

    private bool performChecks()
    {
      if (this.m_electricityManager.DemandedThisTick > this.m_electricityManager.GenerationCapacityThisTick)
      {
        if (this.m_skipLowPowerEarlyWarning > 0)
        {
          --this.m_skipLowPowerEarlyWarning;
        }
        else
        {
          IElectricityManager electricityManager = this.m_electricityManager;
          if (this.m_lowPowerEarlyWarningReported)
          {
            Log.Error(string.Format("Low power: {0}/{1}", (object) electricityManager.GenerationCapacityThisTick, (object) electricityManager.DemandedThisTick));
          }
          else
          {
            Log.Info("Electricity issues early warning: " + string.Format("{0}/{1}", (object) electricityManager.GenerationCapacityThisTick, (object) electricityManager.DemandedThisTick));
            this.m_lowPowerEarlyWarningReported = true;
          }
        }
      }
      if (this.m_workersManager.AmountOfFreeWorkersOrMissing < 0 && this.m_entitiesManager.GetFirstEntityOfType<Beacon>().IsNone)
      {
        Log.Error("Scripted player: No workers left and no beacon built. Terminating AI player. \n" + string.Format("Population: {0} ({1} homeless)\n", (object) this.m_settlementsManager.GetTotalPopulation(), (object) this.m_settlementsManager.NumberOfHomeless) + string.Format("Current action {0}/{1}: ", (object) this.CurrentActionIndex, (object) this.ActionsCount) + (this.m_currentAction.HasValue ? this.m_currentAction.Value.Description : "n/a") + "\n" + string.Format("Stage: {0}\n", (object) this.Stage));
        return false;
      }
      if (!this.m_upointsManager.CanConsume(2.Upoints()))
        this.m_upointsManager.AddInitialUnity(10.Upoints());
      if (Percent.FromRatio(this.m_vehiclesManager.AllVehicles.Count<Vehicle>((Func<Vehicle, bool>) (v => v.IsFuelTankEmpty)), this.m_vehiclesManager.AllVehicles.Count) > 50.Percent())
        Log.Error("More than 50% of vehicles have empty tank.");
      foreach (Vehicle allVehicle in (IEnumerable<Vehicle>) this.m_vehiclesManager.AllVehicles)
      {
        if (allVehicle.UnreachableGoal.HasValue)
        {
          this.m_upointsManager.AddInitialUnity(VehiclesManager.VEHICLE_RECOVERY_COST);
          this.m_inputScheduler.ScheduleInputCmd<RecoverVehicleCmd>(new RecoverVehicleCmd(allVehicle));
          ++this.m_recoveredVehicles;
          int recoveredVehicles = this.m_recoveredVehicles;
          Duration duration = this.SimLoopEvents.CurrentStep.Value.Ticks();
          Fix64 fix64 = duration.Minutes / 2;
          if (recoveredVehicles > fix64)
          {
            string str = string.Format("Too many recovered vehicles: {0}, ", (object) this.m_recoveredVehicles);
            duration = this.SimLoopEvents.CurrentStep.Value.Ticks();
            string calendarDurationStr = duration.ToCalendarDurationStr();
            Log.Error(str + "game duration: " + calendarDurationStr);
          }
        }
      }
      if (this.m_settlementsManager.ArePeopleStarving)
        Log.Error("People are starving!");
      Lyst<IMaintainedEntity> lyst1 = this.m_entitiesManager.Entities.AsEnumerable().OfType<IMaintainedEntity>().ToLyst<IMaintainedEntity>();
      int num1 = lyst1.Count<IMaintainedEntity>((Func<IMaintainedEntity, bool>) (x => x.Maintenance.Status.IsBroken));
      if (lyst1.Count >= 20)
      {
        int num2 = lyst1.Count / 10;
        if (num1 > num2)
        {
          if (this.m_brokenEntitiesEarlyWarningReported)
          {
            IEnumerable<MaintenanceDepot> allEntitiesOfType = this.m_entitiesManager.GetAllEntitiesOfType<MaintenanceDepot>();
            Log.Error(string.Format("More than 10% of maintainable entities are broken ({0}/{1}).\n", (object) num1, (object) lyst1.Count) + "Depots: " + allEntitiesOfType.Select<MaintenanceDepot, string>((Func<MaintenanceDepot, string>) (x => string.Format("#{0} {1}working", (object) x.Id, x.WorkedThisTick ? (object) "" : (object) "NOT "))).JoinStrings(", "));
          }
          else
          {
            Log.Info(string.Format("Maintenance issues early warning: broken {0}/{1}", (object) num1, (object) lyst1.Count));
            this.m_brokenEntitiesEarlyWarningReported = true;
          }
        }
      }
      Lyst<Machine> lyst2 = this.m_entitiesManager.Entities.AsEnumerable().OfType<Machine>().Where<Machine>((Func<Machine, bool>) (x => x.IsEnabled && x.RecipesAssigned.IsEmpty<RecipeProto>())).ToLyst<Machine>();
      if (lyst2.IsEmpty)
      {
        this.m_noRecipesEarlyWarningReported = false;
      }
      else
      {
        string str = ((IEnumerable<string>) lyst2.Select<string>((Func<Machine, string>) (x => x.ToString()))).JoinStrings(", ");
        if (this.m_noRecipesEarlyWarningReported)
        {
          Log.Error("No selected recipes in entities: " + str + " (in very rare case this might be false positive).");
        }
        else
        {
          Log.Info("No selected recipes in entities (may be temporary): " + str);
          this.m_noRecipesEarlyWarningReported = true;
        }
      }
      return true;
    }

    public void Stop()
    {
      this.SimLoopEvents.Sync.Remove<ScriptedAiPlayer>(this, new Action(this.sync));
      this.IsDoneWithAllActions = true;
      if (!this.IsInstaBuildEnabled)
        return;
      this.setInstaBuild(false);
    }

    public void SetStage(int stage)
    {
      Assert.That<int>(stage).IsGreater(this.Stage, "Stages should be increasing");
      this.Stage = stage;
      this.m_skipLowPowerEarlyWarning = 100;
      if (this.IsInstaBuildEnabled && this.m_config.StartAtStage >= 0 && this.Stage >= this.m_config.StartAtStage)
        this.setInstaBuild(false);
      if (this.m_config.TerminateAfterStage <= 0 || this.Stage <= this.m_config.TerminateAfterStage)
        return;
      Log.Info(string.Format("Scripted player: Terminating after configured stage {0}.", (object) this.m_config.TerminateAfterStage));
      this.Stop();
    }

    private void setInstaBuild(bool setEnabled)
    {
      Assert.That<bool>(setEnabled).IsNotEqualTo<bool>(this.IsInstaBuildEnabled);
      Log.Info("Scripted player: " + (setEnabled ? "Ena" : "Dsia") + "bling insta-build.");
      this.m_inputScheduler.ScheduleInputCmd<SetInstaBuildCmd>(new SetInstaBuildCmd(setEnabled));
      this.IsInstaBuildEnabled = setEnabled;
    }

    public Tile3i GetNextBuildingPosition(StaticEntityProto.ID id, PlacementSpec placement)
    {
      if (placement.CustomPosition.HasValue)
        return this.GetSurfaceTile(placement.CustomPosition.Value);
      RelTile3i layoutSize;
      RelTile2i relTile2i1;
      if (this.ProtosDb.GetOrThrow<StaticEntityProto>((Proto.ID) id) is LayoutEntityProto orThrow)
      {
        layoutSize = orThrow.Layout.LayoutSize;
        relTile2i1 = layoutSize.Xy;
      }
      else
        relTile2i1 = new RelTile2i(10, 10);
      RelTile2i absValue1 = relTile2i1.Rotate(placement.Rotation).AbsValue;
      EntityId id1;
      ILayoutEntity entity;
      Tile2i tile;
      if (placement.RelativeTo.HasValue && this.m_namedEntities.TryGetValue(placement.RelativeTo.Value, out id1) && this.m_entitiesManager.TryGetEntity<ILayoutEntity>(id1, out entity))
      {
        layoutSize = entity.Prototype.Layout.LayoutSize;
        RelTile2i absValue2 = layoutSize.Xy.Rotate(entity.Transform.Rotation).AbsValue;
        int num = placement.CustomSpacing ?? this.m_config.BuildingsGridSpacing.Value;
        RelTile2i relTile2i2 = (absValue2 + absValue1) / 2 + new RelTile2i(num, num);
        Vector2i directionVector = placement.RelativeToDirection.DirectionVector;
        relTile2i2 = new RelTile2i(relTile2i2.X * directionVector.X, relTile2i2.Y * directionVector.Y);
        tile = entity.Transform.Position.Xy + relTile2i2 + placement.RelativeOffset;
      }
      else
      {
        if (placement.RelativeTo.HasValue)
          Log.Error(string.Format("Failed to place entity {0} next to non-existent entity {1}.", (object) id, (object) placement.RelativeTo.Value));
        RelTile2i relTile2i3 = placement.AltLane ? this.m_previousBuildingSizeAlt : this.m_previousBuildingSize;
        int num = placement.CustomSpacing ?? this.m_config.BuildingsGridSpacing.Value;
        RelTile2i relTile2i4 = (relTile2i3 + absValue1) / 2 + new RelTile2i(num, num);
        relTile2i4 = placement.SecondRow ? relTile2i4.SetX(0) : relTile2i4.SetY(0);
        tile = (placement.AltLane ? this.m_previousBuildingPositionAlt : this.m_previousBuildingPosition) + relTile2i4;
        if (!placement.SecondRow)
        {
          if (placement.AltLane)
          {
            this.m_previousBuildingPositionAlt = tile;
            this.m_previousBuildingSizeAlt = absValue1;
          }
          else
          {
            this.m_previousBuildingPosition = tile;
            this.m_previousBuildingSize = absValue1;
          }
        }
      }
      return this.GetSurfaceTile(tile);
    }

    public Tile3i GetSurfaceTile(Tile2i tile)
    {
      return tile.ExtendHeight(this.m_terrainManager[tile].Height.TilesHeightRounded);
    }

    public void RegisterNamedEntity(EntityId entityId, string name, bool replace = false)
    {
      Assert.That<bool>(entityId.IsValid).IsTrue();
      if (!replace)
        Assert.That<Dict<string, EntityId>>(this.m_namedEntities).NotContainsKey<string, EntityId>(name);
      this.m_namedEntities[name] = entityId;
    }

    public bool TryGetNamedEntityId(string name, out EntityId id)
    {
      return this.m_namedEntities.TryGetValue(name, out id);
    }

    public bool TryGetNamedEntity<T>(string name, out T entity) where T : class, IEntity
    {
      entity = default (T);
      EntityId id;
      return this.m_namedEntities.TryGetValue(name, out id) && this.m_entitiesManager.TryGetEntity<T>(id, out entity);
    }

    public static void Serialize(ScriptedAiPlayer value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ScriptedAiPlayer>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ScriptedAiPlayer.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsDoneWithAllActions);
      writer.WriteBool(this.IsInstaBuildEnabled);
      writer.WriteBool(this.m_brokenEntitiesEarlyWarningReported);
      ScriptedAiPlayerConfig.Serialize(this.m_config, writer);
      Option<IScriptedAiPlayerAction>.Serialize(this.m_currentAction, writer);
      Option<IScriptedAiPlayerActionCore>.Serialize(this.m_currentActionCore, writer);
      writer.WriteGeneric<IElectricityManager>(this.m_electricityManager);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      writer.WriteGeneric<IInputScheduler>(this.m_inputScheduler);
      writer.WriteBool(this.m_isInitialized);
      writer.WriteBool(this.m_lowPowerEarlyWarningReported);
      MaintenanceManager.Serialize(this.m_maintenanceManager, writer);
      Dict<string, EntityId>.Serialize(this.m_namedEntities, writer);
      writer.WriteInt(this.m_nextActionIndex);
      writer.WriteBool(this.m_noRecipesEarlyWarningReported);
      Tile2i.Serialize(this.m_previousBuildingPosition, writer);
      Tile2i.Serialize(this.m_previousBuildingPositionAlt, writer);
      RelTile2i.Serialize(this.m_previousBuildingSize, writer);
      RelTile2i.Serialize(this.m_previousBuildingSizeAlt, writer);
      writer.WriteInt(this.m_recoveredVehicles);
      RefugeesManager.Serialize(this.m_refugeesManager, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
      SettlementsManager.Serialize(this.m_settlementsManager, writer);
      writer.WriteInt(this.m_skipLowPowerEarlyWarning);
      TerrainManager.Serialize(this.m_terrainManager, writer);
      UpointsManager.Serialize(this.m_upointsManager, writer);
      writer.WriteGeneric<IVehiclesManager>(this.m_vehiclesManager);
      WorkersManager.Serialize(this.m_workersManager, writer);
      writer.WriteGeneric<ISimLoopEvents>(this.SimLoopEvents);
      writer.WriteInt(this.Stage);
    }

    public static ScriptedAiPlayer Deserialize(BlobReader reader)
    {
      ScriptedAiPlayer scriptedAiPlayer;
      if (reader.TryStartClassDeserialization<ScriptedAiPlayer>(out scriptedAiPlayer))
        reader.EnqueueDataDeserialization((object) scriptedAiPlayer, ScriptedAiPlayer.s_deserializeDataDelayedAction);
      return scriptedAiPlayer;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsDoneWithAllActions = reader.ReadBool();
      this.IsInstaBuildEnabled = reader.ReadBool();
      this.m_brokenEntitiesEarlyWarningReported = reader.ReadBool();
      reader.SetField<ScriptedAiPlayer>(this, "m_config", (object) ScriptedAiPlayerConfig.Deserialize(reader));
      this.m_currentAction = Option<IScriptedAiPlayerAction>.Deserialize(reader);
      this.m_currentActionCore = Option<IScriptedAiPlayerActionCore>.Deserialize(reader);
      reader.SetField<ScriptedAiPlayer>(this, "m_electricityManager", (object) reader.ReadGenericAs<IElectricityManager>());
      reader.SetField<ScriptedAiPlayer>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<ScriptedAiPlayer>(this, "m_inputScheduler", (object) reader.ReadGenericAs<IInputScheduler>());
      this.m_isInitialized = reader.ReadBool();
      this.m_lowPowerEarlyWarningReported = reader.ReadBool();
      reader.SetField<ScriptedAiPlayer>(this, "m_maintenanceManager", (object) MaintenanceManager.Deserialize(reader));
      reader.SetField<ScriptedAiPlayer>(this, "m_namedEntities", (object) Dict<string, EntityId>.Deserialize(reader));
      this.m_nextActionIndex = reader.ReadInt();
      this.m_noRecipesEarlyWarningReported = reader.ReadBool();
      this.m_previousBuildingPosition = Tile2i.Deserialize(reader);
      this.m_previousBuildingPositionAlt = Tile2i.Deserialize(reader);
      this.m_previousBuildingSize = RelTile2i.Deserialize(reader);
      this.m_previousBuildingSizeAlt = RelTile2i.Deserialize(reader);
      this.m_recoveredVehicles = reader.ReadInt();
      reader.SetField<ScriptedAiPlayer>(this, "m_refugeesManager", (object) RefugeesManager.Deserialize(reader));
      reader.SetField<ScriptedAiPlayer>(this, "m_resolver", (object) DependencyResolver.Deserialize(reader));
      reader.SetField<ScriptedAiPlayer>(this, "m_settlementsManager", (object) SettlementsManager.Deserialize(reader));
      this.m_skipLowPowerEarlyWarning = reader.ReadInt();
      reader.SetField<ScriptedAiPlayer>(this, "m_terrainManager", (object) TerrainManager.Deserialize(reader));
      reader.SetField<ScriptedAiPlayer>(this, "m_upointsManager", (object) UpointsManager.Deserialize(reader));
      reader.SetField<ScriptedAiPlayer>(this, "m_vehiclesManager", (object) reader.ReadGenericAs<IVehiclesManager>());
      reader.SetField<ScriptedAiPlayer>(this, "m_workersManager", (object) WorkersManager.Deserialize(reader));
      reader.RegisterResolvedMember<ScriptedAiPlayer>(this, "ProtosDb", typeof (ProtosDb), true);
      reader.SetField<ScriptedAiPlayer>(this, "SimLoopEvents", (object) reader.ReadGenericAs<ISimLoopEvents>());
      this.Stage = reader.ReadInt();
    }

    static ScriptedAiPlayer()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ScriptedAiPlayer.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ScriptedAiPlayer) obj).SerializeData(writer));
      ScriptedAiPlayer.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ScriptedAiPlayer) obj).DeserializeData(reader));
    }
  }
}
