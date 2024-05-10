// Decompiled with JetBrains decompiler
// Type: RTG.TorusShape3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class TorusShape3D : Shape3D
  {
    private float _coreRadius;
    private float _tubeRadius;
    private Vector3 _center;
    private Quaternion _rotation;
    private TorusEpsilon _epsilon;
    private TorusShape3D.WireRenderDescriptor _wireRenderDesc;

    public float CoreRadius
    {
      get => this._coreRadius;
      set => this._coreRadius = Mathf.Abs(value);
    }

    public float TubeRadius
    {
      get => this._tubeRadius;
      set => this._tubeRadius = Mathf.Abs(value);
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

    public Vector3 Right => this._rotation * TorusShape3D.ModelRight;

    public Vector3 Up => this._rotation * TorusShape3D.ModelUp;

    public Vector3 Look => this._rotation * TorusShape3D.ModelLook;

    public TorusEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float TubeRadiusEps
    {
      get => this._epsilon.TubeRadiusEps;
      set => this._epsilon.TubeRadiusEps = value;
    }

    public TorusShape3D.WireRenderDescriptor WireRenderDesc => this._wireRenderDesc;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelCenter => Vector3.zero;

    public override bool Raycast(Ray ray, out float t)
    {
      return TorusMath.Raycast(ray, out t, this._center, this._coreRadius, this._tubeRadius, this._rotation, this._epsilon);
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitTorus, Matrix4x4.TRS(this._center, this._rotation, Vector3.one));
    }

    public override void RenderWire()
    {
      Mesh unitWireCircleXy = Singleton<MeshPool>.Get.UnitWireCircleXY;
      if (this._wireRenderDesc.NumTubeSlices != 0 && (this._wireRenderDesc.WireFlags & TorusShape3D.WireRenderFlags.TubeSlices) != TorusShape3D.WireRenderFlags.None)
      {
        Vector3 s = new Vector3(this._tubeRadius, this._tubeRadius, 1f);
        float num = 360f / (float) this._wireRenderDesc.NumTubeSlices;
        for (int index = 0; index < this._wireRenderDesc.NumTubeSlices; ++index)
        {
          float f = (float) ((double) num * (double) index * (Math.PI / 180.0));
          Vector3 pos = this._center + this.Right * Mathf.Cos(f) * this._coreRadius + this.Look * Mathf.Sin(f) * this._coreRadius;
          Vector3 normalized = Vector3.Cross(pos - this._center, this.Up).normalized;
          Graphics.DrawMeshNow(unitWireCircleXy, Matrix4x4.TRS(pos, Quaternion.LookRotation(normalized, this.Up), s));
        }
      }
      if (this._wireRenderDesc.NumAxialSlices == 0 || (this._wireRenderDesc.WireFlags & TorusShape3D.WireRenderFlags.AxialSlices) == TorusShape3D.WireRenderFlags.None)
        return;
      Quaternion q = this._rotation * Quaternion.Euler(90f, 0.0f, 0.0f);
      float num1 = 360f / (float) this._wireRenderDesc.NumAxialSlices;
      for (int index = 0; index < this._wireRenderDesc.NumAxialSlices; ++index)
      {
        float f = (float) ((double) num1 * (double) index * (Math.PI / 180.0));
        float num2 = Mathf.Cos(f);
        float num3 = this._coreRadius - this._tubeRadius * Mathf.Sin(f);
        float num4 = this._tubeRadius * num2;
        Vector3 s = new Vector3(num3, num3, 1f);
        Graphics.DrawMeshNow(unitWireCircleXy, Matrix4x4.TRS(this._center + this.Up * num4, q, s));
      }
    }

    public List<Vector3> GetHrzExtents()
    {
      return TorusMath.Calc3DHrzExtentPoints(this._center, this._coreRadius, this._tubeRadius, this._rotation);
    }

    public override AABB GetAABB()
    {
      return new AABB(this._center, Vector3Ex.FromValue(TorusMath.CalcSphereRadius(this._coreRadius, this._tubeRadius) * 2f));
    }

    public TorusShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._coreRadius = 1f;
      this._tubeRadius = 1f;
      this._center = TorusShape3D.ModelCenter;
      this._rotation = Quaternion.identity;
      this._wireRenderDesc = new TorusShape3D.WireRenderDescriptor();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum WireRenderFlags
    {
      None,
      TubeSlices,
      AxialSlices,
      All,
    }

    public class WireRenderDescriptor
    {
      private TorusShape3D.WireRenderFlags _wireFlags;
      private int _numTubeSlices;
      private int _numAxialSlices;

      public int NumTubeSlices
      {
        get => this._numTubeSlices;
        set => this._numTubeSlices = Mathf.Max(0, value);
      }

      public int NumAxialSlices
      {
        get => this._numAxialSlices;
        set => this._numAxialSlices = Mathf.Max(2, value);
      }

      public TorusShape3D.WireRenderFlags WireFlags
      {
        get => this._wireFlags;
        set => this._wireFlags = value;
      }

      public WireRenderDescriptor()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._wireFlags = TorusShape3D.WireRenderFlags.AxialSlices;
        this._numTubeSlices = 30;
        this._numAxialSlices = 30;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
