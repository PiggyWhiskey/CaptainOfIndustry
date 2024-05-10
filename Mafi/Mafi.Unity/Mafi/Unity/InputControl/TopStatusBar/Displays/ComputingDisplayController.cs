// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.ComputingDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory;
using Mafi.Core.GameLoop;
using Mafi.Core.Localization.Quantity;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.Statistics;
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
  internal class ComputingDisplayController : IStatusBarItem
  {
    private readonly IComputingManager m_computingManager;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IUnityInputMgr m_inputManager;
    private readonly StatsComputingController m_computingStats;
    private readonly UiBuilder m_builder;
    private ComputingDisplayController.ComputingBarView m_computingBarView;

    public ComputingDisplayController(
      IComputingManager computingManager,
      IGameLoopEvents gameLoop,
      IUnityInputMgr inputManager,
      StatsComputingController computingStats,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_computingManager = computingManager;
      this.m_gameLoop = gameLoop;
      this.m_inputManager = inputManager;
      this.m_computingStats = computingStats;
      this.m_builder = builder;
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_computingBarView = new ComputingDisplayController.ComputingBarView(this, statusBar);
      this.m_computingBarView.BuildUi(this.m_builder);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<ComputingDisplayController>(this, new Action<GameTime>(((View) this.m_computingBarView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<ComputingDisplayController>(this, (Action<GameTime>) (x => this.m_computingBarView.RenderUpdate(x)));
    }

    private class ComputingBarView : View
    {
      private readonly ComputingDisplayController m_controller;
      private readonly StatusBar m_statusBar;

      public ComputingBarView(ComputingDisplayController controller, StatusBar statusBar)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (ComputingBarView), SyncFrequency.OncePerSec);
        this.m_controller = controller;
        this.m_statusBar = statusBar;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent1 = this.Builder.NewBtn("Container component").OnClick((Action) (() => this.m_controller.m_inputManager.ActivateNewController((IUnityInputController) this.m_controller.m_computingStats))).AddToolTip(Tr.ComputingDisplayTooltip).PutTo<Btn>((IUiElement) this);
        int size = 20;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Computing128.png", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent1, (float) size);
        Panel parent2 = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent1, new Offset(0.0f, 2f, (float) (size + 4), 2f));
        Txt demandSupply = this.Builder.NewTxt("DemandSupply").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.StatusBar.QuantityStateText).EnableRichText().PutTo<Txt>((IUiElement) parent2, Offset.LeftRight(5f));
        Txt maxProduction = this.Builder.NewTxt("MaxProduction").SetAlignment(TextAnchor.MiddleRight).SetTextStyle(style.StatusBar.QuantityChangeText).SetColor((ColorRgba) 2736606).PutTo<Txt>((IUiElement) parent2, Offset.Right(5f));
        IComputingManager computingManager = this.m_controller.m_computingManager;
        updaterBuilder.Observe<Computing>((Func<Computing>) (() => computingManager.DemandedThisTick)).Observe<Computing>((Func<Computing>) (() => computingManager.ProducedLastTick)).Do((Action<Computing, Computing>) ((demand, produced) =>
        {
          if (!this.IsVisible && (demand.IsPositive || produced.IsPositive))
          {
            this.Show();
            this.m_statusBar.OnItemChanged();
          }
          Computing computing = demand.Max(produced);
          ProductProto computingProductProto = (ProductProto) computingManager.ComputingProductProto;
          FormatInfo formatInfo = computingProductProto.QuantityFormatter.GetFormatInfo((IProtoWithIconAndName) computingProductProto, (QuantityLarge) computing.Quantity);
          demandSupply.SetText(string.Format("{0} / <size=12>{1}</size>", (object) computingProductProto.QuantityFormatter.FormatNumberOnly((QuantityLarge) demand.Quantity, formatInfo), (object) computingProductProto.QuantityFormatter.FormatNumberAndUnitOnly((QuantityLarge) produced.Quantity, formatInfo, 16)));
          demandSupply.SetColor(demand <= produced ? style.StatusBar.QuantityStatePositiveColor : style.StatusBar.QuantityNegativeColor);
        }));
        updaterBuilder.Observe<Computing>((Func<Computing>) (() => computingManager.GenerationCapacityThisTick)).Do((Action<Computing>) (max => maxProduction.SetText(max.FormatShort())));
        this.AddUpdater(updaterBuilder.Build());
        this.Hide();
        this.SetWidth<ComputingDisplayController.ComputingBarView>(220f);
        this.m_statusBar.AddElementToMiddle((IUiElement) this, 200f, true);
      }
    }
  }
}
