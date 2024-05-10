// Decompiled with JetBrains decompiler
// Type: RTG.QuaternionEx
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class QuaternionEx
  {
    public static void RotatePoints(this Quaternion quat, List<Vector3> points, Vector3 pivot)
    {
      for (int index = 0; index < points.Count; ++index)
      {
        Vector3 vector3_1 = points[index] - pivot;
        Vector3 vector3_2 = quat * vector3_1;
        points[index] = pivot + vector3_2;
      }
    }

    public static Quaternion GetRelativeRotation(Quaternion from, Quaternion to)
    {
      return Quaternion.Inverse(from) * to;
    }

    public static float Length(this Quaternion quat)
    {
      return Mathf.Sqrt((float) ((double) quat.x * (double) quat.x + (double) quat.y * (double) quat.y + (double) quat.z * (double) quat.z + (double) quat.w * (double) quat.w));
    }

    public static Quaternion Normalize(Quaternion quat)
    {
      float num1 = quat.Length();
      if ((double) num1 < 9.9999997473787516E-06)
        return quat;
      float num2 = 1f / num1;
      return new Quaternion(quat.x * num2, quat.y * num2, quat.z * num2, quat.w * num2);
    }

    public static Quaternion FromToRotation3D(Vector3 from, Vector3 to, Vector3 perp180)
    {
      from = from.normalized;
      to = to.normalized;
      float cosine = Vector3.Dot(from, to);
      if (1.0 - (double) cosine < 9.9999997473787516E-06)
        return Quaternion.identity;
      return 1.0 + (double) cosine < 9.9999997473787516E-06 ? Quaternion.AngleAxis(180f, perp180) : Quaternion.AngleAxis(MathEx.SafeAcos(cosine) * 57.29578f, Vector3.Cross(from, to).normalized);
    }

    public static Quaternion FromToRotation2D(Vector2 from, Vector2 to)
    {
      from = from.normalized;
      to = to.normalized;
      float cosine = Vector2.Dot(from, to);
      if (1.0 - (double) cosine < 9.9999997473787516E-06)
        return Quaternion.identity;
      return 1.0 + (double) cosine < 9.9999997473787516E-06 ? Quaternion.AngleAxis(180f, Vector3.forward) : Quaternion.AngleAxis(MathEx.SafeAcos(cosine) * 57.29578f, Vector3.Cross((Vector3) from, (Vector3) to).normalized);
    }

    public static float ConvertTo2DRotation(this Quaternion quat)
    {
      float angle;
      Vector3 axis;
      quat.ToAngleAxis(out angle, out axis);
      if ((double) Vector3.Dot(Vector3.forward, axis) < 0.0)
        angle = -angle;
      return angle;
    }
  }
}
