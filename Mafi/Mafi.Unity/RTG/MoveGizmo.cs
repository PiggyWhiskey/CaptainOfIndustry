// Decompiled with JetBrains decompiler
// Type: RTG.MoveGizmo
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
  public class MoveGizmo : GizmoBehaviour
  {
    private GizmoLineSlider3D _pXSlider;
    private GizmoLineSlider3D _pYSlider;
    private GizmoLineSlider3D _pZSlider;
    private GizmoLineSlider3D _nXSlider;
    private GizmoLineSlider3D _nYSlider;
    private GizmoLineSlider3D _nZSlider;
    private GizmoLineSlider3DCollection _axesSliders;
    private GizmoPlaneSlider3D _xySlider;
    private GizmoPlaneSlider3D _yzSlider;
    private GizmoPlaneSlider3D _zxSlider;
    private GizmoPlaneSlider3DCollection _dblSliders;
    private GizmoCap3D _midCap;
    private bool _isVertexSnapEnabled;
    private GizmoCap2D _vertSnapCap;
    private GizmoObjectVertexSnapDrag3D _vertexSnapDrag;
    private Vector3 _postVSnapPosRestore;
    private bool _is2DModeEnabled;
    private GizmoLineSlider2D _p2DModeXSlider;
    private GizmoLineSlider2D _p2DModeYSlider;
    private GizmoLineSlider2D _n2DModeXSlider;
    private GizmoLineSlider2D _n2DModeYSlider;
    private GizmoLineSlider2DCollection _2DModeSliders;
    private GizmoPlaneSlider2D _2DModeDblSlider;
    [SerializeField]
    private bool _useSnapEnableHotkey;
    [SerializeField]
    private bool _useVertSnapEnableHotkey;
    [SerializeField]
    private bool _use2DModeEnableHotkey;
    [SerializeField]
    private MoveGizmoHotkeys _hotkeys;
    [SerializeField]
    private MoveGizmoSettings2D _settings2D;
    [SerializeField]
    private MoveGizmoSettings3D _settings3D;
    [SerializeField]
    private MoveGizmoLookAndFeel2D _lookAndFeel2D;
    [SerializeField]
    private MoveGizmoLookAndFeel3D _lookAndFeel3D;
    private MoveGizmoHotkeys _sharedHotkeys;
    private MoveGizmoSettings2D _sharedSettings2D;
    private MoveGizmoSettings3D _sharedSettings3D;
    private MoveGizmoLookAndFeel2D _sharedLookAndFeel2D;
    private MoveGizmoLookAndFeel3D _sharedLookAndFeel3D;

    public MoveGizmoSettings2D Settings2D
    {
      get => this._sharedSettings2D != null ? this._sharedSettings2D : this._settings2D;
    }

    public MoveGizmoSettings3D Settings3D
    {
      get => this._sharedSettings3D != null ? this._sharedSettings3D : this._settings3D;
    }

    public MoveGizmoLookAndFeel2D LookAndFeel2D
    {
      get => this._sharedLookAndFeel2D != null ? this._sharedLookAndFeel2D : this._lookAndFeel2D;
    }

    public MoveGizmoLookAndFeel3D LookAndFeel3D
    {
      get => this._sharedLookAndFeel3D != null ? this._sharedLookAndFeel3D : this._lookAndFeel3D;
    }

    public MoveGizmoHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public MoveGizmoSettings2D SharedSettings2D
    {
      get => this._sharedSettings2D;
      set
      {
        this._sharedSettings2D = value;
        this.SetupSharedSettings();
      }
    }

    public MoveGizmoSettings3D SharedSettings3D
    {
      get => this._sharedSettings3D;
      set
      {
        this._sharedSettings3D = value;
        this.SetupSharedSettings();
      }
    }

    public MoveGizmoLookAndFeel2D SharedLookAndFeel2D
    {
      get => this._sharedLookAndFeel2D;
      set
      {
        this._sharedLookAndFeel2D = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public MoveGizmoLookAndFeel3D SharedLookAndFeel3D
    {
      get => this._sharedLookAndFeel3D;
      set
      {
        this._sharedLookAndFeel3D = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public MoveGizmoHotkeys SharedHotkeys
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

    public MoveGizmo.VertexSnapState GetVertexSnapState()
    {
      if (!this._isVertexSnapEnabled)
        return MoveGizmo.VertexSnapState.Inactive;
      return this._vertexSnapDrag.IsActive ? MoveGizmo.VertexSnapState.Snapping : MoveGizmo.VertexSnapState.SelectingPivot;
    }

    public float GetZoomFactor(Vector3 position)
    {
      return !this.LookAndFeel3D.UseZoomFactor ? 1f : this.Gizmo.GetWorkCamera().EstimateZoomFactor(position);
    }

    public float GetZoomFactor(Vector3 position, Camera camera)
    {
      return !this.LookAndFeel3D.UseZoomFactor ? 1f : camera.EstimateZoomFactor(position);
    }

    public bool OwnsHandle(int handleId)
    {
      return this._axesSliders.Contains(handleId) || this._axesSliders.ContainsCapId(handleId) || this._midCap.HandleId == handleId || this._dblSliders.Contains(handleId) || this._2DModeSliders.Contains(handleId) || this._2DModeSliders.ContainsCapId(handleId) || this._2DModeDblSlider.HandleId == handleId;
    }

    public void SetAxesLinesHoverable(bool hoverable)
    {
      this._pXSlider.SetHoverable(hoverable);
      this._nXSlider.SetHoverable(hoverable);
      this._pYSlider.SetHoverable(hoverable);
      this._nYSlider.SetHoverable(hoverable);
      this._pZSlider.SetHoverable(hoverable);
      this._nZSlider.SetHoverable(hoverable);
    }

    public void SetSnapEnabled(bool isEnabled)
    {
      this._axesSliders.SetSnapEnabled(isEnabled);
      this._2DModeSliders.SetSnapEnabled(isEnabled);
      this._dblSliders.SetSnapEnabled(isEnabled);
      this._2DModeDblSlider.SetSnapEnabled(isEnabled);
    }

    public void SetVertexSnapEnabled(bool isEnabled)
    {
      if (this._isVertexSnapEnabled == isEnabled || this._is2DModeEnabled || !this._isEnabled || this.Gizmo.IsDragged)
        return;
      if (isEnabled)
      {
        this._vertSnapCap.SetVisible(true);
        this._midCap.SetVisible(false);
        this._dblSliders.SetVisible(false, true);
      }
      else
        this._vertSnapCap.SetVisible(false);
      this._isVertexSnapEnabled = isEnabled;
    }

    public void Set2DModeEnabled(bool isEnabled)
    {
      if (this._is2DModeEnabled == isEnabled || this._isVertexSnapEnabled || !this._isEnabled || this.Gizmo.IsDragged)
        return;
      if (isEnabled)
      {
        this._midCap.SetVisible(false);
        this._2DModeSliders.SetVisible(true);
        this._2DModeSliders.Set2DCapsVisible(true);
        this._2DModeDblSlider.SetVisible(true);
        this._2DModeDblSlider.SetBorderVisible(true);
        this._dblSliders.SetVisible(false, true);
        this._axesSliders.SetVisible(false);
        this._axesSliders.Set3DCapsVisible(false);
        this._2DModeSliders.SetOffsetDragOrigin(this.Gizmo.Transform.Position3D);
        this._2DModeDblSlider.OffsetDragOrigin = this.Gizmo.Transform.Position3D;
        this.Update2DGizmoPosition();
        this.Update2DModeHandlePositions();
      }
      else
        this.Hide2DModeHandles();
      this._is2DModeEnabled = isEnabled;
    }

    public void SetVertexSnapTargetObjects(IEnumerable<GameObject> targetObjects)
    {
      this._vertexSnapDrag.SetTargetObjects(targetObjects);
    }

    public override void OnAttached()
    {
      this._midCap = new GizmoCap3D(this.Gizmo, GizmoHandleId.MidDisplayCap);
      this._midCap.SetHoverable(false);
      this._xySlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.XYDblSlider);
      this._yzSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.YZDblSlider);
      this._zxSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.ZXDblSlider);
      this._dblSliders.Add(this._xySlider);
      this._dblSliders.Add(this._yzSlider);
      this._dblSliders.Add(this._zxSlider);
      this._pXSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PXSlider, GizmoHandleId.PXCap);
      this._pXSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._pXSlider.MapDirection(0, AxisSign.Positive);
      this._nXSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NXSlider, GizmoHandleId.NXCap);
      this._nXSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._nXSlider.MapDirection(0, AxisSign.Negative);
      this._pYSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PYSlider, GizmoHandleId.PYCap);
      this._pYSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._pYSlider.MapDirection(1, AxisSign.Positive);
      this._nYSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NYSlider, GizmoHandleId.NYCap);
      this._nYSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._nYSlider.MapDirection(1, AxisSign.Negative);
      this._pZSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PZSlider, GizmoHandleId.PZCap);
      this._pZSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._pZSlider.MapDirection(2, AxisSign.Positive);
      this._nZSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NZSlider, GizmoHandleId.NZCap);
      this._nZSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._nZSlider.MapDirection(2, AxisSign.Negative);
      this._axesSliders.Add(this._pXSlider);
      this._axesSliders.Add(this._pYSlider);
      this._axesSliders.Add(this._pZSlider);
      this._axesSliders.Add(this._nXSlider);
      this._axesSliders.Add(this._nYSlider);
      this._axesSliders.Add(this._nZSlider);
      this._axesSliders.Make3DHoverPriorityLowerThan(this._xySlider.HoverPriority3D);
      this._axesSliders.Make3DHoverPriorityLowerThan(this._yzSlider.HoverPriority3D);
      this._axesSliders.Make3DHoverPriorityLowerThan(this._zxSlider.HoverPriority3D);
      this._vertSnapCap = new GizmoCap2D(this.Gizmo, GizmoHandleId.VertSnap);
      this._vertSnapCap.SetVisible(false);
      this._vertSnapCap.DragSession = (IGizmoDragSession) this._vertexSnapDrag;
      this._vertexSnapDrag.AddTargetTransform(this.Gizmo.Transform);
      this._2DModeDblSlider = new GizmoPlaneSlider2D(this.Gizmo, GizmoHandleId.CamXYSlider);
      this._2DModeDblSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._2DModeDblSlider.SetVisible(false);
      this._p2DModeXSlider = new GizmoLineSlider2D(this.Gizmo, GizmoHandleId.PCamXSlider, GizmoHandleId.PCamXCap);
      this._p2DModeXSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._p2DModeXSlider.MapDirection(0, AxisSign.Positive);
      this._p2DModeXSlider.HoverPriority2D.MakeLowerThan(this._2DModeDblSlider.HoverPriority2D);
      this._p2DModeYSlider = new GizmoLineSlider2D(this.Gizmo, GizmoHandleId.PCamYSlider, GizmoHandleId.PCamYCap);
      this._p2DModeYSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._p2DModeYSlider.MapDirection(1, AxisSign.Positive);
      this._p2DModeYSlider.HoverPriority2D.MakeLowerThan(this._2DModeDblSlider.HoverPriority2D);
      this._n2DModeXSlider = new GizmoLineSlider2D(this.Gizmo, GizmoHandleId.NCamXSlider, GizmoHandleId.NCamXCap);
      this._n2DModeXSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._n2DModeXSlider.MapDirection(0, AxisSign.Negative);
      this._n2DModeXSlider.HoverPriority2D.MakeLowerThan(this._2DModeDblSlider.HoverPriority2D);
      this._n2DModeYSlider = new GizmoLineSlider2D(this.Gizmo, GizmoHandleId.NCamYSlider, GizmoHandleId.NCamYCap);
      this._n2DModeYSlider.SetDragChannel(GizmoDragChannel.Offset);
      this._n2DModeYSlider.MapDirection(1, AxisSign.Negative);
      this._n2DModeYSlider.HoverPriority2D.MakeLowerThan(this._2DModeDblSlider.HoverPriority2D);
      this._2DModeSliders.Add(this._p2DModeXSlider);
      this._2DModeSliders.Add(this._p2DModeYSlider);
      this._2DModeSliders.Add(this._n2DModeXSlider);
      this._2DModeSliders.Add(this._n2DModeYSlider);
      this.Hide2DModeHandles();
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
      this.Update2DGizmoPosition();
      if (!this._is2DModeEnabled && !this._isVertexSnapEnabled)
        this._midCap.SetVisible(this.LookAndFeel3D.IsMidCapVisible);
      if (this.UseSnapEnableHotkey)
        this.SetSnapEnabled(this.Hotkeys.EnableSnapping.IsActive());
      if (this.Use2DModeEnableHotkey)
        this.Set2DModeEnabled(this.Hotkeys.Enable2DMode.IsActive());
      if (!this._is2DModeEnabled)
      {
        bool vertexSnapEnabled = this._isVertexSnapEnabled;
        if (!vertexSnapEnabled)
          this._postVSnapPosRestore = this.Gizmo.Transform.Position3D;
        if (this.UseVertSnapEnableHotkey)
          this.SetVertexSnapEnabled(this.Hotkeys.EnableVertexSnapping.IsActive());
        if (vertexSnapEnabled && !this._isVertexSnapEnabled)
          this.Gizmo.Transform.Position3D = this._postVSnapPosRestore;
        this._pXSlider.SetVisible(this.LookAndFeel3D.IsPositiveSliderVisible(0));
        this._pXSlider.Set3DCapVisible(this.LookAndFeel3D.IsPositiveSliderCapVisible(0));
        this._pYSlider.SetVisible(this.LookAndFeel3D.IsPositiveSliderVisible(1));
        this._pYSlider.Set3DCapVisible(this.LookAndFeel3D.IsPositiveSliderCapVisible(1));
        this._pZSlider.SetVisible(this.LookAndFeel3D.IsPositiveSliderVisible(2));
        this._pZSlider.Set3DCapVisible(this.LookAndFeel3D.IsPositiveSliderCapVisible(2));
        this._nXSlider.SetVisible(this.LookAndFeel3D.IsNegativeSliderVisible(0));
        this._nXSlider.Set3DCapVisible(this.LookAndFeel3D.IsNegativeSliderCapVisible(0));
        this._nYSlider.SetVisible(this.LookAndFeel3D.IsNegativeSliderVisible(1));
        this._nYSlider.Set3DCapVisible(this.LookAndFeel3D.IsNegativeSliderCapVisible(1));
        this._nZSlider.SetVisible(this.LookAndFeel3D.IsNegativeSliderVisible(2));
        this._nZSlider.Set3DCapVisible(this.LookAndFeel3D.IsNegativeSliderCapVisible(2));
      }
      if (!this._isVertexSnapEnabled && !this._is2DModeEnabled)
      {
        this._xySlider.SetVisible(this.LookAndFeel3D.IsDblSliderVisible(PlaneId.XY));
        this._xySlider.SetBorderVisible(this._xySlider.IsVisible);
        this._yzSlider.SetVisible(this.LookAndFeel3D.IsDblSliderVisible(PlaneId.YZ));
        this._yzSlider.SetBorderVisible(this._yzSlider.IsVisible);
        this._zxSlider.SetVisible(this.LookAndFeel3D.IsDblSliderVisible(PlaneId.ZX));
        this._zxSlider.SetBorderVisible(this._zxSlider.IsVisible);
        this.PlaceDblSlidersInSliderPlanes(this.Gizmo.FocusCamera);
      }
      else if (this._isVertexSnapEnabled)
      {
        if (this.GetVertexSnapState() != MoveGizmo.VertexSnapState.SelectingPivot || !this._vertexSnapDrag.SelectSnapPivotPoint(this.Gizmo))
          return;
        this.Gizmo.Transform.Position3D = this._vertexSnapDrag.SnapPivot;
      }
      else
      {
        if (!this._is2DModeEnabled)
          return;
        this._p2DModeXSlider.SetVisible(this.LookAndFeel2D.IsPositiveSliderVisible(0));
        this._p2DModeXSlider.Set2DCapVisible(this.LookAndFeel2D.IsPositiveSliderCapVisible(0));
        this._p2DModeYSlider.SetVisible(this.LookAndFeel2D.IsPositiveSliderVisible(1));
        this._p2DModeYSlider.Set2DCapVisible(this.LookAndFeel2D.IsPositiveSliderCapVisible(1));
        this._n2DModeXSlider.SetVisible(this.LookAndFeel2D.IsNegativeSliderVisible(0));
        this._n2DModeXSlider.Set2DCapVisible(this.LookAndFeel2D.IsNegativeSliderCapVisible(0));
        this._n2DModeYSlider.SetVisible(this.LookAndFeel2D.IsNegativeSliderVisible(1));
        this._n2DModeYSlider.Set2DCapVisible(this.LookAndFeel2D.IsNegativeSliderCapVisible(1));
        bool isVisible = this._2DModeDblSlider.IsVisible;
        this._2DModeDblSlider.SetVisible(this.LookAndFeel2D.IsDblSliderVisible);
        this._2DModeDblSlider.SetBorderVisible(this.LookAndFeel2D.IsDblSliderVisible);
        if (isVisible || !this._2DModeDblSlider.IsVisible)
          return;
        this.Update2DModeHandlePositions();
      }
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
      {
        this._axesSliders.ApplyZoomFactor(camera);
        if (this._midCap.IsVisible)
          this._midCap.ApplyZoomFactor(camera);
        if (!this._isVertexSnapEnabled && !this._is2DModeEnabled)
        {
          this._dblSliders.ApplyZoomFactor(camera);
          this.PlaceDblSlidersInSliderPlanes(camera);
        }
        this.Update2DGizmoPosition();
        if (this._is2DModeEnabled)
          this.Update2DModeHandlePositions();
      }
      foreach (GizmoSlider renderSortedSlider in this._axesSliders.GetRenderSortedSliders(camera))
        renderSortedSlider.Render(camera);
      this._midCap.Render(camera);
      this._xySlider.Render(camera);
      this._yzSlider.Render(camera);
      this._zxSlider.Render(camera);
      this._vertSnapCap.Render(camera);
      this._2DModeSliders.Render(camera);
      this._2DModeDblSlider.Render(camera);
    }

    public override void OnGizmoDragUpdate(int handleId)
    {
      if (!this._isVertexSnapEnabled)
        return;
      this._postVSnapPosRestore += this.Gizmo.RelativeDragOffset;
    }

    private void PlaceDblSlidersInSliderPlanes(Camera camera)
    {
      if (this._xySlider.IsVisible)
        this._xySlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.XY, this._pXSlider, this._pYSlider, camera);
      if (this._yzSlider.IsVisible)
        this._yzSlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.YZ, this._pYSlider, this._pZSlider, camera);
      if (!this._zxSlider.IsVisible)
        return;
      this._zxSlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.ZX, this._pZSlider, this._pXSlider, camera);
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._pXSlider, 0, AxisSign.Positive);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._pYSlider, 1, AxisSign.Positive);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._pZSlider, 2, AxisSign.Positive);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._nXSlider, 0, AxisSign.Negative);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._nYSlider, 1, AxisSign.Negative);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._nZSlider, 2, AxisSign.Negative);
      this.LookAndFeel3D.ConnectDblSliderLookAndFeel(this._xySlider, PlaneId.XY);
      this.LookAndFeel3D.ConnectDblSliderLookAndFeel(this._yzSlider, PlaneId.YZ);
      this.LookAndFeel3D.ConnectDblSliderLookAndFeel(this._zxSlider, PlaneId.ZX);
      this.LookAndFeel3D.ConnectMidCapLookAndFeel(this._midCap);
      this.LookAndFeel3D.ConnectVertSnapCapLookAndFeel(this._vertSnapCap);
      this.LookAndFeel2D.ConnectSliderLookAndFeel(this._p2DModeXSlider, 0, AxisSign.Positive);
      this.LookAndFeel2D.ConnectSliderLookAndFeel(this._p2DModeYSlider, 1, AxisSign.Positive);
      this.LookAndFeel2D.ConnectSliderLookAndFeel(this._n2DModeXSlider, 0, AxisSign.Negative);
      this.LookAndFeel2D.ConnectSliderLookAndFeel(this._n2DModeYSlider, 1, AxisSign.Negative);
      this.LookAndFeel2D.ConnectDblSliderLookAndFeel(this._2DModeDblSlider);
    }

    private void SetupSharedSettings()
    {
      this.Settings3D.ConnectSliderSettings(this._pXSlider, 0, AxisSign.Positive);
      this.Settings3D.ConnectSliderSettings(this._pYSlider, 1, AxisSign.Positive);
      this.Settings3D.ConnectSliderSettings(this._pZSlider, 2, AxisSign.Positive);
      this.Settings3D.ConnectSliderSettings(this._nXSlider, 0, AxisSign.Negative);
      this.Settings3D.ConnectSliderSettings(this._nYSlider, 1, AxisSign.Negative);
      this.Settings3D.ConnectSliderSettings(this._nZSlider, 2, AxisSign.Negative);
      this.Settings3D.ConnectDblSliderSettings(this._xySlider, PlaneId.XY);
      this.Settings3D.ConnectDblSliderSettings(this._yzSlider, PlaneId.YZ);
      this.Settings3D.ConnectDblSliderSettings(this._zxSlider, PlaneId.ZX);
      this.Settings2D.ConnectSliderSettings(this._p2DModeXSlider, 0, AxisSign.Positive);
      this.Settings2D.ConnectSliderSettings(this._p2DModeYSlider, 1, AxisSign.Positive);
      this.Settings2D.ConnectSliderSettings(this._n2DModeXSlider, 0, AxisSign.Negative);
      this.Settings2D.ConnectSliderSettings(this._n2DModeYSlider, 1, AxisSign.Negative);
      this.Settings2D.ConnectDblSliderSettings(this._2DModeDblSlider);
      this._vertexSnapDrag.Settings = this.Settings3D.VertexSnapSettings;
    }

    private void Update2DGizmoPosition()
    {
      this.Gizmo.Transform.Position2D = (Vector2) this.Gizmo.GetWorkCamera().WorldToScreenPoint(this.Gizmo.Transform.Position3D);
    }

    private void Update2DModeHandlePositions()
    {
      if (this.LookAndFeel2D.IsDblSliderVisible)
      {
        this._p2DModeXSlider.StartPosition = this._2DModeDblSlider.GetRealExtentPoint(Shape2DExtentPoint.Right);
        this._p2DModeYSlider.StartPosition = this._2DModeDblSlider.GetRealExtentPoint(Shape2DExtentPoint.Top);
        this._n2DModeXSlider.StartPosition = this._2DModeDblSlider.GetRealExtentPoint(Shape2DExtentPoint.Left);
        this._n2DModeYSlider.StartPosition = this._2DModeDblSlider.GetRealExtentPoint(Shape2DExtentPoint.Bottom);
      }
      else
      {
        Vector2 position2D = this.Gizmo.Transform.Position2D;
        this._p2DModeXSlider.StartPosition = position2D;
        this._p2DModeYSlider.StartPosition = position2D;
        this._n2DModeXSlider.StartPosition = position2D;
        this._n2DModeYSlider.StartPosition = position2D;
      }
    }

    private void OnGizmoTransformChanged(
      GizmoTransform transform,
      GizmoTransform.ChangeData changeData)
    {
      this.Update2DGizmoPosition();
    }

    private void Hide2DModeHandles()
    {
      this._2DModeSliders.SetVisible(false);
      this._2DModeSliders.Set2DCapsVisible(false);
      this._2DModeDblSlider.SetVisible(false);
      this._2DModeDblSlider.SetBorderVisible(false);
    }

    public MoveGizmo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._axesSliders = new GizmoLineSlider3DCollection();
      this._dblSliders = new GizmoPlaneSlider3DCollection();
      this._vertexSnapDrag = new GizmoObjectVertexSnapDrag3D();
      this._2DModeSliders = new GizmoLineSlider2DCollection();
      this._useSnapEnableHotkey = true;
      this._useVertSnapEnableHotkey = true;
      this._use2DModeEnableHotkey = true;
      this._hotkeys = new MoveGizmoHotkeys();
      this._settings2D = new MoveGizmoSettings2D();
      this._settings3D = new MoveGizmoSettings3D();
      this._lookAndFeel2D = new MoveGizmoLookAndFeel2D();
      this._lookAndFeel3D = new MoveGizmoLookAndFeel3D();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public enum VertexSnapState
    {
      SelectingPivot,
      Snapping,
      Inactive,
    }
  }
}
