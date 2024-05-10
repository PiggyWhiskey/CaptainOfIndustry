// Decompiled with JetBrains decompiler
// Type: RTG.BoxEpsilon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public struct BoxEpsilon
  {
    private Vector3 _sizeEps;

    public Vector3 SizeEps
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

    public float DepthEps
    {
      get => this._sizeEps.z;
      set => this._sizeEps.z = Mathf.Abs(value);
    }
  }
}
