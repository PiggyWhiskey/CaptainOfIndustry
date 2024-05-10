// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Commands.TogglePartialTrucksLoadCmd
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
  public class TogglePartialTrucksLoadCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(TogglePartialTrucksLoadCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<TogglePartialTrucksLoadCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, TogglePartialTrucksLoadCmd.s_serializeDataDelayedAction);
    }

    public static TogglePartialTrucksLoadCmd Deserialize(BlobReader reader)
    {
      TogglePartialTrucksLoadCmd partialTrucksLoadCmd;
      if (reader.TryStartClassDeserialization<TogglePartialTrucksLoadCmd>(out partialTrucksLoadCmd))
        reader.EnqueueDataDeserialization((object) partialTrucksLoadCmd, TogglePartialTrucksLoadCmd.s_deserializeDataDelayedAction);
      return partialTrucksLoadCmd;
    }

    public TogglePartialTrucksLoadCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TogglePartialTrucksLoadCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      TogglePartialTrucksLoadCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      TogglePartialTrucksLoadCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
