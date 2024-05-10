// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Research.ResearchQueueDequeueCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Research
{
  [GenerateSerializer(false, null, 0)]
  public class ResearchQueueDequeueCmd : InputCommand
  {
    public readonly ResearchNodeProto.ID NodeId;
    public readonly bool IsEnqueue;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public ResearchQueueDequeueCmd(ResearchNodeProto node, bool isEnqueue)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      this.\u002Ector(node.Id, isEnqueue);
    }

    public ResearchQueueDequeueCmd(ResearchNodeProto.ID nodeId, bool isEnqueue)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.NodeId = nodeId;
      this.IsEnqueue = isEnqueue;
    }

    public static void Serialize(ResearchQueueDequeueCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResearchQueueDequeueCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResearchQueueDequeueCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteBool(this.IsEnqueue);
      ResearchNodeProto.ID.Serialize(this.NodeId, writer);
    }

    public static ResearchQueueDequeueCmd Deserialize(BlobReader reader)
    {
      ResearchQueueDequeueCmd researchQueueDequeueCmd;
      if (reader.TryStartClassDeserialization<ResearchQueueDequeueCmd>(out researchQueueDequeueCmd))
        reader.EnqueueDataDeserialization((object) researchQueueDequeueCmd, ResearchQueueDequeueCmd.s_deserializeDataDelayedAction);
      return researchQueueDequeueCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ResearchQueueDequeueCmd>(this, "IsEnqueue", (object) reader.ReadBool());
      reader.SetField<ResearchQueueDequeueCmd>(this, "NodeId", (object) ResearchNodeProto.ID.Deserialize(reader));
    }

    static ResearchQueueDequeueCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResearchQueueDequeueCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ResearchQueueDequeueCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
