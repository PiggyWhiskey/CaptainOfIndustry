// Decompiled with JetBrains decompiler
// Type: RTG.GizmoLineSlider3DLookAndFeel
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
  public class GizmoLineSlider3DLookAndFeel
  {
    [SerializeField]
    private GizmoShadeMode _shadeMode;
    [SerializeField]
    private GizmoLine3DType _lineType;
    [SerializeField]
    private GizmoFillMode3D _fillMode;
    [SerializeField]
    private float _length;
    [SerializeField]
    private float _scale;
    [SerializeField]
    private bool _useZoomFactor;
    [SerializeField]
    private float _boxHeight;
    [SerializeField]
    private float _boxDepth;
    [SerializeField]
    private float _cylinderRadius;
    [SerializeField]
    private bool _isRotationArcVisible;
    [SerializeField]
    private GizmoRotationArc3DLookAndFeel _rotationArcLookAndFeel;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _hoveredColor;
    [SerializeField]
    private GizmoCap3DLookAndFeel _capLookAndFeel;

    public GizmoShadeMode ShadeMode
    {
      get => this._shadeMode;
      set => this._shadeMode = value;
    }

    public GizmoLine3DType LineType
    {
      get => this._lineType;
      set => this._lineType = value;
    }

    public GizmoFillMode3D FillMode
    {
      get => this._fillMode;
      set => this._fillMode = value;
    }

    public float Length
    {
      get => this._length;
      set => this._length = Mathf.Max(0.0f, value);
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

    public bool IsRotationArcVisible
    {
      get => this._isRotationArcVisible;
      set => this._isRotationArcVisible = value;
    }

    public GizmoRotationArc3DLookAndFeel RotationArcLookAndFeel => this._rotationArcLookAndFeel;

    public GizmoCap3DLookAndFeel CapLookAndFeel => this._capLookAndFeel;

    public float BoxHeight
    {
      get => this._boxHeight;
      set => this._boxHeight = Mathf.Max(0.0f, value);
    }

    public float BoxDepth
    {
      get => this._boxDepth;
      set => this._boxDepth = Mathf.Max(0.0f, value);
    }

    public float CylinderRadius
    {
      get => this._cylinderRadius;
      set => this._cylinderRadius = Mathf.Max(0.0f, value);
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

    public GizmoLineSlider3DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._length = 5f;
      this._scale = 1f;
      this._useZoomFactor = true;
      this._boxHeight = 0.18f;
      this._boxDepth = 0.18f;
      this._cylinderRadius = 0.15f;
      this._isRotationArcVisible = true;
      this._rotationArcLookAndFeel = new GizmoRotationArc3DLookAndFeel();
      this._color = RTSystemValues.XAxisColor;
      this._hoveredColor = RTSystemValues.HoveredAxisColor;
      this._capLookAndFeel = new GizmoCap3DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
