// Decompiled with JetBrains decompiler
// Type: RTG.SpotLightGizmo3D
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
  public class SpotLightGizmo3D : GizmoBehaviour
  {
    private Light _targetLight;
    private Vector3 _pickedWorldSnapPoint;
    private bool _isSnapEnabled;
    private List<Vector3> _coneCirclePoints;
    private GizmoCap2D _dirSnapTick;
    private SceneRaycastFilter _raycastFilter;
    private SpotLightGizmo3D.AngleTick[] _angleTicks;
    private GizmoCap2D _rangeTick;
    private GizmoSglAxisOffsetDrag3D _dummyDragSession;
    private GizmoSglAxisOffsetDrag3D.WorkData _dummySessionWorkData;
    private GizmoSglAxisOffsetDrag3D _sglAxisDrag;
    private GizmoSglAxisOffsetDrag3D.WorkData _sglAxisDragWorkData;
    private Light3DSnapshot _preChangeSnapshot;
    private Light3DSnapshot _postChangeSnapshot;
    private SpotLightGizmo3DLookAndFeel _lookAndFeel;
    private SpotLightGizmo3DLookAndFeel _sharedLookAndFeel;
    private SpotLightGizmo3DSettings _settings;
    private SpotLightGizmo3DSettings _sharedSettings;
    private SpotLightGizmo3DHotkeys _hotkeys;
    private SpotLightGizmo3DHotkeys _sharedHotkeys;

    public SpotLightGizmo3DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel != null ? this._sharedLookAndFeel : this._lookAndFeel;
    }

    public SpotLightGizmo3DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set
      {
        this._sharedLookAndFeel = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public SpotLightGizmo3DSettings Settings
    {
      get => this._sharedSettings != null ? this._sharedSettings : this._settings;
    }

    public SpotLightGizmo3DSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public SpotLightGizmo3DHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public SpotLightGizmo3DHotkeys SharedHotkeys
    {
      get => this._sharedHotkeys;
      set => this._sharedHotkeys = value;
    }

    public Light TargetLight => this._targetLight;

    public bool IsSnapEnabled => this._isSnapEnabled || this.Hotkeys.EnableSnapping.IsActive();

    public void SetTargetLight(Light targetLight) => this._targetLight = targetLight;

    public void SetSnapEnabled(bool isEnabled) => this._isSnapEnabled = isEnabled;

    public bool OwnsHandle(int handleId)
    {
      foreach (SpotLightGizmo3D.AngleTick angleTick in this._angleTicks)
      {
        if (angleTick.Tick.HandleId == handleId)
          return true;
      }
      return handleId == this._dirSnapTick.HandleId || handleId == this._rangeTick.HandleId;
    }

    public override void OnAttached()
    {
      this._dirSnapTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.DirectionSnapCap);
      this._dirSnapTick.DragSession = (IGizmoDragSession) this._dummyDragSession;
      int index1 = 2;
      this._angleTicks[index1] = new SpotLightGizmo3D.AngleTick();
      this._angleTicks[index1].Tick = new GizmoCap2D(this.Gizmo, GizmoHandleId.SpotAngleCapBottom);
      this._angleTicks[index1].Tick.DragSession = (IGizmoDragSession) this._sglAxisDrag;
      int index2 = 0;
      this._angleTicks[index2] = new SpotLightGizmo3D.AngleTick();
      this._angleTicks[index2].Tick = new GizmoCap2D(this.Gizmo, GizmoHandleId.SpotAngleCapTop);
      this._angleTicks[index2].Tick.DragSession = (IGizmoDragSession) this._sglAxisDrag;
      int index3 = 3;
      this._angleTicks[index3] = new SpotLightGizmo3D.AngleTick();
      this._angleTicks[index3].Tick = new GizmoCap2D(this.Gizmo, GizmoHandleId.SpotAngleCapLeft);
      this._angleTicks[index3].Tick.DragSession = (IGizmoDragSession) this._sglAxisDrag;
      int index4 = 1;
      this._angleTicks[index4] = new SpotLightGizmo3D.AngleTick();
      this._angleTicks[index4].Tick = new GizmoCap2D(this.Gizmo, GizmoHandleId.SpotAngleCapRight);
      this._angleTicks[index4].Tick.DragSession = (IGizmoDragSession) this._sglAxisDrag;
      this._rangeTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.RangeTick);
      this._rangeTick.DragSession = (IGizmoDragSession) this._sglAxisDrag;
      this.SetupSharedLookAndFeel();
      this._coneCirclePoints = PrimitiveFactory.Generate3DCircleBorderPoints(Vector3.zero, 1f, Vector3.right, Vector3.up, 100);
      this._raycastFilter.AllowedObjectTypes.Add(GameObjectType.Mesh);
      this._raycastFilter.AllowedObjectTypes.Add(GameObjectType.Terrain);
    }

    public override void OnGizmoUpdateBegin()
    {
      if (!this.IsTargetReady())
        return;
      this.Gizmo.Transform.Position3D = this._targetLight.transform.position;
      this.UpdateTicks();
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      if (!this.OwnsHandle(handleId) || !this.IsTargetReady())
        return;
      if (handleId == this._dirSnapTick.HandleId)
      {
        this._dummySessionWorkData.Axis = Vector3.one;
        this._dummyDragSession.SetWorkData(this._dummySessionWorkData);
      }
      else
      {
        if (handleId == this._angleTicks[3].Tick.HandleId)
        {
          this._sglAxisDragWorkData.Axis = this._angleTicks[3].LightAxis;
          this._sglAxisDragWorkData.DragOrigin = this.CalcConeBase();
          this._sglAxisDragWorkData.SnapStep = this.Settings.RadiusSnapStep;
        }
        else if (handleId == this._angleTicks[1].Tick.HandleId)
        {
          this._sglAxisDragWorkData.Axis = this._angleTicks[1].LightAxis;
          this._sglAxisDragWorkData.DragOrigin = this.CalcConeBase();
          this._sglAxisDragWorkData.SnapStep = this.Settings.RadiusSnapStep;
        }
        else if (handleId == this._angleTicks[0].Tick.HandleId)
        {
          this._sglAxisDragWorkData.Axis = this._angleTicks[0].LightAxis;
          this._sglAxisDragWorkData.DragOrigin = this.CalcConeBase();
          this._sglAxisDragWorkData.SnapStep = this.Settings.RadiusSnapStep;
        }
        else if (handleId == this._angleTicks[2].Tick.HandleId)
        {
          this._sglAxisDragWorkData.Axis = this._angleTicks[2].LightAxis;
          this._sglAxisDragWorkData.DragOrigin = this.CalcConeBase();
          this._sglAxisDragWorkData.SnapStep = this.Settings.RadiusSnapStep;
        }
        else if (handleId == this._rangeTick.HandleId)
        {
          this._sglAxisDragWorkData.Axis = this._targetLight.transform.forward;
          this._sglAxisDragWorkData.DragOrigin = this.CalcConeBase();
          this._sglAxisDragWorkData.SnapStep = this.Settings.RangeSnapStep;
        }
        this._sglAxisDrag.SetWorkData(this._sglAxisDragWorkData);
      }
      this._preChangeSnapshot.Snapshot(this._targetLight);
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (!this.OwnsHandle(handleId) || !this.IsTargetReady())
        return;
      this._sglAxisDrag.IsSnapEnabled = this.IsSnapEnabled;
      if (handleId == this._dirSnapTick.HandleId)
        this.SnapDirection();
      else if (handleId >= GizmoHandleId.SpotAngleCapTop && handleId <= GizmoHandleId.SpotAngleCapRight)
      {
        this._sglAxisDrag.IsSnapEnabled = this.Hotkeys.EnableSnapping.IsActive();
        this._targetLight.spotAngle = this.CalcSpotAngleDegrees(this.CalcConeRadius() + this.Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(this.Gizmo.RelativeDragOffset, this._sglAxisDrag.Axis))) * 2f;
      }
      else if (handleId == this._rangeTick.HandleId)
      {
        this._sglAxisDrag.IsSnapEnabled = this.Hotkeys.EnableSnapping.IsActive();
        this._targetLight.range += this.Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(this.Gizmo.RelativeDragOffset, this._sglAxisDrag.Axis));
      }
      this.UpdateTicks();
    }

    public override void OnGizmoDragEnd(int handleId)
    {
      this._postChangeSnapshot.Snapshot(this._targetLight);
      new Light3DChangedAction(this._preChangeSnapshot, this._postChangeSnapshot).Execute();
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (!this.IsTargetReady())
        return;
      Vector3 position = this._targetLight.transform.position;
      Vector3 pos = this.CalcConeBase();
      float num = this.CalcConeRadius();
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
        this.UpdateTicks();
      GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
      get.ResetValuesToSensibleDefaults();
      get.SetColor(this.LookAndFeel.WireColor);
      get.SetPass(0);
      GL.PushMatrix();
      GL.MultMatrix(Matrix4x4.TRS(pos, this._targetLight.transform.rotation, new Vector3(num, num, 1f)));
      GLRenderer.DrawLines3D(this._coneCirclePoints);
      GL.PopMatrix();
      GLRenderer.DrawLine3D(position, pos + this._targetLight.transform.up * num);
      GLRenderer.DrawLine3D(position, pos + this._targetLight.transform.right * num);
      GLRenderer.DrawLine3D(position, pos - this._targetLight.transform.up * num);
      GLRenderer.DrawLine3D(position, pos - this._targetLight.transform.right * num);
      if (this.Gizmo.DragHandleId == this._dirSnapTick.HandleId)
      {
        get.SetColor(this.LookAndFeel.DirSnapSegmentColor);
        get.SetPass(0);
        GLRenderer.DrawLine3D(position, this._pickedWorldSnapPoint);
      }
      foreach (SpotLightGizmo3D.AngleTick angleTick in this._angleTicks)
        angleTick.Tick.Render(camera);
      this._rangeTick.Render(camera);
      this._dirSnapTick.Render(camera);
    }

    private float CalcConeRadius()
    {
      return Mathf.Tan((float) (Math.PI / 180.0 * (double) this._targetLight.spotAngle * 0.5)) * this._targetLight.range;
    }

    private Vector3 CalcConeBase()
    {
      return this._targetLight.transform.position + this._targetLight.transform.forward * this._targetLight.range;
    }

    private float CalcSpotAngleDegrees(float radius)
    {
      return Mathf.Atan2(radius, this._targetLight.range) * 57.29578f;
    }

    private void UpdateTicks()
    {
      Camera workCamera = this.Gizmo.GetWorkCamera();
      Plane plane = new Plane(workCamera.transform.forward, workCamera.transform.position);
      if (this.Gizmo.DragHandleId == this._dirSnapTick.HandleId)
      {
        if ((double) plane.GetDistanceToPoint(this._pickedWorldSnapPoint) > 0.0)
          this._dirSnapTick.SetVisible(true);
        else
          this._dirSnapTick.SetVisible(false);
        this._dirSnapTick.Position = (Vector2) this.Gizmo.GetWorkCamera().WorldToScreenPoint(this._pickedWorldSnapPoint);
      }
      else
      {
        Vector3 position = this._targetLight.transform.position;
        this._dirSnapTick.Position = (Vector2) this.Gizmo.GetWorkCamera().WorldToScreenPoint(position);
        if ((double) plane.GetDistanceToPoint(position) > 0.0)
          this._dirSnapTick.SetVisible(true);
        else
          this._dirSnapTick.SetVisible(false);
      }
      Vector3 vector3 = this.CalcConeBase();
      float num = this.CalcConeRadius();
      Vector3 right = this._targetLight.transform.right;
      Vector3 up = this._targetLight.transform.up;
      this._rangeTick.Position = (Vector2) workCamera.WorldToScreenPoint(vector3);
      if ((double) plane.GetDistanceToPoint(vector3) > 0.0)
        this._rangeTick.SetVisible(true);
      else
        this._rangeTick.SetVisible(false);
      int index1 = 2;
      this._angleTicks[index1].Position = vector3 - up * num;
      this._angleTicks[index1].LightAxis = -up;
      this._angleTicks[index1].Tick.Position = (Vector2) workCamera.WorldToScreenPoint(this._angleTicks[index1].Position);
      if ((double) plane.GetDistanceToPoint(this._angleTicks[index1].Position) > 0.0)
        this._angleTicks[index1].Tick.SetVisible(true);
      else
        this._angleTicks[index1].Tick.SetVisible(false);
      int index2 = 0;
      this._angleTicks[index2].Position = vector3 + up * num;
      this._angleTicks[index2].LightAxis = up;
      this._angleTicks[index2].Tick.Position = (Vector2) workCamera.WorldToScreenPoint(this._angleTicks[index2].Position);
      if ((double) plane.GetDistanceToPoint(this._angleTicks[index2].Position) > 0.0)
        this._angleTicks[index2].Tick.SetVisible(true);
      else
        this._angleTicks[index2].Tick.SetVisible(false);
      int index3 = 3;
      this._angleTicks[index3].Position = vector3 - right * num;
      this._angleTicks[index3].LightAxis = -right;
      this._angleTicks[index3].Tick.Position = (Vector2) workCamera.WorldToScreenPoint(this._angleTicks[index3].Position);
      if ((double) plane.GetDistanceToPoint(this._angleTicks[index3].Position) > 0.0)
        this._angleTicks[index3].Tick.SetVisible(true);
      else
        this._angleTicks[index3].Tick.SetVisible(false);
      int index4 = 1;
      this._angleTicks[index4].Position = vector3 + right * num;
      this._angleTicks[index4].LightAxis = right;
      this._angleTicks[index4].Tick.Position = (Vector2) workCamera.WorldToScreenPoint(this._angleTicks[index4].Position);
      if ((double) plane.GetDistanceToPoint(this._angleTicks[index4].Position) > 0.0)
        this._angleTicks[index4].Tick.SetVisible(true);
      else
        this._angleTicks[index4].Tick.SetVisible(false);
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel.ConnectDirSnapTickLookAndFeel(this._dirSnapTick);
      this.LookAndFeel.ConnectTickLookAndFeel(this._rangeTick);
      foreach (SpotLightGizmo3D.AngleTick angleTick in this._angleTicks)
        this.LookAndFeel.ConnectTickLookAndFeel(angleTick.Tick);
    }

    private void SnapDirection()
    {
      SceneRaycastHit sceneRaycastHit = MonoSingleton<RTScene>.Get.Raycast(MonoSingleton<RTInputDevice>.Get.Device.GetRay(this.Gizmo.GetWorkCamera()), SceneRaycastPrecision.BestFit, this._raycastFilter);
      if (sceneRaycastHit.WasAnObjectHit)
      {
        this._pickedWorldSnapPoint = sceneRaycastHit.ObjectHit.HitPoint;
      }
      else
      {
        if (!sceneRaycastHit.WasGridHit)
          return;
        this._pickedWorldSnapPoint = sceneRaycastHit.GridHit.HitPoint;
      }
      Vector3 vector3 = this._pickedWorldSnapPoint - this._targetLight.transform.position;
      if ((double) vector3.magnitude <= 9.9999997473787516E-05)
        return;
      this._targetLight.transform.Align(vector3.normalized, TransformAxis.PositiveZ);
    }

    private bool IsTargetReady()
    {
      return (UnityEngine.Object) this._targetLight != (UnityEngine.Object) null && this._targetLight.enabled && this._targetLight.gameObject.activeSelf && this._targetLight.type == LightType.Spot;
    }

    public SpotLightGizmo3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._coneCirclePoints = new List<Vector3>();
      this._raycastFilter = new SceneRaycastFilter();
      this._angleTicks = new SpotLightGizmo3D.AngleTick[4];
      this._dummyDragSession = new GizmoSglAxisOffsetDrag3D();
      this._sglAxisDrag = new GizmoSglAxisOffsetDrag3D();
      this._preChangeSnapshot = new Light3DSnapshot();
      this._postChangeSnapshot = new Light3DSnapshot();
      this._lookAndFeel = new SpotLightGizmo3DLookAndFeel();
      this._settings = new SpotLightGizmo3DSettings();
      this._hotkeys = new SpotLightGizmo3DHotkeys();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private enum AngleTickId
    {
      Top,
      Right,
      Bottom,
      Left,
    }

    private class AngleTick
    {
      public Vector3 Position;
      public Vector3 LightAxis;
      public GizmoCap2D Tick;

      public AngleTick()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
