// Decompiled with JetBrains decompiler
// Type: RTG.CameraOrbitSettings
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
  public class CameraOrbitSettings : Settings
  {
    [SerializeField]
    private CameraOrbitMode _orbitMode;
    [SerializeField]
    private float _standardOrbitSensitivity;
    [SerializeField]
    private float _smoothOrbitSensitivity;
    [SerializeField]
    private float _smoothValue;
    [SerializeField]
    private bool _invertX;
    [SerializeField]
    private bool _invertY;
    [SerializeField]
    private bool _isOrbitEnabled;

    public CameraOrbitMode OrbitMode
    {
      get => this._orbitMode;
      set => this._orbitMode = value;
    }

    public float StandardOrbitSensitivity
    {
      get => this._standardOrbitSensitivity;
      set => this._standardOrbitSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float SmoothOrbitSensitivity
    {
      get => this._smoothOrbitSensitivity;
      set => this._smoothOrbitSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float OrbitSensitivity
    {
      get
      {
        return this._orbitMode != CameraOrbitMode.Smooth ? this._standardOrbitSensitivity : this._smoothOrbitSensitivity;
      }
    }

    public float SmoothValue
    {
      get => this._smoothValue;
      set => this._smoothValue = Mathf.Max(value, 1f / 1000f);
    }

    public bool InvertX
    {
      get => this._invertX;
      set => this._invertX = value;
    }

    public bool InvertY
    {
      get => this._invertY;
      set => this._invertY = value;
    }

    public bool IsOrbitEnabled
    {
      get => this._isOrbitEnabled;
      set => this._isOrbitEnabled = value;
    }

    public CameraOrbitSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._standardOrbitSensitivity = 5f;
      this._smoothOrbitSensitivity = 5f;
      this._smoothValue = 8f;
      this._isOrbitEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
