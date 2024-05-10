// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPlaneSlider3DLookAndFeel
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
  public class GizmoPlaneSlider3DLookAndFeel
  {
    [SerializeField]
    private GizmoPlane3DType _planeType;
    [SerializeField]
    private float _scale;
    [SerializeField]
    private bool _useZoomFactor;
    [SerializeField]
    private float _quadWidth;
    [SerializeField]
    private float _quadHeight;
    [SerializeField]
    private float _raTriangleXLength;
    [SerializeField]
    private float _raTriangleYLength;
    [SerializeField]
    private float _circleRadius;
    [SerializeField]
    private float _borderBoxHeight;
    [SerializeField]
    private float _borderBoxDepth;
    [SerializeField]
    private float _borderTorusThickness;
    [SerializeField]
    private int _numBorderTorusWireAxialSlices;
    [SerializeField]
    private float _borderCylTorusWidth;
    [SerializeField]
    private float _borderCylTorusHeight;
    [SerializeField]
    private GizmoShadeMode _shadeMode;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _hoveredColor;
    [SerializeField]
    private Color _borderColor;
    [SerializeField]
    private Color _hoveredBorderColor;
    [SerializeField]
    private float _borderCircleCullAlphaScale;
    [SerializeField]
    private GizmoShadeMode _borderShadeMode;
    [SerializeField]
    private GizmoFillMode3D _borderFillMode;
    [SerializeField]
    private GizmoQuad3DBorderType _quadBorderType;
    [SerializeField]
    private GizmoRATriangle3DBorderType _raTriangleBorderType;
    [SerializeField]
    private GizmoCircle3DBorderType _circleBorderType;
    [SerializeField]
    private bool _isRotationArcVisible;
    [SerializeField]
    private GizmoRotationArc3DLookAndFeel _rotationArcLookAndFeel;

    public GizmoShadeMode ShadeMode
    {
      get => this._shadeMode;
      set => this._shadeMode = value;
    }

    public GizmoPlane3DType PlaneType
    {
      get => this._planeType;
      set => this._planeType = value;
    }

    public float Scale
    {
      get => this._scale;
      set => this._scale = Mathf.Max(0.0f, value);
    }

    public bool UseZoomFactor
    {
      get => this._useZoomFactor;
      set => this._useZoomFactor = value;
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

    public float RATriangleXLength
    {
      get => this._raTriangleXLength;
      set => this._raTriangleXLength = Mathf.Max(0.0f, value);
    }

    public float RATriangleYLength
    {
      get => this._raTriangleYLength;
      set => this._raTriangleYLength = Mathf.Max(0.0f, value);
    }

    public float CircleRadius
    {
      get => this._circleRadius;
      set => this._circleRadius = Mathf.Max(0.0f, value);
    }

    public float BorderCircleCullAlphaScale
    {
      get => this._borderCircleCullAlphaScale;
      set => this._borderCircleCullAlphaScale = Mathf.Clamp(value, 0.0f, 1f);
    }

    public float BorderBoxHeight
    {
      get => this._borderBoxHeight;
      set => this._borderBoxHeight = Mathf.Max(0.0f, value);
    }

    public float BorderBoxDepth
    {
      get => this._borderBoxDepth;
      set => this._borderBoxDepth = Mathf.Max(0.0f, value);
    }

    public float BorderTorusThickness
    {
      get => this._borderTorusThickness;
      set => this._borderTorusThickness = Mathf.Max(0.0f, value);
    }

    public float BorderCylTorusWidth
    {
      get => this._borderCylTorusWidth;
      set => this._borderCylTorusWidth = Mathf.Max(0.0f, value);
    }

    public float BorderCylTorusHeight
    {
      get => this._borderCylTorusHeight;
      set => this._borderCylTorusHeight = Mathf.Max(0.0f, value);
    }

    public int NumBorderTorusWireAxialSlices
    {
      get => this._numBorderTorusWireAxialSlices;
      set => this._numBorderTorusWireAxialSlices = Mathf.Max(2, value);
    }

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

    public GizmoShadeMode BorderShadeMode
    {
      get => this._borderShadeMode;
      set => this._borderShadeMode = value;
    }

    public GizmoFillMode3D BorderFillMode
    {
      get => this._borderFillMode;
      set => this._borderFillMode = value;
    }

    public GizmoQuad3DBorderType QuadBorderType
    {
      get => this._quadBorderType;
      set => this._quadBorderType = value;
    }

    public GizmoCircle3DBorderType CircleBorderType
    {
      get => this._circleBorderType;
      set => this._circleBorderType = value;
    }

    public GizmoRATriangle3DBorderType RATriangleBorderType
    {
      get => this._raTriangleBorderType;
      set => this._raTriangleBorderType = value;
    }

    public bool IsRotationArcVisible
    {
      get => this._isRotationArcVisible;
      set => this._isRotationArcVisible = value;
    }

    public GizmoRotationArc3DLookAndFeel RotationArcLookAndFeel => this._rotationArcLookAndFeel;

    public GizmoPlaneSlider3DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._scale = 1f;
      this._useZoomFactor = true;
      this._quadWidth = 1f;
      this._quadHeight = 1f;
      this._raTriangleXLength = 1f;
      this._raTriangleYLength = 1f;
      this._circleRadius = 0.5f;
      this._borderBoxHeight = 0.18f;
      this._borderBoxDepth = 0.18f;
      this._borderTorusThickness = 0.18f;
      this._numBorderTorusWireAxialSlices = 5;
      this._borderCylTorusWidth = 0.18f;
      this._borderCylTorusHeight = 0.18f;
      this._shadeMode = GizmoShadeMode.Flat;
      this._color = Color.white;
      this._hoveredColor = RTSystemValues.HoveredAxisColor;
      this._borderColor = Color.white;
      this._hoveredBorderColor = RTSystemValues.HoveredAxisColor;
      this._isRotationArcVisible = true;
      this._rotationArcLookAndFeel = new GizmoRotationArc3DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
