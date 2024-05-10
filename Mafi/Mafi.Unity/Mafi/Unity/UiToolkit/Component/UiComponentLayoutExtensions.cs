// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentLayoutExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentLayoutExtensions
  {
    public static T Size<T>(this T component, Percent width, Percent height) where T : IComponentWithLayout
    {
      return component.Width<T>(width).Height<T>(height);
    }

    public static T Size<T>(this T component, Percent size) where T : IComponentWithLayout
    {
      return component.Size<T>(size, size);
    }

    public static T Width<T>(this T component, Percent width) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetSize(new StyleLength?(UiComponentLayoutExtensions.percentToLength(new Percent?(width))));
      return component;
    }

    public static T WidthAuto<T>(this T component) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      StyleLength? nullable = new StyleLength?(new StyleLength(StyleKeyword.Auto));
      StyleLength? width = new StyleLength?();
      StyleLength? height = nullable;
      layoutDecorator.SetSize(width, height);
      return component;
    }

    public static T Height<T>(this T component, Percent height) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      StyleLength? nullable = new StyleLength?(UiComponentLayoutExtensions.percentToLength(new Percent?(height)));
      StyleLength? width = new StyleLength?();
      StyleLength? height1 = nullable;
      layoutDecorator.SetSize(width, height1);
      return component;
    }

    public static T HeightAuto<T>(this T component) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      StyleLength? nullable = new StyleLength?(new StyleLength(StyleKeyword.Auto));
      StyleLength? width = new StyleLength?();
      StyleLength? height = nullable;
      layoutDecorator.SetSize(width, height);
      return component;
    }

    public static T MinWidth<T>(this T component, Percent width) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMinWidth(UiComponentLayoutExtensions.percentToLength(new Percent?(width)));
      return component;
    }

    public static T MinHeight<T>(this T component, Percent height) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMinHeight(UiComponentLayoutExtensions.percentToLength(new Percent?(height)));
      return component;
    }

    public static T Size<T>(this T component, Px width, Px height) where T : IComponentWithLayout
    {
      return component.Width<T>(new Px?(width)).Height<T>(new Px?(height));
    }

    public static T Size<T>(this T component, Px size) where T : IComponentWithLayout
    {
      return component.Size<T>(size, size);
    }

    public static T Width<T>(this T component, Px? width) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetSize(new StyleLength?(UiComponentLayoutExtensions.pixelsToLength(width)));
      return component;
    }

    public static T Height<T>(this T component, Px? height) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      StyleLength? nullable = new StyleLength?(UiComponentLayoutExtensions.pixelsToLength(height));
      StyleLength? width = new StyleLength?();
      StyleLength? height1 = nullable;
      layoutDecorator.SetSize(width, height1);
      return component;
    }

    public static T MinWidth<T>(this T component, Px? width) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMinWidth(UiComponentLayoutExtensions.pixelsToLength(width));
      return component;
    }

    public static T MinHeight<T>(this T component, Px? height) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMinHeight(UiComponentLayoutExtensions.pixelsToLength(height));
      return component;
    }

    public static T MaxWidth<T>(this T component, Px width) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMaxSize(new StyleLength?(UiComponentLayoutExtensions.pixelsToLength(new Px?(width))));
      return component;
    }

    public static T MaxHeight<T>(this T component, Px height) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      StyleLength? nullable = new StyleLength?(UiComponentLayoutExtensions.pixelsToLength(new Px?(height)));
      StyleLength? width = new StyleLength?();
      StyleLength? height1 = nullable;
      layoutDecorator.SetMaxSize(width, height1);
      return component;
    }

    public static T MarginLeft<T>(this T component, Px left) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Px? nullable = new Px?(left);
      Px? top = new Px?();
      Px? right = new Px?();
      Px? bottom = new Px?();
      Px? left1 = nullable;
      layoutDecorator.SetMargin(top, right, bottom, left1);
      return component;
    }

    public static T MarginRight<T>(this T component, Px right) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Px? nullable = new Px?(right);
      Px? top = new Px?();
      Px? right1 = nullable;
      Px? bottom = new Px?();
      Px? left = new Px?();
      layoutDecorator.SetMargin(top, right1, bottom, left);
      return component;
    }

    public static T MarginTop<T>(this T component, Px top) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMargin(new Px?(top));
      return component;
    }

    public static T MarginBottom<T>(this T component, Px bottom) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Px? nullable = new Px?(bottom);
      Px? top = new Px?();
      Px? right = new Px?();
      Px? bottom1 = nullable;
      Px? left = new Px?();
      layoutDecorator.SetMargin(top, right, bottom1, left);
      return component;
    }

    public static T MarginLeftRight<T>(this T component, Px value) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Px? nullable1 = new Px?(value);
      Px? nullable2 = new Px?(value);
      Px? top = new Px?();
      Px? right = nullable2;
      Px? bottom = new Px?();
      Px? left = nullable1;
      layoutDecorator.SetMargin(top, right, bottom, left);
      return component;
    }

    public static T MarginTopBottom<T>(this T component, Px value) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Px? top = new Px?(value);
      Px? nullable = new Px?(value);
      Px? right = new Px?();
      Px? bottom = nullable;
      Px? left = new Px?();
      layoutDecorator.SetMargin(top, right, bottom, left);
      return component;
    }

    public static T Margin<T>(this T component, Px all) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMargin(new Px?(all), new Px?(all), new Px?(all), new Px?(all));
      return component;
    }

    public static T Margin<T>(this T component, Px top, Px right, Px bottom, Px left) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMargin(new Px?(top), new Px?(right), new Px?(bottom), new Px?(left));
      return component;
    }

    public static T MarginLeft<T>(this T component, Percent left) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Percent? nullable = new Percent?(left);
      Percent? top = new Percent?();
      Percent? right = new Percent?();
      Percent? bottom = new Percent?();
      Percent? left1 = nullable;
      layoutDecorator.SetMargin(top, right, bottom, left1);
      return component;
    }

    public static T MarginRight<T>(this T component, Percent right) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Percent? nullable = new Percent?(right);
      Percent? top = new Percent?();
      Percent? right1 = nullable;
      Percent? bottom = new Percent?();
      Percent? left = new Percent?();
      layoutDecorator.SetMargin(top, right1, bottom, left);
      return component;
    }

    public static T MarginTop<T>(this T component, Percent top) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMargin(new Percent?(top));
      return component;
    }

    public static T MarginBottom<T>(this T component, Percent bottom) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Percent? nullable = new Percent?(bottom);
      Percent? top = new Percent?();
      Percent? right = new Percent?();
      Percent? bottom1 = nullable;
      Percent? left = new Percent?();
      layoutDecorator.SetMargin(top, right, bottom1, left);
      return component;
    }

    public static T MarginLeftRight<T>(this T component, Percent value) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Percent? nullable1 = new Percent?(value);
      Percent? nullable2 = new Percent?(value);
      Percent? top = new Percent?();
      Percent? right = nullable2;
      Percent? bottom = new Percent?();
      Percent? left = nullable1;
      layoutDecorator.SetMargin(top, right, bottom, left);
      return component;
    }

    public static T MarginTopBottom<T>(this T component, Percent value) where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Percent? top = new Percent?(value);
      Percent? nullable = new Percent?(value);
      Percent? right = new Percent?();
      Percent? bottom = nullable;
      Percent? left = new Percent?();
      layoutDecorator.SetMargin(top, right, bottom, left);
      return component;
    }

    public static T Margin<T>(this T component, Percent all) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMargin(new Percent?(all), new Percent?(all), new Percent?(all), new Percent?(all));
      return component;
    }

    public static T Margin<T>(
      this T component,
      Percent top,
      Percent right,
      Percent bottom,
      Percent left)
      where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetMargin(new Percent?(top), new Percent?(right), new Percent?(bottom), new Percent?(left));
      return component;
    }

    public static T Fill<T>(this T component) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().Fill();
      return component;
    }

    public static T FlexGrow<T>(this T component, float flexGrow) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().FlexGrow(flexGrow);
      return component;
    }

    public static T FlexShrink<T>(this T component, float flexShrink) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().FlexShrink(flexShrink);
      return component;
    }

    public static T FlexNoShrink<T>(this T component) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().FlexShrink(0.0f);
      return component;
    }

    public static T AlignSelf<T>(this T component, Align alignSelf) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().AlignSelf(alignSelf);
      return component;
    }

    public static T RelativePosition<T>(this T component) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().RelativePosition();
      return component;
    }

    /// <summary>
    /// Will set absolute position. Any unset values will be set to None,
    /// relying on element's size.
    /// </summary>
    public static T AbsolutePosition<T>(
      this T component,
      Px? top = null,
      Px? right = null,
      Px? bottom = null,
      Px? left = null)
      where T : IComponentWithLayout
    {
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      Px? nullable = top;
      Px auto1 = Px.Auto;
      float? pixels;
      StyleLength styleLength1;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() == auto1 ? 1 : 0) : 0) == 0)
      {
        pixels = top?.Pixels;
        styleLength1 = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.Null);
      }
      else
        styleLength1 = (StyleLength) StyleKeyword.Auto;
      StyleLength? top1 = new StyleLength?(styleLength1);
      nullable = right;
      Px auto2 = Px.Auto;
      StyleLength styleLength2;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() == auto2 ? 1 : 0) : 0) == 0)
      {
        pixels = right?.Pixels;
        styleLength2 = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.Null);
      }
      else
        styleLength2 = (StyleLength) StyleKeyword.Auto;
      StyleLength? right1 = new StyleLength?(styleLength2);
      nullable = bottom;
      Px auto3 = Px.Auto;
      StyleLength styleLength3;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() == auto3 ? 1 : 0) : 0) == 0)
      {
        pixels = bottom?.Pixels;
        styleLength3 = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.Null);
      }
      else
        styleLength3 = (StyleLength) StyleKeyword.Auto;
      StyleLength? bottom1 = new StyleLength?(styleLength3);
      nullable = left;
      Px auto4 = Px.Auto;
      StyleLength styleLength4;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() == auto4 ? 1 : 0) : 0) == 0)
      {
        pixels = left?.Pixels;
        styleLength4 = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.Null);
      }
      else
        styleLength4 = (StyleLength) StyleKeyword.Auto;
      StyleLength? left1 = new StyleLength?(styleLength4);
      layoutDecorator.SetAbsolutePosition(top1, right1, bottom1, left1);
      return component;
    }

    /// <summary>
    /// Will set absolute position. Any unset values will be set to None,
    /// relying on element's size.
    /// </summary>
    public static T AbsolutePosition<T>(
      this T component,
      Percent? top = null,
      Percent? right = null,
      Percent? bottom = null,
      Percent? left = null)
      where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetAbsolutePosition(top.HasValue ? new StyleLength?(UiComponentLayoutExtensions.percentToLength(top)) : new StyleLength?(), right.HasValue ? new StyleLength?(UiComponentLayoutExtensions.percentToLength(right)) : new StyleLength?(), bottom.HasValue ? new StyleLength?(UiComponentLayoutExtensions.percentToLength(bottom)) : new StyleLength?(), left.HasValue ? new StyleLength?(UiComponentLayoutExtensions.percentToLength(left)) : new StyleLength?());
      return component;
    }

    /// <summary>
    /// Will fully stretch in the parent, no matter its own size.
    /// </summary>
    public static T AbsolutePositionFillParent<T>(this T component) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetAbsolutePosition(new StyleLength?((StyleLength) 0.0f), new StyleLength?((StyleLength) 0.0f), new StyleLength?((StyleLength) 0.0f), new StyleLength?((StyleLength) 0.0f));
      return component;
    }

    /// <summary>
    /// If size is set, this element will position itself in the parent based on its size
    /// and parent's alignment settings (align-items, justify-content).
    /// </summary>
    public static T AbsolutePositionAuto<T>(this T component) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetAbsolutePosition();
      return component;
    }

    /// <summary>
    /// Aligns self into the middle of its parent without relying on parents alignment configuration.
    /// Needs size to be set.
    /// </summary>
    public static T AbsolutePositionMiddle<T>(this T component, Px? left = null, Px? right = null) where T : IComponentWithLayout
    {
      component.GetClassDecorator().AddClass(Cls.absoluteMiddle);
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      StyleLength? nullable1;
      ref StyleLength? local1 = ref nullable1;
      float? pixels = left?.Pixels;
      StyleLength styleLength1 = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.None);
      local1 = new StyleLength?(styleLength1);
      StyleLength? nullable2;
      ref StyleLength? local2 = ref nullable2;
      pixels = right?.Pixels;
      StyleLength styleLength2 = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.None);
      local2 = new StyleLength?(styleLength2);
      StyleLength? top = new StyleLength?();
      StyleLength? right1 = nullable2;
      StyleLength? bottom = new StyleLength?();
      StyleLength? left1 = nullable1;
      layoutDecorator.SetAbsolutePosition(top, right1, bottom, left1);
      return component;
    }

    /// <summary>
    /// Aligns self into the center of its parent without relying on parents alignment configuration.
    /// Needs size to be set.
    /// </summary>
    public static T AbsolutePositionCenter<T>(this T component, Px? top = null, Px? bottom = null) where T : IComponentWithLayout
    {
      component.GetClassDecorator().AddClass(Cls.absoluteCenter);
      ILayoutDecorator layoutDecorator = component.GetLayoutDecorator();
      float? pixels = top?.Pixels;
      StyleLength? top1 = new StyleLength?(pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.None));
      StyleLength? nullable;
      ref StyleLength? local = ref nullable;
      pixels = bottom?.Pixels;
      StyleLength styleLength = pixels.HasValue ? (StyleLength) pixels.GetValueOrDefault() : new StyleLength(StyleKeyword.None);
      local = new StyleLength?(styleLength);
      StyleLength? right = new StyleLength?();
      StyleLength? bottom1 = nullable;
      StyleLength? left = new StyleLength?();
      layoutDecorator.SetAbsolutePosition(top1, right, bottom1, left);
      return component;
    }

    /// <summary>
    /// Aligns self into the center, middle of the parent without relying on parents alignment configuration.
    /// Needs size to be set.
    /// </summary>
    public static T AbsolutePositionCenterMiddle<T>(this T component) where T : IComponentWithLayout
    {
      component.GetClassDecorator().AddClass(Cls.absoluteCenterMiddle);
      return component;
    }

    public static T Opacity<T>(this T component, float opacity) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().Opacity(opacity);
      return component;
    }

    public static T Rotate<T>(this T component, int? degrees) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetRotate(degrees);
      return component;
    }

    public static T Scale<T>(this T component, float x = 1f, float y = 1f) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetScale(x, y);
      return component;
    }

    public static T Translate<T>(this T component, int x = 0, int y = 0) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetTranslate(x, y);
      return component;
    }

    public static T Translate<T>(this T component, Percent x = default (Percent), Percent y = default (Percent)) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetTranslate(x, y);
      return component;
    }

    public static T TransformOrigin<T>(this T component, Percent x, Percent y) where T : IComponentWithLayout
    {
      component.GetLayoutDecorator().SetTransformOrigin(x, y);
      return component;
    }

    private static StyleLength percentToLength(Percent? perc)
    {
      return perc.HasValue ? new StyleLength(new Length(perc.Value.ToFloat() * 100f, LengthUnit.Percent)) : new StyleLength(StyleKeyword.Null);
    }

    private static StyleLength pixelsToLength(Px? px)
    {
      Px? nullable = px;
      Px auto = Px.Auto;
      if ((nullable.HasValue ? (nullable.GetValueOrDefault() == auto ? 1 : 0) : 0) != 0)
        return new StyleLength(StyleKeyword.Auto);
      return px.HasValue ? new StyleLength(new Length(px.Value.Pixels, LengthUnit.Pixel)) : new StyleLength(StyleKeyword.Null);
    }
  }
}
