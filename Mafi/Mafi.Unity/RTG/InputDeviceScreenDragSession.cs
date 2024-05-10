// Decompiled with JetBrains decompiler
// Type: RTG.InputDeviceScreenDragSession
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class InputDeviceScreenDragSession
  {
    private Vector2 _dragPoint;
    private Vector2 _dragDelta;
    private Vector2 _accumDrag;
    private IInputDevice _inputDevice;
    private bool _isActive;

    public Vector2 DragPoint => this._dragPoint;

    public Vector2 DragDelta => this._dragDelta;

    public Vector2 AccumDrag => this._accumDrag;

    public bool IsActive => this._isActive;

    public InputDeviceScreenDragSession(IInputDevice inputDevice)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._inputDevice = inputDevice;
    }

    public bool Begin()
    {
      if (this._isActive)
        return false;
      this._isActive = this.UpdateDragPoint();
      return this._isActive;
    }

    public void End()
    {
      if (!this._isActive)
        return;
      this._isActive = false;
      this._dragPoint = (Vector2) Vector3.zero;
      this._dragDelta = (Vector2) Vector3.zero;
      this._accumDrag = (Vector2) Vector3.zero;
    }

    public bool Update()
    {
      if (!this._isActive)
        return false;
      Vector2 dragPoint = this._dragPoint;
      if (!this.UpdateDragPoint())
      {
        this._dragDelta = (Vector2) Vector3.zero;
        return false;
      }
      this._dragDelta = this._dragPoint - dragPoint;
      this._accumDrag += this._dragDelta;
      return true;
    }

    private bool UpdateDragPoint()
    {
      if (!this._inputDevice.HasPointer())
        return false;
      this._dragPoint = (Vector2) this._inputDevice.GetPositionYAxisUp();
      return true;
    }
  }
}
