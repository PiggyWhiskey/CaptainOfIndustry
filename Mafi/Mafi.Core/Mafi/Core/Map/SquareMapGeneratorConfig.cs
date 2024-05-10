// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Map.SquareMapGeneratorConfig
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Game;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.Terrain.Generation;
using Mafi.Core.Terrain.Props;
using Mafi.Core.Terrain.Trees;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Map
{
  [GenerateSerializer(false, null, 0)]
  public class SquareMapGeneratorConfig : IConfig
  {
    public LystStruct<TerrainPropMapData> TerrainProps;
    public Lyst<ITerrainPostProcessor> TerrainPostProcessors;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public Proto.ID BedrockMaterialId { get; set; }

    public int TerrainWidth { get; set; }

    public int TerrainHeight { get; set; }

    /// <summary>
    /// Whether ocean map cells should be generated in the given direction.
    /// </summary>
    public Direction90 OceanAtDirection { get; set; }

    public int OceanSize { get; set; }

    public HeightTilesI GroundHeight { get; set; }

    public MapCellSurfaceGeneratorProto.ID SurfaceProtoId { get; set; }

    public Proto.ID? ForestProtoId { get; set; }

    public RectangleTerrainArea2i ForestArea { get; set; }

    public static void Serialize(SquareMapGeneratorConfig value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SquareMapGeneratorConfig>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SquareMapGeneratorConfig.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Proto.ID.Serialize(this.BedrockMaterialId, writer);
      RectangleTerrainArea2i.Serialize(this.ForestArea, writer);
      writer.WriteNullableStruct<Proto.ID>(this.ForestProtoId);
      HeightTilesI.Serialize(this.GroundHeight, writer);
      Direction90.Serialize(this.OceanAtDirection, writer);
      writer.WriteInt(this.OceanSize);
      MapCellSurfaceGeneratorProto.ID.Serialize(this.SurfaceProtoId, writer);
      writer.WriteInt(this.TerrainHeight);
      Lyst<ITerrainPostProcessor>.Serialize(this.TerrainPostProcessors, writer);
      LystStruct<TerrainPropMapData>.Serialize(this.TerrainProps, writer);
      writer.WriteInt(this.TerrainWidth);
    }

    public static SquareMapGeneratorConfig Deserialize(BlobReader reader)
    {
      SquareMapGeneratorConfig mapGeneratorConfig;
      if (reader.TryStartClassDeserialization<SquareMapGeneratorConfig>(out mapGeneratorConfig))
        reader.EnqueueDataDeserialization((object) mapGeneratorConfig, SquareMapGeneratorConfig.s_deserializeDataDelayedAction);
      return mapGeneratorConfig;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.BedrockMaterialId = Proto.ID.Deserialize(reader);
      this.ForestArea = RectangleTerrainArea2i.Deserialize(reader);
      this.ForestProtoId = reader.ReadNullableStruct<Proto.ID>();
      this.GroundHeight = HeightTilesI.Deserialize(reader);
      this.OceanAtDirection = Direction90.Deserialize(reader);
      this.OceanSize = reader.ReadInt();
      this.SurfaceProtoId = MapCellSurfaceGeneratorProto.ID.Deserialize(reader);
      this.TerrainHeight = reader.ReadInt();
      this.TerrainPostProcessors = Lyst<ITerrainPostProcessor>.Deserialize(reader);
      this.TerrainProps = LystStruct<TerrainPropMapData>.Deserialize(reader);
      this.TerrainWidth = reader.ReadInt();
    }

    public SquareMapGeneratorConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CBedrockMaterialId\u003Ek__BackingField = IdsCore.TerrainMaterials.Bedrock;
      // ISSUE: reference to a compiler-generated field
      this.\u003CTerrainWidth\u003Ek__BackingField = 256;
      // ISSUE: reference to a compiler-generated field
      this.\u003CTerrainHeight\u003Ek__BackingField = 256;
      // ISSUE: reference to a compiler-generated field
      this.\u003COceanAtDirection\u003Ek__BackingField = Direction90.MinusX;
      // ISSUE: reference to a compiler-generated field
      this.\u003CGroundHeight\u003Ek__BackingField = new HeightTilesI(2);
      // ISSUE: reference to a compiler-generated field
      this.\u003CForestArea\u003Ek__BackingField = new RectangleTerrainArea2i(new Tile2i(128, 128), new RelTile2i(64, 64));
      this.TerrainPostProcessors = new Lyst<ITerrainPostProcessor>()
      {
        (ITerrainPostProcessor) new ForestFloorTerrainPostProcessor(),
        (ITerrainPostProcessor) new ExplicitMapPropsTerrainPostProcessor()
      };
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static SquareMapGeneratorConfig()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SquareMapGeneratorConfig.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((SquareMapGeneratorConfig) obj).SerializeData(writer));
      SquareMapGeneratorConfig.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((SquareMapGeneratorConfig) obj).DeserializeData(reader));
    }
  }
}
