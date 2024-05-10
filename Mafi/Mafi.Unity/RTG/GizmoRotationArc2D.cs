// Decompiled with JetBrains decompiler
// Type: RTG.GizmoRotationArc2D
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
  public class GizmoRotationArc2D
  {
    private ArcShape2D _arc;
    private GizmoRotationArc2D.ArcType _type;
    private PolygonShape2D _projectionPoly;
    private int _numProjectedPoints;

    public float RotationAngle
    {
      get => this._arc.DegreeAngleFromStart;
      set => this._arc.DegreeAngleFromStart = value;
    }

    public GizmoRotationArc2D.ArcType Type
    {
      get => this._type;
      set => this._type = value;
    }

    public PolygonShape2D ProjectionPoly
    {
      get => this._projectionPoly;
      set => this._projectionPoly = value;
    }

    public int NumProjectedPoints
    {
      get => this._numProjectedPoints;
      set => this._numProjectedPoints = Mathf.Max(3, value);
    }

    public void SetArcData(Vector2 arcOrigin, Vector2 arcStart, float radius)
    {
      this._arc.Origin = arcOrigin;
      this._arc.SetArcData(arcStart, radius);
    }

    public void Render(GizmoRotationArc2DLookAndFeel lookAndFeel, Camera camera)
    {
      if (this._type == GizmoRotationArc2D.ArcType.Standard || this._projectionPoly == null)
      {
        this._arc.ForceShortestArc = lookAndFeel.UseShortestRotation;
        if ((lookAndFeel.FillFlags & GizmoRotationArcFillFlags.Area) != GizmoRotationArcFillFlags.None)
        {
          GizmoSolidMaterial get = Singleton<GizmoSolidMaterial>.Get;
          get.ResetValuesToSensibleDefaults();
          get.SetCullModeOff();
          get.SetLit(false);
          get.SetColor(lookAndFeel.Color);
          get.SetPass(0);
          this._arc.RenderArea(camera);
        }
        ArcShape2D.BorderRenderFlags borderRenderFlags1 = ArcShape2D.BorderRenderFlags.None;
        if ((lookAndFeel.FillFlags & GizmoRotationArcFillFlags.ArcBorder) != GizmoRotationArcFillFlags.None)
          borderRenderFlags1 |= ArcShape2D.BorderRenderFlags.ArcBorder;
        if ((lookAndFeel.FillFlags & GizmoRotationArcFillFlags.ExtremitiesBorder) != GizmoRotationArcFillFlags.None)
        {
          ArcShape2D.BorderRenderFlags borderRenderFlags2 = borderRenderFlags1 | ArcShape2D.BorderRenderFlags.ExtremitiesBorder;
        }
        GizmoLineMaterial get1 = Singleton<GizmoLineMaterial>.Get;
        get1.ResetValuesToSensibleDefaults();
        get1.SetColor(lookAndFeel.BorderColor);
        get1.SetPass(0);
        this._arc.RenderBorder(camera);
      }
      else
      {
        if (this._type != GizmoRotationArc2D.ArcType.PolyProjected || this._projectionPoly == null)
          return;
        List<Vector2> vector2List = PrimitiveFactory.ProjectArcPointsOnPoly2DBorder(this._arc.Origin, PrimitiveFactory.Generate2DArcBorderPoints(this._arc.Origin, this._arc.StartPoint, this._arc.DegreeAngleFromStart, lookAndFeel.UseShortestRotation, this.NumProjectedPoints), this._projectionPoly.GetPoints());
        if ((lookAndFeel.FillFlags & GizmoRotationArcFillFlags.Area) != GizmoRotationArcFillFlags.None)
        {
          GizmoSolidMaterial get = Singleton<GizmoSolidMaterial>.Get;
          get.ResetValuesToSensibleDefaults();
          get.SetCullModeOff();
          get.SetLit(false);
          get.SetColor(lookAndFeel.Color);
          get.SetPass(0);
          GLRenderer.DrawTriangleFan2D(this._arc.Origin, vector2List, camera);
        }
        if (lookAndFeel.FillFlags == GizmoRotationArcFillFlags.None)
          return;
        GizmoLineMaterial get2 = Singleton<GizmoLineMaterial>.Get;
        get2.ResetValuesToSensibleDefaults();
        get2.SetColor(lookAndFeel.BorderColor);
        get2.SetPass(0);
        if ((lookAndFeel.FillFlags & GizmoRotationArcFillFlags.ArcBorder) != GizmoRotationArcFillFlags.None)
          GLRenderer.DrawLines2D(vector2List, camera);
        if ((lookAndFeel.FillFlags & GizmoRotationArcFillFlags.ExtremitiesBorder) == GizmoRotationArcFillFlags.None)
          return;
        GLRenderer.DrawLines2D(new List<Vector2>()
        {
          this._arc.Origin,
          vector2List[0],
          this._arc.Origin,
          vector2List[vector2List.Count - 1]
        }, camera);
      }
    }

    public GizmoRotationArc2D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._arc = new ArcShape2D();
      this._numProjectedPoints = 100;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum ArcType
    {
      Standard,
      PolyProjected,
    }
  }
}
