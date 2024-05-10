// Decompiled with JetBrains decompiler
// Type: RTG.CameraPanSettings
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
  public class CameraPanSettings : Settings
  {
    [SerializeField]
    private CameraPanMode _panMode;
    [SerializeField]
    private float _standardPanSensitivity;
    [SerializeField]
    private float _smoothPanSensitivity;
    [SerializeField]
    private float _smoothValue;
    [SerializeField]
    private bool _invertX;
    [SerializeField]
    private bool _invertY;
    [SerializeField]
    private bool _isPanningEnabled;

    public CameraPanMode PanMode
    {
      get => this._panMode;
      set => this._panMode = value;
    }

    public float StandardPanSensitivity
    {
      get => this._standardPanSensitivity;
      set => this._standardPanSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float SmoothPanSensitivity
    {
      get => this._smoothPanSensitivity;
      set => this._smoothPanSensitivity = Mathf.Max(value, 1f / 1000f);
    }

    public float Sensitivity
    {
      get
      {
        return this._panMode != CameraPanMode.Standard ? this._smoothPanSensitivity : this._standardPanSensitivity;
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

    public bool IsPanningEnabled
    {
      get => this._isPanningEnabled;
      set => this._isPanningEnabled = value;
    }

    public CameraPanSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._standardPanSensitivity = 1f;
      this._smoothPanSensitivity = 0.7f;
      this._smoothValue = 4f;
      this._isPanningEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
