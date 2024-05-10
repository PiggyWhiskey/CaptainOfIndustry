// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.CutPasteEntityInputController
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
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
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
using Mafi.Unity.InputControl.LayoutEntityPlacing;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class CutPasteEntityInputController : CopyEntityInputControllerBase
  {
    private static readonly ColorRgba HOVER_HIGHLIGHT;
    private readonly EntitiesManager m_entitiesManager;
    private readonly TransportsManager m_transportsManager;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly AssetTransactionManager m_assetTransactionManager;
    private readonly Lyst<IStaticEntity> m_entitiesToCut;
    private readonly Lyst<SubTransport> m_partialTransportsToCut;
    private LystStruct<TileSurfaceCopyPasteData> m_surfacesToCut;
    private readonly Dict<IStaticEntity, EntityConfigData> m_configsMap;
    private readonly Lyst<EntityConfigData> m_cutConfigs;
    private readonly CutPasteEntityInputController.CutPasteToolbox m_toolbox;
    private bool m_performCut;

    public CutPasteEntityInputController(
      ProtosDb protosDb,
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      ShortcutsManager shortcutsManager,
      ToolbarController toolbarController,
      IUnityInputMgr inputManager,
      IGameLoopEvents gameLoopEvents,
      IInputScheduler inputScheduler,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      NewInstanceOf<StaticEntityMassPlacer> entityPlacer,
      AreaSelectionToolFactory areaSelectionToolFactory,
      NewInstanceOf<FloatingPricePopup> pricePopup,
      NewInstanceOf<EntityHighlighter> highlighter,
      NewInstanceOf<TransportTrajectoryHighlighter> transportTrajectoryHighlighter,
      EntitiesManager entitiesManager,
      EntitiesCloneConfigHelper configCloneHelper,
      TransportBuildController transportBuildController,
      TransportsManager transportsManager,
      TerrainOccupancyManager occupancyManager,
      AssetTransactionManager assetTransactionManager,
      TerrainManager terrainManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_entitiesToCut = new Lyst<IStaticEntity>();
      this.m_partialTransportsToCut = new Lyst<SubTransport>();
      this.m_configsMap = new Dict<IStaticEntity, EntityConfigData>();
      this.m_cutConfigs = new Lyst<EntityConfigData>();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb, builder, unlockedProtosDb, shortcutsManager, inputManager, inputScheduler, cursorPickingManager, cursorManager, entityPlacer, areaSelectionToolFactory, pricePopup, highlighter, transportTrajectoryHighlighter, entitiesManager, configCloneHelper, transportBuildController, (Mafi.Unity.InputControl.Toolbar.Toolbox) new CutPasteEntityInputController.CutPasteToolbox(toolbarController, builder), new Proto.ID?(IdsCore.Technology.CutTool), terrainManager);
      this.m_transportsManager = transportsManager;
      this.m_occupancyManager = occupancyManager;
      this.m_assetTransactionManager = assetTransactionManager;
      this.m_entitiesManager = entitiesManager;
      this.m_toolbox = (CutPasteEntityInputController.CutPasteToolbox) this.Toolbox;
      this.InitializeUi(new CursorStyle?(builder.Style.Cursors.Cut), builder.Audio.EntitySelect, CutPasteEntityInputController.HOVER_HIGHLIGHT, CutPasteEntityInputController.HOVER_HIGHLIGHT);
      gameLoopEvents.SyncUpdate.AddNonSaveable<CutPasteEntityInputController>(this, new Action<GameTime>(this.onSync));
    }

    protected override void RegisterToolbar(ToolbarController controller)
    {
      controller.AddLeftMenuButton(TrCore.CutTool.TranslatedString, (IToolbarItemController) this, "Assets/Unity/UserInterface/Toolbar/Cut.svg", 0.0f, (Func<ShortcutsManager, KeyBindings>) (m => m.ToggleInstaCutTool), IdsCore.Messages.TutorialOnCutTool);
      this.InputManager.RegisterGlobalShortcut((Func<ShortcutsManager, KeyBindings>) (map => map.ToggleCutTool), (IUnityInputController) this);
    }

    protected override bool Matches(IStaticEntity entity, bool isAreaSelection, bool isLeftCLick)
    {
      if (entity is ILayoutEntity)
      {
        if (!isAreaSelection && entity is MiniZipper)
          return false;
      }
      else if (entity is TransportPillar)
        return false;
      bool flag;
      switch (entity.ConstructionState)
      {
        case ConstructionState.NotInitialized:
        case ConstructionState.NotStarted:
        case ConstructionState.InConstruction:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    }

    public override void Activate()
    {
      this.SetInstaActionDisabled(this.ShortcutsManager.IsDown(this.ShortcutsManager.ToggleCutTool));
      base.Activate();
    }

    protected override void OnEntitiesSelected(
      IIndexable<IStaticEntity> selectedEntities,
      IIndexable<SubTransport> hoveredPartialTransports,
      bool isAreaSelection,
      bool isLeftClick,
      RectangleTerrainArea2i? area)
    {
      this.Toolbox.Hide();
      if (this.m_performCut)
      {
        Log.Warning("Cut is already being performed.");
      }
      else
      {
        this.m_surfacesToCut.ClearSkipZeroingMemory();
        if (area.HasValue)
        {
          foreach (Tile2iAndIndex enumerateTilesAndIndex in area.Value.EnumerateTilesAndIndices(this.m_terrainManager))
          {
            TileSurfaceData surfaceData = this.m_terrainManager.TileSurfacesData[enumerateTilesAndIndex.IndexRaw];
            if (!surfaceData.SurfaceSlimId.IsPhantom && !surfaceData.DecalSlimId.IsPhantom)
              this.m_surfacesToCut.Add(new TileSurfaceCopyPasteData(surfaceData, (Tile2i) enumerateTilesAndIndex.TileCoordSlim));
          }
        }
        this.m_entitiesToCut.Clear();
        this.m_entitiesToCut.AddRange(selectedEntities);
        this.m_partialTransportsToCut.Clear();
        this.m_partialTransportsToCut.AddRange(hoveredPartialTransports);
        this.m_performCut = this.m_entitiesToCut.IsNotEmpty || this.m_partialTransportsToCut.IsNotEmpty || this.m_surfacesToCut.IsNotEmpty;
      }
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.ShortcutsManager.IsPrimaryActionOn);
      return !this.m_performCut && base.InputUpdate(inputScheduler);
    }

    private void onSync(GameTime time)
    {
      if (!this.IsActive || !this.m_performCut)
        return;
      this.m_performCut = false;
      this.m_configsMap.Clear();
      this.m_cutConfigs.Clear();
      foreach (IStaticEntity staticEntity in this.m_entitiesToCut)
      {
        EntityConfigData configFrom = this.ConfigCloneHelper.CreateConfigFrom((IEntity) staticEntity);
        this.m_configsMap.Add(staticEntity, configFrom);
      }
      foreach (IStaticEntity staticEntity in this.m_entitiesToCut)
      {
        if (this.m_entitiesManager.TryCutAndDestroy(staticEntity).IsSuccess)
          this.m_cutConfigs.Add(this.m_configsMap[staticEntity]);
      }
      this.m_entitiesToCut.Clear();
      this.m_tileSurfaceDataTmp.ClearSkipZeroingMemory();
      foreach (TileSurfaceCopyPasteData surfaceCopyPasteData in this.m_surfacesToCut)
        this.m_tileSurfaceDataTmp.Add(surfaceCopyPasteData);
      this.InputScheduler.ScheduleInputCmd<BatchRemoveSurfaceDecalCmd>(new BatchRemoveSurfaceDecalCmd(this.m_tileSurfaceDataTmp.ToImmutableArray()));
      if (this.m_cutConfigs.IsEmpty && this.m_partialTransportsToCut.IsEmpty && this.m_tileSurfaceDataTmp.IsEmpty)
      {
        this.InvalidOpSound.Play();
      }
      else
      {
        foreach (SubTransport subTransport in this.m_partialTransportsToCut)
        {
          Mafi.Core.Factory.Transports.Transport entity1;
          Mafi.Core.Factory.Transports.Transport entity2;
          if (this.m_occupancyManager.TryGetOccupyingEntityAt<Mafi.Core.Factory.Transports.Transport>(subTransport.SubTrajectory.Pivots.First, out entity1) && (subTransport.SubTrajectory.Pivots.Length <= 1 || this.m_occupancyManager.TryGetOccupyingEntityAt<Mafi.Core.Factory.Transports.Transport>(subTransport.SubTrajectory.Pivots.Last, out entity2) && entity1 == entity2))
          {
            EntityConfigData configFrom = this.ConfigCloneHelper.CreateConfigFrom((IEntity) entity1);
            Option<TransportTrajectory> cutOutSubTrajectory;
            AssetValue cutOutProducts;
            if (this.m_transportsManager.TryCutOutTransport(entity1, subTransport.SubTrajectory.Pivots.First, subTransport.SubTrajectory.Pivots.Last, true, true, out Option<Mafi.Core.Factory.Transports.Transport> _, out cutOutSubTrajectory, out Option<Mafi.Core.Factory.Transports.Transport> _, out cutOutProducts, out LocStrFormatted _) && cutOutSubTrajectory.HasValue)
            {
              configFrom.Trajectory = (Option<TransportTrajectory>) subTransport.SubTrajectory;
              this.m_cutConfigs.Add(configFrom);
              this.m_assetTransactionManager.AddClearedProduct(cutOutProducts);
            }
          }
        }
        this.m_partialTransportsToCut.Clear();
        if (this.m_cutConfigs.IsEmpty && this.m_tileSurfaceDataTmp.IsEmpty)
        {
          this.InvalidOpSound.Play();
        }
        else
        {
          this.SelectSound.Play();
          this.EntityPlacer.Activate((object) this, this.EntityPlacedOrCanceled, this.EntityPlacedOrCanceled);
          this.EntityPlacer.SetEntitiesToClone((IIndexable<EntityConfigData>) this.m_cutConfigs, Option<IIndexable<TileSurfaceCopyPasteData>>.Create(this.m_surfacesToCut.ToImmutableArray().AsIndexable), StaticEntityMassPlacer.ApplyConfigMode.AlwaysApply);
          this.PricePopup.Hide();
          this.m_surfacesToCut.ClearSkipZeroingMemory();
          this.m_tileSurfaceDataTmp.ClearSkipZeroingMemory();
        }
      }
    }

    public override void Deactivate()
    {
      base.Deactivate();
      this.m_entitiesToCut.Clear();
      this.m_cutConfigs.Clear();
      this.m_partialTransportsToCut.Clear();
    }

    static CutPasteEntityInputController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CutPasteEntityInputController.HOVER_HIGHLIGHT = new ColorRgba(12876548);
    }

    private class CutPasteToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;

      public CutPasteToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("Cut", "Assets/Unity/UserInterface/Toolbar/Cut.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.CutTool__Tooltip);
        this.AddToToolbar();
      }
    }
  }
}
