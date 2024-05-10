// Decompiled with JetBrains decompiler
// Type: RTG.GizmoThinLineSlider3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoThinLineSlider3DController : GizmoLineSlider3DController
  {
    public GizmoThinLineSlider3DController(GizmoLineSlider3DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.SliderHandle.Set3DShapeVisible(this._data.CylinderIndex, false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.BoxIndex, false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.SegmentIndex, this._data.Slider.IsVisible);
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      GizmoLineSlider3D slider = this._data.Slider;
      this._data.Segment.StartPoint = slider.StartPosition;
      this._data.Segment.SetEndPtFromStart(slider.Direction, slider.GetRealLength(zoomFactor));
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.Segment.RaycastEps = this._data.Slider.Settings.LineHoverEps * zoomFactor;
    }

    public override float GetRealSizeAlongDirection(Vector3 direction, float zoomFactor)
    {
      GizmoLineSlider3D slider = this._data.Slider;
      return direction.AbsDot(slider.Direction * slider.GetRealLength(zoomFactor));
    }
  }
}
