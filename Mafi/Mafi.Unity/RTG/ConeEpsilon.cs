// Decompiled with JetBrains decompiler
// Type: RTG.ConeEpsilon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public struct ConeEpsilon
  {
    private float _hrzEps;
    private float _vertEps;

    public float HrzEps
    {
      get => this._hrzEps;
      set => this._hrzEps = Mathf.Abs(value);
    }

    public float VertEps
    {
      get => this._vertEps;
      set => this._vertEps = Mathf.Abs(value);
    }
  }
}
