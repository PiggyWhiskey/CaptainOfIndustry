// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TreeRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Trees;
using Mafi.Unity.Entities.Static;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TreeRenderer : ITreeRenderer, IDisposable
  {
    private static readonly int TREE_UPDATE_PERIOD;
    private readonly ITreesManager m_treeManager;
    private readonly ICalendar m_calendar;
    private readonly DependencyResolver m_resolver;
    private readonly ObjectHighlighter m_objectHighlighter;
    private readonly DelayedItemsProcessing<TreeData> m_treesPreviewObserver;
    private readonly DelayedItemsProcessing<TreeData> m_treesObserver;
    private readonly DelayedItemsProcessing<TreeStumpData> m_stumpObserver;
    private readonly Dict<TreeId, TreeMb> m_trees;
    private readonly Dict<TreeId, TreeMb> m_treePreviews;
    private readonly Dict<TreeId, StumpMb> m_stumps;
    private readonly Dict<GameObject, TreeMb> m_GoToTreeMbs;
    private readonly Set<TreeMb> m_collapsingTreeMbs;
    private readonly Lyst<TreeMb> m_collapsingTreeMbToRemove;
    private readonly Lyst<Set<TreeMb>> m_growingTreeMbs;
    private int m_growingUpdateTick;
    private readonly Lyst<TreeMb> m_growingTreeMbToRemove;
    private readonly Lyst<TreeId> m_collapsing;
    private int m_treesCounter;
    private int m_stumpsCounter;
    private readonly Material m_blueprintMaterialShared;

    public TreeRenderer(
      IGameLoopEvents gameLoopEvents,
      ITreesManager treeManager,
      ICalendar calendar,
      DependencyResolver resolver,
      ObjectHighlighter objectHighlighter,
      AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_trees = new Dict<TreeId, TreeMb>();
      this.m_treePreviews = new Dict<TreeId, TreeMb>();
      this.m_stumps = new Dict<TreeId, StumpMb>();
      this.m_GoToTreeMbs = new Dict<GameObject, TreeMb>();
      this.m_collapsingTreeMbs = new Set<TreeMb>();
      this.m_collapsingTreeMbToRemove = new Lyst<TreeMb>();
      this.m_growingTreeMbs = new Lyst<Set<TreeMb>>();
      this.m_growingTreeMbToRemove = new Lyst<TreeMb>();
      this.m_collapsing = new Lyst<TreeId>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_treeManager = treeManager.CheckNotNull<ITreesManager>();
      this.m_calendar = calendar.CheckNotNull<ICalendar>();
      this.m_resolver = resolver;
      this.m_objectHighlighter = objectHighlighter;
      this.m_blueprintMaterialShared = assetsDb.GetSharedMaterial("Assets/Core/Materials/BuildingBlueprint.mat");
      this.m_treesPreviewObserver = new DelayedItemsProcessing<TreeData>(new Action<TreeData>(this.addTreePreviewImmediate), new Action<TreeData>(this.removeTreePreviewImmediate));
      this.m_treesObserver = new DelayedItemsProcessing<TreeData>(new Action<TreeData>(this.addTreeImmediate), new Action<TreeData>(this.removeTreeImmediate));
      this.m_stumpObserver = new DelayedItemsProcessing<TreeStumpData>(new Action<TreeStumpData>(this.addStumpImmediate), new Action<TreeStumpData>(this.removeStumpImmediate));
      for (int index = 0; index < TreeRenderer.TREE_UPDATE_PERIOD; ++index)
        this.m_growingTreeMbs.Add(new Set<TreeMb>());
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      gameLoopEvents.SyncUpdate.AddNonSaveable<TreeRenderer>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<TreeRenderer>(this, new Action<GameTime>(this.renderUpdate));
      treeManager.TreeCollapseTriggered += new Action<TreeId>(this.treeCollapseTriggered);
    }

    private void initState()
    {
      foreach (TreeData tree in this.m_treeManager.Trees.Values)
        this.addTreeImmediate(tree);
      foreach (TreeData tree in this.m_treeManager.PreviewTrees.Values)
        this.addTreePreviewImmediate(tree);
      foreach (TreeStumpData stump in this.m_treeManager.Stumps.Values)
        this.addStumpImmediate(stump);
      this.m_treeManager.TreePreviewAdded.AddNonSaveable<TreeRenderer>(this, new Action<TreeData>(this.m_treesPreviewObserver.AddOnSim));
      this.m_treeManager.TreePreviewRemoved.AddNonSaveable<TreeRenderer>(this, new Action<TreeData>(this.m_treesPreviewObserver.RemoveOnSim));
      this.m_treeManager.TreeAdded.AddNonSaveable<TreeRenderer>(this, new Action<TreeData>(this.m_treesObserver.AddOnSim));
      this.m_treeManager.TreeRemoved.AddNonSaveable<TreeRenderer>(this, new Action<TreeData>(this.m_treesObserver.RemoveOnSim));
      this.m_treeManager.StumpAdded.AddNonSaveable<TreeRenderer>(this, new Action<TreeStumpData>(this.m_stumpObserver.AddOnSim));
      this.m_treeManager.StumpRemoved.AddNonSaveable<TreeRenderer>(this, new Action<TreeStumpData>(this.m_stumpObserver.RemoveOnSim));
    }

    public void Dispose()
    {
      foreach (Component component in this.m_trees.Values)
        component.gameObject.Destroy();
      foreach (Component component in this.m_stumps.Values)
        component.gameObject.Destroy();
      this.m_trees.Clear();
      this.m_stumps.Clear();
      this.m_treeManager.TreeCollapseTriggered -= new Action<TreeId>(this.treeCollapseTriggered);
    }

    public Option<TreeMb> GetMbOrNoneFor(TreeId tree)
    {
      TreeMb treeMb;
      return this.m_trees.TryGetValue(tree, out treeMb) ? (Option<TreeMb>) treeMb : (Option<TreeMb>) Option.None;
    }

    public Option<TreeMb> GetPlannedMbOrNoneFor(TreeId tree)
    {
      TreeMb treeMb;
      return this.m_treePreviews.TryGetValue(tree, out treeMb) ? (Option<TreeMb>) treeMb : (Option<TreeMb>) Option.None;
    }

    private void treeCollapseTriggered(TreeId tree) => this.m_collapsing.Add(tree);

    private void syncUpdate(GameTime time)
    {
      foreach (TreeId key in this.m_collapsing)
      {
        TreeMb treeMb;
        if (this.m_trees.TryGetValue(key, out treeMb))
        {
          treeMb.TriggerCollapse();
          this.m_collapsingTreeMbs.Add(treeMb);
        }
      }
      this.m_collapsing.Clear();
      this.m_treesPreviewObserver.SyncUpdate();
      this.m_treesPreviewObserver.RenderUpdate();
      this.m_stumpObserver.SyncUpdate();
      this.m_stumpObserver.RenderUpdate();
      this.m_treesObserver.SyncUpdate();
      this.m_treesObserver.RenderUpdate();
      if (!time.IsGamePaused)
      {
        foreach (StumpMb mb in this.m_stumps.Values)
        {
          if (!mb.IsNullOrDestroyed())
            mb.SyncUpdate(time);
        }
      }
      Set<TreeMb> growingTreeMb = this.m_growingTreeMbs[this.m_growingUpdateTick & TreeRenderer.TREE_UPDATE_PERIOD - 1];
      ++this.m_growingUpdateTick;
      if (this.m_growingTreeMbs.IsNotEmpty)
      {
        this.m_growingTreeMbToRemove.Clear();
        foreach (TreeMb mb in growingTreeMb)
        {
          TreeData treeData;
          if (mb.IsNullOrDestroyed() || mb.IsBeingHarvested || mb.IsCollapsing || this.m_treeManager.Trees.TryGetValue(mb.TreeId, out treeData) && treeData.GetScaleIgnoringBase(this.m_calendar) >= Percent.Hundred)
            this.m_growingTreeMbToRemove.Add(mb);
          else
            mb.SyncUpdate(time);
        }
        foreach (TreeMb treeMb in this.m_growingTreeMbToRemove)
          growingTreeMb.Remove(treeMb);
        this.m_growingTreeMbToRemove.Clear();
      }
      if (!this.m_collapsingTreeMbs.IsNotEmpty)
        return;
      this.m_collapsingTreeMbToRemove.Clear();
      foreach (TreeMb collapsingTreeMb in this.m_collapsingTreeMbs)
      {
        if (collapsingTreeMb.IsNullOrDestroyed() || !collapsingTreeMb.IsCollapsing)
          this.m_collapsingTreeMbToRemove.Add(collapsingTreeMb);
      }
      foreach (TreeMb treeMb in this.m_collapsingTreeMbToRemove)
        this.m_collapsingTreeMbs.Remove(treeMb);
      this.m_collapsingTreeMbToRemove.Clear();
    }

    private void renderUpdate(GameTime time)
    {
      foreach (TreeMb collapsingTreeMb in this.m_collapsingTreeMbs)
      {
        if (collapsingTreeMb.IsCollapsing)
          collapsingTreeMb.RenderUpdateCollapsing(time);
      }
    }

    private void addTreeImmediate(TreeData tree)
    {
      Assert.That<bool>(tree.IsValid).IsTrue();
      Assert.That<Dict<TreeId, TreeMb>>(this.m_trees).NotContainsKey<TreeId, TreeMb>(tree.Id);
      TreeMb treeMb = this.m_resolver.InvokeFactoryHierarchy<TreeMb>((object) tree);
      treeMb.name = "Tr " + this.m_treesCounter++.ToString();
      this.m_trees.Add(tree.Id, treeMb);
      this.m_GoToTreeMbs.Add(treeMb.gameObject, treeMb);
      if (!(tree.GetScaleIgnoringBase(this.m_calendar) < Percent.Hundred))
        return;
      this.m_growingTreeMbs[tree.Id.GetHashCode() % TreeRenderer.TREE_UPDATE_PERIOD].Add(treeMb);
    }

    private void addTreePreviewImmediate(TreeData tree)
    {
      Assert.That<bool>(tree.IsValid).IsTrue();
      Assert.That<Dict<TreeId, TreeMb>>(this.m_treePreviews).NotContainsKey<TreeId, TreeMb>(tree.Id);
      TreeMb treeMb = this.m_resolver.InvokeFactoryHierarchy<TreeMb>((object) tree);
      treeMb.name = "Tr " + this.m_treesCounter++.ToString();
      treeMb.SetAsPreview();
      this.m_treePreviews.Add(tree.Id, treeMb);
      this.m_GoToTreeMbs.Add(treeMb.gameObject, treeMb);
      treeMb.EnsureBlueprintMaterial(this.m_blueprintMaterialShared, InstancedChunkBasedLayoutEntitiesRenderer.BLUEPRINT_CONSTRUCTION_COLOR);
    }

    private void addStumpImmediate(TreeStumpData stump)
    {
      Assert.That<Dict<TreeId, StumpMb>>(this.m_stumps).NotContainsKey<TreeId, StumpMb>(stump.Id);
      StumpMb stumpMb = this.m_resolver.InvokeFactoryHierarchy<StumpMb>((object) stump);
      stumpMb.name = "St " + this.m_stumpsCounter++.ToString();
      this.m_stumps.Add(stump.Id, stumpMb);
      Option<TreeMb> option = this.m_trees.Get<TreeId, TreeMb>(stump.Id);
      if (option.HasValue)
        stumpMb.LoadPrefabFromTree(option.Value);
      else
        stumpMb.LoadPrefabFromProto(stump.TreeProto);
    }

    private void removeTreeImmediate(TreeData tree)
    {
      TreeMb treeMb;
      if (!this.m_trees.TryGetValue(tree.Id, out treeMb))
      {
        Assert.Fail("Trying to remove non-existent tree from renderer.");
      }
      else
      {
        this.m_objectHighlighter.RemoveAllHighlights(treeMb.gameObject);
        this.m_GoToTreeMbs.Remove(treeMb.gameObject);
        if (!treeMb.IsBeingHarvested && !treeMb.IsCollapsing)
          treeMb.gameObject.Destroy();
        this.m_trees.Remove(tree.Id);
      }
    }

    private void removeTreePreviewImmediate(TreeData tree)
    {
      TreeMb treeMb;
      if (!this.m_treePreviews.TryGetValue(tree.Id, out treeMb))
      {
        Assert.Fail("Trying to remove non-existent tree from renderer.");
      }
      else
      {
        this.m_objectHighlighter.RemoveAllHighlights(treeMb.gameObject);
        this.m_GoToTreeMbs.Remove(treeMb.gameObject);
        if (!treeMb.IsBeingHarvested && !treeMb.IsCollapsing)
          treeMb.gameObject.Destroy();
        this.m_treePreviews.Remove(tree.Id);
      }
    }

    private void removeStumpImmediate(TreeStumpData stump)
    {
      StumpMb stumpMb;
      if (!this.m_stumps.TryGetValue(stump.Id, out stumpMb))
      {
        Assert.Fail("Trying to remove non-existent stump from renderer.");
      }
      else
      {
        this.m_objectHighlighter.RemoveAllHighlights(stumpMb.gameObject);
        stumpMb.gameObject.Destroy();
        this.m_stumps.Remove(stump.Id);
      }
    }

    public void HighlightTree(TreeMb treeMb, ColorRgba color)
    {
      this.m_objectHighlighter.Highlight(treeMb.gameObject, color);
    }

    public void RemoveTreeHighlight(TreeMb treeMb, ColorRgba color)
    {
      this.m_objectHighlighter.RemoveHighlight(treeMb.gameObject, color);
    }

    public bool TryGetTreeMbFromGO(GameObject go, out TreeMb treeMb)
    {
      return this.m_GoToTreeMbs.TryGetValue(go, out treeMb);
    }

    public bool TryGetTreeMb(TreeId treeId, out TreeMb treeMb)
    {
      return this.m_trees.TryGetValue(treeId, out treeMb);
    }

    public bool TryGetStumpMb(TreeId treeId, out StumpMb stumpMb)
    {
      return this.m_stumps.TryGetValue(treeId, out stumpMb);
    }

    static TreeRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TreeRenderer.TREE_UPDATE_PERIOD = 4;
    }
  }
}
