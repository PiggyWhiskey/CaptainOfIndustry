// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.NavigateVehicleToVehicleCmd
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
  public class NavigateVehicleToVehicleCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;
    public readonly EntityId GoalVehicleId;

    public static void Serialize(NavigateVehicleToVehicleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NavigateVehicleToVehicleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NavigateVehicleToVehicleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.GoalVehicleId, writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static NavigateVehicleToVehicleCmd Deserialize(BlobReader reader)
    {
      NavigateVehicleToVehicleCmd vehicleToVehicleCmd;
      if (reader.TryStartClassDeserialization<NavigateVehicleToVehicleCmd>(out vehicleToVehicleCmd))
        reader.EnqueueDataDeserialization((object) vehicleToVehicleCmd, NavigateVehicleToVehicleCmd.s_deserializeDataDelayedAction);
      return vehicleToVehicleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<NavigateVehicleToVehicleCmd>(this, "GoalVehicleId", (object) EntityId.Deserialize(reader));
      reader.SetField<NavigateVehicleToVehicleCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public NavigateVehicleToVehicleCmd(Vehicle vehicle, Vehicle goalVehicle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicle.Id, goalVehicle.Id);
    }

    public NavigateVehicleToVehicleCmd(EntityId vehicleId, EntityId goalVehicleId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleId = vehicleId;
      this.GoalVehicleId = goalVehicleId;
    }

    static NavigateVehicleToVehicleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NavigateVehicleToVehicleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      NavigateVehicleToVehicleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
