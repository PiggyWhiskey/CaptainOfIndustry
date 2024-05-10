using Mafi;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Unity;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.InputControl.Inspectors.Vehicles;
using Mafi.Unity.UserInterface;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace TerrainTower.TTower
{
    internal abstract class EntityInspector<TEntity, TView> :
    IEntityInspector<TEntity>,
    IEntityInspector,
    IEntityInspectorFactory<TEntity>,
    IFactory<TEntity, IEntityInspector>
    where TEntity : class, IRenderedEntity, IEntity
    where TView : ItemDetailWindowView
    {
        private readonly Lyst<IRenderedEntity> m_secondaryHighlightedEntities;
        private readonly Lyst<IPathFindingVehicle> m_unreachableVehiclesCache;
        private readonly MultiLineOverlayRendererHelper m_unreachableVehiclesLineRenderer;
        private Option<IStaticEntityWithReservedOcean> m_activatedOverlayEntity;
        private Option<BuildingsAssigner> m_buildingsAssigner;
        private bool m_restoreHighlightAfterUpgrade;
        private TEntity m_selectedEntity;
        private TView m_windowView;

        protected EntityInspector(InspectorContext context) : base()
        {
            m_secondaryHighlightedEntities = new Lyst<IRenderedEntity>();
            m_unreachableVehiclesCache = new Lyst<IPathFindingVehicle>();
            Context = context;
            m_unreachableVehiclesLineRenderer = new MultiLineOverlayRendererHelper(context.LinesFactory);
        }

        public InspectorContext Context { get; }
        public virtual bool DeactivateOnNonUiClick => true;
        public IInputScheduler InputScheduler => Context.InputScheduler;

        /// <summary>
        /// Entity is null when this inspector is deactivated and not null when activated. We do not use option here
        /// because any time this class is used it has to be activated first.
        /// </summary>
        public TEntity SelectedEntity => m_selectedEntity;

        protected InspectorController Controller => Context.MainController;
        protected IUnityInputMgr InputMgr => Context.InputMgr;

        protected TView WindowView
        {
            get
            {
                if (m_windowView != null)
                    return m_windowView;
                m_windowView = GetView();
                m_windowView.SetOnCloseButtonClickAction(() => InputMgr.DeactivateController(Controller));
                m_windowView.BuildUi(Context.Builder);
                if (m_buildingsAssigner.HasValue)
                    WindowView.AddUpdater(m_buildingsAssigner.Value.Updater);
                return m_windowView;
            }
        }

        public void Activate()
        {
            Assert.That(SelectedEntity).IsNotNull();
            if (m_buildingsAssigner.HasValue)
                m_buildingsAssigner.Value.SetEntity(SelectedEntity);
            Context.Highlighter.Highlight(SelectedEntity, ColorRgba.Yellow);
            OnActivated();
            WindowView.Show();
        }

        public void Clear()
        {
            if (!((object)SelectedEntity is StaticEntity))
                return;
            WindowView.RenderUpdate(new GameTime());
        }

        public IEntityInspector Create(TEntity entity)
        {
            m_selectedEntity = entity.CheckNotNull();
            return this;
        }

        public void Deactivate()
        {
            if (m_buildingsAssigner.HasValue)
            {
                m_buildingsAssigner.Value.DeactivateTool();
                Controller.SetHoverCursorSuppression(false);
            }
            WindowView.Hide();
            OnDeactivated();
            Context.Highlighter.RemoveHighlight(SelectedEntity);
            RemoveSecondaryHighlight();
            Assert.That(Context.Highlighter.HighlightedCount).IsZero();
            m_selectedEntity = default;
        }

        public void EditInputBuildingsClicked()
        {
            Assert.That(m_buildingsAssigner).HasValue();
            if (m_buildingsAssigner.IsNone)
                return;
            Controller.SetHoverCursorSuppression(true);
            m_buildingsAssigner.Value.ActivateTool(() => InputMgr.DeactivateController(Controller), true);
            WindowView.Hide();
        }

        public void EditOutputBuildingsClicked()
        {
            Assert.That(m_buildingsAssigner).HasValue();
            if (m_buildingsAssigner.IsNone)
                return;
            Controller.SetHoverCursorSuppression(true);
            m_buildingsAssigner.Value.ActivateTool(() => InputMgr.DeactivateController(Controller), false);
            WindowView.Hide();
        }

        public virtual bool InputUpdate(IInputScheduler inputScheduler)
        {
            return !m_buildingsAssigner.IsNone && m_buildingsAssigner.Value.InputUpdate(inputScheduler);
        }

        public virtual void RenderUpdate(GameTime gameTime)
        {
            if (m_buildingsAssigner.HasValue)
                m_buildingsAssigner.Value.RenderUpdate();
            if (m_restoreHighlightAfterUpgrade)
            {
                Context.Highlighter.Highlight(SelectedEntity, ColorRgba.Yellow);
                m_restoreHighlightAfterUpgrade = false;
            }
            m_unreachableVehiclesLineRenderer.ClearLines();
            Tile3f? nullable = new Tile3f?();
            if (SelectedEntity is IStaticEntity selectedEntity2)
                nullable = new Tile3f?(selectedEntity2.Position3f);
            else if (SelectedEntity is Vehicle selectedEntity1)
                nullable = new Tile3f?(selectedEntity1.Position3f);
            if (SelectedEntity.IsEnabled && nullable.HasValue)
            {
                foreach (IPathFindingVehicle pathFindingVehicle in m_unreachableVehiclesCache)
                    m_unreachableVehiclesLineRenderer.AddLine(nullable.Value, pathFindingVehicle.Position3f, 0.6f, new Color(1f, 0.2f, 0.0f, 0.3f));
            }
            WindowView.RenderUpdate(gameTime);
        }

        public T ScheduleInputCmd<T>(T cmd) where T : IInputCommand
        {
            return InputScheduler.ScheduleInputCmd(cmd);
        }

        public virtual void SyncUpdate(GameTime gameTime)
        {
            Assert.That(SelectedEntity).IsNotNull();
            if (SelectedEntity.IsDestroyed)
            {
                ForceDeactivate();
            }
            else
            {
                Context.UnreachablesManager.GetVehiclesThatFailedToNavigateTo(SelectedEntity, m_unreachableVehiclesCache, true);
                WindowView.SyncUpdate(gameTime);
            }
        }

        protected IUiUpdater CreateVehiclesUpdater()
        {
            Assert.That(typeof(TEntity)).IsAssignableTo<IEntityAssignedWithVehicles>();
            return UpdaterBuilder.Start().Observe(() => ((IEntityAssignedWithVehicles)(object)SelectedEntity).AllSpawnedVehicles(), CompareFixedOrder<Vehicle>.Instance).Do(new Action<Lyst<Vehicle>>(HighlightSecondaryEntities)).Build(SyncFrequency.MoreThanSec);
        }

        protected void ForceDeactivate()
        {
            InputMgr.DeactivateController(Controller);
        }

        /// <summary>Called only once. Safe to create the window there.</summary>
        protected abstract TView GetView();

        protected void HighlightSecondaryEntities<T>(IEnumerable<T> entities) where T : IRenderedEntity
        {
            RemoveSecondaryHighlight();
            foreach (T entity in entities)
                m_secondaryHighlightedEntities.Add(entity);
            m_secondaryHighlightedEntities.ForEach(x => Context.Highlighter.Highlight(x, ColorRgba.Blue));
        }

        protected void HighlightSecondaryEntity(IRenderedEntity entity)
        {
            RemoveSecondaryHighlight();
            m_secondaryHighlightedEntities.Add(entity);
            m_secondaryHighlightedEntities.ForEach(x => Context.Highlighter.Highlight(x, ColorRgba.Blue));
        }

        protected virtual void OnActivated()
        {
            if (SelectedEntity is IStaticEntityWithReservedOcean selectedEntity)
            {
                m_activatedOverlayEntity = Context.OceanOverlayRenderer.ActivateForSingleEntity(selectedEntity);
            }
        }

        protected virtual void OnDeactivated()
        {
            if (m_activatedOverlayEntity.HasValue)
                m_activatedOverlayEntity = Context.OceanOverlayRenderer.DeactivateForSingleEntity(m_activatedOverlayEntity);
            m_unreachableVehiclesLineRenderer.ClearLines();
            m_unreachableVehiclesCache.Clear();
        }

        protected void RemoveSecondaryHighlight()
        {
            m_secondaryHighlightedEntities.ForEach(x => Context.Highlighter.RemoveHighlight(x));
            m_secondaryHighlightedEntities.Clear();
        }

        /// <summary>
        /// If you have a buildings assigner set it here and its lifecycle and control will be managed for you.
        /// </summary>
        protected void SetBuildingsAssigner(BuildingsAssigner buildingsAssigner)
        {
            m_buildingsAssigner = (Option<BuildingsAssigner>)buildingsAssigner;
        }
    }
}