// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.GoalsListTriggerOnMessageDelivered
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  /// <summary>Trigger a goal list on a message delivery.</summary>
  [GenerateSerializer(false, null, 0)]
  public class GoalsListTriggerOnMessageDelivered : GoalListTrigger
  {
    private readonly MessagesManager m_messagesManager;
    private readonly Mafi.Core.Prototypes.Proto.ID m_messageIdToWaitFor;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public GoalsListTriggerOnMessageDelivered(
      GoalsListTriggerOnMessageDelivered.Data triggerData,
      GoalsList goalListToActivate,
      GoalsManager goalsManager,
      MessagesManager messagesManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(goalListToActivate, goalsManager, triggerData.Version);
      this.m_messagesManager = messagesManager;
      this.m_messageIdToWaitFor = triggerData.MessageProtoId;
      this.m_messagesManager.OnNewMessage.Add<GoalsListTriggerOnMessageDelivered>(this, new Action<Message>(this.onMessageAdded));
    }

    private void onMessageAdded(Message message)
    {
      if (!(message.Proto.Id == this.m_messageIdToWaitFor))
        return;
      this.ActivateGoal();
    }

    protected override void OnDestroy()
    {
      this.m_messagesManager.OnNewMessage.Remove<GoalsListTriggerOnMessageDelivered>(this, new Action<Message>(this.onMessageAdded));
    }

    public static void Serialize(GoalsListTriggerOnMessageDelivered value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<GoalsListTriggerOnMessageDelivered>(value))
        return;
      writer.EnqueueDataSerialization((object) value, GoalsListTriggerOnMessageDelivered.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      Mafi.Core.Prototypes.Proto.ID.Serialize(this.m_messageIdToWaitFor, writer);
      MessagesManager.Serialize(this.m_messagesManager, writer);
    }

    public static GoalsListTriggerOnMessageDelivered Deserialize(BlobReader reader)
    {
      GoalsListTriggerOnMessageDelivered messageDelivered;
      if (reader.TryStartClassDeserialization<GoalsListTriggerOnMessageDelivered>(out messageDelivered))
        reader.EnqueueDataDeserialization((object) messageDelivered, GoalsListTriggerOnMessageDelivered.s_deserializeDataDelayedAction);
      return messageDelivered;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<GoalsListTriggerOnMessageDelivered>(this, "m_messageIdToWaitFor", (object) Mafi.Core.Prototypes.Proto.ID.Deserialize(reader));
      reader.SetField<GoalsListTriggerOnMessageDelivered>(this, "m_messagesManager", (object) MessagesManager.Deserialize(reader));
    }

    static GoalsListTriggerOnMessageDelivered()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      GoalsListTriggerOnMessageDelivered.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((GoalListTrigger) obj).SerializeData(writer));
      GoalsListTriggerOnMessageDelivered.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((GoalListTrigger) obj).DeserializeData(reader));
    }

    public class Data : IGoalListTriggerData
    {
      public readonly Mafi.Core.Prototypes.Proto.ID MessageProtoId;

      public Type Implementation => typeof (GoalsListTriggerOnMessageDelivered);

      public int Version { get; }

      public Data(Mafi.Core.Prototypes.Proto.ID messageProtoId, int version = 0)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        this.MessageProtoId = messageProtoId;
        this.Version = version;
      }
    }
  }
}
