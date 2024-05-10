// Decompiled with JetBrains decompiler
// Type: RTG.PyramidShape3D
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
  public class PyramidShape3D : Shape3D
  {
    private Vector3 _baseCenter;
    private float _baseWidth;
    private float _baseDepth;
    private float _height;
    private Quaternion _rotation;
    private PyramidEpsilon _epsilon;

    public Vector3 BaseCenter
    {
      get => this._baseCenter;
      set => this._baseCenter = value;
    }

    public Vector3 Tip
    {
      get => this._baseCenter + this.CentralAxis * this._height;
      set => this._baseCenter = value - this.CentralAxis * this._height;
    }

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public float BaseWidth
    {
      get => this._baseWidth;
      set => this._baseWidth = Mathf.Abs(value);
    }

    public float BaseDepth
    {
      get => this._baseDepth;
      set => this._baseDepth = Mathf.Abs(value);
    }

    public float Height
    {
      get => this._height;
      set => this._height = Mathf.Abs(value);
    }

    public PyramidEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float PtContainEps
    {
      get => this._epsilon.PtContainEps;
      set => this._epsilon.PtContainEps = value;
    }

    public Vector3 CentralAxis => this._rotation * PyramidShape3D.ModelUp;

    public Vector3 Right => this._rotation * PyramidShape3D.ModelRight;

    public Vector3 Up => this._rotation * PyramidShape3D.ModelUp;

    public Vector3 Look => this._rotation * PyramidShape3D.ModelLook;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelBaseCenter => Vector3.zero;

    public void PointTipAlongAxis(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.CentralAxis, axis, this.Right) * this._rotation;
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitPyramid, Matrix4x4.TRS(this._baseCenter, this._rotation, new Vector3(this._baseWidth, this._height, this._baseDepth)));
    }

    public override void RenderWire()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWirePyramid, Matrix4x4.TRS(this._baseCenter, this._rotation, new Vector3(this._baseWidth, this._height, this._baseDepth)));
    }

    public List<Vector3> GetBaseCornerPoints()
    {
      return PyramidMath.CalcBaseCornerPoints(this._baseCenter, this._baseWidth, this._baseDepth, this._rotation);
    }

    public override AABB GetAABB()
    {
      List<Vector3> baseCornerPoints = this.GetBaseCornerPoints();
      baseCornerPoints.Add(this.Tip);
      return new AABB((IEnumerable<Vector3>) baseCornerPoints);
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return PyramidMath.Raycast(ray, out t, this._baseCenter, this._baseWidth, this._baseDepth, this._height, this._rotation);
    }

    public bool ContainsPoint(Vector3 point)
    {
      return PyramidMath.ContainsPoint(point, this._baseCenter, this._baseWidth, this._baseDepth, this._height, this._rotation, this._epsilon);
    }

    public PyramidShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._baseCenter = PyramidShape3D.ModelBaseCenter;
      this._baseWidth = 1f;
      this._baseDepth = 1f;
      this._height = 1f;
      this._rotation = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
