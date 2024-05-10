// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.ForestryTowerInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Buildings.Forestry;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Vehicles.TreeHarvesters;
using Mafi.Core.Vehicles.TreePlanters;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.Mine;
using Mafi.Unity.Terrain;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.Utils;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ForestryTowerInspector : EntityInspector<ForestryTower, ForestryTowerWindowView>
  {
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly TowerAreasRenderer m_towerAreasRenderer;
    private readonly TerrainRectSelection m_terrainOutlineRenderer;
    private readonly IActivator m_towerAreasAndDesignatorsActivator;
    private readonly AreaSelectionTool m_areaSelectionTool;
    private readonly ForestryDesignationController m_designationController;
    private readonly IUnityInputMgr m_unityInputManager;
    public bool AreaEditInProgress;
    public bool DesignationEditInProgress;
    /// <summary>
    /// Area highlighted while <see cref="F:Mafi.Unity.InputControl.Inspectors.Buildings.ForestryTowerInspector.AreaEditInProgress" /> is true, but user is not selecting the new area using
    /// dragging yet.
    /// </summary>
    private RectangleTerrainArea2i? m_highlightedArea;
    private readonly ImmutableArray<TreePlantingGroupProto> m_allTrees;
    private readonly VehiclesAssignerView<TreePlanterProto> m_treePlanterAssigner;
    private readonly VehiclesAssignerView<TreeHarvesterProto> m_treeHarvesterAssigner;

    public ForestryTowerInspector(
      ShortcutsManager shortcutsManager,
      InspectorContext inspectorContext,
      UnlockedProtosDb unlockedProtosDb,
      ProtosDb protosDb,
      TowerAreasRenderer towerAreasRenderer,
      BuildingsAssigner buildingsAssigner,
      VehiclesAssignerFactory vehiclesAssignerFactory,
      TerrainRectSelection terrainOutlineRenderer,
      AreaSelectionToolFactory areaToolFactory,
      ForestryDesignationController designationController,
      IUnityInputMgr unityInputManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_areaSelectionTool = areaToolFactory.CreateInstance((Action<RectangleTerrainArea2i, bool>) ((x, y) => { }), new Action<RectangleTerrainArea2i, bool>(this.selectionDone), new Action(this.deactivateAreaEditing), (Action) (() => { }));
      this.m_designationController = designationController;
      this.m_unityInputManager = unityInputManager;
      this.SetBuildingsAssigner(buildingsAssigner);
      this.m_treePlanterAssigner = vehiclesAssignerFactory.CreateNewView<TreePlanterProto>((Func<IEntityAssignedWithVehicles>) (() => (IEntityAssignedWithVehicles) this.SelectedEntity));
      this.m_treeHarvesterAssigner = vehiclesAssignerFactory.CreateNewView<TreeHarvesterProto>((Func<IEntityAssignedWithVehicles>) (() => (IEntityAssignedWithVehicles) this.SelectedEntity));
      this.m_allTrees = protosDb.All<TreePlantingGroupProto>().ToImmutableArray<TreePlantingGroupProto>();
      this.m_shortcutsManager = shortcutsManager;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_towerAreasRenderer = towerAreasRenderer;
      this.m_terrainOutlineRenderer = terrainOutlineRenderer;
      this.m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignators();
    }

    private void selectionDone(RectangleTerrainArea2i area, bool leftClick)
    {
      this.InputScheduler.ScheduleInputCmd<ForestryTowerAreaChangeCmd>(new ForestryTowerAreaChangeCmd(this.SelectedEntity.Id, area));
      Assert.That<bool>(this.AreaEditInProgress).IsTrue();
      this.ToggleAreaEditing();
    }

    protected override ForestryTowerWindowView GetView()
    {
      ForestryTowerWindowView view = new ForestryTowerWindowView(this, this.m_treePlanterAssigner, this.m_treeHarvesterAssigner, this.m_unlockedProtosDb, this.m_allTrees);
      view.AddUpdater(this.CreateVehiclesUpdater());
      return view;
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.m_areaSelectionTool.IsActive)
        return true;
      if (this.m_designationController.IsActive)
        return false;
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
      else if (this.DesignationEditInProgress)
      {
        this.DesignationEditInProgress = false;
        this.WindowView.Show();
        return true;
      }
      return base.InputUpdate(inputScheduler);
    }

    protected override void OnActivated()
    {
      base.OnActivated();
      this.WindowView.SetTreeProtoFromInspector((IProtoWithIconAndName) this.m_allTrees.First);
      this.m_towerAreasRenderer.SelectTowerArea((Option<IAreaManagingTower>) (IAreaManagingTower) this.SelectedEntity);
      this.m_towerAreasAndDesignatorsActivator.Activate();
    }

    protected override void OnDeactivated()
    {
      base.OnDeactivated();
      this.m_towerAreasAndDesignatorsActivator.Deactivate();
      this.m_towerAreasRenderer.SelectTowerArea((Option<IAreaManagingTower>) Option.None);
      this.deactivateDesignationEditing();
      this.deactivateAreaEditing();
    }

    public void ToggleAreaEditing()
    {
      if (this.AreaEditInProgress)
      {
        this.deactivateAreaEditing();
        this.WindowView.Show();
      }
      else
      {
        this.m_areaSelectionTool.TerrainCursor.Activate();
        this.WindowView.Hide();
        this.AreaEditInProgress = true;
      }
    }

    public void ToggleDesignationEditing()
    {
      if (this.DesignationEditInProgress)
      {
        this.deactivateDesignationEditing();
        this.WindowView.Show();
      }
      else
      {
        this.m_unityInputManager.ActivateNewController((IUnityInputController) this.m_designationController);
        this.WindowView.Hide();
        this.DesignationEditInProgress = true;
      }
    }

    private void deactivateDesignationEditing()
    {
      if (!this.DesignationEditInProgress)
        return;
      this.m_unityInputManager.DeactivateController((IUnityInputController) this.m_designationController);
      this.DesignationEditInProgress = false;
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

    public void SetTreePlantingGroupProto(Option<TreePlantingGroupProto> product)
    {
      if (!product.HasValue)
        return;
      this.InputScheduler.ScheduleInputCmd<ForestryTowerSetTreeProtoCmd>(new ForestryTowerSetTreeProtoCmd(this.SelectedEntity, product.Value));
    }

    public void SetCutPercentage(Percent newValue)
    {
      this.InputScheduler.ScheduleInputCmd<ForestryTowerSetCutPercentageCmd>(new ForestryTowerSetCutPercentageCmd(this.SelectedEntity, newValue));
    }
  }
}
