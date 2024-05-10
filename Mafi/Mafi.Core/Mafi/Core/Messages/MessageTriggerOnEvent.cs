// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerOnEvent
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Simulation;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages
{
  [GenerateSerializer(false, null, 0)]
  public class MessageTriggerOnEvent : MessageTrigger
  {
    private readonly Duration m_delay;
    private readonly IEvent m_event;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageTriggerOnEvent(
      MessageTriggerOnEventProto triggerProto,
      IResolver resolver,
      ISimLoopEvents simLoopEvents,
      MessagesManager messageManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
      this.m_event = triggerProto.GetEvent(resolver);
      this.m_event.Add<MessageTriggerOnEvent>(this, new Action(this.onEvent));
      this.m_delay = triggerProto.DelayDays.Days();
    }

    protected override void OnDestroy()
    {
      this.m_event.Remove<MessageTriggerOnEvent>(this, new Action(this.onEvent));
    }

    private void onEvent() => this.DeliverMessage(this.m_delay);

    public static void Serialize(MessageTriggerOnEvent value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageTriggerOnEvent>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageTriggerOnEvent.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Duration.Serialize(this.m_delay, writer);
      writer.WriteGeneric<IEvent>(this.m_event);
    }

    public static MessageTriggerOnEvent Deserialize(BlobReader reader)
    {
      MessageTriggerOnEvent messageTriggerOnEvent;
      if (reader.TryStartClassDeserialization<MessageTriggerOnEvent>(out messageTriggerOnEvent))
        reader.EnqueueDataDeserialization((object) messageTriggerOnEvent, MessageTriggerOnEvent.s_deserializeDataDelayedAction);
      return messageTriggerOnEvent;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MessageTriggerOnEvent>(this, "m_delay", (object) Duration.Deserialize(reader));
      reader.SetField<MessageTriggerOnEvent>(this, "m_event", (object) reader.ReadGenericAs<IEvent>());
    }

    static MessageTriggerOnEvent()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageTriggerOnEvent.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
      MessageTriggerOnEvent.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
    }
  }
}
