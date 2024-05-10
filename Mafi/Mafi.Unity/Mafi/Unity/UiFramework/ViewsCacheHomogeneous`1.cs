// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.ViewsCacheHomogeneous`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Syncers;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  /// <summary>
  /// This is a very simple cache that counts with the fact that its elements can be reused for any input thus they are
  /// all equivalent. So this cache creates a new view if needed otherwise it provides the ones that were previously
  /// created and returned to it.
  /// </summary>
  public class ViewsCacheHomogeneous<T> where T : IUiElement
  {
    public readonly IUiUpdater Updater;
    private readonly Func<T> m_viewFactory;
    private readonly Queueue<T> m_freeItems;
    private readonly Queueue<T> m_usedItems;

    public ViewsCacheHomogeneous(Func<T> viewFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_freeItems = new Queueue<T>(true);
      this.m_usedItems = new Queueue<T>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_viewFactory = viewFactory;
      this.Updater = UpdaterBuilder.Start().Build();
    }

    public T GetView()
    {
      if (this.m_freeItems.IsEmpty)
        this.m_freeItems.Enqueue(this.m_viewFactory());
      T view = this.m_freeItems.Dequeue().Show<T>();
      this.m_usedItems.Enqueue(view);
      if (view is IUiElementWithUpdater elementWithUpdater)
        this.Updater.AddChildUpdater(elementWithUpdater.Updater);
      return view;
    }

    public void Return(T item)
    {
      if (item is IUiElementWithUpdater elementWithUpdater)
        this.Updater.RemoveChildUpdater(elementWithUpdater.Updater);
      if (this.m_usedItems.TryRemove(item))
        this.m_freeItems.Enqueue(item);
      item.Hide<T>();
    }

    public void ReturnAll()
    {
      foreach (T usedItem in this.m_usedItems)
      {
        usedItem.Hide<T>();
        this.m_freeItems.Enqueue(usedItem);
      }
      this.m_usedItems.Clear();
      this.Updater.ClearAllChildUpdaters();
    }

    public IIndexable<T> AllExistingOnes() => (IIndexable<T>) this.m_usedItems;
  }
}
