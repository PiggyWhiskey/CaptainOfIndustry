// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.Previews.PreviewHelper`1
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

#nullable disable
namespace Mafi.Base.Terrain.Previews
{
  internal class PreviewHelper<T> where T : class, ITerrainFeaturePreview, new()
  {
    private static readonly ObjectPool2<T> s_previewChunkDataPool;
    private readonly Dict<Chunk2i, T> m_chunkPreviews;
    private readonly Queueue<Chunk2i> m_previewToProcess;
    private readonly Stopwatch m_stopwatch;
    private LystStruct<Chunk2i> m_previewsToRemoveTmp;
    private RectangleTerrainArea2i m_boundingBox;
    private readonly ParallelOptions m_parallelOptions;

    public void GeneratePreviewsInParallel(
      RectangleTerrainArea2i boundingBox,
      bool dirty,
      Action<T> processChunk,
      int timeBudgetMs,
      out bool isComplete)
    {
      this.m_stopwatch.Restart();
      this.m_boundingBox = boundingBox;
      if (dirty)
      {
        foreach (Chunk2i enumerateChunk in boundingBox.EnumerateChunks())
        {
          if (enumerateChunk.Tile2i.X >= 0 && enumerateChunk.Tile2i.Y >= 0 && !this.m_previewToProcess.Contains(enumerateChunk))
            this.m_previewToProcess.Enqueue(enumerateChunk);
        }
        this.m_previewsToRemoveTmp.ClearSkipZeroingMemory();
        foreach (Chunk2i key in this.m_chunkPreviews.Keys)
        {
          if (!boundingBox.OverlapsWith(key.Area))
            this.m_previewsToRemoveTmp.Add(key);
        }
        foreach (Chunk2i key in this.m_previewsToRemoveTmp)
        {
          T chunkPreview = this.m_chunkPreviews[key];
          PreviewHelper<T>.s_previewChunkDataPool.ReturnInstance(ref chunkPreview);
          this.m_chunkPreviews.Remove(key);
        }
      }
      foreach (T obj in this.m_chunkPreviews.Values)
        obj.SetDirty(false);
      try
      {
        Parallel.For(0, this.m_previewToProcess.Count, this.m_parallelOptions, (Action<int, ParallelLoopState>) ((i, state) =>
        {
          if (i != 0 && this.m_stopwatch.ElapsedMilliseconds > (long) timeBudgetMs)
          {
            state.Break();
          }
          else
          {
            Chunk2i chunk2i;
            lock (this.m_previewToProcess)
              chunk2i = this.m_previewToProcess.Dequeue();
            if (!this.m_boundingBox.OverlapsWith(chunk2i.Area))
              return;
            T instance;
            lock (this.m_chunkPreviews)
            {
              if (!this.m_chunkPreviews.TryGetValue(chunk2i, out instance))
              {
                instance = PreviewHelper<T>.s_previewChunkDataPool.GetInstance();
                this.m_chunkPreviews[chunk2i] = instance;
              }
            }
            instance.Initialize(chunk2i);
            processChunk(instance);
            if (this.m_stopwatch.ElapsedMilliseconds <= (long) timeBudgetMs)
              return;
            state.Break();
          }
        }));
      }
      catch (Exception ex)
      {
        Mafi.Log.Exception(ex, "Exception was thrown during preview generation.");
      }
      isComplete = this.m_previewToProcess.IsEmpty;
    }

    public IEnumerable<T> GetPreviews() => (IEnumerable<T>) this.m_chunkPreviews.Values;

    public void ClearPreviews()
    {
      foreach (KeyValuePair<Chunk2i, T> chunkPreview in this.m_chunkPreviews)
      {
        T obj = chunkPreview.Value;
        PreviewHelper<T>.s_previewChunkDataPool.ReturnInstance(ref obj);
      }
      this.m_chunkPreviews.Clear();
    }

    public PreviewHelper()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_chunkPreviews = new Dict<Chunk2i, T>();
      this.m_previewToProcess = new Queueue<Chunk2i>();
      this.m_stopwatch = new Stopwatch();
      this.m_parallelOptions = new ParallelOptions()
      {
        MaxDegreeOfParallelism = Environment.ProcessorCount.CeilDivPositive(2)
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static PreviewHelper()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PreviewHelper<T>.s_previewChunkDataPool = new ObjectPool2<T>(1024, (Func<ObjectPool2<T>, T>) (pool => new T()), (Action<T>) (_ => { }));
    }
  }
}
