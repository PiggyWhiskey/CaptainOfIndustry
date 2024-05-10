// Decompiled with JetBrains decompiler
// Type: RTG.QuadMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class QuadMath
  {
    public static void Calc2DQuadRightUp(float degreeRotation, out Vector2 right, out Vector2 up)
    {
      right = Vector2.right;
      up = Vector2.up;
      Quaternion quaternion = Quaternion.AngleAxis(degreeRotation, Vector3.forward);
      right = (Vector2) (quaternion * (Vector3) right);
      up = (Vector2) (quaternion * (Vector3) up);
    }

    public static List<Vector2> Calc2DQuadCornerPoints(
      Vector2 quadCenter,
      Vector2 quadSize,
      float degreeRotation)
    {
      Vector2 right = Vector2.right;
      Vector2 up = Vector2.up;
      QuadMath.Calc2DQuadRightUp(degreeRotation, out right, out up);
      Vector2 vector2 = quadSize * 0.5f;
      return new List<Vector2>()
      {
        quadCenter - right * vector2.x + up * vector2.y,
        quadCenter + right * vector2.x + up * vector2.y,
        quadCenter + right * vector2.x - up * vector2.y,
        quadCenter - right * vector2.x - up * vector2.y
      };
    }

    public static List<Vector2> Calc2DQuadCornerPoints(
      Vector2 quadCenter,
      Vector2 quadSize,
      Vector2 right,
      Vector2 up)
    {
      Vector2 vector2 = quadSize * 0.5f;
      return new List<Vector2>()
      {
        quadCenter - right * vector2.x + up * vector2.y,
        quadCenter + right * vector2.x + up * vector2.y,
        quadCenter + right * vector2.x - up * vector2.y,
        quadCenter - right * vector2.x - up * vector2.y
      };
    }

    public static List<Vector3> Calc3DQuadCornerPoints(
      Vector3 quadCenter,
      Vector2 quadSize,
      Quaternion quadRotation)
    {
      Vector3 vector3_1 = quadRotation * Vector3.right;
      Vector3 vector3_2 = quadRotation * Vector3.up;
      Vector3 vector3_3 = (Vector3) (quadSize * 0.5f);
      return new List<Vector3>()
      {
        quadCenter - vector3_1 * vector3_3.x + vector3_2 * vector3_3.y,
        quadCenter + vector3_1 * vector3_3.x + vector3_2 * vector3_3.y,
        quadCenter + vector3_1 * vector3_3.x - vector3_2 * vector3_3.y,
        quadCenter - vector3_1 * vector3_3.x - vector3_2 * vector3_3.y
      };
    }

    public static Vector3 Calc3DQuadCorner(
      Vector3 quadCenter,
      Vector2 quadSize,
      Quaternion quadRotation,
      QuadCorner quadCorner)
    {
      Vector3 vector3_1 = quadRotation * Vector3.right;
      Vector3 vector3_2 = quadRotation * Vector3.up;
      Vector3 vector3_3 = (Vector3) (quadSize * 0.5f);
      switch (quadCorner)
      {
        case QuadCorner.TopLeft:
          return quadCenter - vector3_1 * vector3_3.x + vector3_2 * vector3_3.y;
        case QuadCorner.TopRight:
          return quadCenter + vector3_1 * vector3_3.x + vector3_2 * vector3_3.y;
        case QuadCorner.BottomRight:
          return quadCenter + vector3_1 * vector3_3.x - vector3_2 * vector3_3.y;
        default:
          return quadCenter - vector3_1 * vector3_3.x - vector3_2 * vector3_3.y;
      }
    }

    public static OBB Calc3DQuadOBB(
      Vector3 quadCenter,
      Vector2 quadSize,
      Quaternion quadRotation,
      QuadEpsilon epsilon = default (QuadEpsilon))
    {
      Vector3 size = (Vector3) (quadSize + epsilon.SizeEps) with
      {
        z = epsilon.ExtrudeEps * 2f
      };
      return new OBB(quadCenter, size, quadRotation);
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 quadCenter,
      float quadWidth,
      float quadHeight,
      Vector3 quadRight,
      Vector3 quadUp,
      QuadEpsilon epsilon = default (QuadEpsilon))
    {
      t = 0.0f;
      Vector3 vector3 = Vector3.Normalize(Vector3.Cross(quadRight, quadUp));
      Plane plane = new Plane(vector3, quadCenter);
      float enter;
      if (plane.Raycast(ray, out enter) && QuadMath.Contains3DPoint(ray.GetPoint(enter), false, quadCenter, quadWidth, quadHeight, quadRight, quadUp, epsilon))
      {
        t = enter;
        return true;
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(plane.normal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      OBB obb = QuadMath.Calc3DQuadOBB(quadCenter, new Vector2(quadWidth, quadHeight), Quaternion.LookRotation(vector3, quadUp), epsilon);
      return BoxMath.Raycast(ray, obb.Center, obb.Size, obb.Rotation);
    }

    public static bool RaycastWire(
      Ray ray,
      out float t,
      Vector3 quadCenter,
      float quadWidth,
      float quadHeight,
      Vector3 quadRight,
      Vector3 quadUp,
      QuadEpsilon epsilon = default (QuadEpsilon))
    {
      t = 0.0f;
      Vector3 vector3 = Vector3.Normalize(Vector3.Cross(quadRight, quadUp));
      Plane plane = new Plane(vector3, quadCenter);
      Vector2 quadSize = new Vector2(quadWidth, quadHeight);
      Quaternion quadRotation = Quaternion.LookRotation(vector3, quadUp);
      float enter;
      if (plane.Raycast(ray, out enter))
      {
        Vector3 point = ray.GetPoint(enter);
        List<Vector3> vector3List = QuadMath.Calc3DQuadCornerPoints(quadCenter, quadSize, quadRotation);
        if ((double) point.GetDistanceToSegment(vector3List[0], vector3List[1]) <= (double) epsilon.WireEps)
        {
          t = enter;
          return true;
        }
        if ((double) point.GetDistanceToSegment(vector3List[1], vector3List[2]) <= (double) epsilon.WireEps)
        {
          t = enter;
          return true;
        }
        if ((double) point.GetDistanceToSegment(vector3List[2], vector3List[3]) <= (double) epsilon.WireEps)
        {
          t = enter;
          return true;
        }
        if ((double) point.GetDistanceToSegment(vector3List[3], vector3List[0]) <= (double) epsilon.WireEps)
        {
          t = enter;
          return true;
        }
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(plane.normal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      OBB obb = QuadMath.Calc3DQuadOBB(quadCenter, quadSize, Quaternion.LookRotation(vector3, quadUp), epsilon);
      return BoxMath.Raycast(ray, obb.Center, obb.Size, obb.Rotation);
    }

    public static bool Contains3DPoint(
      Vector3 point,
      bool checkOnPlane,
      Vector3 quadCenter,
      float quadWidth,
      float quadHeight,
      Vector3 quadRight,
      Vector3 quadUp,
      QuadEpsilon epsilon = default (QuadEpsilon))
    {
      Plane plane = new Plane(Vector3.Cross(quadRight, quadUp).normalized, quadCenter);
      if (checkOnPlane && (double) plane.GetAbsDistanceToPoint(point) > (double) epsilon.ExtrudeEps)
        return false;
      quadWidth += epsilon.WidthEps;
      quadHeight += epsilon.HeightEps;
      Vector3 v1 = point - quadCenter;
      float num1 = v1.AbsDot(quadRight);
      float num2 = v1.AbsDot(quadUp);
      return (double) num1 <= (double) quadWidth * 0.5 && (double) num2 <= (double) quadHeight * 0.5;
    }

    public static bool Contains2DPoint(
      Vector2 point,
      Vector2 quadCenter,
      float quadWidth,
      float quadHeight,
      float degreeRotation,
      QuadEpsilon epsilon = default (QuadEpsilon))
    {
      Vector2 right;
      Vector2 up;
      QuadMath.Calc2DQuadRightUp(degreeRotation, out right, out up);
      return QuadMath.Contains2DPoint(point, quadCenter, quadWidth, quadHeight, right, up, epsilon);
    }

    public static bool Contains2DPoint(
      Vector2 point,
      Vector2 quadCenter,
      float quadWidth,
      float quadHeight,
      Vector2 quadRight,
      Vector2 quadUp,
      QuadEpsilon epsilon = default (QuadEpsilon))
    {
      quadWidth += epsilon.WidthEps;
      quadHeight += epsilon.HeightEps;
      Vector2 v1 = point - quadCenter;
      float num1 = v1.AbsDot(quadRight);
      float num2 = v1.AbsDot(quadUp);
      return (double) num1 <= (double) quadWidth * 0.5 && (double) num2 <= (double) quadHeight * 0.5;
    }

    public static bool Is2DPointOnBorder(
      Vector2 point,
      Vector2 quadCenter,
      float quadWidth,
      float quadHeight,
      float degreeRotation,
      QuadEpsilon epsilon = default (QuadEpsilon))
    {
      Vector2 right;
      Vector2 up;
      QuadMath.Calc2DQuadRightUp(degreeRotation, out right, out up);
      return QuadMath.Is2DPointOnBorder(point, quadCenter, quadWidth, quadHeight, right, up, epsilon);
    }

    public static bool Is2DPointOnBorder(
      Vector2 point,
      Vector2 quadCenter,
      float quadWidth,
      float quadHeight,
      Vector2 quadRight,
      Vector2 quadUp,
      QuadEpsilon epsilon = default (QuadEpsilon))
    {
      SegmentEpsilon epsilon1 = new SegmentEpsilon();
      epsilon1.PtOnSegmentEps = epsilon.WireEps;
      List<Vector2> vector2List = QuadMath.Calc2DQuadCornerPoints(quadCenter, new Vector2(quadWidth, quadHeight), quadRight, quadUp);
      for (int index = 0; index < vector2List.Count; ++index)
      {
        Vector2 startPoint = vector2List[index];
        Vector2 endPoint = vector2List[(index + 1) % vector2List.Count];
        if (SegmentMath.Is2DPointOnSegment(point, startPoint, endPoint, epsilon1))
          return true;
      }
      return false;
    }
  }
}
