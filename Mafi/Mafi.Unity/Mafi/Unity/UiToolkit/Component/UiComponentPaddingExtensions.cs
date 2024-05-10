// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentPaddingExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentPaddingExtensions
  {
    public static T PaddingLeft<T>(this T component, Px left) where T : IComponentWithPadding
    {
      IPaddingDecorator paddingDecorator = component.GetPaddingDecorator();
      Px? nullable = new Px?(left);
      Px? top = new Px?();
      Px? right = new Px?();
      Px? bottom = new Px?();
      Px? left1 = nullable;
      paddingDecorator.SetPadding(top, right, bottom, left1);
      return component;
    }

    public static T PaddingRight<T>(this T component, Px right) where T : IComponentWithPadding
    {
      IPaddingDecorator paddingDecorator = component.GetPaddingDecorator();
      Px? nullable = new Px?(right);
      Px? top = new Px?();
      Px? right1 = nullable;
      Px? bottom = new Px?();
      Px? left = new Px?();
      paddingDecorator.SetPadding(top, right1, bottom, left);
      return component;
    }

    public static T PaddingTop<T>(this T component, Px top) where T : IComponentWithPadding
    {
      component.GetPaddingDecorator().SetPadding(new Px?(top));
      return component;
    }

    public static T PaddingBottom<T>(this T component, Px bottom) where T : IComponentWithPadding
    {
      IPaddingDecorator paddingDecorator = component.GetPaddingDecorator();
      Px? nullable = new Px?(bottom);
      Px? top = new Px?();
      Px? right = new Px?();
      Px? bottom1 = nullable;
      Px? left = new Px?();
      paddingDecorator.SetPadding(top, right, bottom1, left);
      return component;
    }

    public static T PaddingLeftRight<T>(this T component, Px value) where T : IComponentWithPadding
    {
      IPaddingDecorator paddingDecorator = component.GetPaddingDecorator();
      Px? nullable1 = new Px?(value);
      Px? nullable2 = new Px?(value);
      Px? top = new Px?();
      Px? right = nullable2;
      Px? bottom = new Px?();
      Px? left = nullable1;
      paddingDecorator.SetPadding(top, right, bottom, left);
      return component;
    }

    public static T PaddingTopBottom<T>(this T component, Px value) where T : IComponentWithPadding
    {
      IPaddingDecorator paddingDecorator = component.GetPaddingDecorator();
      Px? top = new Px?(value);
      Px? nullable = new Px?(value);
      Px? right = new Px?();
      Px? bottom = nullable;
      Px? left = new Px?();
      paddingDecorator.SetPadding(top, right, bottom, left);
      return component;
    }

    public static T Padding<T>(this T component, Px topBottom, Px leftRight) where T : IComponentWithPadding
    {
      component.GetPaddingDecorator().SetPadding(new Px?(topBottom), new Px?(leftRight), new Px?(topBottom), new Px?(leftRight));
      return component;
    }

    public static T Padding<T>(this T component, Px all) where T : IComponentWithPadding
    {
      component.GetPaddingDecorator().SetPadding(new Px?(all), new Px?(all), new Px?(all), new Px?(all));
      return component;
    }

    public static T Padding<T>(this T component, Px top, Px right, Px bottom, Px left) where T : IComponentWithPadding
    {
      component.GetPaddingDecorator().SetPadding(new Px?(top), new Px?(right), new Px?(bottom), new Px?(left));
      return component;
    }
  }
}
