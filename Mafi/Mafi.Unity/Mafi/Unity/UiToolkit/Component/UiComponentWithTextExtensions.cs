// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentWithTextExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Localization;
using UnityEngine;
using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentWithTextExtensions
  {
    public static T Text<T>(this T component, LocStrFormatted text) where T : IComponentWithText
    {
      component.SetText(text);
      return component;
    }

    public static T TextOverflow<T>(this T component, Mafi.Unity.UiToolkit.Component.TextOverflow overflow) where T : IComponentWithText
    {
      component.SetTextOverflow(overflow);
      return component;
    }

    public static T Label<T>(this T component, LocStrFormatted text) where T : IComponentWithLabel
    {
      component.SetLabel(text);
      return component;
    }

    public static T LabelWidth<T>(this T component, Percent width) where T : IComponentWithLabel
    {
      component.SetLabelWidth(width);
      return component;
    }

    public static T LabelWidth<T>(this T component, Px width) where T : IComponentWithLabel
    {
      component.SetLabelWidth(width);
      return component;
    }

    /// <summary>
    /// Sets up text shadow. Call with no arguments for the default shadow, or pass ColorRgba.Empty to clear.
    /// </summary>
    public static T TextShadow<T>(
      this T component,
      ColorRgba? color = null,
      float x = 1f,
      float y = 1f,
      float blur = 2f)
      where T : UiComponent
    {
      ColorRgba? nullable = color;
      ColorRgba empty = ColorRgba.Empty;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() == empty ? 1 : 0) : 0) != 0)
        component.RootElement.style.textShadow = (StyleTextShadow) StyleKeyword.None;
      else
        component.RootElement.style.textShadow = (StyleTextShadow) new UnityEngine.UIElements.TextShadow()
        {
          color = (color ?? ColorRgba.Black).ToColor(),
          offset = new Vector2(x, y),
          blurRadius = blur
        };
      return component;
    }
  }
}
