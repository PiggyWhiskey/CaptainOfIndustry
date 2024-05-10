// Decompiled with JetBrains decompiler
// Type: RTG.UniversalGizmoSettings3D
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
  public class UniversalGizmoSettings3D : Settings
  {
    [SerializeField]
    private UniversalGizmoSettingsCategory _displayCategory;
    [SerializeField]
    private GizmoObjectVertexSnapSettings _mvVertexSnapSettings;
    [SerializeField]
    private GizmoLineSlider3DSettings[] _mvSglSliderSettings;
    [SerializeField]
    private GizmoPlaneSlider3DSettings[] _mvDblSliderSettings;
    [SerializeField]
    private float _rtCamRightSnapStep;
    [SerializeField]
    private float _rtCamUpSnapStep;
    [SerializeField]
    private GizmoPlaneSlider3DSettings[] _rtSliderSettings;
    [SerializeField]
    private GizmoPlaneSlider2DSettings _rtCamLookSliderSettings;
    [SerializeField]
    private float _scUniformSnapStep;
    [SerializeField]
    private GizmoLineSlider3DSettings[] _scSglSliderSettings;
    [SerializeField]
    private GizmoPlaneSlider3DSettings[] _scDblSliderSettings;

    public GizmoObjectVertexSnapSettings VertexSnapSettings => this._mvVertexSnapSettings;

    public float MvLineSliderHoverEps => this._mvSglSliderSettings[0].LineHoverEps;

    public float MvBoxSliderHoverEps => this._mvSglSliderSettings[0].BoxHoverEps;

    public float MvCylinderSliderHoverEps => this._mvSglSliderSettings[0].CylinderHoverEps;

    public float MvXSnapStep => this.GetMvSglSliderSettings(0, AxisSign.Positive).OffsetSnapStep;

    public float MvYSnapStep => this.GetMvSglSliderSettings(1, AxisSign.Positive).OffsetSnapStep;

    public float MvZSnapStep => this.GetMvSglSliderSettings(2, AxisSign.Positive).OffsetSnapStep;

    public float MvDragSensitivity => this._mvSglSliderSettings[0].OffsetSensitivity;

    public float RtAxisLineHoverEps => this._rtSliderSettings[0].BorderLineHoverEps;

    public float RtAxisTorusHoverEps => this._rtSliderSettings[0].BorderTorusHoverEps;

    public float RtCamLookLineHoverEps => this._rtCamLookSliderSettings.BorderLineHoverEps;

    public float RtCamLookThickHoverEps => this._rtCamLookSliderSettings.ThickBorderPolyHoverEps;

    public bool RtCanHoverCulledPixels => !this._rtSliderSettings[0].IsCircleHoverCullEnabled;

    public GizmoSnapMode RtSnapMode => this._rtSliderSettings[0].RotationSnapMode;

    public float RtXSnapStep => this._rtSliderSettings[0].RotationSnapStep;

    public float RtYSnapStep => this._rtSliderSettings[1].RotationSnapStep;

    public float RtZSnapStep => this._rtSliderSettings[2].RotationSnapStep;

    public float RtCamRightSnapStep => this._rtCamRightSnapStep;

    public float RtCamUpSnapStep => this._rtCamUpSnapStep;

    public float RtCamLookSnapStep => this._rtCamLookSliderSettings.RotationSnapStep;

    public float RtDragSensitivity => this._rtSliderSettings[0].RotationSensitivity;

    public float ScLineSliderHoverEps => this._scSglSliderSettings[0].LineHoverEps;

    public float ScBoxSliderHoverEps => this._scSglSliderSettings[0].BoxHoverEps;

    public float ScCylinderSliderHoverEps => this._scSglSliderSettings[0].CylinderHoverEps;

    public float ScXSnapStep => this.GetScSglSliderSettings(0, AxisSign.Positive).ScaleSnapStep;

    public float ScYSnapStep => this.GetScSglSliderSettings(1, AxisSign.Positive).ScaleSnapStep;

    public float ScZSnapStep => this.GetScSglSliderSettings(2, AxisSign.Positive).ScaleSnapStep;

    public float ScXYSnapStep => this.GetScDblSliderSettings(PlaneId.XY).ProportionalScaleSnapStep;

    public float ScYZSnapStep => this.GetScDblSliderSettings(PlaneId.YZ).ProportionalScaleSnapStep;

    public float ScZXSnapStep => this.GetScDblSliderSettings(PlaneId.ZX).ProportionalScaleSnapStep;

    public float ScUniformSnapStep => this._scUniformSnapStep;

    public float ScDragSensitivity => this._scSglSliderSettings[0].ScaleSensitivity;

    public UniversalGizmoSettingsCategory DisplayCategory
    {
      get => this._displayCategory;
      set => this._displayCategory = value;
    }

    public UniversalGizmoSettings3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._mvVertexSnapSettings = new GizmoObjectVertexSnapSettings();
      this._mvSglSliderSettings = new GizmoLineSlider3DSettings[6];
      this._mvDblSliderSettings = new GizmoPlaneSlider3DSettings[3];
      this._rtCamRightSnapStep = 15f;
      this._rtCamUpSnapStep = 15f;
      this._rtSliderSettings = new GizmoPlaneSlider3DSettings[3];
      this._rtCamLookSliderSettings = new GizmoPlaneSlider2DSettings();
      this._scUniformSnapStep = 0.1f;
      this._scSglSliderSettings = new GizmoLineSlider3DSettings[6];
      this._scDblSliderSettings = new GizmoPlaneSlider3DSettings[3];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._mvSglSliderSettings.Length; ++index)
        this._mvSglSliderSettings[index] = new GizmoLineSlider3DSettings();
      for (int index = 0; index < this._mvDblSliderSettings.Length; ++index)
      {
        this._mvDblSliderSettings[index] = new GizmoPlaneSlider3DSettings();
        this._mvDblSliderSettings[index].AreaHoverEps = 0.0f;
        this._mvDblSliderSettings[index].BorderLineHoverEps = 0.0f;
        this._mvDblSliderSettings[index].BorderBoxHoverEps = 0.0f;
      }
      for (int index = 0; index < this._rtSliderSettings.Length; ++index)
        this._rtSliderSettings[index] = new GizmoPlaneSlider3DSettings();
      this.SetRtCamLookLineHoverEps(7f);
      this.SetRtCanHoverCulledPixels(false);
      this.SetRtAxisTorusHoverEps(0.4f);
      for (int index = 0; index < this._scSglSliderSettings.Length; ++index)
        this._scSglSliderSettings[index] = new GizmoLineSlider3DSettings();
      for (int index = 0; index < this._scDblSliderSettings.Length; ++index)
      {
        this._scDblSliderSettings[index] = new GizmoPlaneSlider3DSettings();
        this._scDblSliderSettings[index].ScaleMode = GizmoDblAxisScaleMode.Proportional;
        this._scDblSliderSettings[index].AreaHoverEps = 0.0f;
        this._scDblSliderSettings[index].BorderLineHoverEps = 0.0f;
        this._scDblSliderSettings[index].BorderBoxHoverEps = 0.0f;
      }
      this.SetScDragSensitivity(0.6f);
    }

    public void SetMvLineSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._mvSglSliderSettings)
        sglSliderSetting.LineHoverEps = eps;
    }

    public void SetMvBoxSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._mvSglSliderSettings)
        sglSliderSetting.BoxHoverEps = eps;
    }

    public void SetMvCylinderSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._mvSglSliderSettings)
        sglSliderSetting.CylinderHoverEps = eps;
    }

    public void SetMvXSnapStep(float snapStep)
    {
      this.GetMvSglSliderSettings(0, AxisSign.Positive).OffsetSnapStep = snapStep;
      this.GetMvSglSliderSettings(0, AxisSign.Negative).OffsetSnapStep = snapStep;
      this.GetMvDblSliderSettings(PlaneId.XY).OffsetSnapStepRight = snapStep;
      this.GetMvDblSliderSettings(PlaneId.ZX).OffsetSnapStepUp = snapStep;
    }

    public void SetMvYSnapStep(float snapStep)
    {
      this.GetMvSglSliderSettings(1, AxisSign.Positive).OffsetSnapStep = snapStep;
      this.GetMvSglSliderSettings(1, AxisSign.Negative).OffsetSnapStep = snapStep;
      this.GetMvDblSliderSettings(PlaneId.XY).OffsetSnapStepUp = snapStep;
      this.GetMvDblSliderSettings(PlaneId.YZ).OffsetSnapStepRight = snapStep;
    }

    public void SetMvZSnapStep(float snapStep)
    {
      this.GetMvSglSliderSettings(2, AxisSign.Positive).OffsetSnapStep = snapStep;
      this.GetMvSglSliderSettings(2, AxisSign.Negative).OffsetSnapStep = snapStep;
      this.GetMvDblSliderSettings(PlaneId.YZ).OffsetSnapStepUp = snapStep;
      this.GetMvDblSliderSettings(PlaneId.ZX).OffsetSnapStepRight = snapStep;
    }

    public void SetMvDragSensitivity(float sensitivity)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._mvSglSliderSettings)
        sglSliderSetting.OffsetSensitivity = sensitivity;
      foreach (GizmoPlaneSlider3DSettings dblSliderSetting in this._mvDblSliderSettings)
        dblSliderSetting.OffsetSensitivity = sensitivity;
    }

    public void ConnectMvSliderSettings(GizmoLineSlider3D slider, int axisIndex, AxisSign axisSign)
    {
      slider.SharedSettings = this.GetMvSglSliderSettings(axisIndex, axisSign);
    }

    public void ConnectMvDblSliderSettings(GizmoPlaneSlider3D dblSlider, PlaneId planeId)
    {
      dblSlider.SharedSettings = this.GetMvDblSliderSettings(planeId);
    }

    public void Inherit(MoveGizmoSettings3D settings)
    {
      this.SetMvLineSliderHoverEps(settings.LineSliderHoverEps);
      this.SetMvBoxSliderHoverEps(settings.BoxSliderHoverEps);
      this.SetMvCylinderSliderHoverEps(settings.CylinderSliderHoverEps);
      this.SetMvDragSensitivity(settings.DragSensitivity);
      this.SetMvXSnapStep(settings.XSnapStep);
      this.SetMvYSnapStep(settings.YSnapStep);
      this.SetMvZSnapStep(settings.ZSnapStep);
      settings.VertexSnapSettings.Transfer(this._mvVertexSnapSettings);
    }

    private GizmoLineSlider3DSettings GetMvSglSliderSettings(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._mvSglSliderSettings[axisIndex] : this._mvSglSliderSettings[3 + axisIndex];
    }

    private GizmoPlaneSlider3DSettings GetMvDblSliderSettings(PlaneId planeId)
    {
      return this._mvDblSliderSettings[(int) planeId];
    }

    public void SetRtCanHoverCulledPixels(bool canHover)
    {
      foreach (GizmoPlaneSlider3DSettings rtSliderSetting in this._rtSliderSettings)
        rtSliderSetting.IsCircleHoverCullEnabled = !canHover;
    }

    public void SetRtAxisLineHoverEps(float eps)
    {
      foreach (GizmoPlaneSlider3DSettings rtSliderSetting in this._rtSliderSettings)
        rtSliderSetting.BorderLineHoverEps = eps;
    }

    public void SetRtAxisTorusHoverEps(float eps)
    {
      foreach (GizmoPlaneSlider3DSettings rtSliderSetting in this._rtSliderSettings)
        rtSliderSetting.BorderTorusHoverEps = eps;
    }

    public void SetRtCamLookLineHoverEps(float eps)
    {
      this._rtCamLookSliderSettings.BorderLineHoverEps = eps;
    }

    public void SetRtCamLookThickHoverEps(float eps)
    {
      this._rtCamLookSliderSettings.ThickBorderPolyHoverEps = eps;
    }

    public void SetRtAxisSnapStep(int axisIndex, float snapStep)
    {
      this._rtSliderSettings[axisIndex].RotationSnapStep = snapStep;
    }

    public void SetRtCamRightSnapStep(float snapStep)
    {
      this._rtCamRightSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public void SetRtCamUpSnapStep(float snapStep)
    {
      this._rtCamUpSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public void SetRtCamLookSnapStep(float snapStep)
    {
      this._rtCamLookSliderSettings.RotationSnapStep = snapStep;
    }

    public void SetRtSnapMode(GizmoSnapMode snapMode)
    {
      foreach (GizmoPlaneSlider3DSettings rtSliderSetting in this._rtSliderSettings)
        rtSliderSetting.RotationSnapMode = snapMode;
      this._rtCamLookSliderSettings.RotationSnapMode = snapMode;
    }

    public void SetRtDragSensitivity(float sensitivity)
    {
      foreach (GizmoPlaneSlider3DSettings rtSliderSetting in this._rtSliderSettings)
        rtSliderSetting.RotationSensitivity = sensitivity;
      this._rtCamLookSliderSettings.RotationSensitivity = sensitivity;
    }

    public void ConnectRtSliderSettings(GizmoPlaneSlider3D slider, int axisIndex)
    {
      slider.SharedSettings = this._rtSliderSettings[axisIndex];
    }

    public void ConnectRtCamLookSliderSettings(GizmoPlaneSlider2D slider)
    {
      slider.SharedSettings = this._rtCamLookSliderSettings;
    }

    public void Inherit(RotationGizmoSettings3D settings)
    {
      this.SetRtAxisLineHoverEps(settings.AxisLineHoverEps);
      this.SetRtAxisTorusHoverEps(settings.AxisTorusHoverEps);
      this.SetRtCamLookThickHoverEps(settings.CamLookThickHoverEps);
      this.SetRtCamLookLineHoverEps(settings.CamLookLineHoverEps);
      this.SetRtCamLookSnapStep(settings.CamLookSnapStep);
      this.SetRtCamRightSnapStep(settings.CamRightSnapStep);
      this.SetRtCamUpSnapStep(settings.CamUpSnapStep);
      this.SetRtCanHoverCulledPixels(settings.CanHoverCulledPixels);
      this.SetRtDragSensitivity(settings.DragSensitivity);
      this.SetRtSnapMode(settings.SnapMode);
      this.SetRtAxisSnapStep(0, settings.XSnapStep);
      this.SetRtAxisSnapStep(1, settings.YSnapStep);
      this.SetRtAxisSnapStep(2, settings.ZSnapStep);
    }

    public void SetScLineSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._scSglSliderSettings)
        sglSliderSetting.LineHoverEps = eps;
    }

    public void SetScBoxSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._scSglSliderSettings)
        sglSliderSetting.BoxHoverEps = eps;
    }

    public void SetScCylinderSliderHoverEps(float eps)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._scSglSliderSettings)
        sglSliderSetting.CylinderHoverEps = eps;
    }

    public void SetScXSnapStep(float snapStep)
    {
      this.GetScSglSliderSettings(0, AxisSign.Positive).ScaleSnapStep = snapStep;
      this.GetScSglSliderSettings(0, AxisSign.Negative).ScaleSnapStep = snapStep;
      this.GetScDblSliderSettings(PlaneId.XY).ScaleSnapStepRight = snapStep;
      this.GetScDblSliderSettings(PlaneId.ZX).ScaleSnapStepUp = snapStep;
    }

    public void SetScYSnapStep(float snapStep)
    {
      this.GetScSglSliderSettings(1, AxisSign.Positive).ScaleSnapStep = snapStep;
      this.GetScSglSliderSettings(1, AxisSign.Negative).ScaleSnapStep = snapStep;
      this.GetScDblSliderSettings(PlaneId.XY).ScaleSnapStepUp = snapStep;
      this.GetScDblSliderSettings(PlaneId.YZ).ScaleSnapStepRight = snapStep;
    }

    public void SetScZSnapStep(float snapStep)
    {
      this.GetScSglSliderSettings(2, AxisSign.Positive).ScaleSnapStep = snapStep;
      this.GetScSglSliderSettings(2, AxisSign.Negative).ScaleSnapStep = snapStep;
      this.GetScDblSliderSettings(PlaneId.YZ).ScaleSnapStepUp = snapStep;
      this.GetScDblSliderSettings(PlaneId.ZX).ScaleSnapStepRight = snapStep;
    }

    public void SetScXYSnapStep(float snapStep)
    {
      this.GetScDblSliderSettings(PlaneId.XY).ProportionalScaleSnapStep = snapStep;
    }

    public void SetScYZSnapStep(float snapStep)
    {
      this.GetScDblSliderSettings(PlaneId.YZ).ProportionalScaleSnapStep = snapStep;
    }

    public void SetScZXSnapStep(float snapStep)
    {
      this.GetScDblSliderSettings(PlaneId.ZX).ProportionalScaleSnapStep = snapStep;
    }

    public void SetScUniformScaleSnapStep(float snapStep)
    {
      this._scUniformSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public void SetScDragSensitivity(float sensitivity)
    {
      foreach (GizmoLineSlider3DSettings sglSliderSetting in this._scSglSliderSettings)
        sglSliderSetting.ScaleSensitivity = sensitivity;
    }

    public void ConnectScSliderSettings(GizmoLineSlider3D slider, int axisIndex, AxisSign axisSign)
    {
      slider.SharedSettings = this.GetScSglSliderSettings(axisIndex, axisSign);
    }

    public void ConnectScDblSliderSettings(GizmoPlaneSlider3D dblSlider, PlaneId planeId)
    {
      dblSlider.SharedSettings = this.GetScDblSliderSettings(planeId);
    }

    public void Inherit(ScaleGizmoSettings3D settings)
    {
      this.SetScLineSliderHoverEps(settings.LineSliderHoverEps);
      this.SetScCylinderSliderHoverEps(settings.CylinderSliderHoverEps);
      this.SetScBoxSliderHoverEps(settings.BoxSliderHoverEps);
      this.SetScDragSensitivity(settings.DragSensitivity);
      this.SetScUniformScaleSnapStep(settings.UniformSnapStep);
      this.SetScXSnapStep(settings.XSnapStep);
      this.SetScYSnapStep(settings.YSnapStep);
      this.SetScZSnapStep(settings.ZSnapStep);
      this.SetScXYSnapStep(settings.XYSnapStep);
      this.SetScYZSnapStep(settings.YZSnapStep);
      this.SetScZXSnapStep(settings.ZXSnapStep);
    }

    private GizmoLineSlider3DSettings GetScSglSliderSettings(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._scSglSliderSettings[axisIndex] : this._scSglSliderSettings[3 + axisIndex];
    }

    private GizmoPlaneSlider3DSettings GetScDblSliderSettings(PlaneId planeId)
    {
      return this._scDblSliderSettings[(int) planeId];
    }
  }
}
