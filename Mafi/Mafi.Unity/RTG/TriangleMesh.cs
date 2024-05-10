// Decompiled with JetBrains decompiler
// Type: RTG.TriangleMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class TriangleMesh
  {
    public static Mesh CreateEqXY(Vector3 centroid, float sideLength, Color color)
    {
      Vector3[] array = TriangleMath.CalcEqTriangle3DPoints(centroid, sideLength, Quaternion.identity).ToArray();
      Vector3[] vector3Array = new Vector3[3]
      {
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward
      };
      Mesh eqXy = new Mesh();
      eqXy.vertices = array;
      eqXy.colors = new Color[3]{ color, color, color };
      eqXy.normals = vector3Array;
      eqXy.SetIndices(new int[3]{ 0, 1, 2 }, MeshTopology.Triangles, 0);
      eqXy.UploadMeshData(false);
      return eqXy;
    }

    public static Mesh CreateWireEqXY(Vector3 centroid, float sideLength, Color color)
    {
      Vector3[] array = TriangleMath.CalcEqTriangle3DPoints(centroid, sideLength, Quaternion.identity).ToArray();
      Mesh wireEqXy = new Mesh();
      wireEqXy.vertices = array;
      wireEqXy.colors = new Color[3]{ color, color, color };
      wireEqXy.SetIndices(new int[4]{ 0, 1, 2, 0 }, MeshTopology.LineStrip, 0);
      wireEqXy.UploadMeshData(false);
      return wireEqXy;
    }

    public static Mesh CreateRightAngledTriangleXY(
      Vector3 cornerPosition,
      float xLength,
      float yLength,
      Color color)
    {
      if ((double) xLength < 9.9999997473787516E-05 || (double) yLength < 9.9999997473787516E-05)
        return (Mesh) null;
      Vector3[] vector3Array = new Vector3[3]
      {
        cornerPosition,
        cornerPosition + Vector3.up * xLength,
        cornerPosition + Vector3.right * yLength
      };
      Mesh angledTriangleXy = new Mesh();
      angledTriangleXy.vertices = vector3Array;
      angledTriangleXy.colors = new Color[3]
      {
        color,
        color,
        color
      };
      angledTriangleXy.SetIndices(new int[3]{ 0, 1, 2 }, MeshTopology.Triangles, 0);
      angledTriangleXy.UploadMeshData(false);
      return angledTriangleXy;
    }

    public static Mesh CreateWireRightAngledTriangleXY(
      Vector3 cornerPosition,
      float xLength,
      float yLength,
      Color color)
    {
      if ((double) xLength < 9.9999997473787516E-05 || (double) yLength < 9.9999997473787516E-05)
        return (Mesh) null;
      Vector3[] vector3Array = new Vector3[3]
      {
        cornerPosition,
        cornerPosition + Vector3.up * xLength,
        cornerPosition + Vector3.right * yLength
      };
      Mesh angledTriangleXy = new Mesh();
      angledTriangleXy.vertices = vector3Array;
      angledTriangleXy.colors = new Color[3]
      {
        color,
        color,
        color
      };
      angledTriangleXy.SetIndices(new int[4]{ 0, 1, 2, 0 }, MeshTopology.LineStrip, 0);
      angledTriangleXy.UploadMeshData(false);
      return angledTriangleXy;
    }
  }
}
