// Decompiled with JetBrains decompiler
// Type: RTG.Plane2D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class Plane2D
  {
    private Vector2 _normal;
    private float _distance;

    public Vector2 Normal
    {
      get => this._normal;
      set => this._normal = value.normalized;
    }

    public float Distance
    {
      get => this._distance;
      set => this._distance = value;
    }

    public Plane2D(Vector2 normal, float distance)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._normal = normal.normalized;
      this._distance = distance;
    }

    public Plane2D(Vector2 normal, Vector2 pointOnPlane)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._normal = normal.normalized;
      this._distance = Vector2.Dot(pointOnPlane, this._normal);
    }

    public float GetDistanceToPoint(Vector2 point)
    {
      return Vector2.Dot(point, this._normal) - this._distance;
    }

    public bool Raycast(Vector2 rayOrigin, Vector2 rayDir, out float t)
    {
      t = 0.0f;
      float f = Vector2.Dot(rayDir, this._normal);
      if ((double) Mathf.Abs(f) < 9.9999997473787516E-06)
        return false;
      float distanceToPoint = this.GetDistanceToPoint(rayOrigin);
      t = -distanceToPoint / f;
      return (double) t >= 0.0;
    }
  }
}
