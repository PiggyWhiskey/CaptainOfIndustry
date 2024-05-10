// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UpdaterExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiToolkit.Library;
using System;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UpdaterExtensions
  {
    public static T Apply<T>(this T target, Action<T> toApply) where T : UiComponent
    {
      toApply(target);
      return target;
    }

    public static T SelectedObserve<T>(
      this T component,
      UpdaterBuilder builder,
      Func<bool> selectedObserver)
      where T : IButtonComponent
    {
      builder.Observe<bool>(selectedObserver).Do((Action<bool>) (selected => component.Selected<T>(selected)));
      return component;
    }

    public static T VisibleObserve<T>(
      this T component,
      UpdaterBuilder builder,
      Func<bool> visibleObserver)
      where T : UiComponent
    {
      builder.Observe<bool>(visibleObserver).Do((Action<bool>) (visible => component.Visible<T>(visible)));
      return component;
    }

    public static T VisibleForRenderObserve<T>(
      this T component,
      UpdaterBuilder builder,
      Func<bool> visibleObserver)
      where T : UiComponent
    {
      builder.Observe<bool>(visibleObserver).Do((Action<bool>) (visible => component.VisibleForRender<T>(visible)));
      return component;
    }

    public static Dropdown<T> ValueIndexObserve<T>(
      this Dropdown<T> component,
      UpdaterBuilder builder,
      Func<int> indexObserver)
    {
      builder.Observe<int>(indexObserver).Do((Action<int>) (index => component.SetValueIndex(index)));
      return component;
    }

    public static T ValueObserve<T>(
      this T component,
      UpdaterBuilder builder,
      Func<bool> valueObserver)
      where T : Toggle
    {
      builder.Observe<bool>(valueObserver).Do((Action<bool>) (value => ((T) component).Value(value)));
      return component;
    }

    public static T ValueObserve<T>(
      this T component,
      UpdaterBuilder builder,
      Func<string> observer)
      where T : IComponentWithText
    {
      builder.Observe<string>(observer).Do((Action<string>) (v =>
      {
        ref T local = ref component;
        if ((object) default (T) == null)
        {
          T obj = local;
          local = ref obj;
        }
        LocStrFormatted text = v.AsLoc();
        local.SetText(text);
      }));
      return component;
    }

    public static T ErrorObserve<T>(
      this T component,
      UpdaterBuilder builder,
      Func<LocStrFormatted> observer)
      where T : TxtField
    {
      builder.Observe<LocStrFormatted>(observer).Do((Action<LocStrFormatted>) (str => ((T) component).MarkAsError(!str.IsEmptyOrNull, str)));
      return component;
    }

    public static T EnabledObserve<T>(
      this T component,
      UpdaterBuilder builder,
      Func<bool> observer)
      where T : UiComponent
    {
      builder.Observe<bool>(observer).Do((Action<bool>) (enabled => component.Enabled<T>(enabled)));
      return component;
    }

    public static T ConfirmRequiredObserve<T>(
      this T component,
      UpdaterBuilder builder,
      Func<bool> observer)
      where T : ConfirmButton
    {
      builder.Observe<bool>(observer).Do((Action<bool>) (required => ((T) component).ConfirmRequired(required)));
      return component;
    }
  }
}
