// Decompiled with JetBrains decompiler
// Type: RTG.TriangPrismShape3D
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
  public class TriangPrismShape3D : Shape3D
  {
    private Vector3 _baseCenter;
    private float _width;
    private float _height;
    private float _depth;
    private Quaternion _rotation;
    private PrismEpsilon _epsilon;

    public Vector3 BaseCenter
    {
      get => this._baseCenter;
      set => this._baseCenter = value;
    }

    public Vector3 TopCenter
    {
      get => this._baseCenter + this.CentralAxis * this._height;
      set => this._baseCenter = value - this.CentralAxis * this._height;
    }

    public Vector3 FrontCenter
    {
      get => this.Center - this.Look * this._depth * 0.5f;
      set
      {
        this._baseCenter = value + 0.5f * (this.Look * this._depth - this.CentralAxis * this._height);
      }
    }

    public Vector3 Center
    {
      get => this._baseCenter + this.CentralAxis * this._height * 0.5f;
      set => this._baseCenter = value - this.CentralAxis * this._height * 0.5f;
    }

    public Vector3 MidTip
    {
      get => this.BaseCenter + 0.5f * (this.CentralAxis * this._height + this.Look * this._depth);
      set
      {
        this._baseCenter = value - 0.5f * (this.Look * this._depth - this.CentralAxis * this._height);
      }
    }

    public float Width
    {
      get => this._width;
      set => this._width = Mathf.Abs(value);
    }

    public float Height
    {
      get => this._height;
      set => this._height = Mathf.Abs(value);
    }

    public float Depth
    {
      get => this._depth;
      set => this._depth = Mathf.Abs(value);
    }

    public PrismEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float PtContainEps
    {
      get => this._epsilon.PtContainEps;
      set => this._epsilon.PtContainEps = value;
    }

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public Vector3 CentralAxis => this._rotation * TriangPrismShape3D.ModelUp;

    public Vector3 Right => this._rotation * TriangPrismShape3D.ModelRight;

    public Vector3 Up => this._rotation * TriangPrismShape3D.ModelUp;

    public Vector3 Look => this._rotation * TriangPrismShape3D.ModelLook;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelBaseCenter => Vector3.zero;

    public void AlignWidth(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.Right, axis, this.Up) * this._rotation;
    }

    public void AlignHeight(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.Up, axis, this.Right) * this._rotation;
    }

    public void AlignDepth(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.Look, axis, this.Right) * this._rotation;
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitTriangularPrism, Matrix4x4.TRS(this._baseCenter, this._rotation, new Vector3(this._width, this._height, this._depth)));
    }

    public override void RenderWire()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireTriangularPrism, Matrix4x4.TRS(this._baseCenter, this._rotation, new Vector3(this._width, this._height, this._depth)));
    }

    public void MakeEquilateral(float sideLength)
    {
      this._width = sideLength;
      float num = 0.5f * this._width;
      this._depth = Mathf.Sqrt((float) ((double) sideLength * (double) sideLength - (double) num * (double) num));
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return PrismMath.RaycastTriangular(ray, out t, this._baseCenter, this._width, this._depth, this._width, this._depth, this._height, this._rotation);
    }

    public bool ContainsPoint(Vector3 point)
    {
      return PrismMath.ContainsPoint(point, this._baseCenter, this._width, this._depth, this._width, this._depth, this._height, this._rotation, this._epsilon);
    }

    public override AABB GetAABB()
    {
      return new AABB((IEnumerable<Vector3>) PrismMath.CalcTriangPrismCornerPoints(this._baseCenter, this._width, this._depth, this._width, this._depth, this._height, this._rotation));
    }

    public TriangPrismShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._baseCenter = TriangPrismShape3D.ModelBaseCenter;
      this._width = 1f;
      this._height = 1f;
      this._depth = 1f;
      this._rotation = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
