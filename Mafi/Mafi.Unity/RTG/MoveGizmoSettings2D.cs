// Decompiled with JetBrains decompiler
// Type: RTG.MoveGizmoSettings2D
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
  public class MoveGizmoSettings2D : Settings
  {
    [SerializeField]
    private GizmoPlaneSlider2DSettings _dblSliderSettings;
    [SerializeField]
    private GizmoLineSlider2DSettings[] _sglSliderSettings;

    public float LineSliderHoverEps => this._sglSliderSettings[0].LineHoverEps;

    public float BoxSliderHoverEps => this._sglSliderSettings[0].BoxHoverEps;

    public float XSnapStep => this._dblSliderSettings.OffsetSnapStepRight;

    public float YSnapStep => this._dblSliderSettings.OffsetSnapStepUp;

    public float DragSensitivity => this._dblSliderSettings.OffsetSensitivity;

    public MoveGizmoSettings2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._dblSliderSettings = new GizmoPlaneSlider2DSettings();
      this._sglSliderSettings = new GizmoLineSlider2DSettings[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._sglSliderSettings.Length; ++index)
        this._sglSliderSettings[index] = new GizmoLineSlider2DSettings();
      this._dblSliderSettings.ScaleMode = GizmoDblAxisScaleMode.Proportional;
    }

    public void SetLineSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider2DSettings sglSliderSetting in this._sglSliderSettings)
        sglSliderSetting.LineHoverEps = eps;
    }

    public void SetBoxSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider2DSettings sglSliderSetting in this._sglSliderSettings)
        sglSliderSetting.BoxHoverEps = eps;
    }

    public void SetXSnapStep(float snapStep)
    {
      this.GetSliderSettings(0, AxisSign.Positive).OffsetSnapStep = snapStep;
      this.GetSliderSettings(0, AxisSign.Negative).OffsetSnapStep = snapStep;
      this._dblSliderSettings.OffsetSnapStepRight = snapStep;
    }

    public void SetYSnapStep(float snapStep)
    {
      this.GetSliderSettings(1, AxisSign.Positive).OffsetSnapStep = snapStep;
      this.GetSliderSettings(1, AxisSign.Negative).OffsetSnapStep = snapStep;
      this._dblSliderSettings.OffsetSnapStepUp = snapStep;
    }

    public void SetDragSensitivity(float sensitivity)
    {
      foreach (GizmoLineSlider2DSettings sglSliderSetting in this._sglSliderSettings)
        sglSliderSetting.OffsetSensitivity = sensitivity;
      this._dblSliderSettings.OffsetSensitivity = sensitivity;
    }

    public void ConnectSliderSettings(GizmoLineSlider2D slider, int axisIndex, AxisSign axisSign)
    {
      slider.SharedSettings = this.GetSliderSettings(axisIndex, axisSign);
    }

    public void ConnectDblSliderSettings(GizmoPlaneSlider2D slider)
    {
      slider.SharedSettings = this._dblSliderSettings;
    }

    private GizmoLineSlider2DSettings GetSliderSettings(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._sglSliderSettings[axisIndex] : this._sglSliderSettings[2 + axisIndex];
    }
  }
}
