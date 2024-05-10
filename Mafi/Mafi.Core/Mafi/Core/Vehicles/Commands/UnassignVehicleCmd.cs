// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.UnassignVehicleCmd
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
  public class UnassignVehicleCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;

    public static void Serialize(UnassignVehicleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<UnassignVehicleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, UnassignVehicleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static UnassignVehicleCmd Deserialize(BlobReader reader)
    {
      UnassignVehicleCmd unassignVehicleCmd;
      if (reader.TryStartClassDeserialization<UnassignVehicleCmd>(out unassignVehicleCmd))
        reader.EnqueueDataDeserialization((object) unassignVehicleCmd, UnassignVehicleCmd.s_deserializeDataDelayedAction);
      return unassignVehicleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<UnassignVehicleCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public UnassignVehicleCmd(DynamicGroundEntity vehicle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicle.Id);
    }

    public UnassignVehicleCmd(EntityId vehicleId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(vehicleId.IsValid).IsTrue();
      this.VehicleId = vehicleId;
    }

    static UnassignVehicleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      UnassignVehicleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      UnassignVehicleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
