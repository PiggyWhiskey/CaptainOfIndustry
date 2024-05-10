// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.CargoShipLimitDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Cargo.Modules;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
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
  internal class CargoShipLimitDisplayController : IStatusBarItem
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly CargoDepotManager m_cargoDepotManager;
    private readonly UiBuilder m_builder;
    private CargoShipLimitDisplayController.CargoShipLimitView m_cargoShipsLimitView;

    public CargoShipLimitDisplayController(
      IGameLoopEvents gameLoop,
      CargoDepotManager cargoDepotManager,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_cargoDepotManager = cargoDepotManager;
      this.m_builder = builder;
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_cargoShipsLimitView = new CargoShipLimitDisplayController.CargoShipLimitView(this, statusBar);
      this.m_cargoShipsLimitView.BuildUi(this.m_builder);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<CargoShipLimitDisplayController>(this, new Action<GameTime>(((View) this.m_cargoShipsLimitView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<CargoShipLimitDisplayController>(this, (Action<GameTime>) (x => this.m_cargoShipsLimitView.RenderUpdate(x)));
    }

    private class CargoShipLimitView : View
    {
      private readonly CargoShipLimitDisplayController m_controller;
      private readonly StatusBar m_statusBar;

      public CargoShipLimitView(CargoShipLimitDisplayController controller, StatusBar statusBar)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (CargoShipLimitView), SyncFrequency.OncePerSec);
        this.m_controller = controller;
        this.m_statusBar = statusBar;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent1 = this.Builder.NewBtn("Container component").AddToolTip(Tr.CargoShipsLimitTooltip).PutTo<Btn>((IUiElement) this);
        int iconWidth = style.StatusBar.IconWidth;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/Toolbar/CargoShip.svg", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent1, (float) iconWidth);
        Panel parent2 = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent1, new Offset(0.0f, 2f, (float) (iconWidth + 4), 2f));
        Txt demandSupply = this.Builder.NewTxt("Limit").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.StatusBar.QuantityStateText).PutTo<Txt>((IUiElement) parent2, Offset.Left(5f));
        CargoDepotManager cargoDepotManager = this.m_controller.m_cargoDepotManager;
        updaterBuilder.Observe<int>((Func<int>) (() => cargoDepotManager.RepairedUnusedShips.Count)).Observe<int>((Func<int>) (() => cargoDepotManager.AmountOfShipsInUse)).Observe<int>((Func<int>) (() => cargoDepotManager.AmountOfShipsDiscovered)).Observe<bool>((Func<bool>) (() => cargoDepotManager.HasShipOrDepot)).Do((Action<int, int, int, bool>) ((repairedCount, shipsInUse, foundTotal, hasShipOrDepot) =>
        {
          if (!this.IsVisible & hasShipOrDepot)
          {
            this.Show();
            this.m_statusBar.OnItemChanged();
          }
          demandSupply.SetText(string.Format("{0} / {1}", (object) (repairedCount + shipsInUse), (object) foundTotal));
          demandSupply.SetColor(style.StatusBar.QuantityStatePositiveColor);
        }));
        this.AddUpdater(updaterBuilder.Build());
        this.Hide();
        this.SetWidth<CargoShipLimitDisplayController.CargoShipLimitView>(100f);
        this.m_statusBar.AddElementToMiddle((IUiElement) this, 350f, false);
      }
    }
  }
}
