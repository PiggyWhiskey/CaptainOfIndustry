// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.UpointsToolInputController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Designation;
using Mafi.Localization;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity.Utils;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  /// <summary>
  /// Tool to smartly apply unity on entities - boost, quick delivery / remove.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class UpointsToolInputController : BaseEntityCursorInputController<IStaticEntity>
  {
    private static readonly ColorRgba HOVER_HIGHLIGHT;
    private readonly IInputScheduler m_inputScheduler;
    private readonly TerrainManager m_terrainManager;
    private readonly ISurfaceDesignationsManager m_surfaceDesignationsMgr;
    private readonly MessagesCenterController m_messagesCenterController;
    private readonly FloatingUpointsCostPopup m_costPopup;
    private readonly FloatingPricePopup m_errorPopup;
    private readonly IAssetTransactionManager m_assetTransactionManager;
    private readonly IUnityInputMgr m_inputManager;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly UpointsToolInputController.UpointsToolToolbox m_toolbox;
    private readonly IActivator m_surfaceDesignationsActivator;
    private int m_surfaceTilesTotal;
    private readonly Lyst<IStaticEntity> m_entitiesToEvaluate;
    private readonly Lyst<IStaticEntity> m_entitiesForQuickDeliverTmp;
    private readonly Lyst<IEntityWithBoost> m_entitiesForBoost;
    private readonly string m_defaultTitle;
    private RectangleTerrainArea2i? m_areaToProcess;
    private bool m_selectionChanged;

    public override bool AllowAreaOnlySelection => true;

    public UpointsToolInputController(
      ProtosDb protosDb,
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      ShortcutsManager shortcutsManager,
      IAssetTransactionManager assetTransactionManager,
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoopEvents,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      AreaSelectionToolFactory areaSelectionToolFactory,
      NewInstanceOf<EntityHighlighter> highlighter,
      EntitiesManager entitiesManager,
      ToolbarController toolbarController,
      IInputScheduler inputScheduler,
      TerrainManager terrainManager,
      ISurfaceDesignationsManager surfaceDesignationsMgr,
      MessagesCenterController messagesCenterController,
      SurfaceDesignationsRenderer surfaceDesignationsRenderer,
      NewInstanceOf<FloatingUpointsCostPopup> costPopup,
      NewInstanceOf<FloatingPricePopup> errorPopup)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_entitiesToEvaluate = new Lyst<IStaticEntity>();
      this.m_entitiesForQuickDeliverTmp = new Lyst<IStaticEntity>();
      this.m_entitiesForBoost = new Lyst<IEntityWithBoost>();
      LocStr locStr = Tr.QuickBuild__Action;
      string str1 = locStr.ToString();
      locStr = Tr.QuickRemove__Action;
      string str2 = locStr.ToString();
      this.m_defaultTitle = str1 + " / " + str2;
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb, builder, unlockedProtosDb, shortcutsManager, inputManager, cursorPickingManager, cursorManager, areaSelectionToolFactory, (IEntitiesManager) entitiesManager, highlighter, (Option<NewInstanceOf<TransportTrajectoryHighlighter>>) Option.None, new Proto.ID?(IdsCore.Technology.UnityTool));
      this.m_assetTransactionManager = assetTransactionManager;
      this.m_inputManager = inputManager;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_inputScheduler = inputScheduler;
      this.m_terrainManager = terrainManager;
      this.m_surfaceDesignationsMgr = surfaceDesignationsMgr;
      this.m_messagesCenterController = messagesCenterController;
      this.m_costPopup = costPopup.Instance;
      this.m_errorPopup = errorPopup.Instance;
      this.m_toolbox = new UpointsToolInputController.UpointsToolToolbox(toolbarController, builder);
      this.m_surfaceDesignationsActivator = surfaceDesignationsRenderer.CreateActivator();
      this.InitializeUi(new CursorStyle?(builder.Style.Cursors.Upoints), builder.Audio.Assign, UpointsToolInputController.HOVER_HIGHLIGHT, UpointsToolInputController.HOVER_HIGHLIGHT);
    }

    protected override void RegisterToolbar(ToolbarController controller)
    {
      controller.AddLeftMenuButton(TrCore.UnityTool.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/UpointsTool.svg", 40f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleUpointsTool), IdsCore.Messages.TutorialOnUnityTool);
    }

    protected override void OnUpdateSelectionSync(RectangleTerrainArea2i area)
    {
      base.OnUpdateSelectionSync(area);
      this.m_areaToProcess = new RectangleTerrainArea2i?(area);
      this.m_selectionChanged = true;
    }

    private void onSync(GameTime time)
    {
      if (!this.m_selectionChanged)
        return;
      this.m_selectionChanged = false;
      string title = this.m_defaultTitle;
      Upoints cost = Upoints.Zero;
      this.m_entitiesForQuickDeliverTmp.Clear();
      this.m_entitiesForBoost.Clear();
      if (this.m_areaToProcess.HasValue)
      {
        this.m_surfaceTilesTotal = 0;
        foreach (Tile2iAndIndex enumerateTilesAndIndex in this.m_areaToProcess.Value.EnumerateTilesAndIndices(this.m_terrainManager))
        {
          Option<SurfaceDesignation> designationAt = this.m_surfaceDesignationsMgr.GetDesignationAt(enumerateTilesAndIndex.TileCoord);
          if (!designationAt.IsNone && !designationAt.Value.IsFulfilled)
            ++this.m_surfaceTilesTotal;
        }
        cost += this.m_surfaceTilesTotal * EntitiesCommandsProcessor.UnityPerSurfaceTile;
      }
      int noOfEntitiesBannedForDelivery = 0;
      if (this.m_entitiesToEvaluate.IsNotEmpty)
      {
        bool enableBoost;
        EntitiesCommandsProcessor.EvaluateUnitySpending(this.m_surfaceTilesTotal > 0, this.m_entitiesToEvaluate, this.m_entitiesForQuickDeliverTmp, this.m_entitiesForBoost, out enableBoost, out noOfEntitiesBannedForDelivery);
        Upoints? nullable;
        foreach (IStaticEntity staticEntity in this.m_entitiesForQuickDeliverTmp)
        {
          IEntityConstructionProgress valueOrNull = staticEntity.ConstructionProgress.ValueOrNull;
          if (valueOrNull == null)
            Log.Warning("Entity evaluated for quick delivery but has no construction state.");
          else if (valueOrNull.IsDeconstruction)
          {
            Upoints upoints1 = cost;
            nullable = valueOrNull.CostForQuickRemove(this.m_assetTransactionManager);
            Upoints upoints2 = nullable ?? Upoints.Zero;
            cost = upoints1 + upoints2;
          }
          else
          {
            bool hasProducts;
            Upoints upoints = valueOrNull.CostForQuickBuild(this.m_assetTransactionManager, out hasProducts);
            if (hasProducts)
              cost += upoints;
          }
        }
        if (this.m_entitiesForBoost.IsNotEmpty)
        {
          Assert.That<Lyst<IStaticEntity>>(this.m_entitiesForQuickDeliverTmp).IsEmpty<IStaticEntity>();
          title = (enableBoost ? Tr.BoostMachine__Enable : Tr.BoostMachine__Disable).TranslatedString;
          foreach (IEntityWithBoost entityWithBoost in this.m_entitiesForBoost)
          {
            nullable = entityWithBoost.BoostCost;
            if (nullable.HasValue)
            {
              Upoints upoints = cost;
              Upoints zero;
              if (!entityWithBoost.IsBoostRequested)
              {
                nullable = entityWithBoost.BoostCost;
                zero = nullable.Value;
              }
              else
                zero = Upoints.Zero;
              cost = upoints + zero;
            }
          }
        }
        this.m_entitiesForQuickDeliverTmp.Clear();
        this.m_entitiesForBoost.Clear();
      }
      if (cost.IsNotPositive)
      {
        this.m_costPopup.Hide();
        if (noOfEntitiesBannedForDelivery > 0 && noOfEntitiesBannedForDelivery == this.m_entitiesToEvaluate.Count)
        {
          this.m_errorPopup.SetErrorMessage((LocStrFormatted) Tr.QuickBuild__NotAllowed);
          this.m_errorPopup.Show();
        }
        else
          this.m_errorPopup.Hide();
      }
      else
      {
        this.m_costPopup.SetUpointsCost(cost, title);
        this.m_errorPopup.Hide();
      }
    }

    protected override bool OnFirstActivated(
      IStaticEntity hoveredEntity,
      Lyst<IStaticEntity> selectedEntities,
      Lyst<SubTransport> selectedPartialTransports)
    {
      return false;
    }

    protected override void OnEntitiesSelected(
      IIndexable<IStaticEntity> selectedEntities,
      IIndexable<SubTransport> selectedPartialTransports,
      bool isAreaSelection,
      bool isLeftMouse,
      RectangleTerrainArea2i? area)
    {
      if (selectedEntities.IsEmpty<IStaticEntity>() && this.m_surfaceTilesTotal <= 0)
        return;
      Assert.That<IIndexable<SubTransport>>(selectedPartialTransports).IsEmpty<SubTransport>();
      this.RegisterPendingCommand((InputCommand) this.m_inputScheduler.ScheduleInputCmd<SpendUpointsOnEntitiesCmd>(new SpendUpointsOnEntitiesCmd(selectedEntities.ToImmutableArray<IStaticEntity, EntityId>((Func<IStaticEntity, EntityId>) (x => x.Id)), area)));
    }

    protected override bool Matches(IStaticEntity entity, bool isAreaSelection, bool isLeftMouse)
    {
      if (entity.ConstructionState == ConstructionState.NotInitialized || entity is TransportPillar)
        return false;
      if (!entity.IsConstructed)
        return true;
      return entity is IEntityWithBoost entityWithBoost && entityWithBoost.BoostCost.HasValue;
    }

    public override void Activate()
    {
      base.Activate();
      this.m_surfaceDesignationsActivator.Activate();
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<UpointsToolInputController>(this, new Action<GameTime>(this.onSync));
      this.m_toolbox.Show();
    }

    public override void Deactivate()
    {
      base.Deactivate();
      this.m_surfaceDesignationsActivator.Deactivate();
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<UpointsToolInputController>(this, new Action<GameTime>(this.onSync));
      this.m_costPopup.Hide();
      this.m_errorPopup.Hide();
      this.m_toolbox.Hide();
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.ShortcutsManager.IsPrimaryActionOn);
      this.m_errorPopup.UpdatePosition();
      this.m_costPopup.UpdatePosition();
      return base.InputUpdate(inputScheduler);
    }

    protected override void OnHoverChanged(
      IIndexable<IStaticEntity> hoveredEntities,
      IIndexable<SubTransport> hoveredPartialTransports,
      bool isLeftClick)
    {
      this.m_entitiesToEvaluate.Clear();
      if (hoveredEntities.IsNotEmpty<IStaticEntity>())
        this.m_entitiesToEvaluate.AddRange(hoveredEntities);
      this.m_selectionChanged = true;
    }

    static UpointsToolInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      UpointsToolInputController.HOVER_HIGHLIGHT = new ColorRgba(6364589);
    }

    private class UpointsToolToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;

      public UpointsToolToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("UpointsTool", "Assets/Unity/UserInterface/Toolbar/UpointsTool.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.UnityTool__Tooltip);
        this.AddToToolbar();
      }
    }
  }
}
