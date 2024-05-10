// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Commands.EntityDisableLogisticsToggleCmd
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
  public class EntityDisableLogisticsToggleCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly bool IsInput;
    public readonly EntityLogisticsMode Mode;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public EntityDisableLogisticsToggleCmd(
      IEntityWithLogisticsControl entity,
      bool isInput,
      EntityLogisticsMode mode)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, isInput, mode);
    }

    public EntityDisableLogisticsToggleCmd(
      EntityId entityId,
      bool isInput,
      EntityLogisticsMode mode)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.IsInput = isInput;
      this.Mode = mode;
    }

    public static void Serialize(EntityDisableLogisticsToggleCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<EntityDisableLogisticsToggleCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, EntityDisableLogisticsToggleCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteBool(this.IsInput);
      writer.WriteInt((int) this.Mode);
    }

    public static EntityDisableLogisticsToggleCmd Deserialize(BlobReader reader)
    {
      EntityDisableLogisticsToggleCmd logisticsToggleCmd;
      if (reader.TryStartClassDeserialization<EntityDisableLogisticsToggleCmd>(out logisticsToggleCmd))
        reader.EnqueueDataDeserialization((object) logisticsToggleCmd, EntityDisableLogisticsToggleCmd.s_deserializeDataDelayedAction);
      return logisticsToggleCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<EntityDisableLogisticsToggleCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<EntityDisableLogisticsToggleCmd>(this, "IsInput", (object) reader.ReadBool());
      reader.SetField<EntityDisableLogisticsToggleCmd>(this, "Mode", (object) (EntityLogisticsMode) reader.ReadInt());
    }

    static EntityDisableLogisticsToggleCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EntityDisableLogisticsToggleCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      EntityDisableLogisticsToggleCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
