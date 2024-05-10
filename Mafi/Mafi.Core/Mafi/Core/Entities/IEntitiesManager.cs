// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.IEntitiesManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Validators;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities
{
  public interface IEntitiesManager
  {
    IEvent<IEntity> EntityAdded { get; }

    IEvent<IEntity, EntityAddReason> EntityAddedFull { get; }

    IEvent<IEntity> EntityRemoved { get; }

    IEvent<IEntity, EntityRemoveReason> EntityRemovedFull { get; }

    IEvent<IStaticEntity> StaticEntityAdded { get; }

    IEvent<IStaticEntity> StaticEntityRemoved { get; }

    /// <summary>
    /// Called just before upgrade is performed with non-upgraded entity.
    /// </summary>
    IEvent<IUpgradableEntity> OnUpgradeToBePerformed { get; }

    /// <summary>
    /// Called just after upgrade is performed with upgraded entity. Also provides the previous proto.
    /// </summary>
    IEvent<IUpgradableEntity, IEntityProto> OnUpgradeJustPerformed { get; }

    IEvent<IEntity, bool> EntityPauseStateChanged { get; }

    IEvent<IEntity, bool> EntityEnabledChanged { get; }

    IEventNonSaveable<IEntity> OnEntityVisualChanged { get; }

    IIndexable<IEntity> Entities { get; }

    Option<IEntity> GetEntity(EntityId id);

    Option<T> GetEntity<T>(EntityId id) where T : class, IEntity;

    bool TryGetEntity<T>(EntityId id, out T entity) where T : class, IEntity;

    IEnumerable<T> GetAllEntitiesOfType<T>() where T : class, IEntity;

    void RemoveAndDestroyEntityNoChecks(IEntity entity, EntityRemoveReason reasonToRemove);

    EntityValidationResult CanRemoveEntity(IEntity entity, EntityRemoveReason reasonToRemove);

    void TryUpgradeEntity(IUpgradableEntity entity);

    void InvokeOnEntityVisualChanged(IEntity entity);

    /// <summary>
    /// Will force all entities of the given type to update their properties.
    /// </summary>
    void UpdatePropertiesForAllEntities(ImmutableArray<Type> types);
  }
}
