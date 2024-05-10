// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.MessageNotifications.Handlers.GameOverMessageView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Localization;
using Mafi.Unity.MainMenu;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.MessageNotifications.Handlers
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class GameOverMessageView : WindowView
  {
    private readonly IMain m_main;
    private Txt m_text;
    private StackContainer m_btnsContainer;
    private Btn m_dismissBtn;

    public GameOverMessageView(IMain main, UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (GameOverMessageView), WindowView.FooterStyle.Flat);
      this.m_main = main;
      this.BuildUi(builder);
    }

    protected override void BuildWindowContent()
    {
      Txt txt = this.Builder.NewTxt("over").SetAlignment(TextAnchor.UpperLeft);
      TextStyle text = this.Builder.Style.Global.Text;
      ref TextStyle local = ref text;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_text = txt.SetTextStyle(textStyle).PutToTopOf<Txt>((IUiElement) this.GetContentPanel(), 40f, Offset.All(30f));
      this.m_btnsContainer = this.Builder.NewStackContainer("BtnsContainer").SetSizeMode(StackContainer.SizeMode.StaticCenterAligned).SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(20f).SetInnerPadding(Offset.LeftRight(20f)).PutToBottomOf<StackContainer>((IUiElement) this.GetContentPanel(), 40f, Offset.All(20f));
      this.m_dismissBtn = this.Builder.NewBtn("Close").SetButtonStyle(this.Style.Global.GeneralBtn).OnClick(new Action(((View) this).Hide)).SetText((LocStrFormatted) Tr.Dismiss).AppendTo<Btn>(this.m_btnsContainer, new float?(150f));
      this.Builder.NewBtnPrimary("quit").OnClick((Action) (() => this.m_main.GoToMainMenu(new MainMenuArgs()))).SetText((LocStrFormatted) Tr.QuitGame).AppendTo<Btn>(this.m_btnsContainer, new float?(150f));
    }

    public void ShowGameOverNotification(GameOverNotification message)
    {
      this.SetTitle(message.Title);
      this.m_text.SetText(message.Message);
      this.m_btnsContainer.SetItemVisibility((IUiElement) this.m_dismissBtn, message.CanBeDismissed);
      this.SetContentSize(400f, 200f);
      this.PositionSelfToCenter();
      this.Show();
    }
  }
}
