// Decompiled with JetBrains decompiler
// Type: RTG.GizmoSlider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  /// <summary>
  /// Abstract slider base class which can be derived to define behaviours
  /// for different types of sliders. A slider is a gizmo entity that allows
  /// for moving, rotating and/or scaling entities.
  /// </summary>
  public abstract class GizmoSlider : IGizmoSlider
  {
    private GizmoHandle _handle;
    private Gizmo _gizmo;
    private bool _isVisible;
    private bool _isHoverable;

    protected GizmoHandle Handle => this._handle;

    /// <summary>Returns the gizmo that owns the slider handle.</summary>
    public Gizmo Gizmo => this._gizmo;

    /// <summary>Returns the id of the slider handle.</summary>
    public int HandleId => this._handle.Id;

    /// <summary>Checks if the slider is visible.</summary>
    public bool IsVisible => this._isVisible;

    /// <summary>Checks if the slider is hoverable.</summary>
    public bool IsHoverable => this._isHoverable;

    /// <summary>Checks if the slider is hovered.</summary>
    public bool IsHovered => this._gizmo.HoverHandleId == this.HandleId;

    /// <summary>
    /// Returns the slider's 3D hover priority. This property is only useful
    /// when the slider uses 3D shapes.
    /// </summary>
    public Priority HoverPriority3D => this.Handle.HoverPriority3D;

    /// <summary>
    /// Returns the slider's 2D hover priority. This property is only useful
    /// when the slider uses 2D shapes.
    /// </summary>
    public Priority HoverPriority2D => this.Handle.HoverPriority2D;

    /// <summary>Returns the slider's generic hover priority.</summary>
    public Priority GenericHoverPriority => this.Handle.GenericHoverPriority;

    /// <summary>Constructor.</summary>
    /// <param name="gizmo">The gizmo which owns the slider.</param>
    /// <param name="handleId">The id of the slider handle.</param>
    public GizmoSlider(Gizmo gizmo, int handleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._gizmo = gizmo;
      this._handle = this.Gizmo.CreateHandle(handleId);
    }

    /// <summary>
    /// Sets the visibility state of the slider. A visible slider will be rendered,
    /// and it can also be hovered if it is set to be hoverable (see 'SetHoverable').
    /// </summary>
    public void SetVisible(bool isVisible)
    {
      this._isVisible = isVisible;
      this.OnVisibilityStateChanged();
    }

    /// <summary>
    /// Sets the hoverable state of the slider. A hoverable slider can be hovered
    /// ONLY if it is visible (see 'SetVisible'). So passing true to this function
    /// will only allow the slider to be hovered if it is also visible.
    /// </summary>
    public void SetHoverable(bool isHoverable)
    {
      this._isHoverable = isHoverable;
      this.OnHoverableStateChanged();
    }

    public abstract void SetSnapEnabled(bool isEnabled);

    public abstract void Render(Camera camera);

    protected abstract void OnVisibilityStateChanged();

    protected abstract void OnHoverableStateChanged();
  }
}
