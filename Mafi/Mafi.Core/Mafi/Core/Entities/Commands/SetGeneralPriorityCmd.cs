// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Commands.SetGeneralPriorityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class SetGeneralPriorityCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly int Priority;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetGeneralPriorityCmd(IEntity entity, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, priority);
    }

    public SetGeneralPriorityCmd(EntityId entityId, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.Priority = priority;
    }

    public static void Serialize(SetGeneralPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetGeneralPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetGeneralPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteInt(this.Priority);
    }

    public static SetGeneralPriorityCmd Deserialize(BlobReader reader)
    {
      SetGeneralPriorityCmd generalPriorityCmd;
      if (reader.TryStartClassDeserialization<SetGeneralPriorityCmd>(out generalPriorityCmd))
        reader.EnqueueDataDeserialization((object) generalPriorityCmd, SetGeneralPriorityCmd.s_deserializeDataDelayedAction);
      return generalPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetGeneralPriorityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetGeneralPriorityCmd>(this, "Priority", (object) reader.ReadInt());
    }

    static SetGeneralPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetGeneralPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetGeneralPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
