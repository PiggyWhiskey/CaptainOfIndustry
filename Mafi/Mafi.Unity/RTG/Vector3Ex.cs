// Decompiled with JetBrains decompiler
// Type: RTG.Vector3Ex
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class Vector3Ex
  {
    public static void OffsetPoints(List<Vector3> points, Vector3 offset)
    {
      for (int index = 0; index < points.Count; ++index)
        points[index] += offset;
    }

    public static Vector2 ConvertDirTo2D(Vector3 start, Vector3 end, Camera camera)
    {
      Vector2 screenPoint = (Vector2) camera.WorldToScreenPoint(start);
      return (Vector2) camera.WorldToScreenPoint(end) - screenPoint;
    }

    public static Vector3 Abs(this Vector3 v)
    {
      return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
    }

    public static Vector3 GetSignVector(this Vector3 v)
    {
      return new Vector3(Mathf.Sign(v.x), Mathf.Sign(v.y), Mathf.Sign(v.z));
    }

    public static float GetMaxAbsComp(this Vector3 v)
    {
      float maxAbsComp = Mathf.Abs(v.x);
      float num1 = Mathf.Abs(v.y);
      if ((double) num1 > (double) maxAbsComp)
        maxAbsComp = num1;
      float num2 = Mathf.Abs(v.z);
      if ((double) num2 > (double) maxAbsComp)
        maxAbsComp = num2;
      return maxAbsComp;
    }

    public static float Dot(this Vector3 v1, Vector3 v2) => Vector3.Dot(v1, v2);

    public static float AbsDot(this Vector3 v1, Vector3 v2) => Mathf.Abs(Vector3.Dot(v1, v2));

    public static Vector3 FromValue(float value) => new Vector3(value, value, value);

    public static float SignedAngle(Vector3 from, Vector3 to, Vector3 axis)
    {
      float cosine = Vector3.Dot(from.normalized, to.normalized);
      if (1.0 - (double) cosine < 9.9999997473787516E-06)
        return 0.0f;
      if (1.0 + (double) cosine < 9.9999997473787516E-06)
        return 180f;
      Vector3 normalized = Vector3.Cross(from, to).normalized;
      float num = MathEx.SafeAcos(cosine) * 57.29578f;
      if ((double) Vector3.Dot(normalized, axis) < 0.0)
        num = -num;
      return num;
    }

    public static float GetDistanceToSegment(this Vector3 point, Vector3 point0, Vector3 point1)
    {
      Vector3 lhs = point1 - point0;
      float magnitude = lhs.magnitude;
      lhs.Normalize();
      Vector3 rhs = point - point0;
      float num = Vector3.Dot(lhs, rhs);
      if ((double) num >= 0.0 && (double) num <= (double) magnitude)
        return (point0 + lhs * num - point).magnitude;
      return (double) num < 0.0 ? rhs.magnitude : (point1 - point).magnitude;
    }

    public static Vector3 ProjectOnSegment(this Vector3 point, Vector3 point0, Vector3 point1)
    {
      Vector3 normalized = (point1 - point0).normalized;
      return point0 + normalized * Vector3.Dot(point - point0, normalized);
    }

    public static int GetPointClosestToPoint(List<Vector3> points, Vector3 pt)
    {
      float num = float.MaxValue;
      int pointClosestToPoint = -1;
      for (int index = 0; index < points.Count; ++index)
      {
        float sqrMagnitude = (points[index] - pt).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num)
        {
          num = sqrMagnitude;
          pointClosestToPoint = index;
        }
      }
      return pointClosestToPoint;
    }

    public static Vector3 GetPointCloudCenter(IEnumerable<Vector3> ptCloud)
    {
      Vector3 lhs1 = Vector3Ex.FromValue(float.MinValue);
      Vector3 lhs2 = Vector3Ex.FromValue(float.MaxValue);
      foreach (Vector3 rhs in ptCloud)
      {
        lhs1 = Vector3.Max(lhs1, rhs);
        lhs2 = Vector3.Min(lhs2, rhs);
      }
      return (lhs1 + lhs2) * 0.5f;
    }

    public static Vector3 GetInverse(this Vector3 vector)
    {
      return new Vector3(1f / vector.x, 1f / vector.y, 1f / vector.z);
    }

    public static bool IsAligned(this Vector3 vector, Vector3 other, bool checkSameDirection)
    {
      if (!checkSameDirection)
        return (double) Mathf.Abs(vector.AbsDot(other) - 1f) < 9.9999997473787516E-06;
      float num = vector.Dot(other);
      return (double) num > 0.0 && (double) Mathf.Abs(num - 1f) < 9.9999997473787516E-06;
    }

    public static bool PointsSameDir(this Vector3 vector, Vector3 other)
    {
      return (double) vector.Dot(other) > 0.0;
    }

    public static int GetMostAligned(Vector3[] vectors, Vector3 dir, bool checkSameDirection)
    {
      if (vectors.Length == 0)
        return -1;
      float num1 = float.MinValue;
      int mostAligned = -1;
      if (!checkSameDirection)
      {
        for (int index = 0; index < vectors.Length; ++index)
        {
          float num2 = vectors[index].AbsDot(dir);
          if ((double) num2 > (double) num1)
          {
            num1 = num2;
            mostAligned = index;
          }
        }
        return mostAligned;
      }
      for (int index = 0; index < vectors.Length; ++index)
      {
        float num3 = vectors[index].Dot(dir);
        if ((double) num3 > 0.0 && (double) num3 > (double) num1)
        {
          num1 = num3;
          mostAligned = index;
        }
      }
      return mostAligned;
    }
  }
}
