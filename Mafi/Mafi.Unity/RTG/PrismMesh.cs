// Decompiled with JetBrains decompiler
// Type: RTG.PrismMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class PrismMesh
  {
    public static Mesh CreateTriangularPrism(
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      float topWidth,
      float topDepth,
      float height,
      Color color)
    {
      baseWidth = Mathf.Max(Mathf.Abs(baseWidth), 0.0001f);
      baseDepth = Mathf.Max(Mathf.Abs(baseDepth), 0.0001f);
      topWidth = Mathf.Max(Mathf.Abs(topWidth), 0.0001f);
      topDepth = Mathf.Max(Mathf.Abs(topDepth), 0.0001f);
      height = Mathf.Max(Mathf.Abs(height), 0.0001f);
      List<Vector3> vector3List = PrismMath.CalcTriangPrismCornerPoints(baseCenter, baseWidth, baseDepth, topWidth, topDepth, height, Quaternion.identity);
      Vector3 vector3_1 = vector3List[0];
      Vector3 vector3_2 = vector3List[1];
      Vector3 vector3_3 = vector3List[2];
      Vector3 vector3_4 = vector3List[3];
      Vector3 vector3_5 = vector3List[5];
      Vector3 vector3_6 = vector3List[4];
      Vector3[] vector3Array1 = new Vector3[18]
      {
        vector3_4,
        vector3_6,
        vector3_5,
        vector3_1,
        vector3_2,
        vector3_3,
        vector3_1,
        vector3_4,
        vector3_5,
        vector3_2,
        vector3_1,
        vector3_3,
        vector3_6,
        vector3_4,
        vector3_2,
        vector3_5,
        vector3_6,
        vector3_3
      };
      int[] indices = new int[24]
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
        8,
        9,
        6,
        10,
        11,
        12,
        12,
        13,
        10,
        14,
        15,
        16,
        16,
        17,
        14
      };
      Vector3 normalized1 = Vector3.Cross(vector3Array1[11] - vector3Array1[10], vector3Array1[13] - vector3Array1[10]).normalized;
      Vector3 normalized2 = Vector3.Cross(vector3Array1[15] - vector3Array1[14], vector3Array1[17] - vector3Array1[14]).normalized;
      Vector3[] vector3Array2 = new Vector3[18]
      {
        Vector3.up,
        Vector3.up,
        Vector3.up,
        -Vector3.up,
        -Vector3.up,
        -Vector3.up,
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward,
        normalized1,
        normalized1,
        normalized1,
        normalized1,
        normalized2,
        normalized2,
        normalized2,
        normalized2
      };
      Mesh triangularPrism = new Mesh();
      triangularPrism.vertices = vector3Array1;
      triangularPrism.colors = ColorEx.GetFilledColorArray(vector3Array1.Length, color);
      triangularPrism.normals = vector3Array2;
      triangularPrism.SetIndices(indices, MeshTopology.Triangles, 0);
      triangularPrism.UploadMeshData(false);
      return triangularPrism;
    }

    public static Mesh CreateWireTriangularPrism(
      Vector3 baseCenter,
      float baseWidth,
      float baseDepth,
      float topWidth,
      float topDepth,
      float height,
      Color color)
    {
      baseWidth = Mathf.Max(Mathf.Abs(baseWidth), 0.0001f);
      baseDepth = Mathf.Max(Mathf.Abs(baseDepth), 0.0001f);
      topWidth = Mathf.Max(Mathf.Abs(topWidth), 0.0001f);
      topDepth = Mathf.Max(Mathf.Abs(topDepth), 0.0001f);
      height = Mathf.Max(Mathf.Abs(height), 0.0001f);
      List<Vector3> vector3List = PrismMath.CalcTriangPrismCornerPoints(baseCenter, baseWidth, baseDepth, topWidth, topDepth, height, Quaternion.identity);
      Vector3 vector3_1 = vector3List[0];
      Vector3 vector3_2 = vector3List[1];
      Vector3 vector3_3 = vector3List[2];
      Vector3 vector3_4 = vector3List[3];
      Vector3 vector3_5 = vector3List[5];
      Vector3 vector3_6 = vector3List[4];
      Vector3[] vector3Array = new Vector3[6]
      {
        vector3_1,
        vector3_3,
        vector3_2,
        vector3_4,
        vector3_6,
        vector3_5
      };
      int[] indices = new int[18]
      {
        0,
        1,
        1,
        2,
        2,
        0,
        3,
        4,
        4,
        5,
        5,
        3,
        0,
        3,
        1,
        4,
        2,
        5
      };
      Mesh wireTriangularPrism = new Mesh();
      wireTriangularPrism.vertices = vector3Array;
      wireTriangularPrism.colors = ColorEx.GetFilledColorArray(vector3Array.Length, color);
      wireTriangularPrism.SetIndices(indices, MeshTopology.Lines, 0);
      wireTriangularPrism.UploadMeshData(false);
      return wireTriangularPrism;
    }
  }
}
