// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponent
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Unity.UserInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public class UiComponent : 
    IEnumerable<UiComponent>,
    IEnumerable,
    IUiComponent,
    IComponentWithClass,
    IComponentWithLayout,
    IComponentWithPadding,
    IComponentWithBorder,
    IComponentWithBackground,
    IComponentWithFont
  {
    protected readonly VisualElement Element;
    private LystStruct<UiComponent> m_children;
    private Option<Lyst<Action<UiBuilder>>> m_performOnBuild;
    private Option<SimpleTooltipPromise> m_tooltip;

    /// <summary>Careful! Valid only after layout phase is done!</summary>
    public Rect WorldBound => this.Element.worldBound;

    public Rect LocalBound => this.Element.localBound;

    /// <summary>Careful! Valid only after layout phase is done!</summary>
    public float ResolvedWidth => this.Element.resolvedStyle.width;

    /// <summary>Careful! Valid only after layout phase is done!</summary>
    public float ResolvedHeight => this.Element.resolvedStyle.height;

    /// <summary>Careful! Valid only after layout phase is done!</summary>
    public Vector2 ResolvedSize => new Vector2(this.ResolvedWidth, this.ResolvedHeight);

    public IVisualElementScheduler Schedule => this.Element.schedule;

    /// <summary>
    /// Should be the root container. Use only when really necessary.
    /// </summary>
    public VisualElement RootElement => this.Element;

    protected Option<UiBuilder> Builder { get; set; }

    public bool HasParent => this.Parent.HasValue;

    public Option<UiComponent> Parent { get; private set; }

    public int Count => this.m_children.Count;

    public IEnumerable<UiComponent> AllChildren => this.m_children.AsEnumerable();

    public UiComponent this[int index] => this.m_children[index];

    protected bool HasNonEmptyTooltipSet
    {
      get
      {
        SimpleTooltipPromise valueOrNull = this.m_tooltip.ValueOrNull;
        return valueOrNull != null && valueOrNull.Text.IsNotEmpty;
      }
    }

    public UiComponent(VisualElement element)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Element = element;
    }

    public UiComponent()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Element = new VisualElement();
    }

    public void Add(UiComponent child) => this.InsertAt(this.Count, child);

    public void Add(IEnumerable<UiComponent> children)
    {
      foreach (UiComponent child in children)
        this.InsertAt(this.Count, child);
    }

    public virtual void InsertAt(int index, [CanBeNull] UiComponent child)
    {
      if (child == null)
        return;
      if (child == this)
        Log.Error("Child can't be its own parent! Are you trying to break the space-time continuum?!?");
      else if (child.Parent.ValueOrNull == this)
      {
        Log.Error("Child is already attached.");
      }
      else
      {
        if (child.Parent.HasValue)
        {
          Log.Error("Child is already attached to a different parent.");
          child.RemoveFromHierarchy();
        }
        child.Parent = (Option<UiComponent>) this;
        if (index == this.m_children.Count)
        {
          this.m_children.Add(child);
          this.GetChildrenContainer().Add(child.Element);
        }
        else
        {
          this.m_children.Insert(index, child);
          this.GetChildrenContainer().Insert(index, child.Element);
        }
        this.asGapHandler().ValueOrNull?.OnChildInserted(index, child);
        this.OnChildInserted(child);
        if (!this.Builder.HasValue)
          return;
        child.Build(this.Builder.Value);
      }
    }

    protected virtual void OnChildInserted(UiComponent child)
    {
    }

    protected int? IndexOfChild(UiComponent child)
    {
      int num = this.m_children.IndexOf(child);
      return num < 0 ? new int?() : new int?(num);
    }

    public virtual void Clear()
    {
      this.asGapHandler().ValueOrNull?.OnBeforeClear();
      foreach (UiComponent child in this.m_children)
        child.Parent = (Option<UiComponent>) Option.None;
      this.m_children.Clear();
      this.GetChildrenContainer().Clear();
    }

    public void RemoveFromHierarchy()
    {
      this.Element.RemoveFromHierarchy();
      this.Parent.ValueOrNull?.removeChild(this);
    }

    private void removeChild(UiComponent child)
    {
      int index = this.m_children.IndexOf(child);
      if (index < 0)
      {
        Log.Error("Child not found");
      }
      else
      {
        this.m_children.RemoveAt(index);
        child.Parent = (Option<UiComponent>) Option.None;
        this.asGapHandler().ValueOrNull?.OnChildRemovedAt(index, child);
      }
    }

    internal virtual VisualElement GetChildrenContainer() => this.Element;

    public virtual VisualElement Build(UiBuilder builder)
    {
      if (this.Builder.HasValue)
        return this.Element;
      this.Builder = (Option<UiBuilder>) builder;
      if (this.m_performOnBuild.HasValue)
      {
        foreach (Action<UiBuilder> action in this.m_performOnBuild.Value)
          action(this.Builder.Value);
        this.m_performOnBuild = (Option<Lyst<Action<UiBuilder>>>) Option.None;
      }
      foreach (UiComponent child in this.m_children)
        child.Build(builder);
      return this.Element;
    }

    void IUiComponent.SetColor(ColorRgba? color) => this.SetColorInternal(color);

    protected virtual void SetColorInternal(ColorRgba? color)
    {
      IStyle style = this.Element.style;
      Color? nullable = color.HasValue ? new Color?(color.GetValueOrDefault().ToColor()) : new Color?();
      StyleColor styleColor = nullable.HasValue ? (StyleColor) nullable.GetValueOrDefault() : new StyleColor(StyleKeyword.Null);
      style.color = styleColor;
    }

    void IUiComponent.SetEnabled(bool enabled) => this.SetEnabledInternal(enabled);

    protected virtual void SetEnabledInternal(bool enabled) => this.Element.SetEnabled(enabled);

    public void RunWithBuilder(Action<UiBuilder> buildAction)
    {
      if (this.Builder.HasValue)
      {
        buildAction(this.Builder.Value);
      }
      else
      {
        if (this.m_performOnBuild.IsNone)
          this.m_performOnBuild = (Option<Lyst<Action<UiBuilder>>>) new Lyst<Action<UiBuilder>>();
        this.m_performOnBuild.Value.Add(buildAction);
      }
    }

    private Option<IGapHandler> asGapHandler()
    {
      return !(this is IFlexComponent flexComponent) ? (Option<IGapHandler>) Option.None : flexComponent.GapHandler;
    }

    public IEnumerator<UiComponent> GetEnumerator()
    {
      return this.m_children.AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.m_children.AsEnumerable().GetEnumerator();
    }

    void IUiComponent.SetName(string name) => this.Element.name = name;

    public virtual ILayoutDecorator GetLayoutDecorator()
    {
      return (ILayoutDecorator) LayoutDecorator.GetSharedInstance(this.Element, (IComponentWithLayout) this);
    }

    void IComponentWithLayout.SetSize(StyleLength? width, StyleLength? height)
    {
      this.setSizeInternal(width, height);
    }

    protected virtual void setSizeInternal(StyleLength? width, StyleLength? height)
    {
      if (width.HasValue)
        this.Element.style.width = width.Value;
      if (!height.HasValue)
        return;
      this.Element.style.height = height.Value;
    }

    public virtual IClassDecorator GetClassDecorator()
    {
      return (IClassDecorator) ClassDecorator.GetSharedInstance(this.Element);
    }

    public virtual IPaddingDecorator GetPaddingDecorator()
    {
      return (IPaddingDecorator) PaddingDecorator.GetSharedInstance(this.Element);
    }

    public virtual IFontDecorator GetFontDecorator()
    {
      return (IFontDecorator) FontDecorator.GetSharedInstance(this.Element);
    }

    public virtual IBorderDecorator GetBorderDecorator()
    {
      return (IBorderDecorator) BorderDecorator.GetSharedInstance(this.Element);
    }

    public virtual IBackgroundDecorator GetBackgroundDecorator()
    {
      return (IBackgroundDecorator) BackgroundDecorator.GetSharedInstance(this.Element);
    }

    public virtual void SetTooltipOrCreate(LocStrFormatted tooltip)
    {
      if (this.m_tooltip.HasValue)
        this.m_tooltip.Value.Text = tooltip;
      else
        this.m_tooltip = (Option<SimpleTooltipPromise>) new SimpleTooltipPromise(this, tooltip);
    }

    public virtual UiComponent RegisterCallback<TEventType>(
      EventCallback<TEventType> callback,
      TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
      where TEventType : UnityEngine.UIElements.EventBase<TEventType>, new()
    {
      this.Element.RegisterCallback<TEventType>(callback, useTrickleDown);
      return this;
    }

    public virtual void UnregisterCallback<TEventType>(
      EventCallback<TEventType> callback,
      TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
      where TEventType : UnityEngine.UIElements.EventBase<TEventType>, new()
    {
      this.Element.UnregisterCallback<TEventType>(callback, useTrickleDown);
    }

    /// <summary>
    /// Registers for the given callback once. Automatically unregisters when the callback is received.
    /// </summary>
    public virtual UiComponent CallbackOnce<TEventType>(
      EventCallback<TEventType> callback,
      TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
      where TEventType : UnityEngine.UIElements.EventBase<TEventType>, new()
    {
      this.RegisterCallback<TEventType>(new EventCallback<TEventType>(wrap), useTrickleDown);
      return this;

      void wrap(TEventType evt)
      {
        this.UnregisterCallback<TEventType>(new EventCallback<TEventType>(wrap), useTrickleDown);
        callback(evt);
      }
    }

    public virtual void SetFocusable(bool isFocusable) => this.Element.focusable = isFocusable;

    /// <summary>
    /// Whether this element will go through the layout (or will be absent).
    /// Attention: If you use RemoveFromHierarchy, this will still return true.
    /// </summary>
    public virtual bool IsVisible()
    {
      return this.Element.style.display != (StyleEnum<DisplayStyle>) DisplayStyle.None;
    }

    /// <summary>
    /// Determines whether this element will go through the layout (or will be absent).
    /// </summary>
    public virtual void SetVisible(bool isVisible)
    {
      this.Element.style.display = (StyleEnum<DisplayStyle>) (isVisible ? DisplayStyle.Flex : DisplayStyle.None);
    }

    /// <summary>
    /// Important: Even if hidden this element will still go through layout and will occupy space
    /// in its parent container. This is only useful in cases where you want to run layout on
    /// hidden items.
    /// </summary>
    /// <param name="isVisible">True to show, false to hide</param>
    void IUiComponent.SetVisibleForRender(bool isVisible) => this.Element.visible = isVisible;

    public bool IsPositionAbsolute() => this.Element.resolvedStyle.position == Position.Absolute;

    /// <summary>
    /// This will prevent elements to pick input. Very useful for transparent
    /// fullscreen containers that only host children.
    /// </summary>
    /// <param name="isIgnored">True to disable mouse interaction, false to enable</param>
    /// <param name="recursive">True to apply changes recursively to all children</param>
    void IUiComponent.SetIgnoreInputPicking(bool isIgnored, bool recursive)
    {
      this.setIgnoreInputPicking(isIgnored, recursive);
    }

    protected virtual void setIgnoreInputPicking(bool isIgnored, bool recursive)
    {
      this.Element.pickingMode = isIgnored ? PickingMode.Ignore : PickingMode.Position;
      if (!recursive)
        return;
      this.Schedule.Execute((Action) (() => UiComponent.setPickingModeRecursively(this.Element, isIgnored ? PickingMode.Ignore : PickingMode.Position)));
    }

    public virtual void Focus() => this.Element.Focus();

    public void SendToBack() => this.Element.SendToBack();

    public void BringToFront() => this.Element.BringToFront();

    public Rect WorldToLocal(Rect r) => this.Element.WorldToLocal(r);

    public void OnMouseEnter(EventCallback<MouseEnterEvent> action)
    {
      this.Element.RegisterCallback<MouseEnterEvent>(action);
    }

    public void OnMouseLeave(EventCallback<MouseLeaveEvent> action)
    {
      this.Element.RegisterCallback<MouseLeaveEvent>(action);
    }

    protected static void setPickingModeRecursively(VisualElement elem, PickingMode mode)
    {
      elem.pickingMode = mode;
      foreach (VisualElement child in elem.hierarchy.Children())
        UiComponent.setPickingModeRecursively(child, mode);
    }
  }
}
