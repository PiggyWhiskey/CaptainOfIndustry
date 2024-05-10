// Decompiled with JetBrains decompiler
// Type: RTG.PrismMath
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
  public static class PrismMath
  {
    public static List<Vector3> CalcTriangPrismCornerPoints(
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      float topWidth,
      float topDepth,
      float height,
      Quaternion prismRotation)
    {
      float num1 = baseWidth * 0.5f;
      float num2 = baseDepth * 0.5f;
      float num3 = topWidth * 0.5f;
      float num4 = topDepth * 0.5f;
      Vector3 point1 = -Vector3.forward * num2 - Vector3.right * num1;
      Vector3 point2 = point1 + Vector3.right * baseWidth;
      Vector3 point3 = Vector3.forward * num2;
      Vector3 vector3 = Vector3.up * height;
      Vector3 point4 = vector3 - Vector3.forward * num4 - Vector3.right * num3;
      Vector3 point5 = vector3 + Vector3.forward * num4;
      Vector3 point6 = point4 + Vector3.right * topWidth;
      List<Vector3> vector3List = new List<Vector3>(6);
      Matrix4x4 matrix4x4 = Matrix4x4.TRS(baseCenter, prismRotation, Vector3.one);
      vector3List.Add(matrix4x4.MultiplyPoint(point1));
      vector3List.Add(matrix4x4.MultiplyPoint(point2));
      vector3List.Add(matrix4x4.MultiplyPoint(point3));
      vector3List.Add(matrix4x4.MultiplyPoint(point4));
      vector3List.Add(matrix4x4.MultiplyPoint(point5));
      vector3List.Add(matrix4x4.MultiplyPoint(point6));
      return vector3List;
    }

    public static bool RaycastTriangular(
      Ray ray,
      out float t,
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      float topWidth,
      float topDepth,
      float height,
      Quaternion prismRotation)
    {
      t = 0.0f;
      if ((double) baseWidth == 0.0 || (double) baseDepth == 0.0 || (double) topWidth == 0.0 || (double) topDepth == 0.0 || (double) height == 0.0)
        return false;
      baseWidth = Mathf.Abs(baseWidth);
      baseDepth = Mathf.Abs(baseDepth);
      topWidth = Mathf.Abs(topWidth);
      topDepth = Mathf.Abs(topDepth);
      ray = ray.InverseTransform(Matrix4x4.TRS(baseCenter, prismRotation, Vector3.one));
      Vector3 boxSize = Vector3.Max(new Vector3(baseWidth, height, baseDepth), new Vector3(topWidth, height, topDepth));
      if (!BoxMath.Raycast(ray, Vector3.up * height * 0.5f, boxSize, Quaternion.identity))
        return false;
      List<Vector3> vector3List = PrismMath.CalcTriangPrismCornerPoints(Vector3.zero, baseWidth, baseDepth, topWidth, topDepth, height, Quaternion.identity);
      Vector3 p0_1 = vector3List[0];
      Vector3 p1_1 = vector3List[1];
      Vector3 p2_1 = vector3List[2];
      Vector3 p0_2 = vector3List[3];
      Vector3 p2_2 = vector3List[5];
      Vector3 p1_2 = vector3List[4];
      List<float> floatList = new List<float>(5);
      float t2;
      if (TriangleMath.Raycast(ray, out t2, p0_1, p1_1, p2_1))
        floatList.Add(t2);
      if (TriangleMath.Raycast(ray, out t2, p0_2, p1_2, p2_2))
        floatList.Add(t2);
      List<Vector3> cwPolyPoints = new List<Vector3>(4)
      {
        p0_1,
        p0_2,
        p2_2,
        p1_1
      };
      Vector3 vector3 = Vector3.Cross(cwPolyPoints[1] - cwPolyPoints[0], cwPolyPoints[3] - cwPolyPoints[0]);
      Vector3 normalized1 = vector3.normalized;
      if (PolygonMath.Raycast(ray, out t2, cwPolyPoints, false, normalized1))
        floatList.Add(t2);
      cwPolyPoints[1] = p2_1;
      cwPolyPoints[2] = p1_2;
      cwPolyPoints[3] = p0_2;
      vector3 = Vector3.Cross(cwPolyPoints[1] - cwPolyPoints[0], cwPolyPoints[3] - cwPolyPoints[0]);
      Vector3 normalized2 = vector3.normalized;
      if (PolygonMath.Raycast(ray, out t2, cwPolyPoints, false, normalized2))
        floatList.Add(t2);
      cwPolyPoints[0] = p1_1;
      cwPolyPoints[1] = p2_2;
      cwPolyPoints[3] = p2_1;
      vector3 = Vector3.Cross(cwPolyPoints[1] - cwPolyPoints[0], cwPolyPoints[3] - cwPolyPoints[0]);
      Vector3 normalized3 = vector3.normalized;
      if (PolygonMath.Raycast(ray, out t2, cwPolyPoints, false, normalized3))
        floatList.Add(t2);
      if (floatList.Count == 0)
        return false;
      floatList.Sort((Comparison<float>) ((t0, t1) => t0.CompareTo(t1)));
      t = floatList[0];
      return true;
    }

    public static bool ContainsPoint(
      Vector3 point,
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      float topWidth,
      float topDepth,
      float height,
      Quaternion prismRotation,
      PrismEpsilon epsilon = default (PrismEpsilon))
    {
      point = Matrix4x4.TRS(baseCenter, prismRotation, Vector3.one).inverse.MultiplyPoint(point);
      List<Vector3> vector3List = PrismMath.CalcTriangPrismCornerPoints(Vector3.zero, baseWidth, baseDepth, topWidth, topDepth, height, Quaternion.identity);
      Vector3 vector3_1 = vector3List[0];
      Vector3 a = vector3List[1];
      Vector3 vector3_2 = vector3List[2];
      Vector3 c = vector3List[3];
      Vector3 b = vector3List[5];
      Vector3 vector3_3 = vector3List[4];
      Plane plane = new Plane(Vector3.up, vector3_3);
      if ((double) plane.GetDistanceToPoint(point) > (double) epsilon.PtContainEps)
        return false;
      plane = new Plane(-Vector3.up, vector3_2);
      if ((double) plane.GetDistanceToPoint(point) > (double) epsilon.PtContainEps)
        return false;
      plane = new Plane(vector3_1, vector3_2, vector3_3);
      if ((double) plane.GetDistanceToPoint(point) > (double) epsilon.PtContainEps)
        return false;
      plane = new Plane(a, b, vector3_3);
      if ((double) plane.GetDistanceToPoint(point) > (double) epsilon.PtContainEps)
        return false;
      plane = new Plane(a, vector3_1, c);
      return (double) plane.GetDistanceToPoint(point) <= (double) epsilon.PtContainEps;
    }
  }
}
