// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Research.ResearchWindowView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Input;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.InputControl.Research.SidePanel;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.UserInterface.Components.UnlockingTreeView;
using Mafi.Unity.UserInterface.Style;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Research
{
  internal class ResearchWindowView : UnlockingTreeWindowViewBase<ResearchNode>
  {
    private readonly LazyResolve<ResearchController> m_controller;
    private readonly ResearchManager m_manager;
    private readonly ResearchPopupController m_researchPopupController;
    private readonly IInputScheduler m_inputScheduler;
    private readonly ResearchSidePanel m_detailView;
    private Panel m_noResearchPanel;

    public ResearchWindowView(
      LazyResolve<ResearchController> controller,
      ResearchManager manager,
      UnlockedProtosDbForUi unlockedProtosDbForUi,
      RecipesBookController recipesBookController,
      ResearchPopupController researchPopupController,
      IInputScheduler inputScheduler,
      ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector("Research", 0.4f, 1f, (Func<UiStyle, UnlockingTreeWindowUiStyle>) (uiStyle => (UnlockingTreeWindowUiStyle) uiStyle.Research), shortcutsManager, "../Mafi.Base/Prototypes/Research/ResearchNodesPositionSetup.cs");
      ResearchWindowView researchWindowView = this;
      this.m_controller = controller;
      this.m_manager = manager;
      this.m_researchPopupController = researchPopupController;
      this.m_inputScheduler = inputScheduler;
      this.m_detailView = new ResearchSidePanel(controller, unlockedProtosDbForUi, recipesBookController, researchPopupController, (Action) (() => inputScheduler.ScheduleInputCmd<ResearchStartCmd>(new ResearchStartCmd(researchWindowView.SelectedNodeView.Value.Node.Proto))), (Action) (() => inputScheduler.ScheduleInputCmd<ResearchStopCmd>(new ResearchStopCmd())), (Action) (() => inputScheduler.ScheduleInputCmd<ResearchCheatFinishCmd>(new ResearchCheatFinishCmd())), (Action) (() => inputScheduler.ScheduleInputCmd<ResearchQueueDequeueCmd>(new ResearchQueueDequeueCmd(researchWindowView.SelectedNodeView.Value.Node.Proto, true))), (Action) (() => inputScheduler.ScheduleInputCmd<ResearchQueueDequeueCmd>(new ResearchQueueDequeueCmd(researchWindowView.SelectedNodeView.Value.Node.Proto, false))));
    }

    protected override void BuildWindowContent()
    {
      base.BuildWindowContent();
      this.m_researchPopupController.SetParentToUse(this.GetContentPanel().RectTransform);
      this.m_noResearchPanel = this.createNoResearchPanel();
      this.m_detailView.BuildUi(this.Builder);
      this.m_detailView.OnWidthChanged += (Action<float>) (width =>
      {
        if (!this.m_detailView.IsVisible)
          return;
        this.ShowSidePanel((IUiElement) this.m_detailView, width, this.m_detailView.GetHeight());
      });
      Btn btn = this.Builder.NewReturnBtn().OnClick((Action) (() =>
      {
        Action valueOrNull = this.OnCloseButtonClick.ValueOrNull;
        if (valueOrNull == null)
          return;
        valueOrNull();
      }));
      btn.PutToLeftBottomOf<Btn>((IUiElement) this, btn.GetSize(), Offset.BottomLeft(20f, 20f));
      this.OnShowStart += (Action) (() =>
      {
        this.UnSelectNode();
        Option<ResearchNode> currentResearch = this.m_manager.CurrentResearch;
        if (!currentResearch.HasValue)
          return;
        this.SelectNode(currentResearch.Value);
      });
    }

    public void FindResearchFor(IRecipeForUi recipe)
    {
      foreach (IUnlockingTreeNodeView<ResearchNode> nodesView in this.NodesViews)
      {
        foreach (IUnlockNodeUnit unit in nodesView.Node.Units)
        {
          if (unit is RecipeUnlock recipeUnlock && recipeUnlock.Proto == recipe)
          {
            this.SelectNode(nodesView, true);
            return;
          }
        }
      }
    }

    private Panel createNoResearchPanel()
    {
      ResearchWindowUiStyle research = this.Builder.Style.Research;
      Panel parent = this.Builder.NewPanel("NoResearch").SetHeight<Panel>(120f);
      this.Builder.NewIconContainer("Icon").SetIcon(research.NoResearchIcon).PutToCenterTopOf<IconContainer>((IUiElement) parent, research.NoResearchIcon.Size);
      this.Builder.NewTxt("Text").SetText("No research started!").SetTextStyle(research.NoResearchText).SetAlignment(TextAnchor.MiddleCenter).PutToBottomOf<Txt>((IUiElement) parent, 20f);
      return parent;
    }

    protected override ImmutableArray<ResearchNode> GetAllNodes()
    {
      return this.m_manager.AllNodes.Filter((Predicate<ResearchNode>) (x => !x.IsNotAvailable()));
    }

    protected override IUnlockingTreeNodeView<ResearchNode> CreateNodeView(
      PlacementData placement,
      ResearchNode node)
    {
      return (IUnlockingTreeNodeView<ResearchNode>) new ResearchNodeView(placement, this.m_controller.Value, this.m_manager, this.Builder, node);
    }

    protected override void UnSelectNode()
    {
      base.UnSelectNode();
      if (!this.m_manager.CurrentResearch.IsNone)
        return;
      this.m_detailView.Hide();
      this.m_noResearchPanel.Show<Panel>();
      this.ShowSidePanel((IUiElement) this.m_noResearchPanel, (float) this.Builder.Style.Research.DefaultSidePanelWidth, this.m_noResearchPanel.GetHeight());
    }

    protected override void clearHighlightParents(ResearchNode node)
    {
      base.clearHighlightParents(node);
      foreach (ResearchNode parent in node.Parents)
        this.clearHighlightParents(parent);
    }

    protected override void highlightParents(ResearchNode node)
    {
      if (node.State == ResearchNodeState.Researched)
        return;
      base.highlightParents(node);
      foreach (ResearchNode parent in node.Parents)
        this.highlightParents(parent);
    }

    protected override void SelectNode(
      IUnlockingTreeNodeView<ResearchNode> nodeView,
      bool scrollToNode = false)
    {
      base.SelectNode(nodeView, scrollToNode);
      this.m_detailView.SetNode(nodeView.Node);
      this.m_detailView.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.m_detailView.GetHeight());
      this.m_detailView.Show();
      this.ShowSidePanel((IUiElement) this.m_detailView, this.m_detailView.GetWidth(), this.m_detailView.GetHeight());
      this.m_noResearchPanel.Hide<Panel>();
    }

    protected override void NodeDoubleClicked(IUnlockingTreeNodeView<ResearchNode> nodeView)
    {
      if (nodeView.Node.State == ResearchNodeState.NotResearched && !nodeView.Node.IsLocked && !nodeView.Node.CanBeEnqueuedDirect && nodeView.Node.IndexInQueue < 0)
        this.m_inputScheduler.ScheduleInputCmd<ResearchStartCmd>(new ResearchStartCmd(nodeView.Node.Proto));
      else if (nodeView.Node.State == ResearchNodeState.InProgress)
        this.m_inputScheduler.ScheduleInputCmd<ResearchStopCmd>(new ResearchStopCmd());
      else if (nodeView.Node.CanBeEnqueued)
      {
        this.m_inputScheduler.ScheduleInputCmd<ResearchQueueDequeueCmd>(new ResearchQueueDequeueCmd(nodeView.Node.Proto, true));
      }
      else
      {
        if (!nodeView.Node.CanBeDequeued)
          return;
        this.m_inputScheduler.ScheduleInputCmd<ResearchQueueDequeueCmd>(new ResearchQueueDequeueCmd(nodeView.Node.Proto, false));
      }
    }

    protected override void NodeRightClicked(IUnlockingTreeNodeView<ResearchNode> nodeView)
    {
      if (nodeView.Node.State == ResearchNodeState.InProgress)
      {
        this.m_inputScheduler.ScheduleInputCmd<ResearchStopCmd>(new ResearchStopCmd());
      }
      else
      {
        if (!nodeView.Node.CanBeDequeued)
          return;
        this.m_inputScheduler.ScheduleInputCmd<ResearchQueueDequeueCmd>(new ResearchQueueDequeueCmd(nodeView.Node.Proto, false));
      }
    }

    public override void SyncUpdate(GameTime gameTime)
    {
      base.SyncUpdate(gameTime);
      if (!this.m_detailView.IsVisible)
        return;
      this.m_detailView.SyncUpdate(gameTime);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      if (!this.m_detailView.IsVisible)
        return;
      this.m_detailView.RenderUpdate(gameTime);
    }
  }
}
