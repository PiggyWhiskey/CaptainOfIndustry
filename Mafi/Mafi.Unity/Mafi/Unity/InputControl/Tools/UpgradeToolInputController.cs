// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.UpgradeToolInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
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
  /// <summary>Tool for entities upgrade.</summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class UpgradeToolInputController : BaseEntityCursorInputController<IAreaSelectableEntity>
  {
    private static readonly ColorRgba COLOR_HIGHLIGHT;
    private static readonly ColorRgba COLOR_HIGHLIGHT_CONFIRM;
    private readonly IGameLoopEvents m_gameLoop;
    private readonly IUnityInputMgr m_inputManager;
    private readonly ToolbarController m_toolbarController;
    private readonly IInputScheduler m_inputScheduler;
    private readonly UnlockedProtosDbForUi m_unlockedProtosDb;
    private readonly DependencyResolver m_resolver;
    private readonly UiBuilder m_builder;
    private readonly UpgradeToolInputController.UpgradeToolbox m_toolbox;
    private UpgradeToolWindowView m_windowView;

    public override ControllerConfig Config
    {
      get => !this.m_windowView.IsVisible ? ControllerConfig.Tool : ControllerConfig.Window;
    }

    public UpgradeToolInputController(
      ProtosDb protosDb,
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      IGameLoopEvents gameLoop,
      ShortcutsManager shortcutsManager,
      IUnityInputMgr inputManager,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      ToolbarController toolbarController,
      AreaSelectionToolFactory areaSelectionToolFactory,
      IEntitiesManager entitiesManager,
      NewInstanceOf<EntityHighlighter> highlighter,
      IInputScheduler inputScheduler,
      DependencyResolver resolver,
      NewInstanceOf<UpgradeToolWindowView> windowView)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb, builder, unlockedProtosDb, shortcutsManager, inputManager, cursorPickingManager, cursorManager, areaSelectionToolFactory, entitiesManager, highlighter, (Option<NewInstanceOf<TransportTrajectoryHighlighter>>) Option.None, new Proto.ID?(IdsCore.Technology.UpgradeTool));
      this.m_gameLoop = gameLoop;
      this.m_inputManager = inputManager;
      this.m_toolbox = new UpgradeToolInputController.UpgradeToolbox(toolbarController, builder);
      this.m_toolbarController = toolbarController;
      this.m_inputScheduler = inputScheduler;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_resolver = resolver;
      this.m_builder = builder;
      this.m_windowView = windowView.Instance;
      this.m_windowView.SetOnCloseButtonClickAction(new Action(this.DeactivateSelf));
      this.SetEdgeSizeLimit(new RelTile1i(400));
      this.ClearSelectionOnDeactivateOnly();
      this.SetUpRightClickAreaSelection(ColorRgba.Red);
      this.InitializeUi(new CursorStyle?(builder.Style.Cursors.Upgrade), builder.Audio.ButtonClick, UpgradeToolInputController.COLOR_HIGHLIGHT, UpgradeToolInputController.COLOR_HIGHLIGHT_CONFIRM);
    }

    protected override void RegisterToolbar(ToolbarController controller)
    {
      this.m_toolbarController.AddLeftMenuButton(TrCore.UpgradeTool.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Upgrade.svg", 70f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleUpgradeTool));
    }

    protected override void OnEntitiesSelected(
      IIndexable<IAreaSelectableEntity> selectedEntities,
      IIndexable<SubTransport> selectedPartialTransports,
      bool isAreaSelection,
      bool isLeftClick,
      RectangleTerrainArea2i? area)
    {
      if (!isLeftClick)
      {
        foreach (IEntity selectedEntity in selectedEntities)
          this.m_inputScheduler.ScheduleInputCmd<ToggleStaticEntityConstructionCmd>(new ToggleStaticEntityConstructionCmd(selectedEntity.Id));
        this.DeactivateSelf();
      }
      else if (selectedEntities.Count == 1)
      {
        this.m_inputScheduler.ScheduleInputCmd<UpgradeEntityCmd>(new UpgradeEntityCmd(selectedEntities[0].Id));
      }
      else
      {
        this.m_windowView.BuildAndShow(this.m_builder);
        this.m_windowView.SetEntities(selectedEntities);
        this.HideCursor();
      }
    }

    public override void Activate()
    {
      base.Activate();
      this.m_toolbox.Show();
      this.m_gameLoop.SyncUpdate.AddNonSaveable<UpgradeToolInputController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.AddNonSaveable<UpgradeToolInputController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public override void Deactivate()
    {
      base.Deactivate();
      this.m_toolbox.Hide();
      this.m_windowView.Hide();
      this.m_gameLoop.SyncUpdate.RemoveNonSaveable<UpgradeToolInputController>(this, new Action<GameTime>(this.syncUpdate));
      this.m_gameLoop.RenderUpdate.RemoveNonSaveable<UpgradeToolInputController>(this, new Action<GameTime>(this.renderUpdate));
    }

    public void DeactivateSelf()
    {
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    private void renderUpdate(GameTime gameTime)
    {
      if (!this.m_windowView.IsVisible)
        return;
      this.m_windowView.RenderUpdate(gameTime);
    }

    private void syncUpdate(GameTime gameTime)
    {
      if (!this.m_windowView.IsVisible)
        return;
      this.m_windowView.SyncUpdate(gameTime);
    }

    protected override bool OnFirstActivated(
      IAreaSelectableEntity hoveredEntity,
      Lyst<IAreaSelectableEntity> selectedEntities,
      Lyst<SubTransport> selectedPartialTransports)
    {
      return false;
    }

    protected override bool Matches(
      IAreaSelectableEntity entity,
      bool isAreaSelection,
      bool isLeftClick)
    {
      if (entity.IsDestroyed || entity is TransportPillar)
        return false;
      if (!isLeftClick)
      {
        if (!(entity is IStaticEntity staticEntity))
          return false;
        bool flag;
        switch (staticEntity.ConstructionState)
        {
          case ConstructionState.PreparingUpgrade:
          case ConstructionState.BeingUpgraded:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        return flag;
      }
      if (entity is IStaticEntity staticEntity1 && !staticEntity1.IsConstructed)
        return false;
      IUpgradableEntity upgradableEntity = entity as IUpgradableEntity;
      return upgradableEntity != null && isUpgradeVisible();

      bool isUpgradeVisible()
      {
        return upgradableEntity.Upgrader.NextTier.HasValue && upgradableEntity.IsConstructed && upgradableEntity.Prototype.Id != Ids.Buildings.Shipyard && this.m_unlockedProtosDb.IsUnlocked((IProto) upgradableEntity.Upgrader.NextTier.Value);
      }
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (this.m_windowView.IsVisible)
        return false;
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.ShortcutsManager.IsPrimaryActionOn);
      this.m_toolbox.UndoUpgradeBtn.SetIsOn(this.ShortcutsManager.IsSecondaryActionOn);
      return base.InputUpdate(inputScheduler);
    }

    static UpgradeToolInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      UpgradeToolInputController.COLOR_HIGHLIGHT = new ColorRgba(16770068, 192);
      UpgradeToolInputController.COLOR_HIGHLIGHT_CONFIRM = new ColorRgba(16575069, 192);
    }

    private class UpgradeToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;
      public ToggleBtn UndoUpgradeBtn;

      public UpgradeToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("Upgrade", "Assets/Unity/UserInterface/Toolbar/Upgrade.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.UpgradeTool__Tooltip);
        this.UndoUpgradeBtn = this.AddToggleButton("Undo upgrade", "Assets/Unity/UserInterface/General/Clear128.png", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.SecondaryAction), (LocStrFormatted) Tr.UpgradeTool__CancelTooltip);
        this.AddToToolbar();
      }
    }
  }
}
