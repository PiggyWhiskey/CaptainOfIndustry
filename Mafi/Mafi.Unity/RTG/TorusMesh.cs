// Decompiled with JetBrains decompiler
// Type: RTG.TorusMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class TorusMesh
  {
    public static Mesh CreateCylindricalTorus(
      Vector3 center,
      float coreRadius,
      float tubeHrzRadius,
      float tubeVertRadius,
      int numTubeSlices,
      Color color)
    {
      if ((double) coreRadius < 9.9999997473787516E-05 || (double) tubeHrzRadius < 9.9999997473787516E-05 || numTubeSlices < 3)
        return (Mesh) null;
      int num1 = 8;
      int arrayLength = num1 * (numTubeSlices + 1);
      Vector3[] vector3Array1 = new Vector3[arrayLength];
      Vector3[] vector3Array2 = new Vector3[arrayLength];
      Vector2[] vector2Array = new Vector2[arrayLength];
      int index1 = 0;
      float num2 = 360f / (float) (numTubeSlices - 1);
      for (int index2 = 0; index2 <= numTubeSlices; ++index2)
      {
        float f = (float) ((double) num2 * (double) index2 * (Math.PI / 180.0));
        float z = Mathf.Cos(f);
        Vector3 normalized = new Vector3(Mathf.Sin(f), 0.0f, z).normalized;
        Vector3 vector3_1 = center + normalized * coreRadius;
        Vector2 vector2 = new Vector2(normalized.x, normalized.z);
        vector2Array[index1] = vector2;
        vector3Array1[index1] = vector3_1 + Vector3.up * tubeVertRadius - normalized * tubeHrzRadius;
        Vector3[] vector3Array3 = vector3Array2;
        int index3 = index1;
        int index4 = index3 + 1;
        Vector3 up1 = Vector3.up;
        vector3Array3[index3] = up1;
        vector2Array[index4] = vector2;
        vector3Array1[index4] = vector3_1 + Vector3.up * tubeVertRadius + normalized * tubeHrzRadius;
        Vector3[] vector3Array4 = vector3Array2;
        int index5 = index4;
        int index6 = index5 + 1;
        Vector3 up2 = Vector3.up;
        vector3Array4[index5] = up2;
        vector2Array[index6] = vector2;
        vector3Array1[index6] = vector3_1 + Vector3.up * tubeVertRadius + normalized * tubeHrzRadius;
        Vector3[] vector3Array5 = vector3Array2;
        int index7 = index6;
        int index8 = index7 + 1;
        Vector3 vector3_2 = normalized;
        vector3Array5[index7] = vector3_2;
        vector2Array[index8] = vector2;
        vector3Array1[index8] = vector3_1 - Vector3.up * tubeVertRadius + normalized * tubeHrzRadius;
        Vector3[] vector3Array6 = vector3Array2;
        int index9 = index8;
        int index10 = index9 + 1;
        Vector3 vector3_3 = normalized;
        vector3Array6[index9] = vector3_3;
        vector2Array[index10] = vector2;
        vector3Array1[index10] = vector3_1 - Vector3.up * tubeVertRadius + normalized * tubeHrzRadius;
        Vector3[] vector3Array7 = vector3Array2;
        int index11 = index10;
        int index12 = index11 + 1;
        Vector3 vector3_4 = -Vector3.up;
        vector3Array7[index11] = vector3_4;
        vector2Array[index12] = vector2;
        vector3Array1[index12] = vector3_1 - Vector3.up * tubeVertRadius - normalized * tubeHrzRadius;
        Vector3[] vector3Array8 = vector3Array2;
        int index13 = index12;
        int index14 = index13 + 1;
        Vector3 vector3_5 = -Vector3.up;
        vector3Array8[index13] = vector3_5;
        vector2Array[index14] = vector2;
        vector3Array1[index14] = vector3_1 - Vector3.up * tubeVertRadius - normalized * tubeHrzRadius;
        Vector3[] vector3Array9 = vector3Array2;
        int index15 = index14;
        int index16 = index15 + 1;
        Vector3 vector3_6 = -normalized;
        vector3Array9[index15] = vector3_6;
        vector2Array[index16] = vector2;
        vector3Array1[index16] = vector3_1 + Vector3.up * tubeVertRadius - normalized * tubeHrzRadius;
        Vector3[] vector3Array10 = vector3Array2;
        int index17 = index16;
        index1 = index17 + 1;
        Vector3 vector3_7 = -normalized;
        vector3Array10[index17] = vector3_7;
      }
      int num3 = 0;
      int[] indices = new int[numTubeSlices * 24];
      for (int index18 = 0; index18 < numTubeSlices - 1; ++index18)
      {
        int num4 = index18 * num1;
        int[] numArray1 = indices;
        int index19 = num3;
        int num5 = index19 + 1;
        int num6 = num4;
        numArray1[index19] = num6;
        int[] numArray2 = indices;
        int index20 = num5;
        int num7 = index20 + 1;
        int num8 = num4 + 1;
        numArray2[index20] = num8;
        int[] numArray3 = indices;
        int index21 = num7;
        int num9 = index21 + 1;
        int num10 = num4 + 1 + num1;
        numArray3[index21] = num10;
        int[] numArray4 = indices;
        int index22 = num9;
        int num11 = index22 + 1;
        int num12 = num4;
        numArray4[index22] = num12;
        int[] numArray5 = indices;
        int index23 = num11;
        int num13 = index23 + 1;
        int num14 = num4 + 1 + num1;
        numArray5[index23] = num14;
        int[] numArray6 = indices;
        int index24 = num13;
        int num15 = index24 + 1;
        int num16 = num4 + num1;
        numArray6[index24] = num16;
        int num17 = num4 + 2;
        int[] numArray7 = indices;
        int index25 = num15;
        int num18 = index25 + 1;
        int num19 = num17;
        numArray7[index25] = num19;
        int[] numArray8 = indices;
        int index26 = num18;
        int num20 = index26 + 1;
        int num21 = num17 + 1;
        numArray8[index26] = num21;
        int[] numArray9 = indices;
        int index27 = num20;
        int num22 = index27 + 1;
        int num23 = num17 + 1 + num1;
        numArray9[index27] = num23;
        int[] numArray10 = indices;
        int index28 = num22;
        int num24 = index28 + 1;
        int num25 = num17;
        numArray10[index28] = num25;
        int[] numArray11 = indices;
        int index29 = num24;
        int num26 = index29 + 1;
        int num27 = num17 + 1 + num1;
        numArray11[index29] = num27;
        int[] numArray12 = indices;
        int index30 = num26;
        int num28 = index30 + 1;
        int num29 = num17 + num1;
        numArray12[index30] = num29;
        int num30 = num17 + 2;
        int[] numArray13 = indices;
        int index31 = num28;
        int num31 = index31 + 1;
        int num32 = num30;
        numArray13[index31] = num32;
        int[] numArray14 = indices;
        int index32 = num31;
        int num33 = index32 + 1;
        int num34 = num30 + 1;
        numArray14[index32] = num34;
        int[] numArray15 = indices;
        int index33 = num33;
        int num35 = index33 + 1;
        int num36 = num30 + 1 + num1;
        numArray15[index33] = num36;
        int[] numArray16 = indices;
        int index34 = num35;
        int num37 = index34 + 1;
        int num38 = num30;
        numArray16[index34] = num38;
        int[] numArray17 = indices;
        int index35 = num37;
        int num39 = index35 + 1;
        int num40 = num30 + 1 + num1;
        numArray17[index35] = num40;
        int[] numArray18 = indices;
        int index36 = num39;
        int num41 = index36 + 1;
        int num42 = num30 + num1;
        numArray18[index36] = num42;
        int num43 = num30 + 2;
        int[] numArray19 = indices;
        int index37 = num41;
        int num44 = index37 + 1;
        int num45 = num43;
        numArray19[index37] = num45;
        int[] numArray20 = indices;
        int index38 = num44;
        int num46 = index38 + 1;
        int num47 = num43 + 1;
        numArray20[index38] = num47;
        int[] numArray21 = indices;
        int index39 = num46;
        int num48 = index39 + 1;
        int num49 = num43 + 1 + num1;
        numArray21[index39] = num49;
        int[] numArray22 = indices;
        int index40 = num48;
        int num50 = index40 + 1;
        int num51 = num43;
        numArray22[index40] = num51;
        int[] numArray23 = indices;
        int index41 = num50;
        int num52 = index41 + 1;
        int num53 = num43 + 1 + num1;
        numArray23[index41] = num53;
        int[] numArray24 = indices;
        int index42 = num52;
        num3 = index42 + 1;
        int num54 = num43 + num1;
        numArray24[index42] = num54;
      }
      Mesh cylindricalTorus = new Mesh();
      cylindricalTorus.vertices = vector3Array1;
      cylindricalTorus.normals = vector3Array2;
      cylindricalTorus.uv2 = vector2Array;
      cylindricalTorus.colors = ColorEx.GetFilledColorArray(arrayLength, color);
      cylindricalTorus.SetIndices(indices, MeshTopology.Triangles, 0);
      cylindricalTorus.UploadMeshData(false);
      return cylindricalTorus;
    }

    public static Mesh CreateTorus(
      Vector3 center,
      float coreRadius,
      float tubeRadius,
      int numTubeSlices,
      int numSlices,
      Color color)
    {
      if ((double) coreRadius < 9.9999997473787516E-05 || (double) tubeRadius < 9.9999997473787516E-05 || numTubeSlices < 3 || numSlices < 3)
        return (Mesh) null;
      int num1 = numSlices + 1;
      int arrayLength = num1 * (numTubeSlices + 1);
      Vector3[] vector3Array1 = new Vector3[arrayLength];
      Vector3[] vector3Array2 = new Vector3[arrayLength];
      int index1 = 0;
      float num2 = 360f / (float) (numSlices - 1);
      float num3 = 360f / (float) (numTubeSlices - 1);
      for (int index2 = 0; index2 <= numTubeSlices; ++index2)
      {
        float f1 = (float) ((double) num3 * (double) index2 * (Math.PI / 180.0));
        float num4 = Mathf.Cos(f1);
        float num5 = Mathf.Sin(f1);
        Vector3 vector3_1 = new Vector3(num5 * coreRadius, 0.0f, num4 * coreRadius);
        for (int index3 = 0; index3 <= numSlices; ++index3)
        {
          float f2 = (float) ((double) num2 * (double) index3 * (Math.PI / 180.0));
          float num6 = Mathf.Cos(f2);
          float num7 = Mathf.Sin(f2);
          Vector3 vector3_2 = vector3_1;
          vector3_2.x += num5 * num7 * tubeRadius;
          vector3_2.y += num6 * tubeRadius;
          vector3_2.z += num4 * num7 * tubeRadius;
          vector3Array2[index1] = (vector3_2 - vector3_1).normalized;
          vector3_2 += center;
          vector3Array1[index1] = vector3_2;
          ++index1;
        }
      }
      int num8 = 0;
      int[] indices = new int[numTubeSlices * numSlices * 6];
      for (int index4 = 0; index4 < numTubeSlices; ++index4)
      {
        for (int index5 = 0; index5 < numSlices; ++index5)
        {
          int num9 = index4 * num1 + index5;
          int[] numArray1 = indices;
          int index6 = num8;
          int num10 = index6 + 1;
          int num11 = num9;
          numArray1[index6] = num11;
          int[] numArray2 = indices;
          int index7 = num10;
          int num12 = index7 + 1;
          int num13 = num9 + 1;
          numArray2[index7] = num13;
          int[] numArray3 = indices;
          int index8 = num12;
          int num14 = index8 + 1;
          int num15 = num9 + num1;
          numArray3[index8] = num15;
          int[] numArray4 = indices;
          int index9 = num14;
          int num16 = index9 + 1;
          int num17 = num9 + 1;
          numArray4[index9] = num17;
          int[] numArray5 = indices;
          int index10 = num16;
          int num18 = index10 + 1;
          int num19 = num9 + 1 + num1;
          numArray5[index10] = num19;
          int[] numArray6 = indices;
          int index11 = num18;
          num8 = index11 + 1;
          int num20 = num9 + num1;
          numArray6[index11] = num20;
        }
      }
      Mesh torus = new Mesh();
      torus.vertices = vector3Array1;
      torus.normals = vector3Array2;
      torus.colors = ColorEx.GetFilledColorArray(arrayLength, color);
      torus.SetIndices(indices, MeshTopology.Triangles, 0);
      torus.UploadMeshData(false);
      return torus;
    }
  }
}
