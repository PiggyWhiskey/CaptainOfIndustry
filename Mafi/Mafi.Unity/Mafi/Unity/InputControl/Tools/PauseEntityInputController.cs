// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.PauseEntityInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
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
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  /// <summary>Tool for pausing/unpasing of pausable entities.</summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class PauseEntityInputController : BaseEntityCursorInputController<IAreaSelectableEntity>
  {
    private static readonly ColorRgba COLOR_HIGHLIGHT;
    private static readonly ColorRgba COLOR_HIGHLIGHT_CONFIRM;
    private readonly IInputScheduler m_inputScheduler;
    private readonly PauseEntityInputController.PauseToolbox m_toolbox;

    public PauseEntityInputController(
      ProtosDb protosDb,
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      ShortcutsManager shortcutsManager,
      IUnityInputMgr inputManager,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      ToolbarController toolbarController,
      AreaSelectionToolFactory areaSelectionToolFactory,
      IEntitiesManager entitiesManager,
      NewInstanceOf<EntityHighlighter> highlighter,
      IInputScheduler inputScheduler)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb, builder, unlockedProtosDb, shortcutsManager, inputManager, cursorPickingManager, cursorManager, areaSelectionToolFactory, entitiesManager, highlighter, (Option<NewInstanceOf<TransportTrajectoryHighlighter>>) Option.None, new Proto.ID?(IdsCore.Technology.PauseTool));
      this.m_toolbox = new PauseEntityInputController.PauseToolbox(toolbarController, builder);
      this.m_inputScheduler = inputScheduler;
      this.InitializeUi(new CursorStyle?(builder.Style.Cursors.Pause), builder.Audio.ButtonClick, PauseEntityInputController.COLOR_HIGHLIGHT, PauseEntityInputController.COLOR_HIGHLIGHT_CONFIRM);
    }

    protected override bool Matches(
      IAreaSelectableEntity entity,
      bool isAreaSelection,
      bool isLeftClick)
    {
      if (entity.IsDestroyed || entity is TransportPillar)
        return false;
      if (entity is IStaticEntity staticEntity && !staticEntity.IsConstructed)
        return true;
      if (!entity.CanBePaused)
        return false;
      return !isAreaSelection || !(entity is Mafi.Core.Factory.Transports.Transport);
    }

    protected override void OnEntitiesSelected(
      IIndexable<IAreaSelectableEntity> selectedEntities,
      IIndexable<SubTransport> selectedPartialTransports,
      bool isAreaSelection,
      bool isLeftClick,
      RectangleTerrainArea2i? area)
    {
      bool pauseOnly = this.ShortcutsManager.IsOn(this.ShortcutsManager.PauseMore);
      this.RegisterPendingCommand((InputCommand) this.m_inputScheduler.ScheduleInputCmd<ToggleEnabledGroupCmd>(new ToggleEnabledGroupCmd(selectedEntities.ToImmutableArray<IAreaSelectableEntity, EntityId>((Func<IAreaSelectableEntity, EntityId>) (x => x.Id)), pauseOnly)));
    }

    public override void Activate()
    {
      base.Activate();
      this.m_toolbox.Show();
    }

    public override void Deactivate()
    {
      base.Deactivate();
      this.m_toolbox.Hide();
    }

    protected override bool OnFirstActivated(
      IAreaSelectableEntity hoveredEntity,
      Lyst<IAreaSelectableEntity> selectedEntities,
      Lyst<SubTransport> selectedPartialTransports)
    {
      return false;
    }

    protected override void RegisterToolbar(ToolbarController controller)
    {
      controller.AddLeftMenuButton(TrCore.PauseTool.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/PauseTool.svg", 50f, (Func<ShortcutsManager, KeyBindings>) (m => m.TogglePauseTool), IdsCore.Messages.TutorialOnPauseTool);
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.ShortcutsManager.IsPrimaryActionOn);
      this.m_toolbox.PauseMoreBtn.SetIsOn(this.ShortcutsManager.IsOn(this.ShortcutsManager.PauseMore));
      return base.InputUpdate(inputScheduler);
    }

    static PauseEntityInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      PauseEntityInputController.COLOR_HIGHLIGHT = new ColorRgba(16770068, 192);
      PauseEntityInputController.COLOR_HIGHLIGHT_CONFIRM = new ColorRgba(16575069, 192);
    }

    private class PauseToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;
      public ToggleBtn PauseMoreBtn;

      public PauseToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("Pause", "Assets/Unity/UserInterface/Toolbox/PauseAndUnpause.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.PauseTool__Tooltip);
        this.PauseMoreBtn = this.AddToggleButton("PauseMore", "Assets/Unity/UserInterface/Toolbox/PauseOnly.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PauseMore), (LocStrFormatted) Tr.PauseTool__PauseOnlyTooltip);
        this.AddToToolbar();
      }
    }
  }
}
