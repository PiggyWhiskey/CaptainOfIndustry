// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FeatureGenerators.PolygonSurfaceFeatureGenerator
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Base.Terrain.Previews;
using Mafi.Collections;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
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
  /// Polygon-based terrain feature that cuts the feature using a surface function to generate a plateau or similar.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonSurfaceFeatureGenerator : 
    ITerrainFeatureWithOceanCoast,
    ITerrainFeatureGenerator,
    ITerrainFeatureBase,
    ITerrainFeatureWithPreview,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly PolygonSurfaceFeatureGenerator.Configuration ConfigMutable;
    private int m_totalGeneratedChunks;
    private readonly Set<Chunk2iSlim> m_chunksWithNoContributionCache;
    private readonly Set<Chunk2iSlim> m_chunksWithSurfaceAtMaxThicknessCache;
    [DoNotSave(0, null)]
    private Polygon2fFast m_polygonCache;
    [DoNotSave(0, null)]
    private INoise2D m_baseHeightFn;
    [DoNotSave(0, null)]
    private INoise2D m_surfaceHeightFn;
    [DoNotSave(0, null)]
    private INoise2D m_maxSurfaceThicknessFn;
    [DoNotSave(0, null)]
    private ThicknessTilesF m_unaccountedTranslation;
    [DoNotSave(0, null)]
    private bool m_previewsGenerated;
    [DoNotSave(0, null)]
    private bool m_isGeneratingPreviews;
    [DoNotSave(0, null)]
    private Option<PreviewHelper<HeightmapFeaturePreviewChunkData>> m_previewHelper;

    public static void Serialize(PolygonSurfaceFeatureGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonSurfaceFeatureGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonSurfaceFeatureGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonSurfaceFeatureGenerator.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      Set<Chunk2iSlim>.Serialize(this.m_chunksWithNoContributionCache, writer);
      Set<Chunk2iSlim>.Serialize(this.m_chunksWithSurfaceAtMaxThicknessCache, writer);
      writer.WriteInt(this.m_totalGeneratedChunks);
    }

    public static PolygonSurfaceFeatureGenerator Deserialize(BlobReader reader)
    {
      PolygonSurfaceFeatureGenerator featureGenerator;
      if (reader.TryStartClassDeserialization<PolygonSurfaceFeatureGenerator>(out featureGenerator))
        reader.EnqueueDataDeserialization((object) featureGenerator, PolygonSurfaceFeatureGenerator.s_deserializeDataDelayedAction);
      return featureGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonSurfaceFeatureGenerator>(this, "ConfigMutable", (object) PolygonSurfaceFeatureGenerator.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      reader.SetField<PolygonSurfaceFeatureGenerator>(this, "m_chunksWithNoContributionCache", (object) Set<Chunk2iSlim>.Deserialize(reader));
      reader.SetField<PolygonSurfaceFeatureGenerator>(this, "m_chunksWithSurfaceAtMaxThicknessCache", (object) Set<Chunk2iSlim>.Deserialize(reader));
      this.m_totalGeneratedChunks = reader.ReadInt();
    }

    public string Name
    {
      get
      {
        return string.Format("Polygon surface: {0} at {1}", (object) this.ConfigMutable.BaseMaterial.Strings.Name, (object) this.GetPosition().Tile2i);
      }
    }

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => false;

    public bool IsImportable => true;

    public bool Is2D => false;

    public bool CanRotate => true;

    public int SortingPriority
    {
      get
      {
        return 2000 - this.ConfigMutable.BaseHeight.Value + this.ConfigMutable.SortingPriorityAdjustment;
      }
    }

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public PolygonSurfaceFeatureGenerator(
      PolygonSurfaceFeatureGenerator.Configuration initialConfig)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_chunksWithNoContributionCache = new Set<Chunk2iSlim>();
      this.m_chunksWithSurfaceAtMaxThicknessCache = new Set<Chunk2iSlim>();
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
      Tile2f zero = Tile2f.Zero;
      foreach (Vector2f vertex in this.ConfigMutable.Polygon.Vertices)
        zero += new RelTile2f(vertex);
      return new HandleData?(new HandleData(zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length, (ColorRgba) 10913635, (Option<string>) "Assets/Unity/MapEditor/Icons/Plateou.svg", (ColorRgba) 10913635, new HeightTilesF?(this.ConfigMutable.BaseHeight.HeightTilesF)));
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

    public void GetOceanFloorHeightData(Lyst<Line2f> data)
    {
      if (!this.ConfigMutable.ContributesToOceanCoast)
        return;
      foreach (Line2f enumerateEdge in this.ConfigMutable.Polygon.EnumerateEdges())
        data.Add(enumerateEdge);
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
      return this.tryInitialize(resolver);
    }

    private bool tryInitialize(IResolver resolver)
    {
      if (this.m_polygonCache != null)
        return true;
      Dict<string, object> extraArgs = new Dict<string, object>()
      {
        {
          "Polygon".LowerFirstChar(),
          (object) this.ConfigMutable.Polygon.GetPolygon()
        }
      };
      string error;
      if (!this.ConfigMutable.BaseHeightFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_baseHeightFn, out error))
      {
        Log.Error("Failed to initialize base height function: " + error);
        return false;
      }
      if (!this.ConfigMutable.SurfaceHeightFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_surfaceHeightFn, out error))
      {
        Log.Error("Failed to initialize surface height function: " + error);
        return false;
      }
      if (!this.ConfigMutable.MaxSurfaceThicknessFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_maxSurfaceThicknessFn, out error))
      {
        Log.Error("Failed to initialize max surface depth function: " + error);
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
      this.m_baseHeightFn = (INoise2D) null;
      this.m_surfaceHeightFn = (INoise2D) null;
      this.m_maxSurfaceThicknessFn = (INoise2D) null;
      this.ConfigMutable.TotalGeneratedChunks = this.m_totalGeneratedChunks;
      this.ConfigMutable.ChunksWithNoContribution = this.m_chunksWithNoContributionCache.Count;
      this.ConfigMutable.ChunksWithSurfaceAtMaxThickness = this.m_chunksWithSurfaceAtMaxThicknessCache.Count;
      this.m_previewsGenerated = false;
    }

    public void ClearCaches()
    {
      this.m_chunksWithNoContributionCache.Clear();
      this.m_chunksWithSurfaceAtMaxThicknessCache.Clear();
    }

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      this.ConfigMutable.Polygon.TranslateBy(delta.Xy.Vector2f);
      this.m_unaccountedTranslation += delta.Z.TilesThick();
      ThicknessTilesI roundedThicknessTilesI = this.m_unaccountedTranslation.RoundedThicknessTilesI;
      this.ConfigMutable.BaseHeight += roundedThicknessTilesI;
      this.ConfigMutable.BaseHeight = this.ConfigMutable.BaseHeight.Clamp(-2048.TilesHigh(), 2048.TilesHigh());
      this.m_unaccountedTranslation -= roundedThicknessTilesI.ThicknessTilesF;
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
        Log.Error("Not initialized.");
      else if ((Proto) this.ConfigMutable.BaseMaterial == (Proto) null)
        Log.Error("Polygon surface feature '" + this.Name + "' has null BaseMaterial.");
      else if ((Proto) this.ConfigMutable.SurfaceMaterial == (Proto) null)
      {
        Log.Error("Polygon surface feature '" + this.Name + "' has null SurfaceMaterial.");
      }
      else
      {
        bool flag1;
        lock (this.m_chunksWithNoContributionCache)
        {
          ++this.m_totalGeneratedChunks;
          if (this.m_chunksWithNoContributionCache.Contains(data.Chunk.AsSlim))
            return;
          flag1 = this.m_chunksWithSurfaceAtMaxThicknessCache.Contains(data.Chunk.AsSlim);
        }
        TerrainMaterialSlimId slimId1 = this.ConfigMutable.BaseMaterial.SlimId;
        TerrainMaterialSlimId slimId2 = this.ConfigMutable.SurfaceMaterial.SlimId;
        HeightTilesI baseHeight = this.ConfigMutable.BaseHeight;
        int num1 = this.ConfigMutable.MaxInfluenceDistance.Value.Squared();
        if (flag1)
        {
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
                HeightTilesF heightTilesF = baseHeight.HeightTilesF + this.m_surfaceHeightFn.GetValue(vector2f).TilesThick();
                if (!this.ConfigMutable.AllowNonIntegerSurfaceHeights)
                  heightTilesF = heightTilesF.TilesHeightRounded.HeightTilesF;
                ThicknessTilesF thicknessTilesF = heightTilesF - data.Heights[tileIndex];
                if (!thicknessTilesF.IsNegative)
                {
                  data.Heights[tileIndex] = heightTilesF;
                  ThicknessTilesF rhs = this.m_maxSurfaceThicknessFn.GetValue(vector2f).ToFix32().TilesThick();
                  if (rhs < thicknessTilesF)
                    data.PushLayerOnTop(tileIndex, slimId1, thicknessTilesF - rhs);
                  data.SetTopMaterial(tileIndex, slimId2, thicknessTilesF.Min(rhs));
                  int num3 = this.ConfigMutable.AllowNonIntegerSurfaceHeights ? 1 : 0;
                }
              }
            }
            ++y;
            num2 += 64;
          }
        }
        else
        {
          bool flag2 = true;
          bool flag3 = true;
          Fix32 surfaceDepthMult = this.ConfigMutable.SurfaceDepthMult;
          int y = 0;
          int num4 = 0;
          while (y < 64)
          {
            for (int x = 0; x < 64; ++x)
            {
              int tileIndex = num4 + x;
              Vector2f vector2f = (data.Area.Origin + new RelTile2i(x, y)).Vector2f;
              if (this.m_polygonCache.Contains(vector2f) || !(this.m_polygonCache.DistanceSqrTo(vector2f) > num1))
              {
                HeightTilesF height = baseHeight.HeightTilesF + this.m_baseHeightFn.GetValue(vector2f).TilesThick();
                HeightTilesF heightTilesF = baseHeight.HeightTilesF + this.m_surfaceHeightFn.GetValue(vector2f).TilesThick();
                if (!this.ConfigMutable.AllowNonIntegerSurfaceHeights)
                  heightTilesF = heightTilesF.TilesHeightRounded.HeightTilesF;
                if (height <= heightTilesF)
                {
                  flag3 = false;
                  if (!this.ConfigMutable.DisableSubSurfaceGeneration || !(data.Heights[tileIndex] >= height))
                  {
                    flag2 = false;
                    data.RemoveAllLayersBelow(tileIndex, height);
                    data.PushLayerOnBottom(tileIndex, slimId1, this.ConfigMutable.BaseThickness);
                    if (height > data.Heights[tileIndex])
                      data.Heights[tileIndex] = height;
                  }
                }
                else
                {
                  ThicknessTilesF thicknessTilesF1 = heightTilesF - data.Heights[tileIndex];
                  if (!thicknessTilesF1.IsNegative)
                  {
                    ThicknessTilesF rhs = surfaceDepthMult * (height - heightTilesF);
                    ThicknessTilesF thicknessTilesF2 = this.m_maxSurfaceThicknessFn.GetValue(vector2f).ToFix32().TilesThick();
                    if (rhs >= thicknessTilesF2)
                      rhs = thicknessTilesF2;
                    else
                      flag3 = false;
                    flag2 = false;
                    data.Heights[tileIndex] = heightTilesF;
                    if (rhs < thicknessTilesF1)
                      data.PushLayerOnTop(tileIndex, slimId1, thicknessTilesF1 - rhs);
                    data.SetTopMaterial(tileIndex, slimId2, thicknessTilesF1.Min(rhs));
                    int num5 = this.ConfigMutable.AllowNonIntegerSurfaceHeights ? 1 : 0;
                  }
                }
              }
            }
            ++y;
            num4 += 64;
          }
          if (flag2)
          {
            lock (this.m_chunksWithNoContributionCache)
              this.m_chunksWithNoContributionCache.Add(data.Chunk.AsSlim);
          }
          else
          {
            if (!flag3)
              return;
            lock (this.m_chunksWithNoContributionCache)
              this.m_chunksWithSurfaceAtMaxThicknessCache.Add(data.Chunk.AsSlim);
          }
        }
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
        if (!this.tryInitialize(resolver))
        {
          previews = (IEnumerable<ITerrainFeaturePreview>) null;
          previewHelper.ClearPreviews();
          this.m_previewsGenerated = true;
          return false;
        }
        this.generatePreviews(previewHelper, timeBudgetMs);
      }
      previews = (IEnumerable<ITerrainFeaturePreview>) previewHelper.GetPreviews();
      return true;
    }

    private void generatePreviews(
      PreviewHelper<HeightmapFeaturePreviewChunkData> previewHelper,
      int timeBudgetMs)
    {
      Assert.That<Polygon2fFast>(this.m_polygonCache).IsNotNull<Polygon2fFast>();
      bool isComplete;
      // ISSUE: method pointer
      previewHelper.GeneratePreviewsInParallel(this.GetBoundingBox(), !this.m_previewsGenerated, new Action<HeightmapFeaturePreviewChunkData>((object) this, __methodptr(\u003CgeneratePreviews\u003Eg__processChunk\u007C62_0)), timeBudgetMs, out isComplete);
      this.m_previewsGenerated = true;
      this.m_isGeneratingPreviews = !isComplete;
    }

    static PolygonSurfaceFeatureGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonSurfaceFeatureGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonSurfaceFeatureGenerator) obj).SerializeData(writer));
      PolygonSurfaceFeatureGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonSurfaceFeatureGenerator) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        PolygonSurfaceFeatureGenerator.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonSurfaceFeatureGenerator.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonSurfaceFeatureGenerator.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteBool(this.AllowNonIntegerSurfaceHeights);
        HeightTilesI.Serialize(this.BaseHeight, writer);
        writer.WriteGeneric<INoise2dFactory>(this.BaseHeightFn);
        writer.WriteGeneric<TerrainMaterialProto>(this.BaseMaterial);
        ThicknessTilesF.Serialize(this.BaseThickness, writer);
        writer.WriteBool(this.ContributesToOceanCoast);
        writer.WriteBool(this.DisableSubSurfaceGeneration);
        RelTile1i.Serialize(this.MaxInfluenceDistance, writer);
        writer.WriteGeneric<INoise2dFactory>(this.MaxSurfaceThicknessFn);
        Polygon2fMutable.Serialize(this.Polygon, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        Fix32.Serialize(this.SurfaceDepthMult, writer);
        writer.WriteGeneric<INoise2dFactory>(this.SurfaceHeightFn);
        writer.WriteGeneric<TerrainMaterialProto>(this.SurfaceMaterial);
      }

      public static PolygonSurfaceFeatureGenerator.Configuration Deserialize(BlobReader reader)
      {
        PolygonSurfaceFeatureGenerator.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonSurfaceFeatureGenerator.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonSurfaceFeatureGenerator.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.AllowNonIntegerSurfaceHeights = reader.ReadBool();
        this.BaseHeight = HeightTilesI.Deserialize(reader);
        this.BaseHeightFn = reader.ReadGenericAs<INoise2dFactory>();
        this.BaseMaterial = reader.ReadGenericAs<TerrainMaterialProto>();
        this.BaseThickness = ThicknessTilesF.Deserialize(reader);
        this.ContributesToOceanCoast = reader.ReadBool();
        this.DisableSubSurfaceGeneration = reader.ReadBool();
        this.MaxInfluenceDistance = RelTile1i.Deserialize(reader);
        this.MaxSurfaceThicknessFn = reader.ReadGenericAs<INoise2dFactory>();
        this.Polygon = Polygon2fMutable.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.SurfaceDepthMult = Fix32.Deserialize(reader);
        this.SurfaceHeightFn = reader.ReadGenericAs<INoise2dFactory>();
        this.SurfaceMaterial = reader.ReadGenericAs<TerrainMaterialProto>();
      }

      [EditorEnforceOrder(40)]
      public Polygon2fMutable Polygon { get; set; }

      [EditorLabel(null, "Distance beyond the polygon perimeter that this feature can modify terrain. Increase if the generate feature is getting cut off, decrease for better performance.", false, false)]
      [EditorEnforceOrder(44)]
      public RelTile1i MaxInfluenceDistance { get; set; }

      [EditorLabel(null, "Base height of the surface", false, false)]
      [EditorEnforceOrder(49)]
      [EditorRange(-2048.0, 2048.0)]
      public HeightTilesI BaseHeight { get; set; }

      [EditorEnforceOrder(53)]
      [EditorLabel(null, "Material at the cut-out portion (the surface)", false, false)]
      public TerrainMaterialProto SurfaceMaterial { get; set; }

      [EditorLabel(null, "Material at the non-cut portion", false, false)]
      [EditorEnforceOrder(57)]
      public TerrainMaterialProto BaseMaterial { get; set; }

      [EditorEnforceOrder(61)]
      [EditorLabel(null, "Thickness of the boundary layer", false, false)]
      public ThicknessTilesF BaseThickness { get; set; }

      [EditorLabel(null, "2D noise function that defines the overall shape before getting cut out to form a surface. It receives parameter 'polygon' of type 'Polygon2i'.", false, false)]
      [EditorCollapseObject]
      [EditorEnforceOrder(68)]
      public INoise2dFactory BaseHeightFn { get; set; }

      [EditorLabel(null, "2D noise function that defines the relative height of the surface.", false, false)]
      [EditorEnforceOrder(73)]
      [EditorCollapseObject]
      public INoise2dFactory SurfaceHeightFn { get; set; }

      [EditorEnforceOrder(77)]
      [EditorLabel(null, "Multiplier for the surface depth.", false, false)]
      public Fix32 SurfaceDepthMult { get; set; }

      [EditorLabel(null, "2D noise function that defines maximum depth of the surface.", false, false)]
      [EditorCollapseObject]
      [EditorEnforceOrder(82)]
      public INoise2dFactory MaxSurfaceThicknessFn { get; set; }

      [EditorEnforceOrder(87)]
      [EditorLabel(null, "If enabled, feature can have non-integer surface heights. Warning: Construction is hard/impossible on non-integer heights use with care.", false, false)]
      public bool AllowNonIntegerSurfaceHeights { get; set; }

      [EditorLabel(null, "Whether the boundary of this feature contributes to the coast. Ocean is shallower near coast.", false, false)]
      [EditorEnforceOrder(92)]
      public bool ContributesToOceanCoast { get; set; }

      [EditorLabel(null, "Disables generation below existing terrain.", false, false)]
      [EditorEnforceOrder(96)]
      public bool DisableSubSurfaceGeneration { get; set; }

      [EditorEnforceOrder(100)]
      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      public int SortingPriorityAdjustment { get; set; }

      [EditorReadonly]
      [EditorEnforceOrder(106)]
      [DoNotSave(0, null)]
      [EditorSection("Generator stats", null, true, false)]
      public int TotalGeneratedChunks { get; set; }

      [DoNotSave(0, null)]
      [EditorReadonly]
      [EditorEnforceOrder(111)]
      public int ChunksWithNoContribution { get; set; }

      [EditorReadonly]
      [DoNotSave(0, null)]
      [EditorEnforceOrder(116)]
      public int ChunksWithSurfaceAtMaxThickness { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CBaseHeight\u003Ek__BackingField = 3.TilesHigh();
        // ISSUE: reference to a compiler-generated field
        this.\u003CBaseThickness\u003Ek__BackingField = 10.0.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003CSurfaceDepthMult\u003Ek__BackingField = Fix32.One;
        // ISSUE: reference to a compiler-generated field
        this.\u003CContributesToOceanCoast\u003Ek__BackingField = true;
        // ISSUE: reference to a compiler-generated field
        this.\u003CDisableSubSurfaceGeneration\u003Ek__BackingField = true;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        PolygonSurfaceFeatureGenerator.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonSurfaceFeatureGenerator.Configuration) obj).SerializeData(writer));
        PolygonSurfaceFeatureGenerator.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonSurfaceFeatureGenerator.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
