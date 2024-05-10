// Decompiled with JetBrains decompiler
// Type: RTG.QuadShape2D
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
  public class QuadShape2D : Shape2D
  {
    private Vector2 _center;
    private Vector2 _size;
    private float _rotationDegrees;
    private QuadEpsilon _epsilon;
    private Shape2DPtContainMode _ptContainMode;

    public float RotationDegrees
    {
      get => this._rotationDegrees;
      set => this._rotationDegrees = value % 360f;
    }

    public Quaternion Rotation => Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward);

    public Vector2 Center
    {
      get => this._center;
      set => this._center = value;
    }

    public Vector2 Size
    {
      get => this._size;
      set => this._size = value.Abs();
    }

    public Vector2 Extents => this._size * 0.5f;

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

    public QuadEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public Shape2DPtContainMode PtContainMode
    {
      get => this._ptContainMode;
      set => this._ptContainMode = value;
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

    public float WireEps
    {
      get => this._epsilon.WireEps;
      set => this._epsilon.WireEps = value;
    }

    public Vector2 Right
    {
      get
      {
        return (Vector2) (Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward) * (Vector3) QuadShape2D.ModelRight);
      }
    }

    public Vector2 Up
    {
      get
      {
        return (Vector2) (Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward) * (Vector3) QuadShape2D.ModelUp);
      }
    }

    public static Vector2 ModelRight => Vector2.right;

    public static Vector2 ModelUp => Vector2.up;

    public static Vector2 ModelCenter => Vector2.zero;

    public Vector2 GetExtentPoint(Shape2DExtentPoint extentPt)
    {
      Vector2 extents = this.Extents;
      switch (extentPt)
      {
        case Shape2DExtentPoint.Left:
          return this._center - this.Right * extents.x;
        case Shape2DExtentPoint.Top:
          return this._center + this.Up * extents.y;
        case Shape2DExtentPoint.Right:
          return this._center + this.Right * extents.x;
        case Shape2DExtentPoint.Bottom:
          return this._center - this.Up * extents.y;
        default:
          return Vector2.zero;
      }
    }

    public void AlignWidth(Vector2 axis)
    {
      this.RotationDegrees = (QuaternionEx.FromToRotation2D(this.Right, axis) * this.Rotation).ConvertTo2DRotation();
    }

    public float GetSizeAlongDirection(Vector2 direction)
    {
      return direction.AbsDot((Vector2) (this.Rotation * (Vector3) this._size));
    }

    public override void RenderArea(Camera camera)
    {
      GraphicsEx.DrawQuad2D(this._center, this._size, this._rotationDegrees, camera);
    }

    public override void RenderBorder(Camera camera)
    {
      GraphicsEx.DrawQuadBorder2D(this._center, this._size, this._rotationDegrees, camera);
    }

    public override bool ContainsPoint(Vector2 point)
    {
      return this._ptContainMode == Shape2DPtContainMode.InsideArea ? QuadMath.Contains2DPoint(point, this._center, this._size.x, this._size.y, this.Right, this.Up, this._epsilon) : QuadMath.Is2DPointOnBorder(point, this._center, this._size.x, this._size.y, this.Right, this.Up, this._epsilon);
    }

    public override Rect GetEncapsulatingRect()
    {
      return RectEx.FromPoints((IEnumerable<Vector2>) QuadMath.Calc2DQuadCornerPoints(this._center, this._size, this._rotationDegrees));
    }

    public QuadShape2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = QuadShape2D.ModelCenter;
      this._size = Vector2.one;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
