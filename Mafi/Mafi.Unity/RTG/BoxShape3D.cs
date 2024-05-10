// Decompiled with JetBrains decompiler
// Type: RTG.BoxShape3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class BoxShape3D : Shape3D
  {
    private BoxShape3D.WireRenderDescriptor _wireRenderDesc;
    private Vector3 _size;
    private Vector3 _center;
    private Quaternion _rotation;
    private BoxEpsilon _epsilon;

    public Vector3 Size
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

    public float Depth
    {
      get => this._size.z;
      set => this._size.z = Mathf.Abs(value);
    }

    public Vector3 Extents => this._size * 0.5f;

    public Vector3 Center
    {
      get => this._center;
      set => this._center = value;
    }

    public BoxEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public Vector3 SizeEps
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

    public float DepthEps
    {
      get => this._epsilon.DepthEps;
      set => this._epsilon.DepthEps = value;
    }

    public Vector3 Min
    {
      get => this._center - this.Extents;
      set
      {
        Vector3 max = this.Max;
        this._center = (value + max) * 0.5f;
        this._size = max - value;
      }
    }

    public Vector3 Max
    {
      get => this._center + this.Extents;
      set
      {
        Vector3 min = this.Min;
        this._center = (value + min) * 0.5f;
        this._size = value - min;
      }
    }

    public Quaternion Rotation
    {
      get => this._rotation;
      set => this._rotation = QuaternionEx.Normalize(value);
    }

    public Vector3 Right => this._rotation * BoxShape3D.ModelRight;

    public Vector3 Up => this._rotation * BoxShape3D.ModelUp;

    public Vector3 Look => this._rotation * BoxShape3D.ModelLook;

    public BoxShape3D.WireRenderDescriptor WireRenderDesc => this._wireRenderDesc;

    public static Vector3 ModelRight => Vector3.right;

    public static Vector3 ModelUp => Vector3.up;

    public static Vector3 ModelLook => Vector3.forward;

    public static Vector3 ModelCenter => Vector3.zero;

    public void FromOBB(OBB obb)
    {
      this.Center = obb.Center;
      this.Size = obb.Size;
      this.Rotation = obb.Rotation;
    }

    public float GetSizeAlongDirection(Vector3 direction)
    {
      return direction.AbsDot(this._rotation * this._size);
    }

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

    public Vector3 GetFaceCenter(BoxFace boxFace)
    {
      return BoxMath.CalcBoxFaceCenter(this._center, this._size, this._rotation, boxFace);
    }

    public void SetFaceCenter(BoxFace boxFace, Vector3 newCenter)
    {
      Vector3 vector3 = BoxMath.CalcBoxFaceCenter(this._center, this._size, this._rotation, boxFace);
      this.Center = newCenter + (this._center - vector3);
    }

    public override void RenderSolid()
    {
      Graphics.DrawMeshNow(Singleton<MeshPool>.Get.UnitBox, Matrix4x4.TRS(this._center, this._rotation, this._size));
    }

    public override void RenderWire()
    {
      OBB box = new OBB(this.GetAABB(), this._rotation);
      if (this._wireRenderDesc.WireMode == BoxShape3D.WireRenderMode.Wire)
        GraphicsEx.DrawWireBox(box);
      else
        GraphicsEx.DrawWireCornerBox(box, this._wireRenderDesc.CornerLinePercentage);
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return BoxMath.Raycast(ray, out t, this._center, this._size, this._rotation, this._epsilon);
    }

    public override AABB GetAABB() => new AABB(this._center, this._size);

    public OBB GetOBB() => new OBB(this.GetAABB(), this._rotation);

    public bool ContainsPoint(Vector3 point)
    {
      return BoxMath.ContainsPoint(point, this._center, this._size, this._rotation, this._epsilon);
    }

    public BoxShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._wireRenderDesc = new BoxShape3D.WireRenderDescriptor();
      this._size = Vector3.one;
      this._center = BoxShape3D.ModelCenter;
      this._rotation = Quaternion.identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum WireRenderMode
    {
      Wire,
      WireCorners,
    }

    public class WireRenderDescriptor
    {
      private float _cornerLinePercentage;
      private BoxShape3D.WireRenderMode _wireMode;

      public BoxShape3D.WireRenderMode WireMode
      {
        get => this._wireMode;
        set => this._wireMode = value;
      }

      public float CornerLinePercentage
      {
        get => this._cornerLinePercentage;
        set => this._cornerLinePercentage = Mathf.Clamp(value, 0.0f, 1f);
      }

      public WireRenderDescriptor()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._cornerLinePercentage = 0.2f;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
