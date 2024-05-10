// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.AssignVehicleToEntityCmd
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
  /// <summary>Assigns a concrete vehicle to an entity.</summary>
  [GenerateSerializer(false, null, 0)]
  public class AssignVehicleToEntityCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;
    public readonly EntityId EntityId;

    public static void Serialize(AssignVehicleToEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AssignVehicleToEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AssignVehicleToEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static AssignVehicleToEntityCmd Deserialize(BlobReader reader)
    {
      AssignVehicleToEntityCmd vehicleToEntityCmd;
      if (reader.TryStartClassDeserialization<AssignVehicleToEntityCmd>(out vehicleToEntityCmd))
        reader.EnqueueDataDeserialization((object) vehicleToEntityCmd, AssignVehicleToEntityCmd.s_deserializeDataDelayedAction);
      return vehicleToEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AssignVehicleToEntityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<AssignVehicleToEntityCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public AssignVehicleToEntityCmd(Vehicle vehicle, IEntityAssignedWithVehicles entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicle.Id, entity.Id);
    }

    public AssignVehicleToEntityCmd(EntityId vehicleId, EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(vehicleId.IsValid).IsTrue();
      Assert.That<bool>(entityId.IsValid).IsTrue();
      this.VehicleId = vehicleId;
      this.EntityId = entityId;
    }

    static AssignVehicleToEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AssignVehicleToEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AssignVehicleToEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
