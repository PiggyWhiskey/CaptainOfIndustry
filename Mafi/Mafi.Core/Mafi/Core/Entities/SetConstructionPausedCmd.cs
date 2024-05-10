// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.SetConstructionPausedCmd
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
namespace Mafi.Core.Entities
{
  [GenerateSerializer(false, null, 0)]
  public class SetConstructionPausedCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly bool? IsPaused;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetConstructionPausedCmd(IStaticEntity entity, bool? isPaused)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, isPaused);
    }

    public SetConstructionPausedCmd(EntityId entityId, bool? isPaused)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.IsPaused = isPaused;
    }

    public static void Serialize(SetConstructionPausedCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetConstructionPausedCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetConstructionPausedCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteNullableStruct<bool>(this.IsPaused);
    }

    public static SetConstructionPausedCmd Deserialize(BlobReader reader)
    {
      SetConstructionPausedCmd constructionPausedCmd;
      if (reader.TryStartClassDeserialization<SetConstructionPausedCmd>(out constructionPausedCmd))
        reader.EnqueueDataDeserialization((object) constructionPausedCmd, SetConstructionPausedCmd.s_deserializeDataDelayedAction);
      return constructionPausedCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetConstructionPausedCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetConstructionPausedCmd>(this, "IsPaused", (object) reader.ReadNullableStruct<bool>());
    }

    static SetConstructionPausedCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetConstructionPausedCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetConstructionPausedCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
