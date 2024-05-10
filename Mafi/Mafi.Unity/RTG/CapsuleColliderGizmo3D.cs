// Decompiled with JetBrains decompiler
// Type: RTG.CapsuleColliderGizmo3D
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
  public class CapsuleColliderGizmo3D : GizmoBehaviour
  {
    private CapsuleCollider _targetCollider;
    private bool _scaleFromCenter;
    private bool _isSnapEnabled;
    private float _heightOnDragBegin;
    private GizmoCap2D _rightTick;
    private GizmoCap2D _topTick;
    private GizmoCap2D _backTick;
    private GizmoCap2D _leftTick;
    private GizmoCap2D _bottomTick;
    private GizmoCap2D _frontTick;
    private CapsuleColliderGizmo3D.ExtentTick[] _extentTicks;
    private List<Vector3> _semiCirclePts;
    private List<Vector3> _circlePts;
    private CapsuleCollider3DSnapshot _preChangeColliderSnapshot;
    private CapsuleCollider3DSnapshot _postChangeColliderSnapshot;
    private GizmoSglAxisOffsetDrag3D.WorkData _offsetDragWorkData;
    private GizmoSglAxisOffsetDrag3D _offsetDrag;
    private CapsuleColliderGizmo3DLookAndFeel _lookAndFeel;
    private CapsuleColliderGizmo3DLookAndFeel _sharedLookAndFeel;
    private CapsuleColliderGizmo3DSettings _settings;
    private CapsuleColliderGizmo3DSettings _sharedSettings;
    private CapsuleColliderGizmo3DHotkeys _hotkeys;
    private CapsuleColliderGizmo3DHotkeys _sharedHotkeys;

    public CapsuleColliderGizmo3DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel != null ? this._sharedLookAndFeel : this._lookAndFeel;
    }

    public CapsuleColliderGizmo3DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set
      {
        this._sharedLookAndFeel = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public CapsuleColliderGizmo3DSettings Settings
    {
      get => this._sharedSettings != null ? this._sharedSettings : this._settings;
    }

    public CapsuleColliderGizmo3DSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public CapsuleColliderGizmo3DHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public CapsuleColliderGizmo3DHotkeys SharedHotkeys
    {
      get => this._sharedHotkeys;
      set => this._sharedHotkeys = value;
    }

    public CapsuleCollider TargetCollider => this._targetCollider;

    public bool IsSnapEnabled => this._isSnapEnabled || this.Hotkeys.EnableSnapping.IsActive();

    public void SetTargetCollider(CapsuleCollider capsuleCollider)
    {
      this._targetCollider = capsuleCollider;
    }

    public override void OnGizmoEnabled() => this.OnGizmoUpdateBegin();

    public bool OwnsHandle(int handleId)
    {
      return handleId == this._leftTick.HandleId || handleId == this._rightTick.HandleId || handleId == this._topTick.HandleId || handleId == this._bottomTick.HandleId || handleId == this._frontTick.HandleId || handleId == this._backTick.HandleId;
    }

    public void SetSnapEnabled(bool isEnabled) => this._isSnapEnabled = isEnabled;

    public void SetScaleFromCenterEnabled(bool isEnabled) => this._scaleFromCenter = isEnabled;

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
      this._extentTicks[2] = new CapsuleColliderGizmo3D.ExtentTick();
      this._extentTicks[2].Tick = this._leftTick;
      this._extentTicks[3] = new CapsuleColliderGizmo3D.ExtentTick();
      this._extentTicks[3].Tick = this._rightTick;
      this._extentTicks[5] = new CapsuleColliderGizmo3D.ExtentTick();
      this._extentTicks[5].Tick = this._topTick;
      this._extentTicks[4] = new CapsuleColliderGizmo3D.ExtentTick();
      this._extentTicks[4].Tick = this._bottomTick;
      this._extentTicks[0] = new CapsuleColliderGizmo3D.ExtentTick();
      this._extentTicks[0].Tick = this._frontTick;
      this._extentTicks[1] = new CapsuleColliderGizmo3D.ExtentTick();
      this._extentTicks[1].Tick = this._backTick;
      this._semiCirclePts = PrimitiveFactory.Generate3DArcBorderPoints(Vector3.zero, -Vector3.right, new Plane(Vector3.forward, 0.0f), -180f, false, 100);
      this._circlePts = PrimitiveFactory.Generate3DCircleBorderPoints(Vector3.zero, 1f, Vector3.right, Vector3.up, 100);
      this.SetupSharedLookAndFeel();
    }

    public override void OnGizmoUpdateBegin()
    {
      if (!this.IsTargetReady())
        return;
      this.Gizmo.Transform.Position3D = this.CalcWorldCenter();
      this.UpdateHandles();
      this.UpdateHoverPriorities(this.Gizmo.GetWorkCamera());
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      if (!this.IsTargetReady())
        return;
      this._preChangeColliderSnapshot.Snapshot(this._targetCollider);
      if (!this.OwnsHandle(handleId))
        return;
      this._heightOnDragBegin = this._targetCollider.height;
      this._offsetDragWorkData.DragOrigin = this.CalcWorldCenter();
      this._offsetDragWorkData.SnapStep = this.Settings.RadiusSnapStep;
      Quaternion quaternion = this.CalcRotationByDirection();
      Vector3 vector3_1 = quaternion * this._targetCollider.transform.right;
      Vector3 vector3_2 = quaternion * this._targetCollider.transform.up;
      Vector3 vector3_3 = quaternion * this._targetCollider.transform.forward;
      if (handleId == this._leftTick.HandleId)
        this._offsetDragWorkData.Axis = -vector3_1;
      else if (handleId == this._rightTick.HandleId)
        this._offsetDragWorkData.Axis = vector3_1;
      else if (handleId == this._topTick.HandleId)
        this._offsetDragWorkData.Axis = vector3_2;
      else if (handleId == this._bottomTick.HandleId)
        this._offsetDragWorkData.Axis = -vector3_2;
      else if (handleId == this._frontTick.HandleId)
        this._offsetDragWorkData.Axis = -vector3_3;
      else if (handleId == this._backTick.HandleId)
        this._offsetDragWorkData.Axis = vector3_3;
      this._offsetDrag.SetWorkData(this._offsetDragWorkData);
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (!this.IsTargetReady() || !this.OwnsHandle(handleId))
        return;
      this._offsetDrag.IsSnapEnabled = this.IsSnapEnabled;
      if (handleId == this._bottomTick.HandleId || handleId == this._topTick.HandleId)
      {
        float num1 = Mathf.Max(0.0f, this.CalcWorldHeight() + this.Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(this.Gizmo.RelativeDragOffset, this._offsetDrag.Axis)));
        float num2 = num1 / this.CalcHeightScale();
        Vector3 position1 = this.CalcScalePivot(handleId) + this._offsetDrag.Axis * num1 * 0.5f;
        if (!this.Hotkeys.ScaleFromCenter.IsActive() && !this._scaleFromCenter)
          this._targetCollider.center = this._targetCollider.transform.InverseTransformPoint(position1);
        this._targetCollider.height = num2;
        if ((double) this._targetCollider.height < 2.0 * (double) this._targetCollider.radius)
        {
          this._targetCollider.height = 2f * this._targetCollider.radius;
          float height = this._targetCollider.height;
          float num3 = num1;
          float num4 = height * this.CalcHeightScale() - num3;
          Vector3 position2 = position1 + this._offsetDrag.Axis * num4 * 0.5f;
          if (!this.Hotkeys.ScaleFromCenter.IsActive() && !this._scaleFromCenter)
            this._targetCollider.center = this._targetCollider.transform.InverseTransformPoint(position2);
        }
      }
      else
      {
        float num = Mathf.Max(0.0f, this.CalcWorldRadius() + this.Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(this.Gizmo.RelativeDragOffset, this._offsetDrag.Axis)));
        Vector3 position = this.CalcScalePivot(handleId) + this._offsetDrag.Axis * num;
        if (!this.Hotkeys.ScaleFromCenter.IsActive() && !this._scaleFromCenter)
          this._targetCollider.center = this._targetCollider.transform.InverseTransformPoint(position);
        this._targetCollider.radius = num / this.CalcRadiusScale();
        this._targetCollider.height = this._heightOnDragBegin;
        if ((double) this._targetCollider.height < 2.0 * (double) this._targetCollider.radius)
          this._targetCollider.height = 2f * this._targetCollider.radius;
      }
      this.UpdateHandles();
    }

    public override void OnGizmoDragEnd(int handleId)
    {
      if (!this.OwnsHandle(handleId))
        return;
      this._postChangeColliderSnapshot.Snapshot(this._targetCollider);
      new CapsuleCollider3DChangedAction(this._preChangeColliderSnapshot, this._postChangeColliderSnapshot).Execute();
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (!this.IsTargetReady())
        return;
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
        this.UpdateHandles();
      Vector3 vector3_1 = this.CalcWorldCenter();
      float num1 = this.CalcWorldRadius();
      float num2 = this.CalcWorldHeight();
      Quaternion q = this.CalcRotationByDirection();
      Vector3 vector3_2 = q * this._targetCollider.transform.right;
      Vector3 vector3_3 = q * this._targetCollider.transform.up;
      Vector3 vector3_4 = q * this._targetCollider.transform.forward;
      GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
      get.ResetValuesToSensibleDefaults();
      get.SetColor(this.LookAndFeel.WireColor);
      get.SetPass(0);
      float num3 = num2 - 2f * num1;
      if ((double) num3 > 0.0)
      {
        GLRenderer.DrawLine3D(vector3_1 - vector3_2 * num1 - vector3_3 * num3 * 0.5f, vector3_1 - vector3_2 * num1 + vector3_3 * num3 * 0.5f);
        GLRenderer.DrawLine3D(vector3_1 + vector3_2 * num1 - vector3_3 * num3 * 0.5f, vector3_1 + vector3_2 * num1 + vector3_3 * num3 * 0.5f);
        GLRenderer.DrawLine3D(vector3_1 - vector3_4 * num1 - vector3_3 * num3 * 0.5f, vector3_1 - vector3_4 * num1 + vector3_3 * num3 * 0.5f);
        GLRenderer.DrawLine3D(vector3_1 + vector3_4 * num1 - vector3_3 * num3 * 0.5f, vector3_1 + vector3_4 * num1 + vector3_3 * num3 * 0.5f);
      }
      Vector3 s = new Vector3(num1, num1, 1f);
      Vector3 pos1 = vector3_1 + vector3_3 * (num2 * 0.5f - num1);
      GL.PushMatrix();
      GL.MultMatrix(Matrix4x4.TRS(pos1, q, s));
      GLRenderer.DrawLines3D(this._semiCirclePts);
      GL.PopMatrix();
      GL.PushMatrix();
      GL.MultMatrix(Matrix4x4.TRS(pos1, q * Quaternion.Euler(0.0f, 90f, 0.0f), s));
      GLRenderer.DrawLines3D(this._semiCirclePts);
      GL.PopMatrix();
      GL.PushMatrix();
      GL.MultMatrix(Matrix4x4.TRS(pos1, q * Quaternion.Euler(90f, 0.0f, 0.0f), new Vector3(num1, num1, num1)));
      GLRenderer.DrawLines3D(this._circlePts);
      GL.PopMatrix();
      Vector3 pos2 = vector3_1 - vector3_3 * (num2 * 0.5f - num1);
      GL.PushMatrix();
      GL.MultMatrix(Matrix4x4.TRS(pos2, q * Quaternion.Euler(0.0f, 0.0f, 180f), s));
      GLRenderer.DrawLines3D(this._semiCirclePts);
      GL.PopMatrix();
      GL.PushMatrix();
      GL.MultMatrix(Matrix4x4.TRS(pos2, q * Quaternion.Euler(0.0f, 90f, 0.0f) * Quaternion.Euler(0.0f, 0.0f, 180f), s));
      GLRenderer.DrawLines3D(this._semiCirclePts);
      GL.PopMatrix();
      GL.PushMatrix();
      GL.MultMatrix(Matrix4x4.TRS(pos2, q * Quaternion.Euler(90f, 0.0f, 0.0f), new Vector3(num1, num1, num1)));
      GLRenderer.DrawLines3D(this._circlePts);
      GL.PopMatrix();
      this.UpdateTickColors(camera);
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
        Vector3 vector3_1 = this.CalcWorldCenter();
        float num1 = this.CalcWorldRadius();
        float num2 = this.CalcWorldHeight();
        Quaternion quaternion = this.CalcRotationByDirection();
        Vector3 vector3_2 = quaternion * this._targetCollider.transform.right;
        Vector3 vector3_3 = quaternion * this._targetCollider.transform.up;
        Vector3 vector3_4 = quaternion * this._targetCollider.transform.forward;
        if (handleId == this._leftTick.HandleId)
          return vector3_1 + vector3_2 * num1;
        if (handleId == this._rightTick.HandleId)
          return vector3_1 - vector3_2 * num1;
        if (handleId == this._topTick.HandleId)
          return vector3_1 - vector3_3 * num2 * 0.5f;
        if (handleId == this._bottomTick.HandleId)
          return vector3_1 + vector3_3 * num2 * 0.5f;
        if (handleId == this._frontTick.HandleId)
          return vector3_1 + vector3_4 * num1;
        if (handleId == this._backTick.HandleId)
          return vector3_1 - vector3_4 * num1;
      }
      return Vector3.zero;
    }

    private Vector3 CalcWorldCenter()
    {
      return this._targetCollider.transform.TransformPoint(this._targetCollider.center);
    }

    private float CalcWorldRadius() => this._targetCollider.radius * this.CalcRadiusScale();

    private float CalcWorldHeight() => this._targetCollider.height * this.CalcHeightScale();

    private float CalcHeightScale()
    {
      if (this._targetCollider.direction == 1)
        return Mathf.Abs(this._targetCollider.transform.lossyScale.y);
      return this._targetCollider.direction == 0 ? Mathf.Abs(this._targetCollider.transform.lossyScale.x) : Mathf.Abs(this._targetCollider.transform.lossyScale.z);
    }

    private float CalcRadiusScale()
    {
      Vector3 vector3 = this._targetCollider.transform.lossyScale.Abs();
      if (this._targetCollider.direction == 1)
        return Mathf.Max(vector3.x, vector3.z);
      return this._targetCollider.direction == 0 ? Mathf.Max(vector3.y, vector3.z) : Mathf.Max(vector3.x, vector3.y);
    }

    private Quaternion CalcRotationByDirection()
    {
      if (this._targetCollider.direction == 1)
        return Quaternion.identity;
      return this._targetCollider.direction == 0 ? Quaternion.Euler(0.0f, 0.0f, 90f) : Quaternion.Euler(90f, 0.0f, 0.0f);
    }

    private void UpdateHandles()
    {
      Camera workCamera = this.Gizmo.GetWorkCamera();
      Vector3 vector3_1 = this.CalcWorldCenter();
      float num1 = this.CalcWorldRadius();
      float num2 = this.CalcWorldHeight();
      Quaternion quaternion = this.CalcRotationByDirection();
      Vector3 vector3_2 = quaternion * this._targetCollider.transform.right;
      Vector3 vector3_3 = quaternion * this._targetCollider.transform.up;
      Vector3 vector3_4 = quaternion * this._targetCollider.transform.forward;
      Vector3 position1 = vector3_1 - vector3_2 * num1;
      this._leftTick.Position = (Vector2) workCamera.WorldToScreenPoint(position1);
      this._extentTicks[2].Position = position1;
      this._extentTicks[2].Normal = -vector3_2;
      Vector3 position2 = vector3_1 + vector3_2 * num1;
      this._rightTick.Position = (Vector2) workCamera.WorldToScreenPoint(position2);
      this._extentTicks[3].Position = position2;
      this._extentTicks[3].Normal = vector3_2;
      Vector3 position3 = vector3_1 + vector3_3 * num2 * 0.5f;
      this._topTick.Position = (Vector2) workCamera.WorldToScreenPoint(position3);
      this._extentTicks[5].Position = position3;
      this._extentTicks[5].Normal = vector3_3;
      Vector3 position4 = vector3_1 - vector3_3 * num2 * 0.5f;
      this._bottomTick.Position = (Vector2) workCamera.WorldToScreenPoint(position4);
      this._extentTicks[4].Position = position4;
      this._extentTicks[4].Normal = -vector3_3;
      Vector3 position5 = vector3_1 - vector3_4 * num1;
      this._frontTick.Position = (Vector2) workCamera.WorldToScreenPoint(position5);
      this._extentTicks[0].Position = position5;
      this._extentTicks[0].Normal = -vector3_4;
      Vector3 position6 = vector3_1 + vector3_4 * num1;
      this._backTick.Position = (Vector2) workCamera.WorldToScreenPoint(position6);
      this._extentTicks[1].Position = position6;
      this._extentTicks[1].Normal = vector3_4;
    }

    private void UpdateTickColors(Camera camera)
    {
      Plane plane = new Plane(camera.transform.forward, camera.transform.position);
      foreach (CapsuleColliderGizmo3D.ExtentTick extentTick in this._extentTicks)
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
      CapsuleColliderGizmo3D.ExtentTick extentTick1 = this._extentTicks[2];
      CapsuleColliderGizmo3D.ExtentTick extentTick2 = this._extentTicks[3];
      extentTick1.Tick.HoverPriority2D.Value = num1;
      extentTick2.Tick.HoverPriority2D.Value = num1;
      if (camera.IsPointFacingCamera(extentTick1.Position, extentTick1.Normal))
        extentTick1.Tick.HoverPriority2D.MakeHigherThan(extentTick2.Tick.HoverPriority2D);
      else
        extentTick2.Tick.HoverPriority2D.MakeHigherThan(extentTick1.Tick.HoverPriority2D);
      int num2 = num1 + 2;
      CapsuleColliderGizmo3D.ExtentTick extentTick3 = this._extentTicks[5];
      CapsuleColliderGizmo3D.ExtentTick extentTick4 = this._extentTicks[4];
      extentTick3.Tick.HoverPriority2D.Value = num2;
      extentTick4.Tick.HoverPriority2D.Value = num2;
      if (camera.IsPointFacingCamera(extentTick3.Position, extentTick3.Normal))
        extentTick3.Tick.HoverPriority2D.MakeHigherThan(extentTick4.Tick.HoverPriority2D);
      else
        extentTick4.Tick.HoverPriority2D.MakeHigherThan(extentTick3.Tick.HoverPriority2D);
      int num3 = num2 + 2;
      CapsuleColliderGizmo3D.ExtentTick extentTick5 = this._extentTicks[0];
      CapsuleColliderGizmo3D.ExtentTick extentTick6 = this._extentTicks[1];
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
      return (Object) this._targetCollider != (Object) null && this._targetCollider.enabled && this._targetCollider.gameObject.activeSelf;
    }

    public CapsuleColliderGizmo3D()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._extentTicks = new CapsuleColliderGizmo3D.ExtentTick[6];
      this._semiCirclePts = new List<Vector3>();
      this._circlePts = new List<Vector3>();
      this._preChangeColliderSnapshot = new CapsuleCollider3DSnapshot();
      this._postChangeColliderSnapshot = new CapsuleCollider3DSnapshot();
      this._offsetDrag = new GizmoSglAxisOffsetDrag3D();
      this._lookAndFeel = new CapsuleColliderGizmo3DLookAndFeel();
      this._settings = new CapsuleColliderGizmo3DSettings();
      this._hotkeys = new CapsuleColliderGizmo3DHotkeys();
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
