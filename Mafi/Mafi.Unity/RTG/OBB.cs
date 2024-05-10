// Decompiled with JetBrains decompiler
// Type: RTG.OBB
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
  public struct OBB
  {
    private Vector3 _size;
    private Vector3 _center;
    private Quaternion _rotation;
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

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public Matrix4x4 RotationMatrix => Matrix4x4.TRS(Vector3.zero, this._rotation, Vector3.one);

    public Vector3 Right => this._rotation * Vector3.right;

    public Vector3 Up => this._rotation * Vector3.up;

    public Vector3 Look => this._rotation * Vector3.forward;

    public OBB(Vector3 center, Vector3 size)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = center;
      this._size = size;
      this._rotation = Quaternion.identity;
      this._isValid = true;
    }

    public OBB(Vector3 center, Vector3 size, Quaternion rotation)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = center;
      this._size = size;
      this._rotation = rotation;
      this._isValid = true;
    }

    public OBB(Vector3 center, Quaternion rotation)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = center;
      this._size = Vector3.zero;
      this._rotation = rotation;
      this._isValid = true;
    }

    public OBB(Quaternion rotation)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = Vector3.zero;
      this._size = Vector3.zero;
      this._rotation = rotation;
      this._isValid = true;
    }

    public OBB(Bounds bounds, Quaternion rotation)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = bounds.center;
      this._size = bounds.size;
      this._rotation = rotation;
      this._isValid = true;
    }

    public OBB(AABB aabb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = aabb.Center;
      this._size = aabb.Size;
      this._rotation = Quaternion.identity;
      this._isValid = true;
    }

    public OBB(AABB aabb, Quaternion rotation)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = aabb.Center;
      this._size = aabb.Size;
      this._rotation = rotation;
      this._isValid = true;
    }

    public OBB(AABB modelSpaceAABB, Transform worldTransform)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._size = Vector3.Scale(modelSpaceAABB.Size, worldTransform.lossyScale);
      this._center = worldTransform.TransformPoint(modelSpaceAABB.Center);
      this._rotation = worldTransform.rotation;
      this._isValid = true;
    }

    public OBB(OBB copy)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._size = copy._size;
      this._center = copy._center;
      this._rotation = copy._rotation;
      this._isValid = copy._isValid;
    }

    public static OBB GetInvalid() => new OBB();

    public void Inflate(float amount) => this.Size += Vector3Ex.FromValue(amount);

    public Matrix4x4 GetUnitBoxTransform()
    {
      return !this._isValid ? Matrix4x4.identity : Matrix4x4.TRS(this.Center, this.Rotation, this.Size);
    }

    public List<Vector3> GetCornerPoints()
    {
      return BoxMath.CalcBoxCornerPoints(this._center, this._size, this._rotation);
    }

    public List<Vector3> GetCenterAndCornerPoints()
    {
      List<Vector3> cornerPoints = this.GetCornerPoints();
      cornerPoints.Add(this.Center);
      return cornerPoints;
    }

    public void Encapsulate(OBB otherOBB)
    {
      List<Vector3> points1 = BoxMath.CalcBoxCornerPoints(otherOBB.Center, otherOBB.Size, otherOBB.Rotation);
      List<Vector3> points2 = Matrix4x4.TRS(this.Center, this.Rotation, Vector3.one).inverse.TransformPoints(points1);
      AABB aabb = new AABB(Vector3.zero, this.Size);
      aabb.Encapsulate((IEnumerable<Vector3>) points2);
      this.Center = this.Rotation * aabb.Center + this.Center;
      this.Size = aabb.Size;
    }

    /// <summary>
    /// Assuming 'pointOnFace' is a point which resides on one of the OBB's faces,
    /// the method will return the normal of this face.
    /// </summary>
    public Vector3 GetPointFaceNormal(Vector3 pointOnFace)
    {
      Vector3[] normalizedAxes = Matrix4x4.TRS(Vector3.zero, this._rotation, Vector3.one).GetNormalizedAxes();
      Vector3 extents = this.Extents;
      Vector3 lhs = pointOnFace - this._center;
      float num1 = float.MaxValue;
      float num2 = -1f;
      Vector3 vector3 = Vector3.zero;
      for (int index = 0; index < 3; ++index)
      {
        Vector3 rhs = normalizedAxes[index];
        float f = Vector3.Dot(lhs, rhs);
        float num3 = Mathf.Abs(Mathf.Abs(f) - extents[index]);
        if ((double) num3 < (double) num1)
        {
          num1 = num3;
          vector3 = rhs;
          num2 = Mathf.Sign(f);
        }
      }
      return Vector3.Normalize(vector3 * num2);
    }

    public bool IntersectsOBB(OBB otherOBB)
    {
      return BoxMath.BoxIntersectsBox(this._center, this._size, this._rotation, otherOBB.Center, otherOBB.Size, otherOBB.Rotation);
    }

    public Vector3 GetClosestPoint(Vector3 point)
    {
      return BoxMath.CalcBoxPtClosestToPt(point, this._center, this._size, this._rotation);
    }

    public bool IntersectsSphere(Sphere sphere)
    {
      Vector3 closestPoint = this.GetClosestPoint(sphere.Center);
      return sphere.ContainsPoint(closestPoint);
    }
  }
}
