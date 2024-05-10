// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPlaneSlider2DLookAndFeel
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
  public class GizmoPlaneSlider2DLookAndFeel
  {
    [SerializeField]
    private GizmoFillMode2D _fillMode;
    [SerializeField]
    private GizmoPlane2DType _planeType;
    [SerializeField]
    private float _scale;
    [SerializeField]
    private float _quadWidth;
    [SerializeField]
    private float _quadHeight;
    [SerializeField]
    private float _circleRadius;
    [SerializeField]
    private bool _isRotationArcVisible;
    [SerializeField]
    private GizmoRotationArc2DLookAndFeel _rotationArcLookAndFeel;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _hoveredColor;
    [SerializeField]
    private Color _borderColor;
    [SerializeField]
    private Color _hoveredBorderColor;
    [SerializeField]
    private GizmoQuad2DBorderType _quadBorderType;
    [SerializeField]
    private GizmoCircle2DBorderType _circleBorderType;
    [SerializeField]
    private GizmoPolygon2DBorderType _polygonBorderType;
    [SerializeField]
    private float _borderPolyThickness;

    public GizmoFillMode2D FillMode
    {
      get => this._fillMode;
      set => this._fillMode = value;
    }

    public GizmoPlane2DType PlaneType
    {
      get => this._planeType;
      set => this._planeType = value;
    }

    public float Scale
    {
      get => this._scale;
      set => this._scale = Mathf.Max(0.0f, value);
    }

    public float QuadWidth
    {
      get => this._quadWidth;
      set => this._quadWidth = Mathf.Max(0.0f, value);
    }

    public float QuadHeight
    {
      get => this._quadHeight;
      set => this._quadHeight = Mathf.Max(0.0f, value);
    }

    public float CircleRadius
    {
      get => this._circleRadius;
      set => this._circleRadius = Mathf.Max(0.0f, value);
    }

    public bool IsRotationArcVisible
    {
      get => this._isRotationArcVisible;
      set => this._isRotationArcVisible = value;
    }

    public GizmoRotationArc2DLookAndFeel RotationArcLookAndFeel => this._rotationArcLookAndFeel;

    public Color Color
    {
      get => this._color;
      set => this._color = value;
    }

    public Color HoveredColor
    {
      get => this._hoveredColor;
      set => this._hoveredColor = value;
    }

    public Color BorderColor
    {
      get => this._borderColor;
      set => this._borderColor = value;
    }

    public Color HoveredBorderColor
    {
      get => this._hoveredBorderColor;
      set => this._hoveredBorderColor = value;
    }

    public GizmoQuad2DBorderType QuadBorderType
    {
      get => this._quadBorderType;
      set => this._quadBorderType = value;
    }

    public GizmoCircle2DBorderType CircleBorderType
    {
      get => this._circleBorderType;
      set => this._circleBorderType = value;
    }

    public GizmoPolygon2DBorderType PolygonBorderType
    {
      get => this._polygonBorderType;
      set => this._polygonBorderType = value;
    }

    public float BorderPolyThickness
    {
      get => this._borderPolyThickness;
      set => this._borderPolyThickness = Mathf.Max(0.0f, value);
    }

    public GizmoPlaneSlider2DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._fillMode = GizmoFillMode2D.FilledAndBorder;
      this._scale = 1f;
      this._quadWidth = 25f;
      this._quadHeight = 25f;
      this._circleRadius = 12f;
      this._isRotationArcVisible = true;
      this._rotationArcLookAndFeel = new GizmoRotationArc2DLookAndFeel();
      this._color = Color.white;
      this._hoveredColor = RTSystemValues.HoveredAxisColor;
      this._borderColor = Color.white;
      this._hoveredBorderColor = RTSystemValues.HoveredAxisColor;
      this._borderPolyThickness = 8f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
