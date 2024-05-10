// Decompiled with JetBrains decompiler
// Type: RTG.CameraZoomSettings
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
  public class CameraZoomSettings : Settings
  {
    [SerializeField]
    private CameraZoomMode _zoomMode;
    [SerializeField]
    private float _orthoStandardZoomSensitivity;
    [SerializeField]
    private float _perspStandardZoomSensitivity;
    [SerializeField]
    private float _orthoSmoothZoomSensitivity;
    [SerializeField]
    private float _perspSmoothZoomSensitivity;
    [SerializeField]
    private float _orthoZoomSmoothValue;
    [SerializeField]
    private float _perspZoomSmoothValue;
    [SerializeField]
    private bool _invertZoomAxis;
    [SerializeField]
    private bool _isZoomEnabled;

    public CameraZoomMode ZoomMode
    {
      get => this._zoomMode;
      set => this._zoomMode = value;
    }

    public float OrthoStandardZoomSensitivity
    {
      get => this._orthoStandardZoomSensitivity;
      set => this._orthoStandardZoomSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float PerspStandardZoomSensitivity
    {
      get => this._perspStandardZoomSensitivity;
      set => this._perspStandardZoomSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float OrthoSmoothZoomSensitivity
    {
      get => this._orthoSmoothZoomSensitivity;
      set => this._orthoSmoothZoomSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float PerspSmoothZoomSensitivity
    {
      get => this._perspSmoothZoomSensitivity;
      set => this._perspSmoothZoomSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float OrthoZoomSmoothValue
    {
      get => this._orthoZoomSmoothValue;
      set => this._orthoZoomSmoothValue = Mathf.Max(value, 1f / 1000f);
    }

    public float PerspZoomSmoothValue
    {
      get => this._perspZoomSmoothValue;
      set => this._perspZoomSmoothValue = Mathf.Max(value, 1f / 1000f);
    }

    public bool InvertZoomAxis
    {
      get => this._invertZoomAxis;
      set => this._invertZoomAxis = value;
    }

    public bool IsZoomEnabled
    {
      get => this._isZoomEnabled;
      set => this._isZoomEnabled = value;
    }

    public float GetZoomSmoothValue(Camera camera)
    {
      return !camera.orthographic ? this.PerspZoomSmoothValue : this.OrthoZoomSmoothValue;
    }

    public float GetZoomSensitivity(Camera camera)
    {
      if (this._zoomMode == CameraZoomMode.Standard)
        return !camera.orthographic ? this.PerspStandardZoomSensitivity : this.OrthoStandardZoomSensitivity;
      if (this._zoomMode != CameraZoomMode.Smooth)
        return 0.0f;
      return !camera.orthographic ? this.PerspSmoothZoomSensitivity : this.OrthoSmoothZoomSensitivity;
    }

    public CameraZoomSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._orthoStandardZoomSensitivity = 10f;
      this._perspStandardZoomSensitivity = 10f;
      this._orthoSmoothZoomSensitivity = 5f;
      this._perspSmoothZoomSensitivity = 5f;
      this._orthoZoomSmoothValue = 5f;
      this._perspZoomSmoothValue = 5f;
      this._isZoomEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
