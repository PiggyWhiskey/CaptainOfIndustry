// Decompiled with JetBrains decompiler
// Type: RTG.GizmoThinCircle3DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoThinCircle3DBorderController : GizmoCircle3DBorderController
  {
    public GizmoThinCircle3DBorderController(GizmoCircle3DBorderControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderTorusIndex, false);
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderCylTorusIndex, false);
      this._data.TargetHandle.Set3DShapeVisible(this._data.BorderCircleIndex, this._data.Border.IsVisible);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.BorderCircle.WireEps = zoomFactor * this._data.PlaneSlider.Settings.BorderLineHoverEps;
      this._data.BorderCircle.ExtrudeEps = this._data.BorderCircle.WireEps;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      CircleShape3D targetCircle = this._data.TargetCircle;
      CircleShape3D borderCircle = this._data.BorderCircle;
      borderCircle.Rotation = targetCircle.Rotation;
      borderCircle.Center = targetCircle.Center;
      borderCircle.Radius = targetCircle.Radius;
    }
  }
}
