// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Messages.Goals.GoalsController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Messages.Goals
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class GoalsController : IUnityUi
  {
    public const int TOP_OFFSET = 100;
    private IUiUpdater m_updater;
    private readonly GoalsManager m_goalsManager;
    private readonly IInputScheduler m_inputScheduler;
    private readonly MessagesCenterController m_messagesCenterController;
    private readonly ToolbarController m_toolbarController;
    private StackContainer m_itemsContainer;
    private ViewsCacheHomogeneous<GoalsListView> m_goalsViewsCache;
    private ScrollableContainer m_scrollableContainer;
    private bool m_isScrollingNeeded;
    private bool m_isCollapsed;
    private Btn m_collapseBtn;
    private UiBuilder m_builder;

    public event Action OnHeightChanged;

    public bool ShowDoneGoals { get; private set; }

    public bool ShowLockedGoals { get; private set; }

    public GoalsController(
      GoalsManager goalsManager,
      IGameLoopEvents gameLoop,
      IInputScheduler inputScheduler,
      MessagesCenterController messagesCenterController,
      ToolbarController toolbarController)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: reference to a compiler-generated field
      this.\u003CShowDoneGoals\u003Ek__BackingField = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_goalsManager = goalsManager;
      this.m_inputScheduler = inputScheduler;
      this.m_messagesCenterController = messagesCenterController;
      this.m_toolbarController = toolbarController;
      gameLoop.SyncUpdate.AddNonSaveable<GoalsController>(this, new Action<GameTime>(this.syncUpdate));
      gameLoop.RenderUpdate.AddNonSaveable<GoalsController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void RegisterUi(UiBuilder builder)
    {
      this.m_builder = builder;
      this.m_collapseBtn = builder.NewBtn("Collapse").SetButtonStyle(builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/CollapseAll.svg").OnClick(new Action(this.onCollapseToggle)).PutToLeftTopOf<Btn>((IUiElement) builder.MainCanvas, 16.Vector2(), Offset.TopLeft(100f, 20f));
      this.m_scrollableContainer = builder.NewScrollableContainer("ScrollableContainer").AddVerticalScrollbarLeftMinimal().PutToLeftTopOf<ScrollableContainer>((IUiElement) builder.MainCanvas, new Vector2(20f, 0.0f), Offset.TopLeft(122f, 20f));
      this.m_scrollableContainer.SendToBack<ScrollableContainer>();
      IconContainer topGradient = builder.NewIconContainer("TopGradient").SetIcon("Assets/Unity/UserInterface/General/ShadowBottom32.png", ColorRgba.Black, false).DisableRaycast().PutToTopOf<IconContainer>((IUiElement) this.m_scrollableContainer, 15f).Hide<IconContainer>();
      IconContainer bottomGradient = builder.NewIconContainer("BottomGradient").SetIcon("Assets/Unity/UserInterface/General/ShadowTop32.png", ColorRgba.Black, false).DisableRaycast().PutToBottomOf<IconContainer>((IUiElement) this.m_scrollableContainer, 15f).Hide<IconContainer>();
      Panel scrollBg = builder.NewPanel("ScrollBg").SetBackground(ColorRgba.Black.SetA((byte) 125)).SetBorderStyle(new BorderStyle(ColorRgba.Black)).PutTo<Panel>((IUiElement) this.m_scrollableContainer, Offset.Left(-16f)).SendToBack<Panel>();
      this.m_itemsContainer = builder.NewStackContainer("Goals").SetStackingDirection(StackContainer.Direction.TopToBottom).SetSizeMode(StackContainer.SizeMode.Dynamic).SetItemSpacing(8f);
      this.m_scrollableContainer.AddItemTop((IUiElement) this.m_itemsContainer);
      this.m_goalsViewsCache = new ViewsCacheHomogeneous<GoalsListView>((Func<GoalsListView>) (() => new GoalsListView(this.m_inputScheduler, builder, this.m_messagesCenterController, this)));
      this.m_itemsContainer.SizeChanged += (Action<IUiElement>) (_ =>
      {
        float height = (float) ((double) builder.MainCanvas.GetHeight() - 100.0 - 195.0 - (double) this.m_toolbarController.GetToolbarHeight() - 60.0);
        float dynamicHeight = this.m_itemsContainer.GetDynamicHeight();
        if ((double) dynamicHeight > (double) height + 30.0)
        {
          this.m_isScrollingNeeded = true;
          this.m_scrollableContainer.SetHeight<ScrollableContainer>(height);
        }
        else
        {
          this.m_isScrollingNeeded = false;
          this.m_scrollableContainer.SetHeight<ScrollableContainer>(dynamicHeight);
        }
        updateGradientVisibility();
        scrollBg.SetVisibility<Panel>(this.m_isScrollingNeeded);
        Action onHeightChanged = this.OnHeightChanged;
        if (onHeightChanged == null)
          return;
        onHeightChanged();
      });
      this.m_scrollableContainer.SetOnScrollAction((Action) (() =>
      {
        updateGradientVisibility();
        this.hideMenus();
      }));
      this.m_scrollableContainer.SetOnDragAction(new Action(updateGradientVisibility));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      updaterBuilder.Observe<GoalsList>((Func<IIndexable<GoalsList>>) (() => this.m_goalsManager.CompletedGoals), (ICollectionComparator<GoalsList, IIndexable<GoalsList>>) CompareFixedOrder<GoalsList>.Instance).Observe<GoalsList>((Func<IIndexable<GoalsList>>) (() => this.m_goalsManager.ActiveGoals), (ICollectionComparator<GoalsList, IIndexable<GoalsList>>) CompareFixedOrder<GoalsList>.Instance).Do((Action<Lyst<GoalsList>, Lyst<GoalsList>>) ((completed, active) =>
      {
        this.hideMenus();
        this.m_goalsViewsCache.ReturnAll();
        this.m_itemsContainer.StartBatchOperation();
        this.m_itemsContainer.ClearAll();
        float num = 0.0f;
        foreach (GoalsList goalList in completed)
        {
          GoalsListView view = this.m_goalsViewsCache.GetView();
          view.SetGoals(goalList);
          if (this.m_isCollapsed)
            view.SetMinimized(true);
          this.m_itemsContainer.Append((IUiElement) view, new Vector2?(), new ContainerPosition?(ContainerPosition.LeftOrTop));
          num = num.Max(view.GetWidth());
        }
        foreach (GoalsList goalList in active)
        {
          GoalsListView view = this.m_goalsViewsCache.GetView();
          view.SetGoals(goalList);
          if (this.m_isCollapsed)
            view.SetMinimized(true);
          this.m_itemsContainer.Append((IUiElement) view, new Vector2?(), new ContainerPosition?(ContainerPosition.LeftOrTop));
          num = num.Max(view.GetWidth());
        }
        this.m_scrollableContainer.SetWidth<ScrollableContainer>(num);
        this.m_itemsContainer.FinishBatchOperation();
        this.m_collapseBtn.SetVisibility<Btn>(this.m_goalsViewsCache.AllExistingOnes().IsNotEmpty<GoalsListView>());
      }));
      this.m_updater = updaterBuilder.Build();
      this.m_updater.AddChildUpdater(this.m_goalsViewsCache.Updater);

      void updateGradientVisibility()
      {
        if (!this.m_scrollableContainer.IsVisible())
          return;
        topGradient.SetVisibility<IconContainer>(this.m_isScrollingNeeded && (double) this.m_scrollableContainer.NormalizedPosition.y < 0.99900001287460327);
        bottomGradient.SetVisibility<IconContainer>(this.m_isScrollingNeeded && (double) this.m_scrollableContainer.NormalizedPosition.y > 1.0 / 1000.0);
      }
    }

    private void onCollapseToggle()
    {
      this.m_isCollapsed = !this.m_isCollapsed;
      this.m_collapseBtn.SetButtonStyle(this.m_isCollapsed ? this.m_builder.Style.Global.IconBtnOrange2 : this.m_builder.Style.Global.IconBtnWhite);
      foreach (GoalsListView allExistingOne in this.m_goalsViewsCache.AllExistingOnes())
        allExistingOne.SetMinimized(this.m_isCollapsed);
    }

    internal void ToggleGoalsVisibility(bool? showDoneGoals = null, bool? showLockedGoals = null)
    {
      bool? nullable = showDoneGoals;
      this.ShowDoneGoals = ((int) nullable ?? (this.ShowDoneGoals ? 1 : 0)) != 0;
      nullable = showLockedGoals;
      this.ShowLockedGoals = ((int) nullable ?? (this.ShowLockedGoals ? 1 : 0)) != 0;
      foreach (GoalsListView allExistingOne in this.m_goalsViewsCache.AllExistingOnes())
        allExistingOne.ToggleHiddenGoals();
    }

    private void hideMenus()
    {
      foreach (GoalsListView allExistingOne in this.m_goalsViewsCache.AllExistingOnes())
        allExistingOne.HideMenuIfCan();
    }

    internal void OnMenuOpen(GoalsListView view)
    {
      foreach (GoalsListView allExistingOne in this.m_goalsViewsCache.AllExistingOnes())
      {
        if (view != allExistingOne)
          allExistingOne.HideMenuIfCan();
      }
    }

    private void renderUpdate(GameTime time) => this.m_updater.RenderUpdate();

    private void syncUpdate(GameTime time) => this.m_updater.SyncUpdate();

    public float GetOccupiedHeight()
    {
      int num = this.m_collapseBtn.IsVisible() ? 22 : 0;
      return this.m_scrollableContainer.GetHeight() + 20f + (float) num;
    }

    internal void HackMaskOrder() => this.m_scrollableContainer.HackMakeLastMask();
  }
}
