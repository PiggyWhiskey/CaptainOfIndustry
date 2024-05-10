// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FeatureGenerators.OceanFloorFeatureGenerator
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Core.UiState;
using Mafi.Numerics;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.FeatureGenerators
{
  [GenerateSerializer(false, null, 0)]
  public class OceanFloorFeatureGenerator : 
    ITerrainFeatureGenerator,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly OceanFloorFeatureGenerator.Configuration ConfigMutable;
    [DoNotSave(0, null)]
    private Chunk2i m_precomputedOrigin;
    [DoNotSave(0, null)]
    private Vector2i m_precomputedSize;
    [DoNotSave(0, null)]
    private HeightTilesF[] m_oceanFloorHeightsCache;
    [DoNotSave(0, null)]
    private INoise2D m_heightBiasFn;
    [DoNotSave(0, null)]
    private TerrainManager m_terrainManager;

    public static void Serialize(OceanFloorFeatureGenerator value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<OceanFloorFeatureGenerator>(value))
        return;
      writer.EnqueueDataSerialization((object) value, OceanFloorFeatureGenerator.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      OceanFloorFeatureGenerator.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
    }

    public static OceanFloorFeatureGenerator Deserialize(BlobReader reader)
    {
      OceanFloorFeatureGenerator featureGenerator;
      if (reader.TryStartClassDeserialization<OceanFloorFeatureGenerator>(out featureGenerator))
        reader.EnqueueDataDeserialization((object) featureGenerator, OceanFloorFeatureGenerator.s_deserializeDataDelayedAction);
      return featureGenerator;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<OceanFloorFeatureGenerator>(this, "ConfigMutable", (object) OceanFloorFeatureGenerator.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
    }

    public string Name => "Ocean floor";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => true;

    public bool IsImportable => false;

    public bool Is2D => true;

    public bool CanRotate => false;

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public OceanFloorFeatureGenerator(
      OceanFloorFeatureGenerator.Configuration configMutable)
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
      if (this.m_oceanFloorHeightsCache != null)
        return true;
      string error;
      if (!this.ConfigMutable.HeightBiasFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) new Dict<string, object>(), out this.m_heightBiasFn, out error))
      {
        Log.Error("Failed to initialize ocean height bias function: " + error);
        this.m_heightBiasFn = (INoise2D) new ConstantNoise2D(Fix32.Zero);
      }
      this.m_precomputedOrigin = generatedArea.Origin - 1;
      this.m_precomputedSize = generatedArea.Size + 3;
      this.m_oceanFloorHeightsCache = new HeightTilesF[this.m_precomputedSize.ProductInt];
      this.m_terrainManager = resolver.Resolve<TerrainManager>();
      CoastLinesData data;
      ImmutableArray<Line2f> coastLines = extraDataReg.TryGetExtraData<CoastLinesData>(out data) ? data.CoastLines : ImmutableArray<Line2f>.Empty;
      Lyst<Tupple<Polygon2fFast, ThicknessTilesF, RelTile1f>> explicitRegions = new Lyst<Tupple<Polygon2fFast, ThicknessTilesF, RelTile1f>>();
      foreach (ExplicitOceanFloorRegion oceanFloorRegion in this.ConfigMutable.ExplicitOceanFloorRegions)
      {
        Polygon2fFast polygon;
        if (!oceanFloorRegion.Polygon.TryGetFastPolygon(out polygon, out error))
          Log.Error("Failed to construct fast polygon for ocean region, ignoring. " + error);
        else
          explicitRegions.Add(Tupple.Create<Polygon2fFast, ThicknessTilesF, RelTile1f>(polygon, oceanFloorRegion.OceanDepth, oceanFloorRegion.TransitionSize));
      }
      int addedY = 0;
      int num = 0;
      while (addedY < this.m_precomputedSize.Y)
      {
        for (int addedX = 0; addedX < this.m_precomputedSize.X; ++addedX)
        {
          Chunk2i chunk2i = this.m_precomputedOrigin.AddX(addedX);
          chunk2i = chunk2i.AddY(addedY);
          Vector2f vector2f = chunk2i.Tile2i.Vector2f;
          HeightTilesF height;
          Fix32 maxInfluence;
          if (explicitRegions.IsNotEmpty && tryGetExplicitHeight(vector2f, out height, out maxInfluence))
          {
            if (maxInfluence < Fix32.One)
              height = computeCoastlineHeight(vector2f).Lerp(height, maxInfluence);
          }
          else
            height = computeCoastlineHeight(vector2f);
          this.m_oceanFloorHeightsCache[num + addedX] = height;
        }
        ++addedY;
        num += this.m_precomputedSize.X;
      }
      return true;

      HeightTilesF computeCoastlineHeight(Vector2f position)
      {
        if (coastLines.IsEmpty)
          return -new HeightTilesF(this.ConfigMutable.MaxOceanDepth.Value);
        Fix64 fix64 = Fix64.MaxValue;
        foreach (Line2f coastLine in coastLines)
        {
          Fix64 lineSegment = coastLine.DistanceSqrToLineSegment(position);
          if (lineSegment < fix64)
            fix64 = lineSegment;
        }
        return -((this.ConfigMutable.DistanceBias + fix64.SqrtToFix32()) * this.ConfigMutable.OceanDepthIncreaseRatePer64Tiles.Value).DivBy64Fast.Clamp(this.ConfigMutable.MinOceanDepth.Value, this.ConfigMutable.MaxOceanDepth.Value).TilesHigh();
      }

      bool tryGetExplicitHeight(Vector2f position, out HeightTilesF height, out Fix32 maxInfluence)
      {
        maxInfluence = Fix32.Zero;
        height = new HeightTilesF();
        foreach (Tupple<Polygon2fFast, ThicknessTilesF, RelTile1f> explicitRegion in explicitRegions)
        {
          if (explicitRegion.First.Contains(position))
          {
            Fix32 fix32 = explicitRegion.First.DistanceSqrTo(position).SqrtToFix32() / explicitRegion.Third.Value;
            if (fix32 > maxInfluence)
            {
              maxInfluence = fix32;
              height = -new HeightTilesF(explicitRegion.Second.Value);
            }
          }
        }
        return maxInfluence.IsPositive;
      }
    }

    public void Reset()
    {
      this.m_oceanFloorHeightsCache = (HeightTilesF[]) null;
      this.m_heightBiasFn = (INoise2D) null;
      this.m_terrainManager = (TerrainManager) null;
    }

    public void ClearCaches()
    {
    }

    public TerrainFeatureResourceInfo? GetResourceInfo() => new TerrainFeatureResourceInfo?();

    public void GenerateChunkThreadSafe(TerrainGeneratorChunkData data)
    {
      Vector2i vector2i = data.Chunk.Vector2i - this.m_precomputedOrigin.Vector2i;
      Assert.That<int>(vector2i.X).IsWithinExcl(1, this.m_precomputedSize.X - 2);
      Assert.That<int>(vector2i.Y).IsWithinExcl(1, this.m_precomputedSize.Y - 2);
      int x = this.m_precomputedSize.X;
      int index1 = vector2i.X + vector2i.Y * x;
      HeightTilesF[] floorHeightsCache = this.m_oceanFloorHeightsCache;
      TerrainMaterialSlimId slimId = this.m_terrainManager.Bedrock.SlimId;
      CubicCoeffs cubicCoeffs1 = MafiMath.PrecomputeCubicCoeffs(floorHeightsCache[index1 - x - 1].Value, floorHeightsCache[index1 - 1].Value, floorHeightsCache[index1 + x - 1].Value, floorHeightsCache[index1 + x + x - 1].Value);
      CubicCoeffs cubicCoeffs2 = MafiMath.PrecomputeCubicCoeffs(floorHeightsCache[index1 - x].Value, floorHeightsCache[index1].Value, floorHeightsCache[index1 + x].Value, floorHeightsCache[index1 + x + x].Value);
      CubicCoeffs cubicCoeffs3 = MafiMath.PrecomputeCubicCoeffs(floorHeightsCache[index1 - x + 1].Value, floorHeightsCache[index1 + 1].Value, floorHeightsCache[index1 + x + 1].Value, floorHeightsCache[index1 + x + x + 1].Value);
      CubicCoeffs cubicCoeffs4 = MafiMath.PrecomputeCubicCoeffs(floorHeightsCache[index1 - x + 2].Value, floorHeightsCache[index1 + 2].Value, floorHeightsCache[index1 + x + 2].Value, floorHeightsCache[index1 + x + x + 2].Value);
      int num1 = 0;
      int num2 = 0;
      while (num1 < 64)
      {
        Percent t = Percent.FromRatio(num1, 64);
        CubicCoeffs cubicCoeffs5 = MafiMath.PrecomputeCubicCoeffs(cubicCoeffs1.Interpolate(t), cubicCoeffs2.Interpolate(t), cubicCoeffs3.Interpolate(t), cubicCoeffs4.Interpolate(t));
        for (int index2 = 0; index2 < 64; ++index2)
        {
          int tileIndex = num2 + index2;
          HeightTilesF heightTilesF = cubicCoeffs5.Interpolate(Percent.FromRatio(index2, 64)).TilesHigh();
          Tile2i tile2i = data.Area.Origin + new RelTile2i(index2, num1);
          data.Heights[tileIndex] = heightTilesF + this.m_heightBiasFn.GetValue(tile2i.Vector2f).TilesThick();
          data.PushLayerOnBottom(tileIndex, slimId, this.ConfigMutable.InitialBedrockLayerThickness);
        }
        ++num1;
        num2 += 64;
      }
    }

    public void TranslateBy(RelTile3f delta) => this.ConfigMutable.TranslateRegionsBy(delta);

    public void RotateBy(AngleDegrees1f rotation)
    {
    }

    public void MarkPreviewDirty()
    {
    }

    public bool TryGetPreviews(out IEnumerable<ITerrainFeaturePreview> previews)
    {
      previews = (IEnumerable<ITerrainFeaturePreview>) null;
      return false;
    }

    static OceanFloorFeatureGenerator()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      OceanFloorFeatureGenerator.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((OceanFloorFeatureGenerator) obj).SerializeData(writer));
      OceanFloorFeatureGenerator.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((OceanFloorFeatureGenerator) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfigWithInit, ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      [EditorEnforceOrder(70)]
      [DoNotSave(0, null)]
      public Action AddOceanFloorRegion;
      [EditorEnforceOrder(74)]
      [DoNotSave(0, null)]
      public Action TranslateAllRegions;
      [EditorEnforceOrder(82)]
      [EditorCollection(true, true)]
      public readonly Lyst<ExplicitOceanFloorRegion> ExplicitOceanFloorRegions;

      public static void Serialize(
        OceanFloorFeatureGenerator.Configuration value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<OceanFloorFeatureGenerator.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, OceanFloorFeatureGenerator.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Fix32.Serialize(this.DistanceBias, writer);
        Lyst<ExplicitOceanFloorRegion>.Serialize(this.ExplicitOceanFloorRegions, writer);
        writer.WriteGeneric<INoise2dFactory>(this.HeightBiasFn);
        ThicknessTilesF.Serialize(this.InitialBedrockLayerThickness, writer);
        ThicknessTilesF.Serialize(this.MaxOceanDepth, writer);
        ThicknessTilesF.Serialize(this.MinOceanDepth, writer);
        ThicknessTilesF.Serialize(this.OceanDepthIncreaseRatePer64Tiles, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
      }

      public static OceanFloorFeatureGenerator.Configuration Deserialize(BlobReader reader)
      {
        OceanFloorFeatureGenerator.Configuration configuration;
        if (reader.TryStartClassDeserialization<OceanFloorFeatureGenerator.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, OceanFloorFeatureGenerator.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.DistanceBias = Fix32.Deserialize(reader);
        reader.SetField<OceanFloorFeatureGenerator.Configuration>(this, "ExplicitOceanFloorRegions", (object) Lyst<ExplicitOceanFloorRegion>.Deserialize(reader));
        this.HeightBiasFn = reader.ReadGenericAs<INoise2dFactory>();
        this.InitialBedrockLayerThickness = reader.LoadedSaveVersion >= 142 ? ThicknessTilesF.Deserialize(reader) : 10.0.TilesThick();
        this.MaxOceanDepth = ThicknessTilesF.Deserialize(reader);
        this.MinOceanDepth = ThicknessTilesF.Deserialize(reader);
        this.OceanDepthIncreaseRatePer64Tiles = ThicknessTilesF.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
      }

      [EditorEnforceOrder(40)]
      [EditorRange(2.0, 100.0)]
      public ThicknessTilesF MinOceanDepth { get; set; }

      [EditorRange(5.0, 1000.0)]
      [EditorEnforceOrder(44)]
      public ThicknessTilesF MaxOceanDepth { get; set; }

      [EditorRange(0.0, 100.0)]
      [EditorEnforceOrder(49)]
      [EditorLabel(null, "Ocean depth increase per 64 tiles distance from the coast", false, false)]
      public ThicknessTilesF OceanDepthIncreaseRatePer64Tiles { get; set; }

      [EditorEnforceOrder(53)]
      [EditorLabel(null, "Coast distance bias. Lower values will cause coastal regions to be shallower.", false, false)]
      public Fix32 DistanceBias { get; set; }

      [EditorEnforceOrder(57)]
      [NewInSaveVersion(142, null, "10.0.TilesThick()", null, null)]
      public ThicknessTilesF InitialBedrockLayerThickness { get; set; }

      [EditorCollapseObject]
      [EditorEnforceOrder(62)]
      [EditorLabel(null, "Height bias function, applied after clamping", false, false)]
      public INoise2dFactory HeightBiasFn { get; set; }

      [EditorEnforceOrder(66)]
      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      public int SortingPriorityAdjustment { get; set; }

      [DoNotSave(0, null)]
      [EditorEnforceOrder(78)]
      public RelTile3f RegionTranslationAmount { get; set; }

      public void InitializeInMapEditor(IResolver resolver)
      {
        this.AddOceanFloorRegion = (Action) (() =>
        {
          Vector2f vector2f = resolver.Resolve<UiCameraState>().PivotPosition.Vector2f;
          ExplicitOceanFloorRegion oceanFloorRegion = new ExplicitOceanFloorRegion()
          {
            Polygon = new Polygon2fMutable()
          };
          oceanFloorRegion.Polygon.Initialize((IEnumerable<Vector2f>) new Vector2f[4]
          {
            vector2f + new Vector2f((Fix32) -50, (Fix32) -50),
            vector2f + new Vector2f((Fix32) 50, (Fix32) -50),
            vector2f + new Vector2f((Fix32) 50, (Fix32) 50),
            vector2f + new Vector2f((Fix32) -50, (Fix32) 50)
          });
          this.ExplicitOceanFloorRegions.Add(oceanFloorRegion);
        });
        this.TranslateAllRegions = (Action) (() =>
        {
          this.TranslateRegionsBy(this.RegionTranslationAmount);
          this.RegionTranslationAmount = RelTile3f.Zero;
        });
      }

      public void TranslateRegionsBy(RelTile3f delta)
      {
        foreach (ExplicitOceanFloorRegion oceanFloorRegion in this.ExplicitOceanFloorRegions)
        {
          oceanFloorRegion.Polygon.TranslateBy(delta.Xy.Vector2f);
          oceanFloorRegion.OceanDepth += delta.Z.TilesThick();
        }
      }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMinOceanDepth\u003Ek__BackingField = new ThicknessTilesF(2);
        // ISSUE: reference to a compiler-generated field
        this.\u003CMaxOceanDepth\u003Ek__BackingField = 25.0.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003COceanDepthIncreaseRatePer64Tiles\u003Ek__BackingField = 8.0.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003CDistanceBias\u003Ek__BackingField = (Fix32) -10;
        // ISSUE: reference to a compiler-generated field
        this.\u003CInitialBedrockLayerThickness\u003Ek__BackingField = 10.0.TilesThick();
        this.ExplicitOceanFloorRegions = new Lyst<ExplicitOceanFloorRegion>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        OceanFloorFeatureGenerator.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((OceanFloorFeatureGenerator.Configuration) obj).SerializeData(writer));
        OceanFloorFeatureGenerator.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((OceanFloorFeatureGenerator.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
