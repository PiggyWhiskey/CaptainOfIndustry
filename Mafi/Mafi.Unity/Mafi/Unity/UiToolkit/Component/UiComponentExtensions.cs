// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentExtensions
  {
    public static void Add(this UiComponent component, params UiComponent[] children)
    {
      foreach (UiComponent child in children)
        component.Add(child);
    }

    public static void Add(this UiComponent component, IEnumerable<UiComponent> children)
    {
      foreach (UiComponent child in children)
        component.Add(child);
    }

    public static void SetChildren(this UiComponent component, params UiComponent[] children)
    {
      component.Clear();
      component.Add((IEnumerable<UiComponent>) children);
    }

    public static void SetChildren(this UiComponent component, IEnumerable<UiComponent> children)
    {
      component.Clear();
      component.Add(children);
    }

    public static T ReverseChildren<T>(this T component) where T : UiComponent
    {
      UiComponent[] array = component.AllChildren.ToArray<UiComponent>();
      Array.Reverse<UiComponent>(array);
      component.Clear();
      component.Add((IEnumerable<UiComponent>) array);
      return component;
    }

    public static Option<UiComponent> ChildAtOrNone(this UiComponent component, int index)
    {
      return index >= 0 && index < component.Count ? (Option<UiComponent>) component[index] : Option<UiComponent>.None;
    }

    public static Option<T> ChildAtOrNone<T>(this UiComponent component, int index) where T : UiComponent
    {
      return index >= 0 && index < component.Count ? (Option<T>) (component[index] as T) : Option<T>.None;
    }

    public static UiComponent ChildAtOrDefault(this UiComponent component, int index)
    {
      return index >= 0 && index < component.Count ? component[index] : (UiComponent) null;
    }

    public static T ChildAtOrDefault<T>(this UiComponent component, int index) where T : UiComponent
    {
      return index >= 0 && index < component.Count ? (T) component[index] : default (T);
    }

    public static void Add<T>(this T component, [CanBeNull] Action<T> applyStyles) where T : UiComponent
    {
      if (applyStyles == null)
        return;
      applyStyles(component);
    }

    public static T Name<T>(this T component, string name) where T : IUiComponent
    {
      component.SetName(name);
      return component;
    }

    public static T Color<T>(this T component, ColorRgba? color) where T : IUiComponent
    {
      component.SetColor(color);
      return component;
    }

    public static T Enabled<T>(this T component, bool enabled) where T : IUiComponent
    {
      component.SetEnabled(enabled);
      return component;
    }

    public static T Tooltip<T>(this T component, LocStrFormatted? text, bool enabled = true) where T : UiComponent
    {
      if (!enabled || !text.HasValue)
        text = new LocStrFormatted?(LocStrFormatted.Empty);
      component.SetTooltipOrCreate(text.Value);
      return component;
    }

    /// <summary>
    /// This will prevent elements to pick input. Very useful for transparent
    /// fullscreen containers that only host children.
    /// </summary>
    public static T IgnoreInputPicking<T>(this T component, bool isIgnored = true, bool recursive = false) where T : IUiComponent
    {
      component.SetIgnoreInputPicking(isIgnored, recursive);
      return component;
    }

    /// <summary>
    /// Important: Even if hidden this element will still go through layout and will occupy space
    /// in its parent container. This is only useful in cases where you want to run layout on
    /// hidden items.
    /// </summary>
    /// <param name="isVisible">True to show, false to show</param>
    public static T VisibleForRender<T>(this T component, bool isVisible) where T : IUiComponent
    {
      component.SetVisibleForRender(isVisible);
      return component;
    }

    /// <summary>
    /// Determines whether this element will go through the layout (or will be absent).
    /// </summary>
    public static T Visible<T>(this T component, bool isVisible) where T : UiComponent
    {
      component.SetVisible(isVisible);
      return component;
    }

    public static T ToggleVisible<T>(this T component) where T : UiComponent
    {
      component.SetVisible(!component.IsVisible());
      return component;
    }

    /// <summary>Makes this element part of the layout.</summary>
    public static T Show<T>(this T component) where T : UiComponent
    {
      component.SetVisible(true);
      return component;
    }

    /// <summary>Makes this element absent from layout.</summary>
    public static T Hide<T>(this T component) where T : UiComponent
    {
      component.SetVisible(false);
      return component;
    }
  }
}
