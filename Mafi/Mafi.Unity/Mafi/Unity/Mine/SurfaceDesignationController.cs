// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.SurfaceDesignationController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Inspectors.Buildings;
using Mafi.Unity.Terrain;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Mine
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class SurfaceDesignationController
  {
    private static readonly RelTile1i MAX_AREA_SIZE_ADD;
    private static readonly RelTile1i MAX_AREA_SIZE_REMOVE;
    private static readonly ThicknessTilesI MAX_HEIGHT_BIAS;
    private static readonly Color REMOVE_AREA_COLOR;
    private readonly DesignationControllerCursors m_cursors;
    private readonly MouseCursorMessage m_cursorMessage;
    private readonly IUnityInputMgr m_inputManager;
    private readonly TerrainCursor m_terrainCursor;
    private readonly SurfaceDesignationsRenderer m_renderer;
    private readonly SurfaceDesignationsManager m_manager;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly IActivator m_towerAreasAndDesignatorsActivator;
    private readonly IActivator m_oceanAreasActivator;
    private readonly TerrainRectSelection m_terrainOutlineRenderer;
    private SurfaceDesignationController.State m_state;
    private SurfaceDesignationProto m_designationProto;
    private readonly SurfaceDesignationProto m_concreteDesignationProto;
    private readonly SurfaceDesignationProto m_clearDesignationProto;
    private readonly CameraController m_cameraController;
    private readonly SurfacePlacementToolbox m_surfacePlacementToolbox;
    private readonly SurfaceClearToolbox m_surfaceClearToolbox;
    private Tile2i? m_previousCursorPosition;
    private SurfaceDesignationData? m_initialDesignation;
    private TerrainTileSurfaceProto m_surfaceProto;
    private readonly UiBuilder m_builder;
    private LocStrFormatted? m_previewErrorMsg;
    private bool m_errorIsOnlyWarning;
    private readonly Dict<Tile2i, SurfaceDesignationData> m_previewAreas;
    private readonly Set<SurfaceDesignation> m_selectedForRemoval;
    private Option<IInputCommand> m_ongoingCommand;
    private Tile2i m_areaStart;
    private Tile2i m_areaEnd;
    private bool m_areaChanged;
    private readonly AudioSource m_applySound;
    private readonly AudioSource m_applyMetalSound;
    private readonly AudioSource m_errorSound;
    private Tile2i m_startUndesignationCursor;
    private Option<Action> m_onDeactivated;
    private bool m_isActive;
    private readonly AudioSource m_demolishSound;

    public ControllerConfig Config => ControllerConfig.Tool;

    public bool IsActive => this.m_isActive;

    internal SurfaceDesignationController(
      IUnityInputMgr inputManager,
      TerrainCursor terrainCursor,
      SurfaceDesignationsRenderer renderer,
      TowerAreasRenderer towerAreasRenderer,
      SurfaceDesignationsManager manager,
      ShortcutsManager shortcutsManager,
      DesignationControllerCursors cursors,
      MouseCursorMessage cursorMessage,
      IGameLoopEvents gameLoopEvents,
      TerrainRectSelection terrainOutlineRenderer,
      CameraController cameraController,
      OceanAreasOverlayRenderer oceanOverlayRenderer,
      ProtosDb protosDb,
      SurfacePlacementToolbox surfacePlacementToolbox,
      SurfaceClearToolbox surfaceClearToolbox,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_previewAreas = new Dict<Tile2i, SurfaceDesignationData>();
      this.m_selectedForRemoval = new Set<SurfaceDesignation>();
      this.m_ongoingCommand = Option<IInputCommand>.None;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainCursor = terrainCursor;
      this.m_renderer = renderer;
      this.m_manager = manager;
      this.m_shortcutsManager = shortcutsManager;
      this.m_cursors = cursors;
      this.m_cursorMessage = cursorMessage;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_terrainOutlineRenderer = terrainOutlineRenderer;
      this.m_concreteDesignationProto = protosDb.GetOrThrow<SurfaceDesignationProto>(IdsCore.TerrainDesignators.PlaceSurfaceDesignator);
      this.m_clearDesignationProto = protosDb.GetOrThrow<SurfaceDesignationProto>(IdsCore.TerrainDesignators.ClearSurfaceDesignator);
      this.m_designationProto = this.m_concreteDesignationProto;
      this.m_cameraController = cameraController;
      this.m_surfacePlacementToolbox = surfacePlacementToolbox;
      this.m_surfaceClearToolbox = surfaceClearToolbox;
      this.m_inputManager = inputManager;
      this.m_surfaceProto = TerrainTileSurfaceProto.Phantom;
      this.m_builder = builder;
      this.m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
      this.m_oceanAreasActivator = oceanOverlayRenderer.CreateActivator();
      this.m_applySound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/SurfaceApply.prefab");
      this.m_applyMetalSound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/SurfaceMetalApply.prefab");
      this.m_demolishSound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/Demolish.prefab");
      this.m_errorSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
      this.resetState();
    }

    public void ActivateFor(Option<TerrainTileSurfaceProto> proto, Action onDeactivated)
    {
      this.m_onDeactivated = (Option<Action>) onDeactivated;
      this.m_surfaceProto = proto.ValueOrNull ?? TerrainTileSurfaceProto.Phantom;
      this.m_designationProto = proto.HasValue ? this.m_concreteDesignationProto : this.m_clearDesignationProto;
      if (this.m_isActive)
        return;
      this.Activate();
    }

    public void Activate()
    {
      this.m_terrainCursor.Activate();
      this.updateCursor();
      this.m_towerAreasAndDesignatorsActivator.Activate();
      this.m_oceanAreasActivator.Activate();
      if (this.m_surfaceProto.IsNotPhantom)
        this.m_surfacePlacementToolbox.Show();
      else
        this.m_surfaceClearToolbox.Show();
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<SurfaceDesignationController>(this, new Action<GameTime>(this.sync));
      this.m_isActive = true;
    }

    public void Deactivate()
    {
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<SurfaceDesignationController>(this, new Action<GameTime>(this.sync));
      this.m_surfacePlacementToolbox.Hide();
      this.m_surfaceClearToolbox.Hide();
      this.m_terrainCursor.Deactivate();
      this.m_cursors.ClearCursors();
      this.m_cursorMessage.HideMessage();
      this.resetState();
      this.m_towerAreasAndDesignatorsActivator.Deactivate();
      this.m_oceanAreasActivator.Deactivate();
      Action valueOrNull = this.m_onDeactivated.ValueOrNull;
      if (valueOrNull != null)
        valueOrNull();
      this.m_onDeactivated = Option<Action>.None;
      this.m_isActive = false;
    }

    private void clearPreview()
    {
      foreach (SurfaceDesignationData surfaceDesignationData in this.m_previewAreas.Values)
        this.m_renderer.RemovePreviewDesignation((Tile2i) surfaceDesignationData.OriginTile);
      this.m_previewAreas.Clear();
      foreach (SurfaceDesignation surfaceDesignation in this.m_selectedForRemoval)
        this.m_renderer.HideRemoval((Tile2i) surfaceDesignation.OriginTile);
      this.m_selectedForRemoval.Clear();
      this.m_renderer.ClearRemovalArea();
      this.m_renderer.ClearAdditionArea();
    }

    private void resetState()
    {
      this.clearPreview();
      this.m_ongoingCommand = Option<IInputCommand>.None;
      this.m_initialDesignation = new SurfaceDesignationData?();
      this.m_previousCursorPosition = new Tile2i?();
      this.m_terrainOutlineRenderer.Hide();
      this.m_state = SurfaceDesignationController.State.Initial;
      this.m_cameraController.SetMousePanDisabled(false);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.m_surfaceProto.IsNotPhantom)
      {
        this.m_surfacePlacementToolbox.PrimaryActionBtn.SetIsOn(this.m_shortcutsManager.IsPrimaryActionOn);
        this.m_surfacePlacementToolbox.SecondaryActionBtn.SetIsOn(this.m_shortcutsManager.IsSecondaryActionOn);
      }
      else
      {
        this.m_surfaceClearToolbox.PrimaryActionBtn.SetIsOn(this.m_shortcutsManager.IsPrimaryActionOn);
        this.m_surfaceClearToolbox.SecondaryActionBtn.SetIsOn(this.m_shortcutsManager.IsSecondaryActionOn);
      }
      SurfaceDesignationController.State? nullable = this.handleState(inputScheduler);
      if (!nullable.HasValue)
        return false;
      this.m_state = nullable.Value;
      if (this.m_state == SurfaceDesignationController.State.Initial && this.m_ongoingCommand.IsNone)
        this.resetState();
      else
        this.m_cameraController.SetMousePanDisabled(this.m_state == SurfaceDesignationController.State.Designation || this.m_state == SurfaceDesignationController.State.Undesignation);
      return true;
    }

    private SurfaceDesignationController.State? handleState(IInputScheduler inputScheduler)
    {
      if (this.m_ongoingCommand.HasValue)
      {
        if (this.m_ongoingCommand.Value.IsProcessedAndSynced)
          this.resetState();
        return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
      }
      switch (this.m_state)
      {
        case SurfaceDesignationController.State.Initial:
          return this.handleInitialState();
        case SurfaceDesignationController.State.Designation:
          return this.handleDesignation(inputScheduler);
        case SurfaceDesignationController.State.Undesignation:
          return this.handleUndesignation(inputScheduler);
        default:
          Assert.Fail(string.Format("Unhandled state {0}", (object) this.m_state));
          return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
      }
    }

    private SurfaceDesignationController.State? handleInitialState()
    {
      if (!this.m_terrainCursor.HasValue)
        return new SurfaceDesignationController.State?();
      if (EventSystem.current.IsPointerOverGameObject())
        return new SurfaceDesignationController.State?();
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.ClearDesignation))
      {
        this.clearPreview();
        this.m_areaStart = this.m_areaEnd = this.m_previousCursorPosition.GetValueOrDefault();
        this.m_startUndesignationCursor = this.m_terrainCursor.Tile2iClampedToLimits;
        this.m_areaChanged = true;
        this.m_previousCursorPosition = new Tile2i?();
        this.m_terrainOutlineRenderer.SetArea(new RectangleTerrainArea2i(this.m_areaStart, RelTile2i.One), SurfaceDesignationController.REMOVE_AREA_COLOR);
        this.m_terrainOutlineRenderer.Show();
        return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Undesignation);
      }
      if (this.m_shortcutsManager.IsPrimaryActionDown)
      {
        if (!this.m_initialDesignation.HasValue || !this.m_previousCursorPosition.HasValue)
        {
          this.m_errorSound.Play();
          return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
        }
        this.m_areaStart = this.m_areaEnd = this.m_previousCursorPosition.Value;
        this.m_areaChanged = true;
        this.m_previousCursorPosition = new Tile2i?();
        return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Designation);
      }
      if (!this.m_shortcutsManager.IsSecondaryActionDown || this.m_previousCursorPosition.HasValue)
        return new SurfaceDesignationController.State?();
      this.m_errorSound.Play();
      return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
    }

    private SurfaceDesignationController.State? handleDesignation(IInputScheduler inputScheduler)
    {
      if (!this.m_initialDesignation.HasValue)
      {
        Log.Error("Invalid state: no initial designation.");
        return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
      }
      if (this.m_shortcutsManager.IsSecondaryActionDown)
        return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
      if (this.m_shortcutsManager.IsPrimaryActionUp)
      {
        if (this.m_previewAreas.IsEmpty)
        {
          this.m_errorSound.Play();
        }
        else
        {
          this.m_ongoingCommand = (Option<IInputCommand>) (IInputCommand) inputScheduler.ScheduleInputCmd<AddSurfaceDesignationsCmd>(new AddSurfaceDesignationsCmd(this.m_designationProto.Id, this.m_previewAreas.Values.ToImmutableArray<SurfaceDesignationData>()));
          if ((Proto) this.m_designationProto == (Proto) this.m_clearDesignationProto)
          {
            this.m_demolishSound.Play();
          }
          else
          {
            string str = this.m_surfaceProto.CostPerTile.Product.Id.Value;
            if (str.Contains("Iron") || str.Contains("Steel") || str.Contains("Gold"))
              this.m_applyMetalSound.Play();
            else
              this.m_applySound.Play();
          }
        }
        return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
      }
      if (this.computeDesignationEnd(this.m_terrainCursor.Tile2iClampedToLimits, SurfaceDesignationController.MAX_AREA_SIZE_ADD))
        this.m_areaChanged = true;
      return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Designation);
    }

    private SurfaceDesignationController.State? handleUndesignation(IInputScheduler inputScheduler)
    {
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.ClearDesignation))
      {
        if (this.m_selectedForRemoval.IsEmpty)
        {
          if (this.m_startUndesignationCursor == this.m_terrainCursor.Tile2iClampedToLimits)
            this.Deactivate();
          else
            this.m_errorSound.Play();
        }
        else
        {
          this.m_ongoingCommand = (Option<IInputCommand>) (IInputCommand) inputScheduler.ScheduleInputCmd<RemoveSurfaceDesignationsCmd>(new RemoveSurfaceDesignationsCmd(this.m_selectedForRemoval.ToImmutableArray<SurfaceDesignation, Tile2i>((Func<SurfaceDesignation, Tile2i>) (x => x.OriginTileCoord)), RectangleTerrainArea2i.FromTwoPositions(this.m_areaStart, this.m_areaEnd)));
          this.m_demolishSound.Play();
        }
        return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
      }
      if (this.m_shortcutsManager.IsPrimaryActionUp)
        return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Initial);
      if (this.computeDesignationEnd(this.m_terrainCursor.Tile2iClampedToLimits, SurfaceDesignationController.MAX_AREA_SIZE_REMOVE))
      {
        this.m_terrainOutlineRenderer.SetArea(RectangleTerrainArea2i.FromTwoPositions(this.m_areaStart, this.m_areaEnd), SurfaceDesignationController.REMOVE_AREA_COLOR);
        this.m_areaChanged = true;
      }
      return new SurfaceDesignationController.State?(SurfaceDesignationController.State.Undesignation);
    }

    private bool computeDesignationEnd(Tile2i endTile, RelTile1i maxSize)
    {
      if (this.m_areaEnd == endTile)
        return false;
      Tile2i tile2i = this.m_areaStart + (endTile - this.m_areaStart).Clamp(-maxSize.Value, maxSize.Value);
      if (tile2i == this.m_areaEnd)
        return false;
      this.m_areaEnd = tile2i;
      return true;
    }

    private void sync(GameTime time)
    {
      Tile2i tile2iClampedToLimits = this.m_terrainCursor.Tile2iClampedToLimits;
      Tile2i? previousCursorPosition = this.m_previousCursorPosition;
      Tile2i tile2i = tile2iClampedToLimits;
      if ((previousCursorPosition.HasValue ? (previousCursorPosition.GetValueOrDefault() == tile2i ? 1 : 0) : 0) != 0)
        return;
      this.m_previewErrorMsg = new LocStrFormatted?();
      this.m_errorIsOnlyWarning = false;
      if (this.m_state == SurfaceDesignationController.State.Initial)
      {
        this.clearPreview();
        this.m_initialDesignation = new SurfaceDesignationData?();
        if (this.m_terrainCursor.HasValue)
        {
          this.previewInitialDesignationAt(tile2iClampedToLimits, out this.m_previewErrorMsg);
          Option<SurfaceDesignation> designationAt = this.m_manager.GetDesignationAt(tile2iClampedToLimits);
          if (designationAt.HasValue)
          {
            this.m_errorIsOnlyWarning = true;
            this.m_previewErrorMsg = designationAt.Value.GetWarningStr();
          }
        }
        else
          this.m_previewErrorMsg = new LocStrFormatted?((LocStrFormatted) TrCore.DesignationError__Invalid);
      }
      else if (this.m_state == SurfaceDesignationController.State.Designation)
      {
        Assert.That<SurfaceDesignationData?>(this.m_initialDesignation).HasValue<SurfaceDesignationData>();
        if (this.m_areaChanged && this.m_initialDesignation.HasValue)
        {
          SurfaceDesignationDataFactory.UpdateDesignationExtension((ISurfaceDesignationsManager) this.m_manager, this.m_designationProto, this.m_initialDesignation.Value, this.m_areaStart, this.m_areaEnd, this.m_surfaceProto, new Action<SurfaceDesignationProto, SurfaceDesignationData>(this.m_renderer.AddOrUpdatePreviewDesignation), (Action<SurfaceDesignationProto, SurfaceDesignationData>) ((_, d) => this.m_renderer.RemovePreviewDesignation((Tile2i) d.OriginTile)), new Action<SurfaceDesignationProto, SurfaceDesignationData>(this.m_renderer.AddOrUpdatePreviewDesignation), this.m_previewAreas);
          this.m_renderer.SetAdditionArea(RectangleTerrainArea2i.FromTwoPositions(this.m_areaStart, this.m_areaEnd));
        }
      }
      else if (this.m_state == SurfaceDesignationController.State.Undesignation && this.m_areaChanged)
      {
        this.m_manager.UpdateSelectedDesignationsInArea(this.m_areaStart, this.m_areaEnd, (Action<SurfaceDesignation>) (d => this.m_renderer.ShowRemovalImmediate((Tile2i) d.OriginTile)), (Action<SurfaceDesignation>) (d => this.m_renderer.HideRemovalImmediate((Tile2i) d.OriginTile)), this.m_selectedForRemoval);
        this.m_renderer.SetRemovalArea(RectangleTerrainArea2i.FromTwoPositions(this.m_areaStart, this.m_areaEnd));
      }
      this.m_previousCursorPosition = new Tile2i?(tile2iClampedToLimits);
      this.updateCursor();
    }

    private void previewInitialDesignationAt(Tile2i position, out LocStrFormatted? error)
    {
      Assert.That<SurfaceDesignationProto>(this.m_designationProto).IsNotNull<SurfaceDesignationProto>();
      Assert.That<Dict<Tile2i, SurfaceDesignationData>>(this.m_previewAreas).IsEmpty<Tile2i, SurfaceDesignationData>();
      Assert.That<SurfaceDesignationData?>(this.m_initialDesignation).IsNone<SurfaceDesignationData>();
      SurfaceDesignationData data;
      SurfaceDesignationDataFactory.TryCreateSnapToNeighbors(position, (ISurfaceDesignationsManager) this.m_manager, this.m_surfaceProto, out data);
      this.m_initialDesignation = new SurfaceDesignationData?(data);
      this.m_previewAreas.Add((Tile2i) data.OriginTile, data);
      this.m_renderer.AddOrUpdatePreviewDesignation(this.m_designationProto, data);
      this.m_renderer.SetAdditionArea(new RectangleTerrainArea2i(position, RelTile2i.One));
      error = new LocStrFormatted?();
    }

    private void updateCursor()
    {
      if (this.m_previewErrorMsg.HasValue)
      {
        this.m_cursorMessage.ShowMessage(this.m_previewErrorMsg.Value.Value);
        if (!this.m_errorIsOnlyWarning)
        {
          this.m_cursors.ShowDenyDesignationCursor();
          return;
        }
      }
      else
        this.m_cursorMessage.HideMessage();
      if (this.m_state == SurfaceDesignationController.State.Undesignation)
        this.m_cursors.ShowClearDesignationCursor();
      else
        this.m_cursors.ShowCursorFor(AreaMode.Flat);
    }

    static SurfaceDesignationController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      SurfaceDesignationController.MAX_AREA_SIZE_ADD = new RelTile1i((int) sbyte.MaxValue);
      SurfaceDesignationController.MAX_AREA_SIZE_REMOVE = new RelTile1i(191);
      SurfaceDesignationController.MAX_HEIGHT_BIAS = 10.TilesThick();
      SurfaceDesignationController.REMOVE_AREA_COLOR = Color.red;
    }

    private enum State
    {
      Initial,
      Designation,
      Undesignation,
    }
  }
}
