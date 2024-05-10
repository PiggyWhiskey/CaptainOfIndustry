// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InstancedRendering.ChunkBasedRenderingManager
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Console;
using Mafi.Core.GameLoop;
using Mafi.Core.Numerics;
using Mafi.Core.Terrain;
using Mafi.Unity.Camera;
using Mafi.Unity.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InstancedRendering
{
  /// <summary>
  /// Handles rendering of chunks that are visible in the camera.
  /// </summary>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public sealed class ChunkBasedRenderingManager : IDisposable
  {
    public const int BITS_TILES_PER_CHUNK_EDGE = 8;
    public const int MASK_TILES_PER_CHUNK_EDGE = 255;
    public const int TILES_PER_CHUNK_EDGE = 256;
    public const int METERS_PER_CHUNK_EDGE = 512;
    public const int TILES_PER_CHUNK_AREA = 65536;
    public const int TILE_INDEX_MASK = 65535;
    public const float CHUNK_SIZE_METERS = 512f;
    public const float CHUNK_SIZE_HALF_METERS = 256f;
    private readonly TerrainManager m_terrainManager;
    private readonly CameraController m_cameraController;
    private readonly IGameConsole m_console;
    private readonly ChunkBasedRenderingManager.RenderedMasterChunk[] m_masterChunks;
    private LystStruct<IRenderedChunkAlwaysVisible> m_alwaysRenderedChunks;
    private LystStruct<IRenderedChunkCustom> m_customChunks;
    private readonly Plane[] m_frustumPlanes;
    public readonly int ChunksCountX;
    public readonly int ChunksCountY;
    public readonly int ChunksCountTotal;
    private Set<IRenderedChunk> m_renderedChunksThisFrame;
    private Set<IRenderedChunk> m_renderedChunksLastFrame;
    private float m_lodDistanceMult;

    public int MaxLod { get; private set; }

    public int MinLod { get; private set; }

    public bool CullingIsDisabled { get; private set; }

    public ChunkBasedRenderingManager(
      TerrainManager terrainManager,
      IGameLoopEvents gameLoopEvents,
      CameraController cameraController,
      IGameConsole console)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_frustumPlanes = new Plane[6];
      this.m_renderedChunksThisFrame = new Set<IRenderedChunk>();
      this.m_renderedChunksLastFrame = new Set<IRenderedChunk>();
      this.m_lodDistanceMult = 1f;
      // ISSUE: reference to a compiler-generated field
      this.\u003CMaxLod\u003Ek__BackingField = 6;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_terrainManager = terrainManager;
      this.m_cameraController = cameraController;
      this.m_console = console;
      this.ChunksCountX = this.m_terrainManager.TerrainWidth.CeilDivPositive(256);
      this.ChunksCountY = this.m_terrainManager.TerrainHeight.CeilDivPositive(256);
      this.ChunksCountTotal = this.ChunksCountX * this.ChunksCountY;
      Assert.That<int>(this.ChunksCountTotal).IsPositive();
      this.m_masterChunks = new ChunkBasedRenderingManager.RenderedMasterChunk[this.ChunksCountTotal];
      int y = 0;
      int index = 0;
      for (; y < this.ChunksCountY; ++y)
      {
        int x = 0;
        while (x < this.ChunksCountX)
        {
          this.m_masterChunks[index] = new ChunkBasedRenderingManager.RenderedMasterChunk(new Vector2((float) x, (float) y) * 512f);
          ++x;
          ++index;
        }
      }
      gameLoopEvents.RenderUpdateEnd.AddNonSaveable<ChunkBasedRenderingManager>(this, new Action<GameTime>(this.renderChunks));
      GlobalGfxSettings.LodBiasRenderingSetting.OnSettingChange += new Action<RenderingSetting>(this.lodBiasChanged);
      this.lodBiasChanged(GlobalGfxSettings.LodBiasRenderingSetting);
    }

    private void initialize()
    {
    }

    public void Dispose()
    {
      GlobalGfxSettings.LodBiasRenderingSetting.OnSettingChange -= new Action<RenderingSetting>(this.lodBiasChanged);
    }

    [ConsoleCommand(true, false, null, null)]
    public void SetMaxLod(int maxLod) => this.MaxLod = maxLod.Clamp(0, 6);

    [ConsoleCommand(true, false, null, null)]
    public void SetMinLod(int minLod) => this.MinLod = minLod.Clamp(0, 6);

    [ConsoleCommand(true, false, null, null)]
    public void SetCullingEnabled(bool isEnabled) => this.CullingIsDisabled = !isEnabled;

    private void lodBiasChanged(RenderingSetting setting)
    {
      this.m_lodDistanceMult = (float) (1.0 / ((double) setting.CurrentOption.Value / 10.0));
      Shader.SetGlobalFloat("_LodDistanceInverseMult", 1f / this.m_lodDistanceMult);
    }

    public Chunk256Index GetChunkIndex(Tile2i tileCoord)
    {
      return new Chunk256Index((ushort) ((tileCoord.Y >> 8) * this.ChunksCountX + (tileCoord.X >> 8)));
    }

    public Chunk256 GetChunkCoord(Chunk256Index index)
    {
      return new Chunk256((byte) ((uint) index.Value % (uint) this.ChunksCountX), (byte) ((uint) index.Value / (uint) this.ChunksCountX));
    }

    public Chunk256AndIndex ExtendChunkCoord(Chunk256Index index)
    {
      return new Chunk256AndIndex(this.GetChunkCoord(index), index);
    }

    public static Tile2i GetChunkOrigin(Tile2i tileCoord) => tileCoord & -256;

    public static Tile2i GetChunkOrigin(Vector2i chunkCoord)
    {
      return new Tile2i(chunkCoord.X << 8, chunkCoord.Y << 8);
    }

    public static Bounds GetChunkBounds(IRenderedChunk chunk)
    {
      Vector2 minMaxHeight = chunk.MinMaxHeight;
      return ChunkBasedRenderingManager.GetChunkBounds(chunk.Origin, chunk.MaxModelDeviationFromChunkBounds, minMaxHeight.x, minMaxHeight.y);
    }

    public static Bounds GetChunkBounds(
      Vector2 origin,
      float extraRadius,
      float minHeight,
      float maxHeight)
    {
      return new Bounds(new Vector3(origin.x + 256f, (float) (((double) minHeight + (double) maxHeight) * 0.5), origin.y + 256f), new Vector3((float) (512.0 + 2.0 * (double) extraRadius), (float) ((double) maxHeight - (double) minHeight + 2.0 * (double) extraRadius), (float) (512.0 + 2.0 * (double) extraRadius)));
    }

    public void RegisterChunk(IRenderedChunk newChunk)
    {
      this.m_masterChunks[(int) newChunk.CoordAndIndex.ChunkIndex.Value].AddChunk(newChunk);
    }

    public void UnregisterChunk(IRenderedChunk newChunk)
    {
      this.m_masterChunks[(int) newChunk.CoordAndIndex.ChunkIndex.Value].RemoveChunk(newChunk);
    }

    public bool TryUnregisterChunk(IRenderedChunk newChunk)
    {
      return (int) newChunk.CoordAndIndex.ChunkIndex.Value < this.m_masterChunks.Length && this.m_masterChunks[(int) newChunk.CoordAndIndex.ChunkIndex.Value].TryRemoveChunk(newChunk);
    }

    public void RegisterChunk(IRenderedChunkCustom newChunk)
    {
      this.m_customChunks.AddAndAssertNew(newChunk);
    }

    public void UnregisterChunk(IRenderedChunkCustom newChunk)
    {
      this.m_customChunks.RemoveAndAssert(newChunk);
    }

    public bool TryUnregisterChunk(IRenderedChunkCustom newChunk)
    {
      return this.m_customChunks.Remove(newChunk);
    }

    public void RegisterChunkAlwaysRender(IRenderedChunkAlwaysVisible newChunk)
    {
      this.m_alwaysRenderedChunks.AddAndAssertNew(newChunk);
    }

    public void UnregisterChunkAlwaysRender(IRenderedChunkAlwaysVisible newChunk)
    {
      this.m_alwaysRenderedChunks.RemoveAndAssert(newChunk);
    }

    private void renderChunks(GameTime time)
    {
      Vector3 position = this.m_cameraController.Camera.transform.position;
      GeometryUtility.CalculateFrustumPlanes(this.m_cameraController.Camera, this.m_frustumPlanes);
      Swap.Them<Set<IRenderedChunk>>(ref this.m_renderedChunksThisFrame, ref this.m_renderedChunksLastFrame);
      foreach (ChunkBasedRenderingManager.RenderedMasterChunk masterChunk in this.m_masterChunks)
      {
        if (this.CullingIsDisabled || GeometryUtility.TestPlanesAABB(this.m_frustumPlanes, masterChunk.Bounds))
        {
          float num = masterChunk.Bounds.SqrDistance(position).Sqrt() * this.m_lodDistanceMult;
          float forCameraDistance = LodUtils.GetPxPerMeterForCameraDistance(num, this.m_cameraController.Camera);
          int levelFromPxPerMeter = LodUtils.GetLodLevelFromPxPerMeter(forCameraDistance);
          foreach (IRenderedChunk chunk in masterChunk.Chunks)
          {
            chunk.Render(time, num, levelFromPxPerMeter, forCameraDistance);
            if (chunk.TrackStoppedRendering)
            {
              this.m_renderedChunksThisFrame.Add(chunk);
              this.m_renderedChunksLastFrame.Remove(chunk);
            }
          }
        }
      }
      foreach (IRenderedChunkCustom customChunk in this.m_customChunks)
      {
        if (this.CullingIsDisabled || GeometryUtility.TestPlanesAABB(this.m_frustumPlanes, customChunk.Bounds))
        {
          float num = customChunk.Bounds.SqrDistance(position).Sqrt() * this.m_lodDistanceMult;
          float forCameraDistance = LodUtils.GetPxPerMeterForCameraDistance(num, this.m_cameraController.Camera);
          int levelFromPxPerMeter = LodUtils.GetLodLevelFromPxPerMeter(forCameraDistance);
          customChunk.Render(time, num, levelFromPxPerMeter, forCameraDistance);
        }
      }
      Bounds bounds = new Bounds(this.m_cameraController.CameraModel.TargetPosition, new Vector3(10000f, 10000f, 1000f));
      foreach (IRenderedChunkAlwaysVisible alwaysRenderedChunk in this.m_alwaysRenderedChunks)
        alwaysRenderedChunk.RenderAlwaysVisible(time, bounds);
      if (!this.m_renderedChunksLastFrame.IsNotEmpty)
        return;
      foreach (IRenderedChunk renderedChunk in this.m_renderedChunksLastFrame)
        renderedChunk.NotifyWasNotRendered();
      this.m_renderedChunksLastFrame.Clear();
    }

    private sealed class RenderedMasterChunk : IRenderedChunksParent
    {
      private readonly Vector2 m_origin;
      public LystStruct<IRenderedChunk> Chunks;
      private float m_minHeight;
      private float m_maxHeight;
      private float m_extraRadius;

      public bool AreBoundsValid => (double) this.m_minHeight < (double) this.m_maxHeight;

      public Bounds Bounds { get; private set; }

      public RenderedMasterChunk(Vector2 origin)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.m_origin = origin;
        this.Chunks = new LystStruct<IRenderedChunk>(4);
      }

      public void AddChunk(IRenderedChunk chunk)
      {
        this.Chunks.AddAndAssertNew(chunk);
        chunk.Register((IRenderedChunksParent) this);
        this.m_extraRadius = this.m_extraRadius.Max(chunk.MaxModelDeviationFromChunkBounds);
        Vector2 minMaxHeight = chunk.MinMaxHeight;
        this.NotifyHeightRangeChanged(minMaxHeight.x, minMaxHeight.y);
        this.recomputeBounds();
      }

      public void RemoveChunk(IRenderedChunk chunk) => this.Chunks.RemoveAndAssert(chunk);

      public bool TryRemoveChunk(IRenderedChunk chunk) => this.Chunks.Remove(chunk);

      public void NotifyHeightRangeChanged(float minHeight, float maxHeight)
      {
        if (this.AreBoundsValid)
        {
          if ((double) minHeight < (double) this.m_minHeight)
          {
            this.m_minHeight = minHeight;
            this.recomputeBounds();
          }
          if ((double) maxHeight <= (double) this.m_maxHeight)
            return;
          this.m_maxHeight = maxHeight;
          this.recomputeBounds();
        }
        else
        {
          this.m_minHeight = minHeight - 0.01f;
          this.m_maxHeight = maxHeight;
          this.recomputeBounds();
        }
      }

      private void recomputeBounds()
      {
        this.Bounds = ChunkBasedRenderingManager.GetChunkBounds(this.m_origin, this.m_extraRadius, this.m_minHeight, this.m_maxHeight);
      }
    }
  }
}
