// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FeatureGenerators.PolygonTerrainReplaceGenerator
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Terrain.Previews;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Numerics;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.FeatureGenerators
{
  /// <summary>
  /// Generic polygon terrain feature. Good for generating mountains or other non-plateau features.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonTerrainReplaceGenerator : 
    ITerrainFeatureGenerator,
    ITerrainFeatureBase,
    ITerrainFeatureWithPreview,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MIN_THICKNESS;
    public readonly PolygonTerrainReplaceGenerator.Configuration ConfigMutable;
    private int m_totalGeneratedChunks;
    private readonly Set<Chunk2iSlim> m_chunksWithNoContributionCache;
    [DoNotSave(0, null)]
    private Polygon2fFast m_polygonCache;
    [DoNotSave(0, null)]
    private INoise2D m_thicknessFn;
    [DoNotSave(0, null)]
    private bool m_previewsGenerated;
    [DoNotSave(0, null)]
    private bool m_isGeneratingPreviews;
    [DoNotSave(0, null)]
    private Option<PreviewHelper<HeightmapFeaturePreviewChunkData>> m_previewHelper;

    public static void Serialize(PolygonTerrainReplaceGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonTerrainReplaceGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonTerrainReplaceGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonTerrainReplaceGenerator.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      Set<Chunk2iSlim>.Serialize(this.m_chunksWithNoContributionCache, writer);
      writer.WriteInt(this.m_totalGeneratedChunks);
    }

    public static PolygonTerrainReplaceGenerator Deserialize(BlobReader reader)
    {
      PolygonTerrainReplaceGenerator replaceGenerator;
      if (reader.TryStartClassDeserialization<PolygonTerrainReplaceGenerator>(out replaceGenerator))
        reader.EnqueueDataDeserialization((object) replaceGenerator, PolygonTerrainReplaceGenerator.s_deserializeDataDelayedAction);
      return replaceGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonTerrainReplaceGenerator>(this, "ConfigMutable", (object) PolygonTerrainReplaceGenerator.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      reader.SetField<PolygonTerrainReplaceGenerator>(this, "m_chunksWithNoContributionCache", (object) Set<Chunk2iSlim>.Deserialize(reader));
      this.m_totalGeneratedChunks = reader.ReadInt();
    }

    public string Name
    {
      get
      {
        return string.Format("Polygon place: {0} at {1}", (object) this.ConfigMutable.TerrainMaterial.Strings.Name, (object) this.GetPosition().Tile2i);
      }
    }

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => true;

    public bool Is2D => false;

    public bool CanRotate => true;

    public int SortingPriority => 9100 + this.ConfigMutable.SortingPriorityAdjustment;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public PolygonTerrainReplaceGenerator(
      PolygonTerrainReplaceGenerator.Configuration initialConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_chunksWithNoContributionCache = new Set<Chunk2iSlim>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = initialConfig;
    }

    public Tile2f GetPosition()
    {
      Tile2f zero = Tile2f.Zero;
      foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
        zero += new RelTile2f(vertex);
      return zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length;
    }

    public HandleData? GetHandleData()
    {
      return new HandleData?(new HandleData(this.GetPosition(), (ColorRgba) 8940732, (Option<string>) this.ConfigMutable.TerrainMaterial.MinedProduct.Graphics.IconPath, ColorRgba.White));
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
      return new RectangleTerrainArea2i(tile2i, tile2iCeiled - tile2i).ExtendBy(this.ConfigMutable.MaxInfluenceDistance.Value);
    }

    public bool ValidateConfig(IResolver resolver, Lyst<string> errors) => true;

    public bool Initialize(
      Chunk64Area generatedArea,
      RelTile2i terrainSize,
      int initialMapCreationSaveVersion,
      IResolver resolver,
      ITerrainExtraDataRegistrator extraDataReg)
    {
      this.m_totalGeneratedChunks = 0;
      string error;
      if (this.tryInitialize(resolver, out error))
        return true;
      Log.Error(error);
      return false;
    }

    private bool tryInitialize(IResolver resolver, out string error)
    {
      if (this.m_polygonCache != null)
      {
        error = "";
        return true;
      }
      Dict<string, object> extraArgs = new Dict<string, object>()
      {
        {
          "Polygon".LowerFirstChar(),
          (object) this.ConfigMutable.Polygon.GetPolygon()
        }
      };
      if (!this.ConfigMutable.ThicknessFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_thicknessFn, out error))
      {
        error = "Failed to initialize surface cover thickness function: " + error;
        return false;
      }
      if (this.ConfigMutable.Polygon.TryGetFastPolygon(out this.m_polygonCache, out error))
        return true;
      error = "Failed to initialize polygon: " + error;
      return false;
    }

    public void Reset()
    {
      this.m_polygonCache = (Polygon2fFast) null;
      this.m_thicknessFn = (INoise2D) null;
      this.ConfigMutable.TotalGeneratedChunks = this.m_totalGeneratedChunks;
      this.ConfigMutable.ChunksWithNoContribution = this.m_chunksWithNoContributionCache.Count;
      this.m_previewsGenerated = false;
    }

    public void ClearCaches() => this.m_chunksWithNoContributionCache.Clear();

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

    public TerrainFeatureResourceInfo? GetResourceInfo() => new TerrainFeatureResourceInfo?();

    public void GenerateChunkThreadSafe(TerrainGeneratorChunkData data)
    {
      if (this.m_polygonCache == null)
      {
        Log.Error("Not initialized.");
      }
      else
      {
        lock (this.m_chunksWithNoContributionCache)
        {
          ++this.m_totalGeneratedChunks;
          if (this.m_chunksWithNoContributionCache.Contains(data.Chunk.AsSlim))
            return;
        }
        TerrainMaterialSlimId slimId = this.ConfigMutable.TerrainMaterial.SlimId;
        bool flag = true;
        int num1 = this.ConfigMutable.MaxInfluenceDistance.Value.Squared();
        int y = 0;
        int num2 = 0;
        while (y < 64)
        {
          for (int x = 0; x < 64; ++x)
          {
            int tileIndex = num2 + x;
            Vector2f vector2f = (data.Area.Origin + new RelTile2i(x, y)).Vector2f;
            if (this.m_polygonCache.Contains(vector2f) || !(this.m_polygonCache.DistanceSqrTo(vector2f) > num1))
            {
              ThicknessTilesF thickness = this.m_thicknessFn.GetValue(vector2f).ToFix32().TilesThick();
              if (!(thickness < PolygonTerrainReplaceGenerator.MIN_THICKNESS))
              {
                if (this.ConfigMutable.TerrainBlendHeightRange.IsPositive)
                  thickness = thickness.Value.SmoothApproachZero(this.ConfigMutable.TerrainBlendHeightRange.Value).TilesThick();
                flag = false;
                data.SetTopMaterial(tileIndex, slimId, thickness);
              }
            }
          }
          ++y;
          num2 += 64;
        }
        if (!flag)
          return;
        lock (this.m_chunksWithNoContributionCache)
          this.m_chunksWithNoContributionCache.Add(data.Chunk.AsSlim);
      }
    }

    public void InitializePreview(IResolver resolver)
    {
    }

    public bool TryGetPreviews(
      IResolver resolver,
      int timeBudgetMs,
      out IEnumerable<ITerrainFeaturePreview> previews)
    {
      PreviewHelper<HeightmapFeaturePreviewChunkData> previewHelper = this.m_previewHelper.ValueOrNull;
      if (previewHelper == null)
        this.m_previewHelper = (Option<PreviewHelper<HeightmapFeaturePreviewChunkData>>) (previewHelper = new PreviewHelper<HeightmapFeaturePreviewChunkData>());
      if (!this.m_previewsGenerated || this.m_isGeneratingPreviews)
      {
        if (!this.tryInitialize(resolver, out string _))
        {
          previews = (IEnumerable<ITerrainFeaturePreview>) null;
          previewHelper.ClearPreviews();
          this.m_previewsGenerated = true;
          return false;
        }
        this.generatePreviews(resolver, previewHelper, timeBudgetMs);
      }
      previews = (IEnumerable<ITerrainFeaturePreview>) previewHelper.GetPreviews();
      return true;
    }

    private void generatePreviews(
      IResolver resolver,
      PreviewHelper<HeightmapFeaturePreviewChunkData> previewHelper,
      int timeBudgetMs)
    {
      Assert.That<Polygon2fFast>(this.m_polygonCache).IsNotNull<Polygon2fFast>();
      TerrainManager terrainManager = resolver.Resolve<TerrainManager>();
      bool isComplete;
      previewHelper.GeneratePreviewsInParallel(this.GetBoundingBox(), !this.m_previewsGenerated, new Action<HeightmapFeaturePreviewChunkData>(processChunk), timeBudgetMs, out isComplete);
      this.m_previewsGenerated = true;
      this.m_isGeneratingPreviews = !isComplete;

      void processChunk(HeightmapFeaturePreviewChunkData chunkData)
      {
        int num1 = this.ConfigMutable.MaxInfluenceDistance.Value.Squared();
        Chunk2i chunk = chunkData.Chunk;
        HeightTilesF[] heightTilesFArray = terrainManager.GetMutableTerrainDataRef().HeightSnapshot.ValueOrNull;
        if (heightTilesFArray == null)
        {
          Log.Warning("No height snapshot found.");
          heightTilesFArray = terrainManager.GetMutableTerrainDataRef().Heights;
        }
        Tile2i tile2i = chunkData.Chunk.Tile2i;
        int num2 = tile2i.Y * terrainManager.TerrainWidth + tile2i.X;
        int y = 0;
        int num3 = 0;
        while (y < 65)
        {
          for (int x = 0; x < 65; ++x)
          {
            int index = num3 + x;
            Vector2f vector2f = (chunk.Tile2i + new RelTile2i(x, y)).Vector2f;
            if (!this.m_polygonCache.Contains(vector2f) && this.m_polygonCache.DistanceSqrTo(vector2f) > num1)
            {
              chunkData.Heights[index] = HeightTilesF.MinValue;
            }
            else
            {
              ThicknessTilesF thicknessTilesF = this.m_thicknessFn.GetValue(vector2f).ToFix32().TilesThick();
              chunkData.Heights[index] = !(thicknessTilesF < PolygonTerrainReplaceGenerator.MIN_THICKNESS) ? heightTilesFArray[num2 + x] - thicknessTilesF : HeightTilesF.MinValue;
            }
          }
          ++y;
          num3 += 65;
          num2 += terrainManager.TerrainWidth;
        }
        chunkData.SetDirty(true);
      }
    }

    static PolygonTerrainReplaceGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonTerrainReplaceGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonTerrainReplaceGenerator) obj).SerializeData(writer));
      PolygonTerrainReplaceGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonTerrainReplaceGenerator) obj).DeserializeData(reader));
      PolygonTerrainReplaceGenerator.MIN_THICKNESS = (1.0 / 16.0).TilesThick();
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        PolygonTerrainReplaceGenerator.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonTerrainReplaceGenerator.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonTerrainReplaceGenerator.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        RelTile1i.Serialize(this.MaxInfluenceDistance, writer);
        Polygon2fMutable.Serialize(this.Polygon, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        RelTile1f.Serialize(this.TerrainBlendHeightRange, writer);
        writer.WriteGeneric<TerrainMaterialProto>(this.TerrainMaterial);
        writer.WriteGeneric<INoise2dFactory>(this.ThicknessFn);
      }

      public static PolygonTerrainReplaceGenerator.Configuration Deserialize(BlobReader reader)
      {
        PolygonTerrainReplaceGenerator.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonTerrainReplaceGenerator.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonTerrainReplaceGenerator.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.MaxInfluenceDistance = RelTile1i.Deserialize(reader);
        this.Polygon = Polygon2fMutable.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.TerrainBlendHeightRange = RelTile1f.Deserialize(reader);
        this.TerrainMaterial = reader.ReadGenericAs<TerrainMaterialProto>();
        this.ThicknessFn = reader.ReadGenericAs<INoise2dFactory>();
      }

      [EditorEnforceOrder(41)]
      public Polygon2fMutable Polygon { get; set; }

      [EditorEnforceOrder(44)]
      public TerrainMaterialProto TerrainMaterial { get; set; }

      [EditorRange(0.0, 200.0)]
      [EditorEnforceOrder(49)]
      [EditorLabel(null, "Distance beyond the polygon perimeter that this feature can modify terrain. Increase if the generate feature is getting cut off, decrease for better performance.", false, false)]
      public RelTile1i MaxInfluenceDistance { get; set; }

      [EditorEnforceOrder(54)]
      [EditorLabel(null, "Range of smooth blending region with terrain. Set to zero to disable it. Note that this setting won't show on the preview.", false, false)]
      public RelTile1f TerrainBlendHeightRange { get; set; }

      [EditorCollapseObject]
      [EditorEnforceOrder(59)]
      [EditorLabel(null, "2D noise function that defines thickness for surface cover material", false, false)]
      public INoise2dFactory ThicknessFn { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(64)]
      public int SortingPriorityAdjustment { get; set; }

      [EditorReadonly]
      [EditorEnforceOrder(71)]
      [EditorSection("Generator stats", null, true, false)]
      [DoNotSave(0, null)]
      public int TotalGeneratedChunks { get; set; }

      [DoNotSave(0, null)]
      [EditorEnforceOrder(76)]
      [EditorReadonly]
      public int ChunksWithNoContribution { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        PolygonTerrainReplaceGenerator.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonTerrainReplaceGenerator.Configuration) obj).SerializeData(writer));
        PolygonTerrainReplaceGenerator.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonTerrainReplaceGenerator.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
