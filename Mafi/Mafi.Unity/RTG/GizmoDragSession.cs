// Decompiled with JetBrains decompiler
// Type: RTG.GizmoDragSession
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
  public abstract class GizmoDragSession : IGizmoDragSession
  {
    private List<GizmoTransform> _targetTransforms;
    protected Vector3 _totalDragOffset;
    protected Quaternion _totalDragRotation;
    protected Vector3 _totalDragScale;
    protected Vector3 _relativeDragOffset;
    protected Quaternion _relativeDragRotation;
    protected Vector3 _relativeDragScale;

    public int NumTargetTransforms => this._targetTransforms.Count;

    public Vector3 TotalDragOffset => this._totalDragOffset;

    public Quaternion TotalDragRotation => this._totalDragRotation;

    public Vector3 TotalDragScale => this._totalDragScale;

    public Vector3 RelativeDragOffset => this._relativeDragOffset;

    public Quaternion RelativeDragRotation => this._relativeDragRotation;

    public Vector3 RelativeDragScale => this._relativeDragScale;

    public abstract bool IsActive { get; }

    public abstract GizmoDragChannel DragChannel { get; }

    public bool ContainsTargetTransform(GizmoTransform transform)
    {
      return this._targetTransforms.Contains(transform);
    }

    public void AddTargetTransform(GizmoTransform transform)
    {
      if (this.IsActive || this.ContainsTargetTransform(transform))
        return;
      this._targetTransforms.Add(transform);
    }

    public void RemoveTargetTransform(GizmoTransform transform)
    {
      if (this.IsActive)
        return;
      this._targetTransforms.Remove(transform);
    }

    public bool Begin()
    {
      if (!this.CanBegin() || !this.DoBeginSession())
        return false;
      this.OnSessionBegin();
      return true;
    }

    public bool Update()
    {
      if (!this.IsActive || !this.DoUpdateSession())
        return false;
      this.CalculateDragValues();
      this.ApplyDrag();
      return true;
    }

    public void End()
    {
      if (!this.IsActive)
        return;
      this.DoEndSession();
      this._totalDragOffset = this._relativeDragOffset = Vector3.zero;
      this._totalDragRotation = this._relativeDragRotation = Quaternion.identity;
      this._totalDragScale = this._relativeDragScale = Vector3.one;
      this.OnSessionEnd();
    }

    protected abstract bool DoBeginSession();

    protected abstract bool DoUpdateSession();

    protected abstract void DoEndSession();

    protected abstract void CalculateDragValues();

    protected void ApplyDrag()
    {
      List<GizmoTransform> gizmoTransformList = GizmoTransform.FilterParentsOnly((IEnumerable<GizmoTransform>) this._targetTransforms);
      if (this.DragChannel == GizmoDragChannel.Offset)
      {
        foreach (GizmoTransform gizmoTransform in gizmoTransformList)
          gizmoTransform.Position3D += this._relativeDragOffset;
      }
      else
      {
        if (this.DragChannel != GizmoDragChannel.Rotation)
          return;
        foreach (GizmoTransform gizmoTransform in gizmoTransformList)
          gizmoTransform.Rotation3D = this._relativeDragRotation * gizmoTransform.Rotation3D;
      }
    }

    protected virtual bool CanBegin() => !this.IsActive;

    protected virtual void OnSessionBegin()
    {
    }

    protected virtual void OnSessionEnd()
    {
    }

    protected GizmoDragSession()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._targetTransforms = new List<GizmoTransform>();
      this._totalDragRotation = Quaternion.identity;
      this._totalDragScale = Vector3.one;
      this._relativeDragRotation = Quaternion.identity;
      this._relativeDragScale = Vector3.one;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
