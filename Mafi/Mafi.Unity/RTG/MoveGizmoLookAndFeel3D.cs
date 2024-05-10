// Decompiled with JetBrains decompiler
// Type: RTG.MoveGizmoLookAndFeel3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  [Serializable]
  public class MoveGizmoLookAndFeel3D : Settings
  {
    [SerializeField]
    private bool _isMidCapVisible;
    [SerializeField]
    private GizmoCap3DLookAndFeel _midCapLookAndFeel;
    [SerializeField]
    private GizmoCap2DLookAndFeel _vertSnapCapLookAndFeel;
    [SerializeField]
    private bool[] _sglSliderVis;
    [SerializeField]
    private bool[] _sglSliderCapVis;
    [SerializeField]
    private bool[] _dblSliderVis;
    [SerializeField]
    private GizmoLineSlider3DLookAndFeel[] _sglSlidersLookAndFeel;
    [SerializeField]
    private GizmoPlaneSlider3DLookAndFeel[] _dblSlidersLookAndFeel;

    public float Scale => this._midCapLookAndFeel.Scale;

    public bool UseZoomFactor => this._midCapLookAndFeel.UseZoomFactor;

    public float SliderLength => this._sglSlidersLookAndFeel[0].Length;

    public float BoxSliderHeight => this._sglSlidersLookAndFeel[0].BoxHeight;

    public float BoxSliderDepth => this._sglSlidersLookAndFeel[0].BoxDepth;

    public float CylinderSliderRadius => this._sglSlidersLookAndFeel[0].CylinderRadius;

    public float SliderBoxCapWidth => this._sglSlidersLookAndFeel[0].CapLookAndFeel.BoxWidth;

    public float SliderBoxCapHeight => this._sglSlidersLookAndFeel[0].CapLookAndFeel.BoxHeight;

    public float SliderBoxCapDepth => this._sglSlidersLookAndFeel[0].CapLookAndFeel.BoxDepth;

    public float SliderConeCapHeight => this._sglSlidersLookAndFeel[0].CapLookAndFeel.ConeHeight;

    public float SliderConeCapBaseRadius
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.ConeRadius;
    }

    public float SliderPyramidCapWidth
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.PyramidWidth;
    }

    public float SliderPyramidCapHeight
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.PyramidHeight;
    }

    public float SliderPyramidCapDepth
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.PyramidDepth;
    }

    public float SliderTriPrismCapWidth
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismWidth;
    }

    public float SliderTriPrismCapHeight
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismHeight;
    }

    public float SliderTriPrismCapDepth
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismDepth;
    }

    public float SliderSphereCapRadius
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.SphereRadius;
    }

    public GizmoFillMode3D SliderFillMode => this._sglSlidersLookAndFeel[0].FillMode;

    public GizmoFillMode3D SliderCapFillMode
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.FillMode;
    }

    public GizmoCap3DType SliderCapType => this._sglSlidersLookAndFeel[0].CapLookAndFeel.CapType;

    public GizmoShadeMode SliderShadeMode => this._sglSlidersLookAndFeel[0].ShadeMode;

    public GizmoShadeMode SliderCapShadeMode
    {
      get => this._sglSlidersLookAndFeel[0].CapLookAndFeel.ShadeMode;
    }

    public GizmoLine3DType SliderLineType => this._sglSlidersLookAndFeel[0].LineType;

    public Color XColor => this.GetSglSliderLookAndFeel(0, AxisSign.Positive).Color;

    public Color YColor => this.GetSglSliderLookAndFeel(1, AxisSign.Positive).Color;

    public Color ZColor => this.GetSglSliderLookAndFeel(2, AxisSign.Positive).Color;

    public float DblSliderSize => this._dblSlidersLookAndFeel[0].QuadWidth;

    public float DblSliderBorderBoxHeight => this._dblSlidersLookAndFeel[0].BorderBoxHeight;

    public float DblSliderBorderBoxDepth => this._dblSlidersLookAndFeel[0].BorderBoxDepth;

    public float DblSliderFillAlpha => this._dblSlidersLookAndFeel[0].Color.a;

    public GizmoShadeMode DblSliderBorderShadeMode
    {
      get => this._dblSlidersLookAndFeel[0].BorderShadeMode;
    }

    public GizmoQuad3DBorderType DblSliderBorderType
    {
      get => this._dblSlidersLookAndFeel[0].QuadBorderType;
    }

    public GizmoFillMode3D DblSliderBorderFillMode => this._dblSlidersLookAndFeel[0].BorderFillMode;

    public float VertSnapCapQuadWidth => this._vertSnapCapLookAndFeel.QuadWidth;

    public float VertSnapCapQuadHeight => this._vertSnapCapLookAndFeel.QuadHeight;

    public float VertSnapCapCircleRadius => this._vertSnapCapLookAndFeel.CircleRadius;

    public Color VertSnapCapColor => this._vertSnapCapLookAndFeel.Color;

    public Color VertSnapCapBorderColor => this._vertSnapCapLookAndFeel.BorderColor;

    public Color VertSnapCapHoveredColor => this._vertSnapCapLookAndFeel.HoveredColor;

    public Color VertSnapCapHoveredBorderColor => this._vertSnapCapLookAndFeel.HoveredBorderColor;

    public GizmoFillMode2D VertSnapCapFillMode => this._vertSnapCapLookAndFeel.FillMode;

    public GizmoCap2DType VertSnapCapType => this._vertSnapCapLookAndFeel.CapType;

    public bool IsMidCapVisible
    {
      get => this._isMidCapVisible;
      set => this._isMidCapVisible = value;
    }

    public float MidCapBoxWidth => this._midCapLookAndFeel.BoxWidth;

    public float MidCapBoxHeight => this._midCapLookAndFeel.BoxHeight;

    public float MidCapBoxDepth => this._midCapLookAndFeel.BoxDepth;

    public float MidCapSphereRadius => this._midCapLookAndFeel.SphereRadius;

    public Color MidCapColor => this._midCapLookAndFeel.Color;

    public GizmoFillMode3D MidCapFillMode => this._midCapLookAndFeel.FillMode;

    public GizmoShadeMode MidCapShadeMode => this._midCapLookAndFeel.ShadeMode;

    public GizmoCap3DType MidCapType => this._midCapLookAndFeel.CapType;

    public Color HoveredColor => this._sglSlidersLookAndFeel[0].HoveredColor;

    public MoveGizmoLookAndFeel3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._midCapLookAndFeel = new GizmoCap3DLookAndFeel();
      this._vertSnapCapLookAndFeel = new GizmoCap2DLookAndFeel();
      this._sglSliderVis = new bool[6];
      this._sglSliderCapVis = new bool[6];
      this._dblSliderVis = new bool[3];
      this._sglSlidersLookAndFeel = new GizmoLineSlider3DLookAndFeel[6];
      this._dblSlidersLookAndFeel = new GizmoPlaneSlider3DLookAndFeel[3];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._sglSlidersLookAndFeel.Length; ++index)
        this._sglSlidersLookAndFeel[index] = new GizmoLineSlider3DLookAndFeel();
      for (int index = 0; index < this._dblSlidersLookAndFeel.Length; ++index)
        this._dblSlidersLookAndFeel[index] = new GizmoPlaneSlider3DLookAndFeel();
      this.SetSliderLength(5.5f);
      this.SetAxisColor(0, RTSystemValues.XAxisColor);
      this.SetAxisColor(1, RTSystemValues.YAxisColor);
      this.SetAxisColor(2, RTSystemValues.ZAxisColor);
      this.SetHoveredColor(RTSystemValues.HoveredAxisColor);
      this.SetDblSliderFillAlpha(RTSystemValues.AxisAlpha);
      this.SetDblSliderSize(1.5f);
      this.SetDblSliderVisible(PlaneId.XY, true);
      this.SetDblSliderVisible(PlaneId.YZ, true);
      this.SetDblSliderVisible(PlaneId.ZX, true);
      this.SetSliderVisible(0, AxisSign.Positive, true);
      this.SetSliderCapVisible(0, AxisSign.Positive, true);
      this.SetSliderVisible(1, AxisSign.Positive, true);
      this.SetSliderCapVisible(1, AxisSign.Positive, true);
      this.SetSliderVisible(2, AxisSign.Positive, true);
      this.SetSliderCapVisible(2, AxisSign.Positive, true);
      this.SetMidCapType(GizmoCap3DType.Box);
      this.SetMidCapSphereRadius(0.67f);
      this.SetMidCapColor(RTSystemValues.CenterAxisColor);
      this.SetVertSnapCapFillMode(GizmoFillMode2D.Border);
      this.SetVertSnapCapColor(Color.white.KeepAllButAlpha(RTSystemValues.AxisAlpha));
      this.SetVertSnapCapBorderColor(Color.white);
      this.SetVertSnapCapHoveredColor(RTSystemValues.HoveredAxisColor.KeepAllButAlpha(RTSystemValues.AxisAlpha));
      this.SetVertSnapCapHoveredBorderColor(RTSystemValues.HoveredAxisColor);
    }

    public bool IsVertSnapCapTypeAllowed(GizmoCap2DType capType)
    {
      return capType == GizmoCap2DType.Circle || capType == GizmoCap2DType.Quad;
    }

    public List<Enum> GetAllowedVertSnapCapTypes()
    {
      return new List<Enum>()
      {
        (Enum) GizmoCap2DType.Circle,
        (Enum) GizmoCap2DType.Quad
      };
    }

    public void SetVertSnapCapType(GizmoCap2DType capType)
    {
      if (!this.IsVertSnapCapTypeAllowed(capType))
        return;
      this._vertSnapCapLookAndFeel.CapType = capType;
    }

    public void SetVertSnapCapQuadWidth(float width)
    {
      this._vertSnapCapLookAndFeel.QuadWidth = width;
    }

    public void SetVertSnapCapQuadHeight(float height)
    {
      this._vertSnapCapLookAndFeel.QuadHeight = height;
    }

    public void SetVertSnapCapCircleRadius(float radius)
    {
      this._vertSnapCapLookAndFeel.CircleRadius = radius;
    }

    public void SetVertSnapCapFillMode(GizmoFillMode2D fillMode)
    {
      this._vertSnapCapLookAndFeel.FillMode = fillMode;
    }

    public void SetVertSnapCapColor(Color color) => this._vertSnapCapLookAndFeel.Color = color;

    public void SetVertSnapCapBorderColor(Color color)
    {
      this._vertSnapCapLookAndFeel.BorderColor = color;
    }

    public void SetVertSnapCapHoveredColor(Color color)
    {
      this._vertSnapCapLookAndFeel.HoveredColor = color;
    }

    public void SetVertSnapCapHoveredBorderColor(Color color)
    {
      this._vertSnapCapLookAndFeel.HoveredBorderColor = color;
    }

    public bool IsMidCapTypeAllowed(GizmoCap3DType capType)
    {
      return capType == GizmoCap3DType.Box || capType == GizmoCap3DType.Sphere;
    }

    public List<Enum> GetAllowedMidCapTypes()
    {
      return new List<Enum>()
      {
        (Enum) GizmoCap3DType.Box,
        (Enum) GizmoCap3DType.Sphere
      };
    }

    public void SetMidCapType(GizmoCap3DType capType)
    {
      if (!this.IsMidCapTypeAllowed(capType))
        return;
      this._midCapLookAndFeel.CapType = capType;
    }

    public void SetMidCapBoxWidth(float width) => this._midCapLookAndFeel.BoxWidth = width;

    public void SetMidCapBoxHeight(float height) => this._midCapLookAndFeel.BoxHeight = height;

    public void SetMidCapBoxDepth(float depth) => this._midCapLookAndFeel.BoxDepth = depth;

    public void SetMidCapSphereRadius(float radius)
    {
      this._midCapLookAndFeel.SphereRadius = radius;
    }

    public void SetMidCapColor(Color color) => this._midCapLookAndFeel.Color = color;

    public bool IsSliderVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._sglSliderVis[axisIndex] : this._sglSliderVis[3 + axisIndex];
    }

    public bool IsDblSliderVisible(PlaneId planeId) => this._dblSliderVis[(int) planeId];

    public bool IsSliderCapVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._sglSliderCapVis[axisIndex] : this._sglSliderCapVis[3 + axisIndex];
    }

    public bool IsPositiveSliderVisible(int axisIndex) => this._sglSliderVis[axisIndex];

    public bool IsPositiveSliderCapVisible(int axisIndex) => this._sglSliderCapVis[axisIndex];

    public bool IsNegativeSliderVisible(int axisIndex) => this._sglSliderVis[3 + axisIndex];

    public bool IsNegativeSliderCapVisible(int axisIndex) => this._sglSliderCapVis[3 + axisIndex];

    public void SetSliderVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._sglSliderVis[axisIndex] = isVisible;
      else
        this._sglSliderVis[3 + axisIndex] = isVisible;
    }

    public void SetDblSliderVisible(PlaneId planeId, bool isVisible)
    {
      this._dblSliderVis[(int) planeId] = isVisible;
    }

    public void SetSliderCapVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._sglSliderCapVis[axisIndex] = isVisible;
      else
        this._sglSliderCapVis[3 + axisIndex] = isVisible;
    }

    public void SetPositiveSliderVisible(int axisIndex, bool isVisible)
    {
      this._sglSliderVis[axisIndex] = isVisible;
    }

    public void SetPositiveCapVisible(int axisIndex, bool isVisible)
    {
      this._sglSliderCapVis[axisIndex] = isVisible;
    }

    public void SetNegativeSliderVisible(int axisIndex, bool isVisible)
    {
      this._sglSliderVis[3 + axisIndex] = isVisible;
    }

    public void SetNegativeCapVisible(int axisIndex, bool isVisible)
    {
      this._sglSliderCapVis[3 + axisIndex] = isVisible;
    }

    public void SetSliderLength(float axisLength)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.Length = axisLength;
    }

    public void SetSliderLineType(GizmoLine3DType lineType)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.LineType = lineType;
    }

    public void SetDblSliderBorderType(GizmoQuad3DBorderType borderType)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
        slider3DlookAndFeel.QuadBorderType = borderType;
    }

    public void SetDblSliderBorderBoxHeight(float height)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
        slider3DlookAndFeel.BorderBoxHeight = height;
    }

    public void SetDblSliderBorderBoxDepth(float depth)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
        slider3DlookAndFeel.BorderBoxDepth = depth;
    }

    public void SetBoxSliderHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.BoxHeight = height;
    }

    public void SetBoxSliderDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.BoxDepth = depth;
    }

    public void SetCylinderSliderRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CylinderRadius = radius;
    }

    public void SetDblSliderSize(float size)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.QuadWidth = size;
        slider3DlookAndFeel.QuadHeight = size;
      }
    }

    public void SetScale(float scale)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.Scale = scale;
        slider3DlookAndFeel.CapLookAndFeel.Scale = scale;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
        slider3DlookAndFeel.Scale = scale;
      this._midCapLookAndFeel.Scale = scale;
    }

    public void SetUseZoomFactor(bool useZoomFactor)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.UseZoomFactor = useZoomFactor;
        slider3DlookAndFeel.CapLookAndFeel.UseZoomFactor = useZoomFactor;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
        slider3DlookAndFeel.UseZoomFactor = useZoomFactor;
      this._midCapLookAndFeel.UseZoomFactor = useZoomFactor;
    }

    public void SetAxisColor(int axisIndex, Color color)
    {
      this.GetSglSliderLookAndFeel(axisIndex, AxisSign.Positive).Color = color;
      this.GetSglSliderLookAndFeel(axisIndex, AxisSign.Positive).CapLookAndFeel.Color = color;
      this.GetSglSliderLookAndFeel(axisIndex, AxisSign.Negative).Color = color;
      this.GetSglSliderLookAndFeel(axisIndex, AxisSign.Negative).CapLookAndFeel.Color = color;
      GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel = (GizmoPlaneSlider3DLookAndFeel) null;
      switch (axisIndex)
      {
        case 0:
          slider3DlookAndFeel = this.GetDblSliderLookAndFeel(PlaneId.YZ);
          break;
        case 1:
          slider3DlookAndFeel = this.GetDblSliderLookAndFeel(PlaneId.ZX);
          break;
        case 2:
          slider3DlookAndFeel = this.GetDblSliderLookAndFeel(PlaneId.XY);
          break;
      }
      slider3DlookAndFeel.Color = color.KeepAllButAlpha(slider3DlookAndFeel.Color.a);
      slider3DlookAndFeel.BorderColor = color;
    }

    public void SetDblSliderFillAlpha(float alpha)
    {
      alpha = Mathf.Clamp(alpha, 0.0f, 1f);
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.Color = slider3DlookAndFeel.Color.KeepAllButAlpha(alpha);
        slider3DlookAndFeel.HoveredColor = slider3DlookAndFeel.HoveredColor.KeepAllButAlpha(alpha);
      }
    }

    public void SetHoveredColor(Color hoveredColor)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.HoveredColor = hoveredColor;
        slider3DlookAndFeel.CapLookAndFeel.HoveredColor = hoveredColor;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.HoveredBorderColor = hoveredColor;
        slider3DlookAndFeel.HoveredColor = hoveredColor.KeepAllButAlpha(slider3DlookAndFeel.Color.a);
      }
    }

    public void SetSliderShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.ShadeMode = shadeMode;
    }

    public void SetSliderCapShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ShadeMode = shadeMode;
    }

    public void SetMidCapShadeMode(GizmoShadeMode shadeMode)
    {
      this._midCapLookAndFeel.ShadeMode = shadeMode;
    }

    public void SetDblSliderBorderShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
        slider3DlookAndFeel.BorderShadeMode = shadeMode;
    }

    public void SetSliderCapType(GizmoCap3DType capType)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.CapType = capType;
    }

    public void SetSliderFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.FillMode = fillMode;
    }

    public void SetSliderCapFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.FillMode = fillMode;
    }

    public void SetMidCapFillMode(GizmoFillMode3D fillMode)
    {
      this._midCapLookAndFeel.FillMode = fillMode;
    }

    public void SetDblSliderBorderFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._dblSlidersLookAndFeel)
        slider3DlookAndFeel.BorderFillMode = fillMode;
    }

    public void SetSliderBoxCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxWidth = width;
    }

    public void SetSliderBoxCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxHeight = height;
    }

    public void SetSliderBoxCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxDepth = depth;
    }

    public void SetSliderConeCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ConeHeight = height;
    }

    public void SetSliderConeCapBaseRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ConeRadius = radius;
    }

    public void SetSliderPyramidCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidWidth = width;
    }

    public void SetSliderPyramidCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidHeight = height;
    }

    public void SetSliderPyramidCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidDepth = depth;
    }

    public void SetSliderTriPrismCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismWidth = width;
    }

    public void SetSliderTriPrismCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismHeight = height;
    }

    public void SetSliderTriPrismCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismDepth = depth;
    }

    public void SetSliderSphereCapRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._sglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.SphereRadius = radius;
    }

    public void ConnectSliderLookAndFeel(
      GizmoLineSlider3D slider,
      int axisIndex,
      AxisSign axisSign)
    {
      slider.SharedLookAndFeel = this.GetSglSliderLookAndFeel(axisIndex, axisSign);
    }

    public void ConnectDblSliderLookAndFeel(GizmoPlaneSlider3D dblSlider, PlaneId planeId)
    {
      dblSlider.SharedLookAndFeel = this.GetDblSliderLookAndFeel(planeId);
    }

    public void ConnectMidCapLookAndFeel(GizmoCap3D midCap)
    {
      midCap.SharedLookAndFeel = this._midCapLookAndFeel;
    }

    public void ConnectVertSnapCapLookAndFeel(GizmoCap2D vertSnapCap)
    {
      vertSnapCap.SharedLookAndFeel = this._vertSnapCapLookAndFeel;
    }

    private GizmoLineSlider3DLookAndFeel GetSglSliderLookAndFeel(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._sglSlidersLookAndFeel[axisIndex] : this._sglSlidersLookAndFeel[3 + axisIndex];
    }

    private GizmoPlaneSlider3DLookAndFeel GetDblSliderLookAndFeel(PlaneId planeId)
    {
      return this._dblSlidersLookAndFeel[(int) planeId];
    }
  }
}
