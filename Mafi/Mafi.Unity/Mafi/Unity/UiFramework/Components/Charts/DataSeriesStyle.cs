// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.DataSeriesStyle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public struct DataSeriesStyle
  {
    public readonly Material PointsMaterial;
    public readonly Material PointsHighlightMaterial;
    public readonly Material LineMaterial;
    public readonly ColorRgba PointsColor;
    public readonly ColorRgba LineColor;
    public readonly Option<string> IconPath;

    public DataSeriesStyle(
      Material pointsMaterial,
      Material pointsHighlightMaterial,
      Material lineMaterial,
      ColorRgba pointsColor,
      ColorRgba lineColor,
      string iconPath = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.PointsColor = pointsColor;
      this.LineColor = lineColor;
      this.PointsMaterial = pointsMaterial.CheckNotNull<Material>();
      this.PointsHighlightMaterial = pointsHighlightMaterial.CheckNotNull<Material>();
      this.LineMaterial = lineMaterial.CheckNotNull<Material>();
      this.IconPath = (Option<string>) iconPath;
    }
  }
}
