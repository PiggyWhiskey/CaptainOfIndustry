// Decompiled with JetBrains decompiler
// Type: RTG.GizmoLineSlider3DSettings
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
  public class GizmoLineSlider3DSettings
  {
    [SerializeField]
    private float _lineHoverEps;
    [SerializeField]
    private float _boxHoverEps;
    [SerializeField]
    private float _cylinderHoverEps;
    [SerializeField]
    private float _offsetSnapStep;
    [SerializeField]
    private float _rotationSnapStep;
    [SerializeField]
    private GizmoSnapMode _rotationSnapMode;
    [SerializeField]
    private float _scaleSnapStep;
    [SerializeField]
    private float _offsetSensitivity;
    [SerializeField]
    private float _rotationSensitivity;
    [SerializeField]
    private float _scaleSensitivity;

    public float LineHoverEps
    {
      get => this._lineHoverEps;
      set => this._lineHoverEps = Mathf.Max(0.0f, value);
    }

    public float BoxHoverEps
    {
      get => this._boxHoverEps;
      set => this._boxHoverEps = Mathf.Max(0.0f, value);
    }

    public float CylinderHoverEps
    {
      get => this._cylinderHoverEps;
      set => this._cylinderHoverEps = Mathf.Max(0.0f, value);
    }

    public float OffsetSnapStep
    {
      get => this._offsetSnapStep;
      set => this._offsetSnapStep = Mathf.Max(0.0001f, value);
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

    public float ScaleSnapStep
    {
      get => this._scaleSnapStep;
      set => this._scaleSnapStep = Mathf.Max(0.0001f, value);
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

    public GizmoLineSlider3DSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._lineHoverEps = 0.7f;
      this._boxHoverEps = 0.5f;
      this._cylinderHoverEps = 0.5f;
      this._offsetSnapStep = 1f;
      this._rotationSnapStep = 15f;
      this._scaleSnapStep = 0.1f;
      this._offsetSensitivity = 1f;
      this._rotationSensitivity = 0.45f;
      this._scaleSensitivity = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
