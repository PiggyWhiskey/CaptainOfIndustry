// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.AddVehicleToBuildQueueCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.VehicleDepots;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Commands
{
  /// <summary>
  /// Requests assembly of vehicle from the given depot depot.
  /// </summary>
  [GenerateSerializer(false, null, 0)]
  public class AddVehicleToBuildQueueCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly DynamicEntityProto.ID ProtoId;
    public readonly EntityId VehicleDepotId;
    public readonly int Count;

    public static void Serialize(AddVehicleToBuildQueueCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AddVehicleToBuildQueueCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AddVehicleToBuildQueueCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.Count);
      DynamicEntityProto.ID.Serialize(this.ProtoId, writer);
      EntityId.Serialize(this.VehicleDepotId, writer);
    }

    public static AddVehicleToBuildQueueCmd Deserialize(BlobReader reader)
    {
      AddVehicleToBuildQueueCmd vehicleToBuildQueueCmd;
      if (reader.TryStartClassDeserialization<AddVehicleToBuildQueueCmd>(out vehicleToBuildQueueCmd))
        reader.EnqueueDataDeserialization((object) vehicleToBuildQueueCmd, AddVehicleToBuildQueueCmd.s_deserializeDataDelayedAction);
      return vehicleToBuildQueueCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AddVehicleToBuildQueueCmd>(this, "Count", (object) reader.ReadInt());
      reader.SetField<AddVehicleToBuildQueueCmd>(this, "ProtoId", (object) DynamicEntityProto.ID.Deserialize(reader));
      reader.SetField<AddVehicleToBuildQueueCmd>(this, "VehicleDepotId", (object) EntityId.Deserialize(reader));
    }

    public AddVehicleToBuildQueueCmd(
      DrivingEntityProto proto,
      VehicleDepotBase vehicleDepot,
      int count = 1)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(proto.Id, vehicleDepot.Id, count);
    }

    public AddVehicleToBuildQueueCmd(
      DynamicEntityProto.ID protoId,
      EntityId vehicleDepotId,
      int count = 1)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = protoId;
      this.VehicleDepotId = vehicleDepotId;
      this.Count = count;
    }

    static AddVehicleToBuildQueueCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AddVehicleToBuildQueueCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AddVehicleToBuildQueueCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
