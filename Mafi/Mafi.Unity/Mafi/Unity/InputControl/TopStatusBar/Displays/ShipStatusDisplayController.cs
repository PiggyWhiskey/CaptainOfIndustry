// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.ShipStatusDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
using Mafi.Core.World;
using Mafi.Localization;
using Mafi.Unity.InputControl.World;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar.Displays
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ShipStatusDisplayController : IStatusBarItem
  {
    private readonly IUnityInputMgr m_inputMgr;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly TravelingFleetManager m_fleetManager;
    private readonly WorldMapController m_worldMapController;
    private readonly UiBuilder m_builder;
    private StatusBar m_statusBar;
    private readonly ShipStatusDisplayController.ShipStatusView m_shipStatusView;

    public ShipStatusDisplayController(
      IUnityInputMgr inputMgr,
      IGameLoopEvents gameLoop,
      TravelingFleetManager fleetManager,
      WorldMapController worldMapController,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputMgr = inputMgr;
      this.m_gameLoop = gameLoop;
      this.m_fleetManager = fleetManager;
      this.m_worldMapController = worldMapController;
      this.m_builder = builder;
      this.m_shipStatusView = new ShipStatusDisplayController.ShipStatusView(this);
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_statusBar = statusBar;
      this.m_shipStatusView.BuildUi(this.m_builder);
      this.m_shipStatusView.SetWidth<ShipStatusDisplayController.ShipStatusView>(180f);
      if (this.m_fleetManager.HasFleet)
      {
        this.m_shipStatusView.Fleet = this.m_fleetManager.TravelingFleet;
        this.initStatusView();
      }
      else
        this.m_gameLoop.SyncUpdate.AddNonSaveable<ShipStatusDisplayController>(this, new Action<GameTime>(this.waitForFleet));
    }

    private void initStatusView()
    {
      this.m_shipStatusView.Fleet = this.m_fleetManager.TravelingFleet;
      this.m_shipStatusView.Show();
      this.m_gameLoop.SyncUpdate.AddNonSaveable<ShipStatusDisplayController.ShipStatusView>(this.m_shipStatusView, new Action<GameTime>(((View) this.m_shipStatusView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<ShipStatusDisplayController.ShipStatusView>(this.m_shipStatusView, (Action<GameTime>) (x => this.m_shipStatusView.RenderUpdate(x)));
      this.m_statusBar.AddElementToMiddle((IUiElement) this.m_shipStatusView, 250f, false);
    }

    private void waitForFleet(GameTime time)
    {
      if (!this.m_fleetManager.HasFleet)
        return;
      this.initStatusView();
    }

    public void OpenWorldMap()
    {
      this.m_inputMgr.ActivateNewController((IUnityInputController) this.m_worldMapController);
    }

    private class ShipStatusView : View
    {
      private readonly ShipStatusDisplayController m_controller;

      public TravelingFleet Fleet { get; set; }

      public ShipStatusView(ShipStatusDisplayController controller)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (ShipStatusView), SyncFrequency.OncePerSec);
        this.m_controller = controller;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent = this.Builder.NewBtn("Container component").OnClick(new Action(this.m_controller.OpenWorldMap)).PutTo<Btn>((IUiElement) this);
        int size = 26;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/WorldMap/Battleship-256.png", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent, (float) size);
        Panel textContainer = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent, new Offset(0.0f, 2f, (float) (size - 4 + 4), 2f));
        Panel progressBg = this.Builder.NewPanel("ProgressBg").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?((ColorRgba) 3173540)).PutToLeftOf<Panel>((IUiElement) textContainer, 0.0f);
        Txt status = this.Builder.NewTxt("Status").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.Global.Title.Extend(new ColorRgba?((ColorRgba) 16777215))).BestFitEnabled(12).PutTo<Txt>((IUiElement) textContainer, Offset.LeftRight(10f));
        updaterBuilder.Observe<FleetLocationState>((Func<FleetLocationState>) (() => this.Fleet.LocationState)).Observe<bool>((Func<bool>) (() => this.Fleet.IsIdle)).Do((Action<FleetLocationState, bool>) ((state, isIdle) =>
        {
          if (isIdle)
          {
            status.SetText((LocStrFormatted) Tr.EntityStatus__Ship_NoOrders);
            status.SetColor(this.Builder.Style.Global.OrangeText);
          }
          else
          {
            status.SetColor((ColorRgba) 16777215);
            switch (state)
            {
              case FleetLocationState.AtWorld:
              case FleetLocationState.ArrivingFromWorld:
              case FleetLocationState.DepartingToWorld:
                status.SetText((LocStrFormatted) Tr.EntityStatus__Ship_Moving);
                break;
              case FleetLocationState.Docked:
                status.SetText((LocStrFormatted) Tr.EntityStatus__Ship_Docked);
                break;
              case FleetLocationState.ExploreInProgress:
                status.SetText((LocStrFormatted) Tr.EntityStatus__Ship_Exploring);
                break;
              case FleetLocationState.BattleInProgress:
                status.SetText((LocStrFormatted) Tr.EntityStatus__Ship_InBattle);
                status.SetColor(this.Builder.Style.StatusBar.QuantityNegativeColor);
                break;
              default:
                status.SetText("");
                break;
            }
          }
        }));
        updaterBuilder.Observe<FleetLocationState>((Func<FleetLocationState>) (() => this.Fleet.LocationState)).Observe<Percent>((Func<Percent>) (() => this.Fleet.ExplorationProgress)).Observe<Percent>((Func<Percent>) (() => this.Fleet.TravelProgress)).Do((Action<FleetLocationState, Percent, Percent>) ((state, explorationProgress, travelProgress) =>
        {
          float width = textContainer.GetWidth();
          if (state == FleetLocationState.ExploreInProgress)
          {
            progressBg.SetWidth<Panel>(explorationProgress.Apply(width).Min(width));
            progressBg.Show<Panel>();
          }
          else if (state == FleetLocationState.AtWorld)
          {
            progressBg.SetWidth<Panel>(travelProgress.Apply(width).Min(width));
            progressBg.Show<Panel>();
          }
          else
            progressBg.Hide<Panel>();
        }));
        this.AddUpdater(updaterBuilder.Build());
      }
    }
  }
}
