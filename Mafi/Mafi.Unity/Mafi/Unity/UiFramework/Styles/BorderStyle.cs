// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Styles.BorderStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Styles
{
  public struct BorderStyle
  {
    public static readonly BorderStyle DEFAULT;
    public readonly ColorRgba Color;
    public readonly float Thickness;

    public BorderStyle(ColorRgba borderColor, float borderSize = 1f)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.Color = borderColor;
      this.Thickness = borderSize.CheckNotNegative();
    }

    static BorderStyle()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BorderStyle.DEFAULT = new BorderStyle(ColorRgba.Black, 0.0f);
    }
  }
}
