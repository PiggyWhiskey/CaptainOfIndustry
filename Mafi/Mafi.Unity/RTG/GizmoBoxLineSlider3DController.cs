// Decompiled with JetBrains decompiler
// Type: RTG.GizmoBoxLineSlider3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoBoxLineSlider3DController : GizmoLineSlider3DController
  {
    public GizmoBoxLineSlider3DController(GizmoLineSlider3DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.SliderHandle.Set3DShapeVisible(this._data.CylinderIndex, false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.SegmentIndex, false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.BoxIndex, this._data.Slider.IsVisible);
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      GizmoLineSlider3D slider = this._data.Slider;
      this._data.Box.AlignWidth(slider.GetRealDirection());
      this._data.Box.Size = new Vector3(slider.GetRealLength(zoomFactor), slider.GetRealBoxHeight(zoomFactor), slider.GetRealBoxDepth(zoomFactor));
      this._data.Box.SetFaceCenter(BoxFace.Left, slider.StartPosition);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      float num = this._data.Slider.Settings.BoxHoverEps * zoomFactor;
      this._data.Box.SizeEps = new Vector3(0.0f, num, num);
    }

    public override float GetRealSizeAlongDirection(Vector3 direction, float zoomFactor)
    {
      direction.Normalize();
      GizmoLineSlider3D slider = this._data.Slider;
      Vector3 v2 = this._data.Box.Rotation * new Vector3(slider.GetRealLength(zoomFactor), slider.GetRealBoxHeight(zoomFactor), slider.GetRealBoxDepth(zoomFactor));
      return direction.AbsDot(v2);
    }
  }
}
