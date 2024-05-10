// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.ButtonVariantExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Library;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class ButtonVariantExtensions
  {
    public static string ToClass(this ButtonVariant variant)
    {
      switch (variant)
      {
        case ButtonVariant.Default:
          return Cls.btn_default;
        case ButtonVariant.Primary:
          return Cls.btn_primary;
        case ButtonVariant.Boxy:
          return Cls.btn_boxy;
        case ButtonVariant.Area:
          return Cls.btn_area;
        default:
          return (string) null;
      }
    }

    public static Option<Outer> ToOuter(this ButtonVariant variant)
    {
      switch (variant)
      {
        case ButtonVariant.Default:
        case ButtonVariant.Primary:
          return (Option<Outer>) Outer.ShadowCutCorner;
        case ButtonVariant.Boxy:
          return (Option<Outer>) Outer.ShadowAll;
        default:
          return (Option<Outer>) Option.None;
      }
    }
  }
}
