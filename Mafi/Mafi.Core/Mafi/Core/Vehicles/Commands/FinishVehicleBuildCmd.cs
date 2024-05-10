// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.FinishVehicleBuildCmd
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
  /// <summary>Cheat finish current vehicle in progress.</summary>
  [GenerateSerializer(false, null, 0)]
  public class FinishVehicleBuildCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleDepotId;

    public static void Serialize(FinishVehicleBuildCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FinishVehicleBuildCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FinishVehicleBuildCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.VehicleDepotId, writer);
    }

    public static FinishVehicleBuildCmd Deserialize(BlobReader reader)
    {
      FinishVehicleBuildCmd finishVehicleBuildCmd;
      if (reader.TryStartClassDeserialization<FinishVehicleBuildCmd>(out finishVehicleBuildCmd))
        reader.EnqueueDataDeserialization((object) finishVehicleBuildCmd, FinishVehicleBuildCmd.s_deserializeDataDelayedAction);
      return finishVehicleBuildCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<FinishVehicleBuildCmd>(this, "VehicleDepotId", (object) EntityId.Deserialize(reader));
    }

    public FinishVehicleBuildCmd(VehicleDepotBase vehicleDepot)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicleDepot.Id);
    }

    public FinishVehicleBuildCmd(EntityId vehicleDepotId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleDepotId = vehicleDepotId;
    }

    static FinishVehicleBuildCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FinishVehicleBuildCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FinishVehicleBuildCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
