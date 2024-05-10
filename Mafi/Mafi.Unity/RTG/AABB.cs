// Decompiled with JetBrains decompiler
// Type: RTG.AABB
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
  public struct AABB
  {
    private Vector3 _size;
    private Vector3 _center;
    private bool _isValid;

    public bool IsValid => this._isValid;

    public Vector3 Center
    {
      get => this._center;
      set => this._center = value;
    }

    public Vector3 Size
    {
      get => this._size;
      set => this._size = value;
    }

    public Vector3 Extents => this.Size * 0.5f;

    public Vector3 Min
    {
      get => this.Center - this.Extents;
      set
      {
        if (!this._isValid)
          return;
        this.RecalculateCenterAndSize(Vector3.Min(value, this.Max), this.Max);
      }
    }

    public Vector3 Max
    {
      get => this.Center + this.Extents;
      set
      {
        if (!this._isValid)
          return;
        this.RecalculateCenterAndSize(this.Min, Vector3.Max(value, this.Min));
      }
    }

    public AABB(Vector3 center, Vector3 size)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = center;
      this._size = size;
      this._isValid = true;
    }

    public AABB(Bounds bounds)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = bounds.center;
      this._size = bounds.size;
      this._isValid = true;
    }

    public AABB(IEnumerable<Vector3> pointCloud)
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
      this._size = rhs2 - rhs1;
      this._isValid = true;
    }

    public AABB(IEnumerable<Vector2> pointCloud)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      Vector2 rhs1 = new Vector2(float.MaxValue, float.MaxValue);
      Vector2 rhs2 = new Vector2(float.MinValue, float.MinValue);
      foreach (Vector2 lhs in pointCloud)
      {
        rhs1 = Vector2.Min(lhs, rhs1);
        rhs2 = Vector2.Max(lhs, rhs2);
      }
      this._center = (Vector3) ((rhs1 + rhs2) * 0.5f);
      this._size = (Vector3) (rhs2 - rhs1);
      this._isValid = true;
    }

    public static AABB GetInvalid() => new AABB();

    public void Inflate(float amount) => this.Size += Vector3Ex.FromValue(amount);

    public void Inflate(Vector3 amount) => this.Size += amount;

    public void Encapsulate(Vector3 point)
    {
      Vector3 min = this.Min;
      Vector3 max = this.Max;
      if ((double) point.x < (double) min.x)
        min.x = point.x;
      if ((double) point.x > (double) max.x)
        max.x = point.x;
      if ((double) point.y < (double) min.y)
        min.y = point.y;
      if ((double) point.y > (double) max.y)
        max.y = point.y;
      if ((double) point.z < (double) min.z)
        min.z = point.z;
      if ((double) point.z > (double) max.z)
        max.z = point.z;
      this._isValid = true;
      this.Min = min;
      this.Max = max;
    }

    public void Encapsulate(IEnumerable<Vector3> points)
    {
      foreach (Vector3 point in points)
        this.Encapsulate(point);
    }

    public void Encapsulate(AABB aabb)
    {
      Vector3 min1 = this.Min;
      Vector3 max1 = this.Max;
      Vector3 min2 = aabb.Min;
      Vector3 max2 = aabb.Max;
      if ((double) min2.x < (double) min1.x)
        min1.x = min2.x;
      if ((double) min2.y < (double) min1.y)
        min1.y = min2.y;
      if ((double) min2.z < (double) min1.z)
        min1.z = min2.z;
      if ((double) max2.x > (double) max1.x)
        max1.x = max2.x;
      if ((double) max2.y > (double) max1.y)
        max1.y = max2.y;
      if ((double) max2.z > (double) max1.z)
        max1.z = max2.z;
      this._isValid = true;
      this.Min = min1;
      this.Max = max1;
    }

    public void Transform(Matrix4x4 transformMatrix)
    {
      BoxMath.TransformBox(this._center, this._size, transformMatrix, out this._center, out this._size);
    }

    public bool ContainsPoint(Vector3 point)
    {
      return BoxMath.ContainsPoint(point, this._center, this._size, Quaternion.identity);
    }

    public List<Vector3> GetCornerPoints()
    {
      return BoxMath.CalcBoxCornerPoints(this._center, this._size, Quaternion.identity);
    }

    public List<Vector3> GetCenterAndCornerPoints()
    {
      List<Vector3> cornerPoints = this.GetCornerPoints();
      cornerPoints.Add(this.Center);
      return cornerPoints;
    }

    public List<Vector2> GetScreenCornerPoints(Camera camera)
    {
      List<Vector3> cornerPoints = this.GetCornerPoints();
      List<Vector2> screenCornerPoints = new List<Vector2>(cornerPoints.Count);
      foreach (Vector3 position in cornerPoints)
        screenCornerPoints.Add((Vector2) camera.WorldToScreenPoint(position));
      return screenCornerPoints;
    }

    public List<Vector2> GetScreenCenterAndCornerPoints(Camera camera)
    {
      List<Vector3> centerAndCornerPoints1 = this.GetCenterAndCornerPoints();
      List<Vector2> centerAndCornerPoints2 = new List<Vector2>(centerAndCornerPoints1.Count);
      foreach (Vector3 position in centerAndCornerPoints1)
        centerAndCornerPoints2.Add((Vector2) camera.WorldToScreenPoint(position));
      return centerAndCornerPoints2;
    }

    public Rect GetScreenRectangle(Camera camera)
    {
      List<Vector2> screenCornerPoints = this.GetScreenCornerPoints(camera);
      Vector3 lhs1 = (Vector3) screenCornerPoints[0];
      Vector3 lhs2 = (Vector3) screenCornerPoints[0];
      for (int index = 1; index < screenCornerPoints.Count; ++index)
      {
        lhs1 = Vector3.Min(lhs1, (Vector3) screenCornerPoints[index]);
        lhs2 = Vector3.Max(lhs2, (Vector3) screenCornerPoints[index]);
      }
      return Rect.MinMaxRect(lhs1.x, lhs1.y, lhs2.x, lhs2.y);
    }

    public Matrix4x4 GetUnitBoxTransform()
    {
      return Matrix4x4.TRS(this.Center, Quaternion.identity, this.Size);
    }

    public Bounds ToBounds() => new Bounds(this._center, this._size);

    private void RecalculateCenterAndSize(Vector3 min, Vector3 max)
    {
      this._center = (min + max) * 0.5f;
      this._size = max - min;
    }
  }
}
