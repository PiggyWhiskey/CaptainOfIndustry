// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.WorldRegionMap
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Products;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  [MemberRemovedInSaveVersion("PlayerConfig", 149, typeof (WorldRegionMapPlayerConfig), 0, false)]
  [GenerateSerializer(false, null, 0)]
  public class WorldRegionMap : IWorldRegionMap
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public const int MAX_MAP_FILE_NAME_LENGTH = 30;
    public readonly WorldRegionMapBaseConfig BaseConfig;
    public readonly Lyst<ITerrainFeatureGenerator> TerrainFeatureGeneratorsList;
    public readonly Lyst<ITerrainPostProcessorV2> TerrainPostProcessorsList;
    public readonly Lyst<IVirtualTerrainResourceGenerator> VirtualTerrainResourceGeneratorList;
    public readonly Lyst<IStartingLocationV2> StartingLocationsList;

    public static void Serialize(WorldRegionMap value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<WorldRegionMap>(value))
        return;
      writer.EnqueueDataSerialization((object) value, WorldRegionMap.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      WorldRegionMapBaseConfig.Serialize(this.BaseConfig, writer);
      writer.WriteGeneric<TerrainMaterialProto>(this.BedrockMaterial);
      RelTile2i.Serialize(this.Size, writer);
      Lyst<IStartingLocationV2>.Serialize(this.StartingLocationsList, writer);
      Lyst<ITerrainFeatureGenerator>.Serialize(this.TerrainFeatureGeneratorsList, writer);
      Lyst<ITerrainPostProcessorV2>.Serialize(this.TerrainPostProcessorsList, writer);
      Lyst<IVirtualTerrainResourceGenerator>.Serialize(this.VirtualTerrainResourceGeneratorList, writer);
    }

    public static WorldRegionMap Deserialize(BlobReader reader)
    {
      WorldRegionMap worldRegionMap;
      if (reader.TryStartClassDeserialization<WorldRegionMap>(out worldRegionMap))
        reader.EnqueueDataDeserialization((object) worldRegionMap, WorldRegionMap.s_deserializeDataDelayedAction);
      return worldRegionMap;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<WorldRegionMap>(this, "BaseConfig", (object) WorldRegionMapBaseConfig.Deserialize(reader));
      this.BedrockMaterial = reader.ReadGenericAs<TerrainMaterialProto>();
      if (reader.LoadedSaveVersion < 149)
        WorldRegionMapPlayerConfig.Deserialize(reader);
      this.Size = RelTile2i.Deserialize(reader);
      reader.SetField<WorldRegionMap>(this, "StartingLocationsList", (object) Lyst<IStartingLocationV2>.Deserialize(reader));
      reader.SetField<WorldRegionMap>(this, "TerrainFeatureGeneratorsList", (object) Lyst<ITerrainFeatureGenerator>.Deserialize(reader));
      reader.SetField<WorldRegionMap>(this, "TerrainPostProcessorsList", (object) Lyst<ITerrainPostProcessorV2>.Deserialize(reader));
      reader.SetField<WorldRegionMap>(this, "VirtualTerrainResourceGeneratorList", (object) Lyst<IVirtualTerrainResourceGenerator>.Deserialize(reader));
    }

    public RelTile2i Size { get; private set; }

    public TerrainMaterialProto BedrockMaterial { get; set; }

    public MapEdgeType MapEdgeType => this.BaseConfig.MapEdgeType;

    public MapOffLimitsSize OffLimitsSize => this.BaseConfig.OffLimitsSize;

    public IIndexable<ITerrainFeatureGenerator> TerrainFeatureGenerators
    {
      get => (IIndexable<ITerrainFeatureGenerator>) this.TerrainFeatureGeneratorsList;
    }

    public IIndexable<ITerrainPostProcessorV2> TerrainPostProcessors
    {
      get => (IIndexable<ITerrainPostProcessorV2>) this.TerrainPostProcessorsList;
    }

    public IIndexable<IVirtualTerrainResourceGenerator> VirtualResourcesGenerators
    {
      get
      {
        return (IIndexable<IVirtualTerrainResourceGenerator>) this.VirtualTerrainResourceGeneratorList;
      }
    }

    public IIndexable<IStartingLocationV2> StartingLocations
    {
      get => (IIndexable<IStartingLocationV2>) this.StartingLocationsList;
    }

    public WorldRegionMap(RelTile2i size, TerrainMaterialProto bedrock)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.BaseConfig = new WorldRegionMapBaseConfig();
      this.TerrainFeatureGeneratorsList = new Lyst<ITerrainFeatureGenerator>();
      this.TerrainPostProcessorsList = new Lyst<ITerrainPostProcessorV2>();
      this.VirtualTerrainResourceGeneratorList = new Lyst<IVirtualTerrainResourceGenerator>();
      this.StartingLocationsList = new Lyst<IStartingLocationV2>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Size = size;
      this.BedrockMaterial = bedrock;
    }

    public void SetMapSize(RelTile2i size) => this.Size = size;

    public void AddFeature(ITerrainFeatureBase feature)
    {
      switch (feature)
      {
        case ITerrainFeatureGenerator featureGenerator:
          this.TerrainFeatureGeneratorsList.Add(featureGenerator);
          break;
        case ITerrainPostProcessorV2 terrainPostProcessorV2:
          this.TerrainPostProcessorsList.Add(terrainPostProcessorV2);
          break;
        case IVirtualTerrainResourceGenerator resourceGenerator:
          this.VirtualTerrainResourceGeneratorList.Add(resourceGenerator);
          break;
        case IStartingLocationV2 startingLocationV2:
          this.StartingLocationsList.Add(startingLocationV2);
          break;
        default:
          Log.Error("Failed to add terrain feature '" + feature.GetType().Name + "', unknown type.");
          break;
      }
    }

    public IEnumerable<ITerrainFeatureBase> EnumerateAllFeatures()
    {
      return ((IEnumerable<ITerrainFeatureBase>) this.TerrainFeatureGeneratorsList).AsEnumerable<ITerrainFeatureBase>().Concat<ITerrainFeatureBase>((IEnumerable<ITerrainFeatureBase>) this.TerrainPostProcessorsList).Concat<ITerrainFeatureBase>((IEnumerable<ITerrainFeatureBase>) this.VirtualTerrainResourceGeneratorList).Concat<ITerrainFeatureBase>((IEnumerable<ITerrainFeatureBase>) this.StartingLocationsList);
    }

    static WorldRegionMap()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      WorldRegionMap.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((WorldRegionMap) obj).SerializeData(writer));
      WorldRegionMap.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((WorldRegionMap) obj).DeserializeData(reader));
    }
  }
}
