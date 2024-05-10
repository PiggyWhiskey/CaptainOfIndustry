// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Particles.DustParticlesController
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Environment;
using Mafi.Core.GameLoop;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Localization;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Particles
{
  /// <summary>Handles rendering for dust particles.</summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  [HasRenderingSettings]
  public class DustParticlesController : IDisposable
  {
    private static readonly int BASE_PARTICLES_PER_EMIT;
    private static float s_particlesPerEmitForQuality;
    public static readonly LocStr ParticlesRenderingQuality;
    public static readonly RenderingSetting ParticleRenderingSetting;
    private readonly TerrainManager m_terrainManager;
    private readonly WeatherManager m_weatherManager;
    private readonly ImmutableArray<DustinessData> m_dustinessPerMaterial;
    private readonly ParticleSystem m_clonableParticleSystem;
    private Set<Pair<Tile2iAndIndex, HeightTilesF>> m_heightChangedTilesOnSim;
    private Set<Pair<Tile2iAndIndex, HeightTilesF>> m_heightChangedTilesOnSync;
    private bool m_syncSinceRender;
    private int m_lastSeenGameSpeedMult;
    private readonly IRandom m_random;
    private readonly Lyst<ParticleSystem> m_allParticleSystems;
    private readonly ParticleSystem[] m_particleSystems;

    private static void onQualityChanged(RenderingSetting setting)
    {
      DustParticlesController.s_particlesPerEmitForQuality = (float) (DustParticlesController.BASE_PARTICLES_PER_EMIT * setting.CurrentOption.Value) / 10f;
    }

    public DustParticlesController(
      IGameLoopEvents gameLoopEvents,
      TerrainManager terrainManager,
      WeatherManager weatherManager,
      AssetsDb assetsDb,
      RandomProvider randomProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_heightChangedTilesOnSim = new Set<Pair<Tile2iAndIndex, HeightTilesF>>();
      this.m_heightChangedTilesOnSync = new Set<Pair<Tile2iAndIndex, HeightTilesF>>();
      this.m_allParticleSystems = new Lyst<ParticleSystem>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_weatherManager = weatherManager;
      this.m_random = randomProvider.GetNonSimRandomFor((object) this);
      this.m_particleSystems = new ParticleSystem[terrainManager.TerrainHeight * terrainManager.TerrainWidth / 4096];
      this.m_dustinessPerMaterial = terrainManager.TerrainMaterials.Map<DustinessData>((Func<TerrainMaterialProto, DustinessData>) (x => new DustinessData(x.Graphics.Dustiness, x.Graphics.DustColor.AsColor())));
      GameObject prefab;
      if (!assetsDb.TryGetClonedPrefab("Assets/Base/Terrain/Dust/TerrainDustParticleSystem.prefab", out prefab))
        Log.Warning("Failed to get particles for terrain material.");
      ParticleSystem component = prefab.GetComponent<ParticleSystem>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        Log.Warning("Find particles MB for terrain material.");
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        prefab.SetActive(true);
        component.Play();
        ParticleSystem.MainModule main = component.main;
        float constant = main.startLifetime.constant;
        main.startLifetime = (ParticleSystem.MinMaxCurve) constant;
        main.startSize = (ParticleSystem.MinMaxCurve) main.startSize.constant;
        main.maxParticles = 512;
        main.loop = false;
        component.emission.rateOverTime = (ParticleSystem.MinMaxCurve) 0.0f;
        this.m_clonableParticleSystem = component;
      }
      this.m_terrainManager.HeightChanged.AddNonSaveable<DustParticlesController>(this, new Action<Tile2iAndIndex>(this.terrainHeightChangedOnSim));
      gameLoopEvents.SyncUpdateEnd.AddNonSaveable<DustParticlesController>(this, new Action<GameTime>(this.syncUpdate));
      gameLoopEvents.RenderUpdate.AddNonSaveable<DustParticlesController>(this, new Action<GameTime>(this.renderUpdate));
    }

    private void terrainHeightChangedOnSim(Tile2iAndIndex tileAndIndex)
    {
      HeightTilesF height = this.m_terrainManager.GetHeight(tileAndIndex.Index);
      this.m_heightChangedTilesOnSim.Add(new Pair<Tile2iAndIndex, HeightTilesF>(tileAndIndex, height));
    }

    private void syncUpdate(GameTime time)
    {
      Swap.Them<Set<Pair<Tile2iAndIndex, HeightTilesF>>>(ref this.m_heightChangedTilesOnSim, ref this.m_heightChangedTilesOnSync);
      this.m_heightChangedTilesOnSim.Clear();
      this.m_syncSinceRender = true;
    }

    private void renderUpdate(GameTime time)
    {
      if (!this.m_syncSinceRender)
        return;
      this.m_syncSinceRender = false;
      if (time.GameSpeedMult != this.m_lastSeenGameSpeedMult)
      {
        this.m_clonableParticleSystem.main.simulationSpeed = (float) time.GameSpeedMult;
        foreach (ParticleSystem allParticleSystem in this.m_allParticleSystems)
          allParticleSystem.main.simulationSpeed = (float) time.GameSpeedMult;
        this.m_lastSeenGameSpeedMult = time.GameSpeedMult;
      }
      if (this.m_heightChangedTilesOnSync.IsEmpty)
        return;
      foreach (Pair<Tile2iAndIndex, HeightTilesF> pair in this.m_heightChangedTilesOnSync)
      {
        Tile2iAndIndex first = pair.First;
        if (!this.m_terrainManager.IsOcean(first.Index))
        {
          DustinessData dustiness = this.ComputeDustiness(first);
          if ((double) dustiness.Dustiness > 0.0099999997764825821)
          {
            float lifetimeMod = (float) (1.0 - (double) this.m_weatherManager.RainIntensity.ToFloat() * 0.75);
            dustiness = new DustinessData(dustiness.Dustiness * lifetimeMod, dustiness.DustColor);
            this.EmitDustAt(first.TileCoord.CenterTile2f.ExtendHeight(pair.Second).ToVector3(), dustiness, lifetimeMod: lifetimeMod);
          }
        }
      }
      this.m_heightChangedTilesOnSync.Clear();
    }

    private int getIndexForChunkCoord(Chunk2i chunkCoord)
    {
      return chunkCoord.Y * (this.m_terrainManager.TerrainWidth >> 6) + chunkCoord.X;
    }

    public void EmitDustAt(
      Vector3 position,
      DustinessData dustiness,
      bool addNoise = true,
      float lifetimeMod = 1f)
    {
      int indexForChunkCoord = this.getIndexForChunkCoord(position.ToTile3f().Tile2i.ChunkCoord2i);
      if (indexForChunkCoord >= this.m_particleSystems.Length)
      {
        Log.Error(string.Format("Trying to emit dust outside of map {0}", (object) position));
      }
      else
      {
        ParticleSystem particleSystem = this.m_particleSystems[indexForChunkCoord];
        if ((UnityEngine.Object) particleSystem == (UnityEngine.Object) null)
        {
          particleSystem = UnityEngine.Object.Instantiate<ParticleSystem>(this.m_clonableParticleSystem);
          this.m_particleSystems[indexForChunkCoord] = particleSystem;
          this.m_allParticleSystems.Add(particleSystem);
        }
        ParticleSystem.MainModule main;
        if (particleSystem.main.maxParticles < 16384)
        {
          int particleCount = particleSystem.particleCount;
          main = particleSystem.main;
          int maxParticles = main.maxParticles;
          if (particleCount >= maxParticles)
            particleSystem.main.maxParticles *= 2;
        }
        Vector3 vector3 = position;
        int num1 = (DustParticlesController.s_particlesPerEmitForQuality * dustiness.Dustiness).FloorToInt();
        for (int index = 0; index < num1; ++index)
        {
          ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();
          emitParams.position = vector3;
          ref ParticleSystem.EmitParams local = ref emitParams;
          main = particleSystem.main;
          double num2 = (double) main.startLifetime.constant * (double) lifetimeMod;
          local.startLifetime = (float) num2;
          if (addNoise)
            emitParams.position += new Vector3(this.m_random.NextFloat() - 0.5f, this.m_random.NextFloat() - 0.5f, this.m_random.NextFloat() - 0.5f) * 2f;
          emitParams.startColor = (Color32) dustiness.DustColor;
          particleSystem.Emit(emitParams, 1);
        }
      }
    }

    public DustinessData ComputeDustiness(DrivingEntity groundEntity)
    {
      if (groundEntity.IsDrivingOnRoad)
        return new DustinessData();
      Tile2iAndIndex tileAndIndex = groundEntity.Terrain.ExtendTileIndex(groundEntity.GroundPositionTile2i);
      if (groundEntity.SurfaceProvider.GetEntityVehicleSurfaceAt(tileAndIndex.TileCoord, out bool _).HasValue)
        return new DustinessData();
      TileSurfaceData tileSurface = this.m_terrainManager.GetTileSurface(tileAndIndex.Index);
      if (!tileSurface.IsValid || !(tileSurface.Height + ThicknessTilesF.Quarter >= groundEntity.Terrain.GetHeight(tileAndIndex.Index)))
        return this.ComputeDustiness(tileAndIndex);
      TerrainTileSurfaceProto.Gfx graphics = tileSurface.ResolveToProto(this.m_terrainManager).Graphics;
      return new DustinessData(graphics.DustinessPerc, graphics.DustColor.ToColor());
    }

    public DustinessData ComputeDustiness(Tile2iAndIndex tileAndIndex)
    {
      TileMaterialLayers layersRawData = this.m_terrainManager.GetLayersRawData(tileAndIndex.Index);
      if (layersRawData.Count <= 0)
        return this.m_dustinessPerMaterial[(int) this.m_terrainManager.Bedrock.SlimId.Value];
      DustinessData data = this.m_dustinessPerMaterial[layersRawData.First.SlimIdRaw];
      if (layersRawData.First.Thickness >= ThicknessTilesF.One)
        return data;
      float weight1 = layersRawData.First.Thickness.Value.ToFloat();
      if (layersRawData.Count == 1)
        return this.m_dustinessPerMaterial[(int) this.m_terrainManager.Bedrock.SlimId.Value].Lerp(data, weight1);
      float num = layersRawData.Second.Thickness.Value.ToFloat();
      DustinessData dustinessData = this.m_dustinessPerMaterial[layersRawData.Second.SlimIdRaw];
      float weight2 = weight1 + num;
      if ((double) weight2 >= 1.0)
        return dustinessData.Lerp(data, weight1);
      return layersRawData.Count == 2 ? this.m_dustinessPerMaterial[(int) this.m_terrainManager.Bedrock.SlimId.Value].Lerp(dustinessData.Lerp(data, weight1 / weight2), weight2) : this.m_dustinessPerMaterial[layersRawData.Third.SlimIdRaw].Lerp(dustinessData.Lerp(data, weight1 / weight2), weight2);
    }

    public void Dispose()
    {
      foreach (UnityEngine.Object particleSystem in this.m_particleSystems)
        UnityEngine.Object.Destroy(particleSystem);
    }

    static DustParticlesController()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      DustParticlesController.BASE_PARTICLES_PER_EMIT = 5;
      DustParticlesController.s_particlesPerEmitForQuality = (float) DustParticlesController.BASE_PARTICLES_PER_EMIT;
      DustParticlesController.ParticlesRenderingQuality = Loc.Str(nameof (ParticlesRenderingQuality), "Particles quality", "title for particle quality setting");
      LocStr renderingQuality = DustParticlesController.ParticlesRenderingQuality;
      ImmutableArray<RenderingSettingOption> options = ImmutableArray.Create<RenderingSettingOption>(new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__High, 10, RenderingSettingPreset.HighQuality | RenderingSettingPreset.UltraQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Medium, 7, RenderingSettingPreset.MediumQuality), new RenderingSettingOption((LocStrFormatted) Tr.RenderingQuality__Low, 4, RenderingSettingPreset.LowQuality));
      Action<RenderingSetting> action = new Action<RenderingSetting>(DustParticlesController.onQualityChanged);
      int? defaultSettingIndex = new int?();
      Action<RenderingSetting> defaultCallback = action;
      DustParticlesController.ParticleRenderingSetting = new RenderingSetting(nameof (ParticleRenderingSetting), renderingQuality, 110, options, defaultSettingIndex, defaultCallback);
    }
  }
}
