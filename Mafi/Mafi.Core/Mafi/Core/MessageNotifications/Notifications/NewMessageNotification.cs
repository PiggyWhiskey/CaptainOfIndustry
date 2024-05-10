// Decompiled with JetBrains decompiler
// Type: Mafi.Core.MessageNotifications.Notifications.NewMessageNotification
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Messages;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.MessageNotifications.Notifications
{
  [GenerateSerializer(false, null, 0)]
  public class NewMessageNotification : IMessageNotification
  {
    public readonly Message Message;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public MessageNotificationId NotificationId { get; set; }

    public NewMessageNotification(Message message)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Message = message;
    }

    public static void Serialize(NewMessageNotification value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<NewMessageNotification>(value))
        return;
      writer.EnqueueDataSerialization((object) value, NewMessageNotification.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Message.Serialize(this.Message, writer);
      MessageNotificationId.Serialize(this.NotificationId, writer);
    }

    public static NewMessageNotification Deserialize(BlobReader reader)
    {
      NewMessageNotification messageNotification;
      if (reader.TryStartClassDeserialization<NewMessageNotification>(out messageNotification))
        reader.EnqueueDataDeserialization((object) messageNotification, NewMessageNotification.s_deserializeDataDelayedAction);
      return messageNotification;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<NewMessageNotification>(this, "Message", (object) Message.Deserialize(reader));
      this.NotificationId = MessageNotificationId.Deserialize(reader);
    }

    static NewMessageNotification()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      NewMessageNotification.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((NewMessageNotification) obj).SerializeData(writer));
      NewMessageNotification.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((NewMessageNotification) obj).DeserializeData(reader));
    }
  }
}
