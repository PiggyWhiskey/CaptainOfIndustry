// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.FleetLoadCrewCmd
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
  public class FleetLoadCrewCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(FleetLoadCrewCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetLoadCrewCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetLoadCrewCmd.s_serializeDataDelayedAction);
    }

    public static FleetLoadCrewCmd Deserialize(BlobReader reader)
    {
      FleetLoadCrewCmd fleetLoadCrewCmd;
      if (reader.TryStartClassDeserialization<FleetLoadCrewCmd>(out fleetLoadCrewCmd))
        reader.EnqueueDataDeserialization((object) fleetLoadCrewCmd, FleetLoadCrewCmd.s_deserializeDataDelayedAction);
      return fleetLoadCrewCmd;
    }

    public FleetLoadCrewCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FleetLoadCrewCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetLoadCrewCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FleetLoadCrewCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
