// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadEntranceEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Roads
{
  [GenerateSerializer(false, null, 0)]
  public class RoadEntranceEntity : 
    RoadEntityBase,
    IRoadGraphTerrainConnector,
    IRoadGraphEntity,
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public override bool CanBePaused => false;

    public int RoadTerrainConnectionsCount => this.Prototype.TerrainConnections.Length;

    public RoadEntranceEntityProto Prototype { get; private set; }

    public RoadEntranceEntity(
      EntityId id,
      RoadEntranceEntityProto proto,
      TileTransform transform,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (RoadEntityProtoBase) proto, transform, context);
      this.Prototype = proto;
    }

    public RoadTerrainConnection GetRoadTerrainConnection(int index)
    {
      LaneTerrainConnectionSpec terrainConnection = this.Prototype.TerrainConnections[index];
      RoadLaneMetadata roadLaneMetadata = this.RoadProto.LanesData[terrainConnection.LaneIndex];
      RoadGraphNodeKey roadGraphNode = terrainConnection.IsAtLaneStart ? this.RoadProto.GetTransformedRoadGraphNode(roadLaneMetadata.StartPosition, roadLaneMetadata.StartDirection, this.Transform) : this.RoadProto.GetTransformedRoadGraphNode(roadLaneMetadata.EndPosition, roadLaneMetadata.EndDirection, this.Transform);
      return new RoadTerrainConnection(this.Prototype.Layout.Transform(terrainConnection.LayoutTile, this.Transform), roadGraphNode, terrainConnection.IsAtLaneStart);
    }

    public static void Serialize(RoadEntranceEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RoadEntranceEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RoadEntranceEntity.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<RoadEntranceEntityProto>(this.Prototype);
    }

    public static RoadEntranceEntity Deserialize(BlobReader reader)
    {
      RoadEntranceEntity roadEntranceEntity;
      if (reader.TryStartClassDeserialization<RoadEntranceEntity>(out roadEntranceEntity))
        reader.EnqueueDataDeserialization((object) roadEntranceEntity, RoadEntranceEntity.s_deserializeDataDelayedAction);
      return roadEntranceEntity;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Prototype = reader.ReadGenericAs<RoadEntranceEntityProto>();
    }

    static RoadEntranceEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RoadEntranceEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      RoadEntranceEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
