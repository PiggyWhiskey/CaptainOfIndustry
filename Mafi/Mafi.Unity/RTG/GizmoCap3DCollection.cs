// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap3DCollection
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoCap3DCollection
  {
    private List<GizmoCap3D> _caps;
    private Dictionary<int, GizmoCap3D> _handleIdToCap;

    public int Count => this._caps.Count;

    public GizmoCap3D this[int id] => this._handleIdToCap[id];

    public bool Contains(GizmoCap3D cap) => this._handleIdToCap.ContainsKey(cap.HandleId);

    public bool Contains(int capHandleId) => this._handleIdToCap.ContainsKey(capHandleId);

    public void Add(GizmoCap3D cap)
    {
      if (this.Contains(cap))
        return;
      this._caps.Add(cap);
      this._handleIdToCap.Add(cap.HandleId, cap);
    }

    public void Remove(GizmoCap3D cap)
    {
      if (!this.Contains(cap))
        return;
      this._caps.Remove(cap);
      this._handleIdToCap.Remove(cap.HandleId);
    }

    public void ApplyZoomFactor(Camera camera)
    {
      foreach (GizmoCap3D cap in this._caps)
        cap.ApplyZoomFactor(camera);
    }

    public void SetZoomFactorTransform(GizmoTransform zoomFactorTransform)
    {
      foreach (GizmoCap3D cap in this._caps)
        cap.SetZoomFactorTransform(zoomFactorTransform);
    }

    public void Make3DHoverPriorityLowerThan(Priority priority)
    {
      foreach (GizmoCap cap in this._caps)
        cap.HoverPriority3D.MakeLowerThan(priority);
    }

    public void Make3DHoverPriorityHigherThan(Priority priority)
    {
      foreach (GizmoCap cap in this._caps)
        cap.HoverPriority3D.MakeHigherThan(priority);
    }

    public void SetVisible(bool visible)
    {
      foreach (GizmoCap cap in this._caps)
        cap.SetVisible(visible);
    }

    public List<GizmoCap3D> GetRenderSortedCaps(Camera renderCamera)
    {
      List<GizmoCap3D> renderSortedCaps = new List<GizmoCap3D>((IEnumerable<GizmoCap3D>) this._caps);
      Vector3 cameraPos = renderCamera.transform.position;
      renderSortedCaps.Sort((Comparison<GizmoCap3D>) ((c0, c1) =>
      {
        float sqrMagnitude = (c0.Position - cameraPos).sqrMagnitude;
        return (c1.Position - cameraPos).sqrMagnitude.CompareTo(sqrMagnitude);
      }));
      return renderSortedCaps;
    }

    public GizmoCap3DCollection()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._caps = new List<GizmoCap3D>();
      this._handleIdToCap = new Dictionary<int, GizmoCap3D>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
