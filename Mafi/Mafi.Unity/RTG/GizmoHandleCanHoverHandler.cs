// Decompiled with JetBrains decompiler
// Type: RTG.GizmoHandleCanHoverHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

#nullable disable
namespace RTG
{
  /// <summary>
  /// Delegate which is used to handle CanHover events fired from the GizmoHandle class.
  /// </summary>
  /// <param name="handleId">The handle id.</param>
  /// <param name="ownerGizmo">The gizmo which owns the handle.</param>
  /// <param name="handleHoverData">Contains useful information about the handle hover state.</param>
  /// <param name="answer">
  /// All handlers must answer with either yes or no to tell the handle if it can be hovered.
  /// </param>
  public delegate void GizmoHandleCanHoverHandler(
    int handleId,
    Gizmo ownerGizmo,
    GizmoHandleHoverData handleHoverData,
    YesNoAnswer answer);
}
