// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.PolygonRampPostProcessor
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
  /// Post processor that creates a ramp on the terrain defined by a polygon.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonRampPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IPostProcessorWithPreview,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly PolygonRampPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private Polygon3fFast m_polygonCache;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, PolygonRampPostProcessor.EditData[]> m_newLayers;
    [DoNotSave(0, null)]
    private Lyst<Lyst<PolygonRampPostProcessor.EditData>> m_newLayersPool;
    [DoNotSave(0, null)]
    private bool m_previewsGenerated;
    [DoNotSave(0, null)]
    private bool m_isGeneratingPreviews;
    [DoNotSave(0, null)]
    private Option<PreviewHelper<HeightmapFeaturePreviewChunkData>> m_previewHelper;
    [DoNotSave(0, null)]
    private object m_previewInitializeLock;
    [DoNotSave(0, null)]
    private Option<TerrainManager> m_terrainManager;

    public static void Serialize(PolygonRampPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonRampPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonRampPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonRampPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static PolygonRampPostProcessor Deserialize(BlobReader reader)
    {
      PolygonRampPostProcessor rampPostProcessor;
      if (reader.TryStartClassDeserialization<PolygonRampPostProcessor>(out rampPostProcessor))
        reader.EnqueueDataDeserialization((object) rampPostProcessor, PolygonRampPostProcessor.s_deserializeDataDelayedAction);
      return rampPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonRampPostProcessor>(this, "ConfigMutable", (object) PolygonRampPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Ramp";

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

    public PolygonRampPostProcessor(
      PolygonRampPostProcessor.Configuration initialConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_terrainManager = Option<TerrainManager>.None;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = initialConfig;
    }

    public void SnapControlPointsToTerrain(TerrainManager terrainManager)
    {
      Polygon3fMutable polygon = this.ConfigMutable.Polygon;
      for (int i = 0; i < polygon.Vertices.Length; ++i)
      {
        Vector3f vector3f = polygon[i];
        HeightTilesF heightOrDefault = terrainManager.GetHeightOrDefault(new Tile2f(vector3f.Xy));
        polygon[i] = new Vector3f(vector3f.X, vector3f.Y, heightOrDefault.Value.Rounded());
      }
    }

    public HandleData? GetHandleData()
    {
      Tile2f zero = Tile2f.Zero;
      foreach (Vector3f vertex in this.ConfigMutable.Polygon.Vertices)
        zero += new RelTile2f(vertex.Xy);
      return new HandleData?(new HandleData(zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length, ColorRgba.Gray, (Option<string>) "Assets/Unity/UserInterface/Toolbar/Ramp.svg"));
    }

    RectangleTerrainArea2i? ITerrainFeatureBase.GetBoundingBox()
    {
      return new RectangleTerrainArea2i?(this.GetBoundingBox());
    }

    public RectangleTerrainArea2i GetBoundingBox()
    {
      Vector3f min;
      Vector3f max;
      this.ConfigMutable.Polygon.ComputeBounds(out min, out max);
      Tile2i tile2i = new Tile2f(min.Xy).Tile2i;
      Tile2i tile2iCeiled = new Tile2f(max.Xy).Tile2iCeiled;
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
      this.m_terrainManager = (Option<TerrainManager>) resolver.Resolve<TerrainManager>();
      return this.tryInitialize(this.m_terrainManager.Value);
    }

    private bool tryInitialize(TerrainManager terrainManager)
    {
      if (this.m_polygonCache != null)
        return true;
      if (this.ConfigMutable.SnapControlPointsToTerrain)
        this.SnapControlPointsToTerrain(terrainManager);
      this.m_newLayers = new Dict<Chunk2i, PolygonRampPostProcessor.EditData[]>();
      this.m_newLayersPool = new Lyst<Lyst<PolygonRampPostProcessor.EditData>>();
      this.ConfigMutable.Polygon.RoundControlPointHeightsToWholeTiles = this.ConfigMutable.RoundControlPointHeightsToInteger;
      string error;
      if (this.ConfigMutable.Polygon.TryGetFastPolygon(out this.m_polygonCache, out error))
        return true;
      Log.Error("Failed to initialize polygon: " + error);
      return false;
    }

    public void Reset()
    {
      this.m_polygonCache = (Polygon3fFast) null;
      this.m_newLayers = (Dict<Chunk2i, PolygonRampPostProcessor.EditData[]>) null;
      this.m_newLayersPool = (Lyst<Lyst<PolygonRampPostProcessor.EditData>>) null;
      this.m_previewsGenerated = false;
    }

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      this.ConfigMutable.Polygon.TranslateBy(delta.Vector3f);
      if (!this.ConfigMutable.SnapControlPointsToTerrain || !this.m_terrainManager.HasValue)
        return;
      this.SnapControlPointsToTerrain(this.m_terrainManager.Value);
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
      this.Reset();
      this.ConfigMutable.Polygon.RotateBy(rotation.Degrees);
    }

    private HeightTilesF getHeightAtContainedPoint(Vector2f coord)
    {
      Fix64 fix64_1 = (Fix64) 0L;
      Fix64 fix64_2 = (Fix64) 0L;
      for (int index = 0; index < this.m_polygonCache.VerticesCount; ++index)
      {
        Vector3f vector3f1 = this.m_polygonCache.VerticesExt[index];
        Vector3f vector3f2 = this.m_polygonCache.VerticesExt[index + 1];
        if (!(vector3f1 == vector3f2))
        {
          Percent distanceAlongLine;
          Fix64 fix64_3 = new Line2f(vector3f1.Xy, vector3f2.Xy).DistanceSqrToLineSegment(coord, out distanceAlongLine).Sqrt();
          Fix64 fix64_4 = vector3f1.Z.ToFix64().Lerp(vector3f2.Z.ToFix64(), distanceAlongLine);
          if (fix64_3.IsNearZero())
          {
            fix64_1 = fix64_4;
            fix64_2 = (Fix64) 1L;
            break;
          }
          Fix64 fix64_5 = Fix64.One / fix64_3;
          fix64_1 += fix64_4 * fix64_5;
          fix64_2 += fix64_5;
        }
      }
      return new HeightTilesF((fix64_1 / fix64_2).ToFix32());
    }

    private HeightTilesF getHeightAtUncontainedPoint(Vector2f coord)
    {
      Fix64 fix64_1 = (Fix64) 0L;
      Fix64 fix64_2 = (Fix64) 0L;
      for (int index = 0; index < this.m_polygonCache.VerticesCount; ++index)
      {
        Vector3f vector3f1 = this.m_polygonCache.VerticesExt[index];
        Vector3f vector3f2 = this.m_polygonCache.VerticesExt[index + 1];
        if (!(vector3f1 == vector3f2))
        {
          Percent distanceAlongLine;
          Fix64 lineSegment = new Line2f(vector3f1.Xy, vector3f2.Xy).DistanceSqrToLineSegment(coord, out distanceAlongLine);
          Fix64 fix64_3 = lineSegment.Sqrt();
          Fix64 fix64_4 = vector3f1.Z.ToFix64().Lerp(vector3f2.Z.ToFix64(), distanceAlongLine);
          if (fix64_3.IsNearZero())
            return new HeightTilesF(fix64_4.ToFix32());
          Fix64 fix64_5 = Fix64.One / lineSegment;
          fix64_1 += fix64_4 * fix64_5;
          fix64_2 += fix64_5;
        }
      }
      return new HeightTilesF((fix64_1 / fix64_2).ToFix32());
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
        int tileIndex1 = dataReadOnly.GetTileIndex(chunk.Tile2i, dataOrigin);
        int width = dataReadOnly.Width;
        int num = this.ConfigMutable.TransitionDistance.Value.Squared();
        Fix32 fix32 = this.ConfigMutable.TransitionDistance.IsPositive ? Fix32.One / this.ConfigMutable.TransitionDistance.Value : Fix32.Zero;
        bool customSurface = this.ConfigMutable.SurfaceMaterial.HasValue && this.ConfigMutable.SurfaceMaterialThickness.IsPositive;
        Lyst<PolygonRampPostProcessor.EditData> newLayers = (Lyst<PolygonRampPostProcessor.EditData>) null;
        lock (this.m_newLayersPool)
        {
          if (this.m_newLayersPool.IsNotEmpty)
            newLayers = this.m_newLayersPool.PopLast();
        }
        if (newLayers == null)
          newLayers = new Lyst<PolygonRampPostProcessor.EditData>(true);
        int y = 0;
        while (y < 64)
        {
          for (int x = 0; x < 64; ++x)
          {
            int tileIndex = tileIndex1 + x;
            Vector2f vector2f = (chunk.Tile2i + new RelTile2i(x, y)).Vector2f;
            if (this.m_polygonCache.Contains2D(vector2f))
            {
              addNewRecord(this.getHeightAtContainedPoint(vector2f), Fix32.One);
            }
            else
            {
              Fix64 fix64 = this.m_polygonCache.Distance2DSqrTo(vector2f);
              if (!(fix64 > num))
              {
                HeightTilesF uncontainedPoint = this.getHeightAtUncontainedPoint(vector2f);
                Fix32 t = fix64.SqrtToFix32() * fix32;
                addNewRecord(!this.ConfigMutable.SmoothAtTransitionStart ? (!this.ConfigMutable.SmoothAtTransitionEnd ? uncontainedPoint.Lerp(dataReadOnly.Heights[tileIndex], t) : uncontainedPoint.EaseIn(dataReadOnly.Heights[tileIndex], t)) : (!this.ConfigMutable.SmoothAtTransitionEnd ? uncontainedPoint.EaseOut(dataReadOnly.Heights[tileIndex], t) : uncontainedPoint.EaseInOut(dataReadOnly.Heights[tileIndex], t)), Fix32.One - t);
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
              newLayers.Add(new PolygonRampPostProcessor.EditData(new Tile2iIndex(tileIndex), newHeight, surfaceThickness, baseThickness, removedThickness));
            }
          }
          ++y;
          tileIndex1 += width;
        }
        if (newLayers.IsEmpty)
        {
          lock (this.m_unaffectedChunksCache.BackingArray)
            this.m_unaffectedChunksCache.Add(chunk);
        }
        else
        {
          lock (this.m_newLayers)
            this.m_newLayers[chunk] = newLayers.ToArrayAndClear();
        }
        lock (this.m_newLayersPool)
          this.m_newLayersPool.Add(newLayers);
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      PolygonRampPostProcessor.EditData[] editDataArray;
      if (!this.m_newLayers.TryRemove(chunk, out editDataArray))
        return;
      TerrainMaterialProto valueOrNull1 = this.ConfigMutable.BaseMaterial.ValueOrNull;
      // ISSUE: explicit non-virtual call
      TerrainMaterialSlimId slimId1 = valueOrNull1 != null ? __nonvirtual (valueOrNull1.SlimId) : new TerrainMaterialSlimId();
      TerrainMaterialProto valueOrNull2 = this.ConfigMutable.SurfaceMaterial.ValueOrNull;
      // ISSUE: explicit non-virtual call
      TerrainMaterialSlimId slimId2 = valueOrNull2 != null ? __nonvirtual (valueOrNull2.SlimId) : new TerrainMaterialSlimId();
      foreach (PolygonRampPostProcessor.EditData editData in editDataArray)
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
      if (this.m_previewInitializeLock == null)
        this.m_previewInitializeLock = new object();
      if (!this.ConfigMutable.SnapControlPointsToTerrain)
        return;
      this.SnapControlPointsToTerrain(this.m_terrainManager.ValueOr(resolver.Resolve<TerrainManager>()));
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
        if (!this.tryInitialize(resolver.Resolve<TerrainManager>()))
          return;
      }
      int num1 = this.ConfigMutable.TransitionDistance.Value.Squared();
      Fix32 fix32 = this.ConfigMutable.TransitionDistance.IsPositive ? Fix32.One / this.ConfigMutable.TransitionDistance.Value : Fix32.Zero;
      Chunk2i chunk = chunkData.Chunk;
      int y = 0;
      int num2 = 0;
      while (y < 65)
      {
        for (int x = 0; x < 65; ++x)
        {
          int tileI = num2 + x;
          Vector2f vector2f = (chunk.Tile2i + new RelTile2i(x, y)).Vector2f;
          if (this.m_polygonCache.Contains2D(vector2f))
          {
            HeightTilesF atContainedPoint = this.getHeightAtContainedPoint(vector2f);
            if (shouldSkipDueToHeightDelta(atContainedPoint))
            {
              if (final)
                chunkData.Heights[tileI] = HeightTilesF.MinValue;
            }
            else
              chunkData.Heights[tileI] = atContainedPoint;
          }
          else
          {
            Fix64 fix64 = this.m_polygonCache.Distance2DSqrTo(vector2f);
            if (fix64 > num1)
            {
              if (final)
                chunkData.Heights[tileI] = HeightTilesF.MinValue;
            }
            else
            {
              HeightTilesF uncontainedPoint = this.getHeightAtUncontainedPoint(vector2f);
              Fix32 t = fix64.SqrtToFix32() * fix32;
              HeightTilesF testHeight = !this.ConfigMutable.SmoothAtTransitionStart ? (!this.ConfigMutable.SmoothAtTransitionEnd ? uncontainedPoint.Lerp(chunkData.Heights[tileI], t) : uncontainedPoint.EaseIn(chunkData.Heights[tileI], t)) : (!this.ConfigMutable.SmoothAtTransitionEnd ? uncontainedPoint.EaseOut(chunkData.Heights[tileI], t) : uncontainedPoint.EaseInOut(chunkData.Heights[tileI], t));
              if (shouldSkipDueToHeightDelta(testHeight))
              {
                if (final)
                  chunkData.Heights[tileI] = HeightTilesF.MinValue;
              }
              else
                chunkData.Heights[tileI] = testHeight;
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

    static PolygonRampPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonRampPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonRampPostProcessor) obj).SerializeData(writer));
      PolygonRampPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonRampPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(PolygonRampPostProcessor.Configuration value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonRampPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonRampPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteBool(this.AlwaysApplySurfaceMaterial);
        Option<TerrainMaterialProto>.Serialize(this.BaseMaterial, writer);
        writer.WriteInt((int) this.InteractionMode);
        Polygon3fMutable.Serialize(this.Polygon, writer);
        writer.WriteBool(this.RoundControlPointHeightsToInteger);
        writer.WriteBool(this.SmoothAtTransitionEnd);
        writer.WriteBool(this.SmoothAtTransitionStart);
        writer.WriteBool(this.SnapControlPointsToTerrain);
        writer.WriteInt(this.SortingPriorityAdjustment);
        Option<TerrainMaterialProto>.Serialize(this.SurfaceMaterial, writer);
        ThicknessTilesF.Serialize(this.SurfaceMaterialThickness, writer);
        RelTile1i.Serialize(this.TransitionDistance, writer);
      }

      public static PolygonRampPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        PolygonRampPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonRampPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonRampPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.AlwaysApplySurfaceMaterial = reader.LoadedSaveVersion >= 145 && reader.ReadBool();
        this.BaseMaterial = Option<TerrainMaterialProto>.Deserialize(reader);
        this.InteractionMode = reader.LoadedSaveVersion >= 148 ? (PolygonFlattenPostProcessor.Configuration.InteractionModeEnum) reader.ReadInt() : PolygonFlattenPostProcessor.Configuration.InteractionModeEnum.AddAndRemove;
        this.Polygon = Polygon3fMutable.Deserialize(reader);
        this.RoundControlPointHeightsToInteger = reader.ReadBool();
        this.SmoothAtTransitionEnd = reader.ReadBool();
        this.SmoothAtTransitionStart = reader.ReadBool();
        this.SnapControlPointsToTerrain = reader.ReadBool();
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.SurfaceMaterial = Option<TerrainMaterialProto>.Deserialize(reader);
        this.SurfaceMaterialThickness = ThicknessTilesF.Deserialize(reader);
        this.TransitionDistance = RelTile1i.Deserialize(reader);
      }

      [EditorLabel(null, "Snaps polygon vertices to the terrain height", false, false)]
      [EditorEnforceOrder(42)]
      public bool SnapControlPointsToTerrain { get; set; }

      [EditorEnforceOrder(45)]
      public Polygon3fMutable Polygon { get; set; }

      [EditorEnforceOrder(48)]
      public bool RoundControlPointHeightsToInteger { get; set; }

      [EditorRange(0.0, 100.0)]
      [EditorEnforceOrder(52)]
      public RelTile1i TransitionDistance { get; set; }

      [NewInSaveVersion(148, null, null, null, null)]
      [EditorEnforceOrder(55)]
      public PolygonFlattenPostProcessor.Configuration.InteractionModeEnum InteractionMode { get; set; }

      [EditorEnforceOrder(59)]
      public bool SmoothAtTransitionStart { get; set; }

      [EditorEnforceOrder(62)]
      public bool SmoothAtTransitionEnd { get; set; }

      [EditorEnforceOrder(66)]
      [EditorLabel(null, "Specifies material that is place on top of ramp area.", false, false)]
      public Option<TerrainMaterialProto> SurfaceMaterial { get; set; }

      [EditorEnforceOrder(69)]
      public ThicknessTilesF SurfaceMaterialThickness { get; set; }

      [EditorLabel(null, "Whether to apply surface material even on parts of the terrain that were lowered.", false, false)]
      [NewInSaveVersion(145, null, null, null, null)]
      [EditorEnforceOrder(73)]
      public bool AlwaysApplySurfaceMaterial { get; set; }

      [EditorLabel(null, "Specifies material that is used to fill extra space when terrain needs to be raised by more than 'Surface material thickness'.", false, false)]
      [EditorEnforceOrder(79)]
      public Option<TerrainMaterialProto> BaseMaterial { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(83)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CSnapControlPointsToTerrain\u003Ek__BackingField = true;
        // ISSUE: reference to a compiler-generated field
        this.\u003CRoundControlPointHeightsToInteger\u003Ek__BackingField = true;
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
        PolygonRampPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonRampPostProcessor.Configuration) obj).SerializeData(writer));
        PolygonRampPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonRampPostProcessor.Configuration) obj).DeserializeData(reader));
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
