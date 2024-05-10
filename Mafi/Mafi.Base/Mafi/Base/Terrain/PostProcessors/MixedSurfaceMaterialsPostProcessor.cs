// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.MixedSurfaceMaterialsPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  [GenerateSerializer(false, null, 0)]
  public class MixedSurfaceMaterialsPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MIN_PROCESSED_THICKNESS;
    public readonly MixedSurfaceMaterialsPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private INoise2D m_replacedThicknessFn;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, MixedSurfaceMaterialsPostProcessor.ReplaceData[]> m_replacementLayers;
    [DoNotSave(0, null)]
    private Lyst<Lyst<MixedSurfaceMaterialsPostProcessor.ReplaceData>> m_replacedLayersPool;

    public static void Serialize(MixedSurfaceMaterialsPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MixedSurfaceMaterialsPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MixedSurfaceMaterialsPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      MixedSurfaceMaterialsPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static MixedSurfaceMaterialsPostProcessor Deserialize(BlobReader reader)
    {
      MixedSurfaceMaterialsPostProcessor materialsPostProcessor;
      if (reader.TryStartClassDeserialization<MixedSurfaceMaterialsPostProcessor>(out materialsPostProcessor))
        reader.EnqueueDataDeserialization((object) materialsPostProcessor, MixedSurfaceMaterialsPostProcessor.s_deserializeDataDelayedAction);
      return materialsPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<MixedSurfaceMaterialsPostProcessor>(this, "ConfigMutable", (object) MixedSurfaceMaterialsPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Mixed surfaces";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => false;

    public bool Is2D => true;

    public bool CanRotate => false;

    public TerrainPostProcessorParallelizationStrategy ParallelizationStrategy
    {
      get => TerrainPostProcessorParallelizationStrategy.AnalyzeInterleaveAndApply;
    }

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 2500;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public MixedSurfaceMaterialsPostProcessor(
      MixedSurfaceMaterialsPostProcessor.Configuration configMutable)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = configMutable;
    }

    public HandleData? GetHandleData() => new HandleData?();

    public RectangleTerrainArea2i? GetBoundingBox() => new RectangleTerrainArea2i?();

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors) => true;

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      if (this.m_replacedThicknessFn != null)
        return true;
      if ((Proto) this.ConfigMutable.ReplacedMaterialProto == (Proto) this.ConfigMutable.SourceMaterialProto)
      {
        Log.Warning("Replaced material is the same.");
        return false;
      }
      string error;
      if (!this.ConfigMutable.ReplacedThicknessFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) new Dict<string, object>(), out this.m_replacedThicknessFn, out error))
      {
        Log.Error("Failed to initialize thickness function: " + error);
        return false;
      }
      this.m_replacementLayers = new Dict<Chunk2i, MixedSurfaceMaterialsPostProcessor.ReplaceData[]>();
      this.m_replacedLayersPool = new Lyst<Lyst<MixedSurfaceMaterialsPostProcessor.ReplaceData>>();
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      return true;
    }

    public void Reset()
    {
      this.m_replacedThicknessFn = (INoise2D) null;
      this.m_replacementLayers = (Dict<Chunk2i, MixedSurfaceMaterialsPostProcessor.ReplaceData[]>) null;
      this.m_replacedLayersPool = (Lyst<Lyst<MixedSurfaceMaterialsPostProcessor.ReplaceData>>) null;
    }

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
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
      Tile2i tile2i = chunk.Tile2i;
      int tileIndex1 = dataReadOnly.GetTileIndex(chunk.Tile2i, dataOrigin);
      int width = dataReadOnly.Width;
      Tile2i dataMaxExcl = dataOrigin + dataReadOnly.Size;
      TerrainMaterialSlimId slimId1 = this.ConfigMutable.SourceMaterialProto.SlimId;
      TerrainMaterialSlimId slimId2 = this.ConfigMutable.ReplacedMaterialProto.SlimId;
      ThicknessTilesF thicknessTilesF = this.ConfigMutable.MinReplacedThickness.Max(MixedSurfaceMaterialsPostProcessor.MIN_PROCESSED_THICKNESS);
      Lyst<MixedSurfaceMaterialsPostProcessor.ReplaceData> lyst = (Lyst<MixedSurfaceMaterialsPostProcessor.ReplaceData>) null;
      lock (this.m_replacedLayersPool)
      {
        if (this.m_replacedLayersPool.IsNotEmpty)
          lyst = this.m_replacedLayersPool.PopLast();
      }
      if (lyst == null)
        lyst = new Lyst<MixedSurfaceMaterialsPostProcessor.ReplaceData>(true);
      HeightTilesF[] heights = dataReadOnly.Heights;
      int slopeTestDistance = this.ConfigMutable.SlopeTestDistance.Value.Clamp(1, 32);
      int slopeTestDistanceStride = width * slopeTestDistance;
      int slopeTestDistanceFull = slopeTestDistance + slopeTestDistance;
      Fix32 restrictionDelta = this.ConfigMutable.SlopeRestrictionEnd - this.ConfigMutable.SlopeRestrictionStart;
      if (this.ConfigMutable.SlopeRestrictionStart.IsNotPositive || restrictionDelta.IsNear(Fix32.Zero))
        restrictionDelta = Fix32.Zero;
      int y = 0;
      while (y < 64)
      {
        for (int x = 0; x < 64; ++x)
        {
          int tileIndex2 = tileIndex1 + x;
          TerrainMaterialThicknessSlim first = dataReadOnly.MaterialLayers[tileIndex2].First;
          if (first.SlimId == slimId1)
          {
            ThicknessTilesF thickness = getReplacedThickness(tile2i + new RelTile2i(x, y), tileIndex2);
            if (!(thickness < thicknessTilesF))
            {
              if (first.Thickness - thickness < MixedSurfaceMaterialsPostProcessor.MIN_PROCESSED_THICKNESS)
                thickness = first.Thickness;
              lyst.Add(new MixedSurfaceMaterialsPostProcessor.ReplaceData(new Tile2iIndex(tileIndex2), 0, new TerrainMaterialThicknessSlim(slimId2, thickness)));
            }
          }
          else if (!(first.Thickness > this.ConfigMutable.MaxDepthSearched))
          {
            TerrainMaterialThicknessSlim second = dataReadOnly.MaterialLayers[tileIndex2].Second;
            if (second.SlimId == slimId1)
            {
              ThicknessTilesF thickness = getReplacedThickness(tile2i + new RelTile2i(x, y), tileIndex2);
              if (!(thickness < thicknessTilesF))
              {
                if (second.Thickness - thickness < MixedSurfaceMaterialsPostProcessor.MIN_PROCESSED_THICKNESS)
                  thickness = second.Thickness;
                lyst.Add(new MixedSurfaceMaterialsPostProcessor.ReplaceData(new Tile2iIndex(tileIndex2), 1, new TerrainMaterialThicknessSlim(slimId2, thickness)));
              }
            }
            else if (!(first.Thickness + second.Thickness > this.ConfigMutable.MaxDepthSearched))
            {
              TerrainMaterialThicknessSlim third = dataReadOnly.MaterialLayers[tileIndex2].Third;
              if (third.SlimId == slimId1)
              {
                ThicknessTilesF thickness = getReplacedThickness(tile2i + new RelTile2i(x, y), tileIndex2);
                if (!(thickness < thicknessTilesF))
                {
                  if (third.Thickness - thickness < MixedSurfaceMaterialsPostProcessor.MIN_PROCESSED_THICKNESS)
                    thickness = third.Thickness;
                  lyst.Add(new MixedSurfaceMaterialsPostProcessor.ReplaceData(new Tile2iIndex(tileIndex2), 2, new TerrainMaterialThicknessSlim(slimId2, thickness)));
                }
              }
            }
          }
        }
        ++y;
        tileIndex1 += width;
      }
      if (lyst.IsEmpty)
      {
        lock (this.m_unaffectedChunksCache.BackingArray)
          this.m_unaffectedChunksCache.Add(chunk);
      }
      else
      {
        lock (this.m_replacementLayers)
          this.m_replacementLayers[chunk] = lyst.ToArrayAndClear();
      }
      lyst.Clear();
      lock (this.m_replacedLayersPool)
        this.m_replacedLayersPool.Add(lyst);

      ThicknessTilesF getReplacedThickness(Tile2i position, int tileIndex)
      {
        if (restrictionDelta.IsZero)
          return this.m_replacedThicknessFn.GetValue(position.Vector2f).TilesThick().Min(this.ConfigMutable.MaxReplacedThickness);
        Fix32 fix32_1 = position.X - slopeTestDistance >= dataOrigin.X ? (position.X + slopeTestDistance < dataMaxExcl.X ? (heights[tileIndex + slopeTestDistance] - heights[tileIndex - slopeTestDistance]).Value / slopeTestDistanceFull : (heights[tileIndex] - heights[tileIndex - slopeTestDistance]).Value / slopeTestDistance) : (heights[tileIndex + slopeTestDistance] - heights[tileIndex]).Value / slopeTestDistance;
        Fix32 fix32_2 = position.Y - slopeTestDistance >= dataOrigin.Y ? (position.Y + slopeTestDistance < dataMaxExcl.Y ? (heights[tileIndex + slopeTestDistanceStride] - heights[tileIndex - slopeTestDistanceStride]).Value / slopeTestDistanceFull : (heights[tileIndex] - heights[tileIndex - slopeTestDistanceStride]).Value / slopeTestDistance) : (heights[tileIndex + slopeTestDistanceStride] - heights[tileIndex]).Value / slopeTestDistance;
        Fix32 fix32_3 = fix32_1.Abs().Max(fix32_2.Abs());
        Fix32 fix32_4 = Fix32.One;
        if (restrictionDelta.IsPositive)
        {
          if (fix32_3 > this.ConfigMutable.SlopeRestrictionEnd)
            return ThicknessTilesF.Zero;
          if (fix32_3 > this.ConfigMutable.SlopeRestrictionStart)
            fix32_4 = (fix32_3 - this.ConfigMutable.SlopeRestrictionStart) / restrictionDelta;
        }
        else
        {
          if (fix32_3 < this.ConfigMutable.SlopeRestrictionEnd)
            return ThicknessTilesF.Zero;
          if (fix32_3 < this.ConfigMutable.SlopeRestrictionStart)
            fix32_4 = (fix32_3 - this.ConfigMutable.SlopeRestrictionStart) / restrictionDelta;
        }
        return fix32_4 * this.m_replacedThicknessFn.GetValue(position.Vector2f).TilesThick().Min(this.ConfigMutable.MaxReplacedThickness);
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      MixedSurfaceMaterialsPostProcessor.ReplaceData[] replaceDataArray;
      if (!this.m_replacementLayers.TryRemove(chunk, out replaceDataArray))
        return;
      foreach (MixedSurfaceMaterialsPostProcessor.ReplaceData replaceData in replaceDataArray)
      {
        ref TileMaterialLayers local = ref dataRef.MaterialLayers[replaceData.TileIndex.Value];
        if (replaceData.LayerIndex == 0)
        {
          if (local.First.Thickness <= replaceData.NewLayer.Thickness)
          {
            local.First = local.First.WithNewId(replaceData.NewLayer.SlimId);
          }
          else
          {
            local.First -= replaceData.NewLayer.Thickness;
            dataRef.PushNewFirstLayer(ref local, replaceData.NewLayer);
          }
        }
        else if (replaceData.LayerIndex == 1)
        {
          if (local.Second.Thickness <= replaceData.NewLayer.Thickness)
          {
            local.Second = local.Second.WithNewId(replaceData.NewLayer.SlimId);
          }
          else
          {
            local.Second -= replaceData.NewLayer.Thickness;
            dataRef.PushNewSecondLayer(ref local, replaceData.NewLayer);
          }
        }
        else if (local.Third.Thickness <= replaceData.NewLayer.Thickness)
        {
          local.Third = local.Third.WithNewId(replaceData.NewLayer.SlimId);
        }
        else
        {
          local.Third -= replaceData.NewLayer.Thickness;
          dataRef.PushNewThirdLayer(ref local, replaceData.NewLayer);
        }
      }
    }

    static MixedSurfaceMaterialsPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      MixedSurfaceMaterialsPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MixedSurfaceMaterialsPostProcessor) obj).SerializeData(writer));
      MixedSurfaceMaterialsPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MixedSurfaceMaterialsPostProcessor) obj).DeserializeData(reader));
      MixedSurfaceMaterialsPostProcessor.MIN_PROCESSED_THICKNESS = 0.05000000074505806.TilesThick();
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        MixedSurfaceMaterialsPostProcessor.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<MixedSurfaceMaterialsPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, MixedSurfaceMaterialsPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        ThicknessTilesF.Serialize(this.MaxDepthSearched, writer);
        ThicknessTilesF.Serialize(this.MaxReplacedThickness, writer);
        ThicknessTilesF.Serialize(this.MinReplacedThickness, writer);
        writer.WriteGeneric<TerrainMaterialProto>(this.ReplacedMaterialProto);
        writer.WriteGeneric<INoise2dFactory>(this.ReplacedThicknessFn);
        Fix32.Serialize(this.SlopeRestrictionEnd, writer);
        Fix32.Serialize(this.SlopeRestrictionStart, writer);
        RelTile1i.Serialize(this.SlopeTestDistance, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        writer.WriteGeneric<TerrainMaterialProto>(this.SourceMaterialProto);
      }

      public static MixedSurfaceMaterialsPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        MixedSurfaceMaterialsPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<MixedSurfaceMaterialsPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, MixedSurfaceMaterialsPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.MaxDepthSearched = ThicknessTilesF.Deserialize(reader);
        this.MaxReplacedThickness = ThicknessTilesF.Deserialize(reader);
        this.MinReplacedThickness = ThicknessTilesF.Deserialize(reader);
        this.ReplacedMaterialProto = reader.ReadGenericAs<TerrainMaterialProto>();
        this.ReplacedThicknessFn = reader.ReadGenericAs<INoise2dFactory>();
        this.SlopeRestrictionEnd = Fix32.Deserialize(reader);
        this.SlopeRestrictionStart = Fix32.Deserialize(reader);
        this.SlopeTestDistance = RelTile1i.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.SourceMaterialProto = reader.ReadGenericAs<TerrainMaterialProto>();
      }

      [EditorEnforceOrder(40)]
      public TerrainMaterialProto SourceMaterialProto { get; set; }

      [EditorEnforceOrder(43)]
      public TerrainMaterialProto ReplacedMaterialProto { get; set; }

      [EditorRange(0.05000000074505806, 10.0)]
      [EditorEnforceOrder(48)]
      [EditorLabel(null, "Skip replacement if the 'Replaced Thickness function' returns value less than this.", false, false)]
      public ThicknessTilesF MinReplacedThickness { get; set; }

      [EditorLabel(null, "Maximum replaced thickness. Note that only the first matching layer is replaced.", false, false)]
      [EditorEnforceOrder(53)]
      [EditorRange(0.05000000074505806, 100.0)]
      public ThicknessTilesF MaxReplacedThickness { get; set; }

      [EditorEnforceOrder(56)]
      public INoise2dFactory ReplacedThicknessFn { get; set; }

      [EditorLabel(null, "Maximum depth searched. Note that only first three layers are searched regardless of this setting.", false, false)]
      [EditorEnforceOrder(62)]
      [EditorRange(0.0, 10.0)]
      public ThicknessTilesF MaxDepthSearched { get; set; }

      [EditorRange(0.0, 10.0)]
      [EditorEnforceOrder(67)]
      [EditorLabel(null, "When non-zero, specifies slope threshold start for replaced thickness reduction.", false, false)]
      public Fix32 SlopeRestrictionStart { get; set; }

      [EditorLabel(null, "When different than 'Max Slope Start', specifies slope threshold end for replaced thickness reduction. No thickness will be replaced beyond this thickness.", false, false)]
      [EditorEnforceOrder(73)]
      [EditorRange(0.0, 10.0)]
      public Fix32 SlopeRestrictionEnd { get; set; }

      [EditorEnforceOrder(77)]
      [EditorRange(1.0, 32.0)]
      public RelTile1i SlopeTestDistance { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(80)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMaxReplacedThickness\u003Ek__BackingField = 100.0.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMaxDepthSearched\u003Ek__BackingField = 1.0.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003CSlopeTestDistance\u003Ek__BackingField = RelTile1i.One;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        MixedSurfaceMaterialsPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MixedSurfaceMaterialsPostProcessor.Configuration) obj).SerializeData(writer));
        MixedSurfaceMaterialsPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MixedSurfaceMaterialsPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }

    private readonly struct ReplaceData
    {
      public readonly Tile2iIndex TileIndex;
      public readonly int LayerIndex;
      public readonly TerrainMaterialThicknessSlim NewLayer;

      public ReplaceData(
        Tile2iIndex tileIndex,
        int layerIndex,
        TerrainMaterialThicknessSlim newLayer)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.TileIndex = tileIndex;
        this.LayerIndex = layerIndex;
        this.NewLayer = newLayer;
      }
    }
  }
}
