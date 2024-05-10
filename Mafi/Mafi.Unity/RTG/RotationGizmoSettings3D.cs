// Decompiled with JetBrains decompiler
// Type: RTG.RotationGizmoSettings3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public class RotationGizmoSettings3D : Settings
  {
    [SerializeField]
    private float _camRightSnapStep;
    [SerializeField]
    private float _camUpSnapStep;
    [SerializeField]
    private GizmoPlaneSlider3DSettings[] _sliderSettings;
    [SerializeField]
    private GizmoPlaneSlider2DSettings _camLookSliderSettings;

    public float AxisLineHoverEps => this._sliderSettings[0].BorderLineHoverEps;

    public float AxisTorusHoverEps => this._sliderSettings[0].BorderTorusHoverEps;

    public float CamLookLineHoverEps => this._camLookSliderSettings.BorderLineHoverEps;

    public float CamLookThickHoverEps => this._camLookSliderSettings.ThickBorderPolyHoverEps;

    public bool CanHoverCulledPixels => !this._sliderSettings[0].IsCircleHoverCullEnabled;

    public GizmoSnapMode SnapMode => this._sliderSettings[0].RotationSnapMode;

    public float XSnapStep => this._sliderSettings[0].RotationSnapStep;

    public float YSnapStep => this._sliderSettings[1].RotationSnapStep;

    public float ZSnapStep => this._sliderSettings[2].RotationSnapStep;

    public float CamRightSnapStep => this._camRightSnapStep;

    public float CamUpSnapStep => this._camUpSnapStep;

    public float CamLookSnapStep => this._camLookSliderSettings.RotationSnapStep;

    public float DragSensitivity => this._sliderSettings[0].RotationSensitivity;

    public RotationGizmoSettings3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._camRightSnapStep = 15f;
      this._camUpSnapStep = 15f;
      this._sliderSettings = new GizmoPlaneSlider3DSettings[3];
      this._camLookSliderSettings = new GizmoPlaneSlider2DSettings();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._sliderSettings.Length; ++index)
        this._sliderSettings[index] = new GizmoPlaneSlider3DSettings();
      this.SetCamLookLineHoverEps(7f);
      this.SetCanHoverCulledPixels(false);
      this.SetAxisTorusHoverEps(0.4f);
    }

    public void SetCanHoverCulledPixels(bool canHover)
    {
      foreach (GizmoPlaneSlider3DSettings sliderSetting in this._sliderSettings)
        sliderSetting.IsCircleHoverCullEnabled = !canHover;
    }

    public void SetAxisLineHoverEps(float eps)
    {
      foreach (GizmoPlaneSlider3DSettings sliderSetting in this._sliderSettings)
        sliderSetting.BorderLineHoverEps = eps;
    }

    public void SetAxisTorusHoverEps(float eps)
    {
      foreach (GizmoPlaneSlider3DSettings sliderSetting in this._sliderSettings)
        sliderSetting.BorderTorusHoverEps = eps;
    }

    public void SetCamLookLineHoverEps(float eps)
    {
      this._camLookSliderSettings.BorderLineHoverEps = eps;
    }

    public void SetCamLookThickHoverEps(float eps)
    {
      this._camLookSliderSettings.ThickBorderPolyHoverEps = eps;
    }

    public void SetAxisSnapStep(int axisIndex, float snapStep)
    {
      this._sliderSettings[axisIndex].RotationSnapStep = snapStep;
    }

    public void SetCamRightSnapStep(float snapStep)
    {
      this._camRightSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public void SetCamUpSnapStep(float snapStep)
    {
      this._camUpSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public void SetCamLookSnapStep(float snapStep)
    {
      this._camLookSliderSettings.RotationSnapStep = snapStep;
    }

    public void SetSnapMode(GizmoSnapMode snapMode)
    {
      foreach (GizmoPlaneSlider3DSettings sliderSetting in this._sliderSettings)
        sliderSetting.RotationSnapMode = snapMode;
      this._camLookSliderSettings.RotationSnapMode = snapMode;
    }

    public void SetDragSensitivity(float sensitivity)
    {
      foreach (GizmoPlaneSlider3DSettings sliderSetting in this._sliderSettings)
        sliderSetting.RotationSensitivity = sensitivity;
      this._camLookSliderSettings.RotationSensitivity = sensitivity;
    }

    public void ConnectSliderSettings(GizmoPlaneSlider3D slider, int axisIndex)
    {
      slider.SharedSettings = this._sliderSettings[axisIndex];
    }

    public void ConnectCamLookSliderSettings(GizmoPlaneSlider2D slider)
    {
      slider.SharedSettings = this._camLookSliderSettings;
    }
  }
}
