// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPolygon2DBorder
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
  public class GizmoPolygon2DBorder
  {
    private GizmoPlaneSlider2D _planeSlider;
    private GizmoHandle _targetHandle;
    private PolygonShape2D _targetPolygon;
    private bool _isVisible;
    private bool _isHoverable;
    private int _borderPolygonIndex;
    private int _thickBorderPolygonIndex;
    private PolygonShape2D _borderPolygon;
    private PolygonShape2D _thickBorderPolygon;
    private GizmoPolygon2DBorderControllerData _controllerData;
    private IGizmoPolygon2DBorderController[] _controllers;

    public bool IsVisible => this._isVisible;

    public bool IsHoverable => this._isHoverable;

    public GizmoPolygon2DBorder(
      GizmoPlaneSlider2D planeSlider,
      GizmoHandle targetHandle,
      PolygonShape2D targetPolygon)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      this._borderPolygon = new PolygonShape2D();
      this._thickBorderPolygon = new PolygonShape2D();
      this._controllerData = new GizmoPolygon2DBorderControllerData();
      this._controllers = new IGizmoPolygon2DBorderController[Enum.GetValues(typeof (GizmoPolygon2DBorderType)).Length];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._planeSlider = planeSlider;
      this._targetHandle = targetHandle;
      this._targetPolygon = targetPolygon;
      this._borderPolygonIndex = this._targetHandle.Add2DShape((Shape2D) this._borderPolygon);
      this._borderPolygon.PtContainMode = Shape2DPtContainMode.OnBorder;
      this._thickBorderPolygonIndex = this._targetHandle.Add2DShape((Shape2D) this._thickBorderPolygon);
      this._thickBorderPolygon.PtContainMode = Shape2DPtContainMode.OnBorder;
      this._thickBorderPolygon.BorderRenderDesc.BorderType = Shape2DBorderType.Thick;
      this._thickBorderPolygon.BorderRenderDesc.Direction = Shape2DBorderDirection.Outward;
      this._controllerData.Border = this;
      this._controllerData.PlaneSlider = this._planeSlider;
      this._controllerData.BorderPolygon = this._borderPolygon;
      this._controllerData.BorderPolygonIndex = this._borderPolygonIndex;
      this._controllerData.ThickBorderPolygon = this._thickBorderPolygon;
      this._controllerData.ThickBorderPolygonIndex = this._thickBorderPolygonIndex;
      this._controllerData.Gizmo = targetHandle.Gizmo;
      this._controllerData.TargetHandle = targetHandle;
      this._controllerData.TargetPolygon = this._targetPolygon;
      this._controllers[0] = (IGizmoPolygon2DBorderController) new GizmoThinPolygon2DBorderController(this._controllerData);
      this._controllers[1] = (IGizmoPolygon2DBorderController) new GizmoThickPolygon2DBorderController(this._controllerData);
      this._targetHandle.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
    }

    public void SetVisible(bool isVisible)
    {
      this._isVisible = isVisible;
      this._controllers[(int) this._planeSlider.LookAndFeel.PolygonBorderType].UpdateHandles();
      if (!this._isVisible)
        return;
      this._controllers[(int) this._planeSlider.LookAndFeel.PolygonBorderType].UpdateEpsilons();
      this.OnPolygonShapeChanged();
    }

    public void SetHoverable(bool isHoverable)
    {
      this._isHoverable = isHoverable;
      this._targetHandle.Set2DShapeHoverable(this._borderPolygonIndex, isHoverable);
      this._targetHandle.Set2DShapeHoverable(this._thickBorderPolygonIndex, isHoverable);
    }

    public void OnPolygonShapeChanged()
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.PolygonBorderType].UpdateTransforms();
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
      if (this._planeSlider.LookAndFeel.PolygonBorderType == GizmoPolygon2DBorderType.Thin)
      {
        this._targetHandle.Render2DWire(camera, this._borderPolygonIndex);
      }
      else
      {
        if (this._planeSlider.LookAndFeel.PolygonBorderType != GizmoPolygon2DBorderType.Thick)
          return;
        this._targetHandle.Render2DWire(camera, this._thickBorderPolygonIndex);
      }
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.PolygonBorderType].UpdateHandles();
      this._controllers[(int) this._planeSlider.LookAndFeel.PolygonBorderType].UpdateEpsilons();
    }
  }
}
