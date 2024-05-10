// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Priorities.SetGlobalPriorityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Priorities
{
  [GenerateSerializer(false, null, 0)]
  public class SetGlobalPriorityCmd : InputCommand
  {
    public readonly string PriorityId;
    public readonly int Priority;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetGlobalPriorityCmd(string priorityId, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.PriorityId = priorityId;
      this.Priority = priority;
    }

    public static void Serialize(SetGlobalPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetGlobalPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetGlobalPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteInt(this.Priority);
      writer.WriteString(this.PriorityId);
    }

    public static SetGlobalPriorityCmd Deserialize(BlobReader reader)
    {
      SetGlobalPriorityCmd globalPriorityCmd;
      if (reader.TryStartClassDeserialization<SetGlobalPriorityCmd>(out globalPriorityCmd))
        reader.EnqueueDataDeserialization((object) globalPriorityCmd, SetGlobalPriorityCmd.s_deserializeDataDelayedAction);
      return globalPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetGlobalPriorityCmd>(this, "Priority", (object) reader.ReadInt());
      reader.SetField<SetGlobalPriorityCmd>(this, "PriorityId", (object) reader.ReadString());
    }

    static SetGlobalPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetGlobalPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetGlobalPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
