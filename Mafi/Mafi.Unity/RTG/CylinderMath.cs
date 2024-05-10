// Decompiled with JetBrains decompiler
// Type: RTG.CylinderMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class CylinderMath
  {
    public static List<Vector3> CalcExtentPoints(
      Vector3 center,
      float cylinderRadius,
      Quaternion cylinderRotation)
    {
      Vector3 vector3_1 = cylinderRotation * Vector3.right;
      Vector3 vector3_2 = cylinderRotation * Vector3.forward;
      return new List<Vector3>()
      {
        center + vector3_1 * cylinderRadius,
        center - vector3_2 * cylinderRadius,
        center - vector3_1 * cylinderRadius,
        center + vector3_2 * cylinderRadius
      };
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 cylinderAxisPt0,
      Vector3 cylinderAxisPt1,
      float cylinderRadius,
      CylinderEpsilon epsilon = default (CylinderEpsilon))
    {
      return CylinderMath.Raycast(ray, out t, cylinderAxisPt0, cylinderAxisPt1, cylinderRadius, (cylinderAxisPt1 - cylinderAxisPt0).magnitude, epsilon);
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 cylinderAxisPt0,
      Vector3 cylinderAxisPt1,
      float cylinderRadius,
      float cylinderHeight,
      CylinderEpsilon epsilon = default (CylinderEpsilon))
    {
      t = 0.0f;
      cylinderRadius += epsilon.RadiusEps;
      Vector3 normalized = (cylinderAxisPt1 - cylinderAxisPt0).normalized;
      cylinderHeight += epsilon.VertEps * 2f;
      if ((double) cylinderHeight < 9.9999999747524271E-07)
        return false;
      cylinderAxisPt0 -= normalized * epsilon.VertEps;
      cylinderAxisPt1 += normalized * epsilon.VertEps;
      float enter1 = 0.0f;
      float enter2 = 0.0f;
      bool flag1 = false;
      bool flag2 = false;
      if (new Plane(normalized, cylinderAxisPt0).Raycast(ray, out enter1) && (double) (ray.GetPoint(enter1) - cylinderAxisPt0).sqrMagnitude <= (double) cylinderRadius * (double) cylinderRadius)
        flag1 = true;
      if (new Plane(normalized, cylinderAxisPt1).Raycast(ray, out enter2) && (double) (ray.GetPoint(enter2) - cylinderAxisPt1).sqrMagnitude <= (double) cylinderRadius * (double) cylinderRadius)
        flag2 = true;
      Vector3 lhs = Vector3.Cross(ray.direction, normalized);
      Vector3 rhs = Vector3.Cross(ray.origin - cylinderAxisPt0, normalized);
      float t1;
      float t2;
      if (!MathEx.SolveQuadratic(lhs.sqrMagnitude, 2f * Vector3.Dot(lhs, rhs), rhs.sqrMagnitude - cylinderRadius * cylinderRadius, out t1, out t2) || (double) t1 < 0.0 && (double) t2 < 0.0)
        return false;
      if ((double) t1 < 0.0)
        t1 = t2;
      t = t1;
      Vector3 vector3 = ray.origin + ray.direction * t;
      float num = Vector3.Dot(normalized, vector3 - cylinderAxisPt0);
      if ((double) num < 0.0)
      {
        if (flag2 | flag1)
        {
          t = float.MaxValue;
          if (flag2)
            t = enter2;
          if (flag1 && (double) t > (double) enter1)
            t = enter1;
        }
        else
        {
          t = 0.0f;
          return false;
        }
      }
      if ((double) num > (double) cylinderHeight)
      {
        if (flag2 | flag1)
        {
          t = float.MaxValue;
          if (flag2)
            t = enter2;
          if (flag1 && (double) t > (double) enter1)
            t = enter1;
        }
        else
        {
          t = 0.0f;
          return false;
        }
      }
      return true;
    }

    public static bool RaycastNoCaps(
      Ray ray,
      out float t,
      Vector3 cylinderAxisPt0,
      Vector3 cylinderAxisPt1,
      float cylinderRadius,
      CylinderEpsilon epsilon = default (CylinderEpsilon))
    {
      return CylinderMath.RaycastNoCaps(ray, out t, cylinderAxisPt0, cylinderAxisPt1, cylinderRadius, (cylinderAxisPt1 - cylinderAxisPt0).magnitude, epsilon);
    }

    public static bool RaycastNoCaps(
      Ray ray,
      out float t,
      Vector3 cylinderAxisPt0,
      Vector3 cylinderAxisPt1,
      float cylinderRadius,
      float cylinderHeight,
      CylinderEpsilon epsilon = default (CylinderEpsilon))
    {
      t = 0.0f;
      cylinderRadius += epsilon.RadiusEps;
      Vector3 normalized = (cylinderAxisPt1 - cylinderAxisPt0).normalized;
      cylinderHeight += epsilon.VertEps * 2f;
      if ((double) cylinderHeight < 9.9999999747524271E-07)
        return false;
      cylinderAxisPt0 -= normalized * epsilon.VertEps;
      cylinderAxisPt1 += normalized * epsilon.VertEps;
      Vector3 lhs = Vector3.Cross(ray.direction, normalized);
      Vector3 rhs = Vector3.Cross(ray.origin - cylinderAxisPt0, normalized);
      float t1;
      float t2;
      if (!MathEx.SolveQuadratic(lhs.sqrMagnitude, 2f * Vector3.Dot(lhs, rhs), rhs.sqrMagnitude - cylinderRadius * cylinderRadius, out t1, out t2) || (double) t1 < 0.0 && (double) t2 < 0.0)
        return false;
      if ((double) t1 < 0.0)
      {
        float num = t1;
        t1 = t2;
        t2 = num;
      }
      t = t1;
      Vector3 vector3 = ray.origin + ray.direction * t;
      float num1 = Vector3.Dot(normalized, vector3 - cylinderAxisPt0);
      if ((double) num1 < 0.0)
      {
        t = 0.0f;
        return false;
      }
      if ((double) num1 <= (double) cylinderHeight)
        return true;
      t = 0.0f;
      return false;
    }

    public static bool ContainsPoint(
      Vector3 point,
      Vector3 cylinderAxisPt0,
      Vector3 cylinderAxisPt1,
      float cylinderRadius,
      CylinderEpsilon epsilon = default (CylinderEpsilon))
    {
      return CylinderMath.ContainsPoint(point, cylinderAxisPt0, cylinderAxisPt1, cylinderRadius, (cylinderAxisPt1 - cylinderAxisPt0).magnitude, epsilon);
    }

    public static bool ContainsPoint(
      Vector3 point,
      Vector3 cylinderAxisPt0,
      Vector3 cylinderAxisPt1,
      float cylinderRadius,
      float cylinderHeight,
      CylinderEpsilon epsilon = default (CylinderEpsilon))
    {
      Vector3 normalized = (cylinderAxisPt1 - cylinderAxisPt0).normalized;
      Vector3 rhs = point - cylinderAxisPt0;
      float num = Vector3.Dot(normalized, rhs);
      return (double) num >= -(double) epsilon.VertEps && (double) num <= (double) cylinderHeight + (double) epsilon.VertEps && (double) (cylinderAxisPt0 + normalized * num - point).magnitude <= (double) cylinderRadius + (double) epsilon.RadiusEps;
    }
  }
}
