// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.PopulationDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.GameLoop;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Population;
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
  internal class PopulationDisplayController : IStatusBarItem
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly UiBuilder m_builder;
    private readonly PopulationDisplayController.PopulationView m_populationView;

    public PopulationDisplayController(
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoop,
      SettlementsManager settlementsManager,
      SettlementSummaryController settlementSummary,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_builder = builder;
      this.m_populationView = new PopulationDisplayController.PopulationView(this, settlementsManager, (Action) (() => inputManager.ActivateNewController((IUnityInputController) settlementSummary)));
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_populationView.BuildUi(this.m_builder);
      this.m_populationView.SetWidth<PopulationDisplayController.PopulationView>(190f);
      this.m_populationView.Show();
      statusBar.AddElementToMiddle((IUiElement) this.m_populationView, 50f, true);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<PopulationDisplayController>(this, new Action<GameTime>(((View) this.m_populationView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<PopulationDisplayController>(this, (Action<GameTime>) (x => this.m_populationView.RenderUpdate(x)));
    }

    private class PopulationView : View
    {
      private readonly PopulationDisplayController m_controller;
      private readonly SettlementsManager m_settlementsManager;
      private readonly Action m_onClick;

      public PopulationView(
        PopulationDisplayController controller,
        SettlementsManager settlementsManager,
        Action onClick)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (PopulationView), SyncFrequency.OncePerSec);
        this.m_controller = controller;
        this.m_settlementsManager = settlementsManager;
        this.m_onClick = onClick;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent1 = this.Builder.NewBtn(nameof (PopulationView)).OnClick(this.m_onClick).PutTo<Btn>((IUiElement) this);
        Tooltip populationTooltip = parent1.AddToolTipAndReturn();
        populationTooltip.SetText((LocStrFormatted) Tr.TotalPopulation__Tooltip);
        int iconWidth = style.StatusBar.IconWidth;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Population.svg", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent1, (float) iconWidth);
        Panel parent2 = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent1, new Offset(0.0f, 2f, (float) (iconWidth + 4), 2f));
        Txt workersStatus = this.Builder.NewTxt("Population").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.StatusBar.HelperText).EnableRichText().PutTo<Txt>((IUiElement) parent2, Offset.Left(5f));
        int coord = 14;
        IconContainer diffIcon = this.Builder.NewIconContainer("DiffIcon").SetIcon("Assets/Unity/UserInterface/General/PopulationSmall.svg", style.StatusBar.IconColor).PutToRightMiddleOf<IconContainer>((IUiElement) parent1, coord.Vector2(), Offset.Right(5f));
        Txt diffTxt = this.Builder.NewTxt("Diff").SetAlignment(TextAnchor.MiddleRight).SetTextStyle(style.StatusBar.HelperText).PutToRightOf<Txt>((IUiElement) parent2, 50f, Offset.Right((float) (coord + 10)));
        updaterBuilder.Observe<int>((Func<int>) (() => this.m_settlementsManager.GetTotalPopulation())).Observe<int>((Func<int>) (() => this.m_settlementsManager.LastPopulationDiff)).Observe<int>((Func<int>) (() => this.m_settlementsManager.FreeHousingCapacity)).Do((Action<int, int, int>) ((totalPop, lastPopDiff, freeHousing) =>
        {
          int num = totalPop + freeHousing;
          string str = string.Format("<size=16><color={0}>{1}</color></size> / ", (object) this.Builder.Style.StatusBar.QuantityStatePositiveColor.ToHex(), (object) totalPop);
          string text;
          if (freeHousing < 0)
          {
            text = str + string.Format("<color={0}>{1} <size=16>!!</size></color>", (object) this.Builder.Style.Global.DangerClr.ToHex(), (object) num);
            populationTooltip.SetText((LocStrFormatted) Tr.TotalPopulation__HomelessTooltip);
          }
          else
          {
            text = str + num.ToString();
            populationTooltip.SetText((LocStrFormatted) Tr.TotalPopulation__Tooltip);
          }
          workersStatus.SetText(text);
          diffIcon.SetVisibility<IconContainer>(lastPopDiff != 0);
          diffTxt.SetVisibility<Txt>(lastPopDiff != 0);
          if (lastPopDiff == 0)
            return;
          ColorRgba color = lastPopDiff > 0 ? this.Builder.Style.Global.GreenForDark : this.Builder.Style.Global.DangerClr;
          diffIcon.SetColor(color);
          diffTxt.SetColor(color);
          diffTxt.SetText(lastPopDiff > 0 ? string.Format("+{0}", (object) lastPopDiff) : lastPopDiff.ToString());
        }));
        this.AddUpdater(updaterBuilder.Build());
      }
    }
  }
}
