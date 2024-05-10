// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.WaterRendererFft
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Environment;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Core.Utils;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
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
  /// Renders Ocean using sum of large amounts of since waves. To make this efficient, inverse FFT and compute shaders
  /// are used. To avoid tiling, two cascades with different scales are simulated and then merged in shader.
  /// </summary>
  /// <remarks>Inspired by https://github.com/gasgiant/FFT-Ocean (MIT-licensed)</remarks>
  internal class WaterRendererFft : IWaterRenderer, IRenderedChunkAlwaysVisible, IRenderedChunksBase
  {
    internal const float BOUNDS_EXTRA = 4f;
    private static readonly int SHADER_SMOOTHNESS_ID;
    private static readonly int SHADER_SSS_STRENGTH_ID;
    private static readonly int SHADER_DEPTH_COLOR_ATTEN_ID;
    private static readonly int SHADER_TRANSPARENCY_ATTEN_ID;
    private static readonly int SHADER_SHORE_FOAM_ID;
    private static readonly ObjectPool<byte[]> s_updateAllDataPool;
    private readonly AssetsDb m_assetsDb;
    private readonly WaterRendererConfig m_config;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly TerrainManager m_terrain;
    private readonly TerrainRenderer m_terrainRenderer;
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly IWeatherManager m_weatherManager;
    private Option<WaterRendererManager> m_parentManager;
    private WaterRendererFft.WaterRendererChunk[] m_renderChunks;
    private WaterRendererFft.WaterBoundaryChunk[] m_renderChunksBoundary;
    private float m_oceanSimTime;
    private float m_originalWaterSpec;
    private float m_targetChoppiness;
    private float m_oldChoppiness;
    private float m_transitionDurationRemainingMs;
    private InstancedMeshesRenderer<WaterRendererFft.OceanChunkInstanceData> m_instancedOceanRenderer;
    private InstancedMeshesRenderer<WaterRendererFft.OceanChunkInstanceData> m_instancedOceanRendererFar;
    private Material m_oceanMaterial;
    private Material m_oceanMaterialFar;
    private Texture2D m_oceanTexture;
    private Texture2D m_gaussianNoise;
    private WaterRendererFft.FastFourierTransform m_fft;
    private WaterRendererFft.WavesCascade m_cascade0;
    private WaterRendererFft.WavesCascade m_cascade1;
    private ComputeShader m_fftShader;
    private ComputeShader m_initialSpectrumShader;
    private ComputeShader m_timeDependentSpectrumShader;
    private ComputeShader m_texturesMergerShader;
    private bool m_isOceanRenderingEnabled;

    public static bool IsSupported => SystemInfo.supportsComputeShaders;

    public string Name => "Water";

    public WaterRendererFft(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      AssetsDb assets,
      TerrainManager terrainManager,
      TerrainRenderer terrainRenderer,
      ChunkBasedRenderingManager chunksRenderer,
      ReloadAfterAssetUpdateManager reloadManager,
      IWeatherManager weatherManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_isOceanRenderingEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(WaterRendererFft.IsSupported).IsTrue("Creating water FFT renderer on a platform that does not support it");
      this.m_config = new WaterRendererConfig();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_simLoopEvents = simLoopEvents;
      this.m_terrain = terrainManager;
      this.m_terrainRenderer = terrainRenderer;
      this.m_chunksRenderer = chunksRenderer;
      this.m_reloadManager = reloadManager;
      this.m_weatherManager = weatherManager;
      this.m_assetsDb = assets;
      this.m_renderChunks = new WaterRendererFft.WaterRendererChunk[chunksRenderer.ChunksCountTotal];
    }

    public void SetOceanRenderingState(bool isEnabled)
    {
      this.m_isOceanRenderingEnabled = isEnabled;
    }

    public bool TryInitializeAndRegister(WaterRendererManager manager)
    {
      this.m_parentManager = (Option<WaterRendererManager>) manager;
      this.m_targetChoppiness = this.m_oldChoppiness = this.m_weatherManager.CurrentWeather.Graphics.OceanChoppiness;
      this.m_fftShader = this.m_assetsDb.GetSharedAssetOrThrow<ComputeShader>("Assets/Base/Water/ComputeShaders/FastFourierTransform.compute");
      this.m_initialSpectrumShader = this.m_assetsDb.GetSharedAssetOrThrow<ComputeShader>("Assets/Base/Water/ComputeShaders/InitialSpectrum.compute");
      this.m_timeDependentSpectrumShader = this.m_assetsDb.GetSharedAssetOrThrow<ComputeShader>("Assets/Base/Water/ComputeShaders/TimeDependentSpectrum.compute");
      this.m_texturesMergerShader = this.m_assetsDb.GetSharedAssetOrThrow<ComputeShader>("Assets/Base/Water/ComputeShaders/WavesTexturesMerger.compute");
      Texture2D texture2D = new Texture2D(this.m_terrain.TerrainWidth, this.m_terrain.TerrainHeight, TextureFormat.R8, false, true);
      texture2D.name = "OceanAlpha";
      texture2D.anisoLevel = 0;
      texture2D.filterMode = FilterMode.Bilinear;
      texture2D.wrapMode = TextureWrapMode.Clamp;
      this.m_oceanTexture = texture2D;
      if (this.m_renderChunks.Length != this.m_chunksRenderer.ChunksCountTotal)
        this.m_renderChunks = new WaterRendererFft.WaterRendererChunk[this.m_chunksRenderer.ChunksCountTotal];
      for (int index = 0; index < this.m_renderChunks.Length; ++index)
      {
        WaterRendererFft.WaterRendererChunk newChunk = new WaterRendererFft.WaterRendererChunk(this.m_chunksRenderer.ExtendChunkCoord(new Chunk256Index((ushort) index)), this);
        this.m_renderChunks[index] = newChunk;
        this.m_chunksRenderer.RegisterChunk((IRenderedChunk) newChunk);
        newChunk.InitializeData();
      }
      this.m_oceanTexture.Apply(false, true);
      this.m_fft = new WaterRendererFft.FastFourierTransform(256, this.m_fftShader);
      this.m_gaussianNoise = this.generateNoiseTexture(256);
      this.m_cascade0 = new WaterRendererFft.WavesCascade(256, this.m_initialSpectrumShader, this.m_timeDependentSpectrumShader, this.m_texturesMergerShader, this.m_fft, this.m_gaussianNoise);
      this.m_cascade1 = new WaterRendererFft.WavesCascade(256, this.m_initialSpectrumShader, this.m_timeDependentSpectrumShader, this.m_texturesMergerShader, this.m_fft, this.m_gaussianNoise);
      this.m_oceanMaterial = this.m_assetsDb.GetClonedMaterial("Assets/Base/Water/OceanFft.mat");
      this.m_oceanMaterialFar = this.m_assetsDb.GetClonedMaterial("Assets/Base/Water/OceanFft.mat");
      this.initializeMaterial(this.m_oceanMaterial, true);
      this.initializeMaterial(this.m_oceanMaterialFar, false);
      this.setOceanParameters(this.m_targetChoppiness);
      RenderTexture validationRenderTex = this.m_cascade0.displacement;
      RenderTexture.active = validationRenderTex;
      GL.Clear(true, true, Color.clear);
      RenderTexture.active = (RenderTexture) null;
      this.m_oceanSimTime = 0.0f;
      for (int index = 0; index < 5; ++index)
      {
        this.m_oceanSimTime += 0.05f;
        this.m_cascade0.CalculateWavesAtTime(this.m_oceanSimTime, 1f);
        this.m_cascade1.CalculateWavesAtTime(this.m_oceanSimTime, 1f);
      }
      this.m_instancedOceanRenderer = new InstancedMeshesRenderer<WaterRendererFft.OceanChunkInstanceData>(LodUtils.SameMeshForAllLods(this.m_terrainRenderer.TerrainChunkMesh), this.m_oceanMaterial, layer: Layer.Unity04Water, shadowCastingMode: ShadowCastingMode.Off);
      this.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_instancedOceanRenderer);
      Mesh chunkMeshWithSkirt = MeshBuilder.CreateChunkMeshWithSkirt(16, 8f, 2f, true);
      chunkMeshWithSkirt.name = "OceanFarChunkMesh";
      this.m_instancedOceanRendererFar = new InstancedMeshesRenderer<WaterRendererFft.OceanChunkInstanceData>(LodUtils.SameMeshForAllLods(chunkMeshWithSkirt), this.m_oceanMaterialFar, layer: Layer.Unity04Water, shadowCastingMode: ShadowCastingMode.Off);
      this.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_instancedOceanRendererFar);
      this.createBoundaryChunks();
      foreach (IRenderedChunkCustom newChunk in this.m_renderChunksBoundary)
        this.m_chunksRenderer.RegisterChunk(newChunk);
      this.m_chunksRenderer.RegisterChunkAlwaysRender((IRenderedChunkAlwaysVisible) this);
      this.m_terrain.OceanFlagChanged.AddNonSaveable<WaterRendererFft>(this, new Action<Tile2iAndIndex>(this.oceanChanged));
      this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<WaterRendererFft>(this, new Action<GameTime>(this.renderUpdate));
      this.m_gameLoopEvents.RenderUpdateEnd.AddNonSaveable<WaterRendererFft>(this, new Action<GameTime>(this.renderUpdateEnd));
      this.m_simLoopEvents.UpdateEndForUi.AddNonSaveable<WaterRendererFft>(this, new Action(this.simUpdate));
      MinMaxPair<float> minMaxValidation = computeMinMaxValidation();
      return (double) minMaxValidation.Min < -0.10000000149011612 && (double) minMaxValidation.Min > -10.0 && (double) minMaxValidation.Max > 0.10000000149011612 && (double) minMaxValidation.Max < 10.0;

      MinMaxPair<float> computeMinMaxValidation()
      {
        RenderTexture.active = validationRenderTex;
        Texture2D texture2D = new Texture2D(this.m_cascade0.displacement.width, this.m_cascade0.displacement.height, TextureFormat.RGBAFloat, false);
        texture2D.ReadPixels(new Rect(0.0f, 0.0f, (float) texture2D.width, (float) texture2D.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = (RenderTexture) null;
        float num1 = float.PositiveInfinity;
        float num2 = float.NegativeInfinity;
        NativeArray<float> rawTextureData = texture2D.GetRawTextureData<float>();
        for (int index = 0; index < rawTextureData.Length; ++index)
        {
          float num3 = rawTextureData[index];
          num1 = num1.Min(num3);
          num2 = num2.Max(num3);
        }
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) texture2D);
        return new MinMaxPair<float>(num1, num2);
      }
    }

    public void UnregisterAndDispose()
    {
      Assert.That<Option<WaterRendererManager>>(this.m_parentManager).HasValue<WaterRendererManager>();
      this.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<WaterRendererFft.OceanChunkInstanceData>>(this.m_instancedOceanRenderer);
      this.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<WaterRendererFft.OceanChunkInstanceData>>(this.m_instancedOceanRendererFar);
      this.m_terrain.OceanFlagChanged.RemoveNonSaveable<WaterRendererFft>(this, new Action<Tile2iAndIndex>(this.oceanChanged));
      this.m_gameLoopEvents.RenderUpdate.RemoveNonSaveable<WaterRendererFft>(this, new Action<GameTime>(this.renderUpdate));
      this.m_gameLoopEvents.RenderUpdateEnd.RemoveNonSaveable<WaterRendererFft>(this, new Action<GameTime>(this.renderUpdateEnd));
      this.m_simLoopEvents.UpdateEndForUi.RemoveNonSaveable<WaterRendererFft>(this, new Action(this.simUpdate));
      this.m_chunksRenderer.UnregisterChunkAlwaysRender((IRenderedChunkAlwaysVisible) this);
      foreach (WaterRendererFft.WaterRendererChunk renderChunk in this.m_renderChunks)
      {
        this.m_chunksRenderer.TryUnregisterChunk((IRenderedChunk) renderChunk);
        renderChunk.Dispose();
      }
      foreach (IRenderedChunkCustom newChunk in this.m_renderChunksBoundary)
        this.m_chunksRenderer.TryUnregisterChunk(newChunk);
      this.m_cascade0.Dispose();
      this.m_cascade1.Dispose();
      this.m_oceanTexture.DestroyIfNotNull();
      this.m_oceanMaterial.DestroyIfNotNull();
      this.m_oceanMaterialFar.DestroyIfNotNull();
      this.m_gaussianNoise.DestroyIfNotNull();
      this.m_parentManager = Option<WaterRendererManager>.None;
    }

    public void NotifyChunkUpdated(Chunk2i chunk)
    {
      uint index = (uint) this.m_chunksRenderer.GetChunkIndex(chunk.Tile2i).Value;
      if (index < (uint) this.m_renderChunks.Length)
        this.m_renderChunks[(int) index].AllTilesChanged();
      else
        Log.Error(string.Format("Invalid chunk for update: {0}", (object) chunk));
    }

    private void createBoundaryChunks()
    {
      int chunks256PerWidth = this.m_terrain.TerrainWidth / 256;
      int chunks256PerHeight = this.m_terrain.TerrainHeight / 256;
      Vector2 origin = Vector2.zero;
      Lyst<WaterRendererFft.WaterBoundaryChunk> boundaryChunks = new Lyst<WaterRendererFft.WaterBoundaryChunk>(10 * (chunks256PerWidth + chunks256PerHeight));
      for (int index = 0; index < 2; ++index)
        createChunksLayer(1);
      ensureChunksCountDivisibleBy(2);
      for (int index = 0; index < 1; ++index)
        createChunksLayer(2);
      ensureChunksCountDivisibleBy(4);
      for (int index = 0; index < 1; ++index)
        createChunksLayer(4);
      ensureChunksCountDivisibleBy(8);
      for (int index = 0; index < 1; ++index)
        createChunksLayer(8);
      this.m_renderChunksBoundary = boundaryChunks.OrderBy<WaterRendererFft.WaterBoundaryChunk, float>((Func<WaterRendererFft.WaterBoundaryChunk, float>) (x => x.Bounds.center.y)).ThenBy<WaterRendererFft.WaterBoundaryChunk, float>((Func<WaterRendererFft.WaterBoundaryChunk, float>) (x => x.Bounds.center.x)).ToArray<WaterRendererFft.WaterBoundaryChunk>();

      void createChunk(Vector2 o, int scale)
      {
        WaterRendererFft.WaterBoundaryChunk waterBoundaryChunk = new WaterRendererFft.WaterBoundaryChunk(o, scale, this);
        boundaryChunks.Add(waterBoundaryChunk);
      }

      void createChunksLayer(int chunkScale)
      {
        float num1 = 512f * (float) chunkScale;
        origin -= new Vector2(num1, num1);
        chunks256PerWidth += 2 * chunkScale;
        chunks256PerHeight += 2 * chunkScale;
        int num2 = chunks256PerWidth / chunkScale;
        float y1 = origin.y;
        float y2 = origin.y + (float) chunks256PerHeight * 512f - num1;
        for (int index = 0; index < num2; ++index)
        {
          float x = origin.x + (float) index * num1;
          createChunk(new Vector2(x, y1), chunkScale);
          createChunk(new Vector2(x, y2), chunkScale);
        }
        float x1 = origin.x;
        float x2 = origin.x + (float) chunks256PerWidth * 512f - num1;
        int num3 = chunks256PerHeight / chunkScale - 1;
        for (int index = 1; index < num3; ++index)
        {
          float y3 = origin.y + (float) index * num1;
          createChunk(new Vector2(x1, y3), chunkScale);
          createChunk(new Vector2(x2, y3), chunkScale);
        }
      }

      void ensureChunksCountDivisibleBy(int scale)
      {
        int num1 = scale - chunks256PerWidth % scale;
        if (num1 != scale)
        {
          int scale1 = scale / 2;
          float num2 = 512f * (float) scale1;
          int num3 = chunks256PerHeight / scale1;
          float x = origin.x + (float) chunks256PerWidth * 512f;
          for (int index = 0; index < num3; ++index)
            createChunk(new Vector2(x, origin.y + (float) index * num2), scale1);
          chunks256PerWidth += num1;
        }
        int num4 = scale - chunks256PerHeight % scale;
        if (num4 == scale)
          return;
        int scale2 = scale / 2;
        float num5 = 512f * (float) scale2;
        int num6 = chunks256PerWidth / scale2;
        float y = origin.y + (float) chunks256PerHeight * 512f;
        for (int index = 0; index < num6; ++index)
          createChunk(new Vector2(origin.x + (float) index * num5, y), scale2);
        chunks256PerHeight += num4;
      }
    }

    private void initializeMaterial(Material mat, bool useDisplacement)
    {
      Vector4 vector4 = new Vector4((float) (this.m_terrain.TerrainWidth * 2), (float) (this.m_terrain.TerrainHeight * 2), 1f / (float) (this.m_terrain.TerrainWidth * 2), 1f / (float) (this.m_terrain.TerrainHeight * 2));
      mat.SetTexture("_OceanMask", (Texture) this.m_oceanTexture);
      Assert.That<Texture2D>(this.m_terrainRenderer.HeightTexture).IsValidUnityObject<Texture2D>();
      mat.SetTexture("_HeightTex", (Texture) this.m_terrainRenderer.HeightTexture);
      mat.SetVector("_TerrainSize", vector4);
      mat.SetFloat("_CascadeScale0", this.m_config.LengthScale0);
      mat.SetFloat("_CascadeScale1", this.m_config.LengthScale1);
      mat.SetTexture("_Turbulence_c0", (Texture) this.m_cascade0.Turbulence);
      mat.SetTexture("_Turbulence_c1", (Texture) this.m_cascade1.Turbulence);
      if (useDisplacement)
      {
        mat.DisableKeyword("IS_FAR_NO_DISPLACEMENT");
        mat.SetTexture("_Displacement_c0", (Texture) this.m_cascade0.Displacement);
        mat.SetTexture("_Derivatives_c0", (Texture) this.m_cascade0.Derivatives);
        mat.SetTexture("_Displacement_c1", (Texture) this.m_cascade1.Displacement);
        mat.SetTexture("_Derivatives_c1", (Texture) this.m_cascade1.Derivatives);
      }
      else
        mat.EnableKeyword("IS_FAR_NO_DISPLACEMENT");
      Shader.SetGlobalTexture("_Mafi_OceanMask", (Texture) this.m_oceanTexture);
      this.setLightCookieEnabled(mat, this.m_parentManager.Value.IsLightCookieEnabled);
    }

    private void initialiseCascades(WaterRendererFft.SpectrumSettings spectrum)
    {
      float num = (float) (6.2831854820251465 / (double) this.m_config.LengthScale1 * 6.0);
      this.m_cascade0.CalculateInitials(this.m_config, spectrum, this.m_config.LengthScale0, this.m_config.BoundaryLow, num);
      this.m_cascade1.CalculateInitials(this.m_config, spectrum, this.m_config.LengthScale1, num, this.m_config.BoundaryHigh);
    }

    private WaterRendererFft.SpectrumSettings createSettingsStruct(
      WaterRendererConfig.DisplaySpectrumSettings display,
      float g)
    {
      return new WaterRendererFft.SpectrumSettings(display.Scale, jonswapAlpha(g, display.Fetch, display.WindSpeed), jonswapPeakFrequency(g, display.Fetch, display.WindSpeed), display.PeakEnhancement);

      static float jonswapAlpha(float g, float fetch, float windSpeed)
      {
        return 0.076f * Mathf.Pow(g * fetch / windSpeed / windSpeed, -0.22f);
      }

      static float jonswapPeakFrequency(float g, float fetch, float windSpeed)
      {
        return 22f * Mathf.Pow(windSpeed * fetch / g / g, -0.33f);
      }
    }

    private Texture2D generateNoiseTexture(int size)
    {
      XorRsr128PlusGenerator random = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, (ulong) size, 61546541656515UL);
      Texture2D noiseTexture = new Texture2D(size, size, TextureFormat.RGFloat, false, true);
      noiseTexture.filterMode = FilterMode.Point;
      NativeArray<float> pixelData = noiseTexture.GetPixelData<float>(0);
      int length = pixelData.Length;
      for (int index = 0; index < length; index += 2)
      {
        Pair<float, float> pair = random.NextGaussiansFloats();
        pixelData[index] = pair.First;
        pixelData[index + 1] = pair.Second;
      }
      noiseTexture.Apply();
      return noiseTexture;
    }

    private void oceanChanged(Tile2iAndIndex tileAndIndex)
    {
      this.m_renderChunks[(int) this.m_chunksRenderer.GetChunkIndex(tileAndIndex.TileCoord).Value].OceanChanged(TileInChunk256.FromTile(tileAndIndex.TileCoord), this.m_terrain.IsOcean(tileAndIndex.Index));
    }

    private void renderUpdate(GameTime time)
    {
      this.m_instancedOceanRenderer.Clear();
      this.m_instancedOceanRendererFar.Clear();
      if (this.m_isOceanRenderingEnabled && (double) this.m_transitionDurationRemainingMs > 0.0)
      {
        this.m_transitionDurationRemainingMs -= (float) time.GameSpeedMult * time.DeltaTimeMs;
        this.setOceanParameters(this.m_targetChoppiness.Lerp(this.m_oldChoppiness, (this.m_transitionDurationRemainingMs / 8000f).Max(0.0f)));
      }
      WaterRendererFft.WaterRendererChunk[] renderChunks = this.m_renderChunks;
      int index = 0;
      while (index < renderChunks.Length && !renderChunks[index].RenderUpdate())
        ++index;
    }

    private void renderUpdateEnd(GameTime time)
    {
      if (this.m_instancedOceanRenderer.InstancesCount <= 0)
        return;
      float deltaTime = time.DeltaTimeMs / 1000f;
      this.m_oceanSimTime += deltaTime;
      this.m_cascade0.CalculateWavesAtTime(this.m_oceanSimTime, deltaTime);
      this.m_cascade1.CalculateWavesAtTime(this.m_oceanSimTime, deltaTime);
    }

    private void setOceanParameters(float choppiness)
    {
      WaterRendererConfig.WaterWeatherParams calm = this.m_config.CalmWaterParams;
      WaterRendererConfig.WaterWeatherParams rough = this.m_config.RoughWaterParams;
      this.initialiseCascades(this.createSettingsStruct(this.m_config.DefaultSpectrum with
      {
        WindSpeed = calm.Wind.Lerp(rough.Wind, choppiness)
      }, this.m_config.G));
      setParams(this.m_oceanMaterial);
      setParams(this.m_oceanMaterialFar);

      void setParams(Material mat)
      {
        mat.SetFloat(WaterRendererFft.SHADER_SMOOTHNESS_ID, calm.Smoothness.Lerp(rough.Smoothness, choppiness));
        mat.SetFloat(WaterRendererFft.SHADER_SSS_STRENGTH_ID, calm.SssIntensity.Lerp(rough.SssIntensity, choppiness));
        mat.SetFloat(WaterRendererFft.SHADER_TRANSPARENCY_ATTEN_ID, calm.TransparencyAttenMult.Lerp(rough.TransparencyAttenMult, choppiness));
        mat.SetFloat(WaterRendererFft.SHADER_DEPTH_COLOR_ATTEN_ID, calm.DepthColorAttenMult.Lerp(rough.DepthColorAttenMult, choppiness));
        mat.SetFloat(WaterRendererFft.SHADER_SHORE_FOAM_ID, calm.ShoreFoamDepth.Lerp(rough.ShoreFoamDepth, choppiness));
      }
    }

    private void simUpdate()
    {
      int num = 0;
      foreach (WaterRendererFft.WaterRendererChunk renderChunk in this.m_renderChunks)
      {
        num += renderChunk.ProcessChangesOnSim();
        if (num > 16384)
          break;
      }
      if ((double) this.m_targetChoppiness == (double) this.m_weatherManager.CurrentWeather.Graphics.OceanChoppiness)
        return;
      this.m_oldChoppiness = this.m_targetChoppiness;
      this.m_targetChoppiness = this.m_weatherManager.CurrentWeather.Graphics.OceanChoppiness;
      this.m_transitionDurationRemainingMs = 8000f;
    }

    public void SetSpecularIntensity(float percent)
    {
    }

    public void SetLightCookieEnabled(bool enabled)
    {
      this.setLightCookieEnabled(this.m_oceanMaterial, enabled);
      this.setLightCookieEnabled(this.m_oceanMaterialFar, enabled);
    }

    private void setLightCookieEnabled(Material mat, bool enabled)
    {
    }

    public void SetOpacity(float opacity)
    {
      this.m_oceanMaterial.SetFloat("_WaterOpacity", opacity);
      this.m_oceanMaterialFar.SetFloat("_WaterOpacity", opacity);
    }

    public RenderStats RenderAlwaysVisible(GameTime time, Bounds bounds)
    {
      StateAssert.IsInGameState(GameLoopState.RenderUpdateEnd);
      return !this.m_isOceanRenderingEnabled ? new RenderStats() : this.m_instancedOceanRenderer.Render(bounds, 0) + this.m_instancedOceanRendererFar.Render(bounds, 0);
    }

    public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
    {
      info.Add(new RenderedInstancesInfo("Ocean chunks", this.m_terrain.TerrainTilesCount / 4096, 26112));
    }

    private byte[] getAllTilesChunkDataPooled()
    {
      return WaterRendererFft.s_updateAllDataPool.GetInstance();
    }

    private void returnAllTilesChunkDataToPool(byte[] data)
    {
      WaterRendererFft.s_updateAllDataPool.ReturnInstance(data);
      Assert.That<int>(WaterRendererFft.s_updateAllDataPool.CountStoredInstances()).IsLessOrEqual(3);
    }

    static WaterRendererFft()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      WaterRendererFft.SHADER_SMOOTHNESS_ID = Shader.PropertyToID("_Smoothness");
      WaterRendererFft.SHADER_SSS_STRENGTH_ID = Shader.PropertyToID("_SubSurfaceSun");
      WaterRendererFft.SHADER_DEPTH_COLOR_ATTEN_ID = Shader.PropertyToID("_DepthColorAttenMult");
      WaterRendererFft.SHADER_TRANSPARENCY_ATTEN_ID = Shader.PropertyToID("_TransparencyAttenMult");
      WaterRendererFft.SHADER_SHORE_FOAM_ID = Shader.PropertyToID("_ShoreDistance");
      WaterRendererFft.s_updateAllDataPool = new ObjectPool<byte[]>(4, (Func<byte[]>) (() => new byte[65536]), (Action<byte[]>) (_ => { }));
    }

    [ExpectedStructSize(12)]
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct OceanChunkInstanceData
    {
      [FieldOffset(0)]
      public readonly Vector2 Origin;
      [FieldOffset(8)]
      public readonly float Scale;

      public OceanChunkInstanceData(Vector2 origin, float scale)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Origin = origin;
        this.Scale = scale;
      }
    }

    private abstract class WaterRendererChunkBase
    {
      protected readonly WaterRendererFft ParentRenderer;
      private readonly int m_baseScale;

      public Vector2 Origin { get; }

      protected WaterRendererChunkBase(
        Vector2 origin,
        WaterRendererFft parentRenderer,
        int baseScale)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.Origin = origin;
        this.ParentRenderer = parentRenderer;
        this.m_baseScale = baseScale;
      }

      protected RenderStats Render(float pixelsPerMeter)
      {
        if ((double) pixelsPerMeter >= 4.0)
        {
          float num1 = (float) (this.m_baseScale * 512 / 4);
          float y = this.Origin.y;
          int num2 = 0;
          while (num2 < 4)
          {
            int num3 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(new Vector2(this.Origin.x, y), (float) this.m_baseScale));
            int num4 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(new Vector2(this.Origin.x + num1, y), (float) this.m_baseScale));
            int num5 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(new Vector2(this.Origin.x + 2f * num1, y), (float) this.m_baseScale));
            int num6 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(new Vector2(this.Origin.x + 3f * num1, y), (float) this.m_baseScale));
            ++num2;
            y += num1;
          }
        }
        else if ((double) pixelsPerMeter >= 1.0)
        {
          float num7 = (float) (this.m_baseScale * 512 / 2);
          int scale = 2 * this.m_baseScale;
          int num8 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(this.Origin, (float) scale));
          int num9 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(new Vector2(this.Origin.x + num7, this.Origin.y), (float) scale));
          int num10 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(new Vector2(this.Origin.x, this.Origin.y + num7), (float) scale));
          int num11 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(new Vector2(this.Origin.x + num7, this.Origin.y + num7), (float) scale));
        }
        else if ((double) pixelsPerMeter >= 0.5)
        {
          int num12 = (int) this.ParentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRendererFft.OceanChunkInstanceData(this.Origin, (float) (4 * this.m_baseScale)));
        }
        else
        {
          int num13 = (int) this.ParentRenderer.m_instancedOceanRendererFar.AddInstance(new WaterRendererFft.OceanChunkInstanceData(this.Origin, (float) (4 * this.m_baseScale)));
        }
        return new RenderStats();
      }
    }

    private sealed class WaterRendererChunk : 
      WaterRendererFft.WaterRendererChunkBase,
      IRenderedChunk,
      IRenderedChunksBase,
      IDisposable
    {
      private readonly Tile2i m_originTile;
      private readonly Tile2iIndex m_originTileIndex;
      private Texture2D m_oceanTex;
      private LystStruct<Pair<ushort, byte>> m_oceanTexDataUpdates;
      private LystStruct<Pair<TileInChunk256, bool>> m_changedOcean;
      private Option<byte[]> m_updateAllData;
      private bool m_allTilesChanged;
      private int m_ticksSinceLastOceanUpdate;
      private int m_currentLod;
      private int m_oceanTilesCount;
      private bool m_hasOcean;
      private bool m_hasOceanOnSim;
      private volatile bool m_needsOceanTexDataUpload;

      public string Name => "Water";

      public Chunk256AndIndex CoordAndIndex { get; }

      public bool TrackStoppedRendering => true;

      public float MaxModelDeviationFromChunkBounds => 4f;

      public Vector2 MinMaxHeight => new Vector2(-4f, 4f);

      public bool HasOcean => this.m_hasOcean;

      public WaterRendererChunk(Chunk256AndIndex coordAndIndex, WaterRendererFft parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(coordAndIndex.OriginTile2i.ToVector2(), parentRenderer, 1);
        this.CoordAndIndex = coordAndIndex;
        this.m_originTile = coordAndIndex.OriginTile2i;
        this.m_originTileIndex = parentRenderer.m_terrain.GetTileIndex(this.m_originTile);
      }

      public void InitializeData()
      {
        TerrainManager terrain = this.ParentRenderer.m_terrain;
        NativeArray<byte> rawTextureData = this.ParentRenderer.m_oceanTexture.GetRawTextureData<byte>();
        int num = terrain.GetTileIndex(this.m_originTile).Value;
        for (int index1 = 0; index1 < 65536; ++index1)
        {
          TileInChunk256 tileInChunk256 = new TileInChunk256((ushort) index1);
          Tile2iIndex tileIndex = terrain.GetTileIndex(this.m_originTile + tileInChunk256.AsTileOffset);
          int index2 = num + (int) tileInChunk256.X + (int) tileInChunk256.Y * terrain.TerrainWidth;
          if (terrain.IsOcean(tileIndex))
          {
            rawTextureData[index2] = byte.MaxValue;
            ++this.m_oceanTilesCount;
          }
          else
            rawTextureData[index2] = (byte) 0;
        }
        this.m_hasOcean = this.m_hasOceanOnSim = this.m_oceanTilesCount > 0;
      }

      public void Dispose() => this.m_oceanTex.DestroyIfNotNull();

      public void OceanChanged(TileInChunk256 localIndex, bool isOcean)
      {
        this.m_changedOcean.Add(Pair.Create<TileInChunk256, bool>(localIndex, isOcean));
      }

      public void AllTilesChanged() => this.m_allTilesChanged = true;

      public bool RenderUpdate()
      {
        bool flag = false;
        if (this.m_needsOceanTexDataUpload)
        {
          this.uploadOceanTexData();
          this.m_hasOcean = this.m_hasOceanOnSim;
          this.m_needsOceanTexDataUpload = false;
          flag = true;
        }
        return flag;
      }

      public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
      {
        this.m_currentLod = lod;
        return !this.ParentRenderer.m_isOceanRenderingEnabled || !this.m_hasOcean ? new RenderStats() : this.Render(pxPerMeter);
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
      {
      }

      public void Register(IRenderedChunksParent parent)
      {
      }

      public void NotifyWasNotRendered() => this.m_currentLod = 7;

      public int ProcessChangesOnSim()
      {
        int num = 0;
        if (this.m_changedOcean.IsNotEmpty || this.m_allTilesChanged)
        {
          ++this.m_ticksSinceLastOceanUpdate;
          if (this.m_ticksSinceLastOceanUpdate >= (this.m_allTilesChanged ? 2 : this.ParentRenderer.m_terrainRenderer.UpdateDelayPerLod[this.m_currentLod]) && !this.m_needsOceanTexDataUpload)
          {
            this.m_ticksSinceLastOceanUpdate = 0;
            num += this.computeOceanDataUpdateOnSim(this.m_changedOcean);
            this.m_changedOcean.ClearSkipZeroingMemory();
            this.m_hasOceanOnSim = this.m_oceanTilesCount > 0;
            this.m_needsOceanTexDataUpload = true;
          }
        }
        return num;
      }

      private int computeOceanDataUpdateOnSim(
        LystStruct<Pair<TileInChunk256, bool>> changedOcean)
      {
        if (this.m_allTilesChanged)
        {
          this.m_allTilesChanged = false;
          byte[] tilesChunkDataPooled = this.ParentRenderer.getAllTilesChunkDataPooled();
          this.m_updateAllData = (Option<byte[]>) tilesChunkDataPooled;
          TerrainManager terrain = this.ParentRenderer.m_terrain;
          int terrainWidth = terrain.TerrainWidth;
          this.m_oceanTilesCount = 0;
          int num1 = 0;
          int num2 = 0;
          int num3 = this.m_originTileIndex.Value;
          while (num1 < 256)
          {
            for (int index = 0; index < 256; ++index)
            {
              if (terrain.IsOcean(new Tile2iIndex(num3 + index)))
              {
                tilesChunkDataPooled[num2 + index] = byte.MaxValue;
                ++this.m_oceanTilesCount;
              }
              else
                tilesChunkDataPooled[num2 + index] = (byte) 0;
            }
            ++num1;
            num2 += 256;
            num3 += terrainWidth;
          }
          return 65536;
        }
        foreach (Pair<TileInChunk256, bool> pair in changedOcean)
        {
          if (pair.Second)
          {
            this.m_oceanTexDataUpdates.Add(Pair.Create<ushort, byte>(pair.First.Index, byte.MaxValue));
            ++this.m_oceanTilesCount;
          }
          else
          {
            this.m_oceanTexDataUpdates.Add(Pair.Create<ushort, byte>(pair.First.Index, (byte) 0));
            --this.m_oceanTilesCount;
          }
        }
        return changedOcean.Count;
      }

      private void uploadOceanTexData()
      {
        NativeArray<byte> rawTextureData;
        if ((UnityEngine.Object) this.m_oceanTex == (UnityEngine.Object) null)
        {
          Texture2D texture2D = new Texture2D(256, 256, this.ParentRenderer.m_oceanTexture.format, false, true);
          texture2D.name = "OceanAlphaUpdate";
          texture2D.anisoLevel = 0;
          this.m_oceanTex = texture2D;
          rawTextureData = this.m_oceanTex.GetRawTextureData<byte>();
          TerrainManager terrain = this.ParentRenderer.m_terrain;
          for (int index = 0; index < 65536; ++index)
          {
            TileInChunk256 tileInChunk256 = new TileInChunk256((ushort) index);
            Tile2iIndex tileIndex = terrain.GetTileIndex(this.m_originTile + tileInChunk256.AsTileOffset);
            rawTextureData[index] = !terrain.IsOcean(tileIndex) ? (byte) 0 : byte.MaxValue;
          }
        }
        else
          rawTextureData = this.m_oceanTex.GetRawTextureData<byte>();
        byte[] valueOrNull = this.m_updateAllData.ValueOrNull;
        if (valueOrNull != null)
        {
          this.m_updateAllData = Option<byte[]>.None;
          rawTextureData.CopyFrom(valueOrNull);
          this.ParentRenderer.returnAllTilesChunkDataToPool(valueOrNull);
        }
        else
          Assert.That<int>(this.m_oceanTexDataUpdates.Count).IsPositive();
        foreach (Pair<ushort, byte> oceanTexDataUpdate in this.m_oceanTexDataUpdates)
          rawTextureData[(int) oceanTexDataUpdate.First] = oceanTexDataUpdate.Second;
        this.m_oceanTexDataUpdates.ClearSkipZeroingMemory();
        this.m_oceanTex.Apply(false, false);
        Graphics.CopyTexture((Texture) this.m_oceanTex, 0, 0, 0, 0, 256, 256, (Texture) this.ParentRenderer.m_oceanTexture, 0, 0, this.m_originTile.X, this.m_originTile.Y);
      }
    }

    private sealed class WaterBoundaryChunk : 
      WaterRendererFft.WaterRendererChunkBase,
      IRenderedChunkCustom,
      IRenderedChunksBase
    {
      public string Name => "Ocean boundary";

      public Bounds Bounds { get; }

      public WaterBoundaryChunk(Vector2 origin, int scale, WaterRendererFft parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(origin, parentRenderer, scale);
        float num = (float) scale * 512f;
        this.Bounds = new Bounds(new Vector3(origin.x + num / 2f, 0.0f, origin.y + num / 2f), new Vector3(num + 2f, 4f, num + 2f));
      }

      public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
      {
        return this.Render(pxPerMeter);
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
      {
      }
    }

    [ExpectedStructSize(16)]
    public struct SpectrumSettings
    {
      public const int SIZE_BYTES = 16;
      public readonly float Scale;
      public readonly float Alpha;
      public readonly float PeakOmega;
      public readonly float Gamma;

      public SpectrumSettings(float scale, float alpha, float peakOmega, float gamma)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Scale = scale;
        this.Alpha = alpha;
        this.PeakOmega = peakOmega;
        this.Gamma = gamma;
      }
    }

    [Serializable]
    public class WavesCascade
    {
      private static readonly int DELTA_TIME;
      private static readonly int SIZE_PROP;
      private static readonly int LENGTH_SCALE_PROP;
      private static readonly int CUTOFF_HIGH_PROP;
      private static readonly int CUTOFF_LOW_PROP;
      private static readonly int NOISE_PROP;
      private static readonly int H0_PROP;
      private static readonly int H0K_PROP;
      private static readonly int PRECOMPUTED_DATA_PROP;
      private static readonly int TIME_PROP;
      private static readonly int Dx_Dz_PROP;
      private static readonly int Dy_Dxz_PROP;
      private static readonly int Dyx_Dyz_PROP;
      private static readonly int Dxx_Dzz_PROP;
      private static readonly int DISPLACEMENT_PROP;
      private static readonly int DERIVATIVES_PROP;
      private static readonly int TURBULENCE_PROP;
      private static readonly int G_PROP;
      private static readonly int DEPTH_PROP;
      private static readonly int SPECTRUMS_PROP;
      private readonly int m_size;
      private readonly ComputeShader m_initialSpectrumShader;
      private readonly ComputeShader m_timeDependentSpectrumShader;
      private readonly ComputeShader m_texturesMergerShader;
      private readonly WaterRendererFft.FastFourierTransform m_fft;
      private readonly Texture2D m_gaussianNoise;
      private readonly ComputeBuffer m_paramsBuffer;
      private readonly RenderTexture m_initialSpectrum;
      private readonly RenderTexture m_precomputedData;
      private int m_kernelInitialSpectrum;
      private int m_kernelConjugateSpectrum;
      private int m_kernelTimeDependentSpectrums;
      private int m_kernelResultTextures;
      public RenderTexture buffer;
      public RenderTexture DxDz;
      public RenderTexture DyDxz;
      public RenderTexture DyxDyz;
      public RenderTexture DxxDzz;
      public RenderTexture displacement;
      public RenderTexture derivatives;
      public RenderTexture turbulence;

      public RenderTexture Displacement => this.displacement;

      public RenderTexture Derivatives => this.derivatives;

      public RenderTexture Turbulence => this.turbulence;

      public RenderTexture PrecomputedData => this.m_precomputedData;

      public RenderTexture InitialSpectrum => this.m_initialSpectrum;

      public WavesCascade(
        int size,
        ComputeShader initialSpectrumShader,
        ComputeShader timeDependentSpectrumShader,
        ComputeShader texturesMergerShader,
        WaterRendererFft.FastFourierTransform fft,
        Texture2D gaussianNoise)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_size = size;
        this.m_initialSpectrumShader = initialSpectrumShader;
        this.m_timeDependentSpectrumShader = timeDependentSpectrumShader;
        this.m_texturesMergerShader = texturesMergerShader;
        this.m_fft = fft;
        this.m_gaussianNoise = gaussianNoise;
        this.m_kernelInitialSpectrum = initialSpectrumShader.FindKernel("CalculateInitialSpectrum");
        this.m_kernelConjugateSpectrum = initialSpectrumShader.FindKernel("CalculateConjugatedSpectrum");
        this.m_kernelTimeDependentSpectrums = timeDependentSpectrumShader.FindKernel("CalculateAmplitudes");
        this.m_kernelResultTextures = texturesMergerShader.FindKernel("FillResultTextures");
        this.m_paramsBuffer = new ComputeBuffer(1, 16);
        this.m_initialSpectrum = createRenderTexture(size, RenderTextureFormat.ARGBFloat);
        this.m_precomputedData = createRenderTexture(size, RenderTextureFormat.ARGBFloat);
        this.displacement = createRenderTexture(size, RenderTextureFormat.ARGBFloat);
        this.derivatives = createRenderTexture(size, RenderTextureFormat.ARGBFloat, true);
        this.turbulence = createRenderTexture(size, RenderTextureFormat.ARGBFloat, true);
        this.buffer = createRenderTexture(size);
        this.DxDz = createRenderTexture(size);
        this.DyDxz = createRenderTexture(size);
        this.DyxDyz = createRenderTexture(size);
        this.DxxDzz = createRenderTexture(size);

        static RenderTexture createRenderTexture(
          int size,
          RenderTextureFormat format = RenderTextureFormat.RGFloat,
          bool useMips = false)
        {
          RenderTexture renderTexture = new RenderTexture(size, size, 0, format, RenderTextureReadWrite.Linear);
          renderTexture.useMipMap = useMips;
          renderTexture.autoGenerateMips = false;
          renderTexture.anisoLevel = 6;
          renderTexture.filterMode = FilterMode.Trilinear;
          renderTexture.wrapMode = TextureWrapMode.Repeat;
          renderTexture.enableRandomWrite = true;
          renderTexture.Create();
          return renderTexture;
        }
      }

      public void Dispose()
      {
        this.m_paramsBuffer?.Release();
        this.m_initialSpectrum.DestroyIfNotNull();
        this.m_precomputedData.DestroyIfNotNull();
        this.displacement.DestroyIfNotNull();
        this.derivatives.DestroyIfNotNull();
        this.turbulence.DestroyIfNotNull();
        this.buffer.DestroyIfNotNull();
        this.DxDz.DestroyIfNotNull();
        this.DyDxz.DestroyIfNotNull();
        this.DyxDyz.DestroyIfNotNull();
        this.DxxDzz.DestroyIfNotNull();
      }

      public void CalculateInitials(
        WaterRendererConfig wavesSettings,
        WaterRendererFft.SpectrumSettings spectrum,
        float lengthScale,
        float cutoffLow,
        float cutoffHigh)
      {
        this.m_initialSpectrumShader.SetInt(WaterRendererFft.WavesCascade.SIZE_PROP, this.m_size);
        this.m_initialSpectrumShader.SetFloat(WaterRendererFft.WavesCascade.LENGTH_SCALE_PROP, lengthScale);
        this.m_initialSpectrumShader.SetFloat(WaterRendererFft.WavesCascade.CUTOFF_LOW_PROP, cutoffLow);
        this.m_initialSpectrumShader.SetFloat(WaterRendererFft.WavesCascade.CUTOFF_HIGH_PROP, cutoffHigh);
        this.SetParametersToShader(wavesSettings, spectrum, this.m_initialSpectrumShader, this.m_kernelInitialSpectrum, this.m_paramsBuffer);
        this.m_initialSpectrumShader.SetTexture(this.m_kernelInitialSpectrum, WaterRendererFft.WavesCascade.H0K_PROP, (Texture) this.buffer);
        this.m_initialSpectrumShader.SetTexture(this.m_kernelInitialSpectrum, WaterRendererFft.WavesCascade.PRECOMPUTED_DATA_PROP, (Texture) this.m_precomputedData);
        this.m_initialSpectrumShader.SetTexture(this.m_kernelInitialSpectrum, WaterRendererFft.WavesCascade.NOISE_PROP, (Texture) this.m_gaussianNoise);
        this.m_initialSpectrumShader.Dispatch(this.m_kernelInitialSpectrum, this.m_size / 8, this.m_size / 8, 1);
        this.m_initialSpectrumShader.SetTexture(this.m_kernelConjugateSpectrum, WaterRendererFft.WavesCascade.H0_PROP, (Texture) this.m_initialSpectrum);
        this.m_initialSpectrumShader.SetTexture(this.m_kernelConjugateSpectrum, WaterRendererFft.WavesCascade.H0K_PROP, (Texture) this.buffer);
        this.m_initialSpectrumShader.Dispatch(this.m_kernelConjugateSpectrum, this.m_size / 8, this.m_size / 8, 1);
      }

      public void SetParametersToShader(
        WaterRendererConfig config,
        WaterRendererFft.SpectrumSettings spectrum,
        ComputeShader shader,
        int kernelIndex,
        ComputeBuffer paramsBuffer)
      {
        shader.SetFloat(WaterRendererFft.WavesCascade.G_PROP, config.G);
        shader.SetFloat(WaterRendererFft.WavesCascade.DEPTH_PROP, config.Depth);
        WaterRendererFft.SpectrumSettings[] andInit = ArrayPool<WaterRendererFft.SpectrumSettings>.GetAndInit(spectrum);
        paramsBuffer.SetData((Array) andInit);
        shader.SetBuffer(kernelIndex, WaterRendererFft.WavesCascade.SPECTRUMS_PROP, paramsBuffer);
        andInit.ReturnToPool<WaterRendererFft.SpectrumSettings>();
      }

      public void CalculateWavesAtTime(float time, float deltaTime)
      {
        this.m_timeDependentSpectrumShader.SetTexture(this.m_kernelTimeDependentSpectrums, WaterRendererFft.WavesCascade.Dx_Dz_PROP, (Texture) this.DxDz);
        this.m_timeDependentSpectrumShader.SetTexture(this.m_kernelTimeDependentSpectrums, WaterRendererFft.WavesCascade.Dy_Dxz_PROP, (Texture) this.DyDxz);
        this.m_timeDependentSpectrumShader.SetTexture(this.m_kernelTimeDependentSpectrums, WaterRendererFft.WavesCascade.Dyx_Dyz_PROP, (Texture) this.DyxDyz);
        this.m_timeDependentSpectrumShader.SetTexture(this.m_kernelTimeDependentSpectrums, WaterRendererFft.WavesCascade.Dxx_Dzz_PROP, (Texture) this.DxxDzz);
        this.m_timeDependentSpectrumShader.SetTexture(this.m_kernelTimeDependentSpectrums, WaterRendererFft.WavesCascade.H0_PROP, (Texture) this.m_initialSpectrum);
        this.m_timeDependentSpectrumShader.SetTexture(this.m_kernelTimeDependentSpectrums, WaterRendererFft.WavesCascade.PRECOMPUTED_DATA_PROP, (Texture) this.m_precomputedData);
        this.m_timeDependentSpectrumShader.SetFloat(WaterRendererFft.WavesCascade.TIME_PROP, time);
        this.m_timeDependentSpectrumShader.Dispatch(this.m_kernelTimeDependentSpectrums, this.m_size / 8, this.m_size / 8, 1);
        this.m_fft.IFft2D(this.DxDz, this.buffer, true, false, true);
        this.m_fft.IFft2D(this.DyDxz, this.buffer, true, false, true);
        this.m_fft.IFft2D(this.DyxDyz, this.buffer, true, false, true);
        this.m_fft.IFft2D(this.DxxDzz, this.buffer, true, false, true);
        this.m_texturesMergerShader.SetFloat(WaterRendererFft.WavesCascade.DELTA_TIME, deltaTime);
        this.m_texturesMergerShader.SetTexture(this.m_kernelResultTextures, WaterRendererFft.WavesCascade.Dx_Dz_PROP, (Texture) this.DxDz);
        this.m_texturesMergerShader.SetTexture(this.m_kernelResultTextures, WaterRendererFft.WavesCascade.Dy_Dxz_PROP, (Texture) this.DyDxz);
        this.m_texturesMergerShader.SetTexture(this.m_kernelResultTextures, WaterRendererFft.WavesCascade.Dyx_Dyz_PROP, (Texture) this.DyxDyz);
        this.m_texturesMergerShader.SetTexture(this.m_kernelResultTextures, WaterRendererFft.WavesCascade.Dxx_Dzz_PROP, (Texture) this.DxxDzz);
        this.m_texturesMergerShader.SetTexture(this.m_kernelResultTextures, WaterRendererFft.WavesCascade.DISPLACEMENT_PROP, (Texture) this.displacement);
        this.m_texturesMergerShader.SetTexture(this.m_kernelResultTextures, WaterRendererFft.WavesCascade.DERIVATIVES_PROP, (Texture) this.derivatives);
        this.m_texturesMergerShader.SetTexture(this.m_kernelResultTextures, WaterRendererFft.WavesCascade.TURBULENCE_PROP, (Texture) this.turbulence);
        this.m_texturesMergerShader.Dispatch(this.m_kernelResultTextures, this.m_size / 8, this.m_size / 8, 1);
        this.derivatives.GenerateMips();
        this.turbulence.GenerateMips();
      }

      static WavesCascade()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        WaterRendererFft.WavesCascade.DELTA_TIME = Shader.PropertyToID("DeltaTime");
        WaterRendererFft.WavesCascade.SIZE_PROP = Shader.PropertyToID("Size");
        WaterRendererFft.WavesCascade.LENGTH_SCALE_PROP = Shader.PropertyToID("LengthScale");
        WaterRendererFft.WavesCascade.CUTOFF_HIGH_PROP = Shader.PropertyToID("CutoffHigh");
        WaterRendererFft.WavesCascade.CUTOFF_LOW_PROP = Shader.PropertyToID("CutoffLow");
        WaterRendererFft.WavesCascade.NOISE_PROP = Shader.PropertyToID("Noise");
        WaterRendererFft.WavesCascade.H0_PROP = Shader.PropertyToID("H0");
        WaterRendererFft.WavesCascade.H0K_PROP = Shader.PropertyToID("H0K");
        WaterRendererFft.WavesCascade.PRECOMPUTED_DATA_PROP = Shader.PropertyToID("WavesData");
        WaterRendererFft.WavesCascade.TIME_PROP = Shader.PropertyToID("Time");
        WaterRendererFft.WavesCascade.Dx_Dz_PROP = Shader.PropertyToID("Dx_Dz");
        WaterRendererFft.WavesCascade.Dy_Dxz_PROP = Shader.PropertyToID("Dy_Dxz");
        WaterRendererFft.WavesCascade.Dyx_Dyz_PROP = Shader.PropertyToID("Dyx_Dyz");
        WaterRendererFft.WavesCascade.Dxx_Dzz_PROP = Shader.PropertyToID("Dxx_Dzz");
        WaterRendererFft.WavesCascade.DISPLACEMENT_PROP = Shader.PropertyToID(nameof (Displacement));
        WaterRendererFft.WavesCascade.DERIVATIVES_PROP = Shader.PropertyToID(nameof (Derivatives));
        WaterRendererFft.WavesCascade.TURBULENCE_PROP = Shader.PropertyToID(nameof (Turbulence));
        WaterRendererFft.WavesCascade.G_PROP = Shader.PropertyToID("GravityAcceleration");
        WaterRendererFft.WavesCascade.DEPTH_PROP = Shader.PropertyToID("Depth");
        WaterRendererFft.WavesCascade.SPECTRUMS_PROP = Shader.PropertyToID("Spectrums");
      }
    }

    public class FastFourierTransform
    {
      private static readonly int PROP_ID_PRECOMPUTE_BUFFER;
      private static readonly int PROP_ID_PRECOMPUTED_DATA;
      private static readonly int PROP_ID_BUFFER0;
      private static readonly int PROP_ID_BUFFER1;
      private static readonly int PROP_ID_SIZE;
      private static readonly int PROP_ID_STEP;
      private static readonly int PROP_ID_PINGPONG;
      private readonly int m_kernelPrecompute;
      private readonly int m_kernelHorizontalStepFft;
      private readonly int m_kernelVerticalStepFft;
      private readonly int m_kernelHorizontalStepIFft;
      private readonly int m_kernelVerticalStepIFft;
      private readonly int m_kernelScale;
      private readonly int m_kernelPermute;
      private readonly int m_size;
      private readonly ComputeShader m_fftShader;
      private readonly RenderTexture m_precomputedData;

      public FastFourierTransform(int size, ComputeShader fftShader)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_size = size;
        this.m_fftShader = fftShader;
        this.m_precomputedData = this.precomputeTwiddleFactorsAndInputIndices();
        this.m_kernelPrecompute = fftShader.FindKernel("PrecomputeTwiddleFactorsAndInputIndices");
        this.m_kernelHorizontalStepFft = fftShader.FindKernel("HorizontalStepFFT");
        this.m_kernelVerticalStepFft = fftShader.FindKernel("VerticalStepFFT");
        this.m_kernelHorizontalStepIFft = fftShader.FindKernel("HorizontalStepInverseFFT");
        this.m_kernelVerticalStepIFft = fftShader.FindKernel("VerticalStepInverseFFT");
        this.m_kernelScale = fftShader.FindKernel("Scale");
        this.m_kernelPermute = fftShader.FindKernel("Permute");
      }

      public void Fft2D(RenderTexture input, RenderTexture buffer, bool outputToInput = false)
      {
        int num = (int) Mathf.Log((float) this.m_size, 2f);
        bool val1 = false;
        this.m_fftShader.SetTexture(this.m_kernelHorizontalStepFft, WaterRendererFft.FastFourierTransform.PROP_ID_PRECOMPUTED_DATA, (Texture) this.m_precomputedData);
        this.m_fftShader.SetTexture(this.m_kernelHorizontalStepFft, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER0, (Texture) input);
        this.m_fftShader.SetTexture(this.m_kernelHorizontalStepFft, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER1, (Texture) buffer);
        for (int val2 = 0; val2 < num; ++val2)
        {
          val1 = !val1;
          this.m_fftShader.SetInt(WaterRendererFft.FastFourierTransform.PROP_ID_STEP, val2);
          this.m_fftShader.SetBool(WaterRendererFft.FastFourierTransform.PROP_ID_PINGPONG, val1);
          this.m_fftShader.Dispatch(this.m_kernelHorizontalStepFft, this.m_size / 8, this.m_size / 8, 1);
        }
        this.m_fftShader.SetTexture(this.m_kernelVerticalStepFft, WaterRendererFft.FastFourierTransform.PROP_ID_PRECOMPUTED_DATA, (Texture) this.m_precomputedData);
        this.m_fftShader.SetTexture(this.m_kernelVerticalStepFft, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER0, (Texture) input);
        this.m_fftShader.SetTexture(this.m_kernelVerticalStepFft, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER1, (Texture) buffer);
        for (int val3 = 0; val3 < num; ++val3)
        {
          val1 = !val1;
          this.m_fftShader.SetInt(WaterRendererFft.FastFourierTransform.PROP_ID_STEP, val3);
          this.m_fftShader.SetBool(WaterRendererFft.FastFourierTransform.PROP_ID_PINGPONG, val1);
          this.m_fftShader.Dispatch(this.m_kernelVerticalStepFft, this.m_size / 8, this.m_size / 8, 1);
        }
        if (val1 & outputToInput)
          Graphics.Blit((Texture) buffer, input);
        if (val1 || outputToInput)
          return;
        Graphics.Blit((Texture) input, buffer);
      }

      /// <summary>Computes inverse FFT using compute shaders.</summary>
      public void IFft2D(
        RenderTexture input,
        RenderTexture buffer,
        bool outputToInput,
        bool scale,
        bool permute)
      {
        int num = (int) Mathf.Log((float) this.m_size, 2f);
        bool val1 = false;
        this.m_fftShader.SetTexture(this.m_kernelHorizontalStepIFft, WaterRendererFft.FastFourierTransform.PROP_ID_PRECOMPUTED_DATA, (Texture) this.m_precomputedData);
        this.m_fftShader.SetTexture(this.m_kernelHorizontalStepIFft, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER0, (Texture) input);
        this.m_fftShader.SetTexture(this.m_kernelHorizontalStepIFft, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER1, (Texture) buffer);
        for (int val2 = 0; val2 < num; ++val2)
        {
          val1 = !val1;
          this.m_fftShader.SetInt(WaterRendererFft.FastFourierTransform.PROP_ID_STEP, val2);
          this.m_fftShader.SetBool(WaterRendererFft.FastFourierTransform.PROP_ID_PINGPONG, val1);
          this.m_fftShader.Dispatch(this.m_kernelHorizontalStepIFft, this.m_size / 8, this.m_size / 8, 1);
        }
        this.m_fftShader.SetTexture(this.m_kernelVerticalStepIFft, WaterRendererFft.FastFourierTransform.PROP_ID_PRECOMPUTED_DATA, (Texture) this.m_precomputedData);
        this.m_fftShader.SetTexture(this.m_kernelVerticalStepIFft, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER0, (Texture) input);
        this.m_fftShader.SetTexture(this.m_kernelVerticalStepIFft, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER1, (Texture) buffer);
        for (int val3 = 0; val3 < num; ++val3)
        {
          val1 = !val1;
          this.m_fftShader.SetInt(WaterRendererFft.FastFourierTransform.PROP_ID_STEP, val3);
          this.m_fftShader.SetBool(WaterRendererFft.FastFourierTransform.PROP_ID_PINGPONG, val1);
          this.m_fftShader.Dispatch(this.m_kernelVerticalStepIFft, this.m_size / 8, this.m_size / 8, 1);
        }
        if (val1 & outputToInput)
          Graphics.Blit((Texture) buffer, input);
        if (!val1 && !outputToInput)
          Graphics.Blit((Texture) input, buffer);
        if (permute)
        {
          this.m_fftShader.SetInt(WaterRendererFft.FastFourierTransform.PROP_ID_SIZE, this.m_size);
          this.m_fftShader.SetTexture(this.m_kernelPermute, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER0, outputToInput ? (Texture) input : (Texture) buffer);
          this.m_fftShader.Dispatch(this.m_kernelPermute, this.m_size / 8, this.m_size / 8, 1);
        }
        if (!scale)
          return;
        this.m_fftShader.SetInt(WaterRendererFft.FastFourierTransform.PROP_ID_SIZE, this.m_size);
        this.m_fftShader.SetTexture(this.m_kernelScale, WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER0, outputToInput ? (Texture) input : (Texture) buffer);
        this.m_fftShader.Dispatch(this.m_kernelScale, this.m_size / 8, this.m_size / 8, 1);
      }

      private RenderTexture precomputeTwiddleFactorsAndInputIndices()
      {
        int num = (int) Mathf.Log((float) this.m_size, 2f);
        RenderTexture renderTexture = new RenderTexture(num, this.m_size, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.wrapMode = TextureWrapMode.Repeat;
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        this.m_fftShader.SetInt(WaterRendererFft.FastFourierTransform.PROP_ID_SIZE, this.m_size);
        this.m_fftShader.SetTexture(this.m_kernelPrecompute, WaterRendererFft.FastFourierTransform.PROP_ID_PRECOMPUTE_BUFFER, (Texture) renderTexture);
        this.m_fftShader.Dispatch(this.m_kernelPrecompute, num, this.m_size / 2 / 8, 1);
        return renderTexture;
      }

      static FastFourierTransform()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        WaterRendererFft.FastFourierTransform.PROP_ID_PRECOMPUTE_BUFFER = Shader.PropertyToID("PrecomputeBuffer");
        WaterRendererFft.FastFourierTransform.PROP_ID_PRECOMPUTED_DATA = Shader.PropertyToID("PrecomputedData");
        WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER0 = Shader.PropertyToID("Buffer0");
        WaterRendererFft.FastFourierTransform.PROP_ID_BUFFER1 = Shader.PropertyToID("Buffer1");
        WaterRendererFft.FastFourierTransform.PROP_ID_SIZE = Shader.PropertyToID("Size");
        WaterRendererFft.FastFourierTransform.PROP_ID_STEP = Shader.PropertyToID("Step");
        WaterRendererFft.FastFourierTransform.PROP_ID_PINGPONG = Shader.PropertyToID("PingPong");
      }
    }

    public class WaterRendererDebugMb : MonoBehaviour
    {
      public int SimSteps;
      public int SimStepSkipped;
      public int RenderedChunks;
      public int RenderedChunksFar;
      public Material OceanMaterial;
      public Material OceanMaterialFar;
      public Texture2D OceanTexture;
      public bool ApplySettingsEveryFrame;
      public WaterRendererConfig Config;
      public RenderTexture InitialSpectrumC0;
      public RenderTexture InitialSpectrumC1;
      public RenderTexture PrecomputedDataC0;
      public RenderTexture PrecomputedDataC1;
      private WaterRendererFft m_renderer;

      public void Initialize(WaterRendererFft renderer)
      {
        this.m_renderer = renderer;
        this.Config = renderer.m_config;
      }

      public void Update()
      {
        this.OceanMaterial = this.m_renderer.m_oceanMaterial;
        this.OceanMaterialFar = this.m_renderer.m_oceanMaterialFar;
        this.OceanTexture = this.m_renderer.m_oceanTexture;
        this.InitialSpectrumC0 = this.m_renderer.m_cascade0.InitialSpectrum;
        this.InitialSpectrumC1 = this.m_renderer.m_cascade1.InitialSpectrum;
        this.PrecomputedDataC0 = this.m_renderer.m_cascade0.PrecomputedData;
        this.PrecomputedDataC1 = this.m_renderer.m_cascade1.PrecomputedData;
        if (!this.ApplySettingsEveryFrame)
          return;
        this.m_renderer.initialiseCascades(this.m_renderer.createSettingsStruct(this.Config.DefaultSpectrum, this.Config.G));
      }

      public WaterRendererDebugMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
