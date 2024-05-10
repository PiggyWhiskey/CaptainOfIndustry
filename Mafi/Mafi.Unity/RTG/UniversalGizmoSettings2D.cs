// Decompiled with JetBrains decompiler
// Type: RTG.UniversalGizmoSettings2D
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
  public class UniversalGizmoSettings2D : Settings
  {
    [SerializeField]
    private UniversalGizmoSettingsCategory _displayCategory;
    [SerializeField]
    private GizmoPlaneSlider2DSettings _mvDblSliderSettings;
    [SerializeField]
    private GizmoLineSlider2DSettings[] _mvSglSliderSettings;

    public float MvLineSliderHoverEps => this._mvSglSliderSettings[0].LineHoverEps;

    public float MvBoxSliderHoverEps => this._mvSglSliderSettings[0].BoxHoverEps;

    public float MvXSnapStep => this._mvDblSliderSettings.OffsetSnapStepRight;

    public float MvYSnapStep => this._mvDblSliderSettings.OffsetSnapStepUp;

    public float MvDragSensitivity => this._mvDblSliderSettings.OffsetSensitivity;

    public UniversalGizmoSettingsCategory DisplayCategory
    {
      get => this._displayCategory;
      set => this._displayCategory = value;
    }

    public UniversalGizmoSettings2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._mvDblSliderSettings = new GizmoPlaneSlider2DSettings();
      this._mvSglSliderSettings = new GizmoLineSlider2DSettings[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._mvSglSliderSettings.Length; ++index)
        this._mvSglSliderSettings[index] = new GizmoLineSlider2DSettings();
      this._mvDblSliderSettings.ScaleMode = GizmoDblAxisScaleMode.Proportional;
    }

    public void SetMvLineSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider2DSettings sglSliderSetting in this._mvSglSliderSettings)
        sglSliderSetting.LineHoverEps = eps;
    }

    public void SetMvBoxSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider2DSettings sglSliderSetting in this._mvSglSliderSettings)
        sglSliderSetting.BoxHoverEps = eps;
    }

    public void SetMvXSnapStep(float snapStep)
    {
      this.GetMvSliderSettings(0, AxisSign.Positive).OffsetSnapStep = snapStep;
      this.GetMvSliderSettings(0, AxisSign.Negative).OffsetSnapStep = snapStep;
      this._mvDblSliderSettings.OffsetSnapStepRight = snapStep;
    }

    public void SetMvYSnapStep(float snapStep)
    {
      this.GetMvSliderSettings(1, AxisSign.Positive).OffsetSnapStep = snapStep;
      this.GetMvSliderSettings(1, AxisSign.Negative).OffsetSnapStep = snapStep;
      this._mvDblSliderSettings.OffsetSnapStepUp = snapStep;
    }

    public void SetMvDragSensitivity(float sensitivity)
    {
      foreach (GizmoLineSlider2DSettings sglSliderSetting in this._mvSglSliderSettings)
        sglSliderSetting.OffsetSensitivity = sensitivity;
      this._mvDblSliderSettings.OffsetSensitivity = sensitivity;
    }

    public void ConnectMvSliderSettings(GizmoLineSlider2D slider, int axisIndex, AxisSign axisSign)
    {
      slider.SharedSettings = this.GetMvSliderSettings(axisIndex, axisSign);
    }

    public void ConnectMvDblSliderSettings(GizmoPlaneSlider2D slider)
    {
      slider.SharedSettings = this._mvDblSliderSettings;
    }

    public void Inherit(MoveGizmoSettings2D settings)
    {
      this.SetMvLineSliderHoverEps(settings.LineSliderHoverEps);
      this.SetMvBoxSliderHoverEps(settings.BoxSliderHoverEps);
      this.SetMvXSnapStep(settings.XSnapStep);
      this.SetMvYSnapStep(settings.YSnapStep);
      this.SetMvDragSensitivity(settings.DragSensitivity);
    }

    private GizmoLineSlider2DSettings GetMvSliderSettings(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._mvSglSliderSettings[axisIndex] : this._mvSglSliderSettings[2 + axisIndex];
    }
  }
}
