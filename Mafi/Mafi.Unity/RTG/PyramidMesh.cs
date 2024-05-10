// Decompiled with JetBrains decompiler
// Type: RTG.PyramidMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class PyramidMesh
  {
    public static Mesh CreatePyramid(
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      float height,
      Color color)
    {
      baseWidth = Mathf.Max(Mathf.Abs(baseWidth), 0.0001f);
      baseDepth = Mathf.Max(Mathf.Abs(baseDepth), 0.0001f);
      height = Mathf.Max(Mathf.Abs(height), 0.0001f);
      float num1 = baseWidth * 0.5f;
      float num2 = baseDepth * 0.5f;
      Vector3 vector3 = baseCenter + Vector3.up * height;
      Vector3[] vector3Array1 = new Vector3[16]
      {
        vector3,
        baseCenter + Vector3.right * num1 - Vector3.forward * num2,
        baseCenter - Vector3.right * num1 - Vector3.forward * num2,
        vector3,
        baseCenter + Vector3.right * num1 + Vector3.forward * num2,
        baseCenter + Vector3.right * num1 - Vector3.forward * num2,
        vector3,
        baseCenter - Vector3.right * num1 + Vector3.forward * num2,
        baseCenter + Vector3.right * num1 + Vector3.forward * num2,
        vector3,
        baseCenter - Vector3.right * num1 - Vector3.forward * num2,
        baseCenter - Vector3.right * num1 + Vector3.forward * num2,
        baseCenter - Vector3.right * num1 - Vector3.forward * num2,
        baseCenter + Vector3.right * num1 - Vector3.forward * num2,
        baseCenter + Vector3.right * num1 + Vector3.forward * num2,
        baseCenter - Vector3.right * num1 + Vector3.forward * num2
      };
      int[] indices = new int[18]
      {
        0,
        1,
        2,
        3,
        4,
        5,
        6,
        7,
        8,
        9,
        10,
        11,
        12,
        13,
        14,
        12,
        14,
        15
      };
      Vector3[] vector3Array2 = new Vector3[vector3Array1.Length];
      for (int index1 = 0; index1 < 4; ++index1)
      {
        int index2 = indices[index1 * 3];
        int index3 = indices[index1 * 3 + 1];
        int index4 = indices[index1 * 3 + 2];
        Vector3 normalized = Vector3.Cross(vector3Array1[index3] - vector3Array1[index2], vector3Array1[index4] - vector3Array1[index2]).normalized;
        vector3Array2[index2] = normalized;
        vector3Array2[index3] = normalized;
        vector3Array2[index4] = normalized;
      }
      vector3Array2[vector3Array2.Length - 4] = -Vector3.up;
      vector3Array2[vector3Array2.Length - 3] = -Vector3.up;
      vector3Array2[vector3Array2.Length - 2] = -Vector3.up;
      vector3Array2[vector3Array2.Length - 1] = -Vector3.up;
      Mesh pyramid = new Mesh();
      pyramid.vertices = vector3Array1;
      pyramid.colors = ColorEx.GetFilledColorArray(vector3Array1.Length, color);
      pyramid.normals = vector3Array2;
      pyramid.SetIndices(indices, MeshTopology.Triangles, 0);
      pyramid.UploadMeshData(false);
      return pyramid;
    }

    public static Mesh CreateWirePyramid(
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      float height,
      Color color)
    {
      baseWidth = Mathf.Max(Mathf.Abs(baseWidth), 0.0001f);
      baseDepth = Mathf.Max(Mathf.Abs(baseDepth), 0.0001f);
      height = Mathf.Max(Mathf.Abs(height), 0.0001f);
      float num1 = baseWidth * 0.5f;
      float num2 = baseDepth * 0.5f;
      Vector3[] vector3Array = new Vector3[5]
      {
        baseCenter - Vector3.right * num1 - Vector3.forward * num2,
        baseCenter + Vector3.right * num1 - Vector3.forward * num2,
        baseCenter + Vector3.right * num1 + Vector3.forward * num2,
        baseCenter - Vector3.right * num1 + Vector3.forward * num2,
        baseCenter + Vector3.up * height
      };
      int[] indices = new int[16]
      {
        0,
        1,
        1,
        2,
        2,
        3,
        3,
        0,
        0,
        4,
        1,
        4,
        2,
        4,
        3,
        4
      };
      Mesh wirePyramid = new Mesh();
      wirePyramid.vertices = vector3Array;
      wirePyramid.colors = ColorEx.GetFilledColorArray(vector3Array.Length, color);
      wirePyramid.SetIndices(indices, MeshTopology.Lines, 0);
      wirePyramid.UploadMeshData(false);
      return wirePyramid;
    }
  }
}
