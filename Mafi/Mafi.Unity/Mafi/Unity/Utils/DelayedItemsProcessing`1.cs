// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.DelayedItemsProcessing`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  /// <summary>
  /// Accepts additions and removals on the sim thread and properly synchronizes these events to be propagated to the
  /// render thread. The order of events is preserved.
  /// </summary>
  public class DelayedItemsProcessing<T>
  {
    private readonly Action<T> m_onAdded;
    private readonly Action<T> m_onRemoved;
    private readonly Lyst<DelayedItemsProcessing<T>.ItemChange> m_simChanges;
    private readonly Lyst<DelayedItemsProcessing<T>.ItemChange> m_entitiesForRender;

    public DelayedItemsProcessing(Action<T> onAdded, Action<T> onRemoved)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_simChanges = new Lyst<DelayedItemsProcessing<T>.ItemChange>();
      this.m_entitiesForRender = new Lyst<DelayedItemsProcessing<T>.ItemChange>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_onAdded = onAdded.CheckNotNull<Action<T>>();
      this.m_onRemoved = onRemoved.CheckNotNull<Action<T>>();
    }

    public void AddOnSim(T item)
    {
      this.m_simChanges.Add(new DelayedItemsProcessing<T>.ItemChange(item, true));
    }

    public void RemoveOnSim(T item)
    {
      this.m_simChanges.Add(new DelayedItemsProcessing<T>.ItemChange(item, false));
    }

    public void SyncUpdate()
    {
      this.m_entitiesForRender.SwapDataWith(this.m_simChanges);
      this.m_simChanges.Clear();
    }

    public void RenderUpdate()
    {
      foreach (DelayedItemsProcessing<T>.ItemChange itemChange in this.m_entitiesForRender)
      {
        if (itemChange.Added)
          this.m_onAdded(itemChange.Item);
        else
          this.m_onRemoved(itemChange.Item);
      }
      this.m_entitiesForRender.Clear();
    }

    private struct ItemChange
    {
      public readonly T Item;
      public readonly bool Added;

      public ItemChange(T item, bool added)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Item = item;
        this.Added = added;
      }
    }
  }
}
