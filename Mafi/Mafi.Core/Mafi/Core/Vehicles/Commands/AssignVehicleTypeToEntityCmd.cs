// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.AssignVehicleTypeToEntityCmd
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
  /// <summary>Assigns a vehicle type to an entity.</summary>
  [GenerateSerializer(false, null, 0)]
  public class AssignVehicleTypeToEntityCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly DynamicEntityProto.ID VehicleId;
    public readonly EntityId EntityId;
    public readonly int Count;

    public static void Serialize(AssignVehicleTypeToEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AssignVehicleTypeToEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AssignVehicleTypeToEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.Count);
      EntityId.Serialize(this.EntityId, writer);
      DynamicEntityProto.ID.Serialize(this.VehicleId, writer);
    }

    public static AssignVehicleTypeToEntityCmd Deserialize(BlobReader reader)
    {
      AssignVehicleTypeToEntityCmd vehicleTypeToEntityCmd;
      if (reader.TryStartClassDeserialization<AssignVehicleTypeToEntityCmd>(out vehicleTypeToEntityCmd))
        reader.EnqueueDataDeserialization((object) vehicleTypeToEntityCmd, AssignVehicleTypeToEntityCmd.s_deserializeDataDelayedAction);
      return vehicleTypeToEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AssignVehicleTypeToEntityCmd>(this, "Count", (object) reader.ReadInt());
      reader.SetField<AssignVehicleTypeToEntityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<AssignVehicleTypeToEntityCmd>(this, "VehicleId", (object) DynamicEntityProto.ID.Deserialize(reader));
    }

    public AssignVehicleTypeToEntityCmd(
      DynamicEntityProto vehicleProto,
      IEntityAssignedWithVehicles entity,
      int count = 1)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicleProto.Id, entity.Id, count);
    }

    public AssignVehicleTypeToEntityCmd(
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

    static AssignVehicleTypeToEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssignVehicleTypeToEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AssignVehicleTypeToEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
