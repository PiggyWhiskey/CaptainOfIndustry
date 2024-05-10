// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.ComponentsCache`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class ComponentsCache<T> where T : UiComponent
  {
    private readonly Func<T> m_viewFactory;
    private readonly Queueue<T> m_freeItems;
    private readonly Queueue<T> m_usedItems;

    public ComponentsCache(Func<T> viewFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_freeItems = new Queueue<T>(true);
      this.m_usedItems = new Queueue<T>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_viewFactory = viewFactory;
    }

    public T GetView()
    {
      if (this.m_freeItems.IsEmpty)
        this.m_freeItems.Enqueue(this.m_viewFactory());
      T view = this.m_freeItems.Dequeue();
      this.m_usedItems.Enqueue(view);
      return view;
    }

    public void Return(T item)
    {
      if (this.m_usedItems.TryRemove(item))
        this.m_freeItems.Enqueue(item);
      item.RemoveFromHierarchy();
    }

    public void ReturnAll(bool removeFromHierarchy = false)
    {
      foreach (T usedItem in this.m_usedItems)
      {
        if (removeFromHierarchy)
          usedItem.RemoveFromHierarchy();
        this.m_freeItems.Enqueue(usedItem);
      }
      this.m_usedItems.Clear();
    }

    public IIndexable<T> AllExistingOnes() => (IIndexable<T>) this.m_usedItems;
  }
}
