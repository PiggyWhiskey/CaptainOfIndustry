// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.StartDeconstructionOfStaticEntityCmd
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
  public class StartDeconstructionOfStaticEntityCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly EntityRemoveReason RemoveReason;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public StartDeconstructionOfStaticEntityCmd(IStaticEntity entity, EntityRemoveReason reason)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, reason);
    }

    public StartDeconstructionOfStaticEntityCmd(EntityId entityId, EntityRemoveReason reason)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(entityId.IsValid).IsTrue();
      this.EntityId = entityId;
      this.RemoveReason = reason;
    }

    public static void Serialize(StartDeconstructionOfStaticEntityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<StartDeconstructionOfStaticEntityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, StartDeconstructionOfStaticEntityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteInt((int) this.RemoveReason);
    }

    public static StartDeconstructionOfStaticEntityCmd Deserialize(BlobReader reader)
    {
      StartDeconstructionOfStaticEntityCmd ofStaticEntityCmd;
      if (reader.TryStartClassDeserialization<StartDeconstructionOfStaticEntityCmd>(out ofStaticEntityCmd))
        reader.EnqueueDataDeserialization((object) ofStaticEntityCmd, StartDeconstructionOfStaticEntityCmd.s_deserializeDataDelayedAction);
      return ofStaticEntityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<StartDeconstructionOfStaticEntityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<StartDeconstructionOfStaticEntityCmd>(this, "RemoveReason", (object) (EntityRemoveReason) reader.ReadInt());
    }

    static StartDeconstructionOfStaticEntityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      StartDeconstructionOfStaticEntityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      StartDeconstructionOfStaticEntityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
