// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.CancelReplaceVehicleCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Vehicles.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class CancelReplaceVehicleCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly EntityId VehicleId;

    public static void Serialize(CancelReplaceVehicleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CancelReplaceVehicleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CancelReplaceVehicleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.VehicleId, writer);
    }

    public static CancelReplaceVehicleCmd Deserialize(BlobReader reader)
    {
      CancelReplaceVehicleCmd replaceVehicleCmd;
      if (reader.TryStartClassDeserialization<CancelReplaceVehicleCmd>(out replaceVehicleCmd))
        reader.EnqueueDataDeserialization((object) replaceVehicleCmd, CancelReplaceVehicleCmd.s_deserializeDataDelayedAction);
      return replaceVehicleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CancelReplaceVehicleCmd>(this, "VehicleId", (object) EntityId.Deserialize(reader));
    }

    public CancelReplaceVehicleCmd(EntityId vehicleId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.VehicleId = vehicleId;
    }

    static CancelReplaceVehicleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CancelReplaceVehicleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CancelReplaceVehicleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
