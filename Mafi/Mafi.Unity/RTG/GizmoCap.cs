// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class GizmoCap : IGizmoCap
  {
    private Gizmo _gizmo;
    private GizmoHandle _handle;
    private bool _isVisible;
    private bool _isHoverable;

    protected GizmoHandle Handle => this._handle;

    public Gizmo Gizmo => this._gizmo;

    public int HandleId => this._handle.Id;

    public bool IsVisible => this._isVisible;

    public bool IsHoverable => this._isHoverable;

    public bool IsHovered => this._gizmo.HoverHandleId == this.HandleId;

    public Priority HoverPriority3D => this.Handle.HoverPriority3D;

    public Priority HoverPriority2D => this.Handle.HoverPriority2D;

    public Priority GenericHoverPriority => this.Handle.GenericHoverPriority;

    public GizmoCap(Gizmo gizmo, int handleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._gizmo = gizmo;
      this._handle = this.Gizmo.CreateHandle(handleId);
    }

    public void SetVisible(bool isVisible)
    {
      if (this._isVisible == isVisible)
        return;
      this._isVisible = isVisible;
      this.OnVisibilityStateChanged();
    }

    public void SetHoverable(bool isHoverable)
    {
      if (this._isHoverable == isHoverable)
        return;
      this._isHoverable = isHoverable;
      this.OnHoverableStateChanged();
    }

    public abstract void Render(Camera camera);

    protected abstract void OnVisibilityStateChanged();

    protected abstract void OnHoverableStateChanged();
  }
}
