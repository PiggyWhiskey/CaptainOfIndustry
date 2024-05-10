// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.FlowersTerrainPostProcessor
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
using Mafi.Core.Utils;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Base.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public class FlowersTerrainPostProcessor : ITerrainPostProcessor
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF FLOWERS_MIN_THICKNESS;
    private readonly FlowersOnTerrainConfig m_config;
    [DoNotSaveCreateNewOnLoad(null, 0)]
    private readonly HashSet<Tile2iIndex> m_usedTilesTmp;
    [DoNotSaveCreateNewOnLoad("new Lyst<Tile2iAndIndex>(canOmitClearing: true)", 0)]
    private readonly Lyst<Tile2iAndIndex> m_tilesToProcess;

    public static void Serialize(FlowersTerrainPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FlowersTerrainPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FlowersTerrainPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      FlowersOnTerrainConfig.Serialize(this.m_config, writer);
    }

    public static FlowersTerrainPostProcessor Deserialize(BlobReader reader)
    {
      FlowersTerrainPostProcessor terrainPostProcessor;
      if (reader.TryStartClassDeserialization<FlowersTerrainPostProcessor>(out terrainPostProcessor))
        reader.EnqueueDataDeserialization((object) terrainPostProcessor, FlowersTerrainPostProcessor.s_deserializeDataDelayedAction);
      return terrainPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<FlowersTerrainPostProcessor>(this, "m_config", (object) FlowersOnTerrainConfig.Deserialize(reader));
      reader.SetField<FlowersTerrainPostProcessor>(this, "m_tilesToProcess", (object) new Lyst<Tile2iAndIndex>(true));
      reader.SetField<FlowersTerrainPostProcessor>(this, "m_usedTilesTmp", (object) new HashSet<Tile2iIndex>());
    }

    public FlowersTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      this.m_usedTilesTmp = new HashSet<Tile2iIndex>();
      this.m_tilesToProcess = new Lyst<Tile2iAndIndex>(true);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Percent percent1 = 0.5.Percent();
      Percent percent2 = 0.2.Percent();
      ThicknessTilesF thicknessTilesF = 0.5.TilesThick();
      int num1 = 3;
      int num2 = 5;
      int num3 = 3;
      int num4 = 20;
      RelTile1f relTile1f = 0.0.Tiles();
      Percent percent3 = 50.Percent();
      ImmutableArray<FlowersOnTerrainConfig.FlowersConfig> immutableArray = ImmutableArray.Create<FlowersOnTerrainConfig.FlowersConfig>(new FlowersOnTerrainConfig.FlowersConfig()
      {
        FlowerMaterialId = Ids.TerrainMaterials.FlowersWhite,
        SpawnMaterialId = Ids.TerrainMaterials.Grass,
        SpawnProbabilityBase = percent1,
        SpawnMaterialMinThickness = thicknessTilesF,
        MinDistanceFromOthers = relTile1f,
        KeepExpandingFromLatestProbab = percent3,
        CoreSizeMean = num1,
        CoreSizeStdDev = num2,
        AuxiliarySizeMean = num3,
        AuxiliarySizeStdDev = num4
      }, new FlowersOnTerrainConfig.FlowersConfig()
      {
        FlowerMaterialId = Ids.TerrainMaterials.FlowersYellowLush,
        SpawnMaterialId = Ids.TerrainMaterials.GrassLush,
        SpawnProbabilityBase = percent1 + percent2,
        SpawnMaterialMinThickness = thicknessTilesF,
        MinDistanceFromOthers = relTile1f,
        KeepExpandingFromLatestProbab = percent3,
        CoreSizeMean = num1,
        CoreSizeStdDev = num2,
        AuxiliarySizeMean = num3,
        AuxiliarySizeStdDev = num4
      }, new FlowersOnTerrainConfig.FlowersConfig()
      {
        FlowerMaterialId = Ids.TerrainMaterials.FlowersRed,
        SpawnMaterialId = Ids.TerrainMaterials.Grass,
        SpawnProbabilityBase = percent1,
        SpawnMaterialMinThickness = thicknessTilesF,
        MinDistanceFromOthers = relTile1f,
        KeepExpandingFromLatestProbab = percent3,
        CoreSizeMean = num1,
        CoreSizeStdDev = num2,
        AuxiliarySizeMean = num3,
        AuxiliarySizeStdDev = num4
      }, new FlowersOnTerrainConfig.FlowersConfig()
      {
        FlowerMaterialId = Ids.TerrainMaterials.FlowersRed,
        SpawnMaterialId = Ids.TerrainMaterials.DirtBare,
        SpawnProbabilityBase = 2 * percent1,
        SpawnMaterialMinThickness = 0.3.TilesThick(),
        MinDistanceFromOthers = RelTile1f.Zero,
        KeepExpandingFromLatestProbab = percent3,
        CoreSizeMean = num1,
        CoreSizeStdDev = num2,
        AuxiliarySizeMean = num3,
        AuxiliarySizeStdDev = num4
      }, new FlowersOnTerrainConfig.FlowersConfig()
      {
        FlowerMaterialId = Ids.TerrainMaterials.FlowersPurpleLush,
        SpawnMaterialId = Ids.TerrainMaterials.Grass,
        SpawnProbabilityBase = percent1 + percent2,
        SpawnMaterialMinThickness = thicknessTilesF,
        MinDistanceFromOthers = relTile1f,
        KeepExpandingFromLatestProbab = percent3,
        CoreSizeMean = num1,
        CoreSizeStdDev = num2,
        AuxiliarySizeMean = num3,
        AuxiliarySizeStdDev = num4
      });
      this.m_config = new FlowersOnTerrainConfig()
      {
        FlowersConfigs = immutableArray.Concat(immutableArray.Map<FlowersOnTerrainConfig.FlowersConfig>((Func<FlowersOnTerrainConfig.FlowersConfig, FlowersOnTerrainConfig.FlowersConfig>) (x => new FlowersOnTerrainConfig.FlowersConfig()
        {
          FlowerMaterialId = x.FlowerMaterialId,
          SpawnMaterialId = x.SpawnMaterialId,
          SpawnProbabilityBase = x.SpawnProbabilityBase / 20,
          SpawnMaterialMinThickness = 0.9.TilesThick(),
          MinDistanceFromOthers = 50.0.Tiles(),
          KeepExpandingFromLatestProbab = 80.Percent(),
          CoreSizeMean = 50,
          CoreSizeStdDev = 20,
          AuxiliarySizeMean = 100,
          AuxiliarySizeStdDev = 100
        })))
      };
    }

    public void PostProcessGeneratedIslandMap(
      IslandMap map,
      TerrainManager terrain,
      DependencyResolver resolver,
      bool gameIsBeingLoaded)
    {
      ProtosDb protosDb = resolver.Resolve<ProtosDb>();
      TerrainMaterialProto[] terrainMaterialProtoArray = this.m_config.FlowersConfigs.MapArray<TerrainMaterialProto>((Func<FlowersOnTerrainConfig.FlowersConfig, TerrainMaterialProto>) (x => protosDb.GetOrThrow<TerrainMaterialProto>(x.SpawnMaterialId)));
      bool[] canHaveFlowers = terrain.TerrainMaterials.MapArray<bool>((Func<TerrainMaterialProto, bool>) (x => x.IsFarmable && !x.IsForestFloor));
      ref TerrainManager.TerrainData local = ref terrain.GetMutableTerrainDataRef();
      ushort[] flags = local.Flags;
      XorRsr128PlusGenerator rsr128PlusGenerator = new XorRsr128PlusGenerator(RandomGeneratorType.SimOnly, (ulong) terrain.TerrainWidth * (ulong) terrain.TerrainHeight, (ulong) map.MapName.GetHashCode());
      Lyst<Tile2i>[] lystArray = this.m_config.FlowersConfigs.MapArray<Lyst<Tile2i>>((Func<FlowersOnTerrainConfig.FlowersConfig, Lyst<Tile2i>>) (_ => new Lyst<Tile2i>()));
      for (int index1 = 0; index1 < flags.Length; ++index1)
      {
        if (((int) flags[index1] & 5) == 0)
        {
          TileMaterialLayers materialLayer = local.MaterialLayers[index1];
          if (canHaveFlowers[materialLayer.First.SlimIdRaw])
          {
            int num = index1;
            ImmutableArray<FlowersOnTerrainConfig.FlowersConfig> flowersConfigs = this.m_config.FlowersConfigs;
            int length = flowersConfigs.Length;
            int index2 = num % length;
            TerrainMaterialProto terrainMaterialProto = terrainMaterialProtoArray[index2];
            flowersConfigs = this.m_config.FlowersConfigs;
            FlowersOnTerrainConfig.FlowersConfig config = flowersConfigs[index2];
            if ((!(materialLayer.First.SlimId != terrainMaterialProto.SlimId) && !(materialLayer.First.Thickness < config.SpawnMaterialMinThickness) || !(materialLayer.Second.SlimId != terrainMaterialProto.SlimId) && !(materialLayer.Second.Thickness < config.SpawnMaterialMinThickness)) && rsr128PlusGenerator.TestProbability(config.SpawnProbabilityBase))
            {
              Tile2iAndIndex tileAndIndex = terrain.ExtendTileCoord_Slow(new Tile2iIndex(index1));
              if (config.MinDistanceFromOthers.IsPositive)
              {
                if (!lystArray[index2].Any<Tile2i>((Predicate<Tile2i>) (x => x.DistanceSqrTo(tileAndIndex.TileCoord) < config.MinDistanceFromOthers.Squared)))
                  lystArray[index2].Add(tileAndIndex.TileCoord);
                else
                  continue;
              }
              this.spawnFlowersAt(tileAndIndex, config, terrain, protosDb, canHaveFlowers, (IRandom) rsr128PlusGenerator);
            }
          }
        }
      }
    }

    private void spawnFlowersAt(
      Tile2iAndIndex tileAndIndex,
      FlowersOnTerrainConfig.FlowersConfig config,
      TerrainManager terrainManager,
      ProtosDb protosDb,
      bool[] canHaveFlowers,
      IRandom rng)
    {
      this.m_usedTilesTmp.Clear();
      this.m_tilesToProcess.Clear();
      this.m_tilesToProcess.Add(tileAndIndex);
      this.m_usedTilesTmp.Add(tileAndIndex.Index);
      int num = rng.NextGaussian(Percent.FromInt(config.CoreSizeMean), Percent.FromInt(config.CoreSizeStdDev)).ToIntRounded().Max(1);
      TerrainMaterialProto spawnMat = protosDb.GetOrThrow<TerrainMaterialProto>(config.SpawnMaterialId);
      TerrainMaterialProto flowersMat = protosDb.GetOrThrow<TerrainMaterialProto>(config.FlowerMaterialId);
      ref TerrainManager.TerrainData local = ref terrainManager.GetMutableTerrainDataRef();
      for (int index1 = 0; index1 < num && this.m_tilesToProcess.IsNotEmpty; ++index1)
      {
        int index2 = rng.NextInt(this.m_tilesToProcess.Count);
        Tile2iAndIndex tile2iAndIndex = this.m_tilesToProcess[index2];
        this.m_tilesToProcess.RemoveAtReplaceWithLast(index2);
        Percent probability = getSpawnPercent(tile2iAndIndex.Index, ref local).Max(50.Percent());
        if (!probability.IsNotPositive)
        {
          plantFlowers(tile2iAndIndex.Index, ref local, new ThicknessTilesF(probability.ToFix32()));
          tryExpandTo(tile2iAndIndex.PlusXNeighborUnchecked, probability);
          tryExpandTo(tile2iAndIndex.MinusXNeighborUnchecked, probability);
          tryExpandTo(tile2iAndIndex.PlusYNeighborUnchecked(terrainManager.TerrainWidth), probability);
          tryExpandTo(tile2iAndIndex.MinusYNeighborUnchecked(terrainManager.TerrainWidth), probability);
        }
      }
      int denominator = rng.NextGaussian(Percent.FromInt(config.AuxiliarySizeMean), Percent.FromInt(config.AuxiliarySizeStdDev)).ToIntRounded().Max(num);
      for (int index3 = 0; index3 < denominator && this.m_tilesToProcess.IsNotEmpty; ++index3)
      {
        int index4 = !rng.TestProbability(config.KeepExpandingFromLatestProbab) ? rng.NextInt(this.m_tilesToProcess.Count) : this.m_tilesToProcess.Count - 1;
        Tile2iAndIndex tile2iAndIndex = this.m_tilesToProcess[index4];
        this.m_tilesToProcess.RemoveAtReplaceWithLast(index4);
        Percent spawnPercent = getSpawnPercent(tile2iAndIndex.Index, ref local);
        if (!spawnPercent.IsNotPositive)
        {
          Percent percent = spawnPercent * Percent.FromRatio(denominator - index3, denominator);
          plantFlowers(tile2iAndIndex.Index, ref local, FlowersTerrainPostProcessor.FLOWERS_MIN_THICKNESS.Lerp(ThicknessTilesF.One, percent));
          tryExpandTo(tile2iAndIndex.PlusXNeighborUnchecked, percent);
          tryExpandTo(tile2iAndIndex.MinusXNeighborUnchecked, percent);
          tryExpandTo(tile2iAndIndex.PlusYNeighborUnchecked(terrainManager.TerrainWidth), percent);
          tryExpandTo(tile2iAndIndex.MinusYNeighborUnchecked(terrainManager.TerrainWidth), percent);
        }
      }
      this.m_usedTilesTmp.Clear();
      this.m_tilesToProcess.Clear();

      void plantFlowers(
        Tile2iIndex tileIndex,
        ref TerrainManager.TerrainData terrainDataRef,
        ThicknessTilesF newThickness)
      {
        ref TileMaterialLayers local = ref terrainDataRef.MaterialLayers[tileIndex.Value];
        if (local.First.SlimId == flowersMat.SlimId)
        {
          if (!(local.First.Thickness < ThicknessTilesF.One))
            return;
          local.First += newThickness;
        }
        else
          terrainDataRef.PushNewFirstLayer(ref local, new TerrainMaterialThicknessSlim(flowersMat, newThickness));
      }

      void tryExpandTo(Tile2iAndIndex t, Percent probability)
      {
        if (terrainManager.IsOceanOrOnMapBoundary(t.Index) || this.m_usedTilesTmp.Contains(t.Index) || !rng.TestProbability(probability))
          return;
        this.m_tilesToProcess.Add(t);
        this.m_usedTilesTmp.Add(t.Index);
      }

      Percent getSpawnPercent(Tile2iIndex i, ref TerrainManager.TerrainData terrainDataRef)
      {
        TileMaterialLayers materialLayer = terrainDataRef.MaterialLayers[i.Value];
        if (!canHaveFlowers[materialLayer.First.SlimIdRaw])
          return Percent.Zero;
        ThicknessTilesF thicknessTilesF;
        if (materialLayer.First.SlimId == spawnMat.SlimId)
        {
          thicknessTilesF = materialLayer.First.Thickness;
        }
        else
        {
          if (!(materialLayer.Second.SlimId == spawnMat.SlimId))
            return Percent.Zero;
          thicknessTilesF = ThicknessTilesF.One - materialLayer.First.Thickness;
        }
        return Percent.FromRatio(thicknessTilesF.Value, config.SpawnMaterialMinThickness.Value).Min(Percent.Hundred);
      }
    }

    static FlowersTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      FlowersTerrainPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((FlowersTerrainPostProcessor) obj).SerializeData(writer));
      FlowersTerrainPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((FlowersTerrainPostProcessor) obj).DeserializeData(reader));
      FlowersTerrainPostProcessor.FLOWERS_MIN_THICKNESS = 0.1.TilesThick();
    }
  }
}
