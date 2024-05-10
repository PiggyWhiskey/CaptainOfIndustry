// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.TextStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components
{
  public struct TextStyle
  {
    public static readonly TextStyle DEFAULT;
    public ColorRgba Color;
    public FontStyle FontStyle;
    public int FontSize;
    public bool IsCapitalized;

    public TextStyle(ColorRgba color, int fontSize = 12, FontStyle? fontStyle = null, bool isCapitalized = false)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Color = color;
      this.FontStyle = fontStyle.GetValueOrDefault();
      this.FontSize = fontSize;
      this.IsCapitalized = isCapitalized;
    }

    [Pure]
    public TextStyle Extend(
      ColorRgba? color = null,
      FontStyle? fontStyle = null,
      int? fontSize = null,
      bool? isCapitalized = null)
    {
      return new TextStyle()
      {
        Color = color ?? this.Color,
        FontStyle = (FontStyle) ((int) fontStyle ?? (int) this.FontStyle),
        FontSize = fontSize ?? this.FontSize,
        IsCapitalized = ((int) isCapitalized ?? (this.IsCapitalized ? 1 : 0)) != 0
      };
    }

    static TextStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TextStyle.DEFAULT = new TextStyle(ColorRgba.Black);
    }
  }
}
