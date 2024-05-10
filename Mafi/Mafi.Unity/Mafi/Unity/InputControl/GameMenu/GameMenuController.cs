// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.GameMenuController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.SaveGame;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.InputControl.TopStatusBar;
using Mafi.Unity.MapEditor;
using Mafi.Unity.UiToolkit.Component;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class GameMenuController : 
    IGameMenuController,
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private UiBuilder m_builder;
    private bool m_wasPaused;
    private Option<GameEscapeMenu> m_gameMenu;
    private Option<MapEditorEscapeMenu> m_mapEditorMenu;
    private readonly IMain m_main;
    private readonly IUnityInputMgr m_inputManager;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly GameSpeedController m_speedController;
    private readonly RandomProvider m_randomProvider;
    private readonly GameDifficultyApplier m_difficultyApplier;
    private readonly IInputScheduler m_inputScheduler;
    private readonly ISaveManager m_saveManager;
    private readonly IGameSaveInfoProvider m_gameSaveInfoProvider;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly ITutorialProgressCleaner m_progressCleaner;
    private readonly IResolver m_resolver;

    public event Action<IToolbarItemController> VisibilityChanged;

    public ControllerConfig Config => ControllerConfig.GameMenu;

    public bool IsActive { get; private set; }

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    private bool IsInMapEditor => this.m_main.CurrentScene.Value is MapEditorScene;

    public GameMenuController(
      IUnityInputMgr inputManager,
      IMain main,
      IGameLoopEvents gameLoopEvents,
      ISaveManager saveManager,
      GameSpeedController speedController,
      RandomProvider randomProvider,
      ITutorialProgressCleaner progressCleaner,
      GameDifficultyApplier difficultyApplier,
      IInputScheduler inputScheduler,
      UiBuilder builder,
      IGameSaveInfoProvider gameSaveInfoProvider,
      ShortcutsManager shortcutsManager,
      IResolver resolver)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_main = main;
      this.m_inputManager = inputManager;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_speedController = speedController;
      this.m_randomProvider = randomProvider;
      this.m_saveManager = saveManager;
      this.m_gameSaveInfoProvider = gameSaveInfoProvider;
      this.m_shortcutsManager = shortcutsManager;
      this.m_resolver = resolver;
      this.m_progressCleaner = progressCleaner;
      this.m_difficultyApplier = difficultyApplier;
      this.m_inputScheduler = inputScheduler;
      this.m_builder = builder;
      inputManager.RegisterGameMenuController((IGameMenuController) this);
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      controller.AddRightMenuButton("Game menu", (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Menu128.png", 0.0f);
    }

    public void Activate()
    {
      this.IsActive = true;
      this.m_wasPaused = this.m_speedController.IsPaused;
      this.m_speedController.RequestPause();
      if (this.IsInMapEditor)
      {
        if (this.m_mapEditorMenu.IsNone)
        {
          this.m_mapEditorMenu = (Option<MapEditorEscapeMenu>) new MapEditorEscapeMenu(this.m_main, this.m_progressCleaner, this.m_resolver, this.m_shortcutsManager);
          this.m_mapEditorMenu.Value.OnHide(new Action(this.handleDeactivate));
          this.m_builder.AddComponent((UiComponent) this.m_mapEditorMenu.Value);
        }
        this.m_mapEditorMenu.Value.Show<MapEditorEscapeMenu>();
      }
      else
      {
        if (this.m_gameMenu.IsNone)
        {
          this.m_gameMenu = (Option<GameEscapeMenu>) new GameEscapeMenu(this.m_main, this.m_saveManager, this.m_gameSaveInfoProvider, this.m_progressCleaner, this.m_difficultyApplier, this.m_inputScheduler, this.m_randomProvider, this.m_shortcutsManager);
          this.m_gameMenu.Value.OnHide(new Action(this.handleDeactivate));
          this.m_builder.AddComponent((UiComponent) this.m_gameMenu.Value);
        }
        this.m_gameMenu.Value.Show<GameEscapeMenu>();
      }
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<GameMenuController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<GameMenuController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void Deactivate()
    {
      MapEditorEscapeMenu valueOrNull1 = this.m_mapEditorMenu.ValueOrNull;
      if (valueOrNull1 != null)
        valueOrNull1.Hide<MapEditorEscapeMenu>();
      GameEscapeMenu valueOrNull2 = this.m_gameMenu.ValueOrNull;
      if (valueOrNull2 != null)
        valueOrNull2.Hide<GameEscapeMenu>();
      if (!this.m_wasPaused)
        this.m_speedController.RequestResume();
      this.IsActive = false;
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<GameMenuController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoopEvents.RenderUpdate.RemoveNonSaveable<GameMenuController>(this, new Action<GameTime>(this.renderUpdate));
    }

    private void syncUpdate(GameTime gameTime)
    {
      this.m_mapEditorMenu.ValueOrNull?.SyncUpdate(gameTime);
      this.m_gameMenu.ValueOrNull?.SyncUpdate(gameTime);
    }

    private void renderUpdate(GameTime gameTime)
    {
      this.m_mapEditorMenu.ValueOrNull?.RenderUpdate(gameTime);
      this.m_gameMenu.ValueOrNull?.RenderUpdate(gameTime);
    }

    private void handleDeactivate()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      MapEditorEscapeMenu valueOrNull1 = this.m_mapEditorMenu.ValueOrNull;
      if ((valueOrNull1 != null ? (valueOrNull1.InputUpdate() ? 1 : 0) : 0) != 0)
        return true;
      GameEscapeMenu valueOrNull2 = this.m_gameMenu.ValueOrNull;
      int num = valueOrNull2 != null ? (valueOrNull2.InputUpdate() ? 1 : 0) : 0;
      return true;
    }
  }
}
