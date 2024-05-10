// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap2D
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
  public class GizmoCap2D : GizmoCap
  {
    private int _quadIndex;
    private QuadShape2D _quad;
    private int _circleIndex;
    private CircleShape2D _circle;
    private int _arrowIndex;
    private ConeShape2D _arrow;
    private GizmoTransform _transform;
    private GizmoOverrideColor _overrideFillColor;
    private GizmoOverrideColor _overrideBorderColor;
    private GizmoCap2DControllerData _controllerData;
    private IGizmoCap2DController[] _controllers;
    private GizmoCap2DLookAndFeel _lookAndFeel;
    private GizmoCap2DLookAndFeel _sharedLookAndFeel;

    public Vector2 Position
    {
      get => this._transform.Position2D;
      set => this._transform.Position2D = value;
    }

    public Quaternion Rotation => this._transform.Rotation2D;

    public float RotationDegrees
    {
      get => this._transform.Rotation2DDegrees;
      set => this._transform.Rotation2DDegrees = value;
    }

    public GizmoOverrideColor OverrideFillColor => this._overrideFillColor;

    public GizmoOverrideColor OverrideBorderColor => this._overrideBorderColor;

    public IGizmoDragSession DragSession
    {
      get => this.Handle.DragSession;
      set => this.Handle.DragSession = value;
    }

    public GizmoCap2DLookAndFeel LookAndFeel
    {
      get => this._sharedLookAndFeel == null ? this._lookAndFeel : this._sharedLookAndFeel;
    }

    public GizmoCap2DLookAndFeel SharedLookAndFeel
    {
      get => this._sharedLookAndFeel;
      set => this._sharedLookAndFeel = value;
    }

    public GizmoCap2D(Gizmo gizmo, int handleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._quad = new QuadShape2D();
      this._circle = new CircleShape2D();
      this._arrow = new ConeShape2D();
      this._transform = new GizmoTransform();
      this._overrideFillColor = new GizmoOverrideColor();
      this._overrideBorderColor = new GizmoOverrideColor();
      this._controllers = new IGizmoCap2DController[Enum.GetValues(typeof (GizmoCap2DType)).Length];
      this._lookAndFeel = new GizmoCap2DLookAndFeel();
      // ISSUE: explicit constructor call
      base.\u002Ector(gizmo, handleId);
      this._quadIndex = this.Handle.Add2DShape((Shape2D) this._quad);
      this._circleIndex = this.Handle.Add2DShape((Shape2D) this._circle);
      this._arrowIndex = this.Handle.Add2DShape((Shape2D) this._arrow);
      this._controllerData = new GizmoCap2DControllerData();
      this._controllerData.Cap = this;
      this._controllerData.CapHandle = this.Handle;
      this._controllerData.Gizmo = this.Gizmo;
      this._controllerData.Quad = this._quad;
      this._controllerData.QuadIndex = this._quadIndex;
      this._controllerData.Circle = this._circle;
      this._controllerData.CircleIndex = this._circleIndex;
      this._controllerData.Arrow = this._arrow;
      this._controllerData.ArrowIndex = this._arrowIndex;
      this._controllers[0] = (IGizmoCap2DController) new GizmoQuadCap2DController(this._controllerData);
      this._controllers[1] = (IGizmoCap2DController) new GizmoCircleCap2DController(this._controllerData);
      this._controllers[2] = (IGizmoCap2DController) new GizmoArrowCap2DController(this._controllerData);
      this._transform.SetParent(gizmo.Transform);
      this._transform.Changed += new GizmoEntityTransformChangedHandler(this.OnTransformChanged);
      this.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
      this.Gizmo.PostEnabled += new GizmoPostEnabledHandler(this.OnGizmoPostEnabled);
    }

    public void RegisterTransformAsDragTarget(IGizmoDragSession dragSession)
    {
      dragSession.AddTargetTransform(this._transform);
    }

    public void UnregisterTransformAsDragTarget(IGizmoDragSession dragSession)
    {
      dragSession.RemoveTargetTransform(this._transform);
    }

    public void AlignTransformAxis(int axisIndex, AxisSign axisSign, Vector2 axis)
    {
      this._transform.AlignAxis2D(axisIndex, axisSign, axis);
    }

    public float GetRealQuadWidth() => this.LookAndFeel.QuadWidth * this.LookAndFeel.Scale;

    public float GetRealQuadHeight() => this.LookAndFeel.QuadHeight * this.LookAndFeel.Scale;

    public float GetRealCircleRadius() => this.LookAndFeel.CircleRadius * this.LookAndFeel.Scale;

    public float GetRealArrowHeight() => this.LookAndFeel.ArrowHeight * this.LookAndFeel.Scale;

    public float GetRealArrowBaseRadius()
    {
      return this.LookAndFeel.ArrowBaseRadius * this.LookAndFeel.Scale;
    }

    public void CapSlider2D(Vector2 sliderDirection, Vector2 sliderEndPt)
    {
      this._controllers[(int) this.LookAndFeel.CapType].CapSlider2D(sliderDirection, sliderEndPt);
    }

    public void CapSlider2DInvert(Vector2 sliderDirection, Vector2 sliderEndPt)
    {
      this._controllers[(int) this.LookAndFeel.CapType].CapSlider2DInvert(sliderDirection, sliderEndPt);
    }

    public override void Render(Camera camera)
    {
      if (!this.IsVisible)
        return;
      if (this.LookAndFeel.FillMode == GizmoFillMode2D.FilledAndBorder || this.LookAndFeel.FillMode == GizmoFillMode2D.Filled)
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
      if (this.LookAndFeel.FillMode != GizmoFillMode2D.FilledAndBorder && this.LookAndFeel.FillMode != GizmoFillMode2D.Border)
        return;
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
      GizmoLineMaterial get1 = Singleton<GizmoLineMaterial>.Get;
      get1.ResetValuesToSensibleDefaults();
      get1.SetColor(color4);
      get1.SetPass(0);
      this.Handle.Render2DWire(camera);
    }

    public void Refresh()
    {
      this._controllers[(int) this.LookAndFeel.CapType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.CapType].UpdateTransforms();
    }

    protected override void OnVisibilityStateChanged()
    {
      this._controllers[(int) this.LookAndFeel.CapType].UpdateHandles();
      this._controllers[(int) this.LookAndFeel.CapType].UpdateTransforms();
    }

    protected override void OnHoverableStateChanged() => this.Handle.SetHoverable(this.IsHoverable);

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      int capType = (int) this.LookAndFeel.CapType;
      this._controllers[capType].UpdateHandles();
      this._controllers[capType].UpdateTransforms();
    }

    private void OnTransformChanged(GizmoTransform transform, GizmoTransform.ChangeData changeData)
    {
      if (changeData.TRSDimension != GizmoDimension.Dim2D && changeData.ChangeReason != GizmoTransform.ChangeReason.ParentChange)
        return;
      this._controllers[(int) this.LookAndFeel.CapType].UpdateTransforms();
    }

    private void OnGizmoPostEnabled(Gizmo gizmo) => this.Refresh();
  }
}
