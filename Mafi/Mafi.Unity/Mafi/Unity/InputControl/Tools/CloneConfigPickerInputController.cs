// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.CloneConfigPickerInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  /// <summary>Tool for cloning settings of entities.</summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CloneConfigPickerInputController : BaseEntityCursorInputController<ILayoutEntity>
  {
    private static readonly ColorRgba HOVER_HIGHLIGHT;
    private static readonly ColorRgba HOVER_APPLY_HIGHLIGHT;
    private readonly CursorPickingManager m_picker;
    private readonly IInputScheduler m_inputScheduler;
    private readonly FloatingEntityIconPopup m_iconPopup;
    private readonly IUnityInputMgr m_inputManager;
    private readonly CursorManager m_cursorManager;
    private readonly CloneConfigPickerInputController.CloneConfigToolbox m_toolbox;
    private readonly AudioSource m_invalidSound;
    private readonly AudioSource m_onCloneStartSound;
    private readonly Cursoor m_cursor;
    private readonly Cursoor m_cursorApply;
    private Option<ILayoutEntity> m_entityToCloneFrom;

    public CloneConfigPickerInputController(
      ProtosDb protosDb,
      UiBuilder uiBuilder,
      UnlockedProtosDbForUi unlockedProtosDb,
      ShortcutsManager shortcutsManager,
      IUnityInputMgr inputManager,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      AreaSelectionToolFactory areaSelectionToolFactory,
      EntitiesManager entitiesManager,
      ToolbarController toolbarController,
      IInputScheduler inputScheduler,
      NewInstanceOf<EntityHighlighter> highlighter,
      NewInstanceOf<FloatingEntityIconPopup> iconPopup)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb, uiBuilder, unlockedProtosDb, shortcutsManager, inputManager, cursorPickingManager, cursorManager, areaSelectionToolFactory, (IEntitiesManager) entitiesManager, highlighter, (Option<NewInstanceOf<TransportTrajectoryHighlighter>>) Option.None, new Proto.ID?(IdsCore.Technology.CloneTool));
      this.m_inputManager = inputManager;
      this.m_picker = cursorPickingManager;
      this.m_cursorManager = cursorManager;
      this.m_inputScheduler = inputScheduler;
      this.m_iconPopup = iconPopup.Instance;
      this.m_toolbox = new CloneConfigPickerInputController.CloneConfigToolbox(toolbarController, uiBuilder);
      this.m_onCloneStartSound = uiBuilder.AudioDb.GetSharedAudio(uiBuilder.Audio.Unassign);
      this.m_invalidSound = uiBuilder.AudioDb.GetSharedAudio(uiBuilder.Audio.InvalidOp);
      this.m_cursor = this.m_cursorManager.RegisterCursor(uiBuilder.Style.Cursors.Clone);
      this.m_cursorApply = this.m_cursorManager.RegisterCursor(uiBuilder.Style.Cursors.Brush);
      this.InitializeUi(new CursorStyle?(uiBuilder.Style.Cursors.Brush), uiBuilder.Audio.Assign, CloneConfigPickerInputController.HOVER_APPLY_HIGHLIGHT, CloneConfigPickerInputController.HOVER_APPLY_HIGHLIGHT);
    }

    protected override void RegisterToolbar(ToolbarController controller)
    {
      controller.AddLeftMenuButton(TrCore.CloneTool.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Clone128.png", 20f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleCloneConfigTool), IdsCore.Messages.TutorialOnCloneTool);
    }

    protected override bool OnFirstActivated(
      ILayoutEntity hoveredEntity,
      Lyst<ILayoutEntity> selectedEntities,
      Lyst<SubTransport> selectedPartialTransports)
    {
      return false;
    }

    protected override void OnEntitiesSelected(
      IIndexable<ILayoutEntity> selectedEntities,
      IIndexable<SubTransport> selectedPartialTransports,
      bool isAreaSelection,
      bool isLeftMouse,
      RectangleTerrainArea2i? area)
    {
      Assert.That<IIndexable<SubTransport>>(selectedPartialTransports).IsEmpty<SubTransport>();
      foreach (IEntity selectedEntity in selectedEntities)
        this.RegisterPendingCommand((InputCommand) this.m_inputScheduler.ScheduleInputCmd<CloneConfigBetweenEntitiesCmd>(new CloneConfigBetweenEntitiesCmd((IEntity) this.m_entityToCloneFrom.Value, selectedEntity)));
    }

    protected override bool Matches(ILayoutEntity entity, bool isAreaSelection, bool isLeftMouse)
    {
      return entity != this.m_entityToCloneFrom && this.m_entityToCloneFrom.ValueOrNull?.GetType() == entity.GetType();
    }

    protected override void OnHoverChanged(
      IIndexable<ILayoutEntity> hoveredEntities,
      IIndexable<SubTransport> hoveredPartialTransports,
      bool isLeftClick)
    {
      this.m_iconPopup.SetHighlight(hoveredEntities.IsNotEmpty<ILayoutEntity>());
    }

    public override void Activate()
    {
      base.Activate();
      this.m_cursor.Show();
      this.m_toolbox.Show();
      if (EventSystem.current.IsPointerOverGameObject())
        return;
      Option<IRenderedEntity> option = this.m_picker.PickEntityAndSelect<IRenderedEntity>(new CursorPickingManager.EntityPredicateReturningColor<IRenderedEntity>(this.entityMatcher));
      if (!option.HasValue)
        return;
      this.startCloneFor(option.Value);
    }

    public override void Deactivate()
    {
      base.Deactivate();
      this.m_entityToCloneFrom = (Option<ILayoutEntity>) Option.None;
      this.m_cursor.Hide();
      this.m_cursorApply.Hide();
      this.m_picker.ClearPicked();
      this.m_iconPopup.Hide();
      this.m_toolbox.Hide();
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.ShortcutsManager.IsPrimaryActionOn);
      if (this.m_entityToCloneFrom.HasValue)
      {
        this.m_iconPopup.UpdatePosition();
        return base.InputUpdate(inputScheduler);
      }
      if (this.ShortcutsManager.IsSecondaryActionUp)
      {
        this.deactivateSelf();
        return true;
      }
      Option<IRenderedEntity> option = this.m_picker.PickEntityAndSelect<IRenderedEntity>(new CursorPickingManager.EntityPredicateReturningColor<IRenderedEntity>(this.entityMatcher));
      if (option.IsNone || !this.ShortcutsManager.IsPrimaryActionUp || EventSystem.current.IsPointerOverGameObject())
        return false;
      this.startCloneFor(option.Value);
      return true;
    }

    private void startCloneFor(IRenderedEntity entity)
    {
      if (!(entity is ILayoutEntity layoutEntity))
      {
        this.m_invalidSound.Play();
      }
      else
      {
        this.m_onCloneStartSound.Play();
        this.m_picker.ClearPicked();
        this.m_entityToCloneFrom = layoutEntity.SomeOption<ILayoutEntity>().As<ILayoutEntity>();
        this.m_iconPopup.SetProto(layoutEntity.Prototype);
        this.m_cursor.Hide();
        this.m_cursorApply.Show();
      }
    }

    private bool entityMatcher(IEntity entity, out ColorRgba color)
    {
      if (!(entity is IStaticEntity) || entity is Mafi.Core.Factory.Transports.Transport)
      {
        color = ColorRgba.Empty;
        return false;
      }
      color = CloneConfigPickerInputController.HOVER_HIGHLIGHT;
      return true;
    }

    private void deactivateSelf()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    static CloneConfigPickerInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CloneConfigPickerInputController.HOVER_HIGHLIGHT = new ColorRgba(30975);
      CloneConfigPickerInputController.HOVER_APPLY_HIGHLIGHT = new ColorRgba(16773632);
    }

    private class CloneConfigToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;

      public CloneConfigToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("CloneConfig", "Assets/Unity/UserInterface/Toolbar/Clone128.png", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.CopySettings__Tooltip);
        this.AddToToolbar();
      }
    }
  }
}
