// Decompiled with JetBrains decompiler
// Type: RTG.PyramidMath
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
  public static class PyramidMath
  {
    public static List<Vector3> CalcBaseCornerPoints(
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      Quaternion rotation)
    {
      Vector3 vector3_1 = rotation * Vector3.right;
      Vector3 vector3_2 = rotation * Vector3.forward;
      float num1 = baseWidth * 0.5f;
      float num2 = baseDepth * 0.5f;
      return new List<Vector3>()
      {
        baseCenter + vector3_1 * num1 + vector3_2 * num2,
        baseCenter + vector3_1 * num1 - vector3_2 * num2,
        baseCenter - vector3_1 * num1 - vector3_2 * num2,
        baseCenter - vector3_1 * num1 + vector3_2 * num2
      };
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      float height,
      Quaternion rotation)
    {
      t = 0.0f;
      ray = ray.InverseTransform(Matrix4x4.TRS(baseCenter, rotation, Vector3.one));
      Vector3 boxSize = new Vector3(baseWidth, height, baseDepth);
      if (!BoxMath.Raycast(ray, Vector3.up * height * 0.5f, boxSize, Quaternion.identity))
        return false;
      List<float> floatList = new List<float>(5);
      Plane plane1 = new Plane(Vector3.up, Vector3.zero);
      float enter = 0.0f;
      if (plane1.Raycast(ray, out enter) && QuadMath.Contains3DPoint(ray.GetPoint(enter), false, baseCenter, baseWidth, baseDepth, Vector3.right, Vector3.forward))
        floatList.Add(enter);
      float num1 = 0.5f * baseWidth;
      float num2 = 0.5f * baseDepth;
      Vector3 vector3_1 = Vector3.up * height;
      Vector3 vector3_2 = vector3_1;
      Vector3 vector3_3 = Vector3.right * num1 - Vector3.forward * num2;
      Vector3 vector3_4 = vector3_3 - Vector3.right * baseWidth;
      Plane plane2 = new Plane(vector3_2, vector3_3, vector3_4);
      if (plane2.Raycast(ray, out enter) && TriangleMath.Contains3DPoint(ray.GetPoint(enter), false, vector3_2, vector3_3, vector3_4))
        floatList.Add(enter);
      Vector3 vector3_5 = vector3_1;
      Vector3 vector3_6 = Vector3.right * num1 + Vector3.forward * num2;
      Vector3 vector3_7 = vector3_6 - Vector3.forward * baseDepth;
      plane2 = new Plane(vector3_5, vector3_6, vector3_7);
      if (plane2.Raycast(ray, out enter) && TriangleMath.Contains3DPoint(ray.GetPoint(enter), false, vector3_5, vector3_6, vector3_7))
        floatList.Add(enter);
      Vector3 vector3_8 = vector3_1;
      Vector3 vector3_9 = -Vector3.right * num1 + Vector3.forward * num2;
      Vector3 vector3_10 = vector3_9 + Vector3.right * baseWidth;
      plane2 = new Plane(vector3_8, vector3_9, vector3_10);
      if (plane2.Raycast(ray, out enter) && TriangleMath.Contains3DPoint(ray.GetPoint(enter), false, vector3_8, vector3_9, vector3_10))
        floatList.Add(enter);
      Vector3 vector3_11 = vector3_1;
      Vector3 vector3_12 = -Vector3.right * num1 - Vector3.forward * num2;
      Vector3 vector3_13 = vector3_12 + Vector3.forward * baseDepth;
      plane2 = new Plane(vector3_11, vector3_12, vector3_13);
      if (plane2.Raycast(ray, out enter) && TriangleMath.Contains3DPoint(ray.GetPoint(enter), false, vector3_11, vector3_12, vector3_13))
        floatList.Add(enter);
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
      float height,
      Quaternion rotation,
      PyramidEpsilon epsilon = default (PyramidEpsilon))
    {
      point = Matrix4x4.TRS(baseCenter, rotation, Vector3.one).inverse.MultiplyPoint(point);
      if ((double) new Plane(-Vector3.up, Vector3.zero).GetDistanceToPoint(point) > (double) epsilon.PtContainEps)
        return false;
      float num1 = 0.5f * baseWidth;
      float num2 = 0.5f * baseDepth;
      Vector3 vector3 = Vector3.up * height;
      Vector3 a1 = vector3;
      Vector3 b1 = Vector3.right * num1 - Vector3.forward * num2;
      Vector3 c1 = b1 - Vector3.right * baseWidth;
      Plane plane = new Plane(a1, b1, c1);
      if ((double) plane.GetDistanceToPoint(point) > (double) epsilon.PtContainEps)
        return false;
      Vector3 a2 = vector3;
      Vector3 b2 = Vector3.right * num1 + Vector3.forward * num2;
      Vector3 c2 = b2 - Vector3.forward * baseDepth;
      plane = new Plane(a2, b2, c2);
      if ((double) plane.GetDistanceToPoint(point) > (double) epsilon.PtContainEps)
        return false;
      Vector3 a3 = vector3;
      Vector3 b3 = -Vector3.right * num1 + Vector3.forward * num2;
      Vector3 c3 = b3 + Vector3.right * baseWidth;
      plane = new Plane(a3, b3, c3);
      if ((double) plane.GetDistanceToPoint(point) > (double) epsilon.PtContainEps)
        return false;
      Vector3 a4 = vector3;
      Vector3 b4 = -Vector3.right * num1 - Vector3.forward * num2;
      Vector3 c4 = b4 + Vector3.forward * baseDepth;
      plane = new Plane(a4, b4, c4);
      return (double) plane.GetDistanceToPoint(point) <= (double) epsilon.PtContainEps;
    }
  }
}
