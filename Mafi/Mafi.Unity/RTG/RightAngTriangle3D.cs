// Decompiled with JetBrains decompiler
// Type: RTG.RightAngTriangle3D
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
  public class RightAngTriangle3D : Shape3D
  {
    private Vector3 _rightAngleCorner;
    private float _XLength;
    private float _YLength;
    private AxisSign _XLengthSign;
    private AxisSign _YLengthSign;
    private Quaternion _rotation;
    private TriangleEpsilon _epsilon;
    private Shape3DRaycastMode _raycastMode;

    public Vector3 RightAngleCorner
    {
      get => this._rightAngleCorner;
      set => this._rightAngleCorner = value;
    }

    public float XLength
    {
      get => this._XLength;
      set => this._XLength = Mathf.Abs(value);
    }

    public float YLength
    {
      get => this._YLength;
      set => this._YLength = Mathf.Abs(value);
    }

    public float RealXLength => this._XLength * (this._XLengthSign == AxisSign.Positive ? 1f : -1f);

    public float RealYLength => this._YLength * (this._YLengthSign == AxisSign.Positive ? 1f : -1f);

    public AxisSign XLengthSign
    {
      get => this._XLengthSign;
      set => this._XLengthSign = value;
    }

    public AxisSign YLengthSign
    {
      get => this._YLengthSign;
      set => this._YLengthSign = value;
    }

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public Vector3 Right => this._rotation * RightAngTriangle3D.ModelRight;

    public Vector3 Up => this._rotation * RightAngTriangle3D.ModelUp;

    public Vector3 Look => this._rotation * RightAngTriangle3D.ModelLook;

    public Vector3 Normal => this.Look;

    public Plane Plane => new Plane(this.Normal, this.RightAngleCorner);

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

    public Shape3DRaycastMode RaycastMode
    {
      get => this._raycastMode;
      set => this._raycastMode = value;
    }

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelRightAngleCorner => Vector3.zero;

    public static Vector3 ModelNormal => RightAngTriangle3D.ModelLook;

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

    public override bool Raycast(Ray ray, out float t)
    {
      List<Vector3> points = this.GetPoints();
      return this._raycastMode == Shape3DRaycastMode.Solid ? TriangleMath.Raycast(ray, out t, points[0], points[1], points[2], this._epsilon) : TriangleMath.RaycastWire(ray, out t, points[0], points[1], points[2], this._epsilon);
    }

    public override bool RaycastWire(Ray ray, out float t)
    {
      List<Vector3> points = this.GetPoints();
      return TriangleMath.RaycastWire(ray, out t, points[0], points[1], points[2], this._epsilon);
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitRightAngledTriangleXY, Matrix4x4.TRS(this._rightAngleCorner, this._rotation, new Vector3(this.RealXLength, this.RealYLength, 1f)));
    }

    public override void RenderWire()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireRightAngledTriangleXY, Matrix4x4.TRS(this._rightAngleCorner, this._rotation, new Vector3(this.RealXLength, this.RealYLength, 1f)));
    }

    public List<Vector3> GetPoints()
    {
      return TriangleMath.CalcRATriangle3DPoints(this._rightAngleCorner, this.RealXLength, this.RealYLength, this._rotation);
    }

    public override AABB GetAABB() => new AABB((IEnumerable<Vector3>) this.GetPoints());

    public bool ContainsPoint(Vector3 point, bool checkOnPlane)
    {
      List<Vector3> points = this.GetPoints();
      return TriangleMath.Contains3DPoint(point, checkOnPlane, points[0], points[1], points[2], this._epsilon);
    }

    public RightAngTriangle3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._rightAngleCorner = RightAngTriangle3D.ModelRightAngleCorner;
      this._XLength = 1f;
      this._YLength = 1f;
      this._rotation = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
