// Decompiled with JetBrains decompiler
// Type: RTG.GizmoDragInfo
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public struct GizmoDragInfo
  {
    private bool _isDragged;
    private int _handleId;
    private Vector3 _dragBeginPoint;
    private GizmoDragChannel _dragChannel;
    private GizmoDimension _handleDimension;
    private Vector3 _totalOffset;
    private Quaternion _totalRotation;
    private Vector3 _totalScale;
    private Vector3 _relativeOffset;
    private Quaternion _relativeRotation;
    private Vector3 _relativeScale;

    public bool IsDragged
    {
      get => this._isDragged;
      set => this._isDragged = value;
    }

    public int HandleId
    {
      get => this._handleId;
      set => this._handleId = value;
    }

    public Vector3 DragBeginPoint
    {
      get => this._dragBeginPoint;
      set => this._dragBeginPoint = value;
    }

    public GizmoDragChannel DragChannel
    {
      get => this._dragChannel;
      set => this._dragChannel = value;
    }

    public GizmoDimension HandleDimension
    {
      get => this._handleDimension;
      set => this._handleDimension = value;
    }

    public Vector3 TotalOffset
    {
      get => this._totalOffset;
      set => this._totalOffset = value;
    }

    public Quaternion TotalRotation
    {
      get => this._totalRotation;
      set => this._totalRotation = value;
    }

    public Vector3 TotalScale
    {
      get => this._totalScale;
      set => this._totalScale = value;
    }

    public Vector3 RelativeOffset
    {
      get => this._relativeOffset;
      set => this._relativeOffset = value;
    }

    public Quaternion RelativeRotation
    {
      get => this._relativeRotation;
      set => this._relativeRotation = value;
    }

    public Vector3 RelativeScale
    {
      get => this._relativeScale;
      set => this._relativeScale = value;
    }

    public void Reset()
    {
      this._isDragged = false;
      this._handleId = GizmoHandleId.None;
      this._dragBeginPoint = Vector3.zero;
      this._dragChannel = GizmoDragChannel.None;
      this._handleDimension = GizmoDimension.None;
      this._totalOffset = this._relativeOffset = Vector3.zero;
      this._totalRotation = this._relativeRotation = Quaternion.identity;
      this._totalScale = this._relativeScale = Vector3.one;
    }
  }
}
