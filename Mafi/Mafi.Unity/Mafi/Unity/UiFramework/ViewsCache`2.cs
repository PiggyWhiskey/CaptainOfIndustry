// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.ViewsCache`2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Unity.UserInterface;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  /// <summary>
  /// Cache for ui elements to prevent creating the each time they are needed. This is designed for a very special use
  /// case. That is that it can cache each element (for each data) only once. This is useful for containers where we
  /// are showing different elements each time but they are never repeated and it is not worth destroying them since
  /// they might be needed again in very near future (e.g. products in products picker). So the idea is that each such
  /// container has instance of this cache and directly owns all the elements created by this cache. So it is safe to
  /// query <see cref="M:Mafi.Unity.UiFramework.ViewsCache`2.GetView(`0)" /> multiple times and the container gets the same element every time. Having each
  /// instance of cache for each instance of container also prevents reparenting. So all we doing is show / hide.
  /// </summary>
  public abstract class ViewsCache<TData, TView> where TView : IUiElement
  {
    private readonly Dict<TData, TView> m_viewsCache;
    private readonly UiBuilder m_builder;

    protected ViewsCache(UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_viewsCache = new Dict<TData, TView>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
    }

    public TView GetView(TData data)
    {
      TView view;
      if (!this.m_viewsCache.TryGetValue(data, out view))
      {
        view = this.CreateView(this.m_builder, data);
        this.m_viewsCache[data] = view;
      }
      view.Show<TView>();
      return view;
    }

    protected abstract TView CreateView(UiBuilder builder, TData data);
  }
}
