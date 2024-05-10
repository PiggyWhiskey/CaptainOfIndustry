// Decompiled with JetBrains decompiler
// Type: RTG.EqTriangle2D
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
  public class EqTriangle2D : Shape2D
  {
    private float _sideLength;
    private float _rotationDegrees;
    private TriangleEpsilon _epsilon;
    private Vector2[] _points;
    private Vector2 _centroid;
    private bool _arePointsDirty;

    public float SideLength
    {
      get => this._sideLength;
      set
      {
        this._sideLength = Mathf.Abs(value);
        this._arePointsDirty = true;
      }
    }

    public Vector2 Centroid
    {
      get => this._centroid;
      set
      {
        Vector2 vector2 = value - this._centroid;
        this._centroid = value;
        this._points[0] += vector2;
        this._points[1] += vector2;
        this._points[2] += vector2;
      }
    }

    public float Altitude => this._sideLength * TriangleMath.EqTriangleAltFactor;

    public float CentroidAltitude => this.Altitude / 3f;

    public float RotationDegrees
    {
      get => this._rotationDegrees;
      set => this._rotationDegrees = value;
    }

    public Quaternion Rotation => Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward);

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

    public Vector2 Right
    {
      get
      {
        return (Vector2) (Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward) * (Vector3) EqTriangle2D.ModelRight);
      }
    }

    public Vector2 Up
    {
      get
      {
        return (Vector2) (Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward) * (Vector3) EqTriangle2D.ModelUp);
      }
    }

    public static Vector2 ModelRight => Vector2.right;

    public static Vector2 ModelUp => Vector2.up;

    public static Vector2 ModelCentroid => Vector2.zero;

    public Vector2 GetPoint(EqTrianglePoint point)
    {
      if (this._arePointsDirty)
        this.OnPointsFoundDirty();
      return this._points[(int) point];
    }

    public void SetPoint(EqTrianglePoint point, Vector2 pointValue)
    {
      Vector2 vector2 = pointValue - this.GetPoint(point);
      this._points[0] += vector2;
      this._points[1] += vector2;
      this._points[2] += vector2;
    }

    public Vector2 GetEdgeMidPoint(EqTriangleEdge edge)
    {
      if (edge == EqTriangleEdge.LeftTop)
        return this.GetPoint(EqTrianglePoint.Left) + this.GetEdge(edge).normalized * 0.5f;
      return edge == EqTriangleEdge.TopRight ? this.GetPoint(EqTrianglePoint.Top) + this.GetEdge(edge).normalized * 0.5f : this.GetPoint(EqTrianglePoint.Right) + this.GetEdge(edge).normalized * 0.5f;
    }

    public Vector2 GetEdge(EqTriangleEdge edge)
    {
      if (edge == EqTriangleEdge.LeftTop)
        return this.GetPoint(EqTrianglePoint.Top) - this.GetPoint(EqTrianglePoint.Left);
      return edge == EqTriangleEdge.TopRight ? this.GetPoint(EqTrianglePoint.Right) - this.GetPoint(EqTrianglePoint.Top) : this.GetPoint(EqTrianglePoint.Left) - this.GetPoint(EqTrianglePoint.Right);
    }

    public override void RenderArea(Camera camera)
    {
      Vector2 point = this.GetPoint(EqTrianglePoint.Left);
      List<Vector2> points = new List<Vector2>();
      points.Add(this.GetPoint(EqTrianglePoint.Top));
      points.Add(this.GetPoint(EqTrianglePoint.Right));
      Camera camera1 = camera;
      GLRenderer.DrawTriangleFan2D(point, points, camera1);
    }

    public override void RenderBorder(Camera camera)
    {
      GLRenderer.DrawLineLoop2D(new List<Vector2>()
      {
        this.GetPoint(EqTrianglePoint.Left),
        this.GetPoint(EqTrianglePoint.Top),
        this.GetPoint(EqTrianglePoint.Right)
      }, camera);
    }

    public override bool ContainsPoint(Vector2 point)
    {
      return TriangleMath.Contains2DPoint(point, this.GetPoint(EqTrianglePoint.Left), this.GetPoint(EqTrianglePoint.Top), this.GetPoint(EqTrianglePoint.Right), this._epsilon);
    }

    public override Rect GetEncapsulatingRect()
    {
      if (this._arePointsDirty)
        this.OnPointsFoundDirty();
      return RectEx.FromPoints((IEnumerable<Vector2>) this._points);
    }

    private void OnPointsFoundDirty()
    {
      List<Vector2> vector2List = TriangleMath.CalcEqTriangle2DPoints(this._centroid, this._sideLength, this.Rotation);
      this._points[0] = vector2List[0];
      this._points[1] = vector2List[1];
      this._points[2] = vector2List[2];
      this._arePointsDirty = false;
    }

    public EqTriangle2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._sideLength = 1f;
      this._points = new Vector2[3];
      this._centroid = EqTriangle2D.ModelCentroid;
      this._arePointsDirty = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
