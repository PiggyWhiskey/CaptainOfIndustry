// Decompiled with JetBrains decompiler
// Type: RTG.ArcMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class ArcMath
  {
    public static float ConvertToSh3DArcAngle(
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart)
    {
      degreesFromStart %= 360f;
      if ((double) Mathf.Abs(degreesFromStart) > 180.0)
      {
        Vector3 from = arcStartPoint - arcOrigin;
        Vector3 normalized = (Quaternion.AngleAxis(degreesFromStart, arcPlaneNormal) * from).normalized;
        degreesFromStart = Vector3Ex.SignedAngle(from, normalized, arcPlaneNormal);
      }
      return degreesFromStart;
    }

    public static float ConvertToSh2DArcAngle(
      Vector2 arcOrigin,
      Vector2 arcStartPoint,
      float degreesFromStart)
    {
      degreesFromStart %= 360f;
      if ((double) Mathf.Abs(degreesFromStart) > 180.0)
      {
        Vector2 from = arcStartPoint - arcOrigin;
        Vector2 normalized = (Vector2) (Quaternion.AngleAxis(degreesFromStart, Vector3.forward) * (Vector3) from).normalized;
        degreesFromStart = Vector3Ex.SignedAngle((Vector3) from, (Vector3) normalized, Vector3.forward);
      }
      return degreesFromStart;
    }

    public static OBB CalcSh3DArcOBB(
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      degreesFromStart = ArcMath.ConvertToSh3DArcAngle(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart);
      Vector3 v1_1 = arcStartPoint - arcOrigin;
      Vector3 vector3_1 = Quaternion.AngleAxis(degreesFromStart * 0.5f, arcPlaneNormal) * v1_1;
      Vector3 vector3_2 = arcOrigin + vector3_1;
      Quaternion rotation = Quaternion.LookRotation(vector3_1.normalized, arcPlaneNormal);
      OBB obb = new OBB(arcOrigin + vector3_1 * 0.5f, rotation);
      Vector3 v1_2 = Quaternion.AngleAxis(degreesFromStart, arcPlaneNormal) * v1_1;
      float num = 2f * epsilon.AreaEps;
      float x = v1_2.AbsDot(obb.Right) + v1_1.AbsDot(obb.Right) + num;
      float z = vector3_2.magnitude + num;
      float y = epsilon.ExtrudeEps * 2f;
      obb.Size = new Vector3(x, y, z);
      return obb;
    }

    public static OBB CalcLg3DArcOBB(
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      if ((double) Math.Abs(degreesFromStart) <= 180.0)
        return ArcMath.CalcSh3DArcOBB(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon);
      Vector3 vector3_1 = arcStartPoint - arcOrigin;
      Vector3 vector3_2 = Quaternion.AngleAxis(degreesFromStart, arcPlaneNormal) * vector3_1;
      Vector3 vector3_3 = arcOrigin + vector3_2 - arcStartPoint;
      Vector3 vector3_4 = arcStartPoint + vector3_3 * 0.5f;
      Vector3 vector3_5 = vector3_4 - arcOrigin;
      Vector3 vector3_6 = vector3_4 + vector3_5.normalized * epsilon.AreaEps;
      float magnitude = (arcOrigin - arcStartPoint).magnitude;
      float z = (float) ((double) magnitude + (double) vector3_5.magnitude + 2.0 * (double) epsilon.AreaEps);
      Quaternion rotation = Quaternion.LookRotation(vector3_5.normalized, arcPlaneNormal);
      OBB obb = new OBB(vector3_6 - vector3_5.normalized * z * 0.5f, rotation);
      float x = (float) (((double) magnitude + (double) epsilon.AreaEps) * 2.0);
      float y = epsilon.ExtrudeEps * 2f;
      obb.Size = new Vector3(x, y, z);
      return obb;
    }

    public static bool RaycastShArc(
      Ray ray,
      out float t,
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      t = 0.0f;
      degreesFromStart = ArcMath.ConvertToSh3DArcAngle(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart);
      float enter;
      if (new Plane(arcPlaneNormal, arcOrigin).Raycast(ray, out enter) && ArcMath.ShArcContains3DPoint(ray.GetPoint(enter), false, arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon))
      {
        t = enter;
        return true;
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(arcPlaneNormal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      OBB obb = ArcMath.CalcSh3DArcOBB(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon);
      return BoxMath.Raycast(ray, obb.Center, obb.Size, obb.Rotation);
    }

    public static bool RaycastShArcWire(
      Ray ray,
      out float t,
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      t = 0.0f;
      degreesFromStart = ArcMath.ConvertToSh3DArcAngle(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart);
      float enter;
      if (new Plane(arcPlaneNormal, arcOrigin).Raycast(ray, out enter) && ArcMath.Is3DPointOnShArcWire(ray.GetPoint(enter), false, arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon))
      {
        t = enter;
        return true;
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(arcPlaneNormal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      OBB obb = ArcMath.CalcSh3DArcOBB(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon);
      return BoxMath.Raycast(ray, obb.Center, obb.Size, obb.Rotation);
    }

    public static bool RaycastLgArc(
      Ray ray,
      out float t,
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      t = 0.0f;
      float enter;
      if (new Plane(arcPlaneNormal, arcOrigin).Raycast(ray, out enter) && ArcMath.LgArcContains3DPoint(ray.GetPoint(enter), false, arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon))
      {
        t = enter;
        return true;
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(arcPlaneNormal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      OBB obb = ArcMath.CalcLg3DArcOBB(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon);
      return BoxMath.Raycast(ray, obb.Center, obb.Size, obb.Rotation);
    }

    public static bool RaycastLgArcWire(
      Ray ray,
      out float t,
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      t = 0.0f;
      float enter;
      if (new Plane(arcPlaneNormal, arcOrigin).Raycast(ray, out enter) && ArcMath.Is3DPointOnLgArcWire(ray.GetPoint(enter), false, arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon))
      {
        t = enter;
        return true;
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(arcPlaneNormal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      OBB obb = ArcMath.CalcLg3DArcOBB(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon);
      return BoxMath.Raycast(ray, obb.Center, obb.Size, obb.Rotation);
    }

    public static bool ShArcContains3DPoint(
      Vector3 point,
      bool checkOnPlane,
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      Vector3 from = arcStartPoint - arcOrigin;
      Vector3 to = point - arcOrigin;
      if ((double) ((arcOrigin - arcStartPoint).magnitude + epsilon.AreaEps) < (double) to.magnitude)
        return false;
      Plane plane = new Plane(arcPlaneNormal, arcOrigin);
      if (checkOnPlane && (double) plane.GetAbsDistanceToPoint(point) > (double) epsilon.ExtrudeEps)
        return false;
      float f = Vector3Ex.SignedAngle(from, to, arcPlaneNormal);
      if ((double) Mathf.Sign(f) == (double) Mathf.Sign(degreesFromStart) && (double) Mathf.Abs(f) <= (double) Mathf.Abs(degreesFromStart))
        return true;
      if ((double) epsilon.AreaEps != 0.0)
      {
        Vector3 point1 = Quaternion.AngleAxis(degreesFromStart, arcPlaneNormal) * from + arcOrigin;
        if ((double) point.GetDistanceToSegment(arcOrigin, arcStartPoint) <= (double) epsilon.AreaEps || (double) point.GetDistanceToSegment(arcOrigin, point1) <= (double) epsilon.AreaEps)
          return true;
      }
      return false;
    }

    public static bool Is3DPointOnShArcWire(
      Vector3 point,
      bool checkOnPlane,
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      Vector3 from = arcStartPoint - arcOrigin;
      Vector3 to = point - arcOrigin;
      float magnitude1 = to.magnitude;
      float magnitude2 = (arcOrigin - arcStartPoint).magnitude;
      Plane plane = new Plane(arcPlaneNormal, arcOrigin);
      if (checkOnPlane && (double) plane.GetAbsDistanceToPoint(point) > (double) epsilon.ExtrudeEps)
        return false;
      float f = Vector3Ex.SignedAngle(from, to, arcPlaneNormal);
      if ((double) Mathf.Sign(f) == (double) Mathf.Sign(degreesFromStart) && (double) Mathf.Abs(f) <= (double) Mathf.Abs(degreesFromStart) && (double) magnitude1 >= (double) magnitude2 - (double) epsilon.WireEps && (double) magnitude1 <= (double) magnitude2 + (double) epsilon.WireEps)
        return true;
      Vector3 point1 = Quaternion.AngleAxis(degreesFromStart, arcPlaneNormal) * from + arcOrigin;
      return (double) point.GetDistanceToSegment(arcOrigin, arcStartPoint) <= (double) epsilon.WireEps || (double) point.GetDistanceToSegment(arcOrigin, point1) <= (double) epsilon.WireEps;
    }

    public static bool ShArcContains2DPoint(
      Vector2 point,
      Vector2 arcOrigin,
      Vector2 arcStartPoint,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      degreesFromStart = ArcMath.ConvertToSh2DArcAngle(arcOrigin, arcStartPoint, degreesFromStart);
      epsilon.ExtrudeEps = 0.0f;
      return ArcMath.ShArcContains3DPoint(point.ToVector3(), false, arcOrigin.ToVector3(), arcStartPoint.ToVector3(), Vector3.forward, degreesFromStart, epsilon);
    }

    public static bool LgArcContains3DPoint(
      Vector3 point,
      bool checkOnPlane,
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      degreesFromStart %= 360f;
      if ((double) Mathf.Abs(degreesFromStart) <= 180.0)
        return ArcMath.ShArcContains3DPoint(point, checkOnPlane, arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon);
      Vector3 to = point - arcOrigin;
      Vector3 from = arcStartPoint - arcOrigin;
      degreesFromStart = ArcMath.ConvertToSh3DArcAngle(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart);
      float f = Vector3Ex.SignedAngle(from, to, arcPlaneNormal);
      if ((double) Mathf.Sign(f) == (double) Mathf.Sign(degreesFromStart) && (double) Mathf.Abs(f) <= (double) Mathf.Abs(degreesFromStart) && (double) epsilon.AreaEps != 0.0)
      {
        Vector3 point1 = Quaternion.AngleAxis(degreesFromStart, arcPlaneNormal) * from + arcOrigin;
        return (double) point.GetDistanceToSegment(arcOrigin, arcStartPoint) <= (double) epsilon.AreaEps || (double) point.GetDistanceToSegment(arcOrigin, point1) <= (double) epsilon.AreaEps;
      }
      float num = (arcOrigin - arcStartPoint).magnitude + epsilon.AreaEps;
      return (double) to.magnitude <= (double) num;
    }

    public static bool Is3DPointOnLgArcWire(
      Vector3 point,
      bool checkOnPlane,
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Vector3 arcPlaneNormal,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      if ((double) Mathf.Abs(degreesFromStart) <= 180.0)
        return ArcMath.Is3DPointOnShArcWire(point, checkOnPlane, arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart, epsilon);
      Vector3 from = arcStartPoint - arcOrigin;
      Vector3 to = point - arcOrigin;
      float magnitude1 = to.magnitude;
      float magnitude2 = (arcOrigin - arcStartPoint).magnitude;
      Plane plane = new Plane(arcPlaneNormal, arcOrigin);
      if (checkOnPlane && (double) plane.GetAbsDistanceToPoint(point) > (double) epsilon.ExtrudeEps)
        return false;
      Vector3 point1 = Quaternion.AngleAxis(degreesFromStart, arcPlaneNormal) * from + arcOrigin;
      if ((double) point.GetDistanceToSegment(arcOrigin, arcStartPoint) <= (double) epsilon.WireEps || (double) point.GetDistanceToSegment(arcOrigin, point1) <= (double) epsilon.WireEps)
        return true;
      degreesFromStart = ArcMath.ConvertToSh3DArcAngle(arcOrigin, arcStartPoint, arcPlaneNormal, degreesFromStart);
      float f = Vector3Ex.SignedAngle(from, to, arcPlaneNormal);
      return ((double) Mathf.Sign(f) != (double) Mathf.Sign(degreesFromStart) || (double) Mathf.Abs(f) > (double) Mathf.Abs(degreesFromStart)) && (double) magnitude1 >= (double) magnitude2 - (double) epsilon.WireEps && (double) magnitude1 <= (double) magnitude2 + (double) epsilon.WireEps;
    }

    public static bool LgArcContains2DPoint(
      Vector2 point,
      Vector2 arcOrigin,
      Vector2 arcStartPoint,
      float degreesFromStart,
      ArcEpsilon epsilon = default (ArcEpsilon))
    {
      epsilon.ExtrudeEps = 0.0f;
      return ArcMath.LgArcContains3DPoint(point.ToVector3(), false, arcOrigin.ToVector3(), arcStartPoint.ToVector3(), Vector3.forward, degreesFromStart, epsilon);
    }
  }
}
