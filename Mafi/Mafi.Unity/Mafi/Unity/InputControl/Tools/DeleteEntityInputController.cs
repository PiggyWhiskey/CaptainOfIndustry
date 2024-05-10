// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.DeleteEntityInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.Terrain;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  /// <summary>Tool for destroying/selling static entities.</summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class DeleteEntityInputController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    public static readonly ColorRgba COLOR_HIGHLIGHT;
    public static readonly ColorRgba COLOR_HIGHLIGHT_WARN;
    public static readonly ColorRgba COLOR_HIGHLIGHT_TRANSPORT;
    public static readonly ColorRgba COLOR_HIGHLIGHT_CONFIRM;
    private readonly CursorPickingManager m_picker;
    private readonly IInputScheduler m_inputScheduler;
    private readonly IConstructionManager m_constructionManager;
    private readonly TreeRemovalHandler m_treeRemovalHandler;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly IUnityInputMgr m_inputManager;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly CursorManager m_cursorManager;
    private readonly FloatingPricePopup m_pricePopup;
    private readonly ImmutableArray<IStaticEntityRemovalHandler> m_handlers;
    private Option<IStaticEntityRemovalHandler> m_activeHandler;
    private bool m_treeRemovalHandlerActive;
    private readonly Action<IInputCommand> m_scheduleCommandAction;
    private readonly DeleteEntityInputController.DeleteToolbox m_toolbox;
    private readonly UiBuilder m_builder;
    private readonly Cursoor m_cursor;
    private readonly AudioInfo m_successSound;
    private readonly AudioSource m_invalidSound;
    /// <summary>Commands that are currently in progress.</summary>
    private readonly Lyst<IInputCommand> m_pendingCommands;
    private readonly Lyst<AudioSource> m_sounds;
    private readonly Predicate<IStaticEntity> m_anyEntityMatcher;
    private Vector3? m_initialMouseDownPos;
    private Option<IStaticEntity> m_lastHoveredEntity;

    public ControllerConfig Config => ControllerConfig.Tool;

    public bool IsVisible => true;

    public bool DeactivateShortcutsIfNotVisible => false;

    public event Action<IToolbarItemController> VisibilityChanged;

    public DeleteEntityInputController(
      ShortcutsManager shortcutsManager,
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoopEvents,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      NewInstanceOf<FloatingPricePopup> pricePopup,
      ToolbarController toolbarController,
      IInputScheduler inputScheduler,
      IConstructionManager constructionManager,
      AllImplementationsOf<IStaticEntityRemovalHandler> removalHandlers,
      TreeRemovalHandler treeRemovalHandler,
      UiBuilder builder)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_pendingCommands = new Lyst<IInputCommand>();
      this.m_sounds = new Lyst<AudioSource>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
      this.m_inputManager = inputManager;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_picker = cursorPickingManager;
      this.m_cursorManager = cursorManager;
      this.m_pricePopup = pricePopup.Instance;
      this.m_inputScheduler = inputScheduler;
      this.m_constructionManager = constructionManager;
      this.m_treeRemovalHandler = treeRemovalHandler;
      this.m_builder = builder;
      this.m_handlers = removalHandlers.Implementations.OrderBy<int>((Func<IStaticEntityRemovalHandler, int>) (x => x.Priority)).ToImmutableArray<IStaticEntityRemovalHandler>();
      this.m_scheduleCommandAction = new Action<IInputCommand>(this.scheduleCommand);
      this.m_toolbox = new DeleteEntityInputController.DeleteToolbox(toolbarController, builder);
      this.m_cursor = this.m_cursorManager.RegisterCursor(builder.Style.Cursors.Delete);
      this.m_successSound = builder.Audio.Demolish;
      this.m_invalidSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
      this.m_anyEntityMatcher = (Predicate<IStaticEntity>) (entity => !entity.IsDestroyed && !(entity is TransportPillar));
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
      controller.AddLeftMenuButton(TrCore.Demolish.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Buldoze128.png", 30f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleDeleteTool), new BtnStyle?(this.m_builder.Style.Toolbar.ButtonDeleteOn), new BtnStyle?(this.m_builder.Style.Toolbar.ButtonDeleteOff));
    }

    public void Activate()
    {
      this.m_cursor.Show();
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<DeleteEntityInputController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_toolbox.Show();
      this.m_pricePopup.SetTemporarilyHidden(false);
    }

    public void Deactivate()
    {
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<DeleteEntityInputController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_pricePopup.Hide();
      this.m_cursor.Hide();
      this.m_picker.ClearPicked();
      this.m_pendingCommands.Clear();
      this.m_toolbox.Hide();
      this.m_lastHoveredEntity = Option<IStaticEntity>.None;
      if (this.m_treeRemovalHandlerActive)
      {
        this.m_activeHandler.IsNone.AssertTrue();
        this.m_treeRemovalHandler.Deactivate();
        this.m_treeRemovalHandlerActive = false;
      }
      else
      {
        if (!this.m_activeHandler.HasValue)
          return;
        this.m_activeHandler.Value.Deactivate();
        this.m_activeHandler = Option<IStaticEntityRemovalHandler>.None;
      }
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.m_shortcutsManager.IsPrimaryActionOn);
      this.m_toolbox.QuickRemoveBtn.SetIsOn(this.m_shortcutsManager.IsOn(this.m_shortcutsManager.DeleteWithQuickRemove));
      this.m_toolbox.EntireTransportBtn.SetIsOn(this.m_shortcutsManager.IsOn(this.m_shortcutsManager.DeleteEntireTransport));
      if (this.m_shortcutsManager.IsSecondaryActionUp)
      {
        if (this.m_activeHandler.IsNone || !this.m_activeHandler.Value.TryHandleCancel())
          this.m_inputManager.DeactivateController((IUnityInputController) this);
        return true;
      }
      if (this.m_shortcutsManager.IsDown(this.m_shortcutsManager.TogglePricePopup))
      {
        this.m_pricePopup.SetTemporarilyHidden(!this.m_pricePopup.IsTemporarilyHidden);
        return true;
      }
      if (this.m_pendingCommands.IsNotEmpty)
      {
        Assert.That<Option<IStaticEntityRemovalHandler>>(this.m_activeHandler).IsNone<IStaticEntityRemovalHandler>();
        if (this.waitForPendingCommands())
          return false;
        this.m_pendingCommands.Clear();
      }
      ColorRgba empty = ColorRgba.Empty;
      AssetValue deconstructionValue = AssetValue.Empty;
      LocStrFormatted errorMsg = LocStrFormatted.Empty;
      Option<IStaticEntity> entityUnderCursor = this.m_picker.PickEntity<IStaticEntity>(this.m_anyEntityMatcher);
      TreeId? treeUnderCursor = this.m_picker.PickTree((Predicate<TreeMb>) (treeMb => treeMb.IsPreview));
      Tile3f lastPickedTile = this.m_picker.LastPickedTile;
      if (this.m_treeRemovalHandlerActive)
      {
        this.m_activeHandler.IsNone.AssertTrue();
        if (!this.m_treeRemovalHandler.TryHandleHover(treeUnderCursor, out errorMsg))
        {
          this.m_treeRemovalHandler.Deactivate();
          this.m_treeRemovalHandlerActive = false;
        }
      }
      else
      {
        if (entityUnderCursor != this.m_lastHoveredEntity)
        {
          this.m_lastHoveredEntity = entityUnderCursor;
          if (this.m_activeHandler.HasValue && this.m_activeHandler.Value.CanBeInterrupted)
          {
            this.m_activeHandler.Value.Deactivate();
            this.m_activeHandler = Option<IStaticEntityRemovalHandler>.None;
          }
        }
        if (this.m_activeHandler.HasValue && !this.m_activeHandler.Value.TryHandleHover(entityUnderCursor, lastPickedTile, out deconstructionValue, out errorMsg))
        {
          this.m_activeHandler.Value.Deactivate();
          this.m_activeHandler = Option<IStaticEntityRemovalHandler>.None;
        }
        if (this.m_activeHandler.IsNone)
        {
          foreach (IStaticEntityRemovalHandler handler in this.m_handlers)
          {
            if (handler.TryHandleHover(entityUnderCursor, lastPickedTile, out deconstructionValue, out errorMsg))
            {
              this.m_activeHandler = handler.SomeOption<IStaticEntityRemovalHandler>();
              break;
            }
          }
        }
      }
      bool flag = false;
      if (this.m_activeHandler.HasValue)
      {
        if (!EventSystem.current.IsPointerOverGameObject() && this.m_shortcutsManager.IsPrimaryActionDown)
        {
          if (this.m_activeHandler.Value.TryHandleMouseDown(entityUnderCursor))
          {
            this.m_initialMouseDownPos = new Vector3?(UnityEngine.Input.mousePosition);
            flag = true;
          }
          else
          {
            this.m_activeHandler.Value.Deactivate();
            this.m_activeHandler = Option<IStaticEntityRemovalHandler>.None;
            return false;
          }
        }
        if (this.m_shortcutsManager.IsPrimaryActionUp)
        {
          bool useQuickRemove = this.m_shortcutsManager.IsOn(this.m_shortcutsManager.DeleteWithQuickRemove);
          if (!EventSystem.current.IsPointerOverGameObject() && this.m_activeHandler.Value.TryHandleMouseUp(entityUnderCursor, this.m_scheduleCommandAction, useQuickRemove))
          {
            this.m_initialMouseDownPos = new Vector3?();
            flag = true;
          }
          else
          {
            this.m_activeHandler.Value.Deactivate();
            this.m_activeHandler = Option<IStaticEntityRemovalHandler>.None;
            return false;
          }
        }
      }
      else if (entityUnderCursor.IsNone && this.m_treeRemovalHandler.TryHandleHover(treeUnderCursor, out errorMsg))
      {
        this.m_treeRemovalHandlerActive = true;
        if (!EventSystem.current.IsPointerOverGameObject() && this.m_shortcutsManager.IsPrimaryActionDown)
        {
          if (this.m_treeRemovalHandler.TryHandleMouseDown(treeUnderCursor))
          {
            this.m_initialMouseDownPos = new Vector3?(UnityEngine.Input.mousePosition);
            flag = true;
          }
          else
          {
            this.m_treeRemovalHandler.Deactivate();
            this.m_treeRemovalHandlerActive = false;
            return false;
          }
        }
        if (this.m_shortcutsManager.IsPrimaryActionUp)
        {
          bool useQuickRemove = this.m_shortcutsManager.IsOn(this.m_shortcutsManager.DeleteWithQuickRemove);
          if (!EventSystem.current.IsPointerOverGameObject() && this.m_treeRemovalHandler.TryHandleMouseUp(treeUnderCursor, this.m_scheduleCommandAction, useQuickRemove))
          {
            this.m_initialMouseDownPos = new Vector3?();
            flag = true;
          }
          else
          {
            this.m_treeRemovalHandler.Deactivate();
            this.m_treeRemovalHandlerActive = false;
            return false;
          }
        }
      }
      this.m_pricePopup.SetSellPrice(deconstructionValue);
      this.m_pricePopup.SetErrorMessage(errorMsg);
      this.m_pricePopup.UpdatePosition();
      return flag;
    }

    private void syncUpdate(GameTime time)
    {
      if (!this.m_activeHandler.HasValue)
        return;
      this.m_activeHandler.Value.Sync();
    }

    private void scheduleCommand(IInputCommand cmd)
    {
      this.m_inputScheduler.ScheduleInputCmd<IInputCommand>(cmd);
      this.m_pendingCommands.Add(cmd);
    }

    private bool waitForPendingCommands()
    {
      bool flag = false;
      foreach (IInputCommand pendingCommand in this.m_pendingCommands)
      {
        if (!pendingCommand.IsProcessedAndSynced)
          return true;
        flag |= !pendingCommand.HasError;
      }
      if (flag)
        this.playSuccessSound();
      else
        this.m_invalidSound.Play();
      return false;
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

    static DeleteEntityInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      DeleteEntityInputController.COLOR_HIGHLIGHT = new ColorRgba(192, 0, 0, 192);
      DeleteEntityInputController.COLOR_HIGHLIGHT_WARN = new ColorRgba(192, 96, 0, 192);
      DeleteEntityInputController.COLOR_HIGHLIGHT_TRANSPORT = new ColorRgba(192, 192, 0, 64);
      DeleteEntityInputController.COLOR_HIGHLIGHT_CONFIRM = new ColorRgba((int) byte.MaxValue, 0, 0, 192);
    }

    private class DeleteToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;
      public ToggleBtn QuickRemoveBtn;
      public ToggleBtn EntireTransportBtn;

      public DeleteToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("Delete", "Assets/Unity/UserInterface/Toolbar/Buldoze128.png", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.DeleteTool__Tooltip);
        this.QuickRemoveBtn = this.AddToggleButton("Quick remove", "Assets/Unity/UserInterface/General/UnitySmall.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.DeleteWithQuickRemove), (LocStrFormatted) Tr.DeleteTool__QuickRemoveTooltip);
        this.EntireTransportBtn = this.AddToggleButton("Delete entire transport", "Assets/Unity/UserInterface/Toolbar/Transports.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.DeleteEntireTransport), (LocStrFormatted) Tr.DeleteTool__EntireTransport);
        this.AddToToolbar();
      }
    }
  }
}
