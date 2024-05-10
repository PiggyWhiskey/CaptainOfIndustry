// Decompiled with JetBrains decompiler
// Type: RTG.Gizmo
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
  [Serializable]
  public class Gizmo
  {
    private bool _isEnabled;
    private GizmoHandleCollection _handles;
    private GizmoBehaviourCollection _behaviours;
    private GizmoHoverInfo _hoverInfo;
    private GizmoDragInfo _dragInfo;
    private IGizmoHandle _hoveredHandle;
    private Priority _genericHoverPriority;
    private Priority _hoverPriority3D;
    private Priority _hoverPriority2D;
    private IGizmoDragSession _activeDragSession;
    private GizmoTransform _transform;
    [NonSerialized]
    private MoveGizmo _moveGizmo;
    [NonSerialized]
    private RotationGizmo _rotationGizmo;
    [NonSerialized]
    private ScaleGizmo _scaleGizmo;
    [NonSerialized]
    private UniversalGizmo _universalGizmo;
    [NonSerialized]
    private ObjectTransformGizmo _objectTransformGizmo;
    [NonSerialized]
    private SceneGizmo _sceneGizmo;

    public event GizmoPostEnabledHandler PostEnabled;

    public event GizmoPostDisabledHandler PostDisabled;

    public event GizmoPreUpdateBeginHandler PreUpdateBegin;

    public event GizmoPostUpdateEndHandler PostUpdateEnd;

    public event GizmoPreHoverEnterHandler PreHoverEnter;

    public event GizmoPostHoverEnterHandler PostHoverEnter;

    public event GizmoPreHoverExitHandler PreHoverExit;

    public event GizmoPostHoverExitHandler PostHoverExit;

    public event GizmoPreDragBeginHandler PreDragBegin;

    public event GizmoPostDragBeginHandler PostDragBegin;

    public event GizmoPreDragEndHandler PreDragEnd;

    public event GizmoPostDragEndHandler PostDragEnd;

    public event GizmoPreDragUpdateHandler PreDragUpdate;

    public event GizmoPostDragUpdateHandler PostDragUpdate;

    public event GizmoPreHandlePickedHandler PreHandlePicked;

    public event GizmoPostHandlePickedHandler PostHandlePicked;

    public event GizmoPreDragBeginAttemptHandler PreDragBeginAttempt;

    public event GizmoPostDragBeginAttemptHandler PostDragBeginAttempt;

    public event GizmoOffsetDragAxisModifyHandler OffsetDragAxisModify;

    public static int InputDeviceDragButtonIndex => 0;

    public int NumHandles => this._handles.Count;

    public Camera FocusCamera
    {
      get
      {
        return this.SceneGizmo != null ? this.SceneGizmo.SceneGizmoCamera.Camera : MonoSingleton<RTFocusCamera>.Get.TargetCamera;
      }
    }

    public bool IsEnabled => this._isEnabled;

    public Priority GenericHoverPriority => this._genericHoverPriority;

    public Priority HoverPriority3D => this._hoverPriority3D;

    public Priority HoverPriority2D => this._hoverPriority2D;

    public GizmoTransform Transform => this._transform;

    public GizmoHoverInfo HoverInfo => this._hoverInfo;

    public bool IsHovered => this._hoverInfo.IsHovered;

    public int HoverHandleId => this._hoverInfo.HandleId;

    public GizmoDimension HoverHandleDimension => this._hoverInfo.HandleDimension;

    public Vector3 HoverPoint => this._hoverInfo.HoverPoint;

    public GizmoDragInfo DragInfo => this._dragInfo;

    public bool IsDragged => this._dragInfo.IsDragged;

    public GizmoDragChannel ActiveDragChannel => this._dragInfo.DragChannel;

    public int DragHandleId => this._dragInfo.HandleId;

    public Vector3 DragBeginPoint => this._dragInfo.DragBeginPoint;

    public GizmoDimension DragHandleDimension => this._dragInfo.HandleDimension;

    public Vector3 TotalDragOffset => this._dragInfo.TotalOffset;

    public Quaternion TotalDragRotation => this._dragInfo.TotalRotation;

    public Vector3 TotalDragScale => this._dragInfo.TotalScale;

    public Vector3 RelativeDragOffset => this._dragInfo.RelativeOffset;

    public Quaternion RelativeDragRotation => this._dragInfo.RelativeRotation;

    public Vector3 RelativeDragScale => this._dragInfo.RelativeScale;

    public MoveGizmo MoveGizmo => this._moveGizmo;

    public RotationGizmo RotationGizmo => this._rotationGizmo;

    public ScaleGizmo ScaleGizmo => this._scaleGizmo;

    public UniversalGizmo UniversalGizmo => this._universalGizmo;

    public ObjectTransformGizmo ObjectTransformGizmo => this._objectTransformGizmo;

    public SceneGizmo SceneGizmo => this._sceneGizmo;

    public Gizmo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isEnabled = true;
      this._behaviours = new GizmoBehaviourCollection();
      this._genericHoverPriority = new Priority();
      this._hoverPriority3D = new Priority();
      this._hoverPriority2D = new Priority();
      this._transform = new GizmoTransform();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._handles = new GizmoHandleCollection(this);
      this._hoverInfo.Reset();
      this._dragInfo.Reset();
    }

    public Camera GetWorkCamera()
    {
      return MonoSingleton<RTGizmosEngine>.Get.PipelineStage != GizmosEnginePipelineStage.Render ? this.FocusCamera : MonoSingleton<RTGizmosEngine>.Get.RenderStageCamera;
    }

    public GizmoHandle CreateHandle(int id)
    {
      if (this._handles.Contains(id))
        return (GizmoHandle) null;
      GizmoHandle handle = new GizmoHandle(this, id);
      this._handles.Add((IGizmoHandle) handle);
      return handle;
    }

    public void SetEnabled(bool enabled)
    {
      if (enabled == this._isEnabled)
        return;
      if (!enabled)
      {
        this.EndDragSession();
        this._hoverInfo.Reset();
        this._hoveredHandle = (IGizmoHandle) null;
        this._isEnabled = false;
        foreach (IGizmoBehaviour behaviour in this._behaviours)
        {
          if (behaviour.IsEnabled)
            behaviour.OnGizmoDisabled();
        }
        if (this.PostDisabled == null)
          return;
        this.PostDisabled(this);
      }
      else
      {
        this._isEnabled = true;
        foreach (IGizmoBehaviour behaviour in this._behaviours)
        {
          if (behaviour.IsEnabled)
            behaviour.OnGizmoEnabled();
        }
        if (this.PostEnabled == null)
          return;
        this.PostEnabled(this);
      }
    }

    public BehaviourType AddBehaviour<BehaviourType>() where BehaviourType : class, IGizmoBehaviour, new()
    {
      BehaviourType behaviour = new BehaviourType();
      this.AddBehaviour((IGizmoBehaviour) behaviour);
      return behaviour;
    }

    public bool AddBehaviour(IGizmoBehaviour behaviour)
    {
      if (behaviour == null || behaviour.Gizmo != null)
        return false;
      behaviour.Init_SystemCall(new GizmoBehaviorInitParams()
      {
        Gizmo = this
      });
      if (!this._behaviours.Add(behaviour))
        return false;
      System.Type type = behaviour.GetType();
      if (type == typeof (MoveGizmo))
        this._moveGizmo = behaviour as MoveGizmo;
      else if (type == typeof (RotationGizmo))
        this._rotationGizmo = behaviour as RotationGizmo;
      else if (type == typeof (ScaleGizmo))
        this._scaleGizmo = behaviour as ScaleGizmo;
      else if (type == typeof (UniversalGizmo))
        this._universalGizmo = behaviour as UniversalGizmo;
      else if (type == typeof (SceneGizmo))
        this._sceneGizmo = behaviour as SceneGizmo;
      else if (type == typeof (ObjectTransformGizmo))
        this._objectTransformGizmo = behaviour as ObjectTransformGizmo;
      behaviour.OnAttached();
      behaviour.OnEnabled();
      return true;
    }

    public bool RemoveBehaviour(IGizmoBehaviour behaviour)
    {
      if (behaviour == null)
        return false;
      if (behaviour == this._moveGizmo)
        this._moveGizmo = (MoveGizmo) null;
      else if (behaviour == this._rotationGizmo)
        this._rotationGizmo = (RotationGizmo) null;
      else if (behaviour == this._scaleGizmo)
        this._scaleGizmo = (ScaleGizmo) null;
      else if (behaviour == this._universalGizmo)
        this._universalGizmo = (UniversalGizmo) null;
      else if (behaviour == this._sceneGizmo)
        this._sceneGizmo = (SceneGizmo) null;
      else if (behaviour == this._objectTransformGizmo)
        this._objectTransformGizmo = (ObjectTransformGizmo) null;
      return this._behaviours.Remove(behaviour);
    }

    public List<BehaviourType> GetBehavioursOfType<BehaviourType>() where BehaviourType : class, IGizmoBehaviour
    {
      return this._behaviours.GetBehavioursOfType<BehaviourType>();
    }

    public BehaviourType GetFirstBehaviourOfType<BehaviourType>() where BehaviourType : class, IGizmoBehaviour
    {
      return this._behaviours.GetFirstBehaviourOfType<BehaviourType>();
    }

    public IGizmoBehaviour GetFirstBehaviourOfType(System.Type behaviourType)
    {
      return this._behaviours.GetFirstBehaviourOfType(behaviourType);
    }

    public List<GizmoHandleHoverData> GetAllHandlesHoverData(Ray hoverRay)
    {
      return this._handles.GetAllHandlesHoverData(hoverRay);
    }

    public IGizmoHandle GetHandleById_SystemCall(int handleId)
    {
      return this._handles.GetHandleById(handleId);
    }

    public void OnGUI_SystemCall()
    {
      if (!this.IsEnabled)
        return;
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled)
          behaviour.OnGUI();
      }
    }

    public void OnUpdateBegin_SystemCall()
    {
      if (!this.IsEnabled)
        return;
      if (this.PreUpdateBegin != null)
        this.PreUpdateBegin(this);
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled)
          behaviour.OnGizmoUpdateBegin();
      }
    }

    public void OnUpdateEnd_SystemCall()
    {
      if (!this.IsEnabled)
        return;
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled)
          behaviour.OnGizmoUpdateEnd();
      }
      if (this.PostUpdateEnd == null)
        return;
      this.PostUpdateEnd(this);
    }

    public void UpdateHandleHoverInfo_SystemCall(GizmoHoverInfo hoverInfo)
    {
      if (!this.IsEnabled || this.IsDragged)
        return;
      bool isHovered = this._hoverInfo.IsHovered;
      int handleId = this._hoverInfo.HandleId;
      this._hoverInfo.Reset();
      this._hoveredHandle = (IGizmoHandle) null;
      if (hoverInfo.IsHovered && hoverInfo.HandleId != GizmoHandleId.None)
      {
        this._hoverInfo.IsHovered = true;
        this._hoverInfo.HandleId = hoverInfo.HandleId;
        this._hoverInfo.HoverPoint = hoverInfo.HoverPoint;
        this._hoveredHandle = this._handles.GetHandleById(hoverInfo.HandleId);
        this._hoverInfo.HandleDimension = hoverInfo.HandleDimension;
      }
      if (isHovered && !this._hoverInfo.IsHovered)
      {
        if (this.PreHoverExit != null)
          this.PreHoverExit(this, handleId);
        foreach (IGizmoBehaviour behaviour in this._behaviours)
        {
          if (behaviour.IsEnabled)
            behaviour.OnGizmoHoverExit(handleId);
        }
        if (this.PostHoverExit == null)
          return;
        this.PostHoverExit(this, handleId);
      }
      else if (!isHovered && this._hoverInfo.IsHovered)
      {
        if (this.PreHoverEnter != null)
          this.PreHoverEnter(this, this._hoverInfo.HandleId);
        foreach (IGizmoBehaviour behaviour in this._behaviours)
        {
          if (behaviour.IsEnabled)
            behaviour.OnGizmoHoverEnter(this._hoverInfo.HandleId);
        }
        if (this.PostHoverEnter == null)
          return;
        this.PostHoverEnter(this, this._hoverInfo.HandleId);
      }
      else
      {
        if (!isHovered || !this._hoverInfo.IsHovered)
          return;
        if (handleId != this._hoverInfo.HandleId)
        {
          if (this.PreHoverExit != null)
            this.PreHoverExit(this, handleId);
          foreach (IGizmoBehaviour behaviour in this._behaviours)
          {
            if (behaviour.IsEnabled)
              behaviour.OnGizmoHoverExit(handleId);
          }
          if (this.PostHoverExit != null)
            this.PostHoverExit(this, handleId);
        }
        if (this.PreHoverEnter != null)
          this.PreHoverEnter(this, this._hoverInfo.HandleId);
        foreach (IGizmoBehaviour behaviour in this._behaviours)
        {
          if (behaviour.IsEnabled)
            behaviour.OnGizmoHoverEnter(this._hoverInfo.HandleId);
        }
        if (this.PostHoverEnter == null)
          return;
        this.PostHoverEnter(this, this._hoverInfo.HandleId);
      }
    }

    public void Render_SystemCall(Camera camera, Plane[] worldFrustumPlanes)
    {
      if (!this.IsEnabled || this.NumHandles == 0)
        return;
      bool flag = MonoSingleton<RTGizmosEngine>.Get.IsSceneGizmoCamera(camera);
      if (this.SceneGizmo == null & flag || this.SceneGizmo != null && !flag)
        return;
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled)
          behaviour.OnGizmoRender(camera);
      }
    }

    public void HandleInputDeviceEvents_SystemCall()
    {
      if (!this.IsEnabled)
        return;
      IInputDevice device = MonoSingleton<RTInputDevice>.Get.Device;
      if (device.WasButtonPressedInCurrentFrame(Gizmo.InputDeviceDragButtonIndex))
        this.OnInputDevicePickButtonDown();
      else if (device.WasButtonReleasedInCurrentFrame(Gizmo.InputDeviceDragButtonIndex))
        this.OnInputDevicePickButtonUp();
      if (!device.WasMoved())
        return;
      this.OnInputDeviceMoved();
    }

    private void OnInputDevicePickButtonDown()
    {
      if (this._hoveredHandle == null)
        return;
      if (this.PreHandlePicked != null)
        this.PreHandlePicked(this, this._hoveredHandle.Id);
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled)
          behaviour.OnGizmoHandlePicked(this._hoveredHandle.Id);
      }
      if (this.PostHandlePicked != null)
        this.PostHandlePicked(this, this._hoveredHandle.Id);
      this.TryActivateDragSession();
    }

    private void OnInputDevicePickButtonUp() => this.EndDragSession();

    private void EndDragSession()
    {
      if (this._activeDragSession != null)
      {
        this._activeDragSession.End();
        this._dragInfo.IsDragged = false;
        if (this.PreDragEnd != null)
          this.PreDragEnd(this, this._dragInfo.HandleId);
        foreach (IGizmoBehaviour behaviour in this._behaviours)
        {
          if (behaviour.IsEnabled)
            behaviour.OnGizmoDragEnd(this._dragInfo.HandleId);
        }
        int handleId = this._dragInfo.HandleId;
        this._dragInfo.Reset();
        if (this.PostDragEnd != null)
          this.PostDragEnd(this, handleId);
      }
      this._activeDragSession = (IGizmoDragSession) null;
    }

    private void OnInputDeviceMoved()
    {
      if (!MonoSingleton<RTInputDevice>.Get.Device.IsButtonPressed(Gizmo.InputDeviceDragButtonIndex) || this._activeDragSession == null || !this._activeDragSession.IsActive || !this._activeDragSession.Update())
        return;
      this._dragInfo.TotalOffset = this._activeDragSession.TotalDragOffset;
      this._dragInfo.RelativeOffset = this._activeDragSession.RelativeDragOffset;
      this._dragInfo.TotalRotation = this._activeDragSession.TotalDragRotation;
      this._dragInfo.TotalScale = this._activeDragSession.TotalDragScale;
      this._dragInfo.RelativeRotation = this._activeDragSession.RelativeDragRotation;
      this._dragInfo.RelativeScale = this._activeDragSession.RelativeDragScale;
      if ((double) this._activeDragSession.TotalDragOffset.magnitude != 0.0 && this.OffsetDragAxisModify != null)
      {
        Vector3 relativeDragOffset = this._activeDragSession.RelativeDragOffset;
        Vector3 vector3_1 = this.OffsetDragAxisModify(this, relativeDragOffset.normalized, this._hoveredHandle.Id);
        ref GizmoDragInfo local = ref this._dragInfo;
        Vector3 vector3_2 = vector3_1;
        relativeDragOffset = this._activeDragSession.RelativeDragOffset;
        double magnitude = (double) relativeDragOffset.magnitude;
        Vector3 vector3_3 = vector3_2 * (float) magnitude;
        local.RelativeOffset = vector3_3;
        this._dragInfo.TotalOffset = this._activeDragSession.TotalDragOffset - this._activeDragSession.RelativeDragOffset + this._dragInfo.RelativeOffset;
      }
      if (this.PreDragUpdate != null)
        this.PreDragUpdate(this, this._dragInfo.HandleId);
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled)
          behaviour.OnGizmoDragUpdate(this._dragInfo.HandleId);
      }
      if (this.PostDragUpdate == null)
        return;
      this.PostDragUpdate(this, this._dragInfo.HandleId);
    }

    private void TryActivateDragSession()
    {
      if (this._hoveredHandle == null || this._hoveredHandle.DragSession == null)
        return;
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled && !behaviour.OnGizmoCanBeginDrag(this._hoveredHandle.Id))
          return;
      }
      if (this.PreDragBeginAttempt != null)
        this.PreDragBeginAttempt(this, this._hoveredHandle.Id);
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled)
          behaviour.OnGizmoAttemptHandleDragBegin(this._hoveredHandle.Id);
      }
      if (this.PostDragBeginAttempt != null)
        this.PostDragBeginAttempt(this, this._hoveredHandle.Id);
      if (!this._hoveredHandle.DragSession.Begin())
        return;
      this._activeDragSession = this._hoveredHandle.DragSession;
      this._dragInfo.IsDragged = true;
      this._dragInfo.DragChannel = this._activeDragSession.DragChannel;
      this._dragInfo.HandleDimension = this._hoverInfo.HandleDimension;
      this._dragInfo.HandleId = this._hoverInfo.HandleId;
      this._dragInfo.DragBeginPoint = this._hoverInfo.HoverPoint;
      if (this.PreDragBegin != null)
        this.PreDragBegin(this, this._dragInfo.HandleId);
      foreach (IGizmoBehaviour behaviour in this._behaviours)
      {
        if (behaviour.IsEnabled)
          behaviour.OnGizmoDragBegin(this._dragInfo.HandleId);
      }
      if (this.PostDragBegin == null)
        return;
      this.PostDragBegin(this, this._dragInfo.HandleId);
    }
  }
}
