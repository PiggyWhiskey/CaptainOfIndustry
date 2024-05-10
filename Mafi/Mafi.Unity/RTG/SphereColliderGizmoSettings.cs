// Decompiled with JetBrains decompiler
// Type: RTG.SphereColliderGizmoSettings
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
  public class SphereColliderGizmoSettings
  {
    [SerializeField]
    private float _radiusSnapStep;

    public static float DefaultRadiusSnapStep => 0.1f;

    public float RadiusSnapStep => this._radiusSnapStep;

    public void SetRadiusSnapStep(float snapStep)
    {
      this._radiusSnapStep = Mathf.Max(-0.0001f, snapStep);
    }

    public SphereColliderGizmoSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._radiusSnapStep = SphereColliderGizmoSettings.DefaultRadiusSnapStep;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
