// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.FloatingPanel.FloatingPanelPromiseBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.FloatingPanel
{
  /// <summary>
  /// Floating panels such as tooltips typically have very small number or real instances.
  /// So having a UI for each use would unnecessarily increase size of the DOM. For that
  /// reason we just cache the instances via UiBuilder and reuse them. This saves thousands
  /// of unnecessary elements.
  /// </summary>
  public abstract class FloatingPanelPromiseBase<T> where T : UiComponent, new()
  {
    public readonly UiComponent Target;
    private UiBuilder m_builder;
    private Queueue<T> m_cache;
    private Option<T> m_view;

    public FloatingPanelPromiseBase(UiComponent target)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Target = target;
      target.RunWithBuilder(new Action<UiBuilder>(this.build));
    }

    private void build(UiBuilder builder)
    {
      this.m_builder = builder;
      this.m_cache = builder.GetOrCreateCache<T>(typeof (T).FullName);
      this.Target.OnMouseEnter(new EventCallback<MouseEnterEvent>(this.onTargetEnter));
      this.Target.OnMouseLeave(new EventCallback<MouseLeaveEvent>(this.onTargetLeave));
    }

    private void onTargetEnter(MouseEnterEvent owner)
    {
      if (!this.HasDataToShow())
        return;
      if (this.m_view.IsNone)
      {
        if (this.m_cache.IsNotEmpty)
        {
          this.m_view = (Option<T>) this.m_cache.Dequeue();
        }
        else
        {
          this.m_view = (Option<T>) new T();
          this.m_builder.AddFloatingComponent((UiComponent) this.m_view.Value);
        }
      }
      this.PopulateAndShow(this.m_view.Value);
    }

    private void onTargetLeave(MouseLeaveEvent owner) => this.OnTargetLost();

    public void OnTargetLost()
    {
      if (this.m_view.IsNone)
        return;
      this.ClearTarget(this.m_view.Value);
      this.m_cache.Enqueue(this.m_view.Value);
      this.m_view = (Option<T>) Option.None;
    }

    protected void OnTargetValueChanged()
    {
      if (!this.m_view.HasValue)
        return;
      if (this.HasDataToShow())
        this.onTargetEnter((MouseEnterEvent) null);
      else
        this.OnTargetLost();
    }

    protected abstract bool HasDataToShow();

    protected abstract void PopulateAndShow(T view);

    protected abstract void ClearTarget(T view);
  }
}
