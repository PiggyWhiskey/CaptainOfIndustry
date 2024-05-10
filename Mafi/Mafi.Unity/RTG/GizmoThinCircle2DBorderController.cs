// Decompiled with JetBrains decompiler
// Type: RTG.GizmoThinCircle2DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoThinCircle2DBorderController : GizmoCircle2DBorderController
  {
    public GizmoThinCircle2DBorderController(GizmoCircle2DBorderControllerData data)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(data);
    }

    public override void UpdateHandles()
    {
      this._data.TargetHandle.Set2DShapeVisible(this._data.BorderCircleIndex, this._data.Border.IsVisible);
    }

    public override void UpdateEpsilons()
    {
      this._data.BorderCircle.WireEps = this._data.PlaneSlider.Settings.BorderLineHoverEps;
    }

    public override void UpdateTransforms()
    {
      CircleShape2D targetCircle = this._data.TargetCircle;
      CircleShape2D borderCircle = this._data.BorderCircle;
      borderCircle.Center = targetCircle.Center;
      borderCircle.RotationDegrees = targetCircle.RotationDegrees;
      borderCircle.Radius = targetCircle.Radius;
    }
  }
}
