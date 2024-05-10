// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.FloatingPanel.FloatingContainer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.FloatingPanel
{
  public class FloatingContainer : FloatingColumn, IRootEscapeHandler
  {
    protected readonly UiComponent m_target;
    private readonly Action m_onClosed;
    private bool m_matchTargetWidth;

    public FloatingContainer(UiComponent target, Action onClosed, Inner inner = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_matchTargetWidth = true;
      // ISSUE: explicit constructor call
      base.\u002Ector((IFloatingPositionPolicy) new FloatingContainer.FloatingPositionPolicy(), Outer.ShadowAll, inner ?? Inner.GlowAll);
      this.m_target = target;
      this.m_onClosed = onClosed;
      this.Class<FloatingContainer>(Cls.inputDropdown_popup).AlignItemsStretch<FloatingContainer>();
      target.RunWithBuilder(new Action<UiBuilder>(this.build));
    }

    private void build(UiBuilder builder) => this.Builder = (Option<UiBuilder>) builder;

    public override void SetVisible(bool newVisible)
    {
      UiBuilder valueOrNull = this.Builder.ValueOrNull;
      if (valueOrNull == null)
        Log.Error("Builder is NULL!");
      else if (newVisible)
      {
        valueOrNull.AddFloatingComponent((UiComponent) this);
        valueOrNull.SetOneTimeEscBlockingCallback((IRootEscapeHandler) this);
        valueOrNull.UiDocument.rootVisualElement.RegisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.handleRootEvent), TrickleDown.TrickleDown);
        valueOrNull.UiDocument.rootVisualElement.RegisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.handleRootEvent), TrickleDown.TrickleDown);
        valueOrNull.UiDocument.rootVisualElement.pickingMode = PickingMode.Position;
        this.SetTarget(this.m_target);
      }
      else
      {
        this.ClearTarget();
        Action onClosed = this.m_onClosed;
        if (onClosed != null)
          onClosed();
        valueOrNull.UiDocument.rootVisualElement.UnregisterCallback<MouseUpEvent>(new EventCallback<MouseUpEvent>(this.handleRootEvent), TrickleDown.TrickleDown);
        valueOrNull.UiDocument.rootVisualElement.UnregisterCallback<FocusEvent>(new EventCallback<FocusEvent>(this.handleRootEvent), TrickleDown.TrickleDown);
        valueOrNull.UiDocument.rootVisualElement.pickingMode = PickingMode.Ignore;
        this.RemoveFromHierarchy();
        valueOrNull.ClearEscBlockingCallback((IRootEscapeHandler) this);
      }
    }

    public override bool IsVisible() => this.Element.visible;

    bool IRootEscapeHandler.OnEscape()
    {
      if (this.Parent.IsNone)
        return false;
      this.SetVisible(false);
      return true;
    }

    public void Close() => this.SetVisible(false);

    public FloatingContainer MatchTargetWidth(bool match)
    {
      this.m_matchTargetWidth = match;
      this.SetTarget(this.m_target);
      return this;
    }

    private void handleRootEvent(EventBase evt)
    {
      if (this.Element.Contains(evt.target as VisualElement))
        return;
      this.Close();
    }

    protected override void OnTargetLost() => this.Close();

    private class FloatingPositionPolicy : IFloatingPositionPolicy
    {
      public void InitConstraints(UiComponent floatingPanel, UiComponent target)
      {
        if (floatingPanel is FloatingContainer floatingContainer && floatingContainer.m_matchTargetWidth)
          floatingPanel.Width<UiComponent>(new Px?(target.ResolvedWidth.px()));
        else
          floatingPanel.Width<UiComponent>(new Px?());
      }

      public Vector2 GetPosition(
        Rect boundsOfTarget,
        Vector2 floatingPanelSize,
        Vector2 screenSize)
      {
        float x1 = boundsOfTarget.x;
        float y1 = boundsOfTarget.y;
        float y2 = (float) ((double) y1 + (double) boundsOfTarget.height + 3.0);
        if ((double) (y2 + floatingPanelSize.y) > (double) screenSize.y - 4.0)
          y2 = ((float) ((double) y1 - (double) floatingPanelSize.y - 3.0)).Max(0.0f);
        float self = x1;
        float x2 = screenSize.x;
        float num = (float) ((double) self + (double) floatingPanelSize.x + 4.0) - x2;
        if ((double) num > 0.0)
          self -= num + 4f;
        return new Vector2(self.Max(4f), y2);
      }

      public FloatingPositionPolicy()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
