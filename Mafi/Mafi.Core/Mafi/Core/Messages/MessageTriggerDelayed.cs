// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.MessageTriggerDelayed
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
  public class MessageTriggerDelayed : MessageTrigger
  {
    private readonly ICalendar m_calendar;
    private int m_daysRemaining;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageTriggerDelayed(
      MessageTriggerDelayedProto triggerProto,
      ISimLoopEvents simLoopEvents,
      MessagesManager messageManager,
      ICalendar calendar)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector((MessageTriggerProto) triggerProto, simLoopEvents, messageManager);
      this.m_calendar = calendar;
      calendar.NewDay.Add<MessageTriggerDelayed>(this, new Action(this.onNewDay));
      this.m_daysRemaining = triggerProto.DelayDays;
      if (triggerProto.DelayDays > 0)
        return;
      this.DeliverMessage();
    }

    private void onNewDay()
    {
      --this.m_daysRemaining;
      if (this.m_daysRemaining > 0)
        return;
      this.DeliverMessage();
    }

    protected override void OnDestroy()
    {
      this.m_calendar.NewDay.Remove<MessageTriggerDelayed>(this, new Action(this.onNewDay));
    }

    public static void Serialize(MessageTriggerDelayed value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<MessageTriggerDelayed>(value))
        return;
      writer.EnqueueDataSerialization((object) value, MessageTriggerDelayed.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      writer.WriteInt(this.m_daysRemaining);
    }

    public static MessageTriggerDelayed Deserialize(BlobReader reader)
    {
      MessageTriggerDelayed messageTriggerDelayed;
      if (reader.TryStartClassDeserialization<MessageTriggerDelayed>(out messageTriggerDelayed))
        reader.EnqueueDataDeserialization((object) messageTriggerDelayed, MessageTriggerDelayed.s_deserializeDataDelayedAction);
      return messageTriggerDelayed;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<MessageTriggerDelayed>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      this.m_daysRemaining = reader.ReadInt();
    }

    static MessageTriggerDelayed()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      MessageTriggerDelayed.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((MessageTrigger) obj).SerializeData(writer));
      MessageTriggerDelayed.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((MessageTrigger) obj).DeserializeData(reader));
    }
  }
}
