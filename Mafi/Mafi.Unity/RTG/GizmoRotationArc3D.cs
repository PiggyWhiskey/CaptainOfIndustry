// Decompiled with JetBrains decompiler
// Type: RTG.GizmoRotationArc3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoRotationArc3D
  {
    private ArcShape3D _arc;

    public float RotationAngle
    {
      get => this._arc.DegreeAngleFromStart;
      set => this._arc.DegreeAngleFromStart = value;
    }

    public float Radius
    {
      get => this._arc.Radius;
      set => this._arc.Radius = value;
    }

    public void SetArcData(
      Vector3 rotationAxis,
      Vector3 arcOrigin,
      Vector3 arcStart,
      float radius)
    {
      this._arc.SetArcData(new Plane(rotationAxis, arcOrigin), arcOrigin, arcStart, radius);
    }

    public void Render(GizmoRotationArc3DLookAndFeel lookAndFeel)
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
        this._arc.RenderSolid();
      }
      ArcShape3D.WireRenderFlags wireRenderFlags1 = ArcShape3D.WireRenderFlags.None;
      if ((lookAndFeel.FillFlags & GizmoRotationArcFillFlags.ArcBorder) != GizmoRotationArcFillFlags.None)
        wireRenderFlags1 |= ArcShape3D.WireRenderFlags.ArcBorder;
      if ((lookAndFeel.FillFlags & GizmoRotationArcFillFlags.ExtremitiesBorder) != GizmoRotationArcFillFlags.None)
      {
        ArcShape3D.WireRenderFlags wireRenderFlags2 = wireRenderFlags1 | ArcShape3D.WireRenderFlags.ExtremitiesBorder;
      }
      GizmoLineMaterial get1 = Singleton<GizmoLineMaterial>.Get;
      get1.ResetValuesToSensibleDefaults();
      get1.SetColor(lookAndFeel.BorderColor);
      get1.SetPass(0);
      this._arc.RenderWire();
    }

    public GizmoRotationArc3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._arc = new ArcShape3D();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
