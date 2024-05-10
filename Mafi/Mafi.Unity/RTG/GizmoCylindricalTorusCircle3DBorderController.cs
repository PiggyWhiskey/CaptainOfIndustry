// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCylindricalTorusCircle3DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoCylindricalTorusCircle3DBorderController : GizmoCircle3DBorderController
  {
    public GizmoCylindricalTorusCircle3DBorderController(
      GizmoCircle3DBorderControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderCircleIndex, false);
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderTorusIndex, false);
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderCylTorusIndex, this._data.Border.IsVisible);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.BorderCylTorus.CylHrzRadiusEps = zoomFactor * this._data.PlaneSlider.Settings.BorderTorusHoverEps;
      this._data.BorderCylTorus.CylVertRadiusEps = this._data.BorderCylTorus.CylHrzRadiusEps;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      CylTorusShape3D borderCylTorus = this._data.BorderCylTorus;
      CircleShape3D targetCircle = this._data.TargetCircle;
      borderCylTorus.Rotation = targetCircle.Rotation * Quaternion.Euler(-90f, 0.0f, 0.0f);
      borderCylTorus.Center = targetCircle.Center;
      borderCylTorus.CoreRadius = this.GetTorusCoreRadius(zoomFactor);
      borderCylTorus.HrzRadius = this._data.Border.GetRealCylTorusWidth(zoomFactor) * 0.5f;
      borderCylTorus.VertRadius = this._data.Border.GetRealCylTorusHeight(zoomFactor) * 0.5f;
    }

    public float GetTorusCoreRadius(float zoomFactor)
    {
      return this._data.TargetCircle.Radius - this._data.Border.GetRealCylTorusWidth(zoomFactor) * 0.5f;
    }
  }
}
