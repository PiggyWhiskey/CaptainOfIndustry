// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.CopyEntityInputControllerBase
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.LayoutEntityPlacing;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.UserInterface;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  /// <summary>Base class for copy / cut / pasting.</summary>
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal abstract class CopyEntityInputControllerBase : 
    BaseEntityCursorInputController<IStaticEntity>
  {
    protected readonly IUnityInputMgr InputManager;
    protected readonly IInputScheduler InputScheduler;
    protected readonly EntitiesCloneConfigHelper ConfigCloneHelper;
    protected readonly FloatingPricePopup PricePopup;
    protected readonly StaticEntityMassPlacer EntityPlacer;
    protected AudioSource SelectSound;
    protected AudioSource InvalidOpSound;
    private readonly TransportBuildController m_transportBuildController;
    protected readonly Action EntityPlacedOrCanceled;
    protected readonly Mafi.Unity.InputControl.Toolbar.Toolbox Toolbox;
    protected readonly TerrainManager m_terrainManager;
    private readonly Lyst<EntityConfigData> m_copyConfigs;
    private readonly Lyst<IEntity> m_entitiesToCopy;
    protected LystStruct<TileSurfaceCopyPasteData> m_tileSurfaceDataTmp;

    public override bool AllowAreaOnlySelection => true;

    public CopyEntityInputControllerBase(
      ProtosDb protosDb,
      UiBuilder builder,
      UnlockedProtosDbForUi unlockedProtosDb,
      ShortcutsManager shortcutsManager,
      IUnityInputMgr inputManager,
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
      Mafi.Unity.InputControl.Toolbar.Toolbox toolbox,
      Proto.ID? lockByProto,
      TerrainManager terrainManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_copyConfigs = new Lyst<EntityConfigData>();
      this.m_entitiesToCopy = new Lyst<IEntity>();
      // ISSUE: explicit constructor call
      base.\u002Ector(protosDb, builder, unlockedProtosDb, shortcutsManager, inputManager, cursorPickingManager, cursorManager, areaSelectionToolFactory, (IEntitiesManager) entitiesManager, highlighter, (Option<NewInstanceOf<TransportTrajectoryHighlighter>>) transportTrajectoryHighlighter, lockByProto);
      this.InputManager = inputManager;
      this.InputScheduler = inputScheduler;
      this.ConfigCloneHelper = configCloneHelper;
      this.PricePopup = pricePopup.Instance;
      this.m_transportBuildController = transportBuildController;
      this.EntityPlacer = entityPlacer.Instance;
      this.EntityPlacedOrCanceled = new Action(this.entityPlacedOrCancelled);
      this.Toolbox = toolbox;
      this.m_terrainManager = terrainManager;
      this.SetPartialTransportsSelection(true);
      this.SelectSound = builder.AudioDb.GetSharedAudio(builder.Audio.EntitySelect);
      this.InvalidOpSound = builder.AudioDb.GetSharedAudio(builder.Audio.InvalidOp);
    }

    public override void Deactivate()
    {
      base.Deactivate();
      this.PricePopup.Hide();
      this.PricePopup.SetTemporarilyHidden(false);
      if (this.EntityPlacer.IsActive)
        this.EntityPlacer.Deactivate();
      if (this.m_transportBuildController.IsActive)
        this.m_transportBuildController.Deactivate();
      this.Toolbox.Hide();
    }

    public override void Activate()
    {
      base.Activate();
      this.Toolbox.Show();
    }

    private void entityPlacedOrCancelled()
    {
      this.InputManager.DeactivateController((IUnityInputController) this);
    }

    protected override bool OnFirstActivated(
      IStaticEntity hoveredEntity,
      Lyst<IStaticEntity> selectedEntities,
      Lyst<SubTransport> selectedPartialTransports)
    {
      selectedEntities.Add(hoveredEntity);
      return true;
    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (!this.IsActive)
      {
        Log.Error("Input update for non-active controller!");
        return false;
      }
      if (this.EntityPlacer.IsActive)
        return this.EntityPlacer.InputUpdate(inputScheduler);
      if (this.m_transportBuildController.IsActive)
      {
        bool flag = this.m_transportBuildController.InputUpdate(inputScheduler);
        if (this.m_transportBuildController.TransportProtoSelected)
          return flag;
        this.entityPlacedOrCancelled();
        return true;
      }
      if (this.ShortcutsManager.IsDown(this.ShortcutsManager.TogglePricePopup))
      {
        this.PricePopup.SetTemporarilyHidden(!this.PricePopup.IsTemporarilyHidden);
        return true;
      }
      this.PricePopup.UpdatePosition();
      return base.InputUpdate(inputScheduler);
    }

    protected override void OnHoverChanged(
      IIndexable<IStaticEntity> hoveredEntities,
      IIndexable<SubTransport> hoveredPartialTransports,
      bool isLeftClick)
    {
      if (hoveredEntities.Count + hoveredPartialTransports.Count == 0)
      {
        this.PricePopup.Hide();
      }
      else
      {
        AssetValue empty = AssetValue.Empty;
        foreach (IStaticEntity hoveredEntity in hoveredEntities)
          empty += hoveredEntity.Prototype.Costs.Price;
        foreach (SubTransport partialTransport in hoveredPartialTransports)
          empty += partialTransport.SubTrajectory.Price;
        this.PricePopup.SetBuyPrice(empty);
      }
    }

    protected override void OnEntitiesSelected(
      IIndexable<IStaticEntity> selectedEntities,
      IIndexable<SubTransport> selectedPartialTransports,
      bool isAreaSelection,
      bool isLeftClick,
      RectangleTerrainArea2i? area)
    {
      this.PricePopup.Hide();
      this.Toolbox.Hide();
      int num = selectedEntities.Count + selectedPartialTransports.Count;
      this.m_tileSurfaceDataTmp.ClearSkipZeroingMemory();
      if (area.HasValue)
      {
        foreach (Tile2iAndIndex enumerateTilesAndIndex in area.Value.EnumerateTilesAndIndices(this.m_terrainManager))
        {
          TileSurfaceData surfaceData = this.m_terrainManager.TileSurfacesData[enumerateTilesAndIndex.IndexRaw];
          if (!surfaceData.SurfaceSlimId.IsPhantom && !surfaceData.DecalSlimId.IsPhantom)
          {
            this.m_tileSurfaceDataTmp.Add(new TileSurfaceCopyPasteData(surfaceData, (Tile2i) enumerateTilesAndIndex.TileCoordSlim));
            ++num;
          }
        }
      }
      if (num == 0)
        return;
      this.SelectSound.Play();
      if (num == 1 && selectedEntities.Count == 1 && !isAreaSelection && selectedEntities[0] is Mafi.Core.Factory.Transports.Transport selectedEntity1)
      {
        this.m_transportBuildController.Activate();
        this.m_transportBuildController.SelectProto(selectedEntity1.Prototype);
      }
      else
      {
        this.EntityPlacer.Activate((object) this, this.EntityPlacedOrCanceled, this.EntityPlacedOrCanceled);
        this.m_copyConfigs.Clear();
        this.m_entitiesToCopy.Clear();
        foreach (IStaticEntity selectedEntity in selectedEntities)
        {
          if (selectedEntity is TransportPillar)
          {
            Log.Warning("Pillars are not to be copied.");
          }
          else
          {
            this.m_copyConfigs.Add(this.ConfigCloneHelper.CreateConfigFrom((IEntity) selectedEntity));
            this.m_entitiesToCopy.Add((IEntity) selectedEntity);
          }
        }
        foreach (SubTransport partialTransport in selectedPartialTransports)
        {
          EntityConfigData configFrom = this.ConfigCloneHelper.CreateConfigFrom((IEntity) partialTransport.OriginalTransport);
          configFrom.Trajectory = (Option<TransportTrajectory>) partialTransport.SubTrajectory;
          this.m_copyConfigs.Add(configFrom);
          this.m_entitiesToCopy.Add((IEntity) partialTransport.OriginalTransport);
        }
        this.EntityPlacer.SetEntitiesToClone((IIndexable<EntityConfigData>) this.m_copyConfigs, Option<IIndexable<TileSurfaceCopyPasteData>>.Create(this.m_tileSurfaceDataTmp.ToImmutableArray().AsIndexable), StaticEntityMassPlacer.ApplyConfigMode.ApplyIfNotDisabled);
        this.m_entitiesToCopy.Clear();
      }
    }
  }
}
