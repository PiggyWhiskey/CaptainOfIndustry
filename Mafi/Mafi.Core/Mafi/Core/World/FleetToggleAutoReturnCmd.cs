// Decompiled with JetBrains decompiler
// Type: Mafi.Core.World.FleetToggleAutoReturnCmd
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
  public class FleetToggleAutoReturnCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(FleetToggleAutoReturnCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<FleetToggleAutoReturnCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, FleetToggleAutoReturnCmd.s_serializeDataDelayedAction);
    }

    public static FleetToggleAutoReturnCmd Deserialize(BlobReader reader)
    {
      FleetToggleAutoReturnCmd toggleAutoReturnCmd;
      if (reader.TryStartClassDeserialization<FleetToggleAutoReturnCmd>(out toggleAutoReturnCmd))
        reader.EnqueueDataDeserialization((object) toggleAutoReturnCmd, FleetToggleAutoReturnCmd.s_deserializeDataDelayedAction);
      return toggleAutoReturnCmd;
    }

    public FleetToggleAutoReturnCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FleetToggleAutoReturnCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      FleetToggleAutoReturnCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      FleetToggleAutoReturnCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
