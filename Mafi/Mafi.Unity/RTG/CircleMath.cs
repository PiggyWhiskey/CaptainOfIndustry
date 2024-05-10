// Decompiled with JetBrains decompiler
// Type: RTG.CircleMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class CircleMath
  {
    public static List<Vector3> Calc3DExtentPoints(
      Vector3 circleCenter,
      float circleRadius,
      Quaternion circleRotation)
    {
      Vector3 vector3_1 = circleRotation * Vector3.right;
      Vector3 vector3_2 = circleRotation * Vector3.up;
      return new List<Vector3>()
      {
        circleCenter + vector3_2 * circleRadius,
        circleCenter + vector3_1 * circleRadius,
        circleCenter - vector3_2 * circleRadius,
        circleCenter - vector3_1 * circleRadius
      };
    }

    public static List<Vector2> Calc2DExtentPoints(
      Vector2 circleCenter,
      float circleRadius,
      float degreeCircleRotation)
    {
      Quaternion quaternion = Quaternion.AngleAxis(degreeCircleRotation, Vector3.forward);
      Vector2 vector2_1 = (Vector2) (quaternion * (Vector3) Vector2.right);
      Vector2 vector2_2 = (Vector2) (quaternion * (Vector3) Vector2.up);
      return new List<Vector2>()
      {
        circleCenter + vector2_2 * circleRadius,
        circleCenter + vector2_1 * circleRadius,
        circleCenter - vector2_2 * circleRadius,
        circleCenter - vector2_1 * circleRadius
      };
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 circleCenter,
      float circleRadius,
      Vector3 circleNormal,
      CircleEpsilon epsilon = default (CircleEpsilon))
    {
      t = 0.0f;
      circleRadius += epsilon.RadiusEps;
      float enter;
      if (new Plane(circleNormal, circleCenter).Raycast(ray, out enter) && (double) (ray.GetPoint(enter) - circleCenter).magnitude <= (double) circleRadius)
      {
        t = enter;
        return true;
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(circleNormal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      Vector3 cylinderAxisPt0 = circleCenter - circleNormal * epsilon.ExtrudeEps;
      Vector3 cylinderAxisPt1 = circleCenter + circleNormal * epsilon.ExtrudeEps;
      return CylinderMath.Raycast(ray, out t, cylinderAxisPt0, cylinderAxisPt1, circleRadius);
    }

    public static bool RaycastWire(
      Ray ray,
      out float t,
      Vector3 circleCenter,
      float circleRadius,
      Vector3 circleNormal,
      CircleEpsilon epsilon = default (CircleEpsilon))
    {
      t = 0.0f;
      float enter;
      if (new Plane(circleNormal, circleCenter).Raycast(ray, out enter))
      {
        Vector3 point = ray.GetPoint(enter);
        float magnitude = (circleCenter - point).magnitude;
        if ((double) magnitude >= (double) circleRadius - (double) epsilon.WireEps && (double) magnitude <= (double) circleRadius + (double) epsilon.WireEps)
        {
          t = enter;
          return true;
        }
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(circleNormal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      Vector3 cylinderAxisPt0 = circleCenter - circleNormal * epsilon.ExtrudeEps;
      Vector3 cylinderAxisPt1 = circleCenter + circleNormal * epsilon.ExtrudeEps;
      return CylinderMath.Raycast(ray, out t, cylinderAxisPt0, cylinderAxisPt1, circleRadius + epsilon.WireEps);
    }

    public static bool Contains3DPoint(
      Vector3 point,
      bool checkOnPlane,
      Vector3 circleCenter,
      float circleRadius,
      Vector3 circleNormal,
      CircleEpsilon epsilon = default (CircleEpsilon))
    {
      Plane plane = new Plane(circleNormal, circleCenter);
      if (checkOnPlane && (double) plane.GetAbsDistanceToPoint(point) > (double) epsilon.ExtrudeEps)
        return false;
      circleRadius += epsilon.RadiusEps;
      point = plane.ProjectPoint(point);
      return (double) (point - circleCenter).magnitude <= (double) circleRadius;
    }

    public static bool Contains2DPoint(
      Vector2 point,
      Vector2 circleCenter,
      float circleRadius,
      CircleEpsilon epsilon = default (CircleEpsilon))
    {
      circleRadius += epsilon.RadiusEps;
      return (double) (point - circleCenter).magnitude <= (double) circleRadius;
    }

    public static bool Is2DPointOnBorder(
      Vector2 point,
      Vector2 circleCenter,
      float circleRadius,
      CircleEpsilon epsilon = default (CircleEpsilon))
    {
      float magnitude = (point - circleCenter).magnitude;
      return (double) magnitude >= (double) circleRadius - (double) epsilon.WireEps && (double) magnitude <= (double) circleRadius + (double) epsilon.WireEps;
    }
  }
}
