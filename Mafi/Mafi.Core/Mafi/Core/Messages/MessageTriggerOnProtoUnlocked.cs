// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnProtoUnlocked
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class MessageTriggerOnProtoUnlocked : MessageTrigger
  {
    private readonly MessageTriggerOnProtoUnlockedProto m_proto;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageTriggerOnProtoUnlocked(
      MessageTriggerOnProtoUnlockedProto triggerProto,
      ISimLoopEvents simLoopEvents,
      MessagesManager messageManager,
      UnlockedProtosDb unlockedProtosDb)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
      this.m_proto = triggerProto;
      this.m_unlockedProtosDb = unlockedProtosDb;
      unlockedProtosDb.OnUnlockedSetChanged.Add<MessageTriggerOnProtoUnlocked>(this, new Action(this.onProtoUnlocked));
    }

    private void onProtoUnlocked()
    {
      if (this.m_proto.IsObsolete)
      {
        this.OnDestroy();
      }
      else
      {
        if (!this.m_unlockedProtosDb.IsUnlocked(this.m_proto.UnlockedProto))
          return;
        this.DeliverMessage(10.Seconds());
      }
    }

    protected override void OnDestroy()
    {
      this.m_unlockedProtosDb.OnUnlockedSetChanged.Remove<MessageTriggerOnProtoUnlocked>(this, new Action(this.onProtoUnlocked));
    }

    public static void Serialize(MessageTriggerOnProtoUnlocked value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageTriggerOnProtoUnlocked>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageTriggerOnProtoUnlocked.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<MessageTriggerOnProtoUnlockedProto>(this.m_proto);
      UnlockedProtosDb.Serialize(this.m_unlockedProtosDb, writer);
    }

    public static MessageTriggerOnProtoUnlocked Deserialize(BlobReader reader)
    {
      MessageTriggerOnProtoUnlocked triggerOnProtoUnlocked;
      if (reader.TryStartClassDeserialization<MessageTriggerOnProtoUnlocked>(out triggerOnProtoUnlocked))
        reader.EnqueueDataDeserialization((object) triggerOnProtoUnlocked, MessageTriggerOnProtoUnlocked.s_deserializeDataDelayedAction);
      return triggerOnProtoUnlocked;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MessageTriggerOnProtoUnlocked>(this, "m_proto", (object) reader.ReadGenericAs<MessageTriggerOnProtoUnlockedProto>());
      reader.SetField<MessageTriggerOnProtoUnlocked>(this, "m_unlockedProtosDb", (object) UnlockedProtosDb.Deserialize(reader));
    }

    static MessageTriggerOnProtoUnlocked()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageTriggerOnProtoUnlocked.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
      MessageTriggerOnProtoUnlocked.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
    }
  }
}
