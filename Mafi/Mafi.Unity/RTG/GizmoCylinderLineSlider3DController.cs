// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCylinderLineSlider3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoCylinderLineSlider3DController : GizmoLineSlider3DController
  {
    public GizmoCylinderLineSlider3DController(GizmoLineSlider3DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.SliderHandle.Set3DShapeVisible(this._data.SegmentIndex, false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.BoxIndex, false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.CylinderIndex, this._data.Slider.IsVisible);
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      GizmoLineSlider3D slider = this._data.Slider;
      this._data.Cylinder.AlignCentralAxis(slider.GetRealDirection());
      this._data.Cylinder.Radius = slider.GetRealCylinderRadius(zoomFactor);
      this._data.Cylinder.Height = slider.GetRealLength(zoomFactor);
      this._data.Cylinder.BaseCenter = slider.StartPosition;
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.Cylinder.RadiusEps = this._data.Slider.Settings.CylinderHoverEps * zoomFactor;
    }

    public override float GetRealSizeAlongDirection(Vector3 direction, float zoomFactor)
    {
      GizmoLineSlider3D slider = this._data.Slider;
      float realLength = slider.GetRealLength(zoomFactor);
      float realCylinderRadius = slider.GetRealCylinderRadius(zoomFactor);
      Vector3 v2 = this._data.Cylinder.Rotation * new Vector3(realCylinderRadius * 2f, realLength, realCylinderRadius * 2f);
      return direction.AbsDot(v2);
    }
  }
}
