// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.FleetUnloadFuelCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.World
{
  [GenerateSerializer(false, null, 0)]
  public class FleetUnloadFuelCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(FleetUnloadFuelCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetUnloadFuelCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetUnloadFuelCmd.s_serializeDataDelayedAction);
    }

    public static FleetUnloadFuelCmd Deserialize(BlobReader reader)
    {
      FleetUnloadFuelCmd fleetUnloadFuelCmd;
      if (reader.TryStartClassDeserialization<FleetUnloadFuelCmd>(out fleetUnloadFuelCmd))
        reader.EnqueueDataDeserialization((object) fleetUnloadFuelCmd, FleetUnloadFuelCmd.s_deserializeDataDelayedAction);
      return fleetUnloadFuelCmd;
    }

    public FleetUnloadFuelCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FleetUnloadFuelCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetUnloadFuelCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FleetUnloadFuelCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
