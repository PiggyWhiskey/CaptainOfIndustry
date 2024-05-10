// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.TransportBuildController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Ports;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Unity.Audio;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.Mine;
using Mafi.Unity.Ports.Io;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  /// <summary>User interface for transports laying.</summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TransportBuildController : IUnityUi
  {
    private float MIN_MOUSE_DELTA_PX;
    private static readonly ColorRgba SELECTION_OK;
    private static readonly ColorRgba SELECTION_NOT_OK;
    private readonly ProtosDb m_protosDb;
    private readonly TerrainCursor m_terrainCursor;
    private readonly CursorPickingManager m_picker;
    private readonly IoPortsRenderer m_portsRenderer;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly AudioDb m_audioDb;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly CursorManager m_cursorManager;
    private readonly PathFindingTransportPreview m_transportPreview;
    private readonly ImmutableArray<IoPortShapeProto> m_managedPortShapes;
    private readonly IActivator m_towerAreasAndDesignatorsActivator;
    private readonly Lyst<IoPortShapeProto> m_unlockedPortShapes;
    private bool m_unlockedTransportsChanged;
    private Option<TransportProto> m_currTransportProto;
    private readonly TransportMenuToolbox m_toolbox;
    private readonly TerrainCursorVisual m_terrainCursorVisual;
    public readonly Predicate<IoPort> NonConnectedAndUnlockedPortsPredicate;
    private TransportBuildController.State m_state;
    private Option<BuildTransportCmd> m_ongoingCmd;
    private readonly Stak<TransportBuildController.HistoryEntry> m_pivotsHistory;
    private TransportPathFinderFlags m_invertTieBreaking;
    private TransportPathFinderFlags m_onlyStraightTransports;
    private TransportPathFinderFlags m_banTiledInFrontOfPorts;
    private bool m_disableSnapping;
    private Direction903d? m_startDirection;
    private bool m_leftMouseClickBuffer;
    private readonly Lyst<Direction903d> m_tmpBannedStartDirections;
    /// <summary>
    /// This map is used to assign transport to shape. Since more transport types can have the same shape this map
    /// tracks last used transport for each shape.
    /// </summary>
    private readonly Dict<IoPortShapeProto, Option<TransportProto>> m_shapeToTransportMap;
    private Cursoor m_cursor;
    private AudioSource m_addTransportSegmentSound;
    private AudioSource m_buildingPlacedSound;
    private AudioSource m_invalidOpSound;
    private AudioSource m_invalidClickSound;
    private AudioSource m_unselectEntitySound;
    private AudioSource m_bindTransportSound;
    private AudioSource m_unbindTransportSound;
    private Vector3 m_lastMousePos;
    private Tile2i m_lastMouseTile;
    private TransportBuildController.PickResult m_previousPickResult;
    private HighlightId? m_currentPortsHighlight;
    private Predicate<IoPort> m_compatiblePortPredicate;
    private Option<IStaticEntity> m_onlyForEntity;

    public bool IsActive { get; private set; }

    public bool TransportProtoSelected => this.m_currTransportProto != (TransportProto) null;

    public bool ShouldShowDisableSnappingInfo
    {
      get => this.IsActive && this.TransportProtoSelected && this.m_disableSnapping;
    }

    public event Action<Option<TransportProto>> OnProtoSelected;

    public TransportBuildController(
      IGameLoopEvents gameLoopEvents,
      NewInstanceOf<TerrainCursor> terrainCursor,
      CursorPickingManager picker,
      IoPortsRenderer portsRenderer,
      ProtosDb protosDb,
      ShortcutsManager shortcutsManager,
      CursorManager cursorManager,
      AudioDb audioDb,
      NewInstanceOf<PathFindingTransportPreview> transportPreview,
      UnlockedProtosDb unlockedProtosDb,
      TransportMenuToolbox toolbox,
      TerrainCursorVisual terrainCursorVisual,
      TowerAreasRenderer towerAreasRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.MIN_MOUSE_DELTA_PX = 5f;
      this.m_unlockedPortShapes = new Lyst<IoPortShapeProto>();
      this.m_pivotsHistory = new Stak<TransportBuildController.HistoryEntry>();
      this.m_banTiledInFrontOfPorts = TransportPathFinderFlags.BanTilesInFrontOfPorts;
      this.m_tmpBannedStartDirections = new Lyst<Direction903d>(true);
      this.m_shapeToTransportMap = new Dict<IoPortShapeProto, Option<TransportProto>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_terrainCursor = terrainCursor.Instance;
      this.m_picker = picker;
      this.m_portsRenderer = portsRenderer;
      this.m_shortcutsManager = shortcutsManager;
      this.m_cursorManager = cursorManager;
      this.m_audioDb = audioDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_toolbox = toolbox;
      this.m_terrainCursorVisual = terrainCursorVisual;
      this.m_towerAreasAndDesignatorsActivator = towerAreasRenderer.CreateCombinedActivatorWithTerrainDesignatorsAndGrid();
      this.m_transportPreview = transportPreview.Instance;
      this.m_managedPortShapes = protosDb.All<IoPortShapeProto>().ToImmutableArray<IoPortShapeProto>();
      this.m_unlockedTransportsChanged = true;
      this.m_unlockedProtosDb.OnUnlockedSetChanged.AddNonSaveable<TransportBuildController>(this, (Action) (() => this.m_unlockedTransportsChanged = true));
      this.NonConnectedAndUnlockedPortsPredicate = (Predicate<IoPort>) (x => x.IsNotConnected && this.m_unlockedPortShapes.Contains(x.ShapePrototype));
      gameLoopEvents.SyncUpdate.AddNonSaveable<TransportBuildController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_toolbox.SetOnTransportUp(Option<Func<bool>>.Some(new Func<bool>(this.transportUp)));
      this.m_toolbox.SetOnTransportDown(Option<Func<bool>>.Some(new Func<bool>(this.transportDown)));
      this.m_toolbox.SetOnTieBreak(Option<Action>.Some(new Action(this.toggleInvertTieBreaking)));
      this.m_toolbox.SetOnOnlyStraight(Option<Action<bool>>.Some(new Action<bool>(this.setOnlyStraight)));
      this.m_toolbox.SetOnToggleOnlyStraight(Option<Action>.Some(new Action(this.toggleOnlyStraight)));
      this.m_toolbox.SetOnToggleSnapping(Option<Action>.Some(new Action(this.togglePortSnapping)));
      this.m_toolbox.SetOnTogglePortsBlocking(Option<Action>.Some(new Action(this.togglePortsBlocking)));
      this.m_toolbox.SetOnTogglePricePopup(Option<Action>.Some(new Action(this.togglePricePopup)));
    }

    private void syncUpdate(GameTime time)
    {
      if (!this.m_unlockedTransportsChanged)
        return;
      this.m_unlockedTransportsChanged = false;
      this.m_unlockedPortShapes.Clear();
      this.m_unlockedPortShapes.AddRange(this.m_unlockedProtosDb.AllUnlocked<TransportProto>().Select<TransportProto, IoPortShapeProto>((Func<TransportProto, IoPortShapeProto>) (p => p.PortsShape)));
    }

    public void RegisterUi(UiBuilder builder)
    {
      Lyst<TransportProto> transportsCandidates = this.m_protosDb.All<TransportProto>().ToLyst<TransportProto>((Predicate<TransportProto>) (t => t.IsBuildable));
      Lyst<TransportProto> lyst = transportsCandidates.Where<TransportProto>((Func<TransportProto, bool>) (x => !transportsCandidates.Any<TransportProto>((Predicate<TransportProto>) (y => y.Upgrade.NextTier == x)))).ToLyst<TransportProto>();
      int index = 0;
      for (int length = this.m_managedPortShapes.Length; index < length; ++index)
      {
        IoPortShapeProto portShape = this.m_managedPortShapes[index];
        TransportProto[] array = lyst.Where<TransportProto>((Func<TransportProto, bool>) (t => t.IsBuildable && (Proto) t.PortsShape == (Proto) portShape)).ToArray<TransportProto>();
        this.m_shapeToTransportMap[portShape] = array.IsEmpty<TransportProto>() ? Option<TransportProto>.None : (Option<TransportProto>) ((IEnumerable<TransportProto>) array).MinElement<TransportProto, int>((Func<TransportProto, int>) (t => t.Strings.Name.TranslatedString.Length));
      }
      this.m_cursor = this.m_cursorManager.RegisterCursor(builder.Style.Cursors.Build);
      this.m_invalidClickSound = this.m_audioDb.GetSharedAudio(builder.Audio.InvalidOp);
      this.m_unselectEntitySound = this.m_audioDb.GetSharedAudio(builder.Audio.EntityUnselect);
      this.m_bindTransportSound = this.m_audioDb.GetSharedAudio(builder.Audio.TransportBind);
      this.m_unbindTransportSound = this.m_audioDb.GetSharedAudio(builder.Audio.TransportUnbind);
      this.m_addTransportSegmentSound = this.m_audioDb.GetSharedAudio(builder.Audio.TransportBind);
      this.m_invalidOpSound = this.m_audioDb.GetSharedAudio(builder.Audio.InvalidOp);
    }

    /// <summary>Called when user selects an item in the menu.</summary>
    public void SelectProto(TransportProto transportProto)
    {
      Assert.That<bool>(this.IsActive).IsTrue();
      this.cancelTransport(false);
      this.m_toolbox.Show();
      this.m_cursor.Show();
      this.setTransportProto(transportProto);
      this.resetState(TransportBuildController.State.SelectingFirstPivot);
    }

    private void setTransportProto(TransportProto newProto)
    {
      this.m_currTransportProto = (Option<TransportProto>) newProto;
      this.m_shapeToTransportMap[newProto.PortsShape] = (Option<TransportProto>) newProto;
      this.m_buildingPlacedSound = this.m_audioDb.GetSharedAudio(newProto.Graphics.SoundOnBuildPrefabPath, AudioChannel.UserInterface);
    }

    public void Activate(IStaticEntity onlyForEntity = null)
    {
      Assert.That<Option<TransportProto>>(this.m_currTransportProto).IsNone<TransportProto>();
      Assert.That<HighlightId?>(this.m_currentPortsHighlight).IsNull<HighlightId>();
      if (this.IsActive)
      {
        Log.Warning("TransportBuildController is already activated!");
      }
      else
      {
        this.IsActive = true;
        this.resetState(TransportBuildController.State.NoProtoSelected);
        this.m_onlyForEntity = Option.Create<IStaticEntity>(onlyForEntity);
        this.m_transportPreview.Activate();
        this.m_lastMousePos = Vector3.zero;
        this.m_invertTieBreaking = TransportPathFinderFlags.None;
        this.m_onlyStraightTransports = TransportPathFinderFlags.None;
        this.m_toolbox.DisplayTieBreakActive(false);
        this.m_toolbox.DisplayOnlyStraightActive(false);
        this.m_toolbox.DisplaySnappingDisabled(this.m_disableSnapping);
        this.m_toolbox.DisplayPortsBlockingDisabled(this.m_banTiledInFrontOfPorts != TransportPathFinderFlags.BanTilesInFrontOfPorts);
        this.m_toolbox.DisplayPricePopupDisabled(this.m_transportPreview.PricePopupIsHidden);
        this.m_previousPickResult = new TransportBuildController.PickResult();
        this.m_terrainCursor.Activate();
        this.m_terrainCursor.RelativeHeight = ThicknessTilesI.Zero;
      }
    }

    public void Deactivate()
    {
      if (!this.IsActive)
      {
        Log.Warning("TransportBuildController is already deactivated!");
      }
      else
      {
        this.IsActive = false;
        this.cancelTransport(false);
        this.m_transportPreview.Deactivate();
        this.m_terrainCursorVisual.Deactivate();
        this.m_terrainCursor.Deactivate();
        this.m_towerAreasAndDesignatorsActivator.DeactivateIfActive();
        if (this.m_currentPortsHighlight.HasValue)
        {
          this.m_portsRenderer.ClearPortsHighlight(this.m_currentPortsHighlight.Value);
          this.m_currentPortsHighlight = new HighlightId?();
          this.m_compatiblePortPredicate = (Predicate<IoPort>) null;
        }
        this.m_toolbox.Hide();
        this.m_onlyForEntity = Option<IStaticEntity>.None;
      }
    }

    /// <summary>
    /// Sets state to given value and clears preview and other state variables.
    /// </summary>
    private void resetState(TransportBuildController.State newState)
    {
      this.m_ongoingCmd = Option<BuildTransportCmd>.None;
      this.m_pivotsHistory.Clear();
      if (this.m_currentPortsHighlight.HasValue)
      {
        this.m_portsRenderer.ClearPortsHighlight(this.m_currentPortsHighlight.Value);
        this.m_currentPortsHighlight = new HighlightId?();
        this.m_compatiblePortPredicate = (Predicate<IoPort>) null;
      }
      this.m_picker.ClearPicked();
      this.m_terrainCursorVisual.Hide();
      this.m_state = newState;
      this.m_towerAreasAndDesignatorsActivator.SetActive(newState != 0);
    }

    public void CancelTransport()
    {
      this.cancelTransport(true);
      this.m_unselectEntitySound.Play();
      this.m_transportPreview.Clear();
    }

    /// <summary>
    /// Cancels selected prototype and resets state to <see cref="F:Mafi.Unity.InputControl.Factory.TransportBuildController.State.NoProtoSelected" />.
    /// </summary>
    private void cancelTransport(bool invokeEvents)
    {
      this.m_terrainCursor.RelativeHeight = ThicknessTilesI.Zero;
      this.m_currTransportProto = Option<TransportProto>.None;
      if (invokeEvents)
      {
        Action<Option<TransportProto>> onProtoSelected = this.OnProtoSelected;
        if (onProtoSelected != null)
          onProtoSelected((Option<TransportProto>) Option.None);
      }
      this.resetState(TransportBuildController.State.NoProtoSelected);
      this.m_toolbox.Hide();
      this.m_cursor.Hide();
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
      {
        if (!this.m_currTransportProto.HasValue)
          return false;
        this.CancelTransport();
        return true;
      }
      if (this.m_shortcutsManager.IsPrimaryActionUp && EventSystem.current.IsPointerOverGameObject())
        return false;
      bool flag = false;
      if (this.m_currTransportProto.HasValue)
      {
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.LowerDown))
        {
          this.m_toolbox.OnDown();
          flag = true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.RaiseUp))
        {
          this.m_toolbox.OnUp();
          flag = true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.TransportTieBreak))
        {
          this.m_toolbox.OnTieBreak();
          flag = true;
        }
        if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.TransportTieBreak))
        {
          this.m_toolbox.OnTieBreak();
          flag = true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.TransportNoTurn))
        {
          this.m_toolbox.OnSetOnlyStraight(true);
          flag = true;
        }
        if (this.m_shortcutsManager.IsUp(this.m_shortcutsManager.TransportNoTurn))
        {
          this.m_toolbox.OnSetOnlyStraight(false);
          flag = true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.TransportSnapping))
        {
          this.m_toolbox.OnPortSnapping();
          flag = true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.TransportPortsBlocking))
        {
          this.m_toolbox.OnTogglePortsBlocking();
          flag = true;
        }
        if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.TogglePricePopup))
        {
          this.m_toolbox.OnTogglePricePopup();
          flag = true;
        }
      }
      TransportBuildController.State? nullable1 = this.handleState(inputScheduler);
      if (!nullable1.HasValue)
        return flag;
      TransportBuildController.State? nullable2 = nullable1;
      TransportBuildController.State state = this.m_state;
      if (!(nullable2.GetValueOrDefault() == state & nullable2.HasValue))
      {
        if (this.m_currentPortsHighlight.HasValue)
        {
          this.m_portsRenderer.ClearPortsHighlight(this.m_currentPortsHighlight.Value);
          this.m_currentPortsHighlight = new HighlightId?();
          this.m_compatiblePortPredicate = (Predicate<IoPort>) null;
        }
        this.m_state = nullable1.Value;
        this.m_leftMouseClickBuffer = false;
        this.m_towerAreasAndDesignatorsActivator.SetActive(nullable1.Value != TransportBuildController.State.NoProtoSelected);
      }
      return true;
    }

    private TransportBuildController.PickResult pickAtCursorPosition(
      bool allowEntityPortSnapping,
      bool ignoreDisabledSnapping,
      bool disableTransportHover = false)
    {
      Vector3 mousePosition = UnityEngine.Input.mousePosition;
      Tile2i tile2i = this.m_terrainCursor.Tile2i;
      Vector3 vector3 = mousePosition - this.m_lastMousePos;
      if ((double) vector3.x * (double) vector3.x + (double) vector3.y * (double) vector3.y < (double) this.MIN_MOUSE_DELTA_PX * (double) this.MIN_MOUSE_DELTA_PX && tile2i == this.m_lastMouseTile)
        return this.m_previousPickResult;
      this.m_lastMousePos = mousePosition;
      this.m_lastMouseTile = tile2i;
      if (ignoreDisabledSnapping || !this.m_disableSnapping)
      {
        Option<IRenderedEntity> option = this.m_picker.PickEntity<IRenderedEntity>();
        if (option.HasValue && option.Value is Mafi.Core.Factory.Transports.Transport transport)
        {
          if (!disableTransportHover && (this.m_currTransportProto.IsNone || (Proto) this.m_currTransportProto.ValueOrNull.PortsShape == (Proto) transport.Prototype.PortsShape) && transport.OccupiedTiles.Contains(this.m_picker.LastPickedTile.Tile3i - transport.CenterTile))
            return this.m_previousPickResult = new TransportBuildController.PickResult(this.m_picker.LastPickedTile.Tile3i, (Option<TransportProto>) transport.Prototype, mustBeFlat: true);
        }
        else
        {
          IoPort port;
          if (this.pickAndSelectConnectablePort(out port) || allowEntityPortSnapping && option.ValueOrNull is IEntityWithPorts valueOrNull && this.tryFindClosestMatchingPort(valueOrNull, this.m_terrainCursor.Tile3i, out port))
          {
            Assert.That<bool>(this.m_currTransportProto.IsNone || (Proto) this.m_currTransportProto.Value.PortsShape == (Proto) port.ShapePrototype).IsTrue();
            return this.m_previousPickResult = new TransportBuildController.PickResult(port.ExpectedConnectedPortCoord, (Option<TransportProto>) this.m_shapeToTransportMap[port.ShapePrototype].Value, new Direction903d?(-port.Direction));
          }
        }
      }
      HeightTilesI nonCollidingHeight = TransportHelper.GetLowestNonCollidingHeight(this.m_terrainCursor.Tile);
      Tile3i position = this.m_terrainCursor.Tile3i;
      if (position.Height < nonCollidingHeight)
        position = position.SetZ(nonCollidingHeight.Value);
      return this.m_previousPickResult = new TransportBuildController.PickResult(position);
    }

    private bool tryFindClosestMatchingPort(IEntityWithPorts ewp, Tile3i position, out IoPort port)
    {
      Assert.That<Predicate<IoPort>>(this.m_compatiblePortPredicate).IsNotNull<Predicate<IoPort>>();
      port = (IoPort) null;
      long num1 = long.MaxValue;
      foreach (IoPort port1 in ewp.Ports)
      {
        if (this.m_compatiblePortPredicate(port1) && this.m_unlockedPortShapes.Contains(port1.ShapePrototype))
        {
          long num2 = port1.Position.DistanceSqrTo(position);
          if (num2 < num1)
          {
            num1 = num2;
            port = port1;
          }
        }
      }
      return port != null;
    }

    private TransportBuildController.State? handleState(IInputScheduler inputScheduler)
    {
      switch (this.m_state)
      {
        case TransportBuildController.State.NoProtoSelected:
          return this.handleNoProtoSelected();
        case TransportBuildController.State.SelectingFirstPivot:
          return this.handleSelectingFirstPivot();
        case TransportBuildController.State.SelectingContinuationPivot:
          return this.handleSelectingContinuationPivot(inputScheduler);
        case TransportBuildController.State.AwaitingTransportCreation:
          return this.handleAwaitingTransportCreation();
        default:
          Assert.Fail(string.Format("Invalid state '{0}'.", (object) this.m_state));
          return new TransportBuildController.State?(this.m_state);
      }
    }

    private TransportBuildController.State? handleNoProtoSelected()
    {
      Assert.That<Option<TransportProto>>(this.m_currTransportProto).IsNone<TransportProto>();
      Assert.That<bool>(this.m_pivotsHistory.IsEmpty).IsTrue();
      if (!this.m_currentPortsHighlight.HasValue)
      {
        this.m_compatiblePortPredicate = this.NonConnectedAndUnlockedPortsPredicate;
        this.m_currentPortsHighlight = new HighlightId?(this.m_onlyForEntity.HasValue ? this.m_portsRenderer.HighlightAllPortsOf(this.m_onlyForEntity.Value, this.m_compatiblePortPredicate) : this.m_portsRenderer.HighlightPorts(this.m_compatiblePortPredicate));
      }
      if (EventSystem.current.IsPointerOverGameObject())
        return new TransportBuildController.State?();
      TransportBuildController.PickResult pickResult = this.pickAtCursorPosition(false, true, true);
      if (pickResult.TransportProto.IsNone)
      {
        this.m_transportPreview.Clear();
        return new TransportBuildController.State?();
      }
      TransportPathFinderOptions pathFinderOptions = new TransportPathFinderOptions(flags: this.m_invertTieBreaking | this.m_onlyStraightTransports | this.m_banTiledInFrontOfPorts);
      PathFindingTransportPreview.PreviewRequest startRequest = PathFindingTransportPreview.PreviewRequest.CreateStartRequest(pickResult.TransportProto.Value, pickResult.Position, pathFinderOptions, pickResult.Direction, this.m_disableSnapping);
      Option<TransportTrajectory> successTrajectory;
      bool stillWorking;
      bool flag = this.m_transportPreview.ShowStartPreview(startRequest, this.m_terrainCursor.RelativeHeight, out this.m_startDirection, out successTrajectory, out stillWorking);
      if (!this.m_shortcutsManager.IsPrimaryActionUp && !this.m_leftMouseClickBuffer)
        return new TransportBuildController.State?();
      this.m_leftMouseClickBuffer = false;
      if (!flag)
      {
        if (stillWorking)
          this.m_leftMouseClickBuffer = true;
        else
          this.m_invalidClickSound.Play();
        return new TransportBuildController.State?(TransportBuildController.State.NoProtoSelected);
      }
      this.pushTrajToHistoryStack(ImmutableArray.Create<Tile3i>(startRequest.NewPosition), ImmutableArray<Tile2i>.Empty, successTrajectory, pickResult.ForcedDirection, pickResult.MustBeFlat);
      this.setTransportProto(startRequest.Proto);
      Action<Option<TransportProto>> onProtoSelected = this.OnProtoSelected;
      if (onProtoSelected != null)
        onProtoSelected((Option<TransportProto>) this.m_currTransportProto.Value);
      this.m_toolbox.Show();
      this.m_cursor.Show();
      this.m_bindTransportSound.Play();
      return new TransportBuildController.State?(TransportBuildController.State.SelectingContinuationPivot);
    }

    private void pushTrajToHistoryStack(
      ImmutableArray<Tile3i> pivots,
      ImmutableArray<Tile2i> pillarHints,
      Option<TransportTrajectory> trajectory,
      Direction903d? nextForcedStartDirection,
      bool nextStartMustBeFlat)
    {
      ImmutableArray<Tile3i> bannedTiles = trajectory.HasValue ? trajectory.Value.OccupiedTiles.ToTile3iArray() : ImmutableArray<Tile3i>.Empty;
      this.m_pivotsHistory.Push(new TransportBuildController.HistoryEntry(pivots, pillarHints, trajectory, bannedTiles, nextForcedStartDirection, nextStartMustBeFlat));
    }

    private TransportBuildController.State? handleSelectingFirstPivot()
    {
      Assert.That<Option<TransportProto>>(this.m_currTransportProto).HasValue<TransportProto>();
      Assert.That<bool>(this.m_pivotsHistory.IsEmpty).IsTrue();
      if (this.m_shortcutsManager.IsSecondaryActionUp)
      {
        this.cancelTransport(true);
        this.m_unselectEntitySound.Play();
        this.m_transportPreview.Clear();
        return new TransportBuildController.State?(TransportBuildController.State.NoProtoSelected);
      }
      if (!this.m_currentPortsHighlight.HasValue)
      {
        IoPortShapeProto portShapeLocal = this.m_currTransportProto.Value.PortsShape;
        this.m_compatiblePortPredicate = (Predicate<IoPort>) (x => !x.IsConnected && (Proto) x.ShapePrototype == (Proto) portShapeLocal);
        this.m_currentPortsHighlight = new HighlightId?(this.m_portsRenderer.HighlightPorts(this.m_compatiblePortPredicate));
      }
      TransportBuildController.PickResult pickResult = this.m_leftMouseClickBuffer ? this.m_previousPickResult : this.pickAtCursorPosition(true, false);
      TransportPathFinderOptions pathFinderOptions = new TransportPathFinderOptions(flags: this.m_invertTieBreaking | this.m_onlyStraightTransports | this.m_banTiledInFrontOfPorts);
      PathFindingTransportPreview.PreviewRequest startRequest = PathFindingTransportPreview.PreviewRequest.CreateStartRequest(this.m_currTransportProto.Value, pickResult.Position, pathFinderOptions, pickResult.Direction, this.m_disableSnapping);
      Option<TransportTrajectory> successTrajectory;
      bool stillWorking;
      bool flag = this.m_transportPreview.ShowStartPreview(startRequest, this.m_terrainCursor.RelativeHeight, out this.m_startDirection, out successTrajectory, out stillWorking);
      if (!this.m_shortcutsManager.IsPrimaryActionUp && !this.m_leftMouseClickBuffer)
        return new TransportBuildController.State?();
      this.m_leftMouseClickBuffer = false;
      if (!flag)
      {
        if (stillWorking)
          this.m_leftMouseClickBuffer = true;
        else
          this.m_invalidClickSound.Play();
        return new TransportBuildController.State?(TransportBuildController.State.SelectingFirstPivot);
      }
      this.pushTrajToHistoryStack(ImmutableArray.Create<Tile3i>(startRequest.NewPosition), ImmutableArray<Tile2i>.Empty, successTrajectory, pickResult.ForcedDirection, pickResult.MustBeFlat);
      this.m_addTransportSegmentSound.Play();
      return new TransportBuildController.State?(TransportBuildController.State.SelectingContinuationPivot);
    }

    private TransportBuildController.State? handleSelectingContinuationPivot(
      IInputScheduler inputScheduler)
    {
      Assert.That<Option<TransportProto>>(this.m_currTransportProto).HasValue<TransportProto>();
      Assert.That<bool>(this.m_pivotsHistory.IsNotEmpty).IsTrue();
      if (!this.m_currentPortsHighlight.HasValue)
      {
        IoPortShapeProto portShapeLocal = this.m_currTransportProto.Value.PortsShape;
        this.m_compatiblePortPredicate = (Predicate<IoPort>) (x => !x.IsConnected && (Proto) x.ShapePrototype == (Proto) portShapeLocal);
        this.m_currentPortsHighlight = new HighlightId?(this.m_portsRenderer.HighlightPorts(this.m_compatiblePortPredicate));
      }
      TransportBuildController.PickResult pickResult = this.m_leftMouseClickBuffer ? this.m_previousPickResult : this.pickAtCursorPosition(true, false);
      TransportBuildController.HistoryEntry first = this.m_pivotsHistory.First;
      TransportPathFinderFlags transportPathFinderFlags = this.m_invertTieBreaking | this.m_onlyStraightTransports | this.m_banTiledInFrontOfPorts;
      if (first.NextStartMustBeFlat)
        transportPathFinderFlags |= TransportPathFinderFlags.StartMustBeFlat;
      if (pickResult.MustBeFlat)
        transportPathFinderFlags |= TransportPathFinderFlags.GoalMustBeFlat;
      Direction903d? forcedStartDirection = first.NextForcedStartDirection;
      this.m_tmpBannedStartDirections.Clear();
      Tile3i tile3i;
      if (first.Pivots.Length >= 2)
      {
        tile3i = first.Pivots.PreLast;
        Tile2i xy1 = tile3i.Xy;
        tile3i = first.Pivots.Last;
        Tile2i xy2 = tile3i.Xy;
        RelTile2i relTile2i = xy1 - xy2;
        if (relTile2i.IsNotZero)
        {
          this.m_tmpBannedStartDirections.Add(relTile2i.ToDirection90().As3d);
          if (relTile2i.X == 0)
            transportPathFinderFlags |= TransportPathFinderFlags.BanStartRampsInX;
          if (relTile2i.Y == 0)
            transportPathFinderFlags |= TransportPathFinderFlags.BanStartRampsInY;
          if (first.Pivots.PreLast.Z != first.Pivots.Last.Z)
          {
            Lyst<Direction903d> bannedStartDirections1 = this.m_tmpBannedStartDirections;
            Direction90 direction90_1 = relTile2i.ToDirection90();
            direction90_1 = direction90_1.RotatedPlus90;
            Direction903d as3d1 = direction90_1.As3d;
            bannedStartDirections1.Add(as3d1);
            Lyst<Direction903d> bannedStartDirections2 = this.m_tmpBannedStartDirections;
            Direction90 direction90_2 = relTile2i.ToDirection90();
            direction90_2 = direction90_2.RotatedMinus90;
            Direction903d as3d2 = direction90_2.As3d;
            bannedStartDirections2.Add(as3d2);
          }
        }
        else
          this.m_tmpBannedStartDirections.Add((first.Pivots.PreLast - first.Pivots.Last).ToDirection903d());
      }
      else if (this.m_startDirection.HasValue)
      {
        Vector3i directionVector = this.m_startDirection.Value.DirectionVector;
        if (directionVector.X == 0)
          transportPathFinderFlags |= TransportPathFinderFlags.BanStartRampsInX;
        if (directionVector.Y == 0)
          transportPathFinderFlags |= TransportPathFinderFlags.BanStartRampsInY;
      }
      Tile3i newPosition = pickResult.Position;
      ThicknessTilesI relativeHeight = this.m_terrainCursor.RelativeHeight;
      if (!this.m_currTransportProto.Value.CanGoUpDown && this.m_pivotsHistory.IsNotEmpty)
        newPosition = newPosition.SetZ(this.m_pivotsHistory.First.Pivots.First.Z);
      TransportPathFinderFlags flags = transportPathFinderFlags;
      TransportPathFinderOptions pathFinderOptions = new TransportPathFinderOptions(forcedStartDirection: forcedStartDirection, bannedStartDirections: this.m_tmpBannedStartDirections.ToImmutableArray(), flags: flags);
      Option<TransportTrajectory> successTrajectory;
      bool stillWorking;
      bool shouldFinishBuild;
      Option<CanBuildTransportResult> option = this.m_transportPreview.ShowContinuationPreview(PathFindingTransportPreview.PreviewRequest.CreateContRequest(this.m_currTransportProto.Value, newPosition, first.Pivots, first.PillarHints, first.Trajectory, pathFinderOptions, first.BannedTiles, this.m_startDirection, this.m_disableSnapping), relativeHeight, out successTrajectory, out stillWorking, out shouldFinishBuild);
      if (this.m_shortcutsManager.IsSecondaryActionUp)
      {
        this.m_pivotsHistory.Pop();
        this.m_unbindTransportSound.Play();
        this.m_leftMouseClickBuffer = false;
        return new TransportBuildController.State?(this.m_pivotsHistory.IsEmpty ? TransportBuildController.State.SelectingFirstPivot : TransportBuildController.State.SelectingContinuationPivot);
      }
      if (!this.m_shortcutsManager.IsPrimaryActionUp && !this.m_leftMouseClickBuffer)
        return new TransportBuildController.State?();
      this.m_leftMouseClickBuffer = false;
      if (option.IsNone)
      {
        if (stillWorking)
          this.m_leftMouseClickBuffer = true;
        else
          this.m_invalidClickSound.Play();
        return new TransportBuildController.State?(TransportBuildController.State.SelectingContinuationPivot);
      }
      CanBuildTransportResult buildTransportResult = option.Value;
      if (!shouldFinishBuild)
      {
        Assert.That<bool>(pickResult.MustBeFlat).IsFalse("Hovering over transport but not finish build?");
        ImmutableArray<Tile3i> requestPivots = buildTransportResult.RequestPivots;
        bool nextStartMustBeFlat = requestPivots.Length >= 2 && requestPivots.Last.Z != requestPivots.PreLast.Z;
        ImmutableArray<Tile2i> immutableArray;
        if (!first.PillarHints.IsEmpty)
        {
          Tile2i last = first.PillarHints.Last;
          ref Tile2i local = ref last;
          tile3i = requestPivots.Last;
          Tile2i xy = tile3i.Xy;
          if (local.DistanceSqrTo(xy) <= 1L)
          {
            immutableArray = first.PillarHints;
            goto label_36;
          }
        }
        ref readonly ImmutableArray<Tile2i> local1 = ref first.PillarHints;
        tile3i = requestPivots.Last;
        Tile2i xy3 = tile3i.Xy;
        immutableArray = local1.Add(xy3);
label_36:
        ImmutableArray<Tile2i> pillarHints = immutableArray;
        this.pushTrajToHistoryStack(requestPivots, pillarHints, successTrajectory, new Direction903d?(), nextStartMustBeFlat);
        this.m_addTransportSegmentSound.Play();
        return new TransportBuildController.State?(TransportBuildController.State.SelectingContinuationPivot);
      }
      if (buildTransportResult.NewTrajectory.HasValue)
      {
        this.m_ongoingCmd = (Option<BuildTransportCmd>) inputScheduler.ScheduleInputCmd<BuildTransportCmd>(new BuildTransportCmd(buildTransportResult.NewTrajectory.Value.TransportProto.Id, buildTransportResult.RequestPivots, buildTransportResult.SupportedTiles.Map<Tile2i>((Func<Tile3i, Tile2i>) (x => x.Xy)), buildTransportResult.RequestStartDirection, buildTransportResult.RequestEndDirection, this.m_disableSnapping, false));
        return new TransportBuildController.State?(TransportBuildController.State.AwaitingTransportCreation);
      }
      if (buildTransportResult.MiniZipperAtStart.HasValue)
      {
        Assert.That<CanPlaceMiniZipperAtResult?>(buildTransportResult.MiniZipJoinResultAtEnd).IsNone<CanPlaceMiniZipperAtResult>();
        this.m_ongoingCmd = (Option<BuildTransportCmd>) inputScheduler.ScheduleInputCmd<BuildTransportCmd>(new BuildTransportCmd(this.m_currTransportProto.Value.Id, ImmutableArray.Create<Tile3i>(buildTransportResult.MiniZipperAtStart.Value.Position), ImmutableArray<Tile2i>.Empty, new Direction903d?(), new Direction903d?(), this.m_disableSnapping, false));
        return new TransportBuildController.State?(TransportBuildController.State.AwaitingTransportCreation);
      }
      if (buildTransportResult.RequestPivots.Length > 1)
      {
        if (buildTransportResult.MiniZipJoinResultAtStart.HasValue)
        {
          Assert.That<CanPlaceMiniZipperAtResult?>(buildTransportResult.MiniZipJoinResultAtEnd).IsNone<CanPlaceMiniZipperAtResult>();
          this.m_ongoingCmd = (Option<BuildTransportCmd>) inputScheduler.ScheduleInputCmd<BuildTransportCmd>(new BuildTransportCmd(buildTransportResult.MiniZipJoinResultAtStart.Value.CutOutResult.ReplacedTransport.Prototype.Id, ImmutableArray.Create<Tile3i>(buildTransportResult.MiniZipJoinResultAtStart.Value.CutOutResult.CutOutPosition), ImmutableArray<Tile2i>.Empty, buildTransportResult.RequestStartDirection, buildTransportResult.RequestEndDirection, this.m_disableSnapping, false));
          return new TransportBuildController.State?(TransportBuildController.State.AwaitingTransportCreation);
        }
        if (buildTransportResult.MiniZipJoinResultAtEnd.HasValue)
        {
          Assert.That<CanPlaceMiniZipperAtResult?>(buildTransportResult.MiniZipJoinResultAtStart).IsNone<CanPlaceMiniZipperAtResult>();
          this.m_ongoingCmd = (Option<BuildTransportCmd>) inputScheduler.ScheduleInputCmd<BuildTransportCmd>(new BuildTransportCmd(buildTransportResult.MiniZipJoinResultAtEnd.Value.CutOutResult.ReplacedTransport.Prototype.Id, ImmutableArray.Create<Tile3i>(buildTransportResult.MiniZipJoinResultAtEnd.Value.CutOutResult.CutOutPosition), ImmutableArray<Tile2i>.Empty, buildTransportResult.RequestStartDirection, buildTransportResult.RequestEndDirection, this.m_disableSnapping, false));
          return new TransportBuildController.State?(TransportBuildController.State.AwaitingTransportCreation);
        }
      }
      this.m_invalidOpSound.Play();
      return new TransportBuildController.State?(this.m_pivotsHistory.IsEmpty ? TransportBuildController.State.SelectingFirstPivot : TransportBuildController.State.SelectingContinuationPivot);
    }

    private TransportBuildController.State? handleAwaitingTransportCreation()
    {
      if (this.m_ongoingCmd.IsNone)
      {
        Assert.Fail("Invalid state.");
        return new TransportBuildController.State?(TransportBuildController.State.SelectingFirstPivot);
      }
      if (!this.m_ongoingCmd.Value.IsProcessedAndSynced)
        return new TransportBuildController.State?();
      if (this.m_ongoingCmd.Value.Result.IsValid)
        this.m_buildingPlacedSound.Play();
      else
        this.m_invalidOpSound.Play();
      this.m_transportPreview.Clear();
      this.m_pivotsHistory.Clear();
      return new TransportBuildController.State?(TransportBuildController.State.SelectingFirstPivot);
    }

    /// <summary>
    /// Tries to pick a potential port under the cursor that is connectible.
    /// </summary>
    private bool pickAndSelectConnectablePort(out IoPort port)
    {
      port = this.m_picker.PickPortAndSelect<IoPort>(new CursorPickingManager.PortPredicateReturningColor(this.portConnectableMatcher)).ValueOrNull;
      return port != null;
    }

    private bool portConnectableMatcher(IoPort port, out ColorRgba color)
    {
      if (port.IsConnected)
      {
        color = ColorRgba.Empty;
        return false;
      }
      Option<TransportProto> shapeToTransport = this.m_shapeToTransportMap[port.ShapePrototype];
      if (shapeToTransport.IsNone)
      {
        color = ColorRgba.Empty;
        return false;
      }
      if (this.m_currTransportProto.HasValue && this.m_currTransportProto != shapeToTransport)
      {
        color = TransportBuildController.SELECTION_NOT_OK;
        return false;
      }
      if (!this.m_unlockedPortShapes.Contains(port.ShapePrototype))
      {
        color = ColorRgba.Empty;
        return false;
      }
      color = TransportBuildController.SELECTION_OK;
      return true;
    }

    private bool transportUp()
    {
      if (this.m_currTransportProto.IsNone)
        return false;
      ThicknessTilesI relativeHeight = this.m_terrainCursor.RelativeHeight;
      this.m_terrainCursor.RelativeHeight = (this.m_terrainCursor.RelativeHeight + ThicknessTilesI.One).Min(TransportPillarProto.MAX_PILLAR_HEIGHT - ThicknessTilesI.One);
      this.m_lastMousePos = Vector3.zero;
      return relativeHeight != this.m_terrainCursor.RelativeHeight;
    }

    private bool transportDown()
    {
      if (this.m_currTransportProto.IsNone)
        return false;
      ThicknessTilesI relativeHeight = this.m_terrainCursor.RelativeHeight;
      this.m_terrainCursor.RelativeHeight = (this.m_terrainCursor.RelativeHeight - ThicknessTilesI.One).Max(ThicknessTilesI.Zero);
      this.m_lastMousePos = Vector3.zero;
      return relativeHeight != this.m_terrainCursor.RelativeHeight;
    }

    private void toggleInvertTieBreaking()
    {
      this.m_invertTieBreaking ^= TransportPathFinderFlags.InvertTieBreaking;
      this.m_toolbox.DisplayTieBreakActive(this.m_invertTieBreaking != 0);
      this.m_lastMousePos = Vector3.zero;
    }

    private void toggleOnlyStraight()
    {
      this.m_onlyStraightTransports ^= TransportPathFinderFlags.AllowOnlyStraight;
      this.m_toolbox.DisplayOnlyStraightActive(this.m_onlyStraightTransports != 0);
      this.m_lastMousePos = Vector3.zero;
    }

    private void setOnlyStraight(bool enabled)
    {
      if (enabled)
        this.enableOnlyStraight();
      else
        this.disableOnlyStraight();
    }

    private void enableOnlyStraight()
    {
      this.m_onlyStraightTransports |= TransportPathFinderFlags.AllowOnlyStraight;
      this.m_toolbox.DisplayOnlyStraightActive(this.m_onlyStraightTransports != 0);
      this.m_lastMousePos = Vector3.zero;
    }

    private void disableOnlyStraight()
    {
      this.m_onlyStraightTransports &= ~TransportPathFinderFlags.AllowOnlyStraight;
      this.m_toolbox.DisplayOnlyStraightActive(this.m_onlyStraightTransports != 0);
      this.m_lastMousePos = Vector3.zero;
    }

    private void togglePortSnapping()
    {
      this.m_disableSnapping = !this.m_disableSnapping;
      this.m_toolbox.DisplaySnappingDisabled(this.m_disableSnapping);
      this.m_lastMousePos = Vector3.zero;
    }

    private void togglePortsBlocking()
    {
      this.m_banTiledInFrontOfPorts ^= TransportPathFinderFlags.BanTilesInFrontOfPorts;
      this.m_toolbox.DisplayPortsBlockingDisabled(this.m_banTiledInFrontOfPorts != TransportPathFinderFlags.BanTilesInFrontOfPorts);
      this.m_lastMousePos = Vector3.zero;
    }

    private void togglePricePopup()
    {
      this.m_transportPreview.TogglePricePopup();
      this.m_toolbox.DisplayPricePopupDisabled(this.m_transportPreview.PricePopupIsHidden);
    }

    static TransportBuildController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TransportBuildController.SELECTION_OK = new ColorRgba(0, (int) byte.MaxValue, 0, 192);
      TransportBuildController.SELECTION_NOT_OK = new ColorRgba((int) byte.MaxValue, 0, 0, 192);
    }

    private readonly struct PickResult
    {
      public readonly Tile3i Position;
      public readonly Option<TransportProto> TransportProto;
      public readonly Direction903d? Direction;
      public readonly Direction903d? ForcedDirection;
      public readonly bool MustBeFlat;

      public PickResult(
        Tile3i position,
        Option<TransportProto> transportProto = default (Option<TransportProto>),
        Direction903d? direction = null,
        Direction903d? forcedDirection = null,
        bool mustBeFlat = false)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        this.ForcedDirection = forcedDirection;
        this.MustBeFlat = mustBeFlat;
        this.TransportProto = transportProto;
        this.Direction = direction;
      }
    }

    private enum State
    {
      NoProtoSelected,
      SelectingFirstPivot,
      SelectingContinuationPivot,
      AwaitingTransportCreation,
    }

    private readonly struct HistoryEntry
    {
      public readonly ImmutableArray<Tile3i> Pivots;
      public readonly ImmutableArray<Tile2i> PillarHints;
      public readonly Option<TransportTrajectory> Trajectory;
      public readonly ImmutableArray<Tile3i> BannedTiles;
      public readonly Direction903d? NextForcedStartDirection;
      public readonly bool NextStartMustBeFlat;

      public HistoryEntry(
        ImmutableArray<Tile3i> pivots,
        ImmutableArray<Tile2i> pillarHints,
        Option<TransportTrajectory> trajectory,
        ImmutableArray<Tile3i> bannedTiles,
        Direction903d? nextForcedStartDirection,
        bool nextStartMustBeFlat)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Pivots = pivots;
        this.PillarHints = pillarHints;
        this.Trajectory = trajectory;
        this.BannedTiles = bannedTiles;
        this.NextForcedStartDirection = nextForcedStartDirection;
        this.NextStartMustBeFlat = nextStartMustBeFlat;
      }
    }
  }
}
