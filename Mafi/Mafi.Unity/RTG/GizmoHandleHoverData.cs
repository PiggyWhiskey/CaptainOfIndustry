// Decompiled with JetBrains decompiler
// Type: RTG.GizmoHandleHoverData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoHandleHoverData
  {
    private int _handleId;
    private Gizmo _gizmo;
    private GizmoDimension _handleDimension;
    private Ray _hoverRay;
    private Vector3 _hoverPoint;
    private float _hoverEnter3D;

    public int HandleId => this._handleId;

    public Gizmo Gizmo => this._gizmo;

    public GizmoDimension HandleDimension => this._handleDimension;

    public Ray HoverRay => this._hoverRay;

    public Vector3 HoverPoint => this._hoverPoint;

    public float HoverEnter3D => this._hoverEnter3D;

    public GizmoHandleHoverData(Ray hoverRay, IGizmoHandle gizmoHandle, float hoverEnter3D)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._handleId = gizmoHandle.Id;
      this._gizmo = gizmoHandle.Gizmo;
      this._handleDimension = GizmoDimension.Dim3D;
      this._hoverRay = hoverRay;
      this._hoverEnter3D = hoverEnter3D;
      this._hoverPoint = this._hoverRay.GetPoint(this._hoverEnter3D);
    }

    public GizmoHandleHoverData(Ray hoverRay, IGizmoHandle gizmoHandle, Vector2 hoverPt2D)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._handleId = gizmoHandle.Id;
      this._gizmo = gizmoHandle.Gizmo;
      this._handleDimension = GizmoDimension.Dim2D;
      this._hoverRay = hoverRay;
      this._hoverPoint = (Vector3) hoverPt2D;
    }
  }
}
