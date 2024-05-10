// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Previews.ManualDetailPreviewController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.PostProcessors;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Unity.InputControl;
using Mafi.Unity.Terrain;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Previews
{
  public class ManualDetailPreviewController : IUnityInputController, IRegisterInMapEditor
  {
    private static readonly ColorRgba COLOR_REMOVE;
    private static readonly ColorRgba COLOR_ALL_PLACED;
    private readonly IUnityInputMgr m_inputManager;
    private readonly TreesManager m_treesManager;
    private readonly TerrainPropsManager m_terrainPropsManager;
    private readonly TreeRenderer m_treeRenderer;
    private readonly TerrainPropsRenderer m_terrainPropsRenderer;
    private readonly TerrainManager m_terrainManager;
    private readonly DependencyResolver m_resolver;
    private readonly ObjectHighlighter m_objectHighlighter;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly CursorPickingManager m_cursorPickingManager;
    private readonly TerrainCursor m_terrainCursor;
    private readonly Lyst<TreeId> m_placedTrees;
    private LystStruct<TreeId> m_treesWithoutColliders;
    private LystStruct<TreeDataBase> m_treesAddedOnMain;
    private LystStruct<TreeId> m_treesRemovedOnMain;
    private TreeDataBase? m_previewTree;
    private Option<TreeMb> m_previewTreeMb;
    private readonly Dict<TreeId, GameObject> m_treeIdToColliderObject;
    private readonly Dict<GameObject, TreeId> m_colliderObjectToTreeId;
    private readonly Lyst<TerrainPropId> m_placedProps;
    private LystStruct<TerrainPropId> m_propsWithoutColliders;
    private LystStruct<TerrainPropData> m_propsAddedOnMain;
    private LystStruct<TerrainPropId> m_propsRemovedOnMain;
    private TerrainPropData? m_previewProp;
    private Option<TerrainPropMb> m_previewPropMb;
    private readonly Dict<GameObject, TerrainPropData> m_gameObjectToPropData;
    private readonly Dict<TerrainPropId, GameObject> m_propIdToGameObject;
    private Option<GameObject> m_highlightedTreeForRemove;
    private TerrainPropData? m_highlightedPropForRemove;
    private readonly Dict<TerrainPropData, Pair<ushort, ColorRgba>> m_highlightedProps;
    private bool m_highlightingAllPlaced;
    private readonly IRandom m_random;
    private readonly Option<CustomTreesPostProcessor> m_customTreesPostProcessor;
    private readonly Option<CustomPropsPostProcessor> m_customPropsPostProcessor;
    private readonly Lyst<TerrainPropId> m_tmpProps;

    public ControllerConfig Config => ControllerConfig.Tool;

    public bool IsEnabled { get; private set; }

    public Option<Proto> ProtoBeingPlaced
    {
      get
      {
        if (!this.IsEnabled)
          return (Option<Proto>) Option.None;
        if (this.m_previewTree.HasValue)
          return (Option<Proto>) (Proto) this.m_previewTree.Value.Proto;
        return this.m_previewProp.HasValue ? (Option<Proto>) (Proto) this.m_previewProp.Value.Proto : (Option<Proto>) Option.None;
      }
    }

    public bool IsPlacing
    {
      get
      {
        if (!this.IsEnabled)
          return false;
        return this.m_previewTree.HasValue || this.m_previewProp.HasValue;
      }
    }

    public bool IsRemoving => this.IsEnabled && !this.IsPlacing;

    public ManualDetailPreviewController(
      IUnityInputMgr inputManager,
      IWorldRegionMap worldRegionMap,
      NewInstanceOf<TerrainCursor> terrainCursor,
      TreesManager treesManager,
      TerrainPropsManager terrainPropsManager,
      TreeRenderer treeRenderer,
      TerrainPropsRenderer terrainPropsRenderer,
      TerrainManager terrainManager,
      DependencyResolver resolver,
      ObjectHighlighter objectHighlighter,
      RandomProvider randomProvider,
      IGameLoopEvents gameLoopEvents,
      CursorPickingManager cursorPickingManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_placedTrees = new Lyst<TreeId>();
      this.m_treeIdToColliderObject = new Dict<TreeId, GameObject>();
      this.m_colliderObjectToTreeId = new Dict<GameObject, TreeId>();
      this.m_placedProps = new Lyst<TerrainPropId>();
      this.m_gameObjectToPropData = new Dict<GameObject, TerrainPropData>();
      this.m_propIdToGameObject = new Dict<TerrainPropId, GameObject>();
      this.m_highlightedTreeForRemove = Option<GameObject>.None;
      this.m_highlightedProps = new Dict<TerrainPropData, Pair<ushort, ColorRgba>>();
      this.m_tmpProps = new Lyst<TerrainPropId>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_inputManager = inputManager;
      this.m_treesManager = treesManager;
      this.m_terrainPropsManager = terrainPropsManager;
      this.m_treeRenderer = treeRenderer;
      this.m_terrainPropsRenderer = terrainPropsRenderer;
      this.m_terrainManager = terrainManager;
      this.m_resolver = resolver;
      this.m_objectHighlighter = objectHighlighter;
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_cursorPickingManager = cursorPickingManager;
      this.m_terrainCursor = terrainCursor.Instance;
      this.m_random = randomProvider.GetNonSimRandomFor((object) this);
      WorldRegionMap worldRegionMap1;
      if (worldRegionMap is WorldRegionMap worldRegionMap2)
      {
        worldRegionMap1 = worldRegionMap2;
      }
      else
      {
        worldRegionMap1 = new WorldRegionMap(worldRegionMap.Size, worldRegionMap.BedrockMaterial);
        Log.Error("TODO: Convert map to WorldRegionMap");
      }
      foreach (ITerrainPostProcessorV2 terrainPostProcessor in worldRegionMap1.TerrainPostProcessors)
      {
        if (terrainPostProcessor is CustomTreesPostProcessor treesPostProcessor)
        {
          this.m_customTreesPostProcessor = (Option<CustomTreesPostProcessor>) treesPostProcessor;
          break;
        }
      }
      foreach (ITerrainPostProcessorV2 terrainPostProcessor in worldRegionMap1.TerrainPostProcessors)
      {
        if (terrainPostProcessor is CustomPropsPostProcessor propsPostProcessor)
        {
          this.m_customPropsPostProcessor = (Option<CustomPropsPostProcessor>) propsPostProcessor;
          break;
        }
      }
    }

    public void Activate()
    {
      if (this.IsEnabled)
        return;
      this.IsEnabled = true;
      this.m_terrainCursor.Activate();
      this.m_gameLoopEvents.SyncUpdate.AddNonSaveable<ManualDetailPreviewController>(this, new Action<GameTime>(this.syncUpdate));
      foreach (GameObject key in this.m_colliderObjectToTreeId.Keys)
        key.SetActive(true);
      foreach (GameObject key in this.m_gameObjectToPropData.Keys)
        key.SetActive(true);
    }

    public void Deactivate()
    {
      if (!this.IsEnabled)
        return;
      this.IsEnabled = false;
      this.clear();
      this.m_terrainCursor.Deactivate();
      foreach (GameObject key in this.m_colliderObjectToTreeId.Keys)
        key.SetActive(false);
      foreach (GameObject key in this.m_gameObjectToPropData.Keys)
        key.SetActive(false);
    }

    public void DeactivateIfCan()
    {
      if (!this.IsEnabled)
        return;
      this.m_inputManager.DeactivateController((IUnityInputController) this);
    }

    public bool InputUpdate(IInputScheduler inputScheduler)
    {
      if (EventSystem.current.IsPointerOverGameObject())
        return false;
      GameObject valueOrNull = this.m_cursorPickingManager.PickedGameObject.ValueOrNull;
      if (this.IsPlacing)
      {
        if (this.m_previewTree.HasValue)
          this.showOrUpdatePreviewDetail((Proto) this.m_previewTree.Value.Proto);
        else if (this.m_previewProp.HasValue)
          this.showOrUpdatePreviewDetail((Proto) this.m_previewProp.Value.Proto);
      }
      else if ((UnityEngine.Object) valueOrNull != (UnityEngine.Object) null)
        this.MouseOverForRemove(valueOrNull);
      else
        this.ClearRemovalHighlight();
      if (!UnityEngine.Input.GetMouseButtonUp(0))
        return false;
      if ((UnityEngine.Object) valueOrNull != (UnityEngine.Object) null)
      {
        if (this.IsPlacing)
          this.PlaceDetail();
        else
          this.TryRemoveDetail(valueOrNull);
      }
      else if (this.IsPlacing)
        this.PlaceDetail();
      return true;
    }

    public void StartRemoval()
    {
      this.clear();
      this.highlightAllPlaced();
      this.activateSelfIfNeeded();
    }

    private void clear()
    {
      this.ClearPreviewDetail();
      this.ClearRemovalHighlight();
      this.ClearHighlightAllPlaced();
    }

    private void activateSelfIfNeeded()
    {
      if (this.IsEnabled)
        return;
      this.m_inputManager.ActivateNewController((IUnityInputController) this);
    }

    private void highlightAllPlacedTrees()
    {
      foreach (TreeId placedTree in this.m_placedTrees)
      {
        TreeMb treeMb;
        if (this.m_treeRenderer.TryGetTreeMb(placedTree, out treeMb) && !(this.m_highlightedTreeForRemove == treeMb.Lod0GameObject))
          this.m_objectHighlighter.Highlight(treeMb.Lod0GameObject, ManualDetailPreviewController.COLOR_ALL_PLACED);
      }
    }

    private void highlightAllPlacedProps()
    {
      foreach (TerrainPropId placedProp in this.m_placedProps)
      {
        TerrainPropData terrainPropData;
        if (!this.m_terrainPropsManager.TerrainProps.TryGetValue(placedProp, out terrainPropData))
          Log.Error(string.Format("Failed to find prop for {0}", (object) placedProp));
        else if (!this.m_highlightedProps.ContainsKey(terrainPropData))
          this.addPropHighlight(terrainPropData, ManualDetailPreviewController.COLOR_ALL_PLACED);
      }
    }

    private void highlightAllPlaced()
    {
      if (this.m_highlightingAllPlaced)
        return;
      this.highlightAllPlacedTrees();
      this.highlightAllPlacedProps();
      this.m_highlightingAllPlaced = true;
    }

    private void clearHighlightAllPlacedTrees()
    {
      foreach (TreeId placedTree in this.m_placedTrees)
      {
        TreeMb treeMb;
        if (this.m_treeRenderer.TryGetTreeMb(placedTree, out treeMb))
          this.m_objectHighlighter.RemoveHighlight(treeMb.Lod0GameObject, ManualDetailPreviewController.COLOR_ALL_PLACED);
      }
    }

    private void clearHighlightAllPlacedProps()
    {
      foreach (KeyValuePair<TerrainPropData, Pair<ushort, ColorRgba>> highlightedProp in this.m_highlightedProps)
        this.m_terrainPropsRenderer.RemoveHighlight(highlightedProp.Key, highlightedProp.Value.First);
      this.m_highlightedProps.Clear();
    }

    public void ClearHighlightAllPlaced()
    {
      if (!this.m_highlightingAllPlaced)
        return;
      this.clearHighlightAllPlacedTrees();
      this.clearHighlightAllPlacedProps();
      this.m_highlightingAllPlaced = false;
    }

    private void addPropHighlight(TerrainPropData propData, ColorRgba color)
    {
      ushort first = this.m_terrainPropsRenderer.AddHighlight(propData, color);
      this.m_highlightedProps.Add(propData, new Pair<ushort, ColorRgba>(first, color));
    }

    private void removePropHighlight(TerrainPropData propData)
    {
      Pair<ushort, ColorRgba> pair;
      if (this.m_highlightedProps.TryGetValue(propData, out pair))
      {
        this.m_terrainPropsRenderer.RemoveHighlight(propData, pair.First);
        this.m_highlightedProps.Remove(propData);
      }
      else
        Log.Warning("Failed to find instance ID for highlighted prop");
    }

    public void ClearRemovalHighlight()
    {
      if (this.m_highlightedTreeForRemove.HasValue)
      {
        if (!this.m_highlightedTreeForRemove.Value.IsNullOrDestroyed())
        {
          this.m_objectHighlighter.RemoveHighlight(this.m_highlightedTreeForRemove.Value, ManualDetailPreviewController.COLOR_REMOVE);
          if (this.m_highlightingAllPlaced)
            this.m_objectHighlighter.Highlight(this.m_highlightedTreeForRemove.Value, ManualDetailPreviewController.COLOR_ALL_PLACED);
        }
        this.m_highlightedTreeForRemove = Option<GameObject>.None;
      }
      if (!this.m_highlightedPropForRemove.HasValue)
        return;
      this.removePropHighlight(this.m_highlightedPropForRemove.Value);
      if (this.m_highlightingAllPlaced)
        this.addPropHighlight(this.m_highlightedPropForRemove.Value, ManualDetailPreviewController.COLOR_ALL_PLACED);
      this.m_highlightedPropForRemove = new TerrainPropData?();
    }

    public void ClearPreviewDetail()
    {
      if (this.m_previewTree.HasValue)
      {
        Assert.That<Option<TreeMb>>(this.m_previewTreeMb).HasValue<TreeMb>();
        TreeMb valueOrNull = this.m_previewTreeMb.ValueOrNull;
        if (valueOrNull != null)
          valueOrNull.gameObject.Destroy();
        this.m_previewTreeMb = Option<TreeMb>.None;
      }
      this.m_previewTree = new TreeDataBase?();
      if (this.m_previewProp.HasValue)
      {
        Assert.That<Option<TerrainPropMb>>(this.m_previewPropMb).HasValue<TerrainPropMb>();
        TerrainPropMb valueOrNull = this.m_previewPropMb.ValueOrNull;
        if (valueOrNull != null)
          valueOrNull.gameObject.Destroy();
        this.m_previewPropMb = Option<TerrainPropMb>.None;
      }
      this.m_previewProp = new TerrainPropData?();
    }

    public void ShowPreviewDetail(Proto detail)
    {
      this.clear();
      this.activateSelfIfNeeded();
      this.showOrUpdatePreviewDetail(detail);
    }

    private void showOrUpdatePreviewDetail(Proto detail)
    {
      switch (detail)
      {
        case TreeProto proto2:
          if (this.m_previewPropMb.HasValue)
          {
            this.m_previewPropMb.Value.gameObject.Destroy();
            this.m_previewPropMb = Option<TerrainPropMb>.None;
          }
          this.m_previewProp = new TerrainPropData?();
          bool flag1 = this.m_previewTree.HasValue && (Proto) this.m_previewTree.Value.Proto == (Proto) proto2;
          this.m_previewTree = new TreeDataBase?(new TreeDataBase(proto2, this.m_terrainCursor.Tile2i.CenterTile2f, AngleSlim.Zero, Percent.Hundred));
          if (flag1)
          {
            this.m_previewTreeMb.Value.gameObject.transform.position = this.m_terrainCursor.Tile3f.ToVector3();
            break;
          }
          if (this.m_previewTreeMb.HasValue)
            this.m_previewTreeMb.Value.gameObject.Destroy();
          this.m_previewTreeMb = (Option<TreeMb>) this.m_resolver.InvokeFactoryHierarchy<TreeMb>((object) new TreeData(this.m_previewTree.Value, this.m_terrainCursor.Tile3f.Height, TreesManager.GENERATED_TREE_PLANTED_AT_TICK, true));
          break;
        case TerrainPropProto terrainPropProto:
          if (this.m_previewTreeMb.HasValue)
          {
            this.m_previewTreeMb.Value.gameObject.Destroy();
            this.m_previewTreeMb = Option<TreeMb>.None;
          }
          this.m_previewTree = new TreeDataBase?();
          bool flag2 = this.m_previewProp.HasValue && (Proto) this.m_previewProp.Value.Proto == (Proto) terrainPropProto;
          TerrainPropProto proto1 = terrainPropProto;
          TerrainPropData.PropVariant variant = terrainPropProto.Variants[this.m_random.NextInt(terrainPropProto.Variants.Length)];
          Tile2f tile2f = this.m_terrainCursor.Tile2f;
          HeightTilesF height = this.m_terrainCursor.Tile3f.Height;
          Percent baseScale = terrainPropProto.BaseScale;
          ThicknessTilesF relativeHeightTilesF = this.m_terrainCursor.RelativeHeightTilesF;
          AngleSlim rotationYaw = terrainPropProto.AllowYawRandomization ? this.m_random.NextAngleSlim() : AngleSlim.Zero;
          AngleSlim rotationPitch = terrainPropProto.AllowPitchRandomization ? this.m_random.NextAngleSlim() : AngleSlim.Zero;
          AngleSlim rotationRoll = terrainPropProto.AllowRollRandomization ? this.m_random.NextAngleSlim() : AngleSlim.Zero;
          ThicknessTilesF placementHeightOffset = relativeHeightTilesF;
          this.m_previewProp = new TerrainPropData?(new TerrainPropData(proto1, variant, tile2f, height, baseScale, rotationYaw, rotationPitch, rotationRoll, placementHeightOffset));
          if (flag2)
          {
            this.m_previewPropMb.Value.gameObject.transform.position = this.m_terrainCursor.Tile3f.ToVector3();
            break;
          }
          if (this.m_previewPropMb.HasValue)
            this.m_previewPropMb.Value.gameObject.Destroy();
          this.m_previewPropMb = (Option<TerrainPropMb>) this.m_resolver.InvokeFactoryHierarchy<TerrainPropMb>((object) this.m_previewProp);
          break;
      }
    }

    private bool tryRemoveTree(GameObject pickedGo)
    {
      TreeId treeId;
      if (!this.m_colliderObjectToTreeId.TryGetValue(pickedGo, out treeId))
        return false;
      this.m_treesRemovedOnMain.Add(treeId);
      GameObject go;
      if (this.m_treeIdToColliderObject.TryGetValue(treeId, out go))
      {
        go.Destroy();
        this.m_treeIdToColliderObject.Remove(treeId);
      }
      else
        Log.Error(string.Format("No collider found for removed tree {0}", (object) treeId));
      this.m_colliderObjectToTreeId.RemoveAndAssert(pickedGo);
      if (this.m_customTreesPostProcessor.HasValue)
        this.m_customTreesPostProcessor.Value.RemoveManualTree(treeId);
      if (this.m_highlightedTreeForRemove == pickedGo)
        this.m_highlightedTreeForRemove = Option<GameObject>.None;
      return true;
    }

    private bool tryRemoveProp(GameObject pickedGo)
    {
      TerrainPropData terrainPropData;
      if (!this.m_gameObjectToPropData.TryGetValue(pickedGo, out terrainPropData))
        return false;
      this.m_propsRemovedOnMain.Add(terrainPropData.Id);
      GameObject go;
      if (this.m_propIdToGameObject.TryGetValue(terrainPropData.Id, out go))
        go.Destroy();
      else
        Log.Warning("Failed to find game objects to remove from dict");
      this.m_propIdToGameObject.RemoveAndAssert(terrainPropData.Id);
      this.m_gameObjectToPropData.RemoveAndAssert(pickedGo);
      if (this.m_customPropsPostProcessor.HasValue)
        this.m_customPropsPostProcessor.Value.RemoveProp(terrainPropData.Id);
      Pair<ushort, ColorRgba> pair;
      if (this.m_highlightedProps.TryGetValue(terrainPropData, out pair))
      {
        this.m_terrainPropsRenderer.RemoveHighlight(terrainPropData, pair.First);
        this.m_highlightedProps.Remove(terrainPropData);
      }
      if (this.m_highlightedPropForRemove.HasValue && this.m_highlightedPropForRemove.Value.Id == terrainPropData.Id)
        this.m_highlightedPropForRemove = new TerrainPropData?();
      return true;
    }

    public bool TryRemoveDetail(GameObject pickedGo)
    {
      return this.tryRemoveTree(pickedGo) || this.tryRemoveProp(pickedGo);
    }

    private bool tryHighlightTree(GameObject pickedGo)
    {
      TreeId treeId;
      TreeMb treeMb;
      if (!this.m_colliderObjectToTreeId.TryGetValue(pickedGo, out treeId) || !this.m_treeRenderer.TryGetTreeMb(treeId, out treeMb))
        return false;
      if (this.m_highlightedTreeForRemove == treeMb.Lod0GameObject)
        return true;
      if (this.m_highlightingAllPlaced)
        this.m_objectHighlighter.RemoveHighlight(treeMb.Lod0GameObject, ManualDetailPreviewController.COLOR_ALL_PLACED);
      this.ClearRemovalHighlight();
      this.m_objectHighlighter.Highlight(treeMb.Lod0GameObject, ManualDetailPreviewController.COLOR_REMOVE);
      this.m_highlightedTreeForRemove = (Option<GameObject>) treeMb.Lod0GameObject;
      return true;
    }

    private bool tryHighlightProp(GameObject pickedGo)
    {
      TerrainPropData propData;
      if (!this.m_gameObjectToPropData.TryGetValue(pickedGo, out propData))
        return false;
      if (this.m_highlightedPropForRemove.HasValue && this.m_highlightedPropForRemove.Value.Id == propData.Id)
        return true;
      if (this.m_highlightingAllPlaced)
        this.removePropHighlight(propData);
      this.ClearRemovalHighlight();
      this.addPropHighlight(propData, ManualDetailPreviewController.COLOR_REMOVE);
      this.m_highlightedPropForRemove = new TerrainPropData?(propData);
      return true;
    }

    public void MouseOverForRemove(GameObject pickedGo)
    {
      if (this.m_highlightedTreeForRemove == pickedGo || this.tryHighlightTree(pickedGo))
        return;
      this.tryHighlightProp(pickedGo);
    }

    public void PlaceDetail()
    {
      if (this.m_previewTree.HasValue && this.m_customTreesPostProcessor.HasValue)
        this.m_treesAddedOnMain.Add(this.m_previewTree.Value);
      else if (this.m_previewProp.HasValue && this.m_customPropsPostProcessor.HasValue)
        this.m_propsAddedOnMain.Add(this.m_customPropsPostProcessor.Value.GetPropForTerrain(this.m_previewProp.Value, this.m_terrainManager));
      else
        Log.Error("Trying to place invalid object");
    }

    private void syncUpdateTrees()
    {
      foreach (TreeDataBase treeData in this.m_treesAddedOnMain)
      {
        TreeId treeId = new TreeId(treeData.Position.Tile2i.AsSlim);
        if (this.m_treesManager.TryRemoveGeneratedTree(treeId))
        {
          GameObject gameObject;
          if (this.m_treeIdToColliderObject.TryGetValue(treeId, out gameObject))
          {
            this.m_colliderObjectToTreeId.RemoveAndAssert(gameObject);
            this.m_treeIdToColliderObject.RemoveAndAssert(treeId);
            this.m_objectHighlighter.RemoveAllHighlights(gameObject);
            if (this.m_highlightedTreeForRemove == gameObject)
              this.m_highlightedTreeForRemove = Option<GameObject>.None;
            gameObject.Destroy();
          }
          this.m_customTreesPostProcessor.ValueOrNull?.RemoveManualTree(treeId);
        }
        else
          this.m_placedTrees.Add(treeId);
        this.m_customTreesPostProcessor.ValueOrNull?.AddManualTree(treeData);
        this.m_treesManager.TryAddGeneratedTree(treeData);
        this.m_treesWithoutColliders.Add(treeId);
      }
      this.m_treesAddedOnMain.Clear();
      foreach (TreeId treeId in this.m_treesRemovedOnMain)
      {
        this.m_treesManager.TryRemoveGeneratedTree(treeId);
        if (!this.m_placedTrees.TryRemoveReplaceLast(treeId))
          Log.Warning(string.Format("Failed to remove tree {0}", (object) treeId));
        this.m_treesWithoutColliders.TryRemoveReplaceLast(treeId);
      }
      this.m_treesRemovedOnMain.Clear();
      if (!this.m_treesWithoutColliders.IsNotEmpty)
        return;
      foreach (TreeId treeId1 in this.m_treesWithoutColliders.ToArray())
      {
        TreeMb treeMb;
        if (this.m_treeRenderer.TryGetTreeMb(treeId1, out treeMb))
        {
          GameObject key;
          if (this.m_treeIdToColliderObject.TryGetValue(treeId1, out key))
          {
            TreeId treeId2;
            if (!this.m_colliderObjectToTreeId.TryGetValue(key, out treeId2))
              Log.Error("Failed to find test tree Id");
            else
              Assert.That<TreeId>(treeId2).IsEqualTo<TreeId>(treeId1);
          }
          else
          {
            key = new GameObject("TreeCollider");
            Bounds maxBounds = treeMb.Lod0GameObject.ComputeMaxBounds();
            BoxCollider boxCollider = key.AddComponent<BoxCollider>();
            boxCollider.center = maxBounds.center;
            boxCollider.size = maxBounds.size;
            this.m_colliderObjectToTreeId.Add(key, treeId1);
            this.m_treeIdToColliderObject.Add(treeId1, key);
            if (this.m_highlightingAllPlaced)
              this.m_objectHighlighter.Highlight(treeMb.Lod0GameObject, ManualDetailPreviewController.COLOR_ALL_PLACED);
            this.m_treesWithoutColliders.Remove(treeId1);
          }
        }
      }
    }

    private void syncUpdateProps()
    {
      this.m_tmpProps.Clear();
      foreach (TerrainPropId placedProp in this.m_placedProps)
      {
        if (!this.m_terrainPropsManager.TerrainProps.ContainsKey(placedProp))
          this.m_tmpProps.Add(placedProp);
      }
      Tile2f position;
      foreach (TerrainPropData data in this.m_propsAddedOnMain)
      {
        TerrainPropId terrainPropId;
        ref TerrainPropId local = ref terrainPropId;
        position = data.Position;
        Tile2iSlim asSlim = position.Tile2i.AsSlim;
        local = new TerrainPropId(asSlim);
        this.m_terrainPropsManager.ForceAddProp(data, (Option<Lyst<TerrainPropId>>) this.m_tmpProps);
        this.m_placedProps.Add(terrainPropId);
        this.m_propsWithoutColliders.Add(terrainPropId);
      }
      foreach (TerrainPropId tmpProp in this.m_tmpProps)
      {
        GameObject gameObject;
        if (this.m_propIdToGameObject.TryGetValue(tmpProp, out gameObject))
        {
          this.m_gameObjectToPropData.RemoveAndAssert(gameObject);
          this.m_propIdToGameObject.RemoveAndAssert(tmpProp);
          if (!this.m_placedProps.TryRemoveReplaceLast(tmpProp))
            Log.Warning(string.Format("Failed to remove prop {0}", (object) tmpProp));
          if (this.m_highlightedPropForRemove.HasValue && this.m_highlightedPropForRemove.Value.Id == tmpProp)
            this.m_highlightedPropForRemove = new TerrainPropData?();
          this.m_customPropsPostProcessor.ValueOrNull?.RemoveProp(tmpProp);
          gameObject.Destroy();
        }
      }
      for (int index = this.m_propsAddedOnMain.Count - 1; index >= 0; --index)
      {
        if (this.m_placedProps.Contains(this.m_propsAddedOnMain[index].Id))
          this.m_customPropsPostProcessor.ValueOrNull?.AddProp(this.m_propsAddedOnMain[index]);
      }
      this.m_propsAddedOnMain.Clear();
      foreach (TerrainPropId terrainPropId in this.m_propsRemovedOnMain)
      {
        TerrainPropData terrainPropData;
        Pair<ushort, ColorRgba> pair;
        if (this.m_terrainPropsManager.TerrainProps.TryGetValue(terrainPropId, out terrainPropData) && this.m_highlightedProps.TryGetValue(terrainPropData, out pair))
        {
          this.m_terrainPropsRenderer.RemoveHighlight(terrainPropData, pair.First);
          this.m_highlightedProps.Remove(terrainPropData);
        }
        this.m_terrainPropsManager.TryRemoveProp(terrainPropId, true);
        if (!this.m_placedProps.TryRemoveReplaceLast(terrainPropId))
          Log.Warning(string.Format("Failed to remove prop {0}", (object) terrainPropId));
      }
      this.m_propsRemovedOnMain.Clear();
      if (!this.m_propsWithoutColliders.IsNotEmpty)
        return;
      foreach (TerrainPropId propsWithoutCollider in this.m_propsWithoutColliders)
      {
        TerrainPropData terrainPropData;
        if (this.m_terrainPropsManager.TerrainProps.TryGetValue(propsWithoutCollider, out terrainPropData))
        {
          GameObject key = new GameObject("PropCollider");
          BoxCollider boxCollider1 = key.AddComponent<BoxCollider>();
          BoxCollider boxCollider2 = boxCollider1;
          position = terrainPropData.Position;
          Vector3 vector3 = position.ExtendHeight(terrainPropData.PlacedAtHeight).ToVector3();
          boxCollider2.center = vector3;
          boxCollider1.size = (terrainPropData.Proto.Graphics.BoundingBox.Size * terrainPropData.Proto.BaseScale * terrainPropData.Scale * 2).ToVector3();
          this.m_gameObjectToPropData.AddAndAssertNew(key, terrainPropData);
          this.m_propIdToGameObject.AddAndAssertNew(terrainPropData.Id, key);
        }
      }
      this.m_propsWithoutColliders.Clear();
    }

    private void syncUpdate(GameTime time)
    {
      this.syncUpdateTrees();
      this.syncUpdateProps();
      if (this.IsEnabled)
        return;
      this.m_gameLoopEvents.SyncUpdate.RemoveNonSaveable<ManualDetailPreviewController>(this, new Action<GameTime>(this.syncUpdate));
    }

    static ManualDetailPreviewController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ManualDetailPreviewController.COLOR_REMOVE = 16724787.ToColorRgba();
      ManualDetailPreviewController.COLOR_ALL_PLACED = 9474192.ToColorRgba();
    }
  }
}
