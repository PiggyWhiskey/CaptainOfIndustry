// Decompiled with JetBrains decompiler
// Type: RTG.BoxMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public static class BoxMesh
  {
    public static Mesh CreateBox(float width, float height, float depth, Color color)
    {
      if ((double) width < 9.9999997473787516E-05)
        return (Mesh) null;
      if ((double) height < 9.9999997473787516E-05)
        return (Mesh) null;
      if ((double) depth < 9.9999997473787516E-05)
        return (Mesh) null;
      float x = width * 0.5f;
      float y = height * 0.5f;
      float z = depth * 0.5f;
      Vector3[] vector3Array1 = new Vector3[24]
      {
        new Vector3(-x, -y, -z),
        new Vector3(-x, y, -z),
        new Vector3(x, y, -z),
        new Vector3(x, -y, -z),
        new Vector3(x, -y, z),
        new Vector3(x, y, z),
        new Vector3(-x, y, z),
        new Vector3(-x, -y, z),
        new Vector3(-x, y, -z),
        new Vector3(-x, y, z),
        new Vector3(x, y, z),
        new Vector3(x, y, -z),
        new Vector3(x, -y, -z),
        new Vector3(x, -y, z),
        new Vector3(-x, -y, z),
        new Vector3(-x, -y, -z),
        new Vector3(-x, -y, z),
        new Vector3(-x, y, z),
        new Vector3(-x, y, -z),
        new Vector3(-x, -y, -z),
        new Vector3(x, -y, -z),
        new Vector3(x, y, -z),
        new Vector3(x, y, z),
        new Vector3(x, -y, z)
      };
      Vector3[] vector3Array2 = new Vector3[24]
      {
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward,
        Vector3.forward,
        Vector3.forward,
        Vector3.forward,
        Vector3.forward,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        -Vector3.up,
        -Vector3.up,
        -Vector3.up,
        -Vector3.up,
        -Vector3.right,
        -Vector3.right,
        -Vector3.right,
        -Vector3.right,
        Vector3.right,
        Vector3.right,
        Vector3.right,
        Vector3.right
      };
      int[] indices = new int[36]
      {
        0,
        1,
        2,
        2,
        3,
        0,
        4,
        5,
        6,
        6,
        7,
        4,
        8,
        9,
        10,
        10,
        11,
        8,
        12,
        13,
        14,
        14,
        15,
        12,
        16,
        17,
        18,
        18,
        19,
        16,
        20,
        21,
        22,
        22,
        23,
        20
      };
      Mesh box = new Mesh();
      box.vertices = vector3Array1;
      box.normals = vector3Array2;
      box.colors = ColorEx.GetFilledColorArray(24, color);
      box.SetIndices(indices, MeshTopology.Triangles, 0);
      box.UploadMeshData(false);
      return box;
    }

    public static Mesh CreateWireBox(float width, float height, float depth, Color color)
    {
      if ((double) width < 9.9999997473787516E-05)
        return (Mesh) null;
      if ((double) height < 9.9999997473787516E-05)
        return (Mesh) null;
      if ((double) depth < 9.9999997473787516E-05)
        return (Mesh) null;
      float x = width * 0.5f;
      float y = height * 0.5f;
      float z = depth * 0.5f;
      Vector3[] vector3Array1 = new Vector3[24]
      {
        new Vector3(-x, -y, -z),
        new Vector3(-x, y, -z),
        new Vector3(x, y, -z),
        new Vector3(x, -y, -z),
        new Vector3(x, -y, z),
        new Vector3(x, y, z),
        new Vector3(-x, y, z),
        new Vector3(-x, -y, z),
        new Vector3(-x, y, -z),
        new Vector3(-x, y, z),
        new Vector3(x, y, z),
        new Vector3(x, y, -z),
        new Vector3(x, -y, -z),
        new Vector3(x, -y, z),
        new Vector3(-x, -y, z),
        new Vector3(-x, -y, -z),
        new Vector3(-x, -y, z),
        new Vector3(-x, y, z),
        new Vector3(-x, y, -z),
        new Vector3(-x, -y, -z),
        new Vector3(x, -y, -z),
        new Vector3(x, y, -z),
        new Vector3(x, y, z),
        new Vector3(x, -y, z)
      };
      Vector3[] vector3Array2 = new Vector3[24]
      {
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward,
        -Vector3.forward,
        Vector3.forward,
        Vector3.forward,
        Vector3.forward,
        Vector3.forward,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        -Vector3.up,
        -Vector3.up,
        -Vector3.up,
        -Vector3.up,
        -Vector3.right,
        -Vector3.right,
        -Vector3.right,
        -Vector3.right,
        Vector3.right,
        Vector3.right,
        Vector3.right,
        Vector3.right
      };
      int[] indices = new int[48]
      {
        0,
        1,
        1,
        2,
        2,
        3,
        3,
        0,
        4,
        5,
        5,
        6,
        6,
        7,
        7,
        4,
        8,
        9,
        9,
        10,
        10,
        11,
        11,
        8,
        12,
        13,
        13,
        14,
        14,
        15,
        15,
        12,
        16,
        17,
        17,
        18,
        18,
        19,
        19,
        16,
        20,
        21,
        21,
        22,
        22,
        23,
        23,
        20
      };
      Mesh wireBox = new Mesh();
      wireBox.vertices = vector3Array1;
      wireBox.normals = vector3Array2;
      wireBox.colors = ColorEx.GetFilledColorArray(24, color);
      wireBox.SetIndices(indices, MeshTopology.Lines, 0);
      wireBox.UploadMeshData(false);
      return wireBox;
    }
  }
}
