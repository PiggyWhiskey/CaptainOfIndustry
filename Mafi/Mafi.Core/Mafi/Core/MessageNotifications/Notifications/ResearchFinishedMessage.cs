// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.Notifications.ResearchFinishedMessage
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.MessageNotifications.Notifications
{
  [GenerateSerializer(false, null, 0)]
  public class ResearchFinishedMessage : IMessageNotification
  {
    public readonly ResearchNode ResearchNode;
    public readonly Set<Proto> UnlockedProtos;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageNotificationId NotificationId { get; set; }

    public ResearchFinishedMessage(ResearchNode researchNode, Set<Proto> unlockedProtos)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ResearchNode = researchNode;
      this.UnlockedProtos = unlockedProtos;
    }

    public static void Serialize(ResearchFinishedMessage value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResearchFinishedMessage>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResearchFinishedMessage.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      MessageNotificationId.Serialize(this.NotificationId, writer);
      ResearchNode.Serialize(this.ResearchNode, writer);
      Set<Proto>.Serialize(this.UnlockedProtos, writer);
    }

    public static ResearchFinishedMessage Deserialize(BlobReader reader)
    {
      ResearchFinishedMessage researchFinishedMessage;
      if (reader.TryStartClassDeserialization<ResearchFinishedMessage>(out researchFinishedMessage))
        reader.EnqueueDataDeserialization((object) researchFinishedMessage, ResearchFinishedMessage.s_deserializeDataDelayedAction);
      return researchFinishedMessage;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.NotificationId = MessageNotificationId.Deserialize(reader);
      reader.SetField<ResearchFinishedMessage>(this, "ResearchNode", (object) ResearchNode.Deserialize(reader));
      reader.SetField<ResearchFinishedMessage>(this, "UnlockedProtos", (object) Set<Proto>.Deserialize(reader));
    }

    static ResearchFinishedMessage()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResearchFinishedMessage.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((ResearchFinishedMessage) obj).SerializeData(writer));
      ResearchFinishedMessage.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((ResearchFinishedMessage) obj).DeserializeData(reader));
    }
  }
}
