// Decompiled with JetBrains decompiler
// Type: RTG.RTCameraViewports
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
  public class RTCameraViewports : Singleton<RTCameraViewports>
  {
    private List<Camera> _cameras;

    public event RTCameraViewports.CameraAddedHandler CameraAdded;

    public event RTCameraViewports.CameraRemovedHandler CameraRemoved;

    public event RTCameraViewports.FocusCameraChangedHandler FocusCameraChanged;

    public Camera FocusCamera => MonoSingleton<RTFocusCamera>.Get.TargetCamera;

    public int NumCameras => this._cameras.Count;

    public bool ContainsCamera(Camera camera) => this._cameras.Contains(camera);

    public void AddCamera(Camera camera, Rect normViewRect)
    {
      if (!((Object) camera != (Object) null) || this.ContainsCamera(camera))
        return;
      this._cameras.Add(camera);
      camera.rect = normViewRect;
      if (this.CameraAdded == null)
        return;
      this.CameraAdded(camera);
    }

    public void AddCamera(Camera camera)
    {
      if (!((Object) camera != (Object) null) || this.ContainsCamera(camera))
        return;
      this._cameras.Add(camera);
      if (this.CameraAdded == null)
        return;
      this.CameraAdded(camera);
    }

    public void RemoveCamera(Camera camera)
    {
      if ((Object) camera == (Object) null)
        return;
      this._cameras.Remove(camera);
      if (this.CameraRemoved == null)
        return;
      this.CameraRemoved(camera);
    }

    public void SetFocusCamera(int cameraIndex)
    {
      if (cameraIndex < 0 || cameraIndex >= this.NumCameras)
        return;
      this.SetFocusCamera(this._cameras[cameraIndex]);
    }

    public void SetFocusCamera(Camera camera)
    {
      if (!((Object) this.FocusCamera != (Object) camera) || !this.ContainsCamera(camera))
        return;
      Camera focusCamera = this.FocusCamera;
      MonoSingleton<RTFocusCamera>.Get.SetTargetCamera(camera);
      if (this.FocusCameraChanged == null)
        return;
      this.FocusCameraChanged(focusCamera, this.FocusCamera);
    }

    public RTCameraViewports()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._cameras = new List<Camera>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public delegate void CameraAddedHandler(Camera camera);

    public delegate void CameraRemovedHandler(Camera camera);

    public delegate void FocusCameraChangedHandler(Camera oldFocusCam, Camera newFocusCam);
  }
}
