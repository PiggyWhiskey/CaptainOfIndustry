// Decompiled with JetBrains decompiler
// Type: RTG.SegmentEpsilon
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public struct SegmentEpsilon
  {
    private float _raycastEps;
    private float _ptOnSegmentEps;

    public float RaycastEps
    {
      get => this._raycastEps;
      set => this._raycastEps = Mathf.Abs(value);
    }

    public float PtOnSegmentEps
    {
      get => this._ptOnSegmentEps;
      set => this._ptOnSegmentEps = Mathf.Abs(value);
    }
  }
}
