// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentEventExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentEventExtensions
  {
    public static T OnMouseDown<T>(this T component, Action<MouseDownEvent> handler) where T : UiComponent
    {
      component.RegisterCallback<MouseDownEvent>((EventCallback<MouseDownEvent>) (evt => handler(evt)), TrickleDown.NoTrickleDown);
      return component;
    }

    public static T OnKeyDown<T>(this T component, Action<KeyDownEvent> handler) where T : UiComponent
    {
      component.SetFocusable(true);
      component.RegisterCallback<KeyDownEvent>((EventCallback<KeyDownEvent>) (evt => handler(evt)), TrickleDown.NoTrickleDown);
      return component;
    }

    /// <summary>
    /// Runs the given callback the first time this component renders in the UI.
    /// </summary>
    public static T OnFirstRender<T>(this T component, Action action) where T : UiComponent
    {
      UiComponentEventExtensions.registerGeoChanged((UiComponent) component, action, true);
      return component;
    }

    /// <summary>
    /// Runs the given callback each time this component is shown in the UI.
    /// </summary>
    public static T OnShow<T>(this T component, Action action) where T : UiComponent
    {
      UiComponentEventExtensions.registerGeoChanged((UiComponent) component, action);
      return component;
    }

    private static void registerGeoChanged(UiComponent component, Action action, bool once = false)
    {
      component.RegisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(handleGeoChanged));

      void handleGeoChanged(GeometryChangedEvent evt)
      {
        if ((double) evt.newRect.width <= 0.0 && (double) evt.newRect.height <= 0.0)
          return;
        if (once)
          component.UnregisterCallback<GeometryChangedEvent>(new EventCallback<GeometryChangedEvent>(handleGeoChanged));
        component.Schedule.Execute(action);
      }
    }

    public static T OnHide<T>(this T component, Action action) where T : UiComponent
    {
      component.RegisterCallback<GeometryChangedEvent>((EventCallback<GeometryChangedEvent>) (gce =>
      {
        if ((double) gce.newRect.width > 0.0 || (double) gce.newRect.height > 0.0)
          return;
        ((T) component).Schedule.Execute(action);
      }), TrickleDown.NoTrickleDown);
      return component;
    }
  }
}
