// Decompiled with JetBrains decompiler
// Type: RTG.CylTorusShape3D
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
  public class CylTorusShape3D : Shape3D
  {
    private float _coreRadius;
    private float _hrzRadius;
    private float _vertRadius;
    private Vector3 _center;
    private Quaternion _rotation;
    private TorusEpsilon _epsilon;

    public float CoreRadius
    {
      get => this._coreRadius;
      set => this._coreRadius = Mathf.Abs(value);
    }

    public float HrzRadius
    {
      get => this._hrzRadius;
      set => this._hrzRadius = Mathf.Abs(value);
    }

    public float VertRadius
    {
      get => this._vertRadius;
      set => this._vertRadius = Mathf.Abs(value);
    }

    public Vector3 Bottom
    {
      get => this._center - this.Up * this.VertRadius;
      set => this._center = value + this.Up * this.VertRadius;
    }

    public Vector3 Top
    {
      get => this._center + this.Up * this.VertRadius;
      set => this._center = value - this.Up * this.VertRadius;
    }

    public Vector3 Center
    {
      get => this._center;
      set => this._center = value;
    }

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public Vector3 Right => this._rotation * CylTorusShape3D.ModelRight;

    public Vector3 Up => this._rotation * CylTorusShape3D.ModelUp;

    public Vector3 Look => this._rotation * CylTorusShape3D.ModelLook;

    public TorusEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float CylHrzRadiusEps
    {
      get => this._epsilon.CylHrzRadius;
      set => this._epsilon.CylHrzRadius = Mathf.Abs(value);
    }

    public float CylVertRadiusEps
    {
      get => this._epsilon.CylVertRadius;
      set => this._epsilon.CylVertRadius = Mathf.Abs(value);
    }

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelCenter => Vector3.zero;

    public override bool Raycast(Ray ray, out float t)
    {
      return TorusMath.RaycastCylindrical(ray, out t, this._center, this._coreRadius, this._hrzRadius, this._vertRadius, this._rotation, this._epsilon);
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitCylindricalTorus, Matrix4x4.TRS(this._center, this._rotation, Vector3.one));
    }

    public override void RenderWire()
    {
      Mesh unitWireCircleXy = Singleton<MeshPool>.Get.UnitWireCircleXY;
      Vector3 s = new Vector3(this._coreRadius + this._hrzRadius, this._coreRadius + this._hrzRadius, 1f);
      Quaternion quaternion = Quaternion.Euler(90f, 0.0f, 0.0f);
      Graphics.DrawMeshNow(unitWireCircleXy, Matrix4x4.TRS(this.Bottom, this._rotation * quaternion, s));
      Graphics.DrawMeshNow(unitWireCircleXy, Matrix4x4.TRS(this.Top, this._rotation * quaternion, s));
      s = new Vector3(this._coreRadius - this._hrzRadius, this._coreRadius - this._hrzRadius, 1f);
      Graphics.DrawMeshNow(unitWireCircleXy, Matrix4x4.TRS(this.Bottom, this._rotation * quaternion, s));
      Graphics.DrawMeshNow(unitWireCircleXy, Matrix4x4.TRS(this.Top, this._rotation * quaternion, s));
    }

    public List<Vector3> GetHrzExtents()
    {
      return TorusMath.Calc3DHrzExtentPoints(this._center, this._coreRadius, this._hrzRadius, this._rotation);
    }

    public override AABB GetAABB()
    {
      return TorusMath.CalcCylAABB(this._center, this._coreRadius, this._hrzRadius, this._vertRadius, this._rotation);
    }

    public CylTorusShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._coreRadius = 1f;
      this._hrzRadius = 1f;
      this._vertRadius = 1f;
      this._center = CylTorusShape3D.ModelCenter;
      this._rotation = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
