// Decompiled with JetBrains decompiler
// Type: RTG.IGizmoCap2DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public interface IGizmoCap2DController
  {
    void UpdateHandles();

    void UpdateTransforms();

    void CapSlider2D(Vector2 sliderDirection, Vector2 sliderEndPt);

    void CapSlider2DInvert(Vector2 sliderDirection, Vector2 sliderEndPt);

    float GetSliderAlignedRealLength();
  }
}
