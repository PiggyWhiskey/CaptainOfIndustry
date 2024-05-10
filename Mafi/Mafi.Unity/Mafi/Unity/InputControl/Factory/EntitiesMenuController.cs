// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.EntitiesMenuController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.UiState;
using Mafi.Unity.InputControl.LayoutEntityPlacing;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.InputControl.Toolbar.EntitiesMenu;
using Mafi.Unity.InputControl.Toolbar.MenuPopup;
using Mafi.Unity.Mine;
using Mafi.Unity.Ports.Io;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using System;
using System.Collections.Generic;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  /// <summary>
  /// Represents a menu of some set of entities. Typically registered via <see cref="T:Mafi.Unity.InputControl.Factory.ToolbarCategoriesMenuBuilder" />.
  /// </summary>
  internal class EntitiesMenuController : 
    IToolbarItemController,
    IUnityInputController,
    IToolbarItemRegistrar
  {
    private readonly IGameLoopEvents m_gameLoop;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly UnlockedProtosDbForUi m_unlockedProtos;
    private readonly NewProtosTracker m_newProtosTracker;
    private readonly IAssetTransactionManager m_assetTransactionManager;
    private readonly MenuPopupController m_popupController;
    private readonly ToolbarController m_toolbarController;
    private readonly TransportBuildController m_transportBuildController;
    private readonly Mafi.Unity.InputControl.Toolbar.EntitiesMenu.ExpressionEvaluator m_expressionEvaluator;
    private readonly IUnityInputMgr m_inputMgr;
    private readonly StaticEntityMassPlacer m_entityPlacer;
    private readonly TerrainCursor m_terrainCursor;
    private readonly Action m_entityPlacedOrCanceled;
    private readonly IoPortsRenderer m_portsRenderer;
    private readonly EntitiesMenuStripInfoView m_activePlanningView;
    private readonly EntitiesMenuView m_view;
    private readonly SurfaceDesignationController m_surfaceDesignationController;
    private readonly DecalDesignationController m_decalDesignationController;
    /// <summary>
    /// Whether transport build tool is active while entity placing is not active.
    /// </summary>
    private bool m_isTransportBuildAllowed;
    private IconContainer m_newItemsIcon;
    private Set<EntitiesMenuItem> m_thisMenuItems;
    private readonly IActivator m_surfaceDesignationsActivator;

    public event Action<IToolbarItemController> VisibilityChanged;

    public bool IsVisible { get; private set; }

    public bool IsActive { get; private set; }

    public bool DeactivateShortcutsIfNotVisible => false;

    private bool TransportProtoSelected
    {
      get
      {
        return this.m_transportBuildController.IsActive && this.m_transportBuildController.TransportProtoSelected;
      }
    }

    public ControllerConfig Config
    {
      get
      {
        return !this.m_entityPlacer.IsActive && !this.TransportProtoSelected ? ControllerConfig.Menu : ControllerConfig.MenuActive;
      }
    }

    public IReadOnlySet<EntitiesMenuItem> ThisMenuItems
    {
      get => (IReadOnlySet<EntitiesMenuItem>) this.m_thisMenuItems;
    }

    internal EntitiesMenuController(
      IGameLoopEvents gameLoop,
      ShortcutsManager shortcutsManager,
      UnlockedProtosDbForUi unlockedProtos,
      NewProtosTracker newProtosTracker,
      IAssetTransactionManager assetTransactionManager,
      MenuPopupController popupController,
      ToolbarController toolbarController,
      TransportBuildController transportBuildController,
      Mafi.Unity.InputControl.Toolbar.EntitiesMenu.ExpressionEvaluator expressionEvaluator,
      NewInstanceOf<TerrainCursor> terrainCursor,
      NewInstanceOf<StaticEntityMassPlacer> entityPlacer,
      IUnityInputMgr inputMgr,
      IoPortsRenderer portsRenderer,
      EntitiesMenuStripInfoView activePlanningView,
      EntitiesMenuView entitiesMenuView,
      SurfaceDesignationController surfaceDesignationController,
      DecalDesignationController decalDesignationController,
      SurfaceDesignationsRenderer surfaceDesignationsRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoop = gameLoop;
      this.m_shortcutsManager = shortcutsManager;
      this.m_unlockedProtos = unlockedProtos;
      this.m_newProtosTracker = newProtosTracker;
      this.m_assetTransactionManager = assetTransactionManager;
      this.m_popupController = popupController;
      this.m_toolbarController = toolbarController;
      this.m_transportBuildController = transportBuildController;
      this.m_expressionEvaluator = expressionEvaluator;
      this.m_inputMgr = inputMgr;
      this.m_portsRenderer = portsRenderer;
      this.m_activePlanningView = activePlanningView;
      this.m_view = entitiesMenuView;
      this.m_surfaceDesignationController = surfaceDesignationController;
      this.m_decalDesignationController = decalDesignationController;
      this.m_terrainCursor = terrainCursor.Instance;
      this.m_popupController = popupController;
      this.m_entityPlacer = entityPlacer.Instance;
      this.m_entityPlacedOrCanceled = new Action(this.entityPlacedOrCancelled);
      this.m_surfaceDesignationsActivator = surfaceDesignationsRenderer.CreateActivator();
    }

    public void RegisterIntoToolbar(ToolbarController controller)
    {
    }

    public void BuildUi(
      UiBuilder builder,
      ToolbarCategoryProto category,
      IEnumerable<EntitiesMenuItem> thisMenuItems,
      bool isTransportBuildAllowed)
    {
      this.m_thisMenuItems = thisMenuItems.ToSet<EntitiesMenuItem>();
      Func<ShortcutsManager, KeyBindings> shortcut = (Func<ShortcutsManager, KeyBindings>) null;
      if (category.ShortcutId == "TRANSPORT")
        shortcut = (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleTransportMenu);
      else if (category.ShortcutId == "SURFACE")
        shortcut = (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleSurfacingTool);
      else if (category.ShortcutId.IsNotEmpty())
        Log.Error("Not supported shortcut");
      ToggleBtn parent = this.m_toolbarController.AddMainMenuButtonAndReturn(category.Strings.Name.TranslatedString, (IToolbarItemController) this, category.IconPath, category.Order, shortcut);
      this.m_newItemsIcon = builder.NewIconContainer("Icon").SetIcon("Assets/Unity/UserInterface/General/Circle.svg", (ColorRgba) 16756491).PutToRightTopOf<IconContainer>((IUiElement) parent, 7.Vector2(), Offset.Right(0.0f) + Offset.Top(4f)).Hide<IconContainer>();
      this.m_isTransportBuildAllowed = isTransportBuildAllowed;
      this.UpdateNewItemBadge();
      this.m_unlockedProtos.OnUnlockedSetChangedForUi += new Action(this.UpdateNewItemBadge);
      this.updateEntitiesCount(true);
    }

    private void updateEntitiesCount() => this.updateEntitiesCount(false);

    private void updateEntitiesCount(bool isInit)
    {
      bool flag = false;
      foreach (EntitiesMenuItem thisMenuItem in this.m_thisMenuItems)
      {
        if (thisMenuItem.IsUnlocked(this.m_unlockedProtos))
        {
          flag = true;
          break;
        }
      }
      if (this.IsVisible != flag)
      {
        this.IsVisible = flag;
        Action<IToolbarItemController> visibilityChanged = this.VisibilityChanged;
        if (visibilityChanged != null)
          visibilityChanged((IToolbarItemController) this);
      }
      if (this.IsVisible)
      {
        if (isInit)
          return;
        this.m_unlockedProtos.OnUnlockedSetChangedForUi -= new Action(this.updateEntitiesCount);
      }
      else
      {
        if (!isInit)
          return;
        this.m_unlockedProtos.OnUnlockedSetChangedForUi += new Action(this.updateEntitiesCount);
      }
    }

    public void UpdateNewItemBadge()
    {
      bool visibility = false;
      foreach (EntitiesMenuItem thisMenuItem in (IEnumerable<EntitiesMenuItem>) this.ThisMenuItems)
      {
        if (thisMenuItem.Proto.HasValue && thisMenuItem.IsUnlocked(this.m_unlockedProtos) && this.m_newProtosTracker.IsNew(thisMenuItem.Proto.Value))
        {
          visibility = true;
          break;
        }
      }
      this.m_newItemsIcon.SetVisibility<IconContainer>(visibility);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (UnityEngine.Input.GetKeyDown(this.m_shortcutsManager.Cancel))
      {
        if (this.m_transportBuildController.IsActive && this.m_transportBuildController.TransportProtoSelected)
        {
          this.m_transportBuildController.CancelTransport();
          return true;
        }
        if (this.m_entityPlacer.IsActive)
        {
          this.m_entityPlacer.Deactivate(true);
          this.m_view.ClearSelected();
          return true;
        }
        if (!this.m_surfaceDesignationController.IsActive)
          return false;
        this.m_surfaceDesignationController.Deactivate();
        return false;
      }
      if (this.m_entityPlacer.IsActive)
        return this.m_entityPlacer.InputUpdate(inputScheduler);
      if (this.m_surfaceDesignationController.IsActive)
        return this.m_surfaceDesignationController.InputUpdate(inputScheduler);
      if (this.m_isTransportBuildAllowed)
      {
        if (this.m_inputMgr.ActiveControllers.LastOrDefault<IUnityInputController>() == this && !this.m_transportBuildController.IsActive)
          this.activateTransportBuildIfAllowed(Option<TransportProto>.None);
        if (this.m_transportBuildController.IsActive)
          return this.m_transportBuildController.InputUpdate(inputScheduler);
      }
      return false;
    }

    /// <summary>
    /// Deactivates current tool - transport building or entity placing - depending on which one is active.
    /// </summary>
    private void deactivateCurrentTool(bool clearMenu = true)
    {
      if (clearMenu)
        this.m_view.ClearSelected();
      if (this.m_entityPlacer.IsActive)
        this.cancelLayoutEntity();
      else if (this.m_transportBuildController.IsActive)
      {
        this.deactivateTransportBuild();
      }
      else
      {
        if (!this.m_surfaceDesignationController.IsActive)
          return;
        this.m_surfaceDesignationController.Deactivate();
      }
    }

    private void activateTransportBuildIfAllowed(Option<TransportProto> transportProto)
    {
      if (this.m_isTransportBuildAllowed && !this.m_transportBuildController.IsActive)
      {
        this.m_transportBuildController.Activate();
        this.m_transportBuildController.OnProtoSelected += new Action<Option<TransportProto>>(this.transportProtoSelected);
      }
      if (!transportProto.HasValue)
        return;
      this.m_transportBuildController.SelectProto(transportProto.Value);
    }

    private void deactivateTransportBuild()
    {
      this.m_transportBuildController.OnProtoSelected -= new Action<Option<TransportProto>>(this.transportProtoSelected);
      this.m_transportBuildController.Deactivate();
    }

    private void entityPlacedOrCancelled()
    {
      if (this.IsActive)
      {
        this.cancelLayoutEntity();
        this.activateTransportBuildIfAllowed(Option<TransportProto>.None);
      }
      this.m_view.ClearSelected();
    }

    private void surfacePlacementCancelled()
    {
      int num = this.IsActive ? 1 : 0;
      this.m_view.ClearSelected();
    }

    private void cancelLayoutEntity()
    {
      if (!this.m_entityPlacer.IsActive)
        return;
      this.m_entityPlacer.Deactivate();
    }

    public void Activate()
    {
      Assert.That<bool>(this.IsVisible).IsTrue();
      if (this.IsActive)
      {
        Log.Warning("LayoutEntityMenuController is already activated!");
      }
      else
      {
        this.IsActive = true;
        this.m_view.SetControllerAndShow(this);
        this.m_popupController.Activate();
        this.m_terrainCursor.Activate();
        this.m_terrainCursor.RelativeHeight = ThicknessTilesI.Zero;
        this.activateTransportBuildIfAllowed((Option<TransportProto>) Option.None);
        this.m_surfaceDesignationsActivator.Activate();
        this.m_inputMgr.ControllerActivated += new Action<IUnityInputController>(this.onOtherControllerActivated);
        this.m_gameLoop.RenderUpdate.AddNonSaveable<EntitiesMenuController>(this, new Action<GameTime>(this.renderUpdate));
        this.m_gameLoop.SyncUpdate.AddNonSaveable<EntitiesMenuController>(this, new Action<GameTime>(this.syncUpdate));
      }
    }

    public void Deactivate()
    {
      if (!this.IsActive)
      {
        Log.Warning("LayoutEntityMenuController is already deactivated!");
      }
      else
      {
        this.IsActive = false;
        this.m_inputMgr.ControllerActivated -= new Action<IUnityInputController>(this.onOtherControllerActivated);
        this.m_gameLoop.RenderUpdate.RemoveNonSaveable<EntitiesMenuController>(this, new Action<GameTime>(this.renderUpdate));
        this.m_gameLoop.SyncUpdate.RemoveNonSaveable<EntitiesMenuController>(this, new Action<GameTime>(this.syncUpdate));
        this.m_surfaceDesignationsActivator.Deactivate();
        this.deactivateCurrentTool();
        this.m_view.Hide();
        this.m_popupController.Deactivate();
        this.m_terrainCursor.Deactivate();
      }
    }

    public void FocusSearchBox() => this.m_view.FocusSearchBox();

    private void syncUpdate(GameTime gameTime) => this.m_view.SyncUpdate(gameTime);

    private void renderUpdate(GameTime gameTime) => this.m_view.RenderUpdate(gameTime);

    public void OnItemSelected(EntitiesMenuItem item)
    {
      this.m_inputMgr.OnBuildModeActivated((IUnityInputController) this);
      if (item is TerrainTileSurfaceMenuItem tileSurfaceMenuItem)
      {
        this.deactivateCurrentTool(false);
        this.m_surfaceDesignationController.ActivateFor(tileSurfaceMenuItem.Proto, new Action(this.surfacePlacementCancelled));
      }
      else if (item is PaintSurfaceDecalsMenuItem)
      {
        this.m_inputMgr.ActivateNewController((IUnityInputController) this.m_decalDesignationController);
      }
      else
      {
        if (!item.Proto.HasValue)
          return;
        this.SelectProto(item.Proto.Value);
      }
    }

    /// <summary>Called when user selects an item in the menu.</summary>
    public void SelectProto(Proto entity)
    {
      this.deactivateCurrentTool(false);
      switch (entity)
      {
        case TransportProto transportProto:
          this.activateTransportBuildIfAllowed((Option<TransportProto>) transportProto);
          break;
        case LayoutEntityProto proto1:
          this.m_entityPlacer.Activate((object) this, this.m_entityPlacedOrCanceled, this.m_entityPlacedOrCanceled);
          StaticEntityMassPlacer entityPlacer1 = this.m_entityPlacer;
          TileTransform? transform1 = new TileTransform?();
          Option<EntityConfigData> config1 = new Option<EntityConfigData>();
          entityPlacer1.SetLayoutEntityToPlace((ILayoutEntityProto) proto1, transform: transform1, config: config1);
          break;
        case TreeProto proto2:
          this.m_entityPlacer.Activate((object) this, this.m_entityPlacedOrCanceled, this.m_entityPlacedOrCanceled);
          StaticEntityMassPlacer entityPlacer2 = this.m_entityPlacer;
          TileTransform? transform2 = new TileTransform?();
          Option<EntityConfigData> config2 = new Option<EntityConfigData>();
          entityPlacer2.SetLayoutEntityToPlace((ILayoutEntityProto) proto2, transform: transform2, config: config2);
          break;
        default:
          Assert.Fail("Unknown item was selected in the menu.");
          break;
      }
    }

    private void transportProtoSelected(Option<TransportProto> transportProto)
    {
      if (transportProto.HasValue)
        this.m_view.SetSelected((Proto) transportProto.Value);
      else
        this.m_view.ClearSelected();
    }

    private void onOtherControllerActivated(IUnityInputController controller)
    {
      if (controller.Config.Group == ControllerGroup.AlwaysActive)
        return;
      this.deactivateCurrentTool();
    }
  }
}
