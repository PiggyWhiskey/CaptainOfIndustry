// Decompiled with JetBrains decompiler
// Type: RTG.GizmoQuad3DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class GizmoQuad3DBorderController : IGizmoQuad3DBorderController
  {
    protected GizmoQuad3DBorderControllerData _data;

    public GizmoQuad3DBorderController(GizmoQuad3DBorderControllerData data)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._data = data;
    }

    public abstract void UpdateHandles();

    public abstract void UpdateEpsilons(float zoomFactor);

    public abstract void UpdateTransforms(float zoomFactor);
  }
}
