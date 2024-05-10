// Decompiled with JetBrains decompiler
// Type: RTG.CircleShape3D
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
  public class CircleShape3D : Shape3D
  {
    private Vector3 _center;
    private float _radius;
    private Quaternion _rotation;
    private CircleEpsilon _epsilon;
    private Shape3DRaycastMode _raycastMode;

    public Vector3 Center
    {
      get => this._center;
      set => this._center = value;
    }

    public float Radius
    {
      get => this._radius;
      set => this._radius = Mathf.Abs(value);
    }

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public Vector3 Right => this._rotation * CircleShape3D.ModelRight;

    public Vector3 Up => this._rotation * CircleShape3D.ModelUp;

    public Vector3 Look => this._rotation * CircleShape3D.ModelLook;

    public Vector3 Normal => this.Look;

    public CircleEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float RadiusEps
    {
      get => this._epsilon.RadiusEps;
      set => this._epsilon.RadiusEps = value;
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

    public Shape3DRaycastMode RaycastMode
    {
      get => this._raycastMode;
      set => this._raycastMode = value;
    }

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelCenter => Vector3.zero;

    public static Vector3 ModelNormal => CircleShape3D.ModelLook;

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

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitCircleXY, Matrix4x4.TRS(this._center, this._rotation, Vector3Ex.FromValue(this._radius)));
    }

    public override void RenderWire()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(this._center, this._rotation, Vector3Ex.FromValue(this._radius)));
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return this._raycastMode == Shape3DRaycastMode.Solid ? CircleMath.Raycast(ray, out t, this._center, this._radius, this.Normal, this._epsilon) : CircleMath.RaycastWire(ray, out t, this._center, this._radius, this.Normal, this._epsilon);
    }

    public override bool RaycastWire(Ray ray, out float t)
    {
      return CircleMath.RaycastWire(ray, out t, this._center, this._radius, this.Normal, this._epsilon);
    }

    public bool ContainsPoint(Vector3 point, bool checkOnPlane)
    {
      return CircleMath.Contains3DPoint(point, checkOnPlane, this._center, this._radius, this.Normal, this._epsilon);
    }

    public List<Vector3> GetExtentPoints()
    {
      return CircleMath.Calc3DExtentPoints(this._center, this._radius, this._rotation);
    }

    public override AABB GetAABB() => new AABB((IEnumerable<Vector3>) this.GetExtentPoints());

    public CircleShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = CircleShape3D.ModelCenter;
      this._radius = 1f;
      this._rotation = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
