// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCircleCap2DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoCircleCap2DController : GizmoCap2DController
  {
    public GizmoCircleCap2DController(GizmoCap2DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.CapHandle.Set2DShapeVisible(this._data.ArrowIndex, false);
      this._data.CapHandle.Set2DShapeVisible(this._data.QuadIndex, false);
      this._data.CapHandle.Set2DShapeVisible(this._data.CircleIndex, this._data.Cap.IsVisible);
    }

    public override void UpdateTransforms()
    {
      GizmoCap2D cap = this._data.Cap;
      this._data.Circle.Radius = cap.GetRealCircleRadius();
      this._data.Circle.Center = cap.Position;
    }

    public override void CapSlider2D(Vector2 sliderDirection, Vector2 sliderEndPt)
    {
      this._data.Cap.AlignTransformAxis(0, AxisSign.Positive, sliderDirection);
      this._data.Cap.Position = sliderEndPt + sliderDirection.normalized * this._data.Cap.GetRealCircleRadius();
    }

    public override void CapSlider2DInvert(Vector2 sliderDirection, Vector2 sliderEndPt)
    {
      this._data.Cap.AlignTransformAxis(0, AxisSign.Positive, -sliderDirection);
      this._data.Cap.Position = sliderEndPt + sliderDirection.normalized * this._data.Cap.GetRealCircleRadius();
    }

    public override float GetSliderAlignedRealLength() => this._data.Cap.GetRealCircleRadius() * 2f;
  }
}
