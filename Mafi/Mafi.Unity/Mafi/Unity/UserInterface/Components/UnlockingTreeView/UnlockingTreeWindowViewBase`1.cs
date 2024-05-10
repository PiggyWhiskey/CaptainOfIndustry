// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.UnlockingTreeView.UnlockingTreeWindowViewBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Style;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components.UnlockingTreeView
{
  public abstract class UnlockingTreeWindowViewBase<TTreeNode> : WindowView where TTreeNode : class, IUnlockingNode<TTreeNode>
  {
    private IUnlockingTreeEditor<TTreeNode> m_editor;
    private Option<IUiElement> m_sidePanel;
    private readonly Lyst<IUnlockingTreeNodeView<TTreeNode>> m_nodeViews;
    private readonly Dict<TTreeNode, IUnlockingTreeNodeView<TTreeNode>> m_nodeViewsMap;
    private readonly Set<IUnlockingTreeNodeView<TTreeNode>> m_markedNodes;
    private Panel m_container;
    private ScrollableContainer m_scrollableContainer;
    private Panel m_nodesContainer;
    private Panel m_sideViewContainer;
    private Panel m_linesContainer;
    private UnlockingTreeWindowViewBase<TTreeNode>.SearchView m_searchView;
    private ConnectorsRenderer<TTreeNode> m_connectorsRenderer;
    private IconContainer m_hiTopLeft;
    private IconContainer m_hiTopRight;
    private IconContainer m_hiBottomLeft;
    private IconContainer m_hiBottomRight;
    private float m_targetScale;
    private float m_zoomVelocity;
    private Vector2 m_nodesContainerZoomPoint;
    private readonly float m_minZoom;
    private readonly float m_maxZoom;
    private readonly Func<UiStyle, UnlockingTreeWindowUiStyle> m_styleGetter;
    private readonly ShortcutsManager m_shortcutsManager;
    protected readonly string GenCodeUpdatePath;

    protected IIndexable<IUnlockingTreeNodeView<TTreeNode>> NodesViews
    {
      get => (IIndexable<IUnlockingTreeNodeView<TTreeNode>>) this.m_nodeViews;
    }

    protected Option<IUnlockingTreeNodeView<TTreeNode>> SelectedNodeView { get; private set; }

    public UnlockingTreeWindowViewBase(
      string name,
      float minZoom,
      float maxZoom,
      Func<UiStyle, UnlockingTreeWindowUiStyle> styleGetter,
      ShortcutsManager shortcutsManager,
      string genCodeUpdatePath = "")
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_nodeViews = new Lyst<IUnlockingTreeNodeView<TTreeNode>>();
      this.m_nodeViewsMap = new Dict<TTreeNode, IUnlockingTreeNodeView<TTreeNode>>();
      this.m_markedNodes = new Set<IUnlockingTreeNodeView<TTreeNode>>();
      this.m_targetScale = 0.7f;
      // ISSUE: explicit constructor call
      base.\u002Ector(name, WindowView.FooterStyle.None, true);
      this.m_minZoom = minZoom.CheckWithinIncl(0.0f, maxZoom);
      this.m_maxZoom = maxZoom.CheckWithinIncl(0.0f, 2f);
      this.m_styleGetter = styleGetter.CheckNotNull<Func<UiStyle, UnlockingTreeWindowUiStyle>>();
      this.m_shortcutsManager = shortcutsManager;
      this.GenCodeUpdatePath = genCodeUpdatePath;
      this.ShowAfterSync = true;
    }

    protected abstract ImmutableArray<TTreeNode> GetAllNodes();

    protected abstract IUnlockingTreeNodeView<TTreeNode> CreateNodeView(
      PlacementData placement,
      TTreeNode node);

    protected override void BuildWindowContent()
    {
      this.m_editor = (IUnlockingTreeEditor<TTreeNode>) new NoOpUnlockingTreeEditor<TTreeNode>();
      UnlockingTreeWindowUiStyle treeWindowUiStyle = this.m_styleGetter(this.Builder.Style);
      this.PositionSelfToFullscreen();
      this.m_container = this.Builder.NewPanel("Container", (IUiElement) this.GetContentPanel()).PutTo<Panel>((IUiElement) this.GetContentPanel()).SetBackground(treeWindowUiStyle.WindowBg).OnRightClick((Action) (() =>
      {
        this.UnSelectNode();
        this.HideSidePanel();
      }));
      this.m_scrollableContainer = this.Builder.NewScrollableContainer("ScrollableContainer", (IUiElement) this.m_container).DisableScrollByMouseWheel().SetDecelerationRate(0.0f).PutTo<ScrollableContainer>((IUiElement) this.m_container, Offset.Right((float) treeWindowUiStyle.DefaultSidePanelWidth));
      this.m_nodesContainer = this.Builder.NewPanel("NodesContainer", (IUiElement) this.m_scrollableContainer.Viewport);
      this.m_scrollableContainer.AddItem((IUiElement) this.m_nodesContainer, VerticalPosition.Middle);
      this.m_linesContainer = this.Builder.NewPanel("LinesContainer", (IUiElement) this.m_nodesContainer).PutTo<Panel>((IUiElement) this.m_nodesContainer);
      this.m_connectorsRenderer = new ConnectorsRenderer<TTreeNode>(this.Builder, this.m_linesContainer, new Func<TTreeNode, Vector2i>(this.getNodePos), new Func<TTreeNode, IEnumerable<TTreeNode>>(this.m_editor.GetChildrenOf));
      this.buildTree();
      this.m_sideViewContainer = this.Builder.NewPanel("Side", (IUiElement) this.m_container).PutToRightOf<Panel>((IUiElement) this.m_container, (float) treeWindowUiStyle.DefaultSidePanelWidth).SetBackground(treeWindowUiStyle.WindowBg);
      this.Builder.NewIconContainer("LeftGradient", (IUiElement) this.m_sideViewContainer).SetIcon(new IconStyle(this.Style.Icons.GradientToLeft, new ColorRgba?((ColorRgba) 3355443), preserveAspect: false)).PutToLeftOf<IconContainer>((IUiElement) this.m_sideViewContainer, 10f, Offset.Left(-9f));
      this.Builder.NewPanel("LeftBorder").SetBackground(treeWindowUiStyle.SidePanelLeftDividerClr).PutToLeftOf<Panel>((IUiElement) this.m_sideViewContainer, 1f, Offset.Left(-1f));
      this.m_editor.BuildUi(this.Builder, (IUiElement) this);
      this.m_nodesContainer.RectTransform.localScale = new Vector3(this.m_targetScale, this.m_targetScale, 1f);
      ColorRgba iconColor = new ColorRgba(13813823, 160);
      this.m_hiTopLeft = this.Builder.NewIconContainer("TopLeft").SetIcon("Assets/Unity/UserInterface/General/ArrowDiagonal.svg", iconColor).PutTo<IconContainer>((IUiElement) this).Hide<IconContainer>();
      this.m_hiTopRight = this.Builder.NewIconContainer("TopRight").SetIcon("Assets/Unity/UserInterface/General/ArrowDiagonal.svg", iconColor).SetScale<IconContainer>(new Vector2(-1f, 1f)).PutTo<IconContainer>((IUiElement) this).Hide<IconContainer>();
      this.m_hiBottomLeft = this.Builder.NewIconContainer("BottomLeft").SetIcon("Assets/Unity/UserInterface/General/ArrowDiagonal.svg", iconColor).SetScale<IconContainer>(new Vector2(1f, -1f)).PutTo<IconContainer>((IUiElement) this).Hide<IconContainer>();
      this.m_hiBottomRight = this.Builder.NewIconContainer("BottomRight").SetIcon("Assets/Unity/UserInterface/General/ArrowDiagonal.svg", iconColor).SetScale<IconContainer>(new Vector2(-1f, -1f)).PutTo<IconContainer>((IUiElement) this).Hide<IconContainer>();
      this.m_searchView = new UnlockingTreeWindowViewBase<TTreeNode>.SearchView(this.Builder, this);
      this.m_searchView.PutToLeftTopOf<UnlockingTreeWindowViewBase<TTreeNode>.SearchView>((IUiElement) this, new Vector2(220f, this.m_searchView.GetHeight()), Offset.TopLeft(100f, 40f));
      this.OnShowStart += (Action) (() => this.m_searchView.ClearSearch());
    }

    protected override Option<IUiElement> GetParent(UiBuilder builder)
    {
      return (Option<IUiElement>) (IUiElement) builder.MainCanvas;
    }

    protected void HideSidePanel()
    {
      this.SelectedNodeView = Option<IUnlockingTreeNodeView<TTreeNode>>.None;
      this.setSidePanelWidth(0.0f);
      if (!this.m_sidePanel.HasValue)
        return;
      this.m_sidePanel.Value.Hide<IUiElement>();
      this.m_sidePanel = (Option<IUiElement>) Option.None;
    }

    protected void ShowSidePanel(IUiElement sidePanel, float width, float height)
    {
      this.m_sidePanel = Option.Create<IUiElement>(sidePanel);
      this.m_sidePanel.Value.Show<IUiElement>();
      this.setSidePanelWidth(width);
      sidePanel.PutToMiddleOf<IUiElement>((IUiElement) this.m_sideViewContainer, height);
    }

    private void setSidePanelWidth(float width)
    {
      this.m_scrollableContainer.PutTo<ScrollableContainer>((IUiElement) this.m_container, Offset.Right(width + 1f));
      this.m_sideViewContainer.SetWidth<Panel>(width);
    }

    private void buildTree()
    {
      UnlockingTreeWindowUiStyle treeWindowUiStyle = this.m_styleGetter(this.Builder.Style);
      int self1 = 0;
      int self2 = 0;
      ImmutableArray<TTreeNode> allNodes = this.GetAllNodes();
      this.m_connectorsRenderer.BuildConnectors(allNodes);
      foreach (TTreeNode treeNode in allNodes)
      {
        Vector2i nodePos = this.getNodePos(treeNode);
        self1 = self1.Max(nodePos.X);
        self2 = self2.Max(nodePos.Y);
        IUnlockingTreeNodeView<TTreeNode> nodeView = this.CreateNodeView(new PlacementData((IUiElement) this.m_nodesContainer, new Vector2((float) treeWindowUiStyle.NodeSize.X, (float) treeWindowUiStyle.NodeSize.Y), Offset.TopLeft((float) nodePos.Y, (float) nodePos.X)), treeNode);
        this.m_nodeViews.Add(nodeView);
        this.m_nodeViewsMap.Add(treeNode, nodeView);
        nodeView.Click += new Action<IUnlockingTreeNodeView<TTreeNode>>(this.nodeClicked);
        nodeView.RightClick += new Action<IUnlockingTreeNodeView<TTreeNode>>(this.NodeRightClicked);
        nodeView.DoubleClick += new Action<IUnlockingTreeNodeView<TTreeNode>>(this.NodeDoubleClicked);
      }
      this.m_nodesContainer.SetSize<Panel>(new Vector2((float) (self1 + treeWindowUiStyle.NodeSize.X + treeWindowUiStyle.TreePadding.X), (float) (self2 + treeWindowUiStyle.NodeSize.Y + treeWindowUiStyle.TreePadding.Y)));
    }

    private Vector2i getNodePos(TTreeNode node)
    {
      Vector2i nodeGridPos = this.m_editor.GetNodeGridPos(node);
      UnlockingTreeWindowUiStyle treeWindowUiStyle = this.m_styleGetter(this.Builder.Style);
      return treeWindowUiStyle.TreePadding + new Vector2i(nodeGridPos.X * treeWindowUiStyle.NodeSize.X / 3, nodeGridPos.Y * treeWindowUiStyle.NodeSize.Y / 3);
    }

    private void unSelectNodeView()
    {
      if (!this.SelectedNodeView.HasValue)
        return;
      this.SelectedNodeView.Value.SetSelected(false);
      this.clearHighlightParents(this.SelectedNodeView.Value.Node);
      this.m_connectorsRenderer.ClearAllHighlights();
      this.SelectedNodeView = (Option<IUnlockingTreeNodeView<TTreeNode>>) Option.None;
      this.m_hiTopLeft.Hide<IconContainer>();
      this.m_hiTopRight.Hide<IconContainer>();
      this.m_hiBottomLeft.Hide<IconContainer>();
      this.m_hiBottomRight.Hide<IconContainer>();
    }

    protected virtual void UnSelectNode() => this.unSelectNodeView();

    protected virtual void clearHighlightParents(TTreeNode node)
    {
      foreach (TTreeNode parent in node.Parents)
        this.m_nodeViewsMap[parent].SetHighlightedAsParent(false);
    }

    protected virtual void highlightParents(TTreeNode node)
    {
      foreach (TTreeNode parent in node.Parents)
      {
        this.m_nodeViewsMap[parent].SetHighlightedAsParent(true);
        this.m_connectorsRenderer.Highlight(parent, node);
      }
    }

    protected virtual void SelectNode(IUnlockingTreeNodeView<TTreeNode> nodeView, bool scrollToNode = false)
    {
      this.unSelectNodeView();
      this.SelectedNodeView = Option.Some<IUnlockingTreeNodeView<TTreeNode>>(nodeView);
      nodeView.SetSelected(true);
      this.highlightParents(nodeView.Node);
      if (!scrollToNode)
        return;
      Vector2 size = 26.Vector2();
      int num1 = -36;
      int num2 = -10;
      this.m_hiTopLeft.PutToLeftTopOf<IconContainer>((IUiElement) nodeView, size, Offset.TopLeft((float) num1, (float) num1)).Show<IconContainer>();
      this.m_hiTopRight.PutToRightTopOf<IconContainer>((IUiElement) nodeView, size, Offset.TopRight((float) num1, (float) num2)).Show<IconContainer>();
      this.m_hiBottomLeft.PutToLeftBottomOf<IconContainer>((IUiElement) nodeView, size, Offset.BottomLeft((float) num2, (float) num1)).Show<IconContainer>();
      this.m_hiBottomRight.PutToRightBottomOf<IconContainer>((IUiElement) nodeView, size, Offset.BottomRight((float) num2, (float) num2)).Show<IconContainer>();
      this.m_nodesContainer.RectTransform.anchoredPosition = (Vector2) this.m_scrollableContainer.RectTransform.InverseTransformPoint(this.m_nodesContainer.RectTransform.position) - (Vector2) this.m_scrollableContainer.RectTransform.InverseTransformPoint(nodeView.RectTransform.position) + new Vector2(this.m_scrollableContainer.GetWidth() / 2f, 0.0f);
    }

    private void markNodesAsSearchResult(
      IIndexable<IUnlockingTreeNodeView<TTreeNode>> nodes)
    {
      this.unmarkAllNodes();
      foreach (IUnlockingTreeNodeView<TTreeNode> node in nodes)
      {
        this.m_markedNodes.Add(node);
        node.SetMarkAsSearchResult(true);
      }
    }

    private void unmarkAllNodes()
    {
      foreach (IUnlockingTreeNodeView<TTreeNode> markedNode in this.m_markedNodes)
        markedNode.SetMarkAsSearchResult(false);
      this.m_markedNodes.Clear();
    }

    protected void SelectNode(TTreeNode node)
    {
      IUnlockingTreeNodeView<TTreeNode> nodeView = this.m_nodeViews.FirstOrDefault<IUnlockingTreeNodeView<TTreeNode>>((Predicate<IUnlockingTreeNodeView<TTreeNode>>) (x => (object) x.Node == (object) node));
      if (nodeView == null)
        return;
      this.SelectNode(nodeView);
    }

    private void nodeClicked(IUnlockingTreeNodeView<TTreeNode> nodeView)
    {
      if (this.SelectedNodeView == nodeView || this.m_editor.NodeClicked(nodeView, this.SelectedNodeView))
        return;
      this.SelectNode(nodeView);
    }

    protected virtual void NodeDoubleClicked(IUnlockingTreeNodeView<TTreeNode> nodeView)
    {
    }

    protected virtual void NodeRightClicked(IUnlockingTreeNodeView<TTreeNode> nodeView)
    {
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      this.m_nodeViews.ForEach((Action<IUnlockingTreeNodeView<TTreeNode>>) (x => x.SyncUpdate()));
      base.SyncUpdate(gameTime);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      float x1 = this.m_nodesContainer.RectTransform.localScale.x;
      float num = Mathf.SmoothDamp(x1, this.m_targetScale, ref this.m_zoomVelocity, 0.1f);
      this.m_nodesContainer.RectTransform.localScale = new Vector3(num, num, 1f);
      Vector2 vector2_1 = this.m_nodesContainerZoomPoint * x1;
      Vector2 vector2_2 = this.m_nodesContainerZoomPoint * num - vector2_1;
      this.m_nodesContainer.RectTransform.localPosition = this.m_nodesContainer.RectTransform.localPosition - new Vector3(vector2_2.x, vector2_2.y, 0.0f);
      this.m_scrollableContainer.FixScroll();
      this.m_nodeViews.ForEach((Action<IUnlockingTreeNodeView<TTreeNode>>) (x => x.RenderUpdate()));
      base.RenderUpdate(gameTime);
    }

    public bool InputUpdate()
    {
      float num = 1f * Input.GetAxis("MouseScroll");
      if (!num.IsNearZero())
      {
        Vector3 mousePosition = Input.mousePosition;
        if (RectTransformUtility.RectangleContainsScreenPoint(this.m_scrollableContainer.RectTransform, (Vector2) mousePosition))
        {
          this.m_nodesContainerZoomPoint = (Vector2) this.m_nodesContainer.RectTransform.InverseTransformPoint(mousePosition);
          this.m_targetScale = (this.m_targetScale + num).Clamp(this.m_minZoom, this.m_maxZoom);
        }
      }
      if (this.m_searchView.InputUpdate())
        return true;
      this.m_editor.InputUpdate(this.SelectedNodeView);
      return this.m_scrollableContainer.UpdateKeyboardPan(this.m_shortcutsManager);
    }

    private void rebuildTree()
    {
      TTreeNode selectedNode = default (TTreeNode);
      if (this.SelectedNodeView.HasValue)
        selectedNode = this.SelectedNodeView.Value.Node;
      this.m_nodeViews.ForEachAndClear((Action<IUnlockingTreeNodeView<TTreeNode>>) (x => x.Destroy()));
      this.m_nodeViewsMap.Clear();
      this.m_connectorsRenderer.Clear();
      this.buildTree();
      if ((object) selectedNode == null)
        return;
      this.SelectedNodeView = Option.Create<IUnlockingTreeNodeView<TTreeNode>>(this.m_nodeViews.FirstOrDefault<IUnlockingTreeNodeView<TTreeNode>>((Predicate<IUnlockingTreeNodeView<TTreeNode>>) (x => (object) x.Node == (object) selectedNode)));
      this.SelectedNodeView.Value.SetSelected(true);
    }

    private class SearchView : IUiElement
    {
      private static readonly char[] SEARCH_QUERY_SEPARATOR;
      private readonly UnlockingTreeWindowViewBase<TTreeNode> m_parent;
      private readonly Lyst<IUnlockingTreeNodeView<TTreeNode>> m_nodesFound;
      private readonly TxtField m_searchBox;
      private readonly PanelWithShadow m_container;
      private readonly Btn m_previousBtn;
      private readonly Btn m_nextBtn;
      private readonly Txt m_matches;
      private readonly Txt m_nothingFound;
      private int m_currentIndex;
      private string m_previousQuery;

      public GameObject GameObject => this.m_container.GameObject;

      public RectTransform RectTransform => this.m_container.RectTransform;

      public SearchView(UiBuilder builder, UnlockingTreeWindowViewBase<TTreeNode> parent)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_nodesFound = new Lyst<IUnlockingTreeNodeView<TTreeNode>>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_parent = parent;
        UiStyle style = builder.Style;
        this.m_container = builder.NewPanelWithShadow(nameof (SearchView<>), (IUiElement) parent).AddShadowRightBottom().SetBackground(style.Global.ControlsBgColor);
        Panel parent1 = builder.NewPanel("Holder", (IUiElement) this.m_container).PutTo<Panel>((IUiElement) this.m_container, Offset.All(10f));
        IconContainer leftTopOf = builder.NewIconContainer("Icon", (IUiElement) parent1).SetIcon("Assets/Unity/UserInterface/General/Search.svg").PutToLeftTopOf<IconContainer>((IUiElement) parent1, new Vector2(30f, 30f));
        this.m_searchBox = builder.NewTxtField("Search", (IUiElement) parent1).SetStyle(style.Global.LightTxtFieldStyle).SetPlaceholderText(Tr.Search).SetCharLimit(30).PutToTopOf<TxtField>((IUiElement) parent1, 30f, Offset.Left(leftTopOf.GetWidth() + 10f));
        Vector2 size = new Vector2(36f, 25f);
        this.m_previousBtn = builder.NewBtnGeneral("Previous", (IUiElement) parent1).SetIcon("Assets/Unity/UserInterface/General/Return.svg").AddToolTip(string.Format("{0} ({1})", (object) Tr.NavigateTo__Previous, (object) Tr.NavigateTo__KeyHint.Format("Tab"))).OnClick(new Action(this.goToPrevious));
        this.m_previousBtn.PutToLeftBottomOf<Btn>((IUiElement) parent1, size);
        this.m_nextBtn = builder.NewBtnGeneral("Next", (IUiElement) parent1).SetIcon("Assets/Unity/UserInterface/General/Next.svg").AddToolTip(string.Format("{0} ({1})", (object) Tr.NavigateTo__Next, (object) Tr.NavigateTo__KeyHint.Format("Tab"))).OnClick(new Action(this.goToNext));
        this.m_nextBtn.PutToRightBottomOf<Btn>((IUiElement) parent1, size);
        this.m_matches = builder.NewTxt("matches", (IUiElement) parent1).SetTextStyle(style.Global.TextMediumBold).SetAlignment(TextAnchor.MiddleCenter).PutToBottomOf<Txt>((IUiElement) parent1, 25f, Offset.Left(size.x) + Offset.Right(size.x));
        this.m_nothingFound = builder.NewTxt("nothingFound", (IUiElement) parent1).SetText((LocStrFormatted) Tr.NothingFound).SetTextStyle(style.Global.TextMediumBold).SetAlignment(TextAnchor.MiddleCenter).PutToBottomOf<Txt>((IUiElement) parent1, 25f);
        this.m_searchBox.SetDelayedOnEditEndListener(new Action<string>(this.search));
        this.m_searchBox.SendToFront<TxtField>();
        this.clearPreviousResult();
      }

      private void search(string text)
      {
        if (!string.IsNullOrWhiteSpace(text) && text == this.m_previousQuery)
          return;
        this.clearPreviousResult();
        if (string.IsNullOrWhiteSpace(text))
          return;
        this.m_previousQuery = text;
        string[] query = text.Trim().ToLower(LocalizationManager.CurrentCultureInfo).Split(UnlockingTreeWindowViewBase<TTreeNode>.SearchView.SEARCH_QUERY_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
        foreach (IUnlockingTreeNodeView<TTreeNode> nodeView in this.m_parent.m_nodeViews)
        {
          if (nodeView.Matches(query))
            this.m_nodesFound.Add(nodeView);
        }
        this.m_nodesFound.Sort((Comparison<IUnlockingTreeNodeView<TTreeNode>>) ((n1, n2) => n1.RectTransform.anchoredPosition.x.CompareTo(n2.RectTransform.anchoredPosition.x)));
        this.SetHeight<UnlockingTreeWindowViewBase<TTreeNode>.SearchView>(90f);
        this.m_currentIndex = 0;
        this.updateButtons();
        if (this.m_nodesFound.IsNotEmpty)
        {
          this.m_parent.markNodesAsSearchResult((IIndexable<IUnlockingTreeNodeView<TTreeNode>>) this.m_nodesFound);
          this.selectNode(this.m_nodesFound.First);
        }
        else
          this.m_nothingFound.Show<Txt>();
      }

      internal bool InputUpdate()
      {
        if (this.m_parent.m_shortcutsManager.IsUp(this.m_parent.m_shortcutsManager.Search))
        {
          this.m_searchBox.Focus();
          return true;
        }
        if (!Input.GetKeyDown(KeyCode.Tab))
          return false;
        if (this.m_nodesFound.Count > 1)
          this.goToNext();
        return true;
      }

      private void selectNode(IUnlockingTreeNodeView<TTreeNode> nodeView)
      {
        this.m_parent.SelectNode(nodeView, true);
      }

      private void goToNext()
      {
        if (this.m_nodesFound.Count <= 1)
          return;
        this.m_currentIndex = (this.m_currentIndex + 1) % this.m_nodesFound.Count;
        this.selectNode(this.m_nodesFound[this.m_currentIndex]);
        this.updateButtons();
      }

      private void goToPrevious()
      {
        if (this.m_nodesFound.Count <= 1)
          return;
        if (this.m_currentIndex <= 0)
          this.m_currentIndex = this.m_nodesFound.Count - 1;
        else
          --this.m_currentIndex;
        this.selectNode(this.m_nodesFound[this.m_currentIndex]);
        this.updateButtons();
      }

      private void updateButtons()
      {
        this.m_previousBtn.SetVisibility<Btn>(this.m_nodesFound.Count > 1);
        this.m_nextBtn.SetVisibility<Btn>(this.m_nodesFound.Count > 1);
        if (this.m_nodesFound.IsNotEmpty)
          this.m_matches.SetText(string.Format("{0} / {1}", (object) (this.m_currentIndex + 1), (object) Tr.MatchesFound.Format(this.m_nodesFound.Count)));
        this.m_matches.SetVisibility<Txt>(this.m_nodesFound.IsNotEmpty);
      }

      public void ClearSearch()
      {
        this.m_searchBox.ClearInput();
        this.clearPreviousResult();
      }

      private void clearPreviousResult()
      {
        this.m_previousQuery = "";
        this.m_nodesFound.Clear();
        this.m_currentIndex = 0;
        this.updateButtons();
        this.m_nothingFound.Hide<Txt>();
        this.SetHeight<UnlockingTreeWindowViewBase<TTreeNode>.SearchView>(50f);
        this.m_parent.unmarkAllNodes();
      }

      static SearchView()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        UnlockingTreeWindowViewBase<TTreeNode>.SearchView.SEARCH_QUERY_SEPARATOR = new char[1]
        {
          ' '
        };
      }
    }
  }
}
