// Decompiled with JetBrains decompiler
// Type: RTG.IGizmoDragSession
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public interface IGizmoDragSession
  {
    bool IsActive { get; }

    GizmoDragChannel DragChannel { get; }

    Vector3 TotalDragOffset { get; }

    Quaternion TotalDragRotation { get; }

    Vector3 TotalDragScale { get; }

    Vector3 RelativeDragOffset { get; }

    Quaternion RelativeDragRotation { get; }

    Vector3 RelativeDragScale { get; }

    bool ContainsTargetTransform(GizmoTransform transform);

    void AddTargetTransform(GizmoTransform transform);

    void RemoveTargetTransform(GizmoTransform transform);

    bool Begin();

    bool Update();

    void End();
  }
}
