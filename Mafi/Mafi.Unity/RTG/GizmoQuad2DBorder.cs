// Decompiled with JetBrains decompiler
// Type: RTG.GizmoQuad2DBorder
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
  public class GizmoQuad2DBorder
  {
    private GizmoPlaneSlider2D _planeSlider;
    private GizmoHandle _targetHandle;
    private QuadShape2D _targetQuad;
    private bool _isVisible;
    private bool _isHoverable;
    private int _borderQuadIndex;
    private QuadShape2D _borderQuad;
    private GizmoQuad2DBorderControllerData _controllerData;
    private IGizmoQuad2DBorderController[] _controllers;

    public bool IsVisible => this._isVisible;

    public bool IsHoverable => this._isHoverable;

    public GizmoQuad2DBorder(
      GizmoPlaneSlider2D planeSlider,
      GizmoHandle targetHandle,
      QuadShape2D targetQuad)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      this._borderQuad = new QuadShape2D();
      this._controllerData = new GizmoQuad2DBorderControllerData();
      this._controllers = new IGizmoQuad2DBorderController[Enum.GetValues(typeof (GizmoQuad2DBorderType)).Length];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._planeSlider = planeSlider;
      this._targetHandle = targetHandle;
      this._targetQuad = targetQuad;
      this._borderQuadIndex = this._targetHandle.Add2DShape((Shape2D) this._borderQuad);
      this._borderQuad.PtContainMode = Shape2DPtContainMode.OnBorder;
      this._controllerData.Border = this;
      this._controllerData.PlaneSlider = this._planeSlider;
      this._controllerData.BorderQuad = this._borderQuad;
      this._controllerData.BorderQuadIndex = this._borderQuadIndex;
      this._controllerData.Gizmo = targetHandle.Gizmo;
      this._controllerData.TargetHandle = targetHandle;
      this._controllerData.TargetQuad = this._targetQuad;
      this._controllers[0] = (IGizmoQuad2DBorderController) new GizmoThinQuad2DBorderController(this._controllerData);
      this._targetHandle.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
    }

    public void SetVisible(bool isVisible)
    {
      this._isVisible = isVisible;
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateHandles();
      if (!this._isVisible)
        return;
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateEpsilons();
      this.OnQuadShapeChanged();
    }

    public void SetHoverable(bool isHoverable)
    {
      this._isHoverable = isHoverable;
      this._targetHandle.Set2DShapeHoverable(this._borderQuadIndex, isHoverable);
    }

    public void OnQuadShapeChanged()
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateTransforms();
    }

    public void Render(Camera camera)
    {
      if (!this.IsVisible)
        return;
      Color color = this._planeSlider.LookAndFeel.BorderColor;
      if (this._targetHandle.Gizmo.HoverHandleId == this._targetHandle.Id)
        color = this._planeSlider.LookAndFeel.HoveredBorderColor;
      GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
      get.ResetValuesToSensibleDefaults();
      get.SetColor(color);
      get.SetPass(0);
      this._targetHandle.Render2DWire(camera, this._borderQuadIndex);
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateHandles();
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateEpsilons();
    }
  }
}
