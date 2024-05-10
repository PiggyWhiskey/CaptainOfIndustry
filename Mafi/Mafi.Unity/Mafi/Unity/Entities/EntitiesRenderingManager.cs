// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.EntitiesRenderingManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.GameLoop;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>Manages delegation of different entities renderers.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class EntitiesRenderingManager
  {
    public const int RENDERER_DATA_SHIFT = 56;
    public const ulong RENDER_DATA_MASK = 18374686479671623680;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly ConstructionManager m_constructionManager;
    private Option<Lyst<IEntitiesRenderer>> m_newRenderers;
    private IEntitiesRenderer[] m_renderers;

    public EntitiesRenderingManager(
      IEntitiesManager entitiesManager,
      IGameLoopEvents gameLoopEvents,
      ConstructionManager constructionManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_renderers = Array.Empty<IEntitiesRenderer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_entitiesManager = entitiesManager;
      this.m_constructionManager = constructionManager;
      this.m_newRenderers = (Option<Lyst<IEntitiesRenderer>>) new Lyst<IEntitiesRenderer>();
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      gameLoopEvents.SyncUpdate.AddNonSaveable<EntitiesRenderingManager>(this, new Action<GameTime>(this.syncUpdate));
    }

    public void RegisterRenderer(IEntitiesRenderer renderer)
    {
      if (this.m_newRenderers.IsNone)
        Log.Error("Failed to add renderer '" + renderer.GetType().Name + "'. Renderers can be added only before init phase.");
      else if (this.m_newRenderers.Value.Count >= (int) byte.MaxValue)
      {
        Log.Error("Only 255 renderers are supported.");
      }
      else
      {
        if (this.m_newRenderers.Value.Count >= 240)
          Log.Error("Running our of renderers, only 255 are supported.");
        this.m_newRenderers.Value.AddAssertNew(renderer);
      }
    }

    private byte getEntityRendererIndex(IRenderedEntity entity)
    {
      int entityRendererIndex = entity.Prototype.Graphics.RendererIndex;
      if (entityRendererIndex <= 0)
        entityRendererIndex = this.findAndSetEntityRendererIndex(entity);
      return (byte) entityRendererIndex;
    }

    private int findAndSetEntityRendererIndex(IRenderedEntity entity)
    {
      int entityRendererIndex = 0;
      for (int index = 1; index < this.m_renderers.Length; ++index)
      {
        if (this.m_renderers[index].CanRenderEntity(entity.Prototype))
        {
          entityRendererIndex = index;
          break;
        }
      }
      if (entityRendererIndex <= 0)
      {
        Log.Error(string.Format("Failed to find entity renderer for entity '{0}'.", (object) entity));
        entityRendererIndex = this.m_renderers.Length - 1;
      }
      entity.Prototype.Graphics.RendererIndex = entityRendererIndex;
      return entityRendererIndex;
    }

    private void initState()
    {
      if (this.m_newRenderers.HasValue)
      {
        this.m_newRenderers.Value.Sort((Comparison<IEntitiesRenderer>) ((x, y) => x.Priority.CompareTo(y.Priority)));
        this.m_newRenderers.Value.Insert(0, (IEntitiesRenderer) null);
        this.m_renderers = this.m_newRenderers.Value.ToArray();
        this.m_newRenderers = Option<Lyst<IEntitiesRenderer>>.None;
      }
      else
        Log.Error("Already initialized?");
      foreach (IRenderedEntity renderedEntity in this.m_entitiesManager.GetAllEntitiesOfType<IRenderedEntity>())
        this.addEntity(renderedEntity);
      this.m_entitiesManager.EntityAdded.AddNonSaveable<EntitiesRenderingManager>(this, (Action<IEntity>) (entity =>
      {
        if (entity.IsDestroyed)
        {
          Log.Warning(string.Format("Trying to add destroyed entity for rendering: {0}", (object) entity));
        }
        else
        {
          if (!(entity is IRenderedEntity renderedEntity2))
            return;
          if (((long) renderedEntity2.RendererData & -72057594037927936L) != 0L)
            Log.Warning(string.Format("Trying to render already rendered entity: {0}", (object) entity));
          else
            this.addEntity(renderedEntity2);
        }
      }));
      this.m_entitiesManager.EntityRemovedFull.AddNonSaveable<EntitiesRenderingManager>(this, (Action<IEntity, EntityRemoveReason>) ((entity, reason) =>
      {
        if (!(entity is IRenderedEntity renderedEntity4))
          return;
        if (((long) renderedEntity4.RendererData & -72057594037927936L) == 0L)
          Log.Warning(string.Format("Trying to remove rendering or already removed entity: {0}", (object) entity));
        else
          this.removeEntity(renderedEntity4, reason);
      }));
      this.m_entitiesManager.OnUpgradeJustPerformed.AddNonSaveable<EntitiesRenderingManager>(this, (Action<IUpgradableEntity, IEntityProto>) ((entity, previousProto) =>
      {
        if (entity.IsDestroyed)
        {
          Log.Warning(string.Format("Trying to update destroyed entity for rendering: {0}", (object) entity));
        }
        else
        {
          IRenderedEntity renderedEntity5 = (IRenderedEntity) entity;
          if (renderedEntity5 == null)
            return;
          this.removeEntity(renderedEntity5, EntityRemoveReason.Remove);
          this.addEntity(renderedEntity5);
        }
      }));
      this.m_entitiesManager.OnEntityVisualChanged.AddNonSaveable<EntitiesRenderingManager>(this, (Action<IEntity>) (entity =>
      {
        if (entity.IsDestroyed)
        {
          Log.Warning(string.Format("Trying to update destroyed entity for rendering: {0}", (object) entity));
        }
        else
        {
          if (!(entity is IRenderedEntity renderedEntity7))
            return;
          this.removeEntity(renderedEntity7, EntityRemoveReason.Remove);
          this.addEntity(renderedEntity7);
        }
      }));
      this.m_constructionManager.EntityConstructionStateChanged.AddNonSaveable<EntitiesRenderingManager>(this, (Action<IStaticEntity, ConstructionState>) ((entity, _) =>
      {
        if (entity.IsDestroyed)
        {
          Log.Warning(string.Format("Trying to update destroyed entity for rendering: {0}", (object) entity));
        }
        else
        {
          if (entity.ConstructionState == ConstructionState.Deconstructed)
            return;
          bool flag;
          switch (entity.ConstructionState)
          {
            case ConstructionState.NotInitialized:
            case ConstructionState.NotStarted:
            case ConstructionState.PreparingUpgrade:
            case ConstructionState.PendingDeconstruction:
              flag = true;
              break;
            default:
              flag = false;
              break;
          }
          if (flag)
            return;
          IRenderedEntity renderedEntity8 = (IRenderedEntity) entity;
          if (renderedEntity8 == null)
            return;
          this.updateEntity(renderedEntity8);
        }
      }));
      this.m_constructionManager.EntityPauseStateChanged.AddNonSaveable<EntitiesRenderingManager>(this, (Action<IStaticEntity, bool>) ((entity, _) =>
      {
        if (entity.IsDestroyed)
        {
          Log.Warning(string.Format("Trying to update destroyed entity for rendering: {0}", (object) entity));
        }
        else
        {
          IRenderedEntity renderedEntity9 = (IRenderedEntity) entity;
          if (renderedEntity9 == null)
            return;
          this.updateEntity(renderedEntity9);
        }
      }));
      this.m_constructionManager.EntityConstructionNearlyFinished.AddNonSaveable<EntitiesRenderingManager>(this, (Action<IStaticEntity>) (entity =>
      {
        if (entity.IsDestroyed)
        {
          Log.Warning(string.Format("Trying to update destroyed entity for rendering: {0}", (object) entity));
        }
        else
        {
          IRenderedEntity renderedEntity10 = (IRenderedEntity) entity;
          if (renderedEntity10 == null)
            return;
          this.updateEntity(renderedEntity10);
        }
      }));
      this.m_constructionManager.OnResetConstructionAnimationState.AddNonSaveable<EntitiesRenderingManager>(this, (Action<IStaticEntity>) (entity =>
      {
        if (entity.IsDestroyed)
        {
          Log.Warning(string.Format("Trying to update destroyed entity for rendering: {0}", (object) entity));
        }
        else
        {
          IRenderedEntity renderedEntity11 = (IRenderedEntity) entity;
          if (renderedEntity11 == null)
            return;
          this.updateEntity(renderedEntity11);
        }
      }));
    }

    private void addEntity(IRenderedEntity renderedEntity)
    {
      byte entityRendererIndex = this.getEntityRendererIndex(renderedEntity);
      renderedEntity.RendererData |= (ulong) entityRendererIndex << 56;
      this.m_renderers[(int) entityRendererIndex].AddEntityOnSim(renderedEntity);
    }

    private void updateEntity(IRenderedEntity renderedEntity)
    {
      byte index = (byte) (renderedEntity.RendererData >> 56);
      if (index == (byte) 0)
        Log.Error(string.Format("Trying to update non-rendered entity '{0}'.", (object) renderedEntity));
      else if ((uint) index >= (uint) this.m_renderers.Length)
      {
        Log.Error(string.Format("Trying to update entity '{0}' with invalid renderer ID {1}.", (object) renderedEntity, (object) index));
      }
      else
      {
        byte entityRendererIndex = this.getEntityRendererIndex(renderedEntity);
        if ((int) index == (int) entityRendererIndex)
        {
          this.m_renderers[(int) entityRendererIndex].UpdateEntityOnSim(renderedEntity);
        }
        else
        {
          this.m_renderers[(int) index].RemoveEntityOnSim(renderedEntity, EntityRemoveReason.Remove);
          renderedEntity.RendererData &= 72057594037927935UL;
          renderedEntity.RendererData |= (ulong) entityRendererIndex << 56;
          this.m_renderers[(int) entityRendererIndex].AddEntityOnSim(renderedEntity);
        }
      }
    }

    private void removeEntity(IRenderedEntity renderedEntity, EntityRemoveReason reason)
    {
      int index = (int) (renderedEntity.RendererData >> 56);
      renderedEntity.RendererData &= 72057594037927935UL;
      if (index < this.m_renderers.Length)
        this.m_renderers[index].RemoveEntityOnSim(renderedEntity, reason);
      else
        Log.Error(string.Format("Invalid renderer index: {0}", (object) index));
    }

    /// <summary>
    /// Highlights entity and returns ID for this highlight.
    /// The highlight must be removed via <see cref="M:Mafi.Unity.Entities.EntitiesRenderingManager.RemoveHighlight(System.UInt64)" />.
    /// Returned ID is never 0 so the value 0 can be used as "invalid" value.
    /// </summary>
    public ulong AddHighlight(IRenderedEntity entity, ColorRgba color)
    {
      byte entityRendererIndex = this.getEntityRendererIndex(entity);
      return this.m_renderers[(int) entityRendererIndex].AddHighlight(entity, color) | (ulong) entityRendererIndex << 56;
    }

    public void RemoveHighlight(ulong highlightId)
    {
      if (highlightId == 0UL)
        return;
      int index = (int) (highlightId >> 56);
      if (index >= this.m_renderers.Length)
        Log.Error(string.Format("Failed to remove highlight, invalid renderer index {0}", (object) index));
      else
        this.m_renderers[index].RemoveHighlight(highlightId & 72057594037927935UL);
    }

    private void syncUpdate(GameTime time)
    {
      for (int index = 1; index < this.m_renderers.Length; ++index)
      {
        try
        {
          this.m_renderers[index].HandlePreSyncRemove(time);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception in HandlePreSyncRemove of renderer '" + this.m_renderers[index].GetType().Name + "'.");
        }
      }
      for (int index = 1; index < this.m_renderers.Length; ++index)
      {
        try
        {
          this.m_renderers[index].SyncUpdate(time);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, "Exception in sync of renderer '" + this.m_renderers[index].GetType().Name + "'.");
        }
      }
    }

    public bool TryGetPickableEntityAs<T>(GameObject pickedGo, out T entity) where T : class, IRenderedEntity
    {
      for (int index = 1; index < this.m_renderers.Length; ++index)
      {
        if (this.m_renderers[index].TryGetPickableEntityAs<T>(pickedGo, out entity))
          return true;
      }
      entity = default (T);
      return false;
    }
  }
}
