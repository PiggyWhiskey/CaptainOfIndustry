// Decompiled with JetBrains decompiler
// Type: RTG.CameraRotationSwitchSettings
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
  public class CameraRotationSwitchSettings : Settings
  {
    private static readonly float _minConstantDuration;
    [SerializeField]
    private CameraRotationSwitchMode _switchMode;
    [SerializeField]
    private CameraRotationSwitchType _switchType;
    [SerializeField]
    private float _constantSwitchDurationInSeconds;
    [SerializeField]
    private float _smoothValue;

    public CameraRotationSwitchMode SwitchMode
    {
      get => this._switchMode;
      set => this._switchMode = value;
    }

    public CameraRotationSwitchType SwitchType
    {
      get => this._switchType;
      set => this._switchType = value;
    }

    public float ConstantSwitchDurationInSeconds
    {
      get => this._constantSwitchDurationInSeconds;
      set
      {
        this._constantSwitchDurationInSeconds = Mathf.Max(value, CameraRotationSwitchSettings._minConstantDuration);
      }
    }

    public float SmoothValue
    {
      get => this._smoothValue;
      set => this._smoothValue = Mathf.Max(value, 1f / 1000f);
    }

    public CameraRotationSwitchSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._switchMode = CameraRotationSwitchMode.Smooth;
      this._constantSwitchDurationInSeconds = 0.3f;
      this._smoothValue = 8f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static CameraRotationSwitchSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CameraRotationSwitchSettings._minConstantDuration = 0.1f;
    }
  }
}
