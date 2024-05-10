// Decompiled with JetBrains decompiler
// Type: RTG.IGizmoSlider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;

#nullable disable
namespace RTG
{
  public interface IGizmoSlider
  {
    Gizmo Gizmo { get; }

    int HandleId { get; }

    Priority HoverPriority3D { get; }

    Priority HoverPriority2D { get; }

    Priority GenericHoverPriority { get; }

    void SetHoverable(bool isHoverable);

    void SetVisible(bool isVisible);

    void SetSnapEnabled(bool isEnabled);

    void Render(Camera camera);
  }
}
