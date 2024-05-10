// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.ReplaceVehicleCmd
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
  public class ReplaceVehicleCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;
    public readonly DynamicEntityProto.ID TargetProtoId;

    public static void Serialize(ReplaceVehicleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ReplaceVehicleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ReplaceVehicleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      DynamicEntityProto.ID.Serialize(this.TargetProtoId, writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static ReplaceVehicleCmd Deserialize(BlobReader reader)
    {
      ReplaceVehicleCmd replaceVehicleCmd;
      if (reader.TryStartClassDeserialization<ReplaceVehicleCmd>(out replaceVehicleCmd))
        reader.EnqueueDataDeserialization((object) replaceVehicleCmd, ReplaceVehicleCmd.s_deserializeDataDelayedAction);
      return replaceVehicleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ReplaceVehicleCmd>(this, "TargetProtoId", (object) DynamicEntityProto.ID.Deserialize(reader));
      reader.SetField<ReplaceVehicleCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public ReplaceVehicleCmd(EntityId vehicleId, DynamicEntityProto.ID targetProtoId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleId = vehicleId;
      this.TargetProtoId = targetProtoId;
    }

    static ReplaceVehicleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ReplaceVehicleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ReplaceVehicleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
