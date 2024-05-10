// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Commands.SetConstructionPriorityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class SetConstructionPriorityCmd : InputCommand<EntityId>
  {
    public readonly EntityId EntityId;
    public readonly int Priority;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetConstructionPriorityCmd(IStaticEntity entity, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, priority);
    }

    public SetConstructionPriorityCmd(EntityId entityId, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.Priority = priority;
    }

    public static void Serialize(SetConstructionPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetConstructionPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetConstructionPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteInt(this.Priority);
    }

    public static SetConstructionPriorityCmd Deserialize(BlobReader reader)
    {
      SetConstructionPriorityCmd constructionPriorityCmd;
      if (reader.TryStartClassDeserialization<SetConstructionPriorityCmd>(out constructionPriorityCmd))
        reader.EnqueueDataDeserialization((object) constructionPriorityCmd, SetConstructionPriorityCmd.s_deserializeDataDelayedAction);
      return constructionPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetConstructionPriorityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetConstructionPriorityCmd>(this, "Priority", (object) reader.ReadInt());
    }

    static SetConstructionPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetConstructionPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<EntityId>) obj).SerializeData(writer));
      SetConstructionPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<EntityId>) obj).DeserializeData(reader));
    }
  }
}
