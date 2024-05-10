// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Tools.AreaRemovalHandler
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Commands;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Input;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Trees;
using Mafi.Localization;
using Mafi.Unity.Audio;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.Terrain;
using Mafi.Unity.Trees;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Tools
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal sealed class AreaRemovalHandler : IStaticEntityRemovalHandler
  {
    private static readonly RelTile1i MAX_AREA_EDGE_SIZE;
    private readonly ShortcutsManager m_shortcutsManager;
    private readonly EntitiesManager m_entitiesManager;
    private readonly TerrainCursor m_terrainCursor;
    private readonly TerrainRectSelection m_terrainRectRenderer;
    private readonly EntityHighlighter m_entityHighlighter;
    private readonly IConstructionManager m_constructionManager;
    private readonly Lyst<IStaticEntity> m_selectedEntities;
    private readonly Lyst<TreeId> m_selectedTrees;
    private readonly Dict<IStaticEntity, bool> m_canBeDestroyed;
    private readonly Lyst<TransportTrajectory> m_partialTrajsTmp;
    private readonly Lyst<KeyValuePair<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<Tile3i, Tile3i>>>> m_selectedPartialTransports;
    private readonly TransportTrajectoryHighlighter m_transportTrajectoryHighlighter;
    private readonly ITreePlantingManager m_treePlantingManager;
    private readonly TreeRemovePlanRenderer m_treeHighlightRenderer;
    private Tile2i m_startTile;
    private Tile2i m_endTile;
    private RectangleTerrainArea2i m_area;
    private bool m_recomputeAreaContent;
    private AssetValue m_deconstructionValue;
    private readonly AudioSource m_areaStartSound;

    public int Priority => 999;

    public bool CanBeInterrupted => !this.m_terrainRectRenderer.IsShown;

    public AreaRemovalHandler(
      ShortcutsManager shortcutsManager,
      EntitiesManager entitiesManager,
      TerrainCursor terrainCursor,
      NewInstanceOf<TerrainRectSelection> terrainRectRenderer,
      NewInstanceOf<EntityHighlighter> entityHighlighter,
      IConstructionManager constructionManager,
      AudioDb audioDb,
      NewInstanceOf<TransportTrajectoryHighlighter> transportTrajectoryHighlighter,
      ITreePlantingManager treePlantingManager,
      NewInstanceOf<TreeRemovePlanRenderer> treeHighlightRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_selectedEntities = new Lyst<IStaticEntity>();
      this.m_selectedTrees = new Lyst<TreeId>();
      this.m_canBeDestroyed = new Dict<IStaticEntity, bool>();
      this.m_partialTrajsTmp = new Lyst<TransportTrajectory>();
      this.m_selectedPartialTransports = new Lyst<KeyValuePair<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<Tile3i, Tile3i>>>>();
      this.m_deconstructionValue = AssetValue.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_shortcutsManager = shortcutsManager;
      this.m_entitiesManager = entitiesManager;
      this.m_terrainCursor = terrainCursor;
      this.m_entityHighlighter = entityHighlighter.Instance;
      this.m_constructionManager = constructionManager;
      this.m_treePlantingManager = treePlantingManager;
      this.m_treeHighlightRenderer = treeHighlightRenderer.Instance;
      this.m_terrainRectRenderer = terrainRectRenderer.Instance;
      this.m_transportTrajectoryHighlighter = transportTrajectoryHighlighter.Instance;
      this.m_areaStartSound = audioDb.GetSharedAudio("Assets/Unity/UserInterface/Audio/InspectorClick.prefab", AudioChannel.UserInterface);
    }

    public bool TryHandleHover(
      Option<IStaticEntity> entityUnderCursor,
      Tile3f pickCoord,
      out AssetValue deconstructionValue,
      out LocStrFormatted errorMsg)
    {
      errorMsg = LocStrFormatted.Empty;
      if (this.m_terrainRectRenderer.IsShown)
      {
        Tile3f position;
        if (this.m_terrainCursor.TryComputeCurrentPosition(out position))
        {
          Tile2i p2 = this.m_terrainCursor.TerrainManager.ClampToTerrainLimits(position.Tile2i);
          if (p2 != this.m_endTile)
          {
            this.m_endTile = p2;
            RelTile2i relTile2i = (this.m_startTile - p2).AbsValue - new RelTile2i(AreaRemovalHandler.MAX_AREA_EDGE_SIZE, AreaRemovalHandler.MAX_AREA_EDGE_SIZE);
            if (relTile2i.X > 0)
              p2 = p2.AddX(this.m_startTile.X < p2.X ? -relTile2i.X : relTile2i.X);
            if (relTile2i.Y > 0)
              p2 = p2.AddY(this.m_startTile.Y < p2.Y ? -relTile2i.Y : relTile2i.Y);
            this.m_recomputeAreaContent = true;
            this.m_area = RectangleTerrainArea2i.FromTwoPositions(this.m_startTile, p2);
            this.m_terrainRectRenderer.SetArea(this.m_area, DeleteEntityInputController.COLOR_HIGHLIGHT.ToColor());
          }
        }
        deconstructionValue = this.m_deconstructionValue;
        return true;
      }
      deconstructionValue = AssetValue.Empty;
      return this.m_shortcutsManager.IsPrimaryActionDown;
    }

    public bool TryHandleMouseDown(Option<IStaticEntity> entityUnderCursor)
    {
      Tile3f position;
      if (!this.m_terrainCursor.TryComputeCurrentPosition(out position))
        return false;
      this.m_startTile = this.m_endTile = this.m_terrainCursor.TerrainManager.ClampToTerrainLimits(position.Tile2i);
      this.m_recomputeAreaContent = true;
      this.m_area = RectangleTerrainArea2i.FromTwoPositions(this.m_startTile, this.m_endTile);
      this.m_terrainRectRenderer.SetArea(this.m_area, DeleteEntityInputController.COLOR_HIGHLIGHT.ToColor());
      this.m_terrainRectRenderer.Show();
      this.m_areaStartSound.Play();
      return true;
    }

    public bool TryHandleMouseUp(
      Option<IStaticEntity> entityUnderCursor,
      Action<IInputCommand> scheduleCommand,
      bool useQuickRemove)
    {
      if (!this.m_terrainRectRenderer.IsShown)
        return false;
      foreach (IStaticEntity selectedEntity in this.m_selectedEntities)
        scheduleCommand((IInputCommand) new StartDeconstructionOfStaticEntityCmd(selectedEntity, EntityRemoveReason.Remove));
      foreach (TreeId selectedTree in this.m_selectedTrees)
        scheduleCommand((IInputCommand) new RemoveManualPlantTreeCmd(selectedTree));
      if (useQuickRemove)
      {
        foreach (IStaticEntity selectedEntity in this.m_selectedEntities)
          scheduleCommand((IInputCommand) new FinishBuildOfStaticEntityCmd(selectedEntity.Id, true));
      }
      foreach (KeyValuePair<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<Tile3i, Tile3i>>> partialTransport in this.m_selectedPartialTransports)
        scheduleCommand((IInputCommand) new StartDeconstructionOfTransportSubSectionsCmd(partialTransport.Key.Id, partialTransport.Value, EntityRemoveReason.Remove, useQuickRemove));
      return false;
    }

    public bool TryHandleCancel() => false;

    public void Sync()
    {
      if (!this.m_recomputeAreaContent)
        return;
      this.m_recomputeAreaContent = false;
      this.m_selectedEntities.Clear();
      this.m_selectedTrees.Clear();
      this.m_selectedPartialTransports.Clear();
      this.m_entityHighlighter.ClearAllHighlights();
      this.m_treeHighlightRenderer.ClearAllHighlights();
      this.m_transportTrajectoryHighlighter.ClearAllHighlights();
      this.m_deconstructionValue = AssetValue.Empty;
      foreach (IAreaSelectableStaticEntity selectableStaticEntity in this.m_entitiesManager.GetAllEntitiesOfType<IAreaSelectableStaticEntity>())
      {
        if (!selectableStaticEntity.IsDestroyed && !(selectableStaticEntity is TransportPillar) && selectableStaticEntity.IsSelected(this.m_area))
        {
          bool isSuccess;
          if (!this.m_canBeDestroyed.TryGetValue((IStaticEntity) selectableStaticEntity, out isSuccess))
          {
            isSuccess = this.m_entitiesManager.CanRemoveEntity((IEntity) selectableStaticEntity, EntityRemoveReason.Remove).IsSuccess;
            this.m_canBeDestroyed.Add((IStaticEntity) selectableStaticEntity, isSuccess);
          }
          if (isSuccess)
          {
            if (selectableStaticEntity is Mafi.Core.Factory.Transports.Transport key && !this.m_shortcutsManager.IsOn(this.m_shortcutsManager.DeleteEntireTransport))
            {
              this.m_partialTrajsTmp.Clear();
              bool entireTrajectoryIsInArea;
              key.Trajectory.GetSubTrajectoriesInArea(this.m_area, this.m_partialTrajsTmp, out entireTrajectoryIsInArea);
              if (entireTrajectoryIsInArea)
              {
                addEntity((IStaticEntity) selectableStaticEntity);
                Assert.That<Lyst<TransportTrajectory>>(this.m_partialTrajsTmp).IsEmpty<TransportTrajectory>();
              }
              else
              {
                foreach (TransportTrajectory trajectory in this.m_partialTrajsTmp)
                {
                  this.m_deconstructionValue += trajectory.Price;
                  this.m_transportTrajectoryHighlighter.HighlightTrajectory(trajectory, DeleteEntityInputController.COLOR_HIGHLIGHT);
                }
                this.m_selectedPartialTransports.Add<Mafi.Core.Factory.Transports.Transport, ImmutableArray<Pair<Tile3i, Tile3i>>>(key, this.m_partialTrajsTmp.ToImmutableArray<Pair<Tile3i, Tile3i>>((Func<TransportTrajectory, Pair<Tile3i, Tile3i>>) (x => Pair.Create<Tile3i, Tile3i>(x.Pivots.First, x.Pivots.Last))));
              }
            }
            else
              addEntity((IStaticEntity) selectableStaticEntity);
          }
        }
      }
      foreach (TreeId tree in this.m_treePlantingManager.EnumerateManualTreesInArea(this.m_area))
      {
        this.m_selectedTrees.Add(tree);
        this.m_treeHighlightRenderer.AddHighlight(tree, DeleteEntityInputController.COLOR_HIGHLIGHT);
      }

      void addEntity(IStaticEntity e)
      {
        this.m_selectedEntities.Add(e);
        this.m_entityHighlighter.Highlight((IRenderedEntity) e, DeleteEntityInputController.COLOR_HIGHLIGHT);
        this.m_deconstructionValue += this.m_constructionManager.GetDeconstructionValueFor(e);
      }
    }

    public void Deactivate()
    {
      this.m_terrainRectRenderer.Hide();
      this.m_selectedEntities.Clear();
      this.m_entityHighlighter.ClearAllHighlights();
      this.m_deconstructionValue = AssetValue.Empty;
      this.m_canBeDestroyed.Clear();
      this.m_partialTrajsTmp.Clear();
      this.m_selectedPartialTransports.Clear();
      this.m_transportTrajectoryHighlighter.ClearAllHighlights();
    }

    static AreaRemovalHandler()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      AreaRemovalHandler.MAX_AREA_EDGE_SIZE = new RelTile1i(200);
    }
  }
}
