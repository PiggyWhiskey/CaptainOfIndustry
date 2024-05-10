// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.UiComponentFontExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class UiComponentFontExtensions
  {
    public static T FontStyle<T>(this T component, Mafi.Unity.UiToolkit.Component.FontStyle style) where T : IComponentWithFont
    {
      component.GetFontDecorator().SetFontStyle(style);
      return component;
    }

    public static T FontSize<T>(this T component, int fontSize) where T : IComponentWithFont
    {
      component.GetFontDecorator().SetFontSize(fontSize);
      return component;
    }

    public static T FontSize<T>(this T component, Px fontSize) where T : IComponentWithFont
    {
      component.GetFontDecorator().SetFontSize(fontSize.Pixels.RoundToInt());
      return component;
    }

    public static T FontBold<T>(this T component) where T : IComponentWithFont
    {
      return component.FontStyle<T>(Mafi.Unity.UiToolkit.Component.FontStyle.Bold);
    }

    public static T FontItalic<T>(this T component) where T : IComponentWithFont
    {
      return component.FontStyle<T>(Mafi.Unity.UiToolkit.Component.FontStyle.Italic);
    }

    public static T FontBoldItalic<T>(this T component) where T : IComponentWithFont
    {
      return component.FontStyle<T>(Mafi.Unity.UiToolkit.Component.FontStyle.BoldItalic);
    }

    public static T FontNormal<T>(this T component) where T : IComponentWithFont
    {
      return component.FontStyle<T>(Mafi.Unity.UiToolkit.Component.FontStyle.Normal);
    }

    public static T AlignText<T>(this T component, TextAlign align) where T : IComponentWithFont
    {
      component.GetFontDecorator().SetTextAlign(align);
      return component;
    }

    public static T AlignTextCenter<T>(this T component) where T : IComponentWithFont
    {
      component.GetFontDecorator().SetTextAlign(TextAlign.CenterMiddle);
      return component;
    }
  }
}
