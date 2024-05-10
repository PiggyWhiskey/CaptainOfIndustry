// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Trees.TreeHarvestingDesignatorController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Trees
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class TreeHarvestingDesignatorController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private static readonly RelTile1i MAX_AREA_EDGE_SIZE;
    private readonly IUnityInputMgr m_inputManager;
    private readonly ToolbarController m_toolbarController;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly TreeHarvestingToolbox m_toolbox;
    private readonly TreeHarvestingDesignatorRenderer m_designatorRenderer;
    private readonly IActivator m_designatorRendererActivator;
    private readonly InputScheduler m_inputScheduler;
    private readonly CursorManager m_cursorManager;
    private readonly TreesManager m_treeManager;
    private readonly AreaSelectionTool m_areaSelectionTool;
    private Cursoor m_circularSawCursor;
    private readonly Set<TreeId> m_selectedTrees;
    private readonly Lyst<TreeId> m_removedTreesTmp;
    private readonly Lyst<TreeId> m_addedTreesTmp;
    private Option<DesignateHarvestedTreesCmd> m_ongoingCmd;
    private readonly AudioSource m_harvestApplySound;
    private readonly AudioSource m_errorSound;
    private readonly AudioSource m_harvestRemoveSound;

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public ControllerConfig Config => ControllerConfig.Tool;

    public TreeHarvestingDesignatorController(
      IUnityInputMgr inputManager,
      ToolbarController toolbarController,
      ShortcutsManager shortcutsManager,
      TreeHarvestingToolbox toolbox,
      TreeHarvestingDesignatorRenderer designatorRenderer,
      AreaSelectionToolFactory areaSelectionToolFactory,
      InputScheduler inputScheduler,
      CursorManager cursorManager,
      TreesManager treeManager,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_selectedTrees = new Set<TreeId>();
      this.m_removedTreesTmp = new Lyst<TreeId>(true);
      this.m_addedTreesTmp = new Lyst<TreeId>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputManager = inputManager;
      this.m_toolbarController = toolbarController;
      this.m_shortcutsManager = shortcutsManager;
      this.m_toolbox = toolbox;
      this.m_designatorRenderer = designatorRenderer;
      this.m_designatorRendererActivator = designatorRenderer.CreateActivator();
      this.m_inputScheduler = inputScheduler;
      this.m_cursorManager = cursorManager;
      this.m_treeManager = treeManager;
      this.m_areaSelectionTool = areaSelectionToolFactory.CreateInstance(new Action<RectangleTerrainArea2i, bool>(this.updateSelectionSync), new Action<RectangleTerrainArea2i, bool>(this.selectionDone), new Action(this.clearSelection), new Action(this.onDeactivateSelf));
      this.m_areaSelectionTool.SetEdgeSizeLimit(TreeHarvestingDesignatorController.MAX_AREA_EDGE_SIZE);
      this.m_circularSawCursor = this.m_cursorManager.RegisterCursor(builder.Style.Cursors.CircularSaw);
      this.m_harvestApplySound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/TreeHarvest.prefab");
      this.m_harvestRemoveSound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/ForestryApply.prefab");
      this.m_errorSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
    }

    private void onDeactivateSelf()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      this.m_toolbarController.AddMainMenuButton(Tr.Designation__TreeHarvesting.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/TreeHarvesting.svg", 350f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleTreeHarvestingTool));
    }

    public void Activate()
    {
      this.m_toolbox.Show();
      this.m_circularSawCursor.Show();
      this.m_areaSelectionTool.TerrainCursor.Activate();
      this.m_designatorRendererActivator.Activate();
    }

    public void Deactivate()
    {
      this.m_toolbox.Hide();
      this.m_circularSawCursor.Hide();
      this.m_areaSelectionTool.TerrainCursor.Deactivate();
      this.m_designatorRendererActivator.Deactivate();
      this.m_areaSelectionTool.Deactivate();
      this.clearSelection();
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.m_shortcutsManager.IsPrimaryActionOn);
      this.m_toolbox.SecondaryActionBtn.SetIsOn(this.m_shortcutsManager.IsSecondaryActionOn);
      if (this.m_ongoingCmd.HasValue && this.m_ongoingCmd.Value.IsProcessedAndSynced)
      {
        this.clearSelection();
        this.m_ongoingCmd = Option<DesignateHarvestedTreesCmd>.None;
      }
      bool flag = this.m_shortcutsManager.IsDown(this.m_shortcutsManager.ClearDesignation);
      if (this.m_shortcutsManager.IsPrimaryActionDown | flag)
      {
        this.m_areaSelectionTool.Activate(!flag);
        return true;
      }
      return this.m_areaSelectionTool.IsActive;
    }

    private void selectionDone(RectangleTerrainArea2i area, bool leftClick)
    {
      this.m_ongoingCmd = (Option<DesignateHarvestedTreesCmd>) this.m_inputScheduler.ScheduleInputCmd<DesignateHarvestedTreesCmd>(new DesignateHarvestedTreesCmd(area, leftClick, new ProductProto.ID?()));
      if (leftClick)
      {
        if (this.m_selectedTrees.IsNotEmpty)
          this.m_harvestApplySound.Play();
        else
          this.m_errorSound.Play();
      }
      else
        this.m_harvestRemoveSound.Play();
    }

    private void clearSelection()
    {
      foreach (TreeId selectedTree in this.m_selectedTrees)
        this.m_designatorRenderer.RemoveTemporaryEffects(selectedTree);
      this.m_selectedTrees.Clear();
    }

    private void updateSelectionSync(RectangleTerrainArea2i area, bool leftClick)
    {
      if (!this.m_areaSelectionTool.IsActive && this.m_selectedTrees.IsNotEmpty)
      {
        this.clearSelection();
      }
      else
      {
        this.m_addedTreesTmp.Clear();
        this.m_removedTreesTmp.Clear();
        this.m_treeManager.UpdateSelectedTreesInArea(area, new ProductProto.ID?(), new bool?(!leftClick), this.m_selectedTrees, this.m_addedTreesTmp, this.m_removedTreesTmp);
        foreach (TreeId tree in this.m_removedTreesTmp)
        {
          this.m_designatorRenderer.RemoveTemporaryEffects(tree);
          this.m_selectedTrees.Remove(tree);
        }
        if (leftClick)
        {
          foreach (TreeId tree in this.m_addedTreesTmp)
            this.m_designatorRenderer.TemporarilyAdd(tree);
        }
        else
        {
          foreach (TreeId tree in this.m_addedTreesTmp)
            this.m_designatorRenderer.TemporarilyRemove(tree);
        }
      }
    }

    static TreeHarvestingDesignatorController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TreeHarvestingDesignatorController.MAX_AREA_EDGE_SIZE = new RelTile1i(200);
    }
  }
}
