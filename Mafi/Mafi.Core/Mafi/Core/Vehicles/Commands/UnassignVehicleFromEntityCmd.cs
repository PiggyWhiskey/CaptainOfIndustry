// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.UnassignVehicleFromEntityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class UnassignVehicleFromEntityCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly DynamicEntityProto.ID VehicleId;
    public readonly EntityId EntityId;
    public readonly int Count;

    public static void Serialize(UnassignVehicleFromEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UnassignVehicleFromEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UnassignVehicleFromEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.Count);
      EntityId.Serialize(this.EntityId, writer);
      DynamicEntityProto.ID.Serialize(this.VehicleId, writer);
    }

    public static UnassignVehicleFromEntityCmd Deserialize(BlobReader reader)
    {
      UnassignVehicleFromEntityCmd vehicleFromEntityCmd;
      if (reader.TryStartClassDeserialization<UnassignVehicleFromEntityCmd>(out vehicleFromEntityCmd))
        reader.EnqueueDataDeserialization((object) vehicleFromEntityCmd, UnassignVehicleFromEntityCmd.s_deserializeDataDelayedAction);
      return vehicleFromEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<UnassignVehicleFromEntityCmd>(this, "Count", (object) reader.ReadInt());
      reader.SetField<UnassignVehicleFromEntityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<UnassignVehicleFromEntityCmd>(this, "VehicleId", (object) DynamicEntityProto.ID.Deserialize(reader));
    }

    public UnassignVehicleFromEntityCmd(
      DynamicEntityProto vehicleProto,
      IEntityAssignedWithVehicles entity,
      int count = 1)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicleProto.Id, entity.Id, count);
    }

    public UnassignVehicleFromEntityCmd(
      DynamicEntityProto.ID vehicleId,
      EntityId entityId,
      int count = 1)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(entityId.IsValid).IsTrue();
      this.VehicleId = vehicleId;
      this.EntityId = entityId;
      this.Count = count;
    }

    static UnassignVehicleFromEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnassignVehicleFromEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      UnassignVehicleFromEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
