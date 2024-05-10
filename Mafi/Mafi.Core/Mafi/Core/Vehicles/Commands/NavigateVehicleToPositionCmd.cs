// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.NavigateVehicleToPositionCmd
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
  public class NavigateVehicleToPositionCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;
    public readonly Tile2i Position;

    public static void Serialize(NavigateVehicleToPositionCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NavigateVehicleToPositionCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NavigateVehicleToPositionCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Tile2i.Serialize(this.Position, writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static NavigateVehicleToPositionCmd Deserialize(BlobReader reader)
    {
      NavigateVehicleToPositionCmd vehicleToPositionCmd;
      if (reader.TryStartClassDeserialization<NavigateVehicleToPositionCmd>(out vehicleToPositionCmd))
        reader.EnqueueDataDeserialization((object) vehicleToPositionCmd, NavigateVehicleToPositionCmd.s_deserializeDataDelayedAction);
      return vehicleToPositionCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<NavigateVehicleToPositionCmd>(this, "Position", (object) Tile2i.Deserialize(reader));
      reader.SetField<NavigateVehicleToPositionCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public NavigateVehicleToPositionCmd(Vehicle vehicle, Tile2i position)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicle.Id, position);
    }

    public NavigateVehicleToPositionCmd(EntityId vehicleId, Tile2i position)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleId = vehicleId;
      this.Position = position;
    }

    static NavigateVehicleToPositionCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NavigateVehicleToPositionCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      NavigateVehicleToPositionCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
