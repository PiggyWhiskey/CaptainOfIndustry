// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.UnityInputManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl.GameMenu;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.InputControl.Tools;
using Mafi.Unity.InputControl.TopStatusBar;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class UnityInputManager : IUnityInputMgr, IRootEscapeManager
  {
    private readonly IInputScheduler m_inputScheduler;
    private readonly InspectorController m_inspector;
    private readonly CameraController m_cameraController;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly QuickEntityTransformInputController m_quickTransformController;
    /// <summary>Currently active controllers</summary>
    private readonly Lyst<IUnityInputController> m_controllers;
    /// <summary>Helper array to collect deactivated controllers.</summary>
    private LystStruct<IUnityInputController> m_controllersPendingActivation;
    private LystStruct<IUnityInputController> m_controllersToDeactivate;
    private LystStruct<IUnityInputController> m_iterationHelper;
    /// <summary>All registered global shortcuts.</summary>
    private LystStruct<KeyValuePair<IUnityInputController, Func<ShortcutsManager, KeyBindings>>> m_globalShortcuts;
    private LystStruct<KeyValuePair<IToolbarItemController, Func<ShortcutsManager, KeyBindings>>> m_toolbarShortcuts;
    private LystStruct<KeyValuePair<Action, Func<ShortcutsManager, KeyBindings>>> m_globalActions;
    private Option<IRootEscapeHandler> m_escapeHandler;
    private Option<IGameMenuController> m_gameMenuController;
    private Option<GameSpeedController> m_gameSpeedController;

    public event Action<IUnityInputController> ControllerActivated;

    public event Action<IUnityInputController> ControllerDeactivated;

    public IIndexable<IUnityInputController> ActiveControllers
    {
      get => (IIndexable<IUnityInputController>) this.m_controllers;
    }

    public UnityInputManager(
      IGameLoopEvents gameLoopEvents,
      IInputScheduler inputScheduler,
      ShortcutsManager shortcutsManager,
      InspectorController inspector,
      CameraController cameraController,
      QuickEntityTransformInputController quickTransformController,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_controllers = new Lyst<IUnityInputController>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
      this.m_quickTransformController = quickTransformController;
      this.m_inputScheduler = inputScheduler.CheckNotNull<IInputScheduler>();
      this.m_inspector = inspector.CheckNotNull<InspectorController>();
      this.m_cameraController = cameraController.CheckNotNull<CameraController>();
      builder.SetRootEscManager((IRootEscapeManager) this);
      gameLoopEvents.InputUpdate.AddNonSaveable<UnityInputManager>(this, new Action<GameTime>(this.inputUpdate));
    }

    public void ActivateNewController(IUnityInputController newController)
    {
      if (this.m_controllers.Contains(newController) || this.m_controllersPendingActivation.Contains(newController))
        return;
      this.m_controllersPendingActivation.Add(newController);
      foreach (IUnityInputController controller in this.m_controllers)
      {
        if (controller != newController)
        {
          ControllerConfig config1 = controller.Config;
          ControllerConfig config2 = newController.Config;
          if (config2.Group != ControllerGroup.AlwaysActive && config1.DeactivateOnOtherControllerActive)
            this.m_controllersToDeactivate.Add(controller);
          else if (config2.Group != ControllerGroup.AlwaysActive && config2.Group == config1.Group)
          {
            this.m_controllersToDeactivate.Add(controller);
          }
          else
          {
            ControllerGroup? closeOnActivation = config2.GroupToCloseOnActivation;
            ControllerGroup group = config1.Group;
            if (closeOnActivation.GetValueOrDefault() == group & closeOnActivation.HasValue)
              this.m_controllersToDeactivate.Add(controller);
          }
        }
      }
      foreach (IUnityInputController controller in this.m_controllersToDeactivate)
      {
        try
        {
          this.DeactivateController(controller);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, string.Format("Failed to deactivate controller `{0}`.", (object) controller.GetType()));
        }
      }
      this.m_controllersToDeactivate.ClearSkipZeroingMemory();
      this.activateController(newController);
    }

    /// <summary>
    /// Used by popups to close on escape before anything else gets the key.
    /// </summary>
    public void SetRootEscapeHandler(IRootEscapeHandler handler)
    {
      if (this.m_escapeHandler == handler)
        return;
      if (this.m_escapeHandler.HasValue)
        Log.Error("Escape callback already registered for someone else!");
      else
        this.m_escapeHandler = handler.CreateOption<IRootEscapeHandler>();
    }

    public void ClearRootEscapeHandler(IRootEscapeHandler handler)
    {
      if (this.m_escapeHandler.HasValue && this.m_escapeHandler != handler)
        Log.Error("Escape callback registered for someone else!");
      else
        this.m_escapeHandler = (Option<IRootEscapeHandler>) Option.None;
    }

    public bool IsWindowControllerOpen()
    {
      foreach (IUnityInputController controller in this.m_controllers)
      {
        if (controller.Config.Group == ControllerGroup.Window)
          return true;
      }
      return false;
    }

    public void DeactivateController(IUnityInputController controller)
    {
      if (!this.m_controllers.Remove(controller))
        return;
      controller.Deactivate();
      Action<IUnityInputController> controllerDeactivated = this.ControllerDeactivated;
      if (controllerDeactivated == null)
        return;
      controllerDeactivated(controller);
    }

    public void OnBuildModeActivated(IUnityInputController controllerWithMode)
    {
      foreach (IUnityInputController controller in this.m_controllers)
      {
        if (controller != controllerWithMode)
        {
          if (controller.Config.DeactivateOnOtherControllerActive)
            this.m_controllersToDeactivate.Add(controller);
          else if (controller.Config.Group == ControllerGroup.Window)
            this.m_controllersToDeactivate.Add(controller);
        }
      }
      foreach (IUnityInputController controller in this.m_controllersToDeactivate)
      {
        try
        {
          this.DeactivateController(controller);
        }
        catch (Exception ex)
        {
          Log.Exception(ex, string.Format("Failed to deactivate controller `{0}`.", (object) controller.GetType()));
        }
      }
      this.m_controllersToDeactivate.ClearSkipZeroingMemory();
    }

    public void ToggleController(IUnityInputController controller)
    {
      if (this.m_controllers.Contains(controller))
        this.DeactivateController(controller);
      else
        this.ActivateNewController(controller);
    }

    public void DeactivateAllControllers()
    {
      foreach (IUnityInputController controller in this.m_controllers.ToArray())
        this.DeactivateController(controller);
    }

    public void RegisterGlobalShortcut(
      Func<ShortcutsManager, KeyBindings> shortcut,
      IUnityInputController controller)
    {
      if (controller is IToolbarItemController key)
        this.m_toolbarShortcuts.Add<IToolbarItemController, Func<ShortcutsManager, KeyBindings>>(key, shortcut);
      else
        this.m_globalShortcuts.Add<IUnityInputController, Func<ShortcutsManager, KeyBindings>>(controller, shortcut);
    }

    public void RegisterGlobalShortcut(
      Func<ShortcutsManager, KeyBindings> shortcut,
      Action callback)
    {
      this.m_globalActions.Add<Action, Func<ShortcutsManager, KeyBindings>>(callback, shortcut);
    }

    public void RemoveGlobalShortcut(IUnityInputController controller)
    {
      if (controller is IToolbarItemController key)
        this.m_toolbarShortcuts.Remove<IToolbarItemController, Func<ShortcutsManager, KeyBindings>>(key);
      else
        this.m_globalShortcuts.Remove<IUnityInputController, Func<ShortcutsManager, KeyBindings>>(controller);
    }

    public void RegisterGameMenuController(IGameMenuController controller)
    {
      this.m_gameMenuController = Option.Some<IGameMenuController>(controller);
    }

    public void RegisterGameSpeedController(GameSpeedController controller)
    {
      this.m_gameSpeedController = Option.Some<GameSpeedController>(controller);
    }

    private void activateController(IUnityInputController controller)
    {
      Action<IUnityInputController> controllerActivated = this.ControllerActivated;
      if (controllerActivated != null)
        controllerActivated(controller);
      this.m_controllersPendingActivation.Remove(controller);
      controller.Activate();
      this.m_controllers.Add(controller);
    }

    private void inputUpdate(GameTime gameTime)
    {
      bool flag1 = UnityInputManager.IsInputFieldFocused();
      if (this.m_cameraController.InputUpdateEarly())
        return;
      if (this.m_cameraController.IsInFreeLookMode)
      {
        this.m_cameraController.InputUpdate();
      }
      else
      {
        if (this.m_escapeHandler.HasValue && UnityEngine.Input.GetKey(KeyCode.Escape))
        {
          bool flag2 = false;
          try
          {
            flag2 = this.m_escapeHandler.Value.OnEscape();
          }
          catch (Exception ex)
          {
            Log.Exception(ex);
          }
          this.m_escapeHandler = (Option<IRootEscapeHandler>) Option.None;
          if (flag2)
            return;
        }
        if (flag1)
          return;
        bool updateCamera;
        this.inputUpdate(out updateCamera);
        if (!updateCamera)
          return;
        this.m_cameraController.InputUpdate();
      }
    }

    private void inputUpdate(out bool updateCamera)
    {
      updateCamera = true;
      bool flag1 = true;
      foreach (IUnityInputController controller in this.m_controllers)
        flag1 &= !controller.Config.PreventSpeedControl;
      if (flag1 && this.m_gameSpeedController.HasValue && this.m_gameSpeedController.Value.InputUpdate())
        return;
      bool flag2 = true;
      bool flag3 = true;
      if (this.m_controllers.IsNotEmpty)
      {
        this.m_iterationHelper.ClearSkipZeroingMemory();
        this.m_iterationHelper.AddRange(this.m_controllers);
        foreach (IUnityInputController unityInputController in this.m_iterationHelper)
        {
          ControllerConfig config = unityInputController.Config;
          flag3 &= config.AllowInspectorCursor;
          flag2 &= !config.BlockShortcuts;
          updateCamera &= !config.DisableCameraControl;
          try
          {
            if (unityInputController.InputUpdate(this.m_inputScheduler))
            {
              updateCamera &= !config.BlockCameraControlIfInputWasProcessed;
              return;
            }
          }
          catch (Exception ex)
          {
            Log.Exception(ex, "Input update exception for " + unityInputController.GetType().Name);
          }
        }
      }
      bool flag4 = EventSystem.current.IsPointerOverGameObject();
      if (UnityEngine.Input.anyKeyDown)
      {
        bool flag5 = !flag4 && (this.m_shortcutsManager.IsPrimaryActionDown || this.m_shortcutsManager.IsSecondaryActionUp);
        bool keyDown = UnityEngine.Input.GetKeyDown(KeyCode.Escape);
        if (keyDown | flag5)
        {
          bool flag6 = false;
          for (int index = this.m_controllers.Count - 1; index >= 0; --index)
          {
            IUnityInputController controller = this.m_controllers[index];
            ControllerConfig config = controller.Config;
            if (keyDown && !config.IgnoreEscapeKey || flag5 && config.DeactivateOnNonUiClick)
            {
              this.DeactivateController(controller);
              flag6 = true;
              break;
            }
          }
          if (!flag6 & keyDown && this.m_gameMenuController.HasValue)
            this.ActivateNewController((IUnityInputController) this.m_gameMenuController.Value);
        }
        else
        {
          IUnityInputController unityInputController = (IUnityInputController) null;
          foreach (KeyValuePair<IToolbarItemController, Func<ShortcutsManager, KeyBindings>> toolbarShortcut in this.m_toolbarShortcuts)
          {
            if (toolbarShortcut.Key.IsVisible || !toolbarShortcut.Key.DeactivateShortcutsIfNotVisible || this.m_controllers.Contains((IUnityInputController) toolbarShortcut.Key))
            {
              try
              {
                if (this.m_shortcutsManager.IsDown(toolbarShortcut.Value(this.m_shortcutsManager)))
                {
                  unityInputController = (IUnityInputController) toolbarShortcut.Key;
                  break;
                }
              }
              catch (Exception ex)
              {
                Log.Exception(ex, "Exception in key-binding processing for " + string.Format("{0}, removing the shortcut registration", (object) toolbarShortcut.Key.GetType()));
                this.m_toolbarShortcuts.Remove<IToolbarItemController, Func<ShortcutsManager, KeyBindings>>(toolbarShortcut.Key);
                return;
              }
            }
          }
          if (unityInputController == null)
          {
            foreach (KeyValuePair<IUnityInputController, Func<ShortcutsManager, KeyBindings>> globalShortcut in this.m_globalShortcuts)
            {
              try
              {
                if (this.m_shortcutsManager.IsDown(globalShortcut.Value(this.m_shortcutsManager)))
                {
                  unityInputController = globalShortcut.Key;
                  break;
                }
              }
              catch (Exception ex)
              {
                Log.Exception(ex, "Exception in key-binding processing for " + string.Format("{0}, removing the shortcut registration", (object) globalShortcut.Key.GetType()));
                this.m_globalShortcuts.Remove<IUnityInputController, Func<ShortcutsManager, KeyBindings>>(globalShortcut.Key);
                return;
              }
            }
            foreach (KeyValuePair<Action, Func<ShortcutsManager, KeyBindings>> globalAction in this.m_globalActions)
            {
              try
              {
                if (this.m_shortcutsManager.IsDown(globalAction.Value(this.m_shortcutsManager)))
                {
                  globalAction.Key();
                  return;
                }
              }
              catch (Exception ex)
              {
                Log.Exception(ex, "Exception in key-binding processing for action, removing the shortcut registration");
                this.m_globalActions.Remove<Action, Func<ShortcutsManager, KeyBindings>>(globalAction.Key);
                return;
              }
            }
          }
          if (unityInputController != null)
          {
            if (this.m_controllers.Contains(unityInputController))
            {
              this.DeactivateController(unityInputController);
              return;
            }
            if (flag2)
            {
              this.ActivateNewController(unityInputController);
              return;
            }
          }
        }
      }
      if (!flag3)
        return;
      this.m_inspector.UpdateCursor();
      if (this.m_quickTransformController.InputUpdate() || !this.m_shortcutsManager.IsPrimaryActionDown || flag4 || !this.m_inspector.TryActivate((IUnityInputMgr) this))
        return;
      this.ActivateNewController((IUnityInputController) this.m_inspector);
    }

    public static bool IsInputFieldFocused()
    {
      GameObject selectedGameObject = EventSystem.current.currentSelectedGameObject;
      if ((UnityEngine.Object) selectedGameObject == (UnityEngine.Object) null)
        return false;
      TMP_InputField component1 = selectedGameObject.GetComponent<TMP_InputField>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        return component1.isFocused;
      PanelEventHandler component2 = selectedGameObject.GetComponent<PanelEventHandler>();
      return (UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.panel.focusController.focusedElement is TextField;
    }
  }
}
