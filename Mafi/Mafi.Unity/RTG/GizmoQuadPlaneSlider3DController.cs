// Decompiled with JetBrains decompiler
// Type: RTG.GizmoQuadPlaneSlider3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoQuadPlaneSlider3DController : GizmoPlaneSlider3DController
  {
    public GizmoQuadPlaneSlider3DController(GizmoPlaneSlider3DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.RATriangleBorder.SetVisible(false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.RATriangleIndex, false);
      this._data.CircleBorder.SetVisible(false);
      this._data.SliderHandle.Set3DShapeVisible(this._data.CircleIndex, false);
      this._data.QuadBorder.SetVisible(this._data.Slider.IsBorderVisible);
      this._data.SliderHandle.Set3DShapeVisible(this._data.QuadIndex, this._data.Slider.IsVisible);
    }

    public override void UpdateEpsilons(float zoomFactor)
    {
      this._data.Quad.SizeEps = Vector2Ex.FromValue(this._data.Slider.Settings.AreaHoverEps * zoomFactor);
      this._data.Quad.ExtrudeEps = this._data.Slider.Settings.ExtrudeHoverEps * zoomFactor;
    }

    public override void UpdateTransforms(float zoomFactor)
    {
      QuadShape3D quad = this._data.Quad;
      GizmoPlaneSlider3D slider = this._data.Slider;
      quad.Center = slider.Position;
      quad.Rotation = slider.Rotation;
      quad.Size = slider.GetRealQuadSize(zoomFactor);
      this._data.QuadBorder.OnQuadShapeChanged();
    }
  }
}
