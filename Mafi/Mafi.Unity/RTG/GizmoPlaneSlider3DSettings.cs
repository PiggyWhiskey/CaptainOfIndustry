// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPlaneSlider3DSettings
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
  public class GizmoPlaneSlider3DSettings
  {
    [SerializeField]
    private float _areaHoverEps;
    [SerializeField]
    private float _extrudeHoverEps;
    [SerializeField]
    private float _borderLineHoverEps;
    [SerializeField]
    private float _borderBoxHoverEps;
    [SerializeField]
    private float _borderTorusHoverEps;
    [SerializeField]
    private bool _isCircleHoverCullEnabled;
    [SerializeField]
    private float _offsetSnapStepRight;
    [SerializeField]
    private float _offsetSnapStepUp;
    [SerializeField]
    private float _rotationSnapStep;
    [SerializeField]
    private GizmoSnapMode _rotationSnapMode;
    [SerializeField]
    private GizmoDblAxisScaleMode _scaleMode;
    [SerializeField]
    private float _scaleSnapStepRight;
    [SerializeField]
    private float _scaleSnapStepUp;
    [SerializeField]
    private float _proportionalScaleSnapStep;
    [SerializeField]
    private float _offsetSensitivity;
    [SerializeField]
    private float _rotationSensitivity;
    [SerializeField]
    private float _scaleSensitivity;

    public float AreaHoverEps
    {
      get => this._areaHoverEps;
      set => this._areaHoverEps = Mathf.Max(0.0f, value);
    }

    public float ExtrudeHoverEps
    {
      get => this._extrudeHoverEps;
      set => this._extrudeHoverEps = Mathf.Max(0.0f, value);
    }

    public float BorderLineHoverEps
    {
      get => this._borderLineHoverEps;
      set => this._borderLineHoverEps = Mathf.Max(0.0f, value);
    }

    public float BorderBoxHoverEps
    {
      get => this._borderBoxHoverEps;
      set => this._borderBoxHoverEps = Mathf.Max(0.0f, value);
    }

    public float BorderTorusHoverEps
    {
      get => this._borderTorusHoverEps;
      set => this._borderTorusHoverEps = Mathf.Max(0.0f, value);
    }

    public bool IsCircleHoverCullEnabled
    {
      get => this._isCircleHoverCullEnabled;
      set => this._isCircleHoverCullEnabled = value;
    }

    public float OffsetSnapStepRight
    {
      get => this._offsetSnapStepRight;
      set => this._offsetSnapStepRight = Mathf.Max(0.0001f, value);
    }

    public float OffsetSnapStepUp
    {
      get => this._offsetSnapStepUp;
      set => this._offsetSnapStepUp = Mathf.Max(0.0001f, value);
    }

    public float RotationSnapStep
    {
      get => this._rotationSnapStep;
      set => this._rotationSnapStep = Mathf.Max(0.0001f, value);
    }

    public GizmoSnapMode RotationSnapMode
    {
      get => this._rotationSnapMode;
      set => this._rotationSnapMode = value;
    }

    public GizmoDblAxisScaleMode ScaleMode
    {
      get => this._scaleMode;
      set => this._scaleMode = value;
    }

    public float ScaleSnapStepRight
    {
      get => this._scaleSnapStepRight;
      set => this._scaleSnapStepRight = Mathf.Max(0.0001f, value);
    }

    public float ScaleSnapStepUp
    {
      get => this._scaleSnapStepUp;
      set => this._scaleSnapStepUp = Mathf.Max(0.0001f, value);
    }

    public float ProportionalScaleSnapStep
    {
      get => this._proportionalScaleSnapStep;
      set => this._proportionalScaleSnapStep = Mathf.Max(0.0001f, value);
    }

    public float OffsetSensitivity
    {
      get => this._offsetSensitivity;
      set => this._offsetSensitivity = Mathf.Max(0.0001f, value);
    }

    public float RotationSensitivity
    {
      get => this._rotationSensitivity;
      set => this._rotationSensitivity = Mathf.Max(0.0001f, value);
    }

    public float ScaleSensitivity
    {
      get => this._scaleSensitivity;
      set => this._scaleSensitivity = Mathf.Max(0.0001f, value);
    }

    public GizmoPlaneSlider3DSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._areaHoverEps = 1E-05f;
      this._extrudeHoverEps = 1E-05f;
      this._borderLineHoverEps = 0.7f;
      this._borderBoxHoverEps = 0.7f;
      this._borderTorusHoverEps = 0.7f;
      this._offsetSnapStepRight = 1f;
      this._offsetSnapStepUp = 1f;
      this._rotationSnapStep = 15f;
      this._scaleMode = GizmoDblAxisScaleMode.Proportional;
      this._scaleSnapStepRight = 0.1f;
      this._scaleSnapStepUp = 0.1f;
      this._proportionalScaleSnapStep = 0.1f;
      this._offsetSensitivity = 1f;
      this._rotationSensitivity = 0.45f;
      this._scaleSensitivity = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
