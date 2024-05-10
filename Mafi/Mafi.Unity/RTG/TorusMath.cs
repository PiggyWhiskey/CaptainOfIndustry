// Decompiled with JetBrains decompiler
// Type: RTG.TorusMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class TorusMath
  {
    public static float CalcSphereRadius(float torusCoreRadius, float torusTubeRadius)
    {
      return torusCoreRadius + torusTubeRadius;
    }

    public static AABB CalcCylModelAABB(
      float torusCoreRadius,
      float torusHrzRadius,
      float torusVertRadius)
    {
      float num = (float) (((double) torusCoreRadius + (double) torusHrzRadius) * 2.0);
      return new AABB(Vector3.zero, new Vector3(num, torusVertRadius * 2f, num));
    }

    public static AABB CalcCylAABB(
      Vector3 torusCenter,
      float torusCoreRadius,
      float torusHrzRadius,
      float torusVertRadius,
      Quaternion torusRotation)
    {
      AABB aabb = TorusMath.CalcCylModelAABB(torusCoreRadius, torusHrzRadius, torusVertRadius);
      aabb.Transform(Matrix4x4.TRS(torusCenter, torusRotation, Vector3.one));
      return aabb;
    }

    public static List<Vector3> Calc3DHrzExtentPoints(
      Vector3 torusCenter,
      float torusCoreRadius,
      float torusTubeRadius,
      Quaternion torusRotation)
    {
      Vector3 vector3_1 = torusRotation * Vector3.right;
      Vector3 vector3_2 = torusRotation * Vector3.forward;
      float num = torusCoreRadius + torusTubeRadius;
      return new List<Vector3>()
      {
        torusCenter - vector3_1 * num,
        torusCenter + vector3_2 * num,
        torusCenter + vector3_1 * num,
        torusCenter - vector3_2 * num
      };
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 torusCenter,
      float torusCoreRadius,
      float torusTubeRadius,
      Quaternion torusRotation,
      TorusEpsilon epsilon = default (TorusEpsilon))
    {
      t = 0.0f;
      torusTubeRadius += epsilon.TubeRadiusEps;
      float cylinderRadius1 = torusCoreRadius + torusTubeRadius;
      Vector3 inNormal = torusRotation * Vector3.up;
      Vector3 cylinderAxisPt0 = torusCenter - inNormal * torusTubeRadius;
      Vector3 cylinderAxisPt1 = torusCenter + inNormal * torusTubeRadius;
      if (CylinderMath.Raycast(ray, out t, cylinderAxisPt0, cylinderAxisPt1, cylinderRadius1))
      {
        float num = torusCoreRadius - torusTubeRadius;
        Vector3 point = ray.GetPoint(t);
        Vector3 vector3 = new Plane(inNormal, torusCenter).ProjectPoint(point) - torusCenter;
        if ((double) vector3.magnitude >= (double) num)
          return true;
        float cylinderRadius2 = num;
        Ray ray1 = ray.Mirror(point);
        if (CylinderMath.RaycastNoCaps(ray1, out t, cylinderAxisPt0, cylinderAxisPt1, cylinderRadius2))
        {
          ref float local = ref t;
          vector3 = ray.origin - ray1.GetPoint(t);
          double magnitude = (double) vector3.magnitude;
          local = (float) magnitude;
          return true;
        }
      }
      return false;
    }

    public static bool RaycastCylindrical(
      Ray ray,
      out float t,
      Vector3 torusCenter,
      float torusCoreRadius,
      float torusHrzRadius,
      float torusVertRadius,
      Quaternion torusRotation,
      TorusEpsilon epsilon = default (TorusEpsilon))
    {
      t = 0.0f;
      torusHrzRadius += epsilon.CylHrzRadius;
      torusVertRadius += epsilon.CylVertRadius;
      float cylinderRadius1 = torusCoreRadius + torusHrzRadius;
      Vector3 inNormal = torusRotation * Vector3.up;
      Vector3 cylinderAxisPt0 = torusCenter - inNormal * torusVertRadius;
      Vector3 cylinderAxisPt1 = torusCenter + inNormal * torusVertRadius;
      if (CylinderMath.Raycast(ray, out t, cylinderAxisPt0, cylinderAxisPt1, cylinderRadius1))
      {
        float num = torusCoreRadius - torusHrzRadius;
        Vector3 point = ray.GetPoint(t);
        Vector3 vector3 = new Plane(inNormal, torusCenter).ProjectPoint(point) - torusCenter;
        if ((double) vector3.magnitude >= (double) num)
          return true;
        float cylinderRadius2 = num;
        Ray ray1 = ray.Mirror(point);
        if (CylinderMath.RaycastNoCaps(ray1, out t, cylinderAxisPt0, cylinderAxisPt1, cylinderRadius2))
        {
          ref float local = ref t;
          vector3 = ray.origin - ray1.GetPoint(t);
          double magnitude = (double) vector3.magnitude;
          local = (float) magnitude;
          return true;
        }
      }
      return false;
    }
  }
}
