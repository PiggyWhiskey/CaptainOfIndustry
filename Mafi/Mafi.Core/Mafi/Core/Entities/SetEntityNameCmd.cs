// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.SetEntityNameCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class SetEntityNameCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly string Title;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetEntityNameCmd(IEntity entity, string title)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, title);
    }

    public SetEntityNameCmd(EntityId entityId, string title)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.Title = title;
    }

    public static void Serialize(SetEntityNameCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetEntityNameCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetEntityNameCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteString(this.Title);
    }

    public static SetEntityNameCmd Deserialize(BlobReader reader)
    {
      SetEntityNameCmd setEntityNameCmd;
      if (reader.TryStartClassDeserialization<SetEntityNameCmd>(out setEntityNameCmd))
        reader.EnqueueDataDeserialization((object) setEntityNameCmd, SetEntityNameCmd.s_deserializeDataDelayedAction);
      return setEntityNameCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetEntityNameCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetEntityNameCmd>(this, "Title", (object) reader.ReadString());
    }

    static SetEntityNameCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetEntityNameCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetEntityNameCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
