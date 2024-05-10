// Decompiled with JetBrains decompiler
// Type: RTG.CameraBackgroundSettings
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
  public class CameraBackgroundSettings : Settings
  {
    [SerializeField]
    private Color _firstColor;
    [SerializeField]
    private Color _secondColor;
    [SerializeField]
    private float _gradientOffset;
    [SerializeField]
    private bool _isVisible;

    public Color FirstColor
    {
      get => this._firstColor;
      set => this._firstColor = value;
    }

    public Color SecondColor
    {
      get => this._secondColor;
      set => this._secondColor = value;
    }

    public float GradientOffset
    {
      get => this._gradientOffset;
      set => this._gradientOffset = Mathf.Clamp(value, -1f, 1f);
    }

    public bool IsVisible
    {
      get => this._isVisible;
      set => this._isVisible = value;
    }

    public CameraBackgroundSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._firstColor = RTSystemValues.CameraBkGradientFirstColor;
      this._secondColor = RTSystemValues.CameraBkGradientSecondColor;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
