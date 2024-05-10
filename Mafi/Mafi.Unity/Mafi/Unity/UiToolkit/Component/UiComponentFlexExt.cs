// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentFlexExt
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentFlexExt
  {
    private static T AlignItems<T>(this T component, Align align) where T : IFlexComponent
    {
      component.GetFlexDecorator().AlignItems(align);
      return component;
    }

    public static T AlignItemsStart<T>(this T component) where T : IFlexComponent
    {
      return component.AlignItems<T>(Align.Start);
    }

    public static T AlignItemsCenter<T>(this T component) where T : IFlexComponent
    {
      return component.AlignItems<T>(Align.Center);
    }

    public static T AlignItemsEnd<T>(this T component) where T : IFlexComponent
    {
      return component.AlignItems<T>(Align.End);
    }

    public static T AlignItemsStretch<T>(this T component) where T : IFlexComponent
    {
      return component.AlignItems<T>(Align.Stretch);
    }

    public static T AlignItemsAuto<T>(this T component) where T : IFlexComponent
    {
      return component.AlignItems<T>(Align.Auto);
    }

    public static T OverflowHidden<T>(this T component) where T : UiComponent
    {
      if (component is IFlexComponent flexComponent)
        flexComponent.GetFlexDecorator().SetOverflow(Overflow.Hidden);
      else
        component.RootElement.style.overflow = (StyleEnum<Overflow>) Overflow.Hidden;
      return component;
    }

    public static T OverflowVisible<T>(this T component) where T : IFlexComponent
    {
      component.GetFlexDecorator().SetOverflow(Overflow.Visible);
      return component;
    }

    private static T Justify<T>(this T component, Mafi.Unity.UiToolkit.Component.Justify justify) where T : IFlexComponent
    {
      component.GetFlexDecorator().JustifyItems(justify);
      return component;
    }

    public static T JustifyItemsStart<T>(this T component) where T : IFlexComponent
    {
      return component.Justify<T>(Mafi.Unity.UiToolkit.Component.Justify.Start);
    }

    public static T JustifyItemsEnd<T>(this T component) where T : IFlexComponent
    {
      return component.Justify<T>(Mafi.Unity.UiToolkit.Component.Justify.End);
    }

    public static T JustifyItemsCenter<T>(this T component) where T : IFlexComponent
    {
      return component.Justify<T>(Mafi.Unity.UiToolkit.Component.Justify.Center);
    }

    public static T JustifyItemsSpaceAround<T>(this T component) where T : IFlexComponent
    {
      return component.Justify<T>(Mafi.Unity.UiToolkit.Component.Justify.SpaceAround);
    }

    public static T JustifyItemsSpaceBetween<T>(this T component) where T : IFlexComponent
    {
      return component.Justify<T>(Mafi.Unity.UiToolkit.Component.Justify.SpaceBetween);
    }

    public static T AlignItemsCenterMiddle<T>(this T component) where T : IFlexComponent
    {
      return component.AlignItemsCenter<T>().JustifyItemsCenter<T>();
    }

    public static T Wrap<T>(this T component, bool wrap = true) where T : IFlexComponent
    {
      component.GetFlexDecorator().Wrap(wrap);
      return component;
    }

    public static T Gap<T>(this T component, Px? mainAxis = null, Px? crossAxis = null) where T : UiComponent, IFlexComponent
    {
      component.GetFlexDecorator().SetGap<T>(component, mainAxis, crossAxis);
      return component;
    }

    public static T AlignGridContent<T>(this T component, Align alignContent) where T : IFlexComponent
    {
      component.GetFlexDecorator().AlignGridContent(alignContent);
      return component;
    }

    public static T Direction<T>(this T component, LayoutDirection direction) where T : IFlexComponent
    {
      component.GetFlexDecorator().Direction(direction);
      return component;
    }

    public static T ReversedDirection<T>(this T component) where T : IFlexComponent
    {
      component.GetFlexDecorator().SetReversedDirection((IFlexComponent) component);
      return component;
    }
  }
}
