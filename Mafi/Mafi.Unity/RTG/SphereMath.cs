// Decompiled with JetBrains decompiler
// Type: RTG.SphereMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class SphereMath
  {
    public static List<Vector3> CalcRightUpExtents(
      Vector3 sphereCenter,
      float sphereRadius,
      Vector3 right,
      Vector3 up)
    {
      return new List<Vector3>(4)
      {
        sphereCenter - right * sphereRadius,
        sphereCenter + up * sphereRadius,
        sphereCenter + right * sphereRadius,
        sphereCenter - up * sphereRadius
      };
    }

    public static bool Raycast(
      Ray ray,
      Vector3 sphereCenter,
      float sphereRadius,
      SphereEpsilon epsilon = default (SphereEpsilon))
    {
      return SphereMath.Raycast(ray, out float _, sphereCenter, sphereRadius, epsilon);
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 sphereCenter,
      float sphereRadius,
      SphereEpsilon epsilon = default (SphereEpsilon))
    {
      t = 0.0f;
      sphereRadius += epsilon.RadiusEps;
      Vector3 vector3 = ray.origin - sphereCenter;
      float t1;
      float t2;
      if (!MathEx.SolveQuadratic(Vector3.SqrMagnitude(ray.direction), 2f * Vector3.Dot(ray.direction, vector3), Vector3.SqrMagnitude(vector3) - sphereRadius * sphereRadius, out t1, out t2) || (double) t1 < 0.0 && (double) t2 < 0.0)
        return false;
      if ((double) t1 < 0.0)
        t1 = t2;
      t = t1;
      return true;
    }

    public static bool Raycast(
      Ray ray,
      out float t0,
      out float t1,
      Vector3 sphereCenter,
      float sphereRadius,
      SphereEpsilon epsilon = default (SphereEpsilon))
    {
      t0 = t1 = 0.0f;
      sphereRadius += epsilon.RadiusEps;
      Vector3 vector3 = ray.origin - sphereCenter;
      if (!MathEx.SolveQuadratic(Vector3.SqrMagnitude(ray.direction), 2f * Vector3.Dot(ray.direction, vector3), Vector3.SqrMagnitude(vector3) - sphereRadius * sphereRadius, out t0, out t1) || (double) t0 < 0.0 && (double) t1 < 0.0)
        return false;
      if ((double) t0 > (double) t1)
      {
        float num = t0;
        t0 = t1;
        t1 = num;
      }
      if ((double) t0 < 0.0)
        t0 = t1;
      return true;
    }

    public static bool ContainsPoint(
      Vector3 point,
      Vector3 sphereCenter,
      float sphereRadius,
      SphereEpsilon epsilon = default (SphereEpsilon))
    {
      sphereRadius += epsilon.RadiusEps;
      return (double) (point - sphereCenter).sqrMagnitude <= (double) sphereRadius * (double) sphereRadius;
    }
  }
}
