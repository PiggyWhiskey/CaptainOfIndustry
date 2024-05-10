// Decompiled with JetBrains decompiler
// Type: RTG.IGizmoBehaviour
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public interface IGizmoBehaviour
  {
    Gizmo Gizmo { get; }

    bool IsEnabled { get; }

    void Init_SystemCall(GizmoBehaviorInitParams initParams);

    void SetEnabled(bool enabled);

    void OnAttached();

    void OnDetached();

    void OnEnabled();

    void OnDisabled();

    void OnGizmoEnabled();

    void OnGizmoDisabled();

    void OnGizmoHandlePicked(int handleId);

    bool OnGizmoCanBeginDrag(int handleId);

    void OnGizmoAttemptHandleDragBegin(int handleId);

    void OnGizmoHoverEnter(int handleId);

    void OnGizmoHoverExit(int handleId);

    void OnGizmoDragBegin(int handleId);

    void OnGizmoDragUpdate(int handleId);

    void OnGizmoDragEnd(int handleId);

    void OnGizmoUpdateBegin();

    void OnGizmoUpdateEnd();

    void OnGUI();

    void OnGizmoRender(Camera camera);
  }
}
