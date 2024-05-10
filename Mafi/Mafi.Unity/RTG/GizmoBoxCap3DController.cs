// Decompiled with JetBrains decompiler
// Type: RTG.GizmoBoxCap3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoBoxCap3DController : GizmoCap3DController
  {
    public GizmoBoxCap3DController(GizmoCap3DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.CapHandle.Set3DShapeVisible(this._data.PyramidIndex, false);
      this._data.CapHandle.Set3DShapeVisible(this._data.TrPrismIndex, false);
      this._data.CapHandle.Set3DShapeVisible(this._data.ConeIndex, false);
      this._data.CapHandle.Set3DShapeVisible(this._data.SphereIndex, false);
      this._data.CapHandle.Set3DShapeVisible(this._data.BoxIndex, this._data.Cap.IsVisible);
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      GizmoCap3D cap = this._data.Cap;
      this._data.Box.Center = cap.Position;
      this._data.Box.Rotation = cap.Rotation;
      this._data.Box.Size = cap.GetRealBoxSize(zoomFactor);
    }

    public override void CapSlider3D(
      Vector3 sliderDirection,
      Vector3 sliderEndPt,
      float zoomFactor)
    {
      this._data.Cap.AlignTransformAxis(0, AxisSign.Positive, sliderDirection);
      this._data.Cap.Position = sliderEndPt + sliderDirection * this._data.Box.Size[0] * 0.5f;
    }

    public override void CapSlider3DInvert(
      Vector3 sliderDirection,
      Vector3 sliderEndPt,
      float zoomFactor)
    {
      this._data.Cap.AlignTransformAxis(0, AxisSign.Positive, -sliderDirection);
      this._data.Cap.Position = sliderEndPt + sliderDirection * this.GetSliderAlignedRealLength(zoomFactor) * 0.5f;
    }

    public override float GetSliderAlignedRealLength(float zoomFactor)
    {
      return this._data.Cap.GetRealBoxWidth(zoomFactor);
    }
  }
}
