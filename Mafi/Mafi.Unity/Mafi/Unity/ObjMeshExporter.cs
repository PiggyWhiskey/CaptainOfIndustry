﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.ObjMeshExporter
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  public class ObjMeshExporter
  {
    /// <summary>
    /// Exports OBJ mesh to given directory and clears given builder.
    /// </summary>
    public static void ExportObjAndClear(string path, string nameWithoutExt, MeshBuilder builder)
    {
      ObjMeshExporter.ExportObj(path, nameWithoutExt, builder.Vertices, builder.Indices, builder.Normals, builder.Colors, builder.TexCoords);
      builder.Clear();
    }

    public static void ExportObj(string path, string nameWithoutExt, MeshBuilder builder)
    {
      ObjMeshExporter.ExportObj(path, nameWithoutExt, builder.Vertices, builder.Indices, builder.Normals, builder.Colors, builder.TexCoords);
    }

    public static void ExportObj(
      string path,
      string nameWithoutExt,
      Mesh mesh,
      Vector3 globalVertexOffset = default (Vector3))
    {
      ObjMeshExporter.ExportObj(path, nameWithoutExt, mesh.vertices.AsIndexable<Vector3>(), mesh.GetIndices(0).AsIndexable<int>(), mesh.normals.AsIndexable<Vector3>(), globalVertexOffset: globalVertexOffset);
    }

    public static void ExportObj(
      string path,
      string nameWithoutExt,
      IIndexable<Vector3> vertices,
      IIndexable<int> indices,
      IIndexable<Vector3> normals = null,
      IIndexable<Color32> colors = null,
      IIndexable<Vector2> texCoords = null,
      Vector3 globalVertexOffset = default (Vector3))
    {
      Assert.That<IIndexable<Vector3>>(normals).IsTrue<IIndexable<Vector3>>((Predicate<IIndexable<Vector3>>) (x => x == null || x.Count == vertices.Count));
      Assert.That<IIndexable<Color32>>(colors).IsTrue<IIndexable<Color32>>((Predicate<IIndexable<Color32>>) (x => x == null || x.Count == vertices.Count));
      Assert.That<IIndexable<Vector2>>(texCoords).IsTrue<IIndexable<Vector2>>((Predicate<IIndexable<Vector2>>) (x => x == null || x.Count == vertices.Count));
      Assert.That<int>(indices.Count % 3).IsZero();
      Dict<Color32, string> dict = (Dict<Color32, string>) null;
      string path2_1 = nameWithoutExt + ".obj";
      using (StreamWriter streamWriter1 = new StreamWriter(Path.Combine(path, path2_1)))
      {
        streamWriter1.WriteLine("# Generated by DeepMine game studio");
        streamWriter1.WriteLine("# Materials");
        NumberFormatInfo numberFormat = CultureInfo.InvariantCulture.NumberFormat;
        string str1 = (string) null;
        StreamWriter streamWriter2 = (StreamWriter) null;
        try
        {
          if (colors != null)
          {
            dict = new Dict<Color32, string>();
            string path2_2 = nameWithoutExt + ".mtl";
            streamWriter2 = new StreamWriter(Path.Combine(path, path2_2));
            streamWriter1.WriteLine("mtllib " + path2_2);
            streamWriter2.WriteLine("# Generated by DeepMine game studio");
            streamWriter2.WriteLine("# Materials for " + path2_1);
            streamWriter2.WriteLine();
          }
          streamWriter1.WriteLine();
          streamWriter1.WriteLine(string.Format("# {0} vertices", (object) vertices.Count));
          for (int index = 0; index < vertices.Count; ++index)
          {
            Vector3 vector3 = vertices[index] * 1f + globalVertexOffset;
            vector3.x = -vector3.x;
            if (colors != null)
            {
              Color32 color = colors[index];
              string str2;
              if (!dict.TryGetValue(color, out str2))
              {
                str2 = string.Format("color{0}x{1}x{2}", (object) color.r, (object) color.g, (object) color.b);
                float num1 = (float) color.r / (float) byte.MaxValue;
                float num2 = (float) color.g / (float) byte.MaxValue;
                float num3 = (float) color.b / (float) byte.MaxValue;
                streamWriter2.WriteLine("newmtl " + str2);
                streamWriter2.WriteLine(string.Format("Ka {0} {1} {2}", (object) (float) ((double) num1 / 5.0), (object) (float) ((double) num2 / 5.0), (object) (float) ((double) num3 / 5.0)));
                streamWriter2.WriteLine(string.Format("Kd {0} {1} {2}", (object) (float) (4.0 * (double) num1 / 5.0), (object) (float) (4.0 * (double) num2 / 5.0), (object) (float) (4.0 * (double) num3 / 5.0)));
                streamWriter2.WriteLine("Ks 0.0 0.0 0.0");
                streamWriter2.WriteLine("Ns 10.0");
                streamWriter2.WriteLine();
                dict.Add(color, str2);
              }
              if (str1 != str2)
              {
                str1 = str2;
                streamWriter1.WriteLine("usemtl " + str2);
              }
            }
            streamWriter1.WriteLine("v " + vector3.x.ToString("0.#####", (IFormatProvider) numberFormat) + " " + vector3.y.ToString("0.#####", (IFormatProvider) numberFormat) + " " + vector3.z.ToString("0.#####", (IFormatProvider) numberFormat));
          }
        }
        finally
        {
          streamWriter2?.Dispose();
        }
        if (normals != null)
        {
          streamWriter1.WriteLine();
          streamWriter1.WriteLine(string.Format("# {0} normals", (object) normals.Count));
          foreach (Vector3 normal in normals)
          {
            float num = -normal.x;
            streamWriter1.WriteLine("vn " + num.ToString("0.#####", (IFormatProvider) numberFormat) + " " + normal.y.ToString("0.#####", (IFormatProvider) numberFormat) + " " + normal.z.ToString("0.#####", (IFormatProvider) numberFormat));
          }
        }
        if (texCoords != null)
        {
          streamWriter1.WriteLine();
          streamWriter1.WriteLine(string.Format("# {0} texture coordinates", (object) texCoords.Count));
          foreach (Vector2 texCoord in texCoords)
            streamWriter1.WriteLine("vt " + texCoord.x.ToString("0.#####", (IFormatProvider) numberFormat) + " " + texCoord.y.ToString("0.#####", (IFormatProvider) numberFormat));
        }
        streamWriter1.WriteLine();
        streamWriter1.WriteLine(string.Format("# {0} triangular faces", (object) (indices.Count / 3)));
        string format = "f {0}/" + (texCoords != null ? "{0}" : "") + "/" + (normals != null ? "{0}" : "") + " {2}/" + (texCoords != null ? "{2}" : "") + "/" + (normals != null ? "{2}" : "") + " {1}/" + (texCoords != null ? "{1}" : "") + "/" + (normals != null ? "{1}" : "");
        string str3 = (string) null;
        int maxInclusive = vertices.Count - 1;
        for (int index = 0; index < indices.Count; index += 3)
        {
          if (colors != null)
          {
            string str4 = dict[colors[indices[index]]];
            if (str3 != str4)
            {
              str3 = str4;
              streamWriter1.WriteLine("usemtl " + str4);
            }
          }
          Assert.That<int>(indices[index]).IsWithinIncl(0, maxInclusive);
          Assert.That<int>(indices[index + 1]).IsWithinIncl(0, maxInclusive);
          Assert.That<int>(indices[index + 2]).IsWithinIncl(0, maxInclusive);
          streamWriter1.WriteLine(format, (object) (indices[index] + 1), (object) (indices[index + 1] + 1), (object) (indices[index + 2] + 1));
        }
      }
    }

    public ObjMeshExporter()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
