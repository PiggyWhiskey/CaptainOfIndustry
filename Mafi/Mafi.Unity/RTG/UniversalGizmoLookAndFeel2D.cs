// Decompiled with JetBrains decompiler
// Type: RTG.UniversalGizmoLookAndFeel2D
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
  public class UniversalGizmoLookAndFeel2D : Settings
  {
    [SerializeField]
    private UniversalGizmoSettingsCategory _displayCategory;
    [SerializeField]
    private GizmoPlaneSlider2DLookAndFeel _mvDblSliderLookAndFeel;
    [SerializeField]
    private GizmoLineSlider2DLookAndFeel[] _mvSglSliderLookAndFeel;
    [SerializeField]
    private bool _isMvDblSliderVisible;
    [SerializeField]
    private bool[] _mvSglSliderVis;
    [SerializeField]
    private bool[] _mvSglSliderCapVis;

    public float MvScale => this._mvSglSliderLookAndFeel[0].Scale;

    public float MvSliderLength => this._mvSglSliderLookAndFeel[0].Length;

    public float MvBoxSliderThickness => this._mvSglSliderLookAndFeel[0].BoxThickness;

    public float MvSliderArrowCapHeight
    {
      get => this._mvSglSliderLookAndFeel[0].CapLookAndFeel.ArrowHeight;
    }

    public float MvSliderArrowCapBaseRadius
    {
      get => this._mvSglSliderLookAndFeel[0].CapLookAndFeel.ArrowBaseRadius;
    }

    public float MvSliderQuadCapWidth => this._mvSglSliderLookAndFeel[0].CapLookAndFeel.QuadWidth;

    public float MvSliderQuadCapHeight => this._mvSglSliderLookAndFeel[0].CapLookAndFeel.QuadHeight;

    public float MvSliderCircleCapRadius
    {
      get => this._mvSglSliderLookAndFeel[0].CapLookAndFeel.CircleRadius;
    }

    public float MvDblSliderQuadWidth => this._mvDblSliderLookAndFeel.QuadWidth;

    public float MvDblSliderQuadHeight => this._mvDblSliderLookAndFeel.QuadHeight;

    public float MvDblSliderCircleRadius => this._mvDblSliderLookAndFeel.CircleRadius;

    public Color MvXColor => this.GetMvSliderLookAndFeel(0, AxisSign.Positive).Color;

    public Color MvYColor => this.GetMvSliderLookAndFeel(1, AxisSign.Positive).Color;

    public Color MvXBorderColor => this.GetMvSliderLookAndFeel(0, AxisSign.Positive).BorderColor;

    public Color MvYBorderColor => this.GetMvSliderLookAndFeel(1, AxisSign.Positive).BorderColor;

    public Color MvDblSliderColor => this._mvDblSliderLookAndFeel.Color;

    public Color MvDblSliderBorderColor => this._mvDblSliderLookAndFeel.BorderColor;

    public Color MvDblSliderHoveredColor => this._mvDblSliderLookAndFeel.HoveredColor;

    public Color MvDblSliderHoveredBorderColor => this._mvDblSliderLookAndFeel.HoveredBorderColor;

    public bool IsMvDblSliderVisible => this._isMvDblSliderVisible;

    public Color MvSliderHoveredColor => this._mvSglSliderLookAndFeel[0].HoveredColor;

    public Color MvSliderHoveredBorderColor => this._mvSglSliderLookAndFeel[0].HoveredBorderColor;

    public GizmoFillMode2D MvSliderFillMode => this._mvSglSliderLookAndFeel[0].FillMode;

    public GizmoFillMode2D MvSliderCapFillMode
    {
      get => this._mvSglSliderLookAndFeel[0].CapLookAndFeel.FillMode;
    }

    public GizmoFillMode2D MvDblSliderFillMode => this._mvDblSliderLookAndFeel.FillMode;

    public GizmoCap2DType MvSliderCapType => this._mvSglSliderLookAndFeel[0].CapLookAndFeel.CapType;

    public GizmoLine2DType MvSliderLineType => this._mvSglSliderLookAndFeel[0].LineType;

    public GizmoPlane2DType MvDblSliderPlaneType => this._mvDblSliderLookAndFeel.PlaneType;

    public UniversalGizmoSettingsCategory DisplayCategory
    {
      get => this._displayCategory;
      set => this._displayCategory = value;
    }

    public UniversalGizmoLookAndFeel2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._mvDblSliderLookAndFeel = new GizmoPlaneSlider2DLookAndFeel();
      this._mvSglSliderLookAndFeel = new GizmoLineSlider2DLookAndFeel[4];
      this._isMvDblSliderVisible = true;
      this._mvSglSliderVis = new bool[4];
      this._mvSglSliderCapVis = new bool[4];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      for (int index = 0; index < this._mvSglSliderLookAndFeel.Length; ++index)
        this._mvSglSliderLookAndFeel[index] = new GizmoLineSlider2DLookAndFeel();
      this.SetMvAxisColor(0, RTSystemValues.XAxisColor);
      this.SetMvAxisColor(1, RTSystemValues.YAxisColor);
      this.SetMvAxisBorderColor(0, RTSystemValues.XAxisColor);
      this.SetMvAxisBorderColor(1, RTSystemValues.YAxisColor);
      this.SetMvSliderHoveredFillColor(RTSystemValues.HoveredAxisColor);
      this.SetMvSliderHoveredBorderColor(RTSystemValues.HoveredAxisColor);
      this.SetMvSliderCapType(GizmoCap2DType.Arrow);
      this.SetMvSliderCapFillMode(GizmoFillMode2D.Filled);
      this.SetMvSliderFillMode(GizmoFillMode2D.Filled);
      this.SetMvSliderVisible(0, AxisSign.Positive, true);
      this.SetMvSliderVisible(1, AxisSign.Positive, true);
      this.SetMvSliderCapVisible(0, AxisSign.Positive, true);
      this.SetMvSliderCapVisible(1, AxisSign.Positive, true);
      this.SetMvDblSliderFillMode(GizmoFillMode2D.Border);
      this.SetMvDblSliderColor(Color.white.KeepAllButAlpha(RTSystemValues.AxisAlpha));
      this.SetMvDblSliderBorderColor(Color.white);
      this.SetMvDblSliderHoveredColor(RTSystemValues.HoveredAxisColor.KeepAllButAlpha(RTSystemValues.AxisAlpha));
      this.SetMvDblSliderHoveredBorderColor(RTSystemValues.HoveredAxisColor);
    }

    public void SetMvDblSliderVisible(bool isVisible) => this._isMvDblSliderVisible = isVisible;

    public bool IsMvSliderVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._mvSglSliderVis[axisIndex] : this._mvSglSliderVis[2 + axisIndex];
    }

    public bool IsMvPositiveSliderVisible(int axisIndex) => this._mvSglSliderVis[axisIndex];

    public bool IsMvNegativeSliderVisible(int axisIndex) => this._mvSglSliderVis[2 + axisIndex];

    public void SetMvSliderVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._mvSglSliderVis[axisIndex] = isVisible;
      else
        this._mvSglSliderVis[2 + axisIndex] = isVisible;
    }

    public bool IsMvSliderCapVisible(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._mvSglSliderCapVis[axisIndex] : this._mvSglSliderCapVis[2 + axisIndex];
    }

    public bool IsMvPositiveSliderCapVisible(int axisIndex) => this._mvSglSliderCapVis[axisIndex];

    public bool IsMvNegativeSliderCapVisible(int axisIndex)
    {
      return this._mvSglSliderCapVis[2 + axisIndex];
    }

    public void SetMvSliderCapVisible(int axisIndex, AxisSign axisSign, bool isVisible)
    {
      if (axisSign == AxisSign.Positive)
        this._mvSglSliderCapVis[axisIndex] = isVisible;
      else
        this._mvSglSliderCapVis[2 + axisIndex] = isVisible;
    }

    public void SetMvAxisColor(int axisIndex, Color color)
    {
      this.GetMvSliderLookAndFeel(axisIndex, AxisSign.Positive).Color = color;
      this.GetMvSliderLookAndFeel(axisIndex, AxisSign.Positive).CapLookAndFeel.Color = color;
      this.GetMvSliderLookAndFeel(axisIndex, AxisSign.Negative).Color = color;
      this.GetMvSliderLookAndFeel(axisIndex, AxisSign.Negative).CapLookAndFeel.Color = color;
    }

    public void SetMvAxisBorderColor(int axisIndex, Color color)
    {
      this.GetMvSliderLookAndFeel(axisIndex, AxisSign.Positive).BorderColor = color;
      this.GetMvSliderLookAndFeel(axisIndex, AxisSign.Positive).CapLookAndFeel.BorderColor = color;
      this.GetMvSliderLookAndFeel(axisIndex, AxisSign.Negative).BorderColor = color;
      this.GetMvSliderLookAndFeel(axisIndex, AxisSign.Negative).CapLookAndFeel.BorderColor = color;
    }

    public void SetMvSliderHoveredFillColor(Color color)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
      {
        slider2DlookAndFeel.HoveredColor = color;
        slider2DlookAndFeel.CapLookAndFeel.HoveredColor = color;
      }
    }

    public void SetMvSliderHoveredBorderColor(Color color)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
      {
        slider2DlookAndFeel.HoveredBorderColor = color;
        slider2DlookAndFeel.CapLookAndFeel.HoveredBorderColor = color;
      }
    }

    public void SetMvSliderFillMode(GizmoFillMode2D fillMode)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.FillMode = fillMode;
    }

    public void SetMvDblSliderFillMode(GizmoFillMode2D fillMode)
    {
      this._mvDblSliderLookAndFeel.FillMode = fillMode;
    }

    public void SetMvSliderCapFillMode(GizmoFillMode2D fillMode)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.FillMode = fillMode;
    }

    public void SetMvSliderLineType(GizmoLine2DType lineType)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.LineType = lineType;
    }

    public void SetMvBoxSliderThickness(float thickness)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.BoxThickness = thickness;
    }

    public void SetMvSliderLength(float length)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.Length = length;
    }

    public void SetMvSliderCapType(GizmoCap2DType capType)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.CapType = capType;
    }

    public void SetMvSliderArrowCapBaseRadius(float radius)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.ArrowBaseRadius = radius;
    }

    public void SetMvSliderArrowCapHeight(float height)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.ArrowHeight = height;
    }

    public void SetMvSliderQuadCapWidth(float width)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.QuadWidth = width;
    }

    public void SetMvSliderQuadCapHeight(float height)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.QuadHeight = height;
    }

    public void SetMvSliderCircleCapRadius(float radius)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
        slider2DlookAndFeel.CapLookAndFeel.CircleRadius = radius;
    }

    public void SetMvDblSliderPlaneType(GizmoPlane2DType sliderType)
    {
      this._mvDblSliderLookAndFeel.PlaneType = sliderType;
    }

    public void SetMvDblSliderQuadWidth(float width)
    {
      this._mvDblSliderLookAndFeel.QuadWidth = width;
    }

    public void SetMvDblSliderQuadHeight(float height)
    {
      this._mvDblSliderLookAndFeel.QuadHeight = height;
    }

    public void SetMvDblSliderCircleRadius(float radius)
    {
      this._mvDblSliderLookAndFeel.CircleRadius = radius;
    }

    public void SetMvDblSliderColor(Color color) => this._mvDblSliderLookAndFeel.Color = color;

    public void SetMvDblSliderBorderColor(Color color)
    {
      this._mvDblSliderLookAndFeel.BorderColor = color;
    }

    public void SetMvDblSliderHoveredColor(Color color)
    {
      this._mvDblSliderLookAndFeel.HoveredColor = color;
    }

    public void SetMvDblSliderHoveredBorderColor(Color color)
    {
      this._mvDblSliderLookAndFeel.HoveredBorderColor = color;
    }

    public void SetMvScale(float scale)
    {
      foreach (GizmoLineSlider2DLookAndFeel slider2DlookAndFeel in this._mvSglSliderLookAndFeel)
      {
        slider2DlookAndFeel.Scale = scale;
        slider2DlookAndFeel.CapLookAndFeel.Scale = scale;
      }
      this._mvDblSliderLookAndFeel.Scale = scale;
    }

    public void ConnectMvSliderLookAndFeel(
      GizmoLineSlider2D slider,
      int axisIndex,
      AxisSign axisSign)
    {
      slider.SharedLookAndFeel = this.GetMvSliderLookAndFeel(axisIndex, axisSign);
    }

    public void ConnectMvDblSliderLookAndFeel(GizmoPlaneSlider2D slider)
    {
      slider.SharedLookAndFeel = this._mvDblSliderLookAndFeel;
    }

    public void Inherit(MoveGizmoLookAndFeel2D lookAndFeel)
    {
      this.SetMvAxisBorderColor(0, lookAndFeel.XBorderColor);
      this.SetMvAxisBorderColor(1, lookAndFeel.YBorderColor);
      this.SetMvAxisColor(0, lookAndFeel.XColor);
      this.SetMvAxisColor(1, lookAndFeel.YColor);
      this.SetMvBoxSliderThickness(lookAndFeel.BoxSliderThickness);
      this.SetMvDblSliderBorderColor(lookAndFeel.DblSliderBorderColor);
      this.SetMvDblSliderCircleRadius(lookAndFeel.DblSliderCircleRadius);
      this.SetMvDblSliderColor(lookAndFeel.DblSliderColor);
      this.SetMvDblSliderFillMode(lookAndFeel.DblSliderFillMode);
      this.SetMvDblSliderHoveredBorderColor(lookAndFeel.DblSliderHoveredBorderColor);
      this.SetMvDblSliderHoveredColor(lookAndFeel.DblSliderHoveredColor);
      this.SetMvDblSliderQuadHeight(lookAndFeel.DblSliderQuadHeight);
      this.SetMvDblSliderQuadWidth(lookAndFeel.DblSliderQuadWidth);
      this.SetMvDblSliderPlaneType(lookAndFeel.DblSliderPlaneType);
      this.SetMvDblSliderVisible(lookAndFeel.IsDblSliderVisible);
      this.SetMvScale(lookAndFeel.Scale);
      this.SetMvSliderArrowCapHeight(lookAndFeel.SliderArrowCapHeight);
      this.SetMvSliderArrowCapBaseRadius(lookAndFeel.SliderArrowCapBaseRadius);
      this.SetMvSliderCircleCapRadius(lookAndFeel.SliderCircleCapRadius);
      this.SetMvSliderCapFillMode(lookAndFeel.SliderCapFillMode);
      this.SetMvSliderCapType(lookAndFeel.SliderCapType);
      this.SetMvSliderCapVisible(0, AxisSign.Positive, lookAndFeel.IsSliderCapVisible(0, AxisSign.Positive));
      this.SetMvSliderCapVisible(1, AxisSign.Positive, lookAndFeel.IsSliderCapVisible(1, AxisSign.Positive));
      this.SetMvSliderCapVisible(0, AxisSign.Negative, lookAndFeel.IsSliderCapVisible(0, AxisSign.Negative));
      this.SetMvSliderCapVisible(1, AxisSign.Negative, lookAndFeel.IsSliderCapVisible(1, AxisSign.Negative));
      this.SetMvSliderFillMode(lookAndFeel.SliderFillMode);
      this.SetMvSliderHoveredBorderColor(lookAndFeel.SliderHoveredBorderColor);
      this.SetMvSliderHoveredFillColor(lookAndFeel.SliderHoveredColor);
      this.SetMvSliderLength(lookAndFeel.SliderLength);
      this.SetMvSliderLineType(lookAndFeel.SliderLineType);
      this.SetMvSliderQuadCapHeight(lookAndFeel.SliderQuadCapHeight);
      this.SetMvSliderQuadCapWidth(lookAndFeel.SliderQuadCapWidth);
      this.SetMvSliderVisible(0, AxisSign.Positive, lookAndFeel.IsSliderVisible(0, AxisSign.Positive));
      this.SetMvSliderVisible(1, AxisSign.Positive, lookAndFeel.IsSliderVisible(1, AxisSign.Positive));
      this.SetMvSliderVisible(0, AxisSign.Negative, lookAndFeel.IsSliderVisible(0, AxisSign.Negative));
      this.SetMvSliderVisible(1, AxisSign.Negative, lookAndFeel.IsSliderVisible(1, AxisSign.Negative));
    }

    private GizmoLineSlider2DLookAndFeel GetMvSliderLookAndFeel(int axisIndex, AxisSign axisSign)
    {
      return axisSign == AxisSign.Positive ? this._mvSglSliderLookAndFeel[axisIndex] : this._mvSglSliderLookAndFeel[2 + axisIndex];
    }
  }
}
