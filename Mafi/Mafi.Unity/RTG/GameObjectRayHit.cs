// Decompiled with JetBrains decompiler
// Type: RTG.GameObjectRayHit
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GameObjectRayHit
  {
    private GameObject _hitObject;
    private Vector3 _hitPoint;
    private float _hitEnter;
    private Vector3 _hitNormal;
    private Plane _hitPlane;
    private MeshRayHit _meshRayHit;

    public GameObject HitObject => this._hitObject;

    public Vector3 HitPoint => this._hitPoint;

    public float HitEnter => this._hitEnter;

    public Vector3 HitNormal => this._hitNormal;

    public Plane HitPlane => this._hitPlane;

    public MeshRayHit MeshRayHit => this._meshRayHit;

    public static void SortByHitDistance(List<GameObjectRayHit> hits)
    {
      hits.Sort((Comparison<GameObjectRayHit>) ((h0, h1) => h0.HitEnter.CompareTo(h1.HitEnter)));
    }

    public static void Store(
      Ray hitRay,
      IEnumerable<RaycastHit> hits3D,
      List<GameObjectRayHit> hits)
    {
      hits.Clear();
      if (hits3D == null)
        return;
      foreach (RaycastHit hit3D in hits3D)
        hits.Add(new GameObjectRayHit(hitRay, hit3D));
    }

    public GameObjectRayHit(Ray hitRay, RaycastHit hit3D)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._hitObject = hit3D.collider.gameObject;
      this._hitPoint = hit3D.point;
      this._hitEnter = hit3D.distance;
      this._hitNormal = hit3D.normal;
      this._hitPlane = new Plane(this._hitNormal, this._hitPoint);
    }

    public GameObjectRayHit(Ray hitRay, GameObject hitObject, Vector3 hitNormal, float hitEnter)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._hitObject = hitObject;
      this._hitPoint = hitRay.GetPoint(hitEnter);
      this._hitEnter = hitEnter;
      this._hitNormal = hitNormal;
      this._hitPlane = new Plane(this._hitNormal, this._hitPoint);
    }

    public GameObjectRayHit(Ray ray, GameObject hitObject, MeshRayHit meshRayHit)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._hitObject = hitObject;
      this._hitPoint = meshRayHit.HitPoint;
      this._hitEnter = meshRayHit.HitEnter;
      this._hitNormal = meshRayHit.HitNormal;
      this._hitPlane = new Plane(this._hitNormal, this._hitPoint);
      this._meshRayHit = meshRayHit;
    }
  }
}
