// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.TerrainDesignationController
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
using Mafi.Unity.InputControl.Toolbar;
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
  public abstract class TerrainDesignationController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private static readonly RelTile1i MAX_AREA_SIZE_ADD;
    private static readonly RelTile1i MAX_AREA_SIZE_REMOVE;
    private static readonly ThicknessTilesI MAX_HEIGHT_BIAS;
    private static readonly Color REMOVE_AREA_COLOR;
    private readonly IAreaToolbox m_toolbox;
    private readonly DesignationControllerCursors m_cursors;
    private readonly MouseCursorMessage m_cursorMessage;
    private readonly IUnityInputMgr m_inputManager;
    private readonly TerrainCursor m_terrainCursor;
    private readonly TerrainDesignationsRenderer m_renderer;
    private readonly TerrainDesignationsManager m_manager;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly ToolbarController m_toolbarController;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly IActivator m_towerAreasAndDesignatorsActivator;
    private readonly IActivator m_oceanAreasActivator;
    private readonly TerrainRectSelection m_terrainOutlineRenderer;
    private readonly ITerrainDesignationsManager m_designationManager;
    private TerrainDesignationController.State m_state;
    private AreaMode m_designationMode;
    private readonly TerrainDesignationProto m_designationProto;
    private readonly CameraController m_cameraController;
    private readonly Option<Proto> m_hideIfProtoLocked;
    private readonly UiBuilder m_builder;
    private readonly bool m_isForestry;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private Direction90 m_preferredRampDirection;
    private ThicknessTilesI m_heightBias;
    private Tile2i? m_previousCursorPosition;
    private DesignationData? m_initialDesignation;
    private LocStrFormatted? m_previewErrorMsg;
    private bool m_errorIsOnlyWarning;
    private readonly Dict<Tile2i, DesignationData> m_previewAreas;
    private readonly Set<TerrainDesignation> m_selectedForRemoval;
    private Option<IInputCommand> m_ongoingCommand;
    private Tile2i m_areaStart;
    private Tile2i m_areaEnd;
    private bool m_areaChanged;
    private readonly AudioSource m_miningApplySound;
    private readonly AudioSource m_areaRemoveSound;
    private readonly AudioSource m_errorSound;
    private readonly AudioSource m_upSound;
    private readonly AudioSource m_downSound;
    private readonly AudioSource m_rotateSound;
    private Tile2i m_startUndesignationCursor;
    private Predicate<IDesignation> m_isAllowedToRemovePred;
    private bool m_isActive;
    private readonly AudioSource m_switchSound;

    public ControllerConfig Config
    {
      get => !this.m_isForestry ? ControllerConfig.Tool : ControllerConfig.ForestryTool;
    }

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible { get; private set; }

    public bool DeactivateShortcutsIfNotVisible => true;

    public bool IsActive => this.m_isActive;

    public TerrainDesignationController(
      IUnityInputMgr inputManager,
      TerrainCursor terrainCursor,
      TerrainDesignationsRenderer renderer,
      TowerAreasRenderer towerAreasRenderer,
      TerrainDesignationsManager manager,
      ShortcutsManager shortcutsManager,
      IAreaToolbox toolbox,
      DesignationControllerCursors cursors,
      MouseCursorMessage cursorMessage,
      ToolbarController toolbarController,
      IGameLoopEvents gameLoopEvents,
      TerrainRectSelection terrainOutlineRenderer,
      ITerrainDesignationsManager designationManager,
      UnlockedProtosDbForUi unlockedProtosDb,
      TerrainDesignationProto designationProto,
      CameraController cameraController,
      OceanAreasOverlayRenderer oceanOverlayRenderer,
      UiBuilder builder,
      AudioSource applySound,
      Option<Proto> hideIfProtoLocked,
      bool isForestry,
      bool defaultModeFlat)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_previewAreas = new Dict<Tile2i, DesignationData>();
      this.m_selectedForRemoval = new Set<TerrainDesignation>();
      this.m_ongoingCommand = Option<IInputCommand>.None;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_toolbox = toolbox;
      this.m_terrainCursor = terrainCursor;
      this.m_renderer = renderer;
      this.m_manager = manager;
      this.m_shortcutsManager = shortcutsManager;
      this.m_cursors = cursors;
      this.m_cursorMessage = cursorMessage;
      this.m_toolbarController = toolbarController;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_terrainOutlineRenderer = terrainOutlineRenderer;
      this.m_designationManager = designationManager;
      this.m_designationProto = designationProto;
      this.m_cameraController = cameraController;
      this.m_hideIfProtoLocked = hideIfProtoLocked;
      this.m_builder = builder;
      this.m_isForestry = isForestry;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_inputManager = inputManager;
      this.m_designationMode = defaultModeFlat ? AreaMode.Flat : AreaMode.Ramp;
      this.m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
      this.m_oceanAreasActivator = oceanOverlayRenderer.CreateActivator();
      this.m_isAllowedToRemovePred = (Predicate<IDesignation>) (d => d is TerrainDesignation);
      this.resetState();
      this.m_miningApplySound = applySound;
      this.m_areaRemoveSound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/DesignationRemove.prefab");
      this.m_errorSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
      this.m_switchSound = builder.AudioDb.GetSharedAudio(builder.Audio.TabSwitch);
      this.m_upSound = builder.AudioDb.GetSharedAudio(builder.Audio.Up);
      this.m_downSound = builder.AudioDb.GetSharedAudio(builder.Audio.Down);
      this.m_rotateSound = builder.AudioDb.GetSharedAudio(builder.Audio.Rotate);
    }

    protected abstract void RegisterUiToolbar(
      UiBuilder builder,
      ToolbarController toolbarController);

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      if (!this.m_isForestry)
      {
        this.IsVisible = this.m_hideIfProtoLocked.IsNone || this.m_unlockedProtosDb.IsUnlocked((IProto) this.m_hideIfProtoLocked.Value);
        if (!this.IsVisible)
          this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.updateIsVisible);
      }
      this.RegisterUiToolbar(this.m_builder, this.m_toolbarController);
    }

    private void updateIsVisible()
    {
      bool flag = this.m_unlockedProtosDb.IsUnlocked((IProto) this.m_hideIfProtoLocked.Value);
      if (this.IsVisible == flag)
        return;
      this.IsVisible = flag;
      Action<IToolbarItemController> visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged((IToolbarItemController) this);
    }

    public void Activate()
    {
      this.m_heightBias = ThicknessTilesI.Zero;
      // ISSUE: method pointer
      this.m_toolbox.SetOnRotate(new Action((object) this, __methodptr(\u003CActivate\u003Eg__onRotate\u007C65_0)));
      // ISSUE: method pointer
      this.m_toolbox.SetOnIncreaseElevation(new Action((object) this, __methodptr(\u003CActivate\u003Eg__incHeightBias\u007C65_1)));
      // ISSUE: method pointer
      this.m_toolbox.SetOnDecreaseElevation(new Action((object) this, __methodptr(\u003CActivate\u003Eg__decHeightBias\u007C65_2)));
      this.m_toolbox.SetMode(this.m_designationMode);
      this.m_toolbox.OnModeChanged += new Action<AreaMode>(this.modeChanged);
      this.m_toolbox.Show();
      this.m_terrainCursor.Activate();
      this.updateCursor();
      this.m_towerAreasAndDesignatorsActivator.Activate();
      this.m_oceanAreasActivator.Activate();
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<TerrainDesignationController>(this, new Action<GameTime>(this.sync));
      this.m_isActive = true;
    }

    public void Deactivate()
    {
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<TerrainDesignationController>(this, new Action<GameTime>(this.sync));
      this.m_toolbox.Hide();
      this.m_toolbox.OnModeChanged -= new Action<AreaMode>(this.modeChanged);
      this.m_terrainCursor.Deactivate();
      this.m_cursors.ClearCursors();
      this.m_cursorMessage.HideMessage();
      this.resetState();
      this.m_towerAreasAndDesignatorsActivator.Deactivate();
      this.m_oceanAreasActivator.Deactivate();
      this.m_isActive = false;
    }

    bool IUnityInputController.InputUpdate(IInputScheduler inputScheduler)
    {
      return this.InputUpdate(inputScheduler);
    }

    private void modeChanged(AreaMode mode)
    {
      this.m_designationMode = mode;
      this.m_toolbox.SetMode(this.m_designationMode);
      this.resetState();
      this.updateCursor();
      this.m_switchSound.Play();
    }

    private void rampDirectionChanged(Direction90 dir)
    {
      this.m_preferredRampDirection = dir;
      this.resetState();
      this.m_rotateSound.Play();
    }

    private void heightBiasChanged(ThicknessTilesI bias, bool isUp)
    {
      this.m_heightBias = bias.Clamp(-TerrainDesignationController.MAX_HEIGHT_BIAS, TerrainDesignationController.MAX_HEIGHT_BIAS);
      this.resetState();
      if (isUp)
        this.m_upSound.Play();
      else
        this.m_downSound.Play();
    }

    private void clearPreview()
    {
      foreach (DesignationData designationData in this.m_previewAreas.Values)
        this.m_renderer.RemovePreviewDesignation(designationData.OriginTile);
      this.m_previewAreas.Clear();
      foreach (TerrainDesignation terrainDesignation in this.m_selectedForRemoval)
        this.m_renderer.HideRemoval(terrainDesignation.Data.OriginTile);
      this.m_selectedForRemoval.Clear();
    }

    private void resetState()
    {
      this.clearPreview();
      this.m_ongoingCommand = Option<IInputCommand>.None;
      this.m_initialDesignation = new DesignationData?();
      this.m_previousCursorPosition = new Tile2i?();
      this.m_terrainOutlineRenderer.Hide();
      this.m_state = TerrainDesignationController.State.Initial;
      this.m_cameraController.SetMousePanDisabled(false);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.m_isForestry)
      {
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Flip))
        {
          this.modeChanged((AreaMode) ((int) (this.m_designationMode + 1) % 2));
          return true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Rotate))
        {
          this.rampDirectionChanged(this.m_preferredRampDirection.RotatedMinus90);
          return true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.RaiseUp))
        {
          this.heightBiasChanged(this.m_heightBias + ThicknessTilesI.One, true);
          return true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.LowerDown))
        {
          this.heightBiasChanged(this.m_heightBias - ThicknessTilesI.One, false);
          return true;
        }
      }
      TerrainDesignationController.State? nullable = this.handleState(inputScheduler);
      if (!nullable.HasValue)
        return false;
      this.m_state = nullable.Value;
      if (this.m_state == TerrainDesignationController.State.Initial && this.m_ongoingCommand.IsNone)
        this.resetState();
      else
        this.m_cameraController.SetMousePanDisabled(this.m_state == TerrainDesignationController.State.Designation || this.m_state == TerrainDesignationController.State.Undesignation);
      return true;
    }

    private TerrainDesignationController.State? handleState(IInputScheduler inputScheduler)
    {
      if (this.m_ongoingCommand.HasValue)
      {
        if (this.m_ongoingCommand.Value.IsProcessedAndSynced)
          this.resetState();
        return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
      }
      switch (this.m_state)
      {
        case TerrainDesignationController.State.Initial:
          return this.handleInitialState();
        case TerrainDesignationController.State.Designation:
          return this.handleDesignation(inputScheduler);
        case TerrainDesignationController.State.Undesignation:
          return this.handleUndesignation(inputScheduler);
        default:
          Assert.Fail(string.Format("Unhandled state {0}", (object) this.m_state));
          return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
      }
    }

    private TerrainDesignationController.State? handleInitialState()
    {
      if (!this.m_terrainCursor.HasValue)
        return new TerrainDesignationController.State?();
      if (EventSystem.current.IsPointerOverGameObject())
        return new TerrainDesignationController.State?();
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.ClearDesignation))
      {
        this.clearPreview();
        this.m_areaStart = this.m_areaEnd = this.m_previousCursorPosition.GetValueOrDefault();
        this.m_startUndesignationCursor = this.m_terrainCursor.Tile2iClampedToLimits;
        this.m_areaChanged = true;
        this.m_previousCursorPosition = new Tile2i?();
        this.m_terrainOutlineRenderer.SetArea(new RectangleTerrainArea2i(this.m_areaStart, RelTile2i.One), TerrainDesignationController.REMOVE_AREA_COLOR);
        this.m_terrainOutlineRenderer.Show();
        return new TerrainDesignationController.State?(TerrainDesignationController.State.Undesignation);
      }
      if (this.m_shortcutsManager.IsPrimaryActionDown)
      {
        if (!this.m_initialDesignation.HasValue || !this.m_previousCursorPosition.HasValue)
        {
          this.m_errorSound.Play();
          return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
        }
        this.m_areaStart = this.m_areaEnd = this.m_previousCursorPosition.Value;
        this.m_areaChanged = true;
        this.m_previousCursorPosition = new Tile2i?();
        return new TerrainDesignationController.State?(TerrainDesignationController.State.Designation);
      }
      if (!this.m_shortcutsManager.IsSecondaryActionDown || this.m_previousCursorPosition.HasValue)
        return new TerrainDesignationController.State?();
      this.m_errorSound.Play();
      return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
    }

    private TerrainDesignationController.State? handleDesignation(IInputScheduler inputScheduler)
    {
      if (!this.m_initialDesignation.HasValue)
      {
        Log.Error("Invalid state: no initial designation.");
        return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
      }
      if (this.m_shortcutsManager.IsSecondaryActionDown)
        return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
      if (this.m_shortcutsManager.IsPrimaryActionUp)
      {
        if (this.m_previewAreas.IsEmpty)
        {
          this.m_errorSound.Play();
        }
        else
        {
          this.m_ongoingCommand = (Option<IInputCommand>) (IInputCommand) inputScheduler.ScheduleInputCmd<AddTerrainDesignationsCmd>(new AddTerrainDesignationsCmd(this.m_designationProto.Id, this.m_previewAreas.Values.ToImmutableArray<DesignationData>()));
          this.m_miningApplySound.Play();
        }
        return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
      }
      if (this.computeDesignationEnd(this.m_terrainCursor.Tile2iClampedToLimits, TerrainDesignationController.MAX_AREA_SIZE_ADD))
        this.m_areaChanged = true;
      return new TerrainDesignationController.State?(TerrainDesignationController.State.Designation);
    }

    private TerrainDesignationController.State? handleUndesignation(IInputScheduler inputScheduler)
    {
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.ClearDesignation))
      {
        if (this.m_selectedForRemoval.IsEmpty)
        {
          if (this.m_startUndesignationCursor == this.m_terrainCursor.Tile2iClampedToLimits)
            this.m_inputManager.DeactivateController((IUnityInputController) this);
          else
            this.m_errorSound.Play();
        }
        else
        {
          this.m_ongoingCommand = (Option<IInputCommand>) (IInputCommand) inputScheduler.ScheduleInputCmd<RemoveDesignationsCmd>(new RemoveDesignationsCmd(this.m_selectedForRemoval.ToImmutableArray<TerrainDesignation, Tile2i>((Func<TerrainDesignation, Tile2i>) (x => x.OriginTileCoord))));
          this.m_areaRemoveSound.Play();
        }
        return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
      }
      if (this.m_shortcutsManager.IsPrimaryActionUp)
        return new TerrainDesignationController.State?(TerrainDesignationController.State.Initial);
      if (this.computeDesignationEnd(this.m_terrainCursor.Tile2iClampedToLimits, TerrainDesignationController.MAX_AREA_SIZE_REMOVE))
      {
        this.m_terrainOutlineRenderer.SetArea(RectangleTerrainArea2i.FromTwoPositions(this.m_areaStart, this.m_areaEnd), TerrainDesignationController.REMOVE_AREA_COLOR);
        this.m_areaChanged = true;
      }
      return new TerrainDesignationController.State?(TerrainDesignationController.State.Undesignation);
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
      if (this.m_state == TerrainDesignationController.State.Initial)
      {
        this.clearPreview();
        this.m_initialDesignation = new DesignationData?();
        if (this.m_terrainCursor.HasValue)
        {
          this.previewInitialDesignationAt(tile2iClampedToLimits, out this.m_previewErrorMsg);
          Option<TerrainDesignation> designationAt = this.m_designationManager.GetDesignationAt(tile2iClampedToLimits);
          if (designationAt.HasValue)
          {
            this.m_errorIsOnlyWarning = true;
            this.m_previewErrorMsg = designationAt.Value.GetWarningStr();
          }
        }
        else
          this.m_previewErrorMsg = new LocStrFormatted?((LocStrFormatted) TrCore.DesignationError__Invalid);
      }
      else if (this.m_state == TerrainDesignationController.State.Designation)
      {
        Assert.That<DesignationData?>(this.m_initialDesignation).HasValue<DesignationData>();
        if (this.m_areaChanged && this.m_initialDesignation.HasValue)
          DesignationDataFactory.UpdateDesignationExtension((ITerrainDesignationsManager) this.m_manager, this.m_designationProto, this.m_initialDesignation.Value, this.m_areaEnd, new Action<TerrainDesignationProto, DesignationData>(this.m_renderer.AddOrUpdatePreviewDesignation), (Action<TerrainDesignationProto, DesignationData>) ((_, d) => this.m_renderer.RemovePreviewDesignation(d.OriginTile)), this.m_previewAreas);
      }
      else if (this.m_state == TerrainDesignationController.State.Undesignation && this.m_areaChanged)
        this.m_manager.UpdateSelectedDesignationsInArea(this.m_areaStart, this.m_areaEnd, (Action<TerrainDesignation>) (d => this.m_renderer.ShowRemovalImmediate(d.Data.OriginTile)), (Action<TerrainDesignation>) (d => this.m_renderer.HideRemovalImmediate(d.Data.OriginTile)), (Predicate<TerrainDesignation>) this.m_isAllowedToRemovePred, this.m_selectedForRemoval);
      this.m_previousCursorPosition = new Tile2i?(tile2iClampedToLimits);
      this.updateCursor();
    }

    private void previewInitialDesignationAt(Tile2i position, out LocStrFormatted? error)
    {
      Assert.That<TerrainDesignationProto>(this.m_designationProto).IsNotNull<TerrainDesignationProto>();
      Assert.That<Dict<Tile2i, DesignationData>>(this.m_previewAreas).IsEmpty<Tile2i, DesignationData>();
      Assert.That<DesignationData?>(this.m_initialDesignation).IsNone<DesignationData>();
      DesignationData data;
      if (!DesignationDataFactory.TryCreateSnapToNeighbors(position, this.m_designationMode == AreaMode.Flat, this.m_designationProto.PreferInitialBelowTerrain, this.m_preferredRampDirection, this.m_heightBias, this.m_designationManager, out data))
      {
        error = new LocStrFormatted?((LocStrFormatted) TrCore.DesignationError__Invalid);
      }
      else
      {
        LocStrFormatted error1;
        if (!this.m_manager.IsDesignationAllowed(data, out error1))
        {
          error = new LocStrFormatted?(error1);
        }
        else
        {
          this.m_initialDesignation = new DesignationData?(data);
          this.m_previewAreas.Add(data.OriginTile, data);
          this.m_renderer.AddOrUpdatePreviewDesignation(this.m_designationProto, data);
          error = new LocStrFormatted?();
        }
      }
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
      if (this.m_state == TerrainDesignationController.State.Undesignation)
        this.m_cursors.ShowClearDesignationCursor();
      else
        this.m_cursors.ShowCursorFor(this.m_designationMode);
    }

    static TerrainDesignationController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TerrainDesignationController.MAX_AREA_SIZE_ADD = new RelTile1i((int) sbyte.MaxValue);
      TerrainDesignationController.MAX_AREA_SIZE_REMOVE = new RelTile1i(191);
      TerrainDesignationController.MAX_HEIGHT_BIAS = 10.TilesThick();
      TerrainDesignationController.REMOVE_AREA_COLOR = Color.red;
    }

    private enum State
    {
      Initial,
      Designation,
      Undesignation,
    }
  }
}
