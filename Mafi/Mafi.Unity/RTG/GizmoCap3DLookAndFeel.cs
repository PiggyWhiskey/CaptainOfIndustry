// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap3DLookAndFeel
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
  public class GizmoCap3DLookAndFeel
  {
    [SerializeField]
    private GizmoCap3DType _capType;
    [SerializeField]
    private GizmoFillMode3D _fillMode;
    [SerializeField]
    private GizmoShadeMode _shadeMode;
    [SerializeField]
    private float _scale;
    [SerializeField]
    private bool _useZoomFactor;
    [SerializeField]
    private float _coneHeight;
    [SerializeField]
    private float _coneRadius;
    [SerializeField]
    private float _pyramidHeight;
    [SerializeField]
    private float _pyramidWidth;
    [SerializeField]
    private float _pyramidDepth;
    [SerializeField]
    private float _boxWidth;
    [SerializeField]
    private float _boxHeight;
    [SerializeField]
    private float _boxDepth;
    [SerializeField]
    private float _sphereRadius;
    [SerializeField]
    private float _trPrismWidth;
    [SerializeField]
    private float _trPrismHeight;
    [SerializeField]
    private float _trPrismDepth;
    [SerializeField]
    private bool _isSphereBorderVisible;
    [SerializeField]
    private Color _sphereBorderColor;
    [SerializeField]
    private int _numSphereBorderPoints;
    [SerializeField]
    private Color _color;
    [SerializeField]
    private Color _hoveredColor;

    public GizmoCap3DType CapType
    {
      get => this._capType;
      set => this._capType = value;
    }

    public GizmoFillMode3D FillMode
    {
      get => this._fillMode;
      set => this._fillMode = value;
    }

    public GizmoShadeMode ShadeMode
    {
      get => this._shadeMode;
      set => this._shadeMode = value;
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

    public float ConeHeight
    {
      get => this._coneHeight;
      set => this._coneHeight = Mathf.Max(1E-05f, value);
    }

    public float ConeRadius
    {
      get => this._coneRadius;
      set => this._coneRadius = Mathf.Max(1E-05f, value);
    }

    public float PyramidHeight
    {
      get => this._pyramidHeight;
      set => this._pyramidHeight = Mathf.Max(1E-05f, value);
    }

    public float PyramidWidth
    {
      get => this._pyramidWidth;
      set => this._pyramidWidth = Mathf.Max(1E-05f, value);
    }

    public float PyramidDepth
    {
      get => this._pyramidDepth;
      set => this._pyramidDepth = Mathf.Max(1E-05f, value);
    }

    public float BoxWidth
    {
      get => this._boxWidth;
      set => this._boxWidth = Mathf.Max(1E-05f, value);
    }

    public float BoxHeight
    {
      get => this._boxHeight;
      set => this._boxHeight = Mathf.Max(1E-05f, value);
    }

    public float BoxDepth
    {
      get => this._boxDepth;
      set => this._boxDepth = Mathf.Max(1E-05f, value);
    }

    public float SphereRadius
    {
      get => this._sphereRadius;
      set => this._sphereRadius = Mathf.Max(1E-05f, value);
    }

    public float TrPrismWidth
    {
      get => this._trPrismWidth;
      set => this._trPrismWidth = Mathf.Max(1E-05f, value);
    }

    public float TrPrismHeight
    {
      get => this._trPrismHeight;
      set => this._trPrismHeight = Mathf.Max(1E-05f, value);
    }

    public float TrPrismDepth
    {
      get => this._trPrismDepth;
      set => this._trPrismDepth = Mathf.Max(1E-05f, value);
    }

    public bool IsSphereBorderVisible
    {
      get => this._isSphereBorderVisible;
      set => this._isSphereBorderVisible = value;
    }

    public Color SphereBorderColor
    {
      get => this._sphereBorderColor;
      set => this._sphereBorderColor = value;
    }

    public int NumSphereBorderPoints
    {
      get => this._numSphereBorderPoints;
      set => this._numSphereBorderPoints = Mathf.Max(3, value);
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

    public static float DefaultConeHeight => 1.65f;

    public static float DefaultConeRadius => 0.5f;

    public static float DefaultPyramidHeight => 1.65f;

    public static float DefaultPyramidWidth => 0.8f;

    public static float DefaultPyramidDepth => 0.8f;

    public GizmoCap3DLookAndFeel()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._scale = 1f;
      this._useZoomFactor = true;
      this._coneHeight = 1.65f;
      this._coneRadius = 0.5f;
      this._pyramidHeight = 1.65f;
      this._pyramidWidth = 0.8f;
      this._pyramidDepth = 0.8f;
      this._boxWidth = 0.7f;
      this._boxHeight = 0.7f;
      this._boxDepth = 0.7f;
      this._sphereRadius = 0.45f;
      this._trPrismWidth = 1f;
      this._trPrismHeight = 1f;
      this._trPrismDepth = 1f;
      this._sphereBorderColor = Color.white;
      this._numSphereBorderPoints = 100;
      this._color = RTSystemValues.XAxisColor;
      this._hoveredColor = RTSystemValues.HoveredAxisColor;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
