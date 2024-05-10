// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCircle3DBorder
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
  public class GizmoCircle3DBorder
  {
    private GizmoPlaneSlider3D _planeSlider;
    private GizmoHandle _targetHandle;
    private CircleShape3D _targetCircle;
    private bool _isVisible;
    private bool _isHoverable;
    private int _borderCircleIndex;
    private int _borderTorusIndex;
    private int _borderCylTorusIndex;
    private CircleShape3D _borderCircle;
    private TorusShape3D _borderTorus;
    private CylTorusShape3D _borderCylTorus;
    private GizmoCircle3DBorderControllerData _controllerData;
    private IGizmoCircle3DBorderController[] _controllers;

    public bool IsVisible => this._isVisible;

    public bool IsHoverable => this._isHoverable;

    public Gizmo Gizmo => this._targetHandle.Gizmo;

    public GizmoCircle3DBorder(
      GizmoPlaneSlider3D planeSlider,
      GizmoHandle targetHandle,
      CircleShape3D targetCircle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      this._borderCircle = new CircleShape3D();
      this._borderTorus = new TorusShape3D();
      this._borderCylTorus = new CylTorusShape3D();
      this._controllerData = new GizmoCircle3DBorderControllerData();
      this._controllers = new IGizmoCircle3DBorderController[Enum.GetValues(typeof (GizmoCircle3DBorderType)).Length];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._planeSlider = planeSlider;
      this._targetHandle = targetHandle;
      this._targetCircle = targetCircle;
      this._borderCircleIndex = this._targetHandle.Add3DShape((Shape3D) this._borderCircle);
      this._borderCircle.RaycastMode = Shape3DRaycastMode.Wire;
      this._borderTorusIndex = this._targetHandle.Add3DShape((Shape3D) this._borderTorus);
      this._borderTorus.WireRenderDesc.NumTubeSlices = 0;
      this._borderCylTorusIndex = this._targetHandle.Add3DShape((Shape3D) this._borderCylTorus);
      this._controllerData.Border = this;
      this._controllerData.PlaneSlider = this._planeSlider;
      this._controllerData.Gizmo = this.Gizmo;
      this._controllerData.TargetHandle = this._targetHandle;
      this._controllerData.TargetCircle = targetCircle;
      this._controllerData.BorderCircle = this._borderCircle;
      this._controllerData.BorderTorus = this._borderTorus;
      this._controllerData.BorderCylTorus = this._borderCylTorus;
      this._controllerData.BorderCircleIndex = this._borderCircleIndex;
      this._controllerData.BorderTorusIndex = this._borderTorusIndex;
      this._controllerData.BorderCylTorusIndex = this._borderCylTorusIndex;
      this._controllers[0] = (IGizmoCircle3DBorderController) new GizmoThinCircle3DBorderController(this._controllerData);
      this._controllers[1] = (IGizmoCircle3DBorderController) new GizmoTorusCircle3DBorderController(this._controllerData);
      this._controllers[2] = (IGizmoCircle3DBorderController) new GizmoCylindricalTorusCircle3DBorderController(this._controllerData);
      this.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
    }

    public void SetVisible(bool isVisible)
    {
      this._isVisible = isVisible;
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateHandles();
      if (!this._isVisible)
        return;
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateEpsilons(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
      this.OnCircleShapeChanged();
    }

    public void SetHoverable(bool isHoverable)
    {
      this._isHoverable = isHoverable;
      this._targetHandle.Set3DShapeHoverable(this._borderCircleIndex, isHoverable);
      this._targetHandle.Set3DShapeHoverable(this._borderTorusIndex, isHoverable);
      this._targetHandle.Set3DShapeHoverable(this._borderCylTorusIndex, isHoverable);
    }

    public float GetZoomFactor(Camera camera) => this._planeSlider.GetZoomFactor(camera);

    public float GetRealTorusThickness(float zoomFactor)
    {
      return this._planeSlider.LookAndFeel.BorderTorusThickness * zoomFactor * this._planeSlider.LookAndFeel.Scale;
    }

    public float GetRealCylTorusWidth(float zoomFactor)
    {
      return this._planeSlider.LookAndFeel.BorderCylTorusWidth * zoomFactor * this._planeSlider.LookAndFeel.Scale;
    }

    public float GetRealCylTorusHeight(float zoomFactor)
    {
      return this._planeSlider.LookAndFeel.BorderCylTorusHeight * zoomFactor * this._planeSlider.LookAndFeel.Scale;
    }

    public void OnCircleShapeChanged()
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateTransforms(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    public void Render(Camera camera)
    {
      if (!this.IsVisible)
        return;
      Color color = this._planeSlider.LookAndFeel.BorderColor;
      if (this._targetHandle.Gizmo.HoverHandleId == this._targetHandle.Id)
        color = this._planeSlider.LookAndFeel.HoveredBorderColor;
      if (this._planeSlider.LookAndFeel.CircleBorderType == GizmoCircle3DBorderType.Thin)
      {
        GizmoCircularMaterial get = Singleton<GizmoCircularMaterial>.Get;
        get.CircularType = GizmoCircularMaterial.Type.Circle;
        get.ResetValuesToSensibleDefaults();
        get.SetCamera(camera);
        get.SetShapeCenter(this._targetCircle.Center);
        get.SetCullAlphaScale(this._planeSlider.LookAndFeel.BorderCircleCullAlphaScale);
        get.SetColor(color);
        get.SetPass(0);
        this._targetHandle.Render3DWire(this._borderCircleIndex);
      }
      else if (this._planeSlider.LookAndFeel.CircleBorderType == GizmoCircle3DBorderType.Torus)
      {
        float zoomFactor = this.GetZoomFactor(camera);
        float realTorusThickness = this.GetRealTorusThickness(zoomFactor);
        bool flag = this._planeSlider.LookAndFeel.BorderFillMode == GizmoFillMode3D.Filled;
        GizmoCircularMaterial get = Singleton<GizmoCircularMaterial>.Get;
        get.CircularType = flag ? GizmoCircularMaterial.Type.Torus : GizmoCircularMaterial.Type.Circle;
        get.ResetValuesToSensibleDefaults();
        get.SetCamera(camera);
        get.SetShapeCenter(this._targetCircle.Center);
        get.SetCullAlphaScale(this._planeSlider.LookAndFeel.BorderCircleCullAlphaScale);
        get.SetColor(color);
        get.SetTorusCoreRadius((this._controllers[1] as GizmoTorusCircle3DBorderController).GetTorusCoreRadius(zoomFactor));
        get.SetTorusTubeRadius(realTorusThickness * 0.5f);
        get.SetLit(this._planeSlider.LookAndFeel.BorderShadeMode == GizmoShadeMode.Lit);
        if (get.IsLit)
          get.SetLightDirection(camera.transform.forward);
        get.SetPass(0);
        if (flag)
        {
          this._targetHandle.Render3DSolid(this._borderTorusIndex);
        }
        else
        {
          this._borderTorus.WireRenderDesc.NumAxialSlices = this._planeSlider.LookAndFeel.NumBorderTorusWireAxialSlices;
          this._targetHandle.Render3DWire(this._borderTorusIndex);
        }
      }
      else
      {
        if (this._planeSlider.LookAndFeel.CircleBorderType != GizmoCircle3DBorderType.CylindricalTorus)
          return;
        float zoomFactor = this.GetZoomFactor(camera);
        float realCylTorusWidth = this.GetRealCylTorusWidth(zoomFactor);
        float realCylTorusHeight = this.GetRealCylTorusHeight(zoomFactor);
        bool flag = this._planeSlider.LookAndFeel.BorderFillMode == GizmoFillMode3D.Filled;
        GizmoCircularMaterial get = Singleton<GizmoCircularMaterial>.Get;
        get.CircularType = flag ? GizmoCircularMaterial.Type.CylindricalTorus : GizmoCircularMaterial.Type.Circle;
        get.ResetValuesToSensibleDefaults();
        get.SetCamera(camera);
        get.SetShapeCenter(this._targetCircle.Center);
        get.SetCullAlphaScale(this._planeSlider.LookAndFeel.BorderCircleCullAlphaScale);
        get.SetColor(color);
        get.SetTorusCoreRadius((this._controllers[2] as GizmoCylindricalTorusCircle3DBorderController).GetTorusCoreRadius(zoomFactor));
        get.SetCylindricalTorusRadii(realCylTorusWidth * 0.5f, realCylTorusHeight * 0.5f);
        get.SetLit(this._planeSlider.LookAndFeel.BorderShadeMode == GizmoShadeMode.Lit);
        if (get.IsLit)
          get.SetLightDirection(camera.transform.forward);
        get.SetPass(0);
        if (flag)
          this._targetHandle.Render3DSolid(this._borderCylTorusIndex);
        else
          this._targetHandle.Render3DWire(this._borderCylTorusIndex);
      }
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      float zoomFactor = this.GetZoomFactor(this.Gizmo.FocusCamera);
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateHandles();
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateEpsilons(zoomFactor);
    }
  }
}
