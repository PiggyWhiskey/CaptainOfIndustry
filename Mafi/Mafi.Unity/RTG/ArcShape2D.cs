// Decompiled with JetBrains decompiler
// Type: RTG.ArcShape2D
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
  public class ArcShape2D : Shape2D
  {
    private ArcShape2D.BorderRenderDescriptor _borderRenderDesc;
    private Rect _rect;
    private bool _forceShortestArc;
    private float _radius;
    private Vector2 _origin;
    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private List<Vector2> _borderPoints;
    private float _degreeAngleFromStart;
    private int _numBorderPoints;
    private bool _areBorderPointsDirty;
    private ArcEpsilon _epsilon;

    public float Radius
    {
      get => this._radius;
      set
      {
        this._radius = value;
        this._startPoint = this._origin + (this._startPoint - this._origin).normalized * this._radius;
        this.CalculateEndPoint();
        this._areBorderPointsDirty = true;
      }
    }

    public bool ForceShortestArc
    {
      get => this._forceShortestArc;
      set
      {
        this._forceShortestArc = value;
        this.CalculateEndPoint();
        this._areBorderPointsDirty = true;
      }
    }

    public float DegreeAngleFromStart
    {
      get => this._degreeAngleFromStart;
      set
      {
        this._degreeAngleFromStart = value % 360f;
        this.CalculateEndPoint();
        this._areBorderPointsDirty = true;
      }
    }

    public float AbsDegreeAngleFromStart => Mathf.Abs(this._degreeAngleFromStart);

    public Vector2 Origin
    {
      get => this._origin;
      set
      {
        Vector2 normalized = (this._startPoint - this._origin).normalized;
        this._origin = value;
        this._startPoint = this._origin + normalized * this._radius;
        this.CalculateEndPoint();
        this._areBorderPointsDirty = true;
      }
    }

    public int NumBorderPoints
    {
      get => this._numBorderPoints;
      set
      {
        this._numBorderPoints = Mathf.Max(3, value);
        this._areBorderPointsDirty = true;
      }
    }

    public Vector2 StartPoint => this._startPoint;

    public Vector2 EndPoint => this._endPoint;

    public ArcEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float AreaEps
    {
      get => this._epsilon.AreaEps;
      set => this._epsilon.AreaEps = value;
    }

    public ArcShape2D.BorderRenderDescriptor BorderRenderDesc => this._borderRenderDesc;

    public override void RenderArea(Camera camera)
    {
      if (this._areBorderPointsDirty)
        this.OnBorderPointsFoundDirty();
      GLRenderer.DrawTriangleFan2D(this._origin, this._borderPoints, camera);
    }

    public override void RenderBorder(Camera camera)
    {
      if (this._areBorderPointsDirty)
        this.OnBorderPointsFoundDirty();
      if ((this._borderRenderDesc.BorderFlags & ArcShape2D.BorderRenderFlags.ArcBorder) != ArcShape2D.BorderRenderFlags.None)
        GLRenderer.DrawLines2D(this._borderPoints, camera);
      if ((this._borderRenderDesc.BorderFlags & ArcShape2D.BorderRenderFlags.ExtremitiesBorder) == ArcShape2D.BorderRenderFlags.None)
        return;
      GLRenderer.DrawLines2D(new List<Vector2>()
      {
        this._origin,
        this.StartPoint,
        this._origin,
        this.EndPoint
      }, camera);
    }

    public void SetArcData(Vector2 startPoint, float radius)
    {
      this._startPoint = startPoint;
      this.Radius = radius;
    }

    public override Rect GetEncapsulatingRect()
    {
      if (this._areBorderPointsDirty)
        this.OnBorderPointsFoundDirty();
      return this._rect;
    }

    public override bool ContainsPoint(Vector2 point)
    {
      return this._forceShortestArc || (double) this.AbsDegreeAngleFromStart <= 180.0 ? ArcMath.ShArcContains2DPoint(point, this._origin, this._startPoint, this._degreeAngleFromStart, this._epsilon) : ArcMath.LgArcContains2DPoint(point, this._origin, this._startPoint, this._degreeAngleFromStart, this._epsilon);
    }

    private void OnBorderPointsFoundDirty()
    {
      this._borderPoints = PrimitiveFactory.Generate2DArcBorderPoints(this._origin, this._startPoint, this._degreeAngleFromStart, this._forceShortestArc, this._numBorderPoints);
      this._rect = RectEx.FromPoints((IEnumerable<Vector2>) this._borderPoints);
      this._areBorderPointsDirty = false;
    }

    private void CalculateEndPoint()
    {
      Vector3 vector3 = (Vector3) (this._startPoint - this._origin);
      this._endPoint = (Vector2) (this._origin.ToVector3() + Quaternion.AngleAxis(this.DegreeAngleFromStart, Vector3.forward) * vector3);
    }

    public ArcShape2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._borderRenderDesc = new ArcShape2D.BorderRenderDescriptor();
      this._numBorderPoints = 100;
      this._areBorderPointsDirty = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum BorderRenderFlags
    {
      None,
      ExtremitiesBorder,
      ArcBorder,
      All,
    }

    public class BorderRenderDescriptor
    {
      private ArcShape2D.BorderRenderFlags _borderFlags;

      public ArcShape2D.BorderRenderFlags BorderFlags
      {
        get => this._borderFlags;
        set => this._borderFlags = value;
      }

      public BorderRenderDescriptor()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._borderFlags = ArcShape2D.BorderRenderFlags.All;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
