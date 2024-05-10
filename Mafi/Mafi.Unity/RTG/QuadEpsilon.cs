// Decompiled with JetBrains decompiler
// Type: RTG.QuadEpsilon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public struct QuadEpsilon
  {
    private Vector2 _sizeEps;
    private float _extrudeEps;
    private float _wireEps;

    public Vector2 SizeEps
    {
      get => this._sizeEps;
      set => this._sizeEps = value.Abs();
    }

    public float WidthEps
    {
      get => this._sizeEps.x;
      set => this._sizeEps.x = Mathf.Abs(value);
    }

    public float HeightEps
    {
      get => this._sizeEps.y;
      set => this._sizeEps.y = Mathf.Abs(value);
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
  }
}
