// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCirclePlaneSlider3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoCirclePlaneSlider3DController : GizmoPlaneSlider3DController
  {
    public GizmoCirclePlaneSlider3DController(GizmoPlaneSlider3DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.QuadBorder.SetVisible(false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.QuadIndex, false);
      this._data.RATriangleBorder.SetVisible(false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.RATriangleIndex, false);
      this._data.CircleBorder.SetVisible(this._data.Slider.IsBorderVisible);
      this._data.SliderHandle.Set3DShapeVisible(this._data.CircleIndex, this._data.Slider.IsVisible);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.Circle.RadiusEps = (float) ((double) this._data.Slider.Settings.AreaHoverEps * (double) zoomFactor * 0.5);
      this._data.Circle.ExtrudeEps = this._data.Slider.Settings.ExtrudeHoverEps * zoomFactor;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      CircleShape3D circle = this._data.Circle;
      GizmoPlaneSlider3D slider = this._data.Slider;
      circle.Center = slider.Position;
      circle.Radius = slider.GetRealCircleRadius(zoomFactor);
      circle.Rotation = slider.Rotation;
      this._data.CircleBorder.OnCircleShapeChanged();
    }
  }
}
