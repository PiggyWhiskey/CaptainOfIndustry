// Decompiled with JetBrains decompiler
// Type: RTG.MeshTree
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
  public class MeshTree
  {
    private RTMesh _mesh;
    private SphereTree<MeshTriangle> _tree;
    private List<SphereTreeNode<MeshTriangle>> _nodeBuffer;
    private List<SphereTreeNodeRayHit<MeshTriangle>> _nodeHitBuffer;
    private HashSet<int> _vertexIndexSet;
    private bool _isBuilt;

    public bool IsBuilt => this._isBuilt;

    public MeshTree(RTMesh mesh)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._tree = new SphereTree<MeshTriangle>();
      this._nodeBuffer = new List<SphereTreeNode<MeshTriangle>>();
      this._nodeHitBuffer = new List<SphereTreeNodeRayHit<MeshTriangle>>();
      this._vertexIndexSet = new HashSet<int>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._mesh = mesh;
    }

    public void SetDirty()
    {
      this._isBuilt = false;
      this._tree = new SphereTree<MeshTriangle>();
    }

    public void Build()
    {
      if (this._isBuilt)
        return;
      for (int triangleIndex = 0; triangleIndex < this._mesh.NumTriangles; ++triangleIndex)
      {
        MeshTriangle triangle = this._mesh.GetTriangle(triangleIndex);
        this._tree.AddNode(triangle, new Sphere((IEnumerable<Vector3>) triangle.Vertices));
      }
      this._isBuilt = true;
    }

    public bool OverlapVerts(OBB obb, MeshTransform meshTransform, List<Vector3> verts)
    {
      verts.Clear();
      if (!this._isBuilt)
        this.Build();
      OBB box = meshTransform.InverseTransformOBB(obb);
      if (!this._tree.OverlapBox(box, this._nodeBuffer))
        return false;
      this._vertexIndexSet.Clear();
      foreach (SphereTreeNode<MeshTriangle> sphereTreeNode in this._nodeBuffer)
      {
        MeshTriangle triangle = this._mesh.GetTriangle(sphereTreeNode.Data.TriangleIndex);
        Vector3[] vertices = triangle.Vertices;
        for (int arrayIndex = 0; arrayIndex < vertices.Length; ++arrayIndex)
        {
          int vertIndex = triangle.GetVertIndex(arrayIndex);
          if (!this._vertexIndexSet.Contains(vertIndex))
          {
            Vector3 point = vertices[arrayIndex];
            if (BoxMath.ContainsPoint(point, box.Center, box.Size, box.Rotation))
            {
              verts.Add(meshTransform.TransformPoint(point));
              this._vertexIndexSet.Add(vertIndex);
            }
          }
        }
      }
      return verts.Count != 0;
    }

    public bool OverlapModelVerts(OBB modelOBB, List<Vector3> verts)
    {
      verts.Clear();
      if (!this._isBuilt)
        this.Build();
      if (!this._tree.OverlapBox(modelOBB, this._nodeBuffer))
        return false;
      this._vertexIndexSet.Clear();
      foreach (SphereTreeNode<MeshTriangle> sphereTreeNode in this._nodeBuffer)
      {
        MeshTriangle triangle = this._mesh.GetTriangle(sphereTreeNode.Data.TriangleIndex);
        Vector3[] vertices = triangle.Vertices;
        for (int arrayIndex = 0; arrayIndex < vertices.Length; ++arrayIndex)
        {
          int vertIndex = triangle.GetVertIndex(arrayIndex);
          if (!this._vertexIndexSet.Contains(vertIndex))
          {
            Vector3 point = vertices[arrayIndex];
            if (BoxMath.ContainsPoint(point, modelOBB.Center, modelOBB.Size, modelOBB.Rotation))
            {
              verts.Add(point);
              this._vertexIndexSet.Add(vertIndex);
            }
          }
        }
      }
      return verts.Count != 0;
    }

    /// <summary>
    /// Performs a raycast against the mesh triangles and returns info
    /// about the closest hit or null if no triangle was hit by the ray.
    /// </summary>
    /// <param name="meshTransform">
    /// The mesh transform which brings the mesh in the same space as the
    /// ray.
    /// </param>
    /// <remarks>
    /// This method will build the tree if it hasn't already been built.
    /// </remarks>
    public MeshRayHit RaycastClosest(Ray ray, Matrix4x4 meshTransform)
    {
      if (!this._isBuilt)
        this.Build();
      Ray ray1 = ray.InverseTransform(meshTransform);
      if (!this._tree.RaycastAll(ray1, this._nodeHitBuffer))
        return (MeshRayHit) null;
      float distance = float.MaxValue;
      MeshTriangle meshTriangle = (MeshTriangle) null;
      bool flag = false;
      foreach (SphereTreeNodeRayHit<MeshTriangle> sphereTreeNodeRayHit in this._nodeHitBuffer)
      {
        MeshTriangle data = sphereTreeNodeRayHit.HitNode.Data;
        float t;
        if (TriangleMath.Raycast(ray1, out t, data.Vertex0, data.Vertex1, data.Vertex2) && (double) Vector3.Dot(ray1.direction, data.Normal) < 0.0 && (double) t < (double) distance)
        {
          distance = t;
          meshTriangle = data;
          flag = true;
        }
      }
      if (!flag)
        return (MeshRayHit) null;
      Vector3 vector3 = meshTransform.MultiplyPoint(ray1.GetPoint(distance));
      float hitEnter = (ray.origin - vector3).magnitude / ray.direction.magnitude;
      Matrix4x4 matrix4x4 = meshTransform.inverse;
      matrix4x4 = matrix4x4.transpose;
      Vector3 normalized = matrix4x4.MultiplyVector(meshTriangle.Normal).normalized;
      return new MeshRayHit(ray, meshTriangle.TriangleIndex, hitEnter, normalized);
    }

    public void DebugDraw() => this._tree.DebugDraw();
  }
}
