// Decompiled with JetBrains decompiler
// Type: RTG.IGizmoCap3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public interface IGizmoCap3DController
  {
    void UpdateHandles();

    void UpdateTransforms(float zoomFactor);

    void CapSlider3D(Vector3 sliderDirection, Vector3 sliderEndPt, float zoomFactor);

    void CapSlider3DInvert(Vector3 sliderDirection, Vector3 sliderEndPt, float zoomFactor);

    float GetSliderAlignedRealLength(float zoomFactor);
  }
}
