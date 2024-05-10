// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Themes.StyleExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;

#nullable disable
namespace Mafi.Unity.UiToolkit.Themes
{
  public static class StyleExtensions
  {
    public static T BorderDarkAll<T>(this T component) where T : UiComponent
    {
      component.Border<T>(1, new ColorRgba?(ColorRgba.Black));
      return component;
    }

    public static T BorderDarkR<T>(this T component) where T : UiComponent
    {
      component.BorderRight<T>(1, new ColorRgba?(ColorRgba.Black));
      return component;
    }
  }
}
