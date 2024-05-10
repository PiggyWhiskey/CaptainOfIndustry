// Decompiled with JetBrains decompiler
// Type: RTG.GizmoCircle2DBorderController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class GizmoCircle2DBorderController : IGizmoCircle2DBorderController
  {
    protected GizmoCircle2DBorderControllerData _data;

    public GizmoCircle2DBorderController(GizmoCircle2DBorderControllerData data)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._data = data;
    }

    public abstract void UpdateHandles();

    public abstract void UpdateEpsilons();

    public abstract void UpdateTransforms();
  }
}
