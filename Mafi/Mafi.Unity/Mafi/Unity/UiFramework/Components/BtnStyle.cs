// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.BtnStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework.Styles;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public struct BtnStyle
  {
    public TextStyle Text;
    public BorderStyle Border;
    public ColorRgba? BackgroundClr;
    public ColorRgba? NormalMaskClr;
    public ColorRgba? HoveredMaskClr;
    public ColorRgba? PressedMaskClr;
    public ColorRgba? DisabledMaskClr;
    public ColorRgba? ForegroundClrWhenDisabled;
    public ColorRgba? BackgroundClrWhenDisabled;
    public bool Shadow;
    public readonly int Width;
    public readonly int Height;
    public readonly int SidePaddings;
    public readonly Offset? IconPadding;

    public Vector2 Size => new Vector2((float) this.Width, (float) this.Height);

    public BtnStyle(
      TextStyle? text = null,
      BorderStyle? border = null,
      ColorRgba? backgroundClr = null,
      ColorRgba? normalMaskClr = null,
      ColorRgba? hoveredMaskClr = null,
      ColorRgba? pressedMaskClr = null,
      ColorRgba? disabledMaskClr = null,
      ColorRgba? foregroundClrWhenDisabled = null,
      ColorRgba? backgroundClrWhenDisabled = null,
      bool shadow = false,
      int width = 0,
      int height = 0,
      int sidePaddings = 0,
      Offset? iconPadding = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Text = text ?? TextStyle.DEFAULT;
      this.Border = border ?? BorderStyle.DEFAULT;
      this.BackgroundClr = backgroundClr;
      this.NormalMaskClr = normalMaskClr;
      this.HoveredMaskClr = hoveredMaskClr;
      this.PressedMaskClr = pressedMaskClr;
      this.DisabledMaskClr = disabledMaskClr;
      this.ForegroundClrWhenDisabled = foregroundClrWhenDisabled;
      this.BackgroundClrWhenDisabled = backgroundClrWhenDisabled;
      this.Shadow = shadow;
      this.Width = width;
      this.Height = height;
      this.SidePaddings = sidePaddings;
      this.IconPadding = iconPadding;
    }

    [Pure]
    public BtnStyle Extend(
      TextStyle? text = null,
      BorderStyle? border = null,
      ColorRgba? backgroundClr = null,
      ColorRgba? normalMaskClr = null,
      ColorRgba? hoveredClr = null,
      ColorRgba? pressedClr = null,
      ColorRgba? disabledMaskClr = null,
      ColorRgba? foregroundClrWhenDisabled = null,
      ColorRgba? backgroundClrWhenDisabled = null,
      bool? shadow = null,
      int? width = null,
      int? height = null,
      int? sidePaddings = null,
      Offset? iconPadding = null)
    {
      return new BtnStyle(new TextStyle?(text ?? this.Text), new BorderStyle?(border ?? this.Border), backgroundClr ?? this.BackgroundClr, normalMaskClr ?? this.NormalMaskClr, hoveredClr ?? this.HoveredMaskClr, pressedClr ?? this.PressedMaskClr, disabledMaskClr ?? this.DisabledMaskClr, foregroundClrWhenDisabled ?? this.ForegroundClrWhenDisabled, backgroundClrWhenDisabled ?? this.BackgroundClrWhenDisabled, ((int) shadow ?? (this.Shadow ? 1 : 0)) != 0, width ?? this.Width, height ?? this.Height, sidePaddings ?? this.SidePaddings, iconPadding ?? this.IconPadding);
    }

    [Pure]
    public BtnStyle ExtendText(
      ColorRgba? color = null,
      FontStyle? fontStyle = null,
      int? fontSize = null,
      bool? isCapitalized = null)
    {
      return this.Extend(new TextStyle?(this.Text.Extend(color, fontStyle, fontSize, isCapitalized)));
    }
  }
}
