// Decompiled with JetBrains decompiler
// Type: RTG.MeshVertexChunk
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class MeshVertexChunk : IEnumerable<Vector3>, IEnumerable
  {
    private List<Vector3> _modelSpaceVerts;
    private AABB _modelSpaceAABB;
    private Mesh _mesh;

    public Vector3 this[int vertexIndex] => this._modelSpaceVerts[vertexIndex];

    public Mesh Mesh => this._mesh;

    public int VertexCount => this._modelSpaceVerts.Count;

    public AABB ModelSpaceAABB => this._modelSpaceAABB;

    public MeshVertexChunk(List<Vector3> modelSpaceVerts, Mesh mesh)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._modelSpaceVerts = new List<Vector3>(100);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._modelSpaceVerts = new List<Vector3>((IEnumerable<Vector3>) modelSpaceVerts);
      this._mesh = mesh;
      this._modelSpaceAABB = new AABB((IEnumerable<Vector3>) this._modelSpaceVerts);
    }

    public IEnumerator<Vector3> GetEnumerator()
    {
      return (IEnumerator<Vector3>) this._modelSpaceVerts.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public Vector3 GetWorldVertClosestToScreenPt(
      Vector2 screenPoint,
      Matrix4x4 worldMtx,
      Camera camera)
    {
      float num = float.MaxValue;
      Vector3 closestToScreenPt = Vector3.zero;
      foreach (Vector3 modelSpaceVert in this._modelSpaceVerts)
      {
        Vector3 position = worldMtx.MultiplyPoint(modelSpaceVert);
        float sqrMagnitude = ((Vector2) camera.WorldToScreenPoint(position) - screenPoint).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num)
        {
          num = sqrMagnitude;
          closestToScreenPt = position;
        }
      }
      return closestToScreenPt;
    }
  }
}
