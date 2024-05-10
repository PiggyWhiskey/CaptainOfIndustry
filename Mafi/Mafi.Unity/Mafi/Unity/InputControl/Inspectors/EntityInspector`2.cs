// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.EntityInspector`2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.Inspectors.Vehicles;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  internal abstract class EntityInspector<TEntity, TView> : 
    IEntityInspector<TEntity>,
    IEntityInspector,
    IEntityInspectorFactory<TEntity>,
    IFactory<TEntity, IEntityInspector>
    where TEntity : class, IRenderedEntity, IEntity
    where TView : ItemDetailWindowView
  {
    private Option<BuildingsAssigner> m_buildingsAssigner;
    private bool m_restoreHighlightAfterUpgrade;
    private Option<IStaticEntityWithReservedOcean> m_activatedOverlayEntity;
    private readonly Lyst<IRenderedEntity> m_secondaryHighlightedEntities;
    private TView m_windowView;
    private TEntity m_selectedEntity;
    private readonly MultiLineOverlayRendererHelper m_unreachableVehiclesLineRenderer;
    private readonly Lyst<IPathFindingVehicle> m_unreachableVehiclesCache;

    public IInputScheduler InputScheduler => this.Context.InputScheduler;

    protected IUnityInputMgr InputMgr => this.Context.InputMgr;

    protected InspectorController Controller => this.Context.MainController;

    protected TView WindowView
    {
      get
      {
        if ((object) this.m_windowView != null)
          return this.m_windowView;
        this.m_windowView = this.GetView();
        this.m_windowView.SetOnCloseButtonClickAction((Action) (() => this.InputMgr.DeactivateController((IUnityInputController) this.Controller)));
        this.m_windowView.BuildUi(this.Context.Builder);
        if (this.m_buildingsAssigner.HasValue)
          this.WindowView.AddUpdater(this.m_buildingsAssigner.Value.Updater);
        return this.m_windowView;
      }
    }

    /// <summary>
    /// Entity is null when this inspector is deactivated and not null when activated. We do not use option here
    /// because any time this class is used it has to be activated first.
    /// </summary>
    public TEntity SelectedEntity => this.m_selectedEntity;

    public InspectorContext Context { get; }

    public virtual bool DeactivateOnNonUiClick => true;

    protected EntityInspector(InspectorContext context)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_secondaryHighlightedEntities = new Lyst<IRenderedEntity>();
      this.m_unreachableVehiclesCache = new Lyst<IPathFindingVehicle>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Context = context;
      this.m_unreachableVehiclesLineRenderer = new MultiLineOverlayRendererHelper(context.LinesFactory);
    }

    public IEntityInspector Create(TEntity entity)
    {
      this.m_selectedEntity = entity.CheckNotNull<TEntity>();
      return (IEntityInspector) this;
    }

    public void Activate()
    {
      Assert.That<TEntity>(this.SelectedEntity).IsNotNull<TEntity>();
      if (this.m_buildingsAssigner.HasValue)
        this.m_buildingsAssigner.Value.SetEntity((IEntity) this.SelectedEntity);
      this.Context.Highlighter.Highlight((IRenderedEntity) this.SelectedEntity, ColorRgba.Yellow);
      this.OnActivated();
      this.WindowView.Show();
    }

    public void Deactivate()
    {
      if (this.m_buildingsAssigner.HasValue)
      {
        this.m_buildingsAssigner.Value.DeactivateTool();
        this.Controller.SetHoverCursorSuppression(false);
      }
      this.WindowView.Hide();
      this.OnDeactivated();
      this.Context.Highlighter.RemoveHighlight((IRenderedEntity) this.SelectedEntity);
      this.RemoveSecondaryHighlight();
      Assert.That<int>(this.Context.Highlighter.HighlightedCount).IsZero();
      this.m_selectedEntity = default (TEntity);
    }

    protected void ForceDeactivate()
    {
      this.InputMgr.DeactivateController((IUnityInputController) this.Controller);
    }

    protected void HighlightSecondaryEntity(IRenderedEntity entity)
    {
      this.RemoveSecondaryHighlight();
      this.m_secondaryHighlightedEntities.Add(entity);
      this.m_secondaryHighlightedEntities.ForEach((Action<IRenderedEntity>) (x => this.Context.Highlighter.Highlight(x, ColorRgba.Blue)));
    }

    protected void HighlightSecondaryEntities<T>(IEnumerable<T> entities) where T : IRenderedEntity
    {
      this.RemoveSecondaryHighlight();
      foreach (T entity in entities)
        this.m_secondaryHighlightedEntities.Add((IRenderedEntity) entity);
      this.m_secondaryHighlightedEntities.ForEach((Action<IRenderedEntity>) (x => this.Context.Highlighter.Highlight(x, ColorRgba.Blue)));
    }

    protected void RemoveSecondaryHighlight()
    {
      this.m_secondaryHighlightedEntities.ForEach((Action<IRenderedEntity>) (x => this.Context.Highlighter.RemoveHighlight(x)));
      this.m_secondaryHighlightedEntities.Clear();
    }

    public virtual void SyncUpdate(GameTime gameTime)
    {
      Assert.That<TEntity>(this.SelectedEntity).IsNotNull<TEntity>();
      if (this.SelectedEntity.IsDestroyed)
      {
        this.ForceDeactivate();
      }
      else
      {
        this.Context.UnreachablesManager.GetVehiclesThatFailedToNavigateTo((IEntity) this.SelectedEntity, this.m_unreachableVehiclesCache, true);
        this.WindowView.SyncUpdate(gameTime);
      }
    }

    public virtual void RenderUpdate(GameTime gameTime)
    {
      if (this.m_buildingsAssigner.HasValue)
        this.m_buildingsAssigner.Value.RenderUpdate();
      if (this.m_restoreHighlightAfterUpgrade)
      {
        this.Context.Highlighter.Highlight((IRenderedEntity) this.SelectedEntity, ColorRgba.Yellow);
        this.m_restoreHighlightAfterUpgrade = false;
      }
      this.m_unreachableVehiclesLineRenderer.ClearLines();
      Tile3f? nullable = new Tile3f?();
      if (this.SelectedEntity is IStaticEntity selectedEntity2)
        nullable = new Tile3f?(selectedEntity2.Position3f);
      else if (this.SelectedEntity is Mafi.Core.Entities.Dynamic.Vehicle selectedEntity1)
        nullable = new Tile3f?(selectedEntity1.Position3f);
      if (this.SelectedEntity.IsEnabled && nullable.HasValue)
      {
        foreach (IPathFindingVehicle pathFindingVehicle in this.m_unreachableVehiclesCache)
          this.m_unreachableVehiclesLineRenderer.AddLine(nullable.Value, pathFindingVehicle.Position3f, 0.6f, new Color(1f, 0.2f, 0.0f, 0.3f));
      }
      this.WindowView.RenderUpdate(gameTime);
    }

    public virtual bool InputUpdate(IInputScheduler inputScheduler)
    {
      return !this.m_buildingsAssigner.IsNone && this.m_buildingsAssigner.Value.InputUpdate(inputScheduler);
    }

    public T ScheduleInputCmd<T>(T cmd) where T : IInputCommand
    {
      return this.InputScheduler.ScheduleInputCmd<T>(cmd);
    }

    public void Clear()
    {
      if (!((object) this.SelectedEntity is StaticEntity))
        return;
      this.WindowView.RenderUpdate(new GameTime());
    }

    /// <summary>
    /// If you have a buildings assigner set it here and its lifecycle and control will be managed for you.
    /// </summary>
    protected void SetBuildingsAssigner(BuildingsAssigner buildingsAssigner)
    {
      this.m_buildingsAssigner = (Option<BuildingsAssigner>) buildingsAssigner;
    }

    public void EditInputBuildingsClicked()
    {
      Assert.That<Option<BuildingsAssigner>>(this.m_buildingsAssigner).HasValue<BuildingsAssigner>();
      if (this.m_buildingsAssigner.IsNone)
        return;
      this.Controller.SetHoverCursorSuppression(true);
      this.m_buildingsAssigner.Value.ActivateTool((Action) (() => this.InputMgr.DeactivateController((IUnityInputController) this.Controller)), true);
      this.WindowView.Hide();
    }

    public void EditOutputBuildingsClicked()
    {
      Assert.That<Option<BuildingsAssigner>>(this.m_buildingsAssigner).HasValue<BuildingsAssigner>();
      if (this.m_buildingsAssigner.IsNone)
        return;
      this.Controller.SetHoverCursorSuppression(true);
      this.m_buildingsAssigner.Value.ActivateTool((Action) (() => this.InputMgr.DeactivateController((IUnityInputController) this.Controller)), false);
      this.WindowView.Hide();
    }

    protected IUiUpdater CreateVehiclesUpdater()
    {
      Assert.That<System.Type>(typeof (TEntity)).IsAssignableTo<IEntityAssignedWithVehicles>();
      return UpdaterBuilder.Start().Observe<Mafi.Core.Entities.Dynamic.Vehicle>((Func<IIndexable<Mafi.Core.Entities.Dynamic.Vehicle>>) (() => ((IEntityAssignedWithVehicles) (object) this.SelectedEntity).AllSpawnedVehicles()), (ICollectionComparator<Mafi.Core.Entities.Dynamic.Vehicle, IIndexable<Mafi.Core.Entities.Dynamic.Vehicle>>) CompareFixedOrder<Mafi.Core.Entities.Dynamic.Vehicle>.Instance).Do(new Action<Lyst<Mafi.Core.Entities.Dynamic.Vehicle>>(this.HighlightSecondaryEntities<Mafi.Core.Entities.Dynamic.Vehicle>)).Build(SyncFrequency.MoreThanSec);
    }

    /// <summary>Called only once. Safe to create the window there.</summary>
    protected abstract TView GetView();

    protected virtual void OnActivated()
    {
      if (!(this.SelectedEntity is IStaticEntityWithReservedOcean selectedEntity))
        return;
      this.m_activatedOverlayEntity = this.Context.OceanOverlayRenderer.ActivateForSingleEntity(selectedEntity);
    }

    protected virtual void OnDeactivated()
    {
      if (this.m_activatedOverlayEntity.HasValue)
        this.m_activatedOverlayEntity = this.Context.OceanOverlayRenderer.DeactivateForSingleEntity(this.m_activatedOverlayEntity);
      this.m_unreachableVehiclesLineRenderer.ClearLines();
      this.m_unreachableVehiclesCache.Clear();
    }
  }
}
