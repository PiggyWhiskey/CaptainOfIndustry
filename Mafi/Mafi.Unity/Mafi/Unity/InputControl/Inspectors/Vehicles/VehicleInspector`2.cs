// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.VehicleInspector`2
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Core.Vehicles.Commands;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using Mafi.Unity.Vehicles;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  internal abstract class VehicleInspector<TEntity, TView> : EntityInspector<TEntity, TView>
    where TEntity : Mafi.Core.Entities.Dynamic.Vehicle
    where TView : ItemDetailWindowView
  {
    public readonly IActivator DesignationsActivator;
    private readonly TerrainCursor m_terrainCursor;
    private readonly CursorPickingManager m_pickingManager;
    private readonly CursorManager m_cursorManager;
    private ShortcutsManager m_shortcutsManager;
    private Cursoor m_goToCursor;
    private bool m_commandModeInProgress;
    private AudioSource m_invalidSound;
    private AudioSource m_goToSound;
    private readonly MbBasedEntitiesRenderer m_entitiesRenderer;
    private Option<IRenderedEntity> m_highlightedAssignee;
    private Option<Transform> m_renderedEntityTransform;

    public IVehiclesManager VehiclesManager { get; }

    public LineOverlayRendererHelper GoalLineRenderer { get; }

    public VehiclesPathabilityOverlayRenderer NavOverlayRenderer { get; }

    public MultiLineOverlayRendererHelper UnreachableDesignationsLineRenderer { get; }

    public MultiLineOverlayRendererHelper UnreachableEntitiesLineRenderer { get; }

    public UnreachableTerrainDesignationsManager UnreachableDesignationsManager { get; }

    protected VehicleInspector(
      InspectorContext context,
      LinesFactory linesFactory,
      TerrainCursor terrainCursor,
      CursorPickingManager pickingManager,
      CursorManager cursorManager,
      MbBasedEntitiesRenderer entitiesRenderer,
      VehiclesPathabilityOverlayRenderer navOverlayRenderer,
      UnreachableTerrainDesignationsManager unreachableDesignationsManager,
      TerrainDesignationsRenderer terrainDesignationsRenderer,
      IVehiclesManager vehiclesManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(context);
      this.m_terrainCursor = terrainCursor;
      this.m_pickingManager = pickingManager;
      this.m_cursorManager = cursorManager;
      this.m_entitiesRenderer = entitiesRenderer;
      this.NavOverlayRenderer = navOverlayRenderer;
      this.VehiclesManager = vehiclesManager;
      this.DesignationsActivator = terrainDesignationsRenderer.CreateActivator().Combine(context.SurfaceDesignationsRenderer.CreateActivator());
      this.UnreachableDesignationsManager = unreachableDesignationsManager;
      this.UnreachableDesignationsLineRenderer = new MultiLineOverlayRendererHelper(linesFactory);
      this.UnreachableEntitiesLineRenderer = new MultiLineOverlayRendererHelper(linesFactory);
      this.GoalLineRenderer = new LineOverlayRendererHelper(linesFactory);
      this.GoalLineRenderer.SetWidth(1f);
      this.GoalLineRenderer.SetColor(new Color(0.0f, 0.8f, 0.0f, 0.5f));
      UiBuilder builder = context.Builder;
      this.m_goToCursor = this.m_cursorManager.RegisterCursor(builder.Style.Cursors.GoTo);
      this.m_shortcutsManager = builder.ShortcutsManager;
      this.m_goToSound = builder.AudioDb.GetSharedAudio(builder.Audio.EntitySelect);
      this.m_invalidSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
    }

    protected override void OnDeactivated()
    {
      this.m_renderedEntityTransform = Option<Transform>.None;
      this.GoalLineRenderer.HideLine();
      this.NavOverlayRenderer.HideOverlay();
      this.UnreachableDesignationsLineRenderer.ClearLines();
      this.UnreachableEntitiesLineRenderer.ClearLines();
      this.DesignationsActivator.DeactivateIfActive();
      base.OnDeactivated();
      this.deactivateCommandMode(false);
    }

    protected override void OnActivated()
    {
      base.OnActivated();
      this.m_highlightedAssignee = Option.Create<IRenderedEntity>(this.SelectedEntity.AssignedTo.ValueOrNull as IRenderedEntity);
      if (!this.m_highlightedAssignee.HasValue)
        return;
      this.HighlightSecondaryEntity(this.m_highlightedAssignee.Value);
    }

    public void StartGoToMode() => this.activateCommandMode();

    private void activateCommandMode()
    {
      if (this.m_commandModeInProgress)
        return;
      this.WindowView.Hide();
      this.m_commandModeInProgress = true;
      this.m_goToCursor.Show();
    }

    private void deactivateCommandMode(bool showInspector)
    {
      if (!this.m_commandModeInProgress)
        return;
      if (showInspector)
        this.WindowView.Show();
      this.m_goToCursor.Hide();
      this.m_commandModeInProgress = false;
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.m_commandModeInProgress)
        return base.InputUpdate(inputScheduler);
      if (this.m_shortcutsManager.IsSecondaryActionUp || UnityEngine.Input.GetKeyDown(KeyCode.Escape))
      {
        this.deactivateCommandMode(true);
        return true;
      }
      return this.m_shortcutsManager.IsPrimaryActionDown && tryHandleGoTo();

      bool tryHandleGoTo()
      {
        if (this.m_pickingManager.TryPickEntity<IRenderedEntity>(out IRenderedEntity _))
          return false;
        Tile3f position;
        if (this.m_terrainCursor.TryComputeCurrentPosition(out position))
        {
          inputScheduler.ScheduleInputCmd<NavigateVehicleToPositionCmd>(new NavigateVehicleToPositionCmd((Mafi.Core.Entities.Dynamic.Vehicle) this.SelectedEntity, position.Xy.Tile2i));
          this.deactivateCommandMode(true);
          this.m_goToSound.Play();
          return true;
        }
        this.m_invalidSound.Play();
        return true;
      }
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      if (this.SelectedEntity.IsDestroyed || !this.SelectedEntity.IsSpawned)
      {
        this.ForceDeactivate();
      }
      else
      {
        base.RenderUpdate(gameTime);
        if (!this.GoalLineRenderer.IsShown)
          return;
        this.updateGoalLine();
      }
    }

    private void updateGoalLine()
    {
      if (this.m_renderedEntityTransform.IsNone)
      {
        EntityMb entityMb;
        if (this.m_entitiesRenderer.TryGetMbFor((IRenderedEntity) this.SelectedEntity, out entityMb))
        {
          if (entityMb.gameObject.IsNullOrDestroyed())
          {
            Log.Error(string.Format("Received destroyed mb for {0}", (object) this.SelectedEntity));
            return;
          }
          this.m_renderedEntityTransform = (Option<Transform>) entityMb.transform;
        }
        else
        {
          Log.Warning(string.Format("Failed to find rendered entity for {0}", (object) this.SelectedEntity));
          return;
        }
      }
      this.GoalLineRenderer.UpdateLineStart(this.m_renderedEntityTransform.Value.position);
    }
  }
}
