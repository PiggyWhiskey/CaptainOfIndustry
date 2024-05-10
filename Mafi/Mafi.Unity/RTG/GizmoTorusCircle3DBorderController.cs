// Decompiled with JetBrains decompiler
// Type: RTG.GizmoTorusCircle3DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoTorusCircle3DBorderController : GizmoCircle3DBorderController
  {
    public GizmoTorusCircle3DBorderController(GizmoCircle3DBorderControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderCircleIndex, false);
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderCylTorusIndex, false);
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderTorusIndex, this._data.Border.IsVisible);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.BorderTorus.TubeRadiusEps = zoomFactor * this._data.PlaneSlider.Settings.BorderTorusHoverEps;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      TorusShape3D borderTorus = this._data.BorderTorus;
      CircleShape3D targetCircle = this._data.TargetCircle;
      float realTorusThickness = this._data.Border.GetRealTorusThickness(zoomFactor);
      borderTorus.Rotation = targetCircle.Rotation * Quaternion.Euler(-90f, 0.0f, 0.0f);
      borderTorus.Center = targetCircle.Center;
      borderTorus.CoreRadius = this.GetTorusCoreRadius(zoomFactor);
      borderTorus.TubeRadius = realTorusThickness * 0.5f;
    }

    public float GetTorusCoreRadius(float zoomFactor)
    {
      return this._data.TargetCircle.Radius - this._data.Border.GetRealTorusThickness(zoomFactor) * 0.5f;
    }
  }
}
