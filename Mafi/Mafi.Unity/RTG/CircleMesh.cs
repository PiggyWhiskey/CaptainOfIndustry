// Decompiled with JetBrains decompiler
// Type: RTG.CircleMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class CircleMesh
  {
    public static Mesh CreateCircleXY(float circleRadius, int numBorderPoints, Color color)
    {
      if ((double) circleRadius < 9.9999997473787516E-05 || numBorderPoints < 4)
        return (Mesh) null;
      int num1 = numBorderPoints + 1;
      int num2 = numBorderPoints - 1;
      Vector3[] vector3Array1 = new Vector3[numBorderPoints + 1];
      Vector3[] vector3Array2 = new Vector3[vector3Array1.Length];
      int[] indices = new int[num2 * 3];
      int num3 = 0;
      vector3Array1[0] = Vector3.zero;
      float num4 = 360f / (float) (numBorderPoints - 1);
      for (int index1 = 0; index1 < numBorderPoints; ++index1)
      {
        float f = (float) ((double) num4 * (double) index1 * (Math.PI / 180.0));
        int index2 = index1 + 1;
        vector3Array1[index2] = new Vector3(Mathf.Sin(f) * circleRadius, Mathf.Cos(f) * circleRadius, 0.0f);
        vector3Array2[index2] = Vector3.forward;
      }
      for (int index3 = 1; index3 < num1 - 1; ++index3)
      {
        int[] numArray1 = indices;
        int index4 = num3;
        int num5 = index4 + 1;
        numArray1[index4] = 0;
        int[] numArray2 = indices;
        int index5 = num5;
        int num6 = index5 + 1;
        int num7 = index3;
        numArray2[index5] = num7;
        int[] numArray3 = indices;
        int index6 = num6;
        num3 = index6 + 1;
        int num8 = index3 + 1;
        numArray3[index6] = num8;
      }
      Mesh circleXy = new Mesh();
      circleXy.vertices = vector3Array1;
      circleXy.colors = ColorEx.GetFilledColorArray(vector3Array1.Length, color);
      circleXy.normals = vector3Array2;
      circleXy.SetIndices(indices, MeshTopology.Triangles, 0);
      circleXy.UploadMeshData(false);
      return circleXy;
    }

    public static Mesh CreateWireCircleXY(float circleRadius, int numBorderPoints, Color color)
    {
      if ((double) circleRadius < 9.9999997473787516E-05 || numBorderPoints < 4)
        return (Mesh) null;
      Vector3[] vector3Array = new Vector3[numBorderPoints];
      int[] indices = new int[numBorderPoints];
      float num = 360f / (float) (numBorderPoints - 1);
      for (int index = 0; index < numBorderPoints; ++index)
      {
        float f = (float) ((double) num * (double) index * (Math.PI / 180.0));
        vector3Array[index] = new Vector3(Mathf.Sin(f) * circleRadius, Mathf.Cos(f) * circleRadius, 0.0f);
        indices[index] = index;
      }
      Mesh wireCircleXy = new Mesh();
      wireCircleXy.vertices = vector3Array;
      wireCircleXy.colors = ColorEx.GetFilledColorArray(numBorderPoints, color);
      wireCircleXy.SetIndices(indices, MeshTopology.LineStrip, 0);
      wireCircleXy.UploadMeshData(false);
      return wireCircleXy;
    }
  }
}
