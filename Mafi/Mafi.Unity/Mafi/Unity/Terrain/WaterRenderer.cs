// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.WaterRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public class WaterRenderer : IWaterRenderer, IRenderedChunkAlwaysVisible, IRenderedChunksBase
  {
    private static readonly int SHADER_SPECULARITY_ID;
    private static readonly ObjectPool<byte[]> s_updateAllDataPool;
    private readonly AssetsDb m_assetsDb;
    private readonly IGameLoopEvents m_gameLoopEvents;
    private readonly ISimLoopEvents m_simLoopEvents;
    private readonly TerrainManager m_terrain;
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly ITerrainRenderer m_terrainRenderer;
    private Option<WaterRendererManager> m_parentManager;
    private WaterRenderer.WaterRendererChunk[] m_renderChunks;
    private float m_originalWaterSpec;
    private InstancedMeshesRenderer<WaterRenderer.OceanChunkInstanceData> m_instancedOceanRenderer;
    private Material m_oceanMaterial;
    private Texture2D m_oceanTexture;
    private bool m_isOceanRenderingEnabled;

    public string Name => "Water";

    public WaterRenderer(
      IGameLoopEvents gameLoopEvents,
      ISimLoopEvents simLoopEvents,
      AssetsDb assets,
      TerrainManager terrainManager,
      ChunkBasedRenderingManager chunksRenderer,
      ReloadAfterAssetUpdateManager reloadManager,
      ITerrainRenderer terrainRenderer)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_isOceanRenderingEnabled = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_gameLoopEvents = gameLoopEvents;
      this.m_simLoopEvents = simLoopEvents;
      this.m_terrain = terrainManager;
      this.m_chunksRenderer = chunksRenderer;
      this.m_reloadManager = reloadManager;
      this.m_terrainRenderer = terrainRenderer;
      this.m_assetsDb = assets;
      this.m_renderChunks = new WaterRenderer.WaterRendererChunk[chunksRenderer.ChunksCountTotal];
    }

    public void SetOceanRenderingState(bool isEnabled)
    {
      this.m_isOceanRenderingEnabled = isEnabled;
    }

    public bool TryInitializeAndRegister(WaterRendererManager manager)
    {
      this.m_parentManager = (Option<WaterRendererManager>) manager;
      Texture2D texture2D = new Texture2D(this.m_terrain.TerrainWidth, this.m_terrain.TerrainHeight, TextureFormat.R8, false, true);
      texture2D.name = "OceanAlpha";
      texture2D.anisoLevel = 0;
      texture2D.filterMode = FilterMode.Bilinear;
      texture2D.wrapMode = TextureWrapMode.Clamp;
      this.m_oceanTexture = texture2D;
      Vector4 vector4 = new Vector4((float) this.m_terrain.TerrainWidth, (float) this.m_terrain.TerrainHeight, 1f / (float) this.m_terrain.TerrainWidth, 1f / (float) this.m_terrain.TerrainHeight);
      Mesh oceanChunkMesh = WaterRenderer.createOceanChunkMesh();
      this.m_oceanMaterial = this.m_assetsDb.GetClonedMaterial("Assets/Base/Water/Ocean.mat");
      this.m_oceanMaterial.SetTexture("_OceanMask", (Texture) this.m_oceanTexture);
      this.m_oceanMaterial.SetVector("_TerrainSize", vector4);
      this.m_originalWaterSpec = this.m_oceanMaterial.GetFloat(WaterRenderer.SHADER_SPECULARITY_ID);
      Shader.SetGlobalTexture("_Mafi_OceanMask", (Texture) this.m_oceanTexture);
      if (this.m_renderChunks.Length != this.m_chunksRenderer.ChunksCountTotal)
        this.m_renderChunks = new WaterRenderer.WaterRendererChunk[this.m_chunksRenderer.ChunksCountTotal];
      for (int index = 0; index < this.m_renderChunks.Length; ++index)
      {
        WaterRenderer.WaterRendererChunk newChunk = new WaterRenderer.WaterRendererChunk(this.m_chunksRenderer.ExtendChunkCoord(new Chunk256Index((ushort) index)), this);
        this.m_renderChunks[index] = newChunk;
        this.m_chunksRenderer.RegisterChunk((IRenderedChunk) newChunk);
        newChunk.InitializeData();
      }
      this.m_oceanTexture.Apply(false, true);
      this.m_instancedOceanRenderer = new InstancedMeshesRenderer<WaterRenderer.OceanChunkInstanceData>(LodUtils.SameMeshForAllLods(oceanChunkMesh), this.m_oceanMaterial, layer: Layer.Unity04Water);
      this.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_instancedOceanRenderer);
      float unityUnits1 = (float) 4096.TilesToUnityUnits();
      float unityUnits2 = (float) this.m_terrain.TerrainWidth.TilesToUnityUnits();
      float unityUnits3 = (float) this.m_terrain.TerrainHeight.TilesToUnityUnits();
      int num1 = (int) this.m_instancedOceanRenderer.AddInstance(new WaterRenderer.OceanChunkInstanceData(new Vector2(0.0f, -unityUnits1), new Vector2(unityUnits2 + unityUnits1, unityUnits1)));
      int num2 = (int) this.m_instancedOceanRenderer.AddInstance(new WaterRenderer.OceanChunkInstanceData(new Vector2(unityUnits2, 0.0f), new Vector2(unityUnits1, unityUnits3 + unityUnits1)));
      int num3 = (int) this.m_instancedOceanRenderer.AddInstance(new WaterRenderer.OceanChunkInstanceData(new Vector2(-unityUnits1, unityUnits3), new Vector2(unityUnits2 + unityUnits1, unityUnits1)));
      int num4 = (int) this.m_instancedOceanRenderer.AddInstance(new WaterRenderer.OceanChunkInstanceData(new Vector2(-unityUnits1, -unityUnits1), new Vector2(unityUnits1, unityUnits3 + unityUnits1)));
      this.m_chunksRenderer.RegisterChunkAlwaysRender((IRenderedChunkAlwaysVisible) this);
      this.m_terrain.OceanFlagChanged.AddNonSaveable<WaterRenderer>(this, new Action<Tile2iAndIndex>(this.oceanChanged));
      this.m_gameLoopEvents.RenderUpdate.AddNonSaveable<WaterRenderer>(this, new Action<GameTime>(this.renderUpdate));
      this.m_simLoopEvents.UpdateEndForUi.AddNonSaveable<WaterRenderer>(this, new Action(this.simUpdate));
      return true;
    }

    public void UnregisterAndDispose()
    {
      Assert.That<Option<WaterRendererManager>>(this.m_parentManager).HasValue<WaterRendererManager>();
      this.m_terrain.OceanFlagChanged.RemoveNonSaveable<WaterRenderer>(this, new Action<Tile2iAndIndex>(this.oceanChanged));
      this.m_gameLoopEvents.RenderUpdate.RemoveNonSaveable<WaterRenderer>(this, new Action<GameTime>(this.renderUpdate));
      this.m_simLoopEvents.UpdateEndForUi.RemoveNonSaveable<WaterRenderer>(this, new Action(this.simUpdate));
      this.m_chunksRenderer.UnregisterChunkAlwaysRender((IRenderedChunkAlwaysVisible) this);
      foreach (WaterRenderer.WaterRendererChunk renderChunk in this.m_renderChunks)
      {
        this.m_chunksRenderer.TryUnregisterChunk((IRenderedChunk) renderChunk);
        renderChunk.Dispose();
      }
      this.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<WaterRenderer.OceanChunkInstanceData>>(this.m_instancedOceanRenderer);
      this.m_oceanMaterial.DestroyIfNotNull();
      this.m_oceanTexture.DestroyIfNotNull();
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

    private void oceanChanged(Tile2iAndIndex tileAndIndex)
    {
      this.m_renderChunks[(int) this.m_chunksRenderer.GetChunkIndex(tileAndIndex.TileCoord).Value].OceanChanged(TileInChunk256.FromTile(tileAndIndex.TileCoord), this.m_terrain.IsOcean(tileAndIndex.Index));
    }

    private static Mesh createOceanChunkMesh()
    {
      Mesh mesh = new Mesh();
      mesh.name = "Terrain chunk";
      Mesh oceanChunkMesh = mesh;
      oceanChunkMesh.SetVertices(new Vector3[4]
      {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(1f, 0.0f, 0.0f),
        new Vector3(1f, 0.0f, 1f),
        new Vector3(0.0f, 0.0f, 1f)
      });
      oceanChunkMesh.SetIndices(new ushort[6]
      {
        (ushort) 0,
        (ushort) 1,
        (ushort) 2,
        (ushort) 0,
        (ushort) 2,
        (ushort) 3
      }, MeshTopology.Triangles, 0);
      oceanChunkMesh.Optimize();
      oceanChunkMesh.UploadMeshData(true);
      return oceanChunkMesh;
    }

    private void renderUpdate(GameTime time)
    {
      this.m_instancedOceanRenderer.ClearFromIndex(4);
      WaterRenderer.WaterRendererChunk[] renderChunks = this.m_renderChunks;
      int index = 0;
      while (index < renderChunks.Length && !renderChunks[index].RenderUpdate())
        ++index;
    }

    private void simUpdate()
    {
      int num = 0;
      foreach (WaterRenderer.WaterRendererChunk renderChunk in this.m_renderChunks)
      {
        num += renderChunk.ProcessChangesOnSim();
        if (num > 16384)
          break;
      }
    }

    public float GetSpecularIntensity()
    {
      return this.m_oceanMaterial.GetFloat(WaterRenderer.SHADER_SPECULARITY_ID) / this.m_originalWaterSpec;
    }

    public void SetSpecularIntensity(float percent)
    {
      this.m_oceanMaterial.SetFloat(WaterRenderer.SHADER_SPECULARITY_ID, this.m_originalWaterSpec * percent);
    }

    public void SetLightCookieEnabled(bool enabled)
    {
      if (enabled)
      {
        this.m_oceanMaterial.SetShaderPassEnabled("ForwardBase", false);
        this.m_oceanMaterial.SetShaderPassEnabled("ForwardAdd", true);
      }
      else
      {
        this.m_oceanMaterial.SetShaderPassEnabled("ForwardBase", true);
        this.m_oceanMaterial.SetShaderPassEnabled("ForwardAdd", false);
      }
    }

    public void SetOpacity(float opacity)
    {
      this.m_oceanMaterial.SetFloat("_WaterOpacity", opacity);
    }

    public RenderStats RenderAlwaysVisible(GameTime time, Bounds bounds)
    {
      StateAssert.IsInGameState(GameLoopState.RenderUpdateEnd);
      return !this.m_isOceanRenderingEnabled ? new RenderStats() : this.m_instancedOceanRenderer.Render(bounds, 0);
    }

    public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
    {
      info.Add(new RenderedInstancesInfo("Ocean chunks", ((IEnumerable<WaterRenderer.WaterRendererChunk>) this.m_renderChunks).AsEnumerable<WaterRenderer.WaterRendererChunk>().Count<WaterRenderer.WaterRendererChunk>((Func<WaterRenderer.WaterRendererChunk, bool>) (x => x.HasOcean)), 6));
    }

    private byte[] getAllTilesChunkDataPooled() => WaterRenderer.s_updateAllDataPool.GetInstance();

    private void returnAllTilesChunkDataToPool(byte[] data)
    {
      WaterRenderer.s_updateAllDataPool.ReturnInstance(data);
      Assert.That<int>(WaterRenderer.s_updateAllDataPool.CountStoredInstances()).IsLessOrEqual(3);
    }

    static WaterRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      WaterRenderer.SHADER_SPECULARITY_ID = Shader.PropertyToID("_Specularity");
      WaterRenderer.s_updateAllDataPool = new ObjectPool<byte[]>(4, (Func<byte[]>) (() => new byte[65536]), (Action<byte[]>) (_ => { }));
    }

    [ExpectedStructSize(16)]
    [StructLayout(LayoutKind.Explicit)]
    private readonly struct OceanChunkInstanceData
    {
      [FieldOffset(0)]
      public readonly Vector2 Origin;
      [FieldOffset(8)]
      public readonly Vector2 Size;

      public OceanChunkInstanceData(Vector2 origin, Vector2 size)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Origin = origin;
        this.Size = size;
      }
    }

    private sealed class WaterRendererChunk : IRenderedChunk, IRenderedChunksBase, IDisposable
    {
      private readonly Tile2i m_originTile;
      private readonly Tile2iIndex m_originTileIndex;
      private readonly WaterRenderer m_parentRenderer;
      private Texture2D m_oceanTex;
      private LystStruct<Pair<ushort, byte>> m_oceanTexDataUpdates;
      private LystStruct<Pair<TileInChunk256, bool>> m_changedOcean;
      private Option<byte[]> m_updateAllData;
      private int m_ticksSinceLastOceanUpdate;
      private int m_currentLod;
      private int m_oceanTilesCount;
      private bool m_hasOcean;
      private bool m_hasOceanOnSim;
      private volatile bool m_needsOceanTexDataUpload;
      private bool m_allTilesChanged;

      public string Name => "Water";

      public Chunk256AndIndex CoordAndIndex { get; }

      public Vector2 Origin { get; }

      public bool TrackStoppedRendering => true;

      public float MaxModelDeviationFromChunkBounds => 4f;

      public Vector2 MinMaxHeight => new Vector2(-2f, 2f);

      public bool HasOcean => this.m_hasOcean;

      public WaterRendererChunk(Chunk256AndIndex coordAndIndex, WaterRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.CoordAndIndex = coordAndIndex;
        this.Origin = coordAndIndex.OriginTile2i.ToVector2();
        this.m_originTile = coordAndIndex.OriginTile2i;
        this.m_parentRenderer = parentRenderer;
        this.m_originTileIndex = parentRenderer.m_terrain.GetTileIndex(this.m_originTile);
      }

      public void InitializeData()
      {
        TerrainManager terrain = this.m_parentRenderer.m_terrain;
        NativeArray<byte> rawTextureData = this.m_parentRenderer.m_oceanTexture.GetRawTextureData<byte>();
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
          this.m_hasOcean = this.m_hasOceanOnSim = this.m_oceanTilesCount > 0;
        }
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
        if (!this.m_parentRenderer.m_isOceanRenderingEnabled || !this.m_hasOcean)
          return new RenderStats();
        int num = (int) this.m_parentRenderer.m_instancedOceanRenderer.AddInstance(new WaterRenderer.OceanChunkInstanceData(this.Origin, new Vector2(512f, 512f)));
        return new RenderStats();
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
          if (this.m_ticksSinceLastOceanUpdate >= (this.m_allTilesChanged ? 2 : this.m_parentRenderer.m_terrainRenderer.UpdateDelayPerLod[this.m_currentLod]) && !this.m_needsOceanTexDataUpload)
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
          byte[] tilesChunkDataPooled = this.m_parentRenderer.getAllTilesChunkDataPooled();
          this.m_updateAllData = (Option<byte[]>) tilesChunkDataPooled;
          TerrainManager terrain = this.m_parentRenderer.m_terrain;
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
          Texture2D texture2D = new Texture2D(256, 256, this.m_parentRenderer.m_oceanTexture.format, false, true);
          texture2D.name = "OceanAlphaUpdate";
          texture2D.anisoLevel = 0;
          this.m_oceanTex = texture2D;
          rawTextureData = this.m_oceanTex.GetRawTextureData<byte>();
          TerrainManager terrain = this.m_parentRenderer.m_terrain;
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
          this.m_parentRenderer.returnAllTilesChunkDataToPool(valueOrNull);
        }
        else
          Assert.That<int>(this.m_oceanTexDataUpdates.Count).IsPositive();
        foreach (Pair<ushort, byte> oceanTexDataUpdate in this.m_oceanTexDataUpdates)
          rawTextureData[(int) oceanTexDataUpdate.First] = oceanTexDataUpdate.Second;
        this.m_oceanTexDataUpdates.ClearSkipZeroingMemory();
        if (this.m_oceanTexDataUpdates.Capacity >= 65536)
          this.m_oceanTexDataUpdates = new LystStruct<Pair<ushort, byte>>();
        this.m_oceanTex.Apply(false, false);
        Graphics.CopyTexture((Texture) this.m_oceanTex, 0, 0, 0, 0, 256, 256, (Texture) this.m_parentRenderer.m_oceanTexture, 0, 0, this.m_originTile.X, this.m_originTile.Y);
      }
    }
  }
}
