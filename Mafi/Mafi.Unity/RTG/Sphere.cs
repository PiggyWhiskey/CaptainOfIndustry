// Decompiled with JetBrains decompiler
// Type: RTG.Sphere
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public struct Sphere
  {
    private Vector3 _center;
    private float _radius;

    public Vector3 Center
    {
      get => this._center;
      set => this._center = value;
    }

    public float Radius
    {
      get => this._radius;
      set => this._radius = Mathf.Max(0.0f, value);
    }

    public Sphere(Vector3 center, float radius)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = center;
      this._radius = Mathf.Max(0.0f, radius);
    }

    public Sphere(AABB aabb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = aabb.Center;
      this._radius = aabb.Extents.magnitude;
    }

    public Sphere(IEnumerable<Vector3> pointCloud)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Vector3 rhs1 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
      Vector3 rhs2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
      foreach (Vector3 lhs in pointCloud)
      {
        rhs1 = Vector3.Min(lhs, rhs1);
        rhs2 = Vector3.Max(lhs, rhs2);
      }
      this._center = (rhs1 + rhs2) * 0.5f;
      this._radius = (rhs2 - rhs1).magnitude * 0.5f;
    }

    public bool ContainsPoint(Vector3 point)
    {
      return (double) (this._center - point).sqrMagnitude <= (double) this._radius * (double) this._radius;
    }

    public List<Vector3> GetRightUpExtents(Vector3 right, Vector3 up)
    {
      return SphereMath.CalcRightUpExtents(this._center, this._radius, right, up);
    }

    /// <summary>
    /// Encapsulates the specified sphere. This method will adjust the sphere's
    /// center and radius to include 'sphere'. The method has no effect if the
    /// passed sphere is aready encapsulated by 'this' sphere.
    /// </summary>
    public void Encapsulate(Sphere sphere)
    {
      Vector3 vector3_1 = Vector3.Normalize(sphere.Center - this._center);
      Vector3 point = sphere.Center + vector3_1 * sphere.Radius;
      if (this.ContainsPoint(point))
        return;
      Vector3 vector3_2 = this._center - vector3_1 * this._radius;
      this.Radius = (point - vector3_2).magnitude * 0.5f;
      this.Center = point - vector3_1 * this._radius;
    }
  }
}
