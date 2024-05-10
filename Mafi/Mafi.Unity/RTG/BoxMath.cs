// Decompiled with JetBrains decompiler
// Type: RTG.BoxMath
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
  public static class BoxMath
  {
    private static List<BoxFace> _allBoxFaces;
    private static Vector3[] A;
    private static Vector3[] B;
    private static float[,] R;
    private static float[,] absR;

    static BoxMath()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BoxMath._allBoxFaces = new List<BoxFace>();
      BoxMath.A = new Vector3[3];
      BoxMath.B = new Vector3[3];
      BoxMath.R = new float[3, 3];
      BoxMath.absR = new float[3, 3];
      BoxMath._allBoxFaces.Add(BoxFace.Front);
      BoxMath._allBoxFaces.Add(BoxFace.Back);
      BoxMath._allBoxFaces.Add(BoxFace.Left);
      BoxMath._allBoxFaces.Add(BoxFace.Right);
      BoxMath._allBoxFaces.Add(BoxFace.Bottom);
      BoxMath._allBoxFaces.Add(BoxFace.Top);
    }

    public static List<BoxFace> AllBoxFaces
    {
      get => new List<BoxFace>((IEnumerable<BoxFace>) BoxMath._allBoxFaces);
    }

    public static int GetFaceAxisIndex(BoxFace face)
    {
      switch (face)
      {
        case BoxFace.Front:
        case BoxFace.Back:
          return 2;
        case BoxFace.Left:
        case BoxFace.Right:
          return 0;
        case BoxFace.Bottom:
        case BoxFace.Top:
          return 1;
        default:
          return -1;
      }
    }

    public static BoxFaceDesc GetFaceClosestToPoint(
      Vector3 point,
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation)
    {
      float num = float.MaxValue;
      Plane plane1 = new Plane();
      BoxFace boxFace = BoxFace.Front;
      foreach (BoxFace allBoxFace in BoxMath.AllBoxFaces)
      {
        Plane plane2 = BoxMath.CalcBoxFacePlane(boxCenter, boxSize, boxRotation, allBoxFace);
        float absDistanceToPoint = plane2.GetAbsDistanceToPoint(point);
        if ((double) absDistanceToPoint < (double) num)
        {
          boxFace = allBoxFace;
          plane1 = plane2;
          num = absDistanceToPoint;
        }
      }
      return new BoxFaceDesc()
      {
        Face = boxFace,
        Plane = plane1,
        Center = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, boxRotation, boxFace)
      };
    }

    public static BoxFaceDesc GetFaceClosestToPoint(
      Vector3 point,
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation,
      Vector3 viewVector)
    {
      float num = float.MaxValue;
      Plane plane1 = new Plane();
      BoxFace boxFace = BoxFace.Front;
      foreach (BoxFace allBoxFace in BoxMath.AllBoxFaces)
      {
        Plane plane2 = BoxMath.CalcBoxFacePlane(boxCenter, boxSize, boxRotation, allBoxFace);
        if ((double) Vector3.Dot(viewVector, plane2.normal) < 0.0)
        {
          float absDistanceToPoint = plane2.GetAbsDistanceToPoint(point);
          if ((double) absDistanceToPoint < (double) num)
          {
            boxFace = allBoxFace;
            plane1 = plane2;
            num = absDistanceToPoint;
          }
        }
      }
      return new BoxFaceDesc()
      {
        Face = boxFace,
        Plane = plane1,
        Center = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, boxRotation, boxFace)
      };
    }

    public static BoxFace GetMostAlignedFace(
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation,
      Vector3 direction)
    {
      List<BoxFace> allBoxFaces = BoxMath.AllBoxFaces;
      int index1 = 0;
      float num1 = Vector3.Dot(direction, BoxMath.CalcBoxFaceNormal(boxCenter, boxSize, boxRotation, allBoxFaces[0]));
      for (int index2 = 1; index2 < allBoxFaces.Count; ++index2)
      {
        float num2 = Vector3.Dot(direction, BoxMath.CalcBoxFaceNormal(boxCenter, boxSize, boxRotation, allBoxFaces[index2]));
        if ((double) num2 > (double) num1)
        {
          num1 = num2;
          index1 = index2;
        }
      }
      return allBoxFaces[index1];
    }

    public static Vector3 CalcBoxFaceSize(Vector3 boxSize, BoxFace boxFace)
    {
      Vector3 vector3 = boxSize;
      switch (boxFace)
      {
        case BoxFace.Front:
        case BoxFace.Back:
          vector3.z = 0.0f;
          break;
        case BoxFace.Left:
        case BoxFace.Right:
          vector3.x = 0.0f;
          break;
        default:
          vector3.y = 0.0f;
          break;
      }
      return vector3;
    }

    public static BoxFaceAreaDesc GetBoxFaceAreaDesc(Vector3 boxSize, BoxFace boxFace)
    {
      switch (boxFace)
      {
        case BoxFace.Front:
        case BoxFace.Back:
          float area1 = boxSize.x * boxSize.y;
          return (double) area1 < 9.9999999747524271E-07 ? new BoxFaceAreaDesc(BoxFaceAreaType.Line, Mathf.Max(boxSize.x, boxSize.y)) : new BoxFaceAreaDesc(BoxFaceAreaType.Quad, area1);
        case BoxFace.Left:
        case BoxFace.Right:
          float area2 = boxSize.y * boxSize.z;
          return (double) area2 < 9.9999999747524271E-07 ? new BoxFaceAreaDesc(BoxFaceAreaType.Line, Mathf.Max(boxSize.y, boxSize.z)) : new BoxFaceAreaDesc(BoxFaceAreaType.Quad, area2);
        default:
          float area3 = boxSize.x * boxSize.z;
          return (double) area3 < 9.9999999747524271E-07 ? new BoxFaceAreaDesc(BoxFaceAreaType.Line, Mathf.Max(boxSize.x, boxSize.z)) : new BoxFaceAreaDesc(BoxFaceAreaType.Quad, area3);
      }
    }

    public static Plane CalcBoxFacePlane(
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation,
      BoxFace boxFace)
    {
      Vector3 vector3 = boxSize * 0.5f;
      Vector3 inNormal1 = boxRotation * Vector3.right;
      Vector3 inNormal2 = boxRotation * Vector3.up;
      Vector3 inNormal3 = boxRotation * Vector3.forward;
      switch (boxFace)
      {
        case BoxFace.Front:
          return new Plane(-inNormal3, boxCenter - inNormal3 * vector3.z);
        case BoxFace.Back:
          return new Plane(inNormal3, boxCenter + inNormal3 * vector3.z);
        case BoxFace.Left:
          return new Plane(-inNormal1, boxCenter - inNormal1 * vector3.x);
        case BoxFace.Right:
          return new Plane(inNormal1, boxCenter + inNormal1 * vector3.x);
        case BoxFace.Bottom:
          return new Plane(-inNormal2, boxCenter - inNormal2 * vector3.y);
        default:
          return new Plane(inNormal2, boxCenter + inNormal2 * vector3.y);
      }
    }

    public static Vector3 CalcBoxFaceNormal(
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation,
      BoxFace boxFace)
    {
      Vector3 vector3_1 = boxRotation * Vector3.right;
      Vector3 vector3_2 = boxRotation * Vector3.up;
      Vector3 vector3_3 = boxRotation * Vector3.forward;
      switch (boxFace)
      {
        case BoxFace.Front:
          return -vector3_3;
        case BoxFace.Back:
          return vector3_3;
        case BoxFace.Left:
          return -vector3_1;
        case BoxFace.Right:
          return vector3_1;
        case BoxFace.Bottom:
          return -vector3_2;
        default:
          return vector3_2;
      }
    }

    public static Vector3 CalcBoxFaceCenter(
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation,
      BoxFace boxFace)
    {
      Vector3 vector3_1 = boxSize * 0.5f;
      Vector3 vector3_2 = boxRotation * Vector3.right;
      Vector3 vector3_3 = boxRotation * Vector3.up;
      Vector3 vector3_4 = boxRotation * Vector3.forward;
      switch (boxFace)
      {
        case BoxFace.Front:
          return boxCenter - vector3_4 * vector3_1.z;
        case BoxFace.Back:
          return boxCenter + vector3_4 * vector3_1.z;
        case BoxFace.Left:
          return boxCenter - vector3_2 * vector3_1.x;
        case BoxFace.Right:
          return boxCenter + vector3_2 * vector3_1.x;
        case BoxFace.Bottom:
          return boxCenter - vector3_3 * vector3_1.y;
        default:
          return boxCenter + vector3_3 * vector3_1.y;
      }
    }

    public static List<Vector3> CalcBoxCornerPoints(
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation)
    {
      Vector3 vector3_1 = boxSize * 0.5f;
      Vector3 vector3_2 = boxRotation * Vector3.right;
      Vector3 vector3_3 = boxRotation * Vector3.up;
      Vector3 vector3_4 = boxRotation * Vector3.forward;
      Vector3[] collection = new Vector3[8];
      Vector3 vector3_5 = boxCenter - vector3_4 * vector3_1.z;
      collection[0] = vector3_5 - vector3_2 * vector3_1.x + vector3_3 * vector3_1.y;
      collection[1] = vector3_5 + vector3_2 * vector3_1.x + vector3_3 * vector3_1.y;
      collection[2] = vector3_5 + vector3_2 * vector3_1.x - vector3_3 * vector3_1.y;
      collection[3] = vector3_5 - vector3_2 * vector3_1.x - vector3_3 * vector3_1.y;
      Vector3 vector3_6 = boxCenter + vector3_4 * vector3_1.z;
      collection[4] = vector3_6 + vector3_2 * vector3_1.x + vector3_3 * vector3_1.y;
      collection[5] = vector3_6 - vector3_2 * vector3_1.x + vector3_3 * vector3_1.y;
      collection[6] = vector3_6 - vector3_2 * vector3_1.x - vector3_3 * vector3_1.y;
      collection[7] = vector3_6 + vector3_2 * vector3_1.x - vector3_3 * vector3_1.y;
      return new List<Vector3>((IEnumerable<Vector3>) collection);
    }

    public static void TransformBox(
      Vector3 boxCenter,
      Vector3 boxSize,
      Matrix4x4 transformMatrix,
      out Vector3 newBoxCenter,
      out Vector3 newBoxSize)
    {
      Vector3 column1 = (Vector3) transformMatrix.GetColumn(0);
      Vector3 column2 = (Vector3) transformMatrix.GetColumn(1);
      Vector3 column3 = (Vector3) transformMatrix.GetColumn(2);
      Vector3 vector3_1 = boxSize * 0.5f;
      Vector3 vector3_2 = column1 * vector3_1.x;
      Vector3 vector3_3 = column2 * vector3_1.y;
      Vector3 vector3_4 = column3 * vector3_1.z;
      float x = Mathf.Abs(vector3_2.x) + Mathf.Abs(vector3_3.x) + Mathf.Abs(vector3_4.x);
      float y = Mathf.Abs(vector3_2.y) + Mathf.Abs(vector3_3.y) + Mathf.Abs(vector3_4.y);
      float z = Mathf.Abs(vector3_2.z) + Mathf.Abs(vector3_3.z) + Mathf.Abs(vector3_4.z);
      newBoxCenter = transformMatrix.MultiplyPoint(boxCenter);
      newBoxSize = new Vector3(x, y, z) * 2f;
    }

    public static bool BoxIntersectsBox(
      Vector3 center0,
      Vector3 size0,
      Quaternion rotation0,
      Vector3 center1,
      Vector3 size1,
      Quaternion rotation1)
    {
      BoxMath.A[0] = rotation0 * Vector3.right;
      BoxMath.A[1] = rotation0 * Vector3.up;
      BoxMath.A[2] = rotation0 * Vector3.forward;
      BoxMath.B[0] = rotation1 * Vector3.right;
      BoxMath.B[1] = rotation1 * Vector3.up;
      BoxMath.B[2] = rotation1 * Vector3.forward;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        for (int index2 = 0; index2 < 3; ++index2)
          BoxMath.R[index1, index2] = Vector3.Dot(BoxMath.A[index1], BoxMath.B[index2]);
      }
      Vector3 vector3_1 = size0 * 0.5f;
      Vector3 vector3_2 = new Vector3(vector3_1.x, vector3_1.y, vector3_1.z);
      Vector3 vector3_3 = size1 * 0.5f;
      Vector3 vector3_4 = new Vector3(vector3_3.x, vector3_3.y, vector3_3.z);
      for (int index3 = 0; index3 < 3; ++index3)
      {
        for (int index4 = 0; index4 < 3; ++index4)
          BoxMath.absR[index3, index4] = Mathf.Abs(BoxMath.R[index3, index4]) + 0.0001f;
      }
      Vector3 lhs = center1 - center0;
      Vector3 vector3_5 = new Vector3(Vector3.Dot(lhs, BoxMath.A[0]), Vector3.Dot(lhs, BoxMath.A[1]), Vector3.Dot(lhs, BoxMath.A[2]));
      for (int index = 0; index < 3; ++index)
      {
        float num = (float) ((double) vector3_4[0] * (double) BoxMath.absR[index, 0] + (double) vector3_4[1] * (double) BoxMath.absR[index, 1] + (double) vector3_4[2] * (double) BoxMath.absR[index, 2]);
        if ((double) Mathf.Abs(vector3_5[index]) > (double) vector3_2[index] + (double) num)
          return false;
      }
      for (int index = 0; index < 3; ++index)
      {
        float num = (float) ((double) vector3_2[0] * (double) BoxMath.absR[0, index] + (double) vector3_2[1] * (double) BoxMath.absR[1, index] + (double) vector3_2[2] * (double) BoxMath.absR[2, index]);
        if ((double) Mathf.Abs((float) ((double) vector3_5[0] * (double) BoxMath.R[0, index] + (double) vector3_5[1] * (double) BoxMath.R[1, index] + (double) vector3_5[2] * (double) BoxMath.R[2, index])) > (double) num + (double) vector3_4[index])
          return false;
      }
      float num1 = (float) ((double) vector3_2[1] * (double) BoxMath.absR[2, 0] + (double) vector3_2[2] * (double) BoxMath.absR[1, 0]);
      float num2 = (float) ((double) vector3_4[1] * (double) BoxMath.absR[0, 2] + (double) vector3_4[2] * (double) BoxMath.absR[0, 1]);
      if ((double) Mathf.Abs((float) ((double) vector3_5[2] * (double) BoxMath.R[1, 0] - (double) vector3_5[1] * (double) BoxMath.R[2, 0])) > (double) num1 + (double) num2)
        return false;
      float num3 = (float) ((double) vector3_2[1] * (double) BoxMath.absR[2, 1] + (double) vector3_2[2] * (double) BoxMath.absR[1, 1]);
      float num4 = (float) ((double) vector3_4[0] * (double) BoxMath.absR[0, 2] + (double) vector3_4[2] * (double) BoxMath.absR[0, 0]);
      if ((double) Mathf.Abs((float) ((double) vector3_5[2] * (double) BoxMath.R[1, 1] - (double) vector3_5[1] * (double) BoxMath.R[2, 1])) > (double) num3 + (double) num4)
        return false;
      float num5 = (float) ((double) vector3_2[1] * (double) BoxMath.absR[2, 2] + (double) vector3_2[2] * (double) BoxMath.absR[1, 2]);
      float num6 = (float) ((double) vector3_4[0] * (double) BoxMath.absR[0, 1] + (double) vector3_4[1] * (double) BoxMath.absR[0, 0]);
      if ((double) Mathf.Abs((float) ((double) vector3_5[2] * (double) BoxMath.R[1, 2] - (double) vector3_5[1] * (double) BoxMath.R[2, 2])) > (double) num5 + (double) num6)
        return false;
      float num7 = (float) ((double) vector3_2[0] * (double) BoxMath.absR[2, 0] + (double) vector3_2[2] * (double) BoxMath.absR[0, 0]);
      float num8 = (float) ((double) vector3_4[1] * (double) BoxMath.absR[1, 2] + (double) vector3_4[2] * (double) BoxMath.absR[1, 1]);
      if ((double) Mathf.Abs((float) ((double) vector3_5[0] * (double) BoxMath.R[2, 0] - (double) vector3_5[2] * (double) BoxMath.R[0, 0])) > (double) num7 + (double) num8)
        return false;
      float num9 = (float) ((double) vector3_2[0] * (double) BoxMath.absR[2, 1] + (double) vector3_2[2] * (double) BoxMath.absR[0, 1]);
      float num10 = (float) ((double) vector3_4[0] * (double) BoxMath.absR[1, 2] + (double) vector3_4[2] * (double) BoxMath.absR[1, 0]);
      if ((double) Mathf.Abs((float) ((double) vector3_5[0] * (double) BoxMath.R[2, 1] - (double) vector3_5[2] * (double) BoxMath.R[0, 1])) > (double) num9 + (double) num10)
        return false;
      float num11 = (float) ((double) vector3_2[0] * (double) BoxMath.absR[2, 2] + (double) vector3_2[2] * (double) BoxMath.absR[0, 2]);
      float num12 = (float) ((double) vector3_4[0] * (double) BoxMath.absR[1, 1] + (double) vector3_4[1] * (double) BoxMath.absR[1, 0]);
      if ((double) Mathf.Abs((float) ((double) vector3_5[0] * (double) BoxMath.R[2, 2] - (double) vector3_5[2] * (double) BoxMath.R[0, 2])) > (double) num11 + (double) num12)
        return false;
      float num13 = (float) ((double) vector3_2[0] * (double) BoxMath.absR[1, 0] + (double) vector3_2[1] * (double) BoxMath.absR[0, 0]);
      float num14 = (float) ((double) vector3_4[1] * (double) BoxMath.absR[2, 2] + (double) vector3_4[2] * (double) BoxMath.absR[2, 1]);
      if ((double) Mathf.Abs((float) ((double) vector3_5[1] * (double) BoxMath.R[0, 0] - (double) vector3_5[0] * (double) BoxMath.R[1, 0])) > (double) num13 + (double) num14)
        return false;
      float num15 = (float) ((double) vector3_2[0] * (double) BoxMath.absR[1, 1] + (double) vector3_2[1] * (double) BoxMath.absR[0, 1]);
      float num16 = (float) ((double) vector3_4[0] * (double) BoxMath.absR[2, 2] + (double) vector3_4[2] * (double) BoxMath.absR[2, 0]);
      if ((double) Mathf.Abs((float) ((double) vector3_5[1] * (double) BoxMath.R[0, 1] - (double) vector3_5[0] * (double) BoxMath.R[1, 1])) > (double) num15 + (double) num16)
        return false;
      float num17 = (float) ((double) vector3_2[0] * (double) BoxMath.absR[1, 2] + (double) vector3_2[1] * (double) BoxMath.absR[0, 2]);
      float num18 = (float) ((double) vector3_4[0] * (double) BoxMath.absR[2, 1] + (double) vector3_4[1] * (double) BoxMath.absR[2, 0]);
      return (double) Mathf.Abs((float) ((double) vector3_5[1] * (double) BoxMath.R[0, 2] - (double) vector3_5[0] * (double) BoxMath.R[1, 2])) <= (double) num17 + (double) num18;
    }

    public static Vector3 CalcBoxPtClosestToPt(
      Vector3 point,
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation)
    {
      Vector3 rhs = point - boxCenter;
      Vector3[] vector3Array = new Vector3[3]
      {
        boxRotation * Vector3.right,
        boxRotation * Vector3.up,
        boxRotation * Vector3.forward
      };
      Vector3 vector3 = boxSize * 0.5f;
      Vector3 pt = boxCenter;
      for (int index = 0; index < 3; ++index)
      {
        float num = Vector3.Dot(vector3Array[index], rhs);
        if ((double) num > (double) vector3[index])
          num = vector3[index];
        else if ((double) num < -(double) vector3[index])
          num = -vector3[index];
        pt += vector3Array[index] * num;
      }
      return pt;
    }

    public static bool Raycast(
      Ray ray,
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation,
      BoxEpsilon epsilon = default (BoxEpsilon))
    {
      return BoxMath.Raycast(ray, out float _, boxCenter, boxSize, boxRotation, epsilon);
    }

    public static bool Raycast(
      Ray ray,
      out float t,
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation,
      BoxEpsilon epsilon = default (BoxEpsilon))
    {
      t = 0.0f;
      boxSize += epsilon.SizeEps;
      int num = 0;
      if ((double) boxSize.x < 9.9999999747524271E-07)
        ++num;
      if ((double) boxSize.y < 9.9999999747524271E-07)
        ++num;
      if ((double) boxSize.z < 9.9999999747524271E-07)
        ++num;
      if (num > 1)
        return false;
      if (num == 1)
      {
        if ((double) boxSize.x < 9.9999999747524271E-07)
        {
          Vector3 quadRight = boxRotation * Vector3.forward;
          Vector3 quadUp = boxRotation * Vector3.up;
          return QuadMath.Raycast(ray, out t, boxCenter, boxSize.z, boxSize.y, quadRight, quadUp);
        }
        if ((double) boxSize.y < 9.9999999747524271E-07)
        {
          Vector3 quadRight = boxRotation * Vector3.right;
          Vector3 quadUp = boxRotation * Vector3.forward;
          return QuadMath.Raycast(ray, out t, boxCenter, boxSize.x, boxSize.z, quadRight, quadUp);
        }
        Vector3 quadRight1 = boxRotation * Vector3.right;
        Vector3 quadUp1 = boxRotation * Vector3.up;
        return QuadMath.Raycast(ray, out t, boxCenter, boxSize.x, boxSize.y, quadRight1, quadUp1);
      }
      Matrix4x4 transformMatrix = Matrix4x4.TRS(boxCenter, boxRotation, boxSize);
      Ray ray1 = ray.InverseTransform(transformMatrix);
      if ((double) ray1.direction.sqrMagnitude == 0.0 || !new Bounds(Vector3.zero, Vector3.one).IntersectRay(ray1, out t))
        return false;
      Vector3 vector3 = transformMatrix.MultiplyPoint(ray1.GetPoint(t));
      t = (vector3 - ray.origin).magnitude;
      return true;
    }

    public static bool ContainsPoint(
      Vector3 point,
      Vector3 boxCenter,
      Vector3 boxSize,
      Quaternion boxRotation,
      BoxEpsilon epsilon = default (BoxEpsilon))
    {
      boxSize += epsilon.SizeEps;
      point = Matrix4x4.TRS(boxCenter, boxRotation, boxSize).inverse.MultiplyPoint(point);
      return (double) point.x >= -0.5 && (double) point.x <= 0.5 && (double) point.y >= -0.5 && (double) point.y <= 0.5 && (double) point.z >= -0.5 && (double) point.z <= 0.5;
    }
  }
}
