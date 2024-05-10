// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Population.NewRefugeesInfoView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.MessageNotifications.Notifications;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Population
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class NewRefugeesInfoView : WindowView, IUnityUi, IUnityInputController
  {
    private ProductRewardsView m_rewardsView;
    private StackContainer m_stackContainer;
    private Txt m_amountOfRefugees;
    private Txt m_rewardsTitle;
    private readonly IUnityInputMgr m_inputManager;

    public ControllerConfig Config => ControllerConfig.Window;

    public NewRefugeesInfoView(IUnityInputMgr inputManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("RefugeesPendingView", WindowView.FooterStyle.Flat);
      this.m_inputManager = inputManager;
    }

    public void RegisterUi(UiBuilder builder) => this.BuildUi(builder);

    protected override void BuildWindowContent()
    {
      this.m_rewardsView = new ProductRewardsView(this.Builder);
      this.SetTitle((LocStrFormatted) Tr.NewRefugees);
      this.m_stackContainer = this.Builder.NewStackContainer("Container").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).PutToTopOf<StackContainer>((IUiElement) this.GetContentPanel(), 0.0f);
      this.Builder.NewTxt("Intro").SetText((LocStrFormatted) Tr.NewRefugees__Beacon).SetTextStyle(this.Style.Global.Text).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_stackContainer, new float?(30f), Offset.Bottom(10f));
      Panel parent1 = this.Builder.NewPanel("RefAmountContainer").SetBackground(this.Builder.Style.Panel.ItemOverlay).AppendTo<Panel>(this.m_stackContainer, new float?(60f), Offset.Bottom(10f));
      Txt txt = this.Builder.NewTxt("AmountOfRefugees").SetText("+ 10");
      TextStyle text = this.Builder.Style.Global.Text;
      ref TextStyle local = ref text;
      int? nullable = new int?(14);
      ColorRgba? color = new ColorRgba?();
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = nullable;
      bool? isCapitalized = new bool?();
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_amountOfRefugees = txt.SetTextStyle(textStyle).SetAlignment(TextAnchor.MiddleLeft).PutTo<Txt>((IUiElement) parent1, Offset.Left(10f));
      this.Builder.NewIconContainer("PopIcon").SetIcon("Assets/Unity/UserInterface/General/Population.svg").PutToLeftMiddleOf<IconContainer>((IUiElement) parent1, new Vector2(40f, 40f), Offset.Left((float) ((double) this.m_amountOfRefugees.GetPreferedWidth() + 10.0 + 5.0)));
      this.m_rewardsTitle = this.Builder.NewTxt("RewardsTitle").SetText((LocStrFormatted) Tr.LootReceived).SetTextStyle(this.Builder.Style.Global.Title).SetAlignment(TextAnchor.MiddleLeft).AppendTo<Txt>(this.m_stackContainer, new float?(20f), Offset.Left(10f));
      this.m_rewardsView.AppendTo<ProductRewardsView>(this.m_stackContainer, new float?(0.0f));
      Panel parent2 = this.Builder.NewPanel("BtnsContainer").AppendTo<Panel>(this.m_stackContainer, new float?(30f), Offset.LeftRight(20f) + Offset.Top(15f));
      this.Builder.NewBtnPrimary("Close").OnClick(new Action(this.onCloseClick)).SetText((LocStrFormatted) Tr.ConfirmGreatNews).PutToCenterOf<Btn>((IUiElement) parent2, 120f);
    }

    public void ShowRefugeesMessage(NewRefugeesMessage message)
    {
      this.m_amountOfRefugees.SetText(string.Format("+ {0}", (object) message.AmountOfRefugeesAdded));
      this.m_rewardsView.SetReward(message.Reward);
      this.m_stackContainer.SetItemVisibility((IUiElement) this.m_rewardsTitle, message.Reward.IsNotEmpty);
      this.m_stackContainer.SetItemVisibility((IUiElement) this.m_rewardsView, message.Reward.IsNotEmpty);
      this.SetContentSize(400f, this.m_stackContainer.GetDynamicHeight());
      this.PositionSelfToCenter();
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    private void onCloseClick()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    public void Activate() => this.Show();

    public void Deactivate() => this.Hide();

    public bool InputUpdate(IInputScheduler inputScheduler) => false;
  }
}
