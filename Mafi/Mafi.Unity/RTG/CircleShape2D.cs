// Decompiled with JetBrains decompiler
// Type: RTG.CircleShape2D
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
  public class CircleShape2D : Shape2D
  {
    private Vector2 _center;
    private float _radius;
    private float _rotationDegrees;
    private int _numBorderPoints;
    private List<Vector2> _modelBorderPoints;
    private bool _areModelBorderPointsDirty;
    private CircleEpsilon _epsilon;
    private Shape2DPtContainMode _ptContainMode;

    public Vector2 Center
    {
      get => this._center;
      set => this._center = value;
    }

    public float Radius
    {
      get => this._radius;
      set => this._radius = Mathf.Abs(value);
    }

    public float RotationDegrees
    {
      get => this._rotationDegrees;
      set => this._rotationDegrees = value;
    }

    public Vector2 Right
    {
      get
      {
        return (Vector2) (Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward) * (Vector3) CircleShape2D.ModelRight);
      }
    }

    public Vector2 Up
    {
      get
      {
        return (Vector2) (Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward) * (Vector3) CircleShape2D.ModelUp);
      }
    }

    public CircleEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float RadiusEps
    {
      get => this._epsilon.RadiusEps;
      set => this._epsilon.RadiusEps = value;
    }

    public float WireEps
    {
      get => this._epsilon.WireEps;
      set => this._epsilon.WireEps = value;
    }

    public int NumBorderPoints
    {
      get => this._numBorderPoints;
      set
      {
        this._numBorderPoints = Mathf.Max(value, 4);
        this._areModelBorderPointsDirty = true;
      }
    }

    public Shape2DPtContainMode PtContainMode
    {
      get => this._ptContainMode;
      set => this._ptContainMode = value;
    }

    public static Vector2 ModelRight => Vector2.right;

    public static Vector2 ModelUp => Vector2.up;

    public static Vector2 ModelCenter => Vector2.zero;

    public Vector2 GetExtentPoint(Shape2DExtentPoint extentPt)
    {
      switch (extentPt)
      {
        case Shape2DExtentPoint.Left:
          return this._center - this.Right * this._radius;
        case Shape2DExtentPoint.Top:
          return this._center + this.Up * this._radius;
        case Shape2DExtentPoint.Right:
          return this._center + this.Right * this._radius;
        case Shape2DExtentPoint.Bottom:
          return this._center - this.Up * this._radius;
        default:
          return Vector2.zero;
      }
    }

    public override void RenderBorder(Camera camera)
    {
      if (this._areModelBorderPointsDirty)
        this.CalcModelBorderPoints();
      GLRenderer.DrawLines2D(this._modelBorderPoints, this._center, Vector2Ex.FromValue(this._radius), camera);
    }

    public override void RenderArea(Camera camera)
    {
      if (this._areModelBorderPointsDirty)
        this.CalcModelBorderPoints();
      GLRenderer.DrawTriangleFan2D(CircleShape2D.ModelCenter, this._modelBorderPoints, this._center, Vector2Ex.FromValue(this._radius), camera);
    }

    public override bool ContainsPoint(Vector2 point)
    {
      return this._ptContainMode == Shape2DPtContainMode.InsideArea ? CircleMath.Contains2DPoint(point, this._center, this._radius, this._epsilon) : CircleMath.Is2DPointOnBorder(point, this._center, this._radius, this._epsilon);
    }

    public List<Vector2> GetExtentPoints()
    {
      return CircleMath.Calc2DExtentPoints(this._center, this._radius, this._rotationDegrees);
    }

    public override Rect GetEncapsulatingRect()
    {
      return RectEx.FromPoints((IEnumerable<Vector2>) this.GetExtentPoints());
    }

    private void CalcModelBorderPoints()
    {
      this._modelBorderPoints = PrimitiveFactory.Generate2DCircleBorderPointsCW(Vector2.zero, 1f, this._numBorderPoints);
      this._areModelBorderPointsDirty = false;
    }

    public CircleShape2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._center = CircleShape2D.ModelCenter;
      this._radius = 1f;
      this._numBorderPoints = 100;
      this._modelBorderPoints = new List<Vector2>();
      this._areModelBorderPointsDirty = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
