// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.ButtonExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class ButtonExtensions
  {
    public static T OnClick<T>(this T component, Action<T> onClick) where T : IButtonComponent
    {
      ref T local = ref component;
      if ((object) default (T) == null)
      {
        T obj = local;
        local = ref obj;
      }
      Action onClick1 = (Action) (() => onClick(component));
      local.SetOnClickAction(onClick1);
      return component;
    }

    public static T OnClick<T>(this T component, [CanBeNull] Action onClick) where T : IButtonComponent
    {
      component.SetOnClickAction(onClick);
      return component;
    }

    public static T OnDoubleClick<T>(this T component, Action onDoubleClick) where T : IButtonComponent
    {
      component.SetOnDoubleClickAction(onDoubleClick);
      return component;
    }

    public static bool IsSelected<T>(this T component) where T : IButtonComponent
    {
      return component.GetButtonDecorator().IsSelected();
    }

    public static T Selected<T>(this T component, bool isSelected = true) where T : IButtonComponent
    {
      component.GetButtonDecorator().SetSelected(isSelected);
      return component;
    }

    public static T Toggle<T>(this T component, bool toggle = true) where T : IButtonComponent
    {
      component.GetButtonDecorator().SetToggle(toggle);
      return component;
    }

    public static T CustomClickSound<T>(this T component, string pathToSound) where T : IButtonComponent
    {
      component.SetCustomClickSound(pathToSound);
      return component;
    }

    public static T Variant<T>(this T component, ButtonVariant variant) where T : IButtonComponent
    {
      component.SetVariant(variant);
      return component;
    }

    public static T FlipNotches<T>(this T component) where T : IButtonComponent, UiComponentDecorated<Button>
    {
      component.WrapClass(Cls.flipNotches, true);
      return component;
    }
  }
}
