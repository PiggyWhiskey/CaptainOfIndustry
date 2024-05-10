// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.TestMapGeneratorConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Trees;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public class TestMapGeneratorConfig : IConfig
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Proto.ID BedrockMaterialId { get; set; }

    public ImmutableArray<MapCellSurfaceGeneratorProto.ID> CellSurfaceIds { get; set; }

    public Proto.ID CliffProtoId { get; set; }

    public HeightTilesI OceanHeight { get; set; }

    public HeightTilesI LandHeight { get; set; }

    public MapCellSurfaceGeneratorProto.ID? ExtraTopCellsSurfaceId { get; set; }

    public HeightTilesI ExtraTopCellsHeight { get; set; }

    public Proto.ID? PrimaryForestProtoId { get; set; }

    public Proto.ID? SecondaryForestProtoId { get; set; }

    public ImmutableArray<TestMapResource> MineableResources { get; set; }

    public ImmutableArray<TestMapResource> MineableResourcesOnCliff { get; set; }

    public ImmutableArray<ITerrainPostProcessor> TerrainPostProcessors { get; set; }

    public static void Serialize(TestMapGeneratorConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TestMapGeneratorConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TestMapGeneratorConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Proto.ID.Serialize(this.BedrockMaterialId, writer);
      ImmutableArray<MapCellSurfaceGeneratorProto.ID>.Serialize(this.CellSurfaceIds, writer);
      Proto.ID.Serialize(this.CliffProtoId, writer);
      HeightTilesI.Serialize(this.ExtraTopCellsHeight, writer);
      writer.WriteNullableStruct<MapCellSurfaceGeneratorProto.ID>(this.ExtraTopCellsSurfaceId);
      HeightTilesI.Serialize(this.LandHeight, writer);
      ImmutableArray<TestMapResource>.Serialize(this.MineableResources, writer);
      ImmutableArray<TestMapResource>.Serialize(this.MineableResourcesOnCliff, writer);
      HeightTilesI.Serialize(this.OceanHeight, writer);
      writer.WriteNullableStruct<Proto.ID>(this.PrimaryForestProtoId);
      writer.WriteNullableStruct<Proto.ID>(this.SecondaryForestProtoId);
      ImmutableArray<ITerrainPostProcessor>.Serialize(this.TerrainPostProcessors, writer);
    }

    public static TestMapGeneratorConfig Deserialize(BlobReader reader)
    {
      TestMapGeneratorConfig mapGeneratorConfig;
      if (reader.TryStartClassDeserialization<TestMapGeneratorConfig>(out mapGeneratorConfig))
        reader.EnqueueDataDeserialization((object) mapGeneratorConfig, TestMapGeneratorConfig.s_deserializeDataDelayedAction);
      return mapGeneratorConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.BedrockMaterialId = Proto.ID.Deserialize(reader);
      this.CellSurfaceIds = ImmutableArray<MapCellSurfaceGeneratorProto.ID>.Deserialize(reader);
      this.CliffProtoId = Proto.ID.Deserialize(reader);
      this.ExtraTopCellsHeight = HeightTilesI.Deserialize(reader);
      this.ExtraTopCellsSurfaceId = reader.ReadNullableStruct<MapCellSurfaceGeneratorProto.ID>();
      this.LandHeight = HeightTilesI.Deserialize(reader);
      this.MineableResources = ImmutableArray<TestMapResource>.Deserialize(reader);
      this.MineableResourcesOnCliff = ImmutableArray<TestMapResource>.Deserialize(reader);
      this.OceanHeight = HeightTilesI.Deserialize(reader);
      this.PrimaryForestProtoId = reader.ReadNullableStruct<Proto.ID>();
      this.SecondaryForestProtoId = reader.ReadNullableStruct<Proto.ID>();
      this.TerrainPostProcessors = ImmutableArray<ITerrainPostProcessor>.Deserialize(reader);
    }

    public TestMapGeneratorConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBedrockMaterialId\u003Ek__BackingField = IdsCore.TerrainMaterials.Bedrock;
      // ISSUE: reference to a compiler-generated field
      this.\u003COceanHeight\u003Ek__BackingField = MapCell.DEFAULT_OCEAN_FLOOR_HEIGHT;
      // ISSUE: reference to a compiler-generated field
      this.\u003CLandHeight\u003Ek__BackingField = new HeightTilesI(2);
      // ISSUE: reference to a compiler-generated field
      this.\u003CExtraTopCellsHeight\u003Ek__BackingField = new HeightTilesI(12);
      // ISSUE: reference to a compiler-generated field
      this.\u003CMineableResources\u003Ek__BackingField = ImmutableArray<TestMapResource>.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CMineableResourcesOnCliff\u003Ek__BackingField = ImmutableArray<TestMapResource>.Empty;
      // ISSUE: reference to a compiler-generated field
      this.\u003CTerrainPostProcessors\u003Ek__BackingField = ImmutableArray.Create<ITerrainPostProcessor>((ITerrainPostProcessor) new ForestFloorTerrainPostProcessor());
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TestMapGeneratorConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TestMapGeneratorConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((TestMapGeneratorConfig) obj).SerializeData(writer));
      TestMapGeneratorConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((TestMapGeneratorConfig) obj).DeserializeData(reader));
    }
  }
}
