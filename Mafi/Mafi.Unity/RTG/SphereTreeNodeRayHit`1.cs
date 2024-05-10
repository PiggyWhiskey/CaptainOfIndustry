// Decompiled with JetBrains decompiler
// Type: RTG.SphereTreeNodeRayHit`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SphereTreeNodeRayHit<T>
  {
    private SphereTreeNode<T> _hitNode;
    private Vector3 _hitPoint;
    private float _hitEnter;

    public SphereTreeNode<T> HitNode => this._hitNode;

    public Vector3 HitPoint => this._hitPoint;

    public float HitEnter => this._hitEnter;

    public SphereTreeNodeRayHit(Ray ray, SphereTreeNode<T> hitNode, float hitEnter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._hitNode = hitNode;
      this._hitEnter = hitEnter;
      this._hitPoint = ray.GetPoint(this._hitEnter);
    }
  }
}
