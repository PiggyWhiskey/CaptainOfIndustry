// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Roads.RoadEntity
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Roads
{
  [GenerateSerializer(false, null, 0)]
  public class RoadEntity : RoadEntityBase
  {
    public static readonly RelTile1f DISCRETIZATION_STEP;
    public const int ROAD_LAYOUT_HEIGHT = 3;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public RoadEntityProto Prototype { get; private set; }

    public override bool CanBePaused => false;

    public RoadEntity(
      EntityId id,
      RoadEntityProto proto,
      TileTransform transform,
      EntityContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, (RoadEntityProtoBase) proto, transform, context);
      this.Prototype = proto;
    }

    public static void Serialize(RoadEntity value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RoadEntity>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RoadEntity.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<RoadEntityProto>(this.Prototype);
    }

    public static RoadEntity Deserialize(BlobReader reader)
    {
      RoadEntity roadEntity;
      if (reader.TryStartClassDeserialization<RoadEntity>(out roadEntity))
        reader.EnqueueDataDeserialization((object) roadEntity, RoadEntity.s_deserializeDataDelayedAction);
      return roadEntity;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.Prototype = reader.ReadGenericAs<RoadEntityProto>();
    }

    static RoadEntity()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RoadEntity.DISCRETIZATION_STEP = 0.25.Tiles();
      RoadEntity.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((Entity) obj).SerializeData(writer));
      RoadEntity.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((Entity) obj).DeserializeData(reader));
    }
  }
}
