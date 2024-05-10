// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Trees.TreeHarvestingDesignatorRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Terrain.Trees;
using Mafi.Unity.Terrain;
using Mafi.Unity.Utils;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Trees
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TreeHarvestingDesignatorRenderer
  {
    private static readonly ColorRgba HIGHLIGHT_COLOR;
    private readonly ITreesManager m_treeManager;
    private readonly ITreeRenderer m_treeRenderer;
    private readonly ActivatorState m_activator;
    private readonly Lyst<TreeId> m_treesToRemoveCache;
    private readonly DelayedItemsProcessing<TreeId> m_treesToHarvestObserver;
    private readonly Dict<TreeId, TreeHarvestingDesignatorRenderer.TreeHighlightState> m_treeHighlights;

    internal TreeHarvestingDesignatorRenderer(
      ITreesManager treeManager,
      ITreeRenderer treeRenderer,
      IGameLoopEvents gameLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_treesToRemoveCache = new Lyst<TreeId>();
      this.m_treeHighlights = new Dict<TreeId, TreeHarvestingDesignatorRenderer.TreeHighlightState>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treeManager = treeManager;
      this.m_treeRenderer = treeRenderer;
      this.m_activator = new ActivatorState(new Action(this.activate), new Action(this.deactivate));
      this.m_treesToHarvestObserver = new DelayedItemsProcessing<TreeId>(new Action<TreeId>(this.addHarvestedTree), new Action<TreeId>(this.removeHarvestedTree));
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      gameLoopEvents.SyncUpdate.AddNonSaveable<TreeHarvestingDesignatorRenderer>(this, new Action<GameTime>(this.sync));
    }

    private void initState()
    {
      foreach (TreeId enumerateSelectedTree in this.m_treeManager.EnumerateSelectedTrees())
        this.addHarvestedTree(enumerateSelectedTree);
      this.m_treeManager.TreeAddedToHarvest.AddNonSaveable<TreeHarvestingDesignatorRenderer>(this, new Action<TreeId>(this.m_treesToHarvestObserver.AddOnSim));
      this.m_treeManager.TreeRemovedFromHarvest.AddNonSaveable<TreeHarvestingDesignatorRenderer>(this, new Action<TreeId>(this.m_treesToHarvestObserver.RemoveOnSim));
    }

    private void activate()
    {
      foreach (KeyValuePair<TreeId, TreeHarvestingDesignatorRenderer.TreeHighlightState> treeHighlight in this.m_treeHighlights)
        this.updateTreeHighlight(treeHighlight.Value);
    }

    private void deactivate()
    {
      foreach (KeyValuePair<TreeId, TreeHarvestingDesignatorRenderer.TreeHighlightState> treeHighlight in this.m_treeHighlights)
      {
        TreeHarvestingDesignatorRenderer.TreeHighlightState treeHighlightState = treeHighlight.Value;
        treeHighlightState.RemoveHighlight(this.m_treeRenderer);
        if (!treeHighlightState.IsInHarvest && !treeHighlightState.IsInHarvestTemp)
          this.m_treesToRemoveCache.Add(treeHighlight.Key);
      }
      foreach (TreeId key in this.m_treesToRemoveCache)
        this.m_treeHighlights.Remove(key);
      this.m_treesToRemoveCache.Clear();
    }

    public IActivator CreateActivator() => this.m_activator.CreateActivator();

    public void TemporarilyAdd(TreeId tree)
    {
      TreeHarvestingDesignatorRenderer.TreeHighlightState highlightState = this.getHighlightState(tree);
      highlightState.IsInHarvestTemp = true;
      this.updateTreeHighlight(highlightState);
    }

    public void TemporarilyRemove(TreeId tree)
    {
      TreeHarvestingDesignatorRenderer.TreeHighlightState highlightState = this.getHighlightState(tree);
      highlightState.IsRemovedTemp = true;
      this.updateTreeHighlight(highlightState);
    }

    public void RemoveTemporaryEffects(TreeId tree)
    {
      TreeHarvestingDesignatorRenderer.TreeHighlightState highlightState = this.getHighlightState(tree);
      highlightState.RemoveTempEffects();
      this.updateTreeHighlight(highlightState);
    }

    public void RemoveTemporaryEffects()
    {
      foreach (KeyValuePair<TreeId, TreeHarvestingDesignatorRenderer.TreeHighlightState> treeHighlight in this.m_treeHighlights)
      {
        treeHighlight.Value.RemoveTempEffects();
        this.updateTreeHighlight(treeHighlight.Value);
      }
    }

    private void addHarvestedTree(TreeId tree)
    {
      TreeHarvestingDesignatorRenderer.TreeHighlightState highlightState = this.getHighlightState(tree);
      highlightState.IsInHarvest = true;
      this.updateTreeHighlight(highlightState);
    }

    private void removeHarvestedTree(TreeId tree)
    {
      TreeHarvestingDesignatorRenderer.TreeHighlightState treeHighlightState;
      if (!this.m_treeHighlights.TryGetValue(tree, out treeHighlightState))
        return;
      treeHighlightState.RemoveHighlight(this.m_treeRenderer);
      this.m_treeHighlights.Remove(tree);
    }

    private void sync(GameTime time)
    {
      if (!this.m_activator.IsActive)
        return;
      this.m_treesToHarvestObserver.SyncUpdate();
      this.m_treesToHarvestObserver.RenderUpdate();
    }

    private TreeHarvestingDesignatorRenderer.TreeHighlightState getHighlightState(TreeId tree)
    {
      TreeHarvestingDesignatorRenderer.TreeHighlightState highlightState1;
      if (this.m_treeHighlights.TryGetValue(tree, out highlightState1))
        return highlightState1;
      TreeHarvestingDesignatorRenderer.TreeHighlightState highlightState2 = new TreeHarvestingDesignatorRenderer.TreeHighlightState(tree);
      this.m_treeHighlights.Add(tree, highlightState2);
      return highlightState2;
    }

    private void updateTreeHighlight(
      TreeHarvestingDesignatorRenderer.TreeHighlightState highLightState)
    {
      if (!this.m_activator.IsActive)
        return;
      highLightState.UpdateHighlight(this.m_treeRenderer);
    }

    static TreeHarvestingDesignatorRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TreeHarvestingDesignatorRenderer.HIGHLIGHT_COLOR = ColorRgba.Yellow;
    }

    private class TreeHighlightState
    {
      private readonly TreeId m_tree;
      private Option<TreeMb> m_treeMb;
      private bool m_isHighlighted;
      private bool m_isInHarvestTemp;
      private bool m_isRemovedTemp;

      public TreeHighlightState(TreeId tree)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_tree = tree;
      }

      public bool IsInHarvest { get; set; }

      public bool IsInHarvestTemp
      {
        get => this.m_isInHarvestTemp;
        set
        {
          this.m_isInHarvestTemp = value;
          this.m_isRemovedTemp = false;
        }
      }

      public bool IsRemovedTemp
      {
        get => this.m_isRemovedTemp;
        set
        {
          this.m_isRemovedTemp = value;
          this.m_isInHarvestTemp = false;
        }
      }

      public void RemoveTempEffects()
      {
        this.m_isInHarvestTemp = false;
        this.m_isRemovedTemp = false;
      }

      public void RemoveHighlight(ITreeRenderer treeRenderer)
      {
        if (!this.m_isHighlighted || this.m_treeMb.Value.IsNullOrDestroyed())
          return;
        treeRenderer.RemoveTreeHighlight(this.m_treeMb.Value, TreeHarvestingDesignatorRenderer.HIGHLIGHT_COLOR);
        this.m_isHighlighted = false;
      }

      public void UpdateHighlight(ITreeRenderer treeRenderer)
      {
        if ((this.IsInHarvest || this.m_isInHarvestTemp) && !this.m_isRemovedTemp)
        {
          if (this.m_isHighlighted)
            return;
          if (this.m_treeMb.IsNone)
            this.m_treeMb = treeRenderer.GetMbOrNoneFor(this.m_tree);
          if (!this.m_treeMb.HasValue || this.m_treeMb.Value.IsNullOrDestroyed())
            return;
          treeRenderer.HighlightTree(this.m_treeMb.Value, TreeHarvestingDesignatorRenderer.HIGHLIGHT_COLOR);
          this.m_isHighlighted = true;
        }
        else
          this.RemoveHighlight(treeRenderer);
      }
    }
  }
}
