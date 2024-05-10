// Decompiled with JetBrains decompiler
// Type: RTG.UniversalGizmoLookAndFeel3D
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
  public class UniversalGizmoLookAndFeel3D : Settings
  {
    [SerializeField]
    private UniversalGizmoSettingsCategory _displayCategory;
    [SerializeField]
    private GizmoCap2DLookAndFeel _mvVertSnapCapLookAndFeel;
    [SerializeField]
    private bool[] _mvSglSliderVis;
    [SerializeField]
    private bool[] _mvSglSliderCapVis;
    [SerializeField]
    private bool[] _mvDblSliderVis;
    [SerializeField]
    private GizmoLineSlider3DLookAndFeel[] _mvSglSlidersLookAndFeel;
    [SerializeField]
    private GizmoPlaneSlider3DLookAndFeel[] _mvDblSlidersLookAndFeel;
    [SerializeField]
    private bool _isRtMidCapVisible;
    [SerializeField]
    private GizmoCap3DLookAndFeel _rtMidCapLookAndFeel;
    [SerializeField]
    private bool[] _rtAxesVis;
    [SerializeField]
    private GizmoPlaneSlider3DLookAndFeel[] _rtAxesLookAndFeel;
    [SerializeField]
    private bool _isRtCamLookSliderVisible;
    [SerializeField]
    private float _rtCamLookSliderRadiusOffset;
    [SerializeField]
    private GizmoPlaneSlider2DLookAndFeel _rtCamLookSliderLookAndFeel;
    [SerializeField]
    private GizmoCap3DLookAndFeel _scMidCapLookAndFeel;
    [SerializeField]
    private bool[] _scSglSliderVis;
    [SerializeField]
    private bool[] _scSglSliderCapVis;
    [SerializeField]
    private bool[] _scDblSliderVis;
    [SerializeField]
    private bool _isScMidCapVisible;
    [SerializeField]
    private GizmoScaleGuideLookAndFeel _scScaleGuideLookAndFeel;
    [SerializeField]
    private bool _isScScaleGuideVisible;
    [SerializeField]
    private GizmoLineSlider3DLookAndFeel[] _scSglSlidersLookAndFeel;
    [SerializeField]
    private GizmoPlaneSlider3DLookAndFeel[] _scDblSlidersLookAndFeel;

    public float MvScale => this._mvSglSlidersLookAndFeel[0].Scale;

    public bool MvUseZoomFactor => this._mvSglSlidersLookAndFeel[0].UseZoomFactor;

    public float MvSliderLength => this._mvSglSlidersLookAndFeel[0].Length;

    public float MvBoxSliderHeight => this._mvSglSlidersLookAndFeel[0].BoxHeight;

    public float MvBoxSliderDepth => this._mvSglSlidersLookAndFeel[0].BoxDepth;

    public float MvCylinderSliderRadius => this._mvSglSlidersLookAndFeel[0].CylinderRadius;

    public float MvSliderBoxCapWidth => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.BoxWidth;

    public float MvSliderBoxCapHeight => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.BoxHeight;

    public float MvSliderBoxCapDepth => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.BoxDepth;

    public float MvSliderConeCapHeight
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.ConeHeight;
    }

    public float MvSliderConeCapBaseRadius
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.ConeRadius;
    }

    public float MvSliderPyramidCapWidth
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.PyramidWidth;
    }

    public float MvSliderPyramidCapHeight
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.PyramidHeight;
    }

    public float MvSliderPyramidCapDepth
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.PyramidDepth;
    }

    public float MvSliderTriPrismCapWidth
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismWidth;
    }

    public float MvSliderTriPrismCapHeight
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismHeight;
    }

    public float MvSliderTriPrismCapDepth
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismDepth;
    }

    public float MvSliderSphereCapRadius
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.SphereRadius;
    }

    public GizmoFillMode3D MvSliderFillMode => this._mvSglSlidersLookAndFeel[0].FillMode;

    public GizmoFillMode3D MvSliderCapFillMode
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.FillMode;
    }

    public GizmoCap3DType MvSliderCapType
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.CapType;
    }

    public GizmoShadeMode MvSliderShadeMode => this._mvSglSlidersLookAndFeel[0].ShadeMode;

    public GizmoShadeMode MvSliderCapShadeMode
    {
      get => this._mvSglSlidersLookAndFeel[0].CapLookAndFeel.ShadeMode;
    }

    public GizmoLine3DType MvSliderLineType => this._mvSglSlidersLookAndFeel[0].LineType;

    public Color MvPXColor => this.GetMvSglSliderLookAndFeel(0, AxisSign.Positive).Color;

    public Color MvNXColor => this.GetMvSglSliderLookAndFeel(0, AxisSign.Negative).Color;

    public Color MvPYColor => this.GetMvSglSliderLookAndFeel(1, AxisSign.Positive).Color;

    public Color MvNYColor => this.GetMvSglSliderLookAndFeel(1, AxisSign.Negative).Color;

    public Color MvPZColor => this.GetMvSglSliderLookAndFeel(2, AxisSign.Positive).Color;

    public Color MvNZColor => this.GetMvSglSliderLookAndFeel(2, AxisSign.Negative).Color;

    public float MvDblSliderSize => this._mvDblSlidersLookAndFeel[0].QuadWidth;

    public float MvDblSliderBorderBoxHeight => this._mvDblSlidersLookAndFeel[0].BorderBoxHeight;

    public float MvDblSliderBorderBoxDepth => this._mvDblSlidersLookAndFeel[0].BorderBoxDepth;

    public float MvDblSliderFillAlpha => this._mvDblSlidersLookAndFeel[0].Color.a;

    public GizmoShadeMode MvDblSliderBorderShadeMode
    {
      get => this._mvDblSlidersLookAndFeel[0].BorderShadeMode;
    }

    public GizmoQuad3DBorderType MvDblSliderBorderType
    {
      get => this._mvDblSlidersLookAndFeel[0].QuadBorderType;
    }

    public GizmoFillMode3D MvDblSliderBorderFillMode
    {
      get => this._mvDblSlidersLookAndFeel[0].BorderFillMode;
    }

    public float MvVertSnapCapQuadWidth => this._mvVertSnapCapLookAndFeel.QuadWidth;

    public float MvVertSnapCapQuadHeight => this._mvVertSnapCapLookAndFeel.QuadHeight;

    public float MvVertSnapCapCircleRadius => this._mvVertSnapCapLookAndFeel.CircleRadius;

    public Color MvVertSnapCapColor => this._mvVertSnapCapLookAndFeel.Color;

    public Color MvVertSnapCapBorderColor => this._mvVertSnapCapLookAndFeel.BorderColor;

    public Color MvVertSnapCapHoveredColor => this._mvVertSnapCapLookAndFeel.HoveredColor;

    public Color MvVertSnapCapHoveredBorderColor
    {
      get => this._mvVertSnapCapLookAndFeel.HoveredBorderColor;
    }

    public GizmoFillMode2D MvVertSnapCapFillMode => this._mvVertSnapCapLookAndFeel.FillMode;

    public GizmoCap2DType MvVertSnapCapType => this._mvVertSnapCapLookAndFeel.CapType;

    public Color MvHoveredColor => this._mvSglSlidersLookAndFeel[0].HoveredColor;

    public float RtScale => this._rtMidCapLookAndFeel.Scale;

    public float RtRadius => this._rtMidCapLookAndFeel.SphereRadius;

    public bool RtUseZoomFactor => this._rtMidCapLookAndFeel.UseZoomFactor;

    public Color RtXBorderColor => this._rtAxesLookAndFeel[0].BorderColor;

    public Color RtYBorderColor => this._rtAxesLookAndFeel[1].BorderColor;

    public Color RtZBorderColor => this._rtAxesLookAndFeel[2].BorderColor;

    public Color RtHoveredColor => this._rtAxesLookAndFeel[0].HoveredColor;

    public float RtAxisTorusThickness => this._rtAxesLookAndFeel[0].BorderTorusThickness;

    public float RtAxisCylTorusWidth => this._rtAxesLookAndFeel[0].BorderCylTorusWidth;

    public float RtAxisCylTorusHeight => this._rtAxesLookAndFeel[0].BorderCylTorusHeight;

    public float RtAxisCullAlphaScale => this._rtAxesLookAndFeel[0].BorderCircleCullAlphaScale;

    public GizmoShadeMode RtShadeMode => this._rtMidCapLookAndFeel.ShadeMode;

    public GizmoCircle3DBorderType RtAxisBorderType => this._rtAxesLookAndFeel[0].CircleBorderType;

    public GizmoFillMode3D RtAxisBorderFillMode => this._rtAxesLookAndFeel[0].BorderFillMode;

    public int RtNumAxisTorusWireAxialSlices
    {
      get => this._rtAxesLookAndFeel[0].NumBorderTorusWireAxialSlices;
    }

    public Color RtRotationArcColor => this._rtAxesLookAndFeel[0].RotationArcLookAndFeel.Color;

    public Color RtRotationArcBorderColor
    {
      get => this._rtAxesLookAndFeel[0].RotationArcLookAndFeel.BorderColor;
    }

    public bool RtUseShortestRotationArc
    {
      get => this._rtAxesLookAndFeel[0].RotationArcLookAndFeel.UseShortestRotation;
    }

    public bool IsRtRotationArcVisible => this._rtAxesLookAndFeel[0].IsRotationArcVisible;

    public Color RtMidCapColor => this._rtMidCapLookAndFeel.Color;

    public Color RtHoveredMidCapColor => this._rtMidCapLookAndFeel.HoveredColor;

    public bool IsRtMidCapVisible => this._isRtMidCapVisible;

    public bool IsRtMidCapBorderVisible => this._rtMidCapLookAndFeel.IsSphereBorderVisible;

    public float RtCamLookSliderRadiusOffset => this._rtCamLookSliderRadiusOffset;

    public Color RtCamLookSliderBorderColor => this._rtCamLookSliderLookAndFeel.BorderColor;

    public Color RtCamLookSliderHoveredBorderColor
    {
      get => this._rtCamLookSliderLookAndFeel.HoveredBorderColor;
    }

    public GizmoPolygon2DBorderType RtCamLookSliderPolyBorderType
    {
      get => this._rtCamLookSliderLookAndFeel.PolygonBorderType;
    }

    public float RtCamLookSliderPolyBorderThickness
    {
      get => this._rtCamLookSliderLookAndFeel.BorderPolyThickness;
    }

    public bool IsRtCamLookSliderVisible => this._isRtCamLookSliderVisible;

    public float ScScale => this._scMidCapLookAndFeel.Scale;

    public bool ScUseZoomFactor => this._scMidCapLookAndFeel.UseZoomFactor;

    public float ScSliderLength => this._scSglSlidersLookAndFeel[0].Length;

    public float ScBoxSliderHeight => this._scSglSlidersLookAndFeel[0].BoxHeight;

    public float ScBoxSliderDepth => this._scSglSlidersLookAndFeel[0].BoxDepth;

    public float ScCylinderSliderRadius => this._scSglSlidersLookAndFeel[0].CylinderRadius;

    public float ScSliderBoxCapWidth => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.BoxWidth;

    public float ScSliderBoxCapHeight => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.BoxHeight;

    public float ScSliderBoxCapDepth => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.BoxDepth;

    public float ScSliderConeCapHeight
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.ConeHeight;
    }

    public float ScSliderConeCapBaseRadius
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.ConeRadius;
    }

    public float ScSliderPyramidCapWidth
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.PyramidWidth;
    }

    public float ScSliderPyramidCapHeight
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.PyramidHeight;
    }

    public float ScSliderPyramidCapDepth
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.PyramidDepth;
    }

    public float ScSliderTriPrismCapWidth
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismWidth;
    }

    public float ScSliderTriPrismCapHeight
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismHeight;
    }

    public float ScSliderTriPrismCapDepth
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.TrPrismDepth;
    }

    public float ScSliderSphereCapRadius
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.SphereRadius;
    }

    public GizmoFillMode3D ScSliderFillMode => this._scSglSlidersLookAndFeel[0].FillMode;

    public GizmoFillMode3D ScSliderCapFillMode
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.FillMode;
    }

    public GizmoCap3DType ScSliderCapType
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.CapType;
    }

    public GizmoShadeMode ScSliderShadeMode => this._scSglSlidersLookAndFeel[0].ShadeMode;

    public GizmoShadeMode ScSliderCapShadeMode
    {
      get => this._scSglSlidersLookAndFeel[0].CapLookAndFeel.ShadeMode;
    }

    public GizmoLine3DType ScSliderLineType => this._scSglSlidersLookAndFeel[0].LineType;

    public Color ScPXColor => this.GetScSglSliderLookAndFeel(0, AxisSign.Positive).Color;

    public Color ScNXColor => this.GetScSglSliderLookAndFeel(0, AxisSign.Negative).Color;

    public Color ScPYColor => this.GetScSglSliderLookAndFeel(1, AxisSign.Positive).Color;

    public Color ScNYColor => this.GetScSglSliderLookAndFeel(1, AxisSign.Negative).Color;

    public Color ScPZColor => this.GetScSglSliderLookAndFeel(2, AxisSign.Positive).Color;

    public Color ScNZColor => this.GetScSglSliderLookAndFeel(2, AxisSign.Negative).Color;

    public float ScDblSliderSize => this._scDblSlidersLookAndFeel[0].RATriangleXLength;

    public float ScDblSliderFillAlpha => this._scDblSlidersLookAndFeel[0].Color.a;

    public float ScMidCapBoxWidth => this._scMidCapLookAndFeel.BoxWidth;

    public float ScMidCapBoxHeight => this._scMidCapLookAndFeel.BoxHeight;

    public float ScMidCapBoxDepth => this._scMidCapLookAndFeel.BoxDepth;

    public float ScMidCapSphereRadius => this._scMidCapLookAndFeel.SphereRadius;

    public GizmoCap3DType ScMidCapType => this._scMidCapLookAndFeel.CapType;

    public GizmoShadeMode ScMidCapShadeMode => this._scMidCapLookAndFeel.ShadeMode;

    public GizmoFillMode3D ScMidCapFillMode => this._scMidCapLookAndFeel.FillMode;

    public bool IsScMidCapVisible => this._isScMidCapVisible;

    public Color ScMidCapColor => this._scMidCapLookAndFeel.Color;

    public Color ScHoveredColor => this._scSglSlidersLookAndFeel[0].HoveredColor;

    public bool IsScScaleGuideVisible => this._isScScaleGuideVisible;

    public float ScScaleGuideAxisLength => this._scScaleGuideLookAndFeel.AxisLength;

    public UniversalGizmoSettingsCategory DisplayCategory
    {
      get => this._displayCategory;
      set => this._displayCategory = value;
    }

    public UniversalGizmoLookAndFeel3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._mvVertSnapCapLookAndFeel = new GizmoCap2DLookAndFeel();
      this._mvSglSliderVis = new bool[6];
      this._mvSglSliderCapVis = new bool[6];
      this._mvDblSliderVis = new bool[3];
      this._mvSglSlidersLookAndFeel = new GizmoLineSlider3DLookAndFeel[6];
      this._mvDblSlidersLookAndFeel = new GizmoPlaneSlider3DLookAndFeel[3];
      this._isRtMidCapVisible = true;
      this._rtMidCapLookAndFeel = new GizmoCap3DLookAndFeel();
      this._rtAxesVis = new bool[3];
      this._rtAxesLookAndFeel = new GizmoPlaneSlider3DLookAndFeel[3];
      this._isRtCamLookSliderVisible = true;
      this._rtCamLookSliderRadiusOffset = 0.65f;
      this._rtCamLookSliderLookAndFeel = new GizmoPlaneSlider2DLookAndFeel();
      this._scMidCapLookAndFeel = new GizmoCap3DLookAndFeel();
      this._scSglSliderVis = new bool[6];
      this._scSglSliderCapVis = new bool[6];
      this._scDblSliderVis = new bool[3];
      this._isScMidCapVisible = true;
      this._scScaleGuideLookAndFeel = new GizmoScaleGuideLookAndFeel();
      this._isScScaleGuideVisible = true;
      this._scSglSlidersLookAndFeel = new GizmoLineSlider3DLookAndFeel[6];
      this._scDblSlidersLookAndFeel = new GizmoPlaneSlider3DLookAndFeel[3];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._mvSglSlidersLookAndFeel.Length; ++index)
        this._mvSglSlidersLookAndFeel[index] = new GizmoLineSlider3DLookAndFeel();
      for (int index = 0; index < this._mvDblSlidersLookAndFeel.Length; ++index)
        this._mvDblSlidersLookAndFeel[index] = new GizmoPlaneSlider3DLookAndFeel();
      this.SetMvSliderLength(5.5f);
      this.SetMvAxisColor(0, RTSystemValues.XAxisColor);
      this.SetMvAxisColor(1, RTSystemValues.YAxisColor);
      this.SetMvAxisColor(2, RTSystemValues.ZAxisColor);
      this.SetMvHoveredColor(RTSystemValues.HoveredAxisColor);
      this.SetMvDblSliderFillAlpha(RTSystemValues.AxisAlpha);
      this.SetMvDblSliderSize(1.5f);
      this.SetMvDblSliderVisible(PlaneId.XY, true);
      this.SetMvDblSliderVisible(PlaneId.YZ, true);
      this.SetMvDblSliderVisible(PlaneId.ZX, true);
      this.SetMvSliderVisible(0, AxisSign.Positive, true);
      this.SetMvSliderCapVisible(0, AxisSign.Positive, true);
      this.SetMvSliderVisible(1, AxisSign.Positive, true);
      this.SetMvSliderCapVisible(1, AxisSign.Positive, true);
      this.SetMvSliderVisible(2, AxisSign.Positive, true);
      this.SetMvSliderCapVisible(2, AxisSign.Positive, true);
      this.SetMvVertSnapCapFillMode(GizmoFillMode2D.Border);
      this.SetMvVertSnapCapColor(Color.white.KeepAllButAlpha(RTSystemValues.AxisAlpha));
      this.SetMvVertSnapCapBorderColor(Color.white);
      this.SetMvVertSnapCapHoveredColor(RTSystemValues.HoveredAxisColor.KeepAllButAlpha(RTSystemValues.AxisAlpha));
      this.SetMvVertSnapCapHoveredBorderColor(RTSystemValues.HoveredAxisColor);
      for (int index = 0; index < this._rtAxesLookAndFeel.Length; ++index)
      {
        this._rtAxesLookAndFeel[index] = new GizmoPlaneSlider3DLookAndFeel();
        this._rtAxesLookAndFeel[index].PlaneType = GizmoPlane3DType.Circle;
      }
      this.SetRtAxisVisible(0, true);
      this.SetRtAxisVisible(1, true);
      this.SetRtAxisVisible(2, true);
      this._rtMidCapLookAndFeel.CapType = GizmoCap3DType.Sphere;
      this._rtCamLookSliderLookAndFeel.PlaneType = GizmoPlane2DType.Polygon;
      Color color = new Color(0.3f, 0.3f, 0.3f, 0.12f);
      this.SetRtMidCapColor(color);
      this.SetRtHoveredMidCapColor(color);
      this.SetRtMidCapBorderVisible(true);
      this.SetRtMidCapBorderColor(Color.white);
      this.SetRtRadius(6.5f);
      this.SetRtAxisBorderColor(0, RTSystemValues.XAxisColor);
      this.SetRtAxisBorderColor(1, RTSystemValues.YAxisColor);
      this.SetRtAxisBorderColor(2, RTSystemValues.ZAxisColor);
      this.SetRtHoveredColor(RTSystemValues.HoveredAxisColor);
      this.SetRtCamLookSliderPolyBorderThickness(4f);
      this.SetRtCamLookSliderBorderColor(Color.white);
      this.SetRtCamLookSliderHoveredBorderColor(RTSystemValues.HoveredAxisColor);
      this.SetRtNumAxisTorusWireAxialSlices(2);
      for (int index = 0; index < this._scSglSlidersLookAndFeel.Length; ++index)
        this._scSglSlidersLookAndFeel[index] = new GizmoLineSlider3DLookAndFeel();
      for (int index = 0; index < this._scDblSlidersLookAndFeel.Length; ++index)
      {
        this._scDblSlidersLookAndFeel[index] = new GizmoPlaneSlider3DLookAndFeel();
        this._scDblSlidersLookAndFeel[index].PlaneType = GizmoPlane3DType.RATriangle;
      }
      this.SetScSliderCapType(GizmoCap3DType.Box);
      this.SetScSliderLength(5.5f);
      this.SetScAxisColor(0, RTSystemValues.XAxisColor);
      this.SetScAxisColor(1, RTSystemValues.YAxisColor);
      this.SetScAxisColor(2, RTSystemValues.ZAxisColor);
      this.SetScHoveredColor(RTSystemValues.HoveredAxisColor);
      this.SetScSliderVisible(0, AxisSign.Positive, true);
      this.SetScSliderCapVisible(0, AxisSign.Positive, true);
      this.SetScSliderVisible(1, AxisSign.Positive, true);
      this.SetScSliderCapVisible(1, AxisSign.Positive, true);
      this.SetScSliderVisible(2, AxisSign.Positive, true);
      this.SetScSliderCapVisible(2, AxisSign.Positive, true);
      this.SetScMidCapColor(RTSystemValues.CenterAxisColor);
      this.SetScMidCapType(GizmoCap3DType.Box);
      this.SetScMidCapBoxWidth(0.9f);
      this.SetScMidCapBoxHeight(0.9f);
      this.SetScMidCapBoxDepth(0.9f);
      this.SetScMidCapSphereRadius(0.65f);
      this.SetScDblSliderFillAlpha(RTSystemValues.AxisAlpha);
      this.SetScDblSliderSize(1.9f);
      this.SetScDblSliderVisible(PlaneId.XY, true);
      this.SetScDblSliderVisible(PlaneId.YZ, true);
      this.SetScDblSliderVisible(PlaneId.ZX, true);
    }

    public bool IsMvVertSnapCapTypeAllowed(GizmoCap2DType capType)
    {
      return capType == GizmoCap2DType.Circle || capType == GizmoCap2DType.Quad;
    }

    public List<Enum> GetAllowedMvVertSnapCapTypes()
    {
      return new List<Enum>()
      {
        (Enum) GizmoCap2DType.Circle,
        (Enum) GizmoCap2DType.Quad
      };
    }

    public void SetMvVertSnapCapType(GizmoCap2DType capType)
    {
      if (!this.IsMvVertSnapCapTypeAllowed(capType))
        return;
      this._mvVertSnapCapLookAndFeel.CapType = capType;
    }

    public void SetMvVertSnapCapQuadWidth(float width)
    {
      this._mvVertSnapCapLookAndFeel.QuadWidth = width;
    }

    public void SetMvVertSnapCapQuadHeight(float height)
    {
      this._mvVertSnapCapLookAndFeel.QuadHeight = height;
    }

    public void SetMvVertSnapCapCircleRadius(float radius)
    {
      this._mvVertSnapCapLookAndFeel.CircleRadius = radius;
    }

    public void SetMvVertSnapCapFillMode(GizmoFillMode2D fillMode)
    {
      this._mvVertSnapCapLookAndFeel.FillMode = fillMode;
    }

    public void SetMvVertSnapCapColor(Color color) => this._mvVertSnapCapLookAndFeel.Color = color;

    public void SetMvVertSnapCapBorderColor(Color color)
    {
      this._mvVertSnapCapLookAndFeel.BorderColor = color;
    }

    public void SetMvVertSnapCapHoveredColor(Color color)
    {
      this._mvVertSnapCapLookAndFeel.HoveredColor = color;
    }

    public void SetMvVertSnapCapHoveredBorderColor(Color color)
    {
      this._mvVertSnapCapLookAndFeel.HoveredBorderColor = color;
    }

    public bool IsMvSliderVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._mvSglSliderVis[axisIndex] : this._mvSglSliderVis[3 + axisIndex];
    }

    public bool IsMvDblSliderVisible(PlaneId planeId) => this._mvDblSliderVis[(int) planeId];

    public bool IsMvSliderCapVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._mvSglSliderCapVis[axisIndex] : this._mvSglSliderCapVis[3 + axisIndex];
    }

    public bool IsMvPositiveSliderVisible(int axisIndex) => this._mvSglSliderVis[axisIndex];

    public bool IsMvPositiveSliderCapVisible(int axisIndex) => this._mvSglSliderCapVis[axisIndex];

    public bool IsMvNegativeSliderVisible(int axisIndex) => this._mvSglSliderVis[3 + axisIndex];

    public bool IsMvNegativeSliderCapVisible(int axisIndex)
    {
      return this._mvSglSliderCapVis[3 + axisIndex];
    }

    public void SetMvSliderVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._mvSglSliderVis[axisIndex] = isVisible;
      else
        this._mvSglSliderVis[3 + axisIndex] = isVisible;
    }

    public void SetMvDblSliderVisible(PlaneId planeId, bool isVisible)
    {
      this._mvDblSliderVis[(int) planeId] = isVisible;
    }

    public void SetMvSliderCapVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._mvSglSliderCapVis[axisIndex] = isVisible;
      else
        this._mvSglSliderCapVis[3 + axisIndex] = isVisible;
    }

    public void SetMvPositiveSliderVisible(int axisIndex, bool isVisible)
    {
      this._mvSglSliderVis[axisIndex] = isVisible;
    }

    public void SetMvPositiveSliderCapVisible(int axisIndex, bool isVisible)
    {
      this._mvSglSliderCapVis[axisIndex] = isVisible;
    }

    public void SetMvNegativeSliderVisible(int axisIndex, bool isVisible)
    {
      this._mvSglSliderVis[3 + axisIndex] = isVisible;
    }

    public void SetMvNegativeSliderCapVisible(int axisIndex, bool isVisible)
    {
      this._mvSglSliderCapVis[3 + axisIndex] = isVisible;
    }

    public void SetMvSliderLength(float axisLength)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.Length = axisLength;
    }

    public void SetMvSliderLineType(GizmoLine3DType lineType)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.LineType = lineType;
    }

    public void SetMvDblSliderBorderType(GizmoQuad3DBorderType borderType)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
        slider3DlookAndFeel.QuadBorderType = borderType;
    }

    public void SetMvDblSliderBorderBoxHeight(float height)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
        slider3DlookAndFeel.BorderBoxHeight = height;
    }

    public void SetMvDblSliderBorderBoxDepth(float depth)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
        slider3DlookAndFeel.BorderBoxDepth = depth;
    }

    public void SetMvBoxSliderHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.BoxHeight = height;
    }

    public void SetMvBoxSliderDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.BoxDepth = depth;
    }

    public void SetMvCylinderSliderRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CylinderRadius = radius;
    }

    public void SetMvDblSliderSize(float size)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.QuadWidth = size;
        slider3DlookAndFeel.QuadHeight = size;
      }
    }

    public void SetMvScale(float scale)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.Scale = scale;
        slider3DlookAndFeel.CapLookAndFeel.Scale = scale;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
        slider3DlookAndFeel.Scale = scale;
    }

    public void SetMvUseZoomFactor(bool useZoomFactor)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.UseZoomFactor = useZoomFactor;
        slider3DlookAndFeel.CapLookAndFeel.UseZoomFactor = useZoomFactor;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
        slider3DlookAndFeel.UseZoomFactor = useZoomFactor;
    }

    public void SetMvAxisColor(int axisIndex, Color color)
    {
      this.GetMvSglSliderLookAndFeel(axisIndex, AxisSign.Positive).Color = color;
      this.GetMvSglSliderLookAndFeel(axisIndex, AxisSign.Positive).CapLookAndFeel.Color = color;
      this.GetMvSglSliderLookAndFeel(axisIndex, AxisSign.Negative).Color = color;
      this.GetMvSglSliderLookAndFeel(axisIndex, AxisSign.Negative).CapLookAndFeel.Color = color;
      GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel = (GizmoPlaneSlider3DLookAndFeel) null;
      switch (axisIndex)
      {
        case 0:
          slider3DlookAndFeel = this.GetMvDblSliderLookAndFeel(PlaneId.YZ);
          break;
        case 1:
          slider3DlookAndFeel = this.GetMvDblSliderLookAndFeel(PlaneId.ZX);
          break;
        case 2:
          slider3DlookAndFeel = this.GetMvDblSliderLookAndFeel(PlaneId.XY);
          break;
      }
      slider3DlookAndFeel.Color = color.KeepAllButAlpha(slider3DlookAndFeel.Color.a);
      slider3DlookAndFeel.BorderColor = color;
    }

    public void SetMvDblSliderFillAlpha(float alpha)
    {
      alpha = Mathf.Clamp(alpha, 0.0f, 1f);
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.Color = slider3DlookAndFeel.Color.KeepAllButAlpha(alpha);
        slider3DlookAndFeel.HoveredColor = slider3DlookAndFeel.HoveredColor.KeepAllButAlpha(alpha);
      }
    }

    public void SetMvHoveredColor(Color hoveredColor)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.HoveredColor = hoveredColor;
        slider3DlookAndFeel.CapLookAndFeel.HoveredColor = hoveredColor;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.HoveredBorderColor = hoveredColor;
        slider3DlookAndFeel.HoveredColor = hoveredColor.KeepAllButAlpha(slider3DlookAndFeel.Color.a);
      }
    }

    public void SetMvSliderShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.ShadeMode = shadeMode;
    }

    public void SetMvSliderCapShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ShadeMode = shadeMode;
    }

    public void SetMvDblSliderBorderShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
        slider3DlookAndFeel.BorderShadeMode = shadeMode;
    }

    public void SetMvSliderCapType(GizmoCap3DType capType)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.CapType = capType;
    }

    public void SetMvSliderFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.FillMode = fillMode;
    }

    public void SetMvSliderCapFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.FillMode = fillMode;
    }

    public void SetMvDblSliderBorderFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._mvDblSlidersLookAndFeel)
        slider3DlookAndFeel.BorderFillMode = fillMode;
    }

    public void SetMvSliderBoxCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxWidth = width;
    }

    public void SetMvSliderBoxCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxHeight = height;
    }

    public void SetMvSliderBoxCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxDepth = depth;
    }

    public void SetMvSliderConeCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ConeHeight = height;
    }

    public void SetMvSliderConeCapBaseRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ConeRadius = radius;
    }

    public void SetMvSliderPyramidCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidWidth = width;
    }

    public void SetMvSliderPyramidCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidHeight = height;
    }

    public void SetMvSliderPyramidCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidDepth = depth;
    }

    public void SetMvSliderTriPrismCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismWidth = width;
    }

    public void SetMvSliderTriPrismCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismHeight = height;
    }

    public void SetMvSliderTriPrismCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismDepth = depth;
    }

    public void SetMvSliderSphereCapRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._mvSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.SphereRadius = radius;
    }

    public void ConnectMvSliderLookAndFeel(
      GizmoLineSlider3D slider,
      int axisIndex,
      AxisSign axisSign)
    {
      slider.SharedLookAndFeel = this.GetMvSglSliderLookAndFeel(axisIndex, axisSign);
    }

    public void ConnectMvDblSliderLookAndFeel(GizmoPlaneSlider3D dblSlider, PlaneId planeId)
    {
      dblSlider.SharedLookAndFeel = this.GetMvDblSliderLookAndFeel(planeId);
    }

    public void ConnectMvVertSnapCapLookAndFeel(GizmoCap2D vertSnapCap)
    {
      vertSnapCap.SharedLookAndFeel = this._mvVertSnapCapLookAndFeel;
    }

    public void Inherit(MoveGizmoLookAndFeel3D lookAndFeel)
    {
      this.SetMvAxisColor(0, lookAndFeel.XColor);
      this.SetMvAxisColor(1, lookAndFeel.YColor);
      this.SetMvAxisColor(2, lookAndFeel.ZColor);
      this.SetMvBoxSliderDepth(lookAndFeel.BoxSliderDepth);
      this.SetMvBoxSliderHeight(lookAndFeel.BoxSliderHeight);
      this.SetMvCylinderSliderRadius(lookAndFeel.CylinderSliderRadius);
      this.SetMvDblSliderBorderBoxDepth(lookAndFeel.DblSliderBorderBoxDepth);
      this.SetMvDblSliderBorderBoxHeight(lookAndFeel.DblSliderBorderBoxHeight);
      this.SetMvDblSliderBorderFillMode(lookAndFeel.DblSliderBorderFillMode);
      this.SetMvDblSliderBorderShadeMode(lookAndFeel.DblSliderBorderShadeMode);
      this.SetMvDblSliderBorderType(lookAndFeel.DblSliderBorderType);
      this.SetMvDblSliderFillAlpha(lookAndFeel.DblSliderFillAlpha);
      this.SetMvDblSliderSize(lookAndFeel.DblSliderSize);
      this.SetMvDblSliderVisible(PlaneId.XY, lookAndFeel.IsDblSliderVisible(PlaneId.XY));
      this.SetMvDblSliderVisible(PlaneId.YZ, lookAndFeel.IsDblSliderVisible(PlaneId.YZ));
      this.SetMvDblSliderVisible(PlaneId.ZX, lookAndFeel.IsDblSliderVisible(PlaneId.ZX));
      this.SetMvHoveredColor(lookAndFeel.HoveredColor);
      this.SetMvScale(lookAndFeel.Scale);
      this.SetMvSliderBoxCapDepth(lookAndFeel.SliderBoxCapDepth);
      this.SetMvSliderBoxCapHeight(lookAndFeel.SliderBoxCapHeight);
      this.SetMvSliderBoxCapWidth(lookAndFeel.SliderBoxCapWidth);
      this.SetMvSliderCapFillMode(lookAndFeel.SliderCapFillMode);
      this.SetMvSliderCapShadeMode(lookAndFeel.SliderCapShadeMode);
      this.SetMvSliderCapType(lookAndFeel.SliderCapType);
      this.SetMvSliderCapVisible(0, AxisSign.Positive, lookAndFeel.IsSliderCapVisible(0, AxisSign.Positive));
      this.SetMvSliderCapVisible(1, AxisSign.Positive, lookAndFeel.IsSliderCapVisible(1, AxisSign.Positive));
      this.SetMvSliderCapVisible(2, AxisSign.Positive, lookAndFeel.IsSliderCapVisible(2, AxisSign.Positive));
      this.SetMvSliderCapVisible(0, AxisSign.Negative, lookAndFeel.IsSliderCapVisible(0, AxisSign.Negative));
      this.SetMvSliderCapVisible(1, AxisSign.Negative, lookAndFeel.IsSliderCapVisible(1, AxisSign.Negative));
      this.SetMvSliderCapVisible(2, AxisSign.Negative, lookAndFeel.IsSliderCapVisible(2, AxisSign.Negative));
      this.SetMvSliderConeCapHeight(lookAndFeel.SliderConeCapHeight);
      this.SetMvSliderConeCapBaseRadius(lookAndFeel.SliderConeCapBaseRadius);
      this.SetMvSliderFillMode(lookAndFeel.SliderFillMode);
      this.SetMvSliderLength(lookAndFeel.SliderLength);
      this.SetMvSliderLineType(lookAndFeel.SliderLineType);
      this.SetMvSliderPyramidCapDepth(lookAndFeel.SliderPyramidCapDepth);
      this.SetMvSliderPyramidCapHeight(lookAndFeel.SliderPyramidCapHeight);
      this.SetMvSliderPyramidCapWidth(lookAndFeel.SliderPyramidCapWidth);
      this.SetMvSliderShadeMode(lookAndFeel.SliderShadeMode);
      this.SetMvSliderSphereCapRadius(lookAndFeel.SliderSphereCapRadius);
      this.SetMvSliderTriPrismCapDepth(lookAndFeel.SliderTriPrismCapDepth);
      this.SetMvSliderTriPrismCapHeight(lookAndFeel.SliderTriPrismCapHeight);
      this.SetMvSliderTriPrismCapWidth(lookAndFeel.SliderTriPrismCapWidth);
      this.SetMvSliderVisible(0, AxisSign.Positive, lookAndFeel.IsSliderVisible(0, AxisSign.Positive));
      this.SetMvSliderVisible(1, AxisSign.Positive, lookAndFeel.IsSliderVisible(1, AxisSign.Positive));
      this.SetMvSliderVisible(2, AxisSign.Positive, lookAndFeel.IsSliderVisible(2, AxisSign.Positive));
      this.SetMvSliderVisible(0, AxisSign.Negative, lookAndFeel.IsSliderVisible(0, AxisSign.Negative));
      this.SetMvSliderVisible(1, AxisSign.Negative, lookAndFeel.IsSliderVisible(1, AxisSign.Negative));
      this.SetMvSliderVisible(2, AxisSign.Negative, lookAndFeel.IsSliderVisible(2, AxisSign.Negative));
      this.SetMvUseZoomFactor(lookAndFeel.UseZoomFactor);
      this.SetMvVertSnapCapBorderColor(lookAndFeel.VertSnapCapBorderColor);
      this.SetMvVertSnapCapCircleRadius(lookAndFeel.VertSnapCapCircleRadius);
      this.SetMvVertSnapCapColor(lookAndFeel.VertSnapCapColor);
      this.SetMvVertSnapCapFillMode(lookAndFeel.VertSnapCapFillMode);
      this.SetMvVertSnapCapHoveredBorderColor(lookAndFeel.VertSnapCapHoveredBorderColor);
      this.SetMvVertSnapCapHoveredColor(lookAndFeel.VertSnapCapHoveredColor);
      this.SetMvVertSnapCapQuadHeight(lookAndFeel.VertSnapCapQuadHeight);
      this.SetMvVertSnapCapQuadWidth(lookAndFeel.VertSnapCapQuadWidth);
      this.SetMvVertSnapCapType(lookAndFeel.VertSnapCapType);
    }

    private GizmoLineSlider3DLookAndFeel GetMvSglSliderLookAndFeel(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._mvSglSlidersLookAndFeel[axisIndex] : this._mvSglSlidersLookAndFeel[3 + axisIndex];
    }

    private GizmoPlaneSlider3DLookAndFeel GetMvDblSliderLookAndFeel(PlaneId planeId)
    {
      return this._mvDblSlidersLookAndFeel[(int) planeId];
    }

    public bool IsRtAxisVisible(int axisIndex) => this._rtAxesVis[axisIndex];

    public void SetRtAxisVisible(int axisIndex, bool isVisible)
    {
      this._rtAxesVis[axisIndex] = isVisible;
    }

    public void SetRtShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
      {
        slider3DlookAndFeel.ShadeMode = shadeMode;
        slider3DlookAndFeel.BorderShadeMode = shadeMode;
      }
      this._rtMidCapLookAndFeel.ShadeMode = shadeMode;
    }

    public void SetRtAxisBorderFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.BorderFillMode = fillMode;
    }

    public void SetRtNumAxisTorusWireAxialSlices(int numSlices)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.NumBorderTorusWireAxialSlices = numSlices;
    }

    public void SetRtUseZoomFactor(bool useZoomFactor)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.UseZoomFactor = useZoomFactor;
      this._rtMidCapLookAndFeel.UseZoomFactor = useZoomFactor;
    }

    public void SetRtScale(float scale)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.Scale = scale;
      this._rtMidCapLookAndFeel.Scale = scale;
    }

    public void SetRtRadius(float radius)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.CircleRadius = radius;
      this._rtMidCapLookAndFeel.SphereRadius = radius;
    }

    public void SetRtAxisBorderCullAlphaScale(float scale)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.BorderCircleCullAlphaScale = scale;
    }

    public void SetRtAxisBorderType(GizmoCircle3DBorderType borderType)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.CircleBorderType = borderType;
    }

    public void SetRtAxisTorusThickness(float thickness)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.BorderTorusThickness = thickness;
    }

    public void SetRtAxisCylTorusWidth(float width)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.BorderCylTorusWidth = width;
    }

    public void SetRtAxisCylTorusHeight(float height)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.BorderCylTorusHeight = height;
    }

    public void SetRtMidCapVisible(bool isVisible) => this._isRtMidCapVisible = isVisible;

    public void SetRtMidCapColor(Color color) => this._rtMidCapLookAndFeel.Color = color;

    public void SetRtHoveredMidCapColor(Color color)
    {
      this._rtMidCapLookAndFeel.HoveredColor = color;
    }

    public void SetRtMidCapBorderVisible(bool isVisible)
    {
      this._rtMidCapLookAndFeel.IsSphereBorderVisible = isVisible;
    }

    public void SetRtMidCapBorderColor(Color color)
    {
      this._rtMidCapLookAndFeel.SphereBorderColor = color;
    }

    public void SetRtAxisBorderColor(int axisIndex, Color color)
    {
      this._rtAxesLookAndFeel[axisIndex].BorderColor = color;
    }

    public void SetRtHoveredColor(Color hoveredColor)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
      {
        slider3DlookAndFeel.HoveredColor = hoveredColor;
        slider3DlookAndFeel.HoveredBorderColor = hoveredColor;
      }
    }

    public void SetRtRotationArcColor(Color color)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.RotationArcLookAndFeel.Color = color;
      this._rtCamLookSliderLookAndFeel.RotationArcLookAndFeel.Color = color;
    }

    public void SetRtRotationArcBorderColor(Color color)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.RotationArcLookAndFeel.BorderColor = color;
      this._rtCamLookSliderLookAndFeel.RotationArcLookAndFeel.BorderColor = color;
    }

    public void SetRtUseShortestRotationArc(bool useShortest)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.RotationArcLookAndFeel.UseShortestRotation = useShortest;
      this._rtCamLookSliderLookAndFeel.RotationArcLookAndFeel.UseShortestRotation = useShortest;
    }

    public void SetRtRotationArcVisible(bool isVisible)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._rtAxesLookAndFeel)
        slider3DlookAndFeel.IsRotationArcVisible = isVisible;
      this._rtCamLookSliderLookAndFeel.IsRotationArcVisible = isVisible;
    }

    public void SetRtCamLookSliderRadiusOffset(float offset)
    {
      this._rtCamLookSliderRadiusOffset = Mathf.Max(0.0f, offset);
    }

    public void SetRtCamLookSliderBorderColor(Color color)
    {
      this._rtCamLookSliderLookAndFeel.BorderColor = color;
    }

    public void SetRtCamLookSliderHoveredBorderColor(Color color)
    {
      this._rtCamLookSliderLookAndFeel.HoveredBorderColor = color;
    }

    public void SetRtCamLookSliderVisible(bool isVisible)
    {
      this._isRtCamLookSliderVisible = isVisible;
    }

    public void SetRtCamLookSliderPolyBorderType(GizmoPolygon2DBorderType polyBorderType)
    {
      this._rtCamLookSliderLookAndFeel.PolygonBorderType = polyBorderType;
    }

    public void SetRtCamLookSliderPolyBorderThickness(float thickness)
    {
      this._rtCamLookSliderLookAndFeel.BorderPolyThickness = thickness;
    }

    public void ConnectRtSliderLookAndFeel(GizmoPlaneSlider3D slider, int axisIndex)
    {
      slider.SharedLookAndFeel = this._rtAxesLookAndFeel[axisIndex];
    }

    public void ConnectRtMidCapLookAndFeel(GizmoCap3D cap)
    {
      cap.SharedLookAndFeel = this._rtMidCapLookAndFeel;
    }

    public void ConnectRtCamLookSliderLookAndFeel(GizmoPlaneSlider2D slider)
    {
      slider.SharedLookAndFeel = this._rtCamLookSliderLookAndFeel;
    }

    public void Inherit(RotationGizmoLookAndFeel3D lookAndFeel)
    {
      this.SetRtAxisBorderColor(0, lookAndFeel.XBorderColor);
      this.SetRtAxisBorderColor(1, lookAndFeel.YBorderColor);
      this.SetRtAxisBorderColor(2, lookAndFeel.ZBorderColor);
      this.SetRtAxisBorderCullAlphaScale(lookAndFeel.AxisCullAlphaScale);
      this.SetRtAxisBorderFillMode(lookAndFeel.AxisBorderFillMode);
      this.SetRtAxisBorderType(lookAndFeel.AxisBorderType);
      this.SetRtAxisCylTorusHeight(lookAndFeel.AxisCylTorusHeight);
      this.SetRtAxisCylTorusWidth(lookAndFeel.AxisCylTorusWidth);
      this.SetRtAxisTorusThickness(lookAndFeel.AxisTorusThickness);
      this.SetRtAxisVisible(0, lookAndFeel.IsAxisVisible(0));
      this.SetRtAxisVisible(1, lookAndFeel.IsAxisVisible(1));
      this.SetRtAxisVisible(2, lookAndFeel.IsAxisVisible(2));
      this.SetRtCamLookSliderBorderColor(lookAndFeel.CamLookSliderBorderColor);
      this.SetRtCamLookSliderHoveredBorderColor(lookAndFeel.CamLookSliderHoveredBorderColor);
      this.SetRtCamLookSliderPolyBorderThickness(lookAndFeel.CamLookSliderPolyBorderThickness);
      this.SetRtCamLookSliderPolyBorderType(lookAndFeel.CamLookSliderPolyBorderType);
      this.SetRtCamLookSliderRadiusOffset(lookAndFeel.CamLookSliderRadiusOffset);
      this.SetRtCamLookSliderVisible(lookAndFeel.IsCamLookSliderVisible);
      this.SetRtHoveredColor(lookAndFeel.HoveredColor);
      this.SetRtHoveredMidCapColor(lookAndFeel.HoveredMidCapColor);
      this.SetRtMidCapBorderColor(lookAndFeel.MidCapBorderColor);
      this.SetRtMidCapBorderVisible(lookAndFeel.IsMidCapBorderVisible);
      this.SetRtMidCapColor(lookAndFeel.MidCapColor);
      this.SetRtMidCapVisible(lookAndFeel.IsMidCapVisible);
      this.SetRtNumAxisTorusWireAxialSlices(lookAndFeel.NumAxisTorusWireAxialSlices);
      this.SetRtRadius(lookAndFeel.Radius);
      this.SetRtRotationArcBorderColor(lookAndFeel.RotationArcBorderColor);
      this.SetRtRotationArcColor(lookAndFeel.RotationArcColor);
      this.SetRtRotationArcVisible(lookAndFeel.IsRotationArcVisible);
      this.SetRtScale(lookAndFeel.Scale);
      this.SetRtShadeMode(lookAndFeel.ShadeMode);
      this.SetRtUseShortestRotationArc(lookAndFeel.UseShortestRotationArc);
      this.SetRtUseZoomFactor(lookAndFeel.UseZoomFactor);
    }

    public void SetScScaleGuideVisible(bool isVisible) => this._isScScaleGuideVisible = isVisible;

    public bool IsScDblSliderVisible(PlaneId planeId) => this._scDblSliderVis[(int) planeId];

    public void SetScDblSliderVisible(PlaneId planeId, bool isVisible)
    {
      this._scDblSliderVis[(int) planeId] = isVisible;
    }

    public bool IsScSliderVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._scSglSliderVis[axisIndex] : this._scSglSliderVis[3 + axisIndex];
    }

    public bool IsScSliderCapVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._scSglSliderCapVis[axisIndex] : this._scSglSliderCapVis[3 + axisIndex];
    }

    public bool IsScPositiveSliderVisible(int axisIndex) => this._scSglSliderVis[axisIndex];

    public bool IsScPositiveSliderCapVisible(int axisIndex) => this._scSglSliderCapVis[axisIndex];

    public bool IsScNegativeSliderVisible(int axisIndex) => this._scSglSliderVis[3 + axisIndex];

    public bool IsScNegativeSliderCapVisible(int axisIndex)
    {
      return this._scSglSliderCapVis[3 + axisIndex];
    }

    public void SetScSliderVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._scSglSliderVis[axisIndex] = isVisible;
      else
        this._scSglSliderVis[3 + axisIndex] = isVisible;
    }

    public void SetScSliderCapVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._scSglSliderCapVis[axisIndex] = isVisible;
      else
        this._scSglSliderCapVis[3 + axisIndex] = isVisible;
    }

    public void SetScPositiveSliderVisible(int axisIndex, bool isVisible)
    {
      this._scSglSliderVis[axisIndex] = isVisible;
    }

    public void SetScPositiveSliderCapVisible(int axisIndex, bool isVisible)
    {
      this._scSglSliderCapVis[axisIndex] = isVisible;
    }

    public void SetScNegativeSliderVisible(int axisIndex, bool isVisible)
    {
      this._scSglSliderVis[3 + axisIndex] = isVisible;
    }

    public void SetScNegativeSliderCapVisible(int axisIndex, bool isVisible)
    {
      this._scSglSliderCapVis[3 + axisIndex] = isVisible;
    }

    public void SetScSliderLength(float axisLength)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.Length = axisLength;
    }

    public void SetScSliderLineType(GizmoLine3DType lineType)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.LineType = lineType;
    }

    public void SetScBoxSliderHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.BoxHeight = height;
    }

    public void SetScBoxSliderDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.BoxDepth = depth;
    }

    public void SetScCylinderSliderRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CylinderRadius = radius;
    }

    public void SetScScale(float scale)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.Scale = scale;
        slider3DlookAndFeel.CapLookAndFeel.Scale = scale;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._scDblSlidersLookAndFeel)
        slider3DlookAndFeel.Scale = scale;
      this._scMidCapLookAndFeel.Scale = scale;
    }

    public void SetScUseZoomFactor(bool useZoomFactor)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.UseZoomFactor = useZoomFactor;
        slider3DlookAndFeel.CapLookAndFeel.UseZoomFactor = useZoomFactor;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._scDblSlidersLookAndFeel)
        slider3DlookAndFeel.UseZoomFactor = useZoomFactor;
      this._scMidCapLookAndFeel.UseZoomFactor = useZoomFactor;
      this._scScaleGuideLookAndFeel.UseZoomFactor = useZoomFactor;
    }

    public void SetScScaleGuideAxisLength(float length)
    {
      this._scScaleGuideLookAndFeel.AxisLength = length;
    }

    public void SetScAxisColor(int axisIndex, Color color)
    {
      this.GetScSglSliderLookAndFeel(axisIndex, AxisSign.Positive).Color = color;
      this.GetScSglSliderLookAndFeel(axisIndex, AxisSign.Positive).CapLookAndFeel.Color = color;
      this.GetScSglSliderLookAndFeel(axisIndex, AxisSign.Negative).Color = color;
      this.GetScSglSliderLookAndFeel(axisIndex, AxisSign.Negative).CapLookAndFeel.Color = color;
      GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel = (GizmoPlaneSlider3DLookAndFeel) null;
      switch (axisIndex)
      {
        case 0:
          slider3DlookAndFeel = this.GetScDblSliderLookAndFeel(PlaneId.YZ);
          this._scScaleGuideLookAndFeel.XAxisColor = color;
          break;
        case 1:
          slider3DlookAndFeel = this.GetScDblSliderLookAndFeel(PlaneId.ZX);
          this._scScaleGuideLookAndFeel.YAxisColor = color;
          break;
        case 2:
          slider3DlookAndFeel = this.GetScDblSliderLookAndFeel(PlaneId.XY);
          this._scScaleGuideLookAndFeel.ZAxisColor = color;
          break;
      }
      slider3DlookAndFeel.Color = color.KeepAllButAlpha(slider3DlookAndFeel.Color.a);
      slider3DlookAndFeel.BorderColor = color;
    }

    public void SetScDblSliderFillAlpha(float alpha)
    {
      alpha = Mathf.Clamp(alpha, 0.0f, 1f);
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._scDblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.Color = slider3DlookAndFeel.Color.KeepAllButAlpha(alpha);
        slider3DlookAndFeel.HoveredColor = slider3DlookAndFeel.HoveredColor.KeepAllButAlpha(alpha);
      }
    }

    public void SetScMidCapColor(Color color) => this._scMidCapLookAndFeel.Color = color;

    public void SetScMidCapVisible(bool visible) => this._isScMidCapVisible = visible;

    public void SetScHoveredColor(Color hoveredColor)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
      {
        slider3DlookAndFeel.HoveredColor = hoveredColor;
        slider3DlookAndFeel.CapLookAndFeel.HoveredColor = hoveredColor;
      }
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._scDblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.HoveredBorderColor = hoveredColor;
        slider3DlookAndFeel.HoveredColor = hoveredColor.KeepAllButAlpha(slider3DlookAndFeel.Color.a);
      }
      this._scMidCapLookAndFeel.HoveredColor = hoveredColor;
    }

    public void SetScSliderShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.ShadeMode = shadeMode;
    }

    public void SetScSliderCapShadeMode(GizmoShadeMode shadeMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ShadeMode = shadeMode;
    }

    public void SetScMidCapShadeMode(GizmoShadeMode shadeMode)
    {
      this._scMidCapLookAndFeel.ShadeMode = shadeMode;
    }

    public void SetScSliderCapType(GizmoCap3DType capType)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.CapType = capType;
    }

    public void SetScMidCapType(GizmoCap3DType capType)
    {
      if (!this.IsScMidCapTypeAllowed(capType))
        return;
      this._scMidCapLookAndFeel.CapType = capType;
    }

    public bool IsScMidCapTypeAllowed(GizmoCap3DType capType)
    {
      return capType == GizmoCap3DType.Box || capType == GizmoCap3DType.Sphere;
    }

    public List<Enum> GetAllowedScMidCapTypes()
    {
      return new List<Enum>()
      {
        (Enum) GizmoCap3DType.Box,
        (Enum) GizmoCap3DType.Sphere
      };
    }

    public void SetScSliderFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.FillMode = fillMode;
    }

    public void SetScSliderCapFillMode(GizmoFillMode3D fillMode)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.FillMode = fillMode;
    }

    public void SetScMidCapFillMode(GizmoFillMode3D fillMode)
    {
      this._scMidCapLookAndFeel.FillMode = fillMode;
    }

    public void SetScSliderBoxCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxWidth = width;
    }

    public void SetScSliderBoxCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxHeight = height;
    }

    public void SetScSliderBoxCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.BoxDepth = depth;
    }

    public void SetScSliderConeCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ConeHeight = height;
    }

    public void SetScSliderConeCapBaseRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.ConeRadius = radius;
    }

    public void SetScSliderPyramidCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidWidth = width;
    }

    public void SetScSliderPyramidCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidHeight = height;
    }

    public void SetScSliderPyramidCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.PyramidDepth = depth;
    }

    public void SetScSliderTriPrismCapWidth(float width)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismWidth = width;
    }

    public void SetScSliderTriPrismCapHeight(float height)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismHeight = height;
    }

    public void SetScSliderTriPrismCapDepth(float depth)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.TrPrismDepth = depth;
    }

    public void SetScSliderSphereCapRadius(float radius)
    {
      foreach (GizmoLineSlider3DLookAndFeel slider3DlookAndFeel in this._scSglSlidersLookAndFeel)
        slider3DlookAndFeel.CapLookAndFeel.SphereRadius = radius;
    }

    public void SetScMidCapBoxWidth(float width) => this._scMidCapLookAndFeel.BoxWidth = width;

    public void SetScMidCapBoxHeight(float height) => this._scMidCapLookAndFeel.BoxHeight = height;

    public void SetScMidCapBoxDepth(float depth) => this._scMidCapLookAndFeel.BoxDepth = depth;

    public void SetScMidCapSphereRadius(float radius)
    {
      this._scMidCapLookAndFeel.SphereRadius = radius;
    }

    public void SetScDblSliderSize(float size)
    {
      foreach (GizmoPlaneSlider3DLookAndFeel slider3DlookAndFeel in this._scDblSlidersLookAndFeel)
      {
        slider3DlookAndFeel.RATriangleXLength = size;
        slider3DlookAndFeel.RATriangleYLength = size;
      }
    }

    public void ConnectScSliderLookAndFeel(
      GizmoLineSlider3D slider,
      int axisIndex,
      AxisSign axisSign)
    {
      slider.SharedLookAndFeel = this.GetScSglSliderLookAndFeel(axisIndex, axisSign);
    }

    public void ConnectScMidCapLookAndFeel(GizmoCap3D cap)
    {
      cap.SharedLookAndFeel = this._scMidCapLookAndFeel;
    }

    public void ConnectScDblSliderLookAndFeel(GizmoPlaneSlider3D slider, PlaneId planeId)
    {
      slider.SharedLookAndFeel = this.GetScDblSliderLookAndFeel(planeId);
    }

    public void ConnectScGizmoScaleGuideLookAndFeel(GizmoScaleGuide scaleGuide)
    {
      scaleGuide.SharedLookAndFeel = this._scScaleGuideLookAndFeel;
    }

    public void Inherit(ScaleGizmoLookAndFeel3D lookAndFeel)
    {
      this.SetScAxisColor(0, lookAndFeel.XColor);
      this.SetScAxisColor(1, lookAndFeel.YColor);
      this.SetScAxisColor(2, lookAndFeel.ZColor);
      this.SetScBoxSliderDepth(lookAndFeel.BoxSliderDepth);
      this.SetScBoxSliderHeight(lookAndFeel.BoxSliderHeight);
      this.SetScCylinderSliderRadius(lookAndFeel.CylinderSliderRadius);
      this.SetScDblSliderFillAlpha(lookAndFeel.DblSliderFillAlpha);
      this.SetScDblSliderSize(lookAndFeel.DblSliderSize);
      this.SetScDblSliderVisible(PlaneId.XY, lookAndFeel.IsDblSliderVisible(PlaneId.XY));
      this.SetScDblSliderVisible(PlaneId.YZ, lookAndFeel.IsDblSliderVisible(PlaneId.YZ));
      this.SetScDblSliderVisible(PlaneId.ZX, lookAndFeel.IsDblSliderVisible(PlaneId.ZX));
      this.SetScHoveredColor(lookAndFeel.HoveredColor);
      this.SetScMidCapBoxDepth(lookAndFeel.MidCapBoxDepth);
      this.SetScMidCapBoxHeight(lookAndFeel.MidCapBoxHeight);
      this.SetScMidCapBoxWidth(lookAndFeel.MidCapBoxWidth);
      this.SetScMidCapColor(lookAndFeel.MidCapColor);
      this.SetScMidCapFillMode(lookAndFeel.MidCapFillMode);
      this.SetScMidCapShadeMode(lookAndFeel.MidCapShadeMode);
      this.SetScMidCapSphereRadius(lookAndFeel.MidCapSphereRadius);
      this.SetScMidCapType(lookAndFeel.MidCapType);
      this.SetScScale(lookAndFeel.Scale);
      this.SetScScaleGuideAxisLength(lookAndFeel.ScaleGuideAxisLength);
      this.SetScSliderBoxCapDepth(lookAndFeel.SliderBoxCapDepth);
      this.SetScSliderBoxCapHeight(lookAndFeel.SliderBoxCapHeight);
      this.SetScSliderBoxCapWidth(lookAndFeel.SliderBoxCapWidth);
      this.SetScSliderCapFillMode(lookAndFeel.SliderCapFillMode);
      this.SetScSliderCapShadeMode(lookAndFeel.SliderCapShadeMode);
      this.SetScSliderCapType(lookAndFeel.SliderCapType);
      this.SetScSliderCapVisible(0, AxisSign.Positive, lookAndFeel.IsSliderCapVisible(0, AxisSign.Positive));
      this.SetScSliderCapVisible(1, AxisSign.Positive, lookAndFeel.IsSliderCapVisible(1, AxisSign.Positive));
      this.SetScSliderCapVisible(2, AxisSign.Positive, lookAndFeel.IsSliderCapVisible(2, AxisSign.Positive));
      this.SetScSliderCapVisible(0, AxisSign.Negative, lookAndFeel.IsSliderCapVisible(0, AxisSign.Negative));
      this.SetScSliderCapVisible(1, AxisSign.Negative, lookAndFeel.IsSliderCapVisible(1, AxisSign.Negative));
      this.SetScSliderCapVisible(2, AxisSign.Negative, lookAndFeel.IsSliderCapVisible(2, AxisSign.Negative));
      this.SetScSliderConeCapHeight(lookAndFeel.SliderConeCapHeight);
      this.SetScSliderConeCapBaseRadius(lookAndFeel.SliderConeCapBaseRadius);
      this.SetScSliderFillMode(lookAndFeel.SliderFillMode);
      this.SetScSliderLength(lookAndFeel.SliderLength);
      this.SetScSliderLineType(lookAndFeel.SliderLineType);
      this.SetScSliderPyramidCapDepth(lookAndFeel.SliderPyramidCapDepth);
      this.SetScSliderPyramidCapHeight(lookAndFeel.SliderPyramidCapHeight);
      this.SetScSliderPyramidCapWidth(lookAndFeel.SliderPyramidCapWidth);
      this.SetScSliderShadeMode(lookAndFeel.SliderShadeMode);
      this.SetScSliderSphereCapRadius(lookAndFeel.SliderSphereCapRadius);
      this.SetScSliderTriPrismCapDepth(lookAndFeel.SliderTriPrismCapDepth);
      this.SetScSliderTriPrismCapHeight(lookAndFeel.SliderTriPrismCapHeight);
      this.SetScSliderTriPrismCapWidth(lookAndFeel.SliderTriPrismCapWidth);
      this.SetScSliderVisible(0, AxisSign.Positive, lookAndFeel.IsSliderVisible(0, AxisSign.Positive));
      this.SetScSliderVisible(1, AxisSign.Positive, lookAndFeel.IsSliderVisible(1, AxisSign.Positive));
      this.SetScSliderVisible(2, AxisSign.Positive, lookAndFeel.IsSliderVisible(2, AxisSign.Positive));
      this.SetScSliderVisible(0, AxisSign.Negative, lookAndFeel.IsSliderVisible(0, AxisSign.Negative));
      this.SetScSliderVisible(1, AxisSign.Negative, lookAndFeel.IsSliderVisible(1, AxisSign.Negative));
      this.SetScSliderVisible(2, AxisSign.Negative, lookAndFeel.IsSliderVisible(2, AxisSign.Negative));
      this.SetScUseZoomFactor(lookAndFeel.UseZoomFactor);
    }

    private GizmoLineSlider3DLookAndFeel GetScSglSliderLookAndFeel(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._scSglSlidersLookAndFeel[axisIndex] : this._scSglSlidersLookAndFeel[3 + axisIndex];
    }

    private GizmoPlaneSlider3DLookAndFeel GetScDblSliderLookAndFeel(PlaneId planeId)
    {
      return this._scDblSlidersLookAndFeel[(int) planeId];
    }
  }
}
