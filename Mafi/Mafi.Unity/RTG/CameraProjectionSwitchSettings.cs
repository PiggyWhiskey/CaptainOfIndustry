// Decompiled with JetBrains decompiler
// Type: RTG.CameraProjectionSwitchSettings
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
  public class CameraProjectionSwitchSettings : Settings
  {
    [SerializeField]
    private CameraProjectionSwitchMode _switchMode;
    [SerializeField]
    private float _transitionDurationInSeconds;

    public CameraProjectionSwitchMode SwitchMode
    {
      get => this._switchMode;
      set => this._switchMode = value;
    }

    public float TransitionDurationInSeconds
    {
      get => this._transitionDurationInSeconds;
      set => this._transitionDurationInSeconds = Mathf.Max(value, 0.01f);
    }

    public CameraProjectionSwitchSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._transitionDurationInSeconds = 0.23f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
