// Decompiled with JetBrains decompiler
// Type: RTG.CapsuleCollider3DSnapshot
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class CapsuleCollider3DSnapshot
  {
    private CapsuleCollider _capsuleCollider;
    private Vector3 _localCenter;
    private float _localRadius;
    private float _localHeight;

    public CapsuleCollider3DSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public CapsuleCollider3DSnapshot(CapsuleCollider3DSnapshot src)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._capsuleCollider = src._capsuleCollider;
      this._localCenter = src._localCenter;
      this._localRadius = src._localRadius;
      this._localHeight = src._localHeight;
    }

    public void Snapshot(CapsuleCollider capsuleCollider)
    {
      if ((Object) capsuleCollider == (Object) null)
        return;
      this._capsuleCollider = capsuleCollider;
      this._localCenter = capsuleCollider.center;
      this._localRadius = capsuleCollider.radius;
      this._localHeight = capsuleCollider.height;
    }

    public void Apply()
    {
      if ((Object) this._capsuleCollider == (Object) null)
        return;
      this._capsuleCollider.center = this._localCenter;
      this._capsuleCollider.radius = this._localRadius;
      this._capsuleCollider.height = this._localHeight;
    }
  }
}
