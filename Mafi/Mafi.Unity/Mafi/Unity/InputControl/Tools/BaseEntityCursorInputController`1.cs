// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.BaseEntityCursorInputController`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  public abstract class BaseEntityCursorInputController<T> : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
    where T : class, IAreaSelectableEntity, IRenderedEntity
  {
    public static readonly RelTile1i MAX_AREA_EDGE_SIZE;
    protected readonly ShortcutsManager ShortcutsManager;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly IUnityInputMgr m_inputManager;
    private readonly CursorPickingManager m_picker;
    private readonly CursorManager m_cursorManager;
    private readonly IEntitiesManager m_entitiesManager;
    private readonly EntityHighlighter m_highlighter;
    private readonly Option<TransportTrajectoryHighlighter> m_transportTrajectoryHighlighter;
    private readonly Lyst<AudioSource> m_sounds;
    private UiBuilder m_builder;
    private Option<Cursoor> m_cursor;
    private AudioSource m_invalidSound;
    /// <summary>
    /// Entity that was detected on mouse down. If the same entity is detected on mouse-up it will be (un)paused.
    /// Otherwise it will not be (un)paused.
    /// </summary>
    private Option<T> m_toToggle;
    private Option<T> m_hoveredEntity;
    /// <summary>Commands that are currently in progress.</summary>
    private readonly Lyst<InputCommand<bool>> m_pendingCmds;
    private readonly AreaSelectionTool m_areaSelectionTool;
    private readonly Lyst<T> m_selectedEntities;
    private readonly Lyst<SubTransport> m_selectedPartialTransports;
    private readonly Lyst<TransportTrajectory> m_partialTrajsTmp;
    private AudioInfo m_successSound;
    private ColorRgba m_colorHighlight;
    private ColorRgba m_colorConfirm;
    private ColorRgba? m_rightClickAreaColor;
    private bool m_isFirstUpdate;
    private bool m_enablePartialTransportsSelection;
    private bool m_isInstaActionDisabled;
    private bool m_clearSelectionOnDeactivateOnly;
    private readonly Option<Proto> m_lockedByProto;

    public event Action<IToolbarItemController> VisibilityChanged;

    public virtual ControllerConfig Config => ControllerConfig.Tool;

    public bool IsVisible { get; private set; }

    public virtual bool DeactivateShortcutsIfNotVisible => false;

    public virtual bool AllowAreaOnlySelection => false;

    public bool IsActive { get; private set; }

    protected BaseEntityCursorInputController(
      ProtosDb protosDb,
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      ShortcutsManager shortcutsManager,
      IUnityInputMgr inputManager,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      AreaSelectionToolFactory areaSelectionToolFactory,
      IEntitiesManager entitiesManager,
      NewInstanceOf<EntityHighlighter> highlighter,
      Option<NewInstanceOf<TransportTrajectoryHighlighter>> transportTrajectoryHighlighter,
      Proto.ID? lockByProto)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_sounds = new Lyst<AudioSource>();
      this.m_pendingCmds = new Lyst<InputCommand<bool>>();
      this.m_selectedEntities = new Lyst<T>();
      this.m_selectedPartialTransports = new Lyst<SubTransport>();
      this.m_partialTrajsTmp = new Lyst<TransportTrajectory>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.ShortcutsManager = shortcutsManager;
      this.m_inputManager = inputManager;
      this.m_picker = cursorPickingManager;
      this.m_cursorManager = cursorManager;
      this.m_entitiesManager = entitiesManager;
      this.m_highlighter = highlighter.Instance;
      this.m_transportTrajectoryHighlighter = (Option<TransportTrajectoryHighlighter>) transportTrajectoryHighlighter.ValueOrNull?.Instance;
      this.m_lockedByProto = lockByProto.HasValue ? (Option<Proto>) protosDb.GetOrThrow<Proto>(lockByProto.Value) : Option<Proto>.None;
      this.m_areaSelectionTool = areaSelectionToolFactory.CreateInstance(new Action<RectangleTerrainArea2i, bool>(this.updateSelectionSync), new Action<RectangleTerrainArea2i, bool>(this.selectionDone), new Action(this.clearSelection), new Action(this.deactivateSelf));
      this.m_areaSelectionTool.SetEdgeSizeLimit(BaseEntityCursorInputController<T>.MAX_AREA_EDGE_SIZE);
      this.m_invalidSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
    }

    protected void InitializeUi(
      CursorStyle? cursorStyle,
      AudioInfo successSound,
      ColorRgba colorHighlight,
      ColorRgba colorConfirm)
    {
      this.m_successSound = successSound;
      this.m_colorHighlight = colorHighlight;
      this.m_colorConfirm = colorConfirm;
      if (cursorStyle.HasValue)
        this.m_cursor = (Option<Cursoor>) this.m_cursorManager.RegisterCursor(cursorStyle.Value);
      this.m_areaSelectionTool.SetLeftClickColor(colorHighlight);
    }

    protected void SetEdgeSizeLimit(RelTile1i limit)
    {
      this.m_areaSelectionTool.SetEdgeSizeLimit(limit);
    }

    protected void ClearSelectionOnDeactivateOnly() => this.m_clearSelectionOnDeactivateOnly = true;

    protected void SetPartialTransportsSelection(bool isEnabled)
    {
      if (this.m_transportTrajectoryHighlighter.IsNone)
      {
        Log.Error("Transports trajectory highlighter must be set to allow partial transports.");
      }
      else
      {
        this.m_enablePartialTransportsSelection = isEnabled;
        this.m_areaSelectionTool.ForceSelectionChanged();
      }
    }

    protected void SetUpRightClickAreaSelection(ColorRgba color)
    {
      this.m_rightClickAreaColor = new ColorRgba?(color);
      this.m_areaSelectionTool.SetRightClickColor(color);
    }

    protected abstract bool Matches(T entity, bool isAreaSelection, bool isLeftClick);

    protected abstract void RegisterToolbar(ToolbarController controller);

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      this.IsVisible = this.m_lockedByProto.IsNone || this.m_unlockedProtosDb.IsUnlocked((IProto) this.m_lockedByProto.Value);
      if (!this.IsVisible)
      {
        this.m_unlockedProtosDb.OnUnlockedSetChangedForUi += new Action(this.updateIsVisible);
      }
      else
      {
        Action<IToolbarItemController> visibilityChanged = this.VisibilityChanged;
        if (visibilityChanged != null)
          visibilityChanged((IToolbarItemController) this);
      }
      this.RegisterToolbar(controller);
    }

    private void updateIsVisible()
    {
      bool flag = this.m_unlockedProtosDb.IsUnlocked((IProto) this.m_lockedByProto.Value);
      if (this.IsVisible == flag)
        return;
      this.IsVisible = flag;
      Action<IToolbarItemController> visibilityChanged = this.VisibilityChanged;
      if (visibilityChanged == null)
        return;
      visibilityChanged((IToolbarItemController) this);
    }

    protected virtual void OnHoverChanged(
      IIndexable<T> hoveredEntities,
      IIndexable<SubTransport> hoveredPartialTransports,
      bool isLeftClick)
    {
    }

    protected void SetInstaActionDisabled(bool isDisabled)
    {
      this.m_isInstaActionDisabled = isDisabled;
    }

    public virtual void Activate()
    {
      if (this.IsActive)
        return;
      this.IsActive = true;
      this.m_cursor.ValueOrNull?.Show();
      this.m_areaSelectionTool.TerrainCursor.Activate();
      this.m_isFirstUpdate = !this.m_isInstaActionDisabled;
    }

    public virtual void Deactivate()
    {
      if (!this.IsActive)
        return;
      this.m_cursor.ValueOrNull?.Hide();
      this.m_picker.ClearPicked();
      this.m_pendingCmds.Clear();
      this.m_toToggle = (Option<T>) Option.None;
      this.m_areaSelectionTool.TerrainCursor.Deactivate();
      this.m_areaSelectionTool.Deactivate();
      this.clearSelection();
      this.m_isFirstUpdate = false;
      this.IsActive = false;
    }

    protected void HideCursor() => this.m_cursor.ValueOrNull?.Hide();

    public virtual bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.IsActive)
      {
        Log.Error("Input update for non-active controller!");
        return false;
      }
      if (this.ShortcutsManager.IsSecondaryActionUp && !this.m_rightClickAreaColor.HasValue)
      {
        this.deactivateSelf();
        return true;
      }
      bool isFirstUpdate = this.m_isFirstUpdate;
      this.m_isFirstUpdate = false;
      if (this.m_pendingCmds.IsNotEmpty)
      {
        if (!this.handleCurrentCommand())
          return false;
        this.m_pendingCmds.Clear();
      }
      if (this.m_toToggle.IsNone)
      {
        if (this.m_areaSelectionTool.IsActive)
          return true;
        T obj = this.m_selectedEntities.Count == 1 ? this.m_selectedEntities.First : default (T);
        this.m_selectedEntities.Clear();
        this.m_selectedPartialTransports.Clear();
        this.m_hoveredEntity = this.m_picker.PickEntityAndSelect<T>(new CursorPickingManager.EntityPredicateReturningColor<T>(this.anyEntityMatcher));
        if (this.m_hoveredEntity.HasValue)
        {
          if (isFirstUpdate && !EventSystem.current.IsPointerOverGameObject())
          {
            this.m_selectedEntities.Clear();
            this.m_selectedPartialTransports.Clear();
            if (this.OnFirstActivated(this.m_hoveredEntity.Value, this.m_selectedEntities, this.m_selectedPartialTransports) && (this.m_selectedEntities.IsNotEmpty || this.m_selectedPartialTransports.IsNotEmpty))
            {
              this.OnEntitiesSelected((IIndexable<T>) this.m_selectedEntities, (IIndexable<SubTransport>) this.m_selectedPartialTransports, false, true, new RectangleTerrainArea2i?());
              this.m_picker.RemovePickedHighlight();
              return true;
            }
          }
          this.m_selectedEntities.Add(this.m_hoveredEntity.Value);
          if (obj != this.m_hoveredEntity)
            this.OnHoverChanged((IIndexable<T>) this.m_selectedEntities, (IIndexable<SubTransport>) this.m_selectedPartialTransports, true);
          if (this.ShortcutsManager.IsPrimaryActionDown && !EventSystem.current.IsPointerOverGameObject())
          {
            this.m_toToggle = this.m_hoveredEntity;
            return true;
          }
        }
        else
        {
          if (this.ShortcutsManager.IsPrimaryActionDown)
          {
            this.m_areaSelectionTool.Activate(true);
            return true;
          }
          if (this.m_rightClickAreaColor.HasValue && this.ShortcutsManager.IsSecondaryActionDown)
          {
            this.m_areaSelectionTool.Activate(false);
            return true;
          }
          this.OnHoverChanged((IIndexable<T>) this.m_selectedEntities, (IIndexable<SubTransport>) this.m_selectedPartialTransports, true);
          return false;
        }
      }
      else
      {
        Option<T> option = this.m_picker.PickEntityAndSelect<T>(new CursorPickingManager.EntityPredicateReturningColor<T>(this.entityMatcher));
        if (this.ShortcutsManager.IsPrimaryActionUp)
        {
          this.m_toToggle = (Option<T>) Option.None;
          this.m_selectedEntities.Clear();
          this.m_selectedPartialTransports.Clear();
          if (option.IsNone || EventSystem.current.IsPointerOverGameObject())
          {
            this.OnHoverChanged((IIndexable<T>) this.m_selectedEntities, (IIndexable<SubTransport>) this.m_selectedPartialTransports, true);
            return true;
          }
          this.m_picker.ClearPicked();
          this.m_selectedEntities.Add(option.Value);
          if (this.m_selectedEntities.IsNotEmpty || this.m_selectedPartialTransports.IsNotEmpty)
            this.OnEntitiesSelected((IIndexable<T>) this.m_selectedEntities, (IIndexable<SubTransport>) this.m_selectedPartialTransports, false, true, new RectangleTerrainArea2i?());
          return true;
        }
      }
      return false;
    }

    protected abstract bool OnFirstActivated(
      T hoveredEntity,
      Lyst<T> selectedEntities,
      Lyst<SubTransport> selectedPartialTransports);

    protected abstract void OnEntitiesSelected(
      IIndexable<T> selectedEntities,
      IIndexable<SubTransport> selectedPartialTransports,
      bool isAreaSelection,
      bool isLeftMouse,
      RectangleTerrainArea2i? area);

    protected void RegisterPendingCommand(InputCommand cmd)
    {
      this.m_pendingCmds.Add((InputCommand<bool>) cmd);
    }

    private void deactivateSelf()
    {
      if (this.m_clearSelectionOnDeactivateOnly)
        this.clearSelection();
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    /// <summary>
    /// Handles logic of processing current pause command. Returns true if current command was processed.
    /// </summary>
    private bool handleCurrentCommand()
    {
      bool flag = false;
      foreach (InputCommand<bool> pendingCmd in this.m_pendingCmds)
      {
        if (!pendingCmd.IsProcessedAndSynced)
          return false;
        flag |= pendingCmd.Result;
      }
      if (flag)
        this.playSuccessSound();
      else
        this.m_invalidSound.Play();
      return true;
    }

    private void playSuccessSound()
    {
      AudioSource audioSource = (AudioSource) null;
      foreach (AudioSource sound in this.m_sounds)
      {
        if (!sound.isPlaying)
          audioSource = sound;
      }
      if ((UnityEngine.Object) audioSource == (UnityEngine.Object) null)
      {
        audioSource = this.m_builder.AudioDb.GetClonedAudio(this.m_successSound);
        this.m_sounds.Add(audioSource);
      }
      audioSource.Play();
    }

    private bool anyEntityMatcher(T entity, out ColorRgba color)
    {
      if (!this.Matches(entity, false, true))
      {
        color = ColorRgba.Empty;
        return false;
      }
      color = this.m_colorHighlight;
      return true;
    }

    private bool entityMatcher(T entity, out ColorRgba color)
    {
      if (entity != this.m_toToggle || !this.Matches(entity, false, true))
      {
        color = ColorRgba.Empty;
        return false;
      }
      color = this.m_colorConfirm;
      return true;
    }

    private void selectionDone(RectangleTerrainArea2i area, bool isLeftClick)
    {
      if (this.m_selectedEntities.IsNotEmpty || this.m_selectedPartialTransports.IsNotEmpty || this.AllowAreaOnlySelection)
        this.OnEntitiesSelected((IIndexable<T>) this.m_selectedEntities, (IIndexable<SubTransport>) this.m_selectedPartialTransports, true, isLeftClick, new RectangleTerrainArea2i?(area));
      if (!this.m_clearSelectionOnDeactivateOnly)
        this.clearSelection();
      this.m_areaSelectionTool.Deactivate();
      this.OnHoverChanged((IIndexable<T>) this.m_selectedEntities, (IIndexable<SubTransport>) this.m_selectedPartialTransports, isLeftClick);
    }

    private void clearSelection()
    {
      this.m_selectedEntities.Clear();
      this.m_highlighter.ClearAllHighlights();
      this.m_transportTrajectoryHighlighter.ValueOrNull?.ClearAllHighlights();
      this.m_selectedPartialTransports.Clear();
    }

    private void updateSelectionSync(RectangleTerrainArea2i area, bool leftClick)
    {
      this.clearSelection();
      foreach (T entity in this.m_entitiesManager.GetAllEntitiesOfType<T>())
      {
        if (entity.IsSelected(area) && this.Matches(entity, true, leftClick))
        {
          if (this.m_enablePartialTransportsSelection && entity is Mafi.Core.Factory.Transports.Transport originalTransport)
          {
            this.m_partialTrajsTmp.Clear();
            bool entireTrajectoryIsInArea;
            originalTransport.Trajectory.GetSubTrajectoriesInArea(area, this.m_partialTrajsTmp, out entireTrajectoryIsInArea);
            if (entireTrajectoryIsInArea)
            {
              addEntity(entity);
              Assert.That<Lyst<TransportTrajectory>>(this.m_partialTrajsTmp).IsEmpty<TransportTrajectory>();
            }
            else
            {
              foreach (TransportTrajectory transportTrajectory in this.m_partialTrajsTmp)
              {
                this.m_selectedPartialTransports.Add(new SubTransport(originalTransport, transportTrajectory));
                this.m_transportTrajectoryHighlighter.ValueOrNull?.HighlightTrajectory(transportTrajectory, this.m_colorHighlight);
              }
            }
          }
          else
            addEntity(entity);
        }
      }
      this.m_partialTrajsTmp.Clear();
      this.OnUpdateSelectionSync(area);
      this.OnHoverChanged((IIndexable<T>) this.m_selectedEntities, (IIndexable<SubTransport>) this.m_selectedPartialTransports, leftClick);

      void addEntity(T entity)
      {
        this.m_selectedEntities.Add(entity);
        this.m_highlighter.Highlight((IRenderedEntity) entity, this.m_colorHighlight);
      }
    }

    protected virtual void OnUpdateSelectionSync(RectangleTerrainArea2i area)
    {
    }

    static BaseEntityCursorInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BaseEntityCursorInputController<T>.MAX_AREA_EDGE_SIZE = new RelTile1i(300);
    }
  }
}
