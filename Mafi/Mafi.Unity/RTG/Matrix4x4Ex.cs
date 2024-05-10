// Decompiled with JetBrains decompiler
// Type: RTG.Matrix4x4Ex
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class Matrix4x4Ex
  {
    public static Matrix4x4 GetInverse(this Matrix4x4 mtx)
    {
      float f = (float) ((double) mtx.m00 * ((double) mtx.m11 * ((double) mtx.m22 * (double) mtx.m33 - (double) mtx.m23 * (double) mtx.m32) - (double) mtx.m12 * ((double) mtx.m21 * (double) mtx.m33 - (double) mtx.m23 * (double) mtx.m31) + (double) mtx.m13 * ((double) mtx.m21 * (double) mtx.m32 - (double) mtx.m22 * (double) mtx.m31)) - (double) mtx.m01 * ((double) mtx.m10 * ((double) mtx.m22 * (double) mtx.m33 - (double) mtx.m23 * (double) mtx.m32) - (double) mtx.m12 * ((double) mtx.m20 * (double) mtx.m33 - (double) mtx.m23 * (double) mtx.m30) + (double) mtx.m13 * ((double) mtx.m20 * (double) mtx.m32 - (double) mtx.m22 * (double) mtx.m30)) + (double) mtx.m02 * ((double) mtx.m10 * ((double) mtx.m21 * (double) mtx.m33 - (double) mtx.m23 - (double) mtx.m31) - (double) mtx.m11 * ((double) mtx.m20 * (double) mtx.m33 - (double) mtx.m23 * (double) mtx.m30) + (double) mtx.m13 * ((double) mtx.m20 * (double) mtx.m31 - (double) mtx.m21 * (double) mtx.m30)) - (double) mtx.m03 * ((double) mtx.m10 * ((double) mtx.m21 * (double) mtx.m32 - (double) mtx.m22 * (double) mtx.m31) - (double) mtx.m11 * ((double) mtx.m20 * (double) mtx.m32 - (double) mtx.m22 * (double) mtx.m30) + (double) mtx.m12 * ((double) mtx.m20 * (double) mtx.m31 - (double) mtx.m21 * (double) mtx.m30)));
      if ((double) Mathf.Abs(f) < 9.9999997473787516E-06)
        return mtx;
      float num = 1f / f;
      float m00 = mtx.m00;
      float m10 = mtx.m10;
      float m20 = mtx.m20;
      float m30 = mtx.m30;
      float m01 = mtx.m01;
      float m11 = mtx.m11;
      float m21 = mtx.m21;
      float m31 = mtx.m31;
      float m02 = mtx.m02;
      float m12 = mtx.m12;
      float m22 = mtx.m22;
      float m32 = mtx.m32;
      float m03 = mtx.m03;
      float m13 = mtx.m13;
      float m23 = mtx.m23;
      float m33 = mtx.m33;
      mtx.m00 = num * (float) ((double) m11 * ((double) m22 * (double) m33 - (double) m32 * (double) m23) - (double) m21 * ((double) m12 * (double) m33 - (double) m13 * (double) m32) + (double) m31 * ((double) m12 * (double) m23 - (double) m22 * (double) m13));
      mtx.m01 = (float) (-(double) num * ((double) m01 * ((double) m22 * (double) m33 - (double) m32 * (double) m23) - (double) m21 * ((double) m02 * (double) m33 - (double) m32 * (double) m03) + (double) m31 * ((double) m02 * (double) m23 - (double) m32 * (double) m03)));
      mtx.m02 = num * (float) ((double) m01 * ((double) m12 * (double) m33 - (double) m32 * (double) m13) - (double) m11 * ((double) m02 * (double) m33 - (double) m32 * (double) m03) + (double) m31 * ((double) m02 * (double) m13 - (double) m12 * (double) m03));
      mtx.m03 = (float) (-(double) num * ((double) m01 * ((double) m12 * (double) m23 - (double) m22 * (double) m13) - (double) m11 * ((double) m02 * (double) m23 - (double) m22 * (double) m03) + (double) m21 * ((double) m02 * (double) m13 - (double) m12 * (double) m03)));
      mtx.m10 = (float) (-(double) num * ((double) m10 * ((double) m22 * (double) m33 - (double) m32 * (double) m23) - (double) m20 * ((double) m12 * (double) m33 - (double) m32 * (double) m13) + (double) m30 * ((double) m12 * (double) m23 - (double) m13 * (double) m22)));
      mtx.m11 = num * (float) ((double) m00 * ((double) m22 * (double) m33 - (double) m32 * (double) m23) - (double) m20 * ((double) m02 * (double) m33 - (double) m32 * (double) m03) + (double) m30 * ((double) m02 * (double) m23 - (double) m22 * (double) m03));
      mtx.m12 = (float) (-(double) num * ((double) m00 * ((double) m12 * (double) m33 - (double) m32 * (double) m13) - (double) m10 * ((double) m02 * (double) m33 - (double) m32 * (double) m03) + (double) m30 * ((double) m02 * (double) m13 - (double) m12 * (double) m03)));
      mtx.m13 = num * (float) ((double) m00 * ((double) m12 * (double) m23 - (double) m22 * (double) m13) - (double) m10 * ((double) m02 * (double) m23 - (double) m22 * (double) m03) + (double) m20 * ((double) m02 * (double) m13 - (double) m12 * (double) m03));
      mtx.m20 = num * (float) ((double) m10 * ((double) m21 * (double) m33 - (double) m31 * (double) m23) - (double) m20 * ((double) m11 * (double) m33 - (double) m31 * (double) m13) + (double) m30 * ((double) m11 * (double) m23 - (double) m21 * (double) m13));
      mtx.m21 = (float) (-(double) num * ((double) m00 * ((double) m21 * (double) m33 - (double) m31 * (double) m23) - (double) m20 * ((double) m01 * (double) m33 - (double) m31 * (double) m03) + (double) m30 * ((double) m01 * (double) m23 - (double) m21 * (double) m03)));
      mtx.m22 = num * (float) ((double) m00 * ((double) m11 * (double) m33 - (double) m31 * (double) m13) - (double) m10 * ((double) m01 * (double) m33 - (double) m31 * (double) m03) + (double) m30 * ((double) m01 * (double) m13 - (double) m11 * (double) m03));
      mtx.m23 = (float) (-(double) num * ((double) m00 * ((double) m11 * (double) m23 - (double) m21 * (double) m13) - (double) m10 * ((double) m01 * (double) m23 - (double) m21 * (double) m03) + (double) m20 * ((double) m01 * (double) m13 - (double) m11 * (double) m03)));
      mtx.m30 = (float) (-(double) num * ((double) m10 * ((double) m21 * (double) m32 - (double) m32 * (double) m12) - (double) m20 * ((double) m11 * (double) m32 - (double) m31 * (double) m12) + (double) m30 * ((double) m11 * (double) m22 - (double) m21 * (double) m12)));
      mtx.m31 = num * (float) ((double) m00 * ((double) m21 * (double) m32 - (double) m31 * (double) m12) - (double) m20 * ((double) m01 * (double) m32 - (double) m31 * (double) m02) + (double) m30 * ((double) m01 * (double) m22 - (double) m21 * (double) m02));
      mtx.m32 = (float) (-(double) num * ((double) m00 * ((double) m11 * (double) m32 - (double) m31 * (double) m12) - (double) m10 * ((double) m01 * (double) m32 - (double) m31 * (double) m02) + (double) m30 * ((double) m01 * (double) m12 - (double) m11 * (double) m02)));
      mtx.m33 = num * (float) ((double) m00 * ((double) m11 * (double) m22 - (double) m21 * (double) m12) - (double) m10 * ((double) m01 * (double) m22 - (double) m21 * (double) m02) + (double) m20 * ((double) m01 * (double) m12 - (double) m11 * (double) m02));
      return mtx;
    }

    public static Matrix4x4 GetRelativeTransform(
      this Matrix4x4 matrix,
      Matrix4x4 referenceTransform)
    {
      return referenceTransform.inverse * matrix;
    }

    public static Matrix4x4 Translation(Vector3 translation)
    {
      Matrix4x4 identity = Matrix4x4.identity;
      identity.SetColumn(3, Vector4Ex.FromVector3(translation, 1f));
      return identity;
    }

    public static Matrix4x4 RotationMatrixFromRightUp(Vector3 right, Vector3 up)
    {
      right.Normalize();
      up.Normalize();
      Vector3 normalized = Vector3.Cross(up, right).normalized;
      Matrix4x4 identity = Matrix4x4.identity;
      identity[0, 0] = right.x;
      identity[1, 0] = right.y;
      identity[2, 0] = right.z;
      identity[0, 1] = up.x;
      identity[1, 1] = up.y;
      identity[2, 1] = up.z;
      identity[0, 2] = normalized.x;
      identity[1, 2] = normalized.y;
      identity[2, 2] = normalized.z;
      return identity;
    }

    public static Vector3 GetTranslation(this Matrix4x4 matrix) => (Vector3) matrix.GetColumn(3);

    public static Vector3 GetScale(this Matrix4x4 matrix)
    {
      return new Vector3(((Vector3) matrix.GetColumn(0)).magnitude, ((Vector3) matrix.GetColumn(1)).magnitude, ((Vector3) matrix.GetColumn(2)).magnitude);
    }

    public static Vector3 GetNormalizedAxis(this Matrix4x4 matrix, int axisIndex)
    {
      return Vector3.Normalize((Vector3) matrix.GetColumn(axisIndex));
    }

    public static Vector3[] GetNormalizedAxes(this Matrix4x4 matrix)
    {
      return new Vector3[3]
      {
        matrix.GetNormalizedAxis(0),
        matrix.GetNormalizedAxis(1),
        matrix.GetNormalizedAxis(2)
      };
    }

    public static List<Vector3> TransformPoints(this Matrix4x4 matrix, List<Vector3> points)
    {
      if (points.Count == 0)
        return new List<Vector3>();
      List<Vector3> vector3List = new List<Vector3>(points.Count);
      foreach (Vector3 point in points)
        vector3List.Add(matrix.MultiplyPoint(point));
      return vector3List;
    }
  }
}
