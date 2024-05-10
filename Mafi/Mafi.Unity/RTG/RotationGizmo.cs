// Decompiled with JetBrains decompiler
// Type: RTG.RotationGizmo
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
  [Serializable]
  public class RotationGizmo : GizmoBehaviour
  {
    private GizmoPlaneSlider3D _xSlider;
    private GizmoPlaneSlider3D _ySlider;
    private GizmoPlaneSlider3D _zSlider;
    private GizmoPlaneSlider3DCollection _axesSliders;
    private GizmoCap3D _midCap;
    private GizmoDblAxisRotationDrag3D _camXYRotationDrag;
    private GizmoPlaneSlider2D _camLookSlider;
    [SerializeField]
    private RotationGizmoHotkeys _hotkeys;
    [SerializeField]
    private RotationGizmoSettings3D _settings3D;
    [SerializeField]
    private RotationGizmoLookAndFeel3D _lookAndFeel3D;
    [SerializeField]
    private bool _useSnapEnableHotkey;
    private RotationGizmoHotkeys _sharedHotkeys;
    private RotationGizmoSettings3D _sharedSettings3D;
    private RotationGizmoLookAndFeel3D _sharedLookAndFeel3D;

    public RotationGizmoSettings3D Settings3D
    {
      get => this._sharedSettings3D != null ? this._sharedSettings3D : this._settings3D;
    }

    public RotationGizmoLookAndFeel3D LookAndFeel3D
    {
      get => this._sharedLookAndFeel3D != null ? this._sharedLookAndFeel3D : this._lookAndFeel3D;
    }

    public RotationGizmoHotkeys Hotkeys
    {
      get => this._sharedHotkeys != null ? this._sharedHotkeys : this._hotkeys;
    }

    public RotationGizmoSettings3D SharedSettings3D
    {
      get => this._sharedSettings3D;
      set
      {
        this._sharedSettings3D = value;
        this.SetupSharedSettings();
      }
    }

    public RotationGizmoLookAndFeel3D SharedLookAndFeel3D
    {
      get => this._sharedLookAndFeel3D;
      set
      {
        this._sharedLookAndFeel3D = value;
        this.SetupSharedLookAndFeel();
      }
    }

    public RotationGizmoHotkeys SharedHotkeys
    {
      get => this._sharedHotkeys;
      set => this._sharedHotkeys = value;
    }

    public bool UseSnapEnableHotkey
    {
      get => this._useSnapEnableHotkey;
      set => this._useSnapEnableHotkey = value;
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
      return this._axesSliders.Contains(handleId) || this._midCap.HandleId == handleId;
    }

    public void SetMidCapHoverable(bool hoverable) => this._midCap.SetHoverable(hoverable);

    public void SetSnapEnabled(bool isEnabled)
    {
      this._axesSliders.SetSnapEnabled(isEnabled);
      this._camXYRotationDrag.IsSnapEnabled = isEnabled;
      this._camLookSlider.SetSnapEnabled(isEnabled);
    }

    public override void OnGizmoEnabled() => this.OnGizmoUpdateBegin();

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

    public override void OnAttached()
    {
      this._midCap = new GizmoCap3D(this.Gizmo, GizmoHandleId.CamXYRotation);
      this._midCap.DragSession = (IGizmoDragSession) this._camXYRotationDrag;
      this._camXYRotationDrag.AddTargetTransform(this.Gizmo.Transform);
      this._xSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.XRotationSlider);
      this._xSlider.SetDragChannel(GizmoDragChannel.Rotation);
      this._xSlider.LocalRotation = Quaternion.Euler(0.0f, 90f, 0.0f);
      this._xSlider.SetVisible(false);
      this._axesSliders.Add(this._xSlider);
      this._ySlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.YRotationSlider);
      this._ySlider.SetDragChannel(GizmoDragChannel.Rotation);
      this._ySlider.LocalRotation = Quaternion.Euler(90f, 0.0f, 0.0f);
      this._ySlider.SetVisible(false);
      this._axesSliders.Add(this._ySlider);
      this._zSlider = new GizmoPlaneSlider3D(this.Gizmo, GizmoHandleId.ZRotationSlider);
      this._zSlider.SetDragChannel(GizmoDragChannel.Rotation);
      this._zSlider.SetVisible(false);
      this._axesSliders.Add(this._zSlider);
      this._axesSliders.Make3DHoverPriorityHigherThan(this._midCap.HoverPriority3D);
      this._camLookSlider = new GizmoPlaneSlider2D(this.Gizmo, GizmoHandleId.CamZRotation);
      this._camLookSlider.SetDragChannel(GizmoDragChannel.Rotation);
      this._camLookSlider.SetVisible(false);
      this.SetupSharedLookAndFeel();
      this.SetupSharedSettings();
    }

    public override void OnGizmoUpdateBegin()
    {
      if (this.UseSnapEnableHotkey)
        this.SetSnapEnabled(this.Hotkeys.EnableSnapping.IsActive());
      this._midCap.SetVisible(this.LookAndFeel3D.IsMidCapVisible);
      this._camXYRotationDrag.Sensitivity = this.Settings3D.DragSensitivity;
      this._xSlider.SetBorderVisible(this.LookAndFeel3D.IsAxisVisible(0));
      this._ySlider.SetBorderVisible(this.LookAndFeel3D.IsAxisVisible(1));
      this._zSlider.SetBorderVisible(this.LookAndFeel3D.IsAxisVisible(2));
      this._camLookSlider.SetBorderVisible(this.LookAndFeel3D.IsCamLookSliderVisible);
      if (!this._camLookSlider.IsBorderVisible)
        return;
      this.UpdateCamLookSlider(this.Gizmo.FocusCamera);
    }

    public override void OnGizmoRender(Camera camera)
    {
      if (MonoSingleton<RTGizmosEngine>.Get.NumRenderCameras > 1)
      {
        this._midCap.ApplyZoomFactor(camera);
        this._axesSliders.ApplyZoomFactor(camera);
        if (this._camLookSlider.IsBorderVisible)
          this.UpdateCamLookSlider(camera);
      }
      this._xSlider.Render(camera);
      this._ySlider.Render(camera);
      this._zSlider.Render(camera);
      this._midCap.Render(camera);
      this._camLookSlider.Render(camera);
    }

    public override void OnGizmoAttemptHandleDragBegin(int handleId)
    {
      if (handleId != this._midCap.HandleId)
        return;
      this._camXYRotationDrag.SetWorkData(new GizmoDblAxisRotationDrag3D.WorkData()
      {
        Axis0 = this.Gizmo.FocusCamera.transform.up,
        Axis1 = this.Gizmo.FocusCamera.transform.right,
        ScreenAxis0 = (Vector2) -Vector3.right,
        ScreenAxis1 = (Vector2) Vector3.up,
        SnapMode = this.Settings3D.SnapMode,
        SnapStep0 = this.Settings3D.CamUpSnapStep,
        SnapStep1 = this.Settings3D.CamRightSnapStep
      });
    }

    private void UpdateCamLookSlider(Camera camera)
    {
      float zoomFactor = this._midCap.GetZoomFactor(camera);
      this._camLookSlider.MakePolySphereBorder(this.Gizmo.Transform.Position3D, this._midCap.GetRealSphereRadius(zoomFactor) + zoomFactor * this.LookAndFeel3D.CamLookSliderRadiusOffset, 100, camera);
    }

    private void SetupSharedLookAndFeel()
    {
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._xSlider, 0);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._ySlider, 1);
      this.LookAndFeel3D.ConnectSliderLookAndFeel(this._zSlider, 2);
      this.LookAndFeel3D.ConnectCamLookSliderLookAndFeel(this._camLookSlider);
      this.LookAndFeel3D.ConnectMidCapLookAndFeel(this._midCap);
    }

    private void SetupSharedSettings()
    {
      this.Settings3D.ConnectSliderSettings(this._xSlider, 0);
      this.Settings3D.ConnectSliderSettings(this._ySlider, 1);
      this.Settings3D.ConnectSliderSettings(this._zSlider, 2);
      this.Settings3D.ConnectCamLookSliderSettings(this._camLookSlider);
    }

    private void OnGizmoTransformChanged(
      GizmoTransform gizmoTransform,
      GizmoTransform.ChangeData changeData)
    {
      if (changeData.ChangeReason != GizmoTransform.ChangeReason.ParentChange && changeData.TRSDimension != GizmoDimension.Dim3D)
        return;
      this.UpdateCamLookSlider(this.Gizmo.GetWorkCamera());
    }

    public RotationGizmo()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._axesSliders = new GizmoPlaneSlider3DCollection();
      this._camXYRotationDrag = new GizmoDblAxisRotationDrag3D();
      this._hotkeys = new RotationGizmoHotkeys();
      this._settings3D = new RotationGizmoSettings3D();
      this._lookAndFeel3D = new RotationGizmoLookAndFeel3D();
      this._useSnapEnableHotkey = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
