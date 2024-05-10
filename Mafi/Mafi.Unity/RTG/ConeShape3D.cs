// Decompiled with JetBrains decompiler
// Type: RTG.ConeShape3D
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
  public class ConeShape3D : Shape3D
  {
    private ConeShape3D.WireRenderDescriptor _wireRenderDesc;
    private Vector3 _baseCenter;
    private Quaternion _rotation;
    private float _baseRadius;
    private float _height;
    private ConeEpsilon _epsilon;

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

    public float BaseRadius
    {
      get => this._baseRadius;
      set => this._baseRadius = Mathf.Abs(value);
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

    public Vector3 CentralAxis => this.Up;

    public Vector3 Right => this._rotation * ConeShape3D.ModelRight;

    public Vector3 Up => this._rotation * ConeShape3D.ModelUp;

    public Vector3 Look => this._rotation * ConeShape3D.ModelLook;

    public ConeEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float HrzEps
    {
      get => this._epsilon.HrzEps;
      set => this._epsilon.HrzEps = value;
    }

    public float VertEps
    {
      get => this._epsilon.VertEps;
      set => this._epsilon.VertEps = value;
    }

    public ConeShape3D.WireRenderDescriptor WireRenderDesc => this._wireRenderDesc;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelBaseCenter => Vector3.zero;

    public void AlignTip(Vector3 axis)
    {
      this.Rotation = QuaternionEx.FromToRotation3D(this.CentralAxis, axis, this.Look) * this._rotation;
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitCone, Matrix4x4.TRS(this._baseCenter, this._rotation, new Vector3(this._baseRadius, this._height, this._baseRadius)));
    }

    public override void RenderWire()
    {
      Vector3 tip = this.Tip;
      if (this._wireRenderDesc.WireMode == ConeShape3D.WireRenderMode.Basic)
      {
        Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(this._baseCenter, this._rotation * Quaternion.AngleAxis(90f, Vector3.right), new Vector3(this._baseRadius, this._baseRadius, 1f)));
        List<Vector3> baseExtents = this.GetBaseExtents();
        GLRenderer.DrawLines3D(new List<Vector3>()
        {
          baseExtents[0],
          tip,
          baseExtents[1],
          tip,
          baseExtents[2],
          tip,
          baseExtents[3],
          tip
        });
      }
      else
      {
        Vector3 centralAxis = this.CentralAxis;
        Quaternion q = Quaternion.AngleAxis(90f, Vector3.right);
        float num1 = this._height / (float) (this._wireRenderDesc.NumDetailAxialRings - 1);
        float num2 = this._height / Mathf.Max(this._baseRadius, 1E-05f);
        for (int index = 0; index < this._wireRenderDesc.NumDetailAxialRings; ++index)
        {
          float num3 = num1 * (float) index;
          Vector3 pos = this._baseCenter + centralAxis * num1 * (float) index;
          float num4 = (this._height - num3) / num2;
          Vector3 s = new Vector3(num4, num4, 1f);
          Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(pos, q, s));
        }
        List<Vector3> linePoints = new List<Vector3>(this._wireRenderDesc.NumDetailAxialSegments * 2);
        float num5 = 360f / (float) this._wireRenderDesc.NumDetailAxialSegments;
        for (int index = 0; index < this._wireRenderDesc.NumDetailAxialSegments; ++index)
        {
          Vector3 vector3 = this._baseCenter + (Quaternion.AngleAxis((float) index * num5, centralAxis) * Vector3.right).normalized * this._baseRadius;
          linePoints.Add(vector3);
          linePoints.Add(tip);
        }
        GLRenderer.DrawLines3D(linePoints);
      }
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return ConeMath.Raycast(ray, out t, this._baseCenter, this._baseRadius, this._height, this._rotation);
    }

    public bool ContainsPoint(Vector3 point)
    {
      return ConeMath.ContainsPoint(point, this._baseCenter, this._baseRadius, this._height, this._rotation);
    }

    public List<Vector3> GetBaseExtents()
    {
      return ConeMath.CalcConeBaseExtentPoints(this._baseCenter, this._baseRadius, this._rotation);
    }

    public override AABB GetAABB()
    {
      AABB aabb = new AABB((IEnumerable<Vector3>) this.GetBaseExtents());
      aabb.Encapsulate(this.Tip);
      return aabb;
    }

    public ConeShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._wireRenderDesc = new ConeShape3D.WireRenderDescriptor();
      this._baseCenter = ConeShape3D.ModelBaseCenter;
      this._rotation = Quaternion.identity;
      this._baseRadius = 1f;
      this._height = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum WireRenderMode
    {
      Basic,
      Detailed,
    }

    public class WireRenderDescriptor
    {
      private ConeShape3D.WireRenderMode _wireMode;
      private int _numDetailAxialRings;
      private int _numDetailAxialSegments;

      public ConeShape3D.WireRenderMode WireMode
      {
        get => this._wireMode;
        set => this._wireMode = value;
      }

      public int NumDetailAxialRings
      {
        get => this._numDetailAxialRings;
        set => this._numDetailAxialRings = Mathf.Max(2, value);
      }

      public int NumDetailAxialSegments
      {
        get => this._numDetailAxialSegments;
        set => this._numDetailAxialSegments = Mathf.Max(2, value);
      }

      public WireRenderDescriptor()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._numDetailAxialRings = 20;
        this._numDetailAxialSegments = 20;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
