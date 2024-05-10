// Decompiled with JetBrains decompiler
// Type: RTG.GizmoLineSlider2D
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
  public class GizmoLineSlider2D : GizmoSlider
  {
    private SegmentShape2D _segment;
    private QuadShape2D _quad;
    private int _segmentIndex;
    private int _quadIndex;
    private GizmoDragChannel _dragChannel;
    private GizmoSglAxisOffsetDrag3D _offsetDrag;
    private Vector3 _offsetDragOrigin;
    private GizmoSglAxisRotationDrag3D _rotationDrag;
    private GizmoRotationArc2D _rotationArc;
    private GizmoSglAxisScaleDrag3D _scaleDrag;
    private Vector3 _scaleDragOrigin;
    private Vector3 _scaleAxis;
    private int _scaleDragAxisIndex;
    private IGizmoDragSession _selectedDragSession;
    private GizmoCap2D _cap2D;
    private GizmoTransform _transform;
    private GizmoTransformAxisMap2D _directionAxisMap;
    private GizmoOverrideColor _overrideFillColor;
    private GizmoOverrideColor _overrideBorderColor;
    private GizmoLineSlider2DControllerData _controllerData;
    private IGizmoLineSlider2DController[] _controllers;
    private GizmoLineSlider2DSettings _settings;
    private GizmoLineSlider2DSettings _sharedSettings;
    private GizmoLineSlider2DLookAndFeel _lookAndFeel;
    private GizmoLineSlider2DLookAndFeel _sharedLookAndFeel;

    public Quaternion Rotation => this._transform.Rotation2D;

    public float RotationDegrees => this._transform.Rotation2DDegrees;

    public Vector2 StartPosition
    {
      get => this._transform.Position2D;
      set => this._transform.Position2D = value;
    }

    public Vector2 Direction => this._directionAxisMap.Axis;

    public Vector3 OffsetDragOrigin
    {
      get => this._offsetDragOrigin;
      set => this._offsetDragOrigin = value;
    }

    public Vector3 ScaleDragOrigin
    {
      get => this._scaleDragOrigin;
      set => this._scaleDragOrigin = value;
    }

    public int ScaleDragAxisIndex
    {
      get => this._scaleDragAxisIndex;
      set => this._scaleDragAxisIndex = Mathf.Clamp(value, 0, 2);
    }

    public int Cap2DHandleId => this._cap2D.HandleId;

    public bool IsDragged
    {
      get
      {
        if (!this.Gizmo.IsDragged)
          return false;
        return this.Gizmo.DragHandleId == this.HandleId || this.Gizmo.DragHandleId == this._cap2D.HandleId;
      }
    }

    public bool IsMoving => this._offsetDrag.IsActive;

    public bool IsRotating => this._rotationDrag.IsActive;

    public bool IsScaling => this._scaleDrag.IsActive;

    public bool Is2DCapVisible => this._cap2D.IsVisible;

    public bool Is2DCapHoverable => this._cap2D.IsHoverable;

    public Vector3 TotalDragOffset => this._offsetDrag.TotalDragOffset;

    public Vector3 RelativeDragOffset => this._offsetDrag.RelativeDragOffset;

    public float TotalDragRotation => this._rotationDrag.TotalRotation;

    public float RelativeDragRotation => this._rotationDrag.RelativeRotation;

    public float TotalDragScale => this._scaleDrag.TotalScale;

    public float RelativeDragScale => this._scaleDrag.RelativeScale;

    public GizmoOverrideColor OverrideFillColor => this._overrideFillColor;

    public GizmoOverrideColor OverrideBorderColor => this._overrideBorderColor;

    public GizmoLineSlider2DSettings Settings
    {
      get => this._sharedSettings == null ? this._settings : this._sharedSettings;
    }

    public GizmoLineSlider2DSettings SharedSettings
    {
      get => this._sharedSettings;
      set => this._sharedSettings = value;
    }

    public GizmoLineSlider2DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel == null ? this._lookAndFeel : this._sharedLookAndFeel;
    }

    public GizmoLineSlider2DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set
      {
        this._sharedLookAndFeel = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public GizmoLineSlider2D(Gizmo gizmo, int handleId, int capHandleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._segment = new SegmentShape2D();
      this._quad = new QuadShape2D();
      this._offsetDrag = new GizmoSglAxisOffsetDrag3D();
      this._rotationDrag = new GizmoSglAxisRotationDrag3D();
      this._rotationArc = new GizmoRotationArc2D();
      this._scaleDrag = new GizmoSglAxisScaleDrag3D();
      this._transform = new GizmoTransform();
      this._directionAxisMap = new GizmoTransformAxisMap2D();
      this._overrideFillColor = new GizmoOverrideColor();
      this._overrideBorderColor = new GizmoOverrideColor();
      this._controllerData = new GizmoLineSlider2DControllerData();
      this._controllers = new IGizmoLineSlider2DController[Enum.GetValues(typeof (GizmoLine2DType)).Length];
      this._settings = new GizmoLineSlider2DSettings();
      this._lookAndFeel = new GizmoLineSlider2DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector(gizmo, handleId);
      this._segmentIndex = this.Handle.Add2DShape((Shape2D) this._segment);
      this._quadIndex = this.Handle.Add2DShape((Shape2D) this._quad);
      this._controllerData.Gizmo = this.Gizmo;
      this._controllerData.Slider = this;
      this._controllerData.SliderHandle = this.Handle;
      this._controllerData.Segment = this._segment;
      this._controllerData.SegmentIndex = this._segmentIndex;
      this._controllerData.Quad = this._quad;
      this._controllerData.QuadIndex = this._quadIndex;
      this._controllers[0] = (IGizmoLineSlider2DController) new GizmoThinLineSlider2DController(this._controllerData);
      this._controllers[1] = (IGizmoLineSlider2DController) new GizmoBoxLineSlider2DController(this._controllerData);
      this._cap2D = new GizmoCap2D(gizmo, capHandleId);
      this.SetupSharedLookAndFeel();
      this.SetDragChannel(GizmoDragChannel.Offset);
      this.AddTargetTransform(this.Gizmo.Transform);
      this.AddTargetTransform(this._transform);
      this._cap2D.RegisterTransformAsDragTarget((IGizmoDragSession) this._offsetDrag);
      this._cap2D.RegisterTransformAsDragTarget((IGizmoDragSession) this._rotationDrag);
      this._transform.Changed += new GizmoEntityTransformChangedHandler(this.OnTransformChanged);
      this._transform.SetParent(gizmo.Transform);
      this.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
      this.Gizmo.PreDragUpdate += new GizmoPreDragUpdateHandler(this.OnGizmoHandleDragUpdate);
      this.Gizmo.PreDragBeginAttempt += new GizmoPreDragBeginAttemptHandler(this.OnGizmoAttemptHandleDragBegin);
      this.Gizmo.PreHoverEnter += new GizmoPreHoverEnterHandler(this.OnGizmoHandleHoverEnter);
      this.Gizmo.PreHoverExit += new GizmoPreHoverExitHandler(this.OnGizmoHandleHoverExit);
      this.Gizmo.PostEnabled += new GizmoPostEnabledHandler(this.OnGizmoPostEnabled);
    }

    public override void SetSnapEnabled(bool isEnabled)
    {
      this._offsetDrag.IsSnapEnabled = isEnabled;
      this._rotationDrag.IsSnapEnabled = isEnabled;
      this._scaleDrag.IsSnapEnabled = isEnabled;
    }

    public void Set2DCapVisible(bool isVisible) => this._cap2D.SetVisible(isVisible);

    public void Set2DCapHoverable(bool isHoverable) => this._cap2D.SetHoverable(isHoverable);

    public Vector2 GetRealDirection()
    {
      return this.Direction * (this.IsScaling ? Mathf.Sign(this.TotalDragScale) : 1f);
    }

    public float GetRealLength()
    {
      float num = 1f;
      if (this.IsScaling)
        num = Vector3Ex.ConvertDirTo2D(this.ScaleDragOrigin, this.ScaleDragOrigin + this._scaleAxis * this.TotalDragScale, this.Gizmo.GetWorkCamera()).magnitude / (this.LookAndFeel.Length * this.LookAndFeel.Scale) * Mathf.Sign(this.TotalDragScale);
      return this.LookAndFeel.Length * this.LookAndFeel.Scale * num;
    }

    public Vector2 GetRealEndPosition()
    {
      return this.StartPosition + this.Direction * this.GetRealLength();
    }

    public float GetRealBoxThickness() => this.LookAndFeel.BoxThickness * this.LookAndFeel.Scale;

    public void MapDirection(int axisIndex, AxisSign axisSign)
    {
      if (this.IsDragged || axisIndex > 1)
        return;
      this._directionAxisMap.Map(this._transform, axisIndex, axisSign);
    }

    public void SetDirection(Vector2 directionAxis)
    {
      if (this.IsDragged)
        return;
      this._directionAxisMap.SetAxis(directionAxis);
    }

    public void AddTargetTransform(GizmoTransform transform)
    {
      this._offsetDrag.AddTargetTransform(transform);
      this._rotationDrag.AddTargetTransform(transform);
      this._scaleDrag.AddTargetTransform(transform);
    }

    public void AddTargetTransform(GizmoTransform transform, GizmoDragChannel dragChannel)
    {
      switch (dragChannel)
      {
        case GizmoDragChannel.Offset:
          this._offsetDrag.AddTargetTransform(transform);
          break;
        case GizmoDragChannel.Rotation:
          this._rotationDrag.AddTargetTransform(transform);
          break;
        case GizmoDragChannel.Scale:
          this._scaleDrag.AddTargetTransform(transform);
          break;
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
      switch (dragChannel)
      {
        case GizmoDragChannel.Offset:
          this._offsetDrag.RemoveTargetTransform(transform);
          break;
        case GizmoDragChannel.Rotation:
          this._rotationDrag.RemoveTargetTransform(transform);
          break;
        case GizmoDragChannel.Scale:
          this._scaleDrag.RemoveTargetTransform(transform);
          break;
      }
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
      this._cap2D.DragSession = this._selectedDragSession;
    }

    public override void Render(Camera camera)
    {
      if (!this.IsVisible && !this.Is2DCapVisible)
        return;
      if (this.LookAndFeel.IsRotationArcVisible && this.IsRotating)
      {
        this._rotationArc.RotationAngle = this._rotationDrag.TotalRotation;
        this._rotationArc.Render(this.LookAndFeel.RotationArcLookAndFeel, camera);
      }
      if (this.IsVisible)
      {
        if (this.LookAndFeel.LineType == GizmoLine2DType.Thin || this.LookAndFeel.FillMode == GizmoFillMode2D.FilledAndBorder || this.LookAndFeel.FillMode == GizmoFillMode2D.Filled)
        {
          Color color1 = new Color();
          Color color2;
          if (!this._overrideFillColor.IsActive)
          {
            color2 = this.LookAndFeel.Color;
            if (this.Gizmo.HoverHandleId == this.HandleId)
              color2 = this.LookAndFeel.HoveredColor;
          }
          else
            color2 = this._overrideFillColor.Color;
          GizmoSolidMaterial get = Singleton<GizmoSolidMaterial>.Get;
          get.ResetValuesToSensibleDefaults();
          get.SetLit(false);
          get.SetColor(color2);
          get.SetPass(0);
          this.Handle.Render2DSolid(camera);
        }
        if (this.LookAndFeel.LineType != GizmoLine2DType.Thin && (this.LookAndFeel.FillMode == GizmoFillMode2D.FilledAndBorder || this.LookAndFeel.FillMode == GizmoFillMode2D.Border))
        {
          Color color3 = new Color();
          Color color4;
          if (!this._overrideFillColor.IsActive)
          {
            color4 = this.LookAndFeel.BorderColor;
            if (this.Gizmo.HoverHandleId == this.HandleId)
              color4 = this.LookAndFeel.HoveredBorderColor;
          }
          else
            color4 = this._overrideBorderColor.Color;
          GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
          get.ResetValuesToSensibleDefaults();
          get.SetColor(color4);
          get.SetPass(0);
          this.Handle.Render2DWire(camera);
        }
      }
      this._cap2D.Render(camera);
    }

    public void Refresh()
    {
      this._controllers[(int) this.LookAndFeel.LineType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.LineType].UpdateEpsilons();
      this._controllers[(int) this.LookAndFeel.LineType].UpdateTransforms();
      this._cap2D.CapSlider2D(this.GetRealDirection(), this.GetRealEndPosition());
    }

    protected override void OnVisibilityStateChanged()
    {
      this._controllers[(int) this.LookAndFeel.LineType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.LineType].UpdateEpsilons();
      this._controllers[(int) this.LookAndFeel.LineType].UpdateTransforms();
      this._cap2D.CapSlider2D(this.GetRealDirection(), this.GetRealEndPosition());
    }

    protected override void OnHoverableStateChanged() => this.Handle.SetHoverable(this.IsHoverable);

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      int lineType = (int) this.LookAndFeel.LineType;
      this._controllers[lineType].UpdateHandles();
      this._offsetDrag.Sensitivity = this.Settings.OffsetSensitivity;
      this._rotationDrag.Sensitivity = this.Settings.RotationSensitivity;
      this._controllers[lineType].UpdateTransforms();
      this._controllers[lineType].UpdateEpsilons();
      this._cap2D.GenericHoverPriority.Value = this.GenericHoverPriority.Value;
      this._cap2D.HoverPriority2D.Value = this.HoverPriority2D.Value;
      this._cap2D.HoverPriority3D.Value = this.HoverPriority3D.Value;
      this._cap2D.CapSlider2D(this.GetRealDirection(), this.GetRealEndPosition());
    }

    private void OnGizmoAttemptHandleDragBegin(Gizmo gizmo, int handleId)
    {
      if (handleId != this.Handle.Id && handleId != this._cap2D.HandleId)
        return;
      if (this._dragChannel == GizmoDragChannel.Offset)
        this._offsetDrag.SetWorkData(new GizmoSglAxisOffsetDrag3D.WorkData()
        {
          Axis = Vector2Ex.ConvertDirTo3D(this.StartPosition, this.GetRealEndPosition(), this.OffsetDragOrigin, this.Gizmo.FocusCamera).normalized,
          DragOrigin = this.OffsetDragOrigin,
          SnapStep = this.Settings.OffsetSnapStep
        });
      else if (this._dragChannel == GizmoDragChannel.Rotation)
      {
        GizmoSglAxisRotationDrag3D.WorkData workData = new GizmoSglAxisRotationDrag3D.WorkData();
        workData.Axis = this.Gizmo.FocusCamera.transform.forward;
        workData.SnapMode = this.Settings.RotationSnapMode;
        workData.SnapStep = this.Settings.RotationSnapStep;
        workData.RotationPlanePos = this.Gizmo.FocusCamera.ScreenToWorldPoint(new Vector3(this._transform.Position2D.x, this._transform.Position2D.y, this.Gizmo.FocusCamera.nearClipPlane));
        this._rotationArc.SetArcData(this.StartPosition, this.GetRealEndPosition(), this.GetRealLength());
        this._rotationDrag.SetWorkData(workData);
      }
      else
      {
        if (this._dragChannel != GizmoDragChannel.Scale)
          return;
        this._scaleAxis = Vector2Ex.ConvertDirTo3D(this.StartPosition, this.GetRealEndPosition(), this.ScaleDragOrigin, this.Gizmo.FocusCamera);
        this._scaleDrag.SetWorkData(new GizmoSglAxisScaleDrag3D.WorkData()
        {
          Axis = this._scaleAxis.normalized,
          AxisIndex = this._scaleDragAxisIndex,
          DragOrigin = this.ScaleDragOrigin,
          SnapStep = this.Settings.ScaleSnapStep,
          EntityScale = 1f
        });
      }
    }

    private void OnTransformChanged(GizmoTransform transform, GizmoTransform.ChangeData changeData)
    {
      if (changeData.TRSDimension != GizmoDimension.Dim2D && changeData.ChangeReason != GizmoTransform.ChangeReason.ParentChange)
        return;
      this._controllers[(int) this.LookAndFeel.LineType].UpdateTransforms();
      this._cap2D.CapSlider2D(this.GetRealDirection(), this.GetRealEndPosition());
    }

    private void OnGizmoHandleHoverEnter(Gizmo gizmo, int handleId)
    {
      if (handleId == this.HandleId)
      {
        this._cap2D.OverrideFillColor.IsActive = true;
        this._cap2D.OverrideFillColor.Color = this.LookAndFeel.CapLookAndFeel.HoveredColor;
        this._cap2D.OverrideBorderColor.IsActive = true;
        this._cap2D.OverrideBorderColor.Color = this.LookAndFeel.CapLookAndFeel.HoveredBorderColor;
      }
      else
      {
        if (handleId != this._cap2D.HandleId)
          return;
        this.OverrideFillColor.IsActive = true;
        this.OverrideFillColor.Color = this.LookAndFeel.HoveredColor;
        this.OverrideBorderColor.IsActive = true;
        this.OverrideBorderColor.Color = this.LookAndFeel.HoveredBorderColor;
      }
    }

    private void OnGizmoHandleHoverExit(Gizmo gizmo, int handleId)
    {
      if (handleId == this.HandleId)
      {
        this._cap2D.OverrideFillColor.IsActive = false;
        this._cap2D.OverrideBorderColor.IsActive = false;
      }
      else
      {
        if (handleId != this._cap2D.HandleId)
          return;
        this.OverrideFillColor.IsActive = false;
        this.OverrideBorderColor.IsActive = false;
      }
    }

    private void OnGizmoHandleDragUpdate(Gizmo gizmo, int handleId)
    {
      if (handleId != this.HandleId && handleId != this._cap2D.HandleId)
        return;
      this._transform.Rotate2D(gizmo.RelativeDragRotation);
    }

    private void SetupSharedLookAndFeel()
    {
      this._cap2D.SharedLookAndFeel = this.LookAndFeel.CapLookAndFeel;
    }

    private void OnGizmoPostEnabled(Gizmo gizmo) => this.Refresh();
  }
}
