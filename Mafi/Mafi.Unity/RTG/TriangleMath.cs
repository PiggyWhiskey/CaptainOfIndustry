// Decompiled with JetBrains decompiler
// Type: RTG.TriangleMath
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public static class TriangleMath
  {
    private static readonly float _eqTriangleAltFactor;

    public static float EqTriangleAltFactor => TriangleMath._eqTriangleAltFactor;

    public static float GetEqTriangleAltitude(float sideLength)
    {
      return sideLength * TriangleMath._eqTriangleAltFactor;
    }

    public static float GetEqTriangleCentroidAltitude(float sideLength)
    {
      return (float) ((double) sideLength * (double) TriangleMath._eqTriangleAltFactor / 3.0);
    }

    public static List<Vector3> CalcEqTriangle3DPoints(
      Vector3 centroid,
      float sideLength,
      Quaternion rotation)
    {
      float num = sideLength * 0.5f;
      float centroidAltitude = TriangleMath.GetEqTriangleCentroidAltitude(sideLength);
      Vector3 vector3 = centroid - rotation * Vector3.up * centroidAltitude;
      return new List<Vector3>()
      {
        vector3 - rotation * Vector3.right * num,
        centroid + rotation * Vector3.up * (TriangleMath.GetEqTriangleAltitude(sideLength) - centroidAltitude),
        vector3 + rotation * Vector3.right * num
      };
    }

    public static List<Vector2> CalcEqTriangle2DPoints(
      Vector2 centroid,
      float sideLength,
      Quaternion rotation)
    {
      float num = sideLength * 0.5f;
      float centroidAltitude = TriangleMath.GetEqTriangleCentroidAltitude(sideLength);
      Vector2 vector2_1 = (Vector2) (rotation * (Vector3) Vector2.right);
      Vector2 vector2_2 = (Vector2) (rotation * (Vector3) Vector2.up);
      Vector2 vector2_3 = centroid - vector2_2 * centroidAltitude;
      return new List<Vector2>()
      {
        vector2_3 - vector2_1 * num,
        centroid + vector2_2 * (TriangleMath.GetEqTriangleAltitude(sideLength) - centroidAltitude),
        vector2_3 + vector2_1 * num
      };
    }

    public static float CalcRATriangleHypotenuse(float side0, float side1)
    {
      return Mathf.Sqrt((float) ((double) side0 * (double) side0 + (double) side1 * (double) side1));
    }

    public static float CalcRATriangleHypotenuse(Vector2 sides)
    {
      return Mathf.Sqrt((float) ((double) sides.x * (double) sides.x + (double) sides.y * (double) sides.y));
    }

    public static float CalcRATriangleAltitude(Vector2 sides)
    {
      return sides.x * sides.y / Mathf.Sqrt((float) ((double) sides.x * (double) sides.x + (double) sides.y * (double) sides.y));
    }

    public static List<Vector3> CalcRATriangle3DPoints(
      Vector3 rightAngleCorner,
      float xLength,
      float yLength,
      Quaternion triangleRotation)
    {
      Vector3 vector3_1 = triangleRotation * Vector3.right;
      Vector3 vector3_2 = triangleRotation * Vector3.up;
      return new List<Vector3>()
      {
        rightAngleCorner,
        rightAngleCorner + vector3_2 * yLength,
        rightAngleCorner + vector3_1 * xLength
      };
    }

    public static List<Vector2> CalcRATriangle2DPoints(
      Vector2 rightAngleCorner,
      float xLength,
      float yLength,
      float degreeTriRotation)
    {
      Quaternion quaternion = Quaternion.AngleAxis(degreeTriRotation, Vector3.forward);
      Vector2 vector2_1 = (Vector2) (quaternion * (Vector3) Vector2.right);
      Vector2 vector2_2 = (Vector2) (quaternion * (Vector3) Vector2.up);
      return new List<Vector2>()
      {
        rightAngleCorner,
        rightAngleCorner + vector2_2 * yLength,
        rightAngleCorner + vector2_1 * xLength
      };
    }

    public static OBB Calc3DTriangleOBB(
      Vector3 p0,
      Vector3 p1,
      Vector3 p2,
      Vector3 normal,
      TriangleEpsilon epsilon = default (TriangleEpsilon))
    {
      Vector3 vector3_1 = p0;
      Vector3 vector3_2 = p1;
      Vector3 vector3_3 = p2;
      float f1 = (p0 - p1).sqrMagnitude + 1E-05f;
      float sqrMagnitude1 = (p1 - p2).sqrMagnitude;
      if ((double) sqrMagnitude1 > (double) f1)
      {
        vector3_1 = p1;
        vector3_2 = p2;
        vector3_3 = p0;
        f1 = sqrMagnitude1;
      }
      float sqrMagnitude2 = (p2 - p0).sqrMagnitude;
      if ((double) sqrMagnitude2 > (double) f1)
      {
        vector3_1 = p2;
        vector3_2 = p0;
        vector3_3 = p1;
        f1 = sqrMagnitude2;
      }
      OBB obb = new OBB(Quaternion.LookRotation((vector3_2 - vector3_1).normalized, normal));
      float num = 2f * epsilon.AreaEps;
      float z = Mathf.Sqrt(f1) + num;
      float f2 = Vector3.Dot(obb.Right, vector3_3 - vector3_1);
      float x = Mathf.Abs(f2) + num;
      float y = epsilon.ExtrudeEps * 2f;
      obb.Size = new Vector3(x, y, z);
      Vector3 vector3_4 = obb.Right * Mathf.Sign(f2);
      obb.Center = vector3_4 * x * 0.5f - vector3_4 * epsilon.AreaEps + vector3_1 - obb.Look * epsilon.AreaEps + obb.Look * z * 0.5f;
      return obb;
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 p0,
      Vector3 p1,
      Vector3 p2,
      TriangleEpsilon epsilon = default (TriangleEpsilon))
    {
      t = 0.0f;
      Plane plane = new Plane(p0, p1, p2);
      float enter;
      if (plane.Raycast(ray, out enter) && TriangleMath.Contains3DPoint(ray.GetPoint(enter), false, p0, p1, p2, epsilon))
      {
        t = enter;
        return true;
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(plane.normal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      OBB obb = TriangleMath.Calc3DTriangleOBB(p0, p1, p2, plane.normal, epsilon);
      return BoxMath.Raycast(ray, obb.Center, obb.Size, obb.Rotation);
    }

    public static bool RaycastWire(
      Ray ray,
      out float t,
      Vector3 p0,
      Vector3 p1,
      Vector3 p2,
      TriangleEpsilon epsilon = default (TriangleEpsilon))
    {
      t = 0.0f;
      Plane plane = new Plane(p0, p1, p2);
      float enter;
      if (plane.Raycast(ray, out enter))
      {
        Vector3 point = ray.GetPoint(enter);
        if ((double) point.GetDistanceToSegment(p0, p1) <= (double) epsilon.WireEps)
        {
          t = enter;
          return true;
        }
        if ((double) point.GetDistanceToSegment(p1, p2) <= (double) epsilon.WireEps)
        {
          t = enter;
          return true;
        }
        if ((double) point.GetDistanceToSegment(p2, p0) <= (double) epsilon.WireEps)
        {
          t = enter;
          return true;
        }
      }
      if ((double) epsilon.ExtrudeEps == 0.0 || (double) ray.direction.AbsDot(plane.normal) >= (double) ExtrudeEpsThreshold.Get)
        return false;
      OBB obb = TriangleMath.Calc3DTriangleOBB(p0, p1, p2, plane.normal, epsilon);
      return BoxMath.Raycast(ray, obb.Center, obb.Size, obb.Rotation);
    }

    public static bool Contains3DPoint(
      Vector3 point,
      bool checkOnPlane,
      Vector3 p0,
      Vector3 p1,
      Vector3 p2,
      TriangleEpsilon epsilon = default (TriangleEpsilon))
    {
      Vector3 lhs1 = p1 - p0;
      Vector3 lhs2 = p2 - p1;
      Vector3 lhs3 = p0 - p2;
      Vector3 normalized1 = Vector3.Cross(lhs1, -lhs3).normalized;
      if (checkOnPlane && (double) Mathf.Abs(Vector3.Dot(point - p0, normalized1)) > (double) epsilon.ExtrudeEps)
        return false;
      Vector3 normalized2 = Vector3.Cross(lhs1, normalized1).normalized;
      if ((double) Vector3.Dot(point - p0, normalized2) > (double) epsilon.AreaEps)
        return false;
      Vector3 normalized3 = Vector3.Cross(lhs2, normalized1).normalized;
      if ((double) Vector3.Dot(point - p1, normalized3) > (double) epsilon.AreaEps)
        return false;
      Vector3 normalized4 = Vector3.Cross(lhs3, normalized1).normalized;
      return (double) Vector3.Dot(point - p2, normalized4) <= (double) epsilon.AreaEps;
    }

    public static bool Contains2DPoint(
      Vector2 point,
      Vector2 p0,
      Vector2 p1,
      Vector2 p2,
      TriangleEpsilon epsilon = default (TriangleEpsilon))
    {
      return TriangleMath.Contains3DPoint((Vector3) point, false, (Vector3) p0, (Vector3) p1, (Vector3) p2, epsilon);
    }

    static TriangleMath()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TriangleMath._eqTriangleAltFactor = Mathf.Sqrt(3f) * 0.5f;
    }
  }
}
