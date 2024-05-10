// Decompiled with JetBrains decompiler
// Type: RTG.RTCameraBackground
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
  public class RTCameraBackground : MonoSingleton<RTCameraBackground>
  {
    [SerializeField]
    private CameraBackgroundSettings _bkSettings;
    [SerializeField]
    private List<Camera> _renderIgnoreCameras;
    private Dictionary<Camera, CameraBackgroundSettings> _cameraToBkSettings;

    public CameraBackgroundSettings Settings => this._bkSettings;

    public void SetCameraBkSettings(Camera camera, CameraBackgroundSettings bkSettings)
    {
      if (bkSettings == null && this._cameraToBkSettings.ContainsKey(camera))
        this._cameraToBkSettings.Remove(camera);
      else if (!this._cameraToBkSettings.ContainsKey(camera))
        this._cameraToBkSettings.Add(camera, bkSettings);
      else
        this._cameraToBkSettings[camera] = bkSettings;
    }

    public List<Camera> GetAllRenderIgnoreCameras()
    {
      return new List<Camera>((IEnumerable<Camera>) this._renderIgnoreCameras);
    }

    public bool IsRenderIgnoreCamera(Camera camera) => this._renderIgnoreCameras.Contains(camera);

    public void AddRenderIgnoreCamera(Camera camera)
    {
      if (this.IsRenderIgnoreCamera(camera))
        return;
      this._renderIgnoreCameras.Add(camera);
    }

    public void RemoveRenderIgnoreCamera(Camera camera) => this._renderIgnoreCameras.Remove(camera);

    public void Render_SystemCall(Camera renderCamera)
    {
      if (this.IsRenderIgnoreCamera(renderCamera))
        return;
      CameraBackgroundSettings backgroundSettings = this._bkSettings;
      if (this._cameraToBkSettings.ContainsKey(renderCamera))
        backgroundSettings = this._cameraToBkSettings[renderCamera];
      if (!backgroundSettings.IsVisible)
        return;
      Transform transform = renderCamera.transform;
      QuadShape3D quadShape3D = new QuadShape3D();
      float widthFromDistance = renderCamera.GetFrustumWidthFromDistance(renderCamera.farClipPlane);
      float heightFromDistance = renderCamera.GetFrustumHeightFromDistance(renderCamera.farClipPlane);
      quadShape3D.Size = (Vector2) new Vector3(widthFromDistance + 0.01f, heightFromDistance + 0.01f, 1f);
      quadShape3D.Rotation = transform.rotation;
      quadShape3D.Center = transform.position + transform.forward * renderCamera.farClipPlane * 0.98f;
      Material gradientCameraBk = Singleton<MaterialPool>.Get.LinearGradientCameraBk;
      gradientCameraBk.SetColor("_FirstColor", backgroundSettings.FirstColor);
      gradientCameraBk.SetColor("_SecondColor", backgroundSettings.SecondColor);
      gradientCameraBk.SetFloat("_GradientOffset", backgroundSettings.GradientOffset);
      gradientCameraBk.SetFloat("_FarPlaneHeight", quadShape3D.Size.y);
      gradientCameraBk.SetPass(0);
      quadShape3D.RenderSolid();
    }

    public RTCameraBackground()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._bkSettings = new CameraBackgroundSettings();
      this._renderIgnoreCameras = new List<Camera>();
      this._cameraToBkSettings = new Dictionary<Camera, CameraBackgroundSettings>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
