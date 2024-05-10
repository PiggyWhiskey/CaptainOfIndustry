// Decompiled with JetBrains decompiler
// Type: RTG.CameraViewVolume
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
  public class CameraViewVolume
  {
    private Vector3[] _worldPoints;
    private Plane[] _worldPlanes;
    private Vector2 _farPlaneSize;
    private Vector2 _nearPlaneSize;
    private AABB _worldAABB;
    private OBB _worldOBB;

    public Plane LeftPlane => this._worldPlanes[0];

    public Plane RightPlane => this._worldPlanes[1];

    public Plane BottomPlane => this._worldPlanes[2];

    public Plane TopPlane => this._worldPlanes[3];

    public Plane NearPlane => this._worldPlanes[4];

    public Plane FarPlane => this._worldPlanes[5];

    public Vector3 NearTopLeft => this._worldPoints[0];

    public Vector3 NearTopRight => this._worldPoints[1];

    public Vector3 NearBottomRight => this._worldPoints[2];

    public Vector3 NearBottomLeft => this._worldPoints[3];

    public Vector3 FarTopLeft => this._worldPoints[4];

    public Vector3 FarTopRight => this._worldPoints[5];

    public Vector3 FarBottomRight => this._worldPoints[6];

    public Vector3 FarBottomLeft => this._worldPoints[7];

    public Vector2 FarPlaneSize => this._farPlaneSize;

    public Vector2 NearPlaneSize => this._nearPlaneSize;

    public AABB WorldAABB => this._worldAABB;

    public OBB WorldOBB => this._worldOBB;

    public CameraViewVolume()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._worldPoints = new Vector3[8];
      this._worldPlanes = new Plane[6];
      this._farPlaneSize = Vector2.zero;
      this._nearPlaneSize = Vector2.zero;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public CameraViewVolume(Camera camera)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._worldPoints = new Vector3[8];
      this._worldPlanes = new Plane[6];
      this._farPlaneSize = Vector2.zero;
      this._nearPlaneSize = Vector2.zero;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FromCamera(camera);
    }

    public void FromCamera(Camera camera)
    {
      this._worldPlanes = GeometryUtility.CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix);
      this.CalculateWorldPoints(camera);
      this._farPlaneSize.x = (this.FarTopLeft - this.FarTopRight).magnitude;
      this._farPlaneSize.y = (this.FarTopLeft - this.FarBottomLeft).magnitude;
      this._nearPlaneSize.x = (this.NearTopLeft - this.NearTopRight).magnitude;
      this._nearPlaneSize.y = (this.NearTopLeft - this.NearBottomLeft).magnitude;
      this._worldAABB = new AABB((IEnumerable<Vector3>) this._worldPoints);
      Vector3 size = new Vector3();
      size.x = this._farPlaneSize.x;
      size.y = this._farPlaneSize.y;
      size.z = camera.farClipPlane - camera.nearClipPlane;
      Transform transform = camera.transform;
      this._worldOBB = new OBB(transform.position + transform.forward * (camera.nearClipPlane + size.z * 0.5f), size, transform.rotation);
    }

    /// <summary>
    /// Returns a list of all points which make up the near plane. The points
    /// are stored in the following order: top-left, top-right, bottom-right,
    /// bottom-left.
    /// </summary>
    public List<Vector3> GetNearPlanePoints()
    {
      return new List<Vector3>()
      {
        this.NearTopLeft,
        this.NearTopRight,
        this.NearBottomRight,
        this.NearBottomLeft
      };
    }

    public static Plane[] GetCameraWorldPlanes(Camera camera)
    {
      return GeometryUtility.CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix);
    }

    public bool CheckAABB(AABB aabb)
    {
      return GeometryUtility.TestPlanesAABB(this._worldPlanes, aabb.ToBounds());
    }

    public static bool CheckAABB(Camera camera, AABB aabb)
    {
      return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(camera.projectionMatrix * camera.worldToCameraMatrix), aabb.ToBounds());
    }

    public static bool CheckAABB(Camera camera, AABB aabb, Plane[] cameraWorldPlanes)
    {
      return GeometryUtility.TestPlanesAABB(cameraWorldPlanes, aabb.ToBounds());
    }

    /// <summary>
    /// Calculates the volume world space points for the specified camera.
    /// Must be called after the world planes have been calculated.
    /// </summary>
    private void CalculateWorldPoints(Camera camera)
    {
      Transform transform = camera.transform;
      Plane farPlane = this.FarPlane;
      Ray ray = new Ray(transform.position, -farPlane.normal);
      float enter;
      if (farPlane.Raycast(ray, out enter))
      {
        Vector3 point = ray.GetPoint(enter);
        Vector3 vector3_1 = Vector3.zero;
        Vector3 vector3_2 = Vector3.zero;
        ray = new Ray(point, transform.up);
        if (this.TopPlane.Raycast(ray, out enter))
          vector3_1 = ray.GetPoint(enter);
        ray = new Ray(point, transform.right);
        if (this.RightPlane.Raycast(ray, out enter))
          vector3_2 = ray.GetPoint(enter);
        float magnitude1 = (vector3_2 - point).magnitude;
        float magnitude2 = (vector3_1 - point).magnitude;
        this._worldPoints[4] = point - transform.right * magnitude1 + transform.up * magnitude2;
        this._worldPoints[5] = point + transform.right * magnitude1 + transform.up * magnitude2;
        this._worldPoints[6] = point + transform.right * magnitude1 - transform.up * magnitude2;
        this._worldPoints[7] = point - transform.right * magnitude1 - transform.up * magnitude2;
      }
      Plane nearPlane = this.NearPlane;
      Vector3 direction = (double) nearPlane.GetDistanceToPoint(transform.position) >= 0.0 ? -nearPlane.normal : nearPlane.normal;
      ray = new Ray(transform.position, direction);
      if (!nearPlane.Raycast(ray, out enter))
        return;
      Vector3 point1 = ray.GetPoint(enter);
      Vector3 vector3_3 = Vector3.zero;
      Vector3 vector3_4 = Vector3.zero;
      ray = new Ray(point1, transform.up);
      if (this.TopPlane.Raycast(ray, out enter))
        vector3_3 = ray.GetPoint(enter);
      ray = new Ray(point1, transform.right);
      if (this.RightPlane.Raycast(ray, out enter))
        vector3_4 = ray.GetPoint(enter);
      float magnitude3 = (vector3_4 - point1).magnitude;
      float magnitude4 = (vector3_3 - point1).magnitude;
      this._worldPoints[0] = point1 - transform.right * magnitude3 + transform.up * magnitude4;
      this._worldPoints[1] = point1 + transform.right * magnitude3 + transform.up * magnitude4;
      this._worldPoints[2] = point1 + transform.right * magnitude3 - transform.up * magnitude4;
      this._worldPoints[3] = point1 - transform.right * magnitude3 - transform.up * magnitude4;
    }

    public enum VPoint
    {
      NearTopLeft,
      NearTopRight,
      NearBottomRight,
      NearBottomLeft,
      FarTopLeft,
      FarTopRight,
      FarBottomRight,
      FarBottomLeft,
    }

    public enum VPlane
    {
      Left,
      Right,
      Bottom,
      Top,
      Near,
      Far,
    }
  }
}
