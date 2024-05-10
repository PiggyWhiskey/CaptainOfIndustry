// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.AreaTool.AreaSelectionTool
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Terrain;
using Mafi.Unity.Camera;
using Mafi.Unity.Terrain;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.AreaTool
{
  /// <summary>
  /// Tool for click and drag using mouse to select a terrain area (rectangular).
  /// </summary>
  public class AreaSelectionTool
  {
    public static readonly Color SELECT_COLOR;
    private static readonly Color UNSELECT_COLOR;
    public bool IsActive;
    private readonly CameraController m_cameraController;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ShortcutsManager m_shortcutsManager;
    public readonly TerrainCursor TerrainCursor;
    private readonly TerrainRectSelection m_terrainOutlineRenderer;
    private readonly Action<RectangleTerrainArea2i, bool> m_onSelectionChangedSync;
    private readonly Action<RectangleTerrainArea2i, bool> m_onSelectionDone;
    private readonly Action m_onEmptyRightClick;
    private readonly Option<Action> m_onSelfDeactivated;
    private bool m_additionMode;
    private Tile2i? m_startTile;
    private Tile2i m_currentTile;
    private RectangleTerrainArea2i m_terrainArea;
    private bool m_selectionChanged;
    private Color m_leftClickColor;
    private Color m_rightClickColor;
    /// <summary>
    /// Used to transform area directly selected by user into final selected area, if specified. Can be used to
    /// limit area size or align the area to a specific grid.
    /// </summary>
    private Option<Func<RectangleTerrainArea2i, RectangleTerrainArea2i>> m_areaPreprocessor;
    private Vector3 m_startMousePosition;

    public RelTile1i MaxEdgeSize { get; private set; }

    public AreaSelectionTool(
      CameraController cameraController,
      IGameLoopEvents gameLoopEvents,
      ShortcutsManager shortcutsManager,
      TerrainCursor terrainCursor,
      TerrainRectSelection terrainOutlineRenderer,
      Action<RectangleTerrainArea2i, bool> onSelectionChangedSync,
      Action<RectangleTerrainArea2i, bool> onSelectionDone,
      Action onEmptyRightClick,
      Option<Action> onSelfDeactivated)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxEdgeSize\u003Ek__BackingField = RelTile1i.MaxValue;
      this.m_leftClickColor = AreaSelectionTool.SELECT_COLOR;
      this.m_rightClickColor = AreaSelectionTool.UNSELECT_COLOR;
      this.m_areaPreprocessor = (Option<Func<RectangleTerrainArea2i, RectangleTerrainArea2i>>) Option.None;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_cameraController = cameraController;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_shortcutsManager = shortcutsManager;
      this.TerrainCursor = terrainCursor;
      this.m_terrainOutlineRenderer = terrainOutlineRenderer;
      this.m_onSelectionChangedSync = onSelectionChangedSync;
      this.m_onSelectionDone = onSelectionDone;
      this.m_onEmptyRightClick = onEmptyRightClick;
      this.m_onSelfDeactivated = onSelfDeactivated;
    }

    public void SetEdgeSizeLimit(RelTile1i maxEdgeSize) => this.MaxEdgeSize = maxEdgeSize;

    /// <summary>Activates the tool.</summary>
    /// <param name="additionMode">Whether the we are adding area (green) or removing (red).</param>
    public void Activate(
      bool additionMode,
      Vector3? startMousePos = null,
      Func<RectangleTerrainArea2i, RectangleTerrainArea2i> areaPreprocessor = null)
    {
      if (!this.IsActive)
      {
        this.IsActive = true;
        this.m_gameLoopEvents.InputUpdate.AddNonSaveable<AreaSelectionTool>(this, new Action<GameTime>(this.inputUpdate));
        this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<AreaSelectionTool>(this, new Action<GameTime>(this.syncUpdate));
      }
      this.m_startMousePosition = startMousePos ?? Input.mousePosition;
      this.m_additionMode = additionMode;
      this.m_areaPreprocessor = Option.Create<Func<RectangleTerrainArea2i, RectangleTerrainArea2i>>(areaPreprocessor);
      this.m_selectionChanged = true;
      if (!additionMode)
        this.m_cameraController.SetMousePanDisabled(true);
      Tile3f position;
      if (this.TerrainCursor.TryComputeTerrainPosition(this.m_startMousePosition, out position))
      {
        this.m_startTile = new Tile2i?(this.m_currentTile = this.TerrainCursor.TerrainManager.ClampToTerrainLimits(position.Tile2i));
        this.m_terrainArea = this.transformToFinalArea(new RectangleTerrainArea2i(this.m_currentTile, RelTile2i.Zero));
      }
      else
      {
        this.m_startTile = new Tile2i?();
        this.m_terrainArea = new RectangleTerrainArea2i(Tile2i.Zero, RelTile2i.Zero);
      }
      this.m_terrainOutlineRenderer.SetArea(this.m_terrainArea, this.m_additionMode ? this.m_leftClickColor : this.m_rightClickColor);
      this.m_terrainOutlineRenderer.Show();
    }

    public void ForceSelectionChanged() => this.m_selectionChanged = true;

    public void SetLeftClickColor(ColorRgba color) => this.m_leftClickColor = color.ToColor();

    public void SetRightClickColor(ColorRgba color) => this.m_rightClickColor = color.ToColor();

    public void Deactivate()
    {
      if (!this.IsActive)
        return;
      if (!this.m_additionMode)
        this.m_cameraController.SetMousePanDisabled(false);
      this.IsActive = false;
      this.m_gameLoopEvents.InputUpdate.RemoveNonSaveable<AreaSelectionTool>(this, new Action<GameTime>(this.inputUpdate));
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<AreaSelectionTool>(this, new Action<GameTime>(this.syncUpdate));
      this.m_terrainOutlineRenderer.Hide();
    }

    private void inputUpdate(GameTime gameTime)
    {
      Assert.That<bool>(this.IsActive).IsTrue();
      if (!this.IsActive)
        return;
      if (this.TerrainCursor.HasValue)
      {
        Tile2i tile2iClampedToLimits = this.TerrainCursor.Tile2iClampedToLimits;
        if (tile2iClampedToLimits != this.m_currentTile)
        {
          if (!this.m_startTile.HasValue)
            this.m_startTile = new Tile2i?(tile2iClampedToLimits);
          this.m_currentTile = tile2iClampedToLimits;
          RelTile2i relTile2i = (this.m_startTile.Value - this.m_currentTile).AbsValue - new RelTile2i(this.MaxEdgeSize.Value, this.MaxEdgeSize.Value);
          Tile2i tile2i = this.m_currentTile;
          if (relTile2i.X > 0)
            tile2i = tile2i.AddX(this.m_startTile.Value.X < tile2i.X ? -relTile2i.X : relTile2i.X);
          if (relTile2i.Y > 0)
            tile2i = tile2i.AddY(this.m_startTile.Value.Y < tile2i.Y ? -relTile2i.Y : relTile2i.Y);
          this.m_currentTile = tile2i;
          this.m_terrainArea = this.transformToFinalArea(RectangleTerrainArea2i.FromTwoPositions(this.m_startTile.Value, this.m_currentTile));
          this.m_terrainOutlineRenderer.SetArea(this.m_terrainArea, this.m_additionMode ? this.m_leftClickColor : this.m_rightClickColor);
          this.m_selectionChanged = true;
        }
      }
      bool flag = this.m_shortcutsManager.IsOn(this.m_shortcutsManager.ClearDesignation);
      if (this.m_shortcutsManager.IsPrimaryActionOn && !flag && !this.m_additionMode || flag && this.m_additionMode)
      {
        this.deactivateSelf();
      }
      else
      {
        if ((!this.m_shortcutsManager.IsPrimaryActionUp || !this.m_additionMode) && (!this.m_shortcutsManager.IsUp(this.m_shortcutsManager.ClearDesignation) || this.m_additionMode))
          return;
        if (this.m_shortcutsManager.IsSecondaryActionUp && (double) (this.m_startMousePosition - Input.mousePosition).sqrMagnitude < 1.0)
        {
          this.m_onEmptyRightClick();
        }
        else
        {
          this.m_terrainOutlineRenderer.Hide();
          this.m_onSelectionDone(this.m_terrainArea, this.m_additionMode);
          this.Deactivate();
        }
      }
    }

    private void syncUpdate(GameTime gameTime)
    {
      if (!this.m_selectionChanged)
        return;
      this.m_selectionChanged = false;
      this.m_onSelectionChangedSync(this.m_terrainArea, this.m_additionMode);
    }

    private RectangleTerrainArea2i transformToFinalArea(RectangleTerrainArea2i area)
    {
      return !this.m_areaPreprocessor.HasValue ? area : this.m_areaPreprocessor.Value(area);
    }

    private void deactivateSelf()
    {
      this.Deactivate();
      if (!this.m_onSelfDeactivated.HasValue)
        return;
      this.m_onSelfDeactivated.Value();
    }

    static AreaSelectionTool()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AreaSelectionTool.SELECT_COLOR = Color.green;
      AreaSelectionTool.UNSELECT_COLOR = Color.red;
    }
  }
}
