// Decompiled with JetBrains decompiler
// Type: RTG.RightAngTriangle2D
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
  public class RightAngTriangle2D : Shape2D
  {
    private Vector2 _rightAngleCorner;
    private float _XLength;
    private float _YLength;
    private float _rotationDegrees;
    private TriangleEpsilon _epsilon;

    public Vector2 RightAngleCorner
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

    public float RotationDegrees
    {
      get => this._rotationDegrees;
      set => this._rotationDegrees = value;
    }

    public Vector2 Right
    {
      get
      {
        return (Vector2) (Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward) * (Vector3) RightAngTriangle2D.ModelRight);
      }
    }

    public Vector2 Up
    {
      get
      {
        return (Vector2) (Quaternion.AngleAxis(this._rotationDegrees, Vector3.forward) * (Vector3) RightAngTriangle2D.ModelUp);
      }
    }

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

    public static Vector2 ModelRight => Vector2.right;

    public static Vector2 ModelUp => Vector2.up;

    public static Vector2 ModelRightAngleCorner => Vector2.zero;

    public override void RenderArea(Camera camera)
    {
      List<Vector2> points = this.GetPoints();
      Vector2 origin = points[0];
      points.RemoveAt(0);
      GLRenderer.DrawTriangleFan2D(origin, points, camera);
    }

    public override void RenderBorder(Camera camera)
    {
      GLRenderer.DrawLineLoop2D(this.GetPoints(), camera);
    }

    public List<Vector2> GetPoints()
    {
      return TriangleMath.CalcRATriangle2DPoints(this._rightAngleCorner, this._XLength, this._YLength, this._rotationDegrees);
    }

    public override bool ContainsPoint(Vector2 point)
    {
      List<Vector2> points = this.GetPoints();
      return TriangleMath.Contains2DPoint(point, points[0], points[1], points[2], this._epsilon);
    }

    public override Rect GetEncapsulatingRect()
    {
      return RectEx.FromPoints((IEnumerable<Vector2>) this.GetPoints());
    }

    public RightAngTriangle2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._rightAngleCorner = RightAngTriangle2D.ModelRightAngleCorner;
      this._XLength = 1f;
      this._YLength = 1f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
