// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.SetIsElectricitySurplusConsumerCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  [GenerateSerializer(false, null, 0)]
  public class SetIsElectricitySurplusConsumerCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly bool IsSurplusConsumer;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetIsElectricitySurplusConsumerCmd(IEntity entity, bool isSurplusConsumer)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, isSurplusConsumer);
    }

    public SetIsElectricitySurplusConsumerCmd(EntityId entityId, bool isSurplusConsumer)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.IsSurplusConsumer = isSurplusConsumer;
    }

    public static void Serialize(SetIsElectricitySurplusConsumerCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetIsElectricitySurplusConsumerCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetIsElectricitySurplusConsumerCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteBool(this.IsSurplusConsumer);
    }

    public static SetIsElectricitySurplusConsumerCmd Deserialize(BlobReader reader)
    {
      SetIsElectricitySurplusConsumerCmd surplusConsumerCmd;
      if (reader.TryStartClassDeserialization<SetIsElectricitySurplusConsumerCmd>(out surplusConsumerCmd))
        reader.EnqueueDataDeserialization((object) surplusConsumerCmd, SetIsElectricitySurplusConsumerCmd.s_deserializeDataDelayedAction);
      return surplusConsumerCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetIsElectricitySurplusConsumerCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetIsElectricitySurplusConsumerCmd>(this, "IsSurplusConsumer", (object) reader.ReadBool());
    }

    static SetIsElectricitySurplusConsumerCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetIsElectricitySurplusConsumerCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetIsElectricitySurplusConsumerCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
