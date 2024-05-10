// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UnlockingTreeView.UnlockingTreeNodeViewBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.UnlockingTreeView
{
  public abstract class UnlockingTreeNodeViewBase<TTreeNode> : 
    IUnlockingTreeNodeView<TTreeNode>,
    IUiElement
    where TTreeNode : class, IUnlockingNode<TTreeNode>
  {
    protected readonly UiBuilder Builder;
    private readonly UnlockingTreeWindowUiStyle m_style;
    private readonly Btn m_btn;
    private readonly Txt m_title;
    private readonly Txt m_difficulty;
    private readonly Panel m_diffHolder;
    private readonly PanelWithShadow m_titleBar;
    private readonly StackContainer m_unlocksIconsContainer;
    private readonly IconContainer m_lockedIcon;
    private readonly IconContainer m_doneIcon;
    private readonly IconContainer m_inProgressIcon;
    private readonly IconContainer m_warningIcon;
    private readonly Panel m_selectionBg;
    private readonly Panel m_hoverBg;
    private readonly Panel m_highlightBg;
    private Panel m_progressBar;
    private readonly Panel m_queueIndexPanel;
    private readonly Txt m_queueText;
    private readonly IconContainer m_queuedIcon;

    public event Action<IUnlockingTreeNodeView<TTreeNode>> Click;

    public event Action<IUnlockingTreeNodeView<TTreeNode>> RightClick;

    public event Action<IUnlockingTreeNodeView<TTreeNode>> DoubleClick;

    public GameObject GameObject => this.m_btn.GameObject;

    public RectTransform RectTransform => this.m_btn.RectTransform;

    public TTreeNode Node { get; }

    protected bool IsSelected { get; private set; }

    protected bool IsHovered { get; private set; }

    public UnlockingTreeNodeViewBase(
      PlacementData placement,
      UiBuilder builder,
      UnlockingTreeWindowUiStyle style,
      TTreeNode node,
      LocStrFormatted name)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Builder = builder;
      this.m_style = style;
      this.Node = node.CheckNotNull<TTreeNode>();
      Sprite sharedSprite = builder.AssetsDb.GetSharedSprite(builder.Style.Icons.BorderGradient);
      Btn leftTopOf = builder.NewBtn(nameof (Node), placement.Parent).PutToLeftTopOf<Btn>(placement.Parent, placement.Size, placement.Offset);
      BtnStyle nodeMainStyle = this.m_style.NodeMainStyle;
      ref BtnStyle local1 = ref nodeMainStyle;
      ColorRgba? nullable1 = new ColorRgba?(new ColorRgba(16777215));
      TextStyle? text = new TextStyle?();
      BorderStyle? border = new BorderStyle?();
      ColorRgba? backgroundClr = new ColorRgba?();
      ColorRgba? normalMaskClr = new ColorRgba?();
      ColorRgba? hoveredClr = nullable1;
      ColorRgba? pressedClr = new ColorRgba?();
      ColorRgba? disabledMaskClr = new ColorRgba?();
      ColorRgba? foregroundClrWhenDisabled = new ColorRgba?();
      ColorRgba? backgroundClrWhenDisabled = new ColorRgba?();
      bool? shadow = new bool?();
      int? width = new int?();
      int? height = new int?();
      int? nullable2 = new int?();
      int? sidePaddings = nullable2;
      Offset? iconPadding = new Offset?();
      BtnStyle buttonStyle = local1.Extend(text, border, backgroundClr, normalMaskClr, hoveredClr, pressedClr, disabledMaskClr, foregroundClrWhenDisabled, backgroundClrWhenDisabled, shadow, width, height, sidePaddings, iconPadding);
      this.m_btn = leftTopOf.SetButtonStyle(buttonStyle).OnClick((Action) (() =>
      {
        Action<IUnlockingTreeNodeView<TTreeNode>> click = this.Click;
        if (click == null)
          return;
        click((IUnlockingTreeNodeView<TTreeNode>) this);
      })).OnRightClick((Action) (() =>
      {
        Action<IUnlockingTreeNodeView<TTreeNode>> rightClick = this.RightClick;
        if (rightClick == null)
          return;
        rightClick((IUnlockingTreeNodeView<TTreeNode>) this);
      })).OnDoubleClick((Action) (() =>
      {
        Action<IUnlockingTreeNodeView<TTreeNode>> doubleClick = this.DoubleClick;
        if (doubleClick == null)
          return;
        doubleClick((IUnlockingTreeNodeView<TTreeNode>) this);
      }));
      this.m_btn.SetOnMouseEnterLeaveActions(new Action(this.MouseEnter), new Action(this.MouseLeave));
      this.m_selectionBg = builder.NewPanel("SelectedBorder", (IUiElement) this.m_btn).PutTo<Panel>((IUiElement) this.m_btn, Offset.All((float) -this.m_style.NodeSelectionThickness)).SetBackground(sharedSprite, new ColorRgba?(this.m_style.NodeSelectedGlowClr)).Hide<Panel>();
      this.m_hoverBg = builder.NewPanel("HoveredBorder", (IUiElement) this.m_btn).SetBackground(sharedSprite, new ColorRgba?(this.m_style.NodeHoveredGlowClr)).PutTo<Panel>((IUiElement) this.m_btn, Offset.All((float) -this.m_style.NodeSelectionThickness)).Hide<Panel>();
      this.m_highlightBg = builder.NewPanel("HighlightedBorder", (IUiElement) this.m_btn).SetBackground(sharedSprite, new ColorRgba?(this.m_style.NodeHighlightedGlowClr)).PutTo<Panel>((IUiElement) this.m_btn, Offset.All((float) -this.m_style.NodeSelectionThickness)).Hide<Panel>();
      this.m_titleBar = builder.NewPanelWithShadow("TitleBar", (IUiElement) this.m_btn).PutToTopOf<PanelWithShadow>((IUiElement) this.m_btn, (float) this.m_style.NodeBarHeight).SetBackground(this.m_style.NodeBarBg).AddShadowBottom();
      this.m_progressBar = this.Builder.NewPanel("ProgressBar", (IUiElement) this.m_titleBar).PutToLeftOf<Panel>((IUiElement) this.m_titleBar, (float) this.m_style.NodeBarHeight).SetBackground(this.m_style.NodeProgressBarBg);
      this.m_title = builder.NewTxt("Text", (IUiElement) this.m_titleBar).PutTo<Txt>((IUiElement) this.m_titleBar, Offset.Left(32f) + Offset.Right(22f)).SetAlignment(TextAnchor.MiddleCenter).SetTextStyle(this.m_style.NodeTitleText).BestFitEnabled(15).IncreaseFontForSymbols().SetText(name);
      this.m_diffHolder = builder.NewPanel("Difficulty", (IUiElement) this.m_titleBar).PutToLeftOf<Panel>((IUiElement) this.m_titleBar, 26f, Offset.All(2f)).SetBackground(this.m_style.NodeBarBg);
      Txt txt1 = builder.NewTxt("", (IUiElement) this.m_diffHolder).PutTo<Txt>((IUiElement) this.m_diffHolder).SetAlignment(TextAnchor.MiddleCenter);
      TextStyle nodeTitleText = this.m_style.NodeTitleText;
      ref TextStyle local2 = ref nodeTitleText;
      nullable2 = new int?(14);
      ColorRgba? color1 = new ColorRgba?();
      FontStyle? fontStyle1 = new FontStyle?();
      int? fontSize1 = nullable2;
      bool? isCapitalized1 = new bool?();
      TextStyle textStyle1 = local2.Extend(color1, fontStyle1, fontSize1, isCapitalized1);
      this.m_difficulty = txt1.SetTextStyle(textStyle1);
      this.m_lockedIcon = builder.NewIconContainer("LockIcon", (IUiElement) this.m_titleBar).PutToRightMiddleOf<IconContainer>((IUiElement) this.m_titleBar, this.m_style.NodeLockedIcon.Size, this.m_style.NodeRightIconOffset).SetIcon(this.m_style.NodeLockedIcon);
      this.m_doneIcon = builder.NewIconContainer("TickIcon", (IUiElement) this.m_titleBar).PutToRightMiddleOf<IconContainer>((IUiElement) this.m_titleBar, this.m_style.NodeDoneIcon.Size, this.m_style.NodeRightIconOffset).SetIcon(this.m_style.NodeDoneIcon);
      this.m_inProgressIcon = builder.NewIconContainer("WorkingIcon", (IUiElement) this.m_titleBar).SetIcon(this.m_style.NodeInProgressIcon).PutToRightMiddleOf<IconContainer>((IUiElement) this.m_titleBar, this.m_style.NodeInProgressIcon.Size, this.m_style.NodeRightIconOffset);
      this.m_warningIcon = builder.NewIconContainer("BlockedIcon", (IUiElement) this.m_titleBar).SetIcon(this.m_style.NodeBlockedIcon).PutToRightMiddleOf<IconContainer>((IUiElement) this.m_titleBar, this.m_style.NodeBlockedIcon.Size, this.m_style.NodeRightIconOffset);
      this.m_queuedIcon = builder.NewIconContainer("QueuedIcon", (IUiElement) this.m_titleBar).SetIcon(this.m_style.NodeQueuedIcon).PutToRightMiddleOf<IconContainer>((IUiElement) this.m_titleBar, this.m_style.NodeInProgressIcon.Size, this.m_style.NodeRightIconOffset);
      this.m_unlocksIconsContainer = builder.NewStackContainer("Unlocks", (IUiElement) this.m_btn).SetSizeMode(StackContainer.SizeMode.StaticDirectionAligned).SetItemSpacing(6f).PutTo<StackContainer>((IUiElement) this.m_btn, Offset.TopBottom((float) builder.Style.Research.NodeUnlockBoxVerticalOffset) + Offset.LeftRight((float) builder.Style.Research.NodeUnlockBoxHorizontalOffset) + Offset.Top((float) builder.Style.Research.NodeBarHeight));
      this.m_queueIndexPanel = builder.NewPanel("QueueIndex", (IUiElement) this.m_btn).PutToRightBottomOf<Panel>((IUiElement) this.m_btn, new Vector2(28f, 30f), Offset.BottomRight(4f, 4f)).SetBackground(this.Builder.Style.Icons.WhiteBgGrayBorder, new ColorRgba?(this.Builder.Style.Research.NodeInQueueTitleBgClr));
      Txt txt2 = builder.NewTxt("Text", (IUiElement) this.m_queueIndexPanel).PutTo<Txt>((IUiElement) this.m_queueIndexPanel);
      TextStyle title = this.Builder.Style.Global.Title;
      ref TextStyle local3 = ref title;
      nullable2 = new int?(16);
      ColorRgba? color2 = new ColorRgba?();
      FontStyle? fontStyle2 = new FontStyle?();
      int? fontSize2 = nullable2;
      bool? isCapitalized2 = new bool?();
      TextStyle textStyle2 = local3.Extend(color2, fontStyle2, fontSize2, isCapitalized2);
      this.m_queueText = txt2.SetTextStyle(textStyle2).SetColor(this.Builder.Style.Research.NodeInQueueTitleClr).SetAlignment(TextAnchor.MiddleCenter);
    }

    protected void SetDifficulty(string difficulty, ColorRgba color)
    {
      this.m_difficulty.SetText(difficulty);
      this.m_difficulty.SetColor(color);
      this.m_diffHolder.SetVisibility<Panel>(difficulty.IsNotEmpty());
    }

    protected void SetQueueIndex(int queueIndex)
    {
      if (queueIndex >= 0)
        this.m_queueText.SetText(queueIndex.ToString());
      this.m_queueIndexPanel.SetVisibility<Panel>(queueIndex >= 0);
    }

    private void addUnlockIcon(string iconPath)
    {
      PanelWithShadow parent = this.Builder.NewPanelWithShadow("IconContainer", (IUiElement) this.m_unlocksIconsContainer).AddShadowRightBottom().SetBackground(this.m_style.NodeUnlockIconBg).AppendTo<PanelWithShadow>(this.m_unlocksIconsContainer, new float?((float) this.m_style.NodeUnlockBoxSize));
      this.Builder.NewIconContainer("Unit", (IUiElement) parent).SetIcon(iconPath).PutTo<IconContainer>((IUiElement) parent, Offset.All(2f));
    }

    protected void AddUnlocksIcons(
      ImmutableArray<KeyValuePair<Option<Proto>, string>> iconPaths)
    {
      int index1 = 0;
      for (int index2 = 0; index1 < iconPaths.Length && index2 < 4; ++index1)
      {
        KeyValuePair<Option<Proto>, string> iconPath = iconPaths[index1];
        Proto valueOrNull = iconPath.Key.ValueOrNull;
        // ISSUE: explicit non-virtual call
        if (((object) valueOrNull != null ? (__nonvirtual (valueOrNull.IsNotAvailable) ? 1 : 0) : 0) == 0)
        {
          if (string.IsNullOrEmpty(iconPath.Value))
          {
            Log.Warning(string.Format("Invalid icon path for research unlock '{0}'.", (object) iconPath.Key));
          }
          else
          {
            this.addUnlockIcon(iconPath.Value);
            ++index2;
          }
        }
      }
    }

    protected Panel AddProgressBarPanel() => this.m_progressBar;

    protected void SetTitleIcon(
      UnlockingTreeNodeViewBase<TTreeNode>.TitleIcon icon)
    {
      this.m_lockedIcon.SetVisibility<IconContainer>(icon == UnlockingTreeNodeViewBase<TTreeNode>.TitleIcon.Locked);
      this.m_doneIcon.SetVisibility<IconContainer>(icon == UnlockingTreeNodeViewBase<TTreeNode>.TitleIcon.Done);
      this.m_inProgressIcon.SetVisibility<IconContainer>(icon == UnlockingTreeNodeViewBase<TTreeNode>.TitleIcon.InProgress);
      this.m_warningIcon.SetVisibility<IconContainer>(icon == UnlockingTreeNodeViewBase<TTreeNode>.TitleIcon.Warning);
      this.m_queuedIcon.SetVisibility<IconContainer>(icon == UnlockingTreeNodeViewBase<TTreeNode>.TitleIcon.InQueue);
    }

    protected void SetTitleTextColor(ColorRgba color) => this.m_title.SetColor(color);

    protected void SetTitleBackgroundColor(ColorRgba color) => this.m_titleBar.SetBackground(color);

    public virtual void SetSelected(bool selected)
    {
      this.IsSelected = selected;
      this.m_hoverBg.Hide<Panel>();
      this.m_selectionBg.SetVisibility<Panel>(this.IsSelected);
    }

    public void SetHighlightedAsParent(bool highlighted)
    {
      this.IsSelected = highlighted;
      this.m_hoverBg.Hide<Panel>();
      this.m_highlightBg.SetVisibility<Panel>(this.IsSelected);
    }

    public void SetMarkAsSearchResult(bool isMarked)
    {
      this.m_btn.SetBackgroundColor(isMarked ? (ColorRgba) 11774720 : this.m_style.NodeMainStyle.BackgroundClr.Value);
    }

    protected virtual void MouseEnter()
    {
      if (!this.IsSelected)
        this.m_hoverBg.Show<Panel>();
      this.IsHovered = true;
    }

    protected virtual void MouseLeave()
    {
      if (!this.IsSelected)
        this.m_hoverBg.Hide<Panel>();
      this.IsHovered = false;
    }

    protected void SetBorderColor(ColorRgba color)
    {
    }

    public abstract void SyncUpdate();

    public abstract void RenderUpdate();

    public abstract bool Matches(string[] query);

    /// <summary>Type of icon show in the top right corner.</summary>
    protected enum TitleIcon
    {
      None,
      Locked,
      Done,
      InProgress,
      Warning,
      InQueue,
    }
  }
}
