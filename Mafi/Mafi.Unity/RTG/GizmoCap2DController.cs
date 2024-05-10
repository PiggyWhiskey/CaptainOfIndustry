// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCap2DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class GizmoCap2DController : IGizmoCap2DController
  {
    protected GizmoCap2DControllerData _data;

    public GizmoCap2DController(GizmoCap2DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._data = controllerData;
    }

    public abstract void UpdateHandles();

    public abstract void UpdateTransforms();

    public abstract void CapSlider2D(Vector2 sliderDirection, Vector2 sliderEndPt);

    public abstract void CapSlider2DInvert(Vector2 sliderDirection, Vector2 sliderEndPt);

    public abstract float GetSliderAlignedRealLength();
  }
}
