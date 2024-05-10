// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.WorldMapManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Validators;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.World.Entities;
using Mafi.Core.World.QuickTrade;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.World
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class WorldMapManager : 
    IWorldMapManager,
    ICommandProcessor<WorldMapEntityStartRepairCmd>,
    IAction<WorldMapEntityStartRepairCmd>,
    ICommandProcessor<WorldMapSettlementAdoptPopsCmd>,
    IAction<WorldMapSettlementAdoptPopsCmd>,
    ICommandProcessor<WorldMapEntityUpgradeCmd>,
    IAction<WorldMapEntityUpgradeCmd>,
    ICommandProcessor<QuickTradeCmd>,
    IAction<QuickTradeCmd>,
    ICommandProcessor<WorldMapEntityCancelRepairCmd>,
    IAction<WorldMapEntityCancelRepairCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Event<IWorldMapEntity> m_onWorldEntityCreated;
    [DoNotSave(0, null)]
    private readonly LazyResolve<IWorldMapGenerator> m_generator;
    private readonly DependencyResolver m_resolver;
    private readonly EntitiesManager m_entitiesManager;
    private readonly Set<WorldMapMine> m_mines;
    private readonly Set<ProductProto> m_allMinableProducts;
    private readonly Lyst<QuickTradeProvider> m_allQuickTrades;
    private readonly Set<IWorldMapRepairableEntity> m_entitiesUnderConstruction;
    private WorldMap m_map;

    public static void Serialize(WorldMapManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldMapManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldMapManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Set<ProductProto>.Serialize(this.m_allMinableProducts, writer);
      Lyst<QuickTradeProvider>.Serialize(this.m_allQuickTrades, writer);
      EntitiesManager.Serialize(this.m_entitiesManager, writer);
      Set<IWorldMapRepairableEntity>.Serialize(this.m_entitiesUnderConstruction, writer);
      WorldMap.Serialize(this.m_map, writer);
      Set<WorldMapMine>.Serialize(this.m_mines, writer);
      Event<IWorldMapEntity>.Serialize(this.m_onWorldEntityCreated, writer);
      DependencyResolver.Serialize(this.m_resolver, writer);
    }

    public static WorldMapManager Deserialize(BlobReader reader)
    {
      WorldMapManager worldMapManager;
      if (reader.TryStartClassDeserialization<WorldMapManager>(out worldMapManager))
        reader.EnqueueDataDeserialization((object) worldMapManager, WorldMapManager.s_deserializeDataDelayedAction);
      return worldMapManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WorldMapManager>(this, "m_allMinableProducts", (object) Set<ProductProto>.Deserialize(reader));
      reader.SetField<WorldMapManager>(this, "m_allQuickTrades", (object) Lyst<QuickTradeProvider>.Deserialize(reader));
      reader.SetField<WorldMapManager>(this, "m_entitiesManager", (object) EntitiesManager.Deserialize(reader));
      reader.SetField<WorldMapManager>(this, "m_entitiesUnderConstruction", (object) Set<IWorldMapRepairableEntity>.Deserialize(reader));
      this.m_map = WorldMap.Deserialize(reader);
      reader.SetField<WorldMapManager>(this, "m_mines", (object) Set<WorldMapMine>.Deserialize(reader));
      reader.SetField<WorldMapManager>(this, "m_onWorldEntityCreated", (object) Event<IWorldMapEntity>.Deserialize(reader));
      reader.SetField<WorldMapManager>(this, "m_resolver", (object) DependencyResolver.Deserialize(reader));
    }

    public IEvent<IWorldMapEntity> OnWorldEntityCreated
    {
      get => (IEvent<IWorldMapEntity>) this.m_onWorldEntityCreated;
    }

    public IReadOnlySet<WorldMapMine> Mines => (IReadOnlySet<WorldMapMine>) this.m_mines;

    public IReadOnlySet<ProductProto> AllMinableProducts
    {
      get => (IReadOnlySet<ProductProto>) this.m_allMinableProducts;
    }

    public IIndexable<QuickTradeProvider> AllQuickTrades
    {
      get => (IIndexable<QuickTradeProvider>) this.m_allQuickTrades;
    }

    public IReadOnlySet<IWorldMapRepairableEntity> EntitiesUnderConstruction
    {
      get => (IReadOnlySet<IWorldMapRepairableEntity>) this.m_entitiesUnderConstruction;
    }

    public WorldMap Map
    {
      get
      {
        return this.m_map != null ? this.m_map : throw new Exception("Getting map which was not generated yet.");
      }
    }

    public event Action<WorldMap> MapReplaced;

    public event Action<WorldMapLocation> LocationAdded;

    public event Action<WorldMapLocation> LocationRemoved;

    public event Action<WorldMapLocation> LocationChanged;

    public event Action<WorldMapLocation> LocationExplored;

    public event Action<WorldMapConnection> ConnectionAdded;

    public event Action<WorldMapConnection> ConnectionRemoved;

    public WorldMapManager(
      IGameLoopEvents gameEvents,
      LazyResolve<IWorldMapGenerator> generator,
      DependencyResolver resolver,
      EntitiesManager entitiesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_onWorldEntityCreated = new Event<IWorldMapEntity>();
      this.m_mines = new Set<WorldMapMine>();
      this.m_allMinableProducts = new Set<ProductProto>();
      this.m_allQuickTrades = new Lyst<QuickTradeProvider>();
      this.m_entitiesUnderConstruction = new Set<IWorldMapRepairableEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_generator = generator;
      this.m_resolver = resolver;
      this.m_entitiesManager = entitiesManager;
      gameEvents.RegisterNewGameCreated((object) this, this.newGameCreated());
    }

    private IEnumerator<string> newGameCreated()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator<string>) new WorldMapManager.\u003CnewGameCreated\u003Ed__49(0)
      {
        \u003C\u003E4__this = this
      };
    }

    public void SetMap(WorldMap newMap)
    {
      if (this.m_map == newMap)
        return;
      this.m_map?.Deactivate();
      this.m_map = newMap.CheckNotNull<WorldMap>();
      this.m_map.Initialize(this);
      Action<WorldMap> mapReplaced = this.MapReplaced;
      if (mapReplaced == null)
        return;
      mapReplaced(newMap);
    }

    internal void Cheat_RevealAndResolveAllEntities()
    {
      foreach (WorldMapLocation location in (IEnumerable<WorldMapLocation>) this.m_map.Locations)
      {
        location.IsScannedByRadar = true;
        location.IsEnemyKnown = true;
        if (location.Enemy.HasValue)
          location.MarkEnemyAsDefeated();
        if (location.State != WorldMapLocationState.Explored)
          this.OnLocationExplored(location);
      }
    }

    internal void OnLocationExplored(WorldMapLocation location)
    {
      location.SetState(WorldMapLocationState.Explored);
      if (location.EntityProto.HasValue)
        this.createEntityForLocation(location.EntityProto.Value, location);
      Action<WorldMapLocation> locationExplored = this.LocationExplored;
      if (locationExplored == null)
        return;
      locationExplored(location);
    }

    private void createEntityForLocation(WorldMapEntityProto proto, WorldMapLocation location)
    {
      Option<WorldMapEntity> option = this.m_resolver.TryInvokeFactoryHierarchy<WorldMapEntity>((object) proto, (object) location);
      if (!option.HasValue)
      {
        Log.Error(string.Format("Failed to instantiate entity '{0}'.", (object) proto.Id));
      }
      else
      {
        EntityValidationResult validationResult = this.m_entitiesManager.TryAddEntity((IEntity) option.Value);
        if (validationResult.IsSuccess)
        {
          location.SetEntity((IWorldMapEntity) option.Value);
          if (option.Value is WorldMapMine worldMapMine)
          {
            this.m_mines.AddAndAssertNew(worldMapMine);
            this.m_allMinableProducts.Add(worldMapMine.Product);
          }
          if (option.Value is WorldMapVillage worldMapVillage)
            this.m_allQuickTrades.AddRange(worldMapVillage.QuickTrades);
          this.m_onWorldEntityCreated.Invoke((IWorldMapEntity) option.Value);
        }
        else
        {
          ((IEntityFriend) option.Value).Destroy();
          Log.Error(string.Format("Failed to add entity '{0}' to the world. Error: '{1}'", (object) proto.Id, (object) validationResult.ErrorMessage));
        }
      }
    }

    internal void OnLocationAdded(WorldMapLocation location)
    {
      Action<WorldMapLocation> locationAdded = this.LocationAdded;
      if (locationAdded == null)
        return;
      locationAdded(location);
    }

    internal void OnLocationRemoved(WorldMapLocation location)
    {
      Action<WorldMapLocation> locationRemoved = this.LocationRemoved;
      if (locationRemoved == null)
        return;
      locationRemoved(location);
    }

    internal void OnConnectionAdded(WorldMapConnection connection)
    {
      Action<WorldMapConnection> connectionAdded = this.ConnectionAdded;
      if (connectionAdded == null)
        return;
      connectionAdded(connection);
    }

    internal void OnConnectionRemoved(WorldMapConnection connection)
    {
      Action<WorldMapConnection> connectionRemoved = this.ConnectionRemoved;
      if (connectionRemoved == null)
        return;
      connectionRemoved(connection);
    }

    internal void OnLocationChanged(WorldMapLocation location)
    {
      Action<WorldMapLocation> locationChanged = this.LocationChanged;
      if (locationChanged == null)
        return;
      locationChanged(location);
    }

    public void ReportEntityConstructionStarted(IWorldMapRepairableEntity entity)
    {
      Assert.That<bool>(entity.IsUnderConstruction).IsTrue();
      this.m_entitiesUnderConstruction.AddAndAssertNew(entity);
      entity.OnConstructionDone.Add<WorldMapManager>(this, new Action<IWorldMapRepairableEntity>(this.onEntityConstructed));
    }

    public void ReportEntityConstructionCanceled(IWorldMapRepairableEntity entity)
    {
      this.m_entitiesUnderConstruction.RemoveAndAssert(entity);
      entity.OnConstructionDone.Remove<WorldMapManager>(this, new Action<IWorldMapRepairableEntity>(this.onEntityConstructed));
    }

    private void onEntityConstructed(IWorldMapRepairableEntity entity)
    {
      this.m_entitiesUnderConstruction.RemoveAndAssert(entity);
      entity.OnConstructionDone.Remove<WorldMapManager>(this, new Action<IWorldMapRepairableEntity>(this.onEntityConstructed));
    }

    public void Invoke(WorldMapEntityStartRepairCmd cmd)
    {
      WorldMapEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<WorldMapEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(string.Format("World map entity with id '{0}' not found!", (object) cmd.EntityId));
      else if (entity is WorldMapRepairableEntity repairableEntity)
      {
        if (repairableEntity.IsRepaired)
        {
          cmd.SetResultError(string.Format("Entity with id '{0}' is already repaired!", (object) cmd.EntityId));
        }
        else
        {
          repairableEntity.StartRepair();
          cmd.SetResultSuccess();
        }
      }
      else
        cmd.SetResultError(string.Format("World map entity with id '{0}' is not repairable!", (object) cmd.EntityId));
    }

    public void Invoke(WorldMapEntityCancelRepairCmd cmd)
    {
      WorldMapEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<WorldMapEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(string.Format("World map entity with id '{0}' not found!", (object) cmd.EntityId));
      else if (entity is WorldMapRepairableEntity repairableEntity)
      {
        repairableEntity.CancelRepairOrUpgrade();
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(string.Format("World map entity with id '{0}' is not repairable!", (object) cmd.EntityId));
    }

    public void Invoke(WorldMapEntityUpgradeCmd cmd)
    {
      WorldMapEntity entity;
      if (!this.m_entitiesManager.TryGetEntity<WorldMapEntity>(cmd.EntityId, out entity))
        cmd.SetResultError(string.Format("World map entity with id '{0}' not found!", (object) cmd.EntityId));
      else if (entity is IUpgradableWorldEntity upgradableWorldEntity)
      {
        upgradableWorldEntity.StartUpgrade();
        cmd.SetResultSuccess();
      }
      else
        cmd.SetResultError(string.Format("World map entity with id '{0}' is not upgradable!", (object) cmd.EntityId));
    }

    public void Invoke(WorldMapSettlementAdoptPopsCmd cmd)
    {
      WorldMapVillage entity;
      if (!this.m_entitiesManager.TryGetEntity<WorldMapVillage>(cmd.EntityId, out entity))
      {
        cmd.SetResultError(string.Format("World map entity with id '{0}' not found!", (object) cmd.EntityId));
      }
      else
      {
        entity.AdoptPops(cmd.PopsCount);
        cmd.SetResultSuccess();
      }
    }

    public void RequestEntityRemoval(IWorldMapEntity entity)
    {
      Assert.That<IWorldMapEntity>(entity.Location.Entity.ValueOrNull).IsEqualTo<IWorldMapEntity>(entity);
      Assert.That<bool>(entity is WorldMapMine).IsFalse();
      EntityValidationResult validationResult = this.m_entitiesManager.TryRemoveAndDestroyEntity((IEntity) entity);
      if (validationResult.IsError)
        Log.Error(validationResult.ErrorMessage);
      else
        entity.Location.RemoveCurrentEntity();
    }

    public void Invoke(QuickTradeCmd cmd)
    {
      QuickTradeProvider quickTradeProvider = this.m_allQuickTrades.FirstOrDefault<QuickTradeProvider>((Predicate<QuickTradeProvider>) (x => x.Prototype.Id == cmd.TradeId));
      if (quickTradeProvider == null)
      {
        cmd.SetResultError(string.Format("Could not find trade with id '{0}'!", (object) cmd.TradeId));
      }
      else
      {
        quickTradeProvider.QuickBuy();
        cmd.SetResultSuccess();
      }
    }

    public void AddTradeAfterLoad(QuickTradeProvider trade)
    {
      this.m_allQuickTrades.AddAssertNew(trade);
    }

    static WorldMapManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldMapManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldMapManager) obj).SerializeData(writer));
      WorldMapManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldMapManager) obj).DeserializeData(reader));
    }
  }
}
