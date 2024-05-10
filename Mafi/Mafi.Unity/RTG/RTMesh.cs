// Decompiled with JetBrains decompiler
// Type: RTG.RTMesh
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class RTMesh
  {
    private Mesh _unityMesh;
    private Vector3[] _vertices;
    private int[] _vertIndices;
    private int _numTriangles;
    private AABB _aabb;
    private MeshTree _meshTree;

    public int NumTriangles => this._numTriangles;

    public Mesh UnityMesh => this._unityMesh;

    public AABB AABB => this._aabb;

    public bool IsTreeBuilt => this._meshTree.IsBuilt;

    public RTMesh(Mesh unityMesh)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._unityMesh = unityMesh;
      this._vertices = this._unityMesh.vertices;
      this._vertIndices = this._unityMesh.triangles;
      this._numTriangles = this._vertIndices.Length / 3;
      this._meshTree = new MeshTree(this);
      this._aabb = new AABB(this._unityMesh.bounds);
    }

    /// <summary>
    /// Factory function for creating an RTMesh instance from the specified
    /// Unity mesh. The function will return null if the RTMesh can not be
    /// created. Such a scenario occurs when the Unity mesh is not readable.
    /// </summary>
    /// <remarks>
    /// The client code is responsible for calling 'BuildTree' for the returned
    /// mesh.
    /// </remarks>
    public static RTMesh Create(Mesh unityMesh)
    {
      return (Object) unityMesh == (Object) null || !unityMesh.isReadable ? (RTMesh) null : new RTMesh(unityMesh);
    }

    public void BuildTree() => this._meshTree.Build();

    public void SetDirty()
    {
      this._vertices = this._unityMesh.vertices;
      this._vertIndices = this._unityMesh.triangles;
      this._numTriangles = this._vertIndices.Length / 3;
      this._unityMesh.RecalculateBounds();
      this._aabb = new AABB(this._unityMesh.bounds);
      this._meshTree.SetDirty();
    }

    public MeshTriangle GetTriangle(int triangleIndex)
    {
      int index = triangleIndex * 3;
      int vertIndex1 = this._vertIndices[index];
      int vertIndex2 = this._vertIndices[index + 1];
      int vertIndex3 = this._vertIndices[index + 2];
      return new MeshTriangle(new Vector3[3]
      {
        this._vertices[vertIndex1],
        this._vertices[vertIndex2],
        this._vertices[vertIndex3]
      }, triangleIndex, vertIndex1, vertIndex2, vertIndex3);
    }

    public MeshRayHit Raycast(Ray ray, Matrix4x4 meshTransform)
    {
      return this._meshTree.RaycastClosest(ray, meshTransform);
    }

    public bool OverlapVerts(OBB obb, Transform meshObjectTransform, List<Vector3> verts)
    {
      return this._meshTree.OverlapVerts(obb, new MeshTransform(meshObjectTransform), verts);
    }

    public bool OverlapModelVerts(OBB modelOBB, List<Vector3> verts)
    {
      return this._meshTree.OverlapModelVerts(modelOBB, verts);
    }

    public bool OverlapModelVerts(AABB modelAABB, List<Vector3> verts)
    {
      return this._meshTree.OverlapModelVerts(new OBB(modelAABB), verts);
    }

    public void DebugDrawTree() => this._meshTree.DebugDraw();
  }
}
