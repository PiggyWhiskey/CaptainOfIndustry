// Decompiled with JetBrains decompiler
// Type: RTG.LineMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class LineMesh
  {
    public static Mesh CreateCoordSystemAxesLines(float axisLength, Color color)
    {
      if ((double) axisLength < 9.9999997473787516E-05)
        return (Mesh) null;
      Vector3[] vector3Array = new Vector3[4]
      {
        Vector3.zero,
        Vector3.right * axisLength,
        Vector3.up * axisLength,
        Vector3.forward * axisLength
      };
      int[] indices = new int[6]{ 0, 1, 0, 2, 0, 3 };
      Mesh coordSystemAxesLines = new Mesh();
      coordSystemAxesLines.vertices = vector3Array;
      coordSystemAxesLines.colors = ColorEx.GetFilledColorArray(4, color);
      coordSystemAxesLines.SetIndices(indices, MeshTopology.Lines, 0);
      coordSystemAxesLines.UploadMeshData(false);
      return coordSystemAxesLines;
    }

    public static Mesh CreateLine(Vector3 startPoint, Vector3 endPoint, Color color)
    {
      Mesh line = new Mesh();
      line.vertices = new Vector3[2]{ startPoint, endPoint };
      line.colors = ColorEx.GetFilledColorArray(2, color);
      line.SetIndices(new int[2]{ 0, 1 }, MeshTopology.Lines, 0);
      line.UploadMeshData(false);
      return line;
    }
  }
}
