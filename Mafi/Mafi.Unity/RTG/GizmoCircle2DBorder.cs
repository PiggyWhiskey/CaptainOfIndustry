// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCircle2DBorder
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
  public class GizmoCircle2DBorder
  {
    private GizmoPlaneSlider2D _planeSlider;
    private GizmoHandle _targetHandle;
    private CircleShape2D _targetCircle;
    private bool _isVisible;
    private bool _isHoverable;
    private int _borderCircleIndex;
    private CircleShape2D _borderCircle;
    private GizmoCircle2DBorderControllerData _controllerData;
    private IGizmoCircle2DBorderController[] _controllers;

    public bool IsVisible => this._isVisible;

    public bool IsHoverable => this._isHoverable;

    public GizmoCircle2DBorder(
      GizmoPlaneSlider2D planeSlider,
      GizmoHandle targetHandle,
      CircleShape2D targetCircle)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      this._borderCircle = new CircleShape2D();
      this._controllerData = new GizmoCircle2DBorderControllerData();
      this._controllers = new IGizmoCircle2DBorderController[Enum.GetValues(typeof (GizmoCircle2DBorderType)).Length];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._planeSlider = planeSlider;
      this._targetHandle = targetHandle;
      this._targetCircle = targetCircle;
      this._borderCircleIndex = this._targetHandle.Add2DShape((Shape2D) this._borderCircle);
      this._borderCircle.PtContainMode = Shape2DPtContainMode.OnBorder;
      this._controllerData.Border = this;
      this._controllerData.PlaneSlider = this._planeSlider;
      this._controllerData.BorderCircle = this._borderCircle;
      this._controllerData.BorderCircleIndex = this._borderCircleIndex;
      this._controllerData.Gizmo = targetHandle.Gizmo;
      this._controllerData.TargetHandle = targetHandle;
      this._controllerData.TargetCircle = this._targetCircle;
      this._controllers[0] = (IGizmoCircle2DBorderController) new GizmoThinCircle2DBorderController(this._controllerData);
      this._targetHandle.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
    }

    public void SetVisible(bool isVisible)
    {
      this._isVisible = isVisible;
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateHandles();
      if (!this._isVisible)
        return;
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateEpsilons();
      this.OnCircleShapeChanged();
    }

    public void SetHoverable(bool isHoverable)
    {
      this._isHoverable = isHoverable;
      this._targetHandle.Set2DShapeHoverable(this._borderCircleIndex, isHoverable);
    }

    public void OnCircleShapeChanged()
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateTransforms();
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
      this._targetHandle.Render2DWire(camera, this._borderCircleIndex);
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateHandles();
      this._controllers[(int) this._planeSlider.LookAndFeel.CircleBorderType].UpdateEpsilons();
    }
  }
}
