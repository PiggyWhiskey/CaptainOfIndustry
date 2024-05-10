// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.NavigateVehicleToStaticEntityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class NavigateVehicleToStaticEntityCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;
    public readonly EntityId StaticEntityId;

    public static void Serialize(NavigateVehicleToStaticEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NavigateVehicleToStaticEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NavigateVehicleToStaticEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.StaticEntityId, writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static NavigateVehicleToStaticEntityCmd Deserialize(BlobReader reader)
    {
      NavigateVehicleToStaticEntityCmd toStaticEntityCmd;
      if (reader.TryStartClassDeserialization<NavigateVehicleToStaticEntityCmd>(out toStaticEntityCmd))
        reader.EnqueueDataDeserialization((object) toStaticEntityCmd, NavigateVehicleToStaticEntityCmd.s_deserializeDataDelayedAction);
      return toStaticEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<NavigateVehicleToStaticEntityCmd>(this, "StaticEntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<NavigateVehicleToStaticEntityCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public NavigateVehicleToStaticEntityCmd(Vehicle vehicle, IStaticEntity staticEntity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(vehicle.Id, staticEntity.Id);
    }

    public NavigateVehicleToStaticEntityCmd(EntityId vehicleId, EntityId staticEntityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleId = vehicleId;
      this.StaticEntityId = staticEntityId;
    }

    static NavigateVehicleToStaticEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NavigateVehicleToStaticEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      NavigateVehicleToStaticEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
