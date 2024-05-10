// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentBorderExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentBorderExtensions
  {
    public static T BorderColor<T>(this T component, ColorRgba color) where T : IComponentWithBorder
    {
      component.GetBorderDecorator().SetBorder(color: new ColorRgba?(color));
      return component;
    }

    public static T Border<T>(this T component, int all, ColorRgba? color = null) where T : IComponentWithBorder
    {
      component.GetBorderDecorator().SetBorder(all, color);
      return component;
    }

    public static T BorderLeft<T>(this T component, int left, ColorRgba? color = null) where T : IComponentWithBorder
    {
      component.GetBorderDecorator().SetBorderLeft(left, color);
      return component;
    }

    public static T BorderRight<T>(this T component, int right, ColorRgba? color = null) where T : IComponentWithBorder
    {
      component.GetBorderDecorator().SetBorderRight(right, color);
      return component;
    }

    public static T BorderTop<T>(this T component, int top, ColorRgba? color = null) where T : IComponentWithBorder
    {
      component.GetBorderDecorator().SetBorderTop(top, color);
      return component;
    }

    public static T BorderBottom<T>(this T component, int bottom, ColorRgba? color = null) where T : IComponentWithBorder
    {
      component.GetBorderDecorator().SetBorderBottom(bottom, color);
      return component;
    }

    public static T Border<T>(this T component, int top, int right, int bottom, int left) where T : IComponentWithBorder
    {
      IBorderDecorator borderDecorator = component.GetBorderDecorator();
      borderDecorator.SetBorderTop(top);
      borderDecorator.SetBorderRight(right);
      borderDecorator.SetBorderBottom(bottom);
      borderDecorator.SetBorderLeft(left);
      return component;
    }

    public static T BorderRadius<T>(this T component, Px radius) where T : IComponentWithBorder
    {
      component.GetBorderDecorator().SetBorderRadius(new float?(radius.Pixels), new float?(radius.Pixels), new float?(radius.Pixels), new float?(radius.Pixels));
      return component;
    }

    public static T BorderRadius<T>(this T component, Px? top = null, Px? bottom = null, Px? left = null, Px? right = null) where T : IComponentWithBorder
    {
      IBorderDecorator borderDecorator = component.GetBorderDecorator();
      if (top.HasValue || bottom.HasValue)
        borderDecorator.SetBorderRadius(top?.Pixels, top?.Pixels, bottom?.Pixels, bottom?.Pixels);
      else
        borderDecorator.SetBorderRadius(left?.Pixels, right?.Pixels, right?.Pixels, left?.Pixels);
      return component;
    }
  }
}
