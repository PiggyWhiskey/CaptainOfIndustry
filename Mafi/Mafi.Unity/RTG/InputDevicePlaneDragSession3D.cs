// Decompiled with JetBrains decompiler
// Type: RTG.InputDevicePlaneDragSession3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class InputDevicePlaneDragSession3D
  {
    private Plane _plane;
    private Camera _raycastCamera;
    private Vector3 _dragPoint;
    private Vector3 _dragDelta;
    private Vector3 _accumDrag;
    private IInputDevice _inputDevice;
    private bool _isActive;

    public Plane Plane
    {
      get => this._plane;
      set
      {
        if (this._isActive)
          return;
        this._plane = value;
      }
    }

    public Camera RaycastCamera
    {
      get => this._raycastCamera;
      set
      {
        if (this._isActive)
          return;
        this._raycastCamera = value;
      }
    }

    public Vector3 DragPoint => this._dragPoint;

    public Vector3 DragDelta => this._dragDelta;

    public Vector3 AccumDrag => this._accumDrag;

    public bool IsActive => this._isActive;

    public InputDevicePlaneDragSession3D(IInputDevice inputDevice, Camera raycastCamera)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._inputDevice = inputDevice;
      this._raycastCamera = raycastCamera;
    }

    public bool Begin()
    {
      if (this._isActive || (Object) this._raycastCamera == (Object) null)
        return false;
      this._isActive = this.UpdateDragPoint();
      return this._isActive;
    }

    public void End()
    {
      if (!this._isActive)
        return;
      this._isActive = false;
      this._dragPoint = Vector3.zero;
      this._dragDelta = Vector3.zero;
      this._accumDrag = Vector3.zero;
      this._plane = new Plane();
    }

    public bool Update()
    {
      if (!this._isActive)
        return false;
      Vector3 dragPoint = this._dragPoint;
      if (!this.UpdateDragPoint())
      {
        this._dragDelta = Vector3.zero;
        return false;
      }
      this._dragDelta = this._dragPoint - dragPoint;
      this._accumDrag += this._dragDelta;
      return true;
    }

    private bool UpdateDragPoint()
    {
      Ray ray = this._inputDevice.GetRay(this._raycastCamera);
      float enter;
      if (!this._plane.Raycast(ray, out enter))
        return false;
      this._dragPoint = ray.GetPoint(enter);
      return true;
    }
  }
}
