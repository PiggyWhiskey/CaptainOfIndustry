// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.ViewsCacheTracked`2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Syncers;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  public class ViewsCacheTracked<TData, TView> where TView : IUiElement
  {
    public readonly IUiUpdater Updater;
    private readonly Dict<TData, TView> m_viewsCache;
    private readonly Set<TView> m_activeViews;
    private readonly Func<TData, TView> m_creatorFnc;

    public ViewsCacheTracked(Func<TData, TView> creatorFnc)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_viewsCache = new Dict<TData, TView>();
      this.m_activeViews = new Set<TView>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_creatorFnc = creatorFnc;
      this.Updater = UpdaterBuilder.Start().Build();
    }

    public TView GetView(TData data)
    {
      TView element;
      if (!this.m_viewsCache.TryGetValue(data, out element))
      {
        element = this.m_creatorFnc(data);
        this.m_viewsCache[data] = element;
      }
      this.m_activeViews.Add(element);
      element.Show<TView>();
      if (element is IUiElementWithUpdater elementWithUpdater)
        this.Updater.AddChildUpdater(elementWithUpdater.Updater);
      return element;
    }

    public void ReturnAll()
    {
      foreach (TView activeView in this.m_activeViews)
        activeView.Hide<TView>();
      this.m_activeViews.Clear();
      this.Updater.ClearAllChildUpdaters();
    }
  }
}
