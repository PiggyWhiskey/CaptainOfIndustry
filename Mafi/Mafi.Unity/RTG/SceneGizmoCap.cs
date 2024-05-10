// Decompiled with JetBrains decompiler
// Type: RTG.SceneGizmoCap
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public abstract class SceneGizmoCap
  {
    protected SceneGizmo _sceneGizmo;
    protected GizmoCap3D _cap;

    public int HandleId => this._cap.HandleId;

    public Vector3 Position => this._cap.Position;

    public SceneGizmoCap(SceneGizmo sceneGizmo, int capHandleId)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this._sceneGizmo = sceneGizmo;
      this._cap = new GizmoCap3D(sceneGizmo.Gizmo, capHandleId);
    }

    public void SetHoverable(bool isHoverable) => this._cap.SetHoverable(isHoverable);

    public abstract void Render(Camera camera);
  }
}
