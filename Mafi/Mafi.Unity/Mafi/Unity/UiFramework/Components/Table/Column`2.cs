// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Table.Column`2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Unity.UserInterface;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Table
{
  /// <summary>
  /// Basic implementation of <see cref="T:Mafi.Unity.UiFramework.Components.Table.IColumn" /> to reduce boiler plate code in super classes.
  /// </summary>
  /// <typeparam name="TValue">Type of value</typeparam>
  /// <typeparam name="TView">Type of view</typeparam>
  public abstract class Column<TValue, TView> : IColumn
  {
    protected readonly int Index;
    /// <summary>Table that own this column.</summary>
    private readonly Mafi.Unity.UiFramework.Components.Table.Table m_view;
    /// <summary>Last known values cache to reduce UI updates.</summary>
    private readonly Lyst<TValue> m_cache;
    /// <summary>Views for each row (e.g. Text).</summary>
    private readonly Lyst<TView> m_views;

    public string Title { get; }

    public int Width { get; }

    public bool MergeWithPrevious { get; }

    protected Column(Mafi.Unity.UiFramework.Components.Table.Table view, int index, string title, int width, bool mergeWithPrevious)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_cache = new Lyst<TValue>();
      this.m_views = new Lyst<TView>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_view = view;
      this.Index = index;
      this.Title = title;
      this.Width = width;
      this.MergeWithPrevious = mergeWithPrevious;
    }

    public void UpdateCell(int rowIndex, TValue data)
    {
      this.m_cache[rowIndex] = this.UpdateCell(rowIndex, this.m_views[rowIndex], this.m_cache[rowIndex], data);
    }

    public void AddRow(bool highlighted)
    {
      this.m_cache.Add(this.DefaultValue());
      this.m_views.Add(this.CreateView(this.m_view.Builder, highlighted));
    }

    public void RemoveLastRow()
    {
      this.m_cache.RemoveAt(this.m_cache.Count - 1);
      this.m_views.RemoveAt(this.m_views.Count - 1);
    }

    public TView GetView(int rowIndex) => this.m_views[rowIndex];

    public GameObject GetGameObject(int rowIndex) => this.ResolveGameObject(this.m_views[rowIndex]);

    public virtual Offset InnerOffset() => Offset.Zero;

    protected int GetViewRowIndex(TView view) => this.m_views.IndexOf(view);

    /// <summary>Creates view to be used in the table.</summary>
    protected abstract TView CreateView(UiBuilder builder, bool highlighted);

    /// <summary>Returns gameObject from the given view.</summary>
    protected abstract GameObject ResolveGameObject(TView view);

    protected abstract TValue DefaultValue();

    /// <summary>Updates the given view with the given new value.</summary>
    /// <remarks>WARNING! Implementors shouldn't update UI if oldValue == newValue!</remarks>
    protected abstract TValue UpdateCell(
      int rowIndex,
      TView view,
      TValue oldValue,
      TValue newValue);
  }
}
