// Decompiled with JetBrains decompiler
// Type: RTG.GizmoPlaneSlider2DController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class GizmoPlaneSlider2DController : IGizmoPlaneSlider2DController
  {
    protected GizmoPlaneSlider2DControllerData _data;

    public GizmoPlaneSlider2DController(GizmoPlaneSlider2DControllerData controllerData)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._data = controllerData;
    }

    public abstract void UpdateHandles();

    public abstract void UpdateTransforms();

    public abstract void UpdateEpsilons();

    public abstract Vector2 GetRealExtentPoint(Shape2DExtentPoint extentPt);
  }
}
