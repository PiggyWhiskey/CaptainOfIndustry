// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Factory.LayoutEntityPreviewManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Ports.Io;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Unity.Factory.Transports;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Terrain;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Factory
{
  /// <summary>
  /// Manages layout entity previews, manages creation, pooling, and updating.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class LayoutEntityPreviewManager : IDisposable
  {
    private static readonly int INSTANCE_DATA_SHADER_ID;
    private static readonly int HEIGHT_TEXTURE_SHADER_ID;
    private static readonly int TERRAIN_SIZE_SHADER_ID;
    internal readonly ObjectHighlighter Highlighter;
    internal readonly DependencyResolver Resolver;
    internal readonly PortPreviewManager PreviewManager;
    internal readonly EntitiesManager EntitiesManager;
    internal readonly TerrainOccupancyManager TerrainOccupancyManager;
    internal readonly AssetsDb AssetsDb;
    internal readonly TerrainManager TerrainManager;
    private readonly TerrainRenderer m_terrainRenderer;
    internal readonly LayoutEntityAddRequestFactory LayoutEntityAddRequestFactory;
    internal readonly TransportPillarsRenderer PillarsRenderer;
    internal readonly TransportsConstructionHelper TransportsConstructionHelper;
    internal readonly TransportsManager TransportsManager;
    private readonly Mesh m_tileValidationMesh;
    private readonly Material m_tileValidationMaterial;
    private readonly Material m_previewMaterialTemplateShared;
    private readonly Material m_transportPreviewMaterialTemplateShared;
    private readonly Dict<IEntityProto, Material> m_layoutEntitiesPreviewMaterialsCache;
    private readonly ObjectPool2<LayoutEntityPreview> m_previewPool;
    private readonly Lyst<LayoutEntityPreview> m_activePreviews;
    private readonly Lyst<LayoutEntityPreview> m_activePreviewsForSim;
    private readonly Lyst<LayoutEntityPreview> m_removedPreviews;
    private bool m_updateActivePreviewsForSim;
    private readonly uint[] m_drawArgs;
    private readonly ComputeBuffer m_drawArgsBuffer;
    private TileValidityInstanceData[] m_validityData;
    private ComputeBuffer m_detailsDataBuffer;
    private Bounds m_worldBounds;
    private bool m_forceValidityUpdate;
    [ThreadStatic]
    private static Lyst<PillarVisualsSpec> s_pillarsVisualsTmp;

    public LayoutEntityPreviewManager(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      DependencyResolver resolver,
      PortPreviewManager previewManager,
      ObjectHighlighter highlighter,
      EntitiesManager entitiesManager,
      TerrainOccupancyManager terrainOccupancyManager,
      AssetsDb assetsDb,
      TerrainManager terrainManager,
      TerrainRenderer terrainRenderer,
      LayoutEntityAddRequestFactory layoutEntityAddRequestFactory,
      TransportPillarsRenderer pillarsRenderer,
      TransportsConstructionHelper transportsConstructionHelper,
      TransportsManager transportsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_layoutEntitiesPreviewMaterialsCache = new Dict<IEntityProto, Material>();
      this.m_activePreviews = new Lyst<LayoutEntityPreview>(true);
      this.m_activePreviewsForSim = new Lyst<LayoutEntityPreview>(true);
      this.m_removedPreviews = new Lyst<LayoutEntityPreview>(true);
      this.m_drawArgs = new uint[5];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Resolver = resolver;
      this.PreviewManager = previewManager;
      this.Highlighter = highlighter;
      this.EntitiesManager = entitiesManager;
      this.TerrainOccupancyManager = terrainOccupancyManager;
      this.AssetsDb = assetsDb;
      this.TerrainManager = terrainManager;
      this.m_terrainRenderer = terrainRenderer;
      this.LayoutEntityAddRequestFactory = layoutEntityAddRequestFactory;
      this.PillarsRenderer = pillarsRenderer;
      this.TransportsManager = transportsManager;
      this.TransportsConstructionHelper = transportsConstructionHelper;
      this.m_previewMaterialTemplateShared = assetsDb.GetSharedMaterial("Assets/Core/Materials/BuildingBlueprint.mat");
      this.m_transportPreviewMaterialTemplateShared = assetsDb.GetSharedMaterial("Assets/Core/Materials/TransportBlueprint.mat");
      this.m_tileValidationMesh = assetsDb.GetSharedPrefabOrThrow("Assets/Core/Construction/ValidationTile.prefab").GetComponent<MeshFilter>().sharedMesh;
      this.m_tileValidationMaterial = assetsDb.GetClonedMaterial("Assets/Core/Construction/TileValidityOverlayInstanced.mat");
      this.m_previewPool = new ObjectPool2<LayoutEntityPreview>(64, (Func<ObjectPool2<LayoutEntityPreview>, LayoutEntityPreview>) (pool => new LayoutEntityPreview(this)));
      this.m_drawArgs[0] = this.m_tileValidationMesh.GetIndexCount(0);
      this.m_drawArgs[2] = this.m_tileValidationMesh.GetIndexStart(0);
      this.m_drawArgs[3] = this.m_tileValidationMesh.GetBaseVertex(0);
      this.m_drawArgsBuffer = new ComputeBuffer(this.m_drawArgs.Length, 4, ComputeBufferType.DrawIndirect);
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      gameLoopEvents.RenderUpdate.AddNonSaveable<LayoutEntityPreviewManager>(this, new Action<GameTime>(this.renderUpdate));
      simLoopEvents.UpdateEndForUi.AddNonSaveable<LayoutEntityPreviewManager>(this, new Action(this.simUpdate));
      simLoopEvents.Sync.AddNonSaveable<LayoutEntityPreviewManager>(this, new Action(this.syncUpdate));
    }

    private void initState()
    {
      this.m_worldBounds = new Bounds(new Vector3((float) (this.TerrainManager.TerrainWidth * 2) / 2f, 0.0f, (float) (this.TerrainManager.TerrainHeight * 2) / 2f), new Vector3((float) (this.TerrainManager.TerrainWidth * 2), 10000f, (float) (this.TerrainManager.TerrainHeight * 2)));
    }

    public void Dispose()
    {
      this.m_drawArgsBuffer.Dispose();
      this.m_detailsDataBuffer?.Dispose();
      foreach (Material material in this.m_layoutEntitiesPreviewMaterialsCache.Values)
        material.DestroyIfNotNull();
      this.m_layoutEntitiesPreviewMaterialsCache.Clear();
    }

    public void SetCachedPreviewMaterialFor(IEntityProto entityProto, GameObject entityGo)
    {
      Material material;
      if (!this.m_layoutEntitiesPreviewMaterialsCache.TryGetValue(entityProto, out material))
      {
        MeshRenderer componentInChildren = entityGo.GetComponentInChildren<MeshRenderer>();
        if (!(bool) (UnityEngine.Object) componentInChildren)
        {
          Log.Error(string.Format("Failed to get mesh renderer on entity GO '{0}'.", (object) entityGo));
          return;
        }
        material = !(entityProto is TransportProto) ? InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_previewMaterialTemplateShared, componentInChildren.sharedMaterial, true) : InstancingUtils.InstantiateMaterialAndCopyTextures(this.m_transportPreviewMaterialTemplateShared, componentInChildren.sharedMaterial, true);
        this.m_layoutEntitiesPreviewMaterialsCache[entityProto] = material;
      }
      entityGo.gameObject.SetSharedMaterialRecursively(material);
    }

    public LayoutEntityPreview CreatePreview(
      ILayoutEntityProto proto,
      EntityPlacementPhase placementPhase,
      TileTransform transform,
      bool disableValidation = false,
      bool disablePortPreviews = false,
      bool enableMiniZipperPlacement = false,
      Predicate<IoPortTemplate> disablePortPreviewsPredicate = null)
    {
      LayoutEntityPreview instance = this.m_previewPool.GetInstance();
      Assert.That<bool>(instance.IsDestroyed).IsTrue();
      instance.Initialize(proto, transform, placementPhase, disableValidation, disablePortPreviews, !enableMiniZipperPlacement, disablePortPreviewsPredicate);
      this.m_activePreviews.Add(instance);
      this.m_updateActivePreviewsForSim = true;
      return instance;
    }

    public ImmutableArray<PillarVisualsSpec> CreatePillarsVisualSpec(
      LayoutEntityAddRequest addRequest)
    {
      Tile3i position1 = addRequest.Transform.Position;
      if (LayoutEntityPreviewManager.s_pillarsVisualsTmp == null)
        LayoutEntityPreviewManager.s_pillarsVisualsTmp = new Lyst<PillarVisualsSpec>();
      LayoutEntityPreviewManager.s_pillarsVisualsTmp.Clear();
      Lyst<PillarVisualsSpec> pillarsVisualsTmp = LayoutEntityPreviewManager.s_pillarsVisualsTmp;
      int index = 0;
      while (true)
      {
        int num = index;
        ReadOnlyArray<OccupiedTileRelative> occupiedTiles = addRequest.OccupiedTiles;
        int length = occupiedTiles.Length;
        if (num < length)
        {
          occupiedTiles = addRequest.OccupiedTiles;
          OccupiedTileRelative occupiedTileRelative = occupiedTiles[index];
          if (occupiedTileRelative.Constraint.HasAnyConstraints(LayoutTileConstraint.UsingPillar))
          {
            Tile2i xy = position1.Xy;
            occupiedTiles = addRequest.OccupiedTiles;
            occupiedTileRelative = occupiedTiles[index];
            RelTile2i relCoord = occupiedTileRelative.RelCoord;
            Tile2i position2 = xy + relCoord;
            HeightTilesI height = position1.Height;
            occupiedTiles = addRequest.OccupiedTiles;
            occupiedTileRelative = occupiedTiles[index];
            ThicknessTilesI fromHeightRel = occupiedTileRelative.FromHeightRel;
            HeightTilesI topTileHeight = height + fromHeightRel;
            TransportPillar existingPillar;
            ThicknessTilesI newHeight;
            if (this.TransportsManager.CanExtendPillarAt(position2, topTileHeight, out existingPillar, out newHeight))
            {
              PillarVisualsSpec pillarVisuals = this.TransportsConstructionHelper.ComputePillarVisuals(existingPillar.CenterTile, newHeight, makeNotConstructed: true);
              pillarsVisualsTmp.Add(pillarVisuals);
            }
            else
            {
              HeightTilesI baseHeight;
              if (this.TransportsManager.CanBuildPillarAt(position2, topTileHeight, out baseHeight, out newHeight) && newHeight > ThicknessTilesI.One)
              {
                PillarVisualsSpec pillarVisuals = this.TransportsConstructionHelper.ComputePillarVisuals(position2.ExtendZ(baseHeight.Value), newHeight, makeNotConstructed: true);
                pillarsVisualsTmp.Add(pillarVisuals);
              }
            }
          }
          ++index;
        }
        else
          break;
      }
      return pillarsVisualsTmp.ToImmutableArrayAndClear();
    }

    private void simUpdate()
    {
      foreach (LayoutEntityPreview layoutEntityPreview in this.m_activePreviewsForSim)
        layoutEntityPreview.SimUpdate();
    }

    private void syncUpdate()
    {
      if (this.m_activePreviews.IsEmpty)
      {
        this.m_activePreviewsForSim.Clear();
      }
      else
      {
        this.m_removedPreviews.Clear();
        this.m_activePreviews.RemoveWhere((Predicate<LayoutEntityPreview>) (x => x.IsDestroyed), this.m_removedPreviews);
        if (this.m_removedPreviews.IsNotEmpty)
        {
          this.m_updateActivePreviewsForSim = true;
          foreach (LayoutEntityPreview removedPreview in this.m_removedPreviews)
            this.m_previewPool.ReturnInstanceKeepReference(removedPreview);
          this.m_removedPreviews.Clear();
        }
        if (this.m_updateActivePreviewsForSim)
        {
          this.m_updateActivePreviewsForSim = false;
          this.m_activePreviewsForSim.Clear();
          this.m_activePreviewsForSim.AddRange(this.m_activePreviews);
          this.m_forceValidityUpdate = true;
        }
        foreach (LayoutEntityPreview activePreview in this.m_activePreviews)
          activePreview.SyncUpdate();
      }
    }

    private void renderUpdate(GameTime time)
    {
      if (this.m_activePreviews.IsEmpty)
        return;
      bool forceValidityUpdate = this.m_forceValidityUpdate;
      for (int index = 0; index < this.m_activePreviews.Count; ++index)
      {
        LayoutEntityPreview activePreview = this.m_activePreviews[index];
        activePreview.RenderUpdate();
        forceValidityUpdate |= activePreview.ValidityDataWasUpdated;
      }
      if (forceValidityUpdate)
      {
        this.updateValidityData();
        this.m_forceValidityUpdate = false;
      }
      if (this.m_drawArgs[1] <= 0U)
        return;
      Graphics.DrawMeshInstancedIndirect(this.m_tileValidationMesh, 0, this.m_tileValidationMaterial, this.m_worldBounds, this.m_drawArgsBuffer, 0, (MaterialPropertyBlock) null, ShadowCastingMode.Off, false, Layer.Custom14TerrainOverlays.ToId(), (Camera) null, LightProbeUsage.Off, (LightProbeProxyVolume) null);
    }

    private void updateValidityData()
    {
      int num = 0;
      foreach (LayoutEntityPreview activePreview in this.m_activePreviews)
        num += activePreview.ValidityDataElementsCount;
      this.m_drawArgs[1] = (uint) num;
      if (num <= 0)
        return;
      if (this.m_validityData == null || this.m_validityData.Length < num)
      {
        this.m_validityData = new TileValidityInstanceData[num + 32];
        this.m_detailsDataBuffer?.Dispose();
        this.m_detailsDataBuffer = new ComputeBuffer(this.m_validityData.Length, 12);
        this.m_tileValidationMaterial.SetBuffer(LayoutEntityPreviewManager.INSTANCE_DATA_SHADER_ID, this.m_detailsDataBuffer);
        this.m_tileValidationMaterial.SetTexture(LayoutEntityPreviewManager.HEIGHT_TEXTURE_SHADER_ID, (Texture) this.m_terrainRenderer.HeightTexture);
        this.m_tileValidationMaterial.SetVector(LayoutEntityPreviewManager.TERRAIN_SIZE_SHADER_ID, new Vector4(1f / (float) (this.TerrainManager.TerrainWidth * 2), 1f / (float) (this.TerrainManager.TerrainHeight * 2)));
      }
      int destinationIndex = 0;
      foreach (LayoutEntityPreview activePreview in this.m_activePreviews)
      {
        KeyValuePair<TileValidityInstanceData[], int> dataAndMarkUpdated = activePreview.GetValidityDataAndMarkUpdated();
        Array.Copy((Array) dataAndMarkUpdated.Key, 0, (Array) this.m_validityData, destinationIndex, dataAndMarkUpdated.Value);
        destinationIndex += dataAndMarkUpdated.Value;
      }
      Assert.That<int>(destinationIndex).IsEqualTo(num);
      this.m_detailsDataBuffer.SetData((Array) this.m_validityData, 0, 0, num);
      this.m_drawArgsBuffer.SetData((Array) this.m_drawArgs);
    }

    static LayoutEntityPreviewManager()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LayoutEntityPreviewManager.INSTANCE_DATA_SHADER_ID = Shader.PropertyToID("_PerInstanceData");
      LayoutEntityPreviewManager.HEIGHT_TEXTURE_SHADER_ID = Shader.PropertyToID("_HeightTex");
      LayoutEntityPreviewManager.TERRAIN_SIZE_SHADER_ID = Shader.PropertyToID("_TerrainInvSize");
    }
  }
}
