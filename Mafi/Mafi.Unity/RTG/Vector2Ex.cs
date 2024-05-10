// Decompiled with JetBrains decompiler
// Type: RTG.Vector2Ex
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class Vector2Ex
  {
    public static Vector3 ConvertDirTo3D(Vector2 start, Vector2 end, Vector3 zPos, Camera camera)
    {
      float pointZdistance = camera.GetPointZDistance(zPos);
      Vector3 worldPoint = camera.ScreenToWorldPoint(new Vector3(start.x, start.y, pointZdistance));
      return camera.ScreenToWorldPoint(new Vector3(end.x, end.y, pointZdistance)) - worldPoint;
    }

    public static Vector3 ConvertDirTo3D(Vector2 dir, Vector3 zPos, Camera camera)
    {
      float pointZdistance = camera.GetPointZDistance(zPos);
      Vector3 worldPoint = camera.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, pointZdistance));
      return camera.ScreenToWorldPoint(new Vector3(dir.x, dir.y, pointZdistance)) - worldPoint;
    }

    public static Vector2 Abs(this Vector2 v) => new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));

    public static float AbsDot(this Vector2 v1, Vector2 v2) => Mathf.Abs(Vector2.Dot(v1, v2));

    public static Vector3 ToVector3(this Vector2 vec, float z = 0.0f)
    {
      return new Vector3(vec.x, vec.y, z);
    }

    public static Vector2 GetNormal(this Vector2 vec) => new Vector2(-vec.y, vec.x).normalized;

    public static Vector2 FromValue(float value) => new Vector2(value, value);

    public static float GetDistanceToSegment(this Vector2 point, Vector2 point0, Vector2 point1)
    {
      Vector2 lhs = point1 - point0;
      float magnitude = lhs.magnitude;
      lhs.Normalize();
      Vector2 rhs = point - point0;
      float num = Vector2.Dot(lhs, rhs);
      if ((double) num >= 0.0 && (double) num <= (double) magnitude)
        return (point0 + lhs * num - point).magnitude;
      return (double) num < 0.0 ? rhs.magnitude : (point1 - point).magnitude;
    }

    public static int GetPointClosestToPoint(List<Vector2> points, Vector2 pt)
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
  }
}
