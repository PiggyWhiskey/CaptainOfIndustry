// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Blueprints.BlueprintCreationController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Blueprints;
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
using Mafi.Unity.InputControl.Tools;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using System;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Blueprints
{
  /// <summary>Tool for blueprints creation by selection via cursor.</summary>
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  internal class BlueprintCreationController : BaseEntityCursorInputController<IStaticEntity>
  {
    private static readonly ColorRgba HOVER_HIGHLIGHT;
    public new static readonly RelTile1i MAX_AREA_EDGE_SIZE;
    private readonly EntitiesCloneConfigHelper m_configCloneHelper;
    private readonly IUnityInputMgr m_inputManager;
    private readonly TerrainManager m_terrainManager;
    private readonly BlueprintCreationController.BlueprintToolbox m_toolbox;
    private AudioSource m_selectSound;
    private Option<Action<ImmutableArray<EntityConfigData>, Lyst<TileSurfaceCopyPasteData>>> m_onSelectionDone;
    private LystStruct<TileSurfaceCopyPasteData> m_tileSurfaceDataTmp;

    public override bool AllowAreaOnlySelection => true;

    public BlueprintCreationController(
      ProtosDb protosDb,
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      EntitiesCloneConfigHelper configCloneHelper,
      ShortcutsManager shortcutsManager,
      ToolbarController toolbarController,
      IUnityInputMgr inputManager,
      CursorPickingManager cursorPickingManager,
      CursorManager cursorManager,
      AreaSelectionToolFactory areaSelectionToolFactory,
      NewInstanceOf<EntityHighlighter> highlighter,
      NewInstanceOf<TransportTrajectoryHighlighter> transportTrajectoryHighlighter,
      EntitiesManager entitiesManager,
      TerrainManager terrainManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb, builder, unlockedProtosDb, shortcutsManager, inputManager, cursorPickingManager, cursorManager, areaSelectionToolFactory, (IEntitiesManager) entitiesManager, highlighter, (Option<NewInstanceOf<TransportTrajectoryHighlighter>>) transportTrajectoryHighlighter, new Proto.ID?());
      this.m_configCloneHelper = configCloneHelper;
      this.m_inputManager = inputManager;
      this.m_terrainManager = terrainManager;
      this.m_toolbox = new BlueprintCreationController.BlueprintToolbox(toolbarController, builder);
      this.SetPartialTransportsSelection(true);
      this.SetInstaActionDisabled(true);
      this.SetEdgeSizeLimit(BlueprintCreationController.MAX_AREA_EDGE_SIZE);
      this.m_selectSound = builder.AudioDb.GetSharedAudio(builder.Audio.EntitySelect);
      this.InitializeUi(new CursorStyle?(builder.Style.Cursors.SelectArea), builder.Audio.EntitySelect, BlueprintCreationController.HOVER_HIGHLIGHT, BlueprintCreationController.HOVER_HIGHLIGHT);
    }

    protected override void RegisterToolbar(ToolbarController controller)
    {
    }

    protected override bool Matches(IStaticEntity entity, bool isAreaSelection, bool isLeftCLick)
    {
      return isAreaSelection && BlueprintsLibrary.CanBeInBlueprint((Proto) entity.Prototype);
    }

    public override void Deactivate()
    {
      base.Deactivate();
      this.m_toolbox.Hide();
    }

    public override void Activate()
    {
      base.Activate();
      this.m_toolbox.Show();
    }

    public void ActivateForSelection(
      Action<ImmutableArray<EntityConfigData>, Lyst<TileSurfaceCopyPasteData>> onSelectionDone)
    {
      this.m_onSelectionDone = onSelectionDone.SomeOption<Action<ImmutableArray<EntityConfigData>, Lyst<TileSurfaceCopyPasteData>>>();
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.IsActive)
      {
        Log.Error("Input update for non-active controller!");
        return false;
      }
      this.m_toolbox.PrimaryActionBtn.SetIsOn(this.ShortcutsManager.IsPrimaryActionOn);
      return base.InputUpdate(inputScheduler);
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
      bool isLeftClick,
      RectangleTerrainArea2i? area)
    {
      this.m_toolbox.Hide();
      int num = selectedEntities.Count + selectedPartialTransports.Count - selectedEntities.AsEnumerable().Count<IStaticEntity>((Func<IStaticEntity, bool>) (x => x is TransportPillar));
      this.m_tileSurfaceDataTmp.ClearSkipZeroingMemory();
      if (area.HasValue)
      {
        foreach (Tile2iAndIndex enumerateTilesAndIndex in area.Value.EnumerateTilesAndIndices(this.m_terrainManager))
        {
          TileSurfaceData surfaceData = this.m_terrainManager.TileSurfacesData[enumerateTilesAndIndex.IndexRaw];
          if ((!surfaceData.IsAutoPlaced || !surfaceData.DecalSlimId.IsPhantom) && !surfaceData.IsNotValid)
          {
            this.m_tileSurfaceDataTmp.Add(new TileSurfaceCopyPasteData(surfaceData, (Tile2i) enumerateTilesAndIndex.TileCoordSlim));
            ++num;
          }
        }
      }
      if (num <= 0)
        return;
      this.m_selectSound.Play();
      Lyst<EntityConfigData> lyst = new Lyst<EntityConfigData>();
      foreach (IStaticEntity selectedEntity in selectedEntities)
        lyst.Add(this.m_configCloneHelper.CreateConfigFrom((IEntity) selectedEntity));
      foreach (SubTransport partialTransport in selectedPartialTransports)
      {
        EntityConfigData configFrom = this.m_configCloneHelper.CreateConfigFrom((IEntity) partialTransport.OriginalTransport);
        configFrom.Trajectory = (Option<TransportTrajectory>) partialTransport.SubTrajectory;
        lyst.Add(configFrom);
      }
      this.m_inputManager.DeactivateController((IUnityInputController) this);
      Action<ImmutableArray<EntityConfigData>, Lyst<TileSurfaceCopyPasteData>> valueOrNull = this.m_onSelectionDone.ValueOrNull;
      if (valueOrNull == null)
        return;
      valueOrNull(lyst.ToImmutableArray(), this.m_tileSurfaceDataTmp.ToLyst());
    }

    static BlueprintCreationController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      BlueprintCreationController.HOVER_HIGHLIGHT = new ColorRgba(16766738);
      BlueprintCreationController.MAX_AREA_EDGE_SIZE = new RelTile1i(350);
    }

    private class BlueprintToolbox : Mafi.Unity.InputControl.Toolbar.Toolbox
    {
      public ToggleBtn PrimaryActionBtn;

      public BlueprintToolbox(ToolbarController toolbar, UiBuilder builder)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(toolbar, builder);
      }

      protected override void BuildCustomItems(UiBuilder builder)
      {
        this.PrimaryActionBtn = this.AddToggleButton("SelectArea", "Assets/Unity/UserInterface/Toolbox/SelectArea.svg", (Action<bool>) (_ => { }), (Func<ShortcutsManager, KeyBindings>) (m => m.PrimaryAction), (LocStrFormatted) Tr.Blueprint_NewFromSelectionTooltip);
        this.AddToToolbar();
      }
    }
  }
}
