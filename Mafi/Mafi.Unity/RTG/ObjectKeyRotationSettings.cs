// Decompiled with JetBrains decompiler
// Type: RTG.ObjectKeyRotationSettings
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
  public class ObjectKeyRotationSettings : Settings
  {
    [SerializeField]
    private float _xRotationStep;
    [SerializeField]
    private float _yRotationStep;
    [SerializeField]
    private float _zRotationStep;

    public float XRotationStep
    {
      get => this._xRotationStep;
      set => this._xRotationStep = Mathf.Max(0.0001f, value);
    }

    public float YRotationStep
    {
      get => this._yRotationStep;
      set => this._yRotationStep = Mathf.Max(0.0001f, value);
    }

    public float ZRotationStep
    {
      get => this._zRotationStep;
      set => this._zRotationStep = Mathf.Max(0.0001f, value);
    }

    public ObjectKeyRotationSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._xRotationStep = 90f;
      this._yRotationStep = 90f;
      this._zRotationStep = 90f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
