// Decompiled with JetBrains decompiler
// Type: RTG.CylinderShape3D
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
  public class CylinderShape3D : Shape3D
  {
    private Vector3 _baseCenter;
    private float _radius;
    private float _height;
    private Quaternion _rotation;
    private CylinderEpsilon _epsilon;

    public Vector3 BaseCenter
    {
      get => this._baseCenter;
      set => this._baseCenter = value;
    }

    public Vector3 TopCenter
    {
      get => this._baseCenter + this.CentralAxis * this._height;
      set => this.BaseCenter = value - this.CentralAxis * this._height;
    }

    public Vector3 Center
    {
      get => this._baseCenter + this.CentralAxis * this._height * 0.5f;
      set => this.BaseCenter = value - this.CentralAxis * this._height * 0.5f;
    }

    public float Radius
    {
      get => this._radius;
      set => this._radius = Mathf.Abs(value);
    }

    public float Height
    {
      get => this._height;
      set => this._height = Mathf.Abs(value);
    }

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public CylinderEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float RadiusEps
    {
      get => this._epsilon.RadiusEps;
      set => this._epsilon.RadiusEps = value;
    }

    public float VertEps
    {
      get => this._epsilon.VertEps;
      set => this._epsilon.VertEps = value;
    }

    public Vector3 CentralAxis => this._rotation * CylinderShape3D.ModelUp;

    public Vector3 Right => this._rotation * CylinderShape3D.ModelRight;

    public Vector3 Up => this._rotation * CylinderShape3D.ModelUp;

    public Vector3 Look => this._rotation * CylinderShape3D.ModelLook;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelBaseCenter => Vector3.zero;

    public void AlignCentralAxis(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.CentralAxis, axis, this.Look) * this._rotation;
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitCylinder, Matrix4x4.TRS(this._baseCenter, this._rotation, new Vector3(this._radius, this._height, this._radius)));
    }

    public override void RenderWire()
    {
      Vector3 s = new Vector3(this._radius, this._radius, 1f);
      Quaternion q = this._rotation * Quaternion.AngleAxis(90f, Vector3.right);
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(this._baseCenter, q, s));
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(this.TopCenter, q, s));
      List<Vector3> bottomCapExtentPoints = this.GetBottomCapExtentPoints();
      List<Vector3> topCapExtentPoints = this.GetTopCapExtentPoints();
      GLRenderer.DrawLinePairs3D(new List<Vector3>()
      {
        bottomCapExtentPoints[0],
        topCapExtentPoints[0],
        bottomCapExtentPoints[1],
        topCapExtentPoints[1],
        bottomCapExtentPoints[2],
        topCapExtentPoints[2],
        bottomCapExtentPoints[3],
        topCapExtentPoints[3]
      });
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return CylinderMath.Raycast(ray, out t, this._baseCenter, this.TopCenter, this._radius, this._height, this._epsilon);
    }

    public bool ContainsPoint(Vector3 point)
    {
      return CylinderMath.ContainsPoint(point, this._baseCenter, this.TopCenter, this._radius, this._height, this._epsilon);
    }

    public List<Vector3> GetBottomCapExtentPoints()
    {
      return CylinderMath.CalcExtentPoints(this._baseCenter, this._radius, this._rotation);
    }

    public List<Vector3> GetTopCapExtentPoints()
    {
      return CylinderMath.CalcExtentPoints(this.TopCenter, this._radius, this._rotation);
    }

    public AABB GetModelAABB()
    {
      float num = this._radius * 2f;
      return new AABB(CylinderShape3D.ModelBaseCenter + CylinderShape3D.ModelUp * this._height * 2f, new Vector3(num, this._height, num));
    }

    public override AABB GetAABB()
    {
      AABB modelAabb = this.GetModelAABB();
      modelAabb.Transform(Matrix4x4.TRS(this._baseCenter, this._rotation, Vector3.one));
      return modelAabb;
    }

    public CylinderShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._baseCenter = CylinderShape3D.ModelBaseCenter;
      this._radius = 1f;
      this._height = 1f;
      this._rotation = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
