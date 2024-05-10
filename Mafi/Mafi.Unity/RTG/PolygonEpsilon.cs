// Decompiled with JetBrains decompiler
// Type: RTG.PolygonEpsilon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public struct PolygonEpsilon
  {
    private float _areaEps;
    private float _extrudeEps;
    private float _wireEps;
    private float _thickWireEps;

    public float AreaEps
    {
      get => this._areaEps;
      set => this._areaEps = Mathf.Abs(value);
    }

    public float ExtrudeEps
    {
      get => this._extrudeEps;
      set => this._extrudeEps = Mathf.Abs(value);
    }

    public float WireEps
    {
      get => this._wireEps;
      set => this._wireEps = Mathf.Abs(value);
    }

    public float ThickWireEps
    {
      get => this._thickWireEps;
      set => this._thickWireEps = Mathf.Abs(value);
    }
  }
}
