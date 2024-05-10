// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.ScrapVehicleCmd
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
  public class ScrapVehicleCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;

    public static void Serialize(ScrapVehicleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ScrapVehicleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ScrapVehicleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static ScrapVehicleCmd Deserialize(BlobReader reader)
    {
      ScrapVehicleCmd scrapVehicleCmd;
      if (reader.TryStartClassDeserialization<ScrapVehicleCmd>(out scrapVehicleCmd))
        reader.EnqueueDataDeserialization((object) scrapVehicleCmd, ScrapVehicleCmd.s_deserializeDataDelayedAction);
      return scrapVehicleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ScrapVehicleCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public ScrapVehicleCmd(Vehicle vehicle)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicle.Id);
    }

    public ScrapVehicleCmd(EntityId vehicleId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleId = vehicleId;
    }

    static ScrapVehicleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ScrapVehicleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ScrapVehicleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
