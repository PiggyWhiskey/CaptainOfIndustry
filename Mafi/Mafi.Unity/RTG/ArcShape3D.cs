// Decompiled with JetBrains decompiler
// Type: RTG.ArcShape3D
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
  public class ArcShape3D : Shape3D
  {
    private ArcShape3D.WireRenderDescriptor _wireRenderDesc;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private Vector3 _origin;
    private Plane _plane;
    private float _radius;
    private AABB _aabb;
    private float _degreeAngleFromStart;
    private bool _forceShortestArc;
    private List<Vector3> _borderPoints;
    private int _numBorderPoints;
    private bool _areBorderPointsDirty;
    private ArcEpsilon _epsilon;
    private Shape3DRaycastMode _raycastMode;

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

    public int NumBorderPoints
    {
      get => this._numBorderPoints;
      set
      {
        this._numBorderPoints = Mathf.Max(3, value);
        this._areBorderPointsDirty = true;
      }
    }

    public Vector3 Origin
    {
      get => this._origin;
      set
      {
        Vector3 normalized = (this._startPoint - this._origin).normalized;
        this._origin = value;
        this._startPoint = this._origin + normalized * this._radius;
        this.CalculateEndPoint();
        this._areBorderPointsDirty = true;
      }
    }

    public Vector3 StartPoint => this._startPoint;

    public Vector3 EndPoint => this._endPoint;

    public Plane Plane => this._plane;

    public Vector3 Normal => this._plane.normal;

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

    public ArcShape3D.WireRenderDescriptor WireRenderDesc => this._wireRenderDesc;

    public Shape3DRaycastMode RaycastMode
    {
      get => this._raycastMode;
      set => this._raycastMode = value;
    }

    public override void RenderSolid()
    {
      if (this._areBorderPointsDirty)
        this.OnBorderPointsFoundDirty();
      GLRenderer.DrawTriangleFan3D(this._origin, this._borderPoints);
    }

    public override void RenderWire()
    {
      if (this._areBorderPointsDirty)
        this.OnBorderPointsFoundDirty();
      if ((this._wireRenderDesc.WireFlags & ArcShape3D.WireRenderFlags.ArcBorder) != ArcShape3D.WireRenderFlags.None)
        GLRenderer.DrawLines3D(this._borderPoints);
      if ((this._wireRenderDesc.WireFlags & ArcShape3D.WireRenderFlags.ExtremitiesBorder) == ArcShape3D.WireRenderFlags.None)
        return;
      GLRenderer.DrawLines3D(new List<Vector3>()
      {
        this._origin,
        this.StartPoint,
        this._origin,
        this.EndPoint
      });
    }

    public void SetArcData(Plane plane, Vector3 origin, Vector3 startPoint, float radius)
    {
      this._plane = plane;
      this._origin = this._plane.ProjectPoint(origin);
      this._startPoint = this._plane.ProjectPoint(startPoint);
      this.Radius = radius;
    }

    public override bool Raycast(Ray ray, out float t)
    {
      return this._raycastMode == Shape3DRaycastMode.Solid ? (this._forceShortestArc || (double) this.AbsDegreeAngleFromStart <= 180.0 ? ArcMath.RaycastShArc(ray, out t, this._origin, this.StartPoint, this.Plane.normal, this.DegreeAngleFromStart, this._epsilon) : ArcMath.RaycastLgArc(ray, out t, this._origin, this.StartPoint, this.Plane.normal, this.DegreeAngleFromStart, this._epsilon)) : (this._forceShortestArc || (double) this.AbsDegreeAngleFromStart <= 180.0 ? ArcMath.RaycastShArcWire(ray, out t, this._origin, this.StartPoint, this.Plane.normal, this.DegreeAngleFromStart, this._epsilon) : ArcMath.RaycastLgArcWire(ray, out t, this._origin, this.StartPoint, this.Plane.normal, this.DegreeAngleFromStart, this._epsilon));
    }

    public override bool RaycastWire(Ray ray, out float t)
    {
      return this._forceShortestArc || (double) this.AbsDegreeAngleFromStart <= 180.0 ? ArcMath.RaycastShArcWire(ray, out t, this._origin, this.StartPoint, this.Plane.normal, this.DegreeAngleFromStart, this._epsilon) : ArcMath.RaycastLgArcWire(ray, out t, this._origin, this.StartPoint, this.Plane.normal, this.DegreeAngleFromStart, this._epsilon);
    }

    public bool ContainsPoint(Vector3 point, bool checkOnPlane)
    {
      return this._forceShortestArc || (double) this.AbsDegreeAngleFromStart <= 180.0 ? ArcMath.ShArcContains3DPoint(point, checkOnPlane, this._origin, this._startPoint, this._plane.normal, this._degreeAngleFromStart, this._epsilon) : ArcMath.LgArcContains3DPoint(point, checkOnPlane, this._origin, this._startPoint, this._plane.normal, this._degreeAngleFromStart, this._epsilon);
    }

    public override AABB GetAABB()
    {
      if (this._areBorderPointsDirty)
        this.OnBorderPointsFoundDirty();
      return this._aabb;
    }

    private void OnBorderPointsFoundDirty()
    {
      this._borderPoints = PrimitiveFactory.Generate3DArcBorderPoints(this._origin, this._startPoint, this._plane, this._degreeAngleFromStart, this._forceShortestArc, this._numBorderPoints);
      this._aabb = new AABB((IEnumerable<Vector3>) this._borderPoints);
      this._aabb.Encapsulate(this._origin);
      this._areBorderPointsDirty = false;
    }

    private void CalculateEndPoint()
    {
      Vector3 vector3 = this.StartPoint - this._origin;
      this._endPoint = this._origin + Quaternion.AngleAxis(this.DegreeAngleFromStart, this.Plane.normal) * vector3;
    }

    public ArcShape3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._wireRenderDesc = new ArcShape3D.WireRenderDescriptor();
      this._numBorderPoints = 100;
      this._areBorderPointsDirty = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum WireRenderFlags
    {
      None,
      ExtremitiesBorder,
      ArcBorder,
      All,
    }

    public class WireRenderDescriptor
    {
      private ArcShape3D.WireRenderFlags _wireFlags;

      public ArcShape3D.WireRenderFlags WireFlags
      {
        get => this._wireFlags;
        set => this._wireFlags = value;
      }

      public WireRenderDescriptor()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._wireFlags = ArcShape3D.WireRenderFlags.All;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
