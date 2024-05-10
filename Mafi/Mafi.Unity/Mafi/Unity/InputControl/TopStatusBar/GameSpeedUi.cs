// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TopStatusBar.GameSpeedUi
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.TopStatusBar
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class GameSpeedUi : IStatusBarItem
  {
    private readonly GameSpeedController m_controller;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly UiBuilder m_builder;
    private Panel m_playActivePanel;
    private Panel m_pauseActivePanel;
    private Panel m_fastForwardActivePanel;
    private Panel m_superfastForwardActivePanel;
    private int m_currentSpeedIndex;
    private Panel m_pausedMessageContainer;

    public GameSpeedUi(
      GameSpeedController gameSpeedController,
      IGameLoopEvents gameLoop,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_currentSpeedIndex = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_controller = gameSpeedController;
      this.m_gameLoop = gameLoop;
      this.m_builder = builder;
    }

    public void RegisterIntoStatusBar(StatusBar statusBar)
    {
      UiStyle style = this.m_builder.Style;
      this.m_pausedMessageContainer = this.m_builder.NewPanel("Paused").SetBackground(style.Global.PanelsBg).Hide<Panel>();
      this.m_builder.NewPanel("Shadow").SetBackground(style.Icons.BorderGradient, new ColorRgba?(style.Global.PanelsBg)).PutTo<Panel>((IUiElement) this.m_pausedMessageContainer, Offset.All(-6f));
      Panel container = this.m_builder.NewPanel("CenteredHolder").PutToCenterOf<Panel>((IUiElement) this.m_pausedMessageContainer, 0.0f);
      IconContainer leftMiddleOf = this.m_builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/EnableMachine128.png", (ColorRgba) 12303291).PutToLeftMiddleOf<IconContainer>((IUiElement) container, 12.Vector2());
      Txt txt = this.m_builder.NewTxt("Paused").SetText((LocStrFormatted) Tr.Paused).SetAlignment(TextAnchor.MiddleRight).SetTextStyle(this.m_builder.Style.StatusBar.PausedTextStyle).PutTo<Txt>((IUiElement) container);
      container.SetWidth<Panel>((float) ((double) txt.GetPreferedWidth() + (double) leftMiddleOf.GetWidth() + 5.0));
      IUiElement buttonsPanel = this.createButtonsPanel(this.m_builder);
      statusBar.AddElementToRight(buttonsPanel, 200f, false);
      statusBar.OnHeightChanged += new Action<float>(positionPausedMessage);
      this.m_gameLoop.SyncUpdate.AddNonSaveable<GameSpeedUi>(this, new Action<GameTime>(this.syncUpdate));

      void positionPausedMessage(float barHeight)
      {
        this.m_pausedMessageContainer.PutToCenterTopOf<Panel>((IUiElement) this.m_builder.MainCanvas, new Vector2(container.GetWidth() + 60f, 36f), Offset.Top(barHeight + 15f));
      }
    }

    /// <summary>
    /// Builds button that goes to the bottom menu strip of the main menu.
    /// </summary>
    private IUiElement createButtonsPanel(UiBuilder builder)
    {
      UiStyle style = builder.Style;
      int num = 4;
      Panel container = builder.NewPanel("Play / pause").SetHeight<Panel>(30f).SetWidth<Panel>((float) ((double) style.StatusBar.PauseButtonOffset.LeftRightOffset + (double) num * (double) style.StatusBar.PauseIconSize.x + (double) (num - 1) * (double) style.StatusBar.PlayPauseSpacing));
      this.m_pauseActivePanel = buildPane(GameSpeedUi.PaneType.Pause);
      this.m_playActivePanel = buildPane(GameSpeedUi.PaneType.Play);
      this.m_fastForwardActivePanel = buildPane(GameSpeedUi.PaneType.FastForward);
      this.m_superfastForwardActivePanel = buildPane(GameSpeedUi.PaneType.SuperFastForward);
      return (IUiElement) container;

      Panel buildPane(GameSpeedUi.PaneType paneType)
      {
        Panel parent = builder.NewPanel(string.Format("{0} active", (object) paneType)).PutTo<Panel>((IUiElement) container, style.StatusBar.PauseButtonOffset);
        BtnStyle playButtonEnabled = style.StatusBar.PlayButtonEnabled;
        BtnStyle playButtonDisabled = style.StatusBar.PlayButtonDisabled;
        builder.NewBtn("Pause").SetButtonStyle(paneType == GameSpeedUi.PaneType.Pause ? playButtonEnabled : playButtonDisabled).SetIcon(style.Icons.Pause).OnClick((Action) (() => this.m_controller.SetSpeedIndex(0))).PutRelativeTo<Btn>((IUiElement) parent, style.StatusBar.PauseIconSize, HorizontalPosition.Left, VerticalPosition.Middle, Offset.Left(0.0f));
        builder.NewBtn("Play").SetButtonStyle(paneType == GameSpeedUi.PaneType.Play ? playButtonEnabled : playButtonDisabled).SetIcon(style.Icons.Play).OnClick((Action) (() => this.m_controller.SetSpeedIndex(1))).PutRelativeTo<Btn>((IUiElement) parent, style.StatusBar.PauseIconSize, HorizontalPosition.Left, VerticalPosition.Middle, Offset.Left(style.StatusBar.PlayPauseSpacing + style.StatusBar.PauseIconSize.x));
        builder.NewBtn("Fast forward").SetButtonStyle(paneType == GameSpeedUi.PaneType.FastForward ? playButtonEnabled : playButtonDisabled).SetIcon(style.Icons.FastForward).OnClick((Action) (() => this.m_controller.SetSpeedIndex(2))).PutRelativeTo<Btn>((IUiElement) parent, style.StatusBar.PauseIconSize, HorizontalPosition.Left, VerticalPosition.Middle, Offset.Left((float) (2.0 * ((double) style.StatusBar.PlayPauseSpacing + (double) style.StatusBar.PauseIconSize.x))));
        builder.NewBtn("Super fast forward").SetButtonStyle(paneType == GameSpeedUi.PaneType.SuperFastForward ? playButtonEnabled : playButtonDisabled).SetIcon(style.Icons.SuperFastForward).OnClick((Action) (() => this.m_controller.SetSpeedIndex(3))).PutRelativeTo<Btn>((IUiElement) parent, style.StatusBar.PauseIconSize, HorizontalPosition.Left, VerticalPosition.Middle, Offset.Left((float) (3.0 * ((double) style.StatusBar.PlayPauseSpacing + (double) style.StatusBar.PauseIconSize.x))));
        return parent;
      }
    }

    private void syncUpdate(GameTime gameTime)
    {
      int currentSpeedIndex = this.m_controller.GetCurrentSpeedIndex();
      if (this.m_currentSpeedIndex == currentSpeedIndex)
        return;
      this.m_currentSpeedIndex = currentSpeedIndex;
      this.m_pausedMessageContainer.SetVisibility<Panel>(this.m_currentSpeedIndex == 0);
      this.m_pauseActivePanel.SetVisibility<Panel>(this.m_currentSpeedIndex == 0);
      this.m_playActivePanel.SetVisibility<Panel>(this.m_currentSpeedIndex == 1);
      this.m_fastForwardActivePanel.SetVisibility<Panel>(this.m_currentSpeedIndex == 2);
      this.m_superfastForwardActivePanel.SetVisibility<Panel>(this.m_currentSpeedIndex == 3);
    }

    private enum PaneType
    {
      Pause,
      Play,
      FastForward,
      SuperFastForward,
    }
  }
}
