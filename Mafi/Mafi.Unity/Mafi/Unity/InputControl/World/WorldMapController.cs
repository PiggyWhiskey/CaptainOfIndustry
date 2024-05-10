// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.WorldMapController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Fleet;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Core.Utils;
using Mafi.Core.World;
using Mafi.Unity.InputControl.Fleet.Battle;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class WorldMapController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar,
    IUnityUi
  {
    private readonly WorldMapView m_worldMapView;
    private readonly UiBuilder m_builder;
    private readonly IUnityInputMgr m_inputManager;
    private readonly IGameLoopEvents m_gameLoop;
    private bool m_registeredToUpdates;
    private DelayedEvent<IBattleState, bool> m_onBattleStartedEvent;

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public ControllerConfig Config => ControllerConfig.WindowFullscreen;

    public bool IsActive { get; private set; }

    public WorldMapController(
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoop,
      TravelingFleetManager fleetManager,
      WorldMapView worldMapView,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      WorldMapController controller = this;
      this.m_inputManager = inputManager;
      this.m_gameLoop = gameLoop;
      this.m_worldMapView = worldMapView;
      this.m_builder = builder;
      this.m_worldMapView.SetOnCloseButtonClickAction((Action) (() => inputManager.DeactivateController((IUnityInputController) controller)));
      if (!fleetManager.HasFleet)
        fleetManager.OnFleetCreated += new Action<TravelingFleet>(this.setupEvents);
      else
        this.setupEvents(fleetManager.TravelingFleet);
    }

    private void setupEvents(TravelingFleet fleet)
    {
      this.m_onBattleStartedEvent = new DelayedEvent<IBattleState, bool>(this.m_gameLoop, (Action<Action<IBattleState, bool>>) (action => fleet.OnBattleStarted += action));
      this.m_onBattleStartedEvent.OnSync += new Action<IBattleState, bool>(this.onBattleStarted);
    }

    public void OnLocationExplored(LocationExploredMessage exploredMessage)
    {
      this.m_worldMapView.ExplorationView.OnLocationExplored(exploredMessage);
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public void OpenBattle()
    {
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    private void onBattleStarted(IBattleState battleState, bool forceUiOpen)
    {
      this.m_worldMapView.BattleProgressView.SetBattle(battleState);
      this.m_worldMapView.BattleProgressView.Show<FleetBattleProgressView>();
      if (forceUiOpen)
        this.m_inputManager.ActivateNewController((IUnityInputController) this);
      else
        this.tryRegisterToUpdates();
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      controller.AddMainMenuButton(TrCore.WorldMap.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/WorldMap.svg", 520f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleMap));
    }

    public void Activate()
    {
      this.IsActive = true;
      this.tryRegisterToUpdates();
      this.m_worldMapView.BuildAndShow(this.m_builder);
    }

    public void Deactivate()
    {
      this.IsActive = false;
      this.m_worldMapView.Hide();
      if (this.m_worldMapView.BattleProgressView.IsVisible())
        return;
      this.tryUnregisterFromUpdates();
    }

    private void tryRegisterToUpdates()
    {
      if (this.m_registeredToUpdates)
        return;
      this.m_registeredToUpdates = true;
      this.m_gameLoop.SyncUpdate.AddNonSaveable<WorldMapController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<WorldMapController>(this, new Action<GameTime>(this.renderUpdate));
    }

    private void tryUnregisterFromUpdates()
    {
      if (!this.m_registeredToUpdates)
        return;
      this.m_registeredToUpdates = false;
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<WorldMapController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<WorldMapController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public bool InputUpdate(IInputScheduler inputScheduler) => this.m_worldMapView.InputUpdate();

    private void renderUpdate(GameTime gameTime) => this.m_worldMapView.RenderUpdate(gameTime);

    private void syncUpdate(GameTime gameTime) => this.m_worldMapView.SyncUpdate(gameTime);

    public void RegisterUi(UiBuilder builder) => this.m_worldMapView.BuildIfNeeded(builder);
  }
}
