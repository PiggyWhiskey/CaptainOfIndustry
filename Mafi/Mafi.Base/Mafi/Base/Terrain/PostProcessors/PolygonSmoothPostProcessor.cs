// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.PolygonSmoothPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Terrain.Previews;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>
  /// Polygon-based post processor which smooths surrounding terrain.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonSmoothPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IPostProcessorWithPreview,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly PolygonSmoothPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private Polygon2fFast m_polygonCache;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, Pair<int, HeightTilesF>[]> m_newHeights;
    [DoNotSave(0, null)]
    private Lyst<Lyst<Pair<int, HeightTilesF>>> m_newHeightsPool;
    [DoNotSave(0, null)]
    private bool m_previewsGenerated;
    [DoNotSave(0, null)]
    private bool m_isGeneratingPreviews;
    [DoNotSave(0, null)]
    private Option<PreviewHelper<HeightmapFeaturePreviewChunkData>> m_previewHelper;
    [DoNotSave(0, null)]
    private object m_previewInitializeLock;

    public static void Serialize(PolygonSmoothPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonSmoothPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonSmoothPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonSmoothPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static PolygonSmoothPostProcessor Deserialize(BlobReader reader)
    {
      PolygonSmoothPostProcessor smoothPostProcessor;
      if (reader.TryStartClassDeserialization<PolygonSmoothPostProcessor>(out smoothPostProcessor))
        reader.EnqueueDataDeserialization((object) smoothPostProcessor, PolygonSmoothPostProcessor.s_deserializeDataDelayedAction);
      return smoothPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonSmoothPostProcessor>(this, "ConfigMutable", (object) PolygonSmoothPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Smooth";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => true;

    public bool Is2D => true;

    public bool CanRotate => true;

    public TerrainPostProcessorParallelizationStrategy ParallelizationStrategy
    {
      get => TerrainPostProcessorParallelizationStrategy.AnalyzeAllThenApply;
    }

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment;

    public int PassCount => this.ConfigMutable.Passes;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public PolygonSmoothPostProcessor(
      PolygonSmoothPostProcessor.Configuration initialConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = initialConfig;
    }

    public HandleData? GetHandleData()
    {
      Tile2f zero = Tile2f.Zero;
      foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
        zero += new RelTile2f(vertex);
      return new HandleData?(new HandleData(zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length, ColorRgba.Gray, (Option<string>) "Assets/Unity/UserInterface/Toolbar/Smooth.svg"));
    }

    RectangleTerrainArea2i? ITerrainFeatureBase.GetBoundingBox()
    {
      return new RectangleTerrainArea2i?(this.GetBoundingBox());
    }

    public RectangleTerrainArea2i GetBoundingBox()
    {
      Vector2f min;
      Vector2f max;
      this.ConfigMutable.Polygon.ComputeBounds(out min, out max);
      Tile2i tile2i = new Tile2f(min).Tile2i;
      Tile2i tile2iCeiled = new Tile2f(max).Tile2iCeiled;
      return new RectangleTerrainArea2i(tile2i, tile2iCeiled - tile2i).ExtendBy(this.ConfigMutable.TransitionDistance.Value + 2);
    }

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors) => true;

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      return this.tryInitialize();
    }

    private bool tryInitialize()
    {
      if (this.m_polygonCache != null)
        return true;
      this.m_newHeights = new Dict<Chunk2i, Pair<int, HeightTilesF>[]>();
      this.m_newHeightsPool = new Lyst<Lyst<Pair<int, HeightTilesF>>>();
      string error;
      if (this.ConfigMutable.Polygon.TryGetFastPolygon(out this.m_polygonCache, out error))
        return true;
      Log.Error("Failed to initialize polygon: " + error);
      return false;
    }

    public void Reset()
    {
      this.m_polygonCache = (Polygon2fFast) null;
      this.m_newHeights = (Dict<Chunk2i, Pair<int, HeightTilesF>[]>) null;
      this.m_newHeightsPool = (Lyst<Lyst<Pair<int, HeightTilesF>>>) null;
      this.m_previewsGenerated = false;
    }

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      this.ConfigMutable.Polygon.TranslateBy(delta.Xy.Vector2f);
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
      this.Reset();
      this.ConfigMutable.Polygon.RotateBy(rotation.Degrees);
    }

    public void AnalyzeChunkThreadSafe(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly,
      int pass)
    {
      if (this.m_polygonCache == null)
      {
        Log.Error("Not initialized.");
      }
      else
      {
        Lyst<Pair<int, HeightTilesF>> lyst = (Lyst<Pair<int, HeightTilesF>>) null;
        lock (this.m_unaffectedChunksCache.BackingArray)
        {
          if (this.m_unaffectedChunksCache.Contains(chunk))
            return;
          if (this.m_newHeightsPool.IsNotEmpty)
            lyst = this.m_newHeightsPool.PopLast();
        }
        if (lyst == null)
          lyst = new Lyst<Pair<int, HeightTilesF>>(true);
        int tileIndex = dataReadOnly.GetTileIndex(chunk.Tile2i, dataOrigin);
        int terrainStride = dataReadOnly.Width;
        HeightTilesF[] heights = dataReadOnly.Heights;
        int num1 = this.ConfigMutable.TransitionDistance.Value.Squared();
        Fix32 fix32_1 = this.ConfigMutable.TransitionDistance.IsPositive ? Fix32.One / this.ConfigMutable.TransitionDistance.Value : Fix32.Zero;
        int num2 = 0;
        while (num2 < 64)
        {
          int y = chunk.Tile2i.Y + num2;
          if (y - 2 >= dataOrigin.Y)
          {
            if (y + 2 < dataOrigin.Y + dataReadOnly.Height)
            {
              for (int index1 = 0; index1 < 64; ++index1)
              {
                int x = chunk.Tile2i.X + index1;
                if (x - 2 >= dataOrigin.X)
                {
                  if (x + 2 < dataOrigin.X + dataReadOnly.Width)
                  {
                    int index2 = tileIndex + index1;
                    Vector2f vector2f = new Tile2i(x, y).Vector2f;
                    if (this.m_polygonCache.Contains(vector2f))
                    {
                      HeightTilesF heightTilesF = heights[index2];
                      HeightTilesF other = gaussianBlur5x5(index2);
                      if (!heightTilesF.IsNear(other))
                      {
                        HeightTilesF second = heightTilesF.Lerp(other, this.ConfigMutable.Strength);
                        lyst.Add(Pair.Create<int, HeightTilesF>(index2, second));
                      }
                    }
                    else
                    {
                      Fix64 fix64 = this.m_polygonCache.DistanceSqrTo(vector2f);
                      if (!(fix64 > num1))
                      {
                        Fix32 fix32_2 = fix64.SqrtToFix32() * fix32_1;
                        HeightTilesF heightTilesF = heights[index2];
                        HeightTilesF other = gaussianBlur5x5(index2);
                        if (!heightTilesF.IsNear(other))
                        {
                          HeightTilesF second = heightTilesF.Lerp(other, fix32_2.ScaledBy(this.ConfigMutable.Strength));
                          lyst.Add(Pair.Create<int, HeightTilesF>(index2, second));
                        }
                      }
                    }
                  }
                  else
                    break;
                }
              }
            }
            else
              break;
          }
          ++num2;
          tileIndex += terrainStride;
        }
        lock (this.m_unaffectedChunksCache.BackingArray)
        {
          if (lyst.IsEmpty)
            this.m_unaffectedChunksCache.Add(chunk);
          else
            this.m_newHeights[chunk] = lyst.ToArrayAndClear();
          this.m_newHeightsPool.Add(lyst);
        }

        HeightTilesF gaussianBlur5x5(int i)
        {
          int index1 = i - terrainStride - terrainStride;
          Fix32 fix32_1 = heights[index1 - 1].Value + heights[index1].Value.Times2Fast + heights[index1 + 1].Value;
          int index2 = i - terrainStride;
          Fix32 fix32_2 = fix32_1 + (heights[index2 - 2].Value + heights[index2 - 1].Value.Times4Fast + heights[index2].Value.Times5Fast + heights[index2 + 1].Value.Times4Fast + heights[index2 + 2].Value) + (heights[i - 2].Value.Times2Fast + heights[i - 1].Value.Times5Fast + heights[i].Value.Times12Fast + heights[i + 1].Value.Times5Fast + heights[i + 2].Value.Times2Fast);
          int index3 = i + terrainStride;
          Fix32 fix32_3 = fix32_2 + (heights[index3 - 2].Value + heights[index3 - 1].Value.Times4Fast + heights[index3].Value.Times5Fast + heights[index3 + 1].Value.Times4Fast + heights[index3 + 2].Value);
          int index4 = i + terrainStride + terrainStride;
          return new HeightTilesF((fix32_3 + (heights[index4 - 1].Value + heights[index4].Value.Times2Fast + heights[index4 + 1].Value) + Fix32.FromRaw(8)).DivBy64Fast);
        }
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      Pair<int, HeightTilesF>[] pairArray;
      if (!this.m_newHeights.TryRemove(chunk, out pairArray))
        return;
      foreach (Pair<int, HeightTilesF> pair in pairArray)
        dataRef.Heights[pair.First] = pair.Second;
    }

    public void InitializePreview(IResolver resolver)
    {
      if (this.m_previewInitializeLock != null)
        return;
      this.m_previewInitializeLock = new object();
    }

    public bool TryGetPreviewsWithContextFromPrevious(
      IResolver resolver,
      IIndexable<ITerrainPostProcessorV2> terrainPostProcessors,
      int timeBudgetMs,
      out IEnumerable<ITerrainFeaturePreview> previews)
    {
      PreviewHelper<HeightmapFeaturePreviewChunkData> previewHelper = this.m_previewHelper.ValueOrNull;
      if (previewHelper == null)
        this.m_previewHelper = (Option<PreviewHelper<HeightmapFeaturePreviewChunkData>>) (previewHelper = new PreviewHelper<HeightmapFeaturePreviewChunkData>());
      if (!this.m_previewsGenerated || this.m_isGeneratingPreviews)
        this.generatePreviews(resolver, previewHelper, terrainPostProcessors, timeBudgetMs);
      previews = (IEnumerable<ITerrainFeaturePreview>) previewHelper.GetPreviews();
      return true;
    }

    public void GenerateChunkPreview(
      IResolver resolver,
      ITerrainFeaturePreview inputData,
      bool final)
    {
      HeightmapFeaturePreviewChunkData chunkData = inputData as HeightmapFeaturePreviewChunkData;
      if (chunkData == null || !this.GetBoundingBox().OverlapsWith(chunkData.Area))
        return;
      lock (this.m_previewInitializeLock)
      {
        if (!this.tryInitialize())
          return;
      }
      int num1 = this.ConfigMutable.TransitionDistance.Value.Squared();
      Fix32 fix32_1 = this.ConfigMutable.TransitionDistance.IsPositive ? Fix32.One / this.ConfigMutable.TransitionDistance.Value : Fix32.Zero;
      Chunk2i chunk = chunkData.Chunk;
      HeightmapFeaturePreviewChunkData previewChunkData = new HeightmapFeaturePreviewChunkData();
      Array.Copy((Array) chunkData.Heights, (Array) previewChunkData.Heights, chunkData.Heights.Length);
      for (int index1 = 0; index1 < this.PassCount; ++index1)
      {
        int y = 0;
        int num2 = 0;
        while (y < 65)
        {
          for (int x = 0; x < 65; ++x)
          {
            int index2 = num2 + x;
            Vector2f vector2f = (chunk.Tile2i + new RelTile2i(x, y)).Vector2f;
            if (this.m_polygonCache.Contains(vector2f))
            {
              HeightTilesF heightAtClamped = chunkData.GetHeightAtClamped(x, y);
              HeightTilesF other = gaussianBlur5x5(x, y);
              if (!heightAtClamped.IsNear(other))
              {
                HeightTilesF heightTilesF = heightAtClamped.Lerp(other, this.ConfigMutable.Strength);
                previewChunkData.Heights[index2] = heightTilesF;
              }
            }
            else
            {
              Fix64 fix64 = this.m_polygonCache.DistanceSqrTo(vector2f);
              if (fix64 > num1)
              {
                if (final && index1 == this.PassCount - 1)
                  previewChunkData.Heights[index2] = HeightTilesF.MinValue;
              }
              else
              {
                Fix32 fix32_2 = fix64.SqrtToFix32() * fix32_1;
                HeightTilesF heightAtClamped = chunkData.GetHeightAtClamped(x, y);
                HeightTilesF other = gaussianBlur5x5(x, y);
                if (!heightAtClamped.IsNear(other))
                {
                  HeightTilesF heightTilesF = heightAtClamped.Lerp(other, fix32_2.ScaledBy(this.ConfigMutable.Strength));
                  previewChunkData.Heights[index2] = heightTilesF;
                }
              }
            }
          }
          ++y;
          num2 += 65;
        }
        Swap.Them<HeightTilesF[]>(ref chunkData.Heights, ref previewChunkData.Heights);
      }

      HeightTilesF gaussianBlur5x5(int x, int y)
      {
        int y1 = y - 2;
        Fix32 fix32_1 = chunkData.GetHeightAtClamped(x - 1, y1).Value + chunkData.GetHeightAtClamped(x, y1).Value.Times2Fast + chunkData.GetHeightAtClamped(x + 1, y1).Value;
        int y2 = y - 1;
        Fix32 fix32_2 = fix32_1 + (chunkData.GetHeightAtClamped(x - 2, y2).Value + chunkData.GetHeightAtClamped(x - 1, y2).Value.Times4Fast + chunkData.GetHeightAtClamped(x, y2).Value.Times5Fast + chunkData.GetHeightAtClamped(x + 1, y2).Value.Times4Fast + chunkData.GetHeightAtClamped(x + 2, y2).Value);
        int y3 = y;
        Fix32 fix32_3 = fix32_2;
        HeightTilesF heightAtClamped = chunkData.GetHeightAtClamped(x - 2, y3);
        Fix32 times2Fast1 = heightAtClamped.Value.Times2Fast;
        heightAtClamped = chunkData.GetHeightAtClamped(x - 1, y3);
        Fix32 times5Fast1 = heightAtClamped.Value.Times5Fast;
        Fix32 fix32_4 = times2Fast1 + times5Fast1;
        heightAtClamped = chunkData.GetHeightAtClamped(x, y3);
        Fix32 times12Fast = heightAtClamped.Value.Times12Fast;
        Fix32 fix32_5 = fix32_4 + times12Fast;
        heightAtClamped = chunkData.GetHeightAtClamped(x + 1, y3);
        Fix32 times5Fast2 = heightAtClamped.Value.Times5Fast;
        Fix32 fix32_6 = fix32_5 + times5Fast2;
        heightAtClamped = chunkData.GetHeightAtClamped(x + 2, y3);
        Fix32 times2Fast2 = heightAtClamped.Value.Times2Fast;
        Fix32 fix32_7 = fix32_6 + times2Fast2;
        Fix32 fix32_8 = fix32_3 + fix32_7;
        int y4 = y + 1;
        Fix32 fix32_9 = fix32_8 + (chunkData.GetHeightAtClamped(x - 2, y4).Value + chunkData.GetHeightAtClamped(x - 1, y4).Value.Times4Fast + chunkData.GetHeightAtClamped(x, y4).Value.Times5Fast + chunkData.GetHeightAtClamped(x + 1, y4).Value.Times4Fast + chunkData.GetHeightAtClamped(x + 2, y4).Value);
        int y5 = y + 2;
        return new HeightTilesF((fix32_9 + (chunkData.GetHeightAtClamped(x - 1, y5).Value + chunkData.GetHeightAtClamped(x, y5).Value.Times2Fast + chunkData.GetHeightAtClamped(x + 1, y5).Value) + Fix32.FromRaw(8)).DivBy64Fast);
      }
    }

    private void generatePreviews(
      IResolver resolver,
      PreviewHelper<HeightmapFeaturePreviewChunkData> previewHelper,
      IIndexable<ITerrainPostProcessorV2> terrainPostProcessors,
      int timeBudgetMs)
    {
      TerrainManager terrainManager = resolver.Resolve<TerrainManager>();
      if (this.m_previewInitializeLock == null)
        this.m_previewInitializeLock = new object();
      bool isComplete;
      previewHelper.GeneratePreviewsInParallel(this.GetBoundingBox(), !this.m_previewsGenerated, new Action<HeightmapFeaturePreviewChunkData>(processChunk), timeBudgetMs, out isComplete);
      this.m_previewsGenerated = true;
      this.m_isGeneratingPreviews = !isComplete;

      void processChunk(HeightmapFeaturePreviewChunkData chunkData)
      {
        chunkData.Load(terrainManager);
        if (!terrainManager.IsValidCoord(chunkData.Chunk.Tile2i))
          return;
        foreach (ITerrainPostProcessorV2 terrainPostProcessor in terrainPostProcessors)
        {
          if (terrainPostProcessor != this)
          {
            if (terrainPostProcessor is IPostProcessorWithPreview processorWithPreview)
              processorWithPreview.GenerateChunkPreview(resolver, (ITerrainFeaturePreview) chunkData, false);
          }
          else
            break;
        }
        this.GenerateChunkPreview(resolver, (ITerrainFeaturePreview) chunkData, true);
        chunkData.SetDirty(true);
      }
    }

    static PolygonSmoothPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonSmoothPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonSmoothPostProcessor) obj).SerializeData(writer));
      PolygonSmoothPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonSmoothPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        PolygonSmoothPostProcessor.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonSmoothPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonSmoothPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteInt(this.Passes);
        Polygon2fMutable.Serialize(this.Polygon, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        Percent.Serialize(this.Strength, writer);
        RelTile1i.Serialize(this.TransitionDistance, writer);
      }

      public static PolygonSmoothPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        PolygonSmoothPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonSmoothPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonSmoothPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.Passes = reader.ReadInt();
        this.Polygon = Polygon2fMutable.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.Strength = Percent.Deserialize(reader);
        this.TransitionDistance = RelTile1i.Deserialize(reader);
      }

      [EditorEnforceOrder(41)]
      public Polygon2fMutable Polygon { get; set; }

      [EditorEnforceOrder(45)]
      [EditorRange(0.0, 100.0)]
      public int Passes { get; set; }

      [EditorEnforceOrder(48)]
      [EditorRange(0.0, 10.0)]
      public Percent Strength { get; set; }

      [EditorRange(0.0, 100.0)]
      [EditorEnforceOrder(53)]
      public RelTile1i TransitionDistance { get; set; }

      [EditorEnforceOrder(57)]
      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CPasses\u003Ek__BackingField = 1;
        // ISSUE: reference to a compiler-generated field
        this.\u003CStrength\u003Ek__BackingField = Percent.Hundred;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        PolygonSmoothPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonSmoothPostProcessor.Configuration) obj).SerializeData(writer));
        PolygonSmoothPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonSmoothPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
