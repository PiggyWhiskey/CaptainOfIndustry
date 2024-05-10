// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.ElectricityDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory.ElectricPower;
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
  internal class ElectricityDisplayController : IStatusBarItem
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IElectricityManager m_electricityManager;
    private readonly StatsElectricityController m_electricityStats;
    private readonly IUnityInputMgr m_inputMgr;
    private readonly UiBuilder m_builder;
    private ElectricityDisplayController.ElectricityBarView m_electricityBarView;

    public ElectricityDisplayController(
      IGameLoopEvents gameLoop,
      IElectricityManager electricityManager,
      StatsElectricityController electricityStats,
      IUnityInputMgr inputMgr,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_electricityManager = electricityManager;
      this.m_electricityStats = electricityStats;
      this.m_inputMgr = inputMgr;
      this.m_builder = builder;
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_electricityBarView = new ElectricityDisplayController.ElectricityBarView(this, statusBar);
      this.m_electricityBarView.BuildUi(this.m_builder);
      statusBar.AddElementToMiddle((IUiElement) this.m_electricityBarView, 150f, true);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<ElectricityDisplayController>(this, new Action<GameTime>(((View) this.m_electricityBarView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<ElectricityDisplayController>(this, (Action<GameTime>) (x => this.m_electricityBarView.RenderUpdate(x)));
    }

    private class ElectricityBarView : View
    {
      private readonly ElectricityDisplayController m_controller;
      private readonly StatusBar m_statusBar;

      public ElectricityBarView(ElectricityDisplayController controller, StatusBar statusBar)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (ElectricityBarView), SyncFrequency.OncePerSec);
        this.m_controller = controller;
        this.m_statusBar = statusBar;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent1 = this.Builder.NewBtn("Container component").OnClick((Action) (() => this.m_controller.m_inputMgr.ActivateNewController((IUnityInputController) this.m_controller.m_electricityStats))).AddToolTip(Tr.ElectricityDisplayTooltip).PutTo<Btn>((IUiElement) this);
        int size = 20;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Electricity.svg", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent1, (float) size);
        Panel parent2 = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent1, new Offset(0.0f, 2f, (float) (size + 4), 2f));
        Txt demandSupply = this.Builder.NewTxt("DemandSupply").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.StatusBar.QuantityStateText).EnableRichText().PutTo<Txt>((IUiElement) parent2, Offset.Left(5f));
        Txt maxProduction = this.Builder.NewTxt("MaxProduction").SetAlignment(TextAnchor.MiddleRight).SetTextStyle(style.StatusBar.QuantityChangeText).SetColor((ColorRgba) 2736606).PutTo<Txt>((IUiElement) parent2, Offset.Right(5f));
        IElectricityManager electricityManager = this.m_controller.m_electricityManager;
        updaterBuilder.Observe<Electricity>((Func<Electricity>) (() => electricityManager.DemandedThisTick)).Observe<Electricity>((Func<Electricity>) (() => electricityManager.ConsumedThisTick)).Do((Action<Electricity, Electricity>) ((demand, consumed) =>
        {
          if (!this.IsVisible && (demand.IsPositive || consumed.IsPositive))
          {
            this.Show();
            this.m_statusBar.OnItemChanged();
          }
          Electricity electricity = demand.Max(consumed);
          VirtualProductProto electricityProto = electricityManager.ElectricityProto;
          FormatInfo formatInfo = electricityProto.QuantityFormatter.GetFormatInfo((IProtoWithIconAndName) electricityProto, (QuantityLarge) electricity.Quantity);
          demandSupply.SetText(string.Format("{0} / <size=12>{1}</size>", (object) electricityProto.QuantityFormatter.FormatNumberOnly((QuantityLarge) demand.Quantity, formatInfo), (object) electricityProto.QuantityFormatter.FormatNumberAndUnitOnly((QuantityLarge) consumed.Quantity, formatInfo, 16)));
          demandSupply.SetColor(demand <= consumed ? style.StatusBar.QuantityStatePositiveColor : style.StatusBar.QuantityNegativeColor);
        }));
        updaterBuilder.Observe<Electricity>((Func<Electricity>) (() => electricityManager.GenerationCapacityThisTick)).Do((Action<Electricity>) (max => maxProduction.SetText(max.Format())));
        this.AddUpdater(updaterBuilder.Build());
        this.SetWidth<ElectricityDisplayController.ElectricityBarView>(230f);
        this.Hide();
      }
    }
  }
}
