// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Commands.ToggleStaticEntityConstructionCmd
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
  public class ToggleStaticEntityConstructionCmd : InputCommand
  {
    public readonly EntityId EntityId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ToggleStaticEntityConstructionCmd(IStaticEntity entity)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id);
    }

    public ToggleStaticEntityConstructionCmd(EntityId entityId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      Assert.That<bool>(entityId.IsValid).IsTrue();
      this.EntityId = entityId;
    }

    public static void Serialize(ToggleStaticEntityConstructionCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ToggleStaticEntityConstructionCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ToggleStaticEntityConstructionCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static ToggleStaticEntityConstructionCmd Deserialize(BlobReader reader)
    {
      ToggleStaticEntityConstructionCmd entityConstructionCmd;
      if (reader.TryStartClassDeserialization<ToggleStaticEntityConstructionCmd>(out entityConstructionCmd))
        reader.EnqueueDataDeserialization((object) entityConstructionCmd, ToggleStaticEntityConstructionCmd.s_deserializeDataDelayedAction);
      return entityConstructionCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ToggleStaticEntityConstructionCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    static ToggleStaticEntityConstructionCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ToggleStaticEntityConstructionCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ToggleStaticEntityConstructionCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
