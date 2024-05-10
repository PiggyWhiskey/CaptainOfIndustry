// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.FuelStations.ToggleFuelStationTrucksAllowedToRefuelCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Buildings.FuelStations
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleFuelStationTrucksAllowedToRefuelCmd : InputCommand
  {
    public readonly EntityId FuelStationId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ToggleFuelStationTrucksAllowedToRefuelCmd(EntityId fuelStationId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.FuelStationId = fuelStationId;
    }

    public static void Serialize(ToggleFuelStationTrucksAllowedToRefuelCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleFuelStationTrucksAllowedToRefuelCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleFuelStationTrucksAllowedToRefuelCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.FuelStationId, writer);
    }

    public static ToggleFuelStationTrucksAllowedToRefuelCmd Deserialize(BlobReader reader)
    {
      ToggleFuelStationTrucksAllowedToRefuelCmd allowedToRefuelCmd;
      if (reader.TryStartClassDeserialization<ToggleFuelStationTrucksAllowedToRefuelCmd>(out allowedToRefuelCmd))
        reader.EnqueueDataDeserialization((object) allowedToRefuelCmd, ToggleFuelStationTrucksAllowedToRefuelCmd.s_deserializeDataDelayedAction);
      return allowedToRefuelCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleFuelStationTrucksAllowedToRefuelCmd>(this, "FuelStationId", (object) EntityId.Deserialize(reader));
    }

    static ToggleFuelStationTrucksAllowedToRefuelCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleFuelStationTrucksAllowedToRefuelCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleFuelStationTrucksAllowedToRefuelCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
