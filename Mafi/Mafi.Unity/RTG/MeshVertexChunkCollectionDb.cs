// Decompiled with JetBrains decompiler
// Type: RTG.MeshVertexChunkCollectionDb
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
  public class MeshVertexChunkCollectionDb : Singleton<MeshVertexChunkCollectionDb>
  {
    private Dictionary<Mesh, MeshVertexChunkCollection> _meshToVChunkCollection;

    public MeshVertexChunkCollection this[Mesh mesh]
    {
      get
      {
        return !this.HasChunkCollectionForMesh(mesh) && !this.CreateMeshVertChunkCollection(mesh) ? (MeshVertexChunkCollection) null : this._meshToVChunkCollection[mesh];
      }
    }

    public void SetMeshDirty(Mesh mesh)
    {
      MeshVertexChunkCollection vertexChunkCollection = (MeshVertexChunkCollection) null;
      if (!this._meshToVChunkCollection.TryGetValue(mesh, out vertexChunkCollection))
        return;
      vertexChunkCollection.FromMesh(mesh);
    }

    public bool HasChunkCollectionForMesh(Mesh mesh)
    {
      return this._meshToVChunkCollection.ContainsKey(mesh);
    }

    private bool CreateMeshVertChunkCollection(Mesh mesh)
    {
      MeshVertexChunkCollection vertexChunkCollection = new MeshVertexChunkCollection();
      if (!vertexChunkCollection.FromMesh(mesh))
        return false;
      this._meshToVChunkCollection.Add(mesh, vertexChunkCollection);
      return true;
    }

    public MeshVertexChunkCollectionDb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._meshToVChunkCollection = new Dictionary<Mesh, MeshVertexChunkCollection>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
