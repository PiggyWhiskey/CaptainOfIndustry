// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Camera.PlanarMousePanHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Camera
{
  public struct PlanarMousePanHelper
  {
    private readonly UnityEngine.Camera m_camera;
    private readonly float m_maxDelta;
    private Plane? m_mousePanPlane;
    private Vector3 m_mousePanStartPoint;

    public PlanarMousePanHelper(UnityEngine.Camera camera, float maxDelta = 1000f)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_mousePanPlane = new Plane?();
      this.m_mousePanStartPoint = new Vector3();
      this.m_camera = camera;
      this.m_maxDelta = maxDelta;
    }

    public void StartPan(Plane plane)
    {
      this.m_mousePanPlane = new Plane?(plane);
      Ray ray = this.m_camera.ScreenPointToRay(Input.mousePosition);
      float enter;
      if (!plane.Raycast(ray, out enter))
        this.m_mousePanPlane = new Plane?();
      else
        this.m_mousePanStartPoint = ray.GetPoint(enter);
    }

    public bool ContinuePan(out Vector2 delta)
    {
      if (!this.m_mousePanPlane.HasValue)
      {
        Log.Warning("No pan started.");
        delta = Vector2.zero;
        return false;
      }
      Ray ray = this.m_camera.ScreenPointToRay(Input.mousePosition);
      float enter;
      if (!this.m_mousePanPlane.Value.Raycast(ray, out enter))
      {
        delta = Vector2.zero;
        return false;
      }
      Vector3 point = ray.GetPoint(enter);
      float x = (point.x - this.m_mousePanStartPoint.x).Clamp(-this.m_maxDelta, this.m_maxDelta);
      float y = (point.z - this.m_mousePanStartPoint.z).Clamp(-this.m_maxDelta, this.m_maxDelta);
      this.m_mousePanStartPoint = point;
      delta = new Vector2(x, y);
      return true;
    }

    public void StopPan() => this.m_mousePanPlane = new Plane?();
  }
}
