// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.IEntitiesRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Handles rendering of some type of entities.
  /// May use low 7 bytes of <see cref="P:Mafi.Core.Entities.IRenderedEntity.RendererData" /> for bookkeeping.
  /// </summary>
  public interface IEntitiesRenderer
  {
    /// <summary>The lower the higher priority.</summary>
    int Priority { get; }

    /// <summary>Whether this renderer can render the given entity.</summary>
    bool CanRenderEntity(EntityProto proto);

    void AddEntityOnSim(IRenderedEntity entity);

    void UpdateEntityOnSim(IRenderedEntity entity);

    void RemoveEntityOnSim(IRenderedEntity entity, EntityRemoveReason reason);

    bool TryGetPickableEntityAs<T>(GameObject pickedGo, out T entity) where T : class, IRenderedEntity;

    /// <summary>
    /// Highlights the given entity and returns highlight ID. Returned ID can only use low 7 bytes of the ulong.
    /// A single entity can be independently highlighted multiple times.
    /// </summary>
    ulong AddHighlight(IRenderedEntity entity, ColorRgba color);

    /// <summary>Removes highlight based on the given ID.</summary>
    void RemoveHighlight(ulong highlightId);

    /// <summary>
    /// Handles removing entities from all renderers. Must be called before SyncUpdate, once per sync update.
    /// </summary>
    void HandlePreSyncRemove(GameTime time);

    /// <summary>
    /// Sync update called on all renderers in the order of their priority.
    /// </summary>
    void SyncUpdate(GameTime time);
  }
}
