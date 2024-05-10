// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.SliderStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Styles;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public struct SliderStyle
  {
    public ColorRgba BgColor;
    public ColorRgba FillColor;
    public ColorRgba HandleColor;
    /// <summary>Used for background, fill and handle.</summary>
    public SlicedSpriteStyle BgSprite;
    /// <summary>Note that it must be dividable by 2.</summary>
    public int HandleWidth;
  }
}
