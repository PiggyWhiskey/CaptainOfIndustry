// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.ChartStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public sealed class ChartStyle
  {
    public readonly float Width;
    public readonly float Height;
    public readonly float HorizontalAxisHeight;
    public readonly float VerticalAxisWidth;
    public readonly float LineWidth;
    /// <summary>Diameter of points.</summary>
    public readonly float PointSize;
    public readonly float PointHighlightSize;
    public readonly ColorRgba BackgroundColor;

    public float DataViewWidth => this.Width - this.VerticalAxisWidth;

    public float DataViewHeight => this.Height - this.HorizontalAxisHeight;

    public ChartStyle(
      float width,
      float height,
      float lineWidth,
      float pointSize,
      float pointHighlightSize,
      float horizontalAxisHeight,
      float verticalAxisWidth,
      ColorRgba backgroundColor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Width = width;
      this.Height = height;
      this.LineWidth = lineWidth;
      this.PointSize = pointSize;
      this.PointHighlightSize = pointHighlightSize;
      this.HorizontalAxisHeight = horizontalAxisHeight;
      this.VerticalAxisWidth = verticalAxisWidth;
      this.BackgroundColor = backgroundColor;
    }
  }
}
