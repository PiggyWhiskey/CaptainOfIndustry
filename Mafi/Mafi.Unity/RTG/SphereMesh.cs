// Decompiled with JetBrains decompiler
// Type: RTG.SphereMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class SphereMesh
  {
    public static Mesh CreateSphere(float radius, int numSlices, int numStacks, Color color)
    {
      if ((double) radius < 9.9999997473787516E-05 || numSlices < 3 || numStacks < 2)
        return (Mesh) null;
      int num1 = numStacks + 1;
      int num2 = numSlices + 1;
      int arrayLength = num1 * num2;
      Vector3[] vector3Array1 = new Vector3[arrayLength];
      Vector3[] vector3Array2 = new Vector3[arrayLength];
      int index1 = 0;
      float num3 = 360f / (float) (num2 - 1);
      for (int index2 = 0; index2 < num1; ++index2)
      {
        float f1 = 3.14159274f * (float) index2 / (float) (num1 - 1);
        float num4 = Mathf.Cos(f1);
        float num5 = Mathf.Sin(f1);
        for (int index3 = 0; index3 < num2; ++index3)
        {
          float f2 = (float) ((double) num3 * (double) index3 * (Math.PI / 180.0));
          Vector3 vector3 = Vector3.right * Mathf.Sin(f2) + Vector3.forward * Mathf.Cos(f2);
          vector3Array1[index1] = vector3 * num5 * radius + Vector3.up * num4 * radius;
          vector3Array2[index1] = Vector3.Normalize(vector3Array1[index1]);
          ++index1;
        }
      }
      int num6 = 0;
      int[] indices = new int[numSlices * numStacks * 6];
      for (int index4 = 0; index4 < num1 - 1; ++index4)
      {
        for (int index5 = 0; index5 < num2 - 1; ++index5)
        {
          int num7 = index4 * num2 + index5;
          int[] numArray1 = indices;
          int index6 = num6;
          int num8 = index6 + 1;
          int num9 = num7;
          numArray1[index6] = num9;
          int[] numArray2 = indices;
          int index7 = num8;
          int num10 = index7 + 1;
          int num11 = num7 + num2;
          numArray2[index7] = num11;
          int[] numArray3 = indices;
          int index8 = num10;
          int num12 = index8 + 1;
          int num13 = num7 + num2 + 1;
          numArray3[index8] = num13;
          int[] numArray4 = indices;
          int index9 = num12;
          int num14 = index9 + 1;
          int num15 = num7 + num2 + 1;
          numArray4[index9] = num15;
          int[] numArray5 = indices;
          int index10 = num14;
          int num16 = index10 + 1;
          int num17 = num7 + 1;
          numArray5[index10] = num17;
          int[] numArray6 = indices;
          int index11 = num16;
          num6 = index11 + 1;
          int num18 = num7;
          numArray6[index11] = num18;
        }
      }
      Mesh sphere = new Mesh();
      sphere.vertices = vector3Array1;
      sphere.normals = vector3Array2;
      sphere.colors = ColorEx.GetFilledColorArray(arrayLength, color);
      sphere.SetIndices(indices, MeshTopology.Triangles, 0);
      sphere.UploadMeshData(false);
      return sphere;
    }
  }
}
