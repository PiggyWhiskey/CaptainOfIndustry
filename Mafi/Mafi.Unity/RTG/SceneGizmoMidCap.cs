// Decompiled with JetBrains decompiler
// Type: RTG.SceneGizmoMidCap
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class SceneGizmoMidCap : SceneGizmoCap
  {
    public SceneGizmoMidCap(SceneGizmo sceneGizmo)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(sceneGizmo, GizmoHandleId.SceneGizmoMidCap);
      sceneGizmo.LookAndFeel.ConnectMidCapLookAndFeel(this._cap);
      sceneGizmo.Gizmo.PreHandlePicked += new GizmoPreHandlePickedHandler(this.OnGizmoHandlePicked);
      sceneGizmo.Gizmo.PreUpdateBegin += new GizmoPreUpdateBeginHandler(this.OnGizmoPreUpdateBegin);
    }

    public override void Render(Camera camera) => this._cap.Render(camera);

    private void OnGizmoPreUpdateBegin(Gizmo gizmo)
    {
      this._sceneGizmo.LookAndFeel.ConnectMidCapLookAndFeel(this._cap);
    }

    private void OnGizmoHandlePicked(Gizmo gizmo, int handleId)
    {
      if (handleId != this.HandleId)
        return;
      MonoSingleton<RTFocusCamera>.Get.PerformProjectionSwitch();
    }
  }
}
