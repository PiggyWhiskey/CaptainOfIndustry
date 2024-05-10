// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.PolygonTreeGeneratorPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Terrain.Previews;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Numerics;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>
  /// Generic polygon terrain feature. Good for generating mountains or other non-plateau features.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonTreeGeneratorPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    ITerrainFeatureWithPreview,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly PolygonTreeGeneratorPostProcessor.Configuration ConfigMutable;
    private readonly Dict<Chunk2iSlim, TileInChunk2iSlim[]> m_treesPositionsCache;
    [DoNotSave(0, null)]
    private Polygon2fFast m_polygonCache;
    [DoNotSave(0, null)]
    private INoise2D m_spacingFn;
    [DoNotSave(0, null)]
    private INoise2D m_spawningFn;
    [DoNotSave(0, null)]
    private GeneratedTreesData m_treesData;
    [DoNotSave(0, null)]
    private bool m_previewsGenerated;
    [DoNotSave(0, null)]
    private bool m_isGeneratingPreviews;
    [DoNotSave(0, null)]
    private Option<PreviewHelper<PointFeaturePreviewChunkData>> m_previewHelper;
    [DoNotSave(0, null)]
    private TerrainManager m_terrain;

    public static void Serialize(PolygonTreeGeneratorPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonTreeGeneratorPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonTreeGeneratorPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonTreeGeneratorPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      Dict<Chunk2iSlim, TileInChunk2iSlim[]>.Serialize(this.m_treesPositionsCache, writer);
    }

    public static PolygonTreeGeneratorPostProcessor Deserialize(BlobReader reader)
    {
      PolygonTreeGeneratorPostProcessor generatorPostProcessor;
      if (reader.TryStartClassDeserialization<PolygonTreeGeneratorPostProcessor>(out generatorPostProcessor))
        reader.EnqueueDataDeserialization((object) generatorPostProcessor, PolygonTreeGeneratorPostProcessor.s_deserializeDataDelayedAction);
      return generatorPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonTreeGeneratorPostProcessor>(this, "ConfigMutable", (object) PolygonTreeGeneratorPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      reader.SetField<PolygonTreeGeneratorPostProcessor>(this, "m_treesPositionsCache", (object) Dict<Chunk2iSlim, TileInChunk2iSlim[]>.Deserialize(reader));
    }

    public string Name => "Polygon tree generator";

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

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 2000;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public PolygonTreeGeneratorPostProcessor(
      PolygonTreeGeneratorPostProcessor.Configuration initialConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_treesPositionsCache = new Dict<Chunk2iSlim, TileInChunk2iSlim[]>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = initialConfig;
    }

    public HandleData? GetHandleData()
    {
      Tile2f zero = Tile2f.Zero;
      foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
        zero += new RelTile2f(vertex);
      return new HandleData?(new HandleData(zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length, (ColorRgba) 3568175, (Option<string>) "Assets/Unity/MapEditor/Icons/Forest.svg", (ColorRgba) 6788705));
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
      RectangleTerrainArea2i boundingBox = new RectangleTerrainArea2i(tile2i, tile2iCeiled - tile2i).ExtendBy(this.ConfigMutable.MaxInfluenceDistance.Value);
      if (this.m_terrain != null)
        boundingBox = boundingBox.ClampToTerrainBounds(this.m_terrain);
      return boundingBox;
    }

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors) => true;

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      if (!this.tryInitialize(resolver))
        return false;
      if (extraDataReg.TryGetOrCreateExtraData<GeneratedTreesData>(out this.m_treesData))
        return true;
      Log.Error("Failed to obtain trees data.");
      return false;
    }

    private bool tryInitialize(IResolver resolver)
    {
      if (this.m_polygonCache != null)
        return true;
      this.m_terrain = resolver.Resolve<TerrainManager>();
      Dict<string, object> extraArgs = new Dict<string, object>()
      {
        {
          "Polygon".LowerFirstChar(),
          (object) this.ConfigMutable.Polygon.GetPolygon()
        }
      };
      string error;
      if (!this.ConfigMutable.SpacingFunction.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_spacingFn, out error))
      {
        Log.Error("Failed to initialize base height function: " + error);
        return false;
      }
      if (!this.ConfigMutable.SpawnFunction.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_spawningFn, out error))
      {
        Log.Error("Failed to initialize base height function: " + error);
        return false;
      }
      if (this.ConfigMutable.Polygon.TryGetFastPolygon(out this.m_polygonCache, out error))
        return true;
      Log.Error("Failed to initialize polygon: " + error);
      return false;
    }

    public void Reset()
    {
      this.m_polygonCache = (Polygon2fFast) null;
      this.m_spacingFn = (INoise2D) null;
      this.m_spawningFn = (INoise2D) null;
      this.m_treesData = (GeneratedTreesData) null;
      this.m_previewsGenerated = false;
    }

    public void ClearCaches() => this.m_treesPositionsCache.Clear();

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

    private void placeTreesOnChunk(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData? dataReadOnly,
      bool isPreview)
    {
      if (this.m_polygonCache == null)
      {
        Log.Error("Not initialized.");
      }
      else
      {
        int weightsSum = this.ConfigMutable.Trees.Sum<TreeWithWeight>((Func<TreeWithWeight, int>) (x => x.Weight));
        if (weightsSum <= 0)
          return;
        Chunk2iSlim asSlim = chunk.AsSlim;
        Tile2i tile2i1 = chunk.Tile2i;
        XorRsr128PlusGenerator rsr128PlusGenerator = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 1UL, 2UL);
        rsr128PlusGenerator.SeedFast(chunk.X, chunk.Y);
        rsr128PlusGenerator.Jump();
        TileInChunk2iSlim[] array;
        lock (this.m_treesPositionsCache)
          this.m_treesPositionsCache.TryGetValue(asSlim, out array);
        if (array == null)
        {
          RelTile1f relTile1f = this.ConfigMutable.MinSpacing;
          Fix32 other = relTile1f.Value.Max(Fix32.One);
          relTile1f = this.ConfigMutable.MaxSpacing;
          Fix32 fix32 = relTile1f.Value.Min((Fix32) 10);
          Fix64 fix64_1 = fix32.ToFix64();
          int intCeiled = fix64_1.ToIntCeiled();
          int num1 = this.ConfigMutable.MaxInfluenceDistance.Value.Squared();
          int spacingStride = 64 + intCeiled * 2;
          Fix32[] randomForSpacing = new Fix32[spacingStride * spacingStride];
          Vector2f vector2f1 = tile2i1.Vector2f - (Fix32) intCeiled;
          for (int y = 0; y < spacingStride; ++y)
          {
            int num2 = y * spacingStride;
            for (int x = 0; x < spacingStride; ++x)
              randomForSpacing[num2 + x] = this.m_spawningFn.GetValue(vector2f1 + new Vector2f((Fix32) x, (Fix32) y)).ToFix32();
          }
          BitMap bitMap = new BitMap(spacingStride * spacingStride);
          Lyst<TileInChunk2iSlim> lyst = new Lyst<TileInChunk2iSlim>();
          int num3 = dataReadOnly.HasValue ? dataReadOnly.GetValueOrDefault().GetTileIndex(tile2i1, dataOrigin) : this.m_terrain.GetTileIndex(tile2i1).Value;
          int num4 = dataReadOnly.HasValue ? dataReadOnly.GetValueOrDefault().Width : this.m_terrain.TerrainWidth;
          int num5 = intCeiled * spacingStride;
          int slopeCheckDistance = this.ConfigMutable.SlopeCheckDistance;
          HeightTilesF maxHeightDelta = this.ConfigMutable.MaxHeightDelta;
          ThicknessTilesF materialCheckDepth = this.ConfigMutable.MinFarmableMaterialThickness * 2;
          int y1 = 0;
          while (y1 < 64)
          {
            for (int x = 0; x < 64; ++x)
            {
              if (!bitMap.IsSet(num5 + x))
              {
                int dataIndex = num3 + x;
                TerrainManager.TerrainData data;
                if (dataReadOnly.HasValue)
                {
                  data = dataReadOnly.Value;
                  if (((int) data.Flags[dataIndex] & 5) != 0)
                    continue;
                }
                else
                  data = this.m_terrain.GetMutableTerrainDataRef();
                TileMaterialLayers layers = data.MaterialLayers[dataIndex];
                if (layers.Count > 0 && (!this.ConfigMutable.MinFarmableMaterialThickness.IsPositive || checkEnoughFarmableMaterial(this.ConfigMutable.MinFarmableMaterialThickness)) && (!this.ConfigMutable.LimitToMaterialProto.HasValue || !(layers.First.SlimId != this.ConfigMutable.LimitToMaterialProto.Value.SlimId)))
                {
                  HeightTilesF plantedHeight = data.Heights[dataIndex];
                  Tile2i tile2i2 = tile2i1 + new RelTile2i(x, y1);
                  RelTile2i relTile2i = dataReadOnly.HasValue ? dataReadOnly.GetValueOrDefault().Size : this.m_terrain.TerrainSize;
                  if ((tile2i2.X - slopeCheckDistance < dataOrigin.X || checkHeight(-slopeCheckDistance)) && (tile2i2.Y - slopeCheckDistance < dataOrigin.Y || checkHeight(-slopeCheckDistance * num4)) && (tile2i2.X + slopeCheckDistance >= dataOrigin.X + relTile2i.X || checkHeight(slopeCheckDistance)) && (tile2i2.Y + slopeCheckDistance >= dataOrigin.Y + relTile2i.Y || checkHeight(slopeCheckDistance * num4)))
                  {
                    Vector2f vector2f2 = tile2i2.Vector2f;
                    if (this.m_polygonCache.Contains(vector2f2) || !(this.m_polygonCache.DistanceSqrTo(vector2f2) > num1))
                    {
                      Fix64 fix64_2 = this.m_spacingFn.GetValue(vector2f2);
                      if (!(fix64_2 > fix64_1))
                      {
                        fix32 = fix64_2.ToFix32();
                        int probabilistically = fix32.Max(other).ToIntRoundedProbabilistically((IRandom) rsr128PlusGenerator);
                        if (isLargestInRadius(num5 + x, probabilistically))
                        {
                          lyst.Add(tile2i2.TileInChunkCoordSlim);
                          int intFloored = other.ToIntFloored();
                          int num6 = intFloored * intFloored;
                          x += intFloored - 1;
                          for (int index1 = -intFloored; index1 <= intFloored; ++index1)
                          {
                            int num7 = index1 * index1;
                            for (int index2 = -intFloored; index2 <= intFloored; ++index2)
                            {
                              if (num7 + index2 + index2 <= num6)
                                bitMap.SetBit(num5 + index1 * spacingStride + x + index2);
                            }
                          }
                        }
                      }
                    }
                  }

                  bool checkHeight(int deltaI)
                  {
                    return data.Heights[dataIndex + deltaI].IsNear(plantedHeight, maxHeightDelta);
                  }
                }

                bool checkEnoughFarmableMaterial(ThicknessTilesF farmableRemaining)
                {
                  if (layers.First.SlimId.ToFull(this.m_terrain).IsFarmable)
                  {
                    farmableRemaining -= layers.First.Thickness;
                    if (farmableRemaining.IsNotPositive)
                      return true;
                  }
                  if (layers.First.Thickness >= materialCheckDepth)
                    return false;
                  if (layers.Second.SlimId.ToFull(this.m_terrain).IsFarmable)
                  {
                    farmableRemaining -= layers.Second.Thickness;
                    if (farmableRemaining.IsNotPositive)
                      return true;
                  }
                  if (layers.First.Thickness + layers.Second.Thickness >= materialCheckDepth || !layers.Third.SlimId.ToFull(this.m_terrain).IsFarmable)
                    return false;
                  farmableRemaining -= layers.Third.Thickness;
                  return farmableRemaining.IsNotPositive;
                }
              }
            }
            ++y1;
            num5 += spacingStride;
            num3 += num4;
          }
          array = lyst.ToArray();
          lock (this.m_treesPositionsCache)
            this.m_treesPositionsCache.AddAndAssertNew(asSlim, array);

          bool isLargestInRadius(int centerIndex, int radius)
          {
            Fix32 fix32 = randomForSpacing[centerIndex];
            int num1 = radius * radius;
            int num2 = centerIndex - radius * spacingStride;
            int num3 = -radius;
            while (num3 <= radius)
            {
              int num4 = num3 * num3;
              for (int index = -radius; index <= radius; ++index)
              {
                if (num4 + index * index <= num1 && randomForSpacing[num2 + index] >= fix32 && num2 + index != centerIndex)
                  return false;
              }
              ++num3;
              num2 += spacingStride;
            }
            return true;
          }
        }
        if (array.Length == 0 || isPreview)
          return;
        GeneratedTreesData.Chunk chunk1 = this.m_treesData.GetOrCreateChunk(chunk);
        foreach (TileInChunk2iSlim tileInChunk2iSlim in array)
        {
          Tile2i tile2i3 = chunk + tileInChunk2iSlim;
          Vector2f vector2f = tile2i3.Vector2f;
          bool flag = false;
          foreach (Polygon2fFast clearedPolygon in chunk1.ClearedPolygons)
          {
            if (clearedPolygon.Contains(vector2f))
            {
              flag = true;
              break;
            }
          }
          if (!flag)
          {
            rsr128PlusGenerator.SeedFast(tile2i3.X, tile2i3.Y);
            TreeProto treeProto = this.ConfigMutable.Trees.SampleRandomWeighted<TreeWithWeight>((IRandom) rsr128PlusGenerator, weightsSum, (Func<TreeWithWeight, int>) (x => x.Weight)).TreeProto;
            chunk1.Trees.TryAdd(new TreeId(tile2i3.X, tile2i3.Y), new TreeDataBase(treeProto, tile2i3.CenterTile2f, rsr128PlusGenerator.NextAngleSlim(), treeProto.GetRandomBaseScale((IRandom) rsr128PlusGenerator)));
          }
        }
      }
    }

    public void AnalyzeChunkThreadSafe(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly,
      int pass)
    {
      this.placeTreesOnChunk(chunk, dataOrigin, new TerrainManager.TerrainData?(dataReadOnly), false);
    }

    public void ApplyChunkChanges(
      Chunk2i chunkOrigin,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
    }

    public void InitializePreview(IResolver resolver)
    {
    }

    public bool TryGetPreviews(
      IResolver resolver,
      int timeBudgetMs,
      out IEnumerable<ITerrainFeaturePreview> previews)
    {
      PreviewHelper<PointFeaturePreviewChunkData> previewHelper = this.m_previewHelper.ValueOrNull;
      if (previewHelper == null)
        this.m_previewHelper = (Option<PreviewHelper<PointFeaturePreviewChunkData>>) (previewHelper = new PreviewHelper<PointFeaturePreviewChunkData>());
      if (!this.m_previewsGenerated || this.m_isGeneratingPreviews)
      {
        if (!this.tryInitialize(resolver))
        {
          previews = (IEnumerable<ITerrainFeaturePreview>) null;
          previewHelper.ClearPreviews();
          this.m_previewsGenerated = true;
          return false;
        }
        this.m_treesPositionsCache.Clear();
        this.generatePreviews(previewHelper, timeBudgetMs);
      }
      previews = (IEnumerable<ITerrainFeaturePreview>) previewHelper.GetPreviews();
      return true;
    }

    private void generatePreviews(
      PreviewHelper<PointFeaturePreviewChunkData> previewHelper,
      int timeBudgetMs)
    {
      Assert.That<Polygon2fFast>(this.m_polygonCache).IsNotNull<Polygon2fFast>();
      RectangleTerrainArea2i boundingBox = this.GetBoundingBox();
      bool isComplete;
      // ISSUE: method pointer
      previewHelper.GeneratePreviewsInParallel(boundingBox, !this.m_previewsGenerated, new Action<PointFeaturePreviewChunkData>((object) this, __methodptr(\u003CgeneratePreviews\u003Eg__processChunk\u007C64_0)), timeBudgetMs, out isComplete);
      this.m_previewsGenerated = true;
      this.m_isGeneratingPreviews = !isComplete;
    }

    static PolygonTreeGeneratorPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonTreeGeneratorPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonTreeGeneratorPostProcessor) obj).SerializeData(writer));
      PolygonTreeGeneratorPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonTreeGeneratorPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfigWithInit, ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      [DoNotSave(0, null)]
      [EditorEnforceOrder(66)]
      public Action AddNewTreeOption;

      public static void Serialize(
        PolygonTreeGeneratorPostProcessor.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonTreeGeneratorPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonTreeGeneratorPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Option<TerrainMaterialProto>.Serialize(this.LimitToMaterialProto, writer);
        HeightTilesF.Serialize(this.MaxHeightDelta, writer);
        RelTile1i.Serialize(this.MaxInfluenceDistance, writer);
        RelTile1f.Serialize(this.MaxSpacing, writer);
        ThicknessTilesF.Serialize(this.MinFarmableMaterialThickness, writer);
        RelTile1f.Serialize(this.MinSpacing, writer);
        Polygon2fMutable.Serialize(this.Polygon, writer);
        writer.WriteInt(this.SlopeCheckDistance);
        writer.WriteInt(this.SortingPriorityAdjustment);
        writer.WriteGeneric<INoise2dFactory>(this.SpacingFunction);
        writer.WriteGeneric<INoise2dFactory>(this.SpawnFunction);
        Lyst<TreeWithWeight>.Serialize(this.Trees, writer);
      }

      public static PolygonTreeGeneratorPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        PolygonTreeGeneratorPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonTreeGeneratorPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonTreeGeneratorPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.LimitToMaterialProto = Option<TerrainMaterialProto>.Deserialize(reader);
        this.MaxHeightDelta = HeightTilesF.Deserialize(reader);
        this.MaxInfluenceDistance = RelTile1i.Deserialize(reader);
        this.MaxSpacing = RelTile1f.Deserialize(reader);
        this.MinFarmableMaterialThickness = ThicknessTilesF.Deserialize(reader);
        this.MinSpacing = RelTile1f.Deserialize(reader);
        this.Polygon = Polygon2fMutable.Deserialize(reader);
        this.SlopeCheckDistance = reader.ReadInt();
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.SpacingFunction = reader.ReadGenericAs<INoise2dFactory>();
        this.SpawnFunction = reader.ReadGenericAs<INoise2dFactory>();
        this.Trees = Lyst<TreeWithWeight>.Deserialize(reader);
      }

      [EditorEnforceOrder(63)]
      [EditorLabel(null, "Trees are selected randomly from the following list. Each tree has probability of being selected proportional to its weight.", false, false)]
      [EditorCollection(true, true)]
      public Lyst<TreeWithWeight> Trees { get; set; }

      [EditorEnforceOrder(70)]
      public Polygon2fMutable Polygon { get; set; }

      [EditorLabel(null, "Distance beyond the polygon perimeter that this feature can modify terrain. Increase if the generate feature is getting cut off, decrease for better performance.", false, false)]
      [EditorEnforceOrder(74)]
      public RelTile1i MaxInfluenceDistance { get; set; }

      [EditorEnforceOrder(78)]
      [EditorRange(1.0, 10.0)]
      public RelTile1f MinSpacing { get; set; }

      [EditorEnforceOrder(82)]
      [EditorRange(1.0, 10.0)]
      public RelTile1f MaxSpacing { get; set; }

      [EditorEnforceOrder(86)]
      [EditorRange(0.0, 2.0)]
      public ThicknessTilesF MinFarmableMaterialThickness { get; set; }

      [EditorEnforceOrder(91)]
      [EditorLabel(null, "Check distance for max height delta.", false, false)]
      [EditorRange(1.0, 10.0)]
      public int SlopeCheckDistance { get; set; }

      [EditorEnforceOrder(96)]
      [EditorLabel(null, "Maximum height difference at the 'step check distance'.", false, false)]
      [EditorRange(1.0, 10.0)]
      public HeightTilesF MaxHeightDelta { get; set; }

      [EditorEnforceOrder(100)]
      [EditorLabel(null, "Limits tree spawning to selected material.", false, false)]
      public Option<TerrainMaterialProto> LimitToMaterialProto { get; set; }

      [EditorEnforceOrder(106)]
      [EditorCollapseObject]
      [EditorLabel(null, "Determines tree spacing. No trees are planted if value of this function is higher than max spacing.", false, false)]
      public INoise2dFactory SpacingFunction { get; set; }

      [EditorLabel(null, "Base function for tree spawning. Do not edit unless you understand the tree planting algorithm.", false, false)]
      [EditorEnforceOrder(112)]
      [EditorCollapseObject]
      public INoise2dFactory SpawnFunction { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(116)]
      public int SortingPriorityAdjustment { get; set; }

      public void InitializeInMapEditor(IResolver resolver)
      {
        TreeProto treeProto = resolver.Resolve<ProtosDb>().AnyOrDefault<TreeProto>();
        this.AddNewTreeOption = (Action) (() => this.Trees.Add(new TreeWithWeight(treeProto, 10)));
      }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMinFarmableMaterialThickness\u003Ek__BackingField = 0.5.TilesThick();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        PolygonTreeGeneratorPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonTreeGeneratorPostProcessor.Configuration) obj).SerializeData(writer));
        PolygonTreeGeneratorPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonTreeGeneratorPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
