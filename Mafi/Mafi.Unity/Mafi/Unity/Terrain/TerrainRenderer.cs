// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.TerrainRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using BHxAtV2MExQlQFOmks;
using Mafi.Base.Terrain.PostProcessors;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Core.Utils;
using Mafi.Localization;
using Mafi.Unity.Camera;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.MapEditor;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  /// <summary>
  /// Handles chunked and LODed rendering of: terrain, terrain details, tile surfaces, and ocean.
  /// This class handles all these at the same time to make them efficient since each uses similar infra for chunking,
  /// syncing, and rendering.
  /// </summary>
  [HasRenderingSettings]
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  public class TerrainRenderer : 
    ITerrainRenderer,
    IRenderedChunkAlwaysVisible,
    IRenderedChunksBase,
    IDisposable
  {
    private static readonly ObjectPool<TerrainRenderer.TexUpdateDataSlim[]> s_allTilesDataPool;
    private static readonly int GRID_SHADER_PROP_ID;
    public static readonly LocStr TerrainQuality;
    public static readonly RenderingSetting TerrainQualityRenderSetting;
    public static readonly LocStr TerrainDetailsRenderDistance;
    public static readonly RenderingSetting TerrainDetailsRenderDistanceSetting;
    private static readonly int DECAL_PREVIEW_AREA_SHADER_ID;
    private static readonly int DECAL_PREVIEW_ROTATION_SHADER_ID;
    private static readonly int DECAL_PREVIEW_FLIPPED_SHADER_ID;
    private static readonly int DECAL_PREVIEW_TEXTURE_SHADER_ID;
    private static readonly int DECAL_COLOR_KEY_SHADER_ID;
    private static readonly int SURFACE_PASTE_PREVIEW_BUFFER;
    private static readonly int USING_SURFACE_PASTE;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly TerrainManager m_terrain;
    private readonly TerrainOccupancyManager m_occupancyManager;
    private readonly AssetsDb m_assetsDb;
    private readonly TileSurfacesSlimIdManager m_surfaceSlimIdManager;
    private readonly TileSurfaceDecalsSlimIdManager m_surfaceDecalsSlimIdManager;
    private readonly IResolver m_resolver;
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly CameraController m_cameraController;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly TerrainSurfaceTextureManager m_terrainSurfaceTextureManager;
    private readonly ActivatorState m_gridLinesActivatorState;
    private TerrainRenderer.TerrainRendererChunk[] m_renderChunks;
    private readonly Pair<int, float>[] m_newDetailsTmp;
    private LystStruct<ComputeBuffer> m_buffersToDispose;
    private readonly ImmutableArray<DetailLayerSpecProto> m_detailLayers;
    private readonly ImmutableArray<Pair<int, float>[]> m_detailIndicesPerMaterial;
    private ImmutableArray<Pair<Mesh[], Material>> m_detailLayerMeshesAndMaterials;
    private readonly float[] m_detailSpawnProbabilities;
    private readonly TerrainRenderer.RandomDetailOffset[] m_randomDetailOffsets;
    private readonly Mesh m_terrainChunkMesh;
    private Material m_terrainMaterial;
    public readonly Texture2D HeightTexture;
    private Texture2D m_splatTexture;
    private InstancedMeshesRenderer<TerrainRenderer.TerrainChunkInstanceData> m_instancedTerrainRenderer;
    private Texture2D m_perTexProperties;
    private byte[] m_slimIdToTextureIndex;
    private Mesh[] m_tileSurfaceMeshShared;
    private Material m_tileSurfaceMaterialShared;
    private Bounds m_terrainBounds;
    private float m_minHeight;
    private float m_maxHeight;
    private bool m_usesTriplanarShader;
    private float m_globalDetailDensity;
    private bool m_detailRenderingDisabled;
    private float m_farDetailsPpmThreshold;
    private float m_closeDetailsPpmThreshold;
    private float m_terrainDetailsFarRenderDistance;
    private float m_terrainDetailsCloseRenderDistance;
    private float m_terrainDetailsAnimEndDistance;
    private ComputeBuffer m_surfacePreviewDataGPU;
    private bool m_usingXRay;
    private Tile2i m_xRayPosition;
    private RelTile1i m_xRayRadius;
    private ThicknessTilesI m_xRayDepthOffset;

    public string Name => "Terrain";

    public Mesh TerrainChunkMesh => this.m_terrainChunkMesh;

    public Texture2DArray DiffuseArray { get; private set; }

    public Texture2DArray NormalSaoArray { get; private set; }

    /// <summary>
    /// Terrain update frequency based on LOD, lat value is update frequency of chunks that are not visible.
    /// We still update chunks that are not visible to avoid huge visual jumps when camera jumps to a new position.
    /// The numbers are primes to help avoid updating too many chunks in the same frame when camera is stationary.
    /// IMPORTANT: It is critical that all values are greater than 2 to avoid race conflicts during update.
    /// </summary>
    public ImmutableArray<int> UpdateDelayPerLod { get; private set; }

    public event Action TexturesReloaded;

    public TerrainRenderer(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      TerrainManager terrainManager,
      TerrainOccupancyManager occupancyManager,
      AssetsDb assetsDb,
      TileSurfacesSlimIdManager surfaceSlimIdManager,
      TileSurfaceDecalsSlimIdManager surfaceDecalsSlimIdManager,
      IResolver resolver,
      ChunkBasedRenderingManager chunksRenderer,
      CameraController cameraController,
      ReloadAfterAssetUpdateManager reloadManager,
      TerrainSurfaceTextureManager terrainSurfaceTextureManager,
      ParticleErosionPostProcessor particleErosionPostProcessor)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_usesTriplanarShader = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_simLoopEvents = simLoopEvents;
      this.m_terrain = terrainManager;
      this.m_occupancyManager = occupancyManager;
      this.m_assetsDb = assetsDb;
      this.m_surfaceSlimIdManager = surfaceSlimIdManager;
      this.m_surfaceDecalsSlimIdManager = surfaceDecalsSlimIdManager;
      this.m_resolver = resolver;
      this.m_chunksRenderer = chunksRenderer;
      this.m_cameraController = cameraController;
      this.m_reloadManager = reloadManager;
      this.m_terrainSurfaceTextureManager = terrainSurfaceTextureManager;
      this.m_gridLinesActivatorState = new ActivatorState(new Action(this.showGridLines), new Action(this.hideGridLines));
      this.m_renderChunks = new TerrainRenderer.TerrainRendererChunk[chunksRenderer.ChunksCountTotal];
      this.m_randomDetailOffsets = new TerrainRenderer.RandomDetailOffset[65536];
      this.m_detailSpawnProbabilities = new float[65536];
      this.hideGridLines();
      this.UpdateDelayPerLod = ImmutableArray.Create<int>(2, 2, 2, 13, 13, 67, 241, 593);
      ImmutableArray<TerrainMaterialProto> terrainMaterials = terrainManager.TerrainMaterials;
      this.m_detailLayers = terrainMaterials.SelectMany<DetailLayerSpec>((Func<TerrainMaterialProto, IEnumerable<DetailLayerSpec>>) (x => x.Graphics.DetailLayers.AsEnumerable())).Select<DetailLayerSpec, DetailLayerSpecProto>((Func<DetailLayerSpec, DetailLayerSpecProto>) (x => x.DetailLayerProto)).Distinct<DetailLayerSpecProto>().OrderBy<DetailLayerSpecProto, Proto.ID>((Func<DetailLayerSpecProto, Proto.ID>) (x => x.Id)).ToImmutableArray<DetailLayerSpecProto>();
      terrainMaterials = terrainManager.TerrainMaterials;
      this.m_detailIndicesPerMaterial = terrainMaterials.Map<Pair<int, float>[]>((Func<TerrainMaterialProto, Pair<int, float>[]>) (x => !x.Graphics.DetailLayers.IsEmpty ? x.Graphics.DetailLayers.MapArray<Pair<int, float>>((Func<DetailLayerSpec, Pair<int, float>>) (d => Pair.Create<int, float>(this.m_detailLayers.IndexOf(d.DetailLayerProto), d.SpawnProbability))) : (Pair<int, float>[]) null));
      this.m_newDetailsTmp = new Pair<int, float>[this.m_detailLayers.Length];
      this.m_terrainChunkMesh = MeshBuilder.CreateChunkMeshWithSkirt(64, 2f, 2f, true);
      this.m_terrainChunkMesh.name = "Terrain chunk";
      chunksRenderer.RegisterChunkAlwaysRender((IRenderedChunkAlwaysVisible) this);
      gameLoopEvents.RegisterRendererInitState((object) this, new Action(this.initState));
      cameraController.CameraFovChanged += new Action<float>(this.cameraFovChanged);
      TerrainRenderer.TerrainQualityRenderSetting.OnSettingChange += new Action<RenderingSetting>(this.onTerrainQualityChange);
      TerrainRenderer.TerrainDetailsRenderDistanceSetting.OnSettingChange += new Action<RenderingSetting>(this.onDetailsRenderDistanceChange);
      Texture2D texture2D = new Texture2D(this.m_terrain.TerrainWidth, this.m_terrain.TerrainHeight, TextureFormat.RFloat, false, true);
      texture2D.name = "TerrainHeight";
      texture2D.anisoLevel = 0;
      texture2D.filterMode = FilterMode.Bilinear;
      texture2D.wrapMode = TextureWrapMode.Clamp;
      this.HeightTexture = texture2D;
      this.onTerrainQualityChange(TerrainRenderer.TerrainQualityRenderSetting);
      this.onDetailsRenderDistanceChange(TerrainRenderer.TerrainDetailsRenderDistanceSetting);
    }

    public void Dispose()
    {
      TerrainRenderer.TerrainQualityRenderSetting.OnSettingChange -= new Action<RenderingSetting>(this.onTerrainQualityChange);
      TerrainRenderer.TerrainDetailsRenderDistanceSetting.OnSettingChange -= new Action<RenderingSetting>(this.onDetailsRenderDistanceChange);
      this.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<TerrainRenderer.TerrainChunkInstanceData>>(this.m_instancedTerrainRenderer);
      if (this.m_buffersToDispose.IsNotEmpty)
      {
        foreach (ComputeBuffer computeBuffer in this.m_buffersToDispose)
          computeBuffer.Dispose();
        this.m_buffersToDispose.Clear();
      }
      this.m_surfacePreviewDataGPU?.Dispose();
      this.m_surfacePreviewDataGPU = (ComputeBuffer) null;
      foreach (TerrainRenderer.TerrainRendererChunk renderChunk in this.m_renderChunks)
        renderChunk?.Dispose();
      this.HeightTexture.DestroyIfNotNull();
      this.m_splatTexture.DestroyIfNotNull();
      this.DiffuseArray.DestroyIfNotNull();
      this.NormalSaoArray.DestroyIfNotNull();
    }

    private void initState()
    {
      Texture2D texture2D = new Texture2D(this.m_terrain.TerrainWidth, this.m_terrain.TerrainHeight, TextureFormat.RGBA32, false, true);
      texture2D.name = "TerrainSplat";
      texture2D.anisoLevel = 0;
      texture2D.filterMode = FilterMode.Bilinear;
      texture2D.wrapMode = TextureWrapMode.Clamp;
      this.m_splatTexture = texture2D;
      Texture2DArray diffuseArray;
      Texture2DArray normalSaoArray;
      Texture2D perTexProperties;
      TerrainRenderer.buildTexArrays(this.m_terrain.TerrainMaterials, this.m_assetsDb, false, out diffuseArray, out normalSaoArray, out perTexProperties, out this.m_slimIdToTextureIndex);
      this.DiffuseArray = diffuseArray;
      this.NormalSaoArray = normalSaoArray;
      this.m_perTexProperties = perTexProperties;
      this.m_terrainMaterial = this.createNewMaterial(this.m_usesTriplanarShader);
      this.m_instancedTerrainRenderer = new InstancedMeshesRenderer<TerrainRenderer.TerrainChunkInstanceData>(LodUtils.SameMeshForAllLods(this.m_terrainChunkMesh), this.m_terrainMaterial, layer: Layer.Custom12Terrain);
      this.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_instancedTerrainRenderer);
      this.initializeDetailSpawnProbabilities();
      XorRsr128PlusGenerator random = new XorRsr128PlusGenerator(RandomGeneratorType.NonSim, 78261746543UL, 18446744073662900162UL);
      random.Jump();
      for (int index = 0; index < this.m_randomDetailOffsets.Length; ++index)
      {
        ulong dx = random.NextUlong();
        this.m_randomDetailOffsets[index] = new TerrainRenderer.RandomDetailOffset((sbyte) dx, (sbyte) (dx >> 8), (ushort) (dx >> 16), TerrainRenderer.RandomDetailOffset.PackCosSin(random.NextFloat() * 6.28318548f), (short) (dx >> 32));
      }
      bool anyFailed = false;
      this.m_detailLayerMeshesAndMaterials = this.m_detailLayers.Map<Pair<Mesh[], Material>>((Func<DetailLayerSpecProto, Pair<Mesh[], Material>>) (x =>
      {
        Mesh[] meshes;
        Material sharedMaterial;
        string error;
        if (!InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_assetsDb, x.PrefabPath, out meshes, out sharedMaterial, out error))
        {
          Log.Error("Failed to load detail layer: " + error);
          anyFailed = true;
          return new Pair<Mesh[], Material>();
        }
        Material second = UnityEngine.Object.Instantiate<Material>(sharedMaterial);
        second.SetTexture("_HeightTex", (Texture) this.HeightTexture);
        second.SetTexture("_TerrainSplats", (Texture) this.m_splatTexture);
        second.SetTexture("_TerrainDiffuseArray", (Texture) this.DiffuseArray);
        second.SetVector("_TerrainInvSizeAndNormalWeight", new Vector4(1f / (float) this.m_terrain.TerrainWidth, 1f / (float) this.m_terrain.TerrainHeight, 0.0f, x.UpNormalWeight));
        second.SetColor("_HueVariationColor", x.TintColorAndWeight.AsColor());
        second.SetVector("_WindParams", new Vector4(x.WindSensitivity * 0.4f, 2f, 1.5f, 0.0f));
        return Pair.Create<Mesh[], Material>(meshes, second);
      }));
      this.reloadDetailTextures();
      if (anyFailed)
      {
        Pair<Mesh[], Material> validPair = this.m_detailLayerMeshesAndMaterials.FindOrDefault((Predicate<Pair<Mesh[], Material>>) (x => x.First != null));
        this.m_detailLayerMeshesAndMaterials = this.m_detailLayerMeshesAndMaterials.Map<Pair<Mesh[], Material>>((Func<Pair<Mesh[], Material>, Pair<Mesh[], Material>>) (x => x.First != null ? x : validPair));
      }
      string error1;
      if (!InstancingUtils.TryGetMeshLodsAndMaterialFromPrefab(this.m_assetsDb, "Assets/Base/Terrain/TileSurfaceCube.prefab", out this.m_tileSurfaceMeshShared, out this.m_tileSurfaceMaterialShared, out error1))
        Log.Error("Failed  to load tile surface cube prefab: " + error1);
      Material surfaceMaterialShared = this.m_tileSurfaceMaterialShared;
      TerrainSurfaceTextureManager surfaceTextureManager = this.m_terrainSurfaceTextureManager;
      surfaceMaterialShared.SetTexture("_AlbedoTex", (Texture) surfaceTextureManager.DiffuseSurfacesArray);
      surfaceMaterialShared.SetTexture("_NormalsTex", (Texture) surfaceTextureManager.NormalSurfacesArray);
      surfaceMaterialShared.SetTexture("_SmoothMetalTex", (Texture) surfaceTextureManager.SmoothMetalSurfaceArray);
      surfaceMaterialShared.SetTexture("_AlbedoEdgeFullTex", (Texture) surfaceTextureManager.DiffuseSurfacesEdgeArrayFull);
      surfaceMaterialShared.SetTexture("_NormalsEdgeFullTex", (Texture) surfaceTextureManager.NormalSurfacesEdgeArrayFull);
      surfaceMaterialShared.SetTexture("_SmoothMetalEdgeFullTex", (Texture) surfaceTextureManager.SmoothMetalSurfacesEdgeArrayFull);
      surfaceMaterialShared.SetTexture("_AlbedoEdgeHorizontalTex", (Texture) surfaceTextureManager.DiffuseSurfacesEdgeArrayHorizontal);
      surfaceMaterialShared.SetTexture("_NormalsEdgeHorizontalTex", (Texture) surfaceTextureManager.NormalSurfacesEdgeArrayHorizontal);
      surfaceMaterialShared.SetTexture("_SmoothMetalEdgeHorizontalTex", (Texture) surfaceTextureManager.SmoothMetalSurfacesEdgeArrayHorizontal);
      surfaceMaterialShared.SetTexture("_AlbedoEdgeVerticalTex", (Texture) surfaceTextureManager.DiffuseSurfacesEdgeArrayVertical);
      surfaceMaterialShared.SetTexture("_NormalsEdgeVerticalTex", (Texture) surfaceTextureManager.NormalSurfacesEdgeArrayVertical);
      surfaceMaterialShared.SetTexture("_SmoothMetalEdgeVerticalTex", (Texture) surfaceTextureManager.SmoothMetalSurfacesEdgeArrayVertical);
      surfaceMaterialShared.SetTexture("_AlbedoEdgeCornersTex", (Texture) surfaceTextureManager.DiffuseSurfacesEdgeArrayCorners);
      surfaceMaterialShared.SetTexture("_NormalsEdgeCornersTex", (Texture) surfaceTextureManager.NormalSurfacesEdgeArrayCorners);
      surfaceMaterialShared.SetTexture("_SmoothMetalEdgeCornersTex", (Texture) surfaceTextureManager.SmoothMetalSurfacesEdgeArrayCorners);
      surfaceMaterialShared.SetTexture("_DecalAlbedoTex", (Texture) surfaceTextureManager.DiffuseSurfaceDecalsArray);
      for (int index = 0; index < this.m_renderChunks.Length; ++index)
      {
        TerrainRenderer.TerrainRendererChunk newChunk = new TerrainRenderer.TerrainRendererChunk(this.m_chunksRenderer.ExtendChunkCoord(new Chunk256Index((ushort) index)), this, random.NextInt(this.m_randomDetailOffsets.Length));
        this.m_renderChunks[index] = newChunk;
        this.m_chunksRenderer.RegisterChunk((IRenderedChunk) newChunk);
        newChunk.InitializeData();
      }
      this.hideGridLines();
      this.HeightTexture.Apply(false, true);
      this.m_splatTexture.Apply(false, true);
      this.m_terrain.HeightChanged.AddNonSaveable<TerrainRenderer>(this, new Action<Tile2iAndIndex>(this.TileChanged));
      this.m_terrain.TileMaterialsOnlyChanged.AddNonSaveable<TerrainRenderer>(this, new Action<Tile2iAndIndex>(this.TileChanged));
      this.m_terrain.TileCustomSurfaceChanged.AddNonSaveable<TerrainRenderer>(this, new Action<Tile2iAndIndex>(this.tileSurfaceChanged));
      this.m_occupancyManager.TileOccupancyChanged.AddNonSaveable<TerrainRenderer>(this, new Action<Tile2iAndIndex>(this.TileChanged));
      this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<TerrainRenderer>(this, new Action<GameTime>(this.renderUpdate));
      this.m_simLoopEvents.UpdateEndForUi.AddNonSaveable<TerrainRenderer>(this, new Action(this.simUpdate));
    }

    private void initializeDetailSpawnProbabilities()
    {
      if ((double) this.m_globalDetailDensity <= 0.0)
      {
        Array.Clear((Array) this.m_detailSpawnProbabilities, 0, this.m_detailSpawnProbabilities.Length);
      }
      else
      {
        XorRsr128PlusGenerator random = new XorRsr128PlusGenerator(RandomGeneratorType.NonSim, 261746543UL, 18446744073702900162UL);
        random.Jump();
        for (int index = 0; index < this.m_randomDetailOffsets.Length; ++index)
          this.m_detailSpawnProbabilities[index] = random.NextFloat() / this.m_globalDetailDensity;
      }
    }

    private void reloadDetailTextures()
    {
      for (int index = 0; index < this.m_detailLayerMeshesAndMaterials.Length; ++index)
      {
        Texture2D result;
        if (this.m_assetsDb.TryGetSharedAsset<Texture2D>(this.m_detailLayers[index].TexturePath, out result, doNotCache: true))
          this.m_detailLayerMeshesAndMaterials[index].Second.SetTexture("_MainTex", (Texture) result);
      }
    }

    public void TileChanged(Tile2iAndIndex tileAndIndex)
    {
      this.m_renderChunks[(int) this.m_chunksRenderer.GetChunkIndex(tileAndIndex.TileCoord).Value].TileChanged(TileInChunk256.FromTile(tileAndIndex.TileCoord));
    }

    private void tileSurfaceChanged(Tile2iAndIndex tileAndIndex)
    {
      Chunk256Index chunkIndex = this.m_chunksRenderer.GetChunkIndex(tileAndIndex.TileCoord);
      this.m_renderChunks[(int) chunkIndex.Value].TileSurfaceChanged(TileInChunk256.FromTile(tileAndIndex.TileCoord), this.m_terrain.GetTileSurface(tileAndIndex.Index));
      int terrainWidth = this.m_terrain.TerrainWidth;
      updatePositiveNeighbor(tileAndIndex.PlusXNeighborUnchecked);
      updatePositiveNeighbor(tileAndIndex.PlusXPlusYNeighborUnchecked(terrainWidth));
      updatePositiveNeighbor(tileAndIndex.PlusYNeighborUnchecked(terrainWidth));
      updateNeighbor(tileAndIndex.MinusXPlusYNeighborUnchecked(terrainWidth));
      updateNeighbor(tileAndIndex.MinusXNeighborUnchecked);
      updateNeighbor(tileAndIndex.MinusXMinusYNeighborUnchecked(terrainWidth));
      updateNeighbor(tileAndIndex.MinusYNeighborUnchecked(terrainWidth));
      updateNeighbor(tileAndIndex.PlusXMinusYNeighborUnchecked(terrainWidth));

      void updatePositiveNeighbor(Tile2iAndIndex neighbor)
      {
        chunkIndex = this.m_chunksRenderer.GetChunkIndex(neighbor.TileCoord);
        TileInChunk256 localIndex = TileInChunk256.FromTile(neighbor.TileCoord);
        this.m_renderChunks[(int) chunkIndex.Value].TileChanged(localIndex);
        this.m_renderChunks[(int) chunkIndex.Value].TileNeighborSurfaceChanged(localIndex);
      }

      void updateNeighbor(Tile2iAndIndex neighbor)
      {
        chunkIndex = this.m_chunksRenderer.GetChunkIndex(neighbor.TileCoord);
        this.m_renderChunks[(int) chunkIndex.Value].TileNeighborSurfaceChanged(TileInChunk256.FromTile(neighbor.TileCoord));
      }
    }

    private void cameraFovChanged(float fov)
    {
      this.m_gameLoopEvents.InvokeInSyncNotSaved(new Action(this.onDetailsRenderDistanceChangeSync));
    }

    public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
    {
      info.Add(new RenderedInstancesInfo("Terrain chunks", this.m_terrain.TerrainTilesCount / 4096, 24576));
    }

    public byte GetMaterialTextureIndex(TerrainMaterialSlimId slimId)
    {
      return this.m_slimIdToTextureIndex[(int) slimId.Value];
    }

    private TerrainRenderer.TexUpdateDataSlim[] getAllTilesChunkDataPooled()
    {
      return TerrainRenderer.s_allTilesDataPool.GetInstance();
    }

    private void returnAllTilesChunkDataToPool(TerrainRenderer.TexUpdateDataSlim[] data)
    {
      TerrainRenderer.s_allTilesDataPool.ReturnInstance(data);
    }

    private Material createNewMaterial(bool useTriplanar)
    {
      Vector4 vector4 = new Vector4((float) this.m_terrain.TerrainWidth, (float) this.m_terrain.TerrainHeight, 1f / (float) this.m_terrain.TerrainWidth, 1f / (float) this.m_terrain.TerrainHeight);
      Material clonedMaterial = this.m_assetsDb.GetClonedMaterial(useTriplanar ? "Assets/Base/Terrain/TerrainTriplanarInstanced.mat" : "Assets/Base/Terrain/TerrainInstanced.mat");
      clonedMaterial.SetTexture("_Diffuse", (Texture) this.DiffuseArray);
      clonedMaterial.SetTexture("_NormalSAO", (Texture) this.NormalSaoArray);
      clonedMaterial.SetTexture("_HeightTex", (Texture) this.HeightTexture);
      clonedMaterial.SetTexture("_MegaSplatTexture", (Texture) this.m_splatTexture);
      clonedMaterial.SetVector("_TerrainSize", vector4);
      clonedMaterial.SetTexture("_PerTexProps", (Texture) this.m_perTexProperties);
      return clonedMaterial;
    }

    private void setNewMaterial(bool useTriplanar)
    {
      Material newMaterial = this.createNewMaterial(useTriplanar);
      this.m_instancedTerrainRenderer.SetMaterial(newMaterial);
      this.m_terrainMaterial.DestroyIfNotNull();
      this.m_terrainMaterial = newMaterial;
    }

    private static void buildTexArrays(
      ImmutableArray<TerrainMaterialProto> terrainMaterialProtos,
      AssetsDb assetsDb,
      bool ignoreAssetsCache,
      out Texture2DArray diffuseArray,
      out Texture2DArray normalSaoArray,
      out Texture2D perTexProperties,
      out byte[] slimIdToTextureIndex)
    {
      Lyst<Pair<string, string>> texturePaths = new Lyst<Pair<string, string>>();
      Lyst<TerrainMaterialProto> texSpecsProto = new Lyst<TerrainMaterialProto>();
      Dict<string, byte> albedoPathToTexIndex = new Dict<string, byte>();
      slimIdToTextureIndex = new byte[terrainMaterialProtos.Length];
      for (int index = 0; index < slimIdToTextureIndex.Length; ++index)
        slimIdToTextureIndex[index] = byte.MaxValue;
      foreach (TerrainMaterialProto terrainMaterialProto in terrainMaterialProtos)
      {
        if (!terrainMaterialProto.IsPhantom)
        {
          int num = processTexture(terrainMaterialProto.Graphics.AlbedoHeightTexturePath, terrainMaterialProto.Graphics.NormalSaoTexturePath, terrainMaterialProto);
          slimIdToTextureIndex[(int) terrainMaterialProto.SlimId.Value] = (byte) num;
        }
      }
      Texture2D[] diffuseTextures = new Texture2D[texturePaths.Count];
      Texture2D[] normalSaoTextures = new Texture2D[texturePaths.Count];
      perTexProperties = new Texture2D(32, texturePaths.Count, TextureFormat.RGBAFloat, 0, true);
      for (int index1 = 0; index1 < texturePaths.Count; ++index1)
      {
        Pair<string, string> pair = texturePaths[index1];
        Texture2D result1;
        if (assetsDb.TryGetSharedAsset<Texture2D>(pair.First, out result1, doNotCache: true))
        {
          diffuseTextures[index1] = result1;
        }
        else
        {
          if (index1 <= 0)
            throw new FatalGameException("Failed to load diffuse texture from " + pair.Second + ".");
          Log.Error("Failed to load diffuse texture from " + pair.Second + ".");
          diffuseTextures[index1] = diffuseTextures[index1 - 1];
        }
        Texture2D result2;
        if (assetsDb.TryGetSharedAsset<Texture2D>(pair.Second, out result2, doNotCache: true))
        {
          normalSaoTextures[index1] = result2;
        }
        else
        {
          Log.Error("No NSAO texture for '" + pair.Second + "', creating temporary one.");
          result2 = new Texture2D(diffuseTextures[index1].width, diffuseTextures[index1].height, TextureFormat.RGBA32, true, true);
          Color32[] pixels32 = result2.GetPixels32(0);
          Color32 color32 = new Color32((byte) 0, (byte) 128, byte.MaxValue, (byte) 128);
          for (int index2 = 0; index2 < pixels32.Length; ++index2)
            pixels32[index2] = color32;
          result2.SetPixels32(pixels32);
          result2.Apply(true, false);
          result2.Compress(false);
          normalSaoTextures[index1] = result2;
        }
        Color color1 = new Color(0.0f, texSpecsProto[index1].Graphics.FullyWetSmoothnessDelta, 0.0f, 0.0f);
        Color color2 = new Color(texSpecsProto[index1].Graphics.FullyWetBrightnessDelta, 0.0f, 0.0f, 0.0f);
        perTexProperties.SetPixel(index1, 2, color1);
        perTexProperties.SetPixel(index1, 3, color2);
      }
      perTexProperties.Apply(false, true);
      TerrainRenderer.instantiateTexArrays(diffuseTextures, normalSaoTextures, out diffuseArray, out normalSaoArray);
      if (Log.IsLogged(Mafi.Logging.LogType.Debug))
        ((IEnumerable<byte>) slimIdToTextureIndex).Select<byte, KeyValuePair<byte, string>>((Func<byte, KeyValuePair<byte, string>>) (x => Make.Kvp<byte, string>(x, x == byte.MaxValue ? (string) null : texSpecsProto[(int) x].Id.Value))).Where<KeyValuePair<byte, string>>((Func<KeyValuePair<byte, string>, bool>) (x => x.Value != null)).GroupBy<KeyValuePair<byte, string>, byte>((Func<KeyValuePair<byte, string>, byte>) (x => x.Key)).OrderBy<IGrouping<byte, KeyValuePair<byte, string>>, byte>((Func<IGrouping<byte, KeyValuePair<byte, string>>, byte>) (x => x.Key)).Select<IGrouping<byte, KeyValuePair<byte, string>>, string>((Func<IGrouping<byte, KeyValuePair<byte, string>>, string>) (g => string.Format("#{0} ({1}): ", (object) g.Key, (object) Path.GetFileNameWithoutExtension(texturePaths[(int) g.Key].First)) + g.Select<KeyValuePair<byte, string>, string>((Func<KeyValuePair<byte, string>, string>) (x => x.Value)).Distinct<string>().JoinStrings(", "))).JoinStrings("\n");
      for (int index = 0; index < slimIdToTextureIndex.Length; ++index)
      {
        if (slimIdToTextureIndex[index] == byte.MaxValue)
          slimIdToTextureIndex[index] = (byte) 0;
      }

      int processTexture(
        string albedoHeightTexPath,
        string nsaoTexturePath,
        TerrainMaterialProto proto)
      {
        byte count;
        if (albedoPathToTexIndex.TryGetValue(albedoHeightTexPath, out count))
        {
          if (nsaoTexturePath != texturePaths[(int) count].Second)
            Log.Warning(string.Format("Proto '{0}' has same albedo terrain texture '{1}' ", (object) proto, (object) albedoHeightTexPath) + "with some other proto but they differ in NSAO textures: '" + nsaoTexturePath + "' != '" + texturePaths[(int) count].Second + "'");
        }
        else
        {
          count = (byte) texturePaths.Count;
          texturePaths.Add(Pair.Create<string, string>(albedoHeightTexPath, nsaoTexturePath));
          texSpecsProto.Add(proto);
          albedoPathToTexIndex.Add(albedoHeightTexPath, count);
        }
        return (int) count;
      }
    }

    private static void instantiateTexArrays(
      Texture2D[] diffuseTextures,
      Texture2D[] normalSaoTextures,
      out Texture2DArray diffuseArray,
      out Texture2DArray normalSaoArray)
    {
      object[] objArray = Bmdacxg2KgloU9v9tT.LOPiTZStoR(0, new object[4]
      {
        (object) diffuseTextures,
        (object) normalSaoTextures,
        (object) diffuseArray,
        (object) normalSaoArray
      }, (object) null);
      diffuseArray = (Texture2DArray) objArray[0];
      normalSaoArray = (Texture2DArray) objArray[1];
    }

    private void renderUpdate(GameTime time)
    {
      this.m_instancedTerrainRenderer.Clear();
      if (this.m_buffersToDispose.IsNotEmpty)
      {
        foreach (ComputeBuffer computeBuffer in this.m_buffersToDispose)
          computeBuffer.Dispose();
        this.m_buffersToDispose.Clear();
      }
      TerrainRenderer.TerrainRendererChunk[] renderChunks = this.m_renderChunks;
      int index = 0;
      while (index < renderChunks.Length && !renderChunks[index].RenderUpdate())
        ++index;
    }

    private void simUpdate()
    {
      int num = 0;
      foreach (TerrainRenderer.TerrainRendererChunk renderChunk in this.m_renderChunks)
      {
        num += renderChunk.ProcessChangesOnSim();
        if (num > 32768)
          break;
      }
    }

    public RenderStats RenderAlwaysVisible(GameTime time, Bounds bounds)
    {
      StateAssert.IsInGameState(GameLoopState.RenderUpdateEnd);
      return this.m_instancedTerrainRenderer.Render(bounds, 0);
    }

    private void onDetailsRenderDistanceChange(RenderingSetting setting)
    {
      this.m_gameLoopEvents.InvokeInSyncNotSaved(new Action(this.onDetailsRenderDistanceChangeSync));
    }

    private void onTerrainQualityChange(RenderingSetting setting)
    {
      this.m_usesTriplanarShader = setting.CurrentOption.Value != 0;
      if (this.m_instancedTerrainRenderer == null)
        return;
      this.setNewMaterial(this.m_usesTriplanarShader);
    }

    /// <summary>
    /// This can be used to disable terrain details generation which overrides the current game graphics settings.
    /// </summary>
    public void SetTerrainDetailsGeneration(bool isEnabled)
    {
      this.m_detailRenderingDisabled = !isEnabled;
      this.m_gameLoopEvents.InvokeInSyncNotSaved(new Action(this.onDetailsRenderDistanceChangeSync));
    }

    private void onDetailsRenderDistanceChangeSync()
    {
      float globalDetailDensity = this.m_globalDetailDensity;
      switch (this.m_detailRenderingDisabled ? 0 : TerrainRenderer.TerrainDetailsRenderDistanceSetting.CurrentOption.Value)
      {
        case 1:
          this.m_globalDetailDensity = 0.5f;
          this.m_farDetailsPpmThreshold = 2f;
          this.m_closeDetailsPpmThreshold = 5f;
          break;
        case 2:
          this.m_globalDetailDensity = 0.8f;
          this.m_farDetailsPpmThreshold = 1.5f;
          this.m_closeDetailsPpmThreshold = 4f;
          break;
        case 3:
          this.m_globalDetailDensity = 1f;
          this.m_farDetailsPpmThreshold = 1f;
          this.m_closeDetailsPpmThreshold = 3f;
          break;
        default:
          this.m_globalDetailDensity = 0.0f;
          this.m_farDetailsPpmThreshold = 4f;
          this.m_closeDetailsPpmThreshold = 8f;
          break;
      }
      this.m_terrainDetailsFarRenderDistance = LodUtils.GetCameraDistanceForPpm(this.m_farDetailsPpmThreshold, this.m_cameraController.Camera);
      this.m_terrainDetailsCloseRenderDistance = LodUtils.GetCameraDistanceForPpm(this.m_closeDetailsPpmThreshold, this.m_cameraController.Camera);
      this.m_terrainDetailsAnimEndDistance = this.m_terrainDetailsCloseRenderDistance / 2f;
      bool regenerateAllDetails = (double) globalDetailDensity != (double) this.m_globalDetailDensity;
      if (regenerateAllDetails)
        this.initializeDetailSpawnProbabilities();
      foreach (TerrainRenderer.TerrainRendererChunk renderChunk in this.m_renderChunks)
        renderChunk?.UpdateTerrainDetailsRenderingSettings(regenerateAllDetails);
    }

    private void updateBounds(float minHeight, float maxHeight)
    {
      this.m_minHeight = this.m_minHeight.Min(minHeight);
      this.m_maxHeight = this.m_maxHeight.Max(maxHeight);
      Vector3 size = new Vector3((float) this.m_terrain.TerrainWidth.TilesToUnityUnits(), this.m_maxHeight - this.m_minHeight, (float) this.m_terrain.TerrainHeight.TilesToUnityUnits());
      this.m_terrainBounds = new Bounds(new Vector3(size.x * 0.5f, this.m_minHeight.Average(this.m_maxHeight), size.z * 0.5f), size);
    }

    public Tile3f? Raycast(Ray ray)
    {
      float distance;
      if (!this.m_terrainBounds.IntersectRay(ray, out distance))
        return new Tile3f?();
      if ((double) ray.direction.y >= 0.0)
        return new Tile3f?();
      Vector3 direction = ray.direction;
      Vector3 vector3 = ((double) distance > 0.0 ? ray.origin + ray.direction * distance : ray.origin) * 0.5f;
      int num1 = 0;
      for (int index = this.m_terrain.TerrainWidth + this.m_terrain.TerrainHeight; num1 < index; ++num1)
      {
        Tile2i tile2i = new Tile2i(vector3.x.RoundToInt(), vector3.z.RoundToInt());
        if (!this.m_terrain.IsValidCoord(tile2i))
          return new Tile3f?(returnClosestPointTo(new Tile2f(vector3.x.ToFix32(), vector3.z.ToFix32())));
        float num2 = this.m_terrain.GetHeight(tile2i).Value.ToFloat();
        if ((double) num2 < (double) vector3.y)
        {
          float num3 = vector3.y - num2;
          vector3 += direction * (num3 * 0.25f).Max(1f);
        }
        else
          break;
      }
      float num4 = 0.5f;
      int num5 = -1;
      for (int index = 0; index < 100; ++index)
      {
        Tile2f tile2f = new Tile2f(vector3.x.ToFix32(), vector3.z.ToFix32());
        if (!this.m_terrain.IsValidCoord(tile2f))
          return new Tile3f?(returnClosestPointTo(tile2f));
        HeightTilesF height = this.m_terrain.GetHeight(tile2f);
        float self = vector3.y - height.Value.ToFloat();
        if ((double) self.Abs() <= 0.05000000074505806)
          return new Tile3f?(tile2f.ExtendHeight(height));
        int num6 = self.SignNoZero();
        if (num6 != num5)
          num4 *= 0.5f;
        vector3 += direction * self * num4;
        num5 = num6;
      }
      Log.Warning("Failed to raycast terrain.");
      return new Tile3f?(returnClosestPointTo(new Tile2f(vector3.x.ToFix32(), vector3.z.ToFix32())));

      Tile3f returnClosestPointTo(Tile2f tile)
      {
        return this.m_terrain.ExtendHeight(this.m_terrain.ClampToTerrainBounds(tile));
      }
    }

    public IActivator CreateGridLinesActivator()
    {
      return this.m_gridLinesActivatorState.CreateActivator();
    }

    public void NotifyChunkUpdated(Chunk2i chunk)
    {
      uint index = (uint) this.m_chunksRenderer.GetChunkIndex(chunk.Tile2i).Value;
      if (index < (uint) this.m_renderChunks.Length)
        this.m_renderChunks[(int) index].AllTilesChanged();
      else
        Log.Error(string.Format("Invalid chunk for update: {0}", (object) chunk));
    }

    public void SetUpdateDelayPerLod(ImmutableArray<int> updateDelayPerLod)
    {
      if (updateDelayPerLod.Length == this.UpdateDelayPerLod.Length)
      {
        this.UpdateDelayPerLod = updateDelayPerLod;
        if (!this.UpdateDelayPerLod.Any((Func<int, bool>) (x => x < 2)))
          return;
        Log.Warning("Some values of update-delay-per-LOD are less than 2, setting them to 2.");
        this.UpdateDelayPerLod = this.UpdateDelayPerLod.Map<int>((Func<int, int>) (x => x.Max(2)));
      }
      else
        Log.Error(string.Format("Invalid length of update-delay-per-LOD array, expected {0}, got {1}", (object) this.UpdateDelayPerLod.Length, (object) updateDelayPerLod.Length));
    }

    internal bool IsGridActive()
    {
      return (double) Shader.GetGlobalFloat(TerrainRenderer.GRID_SHADER_PROP_ID) == 1.0;
    }

    internal void ForceSetGridActive(bool isActive)
    {
      Shader.SetGlobalFloat(TerrainRenderer.GRID_SHADER_PROP_ID, isActive ? 1f : 0.0f);
    }

    public void SetXRayData(Tile2i position, RelTile1i radius, ThicknessTilesI depthOffset)
    {
      this.m_usingXRay = true;
      this.m_xRayPosition = position;
      this.m_xRayRadius = radius;
      this.m_xRayDepthOffset = depthOffset;
    }

    public void DisableXRay() => this.m_usingXRay = false;

    private void showGridLines() => Shader.SetGlobalFloat(TerrainRenderer.GRID_SHADER_PROP_ID, 1f);

    private void hideGridLines()
    {
      Shader.SetGlobalFloat(TerrainRenderer.GRID_SHADER_PROP_ID, 0.0f);
    }

    private int collectSplatData(Tile2iIndex index, Pair<TerrainMaterialSlimId, Fix32>[] data)
    {
      TileMaterialLayers layersRawData = this.m_terrain.GetLayersRawData(index);
      if (layersRawData.Count <= 0)
      {
        data[0] = Pair.Create<TerrainMaterialSlimId, Fix32>(this.m_terrain.Bedrock.SlimId, Fix32.One);
        return 1;
      }
      Fix32 second = layersRawData.First.Thickness.Value;
      if (second >= Fix32.One)
      {
        data[0] = Pair.Create<TerrainMaterialSlimId, Fix32>(layersRawData.First.SlimId, Fix32.One);
        return 1;
      }
      data[0] = Pair.Create<TerrainMaterialSlimId, Fix32>(layersRawData.First.SlimId, second);
      int splatsCount = 1;
      if (layersRawData.Count == 1)
      {
        addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One - second);
        return splatsCount;
      }
      Fix32 weight1 = layersRawData.Second.Thickness.Value;
      Fix32 fix32_1 = second + weight1;
      if (fix32_1 >= Fix32.One)
      {
        addSplat(layersRawData.Second.SlimId, Fix32.One - second);
        return splatsCount;
      }
      addSplat(layersRawData.Second.SlimId, weight1);
      if (layersRawData.Count == 2)
      {
        addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One - fix32_1);
        return splatsCount;
      }
      Fix32 weight2 = layersRawData.Third.Thickness.Value;
      Fix32 fix32_2 = fix32_1 + weight2;
      if (fix32_2 >= Fix32.One)
      {
        addSplat(layersRawData.Third.SlimId, Fix32.One - second - weight1);
        return splatsCount;
      }
      addSplat(layersRawData.Third.SlimId, weight2);
      if (layersRawData.Count == 3)
      {
        addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One - fix32_2);
        return splatsCount;
      }
      addSplat(layersRawData.Fourth.SlimId, Fix32.One - fix32_2);
      return splatsCount;

      void addSplat(TerrainMaterialSlimId id, Fix32 weight)
      {
        for (int index = 0; index < splatsCount; ++index)
        {
          Pair<TerrainMaterialSlimId, Fix32> pair = data[index];
          if (pair.First == id)
          {
            data[index] = Pair.Create<TerrainMaterialSlimId, Fix32>(id, pair.Second + weight);
            return;
          }
        }
        data[splatsCount] = Pair.Create<TerrainMaterialSlimId, Fix32>(id, weight);
        ++splatsCount;
      }
    }

    private int collectSplatDataWithDeltaHeight(
      Tile2iIndex index,
      ThicknessTilesF deltaHeight,
      Pair<TerrainMaterialSlimId, Fix32>[] data)
    {
      TileMaterialLayers layersRawData = this.m_terrain.GetLayersRawData(index);
      if (layersRawData.Count <= 0)
      {
        data[0] = Pair.Create<TerrainMaterialSlimId, Fix32>(this.m_terrain.Bedrock.SlimId, Fix32.One);
        return 1;
      }
      int splatsCount = 0;
      ThicknessTilesF thicknessTilesF1 = deltaHeight;
      Fix32 fix32_1 = layersRawData.First.Thickness.Value - thicknessTilesF1.Value;
      Fix32 second = fix32_1.Max(Fix32.Zero);
      ThicknessTilesF thicknessTilesF2;
      if (second == Fix32.Zero)
      {
        thicknessTilesF2 = thicknessTilesF1 - layersRawData.First.Thickness;
      }
      else
      {
        if (second >= Fix32.One)
        {
          data[splatsCount] = Pair.Create<TerrainMaterialSlimId, Fix32>(layersRawData.First.SlimId, Fix32.One);
          return 1;
        }
        thicknessTilesF2 = ThicknessTilesF.Zero;
        data[splatsCount] = Pair.Create<TerrainMaterialSlimId, Fix32>(layersRawData.First.SlimId, second);
        splatsCount++;
      }
      if (layersRawData.Count == 1)
      {
        addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One - second);
        return splatsCount;
      }
      fix32_1 = layersRawData.Second.Thickness.Value - thicknessTilesF2.Value;
      Fix32 weight1 = fix32_1.Max(Fix32.Zero);
      Fix32 fix32_2 = second;
      Fix32 fix32_3 = fix32_2 + weight1;
      ThicknessTilesF thicknessTilesF3;
      if (weight1 == Fix32.Zero)
      {
        thicknessTilesF3 = thicknessTilesF2 - layersRawData.Second.Thickness;
      }
      else
      {
        if (fix32_3 >= Fix32.One)
        {
          addSplat(layersRawData.Second.SlimId, Fix32.One - fix32_2);
          return splatsCount;
        }
        thicknessTilesF3 = ThicknessTilesF.Zero;
        addSplat(layersRawData.Second.SlimId, weight1);
      }
      if (layersRawData.Count == 2)
      {
        addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One - fix32_3);
        return splatsCount;
      }
      fix32_1 = layersRawData.Third.Thickness.Value - thicknessTilesF3.Value;
      Fix32 weight2 = fix32_1.Max(Fix32.Zero);
      Fix32 fix32_4 = fix32_3;
      Fix32 fix32_5 = fix32_3 + weight2;
      ThicknessTilesF thicknessTilesF4;
      if (weight2 == Fix32.Zero)
      {
        thicknessTilesF4 = thicknessTilesF3 - layersRawData.Third.Thickness;
      }
      else
      {
        if (fix32_5 >= Fix32.One)
        {
          addSplat(layersRawData.Third.SlimId, Fix32.One - fix32_4);
          return splatsCount;
        }
        thicknessTilesF4 = ThicknessTilesF.Zero;
        addSplat(layersRawData.Third.SlimId, weight2);
      }
      if (layersRawData.Count == 3)
      {
        addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One - fix32_5);
        return splatsCount;
      }
      fix32_1 = layersRawData.Fourth.Thickness.Value - thicknessTilesF4.Value;
      Fix32 weight3 = fix32_1.Max(Fix32.Zero);
      Fix32 fix32_6 = fix32_5;
      Fix32 fix32_7 = fix32_5 + weight3;
      ThicknessTilesF thicknessTilesF5;
      if (weight3 == Fix32.Zero)
      {
        thicknessTilesF5 = thicknessTilesF4 - layersRawData.Fourth.Thickness;
      }
      else
      {
        if (fix32_7 >= Fix32.One)
        {
          addSplat(layersRawData.Fourth.SlimId, Fix32.One - fix32_6);
          return splatsCount;
        }
        thicknessTilesF5 = ThicknessTilesF.Zero;
        addSplat(layersRawData.Fourth.SlimId, weight3);
      }
      if (splatsCount == 4)
        return splatsCount;
      if (layersRawData.Count == 4)
      {
        addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One - fix32_7);
        return splatsCount;
      }
      int num = 4;
      int overflowIndex = layersRawData.OverflowIndex;
      while (splatsCount < 4 && num < layersRawData.Count)
      {
        TileMaterialLayerOverflow layerOverflowRawData = this.m_terrain.GetLayerOverflowRawData(overflowIndex);
        fix32_1 = layerOverflowRawData.Material.Thickness.Value - thicknessTilesF5.Value;
        Fix32 weight4 = fix32_1.Max(Fix32.Zero);
        Fix32 fix32_8 = fix32_7;
        fix32_7 += weight4;
        if (weight4 == Fix32.Zero)
        {
          thicknessTilesF5 -= layerOverflowRawData.Material.Thickness;
        }
        else
        {
          if (fix32_7 >= Fix32.One)
          {
            addSplat(layerOverflowRawData.Material.SlimId, Fix32.One - fix32_8);
            return splatsCount;
          }
          thicknessTilesF5 = ThicknessTilesF.Zero;
          addSplat(layerOverflowRawData.Material.SlimId, weight4);
        }
        if (splatsCount == 4)
          return splatsCount;
        if (layersRawData.Count == num)
        {
          addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One - fix32_7);
          return splatsCount;
        }
        ++num;
        overflowIndex = layerOverflowRawData.OverflowIndex;
      }
      if (splatsCount == 0)
        addSplat(this.m_terrain.Bedrock.SlimId, Fix32.One);
      return splatsCount;

      void addSplat(TerrainMaterialSlimId id, Fix32 weight)
      {
        for (int index = 0; index < splatsCount; ++index)
        {
          Pair<TerrainMaterialSlimId, Fix32> pair = data[index];
          if (pair.First == id)
          {
            data[index] = Pair.Create<TerrainMaterialSlimId, Fix32>(id, pair.Second + weight);
            return;
          }
        }
        data[splatsCount] = Pair.Create<TerrainMaterialSlimId, Fix32>(id, weight);
        ++splatsCount;
      }
    }

    internal byte GetTextureIndex(TerrainMaterialSlimId id)
    {
      return this.m_slimIdToTextureIndex[(int) id.Value];
    }

    /// <summary>
    /// Encodes splats data. WARNING: This potentially destroys the 4th data element.
    /// </summary>
    /// <remarks>
    /// Data is encoded to Color32 struct such that RGB have 3 material indices and alpha has first two weights
    /// (4 bits each), third weight is just <c>1 - w1 - w2</c>.
    /// </remarks>
    private Color32 encodeSplatsData(Pair<TerrainMaterialSlimId, Fix32>[] data, int count)
    {
      if (count <= 1)
        return new Color32(this.GetTextureIndex(data[0].First), (byte) 0, (byte) 0, (byte) 15);
      if (count == 2)
      {
        Pair<TerrainMaterialSlimId, Fix32> a = data[0];
        Pair<TerrainMaterialSlimId, Fix32> b = data[1];
        if (b.Second > a.Second)
          Swap.Them<Pair<TerrainMaterialSlimId, Fix32>>(ref a, ref b);
        int roundedNonNegative = (15 * a.Second).ToIntRoundedNonNegative();
        int num = 15 - roundedNonNegative;
        return new Color32(this.GetTextureIndex(a.First), this.GetTextureIndex(b.First), (byte) 0, (byte) (roundedNonNegative | num << 4));
      }
      if (count >= 4)
      {
        int index = 0;
        Fix32 second = data[0].Second;
        if (data[1].Second < second)
        {
          second = data[1].Second;
          index = 1;
        }
        if (data[2].Second < second)
        {
          second = data[2].Second;
          index = 2;
        }
        if (!(data[3].Second < second))
          data[index] = data[3];
      }
      Pair<TerrainMaterialSlimId, Fix32> a1 = data[0];
      Pair<TerrainMaterialSlimId, Fix32> b1 = data[1];
      Pair<TerrainMaterialSlimId, Fix32> b2 = data[2];
      if (b1.Second > a1.Second)
        Swap.Them<Pair<TerrainMaterialSlimId, Fix32>>(ref a1, ref b1);
      if (b2.Second > a1.Second)
        Swap.Them<Pair<TerrainMaterialSlimId, Fix32>>(ref a1, ref b2);
      int roundedNonNegative1 = (15 * a1.Second).ToIntRoundedNonNegative();
      int num1 = (15 * b1.Second).ToIntRoundedNonNegative();
      if (roundedNonNegative1 + num1 > 15)
        num1 = 15 - roundedNonNegative1;
      return new Color32(this.GetTextureIndex(a1.First), this.GetTextureIndex(b1.First), this.GetTextureIndex(b2.First), (byte) (roundedNonNegative1 | num1 << 4));
    }

    public void SetDecalPreviewData(
      RectangleTerrainArea2i area,
      Rotation90 rotation,
      bool flipped,
      TerrainTileSurfaceDecalProto proto,
      int colorKey)
    {
      Shader.SetGlobalVector(TerrainRenderer.DECAL_PREVIEW_AREA_SHADER_ID, new Vector4((float) area.Origin.X, (float) area.Origin.Y, (float) area.PlusXyCoordExcl.X, (float) area.PlusXyCoordExcl.Y));
      Shader.SetGlobalInt(TerrainRenderer.USING_SURFACE_PASTE, 0);
      Shader.SetGlobalInteger(TerrainRenderer.DECAL_PREVIEW_ROTATION_SHADER_ID, rotation.AngleIndex);
      Shader.SetGlobalInteger(TerrainRenderer.DECAL_PREVIEW_FLIPPED_SHADER_ID, flipped ? 1 : 0);
      Shader.SetGlobalInteger(TerrainRenderer.DECAL_PREVIEW_TEXTURE_SHADER_ID, (int) proto.SlimId.Value);
      Shader.SetGlobalInteger(TerrainRenderer.DECAL_COLOR_KEY_SHADER_ID, colorKey);
    }

    public void SetSurfacePastePreviewData(
      RectangleTerrainArea2i area,
      TileSurfaceData[] data,
      bool pushToGPU)
    {
      Shader.SetGlobalVector(TerrainRenderer.DECAL_PREVIEW_AREA_SHADER_ID, new Vector4((float) area.Origin.X, (float) area.Origin.Y, (float) area.PlusXyCoordExcl.X, (float) area.PlusXyCoordExcl.Y));
      Shader.SetGlobalInt(TerrainRenderer.USING_SURFACE_PASTE, 1);
      Shader.SetGlobalBuffer(TerrainRenderer.SURFACE_PASTE_PREVIEW_BUFFER, this.m_surfacePreviewDataGPU);
      if (!pushToGPU)
        return;
      if (data.Length < area.AreaTiles)
      {
        Log.Error("TileSurfaceData array is too small for the passed area.");
      }
      else
      {
        if (this.m_surfacePreviewDataGPU == null)
          this.m_surfacePreviewDataGPU = new ComputeBuffer(area.AreaTiles, 8);
        else if (this.m_surfacePreviewDataGPU.count < area.AreaTiles)
        {
          this.m_surfacePreviewDataGPU.Dispose();
          this.m_surfacePreviewDataGPU = new ComputeBuffer(area.AreaTiles, 8);
        }
        this.m_surfacePreviewDataGPU.SetData((Array) data, 0, 0, area.AreaTiles);
      }
    }

    public void DisableSurfacePastePreview()
    {
      Shader.SetGlobalInt(TerrainRenderer.USING_SURFACE_PASTE, 0);
      Shader.SetGlobalVector(TerrainRenderer.DECAL_PREVIEW_AREA_SHADER_ID, new Vector4(-1f, -1f, -1f, -1f));
    }

    static TerrainRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TerrainRenderer.s_allTilesDataPool = new ObjectPool<TerrainRenderer.TexUpdateDataSlim[]>(4, (Func<TerrainRenderer.TexUpdateDataSlim[]>) (() => new TerrainRenderer.TexUpdateDataSlim[65536]), (Action<TerrainRenderer.TexUpdateDataSlim[]>) (_ => { }));
      TerrainRenderer.GRID_SHADER_PROP_ID = Shader.PropertyToID("_TerrainGridEnabled");
      TerrainRenderer.TerrainQuality = Loc.Str(nameof (TerrainQuality), "Terrain quality", "rendering setting name");
      TerrainRenderer.TerrainQualityRenderSetting = new RenderingSetting(nameof (TerrainQualityRenderSetting), TerrainRenderer.TerrainQuality, 140, ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__High, 1, RenderingSettingPreset.MediumQuality | RenderingSettingPreset.HighQuality | RenderingSettingPreset.UltraQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Low, 0, RenderingSettingPreset.LowQuality)));
      TerrainRenderer.TerrainDetailsRenderDistance = Loc.Str(nameof (TerrainDetailsRenderDistance), "Terrain grass quality", "rendering setting name");
      TerrainRenderer.TerrainDetailsRenderDistanceSetting = new RenderingSetting(nameof (TerrainDetailsRenderDistance), TerrainRenderer.TerrainDetailsRenderDistance, 150, ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__High, 3, RenderingSettingPreset.UltraQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Medium, 2, RenderingSettingPreset.HighQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Low, 1, RenderingSettingPreset.MediumQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Off, 0, RenderingSettingPreset.LowQuality)));
      TerrainRenderer.DECAL_PREVIEW_AREA_SHADER_ID = Shader.PropertyToID("_Mafi_DecalPreviewArea");
      TerrainRenderer.DECAL_PREVIEW_ROTATION_SHADER_ID = Shader.PropertyToID("_Mafi_DecalPreviewRotation");
      TerrainRenderer.DECAL_PREVIEW_FLIPPED_SHADER_ID = Shader.PropertyToID("_Mafi_DecalPreviewFlipped");
      TerrainRenderer.DECAL_PREVIEW_TEXTURE_SHADER_ID = Shader.PropertyToID("_Mafi_DecalPreviewTextureId");
      TerrainRenderer.DECAL_COLOR_KEY_SHADER_ID = Shader.PropertyToID("_Mafi_DecalColorKey");
      TerrainRenderer.SURFACE_PASTE_PREVIEW_BUFFER = Shader.PropertyToID("_Mafi_SurfacePastePreviewBuffer");
      TerrainRenderer.USING_SURFACE_PASTE = Shader.PropertyToID("_Mafi_UsingSurfacePaste");
    }

    [ExpectedStructSize(12)]
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct TerrainChunkInstanceData
    {
      [FieldOffset(0)]
      public readonly Vector2 Origin;
      [FieldOffset(8)]
      public readonly float Scale;

      public TerrainChunkInstanceData(Vector2 origin, float scale)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Origin = origin;
        this.Scale = scale;
      }
    }

    [ExpectedStructSize(8)]
    private readonly struct RandomDetailOffset
    {
      /// <summary>
      /// Random signed byte value. Used to determine detail offset.
      /// </summary>
      public readonly sbyte Dx;
      /// <summary>
      /// Random signed byte value. Used to determine detail offset.
      /// </summary>
      public readonly sbyte Dy;
      /// <summary>Random ushort value. Used to determine variant index.</summary>
      public readonly ushort Variant;
      /// <summary>Random rotation (packed as cos and sin per byte).</summary>
      public readonly ushort RotCosSinPacked;
      /// <summary>Random ushort value. Used to determine detail scale.</summary>
      public readonly short Scale;

      public RandomDetailOffset(
        sbyte dx,
        sbyte dy,
        ushort variant,
        ushort rotCosSinPacked,
        short scale)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Dx = dx;
        this.Dy = dy;
        this.Variant = variant;
        this.RotCosSinPacked = rotCosSinPacked;
        this.Scale = scale;
      }

      /// <summary>
      /// Packs cos and sin of the given angle in radians to 1 byte each.
      /// </summary>
      public static ushort PackCosSin(float angleRad)
      {
        return (ushort) ((uint) ((1.0 + (double) MafiMath.Cos(angleRad)) * (double) sbyte.MaxValue) | (uint) ((1.0 + (double) MafiMath.Sin(angleRad)) * (double) sbyte.MaxValue) << 8);
      }
    }

    internal class TerrainRendererDebugMb : MonoBehaviour
    {
      public Material TerrainMaterial;
      public Texture2D HeightTexture;
      public Texture2D SplatTexture;
      public Texture2DArray DiffuseArray;
      public Texture2DArray NormalSaoArray;
      public Texture2DArray DiffuseSurfacesArray;
      public Texture2DArray NormalSurfacesArray;
      public Texture2DArray DecalsDiffuseArray;
      public Bounds TerrainBounds;
      private TerrainRenderer m_renderer;

      public void Initialize(TerrainRenderer renderer) => this.m_renderer = renderer;

      public void Update()
      {
        this.TerrainMaterial = this.m_renderer.m_terrainMaterial;
        this.HeightTexture = this.m_renderer.HeightTexture;
        this.SplatTexture = this.m_renderer.m_splatTexture;
        this.DiffuseArray = this.m_renderer.DiffuseArray;
        this.NormalSaoArray = this.m_renderer.NormalSaoArray;
        this.DiffuseSurfacesArray = this.m_renderer.m_terrainSurfaceTextureManager.DiffuseSurfacesArray;
        this.NormalSurfacesArray = this.m_renderer.m_terrainSurfaceTextureManager.NormalSurfacesArray;
        this.DecalsDiffuseArray = this.m_renderer.m_terrainSurfaceTextureManager.DiffuseSurfaceDecalsArray;
        this.TerrainBounds = this.m_renderer.m_terrainBounds;
      }

      public TerrainRendererDebugMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }

    private sealed class TerrainRendererChunk : 
      IRenderedChunk,
      IRenderedChunksBase,
      IDisposable,
      IReloadAfterAssetUpdate
    {
      private static readonly int SURFACE_DATA_SHADER_ID;
      [ThreadStatic]
      private static Pair<TerrainMaterialSlimId, Fix32>[] s_newSplatsTmp;
      private readonly Tile2i m_originTile;
      private readonly Tile2iIndex m_originTileIndex;
      private readonly TerrainRenderer m_parentRenderer;
      private readonly int m_randomDetailsOffset;
      private readonly Option<TerrainRenderer.DetailsLayerChunkRenderer>[] m_terrainDetails;
      private ImmutableArray<TerrainRenderer.DetailsLayerChunkRenderer> m_terrainDetailsNonEmpty;
      private bool m_terrainDetailsDisabled;
      private readonly BitMap m_changedTilesBitmap;
      private readonly BitMap m_changedNeighborSurfaceBitmap;
      private LystStruct<TileInChunk256> m_changedTiles;
      private bool m_allTilesChanged;
      private LystStruct<Pair<TileInChunk256, TileSurfaceData>> m_changedSurfaces;
      private LystStruct<Pair<TileInChunk256, TileSurfaceData>> m_changedSurfacesForUpdate;
      private LystStruct<TileInChunk256> m_changedNeighborSurfaceTiles;
      private LystStruct<Pair<TileInChunk256, byte>> m_changedNeighborSurfaces;
      private LystStruct<Pair<TileInChunk256, byte>> m_changedNeighborSurfacesForUpdate;
      private LystStruct<Pair<TileInChunk256, TerrainRenderer.TileSurfaceAreaData>> m_lastDecalPreview;
      private Option<InstancedMeshesRenderer<TerrainRenderer.TileSurfaceInstanceData>> m_surfacesRenderer;
      private uint[] m_surfacesInstances;
      private QuadTree<TerrainRenderer.TileSurfaceQuadTreeComparable> m_occupiedSurfaceQuadTree;
      private Option<TerrainRenderer.TerrainDetailLayerData[]> m_farDetailsLayer;
      private Option<TerrainRenderer.TopTwoDetailLayers[]> m_topTwoDetailLayers;
      private Option<TerrainRenderer.TileSurfaceAreaData[]> m_terrainSurfaceDataCPU;
      private Option<ComputeBuffer> m_terrainSurfaceDataGPU;
      private Texture2D m_heightTex;
      private Texture2D m_splatTex;
      private LystStruct<TerrainRenderer.TexUpdateData> m_heightSplatTexDataUpdates;
      private Option<TerrainRenderer.TexUpdateDataSlim[]> m_heightSplatTexDataUpdatesAllTiles;
      private IRenderedChunksParent m_chunkParent;
      private float m_minHeight;
      private float m_maxHeight;
      private Bounds m_bounds;
      private int m_currentLod;
      private int m_ticksSinceLastUpdate;
      private float m_minHeightOnSim;
      private float m_maxHeightOnSim;
      private volatile bool m_needsTerrainTexDataUpload;
      private volatile bool m_surfacesNeedDataUpdate;
      private volatile bool m_surfacesNeedNeighborDataUpdate;

      private static Pair<TerrainMaterialSlimId, Fix32>[] getSplatsTmp()
      {
        Pair<TerrainMaterialSlimId, Fix32>[] splatsTmp = TerrainRenderer.TerrainRendererChunk.s_newSplatsTmp;
        if (splatsTmp == null)
          TerrainRenderer.TerrainRendererChunk.s_newSplatsTmp = splatsTmp = new Pair<TerrainMaterialSlimId, Fix32>[4];
        return splatsTmp;
      }

      public string Name => "Terrain";

      public Vector2 Origin { get; }

      public Chunk256AndIndex CoordAndIndex { get; }

      public bool TrackStoppedRendering => true;

      public float MaxModelDeviationFromChunkBounds => 1f;

      public Vector2 MinMaxHeight => new Vector2(this.m_minHeight, this.m_maxHeight);

      public TerrainRendererChunk(
        Chunk256AndIndex coordAndIndex,
        TerrainRenderer parentRenderer,
        int randomDetailsOffset)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.m_terrainDetailsNonEmpty = ImmutableArray<TerrainRenderer.DetailsLayerChunkRenderer>.Empty;
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.CoordAndIndex = coordAndIndex;
        this.m_originTile = coordAndIndex.OriginTile2i;
        this.m_originTileIndex = parentRenderer.m_terrain.GetTileIndex(this.m_originTile);
        this.Origin = this.m_originTile.ToVector2();
        this.m_parentRenderer = parentRenderer;
        this.m_randomDetailsOffset = randomDetailsOffset;
        this.m_terrainDetails = new Option<TerrainRenderer.DetailsLayerChunkRenderer>[parentRenderer.m_detailLayers.Length];
        this.m_changedTilesBitmap = new BitMap(65536);
        this.m_changedNeighborSurfaceBitmap = new BitMap(65536);
      }

      public void InitializeData()
      {
        TerrainManager terrain = this.m_parentRenderer.m_terrain;
        Pair<TerrainMaterialSlimId, Fix32>[] splatsTmp = TerrainRenderer.TerrainRendererChunk.getSplatsTmp();
        float num1 = float.MaxValue;
        float num2 = float.MinValue;
        this.m_terrainDetailsDisabled = (double) this.m_parentRenderer.m_globalDetailDensity <= 0.0;
        NativeArray<float> rawTextureData1 = this.m_parentRenderer.HeightTexture.GetRawTextureData<float>();
        NativeArray<Color32> rawTextureData2 = this.m_parentRenderer.m_splatTexture.GetRawTextureData<Color32>();
        int num3 = terrain.GetTileIndex(this.m_originTile).Value;
        for (int index1 = 0; index1 < 65536; ++index1)
        {
          TileInChunk256 tileInChunk256 = new TileInChunk256((ushort) index1);
          Tile2iIndex tileIndex = terrain.GetTileIndex(this.m_originTile + tileInChunk256.AsTileOffset);
          HeightTilesF height = terrain.GetHeight(tileIndex);
          float unityUnits = height.ToUnityUnits();
          num1 = unityUnits.Min(num1);
          num2 = unityUnits.Max(num2);
          int index2 = num3 + (int) tileInChunk256.X + (int) tileInChunk256.Y * terrain.TerrainWidth;
          rawTextureData1[index2] = unityUnits;
          int num4 = this.m_parentRenderer.collectSplatData(tileIndex, splatsTmp);
          if (!this.m_terrainDetailsDisabled && this.canHaveDetails(tileIndex, height))
            this.updateTerrainDetails(tileInChunk256, splatsTmp, num4);
          rawTextureData2[index2] = this.m_parentRenderer.encodeSplatsData(splatsTmp, num4);
          TileSurfaceData tileSurface = terrain.GetTileSurface(tileIndex);
          if (tileSurface.IsValid)
            this.m_changedSurfacesForUpdate.Add(Pair.Create<TileInChunk256, TileSurfaceData>(tileInChunk256, tileSurface));
          this.TileNeighborSurfaceChanged(tileInChunk256);
        }
        this.m_minHeight = this.m_minHeightOnSim = num1 - 2f;
        this.m_maxHeight = this.m_maxHeightOnSim = num2 + 2f;
        this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
        this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        this.m_parentRenderer.updateBounds(this.m_minHeight, this.m_maxHeight);
        if (this.m_changedSurfacesForUpdate.IsNotEmpty)
        {
          bool surfaceDataDirty = false;
          this.processSurfaceChanges(ref surfaceDataDirty);
          if (surfaceDataDirty)
            this.m_terrainSurfaceDataGPU.Value.SetData((Array) this.m_terrainSurfaceDataCPU.Value);
          this.m_changedSurfacesForUpdate = new LystStruct<Pair<TileInChunk256, TileSurfaceData>>();
        }
        foreach (TerrainRenderer.DetailsLayerChunkRenderer layerChunkRenderer in this.m_terrainDetailsNonEmpty)
          layerChunkRenderer.UpdateRenderBuffers();
      }

      public void Dispose()
      {
        this.m_parentRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<TerrainRenderer.TileSurfaceInstanceData>>(this.m_surfacesRenderer.ValueOrNull);
        this.m_surfacesRenderer = (Option<InstancedMeshesRenderer<TerrainRenderer.TileSurfaceInstanceData>>) Option.None;
        this.m_parentRenderer.m_reloadManager.TryUnregister((IReloadAfterAssetUpdate) this);
        foreach (Option<TerrainRenderer.DetailsLayerChunkRenderer> terrainDetail in this.m_terrainDetails)
          terrainDetail.ValueOrNull?.Dispose();
        this.m_terrainSurfaceDataGPU.ValueOrNull?.Dispose();
        this.m_terrainSurfaceDataGPU = Option<ComputeBuffer>.None;
      }

      void IReloadAfterAssetUpdate.ReloadAfterAssetUpdate()
      {
        if (!this.m_surfacesRenderer.HasValue || !this.m_terrainSurfaceDataGPU.HasValue)
          return;
        foreach (Material material in this.m_surfacesRenderer.Value.Materials)
          material.SetBuffer(TerrainRenderer.TerrainRendererChunk.SURFACE_DATA_SHADER_ID, this.m_terrainSurfaceDataGPU.Value);
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
      {
        if (this.m_surfacesRenderer.HasValue)
          info.Add(new RenderedInstancesInfo("Terrain surfaces", this.m_surfacesRenderer.Value.InstancesCount, this.m_surfacesRenderer.Value.IndicesCountForLod0));
        foreach (Option<TerrainRenderer.DetailsLayerChunkRenderer> terrainDetail in this.m_terrainDetails)
          terrainDetail.ValueOrNull?.ReportAllRenderedInstances(info);
      }

      public void ReloadDetailTextures()
      {
        foreach (Option<TerrainRenderer.DetailsLayerChunkRenderer> terrainDetail in this.m_terrainDetails)
          terrainDetail.ValueOrNull?.ReloadDetailTextures();
      }

      void IRenderedChunk.Register(IRenderedChunksParent parent) => this.m_chunkParent = parent;

      void IRenderedChunk.NotifyWasNotRendered() => this.m_currentLod = 7;

      RenderStats IRenderedChunk.Render(
        GameTime time,
        float cameraDistance,
        int lod,
        float pxPerMeter)
      {
        this.m_currentLod = lod;
        RenderStats renderStats = new RenderStats();
        if ((double) pxPerMeter >= 1.5)
        {
          float y = this.Origin.y;
          int num1 = 0;
          while (num1 < 4)
          {
            int num2 = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(new Vector2(this.Origin.x, y), 1f));
            int num3 = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(new Vector2(this.Origin.x + 128f, y), 1f));
            int num4 = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(new Vector2(this.Origin.x + 256f, y), 1f));
            int num5 = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(new Vector2(this.Origin.x + 384f, y), 1f));
            ++num1;
            y += 128f;
          }
        }
        else if ((double) pxPerMeter >= 0.5)
        {
          int num6 = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(this.Origin, 2f));
          int num7 = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(new Vector2(this.Origin.x + 256f, this.Origin.y), 2f));
          int num8 = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(new Vector2(this.Origin.x, this.Origin.y + 256f), 2f));
          int num9 = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(new Vector2(this.Origin.x + 256f, this.Origin.y + 256f), 2f));
        }
        else
        {
          int num = (int) this.m_parentRenderer.m_instancedTerrainRenderer.AddInstance(new TerrainRenderer.TerrainChunkInstanceData(this.Origin, 4f));
        }
        if ((double) pxPerMeter >= (double) this.m_parentRenderer.m_farDetailsPpmThreshold)
        {
          foreach (TerrainRenderer.DetailsLayerChunkRenderer layerChunkRenderer in this.m_terrainDetailsNonEmpty)
            renderStats += layerChunkRenderer.Render(this.m_bounds, pxPerMeter);
        }
        if (this.m_surfacesRenderer.HasValue)
          renderStats += this.m_surfacesRenderer.Value.Render(this.m_bounds, lod);
        return renderStats;
      }

      public bool RenderUpdate()
      {
        bool flag = false;
        if (this.m_needsTerrainTexDataUpload)
        {
          this.uploadTerrainTexDataAndDetails();
          this.m_needsTerrainTexDataUpload = false;
          flag = true;
        }
        bool surfaceDataDirty = false;
        if (this.m_surfacesNeedDataUpdate)
        {
          this.processSurfaceChanges(ref surfaceDataDirty);
          this.m_surfacesNeedDataUpdate = false;
          flag = true;
        }
        if (this.m_surfacesNeedNeighborDataUpdate)
        {
          this.processNeighborSurfaceChanges(ref surfaceDataDirty);
          this.m_surfacesNeedNeighborDataUpdate = false;
          flag = true;
        }
        if (surfaceDataDirty)
          this.m_terrainSurfaceDataGPU.Value.SetData((Array) this.m_terrainSurfaceDataCPU.Value);
        return flag;
      }

      public int ProcessChangesOnSim()
      {
        int num = 0;
        if (this.m_changedTiles.IsNotEmpty || this.m_allTilesChanged)
        {
          ++this.m_ticksSinceLastUpdate;
          if ((this.m_ticksSinceLastUpdate >= (this.m_allTilesChanged ? 2 : this.m_parentRenderer.UpdateDelayPerLod[this.m_currentLod]) || this.m_parentRenderer.m_usingXRay) && !this.m_needsTerrainTexDataUpload)
          {
            this.m_ticksSinceLastUpdate = 0;
            num += this.computeDataUpdateOnSim(this.m_changedTiles);
            this.m_changedTiles.ClearSkipZeroingMemory();
            this.m_needsTerrainTexDataUpload = true;
          }
        }
        if (this.m_changedSurfaces.IsNotEmpty && !this.m_surfacesNeedDataUpdate)
        {
          Swap.Them<LystStruct<Pair<TileInChunk256, TileSurfaceData>>>(ref this.m_changedSurfaces, ref this.m_changedSurfacesForUpdate);
          this.m_surfacesNeedDataUpdate = true;
        }
        if (this.m_changedNeighborSurfaceTiles.IsNotEmpty)
        {
          int terrainWidth = this.m_parentRenderer.m_terrain.TerrainWidth;
          foreach (TileInChunk256 neighborSurfaceTile in this.m_changedNeighborSurfaceTiles)
          {
            Tile2iIndex tileIndex = this.m_parentRenderer.m_terrain.GetTileIndex(this.m_originTile + neighborSurfaceTile.AsTileOffset);
            TileSurfaceData mySurfaceData;
            if (this.m_parentRenderer.m_terrain.TryGetTileSurface(tileIndex, out mySurfaceData))
            {
              TerrainTileSurfaceProto mySurfaceProto = mySurfaceData.SurfaceSlimId.AsProtoOrPhantom(this.m_parentRenderer.m_terrain);
              byte surfaceChangedInDirection = 0;
              checkSurfaceIsDifferent(tileIndex.PlusXNeighborUnchecked, 0);
              checkSurfaceIsDifferent(tileIndex.PlusXPlusYNeighborUnchecked(terrainWidth), 1);
              checkSurfaceIsDifferent(tileIndex.PlusYNeighborUnchecked(terrainWidth), 2);
              checkSurfaceIsDifferent(tileIndex.MinusXPlusYNeighborUnchecked(terrainWidth), 3);
              checkSurfaceIsDifferent(tileIndex.MinusXNeighborUnchecked, 4);
              checkSurfaceIsDifferent(tileIndex.MinusXMinusYNeighborUnchecked(terrainWidth), 5);
              checkSurfaceIsDifferent(tileIndex.MinusYNeighborUnchecked(terrainWidth), 6);
              checkSurfaceIsDifferent(tileIndex.PlusXMinusYNeighborUnchecked(terrainWidth), 7);
              this.m_changedNeighborSurfaces.Add(new Pair<TileInChunk256, byte>(neighborSurfaceTile, surfaceChangedInDirection));

              void checkSurfaceIsDifferent(Tile2iIndex tileIndex, int bitToSet)
              {
                TileSurfaceData tileSurfaceData;
                if (this.m_parentRenderer.m_terrain.TryGetTileSurface(tileIndex, out tileSurfaceData) && tileSurfaceData.Height - mySurfaceData.Height < ThicknessTilesF.Half)
                {
                  if (mySurfaceData.SurfaceSlimId == tileSurfaceData.SurfaceSlimId)
                    return;
                  TerrainTileSurfaceProto other = tileSurfaceData.SurfaceSlimId.AsProtoOrPhantom(this.m_parentRenderer.m_terrain);
                  if (mySurfaceProto.CanMergeGraphicsWith(other))
                    return;
                }
                surfaceChangedInDirection |= (byte) (1 << bitToSet);
              }
            }
          }
          if (!this.m_surfacesNeedNeighborDataUpdate)
          {
            Swap.Them<LystStruct<Pair<TileInChunk256, byte>>>(ref this.m_changedNeighborSurfaces, ref this.m_changedNeighborSurfacesForUpdate);
            this.m_surfacesNeedNeighborDataUpdate = true;
            this.m_changedNeighborSurfaceBitmap.ClearAllBits();
            this.m_changedNeighborSurfaceTiles.Clear();
          }
        }
        return num;
      }

      public void TileChanged(TileInChunk256 localIndex)
      {
        if (!this.m_changedTilesBitmap.SetBitReportChanged((int) localIndex.Index))
          return;
        this.m_changedTiles.Add(localIndex);
      }

      public void TileSurfaceChanged(TileInChunk256 localIndex, TileSurfaceData surfaceData)
      {
        this.TileChanged(localIndex);
        this.m_changedSurfaces.Add(Pair.Create<TileInChunk256, TileSurfaceData>(localIndex, surfaceData));
        this.TileNeighborSurfaceChanged(localIndex);
      }

      public void TileNeighborSurfaceChanged(TileInChunk256 localIndex)
      {
        if (!this.m_changedNeighborSurfaceBitmap.SetBitReportChanged((int) localIndex.Index))
          return;
        this.m_changedNeighborSurfaceTiles.Add(localIndex);
      }

      public void UpdateTerrainDetailsRenderingSettings(bool regenerateAllDetails)
      {
        if (!regenerateAllDetails)
        {
          foreach (TerrainRenderer.DetailsLayerChunkRenderer layerChunkRenderer in this.m_terrainDetailsNonEmpty)
            layerChunkRenderer.UpdateRenderDistance();
        }
        else
        {
          for (int index = 0; index < this.m_terrainDetails.Length; ++index)
          {
            this.m_terrainDetails[index].ValueOrNull?.Dispose();
            this.m_terrainDetails[index] = Option<TerrainRenderer.DetailsLayerChunkRenderer>.None;
          }
          this.m_terrainDetailsNonEmpty = ImmutableArray<TerrainRenderer.DetailsLayerChunkRenderer>.Empty;
          this.m_farDetailsLayer = Option<TerrainRenderer.TerrainDetailLayerData[]>.None;
          this.m_topTwoDetailLayers = Option<TerrainRenderer.TopTwoDetailLayers[]>.None;
          this.m_terrainDetailsDisabled = (double) this.m_parentRenderer.m_globalDetailDensity <= 0.0;
          if (this.m_terrainDetailsDisabled)
            return;
          TerrainManager terrain = this.m_parentRenderer.m_terrain;
          Pair<TerrainMaterialSlimId, Fix32>[] splatsTmp = TerrainRenderer.TerrainRendererChunk.getSplatsTmp();
          for (int index = 0; index < 65536; ++index)
          {
            TileInChunk256 tileLocalIndex = new TileInChunk256((ushort) index);
            Tile2iIndex tileIndex = terrain.GetTileIndex(this.m_originTile + tileLocalIndex.AsTileOffset);
            int materialsCount = this.m_parentRenderer.collectSplatData(tileIndex, splatsTmp);
            if (this.canHaveDetails(tileIndex, terrain.GetHeight(tileIndex)))
              this.updateTerrainDetails(tileLocalIndex, splatsTmp, materialsCount);
          }
          foreach (TerrainRenderer.DetailsLayerChunkRenderer layerChunkRenderer in this.m_terrainDetailsNonEmpty)
            layerChunkRenderer.UpdateRenderBuffers();
        }
      }

      public void AllTilesChanged() => this.m_allTilesChanged = true;

      private int computeDataUpdateOnSim(LystStruct<TileInChunk256> changedTiles)
      {
        TerrainManager terrainManager = this.m_parentRenderer.m_terrain;
        Pair<TerrainMaterialSlimId, Fix32>[] splatsTmp = TerrainRenderer.TerrainRendererChunk.getSplatsTmp();
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;
        int terrainWidth = this.m_parentRenderer.m_terrain.TerrainWidth;
        if (this.m_parentRenderer.m_usingXRay)
        {
          long xRayRadiusSquared = this.m_parentRenderer.m_xRayRadius.Squared;
          HeightTilesF xRayBottomHeight = terrainManager.GetHeight(this.m_parentRenderer.m_xRayPosition) + (this.m_parentRenderer.m_xRayDepthOffset - XRayTool.TOOL_BASE_DEPTH).ThicknessTilesF;
          if (this.m_allTilesChanged)
          {
            this.m_allTilesChanged = false;
            this.m_changedTilesBitmap.ClearAllBits();
            TerrainRenderer.TexUpdateDataSlim[] tilesChunkDataPooled = this.m_parentRenderer.getAllTilesChunkDataPooled();
            this.m_heightSplatTexDataUpdatesAllTiles = (Option<TerrainRenderer.TexUpdateDataSlim[]>) tilesChunkDataPooled;
            int y = 0;
            int num1 = 0;
            int num2 = this.m_originTileIndex.Value;
            while (y < 256)
            {
              for (int x = 0; x < 256; ++x)
              {
                float heightF;
                Color32 encodedSplatsData;
                computeUpdate(new Tile2iIndex(num2 + x), new TileInChunk256((byte) x, (byte) y), out heightF, out encodedSplatsData);
                tilesChunkDataPooled[num1 + x] = new TerrainRenderer.TexUpdateDataSlim(heightF, encodedSplatsData);
              }
              ++y;
              num1 += 256;
              num2 += terrainWidth;
            }
            this.m_minHeightOnSim = minHeight;
            this.m_maxHeightOnSim = maxHeight;
            return 65536;
          }
          this.m_heightSplatTexDataUpdates.EnsureCapacity(changedTiles.Count);
          foreach (TileInChunk256 changedTile in changedTiles)
          {
            float heightF;
            Color32 encodedSplatsData;
            computeUpdate(this.m_originTileIndex + (int) changedTile.Y * terrainWidth + (int) changedTile.X, changedTile, out heightF, out encodedSplatsData);
            this.m_heightSplatTexDataUpdates.Add(new TerrainRenderer.TexUpdateData((int) changedTile.Index, heightF, encodedSplatsData));
            this.m_changedTilesBitmap.ClearBitsAround((int) changedTile.Index);
          }

          void computeUpdate(
            Tile2iIndex tileIndex,
            TileInChunk256 tileInChunk,
            out float heightF,
            out Color32 encodedSplatsData)
          {
            long num1 = terrainManager.IndexToTile_Slow(tileIndex).DistanceSqrTo(this.m_parentRenderer.m_xRayPosition);
            HeightTilesF height1;
            int num2;
            if (num1 <= xRayRadiusSquared)
            {
              HeightTilesF height2 = terrainManager.GetHeight(tileIndex);
              HeightTilesF xRayBottomHeight = xRayBottomHeight;
              if (height2 > xRayBottomHeight)
              {
                Fix32 t = (4 * ((Fix32) this.m_parentRenderer.m_xRayRadius.Value - num1.SqrtToFix32()) / this.m_parentRenderer.m_xRayRadius.Value).Clamp01();
                height1 = height2.Lerp(xRayBottomHeight, t);
                num2 = this.m_parentRenderer.collectSplatDataWithDeltaHeight(tileIndex, height2 - height1, splatsTmp);
              }
              else
              {
                height1 = height2;
                num2 = this.m_parentRenderer.collectSplatData(tileIndex, splatsTmp);
              }
              this.ensureNoCloseDetails(tileInChunk);
              this.ensureNoFarDetails(tileInChunk);
            }
            else
            {
              height1 = terrainManager.GetHeight(tileIndex);
              num2 = this.m_parentRenderer.collectSplatData(tileIndex, splatsTmp);
              if (!this.m_terrainDetailsDisabled)
              {
                if (this.canHaveDetails(tileIndex, height1))
                {
                  this.updateTerrainDetails(tileInChunk, splatsTmp, num2);
                }
                else
                {
                  this.ensureNoCloseDetails(tileInChunk);
                  this.ensureNoFarDetails(tileInChunk);
                }
              }
            }
            heightF = height1.ToUnityUnits();
            minHeight = heightF.Min(minHeight);
            maxHeight = heightF.Max(maxHeight);
            encodedSplatsData = this.m_parentRenderer.encodeSplatsData(splatsTmp, num2);
          }
        }
        else
        {
          if (this.m_allTilesChanged)
          {
            this.m_allTilesChanged = false;
            this.m_changedTilesBitmap.ClearAllBits();
            TerrainRenderer.TexUpdateDataSlim[] tilesChunkDataPooled = this.m_parentRenderer.getAllTilesChunkDataPooled();
            this.m_heightSplatTexDataUpdatesAllTiles = (Option<TerrainRenderer.TexUpdateDataSlim[]>) tilesChunkDataPooled;
            int y = 0;
            int num3 = 0;
            int num4 = this.m_originTileIndex.Value;
            while (y < 256)
            {
              for (int x = 0; x < 256; ++x)
              {
                float heightF;
                Color32 encodedSplatsData;
                computeUpdate(new Tile2iIndex(num4 + x), new TileInChunk256((byte) x, (byte) y), out heightF, out encodedSplatsData);
                tilesChunkDataPooled[num3 + x] = new TerrainRenderer.TexUpdateDataSlim(heightF, encodedSplatsData);
              }
              ++y;
              num3 += 256;
              num4 += terrainWidth;
            }
            this.m_minHeightOnSim = minHeight;
            this.m_maxHeightOnSim = maxHeight;
            return 65536;
          }
          this.m_heightSplatTexDataUpdates.EnsureCapacity(changedTiles.Count);
          foreach (TileInChunk256 changedTile in changedTiles)
          {
            float heightF;
            Color32 encodedSplatsData;
            computeUpdate(this.m_originTileIndex + (int) changedTile.Y * terrainWidth + (int) changedTile.X, changedTile, out heightF, out encodedSplatsData);
            this.m_heightSplatTexDataUpdates.Add(new TerrainRenderer.TexUpdateData((int) changedTile.Index, heightF, encodedSplatsData));
            this.m_changedTilesBitmap.ClearBitsAround((int) changedTile.Index);
          }

          void computeUpdate(
            Tile2iIndex tileIndex,
            TileInChunk256 tileInChunk,
            out float heightF,
            out Color32 encodedSplatsData)
          {
            HeightTilesF height = terrainManager.GetHeight(tileIndex);
            heightF = height.ToUnityUnits();
            minHeight = heightF.Min(minHeight);
            maxHeight = heightF.Max(maxHeight);
            int num = this.m_parentRenderer.collectSplatData(tileIndex, splatsTmp);
            encodedSplatsData = this.m_parentRenderer.encodeSplatsData(splatsTmp, num);
            if (this.m_terrainDetailsDisabled)
              return;
            if (this.canHaveDetails(tileIndex, height))
            {
              this.updateTerrainDetails(tileInChunk, splatsTmp, num);
            }
            else
            {
              this.ensureNoCloseDetails(tileInChunk);
              this.ensureNoFarDetails(tileInChunk);
            }
          }
        }
        this.m_minHeightOnSim = minHeight;
        this.m_maxHeightOnSim = maxHeight;
        if (!((UnityEngine.Object) this.m_heightTex == (UnityEngine.Object) null))
          return this.m_heightSplatTexDataUpdates.Count;
        this.m_heightSplatTexDataUpdates.ClearSkipZeroingMemory();
        TerrainRenderer.TexUpdateDataSlim[] tilesChunkDataPooled1 = this.m_parentRenderer.getAllTilesChunkDataPooled();
        this.m_heightSplatTexDataUpdatesAllTiles = (Option<TerrainRenderer.TexUpdateDataSlim[]>) tilesChunkDataPooled1;
        int num5 = 0;
        int num6 = 0;
        int num7 = this.m_originTileIndex.Value;
        while (num5 < 256)
        {
          for (int index1 = 0; index1 < 256; ++index1)
          {
            Tile2iIndex index2 = new Tile2iIndex(num7 + index1);
            int count = this.m_parentRenderer.collectSplatData(index2, splatsTmp);
            tilesChunkDataPooled1[num6 + index1] = new TerrainRenderer.TexUpdateDataSlim(terrainManager.GetHeight(index2).ToUnityUnits(), this.m_parentRenderer.encodeSplatsData(splatsTmp, count));
          }
          ++num5;
          num6 += 256;
          num7 += terrainWidth;
        }
        return 65536;
      }

      private void uploadTerrainTexDataAndDetails()
      {
        if ((UnityEngine.Object) this.m_heightTex == (UnityEngine.Object) null)
        {
          Texture2D texture2D1 = new Texture2D(256, 256, this.m_parentRenderer.HeightTexture.format, false, true);
          texture2D1.name = "TerrainHeightUpdate";
          texture2D1.anisoLevel = 0;
          this.m_heightTex = texture2D1;
          Texture2D texture2D2 = new Texture2D(256, 256, this.m_parentRenderer.m_splatTexture.format, false, true);
          texture2D2.name = "TerrainSplatUpdate";
          texture2D2.anisoLevel = 0;
          this.m_splatTex = texture2D2;
        }
        NativeArray<float> rawTextureData1 = this.m_heightTex.GetRawTextureData<float>();
        NativeArray<Color32> rawTextureData2 = this.m_splatTex.GetRawTextureData<Color32>();
        TerrainRenderer.TexUpdateDataSlim[] valueOrNull = this.m_heightSplatTexDataUpdatesAllTiles.ValueOrNull;
        if (valueOrNull != null)
        {
          this.m_heightSplatTexDataUpdatesAllTiles = Option<TerrainRenderer.TexUpdateDataSlim[]>.None;
          for (int index = 0; index < valueOrNull.Length; ++index)
          {
            TerrainRenderer.TexUpdateDataSlim texUpdateDataSlim = valueOrNull[index];
            rawTextureData1[index] = texUpdateDataSlim.Height;
            rawTextureData2[index] = texUpdateDataSlim.Splats;
          }
          this.m_parentRenderer.returnAllTilesChunkDataToPool(valueOrNull);
        }
        foreach (TerrainRenderer.TexUpdateData splatTexDataUpdate in this.m_heightSplatTexDataUpdates)
        {
          rawTextureData1[splatTexDataUpdate.Index] = splatTexDataUpdate.Height;
          rawTextureData2[splatTexDataUpdate.Index] = splatTexDataUpdate.Splats;
        }
        this.m_heightSplatTexDataUpdates.ClearSkipZeroingMemory();
        this.m_heightTex.Apply(false, false);
        this.m_splatTex.Apply(false, false);
        Graphics.CopyTexture((Texture) this.m_heightTex, 0, 0, 0, 0, 256, 256, (Texture) this.m_parentRenderer.HeightTexture, 0, 0, this.m_originTile.X, this.m_originTile.Y);
        Graphics.CopyTexture((Texture) this.m_splatTex, 0, 0, 0, 0, 256, 256, (Texture) this.m_parentRenderer.m_splatTexture, 0, 0, this.m_originTile.X, this.m_originTile.Y);
        if ((double) this.m_minHeightOnSim < (double) this.m_minHeight || (double) this.m_maxHeightOnSim > (double) this.m_maxHeight)
        {
          this.m_minHeight = this.m_minHeightOnSim - 2f;
          this.m_maxHeight = this.m_maxHeightOnSim + 2f;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
          this.m_parentRenderer.updateBounds(this.m_minHeight, this.m_maxHeight);
        }
        foreach (TerrainRenderer.DetailsLayerChunkRenderer layerChunkRenderer in this.m_terrainDetailsNonEmpty)
          layerChunkRenderer.UpdateRenderBuffers();
      }

      public void ClearPreviewDecal()
      {
        if (this.m_terrainSurfaceDataCPU.IsNone)
        {
          Log.Error("m_terrainSurfaceDataCPU not yet allocated.");
          this.m_lastDecalPreview.Clear();
        }
        else
        {
          if (!this.m_lastDecalPreview.IsNotEmpty)
            return;
          foreach (Pair<TileInChunk256, TerrainRenderer.TileSurfaceAreaData> pair in this.m_lastDecalPreview)
            this.m_terrainSurfaceDataCPU.Value[(int) pair.First.Y * 256 + (int) pair.First.X] = pair.Second;
          this.m_lastDecalPreview.Clear();
        }
      }

      public void PreviewDecal(
        IReadOnlyCollection<Pair<TileInChunk256, TileSurfaceData>> decalData)
      {
        if (this.m_terrainSurfaceDataCPU.IsNone)
        {
          Log.Error("m_terrainSurfaceDataCPU not yet allocated.");
          this.m_lastDecalPreview.Clear();
        }
        else
        {
          this.ClearPreviewDecal();
          Log.Error("Decal previews not implemented");
        }
      }

      private void processSurfaceChanges(ref bool surfaceDataDirty)
      {
        uint[] existingSurfaces = this.m_surfacesInstances;
        QuadTree<TerrainRenderer.TileSurfaceQuadTreeComparable> occupiedMap = this.m_occupiedSurfaceQuadTree;
        if (existingSurfaces == null)
        {
          existingSurfaces = new uint[65536];
          for (int index = 0; index < existingSurfaces.Length; ++index)
            existingSurfaces[index] = uint.MaxValue;
          this.m_surfacesInstances = existingSurfaces;
        }
        if (this.m_terrainSurfaceDataCPU.IsNone)
        {
          this.m_terrainSurfaceDataCPU = (Option<TerrainRenderer.TileSurfaceAreaData[]>) new TerrainRenderer.TileSurfaceAreaData[65536];
          this.m_terrainSurfaceDataGPU.ValueOrNull?.Dispose();
          this.m_terrainSurfaceDataGPU = (Option<ComputeBuffer>) new ComputeBuffer(65536, 4);
        }
        if (occupiedMap.Size != (ushort) 256)
        {
          Assert.That<int>((int) occupiedMap.Size).IsZero();
          occupiedMap = new QuadTree<TerrainRenderer.TileSurfaceQuadTreeComparable>((ushort) 256);
          this.m_occupiedSurfaceQuadTree = occupiedMap;
        }
        InstancedMeshesRenderer<TerrainRenderer.TileSurfaceInstanceData> renderer = this.m_surfacesRenderer.ValueOrNull;
        if (renderer == null)
        {
          Material nonSharedMaterial = UnityEngine.Object.Instantiate<Material>(this.m_parentRenderer.m_tileSurfaceMaterialShared);
          nonSharedMaterial.SetBuffer(TerrainRenderer.TerrainRendererChunk.SURFACE_DATA_SHADER_ID, this.m_terrainSurfaceDataGPU.Value);
          this.m_surfacesRenderer = (Option<InstancedMeshesRenderer<TerrainRenderer.TileSurfaceInstanceData>>) (renderer = new InstancedMeshesRenderer<TerrainRenderer.TileSurfaceInstanceData>(this.m_parentRenderer.m_tileSurfaceMeshShared, nonSharedMaterial, shadowCastingMode: ShadowCastingMode.Off));
          this.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) renderer);
          this.m_parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) this);
        }
        foreach (Pair<TileInChunk256, TileSurfaceData> pair1 in this.m_changedSurfacesForUpdate)
        {
          Pair<TileInChunk256, TileSurfaceData> pair = pair1;
          byte x = pair.First.X;
          byte y = pair.First.Y;
          int lastLevel = occupiedMap.IsSet(x, y) ? occupiedMap.GetHighestSetLevel(x, y) : 0;
          if (pair.Second.IsValid)
          {
            TileSurfaceData second = pair.Second;
            this.m_terrainSurfaceDataCPU.Value[(int) y * 256 + (int) x] = this.m_terrainSurfaceDataCPU.Value[(int) y * 256 + (int) x].WithNewSurfaceData(second.SurfaceSlimId, second.TextureRotation.ToRotation(), second.DecalSlimId, second.IsDecalFlipped, second.ColorKey, second.DecalRotation.ToRotation());
            surfaceDataDirty = true;
            TerrainRenderer.TileSurfaceQuadTreeComparable other = new TerrainRenderer.TileSurfaceQuadTreeComparable(pair.Second);
            if (lastLevel != 0 && (pair.Second.IsRamp || occupiedMap.IsSet(x, y) && !occupiedMap.GetComparableAt(x, y).CanMergeQuadTreeWith(other)))
            {
              clearSurface();
              lastLevel = 0;
            }
            occupiedMap.SetAt(x, y, other);
            int highestSetLevel = occupiedMap.GetHighestSetLevel(x, y);
            for (int index1 = lastLevel + 1; index1 <= highestSetLevel; ++index1)
            {
              int num1 = 1 << index1;
              int num2 = 1 << index1 - 1;
              byte num3 = (byte) ((uint) x - ((uint) x & (uint) (num1 - 1)));
              byte num4 = (byte) ((uint) y - ((uint) y & (uint) (num1 - 1)));
              for (int index2 = 0; index2 <= 1; ++index2)
              {
                for (int index3 = 0; index3 <= 1; ++index3)
                {
                  TileInChunk256 tileInChunk256 = new TileInChunk256((byte) ((uint) num3 + (uint) (index3 * num2)), (byte) ((uint) num4 + (uint) (index2 * num2)));
                  uint index4 = existingSurfaces[(int) tileInChunk256.Index];
                  if (index4 < uint.MaxValue)
                  {
                    renderer.RemoveInstance((ushort) index4);
                    existingSurfaces[(int) tileInChunk256.Index] = uint.MaxValue;
                  }
                }
              }
            }
            int scale = 1 << highestSetLevel;
            TileInChunk256 tileInChunk256_1 = new TileInChunk256((byte) ((uint) x - ((uint) x & (uint) (scale - 1))), (byte) ((uint) y - ((uint) y & (uint) (scale - 1))));
            uint index = existingSurfaces[(int) tileInChunk256_1.Index];
            TerrainRenderer.TileSurfaceInstanceData surfaceInstanceData = new TerrainRenderer.TileSurfaceInstanceData(this.Origin + tileInChunk256_1.AsTileOffset.ToUnityUnits(), pair.Second, scale);
            if (index < uint.MaxValue)
              renderer.UpdateInstance((ushort) index, surfaceInstanceData);
            else
              existingSurfaces[(int) tileInChunk256_1.Index] = (uint) renderer.AddInstance(surfaceInstanceData);
          }
          else
            clearSurface();

          void clearSurface()
          {
            occupiedMap.ClearAt(x, y);
            int num1 = 1 << lastLevel;
            TileInChunk256 tileInChunk256_1 = new TileInChunk256((byte) ((uint) x - ((uint) x & (uint) (num1 - 1))), (byte) ((uint) y - ((uint) y & (uint) (num1 - 1))));
            uint existingSurface = existingSurfaces[(int) tileInChunk256_1.Index];
            if (existingSurface == uint.MaxValue)
              return;
            TerrainRenderer.TileSurfaceInstanceData data1 = renderer.GetData((ushort) existingSurface);
            renderer.RemoveInstance((ushort) existingSurface);
            existingSurfaces[(int) tileInChunk256_1.Index] = uint.MaxValue;
            for (int lastLevel = lastLevel; lastLevel > 0; --lastLevel)
            {
              int num2 = (int) pair.First.X & (1 << lastLevel) - 1;
              int num3 = (int) pair.First.Y & (1 << lastLevel) - 1;
              byte num4 = (byte) ((uint) pair.First.X - (uint) num2);
              byte num5 = (byte) ((uint) pair.First.Y - (uint) num3);
              int scale = 1 << lastLevel - 1;
              for (int index1 = 0; index1 <= 1; ++index1)
              {
                for (int index2 = 0; index2 <= 1; ++index2)
                {
                  if (num2 >= scale != index2 > 0 || num3 >= scale != index1 > 0)
                  {
                    TileInChunk256 tileInChunk256_2 = new TileInChunk256((byte) ((uint) num4 + (uint) (index2 * scale)), (byte) ((uint) num5 + (uint) (index1 * scale)));
                    Assert.That<uint>(existingSurfaces[(int) tileInChunk256_2.Index]).IsEqualTo(uint.MaxValue);
                    TerrainRenderer.TileSurfaceInstanceData data2 = new TerrainRenderer.TileSurfaceInstanceData(this.Origin + tileInChunk256_2.AsTileOffset.ToUnityUnits(), data1.Data, scale);
                    existingSurfaces[(int) tileInChunk256_2.Index] = (uint) renderer.AddInstance(data2);
                  }
                }
              }
            }
          }
        }
        this.m_changedSurfacesForUpdate.ClearSkipZeroingMemory();
      }

      private void processNeighborSurfaceChanges(ref bool surfaceDataDirty)
      {
        if (this.m_terrainSurfaceDataCPU.IsNone)
        {
          this.m_terrainSurfaceDataCPU = (Option<TerrainRenderer.TileSurfaceAreaData[]>) new TerrainRenderer.TileSurfaceAreaData[65536];
          this.m_terrainSurfaceDataGPU.ValueOrNull?.Dispose();
          this.m_terrainSurfaceDataGPU = (Option<ComputeBuffer>) new ComputeBuffer(65536, 4);
        }
        foreach (Pair<TileInChunk256, byte> pair in this.m_changedNeighborSurfacesForUpdate)
        {
          byte x = pair.First.X;
          byte y = pair.First.Y;
          this.m_terrainSurfaceDataCPU.Value[(int) y * 256 + (int) x] = this.m_terrainSurfaceDataCPU.Value[(int) y * 256 + (int) x].WithNewNeighborData(pair.Second);
        }
        surfaceDataDirty = true;
        this.m_changedNeighborSurfacesForUpdate.Clear();
      }

      private bool canHaveDetails(Tile2iIndex tileIndex, HeightTilesF height)
      {
        TerrainManager terrain = this.m_parentRenderer.m_terrain;
        if (terrain.IsOnMapBoundary(tileIndex))
          return false;
        ReadOnlyArray<TileSurfaceData> rawSurfacesData = terrain.GetRawSurfacesData();
        if (!rawSurfacesData[tileIndex.Value].SurfaceSlimId.IsNotPhantom)
        {
          TileSurfaceSlimId surfaceSlimId = rawSurfacesData[tileIndex.MinusXNeighborUnchecked.Value].SurfaceSlimId;
          if (!surfaceSlimId.IsNotPhantom)
          {
            surfaceSlimId = rawSurfacesData[tileIndex.MinusXMinusYNeighborUnchecked(terrain.TerrainWidth).Value].SurfaceSlimId;
            if (!surfaceSlimId.IsNotPhantom)
            {
              surfaceSlimId = rawSurfacesData[tileIndex.MinusYNeighborUnchecked(terrain.TerrainWidth).Value].SurfaceSlimId;
              if (!surfaceSlimId.IsNotPhantom)
              {
                HeightTilesI tilesHeightCeiled = height.TilesHeightCeiled;
                return this.m_parentRenderer.m_occupancyManager.GetHeightClearance(tileIndex, tilesHeightCeiled) >= ThicknessTilesI.One && this.m_parentRenderer.m_occupancyManager.GetHeightClearance(tileIndex.MinusXNeighborUnchecked, tilesHeightCeiled) >= ThicknessTilesI.One && this.m_parentRenderer.m_occupancyManager.GetHeightClearance(tileIndex.MinusXMinusYNeighborUnchecked(terrain.TerrainWidth), tilesHeightCeiled) >= ThicknessTilesI.One && this.m_parentRenderer.m_occupancyManager.GetHeightClearance(tileIndex.MinusYNeighborUnchecked(terrain.TerrainWidth), tilesHeightCeiled) >= ThicknessTilesI.One;
              }
            }
          }
        }
        return false;
      }

      private void ensureNoCloseDetails(TileInChunk256 tileLocalIndex)
      {
        TerrainRenderer.TopTwoDetailLayers[] valueOrNull = this.m_topTwoDetailLayers.ValueOrNull;
        if (valueOrNull == null)
          return;
        TerrainRenderer.TopTwoDetailLayers topTwoDetailLayers = valueOrNull[(int) tileLocalIndex.Index];
        if (topTwoDetailLayers.IsEmpty)
          return;
        this.removeDetail((int) topTwoDetailLayers.First.DetailId, topTwoDetailLayers.First.InstanceId);
        if (topTwoDetailLayers.Second.IsNotEmpty)
          this.removeDetail((int) topTwoDetailLayers.Second.DetailId, topTwoDetailLayers.Second.InstanceId);
        valueOrNull[(int) tileLocalIndex.Index] = new TerrainRenderer.TopTwoDetailLayers();
      }

      private void ensureNoFarDetails(TileInChunk256 tileLocalIndex)
      {
        TerrainRenderer.TerrainDetailLayerData[] valueOrNull = this.m_farDetailsLayer.ValueOrNull;
        if (valueOrNull == null)
          return;
        TerrainRenderer.TerrainDetailLayerData terrainDetailLayerData = valueOrNull[(int) tileLocalIndex.Index];
        if (terrainDetailLayerData.IsNotEmpty)
          this.removeDetailFar((int) terrainDetailLayerData.DetailId, terrainDetailLayerData.InstanceId);
        valueOrNull[(int) tileLocalIndex.Index] = new TerrainRenderer.TerrainDetailLayerData();
      }

      private void updateTerrainDetails(
        TileInChunk256 tileLocalIndex,
        Pair<TerrainMaterialSlimId, Fix32>[] materials,
        int materialsCount)
      {
        int detailsCount = 0;
        Pair<int, float>[] detailsWeights = this.m_parentRenderer.m_newDetailsTmp;
        for (int index = 0; index < materialsCount; ++index)
        {
          Pair<TerrainMaterialSlimId, Fix32> material = materials[index];
          Pair<int, float>[] pairArray = this.m_parentRenderer.m_detailIndicesPerMaterial[(int) material.First.Value];
          if (pairArray != null)
          {
            foreach (Pair<int, float> pair in pairArray)
              mergeDetail(pair.First, pair.Second * material.Second.ToFloat());
          }
        }
        for (int index = 0; index < detailsCount; ++index)
        {
          if ((double) detailsWeights[index].Second < 0.05000000074505806)
          {
            --detailsCount;
            if (detailsCount != index)
            {
              detailsWeights[index] = detailsWeights[detailsCount];
              --index;
            }
            else
              break;
          }
        }
        if (detailsCount <= 1)
        {
          if (detailsCount <= 0)
          {
            this.ensureNoCloseDetails(tileLocalIndex);
            this.ensureNoFarDetails(tileLocalIndex);
          }
          else
          {
            Pair<int, float> pair = detailsWeights[0];
            DetailLayerSpecProto detailLayer = this.m_parentRenderer.m_detailLayers[pair.First];
            int index1 = getDetailProbabilityIndex(pair.First) & (int) ushort.MaxValue;
            if ((double) (pair.Second * detailLayer.UniformRandomSpawnProbability) >= (double) this.m_parentRenderer.m_detailSpawnProbabilities[index1])
              this.updateCloseDetails(tileLocalIndex, pair, new Pair<int, float>());
            else
              this.ensureNoCloseDetails(tileLocalIndex);
            int index2 = index1 * 15913 & (int) ushort.MaxValue;
            if ((double) (pair.Second * detailLayer.UniformRandomSpawnProbabilityFar) >= (double) this.m_parentRenderer.m_detailSpawnProbabilities[index2])
              this.updateFarDetail(tileLocalIndex, pair);
            else
              this.ensureNoFarDetails(tileLocalIndex);
          }
        }
        else
        {
          Pair<int, float> farDetail = new Pair<int, float>();
          float num1 = 0.0f;
          Pair<int, float> detail1 = new Pair<int, float>();
          float num2 = 0.0f;
          Pair<int, float> detail2 = new Pair<int, float>();
          float num3 = 0.0f;
          for (int index3 = 0; index3 < detailsCount; ++index3)
          {
            Pair<int, float> pair = detailsWeights[index3];
            DetailLayerSpecProto detailLayer = this.m_parentRenderer.m_detailLayers[pair.First];
            int index4 = getDetailProbabilityIndex(pair.First) & (int) ushort.MaxValue;
            float num4 = pair.Second * detailLayer.UniformRandomSpawnProbability;
            float spawnProbability1 = this.m_parentRenderer.m_detailSpawnProbabilities[index4];
            if ((double) num4 >= (double) spawnProbability1)
            {
              float num5 = num4 * spawnProbability1;
              if ((double) num5 > (double) num2)
              {
                detail2 = detail1;
                detail1 = pair;
                num2 = num5;
              }
              else if ((double) num5 > (double) num3)
              {
                detail2 = pair;
                num3 = num5;
              }
            }
            int index5 = index4 * 15913 & (int) ushort.MaxValue;
            float num6 = pair.Second * detailLayer.UniformRandomSpawnProbabilityFar;
            float spawnProbability2 = this.m_parentRenderer.m_detailSpawnProbabilities[index5];
            if ((double) num6 >= (double) spawnProbability2)
            {
              float num7 = num6 * spawnProbability2;
              if ((double) num7 > (double) num1)
              {
                farDetail = pair;
                num1 = num7;
              }
            }
          }
          if ((double) farDetail.Second < 0.05000000074505806)
          {
            this.ensureNoFarDetails(tileLocalIndex);
            this.ensureNoCloseDetails(tileLocalIndex);
          }
          else
          {
            this.updateFarDetail(tileLocalIndex, farDetail);
            if ((double) detail1.Second < 0.05000000074505806)
              this.ensureNoCloseDetails(tileLocalIndex);
            else
              this.updateCloseDetails(tileLocalIndex, detail1, detail2);
          }
        }

        void mergeDetail(int index, float weight)
        {
          for (int index1 = 0; index1 < detailsCount; ++index1)
          {
            Pair<int, float> detailsWeight = detailsWeights[index1];
            if (detailsWeight.First == index)
            {
              detailsWeights[index1] = Pair.Create<int, float>(index, detailsWeight.Second + weight);
              return;
            }
          }
          detailsWeights[detailsCount] = Pair.Create<int, float>(index, weight);
          ++detailsCount;
        }

        int getDetailProbabilityIndex(int detailIndex)
        {
          return (int) tileLocalIndex.Index + this.m_randomDetailsOffset + this.m_parentRenderer.m_detailLayers[detailIndex].RandomSeed;
        }
      }

      private void updateFarDetail(TileInChunk256 tileLocalIndex, Pair<int, float> farDetail)
      {
        TerrainRenderer.TerrainDetailLayerData[] terrainDetailLayerDataArray = this.m_farDetailsLayer.ValueOrNull;
        if (terrainDetailLayerDataArray == null)
        {
          terrainDetailLayerDataArray = new TerrainRenderer.TerrainDetailLayerData[65536];
          this.m_farDetailsLayer = (Option<TerrainRenderer.TerrainDetailLayerData[]>) terrainDetailLayerDataArray;
        }
        TerrainRenderer.TerrainDetailLayerData terrainDetailLayerData = terrainDetailLayerDataArray[(int) tileLocalIndex.Index];
        if (terrainDetailLayerData.IsEmpty)
        {
          terrainDetailLayerDataArray[(int) tileLocalIndex.Index] = this.addDetailFar(farDetail.First, this.m_originTile + tileLocalIndex.AsTileOffset, tileLocalIndex);
        }
        else
        {
          if ((int) terrainDetailLayerData.DetailId == farDetail.First)
            return;
          this.removeDetailFar((int) terrainDetailLayerData.DetailId, terrainDetailLayerData.InstanceId);
          terrainDetailLayerDataArray[(int) tileLocalIndex.Index] = this.addDetailFar(farDetail.First, this.m_originTile + tileLocalIndex.AsTileOffset, tileLocalIndex);
        }
      }

      private void updateCloseDetails(
        TileInChunk256 tileLocalIndex,
        Pair<int, float> detail1,
        Pair<int, float> detail2)
      {
        TerrainRenderer.TopTwoDetailLayers[] topTwoDetailLayersArray = this.m_topTwoDetailLayers.ValueOrNull;
        if (topTwoDetailLayersArray == null)
        {
          topTwoDetailLayersArray = new TerrainRenderer.TopTwoDetailLayers[65536];
          this.m_topTwoDetailLayers = (Option<TerrainRenderer.TopTwoDetailLayers[]>) topTwoDetailLayersArray;
        }
        TerrainRenderer.TopTwoDetailLayers topTwoDetailLayers = topTwoDetailLayersArray[(int) tileLocalIndex.Index];
        if (topTwoDetailLayers.IsEmpty)
        {
          Tile2i position = this.m_originTile + tileLocalIndex.AsTileOffset;
          TerrainRenderer.TerrainDetailLayerData first = this.addDetail(detail1.First, position, tileLocalIndex);
          TerrainRenderer.TerrainDetailLayerData second = (double) detail2.Second >= 0.05000000074505806 ? this.addDetail(detail2.First, position, tileLocalIndex) : new TerrainRenderer.TerrainDetailLayerData();
          topTwoDetailLayersArray[(int) tileLocalIndex.Index] = new TerrainRenderer.TopTwoDetailLayers(first, second);
        }
        else if (topTwoDetailLayers.Second.IsEmpty)
        {
          TerrainRenderer.TerrainDetailLayerData first;
          if ((int) topTwoDetailLayers.First.DetailId == detail1.First)
          {
            if ((double) detail2.Second < 0.05000000074505806)
              return;
            first = topTwoDetailLayers.First;
          }
          else
          {
            this.removeDetail((int) topTwoDetailLayers.First.DetailId, topTwoDetailLayers.First.InstanceId);
            first = this.addDetail(detail1.First, this.m_originTile + tileLocalIndex.AsTileOffset, tileLocalIndex);
          }
          TerrainRenderer.TerrainDetailLayerData second = (double) detail2.Second >= 0.05000000074505806 ? this.addDetail(detail2.First, this.m_originTile + tileLocalIndex.AsTileOffset, tileLocalIndex) : new TerrainRenderer.TerrainDetailLayerData();
          topTwoDetailLayersArray[(int) tileLocalIndex.Index] = new TerrainRenderer.TopTwoDetailLayers(first, second);
        }
        else
        {
          TerrainRenderer.TerrainDetailLayerData first;
          if ((int) topTwoDetailLayers.First.DetailId == detail1.First)
          {
            first = topTwoDetailLayers.First;
          }
          else
          {
            this.removeDetail((int) topTwoDetailLayers.First.DetailId, topTwoDetailLayers.First.InstanceId);
            first = this.addDetail(detail1.First, this.m_originTile + tileLocalIndex.AsTileOffset, tileLocalIndex);
          }
          TerrainRenderer.TerrainDetailLayerData second;
          if ((double) detail2.Second >= 0.05000000074505806)
          {
            if ((int) topTwoDetailLayers.Second.DetailId == detail2.First)
            {
              second = topTwoDetailLayers.Second;
            }
            else
            {
              this.removeDetail((int) topTwoDetailLayers.Second.DetailId, topTwoDetailLayers.Second.InstanceId);
              second = this.addDetail(detail2.First, this.m_originTile + tileLocalIndex.AsTileOffset, tileLocalIndex);
            }
          }
          else
          {
            this.removeDetail((int) topTwoDetailLayers.Second.DetailId, topTwoDetailLayers.Second.InstanceId);
            second = new TerrainRenderer.TerrainDetailLayerData();
          }
          topTwoDetailLayersArray[(int) tileLocalIndex.Index] = new TerrainRenderer.TopTwoDetailLayers(first, second);
        }
      }

      private TerrainRenderer.DetailsLayerChunkRenderer getOrCreateDetailsChunk(int detailIndex)
      {
        TerrainRenderer.DetailsLayerChunkRenderer detailsChunk = this.m_terrainDetails[detailIndex].ValueOrNull;
        if (detailsChunk == null)
        {
          new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, (ulong) this.m_randomDetailsOffset, (ulong) this.CoordAndIndex.CoordAndIndexPacked).Jump();
          TerrainRenderer.DetailsLayerChunkRenderer layerChunkRenderer = new TerrainRenderer.DetailsLayerChunkRenderer(detailIndex, this.m_parentRenderer);
          this.m_terrainDetails[detailIndex] = (Option<TerrainRenderer.DetailsLayerChunkRenderer>) (detailsChunk = layerChunkRenderer);
          this.m_terrainDetailsNonEmpty = this.m_terrainDetailsNonEmpty.Add(layerChunkRenderer);
        }
        return detailsChunk;
      }

      private TerrainRenderer.TerrainDetailLayerData addDetail(
        int detailIndex,
        Tile2i position,
        TileInChunk256 tileIndex)
      {
        return this.getOrCreateDetailsChunk(detailIndex).AddDetail(position, tileIndex, this.m_randomDetailsOffset);
      }

      private TerrainRenderer.TerrainDetailLayerData addDetailFar(
        int detailIndex,
        Tile2i position,
        TileInChunk256 tileIndex)
      {
        return this.getOrCreateDetailsChunk(detailIndex).AddDetailFar(position, tileIndex, this.m_randomDetailsOffset);
      }

      private void removeDetail(int detailIndex, ushort instanceId)
      {
        TerrainRenderer.DetailsLayerChunkRenderer valueOrNull = this.m_terrainDetails[detailIndex].ValueOrNull;
        if (valueOrNull == null)
          Log.Error("Deleting non-existing detail");
        else
          valueOrNull.RemoveDetail(instanceId);
      }

      private void removeDetailFar(int detailIndex, ushort instanceId)
      {
        TerrainRenderer.DetailsLayerChunkRenderer valueOrNull = this.m_terrainDetails[detailIndex].ValueOrNull;
        if (valueOrNull == null)
          Log.Error("Deleting non-existing detail");
        else
          valueOrNull.RemoveDetailFar(instanceId);
      }

      static TerrainRendererChunk()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        TerrainRenderer.TerrainRendererChunk.SURFACE_DATA_SHADER_ID = Shader.PropertyToID("_PerTileData");
      }
    }

    /// <summary>Handles rendering of one type of terrain detail.</summary>
    private sealed class DetailsLayerChunkRenderer : IDisposable
    {
      private static readonly int FADE_START_END_SHADER_ID;
      private static readonly int ANIMATION_END_SHADER_ID;
      private static readonly int VISIBILITY_BOOST_SHADER_ID;
      private readonly int m_detailIndex;
      private readonly TerrainRenderer m_parentRenderer;
      private readonly DetailLayerSpecProto m_detailProto;
      private readonly InstancedMeshesRenderer<TerrainRenderer.DetailInstanceData> m_instancedRenderer;
      private readonly InstancedMeshesRenderer<TerrainRenderer.DetailInstanceData> m_instancedRendererFar;

      public int InstancesCount => this.m_instancedRenderer.InstancesCount;

      public DetailsLayerChunkRenderer(int detailIndex, TerrainRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_detailIndex = detailIndex;
        this.m_parentRenderer = parentRenderer;
        this.m_detailProto = parentRenderer.m_detailLayers[detailIndex];
        this.m_instancedRenderer = InstancedMeshesRenderer<TerrainRenderer.DetailInstanceData>.CreateUninitialized(16, Layer.Custom12Terrain, shadowCastingMode: ShadowCastingMode.Off);
        parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_instancedRenderer);
        this.m_instancedRendererFar = InstancedMeshesRenderer<TerrainRenderer.DetailInstanceData>.CreateUninitialized(16, Layer.Custom12Terrain, shadowCastingMode: ShadowCastingMode.Off);
        parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_instancedRendererFar);
      }

      public void Dispose()
      {
        this.m_parentRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<TerrainRenderer.DetailInstanceData>>(this.m_instancedRenderer);
        this.m_parentRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<TerrainRenderer.DetailInstanceData>>(this.m_instancedRendererFar);
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> instances)
      {
        instances.Add(new RenderedInstancesInfo("Terrain detail: " + this.m_detailProto.Id.Value, this.m_instancedRenderer.InstancesCount, this.m_instancedRenderer.IndicesCountForLod0));
        instances.Add(new RenderedInstancesInfo("Terrain detail far: " + this.m_detailProto.Id.Value, this.m_instancedRendererFar.InstancesCount, this.m_instancedRendererFar.IndicesCountForLod0));
      }

      public TerrainRenderer.TerrainDetailLayerData AddDetail(
        Tile2i position,
        TileInChunk256 tileIndex,
        int randomDetailsOffset)
      {
        TerrainRenderer.RandomDetailOffset randomDetailOffset = this.m_parentRenderer.m_randomDetailOffsets[(int) tileIndex.Index + randomDetailsOffset + this.m_detailProto.RandomSeed & (int) ushort.MaxValue];
        DetailLayerSpecProto.DetailVariant detailVariant = this.m_detailProto.Variants.SampleRandomWeighted((int) randomDetailOffset.Variant * this.m_detailProto.VariantsTotalWeight / 65536, (Func<DetailLayerSpecProto.DetailVariant, int>) (x => x.SpawnWeight));
        float num1 = (float) randomDetailOffset.Scale * detailVariant.ScaleVariationOverShortMaxMult;
        uint num2 = (uint) (((double) detailVariant.BaseScale + (double) num1) * (double) sbyte.MaxValue);
        return new TerrainRenderer.TerrainDetailLayerData((byte) this.m_detailIndex, this.m_instancedRenderer.AddInstance(new TerrainRenderer.DetailInstanceData((float) (position.X * 2) + (float) randomDetailOffset.Dx * detailVariant.PositionRandomnessOverrSbyteMaxMult, (float) (position.Y * 2) + (float) randomDetailOffset.Dy * detailVariant.PositionRandomnessOverrSbyteMaxMult, (uint) ((int) randomDetailOffset.RotCosSinPacked << 8 | ((int) num2 & (int) byte.MaxValue) << 24), detailVariant.UvOriginAndSizePacked)));
      }

      public TerrainRenderer.TerrainDetailLayerData AddDetailFar(
        Tile2i position,
        TileInChunk256 tileIndex,
        int randomDetailsOffset)
      {
        TerrainRenderer.RandomDetailOffset randomDetailOffset = this.m_parentRenderer.m_randomDetailOffsets[((int) tileIndex.Index + randomDetailsOffset + this.m_detailProto.RandomSeed) * 15913 & (int) ushort.MaxValue];
        DetailLayerSpecProto.DetailVariant detailVariant = this.m_detailProto.Variants.SampleRandomWeighted((int) randomDetailOffset.Variant * this.m_detailProto.VariantsTotalWeight / 65536, (Func<DetailLayerSpecProto.DetailVariant, int>) (x => x.SpawnWeight));
        float num1 = (float) randomDetailOffset.Scale * detailVariant.ScaleVariationOverShortMaxMult;
        uint num2 = (uint) (((double) detailVariant.BaseScale + (double) num1) * (double) sbyte.MaxValue);
        return new TerrainRenderer.TerrainDetailLayerData((byte) this.m_detailIndex, this.m_instancedRendererFar.AddInstance(new TerrainRenderer.DetailInstanceData((float) (position.X * 2) + (float) randomDetailOffset.Dx * detailVariant.PositionRandomnessOverrSbyteMaxMult, (float) (position.Y * 2) + (float) randomDetailOffset.Dy * detailVariant.PositionRandomnessOverrSbyteMaxMult, (uint) ((int) randomDetailOffset.RotCosSinPacked << 8 | ((int) num2 & (int) byte.MaxValue) << 24), detailVariant.UvOriginAndSizePacked)));
      }

      public void RemoveDetail(ushort instanceId)
      {
        this.m_instancedRenderer.RemoveInstance(instanceId);
      }

      public void RemoveDetailFar(ushort instanceId)
      {
        this.m_instancedRendererFar.RemoveInstance(instanceId);
      }

      public void UpdateRenderBuffers()
      {
        if (!this.m_instancedRenderer.IsInitialized)
        {
          Pair<Mesh[], Material> meshesAndMaterial = this.m_parentRenderer.m_detailLayerMeshesAndMaterials[this.m_detailIndex];
          this.m_instancedRenderer.DelayedInitialize(meshesAndMaterial.First, UnityEngine.Object.Instantiate<Material>(meshesAndMaterial.Second));
          this.m_instancedRendererFar.DelayedInitialize(meshesAndMaterial.First, UnityEngine.Object.Instantiate<Material>(meshesAndMaterial.Second));
          this.UpdateRenderDistance();
        }
        this.m_instancedRenderer.UpdateRenderDataIfNeeded();
        this.m_instancedRendererFar.UpdateRenderDataIfNeeded();
      }

      public void UpdateRenderDistance()
      {
        if (!this.m_instancedRenderer.IsInitialized)
          return;
        setFade(this.m_instancedRenderer.Materials, this.m_parentRenderer.m_terrainDetailsCloseRenderDistance * 0.4f, this.m_parentRenderer.m_terrainDetailsCloseRenderDistance, 0.0f, 0.0f);
        setFade(this.m_instancedRendererFar.Materials, this.m_parentRenderer.m_terrainDetailsFarRenderDistance * 0.8f, this.m_parentRenderer.m_terrainDetailsFarRenderDistance * 1.2f, this.m_parentRenderer.m_terrainDetailsCloseRenderDistance * 0.4f, this.m_parentRenderer.m_terrainDetailsCloseRenderDistance);

        void setFade(Material[] mats, float start, float end, float boostStart, float boostEnd)
        {
          Material material = (Material) null;
          foreach (Material mat in mats)
          {
            if (!((UnityEngine.Object) mat == (UnityEngine.Object) material))
            {
              mat.SetVector(TerrainRenderer.DetailsLayerChunkRenderer.FADE_START_END_SHADER_ID, new Vector4(start, end, end * end, (float) (1.0 / ((double) end - (double) start))));
              mat.SetFloat(TerrainRenderer.DetailsLayerChunkRenderer.ANIMATION_END_SHADER_ID, this.m_parentRenderer.m_terrainDetailsAnimEndDistance);
              mat.SetVector(TerrainRenderer.DetailsLayerChunkRenderer.VISIBILITY_BOOST_SHADER_ID, new Vector4(boostStart, boostEnd, 0.5f, (double) boostEnd <= 0.0 ? 0.0f : (float) (1.0 / ((double) boostEnd - (double) boostStart))));
              material = mat;
            }
          }
        }
      }

      public RenderStats Render(Bounds bounds, float pixelsPerMeter)
      {
        if (!this.m_instancedRenderer.IsInitialized)
          return new RenderStats();
        RenderStats renderStats = new RenderStats() + this.m_instancedRendererFar.RenderNoDataUpdate(bounds, 0);
        if ((double) pixelsPerMeter >= (double) this.m_parentRenderer.m_closeDetailsPpmThreshold)
          renderStats += this.m_instancedRenderer.RenderNoDataUpdate(bounds, 0);
        return renderStats;
      }

      public void ReloadDetailTextures()
      {
        Material material1 = (Material) null;
        foreach (Material material2 in this.m_instancedRenderer.Materials)
        {
          if (!((UnityEngine.Object) material2 == (UnityEngine.Object) material1))
          {
            material2.SetTexture("_MainTex", this.m_parentRenderer.m_detailLayerMeshesAndMaterials[this.m_detailIndex].Second.GetTexture("_MainTex"));
            material2.SetTexture("_MainTex", this.m_parentRenderer.m_detailLayerMeshesAndMaterials[this.m_detailIndex].Second.GetTexture("_MainTex"));
            material1 = material2;
          }
        }
      }

      static DetailsLayerChunkRenderer()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        TerrainRenderer.DetailsLayerChunkRenderer.FADE_START_END_SHADER_ID = Shader.PropertyToID("_FadeStartEnd");
        TerrainRenderer.DetailsLayerChunkRenderer.ANIMATION_END_SHADER_ID = Shader.PropertyToID("_AnimationEndDistance");
        TerrainRenderer.DetailsLayerChunkRenderer.VISIBILITY_BOOST_SHADER_ID = Shader.PropertyToID("_VisibilityBoost");
      }
    }

    [ExpectedStructSize(12)]
    private readonly struct TexUpdateData
    {
      public readonly int Index;
      public readonly float Height;
      public readonly Color32 Splats;

      public TexUpdateData(int index, float height, Color32 splats)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Index = index;
        this.Height = height;
        this.Splats = splats;
      }
    }

    [ExpectedStructSize(12)]
    private readonly struct TexUpdateDataSlim
    {
      public readonly float Height;
      public readonly Color32 Splats;

      public TexUpdateDataSlim(float height, Color32 splats)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Height = height;
        this.Splats = splats;
      }
    }

    [ExpectedStructSize(4)]
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct TerrainDetailLayerData
    {
      [FieldOffset(0)]
      public readonly byte DetailId;
      [FieldOffset(1)]
      public readonly byte Unused;
      [FieldOffset(2)]
      public readonly ushort InstanceId;
      [FieldOffset(0)]
      public readonly uint RawData;

      public bool IsEmpty => this.RawData == 0U;

      public bool IsNotEmpty => this.RawData > 0U;

      public TerrainDetailLayerData(byte detailId, ushort instanceId)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Unused = (byte) 0;
        this.RawData = 0U;
        this.DetailId = detailId;
        this.InstanceId = instanceId;
      }
    }

    [ExpectedStructSize(8)]
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct TopTwoDetailLayers
    {
      [FieldOffset(0)]
      public readonly TerrainRenderer.TerrainDetailLayerData First;
      [FieldOffset(4)]
      public readonly TerrainRenderer.TerrainDetailLayerData Second;
      [FieldOffset(0)]
      public readonly ulong RawData;

      public bool IsEmpty => this.RawData == 0UL;

      public bool IsNotEmpty => this.RawData > 0UL;

      public TopTwoDetailLayers(
        TerrainRenderer.TerrainDetailLayerData first,
        TerrainRenderer.TerrainDetailLayerData second)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.RawData = 0UL;
        this.First = first;
        this.Second = second;
      }
    }

    /// <summary>
    /// Per-instance data that is passed to GPU. Layout of this struct must match the shader.
    /// </summary>
    [ExpectedStructSize(16)]
    private struct DetailInstanceData
    {
      public readonly float X;
      public readonly float Y;
      public uint YOffsetRotCosSinScalePacked;
      public readonly uint UvOriginAndSize;

      public DetailInstanceData(
        float x,
        float y,
        uint yOffsetRotCosSinScalePacked,
        uint uvOriginAndSize)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.X = x;
        this.Y = y;
        this.YOffsetRotCosSinScalePacked = yOffsetRotCosSinScalePacked;
        this.UvOriginAndSize = uvOriginAndSize;
      }

      public void SetYOffset(byte offsetEncoded)
      {
        this.YOffsetRotCosSinScalePacked = this.YOffsetRotCosSinScalePacked & 4294967040U | (uint) offsetEncoded;
      }

      public override readonly string ToString()
      {
        return string.Format("({0}, {1}) data 0x{2:X8} uv 0x{3:X8}", (object) this.X, (object) this.Y, (object) this.YOffsetRotCosSinScalePacked, (object) this.UvOriginAndSize);
      }
    }

    /// <summary>
    /// A struct to be used by the quadTree to determine if two surfaces can be merged into one mesh.
    /// </summary>
    private struct TileSurfaceQuadTreeComparable : 
      IQuadTreeComparable<TerrainRenderer.TileSurfaceQuadTreeComparable>
    {
      public ushort Value;

      public TileSurfaceQuadTreeComparable(TileSurfaceData data)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        Assert.That<int>(13).IsLessOrEqual(13);
        this.Value = (ushort) ((data.Height.HeightI.Value | 8192) & 65527);
        if (!data.IsRamp)
          return;
        this.Value |= (ushort) 32768;
      }

      public readonly bool IsSet() => this.Value > (ushort) 0;

      public void Clear() => this.Value = (ushort) 0;

      public readonly bool CanMergeQuadTreeWith(
        TerrainRenderer.TileSurfaceQuadTreeComparable other)
      {
        return ((int) this.Value & 32768) == 0 && this.IsSet() && (int) other.Value == (int) this.Value;
      }
    }

    /// <summary>Per-instance data that is passed to GPU.</summary>
    [ExpectedStructSize(12)]
    private readonly struct TileSurfaceInstanceData
    {
      public readonly Vector2 Position;
      public readonly int Data;

      public int Scale => 1 << (this.Data >> 14 & 15);

      public TileSurfaceInstanceData(Vector2 position, TileSurfaceData data, int scale)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        int num = Mathf.Log((float) scale, 2f).RoundToInt();
        this.Data = data.RawValue;
        if (data.IsRamp)
          return;
        this.Data |= (num & 15) << 14;
      }

      public TileSurfaceInstanceData(Vector2 position, int rawData, int scale)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
        int num = Mathf.Log((float) scale, 2f).RoundToInt();
        rawData &= -245761;
        this.Data = rawData | (num & 15) << 14;
      }
    }

    /// <summary>Per-tile data that is passed to GPU.</summary>
    [ExpectedStructSize(4)]
    private readonly struct TileSurfaceAreaData
    {
      public readonly uint RawData;

      public TileSurfaceAreaData(
        TileSurfaceSlimId surfaceSlimId,
        Rotation90 rotation,
        TileSurfaceDecalSlimId decalSlimId,
        bool flipped,
        Rotation90 decalRotation,
        int colorKey,
        byte neighborDifferentData)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        Assert.That<int>((int) surfaceSlimId.Value).IsLess(256);
        this.RawData = (uint) ((int) surfaceSlimId.Value & (int) byte.MaxValue | rotation.AngleIndex << 8 | (int) decalSlimId.Value << 10 | (flipped ? 1 : 0) << 18 | decalRotation.AngleIndex << 19 | (colorKey & 7) << 21 | (int) neighborDifferentData << 24);
      }

      private TileSurfaceAreaData(uint rawData)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.RawData = rawData;
      }

      public TerrainRenderer.TileSurfaceAreaData WithNewSurfaceData(
        TileSurfaceSlimId surfaceSlimId,
        Rotation90 rotation,
        TileSurfaceDecalSlimId decalSlimId,
        bool flipped,
        int colorKey,
        Rotation90 decalRotation)
      {
        return new TerrainRenderer.TileSurfaceAreaData((uint) ((int) surfaceSlimId.Value & (int) byte.MaxValue | rotation.AngleIndex << 8 | (int) decalSlimId.Value << 10 | (flipped ? 1 : 0) << 18 | decalRotation.AngleIndex << 19 | (colorKey & 7) << 21 | (int) this.RawData & -16777216));
      }

      public TerrainRenderer.TileSurfaceAreaData WithNewNeighborData(byte neighborDifferent)
      {
        return new TerrainRenderer.TileSurfaceAreaData((uint) ((int) neighborDifferent << 24 | (int) this.RawData & 16777215));
      }
    }
  }
}
