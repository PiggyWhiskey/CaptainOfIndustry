// Decompiled with JetBrains decompiler
// Type: RTG.QuadShape3D
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
  public class QuadShape3D : Shape3D
  {
    private Shape3DRaycastMode _raycastMode;
    private Vector3 _center;
    private Vector2 _size;
    private Quaternion _rotation;
    private QuadEpsilon _epsilon;
    private QuadShape3D.WireRenderDescriptor _wireRenderDesc;

    public Vector3 Center
    {
      get => this._center;
      set => this._center = value;
    }

    public Vector2 Size
    {
      get => this._size;
      set => this._size = value.Abs();
    }

    public float Width
    {
      get => this._size.x;
      set => this._size.x = Mathf.Abs(value);
    }

    public float Height
    {
      get => this._size.y;
      set => this._size.y = Mathf.Abs(value);
    }

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = value;
    }

    public Vector3 Right => this._rotation * QuadShape3D.ModelRight;

    public Vector3 Up => this._rotation * QuadShape3D.ModelUp;

    public Vector3 Look => this._rotation * QuadShape3D.ModelLook;

    public Vector3 Normal => this.Look;

    public QuadEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public Vector2 SizeEps
    {
      get => this._epsilon.SizeEps;
      set => this._epsilon.SizeEps = value;
    }

    public float WidthEps
    {
      get => this._epsilon.WidthEps;
      set => this._epsilon.WidthEps = value;
    }

    public float HeightEps
    {
      get => this._epsilon.HeightEps;
      set => this._epsilon.HeightEps = value;
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

    public QuadShape3D.WireRenderDescriptor WireRenderDesc => this._wireRenderDesc;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelCenter => Vector3.zero;

    public static Vector3 ModelNormal => QuadShape3D.ModelLook;

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

    public List<Vector3> GetCornerPoints()
    {
      return QuadMath.Calc3DQuadCornerPoints(this._center, this._size, this._rotation);
    }

    public Vector3 GetCornerPosition(QuadCorner quadCorner)
    {
      return QuadMath.Calc3DQuadCorner(this._center, this._size, this._rotation, quadCorner);
    }

    public void SetCornerPointPosition(QuadCorner quadCorner, Vector3 position)
    {
      Vector3 vector3 = this._center - this.GetCornerPosition(quadCorner);
      this.Center = position + vector3;
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitQuadXY, Matrix4x4.TRS(this._center, this._rotation, new Vector3(this._size.x, this._size.y, 1f)));
    }

    public override void RenderWire()
    {
      if (this._wireRenderDesc.WireEdgeFlags == QuadShape3D.WireEdgeFlags.All)
      {
        Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitWireQuadXY, Matrix4x4.TRS(this._center, this._rotation, new Vector3(this._size.x, this._size.y, 1f)));
      }
      else
      {
        List<Vector3> cornerPoints = this.GetCornerPoints();
        if ((this._wireRenderDesc.WireEdgeFlags & QuadShape3D.WireEdgeFlags.Top) != QuadShape3D.WireEdgeFlags.None)
          GLRenderer.DrawLine3D(cornerPoints[0], cornerPoints[1]);
        if ((this._wireRenderDesc.WireEdgeFlags & QuadShape3D.WireEdgeFlags.Right) != QuadShape3D.WireEdgeFlags.None)
          GLRenderer.DrawLine3D(cornerPoints[1], cornerPoints[2]);
        if ((this._wireRenderDesc.WireEdgeFlags & QuadShape3D.WireEdgeFlags.Bottom) != QuadShape3D.WireEdgeFlags.None)
          GLRenderer.DrawLine3D(cornerPoints[2], cornerPoints[3]);
        if ((this._wireRenderDesc.WireEdgeFlags & QuadShape3D.WireEdgeFlags.Left) == QuadShape3D.WireEdgeFlags.None)
          return;
        GLRenderer.DrawLine3D(cornerPoints[3], cornerPoints[0]);
      }
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return this._raycastMode == Shape3DRaycastMode.Solid ? QuadMath.Raycast(ray, out t, this._center, this._size.x, this._size.y, this.Right, this.Up, this._epsilon) : this.RaycastWire(ray, out t);
    }

    public override bool RaycastWire(Ray ray, out float t)
    {
      return QuadMath.RaycastWire(ray, out t, this._center, this._size.x, this._size.y, this.Right, this.Up, this._epsilon);
    }

    public bool ContainsPoint(Vector3 point, bool checkOnPlane)
    {
      return QuadMath.Contains3DPoint(point, checkOnPlane, this._center, this._size.x, this._size.y, this.Right, this.Up, this._epsilon);
    }

    public List<Vector3> GetCorners()
    {
      return QuadMath.Calc3DQuadCornerPoints(this._center, this._size, this._rotation);
    }

    public override AABB GetAABB() => new AABB((IEnumerable<Vector3>) this.GetCorners());

    public QuadShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = QuadShape3D.ModelCenter;
      this._size = Vector2.one;
      this._rotation = Quaternion.identity;
      this._wireRenderDesc = new QuadShape3D.WireRenderDescriptor();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    [Flags]
    public enum WireEdgeFlags
    {
      None = 0,
      Top = 1,
      Right = 2,
      Bottom = 4,
      Left = 8,
      All = Left | Bottom | Right | Top, // 0x0000000F
    }

    public class WireRenderDescriptor
    {
      private QuadShape3D.WireEdgeFlags _wireEdgeFlags;

      public QuadShape3D.WireEdgeFlags WireEdgeFlags
      {
        get => this._wireEdgeFlags;
        set => this._wireEdgeFlags = value;
      }

      public WireRenderDescriptor()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._wireEdgeFlags = QuadShape3D.WireEdgeFlags.All;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
