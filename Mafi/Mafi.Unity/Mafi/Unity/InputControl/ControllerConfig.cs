// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.ControllerConfig
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  public struct ControllerConfig
  {
    public static readonly ControllerConfig Window;
    public static readonly ControllerConfig WindowFullscreen;
    public static readonly ControllerConfig InspectorWindow;
    public static readonly ControllerConfig Menu;
    public static readonly ControllerConfig MenuActive;
    public static readonly ControllerConfig GameMenu;
    public static readonly ControllerConfig MessageCenter;
    public static readonly ControllerConfig Tool;
    public static readonly ControllerConfig ToolBlockingCamera;
    public static readonly ControllerConfig ForestryTool;
    public static readonly ControllerConfig Mode;
    public static readonly ControllerConfig LayersPanel;
    public static readonly ControllerConfig PhotoMode;
    /// <summary>
    /// Returns whether the controller should stay open even when player presses the escape key.
    /// </summary>
    public bool IgnoreEscapeKey;
    /// <summary>
    /// Typically used for tools. We don't want to show a cursor with bulldozer while presenting research window.
    /// Note: Overlay group will not affect this
    /// </summary>
    public bool DeactivateOnOtherControllerActive;
    /// <summary>
    /// Whether the controller should be deactivated when the player clicks out of the UI. This is useful for pop-ups
    /// that can be closed very easily.
    /// </summary>
    public bool DeactivateOnNonUiClick;
    /// <summary>
    /// Whether the inspector cursor (and functionality) can be used while this controller is active. This does not
    /// mean that the inspector window will be present together with this controller.
    /// </summary>
    public bool AllowInspectorCursor;
    public bool BlockShortcuts;
    /// <summary>
    /// Block camera control e.g. movements keys when this controller is active.
    /// </summary>
    public bool DisableCameraControl;
    public bool BlockCameraControlIfInputWasProcessed;
    /// <summary>
    /// Whether game speed control should be disabled while this controller is active.
    /// </summary>
    public bool PreventSpeedControl;
    public ControllerGroup Group;
    public ControllerGroup? GroupToCloseOnActivation;

    static ControllerConfig()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ControllerConfig.Window = new ControllerConfig()
      {
        DeactivateOnNonUiClick = true,
        AllowInspectorCursor = false,
        Group = ControllerGroup.Window
      };
      ControllerConfig.WindowFullscreen = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = false,
        BlockShortcuts = true,
        DisableCameraControl = true,
        Group = ControllerGroup.Window
      };
      ControllerConfig.InspectorWindow = new ControllerConfig()
      {
        DeactivateOnNonUiClick = true,
        AllowInspectorCursor = true,
        Group = ControllerGroup.Window
      };
      ControllerConfig.Menu = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = true,
        Group = ControllerGroup.BottomMenu
      };
      ControllerConfig.MenuActive = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = false,
        Group = ControllerGroup.BottomMenu
      };
      ControllerConfig.GameMenu = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = false,
        PreventSpeedControl = true,
        Group = ControllerGroup.Window
      };
      ControllerConfig.MessageCenter = new ControllerConfig()
      {
        DeactivateOnNonUiClick = true,
        AllowInspectorCursor = false,
        PreventSpeedControl = true,
        Group = ControllerGroup.Window
      };
      ControllerConfig.Tool = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = false,
        DeactivateOnOtherControllerActive = true,
        Group = ControllerGroup.Tool,
        GroupToCloseOnActivation = new ControllerGroup?(ControllerGroup.Window)
      };
      ControllerConfig.ToolBlockingCamera = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = false,
        DeactivateOnOtherControllerActive = true,
        BlockCameraControlIfInputWasProcessed = true,
        Group = ControllerGroup.Tool,
        GroupToCloseOnActivation = new ControllerGroup?(ControllerGroup.Window)
      };
      ControllerConfig.ForestryTool = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = false,
        DeactivateOnOtherControllerActive = true,
        Group = ControllerGroup.Tool
      };
      ControllerConfig.Mode = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = true,
        DeactivateOnOtherControllerActive = false,
        IgnoreEscapeKey = true,
        Group = ControllerGroup.AlwaysActive
      };
      ControllerConfig.LayersPanel = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = true,
        Group = ControllerGroup.AlwaysActive
      };
      ControllerConfig.PhotoMode = new ControllerConfig()
      {
        DeactivateOnNonUiClick = false,
        AllowInspectorCursor = false,
        BlockCameraControlIfInputWasProcessed = true,
        Group = ControllerGroup.Window
      };
    }
  }
}
