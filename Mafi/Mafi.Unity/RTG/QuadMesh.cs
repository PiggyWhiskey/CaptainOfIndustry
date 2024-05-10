// Decompiled with JetBrains decompiler
// Type: RTG.QuadMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class QuadMesh
  {
    public static Mesh CreateQuadXZ(float width, float depth, Color color)
    {
      if ((double) width < 9.9999997473787516E-05)
        return (Mesh) null;
      if ((double) depth < 9.9999997473787516E-05)
        return (Mesh) null;
      float num1 = width * 0.5f;
      float num2 = depth * 0.5f;
      Vector3[] vector3Array1 = new Vector3[4]
      {
        -Vector3.right * num1 - Vector3.forward * num2,
        -Vector3.right * num1 + Vector3.forward * num2,
        Vector3.right * num1 + Vector3.forward * num2,
        Vector3.right * num1 - Vector3.forward * num2
      };
      Vector3[] vector3Array2 = new Vector3[4]
      {
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up
      };
      Vector2[] vector2Array = new Vector2[4]
      {
        Vector2.zero,
        new Vector2(0.0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0.0f)
      };
      Mesh quadXz = new Mesh();
      quadXz.vertices = vector3Array1;
      quadXz.normals = vector3Array2;
      quadXz.uv = vector2Array;
      quadXz.colors = ColorEx.GetFilledColorArray(4, color);
      quadXz.SetIndices(new int[6]{ 0, 1, 2, 2, 3, 0 }, MeshTopology.Triangles, 0);
      quadXz.UploadMeshData(false);
      return quadXz;
    }

    public static Mesh CreateQuadXY(float width, float height, Color color)
    {
      if ((double) width < 9.9999997473787516E-05)
        return (Mesh) null;
      if ((double) height < 9.9999997473787516E-05)
        return (Mesh) null;
      float num1 = width * 0.5f;
      float num2 = height * 0.5f;
      Vector3[] vector3Array1 = new Vector3[4]
      {
        -Vector3.right * num1 - Vector3.up * num2,
        -Vector3.right * num1 + Vector3.up * num2,
        Vector3.right * num1 + Vector3.up * num2,
        Vector3.right * num1 - Vector3.up * num2
      };
      Vector3[] vector3Array2 = new Vector3[4]
      {
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward
      };
      Vector2[] vector2Array = new Vector2[4]
      {
        Vector2.zero,
        new Vector2(0.0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0.0f)
      };
      Mesh quadXy = new Mesh();
      quadXy.vertices = vector3Array1;
      quadXy.normals = vector3Array2;
      quadXy.uv = vector2Array;
      quadXy.colors = ColorEx.GetFilledColorArray(4, color);
      quadXy.SetIndices(new int[6]{ 0, 1, 2, 2, 3, 0 }, MeshTopology.Triangles, 0);
      quadXy.UploadMeshData(false);
      return quadXy;
    }

    public static Mesh CreateWireQuadXY(Vector3 center, Vector2 size, Color color)
    {
      Vector2 vector2 = size * 0.5f;
      Vector3[] vector3Array = new Vector3[4]
      {
        center - Vector3.right * vector2.x - Vector3.up * vector2.y,
        center - Vector3.right * vector2.x + Vector3.up * vector2.y,
        center + Vector3.right * vector2.x + Vector3.up * vector2.y,
        center + Vector3.right * vector2.x - Vector3.up * vector2.y
      };
      int[] indices = new int[8]{ 0, 1, 1, 2, 2, 3, 3, 0 };
      Mesh wireQuadXy = new Mesh();
      wireQuadXy.vertices = vector3Array;
      wireQuadXy.colors = ColorEx.GetFilledColorArray(4, color);
      wireQuadXy.SetIndices(indices, MeshTopology.Lines, 0);
      wireQuadXy.UploadMeshData(false);
      return wireQuadXy;
    }
  }
}
