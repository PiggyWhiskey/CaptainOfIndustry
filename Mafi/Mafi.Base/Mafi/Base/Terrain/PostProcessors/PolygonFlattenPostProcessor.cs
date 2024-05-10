// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.PolygonFlattenPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Terrain.Previews;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;
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
  /// Post processor that flattens the terrain within a polygon.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonFlattenPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IPostProcessorWithPreview,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly PolygonFlattenPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private Polygon2fFast m_polygonCache;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, PolygonFlattenPostProcessor.EditData[]> m_newHeights;
    [DoNotSave(0, null)]
    private Lyst<Lyst<PolygonFlattenPostProcessor.EditData>> m_newHeightsPool;
    [DoNotSave(0, null)]
    private bool m_previewsGenerated;
    [DoNotSave(0, null)]
    private bool m_isGeneratingPreviews;
    [DoNotSave(0, null)]
    private Option<PreviewHelper<HeightmapFeaturePreviewChunkData>> m_previewHelper;
    [DoNotSave(0, null)]
    private object m_previewInitializeLock;

    public static void Serialize(PolygonFlattenPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonFlattenPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonFlattenPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonFlattenPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static PolygonFlattenPostProcessor Deserialize(BlobReader reader)
    {
      PolygonFlattenPostProcessor flattenPostProcessor;
      if (reader.TryStartClassDeserialization<PolygonFlattenPostProcessor>(out flattenPostProcessor))
        reader.EnqueueDataDeserialization((object) flattenPostProcessor, PolygonFlattenPostProcessor.s_deserializeDataDelayedAction);
      return flattenPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonFlattenPostProcessor>(this, "ConfigMutable", (object) PolygonFlattenPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Flatten";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => true;

    public bool Is2D => false;

    public bool CanRotate => true;

    public TerrainPostProcessorParallelizationStrategy ParallelizationStrategy
    {
      get => TerrainPostProcessorParallelizationStrategy.AnalyzeAllThenApply;
    }

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public PolygonFlattenPostProcessor(
      PolygonFlattenPostProcessor.Configuration initialConfig)
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
      return new HandleData?(new HandleData(zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length, ColorRgba.Gray, (Option<string>) "Assets/Unity/UserInterface/Toolbar/Flatten.svg"));
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
      return new RectangleTerrainArea2i(tile2i, tile2iCeiled - tile2i).ExtendBy(this.ConfigMutable.TransitionDistance.Value);
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
      this.m_newHeights = new Dict<Chunk2i, PolygonFlattenPostProcessor.EditData[]>();
      this.m_newHeightsPool = new Lyst<Lyst<PolygonFlattenPostProcessor.EditData>>();
      string error;
      if (this.ConfigMutable.Polygon.TryGetFastPolygon(out this.m_polygonCache, out error))
        return true;
      Log.Error("Failed to initialize polygon: " + error);
      return false;
    }

    public void Reset()
    {
      this.m_polygonCache = (Polygon2fFast) null;
      this.m_newHeights = (Dict<Chunk2i, PolygonFlattenPostProcessor.EditData[]>) null;
      this.m_newHeightsPool = (Lyst<Lyst<PolygonFlattenPostProcessor.EditData>>) null;
      this.m_previewsGenerated = false;
    }

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      this.ConfigMutable.Polygon.TranslateBy(delta.Xy.Vector2f);
      this.ConfigMutable.ExplicitHeight += delta.Z.TilesThick();
      HeightTilesF heightTilesF = 2048.TilesHigh().HeightTilesF;
      this.ConfigMutable.ExplicitHeight = this.ConfigMutable.ExplicitHeight.Clamp(-heightTilesF, heightTilesF);
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
      lock (this.m_unaffectedChunksCache.BackingArray)
      {
        if (this.m_unaffectedChunksCache.Contains(chunk))
          return;
      }
      if (this.m_polygonCache == null)
      {
        Log.Error("Not initialized.");
      }
      else
      {
        HeightTilesF newHeight;
        if (this.ConfigMutable.TargetHeightSource == PolygonFlattenPostProcessor.Configuration.TargetHeightSourceEnum.MinOfControlPointsHeights)
        {
          newHeight = HeightTilesF.MaxValue;
          foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
            newHeight = newHeight.Min(getHeightAt(vertex));
          if (newHeight == HeightTilesF.MaxValue)
            newHeight = this.ConfigMutable.ExplicitHeight;
        }
        else if (this.ConfigMutable.TargetHeightSource == PolygonFlattenPostProcessor.Configuration.TargetHeightSourceEnum.MaxOfControlPointsHeights)
        {
          newHeight = HeightTilesF.MinValue;
          foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
            newHeight = newHeight.Max(getHeightAt(vertex));
          if (newHeight == HeightTilesF.MinValue)
            newHeight = this.ConfigMutable.ExplicitHeight;
        }
        else
          newHeight = this.ConfigMutable.ExplicitHeight;
        if (this.ConfigMutable.RoundHeightToInteger)
          newHeight = newHeight.Value.Rounded().TilesHigh();
        int tileIndex1 = dataReadOnly.GetTileIndex(chunk.Tile2i - dataOrigin);
        int width = dataReadOnly.Width;
        int num = this.ConfigMutable.TransitionDistance.Value.Squared();
        Fix32 fix32 = this.ConfigMutable.TransitionDistance.IsPositive ? Fix32.One / this.ConfigMutable.TransitionDistance.Value : Fix32.Zero;
        bool customSurface = this.ConfigMutable.SurfaceMaterial.HasValue && this.ConfigMutable.SurfaceMaterialThickness.IsPositive;
        Lyst<PolygonFlattenPostProcessor.EditData> newHeights = (Lyst<PolygonFlattenPostProcessor.EditData>) null;
        lock (this.m_newHeightsPool)
        {
          if (this.m_newHeightsPool.IsNotEmpty)
            newHeights = this.m_newHeightsPool.PopLast();
        }
        if (newHeights == null)
          newHeights = new Lyst<PolygonFlattenPostProcessor.EditData>(true);
        int y = 0;
        while (y < 64)
        {
          for (int x = 0; x < 64; ++x)
          {
            int tileIndex = tileIndex1 + x;
            Vector2f vector2f = (chunk.Tile2i + new RelTile2i(x, y)).Vector2f;
            if (this.m_polygonCache.Contains(vector2f))
            {
              addNewRecord(newHeight, Fix32.One);
            }
            else
            {
              Fix64 fix64 = this.m_polygonCache.DistanceSqrTo(vector2f);
              if (!(fix64 > num))
              {
                Fix32 t = fix64.SqrtToFix32() * fix32;
                addNewRecord(!this.ConfigMutable.SmoothAtTransitionStart ? (!this.ConfigMutable.SmoothAtTransitionEnd ? newHeight.Lerp(dataReadOnly.Heights[tileIndex], t) : newHeight.EaseIn(dataReadOnly.Heights[tileIndex], t)) : (this.ConfigMutable.SmoothAtTransitionEnd ? newHeight.EaseInOut(dataReadOnly.Heights[tileIndex], t) : newHeight.EaseOut(dataReadOnly.Heights[tileIndex], t)), Fix32.One - t);
              }
            }

            void addNewRecord(HeightTilesF newHeight, Fix32 influence)
            {
              ThicknessTilesF rhs = newHeight - dataReadOnly.Heights[tileIndex];
              if (this.ConfigMutable.InteractionMode == PolygonFlattenPostProcessor.Configuration.InteractionModeEnum.AddOnly && rhs.IsNegative || this.ConfigMutable.InteractionMode == PolygonFlattenPostProcessor.Configuration.InteractionModeEnum.RemoveOnly && rhs.IsPositive)
                return;
              ThicknessTilesF thicknessTilesF = rhs;
              ThicknessTilesF surfaceThickness;
              if (customSurface)
              {
                surfaceThickness = !this.ConfigMutable.AlwaysApplySurfaceMaterial ? (!rhs.IsPositive ? ThicknessTilesF.Zero : this.ConfigMutable.SurfaceMaterialThickness.Min(rhs) * influence) : this.ConfigMutable.SurfaceMaterialThickness * influence;
                thicknessTilesF -= surfaceThickness;
              }
              else
                surfaceThickness = ThicknessTilesF.Zero;
              ThicknessTilesF removedThickness = thicknessTilesF.Min(ThicknessTilesF.Zero);
              ThicknessTilesF baseThickness = !this.ConfigMutable.BaseMaterial.HasValue || !thicknessTilesF.IsPositive ? ThicknessTilesF.Zero : thicknessTilesF;
              newHeights.Add(new PolygonFlattenPostProcessor.EditData(new Tile2iIndex(tileIndex), newHeight, surfaceThickness, baseThickness, removedThickness));
            }
          }
          ++y;
          tileIndex1 += width;
        }
        if (newHeights.IsEmpty)
        {
          lock (this.m_unaffectedChunksCache.BackingArray)
            this.m_unaffectedChunksCache.Add(chunk);
        }
        else
        {
          lock (this.m_newHeights)
            this.m_newHeights[chunk] = newHeights.ToArrayAndClear();
        }
        lock (this.m_newHeightsPool)
          this.m_newHeightsPool.Add(newHeights);
      }

      HeightTilesF getHeightAt(Vector2f v)
      {
        Tile2i tile2i = new Tile2i(v.RoundedVector2i);
        tile2i = tile2i.Max(dataOrigin);
        Tile2i tile = tile2i.Min(dataOrigin + dataReadOnly.Size - RelTile2i.One);
        int tileIndex = dataReadOnly.GetTileIndex(tile, dataOrigin);
        return dataReadOnly.Heights[tileIndex];
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      PolygonFlattenPostProcessor.EditData[] editDataArray;
      if (!this.m_newHeights.TryRemove(chunk, out editDataArray))
        return;
      TerrainMaterialProto valueOrNull1 = this.ConfigMutable.BaseMaterial.ValueOrNull;
      // ISSUE: explicit non-virtual call
      TerrainMaterialSlimId slimId1 = valueOrNull1 != null ? __nonvirtual (valueOrNull1.SlimId) : new TerrainMaterialSlimId();
      TerrainMaterialProto valueOrNull2 = this.ConfigMutable.SurfaceMaterial.ValueOrNull;
      // ISSUE: explicit non-virtual call
      TerrainMaterialSlimId slimId2 = valueOrNull2 != null ? __nonvirtual (valueOrNull2.SlimId) : new TerrainMaterialSlimId();
      foreach (PolygonFlattenPostProcessor.EditData editData in editDataArray)
      {
        dataRef.Heights[editData.TileIndex.Value] = editData.Height;
        if (editData.BaseThickness.IsPositive)
          dataRef.AppendOrPushFirstLayer(editData.TileIndex, slimId1, editData.BaseThickness);
        if (editData.SurfaceThickness.IsPositive)
          dataRef.AppendOrPushFirstLayer(editData.TileIndex, slimId2, editData.SurfaceThickness);
      }
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
      Fix32 fix32 = this.ConfigMutable.TransitionDistance.IsPositive ? Fix32.One / this.ConfigMutable.TransitionDistance.Value : Fix32.Zero;
      TerrainManager terrainManager = resolver.Resolve<TerrainManager>();
      HeightTilesF[] oldHeights = terrainManager.GetMutableTerrainDataRef().HeightSnapshot.ValueOrNull;
      if (oldHeights == null)
      {
        Log.Warning("No height snapshot found.");
        oldHeights = terrainManager.GetMutableTerrainDataRef().Heights;
      }
      HeightTilesF testHeight1;
      if (this.ConfigMutable.TargetHeightSource == PolygonFlattenPostProcessor.Configuration.TargetHeightSourceEnum.MinOfControlPointsHeights)
      {
        testHeight1 = HeightTilesF.MaxValue;
        foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
          testHeight1 = testHeight1.Min(getHeightAt(vertex));
        if (testHeight1 == HeightTilesF.MaxValue)
          testHeight1 = this.ConfigMutable.ExplicitHeight;
      }
      else if (this.ConfigMutable.TargetHeightSource == PolygonFlattenPostProcessor.Configuration.TargetHeightSourceEnum.MaxOfControlPointsHeights)
      {
        testHeight1 = HeightTilesF.MinValue;
        foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
          testHeight1 = testHeight1.Max(getHeightAt(vertex));
        if (testHeight1 == HeightTilesF.MinValue)
          testHeight1 = this.ConfigMutable.ExplicitHeight;
      }
      else
        testHeight1 = this.ConfigMutable.ExplicitHeight;
      if (this.ConfigMutable.RoundHeightToInteger)
        testHeight1 = testHeight1.Value.Rounded().TilesHigh();
      Chunk2i chunk = chunkData.Chunk;
      int y = 0;
      int num2 = 0;
      while (y < 65)
      {
        for (int x = 0; x < 65; ++x)
        {
          int tileI = num2 + x;
          Vector2f vector2f = (chunk.Tile2i + new RelTile2i(x, y)).Vector2f;
          if (this.m_polygonCache.Contains(vector2f))
          {
            if (shouldSkipDueToHeightDelta(testHeight1))
            {
              if (final)
                chunkData.Heights[tileI] = HeightTilesF.MinValue;
            }
            else
              chunkData.Heights[tileI] = testHeight1;
          }
          else
          {
            Fix64 fix64 = this.m_polygonCache.DistanceSqrTo(vector2f);
            if (fix64 > num1)
            {
              if (final)
                chunkData.Heights[tileI] = HeightTilesF.MinValue;
            }
            else
            {
              Fix32 t = fix64.SqrtToFix32() * fix32;
              HeightTilesF testHeight2 = !this.ConfigMutable.SmoothAtTransitionStart ? (!this.ConfigMutable.SmoothAtTransitionEnd ? testHeight1.Lerp(chunkData.Heights[tileI], t) : testHeight1.EaseIn(chunkData.Heights[tileI], t)) : (this.ConfigMutable.SmoothAtTransitionEnd ? testHeight1.EaseInOut(chunkData.Heights[tileI], t) : testHeight1.EaseOut(chunkData.Heights[tileI], t));
              if (shouldSkipDueToHeightDelta(testHeight2))
              {
                if (final)
                  chunkData.Heights[tileI] = HeightTilesF.MinValue;
              }
              else
                chunkData.Heights[tileI] = testHeight2;
            }
          }

          bool shouldSkipDueToHeightDelta(HeightTilesF testHeight)
          {
            ThicknessTilesF thicknessTilesF = testHeight - chunkData.Heights[tileI];
            return this.ConfigMutable.InteractionMode == PolygonFlattenPostProcessor.Configuration.InteractionModeEnum.AddOnly && thicknessTilesF.IsNegative || this.ConfigMutable.InteractionMode == PolygonFlattenPostProcessor.Configuration.InteractionModeEnum.RemoveOnly && thicknessTilesF.IsPositive;
          }
        }
        ++y;
        num2 += 65;
      }

      HeightTilesF getHeightAt(Vector2f v)
      {
        Vector2i vector2i1 = v.RoundedVector2i;
        vector2i1 = vector2i1.Max(Vector2i.Zero);
        Vector2i vector2i2 = vector2i1.Min((terrainManager.TerrainSize - RelTile2i.One).Vector2i);
        return oldHeights[terrainManager.GetTileIndex(vector2i2.X, vector2i2.Y).Value];
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

    static PolygonFlattenPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonFlattenPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonFlattenPostProcessor) obj).SerializeData(writer));
      PolygonFlattenPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonFlattenPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        PolygonFlattenPostProcessor.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonFlattenPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonFlattenPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteBool(this.AlwaysApplySurfaceMaterial);
        Option<TerrainMaterialProto>.Serialize(this.BaseMaterial, writer);
        HeightTilesF.Serialize(this.ExplicitHeight, writer);
        writer.WriteInt((int) this.InteractionMode);
        Polygon2fMutable.Serialize(this.Polygon, writer);
        writer.WriteBool(this.RoundHeightToInteger);
        writer.WriteBool(this.SmoothAtTransitionEnd);
        writer.WriteBool(this.SmoothAtTransitionStart);
        writer.WriteInt(this.SortingPriorityAdjustment);
        Option<TerrainMaterialProto>.Serialize(this.SurfaceMaterial, writer);
        ThicknessTilesF.Serialize(this.SurfaceMaterialThickness, writer);
        writer.WriteInt((int) this.TargetHeightSource);
        RelTile1i.Serialize(this.TransitionDistance, writer);
      }

      public static PolygonFlattenPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        PolygonFlattenPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonFlattenPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonFlattenPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.AlwaysApplySurfaceMaterial = reader.LoadedSaveVersion >= 145 && reader.ReadBool();
        this.BaseMaterial = Option<TerrainMaterialProto>.Deserialize(reader);
        this.ExplicitHeight = HeightTilesF.Deserialize(reader);
        this.InteractionMode = reader.LoadedSaveVersion >= 148 ? (PolygonFlattenPostProcessor.Configuration.InteractionModeEnum) reader.ReadInt() : PolygonFlattenPostProcessor.Configuration.InteractionModeEnum.AddAndRemove;
        this.Polygon = Polygon2fMutable.Deserialize(reader);
        this.RoundHeightToInteger = reader.ReadBool();
        this.SmoothAtTransitionEnd = reader.ReadBool();
        this.SmoothAtTransitionStart = reader.ReadBool();
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.SurfaceMaterial = Option<TerrainMaterialProto>.Deserialize(reader);
        this.SurfaceMaterialThickness = ThicknessTilesF.Deserialize(reader);
        this.TargetHeightSource = (PolygonFlattenPostProcessor.Configuration.TargetHeightSourceEnum) reader.ReadInt();
        this.TransitionDistance = RelTile1i.Deserialize(reader);
      }

      [EditorEnforceOrder(53)]
      public Polygon2fMutable Polygon { get; set; }

      [EditorEnforceOrder(56)]
      public PolygonFlattenPostProcessor.Configuration.TargetHeightSourceEnum TargetHeightSource { get; set; }

      [EditorLabel(null, "This value is used only when the Target height source is set to Explicit height.'", false, false)]
      [EditorRange(-2048.0, 2048.0)]
      [EditorEnforceOrder(61)]
      public HeightTilesF ExplicitHeight { get; set; }

      [EditorEnforceOrder(65)]
      public bool RoundHeightToInteger { get; set; }

      [EditorRange(0.0, 100.0)]
      [EditorEnforceOrder(69)]
      public RelTile1i TransitionDistance { get; set; }

      [NewInSaveVersion(148, null, null, null, null)]
      [EditorEnforceOrder(72)]
      public PolygonFlattenPostProcessor.Configuration.InteractionModeEnum InteractionMode { get; set; }

      [EditorEnforceOrder(76)]
      public bool SmoothAtTransitionStart { get; set; }

      [EditorEnforceOrder(79)]
      public bool SmoothAtTransitionEnd { get; set; }

      [EditorLabel(null, "Specifies material that is place on top of smoothed area.", false, false)]
      [EditorEnforceOrder(83)]
      public Option<TerrainMaterialProto> SurfaceMaterial { get; set; }

      [EditorEnforceOrder(86)]
      public ThicknessTilesF SurfaceMaterialThickness { get; set; }

      [NewInSaveVersion(145, null, null, null, null)]
      [EditorEnforceOrder(90)]
      [EditorLabel(null, "Whether to apply surface material even on parts of the terrain that were lowered.", false, false)]
      public bool AlwaysApplySurfaceMaterial { get; set; }

      [EditorLabel(null, "Specifies material that is used to fill extra space when terrain needs to be raised by more than 'Surface material thickness'.", false, false)]
      [EditorEnforceOrder(96)]
      public Option<TerrainMaterialProto> BaseMaterial { get; set; }

      [EditorEnforceOrder(100)]
      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CTargetHeightSource\u003Ek__BackingField = PolygonFlattenPostProcessor.Configuration.TargetHeightSourceEnum.MinOfControlPointsHeights;
        // ISSUE: reference to a compiler-generated field
        this.\u003CExplicitHeight\u003Ek__BackingField = 5.0.TilesHigh();
        // ISSUE: reference to a compiler-generated field
        this.\u003CRoundHeightToInteger\u003Ek__BackingField = true;
        // ISSUE: reference to a compiler-generated field
        this.\u003CSmoothAtTransitionStart\u003Ek__BackingField = true;
        // ISSUE: reference to a compiler-generated field
        this.\u003CSmoothAtTransitionEnd\u003Ek__BackingField = true;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        PolygonFlattenPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonFlattenPostProcessor.Configuration) obj).SerializeData(writer));
        PolygonFlattenPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonFlattenPostProcessor.Configuration) obj).DeserializeData(reader));
      }

      public enum TargetHeightSourceEnum
      {
        ExplicitHeight,
        MinOfControlPointsHeights,
        MaxOfControlPointsHeights,
      }

      public enum InteractionModeEnum
      {
        AddAndRemove,
        AddOnly,
        RemoveOnly,
      }
    }

    private readonly struct EditData
    {
      public readonly Tile2iIndex TileIndex;
      public readonly HeightTilesF Height;
      public readonly ThicknessTilesF SurfaceThickness;
      public readonly ThicknessTilesF BaseThickness;
      public readonly ThicknessTilesF RemovedThickness;

      public EditData(
        Tile2iIndex tileIndex,
        HeightTilesF height,
        ThicknessTilesF surfaceThickness,
        ThicknessTilesF baseThickness,
        ThicknessTilesF removedThickness)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.TileIndex = tileIndex;
        this.Height = height;
        this.SurfaceThickness = surfaceThickness;
        this.BaseThickness = baseThickness;
        this.RemovedThickness = removedThickness;
      }
    }
  }
}
