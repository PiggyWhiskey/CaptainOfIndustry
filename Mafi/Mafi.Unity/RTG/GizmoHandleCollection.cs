// Decompiled with JetBrains decompiler
// Type: RTG.GizmoHandleCollection
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
  public class GizmoHandleCollection
  {
    private Gizmo _gizmo;
    private List<IGizmoHandle> _handles;
    private Dictionary<int, IGizmoHandle> _idToHandle;

    public Gizmo Gizmo => this._gizmo;

    public int Count => this._handles.Count;

    public IGizmoHandle this[int index] => this._handles[index];

    public GizmoHandleCollection(Gizmo gizmo)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._handles = new List<IGizmoHandle>();
      this._idToHandle = new Dictionary<int, IGizmoHandle>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._gizmo = gizmo;
    }

    public void Clear()
    {
      this._handles.Clear();
      this._idToHandle.Clear();
    }

    public IGizmoHandle GetHandleById(int handleId) => this._idToHandle[handleId];

    public bool Contains(IGizmoHandle handle) => this._idToHandle.ContainsKey(handle.Id);

    public bool Contains(int handleId) => this._idToHandle.ContainsKey(handleId);

    public void Add(IGizmoHandle handle)
    {
      if (this.Contains(handle) || handle.Gizmo != this.Gizmo)
        return;
      this._handles.Add(handle);
      this._idToHandle.Add(handle.Id, handle);
    }

    public void Remove(IGizmoHandle handle)
    {
      this._handles.Remove(handle);
      this._idToHandle.Remove(handle.Id);
    }

    public List<IGizmoHandle> GetAll()
    {
      return new List<IGizmoHandle>((IEnumerable<IGizmoHandle>) this._handles);
    }

    public List<GizmoHandleHoverData> GetAllHandlesHoverData(Ray hoverRay)
    {
      List<GizmoHandleHoverData> handlesHoverData = new List<GizmoHandleHoverData>(10);
      foreach (IGizmoHandle handle in this._handles)
      {
        GizmoHandleHoverData hoverData = handle.GetHoverData(hoverRay);
        if (hoverData != null)
          handlesHoverData.Add(hoverData);
      }
      return handlesHoverData;
    }
  }
}
