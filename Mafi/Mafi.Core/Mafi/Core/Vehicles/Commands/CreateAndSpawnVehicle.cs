// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.CreateAndSpawnVehicle
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class CreateAndSpawnVehicle : InputCommand<EntityId>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly DynamicEntityProto.ID ProtoId;
    public readonly Tile2f SpawnLocation;
    public readonly bool EnqueueEmptyJob;

    public static void Serialize(CreateAndSpawnVehicle value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CreateAndSpawnVehicle>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CreateAndSpawnVehicle.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.EnqueueEmptyJob);
      DynamicEntityProto.ID.Serialize(this.ProtoId, writer);
      Tile2f.Serialize(this.SpawnLocation, writer);
    }

    public static CreateAndSpawnVehicle Deserialize(BlobReader reader)
    {
      CreateAndSpawnVehicle createAndSpawnVehicle;
      if (reader.TryStartClassDeserialization<CreateAndSpawnVehicle>(out createAndSpawnVehicle))
        reader.EnqueueDataDeserialization((object) createAndSpawnVehicle, CreateAndSpawnVehicle.s_deserializeDataDelayedAction);
      return createAndSpawnVehicle;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CreateAndSpawnVehicle>(this, "EnqueueEmptyJob", (object) reader.ReadBool());
      reader.SetField<CreateAndSpawnVehicle>(this, "ProtoId", (object) DynamicEntityProto.ID.Deserialize(reader));
      reader.SetField<CreateAndSpawnVehicle>(this, "SpawnLocation", (object) Tile2f.Deserialize(reader));
    }

    public CreateAndSpawnVehicle(
      DynamicEntityProto.ID protoId,
      Tile2f spawnLocation,
      bool enqueueEmptyJob)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EnqueueEmptyJob = enqueueEmptyJob;
      this.ProtoId = protoId;
      this.SpawnLocation = spawnLocation;
    }

    static CreateAndSpawnVehicle()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CreateAndSpawnVehicle.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<EntityId>) obj).SerializeData(writer));
      CreateAndSpawnVehicle.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<EntityId>) obj).DeserializeData(reader));
    }
  }
}
