// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Mine.DecalDesignationController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Inspectors.Buildings;
using Mafi.Unity.Terrain;
using Mafi.Unity.UiFramework;
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
  internal class DecalDesignationController : IUnityInputController
  {
    private static readonly RelTile1i MAX_AREA_SIZE_ADD;
    private static readonly RelTile1i MAX_AREA_SIZE_REMOVE;
    private static readonly Color REMOVE_AREA_COLOR;
    private readonly MouseCursorMessage m_cursorMessage;
    private readonly TerrainCursor m_terrainCursor;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly IActivator m_towerAreasAndDesignatorsActivator;
    private readonly IActivator m_oceanAreasActivator;
    private readonly TerrainRectSelection m_terrainOutlineRenderer;
    private DecalDesignationController.State m_state;
    private readonly CameraController m_cameraController;
    private readonly DecalPlacementToolbox m_decalPlacementToolbox;
    private readonly TerrainManager m_terrainManager;
    private readonly UiBuilder m_builder;
    private Tile2i? m_previousCursorPosition;
    private Tile2i? m_initialCursorPosition;
    private bool m_isInitialCursorOnValidSurface;
    private RectangleTerrainArea2i m_previewArea;
    private LocStrFormatted? m_previewErrorMsg;
    private Option<IInputCommand> m_ongoingCommand;
    private Tile2i m_areaStart;
    private Tile2i m_areaEnd;
    private bool m_areaChanged;
    private readonly AudioSource m_applySound;
    private readonly AudioSource m_applySoundLong;
    private readonly AudioSource m_removeSound;
    private readonly AudioSource m_errorSound;
    private readonly AudioSource m_rotateSound;
    private Option<Action> m_onDeactivated;
    private bool m_isActive;
    private Rotation90 m_rotation;
    private bool m_flipped;
    private bool m_flipRotateChanged;
    private readonly DecalPickerView m_decalPicker;
    private bool m_wasPickerBuilt;
    private readonly Cursoor m_paintCursor;
    private readonly Cursoor m_clearCursor;

    public ControllerConfig Config => ControllerConfig.Tool;

    private TerrainTileSurfaceDecalProto DecalProto => this.m_decalPicker.SelectedDecal;

    private int ColorKey => this.m_decalPicker.SelectedColorIndex;

    internal DecalDesignationController(
      IUnityInputMgr inputManager,
      NewInstanceOf<DecalPickerView> decalPicker,
      TerrainCursor terrainCursor,
      TowerAreasRenderer towerAreasRenderer,
      TerrainRenderer terrainRenderer,
      ShortcutsManager shortcutsManager,
      CursorManager cursorManager,
      MouseCursorMessage cursorMessage,
      IGameLoopEvents gameLoopEvents,
      TerrainRectSelection terrainOutlineRenderer,
      CameraController cameraController,
      OceanAreasOverlayRenderer oceanOverlayRenderer,
      DecalPlacementToolbox decalPlacementToolbox,
      TerrainManager terrainManager,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_ongoingCommand = Option<IInputCommand>.None;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      DecalDesignationController controller = this;
      this.m_terrainCursor = terrainCursor;
      this.m_terrainRenderer = terrainRenderer;
      this.m_shortcutsManager = shortcutsManager;
      this.m_cursorMessage = cursorMessage;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_terrainOutlineRenderer = terrainOutlineRenderer;
      this.m_cameraController = cameraController;
      this.m_decalPlacementToolbox = decalPlacementToolbox;
      this.m_terrainManager = terrainManager;
      this.m_builder = builder;
      this.m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
      this.m_oceanAreasActivator = oceanOverlayRenderer.CreateActivator();
      this.m_applySound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/SprayPaint.prefab");
      this.m_applySoundLong = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/SprayPaintLong.prefab");
      this.m_removeSound = builder.AudioDb.GetSharedAudio(builder.Audio.ButtonClick);
      this.m_rotateSound = builder.AudioDb.GetSharedAudioUi("Assets/Unity/UserInterface/Audio/Rotate.prefab");
      this.m_errorSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
      this.m_decalPlacementToolbox.SetOnRotate(new Action(this.onRotate));
      this.m_decalPlacementToolbox.SetOnFlip(new Action(this.onFlip));
      this.m_decalPicker = decalPicker.Instance;
      this.m_decalPicker.SetOnClose((Action) (() => inputManager.DeactivateController((IUnityInputController) controller)));
      this.m_paintCursor = cursorManager.RegisterCursor(builder.Style.Cursors.Paint);
      this.m_clearCursor = cursorManager.RegisterCursor(builder.Style.Cursors.Clear);
      this.resetState();
    }

    private void onRotate()
    {
      this.m_rotation -= Rotation90.Deg90;
      this.m_flipRotateChanged = true;
      this.m_rotateSound.Play();
    }

    private void onFlip()
    {
      this.m_flipped = !this.m_flipped;
      this.m_flipRotateChanged = true;
      this.m_rotateSound.Play();
    }

    public void Activate()
    {
      this.m_terrainCursor.Activate();
      this.updateCursor();
      if (!this.m_wasPickerBuilt)
      {
        this.m_decalPicker.BuildUi(this.m_builder);
        this.m_wasPickerBuilt = true;
      }
      this.m_decalPicker.Show<DecalPickerView>();
      this.m_towerAreasAndDesignatorsActivator.Activate();
      this.m_oceanAreasActivator.Activate();
      Assert.That<TerrainTileSurfaceDecalProto>(this.DecalProto).IsNotNullOrPhantom<TerrainTileSurfaceDecalProto>();
      this.m_decalPlacementToolbox.Show();
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<DecalDesignationController>(this, new Action<GameTime>(this.sync));
      this.m_isActive = true;
    }

    public void Deactivate()
    {
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<DecalDesignationController>(this, new Action<GameTime>(this.sync));
      this.m_decalPlacementToolbox.Hide();
      this.m_terrainCursor.Deactivate();
      this.m_paintCursor.Hide();
      this.m_clearCursor.Hide();
      this.m_cursorMessage.HideMessage();
      this.resetState();
      this.m_towerAreasAndDesignatorsActivator.Deactivate();
      this.m_oceanAreasActivator.Deactivate();
      Action valueOrNull = this.m_onDeactivated.ValueOrNull;
      if (valueOrNull != null)
        valueOrNull();
      this.m_onDeactivated = Option<Action>.None;
      this.m_isActive = false;
      this.m_decalPicker.Hide<DecalPickerView>();
    }

    private void clearPreview()
    {
      this.m_terrainRenderer.SetDecalPreviewData(new RectangleTerrainArea2i(Tile2i.MinValue, RelTile2i.Zero), Rotation90.Deg0, false, TerrainTileSurfaceDecalProto.Phantom, this.ColorKey);
    }

    private void resetState()
    {
      this.clearPreview();
      this.m_ongoingCommand = Option<IInputCommand>.None;
      this.m_previousCursorPosition = new Tile2i?();
      this.m_terrainOutlineRenderer.Hide();
      this.m_state = DecalDesignationController.State.Initial;
      this.m_cameraController.SetMousePanDisabled(false);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_decalPlacementToolbox.PrimaryActionBtn.SetIsOn(this.m_shortcutsManager.IsPrimaryActionOn);
      this.m_decalPlacementToolbox.SecondaryActionBtn.SetIsOn(this.m_shortcutsManager.IsSecondaryActionOn);
      bool flag = false;
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Rotate))
      {
        this.onRotate();
        flag = true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.Flip))
      {
        this.onFlip();
        flag = true;
      }
      DecalDesignationController.State? nullable = this.handleState(inputScheduler);
      if (!nullable.HasValue)
        return flag;
      this.m_state = nullable.Value;
      if (this.m_state == DecalDesignationController.State.Initial && this.m_ongoingCommand.IsNone)
        this.resetState();
      else
        this.m_cameraController.SetMousePanDisabled(this.m_state == DecalDesignationController.State.Designation || this.m_state == DecalDesignationController.State.Undesignation);
      return true;
    }

    private DecalDesignationController.State? handleState(IInputScheduler inputScheduler)
    {
      if (this.m_ongoingCommand.HasValue)
      {
        if (this.m_ongoingCommand.Value.IsProcessedAndSynced)
          this.resetState();
        return new DecalDesignationController.State?(DecalDesignationController.State.Initial);
      }
      switch (this.m_state)
      {
        case DecalDesignationController.State.Initial:
          return this.handleInitialState();
        case DecalDesignationController.State.Designation:
          return this.handleDesignation(inputScheduler);
        case DecalDesignationController.State.Undesignation:
          return this.handleUndesignation(inputScheduler);
        default:
          Assert.Fail(string.Format("Unhandled state {0}", (object) this.m_state));
          return new DecalDesignationController.State?(DecalDesignationController.State.Initial);
      }
    }

    private DecalDesignationController.State? handleInitialState()
    {
      if (!this.m_terrainCursor.HasValue)
        return new DecalDesignationController.State?();
      if (EventSystem.current.IsPointerOverGameObject())
        return new DecalDesignationController.State?();
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.ClearDesignation))
      {
        this.clearPreview();
        if (this.m_previousCursorPosition.HasValue)
        {
          this.m_areaStart = this.m_areaEnd = this.m_previousCursorPosition.GetValueOrDefault();
          this.m_terrainOutlineRenderer.SetArea(new RectangleTerrainArea2i(this.m_areaStart, RelTile2i.One), DecalDesignationController.REMOVE_AREA_COLOR);
          this.m_terrainOutlineRenderer.Show();
        }
        this.m_areaChanged = true;
        this.m_previousCursorPosition = new Tile2i?();
        return new DecalDesignationController.State?(DecalDesignationController.State.Undesignation);
      }
      if (this.m_shortcutsManager.IsPrimaryActionDown && this.m_previousCursorPosition.HasValue)
      {
        this.m_areaStart = this.m_areaEnd = this.m_previousCursorPosition.Value;
        this.m_areaChanged = true;
        this.m_previousCursorPosition = new Tile2i?();
        return new DecalDesignationController.State?(DecalDesignationController.State.Designation);
      }
      if (!this.m_shortcutsManager.IsSecondaryActionDown || this.m_previousCursorPosition.HasValue)
        return new DecalDesignationController.State?();
      this.m_errorSound.Play();
      return new DecalDesignationController.State?(DecalDesignationController.State.Initial);
    }

    private DecalDesignationController.State? handleDesignation(IInputScheduler inputScheduler)
    {
      if (this.m_shortcutsManager.IsSecondaryActionDown)
        return new DecalDesignationController.State?(DecalDesignationController.State.Initial);
      if (this.m_shortcutsManager.IsPrimaryActionUp)
      {
        if (!this.m_isInitialCursorOnValidSurface)
        {
          this.m_errorSound.Play();
          return new DecalDesignationController.State?(DecalDesignationController.State.Initial);
        }
        this.m_ongoingCommand = (Option<IInputCommand>) (IInputCommand) inputScheduler.ScheduleInputCmd<AddSurfaceDecalCmd>(new AddSurfaceDecalCmd((Proto.ID) this.DecalProto.Id, this.m_previewArea, this.m_rotation, this.m_flipped, this.ColorKey));
        if (this.m_previewArea.AreaTiles == 1)
          this.m_applySound.Play();
        else
          this.m_applySoundLong.Play();
        return new DecalDesignationController.State?(DecalDesignationController.State.Initial);
      }
      if (this.computeDesignationEnd(this.m_terrainCursor.Tile2iClampedToLimits, DecalDesignationController.MAX_AREA_SIZE_ADD))
        this.m_areaChanged = true;
      return new DecalDesignationController.State?(DecalDesignationController.State.Designation);
    }

    private DecalDesignationController.State? handleUndesignation(IInputScheduler inputScheduler)
    {
      if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.ClearDesignation))
      {
        this.m_ongoingCommand = (Option<IInputCommand>) (IInputCommand) inputScheduler.ScheduleInputCmd<RemoveSurfaceDecalCmd>(new RemoveSurfaceDecalCmd(this.m_previewArea));
        this.m_removeSound.Play();
        return new DecalDesignationController.State?(DecalDesignationController.State.Initial);
      }
      if (this.m_shortcutsManager.IsPrimaryActionUp)
        return new DecalDesignationController.State?(DecalDesignationController.State.Initial);
      if (this.computeDesignationEnd(this.m_terrainCursor.Tile2iClampedToLimits, DecalDesignationController.MAX_AREA_SIZE_REMOVE))
      {
        this.m_terrainOutlineRenderer.SetArea(RectangleTerrainArea2i.FromTwoPositions(this.m_areaStart, this.m_areaEnd), DecalDesignationController.REMOVE_AREA_COLOR);
        this.m_areaChanged = true;
      }
      return new DecalDesignationController.State?(DecalDesignationController.State.Undesignation);
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
      if ((previousCursorPosition.HasValue ? (previousCursorPosition.GetValueOrDefault() == tile2i ? 1 : 0) : 0) != 0 && !this.m_flipRotateChanged)
        return;
      this.m_flipRotateChanged = false;
      this.m_previewErrorMsg = new LocStrFormatted?();
      if (this.m_state == DecalDesignationController.State.Initial)
      {
        this.clearPreview();
        if (this.m_terrainCursor.HasValue)
        {
          this.m_initialCursorPosition = new Tile2i?(tile2iClampedToLimits);
          if (this.m_terrainManager.TryGetTileSurface(this.m_terrainManager.GetTileIndex(tile2iClampedToLimits), out TileSurfaceData _))
          {
            this.m_isInitialCursorOnValidSurface = true;
            this.previewInitialDesignationAt(tile2iClampedToLimits, out this.m_previewErrorMsg);
          }
          else
          {
            this.m_isInitialCursorOnValidSurface = false;
            this.m_previewErrorMsg = new LocStrFormatted?((LocStrFormatted) TrCore.DesignationWarning__CannotPlaceDecal);
          }
        }
        else
        {
          this.m_isInitialCursorOnValidSurface = false;
          this.m_previewErrorMsg = new LocStrFormatted?((LocStrFormatted) TrCore.DesignationError__Invalid);
        }
      }
      else if (this.m_state == DecalDesignationController.State.Designation)
      {
        Assert.That<Tile2i?>(this.m_initialCursorPosition).HasValue<Tile2i>();
        if (this.m_areaChanged && this.m_initialCursorPosition.HasValue && this.m_isInitialCursorOnValidSurface)
        {
          this.m_previewArea = RectangleTerrainArea2i.FromTwoPositions(this.m_initialCursorPosition.Value, tile2iClampedToLimits);
          this.m_terrainRenderer.SetDecalPreviewData(this.m_previewArea, this.m_rotation, this.m_flipped, this.DecalProto, this.ColorKey);
        }
      }
      else if (this.m_state == DecalDesignationController.State.Undesignation)
      {
        Assert.That<Tile2i?>(this.m_initialCursorPosition).HasValue<Tile2i>();
        if (this.m_areaChanged && this.m_initialCursorPosition.HasValue)
        {
          this.m_previewArea = RectangleTerrainArea2i.FromTwoPositions(this.m_initialCursorPosition.Value, tile2iClampedToLimits);
          this.m_terrainRenderer.SetDecalPreviewData(this.m_previewArea, Rotation90.Deg0, false, TerrainTileSurfaceDecalProto.Phantom, this.ColorKey);
        }
      }
      this.m_previousCursorPosition = new Tile2i?(tile2iClampedToLimits);
      this.updateCursor();
    }

    private void previewInitialDesignationAt(Tile2i position, out LocStrFormatted? error)
    {
      Assert.That<TerrainTileSurfaceDecalProto>(this.DecalProto).IsNotNull<TerrainTileSurfaceDecalProto>();
      this.m_previewArea = new RectangleTerrainArea2i(position, RelTile2i.One);
      this.m_terrainRenderer.SetDecalPreviewData(this.m_previewArea, this.m_rotation, this.m_flipped, this.DecalProto, this.ColorKey);
      error = new LocStrFormatted?();
    }

    private void updateCursor()
    {
      if (this.m_previewErrorMsg.HasValue)
        this.m_cursorMessage.ShowMessage(this.m_previewErrorMsg.Value.Value);
      else
        this.m_cursorMessage.HideMessage();
      if (EventSystem.current.IsPointerOverGameObject())
      {
        this.m_clearCursor.Hide();
        this.m_paintCursor.Hide();
      }
      else if (this.m_state == DecalDesignationController.State.Undesignation)
      {
        this.m_clearCursor.Show();
        this.m_paintCursor.Hide();
      }
      else
      {
        this.m_clearCursor.Hide();
        if (this.m_state == DecalDesignationController.State.Initial && this.m_previewErrorMsg.HasValue)
          this.m_paintCursor.Hide();
        else
          this.m_paintCursor.Show();
      }
    }

    static DecalDesignationController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      DecalDesignationController.MAX_AREA_SIZE_ADD = new RelTile1i((int) sbyte.MaxValue);
      DecalDesignationController.MAX_AREA_SIZE_REMOVE = new RelTile1i(191);
      DecalDesignationController.REMOVE_AREA_COLOR = Color.red;
    }

    private enum State
    {
      Initial,
      Designation,
      Undesignation,
    }
  }
}
