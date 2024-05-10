// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Research.ResearchNodeView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Research;
using Mafi.Core.Syncers;
using Mafi.Core.UnlockingTree;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components.UnlockingTreeView;
using Mafi.Unity.UserInterface.Style;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Research
{
  internal class ResearchNodeView : UnlockingTreeNodeViewBase<ResearchNode>
  {
    private readonly IUiUpdater m_updater;

    public ResearchNodeView(
      PlacementData placement,
      ResearchController controller,
      ResearchManager researchManager,
      UiBuilder builder,
      ResearchNode node)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(placement, builder, (UnlockingTreeWindowUiStyle) builder.Style.Research, node, (LocStrFormatted) node.Proto.Strings.Name);
      ResearchNodeView researchNodeView = this;
      NodeColorsSelector nodeColorSelector = new NodeColorsSelector(builder.Style);
      Panel progressBar = this.AddProgressBarPanel();
      this.AddUnlocksIcons(node.Proto.Graphics.Icons);
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      // ISSUE: explicit non-virtual call
      updaterBuilder.Observe<ResearchNode.InfoForUi>((Func<ResearchNode.InfoForUi>) (() => __nonvirtual (researchNodeView.Node).GetInfo())).Observe<bool>((Func<bool>) (() => controller.NoLabAvailable)).Do((Action<ResearchNode.InfoForUi, bool>) ((info, noLabAvailable) =>
      {
        bool isBlocked = info.State == ResearchNodeState.InProgress & noLabAvailable;
        closure_0.SetTitleTextColor(nodeColorSelector.ResolveTitleTextColor(isBlocked, info));
        closure_0.SetTitleBackgroundColor(nodeColorSelector.ResolveTitleBgColor(isBlocked, info));
      }));
      // ISSUE: explicit non-virtual call
      // ISSUE: explicit non-virtual call
      updaterBuilder.Observe<ResearchNodeState>((Func<ResearchNodeState>) (() => __nonvirtual (researchNodeView.Node).State)).Observe<Percent>((Func<Percent>) (() => __nonvirtual (researchNodeView.Node).ProgressInPerc)).Do((Action<ResearchNodeState, Percent>) ((state, progress) =>
      {
        progressBar.SetVisibility<Panel>(state == ResearchNodeState.InProgress || progress.IsPositive && progress != Percent.Hundred);
        progressBar.SetWidth<Panel>((float) progress.Apply(closure_0.Builder.Style.Research.NodeSize.X));
      }));
      // ISSUE: explicit non-virtual call
      // ISSUE: explicit non-virtual call
      updaterBuilder.Observe<ResearchNode.InfoForUi>((Func<ResearchNode.InfoForUi>) (() => __nonvirtual (researchNodeView.Node).GetInfo())).Observe<bool>((Func<bool>) (() => __nonvirtual (researchNodeView.Node).State == ResearchNodeState.InProgress && controller.NoLabAvailable)).Do((Action<ResearchNode.InfoForUi, bool>) ((info, isBlocked) =>
      {
        if (isBlocked)
          researchNodeView.SetTitleIcon(UnlockingTreeNodeViewBase<ResearchNode>.TitleIcon.Warning);
        else if (info.IsInQueue)
          researchNodeView.SetTitleIcon(UnlockingTreeNodeViewBase<ResearchNode>.TitleIcon.InQueue);
        else if (info.CanBeEnqueued)
          researchNodeView.SetTitleIcon(UnlockingTreeNodeViewBase<ResearchNode>.TitleIcon.None);
        else if (info.IsLocked)
          researchNodeView.SetTitleIcon(UnlockingTreeNodeViewBase<ResearchNode>.TitleIcon.Locked);
        else if (info.State == ResearchNodeState.Researched)
          researchNodeView.SetTitleIcon(UnlockingTreeNodeViewBase<ResearchNode>.TitleIcon.Done);
        else
          researchNodeView.SetTitleIcon(info.State == ResearchNodeState.InProgress ? UnlockingTreeNodeViewBase<ResearchNode>.TitleIcon.InProgress : UnlockingTreeNodeViewBase<ResearchNode>.TitleIcon.None);
      }));
      // ISSUE: explicit non-virtual call
      // ISSUE: explicit non-virtual call
      updaterBuilder.Observe<ResearchNodeState>((Func<ResearchNodeState>) (() => __nonvirtual (researchNodeView.Node).State)).Observe<int>((Func<int>) (() => __nonvirtual (researchNodeView.Node).IndexInQueue)).Do((Action<ResearchNodeState, int>) ((state, indexInQueue) =>
      {
        if (state == ResearchNodeState.InProgress)
          researchNodeView.SetQueueIndex(1);
        else if (indexInQueue >= 0)
          researchNodeView.SetQueueIndex(indexInQueue + 2);
        else
          researchNodeView.SetQueueIndex(-1);
      }));
      // ISSUE: explicit non-virtual call
      // ISSUE: explicit non-virtual call
      // ISSUE: explicit non-virtual call
      updaterBuilder.Observe<ResearchNodeState>((Func<ResearchNodeState>) (() => __nonvirtual (researchNodeView.Node).State)).Observe<int>((Func<int>) (() => researchManager.OptimalSteps)).Observe<int>((Func<int>) (() => __nonvirtual (researchNodeView.Node).Proto.TotalStepsRequired)).Observe<int>((Func<int>) (() => __nonvirtual (researchNodeView.Node).Proto.Difficulty)).Do((Action<ResearchNodeState, int, int, int>) ((state, optSteps, nodeStepsRequired, nodeDiff) =>
      {
        if (state == ResearchNodeState.Researched || nodeStepsRequired == 0)
        {
          researchNodeView.SetDifficulty("", ColorRgba.Empty);
        }
        else
        {
          ColorRgba color = optSteps != 0 ? (optSteps < nodeStepsRequired ? (optSteps.ScaledByRounded(150.Percent()) < nodeStepsRequired ? researchNodeView.Builder.Style.Global.DangerClr : researchNodeView.Builder.Style.Global.OrangeText) : researchNodeView.Builder.Style.Global.GreenForDark) : researchNodeView.Builder.Style.Global.DangerClr;
          researchNodeView.SetDifficulty(nodeDiff.ToString(), color);
        }
      }));
      this.m_updater = updaterBuilder.Build();
    }

    public override void SyncUpdate() => this.m_updater.SyncUpdate();

    public override void RenderUpdate() => this.m_updater.RenderUpdate();

    public override bool Matches(string[] query)
    {
      if (UiSearchUtils.Matches(this.Node.Proto.Strings.Name.TranslatedString, query))
        return true;
      foreach (IUnlockNodeUnit unit in this.Node.Units)
      {
        if (!unit.HideInUI && unit.MatchesSearchQuery(query))
          return true;
      }
      return false;
    }
  }
}
