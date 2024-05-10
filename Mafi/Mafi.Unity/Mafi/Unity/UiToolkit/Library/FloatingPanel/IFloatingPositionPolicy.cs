// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiToolkit.Library.FloatingPanel.IFloatingPositionPolicy
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiToolkit.Component;
using UnityEngine;

#nullable disable
namespace Mafi.Unity.UiToolkit.Library.FloatingPanel
{
  /// <summary>
  /// Policy that helps to customize how to place floating windows.
  /// </summary>
  public interface IFloatingPositionPolicy
  {
    /// <summary>
    /// Applies any constraints or configuration on the floating panel before it
    /// gets its first layout to determine its position.
    /// </summary>
    void InitConstraints(UiComponent floatingPanel, UiComponent target);

    /// <summary>
    /// Computes placement of the floating panel based on the provided information.
    /// </summary>
    /// <returns>Left, top absolute position.</returns>
    Vector2 GetPosition(Rect boundsOfTarget, Vector2 floatingPanelSize, Vector2 screenSize);
  }
}
