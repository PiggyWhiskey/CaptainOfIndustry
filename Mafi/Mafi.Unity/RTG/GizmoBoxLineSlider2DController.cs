// Decompiled with JetBrains decompiler
// Type: RTG.GizmoBoxLineSlider2DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class GizmoBoxLineSlider2DController : GizmoLineSlider2DController
  {
    public GizmoBoxLineSlider2DController(GizmoLineSlider2DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(controllerData);
    }

    public override void UpdateHandles()
    {
      this._data.SliderHandle.Set2DShapeVisible(this._data.SegmentIndex, false);
      this._data.SliderHandle.Set2DShapeVisible(this._data.QuadIndex, this._data.Slider.IsVisible);
    }

    public override void UpdateEpsilons()
    {
      this._data.Quad.SizeEps = Vector2Ex.FromValue(this._data.Slider.Settings.BoxHoverEps);
    }

    public override void UpdateTransforms()
    {
      QuadShape2D quad = this._data.Quad;
      GizmoLineSlider2D slider = this._data.Slider;
      float realLength = slider.GetRealLength();
      Vector2 realDirection = slider.GetRealDirection();
      quad.Width = realLength;
      quad.Height = slider.GetRealBoxThickness();
      quad.AlignWidth(realDirection);
      quad.Center = slider.StartPosition + realDirection * 0.5f * realLength;
    }
  }
}
