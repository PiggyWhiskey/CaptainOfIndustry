// Decompiled with JetBrains decompiler
// Type: RTG.ScaleGizmoSettings3D
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
  public class ScaleGizmoSettings3D : Settings
  {
    [SerializeField]
    private float _uniformSnapStep;
    [SerializeField]
    private GizmoLineSlider3DSettings[] _sglSliderSettings;
    [SerializeField]
    private GizmoPlaneSlider3DSettings[] _dblSliderSettings;

    public float LineSliderHoverEps => this._sglSliderSettings[0].LineHoverEps;

    public float BoxSliderHoverEps => this._sglSliderSettings[0].BoxHoverEps;

    public float CylinderSliderHoverEps => this._sglSliderSettings[0].CylinderHoverEps;

    public float XSnapStep => this.GetSglSliderSettings(0, AxisSign.Positive).ScaleSnapStep;

    public float YSnapStep => this.GetSglSliderSettings(1, AxisSign.Positive).ScaleSnapStep;

    public float ZSnapStep => this.GetSglSliderSettings(2, AxisSign.Positive).ScaleSnapStep;

    public float XYSnapStep => this.GetDblSliderSettings(PlaneId.XY).ProportionalScaleSnapStep;

    public float YZSnapStep => this.GetDblSliderSettings(PlaneId.YZ).ProportionalScaleSnapStep;

    public float ZXSnapStep => this.GetDblSliderSettings(PlaneId.ZX).ProportionalScaleSnapStep;

    public float UniformSnapStep => this._uniformSnapStep;

    public float DragSensitivity => this._sglSliderSettings[0].ScaleSensitivity;

    public ScaleGizmoSettings3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._uniformSnapStep = 0.1f;
      this._sglSliderSettings = new GizmoLineSlider3DSettings[6];
      this._dblSliderSettings = new GizmoPlaneSlider3DSettings[3];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._sglSliderSettings.Length; ++index)
        this._sglSliderSettings[index] = new GizmoLineSlider3DSettings();
      for (int index = 0; index < this._dblSliderSettings.Length; ++index)
      {
        this._dblSliderSettings[index] = new GizmoPlaneSlider3DSettings();
        this._dblSliderSettings[index].ScaleMode = GizmoDblAxisScaleMode.Proportional;
        this._dblSliderSettings[index].AreaHoverEps = 0.0f;
        this._dblSliderSettings[index].BorderLineHoverEps = 0.0f;
        this._dblSliderSettings[index].BorderBoxHoverEps = 0.0f;
      }
      this.SetDragSensitivity(0.6f);
    }

    public void SetLineSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._sglSliderSettings)
        sglSliderSetting.LineHoverEps = eps;
    }

    public void SetBoxSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._sglSliderSettings)
        sglSliderSetting.BoxHoverEps = eps;
    }

    public void SetCylinderSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._sglSliderSettings)
        sglSliderSetting.CylinderHoverEps = eps;
    }

    public void SetXSnapStep(float snapStep)
    {
      this.GetSglSliderSettings(0, AxisSign.Positive).ScaleSnapStep = snapStep;
      this.GetSglSliderSettings(0, AxisSign.Negative).ScaleSnapStep = snapStep;
      this.GetDblSliderSettings(PlaneId.XY).ScaleSnapStepRight = snapStep;
      this.GetDblSliderSettings(PlaneId.ZX).ScaleSnapStepUp = snapStep;
    }

    public void SetYSnapStep(float snapStep)
    {
      this.GetSglSliderSettings(1, AxisSign.Positive).ScaleSnapStep = snapStep;
      this.GetSglSliderSettings(1, AxisSign.Negative).ScaleSnapStep = snapStep;
      this.GetDblSliderSettings(PlaneId.XY).ScaleSnapStepUp = snapStep;
      this.GetDblSliderSettings(PlaneId.YZ).ScaleSnapStepRight = snapStep;
    }

    public void SetZSnapStep(float snapStep)
    {
      this.GetSglSliderSettings(2, AxisSign.Positive).ScaleSnapStep = snapStep;
      this.GetSglSliderSettings(2, AxisSign.Negative).ScaleSnapStep = snapStep;
      this.GetDblSliderSettings(PlaneId.YZ).ScaleSnapStepUp = snapStep;
      this.GetDblSliderSettings(PlaneId.ZX).ScaleSnapStepRight = snapStep;
    }

    public void SetXYSnapStep(float snapStep)
    {
      this.GetDblSliderSettings(PlaneId.XY).ProportionalScaleSnapStep = snapStep;
    }

    public void SetYZSnapStep(float snapStep)
    {
      this.GetDblSliderSettings(PlaneId.YZ).ProportionalScaleSnapStep = snapStep;
    }

    public void SetZXSnapStep(float snapStep)
    {
      this.GetDblSliderSettings(PlaneId.ZX).ProportionalScaleSnapStep = snapStep;
    }

    public void SetUniformScaleSnapStep(float snapStep)
    {
      this._uniformSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public void SetDragSensitivity(float sensitivity)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._sglSliderSettings)
        sglSliderSetting.ScaleSensitivity = sensitivity;
    }

    public void ConnectSliderSettings(GizmoLineSlider3D slider, int axisIndex, AxisSign axisSign)
    {
      slider.SharedSettings = this.GetSglSliderSettings(axisIndex, axisSign);
    }

    public void ConnectDblSliderSettings(GizmoPlaneSlider3D dblSlider, PlaneId planeId)
    {
      dblSlider.SharedSettings = this.GetDblSliderSettings(planeId);
    }

    private GizmoLineSlider3DSettings GetSglSliderSettings(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._sglSliderSettings[axisIndex] : this._sglSliderSettings[3 + axisIndex];
    }

    private GizmoPlaneSlider3DSettings GetDblSliderSettings(PlaneId planeId)
    {
      return this._dblSliderSettings[(int) planeId];
    }
  }
}
