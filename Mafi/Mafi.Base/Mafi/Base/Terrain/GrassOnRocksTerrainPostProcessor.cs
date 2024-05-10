// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.GrassOnRocksTerrainPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public class GrassOnRocksTerrainPostProcessor : ITerrainPostProcessor
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MIN_ELIGIBLE_THICKNESS;
    private static readonly ThicknessTilesF MIN_COVER;
    private static readonly Fix32 MAX_COVER_PERC;
    private static readonly ThicknessTilesF MAX_DELTA;

    public static void Serialize(GrassOnRocksTerrainPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GrassOnRocksTerrainPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GrassOnRocksTerrainPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
    }

    public static GrassOnRocksTerrainPostProcessor Deserialize(BlobReader reader)
    {
      GrassOnRocksTerrainPostProcessor terrainPostProcessor;
      if (reader.TryStartClassDeserialization<GrassOnRocksTerrainPostProcessor>(out terrainPostProcessor))
        reader.EnqueueDataDeserialization((object) terrainPostProcessor, GrassOnRocksTerrainPostProcessor.s_deserializeDataDelayedAction);
      return terrainPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
    }

    public GrassOnRocksTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    public void PostProcessGeneratedIslandMap(
      IslandMap map,
      TerrainManager terrain,
      DependencyResolver resolver,
      bool gameIsBeingLoaded)
    {
      ProtosDb protosDb = resolver.Resolve<ProtosDb>();
      TerrainMaterialProto orThrow1 = protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.Grass);
      TerrainMaterialProto orThrow2 = protosDb.GetOrThrow<TerrainMaterialProto>(Ids.TerrainMaterials.GrassLush);
      Fix32[] grassGrowthMult = terrain.TerrainMaterials.MapArray<Fix32>((Func<TerrainMaterialProto, Fix32>) (x => x.GrassGrowthOnTop.ToFix32()));
      Lyst<Pair<Tile2iIndex, Pair<TerrainMaterialThicknessSlim, TerrainMaterialSlimId>>> newLayers = new Lyst<Pair<Tile2iIndex, Pair<TerrainMaterialThicknessSlim, TerrainMaterialSlimId>>>(4096);
      ref TerrainManager.TerrainData local1 = ref terrain.GetMutableTerrainDataRef();
      foreach (Tile2iIndex indicesSkipOffLimit in terrain.EnumerateAllTileIndicesSkipOffLimits())
      {
        Tile2iIndex tileIndex = indicesSkipOffLimit;
        if (!terrain.HasAnyTileFlagSet(tileIndex, 4U))
        {
          TileMaterialLayers materialLayer = local1.MaterialLayers[tileIndex.Value];
          Fix32 fix32_1 = grassGrowthMult[materialLayer.First.SlimIdRaw];
          if (materialLayer.Count > 0)
          {
            ThicknessTilesF thicknessTilesF1;
            ThicknessTilesF thicknessTilesF2;
            if (fix32_1.IsNotPositive)
            {
              if (!(materialLayer.First.Thickness >= ThicknessTilesF.One) && materialLayer.First.ToFull(terrain).Material.IsFarmable)
              {
                Fix32 fix32_2 = grassGrowthMult[materialLayer.Second.SlimIdRaw];
                if (fix32_2.IsNotPositive && materialLayer.Count >= 2)
                {
                  ThicknessTilesF thicknessTilesF3 = materialLayer.First.Thickness + materialLayer.Second.Thickness;
                  if (!(thicknessTilesF3 >= ThicknessTilesF.One) && materialLayer.Second.ToFull(terrain).Material.IsFarmable)
                  {
                    Fix32 fix32_3 = grassGrowthMult[materialLayer.Third.SlimIdRaw];
                    thicknessTilesF1 = materialLayer.Third.Thickness;
                    ThicknessTilesF thicknessTilesF4 = thicknessTilesF1.Min(ThicknessTilesF.One) - thicknessTilesF3;
                    thicknessTilesF2 = fix32_3 * thicknessTilesF4;
                  }
                  else
                    continue;
                }
                else
                {
                  Fix32 fix32_4 = fix32_2;
                  thicknessTilesF1 = materialLayer.Second.Thickness;
                  ThicknessTilesF thicknessTilesF5 = thicknessTilesF1.Min(ThicknessTilesF.One) - materialLayer.First.Thickness;
                  thicknessTilesF2 = fix32_4 * thicknessTilesF5;
                }
              }
              else
                continue;
            }
            else
            {
              Fix32 fix32_5 = fix32_1;
              thicknessTilesF1 = materialLayer.First.Thickness;
              ThicknessTilesF thicknessTilesF6 = thicknessTilesF1.Min(ThicknessTilesF.One);
              thicknessTilesF2 = fix32_5 * thicknessTilesF6;
              if (materialLayer.First.Thickness < ThicknessTilesF.One)
              {
                ThicknessTilesF thicknessTilesF7 = thicknessTilesF2;
                Fix32 fix32_6 = grassGrowthMult[materialLayer.Second.SlimIdRaw];
                thicknessTilesF1 = materialLayer.Second.Thickness;
                ThicknessTilesF thicknessTilesF8 = thicknessTilesF1.Min(ThicknessTilesF.One - materialLayer.First.Thickness);
                ThicknessTilesF thicknessTilesF9 = fix32_6 * thicknessTilesF8;
                thicknessTilesF2 = thicknessTilesF7 + thicknessTilesF9;
              }
            }
            if (!(thicknessTilesF2 < GrassOnRocksTerrainPostProcessor.MIN_ELIGIBLE_THICKNESS))
            {
              int terrainWidth = terrain.TerrainWidth;
              int num = 6;
              TerrainMaterialSlimIdOption none = TerrainMaterialSlimIdOption.None;
              for (int deltaI1 = 1; deltaI1 <= 5; ++deltaI1)
              {
                int deltaI2 = terrainWidth * deltaI1;
                if (terrain.IsOcean(new Tile2iIndex(tileIndex.Value - deltaI1)) || terrain.IsOcean(new Tile2iIndex(tileIndex.Value + deltaI1)) || terrain.IsOcean(new Tile2iIndex(tileIndex.Value - deltaI2)) || terrain.IsOcean(new Tile2iIndex(tileIndex.Value + deltaI2)))
                {
                  num = deltaI1;
                  break;
                }
                if (isIncompatibleMaterial(-deltaI1, ref none) || isIncompatibleMaterial(deltaI1, ref none) || isIncompatibleMaterial(-deltaI2, ref none) || isIncompatibleMaterial(deltaI2, ref none))
                {
                  num = deltaI1;
                  break;
                }
              }
              ThicknessTilesF thicknessTilesF10 = thicknessTilesF2.Min(ThicknessTilesF.One);
              if (num <= 5)
                thicknessTilesF10 *= num.ToFix32() / 6;
              HeightTilesF height = terrain.GetHeight(tileIndex);
              ThicknessTilesF zero = ThicknessTilesF.Zero;
              foreach (Tile2iAndIndexRel eightNeighborsDelta in terrain.EightNeighborsDeltas)
              {
                thicknessTilesF1 = height - terrain.GetHeight(tileIndex + eightNeighborsDelta.IndexDelta);
                ThicknessTilesF abs = thicknessTilesF1.Abs;
                zero += abs;
              }
              Fix32 fix32_7 = (GrassOnRocksTerrainPostProcessor.MAX_DELTA.Value - zero.Value / terrain.EightNeighborsDeltas.Length) / GrassOnRocksTerrainPostProcessor.MAX_DELTA.Value;
              ThicknessTilesF thicknessTilesF11 = new ThicknessTilesF(fix32_7 * fix32_7 * GrassOnRocksTerrainPostProcessor.MAX_COVER_PERC);
              GrassOnRocksTerrainPostProcessor.processAt(tileIndex, thicknessTilesF11 * thicknessTilesF10.Value, orThrow1.SlimId, orThrow2.SlimId, newLayers);
              if (none.HasValue)
              {
                thicknessTilesF1 = ThicknessTilesF.One - thicknessTilesF10;
                ThicknessTilesF halfFast = thicknessTilesF1.HalfFast;
                GrassOnRocksTerrainPostProcessor.processAt(tileIndex, thicknessTilesF11 * halfFast.Value, none.Value, none.Value, newLayers);
              }
            }
          }
        }

        bool isIncompatibleMaterial(int deltaI, ref TerrainMaterialSlimIdOption incompatibleSlimId)
        {
          Tile2iIndex index = new Tile2iIndex(tileIndex.Value + deltaI);
          TerrainMaterialThicknessSlim slimOrNoneNoBedrock1 = terrain.GetFirstLayerSlimOrNoneNoBedrock(index);
          if (grassGrowthMult[slimOrNoneNoBedrock1.SlimIdRaw].IsNotPositive)
          {
            TerrainMaterialProto material = slimOrNoneNoBedrock1.ToFull(terrain).Material;
            if (incompatibleSlimId.IsNone && material.CanSpreadToNearbyMaterials)
              incompatibleSlimId = (TerrainMaterialSlimIdOption) slimOrNoneNoBedrock1.SlimId;
            return !material.IsFarmable;
          }
          if (slimOrNoneNoBedrock1.Thickness < ThicknessTilesF.One)
          {
            TerrainMaterialThicknessSlim slimOrNoneNoBedrock2 = terrain.GetSecondLayerSlimOrNoneNoBedrock(index);
            if (grassGrowthMult[slimOrNoneNoBedrock2.SlimIdRaw].IsNotPositive)
              return !slimOrNoneNoBedrock2.ToFull(terrain).Material.IsFarmable;
          }
          return false;
        }
      }
      foreach (Pair<Tile2iIndex, Pair<TerrainMaterialThicknessSlim, TerrainMaterialSlimId>> pair in newLayers)
      {
        ref TileMaterialLayers local2 = ref local1.MaterialLayers[pair.First.Value];
        if (local2.First.SlimId == pair.Second.First.SlimId || local2.First.SlimId == pair.Second.Second)
          local2.First += pair.Second.First.Thickness;
        local1.PushNewFirstLayer(ref local2, pair.Second.First);
      }
    }

    private static void processAt(
      Tile2iIndex tileIndex,
      ThicknessTilesF fillAmount,
      TerrainMaterialSlimId depositedMaterialSlimId,
      TerrainMaterialSlimId compatibleMaterialSlimId,
      Lyst<Pair<Tile2iIndex, Pair<TerrainMaterialThicknessSlim, TerrainMaterialSlimId>>> newLayers)
    {
      if (fillAmount < GrassOnRocksTerrainPostProcessor.MIN_COVER)
        return;
      newLayers.Add(Pair.Create<Tile2iIndex, Pair<TerrainMaterialThicknessSlim, TerrainMaterialSlimId>>(tileIndex, Pair.Create<TerrainMaterialThicknessSlim, TerrainMaterialSlimId>(new TerrainMaterialThicknessSlim(depositedMaterialSlimId, fillAmount), compatibleMaterialSlimId)));
    }

    static GrassOnRocksTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      GrassOnRocksTerrainPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GrassOnRocksTerrainPostProcessor) obj).SerializeData(writer));
      GrassOnRocksTerrainPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GrassOnRocksTerrainPostProcessor) obj).DeserializeData(reader));
      GrassOnRocksTerrainPostProcessor.MIN_ELIGIBLE_THICKNESS = 0.1.TilesThick();
      GrassOnRocksTerrainPostProcessor.MIN_COVER = 0.2.TilesThick();
      GrassOnRocksTerrainPostProcessor.MAX_COVER_PERC = 0.9.ToFix32();
      GrassOnRocksTerrainPostProcessor.MAX_DELTA = 2.0.TilesThick();
    }
  }
}
