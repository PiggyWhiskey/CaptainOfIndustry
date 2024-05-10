// Decompiled with JetBrains decompiler
// Type: RTG.TorusEpsilon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public struct TorusEpsilon
  {
    private float _tubeRadiusEps;
    private float _cylHrzRadius;
    private float _cylVertRadius;

    public float TubeRadiusEps
    {
      get => this._tubeRadiusEps;
      set => this._tubeRadiusEps = Mathf.Abs(value);
    }

    public float CylHrzRadius
    {
      get => this._cylHrzRadius;
      set => this._cylHrzRadius = Mathf.Abs(value);
    }

    public float CylVertRadius
    {
      get => this._cylVertRadius;
      set => this._cylVertRadius = Mathf.Abs(value);
    }
  }
}
