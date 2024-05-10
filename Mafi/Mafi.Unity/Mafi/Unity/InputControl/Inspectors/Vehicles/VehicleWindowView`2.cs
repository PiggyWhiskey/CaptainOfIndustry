// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.VehicleWindowView`2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.PathFinding.Goals;
using Mafi.Core.Syncers;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  internal abstract class VehicleWindowView<T, TView> : EntityInspectorBase<T>
    where T : Mafi.Core.Entities.Dynamic.Vehicle
    where TView : VehicleWindowView<T, TView>
  {
    private readonly VehicleInspector<T, TView> m_controller;
    private readonly OrbitalCameraModel m_orbitalCameraModel;
    private readonly MbBasedEntitiesRenderer m_entitiesRenderer;
    private readonly Lyst<IEntity> m_unreachableEntitiesForRender;

    protected VehicleWindowView(
      VehicleInspector<T, TView> controller,
      OrbitalCameraModel orbitalCameraModel,
      MbBasedEntitiesRenderer entitiesRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_unreachableEntitiesForRender = new Lyst<IEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector((IEntityInspector) controller);
      this.m_orbitalCameraModel = orbitalCameraModel;
      this.m_entitiesRenderer = entitiesRenderer;
      this.m_controller = controller.CheckNotNull<VehicleInspector<T, TView>>();
      this.OnHide += new Action(this.onHide);
    }

    protected override void AddCustomItems(StackContainer itemContainer)
    {
      this.AddFollowVehicleButton(this.m_entitiesRenderer, this.m_orbitalCameraModel, (Func<IRenderedEntity>) (() => (IRenderedEntity) this.Entity));
      base.AddCustomItems(itemContainer);
      this.AddNavigationOverlayPanel(this.m_controller.NavOverlayRenderer, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.Entity));
      UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
      AlertIndicator alertIndicator = this.GetOrAddAlertIndicator(this.m_controller.Context);
      alertIndicator.SetIcon("Assets/Unity/UserInterface/General/Speed.svg");
      alertIndicator.HideTooltipIcon();
      updaterBuilder.Observe<LocStrFormatted>((Func<LocStrFormatted>) (() => this.Entity.GetSlowDownMessageForUi())).Do((Action<LocStrFormatted>) (msg =>
      {
        bool isNotEmpty = msg.IsNotEmpty;
        this.SetAlertIndicatorVisibility(alertIndicator, isNotEmpty);
        if (!isNotEmpty)
          return;
        alertIndicator.SetMessage(msg.Value);
      }));
      updaterBuilder.Observe<IVehicleGoal>((Func<IVehicleGoal>) (() => !this.Entity.IsNavigating ? (IVehicleGoal) null : this.Entity.NavigationGoal.ValueOrNull)).Do((Action<IVehicleGoal>) (goal =>
      {
        if (goal != null)
          this.m_controller.GoalLineRenderer.ShowLine(this.Entity.Position3f, goal.GetGoalPosition());
        else
          this.m_controller.GoalLineRenderer.HideLine();
      }));
      updaterBuilder.Observe<IDesignation>((Func<IReadOnlyCollection<IDesignation>>) (() => (IReadOnlyCollection<IDesignation>) this.m_controller.UnreachableDesignationsManager.GetUnreachableDesignationsFor((IPathFindingVehicle) this.Entity)), (ICollectionComparator<IDesignation, IReadOnlyCollection<IDesignation>>) CompareByCount<IDesignation>.Instance).Observe<bool>((Func<bool>) (() => showUnreachableDesignations(this.Entity))).ObserveNoCompare<Tile3f>((Func<Tile3f>) (() => this.Entity.Position3f)).Do((Action<Lyst<IDesignation>, bool, Tile3f>) ((designations, showLines, position) =>
      {
        this.m_controller.UnreachableDesignationsLineRenderer.ClearLines();
        if (showLines && designations.IsNotEmpty)
        {
          foreach (IDesignation designation in designations)
            this.m_controller.UnreachableDesignationsLineRenderer.AddLine(position, designation.CenterTileCoord.CenterTile2f.ExtendHeight(designation.GetTargetHeightAt(designation.CenterTileCoord)), 0.6f, new Color(1f, 0.2f, 0.0f, 0.3f));
          this.m_controller.DesignationsActivator.ActivateIfNotActive();
        }
        else
          this.m_controller.DesignationsActivator.DeactivateIfActive();
      }));
      updaterBuilder.Observe<IEntity>((Func<IReadOnlyCollection<IEntity>>) (() => (IReadOnlyCollection<IEntity>) this.m_controller.UnreachableDesignationsManager.GetUnreachableEntitiesFor((IPathFindingVehicle) this.Entity)), (ICollectionComparator<IEntity, IReadOnlyCollection<IEntity>>) CompareByCount<IEntity>.Instance).Observe<bool>((Func<bool>) (() => showUnreachableEntities(this.Entity))).Do((Action<Lyst<IEntity>, bool>) ((entities, showLines) =>
      {
        this.m_unreachableEntitiesForRender.Clear();
        if (!showLines)
          return;
        this.m_unreachableEntitiesForRender.AddRange(entities);
      }));
      this.AddUpdater(updaterBuilder.Build());

      static bool showUnreachableDesignations(T entity)
      {
        if (entity.IsDriving || !entity.IsEnabled)
          return false;
        return entity.NavigationFailedStreak > 0 || !(entity is Truck truck) || truck.Cargo.IsNotEmpty;
      }

      static bool showUnreachableEntities(T entity)
      {
        if (entity.IsDriving || !entity.IsEnabled)
          return false;
        int navigationFailedStreak = entity.NavigationFailedStreak;
        return true;
      }
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      this.m_controller.UnreachableEntitiesLineRenderer.ClearLines();
      foreach (IEntity entity in this.m_unreachableEntitiesForRender)
      {
        if (entity is IStaticEntity staticEntity)
          this.m_controller.UnreachableEntitiesLineRenderer.AddLine(this.Entity.Position3f, staticEntity.Position3f, 0.6f, new Color(1f, 0.2f, 0.0f, 0.3f));
        else if (entity is Mafi.Core.Entities.Dynamic.Vehicle vehicle)
          this.m_controller.UnreachableEntitiesLineRenderer.AddLine(this.Entity.Position3f, vehicle.Position3f, 0.6f, new Color(1f, 0.2f, 0.0f, 0.3f));
      }
    }

    private void onHide() => this.m_controller.UnreachableEntitiesLineRenderer.ClearLines();

    protected void AddVehicleButtons(UpdaterBuilder updaterBuilder, bool doNotAddReplaceBtn = false)
    {
      StackContainer container = this.AddVehicleButtonsSection(this.ItemsContainer);
      this.AddScrapVehicleBtn(container, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.m_controller.SelectedEntity), this.m_controller.VehiclesManager, this.m_controller.InputScheduler);
      this.AddRecoverVehicleBtn(container, updaterBuilder, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.m_controller.SelectedEntity), this.m_controller.VehiclesManager, this.m_controller.InputScheduler);
      this.AddVehicleGoToBtn(container, new Action(this.m_controller.StartGoToMode));
      if (doNotAddReplaceBtn)
        return;
      this.AddReplaceVehicleBtn(container, (Func<Mafi.Core.Entities.Dynamic.Vehicle>) (() => (Mafi.Core.Entities.Dynamic.Vehicle) this.m_controller.SelectedEntity), this.m_controller.Context);
    }
  }
}
