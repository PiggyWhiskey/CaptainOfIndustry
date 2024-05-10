// Decompiled with JetBrains decompiler
// Type: RTG.PointLightGizmo3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class PointLightGizmo3D : GizmoBehaviour
  {
    private Light _targetLight;
    private bool _isSnapEnabled;
    private GizmoCap2D _rightTick;
    private GizmoCap2D _topTick;
    private GizmoCap2D _backTick;
    private GizmoCap2D _leftTick;
    private GizmoCap2D _bottomTick;
    private GizmoCap2D _frontTick;
    private PointLightGizmo3D.ExtentTick[] _extentTicks;
    private GizmoPlaneSlider3D _axialCircleXY;
    private GizmoPlaneSlider3D _axialCircleYZ;
    private GizmoPlaneSlider3D _axialCircleZX;
    private PolygonShape2D _sphereBorderPoly;
    private Light3DSnapshot _preChangeSnapshot;
    private Light3DSnapshot _postChangeSnapshot;
    private GizmoSglAxisOffsetDrag3D.WorkData _offsetDragWorkData;
    private GizmoSglAxisOffsetDrag3D _offsetDrag;
    private PointLightGizmo3DLookAndFeel _lookAndFeel;
    private PointLightGizmo3DLookAndFeel _sharedLookAndFeel;
    private PointLightGizmo3DSettings _settings;
    private PointLightGizmo3DSettings _sharedSettings;
    private PointLightGizmo3DHotkeys _hotkeys;
    private PointLightGizmo3DHotkeys _sharedHotkeys;

    public PointLightGizmo3DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel != null ? this._sharedLookAndFeel : this._lookAndFeel;
    }

    public PointLightGizmo3DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set
      {
        this._sharedLookAndFeel = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public PointLightGizmo3DSettings Settings
    {
      get => this._sharedSettings != null ? this._sharedSettings : this._settings;
    }

    public PointLightGizmo3DSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public PointLightGizmo3DHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public PointLightGizmo3DHotkeys SharedHotkeys
    {
      get => this._sharedHotkeys;
      set => this._sharedHotkeys = value;
    }

    public Light TargetLight => this._targetLight;

    public bool IsSnapEnabled => this._isSnapEnabled || this.Hotkeys.EnableSnapping.IsActive();

    public void SetTargetLight(Light targetLight) => this._targetLight = targetLight;

    public override void OnGizmoEnabled() => this.OnGizmoUpdateBegin();

    public bool OwnsHandle(int handleId)
    {
      return handleId == this._leftTick.HandleId || handleId == this._rightTick.HandleId || handleId == this._topTick.HandleId || handleId == this._bottomTick.HandleId || handleId == this._frontTick.HandleId || handleId == this._backTick.HandleId || handleId == this._axialCircleXY.HandleId || handleId == this._axialCircleYZ.HandleId || handleId == this._axialCircleZX.HandleId;
    }

    public void SetSnapEnabled(bool isEnabled) => this._isSnapEnabled = isEnabled;

    public override void OnAttached()
    {
      this._leftTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.NXCap);
      this._leftTick.DragSession = (IGizmoDragSession) this._offsetDrag;
      this._rightTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.PXCap);
      this._rightTick.DragSession = (IGizmoDragSession) this._offsetDrag;
      this._topTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.PYCap);
      this._topTick.DragSession = (IGizmoDragSession) this._offsetDrag;
      this._bottomTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.NYCap);
      this._bottomTick.DragSession = (IGizmoDragSession) this._offsetDrag;
      this._backTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.NZCap);
      this._backTick.DragSession = (IGizmoDragSession) this._offsetDrag;
      this._frontTick = new GizmoCap2D(this.Gizmo, GizmoHandleId.PZCap);
      this._frontTick.DragSession = (IGizmoDragSession) this._offsetDrag;
      this._extentTicks[2] = new PointLightGizmo3D.ExtentTick();
      this._extentTicks[2].Tick = this._leftTick;
      this._extentTicks[3] = new PointLightGizmo3D.ExtentTick();
      this._extentTicks[3].Tick = this._rightTick;
      this._extentTicks[5] = new PointLightGizmo3D.ExtentTick();
      this._extentTicks[5].Tick = this._topTick;
      this._extentTicks[4] = new PointLightGizmo3D.ExtentTick();
      this._extentTicks[4].Tick = this._bottomTick;
      this._extentTicks[0] = new PointLightGizmo3D.ExtentTick();
      this._extentTicks[0].Tick = this._frontTick;
      this._extentTicks[1] = new PointLightGizmo3D.ExtentTick();
      this._extentTicks[1].Tick = this._backTick;
      this._axialCircleXY = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.XYDblSlider);
      this._axialCircleXY.SetVisible(false);
      this._axialCircleXY.SetBorderHoverable(false);
      this._axialCircleXY.LookAndFeel.UseZoomFactor = false;
      this._axialCircleXY.LookAndFeel.PlaneType = GizmoPlane3DType.Circle;
      this._axialCircleYZ = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.YZDblSlider);
      this._axialCircleYZ.SetVisible(false);
      this._axialCircleYZ.SetBorderHoverable(false);
      this._axialCircleYZ.LookAndFeel.UseZoomFactor = false;
      this._axialCircleYZ.LookAndFeel.PlaneType = GizmoPlane3DType.Circle;
      this._axialCircleZX = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.ZXDblSlider);
      this._axialCircleZX.SetVisible(false);
      this._axialCircleZX.SetBorderHoverable(false);
      this._axialCircleZX.LookAndFeel.UseZoomFactor = false;
      this._axialCircleZX.LookAndFeel.PlaneType = GizmoPlane3DType.Circle;
      this.SetupSharedLookAndFeel();
    }

    public override void OnGizmoUpdateBegin()
    {
      if (!this.IsTargetReady())
        return;
      this.Gizmo.Transform.Position3D = this._targetLight.transform.position;
      this.UpdateHandles();
      this.UpdateHoverPriorities(this.Gizmo.GetWorkCamera());
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      if (!this.IsTargetReady())
        return;
      this._preChangeSnapshot.Snapshot(this._targetLight);
      if (!this.OwnsHandle(handleId))
        return;
      this._offsetDragWorkData.DragOrigin = this._targetLight.transform.position;
      this._offsetDragWorkData.SnapStep = this.Settings.RadiusSnapStep;
      if (handleId == this._leftTick.HandleId)
        this._offsetDragWorkData.Axis = -Vector3.right;
      else if (handleId == this._rightTick.HandleId)
        this._offsetDragWorkData.Axis = Vector3.right;
      else if (handleId == this._topTick.HandleId)
        this._offsetDragWorkData.Axis = Vector3.up;
      else if (handleId == this._bottomTick.HandleId)
        this._offsetDragWorkData.Axis = -Vector3.up;
      else if (handleId == this._frontTick.HandleId)
        this._offsetDragWorkData.Axis = -Vector3.forward;
      else if (handleId == this._backTick.HandleId)
        this._offsetDragWorkData.Axis = Vector3.forward;
      this._offsetDrag.SetWorkData(this._offsetDragWorkData);
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (!this.IsTargetReady() || !this.OwnsHandle(handleId))
        return;
      this._offsetDrag.IsSnapEnabled = this.IsSnapEnabled;
      this._targetLight.range = Mathf.Max(0.0f, this._targetLight.range + this.Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(this.Gizmo.RelativeDragOffset, this._offsetDrag.Axis)));
      this.UpdateHandles();
    }

    public override void OnGizmoDragEnd(int handleId)
    {
      if (!this.OwnsHandle(handleId))
        return;
      this._postChangeSnapshot.Snapshot(this._targetLight);
      new Light3DChangedAction(this._preChangeSnapshot, this._postChangeSnapshot).Execute();
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (!this.IsTargetReady())
        return;
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
        this.UpdateHandles();
      this.UpdateTickColors(camera);
      Plane plane = new Plane(camera.transform.forward, camera.transform.position + camera.transform.forward * camera.nearClipPlane);
      if (!new Sphere(this._targetLight.transform.position, this._targetLight.range).ContainsPoint(camera.transform.position) && (double) plane.GetDistanceToPoint(this._targetLight.transform.position) > 0.0)
      {
        GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
        get.ResetValuesToSensibleDefaults();
        get.SetColor(this.LookAndFeel.SphereBorderColor);
        get.SetPass(0);
        this._sphereBorderPoly.MakeSphereBorder(this._targetLight.transform.position, this._targetLight.range, 100, camera);
        this._sphereBorderPoly.RenderBorder(camera);
      }
      this._axialCircleXY.Render(camera);
      this._axialCircleYZ.Render(camera);
      this._axialCircleZX.Render(camera);
      this._leftTick.Render(camera);
      this._rightTick.Render(camera);
      this._topTick.Render(camera);
      this._bottomTick.Render(camera);
      this._frontTick.Render(camera);
      this._backTick.Render(camera);
    }

    private Vector3 CalcScalePivot(int handleId)
    {
      if (this.OwnsHandle(handleId))
      {
        Vector3 position = this._targetLight.transform.position;
        float range = this._targetLight.range;
        if (handleId == this._leftTick.HandleId)
          return position + Vector3.right * range;
        if (handleId == this._rightTick.HandleId)
          return position - Vector3.right * range;
        if (handleId == this._topTick.HandleId)
          return position - Vector3.up * range;
        if (handleId == this._bottomTick.HandleId)
          return position + Vector3.up * range;
        if (handleId == this._frontTick.HandleId)
          return position + Vector3.forward * range;
        if (handleId == this._backTick.HandleId)
          return position - Vector3.forward * range;
      }
      return Vector3.zero;
    }

    private void UpdateHandles()
    {
      Camera workCamera = this.Gizmo.GetWorkCamera();
      Vector3 position1 = this._targetLight.transform.position;
      float range = this._targetLight.range;
      Vector3 position2 = position1 - Vector3.right * range;
      this._leftTick.Position = (Vector2) workCamera.WorldToScreenPoint(position2);
      this._extentTicks[2].Position = position2;
      this._extentTicks[2].Normal = -Vector3.right;
      Vector3 position3 = position1 + Vector3.right * range;
      this._rightTick.Position = (Vector2) workCamera.WorldToScreenPoint(position3);
      this._extentTicks[3].Position = position3;
      this._extentTicks[3].Normal = Vector3.right;
      Vector3 position4 = position1 + Vector3.up * range;
      this._topTick.Position = (Vector2) workCamera.WorldToScreenPoint(position4);
      this._extentTicks[5].Position = position4;
      this._extentTicks[5].Normal = Vector3.up;
      Vector3 position5 = position1 - Vector3.up * range;
      this._bottomTick.Position = (Vector2) workCamera.WorldToScreenPoint(position5);
      this._extentTicks[4].Position = position5;
      this._extentTicks[4].Normal = -Vector3.up;
      Vector3 position6 = position1 - Vector3.forward * range;
      this._frontTick.Position = (Vector2) workCamera.WorldToScreenPoint(position6);
      this._extentTicks[0].Position = position6;
      this._extentTicks[0].Normal = -Vector3.forward;
      Vector3 position7 = position1 + Vector3.forward * range;
      this._backTick.Position = (Vector2) workCamera.WorldToScreenPoint(position7);
      this._extentTicks[1].Position = position7;
      this._extentTicks[1].Normal = Vector3.forward;
      this._axialCircleXY.Position = position1;
      this._axialCircleYZ.Position = position1;
      this._axialCircleZX.Position = position1;
      this._axialCircleXY.Rotation = Quaternion.identity;
      this._axialCircleYZ.Rotation = Quaternion.Euler(0.0f, 90f, 0.0f);
      this._axialCircleZX.Rotation = Quaternion.Euler(90f, 0.0f, 0.0f);
      this._axialCircleXY.LookAndFeel.BorderColor = this.LookAndFeel.WireColor;
      this._axialCircleYZ.LookAndFeel.BorderColor = this.LookAndFeel.WireColor;
      this._axialCircleZX.LookAndFeel.BorderColor = this.LookAndFeel.WireColor;
      this._axialCircleXY.LookAndFeel.CircleRadius = range;
      this._axialCircleYZ.LookAndFeel.CircleRadius = range;
      this._axialCircleZX.LookAndFeel.CircleRadius = range;
      this._axialCircleXY.LookAndFeel.BorderCircleCullAlphaScale = this.LookAndFeel.AxialCircleCullAlphaScale;
      this._axialCircleYZ.LookAndFeel.BorderCircleCullAlphaScale = this.LookAndFeel.AxialCircleCullAlphaScale;
      this._axialCircleZX.LookAndFeel.BorderCircleCullAlphaScale = this.LookAndFeel.AxialCircleCullAlphaScale;
      this._axialCircleXY.Refresh();
      this._axialCircleYZ.Refresh();
      this._axialCircleZX.Refresh();
    }

    private void UpdateTickColors(Camera camera)
    {
      Plane plane = new Plane(camera.transform.forward, camera.transform.position);
      foreach (PointLightGizmo3D.ExtentTick extentTick in this._extentTicks)
      {
        GizmoCap2D tick = extentTick.Tick;
        if (this.Gizmo.HoverHandleId != tick.HandleId && !camera.IsPointFacingCamera(extentTick.Position, extentTick.Normal))
        {
          tick.OverrideFillColor.IsActive = true;
          tick.OverrideBorderColor.IsActive = true;
          Color color = tick.SharedLookAndFeel.Color;
          tick.OverrideFillColor.Color = color.KeepAllButAlpha(this.LookAndFeel.TickCullAlphaScale * color.a);
          Color borderColor = tick.SharedLookAndFeel.BorderColor;
          tick.OverrideBorderColor.Color = borderColor.KeepAllButAlpha(this.LookAndFeel.TickCullAlphaScale * borderColor.a);
        }
        else
        {
          tick.OverrideFillColor.IsActive = false;
          tick.OverrideBorderColor.IsActive = false;
        }
        if ((double) plane.GetDistanceToPoint(extentTick.Position) > 0.0)
          tick.SetVisible(true);
        else
          tick.SetVisible(false);
      }
    }

    private void UpdateHoverPriorities(Camera camera)
    {
      int num1 = 0;
      PointLightGizmo3D.ExtentTick extentTick1 = this._extentTicks[2];
      PointLightGizmo3D.ExtentTick extentTick2 = this._extentTicks[3];
      extentTick1.Tick.HoverPriority2D.Value = num1;
      extentTick2.Tick.HoverPriority2D.Value = num1;
      if (camera.IsPointFacingCamera(extentTick1.Position, extentTick1.Normal))
        extentTick1.Tick.HoverPriority2D.MakeHigherThan(extentTick2.Tick.HoverPriority2D);
      else
        extentTick2.Tick.HoverPriority2D.MakeHigherThan(extentTick1.Tick.HoverPriority2D);
      int num2 = num1 + 2;
      PointLightGizmo3D.ExtentTick extentTick3 = this._extentTicks[5];
      PointLightGizmo3D.ExtentTick extentTick4 = this._extentTicks[4];
      extentTick3.Tick.HoverPriority2D.Value = num2;
      extentTick4.Tick.HoverPriority2D.Value = num2;
      if (camera.IsPointFacingCamera(extentTick3.Position, extentTick3.Normal))
        extentTick3.Tick.HoverPriority2D.MakeHigherThan(extentTick4.Tick.HoverPriority2D);
      else
        extentTick4.Tick.HoverPriority2D.MakeHigherThan(extentTick3.Tick.HoverPriority2D);
      int num3 = num2 + 2;
      PointLightGizmo3D.ExtentTick extentTick5 = this._extentTicks[0];
      PointLightGizmo3D.ExtentTick extentTick6 = this._extentTicks[1];
      extentTick5.Tick.HoverPriority2D.Value = num3;
      extentTick6.Tick.HoverPriority2D.Value = num3;
      if (camera.IsPointFacingCamera(extentTick5.Position, extentTick5.Normal))
        extentTick5.Tick.HoverPriority2D.MakeHigherThan(extentTick6.Tick.HoverPriority2D);
      else
        extentTick6.Tick.HoverPriority2D.MakeHigherThan(extentTick5.Tick.HoverPriority2D);
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel.ConnectTickLookAndFeel(this._rightTick, 0, AxisSign.Positive);
      this.LookAndFeel.ConnectTickLookAndFeel(this._topTick, 1, AxisSign.Positive);
      this.LookAndFeel.ConnectTickLookAndFeel(this._backTick, 2, AxisSign.Positive);
      this.LookAndFeel.ConnectTickLookAndFeel(this._leftTick, 0, AxisSign.Negative);
      this.LookAndFeel.ConnectTickLookAndFeel(this._bottomTick, 1, AxisSign.Negative);
      this.LookAndFeel.ConnectTickLookAndFeel(this._frontTick, 2, AxisSign.Negative);
    }

    private bool IsTargetReady()
    {
      return (Object) this._targetLight != (Object) null && this._targetLight.enabled && this._targetLight.gameObject.activeSelf && this._targetLight.type == LightType.Point;
    }

    public PointLightGizmo3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._extentTicks = new PointLightGizmo3D.ExtentTick[6];
      this._sphereBorderPoly = new PolygonShape2D();
      this._preChangeSnapshot = new Light3DSnapshot();
      this._postChangeSnapshot = new Light3DSnapshot();
      this._offsetDrag = new GizmoSglAxisOffsetDrag3D();
      this._lookAndFeel = new PointLightGizmo3DLookAndFeel();
      this._settings = new PointLightGizmo3DSettings();
      this._hotkeys = new PointLightGizmo3DHotkeys();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private class ExtentTick
    {
      public Vector3 Position;
      public Vector3 Normal;
      public GizmoCap2D Tick;

      public ExtentTick()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
