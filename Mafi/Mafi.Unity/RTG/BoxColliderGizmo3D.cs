// Decompiled with JetBrains decompiler
// Type: RTG.BoxColliderGizmo3D
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class BoxColliderGizmo3D : GizmoBehaviour
  {
    private BoxCollider _targetCollider;
    private bool _scaleFromCenter;
    private bool _isMidCapVisible;
    private bool _isSnapEnabled;
    private GizmoCap2D _rightTick;
    private GizmoCap2D _topTick;
    private GizmoCap2D _backTick;
    private GizmoCap2D _leftTick;
    private GizmoCap2D _bottomTick;
    private GizmoCap2D _frontTick;
    private BoxColliderGizmo3D.FaceTick[] _faceTicks;
    private GizmoCap3D _midCap;
    private BoxCollider3DSnapshot _preChangeColliderSnapshot;
    private BoxCollider3DSnapshot _postChangeColliderSnapshot;
    private GizmoSglAxisOffsetDrag3D.WorkData _offsetDragWorkData;
    private GizmoSglAxisOffsetDrag3D _offsetDrag;
    private Vector3 _scalePivot;
    private int _dragAxisIndex;
    private GizmoUniformScaleDrag3D.WorkData _uniScaleDragWorkData;
    private GizmoUniformScaleDrag3D _uniScaleDrag;
    private BoxColliderGizmo3DLookAndFeel _lookAndFeel;
    private BoxColliderGizmo3DLookAndFeel _sharedLookAndFeel;
    private BoxColliderGizmo3DSettings _settings;
    private BoxColliderGizmo3DSettings _sharedSettings;
    private BoxColliderGizmo3DHotkeys _hotkeys;
    private BoxColliderGizmo3DHotkeys _sharedHotkeys;

    public BoxColliderGizmo3DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel != null ? this._sharedLookAndFeel : this._lookAndFeel;
    }

    public BoxColliderGizmo3DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set
      {
        this._sharedLookAndFeel = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public BoxColliderGizmo3DSettings Settings
    {
      get => this._sharedSettings != null ? this._sharedSettings : this._settings;
    }

    public BoxColliderGizmo3DSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public BoxColliderGizmo3DHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public BoxColliderGizmo3DHotkeys SharedHotkeys
    {
      get => this._sharedHotkeys;
      set => this._sharedHotkeys = value;
    }

    public BoxCollider TargetCollider => this._targetCollider;

    public bool IsSnapEnabled => this._isSnapEnabled || this.Hotkeys.EnableSnapping.IsActive();

    public void SetTargetCollider(BoxCollider boxCollider) => this._targetCollider = boxCollider;

    public override void OnGizmoEnabled() => this.OnGizmoUpdateBegin();

    public bool OwnsHandle(int handleId)
    {
      return handleId == this._leftTick.HandleId || handleId == this._rightTick.HandleId || handleId == this._topTick.HandleId || handleId == this._bottomTick.HandleId || handleId == this._frontTick.HandleId || handleId == this._backTick.HandleId || handleId == this._midCap.HandleId;
    }

    public void SetSnapEnabled(bool isEnabled) => this._isSnapEnabled = isEnabled;

    public void SetScaleFromCenterEnabled(bool isEnabled) => this._scaleFromCenter = isEnabled;

    public void SetMidCapVisible(bool visible) => this._isMidCapVisible = visible;

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
      this._faceTicks[2] = new BoxColliderGizmo3D.FaceTick();
      this._faceTicks[2].Tick = this._leftTick;
      this._faceTicks[3] = new BoxColliderGizmo3D.FaceTick();
      this._faceTicks[3].Tick = this._rightTick;
      this._faceTicks[5] = new BoxColliderGizmo3D.FaceTick();
      this._faceTicks[5].Tick = this._topTick;
      this._faceTicks[4] = new BoxColliderGizmo3D.FaceTick();
      this._faceTicks[4].Tick = this._bottomTick;
      this._faceTicks[0] = new BoxColliderGizmo3D.FaceTick();
      this._faceTicks[0].Tick = this._frontTick;
      this._faceTicks[1] = new BoxColliderGizmo3D.FaceTick();
      this._faceTicks[1].Tick = this._backTick;
      this._midCap = new GizmoCap3D(this.Gizmo, GizmoHandleId.MidScaleCap);
      this._midCap.DragSession = (IGizmoDragSession) this._uniScaleDrag;
      this.SetupSharedLookAndFeel();
    }

    public override void OnGizmoUpdateBegin()
    {
      if (!this.IsTargetReady())
        return;
      this.Gizmo.Transform.Position3D = this.CalcWorldCenter();
      this.UpdateTicks();
      this.UpdateHoverPriorities(this.Gizmo.GetWorkCamera());
      this._midCap.Position = this.Gizmo.Transform.Position3D;
      this._midCap.SetVisible(this.LookAndFeel.IsMidCapVisible && this._isMidCapVisible);
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      if (!this.IsTargetReady())
        return;
      this._preChangeColliderSnapshot.Snapshot(this._targetCollider);
      if (!this.OwnsHandle(handleId))
        return;
      if (handleId == this._midCap.HandleId)
      {
        this._uniScaleDragWorkData.CameraRight = this.Gizmo.GetWorkCamera().transform.right;
        this._uniScaleDragWorkData.CameraUp = this.Gizmo.GetWorkCamera().transform.up;
        this._uniScaleDragWorkData.DragOrigin = this.CalcWorldCenter();
        this._uniScaleDragWorkData.SnapStep = this.Settings.UniformSizeSnapStep;
        this._uniScaleDrag.SetWorkData(this._uniScaleDragWorkData);
      }
      else
      {
        this._offsetDragWorkData.DragOrigin = this.CalcWorldCenter();
        this._scalePivot = this.CalcScalePivot(handleId);
        if (handleId == this._leftTick.HandleId)
        {
          this._offsetDragWorkData.Axis = -this._targetCollider.transform.right;
          this._offsetDragWorkData.SnapStep = this.Settings.XSizeSnapStep;
          this._dragAxisIndex = 0;
        }
        else if (handleId == this._rightTick.HandleId)
        {
          this._offsetDragWorkData.Axis = this._targetCollider.transform.right;
          this._offsetDragWorkData.SnapStep = this.Settings.XSizeSnapStep;
          this._dragAxisIndex = 0;
        }
        else if (handleId == this._topTick.HandleId)
        {
          this._offsetDragWorkData.Axis = this._targetCollider.transform.up;
          this._offsetDragWorkData.SnapStep = this.Settings.YSizeSnapStep;
          this._dragAxisIndex = 1;
        }
        else if (handleId == this._bottomTick.HandleId)
        {
          this._offsetDragWorkData.Axis = -this._targetCollider.transform.up;
          this._offsetDragWorkData.SnapStep = this.Settings.YSizeSnapStep;
          this._dragAxisIndex = 1;
        }
        else if (handleId == this._frontTick.HandleId)
        {
          this._offsetDragWorkData.Axis = -this._targetCollider.transform.forward;
          this._offsetDragWorkData.SnapStep = this.Settings.ZSizeSnapStep;
          this._dragAxisIndex = 2;
        }
        else if (handleId == this._backTick.HandleId)
        {
          this._offsetDragWorkData.Axis = this._targetCollider.transform.forward;
          this._offsetDragWorkData.SnapStep = this.Settings.ZSizeSnapStep;
          this._dragAxisIndex = 2;
        }
        this._offsetDrag.SetWorkData(this._offsetDragWorkData);
      }
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (!this.IsTargetReady() || !this.OwnsHandle(handleId))
        return;
      this._uniScaleDrag.IsSnapEnabled = this.IsSnapEnabled;
      this._offsetDrag.IsSnapEnabled = this.IsSnapEnabled;
      if (handleId == this._midCap.HandleId)
      {
        Vector3 a = Vector3.Scale(this.Gizmo.RelativeDragScale, this.CalcWorldSize()).Abs();
        if (a == Vector3.zero)
          a = Vector3Ex.FromValue(1E-08f);
        this._targetCollider.size = Vector3.Scale(a, this._targetCollider.transform.lossyScale.GetInverse());
      }
      else
      {
        Vector3 a = this.CalcWorldSize();
        float f = Vector3.Dot(this.Gizmo.RelativeDragOffset, this._offsetDrag.Axis);
        a[this._dragAxisIndex] = Math.Max(0.0f, a[this._dragAxisIndex] + Mathf.Abs(f) * Mathf.Sign(f));
        Vector3 position = this._scalePivot + this._offsetDrag.Axis * (a[this._dragAxisIndex] * 0.5f);
        if (!this.Hotkeys.ScaleFromCenter.IsActive() && !this._scaleFromCenter)
          this._targetCollider.center = this._targetCollider.transform.InverseTransformPoint(position);
        this._targetCollider.size = Vector3.Scale(a, this._targetCollider.transform.lossyScale.GetInverse());
      }
      this.UpdateTicks();
    }

    public override void OnGizmoDragEnd(int handleId)
    {
      if (!this.OwnsHandle(handleId))
        return;
      this._postChangeColliderSnapshot.Snapshot(this._targetCollider);
      new BoxCollider3DChangedAction(this._preChangeColliderSnapshot, this._postChangeColliderSnapshot).Execute();
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (!this.IsTargetReady())
        return;
      GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
      get.ResetValuesToSensibleDefaults();
      get.SetColor(this.LookAndFeel.WireColor);
      get.SetPass(0);
      GraphicsEx.DrawWireBox(new OBB(this.CalcWorldCenter(), this.CalcWorldSize(), this._targetCollider.transform.rotation));
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
        this.UpdateTicks();
      this.UpdateTickColors(camera);
      this._leftTick.Render(camera);
      this._rightTick.Render(camera);
      this._topTick.Render(camera);
      this._bottomTick.Render(camera);
      this._frontTick.Render(camera);
      this._backTick.Render(camera);
      this._midCap.Render(camera);
    }

    private Vector3 CalcScalePivot(int handleId)
    {
      if (this.OwnsHandle(handleId))
      {
        Vector3 boxCenter = this.CalcWorldCenter();
        Vector3 boxSize = this.CalcWorldSize();
        Quaternion rotation = this._targetCollider.transform.rotation;
        if (handleId == this._leftTick.HandleId)
          return BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Right);
        if (handleId == this._rightTick.HandleId)
          return BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Left);
        if (handleId == this._topTick.HandleId)
          return BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Bottom);
        if (handleId == this._bottomTick.HandleId)
          return BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Top);
        if (handleId == this._frontTick.HandleId)
          return BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Back);
        if (handleId == this._backTick.HandleId)
          return BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Front);
      }
      return Vector3.zero;
    }

    private Vector3 CalcWorldCenter()
    {
      return this._targetCollider.transform.TransformPoint(this._targetCollider.center);
    }

    private Vector3 CalcWorldSize()
    {
      return Vector3.Scale(this._targetCollider.size, this._targetCollider.transform.lossyScale);
    }

    private void UpdateTicks()
    {
      Camera workCamera = this.Gizmo.GetWorkCamera();
      Vector3 boxCenter = this.CalcWorldCenter();
      Vector3 boxSize = this.CalcWorldSize();
      Quaternion rotation = this._targetCollider.transform.rotation;
      Vector3 position1 = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Left);
      this._leftTick.Position = (Vector2) workCamera.WorldToScreenPoint(position1);
      this._faceTicks[2].FaceCenter = position1;
      this._faceTicks[2].FaceNormal = -this._targetCollider.transform.right;
      Vector3 position2 = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Right);
      this._rightTick.Position = (Vector2) workCamera.WorldToScreenPoint(position2);
      this._faceTicks[3].FaceCenter = position2;
      this._faceTicks[3].FaceNormal = this._targetCollider.transform.right;
      Vector3 position3 = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Top);
      this._topTick.Position = (Vector2) workCamera.WorldToScreenPoint(position3);
      this._faceTicks[5].FaceCenter = position3;
      this._faceTicks[5].FaceNormal = this._targetCollider.transform.up;
      Vector3 position4 = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Bottom);
      this._bottomTick.Position = (Vector2) workCamera.WorldToScreenPoint(position4);
      this._faceTicks[4].FaceCenter = position4;
      this._faceTicks[4].FaceNormal = -this._targetCollider.transform.up;
      Vector3 position5 = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Front);
      this._frontTick.Position = (Vector2) workCamera.WorldToScreenPoint(position5);
      this._faceTicks[0].FaceCenter = position5;
      this._faceTicks[0].FaceNormal = -this._targetCollider.transform.forward;
      Vector3 position6 = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Back);
      this._backTick.Position = (Vector2) workCamera.WorldToScreenPoint(position6);
      this._faceTicks[1].FaceCenter = position6;
      this._faceTicks[1].FaceNormal = this._targetCollider.transform.forward;
    }

    private void UpdateTickColors(Camera camera)
    {
      Plane plane = new Plane(camera.transform.forward, camera.transform.position);
      foreach (BoxColliderGizmo3D.FaceTick faceTick in this._faceTicks)
      {
        GizmoCap2D tick = faceTick.Tick;
        if (this.Gizmo.HoverHandleId != tick.HandleId && !camera.IsPointFacingCamera(faceTick.FaceCenter, faceTick.FaceNormal))
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
        if ((double) plane.GetDistanceToPoint(faceTick.FaceCenter) > 0.0)
          tick.SetVisible(true);
        else
          tick.SetVisible(false);
      }
    }

    private void UpdateHoverPriorities(Camera camera)
    {
      int num1 = 0;
      BoxColliderGizmo3D.FaceTick faceTick1 = this._faceTicks[2];
      BoxColliderGizmo3D.FaceTick faceTick2 = this._faceTicks[3];
      faceTick1.Tick.HoverPriority2D.Value = num1;
      faceTick2.Tick.HoverPriority2D.Value = num1;
      if (camera.IsPointFacingCamera(faceTick1.FaceCenter, faceTick1.FaceNormal))
        faceTick1.Tick.HoverPriority2D.MakeHigherThan(faceTick2.Tick.HoverPriority2D);
      else
        faceTick2.Tick.HoverPriority2D.MakeHigherThan(faceTick1.Tick.HoverPriority2D);
      this._midCap.GenericHoverPriority.MakeHigherThan(faceTick1.Tick.GenericHoverPriority);
      this._midCap.GenericHoverPriority.MakeHigherThan(faceTick2.Tick.GenericHoverPriority);
      int num2 = num1 + 2;
      BoxColliderGizmo3D.FaceTick faceTick3 = this._faceTicks[5];
      BoxColliderGizmo3D.FaceTick faceTick4 = this._faceTicks[4];
      faceTick3.Tick.HoverPriority2D.Value = num2;
      faceTick4.Tick.HoverPriority2D.Value = num2;
      if (camera.IsPointFacingCamera(faceTick3.FaceCenter, faceTick3.FaceNormal))
        faceTick3.Tick.HoverPriority2D.MakeHigherThan(faceTick4.Tick.HoverPriority2D);
      else
        faceTick4.Tick.HoverPriority2D.MakeHigherThan(faceTick3.Tick.HoverPriority2D);
      this._midCap.GenericHoverPriority.MakeHigherThan(faceTick3.Tick.GenericHoverPriority);
      this._midCap.GenericHoverPriority.MakeHigherThan(faceTick4.Tick.GenericHoverPriority);
      int num3 = num2 + 2;
      BoxColliderGizmo3D.FaceTick faceTick5 = this._faceTicks[0];
      BoxColliderGizmo3D.FaceTick faceTick6 = this._faceTicks[1];
      faceTick5.Tick.HoverPriority2D.Value = num3;
      faceTick6.Tick.HoverPriority2D.Value = num3;
      if (camera.IsPointFacingCamera(faceTick5.FaceCenter, faceTick5.FaceNormal))
        faceTick5.Tick.HoverPriority2D.MakeHigherThan(faceTick6.Tick.HoverPriority2D);
      else
        faceTick6.Tick.HoverPriority2D.MakeHigherThan(faceTick5.Tick.HoverPriority2D);
      this._midCap.GenericHoverPriority.MakeHigherThan(faceTick5.Tick.GenericHoverPriority);
      this._midCap.GenericHoverPriority.MakeHigherThan(faceTick6.Tick.GenericHoverPriority);
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel.ConnectTickLookAndFeel(this._rightTick, 0, AxisSign.Positive);
      this.LookAndFeel.ConnectTickLookAndFeel(this._topTick, 1, AxisSign.Positive);
      this.LookAndFeel.ConnectTickLookAndFeel(this._backTick, 2, AxisSign.Positive);
      this.LookAndFeel.ConnectTickLookAndFeel(this._leftTick, 0, AxisSign.Negative);
      this.LookAndFeel.ConnectTickLookAndFeel(this._bottomTick, 1, AxisSign.Negative);
      this.LookAndFeel.ConnectTickLookAndFeel(this._frontTick, 2, AxisSign.Negative);
      this.LookAndFeel.ConnectMidCapLookAndFeel(this._midCap);
    }

    private bool IsTargetReady()
    {
      return (UnityEngine.Object) this._targetCollider != (UnityEngine.Object) null && this._targetCollider.enabled && this._targetCollider.gameObject.activeSelf;
    }

    public BoxColliderGizmo3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isMidCapVisible = true;
      this._faceTicks = new BoxColliderGizmo3D.FaceTick[6];
      this._preChangeColliderSnapshot = new BoxCollider3DSnapshot();
      this._postChangeColliderSnapshot = new BoxCollider3DSnapshot();
      this._offsetDrag = new GizmoSglAxisOffsetDrag3D();
      this._dragAxisIndex = -1;
      this._uniScaleDrag = new GizmoUniformScaleDrag3D();
      this._lookAndFeel = new BoxColliderGizmo3DLookAndFeel();
      this._settings = new BoxColliderGizmo3DSettings();
      this._hotkeys = new BoxColliderGizmo3DHotkeys();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    private class FaceTick
    {
      public Vector3 FaceCenter;
      public Vector3 FaceNormal;
      public GizmoCap2D Tick;

      public FaceTick()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
