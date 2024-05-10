// Decompiled with JetBrains decompiler
// Type: RTG.GizmoRATriangle3DBorder
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
  public class GizmoRATriangle3DBorder
  {
    private GizmoPlaneSlider3D _planeSlider;
    private GizmoHandle _targetHandle;
    private RightAngTriangle3D _targetTriangle;
    private bool _isVisible;
    private bool _isHoverable;
    private int _borderTriangleIndex;
    private RightAngTriangle3D _borderTriangle;
    private GizmoRATriangle3DBorderControllerData _controllerData;
    private IGizmoRATriangle3DBorderController[] _controllers;

    public bool IsVisible => this._isVisible;

    public bool IsHoverable => this._isHoverable;

    public Gizmo Gizmo => this._targetHandle.Gizmo;

    public GizmoRATriangle3DBorder(
      GizmoPlaneSlider3D planeSlider,
      GizmoHandle targetHandle,
      RightAngTriangle3D targetRiangle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      this._borderTriangle = new RightAngTriangle3D();
      this._controllerData = new GizmoRATriangle3DBorderControllerData();
      this._controllers = new IGizmoRATriangle3DBorderController[Enum.GetValues(typeof (GizmoRATriangle3DBorderType)).Length];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._planeSlider = planeSlider;
      this._targetHandle = targetHandle;
      this._targetTriangle = targetRiangle;
      this._borderTriangleIndex = this._targetHandle.Add3DShape((Shape3D) this._borderTriangle);
      this._borderTriangle.RaycastMode = Shape3DRaycastMode.Wire;
      this._controllerData.Border = this;
      this._controllerData.PlaneSlider = this._planeSlider;
      this._controllerData.Gizmo = this._targetHandle.Gizmo;
      this._controllerData.TargetHandle = this._targetHandle;
      this._controllerData.TargetTriangle = this._targetTriangle;
      this._controllerData.BorderTriangle = this._borderTriangle;
      this._controllerData.BorderTriangleIndex = this._borderTriangleIndex;
      this._controllers[0] = (IGizmoRATriangle3DBorderController) new GizmoThinRATriangle3DBorderController(this._controllerData);
      this._targetHandle.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
    }

    public void SetVisible(bool isVisible)
    {
      this._isVisible = isVisible;
      this._controllers[(int) this._planeSlider.LookAndFeel.RATriangleBorderType].UpdateHandles();
      if (!this._isVisible)
        return;
      this._controllers[(int) this._planeSlider.LookAndFeel.RATriangleBorderType].UpdateEpsilons(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
      this.OnTriangleShapeChanged();
    }

    public void SetHoverable(bool isHoverable)
    {
      this._isHoverable = isHoverable;
      this._targetHandle.Set3DShapeHoverable(this._borderTriangleIndex, isHoverable);
    }

    public float GetZoomFactor(Camera camera) => this._planeSlider.GetZoomFactor(camera);

    public void OnTriangleShapeChanged()
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.RATriangleBorderType].UpdateTransforms(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    public void Render(Camera camera)
    {
      if (!this.IsVisible)
        return;
      Color color = this._planeSlider.LookAndFeel.BorderColor;
      if (this._targetHandle.Gizmo.HoverHandleId == this._targetHandle.Id)
        color = this._planeSlider.LookAndFeel.HoveredBorderColor;
      if (this._planeSlider.LookAndFeel.RATriangleBorderType != GizmoRATriangle3DBorderType.Thin)
        return;
      GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
      get.ResetValuesToSensibleDefaults();
      get.SetColor(color);
      get.SetPass(0);
      this._targetHandle.Render3DWire(this._borderTriangleIndex);
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      float zoomFactor = this.GetZoomFactor(this.Gizmo.FocusCamera);
      this._controllers[(int) this._planeSlider.LookAndFeel.RATriangleBorderType].UpdateHandles();
      this._controllers[(int) this._planeSlider.LookAndFeel.RATriangleBorderType].UpdateEpsilons(zoomFactor);
    }
  }
}
