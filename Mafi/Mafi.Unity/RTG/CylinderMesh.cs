// Decompiled with JetBrains decompiler
// Type: RTG.CylinderMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace RTG
{
  public static class CylinderMesh
  {
    public static Mesh CreateCylinder(
      float bottomRadius,
      float topRadius,
      float height,
      int numSlices,
      int numStacks,
      int numBottomCapRings,
      int numTopCapRings,
      Color color)
    {
      if ((double) bottomRadius < 9.9999997473787516E-05)
        bottomRadius = 0.0001f;
      if ((double) topRadius < 9.9999997473787516E-05)
        topRadius = 0.0001f;
      if ((double) height < 9.9999997473787516E-05)
        height = 0.0001f;
      if (numSlices < 3)
        numSlices = 3;
      if (numStacks < 1)
        numStacks = 1;
      int num1 = 1;
      bool flag1 = numBottomCapRings >= num1;
      bool flag2 = numTopCapRings >= num1;
      int num2 = numStacks + 1;
      int num3 = numSlices + 1;
      int capacity = num2 * num3;
      int arrayLength = capacity;
      Vector3[] collection1 = new Vector3[capacity];
      Vector3[] collection2 = new Vector3[capacity];
      List<Vector3> vector3List1 = new List<Vector3>(capacity);
      List<Vector3> vector3List2 = new List<Vector3>(capacity);
      int index1 = 0;
      Vector3 zero = Vector3.zero;
      Vector3 vector3 = Vector3.up * height;
      Vector3 up = Vector3.up;
      float num4 = height / (float) numStacks;
      float num5 = 360f / (float) numSlices;
      for (int index2 = 0; index2 < num2; ++index2)
      {
        float num6 = zero.y + (float) index2 * num4;
        float num7 = Mathf.Lerp(bottomRadius, topRadius, num6 / vector3.y);
        for (int index3 = 0; index3 < num3; ++index3)
        {
          float num8 = (float) index3 * num5;
          Vector3 normalized = new Vector3(Mathf.Cos(num8 * ((float) Math.PI / 180f)), 0.0f, Mathf.Sin(num8 * ((float) Math.PI / 180f))).normalized;
          collection2[index1] = normalized;
          collection1[index1] = zero + num6 * up + normalized * num7;
          ++index1;
        }
      }
      vector3List1.AddRange((IEnumerable<Vector3>) collection1);
      vector3List2.AddRange((IEnumerable<Vector3>) collection2);
      int num9 = 0;
      List<int> intList = new List<int>(100);
      int[] collection3 = new int[numSlices * numStacks * 6];
      for (int index4 = 0; index4 < num2 - 1; ++index4)
      {
        for (int index5 = 0; index5 < num3 - 1; ++index5)
        {
          int num10 = index4 * num3 + index5;
          int[] numArray1 = collection3;
          int index6 = num9;
          int num11 = index6 + 1;
          int num12 = num10;
          numArray1[index6] = num12;
          int[] numArray2 = collection3;
          int index7 = num11;
          int num13 = index7 + 1;
          int num14 = num10 + num3;
          numArray2[index7] = num14;
          int[] numArray3 = collection3;
          int index8 = num13;
          int num15 = index8 + 1;
          int num16 = num10 + 1;
          numArray3[index8] = num16;
          int[] numArray4 = collection3;
          int index9 = num15;
          int num17 = index9 + 1;
          int num18 = num10 + num3;
          numArray4[index9] = num18;
          int[] numArray5 = collection3;
          int index10 = num17;
          int num19 = index10 + 1;
          int num20 = num10 + num3 + 1;
          numArray5[index10] = num20;
          int[] numArray6 = collection3;
          int index11 = num19;
          num9 = index11 + 1;
          int num21 = num10 + 1;
          numArray6[index11] = num21;
        }
      }
      intList.AddRange((IEnumerable<int>) collection3);
      if (flag1)
      {
        int num22 = numBottomCapRings + 1;
        int num23 = numSlices + 1;
        int length = num22 * num23;
        arrayLength += length;
        int index12 = 0;
        Vector3[] collection4 = new Vector3[length];
        Vector3[] collection5 = new Vector3[length];
        for (int index13 = 0; index13 < num22; ++index13)
        {
          float num24 = Mathf.Lerp(bottomRadius, 0.0f, (float) index13 / (float) (num22 - 1));
          for (int index14 = 0; index14 < num23; ++index14)
          {
            float num25 = (float) index14 * num5;
            Vector3 normalized = new Vector3(Mathf.Cos(num25 * ((float) Math.PI / 180f)), 0.0f, Mathf.Sin(num25 * ((float) Math.PI / 180f))).normalized;
            collection4[index12] = zero + normalized * num24;
            collection5[index12] = -up;
            ++index12;
          }
        }
        int count = vector3List1.Count;
        vector3List1.AddRange((IEnumerable<Vector3>) collection4);
        vector3List2.AddRange((IEnumerable<Vector3>) collection5);
        int num26 = 0;
        int[] collection6 = new int[numSlices * numBottomCapRings * 6];
        for (int index15 = 0; index15 < num22 - 1; ++index15)
        {
          for (int index16 = 0; index16 < num23 - 1; ++index16)
          {
            int num27 = count + index15 * num23 + index16;
            int[] numArray7 = collection6;
            int index17 = num26;
            int num28 = index17 + 1;
            int num29 = num27;
            numArray7[index17] = num29;
            int[] numArray8 = collection6;
            int index18 = num28;
            int num30 = index18 + 1;
            int num31 = num27 + 1;
            numArray8[index18] = num31;
            int[] numArray9 = collection6;
            int index19 = num30;
            int num32 = index19 + 1;
            int num33 = num27 + num23;
            numArray9[index19] = num33;
            int[] numArray10 = collection6;
            int index20 = num32;
            int num34 = index20 + 1;
            int num35 = num27 + num23;
            numArray10[index20] = num35;
            int[] numArray11 = collection6;
            int index21 = num34;
            int num36 = index21 + 1;
            int num37 = num27 + 1;
            numArray11[index21] = num37;
            int[] numArray12 = collection6;
            int index22 = num36;
            num26 = index22 + 1;
            int num38 = num27 + num23 + 1;
            numArray12[index22] = num38;
          }
        }
        intList.AddRange((IEnumerable<int>) collection6);
      }
      if (flag2)
      {
        int num39 = numTopCapRings + 1;
        int num40 = numSlices + 1;
        int length = num39 * num40;
        arrayLength += length;
        int index23 = 0;
        Vector3[] collection7 = new Vector3[length];
        Vector3[] collection8 = new Vector3[length];
        for (int index24 = 0; index24 < num39; ++index24)
        {
          float num41 = Mathf.Lerp(topRadius, 0.0f, (float) index24 / (float) (num39 - 1));
          for (int index25 = 0; index25 < num40; ++index25)
          {
            float num42 = (float) index25 * num5;
            Vector3 normalized = new Vector3(Mathf.Cos(num42 * ((float) Math.PI / 180f)), 0.0f, Mathf.Sin(num42 * ((float) Math.PI / 180f))).normalized;
            collection7[index23] = vector3 + normalized * num41;
            collection8[index23] = up;
            ++index23;
          }
        }
        int count = vector3List1.Count;
        vector3List1.AddRange((IEnumerable<Vector3>) collection7);
        vector3List2.AddRange((IEnumerable<Vector3>) collection8);
        int num43 = 0;
        int[] collection9 = new int[numSlices * numTopCapRings * 6];
        for (int index26 = 0; index26 < num39 - 1; ++index26)
        {
          for (int index27 = 0; index27 < num40 - 1; ++index27)
          {
            int num44 = count + index26 * num40 + index27;
            int[] numArray13 = collection9;
            int index28 = num43;
            int num45 = index28 + 1;
            int num46 = num44;
            numArray13[index28] = num46;
            int[] numArray14 = collection9;
            int index29 = num45;
            int num47 = index29 + 1;
            int num48 = num44 + num40;
            numArray14[index29] = num48;
            int[] numArray15 = collection9;
            int index30 = num47;
            int num49 = index30 + 1;
            int num50 = num44 + 1;
            numArray15[index30] = num50;
            int[] numArray16 = collection9;
            int index31 = num49;
            int num51 = index31 + 1;
            int num52 = num44 + num40;
            numArray16[index31] = num52;
            int[] numArray17 = collection9;
            int index32 = num51;
            int num53 = index32 + 1;
            int num54 = num44 + num40 + 1;
            numArray17[index32] = num54;
            int[] numArray18 = collection9;
            int index33 = num53;
            num43 = index33 + 1;
            int num55 = num44 + 1;
            numArray18[index33] = num55;
          }
        }
        intList.AddRange((IEnumerable<int>) collection9);
      }
      Mesh cylinder = new Mesh();
      cylinder.vertices = vector3List1.ToArray();
      cylinder.normals = vector3List2.ToArray();
      cylinder.colors = ColorEx.GetFilledColorArray(arrayLength, color);
      cylinder.SetIndices(intList.ToArray(), MeshTopology.Triangles, 0);
      cylinder.UploadMeshData(false);
      return cylinder;
    }
  }
}
