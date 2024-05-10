// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.WorkersDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.GameLoop;
using Mafi.Core.Population;
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
  internal class WorkersDisplayController : IStatusBarItem
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IWorkersManager m_workersManager;
    private readonly SettlementsManager m_settlementsManager;
    private readonly StatsWorkersController m_workersStats;
    private readonly IUnityInputMgr m_inputMgr;
    private readonly UiBuilder m_builder;
    private readonly WorkersDisplayController.WorkersView m_workersView;

    public WorkersDisplayController(
      IGameLoopEvents gameLoop,
      IWorkersManager workersManager,
      SettlementsManager settlementsManager,
      StatsWorkersController workersStats,
      IUnityInputMgr inputMgr,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_workersManager = workersManager;
      this.m_settlementsManager = settlementsManager;
      this.m_workersStats = workersStats;
      this.m_inputMgr = inputMgr;
      this.m_builder = builder;
      this.m_workersView = new WorkersDisplayController.WorkersView(this);
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_workersView.BuildUi(this.m_builder);
      this.m_workersView.SetWidth<WorkersDisplayController.WorkersView>(200f);
      this.m_workersView.Show();
      statusBar.AddElementToMiddle((IUiElement) this.m_workersView, 50f, true);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<WorkersDisplayController>(this, new Action<GameTime>(((View) this.m_workersView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<WorkersDisplayController>(this, (Action<GameTime>) (x => this.m_workersView.RenderUpdate(x)));
    }

    private class WorkersView : View
    {
      private readonly WorkersDisplayController m_controller;

      public WorkersView(WorkersDisplayController controller)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (WorkersView), SyncFrequency.OncePerSec);
        this.m_controller = controller;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent1 = this.Builder.NewBtn("Container component").OnClick((Action) (() => this.m_controller.m_inputMgr.ActivateNewController((IUnityInputController) this.m_controller.m_workersStats))).PutTo<Btn>((IUiElement) this);
        Tooltip tooltip = parent1.AddToolTipAndReturn();
        int iconWidth = style.StatusBar.IconWidth;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/WorkerSmall.svg", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent1, (float) iconWidth);
        Panel parent2 = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent1, new Offset(0.0f, 2f, (float) (iconWidth + 4), 2f));
        Txt workersStatus = this.Builder.NewTxt("Workers").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.StatusBar.HelperText).EnableRichText().PutTo<Txt>((IUiElement) parent2, Offset.Left(5f));
        updaterBuilder.Observe<int>((Func<int>) (() => this.m_controller.m_workersManager.AmountOfFreeWorkersOrMissing)).Observe<int>((Func<int>) (() => this.m_controller.m_workersManager.NumberOfWorkersWithheld)).Observe<int>((Func<int>) (() => this.m_controller.m_settlementsManager.NumberOfStarvingPopsWithheld)).Do((Action<int, int, int>) ((freeWorkers, withheldWorkersQuarantine, withheldWorkersStarving) =>
        {
          int num = withheldWorkersQuarantine + withheldWorkersStarving;
          string str = string.Format("<size=16><color={0}>{1}</color></size>", (object) (freeWorkers >= 0 ? (freeWorkers == 0 || num > 0 ? this.Builder.Style.Global.OrangeText : this.Builder.Style.StatusBar.QuantityStatePositiveColor) : this.Builder.Style.Global.DangerClr).ToHex(), (object) freeWorkers);
          workersStatus.SetText(Tr.AmountOfWorkers.Format(str, freeWorkers).Value);
          string translatedString = Tr.WorkersAvailable__Tooltip.TranslatedString;
          if (num > 0)
            translatedString += string.Format("\n\n<color=#FF9900FF>{0}:", (object) Tr.PopsCannotWorkTitle);
          if (withheldWorkersStarving > 0)
            translatedString += string.Format("\n- {0}: <b>{1}</b>", (object) Tr.PopsCannotWork__Starving, (object) Tr.AmountOfPops.Format(withheldWorkersStarving));
          if (withheldWorkersQuarantine > 0)
            translatedString += string.Format("\n- {0}: <b>{1}</b>", (object) Tr.PopsCannotWork__Quarantine, (object) Tr.AmountOfPops.Format(withheldWorkersQuarantine));
          tooltip.SetText(translatedString + "</color>");
        }));
        this.AddUpdater(updaterBuilder.Build());
      }
    }
  }
}
