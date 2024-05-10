// Decompiled with JetBrains decompiler
// Type: RTG.GizmoQuad3DBorder
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
  public class GizmoQuad3DBorder
  {
    private GizmoPlaneSlider3D _planeSlider;
    private GizmoHandle _targetHandle;
    private QuadShape3D _targetQuad;
    private bool _isVisible;
    private bool _isHoverable;
    private int _borderQuadIndex;
    private QuadShape3D _borderQuad;
    private int _topBoxIndex;
    private BoxShape3D _topBox;
    private int _rightBoxIndex;
    private BoxShape3D _rightBox;
    private int _bottomBoxIndex;
    private BoxShape3D _bottomBox;
    private int _leftBoxIndex;
    private BoxShape3D _leftBox;
    private int _topLeftBoxIndex;
    private BoxShape3D _topLeftBox;
    private int _topRightBoxIndex;
    private BoxShape3D _topRightBox;
    private int _bottomRightBoxIndex;
    private BoxShape3D _bottomRightBox;
    private int _bottomLeftBoxIndex;
    private BoxShape3D _bottomLeftBox;
    private List<int> _sortedBoxIndices;
    private GizmoQuad3DBorderControllerData _controllerData;
    private IGizmoQuad3DBorderController[] _controllers;

    public bool IsVisible => this._isVisible;

    public bool IsHoverable => this._isHoverable;

    public Gizmo Gizmo => this._targetHandle.Gizmo;

    public GizmoQuad3DBorder(
      GizmoPlaneSlider3D planeSlider,
      GizmoHandle targetHandle,
      QuadShape3D targetQuad)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this._isVisible = true;
      this._isHoverable = true;
      this._borderQuad = new QuadShape3D();
      this._topBox = new BoxShape3D();
      this._rightBox = new BoxShape3D();
      this._bottomBox = new BoxShape3D();
      this._leftBox = new BoxShape3D();
      this._topLeftBox = new BoxShape3D();
      this._topRightBox = new BoxShape3D();
      this._bottomRightBox = new BoxShape3D();
      this._bottomLeftBox = new BoxShape3D();
      this._sortedBoxIndices = new List<int>();
      this._controllerData = new GizmoQuad3DBorderControllerData();
      this._controllers = new IGizmoQuad3DBorderController[Enum.GetValues(typeof (GizmoQuad3DBorderType)).Length];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._planeSlider = planeSlider;
      this._targetHandle = targetHandle;
      this._targetQuad = targetQuad;
      this._borderQuadIndex = this._targetHandle.Add3DShape((Shape3D) this._borderQuad);
      this._borderQuad.RaycastMode = Shape3DRaycastMode.Wire;
      this._topBoxIndex = this._targetHandle.Add3DShape((Shape3D) this._topBox);
      this._rightBoxIndex = this._targetHandle.Add3DShape((Shape3D) this._rightBox);
      this._bottomBoxIndex = this._targetHandle.Add3DShape((Shape3D) this._bottomBox);
      this._leftBoxIndex = this._targetHandle.Add3DShape((Shape3D) this._leftBox);
      this._topLeftBoxIndex = this._targetHandle.Add3DShape((Shape3D) this._topLeftBox);
      this._topRightBoxIndex = this._targetHandle.Add3DShape((Shape3D) this._topRightBox);
      this._bottomRightBoxIndex = this._targetHandle.Add3DShape((Shape3D) this._bottomRightBox);
      this._bottomLeftBoxIndex = this._targetHandle.Add3DShape((Shape3D) this._bottomLeftBox);
      this._sortedBoxIndices.Add(this._topBoxIndex);
      this._sortedBoxIndices.Add(this._rightBoxIndex);
      this._sortedBoxIndices.Add(this._bottomBoxIndex);
      this._sortedBoxIndices.Add(this._leftBoxIndex);
      this._sortedBoxIndices.Add(this._topLeftBoxIndex);
      this._sortedBoxIndices.Add(this._topRightBoxIndex);
      this._sortedBoxIndices.Add(this._bottomRightBoxIndex);
      this._sortedBoxIndices.Add(this._bottomLeftBoxIndex);
      this._controllerData.Border = this;
      this._controllerData.PlaneSlider = this._planeSlider;
      this._controllerData.Gizmo = this.Gizmo;
      this._controllerData.TargetHandle = this._targetHandle;
      this._controllerData.TargetQuad = this._targetQuad;
      this._controllerData.BorderQuad = this._borderQuad;
      this._controllerData.TopBox = this._topBox;
      this._controllerData.RightBox = this._rightBox;
      this._controllerData.BottomBox = this._bottomBox;
      this._controllerData.LeftBox = this._leftBox;
      this._controllerData.TopLeftBox = this._topLeftBox;
      this._controllerData.TopRightBox = this._topRightBox;
      this._controllerData.BottomRightBox = this._bottomRightBox;
      this._controllerData.BottomLeftBox = this._bottomLeftBox;
      this._controllerData.BorderQuadIndex = this._borderQuadIndex;
      this._controllerData.TopBoxIndex = this._topBoxIndex;
      this._controllerData.RightBoxIndex = this._rightBoxIndex;
      this._controllerData.BottomBoxIndex = this._bottomBoxIndex;
      this._controllerData.LeftBoxIndex = this._leftBoxIndex;
      this._controllerData.TopLeftBoxIndex = this._topLeftBoxIndex;
      this._controllerData.TopRightBoxIndex = this._topRightBoxIndex;
      this._controllerData.BottomRightBoxIndex = this._bottomRightBoxIndex;
      this._controllerData.BottomLeftBoxIndex = this._bottomLeftBoxIndex;
      this._controllers[0] = (IGizmoQuad3DBorderController) new GizmoThinQuad3DBorderController(this._controllerData);
      this._controllers[1] = (IGizmoQuad3DBorderController) new GizmoBoxQuad3DBorderController(this._controllerData);
      this.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
    }

    public void SetVisible(bool isVisible)
    {
      this._isVisible = isVisible;
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateHandles();
      if (!this._isVisible)
        return;
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateEpsilons(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
      this.OnQuadShapeChanged();
    }

    public void SetHoverable(bool isHoverable)
    {
      this._isHoverable = isHoverable;
      this._targetHandle.Set3DShapeHoverable(this._borderQuadIndex, isHoverable);
      foreach (int sortedBoxIndex in this._sortedBoxIndices)
        this._targetHandle.Set3DShapeHoverable(sortedBoxIndex, isHoverable);
    }

    public float GetZoomFactor(Camera camera) => this._planeSlider.GetZoomFactor(camera);

    public float GetRealBoxHeight(float zoomFactor)
    {
      return this._planeSlider.LookAndFeel.BorderBoxHeight * zoomFactor * this._planeSlider.LookAndFeel.Scale;
    }

    public float GetRealBoxDepth(float zoomFactor)
    {
      return this._planeSlider.LookAndFeel.BorderBoxDepth * zoomFactor * this._planeSlider.LookAndFeel.Scale;
    }

    public void OnQuadShapeChanged()
    {
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateTransforms(this.GetZoomFactor(this.Gizmo.GetWorkCamera()));
    }

    public void Render(Camera camera)
    {
      if (!this.IsVisible)
        return;
      GizmoPlaneSlider3DLookAndFeel lookAndFeel = this._planeSlider.LookAndFeel;
      Color color = lookAndFeel.BorderColor;
      if (this.Gizmo.HoverHandleId == this._targetHandle.Id)
        color = lookAndFeel.HoveredBorderColor;
      if (lookAndFeel.QuadBorderType == GizmoQuad3DBorderType.Thin)
      {
        GizmoLineMaterial get = Singleton<GizmoLineMaterial>.Get;
        get.ResetValuesToSensibleDefaults();
        get.SetColor(color);
        get.SetPass(0);
        this._targetHandle.Render3DWire(this._borderQuadIndex);
      }
      else
      {
        GizmoSolidMaterial get = Singleton<GizmoSolidMaterial>.Get;
        get.ResetValuesToSensibleDefaults();
        get.SetColor(color);
        get.SetLit(lookAndFeel.BorderShadeMode == GizmoShadeMode.Lit);
        if (get.IsLit)
          get.SetLightDirection(camera.transform.forward);
        get.SetPass(0);
        Vector3 camPos = camera.transform.position;
        this._sortedBoxIndices.Sort((Comparison<int>) ((i0, i1) =>
        {
          BoxShape3D boxShape3D = this._targetHandle.Get3DShape(i0) as BoxShape3D;
          return ((this._targetHandle.Get3DShape(i1) as BoxShape3D).Center - camPos).sqrMagnitude.CompareTo((boxShape3D.Center - camPos).sqrMagnitude);
        }));
        if (lookAndFeel.BorderFillMode == GizmoFillMode3D.Filled)
        {
          foreach (int sortedBoxIndex in this._sortedBoxIndices)
            this._targetHandle.Render3DSolid(sortedBoxIndex);
        }
        else
        {
          foreach (int sortedBoxIndex in this._sortedBoxIndices)
            this._targetHandle.Render3DWire(sortedBoxIndex);
        }
      }
    }

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      float zoomFactor = this.GetZoomFactor(this.Gizmo.FocusCamera);
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateHandles();
      this._controllers[(int) this._planeSlider.LookAndFeel.QuadBorderType].UpdateEpsilons(zoomFactor);
    }
  }
}
