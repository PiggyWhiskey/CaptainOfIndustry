// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Research.ResearchFinishedMessageWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Components.Levelling;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Research
{
  internal class ResearchFinishedMessageWindowView : View, IUnityInputController
  {
    private Panel m_background;
    private StackContainer m_content;
    private Panel m_levelHeadline;
    private IconContainer m_upLevelIcon;
    private Txt m_newLevelText;
    private LevelProtoUnlocksView m_unlocksView;
    private readonly IUnityInputMgr m_inputManager;
    private readonly ResearchController m_researchController;
    private readonly ResearchManager m_researchManager;
    private Btn m_openResearchBtn;
    private StackContainer m_actionsContainer;
    private Btn m_dismissBtn;

    public ControllerConfig Config => ControllerConfig.Window;

    public ResearchFinishedMessageWindowView(
      IUnityInputMgr inputManager,
      ResearchController researchController,
      ResearchManager researchManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(nameof (ResearchFinishedMessageWindowView));
      this.m_inputManager = inputManager;
      this.m_researchController = researchController;
      this.m_researchManager = researchManager;
    }

    public void ShowNewLevelInfo(string researchName, IEnumerable<Proto> unlockedProtos)
    {
      this.updateHeadline(researchName);
      this.m_unlocksView.SetProtos(unlockedProtos);
      this.m_content.UpdateSizesFromItems();
      this.SendToFront<ResearchFinishedMessageWindowView>();
      this.PositionToTopMiddle(new Offset?(Offset.Top(50f)));
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
      bool hasValue = this.m_researchManager.CurrentResearch.HasValue;
      this.m_openResearchBtn.SetButtonStyle(hasValue ? this.Builder.Style.Global.GeneralBtn : this.Builder.Style.Global.PrimaryBtn);
      this.m_openResearchBtn.SetText((LocStrFormatted) (hasValue ? Tr.OpenResearch_Action : Tr.StartNewResearch_Action));
      this.m_dismissBtn.SetButtonStyle(!hasValue ? this.Builder.Style.Global.GeneralBtn : this.Builder.Style.Global.PrimaryBtn);
      this.m_actionsContainer.UpdateItemSize((IUiElement) this.m_openResearchBtn, this.m_openResearchBtn.GetOptimalSize() + new Vector2(50f, 0.0f));
    }

    protected override void BuildUi()
    {
      ColorRgba colorRgba = new ColorRgba(0, 222);
      this.m_background = this.Builder.NewPanel(nameof (ResearchFinishedMessageWindowView)).SetBackground(colorRgba);
      this.m_background.PutTo<Panel>((IUiElement) this);
      string spriteAssetPath = "Assets/Unity/UserInterface/Levelling/MessageWindowGradient128.png";
      float size1 = 80f;
      this.Builder.NewIconContainer("RightGradient").SetIcon(spriteAssetPath, colorRgba, false).PutToRightOf<IconContainer>((IUiElement) this.m_background, size1, Offset.Right(-size1));
      this.Builder.NewIconContainer("LeftGradient").SetIcon(spriteAssetPath, colorRgba, false).PutToLeftOf<IconContainer>((IUiElement) this.m_background, size1).RectTransform.localScale = new Vector3(-1f, 1f, 1f);
      this.m_content = this.Builder.NewStackContainer("Content").SetSizeMode(StackContainer.SizeMode.StaticCenterAligned).SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(8f).PutTo<StackContainer>((IUiElement) this, Offset.TopBottom(12f));
      TextStyle text = this.Style.Global.Text;
      ref TextStyle local1 = ref text;
      int? nullable = new int?(14);
      ColorRgba? color1 = new ColorRgba?(ColorRgba.White);
      FontStyle? fontStyle1 = new FontStyle?();
      int? fontSize1 = nullable;
      bool? isCapitalized1 = new bool?();
      TextStyle textStyle1 = local1.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
      this.m_levelHeadline = this.Builder.NewPanel("Headline").AppendTo<Panel>(this.m_content, new Vector2?(Vector2.zero), ContainerPosition.MiddleOrCenter, Offset.Bottom(5f));
      this.m_upLevelIcon = this.Builder.NewIconContainer("ResearchIcon").SetIcon("Assets/Unity/UserInterface/Toolbar/Research.svg", ColorRgba.White, false).PutToLeftBottomOf<IconContainer>((IUiElement) this.m_levelHeadline, new Vector2(20f, 30f));
      Txt txt = this.Builder.NewTxt("ResearchTitleTxt");
      ref TextStyle local2 = ref textStyle1;
      nullable = new int?(22);
      ColorRgba? color2 = new ColorRgba?();
      FontStyle? fontStyle2 = new FontStyle?();
      int? fontSize2 = nullable;
      bool? isCapitalized2 = new bool?();
      TextStyle textStyle2 = local2.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
      this.m_newLevelText = txt.SetTextStyle(textStyle2).EnableRichText().AllowHorizontalOverflow().PutToLeftBottomOf<Txt>((IUiElement) this.m_levelHeadline, Vector2.zero, Offset.Left(32f));
      this.m_unlocksView = new LevelProtoUnlocksView(this.Builder, new LevelProtoUnlocksView.Style(50, 30, 4f, 4f, 8f, textStyle1));
      this.m_unlocksView.AppendTo<LevelProtoUnlocksView>(this.m_content, new Vector2?(this.m_unlocksView.GetSize()), ContainerPosition.MiddleOrCenter);
      this.m_actionsContainer = this.Builder.NewStackContainer("actions const").SetSizeMode(StackContainer.SizeMode.Dynamic).SetStackingDirection(StackContainer.Direction.LeftToRight).SetItemSpacing(10f);
      this.m_dismissBtn = this.Builder.NewBtn("Dismiss").SetButtonStyle(this.Style.Global.GeneralBtn).SetText((LocStrFormatted) Tr.Dismiss);
      Vector2 size2 = this.m_dismissBtn.GetOptimalSize() + new Vector2(50f, 0.0f);
      this.m_dismissBtn.SetSize<Btn>(size2).AppendTo<Btn>(this.m_actionsContainer, new Vector2?(size2), ContainerPosition.MiddleOrCenter).OnClick(new Action(this.deactivateSelf));
      this.m_openResearchBtn = this.Builder.NewBtnPrimary("openResearch").SetText((LocStrFormatted) Tr.StartNewResearch_Action);
      Vector2 size3 = this.m_openResearchBtn.GetOptimalSize() + new Vector2(50f, 0.0f);
      this.m_openResearchBtn.SetSize<Btn>(size3).AppendTo<Btn>(this.m_actionsContainer, new Vector2?(size3), ContainerPosition.MiddleOrCenter).OnClick((Action) (() =>
      {
        this.deactivateSelf();
        this.m_inputManager.ActivateNewController((IUnityInputController) this.m_researchController);
      }));
      this.m_actionsContainer.AppendTo<StackContainer>(this.m_content, new Vector2?(new Vector2(size2.x + 10f + size3.x, size2.y)), ContainerPosition.MiddleOrCenter);
      this.PositionToTopMiddle(new Offset?(Offset.Top(50f)));
    }

    private void updateHeadline(string researchName)
    {
      this.m_newLevelText.SetText(Tr.ResearchUnlocked.Format(researchName));
      Vector2 preferedSize = this.m_newLevelText.GetPreferedSize();
      this.m_newLevelText.SetSize<Txt>(preferedSize);
      float width = (float) ((double) preferedSize.x + 20.0 + 12.0);
      float height = preferedSize.y.Max(this.m_upLevelIcon.GetHeight());
      this.m_levelHeadline.SetWidth<Panel>(width);
      this.m_levelHeadline.SetHeight<Panel>(height);
    }

    /// <summary>
    /// Positions the window to the corner of the screen while setting it the given dimensions.
    /// </summary>
    protected void PositionToTopMiddle(Offset? offset = null, bool gameOverlayParent = false)
    {
      this.PutToCenterTopOf<ResearchFinishedMessageWindowView>(gameOverlayParent ? (IUiElement) this.Builder.GameOverlay : (IUiElement) this.Builder.MainCanvas, new Vector2(440f, (this.m_content.GetDynamicHeight() + 24f).Max(50f)), offset ?? Offset.Zero);
    }

    private void deactivateSelf()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    public void Activate() => this.Show();

    public void Deactivate() => this.Hide();

    public bool InputUpdate(IInputScheduler inputScheduler) => false;
  }
}
