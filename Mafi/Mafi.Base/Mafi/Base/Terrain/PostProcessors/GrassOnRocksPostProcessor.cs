// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.GrassOnRocksPostProcessor
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
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>
  /// Grows grass on rocks as well as spreads incompatible materials such as sand.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class GrassOnRocksPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public const int MAX_CHECK_DISTANCE = 8;
    public readonly GrassOnRocksPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private Fix32[] m_grassGrowthMult;
    [DoNotSave(0, null)]
    private bool[] m_isMaterialFarmable;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>[]> m_newLayers;
    [DoNotSave(0, null)]
    private TerrainManager m_terrain;
    [DoNotSave(0, null)]
    private Lyst<Lyst<Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>>> m_newLayersPool;

    public static void Serialize(GrassOnRocksPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GrassOnRocksPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GrassOnRocksPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      GrassOnRocksPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static GrassOnRocksPostProcessor Deserialize(BlobReader reader)
    {
      GrassOnRocksPostProcessor rocksPostProcessor;
      if (reader.TryStartClassDeserialization<GrassOnRocksPostProcessor>(out rocksPostProcessor))
        reader.EnqueueDataDeserialization((object) rocksPostProcessor, GrassOnRocksPostProcessor.s_deserializeDataDelayedAction);
      return rocksPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<GrassOnRocksPostProcessor>(this, "ConfigMutable", (object) GrassOnRocksPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Grass on rocks";

    public int Id { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsUnique => true;

    public bool IsImportable => false;

    public bool Is2D => true;

    public bool CanRotate => false;

    public TerrainPostProcessorParallelizationStrategy ParallelizationStrategy
    {
      get => TerrainPostProcessorParallelizationStrategy.AnalyzeInterleaveAndApply;
    }

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 1000;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public GrassOnRocksPostProcessor(
      GrassOnRocksPostProcessor.Configuration configMutable)
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
      this.m_terrain = resolver.Resolve<TerrainManager>();
      ImmutableArray<TerrainMaterialProto> terrainMaterials = this.m_terrain.TerrainMaterials;
      this.m_grassGrowthMult = terrainMaterials.MapArray<Fix32>((Func<TerrainMaterialProto, Fix32>) (x => x.GrassGrowthOnTop.ToFix32()));
      terrainMaterials = this.m_terrain.TerrainMaterials;
      this.m_isMaterialFarmable = terrainMaterials.MapArray<bool>((Func<TerrainMaterialProto, bool>) (x => x.IsFarmable));
      this.m_newLayers = new Dict<Chunk2i, Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>[]>();
      this.m_newLayersPool = new Lyst<Lyst<Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>>>();
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      return true;
    }

    public void Reset()
    {
      this.m_grassGrowthMult = (Fix32[]) null;
      this.m_isMaterialFarmable = (bool[]) null;
      this.m_newLayers = (Dict<Chunk2i, Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>[]>) null;
      this.m_terrain = (TerrainManager) null;
      this.m_newLayersPool = (Lyst<Lyst<Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>>>) null;
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
      int tileIndex1 = dataReadOnly.GetTileIndex(chunk.Tile2i - dataOrigin);
      TerrainMaterialSlimId slimId = this.ConfigMutable.GrassMaterialProto.SlimId;
      int width = dataReadOnly.Width;
      ushort[] flags = dataReadOnly.Flags;
      HeightTilesF[] heights = dataReadOnly.Heights;
      int num1 = this.ConfigMutable.MaxIncompatibleMatCheckDistance.Clamp(0, 8);
      Fix32 rhs1 = this.ConfigMutable.ThresholdDeltaHeight.Value.Clamp((Fix32) 1, (Fix32) 5);
      Fix32 rhs2 = this.ConfigMutable.MaxCoverPercentage.ToFix32().Clamp(Fix32.Half, Fix32.Two);
      Lyst<Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>> newLayers = (Lyst<Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>>) null;
      lock (this.m_newLayersPool)
      {
        if (this.m_newLayersPool.IsNotEmpty)
          newLayers = this.m_newLayersPool.PopLast();
      }
      if (newLayers == null)
        newLayers = new Lyst<Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>>(true);
      int num2 = 0;
      while (num2 < 64)
      {
        for (int index = 0; index < 64; ++index)
        {
          int tileIndex = tileIndex1 + index;
          if (((int) flags[tileIndex] & 5) == 0)
          {
            TileMaterialLayers layers = dataReadOnly.MaterialLayers[tileIndex];
            if (layers.Count > 0)
            {
              ThicknessTilesF eligibleThickness = getEligibleThickness();
              if (!(eligibleThickness < this.ConfigMutable.MinAddedThickness))
              {
                int numerator = num1 + 1;
                TerrainMaterialSlimIdOption none = TerrainMaterialSlimIdOption.None;
                int deltaI1 = 1;
                int deltaI2 = width;
                while (deltaI1 <= num1)
                {
                  if (((int) flags[tileIndex - deltaI1] & 5) != 0 || ((int) flags[tileIndex + deltaI1] & 5) != 0 || ((int) flags[tileIndex - deltaI2] & 5) != 0 || ((int) flags[tileIndex + deltaI2] & 5) != 0)
                  {
                    numerator = deltaI1;
                    break;
                  }
                  if (isIncompatibleMaterial(-deltaI1, ref none) || isIncompatibleMaterial(deltaI1, ref none) || isIncompatibleMaterial(-deltaI2, ref none) || isIncompatibleMaterial(deltaI2, ref none))
                  {
                    numerator = deltaI1;
                    break;
                  }
                  ++deltaI1;
                  deltaI2 += width;
                }
                ThicknessTilesF thicknessTilesF1 = eligibleThickness.Min(ThicknessTilesF.One);
                ThicknessTilesF thicknessTilesF2 = thicknessTilesF1;
                if (numerator <= num1)
                  thicknessTilesF1 *= Fix32.FromFraction((long) numerator, (long) (num1 + 1));
                HeightTilesF heightTilesF = heights[tileIndex];
                ThicknessTilesF thicknessTilesF3 = heightTilesF - heights[tileIndex - width - 1];
                ThicknessTilesF abs1 = thicknessTilesF3.Abs;
                thicknessTilesF3 = heightTilesF - heights[tileIndex - width];
                ThicknessTilesF abs2 = thicknessTilesF3.Abs;
                ThicknessTilesF thicknessTilesF4 = abs1 + abs2;
                thicknessTilesF3 = heightTilesF - heights[tileIndex - width + 1];
                ThicknessTilesF abs3 = thicknessTilesF3.Abs;
                ThicknessTilesF thicknessTilesF5 = thicknessTilesF4 + abs3;
                thicknessTilesF3 = heightTilesF - heights[tileIndex - 1];
                ThicknessTilesF abs4 = thicknessTilesF3.Abs;
                ThicknessTilesF thicknessTilesF6 = thicknessTilesF5 + abs4;
                thicknessTilesF3 = heightTilesF - heights[tileIndex + 1];
                ThicknessTilesF abs5 = thicknessTilesF3.Abs;
                ThicknessTilesF thicknessTilesF7 = thicknessTilesF6 + abs5;
                thicknessTilesF3 = heightTilesF - heights[tileIndex + width - 1];
                ThicknessTilesF abs6 = thicknessTilesF3.Abs;
                ThicknessTilesF thicknessTilesF8 = thicknessTilesF7 + abs6;
                thicknessTilesF3 = heightTilesF - heights[tileIndex + width];
                ThicknessTilesF abs7 = thicknessTilesF3.Abs;
                ThicknessTilesF thicknessTilesF9 = thicknessTilesF8 + abs7;
                thicknessTilesF3 = heightTilesF - heights[tileIndex + width + 1];
                ThicknessTilesF abs8 = thicknessTilesF3.Abs;
                ThicknessTilesF thicknessTilesF10 = thicknessTilesF9 + abs8;
                Fix32 fix32_1 = rhs1 - thicknessTilesF10.Value.DivBy8Fast;
                Fix32 rhs3 = fix32_1.DivByPositiveUncheckedUnrounded(rhs1);
                if (!(rhs3 < Fix32.EpsilonNear))
                {
                  ThicknessTilesF thicknessTilesF11;
                  ref ThicknessTilesF local = ref thicknessTilesF11;
                  fix32_1 = rhs3.MultByUnchecked(rhs3);
                  Fix32 fix32_2 = fix32_1.MultByUnchecked(rhs2);
                  local = new ThicknessTilesF(fix32_2);
                  addLayerIfThickEnough(thicknessTilesF11 * thicknessTilesF1.Value, slimId, this.m_isMaterialFarmable[layers.First.SlimIdRaw]);
                  if (none.HasValue)
                  {
                    ThicknessTilesF thicknessTilesF12 = thicknessTilesF2 * (Fix32.One - Fix32.FromFraction((long) numerator, (long) (num1 + 1)));
                    addLayerIfThickEnough(thicknessTilesF11 * thicknessTilesF12.Value, none.Value, (TerrainMaterialSlimIdOption) layers.First.SlimId == none);
                  }
                }
              }
            }

            ThicknessTilesF getEligibleThickness()
            {
              ThicknessTilesF maxExaminedDepth = this.ConfigMutable.MaxExaminedDepth;
              Fix32 fix32_1 = this.m_grassGrowthMult[layers.First.SlimIdRaw];
              ThicknessTilesF zero = ThicknessTilesF.Zero;
              if (fix32_1.IsPositive)
                zero += fix32_1 * layers.First.Thickness;
              if (layers.First.Thickness >= maxExaminedDepth)
                return zero;
              Fix32 fix32_2 = this.m_grassGrowthMult[layers.Second.SlimIdRaw];
              if (fix32_2.IsPositive)
              {
                ThicknessTilesF thicknessTilesF = layers.Second.Thickness.Min(maxExaminedDepth - layers.First.Thickness);
                zero += fix32_2 * thicknessTilesF;
              }
              ThicknessTilesF thicknessTilesF1 = layers.First.Thickness + layers.Second.Thickness;
              if (thicknessTilesF1 >= maxExaminedDepth)
                return zero;
              Fix32 fix32_3 = this.m_grassGrowthMult[layers.Third.SlimIdRaw];
              if (fix32_3.IsPositive)
              {
                ThicknessTilesF thicknessTilesF2 = layers.Third.Thickness.Min(maxExaminedDepth - thicknessTilesF1);
                zero += fix32_3 * thicknessTilesF2;
              }
              return zero;
            }
          }

          bool isIncompatibleMaterial(
            int deltaI,
            ref TerrainMaterialSlimIdOption incompatibleSlimId)
          {
            int index = tileIndex + deltaI;
            TerrainMaterialThicknessSlim first = dataReadOnly.MaterialLayers[index].First;
            if (this.m_grassGrowthMult[first.SlimIdRaw].IsNotPositive)
            {
              TerrainMaterialProto full = first.SlimId.ToFull(this.m_terrain);
              if (incompatibleSlimId.IsNone && full.CanSpreadToNearbyMaterials)
                incompatibleSlimId = (TerrainMaterialSlimIdOption) first.SlimId;
              return !full.IsFarmable;
            }
            if (first.Thickness < ThicknessTilesF.One)
            {
              TerrainMaterialThicknessSlim second = dataReadOnly.MaterialLayers[index].Second;
              if (this.m_grassGrowthMult[second.SlimIdRaw].IsNotPositive)
                return !this.m_isMaterialFarmable[(int) second.SlimId.Value];
            }
            return false;
          }

          void addLayerIfThickEnough(
            ThicknessTilesF fillAmount,
            TerrainMaterialSlimId depositedMaterialSlimId,
            bool extendExistingLayer)
          {
            if (fillAmount < this.ConfigMutable.MinAddedThickness)
              return;
            if (newLayers == null)
              newLayers = new Lyst<Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>>(64);
            newLayers.Add(Tupple.Create<Tile2iIndex, bool, TerrainMaterialThicknessSlim>(new Tile2iIndex(tileIndex), extendExistingLayer, new TerrainMaterialThicknessSlim(depositedMaterialSlimId, fillAmount)));
          }
        }
        ++num2;
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

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim>[] tuppleArray;
      if (!this.m_newLayers.TryRemove(chunk, out tuppleArray))
        return;
      foreach (Tupple<Tile2iIndex, bool, TerrainMaterialThicknessSlim> tupple in tuppleArray)
      {
        ref TileMaterialLayers local = ref dataRef.MaterialLayers[tupple.First.Value];
        if (tupple.Second)
          local.First += tupple.Third.Thickness;
        else
          dataRef.PushNewFirstLayer(ref local, tupple.Third);
      }
    }

    static GrassOnRocksPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      GrassOnRocksPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GrassOnRocksPostProcessor) obj).SerializeData(writer));
      GrassOnRocksPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GrassOnRocksPostProcessor) obj).DeserializeData(reader));
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(GrassOnRocksPostProcessor.Configuration value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<GrassOnRocksPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, GrassOnRocksPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteGeneric<TerrainMaterialProto>(this.GrassMaterialProto);
        Percent.Serialize(this.MaxCoverPercentage, writer);
        ThicknessTilesF.Serialize(this.MaxExaminedDepth, writer);
        writer.WriteInt(this.MaxIncompatibleMatCheckDistance);
        ThicknessTilesF.Serialize(this.MinAddedThickness, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        ThicknessTilesF.Serialize(this.ThresholdDeltaHeight, writer);
      }

      public static GrassOnRocksPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        GrassOnRocksPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<GrassOnRocksPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, GrassOnRocksPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.GrassMaterialProto = reader.ReadGenericAs<TerrainMaterialProto>();
        this.MaxCoverPercentage = Percent.Deserialize(reader);
        this.MaxExaminedDepth = ThicknessTilesF.Deserialize(reader);
        this.MaxIncompatibleMatCheckDistance = reader.ReadInt();
        this.MinAddedThickness = ThicknessTilesF.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
        this.ThresholdDeltaHeight = ThicknessTilesF.Deserialize(reader);
      }

      [EditorEnforceOrder(39)]
      public TerrainMaterialProto GrassMaterialProto { get; set; }

      [EditorEnforceOrder(44)]
      [EditorRange(0.0, 8.0)]
      [EditorLabel(null, "Distance that is checked for incompatible surface such as ocean next to grass.", false, false)]
      public int MaxIncompatibleMatCheckDistance { get; set; }

      [EditorEnforceOrder(49)]
      [EditorLabel(null, "Minimum thickness added to the terrain.", false, false)]
      [EditorRange(0.0, 0.5)]
      public ThicknessTilesF MinAddedThickness { get; set; }

      [EditorRange(0.5, 4.0)]
      [EditorEnforceOrder(53)]
      public ThicknessTilesF MaxExaminedDepth { get; set; }

      [EditorEnforceOrder(61)]
      [EditorRange(0.5, 1.5)]
      public Percent MaxCoverPercentage { get; set; }

      [EditorLabel(null, "Maximum height difference at which the added grass is zero. An average delta from all 8 neighbors is computed.", false, false)]
      [EditorRange(1.0, 5.0)]
      [EditorEnforceOrder(67)]
      public ThicknessTilesF ThresholdDeltaHeight { get; set; }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(71)]
      public int SortingPriorityAdjustment { get; set; }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMaxIncompatibleMatCheckDistance\u003Ek__BackingField = 5;
        // ISSUE: reference to a compiler-generated field
        this.\u003CMinAddedThickness\u003Ek__BackingField = 0.2.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMaxExaminedDepth\u003Ek__BackingField = 1.5.TilesThick();
        // ISSUE: reference to a compiler-generated field
        this.\u003CMaxCoverPercentage\u003Ek__BackingField = 110.Percent();
        // ISSUE: reference to a compiler-generated field
        this.\u003CThresholdDeltaHeight\u003Ek__BackingField = 1.5.TilesThick();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        GrassOnRocksPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GrassOnRocksPostProcessor.Configuration) obj).SerializeData(writer));
        GrassOnRocksPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GrassOnRocksPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }
  }
}
