// Decompiled with JetBrains decompiler
// Type: RTG.CameraFocusSettings
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
  public class CameraFocusSettings : Settings
  {
    [SerializeField]
    private CameraFocusMode _focusMode;
    [SerializeField]
    private float _constantSpeed;
    [SerializeField]
    private float _smoothTime;
    [SerializeField]
    private float _focusDistanceAdd;

    public CameraFocusMode FocusMode
    {
      get => this._focusMode;
      set => this._focusMode = value;
    }

    public float ConstantSpeed
    {
      get => this._constantSpeed;
      set => this._constantSpeed = Mathf.Max(value, 0.0001f);
    }

    public float SmoothTime
    {
      get => this._smoothTime;
      set => this._smoothTime = Mathf.Max(value, 0.0001f);
    }

    public float FocusDistanceAdd
    {
      get => this._focusDistanceAdd;
      set => this._focusDistanceAdd = Mathf.Max(value, 0.0001f);
    }

    public CameraFocusSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._focusMode = CameraFocusMode.Smooth;
      this._constantSpeed = 10f;
      this._smoothTime = 1.5f;
      this._focusDistanceAdd = 1.2f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
