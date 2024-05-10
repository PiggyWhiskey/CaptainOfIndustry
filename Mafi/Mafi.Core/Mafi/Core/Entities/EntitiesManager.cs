// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.EntitiesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public sealed class EntitiesManager : IEntitiesManager, IEntitiesManagerInternal
  {
    private readonly Event<IEntity> m_entityAdded;
    private readonly Event<IEntity, EntityAddReason> m_entityAddedFull;
    private readonly Event<IStaticEntity> m_staticEntityAdded;
    private readonly Event<IEntity> m_entityRemoved;
    private readonly Event<IEntity, EntityRemoveReason> m_entityRemovedFull;
    private readonly Event<IStaticEntity> m_staticEntityRemoved;
    private readonly Event<IEntity, bool> m_staticEntityPauseStateChanged;
    private readonly Event<IEntity, bool> m_entityEnabledChanged;
    private readonly Event<IUpgradableEntity> m_onUpgradeToBePerformed;
    private readonly Event<IUpgradableEntity, IEntityProto> m_onUpgradeJustPerformed;
    [DoNotSaveCreateNewOnLoad("new EventNonSaveable<IEntity>(ThreadType.Sim)", 0)]
    private readonly EventNonSaveable<IEntity> m_onEntityVisualChanged;
    private readonly LystMutableDuringIter<IEntity> m_entitiesLinear;
    [DoNotSave(0, null)]
    private Set<IEntity> m_entities;
    [DoNotSave(0, null)]
    private Dict<EntityId, IEntity> m_entitiesById;
    [DoNotSave(0, null)]
    private LystMutableDuringIter<IEntityWithSimUpdate> m_entitiesWithSimUpdate;
    [DoNotSaveCreateNewOnLoad("new Lyst<IEntityPreAddValidator>()", 0)]
    private readonly Lyst<IEntityPreAddValidator> m_preAddValidatorsCache;
    private readonly EntityValidators m_validators;
    private readonly LayoutEntityAddRequestFactory m_layoutEntityAddRequestFactory;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    /// <summary>
    /// Invoked when an entity gets added to the game. If you need also a reason why, use <see cref="P:Mafi.Core.Entities.EntitiesManager.EntityAddedFull" /> instead.
    /// </summary>
    public IEvent<IEntity> EntityAdded => (IEvent<IEntity>) this.m_entityAdded;

    /// <summary>Invoked when an entity gets added to the game.</summary>
    public IEvent<IEntity, EntityAddReason> EntityAddedFull
    {
      get => (IEvent<IEntity, EntityAddReason>) this.m_entityAddedFull;
    }

    public IEvent<IStaticEntity> StaticEntityAdded
    {
      get => (IEvent<IStaticEntity>) this.m_staticEntityAdded;
    }

    /// <summary>
    /// Invoked when an entity gets removed from the game. If you need also a reason why, use <see cref="P:Mafi.Core.Entities.EntitiesManager.EntityRemovedFull" /> instead.
    /// </summary>
    public IEvent<IEntity> EntityRemoved => (IEvent<IEntity>) this.m_entityRemoved;

    /// <summary>Invoked when an entity gets removed from the game.</summary>
    public IEvent<IEntity, EntityRemoveReason> EntityRemovedFull
    {
      get => (IEvent<IEntity, EntityRemoveReason>) this.m_entityRemovedFull;
    }

    public IEvent<IStaticEntity> StaticEntityRemoved
    {
      get => (IEvent<IStaticEntity>) this.m_staticEntityRemoved;
    }

    public IEvent<IEntity, bool> EntityPauseStateChanged
    {
      get => (IEvent<IEntity, bool>) this.m_staticEntityPauseStateChanged;
    }

    public IEvent<IEntity, bool> EntityEnabledChanged
    {
      get => (IEvent<IEntity, bool>) this.m_entityEnabledChanged;
    }

    public IEvent<IUpgradableEntity> OnUpgradeToBePerformed
    {
      get => (IEvent<IUpgradableEntity>) this.m_onUpgradeToBePerformed;
    }

    public IEvent<IUpgradableEntity, IEntityProto> OnUpgradeJustPerformed
    {
      get => (IEvent<IUpgradableEntity, IEntityProto>) this.m_onUpgradeJustPerformed;
    }

    public IEventNonSaveable<IEntity> OnEntityVisualChanged
    {
      get => (IEventNonSaveable<IEntity>) this.m_onEntityVisualChanged;
    }

    public IIndexable<IEntity> Entities => (IIndexable<IEntity>) this.m_entitiesLinear;

    /// <summary>Total number of managed entities.</summary>
    public int EntitiesCount => this.m_entities.Count;

    public EntitiesManager(
      EntityValidators validators,
      ISimLoopEvents simLoopEvents,
      LayoutEntityAddRequestFactory layoutEntityAddRequestFactory)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_entityAdded = new Event<IEntity>();
      this.m_entityAddedFull = new Event<IEntity, EntityAddReason>();
      this.m_staticEntityAdded = new Event<IStaticEntity>();
      this.m_entityRemoved = new Event<IEntity>();
      this.m_entityRemovedFull = new Event<IEntity, EntityRemoveReason>();
      this.m_staticEntityRemoved = new Event<IStaticEntity>();
      this.m_staticEntityPauseStateChanged = new Event<IEntity, bool>();
      this.m_entityEnabledChanged = new Event<IEntity, bool>();
      this.m_onUpgradeToBePerformed = new Event<IUpgradableEntity>();
      this.m_onUpgradeJustPerformed = new Event<IUpgradableEntity, IEntityProto>();
      this.m_onEntityVisualChanged = new EventNonSaveable<IEntity>(ThreadType.Sim);
      this.m_entitiesLinear = new LystMutableDuringIter<IEntity>();
      this.m_entities = new Set<IEntity>();
      this.m_entitiesById = new Dict<EntityId, IEntity>();
      this.m_entitiesWithSimUpdate = new LystMutableDuringIter<IEntityWithSimUpdate>();
      this.m_preAddValidatorsCache = new Lyst<IEntityPreAddValidator>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_validators = validators;
      this.m_layoutEntityAddRequestFactory = layoutEntityAddRequestFactory;
      simLoopEvents.Update.Add<EntitiesManager>(this, new Action(this.simUpdate));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf()
    {
      this.m_entities = new Set<IEntity>();
      this.m_entitiesById = new Dict<EntityId, IEntity>();
      this.m_entitiesWithSimUpdate = new LystMutableDuringIter<IEntityWithSimUpdate>();
      foreach (IEntity entity in this.m_entitiesLinear)
      {
        this.m_entities.Add(entity);
        this.m_entitiesById.Add(entity.Id, entity);
        if (entity is IEntityWithSimUpdate entityWithSimUpdate)
          this.m_entitiesWithSimUpdate.Add(entityWithSimUpdate);
      }
    }

    [OnlyForSaveCompatibility(null)]
    [InitAfterLoad(InitPriority.Lowest)]
    private void repairStates(int saveVersion)
    {
      foreach (IEntity entity1 in this.m_entitiesLinear)
      {
        if (entity1 is StaticEntity entity2 && (entity2.ConstructionState == ConstructionState.InDeconstruction || entity2.ConstructionState == ConstructionState.InConstruction) && entity2.ConstructionProgress.IsNone)
        {
          Log.Error(string.Format("Removing entity {0} with invalid construction state, save version: {1}", (object) entity2.GetTitle(), (object) saveVersion));
          this.RemoveAndDestroyEntityNoChecks((IEntity) entity2, EntityRemoveReason.Remove);
        }
      }
    }

    private void simUpdate()
    {
      foreach (IEntityWithSimUpdate entityWithSimUpdate in this.m_entitiesWithSimUpdate)
      {
        if (entityWithSimUpdate.IsDestroyed)
        {
          Log.Error(string.Format("SimStep on destroyed entity '{0}'", (object) entityWithSimUpdate));
          this.RemoveAndDestroyEntityNoChecks((IEntity) entityWithSimUpdate, EntityRemoveReason.Remove);
        }
        else
        {
          try
          {
            entityWithSimUpdate.SimUpdate();
          }
          catch (Exception ex)
          {
            Log.Exception(ex, "Exception thrown in entity sim update.");
          }
        }
      }
    }

    public bool HasEntity(IEntity entity) => this.m_entities.Contains(entity);

    public Option<IEntity> GetEntity(EntityId id) => this.m_entitiesById.Get<EntityId, IEntity>(id);

    public Option<T> GetEntity<T>(EntityId id) where T : class, IEntity
    {
      IEntity entity;
      if (!this.m_entitiesById.TryGetValue(id, out entity))
        return Option<T>.None;
      Assert.That<bool>(entity is T).IsTrue<IEntity, Type, Type>("Trying to get entity '{0}' as '{1}' but it is '{2}'.", entity, typeof (T), entity.GetType());
      return (Option<T>) (entity as T);
    }

    public bool TryGetEntity<T>(EntityId id, out T entity) where T : class, IEntity
    {
      IEntity entity1;
      if (this.m_entitiesById.TryGetValue(id, out entity1))
      {
        entity = entity1 as T;
        return (object) entity != null;
      }
      entity = default (T);
      return false;
    }

    public IEnumerable<T> GetAllEntitiesOfType<T>() where T : class, IEntity
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<T>) new EntitiesManager.\u003CGetAllEntitiesOfType\u003Ed__52<T>(-2)
      {
        \u003C\u003E4__this = this
      };
    }

    public IEnumerable<T> GetAllEntitiesOfType<T>(Predicate<T> predicate) where T : class, IEntity
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<T>) new EntitiesManager.\u003CGetAllEntitiesOfType\u003Ed__53<T>(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__predicate = predicate
      };
    }

    public Option<T> GetFirstEntityOfType<T>() where T : class, IEntity
    {
      foreach (IEntity entity in this.m_entitiesLinear)
      {
        if (entity is T firstEntityOfType)
          return (Option<T>) firstEntityOfType;
      }
      return Option<T>.None;
    }

    public Option<T> GetFirstEntityOfType<T>(Predicate<T> predicate) where T : class, IEntity
    {
      foreach (IEntity entity in this.m_entitiesLinear)
      {
        if (entity is T firstEntityOfType && predicate(firstEntityOfType))
          return (Option<T>) firstEntityOfType;
      }
      return Option<T>.None;
    }

    public int GetCountOf<T>(Predicate<T> predicate) where T : class, IEntity
    {
      int countOf = 0;
      foreach (IEntity entity in this.m_entitiesLinear)
      {
        if (entity is T obj && predicate(obj))
          ++countOf;
      }
      return countOf;
    }

    public Option<T> GetEntityOrLog<T>(EntityId id) where T : class, IEntity
    {
      IEntity entity;
      if (!this.m_entitiesById.TryGetValue(id, out entity))
        Log.Error(string.Format("Entity with '{0}' was not found.", (object) id));
      if (entity is T entityOrLog)
        return (Option<T>) entityOrLog;
      Log.Error(string.Format("Entity '{0}' was found but couldn't be cast to {1}, ", (object) id, (object) typeof (T).Name) + "its real type is " + entity?.GetType().Name + ".");
      return Option<T>.None;
    }

    public Option<T> FindEntity<T>(Func<T, bool> predicate) where T : class, IEntity
    {
      return (Option<T>) (T) this.m_entitiesLinear.FirstOrDefault<IEntity>((Predicate<IEntity>) (x => x is T obj && predicate(obj)));
    }

    public EntityValidationResult CanAdd(
      IEntityAddRequest addRequest,
      bool forceRunAllValidators = false,
      Lyst<IEntityPreAddValidator> preAddValidators = null)
    {
      return this.m_validators.CallAllAddValidatorsFor(addRequest, forceRunAllValidators, preAddValidators);
    }

    public EntityValidationResult TryAddEntity(IEntity entity, EntityAddReason reasonToAdd = EntityAddReason.New)
    {
      if (this.m_entities.Contains(entity))
      {
        EntityValidationResult error = EntityValidationResult.CreateError(string.Format("Entity {0} already added.", (object) entity));
        if (DebugGameRendererConfig.SaveLayoutEntityFailedToBuild)
          drawError(error);
        return error;
      }
      this.m_preAddValidatorsCache.Clear();
      if (entity is ILayoutEntity layoutEntity)
      {
        EntityValidationResult error = this.CanAdd((IEntityAddRequest) this.m_layoutEntityAddRequestFactory.CreateRequestFor<LayoutEntityProto>(layoutEntity.Prototype, new EntityAddRequestData(layoutEntity.Transform)), preAddValidators: this.m_preAddValidatorsCache);
        if (error.IsError)
        {
          if (DebugGameRendererConfig.SaveLayoutEntityFailedToBuild)
            drawError(error);
          return error;
        }
      }
      else if (entity is IEntityWithAdditionRequest withAdditionRequest)
      {
        EntityValidationResult error = this.CanAdd(withAdditionRequest.GetAddRequest(reasonToAdd), preAddValidators: this.m_preAddValidatorsCache);
        if (error.IsError)
        {
          if (DebugGameRendererConfig.SaveLayoutEntityFailedToBuild)
            drawError(error);
          return error;
        }
      }
      foreach (IEntityPreAddValidator entityPreAddValidator in this.m_preAddValidatorsCache)
        entityPreAddValidator.PrepareForAdd();
      this.m_preAddValidatorsCache.Clear();
      this.addEntityInternal(entity, reasonToAdd);
      return EntityValidationResult.Success;

      void drawError(EntityValidationResult error)
      {
        if (!DebugGameRenderer.IsEnabled || !(entity is IStaticEntity entity1))
          return;
        DebugGameRenderer.DrawGameImage(entity1.CenterTile.Xy - 20, new RelTile2i(40, 40)).DrawStaticEntity(entity1, ColorRgba.Orange).DrawString(entity1.Position2f, error.ErrorMessage, ColorRgba.Red, centered: true).SaveMapAsTga("StaticEntityFailedToBuild");
      }
    }

    public void AddEntityNoChecks(IEntity entity, EntityAddReason reasonToAdd = EntityAddReason.New)
    {
      if (this.m_entities.Contains(entity))
        Assert.Fail(string.Format("Trying to add already added entity '{0}'.", (object) entity));
      else
        this.addEntityInternal(entity, reasonToAdd);
    }

    private void addEntityInternal(IEntity entity, EntityAddReason reasonToAdd)
    {
      if (!this.m_entities.Add(entity))
      {
        Log.Error(string.Format("Entity '{0}' is already added.", (object) entity));
      }
      else
      {
        this.m_entitiesLinear.Add(entity);
        this.m_entitiesById.Add(entity.Id, entity);
        if (entity is IEntityWithSimUpdate entityWithSimUpdate)
          this.m_entitiesWithSimUpdate.Add(entityWithSimUpdate);
        this.m_entityAdded.Invoke(entity);
        this.m_entityAddedFull.Invoke(entity, reasonToAdd);
        if (entity is IStaticEntity staticEntity)
          this.m_staticEntityAdded.Invoke(staticEntity);
        ((IEntityFriend) entity).AddedToWorld(reasonToAdd);
      }
    }

    public EntityValidationResult CanRemoveEntity(IEntity entity, EntityRemoveReason reasonToRemove)
    {
      return !this.m_entities.Contains(entity) ? EntityValidationResult.CreateError(string.Format("Entity {0} not found.", (object) entity)) : this.m_validators.CallRemoveValidatorsFor(entity, reasonToRemove);
    }

    /// <summary>
    /// Removes given entity without destroying it. Returns whether the remove operation was successful.
    /// </summary>
    public EntityValidationResult TryRemoveEntity(IEntity entity, EntityRemoveReason reasonToRemove = EntityRemoveReason.Remove)
    {
      EntityValidationResult validationResult = this.CanRemoveEntity(entity, reasonToRemove);
      if (validationResult.IsError)
        return validationResult;
      this.removeEntityInternal(entity, reasonToRemove);
      return EntityValidationResult.Success;
    }

    /// <summary>
    /// Removes given entity without destroying it. This skips any removal checks.
    /// </summary>
    public void RemoveEntityNoChecks(IEntity entity, EntityRemoveReason reasonToRemove = EntityRemoveReason.Remove)
    {
      this.removeEntityInternal(entity, reasonToRemove);
    }

    private void removeEntityInternal(IEntity entity, EntityRemoveReason reasonToRemove)
    {
      if (!this.m_entities.Remove(entity))
      {
        Log.Error(string.Format("Failed to remove entity {0}.", (object) entity));
      }
      else
      {
        this.m_entitiesLinear.RemoveAndAssert(entity);
        this.m_entitiesById.RemoveAndAssert(entity.Id);
        if (entity is IEntityWithSimUpdate entityWithSimUpdate)
          this.m_entitiesWithSimUpdate.RemoveAndAssert(entityWithSimUpdate);
        this.m_entityRemoved.Invoke(entity);
        this.m_entityRemovedFull.Invoke(entity, reasonToRemove);
        if (!(entity is IStaticEntity staticEntity))
          return;
        this.m_staticEntityRemoved.Invoke(staticEntity);
      }
    }

    /// <summary>
    /// Removes given entity from this manager and calls <see cref="M:Mafi.Core.Entities.Entity.OnDestroy" /> on it.
    /// </summary>
    public void RemoveAndDestroyEntityNoChecks(IEntity entity, EntityRemoveReason reasonToRemove)
    {
      if (entity.IsDestroyed)
      {
        Log.Error(string.Format("Removing already destroyed entity '{0}'.", (object) entity));
        this.removeEntityInternal(entity, reasonToRemove);
      }
      else
      {
        this.removeEntityInternal(entity, reasonToRemove);
        ((IEntityFriend) entity).Destroy();
      }
    }

    public void NotifyEntityPauseStateChanged(IEntity entity, bool pauseState)
    {
      this.m_staticEntityPauseStateChanged.Invoke(entity, pauseState);
    }

    public void NotifyEntityEnabledChanged(IEntity entity, bool isEnabled)
    {
      this.m_entityEnabledChanged.Invoke(entity, isEnabled);
    }

    /// <summary>
    /// Tries to remove entity without validation checks and calls <see cref="M:Mafi.Core.Entities.Entity.OnDestroy" /> on it. It is valid
    /// to call this method with a non-existing entity. In that case nothing happens
    /// </summary>
    public bool TryRemoveAndDestroyEntityNoChecks(IEntity entity, EntityRemoveReason reasonToRemove = EntityRemoveReason.Remove)
    {
      if (!this.m_entities.Contains(entity))
        return false;
      this.removeEntityInternal(entity, reasonToRemove);
      ((IEntityFriend) entity).Destroy();
      return true;
    }

    /// <summary>
    /// Tries to removes given entity from this manager and if successful calls <see cref="M:Mafi.Core.Entities.Entity.OnDestroy" /> on it.
    /// Returns whether the entity was removed successfully.
    /// </summary>
    public EntityValidationResult TryRemoveAndDestroyEntity(
      IEntity entity,
      EntityRemoveReason reasonToRemove = EntityRemoveReason.Remove)
    {
      EntityValidationResult validationResult = this.TryRemoveEntity(entity, reasonToRemove);
      if (validationResult.IsError)
        return validationResult;
      ((IEntityFriend) entity).Destroy();
      return EntityValidationResult.Success;
    }

    public EntityValidationResult TryCutAndDestroy(
      IStaticEntity entity,
      EntityRemoveReason reasonToRemove = EntityRemoveReason.Remove)
    {
      EntityValidationResult validationResult = this.CanCutEntity(entity);
      if (validationResult.IsError)
        return validationResult;
      this.RemoveAndDestroyEntityNoChecks((IEntity) entity, reasonToRemove);
      return EntityValidationResult.Success;
    }

    /// <summary>
    /// Returns success if the entity is under construction but not started
    /// and can be removed.
    /// </summary>
    public EntityValidationResult CanCutEntity(IStaticEntity entity)
    {
      bool flag;
      switch (entity.ConstructionState)
      {
        case ConstructionState.NotInitialized:
        case ConstructionState.NotStarted:
        case ConstructionState.InConstruction:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (!flag)
        return EntityValidationResult.CreateError("Already under construction");
      return entity.ConstructionProgress.HasValue && (entity.ConstructionProgress.Value.IsDeconstruction || entity.ConstructionProgress.Value.CurrentSteps > 0 || entity.ConstructionProgress.Value.GetAssetValueOfBuffers().IsNotEmpty) ? EntityValidationResult.CreateError("Already under construction") : this.CanRemoveEntity((IEntity) entity, EntityRemoveReason.Remove);
    }

    /// <summary>
    /// This should be called once upgrade was already verified and was paid for.
    /// </summary>
    public void TryUpgradeEntity(IUpgradableEntity entity)
    {
      this.m_onUpgradeToBePerformed.Invoke(entity);
      IEntityProto prototype1 = (IEntityProto) entity.Prototype;
      entity.Upgrader.Upgrade();
      IEntityProto prototype2 = (IEntityProto) entity.Prototype;
      ((IEntityFriend) entity).NotifyAfterUpgrade(prototype1, prototype2);
      this.m_onUpgradeJustPerformed.Invoke(entity, prototype1);
    }

    public void InvokeOnEntityVisualChanged(IEntity entity)
    {
      this.m_onEntityVisualChanged.Invoke(entity);
    }

    public void UpdatePropertiesForAllEntities(ImmutableArray<Type> types)
    {
      foreach (IEntity entity in this.m_entitiesLinear)
      {
        foreach (Type type in types)
        {
          if (entity.GetType().IsAssignableTo(type))
          {
            entity.UpdateProperties();
            break;
          }
        }
      }
    }

    public static void Serialize(EntitiesManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<EntitiesManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, EntitiesManager.s_serializeDataDelayedAction);
    }

    private void SerializeData(BlobWriter writer)
    {
      LystMutableDuringIter<IEntity>.Serialize(this.m_entitiesLinear, writer);
      Event<IEntity>.Serialize(this.m_entityAdded, writer);
      Event<IEntity, EntityAddReason>.Serialize(this.m_entityAddedFull, writer);
      Event<IEntity, bool>.Serialize(this.m_entityEnabledChanged, writer);
      Event<IEntity>.Serialize(this.m_entityRemoved, writer);
      Event<IEntity, EntityRemoveReason>.Serialize(this.m_entityRemovedFull, writer);
      Event<IUpgradableEntity, IEntityProto>.Serialize(this.m_onUpgradeJustPerformed, writer);
      Event<IUpgradableEntity>.Serialize(this.m_onUpgradeToBePerformed, writer);
      Event<IStaticEntity>.Serialize(this.m_staticEntityAdded, writer);
      Event<IEntity, bool>.Serialize(this.m_staticEntityPauseStateChanged, writer);
      Event<IStaticEntity>.Serialize(this.m_staticEntityRemoved, writer);
    }

    public static EntitiesManager Deserialize(BlobReader reader)
    {
      EntitiesManager entitiesManager;
      if (reader.TryStartClassDeserialization<EntitiesManager>(out entitiesManager))
        reader.EnqueueDataDeserialization((object) entitiesManager, EntitiesManager.s_deserializeDataDelayedAction);
      return entitiesManager;
    }

    private void DeserializeData(BlobReader reader)
    {
      reader.SetField<EntitiesManager>(this, "m_entitiesLinear", (object) LystMutableDuringIter<IEntity>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_entityAdded", (object) Event<IEntity>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_entityAddedFull", (object) Event<IEntity, EntityAddReason>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_entityEnabledChanged", (object) Event<IEntity, bool>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_entityRemoved", (object) Event<IEntity>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_entityRemovedFull", (object) Event<IEntity, EntityRemoveReason>.Deserialize(reader));
      reader.RegisterResolvedMember<EntitiesManager>(this, "m_layoutEntityAddRequestFactory", typeof (LayoutEntityAddRequestFactory), true);
      reader.SetField<EntitiesManager>(this, "m_onEntityVisualChanged", (object) new EventNonSaveable<IEntity>(ThreadType.Sim));
      reader.SetField<EntitiesManager>(this, "m_onUpgradeJustPerformed", (object) Event<IUpgradableEntity, IEntityProto>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_onUpgradeToBePerformed", (object) Event<IUpgradableEntity>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_preAddValidatorsCache", (object) new Lyst<IEntityPreAddValidator>());
      reader.SetField<EntitiesManager>(this, "m_staticEntityAdded", (object) Event<IStaticEntity>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_staticEntityPauseStateChanged", (object) Event<IEntity, bool>.Deserialize(reader));
      reader.SetField<EntitiesManager>(this, "m_staticEntityRemoved", (object) Event<IStaticEntity>.Deserialize(reader));
      reader.RegisterResolvedMember<EntitiesManager>(this, "m_validators", typeof (EntityValidators), true);
      reader.RegisterInitAfterLoad<EntitiesManager>(this, "initSelf", InitPriority.Normal);
      reader.RegisterInitAfterLoad<EntitiesManager>(this, "repairStates", InitPriority.Lowest);
    }

    static EntitiesManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntitiesManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((EntitiesManager) obj).SerializeData(writer));
      EntitiesManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((EntitiesManager) obj).DeserializeData(reader));
    }
  }
}
