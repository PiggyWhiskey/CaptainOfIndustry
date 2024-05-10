// Decompiled with JetBrains decompiler
// Type: RTG.RotationGizmoLookAndFeel3D
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
  public class RotationGizmoLookAndFeel3D : Settings
  {
    [SerializeField]
    private bool _isMidCapVisible;
    [SerializeField]
    private GizmoCap3DLookAndFeel _midCapLookAndFeel;
    [SerializeField]
    private bool[] _axesVis;
    [SerializeField]
    private GizmoPlaneSlider3DLookAndFeel[] _axesLookAndFeel;
    [SerializeField]
    private bool _isCamLookSliderVisible;
    [SerializeField]
    private float _camLookSliderRadiusOffset;
    [SerializeField]
    private GizmoPlaneSlider2DLookAndFeel _camLookSliderLookAndFeel;

    public float Scale => this._midCapLookAndFeel.Scale;

    public float Radius => this._midCapLookAndFeel.SphereRadius;

    public bool UseZoomFactor => this._midCapLookAndFeel.UseZoomFactor;

    public Color XBorderColor => this._axesLookAndFeel[0].BorderColor;

    public Color YBorderColor => this._axesLookAndFeel[1].BorderColor;

    public Color ZBorderColor => this._axesLookAndFeel[2].BorderColor;

    public Color HoveredColor => this._axesLookAndFeel[0].HoveredColor;

    public float AxisTorusThickness => this._axesLookAndFeel[0].BorderTorusThickness;

    public float AxisCylTorusWidth => this._axesLookAndFeel[0].BorderCylTorusWidth;

    public float AxisCylTorusHeight => this._axesLookAndFeel[0].BorderCylTorusHeight;

    public float AxisCullAlphaScale => this._axesLookAndFeel[0].BorderCircleCullAlphaScale;

    public GizmoShadeMode ShadeMode => this._midCapLookAndFeel.ShadeMode;

    public GizmoCircle3DBorderType AxisBorderType => this._axesLookAndFeel[0].CircleBorderType;

    public GizmoFillMode3D AxisBorderFillMode => this._axesLookAndFeel[0].BorderFillMode;

    public int NumAxisTorusWireAxialSlices
    {
      get => this._axesLookAndFeel[0].NumBorderTorusWireAxialSlices;
    }

    public Color RotationArcColor => this._axesLookAndFeel[0].RotationArcLookAndFeel.Color;

    public Color RotationArcBorderColor
    {
      get => this._axesLookAndFeel[0].RotationArcLookAndFeel.BorderColor;
    }

    public bool UseShortestRotationArc
    {
      get => this._axesLookAndFeel[0].RotationArcLookAndFeel.UseShortestRotation;
    }

    public bool IsRotationArcVisible => this._axesLookAndFeel[0].IsRotationArcVisible;

    public Color MidCapColor => this._midCapLookAndFeel.Color;

    public Color MidCapBorderColor => this._midCapLookAndFeel.SphereBorderColor;

    public Color HoveredMidCapColor => this._midCapLookAndFeel.HoveredColor;

    public bool IsMidCapVisible => this._isMidCapVisible;

    public bool IsMidCapBorderVisible => this._midCapLookAndFeel.IsSphereBorderVisible;

    public float CamLookSliderRadiusOffset => this._camLookSliderRadiusOffset;

    public Color CamLookSliderBorderColor => this._camLookSliderLookAndFeel.BorderColor;

    public Color CamLookSliderHoveredBorderColor
    {
      get => this._camLookSliderLookAndFeel.HoveredBorderColor;
    }

    public GizmoPolygon2DBorderType CamLookSliderPolyBorderType
    {
      get => this._camLookSliderLookAndFeel.PolygonBorderType;
    }

    public float CamLookSliderPolyBorderThickness
    {
      get => this._camLookSliderLookAndFeel.BorderPolyThickness;
    }

    public bool IsCamLookSliderVisible => this._isCamLookSliderVisible;

    public RotationGizmoLookAndFeel3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isMidCapVisible = true;
      this._midCapLookAndFeel = new GizmoCap3DLookAndFeel();
      this._axesVis = new bool[3];
      this._axesLookAndFeel = new GizmoPlaneSlider3DLookAndFeel[3];
      this._isCamLookSliderVisible = true;
      this._camLookSliderRadiusOffset = 0.65f;
      this._camLookSliderLookAndFeel = new GizmoPlaneSlider2DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._axesLookAndFeel.Length; ++index)
      {
        this._axesLookAndFeel[index] = new GizmoPlaneSlider3DLookAndFeel();
        this._axesLookAndFeel[index].PlaneType = GizmoPlane3DType.Circle;
      }
      this.SetAxisVisible(0, true);
      this.SetAxisVisible(1, true);
      this.SetAxisVisible(2, true);
      this._midCapLookAndFeel.CapType = GizmoCap3DType.Sphere;
      this._camLookSliderLookAndFeel.PlaneType = GizmoPlane2DType.Polygon;
      Color color = new Color(0.3f, 0.3f, 0.3f, 0.12f);
      this.SetMidCapColor(color);
      this.SetHoveredMidCapColor(color);
      this.SetMidCapBorderVisible(true);
      this.SetMidCapBorderColor(Color.white);
      this.SetRadius(6.5f);
      this.SetAxisBorderColor(0, RTSystemValues.XAxisColor);
      this.SetAxisBorderColor(1, RTSystemValues.YAxisColor);
      this.SetAxisBorderColor(2, RTSystemValues.ZAxisColor);
      this.SetHoveredColor(RTSystemValues.HoveredAxisColor);
      this.SetCamLookSliderPolyBorderThickness(4f);
      this.SetCamLookSliderBorderColor(Color.white);
      this.SetCamLookSliderHoveredBorderColor(RTSystemValues.HoveredAxisColor);
      this.SetNumAxisTorusWireAxialSlices(2);
    }

    public bool IsAxisVisible(int axisIndex) => this._axesVis[axisIndex];

    public void SetAxisVisible(int axisIndex, bool isVisible)
    {
      this._axesVis[axisIndex] = isVisible;
    }

    public void SetShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
      {
        slider3DlookAndFeel.ShadeMode = shadeMode;
        slider3DlookAndFeel.BorderShadeMode = shadeMode;
      }
      this._midCapLookAndFeel.ShadeMode = shadeMode;
    }

    public void SetAxisBorderFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.BorderFillMode = fillMode;
    }

    public void SetNumAxisTorusWireAxialSlices(int numSlices)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.NumBorderTorusWireAxialSlices = numSlices;
    }

    public void SetUseZoomFactor(bool useZoomFactor)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.UseZoomFactor = useZoomFactor;
      this._midCapLookAndFeel.UseZoomFactor = useZoomFactor;
    }

    public void SetScale(float scale)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.Scale = scale;
      this._midCapLookAndFeel.Scale = scale;
    }

    public void SetRadius(float radius)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.CircleRadius = radius;
      this._midCapLookAndFeel.SphereRadius = radius;
    }

    public void SetAxisBorderCullAlphaScale(float scale)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.BorderCircleCullAlphaScale = scale;
    }

    public void SetAxisBorderType(GizmoCircle3DBorderType borderType)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.CircleBorderType = borderType;
    }

    public void SetAxisTorusThickness(float thickness)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.BorderTorusThickness = thickness;
    }

    public void SetAxisCylTorusWidth(float width)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.BorderCylTorusWidth = width;
    }

    public void SetAxisCylTorusHeight(float height)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.BorderCylTorusHeight = height;
    }

    public void SetMidCapVisible(bool isVisible) => this._isMidCapVisible = isVisible;

    public void SetMidCapColor(Color color) => this._midCapLookAndFeel.Color = color;

    public void SetHoveredMidCapColor(Color color) => this._midCapLookAndFeel.HoveredColor = color;

    public void SetMidCapBorderVisible(bool isVisible)
    {
      this._midCapLookAndFeel.IsSphereBorderVisible = isVisible;
    }

    public void SetMidCapBorderColor(Color color)
    {
      this._midCapLookAndFeel.SphereBorderColor = color;
    }

    public void SetAxisBorderColor(int axisIndex, Color color)
    {
      this._axesLookAndFeel[axisIndex].BorderColor = color;
    }

    public void SetHoveredColor(Color hoveredColor)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
      {
        slider3DlookAndFeel.HoveredColor = hoveredColor;
        slider3DlookAndFeel.HoveredBorderColor = hoveredColor;
      }
    }

    public void SetRotationArcColor(Color color)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.RotationArcLookAndFeel.Color = color;
      this._camLookSliderLookAndFeel.RotationArcLookAndFeel.Color = color;
    }

    public void SetRotationArcBorderColor(Color color)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.RotationArcLookAndFeel.BorderColor = color;
      this._camLookSliderLookAndFeel.RotationArcLookAndFeel.BorderColor = color;
    }

    public void SetUseShortestRotationArc(bool useShortest)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.RotationArcLookAndFeel.UseShortestRotation = useShortest;
      this._camLookSliderLookAndFeel.RotationArcLookAndFeel.UseShortestRotation = useShortest;
    }

    public void SetRotationArcVisible(bool isVisible)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._axesLookAndFeel)
        slider3DlookAndFeel.IsRotationArcVisible = isVisible;
      this._camLookSliderLookAndFeel.IsRotationArcVisible = isVisible;
    }

    public void SetCamLookSliderRadiusOffset(float offset)
    {
      this._camLookSliderRadiusOffset = Mathf.Max(0.0f, offset);
    }

    public void SetCamLookSliderBorderColor(Color color)
    {
      this._camLookSliderLookAndFeel.BorderColor = color;
    }

    public void SetCamLookSliderHoveredBorderColor(Color color)
    {
      this._camLookSliderLookAndFeel.HoveredBorderColor = color;
    }

    public void SetCamLookSliderVisible(bool isVisible) => this._isCamLookSliderVisible = isVisible;

    public void SetCamLookSliderPolyBorderType(GizmoPolygon2DBorderType polyBorderType)
    {
      this._camLookSliderLookAndFeel.PolygonBorderType = polyBorderType;
    }

    public void SetCamLookSliderPolyBorderThickness(float thickness)
    {
      this._camLookSliderLookAndFeel.BorderPolyThickness = thickness;
    }

    public void ConnectSliderLookAndFeel(GizmoPlaneSlider3D slider, int axisIndex)
    {
      slider.SharedLookAndFeel = this._axesLookAndFeel[axisIndex];
    }

    public void ConnectMidCapLookAndFeel(GizmoCap3D cap)
    {
      cap.SharedLookAndFeel = this._midCapLookAndFeel;
    }

    public void ConnectCamLookSliderLookAndFeel(GizmoPlaneSlider2D slider)
    {
      slider.SharedLookAndFeel = this._camLookSliderLookAndFeel;
    }
  }
}
