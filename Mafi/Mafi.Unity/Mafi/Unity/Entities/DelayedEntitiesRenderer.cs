// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.DelayedEntitiesRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Helper class that delays entity operations from sim to sync.
  /// Ideally, we should not be doing much work during sync so consider this as a temporary solution before all
  /// renderers can accept events on sim thread.
  /// </summary>
  public abstract class DelayedEntitiesRenderer : IEntitiesRenderer
  {
    private readonly Lyst<KeyValuePair<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>>> m_eventsToProcess;

    public abstract int Priority { get; }

    public abstract bool CanRenderEntity(EntityProto proto);

    public abstract void AddEntityOnSync(IRenderedEntity entity, GameTime time);

    public abstract void UpdateEntityOnSync(IRenderedEntity entity, GameTime time);

    public abstract void RemoveEntityOnSync(
      IRenderedEntity entity,
      GameTime time,
      EntityRemoveReason reason);

    public abstract bool TryGetPickableEntityAs<T>(GameObject pickedGo, out T entity) where T : class, IRenderedEntity;

    public abstract ulong AddHighlight(IRenderedEntity entity, ColorRgba color);

    public abstract void RemoveHighlight(ulong highlightId);

    public void AddEntityOnSim(IRenderedEntity entity)
    {
      for (int index = this.m_eventsToProcess.Count - 1; index >= 0; --index)
      {
        KeyValuePair<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>> keyValuePair = this.m_eventsToProcess[index];
        if (keyValuePair.Key == entity)
        {
          switch (keyValuePair.Value.First)
          {
            case DelayedEntitiesRenderer.EventType.Add:
              Log.Warning(string.Format("Add event after another add event for {0}, ignoring.", (object) entity));
              return;
            case DelayedEntitiesRenderer.EventType.Update:
              Log.Warning(string.Format("Add event after update event for {0}, strange.", (object) entity));
              goto label_7;
            default:
              goto label_7;
          }
        }
      }
label_7:
      this.m_eventsToProcess.Add<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>>(entity, Pair.Create<DelayedEntitiesRenderer.EventType, EntityRemoveReason>(DelayedEntitiesRenderer.EventType.Add, EntityRemoveReason.Remove));
    }

    public void UpdateEntityOnSim(IRenderedEntity entity)
    {
      for (int index = this.m_eventsToProcess.Count - 1; index >= 0; --index)
      {
        KeyValuePair<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>> keyValuePair = this.m_eventsToProcess[index];
        if (keyValuePair.Key == entity)
        {
          switch (keyValuePair.Value.First)
          {
            case DelayedEntitiesRenderer.EventType.Add:
              return;
            case DelayedEntitiesRenderer.EventType.Remove:
              Log.Warning(string.Format("Update event after a remove event for {0}, ignoring", (object) entity));
              return;
            case DelayedEntitiesRenderer.EventType.Update:
              return;
            default:
              goto label_7;
          }
        }
      }
label_7:
      this.m_eventsToProcess.Add<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>>(entity, Pair.Create<DelayedEntitiesRenderer.EventType, EntityRemoveReason>(DelayedEntitiesRenderer.EventType.Update, EntityRemoveReason.Remove));
    }

    public void RemoveEntityOnSim(IRenderedEntity entity, EntityRemoveReason reason)
    {
      for (int index = this.m_eventsToProcess.Count - 1; index >= 0; --index)
      {
        KeyValuePair<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>> keyValuePair = this.m_eventsToProcess[index];
        if (keyValuePair.Key == entity)
        {
          switch (keyValuePair.Value.First)
          {
            case DelayedEntitiesRenderer.EventType.Add:
              this.m_eventsToProcess.RemoveAt(index);
              return;
            case DelayedEntitiesRenderer.EventType.Remove:
              Log.Warning(string.Format("Remove event after a remove event for {0}, ignoring", (object) entity));
              return;
            case DelayedEntitiesRenderer.EventType.Update:
              this.m_eventsToProcess.RemoveAt(index);
              continue;
            default:
              continue;
          }
        }
      }
      this.m_eventsToProcess.Add<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>>(entity, Pair.Create<DelayedEntitiesRenderer.EventType, EntityRemoveReason>(DelayedEntitiesRenderer.EventType.Remove, reason));
    }

    public void HandlePreSyncRemove(GameTime time)
    {
      foreach (KeyValuePair<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>> keyValuePair in this.m_eventsToProcess)
      {
        try
        {
          switch (keyValuePair.Value.First)
          {
            case DelayedEntitiesRenderer.EventType.Add:
            case DelayedEntitiesRenderer.EventType.Update:
              continue;
            case DelayedEntitiesRenderer.EventType.Remove:
              this.RemoveEntityOnSync(keyValuePair.Key, time, keyValuePair.Value.Second);
              continue;
            default:
              Log.Error(string.Format("Unhandled event type {0}.", (object) keyValuePair.Value.First));
              continue;
          }
        }
        catch (Exception ex)
        {
          Log.Exception(ex, string.Format("Failed to process {0} event for '{1}'.", (object) keyValuePair.Value.First, (object) keyValuePair.Key));
        }
      }
    }

    public virtual void SyncUpdate(GameTime time)
    {
      foreach (KeyValuePair<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>> keyValuePair in this.m_eventsToProcess)
      {
        try
        {
          switch (keyValuePair.Value.First)
          {
            case DelayedEntitiesRenderer.EventType.Add:
              this.AddEntityOnSync(keyValuePair.Key, time);
              continue;
            case DelayedEntitiesRenderer.EventType.Remove:
              continue;
            case DelayedEntitiesRenderer.EventType.Update:
              this.UpdateEntityOnSync(keyValuePair.Key, time);
              continue;
            default:
              Log.Error(string.Format("Unhandled event type {0}.", (object) keyValuePair.Value.First));
              continue;
          }
        }
        catch (Exception ex)
        {
          Log.Exception(ex, string.Format("Failed to process {0} event for '{1}'.", (object) keyValuePair.Value.First, (object) keyValuePair.Key));
        }
      }
      this.m_eventsToProcess.Clear();
    }

    protected DelayedEntitiesRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_eventsToProcess = new Lyst<KeyValuePair<IRenderedEntity, Pair<DelayedEntitiesRenderer.EventType, EntityRemoveReason>>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private enum EventType
    {
      Add,
      Remove,
      Update,
    }
  }
}
