// Decompiled with JetBrains decompiler
// Type: RTG.BoxCollider3DSnapshot
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class BoxCollider3DSnapshot
  {
    private BoxCollider _boxCollider;
    private Vector3 _localCenter;
    private Vector3 _localSize;

    public BoxCollider3DSnapshot()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public BoxCollider3DSnapshot(BoxCollider3DSnapshot src)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._boxCollider = src._boxCollider;
      this._localCenter = src._localCenter;
      this._localSize = src._localSize;
    }

    public void Snapshot(BoxCollider boxCollider)
    {
      if ((Object) boxCollider == (Object) null)
        return;
      this._boxCollider = boxCollider;
      this._localCenter = boxCollider.center;
      this._localSize = boxCollider.size;
    }

    public void Apply()
    {
      if ((Object) this._boxCollider == (Object) null)
        return;
      this._boxCollider.center = this._localCenter;
      this._boxCollider.size = this._localSize;
    }
  }
}
