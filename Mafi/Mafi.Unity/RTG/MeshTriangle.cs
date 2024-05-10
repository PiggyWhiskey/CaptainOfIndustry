// Decompiled with JetBrains decompiler
// Type: RTG.MeshTriangle
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class MeshTriangle
  {
    private Vector3[] _vertices;
    private Vector3 _normal;
    private int _triangleIndex;
    private int[] _vertIndices;

    public int TriangleIndex => this._triangleIndex;

    public Vector3[] Vertices => this._vertices.Clone() as Vector3[];

    public Vector3 Vertex0 => this._vertices[0];

    public Vector3 Vertex1 => this._vertices[1];

    public Vector3 Vertex2 => this._vertices[2];

    public Vector3 Normal => this._normal;

    public int[] VertIndices => this._vertIndices.Clone() as int[];

    public int VertIndex0 => this._vertIndices[0];

    public int VertIndex1 => this._vertIndices[1];

    public int VertIndex2 => this._vertIndices[2];

    public MeshTriangle(
      Vector3[] vertices,
      int triangleIndex,
      int vertIndex0,
      int vertIndex1,
      int vertIndex2)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._vertices = vertices.Clone() as Vector3[];
      this._triangleIndex = triangleIndex;
      this._vertIndices = new int[3];
      this._vertIndices[0] = vertIndex0;
      this._vertIndices[1] = vertIndex1;
      this._vertIndices[2] = vertIndex2;
      this._normal = Vector3.Cross(this._vertices[1] - this._vertices[0], this._vertices[2] - this._vertices[0]).normalized;
    }

    public int GetVertIndex(int arrayIndex) => this._vertIndices[arrayIndex];
  }
}
