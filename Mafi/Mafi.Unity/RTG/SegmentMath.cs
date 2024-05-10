// Decompiled with JetBrains decompiler
// Type: RTG.SegmentMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class SegmentMath
  {
    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 startPoint,
      Vector3 endPoint,
      SegmentEpsilon epsilon = default (SegmentEpsilon))
    {
      return CylinderMath.Raycast(ray, out t, startPoint, endPoint, epsilon.RaycastEps) || SphereMath.Raycast(ray, out t, startPoint, epsilon.RaycastEps) || SphereMath.Raycast(ray, out t, endPoint, epsilon.RaycastEps);
    }

    public static bool Is3DPointOnSegment(
      Vector3 point,
      Vector3 startPoint,
      Vector3 endPoint,
      SegmentEpsilon epsilon = default (SegmentEpsilon))
    {
      return (double) point.GetDistanceToSegment(startPoint, endPoint) <= (double) epsilon.PtOnSegmentEps;
    }

    public static bool Is2DPointOnSegment(
      Vector2 point,
      Vector2 startPoint,
      Vector2 endPoint,
      SegmentEpsilon epsilon = default (SegmentEpsilon))
    {
      return (double) point.GetDistanceToSegment(startPoint, endPoint) <= (double) epsilon.PtOnSegmentEps;
    }

    public static Vector3 ProjectPtOnSegment(Vector3 point, Vector3 startPoint, Vector3 endPoint)
    {
      Vector3 normalized = (endPoint - startPoint).normalized;
      float num = Vector3.Dot(normalized, point - startPoint);
      return startPoint + normalized * num;
    }
  }
}
