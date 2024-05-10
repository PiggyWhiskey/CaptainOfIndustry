// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.SetEntityEnabledCmd
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
  public class SetEntityEnabledCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly bool IsEnabled;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetEntityEnabledCmd(IEntity entity, bool isEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, isEnabled);
    }

    public SetEntityEnabledCmd(EntityId entityId, bool isEnabled)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.IsEnabled = isEnabled;
    }

    public static void Serialize(SetEntityEnabledCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetEntityEnabledCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetEntityEnabledCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      EntityId.Serialize(this.EntityId, writer);
      writer.WriteBool(this.IsEnabled);
    }

    public static SetEntityEnabledCmd Deserialize(BlobReader reader)
    {
      SetEntityEnabledCmd entityEnabledCmd;
      if (reader.TryStartClassDeserialization<SetEntityEnabledCmd>(out entityEnabledCmd))
        reader.EnqueueDataDeserialization((object) entityEnabledCmd, SetEntityEnabledCmd.s_deserializeDataDelayedAction);
      return entityEnabledCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetEntityEnabledCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
      reader.SetField<SetEntityEnabledCmd>(this, "IsEnabled", (object) reader.ReadBool());
    }

    static SetEntityEnabledCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetEntityEnabledCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetEntityEnabledCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
