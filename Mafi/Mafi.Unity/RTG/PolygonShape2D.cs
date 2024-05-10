// Decompiled with JetBrains decompiler
// Type: RTG.PolygonShape2D
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
  public class PolygonShape2D : Shape2D
  {
    private Rect _rect;
    private bool _isRectDirty;
    private bool _isClosed;
    private List<Vector2> _cwPolyPoints;
    private List<Vector2> _thickCwBorderPoints;
    private bool _isThickBorderDirty;
    private PolygonEpsilon _epsilon;
    private Shape2DPtContainMode _ptContainMode;
    private PolygonShape2D.BorderRenderDescriptor _borderRenderDesc;

    public int NumPoints => this._cwPolyPoints.Count;

    public PolygonEpsilon Epsilon
    {
      get => this._epsilon;
      set => this._epsilon = value;
    }

    public float AreaEps
    {
      get => this._epsilon.AreaEps;
      set => this._epsilon.AreaEps = value;
    }

    public float WireEps
    {
      get => this._epsilon.WireEps;
      set => this._epsilon.WireEps = value;
    }

    public float ThickWireEps
    {
      get => this._epsilon.ThickWireEps;
      set => this._epsilon.ThickWireEps = value;
    }

    public bool IsClosed => this._isClosed;

    public Shape2DPtContainMode PtContainMode
    {
      get => this._ptContainMode;
      set => this._ptContainMode = value;
    }

    public PolygonShape2D.BorderRenderDescriptor BorderRenderDesc => this._borderRenderDesc;

    public Vector2 GetExtentPoint(Shape2DExtentPoint extentPt)
    {
      Rect encapsulatingRect = this.GetEncapsulatingRect();
      switch (extentPt)
      {
        case Shape2DExtentPoint.Left:
          return encapsulatingRect.center - Vector2.right * encapsulatingRect.width * 0.5f;
        case Shape2DExtentPoint.Top:
          return encapsulatingRect.center + Vector2.up * encapsulatingRect.height * 0.5f;
        case Shape2DExtentPoint.Right:
          return encapsulatingRect.center + Vector2.right * encapsulatingRect.width * 0.5f;
        case Shape2DExtentPoint.Bottom:
          return encapsulatingRect.center - Vector2.up * encapsulatingRect.height * 0.5f;
        default:
          return Vector2.zero;
      }
    }

    public override void RenderArea(Camera camera)
    {
      GLRenderer.DrawTriangleFan2D(this.GetEncapsulatingRect().center, this._cwPolyPoints, camera);
    }

    public override void RenderBorder(Camera camera)
    {
      if (this._borderRenderDesc.BorderType == Shape2DBorderType.Thin)
      {
        GLRenderer.DrawLines2D(this._cwPolyPoints, camera);
      }
      else
      {
        if (this._isThickBorderDirty)
          this.CalculateThickBorderPoints();
        if (this._borderRenderDesc.FillMode == PolygonShape2D.ThickBorderFillMode.Border)
        {
          GLRenderer.DrawLines2D(this._cwPolyPoints, camera);
          GLRenderer.DrawLines2D(this._thickCwBorderPoints, camera);
        }
        else
          GLRenderer.DrawQuads2D(PrimitiveFactory.Generate2DPolyBorderQuadsCW(this._cwPolyPoints, this._thickCwBorderPoints, this._borderRenderDesc.Direction == Shape2DBorderDirection.Inward ? PrimitiveFactory.PolyBorderDirection.Inward : PrimitiveFactory.PolyBorderDirection.Outward, this._isClosed), camera);
      }
    }

    public List<Vector2> GetPoints()
    {
      return new List<Vector2>((IEnumerable<Vector2>) this._cwPolyPoints);
    }

    public override Rect GetEncapsulatingRect()
    {
      if (this._isRectDirty)
        this.CalculateRect();
      return this._rect;
    }

    public void CopyPoints(PolygonShape2D sourcePoly)
    {
      this._isClosed = sourcePoly._isClosed;
      if (sourcePoly.NumPoints != 0)
        this._cwPolyPoints = new List<Vector2>((IEnumerable<Vector2>) sourcePoly._cwPolyPoints);
      else
        this._cwPolyPoints.Clear();
      this._isThickBorderDirty = true;
      this._isRectDirty = true;
    }

    public void SetClockwisePoints(List<Vector2> cwBorderPoints, bool isClosed)
    {
      int count = cwBorderPoints.Count;
      this._cwPolyPoints.Clear();
      for (int index = 0; index < count; ++index)
      {
        Vector2 cwBorderPoint1 = cwBorderPoints[index];
        Vector2 cwBorderPoint2 = cwBorderPoints[(index + 1) % count];
        this._cwPolyPoints.Add(cwBorderPoint1);
      }
      this._isRectDirty = true;
      this._isClosed = isClosed;
    }

    public void MakeSphereBorder(
      Vector3 sphereCenter,
      float sphereRadius,
      int numPoints,
      Camera camera)
    {
      List<Vector3> sphereBorderPoints = PrimitiveFactory.GenerateSphereBorderPoints(camera, sphereCenter, sphereRadius, numPoints);
      this.SetClockwisePoints(camera.ConvertWorldToScreenPoints(sphereBorderPoints), true);
    }

    public override bool ContainsPoint(Vector2 point)
    {
      if (this._borderRenderDesc.BorderType == Shape2DBorderType.Thin)
        return this._ptContainMode == Shape2DPtContainMode.InsideArea ? PolygonMath.Contains2DPoint(point, this._cwPolyPoints, this._isClosed, this._epsilon) : PolygonMath.Is2DPointOnBorder(point, this._cwPolyPoints, this._isClosed, this._epsilon);
      if (this._ptContainMode == Shape2DPtContainMode.InsideArea)
        return PolygonMath.Contains2DPoint(point, this._cwPolyPoints, this._isClosed, this._epsilon);
      if (this._isThickBorderDirty)
        this.CalculateThickBorderPoints();
      return PolygonMath.Is2DPointOnThickBorder(point, this._cwPolyPoints, this._thickCwBorderPoints, this._isClosed, this._epsilon);
    }

    private void CalculateRect()
    {
      this._rect = RectEx.FromPoints((IEnumerable<Vector2>) this._cwPolyPoints);
      this._isRectDirty = false;
    }

    private void CalculateThickBorderPoints()
    {
      this._thickCwBorderPoints = PrimitiveFactory.Generate2DPolyBorderPointsCW(this._cwPolyPoints, this._borderRenderDesc.Direction == Shape2DBorderDirection.Inward ? PrimitiveFactory.PolyBorderDirection.Inward : PrimitiveFactory.PolyBorderDirection.Outward, this._borderRenderDesc.Thickness, this._isClosed);
      this._isThickBorderDirty = false;
    }

    public PolygonShape2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isRectDirty = true;
      this._cwPolyPoints = new List<Vector2>(100);
      this._thickCwBorderPoints = new List<Vector2>(100);
      this._isThickBorderDirty = true;
      this._borderRenderDesc = new PolygonShape2D.BorderRenderDescriptor();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum ThickBorderFillMode
    {
      Filled,
      Border,
    }

    public class BorderRenderDescriptor
    {
      private Shape2DBorderType _borderType;
      private float _thickness;
      private Shape2DBorderDirection _direction;
      private PolygonShape2D.ThickBorderFillMode _fillMode;

      public Shape2DBorderType BorderType
      {
        get => this._borderType;
        set => this._borderType = value;
      }

      public float Thickness
      {
        get => this._thickness;
        set => this._thickness = Mathf.Abs(value);
      }

      public Shape2DBorderDirection Direction
      {
        get => this._direction;
        set => this._direction = value;
      }

      public PolygonShape2D.ThickBorderFillMode FillMode
      {
        get => this._fillMode;
        set => this._fillMode = value;
      }

      public BorderRenderDescriptor()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._thickness = 5f;
        this._direction = Shape2DBorderDirection.Outward;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
