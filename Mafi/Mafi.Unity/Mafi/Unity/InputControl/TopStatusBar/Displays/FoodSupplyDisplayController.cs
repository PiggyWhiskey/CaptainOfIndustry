// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.FoodSupplyDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.GameLoop;
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
  internal class FoodSupplyDisplayController : IStatusBarItem
  {
    private readonly UiBuilder m_builder;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly SettlementsManager m_settlementsManager;
    private readonly FoodSupplyDisplayController.FoodSupplyView m_supplyView;

    public FoodSupplyDisplayController(
      IGameLoopEvents gameLoop,
      SettlementsManager settlementsManager,
      StatisticsController statisticsController,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_settlementsManager = settlementsManager;
      this.m_builder = builder;
      this.m_supplyView = new FoodSupplyDisplayController.FoodSupplyView(this, statisticsController);
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_supplyView.BuildUi(this.m_builder);
      this.m_supplyView.SetWidth<FoodSupplyDisplayController.FoodSupplyView>(180f);
      this.m_supplyView.Show();
      statusBar.AddElementToMiddle((IUiElement) this.m_supplyView, 20f, true);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<FoodSupplyDisplayController>(this, new Action<GameTime>(((View) this.m_supplyView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<FoodSupplyDisplayController>(this, (Action<GameTime>) (x => this.m_supplyView.RenderUpdate(x)));
    }

    private class FoodSupplyView : View
    {
      private readonly FoodSupplyDisplayController m_controller;
      private readonly StatisticsController m_statisticsController;

      public FoodSupplyView(
        FoodSupplyDisplayController controller,
        StatisticsController statisticsController)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (FoodSupplyView), SyncFrequency.OncePerSec);
        this.m_controller = controller;
        this.m_statisticsController = statisticsController;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent1 = this.Builder.NewBtn("Container component").AddToolTip(Tr.FoodLeftMainPanel__Tooltip).OnClick((Action) (() => this.m_statisticsController.OpenAndShowFoodStats())).PutTo<Btn>((IUiElement) this);
        int iconWidth = style.StatusBar.IconWidth;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Food.svg", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent1, (float) iconWidth);
        Panel parent2 = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent1, new Offset(0.0f, 2f, (float) (iconWidth + 4), 2f));
        Txt supplyStatus = this.Builder.NewTxt("Supply").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.StatusBar.HelperText).EnableRichText().PutTo<Txt>((IUiElement) parent2, Offset.Left(5f));
        updaterBuilder.Observe<int>((Func<int>) (() => this.m_controller.m_settlementsManager.MonthsOfFood)).Do((Action<int>) (monthsOfFood =>
        {
          ColorRgba colorRgba = monthsOfFood > 6 ? (monthsOfFood > 12 ? this.Builder.Style.StatusBar.QuantityStatePositiveColor : this.Builder.Style.Global.OrangeText) : this.Builder.Style.Global.DangerClr;
          if (monthsOfFood > 99)
            monthsOfFood = 99;
          string str = string.Format("<size=16><color={0}>{1}</color></size>", (object) colorRgba.ToHex(), (object) monthsOfFood);
          supplyStatus.SetText(TrCore.NumberOfMonths.Format(str, monthsOfFood).Value);
        }));
        this.AddUpdater(updaterBuilder.Build());
      }
    }
  }
}
