// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.ToggleConstructionPriorityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Entities.Static.Commands
{
  [GenerateSerializer(false, null, 0)]
  public class ToggleConstructionPriorityCmd : InputCommand
  {
    public readonly EntityId EntityId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ToggleConstructionPriorityCmd(IStaticEntity entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id);
    }

    public ToggleConstructionPriorityCmd(EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(entityId.IsValid).IsTrue();
      this.EntityId = entityId;
    }

    public static void Serialize(ToggleConstructionPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleConstructionPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleConstructionPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static ToggleConstructionPriorityCmd Deserialize(BlobReader reader)
    {
      ToggleConstructionPriorityCmd constructionPriorityCmd;
      if (reader.TryStartClassDeserialization<ToggleConstructionPriorityCmd>(out constructionPriorityCmd))
        reader.EnqueueDataDeserialization((object) constructionPriorityCmd, ToggleConstructionPriorityCmd.s_deserializeDataDelayedAction);
      return constructionPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleConstructionPriorityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    static ToggleConstructionPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleConstructionPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleConstructionPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
