// Decompiled with JetBrains decompiler
// Type: RTG.RTMeshDb
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
  public class RTMeshDb : Singleton<RTMeshDb>
  {
    private Dictionary<Mesh, RTMesh> _meshes;

    public bool Contains(RTMesh rtMesh)
    {
      return rtMesh != null && this._meshes.ContainsKey(rtMesh.UnityMesh);
    }

    public bool Contains(Mesh unityMesh)
    {
      return !((Object) unityMesh == (Object) null) && this._meshes.ContainsKey(unityMesh);
    }

    public void SetMeshDirty(Mesh unityMesh)
    {
      RTMesh rtMesh = (RTMesh) null;
      if (!this._meshes.TryGetValue(unityMesh, out rtMesh))
        return;
      rtMesh.SetDirty();
    }

    /// <summary>
    /// Given a Unity mesh, the method will return the associated RTMesh. The
    /// method will attempt to create an RTMesh instance if one doesn't exist
    /// for the passed Unity mesh. If no RTMesh is associated with the Unity
    /// mesh and if one can not be created, null will be returned.
    /// </summary>
    /// <remarks>
    /// The client code is responsible for calling 'BuildTree' for the returned
    /// mesh. This would only be necessary if the method had to create a new
    /// RTMesh instance in case one didn't already exist.
    /// </remarks>
    public RTMesh GetRTMesh(Mesh unityMesh)
    {
      if ((Object) unityMesh == (Object) null)
        return (RTMesh) null;
      return !this._meshes.ContainsKey(unityMesh) ? this.CreateRTMesh(unityMesh) : this._meshes[unityMesh];
    }

    public void OnMeshWillBeDestroyed(Mesh unityMesh)
    {
      if (!this._meshes.ContainsKey(unityMesh))
        return;
      this._meshes.Remove(unityMesh);
    }

    /// <summary>
    /// Creates an RTMesh from the passed Unity mesh. The method returns the
    /// RTMesh instance or null if the mesh can not be created.
    /// </summary>
    /// <remarks>
    /// The client code is responsible for calling 'BuildTree' for the returned
    /// mesh.
    /// </remarks>
    private RTMesh CreateRTMesh(Mesh unityMesh)
    {
      RTMesh rtMesh = RTMesh.Create(unityMesh);
      if (rtMesh == null)
        return (RTMesh) null;
      this._meshes.Add(unityMesh, rtMesh);
      return rtMesh;
    }

    public RTMeshDb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._meshes = new Dictionary<Mesh, RTMesh>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
