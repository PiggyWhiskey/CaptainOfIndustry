// Decompiled with JetBrains decompiler
// Type: RTG.XZGridSettings
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
  public class XZGridSettings : Settings
  {
    [SerializeField]
    private bool _isVisible;
    [SerializeField]
    private float _cellSizeX;
    [SerializeField]
    private float _cellSizeZ;
    [SerializeField]
    private float _yOffset;
    [SerializeField]
    private Vector3 _rotationAngles;
    [SerializeField]
    private float _upDownStep;

    public bool IsVisible
    {
      get => this._isVisible;
      set => this._isVisible = value;
    }

    public float CellSizeX
    {
      get => this._cellSizeX;
      set => this._cellSizeX = Mathf.Max(value, 0.01f);
    }

    public float CellSizeZ
    {
      get => this._cellSizeZ;
      set => this._cellSizeZ = Mathf.Max(value, 0.01f);
    }

    public Vector3 RotationAngles
    {
      get => this._rotationAngles;
      set => this._rotationAngles = value;
    }

    public float YOffset
    {
      get => this._yOffset;
      set => this._yOffset = value;
    }

    public float UpDownStep
    {
      get => this._upDownStep;
      set => this._upDownStep = Mathf.Max(0.0001f, value);
    }

    public XZGridSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._cellSizeX = 1f;
      this._cellSizeZ = 1f;
      this._rotationAngles = Vector3.zero;
      this._upDownStep = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
