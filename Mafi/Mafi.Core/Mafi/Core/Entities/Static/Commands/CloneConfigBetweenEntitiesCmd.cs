// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.CloneConfigBetweenEntitiesCmd
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
  public class CloneConfigBetweenEntitiesCmd : InputCommand
  {
    public readonly EntityId SourceEntityId;
    public readonly EntityId TargetEntityId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public CloneConfigBetweenEntitiesCmd(IEntity sourceEntity, IEntity targetEntity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(sourceEntity.Id, targetEntity.Id);
    }

    public CloneConfigBetweenEntitiesCmd(EntityId sourceEntityId, EntityId targetEntityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(sourceEntityId.IsValid).IsTrue();
      Assert.That<bool>(targetEntityId.IsValid).IsTrue();
      this.SourceEntityId = sourceEntityId;
      this.TargetEntityId = targetEntityId;
    }

    public static void Serialize(CloneConfigBetweenEntitiesCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<CloneConfigBetweenEntitiesCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, CloneConfigBetweenEntitiesCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.SourceEntityId, writer);
      EntityId.Serialize(this.TargetEntityId, writer);
    }

    public static CloneConfigBetweenEntitiesCmd Deserialize(BlobReader reader)
    {
      CloneConfigBetweenEntitiesCmd betweenEntitiesCmd;
      if (reader.TryStartClassDeserialization<CloneConfigBetweenEntitiesCmd>(out betweenEntitiesCmd))
        reader.EnqueueDataDeserialization((object) betweenEntitiesCmd, CloneConfigBetweenEntitiesCmd.s_deserializeDataDelayedAction);
      return betweenEntitiesCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<CloneConfigBetweenEntitiesCmd>(this, "SourceEntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<CloneConfigBetweenEntitiesCmd>(this, "TargetEntityId", (object) EntityId.Deserialize(reader));
    }

    static CloneConfigBetweenEntitiesCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      CloneConfigBetweenEntitiesCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      CloneConfigBetweenEntitiesCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
