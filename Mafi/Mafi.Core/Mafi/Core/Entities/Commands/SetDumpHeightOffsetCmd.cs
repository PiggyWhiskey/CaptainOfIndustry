// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Commands.SetDumpHeightOffsetCmd
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
  public class SetDumpHeightOffsetCmd : InputCommand
  {
    public readonly EntityId EntityId;
    public readonly ThicknessTilesI Depth;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public SetDumpHeightOffsetCmd(IEntity entity, ThicknessTilesI depth)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(entity.Id, depth);
    }

    public SetDumpHeightOffsetCmd(EntityId entityId, ThicknessTilesI depth)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.EntityId = entityId;
      this.Depth = depth;
    }

    public static void Serialize(SetDumpHeightOffsetCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<SetDumpHeightOffsetCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, SetDumpHeightOffsetCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ThicknessTilesI.Serialize(this.Depth, writer);
      EntityId.Serialize(this.EntityId, writer);
    }

    public static SetDumpHeightOffsetCmd Deserialize(BlobReader reader)
    {
      SetDumpHeightOffsetCmd dumpHeightOffsetCmd;
      if (reader.TryStartClassDeserialization<SetDumpHeightOffsetCmd>(out dumpHeightOffsetCmd))
        reader.EnqueueDataDeserialization((object) dumpHeightOffsetCmd, SetDumpHeightOffsetCmd.s_deserializeDataDelayedAction);
      return dumpHeightOffsetCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<SetDumpHeightOffsetCmd>(this, "Depth", (object) ThicknessTilesI.Deserialize(reader));
      reader.SetField<SetDumpHeightOffsetCmd>(this, "EntityId", (object) EntityId.Deserialize(reader));
    }

    static SetDumpHeightOffsetCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      SetDumpHeightOffsetCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      SetDumpHeightOffsetCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
