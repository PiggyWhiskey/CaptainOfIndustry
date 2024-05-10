// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCirclePlaneSlider2DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoCirclePlaneSlider2DController : GizmoPlaneSlider2DController
  {
    public GizmoCirclePlaneSlider2DController(GizmoPlaneSlider2DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.QuadBorder.SetVisible(false);
      this._data.SliderHandle.Set2DShapeVisible(this._data.QuadIndex, false);
      this._data.PolygonBorder.SetVisible(false);
      this._data.SliderHandle.Set2DShapeVisible(this._data.PolygonIndex, false);
      this._data.CircleBorder.SetVisible(this._data.Slider.IsBorderVisible);
      this._data.SliderHandle.Set2DShapeVisible(this._data.CircleIndex, this._data.Slider.IsVisible);
    }

    public override void UpdateEpsilons()
    {
      this._data.Circle.RadiusEps = this._data.Slider.Settings.AreaHoverEps;
    }

    public override void UpdateTransforms()
    {
      CircleShape2D circle = this._data.Circle;
      GizmoPlaneSlider2D slider = this._data.Slider;
      circle.Center = slider.Position;
      circle.RotationDegrees = slider.RotationDegrees;
      circle.Radius = slider.GetRealCircleRadius();
      this._data.CircleBorder.OnCircleShapeChanged();
    }

    public override Vector2 GetRealExtentPoint(Shape2DExtentPoint extentPt)
    {
      return this._data.Circle.GetExtentPoint(extentPt);
    }
  }
}
