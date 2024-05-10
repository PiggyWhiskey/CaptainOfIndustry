// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Messages.Goals.GoalsListView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Input;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Syncers;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Messages.Goals
{
  public class GoalsListView : IUiElement, IDynamicSizeElement, IUiElementWithUpdater
  {
    public static readonly ColorRgba ICON_COLOR;
    private static readonly ColorRgba HOVER_COLOR;
    private static readonly ColorRgba COLOR;
    private static readonly int HEADER_HEIGHT;
    private int m_goalsTotal;
    private int m_goalsSatisfied;
    private readonly IInputScheduler m_inputScheduler;
    private readonly UiBuilder m_builder;
    private readonly GoalsController m_controller;
    private readonly StackContainer m_itemsContainer;
    private readonly StackContainer m_rewardsContainer;
    private readonly ViewsCacheHomogeneous<GoalView> m_tasksCache;
    private readonly Panel m_container;
    private readonly Txt m_titleView;
    private readonly IconContainer m_icon;
    private readonly Panel m_titleHolder;
    private readonly Panel m_containerBg;
    private readonly Panel m_longTermTaskIcon;
    private readonly Btn m_optionsBtn;
    private readonly Panel m_optionsMenu;
    private readonly Panel m_header;
    private readonly Panel m_progressBar;
    private readonly ProductQuantitiesView m_rewardsView;
    private readonly Btn m_skipTutorialBtn;

    public event Action<IUiElement> SizeChanged;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public GoalsList GoalList { get; private set; }

    public bool IsMinimized { get; private set; }

    public GoalsListView(
      IInputScheduler inputScheduler,
      UiBuilder builder,
      MessagesCenterController messagesCenterController,
      GoalsController controller)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      GoalsListView goalsListView = this;
      SkipGoalConfirmDialog confirmSkipDialog = new SkipGoalConfirmDialog(builder, new Action(this.OnConfirmClick));
      this.m_inputScheduler = inputScheduler;
      this.m_builder = builder;
      this.m_controller = controller;
      this.m_container = builder.NewPanel("TaskList");
      this.m_header = builder.NewPanel("TitleHolder").PutToTopOf<Panel>((IUiElement) this, (float) GoalsListView.HEADER_HEIGHT).SetBackground(new ColorRgba(0, 190));
      this.m_progressBar = builder.NewPanel("ProgressBar").PutToLeftOf<Panel>((IUiElement) this.m_header, 0.0f);
      this.m_titleHolder = builder.NewPanel("TitleHolder").SetBackground(ColorRgba.Empty).SetOnMouseEnterLeaveActions(new Action(this.onMouseEnter), new Action(this.onMouseLeave)).OnClick(new Action(this.toggleIsMinimized)).PutTo<Panel>((IUiElement) this.m_header, Offset.Right(25f));
      this.m_icon = builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/ArrowDown.svg", GoalsListView.COLOR).PutToLeftMiddleOf<IconContainer>((IUiElement) this.m_titleHolder, 14.Vector2(), Offset.Left(7f));
      Txt txt = builder.NewTxt("Title", (IUiElement) this);
      TextStyle textMediumBold = builder.Style.Global.TextMediumBold;
      ref TextStyle local = ref textMediumBold;
      ColorRgba? color = new ColorRgba?(GoalsListView.COLOR);
      bool? nullable1 = new bool?(true);
      FontStyle? fontStyle = new FontStyle?();
      int? fontSize = new int?();
      bool? isCapitalized = nullable1;
      TextStyle textStyle = local.Extend(color, fontStyle, fontSize, isCapitalized);
      this.m_titleView = txt.SetTextStyle(textStyle).AddOutline().BestFitEnabled(builder.Style.Global.TextMediumBold.FontSize).SetAlignment(TextAnchor.MiddleLeft);
      this.m_titleView.PutToLeftOf<Txt>((IUiElement) this.m_titleHolder, this.m_titleView.GetPreferedWidth(), Offset.Left(this.m_icon.GetWidth() + 14f));
      this.m_longTermTaskIcon = builder.NewPanel("LongTermIcon").SetBackground("Assets/Unity/UserInterface/General/SlowDown.svg", new ColorRgba?(GoalsListView.COLOR)).OnClick(new Action(this.toggleIsMinimized)).PutToRightMiddleOf<Panel>((IUiElement) this.m_titleHolder, 0.Vector2()).Hide<Panel>();
      builder.AddTooltipFor<Panel>((IUiElementWithHover<Panel>) this.m_longTermTaskIcon).SetText((LocStrFormatted) Tr.Goal_TakeTime);
      this.m_optionsBtn = builder.NewBtn("Options").SetButtonStyle(builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Working128.png").OnClick(new Action(this.toggleOptionsMenu)).PutToRightMiddleOf<Btn>((IUiElement) this.m_header, 18.Vector2(), Offset.Right(5f));
      this.m_optionsMenu = builder.NewPanel("OptionsMenu").SetBackground(new ColorRgba(0, 190)).OnClick(new Action(this.toggleIsMinimized)).Hide<Panel>();
      TextStyle? text = new TextStyle?(builder.Style.Global.Title.Extend(new ColorRgba?(ColorRgba.White)));
      ColorRgba? nullable2 = new ColorRgba?(ColorRgba.White);
      ColorRgba? nullable3 = new ColorRgba?(GoalsListView.HOVER_COLOR);
      BorderStyle? border = new BorderStyle?();
      ColorRgba? backgroundClr = new ColorRgba?();
      ColorRgba? normalMaskClr = nullable2;
      ColorRgba? hoveredMaskClr = nullable3;
      ColorRgba? pressedMaskClr = new ColorRgba?();
      ColorRgba? disabledMaskClr = new ColorRgba?();
      ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
      ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
      Offset? iconPadding = new Offset?();
      BtnStyle buttonStyle = new BtnStyle(text, border, backgroundClr, normalMaskClr, hoveredMaskClr, pressedMaskClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, iconPadding: iconPadding);
      this.m_skipTutorialBtn = builder.NewBtn("SkipThisBtn").SetButtonStyle(buttonStyle).SetText((LocStrFormatted) Tr.GoalSkip__Action).SetTextAlignment(TextAnchor.MiddleLeft).MakeTextTargetGraphics().OnClick((Action) (() =>
      {
        confirmSkipDialog.Show();
        closure_0.m_optionsMenu.Hide<Panel>();
      }));
      Btn toggleDoneGoalsBtn = builder.NewBtn("ToggleDoneGoals").SetButtonStyle(buttonStyle).SetText("[  ]" + Tr.GoalShowCompleted__Action.ToString()).SetTextAlignment(TextAnchor.MiddleLeft).MakeTextTargetGraphics().OnClick((Action) (() =>
      {
        goalsListView.m_controller.ToggleGoalsVisibility(new bool?(!goalsListView.m_controller.ShowDoneGoals));
        goalsListView.m_optionsMenu.Hide<Panel>();
      }));
      Btn toggleLockedGoalsBtn = builder.NewBtn("ToggleLockedGoals").SetButtonStyle(buttonStyle).SetText("[  ]" + Tr.GoalShowLocked__Action.ToString()).SetTextAlignment(TextAnchor.MiddleLeft).MakeTextTargetGraphics().OnClick((Action) (() =>
      {
        GoalsController controller1 = goalsListView.m_controller;
        bool? nullable4 = new bool?(!goalsListView.m_controller.ShowLockedGoals);
        bool? showDoneGoals = new bool?();
        bool? showLockedGoals = nullable4;
        controller1.ToggleGoalsVisibility(showDoneGoals, showLockedGoals);
        goalsListView.m_optionsMenu.Hide<Panel>();
      }));
      Btn objectToPlace = builder.NewBtn("CloseMenu").SetButtonStyle(buttonStyle).SetTextAlignment(TextAnchor.MiddleCenter).SetText("X").MakeTextTargetGraphics().OnClick((Action) (() => goalsListView.m_optionsMenu.Hide<Panel>()));
      objectToPlace.PutToRightTopOf<Btn>((IUiElement) this.m_optionsMenu, new Vector2(objectToPlace.GetOptimalWidth(), (float) GoalsListView.HEADER_HEIGHT));
      Offset offset = Offset.Left(5f) + Offset.Right(objectToPlace.GetOptimalWidth());
      this.m_skipTutorialBtn.PutToTopOf<Btn>((IUiElement) this.m_optionsMenu, (float) GoalsListView.HEADER_HEIGHT, offset);
      toggleDoneGoalsBtn.PutToTopOf<Btn>((IUiElement) this.m_optionsMenu, (float) GoalsListView.HEADER_HEIGHT, offset + Offset.Top(this.m_skipTutorialBtn.GetHeight()));
      toggleLockedGoalsBtn.PutToTopOf<Btn>((IUiElement) this.m_optionsMenu, (float) GoalsListView.HEADER_HEIGHT, offset + Offset.Top(this.m_skipTutorialBtn.GetHeight()) + Offset.Top(toggleDoneGoalsBtn.GetHeight()));
      this.m_optionsMenu.SetWidth<Panel>(this.m_skipTutorialBtn.GetOptimalWidth().Max(toggleDoneGoalsBtn.GetOptimalWidth()).Max(toggleLockedGoalsBtn.GetOptimalWidth()) + 10f + objectToPlace.GetOptimalWidth());
      this.m_containerBg = builder.NewPanel("Bg").SetBackground("Assets/Unity/UserInterface/General/GradientVertical.png", new ColorRgba?(new ColorRgba(0, 190))).PutTo<Panel>((IUiElement) this.m_container, Offset.Top(this.m_header.GetHeight() + 2f));
      this.m_itemsContainer = builder.NewStackContainer("Tasks").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(5f).SetInnerPadding(Offset.TopBottom(5f)).PutToTopOf<StackContainer>((IUiElement) this.m_containerBg, 0.0f);
      this.m_tasksCache = new ViewsCacheHomogeneous<GoalView>((Func<GoalView>) (() => new GoalView((IUiElement) goalsListView.m_itemsContainer, builder, messagesCenterController, new Action<GoalView>(goalsListView.onGoalStateChanged))));
      Btn element = builder.NewBtnPrimary("Collect").SetText((LocStrFormatted) Tr.Collect).OnClick((Action) (() => inputScheduler.ScheduleInputCmd<MarkGoalAsFinishedCmd>(new MarkGoalAsFinishedCmd(goalsListView.GoalList.Prototype.Id)))).SetIcon(new IconStyle("Assets/Unity/UserInterface/General/Trophy.svg", new ColorRgba?(ColorRgba.Black), new Vector2?(16.Vector2())));
      this.m_rewardsView = new ProductQuantitiesView((IUiElement) null, builder, cellSize: 60, useDynamicWidth: true);
      this.m_rewardsView.SetTransparentBackground();
      this.m_rewardsContainer = builder.NewStackContainer("Rewards").SetStackingDirection(StackContainer.Direction.LeftToRight).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(5f).PutToTopOf<StackContainer>((IUiElement) this.m_containerBg, 60f);
      this.m_rewardsContainer.Append((IUiElement) element, new float?(element.GetOptimalWidth()), new Offset(0.0f, 10f, 10f, 10f));
      this.m_rewardsContainer.Append((IUiElement) this.m_rewardsView, new float?(200f));
      this.m_itemsContainer.SizeChanged += (Action<IUiElement>) (c => goalsListView.updateHeight());
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<int>(new Func<int>(this.getGoalsSatisfied)).Do((Action<int>) (goalsSatisfied =>
      {
        goalsListView.m_goalsSatisfied = goalsSatisfied;
        goalsListView.updateView();
      }));
      updaterBuilder.Observe<bool>((Func<bool>) (() => goalsListView.m_controller.ShowDoneGoals)).Observe<bool>((Func<bool>) (() => goalsListView.m_controller.ShowLockedGoals)).Do((Action<bool, bool>) ((showDoneGoals, showLockedGoals) =>
      {
        toggleDoneGoalsBtn.SetText(string.Format("[{0}] {1}", showDoneGoals ? (object) "x" : (object) "  ", (object) Tr.GoalShowCompleted__Action));
        toggleLockedGoalsBtn.SetText(string.Format("[{0}] {1}", showLockedGoals ? (object) "x" : (object) "  ", (object) Tr.GoalShowLocked__Action));
      }));
      this.Updater = updaterBuilder.Build();
      this.Updater.AddChildUpdater(this.m_tasksCache.Updater);
    }

    internal void ToggleHiddenGoals()
    {
      this.m_itemsContainer.StartBatchOperation();
      foreach (GoalView allExistingOne in this.m_tasksCache.AllExistingOnes())
        this.m_itemsContainer.SetItemVisibility((IUiElement) allExistingOne, this.shouldShowGoal(allExistingOne));
      this.m_itemsContainer.FinishBatchOperation();
    }

    private bool shouldShowGoal(GoalView goal)
    {
      if (goal.IsDone && !this.m_controller.ShowDoneGoals)
        return false;
      return !goal.IsLocked || this.m_controller.ShowLockedGoals;
    }

    private void toggleOptionsMenu()
    {
      if (this.m_optionsMenu.IsVisible())
      {
        this.m_optionsMenu.Hide<Panel>();
      }
      else
      {
        this.m_optionsMenu.PutToRightTopOf<Panel>((IUiElement) this.m_header, new Vector2(this.m_optionsMenu.GetWidth(), (float) (3 * GoalsListView.HEADER_HEIGHT)), Offset.Right(-5f - this.m_optionsMenu.GetWidth()));
        this.m_optionsMenu.SetParent<Panel>((IUiElement) this.m_builder.MainCanvas);
        this.m_optionsMenu.Show<Panel>();
        this.m_controller.OnMenuOpen(this);
      }
    }

    public void HideMenuIfCan() => this.m_optionsMenu.Hide<Panel>();

    private void onGoalStateChanged(GoalView view)
    {
      this.m_itemsContainer.SetItemVisibility((IUiElement) view, this.shouldShowGoal(view));
    }

    private void OnConfirmClick()
    {
      this.m_inputScheduler.ScheduleInputCmd<MarkGoalAsFinishedCmd>(new MarkGoalAsFinishedCmd(this.GoalList.Prototype.Id, true));
    }

    private void onMouseEnter()
    {
      this.m_icon.SetColor(GoalsListView.HOVER_COLOR);
      this.m_titleView.SetColor(GoalsListView.HOVER_COLOR);
      this.m_longTermTaskIcon.SetBackground("Assets/Unity/UserInterface/General/SlowDown.svg", new ColorRgba?(GoalsListView.HOVER_COLOR));
    }

    private void onMouseLeave()
    {
      this.m_icon.SetColor(GoalsListView.COLOR);
      this.m_titleView.SetColor(GoalsListView.COLOR);
      this.m_longTermTaskIcon.SetBackground("Assets/Unity/UserInterface/General/SlowDown.svg", new ColorRgba?(GoalsListView.COLOR));
    }

    private int getGoalsSatisfied()
    {
      int goalsSatisfied = 0;
      foreach (Goal goal in this.GoalList.Goals)
        goalsSatisfied += goal.IsCompleted ? 1 : 0;
      return goalsSatisfied;
    }

    public void SetMinimized(bool minimized)
    {
      this.IsMinimized = minimized;
      this.updateView();
    }

    private void toggleIsMinimized()
    {
      this.IsMinimized = !this.IsMinimized;
      this.updateView();
    }

    private void updateView()
    {
      this.m_titleView.SetText(string.Format("{0}  {1} / {2}", (object) this.GoalList.Title, (object) this.m_goalsSatisfied, (object) this.m_goalsTotal));
      this.m_skipTutorialBtn.SetEnabled(!this.GoalList.IsCompleted);
      this.m_itemsContainer.SetVisibility<StackContainer>(!this.IsMinimized);
      this.m_rewardsContainer.SetVisibility<StackContainer>(this.GoalList.IsCompleted);
      this.m_itemsContainer.PutToTopOf<StackContainer>((IUiElement) this.m_containerBg, 0.0f, Offset.Top(this.GoalList.IsCompleted ? 60f : 0.0f));
      this.m_longTermTaskIcon.PutToLeftMiddleOf<Panel>((IUiElement) this.m_titleHolder, 18.Vector2(), Offset.Left((float) ((double) this.m_titleView.GetPreferedWidth() + (double) this.m_icon.GetWidth() + 20.0)));
      this.m_longTermTaskIcon.SetVisibility<Panel>(this.GoalList.Prototype.IsLongTermTask);
      this.m_optionsBtn.SetVisibility<Btn>(!this.IsMinimized);
      this.m_icon.SetScale<IconContainer>(new Vector2(1f, this.IsMinimized ? 1f : -1f));
      this.m_containerBg.SetVisibility<Panel>(this.GoalList.IsCompleted || !this.IsMinimized);
      this.updateHeight();
      if (this.m_goalsTotal <= 0)
        return;
      this.m_progressBar.SetWidth<Panel>(Percent.FromRatio(this.m_goalsSatisfied, this.m_goalsTotal).Apply(this.m_header.GetWidth()));
      this.m_progressBar.SetBackground(this.GoalList.IsCompleted ? new ColorRgba(13409792, 96) : new ColorRgba(5877784, 58));
    }

    private void updateHeight()
    {
      int num = this.GoalList.IsCompleted ? 60 : 0;
      if (this.IsMinimized)
        this.m_container.SetHeight<Panel>(this.m_header.GetHeight() + (float) num);
      else
        this.m_container.SetHeight<Panel>(this.m_header.GetHeight() + 2f + this.m_itemsContainer.GetDynamicHeight() + (float) num);
      Action<IUiElement> sizeChanged = this.SizeChanged;
      if (sizeChanged == null)
        return;
      sizeChanged((IUiElement) this);
    }

    public void SetGoals(GoalsList goalList)
    {
      if (this.GoalList == goalList)
        return;
      this.GoalList = goalList;
      this.m_goalsTotal = goalList.Goals.Count;
      this.m_goalsSatisfied = this.getGoalsSatisfied();
      this.IsMinimized = false;
      this.m_titleView.SetText(this.GoalList.Title);
      this.m_titleView.SetWidth<Txt>(this.m_titleView.GetPreferedWidth() + 50f);
      this.m_tasksCache.ReturnAll();
      this.m_itemsContainer.ClearAll();
      this.m_itemsContainer.StartBatchOperation();
      this.m_rewardsView.SetProducts(goalList.Prototype.Rewards);
      float num = (float) ((double) this.m_titleView.GetWidth() + (double) this.m_icon.GetWidth() + (double) this.m_optionsBtn.GetWidth() + 10.0);
      foreach (Goal goal in goalList.Goals)
      {
        GoalView view = this.m_tasksCache.GetView();
        view.SetGoal(goal);
        this.m_itemsContainer.Append((IUiElement) view, new float?(20f));
        num = num.Max(view.GetWidthRequired());
        this.m_itemsContainer.SetItemVisibility((IUiElement) view, this.m_controller.ShowDoneGoals || !view.IsDone);
      }
      foreach (GoalView allExistingOne in this.m_tasksCache.AllExistingOnes())
        this.m_itemsContainer.UpdateItemHeight((IUiElement) allExistingOne, allExistingOne.GetFinalHeight(num));
      this.m_itemsContainer.FinishBatchOperation();
      this.m_container.SetWidth<Panel>(num);
      this.updateView();
    }

    static GoalsListView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GoalsListView.ICON_COLOR = (ColorRgba) 12632256;
      GoalsListView.HOVER_COLOR = (ColorRgba) 15771942;
      GoalsListView.COLOR = (ColorRgba) 16777215;
      GoalsListView.HEADER_HEIGHT = 25;
    }
  }
}
