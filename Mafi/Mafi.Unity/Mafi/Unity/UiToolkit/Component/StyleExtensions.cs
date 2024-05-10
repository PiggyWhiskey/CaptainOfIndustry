// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.StyleExtensions
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using UnityEngine.UIElements;

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public static class StyleExtensions
  {
    public static FlexDirection ToUnity(this LayoutDirection value)
    {
      switch (value)
      {
        case LayoutDirection.Row:
          return FlexDirection.Row;
        case LayoutDirection.Column:
          return FlexDirection.Column;
        case LayoutDirection.RowReverse:
          return FlexDirection.RowReverse;
        case LayoutDirection.ColumnReverse:
          return FlexDirection.ColumnReverse;
        default:
          return FlexDirection.Row;
      }
    }

    public static UnityEngine.UIElements.Align ToUnity(this Align value)
    {
      switch (value)
      {
        case Align.Start:
          return UnityEngine.UIElements.Align.FlexStart;
        case Align.Center:
          return UnityEngine.UIElements.Align.Center;
        case Align.End:
          return UnityEngine.UIElements.Align.FlexEnd;
        case Align.Stretch:
          return UnityEngine.UIElements.Align.Stretch;
        case Align.Auto:
          return UnityEngine.UIElements.Align.Auto;
        default:
          return UnityEngine.UIElements.Align.FlexStart;
      }
    }

    public static UnityEngine.UIElements.Justify ToUnity(this Justify value)
    {
      switch (value)
      {
        case Justify.Start:
          return UnityEngine.UIElements.Justify.FlexStart;
        case Justify.Center:
          return UnityEngine.UIElements.Justify.Center;
        case Justify.End:
          return UnityEngine.UIElements.Justify.FlexEnd;
        case Justify.SpaceAround:
          return UnityEngine.UIElements.Justify.SpaceAround;
        case Justify.SpaceBetween:
          return UnityEngine.UIElements.Justify.SpaceBetween;
        default:
          return UnityEngine.UIElements.Justify.FlexStart;
      }
    }

    public static UnityEngine.FontStyle ToUnity(this FontStyle value)
    {
      switch (value)
      {
        case FontStyle.Normal:
          return UnityEngine.FontStyle.Normal;
        case FontStyle.Bold:
          return UnityEngine.FontStyle.Bold;
        case FontStyle.Italic:
          return UnityEngine.FontStyle.Italic;
        case FontStyle.BoldItalic:
          return UnityEngine.FontStyle.BoldAndItalic;
        default:
          return UnityEngine.FontStyle.Normal;
      }
    }

    public static LengthUnit ToUnity(this UnitType value)
    {
      return value == UnitType.Pixel || value != UnitType.Percent ? LengthUnit.Pixel : LengthUnit.Percent;
    }

    public static TextAnchor ToUnity(this TextAlign value)
    {
      switch (value)
      {
        case TextAlign.LeftTop:
          return TextAnchor.UpperLeft;
        case TextAlign.CenterTop:
          return TextAnchor.UpperCenter;
        case TextAlign.RightTop:
          return TextAnchor.UpperRight;
        case TextAlign.LeftMiddle:
          return TextAnchor.MiddleLeft;
        case TextAlign.CenterMiddle:
          return TextAnchor.MiddleCenter;
        case TextAlign.RightMiddle:
          return TextAnchor.MiddleRight;
        case TextAlign.LeftBottom:
          return TextAnchor.LowerLeft;
        case TextAlign.CenterBottom:
          return TextAnchor.LowerCenter;
        case TextAlign.RightBottom:
          return TextAnchor.LowerRight;
        default:
          return TextAnchor.UpperLeft;
      }
    }
  }
}
