// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.IUnityInputMgr
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ReadonlyCollections;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.GameMenu;
using Mafi.Unity.InputControl.TopStatusBar;
using System;

#nullable disable
namespace Mafi.Unity
{
  public interface IUnityInputMgr : IRootEscapeManager
  {
    IIndexable<IUnityInputController> ActiveControllers { get; }

    /// <summary>Called when a controller is activated or deactivated.</summary>
    event Action<IUnityInputController> ControllerActivated;

    event Action<IUnityInputController> ControllerDeactivated;

    void ActivateNewController(IUnityInputController controller);

    void DeactivateController(IUnityInputController controller);

    void ToggleController(IUnityInputController controller);

    void DeactivateAllControllers();

    void RegisterGlobalShortcut(
      Func<ShortcutsManager, KeyBindings> shortcut,
      IUnityInputController controller);

    void RegisterGlobalShortcut(Func<ShortcutsManager, KeyBindings> shortcut, Action callback);

    void RemoveGlobalShortcut(IUnityInputController controller);

    void RegisterGameMenuController(IGameMenuController controller);

    void RegisterGameSpeedController(GameSpeedController controller);

    void OnBuildModeActivated(IUnityInputController controllerWithMode);

    bool IsWindowControllerOpen();
  }
}
