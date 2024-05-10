// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.Displays.ResearchProgressDisplayController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Research;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.InputControl.Research;
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
  internal class ResearchProgressDisplayController : IStatusBarItem
  {
    private readonly IUnityInputMgr m_inputMgr;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly ResearchManager m_researchManager;
    private readonly ResearchController m_researchController;
    private readonly UiBuilder m_builder;
    private readonly ResearchProgressDisplayController.ReserachProgressView m_researchResearchProgressView;
    private StatusBar m_statusBar;

    public ResearchProgressDisplayController(
      IUnityInputMgr inputMgr,
      IGameLoopEvents gameLoop,
      ResearchManager researchManager,
      ResearchController researchController,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputMgr = inputMgr;
      this.m_gameLoop = gameLoop;
      this.m_researchManager = researchManager;
      this.m_researchController = researchController;
      this.m_builder = builder;
      this.m_researchResearchProgressView = new ResearchProgressDisplayController.ReserachProgressView(this);
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      this.m_statusBar = statusBar;
      this.m_researchResearchProgressView.BuildUi(this.m_builder);
      this.m_researchResearchProgressView.SetWidth<ResearchProgressDisplayController.ReserachProgressView>(240f);
      if (this.m_researchManager.HasActiveLab)
        this.m_researchResearchProgressView.Show();
      else
        this.m_gameLoop.SyncUpdate.AddNonSaveable<ResearchProgressDisplayController>(this, new Action<GameTime>(this.waitForLabBuilt));
      statusBar.AddElementToMiddle((IUiElement) this.m_researchResearchProgressView, 100f, false);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<ResearchProgressDisplayController.ReserachProgressView>(this.m_researchResearchProgressView, new Action<GameTime>(((View) this.m_researchResearchProgressView).SyncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<ResearchProgressDisplayController.ReserachProgressView>(this.m_researchResearchProgressView, (Action<GameTime>) (x => this.m_researchResearchProgressView.RenderUpdate(x)));
    }

    private void waitForLabBuilt(GameTime time)
    {
      if (!this.m_researchManager.HasActiveLab && !this.m_researchManager.CurrentResearch.HasValue)
        return;
      this.m_researchResearchProgressView.Show();
      this.m_statusBar.OnItemChanged();
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<ResearchProgressDisplayController>(this, new Action<GameTime>(this.waitForLabBuilt));
    }

    public void OpenResearch()
    {
      this.m_inputMgr.ActivateNewController((IUnityInputController) this.m_researchController);
    }

    private class ReserachProgressView : View
    {
      private readonly ResearchProgressDisplayController m_controller;

      public ReserachProgressView(ResearchProgressDisplayController controller)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(nameof (ReserachProgressView), SyncFrequency.OncePerSec);
        this.m_controller = controller;
      }

      protected override void BuildUi()
      {
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        UiStyle style = this.Builder.Style;
        Btn parent = this.Builder.NewBtn("Container component").OnClick(new Action(this.m_controller.OpenResearch)).PutTo<Btn>((IUiElement) this);
        int iconWidth = style.StatusBar.IconWidth;
        this.Builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/Toolbar/Research.svg", style.StatusBar.IconColor).PutToLeftOf<IconContainer>((IUiElement) parent, (float) iconWidth);
        Panel textContainer = this.Builder.NewPanel("Text Container").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?(style.StatusBar.DisplayBgColor)).PutTo<Panel>((IUiElement) parent, new Offset(0.0f, 2f, (float) (iconWidth + 4), 2f));
        Panel progressBg = this.Builder.NewPanel("ProgressBg").SetBackground(this.Builder.AssetsDb.GetSharedSprite(style.StatusBar.QuantityStateBg), new ColorRgba?((ColorRgba) 2061334)).PutToLeftOf<Panel>((IUiElement) textContainer, 0.0f);
        Txt status = this.Builder.NewTxt("Status").SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(style.Global.Title).BestFitEnabled(12).PutTo<Txt>((IUiElement) textContainer, Offset.LeftRight(10f));
        ResearchManager researchManager = this.m_controller.m_researchManager;
        updaterBuilder.Observe<Option<ResearchNode>>((Func<Option<ResearchNode>>) (() => researchManager.CurrentResearch)).Observe<bool>((Func<bool>) (() => researchManager.HasActiveLab)).Observe<int>((Func<int>) (() => researchManager.ResearchQueue.Count)).Do((Action<Option<ResearchNode>, bool, int>) ((research, hasActiveLab, queueSize) =>
        {
          if (research.HasValue)
          {
            if (!hasActiveLab)
            {
              status.SetText((LocStrFormatted) Tr.NoLabAvailable);
              status.SetColor(this.Style.Global.OrangeText);
            }
            else
            {
              string translatedString = research.Value.Proto.Strings.Name.TranslatedString;
              if (queueSize > 0)
                translatedString += string.Format(" +{0}", (object) researchManager.ResearchQueue.Count);
              status.SetText(translatedString);
              status.SetColor(this.Style.Global.Text.Color);
            }
          }
          else if (hasActiveLab)
          {
            status.SetText((LocStrFormatted) Tr.NoResearchSelected);
            status.SetColor(this.Style.Global.OrangeText);
          }
          else
          {
            status.SetText("...");
            status.SetColor(this.Style.Global.Text.Color);
          }
        }));
        updaterBuilder.Observe<Percent?>((Func<Percent?>) (() => researchManager.CurrentResearch.ValueOrNull?.ProgressInPerc)).Do((Action<Percent?>) (progressMaybe =>
        {
          if (progressMaybe.HasValue)
          {
            progressBg.SetWidth<Panel>(progressMaybe.Value.Apply(textContainer.GetWidth()));
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
