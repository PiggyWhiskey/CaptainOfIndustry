// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Research.SidePanel.ResearchSidePanel
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Research;
using Mafi.Core.Syncers;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Components.UnlockingTreeView;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Research.SidePanel
{
  /// <summary>The side panel that displays a research node details.</summary>
  internal class ResearchSidePanel : View
  {
    private Option<ResearchNode> m_node;
    private StackContainer m_container;
    private Txt m_title;
    private Txt m_desc;
    private Btn m_startBtn;
    private Txt m_progress;
    private Panel m_blockedInfo;
    private Btn m_cancelBtn;
    private Panel m_finishedInfo;
    private Panel m_lockedInfo;
    private readonly LazyResolve<ResearchController> m_controller;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDbForUi;
    private readonly RecipesBookController m_recipesBookController;
    private readonly ResearchPopupController m_researchPopupController;
    private readonly Action m_onEnqueueClick;
    private readonly Action m_onDequeueClick;
    private readonly Action m_onFinishCheatClick;
    private readonly Action m_onStartClick;
    private readonly Action m_onCancelClick;
    private Panel m_titleHolder;
    private RecipesView m_recipes;
    private IconsWithTitlesView m_unlocks;
    private NodeColorsSelector m_nodeColorSelector;
    private IconsWithTitlesView m_requiredProtos;
    private ScrollableContainer m_scrollableContainer;
    private StackContainer m_innerContainer;
    private Btn m_addToQueue;
    private Btn m_removeFromQueue;
    private Txt m_inQueueText;

    public event Action<float> OnWidthChanged;

    internal ResearchSidePanel(
      LazyResolve<ResearchController> controller,
      UnlockedProtosDbForUi unlockedProtosDbForUi,
      RecipesBookController recipesBookController,
      ResearchPopupController researchPopupController,
      Action onStartClick,
      Action onCancelClick,
      Action onFinishCheatClick,
      Action onEnqueueClick,
      Action onDequeueClick)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("ResearchDetail");
      this.m_controller = controller;
      this.m_unlockedProtosDbForUi = unlockedProtosDbForUi;
      this.m_recipesBookController = recipesBookController;
      this.m_researchPopupController = researchPopupController;
      this.m_onEnqueueClick = onEnqueueClick;
      this.m_onDequeueClick = onDequeueClick;
      this.m_onStartClick = onStartClick.CheckNotNull<Action>();
      this.m_onCancelClick = onCancelClick.CheckNotNull<Action>();
      this.m_onFinishCheatClick = onFinishCheatClick.CheckNotNull<Action>();
    }

    protected override void BuildUi()
    {
      ResearchWindowUiStyle research = this.Style.Research;
      this.m_nodeColorSelector = new NodeColorsSelector(this.Style);
      this.m_container = this.Builder.NewStackContainer("Container", (IUiElement) this).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetBackground(new ColorRgba(3684408)).SetItemSpacing(10f).PutToMiddleOf<StackContainer>((IUiElement) this, 0.0f);
      this.m_titleHolder = this.Builder.NewPanel("TitleBar", (IUiElement) this.m_container).SetBorderStyle(new BorderStyle(ColorRgba.Black)).AppendTo<Panel>(this.m_container, new float?(30f));
      this.m_title = this.Builder.NewTxt("Title", (IUiElement) this.m_titleHolder).SetTextStyle(research.NodeDetailTitleText).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_titleHolder);
      this.m_blockedInfo = this.Builder.NewPanel("BlockedInfo", (IUiElement) this.m_container).AppendTo<Panel>(this.m_container, new float?(20f));
      Txt txt1 = this.Builder.NewTxt("Text", (IUiElement) this.m_blockedInfo).SetText((LocStrFormatted) Tr.NoLabAvailable).SetAlignment(TextAnchor.MiddleLeft).SetTextStyle(research.NodeDetailBlockedText);
      txt1.PutToCenterOf<Txt>((IUiElement) this.m_blockedInfo, txt1.GetPreferedWidth());
      this.Builder.NewIconContainer("Icon", (IUiElement) txt1).SetIcon(research.NodeDetailBlockedIcon).PutToLeftMiddleOf<IconContainer>((IUiElement) txt1, research.NodeBlockedIcon.Size, Offset.Left((float) (-(double) research.NodeBlockedIcon.Width - 5.0)));
      this.m_progress = this.Builder.NewTxt("ProgressInfo", (IUiElement) this.m_container).SetTextStyle(research.NodeDetailProgressText).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_container, new float?(20f));
      this.m_desc = this.Builder.NewTxt("Desc", (IUiElement) this.m_container).SetTextStyle(research.NodeDetailDescText).SetAlignment(TextAnchor.UpperLeft).AllowVerticalOverflow().AppendTo<Txt>(this.m_container, new float?(0.0f), Offset.LeftRight(this.Style.Panel.Padding));
      this.m_requiredProtos = new IconsWithTitlesView(this.Builder, this.m_container, this.AddTitle("RequiredProtosTitle", Tr.Requires, this.m_container), this.m_researchPopupController);
      this.m_scrollableContainer = this.Builder.NewScrollableContainer("ScrollableContainer", (IUiElement) this.m_container).AddVerticalScrollbar().AppendTo<ScrollableContainer>(this.m_container, new float?(0.0f));
      this.m_innerContainer = this.Builder.NewStackContainer("Container", (IUiElement) this.m_scrollableContainer.Viewport).SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(10f);
      this.m_scrollableContainer.AddItemTop((IUiElement) this.m_innerContainer);
      this.m_unlocks = new IconsWithTitlesView(this.Builder, this.m_innerContainer, this.AddTitle("UnlocksTitle", Tr.Unlocks, this.m_innerContainer), this.m_researchPopupController);
      Txt recipesTitle = this.AddTitle("RecipesTitle", Tr.Recipes__New, this.m_innerContainer);
      this.m_recipes = new RecipesView(this.Builder, this.m_recipesBookController, this.m_innerContainer, new Action<float>(onRecipesWidthChange));
      this.AddUpdater(this.m_recipes.CreateUpdater((Func<ResearchNode>) (() => this.m_node.Value)));
      this.AddUpdater(this.m_unlocks.CreateUpdater((Func<IUnlockingNode>) (() => (IUnlockingNode) this.m_node.Value)));
      this.AddUpdater(this.m_requiredProtos.CreateUpdaterForRequiredProtos((Func<ResearchNode>) (() => this.m_node.Value), this.m_unlockedProtosDbForUi));
      this.m_startBtn = this.Builder.NewBtnPrimaryBig("Start").SetText((LocStrFormatted) Tr.StartResearch_Action).OnClick(this.m_onStartClick);
      appendButton(this.m_startBtn);
      this.m_cancelBtn = this.Builder.NewBtnDangerBig("Cancel").SetText((LocStrFormatted) Tr.Cancel).OnClick(this.m_onCancelClick);
      appendButton(this.m_cancelBtn);
      this.m_addToQueue = this.Builder.NewBtnPrimaryBig("AddToQueue").SetText((LocStrFormatted) Tr.ResearchQueue__Add).UseSmallTextIfNeeded().OnClick(this.m_onEnqueueClick);
      appendButton(this.m_addToQueue);
      this.m_removeFromQueue = this.Builder.NewBtnGeneralBig("RemoveToQueue").SetText((LocStrFormatted) Tr.ResearchQueue__Remove).UseSmallTextIfNeeded().OnClick(this.m_onDequeueClick);
      appendButton(this.m_removeFromQueue);
      this.m_inQueueText = this.Builder.NewTxt("InQueue").SetText("").SetTextStyle(this.Style.Global.TextMedium.Extend(new ColorRgba?(this.Style.Global.GreenForDark))).SetAlignment(TextAnchor.MiddleCenter).AppendTo<Txt>(this.m_container, new float?(20f));
      this.m_lockedInfo = this.Builder.NewPanel("LockedInfo");
      float leftOffset1 = research.NodeDetailLockedIcon.Width + 4f;
      Txt txt2 = this.Builder.NewTxt("Text").SetText((LocStrFormatted) Tr.Locked).SetTextStyle(research.NodeDetailLockedText).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_lockedInfo, Offset.Left(leftOffset1));
      this.Builder.NewIconContainer("LockIcon").SetIcon(research.NodeDetailLockedIcon).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_lockedInfo, research.NodeDetailLockedIcon.Size);
      this.m_lockedInfo.AppendTo<Panel>(this.m_container, new Vector2?(new Vector2(txt2.GetPreferedWidth() + leftOffset1, 30f)), ContainerPosition.MiddleOrCenter);
      this.m_finishedInfo = this.Builder.NewPanel("FinishedInfo");
      float leftOffset2 = research.NodeDetailFinishedIcon.Width + 4f;
      Txt txt3 = this.Builder.NewTxt("Text").SetText("Finished").SetTextStyle(research.NodeDetailFinishedText).SetAlignment(TextAnchor.MiddleCenter).PutTo<Txt>((IUiElement) this.m_finishedInfo, Offset.Left(leftOffset2));
      this.Builder.NewIconContainer("TickIcon").SetIcon(research.NodeDetailFinishedIcon).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_finishedInfo, research.NodeDetailFinishedIcon.Size);
      this.m_finishedInfo.AppendTo<Panel>(this.m_container, new Vector2?(new Vector2(txt3.GetPreferedWidth() + leftOffset2, 30f)), ContainerPosition.MiddleOrCenter);
      this.Builder.NewPanel("Divider").SetBackground((ColorRgba) 3092271).AppendTo<Panel>(this.m_container, new float?(2f), Offset.Top(16f));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<ResearchNodeProto>((Func<ResearchNodeProto>) (() => this.m_node.Value.Proto)).Do((Action<ResearchNodeProto>) (proto =>
      {
        string upper = proto.Strings.Name.TranslatedString.ToUpper(LocalizationManager.CurrentCultureInfo);
        int difficulty = proto.Difficulty;
        if (difficulty > 0)
          upper += string.Format(" [{0}]", (object) difficulty);
        this.m_title.SetText(upper);
        this.m_desc.SetText((LocStrFormatted) proto.ResolvedDescription);
        this.m_container.UpdateItemHeight((IUiElement) this.m_desc, this.m_desc.GetPreferedHeight(this.GetWidth()));
      }));
      updaterBuilder.Observe<ResearchNodeState>((Func<ResearchNodeState>) (() => this.m_node.Value.State)).Observe<Percent>((Func<Percent>) (() => this.m_node.Value.ProgressInPerc)).Observe<bool>((Func<bool>) (() => this.m_controller.Value.NoLabAvailable)).Do(new Action<ResearchNodeState, Percent, bool>(this.updateState));
      updaterBuilder.Observe<ResearchNode.InfoForUi>((Func<ResearchNode.InfoForUi>) (() => this.m_node.Value.GetInfo())).Observe<bool>((Func<bool>) (() => this.m_node.Value.State == ResearchNodeState.InProgress && this.m_controller.Value.NoLabAvailable)).Do((Action<ResearchNode.InfoForUi, bool>) ((info, isBlocked) =>
      {
        this.m_titleHolder.SetBackground(this.m_nodeColorSelector.ResolveTitleBgColor(isBlocked, info));
        this.m_title.SetColor(this.m_nodeColorSelector.ResolveTitleTextColor(isBlocked, info));
        this.updateButtons(info);
      }));
      this.AddUpdater(updaterBuilder.Build());

      void onRecipesWidthChange(float width)
      {
        float num = (float) this.Builder.Style.Research.DefaultSidePanelWidth.Max(width.CeilToInt());
        Action<float> onWidthChanged = this.OnWidthChanged;
        if (onWidthChanged != null)
          onWidthChanged(num + 16f);
        recipesTitle.SetVisibility<Txt>(!width.IsNearZero());
      }

      void appendButton(Btn button)
      {
        Vector2 second = new Vector2(200f, 40f);
        button.AppendTo<Btn>(this.m_container, new Vector2?(button.GetOptimalSize().Min(second)), ContainerPosition.MiddleOrCenter, Offset.Top(10f));
      }
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      float num = this.Builder.MainCanvas.GetHeight() - 120f;
      float height1 = this.m_innerContainer.GetDynamicHeight();
      float height2 = this.m_container.GetDynamicHeight() - this.m_scrollableContainer.GetHeight() + height1;
      if ((double) height2 > (double) num)
      {
        this.m_innerContainer.SetInnerPadding(Offset.Right(16f));
        height1 = (height1 - (height2 - num)).Max(100f);
        height2 = num;
      }
      else
        this.m_innerContainer.SetInnerPadding(Offset.Zero);
      this.m_container.UpdateItemHeight((IUiElement) this.m_scrollableContainer, height1);
      this.SetHeight<ResearchSidePanel>(height2);
    }

    public Txt AddTitle(string id, LocStr title, StackContainer container)
    {
      return this.AddTitle(id, title.TranslatedString, container);
    }

    public Txt AddTitle(string id, string title, StackContainer container)
    {
      return this.Builder.NewTxt(id).SetText(title).SetTextStyle(this.Style.Research.NodeTitleText).SetAlignment(TextAnchor.MiddleLeft).AppendTo<Txt>(container, new float?((float) this.Style.Research.SectionTitleHeight), Offset.Left(this.Style.Panel.Padding), true);
    }

    public void SetNode(ResearchNode node)
    {
      this.m_node = (Option<ResearchNode>) node.CheckNotNull<ResearchNode>();
    }

    public void UpdateWidth(float width)
    {
      Action<float> onWidthChanged = this.OnWidthChanged;
      if (onWidthChanged == null)
        return;
      onWidthChanged(width + 16f);
    }

    private void updateState(ResearchNodeState nodeState, Percent progress, bool noLabAvailable)
    {
      bool isVisible1 = nodeState == ResearchNodeState.InProgress & noLabAvailable;
      this.m_container.StartBatchOperation();
      this.m_container.SetItemVisibility((IUiElement) this.m_blockedInfo, isVisible1);
      int num;
      switch (nodeState)
      {
        case ResearchNodeState.Researched:
          num = 0;
          break;
        case ResearchNodeState.InProgress:
          num = 1;
          break;
        default:
          num = progress > Percent.Zero ? 1 : 0;
          break;
      }
      bool isVisible2 = num != 0;
      this.m_container.SetItemVisibility((IUiElement) this.m_progress, isVisible2);
      if (isVisible2)
      {
        bool flag = nodeState == ResearchNodeState.InProgress && !isVisible1;
        this.m_progress.SetText((flag ? "IN PROGRESS - " : "RESEARCHED: ") + progress.ToStringRounded());
        this.m_progress.SetColor(flag ? this.Style.Research.NodeDetailProgressText.Color : ColorRgba.White);
      }
      this.m_container.FinishBatchOperation();
    }

    private void updateButtons(ResearchNode.InfoForUi info)
    {
      bool isVisible1 = info.IndexInQueue >= 0;
      this.m_container.StartBatchOperation();
      bool isVisible2 = !info.IsLocked && info.State == ResearchNodeState.NotResearched && !info.CanBeEnqueuedDirect && !isVisible1;
      this.m_container.SetItemVisibility((IUiElement) this.m_startBtn, isVisible2);
      this.m_container.SetItemVisibility((IUiElement) this.m_cancelBtn, info.State == ResearchNodeState.InProgress);
      this.m_container.SetItemVisibility((IUiElement) this.m_lockedInfo, info.IsLockedByCondition || info.IsLockedByParents && !info.CanBeEnqueued && !isVisible1);
      this.m_container.SetItemVisibility((IUiElement) this.m_inQueueText, isVisible1);
      this.m_inQueueText.SetText(Tr.ResearchQueue__Status.Format((info.IndexInQueue + 2).ToString()));
      this.m_container.SetItemVisibility((IUiElement) this.m_removeFromQueue, info.CanBeDequeued | isVisible1);
      this.m_removeFromQueue.SetEnabled(info.CanBeDequeued);
      this.m_container.SetItemVisibility((IUiElement) this.m_addToQueue, !isVisible2 && info.CanBeEnqueued);
      this.m_container.SetItemVisibility((IUiElement) this.m_finishedInfo, info.State == ResearchNodeState.Researched);
      this.m_container.FinishBatchOperation();
    }

    public float GetWidth() => this.RectTransform.rect.width;

    public float GetHeight() => this.m_container.GetDynamicHeight();
  }
}
