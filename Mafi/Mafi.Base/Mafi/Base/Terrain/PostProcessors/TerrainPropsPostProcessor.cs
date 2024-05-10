// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.TerrainPropsPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Core.Utils;
using Mafi.Numerics;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  [GenerateSerializer(false, null, 0)]
  public class TerrainPropsPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MAX_EXPLORE_DEPTH;
    public readonly TerrainPropsPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private TerrainPropsPostProcessor.RecordsForMaterial[] m_records;
    [DoNotSave(0, null)]
    private TerrainManager m_terrain;
    [DoNotSave(0, null)]
    private GeneratedPropsData m_propsData;
    [DoNotSave(0, null)]
    private GeneratedTreesData m_treesData;

    public static void Serialize(TerrainPropsPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainPropsPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainPropsPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      TerrainPropsPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static TerrainPropsPostProcessor Deserialize(BlobReader reader)
    {
      TerrainPropsPostProcessor propsPostProcessor;
      if (reader.TryStartClassDeserialization<TerrainPropsPostProcessor>(out propsPostProcessor))
        reader.EnqueueDataDeserialization((object) propsPostProcessor, TerrainPropsPostProcessor.s_deserializeDataDelayedAction);
      return propsPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TerrainPropsPostProcessor>(this, "ConfigMutable", (object) TerrainPropsPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Props";

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

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 4000;

    public int PassCount => 1;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public TerrainPropsPostProcessor(
      TerrainPropsPostProcessor.Configuration configMutable)
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
      if (!extraDataReg.TryGetOrCreateExtraData<GeneratedTreesData>(out this.m_treesData))
      {
        Log.Error("Failed to obtain trees data.");
        return false;
      }
      if (!extraDataReg.TryGetOrCreateExtraData<GeneratedPropsData>(out this.m_propsData))
      {
        Log.Error("Failed to obtain props data.");
        return false;
      }
      IEnumerable<Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>> pairs = this.ConfigMutable.SpawnConfigs.AsEnumerable<TerrainPropsPostProcessor.PropSpawnConfig>().SelectMany<TerrainPropsPostProcessor.PropSpawnConfig, Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>>((Func<TerrainPropsPostProcessor.PropSpawnConfig, IEnumerable<Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>>>) (x => (IEnumerable<Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>>) x.SpawnMaterials.Select<Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>>((Func<TerrainMaterialProto, Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>>) (m =>
      {
        TerrainMaterialProto first = m;
        TerrainPropsPostProcessor.PropProtoWithVariant[] array = x.SpawnedProps.Where<TerrainPropsPostProcessor.PropProtoWithVariant>((Func<TerrainPropsPostProcessor.PropProtoWithVariant, bool>) (p => p.PropProto.IsNotPhantom)).ToArray<TerrainPropsPostProcessor.PropProtoWithVariant>();
        RngSeed64 spawnSeed = new RngSeed64(x.Seed.Value ^ x.SpawnProbability.RawValue.GetRngSeed());
        Percent spawnProbability = x.SpawnProbability;
        Percent minScale = x.MinScale;
        Percent maxScale = x.MaxScale;
        ThicknessTilesF materialThickness = x.MinSpawnMaterialThickness;
        ThicknessTilesF spawnMaterialDepth = x.MaxSpawnMaterialDepth;
        ThicknessTilesF maxHeightDelta = x.MaxHeightDelta;
        ThicknessTilesF placementHeightOffset = x.PlacementHeightOffset;
        ThicknessTilesF placementHeightRandom = x.PlacementHeightRandom;
        TerrainMaterialProto valueOrNull1 = x.PropMaterialOverride.ValueOrNull;
        // ISSUE: explicit non-virtual call
        TerrainMaterialSlimId propMaterialOverride = valueOrNull1 != null ? __nonvirtual (valueOrNull1.SlimId) : new TerrainMaterialSlimId();
        TerrainMaterialProto valueOrNull2 = x.BelowPropMaterial.ValueOrNull;
        // ISSUE: explicit non-virtual call
        TerrainMaterialSlimIdOption belowPropMaterial = (TerrainMaterialSlimIdOption) (valueOrNull2 != null ? __nonvirtual (valueOrNull2.SlimId) : new TerrainMaterialSlimId());
        TerrainPropsPostProcessor.RecordLookup second = new TerrainPropsPostProcessor.RecordLookup(array, spawnSeed, spawnProbability, minScale, maxScale, materialThickness, spawnMaterialDepth, maxHeightDelta, placementHeightOffset, placementHeightRandom, propMaterialOverride, belowPropMaterial);
        return Pair.Create<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>(first, second);
      })))).Where<Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>>((Func<Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup>, bool>) (x => x.First.IsNotPhantom && x.Second.Props.Length != 0));
      this.m_terrain = resolver.Resolve<TerrainManager>();
      this.m_records = this.m_terrain.TerrainMaterials.MapArray<TerrainPropsPostProcessor.RecordsForMaterial>((Func<TerrainMaterialProto, TerrainPropsPostProcessor.RecordsForMaterial>) (_ => new TerrainPropsPostProcessor.RecordsForMaterial(ImmutableArray<TerrainPropsPostProcessor.RecordLookup>.Empty, ThicknessTilesF.MaxValue, ThicknessTilesF.Zero)));
      foreach (Pair<TerrainMaterialProto, TerrainPropsPostProcessor.RecordLookup> pair in pairs)
      {
        TerrainPropsPostProcessor.RecordsForMaterial record = this.m_records[(int) pair.First.SlimId.Value];
        this.m_records[(int) pair.First.SlimId.Value] = new TerrainPropsPostProcessor.RecordsForMaterial(record.Records.Add(pair.Second), record.MinSpawnThickness.Min(pair.Second.MinSpawnMaterialThickness), record.MaxSpawnDepth.Max(pair.Second.MaxSpawnMaterialDepth));
      }
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      return true;
    }

    public void Reset()
    {
      this.m_records = (TerrainPropsPostProcessor.RecordsForMaterial[]) null;
      this.m_terrain = (TerrainManager) null;
      this.m_propsData = (GeneratedPropsData) null;
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
      BitMap occupiedTilesBitmap = new BitMap(36864);
      Lyst<Tile2i> occupiedTiles = new Lyst<Tile2i>();
      XorRsr128PlusGenerator rng = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 1UL, 2UL);
      GeneratedPropsData.Chunk propChunk = this.m_propsData.GetOrCreateChunk(chunk);
      GeneratedTreesData.Chunk treeChunk = this.m_treesData.GetOrCreateChunk(chunk);
      int terrainStride = dataReadOnly.Width;
      ushort[] flags = dataReadOnly.Flags;
      bool flag1 = false;
      for (int addedY = -1; addedY <= 1; ++addedY)
      {
        for (int addedX = -1; addedX <= 1; ++addedX)
        {
          Chunk2i spawnChunk = chunk.AddX(addedX).AddY(addedY);
          if ((long) (uint) (spawnChunk.X * 64 - dataOrigin.X) < (long) dataReadOnly.Width && (long) (uint) (spawnChunk.Y * 64 - dataOrigin.Y) < (long) dataReadOnly.Height)
          {
            int tileIndex1 = dataReadOnly.GetTileIndex(spawnChunk.Tile2i - dataOrigin);
            int tileYInChunk = 0;
            while (tileYInChunk < 64)
            {
              for (int tileXInChunk = 0; tileXInChunk < 64; ++tileXInChunk)
              {
                int tileIndex2 = tileIndex1 + tileXInChunk;
                if (((int) flags[tileIndex2] & 5) == 0)
                {
                  bool flag2 = false;
                  TileMaterialLayers materialLayer = dataReadOnly.MaterialLayers[tileIndex2];
                  TerrainPropsPostProcessor.RecordsForMaterial record1 = this.m_records[materialLayer.First.SlimIdRaw];
                  if (record1.Records.IsNotEmpty && materialLayer.First.Thickness >= record1.MinSpawnThickness)
                  {
                    foreach (TerrainPropsPostProcessor.RecordLookup record2 in record1.Records)
                    {
                      if (materialLayer.First.Thickness >= record2.MinSpawnMaterialThickness)
                      {
                        flag2 = trySpawn(spawnChunk, tileIndex2, tileXInChunk, tileYInChunk, materialLayer.First.SlimId, record2);
                        if (flag2)
                        {
                          flag1 = true;
                          break;
                        }
                      }
                    }
                    if (flag2)
                    {
                      flag1 = true;
                      continue;
                    }
                  }
                  if (materialLayer.First.Thickness < TerrainPropsPostProcessor.MAX_EXPLORE_DEPTH)
                  {
                    TerrainPropsPostProcessor.RecordsForMaterial record3 = this.m_records[materialLayer.Second.SlimIdRaw];
                    if (record3.Records.IsNotEmpty && materialLayer.First.Thickness <= record3.MaxSpawnDepth && materialLayer.Second.Thickness >= record3.MinSpawnThickness)
                    {
                      foreach (TerrainPropsPostProcessor.RecordLookup record4 in record3.Records)
                      {
                        if (materialLayer.First.Thickness <= record4.MaxSpawnMaterialDepth && materialLayer.Second.Thickness >= record4.MinSpawnMaterialThickness)
                        {
                          flag2 = trySpawn(spawnChunk, tileIndex2, tileXInChunk, tileYInChunk, materialLayer.Second.SlimId, record4);
                          if (flag2)
                          {
                            flag1 = true;
                            break;
                          }
                        }
                      }
                      if (flag2)
                      {
                        flag1 = true;
                        continue;
                      }
                    }
                  }
                  ThicknessTilesF thicknessTilesF = materialLayer.First.Thickness + materialLayer.Second.Thickness;
                  if (thicknessTilesF < TerrainPropsPostProcessor.MAX_EXPLORE_DEPTH)
                  {
                    TerrainPropsPostProcessor.RecordsForMaterial record5 = this.m_records[materialLayer.Third.SlimIdRaw];
                    if (record5.Records.IsNotEmpty && thicknessTilesF <= record5.MaxSpawnDepth && materialLayer.Third.Thickness >= record5.MinSpawnThickness)
                    {
                      foreach (TerrainPropsPostProcessor.RecordLookup record6 in record5.Records)
                      {
                        if (thicknessTilesF <= record6.MaxSpawnMaterialDepth && materialLayer.Third.Thickness >= record6.MinSpawnMaterialThickness && trySpawn(spawnChunk, tileIndex2, tileXInChunk, tileYInChunk, materialLayer.Third.SlimId, record6))
                        {
                          flag1 = true;
                          break;
                        }
                      }
                    }
                  }
                }
              }
              ++tileYInChunk;
              tileIndex1 += terrainStride;
            }
          }
        }
      }
      if (flag1)
        return;
      lock (this.m_unaffectedChunksCache.BackingArray)
        this.m_unaffectedChunksCache.Add(chunk);

      bool trySpawn(
        Chunk2i spawnChunk,
        int tileIndex,
        int tileXInChunk,
        int tileYInChunk,
        TerrainMaterialSlimId terrainMaterialSlimId,
        TerrainPropsPostProcessor.RecordLookup record,
        bool forceSpawn = false)
      {
        rng.SeedFast((ulong) ((long) tileXInChunk | (long) (tileYInChunk << 16) | (long) spawnChunk.AsPackedUint << 32), record.SpawnSeed.Value);
        if (!forceSpawn)
        {
          if (!rng.TestProbability(record.SpawnProbability))
            return false;
          HeightTilesF height = dataReadOnly.Heights[tileIndex];
          ThicknessTilesF thicknessTilesF = height - dataReadOnly.Heights[tileIndex - 1];
          ThicknessTilesF rhs1 = height - dataReadOnly.Heights[tileIndex + 1];
          ThicknessTilesF rhs2 = height - dataReadOnly.Heights[tileIndex - terrainStride];
          ThicknessTilesF rhs3 = height - dataReadOnly.Heights[tileIndex + terrainStride];
          if (thicknessTilesF.Max(rhs1).Max(rhs2).Max(rhs3) > record.MaxHeightDelta)
            return false;
        }
        int index1 = rng.NextInt(record.Props.Length);
        TerrainPropsPostProcessor.PropProtoWithVariant prop = record.Props[index1];
        TerrainPropProto propProto = prop.PropProto;
        Percent scale = propProto.BaseScale * record.MinScale.Lerp(record.MaxScale, rng.NextPercent());
        TerrainPropData.PropVariant variant;
        if (propProto.Graphics.UseTerrainTextures)
        {
          variant = new TerrainPropData.PropVariant(record.PropMaterialOverride.HasValue ? record.PropMaterialOverride.Value : terrainMaterialSlimId, rng.NextFloat(), rng.NextFloat(), scale.ToFloat());
        }
        else
        {
          int index2;
          if (prop.RestrictVariants)
          {
            int minValueIncl = prop.MinVariantIndex.Min(propProto.Variants.Length - 1);
            int num = prop.MaxVariantIndex.Max(minValueIncl);
            index2 = rng.NextInt(minValueIncl, num + 1);
          }
          else
            index2 = rng.NextInt(propProto.Variants.Length);
          variant = propProto.Variants[index2];
        }
        Tile2f position = new Tile2i(spawnChunk.Tile2i.X + tileXInChunk, spawnChunk.Tile2i.Y + tileYInChunk).CornerTile2f + rng.NextRelTile2fBetween01();
        TerrainPropData first = new TerrainPropData(propProto, variant, position, dataReadOnly.Heights[tileIndex], scale, propProto.AllowYawRandomization ? rng.NextAngleSlim() : AngleSlim.Zero, propProto.AllowPitchRandomization ? rng.NextAngleSlim() : AngleSlim.Zero, propProto.AllowRollRandomization ? rng.NextAngleSlim() : AngleSlim.Zero, record.PlacementHeightOffset + record.PlacementHeightRandom * rng.NextFix32Between01Fast());
        occupiedTiles.Clear();
        first.CalculateOccupiedTiles(this.m_terrain, occupiedTiles);
        foreach (Tile2i occupiedTile in occupiedTiles)
        {
          if (!forceSpawn && treeChunk.Trees.ContainsKey(new TreeId(occupiedTile.AsSlim)))
            return true;
          if ((long) (uint) (occupiedTile.X - dataOrigin.X) < (long) dataReadOnly.Width && (long) (uint) (occupiedTile.Y - dataOrigin.Y) < (long) dataReadOnly.Height)
          {
            GeneratedPropsData.Chunk chunk = this.m_propsData.GetOrCreateChunk(occupiedTile.ChunkCoord2i);
            bool flag = false;
            foreach (Polygon2fFast clearedPolygon in chunk.ClearedPolygons)
            {
              if (clearedPolygon.Contains(occupiedTile.Vector2f))
              {
                flag = true;
                break;
              }
            }
            if (flag)
              return true;
            int num1 = occupiedTile.X - chunk.Tile2i.X + 64;
            if ((uint) num1 < 192U)
            {
              int num2 = occupiedTile.Y - chunk.Tile2i.Y + 64;
              if ((uint) num2 < 192U)
              {
                int index3 = num2 * 3 * 64 + num1;
                if (!forceSpawn && occupiedTilesBitmap.IsSet(index3))
                  return true;
                occupiedTilesBitmap.SetBit(index3);
              }
            }
          }
        }
        if (spawnChunk == chunk)
        {
          TerrainMaterialProto full = (record.BelowPropMaterial.HasValue ? record.BelowPropMaterial.Value : terrainMaterialSlimId).ToFull(this.m_terrain);
          propChunk.PropsAndSurfaceMaterials.Add<TerrainPropId, Pair<TerrainPropData, TerrainMaterialProto>>(first.Id, new Pair<TerrainPropData, TerrainMaterialProto>(first, full));
        }
        return true;
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
    }

    static TerrainPropsPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TerrainPropsPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainPropsPostProcessor) obj).SerializeData(writer));
      TerrainPropsPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainPropsPostProcessor) obj).DeserializeData(reader));
      TerrainPropsPostProcessor.MAX_EXPLORE_DEPTH = 3.0.TilesThick();
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfigWithInit, ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      [EditorEnforceOrder(45)]
      [EditorCollection(true, true)]
      public readonly Lyst<TerrainPropsPostProcessor.PropSpawnConfig> SpawnConfigs;
      [EditorIgnore]
      public readonly Lyst<TerrainPropData> CustomPlacedProps;

      public static void Serialize(TerrainPropsPostProcessor.Configuration value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TerrainPropsPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TerrainPropsPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Lyst<TerrainPropData>.Serialize(this.CustomPlacedProps, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
        Lyst<TerrainPropsPostProcessor.PropSpawnConfig>.Serialize(this.SpawnConfigs, writer);
      }

      public static TerrainPropsPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        TerrainPropsPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<TerrainPropsPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, TerrainPropsPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<TerrainPropsPostProcessor.Configuration>(this, "CustomPlacedProps", (object) Lyst<TerrainPropData>.Deserialize(reader));
        this.SortingPriorityAdjustment = reader.ReadInt();
        reader.SetField<TerrainPropsPostProcessor.Configuration>(this, "SpawnConfigs", (object) Lyst<TerrainPropsPostProcessor.PropSpawnConfig>.Deserialize(reader));
      }

      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      [EditorEnforceOrder(49)]
      public int SortingPriorityAdjustment { get; set; }

      [DoNotSave(0, null)]
      [EditorEnforceOrder(53)]
      public Action CreateNewConfig { get; private set; }

      public void InitializeInMapEditor(IResolver resolver)
      {
        ProtosDb protosDb = resolver.Resolve<ProtosDb>();
        TerrainMaterialProto newMat = protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Dirt);
        TerrainPropProto newProp = protosDb.GetOrThrow<TerrainPropProto>(Ids.TerrainProps.Stone01);
        foreach (TerrainPropsPostProcessor.PropSpawnConfig spawnConfig in this.SpawnConfigs)
          this.addConfigActions(spawnConfig, newMat, newProp);
        this.CreateNewConfig = (Action) (() => this.SpawnConfigs.Insert(0, this.addConfigActions(new TerrainPropsPostProcessor.PropSpawnConfig(), newMat, newProp)));
      }

      private TerrainPropsPostProcessor.PropSpawnConfig addConfigActions(
        TerrainPropsPostProcessor.PropSpawnConfig config,
        TerrainMaterialProto newMat,
        TerrainPropProto newProp)
      {
        config.AddSpawnMaterial = (Action) (() => config.SpawnMaterials.Add(newMat));
        config.AddSpawnedProps = (Action) (() => config.SpawnedProps.Add(new TerrainPropsPostProcessor.PropProtoWithVariant()
        {
          PropProto = newProp
        }));
        return config;
      }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.SpawnConfigs = new Lyst<TerrainPropsPostProcessor.PropSpawnConfig>();
        this.CustomPlacedProps = new Lyst<TerrainPropData>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        TerrainPropsPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainPropsPostProcessor.Configuration) obj).SerializeData(writer));
        TerrainPropsPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainPropsPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct PropProtoWithVariant
    {
      [EditorEnforceOrder(154)]
      public TerrainPropProto PropProto;
      [EditorEnforceOrder(157)]
      public bool RestrictVariants;
      [EditorRange(0.0, 256.0)]
      [EditorEnforceOrder(161)]
      public int MinVariantIndex;
      [EditorRange(0.0, 256.0)]
      [EditorEnforceOrder(165)]
      public int MaxVariantIndex;

      public static void Serialize(
        TerrainPropsPostProcessor.PropProtoWithVariant value,
        BlobWriter writer)
      {
        writer.WriteInt(value.MaxVariantIndex);
        writer.WriteInt(value.MinVariantIndex);
        writer.WriteGeneric<TerrainPropProto>(value.PropProto);
        writer.WriteBool(value.RestrictVariants);
      }

      public static TerrainPropsPostProcessor.PropProtoWithVariant Deserialize(BlobReader reader)
      {
        return new TerrainPropsPostProcessor.PropProtoWithVariant()
        {
          MaxVariantIndex = reader.ReadInt(),
          MinVariantIndex = reader.ReadInt(),
          PropProto = reader.ReadGenericAs<TerrainPropProto>(),
          RestrictVariants = reader.ReadBool()
        };
      }
    }

    [GenerateSerializer(false, null, 0)]
    public class PropSpawnConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      [EditorEnforceOrder(87)]
      [EditorMaxLength(50)]
      public string Name;
      [EditorCollection(true, true)]
      [EditorEnforceOrder(91)]
      public readonly Lyst<TerrainMaterialProto> SpawnMaterials;
      [EditorEnforceOrder(95)]
      [EditorRange(0.0, 2.0)]
      public ThicknessTilesF MinSpawnMaterialThickness;
      [EditorEnforceOrder(99)]
      [EditorRange(0.0, 3.0)]
      public ThicknessTilesF MaxSpawnMaterialDepth;
      [EditorCollection(true, true)]
      [EditorEnforceOrder(107)]
      public readonly Lyst<TerrainPropsPostProcessor.PropProtoWithVariant> SpawnedProps;
      [EditorEnforceOrder(114)]
      public Option<TerrainMaterialProto> PropMaterialOverride;
      [EditorEnforceOrder(117)]
      public Option<TerrainMaterialProto> BelowPropMaterial;
      [EditorEnforceOrder(121)]
      [EditorRange(0.0, 0.1)]
      public Percent SpawnProbability;
      [EditorEnforceOrder(125)]
      [EditorRange(0.1, 2.0)]
      public Percent MinScale;
      [EditorRange(0.1, 2.0)]
      [EditorEnforceOrder(129)]
      public Percent MaxScale;
      [EditorEnforceOrder(135)]
      [EditorRange(0.0, 10.0)]
      [EditorLabel(null, "Prop will not be placed if height difference to any neighboring tiles is greater than the configured value. This can be used to restrict placement on flat terrain.", false, false)]
      public ThicknessTilesF MaxHeightDelta;
      [EditorEnforceOrder(139)]
      [EditorRange(-2.0, 2.0)]
      public ThicknessTilesF PlacementHeightOffset;
      [EditorRange(0.0, 1.0)]
      [EditorEnforceOrder(143)]
      public ThicknessTilesF PlacementHeightRandom;
      [EditorEnforceOrder(146)]
      public RngSeed64 Seed;

      public static void Serialize(
        TerrainPropsPostProcessor.PropSpawnConfig value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TerrainPropsPostProcessor.PropSpawnConfig>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TerrainPropsPostProcessor.PropSpawnConfig.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Option<TerrainMaterialProto>.Serialize(this.BelowPropMaterial, writer);
        ThicknessTilesF.Serialize(this.MaxHeightDelta, writer);
        Percent.Serialize(this.MaxScale, writer);
        ThicknessTilesF.Serialize(this.MaxSpawnMaterialDepth, writer);
        Percent.Serialize(this.MinScale, writer);
        ThicknessTilesF.Serialize(this.MinSpawnMaterialThickness, writer);
        writer.WriteString(this.Name);
        ThicknessTilesF.Serialize(this.PlacementHeightOffset, writer);
        ThicknessTilesF.Serialize(this.PlacementHeightRandom, writer);
        Option<TerrainMaterialProto>.Serialize(this.PropMaterialOverride, writer);
        RngSeed64.Serialize(this.Seed, writer);
        Lyst<TerrainPropsPostProcessor.PropProtoWithVariant>.Serialize(this.SpawnedProps, writer);
        Lyst<TerrainMaterialProto>.Serialize(this.SpawnMaterials, writer);
        Percent.Serialize(this.SpawnProbability, writer);
      }

      public static TerrainPropsPostProcessor.PropSpawnConfig Deserialize(BlobReader reader)
      {
        TerrainPropsPostProcessor.PropSpawnConfig propSpawnConfig;
        if (reader.TryStartClassDeserialization<TerrainPropsPostProcessor.PropSpawnConfig>(out propSpawnConfig))
          reader.EnqueueDataDeserialization((object) propSpawnConfig, TerrainPropsPostProcessor.PropSpawnConfig.s_deserializeDataDelayedAction);
        return propSpawnConfig;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.BelowPropMaterial = Option<TerrainMaterialProto>.Deserialize(reader);
        this.MaxHeightDelta = ThicknessTilesF.Deserialize(reader);
        this.MaxScale = Percent.Deserialize(reader);
        this.MaxSpawnMaterialDepth = ThicknessTilesF.Deserialize(reader);
        this.MinScale = Percent.Deserialize(reader);
        this.MinSpawnMaterialThickness = ThicknessTilesF.Deserialize(reader);
        this.Name = reader.ReadString();
        this.PlacementHeightOffset = ThicknessTilesF.Deserialize(reader);
        this.PlacementHeightRandom = ThicknessTilesF.Deserialize(reader);
        this.PropMaterialOverride = Option<TerrainMaterialProto>.Deserialize(reader);
        this.Seed = RngSeed64.Deserialize(reader);
        reader.SetField<TerrainPropsPostProcessor.PropSpawnConfig>(this, "SpawnedProps", (object) Lyst<TerrainPropsPostProcessor.PropProtoWithVariant>.Deserialize(reader));
        reader.SetField<TerrainPropsPostProcessor.PropSpawnConfig>(this, "SpawnMaterials", (object) Lyst<TerrainMaterialProto>.Deserialize(reader));
        this.SpawnProbability = Percent.Deserialize(reader);
      }

      [EditorEnforceOrder(103)]
      [DoNotSave(0, null)]
      public Action AddSpawnMaterial { get; set; }

      [EditorEnforceOrder(111)]
      [DoNotSave(0, null)]
      public Action AddSpawnedProps { get; set; }

      public PropSpawnConfig()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Name = "New config";
        this.SpawnMaterials = new Lyst<TerrainMaterialProto>();
        this.MinSpawnMaterialThickness = 0.5.TilesThick();
        this.MaxSpawnMaterialDepth = 0.5.TilesThick();
        this.SpawnedProps = new Lyst<TerrainPropsPostProcessor.PropProtoWithVariant>();
        this.SpawnProbability = 0.01.Percent();
        this.MinScale = 80.Percent();
        this.MaxScale = 120.Percent();
        this.MaxHeightDelta = 0.2.TilesThick();
        this.Seed = MafiMath.GetRngSeedFromCurrentTime();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static PropSpawnConfig()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        TerrainPropsPostProcessor.PropSpawnConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainPropsPostProcessor.PropSpawnConfig) obj).SerializeData(writer));
        TerrainPropsPostProcessor.PropSpawnConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainPropsPostProcessor.PropSpawnConfig) obj).DeserializeData(reader));
      }
    }

    private readonly struct RecordsForMaterial
    {
      public readonly ImmutableArray<TerrainPropsPostProcessor.RecordLookup> Records;
      public readonly ThicknessTilesF MinSpawnThickness;
      public readonly ThicknessTilesF MaxSpawnDepth;

      public RecordsForMaterial(
        ImmutableArray<TerrainPropsPostProcessor.RecordLookup> records,
        ThicknessTilesF minSpawnThickness,
        ThicknessTilesF maxSpawnDepth)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Records = records;
        this.MinSpawnThickness = minSpawnThickness;
        this.MaxSpawnDepth = maxSpawnDepth;
      }
    }

    public readonly struct RecordLookup
    {
      public readonly TerrainPropsPostProcessor.PropProtoWithVariant[] Props;
      public readonly RngSeed64 SpawnSeed;
      public readonly Percent SpawnProbability;
      public readonly Percent MinScale;
      public readonly Percent MaxScale;
      public readonly ThicknessTilesF MinSpawnMaterialThickness;
      public readonly ThicknessTilesF MaxSpawnMaterialDepth;
      public readonly ThicknessTilesF MaxHeightDelta;
      public readonly ThicknessTilesF PlacementHeightOffset;
      public readonly ThicknessTilesF PlacementHeightRandom;
      public readonly TerrainMaterialSlimIdOption PropMaterialOverride;
      public readonly TerrainMaterialSlimIdOption BelowPropMaterial;

      public RecordLookup(
        TerrainPropsPostProcessor.PropProtoWithVariant[] props,
        RngSeed64 spawnSeed,
        Percent spawnProbability,
        Percent minScale,
        Percent maxScale,
        ThicknessTilesF minSpawnMaterialThickness,
        ThicknessTilesF maxSpawnMaterialDepth,
        ThicknessTilesF maxHeightDelta,
        ThicknessTilesF placementHeightOffset,
        ThicknessTilesF placementHeightRandom,
        TerrainMaterialSlimId propMaterialOverride,
        TerrainMaterialSlimIdOption belowPropMaterial)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Props = props;
        this.SpawnSeed = spawnSeed;
        this.SpawnProbability = spawnProbability;
        this.MinScale = minScale;
        this.MaxScale = maxScale;
        this.MinSpawnMaterialThickness = minSpawnMaterialThickness;
        this.MaxSpawnMaterialDepth = maxSpawnMaterialDepth;
        this.MaxHeightDelta = maxHeightDelta;
        this.PlacementHeightOffset = placementHeightOffset;
        this.PlacementHeightRandom = placementHeightRandom;
        this.PropMaterialOverride = (TerrainMaterialSlimIdOption) propMaterialOverride;
        this.BelowPropMaterial = belowPropMaterial;
      }
    }
  }
}
