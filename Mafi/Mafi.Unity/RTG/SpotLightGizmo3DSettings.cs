// Decompiled with JetBrains decompiler
// Type: RTG.SpotLightGizmo3DSettings
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
  public class SpotLightGizmo3DSettings
  {
    [SerializeField]
    private float _radiusSnapStep;
    [SerializeField]
    private float _rangeSnapStep;

    public static float DefaultRadiusSnapStep => 0.1f;

    public static float DefaultRangeSnapStep => 0.1f;

    public float RadiusSnapStep => this._radiusSnapStep;

    public float RangeSnapStep => this._rangeSnapStep;

    public void SetRadiusSnapStep(float snapStep)
    {
      this._radiusSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public void SetRangeSnapStep(float snapStep)
    {
      this._rangeSnapStep = Mathf.Max(0.0001f, snapStep);
    }

    public SpotLightGizmo3DSettings()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._radiusSnapStep = SpotLightGizmo3DSettings.DefaultRadiusSnapStep;
      this._rangeSnapStep = SpotLightGizmo3DSettings.DefaultRangeSnapStep;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
