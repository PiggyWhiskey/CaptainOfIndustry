// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap2DCollection
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
  public class GizmoCap2DCollection
  {
    private List<GizmoCap2D> _caps;
    private Dictionary<int, GizmoCap2D> _handleIdToCap;

    public int Count => this._caps.Count;

    public GizmoCap2D this[int id] => this._handleIdToCap[id];

    public bool Contains(GizmoCap2D cap) => this._handleIdToCap.ContainsKey(cap.HandleId);

    public bool Contains(int capHandleId) => this._handleIdToCap.ContainsKey(capHandleId);

    public void Add(GizmoCap2D cap)
    {
      if (this.Contains(cap))
        return;
      this._caps.Add(cap);
      this._handleIdToCap.Add(cap.HandleId, cap);
    }

    public void Remove(GizmoCap2D cap)
    {
      if (!this.Contains(cap))
        return;
      this._caps.Remove(cap);
      this._handleIdToCap.Remove(cap.HandleId);
    }

    public void Make2DHoverPriorityLowerThan(Priority priority)
    {
      foreach (GizmoCap cap in this._caps)
        cap.HoverPriority2D.MakeLowerThan(priority);
    }

    public void Make2DHoverPriorityHigherThan(Priority priority)
    {
      foreach (GizmoCap cap in this._caps)
        cap.HoverPriority2D.MakeHigherThan(priority);
    }

    public void SetVisible(bool visible)
    {
      foreach (GizmoCap cap in this._caps)
        cap.SetVisible(visible);
    }

    public void SetHoverable(bool hoverable)
    {
      foreach (GizmoCap cap in this._caps)
        cap.SetHoverable(hoverable);
    }

    public void SetDragSession(IGizmoDragSession dragSession)
    {
      foreach (GizmoCap2D cap in this._caps)
        cap.DragSession = dragSession;
    }

    public void Render(Camera camera)
    {
      foreach (GizmoCap cap in this._caps)
        cap.Render(camera);
    }

    public GizmoCap2DCollection()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._caps = new List<GizmoCap2D>();
      this._handleIdToCap = new Dictionary<int, GizmoCap2D>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
