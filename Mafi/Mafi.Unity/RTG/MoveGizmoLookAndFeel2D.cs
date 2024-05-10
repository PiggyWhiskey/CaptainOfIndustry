// Decompiled with JetBrains decompiler
// Type: RTG.MoveGizmoLookAndFeel2D
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
  public class MoveGizmoLookAndFeel2D : Settings
  {
    [SerializeField]
    private GizmoPlaneSlider2DLookAndFeel _dblSliderLookAndFeel;
    [SerializeField]
    private GizmoLineSlider2DLookAndFeel[] _sglSliderLookAndFeel;
    [SerializeField]
    private bool _isDblSliderVisible;
    [SerializeField]
    private bool[] _sglSliderVis;
    [SerializeField]
    private bool[] _sglSliderCapVis;

    public float Scale => this._sglSliderLookAndFeel[0].Scale;

    public float SliderLength => this._sglSliderLookAndFeel[0].Length;

    public float BoxSliderThickness => this._sglSliderLookAndFeel[0].BoxThickness;

    public float SliderArrowCapHeight => this._sglSliderLookAndFeel[0].CapLookAndFeel.ArrowHeight;

    public float SliderArrowCapBaseRadius
    {
      get => this._sglSliderLookAndFeel[0].CapLookAndFeel.ArrowBaseRadius;
    }

    public float SliderQuadCapWidth => this._sglSliderLookAndFeel[0].CapLookAndFeel.QuadWidth;

    public float SliderQuadCapHeight => this._sglSliderLookAndFeel[0].CapLookAndFeel.QuadHeight;

    public float SliderCircleCapRadius => this._sglSliderLookAndFeel[0].CapLookAndFeel.CircleRadius;

    public float DblSliderQuadWidth => this._dblSliderLookAndFeel.QuadWidth;

    public float DblSliderQuadHeight => this._dblSliderLookAndFeel.QuadHeight;

    public float DblSliderCircleRadius => this._dblSliderLookAndFeel.CircleRadius;

    public Color XColor => this.GetSliderLookAndFeel(0, AxisSign.Positive).Color;

    public Color YColor => this.GetSliderLookAndFeel(1, AxisSign.Positive).Color;

    public Color XBorderColor => this.GetSliderLookAndFeel(0, AxisSign.Positive).BorderColor;

    public Color YBorderColor => this.GetSliderLookAndFeel(1, AxisSign.Positive).BorderColor;

    public Color DblSliderColor => this._dblSliderLookAndFeel.Color;

    public Color DblSliderBorderColor => this._dblSliderLookAndFeel.BorderColor;

    public Color DblSliderHoveredColor => this._dblSliderLookAndFeel.HoveredColor;

    public Color DblSliderHoveredBorderColor => this._dblSliderLookAndFeel.HoveredBorderColor;

    public bool IsDblSliderVisible => this._isDblSliderVisible;

    public Color SliderHoveredColor => this._sglSliderLookAndFeel[0].HoveredColor;

    public Color SliderHoveredBorderColor => this._sglSliderLookAndFeel[0].HoveredBorderColor;

    public GizmoFillMode2D SliderFillMode => this._sglSliderLookAndFeel[0].FillMode;

    public GizmoFillMode2D SliderCapFillMode
    {
      get => this._sglSliderLookAndFeel[0].CapLookAndFeel.FillMode;
    }

    public GizmoFillMode2D DblSliderFillMode => this._dblSliderLookAndFeel.FillMode;

    public GizmoCap2DType SliderCapType => this._sglSliderLookAndFeel[0].CapLookAndFeel.CapType;

    public GizmoLine2DType SliderLineType => this._sglSliderLookAndFeel[0].LineType;

    public GizmoPlane2DType DblSliderPlaneType => this._dblSliderLookAndFeel.PlaneType;

    public MoveGizmoLookAndFeel2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._dblSliderLookAndFeel = new GizmoPlaneSlider2DLookAndFeel();
      this._sglSliderLookAndFeel = new GizmoLineSlider2DLookAndFeel[4];
      this._isDblSliderVisible = true;
      this._sglSliderVis = new bool[4];
      this._sglSliderCapVis = new bool[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._sglSliderLookAndFeel.Length; ++index)
        this._sglSliderLookAndFeel[index] = new GizmoLineSlider2DLookAndFeel();
      this.SetAxisColor(0, RTSystemValues.XAxisColor);
      this.SetAxisColor(1, RTSystemValues.YAxisColor);
      this.SetAxisBorderColor(0, RTSystemValues.XAxisColor);
      this.SetAxisBorderColor(1, RTSystemValues.YAxisColor);
      this.SetSliderHoveredFillColor(RTSystemValues.HoveredAxisColor);
      this.SetSliderHoveredBorderColor(RTSystemValues.HoveredAxisColor);
      this.SetSliderCapType(GizmoCap2DType.Arrow);
      this.SetSliderCapFillMode(GizmoFillMode2D.Filled);
      this.SetSliderFillMode(GizmoFillMode2D.Filled);
      this.SetSliderVisible(0, AxisSign.Positive, true);
      this.SetSliderVisible(1, AxisSign.Positive, true);
      this.SetSliderCapVisible(0, AxisSign.Positive, true);
      this.SetSliderCapVisible(1, AxisSign.Positive, true);
      this.SetDblSliderFillMode(GizmoFillMode2D.Border);
      this.SetDblSliderColor(Color.white.KeepAllButAlpha(RTSystemValues.AxisAlpha));
      this.SetDblSliderBorderColor(Color.white);
      this.SetDblSliderHoveredColor(RTSystemValues.HoveredAxisColor.KeepAllButAlpha(RTSystemValues.AxisAlpha));
      this.SetDblSliderHoveredBorderColor(RTSystemValues.HoveredAxisColor);
    }

    public bool IsDblSliderPlaneTypeAllowed(GizmoPlane2DType planeType)
    {
      return planeType == GizmoPlane2DType.Circle || planeType == GizmoPlane2DType.Quad;
    }

    public List<Enum> GetAllowedDblSliderPlaneTypes()
    {
      return new List<Enum>()
      {
        (Enum) GizmoPlane2DType.Circle,
        (Enum) GizmoPlane2DType.Quad
      };
    }

    public void SetDblSliderVisible(bool isVisible) => this._isDblSliderVisible = isVisible;

    public bool IsSliderVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._sglSliderVis[axisIndex] : this._sglSliderVis[2 + axisIndex];
    }

    public bool IsPositiveSliderVisible(int axisIndex) => this._sglSliderVis[axisIndex];

    public bool IsNegativeSliderVisible(int axisIndex) => this._sglSliderVis[2 + axisIndex];

    public void SetSliderVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._sglSliderVis[axisIndex] = isVisible;
      else
        this._sglSliderVis[2 + axisIndex] = isVisible;
    }

    public bool IsSliderCapVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._sglSliderCapVis[axisIndex] : this._sglSliderCapVis[2 + axisIndex];
    }

    public bool IsPositiveSliderCapVisible(int axisIndex) => this._sglSliderCapVis[axisIndex];

    public bool IsNegativeSliderCapVisible(int axisIndex) => this._sglSliderCapVis[2 + axisIndex];

    public void SetSliderCapVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._sglSliderCapVis[axisIndex] = isVisible;
      else
        this._sglSliderCapVis[2 + axisIndex] = isVisible;
    }

    public void SetAxisColor(int axisIndex, Color color)
    {
      this.GetSliderLookAndFeel(axisIndex, AxisSign.Positive).Color = color;
      this.GetSliderLookAndFeel(axisIndex, AxisSign.Positive).CapLookAndFeel.Color = color;
      this.GetSliderLookAndFeel(axisIndex, AxisSign.Negative).Color = color;
      this.GetSliderLookAndFeel(axisIndex, AxisSign.Negative).CapLookAndFeel.Color = color;
    }

    public void SetAxisBorderColor(int axisIndex, Color color)
    {
      this.GetSliderLookAndFeel(axisIndex, AxisSign.Positive).BorderColor = color;
      this.GetSliderLookAndFeel(axisIndex, AxisSign.Positive).CapLookAndFeel.BorderColor = color;
      this.GetSliderLookAndFeel(axisIndex, AxisSign.Negative).BorderColor = color;
      this.GetSliderLookAndFeel(axisIndex, AxisSign.Negative).CapLookAndFeel.BorderColor = color;
    }

    public void SetSliderHoveredFillColor(Color color)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
      {
        slider2DlookAndFeel.HoveredColor = color;
        slider2DlookAndFeel.CapLookAndFeel.HoveredColor = color;
      }
    }

    public void SetSliderHoveredBorderColor(Color color)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
      {
        slider2DlookAndFeel.HoveredBorderColor = color;
        slider2DlookAndFeel.CapLookAndFeel.HoveredBorderColor = color;
      }
    }

    public void SetSliderFillMode(GizmoFillMode2D fillMode)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.FillMode = fillMode;
    }

    public void SetDblSliderFillMode(GizmoFillMode2D fillMode)
    {
      this._dblSliderLookAndFeel.FillMode = fillMode;
    }

    public void SetSliderCapFillMode(GizmoFillMode2D fillMode)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.FillMode = fillMode;
    }

    public void SetSliderLineType(GizmoLine2DType lineType)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.LineType = lineType;
    }

    public void SetBoxSliderThickness(float thickness)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.BoxThickness = thickness;
    }

    public void SetSliderLength(float length)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.Length = length;
    }

    public void SetSliderCapType(GizmoCap2DType capType)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.CapType = capType;
    }

    public void SetSliderArrowCapBaseRadius(float radius)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.ArrowBaseRadius = radius;
    }

    public void SetSliderArrowCapHeight(float height)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.ArrowHeight = height;
    }

    public void SetSliderQuadCapWidth(float width)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.QuadWidth = width;
    }

    public void SetSliderQuadCapHeight(float height)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.QuadHeight = height;
    }

    public void SetSliderCircleCapRadius(float radius)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.CircleRadius = radius;
    }

    public void SetDblSliderPlaneType(GizmoPlane2DType sliderType)
    {
      this._dblSliderLookAndFeel.PlaneType = sliderType;
    }

    public void SetDblSliderQuadWidth(float width) => this._dblSliderLookAndFeel.QuadWidth = width;

    public void SetDblSliderQuadHeight(float height)
    {
      this._dblSliderLookAndFeel.QuadHeight = height;
    }

    public void SetDblSliderCircleRadius(float radius)
    {
      this._dblSliderLookAndFeel.CircleRadius = radius;
    }

    public void SetDblSliderColor(Color color) => this._dblSliderLookAndFeel.Color = color;

    public void SetDblSliderBorderColor(Color color)
    {
      this._dblSliderLookAndFeel.BorderColor = color;
    }

    public void SetDblSliderHoveredColor(Color color)
    {
      this._dblSliderLookAndFeel.HoveredColor = color;
    }

    public void SetDblSliderHoveredBorderColor(Color color)
    {
      this._dblSliderLookAndFeel.HoveredBorderColor = color;
    }

    public void SetScale(float scale)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._sglSliderLookAndFeel)
      {
        slider2DlookAndFeel.Scale = scale;
        slider2DlookAndFeel.CapLookAndFeel.Scale = scale;
      }
      this._dblSliderLookAndFeel.Scale = scale;
    }

    public void ConnectSliderLookAndFeel(
      GizmoLineSlider2D slider,
      int axisIndex,
      AxisSign axisSign)
    {
      slider.SharedLookAndFeel = this.GetSliderLookAndFeel(axisIndex, axisSign);
    }

    public void ConnectDblSliderLookAndFeel(GizmoPlaneSlider2D slider)
    {
      slider.SharedLookAndFeel = this._dblSliderLookAndFeel;
    }

    private GizmoLineSlider2DLookAndFeel GetSliderLookAndFeel(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._sglSliderLookAndFeel[axisIndex] : this._sglSliderLookAndFeel[2 + axisIndex];
    }
  }
}
