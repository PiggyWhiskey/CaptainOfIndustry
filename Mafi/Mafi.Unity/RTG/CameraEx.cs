// Decompiled with JetBrains decompiler
// Type: RTG.CameraEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class CameraEx
  {
    private static List<GameObject> _objectBuffer;

    public static bool IsCurrent(this Camera camera) => (UnityEngine.Object) Camera.current == (UnityEngine.Object) camera;

    public static float GetFrustumDistanceFromHeight(this Camera camera, float frustumHeight)
    {
      return frustumHeight * 0.5f / Mathf.Tan((float) ((double) camera.fieldOfView * 0.5 * (Math.PI / 180.0)));
    }

    public static float GetFOVFromDistanceAndHeight(
      this Camera camera,
      float frustumHeight,
      float distance)
    {
      return (float) (2.0 * (double) Mathf.Atan2(frustumHeight * 0.5f, distance) * 57.295780181884766);
    }

    public static float GetFrustumWidthFromDistance(this Camera camera, float distance)
    {
      return 2f * distance * Mathf.Tan((float) ((double) camera.fieldOfView * 0.5 * (Math.PI / 180.0))) * camera.aspect;
    }

    public static float GetFrustumHeightFromDistance(this Camera camera, float distance)
    {
      return 2f * distance * Mathf.Tan((float) ((double) camera.fieldOfView * 0.5 * (Math.PI / 180.0)));
    }

    public static AABB CalculateVolumeAABB(this Camera camera)
    {
      return !camera.orthographic ? camera.CalculateFrustumAABB() : camera.CalculateOrthoAABB();
    }

    public static AABB CalculateFrustumAABB(this Camera camera)
    {
      Transform transform = camera.transform;
      float widthFromDistance = camera.GetFrustumWidthFromDistance(camera.farClipPlane);
      float heightFromDistance = camera.GetFrustumHeightFromDistance(camera.farClipPlane);
      Vector3 vector3_1 = transform.position + transform.forward * camera.farClipPlane - transform.right * widthFromDistance * 0.5f + transform.up * heightFromDistance * 0.5f;
      Vector3 vector3_2 = vector3_1 + transform.right * widthFromDistance;
      Vector3 vector3_3 = vector3_1 - transform.up * heightFromDistance;
      Vector3 vector3_4 = vector3_3 + transform.right * widthFromDistance;
      return new AABB((IEnumerable<Vector3>) new Vector3[5]
      {
        transform.position,
        vector3_1,
        vector3_2,
        vector3_3,
        vector3_4
      });
    }

    public static AABB CalculateOrthoAABB(this Camera camera)
    {
      float num1 = camera.orthographicSize * 2f;
      float num2 = num1 * camera.aspect;
      Transform transform = camera.transform;
      Vector3 vector3_1 = transform.position + transform.forward * camera.nearClipPlane - transform.right * num2 * 0.5f + transform.up * num1 * 0.5f;
      Vector3 vector3_2 = vector3_1 + transform.right * num2;
      Vector3 vector3_3 = vector3_2 - transform.up * num1;
      Vector3 vector3_4 = vector3_3 - transform.right * num2;
      Vector3 vector3_5 = transform.position + transform.forward * camera.farClipPlane - transform.right * num2 * 0.5f + transform.up * num1 * 0.5f;
      Vector3 vector3_6 = vector3_5 + transform.right * num2;
      Vector3 vector3_7 = vector3_6 - transform.up * num1;
      Vector3 vector3_8 = vector3_7 - transform.right * num2;
      return new AABB((IEnumerable<Vector3>) new Vector3[8]
      {
        vector3_1,
        vector3_2,
        vector3_3,
        vector3_4,
        vector3_5,
        vector3_6,
        vector3_7,
        vector3_8
      });
    }

    public static bool IsPointInFrontNearPlane(this Camera camera, Vector3 position)
    {
      return (double) camera.GetNearPlaneForward().GetDistanceToPoint(position) > 0.0;
    }

    public static Plane GetNearPlaneForward(this Camera camera)
    {
      Transform transform = camera.transform;
      return new Plane(transform.forward, transform.position + transform.forward * camera.nearClipPlane);
    }

    public static Vector3 GetFarMidPoint(this Camera camera)
    {
      Transform transform = camera.transform;
      return transform.position + transform.forward * camera.farClipPlane;
    }

    public static Vector3 GetFarMidOrthoTop(this Camera camera)
    {
      return camera.GetFarMidPoint() + camera.transform.up * camera.orthographicSize;
    }

    public static float GetOrthoFOV(this Camera camera)
    {
      Vector3 position = camera.transform.position;
      return Vector3.Angle(camera.GetFarMidPoint() - position, camera.GetFarMidOrthoTop() - position) * 2f;
    }

    public static bool IsPointFacingCamera(this Camera camera, Vector3 point, Vector3 pointNormal)
    {
      Vector3 lhs = point - camera.transform.position;
      if (camera.orthographic)
        lhs = camera.transform.forward;
      return (double) Vector3.Dot(lhs, pointNormal) < 0.0;
    }

    public static float GetPointZDistance(this Camera camera, Vector3 point)
    {
      Transform transform = camera.transform;
      return Vector3.Dot(point - transform.position, transform.forward);
    }

    public static List<Vector3> GetVisibleSphereExtents(this Camera camera, Sphere sphere)
    {
      Transform transform = camera.transform;
      List<Vector3> rightUpExtents = sphere.GetRightUpExtents(transform.right, transform.up);
      if (!camera.orthographic)
      {
        for (int index = 0; index < 4; ++index)
        {
          Vector3 vector3 = sphere.Center.ProjectOnSegment(transform.position, rightUpExtents[index]);
          rightUpExtents[index] = sphere.Center + (vector3 - sphere.Center).normalized * sphere.Radius;
        }
      }
      return rightUpExtents;
    }

    public static List<Vector2> ConvertWorldToScreenPoints(
      this Camera camera,
      List<Vector3> worldPoints)
    {
      if (worldPoints.Count == 0)
        return new List<Vector2>();
      List<Vector2> screenPoints = new List<Vector2>(worldPoints.Count);
      foreach (Vector3 worldPoint in worldPoints)
        screenPoints.Add((Vector2) camera.WorldToScreenPoint(worldPoint));
      return screenPoints;
    }

    public static float ScreenToEstimatedWorldSize(
      this Camera camera,
      Vector3 worldPos,
      float screenSize)
    {
      float pixelHeight = (float) camera.pixelHeight;
      if (camera.orthographic)
        return screenSize * (camera.orthographicSize * 2f / pixelHeight);
      Transform transform = camera.transform;
      float num = 2f * Vector3.Dot(transform.forward, worldPos - transform.position) * Mathf.Tan((float) ((double) camera.fieldOfView * 0.5 * (Math.PI / 180.0)));
      return screenSize * (num / pixelHeight);
    }

    public static float EstimateZoomFactor(this Camera camera, Vector3 worldPos)
    {
      if (camera.orthographic)
        return (float) ((double) camera.orthographicSize * 2.0 / (0.071000002324581146 * (double) camera.pixelHeight));
      Transform transform = camera.transform;
      return Vector3.Dot(transform.forward, worldPos - transform.position) / ((float) (23.0 / 500.0 * (double) camera.pixelHeight * 90.0) / camera.fieldOfView);
    }

    public static float EstimateZoomFactorSpherical(this Camera camera, Vector3 worldPos)
    {
      if (camera.orthographic)
        return (float) ((double) camera.orthographicSize * 2.0 / (0.071000002324581146 * (double) camera.pixelHeight));
      Transform transform = camera.transform;
      return (worldPos - transform.position).magnitude / ((float) (23.0 / 500.0 * (double) camera.pixelHeight * 90.0) / camera.fieldOfView);
    }

    public static void GetVisibleObjects(
      this Camera camera,
      CameraViewVolume viewVolume,
      List<GameObject> visibleObjects)
    {
      visibleObjects.Clear();
      MonoSingleton<RTScene>.Get.OverlapBox(viewVolume.WorldOBB, CameraEx._objectBuffer);
      if (CameraEx._objectBuffer.Count == 0)
        return;
      ObjectBounds.QueryConfig queryConfig = new ObjectBounds.QueryConfig();
      queryConfig.ObjectTypes = GameObjectTypeHelper.AllCombined;
      queryConfig.NoVolumeSize = Vector3Ex.FromValue(1E-05f);
      foreach (GameObject gameObject in CameraEx._objectBuffer)
      {
        AABB aabb = ObjectBounds.CalcWorldAABB(gameObject, queryConfig);
        if (aabb.IsValid && viewVolume.CheckAABB(aabb))
          visibleObjects.Add(gameObject);
      }
    }

    static CameraEx()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CameraEx._objectBuffer = new List<GameObject>();
    }
  }
}
