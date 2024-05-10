// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ElectricPower.SetElectricityGenerationPriorityCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.ElectricPower
{
  [GenerateSerializer(false, null, 0)]
  public class SetElectricityGenerationPriorityCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly int Priority;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetElectricityGenerationPriorityCmd(IElectricityGeneratingEntity entity, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, priority);
    }

    public SetElectricityGenerationPriorityCmd(EntityId entityId, int priority)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.Priority = priority;
    }

    public static void Serialize(SetElectricityGenerationPriorityCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetElectricityGenerationPriorityCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetElectricityGenerationPriorityCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteInt(this.Priority);
    }

    public static SetElectricityGenerationPriorityCmd Deserialize(BlobReader reader)
    {
      SetElectricityGenerationPriorityCmd generationPriorityCmd;
      if (reader.TryStartClassDeserialization<SetElectricityGenerationPriorityCmd>(out generationPriorityCmd))
        reader.EnqueueDataDeserialization((object) generationPriorityCmd, SetElectricityGenerationPriorityCmd.s_deserializeDataDelayedAction);
      return generationPriorityCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetElectricityGenerationPriorityCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetElectricityGenerationPriorityCmd>(this, "Priority", (object) reader.ReadInt());
    }

    static SetElectricityGenerationPriorityCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetElectricityGenerationPriorityCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetElectricityGenerationPriorityCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
