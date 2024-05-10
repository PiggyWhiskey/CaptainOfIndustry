// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPlaneSlider2D
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
  public class GizmoPlaneSlider2D : GizmoSlider
  {
    private int _quadIndex;
    private int _circleIndex;
    private int _polygonIndex;
    private QuadShape2D _quad;
    private CircleShape2D _circle;
    private PolygonShape2D _polygon;
    private GizmoQuad2DBorder _quadBorder;
    private GizmoCircle2DBorder _circleBorder;
    private GizmoPolygon2DBorder _polygonBorder;
    private bool _isBorderVisible;
    private bool _isBorderHoverable;
    private GizmoTransform _transform;
    private GizmoDragChannel _dragChannel;
    private IGizmoDragSession _selectedDragSession;
    private GizmoDblAxisOffsetDrag3D _offsetDrag;
    private Vector3 _offsetDragOrigin;
    private GizmoSglAxisRotationDrag3D _rotationDrag;
    private GizmoRotationArc2D _rotationArc;
    private GizmoDblAxisScaleDrag3D _scaleDrag;
    private Vector3 _scaleDragOrigin;
    private Vector3 _scaleAxisRight;
    private Vector3 _scaleAxisUp;
    private int _scaleDragAxisIndexRight;
    private int _scaleDragAxisIndexUp;
    private GizmoPlaneSlider2DControllerData _controllerData;
    private IGizmoPlaneSlider2DController[] _controllers;
    private GizmoPlaneSlider2DSettings _settings;
    private GizmoPlaneSlider2DSettings _sharedSettings;
    private GizmoPlaneSlider2DLookAndFeel _lookAndFeel;
    private GizmoPlaneSlider2DLookAndFeel _sharedLookAndFeel;

    public GizmoPlaneSlider2DSettings Settings
    {
      get => this._sharedSettings == null ? this._settings : this._sharedSettings;
    }

    public GizmoPlaneSlider2DSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public GizmoPlaneSlider2DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel == null ? this._lookAndFeel : this._sharedLookAndFeel;
    }

    public GizmoPlaneSlider2DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set => this._sharedLookAndFeel = value;
    }

    public Vector2 Position
    {
      get => this._transform.Position2D;
      set => this._transform.Position2D = value;
    }

    public Vector2 PolyCenter => this._polygon.GetEncapsulatingRect().center;

    public Quaternion Rotation => this._transform.Rotation2D;

    public float RotationDegrees
    {
      get => this._transform.Rotation2DDegrees;
      set => this._transform.Rotation2DDegrees = value;
    }

    public Vector2 Right => this._transform.GetAxis2D(0, AxisSign.Positive);

    public Vector2 Up => this._transform.GetAxis2D(1, AxisSign.Positive);

    public Vector3 OffsetDragOrigin
    {
      get => this._offsetDragOrigin;
      set => this._offsetDragOrigin = value;
    }

    public GizmoDragChannel DragChannel => this._dragChannel;

    public Vector3 ScaleDragOrigin
    {
      get => this._scaleDragOrigin;
      set => this._scaleDragOrigin = value;
    }

    public int ScaleDragAxisIndexRight
    {
      get => this._scaleDragAxisIndexRight;
      set => this._scaleDragAxisIndexRight = Mathf.Clamp(value, 0, 2);
    }

    public int ScaleDragAxisIndexUp
    {
      get => this._scaleDragAxisIndexUp;
      set => this._scaleDragAxisIndexUp = Mathf.Clamp(value, 0, 2);
    }

    public Vector3 TotalDragOffset => this._offsetDrag.TotalDragOffset;

    public Vector3 RelativeDragOffset => this._offsetDrag.RelativeDragOffset;

    public float TotalDragRotation => this._rotationDrag.TotalRotation;

    public float RelativeDragRotation => this._rotationDrag.RelativeRotation;

    public float TotalDragScaleRight => this._scaleDrag.TotalScale0;

    public float RelativeDragScaleRight => this._scaleDrag.RelativeScale0;

    public float TotalDragScaleUp => this._scaleDrag.TotalScale1;

    public float RelativeDragScaleUp => this._scaleDrag.RelativeScale1;

    public bool IsBorderVisible => this._isBorderVisible;

    public bool IsBorderHoverable => this._isBorderHoverable;

    public bool IsDragged => this.Gizmo.IsDragged && this.Gizmo.DragHandleId == this.HandleId;

    public bool IsMoving => this._offsetDrag.IsActive;

    public bool IsRotating => this._rotationDrag.IsActive;

    public bool IsScaling => this._scaleDrag.IsActive;

    public GizmoPlaneSlider2D(Gizmo gizmo, int handleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._quad = new QuadShape2D();
      this._circle = new CircleShape2D();
      this._polygon = new PolygonShape2D();
      this._isBorderVisible = true;
      this._isBorderHoverable = true;
      this._transform = new GizmoTransform();
      this._dragChannel = GizmoDragChannel.Offset;
      this._offsetDrag = new GizmoDblAxisOffsetDrag3D();
      this._rotationDrag = new GizmoSglAxisRotationDrag3D();
      this._rotationArc = new GizmoRotationArc2D();
      this._scaleDrag = new GizmoDblAxisScaleDrag3D();
      this._scaleDragAxisIndexUp = 1;
      this._controllerData = new GizmoPlaneSlider2DControllerData();
      this._controllers = new IGizmoPlaneSlider2DController[Enum.GetValues(typeof (GizmoPlane2DType)).Length];
      this._settings = new GizmoPlaneSlider2DSettings();
      this._lookAndFeel = new GizmoPlaneSlider2DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector(gizmo, handleId);
      this._quadIndex = this.Handle.Add2DShape((Shape2D) this._quad);
      this._circleIndex = this.Handle.Add2DShape((Shape2D) this._circle);
      this._polygonIndex = this.Handle.Add2DShape((Shape2D) this._polygon);
      this._quadBorder = new GizmoQuad2DBorder(this, this.Handle, this._quad);
      this._circleBorder = new GizmoCircle2DBorder(this, this.Handle, this._circle);
      this._polygonBorder = new GizmoPolygon2DBorder(this, this.Handle, this._polygon);
      this._controllerData.Gizmo = this.Gizmo;
      this._controllerData.Slider = this;
      this._controllerData.SliderHandle = this.Handle;
      this._controllerData.QuadBorder = this._quadBorder;
      this._controllerData.Quad = this._quad;
      this._controllerData.QuadIndex = this._quadIndex;
      this._controllerData.CircleBorder = this._circleBorder;
      this._controllerData.Circle = this._circle;
      this._controllerData.CircleIndex = this._circleIndex;
      this._controllerData.PolygonBorder = this._polygonBorder;
      this._controllerData.Polygon = this._polygon;
      this._controllerData.PolygonIndex = this._polygonIndex;
      this._controllers[0] = (IGizmoPlaneSlider2DController) new GizmoQuadPlaneSlider2DController(this._controllerData);
      this._controllers[1] = (IGizmoPlaneSlider2DController) new GizmoCirclePlaneSlider2DController(this._controllerData);
      this._controllers[2] = (IGizmoPlaneSlider2DController) new GizmoPolygonPlaneSlider2DController(this._controllerData);
      this._transform.Changed += new GizmoEntityTransformChangedHandler(this.OnTransformChanged);
      this.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
      this.Gizmo.PreDragBeginAttempt += new GizmoPreDragBeginAttemptHandler(this.OnGizmoAttemptHandleDragBegin);
      this.Gizmo.PreDragUpdate += new GizmoPreDragUpdateHandler(this.OnGizmoHandleDragUpdate);
      this.Gizmo.PostEnabled += new GizmoPostEnabledHandler(this.OnGizmoPostEnabled);
      this.AddTargetTransform(this._transform);
      this.AddTargetTransform(this.Gizmo.Transform);
      this._transform.SetParent(this.Gizmo.Transform);
      this.SetDragChannel(GizmoDragChannel.Offset);
    }

    public void SetBorderVisible(bool isVisible)
    {
      if (isVisible == this._isBorderVisible)
        return;
      this._isBorderVisible = isVisible;
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateHandles();
    }

    public void SetBorderHoverable(bool isHoverable)
    {
      this._isBorderHoverable = isHoverable;
      this._quadBorder.SetHoverable(isHoverable);
      this._circleBorder.SetHoverable(isHoverable);
      this._polygonBorder.SetHoverable(isHoverable);
    }

    public override void SetSnapEnabled(bool isEnabled)
    {
      this._offsetDrag.IsSnapEnabled = isEnabled;
      this._rotationDrag.IsSnapEnabled = isEnabled;
      this._scaleDrag.IsSnapEnabled = isEnabled;
    }

    public void SetPolyCwPoints(List<Vector2> cwPoints, bool isClosed)
    {
      if (this.LookAndFeel.PlaneType != GizmoPlane2DType.Polygon)
        return;
      this._polygon.SetClockwisePoints(cwPoints, isClosed);
      this._controllers[2].UpdateTransforms();
    }

    public void MakePolySphereBorder(
      Vector3 sphereCenter,
      float sphereRadius,
      int numPoints,
      Camera camera)
    {
      if (this.LookAndFeel.PlaneType != GizmoPlane2DType.Polygon)
        return;
      this._polygon.MakeSphereBorder(sphereCenter, sphereRadius, numPoints, camera);
      this._controllers[2].UpdateTransforms();
    }

    public float GetRealQuadWidth()
    {
      float num = 1f;
      if (this.IsScaling)
        num = Vector3Ex.ConvertDirTo2D(this.ScaleDragOrigin, this.ScaleDragOrigin + this._scaleAxisRight * this.TotalDragScaleRight, this.Gizmo.GetWorkCamera()).magnitude / (float) ((double) this.LookAndFeel.QuadWidth * (double) this.LookAndFeel.Scale * 0.5) * Mathf.Sign(this.TotalDragScaleRight);
      return this.LookAndFeel.QuadWidth * this.LookAndFeel.Scale * num;
    }

    public float GetRealQuadHeight()
    {
      float num = 1f;
      if (this.IsScaling)
        num = Vector3Ex.ConvertDirTo2D(this.ScaleDragOrigin, this.ScaleDragOrigin + this._scaleAxisUp * this.TotalDragScaleUp, this.Gizmo.GetWorkCamera()).magnitude / (float) ((double) this.LookAndFeel.QuadHeight * (double) this.LookAndFeel.Scale * 0.5) * Mathf.Sign(this.TotalDragScaleUp);
      return this.LookAndFeel.QuadHeight * this.LookAndFeel.Scale * num;
    }

    public Vector2 GetRealQuadSize()
    {
      return new Vector2(this.GetRealQuadWidth(), this.GetRealQuadHeight());
    }

    public float GetRealCircleRadius()
    {
      float num = 1f;
      if (this.IsScaling)
      {
        float f = this.TotalDragScaleRight;
        if ((double) Mathf.Abs(this.TotalDragScaleRight) < (double) Mathf.Abs(this.TotalDragScaleUp))
          f = this.TotalDragScaleUp;
        num = Vector3Ex.ConvertDirTo2D(this.ScaleDragOrigin, this.ScaleDragOrigin + this._scaleAxisUp * f, this.Gizmo.GetWorkCamera()).magnitude / (this.LookAndFeel.CircleRadius * this.LookAndFeel.Scale) * Mathf.Sign(f);
      }
      return this.LookAndFeel.CircleRadius * this.LookAndFeel.Scale * num;
    }

    public Vector2 GetRealExtentPoint(Shape2DExtentPoint extentPt)
    {
      return this._controllers[(int) this.LookAndFeel.PlaneType].GetRealExtentPoint(extentPt);
    }

    public void SetDragChannel(GizmoDragChannel dragChannel)
    {
      this._dragChannel = dragChannel;
      if (this._dragChannel == GizmoDragChannel.Offset)
        this._selectedDragSession = (IGizmoDragSession) this._offsetDrag;
      else if (this._dragChannel == GizmoDragChannel.Rotation)
        this._selectedDragSession = (IGizmoDragSession) this._rotationDrag;
      else if (this._dragChannel == GizmoDragChannel.Scale)
        this._selectedDragSession = (IGizmoDragSession) this._scaleDrag;
      this.Handle.DragSession = this._selectedDragSession;
    }

    public void AddTargetTransform(GizmoTransform transform)
    {
      this._offsetDrag.AddTargetTransform(transform);
      this._rotationDrag.AddTargetTransform(transform);
      this._scaleDrag.AddTargetTransform(transform);
    }

    public void AddTargetTransform(GizmoTransform transform, GizmoDragChannel dragChannel)
    {
      if (dragChannel == GizmoDragChannel.Offset)
        this._offsetDrag.AddTargetTransform(transform);
      else if (dragChannel == GizmoDragChannel.Rotation)
      {
        this._rotationDrag.AddTargetTransform(transform);
      }
      else
      {
        if (this._dragChannel != GizmoDragChannel.Scale)
          return;
        this._scaleDrag.AddTargetTransform(transform);
      }
    }

    public void RemoveTargetTransform(GizmoTransform transform)
    {
      this._offsetDrag.RemoveTargetTransform(transform);
      this._rotationDrag.RemoveTargetTransform(transform);
      this._scaleDrag.RemoveTargetTransform(transform);
    }

    public void RemoveTargetTransform(GizmoTransform transform, GizmoDragChannel dragChannel)
    {
      if (dragChannel == GizmoDragChannel.Offset)
        this._offsetDrag.RemoveTargetTransform(transform);
      else if (dragChannel == GizmoDragChannel.Rotation)
      {
        this._rotationDrag.RemoveTargetTransform(transform);
      }
      else
      {
        if (this._dragChannel != GizmoDragChannel.Scale)
          return;
        this._scaleDrag.RemoveTargetTransform(transform);
      }
    }

    public override void Render(Camera camera)
    {
      if (!this.IsVisible && !this.IsBorderVisible)
        return;
      if (this.IsRotating && this.LookAndFeel.IsRotationArcVisible && (this.LookAndFeel.PlaneType == GizmoPlane2DType.Circle || this.LookAndFeel.PlaneType == GizmoPlane2DType.Polygon) && (UnityEngine.Object) camera == (UnityEngine.Object) this.Gizmo.FocusCamera)
      {
        this._rotationArc.RotationAngle = this.TotalDragRotation;
        this._rotationArc.Render(this.LookAndFeel.RotationArcLookAndFeel, camera);
      }
      if (this.IsVisible && (this.LookAndFeel.FillMode == GizmoFillMode2D.Filled || this.LookAndFeel.FillMode == GizmoFillMode2D.FilledAndBorder))
      {
        Color color = this.LookAndFeel.Color;
        if (this.Gizmo.HoverHandleId == this.HandleId)
          color = this.LookAndFeel.HoveredColor;
        GizmoSolidMaterial get = Singleton<GizmoSolidMaterial>.Get;
        get.ResetValuesToSensibleDefaults();
        get.SetLit(false);
        get.SetColor(color);
        get.SetPass(0);
        this.Handle.Render2DSolid(camera);
      }
      if (!this.IsBorderVisible || this.LookAndFeel.FillMode != GizmoFillMode2D.Border && this.LookAndFeel.FillMode != GizmoFillMode2D.FilledAndBorder)
        return;
      if (this.LookAndFeel.PlaneType == GizmoPlane2DType.Quad)
        this._quadBorder.Render(camera);
      else if (this.LookAndFeel.PlaneType == GizmoPlane2DType.Circle)
      {
        this._circleBorder.Render(camera);
      }
      else
      {
        if (this.LookAndFeel.PlaneType != GizmoPlane2DType.Polygon)
          return;
        this._polygonBorder.Render(camera);
      }
    }

    public void Refresh()
    {
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateEpsilons();
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateTransforms();
    }

    protected override void OnVisibilityStateChanged()
    {
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateEpsilons();
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateTransforms();
    }

    protected override void OnHoverableStateChanged()
    {
      this.Handle.Set2DShapeHoverable(this._quadIndex, this.IsHoverable);
      this.Handle.Set2DShapeHoverable(this._circleIndex, this.IsHoverable);
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      int planeType = (int) this.LookAndFeel.PlaneType;
      this._controllers[planeType].UpdateHandles();
      this._offsetDrag.Sensitivity = this.Settings.OffsetSensitivity;
      this._rotationDrag.Sensitivity = this.Settings.RotationSensitivity;
      this._scaleDrag.Sensitivity = this.Settings.ScaleSensitivity;
      this._controllers[planeType].UpdateTransforms();
      this._controllers[planeType].UpdateEpsilons();
    }

    private void OnTransformChanged(GizmoTransform transform, GizmoTransform.ChangeData changeData)
    {
      if (changeData.TRSDimension != GizmoDimension.Dim2D && changeData.ChangeReason != GizmoTransform.ChangeReason.ParentChange)
        return;
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateTransforms();
    }

    private void OnGizmoAttemptHandleDragBegin(Gizmo gizmo, int handleId)
    {
      if (handleId != this.HandleId)
        return;
      if (this._dragChannel == GizmoDragChannel.Offset)
        this._offsetDrag.SetWorkData(new GizmoDblAxisOffsetDrag3D.WorkData()
        {
          Axis0 = Vector2Ex.ConvertDirTo3D(this._transform.Right2D, this.OffsetDragOrigin, this.Gizmo.FocusCamera).normalized,
          Axis1 = Vector2Ex.ConvertDirTo3D(this._transform.Up2D, this.OffsetDragOrigin, this.Gizmo.FocusCamera).normalized,
          DragOrigin = this.OffsetDragOrigin,
          SnapStep0 = this.Settings.OffsetSnapStepRight,
          SnapStep1 = this.Settings.OffsetSnapStepUp
        });
      else if (this._dragChannel == GizmoDragChannel.Rotation)
      {
        GizmoSglAxisRotationDrag3D.WorkData workData = new GizmoSglAxisRotationDrag3D.WorkData();
        workData.Axis = this.Gizmo.FocusCamera.transform.forward;
        workData.SnapMode = this.Settings.RotationSnapMode;
        workData.SnapStep = this.Settings.RotationSnapStep;
        if (this.LookAndFeel.PlaneType != GizmoPlane2DType.Polygon)
          workData.RotationPlanePos = this.Gizmo.FocusCamera.ScreenToWorldPoint(new Vector3(this.Position.x, this.Position.y, this.Gizmo.FocusCamera.nearClipPlane));
        if (this.LookAndFeel.PlaneType == GizmoPlane2DType.Circle)
        {
          this._rotationArc.SetArcData(this.Position, (Vector2) this.Gizmo.HoverInfo.HoverPoint, this.GetRealCircleRadius());
          this._rotationArc.Type = GizmoRotationArc2D.ArcType.Standard;
        }
        else if (this.LookAndFeel.PlaneType == GizmoPlane2DType.Polygon)
        {
          Vector3 polyCenter = (Vector3) this.PolyCenter;
          workData.RotationPlanePos = this.Gizmo.FocusCamera.ScreenToWorldPoint(new Vector3(polyCenter.x, polyCenter.y, this.Gizmo.FocusCamera.nearClipPlane));
          this._rotationArc.SetArcData(this.PolyCenter, (Vector2) this.Gizmo.HoverInfo.HoverPoint, 1f);
          this._rotationArc.Type = GizmoRotationArc2D.ArcType.PolyProjected;
          this._rotationArc.ProjectionPoly = this._polygon;
          this._rotationArc.NumProjectedPoints = 100;
        }
        this._rotationDrag.SetWorkData(workData);
      }
      else
      {
        if (this._dragChannel != GizmoDragChannel.Scale)
          return;
        this._scaleAxisRight = Vector2Ex.ConvertDirTo3D(this.Position, this.GetRealExtentPoint(Shape2DExtentPoint.Right), this.ScaleDragOrigin, this.Gizmo.FocusCamera);
        this._scaleAxisUp = Vector2Ex.ConvertDirTo3D(this.Position, this.GetRealExtentPoint(Shape2DExtentPoint.Top), this.ScaleDragOrigin, this.Gizmo.FocusCamera);
        this._scaleDrag.SetWorkData(new GizmoDblAxisScaleDrag3D.WorkData()
        {
          Axis0 = this._scaleAxisRight.normalized,
          Axis1 = this._scaleAxisUp.normalized,
          AxisIndex0 = this._scaleDragAxisIndexRight,
          AxisIndex1 = this._scaleDragAxisIndexUp,
          DragOrigin = this.ScaleDragOrigin,
          SnapStep = this.Settings.ProportionalScaleSnapStep
        });
      }
    }

    private void OnGizmoHandleDragUpdate(Gizmo gizmo, int handleId)
    {
      if (handleId != this.HandleId || this.DragChannel != GizmoDragChannel.Rotation)
        return;
      this._transform.Rotate2D(this.RelativeDragRotation);
    }

    private void OnGizmoPostEnabled(Gizmo gizmo) => this.Refresh();
  }
}
