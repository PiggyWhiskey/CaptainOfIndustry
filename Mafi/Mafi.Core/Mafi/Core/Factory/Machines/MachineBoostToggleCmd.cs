// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Machines.MachineBoostToggleCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Machines
{
  [GenerateSerializer(false, null, 0)]
  public class MachineBoostToggleCmd : InputCommand
  {
    public readonly EntityId MachineId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MachineBoostToggleCmd(EntityId machineId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.MachineId = machineId;
    }

    public static void Serialize(MachineBoostToggleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MachineBoostToggleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MachineBoostToggleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.MachineId, writer);
    }

    public static MachineBoostToggleCmd Deserialize(BlobReader reader)
    {
      MachineBoostToggleCmd machineBoostToggleCmd;
      if (reader.TryStartClassDeserialization<MachineBoostToggleCmd>(out machineBoostToggleCmd))
        reader.EnqueueDataDeserialization((object) machineBoostToggleCmd, MachineBoostToggleCmd.s_deserializeDataDelayedAction);
      return machineBoostToggleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MachineBoostToggleCmd>(this, "MachineId", (object) EntityId.Deserialize(reader));
    }

    static MachineBoostToggleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MachineBoostToggleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      MachineBoostToggleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
