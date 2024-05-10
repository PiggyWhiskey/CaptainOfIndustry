// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.MixedGrassTerrainPostProcessor
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core;
using Mafi.Core.Map;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Utils;
using Mafi.Random.Noise;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Base.Terrain
{
  [GenerateSerializer(false, null, 0)]
  public class MixedGrassTerrainPostProcessor : ITerrainPostProcessor
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private static readonly ThicknessTilesF MIN_THICKNESS;
    private readonly Proto.ID m_grassProtoId;
    private readonly Proto.ID m_lushGrassProtoId;
    private readonly Proto.ID m_dirtBareProtoId;
    private readonly SimplexNoise2dParams m_lushGrassNoiseParams;
    private readonly SimplexNoise2dParams m_dirtBareNoiseParams;
    private readonly Fix32 m_lushGrassBias;
    private readonly Fix32 m_lushGrassBareMult;
    private readonly Fix32 m_dirtBareBias;
    private readonly Fix32 m_dirtBareTransitionSharpness;
    private readonly Fix32 m_dirtBareStrength;
    private readonly ThicknessTilesF m_dirtBareMaxThickness;

    public static void Serialize(MixedGrassTerrainPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MixedGrassTerrainPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MixedGrassTerrainPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Fix32.Serialize(this.m_dirtBareBias, writer);
      ThicknessTilesF.Serialize(this.m_dirtBareMaxThickness, writer);
      SimplexNoise2dParams.Serialize(this.m_dirtBareNoiseParams, writer);
      Proto.ID.Serialize(this.m_dirtBareProtoId, writer);
      Fix32.Serialize(this.m_dirtBareStrength, writer);
      Fix32.Serialize(this.m_dirtBareTransitionSharpness, writer);
      Proto.ID.Serialize(this.m_grassProtoId, writer);
      Fix32.Serialize(this.m_lushGrassBareMult, writer);
      Fix32.Serialize(this.m_lushGrassBias, writer);
      SimplexNoise2dParams.Serialize(this.m_lushGrassNoiseParams, writer);
      Proto.ID.Serialize(this.m_lushGrassProtoId, writer);
    }

    public static MixedGrassTerrainPostProcessor Deserialize(BlobReader reader)
    {
      MixedGrassTerrainPostProcessor terrainPostProcessor;
      if (reader.TryStartClassDeserialization<MixedGrassTerrainPostProcessor>(out terrainPostProcessor))
        reader.EnqueueDataDeserialization((object) terrainPostProcessor, MixedGrassTerrainPostProcessor.s_deserializeDataDelayedAction);
      return terrainPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_dirtBareBias", (object) Fix32.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_dirtBareMaxThickness", (object) ThicknessTilesF.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_dirtBareNoiseParams", (object) SimplexNoise2dParams.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_dirtBareProtoId", (object) Proto.ID.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_dirtBareStrength", (object) Fix32.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_dirtBareTransitionSharpness", (object) Fix32.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_grassProtoId", (object) Proto.ID.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_lushGrassBareMult", (object) Fix32.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_lushGrassBias", (object) Fix32.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_lushGrassNoiseParams", (object) SimplexNoise2dParams.Deserialize(reader));
      reader.SetField<MixedGrassTerrainPostProcessor>(this, "m_lushGrassProtoId", (object) Proto.ID.Deserialize(reader));
    }

    public MixedGrassTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_grassProtoId = Ids.TerrainMaterials.Grass;
      this.m_lushGrassProtoId = Ids.TerrainMaterials.GrassLush;
      this.m_dirtBareProtoId = Ids.TerrainMaterials.DirtBare;
      this.m_lushGrassBias = -0.3.ToFix32();
      this.m_lushGrassBareMult = 0.6.ToFix32();
      this.m_dirtBareBias = -0.33.ToFix32();
      this.m_dirtBareStrength = (Fix32) 1;
      this.m_dirtBareTransitionSharpness = (Fix32) 8;
      this.m_dirtBareMaxThickness = 0.5.TilesThick();
      this.m_lushGrassNoiseParams = new SimplexNoise2dParams(Fix32.Zero, Fix32.One, (Fix32) 200);
      this.m_dirtBareNoiseParams = new SimplexNoise2dParams(Fix32.Zero, Fix32.One, (Fix32) 80);
    }

    public void PostProcessGeneratedIslandMap(
      IslandMap map,
      TerrainManager terrain,
      DependencyResolver resolver,
      bool gameIsBeingLoaded)
    {
      ProtosDb protosDb = resolver.Resolve<ProtosDb>();
      TerrainMaterialSlimId slimId1 = protosDb.GetOrThrow<TerrainMaterialProto>(this.m_grassProtoId).SlimId;
      TerrainMaterialSlimId slimId2 = protosDb.GetOrThrow<TerrainMaterialProto>(this.m_lushGrassProtoId).SlimId;
      TerrainMaterialSlimId dirtBareSlimId = protosDb.GetOrThrow<TerrainMaterialProto>(this.m_dirtBareProtoId).SlimId;
      IRandom random = (IRandom) new XorRsr128PlusGenerator(RandomGeneratorType.SimOnly, (ulong) (uint) terrain.TerrainWidth << 32 | (ulong) (uint) terrain.TerrainHeight, 3296516569256545UL);
      INoise2D noise2D = new SimplexNoise2D(random.NoiseSeed2dLegacy(), this.m_lushGrassNoiseParams).Turbulence(random.NoiseSeed2dLegacy(), new NoiseTurbulenceParams(3, 192.Percent(), 50.Percent()));
      INoise2D dirtBareNoise = new SimplexNoise2D(random.NoiseSeed2dLegacy(), this.m_dirtBareNoiseParams).Turbulence(random.NoiseSeed2dLegacy(), new NoiseTurbulenceParams(5, 192.Percent(), 50.Percent()));
      INoise2D dirtBareNoiseDetails = (INoise2D) new SimplexNoise2D(random.NoiseSeed2dLegacy(), new SimplexNoise2dParams(Fix32.One, Fix32.One, (Fix32) 5));
      ref TerrainManager.TerrainData local1 = ref terrain.GetMutableTerrainDataRef();
      foreach (Tile2iAndIndex enumerateAllTile in terrain.EnumerateAllTiles())
      {
        if (!terrain.IsOcean(enumerateAllTile.Index))
        {
          TerrainMaterialThicknessSlim slimOrNoneNoBedrock = terrain.GetFirstLayerSlimOrNoneNoBedrock(enumerateAllTile.Index);
          if (!(slimOrNoneNoBedrock.SlimId != slimId1))
          {
            Fix32 fix32_1 = noise2D.GetValue(enumerateAllTile.TileCoord.Vector2f).ToFix32() + this.m_lushGrassBias;
            Fix32 fix32_2 = slimOrNoneNoBedrock.Thickness.Value - Fix32.One;
            Fix32 mult = fix32_2.Clamp01();
            if (fix32_1 < -Fix32.Half)
            {
              spawnBareDirt(enumerateAllTile, mult, ref local1);
            }
            else
            {
              ThicknessTilesF thickness;
              ref ThicknessTilesF local2 = ref thickness;
              fix32_2 = fix32_1 + Fix32.Half;
              Fix32 fix32_3 = fix32_2.Clamp01();
              local2 = new ThicknessTilesF(fix32_3);
              if (thickness >= MixedGrassTerrainPostProcessor.MIN_THICKNESS)
              {
                ref TileMaterialLayers local3 = ref local1.MaterialLayers[enumerateAllTile.IndexRaw];
                if (local3.First.Thickness - MixedGrassTerrainPostProcessor.MIN_THICKNESS <= thickness)
                {
                  local1.ChangeFirstLayerTo(enumerateAllTile.Index, slimId2);
                }
                else
                {
                  local3.First -= thickness;
                  local1.PushNewFirstLayer(ref local3, new TerrainMaterialThicknessSlim(slimId2, thickness));
                }
              }
              Fix32 t = thickness.Value.Min(Fix32.One);
              spawnBareDirt(enumerateAllTile, Fix32.One.Lerp(this.m_lushGrassBareMult, t) * mult, ref local1);
            }
          }
        }
      }

      void spawnBareDirt(
        Tile2iAndIndex tileAndIndex,
        Fix32 mult,
        ref TerrainManager.TerrainData terrainDataRef)
      {
        Fix32 fix32 = (dirtBareNoise.GetValue(tileAndIndex.TileCoord.Vector2f).ToFix32() + this.m_dirtBareBias) * (this.m_dirtBareStrength * mult) * dirtBareNoiseDetails.GetValue(tileAndIndex.TileCoord.Vector2f).ToFix32();
        if (fix32 < MixedGrassTerrainPostProcessor.MIN_THICKNESS.Value)
          return;
        ThicknessTilesF thickness = new ThicknessTilesF(fix32).Min(this.m_dirtBareMaxThickness);
        ref TileMaterialLayers local = ref terrainDataRef.MaterialLayers[tileAndIndex.IndexRaw];
        if (local.First.Thickness - MixedGrassTerrainPostProcessor.MIN_THICKNESS <= thickness)
        {
          terrainDataRef.ChangeFirstLayerTo(tileAndIndex.Index, dirtBareSlimId);
        }
        else
        {
          local.First -= thickness;
          terrainDataRef.PushNewFirstLayer(ref local, new TerrainMaterialThicknessSlim(dirtBareSlimId, thickness));
        }
      }
    }

    static MixedGrassTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.chMFXj727();
      MixedGrassTerrainPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MixedGrassTerrainPostProcessor) obj).SerializeData(writer));
      MixedGrassTerrainPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MixedGrassTerrainPostProcessor) obj).DeserializeData(reader));
      MixedGrassTerrainPostProcessor.MIN_THICKNESS = 0.05.TilesThick();
    }
  }
}
