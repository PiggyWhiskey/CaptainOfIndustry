// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Previews.PointPreviewRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Numerics;
using Mafi.Unity.InstancedRendering;
using Mafi.Unity.Utils;
using Mafi.Utils;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Previews
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class PointPreviewRenderer : IDisposable
  {
    private readonly Color PREVIEW_COLOR;
    private readonly ChunkBasedRenderingManager m_chunksRenderer;
    private readonly ReloadAfterAssetUpdateManager m_reloadManager;
    private readonly Option<PointPreviewRenderer.PreviewRenderingChunk>[] m_chunks;
    private readonly Mesh[] m_meshSharedLods;
    private readonly Material m_sharedMaterial;

    public PointPreviewRenderer(
      ChunkBasedRenderingManager visibleChunksRenderer,
      AssetsDb assetsDb,
      ReloadAfterAssetUpdateManager reloadManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.PREVIEW_COLOR = new Color(0.3f, 0.7f, 0.3f);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<int>(visibleChunksRenderer.ChunksCountTotal).IsLess((int) ushort.MaxValue, "Encoding of chunks will overflow.");
      this.m_chunksRenderer = visibleChunksRenderer;
      this.m_reloadManager = reloadManager;
      this.m_chunks = new Option<PointPreviewRenderer.PreviewRenderingChunk>[visibleChunksRenderer.ChunksCountTotal];
      MeshBuilder instance = MeshBuilder.Instance;
      instance.AddAaPyramid(Vector3.zero, new Vector3(4f, 15f, 4f), (Color32) this.PREVIEW_COLOR);
      Mesh meshAndClear = instance.GetMeshAndClear();
      meshAndClear.name = "previewPillar";
      this.m_sharedMaterial = UnityEngine.Object.Instantiate<Material>(assetsDb.GetSharedMaterial("Assets/Unity/MapEditor/PillarPreviewInstanced.mat"));
      this.m_sharedMaterial.color = this.PREVIEW_COLOR;
      this.m_meshSharedLods = new Mesh[7];
      for (int index = 0; index < 7; ++index)
        this.m_meshSharedLods[index] = meshAndClear;
    }

    public void Dispose()
    {
      for (int index = 0; index < this.m_chunks.Length; ++index)
      {
        this.m_chunks[index].ValueOrNull?.Dispose();
        this.m_chunks[index] = Option<PointPreviewRenderer.PreviewRenderingChunk>.None;
      }
    }

    private PointPreviewRenderer.PreviewRenderingChunk getOrCreateChunk(Chunk256Index index)
    {
      PointPreviewRenderer.PreviewRenderingChunk newChunk = this.m_chunks[(int) index.Value].ValueOrNull;
      if (newChunk == null)
      {
        this.m_chunks[(int) index.Value] = (Option<PointPreviewRenderer.PreviewRenderingChunk>) (newChunk = new PointPreviewRenderer.PreviewRenderingChunk(this.m_chunksRenderer.ExtendChunkCoord(index), this));
        this.m_chunksRenderer.RegisterChunk((IRenderedChunk) newChunk);
      }
      return newChunk;
    }

    public uint AddPreview(Tile3i position)
    {
      PointPreviewRenderer.PreviewInstanceData data = new PointPreviewRenderer.PreviewInstanceData(position.ToGroundCenterVector3());
      Chunk256Index chunkIndex = this.m_chunksRenderer.GetChunkIndex(position.Xy);
      PointPreviewRenderer.PreviewRenderingChunk chunk = this.getOrCreateChunk(chunkIndex);
      if (chunk.InstancesCount < (int) ushort.MaxValue)
        return (uint) chunk.AddInstance(data) | (uint) chunkIndex.Value << 16;
      Log.Error("Too many construction cubes on a chunk!");
      return uint.MaxValue;
    }

    public void Clear()
    {
      for (int index = 0; index < this.m_chunks.Length; ++index)
        this.m_chunks[index].ValueOrNull?.Clear();
    }

    [ExpectedStructSize(12)]
    private readonly struct PreviewInstanceData
    {
      public readonly Vector3 Position;

      public PreviewInstanceData(Vector3 position)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.Position = position;
      }
    }

    private sealed class PreviewRenderingChunk : IRenderedChunk, IRenderedChunksBase, IDisposable
    {
      private readonly PointPreviewRenderer m_parentRenderer;
      private readonly InstancedMeshesRenderer<PointPreviewRenderer.PreviewInstanceData> m_renderer;
      private IRenderedChunksParent m_chunkParent;
      private Bounds m_bounds;
      private float m_minHeight;
      private float m_maxHeight;

      public string Name => "Construction cubes";

      public int InstancesCount => this.m_renderer.InstancesCount;

      public Vector2 Origin { get; }

      public Chunk256AndIndex CoordAndIndex { get; }

      public bool TrackStoppedRendering => false;

      public float MaxModelDeviationFromChunkBounds => 0.0f;

      public Vector2 MinMaxHeight => new Vector2(this.m_minHeight, this.m_maxHeight);

      public PreviewRenderingChunk(
        Chunk256AndIndex coordAndIndex,
        PointPreviewRenderer parentRenderer)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.CoordAndIndex = coordAndIndex;
        this.Origin = coordAndIndex.OriginTile2i.ToVector2();
        this.m_parentRenderer = parentRenderer;
        this.m_renderer = new InstancedMeshesRenderer<PointPreviewRenderer.PreviewInstanceData>(parentRenderer.m_meshSharedLods, UnityEngine.Object.Instantiate<Material>(parentRenderer.m_sharedMaterial));
        parentRenderer.m_reloadManager.Register((IReloadAfterAssetUpdate) this.m_renderer);
      }

      public void Dispose()
      {
        this.m_parentRenderer.m_reloadManager.TryUnregisterAndDispose<InstancedMeshesRenderer<PointPreviewRenderer.PreviewInstanceData>>(this.m_renderer);
      }

      public void ReportAllRenderedInstances(Lyst<RenderedInstancesInfo> info)
      {
        info.Add(new RenderedInstancesInfo(this.Name, this.m_renderer.InstancesCount, this.m_renderer.IndicesCountForLod0));
      }

      public RenderStats Render(GameTime time, float cameraDistance, int lod, float pxPerMeter)
      {
        return this.m_renderer.Render(this.m_bounds, lod);
      }

      public void Register(IRenderedChunksParent parent) => this.m_chunkParent = parent;

      public void NotifyWasNotRendered()
      {
      }

      public ushort AddInstance(PointPreviewRenderer.PreviewInstanceData data)
      {
        ushort num = this.m_renderer.AddInstance(data);
        float y = data.Position.y;
        if (this.m_renderer.InstancesCount == 1)
        {
          this.m_minHeight = this.m_maxHeight = y;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        else if ((double) y < (double) this.m_minHeight)
        {
          this.m_minHeight = y;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        else if ((double) y > (double) this.m_maxHeight)
        {
          this.m_maxHeight = y;
          this.m_bounds = ChunkBasedRenderingManager.GetChunkBounds((IRenderedChunk) this);
          this.m_chunkParent.NotifyHeightRangeChanged(this.m_minHeight, this.m_maxHeight);
        }
        return num;
      }

      public void Clear() => this.m_renderer.Clear();
    }
  }
}
