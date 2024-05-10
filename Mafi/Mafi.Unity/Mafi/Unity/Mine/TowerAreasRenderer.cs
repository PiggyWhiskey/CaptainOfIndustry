// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.TowerAreasRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
using Mafi.Core.GameLoop;
using Mafi.Core.Terrain;
using Mafi.Unity.Terrain;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Mine
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class TowerAreasRenderer
  {
    private static readonly ColorUniversal AREA_LINE_COLOR;
    private static readonly ColorUniversal SELECTED_AREA_LINE_COLOR;
    /// <summary>
    /// All mine towers in the scene with their (cached) areas and outlines of areas.
    /// </summary>
    /// <remarks>
    /// List should be enough here (no need for HashSet) as the only cases when we need to search in for a tower in
    /// the list is tower addition and removal, which are expected to be infrequent.
    /// TODO: We also search this list when selecting a single tower.
    /// </remarks>
    private LystStruct<TowerAreasRenderer.TowerAreaData> m_towersData;
    /// <summary>Parent game object of all the are outlines.</summary>
    private readonly GameObject m_parentGo;
    private Option<TowerAreasRenderer.TowerAreaData> m_selectedTowerData;
    private readonly DelayedItemsProcessing<IAreaManagingTower> m_delayedProcessing;
    private readonly ActivatorState m_activator;
    private readonly DependencyResolver m_resolver;
    private readonly MineTowersManager m_mineTowersManager;
    private readonly ForestryTowersManager m_forestryTowersManager;
    private readonly TerrainDesignationsRenderer m_terrainDesignationsRenderer;
    private readonly SurfaceDesignationsRenderer m_surfaceDesignationsRenderer;

    internal bool IsActive => this.m_parentGo.activeSelf;

    public TowerAreasRenderer(
      DependencyResolver resolver,
      IGameLoopEvents gameLoopEvents,
      MineTowersManager mineTowersManager,
      ForestryTowersManager forestryTowersManager,
      TerrainDesignationsRenderer terrainDesignationsRenderer,
      SurfaceDesignationsRenderer surfaceDesignationsRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
      this.m_mineTowersManager = mineTowersManager;
      this.m_forestryTowersManager = forestryTowersManager;
      this.m_terrainDesignationsRenderer = terrainDesignationsRenderer;
      this.m_surfaceDesignationsRenderer = surfaceDesignationsRenderer;
      this.m_parentGo = new GameObject("TowerAreas");
      this.m_parentGo.SetActive(false);
      this.m_activator = new ActivatorState(new Action(this.activate), new Action(this.deactivate));
      this.m_delayedProcessing = new DelayedItemsProcessing<IAreaManagingTower>(new Action<IAreaManagingTower>(this.addTowerOrUpdateArea), new Action<IAreaManagingTower>(this.removeTower));
      gameLoopEvents.SyncUpdate.AddNonSaveable<TowerAreasRenderer>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<TowerAreasRenderer>(this, new Action<GameTime>(this.renderUpdate));
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.rendererLoadState));
    }

    private void rendererLoadState()
    {
      foreach (IAreaManagingTower tower in this.m_mineTowersManager.Towers)
        this.addTowerOrUpdateArea(tower);
      this.m_mineTowersManager.OnTowerAdded.AddNonSaveable<TowerAreasRenderer>(this, (Action<MineTower, EntityAddReason>) ((tower, reason) => this.m_delayedProcessing.AddOnSim((IAreaManagingTower) tower)));
      this.m_mineTowersManager.OnAreaChange.AddNonSaveable<TowerAreasRenderer>(this, (Action<MineTower, RectangleTerrainArea2i>) ((tower, oldArea) => this.m_delayedProcessing.AddOnSim((IAreaManagingTower) tower)));
      this.m_mineTowersManager.OnTowerRemoved.AddNonSaveable<TowerAreasRenderer>(this, (Action<MineTower, EntityRemoveReason>) ((tower, reason) => this.m_delayedProcessing.RemoveOnSim((IAreaManagingTower) tower)));
      foreach (IAreaManagingTower tower in this.m_forestryTowersManager.Towers)
        this.addTowerOrUpdateArea(tower);
      this.m_forestryTowersManager.OnTowerAdded.AddNonSaveable<TowerAreasRenderer>(this, (Action<ForestryTower, EntityAddReason>) ((tower, reason) => this.m_delayedProcessing.AddOnSim((IAreaManagingTower) tower)));
      this.m_forestryTowersManager.OnAreaChange.AddNonSaveable<TowerAreasRenderer>(this, (Action<ForestryTower, RectangleTerrainArea2i>) ((tower, oldArea) => this.m_delayedProcessing.AddOnSim((IAreaManagingTower) tower)));
      this.m_forestryTowersManager.OnTowerRemoved.AddNonSaveable<TowerAreasRenderer>(this, (Action<ForestryTower, EntityRemoveReason>) ((tower, reason) => this.m_delayedProcessing.RemoveOnSim((IAreaManagingTower) tower)));
    }

    private void syncUpdate(GameTime gameTime) => this.m_delayedProcessing.SyncUpdate();

    private void renderUpdate(GameTime time) => this.m_delayedProcessing.RenderUpdate();

    private void addTowerOrUpdateArea(IAreaManagingTower tower)
    {
      Assert.That<IAreaManagingTower>(tower).IsNotNull<IAreaManagingTower>();
      if (this.m_selectedTowerData.HasValue && tower == this.m_selectedTowerData.Value.Tower)
      {
        this.updateArea(this.m_selectedTowerData.Value);
      }
      else
      {
        foreach (TowerAreasRenderer.TowerAreaData towerData in this.m_towersData)
        {
          if (towerData.Tower == tower)
          {
            this.updateArea(towerData);
            return;
          }
        }
        this.addTower(tower);
      }
    }

    private void addTower(IAreaManagingTower tower)
    {
      TerrainRectSelection areaOutline = this.m_resolver.Instantiate<TerrainRectSelection>();
      areaOutline.SetParent(this.m_parentGo, false);
      areaOutline.SetArea(tower.Area, TowerAreasRenderer.AREA_LINE_COLOR.AsColor());
      areaOutline.Show();
      this.m_towersData.Add(new TowerAreasRenderer.TowerAreaData(tower, tower.Area, areaOutline));
    }

    private void updateArea(TowerAreasRenderer.TowerAreaData towerData)
    {
      towerData.Area = towerData.Tower.Area;
      towerData.AreaOutline.SetArea(towerData.Area, towerData == this.m_selectedTowerData ? TowerAreasRenderer.SELECTED_AREA_LINE_COLOR.AsColor() : TowerAreasRenderer.AREA_LINE_COLOR.AsColor());
    }

    private void removeTower(IAreaManagingTower tower)
    {
      Assert.That<IAreaManagingTower>(tower).IsNotNull<IAreaManagingTower>();
      int towerIndex = this.getTowerIndex(tower);
      if (towerIndex < 0)
      {
        Log.Error(string.Format("ITower {0} not found in the list of towers, cannot remove.", (object) tower));
      }
      else
      {
        this.m_towersData[towerIndex].AreaOutline.Destroy();
        this.m_towersData.RemoveAt(towerIndex);
      }
    }

    private int getTowerIndex(IAreaManagingTower tower)
    {
      return this.m_towersData.IndexOf<IAreaManagingTower>(tower, (Func<TowerAreasRenderer.TowerAreaData, IAreaManagingTower>) (x => x.Tower));
    }

    public IActivator CreateActivator() => this.m_activator.CreateActivator();

    public IActivator CreateCombinedActivatorWithTerrainDesignators()
    {
      return this.CreateActivator().Combine(this.m_terrainDesignationsRenderer.CreateActivator()).Combine(this.m_surfaceDesignationsRenderer.CreateActivator());
    }

    public IActivator CreateCombinedActivatorWithTerrainDesignatorsAndGrid()
    {
      return this.CreateActivator().Combine(this.m_terrainDesignationsRenderer.CreateActivatorCombinedWithTerrainGrid()).Combine(this.m_surfaceDesignationsRenderer.CreateActivatorCombinedWithTerrainGrid());
    }

    internal void ForceSetActive(bool isActive) => this.m_parentGo.SetActive(isActive);

    private void activate() => this.m_parentGo.SetActive(true);

    private void deactivate() => this.m_parentGo.SetActive(false);

    public void SelectTowerArea(Option<IAreaManagingTower> tower)
    {
      if (tower.IsNone)
      {
        if (this.m_selectedTowerData.IsNone)
          return;
        this.m_selectedTowerData.Value.AreaOutline.SetColor(TowerAreasRenderer.AREA_LINE_COLOR.AsColor());
        this.m_selectedTowerData = (Option<TowerAreasRenderer.TowerAreaData>) Option.None;
      }
      else
      {
        int towerIndex = this.getTowerIndex(tower.Value);
        if (towerIndex < 0)
        {
          Log.Error(string.Format("ITower {0} not found in the list of towers, cannot select.", (object) tower));
        }
        else
        {
          TowerAreasRenderer.TowerAreaData towerAreaData = this.m_towersData[towerIndex];
          towerAreaData.AreaOutline.SetColor(TowerAreasRenderer.SELECTED_AREA_LINE_COLOR.AsColor());
          this.m_selectedTowerData = (Option<TowerAreasRenderer.TowerAreaData>) towerAreaData;
        }
      }
    }

    static TowerAreasRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TowerAreasRenderer.AREA_LINE_COLOR = new ColorUniversal((ColorRgba) 12303291);
      TowerAreasRenderer.SELECTED_AREA_LINE_COLOR = new ColorUniversal((ColorRgba) 16772625);
    }

    private sealed class TowerAreaData
    {
      public readonly IAreaManagingTower Tower;
      public readonly TerrainRectSelection AreaOutline;

      public RectangleTerrainArea2i Area { get; set; }

      public TowerAreaData(
        IAreaManagingTower tower,
        RectangleTerrainArea2i area,
        TerrainRectSelection areaOutline)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Tower = tower;
        this.Area = area;
        this.AreaOutline = areaOutline;
      }
    }
  }
}
