// Decompiled with JetBrains decompiler
// Type: RTG.XZGridRayHit
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class XZGridRayHit
  {
    private XZGridCell _hitCell;
    private Vector3 _hitPoint;
    private float _hitEnter;
    private Vector3 _hitNormal;
    private Plane _hitPlane;

    public XZGridCell HitCell => this._hitCell;

    public Vector3 HitPoint => this._hitPoint;

    public float HitEnter => this._hitEnter;

    public Vector3 HitNormal => this._hitNormal;

    public Plane HitPlane => this._hitPlane;

    public XZGridRayHit(Ray ray, XZGridCell hitCell, float hitEnter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._hitCell = hitCell;
      this._hitEnter = hitEnter;
      this._hitPoint = ray.GetPoint(hitEnter);
      this._hitPlane = hitCell.ParentGrid.WorldPlane;
      this._hitNormal = this._hitPlane.normal;
    }
  }
}
