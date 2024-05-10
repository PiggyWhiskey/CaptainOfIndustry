// Decompiled with JetBrains decompiler
// Type: RTG.MeshVertexChunkCollection
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
  public class MeshVertexChunkCollection : IEnumerable<MeshVertexChunk>, IEnumerable
  {
    private Mesh _mesh;
    private List<MeshVertexChunk> _vertexChunks;

    public MeshVertexChunk this[int chunkIndex] => this._vertexChunks[chunkIndex];

    public int Count => this._vertexChunks.Count;

    public IEnumerator<MeshVertexChunk> GetEnumerator()
    {
      return (IEnumerator<MeshVertexChunk>) this._vertexChunks.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public List<MeshVertexChunk> GetWorldChunksHoveredByPoint(
      Vector3 hoverPoint,
      Matrix4x4 worldMtx,
      Camera camera)
    {
      List<MeshVertexChunk> chunksHoveredByPoint = new List<MeshVertexChunk>();
      foreach (MeshVertexChunk vertexChunk in this._vertexChunks)
      {
        AABB modelSpaceAabb = vertexChunk.ModelSpaceAABB;
        modelSpaceAabb.Transform(worldMtx);
        if (modelSpaceAabb.GetScreenRectangle(camera).Contains(hoverPoint, true))
          chunksHoveredByPoint.Add(vertexChunk);
      }
      return chunksHoveredByPoint;
    }

    public MeshVertexChunk GetWorldVertChunkClosestToScreenPt(
      Vector2 screenPoint,
      Matrix4x4 worldMtx,
      Camera camera)
    {
      float num = float.MaxValue;
      MeshVertexChunk closestToScreenPt = (MeshVertexChunk) null;
      foreach (MeshVertexChunk vertexChunk in this._vertexChunks)
      {
        AABB modelSpaceAabb = vertexChunk.ModelSpaceAABB;
        modelSpaceAabb.Transform(worldMtx);
        foreach (Vector2 centerAndCornerPoint in modelSpaceAabb.GetScreenCenterAndCornerPoints(camera))
        {
          float sqrMagnitude = (centerAndCornerPoint - screenPoint).sqrMagnitude;
          if ((double) sqrMagnitude < (double) num)
          {
            num = sqrMagnitude;
            closestToScreenPt = vertexChunk;
          }
        }
      }
      return closestToScreenPt;
    }

    public bool FromMesh(Mesh mesh)
    {
      if ((Object) mesh == (Object) null || !mesh.isReadable)
        return false;
      mesh.RecalculateBounds();
      Bounds bounds = mesh.bounds;
      Vector3[] vertices = mesh.vertices;
      float num1 = bounds.size.x / 2f;
      float num2 = bounds.size.y / 2f;
      float num3 = bounds.size.z / 2f;
      Dictionary<MeshVertexChunkCollection.VertexChunkIndices, List<Vector3>> dictionary = new Dictionary<MeshVertexChunkCollection.VertexChunkIndices, List<Vector3>>();
      foreach (Vector3 vector3 in vertices)
      {
        MeshVertexChunkCollection.VertexChunkIndices key = new MeshVertexChunkCollection.VertexChunkIndices(Mathf.FloorToInt(vector3.x / num1), Mathf.FloorToInt(vector3.y / num2), Mathf.FloorToInt(vector3.z / num3));
        if (dictionary.ContainsKey(key))
        {
          dictionary[key].Add(vector3);
        }
        else
        {
          dictionary.Add(key, new List<Vector3>(50));
          dictionary[key].Add(vector3);
        }
      }
      if (dictionary.Count == 0)
        return false;
      this._mesh = mesh;
      this._vertexChunks.Clear();
      foreach (KeyValuePair<MeshVertexChunkCollection.VertexChunkIndices, List<Vector3>> keyValuePair in dictionary)
      {
        if (keyValuePair.Value.Count != 0)
          this._vertexChunks.Add(new MeshVertexChunk(keyValuePair.Value, this._mesh));
      }
      return true;
    }

    public MeshVertexChunkCollection()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._vertexChunks = new List<MeshVertexChunk>(50);
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private struct VertexChunkIndices
    {
      private int _XIndex;
      private int _YIndex;
      private int _ZIndex;

      public int XIndex => this._XIndex;

      public int YIndex => this._YIndex;

      public int ZIndex => this._ZIndex;

      public VertexChunkIndices(int xIndex, int yIndex, int zIndex)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this._XIndex = xIndex;
        this._YIndex = yIndex;
        this._ZIndex = zIndex;
      }
    }
  }
}
