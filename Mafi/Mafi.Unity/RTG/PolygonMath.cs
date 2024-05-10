// Decompiled with JetBrains decompiler
// Type: RTG.PolygonMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class PolygonMath
  {
    public static bool Raycast(
      Ray ray,
      out float t,
      List<Vector3> cwPolyPoints,
      bool isClosed,
      Vector3 polyNormal,
      PolygonEpsilon epsilon = default (PolygonEpsilon))
    {
      t = 0.0f;
      float enter;
      if (cwPolyPoints.Count < (isClosed ? 4 : 3) || !new Plane(polyNormal, cwPolyPoints[0]).Raycast(ray, out enter) || !PolygonMath.Contains3DPoint(ray.GetPoint(enter), false, cwPolyPoints, isClosed, polyNormal, epsilon))
        return false;
      t = enter;
      return true;
    }

    public static bool Contains3DPoint(
      Vector3 point,
      bool checkOnPlane,
      List<Vector3> cwPolyPoints,
      bool isClosed,
      Vector3 polyNormal,
      PolygonEpsilon epsilon = default (PolygonEpsilon))
    {
      if (cwPolyPoints.Count < (isClosed ? 4 : 3))
        return false;
      Plane plane = new Plane(polyNormal, cwPolyPoints[0]);
      if (checkOnPlane && (double) Mathf.Abs(plane.GetDistanceToPoint(point)) > (double) epsilon.ExtrudeEps)
        return false;
      if (isClosed)
      {
        for (int index = 0; index < cwPolyPoints.Count - 1; ++index)
        {
          Vector3 normalized = Vector3.Cross(cwPolyPoints[index + 1] - cwPolyPoints[index], polyNormal).normalized;
          if ((double) Vector3.Dot(point - cwPolyPoints[index], normalized) > (double) epsilon.AreaEps)
            return false;
        }
      }
      else
      {
        for (int index = 0; index < cwPolyPoints.Count; ++index)
        {
          Vector3 normalized = Vector3.Cross(cwPolyPoints[(index + 1) % cwPolyPoints.Count] - cwPolyPoints[index], polyNormal).normalized;
          if ((double) Vector3.Dot(point - cwPolyPoints[index], normalized) > (double) epsilon.AreaEps)
            return false;
        }
      }
      return true;
    }

    public static bool Contains2DPoint(
      Vector2 point,
      List<Vector2> polyPoints,
      bool isClosed,
      PolygonEpsilon epsilon = default (PolygonEpsilon))
    {
      if (isClosed)
      {
        for (int index = 0; index < polyPoints.Count - 1; ++index)
        {
          Vector2 normal = (polyPoints[index + 1] - polyPoints[index]).GetNormal();
          if ((double) Vector2.Dot(point - polyPoints[index], normal) > (double) epsilon.AreaEps)
            return false;
        }
      }
      else
      {
        for (int index = 0; index < polyPoints.Count; ++index)
        {
          Vector2 normal = (polyPoints[(index + 1) % polyPoints.Count] - polyPoints[index]).GetNormal();
          if ((double) Vector2.Dot(point - polyPoints[index], normal) > (double) epsilon.AreaEps)
            return false;
        }
      }
      return true;
    }

    public static bool Is3DPointOnBorder(
      Vector3 point,
      bool checkOnPlane,
      List<Vector3> cwPolyPoints,
      bool isClosed,
      Vector3 polyNormal,
      PolygonEpsilon epsilon = default (PolygonEpsilon))
    {
      if (cwPolyPoints.Count < (isClosed ? 4 : 3))
        return false;
      Plane plane = new Plane(polyNormal, cwPolyPoints[0]);
      if (checkOnPlane && (double) Mathf.Abs(plane.GetDistanceToPoint(point)) > (double) epsilon.ExtrudeEps)
        return false;
      if (!checkOnPlane)
        point = plane.ProjectPoint(point);
      if (isClosed)
      {
        for (int index = 0; index < cwPolyPoints.Count - 1; ++index)
        {
          if ((double) point.GetDistanceToSegment(cwPolyPoints[index], cwPolyPoints[index + 1]) <= (double) epsilon.WireEps)
            return true;
        }
      }
      else
      {
        for (int index = 0; index < cwPolyPoints.Count; ++index)
        {
          if ((double) point.GetDistanceToSegment(cwPolyPoints[index], cwPolyPoints[(index + 1) % cwPolyPoints.Count]) <= (double) epsilon.WireEps)
            return true;
        }
      }
      return false;
    }

    public static bool Is2DPointOnBorder(
      Vector2 point,
      List<Vector2> polyPoints,
      bool isClosed,
      PolygonEpsilon epsilon = default (PolygonEpsilon))
    {
      if (polyPoints.Count < (isClosed ? 4 : 3))
        return false;
      if (isClosed)
      {
        for (int index = 0; index < polyPoints.Count - 1; ++index)
        {
          if ((double) point.GetDistanceToSegment(polyPoints[index], polyPoints[index + 1]) <= (double) epsilon.WireEps)
            return true;
        }
      }
      else
      {
        for (int index = 0; index < polyPoints.Count; ++index)
        {
          if ((double) point.GetDistanceToSegment(polyPoints[index], polyPoints[(index + 1) % polyPoints.Count]) <= (double) epsilon.WireEps)
            return true;
        }
      }
      return false;
    }

    public static bool Is2DPointOnThickBorder(
      Vector2 point,
      List<Vector2> polyPoints,
      List<Vector2> thickBorderPoints,
      bool isClosed,
      PolygonEpsilon epsilon = default (PolygonEpsilon))
    {
      if (polyPoints.Count != thickBorderPoints.Count || polyPoints.Count < (isClosed ? 4 : 3))
        return false;
      List<Vector2> polyPoints1 = new List<Vector2>(4);
      PolygonEpsilon epsilon1 = new PolygonEpsilon();
      epsilon1.AreaEps = epsilon.ThickWireEps;
      for (int index = 0; index < polyPoints.Count - 1; ++index)
      {
        polyPoints1.Clear();
        polyPoints1.Add(polyPoints[index]);
        polyPoints1.Add(thickBorderPoints[index]);
        polyPoints1.Add(thickBorderPoints[index + 1]);
        polyPoints1.Add(polyPoints[index + 1]);
        polyPoints1.Add(polyPoints[index]);
        if (PolygonMath.Contains2DPoint(point, polyPoints1, true, epsilon1))
          return true;
      }
      return false;
    }
  }
}
