// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.TerrainTexturedPorpsTerrainPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Base.Terrain
{
  [GenerateSerializer(false, null, 0)]
  [OnlyForSaveCompatibility(null)]
  public class TerrainTexturedPorpsTerrainPostProcessor : ITerrainPostProcessor
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MIN_LAYER_THICKNESS;
    private static readonly ThicknessTilesF MAX_EXPLORE_DEPTH;
    private readonly TerrainTexturedPorpsTerrainPostProcessor.Config m_config;

    public static void Serialize(TerrainTexturedPorpsTerrainPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TerrainTexturedPorpsTerrainPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TerrainTexturedPorpsTerrainPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      TerrainTexturedPorpsTerrainPostProcessor.Config.Serialize(this.m_config, writer);
    }

    public static TerrainTexturedPorpsTerrainPostProcessor Deserialize(BlobReader reader)
    {
      TerrainTexturedPorpsTerrainPostProcessor terrainPostProcessor;
      if (reader.TryStartClassDeserialization<TerrainTexturedPorpsTerrainPostProcessor>(out terrainPostProcessor))
        reader.EnqueueDataDeserialization((object) terrainPostProcessor, TerrainTexturedPorpsTerrainPostProcessor.s_deserializeDataDelayedAction);
      return terrainPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<TerrainTexturedPorpsTerrainPostProcessor>(this, "m_config", (object) TerrainTexturedPorpsTerrainPostProcessor.Config.Deserialize(reader));
    }

    public TerrainTexturedPorpsTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      LystStruct<TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord> lystStruct = new LystStruct<TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord>();
      lystStruct.Add(new TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord()
      {
        MaterialIds = ImmutableArray.Create<Proto.ID>(Ids.TerrainMaterials.Grass, Ids.TerrainMaterials.GrassLush, Ids.TerrainMaterials.ForestFloor, Ids.TerrainMaterials.DirtBare),
        PropIds = ImmutableArray.Create<Proto.ID>(Ids.TerrainProps.Stone01, Ids.TerrainProps.Stone02, Ids.TerrainProps.Stone03),
        PropMaterialOverride = new Proto.ID?(Ids.TerrainMaterials.Rock),
        BelowPropMaterial = new Proto.ID?(Ids.TerrainMaterials.DirtBare),
        SpawnProbability = 0.02.Percent(),
        MinScale = 40.Percent(),
        MaxScale = 160.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        PlacementHeightOffset = -0.25.TilesThick()
      });
      lystStruct.Add(new TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord()
      {
        MaterialIds = ImmutableArray.Create<Proto.ID>(Ids.TerrainMaterials.Sand),
        PropIds = ImmutableArray.Create<Proto.ID>(Ids.TerrainProps.Stone01, Ids.TerrainProps.Stone02, Ids.TerrainProps.Stone03),
        PropMaterialOverride = new Proto.ID?(Ids.TerrainMaterials.Limestone),
        SpawnProbability = 0.1.Percent(),
        MinScale = 40.Percent(),
        MaxScale = 160.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        PlacementHeightOffset = -0.25.TilesThick()
      });
      lystStruct.Add(new TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord()
      {
        MaterialIds = ImmutableArray.Create<Proto.ID>(Ids.TerrainMaterials.Coal),
        PropIds = ImmutableArray.Create<Proto.ID>(Ids.TerrainProps.StoneSharp01),
        SpawnProbability = 0.1.Percent(),
        MinScale = 40.Percent(),
        MaxScale = 160.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        PlacementHeightOffset = -0.25.TilesThick()
      });
      lystStruct.Add(new TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord()
      {
        MaterialIds = ImmutableArray.Create<Proto.ID>(Ids.TerrainMaterials.Rock, Ids.TerrainMaterials.IronOre, Ids.TerrainMaterials.CopperOre, Ids.TerrainMaterials.GoldOre, Ids.TerrainMaterials.Limestone),
        PropIds = ImmutableArray.Create<Proto.ID>(Ids.TerrainProps.Stone01, Ids.TerrainProps.Stone02, Ids.TerrainProps.Stone03),
        SpawnProbability = 0.5.Percent(),
        MinScale = 40.Percent(),
        MaxScale = 160.Percent(),
        MaxHeightDelta = 0.2.TilesThick(),
        PlacementHeightOffset = -0.25.TilesThick()
      });
      this.m_config = new TerrainTexturedPorpsTerrainPostProcessor.Config()
      {
        Records = lystStruct
      };
    }

    public void PostProcessGeneratedIslandMap(
      IslandMap map,
      TerrainManager terrain,
      DependencyResolver resolver,
      bool gameIsBeingLoaded)
    {
      ProtosDb db = resolver.Resolve<ProtosDb>();
      TerrainPropsManager propsManager = resolver.Resolve<TerrainPropsManager>();
      IEnumerable<Pair<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>> pairs = this.m_config.Records.AsEnumerable().SelectMany<TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord, Pair<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>>((Func<TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord, IEnumerable<Pair<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>>>) (x => x.MaterialIds.Select<Pair<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>>((Func<Proto.ID, Pair<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>>) (m =>
      {
        Option<TerrainMaterialProto> option = db.Get<TerrainMaterialProto>(m);
        TerrainMaterialProto valueOrNull1 = option.ValueOrNull;
        TerrainPropProto[] array = x.PropIds.Select<TerrainPropProto>((Func<Proto.ID, TerrainPropProto>) (p => db.Get<TerrainPropProto>(p).ValueOrNull)).Where<TerrainPropProto>((Func<TerrainPropProto, bool>) (p => (Proto) p != (Proto) null)).ToArray<TerrainPropProto>();
        long spawnSeed = (long) x.MaterialIds.Length.GetRngSeed() ^ (long) x.SpawnProbability.RawValue.GetRngSeed();
        Percent spawnProbability = x.SpawnProbability;
        Percent minScale = x.MinScale;
        Percent maxScale = x.MaxScale;
        ThicknessTilesF maxHeightDelta = x.MaxHeightDelta;
        ThicknessTilesF placementHeightOffset = x.PlacementHeightOffset;
        TerrainMaterialSlimId propMaterialOverride;
        if (!x.PropMaterialOverride.HasValue)
        {
          propMaterialOverride = new TerrainMaterialSlimId();
        }
        else
        {
          option = db.Get<TerrainMaterialProto>(x.PropMaterialOverride.Value);
          TerrainMaterialProto valueOrNull2 = option.ValueOrNull;
          // ISSUE: explicit non-virtual call
          propMaterialOverride = valueOrNull2 != null ? __nonvirtual (valueOrNull2.SlimId) : new TerrainMaterialSlimId();
        }
        TerrainMaterialSlimId terrainMaterialSlimId;
        if (!x.BelowPropMaterial.HasValue)
        {
          terrainMaterialSlimId = new TerrainMaterialSlimId();
        }
        else
        {
          option = db.Get<TerrainMaterialProto>(x.BelowPropMaterial.Value);
          TerrainMaterialProto valueOrNull3 = option.ValueOrNull;
          // ISSUE: explicit non-virtual call
          terrainMaterialSlimId = valueOrNull3 != null ? __nonvirtual (valueOrNull3.SlimId) : new TerrainMaterialSlimId();
        }
        TerrainMaterialSlimIdOption belowPropMaterial = (TerrainMaterialSlimIdOption) terrainMaterialSlimId;
        TerrainTexturedPorpsTerrainPostProcessor.RecordLookup second = new TerrainTexturedPorpsTerrainPostProcessor.RecordLookup(array, (ulong) spawnSeed, spawnProbability, minScale, maxScale, maxHeightDelta, placementHeightOffset, propMaterialOverride, belowPropMaterial);
        return Pair.Create<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>(valueOrNull1, second);
      })))).Where<Pair<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>>((Func<Pair<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>, bool>) (x => (Proto) x.First != (Proto) null && x.Second.Props.Length != 0));
      ImmutableArray<TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>[] immutableArrayArray = terrain.TerrainMaterials.MapArray<ImmutableArray<TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>>((Func<TerrainMaterialProto, ImmutableArray<TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>>) (_ => ImmutableArray<TerrainTexturedPorpsTerrainPostProcessor.RecordLookup>.Empty));
      foreach (Pair<TerrainMaterialProto, TerrainTexturedPorpsTerrainPostProcessor.RecordLookup> pair in pairs)
        immutableArrayArray[(int) pair.First.SlimId.Value] = immutableArrayArray[(int) pair.First.SlimId.Value].Add(pair.Second);
      XorRsr128PlusGenerator rng = new XorRsr128PlusGenerator(RandomGeneratorType.SimOnly, 1UL, 2UL);
      foreach (Tile2iAndIndex tilesSkipOffLimit in terrain.EnumerateAllTilesSkipOffLimits())
      {
        if (!terrain.IsOcean(tilesSkipOffLimit.Index))
        {
          TileMaterialLayers layersRawData = terrain.GetLayersRawData(tilesSkipOffLimit.Index);
          ImmutableArray<TerrainTexturedPorpsTerrainPostProcessor.RecordLookup> immutableArray1 = immutableArrayArray[layersRawData.First.SlimIdRaw];
          if (immutableArray1.IsNotEmpty && layersRawData.First.Thickness >= TerrainTexturedPorpsTerrainPostProcessor.MIN_LAYER_THICKNESS)
          {
            foreach (TerrainTexturedPorpsTerrainPostProcessor.RecordLookup record in immutableArray1)
              trySpawn(tilesSkipOffLimit, record, layersRawData.First.SlimId);
          }
          if (layersRawData.First.Thickness < TerrainTexturedPorpsTerrainPostProcessor.MAX_EXPLORE_DEPTH)
          {
            ImmutableArray<TerrainTexturedPorpsTerrainPostProcessor.RecordLookup> immutableArray2 = immutableArrayArray[layersRawData.Second.SlimIdRaw];
            if (immutableArray2.IsNotEmpty && layersRawData.Second.Thickness >= TerrainTexturedPorpsTerrainPostProcessor.MIN_LAYER_THICKNESS)
            {
              foreach (TerrainTexturedPorpsTerrainPostProcessor.RecordLookup record in immutableArray2)
                trySpawn(tilesSkipOffLimit, record, layersRawData.Second.SlimId);
            }
          }
        }
      }

      void trySpawn(
        Tile2iAndIndex tileAndIndex,
        TerrainTexturedPorpsTerrainPostProcessor.RecordLookup record,
        TerrainMaterialSlimId terrainMaterialSlimId)
      {
        rng.SeedFast(tileAndIndex.XyIndexPacked, record.SpawnSeed);
        if (!rng.TestProbability(record.SpawnProbability))
          return;
        HeightTilesF height1 = terrain.GetHeight(tileAndIndex.Index);
        HeightTilesF heightTilesF1 = height1;
        TerrainManager terrainManager1 = terrain;
        Tile2iIndex index1 = tileAndIndex.Index;
        Tile2iIndex xneighborUnchecked1 = index1.MinusXNeighborUnchecked;
        HeightTilesF height2 = terrainManager1.GetHeight(xneighborUnchecked1);
        ThicknessTilesF thicknessTilesF = heightTilesF1 - height2;
        HeightTilesF heightTilesF2 = height1;
        TerrainManager terrainManager2 = terrain;
        index1 = tileAndIndex.Index;
        Tile2iIndex xneighborUnchecked2 = index1.PlusXNeighborUnchecked;
        HeightTilesF height3 = terrainManager2.GetHeight(xneighborUnchecked2);
        ThicknessTilesF rhs1 = heightTilesF2 - height3;
        HeightTilesF heightTilesF3 = height1;
        TerrainManager terrainManager3 = terrain;
        index1 = tileAndIndex.Index;
        Tile2iIndex index2 = index1.MinusYNeighborUnchecked(terrain.TerrainWidth);
        HeightTilesF height4 = terrainManager3.GetHeight(index2);
        ThicknessTilesF rhs2 = heightTilesF3 - height4;
        HeightTilesF heightTilesF4 = height1;
        TerrainManager terrainManager4 = terrain;
        index1 = tileAndIndex.Index;
        Tile2iIndex index3 = index1.PlusYNeighborUnchecked(terrain.TerrainWidth);
        HeightTilesF height5 = terrainManager4.GetHeight(index3);
        ThicknessTilesF rhs3 = heightTilesF4 - height5;
        if (thicknessTilesF.Max(rhs1).Max(rhs2).Max(rhs3) > record.MaxHeightDelta)
          return;
        int index4 = rng.NextInt(record.Props.Length);
        TerrainPropProto prop = record.Props[index4];
        Tile2f position = tileAndIndex.TileCoord.CornerTile2f + rng.NextRelTile2f(-RelTile2f.Half, RelTile2f.Half);
        Percent percent = prop.BaseScale * record.MinScale.Lerp(record.MaxScale, rng.NextPercent());
        if (!propsManager.TryAddProp(new TerrainPropData(record.Props[index4], new TerrainPropData.PropVariant(record.PropMaterialOverride.HasValue ? record.PropMaterialOverride.Value : terrainMaterialSlimId, rng.NextFloat(), rng.NextFloat(), percent.ToFloat()), position, height1, percent, rng.NextAngleSlim(), rng.NextAngleSlim(), rng.NextAngleSlim(), record.PlacementHeightOffset)))
          return;
        TerrainMaterialSlimId slimId = record.BelowPropMaterial.HasValue ? record.BelowPropMaterial.Value : terrainMaterialSlimId;
        ref TerrainManager.TerrainData local = ref terrain.GetMutableTerrainDataRef();
        int intFloored = prop.Extents.MaxComponent().ScaledBy(percent).ToIntFloored();
        if (intFloored <= 0)
        {
          local.AppendOrPushFirstLayer(tileAndIndex.Index, slimId, ThicknessTilesF.Half);
        }
        else
        {
          Fix32 fix32_1 = (Fix32) (intFloored * intFloored);
          for (int index5 = -intFloored; index5 <= intFloored; ++index5)
          {
            Fix32 fix32_2 = (position.Y - (Fix32) ((int) tileAndIndex.Y + index5)).SquaredAsFix32();
            for (int index6 = -intFloored; index6 <= intFloored; ++index6)
            {
              Fix32 fix32_3 = (position.X - (Fix32) ((int) tileAndIndex.X + index6)).SquaredAsFix32() + fix32_2;
              if (!(fix32_3 > fix32_1))
              {
                Fix32 fix32_4 = Fix32.One - fix32_3.Sqrt() / intFloored;
                if (fix32_4 > TerrainTexturedPorpsTerrainPostProcessor.MIN_LAYER_THICKNESS.Value)
                  local.AppendOrPushFirstLayer(terrain.GetTileIndex((int) tileAndIndex.X + index6, (int) tileAndIndex.Y + index5), slimId, new ThicknessTilesF(fix32_4));
              }
            }
          }
        }
      }
    }

    static TerrainTexturedPorpsTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      TerrainTexturedPorpsTerrainPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainTexturedPorpsTerrainPostProcessor) obj).SerializeData(writer));
      TerrainTexturedPorpsTerrainPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainTexturedPorpsTerrainPostProcessor) obj).DeserializeData(reader));
      TerrainTexturedPorpsTerrainPostProcessor.MIN_LAYER_THICKNESS = 0.1.TilesThick();
      TerrainTexturedPorpsTerrainPostProcessor.MAX_EXPLORE_DEPTH = 3.0.TilesThick();
    }

    [GenerateSerializer(false, null, 0)]
    public class Config
    {
      private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
      private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
      public LystStruct<TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord> Records;

      public static void Serialize(
        TerrainTexturedPorpsTerrainPostProcessor.Config value,
        BlobWriter writer)
      {
        if (!writer.TryStartClassSerialization<TerrainTexturedPorpsTerrainPostProcessor.Config>(value))
          return;
        writer.EnqueueDataSerialization((object) value, TerrainTexturedPorpsTerrainPostProcessor.Config.s_serializeDataDelayedAction);
      }

      protected virtual void SerializeData(BlobWriter writer)
      {
        LystStruct<TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord>.Serialize(this.Records, writer);
      }

      public static TerrainTexturedPorpsTerrainPostProcessor.Config Deserialize(BlobReader reader)
      {
        TerrainTexturedPorpsTerrainPostProcessor.Config config;
        if (reader.TryStartClassDeserialization<TerrainTexturedPorpsTerrainPostProcessor.Config>(out config))
          reader.EnqueueDataDeserialization((object) config, TerrainTexturedPorpsTerrainPostProcessor.Config.s_deserializeDataDelayedAction);
        return config;
      }

      protected virtual void DeserializeData(BlobReader reader)
      {
        this.Records = LystStruct<TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord>.Deserialize(reader);
      }

      public Config()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }

      static Config()
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        TerrainTexturedPorpsTerrainPostProcessor.Config.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TerrainTexturedPorpsTerrainPostProcessor.Config) obj).SerializeData(writer));
        TerrainTexturedPorpsTerrainPostProcessor.Config.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TerrainTexturedPorpsTerrainPostProcessor.Config) obj).DeserializeData(reader));
      }
    }

    [GenerateSerializer(false, null, 0)]
    public struct ConfigRecord
    {
      public ImmutableArray<Proto.ID> MaterialIds;
      public ImmutableArray<Proto.ID> PropIds;
      public Proto.ID? PropMaterialOverride;
      public Proto.ID? BelowPropMaterial;
      public Percent SpawnProbability;
      public Percent MinScale;
      public Percent MaxScale;
      public ThicknessTilesF MaxHeightDelta;
      public ThicknessTilesF PlacementHeightOffset;

      public static void Serialize(
        TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord value,
        BlobWriter writer)
      {
        writer.WriteNullableStruct<Proto.ID>(value.BelowPropMaterial);
        ImmutableArray<Proto.ID>.Serialize(value.MaterialIds, writer);
        ThicknessTilesF.Serialize(value.MaxHeightDelta, writer);
        Percent.Serialize(value.MaxScale, writer);
        Percent.Serialize(value.MinScale, writer);
        ThicknessTilesF.Serialize(value.PlacementHeightOffset, writer);
        ImmutableArray<Proto.ID>.Serialize(value.PropIds, writer);
        writer.WriteNullableStruct<Proto.ID>(value.PropMaterialOverride);
        Percent.Serialize(value.SpawnProbability, writer);
      }

      public static TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord Deserialize(
        BlobReader reader)
      {
        return new TerrainTexturedPorpsTerrainPostProcessor.ConfigRecord()
        {
          BelowPropMaterial = reader.ReadNullableStruct<Proto.ID>(),
          MaterialIds = ImmutableArray<Proto.ID>.Deserialize(reader),
          MaxHeightDelta = ThicknessTilesF.Deserialize(reader),
          MaxScale = Percent.Deserialize(reader),
          MinScale = Percent.Deserialize(reader),
          PlacementHeightOffset = ThicknessTilesF.Deserialize(reader),
          PropIds = ImmutableArray<Proto.ID>.Deserialize(reader),
          PropMaterialOverride = reader.ReadNullableStruct<Proto.ID>(),
          SpawnProbability = Percent.Deserialize(reader)
        };
      }
    }

    public readonly struct RecordLookup
    {
      public readonly TerrainPropProto[] Props;
      public readonly ulong SpawnSeed;
      public readonly Percent SpawnProbability;
      public readonly Percent MinScale;
      public readonly Percent MaxScale;
      public readonly ThicknessTilesF MaxHeightDelta;
      public readonly ThicknessTilesF PlacementHeightOffset;
      public readonly TerrainMaterialSlimIdOption PropMaterialOverride;
      public readonly TerrainMaterialSlimIdOption BelowPropMaterial;

      public RecordLookup(
        TerrainPropProto[] props,
        ulong spawnSeed,
        Percent spawnProbability,
        Percent minScale,
        Percent maxScale,
        ThicknessTilesF maxHeightDelta,
        ThicknessTilesF placementHeightOffset,
        TerrainMaterialSlimId propMaterialOverride,
        TerrainMaterialSlimIdOption belowPropMaterial)
      {
        MBiHIp97M4MqqbtZOh.chMFXj727();
        this.Props = props;
        this.SpawnSeed = spawnSeed;
        this.SpawnProbability = spawnProbability;
        this.MinScale = minScale;
        this.MaxScale = maxScale;
        this.MaxHeightDelta = maxHeightDelta;
        this.PlacementHeightOffset = placementHeightOffset;
        this.PropMaterialOverride = (TerrainMaterialSlimIdOption) propMaterialOverride;
        this.BelowPropMaterial = belowPropMaterial;
      }
    }
  }
}
