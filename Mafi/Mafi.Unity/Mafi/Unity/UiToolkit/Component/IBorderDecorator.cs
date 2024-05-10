// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Component.IBorderDecorator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace Mafi.Unity.UiToolkit.Component
{
  public interface IBorderDecorator
  {
    void SetBorderLeft(int left, ColorRgba? topColor = null);

    void SetBorderRight(int right, ColorRgba? topColor = null);

    void SetBorderTop(int top, ColorRgba? topColor = null);

    void SetBorderBottom(int bottom, ColorRgba? topColor = null);

    void SetBorderRadius(float? topLeft, float? topRight, float? bottomRight, float? bottomLeft);

    void SetBorder(int all = -2147483648, ColorRgba? color = null);
  }
}
