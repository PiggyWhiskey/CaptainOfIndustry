// Decompiled with JetBrains decompiler
// Type: RTG.CameraLookAroundSettings
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
  public class CameraLookAroundSettings : Settings
  {
    [SerializeField]
    private CameraLookAroundMode _lookAroundMode;
    [SerializeField]
    private float _standardLookAroundSensitivity;
    [SerializeField]
    private float _smoothLookAroundSensitivity;
    [SerializeField]
    private float smoothValue;
    [SerializeField]
    private bool _invertX;
    [SerializeField]
    private bool _invertY;
    [SerializeField]
    private bool _isLookAroundEnabled;

    public CameraLookAroundMode LookAroundMode
    {
      get => this._lookAroundMode;
      set => this._lookAroundMode = value;
    }

    public float StandardLookAroundSensitivity
    {
      get => this._standardLookAroundSensitivity;
      set => this._standardLookAroundSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float SmoothLookAroundSensitivity
    {
      get => this._smoothLookAroundSensitivity;
      set => this._smoothLookAroundSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float Sensitivity
    {
      get
      {
        return this._lookAroundMode != CameraLookAroundMode.Standard ? this._smoothLookAroundSensitivity : this._standardLookAroundSensitivity;
      }
    }

    public float SmoothValue
    {
      get => this.smoothValue;
      set => this.smoothValue = Mathf.Max(value, 1f / 1000f);
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

    public bool IsLookAroundEnabled
    {
      get => this._isLookAroundEnabled;
      set => this._isLookAroundEnabled = value;
    }

    public CameraLookAroundSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._standardLookAroundSensitivity = 5f;
      this._smoothLookAroundSensitivity = 5f;
      this.smoothValue = 4f;
      this._isLookAroundEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
