// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap3D
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
  public class GizmoCap3D : GizmoCap
  {
    private int _coneIndex;
    private ConeShape3D _cone;
    private int _pyramidIndex;
    private PyramidShape3D _pyramid;
    private int _boxIndex;
    private BoxShape3D _box;
    private int _sphereIndex;
    private SphereShape3D _sphere;
    private int _trPrismIndex;
    private TriangPrismShape3D _trPrism;
    private GizmoCap3DControllerData _controllerData;
    private IGizmoCap3DController[] _controllers;
    private GizmoTransform _transform;
    private GizmoOverrideColor _overrideColor;
    private GizmoCap3DLookAndFeel _lookAndFeel;
    private GizmoCap3DLookAndFeel _sharedLookAndFeel;

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

    public GizmoOverrideColor OverrideColor => this._overrideColor;

    public IGizmoDragSession DragSession
    {
      get => this.Handle.DragSession;
      set => this.Handle.DragSession = value;
    }

    public GizmoCap3DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel == null ? this._lookAndFeel : this._sharedLookAndFeel;
    }

    public GizmoCap3DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set => this._sharedLookAndFeel = value;
    }

    public GizmoCap3D(Gizmo gizmo, int handleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._cone = new ConeShape3D();
      this._pyramid = new PyramidShape3D();
      this._box = new BoxShape3D();
      this._sphere = new SphereShape3D();
      this._trPrism = new TriangPrismShape3D();
      this._controllerData = new GizmoCap3DControllerData();
      this._controllers = new IGizmoCap3DController[Enum.GetValues(typeof (GizmoCap3DType)).Length];
      this._transform = new GizmoTransform();
      this._overrideColor = new GizmoOverrideColor();
      this._lookAndFeel = new GizmoCap3DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector(gizmo, handleId);
      this._coneIndex = this.Handle.Add3DShape((Shape3D) this._cone);
      this._pyramidIndex = this.Handle.Add3DShape((Shape3D) this._pyramid);
      this._boxIndex = this.Handle.Add3DShape((Shape3D) this._box);
      this._sphereIndex = this.Handle.Add3DShape((Shape3D) this._sphere);
      this._trPrismIndex = this.Handle.Add3DShape((Shape3D) this._trPrism);
      this.SetZoomFactorTransform(this._transform);
      this._controllerData.Gizmo = this.Gizmo;
      this._controllerData.Cap = this;
      this._controllerData.CapHandle = this.Handle;
      this._controllerData.Cone = this._cone;
      this._controllerData.ConeIndex = this._coneIndex;
      this._controllerData.Pyramid = this._pyramid;
      this._controllerData.PyramidIndex = this._pyramidIndex;
      this._controllerData.Box = this._box;
      this._controllerData.BoxIndex = this._boxIndex;
      this._controllerData.Sphere = this._sphere;
      this._controllerData.SphereIndex = this._sphereIndex;
      this._controllerData.TrPrism = this._trPrism;
      this._controllerData.TrPrismIndex = this._trPrismIndex;
      this._controllers[0] = (IGizmoCap3DController) new GizmoConeCap3DController(this._controllerData);
      this._controllers[1] = (IGizmoCap3DController) new GizmoPyramidCap3DController(this._controllerData);
      this._controllers[2] = (IGizmoCap3DController) new GizmoBoxCap3DController(this._controllerData);
      this._controllers[3] = (IGizmoCap3DController) new GizmoSphereCap3DController(this._controllerData);
      this._controllers[4] = (IGizmoCap3DController) new GizmoTriPrismCap3DController(this._controllerData);
      this._transform.Changed += new GizmoEntityTransformChangedHandler(this.OnTransformChanged);
      this._transform.SetParent(this.Gizmo.Transform);
      this.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
      this.Gizmo.PostEnabled += new GizmoPostEnabledHandler(this.OnGizmoPostEnabled);
      this.Gizmo.PostDisabled += new GizmoPostDisabledHandler(this.OnGizmoPostDisabled);
    }

    public void RegisterTransformAsDragTarget(IGizmoDragSession dragSession)
    {
      dragSession.AddTargetTransform(this._transform);
    }

    public void UnregisterTransformAsDragTarget(IGizmoDragSession dragSession)
    {
      dragSession.RemoveTargetTransform(this._transform);
    }

    public void AlignTransformAxis(int axisIndex, AxisSign axisSign, Vector3 axis)
    {
      this._transform.AlignAxis3D(axisIndex, axisSign, axis);
    }

    public void SetZoomFactorTransform(GizmoTransform transform)
    {
      this.Handle.SetZoomFactorTransform(transform);
    }

    public void CapSlider3D(Vector3 sliderDirection, Vector3 sliderEndPt)
    {
      this._controllers[(int) this.LookAndFeel.CapType].CapSlider3D(sliderDirection, sliderEndPt, this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    public void CapSlider3DInvert(Vector3 sliderDirection, Vector3 sliderEndPt)
    {
      this._controllers[(int) this.LookAndFeel.CapType].CapSlider3DInvert(sliderDirection, sliderEndPt, this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    public float GetSliderAlignedRealLength(float zoomFactor)
    {
      return this._controllers[(int) this.LookAndFeel.CapType].GetSliderAlignedRealLength(zoomFactor);
    }

    public float GetZoomFactor(Camera camera)
    {
      return !this.LookAndFeel.UseZoomFactor ? 1f : this.Handle.GetZoomFactor(camera);
    }

    public float GetRealConeHeight(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.ConeHeight * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealConeRadius(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.ConeRadius * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealPyramidWidth(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.PyramidWidth * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealPyramidDepth(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.PyramidDepth * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealPyramidHeight(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.PyramidHeight * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealBoxWidth(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.BoxWidth * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealBoxHeight(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.BoxHeight * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealBoxDepth(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.BoxDepth * this.LookAndFeel.Scale * zoomFactor;
    }

    public Vector3 GetRealBoxSize(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return new Vector3(this.LookAndFeel.BoxWidth, this.LookAndFeel.BoxHeight, this.LookAndFeel.BoxDepth) * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealSphereRadius(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.SphereRadius * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealTriPrismWidth(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.TrPrismWidth * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealTriPrismHeight(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.TrPrismHeight * this.LookAndFeel.Scale * zoomFactor;
    }

    public float GetRealTriPrismDepth(float zoomFactor)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        zoomFactor = 1f;
      return this.LookAndFeel.TrPrismDepth * this.LookAndFeel.Scale * zoomFactor;
    }

    public void ApplyZoomFactor(Camera camera)
    {
      if (!this.LookAndFeel.UseZoomFactor)
        return;
      this._controllers[(int) this.LookAndFeel.CapType].UpdateTransforms(this.GetZoomFactor(camera));
    }

    public override void Render(Camera camera)
    {
      if (!this.IsVisible)
        return;
      Color color1 = new Color();
      Color color2 = this.OverrideColor.IsActive ? this.OverrideColor.Color : (!this.Gizmo.IsHovered || this.Gizmo.HoverInfo.HandleId != this.HandleId ? this.LookAndFeel.Color : this.LookAndFeel.HoveredColor);
      if (this.LookAndFeel.FillMode == GizmoFillMode3D.Filled)
      {
        bool isLit = this.LookAndFeel.ShadeMode == GizmoShadeMode.Lit;
        GizmoSolidMaterial get = Singleton<GizmoSolidMaterial>.Get;
        get.ResetValuesToSensibleDefaults();
        get.SetLit(isLit);
        if (isLit)
          get.SetLightDirection(camera.transform.forward);
        get.SetColor(color2);
        get.SetPass(0);
        this.Handle.Render3DSolid();
      }
      else
      {
        GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
        get.ResetValuesToSensibleDefaults();
        get.SetColor(color2);
        get.SetPass(0);
        this.Handle.Render3DWire();
      }
      if (this.LookAndFeel.CapType != GizmoCap3DType.Sphere || !this.LookAndFeel.IsSphereBorderVisible)
        return;
      GizmoLineMaterial get1 = Singleton<GizmoLineMaterial>.Get;
      get1.ResetValuesToSensibleDefaults();
      get1.SetColor(this.LookAndFeel.SphereBorderColor);
      get1.SetPass(0);
      GLRenderer.DrawSphereBorder(camera, this.Position, this.GetRealSphereRadius(this.GetZoomFactor(camera)), this.LookAndFeel.NumSphereBorderPoints);
    }

    public void Refresh()
    {
      float zoomFactor = this.GetZoomFactor(this.Gizmo.GetWorkCamera());
      this._controllers[(int) this.LookAndFeel.CapType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.CapType].UpdateTransforms(zoomFactor);
    }

    protected override void OnVisibilityStateChanged()
    {
      this._controllers[(int) this.LookAndFeel.CapType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.CapType].UpdateTransforms(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    protected override void OnHoverableStateChanged() => this.Handle.SetHoverable(this.IsHoverable);

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      int capType = (int) this.LookAndFeel.CapType;
      this._controllers[capType].UpdateHandles();
      this._controllers[capType].UpdateTransforms(this.GetZoomFactor(this.Gizmo.FocusCamera));
    }

    private void OnTransformChanged(GizmoTransform transform, GizmoTransform.ChangeData changeData)
    {
      if (changeData.ChangeReason != GizmoTransform.ChangeReason.ParentChange && changeData.TRSDimension != GizmoDimension.Dim3D)
        return;
      this._controllers[(int) this.LookAndFeel.CapType].UpdateTransforms(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    private void OnGizmoPostEnabled(Gizmo gizmo) => this.Refresh();

    private void OnGizmoPostDisabled(Gizmo gizmo) => this.OverrideColor.IsActive = false;
  }
}
