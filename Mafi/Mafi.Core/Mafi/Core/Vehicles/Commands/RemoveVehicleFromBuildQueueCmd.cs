// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.RemoveVehicleFromBuildQueueCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.VehicleDepots;
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
  public class RemoveVehicleFromBuildQueueCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly int Index;
    public readonly EntityId VehicleDepotId;

    public static void Serialize(RemoveVehicleFromBuildQueueCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<RemoveVehicleFromBuildQueueCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, RemoveVehicleFromBuildQueueCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.Index);
      EntityId.Serialize(this.VehicleDepotId, writer);
    }

    public static RemoveVehicleFromBuildQueueCmd Deserialize(BlobReader reader)
    {
      RemoveVehicleFromBuildQueueCmd fromBuildQueueCmd;
      if (reader.TryStartClassDeserialization<RemoveVehicleFromBuildQueueCmd>(out fromBuildQueueCmd))
        reader.EnqueueDataDeserialization((object) fromBuildQueueCmd, RemoveVehicleFromBuildQueueCmd.s_deserializeDataDelayedAction);
      return fromBuildQueueCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<RemoveVehicleFromBuildQueueCmd>(this, "Index", (object) reader.ReadInt());
      reader.SetField<RemoveVehicleFromBuildQueueCmd>(this, "VehicleDepotId", (object) EntityId.Deserialize(reader));
    }

    public RemoveVehicleFromBuildQueueCmd(int index, VehicleDepotBase vehicleDepot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(index, vehicleDepot.Id);
    }

    public RemoveVehicleFromBuildQueueCmd(int index, EntityId vehicleDepotId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Index = index;
      this.VehicleDepotId = vehicleDepotId;
    }

    static RemoveVehicleFromBuildQueueCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      RemoveVehicleFromBuildQueueCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      RemoveVehicleFromBuildQueueCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
