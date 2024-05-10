// Decompiled with JetBrains decompiler
// Type: RTG.UniversalGizmo
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
  public class UniversalGizmo : GizmoBehaviour
  {
    private GizmoLineSlider3D _mvPXSlider;
    private GizmoLineSlider3D _mvPYSlider;
    private GizmoLineSlider3D _mvPZSlider;
    private GizmoLineSlider3D _mvNXSlider;
    private GizmoLineSlider3D _mvNYSlider;
    private GizmoLineSlider3D _mvNZSlider;
    private GizmoLineSlider3DCollection _mvAxesSliders;
    private GizmoPlaneSlider3D _mvXYSlider;
    private GizmoPlaneSlider3D _mvYZSlider;
    private GizmoPlaneSlider3D _mvZXSlider;
    private GizmoPlaneSlider3DCollection _mvDblSliders;
    private bool _isMvVertexSnapEnabled;
    private GizmoCap2D _mvVertSnapCap;
    private GizmoObjectVertexSnapDrag3D _mvVertexSnapDrag;
    private Vector3 _mvPostVSnapPosRestore;
    private GizmoLineSlider2D _mvP2DModeXSlider;
    private GizmoLineSlider2D _mvP2DModeYSlider;
    private GizmoLineSlider2D _mvN2DModeXSlider;
    private GizmoLineSlider2D _mvN2DModeYSlider;
    private GizmoLineSlider2DCollection _mv2DModeSliders;
    private GizmoPlaneSlider2D _mv2DModeDblSlider;
    private GizmoPlaneSlider3D _rtXSlider;
    private GizmoPlaneSlider3D _rtYSlider;
    private GizmoPlaneSlider3D _rtZSlider;
    private GizmoPlaneSlider3DCollection _rtAxesSliders;
    private GizmoCap3D _rtMidCap;
    private GizmoDblAxisRotationDrag3D _rtCamXYRotationDrag;
    private GizmoPlaneSlider2D _rtCamLookSlider;
    private GizmoCap3D _scMidCap;
    private GizmoUniformScaleDrag3D _scUnformScaleDrag;
    private GizmoScaleGuide _scScaleGuide;
    private IEnumerable<GameObject> _scScaleGuideTargetObjects;
    private bool _is2DModeEnabled;
    [SerializeField]
    private UniversalGizmoSettings2D _settings2D;
    private UniversalGizmoSettings2D _sharedSettings2D;
    [SerializeField]
    private UniversalGizmoSettings3D _settings3D;
    private UniversalGizmoSettings3D _sharedSettings3D;
    [SerializeField]
    private UniversalGizmoLookAndFeel2D _lookAndFeel2D;
    private UniversalGizmoLookAndFeel2D _sharedLookAndFeel2D;
    [SerializeField]
    private UniversalGizmoLookAndFeel3D _lookAndFeel3D;
    private UniversalGizmoLookAndFeel3D _sharedLookAndFeel3D;
    [SerializeField]
    private UniversalGizmoHotkeys _hotkeys;
    private UniversalGizmoHotkeys _sharedHotkeys;
    [SerializeField]
    private bool _useSnapEnableHotkey;
    [SerializeField]
    private bool _useVertSnapEnableHotkey;
    [SerializeField]
    private bool _use2DModeEnableHotkey;

    public UniversalGizmoSettings2D Settings2D
    {
      get => this._sharedSettings2D != null ? this._sharedSettings2D : this._settings2D;
    }

    public UniversalGizmoSettings2D SharedSettings2D
    {
      get => this._sharedSettings2D;
      set
      {
        this._sharedSettings2D = value;
        this.SetupSharedSettings();
      }
    }

    public UniversalGizmoSettings3D Settings3D
    {
      get => this._sharedSettings3D != null ? this._sharedSettings3D : this._settings3D;
    }

    public UniversalGizmoSettings3D SharedSettings3D
    {
      get => this._sharedSettings3D;
      set
      {
        this._sharedSettings3D = value;
        this.SetupSharedSettings();
      }
    }

    public UniversalGizmoLookAndFeel2D LookAndFeel2D
    {
      get => this._sharedLookAndFeel2D != null ? this._sharedLookAndFeel2D : this._lookAndFeel2D;
    }

    public UniversalGizmoLookAndFeel2D SharedLookAndFeel2D
    {
      get => this._sharedLookAndFeel2D;
      set
      {
        this._sharedLookAndFeel2D = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public UniversalGizmoLookAndFeel3D LookAndFeel3D
    {
      get => this._sharedLookAndFeel3D != null ? this._sharedLookAndFeel3D : this._lookAndFeel3D;
    }

    public UniversalGizmoLookAndFeel3D SharedLookAndFeel3D
    {
      get => this._sharedLookAndFeel3D;
      set
      {
        this._sharedLookAndFeel3D = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public UniversalGizmoHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public UniversalGizmoHotkeys SharedHotkeys
    {
      get => this._sharedHotkeys;
      set => this._sharedHotkeys = value;
    }

    public bool UseSnapEnableHotkey
    {
      get => this._useSnapEnableHotkey;
      set => this._useSnapEnableHotkey = value;
    }

    public bool UseVertSnapEnableHotkey
    {
      get => this._useVertSnapEnableHotkey;
      set => this._useVertSnapEnableHotkey = value;
    }

    public bool Use2DModeEnableHotkey
    {
      get => this._use2DModeEnableHotkey;
      set => this._use2DModeEnableHotkey = value;
    }

    public UniversalGizmo.MvVertexSnapState GetMvVertexSnapState()
    {
      if (!this._isMvVertexSnapEnabled)
        return UniversalGizmo.MvVertexSnapState.Inactive;
      return this._mvVertexSnapDrag.IsActive ? UniversalGizmo.MvVertexSnapState.Snapping : UniversalGizmo.MvVertexSnapState.SelectingPivot;
    }

    public float GetMvZoomFactor(Vector3 position)
    {
      return !this.LookAndFeel3D.MvUseZoomFactor ? 1f : this.Gizmo.GetWorkCamera().EstimateZoomFactor(position);
    }

    public float GetMvZoomFactor(Vector3 position, Camera camera)
    {
      return !this.LookAndFeel3D.MvUseZoomFactor ? 1f : camera.EstimateZoomFactor(position);
    }

    public float GetRtZoomFactor(Vector3 position)
    {
      return !this.LookAndFeel3D.RtUseZoomFactor ? 1f : this.Gizmo.GetWorkCamera().EstimateZoomFactor(position);
    }

    public float GetRtZoomFactor(Vector3 position, Camera camera)
    {
      return !this.LookAndFeel3D.RtUseZoomFactor ? 1f : camera.EstimateZoomFactor(position);
    }

    public float GetScZoomFactor(Vector3 position)
    {
      return !this.LookAndFeel3D.ScUseZoomFactor ? 1f : this.Gizmo.GetWorkCamera().EstimateZoomFactor(position);
    }

    public float GetScZoomFactor(Vector3 position, Camera camera)
    {
      return !this.LookAndFeel3D.ScUseZoomFactor ? 1f : camera.EstimateZoomFactor(position);
    }

    public bool IsDraggingMoveHandle()
    {
      return this.Gizmo.IsDragged && this.IsMoveHandle(this.Gizmo.DragHandleId);
    }

    public bool IsDraggingRotationHandle()
    {
      return this.Gizmo.IsDragged && this.IsRotationHandle(this.Gizmo.DragHandleId);
    }

    public bool IsDraggingScaleHandle()
    {
      return this.Gizmo.IsDragged && this.IsScaleHandle(this.Gizmo.DragHandleId);
    }

    public bool IsMoveHandle(int handleId)
    {
      return this._mvAxesSliders.Contains(handleId) || this._mvAxesSliders.ContainsCapId(handleId) || this._mvDblSliders.Contains(handleId) || this._mv2DModeSliders.Contains(handleId) || this._mv2DModeSliders.ContainsCapId(handleId) || this._mv2DModeDblSlider.HandleId == handleId;
    }

    public bool IsRotationHandle(int handleId)
    {
      return this._rtAxesSliders.Contains(handleId) || this._rtMidCap.HandleId == handleId || this._rtCamLookSlider.HandleId == handleId;
    }

    public bool IsScaleHandle(int handleId) => this._scMidCap.HandleId == handleId;

    public bool OwnsHandle(int handleId)
    {
      return this.IsMoveHandle(handleId) || this.IsRotationHandle(handleId) || this.IsScaleHandle(handleId);
    }

    public void SetSnapEnabled(bool isEnabled)
    {
      this._mvAxesSliders.SetSnapEnabled(isEnabled);
      this._mv2DModeSliders.SetSnapEnabled(isEnabled);
      this._mvDblSliders.SetSnapEnabled(isEnabled);
      this._mv2DModeDblSlider.SetSnapEnabled(isEnabled);
      this._rtAxesSliders.SetSnapEnabled(isEnabled);
      this._rtCamXYRotationDrag.IsSnapEnabled = isEnabled;
      this._rtCamLookSlider.SetSnapEnabled(isEnabled);
      this._scUnformScaleDrag.IsSnapEnabled = isEnabled;
    }

    public void SetMvVertexSnapEnabled(bool isEnabled)
    {
      if (this._isMvVertexSnapEnabled == isEnabled || this._is2DModeEnabled || !this._isEnabled || this.Gizmo.IsDragged)
        return;
      if (isEnabled)
      {
        this._mvVertSnapCap.SetVisible(true);
        this._mvDblSliders.SetVisible(false, true);
        this.SetRotationHandlesVisible(false);
        this.SetScaleHandlesVisible(false);
      }
      else
      {
        this._mvVertSnapCap.SetVisible(false);
        this.SetScaleHandlesVisible(true);
      }
      this._isMvVertexSnapEnabled = isEnabled;
    }

    public void Set2DModeEnabled(bool isEnabled)
    {
      if (this._is2DModeEnabled == isEnabled || this._isMvVertexSnapEnabled || !this._isEnabled || this.Gizmo.IsDragged)
        return;
      if (isEnabled)
      {
        this._mv2DModeSliders.SetVisible(true);
        this._mv2DModeSliders.Set2DCapsVisible(true);
        this._mv2DModeDblSlider.SetVisible(true);
        this._mv2DModeDblSlider.SetBorderVisible(true);
        this._mv2DModeSliders.SetOffsetDragOrigin(this.Gizmo.Transform.Position3D);
        this._mv2DModeDblSlider.OffsetDragOrigin = this.Gizmo.Transform.Position3D;
        this.SetMoveHandlesVisible(false);
        this.SetRotationHandlesVisible(false);
        this.SetScaleHandlesVisible(false);
        this.Update2DGizmoPosition();
        this.Update2DModeHandlePositions();
      }
      else
      {
        this.Hide2DModeHandles();
        this.SetScaleHandlesVisible(true);
      }
      this._is2DModeEnabled = isEnabled;
    }

    public void SetMvVertexSnapTargetObjects(IEnumerable<GameObject> targetObjects)
    {
      this._mvVertexSnapDrag.SetTargetObjects(targetObjects);
    }

    public void SetMvAxesLinesHoverable(bool hoverable)
    {
      this._mvPXSlider.SetHoverable(hoverable);
      this._mvNXSlider.SetHoverable(hoverable);
      this._mvPYSlider.SetHoverable(hoverable);
      this._mvNYSlider.SetHoverable(hoverable);
      this._mvPZSlider.SetHoverable(hoverable);
      this._mvNZSlider.SetHoverable(hoverable);
    }

    public void SetRtMidCapHoverable(bool hoverable) => this._rtMidCap.SetHoverable(hoverable);

    public void SetScaleGuideTargetObjects(IEnumerable<GameObject> targetObjects)
    {
      this._scScaleGuideTargetObjects = targetObjects;
    }

    public override void OnAttached()
    {
      this._mvXYSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.XYDblSlider);
      this._mvYZSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.YZDblSlider);
      this._mvZXSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.ZXDblSlider);
      this._mvDblSliders.Add(this._mvXYSlider);
      this._mvDblSliders.Add(this._mvYZSlider);
      this._mvDblSliders.Add(this._mvZXSlider);
      this._mvPXSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PXSlider, GizmoHandleId.PXCap);
      this._mvPXSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvPXSlider.MapDirection(0, AxisSign.Positive);
      this._mvNXSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NXSlider, GizmoHandleId.NXCap);
      this._mvNXSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvNXSlider.MapDirection(0, AxisSign.Negative);
      this._mvPYSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PYSlider, GizmoHandleId.PYCap);
      this._mvPYSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvPYSlider.MapDirection(1, AxisSign.Positive);
      this._mvNYSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NYSlider, GizmoHandleId.NYCap);
      this._mvNYSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvNYSlider.MapDirection(1, AxisSign.Negative);
      this._mvPZSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PZSlider, GizmoHandleId.PZCap);
      this._mvPZSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvPZSlider.MapDirection(2, AxisSign.Positive);
      this._mvNZSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NZSlider, GizmoHandleId.NZCap);
      this._mvNZSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvNZSlider.MapDirection(2, AxisSign.Negative);
      this._mvAxesSliders.Add(this._mvPXSlider);
      this._mvAxesSliders.Add(this._mvPYSlider);
      this._mvAxesSliders.Add(this._mvPZSlider);
      this._mvAxesSliders.Add(this._mvNXSlider);
      this._mvAxesSliders.Add(this._mvNYSlider);
      this._mvAxesSliders.Add(this._mvNZSlider);
      this._mvAxesSliders.Make3DHoverPriorityLowerThan(this._mvXYSlider.HoverPriority3D);
      this._mvAxesSliders.Make3DHoverPriorityLowerThan(this._mvYZSlider.HoverPriority3D);
      this._mvAxesSliders.Make3DHoverPriorityLowerThan(this._mvZXSlider.HoverPriority3D);
      this._mvVertSnapCap = new GizmoCap2D(this.Gizmo, GizmoHandleId.VertSnap);
      this._mvVertSnapCap.SetVisible(false);
      this._mvVertSnapCap.DragSession = (IGizmoDragSession) this._mvVertexSnapDrag;
      this._mvVertexSnapDrag.AddTargetTransform(this.Gizmo.Transform);
      this._mv2DModeDblSlider = new GizmoPlaneSlider2D(this.Gizmo, GizmoHandleId.CamXYSlider);
      this._mv2DModeDblSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mv2DModeDblSlider.SetVisible(false);
      this._mvP2DModeXSlider = new GizmoLineSlider2D(this.Gizmo, GizmoHandleId.PCamXSlider, GizmoHandleId.PCamXCap);
      this._mvP2DModeXSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvP2DModeXSlider.MapDirection(0, AxisSign.Positive);
      this._mvP2DModeXSlider.HoverPriority2D.MakeLowerThan(this._mv2DModeDblSlider.HoverPriority2D);
      this._mvP2DModeYSlider = new GizmoLineSlider2D(this.Gizmo, GizmoHandleId.PCamYSlider, GizmoHandleId.PCamYCap);
      this._mvP2DModeYSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvP2DModeYSlider.MapDirection(1, AxisSign.Positive);
      this._mvP2DModeYSlider.HoverPriority2D.MakeLowerThan(this._mv2DModeDblSlider.HoverPriority2D);
      this._mvN2DModeXSlider = new GizmoLineSlider2D(this.Gizmo, GizmoHandleId.NCamXSlider, GizmoHandleId.NCamXCap);
      this._mvN2DModeXSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvN2DModeXSlider.MapDirection(0, AxisSign.Negative);
      this._mvN2DModeXSlider.HoverPriority2D.MakeLowerThan(this._mv2DModeDblSlider.HoverPriority2D);
      this._mvN2DModeYSlider = new GizmoLineSlider2D(this.Gizmo, GizmoHandleId.NCamYSlider, GizmoHandleId.NCamYCap);
      this._mvN2DModeYSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._mvN2DModeYSlider.MapDirection(1, AxisSign.Negative);
      this._mvN2DModeYSlider.HoverPriority2D.MakeLowerThan(this._mv2DModeDblSlider.HoverPriority2D);
      this._mv2DModeSliders.Add(this._mvP2DModeXSlider);
      this._mv2DModeSliders.Add(this._mvP2DModeYSlider);
      this._mv2DModeSliders.Add(this._mvN2DModeXSlider);
      this._mv2DModeSliders.Add(this._mvN2DModeYSlider);
      this.Hide2DModeHandles();
      this._rtMidCap = new GizmoCap3D(this.Gizmo, GizmoHandleId.CamXYRotation);
      this._rtMidCap.DragSession = (IGizmoDragSession) this._rtCamXYRotationDrag;
      this._rtCamXYRotationDrag.AddTargetTransform(this.Gizmo.Transform);
      this._rtXSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.XRotationSlider);
      this._rtXSlider.SetDragChannel(GizmoDragChannel.Rotation);
      this._rtXSlider.LocalRotation = Quaternion.Euler(0.0f, 90f, 0.0f);
      this._rtXSlider.SetVisible(false);
      this._rtAxesSliders.Add(this._rtXSlider);
      this._rtYSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.YRotationSlider);
      this._rtYSlider.SetDragChannel(GizmoDragChannel.Rotation);
      this._rtYSlider.LocalRotation = Quaternion.Euler(90f, 0.0f, 0.0f);
      this._rtYSlider.SetVisible(false);
      this._rtAxesSliders.Add(this._rtYSlider);
      this._rtZSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.ZRotationSlider);
      this._rtZSlider.SetDragChannel(GizmoDragChannel.Rotation);
      this._rtZSlider.SetVisible(false);
      this._rtAxesSliders.Add(this._rtZSlider);
      this._rtCamLookSlider = new GizmoPlaneSlider2D(this.Gizmo, GizmoHandleId.CamZRotation);
      this._rtCamLookSlider.SetDragChannel(GizmoDragChannel.Rotation);
      this._rtCamLookSlider.SetVisible(false);
      this._scMidCap = new GizmoCap3D(this.Gizmo, GizmoHandleId.MidScaleCap);
      this._scMidCap.DragSession = (IGizmoDragSession) this._scUnformScaleDrag;
      this._rtAxesSliders.Make3DHoverPriorityHigherThan(this._rtMidCap.HoverPriority3D);
      this._mvAxesSliders.Make3DHoverPriorityHigherThan(this._rtXSlider.HoverPriority3D);
      this._mvDblSliders.Make3DHoverPriorityHigherThan(this._mvPXSlider.HoverPriority3D);
      this._scMidCap.HoverPriority3D.MakeHigherThan(this._mvXYSlider.HoverPriority3D);
      this.SetupSharedLookAndFeel();
      this.SetupSharedSettings();
    }

    public override void OnDetached()
    {
      this.Gizmo.Transform.Changed -= new GizmoEntityTransformChangedHandler(this.OnGizmoTransformChanged);
    }

    public override void OnEnabled()
    {
      this.Gizmo.Transform.Changed += new GizmoEntityTransformChangedHandler(this.OnGizmoTransformChanged);
    }

    public override void OnDisabled()
    {
      this.Gizmo.Transform.Changed -= new GizmoEntityTransformChangedHandler(this.OnGizmoTransformChanged);
    }

    public override void OnGizmoEnabled() => this.OnGizmoUpdateBegin();

    public override void OnGizmoUpdateBegin()
    {
      if (this.UseSnapEnableHotkey)
        this.SetSnapEnabled(this.Hotkeys.EnableSnapping.IsActive());
      if (this.Use2DModeEnableHotkey)
        this.Set2DModeEnabled(this.Hotkeys.Enable2DMode.IsActive());
      this.Update2DGizmoPosition();
      if (!this._is2DModeEnabled)
      {
        bool vertexSnapEnabled = this._isMvVertexSnapEnabled;
        if (!vertexSnapEnabled)
          this._mvPostVSnapPosRestore = this.Gizmo.Transform.Position3D;
        if (this.UseVertSnapEnableHotkey)
          this.SetMvVertexSnapEnabled(this.Hotkeys.EnableVertexSnapping.IsActive());
        if (vertexSnapEnabled && !this._isMvVertexSnapEnabled)
          this.Gizmo.Transform.Position3D = this._mvPostVSnapPosRestore;
        if (!this.Gizmo.IsDragged || this.IsDraggingMoveHandle())
        {
          this._mvPXSlider.SetVisible(this.LookAndFeel3D.IsMvPositiveSliderVisible(0));
          this._mvPXSlider.Set3DCapVisible(this.LookAndFeel3D.IsMvPositiveSliderCapVisible(0));
          this._mvPYSlider.SetVisible(this.LookAndFeel3D.IsMvPositiveSliderVisible(1));
          this._mvPYSlider.Set3DCapVisible(this.LookAndFeel3D.IsMvPositiveSliderCapVisible(1));
          this._mvPZSlider.SetVisible(this.LookAndFeel3D.IsMvPositiveSliderVisible(2));
          this._mvPZSlider.Set3DCapVisible(this.LookAndFeel3D.IsMvPositiveSliderCapVisible(2));
          this._mvNXSlider.SetVisible(this.LookAndFeel3D.IsMvNegativeSliderVisible(0));
          this._mvNXSlider.Set3DCapVisible(this.LookAndFeel3D.IsMvNegativeSliderCapVisible(0));
          this._mvNYSlider.SetVisible(this.LookAndFeel3D.IsMvNegativeSliderVisible(1));
          this._mvNYSlider.Set3DCapVisible(this.LookAndFeel3D.IsMvNegativeSliderCapVisible(1));
          this._mvNZSlider.SetVisible(this.LookAndFeel3D.IsMvNegativeSliderVisible(2));
          this._mvNZSlider.Set3DCapVisible(this.LookAndFeel3D.IsMvNegativeSliderCapVisible(2));
        }
      }
      if (!this._isMvVertexSnapEnabled && !this._is2DModeEnabled)
      {
        if (!this.Gizmo.IsDragged || this.IsDraggingMoveHandle())
        {
          this._mvXYSlider.SetVisible(this.LookAndFeel3D.IsMvDblSliderVisible(PlaneId.XY));
          this._mvXYSlider.SetBorderVisible(this._mvXYSlider.IsVisible);
          this._mvYZSlider.SetVisible(this.LookAndFeel3D.IsMvDblSliderVisible(PlaneId.YZ));
          this._mvYZSlider.SetBorderVisible(this._mvYZSlider.IsVisible);
          this._mvZXSlider.SetVisible(this.LookAndFeel3D.IsMvDblSliderVisible(PlaneId.ZX));
          this._mvZXSlider.SetBorderVisible(this._mvZXSlider.IsVisible);
          this.PlaceMvDblSlidersInSliderPlanes(this.Gizmo.FocusCamera);
        }
      }
      else if (this._isMvVertexSnapEnabled)
      {
        if (this.GetMvVertexSnapState() == UniversalGizmo.MvVertexSnapState.SelectingPivot && this._mvVertexSnapDrag.SelectSnapPivotPoint(this.Gizmo))
          this.Gizmo.Transform.Position3D = this._mvVertexSnapDrag.SnapPivot;
      }
      else if (this._is2DModeEnabled && (!this.Gizmo.IsDragged || this.IsDraggingMoveHandle()))
      {
        this._mvP2DModeXSlider.SetVisible(this.LookAndFeel2D.IsMvPositiveSliderVisible(0));
        this._mvP2DModeXSlider.Set2DCapVisible(this.LookAndFeel2D.IsMvPositiveSliderCapVisible(0));
        this._mvP2DModeYSlider.SetVisible(this.LookAndFeel2D.IsMvPositiveSliderVisible(1));
        this._mvP2DModeYSlider.Set2DCapVisible(this.LookAndFeel2D.IsMvPositiveSliderCapVisible(1));
        this._mvN2DModeXSlider.SetVisible(this.LookAndFeel2D.IsMvNegativeSliderVisible(0));
        this._mvN2DModeXSlider.Set2DCapVisible(this.LookAndFeel2D.IsMvNegativeSliderCapVisible(0));
        this._mvN2DModeYSlider.SetVisible(this.LookAndFeel2D.IsMvNegativeSliderVisible(1));
        this._mvN2DModeYSlider.Set2DCapVisible(this.LookAndFeel2D.IsMvNegativeSliderCapVisible(1));
        bool isVisible = this._mv2DModeDblSlider.IsVisible;
        this._mv2DModeDblSlider.SetVisible(this.LookAndFeel2D.IsMvDblSliderVisible);
        this._mv2DModeDblSlider.SetBorderVisible(this.LookAndFeel2D.IsMvDblSliderVisible);
        if (!isVisible && this._mv2DModeDblSlider.IsVisible)
          this.Update2DModeHandlePositions();
      }
      if (!this._is2DModeEnabled && !this._isMvVertexSnapEnabled && (!this.Gizmo.IsDragged || this.IsDraggingRotationHandle()))
      {
        this._rtMidCap.SetVisible(this.LookAndFeel3D.IsRtMidCapVisible);
        this._rtCamXYRotationDrag.Sensitivity = this.Settings3D.RtDragSensitivity;
        this._rtXSlider.SetBorderVisible(this.LookAndFeel3D.IsRtAxisVisible(0));
        this._rtYSlider.SetBorderVisible(this.LookAndFeel3D.IsRtAxisVisible(1));
        this._rtZSlider.SetBorderVisible(this.LookAndFeel3D.IsRtAxisVisible(2));
        this._rtCamLookSlider.SetBorderVisible(this.LookAndFeel3D.IsRtCamLookSliderVisible);
        if (this._rtCamLookSlider.IsBorderVisible)
          this.UpdateRtCamLookSlider(this.Gizmo.FocusCamera);
      }
      this._scMidCap.SetVisible(this.LookAndFeel3D.IsScMidCapVisible && !this._is2DModeEnabled);
      this._scUnformScaleDrag.Sensitivity = this.Settings3D.ScDragSensitivity;
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
      {
        this._mvAxesSliders.ApplyZoomFactor(camera);
        if (!this._isMvVertexSnapEnabled && !this._is2DModeEnabled)
        {
          this._mvDblSliders.ApplyZoomFactor(camera);
          this.PlaceMvDblSlidersInSliderPlanes(camera);
        }
        this.Update2DGizmoPosition();
        if (this._is2DModeEnabled)
          this.Update2DModeHandlePositions();
        this._rtMidCap.ApplyZoomFactor(camera);
        this._rtAxesSliders.ApplyZoomFactor(camera);
        if (this._rtCamLookSlider.IsBorderVisible)
          this.UpdateRtCamLookSlider(camera);
        this._scMidCap.ApplyZoomFactor(camera);
      }
      this._rtXSlider.Render(camera);
      this._rtYSlider.Render(camera);
      this._rtZSlider.Render(camera);
      this._rtMidCap.Render(camera);
      foreach (GizmoSlider renderSortedSlider in this._mvAxesSliders.GetRenderSortedSliders(camera))
        renderSortedSlider.Render(camera);
      this._rtCamLookSlider.Render(camera);
      this._mvXYSlider.Render(camera);
      this._mvYZSlider.Render(camera);
      this._mvZXSlider.Render(camera);
      this._scMidCap.Render(camera);
      this._mvVertSnapCap.Render(camera);
      this._mv2DModeSliders.Render(camera);
      this._mv2DModeDblSlider.Render(camera);
      if (!this.LookAndFeel3D.IsScScaleGuideVisible || !this.Gizmo.IsDragged || !this.IsScaleHandle(this.Gizmo.DragHandleId))
        return;
      this._scScaleGuide.Render((IEnumerable<GameObject>) GameObjectEx.FilterParentsOnly(this._scScaleGuideTargetObjects), camera);
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (!this._isMvVertexSnapEnabled)
        return;
      this._mvPostVSnapPosRestore += this.Gizmo.RelativeDragOffset;
    }

    public override void OnGizmoDragBegin(int handleId)
    {
      if (this.IsMoveHandle(handleId))
      {
        this.SetRotationHandlesVisible(false);
        this.SetScaleHandlesVisible(false);
      }
      else if (this.IsRotationHandle(handleId))
      {
        this.SetMoveHandlesVisible(false);
        this.SetScaleHandlesVisible(false);
      }
      else
      {
        if (!this.IsScaleHandle(handleId))
          return;
        this.SetMoveHandlesVisible(false);
        this.SetRotationHandlesVisible(false);
      }
    }

    public override void OnGizmoDragEnd(int handleId)
    {
      if (this.IsScaleHandle(handleId) || this._is2DModeEnabled)
        return;
      this.SetScaleHandlesVisible(true);
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      if (handleId == this._rtMidCap.HandleId)
      {
        this._rtCamXYRotationDrag.SetWorkData(new GizmoDblAxisRotationDrag3D.WorkData()
        {
          Axis0 = this.Gizmo.FocusCamera.transform.up,
          Axis1 = this.Gizmo.FocusCamera.transform.right,
          ScreenAxis0 = (Vector2) -Vector3.right,
          ScreenAxis1 = (Vector2) Vector3.up,
          SnapMode = this.Settings3D.RtSnapMode,
          SnapStep0 = this.Settings3D.RtCamUpSnapStep,
          SnapStep1 = this.Settings3D.RtCamRightSnapStep
        });
      }
      else
      {
        if (handleId != this._scMidCap.HandleId)
          return;
        this._scUnformScaleDrag.SetWorkData(new GizmoUniformScaleDrag3D.WorkData()
        {
          DragOrigin = this._scMidCap.Position,
          CameraRight = this.Gizmo.FocusCamera.transform.right,
          CameraUp = this.Gizmo.FocusCamera.transform.up,
          SnapStep = this.Settings3D.ScUniformSnapStep
        });
      }
    }

    private void PlaceMvDblSlidersInSliderPlanes(Camera camera)
    {
      if (this._mvXYSlider.IsVisible)
        this._mvXYSlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.XY, this._mvPXSlider, this._mvPYSlider, camera);
      if (this._mvYZSlider.IsVisible)
        this._mvYZSlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.YZ, this._mvPYSlider, this._mvPZSlider, camera);
      if (!this._mvZXSlider.IsVisible)
        return;
      this._mvZXSlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.ZX, this._mvPZSlider, this._mvPXSlider, camera);
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel3D.ConnectMvSliderLookAndFeel(this._mvPXSlider, 0, AxisSign.Positive);
      this.LookAndFeel3D.ConnectMvSliderLookAndFeel(this._mvPYSlider, 1, AxisSign.Positive);
      this.LookAndFeel3D.ConnectMvSliderLookAndFeel(this._mvPZSlider, 2, AxisSign.Positive);
      this.LookAndFeel3D.ConnectMvSliderLookAndFeel(this._mvNXSlider, 0, AxisSign.Negative);
      this.LookAndFeel3D.ConnectMvSliderLookAndFeel(this._mvNYSlider, 1, AxisSign.Negative);
      this.LookAndFeel3D.ConnectMvSliderLookAndFeel(this._mvNZSlider, 2, AxisSign.Negative);
      this.LookAndFeel3D.ConnectMvDblSliderLookAndFeel(this._mvXYSlider, PlaneId.XY);
      this.LookAndFeel3D.ConnectMvDblSliderLookAndFeel(this._mvYZSlider, PlaneId.YZ);
      this.LookAndFeel3D.ConnectMvDblSliderLookAndFeel(this._mvZXSlider, PlaneId.ZX);
      this.LookAndFeel3D.ConnectMvVertSnapCapLookAndFeel(this._mvVertSnapCap);
      this.LookAndFeel2D.ConnectMvSliderLookAndFeel(this._mvP2DModeXSlider, 0, AxisSign.Positive);
      this.LookAndFeel2D.ConnectMvSliderLookAndFeel(this._mvP2DModeYSlider, 1, AxisSign.Positive);
      this.LookAndFeel2D.ConnectMvSliderLookAndFeel(this._mvN2DModeXSlider, 0, AxisSign.Negative);
      this.LookAndFeel2D.ConnectMvSliderLookAndFeel(this._mvN2DModeYSlider, 1, AxisSign.Negative);
      this.LookAndFeel2D.ConnectMvDblSliderLookAndFeel(this._mv2DModeDblSlider);
      this.LookAndFeel3D.ConnectRtSliderLookAndFeel(this._rtXSlider, 0);
      this.LookAndFeel3D.ConnectRtSliderLookAndFeel(this._rtYSlider, 1);
      this.LookAndFeel3D.ConnectRtSliderLookAndFeel(this._rtZSlider, 2);
      this.LookAndFeel3D.ConnectRtCamLookSliderLookAndFeel(this._rtCamLookSlider);
      this.LookAndFeel3D.ConnectRtMidCapLookAndFeel(this._rtMidCap);
      this.LookAndFeel3D.ConnectScMidCapLookAndFeel(this._scMidCap);
      this.LookAndFeel3D.ConnectScGizmoScaleGuideLookAndFeel(this._scScaleGuide);
    }

    private void SetupSharedSettings()
    {
      this.Settings3D.ConnectMvSliderSettings(this._mvPXSlider, 0, AxisSign.Positive);
      this.Settings3D.ConnectMvSliderSettings(this._mvPYSlider, 1, AxisSign.Positive);
      this.Settings3D.ConnectMvSliderSettings(this._mvPZSlider, 2, AxisSign.Positive);
      this.Settings3D.ConnectMvSliderSettings(this._mvNXSlider, 0, AxisSign.Negative);
      this.Settings3D.ConnectMvSliderSettings(this._mvNYSlider, 1, AxisSign.Negative);
      this.Settings3D.ConnectMvSliderSettings(this._mvNZSlider, 2, AxisSign.Negative);
      this.Settings3D.ConnectMvDblSliderSettings(this._mvXYSlider, PlaneId.XY);
      this.Settings3D.ConnectMvDblSliderSettings(this._mvYZSlider, PlaneId.YZ);
      this.Settings3D.ConnectMvDblSliderSettings(this._mvZXSlider, PlaneId.ZX);
      this.Settings2D.ConnectMvSliderSettings(this._mvP2DModeXSlider, 0, AxisSign.Positive);
      this.Settings2D.ConnectMvSliderSettings(this._mvP2DModeYSlider, 1, AxisSign.Positive);
      this.Settings2D.ConnectMvSliderSettings(this._mvN2DModeXSlider, 0, AxisSign.Negative);
      this.Settings2D.ConnectMvSliderSettings(this._mvN2DModeYSlider, 1, AxisSign.Negative);
      this.Settings2D.ConnectMvDblSliderSettings(this._mv2DModeDblSlider);
      this._mvVertexSnapDrag.Settings = this.Settings3D.VertexSnapSettings;
      this.Settings3D.ConnectRtSliderSettings(this._rtXSlider, 0);
      this.Settings3D.ConnectRtSliderSettings(this._rtYSlider, 1);
      this.Settings3D.ConnectRtSliderSettings(this._rtZSlider, 2);
      this.Settings3D.ConnectRtCamLookSliderSettings(this._rtCamLookSlider);
    }

    private void Update2DGizmoPosition()
    {
      this.Gizmo.Transform.Position2D = (Vector2) this.Gizmo.GetWorkCamera().WorldToScreenPoint(this.Gizmo.Transform.Position3D);
    }

    private void Update2DModeHandlePositions()
    {
      if (this.LookAndFeel2D.IsMvDblSliderVisible)
      {
        this._mvP2DModeXSlider.StartPosition = this._mv2DModeDblSlider.GetRealExtentPoint(Shape2DExtentPoint.Right);
        this._mvP2DModeYSlider.StartPosition = this._mv2DModeDblSlider.GetRealExtentPoint(Shape2DExtentPoint.Top);
        this._mvN2DModeXSlider.StartPosition = this._mv2DModeDblSlider.GetRealExtentPoint(Shape2DExtentPoint.Left);
        this._mvN2DModeYSlider.StartPosition = this._mv2DModeDblSlider.GetRealExtentPoint(Shape2DExtentPoint.Bottom);
      }
      else
      {
        Vector2 position2D = this.Gizmo.Transform.Position2D;
        this._mvP2DModeXSlider.StartPosition = position2D;
        this._mvP2DModeYSlider.StartPosition = position2D;
        this._mvN2DModeXSlider.StartPosition = position2D;
        this._mvN2DModeYSlider.StartPosition = position2D;
      }
    }

    private void OnGizmoTransformChanged(
      GizmoTransform transform,
      GizmoTransform.ChangeData changeData)
    {
      this.Update2DGizmoPosition();
      if (changeData.ChangeReason != GizmoTransform.ChangeReason.ParentChange && changeData.TRSDimension != GizmoDimension.Dim3D)
        return;
      this.UpdateRtCamLookSlider(this.Gizmo.GetWorkCamera());
    }

    private void Hide2DModeHandles()
    {
      this._mv2DModeSliders.SetVisible(false);
      this._mv2DModeSliders.Set2DCapsVisible(false);
      this._mv2DModeDblSlider.SetVisible(false);
      this._mv2DModeDblSlider.SetBorderVisible(false);
    }

    private void UpdateRtCamLookSlider(Camera camera)
    {
      float zoomFactor = this._rtMidCap.GetZoomFactor(camera);
      this._rtCamLookSlider.MakePolySphereBorder(this.Gizmo.Transform.Position3D, this._rtMidCap.GetRealSphereRadius(zoomFactor) + zoomFactor * this.LookAndFeel3D.RtCamLookSliderRadiusOffset, 100, camera);
    }

    private void SetMoveHandlesVisible(bool visible)
    {
      this._mvDblSliders.SetVisible(visible, true);
      this._mvAxesSliders.SetVisible(visible);
      this._mvAxesSliders.Set3DCapsVisible(visible);
    }

    private void SetRotationHandlesVisible(bool visible)
    {
      this._rtAxesSliders.SetBorderVisible(visible);
      this._rtMidCap.SetVisible(visible);
      this._rtCamLookSlider.SetBorderVisible(visible);
    }

    private void SetScaleHandlesVisible(bool visible) => this._scMidCap.SetVisible(visible);

    public UniversalGizmo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._mvAxesSliders = new GizmoLineSlider3DCollection();
      this._mvDblSliders = new GizmoPlaneSlider3DCollection();
      this._mvVertexSnapDrag = new GizmoObjectVertexSnapDrag3D();
      this._mv2DModeSliders = new GizmoLineSlider2DCollection();
      this._rtAxesSliders = new GizmoPlaneSlider3DCollection();
      this._rtCamXYRotationDrag = new GizmoDblAxisRotationDrag3D();
      this._scUnformScaleDrag = new GizmoUniformScaleDrag3D();
      this._scScaleGuide = new GizmoScaleGuide();
      this._settings2D = new UniversalGizmoSettings2D();
      this._settings3D = new UniversalGizmoSettings3D();
      this._lookAndFeel2D = new UniversalGizmoLookAndFeel2D();
      this._lookAndFeel3D = new UniversalGizmoLookAndFeel3D();
      this._hotkeys = new UniversalGizmoHotkeys();
      this._useSnapEnableHotkey = true;
      this._useVertSnapEnableHotkey = true;
      this._use2DModeEnableHotkey = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum MvVertexSnapState
    {
      SelectingPivot,
      Snapping,
      Inactive,
    }
  }
}
