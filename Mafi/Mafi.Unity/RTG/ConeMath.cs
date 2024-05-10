// Decompiled with JetBrains decompiler
// Type: RTG.ConeMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class ConeMath
  {
    public static List<Vector3> CalcConeBaseExtentPoints(
      Vector3 coneBaseCenter,
      float coneBaseRadius,
      Quaternion coneRotation)
    {
      Vector3 vector3_1 = coneRotation * Vector3.right;
      Vector3 vector3_2 = coneRotation * Vector3.forward;
      return new List<Vector3>()
      {
        coneBaseCenter + vector3_1 * coneBaseRadius,
        coneBaseCenter - vector3_2 * coneBaseRadius,
        coneBaseCenter - vector3_1 * coneBaseRadius,
        coneBaseCenter + vector3_2 * coneBaseRadius
      };
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 coneBaseCenter,
      float coneBaseRadius,
      float coneHeight,
      Quaternion coneRotation,
      ConeEpsilon epsilon = default (ConeEpsilon))
    {
      t = 0.0f;
      Ray ray1 = ray.InverseTransform(Matrix4x4.TRS(coneBaseCenter, coneRotation, Vector3.one));
      float num1 = coneBaseRadius * 2f;
      Vector3 boxSize = new Vector3(num1, coneHeight + epsilon.VertEps * 2f, num1);
      if (!BoxMath.Raycast(ray1, Vector3.up * coneHeight * 0.5f, boxSize, Quaternion.identity))
        return false;
      float enter;
      if (new Plane(-Vector3.up, Vector3.zero).Raycast(ray1, out enter) && (double) (ray1.origin + ray1.direction * enter).magnitude <= (double) coneBaseRadius)
      {
        t = enter;
        return true;
      }
      float num2 = coneBaseRadius / coneHeight;
      float num3 = num2 * num2;
      float t1;
      float t2;
      if (!MathEx.SolveQuadratic((float) ((double) ray1.direction.x * (double) ray1.direction.x + (double) ray1.direction.z * (double) ray1.direction.z - (double) num3 * (double) ray1.direction.y * (double) ray1.direction.y), (float) (2.0 * ((double) ray1.origin.x * (double) ray1.direction.x + (double) ray1.origin.z * (double) ray1.direction.z - (double) num3 * (double) ray1.direction.y * ((double) ray1.origin.y - (double) coneHeight))), (float) ((double) ray1.origin.x * (double) ray1.origin.x + (double) ray1.origin.z * (double) ray1.origin.z - (double) num3 * ((double) ray1.origin.y - (double) coneHeight) * ((double) ray1.origin.y - (double) coneHeight)), out t1, out t2) || (double) t1 < 0.0 && (double) t2 < 0.0)
        return false;
      if ((double) t1 < 0.0)
        t1 = t2;
      t = t1;
      Vector3 vector3 = ray1.origin + ray1.direction * t;
      if ((double) vector3.y >= -(double) epsilon.VertEps && (double) vector3.y <= (double) coneHeight + (double) epsilon.VertEps)
        return true;
      t = 0.0f;
      return false;
    }

    public static bool ContainsPoint(
      Vector3 point,
      Vector3 coneBaseCenter,
      float coneBaseRadius,
      float coneHeight,
      Quaternion coneRotation,
      ConeEpsilon epsilon = default (ConeEpsilon))
    {
      point = Matrix4x4.TRS(coneBaseCenter, coneRotation, Vector3.one).inverse.MultiplyPoint(point);
      float num1 = Vector3.Dot(Vector3.up, point);
      if ((double) num1 < -(double) epsilon.VertEps || (double) num1 > (double) coneHeight + (double) epsilon.VertEps)
        return false;
      float num2 = coneHeight / coneBaseRadius;
      float num3 = num1 * num2;
      return (double) point.GetDistanceToSegment(Vector3.zero, Vector3.up * coneHeight) <= (double) num3 + (double) epsilon.HrzEps;
    }
  }
}
