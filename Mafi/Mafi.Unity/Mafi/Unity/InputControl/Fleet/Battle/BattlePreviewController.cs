// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Fleet.Battle.BattlePreviewController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Fleet;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.World;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UserInterface;
using System;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Fleet.Battle
{
  /// <summary>
  /// This controller just create a testing fleet and show a battle. Great of debugging or
  /// experimenting. To enable this uncomment the line below and it will get added to the toolbar.
  /// </summary>
  internal class BattlePreviewController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private BattlePreviewWindow m_battleView;
    private readonly IUnityInputMgr m_inputManager;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IBattleSimulator m_battleSimulator;
    private readonly ProtosDb m_protosDb;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly UiBuilder m_builder;
    private TravelingFleet m_fleet;
    private Option<IBattleState> m_currentBattleState;

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public ControllerConfig Config => ControllerConfig.Window;

    public BattlePreviewController(
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoop,
      IBattleSimulator battleSimulator,
      ProtosDb protosDb,
      TravelingFleetManager fleetManager,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputManager = inputManager;
      this.m_gameLoop = gameLoop;
      this.m_battleSimulator = battleSimulator;
      this.m_protosDb = protosDb;
      this.m_fleetManager = fleetManager;
      this.m_builder = builder;
      fleetManager.OnFleetCreated += (Action<TravelingFleet>) (fleet => this.m_fleet = fleet);
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      controller.AddMainMenuButton("Battle Preview", (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Battleship128.png", 521f);
      this.m_battleView = new BattlePreviewWindow(new Action(this.resetBattleClick), this.m_fleetManager);
      this.m_battleView.SetOnCloseButtonClickAction((Action) (() => this.m_inputManager.DeactivateController((IUnityInputController) this)));
      this.m_battleView.BuildUi(this.m_builder);
    }

    private void resetBattleClick()
    {
      if (!this.m_currentBattleState.HasValue || !this.m_currentBattleState.Value.Result.HasValue)
        return;
      this.resetBattle();
    }

    public void Activate()
    {
      this.m_gameLoop.SyncUpdate.AddNonSaveable<BattlePreviewController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<BattlePreviewController>(this, new Action<GameTime>(this.renderUpdate));
      if (this.m_currentBattleState.IsNone)
        this.resetBattle();
      this.m_battleView.Show();
    }

    private void resetBattle()
    {
      Assert.That<bool>(this.m_currentBattleState.IsNone || this.m_currentBattleState.Value.Result.HasValue).IsTrue();
      BattleFleet fleet = this.createFleet(this.pirateCruiser(2), this.piratePatrol(2));
      this.m_fleet.FleetEntity.Repair();
      this.m_currentBattleState = this.m_battleSimulator.StartBattle(fleet, this.m_fleet.BattleFleet);
      Assert.That<Option<IBattleState>>(this.m_currentBattleState).HasValue<IBattleState>();
      this.m_battleView.SetBattle(this.m_currentBattleState.Value);
    }

    private Lyst<FleetEntity> pirateScout(int shipsCount, int guns = 1)
    {
      return this.fleetEntity(shipsCount, guns, new FleetEntityHullProto.ID("ScoutHull"), new FleetWeaponProto.ID("Gun80mm"));
    }

    private Lyst<FleetEntity> piratePatrol(int shipsCount, int guns = 2)
    {
      return this.fleetEntity(shipsCount, guns, new FleetEntityHullProto.ID("PatrolHull"), new FleetWeaponProto.ID("Gun100mm"));
    }

    private Lyst<FleetEntity> pirateCruiser(int shipsCount, int guns = 4, int armors = 2)
    {
      return this.fleetEntity(shipsCount, guns, new FleetEntityHullProto.ID("CruiserHull"), new FleetWeaponProto.ID("Gun100mm"), armors, new FleetWeaponProto.ID?(new FleetWeaponProto.ID("ArmorT1")));
    }

    private Lyst<FleetEntity> fleetEntity(
      int shipsCount,
      int guns,
      FleetEntityHullProto.ID hullId,
      FleetWeaponProto.ID gunId,
      int armors = 0,
      FleetWeaponProto.ID? armorId = null)
    {
      FleetEntityHullProto orThrow1 = this.m_protosDb.GetOrThrow<FleetEntityHullProto>((Proto.ID) hullId);
      FleetWeaponProto orThrow2 = this.m_protosDb.GetOrThrow<FleetWeaponProto>((Proto.ID) gunId);
      Lyst<FleetEntityPartProto> lyst1 = new Lyst<FleetEntityPartProto>();
      if (armorId.HasValue && armors > 0)
      {
        FleetEntityPartProto orThrow3 = this.m_protosDb.GetOrThrow<FleetEntityPartProto>((Proto.ID) armorId.Value);
        lyst1.AddRange(Enumerable.Repeat<FleetEntityPartProto>(orThrow3, armors));
      }
      Lyst<FleetEntity> lyst2 = new Lyst<FleetEntity>();
      for (int index = 0; index < shipsCount; ++index)
        lyst2.Add(new FleetEntity(orThrow1, Enumerable.Repeat<FleetWeaponProto>(orThrow2, guns).ToImmutableArray<FleetWeaponProto>(), lyst1.ToImmutableArray()));
      return lyst2;
    }

    private BattleFleet createFleet(params Lyst<FleetEntity>[] fleets)
    {
      BattleFleet fleet1 = new BattleFleet("Pirates", false);
      foreach (Lyst<FleetEntity> fleet2 in fleets)
      {
        foreach (FleetEntity entity in fleet2)
          fleet1.AddEntity(entity);
      }
      return fleet1;
    }

    public void Deactivate()
    {
      this.m_battleView.Hide();
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<BattlePreviewController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<BattlePreviewController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public bool InputUpdate(IInputScheduler inputScheduler) => this.m_battleView.InputUpdate();

    private void renderUpdate(GameTime gameTime) => this.m_battleView.RenderUpdate(gameTime);

    private void syncUpdate(GameTime gameTime) => this.m_battleView.SyncUpdate(gameTime);
  }
}
