// Decompiled with JetBrains decompiler
// Type: RTG.GizmoRATrianglePlaneSlider3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoRATrianglePlaneSlider3DController : GizmoPlaneSlider3DController
  {
    public GizmoRATrianglePlaneSlider3DController(GizmoPlaneSlider3DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.QuadBorder.SetVisible(false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.QuadIndex, false);
      this._data.CircleBorder.SetVisible(false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.CircleIndex, false);
      this._data.RATriangleBorder.SetVisible(this._data.Slider.IsBorderVisible);
      this._data.SliderHandle.Set3DShapeVisible(this._data.RATriangleIndex, this._data.Slider.IsVisible);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.RATriangle.AreaEps = this._data.Slider.Settings.AreaHoverEps * zoomFactor;
      this._data.RATriangle.ExtrudeEps = this._data.Slider.Settings.ExtrudeHoverEps * zoomFactor;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      RightAngTriangle3D raTriangle = this._data.RATriangle;
      GizmoPlaneSlider3D slider = this._data.Slider;
      Vector2 realRaTriSize = slider.GetRealRATriSize(zoomFactor);
      raTriangle.RightAngleCorner = slider.Position;
      raTriangle.Rotation = slider.Rotation;
      raTriangle.XLength = realRaTriSize.x;
      raTriangle.YLength = realRaTriSize.y;
      raTriangle.XLengthSign = (double) realRaTriSize.x >= 0.0 ? AxisSign.Positive : AxisSign.Negative;
      raTriangle.YLengthSign = (double) realRaTriSize.y >= 0.0 ? AxisSign.Positive : AxisSign.Negative;
      this._data.RATriangleBorder.OnTriangleShapeChanged();
    }
  }
}
