// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPlaneSlider3D
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
  public class GizmoPlaneSlider3D : GizmoSlider
  {
    private int _quadIndex;
    private int _raTriangleIndex;
    private int _circleIndex;
    private QuadShape3D _quad;
    private RightAngTriangle3D _raTriangle;
    private CircleShape3D _circle;
    private GizmoQuad3DBorder _quadBorder;
    private GizmoRATriangle3DBorder _raTriangleBorder;
    private GizmoCircle3DBorder _circleBorder;
    private bool _isBorderHoverable;
    private bool _isBorderVisible;
    private GizmoTransform _transform;
    private GizmoDragChannel _dragChannel;
    private IGizmoDragSession _selectedDragSession;
    private GizmoDblAxisOffsetDrag3D _dblAxisOffsetDrag;
    private GizmoSglAxisRotationDrag3D _rotationDrag;
    private GizmoRotationArc3D _rotationArc;
    private GizmoDblAxisScaleDrag3D _scaleDrag;
    private int _scaleDragAxisIndexRight;
    private int _scaleDragAxisIndexUp;
    private GizmoPlaneSlider3DControllerData _controllerData;
    private IGizmoPlaneSlider3DController[] _controllers;
    private GizmoPlaneSlider3DSettings _settings;
    private GizmoPlaneSlider3DSettings _sharedSettings;
    private GizmoPlaneSlider3DLookAndFeel _lookAndFeel;
    private GizmoPlaneSlider3DLookAndFeel _sharedLookAndFeel;

    public GizmoPlaneSlider3DSettings Settings
    {
      get => this._sharedSettings == null ? this._settings : this._sharedSettings;
    }

    public GizmoPlaneSlider3DSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public GizmoPlaneSlider3DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel == null ? this._lookAndFeel : this._sharedLookAndFeel;
    }

    public GizmoPlaneSlider3DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set => this._sharedLookAndFeel = value;
    }

    public Plane Plane => new Plane(this.Normal, this.Position);

    public Vector3 Position
    {
      get => this._transform.Position3D;
      set => this._transform.Position3D = value;
    }

    public Quaternion Rotation
    {
      get => this._transform.Rotation3D;
      set => this._transform.Rotation3D = value;
    }

    public Quaternion LocalRotation
    {
      get => this._transform.LocalRotation3D;
      set => this._transform.LocalRotation3D = value;
    }

    public Vector3 Right => this._transform.GetAxis3D(0, AxisSign.Positive);

    public Vector3 Up => this._transform.GetAxis3D(1, AxisSign.Positive);

    public Vector3 Look => this._transform.GetAxis3D(2, AxisSign.Positive);

    public Vector3 Normal => this.Look;

    public GizmoDragChannel DragChannel => this._dragChannel;

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

    public Vector3 TotalDragOffset => this._dblAxisOffsetDrag.TotalDragOffset;

    public Vector3 RelativeDragOffset => this._dblAxisOffsetDrag.RelativeDragOffset;

    public float TotalDragRotation => this._rotationDrag.TotalRotation;

    public float RelativeDragRotation => this._rotationDrag.RelativeRotation;

    public float TotalDragScaleRight => this._scaleDrag.TotalScale0;

    public float RelativeDragScaleRight => this._scaleDrag.RelativeScale0;

    public float TotalDragScaleUp => this._scaleDrag.TotalScale1;

    public float RelativeDragScaleUp => this._scaleDrag.RelativeScale1;

    public bool IsBorderVisible => this._isBorderVisible;

    public bool IsBorderHoverable => this._isBorderHoverable;

    public bool IsDragged => this.Gizmo.IsDragged && this.Gizmo.DragHandleId == this.HandleId;

    public bool IsMoving => this._dblAxisOffsetDrag.IsActive;

    public bool IsRotating => this._rotationDrag.IsActive;

    public bool IsScaling => this._scaleDrag.IsActive;

    public GizmoPlaneSlider3D(Gizmo gizmo, int handleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._quad = new QuadShape3D();
      this._raTriangle = new RightAngTriangle3D();
      this._circle = new CircleShape3D();
      this._isBorderHoverable = true;
      this._isBorderVisible = true;
      this._transform = new GizmoTransform();
      this._dragChannel = GizmoDragChannel.Offset;
      this._dblAxisOffsetDrag = new GizmoDblAxisOffsetDrag3D();
      this._rotationDrag = new GizmoSglAxisRotationDrag3D();
      this._rotationArc = new GizmoRotationArc3D();
      this._scaleDrag = new GizmoDblAxisScaleDrag3D();
      this._scaleDragAxisIndexUp = 1;
      this._controllerData = new GizmoPlaneSlider3DControllerData();
      this._controllers = new IGizmoPlaneSlider3DController[Enum.GetValues(typeof (GizmoPlane3DType)).Length];
      this._settings = new GizmoPlaneSlider3DSettings();
      this._lookAndFeel = new GizmoPlaneSlider3DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector(gizmo, handleId);
      this._quadIndex = this.Handle.Add3DShape((Shape3D) this._quad);
      this._raTriangleIndex = this.Handle.Add3DShape((Shape3D) this._raTriangle);
      this._circleIndex = this.Handle.Add3DShape((Shape3D) this._circle);
      this._quadBorder = new GizmoQuad3DBorder(this, this.Handle, this._quad);
      this._raTriangleBorder = new GizmoRATriangle3DBorder(this, this.Handle, this._raTriangle);
      this._circleBorder = new GizmoCircle3DBorder(this, this.Handle, this._circle);
      this._controllerData.Gizmo = this.Gizmo;
      this._controllerData.Slider = this;
      this._controllerData.SliderHandle = this.Handle;
      this._controllerData.QuadBorder = this._quadBorder;
      this._controllerData.Quad = this._quad;
      this._controllerData.QuadIndex = this._quadIndex;
      this._controllerData.RATriangleBorder = this._raTriangleBorder;
      this._controllerData.RATriangle = this._raTriangle;
      this._controllerData.RATriangleIndex = this._raTriangleIndex;
      this._controllerData.CircleBorder = this._circleBorder;
      this._controllerData.Circle = this._circle;
      this._controllerData.CircleIndex = this._circleIndex;
      this._controllers[0] = (IGizmoPlaneSlider3DController) new GizmoQuadPlaneSlider3DController(this._controllerData);
      this._controllers[1] = (IGizmoPlaneSlider3DController) new GizmoRATrianglePlaneSlider3DController(this._controllerData);
      this._controllers[2] = (IGizmoPlaneSlider3DController) new GizmoCirclePlaneSlider3DController(this._controllerData);
      this._transform.Changed += new GizmoEntityTransformChangedHandler(this.OnTransformChanged);
      this.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
      this.Gizmo.PreDragBeginAttempt += new GizmoPreDragBeginAttemptHandler(this.OnGizmoAttemptHandleDragBegin);
      this.Gizmo.PostEnabled += new GizmoPostEnabledHandler(this.OnGizmoPostEnabled);
      this.Handle.CanHover += new GizmoHandleCanHoverHandler(this.OnCanHoverHandle);
      this.SetDragChannel(GizmoDragChannel.Offset);
      this.AddTargetTransform(this._transform);
      this.AddTargetTransform(this.Gizmo.Transform);
      this._transform.SetParent(this.Gizmo.Transform);
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
      this._raTriangleBorder.SetHoverable(isHoverable);
      this._circleBorder.SetHoverable(isHoverable);
    }

    public override void SetSnapEnabled(bool isEnabled)
    {
      this._dblAxisOffsetDrag.IsSnapEnabled = isEnabled;
      this._rotationDrag.IsSnapEnabled = isEnabled;
      this._scaleDrag.IsSnapEnabled = isEnabled;
    }

    public void SetZoomFactorTransform(GizmoTransform transform)
    {
      this.Handle.SetZoomFactorTransform(transform);
    }

    public float GetZoomFactor(Camera camera)
    {
      return !this.LookAndFeel.UseZoomFactor ? 1f : this.Handle.GetZoomFactor(camera);
    }

    public float GetRealQuadWidth(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      float num = 1f;
      if (this._scaleDrag.IsActive)
        num = Mathf.Abs(this._scaleDrag.TotalScale0);
      return this.LookAndFeel.QuadWidth * this.LookAndFeel.Scale * zoomFactor * num;
    }

    public float GetRealQuadHeight(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      float num = 1f;
      if (this._scaleDrag.IsActive)
        num = Mathf.Abs(this._scaleDrag.TotalScale1);
      return this.LookAndFeel.QuadHeight * this.LookAndFeel.Scale * zoomFactor * num;
    }

    public Vector2 GetRealQuadSize(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      float num1 = 1f;
      float num2 = 1f;
      if (this._scaleDrag.IsActive)
      {
        num1 = this._scaleDrag.TotalScale0;
        num2 = this._scaleDrag.TotalScale1;
      }
      return new Vector2(this.LookAndFeel.QuadWidth * this.LookAndFeel.Scale * zoomFactor * num1, this.LookAndFeel.QuadHeight * this.LookAndFeel.Scale * zoomFactor * num2);
    }

    public float GetRealCircleRadius(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      float num = 1f;
      if (this.IsScaling)
      {
        num = this.TotalDragScaleRight;
        if ((double) Mathf.Abs(this.TotalDragScaleRight) < (double) Mathf.Abs(this.TotalDragScaleUp))
          num = this.TotalDragScaleUp;
      }
      return this.LookAndFeel.CircleRadius * this.LookAndFeel.Scale * zoomFactor * num;
    }

    public float GetRealRATriXLength(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      float num = 1f;
      if (this._scaleDrag.IsActive)
        num = Mathf.Abs(this._scaleDrag.TotalScale0);
      return this.LookAndFeel.RATriangleXLength * this.LookAndFeel.Scale * zoomFactor * num;
    }

    public float GetRealRATriYLength(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      float num = 1f;
      if (this._scaleDrag.IsActive)
        num = Mathf.Abs(this._scaleDrag.TotalScale1);
      return this.LookAndFeel.RATriangleYLength * this.LookAndFeel.Scale * zoomFactor * num;
    }

    public Vector2 GetRealRATriSize(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      float num1 = 1f;
      float num2 = 1f;
      if (this._scaleDrag.IsActive)
      {
        num1 = this._scaleDrag.TotalScale0;
        num2 = this._scaleDrag.TotalScale1;
      }
      return new Vector2(this.LookAndFeel.RATriangleXLength * this.LookAndFeel.Scale * zoomFactor * num1, this.LookAndFeel.RATriangleYLength * this.LookAndFeel.Scale * zoomFactor * num2);
    }

    public void AlignToQuadrant(
      GizmoTransform transform,
      PlaneId planeId,
      PlaneQuadrantId quadrantId,
      bool alignXToFirstAxis)
    {
      Plane plane3D = transform.GetPlane3D(planeId, quadrantId);
      if (alignXToFirstAxis)
      {
        Vector3 axis3D = this.Gizmo.Transform.GetAxis3D(PlaneIdHelper.GetSecondAxisDescriptor(planeId, quadrantId));
        this._transform.Rotation3D = Quaternion.LookRotation(plane3D.normal, axis3D);
      }
      else
      {
        Vector3 axis3D = this.Gizmo.Transform.GetAxis3D(PlaneIdHelper.GetFirstAxisDescriptor(planeId, quadrantId));
        this._transform.Rotation3D = Quaternion.LookRotation(plane3D.normal, axis3D);
      }
    }

    public void MakeSliderPlane(
      GizmoTransform sliderPlaneTransform,
      PlaneId planeId,
      GizmoLineSlider3D firstAxisSlider,
      GizmoLineSlider3D secondAxisSlider,
      Camera camera)
    {
      PlaneQuadrantId planeQuadrantId = sliderPlaneTransform.Get3DQuadrantFacingCamera(planeId, camera);
      this.AlignToQuadrant(sliderPlaneTransform, planeId, planeQuadrantId, true);
      Vector3 axis3D1 = sliderPlaneTransform.GetAxis3D(PlaneIdHelper.GetFirstAxisDescriptor(planeId, planeQuadrantId));
      Vector3 axis3D2 = sliderPlaneTransform.GetAxis3D(PlaneIdHelper.GetSecondAxisDescriptor(planeId, planeQuadrantId));
      Vector3 vector3 = axis3D1 * secondAxisSlider.GetRealSizeAlongDirection(camera, axis3D1) * 0.5f + axis3D2 * firstAxisSlider.GetRealSizeAlongDirection(camera, axis3D2) * 0.5f;
      if (this.LookAndFeel.PlaneType != GizmoPlane3DType.Quad)
        return;
      this.SetQuadCornerPosition(QuadCorner.BottomLeft, sliderPlaneTransform.Position3D + vector3);
    }

    public Vector3 GetQuadCornerPosition(QuadCorner corner) => this._quad.GetCornerPosition(corner);

    public void SetQuadCornerPosition(QuadCorner corner, Vector3 cornerPosition)
    {
      Vector3 vector3 = this.Position - this.GetQuadCornerPosition(corner);
      this.Position = cornerPosition + vector3;
    }

    public void ApplyZoomFactor(Camera camera)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        return;
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateTransforms(this.GetZoomFactor(camera));
    }

    public void SetDragChannel(GizmoDragChannel dragChannel)
    {
      this._dragChannel = dragChannel;
      if (this._dragChannel == GizmoDragChannel.Offset)
        this._selectedDragSession = (IGizmoDragSession) this._dblAxisOffsetDrag;
      else if (this._dragChannel == GizmoDragChannel.Rotation)
        this._selectedDragSession = (IGizmoDragSession) this._rotationDrag;
      else if (this._dragChannel == GizmoDragChannel.Scale)
        this._selectedDragSession = (IGizmoDragSession) this._scaleDrag;
      this.Handle.DragSession = this._selectedDragSession;
    }

    public void AddTargetTransform(GizmoTransform transform)
    {
      this._dblAxisOffsetDrag.AddTargetTransform(transform);
      this._rotationDrag.AddTargetTransform(transform);
      this._scaleDrag.AddTargetTransform(transform);
    }

    public void AddTargetTransform(GizmoTransform transform, GizmoDragChannel dragChannel)
    {
      if (dragChannel == GizmoDragChannel.Offset)
        this._dblAxisOffsetDrag.AddTargetTransform(transform);
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
      this._dblAxisOffsetDrag.RemoveTargetTransform(transform);
      this._rotationDrag.RemoveTargetTransform(transform);
      this._scaleDrag.RemoveTargetTransform(transform);
    }

    public void RemoveTargetTransform(GizmoTransform transform, GizmoDragChannel dragChannel)
    {
      if (dragChannel == GizmoDragChannel.Offset)
        this._dblAxisOffsetDrag.RemoveTargetTransform(transform);
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
      if (this.IsRotating && this.LookAndFeel.PlaneType == GizmoPlane3DType.Circle && this.LookAndFeel.IsRotationArcVisible)
      {
        this._rotationArc.RotationAngle = this.TotalDragRotation;
        this._rotationArc.Radius = this.GetRealCircleRadius(this.GetZoomFactor(camera));
        this._rotationArc.Render(this.LookAndFeel.RotationArcLookAndFeel);
      }
      if (this.IsVisible)
      {
        Color color1 = new Color();
        Color color2 = this.IsHovered ? this.LookAndFeel.HoveredColor : this.LookAndFeel.Color;
        GizmoSolidMaterial get = Singleton<GizmoSolidMaterial>.Get;
        get.ResetValuesToSensibleDefaults();
        get.SetCullModeOff();
        get.SetLit(this.LookAndFeel.ShadeMode == GizmoShadeMode.Lit);
        if (get.IsLit)
          get.SetLightDirection(camera.transform.forward);
        get.SetColor(color2);
        get.SetPass(0);
        if (this.LookAndFeel.PlaneType == GizmoPlane3DType.Quad)
          this.Handle.Render3DSolid(this._quadIndex);
        else if (this.LookAndFeel.PlaneType == GizmoPlane3DType.RATriangle)
          this.Handle.Render3DSolid(this._raTriangleIndex);
        else if (this.LookAndFeel.PlaneType == GizmoPlane3DType.Circle)
          this.Handle.Render3DSolid(this._circleIndex);
      }
      if (this.LookAndFeel.PlaneType == GizmoPlane3DType.Quad)
        this._quadBorder.Render(camera);
      else if (this.LookAndFeel.PlaneType == GizmoPlane3DType.RATriangle)
      {
        this._raTriangleBorder.Render(camera);
      }
      else
      {
        if (this.LookAndFeel.PlaneType != GizmoPlane3DType.Circle)
          return;
        this._circleBorder.Render(camera);
      }
    }

    public void Refresh()
    {
      float zoomFactor = this.GetZoomFactor(this.Gizmo.GetWorkCamera());
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateEpsilons(zoomFactor);
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateTransforms(zoomFactor);
    }

    protected override void OnVisibilityStateChanged()
    {
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateHandles();
      float zoomFactor = this.GetZoomFactor(this.Gizmo.GetWorkCamera());
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateEpsilons(zoomFactor);
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateTransforms(zoomFactor);
    }

    protected override void OnHoverableStateChanged()
    {
      this.Handle.Set3DShapeHoverable(this._quadIndex, this.IsHoverable);
      this.Handle.Set3DShapeHoverable(this._raTriangleIndex, this.IsHoverable);
      this.Handle.Set3DShapeHoverable(this._circleIndex, this.IsHoverable);
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      int planeType = (int) this.LookAndFeel.PlaneType;
      this._controllers[planeType].UpdateHandles();
      this._dblAxisOffsetDrag.Sensitivity = this.Settings.OffsetSensitivity;
      this._rotationDrag.Sensitivity = this.Settings.RotationSensitivity;
      this._scaleDrag.Sensitivity = this.Settings.ScaleSensitivity;
      float zoomFactor = this.GetZoomFactor(this.Gizmo.FocusCamera);
      this._controllers[planeType].UpdateTransforms(zoomFactor);
      this._controllers[planeType].UpdateEpsilons(zoomFactor);
    }

    private void OnTransformChanged(GizmoTransform transform, GizmoTransform.ChangeData changeData)
    {
      if (changeData.ChangeReason != GizmoTransform.ChangeReason.ParentChange && changeData.TRSDimension != GizmoDimension.Dim3D)
        return;
      this._controllers[(int) this.LookAndFeel.PlaneType].UpdateTransforms(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    private void OnGizmoAttemptHandleDragBegin(Gizmo gizmo, int handleId)
    {
      if (handleId != this.HandleId)
        return;
      if (this._dragChannel == GizmoDragChannel.Offset)
        this._dblAxisOffsetDrag.SetWorkData(new GizmoDblAxisOffsetDrag3D.WorkData()
        {
          Axis0 = this.Right,
          Axis1 = this.Up,
          DragOrigin = this.Position,
          SnapStep0 = this.Settings.OffsetSnapStepRight,
          SnapStep1 = this.Settings.OffsetSnapStepUp
        });
      else if (this._dragChannel == GizmoDragChannel.Rotation)
      {
        this._rotationDrag.SetWorkData(new GizmoSglAxisRotationDrag3D.WorkData()
        {
          Axis = this.Normal,
          RotationPlanePos = this.Position,
          SnapMode = this.Settings.RotationSnapMode,
          SnapStep = this.Settings.RotationSnapStep
        });
        this._rotationArc.SetArcData(this.Normal, this.Position, this.Plane.ProjectPoint(this.Gizmo.HoverInfo.HoverPoint), this.GetRealCircleRadius(this.GetZoomFactor(this.Gizmo.FocusCamera)));
      }
      else
      {
        if (this._dragChannel != GizmoDragChannel.Scale)
          return;
        this._scaleDrag.SetWorkData(new GizmoDblAxisScaleDrag3D.WorkData()
        {
          Axis0 = this.Right,
          Axis1 = this.Up,
          DragOrigin = this.Position,
          AxisIndex0 = this._scaleDragAxisIndexRight,
          AxisIndex1 = this._scaleDragAxisIndexUp,
          SnapStep = this.Settings.ProportionalScaleSnapStep
        });
      }
    }

    private void OnCanHoverHandle(
      int handleId,
      Gizmo gizmo,
      GizmoHandleHoverData hoverData,
      YesNoAnswer answer)
    {
      if (handleId == this.HandleId && gizmo == this.Gizmo && this.LookAndFeel.PlaneType == GizmoPlane3DType.Circle && this.Settings.IsCircleHoverCullEnabled)
      {
        Vector3 normalized = (hoverData.HoverPoint - this.Position).normalized;
        if (this.Gizmo.FocusCamera.IsPointFacingCamera(hoverData.HoverPoint, normalized))
          answer.Yes();
        else
          answer.No();
      }
      else
        answer.Yes();
    }

    private void OnGizmoPostEnabled(Gizmo gizmo) => this.Refresh();
  }
}
