// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap3DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class GizmoCap3DController : IGizmoCap3DController
  {
    protected GizmoCap3DControllerData _data;

    public GizmoCap3DController(GizmoCap3DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._data = controllerData;
    }

    public abstract void UpdateHandles();

    public abstract void UpdateTransforms(float zoomFactor);

    public abstract void CapSlider3D(
      Vector3 sliderDirection,
      Vector3 sliderEndPt,
      float zoomFactor);

    public abstract void CapSlider3DInvert(
      Vector3 sliderDirection,
      Vector3 sliderEndPt,
      float zoomFactor);

    public abstract float GetSliderAlignedRealLength(float zoomFactor);
  }
}
