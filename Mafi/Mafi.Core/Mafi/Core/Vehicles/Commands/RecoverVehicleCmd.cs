// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.RecoverVehicleCmd
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
  public class RecoverVehicleCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;

    public static void Serialize(RecoverVehicleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RecoverVehicleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RecoverVehicleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static RecoverVehicleCmd Deserialize(BlobReader reader)
    {
      RecoverVehicleCmd recoverVehicleCmd;
      if (reader.TryStartClassDeserialization<RecoverVehicleCmd>(out recoverVehicleCmd))
        reader.EnqueueDataDeserialization((object) recoverVehicleCmd, RecoverVehicleCmd.s_deserializeDataDelayedAction);
      return recoverVehicleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RecoverVehicleCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public RecoverVehicleCmd(Vehicle vehicle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicle.Id);
    }

    public RecoverVehicleCmd(EntityId vehicleId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleId = vehicleId;
    }

    static RecoverVehicleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RecoverVehicleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      RecoverVehicleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
