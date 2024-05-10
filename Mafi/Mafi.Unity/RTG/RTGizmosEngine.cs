// Decompiled with JetBrains decompiler
// Type: RTG.RTGizmosEngine
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class RTGizmosEngine : MonoSingleton<RTGizmosEngine>, IHoverableSceneEntityContainer
  {
    [SerializeField]
    private EditorToolbar _mainToolbar;
    [SerializeField]
    private GizmoEngineSettings _settings;
    private GizmosEnginePipelineStage _pipelineStage;
    private Gizmo _draggedGizmo;
    private bool _justReleasedDrag;
    private Gizmo _hoveredGizmo;
    private GizmoHoverInfo _gizmoHoverInfo;
    private List<Gizmo> _gizmos;
    private List<ISceneGizmo> _sceneGizmos;
    private List<RTSceneGizmoCamera> _sceneGizmoCameras;
    private List<Camera> _renderCameras;
    [SerializeField]
    private SceneGizmoLookAndFeel _sceneGizmoLookAndFeel;
    [SerializeField]
    private MoveGizmoSettings2D _moveGizmoSettings2D;
    [SerializeField]
    private MoveGizmoSettings3D _moveGizmoSettings3D;
    [SerializeField]
    private MoveGizmoLookAndFeel2D _moveGizmoLookAndFeel2D;
    [SerializeField]
    private MoveGizmoLookAndFeel3D _moveGizmoLookAndFeel3D;
    [SerializeField]
    private MoveGizmoHotkeys _moveGizmoHotkeys;
    [SerializeField]
    private ObjectTransformGizmoSettings _objectMoveGizmoSettings;
    [SerializeField]
    private RotationGizmoSettings3D _rotationGizmoSettings3D;
    [SerializeField]
    private RotationGizmoLookAndFeel3D _rotationGizmoLookAndFeel3D;
    [SerializeField]
    private RotationGizmoHotkeys _rotationGizmoHotkeys;
    [SerializeField]
    private ObjectTransformGizmoSettings _objectRotationGizmoSettings;
    [SerializeField]
    private ScaleGizmoSettings3D _scaleGizmoSettings3D;
    [SerializeField]
    private ScaleGizmoLookAndFeel3D _scaleGizmoLookAndFeel3D;
    [SerializeField]
    private ScaleGizmoHotkeys _scaleGizmoHotkeys;
    [SerializeField]
    private ObjectTransformGizmoSettings _objectScaleGizmoSettings;
    [SerializeField]
    private UniversalGizmoConfig _universalGizmoConfig;
    [SerializeField]
    private UniversalGizmoSettings2D _universalGizmoSettings2D;
    [SerializeField]
    private UniversalGizmoSettings3D _universalGizmoSettings3D;
    [SerializeField]
    private UniversalGizmoLookAndFeel2D _universalGizmoLookAndFeel2D;
    [SerializeField]
    private UniversalGizmoLookAndFeel3D _universalGizmoLookAndFeel3D;
    [SerializeField]
    private UniversalGizmoHotkeys _universalGizmoHotkeys;
    [SerializeField]
    private ObjectTransformGizmoSettings _objectUniversalGizmoSettings;

    public event GizmoEngineCanDoHoverUpdateHandler CanDoHoverUpdate;

    public GizmoEngineSettings Settings => this._settings;

    public GizmosEnginePipelineStage PipelineStage => this._pipelineStage;

    public Camera RenderStageCamera => MonoSingleton<RTGApp>.Get.RenderCamera;

    public bool HasHoveredSceneEntity => this.IsAnyGizmoHovered;

    public bool IsAnyGizmoHovered => this._hoveredGizmo != null;

    public Gizmo HoveredGizmo => this._hoveredGizmo;

    public Gizmo DraggedGizmo => this._draggedGizmo;

    public bool JustReleasedDrag => this._justReleasedDrag;

    public int NumRenderCameras => this._renderCameras.Count;

    public SceneGizmoLookAndFeel SceneGizmoLookAndFeel => this._sceneGizmoLookAndFeel;

    public MoveGizmoSettings2D MoveGizmoSettings2D => this._moveGizmoSettings2D;

    public MoveGizmoSettings3D MoveGizmoSettings3D => this._moveGizmoSettings3D;

    public MoveGizmoLookAndFeel2D MoveGizmoLookAndFeel2D => this._moveGizmoLookAndFeel2D;

    public MoveGizmoLookAndFeel3D MoveGizmoLookAndFeel3D => this._moveGizmoLookAndFeel3D;

    public MoveGizmoHotkeys MoveGizmoHotkeys => this._moveGizmoHotkeys;

    public ObjectTransformGizmoSettings ObjectMoveGizmoSettings => this._objectMoveGizmoSettings;

    public RotationGizmoSettings3D RotationGizmoSettings3D => this._rotationGizmoSettings3D;

    public RotationGizmoLookAndFeel3D RotationGizmoLookAndFeel3D
    {
      get => this._rotationGizmoLookAndFeel3D;
    }

    public RotationGizmoHotkeys RotationGizmoHotkeys => this._rotationGizmoHotkeys;

    public ObjectTransformGizmoSettings ObjectRotationGizmoSettings
    {
      get => this._objectRotationGizmoSettings;
    }

    public ScaleGizmoSettings3D ScaleGizmoSettings3D => this._scaleGizmoSettings3D;

    public ScaleGizmoLookAndFeel3D ScaleGizmoLookAndFeel3D => this._scaleGizmoLookAndFeel3D;

    public ScaleGizmoHotkeys ScaleGizmoHotkeys => this._scaleGizmoHotkeys;

    public ObjectTransformGizmoSettings ObjectScaleGizmoSettings => this._objectScaleGizmoSettings;

    public UniversalGizmoSettings2D UniversalGizmoSettings2D => this._universalGizmoSettings2D;

    public UniversalGizmoSettings3D UniversalGizmoSettings3D => this._universalGizmoSettings3D;

    public UniversalGizmoLookAndFeel2D UniversalGizmoLookAndFeel2D
    {
      get => this._universalGizmoLookAndFeel2D;
    }

    public UniversalGizmoLookAndFeel3D UniversalGizmoLookAndFeel3D
    {
      get => this._universalGizmoLookAndFeel3D;
    }

    public UniversalGizmoHotkeys UniversalGizmoHotkeys => this._universalGizmoHotkeys;

    public ObjectTransformGizmoSettings ObjectUniversalGizmoSettings
    {
      get => this._objectUniversalGizmoSettings;
    }

    public void AddRenderCamera(Camera camera)
    {
      if (this.IsRenderCamera(camera) || this.IsSceneGizmoCamera(camera))
        return;
      this._renderCameras.Add(camera);
    }

    public bool IsRenderCamera(Camera camera) => this._renderCameras.Contains(camera);

    public void RemoveRenderCamera(Camera camera) => this._renderCameras.Remove(camera);

    public RTSceneGizmoCamera CreateSceneGizmoCamera(
      Camera sceneCamera,
      ISceneGizmoCamViewportUpdater viewportUpdater)
    {
      RTSceneGizmoCamera sceneGizmoCamera = new GameObject(typeof (RTSceneGizmoCamera).ToString())
      {
        transform = {
          parent = MonoSingleton<RTGizmosEngine>.Get.transform
        }
      }.AddComponent<RTSceneGizmoCamera>();
      sceneGizmoCamera.ViewportUpdater = viewportUpdater;
      sceneGizmoCamera.SceneCamera = sceneCamera;
      this._sceneGizmoCameras.Add(sceneGizmoCamera);
      return sceneGizmoCamera;
    }

    public bool IsSceneGizmoCamera(Camera camera)
    {
      return this._sceneGizmoCameras.FindAll((Predicate<RTSceneGizmoCamera>) (item => (UnityEngine.Object) item.Camera == (UnityEngine.Object) camera)).Count != 0;
    }

    public ISceneGizmo GetSceneGizmoByCamera(Camera sceneCamera)
    {
      foreach (ISceneGizmo sceneGizmo in this._sceneGizmos)
      {
        if ((UnityEngine.Object) sceneGizmo.SceneCamera == (UnityEngine.Object) sceneCamera)
          return sceneGizmo;
      }
      return (ISceneGizmo) null;
    }

    public Gizmo CreateGizmo()
    {
      Gizmo gizmo = new Gizmo();
      this.RegisterGizmo(gizmo);
      return gizmo;
    }

    public void RemoveGizmo(Gizmo gizmo) => this.UnregisterGizmo(gizmo);

    public SceneGizmo CreateSceneGizmo(Camera sceneCamera)
    {
      if (this.GetSceneGizmoByCamera(sceneCamera) != null)
        return (SceneGizmo) null;
      Gizmo gizmo = new Gizmo();
      this.RegisterGizmo(gizmo);
      SceneGizmo sceneGizmo = gizmo.AddBehaviour<SceneGizmo>();
      sceneGizmo.SceneGizmoCamera.SceneCamera = sceneCamera;
      sceneGizmo.SharedLookAndFeel = this.SceneGizmoLookAndFeel;
      this._sceneGizmos.Add((ISceneGizmo) sceneGizmo);
      return sceneGizmo;
    }

    public MoveGizmo CreateMoveGizmo()
    {
      Gizmo gizmo = this.CreateGizmo();
      MoveGizmo behaviour = new MoveGizmo();
      gizmo.AddBehaviour((IGizmoBehaviour) behaviour);
      behaviour.SharedHotkeys = this._moveGizmoHotkeys;
      behaviour.SharedLookAndFeel2D = this._moveGizmoLookAndFeel2D;
      behaviour.SharedLookAndFeel3D = this._moveGizmoLookAndFeel3D;
      behaviour.SharedSettings2D = this._moveGizmoSettings2D;
      behaviour.SharedSettings3D = this._moveGizmoSettings3D;
      return behaviour;
    }

    public ObjectTransformGizmo CreateObjectMoveGizmo()
    {
      ObjectTransformGizmo objectMoveGizmo = this.CreateMoveGizmo().Gizmo.AddBehaviour<ObjectTransformGizmo>();
      objectMoveGizmo.SetTransformChannelFlags(ObjectTransformGizmo.Channels.Position);
      objectMoveGizmo.SharedSettings = this._objectMoveGizmoSettings;
      return objectMoveGizmo;
    }

    public RotationGizmo CreateRotationGizmo()
    {
      Gizmo gizmo = this.CreateGizmo();
      RotationGizmo behaviour = new RotationGizmo();
      gizmo.AddBehaviour((IGizmoBehaviour) behaviour);
      behaviour.SharedHotkeys = this._rotationGizmoHotkeys;
      behaviour.SharedLookAndFeel3D = this._rotationGizmoLookAndFeel3D;
      behaviour.SharedSettings3D = this._rotationGizmoSettings3D;
      return behaviour;
    }

    public ObjectTransformGizmo CreateObjectRotationGizmo()
    {
      ObjectTransformGizmo objectRotationGizmo = this.CreateRotationGizmo().Gizmo.AddBehaviour<ObjectTransformGizmo>();
      objectRotationGizmo.SetTransformChannelFlags(ObjectTransformGizmo.Channels.Rotation);
      objectRotationGizmo.SharedSettings = this._objectRotationGizmoSettings;
      return objectRotationGizmo;
    }

    public ScaleGizmo CreateScaleGizmo()
    {
      Gizmo gizmo = this.CreateGizmo();
      ScaleGizmo behaviour = new ScaleGizmo();
      gizmo.AddBehaviour((IGizmoBehaviour) behaviour);
      behaviour.SharedHotkeys = this._scaleGizmoHotkeys;
      behaviour.SharedLookAndFeel3D = this._scaleGizmoLookAndFeel3D;
      behaviour.SharedSettings3D = this._scaleGizmoSettings3D;
      return behaviour;
    }

    public ObjectTransformGizmo CreateObjectScaleGizmo()
    {
      ObjectTransformGizmo objectScaleGizmo = this.CreateScaleGizmo().Gizmo.AddBehaviour<ObjectTransformGizmo>();
      objectScaleGizmo.SetTransformChannelFlags(ObjectTransformGizmo.Channels.Scale);
      objectScaleGizmo.SetTransformSpace(GizmoSpace.Local);
      objectScaleGizmo.MakeTransformSpacePermanent();
      objectScaleGizmo.SharedSettings = this._objectScaleGizmoSettings;
      return objectScaleGizmo;
    }

    public UniversalGizmo CreateUniversalGizmo()
    {
      Gizmo gizmo = this.CreateGizmo();
      UniversalGizmo behaviour = new UniversalGizmo();
      gizmo.AddBehaviour((IGizmoBehaviour) behaviour);
      behaviour.SharedHotkeys = this._universalGizmoHotkeys;
      behaviour.SharedLookAndFeel2D = this._universalGizmoLookAndFeel2D;
      behaviour.SharedLookAndFeel3D = this._universalGizmoLookAndFeel3D;
      behaviour.SharedSettings2D = this._universalGizmoSettings2D;
      behaviour.SharedSettings3D = this._universalGizmoSettings3D;
      return behaviour;
    }

    public ObjectTransformGizmo CreateObjectUniversalGizmo()
    {
      ObjectTransformGizmo objectUniversalGizmo = this.CreateUniversalGizmo().Gizmo.AddBehaviour<ObjectTransformGizmo>();
      objectUniversalGizmo.SetTransformChannelFlags(ObjectTransformGizmo.Channels.All);
      objectUniversalGizmo.SharedSettings = this._objectUniversalGizmoSettings;
      return objectUniversalGizmo;
    }

    public void Update_SystemCall()
    {
      foreach (RTSceneGizmoCamera sceneGizmoCamera in this._sceneGizmoCameras)
        sceneGizmoCamera.Update_SystemCall();
      this._pipelineStage = GizmosEnginePipelineStage.Update;
      IInputDevice device = MonoSingleton<RTInputDevice>.Get.Device;
      bool flag1 = device.HasPointer();
      Vector3 positionYaxisUp = device.GetPositionYAxisUp();
      bool flag2 = MonoSingleton<RTScene>.Get.IsAnyUIElementHovered();
      bool flag3 = this._draggedGizmo == null && !flag2 && Cursor.lockState == CursorLockMode.None;
      this._justReleasedDrag = false;
      if (flag3)
      {
        YesNoAnswer answer = new YesNoAnswer();
        if (this.CanDoHoverUpdate != null)
          this.CanDoHoverUpdate(answer);
        if (answer.HasNo)
          flag3 = false;
      }
      this._hoveredGizmo = (Gizmo) null;
      this._gizmoHoverInfo.Reset();
      bool flag4 = flag1 && MonoSingleton<RTFocusCamera>.Get.IsViewportHoveredByDevice();
      bool flag5 = this.IsRenderCamera(MonoSingleton<RTFocusCamera>.Get.TargetCamera);
      List<GizmoHandleHoverData> hoverDataCollection = new List<GizmoHandleHoverData>(10);
      foreach (Gizmo gizmo in this._gizmos)
      {
        gizmo.OnUpdateBegin_SystemCall();
        if (((!flag3 ? 0 : (gizmo.IsEnabled ? 1 : 0)) & (flag4 ? 1 : 0) & (flag1 ? 1 : 0) & (flag5 ? 1 : 0)) != 0)
        {
          GizmoHandleHoverData gizmoHandleHoverData = this.GetGizmoHandleHoverData(gizmo);
          if (gizmoHandleHoverData != null)
            hoverDataCollection.Add(gizmoHandleHoverData);
        }
      }
      if (flag3 && hoverDataCollection.Count != 0)
      {
        this.SortHandleHoverDataCollection(hoverDataCollection, positionYaxisUp);
        GizmoHandleHoverData gizmoHandleHoverData = hoverDataCollection[0];
        this._hoveredGizmo = gizmoHandleHoverData.Gizmo;
        this._gizmoHoverInfo.HandleId = gizmoHandleHoverData.HandleId;
        this._gizmoHoverInfo.HandleDimension = gizmoHandleHoverData.HandleDimension;
        this._gizmoHoverInfo.HoverPoint = gizmoHandleHoverData.HoverPoint;
        this._gizmoHoverInfo.IsHovered = true;
      }
      foreach (Gizmo gizmo in this._gizmos)
      {
        this._gizmoHoverInfo.IsHovered = gizmo == this._hoveredGizmo;
        gizmo.UpdateHandleHoverInfo_SystemCall(this._gizmoHoverInfo);
        gizmo.HandleInputDeviceEvents_SystemCall();
        gizmo.OnUpdateEnd_SystemCall();
      }
      this._pipelineStage = GizmosEnginePipelineStage.PostUpdate;
    }

    public GizmoHandleHoverData GetGizmoHandleHoverData(Gizmo gizmo)
    {
      Camera focusCamera = gizmo.FocusCamera;
      Ray ray = MonoSingleton<RTInputDevice>.Get.Device.GetRay(focusCamera);
      List<GizmoHandleHoverData> handlesHoverData = gizmo.GetAllHandlesHoverData(ray);
      Vector3 screenRayOrigin = focusCamera.WorldToScreenPoint(ray.origin);
      handlesHoverData.Sort((Comparison<GizmoHandleHoverData>) ((h0, h1) =>
      {
        IGizmoHandle handleByIdSystemCall1 = gizmo.GetHandleById_SystemCall(h0.HandleId);
        IGizmoHandle handleByIdSystemCall2 = gizmo.GetHandleById_SystemCall(h1.HandleId);
        if (h0.HandleDimension == h1.HandleDimension)
          return h0.HandleDimension == GizmoDimension.Dim2D ? (handleByIdSystemCall1.HoverPriority2D == handleByIdSystemCall2.HoverPriority2D ? (screenRayOrigin - h0.HoverPoint).sqrMagnitude.CompareTo((screenRayOrigin - h1.HoverPoint).sqrMagnitude) : handleByIdSystemCall1.HoverPriority2D.CompareTo(handleByIdSystemCall2.HoverPriority2D)) : (handleByIdSystemCall1.HoverPriority3D == handleByIdSystemCall2.HoverPriority3D ? h0.HoverEnter3D.CompareTo(h1.HoverEnter3D) : handleByIdSystemCall1.HoverPriority3D.CompareTo(handleByIdSystemCall2.HoverPriority3D));
        if (!(handleByIdSystemCall1.GenericHoverPriority == handleByIdSystemCall2.GenericHoverPriority))
          return handleByIdSystemCall1.GenericHoverPriority.CompareTo(handleByIdSystemCall2.GenericHoverPriority);
        return h0.HandleDimension == GizmoDimension.Dim2D ? -1 : 1;
      }));
      return handlesHoverData.Count == 0 ? (GizmoHandleHoverData) null : handlesHoverData[0];
    }

    public void Render_SystemCall(Camera renderCamera)
    {
      this._pipelineStage = GizmosEnginePipelineStage.Render;
      if (!this.IsSceneGizmoCamera(renderCamera) && !this.IsRenderCamera(renderCamera))
      {
        this._pipelineStage = GizmosEnginePipelineStage.PostRender;
      }
      else
      {
        if (this.Settings.EnableGizmoSorting)
        {
          Vector3 camPos = this.RenderStageCamera.transform.position;
          List<Gizmo> gizmoList = new List<Gizmo>((IEnumerable<Gizmo>) this._gizmos);
          gizmoList.Sort((Comparison<Gizmo>) ((g0, g1) =>
          {
            float sqrMagnitude = (g0.Transform.Position3D - camPos).sqrMagnitude;
            return (g1.Transform.Position3D - camPos).sqrMagnitude.CompareTo(sqrMagnitude);
          }));
          Plane[] cameraWorldPlanes = CameraViewVolume.GetCameraWorldPlanes(renderCamera);
          foreach (Gizmo gizmo in gizmoList)
            gizmo.Render_SystemCall(renderCamera, cameraWorldPlanes);
        }
        else
        {
          Plane[] cameraWorldPlanes = CameraViewVolume.GetCameraWorldPlanes(renderCamera);
          foreach (Gizmo gizmo in this._gizmos)
            gizmo.Render_SystemCall(renderCamera, cameraWorldPlanes);
        }
        this._pipelineStage = GizmosEnginePipelineStage.PostRender;
      }
    }

    private void SortHandleHoverDataCollection(
      List<GizmoHandleHoverData> hoverDataCollection,
      Vector3 inputDevicePos)
    {
      if (hoverDataCollection.Count == 0)
        return;
      Ray hoverRay = hoverDataCollection[0].HoverRay;
      hoverDataCollection.Sort((Comparison<GizmoHandleHoverData>) ((h0, h1) => h0.HandleDimension == h1.HandleDimension ? (h0.HandleDimension == GizmoDimension.Dim2D ? (h0.Gizmo.HoverPriority2D == h1.Gizmo.HoverPriority2D ? (inputDevicePos - h0.HoverPoint).sqrMagnitude.CompareTo((inputDevicePos - h1.HoverPoint).sqrMagnitude) : h0.Gizmo.HoverPriority2D.CompareTo(h1.Gizmo.HoverPriority2D)) : (h0.Gizmo.HoverPriority3D == h1.Gizmo.HoverPriority3D ? h0.HoverEnter3D.CompareTo(h1.HoverEnter3D) : h0.Gizmo.HoverPriority3D.CompareTo(h1.Gizmo.HoverPriority3D))) : (h0.Gizmo.GenericHoverPriority == h1.Gizmo.GenericHoverPriority ? (h0.Gizmo.Transform.Position3D - hoverRay.origin).sqrMagnitude.CompareTo((h1.Gizmo.Transform.Position3D - hoverRay.origin).sqrMagnitude) : h0.Gizmo.GenericHoverPriority.CompareTo(h1.Gizmo.GenericHoverPriority))));
    }

    private void RegisterGizmo(Gizmo gizmo)
    {
      this._gizmos.Add(gizmo);
      gizmo.PreDragBegin += new GizmoPreDragBeginHandler(this.OnGizmoDragBegin);
      gizmo.PreDragEnd += new GizmoPreDragEndHandler(this.OnGizmoDragEnd);
    }

    private void UnregisterGizmo(Gizmo gizmo)
    {
      if (!this._gizmos.Remove(gizmo))
        return;
      gizmo.PreDragBegin -= new GizmoPreDragBeginHandler(this.OnGizmoDragBegin);
      gizmo.PreDragEnd -= new GizmoPreDragEndHandler(this.OnGizmoDragEnd);
    }

    private void OnGUI()
    {
      this._pipelineStage = GizmosEnginePipelineStage.GUI;
      foreach (Gizmo gizmo in this._gizmos)
        gizmo.OnGUI_SystemCall();
      this._pipelineStage = GizmosEnginePipelineStage.PostGUI;
    }

    private void OnGizmoDragBegin(Gizmo gizmo, int handleId) => this._draggedGizmo = gizmo;

    private void OnGizmoDragEnd(Gizmo gizmo, int handleId)
    {
      this._draggedGizmo = (Gizmo) null;
      this._justReleasedDrag = true;
    }

    public RTGizmosEngine()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._mainToolbar = new EditorToolbar(new EditorToolbarTab[6]
      {
        new EditorToolbarTab("General", "General gizmo engine settings."),
        new EditorToolbarTab("Scene gizmo", "Scene gizmo specific settings."),
        new EditorToolbarTab("Move gizmo", "Allows you to change move gizmo settings."),
        new EditorToolbarTab("Rotation gizmo", "Allows you to change rotation settings."),
        new EditorToolbarTab("Scale gizmo", "Allows you to change scale gizmo settings."),
        new EditorToolbarTab("Universal gizmo", "Allows you to change universal gizmo settings.")
      }, 6, Color.green);
      this._settings = new GizmoEngineSettings();
      this._gizmos = new List<Gizmo>();
      this._sceneGizmos = new List<ISceneGizmo>();
      this._sceneGizmoCameras = new List<RTSceneGizmoCamera>();
      this._renderCameras = new List<Camera>();
      this._sceneGizmoLookAndFeel = new SceneGizmoLookAndFeel();
      MoveGizmoSettings2D moveGizmoSettings2D = new MoveGizmoSettings2D();
      moveGizmoSettings2D.IsExpanded = false;
      this._moveGizmoSettings2D = moveGizmoSettings2D;
      MoveGizmoSettings3D moveGizmoSettings3D = new MoveGizmoSettings3D();
      moveGizmoSettings3D.IsExpanded = false;
      this._moveGizmoSettings3D = moveGizmoSettings3D;
      MoveGizmoLookAndFeel2D gizmoLookAndFeel2D1 = new MoveGizmoLookAndFeel2D();
      gizmoLookAndFeel2D1.IsExpanded = false;
      this._moveGizmoLookAndFeel2D = gizmoLookAndFeel2D1;
      MoveGizmoLookAndFeel3D gizmoLookAndFeel3D1 = new MoveGizmoLookAndFeel3D();
      gizmoLookAndFeel3D1.IsExpanded = false;
      this._moveGizmoLookAndFeel3D = gizmoLookAndFeel3D1;
      MoveGizmoHotkeys moveGizmoHotkeys = new MoveGizmoHotkeys();
      moveGizmoHotkeys.IsExpanded = false;
      this._moveGizmoHotkeys = moveGizmoHotkeys;
      ObjectTransformGizmoSettings transformGizmoSettings1 = new ObjectTransformGizmoSettings();
      transformGizmoSettings1.IsExpanded = false;
      this._objectMoveGizmoSettings = transformGizmoSettings1;
      RotationGizmoSettings3D rotationGizmoSettings3D = new RotationGizmoSettings3D();
      rotationGizmoSettings3D.IsExpanded = false;
      this._rotationGizmoSettings3D = rotationGizmoSettings3D;
      RotationGizmoLookAndFeel3D gizmoLookAndFeel3D2 = new RotationGizmoLookAndFeel3D();
      gizmoLookAndFeel3D2.IsExpanded = false;
      this._rotationGizmoLookAndFeel3D = gizmoLookAndFeel3D2;
      RotationGizmoHotkeys rotationGizmoHotkeys = new RotationGizmoHotkeys();
      rotationGizmoHotkeys.IsExpanded = false;
      this._rotationGizmoHotkeys = rotationGizmoHotkeys;
      ObjectTransformGizmoSettings transformGizmoSettings2 = new ObjectTransformGizmoSettings();
      transformGizmoSettings2.IsExpanded = false;
      this._objectRotationGizmoSettings = transformGizmoSettings2;
      ScaleGizmoSettings3D scaleGizmoSettings3D = new ScaleGizmoSettings3D();
      scaleGizmoSettings3D.IsExpanded = false;
      this._scaleGizmoSettings3D = scaleGizmoSettings3D;
      ScaleGizmoLookAndFeel3D gizmoLookAndFeel3D3 = new ScaleGizmoLookAndFeel3D();
      gizmoLookAndFeel3D3.IsExpanded = false;
      this._scaleGizmoLookAndFeel3D = gizmoLookAndFeel3D3;
      ScaleGizmoHotkeys scaleGizmoHotkeys = new ScaleGizmoHotkeys();
      scaleGizmoHotkeys.IsExpanded = false;
      this._scaleGizmoHotkeys = scaleGizmoHotkeys;
      ObjectTransformGizmoSettings transformGizmoSettings3 = new ObjectTransformGizmoSettings();
      transformGizmoSettings3.IsExpanded = false;
      this._objectScaleGizmoSettings = transformGizmoSettings3;
      this._universalGizmoConfig = new UniversalGizmoConfig();
      UniversalGizmoSettings2D universalGizmoSettings2D = new UniversalGizmoSettings2D();
      universalGizmoSettings2D.IsExpanded = false;
      this._universalGizmoSettings2D = universalGizmoSettings2D;
      UniversalGizmoSettings3D universalGizmoSettings3D = new UniversalGizmoSettings3D();
      universalGizmoSettings3D.IsExpanded = false;
      this._universalGizmoSettings3D = universalGizmoSettings3D;
      UniversalGizmoLookAndFeel2D gizmoLookAndFeel2D2 = new UniversalGizmoLookAndFeel2D();
      gizmoLookAndFeel2D2.IsExpanded = false;
      this._universalGizmoLookAndFeel2D = gizmoLookAndFeel2D2;
      UniversalGizmoLookAndFeel3D gizmoLookAndFeel3D4 = new UniversalGizmoLookAndFeel3D();
      gizmoLookAndFeel3D4.IsExpanded = false;
      this._universalGizmoLookAndFeel3D = gizmoLookAndFeel3D4;
      UniversalGizmoHotkeys universalGizmoHotkeys = new UniversalGizmoHotkeys();
      universalGizmoHotkeys.IsExpanded = false;
      this._universalGizmoHotkeys = universalGizmoHotkeys;
      ObjectTransformGizmoSettings transformGizmoSettings4 = new ObjectTransformGizmoSettings();
      transformGizmoSettings4.IsExpanded = false;
      this._objectUniversalGizmoSettings = transformGizmoSettings4;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
