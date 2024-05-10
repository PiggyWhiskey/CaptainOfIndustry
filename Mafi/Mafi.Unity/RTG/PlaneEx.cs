// Decompiled with JetBrains decompiler
// Type: RTG.PlaneEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class PlaneEx
  {
    public static Plane InvertNormal(this Plane plane) => new Plane(-plane.normal, -plane.distance);

    public static float GetAbsDistanceToPoint(this Plane plane, Vector3 point)
    {
      return Mathf.Abs(plane.GetDistanceToPoint(point));
    }

    public static Vector3 ProjectPoint(this Plane plane, Vector3 pt)
    {
      float distanceToPoint = plane.GetDistanceToPoint(pt);
      return pt - distanceToPoint * plane.normal;
    }

    public static List<Vector3> ProjectAllPoints(this Plane plane, List<Vector3> points)
    {
      if (points.Count == 0)
        return new List<Vector3>();
      List<Vector3> vector3List = new List<Vector3>(points.Count);
      foreach (Vector3 point in points)
        vector3List.Add(plane.ProjectPoint(point));
      return vector3List;
    }

    public static int GetFurthestPtInFront(this Plane plane, List<Vector3> points)
    {
      int furthestPtInFront = -1;
      float num = float.MinValue;
      for (int index = 0; index < points.Count; ++index)
      {
        Vector3 point = points[index];
        float distanceToPoint = plane.GetDistanceToPoint(point);
        if ((double) distanceToPoint > 0.0 && (double) distanceToPoint > (double) num)
        {
          num = distanceToPoint;
          furthestPtInFront = index;
        }
      }
      return furthestPtInFront;
    }

    public static int GetClosestPtInFront(this Plane plane, List<Vector3> points)
    {
      int closestPtInFront = -1;
      float num = float.MaxValue;
      for (int index = 0; index < points.Count; ++index)
      {
        Vector3 point = points[index];
        float distanceToPoint = plane.GetDistanceToPoint(point);
        if ((double) distanceToPoint > 0.0 && (double) distanceToPoint < (double) num)
        {
          num = distanceToPoint;
          closestPtInFront = index;
        }
      }
      return closestPtInFront;
    }

    public static int GetClosestPtInFrontOrOnPlane(this Plane plane, List<Vector3> points)
    {
      int inFrontOrOnPlane = -1;
      float num = float.MaxValue;
      for (int index = 0; index < points.Count; ++index)
      {
        Vector3 point = points[index];
        float distanceToPoint = plane.GetDistanceToPoint(point);
        if ((double) distanceToPoint >= 0.0 && (double) distanceToPoint < (double) num || (double) Mathf.Abs(distanceToPoint) < 9.9999997473787516E-05)
        {
          num = distanceToPoint;
          inFrontOrOnPlane = index;
        }
      }
      return inFrontOrOnPlane;
    }

    public static int GetFurthestPtBehind(this Plane plane, List<Vector3> points)
    {
      int furthestPtBehind = -1;
      float num = float.MaxValue;
      for (int index = 0; index < points.Count; ++index)
      {
        Vector3 point = points[index];
        float distanceToPoint = plane.GetDistanceToPoint(point);
        if ((double) distanceToPoint < 0.0 && (double) distanceToPoint < (double) num)
        {
          num = distanceToPoint;
          furthestPtBehind = index;
        }
      }
      return furthestPtBehind;
    }

    public static Plane GetCameraFacingAxisSlicePlane(
      Vector3 axisOrigin,
      Vector3 axis,
      Camera camera)
    {
      Vector3 vector3 = camera.transform.forward;
      if (vector3.IsAligned(axis, false))
        vector3 = camera.transform.right;
      Vector3 lhs = Vector3.Normalize(Vector3.Cross(vector3, axis));
      return (double) lhs.magnitude < 9.9999997473787516E-05 ? new Plane() : new Plane(Vector3.Normalize(Vector3.Cross(lhs, axis)), axisOrigin);
    }
  }
}
