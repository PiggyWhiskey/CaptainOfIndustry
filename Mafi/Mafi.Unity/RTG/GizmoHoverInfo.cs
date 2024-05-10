// Decompiled with JetBrains decompiler
// Type: RTG.GizmoHoverInfo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public struct GizmoHoverInfo
  {
    private bool _isHovered;
    private int _handleId;
    private GizmoDimension _handleDimension;
    private Vector3 _hoverPoint;

    public bool IsHovered
    {
      get => this._isHovered;
      set => this._isHovered = value;
    }

    public int HandleId
    {
      get => this._handleId;
      set => this._handleId = value;
    }

    public GizmoDimension HandleDimension
    {
      get => this._handleDimension;
      set => this._handleDimension = value;
    }

    public Vector3 HoverPoint
    {
      get => this._hoverPoint;
      set => this._hoverPoint = value;
    }

    public void Reset()
    {
      this._isHovered = false;
      this._handleId = GizmoHandleId.None;
      this._handleDimension = GizmoDimension.None;
      this._hoverPoint = Vector3.zero;
    }
  }
}
