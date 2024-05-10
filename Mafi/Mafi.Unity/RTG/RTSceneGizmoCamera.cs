// Decompiled with JetBrains decompiler
// Type: RTG.RTSceneGizmoCamera
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class RTSceneGizmoCamera : MonoBehaviour
  {
    private Camera _camera;
    private Transform _transform;
    private Vector3 _lookAtPoint;
    private float _fieldOfView;
    private float _orthoSize;
    private float _offsetFromFocusPt;
    private Camera _sceneCamera;
    private ISceneGizmoCamViewportUpdater _viewportUpdater;

    public Camera Camera => this._camera;

    public Camera SceneCamera
    {
      get => this._sceneCamera;
      set
      {
        if (!((Object) value != (Object) null) || !((Object) this._camera != (Object) null))
          return;
        this._sceneCamera = value;
        this._camera.depth = this._sceneCamera.depth + 1f;
      }
    }

    public ISceneGizmoCamViewportUpdater ViewportUpdater
    {
      get => this._viewportUpdater;
      set
      {
        if (value == null)
          return;
        this._viewportUpdater = value;
      }
    }

    public Vector3 WorldPosition
    {
      get => this._transform.position;
      set => this._transform.position = value;
    }

    public Quaternion WorldRotation
    {
      get => this._transform.rotation;
      set => this._transform.rotation = value;
    }

    public Vector3 Right => this._transform.right;

    public Vector3 Up => this._transform.up;

    public Vector3 Look => this._transform.forward;

    public Vector3 LookAtPoint => this._lookAtPoint;

    public void Update_SystemCall()
    {
      this.WorldRotation = this._sceneCamera.transform.rotation;
      this.WorldPosition = this._lookAtPoint - this.Look * this._offsetFromFocusPt;
      this.Camera.orthographic = this._sceneCamera.orthographic;
      this.Camera.fieldOfView = this._sceneCamera.fieldOfView;
      if (this._viewportUpdater == null)
        return;
      this._viewportUpdater.Update(this);
    }

    private void Awake()
    {
      this._camera = this.gameObject.AddComponent<Camera>();
      this._transform = this._camera.transform;
    }

    private void Start()
    {
      this._camera.cullingMask = 0;
      this._camera.clearFlags = CameraClearFlags.Depth;
      this._camera.renderingPath = RenderingPath.Forward;
      this._camera.fieldOfView = this._fieldOfView;
      this._camera.orthographicSize = this._orthoSize;
      this._camera.allowHDR = false;
      if ((Object) MonoSingleton<RTCameraBackground>.Get != (Object) null)
        MonoSingleton<RTCameraBackground>.Get.AddRenderIgnoreCamera(this._camera);
      MonoSingleton<RTSceneGrid>.Get.AddRenderIgnoreCamera(this._camera);
    }

    public RTSceneGizmoCamera()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._lookAtPoint = Vector3.zero;
      this._fieldOfView = 60f;
      this._orthoSize = 5f;
      this._offsetFromFocusPt = 5f;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
