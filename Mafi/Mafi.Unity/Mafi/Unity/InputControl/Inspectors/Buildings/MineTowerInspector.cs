// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.MineTowerInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Mine;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Vehicles.Excavators;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.Mine;
using Mafi.Unity.Terrain;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.Utils;
using System;
using System.Linq;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class MineTowerInspector : EntityInspector<MineTower, MineTowerWindowView>
  {
    private readonly MineTowerWindowView m_view;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly TowerAreasRenderer m_towerAreasRenderer;
    private readonly TerrainRectSelection m_terrainOutlineRenderer;
    private readonly IActivator m_towerAreasAndDesignatorsActivator;
    private readonly AreaSelectionTool m_areaSelectionTool;
    public bool AreaEditInProgress;
    /// <summary>
    /// Area highlighted while <see cref="F:Mafi.Unity.InputControl.Inspectors.Buildings.MineTowerInspector.AreaEditInProgress" /> is true, but user is not selecting the new area using
    /// dragging yet.
    /// </summary>
    private RectangleTerrainArea2i? m_highlightedArea;

    public MineTowerInspector(
      ShortcutsManager shortcutsManager,
      InspectorContext inspectorContext,
      ProtosDb protosDb,
      TowerAreasRenderer towerAreasRenderer,
      BuildingsAssigner buildingsAssigner,
      VehiclesAssignerFactory vehiclesAssignerFactory,
      TerrainRectSelection terrainOutlineRenderer,
      AreaSelectionToolFactory areaToolFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_areaSelectionTool = areaToolFactory.CreateInstance((Action<RectangleTerrainArea2i, bool>) ((x, y) => { }), new Action<RectangleTerrainArea2i, bool>(this.selectionDone), new Action(this.deactivateAreaEditing), (Action) (() => { }));
      this.SetBuildingsAssigner(buildingsAssigner);
      VehiclesAssignerView<ExcavatorProto> newView1 = vehiclesAssignerFactory.CreateNewView<ExcavatorProto>((Func<IEntityAssignedWithVehicles>) (() => (IEntityAssignedWithVehicles) this.SelectedEntity));
      VehiclesAssignerView<TruckProto> newView2 = vehiclesAssignerFactory.CreateNewView<TruckProto>((Func<IEntityAssignedWithVehicles>) (() => (IEntityAssignedWithVehicles) this.SelectedEntity));
      ImmutableArray<ProductProto> immutableArray = protosDb.All<LooseProductProto>().Where<LooseProductProto>((Func<LooseProductProto, bool>) (x => x.CanBeOnTerrain)).Cast<ProductProto>().ToImmutableArray<ProductProto>();
      this.m_shortcutsManager = shortcutsManager;
      this.m_towerAreasRenderer = towerAreasRenderer;
      this.m_terrainOutlineRenderer = terrainOutlineRenderer;
      this.m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
      this.m_view = new MineTowerWindowView(this, newView1, newView2, immutableArray);
      this.m_view.AddUpdater(this.CreateVehiclesUpdater());
    }

    private void selectionDone(RectangleTerrainArea2i area, bool leftClick)
    {
      this.InputScheduler.ScheduleInputCmd<MineTowerAreaChangeCmd>(new MineTowerAreaChangeCmd(this.SelectedEntity.Id, area));
      Assert.That<bool>(this.AreaEditInProgress).IsTrue();
      this.ToggleAreaEditing();
    }

    protected override MineTowerWindowView GetView() => this.m_view;

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.m_areaSelectionTool.IsActive)
        return true;
      if (this.AreaEditInProgress)
      {
        if (this.m_shortcutsManager.IsPrimaryActionDown)
        {
          this.m_terrainOutlineRenderer.Hide();
          this.m_highlightedArea = new RectangleTerrainArea2i?();
          this.m_areaSelectionTool.SetEdgeSizeLimit(this.SelectedEntity.Prototype.Area.MaxAreaEdgeSize);
          this.m_areaSelectionTool.Activate(true);
          return true;
        }
        if (this.m_areaSelectionTool.TerrainCursor.HasValue)
        {
          RectangleTerrainArea2i area = new RectangleTerrainArea2i(this.m_areaSelectionTool.TerrainCursor.Tile2i, RelTile2i.One);
          RectangleTerrainArea2i rectangleTerrainArea2i = area;
          RectangleTerrainArea2i? highlightedArea = this.m_highlightedArea;
          if ((highlightedArea.HasValue ? (rectangleTerrainArea2i != highlightedArea.GetValueOrDefault() ? 1 : 0) : 1) != 0)
          {
            this.m_terrainOutlineRenderer.SetArea(area, AreaSelectionTool.SELECT_COLOR);
            this.m_highlightedArea = new RectangleTerrainArea2i?(area);
          }
        }
      }
      return base.InputUpdate(inputScheduler);
    }

    protected override void OnActivated()
    {
      base.OnActivated();
      this.m_towerAreasRenderer.SelectTowerArea((Option<IAreaManagingTower>) (IAreaManagingTower) this.SelectedEntity);
      this.m_towerAreasAndDesignatorsActivator.Activate();
    }

    protected override void OnDeactivated()
    {
      base.OnDeactivated();
      this.m_towerAreasAndDesignatorsActivator.Deactivate();
      this.m_towerAreasRenderer.SelectTowerArea((Option<IAreaManagingTower>) Option.None);
      this.deactivateAreaEditing();
    }

    public void ToggleAreaEditing()
    {
      if (this.AreaEditInProgress)
      {
        this.deactivateAreaEditing();
        this.m_view.Show();
      }
      else
      {
        this.m_areaSelectionTool.TerrainCursor.Activate();
        this.m_view.Hide();
        this.AreaEditInProgress = true;
      }
    }

    private void deactivateAreaEditing()
    {
      if (!this.AreaEditInProgress)
        return;
      this.m_areaSelectionTool.Deactivate();
      this.m_areaSelectionTool.TerrainCursor.Deactivate();
      this.m_terrainOutlineRenderer.Hide();
      this.m_highlightedArea = new RectangleTerrainArea2i?();
      this.AreaEditInProgress = false;
    }

    public void AddDumpableProduct(ProductProto product)
    {
      this.InputScheduler.ScheduleInputCmd<AddProductToDumpCmd>(new AddProductToDumpCmd((Option<MineTower>) this.SelectedEntity, product));
    }

    public void RemoveDumpableProduct(ProductProto product)
    {
      this.InputScheduler.ScheduleInputCmd<RemoveProductToDumpCmd>(new RemoveProductToDumpCmd((Option<MineTower>) this.SelectedEntity, product));
    }
  }
}
