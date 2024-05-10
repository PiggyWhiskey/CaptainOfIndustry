// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Commands.SetCustomPriorityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class SetCustomPriorityCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly int Priority;
    public readonly string PriorityId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetCustomPriorityCmd(IEntityWithCustomPriority entity, string priorityId, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, priorityId, priority);
    }

    public SetCustomPriorityCmd(EntityId entityId, string priorityId, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.Priority = priority;
      this.PriorityId = priorityId;
    }

    public static void Serialize(SetCustomPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetCustomPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetCustomPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteInt(this.Priority);
      writer.WriteString(this.PriorityId);
    }

    public static SetCustomPriorityCmd Deserialize(BlobReader reader)
    {
      SetCustomPriorityCmd customPriorityCmd;
      if (reader.TryStartClassDeserialization<SetCustomPriorityCmd>(out customPriorityCmd))
        reader.EnqueueDataDeserialization((object) customPriorityCmd, SetCustomPriorityCmd.s_deserializeDataDelayedAction);
      return customPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetCustomPriorityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetCustomPriorityCmd>(this, "Priority", (object) reader.ReadInt());
      reader.SetField<SetCustomPriorityCmd>(this, "PriorityId", (object) reader.ReadString());
    }

    static SetCustomPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetCustomPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetCustomPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
