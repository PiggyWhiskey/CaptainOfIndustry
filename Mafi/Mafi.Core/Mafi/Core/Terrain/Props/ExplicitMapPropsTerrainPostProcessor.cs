// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Props.ExplicitMapPropsTerrainPostProcessor
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Map;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Props
{
  [GenerateSerializer(false, null, 0)]
  public class ExplicitMapPropsTerrainPostProcessor : ITerrainPostProcessor
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(ExplicitMapPropsTerrainPostProcessor value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ExplicitMapPropsTerrainPostProcessor>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ExplicitMapPropsTerrainPostProcessor.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
    }

    public static ExplicitMapPropsTerrainPostProcessor Deserialize(BlobReader reader)
    {
      ExplicitMapPropsTerrainPostProcessor terrainPostProcessor;
      if (reader.TryStartClassDeserialization<ExplicitMapPropsTerrainPostProcessor>(out terrainPostProcessor))
        reader.EnqueueDataDeserialization((object) terrainPostProcessor, ExplicitMapPropsTerrainPostProcessor.s_deserializeDataDelayedAction);
      return terrainPostProcessor;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
    }

    public void PostProcessGeneratedIslandMap(
      IslandMap map,
      TerrainManager terrain,
      DependencyResolver resolver,
      bool gameIsBeingLoaded)
    {
      ProtosDb protosDb = resolver.Resolve<ProtosDb>();
      TerrainPropsManager terrainPropsManager1 = resolver.Resolve<TerrainPropsManager>();
      TerrainManager terrainManager = resolver.Resolve<TerrainManager>();
      foreach (TerrainPropMapData terrainProp in map.TerrainProps)
      {
        TerrainPropProto proto1;
        if (!protosDb.TryGetProto<TerrainPropProto>(terrainProp.ProtoId, out proto1))
        {
          Log.WarningOnce(string.Format("Failed to find prop proto '{0}'", (object) terrainProp.ProtoId));
        }
        else
        {
          int variantIndex = terrainProp.VariantIndex;
          ImmutableArray<TerrainPropData.PropVariant> variants = proto1.Variants;
          int length = variants.Length;
          if ((uint) variantIndex >= (uint) length)
          {
            string str1 = string.Format("Invalid prop variant index {0}, ", (object) terrainProp.VariantIndex);
            variants = proto1.Variants;
            string str2 = string.Format(" variants count {0}, prop ID '{1}'", (object) variants.Length, (object) terrainProp.ProtoId);
            Log.WarningOnce(str1 + str2);
          }
          else
          {
            TerrainPropsManager terrainPropsManager2 = terrainPropsManager1;
            TerrainPropProto proto2 = proto1;
            variants = proto1.Variants;
            TerrainPropData.PropVariant variant = variants[terrainProp.VariantIndex];
            Tile2f position = terrainProp.Position;
            HeightTilesF height = terrainManager.GetHeight(terrainProp.Position);
            Percent scale = terrainProp.Scale;
            AngleSlim rotationYaw = terrainProp.RotationYaw;
            AngleSlim rotationPitch = terrainProp.RotationPitch;
            AngleSlim rotationRoll = terrainProp.RotationRoll;
            ThicknessTilesF heightOffset = terrainProp.HeightOffset;
            TerrainPropData data = new TerrainPropData(proto2, variant, position, height, scale, rotationYaw, rotationPitch, rotationRoll, heightOffset);
            terrainPropsManager2.TryAddProp(data);
          }
        }
      }
    }

    public ExplicitMapPropsTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static ExplicitMapPropsTerrainPostProcessor()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ExplicitMapPropsTerrainPostProcessor.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ExplicitMapPropsTerrainPostProcessor) obj).SerializeData(writer));
      ExplicitMapPropsTerrainPostProcessor.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ExplicitMapPropsTerrainPostProcessor) obj).DeserializeData(reader));
    }
  }
}
