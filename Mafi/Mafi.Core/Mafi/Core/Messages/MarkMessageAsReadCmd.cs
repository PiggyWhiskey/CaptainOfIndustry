// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MarkMessageAsReadCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class MarkMessageAsReadCmd : InputCommand
  {
    public Proto.ID ProtoId;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MarkMessageAsReadCmd(Proto.ID protoId)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = protoId;
    }

    public static void Serialize(MarkMessageAsReadCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MarkMessageAsReadCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MarkMessageAsReadCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Proto.ID.Serialize(this.ProtoId, writer);
    }

    public static MarkMessageAsReadCmd Deserialize(BlobReader reader)
    {
      MarkMessageAsReadCmd messageAsReadCmd;
      if (reader.TryStartClassDeserialization<MarkMessageAsReadCmd>(out messageAsReadCmd))
        reader.EnqueueDataDeserialization((object) messageAsReadCmd, MarkMessageAsReadCmd.s_deserializeDataDelayedAction);
      return messageAsReadCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      this.ProtoId = Proto.ID.Deserialize(reader);
    }

    static MarkMessageAsReadCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MarkMessageAsReadCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      MarkMessageAsReadCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
