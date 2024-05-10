// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.TerrainCursor
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
namespace Mafi.Unity.InputControl
{
  /// <summary>Provides mouse position projected on the terrain.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TerrainCursor
  {
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ITerrainRenderer m_terrainRenderer;
    public readonly TerrainManager TerrainManager;
    private readonly UnityEngine.Camera m_camera;
    private bool m_isActivated;
    private bool m_isValid;
    private bool m_hasValue;
    private ThicknessTilesF m_relativeHeight;
    private Tile3f m_cursorPosition;

    public TerrainCursor(
      IGameLoopEvents gameLoopEvents,
      ITerrainRenderer terrainRenderer,
      TerrainManager terrainManager,
      CameraController camera)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents.CheckNotNull<IGameLoopEvents>();
      this.m_terrainRenderer = terrainRenderer.CheckNotNull<ITerrainRenderer>();
      this.TerrainManager = terrainManager.CheckNotNull<TerrainManager>();
      this.m_camera = camera.Camera.CheckNotNull<UnityEngine.Camera>();
    }

    public void Activate()
    {
      if (this.m_isActivated)
        return;
      this.m_isActivated = true;
      this.m_relativeHeight = ThicknessTilesF.Zero;
      this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<TerrainCursor>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void Deactivate()
    {
      if (!this.m_isActivated)
        return;
      this.m_isActivated = false;
      this.m_hasValue = false;
      this.m_isValid = false;
      this.m_gameLoopEvents.RenderUpdate.RemoveNonSaveable<TerrainCursor>(this, new Action<GameTime>(this.renderUpdate));
    }

    private void renderUpdate(GameTime time) => this.m_isValid = false;

    private void ensureValid()
    {
      Assert.That<bool>(this.m_isActivated).IsTrue("Terrain cursor was not activated!");
      if (this.m_isValid)
        return;
      this.recompute();
      Assert.That<bool>(this.m_isValid).IsTrue();
    }

    private void recompute()
    {
      Tile3f position;
      if (this.TryComputeTerrainPosition(Input.mousePosition, out position))
      {
        this.m_cursorPosition = position.SetZ((this.TerrainManager.GetHeight(this.m_cursorPosition.Xy) + this.m_relativeHeight).Value);
        this.m_hasValue = true;
      }
      else
        this.m_hasValue = false;
      this.m_isValid = true;
    }

    /// <summary>
    /// This performs non-cached computation but the cursor does not need to be activated.
    /// </summary>
    public bool TryComputeCurrentPosition(out Tile3f position)
    {
      return this.TryComputeTerrainPosition(Input.mousePosition, out position);
    }

    /// <summary>
    /// This performs non-cached computation but the cursor does not need to be activated.
    /// </summary>
    public bool TryComputeTerrainPosition(Vector3 mousePosition, out Tile3f position)
    {
      Ray ray = this.m_camera.ScreenPointToRay(mousePosition);
      ray.origin -= new Vector3(0.0f, this.m_relativeHeight.ToUnityUnits(), 0.0f);
      Tile3f? nullable = this.m_terrainRenderer.Raycast(ray);
      if (nullable.HasValue)
      {
        position = nullable.Value;
        return true;
      }
      position = new Tile3f();
      return false;
    }

    /// <summary>Gets or sets relative cursor height.</summary>
    public ThicknessTilesF RelativeHeightTilesF
    {
      get => this.m_relativeHeight;
      set
      {
        Assert.That<bool>(this.m_isActivated).IsTrue("Terrain cursor was not activated!");
        this.m_relativeHeight = value;
        this.m_isValid = false;
      }
    }

    /// <summary>Gets or sets relative cursor height in tiles.</summary>
    public ThicknessTilesI RelativeHeight
    {
      get => this.m_relativeHeight.RoundedThicknessTilesI;
      set
      {
        Assert.That<bool>(this.m_isActivated).IsTrue("Terrain cursor was not activated!");
        this.m_relativeHeight = value.ThicknessTilesF;
        this.m_isValid = false;
      }
    }

    /// <summary>
    /// Whether this cursor has valid value. If false is returned the mouse is currently not hovering over terrain..
    /// </summary>
    public bool HasValue
    {
      get
      {
        this.ensureValid();
        return this.m_hasValue;
      }
    }

    /// <summary>
    /// Current global discrete coord of a mouse position projected on the terrain. Only valid if <see cref="P:Mafi.Unity.InputControl.TerrainCursor.HasValue" /> is true.
    /// </summary>
    public Tile3f Tile3f
    {
      get
      {
        this.ensureValid();
        return this.m_cursorPosition;
      }
    }

    /// <summary>
    /// Current 2D global discrete coord of a mouse position projected on the terrain. Only valid if <see cref="P:Mafi.Unity.InputControl.TerrainCursor.HasValue" /> is true.
    /// </summary>
    public Tile2f Tile2f => this.Tile3f.Xy;

    public Tile2i Tile2i => this.Tile3f.Xy.Tile2i;

    public Tile3i Tile3i => this.Tile3f.Tile3i;

    /// <summary>Current hovered terrain tile.</summary>
    public TerrainTile Tile => this.TerrainManager[this.Tile2i];

    public Tile2i Tile2iClampedToLimits => this.TerrainManager.ClampToTerrainLimits(this.Tile2i);
  }
}
