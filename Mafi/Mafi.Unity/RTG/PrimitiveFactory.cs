// Decompiled with JetBrains decompiler
// Type: RTG.PrimitiveFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class PrimitiveFactory
  {
    public static List<Vector2> Generate2DPolyBorderQuadsCW(
      List<Vector2> cwPolyPoints,
      List<Vector2> cwBorderPts,
      PrimitiveFactory.PolyBorderDirection borderDirection,
      bool isClosed)
    {
      if (cwPolyPoints.Count != cwBorderPts.Count)
        return new List<Vector2>();
      int num = cwPolyPoints.Count - 1;
      if (isClosed && num < 3)
        return new List<Vector2>();
      if (!isClosed && num < 2)
        return new List<Vector2>();
      List<Vector2> vector2List = new List<Vector2>(num * 4);
      if (borderDirection == PrimitiveFactory.PolyBorderDirection.Outward)
      {
        for (int index = 0; index < cwPolyPoints.Count - 1; ++index)
        {
          vector2List.Add(cwPolyPoints[index]);
          vector2List.Add(cwBorderPts[index]);
          vector2List.Add(cwBorderPts[index + 1]);
          vector2List.Add(cwPolyPoints[index + 1]);
        }
      }
      else
      {
        for (int index = 0; index < cwPolyPoints.Count - 1; ++index)
        {
          vector2List.Add(cwPolyPoints[index]);
          vector2List.Add(cwPolyPoints[index + 1]);
          vector2List.Add(cwBorderPts[index + 1]);
          vector2List.Add(cwBorderPts[index]);
        }
      }
      return vector2List;
    }

    public static float PolyBorderDirToSign(
      PrimitiveFactory.PolyBorderDirection borderDirection)
    {
      return borderDirection != PrimitiveFactory.PolyBorderDirection.Inward ? 1f : -1f;
    }

    public static List<Vector2> Generate2DPolyBorderPointsCW(
      List<Vector2> cwPolyPoints,
      PrimitiveFactory.PolyBorderDirection borderDirection,
      float borderThickness,
      bool isClosed)
    {
      int num1 = cwPolyPoints.Count - 1;
      if (isClosed && num1 < 3)
        return new List<Vector2>();
      if (!isClosed && num1 < 2)
        return new List<Vector2>();
      float num2 = borderThickness * PrimitiveFactory.PolyBorderDirToSign(borderDirection);
      List<Vector2> vector2List = new List<Vector2>(cwPolyPoints.Count);
      if (isClosed)
      {
        Vector2 cwPolyPoint1 = cwPolyPoints[0];
        Vector2 normal1 = (cwPolyPoints[1] - cwPolyPoint1).GetNormal();
        Vector2 cwPolyPoint2 = cwPolyPoints[cwPolyPoints.Count - 2];
        Vector2 cwPolyPoint3 = cwPolyPoints[cwPolyPoints.Count - 1];
        Vector2 normal2 = (cwPolyPoint3 - cwPolyPoint2).GetNormal();
        Vector2 normalized1 = (cwPolyPoint3 - cwPolyPoint2).normalized;
        Vector2 rayOrigin1 = cwPolyPoint2 + normal2 * num2;
        float t;
        if (PlaneMath.Raycast2D(rayOrigin1, normalized1, normal1, cwPolyPoint1 + normal1 * num2, out t))
          vector2List.Add(rayOrigin1 + normalized1 * t);
        else
          vector2List.Add(cwPolyPoint1);
        for (int index = 0; index < num1 - 1; ++index)
        {
          Vector2 cwPolyPoint4 = cwPolyPoints[index];
          Vector2 cwPolyPoint5 = cwPolyPoints[index + 1];
          Vector2 rayOrigin2 = vector2List[index];
          Vector2 cwPolyPoint6 = cwPolyPoints[index + 2];
          Vector2 normal3 = (cwPolyPoint6 - cwPolyPoint5).GetNormal();
          Vector2 ptOnPlane = cwPolyPoint6 + normal3 * num2;
          Vector2 normalized2 = (cwPolyPoint5 - cwPolyPoint4).normalized;
          if (PlaneMath.Raycast2D(rayOrigin2, normalized2, normal3, ptOnPlane, out t))
            vector2List.Add(rayOrigin2 + normalized2 * t);
        }
        vector2List.Add(vector2List[0]);
        return vector2List;
      }
      Vector2 cwPolyPoint7 = cwPolyPoints[0];
      Vector2 normal4 = (cwPolyPoints[1] - cwPolyPoint7).GetNormal();
      vector2List.Add(cwPolyPoint7 + normal4 * num2);
      for (int index = 0; index < num1 - 1; ++index)
      {
        Vector2 cwPolyPoint8 = cwPolyPoints[index];
        Vector2 cwPolyPoint9 = cwPolyPoints[index + 1];
        Vector2 rayOrigin = vector2List[index];
        Vector2 cwPolyPoint10 = cwPolyPoints[index + 2];
        Vector2 normal5 = (cwPolyPoint10 - cwPolyPoint9).GetNormal();
        Vector2 ptOnPlane = cwPolyPoint10 + normal5 * num2;
        Vector2 normalized = (cwPolyPoint9 - cwPolyPoint8).normalized;
        float t;
        if (PlaneMath.Raycast2D(rayOrigin, normalized, normal5, ptOnPlane, out t))
          vector2List.Add(rayOrigin + normalized * t);
      }
      Vector2 cwPolyPoint11 = cwPolyPoints[cwPolyPoints.Count - 2];
      Vector2 cwPolyPoint12 = cwPolyPoints[cwPolyPoints.Count - 1];
      Vector2 normal6 = (cwPolyPoint12 - cwPolyPoint11).GetNormal();
      vector2List.Add(cwPolyPoint12 + normal6 * num2);
      return vector2List;
    }

    public static List<Vector2> Generate2DCircleBorderPointsCW(
      Vector2 circleCenter,
      float circleRadius,
      int numPoints)
    {
      numPoints = Mathf.Max(numPoints, 4);
      List<Vector2> vector2List = new List<Vector2>(numPoints);
      float num = 360f / (float) (numPoints - 1);
      for (int index = 0; index < numPoints; ++index)
      {
        float f = (float) ((double) num * (double) index * (Math.PI / 180.0));
        vector2List.Add(circleCenter + new Vector2(Mathf.Sin(f) * circleRadius, Mathf.Cos(f) * circleRadius));
      }
      return vector2List;
    }

    public static List<Vector3> Generate3DCircleBorderPoints(
      Vector3 circleCenter,
      float circleRadius,
      Vector3 circleRight,
      Vector3 circleUp,
      int numPoints)
    {
      numPoints = Mathf.Max(numPoints, 4);
      List<Vector3> vector3List = new List<Vector3>(numPoints);
      float num = 360f / (float) (numPoints - 1);
      for (int index = 0; index < numPoints; ++index)
      {
        float f = (float) ((double) num * (double) index * (Math.PI / 180.0));
        vector3List.Add(circleCenter + circleRight * Mathf.Sin(f) * circleRadius + circleUp * Mathf.Cos(f) * circleRadius);
      }
      return vector3List;
    }

    public static List<Vector3> GenerateSphereBorderPoints(
      Camera camera,
      Vector3 sphereCenter,
      float sphereRadius,
      int numPoints)
    {
      if (numPoints < 3)
        return new List<Vector3>();
      Transform transform = camera.transform;
      Plane plane = new Plane(transform.forward, transform.position);
      List<Vector3> sphereBorderPoints = PrimitiveFactory.Generate3DCircleBorderPoints(sphereCenter, sphereRadius, transform.right, transform.up, numPoints);
      for (int index = 0; index < numPoints; ++index)
      {
        Vector3 vector3_1 = sphereBorderPoints[index];
        Vector3 vector3_2 = vector3_1 - sphereCenter;
        Vector3 normalized1 = vector3_2.normalized;
        vector3_2 = vector3_1 - transform.position;
        Vector3 vector3_3 = vector3_2.normalized;
        if (camera.orthographic)
          vector3_3 = transform.forward;
        float num1 = Vector3.Dot(normalized1, vector3_3);
        if ((double) Mathf.Abs(num1) > 9.9999997473787516E-06)
        {
          float num2 = MathEx.SafeAcos(num1) * 57.29578f;
          vector3_2 = Vector3.Cross(vector3_3, normalized1);
          Vector3 normalized2 = vector3_2.normalized;
          vector3_2 = Quaternion.AngleAxis(90f - num2, normalized2) * normalized1;
          Vector3 normalized3 = vector3_2.normalized;
          sphereBorderPoints[index] = sphereCenter + normalized3 * sphereRadius;
          if ((double) plane.GetDistanceToPoint(sphereBorderPoints[index]) < 0.0)
            return new List<Vector3>();
        }
      }
      return sphereBorderPoints;
    }

    public static List<Vector2> Generate2DArcBorderPoints(
      Vector2 arcOrigin,
      Vector2 arcStartPoint,
      float degreesFromStart,
      bool forceShortestArc,
      int numPoints)
    {
      if (numPoints < 2)
        return new List<Vector2>();
      List<Vector2> vector2List = new List<Vector2>(numPoints);
      degreesFromStart %= 360f;
      Vector2 vector2 = arcStartPoint - arcOrigin;
      Quaternion.AngleAxis(degreesFromStart, Vector3.forward);
      float magnitude = vector2.magnitude;
      vector2.Normalize();
      if (forceShortestArc)
        degreesFromStart = ArcMath.ConvertToSh2DArcAngle(arcOrigin, arcStartPoint, degreesFromStart);
      float num = degreesFromStart / (float) (numPoints - 1);
      for (int index = 0; index < numPoints; ++index)
      {
        Vector2 normalized = (Vector2) (Quaternion.AngleAxis((float) index * num, Vector3.forward) * (Vector3) vector2).normalized;
        vector2List.Add(arcOrigin + normalized * magnitude);
      }
      return vector2List;
    }

    public static List<Vector2> ProjectArcPointsOnPoly2DBorder(
      Vector2 arcOrigin,
      List<Vector2> arcPoints,
      List<Vector2> clockwisePolyPoints)
    {
      if (arcPoints.Count < 2 || clockwisePolyPoints.Count < 3)
        return new List<Vector2>();
      int count = clockwisePolyPoints.Count;
      List<Vector2> vector2List = new List<Vector2>(arcPoints.Count);
      foreach (Vector2 arcPoint in arcPoints)
      {
        for (int index = 0; index < count; ++index)
        {
          Vector2 clockwisePolyPoint = clockwisePolyPoints[index];
          Vector2 vec = clockwisePolyPoints[(index + 1) % count] - clockwisePolyPoint;
          Plane2D plane2D = new Plane2D(vec.GetNormal(), clockwisePolyPoint);
          Vector2 normalized = (arcPoint - arcOrigin).normalized;
          float t;
          if (plane2D.Raycast(arcOrigin, normalized, out t))
          {
            Vector2 vector2 = arcOrigin + normalized * t;
            float num = Vector2.Dot(vector2 - clockwisePolyPoint, vec.normalized);
            if ((double) num >= 0.0 && (double) num <= (double) vec.magnitude)
            {
              vector2List.Add(vector2);
              break;
            }
          }
        }
      }
      return vector2List;
    }

    public static List<Vector3> Generate3DArcBorderPoints(
      Vector3 arcOrigin,
      Vector3 arcStartPoint,
      Plane arcPlane,
      float degreesFromStart,
      bool forceShortestArc,
      int numPoints)
    {
      if (numPoints < 2)
        return new List<Vector3>();
      List<Vector3> vector3List = new List<Vector3>(numPoints);
      degreesFromStart %= 360f;
      arcOrigin = arcPlane.ProjectPoint(arcOrigin);
      arcStartPoint = arcPlane.ProjectPoint(arcStartPoint);
      Vector3 vector3 = arcStartPoint - arcOrigin;
      Quaternion.AngleAxis(degreesFromStart, arcPlane.normal);
      float magnitude = vector3.magnitude;
      vector3.Normalize();
      if (forceShortestArc)
        degreesFromStart = ArcMath.ConvertToSh3DArcAngle(arcOrigin, arcStartPoint, arcPlane.normal, degreesFromStart);
      float num = degreesFromStart / (float) (numPoints - 1);
      for (int index = 0; index < numPoints; ++index)
      {
        Vector3 normalized = (Quaternion.AngleAxis((float) index * num, arcPlane.normal) * vector3).normalized;
        vector3List.Add(arcOrigin + normalized * magnitude);
      }
      return vector3List;
    }

    public enum PolyBorderDirection
    {
      Inward,
      Outward,
    }
  }
}
