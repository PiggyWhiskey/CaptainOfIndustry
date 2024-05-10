// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.FloatingPanel.FloatingColumn
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.FloatingPanel
{
  /// <summary>
  /// Implements a floating panel that can float over the rest of the UI while
  /// attached to a target. It properly handles screen edges and is also able to
  /// follow a target that is moving.
  /// 
  /// Make sure to add this panel to a layer that is above the rest of the UI. The
  /// parent layer should occupy all the area that is available for the window to use.
  /// In case of tooltips, such parent would span the entire screen.
  /// 
  /// You can define its placement policy by passing <see cref="T:Mafi.Unity.UiToolkit.Library.FloatingPanel.IFloatingPositionPolicy" />.
  /// </summary>
  public abstract class FloatingColumn : Mafi.Unity.UiToolkit.Library.Column
  {
    protected readonly IFloatingPositionPolicy PositionPolicy;
    private readonly IVisualElementScheduledItem m_updater;
    private Rect m_targetWorldRect;
    private Vector2 m_ourSize;
    private Option<UiComponent> m_target;
    private bool m_geometryCallbackRegistered;

    protected FloatingColumn(IFloatingPositionPolicy positionPolicy, Outer outer = null, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(outer, inner);
      this.PositionPolicy = positionPolicy;
      this.VisibleForRender<FloatingColumn>(false);
      this.m_updater = this.Schedule.Execute(new Action(this.update)).Every(10L);
      this.m_updater.Pause();
    }

    protected void SetTarget(UiComponent target)
    {
      this.m_target = (Option<UiComponent>) target;
      target.RegisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.onTargetDetached));
      this.setGeometryCallback(true);
      Px? left = new Px?(-1.px());
      this.AbsolutePosition<FloatingColumn>(bottom: new Px?(-1.px()), left: left);
      this.BringToFront();
      this.PositionPolicy.InitConstraints((UiComponent) this, target);
    }

    private void updatePosition(UiComponent target)
    {
      Vector2 position = this.PositionPolicy.GetPosition(this.Parent.Value.WorldToLocal(target.WorldBound), this.ResolvedSize, this.Parent.Value.ResolvedSize);
      Px? left = new Px?(position.x.px());
      this.AbsolutePosition<FloatingColumn>(new Px?(position.y.px()), left: left);
    }

    protected void ClearTarget()
    {
      if (this.m_target.IsNone)
        return;
      this.m_target.Value.UnregisterCallback<DetachFromPanelEvent>(new EventCallback<DetachFromPanelEvent>(this.onTargetDetached));
      this.VisibleForRender<FloatingColumn>(false);
      this.m_targetWorldRect = Rect.zero;
      this.m_target = (Option<UiComponent>) Option.None;
      this.m_ourSize = Vector2.zero;
      this.m_updater.Pause();
      this.setGeometryCallback(false);
    }

    protected abstract void OnTargetLost();

    private void onOurSizeChanged(GeometryChangedEvent evt)
    {
      if (this.m_target.IsNone)
        return;
      this.updatePosition(this.m_target.Value);
      this.VisibleForRender<FloatingColumn>(true);
      this.m_updater.Resume();
      this.setGeometryCallback(false);
    }

    /// <summary>
    /// Unity does not call onMouseLeave when element gets removed from the hierarchy.
    /// </summary>
    private void onTargetDetached(DetachFromPanelEvent evt)
    {
      this.ClearTarget();
      this.OnTargetLost();
    }

    /// <summary>
    /// Unity does not provide events to monitor target's position, only size and that's
    /// not enough. Also mouse leave is not called when element gets hidden.
    /// So we check every frame.
    /// </summary>
    private void update()
    {
      if (this.m_target.IsNone || !this.m_target.Value.IsVisible())
      {
        this.ClearTarget();
        this.OnTargetLost();
      }
      else
      {
        Rect worldBound = this.m_target.Value.WorldBound;
        Vector2 resolvedSize = this.ResolvedSize;
        if (this.m_targetWorldRect.x.IsNear(worldBound.x) && this.m_targetWorldRect.y.IsNear(worldBound.y) && this.m_ourSize.x.IsNear(resolvedSize.x) && this.m_ourSize.y.IsNear(resolvedSize.y))
          return;
        this.m_targetWorldRect = worldBound;
        this.updatePosition(this.m_target.Value);
      }
    }

    /// <summary>
    /// Note keeping the callback on, all the time might make us react to our changes causing
    /// recursion in Unity. Happens only when scrolling near the edge where tooltip has to flip
    /// over. It is not clear why as we are not changing our size, just choosing a position.
    /// 
    /// However having callback for the first layout is important as it might not happen on the
    /// next frame.
    /// </summary>
    private void setGeometryCallback(bool register)
    {
      if (this.m_geometryCallbackRegistered == register)
        return;
      this.m_geometryCallbackRegistered = register;
      if (register)
        this.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.onOurSizeChanged), TrickleDown.NoTrickleDown);
      else
        this.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(this.onOurSizeChanged));
    }
  }
}
