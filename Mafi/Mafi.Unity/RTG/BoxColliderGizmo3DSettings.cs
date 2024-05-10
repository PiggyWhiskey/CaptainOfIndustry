// Decompiled with JetBrains decompiler
// Type: RTG.BoxColliderGizmo3DSettings
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
  public class BoxColliderGizmo3DSettings
  {
    [SerializeField]
    private float _xSizeSnapStep;
    [SerializeField]
    private float _ySizeSnapStep;
    [SerializeField]
    private float _zSizeSnapStep;
    [SerializeField]
    private float _uniformSizeSnapStep;

    public static float DefaultSizeSnapStep => 0.1f;

    public static float DefaultUniformSizeSnapStep => 0.1f;

    public float XSizeSnapStep => this._xSizeSnapStep;

    public float YSizeSnapStep => this._ySizeSnapStep;

    public float ZSizeSnapStep => this._zSizeSnapStep;

    public float UniformSizeSnapStep => this._uniformSizeSnapStep;

    public void SetXSizeSnapStep(float snapStep)
    {
      this._xSizeSnapStep = Mathf.Max(-0.0001f, snapStep);
    }

    public void SetYSizeSnapStep(float snapStep)
    {
      this._ySizeSnapStep = Mathf.Max(-0.0001f, snapStep);
    }

    public void SetZSizeSnapStep(float snapStep)
    {
      this._zSizeSnapStep = Mathf.Max(-0.0001f, snapStep);
    }

    public void SetUniformSizeSnapStep(float snapStep)
    {
      this._uniformSizeSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public BoxColliderGizmo3DSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._xSizeSnapStep = BoxColliderGizmo3DSettings.DefaultSizeSnapStep;
      this._ySizeSnapStep = BoxColliderGizmo3DSettings.DefaultSizeSnapStep;
      this._zSizeSnapStep = BoxColliderGizmo3DSettings.DefaultSizeSnapStep;
      this._uniformSizeSnapStep = BoxColliderGizmo3DSettings.DefaultUniformSizeSnapStep;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
