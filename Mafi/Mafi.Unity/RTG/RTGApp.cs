// Decompiled with JetBrains decompiler
// Type: RTG.RTGApp
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi;
using Mafi.Unity;
using Mafi.Unity.Camera;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class RTGApp : MonoSingleton<RTGApp>, IRLDApplication
  {
    private UnityEngine.Camera _renderCamera;
    private RenderPipelineId _renderPipelineId;
    private static Option<CameraController> s_cameraController;

    public event RTGAppInitializedHandler Initialized;

    public RenderPipelineId RenderPipelineId => this._renderPipelineId;

    public UnityEngine.Camera RenderCamera => this._renderCamera;

    private void OnCanCameraUseScrollWheel(YesNoAnswer answer)
    {
      if (MonoSingleton<RTScene>.Get.IsAnyUIElementHovered())
        answer.No();
      else
        answer.Yes();
    }

    private void OnCanCameraProcessInput(YesNoAnswer answer)
    {
      if (MonoSingleton<RTGizmosEngine>.Get.DraggedGizmo != null)
        answer.No();
      else
        answer.Yes();
    }

    private void OnCanUndoRedo(UndoRedoOpType undoRedoOpType, YesNoAnswer answer)
    {
      if (MonoSingleton<RTGizmosEngine>.Get.DraggedGizmo == null)
        answer.Yes();
      else
        answer.No();
    }

    private void OnCanDoGizmoHoverUpdate(YesNoAnswer answer) => answer.Yes();

    private void OnViewportsCameraAdded(UnityEngine.Camera camera)
    {
      MonoSingleton<RTGizmosEngine>.Get.AddRenderCamera(camera);
    }

    private void OnViewportCameraRemoved(UnityEngine.Camera camera)
    {
      MonoSingleton<RTGizmosEngine>.Get.RemoveRenderCamera(camera);
    }

    private void Start()
    {
      this.DetectRenderPipeline();
      MonoSingleton<RTUndoRedo>.Get.CanUndoRedo += new CanUndoRedoHandler(this.OnCanUndoRedo);
      MonoSingleton<RTFocusCamera>.Get.CanProcessInput += new CameraCanProcessInputHandler(this.OnCanCameraProcessInput);
      MonoSingleton<RTFocusCamera>.Get.CanUseScrollWheel += new CameraCanUseScrollWheelHandler(this.OnCanCameraUseScrollWheel);
      Singleton<RTCameraViewports>.Get.CameraAdded += new RTCameraViewports.CameraAddedHandler(this.OnViewportsCameraAdded);
      Singleton<RTCameraViewports>.Get.CameraRemoved += new RTCameraViewports.CameraRemovedHandler(this.OnViewportCameraRemoved);
      MonoSingleton<RTScene>.Get.RegisterHoverableSceneEntityContainer((IHoverableSceneEntityContainer) MonoSingleton<RTGizmosEngine>.Get);
      MonoSingleton<RTSceneGrid>.Get.Initialize_SystemCall();
      MonoSingleton<RTGizmosEngine>.Get.CanDoHoverUpdate += new GizmoEngineCanDoHoverUpdateHandler(this.OnCanDoGizmoHoverUpdate);
      int renderPipelineId = (int) this._renderPipelineId;
      MonoSingleton<RTGizmosEngine>.Get.AddRenderCamera(MonoSingleton<RTFocusCamera>.Get.TargetCamera);
      RTMeshCompiler.CompileEntireScene();
      if (this._renderPipelineId != RenderPipelineId.Standard)
      {
        RenderPipelineManager.beginCameraRendering += new Action<ScriptableRenderContext, UnityEngine.Camera>(this.OnBeginCameraRendering);
        RenderPipelineManager.endCameraRendering += new Action<ScriptableRenderContext, UnityEngine.Camera>(this.OnEndCameraRendering);
      }
      if (this.Initialized == null)
        return;
      this.Initialized();
    }

    private void DetectRenderPipeline()
    {
      this._renderPipelineId = RenderPipelineId.Standard;
      if (!((UnityEngine.Object) GraphicsSettings.currentRenderPipeline != (UnityEngine.Object) null))
        return;
      if (GraphicsSettings.currentRenderPipeline.GetType().ToString().Contains("Universal"))
      {
        this._renderPipelineId = RenderPipelineId.URP;
      }
      else
      {
        Debug.LogError((object) "RLD: Unsupported render pipeline. Only Standard and URP pipelines are supported.");
        Debug.Break();
      }
    }

    private void Update()
    {
      if (RTGApp.s_cameraController.IsNone || RTGApp.s_cameraController.Value.IsInFreeLookMode)
        return;
      MonoSingleton<RTInputDevice>.Get.Update_SystemCall();
      MonoSingleton<RTFocusCamera>.Get.Update_SystemCall();
      MonoSingleton<RTGizmosEngine>.Get.Update_SystemCall();
    }

    private void OnRenderObject()
    {
      if (this._renderPipelineId == RenderPipelineId.Standard)
        this._renderCamera = UnityEngine.Camera.current;
      if (MonoSingleton<RTGizmosEngine>.Get.IsSceneGizmoCamera(this._renderCamera))
      {
        MonoSingleton<RTGizmosEngine>.Get.Render_SystemCall(this._renderCamera);
      }
      else
      {
        if ((UnityEngine.Object) MonoSingleton<RTCameraBackground>.Get != (UnityEngine.Object) null)
          MonoSingleton<RTCameraBackground>.Get.Render_SystemCall(this._renderCamera);
        MonoSingleton<RTSceneGrid>.Get.Render_SystemCall(this._renderCamera);
        MonoSingleton<RTGizmosEngine>.Get.Render_SystemCall(this._renderCamera);
      }
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, UnityEngine.Camera camera)
    {
      this._renderCamera = camera;
    }

    private void OnEndCameraRendering(ScriptableRenderContext context, UnityEngine.Camera camera)
    {
      int renderPipelineId = (int) this._renderPipelineId;
    }

    private void OnDisable()
    {
      if (!((UnityEngine.Object) GraphicsSettings.currentRenderPipeline != (UnityEngine.Object) null))
        return;
      RenderPipelineManager.beginCameraRendering -= new Action<ScriptableRenderContext, UnityEngine.Camera>(this.OnBeginCameraRendering);
      RenderPipelineManager.endCameraRendering -= new Action<ScriptableRenderContext, UnityEngine.Camera>(this.OnEndCameraRendering);
    }

    public static void Initialize(Transform parentTransform = null)
    {
      RTGApp.DestroyAppAndModules();
      Transform transform = RTGApp.CreateAppModuleObject<RTGApp>(parentTransform).transform;
      RTGApp.CreateAppModuleObject<RTGizmosEngine>(transform);
      RTGApp.CreateAppModuleObject<RTScene>(transform);
      RTGApp.CreateAppModuleObject<RTSceneGrid>(transform);
      RTGApp.CreateAppModuleObject<RTFocusCamera>(transform).SetTargetCamera(UnityEngine.Camera.main);
      RTGApp.CreateAppModuleObject<RTCameraBackground>(transform);
      RTGApp.CreateAppModuleObject<RTInputDevice>(transform);
      RTGApp.CreateAppModuleObject<RTUndoRedo>(transform);
    }

    public static void MafiInitialize(AssetsDb assetsDb, CameraController cameraController)
    {
      RTGApp.s_cameraController = (Option<CameraController>) cameraController;
      Singleton<ShaderPool>.Get.MafiInitialize(assetsDb);
      Singleton<TexturePool>.Get.MafiInitialize(assetsDb);
      RTGApp.Initialize();
      MonoSingleton<RTSceneGrid>.Get.Settings.IsVisible = false;
      MonoSingleton<RTUndoRedo>.Get.SetEnabled(false);
      MonoSingleton<RTGizmosEngine>.Get.MoveGizmoHotkeys.Enable2DMode.IsEnabled = false;
      MonoSingleton<RTGizmosEngine>.Get.MoveGizmoHotkeys.EnableSnapping.IsEnabled = false;
      MonoSingleton<RTGizmosEngine>.Get.MoveGizmoHotkeys.EnableVertexSnapping.IsEnabled = false;
    }

    public static void MafiTerminate()
    {
      RTGApp.s_cameraController = Option<CameraController>.None;
      RTGApp.DestroyAppAndModules();
    }

    private static DataType CreateAppModuleObject<DataType>(Transform parentTransform) where DataType : MonoBehaviour
    {
      string name = typeof (DataType).ToString();
      int num = name.IndexOf(".");
      if (num >= 0)
        name = name.Remove(0, num + 1);
      return new GameObject(name)
      {
        transform = {
          parent = parentTransform
        }
      }.AddComponent<DataType>();
    }

    private static void DestroyAppAndModules()
    {
      foreach (System.Type appModuleType in RTGApp.GetAppModuleTypes())
      {
        foreach (UnityEngine.Object @object in UnityEngine.Object.FindObjectsOfType(appModuleType))
        {
          MonoBehaviour monoBehaviour = @object as MonoBehaviour;
          if ((UnityEngine.Object) monoBehaviour != (UnityEngine.Object) null)
            UnityEngine.Object.DestroyImmediate((UnityEngine.Object) monoBehaviour.gameObject);
        }
      }
    }

    private static System.Type[] GetAppModuleTypes()
    {
      return new System.Type[8]
      {
        typeof (RTGApp),
        typeof (RTFocusCamera),
        typeof (RTCameraBackground),
        typeof (RTScene),
        typeof (RTSceneGrid),
        typeof (RTInputDevice),
        typeof (RTUndoRedo),
        typeof (RTGizmosEngine)
      };
    }

    public RTGApp()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
