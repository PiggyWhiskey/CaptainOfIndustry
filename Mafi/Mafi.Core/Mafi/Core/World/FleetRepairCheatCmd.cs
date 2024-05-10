// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.FleetRepairCheatCmd
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
  public class FleetRepairCheatCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(FleetRepairCheatCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetRepairCheatCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetRepairCheatCmd.s_serializeDataDelayedAction);
    }

    public static FleetRepairCheatCmd Deserialize(BlobReader reader)
    {
      FleetRepairCheatCmd fleetRepairCheatCmd;
      if (reader.TryStartClassDeserialization<FleetRepairCheatCmd>(out fleetRepairCheatCmd))
        reader.EnqueueDataDeserialization((object) fleetRepairCheatCmd, FleetRepairCheatCmd.s_deserializeDataDelayedAction);
      return fleetRepairCheatCmd;
    }

    public FleetRepairCheatCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FleetRepairCheatCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetRepairCheatCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FleetRepairCheatCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
