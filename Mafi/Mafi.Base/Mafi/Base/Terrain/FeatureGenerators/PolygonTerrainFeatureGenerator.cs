// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FeatureGenerators.PolygonTerrainFeatureGenerator
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
  /// Generic polygon terrain feature. Good for generating mountains or other non-plateau features.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class PolygonTerrainFeatureGenerator : 
    ITerrainFeatureGenerator,
    ITerrainFeatureBase,
    ITerrainFeatureWithPreview,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MIN_THICKNESS;
    public readonly PolygonTerrainFeatureGenerator.Configuration ConfigMutable;
    private int m_totalGeneratedChunks;
    private readonly Set<Chunk2iSlim> m_chunksWithNoContributionCache;
    [DoNotSave(0, null)]
    private Polygon3fFast m_polygonCache;
    [DoNotSave(0, null)]
    private INoise2D m_baseHeightFn;
    [DoNotSave(0, null)]
    private INoise2D m_surfaceThicknessFn;
    [DoNotSave(0, null)]
    private Percent m_belowSurfaceHeightMult;
    [DoNotSave(0, null)]
    private ThicknessTilesF m_belowSurfaceMaxDepth;
    [DoNotSave(0, null)]
    private ThicknessTilesF m_shapeInversionDepthTimesTwo;
    [DoNotSave(0, null)]
    private bool m_previewsGenerated;
    [DoNotSave(0, null)]
    private bool m_isGeneratingPreviews;
    [DoNotSave(0, null)]
    private Option<PreviewHelper<HeightmapTopBottomPreviewChunkData>> m_previewHelper;
    [DoNotSave(0, null)]
    private readonly Option<string> m_iconAssetPath;

    public static void Serialize(PolygonTerrainFeatureGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<PolygonTerrainFeatureGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, PolygonTerrainFeatureGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      PolygonTerrainFeatureGenerator.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      Set<Chunk2iSlim>.Serialize(this.m_chunksWithNoContributionCache, writer);
      writer.WriteInt(this.m_totalGeneratedChunks);
    }

    public static PolygonTerrainFeatureGenerator Deserialize(BlobReader reader)
    {
      PolygonTerrainFeatureGenerator featureGenerator;
      if (reader.TryStartClassDeserialization<PolygonTerrainFeatureGenerator>(out featureGenerator))
        reader.EnqueueDataDeserialization((object) featureGenerator, PolygonTerrainFeatureGenerator.s_deserializeDataDelayedAction);
      return featureGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<PolygonTerrainFeatureGenerator>(this, "ConfigMutable", (object) PolygonTerrainFeatureGenerator.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      reader.SetField<PolygonTerrainFeatureGenerator>(this, "m_chunksWithNoContributionCache", (object) Set<Chunk2iSlim>.Deserialize(reader));
      this.m_totalGeneratedChunks = reader.ReadInt();
    }

    public string Name
    {
      get
      {
        return string.Format("Polygon feature: {0} at {1}", (object) this.ConfigMutable.TerrainMaterial.Strings.Name, (object) this.GetPosition().Tile2i);
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
        return this.ConfigMutable.SortingPriorityAdjustment + ((this.ConfigMutable.OnlyPlaceOnTopAboveGround || this.ConfigMutable.OnlyReplaceExistingMaterials || this.ConfigMutable.TerrainBlendHeightRange.IsPositive ? 9000 : 1000) - this.ConfigMutable.Polygon.Vertices.Max<Fix32>((Func<Vector3f, Fix32>) (x => x.Z)).ToIntFloored() + (this.ConfigMutable.IgnoreAsResource ? -100 : 0));
      }
    }

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public PolygonTerrainFeatureGenerator(
      PolygonTerrainFeatureGenerator.Configuration initialConfig,
      Option<string> iconAssetPath = default (Option<string>))
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_chunksWithNoContributionCache = new Set<Chunk2iSlim>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ConfigMutable = initialConfig;
      this.m_iconAssetPath = iconAssetPath;
    }

    public Tile2f GetPosition()
    {
      Tile2f zero = Tile2f.Zero;
      foreach (Vector3f vertex in this.ConfigMutable.Polygon.Vertices)
        zero += new RelTile2f(vertex.Xy);
      return zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length;
    }

    public Tile3f GetPosition3f()
    {
      Tile3f zero = Tile3f.Zero;
      foreach (Vector3f vertex in this.ConfigMutable.Polygon.Vertices)
        zero += new RelTile3f(vertex);
      return zero / (Fix32) this.ConfigMutable.Polygon.Vertices.Length;
    }

    public HandleData? GetHandleData()
    {
      Tile3f position3f = this.GetPosition3f();
      return new HandleData?(new HandleData(position3f.Xy, (ColorRgba) (this.ConfigMutable.IgnoreAsResource ? 8940732 : 7113148), (Option<string>) this.m_iconAssetPath.ValueOr(this.ConfigMutable.TerrainMaterial.MinedProduct.Graphics.IconPath), ColorRgba.White, new HeightTilesF?(position3f.Z.TilesHigh())));
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
      this.m_belowSurfaceHeightMult = Percent.Hundred + this.ConfigMutable.BelowSurfaceExtraHeight;
      this.m_belowSurfaceMaxDepth = this.ConfigMutable.BelowSurfaceMaxDepth;
      this.m_shapeInversionDepthTimesTwo = 2 * this.ConfigMutable.ShapeInversionDepth;
      Dict<string, object> extraArgs = new Dict<string, object>()
      {
        {
          "Polygon".LowerFirstChar(),
          (object) this.ConfigMutable.Polygon.GetPolygon().To2f()
        }
      };
      if (!this.ConfigMutable.HeightFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_baseHeightFn, out error))
      {
        error = "Failed to initialize base height function: " + error;
        return false;
      }
      if (!this.ConfigMutable.SurfaceCoverThicknessFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_surfaceThicknessFn, out error))
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
      this.m_polygonCache = (Polygon3fFast) null;
      this.m_baseHeightFn = (INoise2D) null;
      this.m_surfaceThicknessFn = (INoise2D) null;
      this.ConfigMutable.TotalGeneratedChunks = this.m_totalGeneratedChunks;
      this.ConfigMutable.ChunksWithNoContribution = this.m_chunksWithNoContributionCache.Count;
      this.m_previewsGenerated = false;
    }

    public void ClearCaches() => this.m_chunksWithNoContributionCache.Clear();

    public void TranslateBy(RelTile3f delta)
    {
      this.Reset();
      this.ConfigMutable.Polygon.TranslateBy(delta.Xy.ExtendZ(delta.Z).Vector3f);
    }

    public void RotateBy(AngleDegrees1f rotation)
    {
      this.Reset();
      this.ConfigMutable.Polygon.RotateBy(rotation.Degrees);
    }

    private HeightTilesF getBaseHeightAt(Vector2f coord, bool contained)
    {
      Fix64 zero1 = Fix64.Zero;
      Fix64 rhs1 = Fix64.Zero;
      Fix64 zero2 = Fix64.Zero;
      Fix64 zero3 = Fix64.Zero;
      for (int index = 0; index < this.m_polygonCache.VerticesCount; ++index)
      {
        Vector3f vector3f1 = this.m_polygonCache.VerticesExt[index];
        Vector3f vector3f2 = this.m_polygonCache.VerticesExt[index + 1];
        Percent closestTtoLineSegment = new Line2f(vector3f1.Xy, vector3f2.Xy).GetClosestTToLineSegment(coord);
        Vector2f other = vector3f1.Xy.Lerp(vector3f2.Xy, closestTtoLineSegment);
        Fix64 one = coord.DistanceSqrTo(other);
        Fix32 fix32_1 = one.SqrtToFix32();
        Fix32 fix32_2 = vector3f1.Z.Lerp(vector3f2.Z, closestTtoLineSegment);
        if (fix32_1 < Fix32.EpsilonNear)
          return new HeightTilesF(fix32_2);
        Fix64 fix64_1 = fix32_2.ToFix64();
        one = Fix64.One;
        Fix64 rhs2 = one.DivByPositiveUncheckedUnrounded(fix32_1);
        if (!contained)
        {
          Vector3f vector3f3 = vector3f2 - vector3f1;
          Vector2f vector2f = coord - other;
          Fix64 fix64_2 = vector3f3.Xy.Dot(vector2f.Normalized);
          Fix64 fix64_3 = vector3f3.Z.ToFix64();
          Fix64 lengthSqr = vector3f3.Xy.LengthSqr;
          if (lengthSqr > Fix64.EpsilonNear)
          {
            zero2 += fix64_2 * fix64_3.DivByPositiveUncheckedUnrounded(lengthSqr);
            zero3 += rhs2;
          }
          Fix64 fix64_4 = rhs2.MultByUnchecked(rhs2);
          zero1 += fix64_1 * fix64_4;
          rhs1 += fix64_4;
        }
        else
        {
          zero1 += fix64_1 * rhs2;
          rhs1 += rhs2;
        }
      }
      if (rhs1 <= Fix64.Zero)
      {
        Log.Error("sumOneOverDist is zero");
        rhs1 = Fix64.One;
      }
      return !contained ? new HeightTilesF((zero1.DivByPositiveUncheckedUnrounded(rhs1) + (zero3 < Fix64.EpsilonNear ? Fix64.Zero : zero2.DivByPositiveUncheckedUnrounded(zero3))).ToFix32()) : new HeightTilesF(zero1.DivByPositiveUncheckedUnrounded(rhs1).ToFix32());
    }

    public TerrainFeatureResourceInfo? GetResourceInfo()
    {
      return !this.ConfigMutable.IgnoreAsResource ? new TerrainFeatureResourceInfo?(new TerrainFeatureResourceInfo(this.GetPosition().Tile2i, (ProductProto) this.ConfigMutable.TerrainMaterial.MinedProduct)) : new TerrainFeatureResourceInfo?();
    }

    public void GenerateChunkThreadSafe(TerrainGeneratorChunkData data)
    {
      if (this.m_polygonCache == null)
        Log.Error("Not initialized.");
      else if ((Proto) this.ConfigMutable.TerrainMaterial == (Proto) null)
      {
        Log.Error("Polygon terrain feature '" + this.Name + "' has null TerrainMaterial.");
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
        TerrainMaterialSlimId? nullable1 = this.ConfigMutable.SurfaceCoverMaterial.ValueOrNull?.SlimId;
        TerrainMaterialSlimId terrainMaterialSlimId = slimId;
        TerrainMaterialSlimId? nullable2 = nullable1;
        if ((nullable2.HasValue ? (terrainMaterialSlimId == nullable2.GetValueOrDefault() ? 1 : 0) : 0) != 0)
          nullable1 = new TerrainMaterialSlimId?();
        bool flag = true;
        int num1 = this.ConfigMutable.MaxInfluenceDistance.Value.Squared();
        HeightTilesF[] heights = data.Heights;
        Assert.That<HeightTilesF[]>(heights).IsNotNull<HeightTilesF[]>("Heights are null!");
        int y = 0;
        int num2 = 0;
        while (y < 64)
        {
          for (int x = 0; x < 64; ++x)
          {
            int tileIndex = num2 + x;
            Vector2f vector2f = (data.Area.Origin + new RelTile2i(x, y)).Vector2f;
            bool contained = this.m_polygonCache.Contains2D(vector2f);
            if (contained || !(this.m_polygonCache.Distance2DSqrTo(vector2f) > num1))
            {
              Fix64 fix64 = this.m_baseHeightFn.GetValue(vector2f);
              ThicknessTilesF thicknessTilesF1 = fix64.ToFix32().TilesThick();
              ThicknessTilesF thicknessTilesF2 = thicknessTilesF1.ScaledBy(this.m_belowSurfaceHeightMult) + this.m_shapeInversionDepthTimesTwo;
              ThicknessTilesF thicknessTilesF3 = -thicknessTilesF2.Min(this.m_belowSurfaceMaxDepth);
              if (!(thicknessTilesF1 - thicknessTilesF3 < ThicknessTilesF.One))
              {
                HeightTilesF baseHeightAt = this.getBaseHeightAt(vector2f, contained);
                HeightTilesF heightTilesF1 = baseHeightAt + thicknessTilesF1;
                HeightTilesF bottomHeight = baseHeightAt + thicknessTilesF3;
                HeightTilesF heightTilesF2 = heights[tileIndex];
                if (this.ConfigMutable.OnlyReplaceExistingMaterials)
                {
                  if (!(bottomHeight >= heightTilesF2))
                  {
                    if (heightTilesF1 > heightTilesF2)
                      heightTilesF1 = heightTilesF2;
                  }
                  else
                    continue;
                }
                else
                {
                  if (this.ConfigMutable.TerrainBlendHeightRange.IsPositive)
                    heightTilesF1 = (heightTilesF1 - heightTilesF2).Value.SmoothApproachZero(this.ConfigMutable.TerrainBlendHeightRange.Value).TilesThick() + heightTilesF2;
                  if (this.ConfigMutable.OnlyPlaceOnTopAboveGround)
                  {
                    if (!(heightTilesF1 - heightTilesF2).IsNotPositive)
                    {
                      if (bottomHeight < heightTilesF2)
                        bottomHeight = heightTilesF2;
                    }
                    else
                      continue;
                  }
                  if (this.ConfigMutable.UndergroundDepthMult != Percent.Hundred)
                  {
                    ThicknessTilesF thicknessTilesF4 = heightTilesF1 - heightTilesF2;
                    if (thicknessTilesF4.IsNotPositive)
                    {
                      thicknessTilesF4 = thicknessTilesF4.ScaledBy(this.ConfigMutable.UndergroundDepthMult);
                      heightTilesF1 = thicknessTilesF4 + heightTilesF2;
                    }
                  }
                }
                ThicknessTilesF rhs = heightTilesF1 - bottomHeight;
                if (!(rhs < PolygonTerrainFeatureGenerator.MIN_THICKNESS))
                {
                  if (nullable1.HasValue)
                  {
                    fix64 = this.m_surfaceThicknessFn.GetValue(vector2f);
                    thicknessTilesF2 = fix64.ToFix32().TilesThick();
                    ThicknessTilesF thickness = thicknessTilesF2.Min(rhs);
                    if (thickness >= PolygonTerrainFeatureGenerator.MIN_THICKNESS)
                    {
                      flag = false;
                      data.SetMaterialInRange(tileIndex, nullable1.Value, heightTilesF1 - thickness, thickness);
                      if (!(rhs - thickness < PolygonTerrainFeatureGenerator.MIN_THICKNESS))
                        heightTilesF1 -= thickness;
                      else
                        continue;
                    }
                  }
                  flag = false;
                  data.SetMaterialInRange(tileIndex, slimId, bottomHeight, heightTilesF1 - bottomHeight);
                }
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
      PreviewHelper<HeightmapTopBottomPreviewChunkData> previewHelper = this.m_previewHelper.ValueOrNull;
      if (previewHelper == null)
        this.m_previewHelper = (Option<PreviewHelper<HeightmapTopBottomPreviewChunkData>>) (previewHelper = new PreviewHelper<HeightmapTopBottomPreviewChunkData>());
      if (!this.m_previewsGenerated || this.m_isGeneratingPreviews)
      {
        if (!this.tryInitialize(resolver, out string _))
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
      PreviewHelper<HeightmapTopBottomPreviewChunkData> previewHelper,
      int timeBudgetMs)
    {
      Assert.That<Polygon3fFast>(this.m_polygonCache).IsNotNull<Polygon3fFast>();
      bool isComplete;
      // ISSUE: method pointer
      previewHelper.GeneratePreviewsInParallel(this.GetBoundingBox(), !this.m_previewsGenerated, new Action<HeightmapTopBottomPreviewChunkData>((object) this, __methodptr(\u003CgeneratePreviews\u003Eg__processChunk\u007C65_0)), timeBudgetMs, out isComplete);
      this.m_previewsGenerated = true;
      this.m_isGeneratingPreviews = !isComplete;
    }

    static PolygonTerrainFeatureGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      PolygonTerrainFeatureGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonTerrainFeatureGenerator) obj).SerializeData(writer));
      PolygonTerrainFeatureGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonTerrainFeatureGenerator) obj).DeserializeData(reader));
      PolygonTerrainFeatureGenerator.MIN_THICKNESS = (1.0 / 16.0).TilesThick();
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(
        PolygonTerrainFeatureGenerator.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<PolygonTerrainFeatureGenerator.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, PolygonTerrainFeatureGenerator.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Percent.Serialize(this.BelowSurfaceExtraHeight, writer);
        ThicknessTilesF.Serialize(this.BelowSurfaceMaxDepth, writer);
        writer.WriteGeneric<INoise2dFactory>(this.HeightFn);
        writer.WriteBool(this.IgnoreAsResource);
        RelTile1i.Serialize(this.MaxInfluenceDistance, writer);
        writer.WriteBool(this.OnlyPlaceOnTopAboveGround);
        writer.WriteBool(this.OnlyReplaceExistingMaterials);
        Polygon3fMutable.Serialize(this.Polygon, writer);
        ThicknessTilesF.Serialize(this.ShapeInversionDepth, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        Option<TerrainMaterialProto>.Serialize(this.SurfaceCoverMaterial, writer);
        writer.WriteGeneric<INoise2dFactory>(this.SurfaceCoverThicknessFn);
        RelTile1f.Serialize(this.TerrainBlendHeightRange, writer);
        writer.WriteGeneric<TerrainMaterialProto>(this.TerrainMaterial);
        Percent.Serialize(this.UndergroundDepthMult, writer);
      }

      public static PolygonTerrainFeatureGenerator.Configuration Deserialize(BlobReader reader)
      {
        PolygonTerrainFeatureGenerator.Configuration configuration;
        if (reader.TryStartClassDeserialization<PolygonTerrainFeatureGenerator.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, PolygonTerrainFeatureGenerator.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.BelowSurfaceExtraHeight = Percent.Deserialize(reader);
        this.BelowSurfaceMaxDepth = ThicknessTilesF.Deserialize(reader);
        this.HeightFn = reader.ReadGenericAs<INoise2dFactory>();
        this.IgnoreAsResource = reader.ReadBool();
        this.MaxInfluenceDistance = RelTile1i.Deserialize(reader);
        this.OnlyPlaceOnTopAboveGround = reader.ReadBool();
        this.OnlyReplaceExistingMaterials = reader.ReadBool();
        this.Polygon = Polygon3fMutable.Deserialize(reader);
        this.ShapeInversionDepth = ThicknessTilesF.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.SurfaceCoverMaterial = Option<TerrainMaterialProto>.Deserialize(reader);
        this.SurfaceCoverThicknessFn = reader.ReadGenericAs<INoise2dFactory>();
        this.TerrainBlendHeightRange = RelTile1f.Deserialize(reader);
        this.TerrainMaterial = reader.ReadGenericAs<TerrainMaterialProto>();
        this.UndergroundDepthMult = Percent.Deserialize(reader);
      }

      [EditorEnforceOrder(47)]
      public Polygon3fMutable Polygon { get; set; }

      [EditorEnforceOrder(50)]
      public TerrainMaterialProto TerrainMaterial { get; set; }

      [EditorLabel(null, "Distance beyond the polygon perimeter that this feature can modify terrain. Increase if the generate feature is getting cut off, decrease for better performance.", false, false)]
      [EditorEnforceOrder(54)]
      public RelTile1i MaxInfluenceDistance { get; set; }

      [EditorLabel(null, "Maximum depth generated below the base height.", false, false)]
      [EditorEnforceOrder(58)]
      public ThicknessTilesF BelowSurfaceMaxDepth { get; set; }

      [EditorLabel(null, "Depth at which the shape inverses. This can be used to make a resource shrink its size as it goes deeper.", false, false)]
      [EditorEnforceOrder(63)]
      public ThicknessTilesF ShapeInversionDepth { get; set; }

      [EditorLabel(null, "Adds extra material thickness to areas below the base height. This can be used to make mountains 'thicker' on the bottom and underground", false, false)]
      [EditorEnforceOrder(68)]
      public Percent BelowSurfaceExtraHeight { get; set; }

      [EditorEnforceOrder(73)]
      [EditorLabel(null, "Range of smooth blending region with terrain. Set to zero to disable it. Note that this setting won't show on the preview.", false, false)]
      public RelTile1f TerrainBlendHeightRange { get; set; }

      [EditorEnforceOrder(78)]
      [EditorLabel(null, "Multiples the depth of underground sections. Values less than 100% can be used to better blend the resource to the terrain.", false, false)]
      public Percent UndergroundDepthMult { get; set; }

      [EditorEnforceOrder(83)]
      [EditorLabel(null, "Whether to ignore this resource from list of all map resources. Resources and their locations are visible in the map selection.", false, false)]
      public bool IgnoreAsResource { get; set; }

      [EditorEnforceOrder(88)]
      [EditorLabel(null, "When set, only parts that are above the ground will be generated. Note that generation order of features matters and can be adjusted using 'Sorting priority adjustment'.", false, false)]
      public bool OnlyPlaceOnTopAboveGround { get; set; }

      [EditorEnforceOrder(93)]
      [EditorLabel(null, "Only parts that are overlapping with existing terrain will be generated. Note that generation order of features matters and can be adjusted using 'Sorting priority adjustment'.", false, false)]
      public bool OnlyReplaceExistingMaterials { get; set; }

      [EditorCollapseObject]
      [EditorEnforceOrder(99)]
      [EditorLabel(null, "2D noise function that defines the height at each point. It receives parameter 'polygon' of type 'Polygon2i'.", false, false)]
      public INoise2dFactory HeightFn { get; set; }

      [EditorLabel(null, "Optional surface cover material.", false, false)]
      [EditorEnforceOrder(103)]
      public Option<TerrainMaterialProto> SurfaceCoverMaterial { get; set; }

      [EditorLabel(null, "2D noise function that defines thickness for surface cover material", false, false)]
      [EditorEnforceOrder(108)]
      [EditorCollapseObject]
      public INoise2dFactory SurfaceCoverThicknessFn { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(113)]
      public int SortingPriorityAdjustment { get; set; }

      [EditorEnforceOrder(120)]
      [EditorReadonly]
      [DoNotSave(0, null)]
      [EditorSection("Generator stats", null, true, false)]
      public int TotalGeneratedChunks { get; set; }

      [EditorEnforceOrder(125)]
      [DoNotSave(0, null)]
      [EditorReadonly]
      public int ChunksWithNoContribution { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CUndergroundDepthMult\u003Ek__BackingField = Percent.Hundred;
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        PolygonTerrainFeatureGenerator.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((PolygonTerrainFeatureGenerator.Configuration) obj).SerializeData(writer));
        PolygonTerrainFeatureGenerator.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((PolygonTerrainFeatureGenerator.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
