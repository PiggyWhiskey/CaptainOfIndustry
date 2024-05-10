// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.PropsRemovalInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.Terrain;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class PropsRemovalInputController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private static readonly RelTile1i MAX_AREA_EDGE_SIZE;
    private readonly IUnityInputMgr m_inputManager;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly TerrainPropsRenderer m_terrainPropsRenderer;
    private readonly TerrainPropsManager m_terrainPropsManager;
    private readonly TreesManager m_treesManager;
    private readonly TreeRenderer m_treesRenderer;
    private readonly ObjectHighlighter m_objectHighlighter;
    private readonly InputScheduler m_inputScheduler;
    private readonly PropsRemovalInputController.UpointsToolToolbox m_toolbox;
    private readonly AreaSelectionTool m_areaSelectionTool;
    private readonly FloatingUpointsCostPopup m_costPopup;
    private readonly Cursoor m_cursor;
    private readonly AudioSource m_successSound;
    private readonly AudioSource m_invalidSound;
    private LystStruct<KeyValuePair<ushort, TerrainPropData>> m_highlightedProps;
    private LystStruct<TreeId> m_highlightedStumps;
    private Option<QuickRemovePropsCmd> m_ongoingCmd;

    public event Action<IToolbarItemController> VisibilityChanged;

    public ControllerConfig Config => ControllerConfig.Tool;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public PropsRemovalInputController(
      IUnityInputMgr inputManager,
      ToolbarController toolbarController,
      ShortcutsManager shortcutsManager,
      TerrainPropsRenderer terrainPropsRenderer,
      TerrainPropsManager terrainPropsManager,
      TreesManager treesManager,
      TreeRenderer treesRenderer,
      ObjectHighlighter objectHighlighter,
      AreaSelectionToolFactory areaSelectionToolFactory,
      InputScheduler inputScheduler,
      CursorManager cursorManager,
      UiBuilder builder,
      NewInstanceOf<FloatingUpointsCostPopup> costPopup)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputManager = inputManager;
      this.m_shortcutsManager = shortcutsManager;
      this.m_terrainPropsRenderer = terrainPropsRenderer;
      this.m_terrainPropsManager = terrainPropsManager;
      this.m_treesManager = treesManager;
      this.m_treesRenderer = treesRenderer;
      this.m_objectHighlighter = objectHighlighter;
      this.m_inputScheduler = inputScheduler;
      this.m_areaSelectionTool = areaSelectionToolFactory.CreateInstance(new Action<RectangleTerrainArea2i, bool>(this.updateSelectionSync), new Action<RectangleTerrainArea2i, bool>(this.selectionDone), new Action(this.clearSelection), new Action(this.onDeactivateSelf));
      this.m_areaSelectionTool.SetEdgeSizeLimit(PropsRemovalInputController.MAX_AREA_EDGE_SIZE);
      this.m_areaSelectionTool.SetLeftClickColor(ColorRgba.Red);
      this.m_costPopup = costPopup.Instance;
      this.m_toolbox = new PropsRemovalInputController.UpointsToolToolbox(toolbarController, builder);
      this.m_successSound = builder.AudioDb.GetSharedAudio(builder.Audio.Assign);
      this.m_invalidSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
      this.m_cursor = cursorManager.RegisterCursor(builder.Style.Cursors.Upoints);
    }

    private void onDeactivateSelf()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      controller.AddLeftMenuButton(TrCore.PropsRemovalTool.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Sweep.svg", 60f, (Func<ShortcutsManager, KeyBindings>) (m => m.TogglePropsRemovalTool));
    }

    public void Activate()
    {
      this.m_toolbox.Show();
      this.m_cursor.Show();
      this.m_areaSelectionTool.TerrainCursor.Activate();
    }

    public void Deactivate()
    {
      this.m_costPopup.Hide();
      this.m_toolbox.Hide();
      this.m_cursor.Hide();
      this.m_areaSelectionTool.TerrainCursor.Deactivate();
      this.m_areaSelectionTool.Deactivate();
      this.clearSelection();
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.m_shortcutsManager.IsSecondaryActionUp)
      {
        this.onDeactivateSelf();
        return true;
      }
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.m_shortcutsManager.IsPrimaryActionOn);
      this.m_costPopup.UpdatePosition();
      if (this.m_ongoingCmd.HasValue && this.m_ongoingCmd.Value.IsProcessedAndSynced)
      {
        if (this.m_ongoingCmd.Value.Result)
          this.m_successSound.Play();
        else
          this.m_invalidSound.Play();
        this.clearSelection();
        this.m_ongoingCmd = (Option<QuickRemovePropsCmd>) Option.None;
      }
      if (this.m_shortcutsManager.IsPrimaryActionDown)
      {
        this.m_areaSelectionTool.Activate(true);
        return true;
      }
      return this.m_areaSelectionTool.IsActive;
    }

    private void selectionDone(RectangleTerrainArea2i area, bool leftClick)
    {
      this.m_ongoingCmd = (Option<QuickRemovePropsCmd>) this.m_inputScheduler.ScheduleInputCmd<QuickRemovePropsCmd>(new QuickRemovePropsCmd(area));
    }

    private void clearSelection()
    {
      foreach (KeyValuePair<ushort, TerrainPropData> highlightedProp in this.m_highlightedProps)
        this.m_terrainPropsRenderer.RemoveHighlight(highlightedProp.Value, highlightedProp.Key);
      foreach (TreeId highlightedStump in this.m_highlightedStumps)
      {
        StumpMb stumpMb;
        if (this.m_treesRenderer.TryGetStumpMb(highlightedStump, out stumpMb))
          this.m_objectHighlighter.RemoveHighlight(stumpMb.gameObject, DeleteEntityInputController.COLOR_HIGHLIGHT);
      }
      this.m_highlightedProps.Clear();
      this.m_highlightedStumps.Clear();
    }

    private void updateSelectionSync(RectangleTerrainArea2i area, bool leftClick)
    {
      this.clearSelection();
      if (!this.m_areaSelectionTool.IsActive)
        return;
      foreach (TerrainPropData propData in this.m_terrainPropsManager.EnumeratePropsInArea(area))
        this.m_highlightedProps.Add(Make.Kvp<ushort, TerrainPropData>(this.m_terrainPropsRenderer.AddHighlight(propData, DeleteEntityInputController.COLOR_HIGHLIGHT), propData));
      foreach (TreeId treeId in this.m_treesManager.EnumerateStumpsInArea(area))
      {
        StumpMb stumpMb;
        if (this.m_treesRenderer.TryGetStumpMb(treeId, out stumpMb))
          this.m_objectHighlighter.Highlight(stumpMb.gameObject, DeleteEntityInputController.COLOR_HIGHLIGHT);
        this.m_highlightedStumps.Add(treeId);
      }
      this.m_costPopup.SetUpointsCost((this.m_highlightedProps.Count + this.m_highlightedStumps.Count) * PropsRemovalProcessor.COST_PER_PROP, TrCore.PropsRemovalTool.TranslatedString);
    }

    static PropsRemovalInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      PropsRemovalInputController.MAX_AREA_EDGE_SIZE = new RelTile1i(200);
    }

    private class UpointsToolToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;

      public UpointsToolToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("UpointsTool", "Assets/Unity/UserInterface/Toolbar/UpointsTool.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.PropsRemovalTool__Tooltip);
        this.AddToToolbar();
      }
    }
  }
}
