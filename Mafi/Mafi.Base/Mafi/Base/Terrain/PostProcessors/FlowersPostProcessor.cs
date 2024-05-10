// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.PostProcessors.FlowersPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Generation.FeatureGenerators;
using Mafi.Core.Utils;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain.PostProcessors
{
  /// <summary>Places flower locations then grows flowers in clumps.</summary>
  [GenerateSerializer(false, null, 0)]
  public class FlowersPostProcessor : 
    ITerrainPostProcessorV2,
    ITerrainFeatureBase,
    IEditableTerrainFeature
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF FLOWERS_MIN_THICKNESS;
    public static readonly int MAX_FLOWER_SPREAD_DISTANCE_SQUARED;
    public readonly FlowersPostProcessor.Configuration ConfigMutable;
    private TerrainChunk64BitMap m_unaffectedChunksCache;
    [DoNotSave(0, null)]
    private TerrainManager m_terrainManager;
    [DoNotSave(0, null)]
    private TerrainMaterialProto[] m_spawnMaterials;
    [DoNotSave(0, null)]
    private INoise2D[] m_thicknessFunctions;
    [DoNotSave(0, null)]
    private bool[] m_canHaveFlowers;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, Lyst<Tile2i>[]> m_locations;
    [DoNotSave(0, null)]
    private Dict<Chunk2i, Dict<Tile2iIndex, TerrainMaterialThicknessSlim>> m_newFloorThickness;
    private RelTile2f m_translation;

    public static void Serialize(FlowersPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FlowersPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FlowersPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      FlowersPostProcessor.Configuration.Serialize(this.ConfigMutable, writer);
      writer.WriteInt(this.Id);
      writer.WriteBool(this.IsDisabled);
      RelTile2f.Serialize(this.m_translation, writer);
      TerrainChunk64BitMap.Serialize(this.m_unaffectedChunksCache, writer);
    }

    public static FlowersPostProcessor Deserialize(BlobReader reader)
    {
      FlowersPostProcessor flowersPostProcessor;
      if (reader.TryStartClassDeserialization<FlowersPostProcessor>(out flowersPostProcessor))
        reader.EnqueueDataDeserialization((object) flowersPostProcessor, FlowersPostProcessor.s_deserializeDataDelayedAction);
      return flowersPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<FlowersPostProcessor>(this, "ConfigMutable", (object) FlowersPostProcessor.Configuration.Deserialize(reader));
      this.Id = reader.ReadInt();
      this.IsDisabled = reader.ReadBool();
      this.m_translation = RelTile2f.Deserialize(reader);
      this.m_unaffectedChunksCache = TerrainChunk64BitMap.Deserialize(reader);
    }

    public string Name => "Flowers";

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

    public int SortingPriority => this.ConfigMutable.SortingPriorityAdjustment + 3000;

    public int PassCount => 2;

    [DoNotSave(0, null)]
    public TimeSpan LastGenerationTime { get; set; }

    public ITerrainFeatureConfig Config => (ITerrainFeatureConfig) this.ConfigMutable;

    public FlowersPostProcessor(FlowersPostProcessor.Configuration configMutable)
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_translation = RelTile2f.Zero;
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
      if (this.m_newFloorThickness != null)
        return true;
      this.m_terrainManager = resolver.Resolve<TerrainManager>();
      this.m_canHaveFlowers = this.m_terrainManager.TerrainMaterials.MapArray<bool>((Func<TerrainMaterialProto, bool>) (x => x.IsFarmable && !x.IsForestFloor));
      Lyst<FlowersPostProcessor.FlowersConfig> flowersConfigs = this.ConfigMutable.FlowersConfigs;
      int count = flowersConfigs.Count;
      this.m_spawnMaterials = new TerrainMaterialProto[count];
      this.m_thicknessFunctions = new INoise2D[count];
      Dict<string, object> extraArgs = new Dict<string, object>();
      for (int index = 0; index < flowersConfigs.Count; ++index)
      {
        FlowersPostProcessor.FlowersConfig flowersConfig = flowersConfigs[index];
        this.m_spawnMaterials[index] = flowersConfig.SpawnMaterial;
        string error;
        if (!flowersConfig.ThicknessFn.TryCreateNoise(resolver, (IReadOnlyDictionary<string, object>) extraArgs, out this.m_thicknessFunctions[index], out error))
        {
          Log.Error(error);
          return false;
        }
      }
      this.m_unaffectedChunksCache.EnsureCorrectSize(terrainSize);
      this.m_locations = new Dict<Chunk2i, Lyst<Tile2i>[]>();
      this.m_newFloorThickness = new Dict<Chunk2i, Dict<Tile2iIndex, TerrainMaterialThicknessSlim>>();
      return true;
    }

    public void Reset()
    {
      this.m_canHaveFlowers = (bool[]) null;
      this.m_spawnMaterials = (TerrainMaterialProto[]) null;
      this.m_locations = (Dict<Chunk2i, Lyst<Tile2i>[]>) null;
      this.m_newFloorThickness = (Dict<Chunk2i, Dict<Tile2iIndex, TerrainMaterialThicknessSlim>>) null;
      this.m_thicknessFunctions = (INoise2D[]) null;
    }

    public void ClearCaches() => this.m_unaffectedChunksCache.Clear();

    public void TranslateBy(RelTile3f delta) => this.m_translation += delta.Xy;

    public void RotateBy(AngleDegrees1f rotation)
    {
    }

    public void AnalyzeChunkThreadSafe(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly,
      int pass)
    {
      if (pass == 0)
        this.computeLocations(chunk, dataOrigin, dataReadOnly);
      else
        this.computeFlowerThicknesses(chunk, dataOrigin, dataReadOnly);
    }

    private void computeLocations(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly)
    {
      lock (this.m_unaffectedChunksCache.BackingArray)
      {
        if (this.m_unaffectedChunksCache.Contains(chunk))
          return;
      }
      Lyst<Tile2i>[] array = this.ConfigMutable.FlowersConfigs.ToArray<Lyst<Tile2i>>((Func<FlowersPostProcessor.FlowersConfig, Lyst<Tile2i>>) (_ => new Lyst<Tile2i>()));
      Chunk2i chunk2i = chunk.AddX(this.m_translation.X.ToIntFloored().RoundDiv(64)).AddY(this.m_translation.Y.ToIntFloored().RoundDiv(64));
      XorRsr128PlusGenerator random = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 1UL, 2UL);
      random.SeedFast((ulong) chunk2i.AsPackedUint, this.ConfigMutable.Seed.Value);
      random.Jump();
      bool flag1 = false;
      int tileIndex1 = dataReadOnly.GetTileIndex(chunk.Tile2i - dataOrigin);
      int y = 0;
      while (y < 64)
      {
        for (int x = 0; x < 64; ++x)
        {
          int tileIndex2 = tileIndex1 + x;
          int index = random.NextIntNotNegative() % this.ConfigMutable.FlowersConfigs.Count;
          FlowersPostProcessor.FlowersConfig flowersConfig = this.ConfigMutable.FlowersConfigs[index];
          if (random.TestProbability(flowersConfig.SpawnProbability) && this.canGrowFlowers(flowersConfig, dataReadOnly, tileIndex2))
          {
            RelTile1f distanceFromOthers = flowersConfig.MinDistanceFromOthers;
            if (distanceFromOthers.IsPositive)
            {
              Tile2i other = chunk.Tile2i + new RelTile2i(x, y);
              bool flag2 = false;
              distanceFromOthers = flowersConfig.MinDistanceFromOthers;
              Fix64 squared = distanceFromOthers.Squared;
              foreach (Tile2i tile2i in array[index])
              {
                if (tile2i.DistanceSqrTo(other) < squared)
                {
                  flag2 = true;
                  break;
                }
              }
              if (!flag2)
              {
                array[index].Add(other);
                flag1 = true;
              }
            }
          }
        }
        ++y;
        tileIndex1 += dataReadOnly.Width;
      }
      if (!flag1)
      {
        lock (this.m_unaffectedChunksCache.BackingArray)
          this.m_unaffectedChunksCache.Add(chunk);
      }
      else
      {
        lock (this.m_locations)
          this.m_locations[chunk] = array;
      }
    }

    private bool canGrowFlowers(
      FlowersPostProcessor.FlowersConfig config,
      TerrainManager.TerrainData dataReadOnly,
      int tileIndex)
    {
      if (((int) dataReadOnly.Flags[tileIndex] & 5) != 0)
        return false;
      TileMaterialLayers materialLayer = dataReadOnly.MaterialLayers[tileIndex];
      if (!this.m_canHaveFlowers[materialLayer.First.SlimIdRaw])
        return false;
      TerrainMaterialSlimId slimId = config.SpawnMaterial.SlimId;
      ThicknessTilesF materialMinThickness = config.SpawnMaterialMinThickness;
      if (materialLayer.First.SlimId == slimId && materialLayer.First.Thickness >= materialMinThickness)
        return true;
      return !(materialLayer.First.Thickness >= config.SpawnMaterialMaxDepth) && materialLayer.Second.SlimId == slimId && materialLayer.Second.Thickness >= materialMinThickness;
    }

    private void computeFlowerThicknesses(
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly)
    {
      lock (this.m_unaffectedChunksCache.BackingArray)
      {
        if (this.m_unaffectedChunksCache.Contains(chunk))
          return;
      }
      Lyst<Tile2i>[] array = this.ConfigMutable.FlowersConfigs.ToArray<Lyst<Tile2i>>((Func<FlowersPostProcessor.FlowersConfig, Lyst<Tile2i>>) (_ => new Lyst<Tile2i>()));
      Dict<Tile2iIndex, TerrainMaterialThicknessSlim> newFloorThickness = new Dict<Tile2iIndex, TerrainMaterialThicknessSlim>();
      XorRsr128PlusGenerator random = new XorRsr128PlusGenerator(RandomGeneratorType.Unrestricted, 1UL, 2UL);
      Chunk64Area chunk64Area = new Chunk64Area(dataOrigin.ChunkCoord2i, dataReadOnly.Size.Vector2i / 64);
      for (int addedY = -2; addedY <= 2; ++addedY)
      {
        for (int addedX = -2; addedX <= 2; ++addedX)
        {
          Chunk2i chunk1 = chunk.AddX(addedX).AddY(addedY);
          Lyst<Tile2i>[] lystArray;
          if (chunk64Area.Contains(chunk1) && this.m_locations.TryGetValue(chunk, out lystArray))
          {
            for (int index = 0; index < lystArray.Length; ++index)
              array[index].AddRange(lystArray[index]);
          }
        }
      }
      for (int index = 0; index < array.Length; ++index)
      {
        FlowersPostProcessor.FlowersConfig flowersConfig = this.ConfigMutable.FlowersConfigs[index];
        RectangleTerrainArea2i rectangleTerrainArea2i = chunk.Area.ExtendBy(flowersConfig.MaxInfluenceDistance.Value + 1);
        foreach (Tile2i tile2i1 in array[index])
        {
          if (rectangleTerrainArea2i.ContainsTile(tile2i1))
          {
            bool flag = false;
            Fix64 squared = flowersConfig.MinDistanceFromOthers.Squared;
            foreach (Tile2i tile2i2 in array[index])
            {
              if (tile2i2 != tile2i1 && tile2i2.X * tile2i2.Y >= tile2i1.X * tile2i1.Y && tile2i2.DistanceSqrTo(tile2i1) < squared)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              random.SeedFast(flowersConfig.Seed.Value, tile2i1.XyPacked);
              random.Jump();
              Percent size = !(flowersConfig.SizeRandomnessMin < flowersConfig.SizeRandomnessMax) ? Percent.Hundred : flowersConfig.SizeRandomnessMin + (flowersConfig.SizeRandomnessMax - flowersConfig.SizeRandomnessMin) * random.NextPercent();
              this.generateFlowersOnChunk(index, tile2i1, size, chunk, dataOrigin, dataReadOnly, newFloorThickness);
            }
          }
        }
      }
      if (newFloorThickness.IsEmpty)
      {
        lock (this.m_unaffectedChunksCache.BackingArray)
          this.m_unaffectedChunksCache.Add(chunk);
      }
      else
      {
        lock (this.m_newFloorThickness)
          this.m_newFloorThickness[chunk] = newFloorThickness;
      }
    }

    private void generateFlowersOnChunk(
      int configIndex,
      Tile2i seedTile,
      Percent size,
      Chunk2i chunk,
      Tile2i dataOrigin,
      TerrainManager.TerrainData dataReadOnly,
      Dict<Tile2iIndex, TerrainMaterialThicknessSlim> newFloorThickness)
    {
      FlowersPostProcessor.FlowersConfig flowersConfig = this.ConfigMutable.FlowersConfigs[configIndex];
      long squared = flowersConfig.MaxInfluenceDistance.Squared;
      INoise2D thicknessFunction = this.m_thicknessFunctions[configIndex];
      TerrainMaterialSlimId slimId = flowersConfig.FlowerMaterial.SlimId;
      ushort[] flags = dataReadOnly.Flags;
      int tileIndex1 = dataReadOnly.GetTileIndex(chunk.Tile2i, dataOrigin);
      int y = 0;
      while (y < 64)
      {
        for (int x = 0; x < 64; ++x)
        {
          int tileIndex2 = tileIndex1 + x;
          if (((int) flags[tileIndex2] & 5) == 0)
          {
            Tile2i tile2i = chunk.Tile2i + new RelTile2i(x, y);
            long self = tile2i.DistanceSqrTo(seedTile);
            if (self <= squared && this.canGrowFlowers(flowersConfig, dataReadOnly, tileIndex2))
            {
              ThicknessTilesF thickness = new ThicknessTilesF(thicknessFunction.GetValue(tile2i.Vector2f).ToFix32() - self.Sqrt() * size.ApplyInverse(flowersConfig.SpawnPointDistanceFalloff));
              if (!(thickness < FlowersPostProcessor.FLOWERS_MIN_THICKNESS))
              {
                Tile2iIndex key = new Tile2iIndex(tileIndex2);
                newFloorThickness[key] = new TerrainMaterialThicknessSlim(slimId, thickness);
              }
            }
          }
        }
        ++y;
        tileIndex1 += dataReadOnly.Width;
      }
    }

    public void ApplyChunkChanges(
      Chunk2i chunk,
      Tile2i dataOrigin,
      ref TerrainManager.TerrainData dataRef,
      int pass)
    {
      Dict<Tile2iIndex, TerrainMaterialThicknessSlim> dict;
      if (pass == 0 || !this.m_newFloorThickness.TryRemove(chunk, out dict))
        return;
      foreach (KeyValuePair<Tile2iIndex, TerrainMaterialThicknessSlim> keyValuePair in dict)
        dataRef.AppendOrPushFirstLayer(keyValuePair.Key, keyValuePair.Value);
    }

    static FlowersPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      FlowersPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FlowersPostProcessor) obj).SerializeData(writer));
      FlowersPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FlowersPostProcessor) obj).DeserializeData(reader));
      FlowersPostProcessor.FLOWERS_MIN_THICKNESS = 0.1.TilesThick();
      FlowersPostProcessor.MAX_FLOWER_SPREAD_DISTANCE_SQUARED = 32.Squared();
    }

    [GenerateSerializer(false, null, 0)]
    public class Configuration : ITerrainFeatureConfigWithInit, ITerrainFeatureConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      [EditorEnforceOrder(60)]
      [DoNotSave(0, null)]
      public Action AddFlowerConfig;
      [EditorEnforceOrder(64)]
      [EditorCollection(true, true)]
      public readonly Lyst<FlowersPostProcessor.FlowersConfig> FlowersConfigs;

      public static void Serialize(FlowersPostProcessor.Configuration value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<FlowersPostProcessor.Configuration>(value))
          return;
        writer.EnqueueDataSerialization((object) value, FlowersPostProcessor.Configuration.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        Lyst<FlowersPostProcessor.FlowersConfig>.Serialize(this.FlowersConfigs, writer);
        RngSeed64.Serialize(this.Seed, writer);
        writer.WriteInt(this.SortingPriorityAdjustment);
      }

      public static FlowersPostProcessor.Configuration Deserialize(BlobReader reader)
      {
        FlowersPostProcessor.Configuration configuration;
        if (reader.TryStartClassDeserialization<FlowersPostProcessor.Configuration>(out configuration))
          reader.EnqueueDataDeserialization((object) configuration, FlowersPostProcessor.Configuration.s_deserializeDataDelayedAction);
        return configuration;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        reader.SetField<FlowersPostProcessor.Configuration>(this, "FlowersConfigs", (object) Lyst<FlowersPostProcessor.FlowersConfig>.Deserialize(reader));
        this.Seed = RngSeed64.Deserialize(reader);
        this.SortingPriorityAdjustment = reader.ReadInt();
      }

      [EditorEnforceOrder(52)]
      public RngSeed64 Seed { get; set; }

      [EditorEnforceOrder(56)]
      [EditorLabel(null, "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.", false, false)]
      public int SortingPriorityAdjustment { get; set; }

      public void InitializeInMapEditor(IResolver resolver)
      {
        ProtosDb protosDb = resolver.Resolve<ProtosDb>();
        this.AddFlowerConfig = (Action) (() => this.FlowersConfigs.Insert(0, FlowersPostProcessor.FlowersConfig.CreateDefaultConfig(protosDb)));
      }

      public Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CSeed\u003Ek__BackingField = MafiMath.GetRngSeedFromCurrentTime();
        this.FlowersConfigs = new Lyst<FlowersPostProcessor.FlowersConfig>();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Configuration()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        FlowersPostProcessor.Configuration.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FlowersPostProcessor.Configuration) obj).SerializeData(writer));
        FlowersPostProcessor.Configuration.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FlowersPostProcessor.Configuration) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public class FlowersConfig
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

      public static void Serialize(FlowersPostProcessor.FlowersConfig value, BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<FlowersPostProcessor.FlowersConfig>(value))
          return;
        writer.EnqueueDataSerialization((object) value, FlowersPostProcessor.FlowersConfig.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        writer.WriteGeneric<TerrainMaterialProto>(this.FlowerMaterial);
        RelTile1i.Serialize(this.MaxInfluenceDistance, writer);
        RelTile1f.Serialize(this.MinDistanceFromOthers, writer);
        RngSeed64.Serialize(this.Seed, writer);
        Percent.Serialize(this.SizeRandomnessMax, writer);
        Percent.Serialize(this.SizeRandomnessMin, writer);
        writer.WriteGeneric<TerrainMaterialProto>(this.SpawnMaterial);
        ThicknessTilesF.Serialize(this.SpawnMaterialMaxDepth, writer);
        ThicknessTilesF.Serialize(this.SpawnMaterialMinThickness, writer);
        Fix32.Serialize(this.SpawnPointDistanceFalloff, writer);
        Percent.Serialize(this.SpawnProbability, writer);
        writer.WriteGeneric<INoise2dFactory>(this.ThicknessFn);
      }

      public static FlowersPostProcessor.FlowersConfig Deserialize(BlobReader reader)
      {
        FlowersPostProcessor.FlowersConfig flowersConfig;
        if (reader.TryStartClassDeserialization<FlowersPostProcessor.FlowersConfig>(out flowersConfig))
          reader.EnqueueDataDeserialization((object) flowersConfig, FlowersPostProcessor.FlowersConfig.s_deserializeDataDelayedAction);
        return flowersConfig;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.FlowerMaterial = reader.ReadGenericAs<TerrainMaterialProto>();
        this.MaxInfluenceDistance = RelTile1i.Deserialize(reader);
        this.MinDistanceFromOthers = RelTile1f.Deserialize(reader);
        this.Seed = RngSeed64.Deserialize(reader);
        this.SizeRandomnessMax = Percent.Deserialize(reader);
        this.SizeRandomnessMin = Percent.Deserialize(reader);
        this.SpawnMaterial = reader.ReadGenericAs<TerrainMaterialProto>();
        this.SpawnMaterialMaxDepth = ThicknessTilesF.Deserialize(reader);
        this.SpawnMaterialMinThickness = ThicknessTilesF.Deserialize(reader);
        this.SpawnPointDistanceFalloff = Fix32.Deserialize(reader);
        this.SpawnProbability = Percent.Deserialize(reader);
        this.ThicknessFn = reader.ReadGenericAs<INoise2dFactory>();
      }

      [EditorEnforceOrder(80)]
      public TerrainMaterialProto FlowerMaterial { get; set; }

      [EditorEnforceOrder(83)]
      public TerrainMaterialProto SpawnMaterial { get; set; }

      [EditorRange(0.0, 0.1)]
      [EditorEnforceOrder(87)]
      public Percent SpawnProbability { get; set; }

      [EditorEnforceOrder(91)]
      [EditorRange(0.0, 1.0)]
      public Fix32 SpawnPointDistanceFalloff { get; set; }

      [EditorEnforceOrder(95)]
      [EditorRange(0.1, 2.0)]
      public ThicknessTilesF SpawnMaterialMinThickness { get; set; }

      [EditorRange(0.0, 2.0)]
      [EditorEnforceOrder(101)]
      [EditorLabel(null, "Test the second layer for spawn material if not deeper than this value.  Set to 0 of only top layer should be considered for spawning.", false, false)]
      public ThicknessTilesF SpawnMaterialMaxDepth { get; set; }

      [EditorRange(0.1, 4.0)]
      [EditorEnforceOrder(105)]
      public Percent SizeRandomnessMin { get; set; }

      [EditorRange(0.1, 4.0)]
      [EditorEnforceOrder(109)]
      public Percent SizeRandomnessMax { get; set; }

      [EditorRange(0.0, 200.0)]
      [EditorEnforceOrder(113)]
      public RelTile1f MinDistanceFromOthers { get; set; }

      [EditorEnforceOrder(117)]
      [EditorRange(0.0, 50.0)]
      public RelTile1i MaxInfluenceDistance { get; set; }

      [EditorEnforceOrder(120)]
      public RngSeed64 Seed { get; set; }

      [EditorLabel(null, "2D noise function that defines thickness for flowers material", false, false)]
      [EditorEnforceOrder(125)]
      [EditorCollapseObject]
      public INoise2dFactory ThicknessFn { get; set; }

      public static FlowersPostProcessor.FlowersConfig CreateDefaultConfig(ProtosDb db)
      {
        return new FlowersPostProcessor.FlowersConfig()
        {
          FlowerMaterial = db.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.FlowersWhite),
          SpawnMaterial = db.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass),
          SpawnProbability = 0.05.Percent(),
          SpawnPointDistanceFalloff = 0.1.ToFix32(),
          SpawnMaterialMinThickness = 0.75.TilesThick(),
          SpawnMaterialMaxDepth = 0.25.TilesThick(),
          SizeRandomnessMin = 50.Percent(),
          SizeRandomnessMax = 200.Percent(),
          MinDistanceFromOthers = 100.0.Tiles(),
          MaxInfluenceDistance = 20.Tiles(),
          ThicknessFn = (INoise2dFactory) new TextConfigurableNoise2dFactory()
          {
            Configuration = "parameters\r\n\tflowersAmountParams : SimplexNoise2dParams\r\n\tturbulenceParams : NoiseTurbulenceParams\r\n\tseed : SimplexNoise2dSeed64\r\n\r\nreturn\r\n\tSimplexNoise2D(flowersAmountParams, seed)\r\n\t|> Turbulence(turbulenceParams, seed)",
            Parameters = {
              {
                "flowersAmountParams",
                (object) new SimplexNoise2dParams((Fix32) 1, (Fix32) 2, (Fix32) 10)
              },
              {
                "turbulenceParams",
                (object) new NoiseTurbulenceParams(3, 192.Percent(), 50.Percent())
              },
              {
                "seed",
                (object) MafiMath.GetRngSeedFromCurrentTime().ToNoiseSeed2d64()
              }
            }
          }
        };
      }

      public FlowersConfig()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: reference to a compiler-generated field
        this.\u003CSeed\u003Ek__BackingField = MafiMath.GetRngSeedFromCurrentTime();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static FlowersConfig()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        FlowersPostProcessor.FlowersConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FlowersPostProcessor.FlowersConfig) obj).SerializeData(writer));
        FlowersPostProcessor.FlowersConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FlowersPostProcessor.FlowersConfig) obj).DeserializeData(reader));
      }
    }
  }
}
