using Mafi;
using Mafi.Core.Buildings.Towers;
using Mafi.Core.Input;
using Mafi.Core.Terrain;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.Mine;
using Mafi.Unity.Terrain;
using Mafi.Unity.Utils;

using System;

namespace TerrainTower.TTower
{
    [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
    internal class TerrainTowerInspector : EntityInspector<TerrainTowerEntity, TerrainTowerWindowView>
    {
        public bool AreaEditInProgress;
        private readonly AreaSelectionTool m_areaSelectionTool;

        private readonly ShortcutsManager m_shortcutsManager;

        private readonly TerrainRectSelection m_terrainOutlineRenderer;
        private readonly IActivator m_towerAreasAndDesignatorsActivator;
        private readonly TowerAreasRenderer m_towerAreasRenderer;
        private readonly TerrainTowerWindowView m_windowView;
        private RectangleTerrainArea2i? m_highlightedArea;

        public TerrainTowerInspector(
            ShortcutsManager shortcutsManager,
            TowerAreasRenderer towerAreasRenderer,
            InspectorContext inspectorContext,
            TerrainRectSelection terrainOutlineRenderer,
            AreaSelectionToolFactory areaToolFactory
            ) : base(inspectorContext)
        {
            //m_inputScheduler = inputScheduler;
            m_windowView = new TerrainTowerWindowView(this);
            m_shortcutsManager = shortcutsManager;
            m_towerAreasRenderer = towerAreasRenderer;
            m_terrainOutlineRenderer = terrainOutlineRenderer;
            m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
            m_areaSelectionTool = areaToolFactory
                .CreateInstance
                (
                    (x, y) => { },
                    new Action<RectangleTerrainArea2i, bool>(selectionDone),
                    new Action(deactivateAreaEditing),
                    () => { }
                );
        }

        public override bool InputUpdate(IInputScheduler inputScheduler)
        {
            if (m_areaSelectionTool.IsActive) return true;

            if (AreaEditInProgress)
            {
                if (m_shortcutsManager.IsPrimaryActionDown)
                {
                    m_terrainOutlineRenderer.Hide();
                    m_highlightedArea = new RectangleTerrainArea2i?();
                    m_areaSelectionTool.SetEdgeSizeLimit(new RelTile1i(int.MaxValue));
                    m_areaSelectionTool.Activate(true);
                    return true;
                }
                if (m_areaSelectionTool.TerrainCursor.HasValue)
                {
                    RectangleTerrainArea2i area = new RectangleTerrainArea2i(m_areaSelectionTool.TerrainCursor.Tile2i, RelTile2i.One);
                    RectangleTerrainArea2i rectangleTerrainArea2i = area;
                    RectangleTerrainArea2i? highlightedArea = m_highlightedArea;
                    if (!highlightedArea.HasValue || rectangleTerrainArea2i != highlightedArea.GetValueOrDefault())
                    {
                        m_terrainOutlineRenderer.SetArea(area, AreaSelectionTool.SELECT_COLOR);
                        m_highlightedArea = new RectangleTerrainArea2i?(area);
                    }
                }
            }
            return base.InputUpdate(inputScheduler);
        }

        internal void ToggleAreaEditing()
        {
            if (AreaEditInProgress)
            {
                deactivateAreaEditing();
                m_windowView.Show();
            }
            else
            {
                m_areaSelectionTool.TerrainCursor.Activate();
                m_windowView.Hide();
                AreaEditInProgress = true;
            }
        }

        protected override TerrainTowerWindowView GetView() => m_windowView;

        protected override void OnActivated()
        {
            base.OnActivated();
            Assert.AssertTrue(m_towerAreasRenderer != null, "Tower Areas Renderer is NULL");
            Assert.AssertTrue(m_towerAreasAndDesignatorsActivator != null, "Tower Areas and Designators Activator is NULL");
            Assert.AssertTrue(SelectedEntity != null, "Selected Entity is NULL");
            m_towerAreasRenderer.SelectTowerArea((Option<IAreaManagingTower>)SelectedEntity);
            m_towerAreasAndDesignatorsActivator.Activate();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            m_towerAreasAndDesignatorsActivator.Deactivate();
            m_towerAreasRenderer.SelectTowerArea((Option<IAreaManagingTower>)Option.None);
            deactivateAreaEditing();
        }

        private void deactivateAreaEditing()
        {
            if (AreaEditInProgress)
            {
                m_areaSelectionTool.Deactivate();
                m_areaSelectionTool.TerrainCursor.Deactivate();
                m_terrainOutlineRenderer.Hide();
                m_highlightedArea = new RectangleTerrainArea2i?();
                AreaEditInProgress = false;
            }
        }

        private void selectionDone(RectangleTerrainArea2i area, bool leftClick)
        {
            SelectedEntity.EditManagedArea(area);
            ToggleAreaEditing();
        }
    }
}