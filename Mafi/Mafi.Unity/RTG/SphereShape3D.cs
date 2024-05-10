// Decompiled with JetBrains decompiler
// Type: RTG.SphereShape3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SphereShape3D : Shape3D
  {
    private float _radius;
    private Vector3 _center;
    private Quaternion _rotation;
    private SphereEpsilon _epsilon;
    private SphereShape3D.WireRenderDescriptor _wireRenderDesc;

    public float Radius
    {
      get => this._radius;
      set => this._radius = Mathf.Abs(value);
    }

    public float WireRadius => this._wireRenderDesc.RadiusAdd + this._radius;

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

    public SphereEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float RadiusEps
    {
      get => this._epsilon.RadiusEps;
      set => this._epsilon.RadiusEps = value;
    }

    public SphereShape3D.WireRenderDescriptor WireRenderDesc => this._wireRenderDesc;

    public Vector3 CentralAxis => this.Up;

    public Vector3 Right => this._rotation * SphereShape3D.ModelRight;

    public Vector3 Up => this._rotation * SphereShape3D.ModelUp;

    public Vector3 Look => this._rotation * SphereShape3D.ModelLook;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelCenter => Vector3.zero;

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitSphere, Matrix4x4.TRS(this.Center, Quaternion.identity, Vector3Ex.FromValue(this.Radius)));
    }

    public override void RenderWire()
    {
      float wireRadius = this.WireRadius;
      if (this._wireRenderDesc.WireMode == SphereShape3D.WireRenderMode.Basic)
      {
        Vector3 s = new Vector3(wireRadius, wireRadius, 1f);
        Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(this._center, this._rotation, s));
        Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(this._center, this._rotation * Quaternion.Euler(90f, 0.0f, 0.0f), s));
        Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(this._center, this._rotation * Quaternion.Euler(0.0f, -90f, 0.0f), s));
      }
      else
      {
        if (this._wireRenderDesc.NumDetailSliceRings != 0)
        {
          Vector3 s = new Vector3(wireRadius, wireRadius, 1f);
          float num = 360f / (float) Mathf.Max(1, this._wireRenderDesc.NumDetailSliceRings - 1);
          for (int index = 0; index < this._wireRenderDesc.NumDetailSliceRings; ++index)
            Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(this._center, this._rotation * Quaternion.AngleAxis(num * (float) index, Vector3.up), s));
        }
        Quaternion q = this._rotation * Quaternion.AngleAxis(90f, Vector3.right);
        Vector3 vector3 = this._center + Vector3.up * wireRadius;
        float num1 = 2f * wireRadius / (float) this._wireRenderDesc.NumDetailAxialRings;
        for (int index = 0; index < this._wireRenderDesc.NumDetailAxialRings; ++index)
        {
          Vector3 pos = vector3 - Vector3.up * num1 * (float) index;
          float num2 = Mathf.Sqrt(wireRadius * wireRadius - (pos - this._center).sqrMagnitude);
          Vector3 s = new Vector3(num2, num2, 1f);
          Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireCircleXY, Matrix4x4.TRS(pos, q, s));
        }
      }
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return SphereMath.Raycast(ray, out t, this._center, this._radius, this._epsilon);
    }

    public bool ContainsPoint(Vector3 point)
    {
      return SphereMath.ContainsPoint(point, this._center, this._radius, this._epsilon);
    }

    public override AABB GetAABB()
    {
      return new AABB(this._center, Vector3Ex.FromValue(this._radius * 2f));
    }

    public SphereShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._radius = 1f;
      this._center = SphereShape3D.ModelCenter;
      this._rotation = Quaternion.identity;
      this._wireRenderDesc = new SphereShape3D.WireRenderDescriptor();
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
      private SphereShape3D.WireRenderMode _wireMode;
      private int _numDetailAxialRings;
      private int _numDetailSliceRings;
      private float _radiusAdd;

      public SphereShape3D.WireRenderMode WireMode
      {
        get => this._wireMode;
        set => this._wireMode = value;
      }

      public int NumDetailAxialRings
      {
        get => this._numDetailAxialRings;
        set => this._numDetailAxialRings = Mathf.Max(2, value);
      }

      public int NumDetailSliceRings
      {
        get => this._numDetailSliceRings;
        set => this._numDetailSliceRings = Mathf.Max(0, value);
      }

      public float RadiusAdd
      {
        get => this._radiusAdd;
        set => this._radiusAdd = value;
      }

      public WireRenderDescriptor()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._numDetailAxialRings = 20;
        this._numDetailSliceRings = 20;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
