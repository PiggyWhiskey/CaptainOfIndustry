// Decompiled with JetBrains decompiler
// Type: RTG.CameraMoveSettings
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
  public class CameraMoveSettings : Settings
  {
    private static readonly float _minMoveSpeed;
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _accelerationRate;

    public float MoveSpeed
    {
      get => this._moveSpeed;
      set => this._moveSpeed = Mathf.Max(CameraMoveSettings._minMoveSpeed, value);
    }

    public float AccelerationRate
    {
      get => this._accelerationRate;
      set => this._accelerationRate = Mathf.Max(0.0f, value);
    }

    public CameraMoveSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._moveSpeed = 6f;
      this._accelerationRate = 15f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static CameraMoveSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CameraMoveSettings._minMoveSpeed = 0.1f;
    }
  }
}
