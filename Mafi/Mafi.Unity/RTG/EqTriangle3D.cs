// Decompiled with JetBrains decompiler
// Type: RTG.EqTriangle3D
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
  public class EqTriangle3D : Shape3D
  {
    private float _sideLength;
    private Quaternion _rotation;
    private TriangleEpsilon _epsilon;
    private Vector3[] _points;
    private Vector3 _centroid;
    private bool _arePointsDirty;

    public float SideLength
    {
      get => this._sideLength;
      set
      {
        this._sideLength = Mathf.Abs(value);
        this._arePointsDirty = true;
      }
    }

    public Vector3 Centroid
    {
      get => this._centroid;
      set
      {
        Vector3 vector3 = value - this._centroid;
        this._centroid = value;
        this._points[0] += vector3;
        this._points[1] += vector3;
        this._points[2] += vector3;
      }
    }

    public float Altitude => this._sideLength * TriangleMath.EqTriangleAltFactor;

    public float CentroidAltitude => this.Altitude / 3f;

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public TriangleEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float AreaEps
    {
      get => this._epsilon.AreaEps;
      set => this._epsilon.AreaEps = value;
    }

    public float ExtrudeEps
    {
      get => this._epsilon.ExtrudeEps;
      set => this._epsilon.ExtrudeEps = value;
    }

    public float WireEps
    {
      get => this._epsilon.WireEps;
      set => this._epsilon.WireEps = value;
    }

    public Vector3 Normal => this.Look;

    public Vector3 Right => this._rotation * EqTriangle3D.ModelRight;

    public Vector3 Up => this._rotation * EqTriangle3D.ModelUp;

    public Vector3 Look => this._rotation * EqTriangle3D.ModelLook;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelCentroid => Vector3.zero;

    public void AlignNormal(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.Normal, axis, this.Right) * this._rotation;
    }

    public void AlignRight(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.Right, axis, this.Up) * this._rotation;
    }

    public void AlignUp(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.Up, axis, this.Normal) * this._rotation;
    }

    public Vector3 GetPoint(EqTrianglePoint point)
    {
      if (this._arePointsDirty)
        this.OnPointsFoundDirty();
      return this._points[(int) point];
    }

    public void SetPoint(EqTrianglePoint point, Vector3 pointValue)
    {
      Vector3 vector3 = pointValue - this.GetPoint(point);
      this._points[0] += vector3;
      this._points[1] += vector3;
      this._points[2] += vector3;
    }

    public Vector3 GetEdgeMidPoint(EqTriangleEdge edge)
    {
      if (edge == EqTriangleEdge.LeftTop)
        return this.GetPoint(EqTrianglePoint.Left) + this.GetEdge(edge).normalized * 0.5f;
      return edge == EqTriangleEdge.TopRight ? this.GetPoint(EqTrianglePoint.Top) + this.GetEdge(edge).normalized * 0.5f : this.GetPoint(EqTrianglePoint.Right) + this.GetEdge(edge).normalized * 0.5f;
    }

    public Vector3 GetEdge(EqTriangleEdge edge)
    {
      if (edge == EqTriangleEdge.LeftTop)
        return this.GetPoint(EqTrianglePoint.Top) - this.GetPoint(EqTrianglePoint.Left);
      return edge == EqTriangleEdge.TopRight ? this.GetPoint(EqTrianglePoint.Right) - this.GetPoint(EqTrianglePoint.Top) : this.GetPoint(EqTrianglePoint.Left) - this.GetPoint(EqTrianglePoint.Right);
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitEqTriangleXY, Matrix4x4.TRS(this._centroid, this._rotation, new Vector3(this._sideLength, this._sideLength, 1f)));
    }

    public override void RenderWire()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireEqTriangleXY, Matrix4x4.TRS(this._centroid, this._rotation, new Vector3(this._sideLength, this._sideLength, 1f)));
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return TriangleMath.Raycast(ray, out t, this.GetPoint(EqTrianglePoint.Left), this.GetPoint(EqTrianglePoint.Top), this.GetPoint(EqTrianglePoint.Right), this._epsilon);
    }

    public override bool RaycastWire(Ray ray, out float t)
    {
      return TriangleMath.RaycastWire(ray, out t, this.GetPoint(EqTrianglePoint.Left), this.GetPoint(EqTrianglePoint.Top), this.GetPoint(EqTrianglePoint.Right), this._epsilon);
    }

    public override AABB GetAABB()
    {
      if (this._arePointsDirty)
        this.OnPointsFoundDirty();
      return new AABB((IEnumerable<Vector3>) this._points);
    }

    private void OnPointsFoundDirty()
    {
      List<Vector3> vector3List = TriangleMath.CalcEqTriangle3DPoints(this._centroid, this._sideLength, this._rotation);
      this._points[0] = vector3List[0];
      this._points[1] = vector3List[1];
      this._points[2] = vector3List[2];
      this._arePointsDirty = false;
    }

    public EqTriangle3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._sideLength = 1f;
      this._rotation = Quaternion.identity;
      this._points = new Vector3[3];
      this._centroid = EqTriangle3D.ModelCentroid;
      this._arePointsDirty = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
