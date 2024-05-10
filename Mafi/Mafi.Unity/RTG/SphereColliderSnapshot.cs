// Decompiled with JetBrains decompiler
// Type: RTG.SphereColliderSnapshot
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SphereColliderSnapshot
  {
    private SphereCollider _sphereCollider;
    private Vector3 _localCenter;
    private float _localRadius;

    public SphereColliderSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public SphereColliderSnapshot(SphereColliderSnapshot src)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sphereCollider = src._sphereCollider;
      this._localCenter = src._localCenter;
      this._localRadius = src._localRadius;
    }

    public void Snapshot(SphereCollider sphereCollider)
    {
      if ((Object) sphereCollider == (Object) null)
        return;
      this._sphereCollider = sphereCollider;
      this._localCenter = sphereCollider.center;
      this._localRadius = sphereCollider.radius;
    }

    public void Apply()
    {
      if ((Object) this._sphereCollider == (Object) null)
        return;
      this._sphereCollider.center = this._localCenter;
      this._sphereCollider.radius = this._localRadius;
    }
  }
}
