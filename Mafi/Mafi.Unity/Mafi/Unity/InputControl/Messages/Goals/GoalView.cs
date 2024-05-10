// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Messages.Goals.GoalView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Messages.Goals;
using Mafi.Core.Syncers;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Messages.Goals
{
  public class GoalView : IUiElementWithUpdater, IUiElement
  {
    private static readonly int MAX_TEXT_WIDTH;
    private static readonly int CHECKBOX_SIZE;
    private static readonly int TUTORIAL_BTN_SIZE;
    private static readonly int TIP_ICON_SIZE;
    private readonly Panel m_container;
    private readonly Txt m_text;
    private readonly IconContainer m_checkMark;
    private readonly Btn m_tutorialBtn;
    private readonly IconContainer m_lockedIcon;
    private readonly Panel m_checkBg;
    private readonly IconContainer m_tipIcon;
    private readonly Tooltip m_tipIconTooltip;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public IUiUpdater Updater { get; }

    public bool IsLocked { get; private set; }

    public bool IsDone { get; private set; }

    private Goal Goal { get; set; }

    public GoalView(
      IUiElement parent,
      UiBuilder builder,
      MessagesCenterController messagesCenterController,
      Action<GoalView> onGoalStateChanged)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      GoalView goalView = this;
      this.m_container = builder.NewPanel("TaskView", parent);
      ColorRgba iconColor = (ColorRgba) 16777215;
      this.m_checkBg = builder.NewPanel("Background", (IUiElement) this).SetBackground(builder.Style.Icons.WhiteBorderAliased, new ColorRgba?(iconColor)).PutToLeftMiddleOf<Panel>((IUiElement) this, GoalView.CHECKBOX_SIZE.Vector2(), Offset.Left(5f));
      this.m_checkMark = builder.NewIconContainer("CheckMark").SetIcon("Assets/Unity/UserInterface/General/Checkmark.svg", iconColor).PutTo<IconContainer>((IUiElement) this.m_checkBg, Offset.All(4f));
      this.m_lockedIcon = builder.NewIconContainer("CheckMark").SetIcon("Assets/Unity/UserInterface/General/Locked128.png", iconColor).PutToLeftMiddleOf<IconContainer>((IUiElement) this, 16.Vector2(), Offset.Left(6f)).Hide<IconContainer>();
      this.m_text = builder.NewTxt("Text", (IUiElement) this).SetTextStyle(builder.Style.Global.TextMedium.Extend(new ColorRgba?(iconColor))).AddOutline().IncreaseFontForSymbols().EnableRichText().PutToLeftOf<Txt>((IUiElement) this, 0.0f, Offset.Left((float) (GoalView.CHECKBOX_SIZE + 10))).SetAlignment(TextAnchor.MiddleLeft);
      this.m_tutorialBtn = builder.NewBtn("OpenTutorial").SetButtonStyle(builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/Toolbar/Tutorials.svg").OnClick((Action) (() =>
      {
        if (!goalView.Goal.Prototype.Tutorial.HasValue)
          return;
        messagesCenterController.ShowMessage(goalView.Goal.Prototype.Tutorial.Value);
      })).AddToolTip(Tr.OpenTutorial).PutToLeftBottomOf<Btn>((IUiElement) this, 0.Vector2()).Hide<Btn>();
      this.m_tipIcon = builder.NewIconContainer("Tip").SetIcon("Assets/Unity/UserInterface/General/Tip.svg", iconColor).PutToLeftBottomOf<IconContainer>((IUiElement) this, 0.Vector2()).Hide<IconContainer>();
      this.m_tipIconTooltip = builder.AddTooltipFor<IconContainer>((IUiElementWithHover<IconContainer>) this.m_tipIcon);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<string>((Func<string>) (() => goalView.Goal.Title)).Observe<bool>((Func<bool>) (() => goalView.Goal.IsCompleted)).Observe<bool>((Func<bool>) (() => goalView.Goal.IsLocked)).Do((Action<string, bool, bool>) ((title, isCompleted, isLocked) =>
      {
        goalView.update(title, isCompleted, isLocked);
        onGoalStateChanged(goalView);
      }));
      this.Updater = updaterBuilder.Build();
    }

    public void SetGoal(Goal goal)
    {
      this.Goal = goal;
      this.m_tutorialBtn.SetVisibility<Btn>(goal.Prototype.Tutorial.HasValue);
      this.m_tipIcon.SetVisibility<IconContainer>(goal.Prototype.Tip.HasValue);
      if (goal.Prototype.Tip.HasValue)
        this.m_tipIconTooltip.SetText(goal.Prototype.Tip.Value);
      this.update(goal.Title, goal.IsCompleted, goal.IsLocked);
    }

    private void update(string title, bool isDone, bool isLocked)
    {
      this.IsLocked = isLocked;
      this.IsDone = isDone;
      this.m_lockedIcon.SetVisibility<IconContainer>(isLocked);
      this.m_checkBg.SetVisibility<Panel>(!isLocked);
      this.m_checkMark.SetVisibility<IconContainer>(isDone);
      this.m_text.SetColor((ColorRgba) (isDone ? 11250603 : 16777215));
      if (isDone)
        this.m_text.SetText(title.Replace("<bc>", "<b>").Replace("</bc>", "</b>"));
      else
        this.m_text.SetText(title.Replace("<bc>", "<color=#f0a926><b>").Replace("</bc>", "</b></color>"));
    }

    public float GetFinalHeight(float widthAvailable)
    {
      float width = (this.m_text.GetPreferedWidth() + 16f).Min(widthAvailable - this.extraWidthNeeded());
      this.m_text.SetWidth<Txt>(width);
      if (this.m_tutorialBtn.IsVisible())
        this.m_tutorialBtn.PutToLeftMiddleOf<Btn>((IUiElement) this, GoalView.TUTORIAL_BTN_SIZE.Vector2(), Offset.Left((float) ((double) (GoalView.CHECKBOX_SIZE + 10) + (double) this.m_text.GetWidth() + 5.0)));
      if (this.m_tipIcon.IsVisible())
        this.m_tipIcon.PutToLeftMiddleOf<IconContainer>((IUiElement) this, GoalView.TIP_ICON_SIZE.Vector2(), Offset.Left((float) ((double) (GoalView.CHECKBOX_SIZE + 10) + (double) this.m_text.GetWidth() + 5.0)));
      return this.m_text.GetPreferedHeight(width).Max(20f);
    }

    public float GetWidthRequired() => (float) GoalView.MAX_TEXT_WIDTH + this.extraWidthNeeded();

    private float extraWidthNeeded()
    {
      float num = (float) (GoalView.CHECKBOX_SIZE + 15);
      if (this.m_tutorialBtn.IsVisible())
        num += (float) (GoalView.TUTORIAL_BTN_SIZE + 5);
      if (this.m_tipIcon.IsVisible())
        num += (float) (GoalView.TIP_ICON_SIZE + 5);
      return num;
    }

    static GoalView()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      GoalView.MAX_TEXT_WIDTH = 360;
      GoalView.CHECKBOX_SIZE = 18;
      GoalView.TUTORIAL_BTN_SIZE = 20;
      GoalView.TIP_ICON_SIZE = 18;
    }
  }
}
