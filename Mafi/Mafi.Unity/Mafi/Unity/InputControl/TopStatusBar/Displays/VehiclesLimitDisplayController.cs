// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.VehiclesLimitDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
using Mafi.Core.Vehicles;
using Mafi.Unity.InputControl.Logistics;
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
  internal class VehiclesLimitDisplayController : IStatusBarItem
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IVehiclesManager m_vehiclesManager;
    private readonly UiBuilder m_uiBuilder;
    private readonly VehiclesLimitDisplayController.VehiclesLimitView m_vehiclesLimitView;

    public VehiclesLimitDisplayController(
      IGameLoopEvents gameLoop,
      IUnityInputMgr inputMgr,
      IVehiclesManager vehiclesManager,
      LogisticsOverviewController logisticsOverview,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_vehiclesManager = vehiclesManager;
      this.m_uiBuilder = builder;
      this.m_vehiclesLimitView = new VehiclesLimitDisplayController.VehiclesLimitView(inputMgr, this, logisticsOverview);
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_vehiclesLimitView.BuildUi(this.m_uiBuilder);
      this.m_vehiclesLimitView.SetWidth<VehiclesLimitDisplayController.VehiclesLimitView>(120f);
      this.m_vehiclesLimitView.Show();
      statusBar.AddElementToMiddle((IUiElement) this.m_vehiclesLimitView, 300f, false);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<VehiclesLimitDisplayController>(this, new Action<GameTime>(((View) this.m_vehiclesLimitView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<VehiclesLimitDisplayController>(this, (Action<GameTime>) (x => this.m_vehiclesLimitView.RenderUpdate(x)));
    }

    private class VehiclesLimitView : View
    {
      private readonly IUnityInputMgr m_inputMgr;
      private readonly VehiclesLimitDisplayController m_controller;
      private readonly LogisticsOverviewController m_logisticsOverview;

      public VehiclesLimitView(
        IUnityInputMgr inputMgr,
        VehiclesLimitDisplayController controller,
        LogisticsOverviewController logisticsOverview)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (VehiclesLimitView), SyncFrequency.OncePerSec);
        this.m_inputMgr = inputMgr;
        this.m_controller = controller;
        this.m_logisticsOverview = logisticsOverview;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent1 = this.Builder.NewBtn("Container component").AddToolTip(Tr.VehiclesLimit__Tooltip).OnClick((Action) (() => this.m_inputMgr.ActivateNewController((IUnityInputController) this.m_logisticsOverview))).PutTo<Btn>((IUiElement) this);
        int size = 20;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/Toolbar/Vehicles.svg", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent1, (float) size);
        Panel parent2 = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent1, new Offset(0.0f, 2f, (float) (size + 4), 2f));
        Txt demandSupply = this.Builder.NewTxt("Limit").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.StatusBar.QuantityStateText).PutTo<Txt>((IUiElement) parent2, Offset.Left(5f));
        IVehiclesManager vehiclesManager = this.m_controller.m_vehiclesManager;
        updaterBuilder.Observe<int>((Func<int>) (() => vehiclesManager.MaxVehiclesLimit)).Observe<int>((Func<int>) (() => vehiclesManager.VehiclesLimitLeft)).Do((Action<int, int>) ((vehiclesLimit, vehiclesLimitLeft) =>
        {
          demandSupply.SetText(string.Format("{0} / {1}", (object) (vehiclesLimit - vehiclesLimitLeft), (object) vehiclesLimit));
          demandSupply.SetColor(vehiclesLimitLeft > 0 ? style.StatusBar.QuantityStatePositiveColor : style.Global.OrangeText);
        }));
        this.AddUpdater(updaterBuilder.Build());
      }
    }
  }
}
