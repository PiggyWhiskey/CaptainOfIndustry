// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentBackgroundExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentBackgroundExtensions
  {
    public static T Background<T>(this T component, ColorRgba? color) where T : IComponentWithBackground
    {
      component.GetBackgroundDecorator().SetBackground(color);
      return component;
    }

    public static T Background<T>(this T component, [CanBeNull] string imageAssetPath, ColorRgba tintColor = default (ColorRgba)) where T : IComponentWithBackground
    {
      component.GetBackgroundDecorator().SetBackground((IComponentWithBackground) component, imageAssetPath, tintColor);
      return component;
    }

    public static T BackgroundCover<T>(this T component) where T : IComponentWithBackground
    {
      component.GetBackgroundDecorator().SetBackgroundScale(BackgroundSizeType.Cover);
      return component;
    }

    public static T BackgroundFit<T>(this T component) where T : IComponentWithBackground
    {
      component.GetBackgroundDecorator().SetBackgroundScale(BackgroundSizeType.Contain);
      return component;
    }
  }
}
