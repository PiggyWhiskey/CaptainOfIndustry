// Decompiled with JetBrains decompiler
// Type: RTG.ScaleGizmo
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
  public class ScaleGizmo : GizmoBehaviour
  {
    private GizmoLineSlider3D _pstvXSlider;
    private GizmoLineSlider3D _pstvYSlider;
    private GizmoLineSlider3D _pstvZSlider;
    private GizmoLineSlider3D _negXSlider;
    private GizmoLineSlider3D _negYSlider;
    private GizmoLineSlider3D _negZSlider;
    private GizmoLineSlider3DCollection _axesSliders;
    private GizmoMultiAxisScaleMode _multiAxisScaleMode;
    private GizmoPlaneSlider3D _xySlider;
    private GizmoPlaneSlider3D _yzSlider;
    private GizmoPlaneSlider3D _zxSlider;
    private GizmoPlaneSlider3DCollection _dblSliders;
    private GizmoCap3D _midCap;
    private GizmoUniformScaleDrag3D _unformScaleDrag;
    private GizmoScaleGuide _scaleGuide;
    private IEnumerable<GameObject> _scaleGuideTargetObjects;
    [SerializeField]
    private ScaleGizmoLookAndFeel3D _lookAndFeel3D;
    [SerializeField]
    private ScaleGizmoSettings3D _settings3D;
    [SerializeField]
    private ScaleGizmoHotkeys _hotkeys;
    [SerializeField]
    private bool _useSnapEnableHotkey;
    [SerializeField]
    private bool _useMultiAxisScaleModeHotkey;
    private ScaleGizmoLookAndFeel3D _sharedLookAndFeel3D;
    private ScaleGizmoSettings3D _sharedSettings3D;
    private ScaleGizmoHotkeys _sharedHotkeys;

    public GizmoMultiAxisScaleMode MultiAxisScaleMode => this._multiAxisScaleMode;

    public ScaleGizmoLookAndFeel3D LookAndFeel3D
    {
      get => this._sharedLookAndFeel3D != null ? this._sharedLookAndFeel3D : this._lookAndFeel3D;
    }

    public ScaleGizmoSettings3D Settings3D
    {
      get => this._sharedSettings3D != null ? this._sharedSettings3D : this._settings3D;
    }

    public ScaleGizmoHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public ScaleGizmoHotkeys SharedHotkeys
    {
      get => this._sharedHotkeys;
      set
      {
        if (!Application.isPlaying)
          return;
        this._sharedHotkeys = value;
      }
    }

    public ScaleGizmoSettings3D SharedSettings3D
    {
      get => this._sharedSettings3D;
      set
      {
        this._sharedSettings3D = value;
        this.SetupSharedSettings();
      }
    }

    public ScaleGizmoLookAndFeel3D SharedLookAndFeel3D
    {
      get => this._sharedLookAndFeel3D;
      set
      {
        this._sharedLookAndFeel3D = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public bool UseSnapEnableHotkey
    {
      get => this._useSnapEnableHotkey;
      set => this._useSnapEnableHotkey = value;
    }

    public bool UseMultiAxisScaleModeHotkey
    {
      get => this._useMultiAxisScaleModeHotkey;
      set => this._useMultiAxisScaleModeHotkey = value;
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
      return this._axesSliders.Contains(handleId) || this._axesSliders.ContainsCapId(handleId) || this._dblSliders.Contains(handleId) || this._midCap.HandleId == handleId;
    }

    public void SetAxesLinesHoverable(bool hoverable)
    {
      this._pstvXSlider.SetHoverable(hoverable);
      this._negXSlider.SetHoverable(hoverable);
      this._pstvYSlider.SetHoverable(hoverable);
      this._negYSlider.SetHoverable(hoverable);
      this._pstvZSlider.SetHoverable(hoverable);
      this._negZSlider.SetHoverable(hoverable);
    }

    public void SetSnapEnabled(bool isEnabled)
    {
      this._unformScaleDrag.IsSnapEnabled = isEnabled;
      this._axesSliders.SetSnapEnabled(isEnabled);
      this._dblSliders.SetSnapEnabled(isEnabled);
    }

    public void SetMultiAxisScaleMode(GizmoMultiAxisScaleMode scaleMode)
    {
      if (this.Gizmo.IsDragged || scaleMode == this._multiAxisScaleMode)
        return;
      this._multiAxisScaleMode = scaleMode;
      if (this._multiAxisScaleMode == GizmoMultiAxisScaleMode.DoubleAxis)
      {
        this._dblSliders.SetVisible(true, true);
        this._midCap.SetVisible(false);
      }
      else
      {
        if (this._multiAxisScaleMode != GizmoMultiAxisScaleMode.Uniform)
          return;
        this._dblSliders.SetVisible(false, true);
        this._midCap.SetVisible(true);
      }
    }

    public void SetScaleGuideTargetObjects(IEnumerable<GameObject> targetObjects)
    {
      this._scaleGuideTargetObjects = targetObjects;
    }

    public override void OnGizmoEnabled() => this.OnGizmoUpdateBegin();

    public override void OnAttached()
    {
      this._midCap = new GizmoCap3D(this.Gizmo, GizmoHandleId.MidScaleCap);
      this._midCap.DragSession = (IGizmoDragSession) this._unformScaleDrag;
      this._pstvXSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PXSlider, GizmoHandleId.PXCap);
      this._pstvXSlider.SetDragChannel(GizmoDragChannel.Scale);
      this._pstvXSlider.MapDirection(0, AxisSign.Positive);
      this._pstvXSlider.ScaleDragAxisIndex = 0;
      this._negXSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NXSlider, GizmoHandleId.NXCap);
      this._negXSlider.SetDragChannel(GizmoDragChannel.Scale);
      this._negXSlider.MapDirection(0, AxisSign.Negative);
      this._negXSlider.ScaleDragAxisIndex = 0;
      this._pstvYSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PYSlider, GizmoHandleId.PYCap);
      this._pstvYSlider.SetDragChannel(GizmoDragChannel.Scale);
      this._pstvYSlider.MapDirection(1, AxisSign.Positive);
      this._pstvYSlider.ScaleDragAxisIndex = 1;
      this._negYSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NYSlider, GizmoHandleId.NYCap);
      this._negYSlider.SetDragChannel(GizmoDragChannel.Scale);
      this._negYSlider.MapDirection(1, AxisSign.Negative);
      this._negYSlider.ScaleDragAxisIndex = 1;
      this._pstvZSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.PZSlider, GizmoHandleId.PZCap);
      this._pstvZSlider.SetDragChannel(GizmoDragChannel.Scale);
      this._pstvZSlider.MapDirection(2, AxisSign.Positive);
      this._pstvZSlider.ScaleDragAxisIndex = 2;
      this._negZSlider = new GizmoLineSlider3D(this.Gizmo, GizmoHandleId.NZSlider, GizmoHandleId.NZCap);
      this._negZSlider.SetDragChannel(GizmoDragChannel.Scale);
      this._negZSlider.MapDirection(2, AxisSign.Negative);
      this._negZSlider.ScaleDragAxisIndex = 2;
      this._axesSliders.Add(this._pstvXSlider);
      this._axesSliders.Add(this._pstvYSlider);
      this._axesSliders.Add(this._pstvZSlider);
      this._axesSliders.Add(this._negXSlider);
      this._axesSliders.Add(this._negYSlider);
      this._axesSliders.Add(this._negZSlider);
      this._axesSliders.Make3DHoverPriorityLowerThan(this._midCap.HoverPriority3D);
      this._axesSliders.RegisterScalerHandle(this._midCap.HandleId, (IEnumerable<int>) new int[3]
      {
        0,
        1,
        2
      });
      this._xySlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.XYDblSlider);
      this._xySlider.SetDragChannel(GizmoDragChannel.Scale);
      this._xySlider.ScaleDragAxisIndexRight = 0;
      this._xySlider.ScaleDragAxisIndexUp = 1;
      this._yzSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.YZDblSlider);
      this._yzSlider.SetDragChannel(GizmoDragChannel.Scale);
      this._yzSlider.ScaleDragAxisIndexRight = 1;
      this._yzSlider.ScaleDragAxisIndexUp = 2;
      this._zxSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.ZXDblSlider);
      this._zxSlider.SetDragChannel(GizmoDragChannel.Scale);
      this._zxSlider.ScaleDragAxisIndexRight = 2;
      this._zxSlider.ScaleDragAxisIndexUp = 0;
      this._dblSliders.Add(this._xySlider);
      this._dblSliders.Add(this._yzSlider);
      this._dblSliders.Add(this._zxSlider);
      this._axesSliders.RegisterScalerHandle(this._xySlider.HandleId, (IEnumerable<int>) new int[2]
      {
        this._xySlider.ScaleDragAxisIndexRight,
        this._xySlider.ScaleDragAxisIndexUp
      });
      this._axesSliders.RegisterScalerHandle(this._yzSlider.HandleId, (IEnumerable<int>) new int[2]
      {
        this._yzSlider.ScaleDragAxisIndexRight,
        this._yzSlider.ScaleDragAxisIndexUp
      });
      this._axesSliders.RegisterScalerHandle(this._zxSlider.HandleId, (IEnumerable<int>) new int[2]
      {
        this._zxSlider.ScaleDragAxisIndexRight,
        this._zxSlider.ScaleDragAxisIndexUp
      });
      this.SetMultiAxisScaleMode(GizmoMultiAxisScaleMode.Uniform);
      this.SetupSharedLookAndFeel();
      this.SetupSharedSettings();
    }

    public override void OnGizmoUpdateBegin()
    {
      this._unformScaleDrag.Sensitivity = this.Settings3D.DragSensitivity;
      if (this.UseSnapEnableHotkey)
        this.SetSnapEnabled(this.Hotkeys.EnableSnapping.IsActive());
      if (this.UseMultiAxisScaleModeHotkey)
      {
        if (this.Hotkeys.ChangeMultiAxisMode.IsActive())
          this.SetMultiAxisScaleMode(GizmoMultiAxisScaleMode.DoubleAxis);
        else
          this.SetMultiAxisScaleMode(GizmoMultiAxisScaleMode.Uniform);
      }
      this._pstvXSlider.SetVisible(this.LookAndFeel3D.IsPositiveSliderVisible(0));
      this._pstvXSlider.Set3DCapVisible(this.LookAndFeel3D.IsPositiveSliderCapVisible(0));
      this._pstvYSlider.SetVisible(this.LookAndFeel3D.IsPositiveSliderVisible(1));
      this._pstvYSlider.Set3DCapVisible(this.LookAndFeel3D.IsPositiveSliderCapVisible(1));
      this._pstvZSlider.SetVisible(this.LookAndFeel3D.IsPositiveSliderVisible(2));
      this._pstvZSlider.Set3DCapVisible(this.LookAndFeel3D.IsPositiveSliderCapVisible(2));
      this._negXSlider.SetVisible(this.LookAndFeel3D.IsNegativeSliderVisible(0));
      this._negXSlider.Set3DCapVisible(this.LookAndFeel3D.IsNegativeSliderCapVisible(0));
      this._negYSlider.SetVisible(this.LookAndFeel3D.IsNegativeSliderVisible(1));
      this._negYSlider.Set3DCapVisible(this.LookAndFeel3D.IsNegativeSliderCapVisible(1));
      this._negZSlider.SetVisible(this.LookAndFeel3D.IsNegativeSliderVisible(2));
      this._negZSlider.Set3DCapVisible(this.LookAndFeel3D.IsNegativeSliderCapVisible(2));
      if (this._multiAxisScaleMode != GizmoMultiAxisScaleMode.DoubleAxis)
        return;
      this._xySlider.SetVisible(this.LookAndFeel3D.IsDblSliderVisible(PlaneId.XY));
      this._xySlider.SetBorderVisible(this._xySlider.IsVisible);
      this._yzSlider.SetVisible(this.LookAndFeel3D.IsDblSliderVisible(PlaneId.YZ));
      this._yzSlider.SetBorderVisible(this._yzSlider.IsVisible);
      this._zxSlider.SetVisible(this.LookAndFeel3D.IsDblSliderVisible(PlaneId.ZX));
      this._zxSlider.SetBorderVisible(this._zxSlider.IsVisible);
      this.PlaceDblSlidersInSliderPlanes(this.Gizmo.FocusCamera);
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
      {
        this._midCap.ApplyZoomFactor(camera);
        this._axesSliders.ApplyZoomFactor(camera);
        this._dblSliders.ApplyZoomFactor(camera);
        if (this._multiAxisScaleMode == GizmoMultiAxisScaleMode.DoubleAxis)
          this.PlaceDblSlidersInSliderPlanes(camera);
      }
      foreach (GizmoSlider renderSortedSlider in this._axesSliders.GetRenderSortedSliders(camera))
        renderSortedSlider.Render(camera);
      this._xySlider.Render(camera);
      this._yzSlider.Render(camera);
      this._zxSlider.Render(camera);
      this._midCap.Render(camera);
      if (!this.LookAndFeel3D.IsScaleGuideVisible || !this.Gizmo.IsDragged || !this.OwnsHandle(this.Gizmo.DragHandleId))
        return;
      this._scaleGuide.Render((IEnumerable<GameObject>) GameObjectEx.FilterParentsOnly(this._scaleGuideTargetObjects), camera);
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      if (handleId != this._midCap.HandleId)
        return;
      this._unformScaleDrag.SetWorkData(new GizmoUniformScaleDrag3D.WorkData()
      {
        DragOrigin = this._midCap.Position,
        CameraRight = this.Gizmo.FocusCamera.transform.right,
        CameraUp = this.Gizmo.FocusCamera.transform.up,
        SnapStep = this.Settings3D.UniformSnapStep
      });
    }

    private void PlaceDblSlidersInSliderPlanes(Camera camera)
    {
      if (this._xySlider.IsVisible)
        this._xySlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.XY, this._pstvXSlider, this._pstvYSlider, camera);
      if (this._yzSlider.IsVisible)
        this._yzSlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.YZ, this._pstvYSlider, this._pstvZSlider, camera);
      if (!this._zxSlider.IsVisible)
        return;
      this._zxSlider.MakeSliderPlane(this.Gizmo.Transform, PlaneId.ZX, this._pstvZSlider, this._pstvXSlider, camera);
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._pstvXSlider, 0, AxisSign.Positive);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._pstvYSlider, 1, AxisSign.Positive);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._pstvZSlider, 2, AxisSign.Positive);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._negXSlider, 0, AxisSign.Negative);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._negYSlider, 1, AxisSign.Negative);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._negZSlider, 2, AxisSign.Negative);
      this.LookAndFeel3D.ConnectMidCapLookAndFeel(this._midCap);
      this.LookAndFeel3D.ConnectGizmoScaleGuideLookAndFeel(this._scaleGuide);
      this.LookAndFeel3D.ConnectDblSliderLookAndFeel(this._xySlider, PlaneId.XY);
      this.LookAndFeel3D.ConnectDblSliderLookAndFeel(this._yzSlider, PlaneId.YZ);
      this.LookAndFeel3D.ConnectDblSliderLookAndFeel(this._zxSlider, PlaneId.ZX);
    }

    private void SetupSharedSettings()
    {
      this.Settings3D.ConnectSliderSettings(this._pstvXSlider, 0, AxisSign.Positive);
      this.Settings3D.ConnectSliderSettings(this._pstvYSlider, 1, AxisSign.Positive);
      this.Settings3D.ConnectSliderSettings(this._pstvZSlider, 2, AxisSign.Positive);
      this.Settings3D.ConnectSliderSettings(this._negXSlider, 0, AxisSign.Negative);
      this.Settings3D.ConnectSliderSettings(this._negYSlider, 1, AxisSign.Negative);
      this.Settings3D.ConnectSliderSettings(this._negZSlider, 2, AxisSign.Negative);
      this.Settings3D.ConnectDblSliderSettings(this._xySlider, PlaneId.XY);
      this.Settings3D.ConnectDblSliderSettings(this._yzSlider, PlaneId.YZ);
      this.Settings3D.ConnectDblSliderSettings(this._zxSlider, PlaneId.ZX);
    }

    public ScaleGizmo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._axesSliders = new GizmoLineSlider3DCollection();
      this._dblSliders = new GizmoPlaneSlider3DCollection();
      this._unformScaleDrag = new GizmoUniformScaleDrag3D();
      this._scaleGuide = new GizmoScaleGuide();
      this._lookAndFeel3D = new ScaleGizmoLookAndFeel3D();
      this._settings3D = new ScaleGizmoSettings3D();
      this._hotkeys = new ScaleGizmoHotkeys();
      this._useSnapEnableHotkey = true;
      this._useMultiAxisScaleModeHotkey = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
